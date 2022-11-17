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
using UniScanM.EDMS.State;
using UniScanM.EDMS.Data;
using UniScanM.EDMS.Algorithm;
using UniScanM.EDMS.Settings;
using DynMvp.Device.Device.FrameGrabber;

namespace UniScanM.EDMS.Operation
{
    public class InspectRunner_M : UniScanM.Operation.InspectRunner
    {
        IState m_StateHandler = null;
        bool resetZeroing;
        double lotValueMax;
        double lotValueMin;

        profileQ m_profileQ = null;// new profileQ();
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
            m_profileQ =  new profileQ(width, height, Settings.EDMSSettings.Instance().BufferingCount);
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
                    if (autoLight)
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
                        hz = 80E3f;
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
            LightValue lightValue = (LightValue)SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0].LightValue;
            LightCtrl lightCtrl = SystemManager.Instance().DeviceBox.LightCtrlHandler.GetLightCtrl(0);
            lightCtrl.TurnOn(lightValue);
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
            Stopwatch timer = new Stopwatch();
            lastGrabbedIamgeDevice.imageDevice = imageDevice;
            lastGrabbedIamgeDevice.ptr = ptr;
            AlgoImage algoImage = null;

            timer.Start();
            ImageD imageD = imageDevice.GetGrabbedImage(ptr);//not deep copy
            algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);//not deep copy
            m_profileQ.AddImage(algoImage);
            timer.Stop();
            Debug.WriteLine("★" + timer.ElapsedMilliseconds.ToString());

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
                    if (imageDevice != null)
                        this.Inspect(imageDevice, ptr, inspectionResult);
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

            AlgoImage algoImage = null;
            try
            {
                inspectRunnerExtender.OnPreInspection();
                Data.InspectionResult edmsResult = (Data.InspectionResult)inspectionResult;

                if (resetZeroing == true)
                {
                    edmsResult.ResetZeroing = true;
                    resetZeroing = false;
                    m_StateHandler = new StateZeroing();
                    m_profileQ.Clear();
                }
                
                algoImage = m_profileQ.getDisplayImage();
                Debug.Assert(algoImage != null);
                bool isValidImage=false;
                float avgIntensity = 0.0f;
                float []profile = m_profileQ.GetAverge(ref  isValidImage, ref  avgIntensity);
                edmsResult.Profile = profile;
                edmsResult.FrameAvgIntensity = avgIntensity;
                if (profile !=null )
                {
                    ///////////////////////////////////////////
                    //Stopwatch timer = new Stopwatch();
                    //timer.Start();

                    if (isValidImage == true)
                    {
                        IState _state = this.m_StateHandler?.Handling(edmsResult);
                        if (_state != null)
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
                }
                else
                {
                    edmsResult.State = State_EDMS.Waiting;
                    edmsResult.Judgment = Judgment.Skip;
                }
                //디스플레이用 이미지.
                ImageD displayImage = algoImage.ToImageD(); //32ms ?? 뭐이래..DeepCopy   

                if (imageDevice.IsBinningVirtical()) //비닝이면 상하 늘리기..
                     displayImage = GetBinningImage(displayImage);//8ms

                edmsResult.DisplayBitmap = displayImage.ToBitmap();
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
                if (production != null)
                {
                    lock (production)
                    {
                        double value = edmsResult.TotalEdgePositionResult[(int)Data.DataType.Printing_FilmEdge_0];
                        this.lotValueMax = Math.Max(value, this.lotValueMax);
                        this.lotValueMin = Math.Min(value, this.lotValueMin);
                        double diff = this.lotValueMax - this.lotValueMin;
                        if (float.IsNaN(production.Value))
                            production.Value = (float)diff;
                        production.Value = (float)Math.Max(production.Value, diff);
                    }
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
            lotValueMin = lotValueMax = 0;
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(f => f.ClearPanel());
        }
    }
}
