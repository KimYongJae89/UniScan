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

namespace UniScanG.Data.Vision
{
    public enum PatternFeature
    {
        Area, Width, Height, Waist, WaistRatio, //CenterX, CenterY, AreaRatio
    }

    public class SheetPatternGroupCollection : List<SheetPatternGroup>
    {
        public SheetPatternGroupCollection() : base() { }
        public SheetPatternGroupCollection(SheetPatternGroupCollection collection) : base(collection) { }

        public new void Clear()
        {
            base.Clear();
        }

        public void Save(XmlElement xmlElement)
        {
            this.ForEach(f => f.SaveParam(xmlElement, "SheetPatternGroup"));
        }

        public void Save(XmlElement xmlElement, string key)
        {
            XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
            xmlElement.AppendChild(subElement);
            Save(subElement);
        }

        public void Load(XmlElement xmlElement)
        {
            this.Clear();
            XmlNodeList sheetPatternGroupNodeList = xmlElement.GetElementsByTagName("SheetPatternGroup");
            foreach (XmlElement subElement in sheetPatternGroupNodeList)
            {
                SheetPatternGroup sheetPatternGroup = new SheetPatternGroup();
                sheetPatternGroup.LoadParam(subElement);
                this.Add(sheetPatternGroup);
            }
        }

        public void Load(XmlElement xmlElement, string key)
        {
            XmlElement subElement = xmlElement[key];
            if (subElement == null)
                return;
            Load(subElement);
        }

    }

    public class SheetPatternGroup
    {
        public bool Use { get => this.use; set => this.use = value; }
        bool use;

        public ImageD MasterImage { get => this.masterImage; set => this.masterImage = value; }
        ImageD masterImage = null;

        public double CountRatio { get => this.countRatio; set => this.countRatio = value; }
        double countRatio;

        public double Diagonal { get => this.diagonal; set => this.diagonal = value; }
        double diagonal;

        public double AverageArea { get => this.averageArea; set => this.averageArea = value; }
        double averageArea;

        //public float AverageCenterOffsetX { get => this.averageCenterOffsetX; set => this.averageCenterOffsetX = value; }
        //float averageCenterOffsetX;

        //public float AverageCenterOffsetY { get => this.averageCenterOffsetY; set => this.averageCenterOffsetY = value; }
        //float averageCenterOffsetY;

        public double AverageWidth { get => this.averageWidth; set => this.averageWidth = value; }
        double averageWidth;

        public double AverageHeight { get => this.averageHeight; set => this.averageHeight = value; }
        double averageHeight;

        public double AverageWaist { get => this.averageWaist; set => this.averageWaist = value; }
        double averageWaist;

        public double AverageAreaRatio { get => this.averageAreaRatio; set => this.averageAreaRatio = value; }
        double averageAreaRatio;

        public double AverageWaistLengthRatio { get => this.averageWaistLengthRatio; set => this.averageWaistLengthRatio = value; }
        double averageWaistLengthRatio;

        List<PatternInfo> patternList = new List<PatternInfo>();
        public List<PatternInfo> PatternList
        {
            get { return patternList; }
        }

        public int NumPattern
        {
            get { return patternList.Count; }
        }

        public SheetPatternGroup()
        {
        }

        public SheetPatternGroup(List<PatternInfo> patternList)
        {
            this.patternList.AddRange(patternList);
            CalcAll();
        }

        public SheetPatternGroup Clone()
        {
            List<PatternInfo> blobRectList = new List<PatternInfo>();
            this.patternList.ForEach(f => blobRectList.Add(f.Clone()));
            
            SheetPatternGroup newSheetPatternGroup = new SheetPatternGroup(blobRectList);
            newSheetPatternGroup.CalcAll();
            return newSheetPatternGroup;
        }

        //public BlobRect GetAverageBlobRect()
        //{
        //    BlobRect blobRect = new BlobRect();
        //    blobRect.Area = averageArea;
        //    blobRect.BoundingRect = patternList[patternList.Count / 2].BoundingRect;
        //    blobRect.CenterOffset = new PointF(averageCenterOffsetX, averageCenterOffsetY);

        //    blobRect.CenterPt = patternList[patternList.Count / 2].CenterPt;

        //    return blobRect;
        //}

        public void AddPattern(PatternInfo pattern)
        {
            patternList.Add(pattern);
            Calc();
        }

        public void AddPattern(List<PatternInfo> patternList)
        {
            this.patternList.AddRange(patternList.ToArray());
            CalcAll();
        }

        public void Merge(SheetPatternGroup sheetPatternGroup)
        {
            this.patternList.AddRange(sheetPatternGroup.patternList);
            CalcAll();
        }

        private void Calc()
        {
            if (patternList.Count == 1)
            {
                PatternInfo patternInfo = patternList[0];

                averageArea = patternInfo.GetValue(PatternFeature.Area);
                averageWidth = patternInfo.GetValue(PatternFeature.Width);
                averageHeight = patternInfo.GetValue(PatternFeature.Height);
                averageWaist = patternInfo.GetValue(PatternFeature.Waist);
                averageWaistLengthRatio = patternInfo.GetValue(PatternFeature.WaistRatio);
            }
            else
            {
                averageArea = Calc(averageArea, new Func<PatternInfo, double>(f => f.GetValue(PatternFeature.Area)));
                averageWidth = Calc(averageWidth, new Func<PatternInfo, double>(f => f.GetValue(PatternFeature.Width)));
                averageHeight = Calc(averageHeight, new Func<PatternInfo, double>(f => f.GetValue(PatternFeature.Height)));
                averageWaist = Calc(averageWaist, new Func<PatternInfo, double>(f => f.GetValue(PatternFeature.Waist)));
                averageWaistLengthRatio = Calc(averageWaistLengthRatio, new Func<PatternInfo, double>(f => f.GetValue(PatternFeature.WaistRatio)));
            }

            averageAreaRatio = averageArea / (averageWidth * averageHeight);
            diagonal = (float)Math.Sqrt(Math.Pow(averageWidth, 2) + Math.Pow(averageHeight, 2));
        }

        private double Calc(double oldValue, Func<PatternInfo, double> func)
        {
            return (oldValue * (patternList.Count - 1.0) + func(patternList.Last())) / patternList.Count;
        }

        internal void Offset(Point offset)
        {
            this.patternList.ForEach(f => f.MoveOffset(offset));
        }

        private void CalcAll()
        {
            averageArea = patternList.Average(f => f.GetValue(PatternFeature.Area));
            averageWidth = patternList.Average(f => f.GetValue(PatternFeature.Width));
            averageHeight = patternList.Average(f => f.GetValue(PatternFeature.Height));
            averageWaist = patternList.Average(f => f.GetValue(PatternFeature.Waist));
            averageWaistLengthRatio = patternList.Average(f => f.GetValue(PatternFeature.WaistRatio));

            averageAreaRatio = averageArea / (averageWidth * averageHeight);
            diagonal = (float)Math.Sqrt(Math.Pow(averageWidth, 2) + Math.Pow(averageHeight, 2));
        }

        public FigureGroup CreateFigureGroup()
        {
            FigureGroup figureGroup = new FigureGroup();
            foreach (PatternInfo info in patternList)
            {
                figureGroup.AddFigure(new RectangleFigure(info.BoundingRect, new Pen(Color.Yellow)));
                //figureGroup.AddFigure(new TextFigure(info.RotateWidth.ToString("F01"), PointF.Add(info.BoundingRect.Location, new Size(0, 0)), new Font(new FontFamily("맑은 고딕"), 14, FontStyle.Regular), Color.Yellow, StringAlignment.Near, StringAlignment.Near));
                //figureGroup.AddFigure(new TextFigure(info.WaistLengthRatio.ToString("F02"), PointF.Add(info.BoundingRect.Location, new Size(0, 14)), new Font(new FontFamily("맑은 고딕"), 14, FontStyle.Regular), Color.Yellow, StringAlignment.Near, StringAlignment.Near));
            }

            return figureGroup;
        }
        
        private double GetDiffValue(PatternFeature feature, SheetPatternGroup patternGroup, PatternInfo patternInfo)
        {
            double diffValue = 0;

            if (patternGroup.patternList.Count == 0)
                return diffValue;

            switch (feature)
            {
                case PatternFeature.Area:
                    diffValue = (patternGroup.averageArea - patternInfo.GetValue(feature)) / patternGroup.averageArea;
                    break;
                case PatternFeature.Width:
                    diffValue = (patternGroup.averageWidth - patternInfo.GetValue(feature)) / patternGroup.averageWidth;
                    break;
                case PatternFeature.Height:
                    diffValue = (patternGroup.averageHeight - patternInfo.GetValue(feature)) / patternGroup.averageHeight;
                    break;
                case PatternFeature.WaistRatio:
                    //diffValue = (patternGroup.averageWaist - blobRect.WaistLength) / patternGroup.averageWaist;
                    diffValue = patternGroup.averageWaistLengthRatio - patternInfo.GetValue(feature);
                    //diffValue = patternGroup.patternList.First().WaistLengthRatio - patternInfo.GetValue(feature);
                    break;
                //case PatternFeature.CenterX:
                //    diffValue = (patternGroup.averageCenterOffsetX - patternInfo.GetValue(feature)) / patternGroup.averageCenterOffsetX;
                //    break;
                //case PatternFeature.CenterY:
                //    diffValue = (patternGroup.averageCenterOffsetY - patternInfo.GetValue(feature)) / patternGroup.averageCenterOffsetY;
                //    break;
                //case PatternFeature.AreaRatio:
                //    diffValue = (patternGroup.averageAreaRatio - patternInfo.GetValue(feature)) / patternGroup.averageAreaRatio;
                //    break;
            }

            return Math.Abs(diffValue);
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

        private List<PatternInfo> GetSortedList(PatternFeature feature)
        {
            List<PatternInfo> sortedList = null;

            switch (feature)
            {
                case PatternFeature.Area:
                case PatternFeature.Width:
                case PatternFeature.Height:
                //case PatternFeature.CenterX:
                //case PatternFeature.CenterY:
                //case PatternFeature.AreaRatio:
                    sortedList = this.patternList.OrderByDescending(x =>x.GetValue(feature)).ToList();
                    break;
                case PatternFeature.WaistRatio:
                    sortedList = this.patternList.OrderBy(x => x.GetValue(feature)).ToList();
                    break;
            }
            return sortedList;
        }

        private List<SheetPatternGroup> DevideSubGroup(PatternFeature feature, float diffTol)
        {
            //System.Diagnostics.Debug.WriteLine(string.Format("PatternFeature {0} Count: {1}, Mean: {2:F3},  StdVariation: {3:F3}", feature,
            //     this.patternList.Count, this.GetMean(feature), this.GetStdVariation(feature)));

            List<PatternInfo> sortedList = GetSortedList(feature);

            List<SheetPatternGroup> subPatternGroupList = new List<SheetPatternGroup>();
            SheetPatternGroup subPatternGroup = new SheetPatternGroup();

            //if (feature == PatternFeature.Waist)
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.AppendLine("Area, ConvexArea, ConvexFillRatio, Width, WaistLength, WaistRatio");
            //    sortedList.ForEach(f => sb.AppendLine(string.Format("{0},{1},{2},{3},{4},{5}", f.Area, f.ConvexArea, f.ConvexFillRatio, f.BoundingRect.Width, f.WaistLength, f.WaistLengthRatio)));
            //    System.IO.File.WriteAllText(@"C:\temp\sortedList.txt", sb.ToString());
            //}

            //StringBuilder sbDiffValue = feature == PatternFeature.Waist ? new StringBuilder() : null;
            StringBuilder sbDiffValue = null;
            sbDiffValue?.AppendLine("DiffValue");
            foreach (PatternInfo blobRect in sortedList)
            {
                double diffValue = GetDiffValue(feature, subPatternGroup, blobRect);
                sbDiffValue?.AppendLine(diffValue.ToString());

                if (diffValue > diffTol)
                {
                    if (subPatternGroup.patternList.Count > 0)
                        subPatternGroupList.Add(subPatternGroup);

                    subPatternGroup = new SheetPatternGroup();
                }
                subPatternGroup.AddPattern(blobRect);
            }

            if (subPatternGroup.patternList.Count > 0)
                subPatternGroupList.Add(subPatternGroup);

            if (sbDiffValue != null)
                System.IO.File.WriteAllText(@"C:\temp\DiffValues.txt", sbDiffValue.ToString());

            //System.Diagnostics.Debug.WriteLine(string.Format("Result Count {0}: {1}", feature, subPatternGroupList.Count));

            subPatternGroupList = subPatternGroupList.OrderByDescending(f => f.PatternList.Count).ToList();
            return subPatternGroupList;
        }

        public List<SheetPatternGroup> DivideSubGroup(float diffTol)
        {
            List<SheetPatternGroup> workList = new List<SheetPatternGroup>();
            workList.Add(this);

            //List<SheetPatternGroup> areaPatternGroupList = this.DevideSubGroup(PatternFeature.Area, diffTol);
            //foreach (SheetPatternGroup areaPatternGroup in areaPatternGroupList)
            //{
            //    List<SheetPatternGroup> waistPatternGroupList = areaPatternGroup.DevideSubGroup(PatternFeature.WaistRatio, diffTol);
            //    patternGroupList.AddRange(waistPatternGroupList);

            //    return patternGroupList;
            //}

            Tuple<PatternFeature, float>[] tuples = new Tuple<PatternFeature, float>[]
            {
                new Tuple<PatternFeature, float>(PatternFeature.Area,diffTol*2 ),
                new Tuple<PatternFeature, float>(PatternFeature.Height,diffTol ),
                new Tuple<PatternFeature, float>(PatternFeature.Width,diffTol ),
                new Tuple<PatternFeature, float>(PatternFeature.WaistRatio,diffTol/2 ),
            };
            for (int i = 0; i < tuples.Length; i++)
            {
                Tuple<PatternFeature, float> tuple = tuples[i];
                List<SheetPatternGroup> doneList = new List<SheetPatternGroup>();
                workList.ForEach(f => doneList.AddRange(f.DevideSubGroup(tuple.Item1, tuple.Item2)));
                workList = doneList;
            }

            //List<SheetPatternGroup> areaPatternGroupList = this.DevideSubGroup(PatternFeature.Area, diffTol);
            //foreach (SheetPatternGroup areaPatternGroup in areaPatternGroupList)
            //{
            //    List<SheetPatternGroup> heightPatternGroupList = areaPatternGroup.DevideSubGroup(PatternFeature.Height, diffTol);
            //    foreach (SheetPatternGroup heightPatternGroup in heightPatternGroupList)
            //    {
            //        List<SheetPatternGroup> widthPatternGroupList = heightPatternGroup.DevideSubGroup(PatternFeature.Width, diffTol);
            //        foreach (SheetPatternGroup widthPatternGroup in widthPatternGroupList)
            //        {
            //            List<SheetPatternGroup> waistPatternGroupList = widthPatternGroup.DevideSubGroup(PatternFeature.WaistRatio, diffTol);
            //            patternGroupList.AddRange(waistPatternGroupList);
            //        }
            //    }
            //}

            return workList;
        }

        public void UpdateMaterImage(AlgoImage trainImage)
        {
            Rectangle imageRect = new Rectangle(Point.Empty, trainImage.Size);
            int width = (int)Math.Ceiling(this.patternList.Max(f => f.BoundingRect.Width) * 1.1f);
            int heigth = (int)Math.Ceiling(this.patternList.Max(f => f.BoundingRect.Height) * 1.1f);

            if (this.patternList.Count == 1)
            {
                Rectangle rect = Rectangle.Round(this.patternList[0].BoundingRect);
                Rectangle adjustPatternRect = Rectangle.Intersect(rect, imageRect);
                if (adjustPatternRect == rect)
                {
                    AlgoImage subPatternImage = trainImage.GetSubImage(adjustPatternRect);
                    this.masterImage = subPatternImage?.ToImageD();
                    subPatternImage.Dispose();
                }
                return;
            }

            PatternInfo[] patternInfos;
            if (this.patternList.Count < 100)
            {
                patternInfos = this.patternList.ToArray();
            }
            else
            {
                float step = this.patternList.Count / 100;
                patternInfos = new PatternInfo[100];
                for (int i = 0; i < 100; i++)
                    patternInfos[i] = this.patternList[(int)Math.Min(100, Math.Round(i * step))];
            }

            AlgoImage masterAlgoImage = ImageBuilder.Build(trainImage.LibraryType, trainImage.ImageType, width, heigth);
            masterAlgoImage.Clear();
            List<AlgoImage> subAlgoImageList = new List<AlgoImage>();

            for (int i = 0; i < patternInfos.Length; i++)
            {
                BlobRect pattern = patternInfos[i];
                Point pattternCenter = Point.Round(DrawingHelper.CenterPoint(pattern.BoundingRect));
                Rectangle patternRect = DrawingHelper.FromCenterSize(pattternCenter, masterAlgoImage.Size);
                Rectangle adjustPatternRect = Rectangle.Intersect(patternRect, imageRect);
                if (adjustPatternRect == patternRect)
                {
                    AlgoImage subPatternImage = trainImage.GetSubImage(adjustPatternRect);
                    subAlgoImageList.Add(subPatternImage);
                }
            }

            try
            {
                if (subAlgoImageList.Count > 0)
                {
                    ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(masterAlgoImage);
                    ip.WeightedAdd(subAlgoImageList.ToArray(), masterAlgoImage);
                }
                this.masterImage = masterAlgoImage?.ToImageD();
            }
            finally
            {
                subAlgoImageList.ForEach(f => f.Dispose());
                masterAlgoImage?.Dispose();
            }
        }

        public string GetInfoText(Calibration calibration)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0} [%]", StringManager.GetString("Ratio")));
            sb.AppendLine((this.countRatio * 100).ToString("0.00"));

            //sb.AppendLine(string.Format("{0} [px^2]", StringManager.GetString("Area")));
            //sb.AppendLine((this.averageArea * 100).ToString("0.00"));
            string unit = "px";
            double pelSize = 1;
            if (calibration != null)
            {
                unit = "mm";
                pelSize = calibration.PelSize.Width / 1000;
            }

            sb.AppendLine(string.Format("{0} [{1}]", StringManager.GetString("Width"), unit));
            sb.AppendLine((this.averageWidth * pelSize).ToString("F02"));

            sb.AppendLine(string.Format("{0} [{1}]", StringManager.GetString("Heigth"), unit));
            sb.Append((this.averageHeight * pelSize).ToString("F02"));

            return sb.ToString();
        }

        public void LoadParam(XmlElement paramElement, string key)
        {
            XmlElement xmlElement = paramElement[key];
            if (xmlElement == null)
                return;

                LoadParam(xmlElement);
        }

        public void LoadParam(XmlElement paramElement)
        {
            this.use = XmlHelper.GetValue(paramElement, "Use", this.use);
            this.countRatio = XmlHelper.GetValue(paramElement, "CountRatio", this.countRatio);
            this.averageArea = XmlHelper.GetValue(paramElement, "AverageArea", this.averageArea);
            //this.averageCenterOffsetX = XmlHelper.GetValue(paramElement, "AverageCenterOffsetX", this.averageCenterOffsetX);
            //this.averageCenterOffsetY = XmlHelper.GetValue(paramElement, "AverageCenterOffsetY", this.averageCenterOffsetY);
            this.averageWidth = XmlHelper.GetValue(paramElement, "AverageWidth", this.averageWidth);
            this.averageHeight = XmlHelper.GetValue(paramElement, "AverageHeight", this.averageHeight);
            this.diagonal = XmlHelper.GetValue(paramElement, "Diagonal", this.diagonal);

            this.averageWaist = XmlHelper.GetValue(paramElement, "AverageWaist", this.averageWaist);
            this.averageWaistLengthRatio = XmlHelper.GetValue(paramElement, "AverageWaistLengthRatio", this.averageWaistLengthRatio);

            string imageString;
            imageString = XmlHelper.GetValue(paramElement, "MasterImage", "");
            if(string.IsNullOrEmpty(imageString)==false)
            {
                Bitmap bitmap = ImageHelper.Base64StringToBitmap(imageString);
                masterImage = Image2D.FromBitmap(bitmap);
                bitmap.Dispose();
            }

            XmlNodeList xmlNodeList = paramElement.GetElementsByTagName("BlobRect");
            foreach (XmlElement subElement in xmlNodeList)
            {
                PatternInfo blobRect = new PatternInfo();
                blobRect.LoadXml(subElement);
                patternList.Add(blobRect);
            }
        }

        public void SaveParam(XmlElement paramElement, string key)
        {
            XmlElement subElement = paramElement.OwnerDocument.CreateElement(key);
            paramElement.AppendChild(subElement);

            SaveParam(subElement);
        }

        public void SaveParam(XmlElement paramElement)
        {
            XmlHelper.SetValue(paramElement, "Use", this.use);
            XmlHelper.SetValue(paramElement, "CountRatio", this.countRatio);
            XmlHelper.SetValue(paramElement, "AverageArea", this.averageArea);
            //XmlHelper.SetValue(paramElement, "AverageCenterOffsetX", this.averageCenterOffsetX);
            //XmlHelper.SetValue(paramElement, "AverageCenterOffsetY", this.averageCenterOffsetY);
            XmlHelper.SetValue(paramElement, "AverageWidth", this.averageWidth);
            XmlHelper.SetValue(paramElement, "AverageHeight", this.averageHeight);
            XmlHelper.SetValue(paramElement, "Diagonal", this.diagonal);

            XmlHelper.SetValue(paramElement, "AverageWaist", this.averageWaist);
            XmlHelper.SetValue(paramElement, "AverageWaistLengthRatio", this.averageWaistLengthRatio);

            if (this.masterImage != null)
            {
                Bitmap bitmap = masterImage.ToBitmap();
                string imageString = ImageHelper.BitmapToBase64String(bitmap);
                XmlHelper.SetValue(paramElement, "MasterImage", imageString.ToString());
                bitmap.Dispose();
            }

            foreach (PatternInfo blobRect in patternList)
            {
                blobRect.SaveXml(paramElement, "BlobRect");
            }
        }
    }
}
