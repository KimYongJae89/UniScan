using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Module.Observer.Vision
{
    public class MarginCheckerResult
    {
        DateTime inspectStartTime;
        public DateTime InspectStartTime
        {
            get { return this.inspectStartTime; }
        }

        string title;
        public string Title
        {
            get { return this.title; }
        }

        RectangleF margin = RectangleF.Empty;
        public RectangleF Margin
        {
            get { return this.margin; }
            set { this.margin = value; }
        }


        RectangleF realMargin = RectangleF.Empty;
        public RectangleF RealMargin
        {
            get { return this.realMargin; }
            set { this.realMargin = value; }
        }

        public MarginCheckerResult(string title)
        {
            this.title = title;
            this.inspectStartTime = DateTime.Now;
        }
    }

    public class MarginChecker
    {
        public static MarginCheckerResult Inspect(string title, Bitmap bitmap, SizeF pelSize)
        {
            Image2D image2D = Image2D.FromBitmap(bitmap);
            AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.OpenCv, image2D, ImageType.Grey);

            List<BlobRect> blobRectList = GetBlobRectList(algoImage);
            PointF centerPt = DrawingHelper.CenterPoint(new Rectangle(Point.Empty, algoImage.Size));
            //algoImage.Save(@"d:\temp\tt.bmp");
            BlobRect centerBlob = blobRectList.FirstOrDefault(f => f.BoundingRect.Contains(centerPt));
            if (centerBlob == null)
            {
                return new MarginCheckerResult(title)
                {
                    Margin = RectangleF.FromLTRB(0, 0, 0, 0)
                };
            }
            Rectangle centerBlobRect = Rectangle.Round(centerBlob.BoundingRect);
            Point centerBlobCenter = Point.Round(centerBlob.CenterPt);

            // Find L Margin
            Rectangle[] rectangles = new Rectangle[] {
                Rectangle.FromLTRB(0, centerBlobRect.Top, centerBlobCenter.X, centerBlobRect.Bottom) ,
                Rectangle.FromLTRB(centerBlobRect.Left,0, centerBlobRect.Right, centerBlobCenter.Y),
                Rectangle.FromLTRB(centerBlobCenter.X, centerBlobRect.Top, algoImage.Width, centerBlobRect.Bottom),
                Rectangle.FromLTRB(centerBlobRect.Left, centerBlobCenter.Y, centerBlobRect.Right, algoImage.Height)
            };
            float[] margin = new float[4];

            for (int i = 0; i < 4; i++)
            {
                margin[i] = -1;
                Rectangle rectangle = rectangles[i];
                if (rectangle.Width > 0 && rectangle.Height > 0)
                {
                    AlgoImage subImage = algoImage.GetSubImage(rectangle);
                    List<Point> pointList = GetWhiteArea(subImage, i % 2 == 0 ? Direction.Horizontal : Direction.Vertical);
                    if (pointList.Count > 0)
                    {
                        Point point = i / 2 == 0 ? pointList.Last() : pointList.First();
                        margin[i] = point.Y - point.X;
                    }
                    subImage.Dispose();
                }
            }

            MarginCheckerResult marginCheckerResult = new MarginCheckerResult(title);
            marginCheckerResult.Margin = RectangleF.FromLTRB(margin[0], margin[1], margin[2], margin[3]);
            marginCheckerResult.RealMargin = RectangleF.FromLTRB(margin[0] * pelSize.Width, margin[1] * pelSize.Height, margin[2] * pelSize.Width, margin[3] * pelSize.Height);
            return marginCheckerResult;
        }

        private static List<Point> GetWhiteArea(AlgoImage algoImage, Direction direction)
        {
            algoImage.Save(@"d:\temp\GetWhiteArea.bmp");
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
            //float threshold = ip.GetGreyAverage(algoImage);
            StatResult statResult = ip.GetStatValue(algoImage);
            float threshold = (float)(statResult.max + statResult.min) / 2;
            float[] projData = ip.Projection(algoImage, direction, ProjectionType.Mean);

            List<Point> pointList = new List<Point>();
            int src = -1;
            for (int i = 0; i < projData.Length; i++)
            {
                float data = projData[i];
                if (data >= threshold)
                {
                    if (src < 0)
                        src = i;
                }
                else
                {
                    if (src >= 0)
                        pointList.Add(new Point(src, i - 1));
                    src = -1;
                }
            }
            pointList.RemoveAll(f => f.Y - f.X < 10);
            return pointList;
        }

        private static List<BlobRect> GetBlobRectList(AlgoImage algoImage)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
            AlgoImage clone = algoImage.Clone();
            ip.Binarize(clone, true);

            BlobParam blobParam = new BlobParam();
            blobParam.SelectArea = blobParam.SelectBoundingRect = true;
            BlobRectList blobRectList = ip.Blob(clone, blobParam);

            List<BlobRect> blobRects = blobRectList.GetList();
            blobRectList.Dispose();
            clone.Dispose();
            return blobRects;
        }
    }
}
