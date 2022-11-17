using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

using DynMvp.Base;

namespace DynMvp.Vision.OpenCv
{
    public class OpenCvPattern : Pattern
    {
        private OpenCvGreyImage cvImage;

        public OpenCvPattern() : base()
        {
            //LogHelper.Debug(LoggerType.Operation, "OpenCvPattern::OpenCvPattern");
            this.imagingLibrary = ImagingLibrary.OpenCv;
        }

        public override Pattern Clone()
        {
            OpenCvPattern openCvPattern = new OpenCvPattern();
            openCvPattern.Copy(this);

            return openCvPattern;
        }

        public override void Train(AlgoImage algoImage, PatternMatchingParam patternMatchingParam)
        {
            patternImage = (Image2D)algoImage.ToImageD();
            cvImage = algoImage as OpenCvGreyImage;
        }

        public override void UpdateMaskImage()
        {

        }

        public override PatternResult Inspect(AlgoImage targetClipImage, PatternMatchingParam patternMatchingParam, DebugContext debugContext)
        {
            PatternResult pmResult = new PatternResult();

            pmResult.Good = false;
            //OpenCvGreyImage targetOpenCvImage = targetClipImage as OpenCvGreyImage;

            //Image<Gray, float> resultImage = targetOpenCvImage.Image.MatchTemplate(cvImage.Image, Emgu.CV.CvEnum.TM_TYPE.CV_TM_CCORR_NORMED);

            //IntPtr ptrImage = resultImage.Ptr;

            //resultImage.ConvertScale<Byte>(100, 0).Save(String.Format("{0}\\Result.bmp", Configuration.TempFolder));

            //double minVal = 0, maxVal = 0;
            //Point minLoc = new Point();
            //Point maxLoc = new Point();
            //CvInvoke.cvMinMaxLoc(resultImage, ref minVal, ref maxVal, ref minLoc, ref maxLoc, IntPtr.Zero);

            //Size patternSize = cvImage.Image.Size;

            //MatchPos matchPos = new MatchPos();
            //matchPos.Score = (float)maxVal;
            //matchPos.Pos = new PointF(maxLoc.X + patternSize.Width / 2, maxLoc.Y + patternSize.Height / 2);
            //matchPos.PatternSize = patternSize;
            //matchPos.PatternType = PatternType;

            //pmResult.AddMatchPos(matchPos);

            return pmResult;
        }

        //private float SubtractMatching(OpenCvGreyImage findedPosImage)
        //{
        //    Byte[, ,] findedPosImageData = findedPosImage.Image.Data;
        //    Byte[, ,] patternImageData = patternImage.Image.Data;

        //    Size patternSize = patternImage.Image.Size;

        //    float weight;
        //    float totalWeight = 0;
        //    float totalValue = 0;
        //    for (int y = 0; y < patternSize.Height; ++y)
        //    {
        //        for (int x = 0; x < patternSize.Width; ++x)
        //        {
        //            weight = (float)(patternImageData[y, x, 0]) / 255;
        //            totalValue += Math.Abs(findedPosImageData[y, x, 0] - patternImageData[y, x, 0]) * weight;
        //            totalWeight += weight;
        //        }
        //    }

        //    float score = (100 - totalValue / totalWeight) / 100;
        //    if (score < 0)
        //        score = 0;

        //    return score;
        //}
    }
}
