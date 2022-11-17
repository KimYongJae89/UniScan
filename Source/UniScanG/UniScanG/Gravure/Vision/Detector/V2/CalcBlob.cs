using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.RCI.Calculator;

namespace UniScanG.Gravure.Vision.Detector.V2
{
    class CalcBlob
    {
        public static bool CalcDefectType(Inspect.ProcessBufferSetG buffer, AlgoImage subInspImage, AlgoImage subBlobImage, AlgoImage subBinalImage, BlobRect blobRect, DefectObj defectObj, DebugContextG debugContextG)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(subBinalImage);
            ProcessBufferSetG3 bufferSetG3 = (ProcessBufferSetG3)buffer;
            float gvInModel = 0, gvInTarget = 0;

            Rectangle defectInTarget = Rectangle.Round(blobRect.BoundingRect);
            using (AlgoImage algoImage = bufferSetG3.TargetFullImage.GetSubImage(defectInTarget))
                gvInTarget = ip.GetGreyAverage(algoImage);
             
            Rectangle defectInTargetRoi = DrawingHelper.Offset(defectInTarget, bufferSetG3.TargetRoi.Location, true);
            BlockResult blockResult = Array.Find(bufferSetG3.BlockResults, f => f.TargetRect.IntersectsWith(defectInTargetRoi));
            Rectangle defectInBlock = DrawingHelper.Offset(defectInTargetRoi, blockResult.TargetRect.Location, true);
            Rectangle defectInModelRoi = DrawingHelper.Offset(defectInBlock, blockResult.ModelRect.Location);
            using (AlgoImage algoImage = bufferSetG3.ModelRoiImage.GetSubImage(defectInModelRoi))
                gvInModel = ip.GetGreyAverage(algoImage);

            float otsu = SystemManager.Instance().CurrentModel.RCITrainResult.Otsu;

            // 불량 종류 판단
            defectObj.PositionType = (gvInModel > otsu) ? DefectObj.EPositionType.Dielectric : DefectObj.EPositionType.Pole;
            defectObj.ValueType = (gvInModel > gvInTarget) ? DefectObj.EValueType.Dark : DefectObj.EValueType.Bright;

            if (blobRect.Compactness <= 2.5 || blobRect.MaxFeretDiameter / blobRect.MinFeretDiameter <= 2.0)
                defectObj.ShapeType = DefectObj.EShapeType.Circular;
            else
                defectObj.ShapeType = DefectObj.EShapeType.Linear;


            defectObj.SubtractValueMax = (int)blobRect.MaxValue;
            defectObj.FillRate = blobRect.Area * 100f / (blobRect.BoundingRect.Width * blobRect.BoundingRect.Height);
            defectObj.ValueDiff = blobRect.MaxValue - blobRect.MinValue;

            var v = defectObj.GetDefectType();
            //if (blobRect.BoundingRect.Width > 3 && blobRect.BoundingRect.Height > 3)
            //{
            //    using (AlgoImage algoImage = bufferSetG3.TargetFullImage.GetSubImage(Rectangle.Intersect(new Rectangle(Point.Empty, bufferSetG3.TargetFullImage.Size), Rectangle.Inflate(defectInTarget, 30, 30))))
            //        algoImage.Save(@"D:\temp\DefectInTarget.bmp");
            //    using (AlgoImage algoImage = bufferSetG3.CalculatorResultBinal.GetSubImage(Rectangle.Intersect(new Rectangle(Point.Empty, bufferSetG3.CalculatorResultBinal.Size), Rectangle.Inflate(defectInTarget, 30, 30))))
            //        algoImage.Save(@"D:\temp\Binal.bmp");
            //    using (AlgoImage algoImage = bufferSetG3.ModelRoiImage.GetSubImage(Rectangle.Intersect(new Rectangle(Point.Empty, bufferSetG3.ModelRoiImage.Size), Rectangle.Inflate(defectInModelRoi, 30, 30))))
            //        algoImage.Save(@"D:\temp\DefectInModel.bmp");
            //}
            return true;
        }


        private static void CalcDefectSize(AlgoImage blobImage, BlobRect defectBlob, int binValue,
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


        private static bool IsValidSize(DefectObj subResult)
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
    }
}
