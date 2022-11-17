using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;

namespace UniScanG.Gravure.Inspect
{
    public class EdgeFinderBuffer
    {
        public AlgoImage SobelBuffer { get ; private set; }
        public float[] Datas { get; private set; }
        public byte[] Bytes { get; private set; }

        public EdgeFinderBuffer()
        {
            Datas = null;
            Bytes = null;
            SobelBuffer = null;
        }

        public void Build(int length)
        {
            Datas = new float[length];
            Bytes = new byte[length * sizeof(float)];
            SobelBuffer = ImageBuilder.Build(CalculatorBase.TypeName, ImageType.Depth, length, 1);
        }

        public void Clear()
        {
            Array.Clear(Datas, 0, Datas.Length);
            Array.Clear(Bytes, 0, Bytes.Length);
            SobelBuffer?.Clear();
        }

        public void Dispose()
        {
            Datas = null;
            Bytes = null;
            SobelBuffer?.Dispose();
            SobelBuffer = null;
        }
    }

    public abstract class ProcessBufferSetG : UniScanG.Inspect.ProcessBufferSet
    {

        public Size PatternSizePx { get; set; }

        public bool IsMultiLayer { get => isMultiLayer; }
        protected bool isMultiLayer = false;

        public float ScaleFactor { get => scaleFactor; }
        protected float scaleFactor = 1;

        public Bitmap PrevBitmap => this.prevBitmap;
        protected Bitmap prevBitmap;

        public OffsetStructSet OffsetStructSet => offsetStructSet;
        protected OffsetStructSet offsetStructSet;

        public AlgoImage AlgoImage { get => algoImage; }
        protected AlgoImage algoImage = null;

        public AlgoImage ScaledImage { get => scaledImage; }
        protected AlgoImage scaledImage = null;

        public EdgeFinderBuffer EdgeFinderBuffer { get; protected set; }

        public AlgoImage CalculatorResultGray { get => calculatorResultGray; }
        protected AlgoImage calculatorResultGray = null;

        public AlgoImage CalculatorResultBinal { get => calculatorResultBinal; }
        protected AlgoImage calculatorResultBinal = null;

        public AlgoImage DetectorInsp { get => detectorInsp; }
        protected AlgoImage detectorInsp = null;

        public AlgoImage EdgeMapImage { get => edgeMapImage; }
        protected AlgoImage edgeMapImage = null;

        public AlgoImage MaskImage { get => maskImage; }
        protected AlgoImage maskImage = null;

        public AlgoImage FullAlgoImage { get => fullAlgoImage; }
        protected AlgoImage fullAlgoImage = null;

        public PartialProjection PartialProjection { get; private set; }

        public ProcessBufferSetG(float scaleFactor, bool isMultiLayer, int width, int height) : base("", width, height)
        {
            this.isMultiLayer = isMultiLayer;
            this.scaleFactor = scaleFactor;
            this.offsetStructSet = new OffsetStructSet(0);
            this.EdgeFinderBuffer = new EdgeFinderBuffer();
            this.PartialProjection = new PartialProjection();
        }

        public bool Upload(AlgoImage algoImage, DebugContext debugContext)
        {
            this.Clear();

            Size size;
            if (algoImage is SheetImageSet)
            {
                SheetImageSet sheetImageSet = (SheetImageSet)algoImage;
                size = sheetImageSet.GetFullImage(this.fullAlgoImage);
                PatternSizePx = sheetImageSet.PatternSizePx;
            }
            else
            {
                size = new Size(Math.Min(this.fullAlgoImage.Width, algoImage.Width), Math.Min(this.fullAlgoImage.Height, algoImage.Height));
                this.fullAlgoImage.Copy(algoImage, Point.Empty, Point.Empty, size);
                PatternSizePx = size;
            }

            if (size.Width * size.Height == 0)
                return false;

            try
            {
                this.algoImage = this.fullAlgoImage.GetSubImage(new Rectangle(Point.Empty, size));
                Upload(debugContext);

                //return this.offsetStructSet.LocalOffsets.All(f => f.IsGood);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, ex);
                return false;
            }
        }

        public abstract void Upload(DebugContext debugContext);
        public abstract void Download();

        public override void BuildBuffers(bool halfScale)
        {
            bufferList.Add(this.fullAlgoImage = ImageBuilder.Build(CalculatorBase.TypeName, width, height));
            EdgeFinderBuffer.Build(width);

            this.PartialProjection.Initialize(height);
        }

        public override void Dispose()
        {
            this.PartialProjection?.Dispose();

            this.algoImage?.Dispose();
            this.fullAlgoImage?.Dispose();
            this.EdgeFinderBuffer?.Dispose();

            base.Dispose();
        }

        public override void Clear()
        {
            this.algoImage?.Dispose();
            this.fullAlgoImage?.Clear();
            this.EdgeFinderBuffer?.Clear();

            this.prevBitmap?.Dispose();
            this.prevBitmap = null;

            this.PartialProjection.Clear();
            
            base.Clear();
        }

        public void UpdateLengthVariateion(AlgoImage algoImage, Rectangle rectangle)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
            using (AlgoImage subImage = algoImage.GetSubImage(rectangle))
            {
                //subImage.Save(@"C:\temp\subImage.bmp");
                float[] datas = this.PartialProjection.Datas;
                int entries = ip.Projection(subImage, ref datas, Direction.Vertical);
                this.PartialProjection.Set(datas, entries);
            }
        }
    }

}
