using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
//using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using System.IO;
using UniScanM.Data;
using System.Threading;
using UniEye.Base.Device;
using UniScanM.Algorithm;
using DynMvp.Devices.Light;
using DynMvp.Devices.FrameGrabber;
using DynMvp.UI.Touch;
using UniEye.Base;
using DynMvp.InspData;
using UniEye.Base.Inspect;
using System.Drawing.Imaging;
using UniScanM.Pinhole.Algorithm;
using UniScanM.Pinhole.Data;
using DynMvp.Devices.Dio;
using UniScanM.Pinhole.UI.MenuPanel;
using UniScanM.Pinhole.Settings;
using UniScanM.UI.MenuPage.AutoTune;
using System.Windows.Forms;
using UniScanM.Operation;
using UniScanM.AlgoTask;
using System.Collections.Concurrent;

namespace UniScanM.Pinhole.Operation
{
    public delegate bool EnterWaitInspectionDelegate();
    public delegate void ExitWaitInspectionDelegate();

    public delegate void UpdateRealTimeImage(Bitmap bitmap, int deviceIndex);

    public class InspectRunner : UniScanM.Operation.InspectRunner
    {
        bool onReject = false;
        bool onRejectProcess = false;
        int camCount;

        int[] nextSectionIndex;
        ConcurrentQueue<(int, IntPtr)>[] ptrQueue;
        ConcurrentQueue<(ImageD, int)>[] imageBuffer;

        PinholeChecker pinholeChecker = new PinholeChecker();
        DigitalIoHandler digitalIoHandler;
        IoPort resultNg;

        CancellationTokenSource ctsInspTask = new CancellationTokenSource();

        public InspectRunner() : base()
        {
            resultNg = (SystemManager.Instance().DeviceBox.PortMap as PortMap).OutResultNg;
            digitalIoHandler = SystemManager.Instance().DeviceBox.DigitalIoHandler;
            camCount = SystemManager.Instance().DeviceBox.ImageDeviceHandler.Count;

            this.nextSectionIndex = new int[camCount];
            this.ptrQueue = new ConcurrentQueue<(int,IntPtr)>[camCount];
            this.imageBuffer = new ConcurrentQueue<(ImageD, int)>[camCount];

            for (int i = 0; i < camCount; ++i)
            {
                Size size = SystemManager.Instance().DeviceBox.CameraCalibrationList[i].ImageSize;
                this.imageBuffer[i] = new ConcurrentQueue<(ImageD, int)>();
                this.ptrQueue[i] = new ConcurrentQueue<(int,IntPtr)>();
                this.nextSectionIndex[i] = -1;
            }
            this.Reset();
            
            SystemManager.Instance().CreateInspectStarter();
            SystemManager.Instance().InspectStarter.OnStartInspection = EnterWaitInspection;
            SystemManager.Instance().InspectStarter.OnStopInspection = ExitWaitInspection;
            SystemManager.Instance().InspectStarter.OnRewinderCutting = OnRewinderCut;
            SystemManager.Instance().InspectStarter.OnLotChanged = OnLotChanged;
        }

        private void OnLotChanged()
        {
            this.Reset();
        }

        private void OnRewinderCut()
        {
            SystemManager.Instance().MainForm?.InspectPage?.InspectionPanelList.ForEach(f => f.ClearPanel());
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var buf in imageBuffer)
                while (buf != null ? buf.TryDequeue(out var res) : false) { };
        }

        public override bool EnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Function, "InspectRunner::EnterWaitInspection");

            SystemState.Instance().SetWait();
            Reset();
            SetCameraLineSpeed();

            //SystemManager.Instance().MainForm?.InspectPage.InspectionPanelList[0].EnterWaitInspection();  // 화면 초기화 필요시 넣을 것

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            for (int i = 0; i < camCount; i++)
            {
                //if (i == 1) break;
                Camera camera = (Camera)imageDeviceHandler.GetImageDevice(i);
                StartTask(camera, i);
                PtrToImageTask(imageDeviceHandler.GetImageDevice(i), i);
            }

            return PostEnterWaitInspection();
        }
        Task PtrToImageTask(ImageDevice imageDevice, int camIndex)
        {
            return Task.Run(() =>
            {
                while(SystemState.Instance().OpState != OpState.Idle)
                {
                    (int, IntPtr) item = default((int,IntPtr));
                    if(ptrQueue[camIndex].TryDequeue(out item))
                    {
                        ImageD imageD = null;
                        try
                        {
                            imageD = imageDevice.GetGrabbedImage(item.Item2);
                        }
                        catch (Exception ex)
                        {
                            var msg = ex.Message;
                            LogHelper.Debug(LoggerType.Error, msg);
                        }
                        if (imageD == null) continue;
                        imageD.Tag = item.Item1;
                        (ImageD, int) buf = (imageD, item.Item1);
                        imageBuffer[camIndex].Enqueue(buf);
                    }
                    else
                        Thread.Sleep(1);
                }
            });
        }
        Task StartTask(Camera camera, int camIndex)
        {
            return Task.Run(() =>
            {
                Debug.WriteLine("----- begin inspection Task.Run()-----------");
                while (SystemState.Instance().OpState !=  OpState.Idle)
                {
                    //(ImageD, int) buf;
                    //var count = imageBuffer[camIndex].Count -1;
                    //while (count-- > 0) imageBuffer[camIndex].TryDequeue(out buf);

                    if (imageBuffer[camIndex].TryDequeue(out var buf))
                    {

                        Stopwatch sw = new Stopwatch();
                        sw.Start();

                        int sectionIdx = buf.Item2;
                        ImageD image = buf.Item1;
                        LogHelper.Debug(LoggerType.Inspection, string.Format($"Start InspectFilm - ({camIndex}/{sectionIdx})"));

                        Data.InspectionResult inspResult = CreateInspectionResult(camIndex, sectionIdx);
                        // PtrToImageTask에서 Ptr을 통해 획득된 이미지를 inspResult.GrabImageList에 추가해서 넘겨줌

                        
                    //    image.Save(string.Format("d:\\cam{0}_test_{1}.bmp", camIndex, sectionIdx) ); //이건 안되는거.
                        //image.SaveImage(string.Format("d:\\camSI{0}_test_{1}.bmp", camIndex, sectionIdx) );



                        inspResult.GrabImageList.Add(image);

                        Inspect(camera, new IntPtr(0), inspResult, null);

                        LogHelper.Debug(LoggerType.Inspection, string.Format($"End InspectFilm - ({camIndex}/{sectionIdx}) / Time = {sw.ElapsedMilliseconds}"));
                    }
                    else
                        Thread.Sleep(1);
                }
                Debug.WriteLine("-----finish inspection Task.Run()-----------");
            });
        }

        void SetCameraLineSpeed()
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            float lineSpeed = (float)SystemManager.Instance().InspectStarter.GetLineSpeedSv();
            for (int i = 0; i < camCount; ++i)
            {
                Camera camera = (Camera)imageDeviceHandler.GetImageDevice(i);
                if (camera != null)
                {
                    float pixelResoultion = (float)PinholeSettings.Instance().PixelResolution / 1000.0f;

                    if (lineSpeed == 0)
                        lineSpeed = 120;                    

                    float Hz = ((lineSpeed / 60)  * 1000) / pixelResoultion;

                    bool set = camera.SetAcquisitionLineRate(Hz);//7092.1985(100m/min)
                    camera.SetExposureTime(130);
                }
            }
        }

        protected override void ErrorManager_OnStartAlarm()
        {
            ExitWaitInspection();
        }

        public override bool PostEnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Grab, "InspectRunner::PostEnterWaitInspection");

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            
            imageDeviceHandler.SetTriggerMode(TriggerMode.Software);

            TimeSpan idltTimeSpan = TimeSpan.MaxValue;

            if (((InspectRunnerExtender)inspectRunnerExtender).EndTime.HasValue)
                idltTimeSpan = DateTime.Now - ((InspectRunnerExtender)inspectRunnerExtender).EndTime.Value;

            SystemState.Instance().SetInspect();
            ((InspectRunnerExtender)inspectRunnerExtender).StartTime = DateTime.Now;
            ((InspectRunnerExtender)inspectRunnerExtender).EndTime = null;
            
            bool tooLongIdle = idltTimeSpan.TotalMinutes > 10;
            SystemState.Instance().SetInspectState(InspectState.Tune);
            Application.DoEvents();

            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            lightCtrlHandler.TurnOn(SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0]);

            Thread.Sleep(100);

            foreach (ImageDevice imageDevice in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
            {
                imageDevice.ImageGrabbed = ImageGrabbed;
            }
            SystemState.Instance().SetInspectState(InspectState.Run);

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetTriggerMode(TriggerMode.Hardware);
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabMulti();

            Thread.Sleep(200);

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetTriggerMode(TriggerMode.Software);

            return true;
        }
        
        protected override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {

            ptrQueue[imageDevice.Index].Enqueue((nextSectionIndex[imageDevice.Index]++,ptr));
        }

        public void RewinderCut()
        {
            
        }

        
        private void SendError(int sectionIndex)
        {
            if (onReject)
                return;
            onReject = true;
            Task.Run(() =>
            {
                onRejectProcess = true;

                digitalIoHandler.WriteOutput(resultNg, true);
                
                Thread.Sleep(50);

                digitalIoHandler.WriteOutput(resultNg, false);
                onReject = false;
                onRejectProcess = false;
            });
        }

        private void SendReset(int sectionIndex)
        {
            if (onReject == false )
                return;
            if (onRejectProcess)
                return;
                
            //두개의 동시 싱크 맞추기는 불가능에 가까움
            Task.Run(() =>
            {
                digitalIoHandler.WriteOutput(resultNg, false);
                onReject = false;
            });
        }

        private bool Grab()
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                if (imageDevice is CameraVirtual)
                {
                    CameraVirtual cameraVirtual = (CameraVirtual)imageDevice;
                }
            }

            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            lightCtrlHandler.TurnOn(SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0]);
            imageDeviceHandler.GrabOnce();
            bool ok = imageDeviceHandler.WaitGrabDone(5000);
            imageDeviceHandler.Stop();
            lightCtrlHandler.TurnOff();
            return ok;
        }

        public override void PreExitWaitInspection()
        {
            LogHelper.Debug(LoggerType.Operation, "InspectRunner::PreExitWaitInspection");

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.Stop();

            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();

            if (inspectRunnerExtender != null)
                ((InspectRunnerExtender)inspectRunnerExtender).EndTime = DateTime.Now;
        }

        public override void ExitWaitInspection()
        {
            base.ExitWaitInspection();
            onReject = false;
        }

        public override void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Operation, "InspectRunner::ProductInspected");

            inspectionResult.InspectionEndTime = DateTime.Now;
            inspectionResult.InspectionTime = inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime;

            UniScanM.Pinhole.Data.InspectionResult pinholeInspectionResult = (UniScanM.Pinhole.Data.InspectionResult)inspectionResult;

            UniScanM.Data.Production production = SystemManager.Instance().ProductionManager.GetProduction(pinholeInspectionResult);
            lock (production)
            {
                production.Update(pinholeInspectionResult);
            }

            UniScanM.Data.InspectionResult sheetInspectionResult = (UniScanM.Data.InspectionResult)inspectionResult;
            SystemManager.Instance().MainForm?.InspectPage.ProductInspected(sheetInspectionResult);
            SystemManager.Instance().ExportData(inspectionResult);
        }

        public void SetupAutoTune()
        {

        }

        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            SystemState.Instance().SetInspectState(InspectState.Run);

            ImageD grabbedImage = inspectionResult.GrabImageList[0];
            
            Debug.Assert(grabbedImage != null);
            var sheetInspResult = inspectionResult as Data.InspectionResult;
            try
            {
                pinholeChecker.Inspect(grabbedImage, sheetInspResult);
            }
            catch (Exception ex)
            {
                LogHelper.Debug(LoggerType.Inspection, ex.Message);
                LogHelper.Debug(LoggerType.Inspection, ex.StackTrace);
            }
            if (PinholeSettings.Instance().UseReject)
            {
                if (inspectionResult.Judgment != Judgment.Accept && OperationOption.Instance().OnTune == false)
                {
                    SendError(sheetInspResult.SectionIndex);
                }
            }
            ProductInspected(sheetInspResult);

            SystemState.Instance().SetInspectState(InspectState.Done);
        }

        private void Reset()
        {
            //prepareSectionIndex = 0;
            Data.Production production = (Data.Production)SystemManager.Instance().ProductionManager.CurProduction;
            int lastSection = production == null ? -1 : Math.Max(production.Section1, production.Section2);
            for (int i = 0; i < nextSectionIndex.Length; i++)
                nextSectionIndex[i] = lastSection;
            digitalIoHandler.WriteOutput(resultNg, false); // 검사 시작시 IO를 리셋한다.
        }

        internal void ToggleAutoTuneMode()
        {

        }

        private Data.InspectionResult CreateInspectionResult(int deviceIndex, int sectionIndex)
        {
            var inspectionResult = this.inspectRunnerExtender.BuildInspectionResult() as Data.InspectionResult;

            inspectionResult.SectionIndex = sectionIndex;
            inspectionResult.DeviceIndex = deviceIndex;

            return inspectionResult;
        }

        string GetResultPath(string modelName, DateTime startTime, string lotNo, int deviceIndex)
        {
            string autoManual = SystemManager.Instance().InspectStarter.StartMode == StartMode.Auto ? "Auto" : "Manual";
            string result = Path.Combine(PathSettings.Instance().Result, startTime.ToString("yyyyMMdd"), modelName, autoManual, lotNo);
            return result;
        }

        void SetLineSpeed()
        {
            float lineSpeed = GetLineSpeed();
            for(int i = 0; i < camCount; i++)
            {
                ImageDevice cam = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(i);
            }
        }

        float GetLineSpeed()
        {
            float sheetSpeed = 10;
            float resolution = 0.122f;
            float result = ((sheetSpeed / 60) * 1000) / resolution;
            return result;
        }

        public void DemoInspect()
        {

        }
    }

    public class GrabbedImage
    {
        int index = 0;
        Bitmap bitmap;

        public int Index { get => index; set => index = value; }
        public Bitmap Bitmap { get => bitmap;}

        public GrabbedImage(int index, Bitmap bitmap)
        {
            this.index = index;
            this.bitmap = bitmap;
        }
    }

    public class GrabbedImageList : List<GrabbedImage>
    {

    }
}

