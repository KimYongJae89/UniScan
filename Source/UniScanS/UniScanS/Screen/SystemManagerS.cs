// custom
using UniScanS.UI;
using UniScanS.Data;
using UniEye.Base.Settings.UI;
using UniScanS.Common.Settings;
using UniScanS.Common.Settings.UI;
using UniScanS.Common;
using UniEye.Base.Inspect;
using UniScanS.Screen.Inspect;
using UniEye.Base.Settings;
using DynMvp.Vision;          
using UniScanS.Vision.FiducialFinder;
using UniScanS.Screen.Vision.Detector;
using UniScanS.Screen.Vision.Trainer;
using UniScanS.Screen.Vision;
using UniScanS.Screen.Data;

namespace UniScanS.Screen
{
    public class MonitorSystemManagerS : SystemManager
    {
        public override void BuildAlgorithmStrategy()
        {
            //base.BuildAlgorithmStrategy();

            AlgorithmSetting.Instance().Load();
            MonitorSetting.Instance().Load();
        }

        public override void SelectAlgorithmStrategy()
        {
            base.SelectAlgorithmStrategy();
        }

        public override InspectRunner CreateInspectRunner()
        {
            return new InspectRunnerMonitorS();
        }

        public override void InitializeDataExporter()
        {
            DataSetting.Instance().Load();
            dataExporterList.Add(new UniScanS.Data.Inspect.MonitorDataExporterS());
        }
    }

    public class InspectorSystemManagerS : SystemManager
    {
        public override void BuildAlgorithmStrategy()
        {
            base.BuildAlgorithmStrategy();

            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(FiducialFinder.TypeName, ImagingLibrary.MatroxMIL, ""));
            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(SheetInspector.TypeName, ImagingLibrary.MatroxMIL, ""));
            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(SheetTrainer.TypeName, ImagingLibrary.MatroxMIL, ""));

            AlgorithmSetting.Instance().Load();
            InspectorSetting.Instance().Load();
        }

        public override void SelectAlgorithmStrategy()
        {
            base.SelectAlgorithmStrategy();

            AlgorithmBuilder.SetAlgorithmEnabled(FiducialFinder.TypeName, true);
            AlgorithmBuilder.SetAlgorithmEnabled(SheetInspector.TypeName, true);
            AlgorithmBuilder.SetAlgorithmEnabled(SheetTrainer.TypeName, true);
        }

        public override InspectRunner CreateInspectRunner()
        {
            return new InspectRunnerInspectorS();            
        }

        public override void InitializeDataExporter()
        {
            DataSetting.Instance().Load();

            dataExporterList.Add(new UniScanS.Data.Inspect.InspectorDataExporterS());
        }
    }
}
