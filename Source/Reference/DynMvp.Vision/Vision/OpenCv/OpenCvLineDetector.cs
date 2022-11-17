using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using DynMvp.UI;
using DynMvp.Base;
using DynMvp.Vision.OpenCv;

namespace DynMvp.Vision.OpenCv
{
    class OpenCvLineDetector : LineDetector
    {
        public override LineEq Detect(AlgoImage algoImage, PointF startPt, PointF endPt, DebugContext debugContext)
        {
            OpenCvGreyImage greyImage = (OpenCvGreyImage)algoImage;

            EdgeDetectionResult result = new EdgeDetectionResult();

            LineEq lineEq = new LineEq();

            return lineEq;
        }
    }
}
