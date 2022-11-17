using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using DynMvp.UI;
using DynMvp.Base;
using System.Xml;

namespace DynMvp.Vision
{
    public class BlobCheckerParam : AlgorithmParam
    {
        bool darkBlob = true;
        public bool DarkBlob
        {
            get { return darkBlob; }
            set { darkBlob = value; }
        }

        private int searchRangeWidth = 0;
        public int SearchRangeWidth
        {
            get { return searchRangeWidth; }
            set { searchRangeWidth = value; }
        }

        private int searchRangeHeight = 0;
        public int SearchRangeHeight
        {
            get { return searchRangeHeight; }
            set { searchRangeHeight = value; }
        }

        private float offsetRangeX = 0;
        public float OffsetRangeX
        {
            get { return offsetRangeX; }
            set { offsetRangeX = value; }
        }

        private float offsetRangeY = 0;
        public float OffsetRangeY
        {
            get { return offsetRangeY; }
            set { offsetRangeY = value; }
        }

        private int minArea = 0;
        public int MinArea
        {
            get { return minArea; }
            set { minArea = value; }
        }

        private int maxArea = 0;
        public int MaxArea
        {
            get { return maxArea; }
            set { maxArea = value; }
        }

        private bool useRealOffset;
        public bool UseRealOffset
        {
            get { return useRealOffset; }
            set { useRealOffset = value; }
        }

        private bool useWholeImage;
        public bool UseWholeImage
        {
            get { return useWholeImage; }
            set { useWholeImage = value; }
        }

        private bool actAsFiducial;
        public bool ActAsFiducial
        {
            get { return actAsFiducial; }
            set { actAsFiducial = value; }
        }

        public override AlgorithmParam Clone()
        {
            BlobCheckerParam param = new BlobCheckerParam();

            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            BlobCheckerParam param = (BlobCheckerParam)srcAlgorithmParam;

            darkBlob = param.darkBlob;
            searchRangeWidth = param.searchRangeWidth;
            searchRangeHeight = param.searchRangeHeight;
            offsetRangeX = param.offsetRangeX;
            offsetRangeY = param.offsetRangeY;
            minArea = param.minArea;
            maxArea = param.maxArea;
            useWholeImage = param.useWholeImage;
            actAsFiducial = param.actAsFiducial;
            useRealOffset = param.useRealOffset;
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);

            darkBlob = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "DarkBlob", "false"));
            searchRangeWidth = Convert.ToInt32(XmlHelper.GetValue(paramElement, "SearchRangeWidth", "0"));
            searchRangeHeight = Convert.ToInt32(XmlHelper.GetValue(paramElement, "SearchRangeHeight", "0"));
            offsetRangeX = Convert.ToInt32(XmlHelper.GetValue(paramElement, "OffsetRangeX", "0"));
            offsetRangeY = Convert.ToInt32(XmlHelper.GetValue(paramElement, "OffsetRangeY", "0"));
            minArea = Convert.ToInt32(XmlHelper.GetValue(paramElement, "MinArea", "0"));
            maxArea = Convert.ToInt32(XmlHelper.GetValue(paramElement, "MaxArea", "0"));

            useWholeImage = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "UseWholeImage", "false"));
            useRealOffset = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "UseRealOffset", "false"));
            actAsFiducial = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "ActAsFiducial", "false"));
        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);

            XmlHelper.SetValue(paramElement, "DarkBlob", darkBlob.ToString());
            XmlHelper.SetValue(paramElement, "SearchRangeWidth", searchRangeWidth.ToString());
            XmlHelper.SetValue(paramElement, "SearchRangeHeight", searchRangeHeight.ToString());
            XmlHelper.SetValue(paramElement, "OffsetRangeX", offsetRangeX.ToString());
            XmlHelper.SetValue(paramElement, "OffsetRangeY", offsetRangeY.ToString());

            XmlHelper.SetValue(paramElement, "MinArea", minArea.ToString());
            XmlHelper.SetValue(paramElement, "MaxArea", maxArea.ToString());

            XmlHelper.SetValue(paramElement, "UseWholeImage", useWholeImage.ToString());
            XmlHelper.SetValue(paramElement, "UseRealOffset", useRealOffset.ToString());
            XmlHelper.SetValue(paramElement, "ActAsFiducial", actAsFiducial.ToString());
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class BlobChecker : Algorithm, Searchable
    {
        public BlobChecker()
        {
            param = new BlobCheckerParam();
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            BlobCheckerParam blobCheckerParam = (BlobCheckerParam)param;
        }

        public override Algorithm Clone()
        {
            BlobChecker blobChecker = new BlobChecker();
            blobChecker.CopyFrom(this);

            return blobChecker;
        }

        public Size GetSearchRangeSize()
        {
            BlobCheckerParam blobCheckerParam = (BlobCheckerParam)param;

            return new Size(blobCheckerParam.SearchRangeWidth, blobCheckerParam.SearchRangeHeight);
        }

        public void SetSearchRangeSize(Size searchRange)
        {
            BlobCheckerParam blobCheckerParam = (BlobCheckerParam)param;

            blobCheckerParam.SearchRangeWidth = searchRange.Width;
            blobCheckerParam.SearchRangeHeight = searchRange.Height;
        }

        public static string TypeName
        {
            get { return "BlobChecker"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Blob";
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            BlobCheckerParam blobCheckerParam = (BlobCheckerParam)param;

            if (blobCheckerParam.ActAsFiducial)
                inspRegion.Inflate(blobCheckerParam.SearchRangeWidth, blobCheckerParam.SearchRangeHeight);
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("Area", 0, 0, 0));

            resultValues.Add(new AlgorithmResultValue("Blob Center X", 0, 0, 0));
            resultValues.Add(new AlgorithmResultValue("Blob Center Y", 0, 0, 0));

            resultValues.Add(new AlgorithmResultValue("Ref. Center X", 0, 0, 0));
            resultValues.Add(new AlgorithmResultValue("Ref. Center Y", 0, 0, 0));

            resultValues.Add(new AlgorithmResultValue("Offset X", 0, 0, 0));
            resultValues.Add(new AlgorithmResultValue("Offset Y", 0, 0, 0));

            resultValues.Add(new AlgorithmResultValue("Real Offset Y", 0, 0, 0));
            resultValues.Add(new AlgorithmResultValue("Real Offset Y", 0, 0, 0));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.ClipImage, ImageType.Grey, param.ImageBand);
            Filter(clipImage);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect clipRegionInFov = inspectParam.ClipRegionInFov;
            Size wholeImageSize = inspectParam.WholeImageSize;
            Calibration cameraCalibration = inspectParam.CameraCalibration;
            DebugContext debugContext = inspectParam.DebugContext;

            clipImage.Save("FilterImage.bmp", debugContext);

            AlgorithmResult blobCheckerResult = CreateAlgorithmResult();

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(clipImage);

            BlobParam blobParam = new BlobParam();
            blobParam.SelectCenterPt = true;
            blobParam.SelectBoundingRect = true;

            BlobCheckerParam blobCheckerParam = (BlobCheckerParam)param;

            AlgoImage blobImage = clipImage.Clone();
            if (blobCheckerParam.DarkBlob)
                imageProcessing.Not(blobImage, blobImage);

            blobImage.Save("Blob.bmp", debugContext);

            BlobRectList blobRectList = imageProcessing.Blob(blobImage, blobParam);
            
            BlobRect blobRect = blobRectList.GetMaxAreaBlob();
            if (blobRect == null)
                return blobCheckerResult;

            PointF refPosInFov;
            PointF foundPosInFov = DrawingHelper.ClipToFov(clipRegionInFov, blobRect.CenterPt);
            if (blobCheckerParam.UseWholeImage)
                refPosInFov = new PointF(wholeImageSize.Width/2, wholeImageSize.Height/2);
            else
                refPosInFov = DrawingHelper.CenterPoint(probeRegionInFov);

            SizeF realOffset = new SizeF(0, 0);
            SizeF offset = new SizeF(foundPosInFov.X - refPosInFov.X, foundPosInFov.Y - refPosInFov.Y);

            bool fCalibrated = false;

            if (cameraCalibration != null)
                fCalibrated = cameraCalibration.IsCalibrated();

            if (fCalibrated)
            {
                PointF realRefPos = cameraCalibration.PixelToWorld(refPosInFov);
                PointF realFoundPos = cameraCalibration.PixelToWorld(foundPosInFov);

                realOffset = new SizeF(realFoundPos.X - realRefPos.X, realFoundPos.Y - realRefPos.Y);
            }

            blobCheckerResult.OffsetFound = offset;
            blobCheckerResult.RealOffsetFound = realOffset;

            bool areaResult = (blobRect.Area >= blobCheckerParam.MinArea && blobRect.Area <= blobCheckerParam.MaxArea);

            bool offsetResultX = false;
            bool offsetResultY = false;
            if (blobCheckerParam.UseRealOffset)
            {
                if (fCalibrated)
                {
                    offsetResultX = (Math.Abs(realOffset.Width) < blobCheckerParam.OffsetRangeX);
                    offsetResultY = (Math.Abs(realOffset.Height) < blobCheckerParam.OffsetRangeY);
                }
            }
            else
            {
                offsetResultX = (Math.Abs(offset.Width) < blobCheckerParam.OffsetRangeX);
                offsetResultY = (Math.Abs(offset.Height) < blobCheckerParam.OffsetRangeY);
            }

            RotatedRect resultRect = probeRegionInFov;
            if (blobCheckerParam.ActAsFiducial)
                resultRect.Offset(offset.Width, offset.Height);
            blobCheckerResult.ResultRect = resultRect;

            blobCheckerResult.Good = areaResult && offsetResultX && offsetResultY;
            blobCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Area", 0, 0, blobRect.Area, areaResult));

            blobCheckerResult.ResultValueList.Add(new AlgorithmResultValue("BlobCenterX", 0, 0, foundPosInFov.X));
            blobCheckerResult.ResultValueList.Add(new AlgorithmResultValue("BlobCenterY", 0, 0, foundPosInFov.Y));

            blobCheckerResult.ResultValueList.Add(new AlgorithmResultValue("RefCenterX", 0, 0, refPosInFov.X));
            blobCheckerResult.ResultValueList.Add(new AlgorithmResultValue("RefCenterY", 0, 0, refPosInFov.Y));

            if (blobCheckerParam.UseRealOffset)
            {
                blobCheckerResult.ResultValueList.Add(new AlgorithmResultValue("OffsetX", 0, 0, offset.Width));
                blobCheckerResult.ResultValueList.Add(new AlgorithmResultValue("OffsetY", 0, 0, offset.Height));

                blobCheckerResult.ResultValueList.Add(new AlgorithmResultValue("RealOffsetY", 0, 0, realOffset.Width, fCalibrated && offsetResultX));
                blobCheckerResult.ResultValueList.Add(new AlgorithmResultValue("RealOffsetY", 0, 0, realOffset.Height, fCalibrated && offsetResultY));
            }
            else
            {
                blobCheckerResult.ResultValueList.Add(new AlgorithmResultValue("OffsetX", 0, 0, offset.Width, offsetResultX));
                blobCheckerResult.ResultValueList.Add(new AlgorithmResultValue("OffsetY", 0, 0, offset.Height, offsetResultY));
            }

            Pen pen = new Pen(Color.Purple, 10.0F);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            CrossFigure crossFigure = new CrossFigure(foundPosInFov, 3, pen);
            blobCheckerResult.ResultFigureGroup.AddFigure(crossFigure);

            return blobCheckerResult;
        }
    }
}
