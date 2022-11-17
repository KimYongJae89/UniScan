using DynMvp.Base;
using DynMvp.Device.Serial;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanM.MachineIF;
using UniScanM.StillImage.MachineIF;
//using UniScanM.Settings;
using UniScanM.StillImage.Settings;

namespace UniScanM.StillImage.Operation
{
    /// <summary>
    /// Sance running and velosity stable to Encoder position
    /// </summary>
    public class EncoderInspectStarter : UniScanM.Operation.InspectStarter
    {
        SerialEncoder serialEncoder = null;
        public SerialEncoder SerialEncoder
        {
            get { return serialEncoder; }
        }

        Dictionary<DateTime, double> spdHistory = new Dictionary<DateTime, double>();
        Task testTesk = null;
        CancellationTokenSource testTeskCancellationTokenSource = null;

        string debugFile = "";

        bool isStable = false;
        public bool IsStable
        {
            get { return isStable; }
        }
        
        /// <summary>
        /// meter per min
        /// </summary>
        double curSpd = -1;
        public double CurSpd
        {
            get { return curSpd; }
        }

        /// <summary>
        /// meter per min
        /// </summary>
        double avgSpd = -1;
        public double AvgSpd
        {
            get { return avgSpd; }
        }

        public EncoderInspectStarter() : base()
        {
            serialEncoder = (SerialEncoder)SystemManager.Instance().DeviceBox.SerialDeviceHandler.Find(f => f.DeviceInfo.DeviceType == DynMvp.Device.Serial.ESerialDeviceType.SerialEncoder);
        }
        
        private void EncoderStartStopTestProc()
        {
            bool processed = false;
            while (testTeskCancellationTokenSource.IsCancellationRequested == false)
            {
                DateTime curDateTime = DateTime.Now;
                if (curDateTime.Minute % 10 == 0)
                {
                    if (processed == false)
                        debugFile = System.IO.Path.Combine(PathSettings.Instance().Temp, string.Format("EncoderVelocity_{0}.txt", curDateTime.ToString("yyMMdd_HHmm")));
                    processed = true;
                }
                if (curDateTime.Minute % 10 == 1)
                {
                    if (processed == false)
                    {
                        serialEncoder.ExcuteCommand(SerialEncoderV105.ECommand.IN, "1");
                        processed = true;
                    }
                }
                else if (curDateTime.Minute % 10 == 9)
                {
                    if (processed == false)
                    {
                        serialEncoder.ExcuteCommand(SerialEncoderV105.ECommand.IN, "0");
                        processed = true;
                    }
                }
                else if (processed)
                {
                    if (processed == true)
                        processed = false;
                }
                Thread.Sleep(500);
            }
            debugFile = "";
        }

        protected override void ThreadProc()
        {
            Thread.Sleep(1000);

            float umPerPls = 0.0f;
            DateTime prevDateTime = DateTime.MinValue;
            long prevPos = 0;// ExcuteAPCommand();
            int rwCutState = -1;
            System.Diagnostics.Stopwatch commStopWatch = new System.Diagnostics.Stopwatch();
            Settings.StillImageSettings stillImageSettings = Settings.StillImageSettings.Instance();
            while (RequestStop == false)
            {
                Thread.Sleep(100);

                if(this.serialEncoder==null)
                {
                    serialEncoder = (SerialEncoder)SystemManager.Instance().DeviceBox.SerialDeviceHandler.Find(f => f.DeviceInfo.DeviceType == DynMvp.Device.Serial.ESerialDeviceType.SerialEncoder);
                    continue;
                }

                if(prevDateTime == DateTime.MinValue)
                {
                    prevDateTime = DateTime.Now;
                    prevPos = ExcuteAPCommand();
                    rwCutState = -1;
                    commStopWatch = new Stopwatch();
                    stillImageSettings = StillImageSettings.Instance();
                    continue;
                }

                if (stillImageSettings.AutoResetAlarm)
                    ErrorManager.Instance().ResetAllAlarm();

                // 속도 감지
                double spdPlsPerMs = serialEncoder.GetSpeedPlsPerMs();
                if (spdPlsPerMs < 0)
                    continue;

                umPerPls = stillImageSettings.EncoderResolution;
                curSpd = spdPlsPerMs * umPerPls / 1000 * 60;
                DateTime dateTime = DateTime.Now;
                lock (spdHistory)
                {
                    // 최근 1분간의 속도만 가지고 있음.
                    spdHistory.Add(dateTime, curSpd);
                    spdHistory = spdHistory.SkipWhile(f => (dateTime - f.Key).TotalSeconds > 60).ToDictionary(f => f.Key, f => f.Value);
                }

                // 최근 1초간의 속도를 추출. 속도 변화량 측정
                List<double> recentSpdHistory = spdHistory.SkipWhile(f => (dateTime - f.Key).TotalSeconds > 1).ToList().ConvertAll(f => f.Value);
                double spdDiff = 0;
                if (recentSpdHistory.Count > 1)
                {
                    // Average Filter
                    this.avgSpd = recentSpdHistory.Average();
                    spdDiff = Math.Min(recentSpdHistory.Max() - avgSpd, avgSpd - recentSpdHistory.Min());
                }

                if (startMode != StartMode.Auto)
                    // Auto가 아니면 현재 상태(검사/정지) 유지
                {
                    //this.isRunning = this.isStable = (startMode == StartMode.Force);
                    //continue;
                }
                else
                {
                    // Auto일 경우 상태 변화
                    
                    float minLineSpd = stillImageSettings.MinimumLineSpeed;
                    float maxLineSpdVar = stillImageSettings.SpeedStableVariation;
                        
                    this.isInspecting = avgSpd > minLineSpd;
                    this.isStable = spdDiff < maxLineSpdVar; //avgVelosity / 8;//Math.Min(average / 10, 30);
                    //Debug.WriteLine(string.Format("SPD: {0:0.00} {1}\tACC: {2:0.00} {3}", avgSpd, this.isRunning ? "O" : "X", spdDiff, this.isStable ? "O" : "X"));

                    if (ErrorManager.Instance().IsCleared())
                    {
                        if (SystemState.Instance().IsInspectOrWait && (isStable == false || isInspecting == false))
                            OnStopInspection();
                        else if (SystemState.Instance().IsInspectOrWait == false && isInspecting && isStable)
                            OnStartInspection();
                    }
                }

                MachineIfProtocolResponce rwCutResponce = SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfCommonCommand.GET_REWINDER_CUT);  // 리와인더 컷
                if (rwCutResponce != null && rwCutResponce.IsResponced)
                {
                    int machineValue = int.Parse(rwCutResponce.ReciveData);
                    //if (rwCutState >= 0 && rwCutState != machineValue)

                        //((InspectRunnerExtender)SystemManager.Instance().InspectRunner.InspectRunnerExtender).ChangeLotNo();
                    rwCutState = machineValue;
                }

                SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfStillImageCommand.SET_STILLIMAGE_RUN, "1");  // Alive 신호
                Thread.Sleep(100);
            }
        }

        private double Median(double[] filterBuffer)
        {
            List<double> tempBuffer = filterBuffer.ToList();
            tempBuffer.Sort();
            int halfIdx = tempBuffer.Count / 2;
            if ((tempBuffer.Count % 2) == 0)
                return (tempBuffer[halfIdx - 1] + tempBuffer[halfIdx]) / 2;
            else
                return tempBuffer[halfIdx];
        }

        private long ExcuteAPCommand()
        {
            lock (serialEncoder)
            {
                try
                {
                    string[] token = serialEncoder.ExcuteCommand(SerialEncoderV105.ECommand.AP)?.Split(',');
                    if (token == null || token.Length != 2)
                        return -1;

                    long pos = long.Parse(token[1]);
                    return pos;
                }
                catch (TimeoutException)
                {
                    return -1;
                }
            }
        }

        public bool StartTest(string target)
        {
            Action action = null;
            switch (target)
            {
                case "PlcReadWrite":
                    //action = new Action(PlcReadWriteTestProc);
                    break;
                case "EncoderStartStop":
                    action = new Action(EncoderStartStopTestProc);
                    break;
            }

            if (action == null)
                return false;

            if (this.testTesk == null)
            {
                while (this.WorkingThread.ThreadState != System.Threading.ThreadState.WaitSleepJoin)
                    Thread.Sleep(10);
                this.testTeskCancellationTokenSource = new CancellationTokenSource();
                this.testTesk = new Task(action, this.testTeskCancellationTokenSource.Token);
                testTesk.Start();
                return true;
            }
            return false;
        }

        public bool StopTest()
        {
            if (this.testTesk != null)
            {
                this.testTeskCancellationTokenSource.Cancel();
                this.testTesk.Wait();
                this.testTesk = null;
                return true;
            }
            return false;
        }

        private void WriteLog(string debugFile, DateTime curDateTime, long posDiff, double timeDiff, long commTime, double curVelosity, double filteredVelosity, double avgVelosity, double velDiff, bool isRunning, bool isStable)
        {
            if (System.IO.File.Exists(debugFile) == false)
                System.IO.File.AppendAllText(debugFile,
                    string.Format("Time, PosDiff, TimeDiff, ComTime, CurVel, FilterdVel, AvgVel, VelDiff, IsRunning, IsStable\r\n"));
            System.IO.File.AppendAllText(debugFile,
                string.Format("{0}, {1}, {2}, {3}, {4:F3}, {5:F3}, {6:F3}, {7:F3} ,{8}, {9}\r\n", curDateTime.ToString("yyyy-MM-dd HH:mm:ss"), posDiff, timeDiff, commTime, curVelosity, filteredVelosity, this.avgSpd, velDiff, isRunning ? 1 : 0, isStable ? 1 : 0));
        }

        public override void Dispose()
        {
            this.testTesk?.Dispose();
            this.testTesk = null;
        }

        public override string GetWorker()
        {
            return "Operator";
        }
        
        public override double GetLineSpeedSv()
        {
            return GetLineSpeedPv();
        }

        public override double GetLineSpeedPv()
        {
            if (serialEncoder == null)
                return 0;
            return serialEncoder.GetSpeedPlsPerMs();
        }

        public override string GetLotNo()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }

        public override string GetModelName()
        {
            return "Operator";
        }

        public override int GetPosition()
        {
            return 0;
        }

        public override string GetPaste()
        {
            throw new NotImplementedException();
        }
        public override double GetRollerDia()
        {
            throw new NotImplementedException();
        }

        public override int GetRewinderSite()
        {
            throw new NotImplementedException();
        }

        public override int GetUnwinderSite()
        {
            throw new NotImplementedException();
        }
    }
}