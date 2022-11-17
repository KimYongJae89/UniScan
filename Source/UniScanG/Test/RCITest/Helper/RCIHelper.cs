using DynMvp.Base;
using DynMvp.Vision;
using RCITest.Processer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Vision.RCI;
using UniScanG.Gravure.Vision.RCI.Calculator;
using UniScanG.Gravure.Vision.RCI.Trainer;

namespace RCITest.Helper
{
    class RCIHelper
    {
        public static void BuildSoblePrj(AlgoImage algoImage, AlgoImage prjImg, AlgoImage singleBuffer, Size inflate, Direction direction)
        {
            ImageProcessing ip = Program.ImageProcessing;
            Rectangle prjRect = new Rectangle(Point.Empty, algoImage.Size);
            switch (direction)
            {
                case Direction.Horizontal:
                    prjRect.Inflate(0, -inflate.Height);
                    break;
                case Direction.Vertical:
                    prjRect.Inflate(-inflate.Width, 0);
                    break;
            }

            using (AlgoImage prjImage = algoImage.GetSubImage(prjRect))
            {
                float[] prj = ip.Projection(algoImage, direction);
                Debug.Assert(prj.Length == prjImg.Width);

                singleBuffer.SetByte(RCIHelper.GetBytes(prj));
                ip.Average(singleBuffer);
                ip.Sobel(singleBuffer, singleBuffer, Direction.Horizontal);

                float[] sobelDatas = RCIHelper.GetSingles(singleBuffer.GetByte());
                prjImg.SetByte(RCIHelper.AbsRound(sobelDatas));
            }
        }

        public static PointF GetTmOffset(BlockProjection model, BlockProjection tatget, BlockProjection result, out float[] scoresX, out float[] scoresY)
        {
            float x = GetTmOffset(model.Horizental, tatget.Horizental, result.Horizental, out scoresX) - model.Inflate.Width;
            float y = GetTmOffset(model.Veritical, tatget.Veritical, result.Veritical, out scoresY) - model.Inflate.Height;
            return new PointF(x, y);
        }

        public static PointF GetTmOffset(ProjectionData model, ProjectionData tatget, out float[] scoresX, out float[] scoresY)
        {
            float x, y;
            using (AlgoImage modelH = ImageBuilder.Build(Program.ImagingLibrary, ImageType.Depth, model.PrjH.Length, 1))
            {
                modelH.PutByte(RCIHelper.GetBytes(model.PrjH));

                using (AlgoImage targetH = ImageBuilder.Build(Program.ImagingLibrary, ImageType.Depth, tatget.PrjH.Length, 1))
                {
                    targetH.PutByte(RCIHelper.GetBytes(tatget.PrjH));

                    x = GetTmOffset(modelH, targetH, out scoresX) - model.Inflate.Width;
                }
            }

            using (AlgoImage modelV = ImageBuilder.Build(Program.ImagingLibrary, ImageType.Depth, model.PrjV.Length, 1))
            {
                modelV.PutByte(RCIHelper.GetBytes(model.PrjV));

                using (AlgoImage targetV = ImageBuilder.Build(Program.ImagingLibrary, ImageType.Depth, tatget.PrjV.Length, 1))
                {
                    targetV.PutByte(RCIHelper.GetBytes(tatget.PrjV));

                    y = GetTmOffset(modelV, targetV, out scoresY) - model.Inflate.Height;
                }
            }

            return new PointF(x, y);
        }

        internal static float GetTmOffset(AlgoImage image, AlgoImage template, out float[] scores)
        {
            using (AlgoImage result = ImageBuilder.Build(Program.ImagingLibrary, ImageType.Depth, (image.Width - template.Width + 1), (image.Height - template.Height + 1)))
            {
                return GetTmOffset(image, template, result, out scores);
            }
        }

        internal static float GetTmOffset(AlgoImage image, AlgoImage template, AlgoImage result, out float[] scores)
        {
            ImageProcessing ip = Program.ImageProcessing;
            result.Clear();
            ip.Match(image, template, result);

            scores = RCIHelper.GetSingles(result.GetByte());
            float center = 0;
            float[] weight = null;
            if (false)
            // 점수의 가중합
            {
                //weight = RCIHelper.Normalize(scores.Select(f => f * -1)).Select(f => (float)Math.Pow(f, 2)).ToArray(); // invert-normalize-square
                //weight = RCIHelper.Normalize(RCIHelper.Reciprocal(scores)).Select(f => (float)Math.Pow(f, 2)).ToArray();// reciprocal-normalize-square
                weight = scores.Select(f => f < 0 ? 0 : (float)Math.Pow(f, 2)).ToArray();// square
                center = RCIHelper.WeightedCenterIndex(weight, 0, weight.Length);
            }
            else
            // 최대 인덱스
            {
                float val = scores.Max();
                weight = scores;
                center = Array.FindIndex(scores, f => f == val);
            }

            bool forDEBUG = Math.Abs(scores.Length / 2 - center) > 2;
            forDEBUG = false;
            if (forDEBUG)
            {
                byte[] imageDatas = image.GetByte();
                byte[] templateDatas = template.GetByte();
                int templateStart = (int)Math.Round(center);
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < imageDatas.Length; i++)
                {
                    sb.Append($"{imageDatas[i]},");

                    if (i < templateStart)
                        sb.Append($",");
                    else if (i < templateStart + templateDatas.Length)
                        sb.Append($"{templateDatas[i - templateStart]},");

                    if (i < weight.Length)
                        sb.Append($"{scores[i]}");
                    sb.AppendLine();
                }
                System.IO.File.WriteAllText(@"C:\temp\imageBytes.txt", sb.ToString());
            }

            return center;
        }

        internal static void Uniformize(ImageD imageD)
        {
            double coff0, coff1, coff2;

            using (AlgoImage algoImage = ImageBuilder.Build(Program.ImagingLibrary, imageD, ImageType.Grey))
            {
                float[] prj = Program.ImageProcessing.Projection(algoImage, Direction.Horizontal);
                //System.IO.File.WriteAllText(@"C:\temp\prj.txt", string.Join(Environment.NewLine, prj.Select(f => f.ToString())));

                float[] maxs = MovingMaximum(prj, 100);
                //System.IO.File.WriteAllText(@"C:\temp\maxs.txt", string.Join(Environment.NewLine, maxs.Select(f => f.ToString())));

                //float[] mins = MinimumFilter(maxs, 100);
                //System.IO.File.WriteAllText(@"C:\temp\mins.txt", string.Join(Environment.NewLine, mins.Select(f => f.ToString())));

                float[] diffs = Difference(maxs);
                float[] diffMeans = MovingAverage(diffs, 3);
                //System.IO.File.WriteAllText(@"C:\temp\diff.txt", string.Join(Environment.NewLine, diffMeans.Select(f => f.ToString())));

                float[] absDiffs = diffs.Select(f => Math.Abs(f)).ToArray();
                float diffMean = RCIHelper.AverageIf(absDiffs, absDiffs.Average());
                float diffMean2 = RCIHelper.AverageIf(absDiffs, diffMean);

                bool isPos = false;
                List<PointF> posList = new List<PointF>();
                List<PointF> negList = new List<PointF>();
                for (int i = 0; i < diffs.Length; i++)
                {
                    float diff = diffs[i];

                    if (diff >= diffMean2 && !isPos)
                        isPos = true;
                    else if (diffMean2 < -diff && isPos)
                        isPos = false;

                    PointF ptf = new PointF(i, maxs[i]);
                    if (isPos)
                        posList.Add(ptf);
                    else
                        negList.Add(ptf);
                }
                //System.IO.File.WriteAllText(@"C:\temp\Pos.txt", string.Join(Environment.NewLine, posList.Select(f => $"{f.X}, {f.Y}")));
                //System.IO.File.WriteAllText(@"C:\temp\Neg.txt", string.Join(Environment.NewLine, negList.Select(f => $"{f.X}, {f.Y}")));

                double[] xs = posList.Select(f => (double)f.X).ToArray();
                double[] ys = posList.Select(f => (double)f.Y).ToArray();
                Regression2(xs, ys, out coff0, out coff1, out coff2);
            }

            float[] scale = new float[imageD.Width];
            for (int i = 0; i < imageD.Width; i++)
                scale[i] = (float)(127f / (coff0 + coff1 * i + coff2 * i * i));
            Image2D image2D = (Image2D)imageD;
            byte[] datas = image2D.Data.Select((f, i) => (byte)(f * scale[i % image2D.Pitch])).ToArray();
            Array.Copy(datas, image2D.Data, datas.Length);
            image2D.SaveImage(@"C:\temp\Test.png");

            using (AlgoImage algoImage = ImageBuilder.Build(Program.ImagingLibrary, imageD, ImageType.Grey))
            {
                float[] prj = Program.ImageProcessing.Projection(algoImage, Direction.Horizontal);
                System.IO.File.WriteAllText(@"C:\temp\prj2.txt", string.Join(Environment.NewLine, prj.Select(f => f.ToString())));
            }
        }

        public static byte[] AbsRound(float[] datas)
        {
            return datas.Select(f => (byte)Math.Round(Math.Abs(f))).ToArray();
        }

        public static byte[] GetBytes(float[] datas)
        {
            byte[] bytes = new byte[datas.Length * sizeof(float)];
            for (int i = 0; i < datas.Length; i++)
            {
                byte[] convert = BitConverter.GetBytes(datas[i]);
                Array.Copy(convert, 0, bytes, i * sizeof(float), convert.Length);
            }
            return bytes;
        }

        public static float[] GetSingles(byte[] bytes)
        {
            float[] datas = new float[bytes.Length / sizeof(float)];
            for (int i = 0; i < datas.Length; i++)
                datas[i] = BitConverter.ToSingle(bytes, i * sizeof(float));

            return datas;
        }

        public static float WeightedCenterIndex(float[] datas, int startIdx, int length)
        {
            float[] weigth = new float[length];
            Array.Copy(datas, startIdx, weigth, 0, length);

            float weigthSum = weigth.Sum(f => Math.Abs(f));
            float sum = weigth.Select((f, i) => Math.Abs(f) * (startIdx + i)).Sum();
            return sum / weigthSum;
        }

        internal static IEnumerable<float> Normalize(IEnumerable<float> datas, float min = 0, float max = 1)
        {
            float dataMin = datas.Min();
            float dataMax = datas.Max();
            float dataRange = dataMax - dataMin;
            return datas.Select(f => ((f - dataMin) / (dataRange)) * (max - min) + min).ToArray();
        }

        internal static IEnumerable<float> Reciprocal(IEnumerable<float> datas)
        {
            return datas.Select(f => f == 0 ? float.MaxValue : 1 / f).ToArray();
        }

        internal static void SaveTmOffsetDebugData(string path, ProjectionData projection1, ProjectionData projection2, Point point, float[] scores1, float[] scores2)
        {
            for (int y = 0; y < Math.Max(projection1.PrjH.Length, projection2.PrjH.Length); y++)
            {
                System.IO.File.WriteAllText(System.IO.Path.Combine(path, "TmOffset_X.txt"), GetTmOffsetDebugData(projection1.PrjH, projection2.PrjH, scores1, point.X));
                System.IO.File.WriteAllText(System.IO.Path.Combine(path, "TmOffset_Y.txt"), GetTmOffsetDebugData(projection1.PrjV, projection2.PrjV, scores2, point.Y));
            }
        }

        internal static void SaveTmOffsetDebugData(string path, float[] prj1, float[] prj2, float[] scores, int match)
        {
            System.IO.File.WriteAllText(path, GetTmOffsetDebugData(prj1, prj2, scores, match));
        }

        private static string GetTmOffsetDebugData(float[] prj1, float[] prj2, float[] scores, int match)
        {
            int max = Math.Max(prj1.Length, prj2.Length);
            int src1 = (prj1.Length > prj2.Length) ? 0 : (prj1.Length - prj2.Length) / 2 + match;
            int src2 = (prj2.Length > prj1.Length) ? 0 : (prj1.Length - prj2.Length) / 2 + match;
            int src3 = 0;

            RCIDataWriter w = new RCIDataWriter(null, "");
            for (int i = 0; i < max; i++)
            {
                if (i >= src1 && i < src1 + prj1.Length)
                    w.Add(0, i, prj1[i - src1]);

                if (i >= src2 && i < src2 + prj2.Length)
                    w.Add(1, i, prj2[i - src2]);

                if (i >= src3 && i < src3 + scores.Length)
                    w.Add(2, i, scores[i - src3]);
            }

            return w.ToString();
        }

        internal static void Regression2(float[] datas, out double coff2, out double coff1, out double coff0)
        {
            List<double> x = datas.Select((f, i) => (double)i).ToList();
            List<double> y = datas.Select((f, i) => (double)f).ToList();

            // y = b0 + b1*x + b2*x^2
            Regression2(x.ToArray(), y.ToArray(), out coff2, out coff1, out coff0);
            Console.WriteLine($"RCIHelper::Regression2 - coff2: {coff2}, coff1: {coff1}, coff0: {coff0}");
        }

        internal static void Regression2(double[] srcX, double[] srcY, out double coff2, out double coff1, out double coff0)
        {
            double Y = srcY.Sum();
            double X = srcX.Select(f => Math.Pow(f, 1)).Sum();
            double X2 = srcX.Select(f => Math.Pow(f, 2)).Sum();
            double X3 = srcX.Select(f => Math.Pow(f, 3)).Sum();
            double X4 = srcX.Select(f => Math.Pow(f, 4)).Sum();
            double K = 0.0;
            double L = 0.0;
            int n = srcX.Length;

            for (int i = 0; i < n; i++)
            {
                K += (srcY[i] * srcX[i] * srcX[i]);
                L += (srcY[i] * srcX[i]);
            }

            double denominator = -n * X4 * X2 + X4 * X * X + X2 * X2 * X2 + X3 * X3 * n - 2 * X3 * X * X2;
            double b0p = -(Y * X4 * X2 - Y * X3 * X3 - X * L * X4 + X * X3 * K - X2 * X2 * K + X2 * X3 * L);
            double b1p = X * Y * X4 - X * K * X2 - L * n * X4 + X3 * n * K - Y * X2 * X3 + X2 * X2 * L;
            double b2p = -(K * n * X2 - K * X * X - X2 * X2 * Y - X3 * n * L + X3 * X * Y + X * X2 * L);

            coff0 = b0p / denominator;
            coff1 = b1p / denominator;
            coff2 = b2p / denominator;
        }

        internal static float[] MovingMaximum(IEnumerable<float> datas, int filterSize)
        {
            List<float> list = new List<float>(datas);

            int dataLen = datas.Count();
            float[] maximum = new float[dataLen];
            for (int i = 0; i < dataLen; i++)
            {
                int src = Math.Max(i - filterSize / 2, 0);
                int dst = Math.Min(i + filterSize / 2, dataLen);
                maximum[i] = list.GetRange(src, dst - src).Max();
            }

            return maximum;
        }

        internal static float[] MovingMinimum(IEnumerable<float> datas, int filterSize)
        {
            List<float> list = new List<float>(datas);

            int dataLen = datas.Count();
            float[] minimum = new float[dataLen];
            for (int i = 0; i < dataLen; i++)
            {
                int src = Math.Max(i - filterSize / 2, 0);
                int dst = Math.Min(i + filterSize / 2, dataLen);
                minimum[i] = list.GetRange(src, dst - src).Min();
            }

            return minimum;
        }

        internal static float[] MovingAverage(IEnumerable<float> datas, int filterSize)
        {
            List<float> list = new List<float>(datas);

            int dataLen = datas.Count();
            float[] mean = new float[dataLen];
            for (int i = 0; i < dataLen; i++)
            {
                int src = Math.Max(i - filterSize / 2, 0);
                int dst = Math.Min(i + filterSize / 2, dataLen);
                mean[i] = list.GetRange(src, dst - src).Average();
            }

            return mean;
        }

        internal static float[] MovingSum(IEnumerable<float> datas, int filterSize)
        {
            List<float> list = new List<float>(datas);

            int dataLen = datas.Count();
            float[] maximum = new float[dataLen];
            for (int i = 0; i < dataLen; i++)
            {
                int src = Math.Max(i - filterSize / 2, 0);
                int dst = Math.Min(i + filterSize / 2, dataLen);
                maximum[i] = list.GetRange(src, dst - src).Sum();
            }

            return maximum;

        }
        internal static float[] Difference(IEnumerable<float> datas)
        {
            List<float> list = new List<float>(datas);

            float[] diffs = new float[datas.Count() - 1];
            for (int i = 0; i < diffs.Length; i++)
                diffs[i] = list[i + 1] - list[i];

            return diffs;
        }

        internal static float AverageIf(float[] datas, float overAndEqualThan)
        {
            float[] found = Array.FindAll(datas, f => f >= overAndEqualThan);
            return found.Average();
        }

        internal static void SaveSingles(string v, IEnumerable< float> datas)
        {
            System.IO.File.WriteAllText(v, string.Join(Environment.NewLine, datas.Select(f => f.ToString())));
        }
    }
}
