using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DynMvp.UI;
using DynMvp.Base;
using System.Drawing;
using System.Drawing.Imaging;

namespace DynMvp.Vision
{
    public enum ColorCheckerType
    {
        Average, Segmentation
    }

    public class ColorCheckerParam : AlgorithmParam
    {
        ColorCheckerType type = ColorCheckerType.Average;
        public ColorCheckerType Type
        {
            get { return type; }
            set { type = value; }
        }

        List<ColorValue> colorValueList = new List<ColorValue>();
        public List<ColorValue> ColorValueList
        {
            get { return colorValueList; }
        }

        int acceptanceScore = 80;
        public int AcceptanceScore
        {
            get { return acceptanceScore; }
            set { acceptanceScore = value; }
        }

        float scoreWeightValue1 = 1;
        public float ScoreWeightValue1
        {
            get { return scoreWeightValue1; }
            set { scoreWeightValue1 = value; }
        }

        float scoreWeightValue2 = 1;
        public float ScoreWeightValue2
        {
            get { return scoreWeightValue2; }
            set { scoreWeightValue2 = value; }
        }

        float scoreWeightValue3 = 1;
        public float ScoreWeightValue3
        {
            get { return scoreWeightValue3; }
            set { scoreWeightValue3 = value; }
        }

        ColorSpace colorSpace = ColorSpace.RGB;
        public ColorSpace ColorSpace
        {
            get { return colorSpace; }
            set { colorSpace = value; }
        }

        GridParam gridParam = new GridParam();
        public GridParam GridParam
        {
            get { return gridParam; }
        }

        bool useSegmentation;
        public bool UseSegmentation
        {
            get { return useSegmentation; }
            set { useSegmentation = value; }
        }

        int segmentScore;
        public int SegmentScore
        {
            get { return segmentScore; }
            set { segmentScore = value; }
        }

        SegmentCalcType segmentCalcType;
        public SegmentCalcType SegmentCalcType
        {
            get { return segmentCalcType; }
            set { segmentCalcType = value; }
        }

        public override AlgorithmParam Clone()
        {
            ColorCheckerParam param = new ColorCheckerParam();

            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            ColorCheckerParam param = (ColorCheckerParam)srcAlgorithmParam;

            colorValueList.Clear();
            colorValueList.AddRange(param.colorValueList);

            acceptanceScore = param.acceptanceScore;
            scoreWeightValue1 = param.scoreWeightValue1;
            scoreWeightValue2 = param.scoreWeightValue2;
            scoreWeightValue3 = param.scoreWeightValue3;
            colorSpace = param.colorSpace;
            gridParam = param.gridParam.Clone();

            useSegmentation = param.UseSegmentation;
            segmentCalcType = param.SegmentCalcType;
            segmentScore = param.SegmentScore;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            acceptanceScore = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "AcceptanceSore", "10"));
            scoreWeightValue1 = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "ScoreWeightValue1", "1"));
            scoreWeightValue2 = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "ScoreWeightValue2", "1"));
            scoreWeightValue3 = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "ScoreWeightValue3", "1"));
            colorSpace = (ColorSpace)Enum.Parse(typeof(ColorSpace), XmlHelper.GetValue(algorithmElement, "ColorSpace", "RGB"));

            XmlElement colorValueListElement = algorithmElement["ColorValueList"];
            if (colorValueListElement != null)
            {
                foreach (XmlElement colorValueElement in colorValueListElement)
                {
                    ColorValue colorValue = new ColorValue();
                    if (colorValueElement.Name == "ColorValue")
                    {
                        colorValue.Load(colorValueElement);
                        colorValueList.Add(colorValue);
                    }
                }
            }

            gridParam.LoadParam(algorithmElement);

            useSegmentation = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseSegmentation", "False"));
            segmentCalcType = (SegmentCalcType)Enum.Parse(typeof(SegmentCalcType), XmlHelper.GetValue(algorithmElement, "SegmentCalcType", "Ratio"));
            segmentScore = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SegmentScore", "100"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "AcceptanceSore", acceptanceScore.ToString());
            XmlHelper.SetValue(algorithmElement, "ScoreWeightValue1", scoreWeightValue1.ToString());
            XmlHelper.SetValue(algorithmElement, "ScoreWeightValue2", scoreWeightValue2.ToString());
            XmlHelper.SetValue(algorithmElement, "ScoreWeightValue3", scoreWeightValue3.ToString());
            XmlHelper.SetValue(algorithmElement, "ColorSpace", colorSpace.ToString());

            XmlElement colorValueListElement = algorithmElement.OwnerDocument.CreateElement("", "ColorValueList", "");
            algorithmElement.AppendChild(colorValueListElement);

            foreach (ColorValue colorValue in colorValueList)
            {
                XmlElement colorValueElement = algorithmElement.OwnerDocument.CreateElement("", "ColorValue", "");
                colorValueListElement.AppendChild(colorValueElement);

                colorValue.Save(colorValueElement);
            }

            gridParam.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "UseSegmentation", useSegmentation.ToString());
            XmlHelper.SetValue(algorithmElement, "SegmentCalcType", segmentCalcType.ToString());
            XmlHelper.SetValue(algorithmElement, "SegmentScore", segmentScore.ToString());
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class ColorChecker : Algorithm
    {
        public ColorChecker()
        {
            isColorAlgorithm = true;
            Param = new ColorCheckerParam();
        }

        public override bool CanProcess3dImage()
        {
            return false;
        }

        public override Algorithm Clone()
        {
            ColorChecker colorChecker = new ColorChecker();
            colorChecker.CopyFrom(this);

            return colorChecker;
        }

        public override void CopyFrom(Algorithm algorithm)
        {
            base.CopyFrom(algorithm);

            ColorChecker colorChecker = (ColorChecker)algorithm;

            param = (ColorCheckerParam)colorChecker.Param.Clone();
        }

        public static string TypeName
        {
            get { return "ColorChecker"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Color";
        }

        public override ImageD Filter(ImageD image, int previewFilterTyype)
        {
            if (image is Image3D)
                return image;

            image.SaveImage("D:\\FilterSrc.bmp", ImageFormat.Bmp);

            AlgoImage algoImage = ImageBuilder.Build(GetAlgorithmType(), image, ImageType.Color);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            ColorCheckerParam colorCheckerParam = (ColorCheckerParam)Param;

            switch (previewFilterTyype)
            {
                case 0:
                    if (colorCheckerParam.UseSegmentation == true)
                    {
                        Image2D segmentImage;

                        GetSegmentCount((Image2D)image, out segmentImage);

                        return segmentImage.GetColorImage();
                    }
                    else if (colorCheckerParam.GridParam.UseGrid == true)
                    {
                        int gridCount = colorCheckerParam.GridParam.GetNumCol() * colorCheckerParam.GridParam.GetNumRow();
                        float widthStep = image.Width / colorCheckerParam.GridParam.GetNumCol();
                        float heightStep = image.Height / colorCheckerParam.GridParam.GetNumRow();

                        for (float y = 0; y < image.Height; y += heightStep)
                        {
                            for (float x = 0; x < image.Width; x += widthStep)
                            {
                                Rectangle cellRect = Rectangle.Truncate(new RectangleF(x, y, widthStep, heightStep));

                                Color color = imageProcessing.GetColorAverage(algoImage, cellRect);
                                imageProcessing.Clear(algoImage, cellRect, color);
                            }
                        }
                    }
                    else
                    {
                        Color color = imageProcessing.GetColorAverage(algoImage);
                        imageProcessing.Clear(algoImage, color);
                    }
                    break;
            }

            Image2D fillterredImage = (Image2D)algoImage.ToImageD();
            //fillterredImage.SaveImage("D:\\TestAlgoImage.bmp", ImageFormat.Bmp);

            return fillterredImage.GetColorImage();
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {

        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {

        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            ColorCheckerParam colorCheckerParam = (ColorCheckerParam)Param;

            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();

            if (colorCheckerParam.GridParam.UseGrid)
            {
                if (colorCheckerParam.GridParam.CalcType == SegmentCalcType.Ratio)
                {
                    resultValues.Add(new AlgorithmResultValue("Match Ratio", colorCheckerParam.GridParam.AcceptanceScore, 0, 0));
                }
                else
                {
                    resultValues.Add(new AlgorithmResultValue("Match Count", colorCheckerParam.GridParam.AcceptanceScore, 0, 0));
                }
            }
            else
            {
                resultValues.Add(new AlgorithmResultValue("Match Index", 0, 0, 0));
                resultValues.Add(new AlgorithmResultValue("Match Distance", colorCheckerParam.AcceptanceScore, 0, 0));
            }

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect clipRegionInFov = inspectParam.ClipRegionInFov;

            AlgorithmResult algorithmResult = CreateAlgorithmResult();
            algorithmResult.ResultRect = probeRegionInFov;
            algorithmResult.Good = false;

            if ((inspectParam.ClipImage is Image2D) == false)
            {
                algorithmResult.MessageBuilder.AddTextLine("Invalid Image Type : Image is not 2D image");
                return algorithmResult;
            }

            if (inspectParam.ClipImage.NumBand != 3)
            {
                algorithmResult.MessageBuilder.AddTextLine("Invalid Image Type : Image is not color image");
                return algorithmResult;
            }

            ColorCheckerParam colorCheckerParam = (ColorCheckerParam)Param;

            if (colorCheckerParam.GridParam.UseGrid == true)
            {
                InspectGridCell(inspectParam, algorithmResult);
            }
            else
            {
                if (colorCheckerParam.UseSegmentation == true)
                    InspectSegment((Image2D)inspectParam.ClipImage, algorithmResult);
                else
                    InspectAverage((Image2D)inspectParam.ClipImage, algorithmResult);
            }

            return algorithmResult;
        }

        bool InspectAverage(Image2D srcImage, AlgorithmResult algorithmResult)
        {
            ColorCheckerParam colorCheckerParam = (ColorCheckerParam)Param;

            AlgoImage clipAlgoImage = ImageBuilder.Build(GetAlgorithmType(), srcImage, ImageType.Color);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(clipAlgoImage);

            Color averageColor = imageProcessing.GetColorAverage(clipAlgoImage);

            ColorValue colorValue = new ColorValue(averageColor);
            if (colorCheckerParam.ColorSpace == ColorSpace.HSI)
                colorValue = colorValue.RgbToHsi();

            float matchDistance = 0;
            int matchIndex = GetColorIndex(colorValue, out matchDistance);

            bool result = (matchIndex > -1);
            if (algorithmResult != null)
            {
                algorithmResult.Good = result;
                if (algorithmResult.Good == true)
                {
                    algorithmResult.ResultValueList.Add(new AlgorithmResultValue("Match Index", 0, 0, matchIndex));
                    algorithmResult.ResultValueList.Add(new AlgorithmResultValue("Match Distance", colorCheckerParam.AcceptanceScore, 0, matchDistance));
                }
            }

            return result;
        }

        int GetColorIndex(ColorValue colorValue, out float matchDistance)
        {
            ColorCheckerParam colorCheckerParam = (ColorCheckerParam)Param;

            int index = 0;
            foreach (ColorValue referenceColor in colorCheckerParam.ColorValueList)
            {
                ColorValue referenceColorValue = referenceColor;
                if (colorCheckerParam.ColorSpace == ColorSpace.HSI)
                    referenceColorValue = referenceColor.GetColor(ColorSpace.HSI);

                float diffValue1 = Math.Abs(referenceColorValue.Value1 - colorValue.Value1);
                float diffValue2 = Math.Abs(referenceColorValue.Value2 - colorValue.Value2);
                float diffValue3 = Math.Abs(referenceColorValue.Value3 - colorValue.Value3);

                float distance = (float)Math.Sqrt(diffValue1 * diffValue1 + diffValue2 * diffValue2 + diffValue3 * diffValue3);
                bool result = (distance < colorCheckerParam.AcceptanceScore);

                if (result)
                {
                    matchDistance = distance;
                    return index;
                }

                index++;
            }

            matchDistance = 0;
            return -1;
        }

        int GetSegmentCount(Image2D srcImage, out Image2D segmentImage)
        {
            byte[] data = srcImage.Data;
            ColorCheckerParam colorCheckerParam = (ColorCheckerParam)Param;

            segmentImage = new Image2D(srcImage.Width, srcImage.Height, 1);

            int pixelCount = 0;
            float matchDistance;
            for (int y = 0; y < srcImage.Height; y++)
            {
                for (int x = 0; x < srcImage.Width; x++)
                {
                    byte red = data[y * srcImage.Pitch + x * 3];
                    byte green = data[y * srcImage.Pitch + x * 3 + 1];
                    byte blue = data[y * srcImage.Pitch + x * 3 + 2];

                    ColorValue colorValue = new ColorValue(red, green, blue);
                    if (colorCheckerParam.ColorSpace == ColorSpace.HSI)
                        colorValue = colorValue.GetColor(ColorSpace.HSI);

                    int index = GetColorIndex(colorValue, out matchDistance);
                    if (index > -1)
                    {
                        pixelCount++;
                        segmentImage.Data[y * segmentImage.Width + x] = 255;
                    }
                }
            }

            return pixelCount;
        }

        bool InspectSegment(Image2D srcImage, AlgorithmResult algorithmResult)
        {
            ColorCheckerParam colorCheckerParam = (ColorCheckerParam)Param;

            Image2D segmentImage;
            int segmentCount = GetSegmentCount(srcImage, out segmentImage);

            bool result = false;

            if (algorithmResult != null)
            {
                if (colorCheckerParam.SegmentCalcType == SegmentCalcType.Count)
                {
                    result = (segmentCount > colorCheckerParam.SegmentScore);
                    algorithmResult.ResultValueList.Add(new AlgorithmResultValue("Segment Count", colorCheckerParam.SegmentScore, 0, segmentCount));
                }
                else
                {
                    float area = srcImage.Width * srcImage.Height;
                    float segmentRatio = (segmentCount / area * 100);
                    result = (segmentRatio > colorCheckerParam.SegmentScore);
                    algorithmResult.ResultValueList.Add(new AlgorithmResultValue("Segment Ratio", colorCheckerParam.SegmentScore, 0, segmentRatio));
                    algorithmResult.ResultValueList.Add(new AlgorithmResultValue("Segment Count", 0, 0, segmentCount));
                }

                algorithmResult.Good = result;
            }

            return result;
        }

        void InspectGridCell(AlgorithmInspectParam inspectParam, AlgorithmResult algorithmResult)
        {
            RotatedRect clipRegionInFov = inspectParam.ClipRegionInFov;
            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;

            RectangleF probeRect = DrawingHelper.FovToClip(clipRegionInFov, probeRegionInFov);

            ColorCheckerParam colorCheckerParam = (ColorCheckerParam)Param;

            int gridCount = colorCheckerParam.GridParam.GetNumCol() * colorCheckerParam.GridParam.GetNumRow();
            float widthStep = probeRect.Width / colorCheckerParam.GridParam.GetNumCol();
            float heightStep = probeRect.Height / colorCheckerParam.GridParam.GetNumRow();

            bool result = false;
            int goodCount = 0;
            for (float y = probeRect.Y; y < probeRect.Y + probeRect.Height; y += heightStep)
            {
                for (float x = probeRect.X; x < probeRect.X + probeRect.Width; x += widthStep)
                {
                    RectangleF cellRect = new RectangleF(x, y, widthStep, heightStep);
                    Image2D subClipImage = (Image2D)inspectParam.ClipImage.ClipImage(Rectangle.Truncate(cellRect));

                    if (colorCheckerParam.UseSegmentation == true)
                        result = InspectSegment(subClipImage, null);
                    else
                        result = InspectAverage(subClipImage, null);

                    if (result == true)
                        goodCount++;
                    else
                    {
                        RotatedRect fovCellRect = DrawingHelper.ClipToFov(clipRegionInFov, cellRect);
                        algorithmResult.ResultFigureGroup.AddFigure(new XRectFigure(fovCellRect, new Pen(Color.Red, 1.0F)));
                    }
                }
            }

            int gridAcceptanceScore = colorCheckerParam.GridParam.AcceptanceScore;

            if (colorCheckerParam.GridParam.CalcType == SegmentCalcType.Ratio)
            {
                float matchRatio = (float)goodCount / gridCount * 100;
                algorithmResult.Good = matchRatio > gridAcceptanceScore;
                algorithmResult.ResultValueList.Add(new AlgorithmResultValue("Match Ratio", gridAcceptanceScore, 0, matchRatio));
                algorithmResult.ResultValueList.Add(new AlgorithmResultValue("Match Count", 0, 0, goodCount));
            }
            else
            {
                algorithmResult.Good = goodCount > gridAcceptanceScore;
                algorithmResult.ResultValueList.Add(new AlgorithmResultValue("Match Count", gridAcceptanceScore, 0, goodCount));
            }
        }
    }
}
