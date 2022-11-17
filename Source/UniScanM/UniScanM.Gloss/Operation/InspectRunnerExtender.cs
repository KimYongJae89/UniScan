using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.Gloss.Operation
{
    public class InspectRunnerExtender : UniScanM.Operation.InspectRunnerExtender
    {
        protected override InspectionResult CreateInspectionResult()
        {
            return new Gloss.Data.InspectionResult();
        }

        protected override string GetInspectionNo()
        {
            return (SystemManager.Instance().ProductionManager.CurProduction.Total + 1).ToString();
        }
    }
}
