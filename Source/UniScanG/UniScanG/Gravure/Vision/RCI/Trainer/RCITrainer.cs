using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Vision;
using UniEye.Base.Settings;
using UniScanG.Data.Model;
using UniScanG.Gravure.Vision.Extender;

namespace UniScanG.Gravure.Vision.RCI.Trainer
{
    public class RCITrainer : Vision.Trainer.TrainerBase
    {
        Calibration Calibration { get; set; }

        public RCITrainer() : base(ETrainerVersion.RCI)
        {
            this.Calibration = SystemManager.Instance()?.DeviceBox.CameraCalibrationList.FirstOrDefault();
            if (this.Calibration == null)
                this.Calibration = new ScaledCalibration(new SizeF(14, 14));
        }

        public override Point TrainBaseline(AlgoImage trainImage, Model model, int start, int end, DebugContextG debugContextG)
        {
            model.CalculatorModelParam.BasePosition = Point.Empty;
            return Point.Empty;
        }

        public override bool TrainPattern(AlgoImage algoImage, Model model, int start, int end, DebugContextG debugContext)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
            RCIOptions options = model.RCIOptions;
            RCITrainResult result = model.RCITrainResult;
            RCIGlobalOptions globalOptions = AlgorithmSetting.Instance().RCIGlobalOptions;

            model.CalculatorModelParam.PatternGroupCollection.Clear();
            //worker?.ReportProgress(start, StringManager.GetString(this.GetType().FullName, "Analyze Pattern"));

            int iStep = -1;
            string[] messages = new string[] { "Uniformize...", "Find ROI...", "Reconstruct...", };
            float step = (end - start) * 1f / messages.Length;

            // Uniformize
            iStep = 0;
            worker?.ReportProgress(start + (int)(step * (iStep + 1)), StringManager.GetString(this.GetType().FullName, messages[iStep]));

            GetUniformizeFactor(algoImage, out double coff2, out double coff1, out double coff0);
            result.UniformizeCoefficients = new double[] { coff0, coff1, coff2 };
            result.UniformizeLength = algoImage.Width;

            if (false)
                // LINQ
            {
                byte[] datas = result.GetPRNUDatas(algoImage.Width, AlgorithmSetting.Instance().RCIGlobalOptions.UniformizeGv);

                byte[] bytes = algoImage.GetByte();
                byte[] bytes2;
                bytes2 = bytes.Select((f, i) => (byte)(f * datas[i % algoImage.Pitch])).ToArray();

                algoImage.SetByte(bytes2);
            }
            else
                // AVX
            {
                //bytes2 = new byte[bytes.Length];
                //RCIHelperWithSIMD.IterateProduct(bytes, datas, bytes2, algoImage.Width, algoImage.Height);

                IntPtr ptr = algoImage.GetImagePtr();
                int pitch = algoImage.PitchLib;
                int height = algoImage.Height;
                byte[] datas = result.GetPRNUDatas(pitch, AlgorithmSetting.Instance().RCIGlobalOptions.UniformizeGv);

                //algoImage.Save(@"C:\temp\a.bmp");
                SIMD.IterateProduct(ptr, datas, ptr, pitch, algoImage.Height);
                //algoImage.Save(@"C:\temp\b.bmp");
            }

            result.ImageD = algoImage.ToImageD();
            //algoImage.Save(@"uniformize.bmp", debugContext);
            ThrowIfCancellationPending();

            // FIndROI
            iStep = 1;
            worker?.ReportProgress(start + (int)(step * (iStep + 1)), StringManager.GetString(this.GetType().FullName, messages[iStep]));

            Rectangle roi;
            float slope;
            Rectangle roiSeedRange = options.ROISeedRect;
            if (roiSeedRange.Width <= 0) roiSeedRange.Width = algoImage.Width - roiSeedRange.X;
            if (roiSeedRange.Height <= 0) roiSeedRange.Height = algoImage.Height - roiSeedRange.Y;

            using (AlgoImage lineBuffer = ImageBuilder.Build(algoImage.LibraryType, ImageType.Depth, Math.Max(roiSeedRange.Width, roiSeedRange.Height), 1))
                RCIHelper.FindROI(algoImage, roiSeedRange, lineBuffer, new Size(-50, -50), out roi, out slope, debugContext);
            result.ROI = roi;
            result.Slope = slope;
            ThrowIfCancellationPending();

            // Reconstruct
            iStep = 2;
            worker?.ReportProgress(start + (int)(step * (iStep + 1)), StringManager.GetString(this.GetType().FullName, messages[iStep]));

            float chipShare100p;
            using (AlgoImage roiImage = algoImage.GetSubImage(roi))
            {
                roiImage.Save(@"roiImage.bmp", debugContext);
                ImageD reconImageD, weightImageD;
                Reconstruct(roiImage, model.RCIOptions.ReconstructOptions, out reconImageD, out weightImageD);

                ThrowIfCancellationPending();

                result.ReconImageD = reconImageD;
                result.WeightImageD = weightImageD;
                using (AlgoImage reconImage = ImageBuilder.Build(algoImage.LibraryType, reconImageD, ImageType.Grey))
                {
                    long[] histo = ip.Histogram(reconImage);
                    int otsu = (int)ImageProcessing.Otsu(histo);
                    result.Otsu = otsu;
                    result.DarkMean = RCIHelper.WeightedCenterIndex(histo, 0, otsu);
                    result.WhiteMean = RCIHelper.WeightedCenterIndex(histo, otsu, histo.Length - otsu);

                    ip.Binarize(reconImage);
                    chipShare100p = GetShareP(reconImage);
                    ip.Erode(reconImage, 3);
                    result.BgImageD = reconImage.ToImageD();
                    
                }
            }
            model.ChipShare100p = chipShare100p;
            model.ImageModified = true;

            return true;
        }

        private float GetShareP(AlgoImage algoImage)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
            Rectangle rectangle = Rectangle.Inflate(new Rectangle(Point.Empty, algoImage.Size), -50, -50);
            Size size = new Size(rectangle.Width / 5, rectangle.Height);

            Rectangle[] rects = new Rectangle[5]
                {
                    new Rectangle(rectangle.Left + size.Width*0, 0, size.Width, size.Height),
                    new Rectangle(rectangle.Left + size.Width*1, 0, size.Width, size.Height),
                    new Rectangle(rectangle.Left + size.Width*2, 0, size.Width, size.Height),
                    new Rectangle(rectangle.Left + size.Width*3, 0, size.Width, size.Height),
                    new Rectangle(rectangle.Left + size.Width*4, 0, size.Width, size.Height),
                };

            double[] dd = rects.Select(f =>
            {
                using (AlgoImage clip = algoImage.GetSubImage(f))
                {
                    StatResult statResult = ip.GetStatValue(clip);
                    //long sumPixelValue = (long)(statResult.average * statResult.count);
                    //long whitePixelCount = sumPixelValue / 255;
                    //return 100- (whitePixelCount * 100f / statResult.count);
                    return (1 - statResult.average / 255) * 100;
                }
            }).ToArray();

            return (float)dd.OrderBy(f => f).Skip(1).Take(3).Average();
        }

        private static void GetUniformizeFactor(AlgoImage algoImage, out double coff2, out double coff1, out double coff0)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

            int filterLen = 130;
            int filterLen2 = filterLen / 2;
            Action<List<PointF>, float[], int, int> action = new Action<List<PointF>, float[], int, int>((list, array, start, end) =>
            {
                for (int i = start; i < end; i++)
                    list.Add(new PointF(i, array[i]));
                list.Add(new PointF(-1, -1));
            });

            float[] prj = ip.Projection(algoImage, Direction.Horizontal);
            //System.IO.File.WriteAllText(@"C:\temp\prj.txt", string.Join(Environment.NewLine, prj.Select(f => f.ToString())));

            float[] maxs = RCIHelper.MovingMaximum(prj, filterLen);
            //System.IO.File.WriteAllText(@"C:\temp\maxs.txt", string.Join(Environment.NewLine, maxs.Select(f => f.ToString())));

            //float[] mins = MinimumFilter(maxs, 100);
            //System.IO.File.WriteAllText(@"C:\temp\mins.txt", string.Join(Environment.NewLine, mins.Select(f => f.ToString())));

            float[] diffs = RCIHelper.Difference(maxs);
            //System.IO.File.WriteAllText(@"C:\temp\diff.txt", string.Join(Environment.NewLine, diffs.Select(f => f.ToString())));
            //diffs = RCIHelper.MovingAverage(diffs, 3);
            diffs = RCIHelper.MovingSum(diffs, 5);
            //System.IO.File.WriteAllText(@"C:\temp\diff.txt", string.Join(Environment.NewLine, diffs.Select(f => f.ToString())));
            IEnumerable<Tuple<float, bool>> spikes = FindSpikes(diffs, 0, out float threshold);

            List<PointF> posList = new List<PointF>();
            List<PointF> negList = new List<PointF>();
            int src = 0;
            foreach (Tuple<float, bool> spike in spikes)
            {
                int dst = (int)Math.Round(spike.Item1);
                bool polarity = spike.Item2;
                if (polarity)
                    action(negList, maxs, src, dst);
                else
                    action(posList, maxs, src, dst);
                src = dst;
            }

            if (src != algoImage.Width)
            {
                if (spikes.Last().Item2)
                    action(posList, maxs, src, algoImage.Width);
                else
                    action(negList, maxs, src, algoImage.Width);
            }

            //float[] absDiffs = diffs.Select(f => Math.Abs(f)).ToArray();
            //float diffMean = RCIHelper.AverageIf(absDiffs, absDiffs.Average());
            //float diffMean2 = RCIHelper.AverageIf(absDiffs, diffMean);

            //bool isPos = false;
            //List<PointF> posList = new List<PointF>();
            //List<PointF> negList = new List<PointF>();
            //float[] temps = new float[5];
            //for (int i = 0; i < diffs.Length - temps.Length; i++)
            //{
            //    int src = Math.Max(i - temps.Length / 2, 0);
            //    int dst = Math.Min(i + temps.Length / 2, diffs.Length);
            //    Array.Copy(diffs, src, temps, 0, dst - src);
            //    float diff = temps.Sum();

            //    if (diff >= diffMean2 && !isPos)
            //        isPos = true;
            //    else if (diffMean2 < -diff && isPos)
            //        isPos = false;

            //    PointF ptf = new PointF(i, maxs[i]);
            //    if (isPos)
            //        posList.Add(ptf);
            //    else
            //        negList.Add(ptf);
            //}
            //System.IO.File.WriteAllText(@"C:\temp\Pos.txt", string.Join(Environment.NewLine, posList.Select(f => $"{f.X}, {f.Y}")));
            //System.IO.File.WriteAllText(@"C:\temp\Neg.txt", string.Join(Environment.NewLine, negList.Select(f => $"{f.X},, {f.Y}")));

            List<PointF> workList = (posList.Count > negList.Count) ? posList : negList;
            float posMean = posList.Average(f => f.Y);
            float negMean = negList.Average(f => f.Y);
            List<PointF[]> groups = new List<PointF[]>();
            while (workList.Count > 0)
            {
                int indx = workList.FindIndex(f => f.X < 0);
                if (indx < 0)
                {
                    groups.Add(workList.ToArray());
                    workList.Clear();
                }
                else
                {
                    List<PointF> subList = workList.GetRange(0, indx);
                    groups.Add(subList.ToArray());
                    workList.RemoveRange(0, indx + 1);
                }
            }
            int total = groups.Sum(f => f.Length);
            float[] means = groups.Select(f => f.Average(g => g.Y)).ToArray();
            //groups.RemoveAll(f => f.Length * 1f / total < 0.05);
            workList.AddRange(groups.SelectMany(f => f));

            double[] xs = workList.Select(f => (double)f.X).ToArray();
            double[] ys = workList.Select(f => (double)f.Y).ToArray();
            RCIHelper.Regression2(xs, ys, out coff2, out coff1, out coff0);
        }

        private static void Reconstruct(AlgoImage algoImage, RCIReconOptions reconOptions, out ImageD modelImageD, out ImageD weightImageD)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

            int divFactor = 1;
            Rectangle wholeRect = new Rectangle(Point.Empty, algoImage.Size);
            Rectangle scaleRect = new Rectangle(Point.Empty, DrawingHelper.Div(wholeRect.Size, divFactor));

            // output
            AlgoImage modelImage = ImageBuilder.Build(algoImage.LibraryType, ImageType.Grey, wholeRect.Size);
            AlgoImage weightImage = ImageBuilder.Build(algoImage.LibraryType, ImageType.Grey, wholeRect.Size);

            // Input
            AlgoImage scAlgoImage = ImageBuilder.Build(algoImage.LibraryType, ImageType.Grey, scaleRect.Size);
            ip.Resize(algoImage, scAlgoImage);
            //scAlgoImage.Save(@"C:\temp\bmp\scAlgoImage.bmp");

            AlgoImage scModelImage = ImageBuilder.Build(algoImage.LibraryType, ImageType.Grey, scaleRect.Size);
            AlgoImage scWeightImage = ImageBuilder.Build(algoImage.LibraryType, ImageType.Grey, scaleRect.Size);
            scModelImage.Clear();
            scWeightImage.Clear();

            // 모델이미지 구성
            Reconstrctor reconstrctor = Reconstrctor.Create(reconOptions.Reconstruct);
            reconstrctor.Reconstruct(scAlgoImage, scModelImage, scWeightImage);

            // 가중치 이미지
            using (AlgoImage scWeightTempImage = scWeightImage.Clone())
            {
                // 엣지 확장
                //Rectangle clipRect = new Rectangle(8221, 8230, 490, 496);
                //using (AlgoImage edgeExtendWithAverage = scWeightTempImage.Clone())
                //{
                //    for (int i = 0; i < reconOptions.EdgeSmoothCount; i++)
                //        ip.Average(edgeExtendWithAverage);

                //    using (AlgoImage i = edgeExtendWithAverage.Clip(clipRect))
                //        i.Save(@"C:\temp\edgeExtendWithAverage.bmp");
                //}

                //using (AlgoImage edgeExtendWithDilate = scWeightTempImage.Clone())
                //{
                //    ip.Dilate(edgeExtendWithDilate, edgeExtendWithDilate, reconOptions.EdgeSmoothCount, 3, true);

                //    using (AlgoImage i = edgeExtendWithDilate.Clip(clipRect))
                //        i.Save(@"C:\temp\edgeExtendWithDilate.bmp");
                //}

                ip.Dilate(scWeightTempImage, scWeightTempImage, reconOptions.EdgeSmoothCount, 3, true);

                // 히스토그램상 상위 10%에 해당하는 픽셀의 하한 값. - (1)
                long[] histo = ip.Histogram(scWeightTempImage);
                long histoThres = (long)(scWeightTempImage.Width * scWeightTempImage.Height * 0.9f);
                long hostoAccum = 0;
                int thresValue = 0;
                for (int i = 0; i < histo.Length; i++)
                {
                    hostoAccum += histo[i];
                    if (hostoAccum > histoThres)
                    {
                        thresValue = i;
                        break;
                    }
                }

                // (1)로 이진화하여 마스크 생성. 평균 값 계산. - (2)
                float greyAverage = 0;
                using (AlgoImage scWeightTempImage2 = scWeightImage.Clone())
                {
                    ip.Binarize(scWeightTempImage2, thresValue);
                    //scWeightTempImage2.Save(@"C:\temp\scWeightTempImage2.bmp");
                    greyAverage = ip.GetGreyAverage(scWeightTempImage, scWeightTempImage2);
                }

                float scale = reconOptions.EdgeValue / greyAverage;
                //scWeightTempImage.Save(@"C:\temp\scWeightTempImage.bmp");
                ip.Mul(scWeightTempImage, scWeightTempImage, scale);
                //scWeightTempImage.Save($@"C:\temp\scWeightTempImage_S{reconOptions.EdgeSmoothCount:00}_V{reconOptions.EdgeValue:000}.bmp");

                // 성형층 얼룩 모델링
                //using (AlgoImage scModelTempImage = scModelImage.Clone())
                //{
                //    ip.Subtract(scModelTempImage, scAlgoImage, scModelTempImage);

                //    for (int i = 0; i < 16; i++)
                //        ip.Average(scModelTempImage);
                //    ip.Add(scWeightTempImage, scModelTempImage, scWeightTempImage);
                //}

                // 성형층 전체 +
                //using (AlgoImage scModelTempImage = scModelImage.Clone())
                //{
                //    ip.Binarize(scModelTempImage);
                //    ip.Erode(scModelTempImage, 3);
                //    ip.Div(scModelTempImage, scModelTempImage, 25);
                //    ip.Add(scWeightTempImage, scModelTempImage, scWeightTempImage);
                //}

                scWeightImage.Copy(scWeightTempImage);
            }
            //scModelImage.Save(@"C:\temp\bmp\scModelImage.bmp");
            //scWeightImage.Save(@"C:\temp\bmp\scWeightImage.bmp");
            //scAlgoImage.Save(@"C:\temp\bmp\scAlgoImage.bmp");

            ip.Resize(scModelImage, modelImage);
            ip.Resize(scWeightImage, weightImage);
            //modelImage.Save(@"C:\temp\bmp\ModelImage.bmp");
            //weightImage.Save(@"C:\temp\bmp\WeightImage.bmp");

            scWeightImage.Dispose();
            scModelImage.Dispose();

            modelImageD = modelImage.ToImageD();
            weightImageD = weightImage.ToImageD();

            scAlgoImage.Dispose();
            weightImage.Dispose();
            modelImage.Dispose();
        }

        public override bool TrainRegion(AlgoImage trainImage, Model model, int start, int end, DebugContext debugContext)
        {
            model.CalculatorModelParam.RegionInfoCollection.Clear();

            int iStep = -1;
            string[] messages = new string[] { "Find Features...", "Generate Data..." };
            float step = (end - start) * 1f / messages.Length;


            //worker?.ReportProgress(start, StringManager.GetString(this.GetType().FullName, "Generate Data"));

            RCITrainResult result = model.RCITrainResult;
            bool rightToLeft = AlgorithmSetting.Instance().RCIGlobalOptions.RightToLeft;

            SpikeCollection featureX, featureY;
            using (AlgoImage reconImage = ImageBuilder.Build(trainImage.LibraryType, result.ReconImageD, ImageType.Grey))
            {
                //reconImage.Save(@"C:\temp\reconImage.bmp");

                iStep = 0;
                worker?.ReportProgress(start + (int)(step * (iStep + 1)), StringManager.GetString(this.GetType().FullName, messages[iStep]));

                featureX = GetFeature("X", reconImage, Direction.Horizontal, rightToLeft);
                using (AlgoImage yImage = reconImage.GetSubImage(Rectangle.Inflate(new Rectangle(Point.Empty, reconImage.Size), -reconImage.Width / 3, 0)))
                    featureY = GetFeature("Y", yImage, Direction.Vertical, false);

                iStep = 0;
                worker?.ReportProgress(start + (int)(step * (iStep + 1)), StringManager.GetString(this.GetType().FullName, messages[iStep]));

                SizeF inflatePx = this.Calibration.WorldToPixel(model.RCIOptions.PTM_InflateUm);
                result.Update(featureX, featureY, reconImage, Size.Round(inflatePx), rightToLeft, this.worker);

                if (this.worker.CancellationPending)
                    return false;
            }

            Array.ForEach(result.WorkPoints, f =>
            {
                bool isHead = (f.Row == 0);
                bool isTail = (f.Row == result.WorkPointRowCount - 1);
                f.Use = !(model.RCIOptions.SkipHeadRow && isHead) && !(model.RCIOptions.SkipTailRow && isTail);
            });

            result.DebugImageD = null;
            if (model.RCIOptions.BuildDebugImage)
                result.DebugImageD = BuildDebugBitmap(trainImage, result);

            return true;
        }

        private static SpikeCollection GetFeature(string name, AlgoImage algoImage, Direction direction, bool reverse)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
            float[] proj = ip.Projection(algoImage, direction, ProjectionType.Mean);
            if (reverse)
                proj = proj.Reverse().ToArray();

            float[] datas;
            using (AlgoImage projAlgoImage = ImageBuilder.Build(algoImage.LibraryType, ImageType.Depth, proj.Length, 1))
            {
                projAlgoImage.SetByte(RCIHelper.GetBytes(proj));
                ip.Average(projAlgoImage);
                ip.Sobel(projAlgoImage, projAlgoImage, Direction.Horizontal);
                datas = RCIHelper.GetSingles(projAlgoImage.GetByte());
            }

            //RCIHelper.SaveSingles(@"C:\temp\datas.txt", datas);
            datas = RCIHelper.MovingAverage(datas, 15);
            //RCIHelper.SaveSingles(@"C:\temp\datas2.txt", datas);

            Tuple<float, bool>[] features = FindSpikes(datas, 0, out float threshold).ToArray();
            return new SpikeCollection(name, datas, features);
        }

        private static IEnumerable<Tuple<float, bool>> FindSpikes(float[] datas, int margin, out float threshold)
        {
            float thresMul = 0.9f;
            float[] absDatas = datas.Select(f => Math.Abs(f)).ToArray();
            float temp = RCIHelper.AverageIf(absDatas, absDatas.Average());

            // th보다 크면 확실함.
            threshold = RCIHelper.AverageIf(absDatas, temp) * thresMul;

            return FindSpikes(datas, margin, threshold);
        }

        internal static IEnumerable<Tuple<float, bool>> FindSpikes(float[] datas, int margin, float threshold)
        {
            int minLength = 0;
            List<Tuple<float, float, bool>> featureList = new List<Tuple<float, float, bool>>();

            Func<SortedList<int, float>, Tuple<float, float, bool>> func = new Func<SortedList<int, float>, Tuple<float, float, bool>>(f =>
            {
                bool polarity = f.Sum(g => g.Value) > 0;
                float pos;
                float value;
                if (false)
                {
                    float nom = f.Sum(g => g.Key * g.Value * g.Value);
                    float denom = f.Sum(g => g.Value * g.Value);
                    pos = nom / denom;
                }
                else
                {
                    float[] abs = f.Select(g => Math.Abs(g.Value)).ToArray();
                    int idxMax = Array.IndexOf(abs, abs.Max());
                    pos = f.ElementAt(idxMax).Key;
                }
                value = f[(int)pos];
                f.Clear();

                return new Tuple<float, float, bool>(pos, value, polarity);
            });

            Action<SortedList<int, float>, SortedList<int, float>> MergeList = new Action<SortedList<int, float>, SortedList<int, float>>((to, from) =>
            {
                foreach (var v in from)
                    to.Add(v.Key, v.Value);
            });

            Action<SortedList<int, float>> AddFeature = new Action<SortedList<int, float>>(outter =>
            {
                if (outter.Count > minLength)
                {
                    Tuple<float, float, bool> tuple = func(outter);
                    featureList.Add(tuple);
                }
            });

            // th보다 작고, th의 0.9배보다 크면 애매함.
            float threshold2 = threshold * 0.8f;

            //Console.WriteLine($"threshold: {threshold}, threshold2: {threshold2}");
            //RCIHelper.SaveSingles(@"C:\temp\absDatas.txt", absDatas);

            SortedList<int, float> vagueList = new SortedList<int, float>();
            SortedList<int, float> outerList = new SortedList<int, float>();
            for (int x = 0; x < datas.Length; x++)
            {
                float data = datas[x];
                float absData = Math.Abs(data);

                bool cond1 = (outerList.Count == 0 ? false : outerList.Last().Value * data < 0); // 부호가 다르다.
                bool cond2 = (absData >= threshold); // 임계치 넘어섬.
                bool cond3 = (absData >= threshold2); // 임계치 근처.

                if (cond1)
                // 부호가 다른 경우.
                // 이전의 확실한 목록의 항목만 계산.
                {
                    AddFeature(outerList);
                    outerList.Clear();
                    vagueList.Clear();
                }

                if (cond2)
                // 확실한 경우
                {
                    if (vagueList.Count > 0)
                    // 애매한 목록에 있는 항목을 확실한 목록으로 옮김.
                    {
                        MergeList(outerList, vagueList);
                        vagueList.Clear();
                    }
                    outerList.Add(x, data);
                }
                else if (cond3)
                // 임계치에 근접한 경우
                // 애매한 목록에 추가.
                {
                    vagueList.Add(x, data);
                }
                else
                // 임계치 이하.
                // 이전의 확실한 목록의 항목만 계산.
                {
                    if (outerList.Count > 0)
                    {
                        //if (vagueList.Count > 0)
                        //{
                        //    MergeList(outerList, vagueList);
                        //    vagueList.Clear();
                        //}

                        AddFeature(outerList);
                        outerList.Clear();
                    }

                    if (vagueList.Count > 0)
                        vagueList.Clear();
                }
            }

            if (outerList.Count > minLength)
                featureList.Add(func(outerList));

            if (featureList.Count == 0)
                return new Tuple<float, bool>[0];

            int lastPos = (int)featureList.Last().Item1;
            if (lastPos >= datas.Length - 1)
                featureList.Remove(featureList.Last());

            int min = margin;
            int max = datas.Length - margin;
            featureList.RemoveAll(f => f.Item1 < min || f.Item1 > max);
            return featureList.Select(f => new Tuple<float, bool>(f.Item1, f.Item3));
        }

        public override bool TrainExtention(AlgoImage trainImage, Model model, Point baseOffset, int start, int end, DebugContext debugContext)
        {
            ObserverCollection[] observerCollections = new ObserverCollection[]
            {
                model.WatcherModelParam.MonitorChipCollection,
                model.WatcherModelParam.MonitorFPCollection,
                model.WatcherModelParam.MonitorIndexCollection
            };
            int observeCount = observerCollections.Sum(f => f.Param.Active ? f.Param.Count : 0);

            StopImgCollection stopImgCollection = model.WatcherModelParam.StopImgCollection;
            int monChipCount = stopImgCollection.Param.Active ? stopImgCollection.Param.Count : 0;

            MarginCollection marginCollection = model.WatcherModelParam.MarginCollection;
            int marginCount = marginCollection.Param.Active ? marginCollection.Param.Count : 0;

            TransformCollection transformCollection = model.WatcherModelParam.TransformCollection;
            Size transformCount = transformCollection.Param.Active ? transformCollection.Param.Count : Size.Empty;

            Array.ForEach(observerCollections, f => f.Clear());
            stopImgCollection.Clear();
            marginCollection.Clear();
            transformCollection.Clear();

            return true;
        }

        public static ImageD BuildDebugBitmap(AlgoImage algoImage, RCITrainResult result)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
            ImagingLibrary imagingLibrary = algoImage.LibraryType;

            ImageD imageD;
            using (AlgoImage debugAlgoImage = ImageBuilder.Build(imagingLibrary, ImageType.Grey, algoImage.Size))
            {
                ip.Div(algoImage, debugAlgoImage, 4);

                ip.DrawRect(debugAlgoImage, result.ROI, 127, false);
                using (AlgoImage debugRoiImage = debugAlgoImage.GetSubImage(result.ROI))
                {
                    using (AlgoImage weightRoiImage = ImageBuilder.Build(imagingLibrary, result.WeightImageD, ImageType.Grey))
                    {
                        ip.Div(weightRoiImage, weightRoiImage, 2);
                        ip.Add(debugRoiImage, weightRoiImage, debugRoiImage);
                    }

                    ip.DrawText(debugRoiImage, new Point(5, 5), 127, $"{result.ElapsedMs:F1}[ms]. {result.ROI.Location}");
                    Array.ForEach(result.WorkPoints, f =>
                    {
                        Rectangle drawRect = RCIHelper.Anchor(result.RightToLeft, debugRoiImage.Size, f.GetPrjRectangle());
                        ip.DrawRect(debugRoiImage, drawRect, 127, false);

                        ip.DrawText(debugRoiImage, Point.Add(f.Point, new Size(5, 5)), 127, $"({Array.IndexOf(result.WorkPoints, f)})");
                        ip.DrawText(debugRoiImage, Point.Add(f.Point, new Size(5, 25)), 127, $"X:{f.Column}/{f.Point.X}");
                        ip.DrawText(debugRoiImage, Point.Add(f.Point, new Size(5, 45)), 127, $"Y:{f.Row}/{f.Point.Y}");

                        ip.DrawText(debugRoiImage, Point.Add(f.Point, new Size(5, 85)), 127, $"BgV:{f.MeanBgGv:F1}");
                        ip.DrawText(debugRoiImage, Point.Add(f.Point, new Size(5, 105)), 127, $"Sc:{f.Projection.ScoreH:F1} / {f.Projection.ScoreV:F1}");

                        //using (AlgoImage debugSubImage = debugRoiImage.GetSubImage(f.Rectangle))
                        //{
                        //    ip.DrawText(debugSubImage, new Point(5, 5), 127, $"({Array.IndexOf(result.WorkRects, f)})");
                        //    ip.DrawText(debugSubImage, new Point(5, 25), 127, $"X:{f.Column}/{f.Rectangle.X}");
                        //    ip.DrawText(debugSubImage, new Point(5, 45), 127, $"Y:{f.Row}/{f.Rectangle.Y}");

                        //    ip.DrawText(debugSubImage, new Point(5, 85), 127, $"BgV:{f.MeanBgGv:F1}");
                        //    ip.DrawText(debugSubImage, new Point(5, 105), 127, $"ScH:{f.Projection.ScoreH:F1}");
                        //    ip.DrawText(debugSubImage, new Point(5, 125), 127, $"ScV:{f.Projection.ScoreV:F1}");
                        //}
                    });

                    imageD = debugRoiImage.ToImageD();
                }
            }
            return imageD;
        }

        private static void DrawDebugLine(AlgoImage algoImage, int debugScale, Rectangle roi, SpikeCollection feature, Direction direction)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

            // Features
            int waceTextMax = 8;
            int waveText = 0;
            Array.ForEach(feature.Spikes, f =>
            {
                Rectangle drawRect = Rectangle.Empty;
                Point textLoc = Point.Empty;
                Size textLocOffset = Size.Empty;

                switch (direction)
                {
                    case Direction.Horizontal:
                        // 세로줄 그리기
                        drawRect = new Rectangle((int)((f.Item1 + roi.Left) / debugScale), 0, 0, algoImage.Height);
                        textLoc = new Point(drawRect.Left, 30);
                        textLocOffset.Height = 120;
                        break;
                    case Direction.Vertical:
                        // 가로줄 그리기
                        drawRect = new Rectangle(0, (int)((f.Item1 + roi.Top) / debugScale), algoImage.Width, 0);
                        textLoc = new Point(0, drawRect.Top);
                        textLocOffset.Width = 120;
                        break;
                }

                double drawColor = (f.Item2 ? Color.Red : Color.Blue).ToArgb();
                ip.DrawRect(algoImage, drawRect, drawColor, false);

                //if (!f.Item2)
                //    textLoc.Offset(textLocOffset.Width, textLocOffset.Height);

                textLoc.Offset(textLocOffset.Width / waceTextMax * waveText, textLocOffset.Height / waceTextMax * waveText);
                waveText = (waveText + 1) % waceTextMax;

                ip.DrawText(algoImage, textLoc, drawColor, Array.IndexOf(feature.Spikes, f).ToString(), 0.5);
            });

            Rectangle[] roiLines = new Rectangle[2];

            // ROI
            switch (direction)
            {
                case Direction.Horizontal:
                    // 세로줄 그리기
                    roiLines[0] = new Rectangle((int)((roi.Left) / debugScale), 0, 1, algoImage.Height);
                    roiLines[1] = new Rectangle((int)((roi.Right) / debugScale), 0, 1, algoImage.Height);
                    break;

                case Direction.Vertical:
                    // 가로줄 그리기
                    roiLines[0] = new Rectangle(0, (int)((roi.Top) / debugScale), algoImage.Width, 0);
                    roiLines[1] = new Rectangle(0, (int)((roi.Bottom) / debugScale), algoImage.Width, 0);
                    break;
            }

            ip.DrawRect(algoImage, roiLines[0], Color.Yellow.ToArgb(), false);
            ip.DrawRect(algoImage, roiLines[1], Color.Yellow.ToArgb(), false);
        }
    }
}
