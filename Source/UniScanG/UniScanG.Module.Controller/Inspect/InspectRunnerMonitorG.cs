using System;
using System.Drawing;
using System.IO;

using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.InspData;
using UniEye.Base.Data;
//using UniEye.Base.Settings;
using UniEye.Base.Inspect;
using System.Threading;
using System.Windows.Forms;
using DynMvp.Vision;
using DynMvp.UI;
using DynMvp.UI.Touch;
using UniEye.Base.Device;
using UniScanG.Inspect;
using System.Collections.Generic;
using UniScanG.Vision;
using UniScanG.Common.Exchange;
using DynMvp.Authentication;
using UniScanG.Data;
using System.Diagnostics;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Settings;
using System.Threading.Tasks;
using DynMvp.Device.Serial;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Light;
using UniScanG.Gravure.UI.Inspect.Monitor;
using System.Text;
using UniScanG.Gravure.Data;
using UniEye.Base.MachineInterface;
using System.ComponentModel;
using DynMvp.Devices.MotionController;
using System.Linq;
using UniScanG.Gravure.Settings;
using UniScanG.Module.Controller.MachineIF;
using UniScanG.Gravure.Device;
using UniScanG.Gravure.Inspect;
//using UniScanG.MachineIF;

namespace UniScanG.Module.Controller.Inspect
{
    public class InspectRunnerMonitorG : UniScanG.Inspect.InspectRunner, IModelListener
    {
        bool stopThread = true;
        Thread thread = null;
        object waitInspectLocker = new object();

        Dictionary<int, List<Tuple<string, string>>> resultDic = new Dictionary<int, List<Tuple<string, string>>>();
        InspectorObj[] inspectorObjs = null;
        Dictionary<int, bool> modelTeachedDic = new Dictionary<int, bool>();

        public List<Task> WaitTaskList { get => this.waitTaskList; }
        protected List<Task> waitTaskList = new List<Task>();

        List<InspectionResult> inspectionResultList = new List<InspectionResult>();

        private ProductionG curProduction = null;
        private int inspNoSrc = 0;

        public InspectRunnerMonitorG() : base()
        {
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            SystemManager.Instance().ProductionManager.OnLotChanged += ProductionManager_OnLotChanged;

            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            this.inspectorObjs = server.Inspectors;

            SystemManager.Instance().DeviceController.OnExitWaitInspection();

            //this.AddInspectDoneDelegate(OnInspectDone);

            this.inspectObserver = new InspectObserver(this.inspectorObjs);
            this.alarmManager = new UniScanG.Module.Controller.Inspect.AlarmManager();
        }

        protected override void SetupUnitManager() { }

        public override GrabProcesserG BuildSheetGrabProcesser(bool useLengthFilter)
        {
            return null;
        }

        public override bool EnterWaitInspection()
        {
            try
            {
                if (SystemState.Instance().IsInspectOrWait == true)
                    return false;

                if (SystemManager.Instance().CurrentModel == null || SystemManager.Instance().ProductionManager.CurProduction == null)
                    return false;

                // IM 로트 변경.
                ProductionG curProduction = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
                SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_LOTCHANGE, curProduction.LotNo, curProduction.RewinderZone.ToString(), curProduction.LineSpeedMpm.ToString());

                // 라인 속도 체크
                ProductionG pg = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
                if (pg != null)
                    pg.UpdateLineSpeedMpm();

                // 조명 보정
                if (AdditionalSettings.Instance().AutoLight)
                {
                    float rollDiaMm = ((DeviceControllerG)SystemManager.Instance().DeviceController).MachineIfMonitor.MachineIfData.GET_ROLL_DIAMETER_REAL;
                    if (!AutoLight(rollDiaMm))
                        throw new AlarmException(ErrorCodeTeach.Instance.AutoLight, ErrorLevel.Error,
                            "AutoLighting Process Failure.", null, "");
                }

                // 검사기, 검사화면으로 전환
                SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.V_INSPECT);

                SystemState.Instance().SetWait();

                return true;
            }
            catch (AlarmException ex)
            {
                ErrorManager.Instance().Report(ex);
                return false;
            }
        }

        //private bool WaitUntil(string message, int waitTimeMs, OpState waitUntil, bool forAll)
        //{
        //    bool onError = false;
        //    SimpleProgressForm simpleProgressForm = new SimpleProgressForm(message);
        //    simpleProgressForm.Show(ConfigHelper.Instance().MainForm, () =>
        //    {
        //        TimeOutTimer tot = new TimeOutTimer();
        //        if (waitTimeMs >= 0)
        //            tot.Start(waitTimeMs);

        //        bool waitDone = false;
        //        do
        //        {
        //            List<InspectorObj> connectedInspObj = inspectorList.FindAll(f => f.CommState == CommState.CONNECTED);
        //            if (connectedInspObj.Count == 0)
        //                break;
        //            Thread.Sleep(100);

        //            onError = connectedInspObj.Exists(f => f.OpState == OpState.Alarm) || tot.TimeOut || ErrorManager.Instance().IsAlarmed();
        //            if (forAll)
        //                waitDone = connectedInspObj.TrueForAll(f => f.OpState == waitUntil);
        //            else
        //                waitDone = connectedInspObj.Exists(f => f.OpState == waitUntil);
        //        } while (onError == false && waitDone == false);

        //        if (tot.TimeOut)
        //            Debug.WriteLine("WaitUntil::TimeOutTimer.TimeOut");
        //    });

        //    return onError == false;
        //}

        public override bool PostEnterWaitInspection()
        {
            try
            {
                // 레이저 먼저 켜봄 -> 실패하면 그냥 종료..
                SystemManager.Instance().DeviceController.OnEnterWaitInspection("Laser");
                SystemManager.Instance().DeviceController.OnExitWaitInspection();

                // 티칭
                if (AdditionalSettings.Instance().AutoTeach)
                {
                    SystemManager.Instance().DeviceController.OnEnterWaitInspection("Stage", "Light", "Encoder");
                    AutoTeach(curProduction.LineSpeedMpm);
                    SystemManager.Instance().DeviceController.OnExitWaitInspection();
                }

                // 버퍼 할당
                SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_READY);
                int timeoutMs = ((AdditionalSettings)AdditionalSettings.Instance()).BufferAllocTimeout;
                InspectorWaiter.WaitAll(this.inspectorObjs, StringManager.GetString(this.GetType().FullName, "Buffer Ready"), OpState.Wait, timeoutMs,
                    new AlarmException(ErrorCodeInspect.Instance.BufferInitialize, ErrorLevel.Error));

                // 결과 수집용 스레드 작동
                InitResultCollectThread();
                this.alarmManager.ClearData();
                this.inspNoSrc = GetNextInspNo(curProduction);

                // 검사 시작
                OpState opState = SystemState.Instance().OpState;
                if (opState != OpState.Wait)
                    throw new AlarmException(ErrorCodeInspect.Instance.EnterWaitInspection, ErrorLevel.Error, "CM Alarm Occure.", null, "");

                this.inspectObserver.Clear();

                string infoString = string.Format("Monitoring Start, Model,{0}, Lot,{1}", SystemManager.Instance().CurrentModel.Name, SystemManager.Instance().ProductionManager.CurProduction.LotNo);
                LogHelper.Info(LoggerType.Inspection, infoString);

                // 카메라 시작 후 엔코더분배기 작동.
                SystemManager.Instance().DeviceController.OnEnterWaitInspection("Stage", "Light", "Laser");                

                SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_START, SystemManager.Instance().ProductionManager.CurProduction.LotNo);

                InspectorWaiter.WaitAll(this.inspectorObjs, StringManager.GetString(this.GetType().FullName, "Wait Inspect"), OpState.Inspect, 60000, null);
            
                UpdateProductionSpec(curProduction);

                SystemState.Instance().SetInspect();
                SystemState.Instance().SetInspectState(InspectState.Run);

                SystemManager.Instance().DeviceController.OnEnterWaitInspection("Encoder");
                return true;
            }
            catch (AlarmException ex)
            {
                //SystemManager.Instance().DeviceController.OnExitWaitInspection();
                //ExitWaitInspection();
                ErrorManager.Instance().Report(ex);
                return false;
            }
        }

        private int GetNextInspNo(ProductionG curProduction)
        {
            return (curProduction.LastSequenceNo + 2) / 2 * 2;
        }

        private void UpdateProductionSpec(ProductionG curProduction)
        {
            string[] dstFiles = CopyTeachParam();
            curProduction.UpdateSpec(dstFiles);
        }

        private string[] CopyTeachParam()
        {
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorList = server.GetInspectorList().FindAll(f => f.Info.ClientIndex <= 0);
            string[] dstPaths = new string[inspectorList.Count];

            UniScanG.Data.Model.ModelDescription md = SystemManager.Instance().CurrentModel.ModelDescription as UniScanG.Data.Model.ModelDescription;
            //UniScanG.Data.Production curProduction = SystemManager.Instance().ProductionManager.CurProduction as UniScanG.Data.Production;
            //string resultPath = curProduction.ResultPath;
            ProductionG productionG = (ProductionG)SystemManager.Instance().ProductionManager.CurProduction;
            Directory.CreateDirectory(productionG.ResultPath);

            for (int i = 0; i < inspectorList.Count; i++)
            {
                InspectorObj inspectorObj = inspectorList[i];
                string srcPath = Path.Combine(inspectorObj.ModelManager.GetModelPath(md), "InspData", productionG.LotNo);

                string[] files = new string[]
                {
                    "Teach.xml", "Prev.jpg", "Prev0.jpg"
                };

                Array.ForEach(files, f =>
                {
                    string srcFile = Path.Combine(srcPath, f);
                    if (File.Exists(srcFile))
                    {
                        string name = Path.GetFileNameWithoutExtension(srcFile);
                        string ext = Path.GetExtension(srcFile);
                        string dstFile = Path.Combine(productionG.ResultPath, string.Format("{0}_C{1}{2}", name, inspectorObj.Info.CamIndex, ext));
                        FileHelper.CopyFile(srcFile, dstFile, true);
                        File.Delete(srcFile);
                        if (ext == ".xml")
                            dstPaths[i] = dstFile;
                    }
                });

                DirectoryInfo directoryInfo = new DirectoryInfo(srcPath);
                if (directoryInfo.Exists && directoryInfo.GetFiles().Length == 0)
                    directoryInfo.Delete();
            }

            return dstPaths;
        }

        private void InitResultCollectThread()
        {
            foreach (InspectorObj inspector in this.inspectorObjs)
            {
                if (resultDic.ContainsKey(inspector.Info.CamIndex) == false)
                    resultDic.Add(inspector.Info.CamIndex, new List<Tuple<string, string>>());
            }

            thread?.Abort();
            stopThread = false;
            thread = new Thread(InspectionResultTask);
            thread.Start();
        }

        private void DisposeResultCollectThread(bool waitStop)
        {
            this.stopThread = true;
            if (waitStop)
                this.thread?.Join();
        }

        public bool AutoLight(float rollDiametorMm)
        {
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            InspectorObj inspectorObj = server.GetInspectorList().FindAll(f => f.Info.CamIndex <= 0 && f.Info.ClientIndex <= 0 && f.CommState == CommState.CONNECTED).FirstOrDefault();

            // 반복...
            inspectorObj.ResetJobState();
            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_LIGHT, rollDiametorMm.ToString());
            while (!inspectorObj.JobState.IsJobDone) ;
            return true;
        }

        public void AutoTeach(float lineSpeedMpm)
        {
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorObjList = server.GetInspectorList().FindAll(f => f.Info.ClientIndex <= 0 && f.CommState == CommState.CONNECTED);
            this.modelTeachedDic = inspectorObjList.ToDictionary(f => f.Info.CamIndex, f => false);

            int timeoutMs = ((AdditionalSettings)AdditionalSettings.Instance()).AutoTeachTimeout;
            if (timeoutMs > 0 && lineSpeedMpm > 0)
            {
                float rate = 40 / lineSpeedMpm;
                timeoutMs = (int)(timeoutMs * rate);
            }

            // 티칭 시작
            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_TEACH, lineSpeedMpm.ToString());

            InspectorObj[] inspectorObjs = Array.FindAll(this.inspectorObjs, f => f.Info.ClientIndex <= 0);
            InspectorWaiter.WaitAll(inspectorObjs, StringManager.GetString(this.GetType().FullName, "Teaching"), OpState.Teach, timeoutMs,
                new AlarmException(ErrorCodeTeach.Instance.AutoTeach, ErrorLevel.Error));

            InspectorWaiter.WaitAll(inspectorObjs, StringManager.GetString(this.GetType().FullName, "Teaching"), OpState.Idle, timeoutMs,
                new AlarmException(ErrorCodeTeach.Instance.AutoTeach, ErrorLevel.Error));

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            TimeOutTimer tot = new TimeOutTimer();
            if (timeoutMs >= 0)
                tot.Start(timeoutMs);

            bool waitDone = false;
            new SimpleProgressForm().Show(ConfigHelper.Instance().MainForm, () =>
             {
                 while (this.modelTeachedDic.Values.All(f => f) == false)
                 {
                     Application.DoEvents();
                     if (ErrorManager.Instance().IsAlarmed() || tot.TimeOut)
                         break;
                 }
                 waitDone = true;
             }, cancellationTokenSource);
            tot.Stop();

            if (!waitDone)
            {
                throw new AlarmException(ErrorCodeTeach.Instance.AutoTeach, ErrorLevel.Error, "CM",
                  "Wait Done Timeout", null, "");
            }

            LogHelper.Debug(LoggerType.Inspection, string.Format("InspectRunnerMonitorG::AutoLight - tot.TimeOut: {0}, cancellationTokenSource.IsCancellationRequested: {1}", tot.TimeOut, cancellationTokenSource.IsCancellationRequested));
        }

        //private bool IsSelectNeed()
        //{
        //    // VNC 접속된적이 있으면 항상 업데이트
        //    Model model = SystemManager.Instance().CurrentModel;
        //    if (model.Modified)
        //        return true;

        //    // Salve Inspector 중 티칭되지 않은것이 있으면 업데이트
        //    ModelDescription modelDescription = SystemManager.Instance().CurrentModel.ModelDescription;
        //    IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
        //    List<InspectorObj> inspectorObjList = server.GetInspectorList().FindAll(f => f.Info.ClientIndex != 0);
        //    return !inspectorObjList.TrueForAll(f => f.IsTrained(modelDescription));
        //}

        //bool syncDone = false;
        //private void SyncModel(object sender, DoWorkEventArgs e)
        //{
        //    BackgroundWorker worker = sender as BackgroundWorker;

        //    UniScanG.Data.Model.Model curModel = SystemManager.Instance().CurrentModel;
        //    IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
        //    List<InspectorObj> inspectorObjList = server.GetInspectorList();
        //    int cpyCnt = 0;

        //    worker?.ReportProgress(0, string.Format("0 / {0}", inspectorObjList.Count));
        //    for (int i = 0; i < inspectorObjList.Count; i++)
        //    {
        //        worker?.ReportProgress(i * 100 / inspectorObjList.Count, string.Format("{0} / {1}", i, inspectorObjList.Count));
        //        InspectorObj inspectorObj = inspectorObjList[i];
        //        bool exist = inspectorObj.ModelManager.IsModelExist(curModel.ModelDescription);
        //        if (exist == false)
        //        {
        //            e.Result = string.Format(StringManager.GetString("Model Path is NOT exist in {0}"), inspectorObj.Info.GetName());
        //            return ;
        //        }

        //        bool isTrained = inspectorObj.IsTrained(curModel.ModelDescription);
        //        if (isTrained == false || true)
        //        {
        //            InspectorObj baseInspectorObj = inspectorObjList.Find(f => f.Info.CamIndex == inspectorObj.Info.CamIndex && f.Info.ClientIndex == 0);
        //            if (baseInspectorObj == null)
        //            {
        //                e.Result = string.Format(StringManager.GetString("Can not found master device of {0}"), inspectorObj.Info.GetName());
        //                return;

        //            }

        //            string srcModelPath = baseInspectorObj.ModelManager.GetModelPath(curModel.ModelDescription);
        //            string dstModelPath = inspectorObj.ModelManager.GetModelPath(curModel.ModelDescription);
        //            if (srcModelPath == dstModelPath)
        //            {
        //                continue;
        //            }

        //                bool copied = CopyDirectory(srcModelPath, dstModelPath);
        //                if (copied == false)
        //                {
        //                    e.Result = string.Format(StringManager.GetString("Data copy fail in {0}"), inspectorObj.Info.GetName());
        //                    return;
        //                }
        //                cpyCnt++;
        //            //    string[] args = curModel.ModelDescription.GetArgs();
        //            //    SystemManager.Instance().ExchangeOperator.SelectModel(args);
        //            //    inspectorObj.ModelManager.LoadModel(args, null);
        //        }
        //    }
        //}

        //private void SyncModelComplete(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    Exception ex = e.Error;
        //    string message = (string)e.Result;
        //    if (string.IsNullOrEmpty(message) && ex == null)
        //        syncDone = true;
        //    else
        //        syncDone = false;
        //}

        private bool CopyDirectory(string srcModelPath, string dstModelPath)
        {
            if (Directory.Exists(dstModelPath) == false)
                Directory.CreateDirectory(dstModelPath);

            string[] directopries = Directory.GetDirectories(srcModelPath);
            foreach (string directoprie in directopries)
                CopyDirectory(directoprie, Path.Combine(dstModelPath, Path.GetFileName(directoprie)));

            string[] files = Directory.GetFiles(srcModelPath);
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                //try
                //{
                File.Copy(file, Path.Combine(dstModelPath, fileName), true);
                //}
                //catch (IOException ex)
                //{
                //    LogHelper.Error(LoggerType.Operation, string.Format("InspectPage::CopyDirectory. {0}", ex.Message));
                //    return false;
                //}
            }
            return true;
        }


        public override void PreExitWaitInspection()
        {
            //this.alarmManager.Clear();

            // Encoder/Ligth Disable
            SystemManager.Instance().DeviceController.OnExitWaitInspection();
        }

        public override void ExitWaitInspection()
        {
            lock (waitInspectLocker)
            {
                if (SystemState.Instance().IsIdle)
                    return;

                //this.observerForm.Close();           

                string messageFormMessage = StringManager.GetString(this.GetType().FullName, "Stop");
                Form mainForm = ConfigHelper.Instance().MainForm;
                // CM 정지
                new SimpleProgressForm(messageFormMessage).Show(mainForm, () =>
                {
                    DisposeResultCollectThread(false);
                    PreExitWaitInspection();
                });

                // IM 정지
                try
                {
                    LogHelper.Debug(LoggerType.Inspection, string.Format("InspectRunnerMonitorG::ExitWaitInspection - Stop IMs"));
                    SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_STOP);
                    Thread.Sleep(1000);
                    InspectorWaiter.WaitAll(this.inspectorObjs, messageFormMessage, OpState.Idle, 60000, null);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Inspection, string.Format("InspectRunnerMonitorG::ExitWaitInspection - Stop IMs - {0}", ex.Message));
                }


                LogHelper.Info(LoggerType.Inspection, string.Format("InspectRunnerMonitorG::ExitWaitInspection - Wait Thread Join"));
                new SimpleProgressForm(messageFormMessage).Show(mainForm, () => DisposeResultCollectThread(true));
                this.thread = null;

                lock (resultDic)
                    resultDic.Clear();
                this.inspectionResultList.Clear();

                this.alarmManager.ClearSignal();
                //SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.V_MODEL);

                ProductionG production = (ProductionG)SystemManager.Instance().ProductionManager.CurProduction;
                production?.UpdateGrade(AdditionalSettings.Instance().Grade, AdditionalSettings.Instance().GradeNP, AdditionalSettings.Instance().GradePH);

                if (SystemState.Instance().IsInspectOrWait)
                {
                    string infoString = string.Format("Monitoring Stop, Total,{0}", production == null ? -1 : production.Total);
                    LogHelper.Info(LoggerType.Inspection, infoString);
                }

                SystemManager.Instance().ProductionManager.Save();
                production?.Save();

                //MachineIF.MachineIfData machineIfData = this.machineIfMonitor.MachineIfData as MachineIF.MachineIfData;
                //machineIfData?.ClearVisionData();

                SystemState.Instance().SetInspectState(InspectState.Wait);
                SystemState.Instance().SetIdle();
            }
        }

        public override void EnterPauseInspection()
        {
            this.alarmManager.ClearSignal();
            SystemManager.Instance().DeviceController.OnExitWaitInspection(); // 주변장비 끔

            SystemManager.Instance().ExchangeOperator.SendCommand(Common.Exchange.ExchangeCommand.I_PAUSE);

            InspectorWaiter.WaitAll(this.inspectorObjs, StringManager.GetString(this.GetType().FullName, "Wait Pause"), OpState.Wait, 20000,
                new AlarmException(ErrorCodeInspect.Instance.EnterPauseInspection, ErrorLevel.Error));

            new SimpleProgressForm("Wait Pause 2").Show(() => DisposeResultCollectThread(true));

            SystemManager.Instance().ProductionManager.CurProduction.Save();
        }

        public override void ExitPauseInspection()
        {
            InitResultCollectThread();
            ProductionG curProduction = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
            this.inspNoSrc = GetNextInspNo(curProduction);
            this.alarmManager.ClearData();

            SystemManager.Instance().DeviceController.OnEnterWaitInspection("Stage", "Light", "Laser"); // 주변장비 켬

            SystemManager.Instance().ExchangeOperator.SendCommand(Common.Exchange.ExchangeCommand.I_START);
            InspectorWaiter.WaitAll(this.inspectorObjs, StringManager.GetString(this.GetType().FullName, "Wait Inspect"), OpState.Inspect, 10000,
                new AlarmException(ErrorCodeInspect.Instance.ExitPauseInspection, ErrorLevel.Error));

            UpdateProductionSpec(curProduction);
            SystemManager.Instance().DeviceController.OnEnterWaitInspection("Encoder"); // 주변장비 켬
        }

        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            if (SystemState.Instance().OpState == OpState.Idle)
                return;

            object cam = inspectionResult.ExtraResult["Cam"];
            object client = inspectionResult.ExtraResult["Client"];
            object no = inspectionResult.ExtraResult["No"];
            object path = inspectionResult.ExtraResult["Path"];
            object judgment = inspectionResult.ExtraResult["Judgment"];

            //string infoString = string.Format("Inspector Call, CamId,{0}, ClientId,{1}, SheetNo,{2}, Result,{3}", cam, client, no, judgment);
            //LogHelper.Info(LoggerType.Inspection, infoString);

            lock (inspectionResultList)
                inspectionResultList.Add(inspectionResult);
        }

        private void ProductionManager_OnLotChanged()
        {
            this.curProduction = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
        }

        public void InspectionResultTask()
        {
            bool exitable = false;

            while (stopThread == false || exitable == false)
            {
                if (inspectionResultList.Count > 0)
                {
                    exitable = false;
                    List<InspectionResult> inspectionResultList2 = new List<InspectionResult>();
                    lock (inspectionResultList)
                    {
                        inspectionResultList2 = new List<InspectionResult>(inspectionResultList);
                        inspectionResultList.Clear();
                    }

                    foreach (InspectionResult result in inspectionResultList2)
                    {
                        int camIndex = Convert.ToInt32(result.GetExtraResult("Cam"));
                        int clientIndex = Convert.ToInt32(result.GetExtraResult("Client"));
                        string inspectionNo = (string)result.GetExtraResult("No");
                        string Path = (string)result.GetExtraResult("Path");

                        if (resultDic.ContainsKey(camIndex))
                        {
                            lock (resultDic[camIndex])
                            {
                                resultDic[camIndex].Add(new Tuple<string, string>(inspectionNo, Path));
                                //resultDic[camIndex].Sort();
                                this.inspectObserver.AddData(camIndex, clientIndex, int.Parse(inspectionNo));
                            }
                        }
                    }
                }

                //if (this.trasnferTaskList.Count < MAX_TRANSFER_BUFFER_SIZE)
                {
                    Tuple<string, string> foundedT = null;
                    {
                        lock (resultDic)
                        {
                            // 같은거 찾기
                            // 0번 카메라에서 넘어온 시트 번호를 각 카메라에서 넘어온 시트 번호와 매칭한다.
                            int camCnt = resultDic.Count;
                            List<Tuple<string, string>> searchFrom = resultDic.FirstOrDefault().Value;
                            if (searchFrom != null)
                            {
                                int[] idxArray = new int[camCnt];
                                for (int itemIdx = 0; itemIdx < searchFrom.Count; itemIdx++)
                                {
                                    string target = searchFrom[itemIdx].Item1;
                                    idxArray[0] = itemIdx;
                                    for (int camIdx = 1; camIdx < camCnt; camIdx++)
                                        idxArray[camIdx] = resultDic[camIdx].FindIndex(f => f.Item1 == target);

                                    if (Array.TrueForAll(idxArray, f => f >= 0))
                                    {
                                        foundedT = searchFrom[itemIdx];
                                        for (int camIdx = 0; camIdx < camCnt; camIdx++)
                                            resultDic.ElementAt(camIdx).Value.RemoveAt(idxArray[camIdx]);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (foundedT != null)
                    {
                        exitable = false;
                        Task sheetCombinerTask = new Task(() =>
                         {
                             Stopwatch stopwatch = new Stopwatch();
                             stopwatch.Start();

                             int inspectionNo;
                             if (int.TryParse(foundedT.Item1, out inspectionNo) == false)
                                 return;
                             try
                             {
                                 //System.Diagnostics.Debug.WriteLine(string.Format("InspectRunnerMonitorG::InspectionResultTask Start - InspectionNo: {0}", inspectionNo));

                                 InspectionResult inspectionResult = BuildInspectionResult((this.inspNoSrc + inspectionNo).ToString());
                                 AlgorithmResultG sheetResult = SheetCombiner.CombineResult(foundedT);
                                 MergeSheetResult mergeSheetResult = new MergeSheetResult(int.Parse(inspectionResult.InspectionNo), sheetResult);
                                 mergeSheetResult.UpdateDefectDensity(AdditionalSettings.Instance().StripeDefectAlarm);
                                 mergeSheetResult.UpdateCritical(AdditionalSettings.Instance().CriticalRollAlarm);
                                 inspectionResult.InspectionStartTime = mergeSheetResult.StartTime;
                                 inspectionResult.InspectionEndTime = mergeSheetResult.EndTime;

                                 if (Settings.Monitor.MonitorSystemSettings.Instance().UseLaserBurner != Settings.Monitor.LaserMode.None)
                                 {
                                     Controller.Device.DeviceController deviceController = (Controller.Device.DeviceController)SystemManager.Instance().DeviceController;
                                     mergeSheetResult.PostProcessed &= ((deviceController.HanbitLaser != null) && deviceController.HanbitLaser.HanbitLaserExtender.Use);
                                 }
                                 inspectionResult.AlgorithmResultLDic.Add(SheetCombiner.TypeName, mergeSheetResult);
                                 inspectionResult.InspectionTime = mergeSheetResult.SpandTime;

                                 if (mergeSheetResult.IsNG)
                                     inspectionResult.SetDefect();

                                 //SystemManager.Instance().ProductionManager.CurProduction as UniScanG.Data.Production;

                                 lock (this.curProduction)
                                     this.curProduction.Update(inspectionResult);

                                 SystemManager.Instance().ExportData(inspectionResult);

                                 stopwatch.Stop();
                                 inspectionResult.ExportTime = stopwatch.Elapsed;

                                 if (inspectionNo % 50 == 0)
                                 {
                                     this.curProduction.UpdateGrade();
                                     this.curProduction.Save();
                                     SystemManager.Instance().ProductionManager.Save();
                                 }

                                 //System.Diagnostics.Debug.WriteLine(string.Format("InspectRunnerMonitorG::InspectionResultTask End - InspectionNo: {0}", inspectionNo));
                                 //string infoString = string.Format("Monitor Done, SheetNo,{0}, InspectTimeMs,{1}, ImportTimeMs,{2}",
                                 //     inspectionResult.InspectionNo, inspectionResult.InspectionTime.ToString("ss\\.fff"), stopwatch.ElapsedMilliseconds);
                                 //LogHelper.Info(LoggerType.Inspection, infoString);

                                 InspectDone(inspectionResult);
                             }
                             catch (Exception ex)
                             {
                                 LogHelper.Error(LoggerType.Inspection, string.Format("InspectRunnerMonitorG::InspectionResultTask (sheetCombinerTask) Exception({0}) - PatternNo: {1}, {2}, ", ex.GetType(), inspectionNo, ex.Message));
                                 LogHelper.Error(LoggerType.Inspection, ex.StackTrace);
                             };
                         });
                        //sheetCombinerTask.Wait();

                        lock (this.waitTaskList)
                            this.waitTaskList.Add(sheetCombinerTask);
                    }
                    else
                    {
                        exitable = true;
                    }
                }

                lock (this.trasnferTaskList)
                {
                    this.trasnferTaskList.RemoveAll(f => f.IsCompleted || f.IsFaulted);

                    int runningCount = this.trasnferTaskList.Count(f => !f.IsCompleted);
                    int runableCount = Math.Min(MAX_TRANSFER_BUFFER_SIZE - runningCount, this.waitTaskList.Count);

                    List<Task> startTask = this.waitTaskList.GetRange(0, runableCount);
                    this.waitTaskList.RemoveRange(0, runableCount);

                    startTask.ForEach(f => f.Start());
                    this.trasnferTaskList.AddRange(startTask);
                }


                if (exitable == true)
                {
                    exitable = this.trasnferTaskList.Count == 0;
                    Thread.Sleep(10);
                }
            }
            this.trasnferTaskList.Clear();

            WriteLoadFactors();
        }

        private void WriteLoadFactors()
        {
            List<InspectorObj> inspectorList = ((MonitorOperator)SystemManager.Instance().ExchangeOperator).GetInspectorList();
            string path = SystemManager.Instance().ProductionManager.CurProduction?.GetResultPath();
            var res = Parallel.ForEach(inspectorList, f => 
            {
                FileInfo fInfo = new FileInfo(Path.Combine(path, $"LoadFactors_{f.Info.GetName()}.csv"));
                using (StreamWriter writer = new StreamWriter(fInfo.FullName, true))
                {
                    writer.WriteLine($"DateTime,{f.Info.GetName()}");

                    KeyValuePair<DateTime, float>[] pairs = f.LoadFactorList.ToArray();
                    foreach (var pair in pairs)
                    {
                        writer.Write(pair.Key.ToString(AlgorithmResultG.DateTimeFormat));
                        writer.Write(",");
                        writer.Write(pair.Value.ToString("F03"));
                        writer.WriteLine();
                    }
                    writer.WriteLine();
                }
            });            
        }

        public void ModelRefreshed()
        {
            throw new NotImplementedException();
        }

        public void ModelChanged() { }

        public void ModelTeachDone(int camId)
        {
            if (this.modelTeachedDic.ContainsKey(camId))
                this.modelTeachedDic[camId] = true;
        }

        public override bool IsRunable()
        {
            bool isRunnable = base.IsRunable();
            if (!isRunnable)
                return false;

            AdditionalSettings additionalSettings = AdditionalSettings.Instance();
            MachineIfData machineIfData = (SystemManager.Instance().DeviceController as DeviceControllerG)?.MachineIfMonitor?.MachineIfData as MachineIfData;
            if (machineIfData != null)
            {
                if (machineIfData.GET_TARGET_SPEED_REAL > additionalSettings.MaximumLineSpeed)
                    throw new Exception("Speed is too fast.");
                else if (machineIfData.GET_TARGET_SPEED_REAL < additionalSettings.MinimumLineSpeed)
                    throw new Exception("Speed is too slow.");
            }

            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorObjList = server.GetInspectorList();
            bool allConnected = inspectorObjList.TrueForAll(f => f.CommState == CommState.CONNECTED);
            if (allConnected == false)
                throw new Exception("Inspector is not connected.");

            ModelDescription modelDescription = SystemManager.Instance().CurrentModel.ModelDescription;
            string[] modelDescArgs = modelDescription.GetArgs();
            bool allSelected = inspectorObjList.TrueForAll(f => f.ModelSelectState.IsJobDone && modelDescArgs.SequenceEqual(f.ModelSelectState.JobResult));
            if (allSelected == false)
                throw new Exception("Inspector is not selected model.");

            return true;
        }
    }
}
