using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanG.Gravure.Vision.RCI.Calculator;
using UniScanG.Gravure.Vision.RCI.Trainer;

namespace UniScanG.Gravure.Vision.RCI
{
    class RCIHelper
    {
        public static Rectangle Anchor(bool anchorRight, Size imageSize, Rectangle rectangle)
        {
            if (!anchorRight)
                return rectangle;

            rectangle.X = imageSize.Width - rectangle.X - rectangle.Width;
            return rectangle;
        }

        /// <summary>
        /// 평균 127로...
        /// </summary>
        /// <param name="algoImage"></param>
        /// <param name="prjImg"></param>
        /// <param name="singleBuffer"></param>
        /// <param name="inflate"></param>
        /// <param name="direction"></param>
        public static void BuildSoblePrj(AlgoImage algoImage, AlgoImage prjImg, AlgoImage singleBuffer, Size inflate, Direction direction)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
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

            //algoImage.Save(@"C:\temp\prjImage.bmp");
            using (AlgoImage prjImage = algoImage.GetSubImage(prjRect))
            {
                float[] prj = ip.Projection(algoImage, direction);
                Debug.Assert(prj.Length == prjImg.Width);
                //Console.WriteLine(string.Join(Environment.NewLine, prj.Select(g => g.ToString())));

                byte[] bytes = new byte[prj.Length * sizeof(float)];

                RCIHelper.GetBytes(prj, bytes);
                singleBuffer.SetByte(bytes);
                ip.Average(singleBuffer);
                ip.Sobel(singleBuffer, singleBuffer, Direction.Horizontal);
                ip.MulMultiple(singleBuffer, singleBuffer, 0.5f, 128, 1);
                //ip.Add(singleBuffer, singleBuffer, 127);

                singleBuffer.GetByte(bytes);
                RCIHelper.GetSingles(bytes, prj);
                //Console.WriteLine(string.Join(Environment.NewLine, prj.Select(g => g.ToString())));

                prjImg.SetByte(RCIHelper.AbsRound(prj));
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
            ImagingLibrary imagingLibrary = OperationSettings.Instance().ImagingLibrary;

            float x, y;
            using (AlgoImage modelH = ImageBuilder.Build(imagingLibrary, ImageType.Depth, model.PrjH.Length, 1))
            {
                modelH.PutByte(RCIHelper.GetBytes(model.PrjH));

                using (AlgoImage targetH = ImageBuilder.Build(imagingLibrary, ImageType.Depth, tatget.PrjH.Length, 1))
                {
                    targetH.PutByte(RCIHelper.GetBytes(tatget.PrjH));

                    x = GetTmOffset(modelH, targetH, out scoresX) - model.Inflate.Width;
                }
            }

            using (AlgoImage modelV = ImageBuilder.Build(imagingLibrary, ImageType.Depth, model.PrjV.Length, 1))
            {
                modelV.PutByte(RCIHelper.GetBytes(model.PrjV));

                using (AlgoImage targetV = ImageBuilder.Build(imagingLibrary, ImageType.Depth, tatget.PrjV.Length, 1))
                {
                    targetV.PutByte(RCIHelper.GetBytes(tatget.PrjV));

                    y = GetTmOffset(modelV, targetV, out scoresY) - model.Inflate.Height;
                }
            }

            return new PointF(x, y);
        }

        internal static float GetTmOffset(AlgoImage image, AlgoImage template, out float[] scores)
        {
            using (AlgoImage result = ImageBuilder.Build(image.LibraryType, ImageType.Depth, (image.Width - template.Width + 1), (image.Height - template.Height + 1)))
            {
                return GetTmOffset(image, template, result, out scores);
            }
        }

        internal static float GetTmOffset(AlgoImage image, AlgoImage template, AlgoImage result, out float[] scores)
        {
            bool forDEBUG = false;
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(image);
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
                weight = scores;

                float val = scores.Max();
                center = Array.FindIndex(scores, f => f == val);
                if (val == 0)
                {
                    center = scores.Length / 2;
                    forDEBUG = true;
                }
            }
            
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
            ImagingLibrary imagingLibrary = OperationSettings.Instance().ImagingLibrary;
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(imagingLibrary);

            using (AlgoImage algoImage = ImageBuilder.Build(imagingLibrary, imageD, ImageType.Grey))
            {
                float[] prj = ip.Projection(algoImage, Direction.Horizontal);
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

            using (AlgoImage algoImage = ImageBuilder.Build(imagingLibrary, imageD, ImageType.Grey))
            {
                float[] prj = ip.Projection(algoImage, Direction.Horizontal);
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

        public static int GetBytes(float[] datas, byte[] bytes)
        {
            int size = datas.Length * sizeof(float);
            Debug.Assert(bytes.Length >= size);

            for (int i = 0; i < datas.Length; i++)
            {
                byte[] convert = BitConverter.GetBytes(datas[i]);
                Array.Copy(convert, 0, bytes, i * sizeof(float), convert.Length);
            }
            return size;
        }

        public static int GetBytes(float[] datas, int startIndex, int length, byte[] bytes)
        {
            int size = length * sizeof(float);
            Debug.Assert(bytes.Length >= size);

            for (int i = 0; i < length; i++)
            {
                byte[] convert = BitConverter.GetBytes(datas[i + startIndex]);
                Array.Copy(convert, 0, bytes, i * sizeof(float), convert.Length);
            }
            return size;
        }

        public static float[] GetSingles(byte[] bytes, int clipTails=0)
        {
            int size = (bytes.Length / sizeof(float)) - clipTails;
            float[] datas = new float[size];
            for (int i = 0; i < size; i++)
                datas[i] = BitConverter.ToSingle(bytes, i * sizeof(float));

            return datas;
        }
        public static int GetSingles(byte[] bytes, float[] datas, int clipTails = 0)
        {
            int size = (bytes.Length / sizeof(float)) - clipTails;
            Debug.Assert(datas.Length >= size);

            for (int i = 0; i < size; i++)
                datas[i] = BitConverter.ToSingle(bytes, i * sizeof(float));

            return size;
        }

        public static float WeightedCenterIndex(float[] datas, int startIdx, int length)
        {
            float[] weigth = new float[length];
            Array.Copy(datas, startIdx, weigth, 0, length);

            float weigthSum = weigth.Sum(f => Math.Abs(f));
            float sum = weigth.Select((f, i) => Math.Abs(f) * (startIdx + i)).Sum();
            return sum / weigthSum;
        }

        public static float WeightedCenterIndex(long[] datas, int startIdx, int length)
        {
            long[] weigth = new long[length];
            Array.Copy(datas, startIdx, weigth, 0, length);

            long weigthSum = weigth.Sum(f => Math.Abs(f));
            long sum = weigth.Select((f, i) => Math.Abs(f) * (startIdx + i)).Sum();
            return sum * 1.0f / weigthSum;
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

        internal static void SaveSingles(string v, IEnumerable<float> datas)
        {
            System.IO.File.WriteAllText(v, string.Join(Environment.NewLine, datas.Select(f => f.ToString())));
        }

        internal static void FindROI(AlgoImage algoImage, Rectangle seedRect, AlgoImage lineBuffer, Size boundaryMargin, out Rectangle roi, out float slope, DebugContext debugContext)
        {
            Debug.Assert(boundaryMargin.Width <= 0 && boundaryMargin.Height <= 0);

            Tuple<int, int> tupleX;
            Tuple<int, int>[] tupleYs = new Tuple<int, int>[3];
            PointF[] points = new PointF[tupleYs.Length];
            Rectangle fullRect = new Rectangle(Point.Empty, algoImage.Size);
            fullRect.Inflate(boundaryMargin);
            if (seedRect.IsEmpty)
                seedRect = new Rectangle(Point.Empty, algoImage.Size);
            seedRect.Intersect(fullRect);

            using (AlgoImage seedImage = algoImage.GetSubImage(seedRect))
            {
                tupleX = FindROIRange(seedImage, lineBuffer, Direction.Horizontal, -1, out float threshold, debugContext);
                int len = tupleX.Item2 - tupleX.Item1;
                Size subSize = new Size(len / tupleYs.Length, seedRect.Height);
                for (int i = 0; i < tupleYs.Length; i++)
                {
                    Point subLoc = Point.Add(new Point(tupleX.Item1, 0), new Size(subSize.Width * i, 0));
                    Rectangle subRect = new Rectangle(subLoc, subSize);
                    using (AlgoImage y = seedImage.GetSubImage(subRect))
                    {
                        tupleYs[i] = FindROIRange(y, lineBuffer, Direction.Vertical, threshold / 2, out float foundTh, debugContext);
                        points[i] = new PointF((subRect.Left + subRect.Right) / 2, tupleYs[i].Item1);
                    }
                }
            }

            // ROI
            int[] ts = null, bs = null;
            if (false)
            {
                ts = tupleYs.Select(f => f.Item1).OrderBy(f => f).ToList().GetRange(1, tupleYs.Length - 2).ToArray();
                bs = tupleYs.Select(f => f.Item2).OrderBy(f => f).ToList().GetRange(1, tupleYs.Length - 2).ToArray();
            }

            int l = tupleX.Item1;
            int t = (int)(ts == null ? tupleYs.Min(f => f.Item1) : ts.Average());
            int r = tupleX.Item2;
            int b = (int)(bs == null ? tupleYs.Max(f => f.Item2) : bs.Average());
            Rectangle roiRect = Rectangle.FromLTRB(l, t, r, b);
            roiRect.Offset(seedRect.Location);
            roiRect.Inflate(50, 50);
            roiRect.Intersect(fullRect);
            roi = roiRect;

            // Angle
            if (false)
            {
                int[] tsIdx = ts.Select(f => Array.FindIndex(tupleYs, g => g.Item1 == f)).OrderBy(f => f).ToArray();
                PointF[] ptsTop = tsIdx.Select(f => points[f]).ToArray();
                MathHelper.Regression1(ptsTop, out double coff01, out double coff00);

                int[] bsIdx = bs.Select(f => Array.FindIndex(tupleYs, g => g.Item2 == f)).OrderBy(f => f).ToArray();
                PointF[] ptsBot = bsIdx.Select(f => points[f]).ToArray();
                MathHelper.Regression1(ptsBot, out double coff11, out double coff10);

                slope = (float)((coff01 + coff11) / 2);
                double deg = MathHelper.RadToDeg(Math.Atan(slope));
            }
            else
            {
                MathHelper.Regression1(points, out double coff1, out double coff0);
                slope = (float)coff1;
            }
        }

        private static Tuple<int, int> FindROIRange(AlgoImage algoImage, AlgoImage lineBuffer, Direction direction, float threshold, out float foundTh, DebugContext debugContext)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
            algoImage.Save(@"FindRoiRange.bmp", debugContext);
            float[] prj = ip.Projection(algoImage, direction, ProjectionType.Mean);
            Debug.Assert(lineBuffer.Width >= prj.Length);

            float[] datas;
            lineBuffer.Clear();
            using (AlgoImage projAlgoImage = lineBuffer.GetSubImage(new Rectangle(0, 0, prj.Length, 1)))
            {
                projAlgoImage.SetByte(RCIHelper.GetBytes(prj));
                //datas = RCIHelper.GetSingles(projAlgoImage.GetByte());
                //RCIHelper.SaveSingles(@"C:\temp\datas.txt", datas);

                ip.Average(projAlgoImage);
                ip.Sobel(projAlgoImage, projAlgoImage, Direction.Horizontal);
                datas = RCIHelper.GetSingles(projAlgoImage.GetByte(), 2);

                if (debugContext.SaveDebugImage)
                    RCIHelper.SaveSingles(System.IO.Path.Combine(debugContext.FullPath, "FindRoiRange.txt"), datas);
            }
            return FindROIRange(datas, threshold, out foundTh);
        }

        private static Tuple<int, int> FindROIRange(float[] datas, float threshold, out float foundTh)
        {
            // 밝다가 어두워짐(Falling) -> 시작점
            // 어두웠다 밝아짐(Rising) -> 끝점.
            if (threshold < 0)
            {
                float[] absDatas = datas.Select(f => Math.Abs(f)).ToArray();
                double average = absDatas.Average();
                float[] overDatas = Array.FindAll(absDatas, f => f > average).ToArray();
                threshold = overDatas.Average() * 1.3f;
                //RCIHelper.SaveSingles(@"C:\temp\datas.txt", datas);
                //RCIHelper.SaveSingles(@"C:\temp\absDatas.txt", absDatas);
                //average3 = 3 * absDatas.Average();
            }
            foundTh = (float)threshold;

            float src = -1;
            int srcFindStart = 0;
            while (src < 0)
            {
                int srcSrc = Array.FindIndex(datas, srcFindStart, f => f < -threshold);
                if (srcSrc < 0)
                    break;

                int srcDst = Array.FindIndex(datas, srcSrc, f => f > -threshold);
                if (srcDst < 0)
                    srcDst = datas.Length - 1;

                if (srcDst - srcSrc > 0)
                    src = srcSrc;// RCIHelper.WeightedCenterIndex(datas, srcSrc, srcDst - srcSrc);

                srcFindStart = srcDst;
            }

            float dst = -1;
            int dstFindStart = datas.Length - 1;
            while (dst < 0)
            {
                int dstSrc = Array.FindLastIndex(datas, dstFindStart, f => f > threshold);
                if (dstSrc < 0)
                    break;

                int dstDst = Array.FindLastIndex(datas, dstSrc, f => f < threshold);
                if (dstDst < 0)
                    dstDst = 0;

                if (dstSrc - dstDst > 0)
                    dst = RCIHelper.WeightedCenterIndex(datas, dstDst, dstSrc - dstDst);
                dstFindStart = dstDst;
            }

            return new Tuple<int, int>((int)src, (int)dst);
        }
    }

    public static class SIMD
    {
        const string dllName = "UniScanG.Vision.dll";

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        extern static void CalibrateLine(ImageData src, ImageData dst, ImageData mul100, [MarshalAs(UnmanagedType.I4)]int width, int heigth);

        [StructLayout(LayoutKind.Sequential)]
        struct ImageData
        {
            IntPtr ptr;

            [MarshalAs(UnmanagedType.I4)]
            int pitch;

            public ImageData(IntPtr ptr, int pitch)
            {
                this.ptr = ptr;
                this.pitch = pitch;
            }
        }

        public static void IterateProduct(IEnumerable<byte> datas, IEnumerable<byte> coffs, IEnumerable<byte> results, Size size)
        {
            IterateProduct(datas, coffs, results, size.Width, size.Height);
        }

        public static void IterateProduct(IntPtr datas, IEnumerable<byte> coffs, IntPtr results, int pitch, int heigth)
        {
            Debug.Assert(coffs.Count() == pitch);

            GCHandle gcHandleMul = GCHandle.Alloc(coffs, GCHandleType.Pinned);
            IntPtr mulPtr = gcHandleMul.AddrOfPinnedObject();

            IterateProduct(datas, mulPtr, results, pitch, heigth);

            gcHandleMul.Free();
        }

        public static void IterateProduct(IEnumerable<byte> datas, IEnumerable<byte> coffs, IEnumerable<byte> results, int width, int heigth)
        {
            Debug.Assert(datas.Count() == results.Count());

            GCHandle gcHandleSrc = GCHandle.Alloc(datas, GCHandleType.Pinned);
            GCHandle gcHandleDst = GCHandle.Alloc(results, GCHandleType.Pinned);
            GCHandle gcHandleMul = GCHandle.Alloc(coffs, GCHandleType.Pinned);

            IntPtr srcPtr = gcHandleSrc.AddrOfPinnedObject();
            IntPtr dstPtr = gcHandleDst.AddrOfPinnedObject();
            IntPtr mulPtr = gcHandleMul.AddrOfPinnedObject();

            IterateProduct(srcPtr, mulPtr, dstPtr, width, heigth);

            gcHandleMul.Free();
            gcHandleDst.Free();
            gcHandleSrc.Free();
        }

        public static void IterateProduct(IntPtr datas08, IntPtr coffs08, IntPtr results08, int pitch, int heigth)
        {
            ImageData src = new ImageData(datas08, pitch);
            ImageData mul = new ImageData(coffs08, pitch);
            ImageData dst = new ImageData(results08, pitch);

            CalibrateLine(src, dst, mul, pitch, heigth);
        }
    }
}
