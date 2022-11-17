using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using UniEye.Base.Inspect;
using UniScanG.Data;
using UniScanG.Data.Model;
//using UniScanG.Gravure.Data;

namespace UniScanG.Module.Monitor.Inspect
{
    class InspectRunnerExtender : UniEye.Base.Inspect.InspectRunnerExtender
    {
        protected override DynMvp.InspData.InspectionResult CreateInspectionResult()
        {
            return new InspectionResult();
        }

        protected override string GetInspectionNo()
        {
            Production production = SystemManager.Instance().ProductionManager.CurProduction as Production;
            if (production != null)
                lock(production)
                {
                    string inspectionNo = production.Total.ToString();
                    production.AddTotal();
                    return inspectionNo;
                }
                    
            return base.GetInspectionNo();
        }

        public override UniEye.Base.Inspect.InspectOption BuildInspectOption(ImageDevice imageDevice, IntPtr ptr)
        {
            return new InspectOption(imageDevice, ptr);
        }
    }
}
