using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Vision;
using UniEye.Base.Settings;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.RCI.Trainer;
using UniScanG.Vision;
using DynMvp.Extensions;

namespace UniScanG.Gravure.Vision.RCI.Calculator
{
    public class CalculatorV3 : Vision.Calculator.CalculatorBase
    {
        public string DebugPath => Path.Combine(PathSettings.Instance().Temp, "CalculatorV3");

        public override Algorithm Clone()
        {
            CalculatorV3 clone = new CalculatorV3();
            clone.CopyFrom(this);
            return clone;
        }

        public override AlgorithmParam CreateParam()
        {
            return new Vision.Calculator.CalculatorParam(true);
        }

        public override ProcessBufferSetG CreateProcessingBuffer(float scaleFactor, bool isMultiLayer, int width, int height)
        {
            UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;
            return new ProcessBufferSetG3(model, scaleFactor, isMultiLayer, width, height);
        }

        #region Override
        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new CalculatorResultV3();
        }
        #endregion

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            SheetInspectParam sheetInspectParam = algorithmInspectParam as SheetInspectParam;
            Calibration calibration = sheetInspectParam.CameraCalibration;
            DebugContextG debugContextG = algorithmInspectParam.DebugContext as DebugContextG;
            UniScanG.Data.Model.Model model = sheetInspectParam.Model;
            RCIOptions options = model.RCIOptions;
            RCITrainResult trainResult = model.RCITrainResult;
            ProcessBufferSetG3 processBufferSet = sheetInspectParam.ProcessBufferSet as ProcessBufferSetG3;
            CancellationToken cancellationToken = sheetInspectParam.CancellationToken;
            bool isTestInspection = sheetInspectParam.TestInspect;
            bool rightToLeft = AlgorithmSetting.Instance().RCIGlobalOptions.RightToLeft;

            CalculatorResultV3 calculatorResult = (CalculatorResultV3)CreateAlgorithmResult();
            List<BlockResult> blockResultList = new List<BlockResult>();
            AlgoImage targetFullImage = null, resultGrayFullImage = null, resultBinFullImage = null;
            AlgoImage modelRoiImage = null, modelBgRoiImage = null, weightRoiImage = null;
            AlgoImage targetRoiImage = null, resultGreyRoiImage = null, resultBinRoiImage = null;
            try
            {
                trainResult.ThrowIfInvalid();

                modelRoiImage = processBufferSet.ModelRoiImage;
                modelBgRoiImage = processBufferSet.ModelBgRoiImage;
                weightRoiImage = processBufferSet.WeightRoiImage;

                targetFullImage = processBufferSet.TargetFullImage;
                resultGrayFullImage = processBufferSet.ResultGrayFullImage;
                resultBinFullImage = processBufferSet.ResultBinFullImage;

                Stopwatch sw = Stopwatch.StartNew();
                resultGrayFullImage.Clear();
                resultBinFullImage.Clear();
                debugContextG?.ProcessTimeLog.SetSerial(ProcessTimeLog.ProcessTimeLogItem.Calculate);

                Rectangle roiSeedRange = options.ROISeedRect;
                if (roiSeedRange.Width <= 0) roiSeedRange.Width = targetFullImage.Width - roiSeedRange.X;
                if (roiSeedRange.Height <= 0) roiSeedRange.Height = targetFullImage.Height - roiSeedRange.Y;
                RCIHelper.FindROI(targetFullImage, roiSeedRange, processBufferSet.FindRoiLineBuffer, Size.Empty, out Rectangle roi, out float slope, debugContextG);

                DefectObj[] stickers = null;
                if (AlgorithmSetting.Instance().RCIGlobalOptions.StickerOption.Use && AlgorithmSetting.Instance().UseExtSticker)
                {
                    Rectangle stickerFindRect = Rectangle.Empty;
                    SheetFinder.SheetFinderBaseParam sheerFinderParam = SheetFinder.SheetFinderBase.SheetFinderBaseParam;
                    if (sheerFinderParam != null)
                    {
                        if (rightToLeft)
                            stickerFindRect = Rectangle.FromLTRB(roi.Right, 0, targetFullImage.Width, targetFullImage.Height);
                        else
                            stickerFindRect = Rectangle.FromLTRB(0, 0, roi.Left, targetFullImage.Height);

                        stickers = Vision.Calculator.StickerFinder.FindSticker(targetFullImage, stickerFindRect, calibration, debugContextG);
                    }

                }
                targetRoiImage = targetFullImage.GetSubImage(roi);
                resultGreyRoiImage = resultGrayFullImage.GetSubImage(roi);
                resultBinRoiImage = resultBinFullImage.GetSubImage(roi);
                //targetRoiImage.Save(Path.Combine(this.DebugPath, @"targetRoiImage.bmp"));
                //modelRoiImage.Save(Path.Combine(this.DebugPath, @"modelRoiImage.bmp"));

                Dictionary<WorkPoint, Rectangle> modelRects = GetModelRectsWithPTM(targetRoiImage, processBufferSet, rightToLeft);
                Func<WorkPoint, Rectangle> offsetFunc = new Func<WorkPoint, Rectangle>(f =>
                {
                    return modelRects[f];
                });

                // -----
                //WorkPoint[] wps = Array.FindAll(processBufferSet.AlignRects, f => f.Row == 111);
                //WorkPoint wp = wps.First();
                //if (wp != null)
                //{
                //    Rectangle rtTarget = new Rectangle(wp.Point, wp.BlockSize);
                //    Rectangle rtModel = modelRects[wp];
                //    if (AlgorithmSetting.Instance().RCIGlobalOptions.RightToLeft)
                //    {
                //        //rtTarget.Offset(-wp.BlockSize.Width, 0);
                //        rtTarget = RCIHelper.Anchor(true, targetRoiImage.Size, rtTarget);
                //        rtModel = RCIHelper.Anchor(true, modelRoiImage.Size, rtModel);
                //    }
                //    rtTarget.Width = rtTarget.Width * 10;
                //    rtModel.Width = rtModel.Width * 10;
                //    using (AlgoImage algoImage = modelRoiImage.GetSubImage(rtModel))
                //        algoImage.Save(@"C:\temp\model.bmp");
                //    using (AlgoImage algoImage = targetRoiImage.GetSubImage(rtTarget))
                //        algoImage.Save(@"C:\temp\target.bmp");
                //}
                // -----

                ParallelOptions parallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = AlgorithmSetting.Instance().RCIGlobalOptions.Parall ? -1 : 1
                };

                Parallel.For(0, trainResult.WorkPointRowCount, parallelOptions, new Action<int>(row =>
                {
                    List<BlockResult> list = CompareRow(row, processBufferSet, offsetFunc, options.SplitX, rightToLeft,
                        modelRoiImage, modelBgRoiImage, weightRoiImage, targetRoiImage, resultGreyRoiImage, cancellationToken);

                    lock (blockResultList)
                        blockResultList.AddRange(list);
                }));

                processBufferSet.BlockResults = blockResultList.OrderBy(f => f.Index).ToArray();
                
                //DynMvp.ConsoleEx.WriteLine($"Pat: {debugContextG.PatternId}");
                //DynMvp.ConsoleEx.WriteLine($"First BlockTarget: {processBufferSet.BlockResults.First().TargetRect}");
                //DynMvp.ConsoleEx.WriteLine($"Last  BlockTarget: {processBufferSet.BlockResults.Last().TargetRect}");

                processBufferSet.TargetRoi = roi;

                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(resultGreyRoiImage);
                ip.Binarize(resultGreyRoiImage, resultBinRoiImage, options.SensitiveOption.High);

                sw.Stop();
                debugContextG.ProcessTimeLog.Add(ProcessTimeLog.ProcessTimeLogItem.Calculate, sw.ElapsedMilliseconds);


                PointF roiOffset;
                if (rightToLeft)
                    roiOffset = DrawingHelper.Subtract(roi.RightTop(), trainResult.ROI.RightTop());
                else
                    roiOffset = DrawingHelper.Subtract(roi.Location, trainResult.ROI.Location);

                if (stickers != null)
                    calculatorResult.SheetSubResultList.AddRange(stickers);

                calculatorResult.OffsetFound = new SizeF(roiOffset);
                calculatorResult.SheetSizePx = processBufferSet.PatternSizePx;
                calculatorResult.SheetSize = sheetInspectParam.CameraCalibration.PixelToWorld(processBufferSet.PatternSizePx);
                calculatorResult.Good = calculatorResult.SheetSubResultList.Count == 0;
                calculatorResult.UpdateSpandTime();

                calculatorResult.FoundRoi = roi;
                calculatorResult.PTMResults = modelRects;
                calculatorResult.BlockResults = (BlockResult[])processBufferSet.BlockResults.Clone();
                calculatorResult.ElapsedMs = sw.ElapsedMilliseconds;

                bool buildDebugImage = options.BuildDebugImage || isTestInspection;
#if DEBUG
                buildDebugImage = true;
#endif
                if (buildDebugImage)
                    calculatorResult.DebugImageD = BuildDebugImage(processBufferSet, modelRects, modelRoiImage, modelBgRoiImage, targetRoiImage, weightRoiImage, resultGreyRoiImage, calculatorResult, 25);

                //resultGreyRoiImage?.Save(@"C:\temp\resultGreyRoiImage.bmp");

                return calculatorResult;
            }
            finally
            {
                resultBinRoiImage?.Dispose();
                resultGreyRoiImage?.Dispose();
                targetRoiImage?.Dispose();
            }
        }


        private static Dictionary<WorkPoint, Rectangle> GetModelRectsWithPTM(AlgoImage targetRoiImage, ProcessBufferSetG3 bufferItem, bool anchorRight)
        {
            SizeF[] ofs = new SizeF[bufferItem.WorkPoints.Length];
            // Head-Tail matching. Linea Approximate

            // y
            {
                float[][] yOffsets = bufferItem.AlignRectsByVertical.Select(f =>
                {
                    float yBias = 0;
                    return f.Select(g =>
                    {
                        if (g == null)
                            return yBias;
                        Rectangle prjRectangle = RCIHelper.Anchor(anchorRight, targetRoiImage.Size, g.GetPrjRectangle());
                        return GetWorkPointOffset(targetRoiImage, bufferItem, g.Index, prjRectangle, Direction.Vertical, ref yBias);
                    }).ToArray();
                }).ToArray();

                for (int row = 0; row < bufferItem.WorkPointRowCount; row++)
                {
                    for (int i = 1; i < bufferItem.AlignRectsByVertical.Length; i++)
                    {
                        WorkPoint src = bufferItem.AlignRectsByVertical[i - 1][row];
                        WorkPoint dst = bufferItem.AlignRectsByVertical[i][row];
                        if (dst.Column > src.Column)
                        {
                            int start = (i == 1) ? 0 : src.Column;
                            int end = (i == bufferItem.AlignRectsByVertical.Length - 1) ? bufferItem.WorkPointColumnCount - 1 : dst.Column;
                            float[] approximus = Approximate(src.Column, yOffsets[i - 1][row], dst.Column, yOffsets[i][row], start, end);
                            for (int col = start; col <= end; col++)
                            {
                                int idx = bufferItem.WorkRectsByRow[row][col].Index;
                                ofs[idx].Height = approximus[col - start];
                            }
                        }
                    }

                    WorkPoint first = bufferItem.AlignRectsByVertical.First()[row];
                    int firstCol = bufferItem.WorkRectsByRow[row].First().Column;
                    if (first.Column > firstCol)
                    {
                        for (int col = 0; col <= first.Column; col++)
                        {
                            int idx = bufferItem.WorkRectsByRow[row][col].Index;
                            ofs[idx].Height = ofs[first.Index].Height;
                        }
                    }

                    WorkPoint last = bufferItem.AlignRectsByVertical.Last()[row];
                    int lastCol = bufferItem.WorkRectsByRow[row].Last().Column;
                    if (last.Column < lastCol)
                    {
                        for (int col = last.Column; col <= lastCol; col++)
                        {
                            int idx = bufferItem.WorkRectsByRow[row][col].Index;
                            ofs[idx].Height = ofs[last.Index].Height;
                        }
                    }
                }
            }

            // x
            { 
                float[][] xOffsets = bufferItem.AlignRectsByHorizontal.Select(f =>
                {
                    float xBias = 0;
                    return f.Select(g =>
                    {
                        if (g == null)
                            return xBias;

                        Rectangle prjRectangle = RCIHelper.Anchor(anchorRight, targetRoiImage.Size, g.GetPrjRectangle());
                        prjRectangle.Offset(0, -(int)Math.Round(ofs[g.Index].Height));
                        //using (AlgoImage algoImage = targetRoiImage.GetSubImage(prjRectangle))
                        //    algoImage.Save(@"C:\temp\prjRectangle.bmp");
                        return GetWorkPointOffset(targetRoiImage, bufferItem, g.Index, prjRectangle, Direction.Horizontal, ref xBias) * (anchorRight ? -1 : 1);
                    }).ToArray();
                }).ToArray();

                for (int col = 0; col < bufferItem.WorkPointColumnCount; col++)
                {
                    for (int i = 1; i < bufferItem.AlignRectsByHorizontal.Length; i++)
                    {
                        WorkPoint src = bufferItem.AlignRectsByHorizontal[i - 1][col];
                        WorkPoint dst = bufferItem.AlignRectsByHorizontal[i][col];
                        if (dst.Row > src.Row)
                        {
                            float[] approximus = Approximate(src.Row, xOffsets[i - 1][col], dst.Row, xOffsets[i][col], src.Row, dst.Row);
                            for (int row = src.Row; row <= dst.Row; row++)
                            {
                                int idx = bufferItem.WorkRectsByColumn[col][row].Index;
                                ofs[idx].Width = approximus[row - src.Row] ;
                            }
                        }
                    }

                    WorkPoint first = bufferItem.AlignRectsByHorizontal.First()[col];
                    int firstRow = bufferItem.WorkRectsByColumn[col].First().Row;
                    if (first.Row > firstRow)
                    {
                        for (int row = 0; row < first.Row; row++)
                        {
                            int idx = bufferItem.WorkRectsByColumn[col][row].Index;
                            ofs[idx].Width = ofs[first.Index].Width;
                        }
                    }

                    WorkPoint last = bufferItem.AlignRectsByHorizontal.Last()[col];
                    int lastRow = bufferItem.WorkRectsByColumn[col].Last().Row;
                    if (last.Row < lastRow)
                    {
                        for (int row = last.Row; row <= lastRow; row++)
                        {
                            int idx = bufferItem.WorkRectsByColumn[col][row].Index;
                            ofs[idx].Width = ofs[last.Index].Width;
                        }
                    }
                }
            }

            //using (StreamWriter sw = new StreamWriter(@"D:\temp\RCIProjections\offset.txt", false))
            //{
            //    for (int i = 0; i < bufferItem.WorkPoints.Length; i++)
            //    {
            //        WorkPoint workPoint = bufferItem.WorkPoints[i];
            //        if (workPoint.IsReferenceX >= 0)
            //        {
            //            Rectangle r = workPoint.GetPrjInflatedRectangle();
            //            r = DrawingHelper.Offset(r, new Point(Size.Round(ofs[i])), true);
            //            r = RCIHelper.Anchor(anchorRight, targetRoiImage.Size, r);
            //            using (AlgoImage algoImage = targetRoiImage.GetSubImage(r))
            //                algoImage.Save($@"D:\temp\RCIProjections\{i:00000}.bmp");
            //        }
            //        sw.WriteLine($"{i:00000}, dX: {ofs[i].Width:F03}, dY: {ofs[i].Height:F03}");
            //    }
            //    sw.Flush();
            //    sw.Close();
            //}

            //targetRoiImage.Save(@"C:\temp\targetRoiImage.bmp");
            int dicLength = (bufferItem.WorkPointColumnCount) * (bufferItem.WorkPointRowCount);
            Dictionary<WorkPoint, Rectangle> dic = new Dictionary<WorkPoint, Rectangle>(dicLength);
            for (int r0 = 0; r0 < bufferItem.WorkPointRowCount ; r0++)
            {
                int r1 = (r0 == bufferItem.WorkPointRowCount - 1) ? -1 : r0 + 1;

                for (int c0 = 0; c0 < bufferItem.WorkPointColumnCount ; c0++)
                {
                    int c1 = (c0 == bufferItem.WorkPointColumnCount - 1) ? -1 : c0 + 1;

                    Rectangle rectangle = ds(r0, r1, c0, c1, ofs, bufferItem, out int workPointIndex);
                    dic.Add(bufferItem.WorkPoints[workPointIndex], rectangle);

                    Rectangle t = rectangle;
                    t = RCIHelper.Anchor(anchorRight, targetRoiImage.Size, t);
                    

                    Rectangle m = bufferItem.WorkPoints[workPointIndex].GetTeachRectangle();
                    Size wh = DrawingHelper.Subtract(t.Size, m.Size);
                }
            }
          
            return dic;
        }

        private static Rectangle ds(int r0, int r1, int c0, int c1, SizeF[] offsets, ProcessBufferSetG3 bufferItem, out int workPointIndex)
        {
            int baseIdx = r0 * bufferItem.WorkPointColumnCount + c0;
            Point basePt = Point.Add(bufferItem.WorkPoints[baseIdx].Point, Size.Round(offsets[baseIdx]));

            Size size = Size.Empty;
            if (c1 < 0)
            {
                size.Width = bufferItem.WorkPoints[baseIdx].BlockSize.Width;
            }
            else
            {
                int rIdx = r0 * bufferItem.WorkPointColumnCount + c1;
                Point rPt = Point.Add(bufferItem.WorkPoints[rIdx].Point, Size.Round(offsets[rIdx]));
                size.Width = rPt.X - basePt.X;
            }


            if (r1 < 0)
            {
                size.Height = bufferItem.WorkPoints[baseIdx].BlockSize.Height;
            }
            else
            {
                int bIdx = r1 * bufferItem.WorkPointColumnCount + c0;
                Point bPt = Point.Add(bufferItem.WorkPoints[bIdx].Point, Size.Round(offsets[bIdx]));
                size.Height = bPt.Y - basePt.Y;
            }

            workPointIndex = baseIdx;

            Rectangle rectangle = new Rectangle(basePt, size);
            return rectangle;
        }

        private static float GetWorkPointOffset(AlgoImage targetRoiImage, ProcessBufferSetG3 bufferItem, int workPointIndex, Rectangle workRect, Direction direction, ref float bias)
        {
            Rectangle rect = workRect;
            var prjBufTuple = bufferItem.GetPrjBufImg(workPointIndex);
            if (prjBufTuple == null)
                return bias;

            BlockProjection prjImg = prjBufTuple.Item1;
            BlockProjection bufImg = prjBufTuple.Item2;
            BlockProjection resImg = prjBufTuple.Item3;
            PTMLogger pTMLogger = prjBufTuple.Item4;

            Point ptOffset = Point.Empty;
            if (direction == Direction.Horizontal)
                ptOffset.X = -(int)Math.Round(bias);
            else
                ptOffset.Y = -(int)Math.Round(bias);

            rect.Offset(ptOffset);
            if (rect != Rectangle.Intersect(rect, new Rectangle(Point.Empty, targetRoiImage.Size)))
                return bias;

            Debug.Assert(rect.Width > 0 && rect.Height > 0);
            //Debug.Assert(workPointIndex != 379, "Stop for debug");
            using (AlgoImage targetOffsetImage = targetRoiImage.GetSubImage(rect))
                bufImg.Build(targetOffsetImage, direction);

            float[] scores;
            float offset;
            if (direction == Direction.Horizontal)
                offset = RCIHelper.GetTmOffset(prjImg.Horizental, bufImg.Horizental, resImg.Horizental, out scores) - prjImg.Inflate.Width;
            else
                offset = RCIHelper.GetTmOffset(prjImg.Veritical, bufImg.Veritical, resImg.Veritical, out scores) - prjImg.Inflate.Height;
            //Debug.Assert(Math.Abs(offset) < 10);

            //byte[] prj = prjImg.Veritical.GetByte();
            //byte[] buf = bufImg.Veritical.GetByte();
            //RCIHelper.SaveSingles(@"C:\temp\prj.txt", prj.Select(f => (float)f));
            //RCIHelper.SaveSingles(@"C:\temp\buf.txt", buf.Select(f => (float)f));
            //if (Array.TrueForAll(scores, f => f == 0))
            //    offset = 0;

            pTMLogger.Direction = direction;
            pTMLogger.MaxPos = offset;
            pTMLogger.TargetOffset = (int)Math.Round(bias);

            bias += offset;
            return bias;
        }


        private static float[] Approximate(int pos0, float val0, int pos1, float val1, int src, int dst)
        {
            int length = dst - src + 1;
            // y0=ax0+b
            // y1=ax1+b
            float[] approximus = new float[length];
            float a = (val1 - val0) / (pos1 - pos0);
            float b = val0 - a * pos0;
            for (int i = 0; i < length; i++)
                approximus[i] = a * (i + src) + b;

            return approximus;
        }


        private static List<BlockResult> CompareRow(int row, ProcessBufferSetG3 processBufferSet, Func<WorkPoint, Rectangle> OffsetFunc, bool splitX, bool anchorRight,
            AlgoImage modelRoiImage, AlgoImage modelBgRoiImage, AlgoImage weightRoiImage, AlgoImage targetRoiImage, AlgoImage resultRoiImage, CancellationToken cancellationToken)
        {
            List<BlockResult> list = new List<BlockResult>();
            WorkPoint[] workPoints = processBufferSet.WorkRectsByRow[row];

            List<WorkPoint> workPointList = new List<WorkPoint>();
            List<Rectangle> targetRectList = new List<Rectangle>();
            List<Rectangle> modelRectList = new List<Rectangle>();
            int dWidth = 0;
            int loop = workPoints.Length + 1;
            for (int col = 0; col < loop; col++)
            {
                WorkPoint workPoint = null;
                Rectangle targetRect = Rectangle.Empty;
                Rectangle modelRect = Rectangle.Empty;
                int dW = 0;

                bool isLastLoop = (col == workPoints.Length);
                if (!isLastLoop)
                {
                    int workPointIndex = row * processBufferSet.WorkPointColumnCount + col;
                    workPoint = workPoints[col];

                    targetRect = workPoint.GetTeachRectangle();
                    modelRect = OffsetFunc(workPoint);

                    Debug.Assert(targetRect.Left >= 0 && targetRect.Top >= 0);
                    //targetRect = RCIHelper.Anchor(anchorRight, targetRoiImage.Size, targetRect);
                    //modelRect = RCIHelper.Anchor(anchorRight, modelRoiImage.Size, modelRect);

                    int d1 = targetRoiImage.Width - targetRect.Right;
                    int d2 = modelRoiImage.Width - modelRect.Right;

                    dW = DrawingHelper.Subtract(modelRect.Size, targetRect.Size).Width;

                    if (workPoint.Use)
                    {
                        //Debug.Assert(targetRect.Size == modelRect.Size);
                        if (workPointList.Count == 0)
                        {
                            workPointList.Add(workPoint);
                            targetRectList.Add(targetRect);
                            modelRectList.Add(modelRect);
                            dWidth = dW;
                            continue;
                        }
                        else
                        {
                            Rectangle lastModelRect = modelRectList.Last();
                            if ((lastModelRect.Top == modelRect.Top) && (lastModelRect.Bottom == modelRect.Bottom) && (splitX ? (dWidth == dW) : true))
                            {
                                workPointList.Add(workPoint);
                                targetRectList.Add(targetRect);
                                modelRectList.Add(modelRect);
                                dWidth = dW;
                                continue;
                            }
                        }
                    }
                }

                if (targetRectList.Count > 0)
                {
                    //Debug.Assert(!(workPointList.Exists(f => f.Column == 345 && f.Row == 26)), "Assert for Debug");

                    //int meanBgValue = (int)Math.Round(workPoints.Average(f => f.MeanBgGv));
                    Rectangle unionTargetRect = targetRectList.Aggregate((f, g) => Rectangle.Union(f, g));
                    Rectangle unionModelRect = modelRectList.Aggregate((f, g) => Rectangle.Union(f, g));
                    Point unionOffset = DrawingHelper.Subtract(unionModelRect.Location, unionTargetRect.Location);
                    //new Point(unionModelRect.Right - unionTargetRect.Right, unionModelRect.Top - unionTargetRect.Top) :
                    //new Point(unionModelRect.Left - unionTargetRect.Left, unionModelRect.Top - unionTargetRect.Top);

                    int sumTargetW = targetRectList.Sum(f => f.Width);
                    Debug.Assert(unionTargetRect.Width == sumTargetW);

                    int sumModelW = modelRectList.Sum(f => f.Width);
                    Debug.Assert(unionModelRect.Width == sumModelW);

                    if (anchorRight)
                    {
                        unionTargetRect = RCIHelper.Anchor(anchorRight, targetRoiImage.Size, unionTargetRect);
                        unionModelRect = RCIHelper.Anchor(anchorRight, modelRoiImage.Size, unionModelRect);
                    }

                    //if (row == 79)
                    //{
                    //    WorkPoint wp = workPointList.First();
                    //    using (AlgoImage algoImage = targetRoiImage.GetSubImage(unionTargetRect))
                    //        algoImage.Save($@"C:\temp\R{wp.Row}_C{wp.Column}_UnionTarget.bmp");
                    //    using (AlgoImage algoImage = modelRoiImage.GetSubImage(unionModelRect))
                    //        algoImage.Save($@"C:\temp\R{wp.Row}_C{wp.Column}_UnionModel.bmp");
                    //}

                    // --------------
                    //int minWidth = Math.Min(unionTargetRect.Width, unionModelRect.Width);
                    //if (anchorRight)
                    //{
                    //    unionTargetRect.X = unionTargetRect.Right - minWidth;
                    //    unionModelRect.X = unionModelRect.Right - minWidth;
                    //}
                    //unionTargetRect.Width = minWidth;
                    //unionModelRect.Width = minWidth;
                    // --------------

                    float bgMean = -1;
                    try
                    {
                        //Debug.Assert(workPointList.First().Index != 15808, "Assert for Debug");
                        if (!cancellationToken.IsCancellationRequested)
                            bgMean = CompareRect(unionTargetRect, unionModelRect, modelRoiImage, modelBgRoiImage, weightRoiImage, targetRoiImage, resultRoiImage);
                    }
                    catch (Exception ex)
                    {
                        WorkPoint firstWorkRect = workPointList.First();
                        LogHelper.Error(LoggerType.Error, $"CalculatorV3::CompareRow - {ex.Message}. Row: {firstWorkRect.Row}, Col: {firstWorkRect.Column}");
                    }

                    lock (list)
                    {
                        WorkPoint firstWorkRect = workPointList.First();
                        int wRectIndex = firstWorkRect.Index;
                        int wRectColumn = firstWorkRect.Column;
                        int wRectRow = firstWorkRect.Row;
                        bool hasReference = workPointList.Any(f => f.IsReference);
                        list.Add(new BlockResult(wRectIndex, unionTargetRect, unionModelRect, unionOffset, wRectColumn, wRectRow) { MeanBgValue = bgMean, HasReference = hasReference });
                    }

                    workPointList.Clear();
                    targetRectList.Clear();
                    modelRectList.Clear();
                    if (workPoint != null && workPoint.Use)
                    {
                        workPointList.Add(workPoint);
                        targetRectList.Add(targetRect);
                        modelRectList.Add(modelRect);
                        dWidth = dW;
                    }
                }
            }
            return list;
        }

        private static float CompareRect(Rectangle unionTargetRect, Rectangle unionModelRect,
            AlgoImage modelRoiImage, AlgoImage modelBgRoiImage, AlgoImage weightRoiImage, AlgoImage targetRoiImage, AlgoImage resultRoiImage)
        {
            AlgoImage modelSubImage = null, modelBgSubImage = null, weightSubImage = null, targetSubImage = null, resultSubImage = null;
            try
            {
                if (!(targetRoiImage.IsInnerRect(unionTargetRect) && modelRoiImage.IsInnerRect(unionModelRect)))
                {
                    LogHelper.Error(LoggerType.Error, new Exception("Out of Image bound"));
                    return -1;
                }

                modelSubImage = modelRoiImage.GetSubImage(unionModelRect);
                modelBgSubImage = modelBgRoiImage.GetSubImage(unionModelRect);
                weightSubImage = weightRoiImage.GetSubImage(unionModelRect);
                targetSubImage = targetRoiImage.GetSubImage(unionTargetRect);
                resultSubImage = resultRoiImage.GetSubImage(unionTargetRect);

                float bgMean = Subtract(modelSubImage, modelBgSubImage, weightSubImage, targetSubImage, resultSubImage, -1);
                //modelSubImage.Save(@"C:\temp\modelSubImage.bmp");
                //targetSubImage.Save(@"C:\temp\targetSubImage.bmp");
                //weightSubImage.Save(@"C:\temp\weightSubImage.bmp");
                //resultSubImage.Save(@"C:\temp\resultSubImage.bmp");
                return bgMean;

            }
            finally
            {
                modelSubImage?.Dispose();
                modelBgSubImage?.Dispose();
                weightSubImage?.Dispose();
                targetSubImage?.Dispose();
                resultSubImage?.Dispose();
            }
        }

        private static float Subtract(AlgoImage modelImage, AlgoImage modelBgImage, AlgoImage weightImage, AlgoImage targetImage, AlgoImage resultImage, int meanBgGv)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(modelImage);
            //Debug.Assert(modelImage.Width == targetImage.Width);
            bool save = false;
            float gvMean = 0;

            int w = Math.Min(modelImage.Width , targetImage.Width); // 일단 이렇게...
            int ox = (Math.Max(modelImage.Width, targetImage.Width) - w) / 2;
            int hm = modelImage.Height;
            int ht = targetImage.Height;
            if (hm == ht)
            {
                ip.Subtract(modelImage, targetImage, resultImage, true);
                ip.Subtract(resultImage, weightImage, resultImage, false);

                if (meanBgGv >= 0)
                    gvMean = ip.GetGreyAverage(targetImage, modelBgImage);
            }
            else
            {
                WhatToNameThis[] sliceYs = RangeSplitter.SplitRange3(ht, hm);
                Debug.Assert(sliceYs.Sum(f => f.Length) == ht);
                Debug.Assert(sliceYs.Last().ModelStart + sliceYs.Last().Length == hm);
                float[] gvMeans = new float[sliceYs.Length];
                for (int y = 0; y < sliceYs.Length; y++)
                {
                    WhatToNameThis sliceY = sliceYs[y];

                    Rectangle targetRect = new Rectangle(0, sliceY.TargetStart, w, sliceY.Length);
                    Rectangle modelRect = new Rectangle(0, sliceY.ModelStart, w, sliceY.Length);
                    if (modelImage.Width > targetImage.Width)
                    {
                        modelRect.X = ox;
                    }
                    else
                    {
                        targetRect.X = ox;
                    }

                    AlgoImage modelSubImage = null, modelBgSubImage = null, weightSubImage = null, targetSubImage = null, resultSubImage = null;
                    try
                    {
                        modelSubImage = modelImage.GetSubImage(modelRect);
                        modelBgSubImage = modelBgImage.GetSubImage(modelRect);
                        weightSubImage = weightImage.GetSubImage(modelRect);
                        targetSubImage = targetImage.GetSubImage(targetRect);
                        resultSubImage = resultImage.GetSubImage(targetRect);

                        ip.Subtract(modelSubImage, targetSubImage, resultSubImage, true);

                        if (save)
                        {
                            modelSubImage.Save(@"C:\temp\modelSubImage.bmp");
                            targetSubImage.Save(@"C:\temp\targetSubImage.bmp");
                            resultSubImage.Save(@"C:\temp\resultSubImage.bmp");
                        }
                        ip.Subtract(resultSubImage, weightSubImage, resultSubImage, false);

                        if (meanBgGv >= 0)
                            gvMeans[y] = ip.GetGreyAverage(targetSubImage, modelBgSubImage);
                    }
                    finally
                    {
                        resultSubImage?.Dispose();
                        targetSubImage?.Dispose();
                        weightSubImage?.Dispose();
                        modelBgSubImage?.Dispose();
                        modelSubImage?.Dispose();
                    }
                }
                gvMean = (int)Math.Round(gvMeans.Average());
            }

            int gvDiff = (int)Math.Max(0, meanBgGv - gvMean);
            if (gvDiff > 0 && gvMean > 0)
                ip.Subtract(resultImage, resultImage, gvDiff);

            return gvMean;
        }

        internal static ImageD BuildDebugImage(ProcessBufferSetG3 buffer, Dictionary<WorkPoint, Rectangle> modelRects , AlgoImage modelRoiImage, AlgoImage modelBgRoiImage, AlgoImage targetRoiImage, AlgoImage weightRoiImage, AlgoImage resultRoiImage, CalculatorResultV3 result, byte debugThreshold)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(modelRoiImage);

            List<HighDiffBlock> list = new List<HighDiffBlock>();
            ImageD imageD = null;

            try
            {
                int hCount = buffer.PTMLogger.Count(f => f.Direction == Direction.Horizontal);
                int vCount = buffer.PTMLogger.Count(f => f.Direction == Direction.Vertical);
                result.PTMLoggers = buffer.PTMLogger.Select(f =>
                {
                    PTMLogger logger = f;
                    WorkPoint workPoint = logger.WorkPoint;
                    int id = buffer.BlockKeys.IndexOf(workPoint.Index);
                    logger.ModelPrj = buffer.BlockPrjImg[id].GetWorkRectProjection().PrjV;
                    logger.TargetPrj = buffer.BlockBufImg[id].GetWorkRectProjection().PrjV;
                    logger.Scores = buffer.BlockResImg[id].GetWorkRectProjection().PrjV;

                    Rectangle rectangle = RCIHelper.Anchor(buffer.Model.RCITrainResult.RightToLeft, modelRoiImage.Size, workPoint.GetPrjRectangle());
                    //if (modelRoiImage.IsInnerRect(rectangle))
                    {
                        using (AlgoImage algoImage = modelRoiImage.GetSubImage(rectangle))
                            logger.ModelImageD = algoImage.ToImageD();
                    }

                    int offset = (int)Math.Round(logger.TargetOffset + logger.MaxPos);
                    if (logger.Direction == Direction.Horizontal)
                        rectangle.Offset(-offset, 0);
                    if (logger.Direction == Direction.Vertical)
                        rectangle.Offset(0, -offset);

                    if (targetRoiImage.IsInnerRect(rectangle))
                    {
                        using (AlgoImage algoImage = targetRoiImage.GetSubImage(rectangle))
                            logger.TargetImageD = algoImage.ToImageD();
                    }

                    return logger;
                }).ToArray();

                using (AlgoImage debugRoiImage = targetRoiImage.Clone())
                {
                    ip.Div(debugRoiImage, debugRoiImage, 2);
                    ip.DrawText(debugRoiImage, new Point(5, 5), 127, $"Time: {result.ElapsedMs}[ms]");
                    ip.DrawText(debugRoiImage, new Point(175, 5), 127, $"Debug Threshold: {debugThreshold}[lv]");
                    ip.DrawText(debugRoiImage, new Point(5, 25), 127, $"{result.FoundRoi.Location}");
                    ip.DrawText(debugRoiImage, new Point(175, 25), 127, $"delta: {result.OffsetFound}");

                    for (int i = 0; i < buffer.PTMLogger.Count; i++)
                    {
                        PTMLogger logger = buffer.PTMLogger[i];
                        Rectangle rectangle = RCIHelper.Anchor(buffer.Model.RCITrainResult.RightToLeft, debugRoiImage.Size, logger.WorkPoint.GetPrjRectangle());
                        ip.DrawRect(debugRoiImage, rectangle, 200, false);

                        int offset = -(int)Math.Round(logger.TargetOffset + logger.MaxPos);
                        string message = logger.Direction == Direction.Horizontal ? $"dX: {offset}" : $"dY: {offset}";
                        ip.DrawText(debugRoiImage, DrawingHelper.Add(rectangle.Location, new Point(5, -5)), 200, message);
                    }

                    Rectangle resultRoiImageRect = new Rectangle(Point.Empty, resultRoiImage.Size);
                    for (int i = 0; i < buffer.BlockResults.Length; i++)
                    {
                        BlockResult block = buffer.BlockResults[i];
                        Rectangle targetRect = block.TargetRect;
                        Rectangle modelRect = block.ModelRect;
                        Point offset = block.Offset;
                        Size diff = block.Diff;

                        targetRect.Intersect(resultRoiImageRect);
                        using (AlgoImage resultSubImage = resultRoiImage.GetSubImage(targetRect))
                            block.MaxValue = (int)ip.GetGreyMax(resultSubImage);

                        using (AlgoImage targetSubImage = targetRoiImage.GetSubImage(targetRect))
                        {
                            using (AlgoImage modelBgSubImage = modelBgRoiImage.GetSubImage(modelRect))
                            {
                                //block.MeanBgValue = ip.GetGreyAverage(targetSubImage, modelBgSubImage);
                            }
                        }

                        ip.DrawRect(debugRoiImage, targetRect, 127, false);
                        using (AlgoImage debugSubImage = debugRoiImage.GetSubImage(targetRect))
                        {
                            ip.DrawText(debugSubImage, new Point(5, 5), 127, $"({block.Index})");
                            ip.DrawText(debugSubImage, new Point(5, 25), 127, $"X:{block.Column}/{targetRect.X}");
                            ip.DrawText(debugSubImage, new Point(5, 45), 127, $"Y:{block.Row}/{targetRect.Y}");

                            ip.DrawText(debugSubImage, new Point(5, 85), 127, $"V:{block.MaxValue:F0}");
                            ip.DrawText(debugSubImage, new Point(5, 105), 127, $"dXY:{block.Offset.X:F1}, {block.Offset.Y:F1}");
                            ip.DrawText(debugSubImage, new Point(5, 125), 127, $"dWH:{block.Diff.Width:F1}, {block.Diff.Height:F1}");
                            ip.DrawText(debugSubImage, new Point(5, 145), 127, $"BgV:{block.MeanBgValue:F1}");
                        }

                        if (block.MaxValue > debugThreshold)
                        {
                            HighDiffBlock compareHighDiffItem = new HighDiffBlock()
                            {
                                Index = block.Index,
                                TargetRectangle = targetRect,
                                ModelRectangle = modelRect,
                                TmOffsetPoint = offset,
                                Threshold = debugThreshold,
                                MaxValue = (int)block.MaxValue,

                                //ModelProjection = model.BlockPrjImg[targetRect].GetWorkRectProjection(),
                                //TargetProjection = model.BlockBufImg[targetRect].GetWorkRectProjection(),
                                //ScoreX = scoresX,
                                //ScoreY = scoresY
                            };

                            lock (list)
                                list.Add(compareHighDiffItem);
                        }
                    }
                    result.HighDiffBlocks = list.ToArray();

                    using (AlgoImage resultRoiImageBin = resultRoiImage.Clone())
                    {
                        ip.Binarize(resultRoiImageBin, resultRoiImageBin, debugThreshold);

                        IEnumerable<HighDiffBlock> orderd = result.HighDiffBlocks.OrderBy(f => f.Index);
                        foreach (HighDiffBlock item in orderd)
                        {
                            int inflate = 0;
                            Rectangle targetRect = item.TargetRectangle;
                            Rectangle modelRect = item.ModelRectangle;

                            Rectangle inflateModelRect = Rectangle.Inflate(modelRect, inflate, inflate);
                            Rectangle intersectModelRect = Rectangle.Intersect(new Rectangle(Point.Empty, modelRoiImage.Size), inflateModelRect);
                            Rectangle inflateTargetRect = Rectangle.Inflate(targetRect, inflate, inflate);
                            Rectangle intersectTargetRect = Rectangle.Intersect(new Rectangle(Point.Empty, targetRoiImage.Size), inflateTargetRect);
                            //if (intersectModelRect.Size != intersectTargetRect.Size)
                            //{
                            //    Size minSize = Size.Empty;
                            //    minSize.Width = Math.Min(intersectModelRect.Width, intersectTargetRect.Width);
                            //    minSize.Height = Math.Min(intersectModelRect.Height, intersectTargetRect.Height);
                            //    intersectModelRect = DrawingHelper.FromCenterSize(DrawingHelper.CenterPoint(intersectModelRect), minSize);
                            //    intersectTargetRect = DrawingHelper.FromCenterSize(DrawingHelper.CenterPoint(intersectTargetRect), minSize);
                            //}

                            using (AlgoImage subImage = targetRoiImage.GetSubImage(intersectTargetRect))
                                item.Target = subImage.ToImageD();
                            using (AlgoImage subImage = resultRoiImage.GetSubImage(intersectTargetRect))
                                item.Result = subImage.ToImageD();
                            using (AlgoImage subImage = resultRoiImageBin.GetSubImage(intersectTargetRect))
                                item.Binalize = subImage.ToImageD();

                            using (AlgoImage subImage = modelRoiImage.GetSubImage(intersectModelRect))
                                item.Model = subImage.ToImageD();
                            using (AlgoImage subImage = weightRoiImage.GetSubImage(intersectModelRect))
                                item.Weight = subImage.ToImageD();
                        }

                        using (AlgoImage resultRoiImageDil = resultRoiImageBin.Clone())
                        {
                            ip.Dilate(resultRoiImageBin, resultRoiImageDil, 1);
                            ip.Subtract(resultRoiImageDil, resultRoiImageBin, resultRoiImageBin);
                        }
                        ip.Add(debugRoiImage, resultRoiImageBin, debugRoiImage);

                    }
                    imageD = debugRoiImage.ToImageD();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return imageD;
        }
    }
}
