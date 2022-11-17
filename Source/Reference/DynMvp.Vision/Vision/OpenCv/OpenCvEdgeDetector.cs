using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using DynMvp.UI;
using DynMvp.Base;

namespace DynMvp.Vision.OpenCv
{
    public class OpenCvEdgeDetector : EdgeDetector
    {
        public override EdgeDetectionResult Detect(AlgoImage algoImage, RotatedRect rotatedRect, DebugContext debugContext)
        {
            OpenCvGreyImage greyImage = (OpenCvGreyImage)algoImage;

            EdgeDetectionResult result = new EdgeDetectionResult();
            result.Result = false;

            return result;
        }
    }
}
