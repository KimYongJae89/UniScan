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
using UniScanM.RVMS.Operation;
using UniScanM.RVMS.State;
using UniScanM.State;
using UniScanM.RVMS.Settings;
using System.Threading.Tasks;
using System.Threading;
using UniEye.Base.Settings;
using System.Diagnostics;
using System.IO;
using DynMvp.Device.Serial.Sensor;
//using UniScanM.Data;

namespace UniScanM.RVMS.Operation
{
    public class InspectRunner : UniScanM.Operation.InspectRunner
    {
        
        bool resetZeroing;
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
        }        

        void OnRewinderCutting()
        {
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(f => f.ClearPanel());
        }

        public override bool EnterWaitInspection()
        {
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(panel => panel.ClearPanel());
            
            this.processer = new ZeroingState();           
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
            int sleepTime = (int)1000.0f / RVMSSettings.Instance().DataGatheringCountPerSec;
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

        void RealModel()
        {
            float[] datas = new float[2];
            bool ok = serialSensor.GetData(2, datas);
            //d1 += RVMSSettings.Instance().Setting.GearSideOffset;
            //d2 += RVMSSettings.Instance().Setting.ManSideOffset;
            if (ok)
            {
                Sensor_OnDataReceived(datas[0], datas[1]);
            }
        }

        public override void ExitWaitInspection()
        {
            runningThread?.Stop();
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
            Data.InspectionResult rvmsInspectionResult = inspectionResult as Data.InspectionResult;

            LogHelper.Debug(LoggerType.Function, "InspectRunner::ProductInspected");
            inspectionResult.InspectionEndTime = DateTime.Now;
            inspectionResult.InspectionTime = (inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime);

            if (rvmsInspectionResult.ZeroingComplate)
            {
                UniScanM.Data.Production production = this.UpdateProduction(rvmsInspectionResult);
                //UniScanM.Data.Production production = SystemManager.Instance().ProductionManager.GetProduction(rvmsInspectionResult);
                lock (production)
                {
                    //production.Update(rvmsInspectionResult);

                    float value = Math.Abs(rvmsInspectionResult.GearSide.Y - rvmsInspectionResult.ManSide.Y);
                    production.Value = Math.Max(value, production.Value);
                }
            }
            SystemManager.Instance().MainForm.InspectPage.ProductInspected(inspectionResult);
            
            if (Processer is InspectionState)
                SystemManager.Instance().ExportData(inspectionResult);
        }

        private void Sensor_OnDataReceived(params float[] datas)
        {
            if (SystemManager.Instance().InspectStarter.StartMode == StartMode.Stop)
                return;

            try
            {
                RVMS.Data.InspectionResult inspectionResult = (RVMS.Data.InspectionResult)inspectRunnerExtender.BuildInspectionResult();
                inspectionResult.Judgment = Judgment.Accept;

                //float[] converted = Array.ConvertAll(datas, f => f * -1);

                //SystemState.Instance().SetInspect();

                inspectionResult.CurValueList.AddRange(datas);
                inspectionResult.CurValueList[0] += RVMSSettings.Instance().ManSideOffset;
                if (inspectionResult.CurValueList.Count > 1)
                    inspectionResult.CurValueList[1] += RVMSSettings.Instance().GearSideOffset;

                this.processer.Process(null, inspectionResult, null);
                inspectionResult.FirstTime = inspectStartedTime;

                ProductInspected(inspectionResult);

                if (resetZeroing == true)
                {
                    inspectStartedTime = DateTime.Now;
                    inspectionResult.ResetZeroing = this.resetZeroing;
                    resetZeroing = false;
                }

                this.processer = ((UniScanState)Processer).GetNextState(inspectionResult);
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
