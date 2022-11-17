using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Vision;
using UniScanG.Data.Model;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Extender;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Trainer.Cluster;

namespace UniScanG.Gravure.Vision.Trainer
{
    public class Trainer : TrainerBase
    {
        public Trainer(ETrainerVersion trainerVersion) : base(trainerVersion) { }

        public override Point TrainBaseline(AlgoImage trainImage, Model model, int start, int end, DebugContextG debugContextG)
        {
            float step = (end - start) / 1f;

            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();

            // 기준위치 파악
            worker.ReportProgress(start + (int)(step * 0), StringManager.GetString(this.GetType().FullName, "Find Base Position"));
            Point basePosition = AlgorithmCommon.FindOffsetPosition(trainImage, null, Point.Empty, calibration, debugContextG);
            if (basePosition.X == 0 || basePosition.Y == 0)
                throw new Exception(StringManager.GetString("Cannot find Base Position."));

            worker.ReportProgress(end, StringManager.GetString(this.GetType().FullName, "Done"));
            return basePosition;
        }

        public override bool TrainPattern(AlgoImage trainImage, Model model, int start, int end, DebugContextG debugContextG)
        {
            float step = (end - start) / 4f;
            TrainerParam param = Trainer.TrainerParam;

            if (debugContextG.SaveDebugImage)
            {
                if (Directory.Exists(debugContextG.FullPath))
                    Directory.Delete(debugContextG.FullPath, true);
                Directory.CreateDirectory(debugContextG.FullPath);
            }

            AlgoImage patternImage = null;
            try
            {
                trainImage.Save("trainImage.bmp", debugContextG);
                ThrowIfCancellationPending();

                // 이진화 값 찾기
                worker.ReportProgress(start + (int)(step * 1), StringManager.GetString(this.GetType().FullName, "Binalize"));
                int binValue = GetBinValue(trainImage);
                if (binValue == 0)
                    throw new Exception("Pattern Threshold Value Error");

                param.ModelParam.BinValue = binValue;
                ThrowIfCancellationPending();

                // 보조영상 생성
                worker.ReportProgress(start + (int)(step * 2), StringManager.GetString(this.GetType().FullName, "Analyze Pattern"));
                patternImage = ImageBuilder.BuildSameTypeSize(trainImage); // 주 전극만 하얗게 칠한 영상
                CreateMaskImage(trainImage, patternImage);
                patternImage.Save("patternImage.bmp", debugContextG);
                ThrowIfCancellationPending();

                // 패턴 찾기. 패턴 그룹 설정. 주 패턴 선정
                worker.ReportProgress(start + (int)(step * 3), StringManager.GetString(this.GetType().FullName, "Grouping Patterns"));
                DebugContext patternGroupDebugContext = new DebugContext(debugContextG.SaveDebugImage, Path.Combine(debugContextG.FullPath, "PatternGroup"));
                List<SheetPatternGroup> patternGroupList = FindPatternGroup(worker, trainImage, patternImage, patternGroupDebugContext);
                ThrowIfCancellationPending();

                float chipShare100 = 0;
                if (patternGroupList.Count > 0)
                    chipShare100 = GetShareP(patternImage, patternGroupList[0]);
                ThrowIfCancellationPending();

                // 파라메터 설정
                {
                    // 티칭 값 적용
                    //calculatorParam.BinValue = binValue;
                    SystemManager.Instance().CurrentModel.ChipShare100p = chipShare100;

                    // Use 정보 유지
                    List<SheetPatternGroup> list1 = CalculatorBase.CalculatorParam.ModelParam.PatternGroupCollection.FindAll(f => f.CountRatio > 0.01);
                    List<SheetPatternGroup> list2 = patternGroupList.FindAll(f => f.CountRatio > 0.01);
                    if(list2.Count == list1.Count)
                        for (int i = 0; i < Math.Min(list1.Count, list2.Count); i++)
                            list2[i].Use = list1[i].Use;

                    CalculatorBase.CalculatorParam.ModelParam.PatternGroupCollection.Clear();
                    CalculatorBase.CalculatorParam.ModelParam.PatternGroupCollection.AddRange(patternGroupList);

                    //calculatorParam.RegionInfoList.ForEach(f => f.Dispose());
                    //calculatorParam.RegionInfoList.Clear();
                }

                worker.ReportProgress(end, StringManager.GetString(this.GetType().FullName, "Done"));
                return true;
            }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, $"Trainer::TrainPattern - {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                throw ex;
            }
#endif
            finally
            {
                patternImage?.Dispose();
            }
        }
        private float GetShareP(AlgoImage patternImage, SheetPatternGroup sheetPatternGroup)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(patternImage);
            Rectangle unionRect = Rectangle.Round(sheetPatternGroup.PatternList.Select(f => f.BoundingRect).Aggregate((f, g) => RectangleF.Union(f, g)));
            //using (AlgoImage debug = patternImage.Clip(unionRect))
            //    debug.Save(@"C:\temp\debug.bmp");

            PointF[] centerPt = new PointF[5]
                {
                    new PointF((5*unionRect.Left+1*unionRect.Right)/6, (unionRect.Top+unionRect.Bottom)/2),
                    new PointF((4*unionRect.Left+2*unionRect.Right)/6, (unionRect.Top+unionRect.Bottom)/2),
                    new PointF((3*unionRect.Left+3*unionRect.Right)/6, (unionRect.Top+unionRect.Bottom)/2),
                    new PointF((2*unionRect.Left+4*unionRect.Right)/6, (unionRect.Top+unionRect.Bottom)/2),
                    new PointF((1*unionRect.Left+5*unionRect.Right)/6, (unionRect.Top+unionRect.Bottom)/2)
                };

            double[] dd = centerPt.Select(f =>
            {
                PatternInfo pInfo = sheetPatternGroup.PatternList.OrderBy(g => MathHelper.GetLength(DrawingHelper.CenterPoint(g.BoundingRect), f)).First();
                Rectangle rect = Rectangle.Round(pInfo.BoundingRect);
                rect.Y = unionRect.Top;
                rect.Height = (int)unionRect.Height;

                using (AlgoImage clip = patternImage.GetSubImage(rect))
                {
                    //clip.Save(@"C:\temp\clip.bmp");
                    StatResult statResult = ip.GetStatValue(clip);
                    long sumPixelValue = (long)(statResult.average * statResult.count);
                    long whitePixelCount = sumPixelValue / 255;
                    return whitePixelCount * 100f / statResult.count;
                }
            }).ToArray();

            return (float)dd.Average();
        }

        public override bool TrainRegion(AlgoImage trainImage, Model model, int start, int end, DebugContext debugContext)
        {
            float step = (end - start) / 3f;

            TrainerParam param = Trainer.TrainerParam;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(trainImage);
            AlgoImage patternImage = null, majorPattrnImage = null;

            try
            {
                // 선택한 주 패턴들
                List<SheetPatternGroup> majorPatternGroupList = CalculatorBase.CalculatorParam.ModelParam.PatternGroupCollection.FindAll(f => f.Use);
                ThrowIfCancellationPending();

                // 패턴 이미지 가져옴. TrainPattern을 하지 않았으면 새로 만듬.
                worker.ReportProgress(start + (int)(step * 0), StringManager.GetString(this.GetType().FullName, "Generate Pattern Image"));
                if (patternImage == null)
                {
                    patternImage = ImageBuilder.BuildSameTypeSize(trainImage);
                    this.CreateMaskImage(trainImage, patternImage);
                }
                patternImage.Save("patternImage.bmp", debugContext);
                ThrowIfCancellationPending();

                // 주 패턴만 추출한 영상을 만듬
                worker.ReportProgress(start + (int)(step * 1), StringManager.GetString(this.GetType().FullName, "Extract Major Pattern"));
                majorPattrnImage = ImageBuilder.BuildSameTypeSize(patternImage);
                DrawPatternGroup(patternImage, majorPatternGroupList, majorPattrnImage);
                majorPattrnImage.Save("majorPattrnImage.bmp", debugContext);
                ThrowIfCancellationPending();

                // 버 생성
                worker.ReportProgress(start + (int)(step * 2), StringManager.GetString(this.GetType().FullName, "Generate Inspect Region Infomation"));
                BarCluster barCluster = BarCluster.GetBarCluster(this.TrainerVersion, param, this.worker);
                List<RegionInfoG> regionInfoList = barCluster.FindRegionInfoList(trainImage, majorPattrnImage, majorPatternGroupList, debugContext);

                if (regionInfoList.Count > 0)
                {
                    float averageInspRegionCnt = (float)regionInfoList.Average(f => f.InspectElementList.Count);
                    regionInfoList.ForEach(f => f.Use = f.InspectElementList.Count > averageInspRegionCnt / 2);
                }

                // 티칭 이전/이후 바의 개수와 위치가 비슷한가?
                if (CalculatorBase.CalculatorParam.ModelParam.RegionInfoCollection.Count <= regionInfoList.Count)
                {
                    for (int i = 0; i < CalculatorBase.CalculatorParam.ModelParam.RegionInfoCollection.Count; i++)
                    {
                        RegionInfoG regionInfoG = CalculatorBase.CalculatorParam.ModelParam.RegionInfoCollection[i];
                        float[] scores = regionInfoList.Select(f =>
                        {
                            Rectangle inflated = Rectangle.Intersect(regionInfoG.Region, f.Region);
                            return DrawingHelper.GetArea(inflated) * 1f / DrawingHelper.GetArea(regionInfoG.Region);
                        }).ToArray();

                        float maxScore = scores.Max();
                        if (maxScore > 0.85)
                        {
                            // 위치가 비슷할 경우 일부정보 유지
                            RegionInfoG newRegionInfoG = regionInfoList[Array.IndexOf(scores, maxScore)];

                            newRegionInfoG.Use &= regionInfoG.Use;
                            newRegionInfoG.BlockRectList.AddRange(regionInfoG.BlockRectList);
                            newRegionInfoG.PassRectList.AddRange(regionInfoG.PassRectList);
                            newRegionInfoG.CreticalPointList.AddRange(regionInfoG.CreticalPointList);
                        }
                    }
                }

                CalculatorBase.CalculatorParam.ModelParam.RegionInfoCollection.Clear();
                CalculatorBase.CalculatorParam.ModelParam.RegionInfoCollection.AddRange(regionInfoList);

                model.RCITrainResult.Clear();
                worker.ReportProgress(end, StringManager.GetString(this.GetType().FullName, "Done"));
                return true;
            }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, $"Trainer::TrainRegion - {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                throw ex;
            }
#endif
            finally
            {
                patternImage?.Dispose();
                majorPattrnImage?.Dispose();
            }
        }


        public override bool TrainExtention(AlgoImage trainImage, Model model, Point baseOffset, int start, int end, DebugContext debugContext)
        {
            // Point basePos, Point baseOffset, 
            TrainerParam trainerParam = TrainerBase.TrainerParam;

            WatcherParam watcherParam = Watcher.WatcherParam;
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(trainImage);
            int sleepMs = 50;


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

            BaseXSearchDir baseXSearchDir = SheetFinderBase.SheetFinderBaseParam.GetBaseXSearchDir();

            int totalSteps = Math.Max(1, observeCount + monChipCount + marginCount + transformCount.Width * transformCount.Height);
            float step = (end - start) * 1f / totalSteps;
            int curStep = 0;
            worker.ReportProgress((int)Math.Round(start + (step * curStep)),
                string.Format(StringManager.GetString(this.GetType().FullName, "Monitoring Point ({0}/{1})"), curStep, totalSteps));
            Thread.Sleep(sleepMs);

            List<RegionInfoG> regionInfoList = CalculatorBase.CalculatorParam.ModelParam.RegionInfoCollection;
            List<RegionInfoG> useRegionInfoList = regionInfoList.FindAll(f => f.Use);

            Action ProgressUpdated = new Action(() =>
            {
                curStep++;
                worker.ReportProgress((int)Math.Round(start + (step * curStep)),
                    string.Format(StringManager.GetString(this.GetType().FullName, "Monitoring Point ({0}/{1})"), curStep, totalSteps));
                Thread.Sleep(sleepMs);
            });

            try
            {
                ExtCollectionTrainParam trainParam = new ExtCollectionTrainParam()
                {
                    TrainImage = trainImage,
                    RegionInfoList = regionInfoList,
                    BasePos = model.CalculatorModelParam.BasePosition,
                    BaseOffset = baseOffset,
                    BaseXSearchDir = baseXSearchDir,
                    Calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault()
                };

                // 관찰 기능
                Array.ForEach(observerCollections, f =>
                {
                    f.Train(trainParam, ProgressUpdated, debugContext);
                });

                // 정지화상
                stopImgCollection.Train(trainParam, ProgressUpdated, debugContext);
                //int watchItemChipCount = stopImgCollection.Count;
                ////if (watchItemChipCount < monChipCount)
                //// 설정된 Chip 영역이 없으면, 3개의 Chip 영역 생성
                //{
                //    stopImgCollection.Clear();
                //    for (int i = 0; i < Math.Min(useRegionInfoList.Count, monChipCount); i++)
                //    {
                //        RegionInfoG regionInfoG = useRegionInfoList[i];
                //        DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, string.Format("MonitoringItems_{0}", i)));

                //        ExtItem watchItem = stopImgCollection.CreateItem();
                //        if (GetWatchItem(watchItem, trainImage, regionInfoG, new PointF(0.3f, 0.3f), newDebugContext))
                //        {
                //            watchItem.Index = i;
                //            watchItem.Name = string.Format("PT.{0}", i);
                //            watchItem.ContainerIndex = CalculatorBase.CalculatorParam.ModelParam.RegionInfoCollection.IndexOf(regionInfoG);
                //            stopImgCollection.Add(watchItem);
                //        }
                //        curStep++;
                //        worker.ReportProgress((int)Math.Round(start + (step * curStep)),
                //            string.Format(StringManager.GetString(this.GetType().FullName, "Monitoring Point ({0}/{1})"), curStep, totalSteps));
                //        Thread.Sleep(sleepMs);
                //    }

                //    stopImgCollection.RemoveAll(f => f == null);
                //    curStep = monChipCount;
                //}

                // 마진측정
                marginCollection.Train(trainParam, ProgressUpdated, debugContext);
                //if (useRegionInfoList.Count > 0 && marginCount > 0)
                //{
                //    marginCollection.Param.AveragePole = useRegionInfoList.Average(f => f.PoleAvg);
                //    marginCollection.Param.AverageCoating = useRegionInfoList.Average(f => f.DielectricAvg);

                //    // 마진 검사 영역
                //    int curMarginCount = marginCollection.Count;

                //    BaseXSearchDir baseXSearchDir = SheetFinderBase.SheetFinderBaseParam.GetBaseXSearchDir();

                //    int sideXPos = 0;
                //    int centerXPos = trainImage.Width;
                //    float sideXLim = 0.15f;
                //    float centerXLim = 1 - sideXLim;
                //    if (baseXSearchDir == BaseXSearchDir.Right2Left)
                //    {
                //        sideXPos = trainImage.Width;
                //        centerXPos = 0;
                //        sideXLim = 0.9f;
                //        centerXLim = 0.1f;
                //    }

                //    {
                //        //marginCollection.Clear();
                //        marginCollection.RemoveAll(f => ((Margin)f).MarginPos != Margin.EMarginPos.CUSTOM);
                //        //marginCollection.RemoveAt(0);
                //        //marginCollection.RemoveAt(1);
                //        //marginCollection.RemoveAt(2);

                //        Point refPos;
                //        PointF position = PointF.Empty;
                //        for (int i = 0; i < marginCollection.Param.Count; i++)
                //        {
                //            DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, string.Format("MarginItems_{0}", i)));
                //            Margin.EMarginPos marginPos = Margin.EMarginPos.CUSTOM;
                //            switch (i)
                //            {
                //                case 0:
                //                    // Center-Middle
                //                    marginPos = Margin.EMarginPos.CM;
                //                    refPos = new Point(centerXPos, trainImage.Height / 2);
                //                    position = new PointF(centerXLim, -1f);
                //                    break;
                //                case 1:
                //                    // Side-Top
                //                    marginPos = Margin.EMarginPos.ST;
                //                    refPos = new Point(sideXPos, 0);
                //                    position = new PointF(sideXLim, 0.2f);
                //                    break;
                //                case 2:
                //                    // Side-Bottom
                //                    marginPos = Margin.EMarginPos.SB;
                //                    refPos = new Point(sideXPos, trainImage.Height);
                //                    position = new PointF(sideXLim, 0.8f);
                //                    break;
                //                default:
                //                    continue;
                //            }
                //            float[] distances = useRegionInfoList.Select(f => MathHelper.GetLength(f.Region.Location, refPos)).ToArray();
                //            float minimumDist = distances.Min();
                //            int minimumIdx = Array.FindIndex(distances, f => f == minimumDist);
                //            RegionInfoG regionInfoG = useRegionInfoList[minimumIdx];
                //            if (position.Y < 0)
                //            // 바 내부에서 가장 적절한 위치를 찾음 (세로방향)
                //            {
                //                float hLimit = Math.Max(sideXLim, centerXLim);
                //                float lLimit = Math.Min(sideXLim, centerXLim);
                //                float h = (hLimit - lLimit) / (regionInfoG.Region.Height) * (refPos.Y - regionInfoG.Region.Top) + lLimit;
                //                position.Y = Math.Min(hLimit, Math.Max(lLimit, h));
                //            }

                //            ExtItem extItem = marginCollection.CreateItem(marginPos);
                //            if (GetWatchItem(extItem, trainImage, regionInfoG, position, newDebugContext))
                //            {
                //                extItem.Name = marginPos.ToString();
                //                extItem.Index = i;
                //                extItem.ContainerIndex = CalculatorBase.CalculatorParam.ModelParam.RegionInfoCollection.IndexOf(regionInfoG);
                //                marginCollection.Add(extItem);
                //            }
                //            curStep++;
                //            worker.ReportProgress((int)Math.Round(start + (step * curStep)),
                //                string.Format(StringManager.GetString(this.GetType().FullName, "Monitoring Point ({0}/{1})"), curStep, totalSteps));
                //            Thread.Sleep(sleepMs);
                //        }
                //        marginCollection.Sort();
                //        curStep = monChipCount + marginCollection.Param.Count;

                //    }
                //}
                //else
                //{
                //    marginCollection.Clear();
                //    marginCollection.Param.AveragePole = marginCollection.Param.AverageCoating = -1;
                //}

                // 변형측정
                transformCollection.Train(trainParam, ProgressUpdated, debugContext);
                //if (transformCount.Width > 0 && transformCount.Height > 0)
                //{
                //    Rectangle validRect = new Rectangle(Point.Empty, CalculatorBase.CalculatorParam.ModelParam.SheetSizePx);
                //    transformCollection.Clear();

                //    SheetFinderBaseParam param = SheetFinderBase.SheetFinderBaseParam;
                //    int srcX = basePos.X;
                //    int dstX = (param.GetBaseXSearchDir() == BaseXSearchDir.Left2Right) ? trainImage.Width : 0;
                //    int srcY = basePos.Y;
                //    int dstY = trainImage.Height - basePos.Y;

                //    Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
                //    Size count = transformCollection.Param.Count;
                //    SizeF sizeUm = transformCollection.Param.SizeUm;
                //    Size sizePx = Size.Round(calibration.WorldToPixel(transformCollection.Param.SizeUm));

                //    int[] centerX = new int[count.Width];
                //    for (int i = 0; i < count.Width; i++)
                //        centerX[i] = (int)Math.Round(i * (dstX - srcX) * 1f / count.Width) + srcX;
                //    int[] centerY = new int[count.Height];
                //    for (int i = 0; i < count.Height; i++)
                //        centerY[i] = (int)Math.Round(i * (dstY - srcY) * 1f / (count.Height - 1)) + srcY;

                //    int itemIdx = 0;
                //    for (int x = 0; x < count.Width; x++)
                //    {
                //        for (int y = 0; y < count.Height; y++)
                //        {
                //            string name = string.Format("[X{0}/Y{1}]", x, y);

                //            if (transformCollection.Exist(name))
                //            {
                //                ExtItem existItem = transformCollection.Get(name);
                //                existItem.Offset(baseOffset);
                //                continue;
                //            }

                //            Point centerPt = new Point(centerX[x], centerY[y]);
                //            Rectangle rect = DrawingHelper.FromCenterSize(centerPt, sizePx);
                //            rect.Intersect(validRect);

                //            ExtItem watchItem = transformCollection.CreateItem();
                //            using (AlgoImage masterAlgoImage = trainImage.GetSubImage(rect))
                //            {
                //                watchItem.Use = true;
                //                watchItem.Index = itemIdx;
                //                watchItem.ExtType = ExtType.PMVariance;
                //                //watchItem.Name = WatchType.PMVariance.ToString();
                //                watchItem.Name = name;
                //                watchItem.MasterRectangle = rect;
                //                watchItem.MasterImageD = masterAlgoImage.ToImageD();
                //            }
                //            if ((x == 0) || (y == 0 || y == count.Height - 1))
                //            {
                //                transformCollection.Add(watchItem);
                //                itemIdx++;
                //            }

                //            curStep++;
                //            worker.ReportProgress((int)Math.Round(start + (step * curStep)),
                //                string.Format(StringManager.GetString(this.GetType().FullName, "Monitoring Point ({0}/{1})"), curStep, totalSteps));
                //            Thread.Sleep(sleepMs);
                //        }
                //        transformCollection.Sort();
                //    }
                //    // 상-하 기다란 패턴에 대해서는 예외 발생.
                //    if (false)
                //    {
                //        List<SheetPatternGroup> usePatternGroup = CalculatorBase.CalculatorParam.ModelParam.PatternGroupCollection.FindAll(f => f.Use);
                //        SizeF chipSize = new SizeF((float)usePatternGroup.Average(f => f.AverageWidth), (float)usePatternGroup.Average(f => f.AverageHeight));
                //        SizeF masterSize = DrawingHelper.Mul(chipSize, 1.2f);

                //        validRect.Inflate(-CalculatorBase.CalculatorParam.ModelParam.BasePosition.X, -CalculatorBase.CalculatorParam.ModelParam.BasePosition.Y);

                //        float width = validRect.Width * 1f / transformCount.Width;
                //        float heigth = validRect.Height * 1f / transformCount.Height;

                //        for (int y = 0; y < transformCount.Height; y++)
                //        {
                //            float cenY = validRect.Top + ((y + 0.5f) * heigth);
                //            for (int x = 0; x < transformCount.Width; x++)
                //            {
                //                float cenX = validRect.Left + ((x + 0.5f) * width);
                //                PointF cenPt = new PointF(cenX, cenY);
                //                Rectangle masterRect = Rectangle.Round(DrawingHelper.FromCenterSize(cenPt, masterSize));
                //                //Rectangle searchRect = Rectangle.Inflate(masterRect, masterRect.Width / 2, masterRect.Height / 2);
                //                ImageD masterImageD;
                //                using (AlgoImage masterAlgoImage = trainImage.GetSubImage(masterRect))
                //                    masterImageD = masterAlgoImage.ToImageD();

                //                ExtItem watchItem = transformCollection.CreateItem();
                //                watchItem.Use = true;
                //                watchItem.Index = x + y * transformCount.Width;
                //                watchItem.ExtType = ExtType.PMVariance;
                //                watchItem.Name = ExtType.PMVariance.ToString();
                //                //watchItem.ClipRectangle = searchRect;
                //                watchItem.MasterRectangle = masterRect;
                //                watchItem.MasterImageD = masterImageD;

                //                transformCollection.Add(watchItem);

                //                curStep++;
                //                worker.ReportProgress((int)Math.Round(start + (step * curStep)),
                //                    string.Format(StringManager.GetString(this.GetType().FullName, "Monitoring Point ({0}/{1})"), curStep, totalSteps));
                //                Thread.Sleep(sleepMs);
                //            }
                //        }
                //    }
                //    curStep = monChipCount + marginCollection.Param.Count + (transformCollection.Param.Count.Width * transformCollection.Param.Count.Height);
                //}
                //else
                //{
                //    transformCollection.Clear();
                //}
            }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, $"Trainer::TrainMonitoring - {ex.Message}{Environment.NewLine}{ex.StackTrace}");
                throw ex;
            }
#endif
            finally
            {
                worker.ReportProgress(end, StringManager.GetString(this.GetType().FullName, "Done"));
            }

            return true;
        }

    }


}
