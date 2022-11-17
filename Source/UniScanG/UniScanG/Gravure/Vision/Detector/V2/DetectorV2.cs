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
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.Calculator.V2;
using UniScanG.Gravure.Vision.Trainer;
using UniScanG.Vision;

namespace UniScanG.Gravure.Vision.Detector.V2
{
    class DetectorV2 : Detector
    {
        public DetectorV2() : base() { }


        #region Abstract

        public override DynMvp.Vision.Algorithm Clone()
        {
            DetectorV2 clone = new DetectorV2();
            clone.CopyFrom(this);

            return clone;
        }
        #endregion

        #region Override
        public override BlobParam GetBlobParam()
        {
            return new BlobParam()
            {
                MaxCount = 0,
                SelectArea = true,
                TimeoutMs = DetectorParam.TimeoutMs,
                //AreaMin = 2,
                //SelectCenterPt = true,
                SelectGrayMeanValue = true,
                SelectGrayMinValue = true,
                SelectGrayMaxValue = true,

                SelectCompactness = true,
                //SelectFeretDiameter = true,
                SelectBoundingRect = true,
                SelectRotateRect = true,
                EraseBorderBlobs = true,
                Connectivity4 = false
            };
        }
        #endregion


        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            DebugContextG debugContextG = algorithmInspectParam.DebugContext as DebugContextG;
            Calibration calibration = algorithmInspectParam.CameraCalibration;

            DetectorResult detectorResult = (DetectorResult)CreateAlgorithmResult();

            SheetInspectParam inspectParam = algorithmInspectParam as SheetInspectParam;
            CalculatorParam calculatorParam = CalculatorBase.CalculatorParam;
            //DetectorParam detectorParam = Detector.DetectorParam;
            CancellationToken cancellationToken = inspectParam.CancellationToken;

            Inspect.ProcessBufferSetG buffer = inspectParam.ProcessBufferSet as Inspect.ProcessBufferSetG;
            RegionInfoG inspRegionInfo = inspectParam.TargetRegionInfo as RegionInfoG;
            bool testInspect = inspectParam.TestInspect;

            InspectRegion[] inspectRegions = null;
            if (buffer is ProcessBufferSetG2)
                inspectRegions = ((ProcessBufferSetG2)buffer).InspectRegions;

            AlgoImage fullImage = buffer.AlgoImage;
            AlgoImage inspImage = buffer.ScaledImage;
            if (inspImage == null)
                inspImage = fullImage;

            AlgoImage blobImage = buffer.DetectorInsp;
            if (blobImage == null || !blobImage.IsCompatible(TypeName))
                blobImage = buffer.CalculatorResultGray;

            AlgoImage binalImage = buffer.CalculatorResultBinal;
            AlgoImage edgeMapImage = buffer.EdgeMapImage;

            Rectangle imageRect = new Rectangle(Point.Empty, inspImage.Size);

            inspImage.Save("1sheetImage.bmp", debugContextG);
            blobImage.Save("2blobImage.bmp", debugContextG);
            edgeMapImage?.Save("3edgeMapImage.bmp", debugContextG);
            //dynThresholdImage.Save("DetectorDynThresholdImage.bmp", debugContext);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(blobImage);

            int sheetOffsetX = (int)buffer.OffsetStructSet.PatternOffset.OffsetF.X;
            int sheetOffsetY = (int)buffer.OffsetStructSet.PatternOffset.OffsetF.Y;
            Point sheetOffset = new Point(sheetOffsetX, sheetOffsetY);

            float scaleFactor = buffer.ScaleFactor;

            SizeF pelSize = (calibration != null) ? calibration.PelSize : new SizeF(14, 14);
            SizeF scaledPelSize = DrawingHelper.Mul(pelSize, 1.0f / scaleFactor);

            detectorResult.OffsetFound = new SizeF(buffer.OffsetStructSet.PatternOffset.OffsetF);
            detectorResult.SheetSizePx = buffer.PatternSizePx;
            detectorResult.SheetSize = new SizeF(
                buffer.PatternSizePx.Width * pelSize.Width / 1000,
                buffer.PatternSizePx.Height * pelSize.Height / 1000);
            List<FoundedObjInPattern> SheetSubResultList = detectorResult.SheetSubResultList;

            int indexOf = -1, startRegionId = 0, endRegionId = 1;
            if (inspectRegions != null)
            {
                indexOf = Array.FindIndex(inspectRegions, f => f.RegionInfoG == inspRegionInfo);
                startRegionId = Math.Max(0, indexOf);
                endRegionId = indexOf < 0 ? inspectRegions.Length : startRegionId + 1;
            }

            // 1. Threshold (if I>=1, I=MAX, else, I=0)
            binalImage.Save("4binalImage.bmp", debugContextG);
            //binalImage.Save(@"C:\temp\3binalImage.bmp");

            object remainDefectCounterLock = new object();
            int remainDefectCounter = DetectorParam.MaximumDefectCount;
            if (remainDefectCounter < 0)
            //if (remainDefectCounter < 0 || testInspect)
                remainDefectCounter = int.MaxValue;

            ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = DetectorParam.UseMultiThread ? -1 : 1 };
            debugContextG.ProcessTimeLog.SetParallel(ProcessTimeLog.ProcessTimeLogItem.Detection, parallelOptions.MaxDegreeOfParallelism);
            debugContextG.ProcessTimeLog.SetParallel(ProcessTimeLog.ProcessTimeLogItem.Classification, parallelOptions.MaxDegreeOfParallelism);
            
            Parallel.For(startRegionId, endRegionId, parallelOptions, i =>
            //for (int i = startRegionId; i < endRegionId; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                Rectangle subRegion = imageRect;
                InspectRegion inspectRegion = null;
                if (inspectRegions != null)
                {
                    inspectRegion = inspectRegions[i];
                    if (inspectRegion.Use == false)
                        return; //continue;

                    Point patternOffset = sheetOffset;
                    subRegion = DrawingHelper.Offset(inspectRegion.Rectangle, patternOffset);
                    if (subRegion.Size.IsEmpty)
                        return; //continue;

                    Point localOffset = Point.Round(buffer.OffsetStructSet.GetLocalOffset(i));
                    subRegion.Offset(localOffset);
                }

                Rectangle scaledSubRegion = DrawingHelper.Mul(subRegion, 1);
                Rectangle adjustSubRegion = Rectangle.Intersect(imageRect, scaledSubRegion);
                if (scaledSubRegion != adjustSubRegion)
                    return; //continue;

                lock (remainDefectCounterLock)
                {
                    if (remainDefectCounter == 0)
                        return;
                }

                AlgoImage subInspImage = inspImage.GetSubImage(scaledSubRegion);
                AlgoImage subBlobImage = blobImage.GetSubImage(scaledSubRegion);
                AlgoImage subBinalImage = binalImage.GetSubImage(scaledSubRegion);
                AlgoImage subegMapImage = edgeMapImage?.GetSubImage(scaledSubRegion);
                List<BlobRect> blobRectList = null;
                List<DefectObj> sheetSubResultList = new List<DefectObj>();
                try
                {
                    DebugContextG regionDebugContextG = debugContextG.Clone();
                    regionDebugContextG.RegionId = i;
                    Stopwatch sw = new Stopwatch();

                    // 2. Blob
                    sw.Start();
                    blobRectList = BlobProcess(subBlobImage, subBinalImage, regionDebugContextG);
                    debugContextG.ProcessTimeLog.Add(ProcessTimeLog.ProcessTimeLogItem.Detection, sw.ElapsedMilliseconds);
                    if (blobRectList == null)
                        return; //continue;

                    sw.Restart();
                    for (int j = 0; j < blobRectList.Count; j++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        long timeoutMs = DetectorParam.TimeoutMs;
                        if (timeoutMs < 0)
                            timeoutMs = long.MaxValue;
                        if (sw.ElapsedMilliseconds > timeoutMs)
                            break;

                        BlobRect blobRect = blobRectList[j];
                        //if (inspectRegion.RegionInfoG.PassRectList.Count > 0)
                        //// Pass가 설정되면, 설정된 Pass 내부만 통과한다.
                        //{
                        //    Rectangle[] adjectPassRects = inspectRegion.RegionInfoG.PassRectList.Select(f => DrawingHelper.Offset(f, patternOffset)).ToArray();
                        //    if (!Array.Exists(adjectPassRects, f => blobRect.BoundingRect.IntersectsWith(f)))
                        //        continue;
                        //}

                        DebugContextG blobDebugContextG = regionDebugContextG.Clone();
                        blobDebugContextG.BlobId = j;


                        DefectObj subResult = CalculateBlob(buffer, subInspImage, subBlobImage, subBinalImage, subegMapImage, scaledPelSize, inspectRegion, blobRect, blobDebugContextG);
                        if (subResult != null && subResult.IsValid && IsValidSize(subResult))
                        {
                            lock (remainDefectCounterLock)
                            {
                                if (remainDefectCounter == 0)
                                    break;

                                sheetSubResultList.Add(subResult);
                                remainDefectCounter--;
                            }
                        }
                    }

                    if (DetectorParam.MergingDefects)
                        MergeDefects(sheetSubResultList);

                    // 전체 패턴 좌표로 이동하고 이미지 크롭.
                    Common.IClientExchangeOperator client = (Common.IClientExchangeOperator)SystemManager.Instance()?.ExchangeOperator;
                    int camIndex = client == null ? 0 : client.GetCamIndex();
                    for (int j = 0; j < sheetSubResultList.Count; j++)
                    {
                        DefectObj defectObj = sheetSubResultList[j];
                        defectObj.Index = detectorResult.SheetSubResultList.Count + j;
                        defectObj.CamIndex = camIndex;

                        // 불량 이미지 크롭
                        UpdateBinalImage(subBinalImage, scaleFactor, defectObj);

                        defectObj.Offset(subRegion.Location, scaledPelSize);
                        defectObj.Rescale(1.0f / scaleFactor);

                        // 실제 이미지 크롭
                        UpdateDefectImage(fullImage, defectObj);
                    }
                    debugContextG.ProcessTimeLog.Add(ProcessTimeLog.ProcessTimeLogItem.Classification, sw.ElapsedMilliseconds);
                    sw.Stop();
                }
                catch (OperationCanceledException ex) { }
#if DEBUG == false
                catch (Exception ex)
                {
                    int innerCount = 0;
                    while (ex != null)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(string.Format("Error Occure in Detector::Inspect - {0}:{1}", innerCount++, ex.Message));
                        sb.AppendLine(ex.StackTrace);
                        LogHelper.Error(LoggerType.Inspection, sb.ToString());
                        ex = ex.InnerException;
                    }
                }
#endif
                finally
                {
                    subInspImage.Dispose();
                    subBlobImage.Dispose();
                    subBinalImage.Dispose();
                    subegMapImage?.Dispose();

                    //int foundCount = sheetSubResultList.Count;
                    //int curCount = detectorResult.SheetSubResultList.Count;
                    //int remainCount = detectorParam.MaximumDefectCount < 0 ? foundCount : detectorParam.MaximumDefectCount - curCount;
                    //if (testInspect)
                    //    remainCount = int.MaxValue;

                    //detectorResult.SheetSubResultList.AddRange(sheetSubResultList.GetRange(0, Math.Min(remainCount, foundCount)));
                    lock (detectorResult.SheetSubResultList)
                        detectorResult.SheetSubResultList.AddRange(sheetSubResultList);
                }
            }
            );

            int detectCount = detectorResult.SheetSubResultList.Count;
            if (detectCount == 0)
            {
                detectorResult.Message = "No Defect";
            }
            else if (detectCount >= DetectorParam.MaximumDefectCount)
            {
                detectorResult.Message = "Too Many Defects";
            }
            else
            {
                detectorResult.Message = string.Format("{0} Defects", detectCount);
            }

            detectorResult.Good = (detectCount == 0);
            detectorResult.UpdateSpandTime();
            return detectorResult;
        }

         private DefectObj CalculateBlob(Inspect.ProcessBufferSetG buffer, AlgoImage subInspImage, AlgoImage subBlobImage, AlgoImage subBinalImage, AlgoImage subegMapImage, SizeF pelSize, InspectRegion inspectRegion, BlobRect blobRect, DebugContextG debugContextG)
        {
            TrainerParam trainerParam = TrainerBase.TrainerParam;
            int binValue = trainerParam.ModelParam.BinValue + trainerParam.ModelParam.BinValueOffset;
            //float poleAvg = inspectRegion.RegionInfoG.PoleAvg;
            //float dielectricAvg = inspectRegion.RegionInfoG.DielectricAvg;

            DefectObj subResult = new DefectObj();

            // 불량 종류 판단
            bool ok;
            if (inspectRegion == null)
                ok = CalcBlob.CalcDefectType(buffer, subInspImage, subBlobImage, subBinalImage, blobRect, subResult, debugContextG);
            else
                ok = CalcDefectType(subInspImage, subBlobImage, subBinalImage, blobRect, inspectRegion, subResult, debugContextG);

            if (!ok)
                return null;

            // 불량 크기 판단
            CalcDefectSize(subBlobImage, blobRect, binValue, pelSize, subResult, debugContextG);

            if (!IsValidSize(subResult))
                return null;

            // 핀홀인 경우 번짐여부 판단
            DetectorParam detectorParam = Detector.DetectorParam;
            if (detectorParam.UseSpreadTrace && subResult.GetDefectType().HasFlag(DefectType.PinHole))
                this.spreadTracer?.Inspect(subInspImage, subBinalImage, pelSize, subResult, debugContextG.Clone());

            return subResult;

        }

        private bool CalcDefectType(AlgoImage subInspImage, AlgoImage subBlobImage, AlgoImage subBinalImage, BlobRect defectBlob, InspectRegion inspectRegion, DefectObj defectObj, DebugContextG debugContextG)
        {
            if (true)
            {
                // Position
                bool ok = UpdatePositionTypeV2(defectObj, defectBlob, subInspImage, subBinalImage, inspectRegion, debugContextG);
                if (!ok)
                    return false;

                // Value
                UpdateValueType(defectObj, defectBlob, inspectRegion.RegionInfoG, subInspImage, subBlobImage, subBinalImage);
            }
            else
            {
                // Position & Value
                UpdatePositionAndValueType(defectObj, defectBlob, subInspImage, subBinalImage, inspectRegion, debugContextG);
            }

            // Shape
            UpdateShapeType(defectObj, defectBlob);
            return true;

            string path = Path.Combine(debugContextG.FullPath, "CalcDefectType.bmp");
        }

        private bool UpdatePositionTypeV2(DefectObj defectObj, BlobRect defectBlob, AlgoImage subInspImage, AlgoImage subBinalImage, InspectRegion inspectRegion, DebugContextG debugContextG)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(subInspImage);
            Graphics g = null;
            //if (debugBitmap != null)
            //    g = Graphics.FromImage(debugBitmap);

            Rectangle rectangle = Rectangle.Round(defectBlob.BoundingRect);
            //Rectangle imageRect = new Rectangle(Point.Empty, subInspImage.Size);
            //if(Rectangle.Intersect(imageRect, rectangle) != rectangle)
            //{
            //    LogHelper.Error(LoggerType.Inspection, string.Format("DetectorV2::UpdatePositionTypeV2 - Out of image size. ImageRect: {0}, DefectRect: {1}",
            //        imageRect, rectangle));
            //    return false;
            //}

            g?.DrawRectangle(new Pen(Color.Yellow, 2), rectangle);

            Point location = rectangle.Location;
            InspectLine inspectLine = Array.Find(inspectRegion.AllinspectLines, f => DrawingHelper.IntersectsWith(f.Rectangle, location));
            if (inspectLine == null)
                return false;

            Point locInElement = Point.Subtract(location, new Size(inspectLine.Rectangle.Location));
            g?.DrawRectangle(new Pen(Color.Yellow, 2), inspectLine.Rectangle);

            InspectLine nextInspectLine = inspectLine.NextInspectLine;
            g?.DrawRectangle(new Pen(Color.Red, 2), nextInspectLine.Rectangle);

            Point locInNElement = Point.Add(locInElement, new Size(nextInspectLine.Rectangle.Location));
            Rectangle rectInNElement = new Rectangle(locInNElement, rectangle.Size);
            g?.DrawRectangle(new Pen(Color.Red, 2), rectInNElement);
            if (!subInspImage.IsInnerRect(rectInNElement))
                return false;

            InspectLine prevInspectLine = inspectLine.PrevInspectLine;
            g?.DrawRectangle(new Pen(Color.Blue, 2), prevInspectLine.Rectangle);

            Point locInPElement = Point.Add(locInElement, new Size(prevInspectLine.Rectangle.Location));
            Rectangle rectInPElement = new Rectangle(locInPElement, rectangle.Size);
            g?.DrawRectangle(new Pen(Color.Blue, 2), rectInPElement);
            if (!subInspImage.IsInnerRect(rectInPElement))
                return false;

            g?.Dispose();

            if (debugContextG.SaveDebugImage)
            {
                using (AlgoImage orgImage = subBinalImage.GetSubImage(Rectangle.Inflate(rectangle, 128, 128)))
                    orgImage.Save(@"maskBlob.bmp");

                using (AlgoImage orgImage = subInspImage.GetSubImage(Rectangle.Inflate(rectangle, 128, 128)))
                    orgImage.Save(@"baseBlob.bmp");

                using (AlgoImage orgImage = subInspImage.GetSubImage(Rectangle.Inflate(rectInNElement, 128, 128)))
                    orgImage.Save(@"nextBlob.bmp");

                using (AlgoImage orgImage = subInspImage.GetSubImage(Rectangle.Inflate(rectInPElement, 128, 128)))
                    orgImage.Save(@"prevBlob.bmp");
            }

            StatResult statResultN, statResultP;
            using (AlgoImage maskImage = subBinalImage.GetSubImage(rectangle))
            {
                using (AlgoImage nextImage = subInspImage.GetSubImage(rectInNElement))
                    statResultN = ip.GetStatValue(nextImage, maskImage);
                using (AlgoImage prevImage = subInspImage.GetSubImage(rectInPElement))
                    statResultP = ip.GetStatValue(prevImage, maskImage);
            }

            double average = (statResultN.average + statResultP.average) / 2;
            double posTh = (inspectRegion.RegionInfoG.DielectricAvg + inspectRegion.RegionInfoG.PoleAvg) / 2;
            double edgeW = (inspectRegion.RegionInfoG.DielectricAvg - inspectRegion.RegionInfoG.PoleAvg) / 2.2;

            DefectObj.EPositionType positionType = average < posTh ? DefectObj.EPositionType.Pole : DefectObj.EPositionType.Dielectric;
            if (MathHelper.IsInRange(average, posTh - edgeW, posTh + edgeW))
                positionType |= DefectObj.EPositionType.Edge;

            float ppValue = defectBlob.MaxValue - defectBlob.MinValue;
            float pmValue = defectBlob.MaxValue - defectBlob.MeanValue;
            float mpValue = defectBlob.MeanValue - defectBlob.MinValue;
            double valueDiff = ppValue;

            defectObj.PositionType = positionType;
            //defectObj.ValueDiff = (float)(defectBlob.MinValue*1000000 + defectBlob.MaxValue*1000 + defectBlob.MeanValue);
            defectObj.ValueDiff = (float)valueDiff;

            return true;
        }

        private void UpdatePositionAndValueType(DefectObj defectObj, BlobRect defectBlob, AlgoImage subInspImage, AlgoImage subBinalImage, InspectRegion inspectRegion, DebugContextG debugContextG)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(subInspImage);
            Graphics g = null;
            //if (debugBitmap != null)
            //    g = Graphics.FromImage(debugBitmap);

            Rectangle rectangle = Rectangle.Round(defectBlob.BoundingRect);
            g?.DrawRectangle(new Pen(Color.Yellow, 2), rectangle);

            Point location = rectangle.Location;
            InspectLine inspectLine = Array.Find(inspectRegion.AllinspectLines, f => DrawingHelper.IntersectsWith(f.Rectangle, location));
            Point locInElement = Point.Subtract(location, new Size(inspectLine.Rectangle.Location));
            g?.DrawRectangle(new Pen(Color.Yellow, 2), inspectLine.Rectangle);

            InspectLine nextInspectLine = inspectLine.NextInspectLine;
            g?.DrawRectangle(new Pen(Color.Red, 2), nextInspectLine.Rectangle);
            Point locInNElement = Point.Add(locInElement, new Size(nextInspectLine.Rectangle.Location));

            Rectangle rectInNElement = new Rectangle(locInNElement, rectangle.Size);
            g?.DrawRectangle(new Pen(Color.Red, 2), rectInNElement);

            InspectLine prevInspectLine = inspectLine.PrevInspectLine;
            g?.DrawRectangle(new Pen(Color.Blue, 2), prevInspectLine.Rectangle);
            Point locInPElement = Point.Add(locInElement, new Size(prevInspectLine.Rectangle.Location));

            Rectangle rectInPElement = new Rectangle(locInPElement, rectangle.Size);
            g?.DrawRectangle(new Pen(Color.Blue, 2), rectInPElement);

            g?.Dispose();

            if (debugContextG.SaveDebugImage)
            {
                using (AlgoImage orgImage = subBinalImage.GetSubImage(Rectangle.Inflate(rectangle, 128, 128)))
                    orgImage.Save(@"maskBlob.bmp");

                using (AlgoImage orgImage = subInspImage.GetSubImage(Rectangle.Inflate(rectangle, 128, 128)))
                    orgImage.Save(@"baseBlob.bmp");

                using (AlgoImage orgImage = subInspImage.GetSubImage(Rectangle.Inflate(rectInNElement, 128, 128)))
                    orgImage.Save(@"nextBlob.bmp");

                using (AlgoImage orgImage = subInspImage.GetSubImage(Rectangle.Inflate(rectInPElement, 128, 128)))
                    orgImage.Save(@"prevBlob.bmp");
            }

            StatResult statResultN, statResultP;
            using (AlgoImage maskImage = subBinalImage.GetSubImage(rectangle))
            {
                using (AlgoImage nextImage = subInspImage.GetSubImage(rectInNElement))
                    statResultN = ip.GetStatValue(nextImage, maskImage);
                using (AlgoImage prevImage = subInspImage.GetSubImage(rectInPElement))
                    statResultP = ip.GetStatValue(prevImage, maskImage);
            }

            double average = (statResultN.average + statResultP.average) / 2;
            double posTh = (inspectRegion.RegionInfoG.DielectricAvg + inspectRegion.RegionInfoG.PoleAvg) / 2;
            DefectObj.EPositionType positionType = average < posTh ? DefectObj.EPositionType.Pole : DefectObj.EPositionType.Dielectric;

            double myAverage = defectBlob.MeanValue;

            DefectObj.EValueType valueType;
            if (myAverage < average - 5)
                valueType = DefectObj.EValueType.Dark;
            else if (myAverage > average + 5)
                valueType = DefectObj.EValueType.Bright;
            else
                valueType = DefectObj.EValueType.None;

            defectObj.PositionType = positionType;
            defectObj.ValueType = valueType;

            defectObj.ValueDiff = (int)(myAverage - average);
        }

        private void MergeDefects(List<DefectObj> sheetSubResultList)
        {
            // 미인쇄 안에 핀홀/번짐이 있으면 시트어택으로 변경.
            List<DefectObj> nopList = sheetSubResultList.FindAll(f => f.GetDefectType() == DefectType.Noprint);

            nopList.ForEach(f =>
            {
                List<DefectObj> phList = sheetSubResultList.FindAll(g =>
                {
                    DefectType defectType = f.GetDefectType();
                    bool isPh = defectType == DefectType.PinHole || defectType == DefectType.Spread;
                    bool isIntersected = f.Region.IntersectsWith(g.Region);
                    return isPh && isIntersected;
                });

                if (phList.Count > 0)
                {
                    //sheetSubResultList.RemoveAll(g => phList.Contains(g));
                    f.ShapeType = DefectObj.EShapeType.Complex;
                }
            });
        }
    }
}
