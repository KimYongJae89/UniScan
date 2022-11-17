using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.IO;

using DynMvp.Base;
using DynMvp.UI;

namespace DynMvp.Vision
{
    public class ColorPattern
    {
        string name = "";
        public string Name
        {
          get { return name; }
          set { name = value; }
        }

        Image2D image = null;
        public Image2D Image
        {
          get { return image; }
          set { image = value; }
        }

        int matchScore;
        public int MatchScore
        {
            get { return matchScore; }
            set { matchScore = value; }
        }
        int smoothing;
        public int Smoothing
        {
            get { return smoothing; }
            set { smoothing = value; }
        }

        float imageWidth;

        public float ImageWidth
        {
            get { return imageWidth; }
            set { imageWidth = this.image.Width; }
        }

        float imageHeight;

        public float ImageHeight
        {
            get { return imageHeight; }
            set { imageHeight = this.image.Height; }
        }

        public ColorPattern(string name, Image2D image)
        {
            this.name = name;
            this.image = image;
        }

        public ColorPattern(string name, string imageString)
        {
            this.name = name;

            Bitmap bitmap = ImageHelper.Base64StringToBitmap(imageString);
            if (bitmap != null)
            {
                image = Image2D.FromBitmap(bitmap);
                bitmap.Dispose();
            }
        }

        public string GetImageString()
        {
            if (image == null)
                return "";

            Bitmap patternBitmap = image.ToBitmap();
            string patternImageString = ImageHelper.BitmapToBase64String(patternBitmap);
            patternBitmap.Dispose();

            return patternImageString;
        }

        public void SetImageString(string imageString)
        {
            Bitmap patternBitmap = ImageHelper.Base64StringToBitmap(imageString);
            if (patternBitmap != null)
            {
                image = Image2D.FromBitmap(patternBitmap);
                patternBitmap.Dispose();
            }
        }
    }

    public class ColorMatchCheckerParam : AlgorithmParam
    {
        int matchScore;
        public int MatchScore
        {
            get { return matchScore; }
            set { matchScore = value; }
        }

        string matchColor;
        public string MatchColor
        {
            get { return matchColor; }
            set { matchColor = value; }
        }
        int smoothing;
        public int Smoothing
        {
            get { return smoothing; }
            set { smoothing = value;}
        }

        string colorPatternFileName;
        public string ColorPatternFileName
        {
            get { return colorPatternFileName; }
            set { colorPatternFileName = value; }
        }

        bool useColorPatternFile;
        public bool UseColorPatternFile
        {
            get { return useColorPatternFile; }
            set { useColorPatternFile = value; }
        }

        List<ColorPattern> colorPatternList = new List<ColorPattern>();
        public List<ColorPattern> ColorPatternList
        {
            get { return colorPatternList; }
            set { colorPatternList = value; }
        }


        public override AlgorithmParam Clone()
        {
            throw new NotImplementedException();
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            ColorMatchCheckerParam param = (ColorMatchCheckerParam)srcAlgorithmParam;

            matchScore = param.matchScore;
            matchColor = param.matchColor;
            smoothing = param.smoothing;
            colorPatternFileName = param.colorPatternFileName;
            useColorPatternFile = param.useColorPatternFile;
            colorPatternList = param.colorPatternList;
        }

        public ColorPattern AddColorPattern(string name, Image2D image)
        {
            ColorPattern colorPattern = new ColorPattern(name, image);
            colorPattern.Smoothing = smoothing;
            if (colorPattern != null)
                colorPatternList.Add(colorPattern);

            return colorPattern;
        }

        public void DeleteColorPattern(string name)
        {
            colorPatternList.RemoveAll(x => x.Name == name);
        }
        public void RemoveAllColorPattern()
        {
            colorPatternList.Clear();
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            matchScore = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MatchScore", ""));
            smoothing = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "Smoothing", ""));
            matchColor = XmlHelper.GetValue(algorithmElement, "MatchColor", "");
            useColorPatternFile = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseColorPatternFile", ""));
            colorPatternFileName = XmlHelper.GetValue(algorithmElement, "ColorPatternFileName", "");

            if(useColorPatternFile)
            {
                if (File.Exists(colorPatternFileName))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(colorPatternFileName);

                    LoadColorImage(xmlDocument.DocumentElement);
                }
            }
            else
            {
                LoadColorImage(algorithmElement);
            }
        }

        public void LoadColorImage(XmlElement colorPatternListElement)
        {
            foreach (XmlElement colorPatternElement in colorPatternListElement)
            {
                if (colorPatternElement.Name == "ColorPattern")
                {
                    string name = XmlHelper.GetValue(colorPatternElement, "Name", "");
                    string imageString = XmlHelper.GetValue(colorPatternElement, "Image", "");

                    if (name != "" && imageString != "")
                    {
                        ColorPattern colorPattern = new ColorPattern(name, imageString);

                        colorPatternList.Add(colorPattern);
                    }
                }
            }
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "UseColorPatternFile", useColorPatternFile.ToString());
            XmlHelper.SetValue(algorithmElement, "ColorPatternFileName", colorPatternFileName);
            XmlHelper.SetValue(algorithmElement, "MatchScore", matchScore.ToString());
            XmlHelper.SetValue(algorithmElement, "Smoothing", smoothing.ToString());
            XmlHelper.SetValue(algorithmElement, "MatchColor", matchColor.ToString());
            XmlHelper.SetValue(algorithmElement, "ImageSetFileName", matchColor.ToString());
            if (useColorPatternFile == true)
            {
                XmlDocument xmlDocument = new XmlDocument();

                XmlElement colorPatternListElement = xmlDocument.CreateElement("", "ColorPattern", "");
                xmlDocument.AppendChild(colorPatternListElement);

                SaveColorPattern(colorPatternListElement);

                xmlDocument.Save(colorPatternFileName);
            }
            else
            {
                SaveColorPattern(algorithmElement);
            }
        }

        public void SaveColorPattern(XmlElement colorPatternListElement)
        {
            foreach (ColorPattern colorPattern in colorPatternList)
            {
                XmlElement colorPatternElement = colorPatternListElement.OwnerDocument.CreateElement("", "ColorPattern", "");
                colorPatternListElement.AppendChild(colorPatternElement);

                XmlHelper.SetValue(colorPatternElement, "Name", colorPattern.Name);
                XmlHelper.SetValue(colorPatternElement, "Image", colorPattern.GetImageString());

                //colorPatternElement.Save(colorPatternFileName);
            }

            
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class ColorMatchChecker : Algorithm
    {
        public ColorMatchChecker()
        {
            param = new ColorMatchCheckerParam();
            isColorAlgorithm = true;
        }

        public override void CopyFrom(Algorithm algorithm)
        {
            base.CopyFrom(algorithm);

            ColorMatchChecker colorMatchChecker = (ColorMatchChecker)algorithm;

            param.CopyFrom(colorMatchChecker.Param);
        }

        public static string TypeName
        {
            get { return "ColorMatchChecker"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "ColorMatch";
        }

        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new ColorMatchCheckerResult();
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {

        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            param.LoadParam(algorithmElement);

            Train();
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            param.SaveParam(algorithmElement);
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            ColorMatchCheckerParam colorMatchCheckerParam = (ColorMatchCheckerParam)param;

            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("Match Color", colorMatchCheckerParam.MatchColor));
            resultValues.Add(new AlgorithmResultValue("Score", 100, colorMatchCheckerParam.MatchScore, 0));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.ClipImage, ImageType.Color);
            Filter(clipImage);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect clipRegionInFov = inspectParam.ClipRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;
            
            ColorMatchCheckerResult colorMatchCheckerResult = null;
            colorMatchCheckerResult.ResultRect = probeRegionInFov;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(clipRegionInFov, probeRegionInFov);

            ColorMatchCheckerParam colorMatchCheckerParam = (ColorMatchCheckerParam)param;

            if (Trained == true)
            {
                colorMatchCheckerResult = Match(clipImage, probeRegionInClip, debugContext);


                ColorMatchResult maxColorMatchResult = colorMatchCheckerResult.GetMaxColorMatchResult();
                colorMatchCheckerResult.Good = (maxColorMatchResult.Score * 100 > colorMatchCheckerParam.MatchScore);

                bool colorFound = false;
                if (String.IsNullOrEmpty(colorMatchCheckerParam.MatchColor) == false)
                {
                    string[] colorNames = colorMatchCheckerParam.MatchColor.Split(';');
                    foreach(string colorName in colorNames)
                    {
                        //if (maxColorMatchResult.Name == param.MatchColor)
                        if (maxColorMatchResult.Name == colorName)
                        {
                            colorFound = true;
                            break;
                        }
                    }

                    colorMatchCheckerResult.Good &= colorFound;
                }
                
                colorMatchCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Score", 100, colorMatchCheckerParam.MatchScore, maxColorMatchResult.Score*100));
                colorMatchCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Match Color", null, colorMatchCheckerParam.MatchColor));
            }
            else
            {
                colorMatchCheckerResult = new ColorMatchCheckerResult();
                colorMatchCheckerResult.ErrorMessage = "Algorithm is not trained";
                colorMatchCheckerResult.Good = false;

                colorMatchCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Match Color", colorMatchCheckerParam.MatchColor, colorMatchCheckerResult.ErrorMessage));
            }

            return colorMatchCheckerResult;
        }

        public abstract bool Trained { get; }
        public abstract void Train();
        public abstract ColorMatchCheckerResult Match(AlgoImage algoImage, RectangleF probeRegion, DebugContext debugContext);
    }        

    public class ColorMatchResult
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        float score;
        public float Score
        {
            get { return score; }
            set { score = value; }
        }

        public ColorMatchResult(string name, float score)
        {
            this.name = name;
            this.score = score;
        }
    }

    public class ColorMatchCheckerResult : AlgorithmResult
    {
        string errorMessage = "";
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        List<ColorMatchResult> colorMatchResultList = new List<ColorMatchResult>();
        public List<ColorMatchResult> ColorMatchResultList
        {
            get { return colorMatchResultList; }
            set { colorMatchResultList = value; }
        }

        public ColorMatchCheckerResult() : base("ColorMatchCheckerResult")
        {

        }

        public void AddColorMatchResult(ColorMatchResult colorMatchResult)
        {
            colorMatchResultList.Add(colorMatchResult);
        }

        public ColorMatchResult GetMaxColorMatchResult()
        {
            float tempScore = 0;
            string tempName = "";
            ColorMatchResult tempColorMatchResult = new ColorMatchResult(tempName, tempScore);
            foreach (var temp in colorMatchResultList)
            {
                if (temp.Score > tempScore)
                {
                    tempScore = temp.Score;
                    tempColorMatchResult.Name = temp.Name;
                    tempColorMatchResult.Score = temp.Score;
                }
            }
            
            return tempColorMatchResult;
        }
    }
}
