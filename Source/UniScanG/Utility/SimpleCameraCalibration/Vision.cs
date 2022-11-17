using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCameraCalibration
{
    public class Vision
    {
        public static bool UseManualThreshold { get; set; }

        public static byte ManualThresholdValue { get; set; }

        public static double GetFocus(AlgoImage algoImage, AlgoImage buffer)
        {
            ImageProcessing ip = ImageProcessingFactory.CreateImageProcessing(algoImage.LibraryType);
            ip.Sobel(algoImage, buffer, Direction.Horizontal);
            StatResult statResult = ip.GetStatValue(buffer);
            return statResult.stdDev;
        }

        public static Tuple<double,double> GetSlope(AlgoImage algoImage, AlgoImage buffer, out List<PointF> pointList, out byte binValue)
        {
            ImageProcessing ip = ImageProcessingFactory.CreateImageProcessing(algoImage.LibraryType);

            if (UseManualThreshold)
            {
                binValue = ManualThresholdValue;
                ip.Binarize(algoImage, buffer, ManualThresholdValue, true);
            }
            else
            {
                binValue = (byte)ip.GetBinarizeValue(algoImage);
                ip.Binarize(algoImage, buffer, binValue, true);
            }
            ip.Open(buffer, buffer, 2);
            //buffer.Save(@"C:\temp\buffer.bmp");
            Rectangle blobRect = Rectangle.Inflate(new Rectangle(Point.Empty, buffer.Size), -buffer.Width / 8, 0);
            using (AlgoImage blobImage = buffer.GetSubImage(blobRect))
            {
                BlobParam blobParam = new BlobParam() { EraseBorderBlobs = false, SelectBoundingRect = true, AreaMin = 30 };
                using (BlobRectList blobRectList = ip.Blob(blobImage, blobParam))
                {
                    BlobRect[] blobRects = blobRectList.GetArray();
                    pointList = blobRects.Select(f =>
                    {
                        float x = (f.BoundingRect.Left + f.BoundingRect.Right) / 2;
                        float y = f.BoundingRect.Top;
                        return new PointF(blobRect.Left + x, blobRect.Top + y);
                    }).ToList();
                    pointList.RemoveAll(f => f.Y < 5);
                }
            }

            ip.Not(buffer);

            if (pointList.Count < 4)
                return new Tuple<double, double>(double.NaN, double.NaN);

            MathHelper.Regression1(pointList, out double a, out double b);
            Tuple<PointF, float>[] errors = pointList.Select(f => new Tuple<PointF, float>(f, (float)(Math.Abs(a * f.X - f.Y + b) / Math.Sqrt(Math.Pow(a, 2) + 1)))).ToArray();
            MathHelper.StdDev(errors.Select(f => f.Item2), out double mean, out double std);
            double t = std * 2;
            Tuple<PointF, float>[] inlier = Array.FindAll(errors, f => f.Item2 < std);

            MathHelper.Regression1(inlier.Select(f=>f.Item1), out a, out b);
            return new Tuple<double, double>(a, b);
        }
    }
}
