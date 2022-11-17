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
using UniScan.Common.Settings;

namespace UniScanG.Gravure.Vision.Calculator
{
    public class Calculator : Algorithm
    {
        public static string TypeName { get { return "Calculator"; } }

        public Calculator()
        {
            this.AlgorithmName = TypeName;
            this.param = new CalculatorParam();

            ThreadPool.SetMaxThreads(100, 100);
        }

        #region Abstract
        public override Algorithm Clone()
        {
            throw new NotImplementedException();
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            throw new NotImplementedException();
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            throw new NotImplementedException();
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            throw new NotImplementedException();
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            throw new NotImplementedException();
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
            CalculatorParam calculatorParam = this.param as CalculatorParam;

            AlgoImage sheetImage = null;
            ProcessBufferSetG processBufferSet = null;
            AlgoImage bufTemp = null, bufResult = null;
            RegionInfoG inspRegionInfo = sheetInspectParam.RegionInfo as RegionInfoG;
            CancellationToken cancellationToken = sheetInspectParam.CancellationToken;

            bool disposeNeed;
            if (sheetInspectParam != null)
            {
                processBufferSet = sheetInspectParam.ProcessBufferSet as ProcessBufferSetG;
                if (processBufferSet == null)
                    return null;    // null 리턴시 skip

                sheetImage = processBufferSet.ScaledAlgoImage;
                if (sheetImage == null)
                    sheetImage = sheetInspectParam.AlgoImage;

                bufTemp = processBufferSet.CalculatorTemp;
                bufResult = processBufferSet.CalculatorResult;
                disposeNeed = false;
            }
            else
            {
                sheetImage = ImageBuilder.Build(this.GetAlgorithmType(), algorithmInspectParam.ClipImage, ImageType.Grey);
                bufTemp = ImageBuilder.Build(sheetImage);
                bufResult = ImageBuilder.Build(sheetImage);
                disposeNeed = true;
            }


            DebugContext debugContext = new DebugContext(algorithmInspectParam.DebugContext.SaveDebugImage, Path.Combine(algorithmInspectParam.DebugContext.FullPath, "Calculator"));
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(sheetImage);
            sheetImage.Save("SheetImage.bmp", debugContext);

            // Create Preview image
            Bitmap previewBitmap = null;
            float ratio = SystemTypeSettings.Instance().ResizeRatio;
            Task resizeTask =new  Task(() =>
            {
                AlgoImage fullSheetImage = sheetInspectParam.AlgoImage;
                AlgoImage resizeSheetImage = null;
                if (fullSheetImage is SheetImageSet)
                {
                    resizeSheetImage = ((SheetImageSet)fullSheetImage).GetFullImage(ratio);
                }
                else
                {
                    int newWidth = (int)(fullSheetImage.Width * ratio);
                    int newHeigth = (int)(fullSheetImage.Height * ratio);
                    resizeSheetImage = ImageBuilder.Build(fullSheetImage.LibraryType, ImageType.Grey, newWidth, newHeigth);
                    imageProcessing.Resize(fullSheetImage, resizeSheetImage);
                }
                resizeSheetImage.Save("ResizeSheetImage.bmp", debugContext);

                ImageD previewImageD = resizeSheetImage.ToImageD();
                previewBitmap = previewImageD.ToBitmap();
                previewImageD.Dispose();
                resizeSheetImage.Dispose();
            });
            resizeTask.Start();

            List<RegionInfoG> regionInfoList = calculatorParam.RegionInfoList;
            List<AlgorithmResult> algorithmResultList = new List<AlgorithmResult>();

            int binValue = calculatorParam.BinValue;
            Point basePos = calculatorParam.BasePosition;
            Point foundPos = Point.Empty;

            Point diffPos = AlignSheet(sheetImage, calculatorParam.BasePosition);
            //diffPos = Point.Empty;

            sheetInspectParam.FidOffset = new SizeF(diffPos);
            Debug.WriteLine(string.Format("DiffPos: {0:F2},{1:F2}", diffPos.X, diffPos.Y));
            LogHelper.Debug(LoggerType.Inspection, string.Format("SheetOffset: {0:F2},{1:F2}", diffPos.X, diffPos.Y));

            if (inspRegionInfo == null) // 모든 영역 검사
            {
                if (calculatorParam.ParallelOperation)
                {
                    Parallel.For(0, regionInfoList.Count, i =>
                    {
                        RegionInfoG regionInfo = regionInfoList[i];
                        if (regionInfo.Use)
                        {
                            DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, string.Format("RegionInfo_{0}", i)));
                            InspectRegion(sheetImage, bufTemp, bufResult, regionInfo, diffPos, cancellationToken, newDebugContext);
                        }
                    });
                }
                else
                {
                    for (int i = 0; i < regionInfoList.Count; i++)
                    {
                        RegionInfoG regionInfo = regionInfoList[i];
                        if (regionInfo.Use)
                        {
                            DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, string.Format("RegionInfo_{0}", i)));
                            InspectRegion(sheetImage, bufTemp, bufResult, regionInfo, diffPos, cancellationToken, newDebugContext);
                        }
                    }
                }
            }
            else // 지정된 영역만 검사
            {
                DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, "RegionInfo_Test"));
                InspectRegion(sheetImage, bufTemp, bufResult, inspRegionInfo, diffPos, cancellationToken, newDebugContext);
            }
            bufResult.Save("CalculatorResult.bmp", debugContext);

            if (resizeTask.Status != TaskStatus.Created)
                resizeTask.Wait();

            if (disposeNeed)
            {
                sheetImage.Dispose();
                bufTemp.Dispose();
                bufResult.Dispose();
            }

            CalculatorResult calculatorResult = (CalculatorResult)CreateAlgorithmResult();
            calculatorResult.PrevImage = previewBitmap;
            calculatorResult.SheetPosOffset = diffPos;
            calculatorResult.SubResultList.AddRange(algorithmResultList);
            return calculatorResult;
        }

        private Point AlignSheet(AlgoImage algoImage, Point basePosition)
        {
            Point diffPos = Point.Empty;
            //algoImage.Save(@"d:\temp\algoImage.bmp");
            SheetFinderBase sheerFinder = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName) as SheetFinderBase;
            int foundPosX = sheerFinder.FindBasePosition(algoImage, Direction.Horizontal, 20);
            int foundPosY = sheerFinder.FindBasePosition(algoImage, Direction.Vertical, 20);
            if (foundPosX >= 0 && foundPosY >= 0)
                diffPos = new Point(foundPosX - basePosition.X, foundPosY - basePosition.Y);

            return diffPos;
        }

        private void InspectRegion(AlgoImage sheetImage, AlgoImage bufTemp, AlgoImage bufResult, RegionInfoG regionInfo, Point diffPos, CancellationToken cancellationToken, DebugContext debugContext)
        {
            Rectangle region = regionInfo.Region;
            region.Offset(diffPos.X, diffPos.Y);
            Rectangle imageRect = new Rectangle(Point.Empty, sheetImage.Size);
            Rectangle adjustRect = Rectangle.Intersect(imageRect, region);
            if (adjustRect == region)
            {
                AlgoImage subRegionImage = sheetImage.GetSubImage(region);
                AlgoImage subTempImage = bufTemp.GetSubImage(region);
                AlgoImage subResultImage = bufResult.GetSubImage(region);
                subRegionImage.Save("RegionImage.bmp", debugContext);

                InspectRegion(subRegionImage, subTempImage, subResultImage, regionInfo, cancellationToken, debugContext);

                subRegionImage.Dispose();
                subTempImage.Dispose();
                subResultImage.Dispose();
            }
            else
            {
                LogHelper.Error(LoggerType.Error, string.Format("Region Rect {0} is out of image", debugContext.FullPath));
            }
        }

        private void InspectRegion(AlgoImage regionImage, AlgoImage bufTemp, AlgoImage bufResult, RegionInfoG regionInfo, CancellationToken cancellationToken, DebugContext debugContext)
        {
            LogHelper.Debug(LoggerType.Inspection, string.Format("Calculator::InspectRegion Start"));

            CalculatorParam calculatorParam = AlgorithmPool.Instance().GetAlgorithm(Calculator.TypeName).Param as CalculatorParam;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(regionImage);
            Rectangle imageRect = new Rectangle(Point.Empty, regionImage.Size);
            Stopwatch sw = Stopwatch.StartNew();
            regionImage.Save("regionImage.bmp", debugContext);
            
            List<Rectangle> inspRegionRectList = AlignLine(regionImage, regionInfo, debugContext);

            if (regionInfo.OddEvenPair == false)
            {
                List<Tuple<int, Rectangle, bool>> inspRegionRectList2 = new List<Tuple<int, Rectangle, bool>>();
                for (int i = 0; i < inspRegionRectList.Count; i++)
                    inspRegionRectList2.Add(new Tuple<int, Rectangle, bool>(i, inspRegionRectList[i], regionInfo.InspRegionList[i].IncludeDontCare));
                InspectLineSet(inspRegionRectList2, regionImage, bufTemp, bufResult, cancellationToken, debugContext);
            }
            else
            {
                List<Tuple<int, Rectangle, bool>> oddInspLineRectList = new List<Tuple<int, Rectangle, bool>>();
                List<Tuple<int, Rectangle, bool>> evenInspLineRectList = new List<Tuple<int, Rectangle, bool>>();
                for (int i = 0; i < inspRegionRectList.Count; i++)
                {
                    Tuple<int, Rectangle, bool> tuple = new Tuple<int, Rectangle, bool>(i, inspRegionRectList[i], regionInfo.InspRegionList[i].IncludeDontCare);
                    if (i % 2 == 0)
                        evenInspLineRectList.Add(tuple);
                    else
                        oddInspLineRectList.Add(tuple);
                }
                Task t1 = Task.Run(() => InspectLineSet(evenInspLineRectList, regionImage, bufTemp, bufResult, cancellationToken,new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath,"Even"))));
                Task t2 = Task.Run(() => InspectLineSet(oddInspLineRectList, regionImage, bufTemp, bufResult, cancellationToken, new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, "Odd"))));
                t1.Wait();
                t2.Wait();
            }

            bufResult.Save("Result.bmp", debugContext);
            sw.Stop();
            LogHelper.Debug(LoggerType.Inspection, string.Format("Calculator::InspectRegion End. {0}ms", sw.ElapsedMilliseconds));
        }

        private List<Rectangle> AlignLine(AlgoImage regionImage, RegionInfoG regionInfoG, DebugContext debugContext)
        {
            CalculatorParam calculatorParam = AlgorithmPool.Instance().GetAlgorithm(Calculator.TypeName).Param as CalculatorParam;
            if (calculatorParam.InBarAlignX == false && calculatorParam.InBarAlignY == false)
                return regionInfoG.InspRegionList.ConvertAll(f => f.Rectangle);

            SheetFinderBase sheerFinder = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName) as SheetFinderBase;
            Rectangle imageRect = new Rectangle(Point.Empty, regionImage.Size);

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(regionImage);

            int[] offsetX = new int[regionInfoG.InspRegionList.Count];
            int[] offsetY = new int[regionInfoG.InspRegionList.Count];
            Array.Clear(offsetX, 0, offsetX.Length);
            Array.Clear(offsetY, 0, offsetY.Length);

            // Aligh X
            if (calculatorParam.InBarAlignX && regionInfoG.SplitLineDirection == Direction.Vertical)
            {
                Size patCount = new Size(regionInfoG.AdjPatRegionList.GetLength(1), regionInfoG.AdjPatRegionList.GetLength(0));
                int centerPatPos = patCount.Height / 2;

                Rectangle rect = regionInfoG.PatRegionList[centerPatPos, 0];
                for (int i = 1; i < patCount.Width; i++)
                    rect = Rectangle.Union(rect, regionInfoG.PatRegionList[centerPatPos, i]);
                rect.Inflate(regionImage.Width / 2, 0);
                rect.Intersect(imageRect);

                AlgoImage processImage = regionImage.GetSubImage(rect);

                float[] data = ip.Projection(processImage, Direction.Horizontal, ProjectionType.Mean);
                processImage.Dispose();
                List<Point> hillList = AlgorithmCommon.FindHill(data, data.Average(), true);
                if (hillList.Count >= regionInfoG.InspRegionList.Count)
                {
                    float[] hillCenters = hillList.ConvertAll(f => (f.X + f.Y) / 2.0f).ToArray();
                    float[] regionCenters = regionInfoG.InspRegionList.ConvertAll(f => DrawingHelper.CenterPoint(f.Rectangle).X).ToArray();
                    float[] distError = new float[regionCenters.Length];
                    for (int i = 0; i < regionCenters.Length; i++)
                        distError[i] = Math.Abs(regionCenters[0] - hillCenters[i]);
                    float minError = distError.Min();
                    int srcIdx = Array.FindIndex(distError, f => f == minError);
                    int dstIdx = srcIdx + regionInfoG.InspRegionList.Count - 1;
                    if (dstIdx <= regionInfoG.InspRegionList.Count)
                    {
                        int len = dstIdx - srcIdx + 1;
                        for (int i = 0; i < len; i++)
                            offsetX[i] = (int)Math.Round(hillCenters[srcIdx + i] - regionCenters[i]);
                    }
                }
            }

            // Align Y
            if (calculatorParam.InBarAlignY)
            {
                for (int i = 0; i < regionInfoG.InspRegionList.Count; i++)
                {
                    InspRegionElement inspRegionElement = regionInfoG.InspRegionList[i];

                    if (inspRegionElement.OffsetYBase >= 0)
                    {
                        int basePos = inspRegionElement.OffsetYBase;

                        Rectangle subRect = inspRegionElement.Rectangle;
                        subRect.Y = 0;
                        subRect.Height /= 3;
                        AlgoImage subimage = regionImage.GetSubImage(subRect);
                        //subimage.Save(@"d:\temp\subimage.bmp");
                        int foundPos = InspRegionElement.GetOffsetValue(subimage);
                        if (foundPos >= 0)
                            offsetY[i]= foundPos - basePos;
                        subimage.Dispose();
                    }
                }
            }

            List<Rectangle> alignedRect = new List<Rectangle>();
            for (int i = 0; i < regionInfoG.InspRegionList.Count; i++)
            {
                InspRegionElement inspRegionElement = regionInfoG.InspRegionList[i];
                Rectangle baseRect = inspRegionElement.Rectangle;
                switch (regionInfoG.SplitLineDirection)
                {
                    case Direction.Horizontal:
                        baseRect.Offset(offsetY[i], 0);
                        break;
                    case Direction.Vertical:
                        baseRect.Offset(offsetX[i], offsetY[i]);
                        break;
                }
                if (Rectangle.Intersect(imageRect, baseRect) != baseRect)
                    baseRect = inspRegionElement.Rectangle;

                alignedRect.Add(baseRect);
            }
            return alignedRect;
        }

        private List<InspRegionElement> AdjustInspRegion(AlgoImage regionImage, RegionInfoG regionInfo, bool oddEvenPair)
        {
            List<InspRegionElement> inspRegionList = new List<InspRegionElement>(regionInfo.InspRegionList);

            Rectangle projRegion = new Rectangle(0, 0, regionImage.Width / 5, regionImage.Height);
            if (projRegion.Width > 0 && projRegion.Height > 0)
            {
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(regionImage);
                AlgoImage projImage = regionImage.GetSubImage(projRegion);
                float[] projData = ip.Projection(projImage, Direction.Vertical, ProjectionType.Mean);
                projImage.Dispose();
                List<Point> patRegionList = new List<Point>();
                Trainer.TrainerParam trainerParam = AlgorithmPool.Instance().GetAlgorithm(Trainer.Trainer.TypeName).Param as Trainer.TrainerParam;
                if (oddEvenPair)
                    patRegionList = AlgorithmCommon.FindHill3(projData, trainerParam.KernalSize, trainerParam.MinLineIntensity, trainerParam.DiffrentialThreshold);
                else
                    patRegionList = AlgorithmCommon.FindHill(projData, trainerParam.DiffrentialThreshold);

                if (patRegionList.Count == regionInfo.PatRegionList.GetLength(0))
                {
                    int srcIdx = 0, dstIdx = -1;
                    for (int i = 0; i < inspRegionList.Count; i++)
                    {
                        dstIdx = srcIdx + regionInfo.LinePair - 1;
                        int a = (regionInfo.PatRegionList[srcIdx, 0].Top + regionInfo.PatRegionList[dstIdx, 0].Bottom) / 2;
                        int b = (patRegionList[srcIdx].X + patRegionList[dstIdx].Y) / 2;
                        int offset = b - a;
                        Rectangle inspRegion = inspRegionList[i].Rectangle;
                        inspRegion.Offset(0, offset);
                        inspRegionList[i].Rectangle = inspRegion;

                        srcIdx = dstIdx + 1;
                    }
                }
            }
            return inspRegionList;
        }

        private void InspectLineSet(List<Tuple<int, Rectangle, bool>> inspLineRectList,  AlgoImage regionImage, AlgoImage bufTemp, AlgoImage bufResult, CancellationToken cancellationToken, DebugContext debugContext)
        {
            InspectLineSet(inspLineRectList, 0, inspLineRectList.Count, regionImage, bufTemp, bufResult, cancellationToken, debugContext);
        }

        private void InspectLineSet(List<Tuple<int, Rectangle, bool>> inspLineRectList, int src, int dst, AlgoImage regionImage, AlgoImage bufTemp, AlgoImage bufResult,  CancellationToken cancellationToken, DebugContext debugContext)
        {
            CalculatorParam cp = this.param as CalculatorParam;

            if (inspLineRectList.Count < 3)
                return;

            int lineCount = dst - src;
            int splitLineCount = lineCount / 35 + 1;
            if (splitLineCount > 1)
            // 줄 개수가 많으면 나누어서 검사함
            {
                List<Task> taskList = new List<Task>();
                float linePerThread = (float)inspLineRectList.Count / splitLineCount;
                int srcLine = 0, dstLine = 0;
                for (int i = 0; i < splitLineCount; i++)
                {
                    srcLine = dstLine;
                    dstLine = (int)Math.Round(linePerThread * (i + 1));
                    Task task = Task.Run(() => InspectLineSet(inspLineRectList, srcLine, dstLine, regionImage, bufTemp, bufResult, cancellationToken, new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, string.Format("T{0}", i)))));
                    if (cp.ParallelOperation == false)
                        task.Wait();
                    taskList.Add(task);
                }

                while (taskList.TrueForAll(f => f.IsCompleted) == false)
                    Thread.Sleep(10);

                return;
            }

            List<Tuple<int, Rectangle, bool>> careLineRectList = inspLineRectList.FindAll(f => f.Item3 == false);

            int baseLine = (src + dst) / 2 + 1;
            AlgoImage baseLineImage = regionImage.GetSubImage(inspLineRectList[baseLine].Item2);
            AlgoImage baseEdgeImage = GetEdgeImage(baseLineImage, debugContext);
            baseLineImage.Dispose();

            for (int i = src; i < dst; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                // 검사 할 줄
                Tuple<int, Rectangle, bool> curTuple = inspLineRectList[i];
                Rectangle inspRegion = curTuple.Item2;
                bool isDontCare = curTuple.Item3;

                Rectangle upperRegion, lowerRegion;
                if (cp.AdaptivePairing == false || isDontCare)
                // 돈케어 있음 -> 그냥 짝지음.
                {
                    // 줄. 윗줄이 없으면 아래줄의 아래줄 사용
                    upperRegion = (i == 0) ? inspLineRectList[i + 2].Item2 : inspLineRectList[i - 1].Item2;

                    // 아래줄. 아래줄이 없으면 위줄의 위줄 사용
                    lowerRegion = (i == inspLineRectList.Count - 1) ? inspLineRectList[i - 2].Item2 : inspLineRectList[i + 1].Item2;
                }
                else
                // 돈케어 없음 -> 돈케어 없는것 끼리 짝지음.
                {
                    if (careLineRectList.Count < 3)
                        continue;

                    int curInx = careLineRectList.FindIndex(f => f == curTuple);

                    upperRegion = (curInx == 0) ? careLineRectList[curInx + 2].Item2 : careLineRectList[curInx - 1].Item2;
                    lowerRegion = (curInx == careLineRectList.Count - 1) ? careLineRectList[curInx - 2].Item2 : careLineRectList[curInx + 1].Item2;
                }

                int minHeight = Math.Min(Math.Min(inspRegion.Height, upperRegion.Height), lowerRegion.Height);
                int minWidth = Math.Min(Math.Min(inspRegion.Width, upperRegion.Width), lowerRegion.Width);
                Rectangle inspRect = inspRegion;
                Rectangle upperRect = upperRegion;
                Rectangle lowerReect = lowerRegion;
                inspRect.Size = upperRect.Size = lowerReect.Size = new Size(minWidth, minHeight);

                DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, System.IO.Path.Combine(debugContext.FullPath, string.Format("Line{0}", inspLineRectList[i].Item1)));
                InspectLine(regionImage, baseEdgeImage, bufTemp, bufResult, inspRect, upperRect, lowerReect, newDebugContext);
            }

            baseEdgeImage.Dispose();
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
            List<Point> hillListH = AlgorithmCommon.FindHill(dataH, 128, true);
            foreach (Point hill in hillListH)
            {
                edgePositionList.Add(new Tuple<Direction, int, int>(Direction.Vertical, hill.X, +1));
                edgePositionList.Add(new Tuple<Direction, int, int>(Direction.Vertical, hill.Y, -1));
            }

            return edgePositionList;
        }

        private AlgoImage GetEdgeImage(AlgoImage baseLineImage, DebugContext debugContext)
        {
            CalculatorParam cp = this.param as CalculatorParam;
            AlgoImage edgeImage = null;
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(baseLineImage);
            Size drawSize = Size.Empty;
            baseLineImage.Save("BaseLineImage.bmp", debugContext);

            if (false)
            {
                edgeImage = baseLineImage.Clone();
                ip.Binarize(edgeImage);

                BlobParam blobParam = new BlobParam();
                blobParam.MaxCount = 0;
                blobParam.AreaMin = 4;
                blobParam.SelectBoundingRect = true;
                blobParam.EraseBorderBlobs = true;
            }
            else
            {
                edgeImage = ImageBuilder.Build(baseLineImage);
                float[] dataV = ip.Projection(baseLineImage, Direction.Vertical, ProjectionType.Mean);
                List<Point> hillListV = AlgorithmCommon.FindHill(dataV, -1, true);
                drawSize = new Size(baseLineImage.Width, cp.EdgeWidth * 3);
                foreach (Point hill in hillListV)
                {
                    Rectangle srcEdgeRect = new Rectangle(new Point(0, hill.X - cp.EdgeWidth), drawSize);
                    ip.DrawRect(edgeImage, srcEdgeRect, cp.EdgeValue, true);

                    Rectangle dstEdgeRect = new Rectangle(new Point(0, hill.Y - 2 * cp.EdgeWidth), drawSize);
                    ip.DrawRect(edgeImage, dstEdgeRect, cp.EdgeValue, true);
                }

                float[] dataH = ip.Projection(baseLineImage, Direction.Horizontal, ProjectionType.Mean);
                //List<Point> hillListH = AlgorithmCommon.FindHill(dataH, -1, true,debugContext);
                List<Point> hillListH = AlgorithmCommon.FindHill2(dataH, 10, debugContext);
                drawSize = new Size(cp.EdgeWidth * 3, baseLineImage.Height);
                if (hillListH.Count == 3)
                {
                    Rectangle srcEdgeRect = new Rectangle(new Point(hillListH[0].Y - cp.EdgeWidth, 0), drawSize);
                    ip.DrawRect(edgeImage, srcEdgeRect, cp.EdgeValue, true);
                    ip.DrawRect(edgeImage, Rectangle.Inflate(new Rectangle(hillListH[0].Y, 0, 1, baseLineImage.Height), 1, 0), 255, true);

                    Rectangle dstEdgeRect = new Rectangle(new Point(hillListH[2].X - 2 * cp.EdgeWidth, 0), drawSize);
                    ip.DrawRect(edgeImage, dstEdgeRect, cp.EdgeValue, true);
                    ip.DrawRect(edgeImage, Rectangle.Inflate(new Rectangle(hillListH[2].X, 0, 1, baseLineImage.Height), 1, 0), 255, true);
                }
            }
            edgeImage.Save("BaseEdgeImage.bmp", debugContext);
            //edgeImage.Save(@"d:\temp\BaseEdgeImage.bmp");
            return edgeImage;
        }

        private void InspectLine(AlgoImage inspImage, AlgoImage baseEdgeImage, AlgoImage tempImage, AlgoImage resultImage, Rectangle insppRect, Rectangle upperRect, Rectangle lowerRect, DebugContext debugContext)
        {
            Debug.Assert(insppRect.Size == upperRect.Size);
            Debug.Assert(insppRect.Size == lowerRect.Size);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(inspImage);

            AlgoImage upper = null, inspp = null, lower = null;
            AlgoImage temp1 = null, temp2 = null, temp3 = null, result = null;
            try
            {
                upper = inspImage.GetSubImage(upperRect);
                inspp = inspImage.GetSubImage(insppRect);
                lower = inspImage.GetSubImage(lowerRect);
                upper.Save("0-1. A.bmp", debugContext);
                inspp.Save("0-2. B.bmp", debugContext);
                lower.Save("0-3. C.bmp", debugContext);

                temp1 = tempImage.GetSubImage(upperRect);
                temp2 = tempImage.GetSubImage(insppRect);
                temp3 = tempImage.GetSubImage(lowerRect);

                result = resultImage.GetSubImage(insppRect);

                imageProcessing.Subtract(inspp, upper, temp1); //notABS
                temp1.Save("1-1. B-A.bmp", debugContext);
                imageProcessing.Subtract(inspp, lower, temp2); //notABS
                temp2.Save("1-2. B-C.bmp", debugContext);
                imageProcessing.Min(temp1, temp2, temp1); //어두운 불량
                temp1.Save("1-3. Min.bmp", debugContext);

                imageProcessing.Subtract(upper, inspp, temp2); //notABS
                temp2.Save("2-1. A-B.bmp", debugContext);
                imageProcessing.Subtract(lower, inspp, temp3); //notABS
                temp3.Save("2-2. C-B.bmp", debugContext);
                imageProcessing.Min(temp2, temp3, temp3); //밝은 불량
                temp3.Save("2-3. Min.bmp", debugContext);

                imageProcessing.Max(temp1, temp3, result);
                result.Save("3. Max.bmp", debugContext);

                //  엣지영역 제거
                imageProcessing.Subtract(result, baseEdgeImage, result);
                result.Save("4. RemoveEdge.bmp", debugContext);
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
                temp1?.Dispose();
                temp2?.Dispose();
                temp3?.Dispose();
                result?.Dispose();
            }
        }

        private void RemoveEdge(AlgoImage inspp, AlgoImage result, List<Tuple<Direction, int,int>> edgePositionList, DebugContext debugContext)
        {
            CalculatorParam calculatorParam = this.param as CalculatorParam;
            if (calculatorParam.EdgeWidth == 0 || calculatorParam.EdgeValue == 0)
                return;

            AlgoImage debugImage = null;
            if (debugContext.SaveDebugImage)
                debugImage = inspp.ConvertToMilImage(ImageType.Color);

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(inspp);
            Rectangle imageRect = new Rectangle(Point.Empty, inspp.Size);
            foreach (Tuple<Direction, int,int> edgePosition in edgePositionList)
            {
                Rectangle rect = Rectangle.Empty;
                int inflate = calculatorParam.EdgeWidth * 2;
                int offset = calculatorParam.EdgeWidth * edgePosition.Item3;
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
                ip.Subtract(subResultAlgoImage, subResultAlgoImage, calculatorParam.EdgeValue);
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

        private void RemoveEdge(AlgoImage inspp, AlgoImage result, DebugContext debugContext)
        {
            CalculatorParam calculatorParam = this.param as CalculatorParam;
            if (calculatorParam.EdgeWidth == 0 || calculatorParam.EdgeValue == 0)
                return;

            AlgoImage debugImage = null;
            if(debugContext.SaveDebugImage)
                debugImage = inspp.ConvertToMilImage(ImageType.Color);

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(inspp);
            Rectangle imageRect = new Rectangle(Point.Empty, inspp.Size);
            float[] dataV = ip.Projection(inspp, Direction.Vertical, ProjectionType.Mean);
            List<Point> hillListV = AlgorithmCommon.FindHill(dataV, 128);
            foreach (Point hill in hillListV)
            {
                for (int i = 0; i < 2; i++)
                {
                    int y0 = i == 0 ? hill.X : hill.Y;
                    int y1 = i == 0 ? hill.X - calculatorParam.EdgeWidth * 2 : hill.Y - calculatorParam.EdgeWidth;
                    int y2 = i == 0 ? hill.X + calculatorParam.EdgeWidth : hill.Y + calculatorParam.EdgeWidth * 2;
                    Rectangle rect = Rectangle.FromLTRB(0, y1, inspp.Width, y2);
                    rect.Intersect(imageRect);
                     
                    AlgoImage subResultAlgoImage = result.GetSubImage(rect);
                    ip.Subtract(subResultAlgoImage, subResultAlgoImage, calculatorParam.EdgeValue);
                    subResultAlgoImage.Dispose();

                    if (debugImage != null)
                    {
                        ip.DrawRect(debugImage, Rectangle.FromLTRB(0, y0, inspp.Width, y0), Color.Yellow.ToArgb(), false);
                        ip.DrawRect(debugImage, rect, Color.Red.ToArgb(), false);
                    }

                }
            }

            float[] dataH = ip.Projection(inspp, Direction.Horizontal, ProjectionType.Mean);
            List<Point> hillListH = AlgorithmCommon.FindHill(dataH, 128, true);
            foreach (Point hill in hillListH)
            {
                for (int i = 0; i < 2; i++)
                {
                    int y0 = i == 0 ? hill.X : hill.Y;
                    int x1 = i == 0 ? hill.X - calculatorParam.EdgeWidth : hill.Y - calculatorParam.EdgeWidth * 2;
                    int x2 = i == 0 ? hill.X + calculatorParam.EdgeWidth * 2 : hill.Y + calculatorParam.EdgeWidth;
                    Rectangle rect = Rectangle.FromLTRB(x1, 0, x2, inspp.Height);
                    rect.Intersect(imageRect);

                    AlgoImage subResultAlgoImage = result.GetSubImage(rect);
                    ip.Subtract(subResultAlgoImage, subResultAlgoImage, calculatorParam.EdgeValue);
                    subResultAlgoImage.Dispose();

                    if (debugImage != null)
                        {
                        ip.DrawRect(debugImage, Rectangle.FromLTRB(y0, 0, y0, inspp.Height), Color.Yellow.ToArgb(), false);
                        ip.DrawRect(debugImage, rect, Color.Red.ToArgb(), false);
                    }
                }
            }

            if(debugImage!=null)
            {
                debugImage.Save("RemoveDebug.bmp", debugContext);
                debugImage.Dispose();
            }
        }



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
