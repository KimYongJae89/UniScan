using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml;
using UniScanWPF.Helper;

namespace UniScanWPF.Table.Inspect
{
    public enum PatternFeature
    {
        Area, MaxFeret, MinFeret
    }

    public class PatternGroup
    {
        public float Diagonal => (float)Math.Sqrt((this.rotateWidth * this.rotateWidth) + (this.rotateHeight * this.rotateHeight));

        public int Count => this.count;
        int count;

        public float Area => this.area;
        float area;

        public float RotateWidth => this.rotateWidth;
        float rotateWidth;

        public float RotateHeight => this.rotateHeight;
        float rotateHeight;

        public float RotateAngle => this.rotateAngle;
        float rotateAngle;

        public List<BlobRect> PatternList => this.patternList;
        List<BlobRect> patternList = new List<BlobRect>();

        public float SumArea
        {
            get { return area * count; }
        }

        public string RefImagePath { get => this.refImagePath; set => this.refImagePath = value; }
        string refImagePath;

        BitmapSource refImage;
        public BitmapSource RefImage
        {
            get
            {
                if (refImage == null)
                {
                    if (File.Exists(refImagePath))
                        refImage = WPFImageHelper.LoadBitmapSource(refImagePath);
                }
                return refImage;
            }
            set => refImage = value;
        }

        public BlobRect GetAverageBlobRect()
        {
            return patternList[patternList.Count / 2];
        }

        public void AddPattern(BlobRect pattern)
        {
            this.patternList.Add(pattern);
            Calc();
        }

        public void AddPattern(List<BlobRect> patternList)
        {
            this.patternList.AddRange(patternList.ToArray());
            ReCalc();
        }

        private void Calc()
        {
            count = patternList.Count;

            if (patternList.Count == 1)
            {
                area = patternList[0].Area;
                rotateWidth = patternList[0].RotateWidth;
                rotateHeight = patternList[0].RotateHeight;

                rotateAngle = patternList[0].RotateAngle;
                return;
            }

            area = area * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().Area * (1.0f / patternList.Count);            
            rotateWidth = rotateWidth * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().RotateWidth * (1.0f / patternList.Count);
            rotateHeight = rotateHeight * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().RotateHeight * (1.0f / patternList.Count);
            rotateAngle = rotateAngle * ((patternList.Count - 1.0f) / patternList.Count) + patternList.Last().RotateAngle * (1.0f / patternList.Count);
        }

        public void ReCalc()
        {
            count = patternList.Count;
            area = patternList.Average(f => f.Area);
            rotateWidth = patternList.Average(f => f.MaxFeretDiameter);
            rotateHeight = patternList.Average(f => f.MinFeretDiameter);
            rotateAngle = patternList.Average(f => f.RotateAngle);
        }

        public void Clear()
        {
            patternList.Clear();
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
            return GetDiffValue(feature, this, blobRect);
        }

        public static float GetDiffValue(PatternFeature feature, PatternGroup patternGroup, BlobRect blobRect)
        {
            switch (feature)
            {
                case PatternFeature.Area:
                    return 1- Math.Min(patternGroup.Area, blobRect.Area) / Math.Max(patternGroup.Area, blobRect.Area);
                case PatternFeature.MaxFeret:
                    return Math.Abs(patternGroup.RotateWidth - blobRect.RotateWidth);
                case PatternFeature.MinFeret:
                    return Math.Abs(patternGroup.RotateHeight - blobRect.RotateHeight);
            }

            return 0;
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

        private List<BlobRect> GetSortedList(PatternFeature feature, PatternGroup patternGroup)
        {
            List<BlobRect> sortedList = null;

            switch (feature)
            {
                case PatternFeature.Area:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.Area).ToList();
                    break;
                case PatternFeature.MaxFeret:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.RotateWidth).ToList();
                    break;
                case PatternFeature.MinFeret:
                    sortedList = patternGroup.PatternList.OrderByDescending(x => x.RotateHeight).ToList();
                    break;
            }

            return sortedList;
        }

        public List<PatternGroup> DevideSubGroup(PatternFeature feature, float diffTol)
        {
            List<BlobRect> sortedList = GetSortedList(feature, this);

            List<PatternGroup> curPatternGroupList = new List<PatternGroup>();
            PatternGroup curPatternGroup = new PatternGroup();

            if (feature == PatternFeature.Area)
                diffTol = 0.2f;
            foreach (BlobRect blobRect in sortedList)
            {
                if (curPatternGroup.count > 0)
                {
                    float diffValue = curPatternGroup.GetDiffValue(feature, blobRect);
                    if (diffValue > diffTol)
                    {
                        curPatternGroupList.Add(curPatternGroup);
                        curPatternGroup = new PatternGroup();
                    }
                }
                curPatternGroup.AddPattern(blobRect);
            }

            if (curPatternGroup.patternList.Count > 0)
                curPatternGroupList.Add(curPatternGroup);

            return curPatternGroupList;
        }

        public List<PatternGroup> DivideSubGroup(float diffTol)
        {
            List<PatternGroup> patternGroupList = new List<PatternGroup>() { this };       

            foreach (Enum feature in Enum.GetValues(typeof(PatternFeature)))
            {
                List<PatternGroup> temp = new List<PatternGroup>();
                foreach (PatternGroup patternGroup in patternGroupList)
                {
                    List<PatternGroup> subPatternGroupList = patternGroup.DevideSubGroup((PatternFeature)feature, diffTol);
                    temp.AddRange(subPatternGroupList);
                }

                temp = temp.OrderByDescending(x => x.count*x.area).ToList();
                patternGroupList = temp;
            }

            return patternGroupList;
        }

        public void Load(string path, int index, XmlElement xmlElement)
        {
            this.refImagePath = Path.Combine(path, string.Format("Image{0}.bmp", index));

            refImage = WPFImageHelper.LoadBitmapSource(refImagePath);
            count = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "Count", "0"));
            area = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "AverageArea", "0"));
            rotateWidth = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "AverageMajorLength", "0"));
            rotateHeight = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "AverageMinorLength", "0"));
            rotateAngle = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "AverageRotateAngle", "0"));
        }

        public void Save(string path, int index, XmlElement xmlElement)
        {
            this.refImagePath = Path.Combine(path, string.Format("Image{0}.bmp", index));

            if (refImage != null)
                WPFImageHelper.SaveBitmapSource(refImagePath, refImage);

            XmlHelper.SetValue(xmlElement, "Count", count.ToString());
            XmlHelper.SetValue(xmlElement, "AverageArea", area.ToString());
            XmlHelper.SetValue(xmlElement, "AverageMajorLength", rotateWidth.ToString());
            XmlHelper.SetValue(xmlElement, "AverageMinorLength", rotateHeight.ToString());
            XmlHelper.SetValue(xmlElement, "AverageRotateAngle", rotateAngle.ToString());
        }
    }
}
