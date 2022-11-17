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

namespace UniScanS.Screen.Data
{
    public enum PatternFeature
    {
        Area, Width, Height, CenterX, CenterY, Waist
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

        float averageWaist;
        public float AverageWaist
        {
            get { return averageWaist; }
            set { averageWaist = value; }
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
            blobRect.Elongation = averageWaist;
            blobRect.CenterPt = patternList[patternList.Count / 2].CenterPt;

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
                averageWidth = patternList[0].BoundingRect.Width;
                averageHeight = patternList[0].BoundingRect.Height;
                return;
            }

            averageArea = averageArea * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().Area * (1.0f / patternList.Count);
            diagonal = (float)Math.Sqrt(averageArea);
            averageCenterOffsetX = averageCenterOffsetX * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().CenterOffset.X * (1.0f / (float)patternList.Count);
            averageCenterOffsetY = averageCenterOffsetY * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().CenterOffset.Y * (1.0f / (float)patternList.Count);
            averageWidth = averageWidth * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().BoundingRect.Width * (1.0f / patternList.Count);
            averageHeight = averageHeight * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().BoundingRect.Height * (1.0f / patternList.Count);
            averageWaist = averageWaist * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().Elongation * (1.0f / patternList.Count);
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
                case PatternFeature.CenterX:
                    return averageCenterOffsetX - blobRect.CenterOffset.X;
                case PatternFeature.CenterY:
                    return averageCenterOffsetY - blobRect.CenterOffset.Y;
                case PatternFeature.Waist:
                    return averageWaist - blobRect.Elongation;
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
                case PatternFeature.CenterX:
                    diffValue = Math.Abs(patternGroup.averageCenterOffsetX - blobRect.CenterOffset.X);
                    break;
                case PatternFeature.CenterY:
                    diffValue = Math.Abs(patternGroup.averageCenterOffsetY - blobRect.CenterOffset.Y);
                    break;
                case PatternFeature.Waist:
                    diffValue = Math.Abs(patternGroup.averageWaist - blobRect.Elongation);
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
                case PatternFeature.CenterX:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.CenterOffset.X).ToList();
                    break;
                case PatternFeature.CenterY:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.CenterOffset.Y).ToList();
                    break;
                case PatternFeature.Waist:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.Elongation).ToList();
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
            List<SheetPatternGroup> patternGroupList = new List<SheetPatternGroup>();

            List<SheetPatternGroup> areaPatternGroupList = DevideSubGroup(PatternFeature.Area, this, diffTol);
            foreach (SheetPatternGroup areaPatternGroup in areaPatternGroupList)
            {
                List<SheetPatternGroup> heightPatternGroupList = DevideSubGroup(PatternFeature.Height, areaPatternGroup, diffTol);
                foreach (SheetPatternGroup heightPatternGroup in heightPatternGroupList)
                {
                    List<SheetPatternGroup> widthPatternGroupList = DevideSubGroup(PatternFeature.Width, heightPatternGroup, diffTol);
                    foreach (SheetPatternGroup widthPatternGroup in widthPatternGroupList)
                    {
                        List<SheetPatternGroup> centerXPatternGroupList = DevideSubGroup(PatternFeature.CenterX, widthPatternGroup, diffTol / 2.0f);
                        foreach (SheetPatternGroup centerXPatternGroup in centerXPatternGroupList)
                        {
                            List<SheetPatternGroup> centerYPatternGroupList = DevideSubGroup(PatternFeature.CenterY, centerXPatternGroup, diffTol / 2.0f);
                            foreach (SheetPatternGroup centerYPatternGroup in centerYPatternGroupList)
                            {
                                patternGroupList.AddRange(DevideSubGroup(PatternFeature.Waist, centerYPatternGroup, diffTol / 2.0f));
                            }
                        }
                    }
                }
            }

            return patternGroupList;
        }

        public void LoadParam(XmlElement paramElement)
        {
            averageArea = Convert.ToSingle(XmlHelper.GetValue(paramElement, "AverageArea", "0"));
            averageCenterOffsetX = Convert.ToSingle(XmlHelper.GetValue(paramElement, "AverageCenterOffsetX", "0"));
            averageCenterOffsetY = Convert.ToSingle(XmlHelper.GetValue(paramElement, "AverageCenterOffsetY", "0"));
            averageWidth = Convert.ToSingle(XmlHelper.GetValue(paramElement, "AverageWidth", "0"));
            averageHeight = Convert.ToSingle(XmlHelper.GetValue(paramElement, "AverageHeight", "0"));
            averageWaist = Convert.ToSingle(XmlHelper.GetValue(paramElement, "AverageWaist", "0"));
            diagonal = Convert.ToSingle(XmlHelper.GetValue(paramElement, "Diagonal", "0"));

            foreach (XmlElement rectElement in paramElement)
            {
                if (rectElement.Name == "Rect")
                {
                    BlobRect blobRect = new BlobRect();
                    blobRect.BoundingRect = new RectangleF(Convert.ToInt32(XmlHelper.GetValue(rectElement, "X", "0")),
                        Convert.ToInt32(XmlHelper.GetValue(rectElement, "Y", "0")),
                        Convert.ToInt32(XmlHelper.GetValue(rectElement, "Width", "0")),
                        Convert.ToInt32(XmlHelper.GetValue(rectElement, "Height", "0")));

                    patternList.Add(blobRect);
                }
            }
        }

        public void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "AverageArea", averageArea.ToString());
            XmlHelper.SetValue(paramElement, "AverageCenterOffsetX", averageCenterOffsetX.ToString());
            XmlHelper.SetValue(paramElement, "AverageCenterOffsetY", averageCenterOffsetY.ToString());
            XmlHelper.SetValue(paramElement, "AverageWidth", averageWidth.ToString());
            XmlHelper.SetValue(paramElement, "AverageHeight", averageHeight.ToString());
            XmlHelper.SetValue(paramElement, "Diagonal", diagonal.ToString());
            XmlHelper.SetValue(paramElement, "AverageWaist", averageWaist.ToString());

            foreach (BlobRect rect in patternList)
            {
                XmlElement rectElement = paramElement.OwnerDocument.CreateElement("Rect");
                
                XmlHelper.SetValue(rectElement, "X", rect.BoundingRect.X.ToString());
                XmlHelper.SetValue(rectElement, "Y", rect.BoundingRect.Y.ToString());
                XmlHelper.SetValue(rectElement, "Width", rect.BoundingRect.Width.ToString());
                XmlHelper.SetValue(rectElement, "Height", rect.BoundingRect.Height.ToString());

                paramElement.AppendChild(rectElement);
            }
        }
    }
}
