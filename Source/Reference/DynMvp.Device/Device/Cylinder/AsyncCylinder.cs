using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using DynMvp.Base;
using DynMvp.Devices;

namespace DynMvp.Device.Device.Cylinder
{
    public class CylinderActionItem
    {
        bool inject;

        Cylinder cylinder;
        public Cylinder Cylinder
        {
            get { return cylinder; }
            set { cylinder = value; }
        }

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

        public CylinderActionItem(bool inject, Cylinder cylinder, IsActionDone isActionDone, IsActionError isActionError = null)
        {
            this.inject = inject;
            this.cylinder = cylinder;
            this.isActionDone = isActionDone;
            this.isActionError = isActionError;
        }

        public void Execute()
        {
            cylinder.ActAsync(inject);
        }
    }

    public class CylinderAction
    {
        List<CylinderActionItem> cylinderActionItemList = new List<CylinderActionItem>();

        public void AddAction(bool inject, Cylinder cylinder, IsActionDone isActionDone, IsActionError isActionError = null)
        {
            cylinderActionItemList.Add(new CylinderActionItem(inject, cylinder, isActionDone, isActionError));
        }

        public bool IsActionDone()
        {
            foreach (CylinderActionItem item in cylinderActionItemList)
            {
                if (item.IsActionDone() == false)
                    return false;
            }

            return false;
        }

        public bool IsActionError()
        {
            foreach (CylinderActionItem item in cylinderActionItemList)
            {
                if (item.IsActionError != null)
                {
                    if (item.IsActionError() == true)
                        return true;
                }
            }

            return false;
        }

        public bool Execute(int timeoutMilliSec = 0, int initialWaitTime = 0)
        {
            if (timeoutMilliSec == 0)
            {
                timeoutMilliSec = Cylinder.ActionDoneWaitS * 1000;
            }

            foreach(CylinderActionItem item in cylinderActionItemList)
            {
                item.Execute();
            }

            Thread.Sleep(100);

            int loopCount = timeoutMilliSec / 10;

            if (initialWaitTime > 0)
                Thread.Sleep(initialWaitTime);

            for (int i = 0; i < loopCount; i++)
            {
                if (IsActionDone())
                {
                    Thread.Sleep(Cylinder.AirActionStableTimeMs);
                    return true;
                }

                if (IsActionError())
                {
                    throw new ActionTimeoutException("Solenoid Action Error");
                }

                Thread.Sleep(10);
            }

            throw new ActionTimeoutException("Solenoid Action Timeout");
        }
    }
}
