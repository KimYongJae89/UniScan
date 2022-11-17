using System;
using System.Collections.Generic;
using System.Linq;

using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.UI.Touch;

using UniEye.Base.Data;
using UniEye.Base.Inspect;
using DynMvp.InspData;
using DynMvp.Device.Serial;
using DynMvp.Devices.Comm;
using UniScanM.CEDMS.Operation;
using UniScanM.CEDMS.State;
using UniScanM.State;
using UniScanM.CEDMS.Settings;
using System.Threading.Tasks;
using System.Threading;
using UniEye.Base.Settings;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using DynMvp.Device.Device.Serial.Sensor.SC_HG1_485;
using DynMvp.Device.Serial.Sensor;
//using UniScanM.Data;

namespace UniScanM.CEDMS.Operation
{
    public class InspectRunner : UniScanM.Operation.InspectRunner
    {
        const float sensorMaxData = 999.0f;
        const float sensorMinData = -999.0f;

        bool resetZeroing;
        bool lotChanged;
        ThreadHandler runningThread;

        SerialSensor serialSensor = null;

        Random rand1 = new Random(1);
        Random rand2 = new Random(1);

        DateTime inspectStartedTime = DateTime.Now;
        Stopwatch stopWatch;

        public InspectRunner() : base()
        {
            SerialDeviceHandler serialDeviceHandler = SystemManager.Instance().DeviceBox.SerialDeviceHandler;
            SerialDevice serialDevice = serialDeviceHandler.Find(x => x.DeviceInfo.DeviceType == ESerialDeviceType.SerialSensor);
            if (serialDevice is SerialSensor)
            {
                SerialSensor serialSensor = (SerialSensor)serialDevice;

                if (serialSensor != null && serialSensor.SerialPortEx != null)
                {
                    //serialSensor.SerialPortEx.PacketHandler.PacketParser.OnDataReceived += Sensor_OnDataReceived;
                    this.serialSensor = serialSensor;
                }

                SystemManager.Instance().InspectStarter.OnStartInspection += EnterWaitInspection;
                SystemManager.Instance().InspectStarter.OnStopInspection += ExitWaitInspection;
                SystemManager.Instance().InspectStarter.OnRewinderCutting += OnRewinderCutting;
                SystemManager.Instance().InspectStarter.OnLotChanged += OnLotChanged;
            }
            stopWatch = new Stopwatch();
        }

        public override void ResetState()
        {
            resetZeroing = true;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        void OnLotChanged()
        {
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(f => f.ClearPanel());
            lotChanged = true;
        }

        void OnRewinderCutting()
        {
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(f => f.ClearPanel());
        }

        public override bool EnterWaitInspection()
        {
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(panel => panel.ClearPanel());

            // TODO:[Song] 센서 자체 ZeroSet 기능 사용
            //this.inspectProcesser = new ZeroingState();

            this.processer = new InspectionState(0, 0, true);
            return PostEnterWaitInspection();
        }

        public override bool PostEnterWaitInspection()
        {
            runningThread = new ThreadHandler("SensorWorker", new Thread(runningThreadProc), false);
            runningThread.Start();

            resetZeroing = true;

            SystemState.Instance().SetInspect();
            SystemState.Instance().SetInspectState(InspectState.Run);

            return true;
        }

        private void runningThreadProc()
        {
            int sleepTime = (int)1000.0f / CEDMSSettings.Instance().DataGatheringCountPerSec;
            while (runningThread.RequestStop == false)
            {
                Thread.Sleep(sleepTime);
                //if(MachineSettings.Instance().VirtualMode)
                //{
                //    VirtualMode();
                //}
                //else
                {
                    RealModel();
                }
            }
        }

        // TODO : [Jena] 감도설정했을때,, Alarm이 뜨면,,, 999이면 0으로 데이터 표시해달라고 하셨는데,.
        // 여길탈텐데.. 여기서 999일때 데이터 0으로 하면,, 그럼 알람일 경우 무조건 데이터 0이 되는데..?
        void RealModel()
        {
            float[] datas = new float[2];
            bool ok;

            if (lotChanged)
            {
                ok = serialSensor.GetData(2, datas) & CheckSheetAbsence(datas[0], datas[1]);
                lotChanged = false;
            }
            else
                ok = serialSensor.GetData(2, datas);
            
            if (ok == true)
            {
                Sensor_OnDataReceived(datas[0], datas[1]);
            }
        }

        private bool CheckSheetAbsence(params float[] datas)
        {
            if ((datas[0] == sensorMinData || datas[0] == sensorMaxData) && (datas[1] == sensorMinData || datas[1] == sensorMaxData))
            {
                MessageForm.Show(null, "The sheets on the infeed and outfeed sides do not exist.", "C-EDMS (InFeeder &&& OutFeeder)", Color.Red);
                return false;
            }
            else if (datas[0] == sensorMinData || datas[0] == sensorMaxData)
            {
                MessageForm.Show(null, "The sheet on the infeed side does not exist.", "C-EDMS (InFeeder)", Color.Red);
                return false;
            }
            else if (datas[1] == sensorMinData || datas[1] == sensorMaxData)
            {
                MessageForm.Show(null, "The sheet on the outfeed side does not exist.", "C-EDMS (OutFeeder)", Color.Red);
                return false;
            }
            return true;
        }

        public override void ExitWaitInspection()
        {
            runningThread?.Stop();

            if (runningThread != null)
            {
                SerialSensorSC_HG1_485 sensor = serialSensor as SerialSensorSC_HG1_485;
                if (sensor != null && !(sensor is SerialSensorSC_HG1_485Virtual))
                {
                    if (sensor.GetPresetInfo(1))
                    {
                        Thread.Sleep(10);
                        sensor.Preset(true, false);
                        Thread.Sleep(10);
                        sensor.Preset(false, false);
                    }
                }
            }

            runningThread = null;

            SystemState.Instance().SetIdle();
            SystemManager.Instance().ProductionManager.Save();

            //if (SystemManager.Instance().InspectStarter.StartMode == StartMode.Stop)
            //    SystemState.Instance().SetWait();
            ////SystemState.Instance().SetInspectState(InspectState.Stop);
            //else
            //    SystemState.Instance().SetIdle();
            ////SystemState.Instance().SetInspectState(InspectState.Ready);
        }

        public override void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            Data.InspectionResult cedmsInspectionResult = inspectionResult as Data.InspectionResult;

            LogHelper.Debug(LoggerType.Function, "InspectRunner::ProductInspected");
            inspectionResult.InspectionEndTime = DateTime.Now;
            inspectionResult.InspectionTime = (inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime);

            MachineState state = ((PLCInspectStarter)SystemManager.Instance().InspectStarter).MelsecMonitor.StateCopy;

            cedmsInspectionResult.LineSpeed = state.SpSpeed;

            if (cedmsInspectionResult.ZeroingComplete)
            {
                UniScanM.Data.Production production = this.UpdateProduction(cedmsInspectionResult);
                //UniScanM.Data.Production production = SystemManager.Instance().ProductionManager.GetProduction(rvmsInspectionResult);
                lock (production)
                {
                    //production.Update(rvmsInspectionResult);

                    float value = Math.Abs(cedmsInspectionResult.InFeed.Y - cedmsInspectionResult.OutFeed.Y);
                    production.Value = Math.Max(value, production.Value);
                }
            }
            SystemManager.Instance().MainForm.InspectPage.ProductInspected(inspectionResult);

            if (processer is InspectionState)
                SystemManager.Instance().ExportData(inspectionResult);
        }

        private void Sensor_OnDataReceived(params float[] datas)
        {
            if (SystemManager.Instance().InspectStarter.StartMode == StartMode.Stop)
                return;

            try
            {
                CEDMS.Data.InspectionResult inspectionResult = (CEDMS.Data.InspectionResult)inspectRunnerExtender.BuildInspectionResult();

                if (resetZeroing == true)
                {
                    inspectStartedTime = DateTime.Now;
                    inspectionResult.ResetZeroing = this.resetZeroing;

                    SerialSensorSC_HG1_485 sensor = serialSensor as SerialSensorSC_HG1_485;

                    if (sensor != null && !(sensor is SerialSensorSC_HG1_485Virtual))
                    {
                        if (sensor.GetPresetInfo(1))
                        {
                            Thread.Sleep(10);
                            sensor.Preset(true, false);
                            Thread.Sleep(10);
                            sensor.Preset(false, false);
                        }
                        Thread.Sleep(10);
                        sensor.Preset(true, true);

                        Thread.Sleep(10);
                        sensor.Preset(false, true);
                    }

                    resetZeroing = false;
                }
                else
                {
                    inspectionResult.Judgment = Judgment.Accept;
                    
                    if (datas[0] <= sensorMinData || datas[0] >= sensorMaxData)
                        datas[0] = 0.0f;

                    if (datas[1] <= sensorMinData || datas[1] >= sensorMaxData)
                        datas[1] = 0.0f;

                    inspectionResult.CurValueList.AddRange(datas);

                    inspectionResult.CurValueList[0] += CEDMSSettings.Instance().InFeedOffset;
                    if (inspectionResult.CurValueList.Count > 1)
                        inspectionResult.CurValueList[1] += CEDMSSettings.Instance().OutFeedOffset;

                    this.processer.Process(null, inspectionResult, null);
                    inspectionResult.FirstTime = inspectStartedTime;

                    ProductInspected(inspectionResult);

                    // TODO:[Song] 센서 자체 ZeroSet 기능 사용
                    //this.inspectProcesser = ((UniScanState)InspectProcesser).GetNextState(inspectionResult);
                }
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Error, e.Message);
            }
            //SystemState.Instance().SetWait();
        }

        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            throw new NotImplementedException();
        }
    }
}
