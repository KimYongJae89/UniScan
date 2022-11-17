using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.ComponentModel;
using System.Globalization;

using DynMvp.Base;
using DynMvp.UI;

namespace DynMvp.Vision
{
    public class BinaryCounterConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(BinaryCounter))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is BinaryCounter)
            {
                BinaryCounter binaryCounter = (BinaryCounter)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverterAttribute(typeof(BinaryCounterConverter))]
    public class BinaryCounterParam : AlgorithmParam
    {
        private bool countWhitePixel = true;
        public bool CountWhitePixel
        {
            get { return countWhitePixel; }
            set { countWhitePixel = value; }
        }

        private bool countBlackPixel = false;
        public bool CountBlackPixel
        {
            get { return countBlackPixel; }
            set { countBlackPixel = value; }
        }

        private bool countGreyPixel = false;
        public bool CountGreyPixel
        {
            get { return countGreyPixel; }
            set { countGreyPixel = value; }
        }

        private int maxPixelRatio = 100;
        public int MaxPixelRatio
        {
            get { return maxPixelRatio; }
            set { maxPixelRatio = value; }
        }

        private int minPixelRatio = 50;
        public int MinPixelRatio
        {
            get { return minPixelRatio; }
            set { minPixelRatio = value; }
        }

        private bool tapeInspection;
        public bool TapeInspection
        {
            get { return tapeInspection; }
            set { tapeInspection = value; }
        }

        GridParam gridParam = new GridParam();
        public GridParam GridParam
        {
            get { return gridParam; }
        }

        public override AlgorithmParam Clone()
        {
            BinaryCounterParam param = new BinaryCounterParam();

            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            BinaryCounterParam param = (BinaryCounterParam)srcAlgorithmParam;

            countWhitePixel = param.countWhitePixel;
            countBlackPixel = param.countBlackPixel;
            countGreyPixel = param.countGreyPixel;
            maxPixelRatio = param.maxPixelRatio;
            minPixelRatio = param.minPixelRatio;
            tapeInspection = param.tapeInspection;
            gridParam = param.gridParam.Clone();
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            countWhitePixel = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "CountWhitePixel", "True"));
            countBlackPixel = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "CountBlackPixel", "False"));
            countGreyPixel = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "CountGreyPixel", "False"));
            maxPixelRatio = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MaxPixelRatio", "100"));
            minPixelRatio = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MinPixelRatio", "50"));
            if (XmlHelper.Exist(algorithmElement, "BoltInspection"))
                tapeInspection = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "BoltInspection", "False"));
            else
                tapeInspection = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "TapeInspection", "False"));

            gridParam.LoadParam(algorithmElement);
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "CountWhitePixel", countWhitePixel.ToString());
            XmlHelper.SetValue(algorithmElement, "CountBlackPixel", countBlackPixel.ToString());
            XmlHelper.SetValue(algorithmElement, "CountGreyPixel", countGreyPixel.ToString());
            XmlHelper.SetValue(algorithmElement, "MaxPixelRatio", maxPixelRatio.ToString());
            XmlHelper.SetValue(algorithmElement, "MinPixelRatio", minPixelRatio.ToString());
            XmlHelper.SetValue(algorithmElement, "TapeInspection", tapeInspection.ToString());

            gridParam.SaveParam(algorithmElement);
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    [TypeConverterAttribute(typeof(BinaryCounterConverter))]
    public class BinaryCounter : Algorithm
    {
        public BinaryCounter()
        {
            param = new BinaryCounterParam();
            FilterList.Add(new BinarizeFilter(BinarizationType.SingleThreshold, 100));
        }

        public override bool CanProcess3dImage()
        {
            return false;
        }

        public override Algorithm Clone()
        {
            BinaryCounter binaryCounter = new BinaryCounter();
            binaryCounter.CopyFrom(this);

            return binaryCounter;
        }

        public override void CopyFrom(Algorithm algorithm)
        {
            base.CopyFrom(algorithm);

            BinaryCounter binaryCounter = (BinaryCounter)algorithm;

            param = (BinaryCounterParam)binaryCounter.Param.Clone();
        }

        public static string TypeName
        {
            get { return "BinaryCounter"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Binary";
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {

        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {

        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            BinaryCounterParam binaryCounterParam = (BinaryCounterParam)param;

            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("Pixel Ratio", binaryCounterParam.MaxPixelRatio, binaryCounterParam.MinPixelRatio, 0));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            ImageType imageType = (inspectParam.ClipImage is Image3D ? ImageType.Depth : ImageType.Grey);

            AlgoImage probeClipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.ClipImage, imageType, param.ImageBand);
            Filter(probeClipImage);

            DebugHelper.SaveImage(probeClipImage, "ProcImage.bmp", inspectParam.DebugContext);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(probeClipImage);

            BinaryCounterParam binaryCounterParam = (BinaryCounterParam)param;

            if (binaryCounterParam.GridParam.UseGrid)
                return InspectGridCell(inspectParam, probeClipImage);

            int numWhitePixel = 0;
            int numBlackPixel = 0;
            int numGreyPixel = 0;

            if (binaryCounterParam.TapeInspection)
            {
                if (inspectParam.ProbeRegionInFov.Angle != 0)
                {
                    ImageD rotatedMask = ImageHelper.GetRotateMask(inspectParam.ClipImage.Width, inspectParam.ClipImage.Height, inspectParam.ProbeRegionInFov);
                    AlgoImage rotatedAlgoMask = ImageBuilder.Build(BinaryCounter.TypeName, rotatedMask, ImageType.Grey, ImageBandType.Luminance);
                    imageProcessing.Count2(probeClipImage, rotatedAlgoMask, out numBlackPixel, out numGreyPixel, out numWhitePixel);
                }
                else
                {
                    imageProcessing.Count2(probeClipImage, out numBlackPixel, out numGreyPixel, out numWhitePixel);
                }
            }
            else
            {
                if (inspectParam.ProbeRegionInFov.Angle != 0)
                {
                    ImageD rotatedMask = ImageHelper.GetRotateMask(inspectParam.ClipImage.Width, inspectParam.ClipImage.Height, inspectParam.ProbeRegionInFov);
                    AlgoImage rotatedAlgoMask = ImageBuilder.Build(BinaryCounter.TypeName, rotatedMask, ImageType.Grey, ImageBandType.Luminance);
                    imageProcessing.Count(probeClipImage, rotatedAlgoMask, out numBlackPixel, out numGreyPixel, out numWhitePixel);
                }
                else
                {
                    imageProcessing.Count(probeClipImage, out numBlackPixel, out numGreyPixel, out numWhitePixel);
                }
            }

            int pixelCount = 0;
            if (binaryCounterParam.CountWhitePixel)
                pixelCount += numWhitePixel;
            if (binaryCounterParam.CountGreyPixel)
                pixelCount += numGreyPixel;
            if (binaryCounterParam.CountBlackPixel)
                pixelCount += numBlackPixel;

            int area = numWhitePixel + numGreyPixel + numBlackPixel;
            float pixelRatio = 0;
            if (area > 0)
                pixelRatio = ((float)pixelCount) / area * 100;
            else
                pixelRatio = 0;

            AlgorithmResult binaryCountResult = CreateAlgorithmResult();
            binaryCountResult.ResultRect = inspectParam.ProbeRegionInFov;
            binaryCountResult.Good = (pixelRatio >= binaryCounterParam.MinPixelRatio) && (pixelRatio <= binaryCounterParam.MaxPixelRatio);

            binaryCountResult.ResultValueList.Add(new AlgorithmResultValue("Pixel Ratio", binaryCounterParam.MaxPixelRatio, binaryCounterParam.MinPixelRatio, pixelRatio));

            return binaryCountResult;
        }

        AlgorithmResult InspectGridCell(AlgorithmInspectParam inspectParam, AlgoImage probeClipImage)
        {
            RotatedRect clipRegionInFov = inspectParam.ClipRegionInFov;
            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;

            RectangleF probeRect = DrawingHelper.FovToClip(clipRegionInFov, probeRegionInFov);

            BinaryCounterParam binaryCounterParam = (BinaryCounterParam)Param;
            GridParam gridParam = (GridParam)binaryCounterParam.GridParam;

            int gridCount = gridParam.GetNumCol() * gridParam.GetNumRow();
            float widthStep = probeRect.Width / gridParam.GetNumCol();
            float heightStep = probeRect.Height / gridParam.GetNumRow();

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(probeClipImage);

            AlgorithmResult algorithmResult = CreateAlgorithmResult();

            bool result = false;
            int goodCount = 0;
            for (float y = probeRect.Y; y < probeRect.Y + probeRect.Height; y += heightStep)
            {
                for (float x = probeRect.X; x < probeRect.X + probeRect.Width; x += widthStep)
                {
                    RectangleF cellRect = new RectangleF(x, y, widthStep, heightStep);
                    AlgoImage subClipImage = probeClipImage.Clip(Rectangle.Truncate(cellRect));

                    int numWhitePixel = 0;
                    int numBlackPixel = 0;
                    int numGreyPixel = 0;

                    imageProcessing.Count(subClipImage, out numBlackPixel, out numGreyPixel, out numWhitePixel);

                    int pixelCount = 0;
                    if (binaryCounterParam.CountWhitePixel)
                        pixelCount += numWhitePixel;
                    if (binaryCounterParam.CountGreyPixel)
                        pixelCount += numGreyPixel;
                    if (binaryCounterParam.CountBlackPixel)
                        pixelCount += numBlackPixel;

                    int area = numWhitePixel + numGreyPixel + numBlackPixel;
                    float pixelRatio = 0;
                    if (area > 0)
                        pixelRatio = ((float)pixelCount) / area * 100;
                    else
                        pixelRatio = 0;

                    result = (pixelRatio >= binaryCounterParam.MinPixelRatio) && (pixelRatio <= binaryCounterParam.MaxPixelRatio);
                    if (result == true)
                        goodCount++;
                    else
                    {
                        RotatedRect fovCellRect = DrawingHelper.ClipToFov(clipRegionInFov, cellRect);
                        algorithmResult.ResultFigureGroup.AddFigure(new XRectFigure(fovCellRect, new Pen(Color.Red, 1.0F)));
                    }
                }
            }

            int gridAcceptanceScore = gridParam.AcceptanceScore;

            if (gridParam.CalcType == SegmentCalcType.Ratio)
            {
                float matchRatio = (float)goodCount / gridCount * 100;
                algorithmResult.Good = matchRatio >= gridAcceptanceScore;
                algorithmResult.ResultValueList.Add(new AlgorithmResultValue("Match Ratio", gridAcceptanceScore, 0, matchRatio));
                algorithmResult.ResultValueList.Add(new AlgorithmResultValue("Match Count", 0, 0, goodCount));
            }
            else
            {
                algorithmResult.Good = goodCount >= gridAcceptanceScore;
                algorithmResult.ResultValueList.Add(new AlgorithmResultValue("Match Count", gridAcceptanceScore, 0, goodCount));
            }

            return algorithmResult;
        }
    }
}
