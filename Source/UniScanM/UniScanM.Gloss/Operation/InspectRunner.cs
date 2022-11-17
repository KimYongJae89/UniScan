using DynMvp.Base;
using DynMvp.Device.Device.Serial.Sensor.CD22_15_485;
using DynMvp.Device.Serial;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniScanM.Gloss.Data;
using UniScanM.Gloss.Settings;
using UniScanM.Gloss.State;
using UniScanM.Operation;
using UniScanM.UI;

namespace UniScanM.Gloss.Operation
{
    public class InspectRunner : UniScanM.Operation.InspectRunner
    {
        #region 생성자
        public InspectRunner() : base()
        {
            SystemManager.Instance().InspectStarter.OnStartInspection += EnterWaitInspection;
            SystemManager.Instance().InspectStarter.OnStopInspection += ExitWaitInspection;
            SystemManager.Instance().InspectStarter.OnRewinderCutting += OnRewinderCutting;
            SystemManager.Instance().InspectStarter.OnLotChanged += OnLotChanged;

            // Commanager
            if (GlossSettings.Instance().UseModuleMode)
            {
                if (MachineIF.GlossCommManager.Instance() is MachineIF.GlossCommManager commManager)
                {
                    commManager.EnterWaitDelegate += MqttEnterWaitInspection;
                    commManager.ExitWaitDelegate += MqttExitWaitInspection;
                }
            }

            GlossMotionController = new GlossMotionController();
            GlossDataCalculator = new GlossDataCalculator();
            GlossMotionController.StepMoveDone = GlossDataCalculator.StepMoveDone;
            GlossMotionController.MoveDone = GlossDataCalculator.MoveDone;
            GlossDataCalculator.StepCalDone = GlossMotionController.StepCalDone;
            GlossDataCalculator.CalDone = DataCalculatorCalDone;
        }
        #endregion

        #region 속성
        public GlossMotionController GlossMotionController { get; private set; }

        public GlossDataCalculator GlossDataCalculator { get; private set; }

        public List<InspectionResult> InspectionResultList { get; private set; } = new List<InspectionResult>();

        public CalibrationData CalibrationData { get; set; } = new CalibrationData();

        private CancellationTokenSource CancellationTokenSource { get; set; }

        private AxisHandler AxisHandler { get => SystemManager.Instance().DeviceController.RobotStage; }

        private SerialDeviceHandler SerialDeviceHandler { get => SystemManager.Instance().DeviceBox.SerialDeviceHandler; }

        private bool LotChanged { get; set; } = false;
        #endregion

        #region 메서드
        private bool MqttEnterWaitInspection(string lotName)
        {
            if (SystemManager.Instance().MainForm.InspectPage is InspectionPage inspectionPage)
            {
                if (inspectionPage.ChangeStartMode(StartMode.Manual))
                {
                    GlossSettings.Instance().ManualLotNo = lotName;
                    SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(panel => panel.EnterWaitInspection());
                    SystemManager.Instance().InspectStarter.PreStartInspect(false);
                    EnterWaitInspection();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        private bool MqttExitWaitInspection()
        {
            if (SystemManager.Instance().MainForm.InspectPage is InspectionPage inspectionPage)
            {
                if (inspectionPage.ChangeStartMode(StartMode.Stop))
                {
                    ExitWaitInspection();
                    SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(panel => panel.ExitWaitInspection());
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override bool EnterWaitInspection()
        {
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(panel => panel.ClearPanel());
            this.processer = new InspectionState();
            return PostEnterWaitInspection();
        }

        public override bool PostEnterWaitInspection()
        {
            initializeComponent();

            CalibrationProcess();

            InspectionResultList.Clear();

            Task ScanProcTask = Task.Run(() =>
            {
                GlossDataCalculator.ScanProc();
                GlossMotionController.ScanProc();
            }, CancellationTokenSource.Token);

            SystemState.Instance().SetInspect();
            SystemState.Instance().SetInspectState(InspectState.Run);

            return true;
        }

        public override void ProductInspected(DynMvp.InspData.InspectionResult inspectionResult)
        {
            Data.InspectionResult glossInspectionResult = inspectionResult as Data.InspectionResult;

            LogHelper.Debug(LoggerType.Function, "InspectRunner::ProductInspected");
            inspectionResult.InspectionEndTime = DateTime.Now;
            inspectionResult.InspectionTime = (inspectionResult.InspectionEndTime - inspectionResult.InspectionStartTime);

            MachineState state = ((PLCInspectStarter)SystemManager.Instance().InspectStarter).MelsecMonitor.StateCopy;

            UniScanM.Data.Production production = this.UpdateProduction(glossInspectionResult);
            lock (production)
            {
                float value = glossInspectionResult.GlossScanData.AvgGloss;
                production.Value = Math.Max(value, production.Value);
            }

            SystemManager.Instance().MainForm.InspectPage.ProductInspected(inspectionResult);

            if (processer is InspectionState)
                SystemManager.Instance().ExportData(inspectionResult);

            InspectionResultList.Add(glossInspectionResult);
        }

        public override void ExitWaitInspection()
        {
            CancellationTokenSource?.Cancel();

            Thread.Sleep(1000);
            GlossMotionController.MoveSafetyPos();

            SystemState.Instance().SetIdle();
            SystemManager.Instance().ProductionManager.Save();

            foreach (var dataExporter in SystemManager.Instance().DataExporterList)
            {
                if (dataExporter is MachineIF.MachineIfDataExporter machineIfDataExporter)
                {
                    machineIfDataExporter.ExportLotTotalResult(InspectionResultList);
                }
                else if (dataExporter is ReportDataExporter reportDataExporter)
                {
                    reportDataExporter.SaveLotTotalResult(InspectionResultList);
                }
            }
        }

        public void initializeComponent()
        {
            CancellationTokenSource = new CancellationTokenSource();

            GlossDataCalculator.CancellationTokenSource = CancellationTokenSource;
            GlossMotionController.CancellationTokenSource = CancellationTokenSource;

            this.CalibrationData = new CalibrationData();

            if (GlossSettings.Instance().UseModuleMode)
            {
                var glossSettings = GlossSettings.Instance();
                var dbDataExporter = SystemManager.Instance().DataExporterList.Find(x => x is DBDataExporter) as DBDataExporter;
                if (dbDataExporter != null)
                {
                    dbDataExporter.SetDataBaseInfo(glossSettings.CMDBIpAddress, glossSettings.CMDBName, glossSettings.CMDBUserName, glossSettings.CMDBPassword);
                }

                dbDataExporter.SetLotInfo(GlossSettings.Instance().ManualLotNo);
            }
        }

        public void CalibrationProcess()
        {
            this.CalibrationData = new CalibrationData();
            if (AxisHandler != null)
            {
                SimpleProgressForm form = new SimpleProgressForm(StringManager.GetString(this.GetType().FullName, "Calibration..."));
                form.Show(new Action(() =>
                {
                    foreach (SerialSensorCD22_15_485 sensor in SerialDeviceHandler)
                        sensor.LaserOn();

                    AxisHandler.HomeMove(CancellationTokenSource);

                    if (CheckDistanceSensor())
                    {
                        if (GlossSettings.Instance().UseAutoCalibration)
                        {
                            if (AutoCalibration() == false)
                            {
                                LogHelper.Error(LoggerType.Error, "InspectRunner::PostEnterWaitInspection 광택도 측정기의 값 보정 실패");
                                return;
                            }
                        }
                    }
                    else
                    {
                        //MessageForm.Show(null, "광택도 측정기의 각도를 맞춰주십시오.");
                        LogHelper.Error(LoggerType.Error, "InspectRunner::PostEnterWaitInspection 광택도 측정기의 각도 불일치");
                        return;
                    }
                }));
            }
        }

        public double MeasureBlackMirror()
        {
            GlossMotionController.MoveCalPos();
            return GlossDataCalculator.GetGlossData();
        }

        public bool CheckDistanceSensor()
        {
            var farSensorValueList = MeasureFarFromMirrorSensor();
            var nearSensorValueList = MeasureNearToMirrorSensor();

            if (farSensorValueList.Count == 0 || nearSensorValueList.Count == 0)
                return false;

            // X 축을 폭방향, Y 축을 길이방향 이라고 생각한다.
            float distance = Math.Abs(GlossSettings.Instance().NearToMirrorLeftPosition - GlossSettings.Instance().NearToMirrorRightPosition);
            float xDiff = Math.Abs(nearSensorValueList[0] - nearSensorValueList[1]);
            float yDiff = Math.Abs(farSensorValueList[0] - (nearSensorValueList[0] + nearSensorValueList[1]) / 2);

            float xDiffDegree = (float)((180.0 / Math.PI) * Math.Atan2(xDiff, distance));
            float yDiffDegree = (float)((180.0 / Math.PI) * Math.Atan2(yDiff, 124.2));

            CalibrationData.AxisXDiffDegree = xDiffDegree;
            CalibrationData.AxisYDiffDegree = yDiffDegree;
            CalibrationData.FarFromMirrorSensorValue = farSensorValueList[0];
            CalibrationData.NearToMirrorSensorLeftSideValue = nearSensorValueList[0];
            CalibrationData.NearToMirrorSensorRightSideValue = nearSensorValueList[1];

            float baseXDegree = GlossSettings.Instance().AxisXDiffDegree * (1 + GlossSettings.Instance().DistanceSensorTolerance / 100);
            float baseYDegree = GlossSettings.Instance().AxisYDiffDegree * (1 + GlossSettings.Instance().DistanceSensorTolerance / 100);

            // 전에 저장되어있던 거랑 비교,,?
            //if (xDiffDegree > baseXDegree || yDiffDegree > baseYDegree)
            //    return false;

            return true;
        }

        internal List<float> MeasureFarFromMirrorSensor()
        {
            var farSensorValueList = new List<float>();

            GlossMotionController.MoveFarFromMirrorPosition();
            if (GlossDataCalculator.GetFarFromMirrorDistanceData(out float[] dataFar) == true)
            {
                var farValue = Convert.ToSingle(dataFar[0]);
                GlossSettings.Instance().FarFromMirrorSensorValue = farValue;
            }

            if (GlossSettings.Instance().FarFromMirrorSensorValue != -1)
            {
                farSensorValueList.Add(GlossSettings.Instance().FarFromMirrorSensorValue);
                farSensorValueList.Add(0);
            }

            return farSensorValueList;
        }

        internal List<float> MeasureNearToMirrorSensor()
        {
            var nearSensorValueList = new List<float>();

            GlossMotionController.MoveNearToMirrorRightPosition();
            if (GlossDataCalculator.GetNearToMirrorDistanceData(out float[] datasBR) == true)
            {
                var topRightValue = Convert.ToSingle(datasBR[0]);
                GlossSettings.Instance().NearToMirrorSensorRightSideValue = topRightValue;
            }

            GlossMotionController.ChangeMovingSpeed(Convert.ToSingle(GlossSettings.Instance().ReferenceMovingSpeed));

            GlossMotionController.MoveNearToMirrorLeftPosition();
            if (GlossDataCalculator.GetNearToMirrorDistanceData(out float[] datasBL) == true)
            {
                var topLeftValue = Convert.ToSingle(datasBL[0]);
                GlossSettings.Instance().NearToMirrorSensorLeftSideValue = topLeftValue;
            }

            GlossMotionController.ChangeMovingSpeed(Convert.ToSingle(GlossSettings.Instance().MovingSpeed));

            if (GlossSettings.Instance().NearToMirrorSensorLeftSideValue != -1 && GlossSettings.Instance().NearToMirrorSensorRightSideValue != -1)
            {
                nearSensorValueList.Add(GlossSettings.Instance().NearToMirrorSensorLeftSideValue);
                nearSensorValueList.Add(GlossSettings.Instance().NearToMirrorSensorRightSideValue);
            }

            return nearSensorValueList;
        }

        public bool AutoCalibration()
        {
            GlossMotionController.MoveCalPos();
            CalibrationData.BeforeBlackMirrorValue = GlossDataCalculator.GetGlossData();

            var isOk = GlossDataCalculator.GlossCalibration();
            if (isOk == true)
            {
                CalibrationData.AfterBlackMirrorValue = GlossDataCalculator.GetGlossData();
            }
            else
            {
                LogHelper.Error(LoggerType.Error, "GlossMotionController::Calibration 실패");
            }

            return isOk;
        }

        private void DataCalculatorCalDone(GlossScanData scanData)
        {
            if (SystemManager.Instance().InspectStarter.StartMode == StartMode.Stop)
                return;

            try
            {
                Gloss.Data.InspectionResult inspectionResult = (Gloss.Data.InspectionResult)inspectRunnerExtender.BuildInspectionResult();
                inspectionResult.Judgment = DynMvp.InspData.Judgment.Accept;
                inspectionResult.GlossScanData = scanData;
                inspectionResult.CalibrationData = new CalibrationData(CalibrationData);

                this.processer.Process(null, inspectionResult, null);

                ProductInspected(inspectionResult);

                this.processer = ((UniScanM.State.UniScanState)Processer).GetNextState(inspectionResult);
            }
            catch (Exception e)
            {
                LogHelper.Debug(LoggerType.Error, e.Message);
            }
        }

        private void OnLotChanged()
        {
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(f => f.ClearPanel());
            LotChanged = true;
        }

        private void OnRewinderCutting()
        {
            SystemManager.Instance().MainForm.InspectPage.InspectionPanelList.ForEach(f => f.ClearPanel());
        }
        #endregion
    }

    public class CalibrationData
    {
        public float AxisXDiffDegree { get; set; } = 0;
        public float AxisYDiffDegree { get; set; } = 0;
        public float FarFromMirrorSensorValue { get; set; } = 0;
        public float NearToMirrorSensorLeftSideValue { get; set; } = 0;
        public float NearToMirrorSensorRightSideValue { get; set; } = 0;
        public float BeforeBlackMirrorValue { get; set; } = 0;
        public float AfterBlackMirrorValue { get; set; } = 0;

        public CalibrationData(CalibrationData calibrationData = null)
        {
            if (calibrationData != null)
            {
                this.AxisXDiffDegree = calibrationData.AxisXDiffDegree;
                this.AxisYDiffDegree = calibrationData.AxisYDiffDegree;
                this.FarFromMirrorSensorValue = calibrationData.FarFromMirrorSensorValue;
                this.NearToMirrorSensorLeftSideValue = calibrationData.NearToMirrorSensorLeftSideValue;
                this.NearToMirrorSensorRightSideValue = calibrationData.NearToMirrorSensorRightSideValue;
                this.BeforeBlackMirrorValue = calibrationData.BeforeBlackMirrorValue;
                this.AfterBlackMirrorValue = calibrationData.AfterBlackMirrorValue;
            }
        }
    }
}
