using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanM.CEDMS.Operation;
using UniScanM.CEDMS.Settings;
//using UniEye.Base.Inspect;

namespace UniScanM.CEDMS
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
            dataExporterList.Add(new MachineIF.MachineIfDataExporter());
            dataExporterList.Add(new Data.ReportDataExporter());
        }

        protected override void LoadAdditialSettings()
        {
            CEDMSSettings.CreateInstance();
            CEDMSSettings.Instance().Load();
        }

        public override void CreateInspectStarter()
        {
            this.inspectStarter = new PLCInspectStarter();
        }

        public override void InitializeAdditionalUnits()
        {
            base.InitializeAdditionalUnits();
        }
    }
}
