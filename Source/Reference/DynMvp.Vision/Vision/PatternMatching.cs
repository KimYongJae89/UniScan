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
    public class PatternMatchingParamConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PatternMatchingParam))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is PatternMatchingParam)
            {
                PatternMatchingParam patternMatchingParam = (PatternMatchingParam)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverterAttribute(typeof(PatternMatchingParamConverter))]
    public class PatternMatchingParam : AlgorithmParam
    {
        /// <summary>
        /// 0-Very Low
        /// 1-Low
        /// 2-Medium (default)
        /// 3-high
        /// 4-Very High
        /// </summary>
        public int SpeedType { get => speedType; set => this.speedType = value; }
        int speedType = -1;

        private float minAngle;
        public float MinAngle
        {
            get { return minAngle; }
            set { minAngle = value; }
        }

        private float maxAngle;
        public float MaxAngle
        {
            get { return maxAngle; }
            set { maxAngle = value; }
        }

        private float minScale;
        public float MinScale
        {
            get { return minScale; }
            set { minScale = value; }
        }

        private float maxScale;
        public float MaxScale
        {
            get { return maxScale; }
            set { maxScale = value; }
        }

        private bool ignorePolarity;
        public bool IgnorePolarity
        {
            get { return ignorePolarity; }
            set { ignorePolarity = value; }
        }

        private int searchRangeWidth;
        public int SearchRangeWidth
        {
            get { return searchRangeWidth; }
            set { searchRangeWidth = value; }
        }

        private int searchRangeHeight;
        public int SearchRangeHeight
        {
            get { return searchRangeHeight; }
            set { searchRangeHeight = value; }
        }

        private int matchScore;
        public int MatchScore
        {
            get { return matchScore; }
            set { matchScore = value; }
        }

        private int numToFind;
        public int NumToFind
        {
            get { return numToFind; }
            set { numToFind = value; }
        }

        bool useImageCenter;
        public bool UseImageCenter
        {
            get { return useImageCenter; }
            set { useImageCenter = value; }
        }

        bool useWholeImage;
        public bool UseWholeImage
        {
            get { return useWholeImage; }
            set { useWholeImage = value; }
        }

        bool useAngle;
        public bool UseAngle
        {
            get { return useAngle; }
            set { useAngle = value; }
        }

        private string recogString;
        public string RecogString
        {
            get { return recogString; }
            set { recogString = value; }
        }

        private List<Pattern> patternList = new List<Pattern>();
        [BrowsableAttribute(false)]
        public List<Pattern> PatternList
        {
            get { return patternList; }
            set { patternList = value; }
        }

        public PatternMatchingParam()
        {
            minAngle = -30.0f;
            maxAngle = 30.0f;
            minScale = 0.96f;
            maxScale = 1.04f;
            ignorePolarity = false;
            matchScore = 50;
            searchRangeWidth = 100;
            searchRangeHeight = 100;
            numToFind = 1;
            recogString = "";
            useWholeImage = false;
            useImageCenter = false;
            useAngle = false;
        }

        public override void Clear()
        {
            base.Clear();
            foreach (Pattern pattern in patternList)
                pattern.Dispose();
        }

        public override AlgorithmParam Clone()
        {
            PatternMatchingParam param = new PatternMatchingParam();

            param.CopyFrom(this);
            
            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            PatternMatchingParam param = (PatternMatchingParam)srcAlgorithmParam;

            minAngle = param.minAngle;
            maxAngle = param.maxAngle;
            minScale = param.minScale;
            maxScale = param.maxScale;
            ignorePolarity = param.ignorePolarity;
            matchScore = param.matchScore;
            searchRangeWidth = param.searchRangeWidth;
            searchRangeHeight = param.searchRangeHeight;
            numToFind = param.NumToFind;
            recogString = param.RecogString;
            useWholeImage = param.UseWholeImage;
            useImageCenter = param.useImageCenter;
            useAngle = param.useAngle;

            foreach (Pattern pattern in param.PatternList)
                AddPattern((Image2D)pattern.PatternImage.Clone());


            //patternList.Add(pattern.Clone());
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            minAngle = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MinAngle", "-5"));
            maxAngle = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MaxAngle", "5"));
            minScale = (float)Convert.ToDouble(XmlHelper.GetValue(algorithmElement, "MinScale", "0.95"));
            maxScale = (float)Convert.ToDouble(XmlHelper.GetValue(algorithmElement, "MaxScale", "1.05"));
            searchRangeWidth = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeWidth", "30"));
            searchRangeHeight = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeHeight", "30"));
            matchScore = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MatchScore", "50"));
            ignorePolarity = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "IgnorePolarity", "False"));
            recogString = XmlHelper.GetValue(algorithmElement, "RecogString", "");
            useWholeImage = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "WholeImage", "False"));
            useImageCenter = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseImageCenter", "False"));
            useAngle = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseAngle", "False"));

            foreach (XmlElement patternElement in algorithmElement)
            {
                if (patternElement.Name == "Pattern")
                {
                    Pattern pattern = AlgorithmBuilder.CreatePattern();

                    pattern.SetPatternImageString(XmlHelper.GetValue(patternElement, "Image", ""));
                    pattern.PatternType = (PatternType)Enum.Parse(typeof(PatternType), XmlHelper.GetValue(patternElement, "PatternType", "Good"));

                    pattern.Train(pattern.PatternImage, this);

                    foreach (XmlElement maskFiguresElement in patternElement)
                    {
                        if (maskFiguresElement.Name == "MaskFigures")
                        {
                            pattern.MaskFigures.Load(maskFiguresElement);

                            pattern.UpdateMaskImage();

                            break;
                        }
                    }

                    AddPattern(pattern);
                    //patternList.Add(pattern);
                }
            }                
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "MinAngle", minAngle.ToString());
            XmlHelper.SetValue(algorithmElement, "MaxAngle", maxAngle.ToString());
            XmlHelper.SetValue(algorithmElement, "MinScale", minScale.ToString());
            XmlHelper.SetValue(algorithmElement, "MaxScale", maxScale.ToString());
            XmlHelper.SetValue(algorithmElement, "IgnorePolarity", ignorePolarity.ToString());
            XmlHelper.SetValue(algorithmElement, "SearchRangeWidth", searchRangeWidth.ToString());
            XmlHelper.SetValue(algorithmElement, "SearchRangeHeight", searchRangeHeight.ToString());
            XmlHelper.SetValue(algorithmElement, "MatchScore", matchScore.ToString());
            XmlHelper.SetValue(algorithmElement, "RecogString", recogString.ToString());
            XmlHelper.SetValue(algorithmElement, "WholeImage", useWholeImage.ToString());
            XmlHelper.SetValue(algorithmElement, "UseImageCenter", useImageCenter.ToString());
            XmlHelper.SetValue(algorithmElement, "UseAngle", useAngle.ToString());

            foreach (Pattern pattern in patternList)
            {
                XmlElement patternElement = algorithmElement.OwnerDocument.CreateElement("", "Pattern", "");
                algorithmElement.AppendChild(patternElement);

                XmlHelper.SetValue(patternElement, "Image", pattern.GetPatternImageString());
                XmlHelper.SetValue(patternElement, "PatternType", pattern.PatternType.ToString());

                if (pattern.MaskFigures.FigureExist)
                {
                    XmlElement maskFiguresElement = patternElement.OwnerDocument.CreateElement("", "MaskFigures", "");
                    patternElement.AppendChild(maskFiguresElement);

                    pattern.MaskFigures.Save(maskFiguresElement);
                }
            }
        }

        public Pattern AddPattern(Image2D image2d)
        {
            Pattern pattern = AlgorithmBuilder.CreatePattern();
            pattern.Train(image2d, this);

            patternList.Insert(0, pattern);

            return pattern;
        }

        public void AddPattern(Pattern pattern)
        {
            patternList.Add(pattern);
        }

        public void RemovePattern(Pattern pattern)
        {
            pattern.Dispose();
            patternList.Remove(pattern);
        }

        public void RemovePattern(int index)
        {
            patternList[index].Dispose();
            patternList.RemoveAt(index);
        }

        public void RemoveAllPatterns()
        {
            foreach (Pattern pattern in patternList)
                pattern.Dispose();

            patternList.Clear();
        }

        public Pattern GetPattern(int index)
        {
            return patternList[index];
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class PatternMatchingConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(PatternMatching))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is PatternMatching)
            {

                PatternMatching patternMatching = (PatternMatching)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverterAttribute(typeof(PatternMatchingConverter))]
    public class PatternMatching : Algorithm, Searchable
    {
        public PatternMatching()
        {
            param = new PatternMatchingParam();
        }

        public override void Clear()
        {
            ((PatternMatchingParam)param).Clear();
        }

        public override Algorithm Clone()
        {
            PatternMatching patternMatching = new PatternMatching();
            patternMatching.CopyFrom(this);

            return patternMatching;
        }

        public override void CopyFrom(Algorithm algorithm)
        {
            base.CopyFrom(algorithm);

            PatternMatching patternMatching = (PatternMatching)algorithm;

            param.CopyFrom(patternMatching.param);
        }

        public static string TypeName
        {
            get { return "PatternMatching"; }
        }

        public bool fCalibrated { get; private set; }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "PatMat";
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {

        }

        public Size GetSearchRangeSize()
        {
            PatternMatchingParam patternMatchingParam = (PatternMatchingParam)param;

            return new Size(patternMatchingParam.SearchRangeWidth, patternMatchingParam.SearchRangeHeight);
        }

        public void SetSearchRangeSize(Size searchRange)
        {
            PatternMatchingParam patternMatchingParam = (PatternMatchingParam)param;
            patternMatchingParam.SearchRangeWidth = searchRange.Width;
            patternMatchingParam.SearchRangeHeight = searchRange.Height;
        }

        public Pattern AddPattern(Image2D image2d)
        {
            PatternMatchingParam patternMatchingParam = (PatternMatchingParam)param;

            return patternMatchingParam.AddPattern(image2d);
        }

        public void RemovePattern(Pattern pattern)
        {
            PatternMatchingParam patternMatchingParam = (PatternMatchingParam)param;

            patternMatchingParam.RemovePattern(pattern);
        }

        public void RemoveAllPatterns()
        {
            PatternMatchingParam patternMatchingParam = (PatternMatchingParam)param;

            patternMatchingParam.RemoveAllPatterns();
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            PatternMatchingParam patternMatchingParam = (PatternMatchingParam)param;

            if (patternMatchingParam.SearchRangeWidth == 0 || patternMatchingParam.SearchRangeHeight == 0)
            {
                inspRegion = new RotatedRect(0, 0, 0, 0, 0);
            }
            else
            {
                // 패턴 이미지 사이즈까지 이미지 늘림
                inspRegion.Inflate(patternMatchingParam.SearchRangeWidth, patternMatchingParam.SearchRangeHeight);
            }

            useWholeImage = patternMatchingParam.UseWholeImage;
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            PatternMatchingParam patternMatchingParam = (PatternMatchingParam)param;

            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("Matching Ratio", 100, patternMatchingParam.MatchScore, 0));
            resultValues.Add(new AlgorithmResultValue("Matchint Pos", 100, patternMatchingParam.MatchScore, 0));
            resultValues.Add(new AlgorithmResultValue("Offset X", patternMatchingParam.SearchRangeWidth, 0, 0));
            resultValues.Add(new AlgorithmResultValue("Offset Y", patternMatchingParam.SearchRangeHeight, 0, 0));
            resultValues.Add(new AlgorithmResultValue("RealOffset", new SizeF(0, 0)));

            if (patternMatchingParam.RecogString != "")
                resultValues.Add(new AlgorithmResultValue("String Read", patternMatchingParam.RecogString));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.ClipImage, ImageType.Grey, param.ImageBand);
            Filter(clipImage);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect clipRegionInFov = inspectParam.ClipRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;
            Calibration cameraCalibration = inspectParam.CameraCalibration;
            Size wholeImageSize = inspectParam.WholeImageSize;

            AlgorithmResult pmResult = CreateAlgorithmResult();
            PatternMatchingParam patternMatchingParam = (PatternMatchingParam)param;

            MatchPos maxMatchPos = null;

            foreach (Pattern pattern in patternMatchingParam.PatternList)
            {
                PatternResult pmSubResult = pattern.Inspect(clipImage, patternMatchingParam, debugContext);
                pmResult.AddSubResult(pmSubResult);

                MatchPos matchPos = pmSubResult.MaxMatchPos;
                pmSubResult.Good = ((matchPos.Score * 100) >= patternMatchingParam.MatchScore);

                if (maxMatchPos == null && pmSubResult.Good == true)
                {
                    pmResult.Good = true;
                    maxMatchPos = matchPos;
                }

                if (inspectParam.TeachMode == false)
                    break;
            }

            if (maxMatchPos == null)
                maxMatchPos = new MatchPos();

            PointF refPosInFov;
            PointF foundPosInFov;
            if (patternMatchingParam.UseWholeImage)
                refPosInFov = new PointF(wholeImageSize.Width / 2, wholeImageSize.Height / 2);
            else
                refPosInFov = DrawingHelper.CenterPoint(probeRegionInFov);

            SizeF realOffset = new SizeF(0, 0);
            SizeF offset = new SizeF(0, 0);

            float angle = 0;

            if (pmResult.Good == true && maxMatchPos.Pos != new PointF(0, 0))
            {
                foundPosInFov = DrawingHelper.ClipToFov(clipRegionInFov, maxMatchPos.Pos);

                angle = maxMatchPos.Angle;
                offset = new SizeF(foundPosInFov.X - refPosInFov.X, foundPosInFov.Y - refPosInFov.Y);

                angle = angle - probeRegionInFov.Angle;

                if (cameraCalibration != null)
                    pmResult.Calibrated = cameraCalibration.IsCalibrated();

                if (pmResult.Calibrated)
                {
                    PointF realRefPos = cameraCalibration.PixelToWorld(refPosInFov);
                    PointF realFoundPos = cameraCalibration.PixelToWorld(foundPosInFov);

                    realOffset = new SizeF(realFoundPos.X - realRefPos.X, realFoundPos.Y - realRefPos.Y);
                }
                maxMatchPos.Pos = foundPosInFov;
            }

            pmResult.OffsetFound = offset;
            pmResult.AngleFound = angle;
            pmResult.RealOffsetFound = realOffset;

            RotatedRect resultRect = probeRegionInFov;
            resultRect.Offset(offset.Width, offset.Height);
            resultRect.Angle = angle;
            pmResult.ResultRect = resultRect;

            int searchRangeWidth = patternMatchingParam.SearchRangeWidth;
            int searchRangeHeight = patternMatchingParam.SearchRangeHeight;

            if (pmResult.Good)
            {
                if (maxMatchPos.PatternType == PatternType.Ng)
                {
                    pmResult.Good = false;
                }
            }

            if (patternMatchingParam.UseWholeImage == false)
            {
                if (searchRangeWidth == 0 && searchRangeHeight == 0)
                {
                    if (clipRegionInFov.X > resultRect.X || clipRegionInFov.Y > resultRect.Y ||
                            (clipRegionInFov.X + clipRegionInFov.Width) < (resultRect.X + resultRect.Width) ||
                            (clipRegionInFov.Y + clipRegionInFov.Height) < (resultRect.Y + resultRect.Height))
                    {
                        pmResult.Good = false;
                    }

                    searchRangeWidth = (int)(clipRegionInFov.Width / 2);
                    searchRangeHeight = (int)(clipRegionInFov.Height / 2);
                }
                else
                {
                    if ((Math.Abs(offset.Width) > searchRangeWidth) || (Math.Abs(offset.Height) > searchRangeHeight))
                        pmResult.Good = false;
                }
            }

            pmResult.ResultValueList.Add(new AlgorithmResultValue("Matching Score", 100, patternMatchingParam.MatchScore, (float)maxMatchPos.Score * 100));
            pmResult.ResultValueList.Add(new AlgorithmResultValue("Matching Pos", maxMatchPos.Pos));
            pmResult.ResultValueList.Add(new AlgorithmResultValue("Offset X", searchRangeWidth, 0, offset.Width));
            pmResult.ResultValueList.Add(new AlgorithmResultValue("Offset Y", searchRangeHeight, 0, offset.Height));
            pmResult.ResultValueList.Add(new AlgorithmResultValue("RealOffset", pmResult.RealOffsetFound));

            if (pmResult.Good && patternMatchingParam.RecogString != "")
            {
                pmResult.ResultValueList.Add(new AlgorithmResultValue("String Read", patternMatchingParam.RecogString));
            }

            clipImage.Dispose();
            return pmResult;
        }

        public override void BuildMessage(AlgorithmResult algorithmResult)
        {
            MessageBuilder resultMessage = algorithmResult.MessageBuilder;

            PointF matchingPos = (PointF)algorithmResult.GetResultValue("Matching Pos").Value;
            float matchingScore = (float)algorithmResult.GetResultValue("Matching Score").Value;

            resultMessage.BeginTable(null, "Item", "Value");

            resultMessage.AddTableRow("Result", (algorithmResult.Good?"Good":"NG"));
            resultMessage.AddTableRow("Matching Score", matchingScore.ToString());
            resultMessage.AddTableRow("Matching Pos", matchingPos.ToString());
            resultMessage.AddTableRow("Offset", String.Format("({0:0.00}, {1:0.00})", algorithmResult.OffsetFound.Width, algorithmResult.OffsetFound.Height));

            if (algorithmResult.Calibrated)
                resultMessage.AddTableRow("RealOffset", String.Format("({0:0.00}, {1:0.00})", algorithmResult.RealOffsetFound.Width, algorithmResult.RealOffsetFound.Height));

            resultMessage.EndTable();

            resultMessage.BeginTable(null, "No", "Type", "Score", "Found");

            for (int i = 0; i < algorithmResult.SubResultList.Count; i++)
            {
                PatternResult patternResult = (PatternResult)algorithmResult.SubResultList[i];
                resultMessage.AddTableRow(i.ToString(), (patternResult.BadImage ? "B" : "G"), (patternResult.MaxScore * 100).ToString("0.00"), (patternResult.Good?"OK":""));
            }

            resultMessage.EndTable();

            algorithmResult.ShortResultMessage = String.Format("Matching Score : {0}", matchingScore.ToString());
        }
    }
}
