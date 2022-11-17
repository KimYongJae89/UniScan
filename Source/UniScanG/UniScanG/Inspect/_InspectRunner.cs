//using System;
//using System.Drawing;
//using System.IO;

//using DynMvp.Base;
//using DynMvp.Data;
//using DynMvp.Devices;
//using DynMvp.InspData;
//using UniEye.Base.Data;
//using UniEye.Base.Settings;
//using UniEye.Base.Inspect;
//using System.Threading;
//using System.Windows.Forms;
//using DynMvp.Vision;
//using DynMvp.UI;
//using DynMvp.UI.Touch;
//using UniEye.Base.Device;

//namespace UniScanG.Temp
//{
//    internal class UniScanGInspectRunner : DirectTriggerInspectRunner
//    {
//        //SheetChecker sheetChecker = null;

//        GrabProcesser grabProcesser = null;
//        private UniScanGInspectUnitManager unitManager = new UniScanGInspectUnitManager();
//        public UniScanGInspectUnitManager UnitManager
//        {
//            get { return unitManager; }
//        }

//        ThreadHandler inspectionMonitorThread = null;

//        int deviceIndex = 0;

//        object scanLock = new object();
//        int scanRemains = 0;
//        private bool requestStop;

//        public UniScanGInspectRunner() : base()
//        {

//        }

//        ~UniScanGInspectRunner()
//        {
//            if (grabProcesser != null)
//                grabProcesser.Dispose();
//            grabProcesser = null;

//            unitManager.Dispose();

//            if (inspectionMonitorThread != null)
//                inspectionMonitorThread.RequestStop = true;
//        }

//        protected ImageDevice GetImageDevice(int deviceIndex)
//        {
//            return SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(deviceIndex);
//        }


//        protected void SetupGrabProcesser()
//        {
//            //if (grabProcesser != null)
//            //    grabProcesser.Dispose();
//            //grabProcesser = null;

//            //Data.Model model = (Data.Model)SystemManager.Instance().CurrentModel;
//            //VisionProbe visionProbe = model.GetVisionProbe();
//            //SheetCheckerParam param = visionProbe.InspAlgorithm.Param as SheetCheckerParam;

//            //grabProcesser = new GrabProcesser();
//            //grabProcesser.ClientIndex = -1;
//            //grabProcesser.Initialize(param);

//            //grabProcesser.Start();

//            //grabProcesser = new GrabProcesser();
//            //grabProcesser.SetAlgorithm(sheetChecker.SheetCheckerFiducial);
//            //grabProcesser.ClientIndex = UniScanGSettings.Instance().InspectorInfo.ClientIndex;

//            //grabProcesser.Start();
//        }

//        public override bool EnterWaitInspection()
//        {
//            base.EnterWaitInspection();

//            this.requestStop = false;

//            SimpleProgressForm form = new SimpleProgressForm("Prepare");
//            form.Show(() =>
//            {
//                SetupGrabProcesser();
//                SetupUnitManager(UniScanGSettings.Instance().BufferSize);
//                SetupInspectMonitor();
//            });

//            if (form.Task.Exception != null)
//                throw form.Task.Exception;

//            return true;
//        }

//        private void SetupInspectMonitor()
//        {
//            if (inspectionMonitorThread == null)
//            {
//                inspectionMonitorThread = new ThreadHandler(new System.Threading.Thread(inspectionMonitorProc));
//                ThreadManager.AddThread(inspectionMonitorThread);
//                inspectionMonitorThread.WorkingThread.Start();
//            }
//        }

//        private void SetupUnitManager(int bufferSize)
//        {
//            //unitManager.Dispose();

//            //Data.Model model = (Data.Model)SystemManager.Instance().CurrentModel;
//            //VisionProbe visionProbe = model.GetVisionProbe();
//            //SheetCheckerParam sheetCheckerParam = visionProbe.InspAlgorithm.Param as SheetCheckerParam;

//            //unitManager.ProcessBufferManager.CreateBuffer(sheetCheckerParam.GetSheetSizePx(), bufferSize);

//            //// Process Step
//            //InspectUnit processStep = new InspectUnit(0, "ProcessStep");
//            //processStep.inspAlgorithm = new SheetCheckerStepCalculate(sheetCheckerParam);
//            //processStep.UnitInspected = unitManager.CalculateStep_PipelineInspected;
//            //unitManager.Add(processStep);

//            //// Detect Step
//            //InspectUnit detectStep = new InspectUnit(1, "DetectStep");
//            //detectStep.inspAlgorithm = new SheetCheckerStepDetect(sheetCheckerParam);
//            //detectStep.UnitInspected = unitManager.DetectStep_PipelineInspected;
//            //unitManager.Add(detectStep);

//            //// Save Step
//            //InspectUnit saveStep = new InspectUnit(2, "SaveStep");
//            //saveStep.inspAlgorithm = new SheetCheckerStepSave(sheetCheckerParam);
//            //saveStep.UnitInspected = unitManager.SaveStep_PipelineInspected;
//            //saveStep.MaxParamCount = 1;
//            //unitManager.Add(saveStep);

//            //unitManager.ProductInspected = this.InspectionThread_TargetGroupInspected;
//            //unitManager.Run(); //thread run//
//        }


//        public override void ExitWaitInspection()
//        {
//            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
//            imageDeviceHandler.Stop();

//            base.ExitWaitInspection();

//            this.requestStop = true;

//            if (grabProcesser != null)
//                grabProcesser.Stop();

//            //pipelineManager.Stop(grabProcesser);

//            //if (inspectionMonitorThread != null)
//            //{
//            //    inspectionMonitorThread.RequestStop = true;
//            //    while (inspectionMonitorThread.WorkingThread.IsAlive)
//            //        Thread.Sleep(10);
//            //}
//            //ThreadManager.RemoveThread(inspectionMonitorThread);
//            //inspectionMonitorThread = null;

//            //if (grabProcesser != null)
//            //    grabProcesser.Dispose();
//            //grabProcesser = null;

//            SystemState.Instance().SetIdle();
//        }

//        public override bool IsOnInspect()
//        {
//            bool onInspect = false;

//            if (grabProcesser != null)
//                onInspect |= grabProcesser.IsRunning;

//            if (unitManager != null)
//                onInspect |= unitManager.IsRunning();

//            return onInspect;
//        }

//        public override bool IsWaitIntpect()
//        {
//            bool isWait = base.IsWaitIntpect();

//            if (grabProcesser != null)
//                isWait &= !grabProcesser.IsBusy;

//            if (unitManager != null)
//                isWait &= !unitManager.IsBusy();

//            return isWait;
//        }

//        private void WaitInspectionDone()//not used
//        {
//            do
//            {
//                UpdateSystemState();
//                //Thread.Sleep(10);
//                Application.DoEvents();
//                OpState opState = SystemState.Instance().OpState;
//                if (opState == OpState.Wait || opState == OpState.Idle)
//                    break;
//            } while (true);
//        }

//        private void inspectionMonitorProc()
//        {
//            int uiUpdateTerm = 0;
//            LogHelper.Debug(LoggerType.Inspection, "GravureInspectRunner::inspectionMonitorProc - Start");
//            while (inspectionMonitorThread?.RequestStop == false)
//            {
//                System.Threading.Thread.Sleep(10);

//                if (grabProcesser != null && grabProcesser.IsFullImageGrabbed())
//                {
//                    LogHelper.Debug(LoggerType.Inspection, "GravureInspectRunner::inspectionMonitorProc - StartInspection Activate");

//                    StartInspection(InspectState.Run);
//                    //grabProcesser.GetLastSheetImageSet().Dispose();
//                }
//                else if (this.requestStop)
//                {
//                    if (grabProcesser != null && grabProcesser.IsRunning == false)
//                    {
//                        grabProcesser.Dispose();
//                        grabProcesser = null;
//                        unitManager.Stop();
//                    }
//                    else if (unitManager.IsRunning() == false)
//                    {
//                        Thread.Sleep(50);
//                        unitManager.Dispose();
//                        //pipelineManager = null;
//                        this.requestStop = false;
//                    }
//                }

//                uiUpdateTerm++;
//                if (uiUpdateTerm > 5)
//                {
//                    UpdateSystemState();
//                    uiUpdateTerm = 0;
//                }
//            }
//            LogHelper.Debug(LoggerType.Inspection, "GravureInspectRunner::inspectionMonitorProc - Stop");
//        }

//        private void UpdateSystemState()
//        {
//            if (this.grabProcesser != null)
//                SystemManager.Instance().MainForm.UpdateControl("0", this.grabProcesser.GetBufferCount().ToString());

//            if (unitManager != null)
//            {
//                for (int i = 0; i < this.unitManager.Count; i++)
//                    SystemManager.Instance().MainForm.UpdateControl((i + 1).ToString(), this.unitManager.GetBufferCount(i).ToString());
//            }

//            if (SystemState.Instance().OpState == OpState.Idle)
//                return;

//            if (unitManager != null && unitManager.IsBusy())
//                SystemState.Instance().SetInspectState(InspectState.Run);
//            else
//                SystemState.Instance().SetWait();

//        }

//        public override bool StartInspection(InspectState inspectState)
//        {
//            Data.SheetImageSet sheetImageSet = grabProcesser.GetLastSheetImageSet();

//            Data.InspectionResult inspectionResult = inspectRunnerExtender.BuildInspectionResult() as Data.InspectionResult;
//            if (inspectionResult == null)
//            {
//                LogHelper.Debug(LoggerType.Inspection, "Inspection cancel");
//                return false;
//            }

//            SheetCheckerInspectParam sheetCheckerInspectParam = new SheetCheckerInspectParam(sheetImageSet, null, inspectionResult,
//                new AlgorithmInspectParam(null, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, new DebugContext(OperationSettings.Instance().SaveDebugImage, Path.Combine(PathSettings.Instance().Temp, string.Format("Sheet{0}", sheetImageSet.sheetNo)))));
//            SheetCheckerAlgorithmResult sheetCheckerAlgorithmResult = new SheetCheckerAlgorithmResult();
//            UniScanGInspectUnitParam unitParam = new UniScanGInspectUnitParam(sheetCheckerInspectParam, sheetCheckerAlgorithmResult);

//            LogHelper.Debug(LoggerType.Inspection, "GravureInspectRunner::StartInspection");
//            //VisionProbe visionProbe = GetVisionProbe();
//            //SheetCheckerParam sheetCheckerParam = (SheetCheckerParam)visionProbe.InspAlgorithm.Param;

//            if (this.unitManager.StartInspect(unitParam) == false)
//            {
//                inspectionResult.Judgment = Judgment.Skip;
//                InspectionThread_TargetGroupInspected(null, inspectionResult, null);
//                //ProductInspected(inspectionResult);
//            }

//            return true;
//        }

//        public override void StopProcess()
//        {
//            base.StopProcess();
//        }

//        public override void RetryInspection()
//        {
//            //base.RetryInspection();
//        }

//        public override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
//        {
//            LogHelper.Debug(LoggerType.Grab, "GravureInspectRunner::ImageGrabbed");

//            //Image2D grabImage = (Image2D)imageDevice.GetGrabbedImage();
//            //if (grabImage.IsUseIntPtr()==false)
//            //{
//            //    grabImage.ConverFromData();
//            //}

//            if (grabProcesser != null)
//            {
//                ICameraExtender cameraExtender = imageDevice as ICameraExtender;
//                AlgoImage algoImage = cameraExtender.GetAlgoImage(ptr);
//                grabProcesser.OnNewImageArrived(algoImage);
//            }
//        }

//        public void Dispose()
//        {
//            grabProcesser.Dispose();
//        }

//        public override void InspectionThread_TargetGroupInspected(TargetGroup targetGroup, DynMvp.InspData.InspectionResult inspectionResult, DynMvp.InspData.InspectionResult objectInspectionResult)
//        {
//            LogHelper.Debug(LoggerType.Inspection, "InspectionThread_InspectionFinished");

//            inspectionResult.InspectionEndTime = DateTime.Now;

//            if (targetGroup == null)
//            {
//                int inspectionStep = UniScanGSettings.Instance().InspectorInfo.CamIndex;
//                targetGroup = SystemManager.Instance().CurrentModel.GetInspectionStep(inspectionStep).GetTargetGroup(0);
//            }

//            if (inspectionResult == null)
//                inspectionResult = this.inspectionResult;

//            base.InspectionThread_TargetGroupInspected(targetGroup, inspectionResult, objectInspectionResult);

//            //UpdateSystemState();
//        }
//    }
//}
