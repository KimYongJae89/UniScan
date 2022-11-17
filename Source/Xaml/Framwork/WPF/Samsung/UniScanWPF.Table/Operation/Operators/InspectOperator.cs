using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml;
using UniEye.Base.Settings;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Operation.Operators
{
    public class InspectOperator : TaskOperator
    {
        InspectOperatorSettings settings;
        public InspectOperatorSettings Settings { get => settings; }

        public InspectOperator() : base()
        {
            settings = new InspectOperatorSettings();
        }

        public override bool Initialize(ResultKey resultKey, int totalProgressSteps, CancellationTokenSource cancellationTokenSource)
        {
            totalProgressSteps *= 2;
            return base.Initialize(resultKey, totalProgressSteps, cancellationTokenSource);
        }

        public void StartInspect(OperatorResult operatorResult)
        {
            if (operatorResult.Type != ResultType.Extract)
                return;

            this.OperatorState = OperatorState.Run;

            Task task = new Task(() =>
            {
                InspectOperatorResult inspectOperatorResult = Inspect((ExtractOperatorResult)operatorResult);
                SystemManager.Instance().OperatorProcessed(inspectOperatorResult);
                this.CurProgressSteps++;
            }, cancellationTokenSource.Token);

            AddTask(task);

            task.Start();
        }

        public void StartLumpInspect(OperatorResult operatorResult)
        {
            if (operatorResult.Type != ResultType.Extract)
                return;

            this.OperatorState = OperatorState.Run;

            Task task = new Task(() =>
            {
                SystemManager.Instance().OperatorProcessed(LumpInspect((ExtractOperatorResult)operatorResult));
                this.CurProgressSteps++;
            }, cancellationTokenSource.Token);

            AddTask(task);

            task.Start();
        }

        private LumpInspectOperatorResult LumpInspect(ExtractOperatorResult extractOperatorResult)
        {
            if (resultKey.Model.IsTaught() == false)
                return new LumpInspectOperatorResult(resultKey, new Exception("Not Teached"), extractOperatorResult);

            if (extractOperatorResult.BlobRectList == null)
                return new LumpInspectOperatorResult(resultKey, new Exception("No Pattern Exist"), extractOperatorResult);

            List<IResultObject> defectList = new List<IResultObject>();
            if (this.settings.UseLumpDetect)
            {
                List<Defect> lumpDefectList = new List<Defect>();
                PatternInspect(extractOperatorResult, true, lumpDefectList);
                defectList.AddRange(lumpDefectList.ToArray());
            }
            return new LumpInspectOperatorResult(resultKey, extractOperatorResult, defectList);
        }

        private InspectOperatorResult Inspect(ExtractOperatorResult extractOperatorResult)
        {
            if (resultKey.Model.IsTaught() == false)
                return new InspectOperatorResult(resultKey, new Exception("Not Teached"), extractOperatorResult);

            List<IResultObject> defectList = new List<IResultObject>();
            if (extractOperatorResult.BlobRectList == null)
                return new InspectOperatorResult(resultKey, new Exception("No Pattern Exist"), extractOperatorResult);

            List<Defect> patternDefectList = new List<Defect>();
            List<MarginDefect> marginDefectList = new List<MarginDefect>();
            List<ShapeDefect> shapeDefectList = new List<ShapeDefect>();
            List<LengthMeasure> lengthMeasureList = new List<LengthMeasure>();
            List<MeanderMeasure> meanderMeasureList = new List<MeanderMeasure>();

            List<Task> subTaskList = new List<Task>();
            subTaskList.Add(Task.Factory.StartNew(() => { PatternInspect(extractOperatorResult, false, patternDefectList); }, cancellationTokenSource.Token));

            subTaskList.Add(Task.Factory.StartNew(() => { MarginInspect(extractOperatorResult, marginDefectList); }, cancellationTokenSource.Token));
            subTaskList.Add(Task.Factory.StartNew(() => { HeightMeasure(extractOperatorResult, lengthMeasureList); }, cancellationTokenSource.Token));

            if (settings.UseShapeInspect)
                subTaskList.Add(Task.Factory.StartNew(() => { ShapeInspect(extractOperatorResult, shapeDefectList); }, cancellationTokenSource.Token));

            Task.WaitAll(subTaskList.ToArray());

            if (cancellationTokenSource.IsCancellationRequested == true)
                return new InspectOperatorResult(resultKey, new Exception("Cancelled"), extractOperatorResult);

            System.Diagnostics.Debug.WriteLine(string.Format("Inspect {0}, Done", extractOperatorResult.ScanOperatorResult.FlowPosition));

            defectList.AddRange(patternDefectList.ToArray());
            defectList.AddRange(marginDefectList.ToArray());
            defectList.AddRange(shapeDefectList.ToArray());
            defectList.AddRange(lengthMeasureList.ToArray());
            defectList.AddRange(meanderMeasureList.ToArray());
            return new InspectOperatorResult(resultKey, extractOperatorResult, defectList);
        }

        private void ShapeInspect(ExtractOperatorResult extractOperatorResult, List<ShapeDefect> shapeDefectList)
        {
            AlgoImage algoImage = extractOperatorResult.ScanOperatorResult.TopLightImage;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            Rectangle sourceRect = new Rectangle(0, 0, algoImage.Width, algoImage.Height);

            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.CancellationToken = this.cancellationTokenSource.Token;
#if DEBUG
            parallelOptions.MaxDegreeOfParallelism = 1;
#endif
            List<BlobRect> refList = new List<BlobRect>();
            List<ShapeDefect> defectList = new List<ShapeDefect>();
            Parallel.ForEach(extractOperatorResult.BlobRectList, parallelOptions, blobRect =>
            {
                List<Tuple<PatternFeature, float>> minDiffList = null;
                float minDiffSumValue = float.MaxValue;

                foreach (PatternGroup patternGroup in resultKey.Model.InspectPatternList)
                {
                    List<Tuple<PatternFeature, float>> diffList = new List<Tuple<PatternFeature, float>>();
                    foreach (PatternFeature feature in Enum.GetValues(typeof(PatternFeature)))
                        diffList.Add(new Tuple<PatternFeature, float>(feature, patternGroup.GetDiffValue(feature, blobRect)));

                    float sumValue = diffList.Sum(diff => diff.Item2);
                    if (sumValue < minDiffSumValue)
                    {
                        minDiffSumValue = sumValue;
                        minDiffList = diffList;
                    }

                    if (Math.Abs(diffList.Max(diff => diff.Item2)) < Math.Max(1, settings.DiffThreshold / DeveloperSettings.Instance.Resolution))
                    {
                        lock (refList)
                            refList.Add(blobRect);

                        return;
                    }

                    if (this.cancellationTokenSource.IsCancellationRequested)
                        return;
                }

                foreach (PatternGroup patternGroup in resultKey.Model.CandidatePatternList)
                {
                    List<Tuple<PatternFeature, float>> diffList = new List<Tuple<PatternFeature, float>>();
                    foreach (PatternFeature feature in Enum.GetValues(typeof(PatternFeature)))
                        diffList.Add(new Tuple<PatternFeature, float>(feature, patternGroup.GetDiffValue(feature, blobRect)));

                    float sumValue = diffList.Sum(diff => diff.Item2);
                    if (sumValue < minDiffSumValue)
                    {
                        minDiffSumValue = sumValue;
                        minDiffList = diffList;
                    }

                    if (Math.Abs(diffList.Max(diff => diff.Item2)) < Math.Max(1, settings.DiffThreshold / DeveloperSettings.Instance.Resolution))
                        return;
                    if (this.cancellationTokenSource.IsCancellationRequested)
                        return;
                }

                RectangleF imageRect = DrawingHelper.Mul(blobRect.BoundingRect, 1 / extractOperatorResult.BlobScaleFactor);
                imageRect.Inflate(settings.DefectImageInflatePx, settings.DefectImageInflatePx);
                imageRect.Intersect(sourceRect);

                AlgoImage defectImage = algoImage.GetSubImage(Rectangle.Round(imageRect));
                lock (defectList)
                    defectList.Add(new ShapeDefect(blobRect, defectImage.ToBitmapSource(), minDiffList.Max(f => f.Item2)));

                defectImage.Dispose();
            });

            shapeDefectList.AddRange(defectList.ToArray());
        }

        private float ShapeInspect2(BlobRect blobRect, ObservableCollection<PatternGroup> inspectPatternList)
        {
            float[] maxDiffs = inspectPatternList.Select(f =>
            {
                List<Tuple<PatternFeature, float>> diffList = new List<Tuple<PatternFeature, float>>();
                foreach (PatternFeature feature in Enum.GetValues(typeof(PatternFeature)))
                    diffList.Add(new Tuple<PatternFeature, float>(feature, f.GetDiffValue(feature, blobRect)));
                diffList.RemoveAll(g => g.Item1 == PatternFeature.Area);

                return diffList.Max(g => g.Item2);
            }).ToArray();

            return maxDiffs.Min();
        }

        private void PatternInspect(ExtractOperatorResult extractOperatorResult, bool isLumpDetect, List<Defect> commonDefectList)
        {
            //AlgoImage patternInspMask = BufferManager.Instance().GetInspectBuffer();
            //AlgoImage patternInspBuffer = BufferManager.Instance().GetInspectBuffer();
            //AlgoImage patternInspBuffer2 = (isLumpDetect ? BufferManager.Instance().GetInspectBuffer() : null);

            AlgoImage[] buffers = BufferManager.Instance().GetInspectBuffer(isLumpDetect ? 3 : 2, "PatternInspect");
            AlgoImage patternInspMask = buffers[0];
            AlgoImage patternInspBuffer = buffers[1];
            AlgoImage patternInspBuffer2 = null;
            if (isLumpDetect)
                patternInspBuffer2 = buffers[2];

            AlgoImage algoImage = extractOperatorResult.ScanOperatorResult.TopLightImage;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            AlgoImage maskBuffer = extractOperatorResult.MaskBuffer;
            AlgoImage sheetBuffer = extractOperatorResult.SheetBuffer;
            Rectangle sheetRect = extractOperatorResult.SheetRect;

            if (isLumpDetect)
            // 럼프 검출 모드이면 전극 내부만 자세히 봄...
            {
                //maskBuffer.Save(@"C:\temp\maskBuffer.bmp");
                imageProcessing.Close(maskBuffer, 1);
                //maskBuffer.Save(@"C:\temp\maskBuffer1.bmp");
            }

            // 전극-성형 경계 축소(과검)
            float patternIgnoreRangeLength = settings.PatternIgnoreRangeLength / 5;
            if (patternIgnoreRangeLength < 0)
                patternIgnoreRangeLength = DeveloperSettings.Instance.Resolution * 5;
            imageProcessing.Erode(maskBuffer, patternInspMask, (int)(patternIgnoreRangeLength / DeveloperSettings.Instance.Resolution));

            Rectangle sourceRect = new Rectangle(0, 0, algoImage.Width, algoImage.Height);

            List<Tuple<float, Rectangle>> regionList = new List<Tuple<float, Rectangle>>();
            float diffLMin = isLumpDetect ? settings.LumpLowerMin : settings.PatternLower;
            float diffLMax = isLumpDetect ? settings.LumpLowerMax : byte.MinValue;
            float diffUMin = isLumpDetect ? settings.LumpUpperMin : settings.PatternUpper;
            float diffUMax = isLumpDetect ? settings.LumpUpperMax : byte.MaxValue;

            int left = 0;
            int right = algoImage.Width;
            int regionNum = (int)Math.Ceiling((right - left) * 1.0 / settings.InspectRegionWidthPx);
            for (int i = 0; i < regionNum; i++)
            {
                Rectangle subRect = new Rectangle(i * settings.InspectRegionWidthPx, 0, settings.InspectRegionWidthPx, algoImage.Height);
                int offsetX = -Math.Max(0, subRect.Right - right);
                subRect.Offset(offsetX, 0);

                AlgoImage subImage = algoImage.GetSubImage(subRect);
                AlgoImage maskSubImage = patternInspMask.GetSubImage(subRect);
                AlgoImage binSubImage = patternInspBuffer.GetSubImage(subRect);
                AlgoImage binSubImage2 = patternInspBuffer2?.GetSubImage(subRect);

                Size saveSize = new Size(subImage.Width, subImage.Height / 20);
                Point saveLoc = new Point(0, sheetRect.Top);
                Rectangle saveRectangle = Rectangle.Intersect(new Rectangle(saveLoc, saveSize), new Rectangle(Point.Empty, subRect.Size));

                //SaveImage("subImage.bmp", subImage, saveRectangle);
                //SaveImage("maskSubImage.bmp", maskSubImage, saveRectangle);

                int avg = (int)Math.Round(imageProcessing.GetGreyAverage(subImage, maskSubImage));
                if (avg != 0)
                {
                    if (!isLumpDetect)
                    {
                        int thL = (int)Math.Max(byte.MinValue, avg - settings.PatternLower);
                        int thH = (int)Math.Min(byte.MaxValue, avg + settings.PatternUpper);
                        imageProcessing.Binarize(subImage, binSubImage, thL, thH, true);
                    }
                    else
                    {
                        int thLMin = (int)Math.Max(byte.MinValue, avg - settings.LumpLowerMin);
                        int thLMax = (int)Math.Min(byte.MaxValue, avg - settings.LumpLowerMax);
                        imageProcessing.Binarize(subImage, binSubImage, thLMax, thLMin);

                        int thUMin = (int)Math.Max(byte.MinValue, avg + settings.LumpUpperMin);
                        int thUMax = (int)Math.Min(byte.MaxValue, avg + settings.LumpUpperMax);
                        imageProcessing.Binarize(subImage, binSubImage2, thUMin, thUMax);

                        imageProcessing.Or(binSubImage, binSubImage2, binSubImage);
                    }
                    imageProcessing.And(binSubImage, maskSubImage, binSubImage);
                    //SaveImage("binSubImage1.bmp", binSubImage, saveRectangle);

                    //if (settings.UseLumpDetect)
                    //{
                    //    AlgoImage patternInspBuffer2 = BufferManager.Instance().GetInspectBuffer();
                    //    using (AlgoImage binSubImage2 = patternInspBuffer2.GetSubImage(subRect))
                    //    {
                    //        float[] proj = imageProcessing.Projection(subImage, maskSubImage, Direction.Vertical, ProjectionType.Mean);
                    //        byte[] bytess = Array.ConvertAll(proj, f => (byte)Math.Round(f));
                    //        using (AlgoImage algoImage1D = ImageBuilder.Build(binSubImage2.LibraryType, ImageType.Grey, 1, proj.Length))
                    //        {
                    //            algoImage1D.PutByte(bytess);
                    //            imageProcessing.Resize(algoImage1D, binSubImage2);
                    //            //SaveImage("temp.bmp", binSubImage2, saveRectangle);
                    //        }

                    //        imageProcessing.Subtract(subImage, binSubImage2, binSubImage2, true);
                    //        //SaveImage("test.bmp", binSubImage2, saveRectangle);
                    //        imageProcessing.And(binSubImage2, maskSubImage, binSubImage2);
                    //        //SaveImage("test2.bmp", binSubImage2, saveRectangle);
                    //        imageProcessing.Binarize(binSubImage2, binSubImage2, 10, false);
                    //        //SaveImage("binSubImage2.bmp", binSubImage2, saveRectangle);

                    //        imageProcessing.XOr(binSubImage, binSubImage2, binSubImage);
                    //        imageProcessing.Close(binSubImage, 2);
                    //        //SaveImage("binSubImage2.bmp", binSubImage, saveRectangle);
                    //    }
                    //    BufferManager.Instance().PutInspectBuffer(patternInspBuffer2);
                    //}             
                    regionList.Add(new Tuple<float, Rectangle>(avg, subRect));
                }

                binSubImage2?.Dispose();
                binSubImage.Dispose();
                maskSubImage.Dispose();
                subImage.Dispose();
            }

            imageProcessing.And(patternInspBuffer, patternInspMask, patternInspBuffer);
            imageProcessing.And(patternInspBuffer, sheetBuffer, patternInspBuffer);
            //patternInspBuffer.Save(@"C:\temp\patternInspBuffer.bmp");

            if (patternInspBuffer2 != null)
                BufferManager.Instance().PutInspectBuffer(patternInspBuffer2);
            BufferManager.Instance().PutInspectBuffer(patternInspMask);

            BlobParam blobParam = new BlobParam();
            blobParam.MaxCount = 0;
            blobParam.SelectArea = true;
            blobParam.SelectRotateRect = true;
            blobParam.SelectGrayMeanValue = true;
            blobParam.SelectGrayMinValue = true;
            blobParam.SelectGrayMaxValue = true;
            blobParam.EraseBorderBlobs = true;
            blobParam.SelectBoundingRect = true;
            blobParam.SelectCompactness = true;
            if (isLumpDetect)
            {
                blobParam.RotateWidthMin = settings.LumpMinDefectSize / DeveloperSettings.Instance.Resolution;
                if (settings.UseLumpMaxDefectSize)
                    blobParam.RotateWidthMax = settings.LumpMaxDefectSize / DeveloperSettings.Instance.Resolution;
            }
            else
            {
                blobParam.RotateWidthMin = settings.PatternMinDefectSize / DeveloperSettings.Instance.Resolution;

                if (settings.UsePatternMaxDefectSize)
                    blobParam.RotateWidthMax = settings.PatternMaxDefectSize / DeveloperSettings.Instance.Resolution;
            }


            //BlobRectList blobRectList = imageProcessing.Blob(patternInspBuffer, blobParam, algoImage);
            BlobRectList blobRectList = Blob(patternInspBuffer, blobParam, algoImage);
            BufferManager.Instance().PutInspectBuffer(patternInspBuffer);

            List<BlobRect> list = blobRectList.GetList();
            list.Sort((f, g) => g.RotateWidth.CompareTo(f.RotateWidth));

            int maxLen = settings.MaxDefectCount;
            if (maxLen < 0)
                maxLen = int.MaxValue;

            List<Defect> pdList = new List<Defect>();
            foreach (BlobRect blobRect in list)
            {
                List<Tuple<float, Rectangle>> foundList = regionList.FindAll(region => region.Item2.IntersectsWith(Rectangle.Round(blobRect.BoundingRect)));
                if (foundList.Count == 0)
                    continue;

                float avg = foundList.Average(f => f.Item1);

                Rectangle rect = Rectangle.Round(blobRect.BoundingRect);
                rect.Inflate(settings.DefectImageInflatePx, settings.DefectImageInflatePx);
                rect.Intersect(sourceRect);

                float defectValue = blobRect.MeanValue - avg;
                float compactness = blobRect.Compactness;
                bool isDefect = ((-diffLMax <= defectValue) && (defectValue <= -diffLMin)) || ((diffLMin <= defectValue) && (defectValue <= diffUMax));
                //isDefect = true;
                if (isDefect)
                {
                    using (AlgoImage defectImage = algoImage.GetSubImage(Rectangle.Truncate(rect)))
                    {
                        Defect defect;
                        if (isLumpDetect)
                            defect = new LumpDefect(blobRect, defectImage.ToBitmapSource(), defectValue, compactness);
                        else
                            defect = new PatternDefect(blobRect, defectImage.ToBitmapSource(), defectValue, compactness);

                        lock (pdList)
                            pdList.Add(defect);
                    }
                }
                else
                {
                    //patternBuffer.Save(@"d:\temp\patternBuffer.bmp");
                    //patternMask.Save(@"d:\temp\patternMask.bmp");
                    //algoImage.Save(@"d:\temp\algoImage.bmp");
                    //lock (pdList)
                    //    pdList.Add(new PatternDefect(blobRect, defectImage.ToBitmapSource(), defectValue, compactness));
                }

                if (pdList.Count >= maxLen)
                    break;

                if (this.cancellationTokenSource.IsCancellationRequested)
                    break;
            }

            BufferManager.Instance().AddDispoableObj(blobRectList);
            commonDefectList.AddRange(pdList.ToArray());
        }

        private void SaveImage(string v, AlgoImage algoImage, Rectangle rectangle)
        {
            using (AlgoImage saveImage = algoImage.GetSubImage(rectangle))
                saveImage.Save(Path.Combine(@"C:\temp", v));
        }

        private void MarginInspect(ExtractOperatorResult extractOperatorResult, List<MarginDefect> commonDefectList)
        {
            string debugContextSubPath = string.Format("InspectorOperator_MarginInspect_{0}", extractOperatorResult.ScanOperatorResult.FlowPosition);
            DebugContext debugContext = this.GetDebugContext(debugContextSubPath);

            //AlgoImage marginInspMask = BufferManager.Instance().GetInspectBuffer();
            //AlgoImage marginInspBuffer = BufferManager.Instance().GetInspectBuffer();
            AlgoImage[] buffers = BufferManager.Instance().GetInspectBuffer(2, "MarginInspect");
            AlgoImage marginInspMask = buffers[0];
            AlgoImage marginInspBuffer = buffers[1];

            AlgoImage topImage = extractOperatorResult.ScanOperatorResult.TopLightImage;
            AlgoImage backImage = extractOperatorResult.ScanOperatorResult.BackLightImage;
            AlgoImage maskBuffer = extractOperatorResult.MaskBuffer;
            AlgoImage sheetBuffer = extractOperatorResult.SheetBuffer;

            topImage.Save("topImage.bmp", debugImageScale, debugContext);
            backImage.Save("backImage.bmp", debugImageScale, debugContext);
            maskBuffer.Save("maskBuffer.bmp", debugImageScale, debugContext);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(backImage);

            Rectangle sourceRect = new Rectangle(0, 0, backImage.Width, backImage.Height);

            // 테두리를 제거하여 패턴이 잘려서 그랩된 오류 제거
            marginInspMask.Clear(255);
            Rectangle validRect = new Rectangle(Point.Empty, topImage.Size);
            int invalidLengthPx = (int)Math.Round(SystemManager.Instance().OperatorManager.ScanOperator.Settings.OverlapUm / 2 / DeveloperSettings.Instance.Resolution);
            validRect.Inflate(-invalidLengthPx, 0);
            marginInspMask.Copy(maskBuffer, validRect.Location, validRect.Location, validRect.Size);
            marginInspMask.Save("marginBuffer0.bmp", debugImageScale, debugContext);

            // 패턴 마스크를 NOT하여 마진 마스크 생성
            imageProcessing.Not(marginInspMask, marginInspMask);
            marginInspMask.Save("marginMask1.bmp", debugImageScale, debugContext);
            //if (extractOperatorResult.ScanOperatorResult.FlowPosition == 3)
            //    marginInspMask.Save("marginMask1.bmp", 0.5f, new DebugContext(true, @"C:\temp"));

            // Erode 하여 마스크에 여유분 생성. 시트영역 AND
            float marginIgnoreRangeLength = settings.MarginIgnoreRangeLength;
            if (marginIgnoreRangeLength < 0)
                marginIgnoreRangeLength = DeveloperSettings.Instance.Resolution * 5;
            imageProcessing.Erode(marginInspMask, (int)(marginIgnoreRangeLength / DeveloperSettings.Instance.Resolution));
            marginInspMask.Save("marginMask2.bmp", debugImageScale, debugContext);
            imageProcessing.And(marginInspMask, sheetBuffer, marginInspMask);
            marginInspMask.Save("marginMask3.bmp", debugImageScale, debugContext);
            //if (extractOperatorResult.ScanOperatorResult.FlowPosition == 3)
            //marginInspMask.Save(string.Format("MarginMask_{0}.bmp",extractOperatorResult.ScanOperatorResult.FlowPosition), 0.5f, new DebugContext(true, @"C:\temp"));
            //    marginInspMask.Save("marginInspMask.bmp", 0.5f, new DebugContext(true, @"C:\temp"));

            // 이거는 뭐지??
            // 짤린 전극....
            //int binarizeValue = SystemManager.Instance().CurrentModel.BinarizeValueTop;
            //imageProcessing.Binarize(topImage, marginInspBuffer, binarizeValue);
            //marginInspBuffer.Save("marginBuffer1.bmp", debugImageScale, debugContext);
            //extractOperatorResult.SheetBuffer.Save("SheetBuffer.bmp", debugImageScale, debugContext);
            //imageProcessing.And(marginInspBuffer, extractOperatorResult.SheetBuffer, marginInspBuffer);
            //marginInspBuffer.Save("marginBuffer2.bmp", debugImageScale, debugContext);
            //imageProcessing.Erode(marginInspBuffer, marginInspBuffer, (int)(marginIgnoreRangeLength / DeveloperSettings.Instance.Resolution));
            //marginInspBuffer.Save("marginBuffer3.bmp", debugImageScale, debugContext);

            //float overallMarginAvg = imageProcessing.GetGreyAverage(topImage, marginInspBuffer);
            //float limit = overallMarginAvg * 0.7f;
            List<Tuple<float, Rectangle>> regionList = new List<Tuple<float, Rectangle>>();

            //if (extractOperatorResult.ScanOperatorResult.FlowPosition == 3)
            //    topImage.Save("topImage.bmp", 0.5f, new DebugContext(true, @"C:\temp"));

            marginInspBuffer.Clear();
            int src = invalidLengthPx;
            int dst = topImage.Width - invalidLengthPx;
            int regionNum = (int)Math.Ceiling((dst - src) * 1.0 / settings.InspectRegionWidthPx);
            for (int i = 0; i < regionNum; i++)
            {
                Rectangle subRect = new Rectangle(src + i * settings.InspectRegionWidthPx, 0, settings.InspectRegionWidthPx, topImage.Height);
                subRect.Offset(-Math.Max(0, subRect.Right - dst), 0);

                AlgoImage subImage = topImage.GetSubImage(subRect);
                AlgoImage maskSubImage = marginInspMask.GetSubImage(subRect);
                AlgoImage binSubImage = marginInspBuffer.GetSubImage(subRect);

                int avg = (int)Math.Round(imageProcessing.GetGreyAverage(subImage, maskSubImage));
                if (avg > 0)
                {
                    //System.Diagnostics.Debug.WriteLine(string.Format("MarginInspect: Subrect Avg: {0}", avg));
                    imageProcessing.Average(subImage, binSubImage);
                    imageProcessing.Binarize(binSubImage, binSubImage, (int)Math.Max(0, avg - settings.MarginLower), (int)Math.Min(255, avg + settings.MarginUpper), true);
                    binSubImage.Save("binSubImage.bmp", debugImageScale, debugContext);
                    regionList.Add(new Tuple<float, Rectangle>(avg, subRect));

                    //subImage.Save("subImage.bmp", 0.5f, new DebugContext(true, @"C:\temp"));
                    //maskSubImage.Save("maskSubImage.bmp", 0.5f, new DebugContext(true, @"C:\temp"));
                    //binSubImage.Save("binSubImage.bmp", 0.5f, new DebugContext(true, @"C:\temp"));
                }

                subImage.Dispose();
                maskSubImage.Dispose();
                binSubImage.Dispose();

                //if (extractOperatorResult.ScanOperatorResult.FlowPosition == 3)
                //    marginInspBuffer.Save("marginInspBuffer.bmp", 0.5f, new DebugContext(true, @"C:\temp"));
            }
            marginInspBuffer.Save("marginBuffer4.bmp", debugImageScale, debugContext);
            //if (extractOperatorResult.ScanOperatorResult.FlowPosition == 3)
            //    marginInspBuffer.Save("marginInspBuffer.bmp", 0.5f, new DebugContext(true, @"C:\temp"));

            imageProcessing.And(marginInspBuffer, marginInspMask, marginInspBuffer);
            //imageProcessing.And(marginInspBuffer, sheetBuffer, marginInspBuffer);
            marginInspBuffer.Save("marginBuffer5.bmp", debugImageScale, debugContext);
            //if (extractOperatorResult.ScanOperatorResult.FlowPosition == 3)
            //    marginInspBuffer.Save("marginInspBufferAnd.bmp", 0.5f, new DebugContext(true, @"C:\temp"));
            //marginInspBuffer.Save(string.Format("marginInspBuffer_{0}.bmp", extractOperatorResult.ScanOperatorResult.FlowPosition), 0.5f, new DebugContext(true, @"C:\temp"));

            BlobParam blobParam = new BlobParam();
            blobParam.MaxCount = 0;
            blobParam.SelectArea = true;
            blobParam.SelectCenterPt = true;
            blobParam.SelectRotateRect = true;
            blobParam.SelectGrayMeanValue = true;
            blobParam.SelectGrayMinValue = true;
            blobParam.SelectGrayMaxValue = true;
            blobParam.EraseBorderBlobs = true;
            blobParam.SelectBoundingRect = true;
            blobParam.SelectCompactness = true;
            blobParam.RotateWidthMin = settings.MarginMinDefectSize / DeveloperSettings.Instance.Resolution;
            if (settings.UseMarginMaxDefectSize)
                blobParam.RotateWidthMax = settings.MarginMaxDefectSize / DeveloperSettings.Instance.Resolution;

            //BlobRectList blobRectList = imageProcessing.Blob(marginInspBuffer, blobParam, topImage);
            BlobRectList blobRectList = Blob(marginInspBuffer, blobParam, topImage);
            BlobRect[] blobRects = blobRectList.GetArray();

            List<MarginDefect> mdList = new List<MarginDefect>();
            foreach (BlobRect blobRect in blobRects)
            {
                //if (RectangleF.Intersect(extractOperatorResult.SheetRect, blobRect.BoundingRect) != blobRect.BoundingRect)
                //    continue;

                List<Tuple<float, Rectangle>> foundList = regionList.FindAll(region => region.Item2.IntersectsWith(Rectangle.Round(blobRect.BoundingRect)));
                if (foundList.Count == 0)
                    continue;

                float avg = foundList.Average(f => f.Item1);

                Rectangle rect = Rectangle.Round(blobRect.BoundingRect);
                Debug.Assert(rect.Width >= 0 && rect.Height >= 0);

                rect.Inflate(settings.DefectImageInflatePx, settings.DefectImageInflatePx);
                rect.Intersect(sourceRect);

                AlgoImage defectImage = topImage.GetSubImage(rect);
                float compactness = blobRect.Compactness;
                float diff = blobRect.MeanValue - avg;
                if (diff > settings.MarginUpper || diff < -settings.MarginLower)
                {
                    lock (mdList)
                        //mdList.Add(new MarginDefect(blobRect, defectImage.ToBitmapSource(), blobRect.MeanValue < avg ? blobRect.MaxValue - avg : blobRect.MinValue - avg));
                        mdList.Add(new MarginDefect(blobRect, defectImage.ToBitmapSource(), diff, compactness));
                }
                else
                {

                }

                defectImage.Dispose();

                if (this.cancellationTokenSource.IsCancellationRequested)
                    break;
            }

            BufferManager.Instance().AddDispoableObj(blobRectList);
            BufferManager.Instance().PutInspectBuffer(marginInspMask);
            BufferManager.Instance().PutInspectBuffer(marginInspBuffer);

            commonDefectList.AddRange(mdList.ToArray());
        }

        private void HeightMeasure(ExtractOperatorResult extractOperatorResult, List<LengthMeasure> lengthMeasureList)
        {
            float resolution = DeveloperSettings.Instance.Resolution;
            Rectangle sheetRect = extractOperatorResult.SheetRect;
            Point srcPt = new Point((sheetRect.Left + sheetRect.Right) / 2, sheetRect.Top);
            Point dstPt = new Point((sheetRect.Left + sheetRect.Right) / 2, sheetRect.Bottom);
            double value = sheetRect.Height * resolution / 1000;

            if (value > 0)
                lengthMeasureList.Add(new LengthMeasure(Direction.Vertical, srcPt, dstPt, value, false));
        }

        private void MeanderMeasure(ExtractOperatorResult extractOperatorResult, List<MeanderMeasure> meanderMeasureList)
        {
            string debugContextSubPath = string.Format("InspectorOperator_MeanderMeasure_{0}", extractOperatorResult.ScanOperatorResult.FlowPosition);
            DebugContext debugContext = this.GetDebugContext(debugContextSubPath);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (extractOperatorResult.ScanOperatorResult.FlowPosition == 0)
            {
                AlgoImage backLightImage = extractOperatorResult.ScanOperatorResult.BackLightImage;
                Rectangle sheetRect = extractOperatorResult.SheetRect;
                if (sheetRect.Width == 0 || sheetRect.Height == 0)
                    return;

                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(backLightImage);
                AlgoImage clipImage = backLightImage.Clip(Rectangle.FromLTRB(0, sheetRect.Top, backLightImage.Width, sheetRect.Bottom));
                imageProcessing.Average(clipImage);
                imageProcessing.Average(clipImage);
                clipImage.Save("Measure.Average.bmp", debugImageScale, debugContext);

                int subDataSize = clipImage.Height / 3;
                int subDataSrc = 0;
                int subDataDst = subDataSize;
                for (int idxArea = 0; idxArea < 3; idxArea++)
                {
                    AlgoImage subImage = clipImage.GetSubImage(new Rectangle(0, subDataSrc, clipImage.Width, subDataSize));
                    string fileName = string.Format(@"Measure.Average.Sub{0}", idxArea);
                    subImage.Save(string.Format(@"{0}.bmp", fileName), debugImageScale, debugContext);
                    float[] subData = imageProcessing.Projection(subImage, DynMvp.Vision.Direction.Horizontal, ProjectionType.Mean);
                    subImage.Dispose();

                    int fallSrc = -1, fallDst = -1, fallMaxPos = -1;
                    float fallAcc = 0, fallMaxVal = 0;
                    int upCount = -1;
                    List<Tuple<int, int, int, float>> fallList = new List<Tuple<int, int, int, float>>();
                    for (int i = 1; i < subData.Length; i++)
                    {
                        float diff = subData[i] - subData[i - 1];
                        if (diff < 0) // Projection is falling 
                        {
                            upCount = 0;

                            if (fallSrc < 0)
                                fallSrc = i - 1;
                            fallDst = i;
                            fallAcc += diff;
                            if (diff < fallMaxVal)
                            {
                                fallMaxPos = i;
                                fallMaxVal = diff;
                            }
                        }
                        else  // Projection is rising or horizen 
                        {
                            upCount++;

                            if (upCount > (settings.MeanderSensitivity - 100) + 5)
                            {
                                if (fallSrc >= 0 && fallAcc <= (settings.MeanderSensitivity - 100) * 2.55f)
                                    fallList.Add(new Tuple<int, int, int, float>(fallSrc, fallDst, fallMaxPos, fallAcc));
                                fallSrc = fallDst = fallMaxPos = -1;
                                fallAcc = fallMaxVal = 0;
                                upCount = 0;
                            }
                        }
                    }
                    //fallList.Sort(Comp);
                    if (fallList.Count >= 3)
                    {
                        System.Diagnostics.Debug.WriteLine(fallList[0].ToString());
                        System.Diagnostics.Debug.WriteLine(fallList[1].ToString());
                        System.Diagnostics.Debug.WriteLine(fallList[2].ToString());

                        //int[] lines = new int[] { fallList[0].Item3, fallList[1].Item3, fallList[2].Item3 };
                        int[] lines = new int[] {
                            (fallList[0].Item1+ fallList[0].Item2)/2,
                            (fallList[1].Item1+ fallList[1].Item2)/2,
                            (fallList[2].Item1+ fallList[2].Item2)/2,
                           };
                        meanderMeasureList.Add(new MeanderMeasure(sheetRect.Top + ((subDataDst + subDataSrc) / 2), lines[0], lines[1], lines[2]));
                        if (debugContext.SaveDebugImage)
                            System.IO.File.WriteAllText(System.IO.Path.Combine(debugContext.FullPath, string.Format("{0}.txt", fileName)), string.Format("{0} - {1} - {2}", lines[0], lines[1], lines[2]));
                    }
                    subDataSrc = subDataDst;
                    subDataDst += subDataSize;
                }
                clipImage.Dispose();
            }

            //Debug.WriteLine(string.Format("MeanderMeasure {0}", stopwatch.Elapsed));
        }

        public void Measure()
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectOperator::Measure Start");
            OperatorState = OperatorState.Run;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                List<IResultObject> resultObjectList = new List<IResultObject>();

                ResultCombiner resultCombiner = SystemManager.Instance().OperatorManager.ResultCombiner;

                // filter height length measure data
                {
                    List<CanvasDefect> canvasDefectList = null;
                    lock (resultCombiner.CombineDefectList)
                    {
                        canvasDefectList = resultCombiner.CombineDefectList.ToList();
                    }
                    List<LengthMeasure> lengthMeasureList = canvasDefectList.FindAll(f => f.Defect is LengthMeasure).ConvertAll(f => (LengthMeasure)f.Defect);
                    lengthMeasureList.ForEach(f => f.IsValid = false);

                    // Update Height
                    List<LengthMeasure> verticalList = lengthMeasureList.FindAll(f => f.Direction == Direction.Vertical && f.LengthMm > 0);
                    if (verticalList.Count > 0)
                    {
                        int indexSrc = 0;
                        int indexDst = verticalList.Count - 1;
                        int indexMid = (indexSrc + indexDst) / 2;

                        if (indexSrc >= 0)
                            verticalList[indexSrc].IsValid = true;

                        if (indexMid >= 0)
                            verticalList[indexMid].IsValid = true;

                        if (indexDst >= 0)
                            verticalList[indexDst].IsValid = true;
                    }
                }

                ExtractOperatorResult extractOperatorResult = resultCombiner.ExtractOperatorResultArray.LastOrDefault() as ExtractOperatorResult;

                List<LengthMeasure> widthMeasureList = new List<LengthMeasure>();
                WidthMeasure(resultCombiner, widthMeasureList);
                resultObjectList.AddRange(widthMeasureList);

                List<ExtraMeasure> marginMeasure = new List<ExtraMeasure>();
                MarginMeasure(resultCombiner, marginMeasure);

                resultObjectList.AddRange(marginMeasure);

                resultCombiner.AddResult(new InspectOperatorResult(null, null, resultObjectList));
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, $"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }

            OperatorState = OperatorState.Idle;

            Debug.WriteLine(string.Format("Inspect2 {0}", stopwatch.Elapsed));
            LogHelper.Debug(LoggerType.Inspection, "InspectOperator::Measure End");
        }

        private void WidthMeasure(ResultCombiner resultCombiner, List<LengthMeasure> lengthMeasureList)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            DebugContext debugContext = this.GetDebugContext("InspectorOperator_WidthMeasure");
            float res = DeveloperSettings.Instance.Resolution;
            List<Point[]> vertexPointList = new List<Point[]>();
            ExtractOperatorResult[] operatorResults = Array.FindAll(resultCombiner.ExtractOperatorResultArray, f => f != null);
            if (operatorResults.Length == 0)
                return;

            ExtractOperatorResult srcResult = operatorResults.FirstOrDefault(f => f.VertexPoints.Length == 4);
            ExtractOperatorResult dstResult = operatorResults.LastOrDefault(f => f?.VertexPoints.Length == 4);
            if (srcResult == null || dstResult == null)
                return;

            PointF[] src = new PointF[3] { srcResult.VertexPoints[0], srcResult.VertexPoints[3], Point.Empty };
            PointF[] dst = new PointF[3] { dstResult.VertexPoints[1], dstResult.VertexPoints[2], Point.Empty };
            src[2] = DrawingHelper.CenterPoint(src[0], src[1]);
            dst[2] = DrawingHelper.CenterPoint(dst[0], dst[1]);

            for (int i = 0; i < 3; i++)
            {
                PointF calcSrc = new PointF(srcResult.ScanOperatorResult.AxisPosition[0] - (src[i].X * res), srcResult.ScanOperatorResult.AxisPosition[1] + (src[i].Y * res));
                PointF calcDst = new PointF(dstResult.ScanOperatorResult.AxisPosition[0] - (dst[i].X * res), dstResult.ScanOperatorResult.AxisPosition[1] + (dst[i].Y * res));
                double calcDist = MathHelper.GetLength(calcSrc, calcDst) / 1000;

                PointF dispSrc = new PointF(srcResult.ScanOperatorResult.CanvasAxisPosition[0] / Operator.DispResizeRatio + (src[i].X), srcResult.ScanOperatorResult.CanvasAxisPosition[1] / Operator.DispResizeRatio + (src[i].Y));
                PointF dispDst = new PointF(dstResult.ScanOperatorResult.CanvasAxisPosition[0] / Operator.DispResizeRatio + (dst[i].X), dstResult.ScanOperatorResult.CanvasAxisPosition[1] / Operator.DispResizeRatio + (dst[i].Y));
                lengthMeasureList.Add(new LengthMeasure(Direction.Horizontal, dispSrc, dispDst, calcDist, true));
            }

            Debug.WriteLine(string.Format("WidthMeasure {0}", stopwatch.Elapsed));
        }

        private void MarginMeasure(ResultCombiner resultCombiner, List<ExtraMeasure> marginMeasure)
        {
            LogHelper.Debug(LoggerType.Inspection, $"InspectOperator::MarginMeasure Start");
            Stopwatch stopwatch = Stopwatch.StartNew();

            DebugContext debugContext = this.GetDebugContext("InspectorOperator_MarginMeasure");
            float res = DeveloperSettings.Instance.Resolution;
            List<ExtractOperatorResult> extractOperatorResultList = resultCombiner.ExtractOperatorResultArray.ToList();

            List<MarginMeasurePos> marginMeasurePosList = SystemManager.Instance().CurrentModel.MarginMeasurePosList.ToList();
            LogHelper.Debug(LoggerType.Inspection, $"InspectOperator::MarginMeasure marginMeasurePosList.Count: {marginMeasurePosList.Count}");

            for (int i = 0; i < marginMeasurePosList.Count; i++)
            {
                LogHelper.Debug(LoggerType.Inspection, $"InspectOperator::MarginMeasure {i}");
                MarginMeasurePos marginMeasurePos = marginMeasurePosList[i];
                if (marginMeasurePos == null)
                    continue;

                LogHelper.Debug(LoggerType.Inspection, $"InspectOperator::MarginMeasure - Name: {marginMeasurePos.Name}");
                ExtractOperatorResult extractOperatorResult = extractOperatorResultList[marginMeasurePos.FlowPosition];
                if (extractOperatorResult == null)
                    continue;

                AlgoImage maskBuffer = null;
                AlgoImage sheetBuffer = null;
                AlgoImage topLightImage = null;
                AlgoImage backLightImage = null;
                Point offsetPx = Point.Empty;
                Point rotatePx = Point.Empty;
                float ds = 0; // Radian
                PointF dispPoint = new PointF(extractOperatorResult.ScanOperatorResult.CanvasAxisPosition[0] / Operator.DispResizeRatio, extractOperatorResult.ScanOperatorResult.CanvasAxisPosition[1] / Operator.DispResizeRatio);
                Rectangle rectangle = marginMeasurePos.Rectangle;
                try
                {
                    ScanOperatorResult scanOperatorResult = extractOperatorResult.ScanOperatorResult;
                    Rectangle fullRect = new Rectangle(Point.Empty, scanOperatorResult.TopLightImage.Size);

                    // LB 포인트를 보고 오프셋 정렬
                    if (marginMeasurePos.IsAlignable)
                    {
                        Point lb = extractOperatorResultList[0].VertexPoints[3];
                        offsetPx = DrawingHelper.Subtract(lb, marginMeasurePos.BasePos[0]);
                        rectangle.Offset(offsetPx);
                        LogHelper.Debug(LoggerType.Inspection, $"InspectOperator::MarginMeasure - offsetPx: {offsetPx}");

                        // 고급 보정
                        if (settings.MarginMeasureAdvAlign && marginMeasurePos.IsAlignable)
                        {
                            Point[] basePts = new Point[]
                            {
                                DrawingHelper.Add(marginMeasurePos.BasePos[0], offsetPx),
                                DrawingHelper.Add(marginMeasurePos.BasePos[1], offsetPx),
                                DrawingHelper.Add(marginMeasurePos.BasePos[2], offsetPx)
                            };

                            Point loc = basePts[2];

                            // 티칭 회전량
                            Point lb0 = basePts[0];
                            Point lt0 = basePts[1];
                            float s0 = (float)Math.Atan2(lt0.Y - lb0.Y, lt0.X - lb0.X); // Rad

                            // 시트 회전량
                            Point lb1 = extractOperatorResultList[0].VertexPoints[3];
                            Point lt1 = extractOperatorResultList[0].VertexPoints[0];
                            float s1 = (float)Math.Atan2(lt1.Y - lb1.Y, lt1.X - lb1.X); // Rad

                            // 회전량 차이
                            ds = (float)((s0 - s1) * 180 / Math.PI); // deg
                            Point alignedPt = MathHelper.Rotate(loc, basePts[0], ds); // deg
                            rotatePx = DrawingHelper.Subtract(alignedPt, basePts[2]);
                            rectangle.Offset(rotatePx);
                            LogHelper.Debug(LoggerType.Inspection, $"InspectOperator::MarginMeasure - rotatePx: {rotatePx}");
                        }
                    }

                    if (Rectangle.Intersect(rectangle, fullRect) != rectangle)
                        throw new Exception("Rectangle is not intersected");

                    Size imageSize = rectangle.Size;

                    using (AlgoImage t = extractOperatorResult.MaskBuffer.Clip(rectangle))
                    {
                        //t.Save(@"C:\temp\MaskBuffer.bmp");

                        PointF centerPt = DrawingHelper.CenterPoint(new Rectangle(Point.Empty, rectangle.Size));
                        RectangleF[] rectF = GetBlobRects(t).Select(f => f.GetBoundRect()).ToArray();
                        PointF newCenterPt = rectF.Select(f => DrawingHelper.CenterPoint(f))
                            .OrderBy(f => MathHelper.GetLength(f, centerPt)).FirstOrDefault();

                        //PointF centerPt = DrawingHelper.CenterPoint(rectangle);
                        //PointF newCenterPt = extractOperatorResult.BlobRectList
                        //    .Select(f => DrawingHelper.CenterPoint(RectangleF.Intersect(f.BoundingRect, rectangle)))
                        //    .OrderBy(f => MathHelper.GetLength(f, centerPt)).FirstOrDefault();
                        if (!newCenterPt.IsEmpty)
                        {
                            Point offset = Point.Round(new PointF(newCenterPt.X - centerPt.X, newCenterPt.Y - centerPt.Y));
                            rectangle.Offset(offset);
                        }
                    }
                    maskBuffer = extractOperatorResult.MaskBuffer.Clip(rectangle);
                    sheetBuffer = extractOperatorResult.SheetBuffer.Clip(rectangle);
                    topLightImage = scanOperatorResult.TopLightImage.Clip(rectangle);
                    backLightImage = scanOperatorResult.BackLightImage.Clip(rectangle);

                    DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, marginMeasurePos.Name));
                    Tuple<RectangleF, float[], BitmapSource> marginResult = Algorithm.MarginMeasureAlgorhtm.Measure(topLightImage, maskBuffer, marginMeasurePos, newDebugContext);

                    // UI 좌표에 맞게 변환 후 리스트에 Add
                    RectangleF dispRect = DrawingHelper.Offset(marginResult.Item1, rectangle.Location);
                    dispRect.Offset(dispPoint);

                    float[] distUm = marginResult.Item2.Select((f, j) => Math.Max(0, (f * res) + (j % 2 == 0 ? this.settings.MarginMeasureOffsetX : this.settings.MarginMeasureOffsetY))).ToArray();
                    //distUm[0] += this.settings.MarginMeasureOffsetX; // Left
                    //distUm[1] += this.settings.MarginMeasureOffsetY; // Top
                    //distUm[2] += this.settings.MarginMeasureOffsetX; // Right
                    //distUm[3] += this.settings.MarginMeasureOffsetY; // Bottom

                    marginMeasure.Add(new Data.ExtraMeasure(marginMeasurePos.Clone(false), DrawingHelper.Add(offsetPx, rotatePx), dispRect, marginResult.Item3, distUm));
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(ex.Message);
                    sb.AppendLine(ex.StackTrace);

                    if (marginMeasurePos == null)
                    {
                        sb.AppendLine("InspectOperator::MarginMeasure - marginMeasurePos is null");
                    }
                    else
                    {
                        sb.AppendLine($"InspectOperator::MarginMeasure - marginMeasurePos.Rectangle: {marginMeasurePos.Rectangle}");
                        sb.AppendLine($"InspectOperator::MarginMeasure - marginMeasurePos.BasePos: {marginMeasurePos.BasePos}");
                        sb.AppendLine($"InspectOperator::MarginMeasure - DSlope: {ds:F02}");
                        sb.AppendLine($"InspectOperator::MarginMeasure - OffsetPx: {offsetPx}");
                        sb.AppendLine($"InspectOperator::MarginMeasure - RotatePx: {rotatePx}");
                    }
                    LogHelper.Error(LoggerType.Error, sb.ToString());

                    Point rectCenter = DrawingHelper.CenterPoint(rectangle);
                    RectangleF dispRect = DrawingHelper.FromCenterSize(DrawingHelper.Add(dispPoint, rectCenter), new Size(1, 1));
                    marginMeasure.Add(new Data.ExtraMeasure(marginMeasurePos.Clone(false), DrawingHelper.Add(offsetPx, rotatePx), dispRect, null, new float[4]));
                }
                finally
                {
                    maskBuffer?.Dispose();
                    sheetBuffer?.Dispose();
                    topLightImage?.Dispose();
                    backLightImage?.Dispose();
                }
            }

            LogHelper.Debug(LoggerType.Inspection, $"InspectOperator::MarginMeasure End. {stopwatch.Elapsed}[ms]");
            Debug.WriteLine(string.Format("MarginMeasure {0}", stopwatch.Elapsed));
        }

        private RotatedRect[] GetBlobRects(AlgoImage maskSubImage)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(maskSubImage);
            PointF imgCenterPt = DrawingHelper.CenterPoint(new Rectangle(Point.Empty, maskSubImage.Size));

            using (BlobRectList blobRectList = ip.Blob(maskSubImage, new BlobParam() { EraseBorderBlobs = false, SelectRotateRect = true }))
                return blobRectList.GetArray()
                    .Select(f => new RotatedRect(DrawingHelper.FromCenterSize(f.RotateCenterPt, new SizeF(f.RotateWidth, f.RotateHeight)), f.RotateAngle))
                    .OrderBy(f => MathHelper.GetLength(DrawingHelper.CenterPoint(f), imgCenterPt))
                    .ToArray();
        }

        private float GetMarginDistancePx(RectangleF[] orderBlobRectList, Tuple<RectangleF, Func<RectangleF, RectangleF, float>> tuple, float offset)
        {
            if (tuple == null)
                return 0;

            RectangleF searchRect = tuple.Item1;
            Func<RectangleF, RectangleF, float> func = tuple.Item2;

            RectangleF[] findAll = Array.FindAll(orderBlobRectList, f => searchRect.IntersectsWith(f));
            if (findAll.Length == 0)
                return -1;

            List<float> aa = findAll.Select(f => func(searchRect, f)).ToList();
            aa.RemoveAll(f => f <= 0);
            if (aa.Count > 0)
                return aa.Min() + offset;
            return 0;
            return findAll.Min(f => func(searchRect, f));
        }

        public int Comp(Tuple<int, int, float> a, Tuple<int, int, float> b)
        {
            //return a.Item3.CompareTo(b.Item3);
            return b.Item3.CompareTo(a.Item3);
        }

        private BlobRectList Blob(AlgoImage algoImage, BlobParam blobParam, AlgoImage greyMask)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            BlobRectList blobRectList;
            if (algoImage.Size != greyMask.Size)
            {
                //Point p = new Point(0, algoImage.Height - greyMask.Height);
                Point p = Point.Empty;
                using (AlgoImage t = algoImage.GetSubImage(new Rectangle(p, greyMask.Size)))
                    blobRectList = imageProcessing.Blob(t, blobParam, greyMask);
            }
            else
            {
                blobRectList = imageProcessing.Blob(algoImage, blobParam, greyMask);
            }
            return blobRectList;
        }
    }


    public class InspectOperatorResult : OperatorResult
    {
        ExtractOperatorResult extractOperatorResult;
        List<IResultObject> defectList;

        public ExtractOperatorResult ExtractOperatorResult { get => extractOperatorResult; }
        public List<IResultObject> DefectList { get => defectList; }

        protected InspectOperatorResult(ResultType resultType, ResultKey resultKey, ExtractOperatorResult extractOperatorResult, List<IResultObject> defectList)
            : base(resultType, resultKey, DateTime.Now)
        {
            this.extractOperatorResult = extractOperatorResult;
            this.defectList = defectList;
        }

        protected InspectOperatorResult(ResultType resultType, ResultKey resultKey, Exception exception, ExtractOperatorResult extractOperatorResult)
            : base(resultType, resultKey, DateTime.Now, exception)
        {
            this.extractOperatorResult = extractOperatorResult;
            this.defectList = new List<IResultObject>();
        }

        public InspectOperatorResult(ResultKey resultKey, ExtractOperatorResult extractOperatorResult, List<IResultObject> defectList)
            : this(ResultType.Inspect, resultKey, extractOperatorResult, defectList)
        {
        }

        public InspectOperatorResult(ResultKey resultKey, Exception exception, ExtractOperatorResult extractOperatorResult)
            : this(ResultType.Inspect, resultKey, exception, extractOperatorResult)
        { }

        protected override string GetLogMessage()
        {
            return string.Format("FlowPosition,{0}", this.extractOperatorResult.ScanOperatorResult.FlowPosition);
        }
    }

    public class LumpInspectOperatorResult : InspectOperatorResult
    {
        public LumpInspectOperatorResult(ResultKey resultKey, ExtractOperatorResult extractOperatorResult, List<IResultObject> defectList)
            : base(ResultType.InspectLump, resultKey, extractOperatorResult, defectList)
        { }

        public LumpInspectOperatorResult(ResultKey resultKey, Exception exception, ExtractOperatorResult extractOperatorResult)
            : base(ResultType.InspectLump, resultKey, exception, extractOperatorResult)
        { }
    }

    public class InspectOperatorSettings : OperatorSettings
    {
        float patternUpper = 20;
        float patternLower = 20;
        float patternMinDefectSize = 50;
        float patternMaxDefectSize = 50000;
        bool usePatternMaxDefectSize = false;
        float patternIgnoreRangeLength = -1;

        public float PatternUpper { get => patternUpper; set => Set("PatternUpper", ref this.patternUpper, value); }
        public float PatternLower { get => patternLower; set => Set("PatternLower", ref this.patternLower, value); }
        public float PatternMinDefectSize { get => patternMinDefectSize; set => Set("PatternMinDefectSize", ref this.patternMinDefectSize, value); }
        public float PatternMaxDefectSize { get => patternMaxDefectSize; set => Set("PatternMaxDefectSize", ref this.patternMaxDefectSize, value); }
        public bool UsePatternMaxDefectSize { get => usePatternMaxDefectSize; set => Set("UsePatternMaxDefectSize", ref this.usePatternMaxDefectSize, value); }
        public float PatternIgnoreRangeLength { get => patternIgnoreRangeLength; set => Set("PatternIgnoreRangeLength", ref this.patternIgnoreRangeLength, value); }

        float marginUpper = 10;
        float marginLower = 20;
        float marginMinDefectSize = 50;
        float marginMaxDefectSize = 50000;
        bool useMarginMaxDefectSize = false;
        float marginIgnoreRangeLength = -1;

        public float MarginUpper { get => marginUpper; set => Set("MarginUpper", ref this.marginUpper, value); }
        public float MarginLower { get => marginLower; set => Set("MarginLower", ref this.marginLower, value); }
        public float MarginMinDefectSize { get => marginMinDefectSize; set => Set("MarginMinDefectSize", ref this.marginMinDefectSize, value); }
        public float MarginMaxDefectSize { get => marginMaxDefectSize; set => Set("MarginMaxDefectSize", ref this.marginMaxDefectSize, value); }
        public bool UseMarginMaxDefectSize { get => useMarginMaxDefectSize; set => Set("UseMarginMaxDefectSize", ref this.useMarginMaxDefectSize, value); }
        public float MarginIgnoreRangeLength { get => marginIgnoreRangeLength; set => Set("MarginIgnoreRangeLength", ref this.marginIgnoreRangeLength, value); }

        bool useLumpDetect = false;
        int lumpUpperMin = 10;
        int lumpUpperMax = 20;
        int lumpLowerMin = 10;
        int lumpLowerMax = 20;
        float lumpMinDefectSize = 15;
        float lumpMaxDefectSize = 100;
        bool useLumpMaxDefectSize = false;

        public bool UseLumpDetect { get => this.useLumpDetect; set => Set("UseLumpDetect", ref this.useLumpDetect, value); }
        public int LumpUpperMin { get => this.lumpUpperMin; set => Set("LumpUpperMin", ref this.lumpUpperMin, value); }
        public int LumpUpperMax { get => this.lumpUpperMax; set => Set("LumpUpperMax", ref this.lumpUpperMax, value); }
        public int LumpLowerMin { get => this.lumpLowerMin; set => Set("LumpLowerMin", ref this.lumpLowerMin, value); }
        public int LumpLowerMax { get => this.lumpLowerMax; set => Set("LumpLowerMax", ref this.lumpLowerMax, value); }
        public float LumpMinDefectSize { get => this.lumpMinDefectSize; set => Set("LumpMinDefectSize", ref this.lumpMinDefectSize, value); }
        public float LumpMaxDefectSize { get => this.lumpMaxDefectSize; set => Set("LumpMaxDefectSize", ref this.lumpMaxDefectSize, value); }
        public bool UseLumpMaxDefectSize { get => this.useLumpMaxDefectSize; set => Set("UseLumpMaxDefectSize", ref this.useLumpMaxDefectSize, value); }

        bool useShapeInspect = true;
        float diffThreshold = 25;
        float circularThreshold = 1.2f; // Circle = 1, Rect = 4/pi = 1.273..
        int maxDefectCount = 500;
        float meanderSensitivity = 95;
        int inspectRegionWidthPx = 200;
        bool marginMeasureAdvAlign = false;

        public bool UseShapeInspect { get => useShapeInspect; set => Set("UseShapeInspect", ref this.useShapeInspect, value); }
        public float DiffThreshold { get => diffThreshold; set => Set("DiffThreshold", ref this.diffThreshold, value); }
        public float CircularThreshold { get => this.circularThreshold; set => Set("CircularThreshold", ref this.circularThreshold, value); }
        public int MaxDefectCount { get => this.maxDefectCount; set => Set("MaxDefectCount", ref this.maxDefectCount, value); }
        public float MeanderSensitivity { get => meanderSensitivity; set => Set("MeanderSensitivity", ref this.meanderSensitivity, Math.Min(100, Math.Max(0, value))); }
        public int InspectRegionWidthPx { get => this.inspectRegionWidthPx; set => Set("InspectRegionWidthPx", ref this.inspectRegionWidthPx, value); }
        public bool MarginMeasureAdvAlign { get => this.marginMeasureAdvAlign; set => Set("MarginMeasureAdvAlign", ref this.marginMeasureAdvAlign, value); }

        int defectImageInflatePx = 50;
        public MarginMeasureParam MarginMeasureParam { get; private set; } = new MarginMeasureParam();
        float marginMeasureOffsetX = 0;
        float marginMeasureOffsetY = 0;

        public float MarginMeasureOffsetX { get => marginMeasureOffsetX; set => Set("MarginMeasureOffsetX", ref this.marginMeasureOffsetX, value); }

        public float MarginMeasureOffsetY { get => marginMeasureOffsetY; set => Set("MarginMeasureOffsetY", ref this.marginMeasureOffsetY, value); }

        public int DefectImageInflatePx { get => this.defectImageInflatePx; set => Set("DefectImageInflatePx", ref this.defectImageInflatePx, value); }

        protected override void Initialize()
        {
            fileName = String.Format(@"{0}\{1}.xml", PathSettings.Instance().Config, "Inspect");
        }

        public override void Load(XmlElement xmlElement)
        {
            this.patternUpper = XmlHelper.GetValue(xmlElement, "PatternUpper", this.patternUpper);
            this.patternLower = XmlHelper.GetValue(xmlElement, "PatternLower", this.patternLower);
            this.patternMinDefectSize = XmlHelper.GetValue(xmlElement, "PatternMinDefectSize", this.patternMinDefectSize);
            this.patternMaxDefectSize = XmlHelper.GetValue(xmlElement, "PatternMaxDefectSize", this.patternMaxDefectSize);
            this.usePatternMaxDefectSize = XmlHelper.GetValue(xmlElement, "UsePatternMaxDefectSize", this.usePatternMaxDefectSize);
            this.patternIgnoreRangeLength = XmlHelper.GetValue(xmlElement, "PatternIgnoreRangeLength", this.patternIgnoreRangeLength);

            this.marginUpper = XmlHelper.GetValue(xmlElement, "MarginUpper", this.marginUpper);
            this.marginLower = XmlHelper.GetValue(xmlElement, "MarginLower", this.marginLower);
            this.marginMinDefectSize = XmlHelper.GetValue(xmlElement, "MarginMinDefectSize", this.marginMinDefectSize);
            this.marginMaxDefectSize = XmlHelper.GetValue(xmlElement, "MarginMaxDefectSize", this.marginMaxDefectSize);
            this.useMarginMaxDefectSize = XmlHelper.GetValue(xmlElement, "UseMarginMaxDefectSize", this.useMarginMaxDefectSize);
            this.marginIgnoreRangeLength = XmlHelper.GetValue(xmlElement, "MarginIgnoreRangeLength", this.marginIgnoreRangeLength);

            this.useLumpDetect = XmlHelper.GetValue(xmlElement, "UseLumpDetect", this.useLumpDetect);
            this.lumpUpperMin = XmlHelper.GetValue(xmlElement, "LumpUpperMin", this.lumpUpperMin);
            this.lumpUpperMax = XmlHelper.GetValue(xmlElement, "LumpUpperMax", this.lumpUpperMax);
            this.lumpLowerMin = XmlHelper.GetValue(xmlElement, "LumpLowerMin", this.lumpLowerMin);
            this.lumpLowerMax = XmlHelper.GetValue(xmlElement, "LumpLowerMax", this.lumpLowerMax);
            this.lumpMinDefectSize = XmlHelper.GetValue(xmlElement, "LumpMinDefectSize", this.lumpMinDefectSize);
            this.lumpMaxDefectSize = XmlHelper.GetValue(xmlElement, "LumpMaxDefectSize", this.lumpMaxDefectSize);
            this.useLumpMaxDefectSize = XmlHelper.GetValue(xmlElement, "UseLumpMaxDefectSize", this.useLumpMaxDefectSize);

            this.useShapeInspect = XmlHelper.GetValue(xmlElement, "UseShapeInspect ", this.useShapeInspect);
            this.diffThreshold = XmlHelper.GetValue(xmlElement, "DiffThreshold", this.diffThreshold);

            this.circularThreshold = XmlHelper.GetValue(xmlElement, "CircularThreshold", this.circularThreshold);

            this.defectImageInflatePx = XmlHelper.GetValue(xmlElement, "DefectImageInflatePx", this.defectImageInflatePx);
            this.inspectRegionWidthPx = XmlHelper.GetValue(xmlElement, "InspectRegionWidthPx", this.inspectRegionWidthPx);

            this.meanderSensitivity = XmlHelper.GetValue(xmlElement, "MeanderSensitivity", this.meanderSensitivity);

            this.MarginMeasureParam.Load(xmlElement);
            this.marginMeasureAdvAlign = XmlHelper.GetValue(xmlElement, "MarginMeasureAdvAlign", this.marginMeasureAdvAlign);
            this.marginMeasureOffsetX = XmlHelper.GetValue(xmlElement, "MarginMeasureOffsetX", this.marginMeasureOffsetX);
            this.marginMeasureOffsetY = XmlHelper.GetValue(xmlElement, "MarginMeasureOffsetY", this.marginMeasureOffsetY);

            this.maxDefectCount = XmlHelper.GetValue(xmlElement, "MaxDefectCount", this.maxDefectCount);
        }

        public override void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "PatternUpper", this.patternUpper);
            XmlHelper.SetValue(xmlElement, "PatternLower", this.patternLower);
            XmlHelper.SetValue(xmlElement, "PatternMinDefectSize", this.patternMinDefectSize);
            XmlHelper.SetValue(xmlElement, "PatternMaxDefectSize", this.patternMaxDefectSize);
            XmlHelper.SetValue(xmlElement, "UsePatternMaxDefectSize", this.usePatternMaxDefectSize);
            XmlHelper.SetValue(xmlElement, "PatternIgnoreRangeLength", this.patternIgnoreRangeLength);

            XmlHelper.SetValue(xmlElement, "MarginUpper", this.marginUpper);
            XmlHelper.SetValue(xmlElement, "MarginLower", this.marginLower);
            XmlHelper.SetValue(xmlElement, "MarginMinDefectSize", this.marginMinDefectSize);
            XmlHelper.SetValue(xmlElement, "MarginMaxDefectSize", this.marginMaxDefectSize);
            XmlHelper.SetValue(xmlElement, "UseMarginMaxDefectSize", this.useMarginMaxDefectSize);
            XmlHelper.SetValue(xmlElement, "MarginIgnoreRangeLength", this.marginIgnoreRangeLength);

            XmlHelper.SetValue(xmlElement, "UseLumpDetect", this.useLumpDetect);
            XmlHelper.SetValue(xmlElement, "LumpUpperMin", this.lumpUpperMin);
            XmlHelper.SetValue(xmlElement, "LumpUpperMax", this.lumpUpperMax);
            XmlHelper.SetValue(xmlElement, "LumpLowerMin", this.lumpLowerMin);
            XmlHelper.SetValue(xmlElement, "LumpLowerMax", this.lumpLowerMax);
            XmlHelper.SetValue(xmlElement, "LumpMinDefectSize", this.lumpMinDefectSize);
            XmlHelper.SetValue(xmlElement, "LumpMaxDefectSize", this.lumpMaxDefectSize);
            XmlHelper.SetValue(xmlElement, "UseLumpMaxDefectSize", this.useLumpMaxDefectSize);

            XmlHelper.SetValue(xmlElement, "UseShapeInspect", this.useShapeInspect);
            XmlHelper.SetValue(xmlElement, "DiffThreshold", this.diffThreshold);

            XmlHelper.SetValue(xmlElement, "CircularThreshold", this.circularThreshold);

            XmlHelper.SetValue(xmlElement, "DefectImageInflatePx", this.defectImageInflatePx);
            XmlHelper.SetValue(xmlElement, "InspectRegionWidthPx", this.inspectRegionWidthPx);

            XmlHelper.SetValue(xmlElement, "MeanderSensitivity", this.meanderSensitivity);

            this.MarginMeasureParam.Save(xmlElement);
            XmlHelper.SetValue(xmlElement, "MarginMeasureAdvAlign", this.marginMeasureAdvAlign);
            XmlHelper.SetValue(xmlElement, "MarginMeasureOffsetX", this.marginMeasureOffsetX);
            XmlHelper.SetValue(xmlElement, "MarginMeasureOffsetY", this.marginMeasureOffsetY);

            XmlHelper.SetValue(xmlElement, "MaxDefectCount", this.maxDefectCount);
        }
    }
}