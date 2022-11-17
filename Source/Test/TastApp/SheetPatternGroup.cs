using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TastApp
{
    public enum PatternFeature
    {
        Area, Width, Height, CogX, CogY, MiddleLength
    }

    public class SheetPatternGroup
    {
        float diagonal;
        public float Diagonal
        {
            get { return diagonal; }
            set { diagonal = value; }
        }

        float averageArea;
        public float AverageArea
        {
            get { return averageArea; }
            set { averageArea = value; }
        }

        float averageCenterOffsetX;
        public float AverageCenterOffsetX
        {
            get { return averageCenterOffsetX; }
            set { averageCenterOffsetX = value; }
        }

        float averageCenterOffsetY;
        public float AverageCenterOffsetY
        {
            get { return averageCenterOffsetY; }
            set { averageCenterOffsetY = value; }
        }

        float averageWidth;
        public float AverageWidth
        {
            get { return averageWidth; }
            set { averageWidth = value; }
        }

        float averageHeight;
        public float AverageHeight
        {
            get { return averageHeight; }
            set { averageHeight = value; }
        }

        float averageMiddleLength;
        public float AverageMiddleLength
        {
            get { return averageMiddleLength; }
            set { averageMiddleLength = value; }
        }

        List<BlobRect> patternList = new List<BlobRect>();
        public List<BlobRect> PatternList
        {
            get { return patternList; }
        }

        public int NumPattern
        {
            get { return patternList.Count; }
        }

        public BlobRect GetAverageBlobRect()
        {
            BlobRect blobRect = new BlobRect();
            blobRect.Area = averageArea;
            blobRect.BoundingRect = patternList[patternList.Count / 2].BoundingRect;
            blobRect.CenterOffset = new PointF(averageCenterOffsetX, averageCenterOffsetY);

            blobRect.CenterPt = patternList[patternList.Count / 2].CenterPt;
            blobRect.MeasureData = patternList[patternList.Count / 2].MeasureData;

            return blobRect;
        }

        public void AddPattern(BlobRect pattern)
        {
            patternList.Add(pattern);
            Calc();
        }

        public void AddPattern(List<BlobRect> patternList)
        {
            this.patternList.AddRange(patternList.ToArray());
            Calc();
        }

        private void Calc()
        {
            if (patternList.Count == 1)
            {
                averageArea = patternList[0].Area;
                diagonal = (float)Math.Sqrt(averageArea);
                averageCenterOffsetX = patternList[0].CenterOffset.X;
                averageCenterOffsetY = patternList[0].CenterOffset.Y;
                averageWidth = patternList[0].RotateWidth;
                averageHeight = patternList[0].RotateHeight;
                //averageMiddleLength = (float)patternList[0].MeasureData[0];
                return;
            }

            averageArea = averageArea * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().Area * (1.0f / patternList.Count);
            diagonal = (float)Math.Sqrt(averageArea);
            averageCenterOffsetX = averageCenterOffsetX * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().CenterOffset.X * (1.0f / (float)patternList.Count);
            averageCenterOffsetY = averageCenterOffsetY * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().CenterOffset.Y * (1.0f / (float)patternList.Count);
            averageWidth = averageWidth * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().RotateWidth * (1.0f / patternList.Count);
            averageHeight = averageHeight * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().RotateHeight * (1.0f / patternList.Count);
            //averageMiddleLength = averageMiddleLength * ((patternList.Count - 1.0f) / patternList.Count) + (float)patternList.Last().MeasureData[0] * (1.0f / patternList.Count);
        }

        public FigureGroup CreateFigureGroup()
        {
            FigureGroup figureGroup = new FigureGroup();
            foreach (BlobRect blobRect in patternList)
                figureGroup.AddFigure(new RectangleFigure(blobRect.BoundingRect, new Pen(Color.Yellow)));

            return figureGroup;
        }

        public float GetDiffValue(PatternFeature feature, BlobRect blobRect)
        {
            switch (feature)
            {
                case PatternFeature.Area:
                    return diagonal - (float)Math.Sqrt(blobRect.Area);
                case PatternFeature.Width:
                    return averageWidth - blobRect.BoundingRect.Width;
                case PatternFeature.Height:
                    return averageHeight - blobRect.BoundingRect.Height;
                case PatternFeature.CogX:
                    return averageCenterOffsetX - blobRect.CenterOffset.X;
                case PatternFeature.CogY:
                    return averageCenterOffsetY - blobRect.CenterOffset.Y;
                case PatternFeature.MiddleLength:
                    return averageMiddleLength - ((StripeCheckerResult)blobRect.MeasureData).MeanWidth;
            }

            return 0;
        }

        private float GetDiffValue(PatternFeature feature, SheetPatternGroup patternGroup, BlobRect blobRect)
        {
            float diffValue = 0;

            switch (feature)
            {
                case PatternFeature.Area:
                    diffValue = Math.Abs((float)Math.Sqrt(patternGroup.averageArea) - (float)Math.Sqrt(blobRect.Area));
                    break;
                case PatternFeature.Width:
                    diffValue = Math.Abs(patternGroup.averageWidth - blobRect.BoundingRect.Width);
                    break;
                case PatternFeature.Height:
                    diffValue = Math.Abs(patternGroup.averageHeight - blobRect.BoundingRect.Height);
                    break;
                case PatternFeature.CogX:
                    diffValue = Math.Abs(patternGroup.averageCenterOffsetX - blobRect.CenterOffset.X);
                    break;
                case PatternFeature.CogY:
                    diffValue = Math.Abs(patternGroup.averageCenterOffsetY - blobRect.CenterOffset.Y);
                    break;
                case PatternFeature.MiddleLength:
                    //diffValue = Math.Abs(patternGroup.averageMiddleLength - ((StripeCheckerResult)blobRect.MeasureData).MeanWidth;
                    break;
            }

            return diffValue;
        }

        public bool IsContain(Rectangle region)
        {
            foreach (BlobRect blobRect in  patternList)
            {
                if (region.Contains(Point.Round(blobRect.CenterPt)) == true)
                    return true;
            }

            return false;
        }

        private List<BlobRect> GetSortedList(PatternFeature feature, SheetPatternGroup patternGroup)
        {
            List<BlobRect> sortedList = null;

            switch (feature)
            {
                case PatternFeature.Area:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.Area).ToList();
                    break;
                case PatternFeature.Width:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.BoundingRect.Width).ToList();
                    break;
                case PatternFeature.Height:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.BoundingRect.Height).ToList();
                    break;
                case PatternFeature.CogX:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.CenterOffset.X).ToList();
                    break;
                case PatternFeature.CogY:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.CenterOffset.Y).ToList();
                    break;
                case PatternFeature.MiddleLength:
                    //sortedList = patternGroup.PatternList.OrderByDescending(x => x.MeasureData[0]).ToList();
                    break;
            }

            return sortedList;
        }

        private List<SheetPatternGroup> DevideSubGroup(PatternFeature feature, SheetPatternGroup patternGroup, float diffTol)
        {
            List<BlobRect> sortedList = GetSortedList(feature, patternGroup);

            List<SheetPatternGroup> subPatternGroupList = new List<SheetPatternGroup>();
            SheetPatternGroup subPatternGroup = new SheetPatternGroup();

            foreach (BlobRect blobRect in sortedList)
            {
                float diffValue = GetDiffValue(feature, subPatternGroup, blobRect);
                if (diffValue <= diffTol)
                {
                    subPatternGroup.AddPattern(blobRect);
                }
                else
                {
                    if (subPatternGroup.patternList.Count > 0)
                        subPatternGroupList.Add(subPatternGroup);

                    subPatternGroup = new SheetPatternGroup();
                    subPatternGroup.AddPattern(blobRect);
                }
            }

            if (subPatternGroup.patternList.Count > 0)
                subPatternGroupList.Add(subPatternGroup);

            return subPatternGroupList;
        }

        public List<SheetPatternGroup> DivideSubGroup(float diffTol)
        {
            List<SheetPatternGroup> sheetPatternGroupList = new List<SheetPatternGroup>() { this };

            foreach (Enum feature in Enum.GetValues(typeof(PatternFeature)))
            {
                List<SheetPatternGroup> temp = new List<SheetPatternGroup>();
                foreach (SheetPatternGroup sheetPatternGroup in sheetPatternGroupList)
                    temp.AddRange(DevideSubGroup((PatternFeature)feature, sheetPatternGroup, diffTol));

                sheetPatternGroupList = temp;
            }

            return sheetPatternGroupList;
        }
    }
}
