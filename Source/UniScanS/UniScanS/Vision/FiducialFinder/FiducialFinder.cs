using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanS.Screen.Vision.Detector;
using UniScanS.Vision;

namespace UniScanS.Vision.FiducialFinder
{
    public abstract class FiducialFinder : DynMvp.Vision.Algorithm
    {
        public static string TypeName
        {
            get { return "Sheet_Fid"; }
        }

        public FiducialFinder() { }


        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new FiducialFinderAlgorithmResult();
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
        }
        
        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return TypeName;
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            throw new System.NotImplementedException();
        }
    }
}
