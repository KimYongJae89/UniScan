using DynMvp.Base;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device
{
    public delegate bool IoEvent(IoEventHandler eventSource);

    public enum IoEventHandlerDirection { InBound, OutBound }
    public class IoEventHandler
    {

        public string Name { get => this.name; }
        protected string name;

        protected DigitalIoHandler digitalIoHandler;
        public IoPort IoPort { get => this.ioPort; }
        protected IoPort ioPort;

        public IoEventHandlerDirection Bound { get => this.bound; }
        IoEventHandlerDirection bound;

        public bool IsActivate { get => ioPort.ActiveLow? !curState: curState; }
        protected bool curState = false;

        Stopwatch stopwatch = null;

        public IoEvent OnActivate;
        public IoEvent OnDeactivate;
        public IoEvent OnChanged;

        public IoEventHandler(string name, DigitalIoHandler digitalIoHandler, IoPort ioPort, IoEventHandlerDirection bound, bool saveLog = true)
        {
            this.name = name;
            this.digitalIoHandler = digitalIoHandler;
            this.ioPort = ioPort;
            this.bound = bound;
            if(saveLog)
                stopwatch = new Stopwatch();
        }

        public IoEventHandler(DigitalIoHandler digitalIoHandler, IoPort ioPort, IoEventHandlerDirection bound, bool saveLog = true)
        {
            this.name = ioPort.Name;
            this.digitalIoHandler = digitalIoHandler;
            this.ioPort = ioPort;
            this.bound = bound;
            if (saveLog)
                stopwatch = new Stopwatch();
        }

        public override string ToString()
        {
            return this.name;
        }

        public bool CheckState()
        {
            return CheckState(GetValue());
        }

        private DioValue GetValue()
        {
            DioValue inputValue;
            if (bound == IoEventHandlerDirection.InBound)
                inputValue = digitalIoHandler.ReadInput();
            else
                inputValue = digitalIoHandler.ReadOutput();

            return inputValue;
        }

        public bool GetCurrentValue()
        {
            DioValue inputValue;
            inputValue = digitalIoHandler.ReadInput();
            bool buttonState = IoMonitor.Check(inputValue, ioPort);
            return buttonState;
        }

        public bool CheckState(DioValue inputValue)
        {
            bool newState = IoMonitor.Check(inputValue, ioPort);

            if (newState != curState)
            {
                bool processOk1 = true;
                bool processOk2 = true;

                if (stopwatch != null)
                {
                    if (stopwatch.IsRunning)
                        LogHelper.Info(LoggerType.IO, String.Format("IoEventHandler::CheckState -  {0} is Changed. {1} -> {2}. ({3}[ms])",
                            name, curState ? "On" : "Off", newState ? "On" : "Off", stopwatch.ElapsedMilliseconds));
                    else
                        LogHelper.Info(LoggerType.IO, String.Format("IoEventHandler::CheckState -  {0} is Changed. {1} -> {2}",
                            name, curState ? "On" : "Off", newState ? "On" : "Off"));
                    stopwatch.Restart();
                }
                curState = newState;

                if (OnChanged != null)
                {
                    processOk1 = OnChanged(this);
                }

                if (newState)
                {
                    if (OnActivate != null)
                        processOk2 = OnActivate(this);
                }
                else
                {
                    if (OnDeactivate != null)
                    {
                        processOk2 = OnDeactivate(this);
                    }
                }
                bool processOk = processOk1 && processOk2;
                return processOk;
            }
            return false;
        }

        public void Update()
        {
            DioValue value = GetValue();
            if (value.Count > 0)
            {
                this.curState = IoMonitor.Check(value, ioPort);
                if (ioPort.PortNo != IoPort.UNUSED_PORT_NO)
                {
                    OnChanged?.Invoke(this);
                    if (this.curState)
                        OnActivate?.Invoke(this);
                    else
                        OnDeactivate?.Invoke(this);
                }
            }
        }
    }

    public class IoButtonEventHandler : IoEventHandler
    {
        IoPort lampOutPort;
        //public IoButtonHandler ButtonPushed;
        //public IoButtonHandler ButtonPulled;

        public IoButtonEventHandler(string name, DigitalIoHandler digitalIoHandler, IoPort buttonInPort, IoPort lampOutPort)
            :base(name, digitalIoHandler, buttonInPort, IoEventHandlerDirection.InBound)
        {
            this.lampOutPort = lampOutPort;
        }

        public void TurnOn()
        {
            if (lampOutPort != null)
                digitalIoHandler.WriteOutput(lampOutPort, true);
        }

        public void TurnOff()
        {
            if (lampOutPort != null)
                digitalIoHandler.WriteOutput(lampOutPort, false);
        }

        public void ResetState()
        {
            curState = false;
        }

        public new bool CheckState(DioValue inputValue)
        {
            bool buttonState = IoMonitor.Check(inputValue, ioPort);
            if (buttonState)
            {
                if (curState == false)
                {
                    curState = true;
                    LogHelper.Debug(LoggerType.IO, String.Format("{0} Button Pushed", name));
                    if (OnActivate != null)
                        OnActivate(this);
                }
            }
            else
            {
                if (curState == true)
                {
                    if (OnDeactivate != null)
                    {
                        LogHelper.Debug(LoggerType.IO, String.Format("{0} Button Pulled", name));
                        OnDeactivate(this);
                    }
                }

                curState = false;
            }

            return curState;
        }
    }
}
