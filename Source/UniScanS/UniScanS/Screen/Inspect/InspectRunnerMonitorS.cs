using System;
using System.Drawing;
using System.IO;

using DynMvp.Base;
using DynMvp.Data;
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
using UniScanS.Inspect;
using UniScanS.Screen.Vision.Detector;
using UniScanS.Vision.FiducialFinder;
using UniScanS.Screen.Vision;
using System.Collections.Generic;
using UniScanS.Vision;
using UniScanS.Common.Exchange;
using UniScanS.Screen.Data;
using DynMvp.Authentication;
using UniScanS.Data;
using System.Diagnostics;
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Settings;
using System.Threading.Tasks;
using DynMvp.Device.Serial;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Light;
using UniScanS.Data.UI;

namespace UniScanS.Screen.Inspect
{
    internal class InspectRunnerMonitorS : UniScanS.Inspect.InspectRunner
    {
        Thread machineMonitorThread = null;

        bool stopThread = true;
        Thread thread = null;
        Dictionary<int, List<Tuple<string, string>>> resultDic = new Dictionary<int, List<Tuple<string, string>>>();
        List<InspectorObj> InspectorList = new List<InspectorObj>();

        bool onCancel = false;

        bool onInspect = false;

        List<InspectionResult> InspectionResultList = new List<InspectionResult>();

        public object SystemTypeManager { get; private set; }

        public InspectRunnerMonitorS() : base()
        {
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;

            InspectorList = server.GetInspectorList();
            
            UpdateSerialEncoder(false);
            UpdateLightCtrl(false);
        }

        protected override void SetupUnitManager() { }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override bool EnterWaitInspection()
        {
            if (SystemState.Instance().IsInspectOrWait == true)
                return false;
            
            if (SystemManager.Instance().CurrentModel == null || SystemManager.Instance().ProductionManager.CurProduction == null)
                return false;
            
            resultDic.Clear();

            foreach (InspectorObj inspector in InspectorList)
                resultDic.Add(inspector.Info.CamIndex, new List<Tuple<string, string>>());

            //machineMonitorThread = new Thread(MachineStateMonitorTask);
            //machineMonitorThread.Start();

            foreach (AlarmChecker alarmChecker in MonitorSetting.Instance().AlarmCheckerList)
                alarmChecker.Reset();

            MonitorSetting.Instance().ErrorChecker.Reset();

            stopThread = false;
            
            thread = new Thread(InspectionResultTask);
            thread.Start();

            IoPort ioPort = SystemManager.Instance().DeviceBox.PortMap.GetInPort(UniScanS.Screen.Device.PortMap.IoPortName.InMachineRolling);

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();//
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm("Start");
            simpleProgressForm.Show(() =>
            {
                ProductionS curProduction = (ProductionS)SystemManager.Instance().ProductionManager.CurProduction;

                SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_START, curProduction.LotNo, curProduction.SheetIndex.ToString());

                bool startDone = false;
                while (startDone == false)
                {
                    startDone = true;

                    foreach (InspectorObj inspector in InspectorList)
                    {
                        if (inspector.InspectState == InspectState.Done)
                        {
                            startDone = false;
                            break;
                        }
                    }
                }
            }, cancellationTokenSource);

            if (cancellationTokenSource.IsCancellationRequested == true)
            {
                ExitWaitInspection();
                return false;
            }
            
            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_ENTER_PAUSE);
            bool result = PostEnterWaitInspection();
            if (ioPort != null)
            {
                if (WaitFall(ioPort) == false)
                {
                    ExitWaitInspection();
                    return false;
                }
            }

            SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_EXIT_PAUSE);

            SystemState.Instance().SetInspect();

            return result;
        }

        public override bool PostEnterWaitInspection()
        {
            UpdateLightCtrl(true);
            UpdateSerialEncoder(true);

            return true;
        }

        private bool WaitFall(IoPort ioPort)
        {
            bool val1 = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadInput(ioPort);
            bool val2 = val1;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm("Wait Rolling...");

            onCancel = false;
            simpleProgressForm.Show(() =>
            {
                while (onCancel == false)   // Low -> Low
                {
                    Thread.Sleep(10);
                    val1 = val2;
                    val2 = SystemManager.Instance().DeviceBox.DigitalIoHandler.ReadInput(ioPort);

                    if (val1 == true && val2 == false)
                        break;
                }

                //Thread.Sleep(1000);
            }, cancellationTokenSource);

            if (cancellationTokenSource.IsCancellationRequested == true)
            {
                onCancel = true;
                return false;
            }

            return true;
        }

        private void UpdateSerialEncoder(bool enable)
        {
            SerialDeviceHandler sdh = SystemManager.Instance().DeviceBox.SerialDeviceHandler;
            SerialDevice sd = SystemManager.Instance().DeviceBox.SerialDeviceHandler.Find(f => f.DeviceInfo.DeviceType == ESerialDeviceType.SerialEncoder);
            if (sd != null)
            {
                SerialEncoder se = (SerialEncoder)sd;
                string[] token = se.ExcuteCommand(SerialEncoderV105.ECommand.EN, enable ? "1" : "0")?.Split(',');
                //string[] token = se.ExcuteCommand(SerialEncoderV105.ECommand.GR);
            }
        }

        public void UpdateLightCtrl(bool enable)
        {
            LightCtrlHandler lch = SystemManager.Instance().DeviceBox.LightCtrlHandler;
            if (lch.Count > 0)
            {
                SerialLightCtrl serialLightCtrl = lch.GetLightCtrl(0) as SerialLightCtrl;
                if (serialLightCtrl != null)
                {
                    serialLightCtrl.LightSerialPort.WritePacket(enable ? "UDIO1\r\n" : "UDIO0\r\n");
                    if (enable)
                        serialLightCtrl.TurnOn(SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0].LightValue);
                    else
                        serialLightCtrl.TurnOff();
                }
            }
        }

        public override void PreExitWaitInspection()
        {
            // Encoder/Ligth Disable
            UpdateSerialEncoder(false);
            UpdateLightCtrl(false);
        }

        public override void ExitWaitInspection()
        {
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm("Stop");
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            simpleProgressForm.Show(() =>
            {
                SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.I_STOP);

                bool stopDone = false;
                while (stopDone == false)
                {
                    stopDone = true;

                    foreach (InspectorObj inspector in InspectorList)
                    {
                        if (inspector.OpState != OpState.Idle)
                        {
                            stopDone = false;
                            break;
                        }
                    }
                }

                PreExitWaitInspection();

                SystemManager.Instance().ProductionManager.Save(PathSettings.Instance().Result);

                stopThread = true;

                while (true)
                {
                    Thread.Sleep(100);
                    if (this.thread.ThreadState == System.Threading.ThreadState.Stopped)
                        break;
                }

                resultDic.Clear();
                
                ProductionBase production = SystemManager.Instance().ProductionManager.CurProduction;

                foreach (AlarmChecker alarmChecker in MonitorSetting.Instance().AlarmCheckerList)
                    alarmChecker.Reset();

                MonitorSetting.Instance().ErrorChecker.Reset();

                SystemState.Instance().SetIdle();

            }, cancellationTokenSource);
        }

        public override void EnterPauseInspection()
        {
        }

        public override void InspectDone(InspectionResult inspectionResult)
        {
            SystemState.Instance().SetInspectState(UniEye.Base.Data.InspectState.Done);
        }

        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            if (SystemState.Instance().OpState == OpState.Idle)
                return;

            lock (InspectionResultList)
                InspectionResultList.Add(inspectionResult);
        }


        public void MachineStateMonitorTask()
        {

        }

        public void InspectionResultTask()
        {
            while (stopThread == false)
            {
                int pairCount = 0;
                lock (resultDic)
                {
                    if (resultDic.Count != 0)
                    {
                        foreach (KeyValuePair<int, List<Tuple<string, string>>> pair in resultDic)
                            pairCount += pair.Value.Count;
                    }
                }

                if (InspectionResultList.Count == 0 && pairCount == 0)
                {
                    Thread.Sleep(0);
                    continue;
                }

                lock (InspectionResultList)
                {
                    foreach (InspectionResult result in InspectionResultList)
                    {
                        int camIndex = Convert.ToInt32(result.GetExtraResult("Cam"));
                        string inspectionNo = (string)result.GetExtraResult("No");
                        string inspectionTime = (string)result.GetExtraResult("Time");

                        if (resultDic.Count != 0)
                            resultDic[camIndex].Add(new Tuple<string, string>(inspectionNo, inspectionTime));
                    }

                    InspectionResultList.Clear();
                }

                if (resultDic.Count == 0)
                    continue;
                
                Tuple<string, string> foundedT = null;
                foreach (Tuple<string, string> tuple in resultDic[0])
                {
                    bool existResult = true;
                    foreach (InspectorObj inspector in InspectorList)
                    {
                        if (inspector.Info.CamIndex != 0)
                        {
                            if (resultDic[inspector.Info.CamIndex].Find(t => t.Item1 == tuple.Item1) == null)
                            {
                                existResult = false;
                                break;
                            }
                        }
                    }

                    if (existResult == false)
                        continue;

                    foundedT = tuple;
                    break;
                }

                if (foundedT == null)
                    continue;

                if (SystemState.Instance().OpState != OpState.Idle)
                {
                    onInspect = true;
                    SystemState.Instance().SetInspectState(InspectState.Run);
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                
                    foreach (InspectorObj inspector in InspectorList)
                        LoadCurrentProduction(inspector.CurProduction, Path.Combine(inspector.Info.Path, "Result"));

                    MergeSheetResult mergeSheetResult = new MergeSheetResult(Convert.ToInt32(foundedT.Item1), SheetCombiner.CombineResult(foundedT));

                    InspectionResult inspectionResult = BuildInspectionResult();
                    inspectionResult.AlgorithmResultLDic.Add(SheetInspector.TypeName, mergeSheetResult);
                    inspectionResult.InspectionNo = foundedT.Item1;
                    inspectionResult.InspectionTime = mergeSheetResult.SpandTime;

                    ProductionS productionG = (ProductionS)SystemManager.Instance().ProductionManager.CurProduction;
                    if (productionG != null)
                    {
                        productionG.Update(mergeSheetResult);
                        SystemManager.Instance().ProductionManager.Save(PathSettings.Instance().Result);
                    }

                    SystemManager.Instance().ExportData(inspectionResult);

                    stopwatch.Stop();
                    inspectionResult.ExportTime = stopwatch.Elapsed;

                    InspectDone(inspectionResult);
                    
                    if (resultDic.Count == 0)
                        return;

                    SystemState.Instance().SetInspectState(InspectState.Wait);
                    onInspect = false;
                }

                lock (resultDic)
                    foreach (InspectorObj inspector in InspectorList)
                        resultDic[inspector.Info.CamIndex].RemoveAll(t => t.Item1 == foundedT.Item1);
            }
        }
    }
}
