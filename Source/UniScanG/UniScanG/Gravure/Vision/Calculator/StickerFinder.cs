using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.SheetFinder;

namespace UniScanG.Gravure.Vision.Calculator
{
    internal static class StickerFinder
    {
        public static DefectObj[] FindSticker(AlgoImage algoImage, Rectangle findRect, Calibration calibration, DebugContextG debugContext)
        {
            List<Data.DefectObj> sheetSubResultList = new List<UniScanG.Gravure.Data.DefectObj>();
            if (false)
            //test
            {
                Rectangle rectangle = new Rectangle(0, 2057, 566, 1362);
                Rectangle rectangleF = DrawingHelper.Mul(rectangle, 14f);
                Data.DefectObj sheetSubResult = new Data.DefectObj();
                sheetSubResult.PositionType = DefectObj.EPositionType.Sheet;
                sheetSubResult.Region = rectangle;
                sheetSubResult.RealRegion = rectangleF;

                Rectangle clipRect = Rectangle.Intersect(new Rectangle(Point.Empty, algoImage.Size),
                    Rectangle.Inflate(rectangle, 100, 100));
                AlgoImage subAlgoImag2e = algoImage.GetSubImage(clipRect);
                sheetSubResult.Image = subAlgoImag2e.ToBitmap();
                subAlgoImag2e.Dispose();

                sheetSubResultList.Add(sheetSubResult);
                return sheetSubResultList.ToArray();
            }

            bool stickerBrightOnly = CalculatorBase.CalculatorParam.StickerBrightOnly;
            //stickerBrightOnly = false;
            float hysteresisHigh = CalculatorBase.CalculatorParam.StickerDiffHigh;
            float hysteresisLow = CalculatorBase.CalculatorParam.StickerDiffLow;

            SheetFinderBaseParam sheerFinderParam = SheetFinderBase.SheetFinderBaseParam;
            Rectangle fullRect = new Rectangle(Point.Empty, algoImage.Size);
            if (calibration != null)
            {
                int width = (int)Math.Round(calibration.WorldToPixel(sheerFinderParam.SearchSkipWidthMm * 1000));
                fullRect.Inflate(-width, 0);
            }
            if (Rectangle.Intersect(fullRect, findRect) != findRect)
                return sheetSubResultList.ToArray();

            if (findRect.Width <= 0 || findRect.Height <= 0)
                return sheetSubResultList.ToArray();

            float[] proj;
            using (AlgoImage subAlgoImage = algoImage.GetSubImage(findRect))
            {
                subAlgoImage.Save(@"subAlgoImage.bmp", debugContext);
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
                proj = ip.Projection(subAlgoImage, Direction.Vertical, ProjectionType.Mean);
            }
            //StringBuilder sb = new StringBuilder();
            //Array.ForEach(proj, f => sb.AppendLine(f.ToString()));
            //File.WriteAllText(@"D:\temp\GetCoatingBrightness\Projection.txt", sb.ToString());

            float x_m = proj.Length / 2.0f;
            float y_m = proj.Average();
            double nomi = 0, denomi = 0;
            for (int i = 0; i < proj.Length; i++)
            {
                nomi += (proj[i] - y_m) * (i - x_m);
                denomi += Math.Pow((i - x_m), 2);
            }

            // y=ax+b
            double a = nomi / denomi;
            double b = y_m - (a * x_m);

            Func<int, float, float> Func;
            //stickerBrightOnly = false;
            if (stickerBrightOnly)
                Func = new Func<int, float, float>((i, d) => (float)(d - (a * i + b)));
            else
                Func = new Func<int, float, float>((i, d) => (float)Math.Abs(d - (a * i + b)));

            int srcPos = -1;
            List<Point> position = new List<Point>();
            float[] calibrated = new float[proj.Length];
            int[] founded = new int[proj.Length];
            for (int i = 0; i < proj.Length; i++)
            {
                calibrated[i] = Func(i, proj[i]);
                //calibrated[i] = (float)(proj[i] - (a * i + b));
                if (srcPos < 0 && calibrated[i] > hysteresisHigh)
                {
                    srcPos = i;
                }
                else if (srcPos >= 0 && calibrated[i] < hysteresisLow)
                {
                    position.Add(new Point(srcPos, i));
                    srcPos = -1;
                }

                founded[i] = (srcPos < 0) ? 0 : 1;
            }

            if (srcPos >= 0)
                position.Add(new Point(srcPos, proj.Length - 1));

            position.ForEach(f =>
            {
                //Rectangle rectangle;
                //if (sheerFinderParam.BaseXSearchDir == BaseXSearchDir.Left2Right)
                Rectangle rectangle = Rectangle.FromLTRB(findRect.Left, f.X, findRect.Right, f.Y);
                rectangle.Inflate(250, 250);
                rectangle.Intersect(findRect);
                //else
                //    rectangle = Rectangle.FromLTRB((algoImage.Width + basePosition.X) / 2, f.X, algoImage.Width, f.Y);


                RectangleF rectangleF = calibration.PixelToWorld(rectangle);

                Data.DefectObj sheetSubResult = new Data.DefectObj();
                sheetSubResult.PositionType = DefectObj.EPositionType.Sheet;
                sheetSubResult.Region = rectangle;
                sheetSubResult.RealRegion = rectangleF;

                IEnumerable<float> calibratedSub = calibrated.Skip(f.X).Take(f.Y - f.X);
                sheetSubResult.SubtractValueMax = (int)Math.Round(calibratedSub.Max());
                //sheetSubResult.SubtractValueMin = (int)Math.Round(calibratedSub.Min());

                //Rectangle clipRect = Rectangle.Intersect(new Rectangle(Point.Empty, algoImage.Size),
                //    Rectangle.Inflate(rectangle, 250, 250));
                Rectangle clipRect = rectangle;
                AlgoImage subAlgoImag2e = algoImage.GetSubImage(clipRect);
                sheetSubResult.Image = subAlgoImag2e.ToBitmap();
                subAlgoImag2e.Dispose();

                sheetSubResultList.Add(sheetSubResult);
            });

            return sheetSubResultList.ToArray();
        }
    }
}
