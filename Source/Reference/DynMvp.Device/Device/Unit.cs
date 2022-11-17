using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.UI.Touch;
using DynMvp.Devices;

namespace DynMvp.Device.Device
{
    public enum OpMode
    {
        Manual, Auto, SemiAuto, Pass, DryRun
    }

    public enum RunModeStatus
    {
        Start, Stop, Run, CycleEnd, Error, Reset
    }

    public class OpState
    {
        OpMode opMode;
        public OpMode OpMode
        {
            get { return opMode; }
            set { opMode = value; }
        }

        RunModeStatus autoModeStatue;
        public RunModeStatus RunModeStatus
        {
            get { return autoModeStatue; }
            set { autoModeStatue = value; }
        }
    }

    public enum UnitError
    {
        MoveTo
    }

    public abstract class UnitManager
    {
        protected OpState opState = new OpState();
        public OpState OpState
        {
            get { return opState; }
            set { opState = value; }
        }

        protected List<Unit> unitList = new List<Unit>();
        public List<Unit> UnitList
        {
            get { return unitList; }
            set { unitList = value; }
        }


        string lastFileName;

        CancellationTokenSource runProcessCancellationTokenSource;

        public virtual void Start()
        {
            started = true;

            try
            {
                runProcessCancellationTokenSource = new CancellationTokenSource();
                ActionDoneChecker.StopDoneChecker = false;

                foreach (Unit unit in unitList)
                    unit.Start(runProcessCancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
            }
        }

        public void Stop()
        {
            started = false;

            if (runProcessCancellationTokenSource != null)
                runProcessCancellationTokenSource.Cancel();

            ActionDoneChecker.StopDoneChecker = true;

            foreach (Unit unit in unitList)
                unit.Stop();

            ActionDoneChecker.StopDoneChecker = false;
        }

        bool initDone = false;
        public bool InitDone
        {
            get { return initDone; }
            set { initDone = value; }
        }

        bool started = false;
        public bool Started
        {
            get { return started; }
            set { started = value; }
        }

        public void Init(bool initData, bool initMachine, CancellationToken cancellationToken)
        {
            initDone = false;
            SimpleLoading loading = new SimpleLoading("Initialize...");
            loading.Show();
            int maxInitOrder = unitList.Max(x => x.InitOrder);
            for (int i = 0; i <= maxInitOrder; i++)
            {
                foreach (Unit unit in unitList)
                    unit.Init(i, initData, initMachine, cancellationToken);

                TimeOutTimer timeOutTimer = new TimeOutTimer();
                timeOutTimer.Start(200000); // 15000 : 원래 지정되어 있던 타임아웃

                while (IsInitialized() == false)
                {
                    if (timeOutTimer.TimeOut)
                    {
                        initDone = true;
                        loading.Close();
                        ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Initialize.Timeout, ErrorLevel.Fatal,
                            "UnitManager", "Init Timout", null);

                        return;
                    }
                    cancellationToken.ThrowIfCancellationRequested();

                    Thread.Sleep(100);
                }
            }
            initDone = true;
            loading.Close();
        }

        bool IsInitialized()
        {
            foreach (Unit unit in unitList)
            {
                if (unit.IsInitalized() == false)
                    return false;
            }

            return true;
        }

        public bool IsStopped()
        {
            foreach (Unit unit in unitList)
            {
                if (unit.IsStopped == false)
                    return false;
            }

            return true;
        }

        public void Link()
        {
            foreach (Unit unit in unitList)
                unit.Link(this);
        }

        public virtual void LoadData(XmlElement element)
        {
            foreach (Unit unit in unitList)
                unit.LoadData(element);
        }

        public virtual void SaveData(XmlElement element)
        {
            foreach (Unit unit in unitList)
                unit.SaveData(element);
        }

        public void LoadData(string fileName)
        {
            if (File.Exists(fileName) == false)
                throw new Exception();

            XmlDocument stateDocument = new XmlDocument();
            stateDocument.Load(fileName);

            XmlElement stateElement = stateDocument.DocumentElement;

            LoadData(stateDocument.DocumentElement);

            lastFileName = fileName;
        }

        public void SaveData(string fileName = "")
        {
            if (fileName == "")
                fileName = lastFileName;

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement stateElement = xmlDocument.CreateElement("", "State", "");
            xmlDocument.AppendChild(stateElement);

            SaveData(stateElement);

            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "\t";
            xmlSettings.NewLineHandling = NewLineHandling.Entitize;
            xmlSettings.NewLineChars = "\r\n";

            XmlWriter xmlWriter = XmlWriter.Create(fileName, xmlSettings);

            xmlDocument.Save(xmlWriter);
            xmlWriter.Flush();
            xmlWriter.Close();

            lastFileName = fileName;
        }
    }

    public delegate void StateChangedDelegate(string unitName);

    public abstract class Unit
    {
        string name;
        public string Name
        {
            get { return name; }
            set { this.name = value; }
        }

        protected bool onInitializing = false;
        public bool OnInitializing
        {
            get { return onInitializing; }
        }

        protected CancellationToken initCancellationToken;
        protected CancellationToken runProcessCancellationToken;

        public StateChangedDelegate StateChanged;

        Task task;

        public readonly object ioLock = new object();

        protected OpState opState;
        protected string stateFileName;
        object stateFileLock = new object();

        protected DigitalIoHandler digitalIoHandler;

        protected int errorSection;
        protected string errorSectionName;

        private int initOrder;
        public int InitOrder
        {
            get { return initOrder; }
            set { initOrder = value; }
        }

        private bool enabled = true;
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        protected AxisHandler axisHandler;
        public AxisHandler AxisHandler
        {
            get { return axisHandler; }
            set { axisHandler = value; }
        }

        protected Task initMachineTask;

        protected bool isError = false;
        public bool IsError
        {
            get { return isError; }
            set { isError = value; }
        }

        protected bool isStopped = true;
        public bool IsStopped
        {
            get { return isStopped; }
            set { isStopped = value; }
        }

        public abstract string GetStepName();

        System.Windows.Forms.Timer stateUpdateTimer = null;

        public Unit()
        {
            stateUpdateTimer = new System.Windows.Forms.Timer();
            stateUpdateTimer.Interval = 100;
            stateUpdateTimer.Tick += new EventHandler(stateUpdateTimer_Tick);
            stateUpdateTimer.Start();
        }

        private void stateUpdateTimer_Tick(object sender, EventArgs e)
        {
            stateUpdateTimer.Stop();
            if (StateChanged != null)
            {
                StateChanged(this.name);

            }
            stateUpdateTimer.Start();
        }

        public void Start(CancellationToken runProcessCancellationToken)
        {
            if (task != null)
            {
                MessageForm.Show(null, "Task is not finished. Please, check the stop process.");
                return;
            }

            opState.RunModeStatus = RunModeStatus.Start;
            this.runProcessCancellationToken = runProcessCancellationToken;


            task = new Task(new Action(UnitProcess), runProcessCancellationToken);
            task.Start();

            isStopped = false;
        }

        public void Stop()
        {
            opState.RunModeStatus = RunModeStatus.Stop;

            try
            {
                task?.Wait();
            }
            catch (AggregateException)
            {
            }

            task = null;
        }

        public void Reset()
        {
            opState.RunModeStatus = RunModeStatus.Reset;
            task.Wait();
        }

        public void UnitProcess()
        {
            LogHelper.Debug(LoggerType.Machine, "[UnitProcess] Unit Process Start. Name = " + Name);

            try
            {
                bool stopFlag = false;

                while (stopFlag == false)
                {
                    switch (opState.OpMode)
                    {
                        case OpMode.Manual:
                            UnitManualProcess();
                            break;
                        case OpMode.Pass:
                        case OpMode.DryRun:
                        case OpMode.Auto:
                            switch (opState.RunModeStatus)
                            {
                                case RunModeStatus.Stop:
                                    StopRunProcess();
                                    runProcessCancellationToken.ThrowIfCancellationRequested();
                                    break;
                                case RunModeStatus.Start:
                                    StartRunProcess();
                                    opState.RunModeStatus = RunModeStatus.Run;
                                    break;
                                case RunModeStatus.Error:
                                    StopRunProcess();
                                    runProcessCancellationToken.ThrowIfCancellationRequested();
                                    break;
                                case RunModeStatus.Run:
                                    if (enabled == false || ErrorManager.Instance().IsError())
                                        break;

                                    runProcessCancellationToken.ThrowIfCancellationRequested();

                                    if (RunProcess() == false)
                                    {
                                        LogHelper.Debug(LoggerType.Machine, String.Format("[UnitProcess] Run Process Cancelled. Name = {0} , Step = {1}", Name, GetStepName()));

                                        //if (axisHandler.IsMovingTimeOut() == true)
                                        //{
                                        //    axisHandler.StopMove();
                                        //    ErrorReport(errorSection, (int)UnitError.MoveTo, ErrorLevel.Warning, Name, UnitError.MoveTo.ToString(), String.Format("Unit : {0} / Move to {1}", Name, axisHandler.LastAxisPosition.Name));
                                        //}

                                        opState.RunModeStatus = RunModeStatus.Error;
                                    }

                                    runProcessCancellationToken.ThrowIfCancellationRequested();

                                    if (IsStateChanged())
                                    {
                                        SaveLastState();

                                        if (StateChanged != null)
                                            StateChanged(this.name);
                                    }
                                    runProcessCancellationToken.ThrowIfCancellationRequested();

                                    break;
                                case RunModeStatus.Reset:
                                    ResetRunProcess();
                                    break;
                                default:
                                    break;
                            }
                            break;
                    }

                    runProcessCancellationToken.ThrowIfCancellationRequested();

                    Thread.Sleep(10);
                }
            }
            catch (OperationCanceledException)
            {
                StopRunProcess();
                LogHelper.Debug(LoggerType.Machine, "[UnitProcess] Unit Process Cancelled. Name = " + Name);

                task = null;
            }

            isStopped = true;
        }

        public abstract void GetPositionList(List<AxisPosition> positionList);
        public abstract void SetPositionList(List<AxisPosition> positionList);

        public void LoadLastState()
        {
            if (File.Exists(stateFileName) == false)
                return;

            try
            {
                XmlDocument stateDocument = new XmlDocument();
                stateDocument.Load(stateFileName);

                XmlElement stateElement = stateDocument.DocumentElement;

                LoadLastState(stateDocument.DocumentElement);
            }
            catch (XmlException)
            {
                File.Delete(stateFileName);
            }
        }

        public void SaveLastState()
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement stateElement = xmlDocument.CreateElement("", "State", "");
            xmlDocument.AppendChild(stateElement);

            SaveLastState(stateElement);

            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            xmlSettings.IndentChars = "\t";
            xmlSettings.NewLineHandling = NewLineHandling.Entitize;
            xmlSettings.NewLineChars = "\r\n";

            lock (stateFileLock)
            {
                XmlWriter xmlWriter = XmlWriter.Create(stateFileName, xmlSettings);

                xmlDocument.Save(xmlWriter);
                xmlWriter.Flush();
                xmlWriter.Close();
            }
        }

        protected void ErrorReport(ErrorCode errorCode, ErrorLevel errorLevel, string targetName, string message, object[] arguments, string solution = "")
        {
            ErrorManager.Instance().Report(errorCode, errorLevel, targetName, message, arguments, solution);
        }

        public abstract void Init(int orderNo, bool initData, bool initMachine, CancellationToken cancellationToken);

        public virtual bool IsInitalized()
        {
            if (initMachineTask != null)
            {
                if (initMachineTask.IsCompleted == false)
                    return false;

                initMachineTask = null;
            }

            return onInitializing == false;
        }

        public abstract void Link(UnitManager unitManager);

        public abstract bool IsStateChanged();
        public abstract void LoadLastState(XmlElement stateElement);
        public abstract void SaveLastState(XmlElement stateElement);
        public abstract void LoadData(XmlElement element);
        public abstract void SaveData(XmlElement element);

        public abstract void UnitManualProcess();
        public abstract void StartRunProcess();
        public abstract void StopRunProcess();
        public abstract void ResetRunProcess();
        public abstract bool RunProcess();
    }
}
