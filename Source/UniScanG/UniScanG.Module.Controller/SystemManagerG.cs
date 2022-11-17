// custom
using UniScanG.UI;
using UniScanG.Data;
using UniEye.Base.Settings.UI;
using UniScanG.Common.Settings;
using UniScanG.Common.Settings.UI;
using UniScanG.Common;
using UniEye.Base.Inspect;
using UniEye.Base.Settings;
using DynMvp.Vision;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Vision.Trainer;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniEye.Base.UI;
using UniScanG.Module.Controller.MachineIF;
using UniScanG.Module.Controller.Inspect;
using UniEye.Base.MachineInterface;

namespace UniScanG.Module.Controller
{
    public class MonitorSystemManagerG : SystemManager
    {
        public MonitorSystemManagerG() : base()
        {
        }

        public override void BuildAlgorithmStrategy()
        {
            base.BuildAlgorithmStrategy();
        }

        public override void SelectAlgorithmStrategy()
        {
            base.SelectAlgorithmStrategy();
        }

        public override InspectRunner CreateInspectRunner()
        {
            return new InspectRunnerMonitorG();
        }

        public override InspectRunnerExtender GetInspectRunnerExtender()
        {
            return new InspectRunnerExtenderG();
        }

        public override void InitializeDataExporter()
        {
            dataExporterList.Add(new UniScanG.Data.Inspect.DataExporterG());        
        }

        protected override void LoadAdditialSettings()
        {
            // Program.cs에서 함..

            //UniScanG.Gravure.Settings.AdditionalSettings.CreateInstance();
            //AdditionalSettings.Instance().Load();
        }
    }
}
