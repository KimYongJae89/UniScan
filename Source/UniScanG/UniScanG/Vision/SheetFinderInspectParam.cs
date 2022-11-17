using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Vision
{
    public class SheetFinderPrecisionBuffer : IDisposable
    {
        public int Length { get; private set; } = -1;
        public AlgoImage PrecisionBuffer { get; private set; } = null;
        public byte[] PrecisionBytes { get; private set; } = null;
        public float[] PrecisionSingles { get; private set; } = null;

        public SheetFinderPrecisionBuffer(int length)
        {
            this.Length = length;
            this.PrecisionSingles = new float[length];
            this.PrecisionBytes = new byte[length * sizeof(float)];
            this.PrecisionBuffer = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Depth, new Size(length, 1));
            Clear();
        }

        public void Dispose()
        {
            this.PrecisionBuffer?.Dispose();

            this.PrecisionBuffer = null;
            this.PrecisionBytes = null;
            this.PrecisionSingles = null;
            this.Length = -1;
        }

        public void Clear()
        {
            this.PrecisionBuffer?.Clear();
            if (this.PrecisionBytes != null)
                Array.Clear(this.PrecisionBytes, 0, this.PrecisionBytes.Length);
            if (this.PrecisionSingles != null)
                Array.Clear(this.PrecisionSingles, 0, this.PrecisionSingles.Length);
        }
    }

    public class SheetFinderInspectParam : DynMvp.Vision.AlgorithmInspectParam
    {
        public float[] ProjDatas { get; private set; }
        public float PatternFinderThreshold { get; set; }
        public bool SkipOutter { get; set; }

        public SheetFinderPrecisionBuffer PrecisionBuffer { get; private set; }

        public SheetFinderInspectParam(float[] projDatas, SheetFinderPrecisionBuffer precisionBuffer, Calibration calibration, DebugContext debugContext) :
            base(null, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, calibration, debugContext)
        {
            this.ProjDatas = projDatas;
            this.PrecisionBuffer = precisionBuffer;
        }
    }
}
