using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Vision;
using DynMvp.Base;
using DynMvp.UI;
using System.Drawing;
using System.Diagnostics;
using UniEye.Base.Settings;
using DynMvp.Data;
using System.Threading;
using System.Runtime.InteropServices;
using UniEye.Base;
using System.IO;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Data;
using UniScanG.Vision;
using UniScanG.Data;
using UniScanG.Common.Settings;
using UniScanG.Gravure.Vision.Trainer;

namespace UniScanG.Gravure.Vision.Calculator.V1
{
    public class CalculatorV1 : CalculatorBase
    {
        //public static string TypeName { get { return "Calculator"; } }

        public CalculatorV1()
        {
            this.AlgorithmName = CalculatorBase.TypeName;
            this.param = null;

            //ThreadPool.SetMaxThreads(100, 100);
        }

        #region Abstract
        public override DynMvp.Vision.Algorithm Clone()
        {
            CalculatorV1 clone = new CalculatorV1();
            clone.CopyFrom(this);
            return clone;
        }

        public override AlgorithmParam CreateParam()
        {
            return new CalculatorParam(true);
        }

        public override ProcessBufferSetG CreateProcessingBuffer(float scaleFactor, bool isMultiLayer, int width, int height)
        {
            return new ProcessBufferSetG1(scaleFactor, isMultiLayer, width, height);
        }
        #endregion

        #region Override
        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new CalculatorResult();
        }
        #endregion

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            SheetInspectParam sheetInspectParam = algorithmInspectParam as SheetInspectParam;
            Calibration calibration = sheetInspectParam.CameraCalibration;
            CalculatorParam calculatorParam = CalculatorBase.CalculatorParam;
            TrainerParam trainerParam = AlgorithmPool.Instance().GetAlgorithm(Trainer.TrainerBase.TypeName)?.Param as TrainerParam;

            AlgoImage fullImage = null;
            AlgoImage scaleImage = null;
            AlgoImage inspImage = null;
            ProcessBufferSetG1 processBufferSet = null;
            AlgoImage[] bufTemp = null;
            AlgoImage bufResult = null;
            AlgoImage binalImage = null;
            RegionInfoG inspRegionInfo = sheetInspectParam.TargetRegionInfo as RegionInfoG;
            CancellationToken cancellationToken = sheetInspectParam.CancellationToken;

            bool disposeNeed;
            if (sheetInspectParam != null)
            {
                processBufferSet = sheetInspectParam.ProcessBufferSet as ProcessBufferSetG1;
                if (processBufferSet == null)
                    return null;    // null 리턴시 skip

                fullImage = processBufferSet.AlgoImage;
                scaleImage = processBufferSet.ScaledImage;
                inspImage = processBufferSet.CalculatorInsp;
                binalImage = processBufferSet.CalculatorResultBinal;

                if (scaleImage == null)
                    scaleImage = inspImage;

                if (inspImage == null)
                    inspImage = scaleImage = fullImage;
                               
                bufTemp = processBufferSet.CalculatorTemp;
                bufResult = processBufferSet.CalculatorResultGray;
                disposeNeed = false;
            }
            else
            {
                inspImage = ImageBuilder.Build(this.GetAlgorithmType(), algorithmInspectParam.ClipImage, ImageType.Grey);
                scaleImage = inspImage.Clone();
                bufTemp = new AlgoImage[4] { ImageBuilder.BuildSameTypeSize(inspImage), ImageBuilder.BuildSameTypeSize(inspImage), ImageBuilder.BuildSameTypeSize(inspImage), ImageBuilder.BuildSameTypeSize(inspImage) };
                bufResult = ImageBuilder.BuildSameTypeSize(inspImage);
                binalImage = ImageBuilder.BuildSameTypeSize(inspImage);
                disposeNeed = true;
            }

            DebugContext debugContext = new DebugContext(algorithmInspectParam.DebugContext.SaveDebugImage, Path.Combine(algorithmInspectParam.DebugContext.FullPath, "Calculator"));
            inspImage.Save("SheetImage.bmp", debugContext);

            // Create Preview image
            Bitmap previewBitmap = null;
            float ratio = SystemTypeSettings.Instance().ResizeRatio;
            Task resizeTask = Task.Run(() =>
            {
                AlgoImage fullSheetImage = processBufferSet.AlgoImage;
                AlgoImage resizeSheetImage = null;
                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(fullSheetImage);

                if (fullSheetImage is SheetImageSet)
                {
                    resizeSheetImage = ((SheetImageSet)fullSheetImage).GetFullImage(ratio);
                }
                else
                {
                    int newWidth = (int)(fullSheetImage.Width * ratio);
                    int newHeigth = (int)(fullSheetImage.Height * ratio);
                    resizeSheetImage = ImageBuilder.Build(fullSheetImage.LibraryType, fullSheetImage.ImageType, newWidth, newHeigth);
                    imageProcessing.Resize(fullSheetImage, resizeSheetImage);
                }
                resizeSheetImage.Save("ResizeSheetImage.bmp", debugContext);

                ImageD previewImageD = resizeSheetImage.ToImageD();
                previewBitmap = previewImageD.ToBitmap();
                //previewBitmap.Save(@"C:\UniScan\Gravure_Inspector\Temp\Calculator\previewBitmap.bmp");
                previewImageD.Dispose();
                resizeSheetImage.Dispose();
            });

            List<RegionInfoG> regionInfoList = calculatorParam.ModelParam.RegionInfoCollection;
            List<AlgorithmResult> algorithmResultList = new List<AlgorithmResult>();
            Point diffPos = Point.Empty;
            try
            {
                int binValue = trainerParam.ModelParam.BinValue + trainerParam.ModelParam.BinValueOffset;
                Point basePos = calculatorParam.ModelParam.BasePosition;
                Point foundPos = Point.Empty;
                diffPos = AlgorithmCommon.FindOffsetPosition(scaleImage, processBufferSet.EdgeFinderBuffer, calculatorParam.ModelParam.BasePosition, calibration, new DebugContextG(debugContext));

                //Debug.WriteLine(string.Format("DiffPos: {0:F2},{1:F2}", diffPos.X, diffPos.Y));
                LogHelper.Debug(LoggerType.Inspection, string.Format("SheetOffset: {0:F2},{1:F2}", diffPos.X, diffPos.Y));

                int indexOf = calculatorParam.ModelParam.RegionInfoCollection.IndexOf(inspRegionInfo);
                int startRegionId = Math.Max(0, indexOf);
                int endRegionId = indexOf < 0 ? calculatorParam.ModelParam.RegionInfoCollection.Count : startRegionId + 1;
                //if (inspRegionInfo == null) // 모든 영역 검사
                {
                    bool streamAble = (bufResult.ImageType == ImageType.Gpu);
                    if (calculatorParam.UseMultiThread)
                    {
                        List<ImageProcessing> imageProcessingList = new List<ImageProcessing>();
                        Parallel.For(startRegionId, endRegionId, i =>
                        {
                            RegionInfoG regionInfo = regionInfoList[i];
                            if (regionInfo.Use)
                            {
                                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(bufResult);
                                if(streamAble)
                                {
                                    imageProcessing = ImageProcessing.Create(bufResult.LibraryType);
                                    imageProcessingList.Add(imageProcessing);
                                }
                                DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, string.Format("RegionInfo_{0}", i)));
                                InspectRegion(inspImage, bufTemp, bufResult, regionInfo, diffPos, imageProcessing, cancellationToken, newDebugContext);
                            }
                        });
                        imageProcessingList.ForEach(f => f.WaitStream());
                    }
                    else
                    {
                        for (int i = startRegionId; i < endRegionId; i++)
                        {
                            RegionInfoG regionInfo = regionInfoList[i];
                            if (regionInfo.Use)
                            {
                                DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, string.Format("RegionInfo_{0}", i)));
                                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(bufResult);
                                InspectRegion(inspImage, bufTemp, bufResult, regionInfo, diffPos, imageProcessing, cancellationToken, newDebugContext);
                            }
                        }
                    }
                }
                //else // 지정된 영역만 검사
                //{
                //    DebugContext newDebugContext = new DebugContext(true, Path.Combine(debugContext.FullPath, "RegionInfo_Test"));
                //    InspectRegion(sheetImage, bufTemp, bufResult, inspRegionInfo, diffPos, cancellationToken, newDebugContext);
                //}
                bufResult.Save("CalculatorResult.bmp", debugContext);
                Threshold(bufResult, binalImage, calculatorParam.ModelParam.SensitiveParam.Max);
            }
#if DEBUG==false
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("Error Occure in Calculator::Inspect ({0})", ex.Message));
                DynMvp.UI.Touch.MessageForm.Show(null, sb.ToString());

                sb.AppendLine(ex.StackTrace);
                LogHelper.Error(LoggerType.Inspection, sb.ToString());
            }
#endif
            finally { }

            if (resizeTask.Status != TaskStatus.Created)
                resizeTask.Wait();

            processBufferSet.SetPrevBitmap(previewBitmap);

            CalculatorResult calculatorResult = (CalculatorResult)CreateAlgorithmResult();
            calculatorResult.SheetSize = processBufferSet.PatternSizePx;
            calculatorResult.OffsetSet.PatternOffset.OffsetF = diffPos;
            calculatorResult.SubResultList.AddRange(algorithmResultList);

            if (disposeNeed)
            {
                inspImage.Dispose();
                scaleImage.Dispose();
                Array.ForEach(bufTemp, f =>f.Dispose());
                bufResult.Dispose();
                binalImage.Dispose();
            }
         
            return calculatorResult;
        }

        private void Threshold(AlgoImage bufResult, AlgoImage binalImage, int sensitivity)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(bufResult);
            Rectangle imageRect = new Rectangle(Point.Empty, bufResult.Size);

            imageProcessing.Binarize(bufResult, binalImage, sensitivity);
            //imageProcessing.Close(subBinalImage, 1);
        }

        //private Point AlignSheet(AlgoImage algoImage, Point basePosition)
        //{
        //    Point diffPos = Point.Empty;
        //    //algoImage.Save(@"c:\temp\algoImage.bmp");
        //    SheetFinderBase sheerFinder = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName) as SheetFinderBase;
        //    if (sheerFinder == null)
        //        return Point.Empty;

        //    int foundPosX = sheerFinder.FindBasePosition(algoImage, Direction.Horizontal, 20);
        //    int foundPosY = sheerFinder.FindBasePosition(algoImage, Direction.Vertical, 20);
        //    if (foundPosX >= 0 && foundPosY >= 0)
        //        diffPos = new Point(foundPosX - basePosition.X, foundPosY - basePosition.Y);

        //    return diffPos;
        //}

        private void InspectRegion(AlgoImage sheetImage, AlgoImage[] bufTemp, AlgoImage bufResult, RegionInfoG regionInfo, Point diffPos, ImageProcessing imageProcessing, CancellationToken cancellationToken, DebugContext debugContext)
        {
            Rectangle region = regionInfo.Region;
            region.Offset(diffPos.X, diffPos.Y);
            Rectangle imageRect = new Rectangle(Point.Empty, sheetImage.Size);
            Rectangle adjustRect = Rectangle.Intersect(imageRect, region);
            if (adjustRect == region)
            {
                AlgoImage subRegionImage = sheetImage.GetSubImage(region);
                AlgoImage[] subBufImage = Array.ConvertAll(bufTemp, f => f.GetSubImage(region));
                AlgoImage subResultImage = bufResult.GetSubImage(region);
                subRegionImage.Save("RegionImage.bmp", debugContext);

                InspectRegion(regionInfo, subRegionImage, subBufImage, subResultImage, imageProcessing, cancellationToken, debugContext);

                subRegionImage.Dispose();
                Array.ForEach(subBufImage, f => f.Dispose());
                subResultImage.Dispose();
            }
            else
            {
                LogHelper.Error(LoggerType.Error, string.Format("Region Rect {0} is out of image", debugContext.FullPath));
            }
        }

        private void InspectRegion(RegionInfoG regionInfo, AlgoImage regionImage, AlgoImage[] bufTemp, AlgoImage bufResult, ImageProcessing imageProcessing, CancellationToken cancellationToken, DebugContext debugContext)
        {
            LogHelper.Debug(LoggerType.Inspection, string.Format("Calculator::InspectRegion Start"));

            CalculatorParam calculatorParam = CalculatorBase.CalculatorParam;
            Rectangle imageRect = new Rectangle(Point.Empty, regionImage.Size);
            Stopwatch sw = Stopwatch.StartNew();
            regionImage.Save("regionImage.bmp", debugContext);
            //regionImage.Save(@"C:\temp\regionImage.bmp");

            List<InspectElement> inspectElementList = regionInfo.InspectElementList;
            //List<Point> alighOffsetList = GetAlignOffset(regionImage, regionInfo, debugContext);

            if (regionInfo.OddEvenPair == false)
            {
                List<InspectElement> inspElementList = new List<InspectElement>();
                inspElementList.AddRange(inspectElementList);
                //for (int i = 0; i < inspectElementList.Count; i++)
                //    inspElementList.Add(inspectElementList[i]);
                InspectLineSet(regionInfo, inspElementList, regionImage, bufTemp, bufResult, imageProcessing, cancellationToken, debugContext);
            }
            else
            {
                List<InspectElement> oddInspElementList = new List<InspectElement>();
                List<InspectElement> evenInspElementList = new List<InspectElement>();
                for (int i = 0; i < inspectElementList.Count; i++)
                    if (i % 2 == 0)
                        evenInspElementList.Add(inspectElementList[i]);
                    else
                        oddInspElementList.Add(inspectElementList[i]);

                //if (calculatorParam.ParallelOperation)
                //{
                //    Task[] tasks = new Task[]
                //    {
                //        Task.Run(() => InspectLineSet(regionInfo,evenInspElementList, regionImage, bufTemp, bufResult,imageProcessing, cancellationToken, new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, "Even")))),
                //        Task.Run(() => InspectLineSet(regionInfo,oddInspElementList, regionImage, bufTemp, bufResult,imageProcessing, cancellationToken, new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, "Odd"))))
                //    };
                //    Array.ForEach(tasks, f => f.Wait());
                //}
                //else
                {
                    InspectLineSet(regionInfo, evenInspElementList, regionImage, bufTemp, bufResult, imageProcessing, cancellationToken, new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, "Even")));
                    InspectLineSet(regionInfo, oddInspElementList, regionImage, bufTemp, bufResult, imageProcessing, cancellationToken, new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, "Odd")));
                }
            }

            bufResult.Save("Result.bmp", debugContext);
            //bufResult.Save(@"C:\temp\bufResult.bmp");
            sw.Stop();
            LogHelper.Debug(LoggerType.Inspection, string.Format("Calculator::InspectRegion End. {0}ms", sw.ElapsedMilliseconds));
        }

        private List<Point> GetAlignOffset(AlgoImage regionImage, RegionInfoG regionInfoG, DebugContext debugContext)
        {
            CalculatorParam calculatorParam = CalculatorBase.CalculatorParam;
            if (calculatorParam.InBarAlign == false)
            {
                Point[] alighOffsets = new Point[regionInfoG.InspectElementList.Count];
                Array.Clear(alighOffsets, 0, alighOffsets.Length);
                return alighOffsets.ToList();
            }

            SheetFinderBase sheerFinder = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName) as SheetFinderBase;
            Rectangle imageRect = new Rectangle(Point.Empty, regionImage.Size);

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(regionImage);

            int[] offsetX = new int[regionInfoG.InspectElementList.Count];
            int[] offsetY = new int[regionInfoG.InspectElementList.Count];
            Array.Clear(offsetX, 0, offsetX.Length);
            Array.Clear(offsetY, 0, offsetY.Length);

            // Aligh X
            if (calculatorParam.InBarAlign && regionInfoG.GroupDirection == GroupDirection.Vertical)
            {
                Size patCount = new Size(regionInfoG.AdjPatRegionList.GetLength(1), regionInfoG.AdjPatRegionList.GetLength(0));
                int centerPatPos = patCount.Height / 2;

                Rectangle rect = regionInfoG.PatRegionList[centerPatPos, 0];
                for (int i = 1; i < patCount.Width; i++)
                    rect = Rectangle.Union(rect, regionInfoG.PatRegionList[centerPatPos, i]);
                rect.Inflate(regionImage.Width / 2, 0);
                rect.Intersect(imageRect);

                // 세로 가운데 1개 패턴 가로줄 이미지
                AlgoImage processImage = regionImage.GetSubImage(rect);

                float[] data = ip.Projection(processImage, Direction.Horizontal, ProjectionType.Mean);
                processImage.Dispose();
                List<Point> hillList = AlgorithmCommon.FindHill(data, data.Average(), true);
                if (hillList.Count >= regionInfoG.InspectElementList.Count)
                {
                    float[] hillCenters = hillList.ConvertAll(f => (f.X + f.Y) / 2.0f).ToArray();
                    float[] regionCenters = regionInfoG.InspectElementList.ConvertAll(f => DrawingHelper.CenterPoint(new RectangleF(f.Rectangle.Location,f.Rectangle.Size)).X).ToArray();
                    float[] distError = new float[regionCenters.Length];
                    for (int i = 0; i < regionCenters.Length; i++)
                        distError[i] = Math.Abs(regionCenters[0] - hillCenters[i]);
                    float minError = distError.Min();
                    int srcIdx = Array.FindIndex(distError, f => f == minError);
                    int dstIdx = srcIdx + regionInfoG.InspectElementList.Count - 1;
                    if (dstIdx <= regionInfoG.InspectElementList.Count)
                    {
                        int len = dstIdx - srcIdx + 1;
                        for (int i = 0; i < len; i++)
                            offsetX[i] = (int)Math.Round(hillCenters[srcIdx + i] - regionCenters[i]);
                    }
                }
            }

            // Align Y
            if (calculatorParam.InBarAlign)
            {
                for (int i = 0; i < regionInfoG.InspectElementList.Count; i++)
                {
                    InspectElement inspRegionElement = regionInfoG.InspectElementList[i];

                    if (inspRegionElement.OffsetYBase >= 0)
                    {
                        int basePos = inspRegionElement.OffsetYBase;

                        Rectangle subRect = inspRegionElement.Rectangle;
                        subRect.Y = 0;
                        subRect.Height /= 3;
                        AlgoImage subimage = regionImage.GetSubImage(subRect);
                        //subimage.Save(@"d:\temp\subimage.bmp");
                        int foundPos = InspectElement.GetOffsetValue(subimage, Direction.Vertical);
                        if (foundPos >= 0)
                            offsetY[i]= foundPos - basePos;
                        subimage.Dispose();
                    }
                }
            }

            List<Point> offsetList = new List<Point>();
            for (int i = 0; i < regionInfoG.InspectElementList.Count; i++)
            {
                InspectElement inspRegionElement = regionInfoG.InspectElementList[i];
                Point offset = Point.Empty;
                switch (regionInfoG.GroupDirection)
                {
                    case GroupDirection.Horizontal:
                        offset.X = offsetY[i];
                        break;
                    case GroupDirection.Vertical:
                        offset.X = offsetX[i];
                        offset.Y = offsetY[i];
                        break;
                }

                offsetList.Add(offset);
            }
            return offsetList;
        }

        private List<InspectElement> AdjustInspRegion(AlgoImage regionImage, RegionInfoG regionInfo, bool oddEvenPair)
        {
            List<InspectElement> inspRegionList = new List<InspectElement>(regionInfo.InspectElementList);

            Rectangle projRegion = new Rectangle(0, 0, regionImage.Width / 5, regionImage.Height);
            if (projRegion.Width > 0 && projRegion.Height > 0)
            {
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(regionImage);
                AlgoImage projImage = regionImage.GetSubImage(projRegion);
                float[] projData = ip.Projection(projImage, Direction.Vertical, ProjectionType.Mean);
                projImage.Dispose();
                List<Point> patRegionList = new List<Point>();
                Trainer.TrainerParam trainerParam = AlgorithmPool.Instance().GetAlgorithm(Trainer.TrainerBase.TypeName).Param as Trainer.TrainerParam;
                if (oddEvenPair)
                    patRegionList = AlgorithmCommon.FindHill3(projData, trainerParam.KernalSize, trainerParam.MinLineIntensity, trainerParam.DiffrentialThreshold);
                else
                    patRegionList = AlgorithmCommon.FindHill(projData, trainerParam.DiffrentialThreshold);

                if (patRegionList.Count == regionInfo.PatRegionList.GetLength(0))
                {
                    int srcIdx = 0, dstIdx = -1;
                    for (int i = 0; i < inspRegionList.Count; i++)
                    {
                        dstIdx = srcIdx + 1;
                        int a = (regionInfo.PatRegionList[srcIdx, 0].Top + regionInfo.PatRegionList[dstIdx, 0].Bottom) / 2;
                        int b = (patRegionList[srcIdx].X + patRegionList[dstIdx].Y) / 2;
                        int offset = b - a;
                        Rectangle inspRegion = inspRegionList[i].Rectangle;
                        inspRegion.Offset(0, offset);
                        inspRegionList[i].Offset(new Point(0, offset));

                        srcIdx = dstIdx + 1;
                    }
                }
            }
            return inspRegionList;
        }

        private void InspectLineSet(RegionInfoG regionInfo, List<InspectElement> inspElementList, AlgoImage regionImage, AlgoImage[] bufTemp, AlgoImage bufResult, ImageProcessing imageProcessing, CancellationToken cancellationToken, DebugContext debugContext)
        {
            LogHelper.Debug(LoggerType.Inspection, string.Format("Calculator::InspectLineSet Start"));

            //CalculatorParam cp = this.param as CalculatorParam;
            //int lineCount = inspElementList.Count;
            //int splitCount = lineCount / 35 + 1;
            //if (cp.ParallelOperation && splitCount > 1 && bufTemp.Length > 1)
            //// 줄 개수가 많으면 나누어서 검사함
            //{
            //    List<Task> taskList = new List<Task>();
            //    float linePerThread = lineCount * 1.0f / splitCount;
            //    int srcLine = 0;
            //    for (int i = 0; i < splitCount; i++)
            //    {
            //        int dstLine = (int)Math.Min(lineCount, Math.Round(linePerThread * (i + 1)));
            //        Task task = StartInspectLineSetTask(regionInfo, inspElementList, srcLine, dstLine, regionImage, bufTemp, bufResult, cancellationToken,
            //            new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, string.Format("T{0}", i))));
            //        taskList.Add(task);
            //        srcLine = dstLine;
            //    }
            //    taskList.ForEach(f => f.Wait());
            //    return;
            //}
            //else
            {
                InspectLineSet(regionInfo, inspElementList, 0, inspElementList.Count, regionImage, bufTemp, bufResult, imageProcessing, cancellationToken, debugContext);
            }

            LogHelper.Debug(LoggerType.Inspection, string.Format("Calculator::InspectLineSet End"));
        }

        private Task StartInspectLineSetTask(RegionInfoG regionInfo, List<InspectElement> inspList, int src, int dst, AlgoImage regionImage, AlgoImage[] bufTemp, AlgoImage bufResult, ImageProcessing imageProcessing, CancellationToken cancellationToken, DebugContext debugContext)
        {
            return Task.Run(() => InspectLineSet(regionInfo, inspList, src, dst, regionImage, bufTemp, bufResult, imageProcessing, cancellationToken, debugContext));
        }

        private void InspectLineSet(RegionInfoG regionInfo, List<InspectElement> inspList, int src, int dst, AlgoImage regionImage, AlgoImage[] bufTemp, AlgoImage bufResult, ImageProcessing imageProcessing, CancellationToken cancellationToken, DebugContext debugContext)
        {
            if (inspList.Count < 3)
                return;

            CalculatorParam cp = CalculatorBase.CalculatorParam;

            int boundaryPairStep = cp.BoundaryPairStep;

            List<InspectElement> careLineRectList = inspList.FindAll(f => f.HasDontcare == false);

            // 엣지 추출
            bool edgeFound = false;
            AlgoImage baseEdgeImage = null;

            if (true)
            {
                int baseLine = (src + dst) / 2 + 1;
                AlgoImage baseLineImage = regionImage.GetSubImage(inspList[baseLine].Rectangle);
                baseEdgeImage = ImageBuilder.Build(CalculatorBase.TypeName, baseLineImage.Size);
                Debug.Assert(baseEdgeImage.Size == baseLineImage.Size);
                if (cp.ModelParam.EdgeParam.EdgeFindMethod == EdgeParam.EEdgeFindMethod.Soble)
                {
                    AlgoImage tempAlgoImage = ImageBuilder.Build(CalculatorBase.TypeName, baseLineImage.Size); //bufTemp[0].GetSubImage(inspList[tempLine].Rectangle);
                    edgeFound = GetEdgeImageSoble(baseEdgeImage, baseLineImage, tempAlgoImage, imageProcessing, debugContext);
                    tempAlgoImage?.Dispose();
                }
                else
                {
                    edgeFound = GetEdgeImagePJ(baseEdgeImage, baseLineImage, debugContext);
                }
                baseLineImage.Dispose();
                baseEdgeImage.Save("baseEdgeImage.bmp", debugContext);
                //baseEdgeImage.Save(@"C:\temp\baseEdgeImage_M.bmp");
            }

            if (cp.ModelParam.IgnoreSideLine)
            {
                src++;
                dst--;
            }

            for (int i = src; i < dst; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                // 검사 할 줄
                InspectElement curElement = inspList[i];
                int curInx = careLineRectList.IndexOf(curElement);
                InspectElement upperElement, lowerElement;
                if (cp.AdaptivePairing == false || curElement.HasDontcare)
                // 돈케어 있음 -> 그냥 짝지음.
                {
                    upperElement = (i == 0) ? inspList[i + boundaryPairStep] : inspList[i - 1];
                    lowerElement = (i == inspList.Count - 1) ? inspList[i - boundaryPairStep] : inspList[i + 1];
                }
                else
                // 돈케어 없음 -> 돈케어 없는것 끼리 짝지음.
                {
                    if (careLineRectList.Count < 3)
                        continue;


                    upperElement = (curInx == 0) ? careLineRectList[curInx + boundaryPairStep] : careLineRectList[curInx - 1];
                    lowerElement = (curInx == careLineRectList.Count - 1) ? careLineRectList[curInx - boundaryPairStep] : careLineRectList[curInx + 1];
                }

                DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, System.IO.Path.Combine(debugContext.FullPath, string.Format("Line{0}", curInx)));
                InspectLine(regionImage, edgeFound ? baseEdgeImage : null, bufTemp, bufResult, curElement, upperElement, lowerElement, imageProcessing, newDebugContext);
            }
            baseEdgeImage?.Dispose();
        }

        private List<Tuple<Direction, int, int>> GetEdgePosition(AlgoImage baseLineImage)
        {
            List<Tuple<Direction, int, int>> edgePositionList = new List<Tuple<Direction, int, int>>();

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(baseLineImage);

            float[] dataV = ip.Projection(baseLineImage, Direction.Vertical, ProjectionType.Mean);
            List<Point> hillListV = AlgorithmCommon.FindHill(dataV, -1, true);
            foreach (Point hill in hillListV)
            {
                edgePositionList.Add(new Tuple<Direction, int, int>(Direction.Horizontal, hill.X, +1));
                edgePositionList.Add(new Tuple<Direction, int, int>(Direction.Horizontal, hill.Y, -1));
            }

            float[] dataH = ip.Projection(baseLineImage, Direction.Horizontal, ProjectionType.Mean);
            List<Point> hillListH = AlgorithmCommon.FindHill(dataH, 64, true);
            foreach (Point hill in hillListH)
            {
                edgePositionList.Add(new Tuple<Direction, int, int>(Direction.Vertical, hill.X, +1));
                edgePositionList.Add(new Tuple<Direction, int, int>(Direction.Vertical, hill.Y, -1));
            }

            return edgePositionList;
        }

        private bool GetEdgeImagePJ(AlgoImage baseEdgeImage, AlgoImage baseLineImage, DebugContext debugContext)
        {
            CalculatorParam cp = CalculatorBase.CalculatorParam;
            EdgeParam edgeParam = cp.ModelParam.EdgeParam;
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(baseLineImage);
            Size drawSize = Size.Empty;
            if (edgeParam.Width == 0 || edgeParam.Value == 0)
                return false;

            Debug.Assert(baseEdgeImage.Size == baseLineImage.Size);
            baseLineImage.Save("BaseLineImage.bmp", debugContext);
            //baseLineImage.Save(@"d:\temp\BaseLineImage.bmp");

            Rectangle[] vertSkipRect = new Rectangle[2];

            float[] dataH = ip.Projection(baseLineImage, Direction.Horizontal, ProjectionType.Mean);
            //List<Point> hillListH = AlgorithmCommon.FindHill(dataH, -1, true,debugContext);
            List<Point> hillListH = AlgorithmCommon.FindHill2(dataH, 5, debugContext);
            drawSize = new Size(edgeParam.Width* 3, baseLineImage.Height);
            if (hillListH.Count > 0)
            {
                int maxWidth = hillListH.Max(f => f.Y - f.X);
                Point foundHill = hillListH.Find(f => (f.Y - f.X) == maxWidth);
                Rectangle srcEdgeRect = new Rectangle(new Point(foundHill.X - 2 * edgeParam.Width, 0), drawSize);
                ip.DrawRect(baseEdgeImage, srcEdgeRect, edgeParam.Value, true);
                vertSkipRect[0] = new Rectangle(foundHill.X - 3, 0, 5, baseLineImage.Height);

                Rectangle dstEdgeRect = new Rectangle(new Point(foundHill.Y - edgeParam.Width, 0), drawSize);
                ip.DrawRect(baseEdgeImage, dstEdgeRect, edgeParam.Value, true);
                vertSkipRect[1] = new Rectangle(foundHill.Y - 2, 0, 5, baseLineImage.Height);
            }

            float[] dataV = ip.Projection(baseLineImage, Direction.Vertical, ProjectionType.Mean);
            List<Point> hillListV = AlgorithmCommon.FindHill(dataV, -1, true);
            drawSize = new Size(baseLineImage.Width, edgeParam.Width * 3);
            foreach (Point hill in hillListV)
            {
                Rectangle srcEdgeRect = new Rectangle(new Point(0, hill.X - edgeParam.Width), drawSize);
                ip.DrawRect(baseEdgeImage, srcEdgeRect, edgeParam.Value, true);
                ip.DrawRect(baseEdgeImage, new Rectangle(0,hill.X - 3, baseLineImage.Width, 5), 255, true);

                Rectangle dstEdgeRect = new Rectangle(new Point(0, hill.Y - 2 * edgeParam.Width), drawSize);
                ip.DrawRect(baseEdgeImage, dstEdgeRect, edgeParam.Value, true);
                ip.DrawRect(baseEdgeImage, new Rectangle(0, hill.Y - 3, baseLineImage.Width, 5), 255, true);
            }

            Array.ForEach(vertSkipRect, f => ip.DrawRect(baseEdgeImage, f, 255, true));

            baseEdgeImage.Save("BaseEdgeImage.bmp", debugContext);
            //edgeImage.Save(@"d:\temp\BaseEdgeImage.bmp");
            return true;
        }

        private bool GetEdgeImageSoble(AlgoImage baseEdgeImage, AlgoImage baseLineImage, AlgoImage tempAlgoImage, ImageProcessing imageProcessing, DebugContext debugContext)
        {
            CalculatorParam cp = CalculatorBase.CalculatorParam;
            EdgeParam edgeParam = cp.ModelParam.EdgeParam;
            //ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(baseLineImage);

            Size drawSize = Size.Empty;
            if (edgeParam.Width == 0 || edgeParam.Value == 0)
                return false;

            Debug.Assert(baseEdgeImage.Size == baseLineImage.Size);

            baseLineImage.Save("LineImage.bmp", debugContext);

            imageProcessing.Average(baseLineImage, baseEdgeImage);
            tempAlgoImage.Save("EdgeImage-1Average.bmp", debugContext);

            imageProcessing.Sobel(baseEdgeImage, tempAlgoImage);
            baseEdgeImage.Save("EdgeImage-2Sobel.bmp", debugContext);

            imageProcessing.Binarize(tempAlgoImage, baseEdgeImage);
            tempAlgoImage.Save("EdgeImage-3Binarize.bmp", debugContext);

            imageProcessing.Close(baseEdgeImage, tempAlgoImage, 2);
            baseEdgeImage.Save("EdgeImage-4Close.bmp", debugContext);

            imageProcessing.Dilate(tempAlgoImage, baseEdgeImage, edgeParam.Width);
            tempAlgoImage.Save("EdgeImage-5Dilate1.bmp", debugContext);

            //imageProcessing.Dilate(tempAlgoImage, baseEdgeImage, cp.EdgeWidth);
            //baseEdgeImage.Save("EdgeImage-5Dilate2.bmp", debugContext);

            //imageProcessing.WeightedAdd(new AlgoImage[] { baseEdgeImage , tempAlgoImage}, baseEdgeImage);
            //imageProcessing.WaitStream();

            baseEdgeImage.Save("EdgeImage-6WeightedAdd.bmp", debugContext);

            return true;
        }

        private void InspectLine(AlgoImage inspImage, AlgoImage baseEdgeImage, AlgoImage[] tempImage, AlgoImage resultImage, 
            InspectElement inspElement, InspectElement upperElement, InspectElement lowerElement, ImageProcessing imageProcessing, DebugContext debugContext)
        {
            Rectangle inspRect = inspElement.Rectangle;
            Rectangle upperRect = upperElement.Rectangle;
            Rectangle lowerRect = lowerElement.Rectangle;

            int minHeight = Math.Min(Math.Min(inspRect.Height, upperRect.Height), lowerRect.Height);
            int minWidth = Math.Min(Math.Min(inspRect.Width, upperRect.Width), lowerRect.Width);
            inspRect.Size = upperRect.Size = lowerRect.Size = new Size(minWidth, minHeight);

            //ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(inspImage);
            //if (imageProcessing == null)
            //    imageProcessing = AlgorithmBuilder.GetImageProcessing(inspImage);

            AlgoImage upper = null, inspp = null, lower = null;
            AlgoImage[] temp = new AlgoImage[4];
            AlgoImage result = null;
            try
            {
                upper = inspImage.GetSubImage(upperRect);
                inspp = inspImage.GetSubImage(inspRect);
                lower = inspImage.GetSubImage(lowerRect);
                upper.Save("0-1. A.bmp", debugContext);
                inspp.Save("0-2. B.bmp", debugContext);
                lower.Save("0-3. C.bmp", debugContext);

                if(tempImage.Length == 1)
                {
                    temp[0] = tempImage[0].GetSubImage(upperRect);
                    temp[1] = tempImage[0].GetSubImage(inspRect);
                    temp[2] = tempImage[0].GetSubImage(lowerRect);
                    temp[3] = resultImage.GetSubImage(inspRect);
                    result = temp[3];
                }
                else 
                {
                    temp[0] = tempImage[0].GetSubImage(inspRect);
                    temp[1] = tempImage[1].GetSubImage(inspRect);
                    temp[2] = tempImage[2].GetSubImage(inspRect);
                    temp[3] = tempImage[3].GetSubImage(inspRect);
                    result = resultImage.GetSubImage(inspRect);
                }

                imageProcessing.Subtract(inspp, upper, temp[0]); //notABS
                temp[0].Save("1-1. B-A.bmp", debugContext);
                imageProcessing.Subtract(inspp, lower, temp[1]); //notABS
                temp[1].Save("1-2. B-C.bmp", debugContext);
                imageProcessing.Min(temp[0], temp[1], temp[2]); //어두운 불량
                temp[2].Save("1-3. Min.bmp", debugContext);

                imageProcessing.Subtract(upper, inspp, temp[0]); //notABS
                temp[0].Save("2-1. A-B.bmp", debugContext);
                imageProcessing.Subtract(lower, inspp, temp[1]); //notABS
                temp[1].Save("2-2. C-B.bmp", debugContext);
                imageProcessing.Min(temp[0], temp[1], temp[3]); //밝은 불량
                temp[3].Save("2-3. Min.bmp", debugContext);

                imageProcessing.Max(temp[2], temp[3], result);
                result.Save("3. Max.bmp", debugContext);

                //  엣지영역 제거
                if (baseEdgeImage != null)
                    imageProcessing.Subtract(result, baseEdgeImage, result);
                result.Save("4. RemoveEdge.bmp", debugContext);

                // dontcare 영역 제거
                List<Rectangle> doncareRectList = new List<Rectangle>();

                //doncareRectList.AddRange(inspElement.DontcareRects);
                CalculatorParam calculatorParam = CalculatorBase.CalculatorParam;
                if (calculatorParam.ModelParam.IgnoreMethod == CalculatorParam.EIgnoreMethod.Neighborhood)
                {
                    //doncareRectList.AddRange(inspElement.DontcareRects);
                    //doncareRectList.AddRange(upperElement.DontcareRects);
                    //doncareRectList.AddRange(lowerElement.DontcareRects);
                }

                Rectangle imageRect = new Rectangle(Point.Empty, result.Size);
                doncareRectList.ForEach(f =>
                {
                    f.Inflate(50, 50);
                    f.Intersect(imageRect);
                    imageProcessing.Clear(result, f, Color.Black);
                });
                result.Save("5. RemoveDontcare.bmp", debugContext);

                //imageProcessing.WaitStream();
            }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, string.Format("Exception Occure - Calculator::InspectLine - {0}", ex.Message));
            }
#endif
            finally
            {
                upper?.Dispose();
                inspp?.Dispose();
                lower?.Dispose();
                Array.ForEach(temp, f => f.Dispose());
                result?.Dispose();
            }
        }

        private void RemoveEdge(AlgoImage inspp, AlgoImage result, List<Tuple<Direction, int,int>> edgePositionList, DebugContext debugContext)
        {
            CalculatorParam calculatorParam = CalculatorBase.CalculatorParam;
            EdgeParam edgeParam = calculatorParam.ModelParam.EdgeParam;
            if (edgeParam.Width == 0 || edgeParam.Value == 0)
                return;

            AlgoImage debugImage = null;
            if (debugContext.SaveDebugImage)
                debugImage = inspp.ConvertTo(inspp.LibraryType, ImageType.Color);

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(inspp);
            Rectangle imageRect = new Rectangle(Point.Empty, inspp.Size);
            foreach (Tuple<Direction, int,int> edgePosition in edgePositionList)
            {
                Rectangle rect = Rectangle.Empty;
                int inflate = edgeParam.Width * 2;
                int offset = edgeParam.Width * edgePosition.Item3;
                switch (edgePosition.Item1)
                {
                    case Direction.Horizontal:
                        rect = Rectangle.FromLTRB(0, edgePosition.Item2, inspp.Width, edgePosition.Item2);
                        break;
                    case Direction.Vertical:
                        rect = Rectangle.FromLTRB(edgePosition.Item2, 0, edgePosition.Item2, inspp.Height);
                        break;
                }
                if (debugImage != null)
                    ip.DrawRect(debugImage, rect, Color.Yellow.ToArgb(), false);

                rect.Inflate(inflate, inflate);
                rect.Offset(offset, offset);
                rect.Intersect(imageRect);
                
                AlgoImage subResultAlgoImage = result.GetSubImage(rect);
                ip.Subtract(subResultAlgoImage, subResultAlgoImage, edgeParam.Value);
                subResultAlgoImage.Dispose();

                if (debugImage != null)
                    ip.DrawRect(debugImage, rect, Color.Red.ToArgb(), false);
            }

            if (debugImage != null)
            {
                debugImage.Save("RemoveDebug.bmp", debugContext);
                debugImage.Dispose();
            }
        }

        //private void RemoveEdge(AlgoImage inspp, AlgoImage result, AlgoImage baseEdgeImage, DebugContext debugContext)
        //{
        //    CalculatorParam calculatorParam = this.param as CalculatorParam;
        //    if (calculatorParam.EdgeWidth == 0 || calculatorParam.EdgeValue == 0)
        //        return;

        //    AlgoImage debugImage = null;
        //    if(debugContext.SaveDebugImage)
        //        debugImage = inspp.ConvertToMilImage(ImageType.Color);

        //    ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(inspp);
        //    //Rectangle insppRect = new Rectangle(Point.Empty, inspp.Size);
        //    //Rectangle resultRect = new Rectangle(Point.Empty, result.Size);
        //    //Rectangle edgeRect = new Rectangle(Point.Empty, baseEdgeImage.Size);

        //    ip.Subtract(inspp, baseEdgeImage, result);
        //}



        #region Old

        //public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        //{
        //Stopwatch sw = new Stopwatch();
        //sw.Start();

        //AlgorithmResult algorithmResult = CreateAlgorithmResult();

        //SheetCheckerInspectParam inspectParam = algorithmInspectParam as SheetCheckerInspectParam;
        //SheetImageSet inspImageSet = inspectParam.InspImageSet;
        //ProcessBuffer buffer = inspectParam.ProcessBuffer;

        //bool saveImage = ((SamsungElectroSettings.Instance().SaveInspectionDebugData & SaveDebugData.Image) > 0);
        //DebugContext debugContext = new DebugContext(saveImage, Path.Combine(inspectParam.DebugContext.Path, "Calculate"));

        //TrainerParam param = (this.param as SheetCheckerParam).TrainerParam;
        //Size refImageSize = inspImageSet.GetImageSize();
        //int fidOffset = inspImageSet.fidXPos - param.FiducialXPos;

        //float fillRatio = 0.1f;

        //AlgoImage processedImages1 = buffer.ImageBuffer1;
        //AlgoImage processedImages2 = buffer.ImageBuffer2;
        //processedImages1.Clear();
        //processedImages2.Clear();

        //AlgoImage processedImages4 = buffer.SmallBuffer;
        //processedImages4.Clear();

        //List<SheetRange> toalProjectionRangeList = new List<SheetRange>();

        //ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(processedImages1);

        //int patternHeight = (int)Math.Round(param.RefPattern.PatternGroup.AverageHeight);

        ////foreach (ProjectionRegion projectionRegion in param.ProjectionRegionList)
        //Parallel.ForEach(param.ProjectionRegionList, (projectionRegion) =>
        // {
        //     Rectangle clipRect = projectionRegion.Region;
        //     clipRect.Offset(fidOffset, 0);
        //     clipRect.Intersect(Rectangle.FromLTRB(0, 0, refImageSize.Width, refImageSize.Height));
        //     if (clipRect.Width == 0 || clipRect.Height == 0)
        //         return;

        //     AlgoImage inspImage = inspImageSet.GetSubImage(clipRect);
        //     AlgoImage resultImage = processedImages1.GetSubImage(clipRect);
        //     AlgoImage tempImage = processedImages2.GetSubImage(clipRect);

        //     // Process to ROI
        //     DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, System.IO.Path.Combine(debugContext.Path, string.Format("ROI{0}", projectionRegion.Id)));
        //     inspImage.Save(String.Format("ROI.bmp"), newDebugContext);
        //     List<Rectangle> inspLineRectList = ProcessROI(inspImage, resultImage, tempImage, param.AutoThresholdValue, patternHeight, fillRatio, param.DefectThreshold, newDebugContext);
        //     lock (inspectParam.SheetCheckerAlgorithmResult.ResultValueList)
        //         inspectParam.SheetCheckerAlgorithmResult.ResultValueList.Add(new AlgorithmResultValue(String.Format("INSP_LINE_RECT_LIST_{0}", projectionRegion.Id), inspLineRectList));

        //     Rectangle smallClipRect = new Rectangle(clipRect.X / 5, clipRect.Y / 5, clipRect.Width / 5, clipRect.Height / 5);
        //     AlgoImage contourImage = processedImages4.GetSubImage(smallClipRect);

        //     imageProcessing.Resize(inspImage, contourImage, 0.2);

        //     imageProcessing.Binarize(contourImage, contourImage, param.AutoThresholdValue, true);

        //     BlobParam blobParam = new BlobParam();
        //     blobParam.AreaMin = param.MinPatternArea / 16.0f;
        //     blobParam.SelectLabelValue = true;

        //     BlobRectList blobRectList = imageProcessing.Blob(contourImage, blobParam);

        //     contourImage.Clear();
        //     DrawBlobOption drawBlobOption = new DrawBlobOption();
        //     drawBlobOption.SelectBlobContour = true;

        //     imageProcessing.DrawBlob(contourImage, blobRectList, null, drawBlobOption);

        //     imageProcessing.Dilate(contourImage, 1);

        //     imageProcessing.Not(contourImage, contourImage);
        //     imageProcessing.Resize(contourImage, tempImage, 5);

        //     imageProcessing.And(resultImage, tempImage, resultImage);

        //     //tempImage.Save("tempImage.bmp", new DebugContext(true, "c:\\"));
        //     //blackResultImage.Save("blackResultImage.bmp", new DebugContext(true, "c:\\"));

        //     blobRectList.Dispose();

        //     contourImage.Dispose();

        //     inspImage.Dispose();
        //     resultImage.Dispose();
        //     tempImage.Dispose();
        // });

        //processedImages1.Save("processedImages1.bmp", debugContext);
        //processedImages2.Save("processedImages2.bmp", debugContext);

        //sw.Stop();
        //algorithmResult.SpandTime = sw.Elapsed;//new TimeSpan(sw.ElapsedTicks);
        //algorithmResult.Good = true;
        ////LogHelper.Debug(LoggerType.Inspection, string.Format("SheetCheckerStepProcess Inspection Time: {0}[ms]", sw.ElapsedMilliseconds));

        //return algorithmResult;
        //}

        //private List<Rectangle> ProcessROI(AlgoImage inspImage, AlgoImage resultImage, AlgoImage tempImage, int triangle, int patternHeight, float fillRatio, int defectThreshold, DebugContext debugContext)
        //{
        //    
        //}



        //private void ProcessLine2(AlgoImage inspImage, AlgoImage resultImage, AlgoImage tempImage, Rectangle insppRect, Rectangle upperRect, Rectangle lowerRect, DebugContext debugContext)
        //{
        //    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(inspImage);

        //    AlgoImage upper = inspImage.GetSubImage(upperRect);
        //    AlgoImage inspp = inspImage.GetSubImage(insppRect);
        //    AlgoImage lower = inspImage.GetSubImage(lowerRect);

        //    AlgoImage tempUpper = tempImage.GetSubImage(upperRect);
        //    AlgoImage tempLower = tempImage.GetSubImage(lowerRect);

        //    AlgoImage result = resultImage.GetSubImage(insppRect);

        //    imageProcessing.Subtract(inspp, upper, tempUpper, true);
        //    imageProcessing.Subtract(inspp, lower, tempLower, true);
        //    imageProcessing.Min(tempUpper, tempLower, result);

        //    upper.Dispose();
        //    inspp.Dispose();
        //    lower.Dispose();
        //    tempUpper.Dispose();
        //    tempLower.Dispose();
        //    result.Dispose();
        //}

        //private List<Rectangle> GetInspLineRectList(AlgoImage inspImage, AlgoImage tempImage, int triangle, int patternHeight, float fillRatio, DebugContext debugContext)
        //{
        //    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(inspImage);

        //    // V-Projection for Y-Align
        //    Rectangle vProjectionRect = new Rectangle();

        //    int projectionSize2 = inspImage.Width / 6;
        //    Point vProjCenter = new Point(inspImage.Width / 2, inspImage.Height / 2);
        //    vProjectionRect = Rectangle.FromLTRB(vProjCenter.X - projectionSize2, 0, vProjCenter.X + projectionSize2, inspImage.Height);
        //    vProjectionRect.Intersect(new Rectangle(Point.Empty, inspImage.Size));

        //    AlgoImage inspSubImage = inspImage.Clip(vProjectionRect);
        //    inspSubImage.Save(String.Format("GetInspLineList_inspSubImage.bmp"), debugContext);

        //    AlgoImage tempSubImage = tempImage.GetSubImage(vProjectionRect);
        //    imageProcessing.Binarize(inspSubImage, tempSubImage, triangle, true);
        //    tempSubImage.Save(String.Format("GetInspLineList_tempSubImage.bmp"), debugContext);

        //    float[] vProjectionData = imageProcessing.Projection(tempSubImage, Direction.Vertical, ProjectionType.Sum);

        //    List<Rectangle> inspLineRectList = new List<Rectangle>();
        //    List<SheetRange> sheetRangeList = SheetChecker.GetProjectionRangeList(vProjectionData, patternHeight, fillRatio, vProjectionRect.Width);
        //    int inflateHeight = 5;
        //    if (sheetRangeList.Count > 0)
        //    {
        //        int rectHeight = (int)Math.Round(sheetRangeList.Average(f => f.Length)) + inflateHeight;

        //        foreach (SheetRange sheetRange in sheetRangeList)
        //        {
        //            inspLineRectList.Add(new Rectangle(0, sheetRange.StartPos + ((sheetRange.EndPos - sheetRange.StartPos) / 2) - (rectHeight / 2), inspImage.Width, rectHeight));
        //            //inspLineRectList.Add(Rectangle.FromLTRB(0, sheetRange.StartPos, inspImage.Width, sheetRange.EndPos));
        //        }
        //    }
        //    inspLineRectList.RemoveAll(f => f.Top < 0 || f.Bottom >= inspImage.Height);

        //    tempSubImage.Clear();
        //    inspSubImage.Dispose();
        //    tempSubImage.Dispose();

        //    return inspLineRectList;
        //}

        //public override Algorithm Clone()
        //{
        //    throw new NotImplementedException();
        //}

        //public override List<AlgorithmResultValue> GetResultValues()
        //{
        //    throw new NotImplementedException();
        //}

        //public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        //{
        //    throw new NotImplementedException();
        //}

        //public override string GetAlgorithmType()
        //{
        //    throw new NotImplementedException();
        //}

        //public override string GetAlgorithmTypeShort()
        //{
        //    throw new NotImplementedException();
        //}

        //public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion


        #region Garbige
        /*
         * Rectangle refImageRect, inspImageRect;
            bool ok=GetCalculateRect(refImageSize, new Size(inspW, inspH), new Point(fidOffset, nextInspStartLine), out refImageRect, out inspImageRect);
            nextInspStartLine += refImageRect.Height;
            if (ok == false)
            {
                Debug.Assert(false, "Calculate Area is worng");
                continue;
            }

            AlgoImage refChildImage = refImage.GetSubImage(refImageRect);
            AlgoImage maskChildImage = maskImage.GetSubImage(refImageRect);
            AlgoImage inspChildImage = inspImage.GetSubImage(inspImageRect);

            debugContext.TimeProfile.Add("InspectStep", "CreateChildImage");

            if (useSimd)
            {
                processedImages[i] = ImageBuilder.Build(inspChildImage.LibraryType, inspChildImage.ImageType, inspChildImage.Size.Width, inspChildImage.Size.Height);
                IntPtr inspPtr = inspChildImage.GetImagePtr();
                IntPtr reffPtr = refChildImage.GetImagePtr();
                IntPtr maskPtr = maskChildImage.GetImagePtr();
                IntPtr destPtr = processedImages[i].GetImagePtr();
                GravuerSIMD.GravuerCalculateWithSIMD(inspPtr, reffPtr, maskPtr, destPtr, inspChildImage.Pitch, processedImages[i].Pitch, (byte)param.BinarizeValue, inspChildImage.Size.Width, inspChildImage.Size.Height);
            }
            else
            {
                processedImages[i] = Process(refChildImage, maskChildImage, inspChildImage, thVal, debugContext);
            }

            refChildImage.Dispose();
            maskChildImage.Dispose();
            inspChildImage.Dispose();
            */
        #endregion
    }
}
