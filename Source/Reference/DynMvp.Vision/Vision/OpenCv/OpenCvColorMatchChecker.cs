using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

using DynMvp.Base;
using DynMvp.UI;

namespace DynMvp.Vision.OpenCv
{
    public class OpenCvColorMatchChecker : ColorMatchChecker
    {
        public OpenCvColorMatchChecker()
        {

        }

        public override Algorithm Clone()
        {
            OpenCvColorMatchChecker openCvColorMatchChecker = new OpenCvColorMatchChecker();
            openCvColorMatchChecker.CopyFrom(this);

            return openCvColorMatchChecker;
        }

        public override bool Trained
        {
            get { return false; }
        }

        public override void PrepareInspection()
        {
             Train();
        }

        public override void Train()
        {
            
        }

        public override ColorMatchCheckerResult Match(AlgoImage algoImage, RectangleF probeRegion, DebugContext debugContext)
        {
            ColorMatchCheckerResult result = new ColorMatchCheckerResult();

            return result;
        }
    }
}
