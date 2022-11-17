using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using Matrox.MatroxImagingLibrary;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Devices;
using System.Diagnostics;

namespace DynMvp.Vision.Matrox
{
    public class MilPattern : Pattern, MilObject
    {
        public StackTrace StackTrace { get; private set; }

        private MIL_ID patternId = MIL.M_NULL;
        public MIL_ID PatternId
        {
            get { return patternId; }
            set { patternId = value; }
        }

        ~MilPattern()
        {
            Dispose();
        }

        public override void Dispose()
        {
            base.Dispose();
            MilObjectManager.Instance.ReleaseObject(this);
        }

        public MilPattern() : base()
        {
            //LogHelper.Debug(LoggerType.Operation, "MilPattern::MilPattern");
            this.imagingLibrary = ImagingLibrary.MatroxMIL;
            MilObjectManager.Instance.AddObject(this);
        }

        public override Pattern Clone()
        {
            MilPattern milImage = new MilPattern();
            milImage.Copy(this);

            return milImage;
        }

        public override void Train(AlgoImage algoImage, PatternMatchingParam patternMatchingParam)
        {
            MilImage greyImage = MilImage.CheckGreyImage(algoImage, "MilPattern.Train", "Source");

            if (this.patternId != MIL.M_NULL)
                MIL.MpatFree(this.patternId);

            if (this.patternImage != null)
                this.patternImage.Dispose();

            this.patternImage = (Image2D)MilImageBuilder.ConvertImage((MilGreyImage)greyImage);

            this.patternId = MIL.MpatAlloc(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_NULL);
            MIL.MpatDefine(patternId, MIL.M_REGULAR_MODEL, greyImage.Image, 0, 0, greyImage.Width, greyImage.Height, MIL.M_DEFAULT);

            SetParam(patternMatchingParam);

            MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_ACCURACY, MIL.M_HIGH);
            switch (patternMatchingParam.SpeedType)
            {
                case 0:
                    MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SPEED, MIL.M_VERY_LOW);
                    break;
                case 1:
                    MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SPEED, MIL.M_LOW);
                    break;
                default:
                case 2:
                    MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SPEED, MIL.M_MEDIUM);
                    break;
                case 3:
                    MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SPEED, MIL.M_HIGH);
                    break;
                case 4:
                    MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SPEED, MIL.M_VERY_HIGH);
                    break;
            }


            if (patternMatchingParam.UseAngle == true)
            {
                MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_MODE, MIL.M_ENABLE);
                MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_DELTA_POS, patternMatchingParam.MaxAngle);
                MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_DELTA_NEG, -patternMatchingParam.MinAngle);
                MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_ACCURACY, 0.1);
                MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_INTERPOLATION_MODE, MIL.M_NEAREST_NEIGHBOR);
            }
            else
            {
                MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_SEARCH_ANGLE_MODE, MIL.M_DISABLE);
            }

            // 피라미드 이미지 생성: 모델 이미지 크기 기준 -> 모델 형태(Content) 기준
            MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_FIRST_LEVEL, MIL.M_AUTO_CONTENT_BASED);
            //MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_FIRST_LEVEL, 0);

            MIL.MpatPreprocess(patternId, MIL.M_DEFAULT, MIL.M_NULL);

            // 현재 레벨 가져옴
            // 0 - 원본이미지, 1 - 절반이미지. 2 - 반반이미지, ....
            double maxLevel = -1, firstLevel = -1, lastLevel = -1;
            MIL.MpatInquire(patternId, 0, MIL.M_MODEL_MAX_LEVEL, ref maxLevel);
            MIL.MpatInquire(patternId, 0, MIL.M_PROC_FIRST_LEVEL, ref firstLevel);
            MIL.MpatInquire(patternId, 0, MIL.M_PROC_LAST_LEVEL, ref lastLevel);

            // 새 레벨로 설정
            //MIL.MpatControl(patternId, 0, MIL.M_FAST_FIND, MIL.M_DISABLE);
            //MIL.MpatControl(patternId, 0, MIL.M_FIRST_LEVEL, firstLevel);
            //MIL.MpatControl(patternId, 0, MIL.M_LAST_LEVEL, lastLevel);

            //// Level 재설정 후 PreProcess 해야 함.
            //MIL.MpatPreprocess(patternId, MIL.M_DEFAULT, MIL.M_NULL);
        }

        private void SetParam(PatternMatchingParam patternMatchingParam)
        {
            if (patternMatchingParam.NumToFind <= 0)
                MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_NUMBER, MIL.M_ALL);
            else
                MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_NUMBER, patternMatchingParam.NumToFind);

            MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_ACCEPTANCE, patternMatchingParam.MatchScore);
            MIL.MpatControl(patternId, MIL.M_ALL, MIL.M_CERTAINTY, Math.Min(100, patternMatchingParam.MatchScore * 2));
        }

        public override void UpdateMaskImage()
        {
            if (MaskFigures.FigureExist == false)
                return;

            Image2D maskImage = GetMaskedImage();
            MilGreyImage greyImage = (MilGreyImage)ImageBuilder.MilImageBuilder.Build(maskImage, ImageType.Grey, ImageBandType.Luminance);
            MIL.MpatMask(patternId, MIL.M_DEFAULT, greyImage.Image, MIL.M_DONT_CARE, MIL.M_DEFAULT);

            PreprocModel();

            maskImage.Dispose();
        }

        private void PreprocModel()
        {
            if (patternId == MIL.M_NULL)
            {
                throw new InvalidOperationException();
            }

            MIL.MpatPreprocess(MIL.M_NULL, patternId, MIL.M_DEFAULT);
        }

        public override Image2D GetMaskedImage()
        {
            Bitmap rgbImage = new Bitmap(PatternImage.Width, PatternImage.Height);
            ImageHelper.Clear(rgbImage, 0);

            Bitmap maskImage;
            if (MaskFigures.FigureExist == false)
            {
                maskImage = ImageHelper.MakeGrayscale(rgbImage);
            }
            else
            {
                Graphics g = Graphics.FromImage(rgbImage);

                MaskFigures.SetTempBrush(new SolidBrush(Color.White));

                MaskFigures.Draw(g, new CoordTransformer(), true);

                MaskFigures.ResetTempProperty();

                g.Dispose();

                maskImage = ImageHelper.MakeGrayscale(rgbImage);
            }

            rgbImage.Dispose();

            return Image2D.FromBitmap(maskImage);
        }

        public override PatternResult Inspect(AlgoImage targetClipImage, PatternMatchingParam patternMatchingParam, DebugContext debugContext)
        {
            RectangleF imageRect = new RectangleF(0, 0, targetClipImage.Width, targetClipImage.Height);
            MilGreyImage greyImage = MilImage.CheckGreyImage(targetClipImage, "MilPattern.Inspect", "Source");

            PatternResult pmResult = new PatternResult();
            if (patternId == MIL.M_NULL)
            {
                pmResult.NotTrained = true;
                pmResult.Good = false;
                return pmResult;
            }

            SetParam(patternMatchingParam);

            MIL_ID patResultId = MIL.MpatAllocResult(MIL.M_DEFAULT_HOST, MIL.M_DEFAULT, MIL.M_NULL);
            MIL.MpatFind(patternId, greyImage.Image, patResultId);

            Size patternSize = patternImage.Size;
            double numOccurDbl = 0;
            MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_NUMBER, ref numOccurDbl);
            long numOccurrences = (int)numOccurDbl;
            if (numOccurrences > 0)
            {
                double[] posX = new double[numOccurrences];
                double[] posY = new double[numOccurrences];
                double[] score = new double[numOccurrences];
                double[] angle = new double[numOccurrences];

                MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_SCORE, score);
                MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_POSITION_X, posX);
                MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_POSITION_Y, posY);
                MIL.MpatGetResult(patResultId, MIL.M_DEFAULT, MIL.M_ANGLE, angle);

                for (int i = 0; i < numOccurrences; i++)
                {
                    MatchPos matchPos = new MatchPos();
                    matchPos.Score = (float)score[i] / 100;
                    matchPos.Pos = new PointF((float)posX[i], (float)posY[i]);
                    matchPos.PatternSize = patternSize;
                    matchPos.PatternType = PatternType;
                    matchPos.Angle = (float)angle[i];

                    RectangleF patternRect = DrawingHelper.FromCenterSize(matchPos.Pos, new SizeF(patternSize.Width, patternSize.Height));

                    if (imageRect.Contains(patternRect) && matchPos.Score >= patternMatchingParam.MatchScore / 100.0f)
                        pmResult.AddMatchPos(matchPos);

                    if (pmResult.MaxMatchPos != null)
                    {
                        pmResult.ResultRect = new RotatedRect(pmResult.MaxMatchPos.RectF, pmResult.MaxMatchPos.Angle);
                        pmResult.Good = true;
                    }
                }
            }

            MIL.MpatFree(patResultId);

            return pmResult;
        }

        public void Free()
        {
            if (patternId != MIL.M_NULL)
                MIL.MpatFree(patternId);
            patternId = MIL.M_NULL;
        }

        public void AddTrace()
        {
#if DEBUG
            this.StackTrace = new StackTrace();
#endif
        }
    }
}

