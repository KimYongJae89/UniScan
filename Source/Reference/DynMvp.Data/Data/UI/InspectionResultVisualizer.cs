using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Data.UI
{
    public interface InspectionResultVisualizer
    {
        void Update(Model model);
        void ResetResult();
        void UpdateResult(Target target, InspectionResult targetInspectionResult);
        void UpdateResult(TargetGroup targetGroup, InspectionResult targetGroupInspectionResult);
    }
}
