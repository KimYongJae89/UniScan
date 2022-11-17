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
//using UniEye.Base.Settings;
using DynMvp.Data;
using System.Threading;
using System.Runtime.InteropServices;
using UniEye.Base;
using System.IO;
using UniScanG.Vision;
using UniScanG.Gravure.Inspect;
using UniScanG.Data;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.Vision.Trainer;

namespace UniScanG.Gravure.Vision.Detector
{
    public class Detector : AlgorithmG
    {
        public static string TypeName { get { return "Detector"; } }

        public static DetectorParam DetectorParam => AlgorithmSetting.Instance().DetectorParam;

        protected SpreadTracer spreadTracer;

        public Detector()
        {
            this.AlgorithmName = TypeName;
            this.param = null;
        }

        #region Abstract
        public override AlgorithmParam CreateParam()
        {
            return new DetectorParam(true);
        }

        public override DynMvp.Vision.Algorithm Clone()
        {
            Detector clone = new Detector();
            clone.CopyFrom(this);

            return clone;
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }
        #endregion

        #region Override
        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new DetectorResult();
        }

        public override void PrepareInspection()
        {
            base.PrepareInspection();

            DetectorParam detectorParam = Detector.DetectorParam;
            this.spreadTracer = SpreadTracer.Create();
        }
        #endregion

        public virtual BlobParam GetBlobParam()
        {
            return new BlobParam()
            {
                MaxCount = 0,
                SelectArea = true,
                AreaMin = 2,
                SelectCenterPt = true,
                SelectGrayMeanValue = true,
                SelectGrayMinValue = true,
                SelectGrayMaxValue = true,
                SelectCompactness = true,
                SelectFeretDiameter = true,
                SelectBoundingRect = true,
                EraseBorderBlobs = true,
                Connectivity4 = false,
            };
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            DebugContextG debugContextG = algorithmInspectParam.DebugContext as DebugContextG;
            Calibration calibration = algorithmInspectParam.CameraCalibration;

            DetectorResult detectorResult = (DetectorResult)CreateAlgorithmResult();

            SheetInspectParam sheetInspectParam = algorithmInspectParam as SheetInspectParam;
            UniScanG.Data.Model.Model model = sheetInspectParam.Model;
            DetectorParam detectorParam = Detector.DetectorParam;
            CancellationToken cancellationToken = sheetInspectParam.CancellationToken;

            ProcessBufferSetG buffer = sheetInspectParam.ProcessBufferSet as ProcessBufferSetG;
            RegionInfoG inspRegionInfo = sheetInspectParam.TargetRegionInfo as RegionInfoG;
            bool testInspect = sheetInspectParam.TestInspect;

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

            //int patternWidth = fullImage.Width;// - calculatorParam.BasePosition.X + sheetOffsetX;
            //int patternHeight = fullImage.Height;
            float scaleFactor = buffer.ScaleFactor;

            SizeF pelSize = (calibration != null) ? calibration.PelSize : new SizeF(14, 14);
            SizeF scaledPelSize = DrawingHelper.Mul(pelSize, 1.0f / scaleFactor);

            detectorResult.OffsetFound = new SizeF(buffer.OffsetStructSet.PatternOffset.OffsetF);
            detectorResult.SheetSizePx = buffer.PatternSizePx;
            detectorResult.SheetSize = DrawingHelper.Div(DrawingHelper.Mul(buffer.PatternSizePx, pelSize), 1000);

            int indexOf = model.CalculatorModelParam.RegionInfoCollection.IndexOf(inspRegionInfo);
            int startRegionId = Math.Max(0, indexOf);
            int endRegionId = indexOf < 0 ? model.CalculatorModelParam.RegionInfoCollection.Count : startRegionId + 1;

            // 1. Threshold (if I>=1, I=MAX, else, I=0)
            binalImage.Save("4binalImage.bmp", debugContextG);
            //binalImage.Save(@"C:\temp\3binalImage.bmp");

            for (int i = startRegionId; i < endRegionId; i++)
            {
                RegionInfoG regionInfoG = model.CalculatorModelParam.RegionInfoCollection[i];
                if (regionInfoG.Use == false)
                    continue;

                Point patternOffset = sheetOffset;
                Rectangle subRegion = DrawingHelper.Offset(regionInfoG.Region, patternOffset);
                if (subRegion.Size.IsEmpty)
                    continue;

                Rectangle scaledSubRegion = DrawingHelper.Mul(subRegion, scaleFactor);

                PointF localOffsetF = buffer.OffsetStructSet.GetLocalOffset(i);
                Point localOffset = Point.Round(localOffsetF);
                scaledSubRegion.Offset(localOffset);
                subRegion.Offset(localOffset);

                Rectangle adjustSubRegion = Rectangle.Intersect(imageRect, scaledSubRegion);
                if (scaledSubRegion != adjustSubRegion)
                    continue;

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
                    //subSheetImage.Save(@"C:\temp\subSheetImage.bmp");
                    //subBinalImage.Save(@"C:\temp\subBinalImage.bmp");
                    sw.Start();
                    blobRectList = BlobProcess(subInspImage, subBinalImage, regionDebugContextG);
                    debugContextG.ProcessTimeLog.Add(ProcessTimeLog.ProcessTimeLogItem.Detection, sw.ElapsedMilliseconds);
                    if (blobRectList == null)
                        continue;

                    sw.Restart();
                    for (int j = 0; j < blobRectList.Count; j++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        BlobRect blobRect = blobRectList[j];
                        if (regionInfoG.PassRectList.Count > 0)
                        // Pass가 설정되면, 설정된 Pass만 통과한다.
                        {
                            if (!regionInfoG.PassRectList.Exists(f => blobRect.BoundingRect.IntersectsWith(f)))
                                continue;
                        }

                        DebugContextG blobDebugContextG = regionDebugContextG.Clone();
                        blobDebugContextG.BlobId = j;

                        Gravure.Data.DefectObj subResult = CalculateBlob(subInspImage, subBlobImage, subBinalImage, subegMapImage,
                            scaledPelSize, regionInfoG, blobRect, blobDebugContextG);
                        if (subResult != null && subResult.IsValid && IsValidSize(subResult))
                            sheetSubResultList.Add(subResult);
                    }

                    //MergeSubResults(sheetSubResultList);
                    //sheetSubResultList.RemoveAll(f =>
                    //{
                    //    float size;
                    //    switch (detectorParam.CriterionLength)
                    //    {
                    //        case DetectorParam.ECriterionLength.Min:
                    //            size = Math.Min(f.RealRegion.Width, f.RealRegion.Height);
                    //            break;
                    //        case DetectorParam.ECriterionLength.Max:
                    //            size = Math.Max(f.RealRegion.Width, f.RealRegion.Height);
                    //            break;
                    //        case DetectorParam.ECriterionLength.Diagonal:
                    //        default:
                    //            size = f.RealLength;
                    //            break;
                    //    }

                    //    float minSize;
                    //    switch (f.PositionType)
                    //    {
                    //        case DefectObj.EPositionType.Pole:
                    //            minSize = AlgorithmSetting.Instance().MinBlackDefectLength;
                    //            break;
                    //        case DefectObj.EPositionType.Dielectric:
                    //            minSize = AlgorithmSetting.Instance().MinWhiteDefectLength;
                    //            break;
                    //        default:
                    //            minSize = Math.Max(AlgorithmSetting.Instance().MinBlackDefectLength, AlgorithmSetting.Instance().MinWhiteDefectLength);
                    //            break;
                    //    }
                    //    return (size < minSize);
                    //});

                    // 전체 패턴 좌표로 이동하고 이미지 크롭.
                    Common.IClientExchangeOperator client = (Common.IClientExchangeOperator)SystemManager.Instance()?.ExchangeOperator;
                    int camIndex = client == null ? 0 : client.GetCamIndex();
                    for (int j = 0; j < sheetSubResultList.Count; j++)
                    {
                        DefectObj defectObj = sheetSubResultList[j];
                        defectObj.Index = j;
                        defectObj.CamIndex = camIndex;

                        // 불량 이미지 크롭
                        UpdateBinalImage(subBinalImage, scaleFactor, defectObj);

                        defectObj.Rescale(1.0f / scaleFactor);
                        defectObj.Offset(subRegion.Location, scaledPelSize);

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

                    int foundCount = sheetSubResultList.Count;
                    int curCount = detectorResult.SheetSubResultList.Count;
                    int remainCount = detectorParam.MaximumDefectCount < 0 ? foundCount : detectorParam.MaximumDefectCount - curCount;
                    if (testInspect)
                        remainCount = int.MaxValue;

                    detectorResult.SheetSubResultList.AddRange(sheetSubResultList.GetRange(0, Math.Min(remainCount, foundCount)));
                }

                if (testInspect == false && detectorParam.MaximumDefectCount > 0 && detectorResult.SheetSubResultList.Count >= detectorParam.MaximumDefectCount)
                    break;
            }

            int detectCount = detectorResult.SheetSubResultList.Count;
            if (detectCount == 0)
            {
                detectorResult.Message = "No Defect";
            }
            else if (detectCount >= detectorParam.MaximumDefectCount)
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

        protected bool IsValidSize(DefectObj subResult)
        {
            DetectorParam detectorParam = Detector.DetectorParam;

            bool forceMax = false;
            float minSize;
            if (subResult.PositionType.HasFlag(DefectObj.EPositionType.Pole))
            {
                minSize = detectorParam.MinBlackDefectLength;
                if (subResult.PositionType.HasFlag(DefectObj.EPositionType.Edge) && subResult.ValueType == DefectObj.EValueType.Bright)
                {
                    forceMax = true;
                    minSize = Math.Max(minSize * 3, 11);
                }
            }
            else if (subResult.PositionType.HasFlag(DefectObj.EPositionType.Dielectric))
            {
                minSize = detectorParam.MinWhiteDefectLength;
            }
            else
            {
                minSize = Math.Max(detectorParam.MinBlackDefectLength, detectorParam.MinWhiteDefectLength);
            }

            float realSize;
            DetectorParam.ECriterionLength criterionLength = detectorParam.CriterionLength;
            if (forceMax)
                criterionLength = DetectorParam.ECriterionLength.Max;

            switch (detectorParam.CriterionLength)
            {
                case DetectorParam.ECriterionLength.Min:
                    realSize = Math.Min(subResult.RealRegion.Width, subResult.RealRegion.Height);
                    break;
                case DetectorParam.ECriterionLength.Max:
                    realSize = Math.Max(subResult.RealRegion.Width, subResult.RealRegion.Height);
                    break;
                case DetectorParam.ECriterionLength.Diagonal:
                default:
                    realSize = subResult.RealLength;
                    break;
            }

            return (realSize >= minSize);
        }

        private bool MergeSubResults(List<Gravure.Data.DefectObj> sheetSubResultList)
        {
            bool merged = false;
            List<Gravure.Data.DefectObj> mergedList = new List<Gravure.Data.DefectObj>();

            while (sheetSubResultList.Count > 0)
            {
                Gravure.Data.DefectObj sheetSubResult = sheetSubResultList.FirstOrDefault();
                sheetSubResultList.Remove(sheetSubResult);

                RectangleF searchRect = RectangleF.Inflate(sheetSubResult.Region, sheetSubResult.Region.Width * 2, sheetSubResult.Region.Height * 2);
                DefectType searchType = DefectType.Unknown;
                switch (sheetSubResult.GetDefectType())
                {
                    // 시트어택 조건: 핀홀+성형
                    case DefectType.PinHole:
                        searchType = DefectType.Coating;
                        break;
                    // 시트어택 조건: 성형+핀홀
                    case DefectType.Coating:
                        searchType = DefectType.PinHole;
                        break;
                    // 시트어택 조건: 미인쇄+시트어택
                    case DefectType.Noprint:
                        searchType = DefectType.Attack;
                        break;
                    // 시트어택 조건: 시트어택+미인쇄
                    case DefectType.Attack:
                        searchType = DefectType.Noprint;
                        break;
                }

                List<Gravure.Data.DefectObj> foundedSheetSubResult = sheetSubResultList.FindAll(f => f.Region.IntersectsWith(searchRect) && f.GetDefectType() == searchType);
                if (foundedSheetSubResult.Count > 0)
                {
                    merged = true;
                    foundedSheetSubResult.ForEach(f => sheetSubResultList.Remove(f));
                    foundedSheetSubResult.Insert(0, sheetSubResult);
                    sheetSubResult = MergeSubResults2(foundedSheetSubResult);
                }

                mergedList.Add(sheetSubResult);
            }

            sheetSubResultList.AddRange(mergedList);
            if (merged)
                return MergeSubResults(sheetSubResultList);

            return false;
        }

        private Data.DefectObj MergeSubResults2(List<Data.DefectObj> temp)
        {
            Gravure.Data.DefectObj subResult = new Gravure.Data.DefectObj();
            subResult.ShapeType = DefectObj.EShapeType.Complex;

            temp.Aggregate((f, g) =>
            {
                subResult.Region = RectangleF.Union(f.Region, g.Region);
                subResult.RealRegion = RectangleF.Union(f.RealRegion, g.RealRegion);
                subResult.SubtractValueMax = Math.Max(f.SubtractValueMax, g.SubtractValueMax);
                //subResult.SubtractValueMin = Math.Min(f.SubtractValueMin, g.SubtractValueMin);
                return subResult;
            });

            //Rectangle fullClipRect = Rectangle.Intersect(Rectangle.Inflate(subResult.Region, 50, 50), new Rectangle(Point.Empty, fullImage.Size));
            //AlgoImage bullClipImage = fullImage.Clip(fullClipRect);
            //subResult.Image = bullClipImage.ToBitmap();
            //bullClipImage.Dispose();

            //Rectangle binClipRect = Rectangle.Intersect(DrawingHelper.Mul(fullClipRect, scale), new Rectangle(Point.Empty, binImage.Size));
            //AlgoImage binClipImage = binImage.Clip(binClipRect);
            //subResult.BufImage = binClipImage.ToBitmap();
            //binClipImage.Dispose();

            return subResult;
        }


        protected virtual void CalcDefectType(AlgoImage subInspImage, AlgoImage subBlobImage, AlgoImage subBinalImage, BlobRect defectBlob, int binalValue, RegionInfoG regionInfoG, DefectObj defectObj, DebugContextG debugContextG)
        {

            //float defectAroundImageValue = GetDefectImageValue(defectBlob, algoImage, new Size(5, 5));
            //sheetSubResult.PositionType = (defectAroundImageValue < binalValue * 0.8 ? PositionType.Pole : PositionType.Dielectric);

            // Position
            UpdatePositionType(defectObj, defectBlob, subInspImage, new Size(5, 5), binalValue);

            // Value
            UpdateValueType(defectObj, defectBlob, regionInfoG, subInspImage, subBlobImage, subBinalImage);

            // Shape
            UpdateShapeType(defectObj, defectBlob);
        }

        protected void UpdatePositionType(DefectObj defectObj, BlobRect defectBlob, AlgoImage subInspImage, Size inflate, int binValue)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(subInspImage);

            Rectangle blobRect = Rectangle.Round(defectBlob.BoundingRect);
            Rectangle fullRect = Rectangle.Inflate(blobRect, inflate.Width, inflate.Height);
            fullRect.Intersect(new Rectangle(Point.Empty, subInspImage.Size));

            //using (AlgoImage aa = algoImage.GetSubImage(fullRect))
            //    aa.Save(@"C:\temp\algoImage.bmp");

            int[] xs = new int[] { blobRect.Left - inflate.Width, blobRect.Left, blobRect.Right, blobRect.Right + inflate.Width };
            int[] ys = new int[] { blobRect.Top - inflate.Height, blobRect.Top, blobRect.Bottom, blobRect.Bottom + inflate.Height };

            Rectangle[] subRects = new Rectangle[]
            {
                Rectangle.FromLTRB(xs[0],ys[0],xs[1],ys[1]), // 0:L-T
                Rectangle.FromLTRB(xs[1],ys[0],xs[2],ys[1]), // 1:C-T
                Rectangle.FromLTRB(xs[2],ys[0],xs[3],ys[1]), // 2:R-T

                Rectangle.FromLTRB(xs[0],ys[1],xs[1],ys[2]), // 3:L-M
                Rectangle.FromLTRB(xs[1],ys[1],xs[2],ys[2]), // 4:C-M
                Rectangle.FromLTRB(xs[2],ys[1],xs[3],ys[2]), // 5:R-M

                Rectangle.FromLTRB(xs[0],ys[2],xs[1],ys[3]), // 6:L-B
                Rectangle.FromLTRB(xs[1],ys[2],xs[2],ys[3]), // 7:C-B
                Rectangle.FromLTRB(xs[2],ys[2],xs[3],ys[3]), // 8:R-B
            };

            Size subRectSize = inflate;
            float[] darkMap = new float[subRects.Length];
            for (int i = 0; i < subRects.Length; i++)
            {
                Rectangle subRect = Rectangle.Intersect(subRects[i], fullRect);
                if (subRect.Width > 0 && subRect.Height > 0)
                {
                    using (AlgoImage subAlgoImage = subInspImage.GetSubImage(subRect))
                        darkMap[i] = imageProcessing.GetGreyAverage(subAlgoImage);
                }
            }


            float[][] isInPole = new float[][]
            {
                new float[]{darkMap[0] , darkMap[1] , darkMap[3] }, //leftTop
                new float[]{darkMap[1] , darkMap[2] , darkMap[5] }, //rightTop
                new float[]{darkMap[3] , darkMap[6] , darkMap[7] }, //leftBottom
                new float[]{darkMap[5] , darkMap[7] , darkMap[8] }, //rightBottom;

                //new float[]{mapDark[0, 1] , mapDark[2, 1] },//vertical
                //new float[]{mapDark[1, 0] , mapDark[1, 2] },//horizen
                //new float[]{mapDark[2, 0] , mapDark[0, 2] },//upperDiagonal
                //new float[]{mapDark[0, 0] , mapDark[2, 2] }//lowerDiagonal
            };

            if (isInPole.Any(f => Array.TrueForAll(f, g => g < binValue)))
                //if (isInPole.Any(f => f.Average() < binValue))
                defectObj.PositionType = DefectObj.EPositionType.Pole;
            defectObj.PositionType = DefectObj.EPositionType.Dielectric;
        }

        protected void UpdateValueType(DefectObj defectObj, BlobRect defectBlob, RegionInfoG regionInfoG, AlgoImage subInspImage, AlgoImage subBlobImage, AlgoImage subBinalImage)
        {
            DetectorParam detectorParam = Detector.DetectorParam;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(subInspImage);
            DefectObj.EValueType valueType = DefectObj.EValueType.None;
            Rectangle rect = Rectangle.Round(defectBlob.BoundingRect);
            //rect.Inflate(10, 10);

            AlgoImage a = subInspImage.GetSubImage(rect);
            AlgoImage b = subBlobImage.GetSubImage(rect);
            AlgoImage c = subBinalImage.GetSubImage(rect);

            StatResult statResultA = imageProcessing.GetStatValue(a, c);
            StatResult statResultB = imageProcessing.GetStatValue(b, c);

            float ppValue = defectBlob.MaxValue - defectBlob.MinValue;
            float pmValue = defectBlob.MaxValue - defectBlob.MeanValue;
            float mpValue = defectBlob.MeanValue - defectBlob.MinValue;
            double valueDiff = statResultA.max - statResultA.min;
            //float valueDiff = (int)Math.Max(statResultA.max - statResultA.average, statResultA.average - statResultA.min);

            if (defectObj.PositionType.HasFlag(DefectObj.EPositionType.Pole))
            {
                //float valueDiff = defectBlob.MaxValue - defectBlob.MinValue;
                if (defectBlob.MeanValue <= regionInfoG.PoleAvg)// * 1.3 /*|| valueDiff > 40*/)
                    valueType = DefectObj.EValueType.Dark;
                else
                    valueType = DefectObj.EValueType.Bright;
            }
            else if (defectObj.PositionType.HasFlag(DefectObj.EPositionType.Dielectric))
            {
                if (detectorParam.ModelParam.AttackDiffUse &&
                    defectBlob.MinValue >= detectorParam.ModelParam.AttackMinValue && valueDiff >= detectorParam.ModelParam.AttackDiffValue)
                {
                    //a.Save(@"C:\temp\a.bmp");
                    //b.Save(@"C:\temp\b.bmp");
                    //c.Save(@"C:\temp\c.bmp");
                    valueType = DefectObj.EValueType.None;
                }
                else if (defectBlob.MeanValue < regionInfoG.DielectricAvg * 1.2)
                    valueType = DefectObj.EValueType.Dark;
                else
                    valueType = DefectObj.EValueType.Bright;
            }

            defectObj.ValueType = valueType;
            defectObj.FillRate = defectBlob.Area * 1.0f / (defectBlob.BoundingRect.Width * defectBlob.BoundingRect.Height) * 100;
            defectObj.SubtractValueMax = (int)statResultB.max;
            //defectObj.ValueDiff = (int)valueDiff;

            a.Dispose();
            b.Dispose();
            c.Dispose();
        }

        protected void UpdateShapeType(DefectObj defectObj, BlobRect defectBlob)
        {
            if (defectObj.ValueType == DefectObj.EValueType.None)
                defectObj.ShapeType = DefectObj.EShapeType.Complex;
            else if (defectBlob.Compactness <= 2.5 || defectBlob.MaxFeretDiameter / defectBlob.MinFeretDiameter <= 2.0)
                defectObj.ShapeType = DefectObj.EShapeType.Circular;
            else
                defectObj.ShapeType = DefectObj.EShapeType.Linear;
        }

        private float GetDefectImageValue(BlobRect defectBlob, AlgoImage algoImage, Size inflate)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            float defectValue = defectBlob.MeanValue * defectBlob.Area;

            Rectangle defectRect = Rectangle.Round(defectBlob.BoundingRect);
            defectRect.Inflate(inflate);
            defectRect.Intersect(new Rectangle(Point.Empty, algoImage.Size));

            int rectArea = defectRect.Width * defectRect.Height;
            AlgoImage defectCenterImage = algoImage.GetSubImage(defectRect);
            float rectValue = imageProcessing.GetGreyAverage(defectCenterImage) * rectArea;
            defectCenterImage.Dispose();

            // 사각형 부분에서 불량 부분을 제외한 영역의 평균
            return (rectValue - defectValue) / (rectArea - defectBlob.Area);

            //float defectValue = defectBlob.MeanValue;

            //Rectangle defectAroundRect = Rectangle.Round(defectBlob.BoundingRect);
            //defectAroundRect.Inflate(inflate);
            //AlgoImage defectCenterImage = algoImage.GetSubImage(defectAroundRect);
            //float rectValue = imageProcessing.GetGreyAverage(defectCenterImage);
            //defectCenterImage.Dispose();

            //return rectValue;
        }

        private Gravure.Data.DefectObj CalculateBlob(AlgoImage subInspImage, AlgoImage subBlobImage, AlgoImage subBinalImage, AlgoImage subEgMapImage,
            SizeF pelSize, RegionInfoG regionInfoG, BlobRect defectBlob, DebugContextG debugContextG)
        {
            TrainerParam trainerParam = TrainerBase.TrainerParam;
            SheetFinderBaseParam sheetFinderBaseParam = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName).Param as SheetFinderBaseParam;
            int binValue = trainerParam.ModelParam.BinValue + trainerParam.ModelParam.BinValueOffset;
            float poleAvg = regionInfoG.PoleAvg;
            float dielectricAvg = regionInfoG.DielectricAvg;

            DetectorParam detectorParam = Detector.DetectorParam;

            DefectObj subResult = new DefectObj();

            //RectangleF avgIntensityRect = RectangleF.Empty;
            //bool isFalseNG = IsFalseNG(sheetImage, defectBlob, out avgIntensityRect);
            //if (isFalseNG)
            //{
            //    subResult.AvgIntensity = avgIntensityRect;
            //    subResult.IsValid = false;
            //    return subResult;
            //}

            // Reconstruct Blob
            //if (subEgMapImage != null)
            //    defectBlob = ReconstructionBlob(subInspImage, subBlobImage, subBinalImage, subEgMapImage, defectBlob, debugContextG);

            // 불량 종류 판단
            CalcDefectType(subInspImage, subBlobImage, subBinalImage, defectBlob, binValue, regionInfoG, subResult, debugContextG);

            // 불량 크기 판단
            CalcDefectSize(subBlobImage, defectBlob, binValue, pelSize, subResult, debugContextG);

            if (!IsValidSize(subResult))
                return null;

            // 핀홀인 경우 번짐여부 판단
            if (detectorParam.UseSpreadTrace && subResult.GetDefectType() == DefectType.PinHole)
                this.spreadTracer?.Inspect(subInspImage, subBinalImage, pelSize, subResult, debugContextG.Clone());

            return subResult;
        }

        private BlobRect ReconstructionBlob(AlgoImage subInspImage, AlgoImage subBlobImage, AlgoImage subBinalImage, AlgoImage subegMapImage,
            BlobRect defectBlob, DebugContextG debugContextG)
        {
            //return defectBlob;
            CalculatorParam calculatorParam = CalculatorBase.CalculatorParam;
            RectangleF boundingRect = defectBlob.BoundingRect;
            if (Math.Min(boundingRect.Width, boundingRect.Height) < 10)
                return defectBlob;

            Rectangle boundRect = Rectangle.Round(RectangleF.Inflate(defectBlob.BoundingRect, defectBlob.BoundingRect.Width, defectBlob.BoundingRect.Height));
            boundRect.Intersect(new Rectangle(Point.Empty, subInspImage.Size));
            PointF centerPt = new PointF(boundRect.Width / 2f, boundRect.Height / 2f);
            //Rectangle clipRect = Rectangle.Intersect(boundRect, new Rectangle(Point.Empty, subInspImage.Size));

            AlgoImage subInspImage2 = subInspImage.GetSubImage(boundRect);
            AlgoImage subBlobImage2 = subBlobImage.GetSubImage(boundRect);
            AlgoImage subBinalImage2 = subBinalImage.GetSubImage(boundRect);
            AlgoImage subegMapImage2 = subegMapImage.GetSubImage(boundRect);
            //AlgoImage temp = subBinalImage.Clip(boundRect);
            bool saveDebugImage = debugContextG.SaveDebugImage;
            //debugContextG.SaveDebugImage = true;

            try
            {
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(subInspImage);

                subInspImage2.Save("1-1.subInspImage2.bmp", debugContextG);
                subBlobImage2.Save("1-2.subBlobImage2.bmp", debugContextG);
                subBinalImage2.Save("1-3.subBinalImage2.bmp", debugContextG);
                subegMapImage2.Save("1-4.subegMapImage2.bmp", debugContextG);

                StatResult inspStat = ip.GetStatValue(subInspImage2, subBinalImage2);
                StatResult blobStat = ip.GetStatValue(subBlobImage2, subBinalImage2);

                int blobAverage = (int)Math.Round(blobStat.average);
                int binMin = (int)Math.Round(blobStat.min) / 2;
                int binMax = (int)Math.Round(blobStat.max);

                //ip.And(subInspImage2, subBinalImage2, subBinalImage2);
                //subBinalImage2.Save("2-1.subBinalImage2.2.bmp", debugContextG);
                ip.Binarize(subBlobImage2, subBinalImage2, binMin, binMax);
                subBinalImage2.Save("2-2.subBinalImage2.bmp", debugContextG);

                //ip.ReconstructIncludeBlob(subBlobImage2, temp, subBinalImage2, new ResconstructParam() { AllIncluded = true });
                //temp.Save("3.temp.bmp", debugContextG);

                BlobRect blobRect = defectBlob;
                using (BlobRectList blobRectList = ip.Blob(subBinalImage2, new BlobParam(), subInspImage2))
                    blobRect = Array.Find(blobRectList.GetArray(), f => f.BoundingRect.Contains(centerPt));

                return defectBlob;
            }
            finally
            {
                subInspImage2?.Dispose();
                subBlobImage2?.Dispose();
                subBinalImage2?.Dispose();
                subegMapImage2?.Dispose();
                //temp?.Dispose();

                debugContextG.SaveDebugImage = saveDebugImage;
            }
            return defectBlob;
            throw new NotImplementedException();
        }

        protected void UpdateBinalImage(AlgoImage subBinalImage, float scaleFactor, Data.DefectObj subResult)
        {
            Rectangle imageRect = new Rectangle(Point.Empty, subBinalImage.Size);

            RectangleF defectRect = subResult.Region;
            Size clipSize = new Size(50, 50);

            // 검출 이미지 크롭
            Rectangle scaledDefectRect = Rectangle.Round(DrawingHelper.Mul(defectRect, scaleFactor));
            Size scaledClipSize = DrawingHelper.Mul(clipSize, scaleFactor);

            Rectangle clipRect = Rectangle.Round(RectangleF.Inflate(defectRect, scaledClipSize.Width, scaledClipSize.Height));
            clipRect.Intersect(imageRect);
            if (clipRect.Width > 0 && clipRect.Height > 0)
            {
                using (AlgoImage algoImage = subBinalImage.Clip(clipRect))
                {
                    subResult.BufImage = algoImage.ToBitmap();
                    //subBinalImage.Save(string.Format(@"{0}_subBinalImage.bmp", blobId), debugContext);
                }
            }
        }

        protected void UpdateDefectImage(AlgoImage fullImage, Data.DefectObj subResult)
        {
            Rectangle imageRect = new Rectangle(Point.Empty, fullImage.Size);

            RectangleF defectRect = subResult.Region;
            Size clipSize = new Size(50, 50);

            // 원본 이미지 크롭
            Rectangle clipRect = Rectangle.Round(RectangleF.Inflate(defectRect, clipSize.Width, clipSize.Height));
            clipRect.Intersect(imageRect);
            if (clipRect.Width > 0 && clipRect.Height > 0)
            {
                using (AlgoImage algoImage = fullImage.Clip(clipRect))
                {
                    subResult.Image = algoImage.ToBitmap();
                    //subImage.Save(string.Format(@"{0}_subImage.bmp", blobId), debugContext);
                }
            }
        }

        protected void CalcDefectSize(AlgoImage blobImage, BlobRect defectBlob, int binValue,
            SizeF pelSize, Data.DefectObj subResult, DebugContextG debugContext)
        {
            DetectorParam detectorParam = Detector.DetectorParam;

            RectangleF boundRect = defectBlob.BoundingRect;
            float boundLength = MathHelper.GetLength(boundRect.Size);

            PointF realLoc = new PointF(boundRect.X * pelSize.Width, boundRect.Y * pelSize.Height);
            SizeF realSize = new SizeF(boundRect.Width * pelSize.Width, boundRect.Height * pelSize.Height);
            RectangleF realRect = new RectangleF(realLoc, realSize);

            Rectangle defectRect = Rectangle.Round(boundRect);
            AlgoImage defectBlobImage = blobImage.GetSubImage(defectRect);

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(blobImage);
            int maxSubtractValue = (int)ip.GetGreyMax(defectBlobImage);

            if (detectorParam.FineSizeMeasure)
            {
                float maxValue = ip.GetGreyMax(defectBlobImage);
                int newThreshold = (int)Math.Round(maxValue * detectorParam.FineSizeMeasureThresholdMul);
                if (newThreshold > binValue)
                {
                    int scaleF = detectorParam.FineSizeMeasureSizeMul;
                    //defectBlobImage.Save(string.Format("{0}_defectBlobImage.bmp", blobId), debugContext);
                    AlgoImage defectBinalImage = ImageBuilder.Build(defectBlobImage.LibraryType, defectBlobImage.ImageType, defectBlobImage.Width * scaleF, defectBlobImage.Height * scaleF);
                    ip.Resize(defectBlobImage, defectBinalImage, scaleF);
                    //defectBlobImage.Save(string.Format("{0}_defectBinalImage1.bmp", blobId), debugContext);
                    ip.Binarize(defectBinalImage, newThreshold);
                    //defectBlobImage.Save(string.Format("{0}_defectBinalImage2.bmp", blobId), debugContext);

                    BlobRectList blobRectList = ip.Blob(defectBinalImage, new BlobParam { SelectArea = true, SelectBoundingRect = true });
                    List<BlobRect> blobRects = blobRectList.GetList();
                    BlobRect blobRect = blobRects.Find(f => f.BoundingRect.Contains(new PointF(defectBinalImage.Width / 2.0f, defectBinalImage.Height / 2.0f)));
                    if (blobRect != null)
                    {
                        PointF location = PointF.Add(defectBlob.BoundingRect.Location, new SizeF(blobRect.BoundingRect.X / scaleF, blobRect.BoundingRect.Y / scaleF));
                        SizeF size = new SizeF(blobRect.BoundingRect.Width / scaleF, blobRect.BoundingRect.Height / scaleF);
                        boundRect = new RectangleF(location, size);
                        boundLength = MathHelper.GetLength(boundRect.Size);
                        defectRect = Rectangle.Round(boundRect);

                        realLoc = new PointF(boundRect.X * pelSize.Width, boundRect.Y * pelSize.Height);
                        realSize = new SizeF(boundRect.Width * pelSize.Width, boundRect.Height * pelSize.Height);
                        realRect = new RectangleF(realLoc, realSize);
                    }

                    blobRectList.Dispose();
                    defectBinalImage.Dispose();
                }
            }
            defectBlobImage.Dispose();

            subResult.Region = defectRect;
            subResult.RealRegion = realRect;
        }

        private bool IsFalseNG(AlgoImage algoImage, BlobRect defectBlob, out RectangleF avgIntensityRect)
        {
            // 가성확인
            avgIntensityRect = RectangleF.Empty;

            Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);
            Rectangle defectRect = Rectangle.Round(defectBlob.BoundingRect);
            Rectangle adjustDefectRect = Rectangle.Intersect(Rectangle.Inflate(defectRect, 10, 10), imageRect);
            if (adjustDefectRect.Width == 0 || adjustDefectRect.Height == 0)
                return true;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            float maxRect = Math.Max(defectBlob.BoundingRect.Width, defectBlob.BoundingRect.Height);
            float minRect = Math.Min(defectBlob.BoundingRect.Width, defectBlob.BoundingRect.Height);
            bool isLonged = (maxRect / minRect >= 2);
            float isFulled = defectBlob.Area * 1.0f / (defectBlob.BoundingRect.Width * defectBlob.BoundingRect.Height);
            if (isLonged)
            {
                AlgoImage subAlgoImage = algoImage.GetSubImage(adjustDefectRect);
                float[] avgIntensity = new float[4];
                for (int i = 0; i < 4; i++)
                {
                    int l = i == 2 ? adjustDefectRect.Width / 2 : 0;
                    int t = i == 3 ? adjustDefectRect.Height / 2 : 0;
                    int r = i == 0 ? adjustDefectRect.Width / 2 : adjustDefectRect.Width;
                    int b = i == 1 ? adjustDefectRect.Height / 2 : adjustDefectRect.Height;
                    Rectangle sRect = Rectangle.FromLTRB(l, t, r, b);
                    avgIntensity[i] = imageProcessing.GetGreyAverage(subAlgoImage, sRect);
                }
                avgIntensityRect = RectangleF.FromLTRB(avgIntensity[0], avgIntensity[1], avgIntensity[2], avgIntensity[3]);

                bool isFalseNG = false;
                if (defectBlob.BoundingRect.Width > defectBlob.BoundingRect.Height)
                    isFalseNG = (Math.Abs(avgIntensity[1] - avgIntensity[3]) > 70);
                else
                    isFalseNG = (Math.Abs(avgIntensity[0] - avgIntensity[2]) > 70);

                if (isFalseNG)
                {
                    //LogHelper.Debug(LoggerType.Algorithm, "Detector::CalculateBlob - FalseNG Detected.");
                    //string fileName = string.Format("FalseNG_{0}.bmp", DateTime.Now.ToString("yyyyMMdd_HHmmss_fff"));
                    //subAlgoImage.Save(Path.Combine(UniEye.Base.Settings.PathSettings.Instance().Temp, fileName));
                }
                subAlgoImage.Dispose();
                return isFalseNG;
            }
            else
            {
                return false;
            }
        }

        protected List<BlobRect> BlobProcess(AlgoImage subInspImage, AlgoImage subBinalImage, DebugContext debugContext)
        {
            try
            {
                subInspImage.Save("subInspImage.bmp", debugContext);

                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(subBinalImage);
                Rectangle imageRect = new Rectangle(Point.Empty, subBinalImage.Size);

                BlobParam blobParam = GetBlobParam();

                BlobRectList blobResult = imageProcessing.Blob(subBinalImage, blobParam, subInspImage);
                List<BlobRect> blobRectList = blobResult.GetList();
                blobResult.Dispose();

                RectangleF wholeRect = new RectangleF(0, 0, subBinalImage.Width, subBinalImage.Height);
                MergeBlobs(blobRectList, wholeRect, 0);
                RemoveBlobs(blobRectList);

                return blobRectList;
            }
#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, string.Format("Exception Occure - Detector::Process - {0}", ex.Message));
                return null;
            }
#endif
            finally { }
        }

        private void RemoveBlobs(List<BlobRect> blobRectList)
        {
            DetectorParam detectorParam = Detector.DetectorParam;
            if (detectorParam.IgnoreLongDefect)
            {
                blobRectList.RemoveAll(f =>
                {
                    float min, max;
                    if (f.RotateWidth > 0 && f.RotateHeight > 0)
                    {
                        min = Math.Min(f.RotateWidth, f.RotateHeight);
                        max = Math.Max(f.RotateWidth, f.RotateHeight);
                    }
                    else
                    {
                        RectangleF blobRect = f.BoundingRect;
                        min = Math.Min(blobRect.Width, blobRect.Height);
                        max = Math.Max(blobRect.Width, blobRect.Height);
                    }
                    return (min <= 3 && max >= min * 2);
                    //return min <= 3;
                });
            }
        }

        private void MergeBlobs(List<BlobRect> blobRectList, RectangleF wholeRect, int inflate)
        {
            if (inflate == 0)
                return;

            List<BlobRect> mergedBlobRectList = new List<BlobRect>(blobRectList);
            while (blobRectList.Count > 0)
            {
                RectangleF rectangle = blobRectList[0].BoundingRect;
                rectangle.Inflate(inflate, inflate);

                List<BlobRect> intersectedBlobRectList = blobRectList.FindAll(f => rectangle.IntersectsWith(f.BoundingRect));
                BlobRect mergedBlobRect = intersectedBlobRectList.FirstOrDefault();
                for (int i = 1; i < intersectedBlobRectList.Count; i++)
                {
                    BlobRect targetBlobRect = intersectedBlobRectList[i];
                    mergedBlobRect.Area += targetBlobRect.Area;

                    mergedBlobRect.BoundingRect = RectangleF.Union(mergedBlobRect.BoundingRect, targetBlobRect.BoundingRect);
                    mergedBlobRect.CenterPt = new PointF(
                        (mergedBlobRect.CenterPt.X * i + targetBlobRect.CenterPt.X) / (i + 1),
                        (mergedBlobRect.CenterPt.Y * i + targetBlobRect.CenterPt.Y) / (i + 1)
                        );
                    mergedBlobRect.MinValue = Math.Min(mergedBlobRect.MinValue, targetBlobRect.MinValue);
                    mergedBlobRect.MaxValue = Math.Max(mergedBlobRect.MaxValue, targetBlobRect.MaxValue);
                    mergedBlobRect.MeanValue = (mergedBlobRect.MeanValue * i + targetBlobRect.MeanValue) / (i + 1);
                }

                mergedBlobRectList.Add(mergedBlobRect);
                blobRectList.RemoveAll(f => intersectedBlobRectList.Contains(f));
            }

            blobRectList.AddRange(mergedBlobRectList);
        }

        //private void MergeBlobs(List<BlobRect> blobRectList, RectangleF wholeRect, int inflate)
        //{
        //    if (inflate == 0)
        //        return;

        //    bool merged = true;

        //    List<BlobRect> mergedBlobRect = new List<BlobRect>(blobRectList);

        //    int tryNum = 0;
        //    while (merged == true)
        //    {
        //        merged = false;

        //        if (tryNum % 2 == 0)
        //            mergedBlobRect = mergedBlobRect.OrderBy(defect => defect.BoundingRect.X).ToList();
        //        else
        //            mergedBlobRect = mergedBlobRect.OrderBy(defect => defect.BoundingRect.Y).ToList();

        //        for (int srcIndex = 0; srcIndex < mergedBlobRect.Count; srcIndex++)
        //        {
        //            BlobRect srcBlob = mergedBlobRect[srcIndex];

        //            RectangleF inflateRect = srcBlob.BoundingRect;
        //            inflateRect.Inflate(inflate, inflate);

        //            int endSearchIndex = srcIndex;

        //            if (tryNum % 2 == 0)
        //            {
        //                for (int i = endSearchIndex; i < mergedBlobRect.Count; i++)
        //                {
        //                    if (mergedBlobRect[i].BoundingRect.Left - srcBlob.BoundingRect.Right <= inflate)
        //                        endSearchIndex = i;
        //                    else
        //                        break;
        //                }
        //            }
        //            else
        //            {
        //                for (int i = endSearchIndex; i < mergedBlobRect.Count; i++)
        //                {
        //                    if (mergedBlobRect[i].BoundingRect.Top - srcBlob.BoundingRect.Bottom <= inflate)
        //                        endSearchIndex = i;
        //                    else
        //                        break;
        //                }
        //            }

        //            for (int destIndex = srcIndex + 1; destIndex <= endSearchIndex; destIndex++)
        //            {
        //                BlobRect destBlob = mergedBlobRect[destIndex];

        //                if (inflateRect.IntersectsWith(destBlob.BoundingRect) == true)
        //                {
        //                    srcBlob.Area += destBlob.Area;

        //                    srcBlob.BoundingRect = RectangleF.Union(srcBlob.BoundingRect, destBlob.BoundingRect);
        //                    srcBlob.CenterPt = new PointF((srcBlob.CenterPt.X + destBlob.CenterPt.X) / 2.0f, (srcBlob.CenterPt.Y + destBlob.CenterPt.Y) / 2.0f);
        //                    srcBlob.MinValue = Math.Min(srcBlob.MinValue, destBlob.MinValue);
        //                    srcBlob.MaxValue = Math.Max(srcBlob.MaxValue, destBlob.MaxValue);
        //                    srcBlob.MeanValue = (srcBlob.MeanValue + destBlob.MeanValue) / 2;

        //                    inflateRect = srcBlob.BoundingRect;
        //                    inflateRect.Inflate(inflate, inflate);

        //                    mergedBlobRect.RemoveAt(destIndex);

        //                    endSearchIndex--;
        //                    destIndex--;

        //                    if (tryNum % 2 == 0)
        //                    {
        //                        for (int i = endSearchIndex; i < mergedBlobRect.Count; i++)
        //                        {
        //                            if (mergedBlobRect[i].BoundingRect.Left - srcBlob.BoundingRect.Right <= inflate)
        //                                endSearchIndex = i;
        //                            else
        //                                break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        for (int i = endSearchIndex; i < mergedBlobRect.Count; i++)
        //                        {
        //                            if (mergedBlobRect[i].BoundingRect.Top - srcBlob.BoundingRect.Bottom <= inflate)
        //                                endSearchIndex = i;
        //                            else
        //                                break;
        //                        }
        //                    }

        //                    if (merged == false)
        //                        merged = true;
        //                }
        //            }
        //        }

        //        if (merged == true)
        //            tryNum++;
        //    }

        //    blobRectList.Clear();
        //    blobRectList.AddRange(mergedBlobRect);
        //}
    }
}

