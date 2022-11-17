using System;
using System.Collections.Generic;
using UniScanS.Screen.Vision.Detector;
using UniScanS.Screen.Vision.FiducialFinder;
using UniScanS.Screen.Vision.Trainer;
using UniScanS.Vision.FiducialFinder;

namespace UniScanS.Screen
{
    public class AlgorithmArchiver : DynMvp.Vision.AlgorithmArchiver
    {
        public override List<DynMvp.Vision.Algorithm> GetDefaultAlgorithm()
        {
            return new List<DynMvp.Vision.Algorithm>() { CreateAlgorithm(SheetInspector.TypeName), CreateAlgorithm(FiducialFinder.TypeName) , CreateAlgorithm(SheetTrainer.TypeName) };
        }

        public override DynMvp.Vision.Algorithm CreateAlgorithm(string algorithmType)
        {
            if (algorithmType == SheetInspector.TypeName)
                return new SheetInspector();

            if (algorithmType == FiducialFinder.TypeName)
                return new FiducialFinderS();

            if (algorithmType == SheetTrainer.TypeName)
                return new SheetTrainer();

            return base.CreateAlgorithm(algorithmType);
        }
    }
}
