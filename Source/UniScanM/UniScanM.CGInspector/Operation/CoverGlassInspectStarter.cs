using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanM.Operation;
using UniScanM.CGInspector.MachineIF;
using DynMvp.Authentication;

namespace UniScanM.CGInspector.Operation
{
    class CoverGlassInspectStarter : UniScanM.Operation.InspectStarter
    {
        public CoverGlassInspectStarter() : base()
        {

        }

        public override string GetModelName()
        {
            return SystemManager.Instance().CurrentModel.Name;
        }

        public override string GetLotNo()
        {
            return DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss");
        }

        public override string GetPaste()
        {
            return "None";
        }

        public override string GetWorker()
        {
            return UserHandler.Instance().CurrentUser.Id;
        }

        protected override void ThreadProc()
        {
            
        }

        public override double GetLineSpeedSv()
        {
            return 0;
        }

        public override double GetLineSpeedPv()
        {
            return 0;
        }

        public override int GetRewinderSite()
        {
            return 0;
        }

        public override int GetPosition()
        {
            return 0;
        }

        public override double GetRollerDia()
        {
            return 0;
        }

        public override int GetUnwinderSite()
        {
            throw new NotImplementedException();
        }
    }
}
