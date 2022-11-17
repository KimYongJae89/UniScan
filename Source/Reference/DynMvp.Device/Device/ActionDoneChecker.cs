using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.Devices
{
    public class ActionTimeoutException : ApplicationException
    {
        public ActionTimeoutException(string msg)
            : base(msg)
        {
        }
    }

    public delegate bool IsActionError();
    public delegate bool IsActionDone(bool enableLog = true);

    public class ActionDoneChecker
    {
        private IsActionDone isActionDone;
        public IsActionDone IsActionDone
        {
            get { return isActionDone; }
            set { isActionDone = value; }
        }

        private IsActionError isActionError;
        public IsActionError IsActionError
        {
            get { return isActionError; }
            set { isActionError = value; }
        }

        static bool stopDoneChecker = false;
        public static bool StopDoneChecker
        {
            get { return stopDoneChecker; }
            set { stopDoneChecker = value; }
        }

        public bool WaitActionDone(int timeoutMilliSec, int initialWaitTime = 0)
        {
            if (timeoutMilliSec <= 0)
                timeoutMilliSec = 1000;

            TimeOutTimer timeOutTimer = new TimeOutTimer();
            timeOutTimer.Start(timeoutMilliSec);

            while (timeOutTimer.TimeOut == false)
            {
                if (IsActionDone())
                    return true;

                if (stopDoneChecker == true)
                    return true;

                Thread.Sleep(10);
            }

            return false;
        }
    }
}
