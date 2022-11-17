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
using System.Windows.Media.Imaging;

namespace UniScanG.Gravure.Vision.Calculator.V2
{
    public class CalculatorV2 : CalculatorBase
    {
        public CalculatorV2()
        {
            this.AlgorithmName = CalculatorBase.TypeName;
            this.param = null;
        }

        #region Abstract
        public override AlgorithmParam CreateParam()
        {
            return new CalculatorParam(true);
        }

        public override DynMvp.Vision.Algorithm Clone()
        {
            CalculatorV2 clone = new CalculatorV2();
            clone.CopyFrom(this);

            return clone;
        }

        public override ProcessBufferSetG CreateProcessingBuffer(float scaleFactor, bool isMultiLayer, int width, int height)
        {
            return new ProcessBufferSetG2(SystemTypeSettings.Instance().ResizeRatio, scaleFactor, isMultiLayer, width, height);
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
            UniScanG.Data.Model.Model model = sheetInspectParam.Model;
            ProcessBufferSetG2 processBufferSet = sheetInspectParam.ProcessBufferSet as ProcessBufferSetG2;
            CancellationToken cancellationToken = sheetInspectParam.CancellationToken;
            bool isTestInspection = sheetInspectParam.TestInspect;

            if (processBufferSet == null)
                return null;    // null 리턴시 skip

            AlgoImage fullImage = processBufferSet.AlgoImage;

            AlgoImage previewBuffer = processBufferSet.PreviewBuffer;
            AlgoImage scaleImage = processBufferSet.ScaledImage;
            if (scaleImage == null)
                scaleImage = fullImage;

            AlgoImage inspImage = scaleImage;

            AlgoImage diffImage = processBufferSet.CalculatorResultGray;
            AlgoImage binalImage = processBufferSet.CalculatorResultBinal;

            DebugContextG debugContextG = sheetInspectParam.DebugContext as DebugContextG;

            CalculatorResult calculatorResult = (CalculatorResult)CreateAlgorithmResult();
            calculatorResult.SetPartialProjection(processBufferSet.PartialProjection);

            inspImage.Save("SheetImage.bmp", debugContextG);
            Point patternOffset = processBufferSet.OffsetStructSet.PatternOffset.Offset;
            try
            {
                //calculatorResult.OffsetSet = new OffsetStructSet(processBufferSet.OffsetStructSet.LocalCount);
                //calculatorResult.OffsetSet.PatternOffset.CopyFrom(processBufferSet.OffsetStructSet.PatternOffset);
                calculatorResult.OffsetSet.CopyFrom(processBufferSet.OffsetStructSet);
                if (AlgorithmSetting.Instance().UseExtSticker && CalculatorParam.UseSticker)
                {
                    DebugContextG stickerFinderDebugContext = debugContextG.Clone();
                    stickerFinderDebugContext.IsSticker = true;

                    Point basePosition = DrawingHelper.Add(model.CalculatorModelParam.BasePosition, patternOffset);
                    SheetFinderBaseParam sheerFinderParam = SheetFinderBase.SheetFinderBaseParam;
                    Rectangle fullRect = new Rectangle(Point.Empty, fullImage.Size);
                    if (calibration != null)
                    {
                        int width = (int)Math.Round(calibration.WorldToPixel(sheerFinderParam.SearchSkipWidthMm * 1000));
                        fullRect.Inflate(-width, 0);
                    }
                    Rectangle stickerFindRect = Rectangle.Empty;
                    switch (sheerFinderParam.GetBaseXSearchDir())
                    {
                        case BaseXSearchDir.Left2Right:
                            stickerFindRect = Rectangle.FromLTRB(fullRect.Left, 0, basePosition.X, fullRect.Bottom);
                            break;
                        case BaseXSearchDir.Right2Left:
                            stickerFindRect = Rectangle.FromLTRB(basePosition.X, 0, fullRect.Right, fullRect.Bottom);
                            break;
                    }

                    //using (AlgoImage stickerFindImage = fullImage.GetSubImage(stickerFindRect))
                    {
                        DefectObj[] sheetSubResults = StickerFinder.FindSticker(fullImage, stickerFindRect, calibration, stickerFinderDebugContext);
                        calculatorResult.SheetSubResultList.AddRange(sheetSubResults);
                    }
                }

                if (isTestInspection || calculatorResult.SheetSubResultList.Count == 0)
                {
                    RegionInfoG inspRegionInfo = sheetInspectParam.TargetRegionInfo as RegionInfoG;
                    int targetIndex = Array.FindIndex(processBufferSet.InspectRegions, f => f != null && f.RegionInfoG == inspRegionInfo);

                    int stratIndex = Math.Max(0, targetIndex);
                    int endIndex = targetIndex < 0 ? model.CalculatorModelParam.RegionInfoCollection.Count : stratIndex + 1;

                    ParallelOptions parallelOptions = new ParallelOptions()
                    {
                        MaxDegreeOfParallelism = CalculatorParam.UseMultiThread ? -1 : 1,
                        CancellationToken = cancellationToken
                    };
                    //parallelOptions.MaxDegreeOfParallelism = endIndex - stratIndex;
                    debugContextG.ProcessTimeLog.SetParallel(ProcessTimeLog.ProcessTimeLogItem.Calculate, parallelOptions.MaxDegreeOfParallelism);
                    debugContextG.ProcessTimeLog.SetParallel(ProcessTimeLog.ProcessTimeLogItem.Binalize, parallelOptions.MaxDegreeOfParallelism);
                    Parallel.For(stratIndex, endIndex, parallelOptions, i =>
                    {
                        try
                        {
                            //LogHelper.Debug(LoggerType.Inspection, $"CalculatorV2::Inspect - Parallel i = {i}");
                            InspectRegion f = processBufferSet.InspectRegions[i];
                            if (f.Use)
                            {
                                if (!f.IsSet)
                                // 정렬 실패 등으로 버퍼 Upload 안된경우. 일단 스티커로 불량처리.
                                {
                                    Data.DefectObj alignFailObj = new Data.DefectObj();
                                    alignFailObj.PositionType = DefectObj.EPositionType.None;
                                    alignFailObj.ShapeType = DefectObj.EShapeType.None;
                                    alignFailObj.ValueType = DefectObj.EValueType.None;
                                    alignFailObj.Region = f.Rectangle;
                                    alignFailObj.RealRegion = sheetInspectParam.CameraCalibration.PixelToWorld(f.Rectangle);
                                    calculatorResult.SheetSubResultList.Add(alignFailObj);
                                    return;
                                }

                                DebugContextG regionDebugContext = debugContextG.Clone();
                                regionDebugContext.RegionId = i;

                                InspectRegion(f, inspImage, diffImage, binalImage, patternOffset, cancellationToken, regionDebugContext);
                                //calculatorResult.OffsetSet.LocalOffsets[i].Set(f.LocalAlignResult.Result, f.LocalAlignResult.Offset, f.LocalAlignResult.Score, f.LocalAlignResult.imageD);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Error(LoggerType.Inspection, ex);
                        }
                    });
                }
            }
#if !DEBUG
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, ex);
            }
#endif
            finally { }
            diffImage.Save("CalculatorResult.bmp", debugContextG);

            //Debug.Assert(resizeTask.Status == TaskStatus.Created || resizeTask.Status == TaskStatus.RanToCompletion);
            //WaitResizeTask(resizeTask);

            //int patternWidth = fullImage.Width - calculatorParam.BasePosition.X + patternOffset.X;
            //int patternHeight = fullImage.Height;
            calculatorResult.OffsetFound = new Size(patternOffset);
            calculatorResult.SheetSizePx = processBufferSet.PatternSizePx;
            calculatorResult.SheetSize = sheetInspectParam.CameraCalibration.PixelToWorld(processBufferSet.PatternSizePx);
            calculatorResult.Good = calculatorResult.SheetSubResultList.Count == 0;
            calculatorResult.UpdateSpandTime();
            //previewBitmap.Save(@"C:\temp\previewBitmap.bmp");
            return calculatorResult;
        }

        private void WaitResizeTask(Task resizeTask)
        {
            DynMvp.ConsoleEx.WriteLine(resizeTask.Status.ToString());
            if (resizeTask.Status != TaskStatus.Created && !resizeTask.IsCompleted)
            {
                Stopwatch sw = Stopwatch.StartNew();
                resizeTask.Wait();
                DynMvp.ConsoleEx.WriteLine(string.Format("CalculatorV2::WaitResizeTask {0:F3} [ms]", sw.Elapsed.TotalMilliseconds));
            }
            //sw.Stop();
        }

        private Bitmap BuildPreviewBitmap(AlgoImage previewBuffer, DebugContext debugContext)
        {
            Stopwatch sw = Stopwatch.StartNew();
            DynMvp.ConsoleEx.WriteLine(string.Format("CalculatorV2::BuildPreviewBitmap - Start"));

            Bitmap previewBitmap = null;

            {
                previewBitmap = previewBuffer.ToBitmap();
                DynMvp.ConsoleEx.WriteLine(string.Format("CalculatorV2::BuildPreviewBitmap - End: {0}[ms]", sw.Elapsed.TotalMilliseconds));
            }

            //{
            //    System.Windows.Media.Imaging.BitmapSource bitmapSource = resizeSheetImage.ToBitmapSource();
            //    sb.AppendLine(string.Format("ToBitmapSource: {0}", sw.Elapsed.TotalMilliseconds));
            //    using (MemoryStream stream = new MemoryStream())
            //    {
            //        BmpBitmapEncoder bmpBitmapEncoder = new BmpBitmapEncoder();
            //        bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            //        bmpBitmapEncoder.Save(stream);

            //        previewBitmap = new Bitmap(stream);
            //    }
            //    sb.AppendLine(string.Format("ToBitmap: {0}", sw.Elapsed.TotalMilliseconds));
            //    previewBitmap?.Save(@"C:\temp\previewBitmap.bmp", System.Drawing.Imaging.ImageFormat.Bmp);
            //}

            //{
            //    ImageD previewImageD = resizeSheetImage.ToImageD();
            //    sb.AppendLine(string.Format("ToImageD: {0}", sw.Elapsed.TotalMilliseconds));

            //    previewBitmap = previewImageD.ToBitmap();
            //    sb.AppendLine(string.Format("ToBitmap: {0}", sw.Elapsed.TotalMilliseconds));

            //    previewImageD.Dispose();
            //}

            return previewBitmap;
        }

        //private Point AlignSheet(AlgoImage algoImage, Point basePosition)
        //{
        //    Point diffPos = Point.Empty;
        //    SheetFinderBase sheerFinder = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName) as SheetFinderBase;
        //    if (sheerFinder == null)
        //        return Point.Empty;

        //    int foundPosX = sheerFinder.FindBasePosition(algoImage, Direction.Horizontal, 20);
        //    int foundPosY = sheerFinder.FindBasePosition(algoImage, Direction.Vertical, 20);
        //    if (foundPosX >= 0 && foundPosY >= 0)
        //        diffPos = new Point(foundPosX - basePosition.X, foundPosY - basePosition.Y);

        //    return diffPos;
        //}

        private UniScanG.Gravure.Data.DefectObj[] FindSticker(AlgoImage algoImage, Rectangle findRect, Calibration calibration, DebugContext debugContext)
        {
            List<UniScanG.Gravure.Data.DefectObj> sheetSubResultList = new List<UniScanG.Gravure.Data.DefectObj>();
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
                int w = (int)Math.Round(calibration.WorldToPixel(sheerFinderParam.SearchSkipWidthMm*1000));
                fullRect.Inflate(-w, 0);
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

        private void InspectRegion(InspectRegion inspectRegion, AlgoImage inspImage, AlgoImage diffImage, AlgoImage binalImage, Point sheetOffset, CancellationToken cancellationToken, DebugContextG debugContextG)
        {
            //Debug.WriteLine(string.Format("CalculatorV2::InspectRegion({0}) Start", debugContext.LogName));
            Stopwatch sw = new Stopwatch();
            //inspectRegion.SetImage(inspImage, resultImage, sheetOffset, calculatorParam.InBarAlignScore, debugContext);

            if (inspectRegion.IsSet)
            {
                inspectRegion.AlgoImage.Save("AlgoImage.bmp", debugContextG);

                sw.Start();

                int count = inspectRegion.InspectLineSets.Length;
                Parallel.For(0, count, new ParallelOptions() { MaxDegreeOfParallelism = 1 }, i =>
                 {
                     DebugContextG linesetDebugContext = debugContextG.Clone();
                     linesetDebugContext.LineSetId = i;

                     InspectLineSet inspectLineSet = inspectRegion.InspectLineSets[i];
                     InspectLineSet(inspectLineSet, cancellationToken, linesetDebugContext);
                 });

                //for (int i = 0; i < count; i++)
                //{
                //    debugContextG.LineSetId = i;

                //    InspectLineSet inspectLineSet = inspectRegion.InspectLineSets[i];
                //    InspectLineSet(inspectLineSet, cancellationToken, debugContextG);
                //}
                //debugContextG.LineSetId = -1;

                debugContextG.ProcessTimeLog.Add(ProcessTimeLog.ProcessTimeLogItem.Calculate, sw.ElapsedMilliseconds);
                sw.Restart();

                // 상/하/좌/우/돈케어 지움
                inspectRegion.ClearDontcare();
                //inspectRegion.CloseBianlImage(2);

                //inspectRegion.GetResult(diffImage, binalImage, sheetOffset);
                debugContextG.ProcessTimeLog.Add(ProcessTimeLog.ProcessTimeLogItem.Binalize, sw.ElapsedMilliseconds);
                sw.Stop();

                //inspectRegion.BinalImage.Save(@"C:\temp\BinalImage.bmp");
            }
        }

        private void InspectLineSet(InspectLineSet inspectLineSet, CancellationToken cancellationToken, DebugContextG debugContextG)
        {
            InspectLine[] inspectLines = Array.FindAll(inspectLineSet.InspectLines, f => f.IsSet).ToArray();

            try
            {
                int src = inspectLineSet.IgnoreSideLine ? 1 : 0;
                int dst = inspectLines.Length - (inspectLineSet.IgnoreSideLine ? 1 : 0);

                for (int i = src; i < dst; i++)
                {
                    if (inspectLines[i].Parent.Parent.RegionInfoG.PassRectList.Count > 0)
                    {
                        if (!inspectLines[i].Parent.Parent.RegionInfoG.PassRectList.Exists(f => inspectLines[i].Rectangle.IntersectsWith(f)))
                            continue;
                    }
                    cancellationToken.ThrowIfCancellationRequested();

                    debugContextG.LineId = i;

                    InspectLines(inspectLines[i], inspectLineSet.ImageProcessing, debugContextG);
                }
                debugContextG.LineId = -1;
            }
            catch (OperationCanceledException) { }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, string.Format("Exception in CalculatorV2::InspectLineSet - {0}: {1}", ex.GetType().Name, ex.Message));
            }
#endif
        }

        private void InspectLines(InspectLine inspectLine, ImageProcessing ip, DebugContextG debugContextG)
        {
            try
            {
                AlgoImage insp = inspectLine.AlgoImage;
                AlgoImage diff = inspectLine.DiffImage;
                AlgoImage thMap = inspectLine.ThresholdMapImage;
                AlgoImage bin = inspectLine.BinalImage;
                AlgoImage egMap = inspectLine.EdgeMapImage;
                AlgoImage mask = inspectLine.MaskImage;
                InspectEdge inspectEdge = inspectLine.EdgeLineElement;
                AlgoImage prev, next;

                if (inspectLine.IsLinked)
                {
                    prev = inspectLine.PrevInspectLine.AlgoImage;
                    next = inspectLine.NextInspectLine.AlgoImage;
                    if (prev == null || next == null)
                        throw new Exception("Cannot find neighbor lines");

                    prev.Save("0-1. A.bmp", debugContextG);
                    insp.Save("0-2. B.bmp", debugContextG);
                    next.Save("0-3. C.bmp", debugContextG);

                    byte inspPrev = 0;
                    byte inspNext = 0;
                    if (inspectLine.GreyAverage >= 0)
                    {
                        if (inspectLine.PrevInspectLine.GreyAverage >= 0)
                            inspPrev = (byte)Math.Round(Math.Abs(inspectLine.GreyAverage - inspectLine.PrevInspectLine.GreyAverage));
                        if (inspectLine.NextInspectLine.GreyAverage >= 0)
                            inspNext = (byte)Math.Round(Math.Abs(inspectLine.GreyAverage - inspectLine.NextInspectLine.GreyAverage));
                    }

                    if (CalculatorParam.ModelParam.UseMultiData)
                    {
                        AlgoImage edge = inspectEdge.EdgeImage;
                        CalculatorV2E.InspectLines(insp, prev, next, edge, diff, thMap, bin, inspPrev, inspNext, debugContextG);
                    }
                    else
                    {
                        AlgoImage[] tempBuf = new AlgoImage[4];
                        if (inspectLine.BufImage.Length == 1)
                        {
                            tempBuf[0] = inspectLine.PrevInspectLine.BufImage[0];
                            tempBuf[1] = inspectLine.BufImage[0];
                            tempBuf[2] = inspectLine.NextInspectLine.BufImage[0];
                            tempBuf[3] = inspectLine.DiffImage;
                        }
                        else
                        {
                            tempBuf[0] = inspectLine.BufImage[0];
                            tempBuf[1] = inspectLine.BufImage[1];
                            tempBuf[2] = inspectLine.BufImage[2];
                            tempBuf[3] = inspectLine.BufImage[3];
                        }

                        ip.Subtract(insp, prev, tempBuf[0]); //notABS
                        ip.Subtract(tempBuf[0], tempBuf[0], inspPrev);
                        tempBuf[0].Save("1-1. B-A.bmp", debugContextG);

                        ip.Subtract(insp, next, tempBuf[1]); //notABS
                        ip.Subtract(tempBuf[1], tempBuf[1], inspNext);
                        tempBuf[1].Save("1-2. B-C.bmp", debugContextG);

                        ip.Min(tempBuf[0], tempBuf[1], tempBuf[2]); //어두운 불량
                        tempBuf[2].Save("1-3. Min.bmp", debugContextG);

                        ip.Subtract(prev, insp, tempBuf[0]); //notABS
                        ip.Subtract(tempBuf[0], tempBuf[0], inspPrev);
                        tempBuf[0].Save("2-1. A-B.bmp", debugContextG);

                        ip.Subtract(next, insp, tempBuf[1]); //notABS
                        ip.Subtract(tempBuf[1], tempBuf[1], inspNext);
                        tempBuf[1].Save("2-2. C-B.bmp", debugContextG);

                        ip.Min(tempBuf[0], tempBuf[1], tempBuf[3]); //밝은 불량
                        tempBuf[3].Save("2-3. Min.bmp", debugContextG);

                        ip.Max(tempBuf[2], tempBuf[3], diff);
                        diff.Save("3. Max.bmp", debugContextG);

                        //  엣지 제거
                        if (inspectEdge != null)
                        {
                            egMap?.Copy(inspectEdge.EdgeImage);
                            inspectEdge.ClearEdge(diff, debugContextG);
                            diff.Save("4. RemoveEdge.bmp", debugContextG);
                        }

                        // 이진화
                        if (inspectLine.SensitiveParam.Multi)
                        {
                            ip.Subtract(diff, thMap, bin);
                            ip.Binarize(bin, 0);
                        }
                        else
                        {
                            ip.Binarize(diff, bin, inspectLine.SensitiveParam.Max);
                        }

                        //엣지 영역 침식
                        if (inspectEdge.EdgeParam.Erode)
                            inspectEdge.ErodeEdge(bin, debugContextG);

                        if (false)
                        {
                            insp.Save(@"D:\temp\0insp.bmp");
                            diff.Save(@"D:\temp\0diff.bmp");
                            bin.Save(@"D:\temp\0bin.bmp");
                            inspectEdge.EdgeImage.Save(@"D:\temp\0edge.bmp");
                        }
                    }
                }

                // dontcare 영역 제거
                Rectangle imageRect = new Rectangle(Point.Empty, diff.Size);

                if (!inspectLine.DontCare.IsEmpty)
                    ip.Clear(bin, inspectLine.DontCare, Color.Black);

                Array.ForEach(inspectLine.IgnoreArea, f =>
                {
                    f.Inflate(10, 50);
                    f.Intersect(imageRect);
                    if (f.Width > 0 && f.Height > 0)
                        ip.Clear(bin, f, Color.Black);
                });

                if (mask != null)
                    ip.And(bin, mask, bin);

                bin.Save("5. RemoveDontcare.bmp", debugContextG);
            }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, string.Format("Exception in CalculatorV2::InspectLine - {0}: {1}", ex.GetType().Name, ex.Message));
            }
#endif
            finally { }
        }
    }
}
