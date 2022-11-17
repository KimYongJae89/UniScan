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

namespace UniScanM.Pinhole.Operation
{
    public delegate void SpeedUpdateDelegate();
    /// <summary>
    /// Sance running and velosity stable to Encoder position
    /// </summary>
    public class InspectStarter2 : DynMvp.Base.ThreadHandler, IDisposable
    {
        CancellationTokenSource testTeskCancellationTokenSource = null;

        string debugFile = "";

        public EnterWaitInspectionDelegate EnterWaitInspection;
        public ExitWaitInspectionDelegate ExitWaitInspection;
        public SpeedUpdateDelegate SpeedUpdate;

        private StartMode startMode = StartMode.Stop;
        public StartMode StartMode
        {
            get { return startMode; }
            set { startMode = value; }
        }

        bool isStable = false;
        public bool IsStable
        {
            get { return isStable; }
        }

        bool isRunning = false;
        public bool IsRunning
        {
            get { return isRunning; }
        }



        public InspectStarter2() : base("InspectStarter")
        {

        }

        public new void Start()
        {
            
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
                        //serialEncoder.ExcuteCommand(SerialEncoderV105.ECommand.IN, "1");
                        processed = true;
                    }
                }
                else if (curDateTime.Minute % 10 == 9)
                {
                    if (processed == false)
                    {
                        //serialEncoder.ExcuteCommand(SerialEncoderV105.ECommand.IN, "0");
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

            return false;
        }

        public bool StopTest()
        {
            
            this.testTeskCancellationTokenSource.Cancel();
            
            return true;
        }

        public void Dispose()
        {
           
        }

        internal void StartAutoLight()
        {
            
        }
    }
}