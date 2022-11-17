using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.Vision;
using UniEye.Base.Data;

namespace UniEye.Base.Inspect
{
    public interface IProcesser
    {
        InspectState InspectState { get; }

        ProcessTask Process(ImageD imageD, InspectionResult inspectionResult, InspectOption inspectionOption = null);
        bool WaitProcessDone(ProcessTask inspectionTask, int timeoutMs = -1);
        void CancelProcess(ProcessTask inspectionTask=null);
        IProcesser GetNextProcesser();
    }
}
