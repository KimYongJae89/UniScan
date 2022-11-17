using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using UniEye.Base.Data;
using System.Threading;
using DynMvp.Devices.Light;
using DynMvp.Devices.FrameGrabber;
using DynMvp.UI.Touch;
using UniEye.Base.Inspect;
using UniScanM.CGInspector.Settings;
using System.Windows.Forms;
using DynMvp.InspData;
using DynMvp.Device.Device.FrameGrabber;
using UniScanM.CGInspector.UI;
using System.Linq;
using DynMvp.Data;

namespace UniScanM.CGInspector.Operation
{
    public class InspectRunner : UniScanM.Operation.InspectRunner
    {
        private struct LastGrabbedIamgeDevice
        {
            public ImageDevice ImageDevice => this.imageDevice;
            ImageDevice imageDevice;

            public IntPtr Ptr => this.ptr;
            IntPtr ptr;

            public void Set(ImageDevice imageDevice, IntPtr ptr)
            {
                this.imageDevice = imageDevice;
                this.ptr = ptr;
            }
        }

        struct RunrunrunArgumentStruct
        {
            public float startPos;
            public float endPos;
            public int count;
            public CancellationToken cancellationToken;
        }

        LastGrabbedIamgeDevice lastGrabbedIamgeDevice;
        Task task = null;

        public InspectRunner() : base()
        {
            this.lastGrabbedIamgeDevice = new LastGrabbedIamgeDevice();
            this.lastGrabbedIamgeDevice.Set(null, IntPtr.Zero);

            SystemManager.Instance().InspectStarter.OnStartInspection += EnterWaitInspection;
            SystemManager.Instance().InspectStarter.OnStopInspection += ExitWaitInspection;
            SystemManager.Instance().InspectStarter.OnRewinderCutting += OnRewinderCutting;
            SystemManager.Instance().InspectStarter.OnLotChanged += OnLotChanged;
        }

        private void OnLotChanged()
        {
            //SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(f => f.ClearPanel());
        }

        private void OnRewinderCutting()
        {
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(f => f.ClearPanel());
        }

        public override void Dispose()
        {
            base.Dispose();

            //inspectStarter.Stop();
            //inspectStarter.Dispose();
            //ExitWaitInspection();
        }

        protected override void ErrorManager_OnStartAlarm()
        {
            ExitWaitInspection();
        }

        public override void PreExitWaitInspection()
        {
            base.PreExitWaitInspection();
        }

        public override void ExitWaitInspection()
        {
            base.ExitWaitInspection();
        }

        public override bool EnterWaitInspection()
        {
            // 로봇 원점 확인
            bool userNotify = SystemManager.Instance().InspectStarter.StartMode != StartMode.Auto;
            CheckOrigin(userNotify);

            // 조명 컴
            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            Model model = SystemManager.Instance().CurrentModel;
            LightValue lightValue = model.LightParamSet.LightParamList.FirstOrDefault()?.LightValue.Clone();
            if (lightValue != null)
                lightCtrlHandler.TurnOn(lightValue);

            // 카메라 동작 준비
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            foreach (ImageDevice imageDevice in imageDeviceHandler)
                imageDevice.ImageGrabbed += ImageGrabbed;
            imageDeviceHandler.SetTriggerMode(TriggerMode.Software);

            double lineSpeed = SystemManager.Instance().InspectStarter.GetLineSpeedSv();
            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
            double umPpix = calibration != null ? calibration.PelSize.Height : 10.0f;
            float grabHz = 10000.0f; //lineSpeed * 1000.0 * 1000 / 60.0 / umPpix; 
            imageDeviceHandler.SetAquationLineRate(grabHz);

            // 시스템 대기 상태 설정
            SystemState.Instance().SetWait();

            return PostEnterWaitInspection();
        }

        protected override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            this.lastGrabbedIamgeDevice.Set(imageDevice, ptr);
            ImageD imageD = imageDevice.GetGrabbedImage(ptr);
            imageD.SaveImage(@"C:\temp\ImageD.bmp");

            UniScanM.CGInspector.Data.InspectionResult inspectionResult = BuildInspectionResult() as UniScanM.CGInspector.Data.InspectionResult;
            inspectionResult.DisplayBitmap = imageD.ToBitmap();
            ProductInspected(inspectionResult);
        }

        public override bool PostEnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Grab, "InspectRunner::PostEnterWaitInspection");
            SystemState.Instance().SetInspect();
            SystemState.Instance().SetInspectState(InspectState.Wait);
            return true;
        }

        private void CheckOrigin(bool userQuary)
        {
            AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            if (axisHandler == null)
                return;

            if (axisHandler.IsAllHomeDone())
                return;

            bool needHome = true;
            if (userQuary)
                needHome = (MessageForm.Show(null, "Origin Move?", "UniScan", MessageFormType.YesNo, 5000, DialogResult.Yes) == DialogResult.Yes);

            if (needHome)
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
                SimpleProgressForm form = new SimpleProgressForm("Origin");
                form.Show(() =>
                {
                    Task homeTask = axisHandler.StartHomeMove();
                    homeTask.Wait(100);
                    axisHandler.WaitHomeDone(cancellationTokenSource);
                }, cancellationTokenSource);
            }
        }

        public void RunRunRun(float startPos, float endPos,int count, CancellationToken cancellationToken)
        {
            RunrunrunArgumentStruct runrunrunArgument = new RunrunrunArgumentStruct();
            runrunrunArgument.startPos = startPos;
            runrunrunArgument.endPos = endPos;
            runrunrunArgument.count = count;
            runrunrunArgument.cancellationToken = cancellationToken;

            if (this.task == null || this.task.IsCompleted)
            {
                this.task = new Task(TaskProc, runrunrunArgument, cancellationToken);
                this.task.Start();
            }
        }

        private void TaskProc(object arg)
        {
            RunrunrunArgumentStruct argment = (RunrunrunArgumentStruct)arg;
            try
            {
                for (int i = 0; i < argment.count; i++)
                {
                    AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;

                    // 시작점으로 이동
                    if (axisHandler != null)
                        axisHandler.Move(0, argment.startPos);

                    // 카메라 작동
                    SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabOnce();

                    // 끝점으로 이동
                    if (axisHandler != null)
                        axisHandler.Move(0, argment.endPos);

                    // 카메라 정지
                    SystemManager.Instance().DeviceBox.ImageDeviceHandler.WaitGrabDone();
                    SystemManager.Instance().DeviceBox.ImageDeviceHandler.Stop();

                    argment.cancellationToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            { }
        }

        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            LogHelper.Debug(LoggerType.Operation, "InspectRunner::Inspect");
        }

        private bool Grab()
        {
            return false;
        }


        public override void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            inspectionResult.InspectionEndTime = DateTime.Now;
            inspectionResult.InspectionTime = inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime;

            UniScanM.CGInspector.Data.InspectionResult stillImageinspectionResult =
                (UniScanM.CGInspector.Data.InspectionResult)inspectionResult;

            // Update UI
            SystemManager.Instance().MainForm.InspectPage.ProductInspected(inspectionResult);
        }

        private Size GetClipSize(Size imageSize)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Size viewSize = imageSize;
            foreach (var panel in SystemManager.Instance().MainForm.InspectPage.InspectionPanelList)
            {
                if (panel is UniScanM.CGInspector.UI.InspectionPanelLeft)
                {
                    UniScanM.CGInspector.UI.InspectionPanelLeft thisPanel = panel as UniScanM.CGInspector.UI.InspectionPanelLeft;
                    viewSize = thisPanel.GetImageViewSize();
                }
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Size displaySize = viewSize;
            float rX = imageSize.Width * 1.0f / displaySize.Width * 1.0f;
            float rY = imageSize.Height * 1.0f / displaySize.Height * 1.0f;
            float ratio = Math.Min(rX, rY);
            Size clipImageSize = Size.Round(new SizeF(displaySize.Width * ratio, displaySize.Height * ratio));
            return clipImageSize;
        }

        private AlgoImage GetAdjustImage(AlgoImage grabbedImage)
        {
            AlgoImage adjustImage = null;
            // Histogram EQ
            if (false)
            {
                adjustImage = ImageBuilder.BuildSameTypeSize(grabbedImage);
                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(grabbedImage);
                imageProcessing.Mul(grabbedImage, adjustImage, 10);
                //imageProcessing.HistogramEqualization(clipImage);
                //clipImage.Save(Path.Combine(imageSavePath, "HistoEQ", iamgeSaveName));
            }
            else
            {
                adjustImage = grabbedImage.Clone();
            }
            return adjustImage;
        }
    }
}
