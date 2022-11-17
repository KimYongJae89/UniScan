using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniScanM.Operation
{
    public delegate bool EventHandler();
    public delegate bool EnterWaitInspectionDelegate();
    public delegate void ExitWaitInspectionDelegate();
    public delegate void EventHandlerRewindCut();
    public delegate void OnStartModeChangedDelegate();
    public delegate void OnLotChangedDelegate();

    public abstract class InspectStarter : ThreadHandler, IDisposable
    {
        public EnterWaitInspectionDelegate OnStartInspection;
        
        //2. event stop
        public ExitWaitInspectionDelegate OnStopInspection;
        //3. event RewinderCut
        public EventHandlerRewindCut OnRewinderCutting;

        public OnStartModeChangedDelegate OnStartModeChanged;

        public OnLotChangedDelegate OnLotChanged;

        protected StartMode startMode = StartMode.Stop;
        public StartMode StartMode
        {
            get { return startMode; }
            set
            {
                startMode = value;
                if (startMode == StartMode.Stop)
                {
                    isInspecting = false;
                }
                OnStartModeChanged?.Invoke();
            }
        }

        protected bool isInspecting = false;
        
        //common
        protected InspectStarter() : base("InspectStarter")
        {
        }

        public virtual void Dispose()
        {
        }

        public virtual new void Start()
        {
            isInspecting = false;
            this.RequestStop = false;
            this.WorkingThread = new System.Threading.Thread(ThreadProc);
            this.WorkingThread.Start();
        }

        public void Stop()
        {
            this.RequestStop = true;
            WorkingThread?.Join();
            isInspecting = false;
        }

        public virtual bool PreStartInspect(bool autoStart)
        {
            return true;
        }

        protected abstract void ThreadProc();//********************************************************// ThreadProc()
            
        public abstract string GetWorker();
        public abstract double GetLineSpeedPv();
        public abstract double GetLineSpeedSv();
        public abstract string GetLotNo();
        public abstract string GetModelName();
        public abstract int GetUnwinderSite();
        public abstract int GetRewinderSite();
        public abstract int GetPosition();
        public abstract string GetPaste();
        public abstract double GetRollerDia();
    }
}