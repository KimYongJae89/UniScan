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
using UniScanG.Gravure.Vision.Extender;

namespace UniScanG.Module.Inspector
{
    public class InspectorSystemManagerG : SystemManager
    {
        public override void BuildAlgorithmStrategy()
        {
            base.BuildAlgorithmStrategy();

            ImagingLibrary imagingLibrary = OperationSettings.Instance().ImagingLibrary;
            bool useCuda = OperationSettings.Instance().UseCuda;

            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(SheetFinderBase.TypeName, imagingLibrary, "", ImageType.Grey));
            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(TrainerBase.TypeName, imagingLibrary, "", ImageType.Grey));
            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(CalculatorBase.TypeName, imagingLibrary, "", useCuda ? ImageType.Gpu : ImageType.Grey));
            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(Detector.TypeName, imagingLibrary, "", ImageType.Grey));
            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(Watcher.TypeName, imagingLibrary, "", ImageType.Grey));
            //AlgorithmBuilder.AddStrategy(new AlgorithmStrategy("ImageSaver", imagingLibrary, "", ImageType.Grey));
        }

        public override void SelectAlgorithmStrategy()
        {
            base.SelectAlgorithmStrategy();

            AlgorithmBuilder.SetAlgorithmEnabled(SheetFinderBase.TypeName, true);
            AlgorithmBuilder.SetAlgorithmEnabled(TrainerBase.TypeName, true);
            AlgorithmBuilder.SetAlgorithmEnabled(CalculatorBase.TypeName, true);
            AlgorithmBuilder.SetAlgorithmEnabled(Detector.TypeName, true);
            AlgorithmBuilder.SetAlgorithmEnabled(Watcher.TypeName, true);

            AlgorithmBuilder.SetAlgorithmEnabled(PatternMatching.TypeName, true);
            //AlgorithmBuilder.SetAlgorithmEnabled("ImageSaver", true);
        }

        public override InspectRunner CreateInspectRunner()
        {
            return new Inspect.InspectRunnerInspectorG();
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
