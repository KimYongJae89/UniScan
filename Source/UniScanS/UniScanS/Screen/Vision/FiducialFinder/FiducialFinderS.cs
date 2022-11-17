using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanS.Vision;
using UniScanS.Vision.FiducialFinder;

namespace UniScanS.Screen.Vision.FiducialFinder
{
    public class FiducialFinderSParam: FiducialFinderParam
    {
        List<FiducialPattern> fidPatternList = new List<FiducialPattern>();
        public List<FiducialPattern> FidPatternList
        {
            get { return fidPatternList; }
        }

        public FiducialFinderSParam():base()
        {
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            FiducialFinderSParam srcParam = (FiducialFinderSParam)srcAlgorithmParam;
            this.searchRangeHalfWidth = srcParam.SearchRangeHalfWidth;
            this.searchRangeHalfHeight = srcParam.SearchRangeHalfHeight;
            this.minScore = srcParam.MinScore;

            foreach (FiducialPattern fidPattern in srcParam.FidPatternList)
            {
                FiducialPattern copyPattern = new FiducialPattern(fidPattern.Pattern.PatternImage, fidPattern.Region, fidPattern.CenterPt);
                copyPattern.Pattern = fidPattern.Pattern.Clone();
                copyPattern.Region = fidPattern.Region;
                copyPattern.CenterPt = fidPattern.CenterPt;

                this.fidPatternList.Add(copyPattern);
            }
        }

        public override DynMvp.Vision.AlgorithmParam Clone()
        {
            FiducialFinderParam clone = new FiducialFinderParam();
            clone.CopyFrom(this);

            return clone;
        }

        public PatternMatchingParam CreatePatternMatchingParam()
        {
            PatternMatchingParam patternMatchingParam = new PatternMatchingParam();

            patternMatchingParam.MatchScore = minScore;
            patternMatchingParam.SearchRangeWidth = SearchRangeHalfWidth;
            patternMatchingParam.SearchRangeHeight = searchRangeHalfHeight;
            patternMatchingParam.SpeedType = 4;
            patternMatchingParam.MinAngle = 0;
            patternMatchingParam.MaxAngle = 0;
            patternMatchingParam.IgnorePolarity = false;
            patternMatchingParam.UseAngle = false;
            patternMatchingParam.NumToFind = 1;

            return patternMatchingParam;
        }

        public void Train()
        {
            if (MatroxHelper.LicenseExist("PAT") == false)
                return;

            PatternMatchingParam patternMatchingParam = CreatePatternMatchingParam();

            foreach (FiducialPattern fidPattern in fidPatternList)
                fidPattern.Train(patternMatchingParam);
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "SearchRangeHalfWidth", searchRangeHalfWidth.ToString());
            XmlHelper.SetValue(algorithmElement, "SearchRangeHalfHeight", searchRangeHalfHeight.ToString());
            XmlHelper.SetValue(algorithmElement, "MinScore", minScore.ToString());

            foreach (FiducialPattern fidPattern in fidPatternList)
            {
                XmlElement fidPatternElement = algorithmElement.OwnerDocument.CreateElement("FidPattern");
                algorithmElement.AppendChild(fidPatternElement);
                fidPattern.SaveParam(fidPatternElement);
            }
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            searchRangeHalfWidth = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeHalfWidth", "100"));
            searchRangeHalfHeight = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeHalfHeight", "100"));
            minScore = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MinScore", "80"));

            foreach (XmlElement fidPatternElement in algorithmElement)
            {
                if (fidPatternElement.Name == "FidPattern")
                {
                    FiducialPattern fidPattern = new FiducialPattern(fidPatternElement);
                    fidPatternList.Add(fidPattern);
                }
            }

            Train();
        }

        public override void Dispose()
        {
            foreach (FiducialPattern fidPattern in fidPatternList)
                fidPattern.Dispose();
        }
    }

    public class FiducialFinderS : UniScanS.Vision.FiducialFinder.FiducialFinder
    {
        public FiducialFinderS() : base()
        {
            this.param = new FiducialFinderSParam();
        }

        public override Algorithm Clone()
        {
            FiducialFinderS newFiducialFinderS = new FiducialFinderS();
            newFiducialFinderS.CopyFrom(this);
            return newFiducialFinderS;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            SheetInspectParam sheetAlgorithmInspectParam = (SheetInspectParam)algorithmInspectParam;
            ProcessBufferSetS bufferSet = (ProcessBufferSetS)sheetAlgorithmInspectParam.ProcessBufferSet;
            FiducialFinderAlgorithmResult algorithmResult = new FiducialFinderAlgorithmResult();
            
            FiducialFinderSParam param = (FiducialFinderSParam)this.Param;
            
            AlgoImage sourceImage = ImageBuilder.Build(GetAlgorithmType(), algorithmInspectParam.ClipImage, ImageType.Grey, ImageBandType.Luminance);
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(sourceImage);

            Rectangle sourceRect = new Rectangle(0, 0, sourceImage.Width, sourceImage.Height);

            float maxScore = 0;
            PatternResult patternResult = new PatternResult();
            foreach (FiducialPattern fidPattern in param.FidPatternList)
            {
                //늘리고
                Rectangle inflateRegion = fidPattern.Region;
                inflateRegion.Inflate(param.SearchRangeHalfWidth, param.SearchRangeHalfHeight);

                //교차 따로
                Rectangle intersectRegion = inflateRegion;
                intersectRegion.Intersect(sourceRect);

                if (intersectRegion.Width == 0 || intersectRegion.Height == 0)
                    continue;

                AlgoImage searchImage = sourceImage.GetSubImage(intersectRegion);
                
                patternResult = PatternInspect(searchImage, fidPattern.Pattern, algorithmInspectParam.DebugContext);
                
                searchImage.Dispose();

                if (maxScore < patternResult.MaxScore)
                    maxScore = patternResult.MaxScore;

                if (patternResult.Good == true)
                {
                    float offsetX = patternResult.MaxMatchPos.Pos.X - fidPattern.CenterPt.X + (inflateRegion.X < 0 ? 0 : inflateRegion.X);
                    float offsetY = patternResult.MaxMatchPos.Pos.Y - fidPattern.CenterPt.Y + (inflateRegion.Y < 0 ? 0 : inflateRegion.Y);

                    Rectangle imageRect = fidPattern.Region;
                    imageRect.Offset((int)Math.Round(offsetX), (int)Math.Round(offsetY));
                    imageRect.Inflate(50, 50);
                    imageRect.Intersect(sourceRect);
                    AlgoImage patternImage = sourceImage.GetSubImage(imageRect);
                    algorithmResult.FoundImage = patternImage.ToImageD().ToBitmap();
                    algorithmResult.OffsetFound = new SizeF(offsetX, offsetY);
                    algorithmResult.ResultRect = new RotatedRect(patternResult.ResultRect.X + intersectRegion.X, patternResult.ResultRect.Y + intersectRegion.Y,
                        patternResult.ResultRect.Width, patternResult.ResultRect.Height, patternResult.ResultRect.Angle);
                    
                    algorithmResult.Score = patternResult.MaxScore;
                    patternImage.Dispose();
                    break;
                }
            }

            if (algorithmResult.Score == 0)
            {
                algorithmResult.Score = maxScore;
            }
                
            sourceImage.Dispose();

            stopwatch.Stop();
            
            algorithmResult.SpandTime = stopwatch.Elapsed;
            algorithmResult.Good = patternResult.Good;

            return algorithmResult;
        }

        public PatternResult PatternInspect(AlgoImage boundImage, Pattern pattern, DebugContext debugContext)
        {
            FiducialFinderSParam param = (FiducialFinderSParam)this.Param;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(boundImage);
            //imageProcessing.Average(boundImage);
            //imageProcessing.Sobel(boundImage, boundImage);

            return pattern.Inspect(boundImage, param.CreatePatternMatchingParam(), debugContext);
        }
    }
}
