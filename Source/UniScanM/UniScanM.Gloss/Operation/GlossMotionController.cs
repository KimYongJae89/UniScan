using DynMvp.Base;
using DynMvp.Device.Daq.Sensor.UsbSensorGloss_60;
using DynMvp.Device.Serial;
using DynMvp.Devices.MotionController;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniScanM.Gloss.Data;
using UniScanM.Gloss.MachineIF;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss.Operation
{
    public delegate void MoveDoneDelegate(float rollPosition);
    public delegate void StepMoveDoneDelegate(float position);

    public class GlossMotionController
    {
        #region 생성자
        internal GlossMotionController()
        {
            AxisConfiguration = SystemManager.Instance().DeviceBox.AxisConfiguration;
        }
        #endregion

        #region 대리자
        internal StepMoveDoneDelegate StepMoveDone = null;    // Step 이동 완료

        internal MoveDoneDelegate MoveDone = null;        // 트레버스 이동 완료
        #endregion

        #region 속성
        internal CancellationTokenSource CancellationTokenSource { get; set; }

        private AxisHandler AxisHandler => SystemManager.Instance().DeviceController.RobotStage;

        private PLCInspectStarter PLCInspectStarter => SystemManager.Instance().InspectStarter as UniScanM.Gloss.MachineIF.PLCInspectStarter;

        private GlossScanWidth ScanWidth { get; set; }

        private AxisConfiguration AxisConfiguration { get; set; }

        private bool IsStepCalDone { get; set; } = false;
        #endregion

        #region 메서드
        public void MoveSafetyPos(CancellationTokenSource cancellationTokenSource = null)
        {
            MovePos(GlossSettings.Instance().SafetyPosition, cancellationTokenSource);
        }

        public void MoveCalPos(CancellationTokenSource cancellationTokenSource = null)
        {
            MovePos(GlossSettings.Instance().CalibrationPosition, cancellationTokenSource);
        }

        public void MoveFarFromMirrorPosition(CancellationTokenSource cancellationTokenSource = null)
        {
            MovePos(GlossSettings.Instance().FarFromMirrorPosition, cancellationTokenSource);
        }

        public void MoveNearToMirrorRightPosition(CancellationTokenSource cancellationTokenSource = null)
        {
            MovePos(GlossSettings.Instance().NearToMirrorRightPosition, cancellationTokenSource);
        }

        public void MoveNearToMirrorLeftPosition(CancellationTokenSource cancellationTokenSource = null)
        {
            MovePos(GlossSettings.Instance().NearToMirrorLeftPosition, cancellationTokenSource);
        }

        public void MovePos(float position, CancellationTokenSource cancellationTokenSource = null)
        {
            if (AxisHandler != null)
            {
                var form = new SimpleProgressForm(StringManager.GetString(GetType().FullName, "Move to Position..."));
                form.Show(new Action(() => AxisHandler.Move(CalData.GetOffsetMoveAxisPosition(position), cancellationTokenSource)));
            }
        }

        public void ChangeMovingSpeed(float speed)
        {
            AxisParam axisParam = AxisConfiguration[0][0].AxisParam;
            axisParam.MovingParam.MaxVelocity = Convert.ToSingle((Convert.ToDouble(speed) * 1000 / axisParam.MicronPerPulse));
            AxisConfiguration.SaveConfiguration();
        }

        internal void ScanProc()
        {
            ScanWidth = GlossSettings.Instance().SelectedGlossScanWidth;

            var scanTargetPos = CalData.GetOffsetMoveAxisPosition(ScanWidth.Start);
            var scanStartPos = CalData.GetOffsetMoveAxisPosition(ScanWidth.Start);
            var scanEndPos = CalData.GetOffsetMoveAxisPosition(ScanWidth.End);

            int stepCount = GlossSettings.Instance().StepCount;
            float stepLength = Math.Abs(scanEndPos.Position[0] - scanStartPos.Position[0]) / (stepCount - 1);
            if (GlossSettings.Instance().RevesePosition)
            {
                stepLength = -stepLength;
            }

            float virtualRollPosition = 0;
            float virtualMotionPosition = 0;

            try
            {
                if (AxisHandler != null)
                {
                    var form = new SimpleProgressForm(StringManager.GetString(GetType().FullName, "Preparing Start..."));
                    form.Show(new Action(() => AxisHandler.Move(scanStartPos, CancellationTokenSource)));

                    CancellationTokenSource.Token.ThrowIfCancellationRequested();
                }

                while (true)
                {
                    if (AxisHandler != null)
                    {
                        for (int i = 0; i < stepCount; i++)
                        {
                            if (i > 0)
                            {
                                scanTargetPos.Position[0] += stepLength;
                                AxisHandler.StartMove(scanTargetPos);
                            }

                            // 모션 이동 대기
                            while (AxisHandler.IsMoveDone() == false)
                            {
                                CancellationTokenSource.Token.ThrowIfCancellationRequested();
                                Thread.Sleep(10);
                            }
                            IsStepCalDone = false;

                            Task.Run(() => StepMoveDone(CalData.GetOffsetPositionFromActualPosition(AxisHandler.GetActualPos().GetPosition()[0] / 1000f)));
                            // 계산 완료 대기
                            while (IsStepCalDone == false)
                            {
                                CancellationTokenSource.Token.ThrowIfCancellationRequested();
                                Thread.Sleep(10);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < stepCount; i++)
                        {
                            if (i > 0)
                            {
                                virtualMotionPosition += stepLength / 1000;
                                // 가상 모션 이동 대기
                                Thread.Sleep(100);
                            }
                            IsStepCalDone = false;
                            Task.Run(() => StepMoveDone(virtualMotionPosition));
                            // 계산 완료 대기
                            while (IsStepCalDone == false)
                            {
                                CancellationTokenSource.Token.ThrowIfCancellationRequested();
                                Thread.Sleep(10);
                            }
                        }
                    }

                    float rollPosition = 0;
                    if (PLCInspectStarter != null && PLCInspectStarter.MelsecMonitor.State.IsConnected == true)
                    {
                        rollPosition = Convert.ToSingle(SystemManager.Instance().InspectStarter.GetPosition());
                    }
                    else
                    {
                        virtualRollPosition += 10;
                        rollPosition = virtualRollPosition;
                    }
                    var moveDoneTask = Task.Run(() => MoveDone(rollPosition));
                    Task.WaitAll(moveDoneTask);

                    // 방향 전환
                    stepLength = -stepLength;
                }
            }
            catch (OperationCanceledException ex)
            {
                if (AxisHandler != null)
                {
                    var form = new SimpleProgressForm(StringManager.GetString(GetType().FullName, "Move to Safety Position..."));
                    form.Show(new Action(() =>
                    {
                        AxisHandler.StopMove();
                        MoveSafetyPos(CancellationTokenSource);
                    }));
                }
            }
        }

        internal void StepCalDone()
        {
            IsStepCalDone = true;
        }

        internal bool SettingSequence()
        {
            return true;
        }
        #endregion
    }
}

// Code Keep

//public bool SettingSequence()
//{
//    bool lampError = false;
//    string modelPath = PathConfig.Instance().Model;
//    SystemConfig systemConfig = SystemConfig.Instance();

//    // 포지션 일치
//    float positionOffset = systemConfig.PositionOffset * 1000;
//    float scanReferencePos = systemConfig.ReferencePos.GetPosition()[0];
//    float scanBackGroundPos = systemConfig.BackGroundPos.GetPosition()[0];

//    scanReferencePos = positionOffset - scanReferencePos;
//    scanBackGroundPos = positionOffset - scanBackGroundPos;

//    if (axisHandler != null)
//    {
//        SystemState.Instance().SetInspectState(InspectState.Run);

//        Task task = MessageWindowHelper.ShowProgress(null, "Calibration...", new Action(() =>
//        {
//            axisHandler.HomeMove(CancellationTokenSource.Token);

//            spectrometer.ScanStartStop(true);
//            Thread.Sleep(500);
//            axisHandler.Move(0, scanReferencePos, CancellationTokenSource.Token);
//            Thread.Sleep(500);
//            lampError = CheckLampState();
//            spectrometer.SaveRefSpectrum(modelPath);
//            axisHandler.Move(0, scanBackGroundPos, CancellationTokenSource.Token);
//            Thread.Sleep(500);
//            spectrometer.SaveBGSpectrum(modelPath);

//            spectrometer.ScanStartStop(false);
//        }));

//        SystemState.Instance().SetIdle();
//    }

//    if (lampError == true)
//    {
//        Task task = MessageWindowHelper.ShowMessageBox(null, "Lamp Error!! Please Check the Lamp", System.Windows.MessageBoxButton.OK);
//        return false;
//    }

//    return true;
//}

//private bool CheckLampState()
//{
//    int specAvgCount = 5;
//    int lampAvgCount = 5;
//    bool isLampOK = true;

//    SpectrometerProperty property = SystemConfig.Instance().SpectrometerProperty;

//    Dictionary<string, int> averageCountList = new Dictionary<string, int>();
//    Dictionary<string, int> lampPitchList = new Dictionary<string, int>();

//    Dictionary<string, double[]> graphRawData = new Dictionary<string, double[]>();
//    Dictionary<string, double> maxValueList = new Dictionary<string, double>();
//    Dictionary<string, double> sumValueList = new Dictionary<string, double>();

//    foreach (var info in spectrometer.DeviceList.Values)
//    {
//        averageCountList.Add(info.Name, spectrometer.Wrapper.getScansToAverage(info.Index));
//        lampPitchList.Add(info.Name, property.LampPitch[spectrometer.Wrapper.getName(info.Index)]);
//        maxValueList.Add(info.Name, float.MinValue);
//        sumValueList.Add(info.Name, float.MinValue);
//        spectrometer.Wrapper.setScansToAverage(info.Index, specAvgCount);
//    }
//    Thread.Sleep(500);

//    for (int k = 0; k < lampAvgCount; k++)
//    {
//        graphRawData = spectrometer.GetNewSpectrum();

//        foreach (var info in spectrometer.DeviceList.Values)
//        {
//            maxValueList[info.Name] = float.MinValue;
//            for (int i = 0; i < graphRawData[info.Name].Count(); i++)
//                maxValueList[info.Name] = Math.Max(maxValueList[info.Name], graphRawData[info.Name][i]);

//            sumValueList[info.Name] += maxValueList[info.Name];
//        }
//    }

//    foreach (var info in spectrometer.DeviceList.Values)
//    {
//        spectrometer.Wrapper.setScansToAverage(info.Index, averageCountList[info.Name]);

//        maxValueList[info.Name] = sumValueList[info.Name] / lampAvgCount;
//        isLampOK = isLampOK && (maxValueList[info.Name] > lampPitchList[info.Name]);
//    }

//    return isLampOK;
//}

//public void ModelOpened(Model.ModelDescription modelDescription)
//{
//    SystemConfig config = SystemConfig.Instance();
//    scanWidth = config.ScanWidthList.Find(x => x.Name == modelDescription.ScanWidth);
//}

//public void ModelClosed()
//{
//    scanWidth = null;
//}