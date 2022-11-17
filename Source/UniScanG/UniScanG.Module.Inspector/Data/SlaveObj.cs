using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Module.Inspector.Data
{
    public enum CommState
    {
        CONNECTED, DISCONNECTED
    }

    public enum JobState
    {
        DONE, RUN, ERROR
    }

    public enum InspectState
    {
        IDLE, WAIT, INSPECT, PAUSE, ALARM
    }

    public class SlaveObj
    {
        private SlaveInfo info;
        public SlaveInfo Info
        {
            get { return info; }
        }

        CommState commState;
        public CommState CommState
        {
            get { return commState; }
            set { commState = value; }
        }

        JobState jobState;
        public JobState JobState
        {
            get { return jobState; }
            set { jobState = value; }
        }

        InspectState inspectState;
        public InspectState InspectState
        {
            get { return inspectState; }
            set { inspectState = value; }
        }

        public SlaveObj(SlaveInfo slaveInfo)
        {
            this.info = slaveInfo;

            commState = CommState.DISCONNECTED;
            jobState = JobState.DONE;
            inspectState = InspectState.IDLE;
        }
    }
}