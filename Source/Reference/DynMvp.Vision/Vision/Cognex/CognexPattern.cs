using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.UI;

using Cognex.VisionPro;
using Cognex.VisionPro.PMAlign;

namespace DynMvp.Vision.Cognex
{
    public class CognexPattern : Pattern
    {
        private string featureType;
        private CogPMAlignTrainAlgorithmConstants trainAlgorithm;
        private CogPMAlignPattern cogPMAlignPattern = null;

        public CognexPattern(string featureType)
        {
            LogHelper.Debug(LoggerType.Operation, "CognexPattern::CognexPattern");
            this.imagingLibrary = ImagingLibrary.CognexVisionPro;

            this.featureType = featureType;

            if (featureType.Contains("VxPatMax") == true && CognexHelper.LicenseExist("VxPatMax") == true)
            {
                trainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatMaxAndPatQuick;
            }
            else
            {
                trainAlgorithm = CogPMAlignTrainAlgorithmConstants.PatQuick;
            }
        }

        ~CognexPattern()
        {
            Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();

            cogPMAlignPattern = null;
        }

        public override Pattern Clone()
        {
            CognexPattern cognexPattern = new CognexPattern(featureType);
            cognexPattern.Copy(this);

            return cognexPattern;
        }

        public override void Train(AlgoImage algoImage, PatternMatchingParam patternMatchingParam)
        {
            patternImage = (Image2D)algoImage.ToImageD();

            CognexGreyImage greyImage = algoImage as CognexGreyImage;

            cogPMAlignPattern = new CogPMAlignPattern();
            cogPMAlignPattern.TrainAlgorithm = trainAlgorithm;
            cogPMAlignPattern.TrainMode = CogPMAlignTrainModeConstants.Image;
            cogPMAlignPattern.TrainImage = greyImage.Image;

            PointF centerPt = new PointF(greyImage.Width / 2, greyImage.Height / 2);

            cogPMAlignPattern.Origin.TranslationX = centerPt.X;
            cogPMAlignPattern.Origin.TranslationY = centerPt.Y;

            CogRectangleAffine cogRectangle = new CogRectangleAffine();
            cogRectangle.SetCenterLengthsRotationSkew(centerPt.X, centerPt.Y, greyImage.Width, greyImage.Height, 0, 0);

            cogPMAlignPattern.TrainRegion = cogRectangle;
            cogPMAlignPattern.TrainRegionMode = CogRegionModeConstants.PixelAlignedBoundingBoxAdjustMask;

            try
            {
                cogPMAlignPattern.Train();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public override void UpdateMaskImage()
        {
            
        }

        public override PatternResult Inspect(AlgoImage probeClipImage, PatternMatchingParam patternMatchingParam, DebugContext debugContext)
        {
            CognexGreyImage greyImage = (CognexGreyImage)probeClipImage;
            //greyImage.Save("TargetImage1.bmp", debugContext);

            //if (debugContext.SaveDebugImage == true)
            //{
            //    Bitmap targetImage = CognexImageBuilder.ConvertImage(greyImage.Image);
            //    DebugHelper.SaveImage(targetImage, "TargetImage2.bmp", debugContext);
            //}

            //DebugHelper.SaveImage(patternImage, "PatternImage.bmp", debugContext);

            PatternResult pmResult = new PatternResult();

            if (cogPMAlignPattern.Trained == false)
            {
                pmResult.NotTrained = true;
                pmResult.Good = false;
                return pmResult;
            }

            PointF centerPt = new PointF(probeClipImage.Width / 2, probeClipImage.Height / 2);

            CogRectangleAffine cogRectangle = new CogRectangleAffine();
            cogRectangle.SetCenterLengthsRotationSkew(centerPt.X, centerPt.Y, probeClipImage.Width, probeClipImage.Height, 0, 0);

            CogPMAlignRunParams runParams = new CogPMAlignRunParams();

            runParams.AcceptThreshold = ((double)patternMatchingParam.MatchScore / 2) / 100;
            runParams.ApproximateNumberToFind = patternMatchingParam.NumToFind;
            runParams.ZoneAngle.Configuration = CogPMAlignZoneConstants.LowHigh;
            runParams.ZoneAngle.Low = MathHelper.DegToRad(patternMatchingParam.MinAngle);
            runParams.ZoneAngle.High = MathHelper.DegToRad(patternMatchingParam.MaxAngle);
            runParams.ZoneScale.Configuration = CogPMAlignZoneConstants.LowHigh;
            runParams.ZoneScale.Low = patternMatchingParam.MinScale;
            runParams.ZoneScale.High = patternMatchingParam.MaxScale;
//            runParams.ContrastThreshold = 10;

            CogPMAlignResults results;
            try
            {
                results = cogPMAlignPattern.Execute(greyImage.Image, cogRectangle, runParams);
            }
            catch (Exception ex)
            {
                string errorString = "Matching Error" + ex.ToString();
                LogHelper.Error(LoggerType.Error, errorString);
                pmResult.Message = errorString;
                return pmResult;
            }

            Size patternSize = new Size(patternImage.Width, patternImage.Height);
            RectangleF imageRect = new RectangleF(0, 0, probeClipImage.Width, probeClipImage.Height);

            foreach (CogPMAlignResult result in results)
            {
                MatchPos matchPos = new MatchPos();
                matchPos.Score = (float)result.Score;
                CogTransform2DLinear pose = result.GetPose();
                matchPos.Pos = new PointF((float)pose.TranslationX, (float)pose.TranslationY);
                matchPos.Angle = (float)(360 - MathHelper.RadToDeg(pose.MapAngle(0)));

                matchPos.PatternSize = patternSize;
                matchPos.PatternType = PatternType;

                RectangleF patternRect = DrawingHelper.FromCenterSize(matchPos.Pos, new SizeF(patternSize.Width, patternSize.Height));
                if (imageRect.Contains(patternRect))
                    pmResult.AddMatchPos(matchPos);
            }

            return pmResult;
        }
    }
}
