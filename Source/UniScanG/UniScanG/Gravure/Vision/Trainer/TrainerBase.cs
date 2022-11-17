using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System.IO;
using UniEye.Base.Settings;
using System.Drawing.Imaging;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Detector;
using System.Threading;
using UniScanG.Gravure.Vision.Extender;
using UniScanG.Data;
using UniScanG.Gravure.Vision.Trainer.Cluster;
using UniScanG.Vision;
using UniScanG.Data.Model;

namespace UniScanG.Gravure.Vision.Trainer
{
    public abstract class TrainerBase : UniScanG.Vision.AlgorithmG
    {
        public static string TypeName { get { return "SheetTrainer"; } }
        public ETrainerVersion TrainerVersion { get; private set; }
        public static TrainerParam TrainerParam => AlgorithmSetting.Instance().TrainerParam;

        protected BackgroundWorker worker = null;

        public abstract Point TrainBaseline(AlgoImage trainImage, Model model, int start, int end, DebugContextG debugContextG);
        public abstract bool TrainPattern(AlgoImage trainImage, Model model, int start, int end, DebugContextG debugContextG);
        public abstract bool TrainRegion(AlgoImage trainImage, Model model, int start, int end, DebugContext debugContext);
        public abstract bool TrainExtention(AlgoImage trainImage, Model model, Point baseOffset, int start, int end, DebugContext debugContext);

        public TrainerBase(ETrainerVersion trainerVersion)
        {
            this.AlgorithmName = TypeName;
            this.param = null;
            this.TrainerVersion = trainerVersion;
        }

        public override Algorithm Clone()
        {
            TrainerBase t = TrainerBase.Create(this.TrainerVersion);
            t.param.CopyFrom(this.param);
            return t;
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public static TrainerBase Create(ETrainerVersion trainerVersion)
        {
            switch (trainerVersion)
            {
                case ETrainerVersion.V1:
                case ETrainerVersion.V2:
                    return new Trainer(trainerVersion);
                    break;
                case ETrainerVersion.RCI:
                    return new RCI.Trainer.RCITrainer();
                    break;
            }
            return null;
        }

        public override AlgorithmParam CreateParam()
        {
            return new TrainerParam(true);
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            throw new NotImplementedException();
        }

        public void Teach(BackgroundWorker worker, Image2D currentImage, DoWorkEventArgs args)
        {
            LogHelper.Debug(LoggerType.Operation, "Trainer::Teach - Start");

            this.worker = worker;
            TrainerParam param = TrainerBase.TrainerParam;
            if (param == null)
                return;

            TrainerArgument trainerArgument = (TrainerArgument)args.Argument;
            if (trainerArgument == null)
                trainerArgument = new TrainerArgument(false, true, true, true);
            if (trainerArgument.Model == null)
                trainerArgument.Model = SystemManager.Instance().CurrentModel;

            args.Result = trainerArgument;

            AlgoImage fullImage = null;
            DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, Path.Combine(PathSettings.Instance().Temp, "Trainer"));
            DebugContextG debugContextG = new DebugContextG(debugContext) { PatternId = -1 };
            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
            try
            {
                fullImage = ImageBuilder.Build(GetAlgorithmType(), currentImage, ImageType.Grey, ImageBandType.Luminance);

                List<int> progressPosList = new List<int>();
                progressPosList.Add(10);

                if (trainerArgument.DoPattern)
                    progressPosList.Add(40);
                if (trainerArgument.DoRegion)
                    progressPosList.Add(40);
                if (trainerArgument.DoMonitoring)
                    progressPosList.Add(10);

                int totalProgressPos = progressPosList.Sum();

                int srcPos = 0;
                int dstPos = 0;
                int step = 0;

                srcPos = 0;
                dstPos = progressPosList.Take(++step).Sum() * 100 / totalProgressPos;
                Point basePosition = TrainBaseline(fullImage, trainerArgument.Model, srcPos, dstPos, debugContextG); // 10%
                Point baseOffset = Point.Empty;
                if (!basePosition.IsEmpty)
                {
                    // 이전 Base Position과 티칭 후 BasePosition 확인
                    Point oldBasePosition = CalculatorBase.CalculatorParam.ModelParam.BasePosition;
                    if (!oldBasePosition.IsEmpty)
                    {
                        baseOffset = Point.Subtract(basePosition, (Size)oldBasePosition);
                        LogHelper.Debug(LoggerType.Operation, string.Format("Trainer::Teach - BaseOffset: {0}", baseOffset.ToString()));

                        // 패턴들을 오프셋 만큼 이동
                        CalculatorBase.CalculatorParam.ModelParam.PatternGroupCollection.ForEach(f => f.Offset(baseOffset));

                        // 바를 오프셋 만큼 이동
                        CalculatorBase.CalculatorParam.ModelParam.RegionInfoCollection.ForEach(f => f.Offset(baseOffset, fullImage));

                        // Watch Item을 오프셋 만큼 이동
                        //WatcherParam watcherParam = AlgorithmPool.Instance().GetAlgorithm(Watcher.TypeName).Param as WatcherParam;
                        //for (int i = 0; i < watcherParam.WatchItemList.Count; i++)
                        //    watcherParam.WatchItemList[i].Offset(offset);
                    }

                    CalculatorBase.CalculatorParam.ModelParam.BasePosition = basePosition;
                }
                ThrowIfCancellationPending();

                if (trainerArgument.DoPattern)
                {
                    srcPos = dstPos;
                    dstPos = progressPosList.Take(++step).Sum() * 100 / totalProgressPos;
                    trainerArgument.PatternGood = TrainPattern(fullImage, trainerArgument.Model, srcPos, dstPos, debugContextG);
                    if (trainerArgument.PatternGood)
                    {
                        // 시트 길이 표준값 업데이트
                        CalculatorBase.CalculatorParam.ModelParam.SheetSizeMm = DrawingHelper.Div(calibration.PixelToWorld(fullImage.Size), 1000);

                        // 시트 밝기 편차값 업데이트
                        SheetFinderBaseParam sheetFinderBaseParam= AlgorithmSetting.Instance().SheetFinderBaseParam as SheetFinderBaseParam;
                        if (sheetFinderBaseParam != null)
                        {
                            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(fullImage);
                            float[] proj = ip.Projection(fullImage, Direction.Vertical, ProjectionType.Mean);
                            sheetFinderBaseParam.MinBrightnessStdDev = MathHelper.StdDev(proj);
                        }
                    }
                }

                if (trainerArgument.PatternGood && trainerArgument.DoRegion)
                {
                    srcPos = dstPos;
                    dstPos = progressPosList.Take(++step).Sum() * 100 / totalProgressPos;
                    trainerArgument.RegionGood = TrainRegion(fullImage, trainerArgument.Model, srcPos, dstPos, debugContextG);
                }

                if (trainerArgument.PatternGood && trainerArgument.RegionGood && trainerArgument.DoMonitoring)
                {
                    srcPos = dstPos;
                    dstPos = progressPosList.Take(++step).Sum() * 100 / totalProgressPos;
                    trainerArgument.SizeVariationGood &= TrainExtention(fullImage, trainerArgument.Model, baseOffset, srcPos, dstPos, debugContextG); // 10%
                }

                trainerArgument.IsTeachDone =
                    (!trainerArgument.DoPattern || trainerArgument.PatternGood) && (!trainerArgument.DoRegion || trainerArgument.RegionGood);
                
                Thread.Sleep(1000);

                args.Result = trainerArgument;
            }
            catch (OperationCanceledException)
            {
                trainerArgument.IsTeachDone = false;
                trainerArgument.IsCancelled = true;
                args.Cancel = true;
            }
#if DEBUG == false
            catch (Exception ex)
            {
                trainerArgument.IsTeachDone = false;
                trainerArgument.IsCancelled = true;
                trainerArgument.Exception = ex;
                //args.Cancel = true;
            }
#endif
            finally
            {
                fullImage?.Dispose();
            }
        }

        protected void ThrowIfCancellationPending()
        {
            if (worker != null && worker.CancellationPending)
                throw new OperationCanceledException();
        }

        public AlgoImage GetMajorPatternImage(AlgoImage trainImage)
        {
            AlgoImage patternImage = null, majorPatternImage = null;
            try
            {
                patternImage = ImageBuilder.BuildSameTypeSize(trainImage);
                this.CreateMaskImage(trainImage, patternImage);

                // 주 패턴만 추출한 영상을 만듬
                majorPatternImage = ImageBuilder.BuildSameTypeSize(patternImage);
                List<SheetPatternGroup> majorPatternGroupList = CalculatorBase.CalculatorParam.ModelParam.PatternGroupCollection.FindAll(f => f.Use);
                DrawPatternGroup(patternImage, majorPatternGroupList, majorPatternImage);
                return majorPatternImage;
            }
            finally
            {
                patternImage?.Dispose();
            }
        }


        private bool GetWatchItem(ExtItem watchItem, AlgoImage trainImage, RegionInfoG regionInfoG, PointF position, DebugContext debugContext)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(trainImage);

            // Bar 영역 이미지
            Rectangle patRect = regionInfoG.Region;
            AlgoImage barImage = trainImage.Clip(patRect);
            barImage.Save("barImage.bmp", debugContext);

            // 이진화 후 Blob
            ip.Binarize(barImage, barImage, true);
            barImage.Save("Binarize.bmp", debugContext);

            List<BlobRect> blobList;
            using (BlobRectList blobRectList = ip.Blob(barImage, new BlobParam() { EraseBorderBlobs = true }))
                blobList = blobRectList.GetList();

            // Blob 평균 면적의 0.5배 이하인 Blob은 제거
            float average = blobList.Average(f => f.Area);
            blobList.RemoveAll(f => f.Area < average * 0.5);

            // Bar의 가로/세로 pt지점과 가장 가까운 Blob 선택
            PointF centerPt = new PointF(barImage.Width * position.X, barImage.Height * position.Y);
            blobList = blobList.OrderBy(f => MathHelper.GetLength(centerPt, f.CenterPt)).ToList();
            BlobRect centerBlobRect = blobList.FirstOrDefault();
            RectangleF boundRect = centerBlobRect.BoundingRect;
            BlobRect[] neighbor = new BlobRect[]
            {
                blobList.FindAll(f=>f.BoundingRect.IntersectsWith(RectangleF.FromLTRB(0,boundRect.Top,boundRect.Left,boundRect.Bottom))).OrderBy(f=>f.BoundingRect.Right).LastOrDefault(),
                blobList.FindAll(f=>f.BoundingRect.IntersectsWith(RectangleF.FromLTRB(boundRect.Left,0,boundRect.Right,boundRect.Top))).OrderBy(f=>f.BoundingRect.Bottom).LastOrDefault(),
                blobList.FindAll(f=>f.BoundingRect.IntersectsWith(RectangleF.FromLTRB(boundRect.Right,boundRect.Top, barImage.Width,boundRect.Bottom))).OrderBy(f=>f.BoundingRect.Left).FirstOrDefault(),
                blobList.FindAll(f=>f.BoundingRect.IntersectsWith(RectangleF.FromLTRB(boundRect.Left,boundRect.Bottom,boundRect.Right,barImage.Height))).OrderBy(f=>f.BoundingRect.Top).FirstOrDefault()
            };

            float[] dxs = new float[] { neighbor[0] == null ? 0 : centerBlobRect.CenterPt.X - neighbor[0].CenterPt.X, neighbor[2] == null ? 0 : neighbor[2].CenterPt.X - centerBlobRect.CenterPt.X };
            float[] dys = new float[] { neighbor[1] == null ? 0 : centerBlobRect.CenterPt.Y - neighbor[1].CenterPt.Y, neighbor[3] == null ? 0 : neighbor[3].CenterPt.Y - centerBlobRect.CenterPt.Y };
            SizeF masterRectSize = new SizeF(dxs.Max() * 2, dys.Max() * 2);
            if (masterRectSize.IsEmpty)
                return false;

            Rectangle masterRect = Rectangle.Round(DrawingHelper.FromCenterSize(centerBlobRect.CenterPt, masterRectSize));

            //모니터링 영역으로 등록
            masterRect.Offset(regionInfoG.Region.Location);

            ImageD imageD;
            using (AlgoImage monAlgoImage = trainImage.GetSubImage(masterRect))
                imageD = monAlgoImage.ToImageD();

            watchItem.Use = true;
            watchItem.Index = -1;
            watchItem.ContainerIndex = -1;
            watchItem.SetMasterRectangleUm(masterRect);
            watchItem.MasterImageD = imageD;

            barImage.Dispose();
            return true;
        }

        protected void DrawPatternGroup(AlgoImage patternImage, List<SheetPatternGroup> majorPatternGroupList, AlgoImage majorPattrnImage)
        {
            majorPattrnImage.Clear();
            foreach (SheetPatternGroup majorPatternGroup in majorPatternGroupList)
            {
                foreach (BlobRect blobRect in majorPatternGroup.PatternList)
                {
                    Point pt = Point.Round(blobRect.BoundingRect.Location);
                    Size sz = Size.Round(blobRect.BoundingRect.Size);
                    majorPattrnImage.Copy(patternImage, pt, pt, sz);
                }
            }
        }

        protected List<SheetPatternGroup> FindPatternGroup(BackgroundWorker worker, AlgoImage trainImage, AlgoImage patternImage, DebugContext debugContext)
        {
            float ratioThres = 0.35f;
            List<PatternInfo> patternInfoList = FindPattern(worker, trainImage, patternImage, debugContext);

            List<SheetPatternGroup> sheetPatternGroupList = GetPatternGroup(worker, trainImage, patternImage, patternInfoList);
            sheetPatternGroupList.Sort((f, g) => (g.CountRatio).CompareTo(f.CountRatio));
            SheetPatternGroup firstGroup = sheetPatternGroupList.FirstOrDefault();
            if (firstGroup != null && firstGroup.CountRatio > ratioThres && firstGroup.AverageWidth > firstGroup.AverageHeight * 1.5)
            {
                // 가로로 긴 패턴이면 첫 1개 패턴만 use를 활성화.
                firstGroup.Use = true;
            }
            else
            {
                // 세로로 긴 패턴이면 35%를 넘는 모든 패턴을 활성화.
                sheetPatternGroupList.ForEach(f => f.Use = (f.CountRatio > ratioThres));
            }

            if (debugContext.SaveDebugImage)
            {
                using (AlgoImage algoImage = trainImage.Clone())
                {
                    ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(trainImage);
                    for (int i = 0; i < sheetPatternGroupList.Count; i++)
                    {
                        SheetPatternGroup sheetPatternGroup = sheetPatternGroupList[i];
                        sheetPatternGroup.PatternList.ForEach(f =>
                        {
                            Rectangle rect = Rectangle.Round(f.BoundingRect);
                            ip.DrawRect(algoImage, rect, 0, false);
                            ip.DrawText(algoImage, DrawingHelper.Add(rect.Location, new Point(0, 16 * 0)), 255, string.Format("I{0:D02}", i));
                            ip.DrawText(algoImage, DrawingHelper.Add(rect.Location, new Point(0, 16 * 1)), 255, string.Format("W{0:F01}", f.GetValue(PatternFeature.Width)));
                            ip.DrawText(algoImage, DrawingHelper.Add(rect.Location, new Point(0, 16 * 2)), 255, string.Format("H{0:F01}", f.GetValue(PatternFeature.Height)));
                            ip.DrawText(algoImage, DrawingHelper.Add(rect.Location, new Point(0, 16 * 3)), 255, string.Format("L{0:F01}", f.GetValue(PatternFeature.Waist)));
                            ip.DrawText(algoImage, DrawingHelper.Add(rect.Location, new Point(0, 16 * 4)), 255, string.Format("R{0:F02}", f.GetValue(PatternFeature.WaistRatio)));
                        });
                    }

                    algoImage.Save(@"trainImageC.bmp", debugContext);
                }
            }

            return sheetPatternGroupList;
        }

        public void CreateMaskImage(AlgoImage trainImage, AlgoImage patternImage)
        {
            TrainerParam param = TrainerBase.TrainerParam;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(trainImage);

            AlgoImage binImage = null;
            BlobRectList patternBlobRectList = null;
            patternImage.Clear();
            try
            {
                // 이진화 영상
                int binValue = param.ModelParam.BinValue + param.ModelParam.BinValueOffset;
                binValue = Math.Max(0, Math.Min(255, binValue));

                binImage = ImageBuilder.BuildSameTypeSize(trainImage);
                imageProcessing.Binarize(trainImage, binImage, binValue, true);
                //binImage.Save(@"D:\temp\binImage1.bmp");

                imageProcessing.Open(binImage, binImage, 5);
                //binImage.Save(@"D:\temp\binImage2.bmp");

                // 보조영상 생성
                BlobParam patternBlobParam = new BlobParam();
                patternBlobParam.SelectArea = true;
                patternBlobParam.AreaMin = param.MinPatternArea;

                patternBlobRectList = imageProcessing.Blob(binImage, patternBlobParam);

                DrawBlobOption drawPatternBlobOption = new DrawBlobOption();
                drawPatternBlobOption.SelectBlob = true;
                imageProcessing.DrawBlob(patternImage, patternBlobRectList, null, drawPatternBlobOption);
                //imageProcessing.FillHoles(patternImage, patternImage);
            }
            finally
            {
                patternBlobRectList.Dispose();
                binImage?.Dispose();
            }
        }

        public List<PatternInfo> FindPattern(BackgroundWorker worker, AlgoImage trainImage, AlgoImage patternImage, DebugContext debugContext)
        {
            TrainerParam param = TrainerBase.TrainerParam;
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(patternImage);

            BlobParam patternBlobParam = new BlobParam
            {
                AreaMin = param.MinPatternArea,
                EraseBorderBlobs = true,
                SelectSawToothArea = true,
                SelectRotateRect = true
                //SelectCenterPt = true,
                //SelectCompactness = true,
                //SelectFeretDiameter = false,
            };

            Stopwatch sw = Stopwatch.StartNew();
            BlobRectList patternBlobRectList = ip.Blob(patternImage, patternBlobParam);
            //LogHelper.Debug(LoggerType.Inspection, string.Format("Blob Full Image - Width,{0},Heigth,{1},BlobCount,{2},TimeMs,{3}", patternImage.Width, patternImage.Height, patternBlobRectList.Count, sw.ElapsedMilliseconds));

            List<PatternInfo> patternList = patternBlobRectList.GetList().ConvertAll<PatternInfo>(f => new PatternInfo(f));
            Task.Run(new Action(() => patternBlobRectList.Dispose()));

            // 허리부분 길이 재기??
            patternBlobParam.AreaMin = 0;
            patternBlobParam.EraseBorderBlobs = false;
            foreach (PatternInfo patternInfo in patternList)
            {
                patternInfo.WaistLength = patternInfo.GetValue(PatternFeature.Width);
                Rectangle subRect = Rectangle.Round(patternInfo.BoundingRect);
                if (!param.ModelParam.AdvanceWaistMeasure)
                    // 기존 방식
                {
                    subRect.Inflate(0, -(subRect.Height - 1) / 2);
                    Debug.Assert(subRect.Height > 0);
                    //subRect.Inflate(0, 1);

                    using (AlgoImage patternSubImage = patternImage.GetSubImage(subRect))
                    {
                        //patternSubImage.Save(@"C:\temp\patternSubImage.bmp");
                        List<BlobRect> waistBlobList;
                        using (BlobRectList waistBlobRectList = ip.Blob(patternSubImage, patternBlobParam))
                            waistBlobList = waistBlobRectList.GetList();
                        if(true)
                        {
                            if (waistBlobList.Count > 0)
                            {
                                RectangleF union = RectangleF.Empty;
                                union = waistBlobList.Select(f => f.BoundingRect).Aggregate((g, h) => RectangleF.Union(g, h));
                                patternInfo.WaistLength = union.Width;
                            }

                        }
                        else 
                        // 중심을 포함하는 Blob의 Width
                        {
                            PointF centerPt = DrawingHelper.CenterPoint(new RectangleF(Point.Empty, subRect.Size));
                            BlobRect centerBlobRect = waistBlobList.Find(f => f.BoundingRect.Contains(centerPt));
                            if (centerBlobRect != null)
                                patternInfo.WaistLength = (int)centerBlobRect.BoundingRect.Width;
                        }
                    }
                }
                else
                // 새 방식
                {
                    //int idx = patternList.IndexOf(patternInfo);
                    using (AlgoImage trainImage0 = trainImage.GetSubImage(subRect))
                    {
                        //    trainImage0.Save(Path.Combine(@"C:\temp\", string.Format("{0}_T.bmp", idx)));

                        //StringBuilder sb = null;
                        //if (subRect.Contains(new Point(5000, 2100)))
                        //{
                        //    using (AlgoImage patternImage0 = patternImage.GetSubImage(subRect))
                        //        patternImage0.Save(Path.Combine(@"C:\temp\", string.Format("{0}_P.bmp", idx)));
                        //    trainImage0.Save(Path.Combine(@"C:\temp\", string.Format("{0}_T.bmp", idx)));
                        //    sb = new StringBuilder();
                        //}
                        float[] proj = ip.Projection(trainImage0, Direction.Horizontal, ProjectionType.Mean);
                        float[] line;
                        Rectangle lineRect = new Rectangle(0, subRect.Height / 2, subRect.Width, 1);
                        using (AlgoImage trainImage1 = trainImage0.GetSubImage(lineRect))
                            line = ip.Projection(trainImage1, Direction.Horizontal, ProjectionType.Mean);

                        if (proj.Length > 10)
                        {
                            Debug.Assert(proj.Length == line.Length);

                            int len10 = proj.Length / 10;
                            int len50 = proj.Length / 2;
                            int len80 = proj.Length * 8 / 10;
                            int len90 = proj.Length * 9 / 10;

                            float[] line2 = new float[line.Length];
                            //sb?.AppendLine(string.Format("W{0}, H{1}, A{2}", patternInfo.RotateWidth, patternInfo.RotateHeight, patternInfo.RotateAngle));
                            for (int i = 0; i < proj.Length; i++)
                            {
                                line2[i] = proj[i] - line[i];
                                //sb?.AppendLine(string.Format("{0}, {1}, {2}", proj[i], line[i], line2[i]));
                            }
                            //if (sb != null)
                            //    File.WriteAllText(Path.Combine(@"C:\temp\", string.Format("{0}.txt", idx)), sb.ToString());

                            float th0 = line2.Take(len10).Average(); // 최초 10%의 평균
                            float th1 = line2.Skip(len10).Take(len80).Min(); // 중간 80%의 최소
                            float th2 = line2.Skip(len90).Average(); // 마지막 10%의 평균
                            if (th0 < th1 && th2 < th1)
                            {
                                int src = Array.FindLastIndex(line2, len50, f => f < th1);
                                int dst = Array.FindIndex(line2, len50, f => f < th1);
                                int dist = dst - src;
                                if (src >= 0 && dst >= 0 && dist > 0)
                                {
                                    //if (dist > patternInfo.WaistLength)
                                    //    trainImage0.Save(Path.Combine(@"C:\temp\", string.Format("{0}_T.bmp", idx)));
                                    patternInfo.WaistLength = dst - src;
                                }
                            }
                            //if (patternInfo.WaistLength == 0)
                            //{
                            //    StringBuilder sb = new StringBuilder();
                            //    Array.ForEach(line2, f => sb.AppendLine(f.ToString()));
                            //    File.WriteAllText(Path.Combine(@"C:\temp\", string.Format("{0}.txt", idx)), sb.ToString());
                            //    trainImage0.Save(Path.Combine(@"C:\temp\", string.Format("{0}_T.bmp", idx)));
                            //}
                            Debug.Assert(patternInfo.WaistLength >= 0);
                            Debug.Assert(patternInfo.WaistLengthRatio <= 1);
                        }
                    }
                }

                ThrowIfCancellationPending();
            }

            return patternList;
        }

        private List<SheetPatternGroup> GetPatternGroup(BackgroundWorker worker, AlgoImage trainImage, AlgoImage patternImage, List<PatternInfo> patternList)
        {
            TrainerParam param = TrainerBase.TrainerParam;

            //patternList.Sort((f, g) => f.Area.CompareTo(g.Area));
            SheetPatternGroupG curSheetPatternGroup = new SheetPatternGroupG(patternList);

            List<SheetPatternGroup> sheetPatternGroupList = curSheetPatternGroup.DivideSubGroup(param.ModelParam.SheetPatternGroupThreshold / 100);
            sheetPatternGroupList.ForEach(f =>
            {
                ThrowIfCancellationPending();
                f.CountRatio = (f.AverageArea * f.NumPattern) / patternList.Sum(g => g.Area);
                f.UpdateMaterImage(trainImage);
            });

            //trainImage.Save(@"C:\temp\trainImage.bmp");
            //patternImage.Save(@"C:\temp\patternImage.bmp");
            return sheetPatternGroupList;

            //List<SheetPatternGroup> sheetPatternGroupList2 = new List<SheetPatternGroup>();
            //while (sheetPatternGroupList.Count > 0)
            //{
            //    ThrowIfCancellationPending();
            //    SheetPatternGroup searchSheetPatternGroup = sheetPatternGroupList[0];
            //    sheetPatternGroupList.RemoveAt(0);

            //    List<SheetPatternGroup> samePatternGroupList = sheetPatternGroupList.FindAll(f =>
            //     {
            //         if (f == null)
            //             return false;

            //         return IsSamePattern(f.GetAverageBlobRect(), searchSheetPatternGroup, param.ModelParam.SheetPatternGroupThreshold / 100f, 1.0f, 1.0f);
            //     });
            //    sheetPatternGroupList.RemoveAll(f => samePatternGroupList.Contains(f));

            //    foreach (SheetPatternGroup samePatternGroup in samePatternGroupList)
            //        searchSheetPatternGroup.Merge(samePatternGroup);

            //    if (searchSheetPatternGroup.PatternList.Count() > 3)
            //    {
            //        searchSheetPatternGroup.CountRatio = searchSheetPatternGroup.PatternList.Count() * 1.0f / patternList.Count;
            //        searchSheetPatternGroup.UpdateMaterImage(trainImage);
            //        sheetPatternGroupList2.Add(searchSheetPatternGroup);
            //    }
            //}

            //return sheetPatternGroupList2;
        }

        private bool FindCrisscross(float[] v)
        {
            //지그제그 패턴: Y방향 프로젝션이 최소 - 최대 모양이 아니다.
            float maxVal = v.Max();
            float minxVal = v.Min();
            float bound = Math.Min(20, (maxVal - minxVal) / 2);

            int totalLength = v.Length;
            int maxLength = Array.FindAll(v, f => f > maxVal - 20).Length;
            int minLength = Array.FindAll(v, f => f < minxVal + 20).Length;
            float rate = (maxLength + minLength) * 1f / totalLength;
            return rate < 0.9f;
        }

        private void AdjustRect(AlgoImage debugImage, Rectangle[,] rectangles, Point posIdx, List<Point>[] hillLists, Size inflateSize, int v)
        {
            // 모든 패턴 격자를 inflate하면 겹치거나 1~2픽셀이 붕 뜨는 경우 발생.
            // 중심부터 inflate를 수행하며 Grid 정렬 수행.

            if (posIdx.X < 0 || posIdx.Y < 0 || posIdx.X >= rectangles.GetLength(1) || posIdx.Y >= rectangles.GetLength(0))
                return;
            if (rectangles[posIdx.Y, posIdx.X].IsEmpty == false)
                return;

            ImageProcessing ip = null;
            if (debugImage != null)
                ip = AlgorithmBuilder.GetImageProcessing(debugImage);

            Rectangle rectangle = Rectangle.FromLTRB(hillLists[0][posIdx.X].X, hillLists[1][posIdx.Y].X, hillLists[0][posIdx.X].Y, hillLists[1][posIdx.Y].Y);
            rectangle.Inflate(inflateSize);
            int diff;
            if (v < 0)
            {
                rectangles[posIdx.Y, posIdx.X] = rectangle;
            }
            else if (v == 0)   // left. compare with right
            {
                Rectangle right = rectangles[posIdx.Y, posIdx.X + 1];
                rectangle.Y = right.Y;
                rectangle.Height = right.Height;
                diff = right.Left - rectangle.Right;
                rectangle.Inflate(diff, 0);
                Debug.Assert(right.Left == rectangle.Right);

                rectangles[posIdx.Y, posIdx.X] = rectangle;
            }
            else if (v == 1)   // top. compare with bottom
            {
                Rectangle bottom = rectangles[posIdx.Y + 1, posIdx.X];
                rectangle.X = bottom.X;
                rectangle.Width = bottom.Width;
                //rectangle.Y = target.Y - rectangle.Height;
                //rectangle.Height = target.Y - rectangle.Y;
                diff = bottom.Top - rectangle.Bottom;
                rectangle.Inflate(0, diff);
                Debug.Assert(bottom.Top == rectangle.Bottom);

                rectangles[posIdx.Y, posIdx.X] = rectangle;
            }
            else if (v == 2)   // right. compare with left
            {
                Rectangle left = rectangles[posIdx.Y, posIdx.X - 1];
                rectangle.Y = left.Y;
                rectangle.Height = left.Height;
                diff = rectangle.Left - left.Right;
                rectangle.Inflate(diff, 0);
                Debug.Assert(left.Right == rectangle.Left);

                rectangles[posIdx.Y, posIdx.X] = rectangle;
            }
            else if (v == 3)   // bottom. compare with top
            {
                Rectangle top = rectangles[posIdx.Y - 1, posIdx.X];
                rectangle.X = top.X;
                rectangle.Width = top.Width;
                //rectangle.Y = target.Y + target.Height;
                //rectangle.Height = rectangle.Bottom - target.Bottom;
                diff = rectangle.Top - top.Bottom;
                rectangle.Inflate(0, diff);
                Debug.Assert(top.Bottom == rectangle.Top);

                rectangles[posIdx.Y, posIdx.X] = rectangle;
            }
            //Debug.Assert(rectangle.Width > 0 && rectangle.Height > 0);

            ip?.DrawRect(debugImage, rectangle, 255, false);

            //debugImage.Save(@"d:\temp\debugImage.bmp");

#if DEBUG
            if (posIdx.X + 1 < rectangles.GetLength(1) && rectangles[posIdx.Y, posIdx.X + 1].IsEmpty == false) Debug.Assert(rectangles[posIdx.Y, posIdx.X + 1].Top == rectangle.Top && rectangles[posIdx.Y, posIdx.X + 1].Bottom == rectangle.Bottom);
            if (posIdx.X - 1 >= 0 && rectangles[posIdx.Y, posIdx.X - 1].IsEmpty == false) Debug.Assert(rectangles[posIdx.Y, posIdx.X - 1].Top == rectangle.Top && rectangles[posIdx.Y, posIdx.X - 1].Bottom == rectangle.Bottom);
            if (posIdx.Y + 1 < rectangles.GetLength(0) && rectangles[posIdx.Y + 1, posIdx.X].IsEmpty == false) Debug.Assert(rectangles[posIdx.Y + 1, posIdx.X].Left == rectangle.Left && rectangles[posIdx.Y + 1, posIdx.X].Right == rectangle.Right);
            if (posIdx.Y - 1 >= 0 && rectangles[posIdx.Y - 1, posIdx.X].IsEmpty == false) Debug.Assert(rectangles[posIdx.Y - 1, posIdx.X].Left == rectangle.Left && rectangles[posIdx.Y - 1, posIdx.X].Right == rectangle.Right);
#endif

            AdjustRect(debugImage, rectangles, new Point(posIdx.X - 1, posIdx.Y), hillLists, inflateSize, 0);
            AdjustRect(debugImage, rectangles, new Point(posIdx.X, posIdx.Y - 1), hillLists, inflateSize, 1);
            AdjustRect(debugImage, rectangles, new Point(posIdx.X + 1, posIdx.Y), hillLists, inflateSize, 2);
            AdjustRect(debugImage, rectangles, new Point(posIdx.X, posIdx.Y + 1), hillLists, inflateSize, 3);

            if (v < 0)
                debugImage?.Save(@"d:\temp\debugImage.bmp");
        }

        protected int GetBinValue(AlgoImage algoImage)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            //int n = 16;
            //float width = algoImage.Width / n;
            //float[] value = new float[n];
            //for (int i = 0; i < n; i++)
            //{
            //    Rectangle region = Rectangle.FromLTRB((int)(width * i), 0, (int)(width * (i + 1)), algoImage.Height);
            //    AlgoImage subImage = algoImage.GetSubImage(region);
            //    //subImage.Save(@"D:\temp\subImage.bmp");
            //    value[i] = imageProcessing.Li(subImage);
            //    subImage.Dispose();
            //}
            //int result = (int)Math.Round((value.Sum() - value.Max() - value.Min()) / (value.Length - 2));
            //Array.Sort(value);
            //int result2 = (int)Math.Round((value.Take(value.Length/2).Average()));

            int result = (int)Math.Round(imageProcessing.GetGreyAverage(algoImage));
            return result;
        }

        public bool IsSamePattern(BlobRect blobRect, SheetPatternGroup patternGroup, float threshold, float imageScaleX, float imageScaleY)
        {
            TrainerParam param = TrainerBase.TrainerParam;

            float inverseScaleX = 1.0f / imageScaleX;
            float inverseScaleY = 1.0f / imageScaleY;

            //blobRect.Area *= inverseScaleX * inverseScaleY;
            //blobRect.CenterPt = new PointF(blobRect.CenterPt.X * inverseScaleX, blobRect.CenterPt.Y * inverseScaleY);
            //blobRect.CenterOffset = new PointF(blobRect.CenterOffset.X * inverseScaleX, blobRect.CenterOffset.Y * inverseScaleY);
            //blobRect.BoundingRect = new RectangleF(blobRect.BoundingRect.X * inverseScaleX, blobRect.BoundingRect.Y * inverseScaleY,
            //    blobRect.BoundingRect.Width * inverseScaleX, blobRect.BoundingRect.Height * inverseScaleY);

            float halfDiffTol = threshold / 2;

            double areaRatio = patternGroup.AverageArea / (patternGroup.AverageWidth * patternGroup.AverageHeight) * 100;
            //float maxArea = Math.Min(patternGroup.AverageWidth * (patternGroup.AverageHeight + threshold), (patternGroup.AverageWidth + threshold) * patternGroup.AverageHeight) * areaRatio;
            //float minArea = Math.Max(patternGroup.AverageWidth * (patternGroup.AverageHeight - threshold), (patternGroup.AverageWidth - threshold) * patternGroup.AverageHeight) * areaRatio;
            double minArea = Math.Max(0, areaRatio * (1 - halfDiffTol));
            double maxArea = Math.Min(100, areaRatio * (1 + halfDiffTol));

            //float minCenterOffsetX = patternGroup.AverageCenterOffsetX - halfDiffTol;
            //float maxCenterOffsetX = patternGroup.AverageCenterOffsetX + halfDiffTol;
            //float minCenterOffsetX = patternGroup.AverageCenterOffsetX * (1 - halfDiffTol);
            //float maxCenterOffsetX = patternGroup.AverageCenterOffsetX * (1 + halfDiffTol);

            //float minCenterOffsetY = patternGroup.AverageCenterOffsetY - halfDiffTol;
            //float maxCenterOffsetY = patternGroup.AverageCenterOffsetY + halfDiffTol;
            //float minCenterOffsetY = patternGroup.AverageCenterOffsetY * (1 - halfDiffTol);
            //float maxCenterOffsetY = patternGroup.AverageCenterOffsetY * (1 + halfDiffTol);

            //float minWidth = patternGroup.AverageWidth - threshold;
            //float maxWidth = patternGroup.AverageWidth + threshold;
            double minWidth = patternGroup.AverageWidth * (1 - halfDiffTol);
            double maxWidth = patternGroup.AverageWidth * (1 + halfDiffTol);

            //float minHeight = patternGroup.AverageHeight - threshold;
            //float maxHeight = patternGroup.AverageHeight + threshold;
            double minHeight = patternGroup.AverageHeight * (1 - halfDiffTol);
            double maxHeight = patternGroup.AverageHeight * (1 + halfDiffTol);

            if (minArea > blobRect.AreaRatio || maxArea < blobRect.AreaRatio)
                return false;

            //if (minCenterOffsetX > blobRect.CenterOffset.X || maxCenterOffsetX < blobRect.CenterOffset.X)
            //    return false;

            //if (minCenterOffsetY > blobRect.CenterOffset.Y || maxCenterOffsetY < blobRect.CenterOffset.Y)
            //    return false;

            if (minWidth > blobRect.BoundingRect.Width || maxWidth < blobRect.BoundingRect.Width)
                return false;

            if (minHeight > blobRect.BoundingRect.Height || maxHeight < blobRect.BoundingRect.Height)
                return false;

            return true;
        }
    }
}
