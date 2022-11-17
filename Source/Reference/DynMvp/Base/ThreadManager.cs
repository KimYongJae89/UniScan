using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DynMvp.Base
{
    public class ThreadHandler
    {
        [DllImport("kernel32")]
        static extern int GetCurrentThreadId();

        private string name;


        public DateTime ThreadStartTime { get; private set; }
        public DateTime WorkStartTime { get; private set; }
        public SortedList<DateTime,TimeSpan> WorkEndTimes { get; private set; }

        protected Thread workingThread;
        public Thread WorkingThread
        {
            get { return workingThread; }
            set { workingThread = value; }
        }

        protected bool requestStop;
        public bool RequestStop
        {
            get { return requestStop; }
            set { requestStop = value; }
        }

        public bool IsRunning
        {
            get { return workingThread.IsAlive; }
        }

        protected ThreadHandler(string name)
        {
            this.name = name;

            this.ThreadStartTime = DateTime.MinValue;
            this.WorkStartTime = DateTime.MaxValue;
            this.WorkEndTimes = new SortedList<DateTime, TimeSpan>();
        }

        public ThreadHandler(string name, Thread workingThread = null, bool requestStop = false) : this(name)
        {
            this.workingThread = workingThread;
            this.requestStop = requestStop;
        }

        public void WorkBegin()
        {
            this.WorkStartTime = DateTime.Now;
        }

        public void WorkEnd()
        {
            DateTime now = DateTime.Now;
            if (this.WorkStartTime <= now)
            {
                TimeSpan ts = now - this.WorkStartTime;
                lock (this.WorkEndTimes)
                {
                    // 키가 없으면 새로 넣는다.
                    // 키가 있으면 큰 TimeSpan으로 덮어쓴다.
                    if (!this.WorkEndTimes.ContainsKey(now))
                        this.WorkEndTimes.Add(now, ts);

                    TimeSpan ts2 = this.WorkEndTimes[now];
                    if (ts > ts2)
                        this.WorkEndTimes[now] = ts;

                    this.WorkStartTime = DateTime.MaxValue;
                }
            }
        }

        public float GetLoadFactor()
        {
            double seconds = 1;
            DateTime t0 = DateTime.Now;
            DateTime t1 = t0.AddSeconds(-seconds);

            TimeSpan timeSpan = new TimeSpan(0);
            if (this.WorkStartTime < t1)
                timeSpan = timeSpan.Add(t0 - t1);
            else if (this.WorkStartTime < t0)
                timeSpan = timeSpan.Add(t0 - this.WorkStartTime);


            KeyValuePair<DateTime, TimeSpan>[] pairs;
            lock (this.WorkEndTimes)
                pairs = this.WorkEndTimes.SkipWhile(f => f.Key < t1).ToArray();

            foreach (KeyValuePair<DateTime, TimeSpan> pair in pairs)
            {
                DateTime workBeginTime = pair.Key - pair.Value;
                if (workBeginTime < t1)
                    timeSpan = timeSpan.Add(pair.Key - t1);
                else
                    timeSpan = timeSpan.Add(pair.Value);
            }

            float factor = (float)(timeSpan.TotalSeconds / (t0 - t1).TotalSeconds);
            return factor;
        }

        protected void SetAffinity(int coreNo)
        {
            LogHelper.Debug(LoggerType.Function, string.Format("ThreadHandler({0})::SetAffinity", name));
            Process Proc = Process.GetCurrentProcess();
            //long AffinityMask = (long)Proc.ProcessorAffinity;
            //if (coreMask <= 0)
            //    coreMask = 0xFFFF;
            //Proc.ProcessorAffinity = (IntPtr)coreMask;

            int curThreadId = GetCurrentThreadId();
            foreach (ProcessThread th in Proc.Threads)
            {
                if (curThreadId == th.Id)
                {
                    if (coreNo == -1)
                        coreNo = 0xFFFF;
                    else
                        coreNo = 0x01 << (coreNo % Environment.ProcessorCount);
                    th.ProcessorAffinity = (IntPtr)coreNo;
                    break;
                }
            }
        }

        public virtual void Start()
        {
            LogHelper.Debug(LoggerType.Function, string.Format("ThreadHandler({0})::Start", name));
            this.ThreadStartTime = DateTime.Now;
            this.WorkStartTime = DateTime.MaxValue;
            this.WorkEndTimes.Clear();

            workingThread.Start();
            ThreadManager.AddThread(this);
        }

        public void AsyncStop()
        {
            LogHelper.Debug(LoggerType.Function, string.Format("ThreadHandler({0})::AsyncStop", name));
            if (workingThread == null)
                return;

            this.requestStop = true;
        }

        public bool IsStop()
        {
            LogHelper.Debug(LoggerType.Function, string.Format("ThreadHandler({0})::WaitStop", name));
            if (workingThread == null)
                return true;

            return (workingThread.IsAlive == false);
        }

        public virtual bool Stop(int timeoutMs = -1)
        {
            if (workingThread == null)
                return true;

            TimeOutTimer tt = new TimeOutTimer();
            this.requestStop = true;

            if (timeoutMs >= 0)
                tt.Start(timeoutMs);

            while (workingThread.IsAlive)
            {
                Application.DoEvents();
                Thread.Sleep(10);
                if (tt.TimeOut)
                    return false;
            }
            ThreadManager.RemoveThread(this);
            LogHelper.Debug(LoggerType.Function, string.Format("ThreadHandler({0})::Stop", name));
            return true;
        }

        public void Abort()
        {
            workingThread?.Abort();
        }
    }



    public class ThreadManager
    {
        private static List<ThreadHandler> threadHandlerList = new List<ThreadHandler>();

        public static void AddThread(ThreadHandler threadHandler)
        {
            if (threadHandlerList.Contains(threadHandler) == false)
                threadHandlerList.Add(threadHandler);
        }

        public static void RemoveThread(ThreadHandler threadHandler)
        {
            try
            {
                threadHandlerList.Remove(threadHandler);
            }
            catch (ArgumentOutOfRangeException)
            {

            }
        }

        public static void StopAllThread()
        {
            foreach (ThreadHandler threadHandler in threadHandlerList)
            {
                threadHandler.RequestStop = true;
            }
        }

        public static bool IsAllDead()
        {
            foreach (ThreadHandler threadHandler in threadHandlerList)
            {
                if (threadHandler.WorkingThread == null)
                    continue;

                if (threadHandler.WorkingThread.IsAlive)
                    return false;
            }

            return true;
        }

        public static bool WaitAllDead(int timeoutMs)
        {
            Thread.Sleep(10);

            for (int i = 0; i < timeoutMs / 10; i++)
            {
                if (IsAllDead())
                {
                    Debug.Write("Wait All Thread Dead\n");

                    return true;
                }

                Thread.Sleep(10);
            }

            return false;
        }
    }
}
