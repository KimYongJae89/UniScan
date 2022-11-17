using DynMvp.Data;
using UniScanM.Gloss.Data;
using UniScanM.Gloss.MachineIF;
using UniScanM.Gloss.Operation;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss
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
            dataExporterList.Add(new ReportDataExporter());
            if (GlossSettings.Instance().UseModuleMode)
            {
                dataExporterList.Add(new DBDataExporter());
            }
        }

        protected override void LoadAdditialSettings()
        {
            GlossSettings.CreateInstance();
            GlossSettings.Instance().Load();
        }

        public override void CreateInspectStarter()
        {
            this.inspectStarter = new PLCInspectStarter();
        }

        public override void InitializeAdditionalUnits()
        {
            base.InitializeAdditionalUnits();
        }

        public override void LoadDefaultModel()
        {
            this.currentModel = ModelManager.CreateModel();
        }
    }
}
