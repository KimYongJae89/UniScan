using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanS.Inspect;

namespace UniScanS.Vision
{
    public class SheetInspectParam : DynMvp.Vision.AlgorithmInspectParam
    {
        public ProcessBufferSet ProcessBufferSet
        {
            get { return processBufferSet; }
            set { processBufferSet = value; }
        }

        bool fidResult;
        public bool FidResult
        {
            get { return fidResult; }
            set { fidResult = value; }
        }

        public SizeF FidOffset
        {
            get { return fidOffset; }
            set { fidOffset = value; }
        }

        private ProcessBufferSet processBufferSet;
        private SizeF fidOffset;

        
        public SheetInspectParam(ImageD clipImage, RotatedRect probeRegionInFov, RotatedRect clipRegionInFov, Size wholeImageSize, Calibration calibration, DebugContext debugContext) : base(clipImage, probeRegionInFov, clipRegionInFov, wholeImageSize, calibration, debugContext)
        {
        }
    }
}
