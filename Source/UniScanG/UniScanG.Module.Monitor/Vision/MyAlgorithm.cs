using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanG.Module.Monitor.Data;

namespace UniScanG.Module.Monitor.Vision
{
    public class Defect
    {
        public Rectangle Rectangle => this.rectangle;
        Rectangle rectangle;

        public SizeF SizeUm => this.sizeUm;
        SizeF sizeUm;

        public float Area => this.sizeUm.Width * this.sizeUm.Height;

        public Defect(Rectangle rectangle, SizeF sizeUm)
        {
            this.rectangle = rectangle;
            this.sizeUm = sizeUm;
        }
    }

    public class MyAlgorithmResult : AlgorithmResult
    {
        public SizeF MarginUm { get; set; }
        //SizeF marginUm = Size.Empty;

        public Rectangle MarginRect { get; set; }
        //Rectangle marginRect = Rectangle.Empty;


        public SizeF BlotUm { get; set; }
        //SizeF blotUm = Size.Empty;

        public Rectangle BlotRect { get; set; }
        //Rectangle blotRect = Rectangle.Empty;

        public List<Defect> Defects { get; set; }
        //Rectangle[] defectRects = new Rectangle[0];

        public MyAlgorithmResult(string algorithmName) : base(algorithmName)
        {
        }
    }

    public class MyAlgorithmInspectionParam : AlgorithmInspectParam
    {
        public MyAlgorithmInspectionParam(ImageD clipImage, Calibration calibration, DebugContext debugContext) : base(clipImage, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, calibration, debugContext)
        {
        }
    }

    public class MyAlgorithmParam : DynMvp.Vision.AlgorithmParam
    {
        public override AlgorithmParam Clone()
        {
            MyAlgorithmParam newParam = new MyAlgorithmParam();
            return newParam;
        }

        public override void Dispose()
        {
            
        }
    }

    public class MyAlgorithm : DynMvp.Vision.Algorithm
    {
        public static string TypeName => "MyAlgorithm";

        public MyAlgorithm() : base()
        {
            this.param = new MyAlgorithmParam();
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
            return TypeName;
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            throw new NotImplementedException();
        }

        public override AlgorithmInspectParam CreateAlgorithmInspectParam(ImageD clipImage, RotatedRect probeRegionInFov, RotatedRect clipRegionInFov, Size wholeImageSize, Calibration calibration, DebugContext debugContext)
        {
            return new MyAlgorithmInspectionParam(clipImage, calibration, debugContext);
        }

        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new MyAlgorithmResult(TypeName);
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            MyAlgorithmInspectionParam myAlgorithmInspectionParam = algorithmInspectParam as MyAlgorithmInspectionParam;
            SizeF pelSize = myAlgorithmInspectionParam.CameraCalibration.PelSize;
            
            MyAlgorithmResult myAlgorithmResult = CreateAlgorithmResult() as MyAlgorithmResult;
            DebugContext debugContext = algorithmInspectParam.DebugContext;
            //debugContext.SaveDebugImage = true;

            AlgoImage algoImage = ImageBuilder.Build(TypeName, algorithmInspectParam.ClipImage);
            algoImage.Save("algoImage.bmp", debugContext);

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
            AlgoImage binImage = ImageBuilder.Build(TypeName, algoImage.Size);
            ip.Binarize(algoImage, binImage, true);
            ip.Close(binImage, 2);
            //binImage.Save("binImage.bmp", debugContext);

            BlobParam blobParam = new BlobParam()
            {
                SelectBoundingRect = true,
                SelectCenterPt = true,
                EraseBorderBlobs = false,
                SelectCompactness = true
            };

            PointF imageCenter = new PointF(algoImage.Width / 2f, algoImage.Height / 2f);
            BlobRectList blobResult = ip.Blob(binImage, blobParam);
            List<BlobRect> blobRectList = blobResult.GetList();
            blobRectList = blobRectList.OrderBy(f => MathHelper.GetLength(imageCenter, f.CenterPt)).ToList();
            BlobRect blobRect = blobRectList[0];
            blobRectList.Remove(blobRect);

            InspectMargin(myAlgorithmResult, blobRect, blobRectList, algoImage.Size, pelSize);
            InspectBlot(myAlgorithmResult, binImage, blobRect, pelSize);
            InspectDefect(myAlgorithmResult, binImage, blobRect, blobRectList, pelSize);

            return myAlgorithmResult;
        }

        #region Defect
        private void InspectDefect(MyAlgorithmResult myAlgorithmResult, AlgoImage binImage, BlobRect blobRect, List<BlobRect> blobRectList, SizeF pelSize)
        {
            Rectangle imageRect = new Rectangle(Point.Empty, binImage.Size);
            imageRect.Inflate(-1, -1);
            blobRectList.RemoveAll(f => RectangleF.Intersect(f.BoundingRect, imageRect) != f.BoundingRect);

            //List<BlobRect> blobRectList2 = new List<BlobRect>(blobRectList);
            //blobRectList2.RemoveAll(f => RectangleF.Intersect(f.BoundingRect, imageRect) != f.BoundingRect);
            //blobRectList2.RemoveAll(f => f.Area >= blobRect.Area * 0.03);

            //blobRectList2.Sort((f, g) => f.Area.CompareTo(g.Area));

            //myAlgorithmResult.Defects = blobRectList2.ConvertAll(f => new Defect(Rectangle.Round(f.BoundingRect), new SizeF(f.BoundingRect.Width * pelSize.Width, f.BoundingRect.Height * pelSize.Height)));

            List<Defect> defectList = blobRectList.ConvertAll(f => new Defect(Rectangle.Round(f.BoundingRect), new SizeF(f.BoundingRect.Width * pelSize.Width, f.BoundingRect.Height * pelSize.Height)));
            List<Defect> orderDefectList = defectList.OrderBy(f => f.Area).ToList();

            List<List<Defect>> defectListList = new List<List<Defect>>();
            SizeF meanSize = SizeF.Empty;
            List<Defect> tempList = null;
            orderDefectList.ForEach(f =>
            {
                if (!meanSize.IsEmpty)
                {
                    float sizeWLimL = meanSize.Width * 0.9f;
                    float sizeWLimH = meanSize.Width * 1.1f;
                    float sizeLLimL = meanSize.Height * 0.9f;
                    float sizeLLimH = meanSize.Height * 1.1f;

                    if (sizeWLimL < f.SizeUm.Width && f.SizeUm.Width < sizeWLimH && sizeLLimL < f.SizeUm.Height && f.SizeUm.Height < sizeLLimH)
                    {
                        tempList.Add(f);
                        meanSize = new SizeF(tempList.Average(g => g.SizeUm.Width), tempList.Average(g => g.SizeUm.Height));
                        return;
                    }
                }

                tempList = new List<Defect>();
                defectListList.Add(tempList);
                tempList.Add(f);
                meanSize = f.SizeUm;
            });

            defectListList.ForEach(f =>
            {
                if (f.Count > 1)
                    f.ForEach(g => defectList.Remove(g));
            });

            myAlgorithmResult.Defects = new List<Defect>(defectList);

        }
        #endregion

        #region Blot
        private void InspectBlot(MyAlgorithmResult myAlgorithmResult, AlgoImage binImage, BlobRect blobRect, SizeF pelSize)
        {
            Rectangle blobRectangle = Rectangle.Round(RectangleF.Inflate(blobRect.BoundingRect, 1, 1));
            AlgoImage clipAlgoImage = binImage.Clip(blobRectangle);
            //clipAlgoImage.Save(@"d:\temp\clipAlgoImage.bmp");

            float w = InspectBlot(clipAlgoImage, Direction.Horizontal, clipAlgoImage.Width);
            float h = InspectBlot(clipAlgoImage, Direction.Vertical, clipAlgoImage.Height);
            
            myAlgorithmResult.BlotUm = new SizeF(w*pelSize.Width, h * pelSize.Height);
            myAlgorithmResult.BlotRect = Rectangle.Round(blobRect.BoundingRect);
        }

        private float InspectBlot(AlgoImage algoImage, Direction direction, int maxValue)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
            ip.Open(algoImage, 1);
            StatResult statResult = ip.GetStatValue(algoImage);
            float thres = (float)Math.Max(0, statResult.average - 2*statResult.stdDev);

            float[] proj = ip.Projection(algoImage, direction, ProjectionType.Mean);
            int src = Array.FindIndex(proj, f => f > thres);

            int dst = maxValue - Array.FindLastIndex(proj, f => f > thres);
            return Math.Max(src, dst);
        }
        #endregion

        #region Margin
        private void InspectMargin(MyAlgorithmResult myAlgorithmResult, BlobRect blobRect, List<BlobRect> blobRectList, Size imageSize, SizeF pelSize)
        {
            float w = InspectMargin(Direction.Horizontal, blobRect, blobRectList, imageSize);
            float h = InspectMargin(Direction.Vertical, blobRect, blobRectList, imageSize);

            myAlgorithmResult.MarginUm = new SizeF(w * pelSize.Width, h * pelSize.Height);
            myAlgorithmResult.MarginRect = Rectangle.Round(RectangleF.Inflate(blobRect.BoundingRect, w, h));
        }
        private float InspectMargin(Direction direction, BlobRect blobRect, List<BlobRect> blobRectList, Size imageSize)
        {
            RectangleF searchRect = RectangleF.Empty;
            Comparison<BlobRect> comparison = null;
            Converter<BlobRect, float> converter = null;
            Predicate<BlobRect> predicate = new Predicate<BlobRect>(f => f.BoundingRect.IntersectsWith(searchRect));
            if(direction == Direction.Horizontal)
            {
                searchRect = RectangleF.FromLTRB(0, blobRect.BoundingRect.Top, imageSize.Width, blobRect.BoundingRect.Bottom);
                comparison = new Comparison<BlobRect>((f, g) => f.CenterPt.X.CompareTo(g.CenterPt.X));
                converter = new Converter<BlobRect, float>(f => Math.Max(f.BoundingRect.Left - blobRect.BoundingRect.Right, blobRect.BoundingRect.Left - f.BoundingRect.Right));
            }
            else
            {
                searchRect = RectangleF.FromLTRB(blobRect.BoundingRect.Left, 0, blobRect.BoundingRect.Right, imageSize.Height);
                comparison = new Comparison<BlobRect>((f, g) => f.CenterPt.Y.CompareTo(g.CenterPt.Y));
                converter = new Converter<BlobRect, float>(f => Math.Max(f.BoundingRect.Top - blobRect.BoundingRect.Bottom, blobRect.BoundingRect.Top - f.BoundingRect.Bottom));
            }

            List<BlobRect> candidateBlobRectList = blobRectList.FindAll(predicate);
            candidateBlobRectList.Sort(comparison);
            List<float> marginList = candidateBlobRectList.ConvertAll<float>(converter);
            marginList.RemoveAll(f => f <= 0);
            float minValue = marginList.Count == 0 ? 0 : marginList.Min();

            return minValue;
        }
        #endregion
    }
}
