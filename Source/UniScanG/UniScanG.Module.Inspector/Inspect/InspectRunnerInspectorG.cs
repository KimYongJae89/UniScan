using System;
using System.Drawing;
using System.IO;

using DynMvp.Base;
//using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using UniEye.Base.Inspect;
using System.Threading;
using System.Windows.Forms;
using DynMvp.Vision;
using DynMvp.UI;
using DynMvp.UI.Touch;
using UniEye.Base.Device;
using UniScanG.Inspect;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Trainer;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Data;
using UniScanG.Vision;
using UniScanG.Data;
using UniScanG.Data.Model;
using UniScanG.Common;
//using DynMvp.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using UniScanG.Gravure.UI.Inspect.Inspector;
using UniScanG.Gravure.Vision;
using System.Text;
using UniScanG.Gravure.Vision.Extender;
using System.Xml;
using System.Collections.Generic;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Device;
using System.Linq;
using UniScanG.Module.Inspector.Settings.Inspector;
using System.Collections.Concurrent;

namespace UniScanG.Module.Inspector.Inspect
{
    public class InspectRunnerInspectorG : UniScanG.Inspect.InspectRunner
    {
        Inspect.DebugRawImageSaver imageSaver = null;

        //Gravure.Data.ResultCollector collector;
        StreamWriter inspTimeLoggerStreamWriter = null;
        const string TimeLoggerName = "InspTime.csv";

        public ConcurrentQueue<Tuple<ImageDevice, IntPtr>> GrabbedQueue { get; } = new ConcurrentQueue<Tuple<ImageDevice, IntPtr>>();
        public BufferUploadThread BufferUploadThread { get; private set; }

        public InspectRunnerInspectorG() : base()
        {
            this.grabProcesser = BuildSheetGrabProcesser(true);
            this.grabProcesser.StartInspectionDelegate += grabProcesser_StartInspection;
            this.BufferUploadThread = null;

            this.inspectObserver = new InspectObserver();
            this.alarmManager = new AlarmManager();
        }

        ~InspectRunnerInspectorG()
        {
        }

        public override GrabProcesserG BuildSheetGrabProcesser(bool useLengthFilter)
        {
            DynMvp.Devices.FrameGrabber.Camera camera = (DynMvp.Devices.FrameGrabber.Camera)SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0);
            switch(camera.CameraInfo.FrameType)
            {
                case DynMvp.Devices.FrameGrabber.CameraInfo.EFrameType.Continuous:
                    return new SheetGrabProcesserG()
                    {
                        UseLengthFilter = useLengthFilter,
                        SplitExactPattern = InspectorSystemSettings.Instance().SplitExactPattern,
                        Algorithm = (SheetFinderBase)AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName)
                    };

                case DynMvp.Devices.FrameGrabber.CameraInfo.EFrameType.Partial:
                    return new SheetGrabProcesserSAG()
                    {
                        UsetLengthFilter = useLengthFilter,
                        Algorithm = (SheetFinderBase)AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName)
                    };
            }
            return null;    
        }

        protected ImageDevice GetImageDevice(int deviceIndex)
        {
            return SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(deviceIndex);
        }

        public override bool EnterWaitInspection()
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectRunnerInspectorG::EnterWaitInspection");

            ProductionG production = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
            if (production == null)
            {
                SystemState.Instance().SetAlarm("Production is not created.");
                //throw new Exception("Production is not created.");
                return false;
            }

            UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = AdditionalSettings.Instance() as UniScanG.Gravure.Settings.AdditionalSettings;
            if (production.LineSpeedMpm > additionalSettings.MaximumLineSpeed)
            {
                //throw new Exception("Speed is too fast.");
                SystemState.Instance().SetAlarm("Speed is too fast.");
                return false;
            }
            else if (production.LineSpeedMpm < additionalSettings.MinimumLineSpeed)
            {
                //throw new Exception("Speed is too slow.");
                SystemState.Instance().SetAlarm("Speed is too slow.");
                return false;
            }

            SystemManager.Instance().UiController.ChangeTab(UniScanG.UI.MainTabKey.Inspect.ToString());

            if (SystemManager.Instance().DeviceController.OnEnterWaitInspection("Laser") == false)
                return false;


            // Create Buffer
            float scaleFactorF = SystemManager.Instance().CurrentModel.ScaleFactorF;
            bool isMultiLayerBuffer = additionalSettings.MultiLayerBuffer;
            string readyFailMessage = "";
            bool halfScale = false;

            if (additionalSettings.AutoResolutionScale && production.LineSpeedMpm >= 60)
                halfScale = true;

            new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Ready")).Show(ConfigHelper.Instance().MainForm, () =>
            {
                CalculatorBase calculator = (CalculatorBase)AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName);
                Detector detector = (Detector)AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName);
                Watcher watcher = (Watcher)AlgorithmPool.Instance().GetAlgorithm(Watcher.TypeName);
                try
                {
                    Model model = SystemManager.Instance().CurrentModel;

                    // Build Buffer
                    LogHelper.Debug(LoggerType.Inspection, $"InspectRunnerInspectorG::EnterWaitInspection - Build Buffer");
                    readyFailMessage = "Buffer Initialize Fail.";

                    int bufferCount = AlgorithmSetting.Instance().InspBufferCount;
                    Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
                    Size bufferSize = Size.Round(calibration.WorldToPixel(DrawingHelper.Mul(model.CalculatorModelParam.SheetSizeMm, 1000)));
                    for (int i = 0; i < bufferCount; i++)
                    {
                        ProcessBufferSetG processBufferSetG = calculator.CreateProcessingBuffer(scaleFactorF, isMultiLayerBuffer, bufferSize.Width, (int)(bufferSize.Height * 1.05));
                        processBufferSetG.BuildBuffers(halfScale);
                        this.processBufferManager.AddProcessBufferSet(processBufferSetG);
                    }

                    this.trasnferTaskList.Clear();

                    // Prepare Algorithm
                    LogHelper.Debug(LoggerType.Inspection, $"InspectRunnerInspectorG::EnterWaitInspection - Prepare Algorithm");
                    readyFailMessage = "Algorithm Initialize Fail.";
                    calculator.PrepareInspection();
                    detector.PrepareInspection();
                    watcher.PrepareInspection();

                    // Init Unit
                    LogHelper.Debug(LoggerType.Inspection, $"InspectRunnerInspectorG::EnterWaitInspection - Init Unit");
                    readyFailMessage = "Unit Initialize Fail.";
                    SetupUnitManager();

                    readyFailMessage = "";
                }
                catch (Exception ex)
                {
                    readyFailMessage = ex.Message;
                    LogHelper.Error(LoggerType.Error, $"InspectRunnerInspectorG::EnterWaitInspection - {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                    return;
                }
            });

            if (!string.IsNullOrEmpty(readyFailMessage))
            {
                SystemState.Instance().SetAlarm(readyFailMessage);
                //MessageForm.Show(null, StringManager.GetString("."));
                //SystemState.Instance().SetIdle();
                return false;
            }

            this.inspectObserver.Clear();

            this.BufferUploadThread?.Stop();
            this.BufferUploadThread = new Inspect.BufferUploadThread("BufferUploadThread", GrabDoneQueueManageProc);
            this.BufferUploadThread.Start();

            {
                int camIndex = SystemManager.Instance().ExchangeOperator.GetCamIndex();
                int clientIndex = SystemManager.Instance().ExchangeOperator.GetClientIndex();
                SystemManager.Instance().ExchangeOperator.SendCommand(Common.Exchange.ExchangeCommand.I_LOADFACTOR, camIndex.ToString(), clientIndex.ToString(), "-1");
            }

            SystemState.Instance().SetWait();
            return true;
        }

        public override void EnterPauseInspection()
        {
            base.EnterPauseInspection();

            // 카메라 끔.
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.RemoveImageGrabbed(this.grabProcesser.ImageGrabbed);
            imageDeviceHandler.Stop();

            this.inspectUnitManager.Stop();
            this.grabProcesser.Stop();

            this.processBufferManager.ReturnAll();
            SystemState.Instance().SetWait();
        }

        public override bool PostEnterWaitInspection()
        {
            // 현재 티칭 값 저장
            SaveTeachParam();
            this.alarmManager.ClearData();

            this.inspectUnitManager.Run();

            // 카메라 및 GrabProcesser 초기화
            ProductionG productionG = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
            float lineSpeedMpm = productionG == null ? 0 : productionG.LineSpeedMpm;

            ((DeviceControllerG)SystemManager.Instance().DeviceController).SetAsyncMode(lineSpeedMpm);

            if (this.grabProcesser is SheetGrabProcesserG)
            {
                SheetGrabProcesserG sheetGrabProcesserG = (SheetGrabProcesserG)this.grabProcesser;
                sheetGrabProcesserG.Algorithm = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName) as SheetFinderBase;

                UniScanG.Gravure.Settings.AdditionalSettings additionalSettings = AdditionalSettings.Instance() as UniScanG.Gravure.Settings.AdditionalSettings;
                if (additionalSettings != null)
                {
                    sheetGrabProcesserG.PrecisionTimeTrace = additionalSettings.PrecisionTimeTrace;
                    sheetGrabProcesserG.SetDebugMode(additionalSettings.DebugSheetGrabProcesser);
                }
            }

            this.grabProcesser.Start();

            // 시간 로그 저장 스트림 생성
            this.inspTimeLoggerStreamWriter?.Close();
            this.inspTimeLoggerStreamWriter?.Dispose();
            Directory.CreateDirectory(productionG.ResultPath);
            this.inspTimeLoggerStreamWriter = new StreamWriter(Path.Combine(productionG.ResultPath, TimeLoggerName), true);

            this.imageSaver?.Abort();
            this.imageSaver = new Inspect.DebugRawImageSaver("DebugRawImageSaver");
            this.imageSaver?.Start();

            SystemManager.Instance().DeviceBox.ImageDeviceHandler.AddImageGrabbed(this.grabProcesser.ImageGrabbed);
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetStepLight(0, 0); // 가상카메라 이미지인덱스 초기화
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabMulti();
            //SystemManager.Instance().DeviceBox.ImageDeviceHandler.GrabOnce();

            SystemState.Instance().SetInspect();
            return true;
        }

        private bool SaveTeachParam()
        {
            ProductionG productionG = (ProductionG)SystemManager.Instance().ProductionManager.CurProduction;
            if (productionG == null)
                return false;

            string savePath = Path.Combine(SystemManager.Instance().CurrentModel.ModelPath, "InspData", productionG.LotNo);
            if (string.IsNullOrEmpty(savePath))
                return false;

            Directory.CreateDirectory(savePath);

            // Save TeachData XML
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElement = xmlDoc.CreateElement("Root");
            xmlDoc.AppendChild(xmlElement);

            SheetFinderBase.SheetFinderBaseParam.SaveParam(xmlElement, SheetFinderBase.TypeName);
            TrainerBase.TrainerParam.SaveParam(xmlElement, TrainerBase.TypeName);
            CalculatorBase.CalculatorParam.SaveParam(xmlElement, CalculatorBase.TypeName);
            Detector.DetectorParam.SaveParam(xmlElement, Detector.TypeName);
            Watcher.WatcherParam.SaveParam(xmlElement, Watcher.TypeName);

            // CM에 보내는 티칭 정보에는 영역 데이터 포함 안 한다.
            //XmlHelper.SetValue(calcElement, "IncludeChipRegionData", false); 

            string xmlFileName = string.Format("Teach.xml");
            xmlDoc.Save(Path.Combine(savePath, xmlFileName));

            // Save TeachData Image
            Model model = SystemManager.Instance().CurrentModel;
            if (model == null)
                return false;

            string[] prevImages = new string[]
            {
                model.GetPreviewImagePath(""),
                model.GetPreviewImagePath("0")
            };

            Array.ForEach(prevImages, f =>
            {
                try
                {
                    if (File.Exists(f))
                    {
                        string dst = Path.Combine(savePath, Path.GetFileName(f));
                        LogHelper.Debug(LoggerType.Operation, string.Format("SaveTeachParam - src: {0}, dst: {1}", f, dst));
                        File.Copy(f, dst);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Operation, string.Format("SaveTeachParam - {0}", ex.Message));
                }
            });

            return true;
        }

        private void grabProcesser_StartInspection(ImageDevice imageDevice, IntPtr ptr)
        {
            if (this.GrabbedQueue != null)
            {
                LogHelper.Debug(LoggerType.Inspection, string.Format("InspectRunnerInspectorG::grabProcesser_StartInspection - SheetNo {0}, QueueCount: {1}", (int)ptr, this.GrabbedQueue.Count));
                this.GrabbedQueue.Enqueue(new Tuple<ImageDevice, IntPtr>(imageDevice, ptr));
            }
        }

        private void GrabDoneQueueManageProc()
        {
            while (!this.BufferUploadThread.RequestStop)
            {
                Tuple<ImageDevice, IntPtr> tuple;
                if (!GrabbedQueue.TryDequeue(out tuple))
                {
                    Thread.Sleep(50);
                    continue;
                }
                this.BufferUploadThread.WorkBegin();
                Task.Run(() => StartInspection(tuple.Item1, tuple.Item2));
                //StartInspection(tuple.Item1, tuple.Item2);
                this.BufferUploadThread.WorkEnd();
            }
        }

        private void StartInspection(ImageDevice imageDevice, IntPtr ptr)
        {
            try
            {
                Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();

                // GrabProcesser에서 시트 1장을 그랩함.
                LogHelper.Debug(LoggerType.Inspection, string.Format("InspectRunnerInspectorG::StartInspection - SheetNo {0}", (int)ptr));

                SheetImageSet sheetImageSet = this.grabProcesser.GetSheetImageSet((int)ptr);

                UniScanG.Data.Inspect.InspectionResult inspectionResult = BuildInspectionResult() as UniScanG.Data.Inspect.InspectionResult;
                inspectionResult.ImageGrabbedTime = sheetImageSet.DateTime;
                LogHelper.Debug(LoggerType.Inspection, string.Format("InspectRunnerInspectorG::StartInspection - ImageGrabbedTime: {0}", inspectionResult.ImageGrabbedTime.ToString("HH:mm:ss")));
                //InspectionResult inspectionResult = BuildInspectionResult(sheetImageSet.SheetNo.ToString());

                // Client Index 확인
                int camIndex = SystemManager.Instance().ExchangeOperator.GetCamIndex();
                int clientIndex = SystemManager.Instance().ExchangeOperator.GetClientIndex();
                int inspectionNo = int.Parse(inspectionResult.InspectionNo);
                this.inspectObserver.AddData(0, 0, inspectionNo);

                bool skipRun = (clientIndex >= 0 && inspectionNo % 2 != clientIndex);
                
                UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;
                ProcessBufferSetG bufferSet = this.processBufferManager.Request() as ProcessBufferSetG;
                DebugContextG debugContextG = new DebugContextG(new DebugContext(OperationSettings.Instance().SaveDebugImage, PathSettings.Instance().Temp));
                debugContextG.LotNo = SystemManager.Instance().ProductionManager.CurProduction?.LotNo;
                debugContextG.PatternId = inspectionNo;

                SheetInspectParam inspectParam = new SheetInspectParam(model, bufferSet, calibration, debugContextG);
                inspectParam.OnDisposed += new SheetInspectParamEventHandler(f => this.processBufferManager.Return(f.ProcessBufferSet));

                InspectOption inspectionOption = this.InspectRunnerExtender.BuildInspectOption(imageDevice, ptr);
                UnitInspectItem unitInspectItem = new UnitInspectItem(inspectParam, inspectionResult, inspectionOption);
                bool runOk = false;
                if (!skipRun && bufferSet != null)
                {
                    runOk = bufferSet.Upload(sheetImageSet, debugContextG);
                    if(runOk)
                    {
                        inspectionResult.SetOffsetStruct(bufferSet.OffsetStructSet);

                        runOk = this.inspectUnitManager.StartInspect(unitInspectItem);
                        this.imageSaver.BeginSave(sheetImageSet, inspectionNo);
                    }
                }
                sheetImageSet.Dispose();

                if (runOk)
                {
                    LogHelper.Info(LoggerType.Inspection, $"InspectRunnerInspectorG::StartInspection - Running ({inspectionNo})");
                }
                else
                {
                    inspectionResult.Judgment = Judgment.Skip;
                    LogHelper.Info(LoggerType.Inspection, $"InspectRunnerInspectorG::StartInspection - Skip ({inspectionNo})");
                    this.processBufferManager.Return(bufferSet);

                    unitInspectItem.Dispose();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, ex.Message);

                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
                LogHelper.Error(LoggerType.Error, "Line: " + trace.GetFrame(0).GetFileLineNumber());

                LogHelper.Error(LoggerType.Error, ex.StackTrace);
            }
        }

        protected override void SetupUnitManager()
        {
            this.inspectUnitManager.Dispose();

            // calculator
            Algorithm calculateAlgorithm = AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName);
            if (calculateAlgorithm != null)
            {
                InspectUnit calculateInspectUnit = new InspectUnit(CalculatorBase.TypeName, calculateAlgorithm);
                calculateInspectUnit.UnitInspected = CalculateInspectUnit_UnitInspected;
                this.inspectUnitManager.Add(calculateInspectUnit);
            }

            // detector
            Algorithm detectAlgorithm = AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName);
            if (detectAlgorithm != null)
            {
                InspectUnit detectInspectUnit = new InspectUnit(Detector.TypeName, detectAlgorithm);
                detectInspectUnit.UnitInspected = DetectInspectUnit_UnitInspected;
                this.inspectUnitManager.Add(detectInspectUnit);
            }

            // Watcher
            Algorithm watchAlgorithm = AlgorithmPool.Instance().GetAlgorithm(Watcher.TypeName);
            if (watchAlgorithm != null)
            {
                InspectUnit watchInspectUnit = new InspectUnit(Watcher.TypeName, watchAlgorithm);
                watchInspectUnit.UnitInspected = WatchInspectUnit_UnitInspected;
                this.inspectUnitManager.Add(watchInspectUnit);
            }

            inspectUnitManager.AllUnitInspected = inspectUnitManager_AllUnitInspected;
        }

        private void CalculateInspectUnit_UnitInspected(UnitInspectItem unitInspectItem)
        {
            ProcessBufferSetG processBufferSetG = ((SheetInspectParam)unitInspectItem.AlgorithmInspectParam).ProcessBufferSet as ProcessBufferSetG;
            processBufferSetG.Download();
        }

        private void DetectInspectUnit_UnitInspected(UnitInspectItem unitInspectItem)
        {
        }

        private void WatchInspectUnit_UnitInspected(UnitInspectItem unitInspectItem)
        {
        }

        CancellationTokenSource exportCancellationTokenSource;
        private void inspectUnitManager_AllUnitInspected(UnitInspectItem unitInspectItem)
        {
            InspectionResult inspectionResult = unitInspectItem.InspectionResult;
            LogHelper.Info(LoggerType.Inspection, string.Format("InspectRunnerInspectorG::inspectUnitManager_AllUnitInspected - InspectionNo is {0}", inspectionResult.InspectionNo));
            
            SheetInspectParam sheetAlgorithmInspectParam = (SheetInspectParam)unitInspectItem.AlgorithmInspectParam;
            DebugContextG debugContextG = sheetAlgorithmInspectParam.DebugContext as DebugContextG;
            //LogHelper.Debug(LoggerType.Inspection, string.Format("InsepctionTime {0} {1}", debugContextG.PatternId, debugContextG.ProcessTimeLog.GetExportData()));

            ProcessBufferSetG processBufferSet = sheetAlgorithmInspectParam.ProcessBufferSet as ProcessBufferSetG;
            processBufferSet?.WaitDone();
            sheetAlgorithmInspectParam.Dispose();

            bool ok = inspectionResult.AlgorithmResultLDic.TryGetValue(CalculatorBase.TypeName, out AlgorithmResult algorithmResult);
            if (ok)
                ((CalculatorResult)algorithmResult).PrevImage = (Bitmap)processBufferSet.PrevBitmap?.Clone();

            // 원본 이미지 저장?
            //SaveImage(processBufferSet.AlgoImage, unitInspectItem.InspectionResult);

            // 버퍼 반환
            processBufferManager.Return(processBufferSet);

            inspectionResult.UpdateJudgement();
            UniScanG.Data.Production productionG = (UniScanG.Data.Production)SystemManager.Instance().ProductionManager.CurProduction;

            // 카운트 업데이트
            if (inspectionResult.Judgment == Judgment.Skip)
            {
                productionG.Update(null);
            }
            else
            {
                SystemManager.Instance().DeviceController.OnProductInspected(inspectionResult);

                productionG.Update(inspectionResult);

                //exportCancellationTokenSource?.Cancel();
                exportCancellationTokenSource = new CancellationTokenSource();
                Task saveTask = Task.Run(() =>
                {
                    // 저장
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    SystemManager.Instance().ExportData(inspectionResult, exportCancellationTokenSource);
                    sw.Stop();
                    inspectionResult.ExportTime = sw.Elapsed;

                    // 서버에 통보
                    string path = productionG.GetResultPath("");
                    IClientExchangeOperator clientExchangeOperator = (IClientExchangeOperator)SystemManager.Instance().ExchangeOperator;
                    clientExchangeOperator.SendInspectDone(inspectionResult.InspectionNo, path, inspectionResult.Judgment);

                    WriteTimeLog(productionG.GetResultPath(), debugContextG);

                    // UI 업데이트
                    InspectDone(inspectionResult);
                });

                lock (trasnferTaskList)
                {
                    trasnferTaskList.Add(saveTask);
                    trasnferTaskList.RemoveAll(f => f.IsCompleted);
                }

                this.inspectObserver.AddData(1, 0, int.Parse(inspectionResult.InspectionNo));
            }

            unitInspectItem.Dispose();
            //StringBuilder infoStringBuilder = new StringBuilder();
            //infoStringBuilder.Append(string.Format("Sheet,{0}", unitInspectItem.InspectionResult.InspectionNo));
            //infoStringBuilder.Append(string.Format(",Result,{0}", unitInspectItem.InspectionResult.Judgment.ToString()));
            //if (unitInspectItem.InspectionResult.AlgorithmResultLDic.ContainsKey(CalculatorBase.TypeName))
            //    infoStringBuilder.Append(string.Format(",Calculator,{0}", unitInspectItem.InspectionResult.AlgorithmResultLDic[CalculatorBase.TypeName].SpandTime.ToString("ss\\.fff")));
            //if (unitInspectItem.InspectionResult.AlgorithmResultLDic.ContainsKey(Detector.TypeName))
            //    infoStringBuilder.Append(string.Format(",Detector,{0}", unitInspectItem.InspectionResult.AlgorithmResultLDic[Detector.TypeName].SpandTime.ToString("ss\\.fff")));
            //LogHelper.Info(LoggerType.Inspection, infoStringBuilder.ToString());

            //InspectDone(unitInspectItem.InspectionResult);
            if (this.inspectUnitManager.IsBusy() == false)
                SystemState.Instance().SetInspectState(InspectState.Wait);
        }

        public float GetLoadFactor()
        {
            float bufferLoadFactor = 0;
            if (this.BufferUploadThread != null)
                bufferLoadFactor = this.BufferUploadThread.GetLoadFactor();

            float unitLoadFactor = this.InspectUnitManager.GetLoadFactor();

            return Math.Max(bufferLoadFactor, unitLoadFactor);
        }

        private void SaveImage(AlgoImage algoImage, InspectionResult inspectionResult )
        {
            //if (!Directory.Exists(resultPath))
            //    Directory.CreateDirectory(resultPath);
            string resultPath = inspectionResult.ResultPath;
            string path = Path.Combine(resultPath, $"Image_{inspectionResult.InspectionNo}.bmp");
            try
            {
                LogHelper.Debug(LoggerType.Inspection, $"InspectRunnerInspectorG::SaveOrgImage - resultPath: {path}");
                algoImage.Save(path);
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, $"InspectRunnerInspectorG::SaveOrgImage - {ex.Message}");
            }
        }

        private void WriteTimeLog(string resultPath, DebugContextG debugContextG)
        {
            string logPath = Path.Combine(resultPath, "InspTime.log");

            if (this.inspTimeLoggerStreamWriter == null)
                return;

            if (this.inspTimeLoggerStreamWriter.BaseStream.Position == 0)
            {
                string headerString = string.Format("No,{0}", ProcessTimeLog.GetExportHeader());
                this.inspTimeLoggerStreamWriter.WriteLine(headerString);
            }

            string exportString = string.Format("{0},{1}", debugContextG.PatternId, debugContextG.ProcessTimeLog.GetExportData());
            this.inspTimeLoggerStreamWriter.WriteLine(exportString);
            this.inspTimeLoggerStreamWriter.Flush();
        }

        public override void PreExitWaitInspection()
        {
            SystemManager.Instance().DeviceController.OnExitWaitInspection();

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.RemoveImageGrabbed(this.grabProcesser.ImageGrabbed);
            imageDeviceHandler.Stop();
            imageDeviceHandler.SetTriggerMode(TriggerMode.Hardware);
            //imageDeviceHandler.SetTriggerMode(TriggerMode.Software);

            this.imageSaver?.Stop();

        }

        public override void ExitWaitInspection()
        {
            PreExitWaitInspection();
            LogHelper.Debug(LoggerType.Inspection, "InspectRunner::ExitWaitInspection");

            //ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            //foreach (ImageDevice imageDevice in imageDeviceHandler)
            //    imageDevice.ImageGrabbed -= grabProcesser.ImageGrabbed;

            LogHelper.Debug(LoggerType.Inspection, "InspectRunner::ExitWaitInspection - START EXIT");
            SimpleProgressForm form = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Wait"));
            form.Show(() =>
            {
                LogHelper.Debug(LoggerType.Inspection, "InspectRunner::ExitWaitInspection - grabProcesser.Stop");
                this.grabProcesser.Stop();

                LogHelper.Debug(LoggerType.Inspection, "InspectRunner::ExitWaitInspection - BufferUploadThread.Stop");
                this.BufferUploadThread?.Stop();

                LogHelper.Debug(LoggerType.Inspection, "InspectRunner::ExitWaitInspection - inspectUnitManager.Stop");
                this.inspectUnitManager.Stop();
                LogHelper.Debug(LoggerType.Inspection, "InspectRunner::ExitWaitInspection - inspectUnitManager.Dispose");
                this.inspectUnitManager.Dispose();

                bool waiting = true;
                do
                {
                    LogHelper.Debug(LoggerType.Inspection, "InspectRunner::ExitWaitInspection - processBufferManager.WaitDisposable");
                    waiting = !this.processBufferManager.WaitDisposable(1000);
                    //LogHelper.Debug(LoggerType.Inspection, "InspectRunner::ExitWaitInspection - processBufferManager.ReturnAll");
                    //this.processBufferManager.ReturnAll();
                } while (waiting);

                LogHelper.Debug(LoggerType.Inspection, "InspectRunner::ExitWaitInspection - grabProcesser.Dispose");
                this.grabProcesser.Dispose();
                LogHelper.Debug(LoggerType.Inspection, "InspectRunner::ExitWaitInspection - processBufferManager.Dispose");
                this.processBufferManager.Dispose();

                while (this.GrabbedQueue.TryDequeue(out var v)) ;
            });

            LogHelper.Debug(LoggerType.Inspection, "InspectRunner::ExitWaitInspection - END EXIT");
            SystemManager.Instance().ProductionManager.Save();
            this.alarmManager.ClearSignal();

            // Clear Algorithm
            AlgorithmPool.Instance().GetAlgorithm(CalculatorBase.TypeName)?.ClearInspection();
            AlgorithmPool.Instance().GetAlgorithm(Detector.TypeName)?.ClearInspection();

            //SystemState.Instance().SetInspectState(InspectState.Wait);
            SystemState.Instance().SetIdle();

            this.inspTimeLoggerStreamWriter?.Flush();
            this.inspTimeLoggerStreamWriter?.Close();
            this.inspTimeLoggerStreamWriter?.Dispose();
            this.inspTimeLoggerStreamWriter = null;
        }

        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            base.Inspect(imageDevice, ptr, inspectionResult, inspectionOption);
        }

        public override void Dispose()
        {
            grabProcesser.Dispose();
        }
    }
}
