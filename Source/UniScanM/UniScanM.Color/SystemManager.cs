using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Inspect;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanM.ColorSens.Operation;

namespace UniScanM.ColorSens
{
    public class SystemManager : UniScanM.SystemManager
    {
        public override UniEye.Base.Inspect.InspectRunner CreateInspectRunner()
        {
            return new UniScanM.ColorSens.Operation.InspectRunner();
        }

        public override UniEye.Base.Inspect.InspectRunnerExtender GetInspectRunnerExtender()
        {
            return new UniScanM.ColorSens.Operation.InspectRunnerExtender();
        }
        public override string[] GetSystemTypeNames()
        {
            return base.GetSystemTypeNames();
        }

        public override void InitializeDataExporter()
        {
           // dataExporterList.Add(new UniScanM.Data.InspectionResultDataExporter());
            dataExporterList.Add(new UniScanM.ColorSens.MachineIF.MachineIfDataExporter());
            dataExporterList.Add(new UniScanM.ColorSens.Data.ReportDataExporter());
        }

        protected override void LoadAdditialSettings()
        {
            //base.LoadAdditialSettings(); //UniScanM UI때문에 어쩔수 없이 로드.. 근데 컬러센서는 필요없음.
            UniScanM.ColorSens.Settings.ColorSensorSettings.CreateInstance();
            UniScanM.ColorSens.Settings.ColorSensorSettings.Instance().Load();

        }

        public override void CreateInspectStarter()
        {
            this.inspectStarter = new PLCInspectStarter();
        }
    }
}
