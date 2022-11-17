using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Inspect;
using UniEye.Base.Settings;
using UniScanM.RVMS.Operation;
using UniScanM.RVMS.Settings;

namespace UniScanM.RVMS
{
    public class SystemManager : UniScanM.SystemManager
    {
        public override UniEye.Base.Inspect.InspectRunner CreateInspectRunner()
        {
            return new RVMS.Operation.InspectRunner();
        }

        public override UniEye.Base.Inspect.InspectRunnerExtender GetInspectRunnerExtender()
        {
            return new RVMS.Operation.InspectRunnerExtender();
        }

        public override void InitializeDataExporter()
        {
            dataExporterList.Add(new RVMS.Data.ReportDataExporter());
            dataExporterList.Add(new RVMS.MachineIF.MachineIfDataExporter());
            //dataExporterList.Add(new UniScanM.Pinhole.MachineIF.MachineIfDataExporter());
        }

        protected override void LoadAdditialSettings()
        {
            Settings.RVMSSettings.CreateInstance();
            Settings.RVMSSettings.Instance().Load();
        }

        public override void CreateInspectStarter()
        {
            this.inspectStarter = new PLCInspectStarter();
        }
    }
}
