using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.Operation
{
    public abstract class InspectRunner : UniEye.Base.Inspect.DirectTriggerInspectRunner
    {
        //public InspectStarter InspectStarter
        //{
        //    get { return inspectStarter; }
        //}

        //InspectStarter inspectStarter = null;

        public InspectRunner() : base()
        {
            SystemManager.Instance().CreateInspectStarter();
            //inspectStarter.EnterWaitInspection = EnterWaitInspection;
            //inspectStarter.ExitWaitInspection = ExitWaitInspection;
            //inspectStarter.Start();
        }

        protected Data.Production GetProduction(Data.InspectionResult insepctionResult)
        {
            return SystemManager.Instance().ProductionManager.GetProduction(insepctionResult);
        }

        protected Data.Production UpdateProduction(Data.InspectionResult insepctionResult)
        {
            UniScanM.Data.Production p = (UniScanM.Data.Production)SystemManager.Instance().ProductionManager.GetProduction(insepctionResult);
            if(p !=null)
            lock (p)
                p.Update(insepctionResult);
            return p;
        }
    }
}
