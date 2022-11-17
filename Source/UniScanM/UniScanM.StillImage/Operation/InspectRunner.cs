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
using UniScanM.StillImage.State;
using UniScanM.StillImage.Settings;
using System.Windows.Forms;
using DynMvp.InspData;
using DynMvp.Device.Device.FrameGrabber;


namespace UniScanM.StillImage.Operation
{
    public class InspectRunner : UniScanM.Operation.InspectRunner
    {
        private struct LastGrabbedIamgeDevice
        {
            public ImageDevice imageDevice;
            public IntPtr ptr;
        }

        LastGrabbedIamgeDevice lastGrabbedIamgeDevice;
        //EncoderInspectStarter inspectStarter = null; //시트 속도 감지후 시작 or 정지.
        ThreadHandler runningThreadHandler = null;  // 검사시 While 돌릴 쓰레드.
        
        public InspectRunner() : base()
        {
            lastGrabbedIamgeDevice = new LastGrabbedIamgeDevice();

            //inspectStarter = new EncoderInspectStarter();
            //inspectStarter.OnStartInspection = EnterWaitInspection;
            //inspectStarter.OnStopInspection = ExitWaitInspection;

            SystemManager.Instance().InspectStarter.OnStartInspection += EnterWaitInspection;
            SystemManager.Instance().InspectStarter.OnStopInspection += ExitWaitInspection;
            SystemManager.Instance().InspectStarter.OnRewinderCutting+= OnRewinderCutting;
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

        public override bool EnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Function, "InspectRunner::EnterWaitInspection");

            if (runningThreadHandler != null)
                return false;

            if (SystemManager.Instance().DeviceController.OnEnterWaitInspection() == false)
                return false;


            StillImageSettings stillImageSettings = StillImageSettings.Instance();

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            foreach (ImageDevice imageDevice in imageDeviceHandler)
                imageDevice.ImageGrabbed += ImageGrabbed;

            imageDeviceHandler.SetTriggerMode(TriggerMode.Hardware);

            //if (SystemManager.Instance().InspectStarter.StartMode == StartMode.Auto)
            //{
            //    // Get Model Name
            //    if(additionalSettings.ModelAutoChange)
            //        ChangeModel();
            //}

            //if (SystemManager.Instance().CurrentModel == null)
            //{
            //    ErrorSection errorSection = ErrorSection.Environment;
            //    ErrorSubSection errorSubSection = ErrorSubSection.CommonReason;
            //    ErrorManager.Instance().Report((int)errorSection, (int)errorSubSection, ErrorLevel.Error, errorSection.ToString(), "Model", "There is no selected Model");
            //    //MessageForm.Show(null, "There is no selected Model");

            //    return false;
            //}

            // Load Lot No
            if (SystemManager.Instance().InspectStarter is EncoderInspectStarter)
                ChangeLotNo();

            SystemState.Instance().SetWait();

            bool userNotify =  SystemManager.Instance().InspectStarter.StartMode != StartMode.Auto;
            CheckOrigin(userNotify);

            return PostEnterWaitInspection();
        }

        public override bool PostEnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Grab, "InspectRunner::PostEnterWaitInspection");

            StillImageSettings additionalSettings = StillImageSettings.Instance();
            InspectRunnerExtender inspectRunnerExtender = this.InspectRunnerExtender as InspectRunnerExtender;

            if (additionalSettings.AsyncMode)
            {
                float grabHz = additionalSettings.AsyncGrabHz;
                if (grabHz < 0)
                // 음수면 속도에 맞춰 동기화
                {
                    double lineSpeed = SystemManager.Instance().InspectStarter.GetLineSpeedSv();
                    double umPpix = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height;
                    grabHz = (float)(lineSpeed * 1000.0 * 1000 / 60.0 / umPpix);
                }

                SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetTriggerMode(TriggerMode.Software);
                SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetAquationLineRate(grabHz);
            }

            string modelName = SystemManager.Instance().CurrentModel.PlcModel;
            string modelName2 = SystemManager.Instance().InspectStarter.GetModelName();
            if (!string.IsNullOrEmpty(modelName)
                && !string.IsNullOrEmpty(modelName2)
                && modelName == modelName2 
                && !modelName.Contains($"ABCDEFG00")//debug 용
                && false)
            {
                Data.Model model = (Data.Model)SystemManager.Instance().CurrentModel;

                if (additionalSettings.InspectionMode == EInspectionMode.Inspect)
                {
                    processer = new InspectionState(model.TeachDataDic, (Data.InspectParam)model.InspectParam);
                }
                else
                {
                    processer = new MonitoringState(model.TeachDataDic);
                }
            }
            else
            {
                processer = new LightTuneState();
            }

            runningThreadHandler = new ThreadHandler("InspectRunner", new Thread(RunningThreadProc), false);
            requestStop = false;
            runningThreadHandler.Start();

            SystemState.Instance().SetInspect();
            SystemState.Instance().SetInspectState(InspectState.Run);

            return true;
        }

        private void ChangeLotNo()
        {
            string model = SystemManager.Instance().CurrentModel.Name;
            string worker = SystemManager.Instance().InspectStarter.GetWorker();
            string lotNo = SystemManager.Instance().InspectStarter.GetLotNo();
            string paste = SystemManager.Instance().InspectStarter.GetPaste();
            string mode = SystemManager.Instance().InspectStarter.StartMode.ToString();
            int rewinderSite = SystemManager.Instance().InspectStarter.GetRewinderSite();
            int position = SystemManager.Instance().InspectStarter.GetPosition();

            if (string.IsNullOrEmpty(lotNo))
                lotNo = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            //SystemManager.Instance().ProductionManager.LotChange(SystemManager.Instance().CurrentModel, lotNo);
            SystemManager.Instance().ProductionManager.LotChange(model, worker, lotNo, paste, mode, rewinderSite);
            SystemManager.Instance().ProductionManager.CurProduction.StartPosition = position;

            SystemManager.Instance().InspectStarter?.OnLotChanged();
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

        public override void ExitWaitInspection()
        {
            base.ExitWaitInspection();

            if (SystemManager.Instance().InspectStarter.StartMode == StartMode.Stop)
                SystemState.Instance().SetInspectState(InspectState.Stop);
            else
                SystemState.Instance().SetInspectState(InspectState.Ready);
        }


        protected override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            lastGrabbedIamgeDevice.imageDevice = imageDevice;
            lastGrabbedIamgeDevice.ptr = ptr;
        }

        private void RunningThreadProc()
        {
            SystemState.Instance().SetInspect();

            while (runningThreadHandler.RequestStop == false)
            {
                UniscanState curState = (UniscanState)processer;

                // Grab
                LogHelper.Debug(LoggerType.Operation, "Grab Start");
                if (Grab() == false)
                {
                    if (requestStop == false)
                    {
                        ErrorManager.Instance().Report(ErrorCodeGrabber.Instance.Timeout, ErrorLevel.Error,
                            ErrorCodeGrabber.Instance.Message, "Grab Timeout", null, "Check the Grabber and Camera");
                    }
                    runningThreadHandler.RequestStop = true;
                    continue;
                }
                LogHelper.Debug(LoggerType.Operation, "Grab Done");

                DynMvp.InspData.InspectionResult inspectionResult = BuildInspectionResult();
                try
                {
                    if (inspectionResult == null)
                        continue;

                    ImageDevice imageDevice = this.lastGrabbedIamgeDevice.imageDevice;
                    if (imageDevice == null)
                        continue;

                    IntPtr ptr = this.lastGrabbedIamgeDevice.ptr;
                    InspectOption inspectionOption = new InspectOption(imageDevice, ptr);
              
                    this.Inspect(imageDevice, ptr, inspectionResult, inspectionOption);
                }
#if DEBUG == false
            catch(Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, ex.Message);
                inspectionResult.SetDefect();
            }
#endif
                finally { }

                //GC.Collect();
                //GC.WaitForFullGCComplete();
            }

            SystemState.Instance().SetIdle();

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.Stop();
            SystemManager.Instance().DeviceController.RobotStage.StopMove();
            SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
        }

        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, DynMvp.InspData.InspectionResult inspectionResult,
            InspectOption inspectionOption = null)
        {
            LogHelper.Debug(LoggerType.Operation, "InspectRunner::Inspect");

            //AlgoImage algoImage = null;
            //AlgoImage adjustImage = null;
            UniscanState curState = (UniscanState)this.processer;
            ImageD imageD = null;
            try
            {
                imageD = imageDevice.GetGrabbedImage(ptr);
                Debug.Assert(imageD != null);
                //imageD = imageD.Clone(); // shallow copy
                ImageD cloneimageD = new Image2D(imageD.Width, imageD.Height,imageD.NumBand, imageD.Pitch);
                cloneimageD.CopyFrom(imageD);

            //    cloneimageD.SaveImage("d:\\test11.bmp");
                //cloneimageD.Save("d:\\test22.bmp");



                //if (imageDevice.IsBinningVirtical())
                //    imageD = ((UniScanM.Operation.InspectRunnerExtender)inspectRunnerExtender).GetBinningImage(imageD);

                //algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);
                //Debug.Assert(algoImage != null);

                //adjustImage = GetAdjustImage(algoImage);
                inspectRunnerExtender.OnPreInspection();

                SystemState.Instance().SetInspectState(curState.InspectState);
                this.processer.Process(cloneimageD, inspectionResult, inspectionOption);

                inspectRunnerExtender.OnPostInspection();

                if (curState.IsSyncState)
                    ProductInspected(inspectionResult);
            }
            finally
            {
                //if (curState.IsSyncState)
                //    adjustImage?.Dispose();

                //algoImage?.Dispose();
                //imageD?.Dispose();
                //SystemState.Instance().SetInspectState(InspectState.Wait);
                //SystemState.Instance().SetWait();
            }
        }
        
        private bool Grab()
        {
            Debug.WriteLine("InspectRunner::Grab Start");
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                CameraVirtual cameraVirtual = imageDevice as CameraVirtual;
                if (cameraVirtual != null)
                    cameraVirtual.SetStepLight(((UniscanState)this.processer).ImageSequnece, 0);
                //cameraVirtual.SetStepLight(0, 0);
            }

            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            LightValue lightValue = SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0].LightValue.Clone();
            lightCtrlHandler.TurnOn(lightValue);

            imageDeviceHandler.GrabOnce();
            bool ok = imageDeviceHandler.WaitGrabDone(-1);
            imageDeviceHandler.Stop();
            lightCtrlHandler.TurnOff();
            Debug.WriteLine("InspectRunner::Grab End");
            return ok;
        }

        private bool requestStop = false;
        public override void PreExitWaitInspection()
        {
            requestStop = true;
            base.PreExitWaitInspection();

            if (runningThreadHandler != null)
            {
                SimpleProgressForm form = new SimpleProgressForm();
                form.Show(() => runningThreadHandler.Stop());

                runningThreadHandler = null;
            }
        }

 
        public override void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            inspectionResult.InspectionEndTime = DateTime.Now;
            inspectionResult.InspectionTime = inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime;
            UniScanM.StillImage.Data.InspectionResult stillImageinspectionResult = (UniScanM.StillImage.Data.InspectionResult)inspectionResult;

            //1. 화면 비율에 맞게 적당히 잘라냄 -> 화면에 벗어난 결함은 제외시킴
            ResizingDisplayImageAndDefectResult(stillImageinspectionResult);
            //2. OK/NG 판정실시
            stillImageinspectionResult.UpdateJudgement();
            //3. 프로덕션 카운트 값 수정(토탈,OK, NG 수량, 핀홀, 블랏 마진 수량)
            if (((UniscanState)processer).IsTeachState == false)
            {
                UniScanM.Data.Production production = this.UpdateProduction(stillImageinspectionResult);
            }

            //4. Update UI
            SystemManager.Instance().MainForm.InspectPage.ProductInspected(inspectionResult);
            //5. ????? 굳이?
            if (stillImageinspectionResult.SheetRectInFrame.IsEmpty == false)
            {
                double sheetLength = stillImageinspectionResult.SheetRectInFrame.Height * SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height / 1000.0;
                SystemManager.Instance().MainForm.SettingPage.UpdateControl("SheetLength", sheetLength);
            }
            //6. 데이터 파일 출력 ( PLC )
            if (((UniscanState)processer).IsTeachState == false)
            {
                SystemManager.Instance().ExportData(inspectionResult);
            }
            //7. 다음 상태로...
            this.processer = ((UniscanState)processer).GetNextState(inspectionResult);
            //if (((UniscanState)inspectProcesser).Initialized == false)
                //((UniscanState)inspectProcesser).Initialize();
        }

        private Size GetClipSize(Size imageSize)
        {
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Size viewSize= imageSize;
            foreach (var panel in SystemManager.Instance().MainForm.InspectPage.InspectionPanelList)
            {
                if (panel is UniScanM.StillImage.UI.InspectionPanelLeft)
                {
                    UniScanM.StillImage.UI.InspectionPanelLeft thisPanel = panel as UniScanM.StillImage.UI.InspectionPanelLeft;
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

        private void ResizingDisplayImageAndDefectResult(UniScanM.StillImage.Data.InspectionResult inspectionResult)
        {
            Data.InspectionResult myInspectionResult = (Data.InspectionResult)inspectionResult;

            myInspectionResult.DisplayImageRect = Rectangle.Empty;

            if (myInspectionResult.DisplayBitmap != null)
            {// 화면 비율에 맞게 적당히 잘라냄
                Rectangle imageRect = new Rectangle(Point.Empty, myInspectionResult.DisplayBitmap.Size);

                Point imageCenter = Point.Round(DrawingHelper.CenterPoint(imageRect));
                if (myInspectionResult.ProcessResultList?.InterestProcessResult?.InspPatternInfo?.ShapeInfo?.BaseRect.IsEmpty == false)
                    imageCenter = Point.Round(DrawingHelper.CenterPoint(myInspectionResult.ProcessResultList.InterestProcessResult.InspPatternInfo.ShapeInfo.BaseRect));

                Size clipSize = GetClipSize(imageRect.Size); //화면사이즈에 맞게 영역을 설정함. drawBox.Size;
                Rectangle clipRect = DrawingHelper.FromCenterSize(imageCenter, clipSize);
                if (clipRect.Width > imageRect.Width)
                    clipRect.Inflate((imageRect.Width - clipRect.Width - 1) / 2, 0);
                if (clipRect.Height > imageRect.Height)
                    clipRect.Inflate(0, (imageRect.Width - clipRect.Width - 1) / 2);

                if (clipRect.Left < imageRect.Left)
                    clipRect.Offset(imageRect.Left - clipRect.Left, 0);
                else if (clipRect.Right > imageRect.Right)
                    clipRect.Offset(imageRect.Right - clipRect.Right, 0);

                if (clipRect.Top < imageRect.Top)
                    clipRect.Offset(0, imageRect.Top - clipRect.Top);
                else if (clipRect.Bottom > imageRect.Bottom)
                    clipRect.Offset(0, imageRect.Bottom - clipRect.Bottom);

                System.Diagnostics.Debug.WriteLine(string.Format("clipRect: {0}, {1}", clipRect.Width, clipRect.Height));
                clipRect.Intersect(imageRect);
                myInspectionResult.DisplayImageRect = clipRect;
            }// 화면 비율에 맞게 적당히 잘라냄

            System.Diagnostics.Debug.WriteLine(string.Format("displayRect: {0}, {1}", myInspectionResult.DisplayImageRect.Width, myInspectionResult.DisplayImageRect.Height));

            int inspectZone = myInspectionResult.InspZone;
            string inspectState = myInspectionResult.InspectState;
            if (inspectState == "Inspection")   // In Inspection State
            {
                if (inspectionResult.Judgment == Judgment.Skip)
                // sheet not founded 
                {
                    return;
                }

                Data.ProcessResult interestProcessResult = myInspectionResult.ProcessResultList.InterestProcessResult;
                if (interestProcessResult != null)
                {
                    // 영역 밖 핀홀 불량 지움
                    int invalidateBound = Settings.StillImageSettings.Instance().InvalidateBound;
                    Rectangle innerValidRect = Rectangle.Inflate(myInspectionResult.DisplayImageRect, -invalidateBound, -invalidateBound);

                    myInspectionResult.ProcessResultList.DefectRectList.RemoveAll(f =>
                    {
                        bool inbound = Rectangle.Intersect(innerValidRect, f) != f;
                        return inbound;
                    }); // Inbound에 완전히 속해야 불량을 살린다.
                }
            }
            //Debug.WriteLine("▣ RemoveAll  222222 = " + myInspectionResult.InspZone.ToString() + "=" + myInspectionResult.ProcessResultList.DefectRectList.Count.ToString());
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
