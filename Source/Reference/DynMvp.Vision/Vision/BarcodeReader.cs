using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;

using DynMvp.Base;
using DynMvp.UI;

namespace DynMvp.Vision
{
    public enum BarcodeType
    {
        //Common
        Codabar, Code128, Code39, Code93, Interleaved2of5, Pharmacode, PLANET, POSTNET, FourStatePostal, //1D Barcode
        DataMatrix, QRCode, //2D Barcode

        //Cognex
        UPCEAN, EANUCCComposite, PDF417, // 1D Barcode
        
        //MIL
        BC412, EAN8, EAN13, EAN14, UPC_A, UPC_E, GS1_128, GS1Databar //1D Barcode
    }

    public class BarcodeReaderConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(Algorithm))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is BarcodeReader)
            {
                BarcodeReader barcodeReader = (BarcodeReader)value;
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class BarcodeReaderParam : AlgorithmParam
    {
        private string desiredString = "";
        public string DesiredString
        {
            get { return desiredString; }
            set { desiredString = value; }
        }

        private int desiredNum = 1;
        public int DesiredNum
        {
            get { return desiredNum; }
            set { desiredNum = value; }
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

        private bool offsetRange = false;
        public bool OffsetRange
        {
            get { return offsetRange; }
            set { offsetRange = value; }
        }

        private int rangeThresholdLeft = 0;
        public int RangeThresholdLeft
        {
            get { return rangeThresholdLeft; }
            set { rangeThresholdLeft = value; }
        }

        private int rangeThresholdRight = 0;
        public int RangeThresholdRight
        {
            get { return rangeThresholdRight; }
            set { rangeThresholdRight = value; }
        }

        private int rangeThresholdBottom = 0;
        public int RangeThresholdBottom
        {
            get { return rangeThresholdBottom; }
            set { rangeThresholdBottom = value; }
        }

        private int rangeThresholdTop = 0;
        public int RangeThresholdTop
        {
            get { return rangeThresholdTop; }
            set { rangeThresholdTop = value; }
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

        private int closeNum = 3;
        public int CloseNum
        {
            get { return closeNum; }
            set { closeNum = value; }
        }

        private int thresholdPercent = 50;
        public int ThresholdPercent
        {
            get { return thresholdPercent; }
            set { thresholdPercent = value; }
        }

        List<int> thresholdPercentList = new List<int>();
        public List<int> ThresholdPercentList
        {
            get { return thresholdPercentList; }
            set { thresholdPercentList = value; }
        }

        List<BarcodeType> barcodeTypeList = new List<BarcodeType>();
        public List<BarcodeType> BarcodeTypeList
        {
            get { return barcodeTypeList; }
            set { barcodeTypeList = value; }
        }

        bool useAreaFilter = false;
        public bool UseAreaFilter
        {
            get { return useAreaFilter; }
            set { useAreaFilter = value; }
        }

        bool useBlobing = false;
        public bool UseBlobing
        {
            get { return useBlobing; }
            set { useBlobing = value; }
        }
        int timeoutTime = 5000;
        public int TimeoutTime
        {
            get { return timeoutTime; }
            set { timeoutTime = value; }
        }
        public override AlgorithmParam Clone()
        {
            BarcodeReaderParam param = new BarcodeReaderParam();

            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            BarcodeReaderParam param = (BarcodeReaderParam)srcAlgorithmParam;

            desiredString = param.desiredString;
            desiredNum = param.desiredNum;
            barcodeTypeList = param.barcodeTypeList;
            offsetRange = param.offsetRange;
            rangeThresholdLeft = param.rangeThresholdLeft;
            rangeThresholdRight = param.rangeThresholdRight;
            rangeThresholdBottom = param.rangeThresholdBottom;
            rangeThresholdTop = param.rangeThresholdTop;
            minArea = param.minArea;
            maxArea = param.maxArea;
            thresholdPercent = param.thresholdPercent;
            useAreaFilter = param.useAreaFilter;
            closeNum = param.closeNum;
            useBlobing = param.useBlobing;

            thresholdPercentList = param.thresholdPercentList;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            desiredString = XmlHelper.GetValue(algorithmElement, "DesiredString", "");
            desiredNum = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "DesiredNum", "0"));
            searchRangeWidth = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeWidth", "30"));
            searchRangeHeight = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeHeight", "30"));

            offsetRange = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "OffsetRange", "false"));
            rangeThresholdRight = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "RangeThresholdRight", "30"));
            rangeThresholdLeft = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "RangeThresholdLeft", "30"));
            rangeThresholdBottom = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "RangeThresholdBottom", "30"));
            rangeThresholdTop = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "RangeThresholdTop", "30"));

            minArea = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MinArea", "0"));
            maxArea = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MaxArea", "0"));

            useAreaFilter = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseAreaFilter", "false"));
            useBlobing = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseBlobing", "false"));

            closeNum = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "CloseNum", "0"));

            foreach (XmlElement barcodeTypeElement in algorithmElement)
            {
                if (barcodeTypeElement.Name == "BarcodeType")
                    barcodeTypeList.Add((BarcodeType)Enum.Parse(typeof(BarcodeType), barcodeTypeElement.InnerText));
            }

            foreach (XmlElement thresholdPercentElement in algorithmElement)
            {
                if (thresholdPercentElement.Name == "ThresholdPercent")
                    thresholdPercentList.Add(Convert.ToInt32(thresholdPercentElement.InnerText));
            }
            timeoutTime = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "TimeoutTime", "5000"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "DesiredString", desiredString);
            XmlHelper.SetValue(algorithmElement, "DesiredNum", desiredNum.ToString());
            XmlHelper.SetValue(algorithmElement, "SearchRangeWidth", searchRangeWidth.ToString());
            XmlHelper.SetValue(algorithmElement, "SearchRangeHeight", searchRangeHeight.ToString());

            XmlHelper.SetValue(algorithmElement, "OffsetRange", offsetRange.ToString());
            XmlHelper.SetValue(algorithmElement, "RangeThresholdRight", rangeThresholdRight.ToString());
            XmlHelper.SetValue(algorithmElement, "RangeThresholdLeft", rangeThresholdLeft.ToString());
            XmlHelper.SetValue(algorithmElement, "RangeThresholdBottom", rangeThresholdBottom.ToString());
            XmlHelper.SetValue(algorithmElement, "RangeThresholdTop", rangeThresholdTop.ToString());

            XmlHelper.SetValue(algorithmElement, "MinArea", minArea.ToString());
            XmlHelper.SetValue(algorithmElement, "MaxArea", maxArea.ToString());

            XmlHelper.SetValue(algorithmElement, "UseAreaFilter", useAreaFilter.ToString());
            XmlHelper.SetValue(algorithmElement, "UseBlobing", useBlobing.ToString());

            XmlHelper.SetValue(algorithmElement, "CloseNum", closeNum.ToString());

            foreach (BarcodeType barcodeType in barcodeTypeList)
            {
                XmlHelper.SetValue(algorithmElement, "BarcodeType", barcodeType.ToString());
            }

            foreach (int thresholdPercentOfList in thresholdPercentList)
            {
                XmlHelper.SetValue(algorithmElement, "ThresholdPercent", thresholdPercentOfList.ToString());
            }
            XmlHelper.SetValue(algorithmElement, "TimeoutTime", timeoutTime.ToString());
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    [TypeConverterAttribute(typeof(BarcodeReaderConverter))]
    public abstract class BarcodeReader : Algorithm, Searchable
    {
        string lastReadString;
        public string LastReadString
        {
            get { return lastReadString; }
        }

        public BarcodeReader()
        {
            param = new BarcodeReaderParam();
        }
        
        public override void CopyFrom(Algorithm algorithm)
        {
            base.CopyFrom(algorithm);

            BarcodeReader barcodeReader = (BarcodeReader)algorithm;
            param = (BarcodeReaderParam)barcodeReader.Param.Clone();
        }

        public static string TypeName
        {
            get { return "BarcodeReader"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Barcode";
        }

        public Size GetSearchRangeSize()
        {
            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)param;

            return new Size(barcodeReaderParam.SearchRangeWidth, barcodeReaderParam.SearchRangeHeight);
        }

        public void SetSearchRangeSize(Size searchRange)
        {
            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)param;

            barcodeReaderParam.SearchRangeWidth = searchRange.Width;
            barcodeReaderParam.SearchRangeHeight = searchRange.Height;
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)param;

            float rectWidth = barcodeReaderParam.RangeThresholdRight + barcodeReaderParam.RangeThresholdLeft + region.Width;
            float rectHeight = barcodeReaderParam.RangeThresholdBottom + barcodeReaderParam.RangeThresholdTop + region.Height;

            RotatedRect searchRangeRect = new RotatedRect(region.Left - barcodeReaderParam.RangeThresholdLeft, region.Top - barcodeReaderParam.RangeThresholdTop, rectWidth, rectHeight, region.Angle);
            
            Pen pen = new Pen(Color.Purple, 1.0F);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            RectangleFigure figure = new RectangleFigure(searchRangeRect, pen);

            figure.Selectable = false;
            figureGroup.AddFigure(figure);
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)param;

            if (barcodeReaderParam.SearchRangeWidth != 0 || barcodeReaderParam.SearchRangeHeight != 0)
            {
                inspRegion.Inflate(barcodeReaderParam.SearchRangeWidth, barcodeReaderParam.SearchRangeHeight);
            }
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)param;

            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("DesiredString", barcodeReaderParam.DesiredString));
            resultValues.Add(new AlgorithmResultValue("BarcodePositionList", null));

            if (barcodeReaderParam.OffsetRange == true)
            {
                resultValues.Add(new AlgorithmResultValue("XPosGap", 100, 0, 0));
                resultValues.Add(new AlgorithmResultValue("YPosGap", 100, 0, 0));
            }

            return resultValues;
        }

        public override string[] GetPreviewNames()
        {
            return new string[] { "Default" };
        }

        public override ImageD Filter(ImageD image, int previewFilterType)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - BarcodeReader_Filter");

            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)param;

            AlgoImage algoImage = ImageBuilder.Build(GetAlgorithmType(), image, ImageType.Grey);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            switch (previewFilterType)
            {
                case 0:
                    imageProcessing.BinarizeHistogram(algoImage, algoImage, barcodeReaderParam.ThresholdPercent);
                    
                    imageProcessing.Not(algoImage, algoImage);
                    imageProcessing.Dilate(algoImage, barcodeReaderParam.CloseNum);
                    imageProcessing.Erode(algoImage, barcodeReaderParam.CloseNum);
                    break;
            }

            return algoImage.ToImageD();
        }

        public AlgorithmResult Inspect(ImageD probeClipImage, Size wholeImageSize, DebugContext debugContext)
        {
            RotatedRect probeRegionInFov = new RotatedRect(0, 0, probeClipImage.Width, probeClipImage.Height, 0);
            RotatedRect imageRegionInFov = new RotatedRect(0, 0, probeClipImage.Width, probeClipImage.Height, 0);

            return Inspect(new AlgorithmInspectParam(probeClipImage, probeRegionInFov, imageRegionInFov, wholeImageSize, null, debugContext));
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage probeClipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.ClipImage, ImageType.Grey);
            Filter(probeClipImage);

            BarcodeReaderParam barcodeReaderParam = (BarcodeReaderParam)param;

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect clipRegionInFov = inspectParam.ClipRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(clipRegionInFov, probeRegionInFov);

            AlgorithmResult barcodeReaderResult = Read(probeClipImage, probeRegionInClip, inspectParam.DebugContext);

            string desiredString = barcodeReaderParam.DesiredString;
            barcodeReaderResult.ResultRect = inspectParam.ProbeRegionInFov;
            barcodeReaderResult.ResultValueList.Add(new AlgorithmResultValue("DesiredString", desiredString));

            AlgorithmResultValue barcodePositionListResult = barcodeReaderResult.GetResultValue("BarcodePositionList");
            if (barcodePositionListResult == null)
                return barcodeReaderResult;

            BarcodePositionList barcodePositionList = (BarcodePositionList)barcodePositionListResult.Value;

            int barcodeNum = barcodePositionList.Items.Count();

            Size whileImageSize = inspectParam.WholeImageSize;

            SizeF offset = new SizeF(0, 0);
            float angle = 0;

            if (barcodeNum > 0)
            {
                barcodeReaderResult.Good = true;

                if (String.IsNullOrEmpty(desiredString) != true)
                {
                    foreach (BarcodePosition barcodePosition in barcodePositionList.Items)
                    {
                        barcodePosition.FoundPosition = DrawingHelper.ClipToFov(clipRegionInFov, barcodePosition.FoundPosition);
                        barcodeReaderResult.ResultFigureGroup.AddFigure(new PolygonFigure(barcodePosition.FoundPosition, true, new Pen(Color.Lime, 3)));

                        barcodePosition.Good = (barcodePosition.StringRead.IndexOf(desiredString) > -1);
                        if (barcodePosition.Good == false)
                            barcodeReaderResult.Good = false;
                    }
                }

                if (barcodeReaderParam.DesiredNum > 0)
                {
                    barcodeReaderResult.Good = (barcodeReaderParam.DesiredNum == barcodeNum) && barcodeReaderResult.Good;
                }

                RectangleF boundingBox = DrawingHelper.GetBoundRect(barcodePositionList.Items[0].FoundPosition.ToArray());

                if (barcodeNum == 1)
                {
                    BarcodePosition position = barcodePositionList.Items[0];
                    lastReadString = barcodePositionList.Items[0].StringRead;
                    angle = position.FoundAngle;

                    PointF probePosInFov = DrawingHelper.CenterPoint(probeRegionInFov);
                    PointF foundPosInFov = DrawingHelper.ClipToFov(clipRegionInFov, DrawingHelper.CenterPoint(boundingBox));

                    offset.Width = foundPosInFov.X - probePosInFov.X;
                    offset.Height = foundPosInFov.Y - probePosInFov.Y;

                    barcodeReaderResult.OffsetFound = offset;
                    barcodeReaderResult.AngleFound = angle;

                    if (inspectParam.CameraCalibration != null)
                    {
                        PointF realProbeCenter = inspectParam.CameraCalibration.PixelToWorld(probePosInFov);
                        PointF realFoundPos = inspectParam.CameraCalibration.PixelToWorld(foundPosInFov);

                        barcodeReaderResult.RealOffsetFound = new SizeF(realFoundPos.X - realProbeCenter.X, realFoundPos.Y - realProbeCenter.Y);
                    }

                    if (barcodeReaderParam.OffsetRange == true)
                    {
                        barcodeReaderResult.ResultValueList.Add(new AlgorithmResultValue("XPosGap", offset.Width));
                        barcodeReaderResult.ResultValueList.Add(new AlgorithmResultValue("YPosGap", offset.Height));
                        
                        float leftLimit = inspectParam.ProbeRegionInFov.Left - barcodeReaderParam.RangeThresholdLeft;
                        float RightLimit = inspectParam.ProbeRegionInFov.Right + barcodeReaderParam.RangeThresholdRight;
                        float bottomLimit = inspectParam.ProbeRegionInFov.Bottom + barcodeReaderParam.RangeThresholdBottom;
                        float topLimit = inspectParam.ProbeRegionInFov.Top - barcodeReaderParam.RangeThresholdTop;

                        barcodeReaderResult.Good = position.FoundPosition[0].X > leftLimit && position.FoundPosition[2].X < RightLimit && position.FoundPosition[0].Y > topLimit && position.FoundPosition[2].Y < bottomLimit && barcodeReaderResult.Good;
                        
                        if (barcodeReaderResult.Good == true)
                            barcodeReaderResult.ResultFigureGroup.AddFigure(new LineFigure(probePosInFov, foundPosInFov, new Pen(Color.Blue)));
                        else
                            barcodeReaderResult.ResultFigureGroup.AddFigure(new LineFigure(probePosInFov, foundPosInFov, new Pen(Color.Red)));
                    }
                }
            }

            return barcodeReaderResult;
        }

        public abstract AlgorithmResult Read(AlgoImage clipImage, RectangleF clipRect, DebugContext debugContext);
    }

    public class BarcodePositionList : ResultValueItem
    {
        List<BarcodePosition> items = new List<BarcodePosition>();
        public List<BarcodePosition> Items
        {
            get { return items; }
            set { items = value; }
        }

        public BarcodePosition GetBarcodePosition(string barcode)
        {
            foreach (BarcodePosition position in items)
            {
                if (position.StringRead == barcode)
                    return position;
            }

            return null;
        }

        public List<BarcodePosition> GetDuplicatePosition(string barcode)
        {
            List<BarcodePosition> barcodeDuplicateList = new List<BarcodePosition>();
            foreach (BarcodePosition position in items)
            {
                if (position.StringRead == barcode)
                {
                    barcodeDuplicateList.Add(position);
                }
            }
            return barcodeDuplicateList;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (BarcodePosition position in items)
            {
                stringBuilder.AppendLine(position.ToString());
            }

            return stringBuilder.ToString();
        }

        public override string GetValueString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (BarcodePosition position in items)
            {
                stringBuilder.Append(position.StringRead);
                stringBuilder.Append("/");
            }

            return stringBuilder.ToString();
        }
    }

    public class BarcodePosition
    {
        bool good;
        public bool Good
        {
            get { return good; }
            set { good = value; }
        }

        string stringRead;
        public string StringRead
        {
            get { return stringRead; }
            set { stringRead = value; }
        }

        List<PointF> foundPosition = new List<PointF>();
        public List<PointF> FoundPosition
        {
            get { return foundPosition; }
            set { foundPosition = value; }
        }

        float foundAngle = new float();
        public float FoundAngle
        {
            get { return foundAngle; }
            set { foundAngle = value; }
        }

        int area;
        public int Area
        {
            get { return area; }
            set { area = value; }
        }
            

        public override string ToString()
        {
            PointF centerPt = DrawingHelper.CenterPoint(foundPosition.ToArray());
            return String.Format("Barcode : {0} ( Pos : {1} / Angle : {2} / Area : {3} ) ", stringRead, centerPt.ToString(), foundAngle, area);
        }
    }
}
