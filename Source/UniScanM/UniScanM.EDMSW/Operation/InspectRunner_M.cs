using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using System.IO;
//using UniScanM.Data;
using System.Threading;
using UniEye.Base.Device;
using UniScanM.Algorithm;
using DynMvp.Devices.Light;
using DynMvp.Devices.FrameGrabber;
using DynMvp.UI.Touch;
using UniEye.Base;
using DynMvp.InspData;
using UniEye.Base.Inspect;
using UniEye.Base.MachineInterface;
using UniScanM.MachineIF;
using DynMvp.Data;
using UniScanM.EDMSW.State;
using UniScanM.EDMSW.Data;
using UniScanM.EDMSW.Algorithm;
using UniScanM.EDMSW.Settings;
using DynMvp.Device.Device.FrameGrabber;
using System.Drawing.Imaging;

namespace UniScanM.EDMSW.Operation
{
    public class InspectRunner_M : UniScanM.Operation.InspectRunner
    {
        IState m_StateHandler = null;
        bool resetZeroing;
        profileQ m_profileQ_Left = null;// new profileQ();
        profileQ m_profileQ_Right = null;// new profileQ();
        private struct LastGrabbedIamgeDevice
        {
            public ImageDevice imageDevice;
            public IntPtr ptr;
        }
        LastGrabbedIamgeDevice lastGrabbedIamgeDevice;
        float asyncGrabExpUs = -1;
        ThreadHandler runningThreadHandler = null;  // 검사시 While 돌릴 쓰레드.

        public float AsyncGrabExpUs
        {
            get { return asyncGrabExpUs; }
            set { asyncGrabExpUs = value; }
        }
        
        public InspectRunner_M() : base()
        {
            lastGrabbedIamgeDevice = new LastGrabbedIamgeDevice();

            SystemManager.Instance().InspectStarter.OnStartInspection += EnterWaitInspection;
            SystemManager.Instance().InspectStarter.OnStopInspection += ExitWaitInspection;
            SystemManager.Instance().InspectStarter.OnRewinderCutting += OnRewinderCutting;
            SystemManager.Instance().InspectStarter.OnLotChanged += OnLotChanged;
        }

        public override void Dispose()
        {
            base.Dispose();

            //ExitWaitInspection();
        }

        protected override void ErrorManager_OnStartAlarm()
        {
            ExitWaitInspection();
        }

        public override bool EnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Function, "InspectRunner::EnterWaitInspection");
            if (SystemManager.Instance().CurrentModel == null)
                return false;

            //Model model = SystemManager.Instance().CurrentModel;
            if (SystemManager.Instance().CurrentModel.IsTaught() == false)
            {
                MessageForm.Show(null, "There is no data or teach state is invalid.");
                return false;
            }

            if (SystemManager.Instance().DeviceController.OnEnterWaitInspection() == false)
                return false;
            
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                imageDevice.ImageGrabbed += ImageGrabbed;
                imageDevice.SetExposureTime(50);
            }
            imageDeviceHandler.SetTriggerMode(TriggerMode.Software);

            SystemState.Instance().SetInspect();
            //SystemState.Instance().SetInspectState(InspectState.Run);

            int width = imageDeviceHandler[0].ImageSize.Width;
            int height = imageDeviceHandler[0].ImageSize.Height;
            m_profileQ_Left =  new profileQ(width, height, Settings.EDMSSettings.Instance().BufferingCount);
            m_profileQ_Right = new profileQ(width, height, Settings.EDMSSettings.Instance().BufferingCount);
            //
            SetCameraAcqusitionRate();
            //
            bool ok = PostEnterWaitInspection(); //조명켜기

            if (GrabStartConinuous() == false) //카메라켜기
            {
                Thread.Sleep(100);
                if (runningThreadHandler.RequestStop == false)
                {
                    ErrorManager.Instance().Report(ErrorCodeGrabber.Instance.Timeout, ErrorLevel.Error,
                        "Grab Timeout", null);
                    runningThreadHandler.RequestStop = true;
                }
                return false;
            }

            //if (ok) // 검사쓰레드 시작
            {
                m_StateHandler = new StateSkip();
                runningThreadHandler = new ThreadHandler("InspectRunner", new Thread(RunningThreadProc), false);
                runningThreadHandler.Start();
            }
            return ok;
        }

        private void SetCameraAcqusitionRate()
        {
            bool good = true;
            float lineSpd = (float)SystemManager.Instance().InspectStarter.GetLineSpeedSv();
            bool autoLight = Settings.EDMSSettings.Instance().AutoLight;
            if (lineSpd > 0)
            {
                ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

                SystemManager.Instance().DeviceBox.CameraCalibrationList.ForEach(f =>
                {
                    float hz = 80E3f;
                    //if (autoLight)
                    {
                        hz = lineSpd / (6 * f.PelSize.Height) * 1E5f; // (lineSpd * 1000 mm /60s) / (res /1000mm) 
                    }
                    //Max 172413.78125 BinningY, 7200, 2640, 67.2fps@2560y, => 103.4478m/min
                    if (imageDeviceHandler[f.CameraIndex] is CameraPylon) //Max Exp: 77.7us ,12.004Khz
                    {
                        hz = 12000;
                        imageDeviceHandler[f.CameraIndex].SetExposureTime(77.0f);
                    }
                    else if (imageDeviceHandler[f.CameraIndex] is CameraInfoGenTL)
                    {
                       // hz = 80E3f;
                    }
                    good &= imageDeviceHandler[f.CameraIndex].SetAcquisitionLineRate(hz);
                });
            }
            LightValue lightValue = (LightValue)SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0].LightValue;
            if (autoLight)
                lightValue = GetAutoLightValue(lineSpd);
        }

        public override bool PostEnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Grab, "InspectRunner::PostEnterWaitInspection");
            //double rollerDia = SystemManager.Instance().InspectStarter.GetRollerDia();
            float lineSpd = (float)SystemManager.Instance().InspectStarter.GetLineSpeedSv();

            LightValue lightValue = (LightValue)SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0].LightValue;

            var newvalue = new int[lightValue.NumLight];
            for (int i = 0; i < lightValue.NumLight; i++)
            {
                int lval = (int)(lightValue.Value[i] * lineSpd / 60.0f); //60m/min 속도를 기준으로 세팅한것을 가정하고, 비례적으로 조명값 조정
                lval = lval <= 255 ? lval : 255;
                newvalue[i] = lval;
            }

            LightValue caliblightValue = new LightValue(newvalue);
            LightCtrl lightCtrl = SystemManager.Instance().DeviceBox.LightCtrlHandler.GetLightCtrl(0);
            lightCtrl.TurnOn(caliblightValue);
            return true;
        }

        private LightValue GetAutoLightValue(float lineSpd)
        {
            // 80kHz일 때 조명값 50,150
            // spd => back, top
            // 5 => 50, 80
            // 10 => 55, 130
            // 20 => 60, 180
            // 40 => 65, 230
            // 80 => 70, 255
            int[] value = new int[2];
            if (lineSpd < 5)
            {
                value[0] = (int)Math.Round((lineSpd - 5) * 10 + 50);
                value[1] = (int)Math.Round((lineSpd - 5) * 10 + 80);
            }
            else
            {
                double x = Math.Log(lineSpd / 5, 2);
                value[0] = (int)Math.Min(255, Math.Round(5 * x + 50));
                value[1] = (int)Math.Min(255, Math.Round(50 * x + 80));
            }

            value[0] += Settings.EDMSSettings.Instance().AutoLightOffsetBottom;
            value[1] += Settings.EDMSSettings.Instance().AutoLightOffsetTop;

            return new LightValue(value);
        }

        public ImageD GetBinningImage(ImageD srcImage)
        {
            Image2D binningImage = new Image2D(srcImage.Width, srcImage.Height * 2, srcImage.NumBand);
            Image2D srcImage2D = srcImage as Image2D;
            if (srcImage2D.IsUseIntPtr())
                srcImage2D.ConvertFromPtr();

            int width = srcImage2D.Width;
            int height = srcImage2D.Height;

            int size = width * height * srcImage.NumBand;

            byte[] imageBuf = srcImage2D.Data;
            byte[] binningBuf = binningImage.Data;

            Parallel.For(0, height, y =>
            {
                Array.Copy(imageBuf, srcImage2D.Pitch * y, binningBuf, width * y * 2, width);
                Array.Copy(imageBuf, srcImage2D.Pitch * y, binningBuf, width * y * 2 + width, width);
            });

            return binningImage;
        }
        //*********************************************************************//todo

        protected override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            if (imageDevice.Index == 0) //Left
            {

                Task.Factory.StartNew(() =>
                {
                    Stopwatch timer = new Stopwatch();
                    lastGrabbedIamgeDevice.imageDevice = imageDevice;
                    lastGrabbedIamgeDevice.ptr = ptr;
                    AlgoImage algoImage = null;

                    timer.Start();
                    ImageD imageD = imageDevice.GetGrabbedImage(ptr);//not deep copy
                    algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);//not deep copy
                    m_profileQ_Left.AddImage(algoImage);
                    //algoImage.Save("d:\\Left.bmp");
                    timer.Stop();
                    Debug.WriteLine("★Left_" + timer.ElapsedMilliseconds.ToString() + "_" + DateTime.Now.ToString("HH:mm:ss:fff"));
                });
            }
            else //right
            {
                Task.Factory.StartNew(() =>
                {
                    Stopwatch timer = new Stopwatch();
                    lastGrabbedIamgeDevice.imageDevice = imageDevice;
                    lastGrabbedIamgeDevice.ptr = ptr;
                    AlgoImage algoImage = null;

                    timer.Start();
                    ImageD imageD = imageDevice.GetGrabbedImage(ptr);//not deep copy
                    algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);//not deep copy
                    m_profileQ_Right.AddImage(algoImage);
                    //algoImage.Save("d:\\Right.bmp");
                    timer.Stop();
                    Debug.WriteLine("★Right_" + timer.ElapsedMilliseconds.ToString() + "_" + DateTime.Now.ToString("HH:mm:ss:fff"));
                });
            }
        }

        private void RunningThreadProc()
        {
            while (runningThreadHandler.RequestStop == false)
            {
                DynMvp.InspData.InspectionResult inspectionResult = BuildInspectionResult();//todo check  path..
                try
                {
                    Console.WriteLine(inspectionResult.Judgment);
                    Console.WriteLine(inspectionResult);
                    if (inspectionResult == null)
                        continue;
                    ImageDevice imageDevice = this.lastGrabbedIamgeDevice.imageDevice;
                    IntPtr ptr = this.lastGrabbedIamgeDevice.ptr;
                    //InspectionOption inspectionOption = new InspectionOption(imageDevice);

                    Stopwatch timer = new Stopwatch();
                    timer.Start();

                    if (imageDevice != null)
                        this.Inspect(imageDevice, ptr, inspectionResult);
                    
                    Debug.WriteLine("/// Inpect() take " + timer.ElapsedMilliseconds.ToString());
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Inspection, ex.Message);
                    inspectionResult.SetDefect();
                }
                finally
                {
                    int sleepTimeMs = (int)Math.Round(1000f / Settings.EDMSSettings.Instance().MaxMeasCountPerSec);
                    if (inspectionResult != null)
                        sleepTimeMs = (int)Math.Max(0, sleepTimeMs - inspectionResult.InspectionTime.TotalMilliseconds);
                    Debug.WriteLine("----------------->" + inspectionResult.InspectionTime.TotalMilliseconds.ToString());
                    if (sleepTimeMs < 250) sleepTimeMs = 250;
                    Thread.Sleep(sleepTimeMs);
                }
            }
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.Stop();
            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
        }
        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null)
        //public  void Inspect(DynMvp.InspData.InspectionResult inspectionResult, InspectionOption inspectionOption = null)//
        {
            LogHelper.Debug(LoggerType.Operation, "InspectRunner::Inspect");
            Debug.WriteLine("■Inspect__" + DateTime.Now.ToString("HH:mm:ss:fff"));
            AlgoImage dispAlgImgLeft = null;
            AlgoImage dispAlgImgRight = null;
            try
            {
                inspectRunnerExtender.OnPreInspection();
                Data.InspectionResult edmsResult = (Data.InspectionResult)inspectionResult;

                if (resetZeroing == true)
                {
                    edmsResult.ResetZeroing = true;
                    this.resetZeroing = false;
                    this.m_StateHandler = new StateZeroing();
                    this.m_profileQ_Left.Clear();
                    this.m_profileQ_Right.Clear();
                }

                dispAlgImgLeft = m_profileQ_Left.getDisplayImage();
                Debug.Assert(dispAlgImgLeft != null);

                dispAlgImgRight = m_profileQ_Right.getDisplayImage();
                Debug.Assert(dispAlgImgRight != null);


                bool isValidImageLeft=false;
                float avgIntensityLeft = 0.0f;

                bool isValidImageRight = false;
                float avgIntensityRight = 0.0f;

                ////Left -----------------------------------------------------------------------------------left //
                float [] profileLeftHor = m_profileQ_Left.GetProfileHorAverge(ref  isValidImageLeft, ref  avgIntensityLeft);
                edmsResult.ProfileLeftHor = profileLeftHor;
                edmsResult.FrameAvgIntensityLeft = avgIntensityLeft;
                float[] profileLeftVer = m_profileQ_Left.GetProfileVer();
                edmsResult.ProfileLeftVer = profileLeftVer;

                //Right
                float[] profileRightHor = m_profileQ_Right.GetProfileHorAverge(ref isValidImageRight, ref avgIntensityRight);
                edmsResult.ProfileRightHor = profileRightHor;
                edmsResult.FrameAvgIntensityRight = avgIntensityRight;
                float[] profileRightVer = m_profileQ_Right.GetProfileVer();
                edmsResult.ProfileRightVer = profileRightVer;

                // Q가 충분하고, 데이터 출력됨
                if ((profileLeftHor != null && profileRightHor !=null) &&
                    (profileLeftVer != null && profileRightVer != null) )
                {
                    ///////////////////////////////////////////
                    //Stopwatch timer = new Stopwatch();
                    //timer.Start();

                    if (isValidImageLeft == true && isValidImageRight == true)
                    {
                        IState _state = this.m_StateHandler?.Handling(edmsResult);
                        if (_state != null)  // 상태가 바뀜.
                        {
                            _state.Enter();
                            this.m_StateHandler = _state;
                        }
                    }
                    else
                    {
                        edmsResult.State = State_EDMS.Inspecting; 
                        edmsResult.Judgment = Judgment.Skip;
                    }
                        //timer.Stop();
                        //Debug.WriteLine("--------☆" + timer.ElapsedMilliseconds.ToString());
                        /////////////////////////////////////////////////////////////
                }//if (profile !=null ) // Q가 충분하고, 데이터 출력됨
                else
                {
                    edmsResult.State = State_EDMS.Waiting;
                    edmsResult.Judgment = Judgment.Skip;
                }
                //디스플레이用 이미지.
                ImageD displayImageLeft = dispAlgImgLeft.ToImageD(); //32ms ?? 뭐이래..DeepCopy 
                ImageD displayImageRight = dispAlgImgRight.ToImageD();

                //if (imageDevice.IsBinningVirtical()) //비닝이면 상하 늘리기..
                //{
                //    displayImageLeft = GetBinningImage(displayImageLeft);//8ms
                //    displayImageRight = GetBinningImage(displayImageRight);//8ms
                //}

                edmsResult.DisplayBitmapLeft = displayImageLeft.ToBitmap();
                edmsResult.DisplayBitmapRight = displayImageRight.ToBitmap();

                int width = displayImageLeft.Width / 10;
                int height = displayImageLeft.Height / 10;
                Bitmap resizeImageLeft = new Bitmap(edmsResult.DisplayBitmapLeft, new Size(width, height));
                Bitmap resizeImageRight = new Bitmap(edmsResult.DisplayBitmapRight, new Size(width, height));



                //이미지 합쳐서 하나로..
                width = resizeImageLeft.Width + resizeImageRight.Width;
                height = resizeImageLeft.Height;
                Bitmap Mergebitmap = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(Mergebitmap);
                g.DrawImage(resizeImageLeft, 0, 0);
                g.DrawImage(resizeImageRight, resizeImageLeft.Width, 0);


                //// 사이즈가 리사이즈
                //width = bitmap.Width / 10;
                //height = bitmap.Height / 10;
                //Bitmap resizeImage = new Bitmap(bitmap, new Size(width, height));
                edmsResult.DisplayBitmap = Mergebitmap;
                /////////////////////////////////////////////////////////////
                ProductInspected(edmsResult);
            }
            catch (Exception e)
            {

            }
            finally
            {
                SystemState.Instance().SetInspectState(InspectState.Wait);
            }
        }
        
        private bool GrabStartConinuous()
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.GrabMulti(-1);
            return true;
        }

        public override void ResetState()
        {
            resetZeroing = true;
        }

        public override void PreExitWaitInspection()
        {
            base.PreExitWaitInspection();

            if (runningThreadHandler != null && runningThreadHandler.IsRunning)
            {
                SimpleProgressForm form = new SimpleProgressForm();
                form.Show(() => runningThreadHandler?.Stop());
            }
            runningThreadHandler = null;
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.Stop();
        }

        public override void ExitWaitInspection()
        {
            PreExitWaitInspection();
            LogHelper.Debug(LoggerType.Function, "InspectRunner::ExitWaitInspection");

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.SetTriggerMode(TriggerMode.Software);
            foreach (ImageDevice imageDevice in imageDeviceHandler)
                imageDevice.ImageGrabbed -= ImageGrabbed;

            SystemManager.Instance().ProductionManager.Save();

            SystemState.Instance().SetIdle();
        }


        public override void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            Data.InspectionResult edmsResult = (Data.InspectionResult)inspectionResult;
            Settings.EDMSSettings edmsSetting = Settings.EDMSSettings.Instance();

            inspectionResult.InspectionEndTime = DateTime.Now;
            inspectionResult.InspectionTime = inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime;

            //UI update
            SystemManager.Instance().MainForm.InspectPage.ProductInspected(inspectionResult);
            //Production Revision
            if ((edmsResult.State == State_EDMS.Inspecting) && (edmsResult.Judgment != Judgment.Skip))
            {
                UniScanM.Data.Production production = this.UpdateProduction(edmsResult);
                if(production != null)
                lock (production)
                {
                    double value = Math.Abs(edmsResult.TotalEdgePositionResultLeft[(int)Data.DataType.Printing_FilmEdge_0]);
                    production.Value = (float)Math.Max(production.Value, value);
                }
            }     
            //data Export
            SystemManager.Instance().ExportData(inspectionResult);
        }

        public void OnRewinderCutting()
        {
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(f => f.ClearPanel());
        }

        public void OnLotChanged()
        {
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(f => f.ClearPanel());
        }
    }
}
