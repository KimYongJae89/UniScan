using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.InspData;

namespace UniScanM.CEDMS.Operation
{
    public class InspectRunnerExtender : UniScanM.Operation.InspectRunnerExtender
    {
        protected override InspectionResult CreateInspectionResult()
        {
            return new CEDMS.Data.InspectionResult();
        }

        protected override string GetInspectionNo()
        {
            return (SystemManager.Instance().ProductionManager.CurProduction.Total + 1).ToString();
        }
    }
}
