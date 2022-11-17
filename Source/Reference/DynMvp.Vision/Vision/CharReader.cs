using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.IO;
using System.ComponentModel;
using System.Globalization;

using DynMvp.UI;
using DynMvp.Base;

namespace DynMvp.Vision
{
    public enum CharactorPolarity
    {
        DarkOnLight, LightOnDark
    }

    public class CharFont
    {
        ImageD image;
        public ImageD Image
        {
            get { return image; }
            set { image = value; }
        }

        string character;
        public string Character
        {
            get { return character; }
            set { character = value; }
        }

        object algorithmCharObject;

        public object AlgorithmCharObject
        {
            get { return algorithmCharObject; }
            set { algorithmCharObject = value; }
        }

        public CharFont(ImageD image, string character, object algorithmCharObject)
        {
            this.image = image;
            this.character = character;
            this.algorithmCharObject = algorithmCharObject;
        }
    }

    public class CharReaderParamConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(CharReaderParam))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is CharReaderParam)
            {
                CharReaderParam charReaderParam = (CharReaderParam)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverterAttribute(typeof(CharReaderParamConverter))]
    public class CharReaderParam : AlgorithmParam
    {
        int characterMaxHeight;
        public int CharacterMaxHeight
        {
            get { return characterMaxHeight; }
            set { characterMaxHeight = value; }
        }

        int characterMinHeight;
        public int CharacterMinHeight
        {
            get { return characterMinHeight; }
            set { characterMinHeight = value; }
        }

        int characterMaxWidth;
        public int CharacterMaxWidth
        {
            get { return characterMaxWidth; }
            set { characterMaxWidth = value; }
        }

        int characterMinWidth;
        public int CharacterMinWidth
        {
            get { return characterMinWidth; }
            set { characterMinWidth = value; }
        }

        CharactorPolarity charactorPolarity;
        public CharactorPolarity CharactorPolarity
        {
            get { return charactorPolarity; }
            set { charactorPolarity = value; }
        }

        int xOverlapRatio;
        public int XOverlapRatio
        {
            get { return xOverlapRatio; }
            set { xOverlapRatio = value; }
        }

        List<int> thresholdList = new List<int>();
        public List<int> ThresholdList
        {
            get { return thresholdList; }
            set { thresholdList = value; }
        }

        float minScore;
        public float MinScore
        {
            get { return minScore; }
            set { minScore = value; }
        }

        int desiredNumCharacter;
        public int DesiredNumCharacter
        {
            get { return desiredNumCharacter; }
            set { desiredNumCharacter = value; }
        }

        string desiredString = "";
        public string DesiredString
        {
            get { return desiredString; }
            set { desiredString = value; }
        }

        string fontFileName = "";
        public string FontFileName
        {
            get { return fontFileName; }
            set { fontFileName = value; }
        }

        public override AlgorithmParam Clone()
        {
            CharReaderParam param = new CharReaderParam();

            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            CharReaderParam param = (CharReaderParam)srcAlgorithmParam;

            characterMaxHeight = param.characterMaxHeight;
            characterMinHeight = param.characterMinHeight;
            characterMaxWidth = param.characterMaxWidth;
            characterMinWidth = param.characterMinWidth;
            charactorPolarity = param.charactorPolarity;
            desiredNumCharacter = param.desiredNumCharacter;
            desiredString = param.desiredString;
            thresholdList = param.thresholdList;
            fontFileName = param.fontFileName;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            characterMaxHeight = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "CharacterMaxHeight", "0"));
            characterMinHeight = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "CharacterMinHeight", "0"));
            characterMaxWidth = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "CharacterMaxWidth", "0"));
            characterMinWidth = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "CharacterMinWidth", "0"));
            xOverlapRatio = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "XOverlapRatio", "0"));
            charactorPolarity = (CharactorPolarity)Enum.Parse(typeof(CharactorPolarity), XmlHelper.GetValue(algorithmElement, "CharactorPolarity", "DarkOnLight"));
            desiredNumCharacter = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "DesiredNumCharacter", "0"));
            desiredString = XmlHelper.GetValue(algorithmElement, "DesiredString", "");
            fontFileName = XmlHelper.GetValue(algorithmElement, "FontFileName", "");

            foreach (XmlElement thresholdElement in algorithmElement)
            {
                if (thresholdElement.Name == "Threshold")
                {
                    int threshold = Convert.ToInt32(thresholdElement.InnerText);
                    thresholdList.Add(threshold);
                }
            }
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "CharacterMaxHeight", characterMaxHeight.ToString());
            XmlHelper.SetValue(algorithmElement, "CharacterMinHeight", characterMinHeight.ToString());
            XmlHelper.SetValue(algorithmElement, "CharacterMaxWidth", characterMaxWidth.ToString());
            XmlHelper.SetValue(algorithmElement, "CharacterMinWidth", characterMinWidth.ToString());
            XmlHelper.SetValue(algorithmElement, "XOverlapRatio", xOverlapRatio.ToString());
            XmlHelper.SetValue(algorithmElement, "CharactorPolarity", charactorPolarity.ToString());
            XmlHelper.SetValue(algorithmElement, "DesiredNumCharacter", desiredNumCharacter.ToString());
            XmlHelper.SetValue(algorithmElement, "DesiredString", desiredString);
            XmlHelper.SetValue(algorithmElement, "FontFileName", fontFileName);

            foreach (int threshold in thresholdList)
            {
                XmlHelper.SetValue(algorithmElement, "Threshold", threshold.ToString());
            }
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class CharReaderConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(CharReader))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is CharReader)
            {
                CharReader charReader = (CharReader)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverterAttribute(typeof(CharReaderConverter))]
    public abstract class CharReader : Algorithm
    {
        ImagingLibrary imagingLibrary = ImagingLibrary.MatroxMIL;
        public ImagingLibrary ImagingLibrary
        {
            get { return imagingLibrary; }
            set { imagingLibrary = value; }
        }

       
        public CharReader()
        {
            param = new CharReaderParam();
        }

        public override void CopyFrom(Algorithm algorithm)
        {
            base.CopyFrom(algorithm);

            CharReader charReader = (CharReader)algorithm;

            param.CopyFrom(charReader.Param);
        }

        public static string TypeName
        {
            get { return "CharReader"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Char";
        }

        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new CharReaderResult();
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {

        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            param.LoadParam(algorithmElement);

            CharReaderParam charReaderParam = (CharReaderParam)param;

            if (File.Exists(charReaderParam.FontFileName) == true)
            {
                if (imagingLibrary != ImagingLibrary.MatroxMIL)
                    Train(charReaderParam.FontFileName);
            }
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            XmlHelper.SetValue(algorithmElement, "ImagingLibrary", imagingLibrary.ToString());

            param.SaveParam(algorithmElement);
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            inspRegion.Inflate(10, 10);
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            CharReaderParam charReaderParam = (CharReaderParam)param;

            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("String Read", charReaderParam.DesiredString));

            return resultValues;
        }

        public AlgorithmResult Extract(ImageD clipImage, RotatedRect probeRegionInFov, RotatedRect clipRegionInFov, int threshold, DebugContext debugContext)
        {
            AlgoImage clipAlgoImage = ImageBuilder.Build(GetAlgorithmType(), clipImage, ImageType.Grey, param.ImageBand);
#if DEBUG
            clipAlgoImage.Save("Extract_clipAlgoImage.bmp", debugContext);
#endif
            Filter(clipAlgoImage);

            CharReaderResult charReaderResult = null;

            CharReaderParam charReaderParam = (CharReaderParam)param;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(clipRegionInFov, probeRegionInFov);

            if (Trained == true)
            {
                charReaderResult = Extract(clipAlgoImage, probeRegionInClip, threshold, debugContext);
            }
            else
            {
                charReaderResult = new CharReaderResult();
                charReaderResult.ErrorMessage = "Font is not trained";
                charReaderResult.Good = false;

                charReaderResult.ResultValueList.Add(new AlgorithmResultValue("String Read", charReaderParam.DesiredString, charReaderResult.ErrorMessage));
            }
            charReaderResult.ResultRect = probeRegionInFov;
            charReaderResult.OffsetCharPosition(clipRegionInFov.X, clipRegionInFov.Y);

            return charReaderResult;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.ClipImage, ImageType.Grey, param.ImageBand);
            Filter(clipImage);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect clipRegionInFov = inspectParam.ClipRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            CharReaderResult charReaderResult = null;

            CharReaderParam charReaderParam = (CharReaderParam)param;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(clipRegionInFov, probeRegionInFov);

            if (Trained == true)
            {
                charReaderResult = Read(clipImage, probeRegionInClip, debugContext);
            }
            else
            {
                charReaderResult = new CharReaderResult();
                charReaderResult.ErrorMessage = "Font is not trained";
                charReaderResult.Good = false;

                charReaderResult.ResultValueList.Add(new AlgorithmResultValue("String Read", charReaderParam.DesiredString, charReaderResult.ErrorMessage));
            }

            charReaderResult.ResultRect = probeRegionInFov;

            if (charReaderResult.Good == false)
            {
                charReaderResult.ErrorMessage = charReaderResult.ErrorMessage;

                String pathName = Path.Combine(Configuration.TempFolder, "OcrFailed");
                DebugContext ocrDebugContext = new DebugContext(true, pathName);

                string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff");
                clipImage.Save(timeStamp + ".bmp", ocrDebugContext);

                Bitmap clipBmpImage = inspectParam.ClipImage.ToBitmap();
                Bitmap probeImage = ImageHelper.ClipImage(clipBmpImage, probeRegionInClip);
                ImageHelper.SaveImage(probeImage, Path.Combine(pathName, timeStamp + "_0.bmp"));

                clipBmpImage.Dispose();
                probeImage.Dispose();
            }

            charReaderResult.OffsetCharPosition(clipRegionInFov.X, clipRegionInFov.Y);
            
            return charReaderResult;
        }

        public abstract bool Trained { get; }
        public abstract void Train(string fontFileName);
        public abstract CharReaderResult Read(AlgoImage algoImage, RectangleF characterRegion, DebugContext debugContext);
        public abstract CharReaderResult Extract(AlgoImage algoImage, RectangleF characterRegion, int threshold, DebugContext debugContext);

        public abstract void AutoSegmentation(AlgoImage algoImage, RotatedRect rotatedRect, string desiredString);
        public abstract void AddCharactor(CharPosition charPosition, int charactorCode);
        public abstract void AddCharactor(AlgoImage charImage, string charactorCode);
        public abstract void SaveFontFile(string fontFileName);

        public abstract List<CharFont> GetFontList();
        public abstract void RemoveFont(CharFont charFont);

        public abstract int CalibrateFont(AlgoImage charImage, string calibrationString);
    }

    public class CharPosition
    {
        int charCode;
        public int CharCode
        {
            get { return charCode; }
            set { charCode = value; }
        }

        int resultType = 0;
        public int ResultType
        {
            get { return resultType; }
            set { resultType = value; }
        }

        RotatedRect position;
        public RotatedRect Position
        {
            get { return position; }
            set { position = value; }
        }

        public CharPosition(int charCode, RotatedRect position)
        {
            this.charCode = charCode;
            this.position = position;
        }

        public void Offset(float offsetX, float offsetY)
        {
            position.Offset(offsetX, offsetY);
        }
    }

    public class CharReaderResult : AlgorithmResult
    {
        string desiredString;
        public string DesiredString
        {
            get { return desiredString; }
            set { desiredString = value; }
        }

        string stringRead;
        public string StringRead
        {
            get { return stringRead; }
            set { stringRead = value; }
        }

        List<float> scoreList = new List<float>();
        public List<float> ScoreList
        {
            get { return scoreList; }
            set { scoreList = value; }
        }

        string errorMessage = "";
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        List<string> trialResult = new List<string>();
        public List<string> TrialResult
        {
            get { return trialResult; }
            set { trialResult = value; }
        }

        List<CharPosition> charPositionList = new List<CharPosition>();
        public List<CharPosition> CharPositionList
        {
            get { return charPositionList; }
            set { charPositionList = value; }
        }

        public CharReaderResult() : base("CharReader")
        {

        }

        public override string ToString()
        {
            string msg = String.Format("Chracter Result : {0} ", stringRead);
            if (errorMessage != "")
                msg += Environment.NewLine + "    Error Message : "  + errorMessage;

            foreach (string trialStr in trialResult)
            {
                msg += Environment.NewLine +  trialStr;
            }

            msg += Environment.NewLine + String.Format("Desired String : {0} ", desiredString);

            for (int index = 0; index < scoreList.Count; index++)
            {
                msg += Environment.NewLine + String.Format("Char : {0} - Score : {1} ", stringRead[index], scoreList[index]);
            }
                
            return msg;
        }

        public void AddCharPosition(CharPosition charPosition)
        {
            charPositionList.Add(charPosition);
        }

        public void OffsetCharPosition(float offsetX, float offsetY)
        {
            foreach (CharPosition charPosition in charPositionList)
            {
                charPosition.Offset(offsetX, offsetY);
            }
        }

        public override void AppendResultFigures(FigureGroup figureGroup, PointF offset)
        {
            foreach (CharPosition charPosition in charPositionList)
            {
                RotatedRect position = charPosition.Position;
                if (charPosition.ResultType == 1)
                    figureGroup.AddFigure(new RectangleFigure(position, new Pen(Color.Lime)));
                else if (charPosition.ResultType == -1)
                    figureGroup.AddFigure(new RectangleFigure(position, new Pen(Color.Red)));
                else if (charPosition.ResultType == 0)
                    figureGroup.AddFigure(new RectangleFigure(position, new Pen(Color.Yellow)));
            }
        }
    }
}
