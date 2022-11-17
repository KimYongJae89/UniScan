using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Settings;

namespace UniScanG.Gravure.Vision.Fiducial
{
    internal class FiducialFinder
    {
        public string Position => this.position;
        string position;

        public AlgoImage MasterImage => this.masterImage;
        AlgoImage masterImage;

        public Rectangle SearchRect => searchRect;
        Rectangle searchRect;

        public bool IsInitialized => this.isInitialized;
        bool isInitialized;

        AlgoImage tempSearchImage;
        Pattern pattern;

        public FiducialFinder()
        {
            this.masterImage = null;
            this.pattern = null;
            this.tempSearchImage = null;
            this.isInitialized = false;
        }

        public void Initialize(AlignInfo alignInfo, PatternMatchingParam patternMatchingParam)
        {
            this.position = alignInfo.Position;
            this.searchRect = alignInfo.SearchRect;

            ImageD masterImage = alignInfo.MasterImageD;
            if (masterImage != null)
            {
                this.tempSearchImage = ImageBuilder.Build(PatternMatching.TypeName, this.searchRect.Size);
                this.masterImage = ImageBuilder.Build(PatternMatching.TypeName, masterImage);
                AlgorithmBuilder.GetImageProcessing(this.masterImage).Sobel(this.masterImage, this.masterImage);

                this.pattern = AlgorithmBuilder.CreatePattern();
                this.pattern.Train((Image2D)this.masterImage.ToImageD(), patternMatchingParam);

                this.isInitialized = true;
            }
        }

        public void Dispose()
        {
            this.pattern.Dispose();
            this.masterImage.Dispose();
            this.tempSearchImage.Dispose();
            this.isInitialized = false;
        }

        internal OffsetStruct Align(AlgoImage fullImage, Point offset, float minScore, bool saveImage, DebugContextG debugContextG)
        {
            OffsetStruct offsetStruct = new OffsetStruct();
            offsetStruct.Position = this.position;
            if (!this.IsInitialized)
                return offsetStruct;

            Rectangle fullRect = new Rectangle(Point.Empty, fullImage.Size);
            Rectangle pmSearchRect = DrawingHelper.Offset(this.searchRect, offset);
            if (Rectangle.Intersect(fullRect, pmSearchRect) != pmSearchRect)
                return offsetStruct;

            using (AlgoImage searchImage = fullImage.GetSubImage(pmSearchRect))
            {
                searchImage.Save(string.Format(@"searchImage_{0}.bmp", this.position), debugContextG);
                AlgorithmBuilder.GetImageProcessing(searchImage).Sobel(searchImage, tempSearchImage);
                tempSearchImage.Save(string.Format(@"searchSobelImage_{0}.bmp", this.position), debugContextG);

                this.masterImage.Save(string.Format(@"MasterImage_{0}.bmp", this.position), debugContextG);
                PatternResult patternResult = this.pattern.Inspect(tempSearchImage, new PatternMatchingParam() { NumToFind = 1, MatchScore = (int)minScore }, debugContextG);
                if (patternResult.MaxMatchPos == null)
                {
                    offsetStruct.Score = 0;
                    offsetStruct.IsGood = false;
                }
                else
                {
                    offsetStruct.Score = patternResult.MaxMatchPos.Score * 100;
                    offsetStruct.IsGood = (offsetStruct.Score >= minScore);
                }

                if (offsetStruct.IsGood)
                {
                    PointF searchCenter = DrawingHelper.CenterPoint(new Rectangle(Point.Empty, pmSearchRect.Size));
                    //PointF foundCenter = DrawingHelper.CenterPoint(patternResult.MaxMatchPos.RectF);
                    PointF foundCenter = patternResult.MaxMatchPos.Pos;
                    offsetStruct.OffsetF = PointF.Subtract(foundCenter, new SizeF(searchCenter));
                    offsetStruct.BaseF = DrawingHelper.CenterPoint(pmSearchRect);
                }
                patternResult.Dispose();

                // Save BasePoint Image
                if (saveImage)
                {
                    Point localCenter = Point.Round(DrawingHelper.Add(DrawingHelper.CenterPoint(pmSearchRect), offsetStruct.OffsetF));
                    Rectangle localRect = DrawingHelper.FromCenterSize(localCenter, this.masterImage.Size);

                    localRect.Intersect(new Rectangle(Point.Empty, fullImage.Size));
                    if (localRect.Width * localRect.Height > 0)
                    {
                        float scale = Math.Min(this.masterImage.Width, this.masterImage.Height) * 1f / Math.Min(OffsetStruct.ImageSize.Width, OffsetStruct.ImageSize.Height);
                        if (scale > 1)
                            scale = 1;
                        using (AlgoImage algoImage = fullImage.GetSubImage(localRect))
                            offsetStruct.ImageD = algoImage.ToImageD().Resize(scale);
                    }
                }
            }

            return offsetStruct;
        }
    }
}
