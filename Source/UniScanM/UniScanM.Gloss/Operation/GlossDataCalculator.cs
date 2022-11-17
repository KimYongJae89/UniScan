using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Device.Serial;
using DynMvp.Device.Serial.Sensor;
using DynMvp.Devices.Daq;
using UniScanM.Gloss.Data;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss.Operation
{
    public delegate void CalDoneDelegate(GlossScanData scanData);
    public delegate void StepCalDoneDelegate();

    public class GlossDataCalculator
    {
        #region 생성자
        public GlossDataCalculator() { }
        #endregion


        #region 대리자
        public StepCalDoneDelegate StepCalDone = null;  // Step 이동 완료

        public CalDoneDelegate CalDone = null;          // 트레버스 이동 완료
        #endregion


        #region 열거형
        private enum SensorPosition
        {
            NextToGloss = 0, FarFromMirror = 1, NearToMirror = 2
        }
        #endregion


        #region 속성
        internal CancellationTokenSource CancellationTokenSource { get; set; }

        private DaqChannel GlossSensor => SystemManager.Instance().DeviceBox.DaqChannelList.Count > 0 ? SystemManager.Instance().DeviceBox.DaqChannelList[0] : null;

        private SerialDeviceHandler SerialDeviceHandler => SystemManager.Instance().DeviceBox.SerialDeviceHandler.Count > 0 ? SystemManager.Instance().DeviceBox.SerialDeviceHandler : null;

        private float SerialSensorData = 0;

        private GlossScanData ScanData { get; set; } = null;

        private bool CanMeasure { get; set; } = true;
        #endregion


        #region 메서드
        public float GetGlossData()
        {
            return Convert.ToSingle(GlossSensor.ReadData(1).Average());
        }

        public bool GetCenterDistanceData(out float[] datas)
        {
            return GetDistanceData(SensorPosition.NextToGloss, out datas);
        }

        public bool GetFarFromMirrorDistanceData(out float[] datas)
        {
            return GetDistanceData(SensorPosition.FarFromMirror, out datas);
        }

        public bool GetNearToMirrorDistanceData(out float[] datas)
        {
            return GetDistanceData(SensorPosition.NearToMirror, out datas);
        }

        public bool GlossCalibration()
        {
            var glossSensor = GlossSensor as DaqUSBGloss60;
            return glossSensor.Calibration();
        }

        public void ScanProc()
        {
            InitializeScanData();
        }

        // 모션이 한 스텝 이동을 완료하면 StepMoveDone Delegate 가 온다.
        public void StepMoveDone(float position)
        {
            int avgCount = GlossSettings.Instance().AvgCount;
            float glossSensorData = 0;

            if (GlossSensor != null)
            {
                double orgData = GlossSensor.ReadData(avgCount).Average();
                GlossCalibrationData param = GlossSettings.Instance().SelectedGlossCalibrationParam;
                if (param != null)
                {
                    float c3 = param.C3;
                    float c2 = param.C2;
                    float c1 = param.C1;
                    float c0 = param.C0;
                    glossSensorData = Convert.ToSingle(c3 * Math.Pow(orgData, 3) + c2 * Math.Pow(orgData, 2) + c1 * orgData + c0);
                }
                else
                {
                    glossSensorData = Convert.ToSingle(orgData);
                }
            }
            else
            {
                glossSensorData = Convert.ToSingle(new Random().NextDouble());
            }

            if (SerialDeviceHandler != null)
            {
                float[] datas = new float[1];
                bool isSensorOK = GetCenterDistanceData(out datas);

                bool isDistanceOK;
                //센서값이 셋되기전일때(처음한번)
                if (SerialSensorData == 0)
                {
                    isDistanceOK = true;
                }
                else
                {
                    isDistanceOK = Math.Max(datas[0], SerialSensorData) - Math.Min(datas[0], SerialSensorData) > datas[0] * GlossSettings.Instance().DistanceSensorTolerance / 100 ? false : true;
                }

                if (isSensorOK == false)
                {
                    LogHelper.Error(LoggerType.Error, "GlossDataCalculator::StepMoveDone 유효하지않은 거리센서 값");
                    //MessageForm.Show(null, "거리센서의 값이 유효하지않습니다..");
                }
                else if ((isSensorOK && isDistanceOK) == false)
                {
                    LogHelper.Error(LoggerType.Error, "GlossDataCalculator::StepMoveDone 거리센서 값 증가량 범위 초과");
                    //MessageForm.Show(null, "거리센서의 값의 증가량이 범위를 초과하였습니다.");
                }

                SerialSensorData = Convert.ToSingle(datas[0]);
            }
            else
            {
                SerialSensorData = Convert.ToSingle(new Random().NextDouble());
            }

            ScanData.GlossDatas.Add(new GlossData(position, glossSensorData, SerialSensorData));

            Task.Run(() => { StepCalDone(); });
        }

        // 모션이 끝에서 끝으로 이동을 완료하면 MoveDone Delegate 가 온다.
        public void MoveDone(float rollPosition)
        {
            ScanData.RollPosition = rollPosition;
            var logScanData = ScanData.Clone();
            Task.Run(() => { AfterScanTask(logScanData); });
            InitializeScanData();
        }

        private bool GetDistanceData(SensorPosition sensorPosition, out float[] datas)
        {
            var serialSensor = SerialDeviceHandler[(int)sensorPosition] as SerialSensor;
            datas = new float[1];
            if (serialSensor.GetData(1, datas) == true)
            {
                return true;
            }

            return false;
        }

        private void AfterScanTask(GlossScanData scanData)
        {
            // 데이터 방향 설정
            SortData(scanData);
            // 자체 데이터 계산
            SetAverageData(scanData);
            scanData.EndTime = DateTime.Now;
            // 데이터 완성
            Task.Run(() => { CalDone(scanData); });
        }

        private void InitializeScanData()
        {
            if (ScanData == null)
            {
                ScanData = new GlossScanData();
            }
            else
            {
                ScanData.Clear();
                ScanData.StartTime = DateTime.Now;
            }
        }

        // Call Data
        private void SortData(GlossScanData scanData)
        {
            scanData.GlossDatas.Sort((a, b) => a.X.CompareTo(b.X));
        }

        private void SetAverageData(GlossScanData scanData)
        {
            scanData.MaxGloss = scanData.GlossDatas.Max(x => x.Y);
            scanData.MinGloss = scanData.GlossDatas.Min(x => x.Y);
            scanData.AvgGloss = scanData.GlossDatas.Average(x => x.Y);
            scanData.DevGloss = DynMvp.Vision.DataProcessing.StdDev(scanData.GlossDatas.ConvertAll(x => x.Y).ToArray());

            scanData.MaxDistance = scanData.GlossDatas.Max(x => x.Distance);
            scanData.MinDistance = scanData.GlossDatas.Min(x => x.Distance);
            scanData.AvgDistance = scanData.GlossDatas.Average(x => x.Distance);
            scanData.DevDistance = DynMvp.Vision.DataProcessing.StdDev(scanData.GlossDatas.ConvertAll(x => x.Distance).ToArray());
        }
        #endregion
    }
}
