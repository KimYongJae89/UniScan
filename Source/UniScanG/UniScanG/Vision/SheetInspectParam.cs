using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanG.Data.Vision;
using UniScanG.Inspect;

namespace UniScanG.Vision
{
    public delegate void SheetInspectParamEventHandler(SheetInspectParam sheetInspectParam);
    public class SheetInspectParam : DynMvp.Vision.AlgorithmInspectParam
    {
        public event SheetInspectParamEventHandler OnDisposed;

        public UniScanG.Data.Model.Model Model => this.model;
        UniScanG.Data.Model.Model model = null;

        public AlgoImage AlgoImage => algoImage;
        private AlgoImage algoImage = null;

        public bool TestInspect { get => this.testInspect; set => this.testInspect = value; }
        private bool testInspect = false;

        public RegionInfo TargetRegionInfo { get => this.targetRegionInfo; set => this.targetRegionInfo = value; }
        private RegionInfo targetRegionInfo = null;

        public ProcessBufferSet ProcessBufferSet => processBufferSet;
        private ProcessBufferSet processBufferSet = null;

        public SheetInspectParam(UniScanG.Data.Model.Model model, AlgoImage algoImage, Calibration calibration, DebugContext debugContext) :
            base(null, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, calibration, debugContext)
        {
            this.model = model;
            this.algoImage = algoImage;
        }

        public SheetInspectParam(UniScanG.Data.Model.Model model, ProcessBufferSet processBufferSet, Calibration calibration, DebugContext debugContext) :
            base(null, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, calibration, debugContext)
        {
            this.model = model;
            this.processBufferSet = processBufferSet;
        }


        public override void Dispose()
        {
            base.Dispose();

            this.algoImage?.Dispose();

            this.OnDisposed?.Invoke(this);
        }
    }
}
