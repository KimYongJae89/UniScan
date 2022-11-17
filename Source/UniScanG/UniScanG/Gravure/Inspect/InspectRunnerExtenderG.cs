using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.InspData;
using UniEye.Base.Inspect;

namespace UniScanG.Gravure.Inspect
{
    public class InspectRunnerExtenderG : InspectRunnerExtender
    {
        protected override InspectionResult CreateInspectionResult()
        {
            return new UniScanG.Data.Inspect.InspectionResult();
        }

    }
}
