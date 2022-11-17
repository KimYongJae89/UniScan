using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.UI;
using DynMvp.Vision;

namespace UniScanG.Gravure.Algorithm
{
    public class DefectDetectorParam : DynMvp.Vision.AlgorithmParam
    {
        public override AlgorithmParam Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class DefectDetectorResult : DynMvp.Vision.AlgorithmResult
    {

    }

    public class DefectDetector : DynMvp.Vision.Algorithm
    {
        public static string TypeName
        {
            get { return "DefectDetector"; }
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            throw new NotImplementedException();
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            throw new NotImplementedException();
        }

        public override DynMvp.Vision.Algorithm Clone()
        {
            throw new NotImplementedException();
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Gravure" + TypeName;
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            throw new NotImplementedException();
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            throw new NotImplementedException();
        }
    }
}
