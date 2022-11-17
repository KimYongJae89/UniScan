using System;
using System.Collections.Generic;
using DynMvp.Vision;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Trainer;
using UniScanG.Gravure.Vision.Extender;

namespace UniScanG.Gravure
{
    public class AlgorithmArchiver : DynMvp.Vision.AlgorithmArchiver
    {
        public override List<Algorithm> GetDefaultAlgorithm()
        {
            return new List<DynMvp.Vision.Algorithm>() {
                CreateAlgorithm(SheetFinderBase.TypeName),
                CreateAlgorithm(TrainerBase.TypeName),
                CreateAlgorithm(CalculatorBase.TypeName),
                CreateAlgorithm(Detector.TypeName) ,
                CreateAlgorithm(Watcher.TypeName) };
        }

        public override Algorithm CreateAlgorithm(string algorithmType)
        {
            if (algorithmType == SheetFinderBase.TypeName)
                return GetSheetFinder(AlgorithmSetting.Instance().SheetFinderVersion);

            if (algorithmType == TrainerBase.TypeName)
                return GetTrainer(AlgorithmSetting.Instance().TrainerVersion); // TrainerBase.Create(AlgorithmSetting.Instance().TrainerVersion);

            if (algorithmType == CalculatorBase.TypeName)
                return GetCalculator(AlgorithmSetting.Instance().CalculatorVersion);

            if (algorithmType == Detector.TypeName)
                return GetDetector(AlgorithmSetting.Instance().DetectorVersion);

            if (algorithmType == Watcher.TypeName)
                return new Watcher();

            return base.CreateAlgorithm(algorithmType);
        }

        private Algorithm GetSheetFinder(ESheetFinderVersion version)
        {
            switch (version)
            {
                default:
                case ESheetFinderVersion.GRVE:
                    return new Vision.SheetFinder.SheetBase.SheetFinderV2();
                    break;
                case ESheetFinderVersion.RVOS:
                    return new Vision.SheetFinder.ReversOffset.SheetFinderRVOS();
                    break;
            }
        }

        private Algorithm GetCalculator(ECalculatorVersion version)
        {
            switch (version)
            {
                case ECalculatorVersion.V1:
                    return new Vision.Calculator.V1.CalculatorV1();
                default:
                case ECalculatorVersion.V2:
                    return new Vision.Calculator.V2.CalculatorV2();
                    break;
                case ECalculatorVersion.V3_RCI:
                    return new Vision.RCI.Calculator.CalculatorV3();
                    break;
            }
        }

        private Algorithm GetDetector(EDetectorVersion version)
        {
            switch (version)
            {
                default:
                case EDetectorVersion.V1:
                    return new Vision.Detector.Detector();
                case EDetectorVersion.V2:
                    return new Vision.Detector.V2.DetectorV2();
            }
        }

        private Algorithm GetTrainer(ETrainerVersion version)
        {
            switch (version)
            {
                default:
                case ETrainerVersion.V1:
                case ETrainerVersion.V2:
                    return new Vision.Trainer.Trainer(version);
                    break;
                case ETrainerVersion.RCI:
                    return new Vision.RCI.Trainer.RCITrainer();
                    break;

            }
        }
    }
}
