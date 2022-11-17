using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanM.Settings;
using UniEye.Base.MachineInterface;
using UniScanM.Pinhole.Operation;
using System.Drawing;
using UniEye.Base.Settings;
using UniEye.Base;
using UniScanM.Pinhole.Settings;
using UniScanM.Pinhole.Data;
using UniScanM.Data;
using DynMvp.UI;
//using UniEye.Base.Inspect;

namespace UniScanM.Pinhole
{
    public class SystemManager : UniScanM.SystemManager
    {
        public override UniEye.Base.Inspect.InspectRunner CreateInspectRunner()
        {
            return new InspectRunner();
        }
        
        public override UniEye.Base.Inspect.InspectRunnerExtender GetInspectRunnerExtender()
        {
            return new InspectRunnerExtender();
        }

        public override void InitializeDataExporter()
        {
            dataExporterList.Add(new UniScanM.Pinhole.Data.DataExporter());
            dataExporterList.Add(new UniScanM.Pinhole.MachineIF.MachineIfDataExporter());
        }

        protected override void LoadAdditialSettings()
        {
            PinholeSettings.CreateInstance();
            PinholeSettings.Instance().Load();

            UISettings.Instance().Load();

            //inspectStarter = new PLCInspectStarter();
        }
        
        public override void InitializeAdditionalUnits()
        {
            base.InitializeAdditionalUnits();
        }

        private void LotChanged(string lotNo)
        {

        }

        private void CopperInfoChanged(string copperInfo)
        {
            
        }

        public override void CreateInspectStarter()
        {
            this.inspectStarter = new PLCInspectStarter();
        }
    }
}
