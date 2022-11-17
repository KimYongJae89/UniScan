using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.Device.Device;

namespace DynMvp.Devices.Dio
{
    public delegate bool IoMonitorEventHandler0();
    public delegate bool IoMonitorEventHandler1(DioValue value);
    public delegate bool IoMonitorEventHandler2(DioValue oldValue, DioValue newValue);

    public class IoMonitor : ThreadHandler
    {
        List<IoEventHandler> ioEventHandlerList;
        DioValue nowInputValue = new DioValue();
        DioValue preInputValue = new DioValue();
        DioValue nowOutputValue = new DioValue();

        bool preCleared = true;

        private DigitalIoHandler digitalIoHandler = null;

        public IoMonitorEventHandler1 ProcessInitial;
        public IoMonitorEventHandler1 ProcessIdle;
        public IoMonitorEventHandler2 ProcessInputChanged;
        public IoMonitorEventHandler2 ProcessOutputChanged;

        public IoMonitor(string name, DigitalIoHandler digitalIoHandler, List<IoEventHandler> ioEventHandlerList = null) : base(name)
        {
            this.digitalIoHandler = digitalIoHandler;
            this.ioEventHandlerList = ioEventHandlerList;
            this.ioEventHandlerList.ForEach(f => f.Update());
        }

        public IoMonitor(DigitalIoHandler digitalIoHandler, List<IoEventHandler> ioEventHandlerList = null) : base("IoMonitor")
        {
            this.digitalIoHandler = digitalIoHandler;
            this.ioEventHandlerList = ioEventHandlerList;
        }

        public override void Start()
        {
            this.requestStop = false;
            if (digitalIoHandler.Count > 0)
            {
                WorkingThread = new Thread(new ThreadStart(IOMonitorThreadFunc));
                WorkingThread.IsBackground = true;
                base.Start();
            }
        }

        public bool CheckOutput(IoPort ioPort)
        {
            return Check(nowOutputValue, ioPort);
        }

        public bool CheckInput(IoPort ioPort)
        {
            return Check(nowInputValue, ioPort);
        }

        public static bool Check(DioValue value, IoPort ioPort)
        {
            //if (ioPort == null)
            //    return false;

            if (ioPort.PortNo == IoPort.UNUSED_PORT_NO)
                return false;

            uint channelValue = value.GetValue(ioPort.DeviceNo, ioPort.GroupNo);

            return ((channelValue >> ioPort.PortNo) & 1) == (ioPort.ActiveLow ? 0 : 1);
        }

        private void IOMonitorThreadFunc()
        {
            bool initial = true;

            Stopwatch sw = new Stopwatch();
            while (RequestStop == false)
            {
                try
                {
                    nowOutputValue = digitalIoHandler.ReadOutput();
                    nowInputValue = digitalIoHandler.ReadInput();

                    if (initial == true)
                    {
                        if (ProcessInitial != null)
                            ProcessInitial(nowInputValue);
                        initial = false;
                    }
                    else
                    {
                        if (ProcessIdle != null)
                            ProcessIdle(nowInputValue);
                    }

                    if (ErrorManager.Instance().IsCleared() != preCleared)
                    // Alarm Occure or Clear
                    {
                        preCleared = ErrorManager.Instance().IsCleared();
                        Thread.Sleep(100);
                        continue;
                    }

                    if (nowInputValue.Equals(preInputValue) == false)
                    {
                        if (ProcessInputChanged != null)
                        {
                            ProcessInputChanged(preInputValue, nowInputValue);
                        }
                        preInputValue.Copy(nowInputValue);
                    }
                    Thread.Sleep(5);

                    if (ioEventHandlerList != null)
                    {
                        //LogHelper.Debug(LoggerType.IO, "Monitor EventList loop Start");
                        sw.Restart();
                        bool onChanged = false;
                        lock (ioEventHandlerList)
                        {
                            foreach (IoEventHandler ioEventHandler in ioEventHandlerList)
                            {
                                switch (ioEventHandler.Bound)
                                {
                                    case IoEventHandlerDirection.InBound:
                                        onChanged |= ioEventHandler.CheckState(nowInputValue);
                                        break;
                                    case IoEventHandlerDirection.OutBound:
                                        onChanged |= ioEventHandler.CheckState(nowOutputValue);
                                        break;
                                }
                            }
                        }
                        sw.Stop();
                        //if (onChanged)
                        //    LogHelper.Debug(LoggerType.IO, string.Format("Monitor EventList loop End. {0}[ms]", sw.ElapsedMilliseconds));
                    }
                }
                catch (System.ComponentModel.InvalidAsynchronousStateException exception) { LogHelper.Error(LoggerType.Error, "dd"); }
            }

            LogHelper.Debug(LoggerType.Shutdown, "Thread Stopped : IOMonitorThreadFunc");
        }
    }
}
