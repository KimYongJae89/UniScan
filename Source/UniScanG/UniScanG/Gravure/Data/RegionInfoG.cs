using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.Calculator;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Gravure.Vision.Trainer;

namespace UniScanG.Gravure.Data
{
    public enum GroupDirection { None, Horizontal, Vertical }
    public class PatternRegionElement
    {
        public Rectangle PatternRect { get => patternRect; set => patternRect = value; }
        public Rectangle AdjustpatternRect { get => adjustpatternRect; set => adjustpatternRect = value; }

        Rectangle patternRect;
        Rectangle adjustpatternRect;

        public PatternRegionElement(Rectangle patternRect, Rectangle adjustpatternRect)
        {
            this.patternRect = patternRect;
            this.adjustpatternRect = adjustpatternRect;
        }
    }

    public class InspectElement
    {
        public int Group { get => this.group; }
        int group;

        public Rectangle Rectangle { get => rectangle; }
        Rectangle rectangle = Rectangle.Empty;

        public Rectangle Dontcare { get => dontcare; set => dontcare = value; }
        Rectangle dontcare = Rectangle.Empty;

        public int OffsetYBase { get => offsetYBase; set => offsetYBase = value; }
        int offsetYBase = -1;

        public int OffsetXBase { get => offsetXBase; set => offsetXBase = value; }
        int offsetXBase = -1;

        public float GreyAverage { get => this.greyAverage; set => this.greyAverage = value; }
        float greyAverage = -1;

        public bool HasDontcare { get => hasDontcare; }
        bool hasDontcare = false;

        public InspectElement(int group, Rectangle rectangle, int offsetXBase, int offsetYBase, float greyAverage) : this(group, rectangle, offsetXBase, offsetYBase, greyAverage, new Rectangle[0]) { }
        public InspectElement(int group, Rectangle rectangle, int offsetXBase, int offsetYBase, float greyAverage, Rectangle[] dontcareRects)
        {
            this.group = group;
            this.rectangle = rectangle;
            this.offsetXBase = offsetXBase;
            this.offsetYBase = offsetYBase;
            //this.dontcareRectList = new List<Rectangle>(dontcareRects);
            this.greyAverage = greyAverage;

            UpdateDontcareFlag();
        }

        private void UpdateDontcareFlag()
        {
            Rectangle inflateRect = Rectangle.Inflate(new Rectangle(Point.Empty, this.rectangle.Size), 0, -1);
            //this.hasDontcare = this.dontcareRectList.Exists(f => Rectangle.Intersect(inflateRect, f) == f);
        }

        public void Inflate(Size size)
        {
            Rectangle rectangle = this.rectangle;
            this.rectangle.Inflate(size);

            //Rectangle dontcareFullRect = new Rectangle(Point.Empty, this.rectangle.Size);
            //for (int i = 0; i < this.dontcareRectList.Count; i++)
            //{
            //    Rectangle dontcareRect = this.dontcareRectList[i];
            //    RectangleF ratio = new RectangleF(
            //        dontcareRect.X * 1.0f / rectangle.Width,
            //        dontcareRect.Y * 1.0f / rectangle.Height,
            //        dontcareRect.Width * 1.0f / rectangle.Width,
            //        dontcareRect.Height * 1.0f / rectangle.Height);

            //    Rectangle dontcareRect2 = new Rectangle(
            //        (int)Math.Round(ratio.X * this.rectangle.Width),
            //        (int)Math.Round(ratio.Y * this.rectangle.Height),
            //        (int)Math.Round(ratio.Width * this.rectangle.Width),
            //        (int)Math.Round(ratio.Height * this.rectangle.Height));
            //    dontcareRect2.Intersect(dontcareFullRect);
            //    this.dontcareRectList[i] = dontcareRect2;
            //}

            UpdateDontcareFlag();
        }

        public void Add(Size size)
        {
            this.rectangle.Size = Size.Add(this.rectangle.Size, size);
            //for (int i = 0; i < this.dontcareRectList.Count; i++)
            //{
            //    Point pt = this.dontcareRectList[i].Location;
            //    Size sz = Size.Add(this.dontcareRectList[i].Size, size);
            //    this.dontcareRectList[i] = new Rectangle(pt, size);
            //}
        }

        public void Offset(Point point)
        {
            this.rectangle.Offset(point);
        }

        public void Clear()
        {
            this.group = -1;
            this.rectangle = Rectangle.Empty;
            this.offsetXBase = -1;
            this.offsetYBase = -1;
        }

        public void Save(XmlElement xmlElement, string key = null)
        {
            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            XmlHelper.SetValue(xmlElement, "Group", this.group);
            XmlHelper.SetValue(xmlElement, "Rectangle", this.rectangle);
            XmlHelper.SetValue(xmlElement, "OffsetXBase", this.offsetXBase);
            XmlHelper.SetValue(xmlElement, "OffsetYBase", this.offsetYBase);
            XmlHelper.SetValue(xmlElement, "GreyAverage", this.greyAverage);
            XmlHelper.SetValue(xmlElement, "Dontcare", this.dontcare);

            //Rectangle[] dontcareRects = this.dontcareRectList.ToArray();
            //XmlHelper.SetValue(xmlElement, "DontcareRects", dontcareRects);
        }

        public static InspectElement Load(XmlElement xmlElement)
        {
            int group = XmlHelper.GetValue(xmlElement, "Group", -1);

            Rectangle rectangle = Rectangle.Empty;
            XmlHelper.GetValue(xmlElement, "Rectangle", ref rectangle);

            int offsetXBase = XmlHelper.GetValue(xmlElement, "OffsetXBase", -1);
            int offsetYBase = XmlHelper.GetValue(xmlElement, "OffsetYBase", -1);
            float greyAverage = XmlHelper.GetValue(xmlElement, "GreyAverage", -1.0f);
            Rectangle dontcare = XmlHelper.GetValue(xmlElement, "Dontcare", Rectangle.Empty);

            Rectangle[] dontcareRects = null;
            XmlHelper.GetValue(xmlElement, "DontcareRects", ref dontcareRects);

            return new InspectElement(group, rectangle, offsetXBase, offsetYBase, greyAverage, dontcareRects) { Dontcare = dontcare };
        }

        internal static int GetOffsetValue(AlgoImage algoImage, Direction direction)
        {
            Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

            int foundPos = SheetFinderBase.FindBasePosition(algoImage, null, direction, 5, false);
            return foundPos;
        }

        public void Merge(InspectElement element)
        {
            Point offset = Point.Subtract(element.rectangle.Location, new Size(this.rectangle.Location));

            this.rectangle = Rectangle.Union(this.rectangle, element.rectangle);

            this.offsetXBase = (this.offsetXBase + element.offsetXBase) / 2;
            this.offsetYBase = (this.offsetYBase + element.offsetYBase) / 2;
        }

        public override string ToString()
        {
            return string.Format("Group: {0}, Rectangle: {1}", this.group, this.rectangle);
        }
    }

    public class AlignInfoCollection : List<AlignInfo>
    {
        public void Save(XmlElement xmlElement, string key = null)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            this.ForEach(f => f.Save(xmlElement, "AlignInfo"));
        }

        public void Load(XmlElement xmlElement, string key = null)
        {
            if (xmlElement == null)
                return;

            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement[key];
                Load(subElement);
                return;
            }

            Clear();
            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("AlignInfo");
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                AlignInfo inBarAlignElement = new AlignInfo((XmlElement)xmlNode);
                Add(inBarAlignElement);
            }
        }

        public AlignInfoCollection Clone()
        {
            AlignInfoCollection collenction = new AlignInfoCollection();
            this.ForEach(f => collenction.Add(f.Clone()));
            return collenction;
        }

        internal void AppendFigures(FigureGroup figureGroup, Pen pen)
        {
            this.ForEach(f => f.AppendFigures(figureGroup, pen));
        }
    }

    public class AlignInfo
    {
        public string Position { get => this.position; set => this.position = value; }
        string position = "";

        public ImageD MasterImageD => this.masterImageD;
        ImageD masterImageD = null;

        public Rectangle SearchRect { get => this.searchRect; set => this.searchRect = value; }
        Rectangle searchRect;

        public Rectangle MasterRect { get => this.masterRect; set => this.masterRect = value; }
        Rectangle masterRect;

        public AlignInfo(string position, ImageD masterImageD, Rectangle masterRect, Rectangle searchRect)
        {
            this.position = position;
            this.masterImageD = masterImageD;
            this.masterRect = masterRect;
            this.searchRect = searchRect;
        }

        public AlignInfo(XmlElement xmlElement)
        {
            Load(xmlElement);
        }

        internal void AppendFigures(FigureGroup figureGroup, Pen pen)
        {
            figureGroup.AddFigure(new RectangleFigure(masterRect, pen) { Selectable = false });
            figureGroup.AddFigure(new RectangleFigure(searchRect, pen) { Selectable = false });
        }

        public AlignInfo Clone()
        {
            AlignInfo element = new AlignInfo(this.position, this.masterImageD.Clone(), this.masterRect, this.searchRect);
            return element;
        }

        public void Save(XmlElement xmlElement, string key = null)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            XmlHelper.SetValue(xmlElement, "Position", this.position);
            XmlHelper.SetValue(xmlElement, "MasterImageD", this.masterImageD);
            XmlHelper.SetValue(xmlElement, "MasterRect", this.masterRect);
            XmlHelper.SetValue(xmlElement, "SearchRect", this.searchRect);
        }

        public void Load(XmlElement xmlElement, string key = null)
        {
            if (xmlElement == null)
                return;

            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement[key];
                Load(subElement);
                return;
            }

            this.position = XmlHelper.GetValue(xmlElement, "Position", this.position);
            this.masterImageD = XmlHelper.GetValue(xmlElement, "MasterImageD", this.masterImageD);
            this.masterRect = XmlHelper.GetValue(xmlElement, "MasterRect", Rectangle.Empty);
            this.searchRect = XmlHelper.GetValue(xmlElement, "SearchRect", Rectangle.Empty);
        }
    }

    public class RegionInfoGCollection : List<RegionInfoG>
    {
        public RegionInfoGCollection() : base() { }
        public RegionInfoGCollection(RegionInfoGCollection collection) : base(collection) { }

        public new void Clear()
        {
            this.ForEach(f => f.Dispose());
            base.Clear();
        }

        public void Save(XmlElement xmlElement)
        {
            this.ForEach(f => f.SaveParam(xmlElement, "RegionInfo"));
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
            XmlNodeList sheetPatternGroupNodeList = xmlElement.GetElementsByTagName("RegionInfo");
            foreach (XmlElement subElement in sheetPatternGroupNodeList)
            {
                RegionInfoG regionInfoG = RegionInfoG.Load(subElement);
                this.Add(regionInfoG);
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

    public class RegionInfoG : UniScanG.Data.Vision.RegionInfo
    {
        public readonly static Pen RegionPen = new Pen(Color.Yellow, 5);
        public readonly static Pen SearchRegionPen = new Pen(Color.YellowGreen, 2);
        public readonly static Pen PatRegionInsidePen = new Pen(Color.LightBlue, 1);
        public readonly static Pen PatRegionOutsidePen = new Pen(Color.CadetBlue, 1);
        public readonly static Pen InspRegionPen = new Pen(Color.FromArgb(192, Color.Red), 2);
        public readonly static Pen InspRegionPen2 = new Pen(Color.FromArgb(192, Color.Blue), 2);
        public readonly static Pen BlockRegionPen = new Pen(Color.Red, 0);
        public readonly static Brush BlockRegionBrush = new SolidBrush(Color.FromArgb(128, Color.Red));
        public readonly static Pen PassRegionPen = new Pen(Color.LightGreen, 0);
        public readonly static Brush PassRegionBrush = new SolidBrush(Color.FromArgb(128, Color.LightGreen));
        public readonly static Pen SkipRegionPen = new Pen(Color.Yellow, 15);

        public bool OddEvenPair { get => this.oddEvenPair; set => this.oddEvenPair = value; }
        bool oddEvenPair = false;

        public bool AdvancedPair { get => this.advancedPair; set => this.advancedPair = value; }
        bool advancedPair = false;

        public ImageD ThumbnailImageD => this.thumbnail;
        ImageD thumbnail = null;

        public Rectangle[,] AdjPatRegionList => this.adjPatRegionList;
        Rectangle[,] adjPatRegionList = null;

        public Rectangle[,] PatRegionList => this.patRegionList;
        Rectangle[,] patRegionList = null;

        public List<InspectElement> InspectElementList => this.inspectElementList;
        List<InspectElement> inspectElementList = null;

        public List<Point> DontcarePatLocationList => this.dontcarePatLocationList;
        List<Point> dontcarePatLocationList = null;

        public List<Rectangle> BlockRectList => this.blockRectList;
        List<Rectangle> blockRectList = new List<Rectangle>();

        public List<Rectangle> PassRectList => this.passRectList;
        List<Rectangle> passRectList = new List<Rectangle>();

        public List<Rectangle> CreticalPointList => this.creticalPointList;
        List<Rectangle> creticalPointList = new List<Rectangle>();

        public GroupDirection GroupDirection { get => this.groupDirection; set => this.groupDirection = value; }
        GroupDirection groupDirection = GroupDirection.Vertical;

        public bool IgnoreInnerChip { get => this.ignoreInnerChip; set => this.ignoreInnerChip = value; }
        bool ignoreInnerChip = false;

        public float TotalSlope { get => this.slope * this.patRegionList.GetLength(1); }

        public float Slope { get => this.slope; set => this.slope = value; }
        float slope;

        public float PoleAvg => this.poleAvg;
        float poleAvg;

        public float DielectricAvg => this.dielectricAvg;
        float dielectricAvg;

        public AlignInfoCollection AlignInfoCollection => this.alignInfoCollection;
        AlignInfoCollection alignInfoCollection = new AlignInfoCollection();

        private RegionInfoG() : base()
        {
            this.use = true;
            this.oddEvenPair = false;
            //this.trainImage = null;
            //this.patternImage = null;
            this.adjPatRegionList = new Rectangle[0, 0];
            this.patRegionList = new Rectangle[0, 0];
            this.inspectElementList = new List<InspectElement>();
            this.dontcarePatLocationList = new List<Point>();
            this.slope = 0;
            this.poleAvg = -1;
            this.dielectricAvg = -1;
            this.GroupDirection = GroupDirection.Vertical;
            this.IgnoreInnerChip = false;
        }

        public RegionInfoG(Rectangle region, Rectangle inflateSize, ImageD thumbnail, AlignInfo[] alignInfos, Rectangle[,] subRegionList, Rectangle[,] adjustSubRegionList, float poleAvg, float dielectricAvg)
            : base(region, inflateSize)
        {
            this.patRegionList = subRegionList;
            this.adjPatRegionList = adjustSubRegionList;
            this.inspectElementList = new List<InspectElement>();
            this.dontcarePatLocationList = new List<Point>();
            //this.trainImage = trainImage.Resize(imageScale, imageScale);
            this.thumbnail = thumbnail;
            //this.imageScale = imageScale;
            this.poleAvg = poleAvg;
            this.dielectricAvg = dielectricAvg;
            this.alignInfoCollection.AddRange(alignInfos);
        }

        public void Offset(Point offset, AlgoImage traginImage)
        {
            this.region.Offset(offset);

            for (int x = 0; x < this.adjPatRegionList.GetLength(1); x++)
                for (int y = 0; y < this.adjPatRegionList.GetLength(0); y++)
                    this.adjPatRegionList[y, x].Offset(offset);

            for (int x = 0; x < this.patRegionList.GetLength(1); x++)
                for (int y = 0; y < this.patRegionList.GetLength(0); y++)
                    this.patRegionList[y, x].Offset(offset);

            this.inspectElementList.ForEach(f => f.Offset(offset));
        }

        internal void BuildInspRegion(ImageD trainImageD, ImageD majorPatternImageD)
        {
            Debug.Assert(trainImageD.Size == majorPatternImageD.Size);
            Rectangle imageRect = new Rectangle(Point.Empty, trainImageD.Size);
            this.inspectElementList.Clear();


            // 기본 검사 줄 생성(가로/세로)
            List<InspectElement> inspElementList = new List<InspectElement>();
            Rectangle[,] workRect = this.adjPatRegionList;
            switch (this.GroupDirection)
            {
                case GroupDirection.Horizontal:
                    {
                        int loopY = this.adjPatRegionList.GetLength(0);
                        for (int y = 0; y < loopY; y++)
                        {
                            List<Rectangle> adjPatRectList = new List<Rectangle>();
                            List<Rectangle> patRectList = new List<Rectangle>();

                            int loopX = this.adjPatRegionList.GetLength(1);
                            for (int x = 0; x < loopX; x++)
                            {
                                Rectangle rect = this.adjPatRegionList[y, x];
                                if (rect.IsEmpty == false)
                                {
                                    adjPatRectList.Add(this.adjPatRegionList[y, x]);
                                    patRectList.Add(this.patRegionList[y, x]);
                                }
                            }

                            Rectangle unionRect = Rectangle.FromLTRB(
                                (imageRect.Left + patRectList.Min(f => f.Left)) / 2,
                                adjPatRectList.Min(f => f.Top),
                                (imageRect.Right + patRectList.Max(f => f.Right)) / 2,
                                adjPatRectList.Max(f => f.Bottom)
                                );

                            inspElementList.Add(new InspectElement(0, unionRect, -1, -1, -1));
                        }

                        int width = inspElementList[0].Rectangle.Width;
                        Debug.Assert(inspElementList.TrueForAll(f => f.Rectangle.Width == width));
                    }
                    break;

                case GroupDirection.Vertical:
                    {
                        List<Rectangle> rectangelList = new List<Rectangle>();
                        int loopX = this.adjPatRegionList.GetLength(1);
                        for (int x = 0; x < loopX; x++)
                        {
                            List<Rectangle> adjPatRectList = new List<Rectangle>();
                            List<Rectangle> patRectList = new List<Rectangle>();
                            int loopY = this.adjPatRegionList.GetLength(0);
                            for (int y = 0; y < loopY; y++)
                            {
                                Rectangle rect = this.adjPatRegionList[y, x];
                                if (rect.IsEmpty == false)
                                {
                                    adjPatRectList.Add(this.adjPatRegionList[y, x]);
                                    patRectList.Add(this.patRegionList[y, x]);
                                }
                            }

                            Rectangle unionRect = Rectangle.FromLTRB(
                                adjPatRectList.Min(f => f.Left),
                                patRectList.Min(f => f.Top),
                                adjPatRectList.Max(f => f.Right),
                                patRectList.Max(f => f.Bottom)
                                );

                            rectangelList.Add(unionRect);
                        }
                        int top = (imageRect.Top + rectangelList.Min(f => f.Top)) / 2;
                        int bottom = (imageRect.Bottom + rectangelList.Max(f => f.Bottom)) / 2;
                        inspElementList.AddRange(rectangelList.Select(f => new InspectElement(0, Rectangle.FromLTRB(f.Left, top, f.Right, bottom), -1, -1, -1)));

                        int height = inspElementList[0].Rectangle.Height;
                        Debug.Assert(inspElementList.TrueForAll(f => f.Rectangle.Height == height));
                    }
                    break;

                case GroupDirection.None:
                    int idx = 0;
                    for (int y = 0; y < this.adjPatRegionList.GetLength(0); y++)
                    {
                        for (int x = 0; x < this.adjPatRegionList.GetLength(1); x++)
                        {
                            Rectangle rect = this.adjPatRegionList[y, x];
                            //bool isDontcare = this.dontcarePatLocationList.Contains(new Point(x, y));
                            //Rectangle[] dontCareRect = !isDontcare ? new Rectangle[0] : new Rectangle[1] { new Rectangle(Point.Empty, rectangle.Size) };

                            //inspElementList.Add(new InspectElement(y, rectangle, -1, -1, -1, dontCareRect));
                            inspElementList.Add(new InspectElement(y, rect, -1, -1, -1));
                        }
                    }
                    break;
            }

            // 크기 정렬 (같은 크기, 패턴 위치 동일하게)
            // 가로 길이를 4의 배수로 맞춤.???
            SizeF inspRegionMaxSize = new SizeF((float)inspElementList.Max(f => f.Rectangle.Width), (float)inspElementList.Max(f => f.Rectangle.Height));
            SizeF inspRegionMeanSize = new SizeF((float)inspElementList.Average(f => f.Rectangle.Width), (float)inspElementList.Average(f => f.Rectangle.Height));
            Size inspRegionSize = Size.Round(new SizeF((inspRegionMeanSize.Width + inspRegionMaxSize.Width) / 2, (inspRegionMeanSize.Height + inspRegionMaxSize.Height) / 2));
            inspRegionSize = Size.Ceiling(inspRegionMaxSize);
            //inspRegionSize.Width = ((inspRegionSize.Width + 3) / 4) * 4;

            for (int i = 0; i < inspElementList.Count; i++)
            {
                InspectElement f = inspElementList[i];
                Size diff = new Size(inspRegionSize.Width - f.Rectangle.Width, inspRegionSize.Height - f.Rectangle.Height);
                Size diff1 = new Size(diff.Width / 2, diff.Height / 2);
                Size diff2 = new Size(diff.Width % 2, diff.Height % 2);

                f.Inflate(diff1);
                f.Add(diff2);
            }

            // 그룹 방향에 수직인 방향 보정.
            AlgoImage trainImage = null;
            AlgoImage majorPatternImage = null;
            try
            {
                trainImage = ImageBuilder.Build(TrainerBase.TypeName, trainImageD, ImageType.Grey);
                majorPatternImage = ImageBuilder.Build(TrainerBase.TypeName, majorPatternImageD, ImageType.Grey);

                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(majorPatternImage);
                Rectangle baseRect = inspElementList[inspElementList.Count / 2].Rectangle;
                if (Rectangle.Intersect(imageRect, baseRect) != baseRect)
                {
                    this.inspectElementList.Clear();
                    return;
                }
                using (AlgoImage subImageBase = majorPatternImage.GetSubImage(baseRect))
                {
                    Direction projDirection = this.groupDirection == GroupDirection.Horizontal ? Direction.Vertical : Direction.Horizontal;
                    float[] projData = ip.Projection(subImageBase, projDirection, ProjectionType.Mean);

                    for (int i = 0; i < inspElementList.Count; i++)
                    {
                        InspectElement inspRegionElement = inspElementList[i];
                        Rectangle intersectRect = Rectangle.Intersect(imageRect, inspRegionElement.Rectangle);
                        if (intersectRect == inspRegionElement.Rectangle)
                        {
                            //subRect.Inflate(0, -baseRect.Height / 4);
                            AlgoImage subTrainImage = trainImage.GetSubImage(inspRegionElement.Rectangle);
                            AlgoImage subPatternImage = majorPatternImage.GetSubImage(inspRegionElement.Rectangle);
                            float[] projData2 = ip.Projection(subPatternImage, projDirection, ProjectionType.Mean);
                            float offset = AlgorithmCommon.Correlation(projData, projData2);
                            PointF pt = Point.Empty;
                            switch (this.groupDirection)
                            {
                                case GroupDirection.Horizontal:
                                    pt.Y = -offset;
                                    break;
                                case GroupDirection.Vertical:
                                    pt.X = -offset;
                                    break;
                            }
                            inspRegionElement.Offset(Point.Round(pt));
                            subTrainImage.Dispose();
                            subPatternImage.Dispose();

                            intersectRect = Rectangle.Intersect(imageRect, inspRegionElement.Rectangle);
                            if (intersectRect == inspRegionElement.Rectangle)
                            {
                                Rectangle rectangle = inspRegionElement.Rectangle;
                                Rectangle lineImagerect = Rectangle.FromLTRB(rectangle.Left, 0, rectangle.Right, rectangle.Height);
                                using (AlgoImage lineImage = trainImage.GetSubImage(lineImagerect))
                                {
                                    switch (this.groupDirection)
                                    {
                                        case GroupDirection.Horizontal:
                                            inspElementList[i].OffsetXBase = InspectElement.GetOffsetValue(lineImage, Direction.Horizontal);
                                            pt.Y = -offset;
                                            break;
                                        case GroupDirection.Vertical:
                                            inspElementList[i].OffsetYBase = InspectElement.GetOffsetValue(lineImage, Direction.Vertical);
                                            //lineImage.Save(@"C:\temp\lineImage.png");
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                inspElementList[i].Clear();
                            }
                        }
                        else
                        {
                            inspElementList[i].Clear();
                        }
                        ////검증
                        //AlgoImage subAlgoImage2 = algoImage.GetSubImage(subRect);
                        //float[] projData3 = ip.Projection(subAlgoImage2, projDirection, ProjectionType.Mean);
                        //int offset2 = (int)Math.Round(AlgorithmCommon.Correlation(projData, projData3));
                        //subAlgoImage2.Dispose();
                        //if(offset2 != 0)
                        //{
                        //    LogHelper.Debug(LoggerType.Operation, "RegionInfoG::BuildInspRegion offset2 is not 0");
                        //}

                    }

                    if (true)
                    // 그룹 방향 기울어짐 보정
                    {
                        // x
                        double fX = 0;
                        Point[] xPoints = inspElementList.FindAll(f => f.OffsetXBase >= 0).Select(f => new Point(inspElementList.IndexOf(f), f.OffsetXBase)).ToArray();
                        if (xPoints.Length > 0)
                        {
                            double xMeanX = xPoints.Average(f => f.X);
                            double xMeanY = xPoints.Average(f => f.Y);
                            fX = xPoints.Select(f => (f.X - xMeanX) * (f.Y - xMeanY)).Sum() / xPoints.Select(f => Math.Pow(f.X - xMeanX, 2)).Sum();
                        }

                        // y
                        double fY = 0;
                        Point[] yPoints = inspElementList.FindAll(f => f.OffsetYBase >= 0).Select(f => new Point(inspElementList.IndexOf(f), f.OffsetYBase)).ToArray();
                        if (yPoints.Length > 0)
                        {
                            //double yMeanX = yPoints.Average(f => f.X);
                            //double yMeanY = yPoints.Average(f => f.Y);
                            //fY = yPoints.Select(f => (f.X - yMeanX) * (f.Y - yMeanY)).Sum() / yPoints.Select(f => Math.Pow(f.X - yMeanX, 2)).Sum();
                            fY = this.slope;
                        }

                        int center = inspElementList.Count / 2;
                        for (int i = 0; i < inspElementList.Count; i++)
                        {
                            InspectElement inspRegionElement = inspElementList[i];

                            float oX = (float)(fX * (i - center));
                            float oY = (float)(fY * (i - center));
                            Point offset = Point.Round(new PointF(oX, oY));
                            inspRegionElement.Offset(offset);

                            if (inspRegionElement.Rectangle != Rectangle.Intersect(imageRect, inspRegionElement.Rectangle))
                                inspRegionElement.Clear();
                        }
                    }
                }
                inspElementList.RemoveAll(f => f.Rectangle.Size.IsEmpty);
                if (inspElementList.Count <= 3)
                    return;

                // 검사영역에서 빠진 부분(갭)이 없도록 영역을 넓힘
                List<int> gap = new List<int>();
                switch (this.groupDirection)
                {
                    case GroupDirection.Horizontal:
                        inspElementList.Aggregate((f, g) => { gap.Add(g.Rectangle.Top - f.Rectangle.Bottom); return g; });
                        break;
                    case GroupDirection.Vertical:
                        inspElementList.Aggregate((f, g) => { gap.Add(g.Rectangle.Left - f.Rectangle.Right); return g; });
                        break;
                }

                int maxGap = gap.Count > 0 ? gap.Max() : -1;
                if (maxGap > 0)
                {
                    int inflate = (int)Math.Ceiling(maxGap / 2.0);
                    for (int i = 0; i < this.inspectElementList.Count; i++)
                    {
                        switch (this.groupDirection)
                        {
                            case GroupDirection.Horizontal:
                                inspElementList[i].Inflate(new Size(0, inflate));
                                break;
                            case GroupDirection.Vertical:
                                inspElementList[i].Inflate(new Size(inflate, 0));
                                break;
                        }
                    }
                }

                Rectangle regionRect = new Rectangle(Point.Empty, this.region.Size);
                inspElementList.RemoveAll(f => f.Rectangle != Rectangle.Intersect(f.Rectangle, regionRect));
                inspElementList.ForEach(f =>
                {
                    using (AlgoImage subImage = trainImage.GetSubImage(f.Rectangle))
                    {
                        f.GreyAverage = ip.GetGreyAverage(subImage);
                    }
                });
                //Debug.Assert(inspElementList.All(f => f.Rectangle.Width % 4 == 0));

                this.inspectElementList = inspElementList;

                if (this.IgnoreInnerChip)
                {
                    int div = 6;
                    this.inspectElementList.ForEach(f =>
                    {
                        switch (this.groupDirection)
                        {
                            case GroupDirection.Horizontal:
                                f.Dontcare = Rectangle.Inflate(new Rectangle(Point.Empty, f.Rectangle.Size), 0, -f.Rectangle.Height / div);
                                break;

                            case GroupDirection.Vertical:
                                f.Dontcare = Rectangle.Inflate(new Rectangle(Point.Empty, f.Rectangle.Size), -f.Rectangle.Width / div, 0);
                                break;

                            case GroupDirection.None:
                            default:
                                f.Dontcare = Rectangle.Inflate(new Rectangle(Point.Empty, f.Rectangle.Size), -f.Rectangle.Width / div, -f.Rectangle.Height / div);
                                break;
                        }
                    });
                }
            }
            finally
            {
                trainImage?.Dispose();
                majorPatternImage?.Dispose();
            }
        }

        public bool AlignX(AlgoImage regionAlgoImage)
        {
            Rectangle imageRect = new Rectangle(Point.Empty, regionAlgoImage.Size);
            Size patSize = new Size(this.patRegionList.GetLength(1), this.patRegionList.GetLength(0));
            int centerPatPos = patSize.Height / 2;

            Rectangle rect = this.patRegionList[centerPatPos, 0];
            for (int i = 1; i < patSize.Width; i++)
                rect = Rectangle.Union(rect, this.patRegionList[centerPatPos, i]);
            rect.Intersect(imageRect);

            AlgoImage processImage = regionAlgoImage.GetSubImage(rect);
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(processImage);
            float[] data = ip.Projection(processImage, Direction.Horizontal, ProjectionType.Mean);
            List<Point> hillList = AlgorithmCommon.FindHill(data, data.Average());
            if (patSize.Width != hillList.Count)
                return false;

            return true;
        }

        public override void Dispose()
        {
            base.Dispose();

            //trainImage?.Dispose();
            //trainImage = null;

            //patternImage?.Dispose();
            //patternImage = null;

            this.inspectElementList = null;
            this.patRegionList = null;
            this.dontcarePatLocationList = null;
        }

        public static RegionInfoG Load(XmlElement xmlElement)
        {
            RegionInfoG regionInfoG = new RegionInfoG();
            regionInfoG.LoadParam(xmlElement);
            return regionInfoG;
        }

        public override UniScanG.Data.Vision.RegionInfo Clone()
        {
            RegionInfoG clone = new RegionInfoG();
            clone.Copy(this);
            return clone;
        }

        public override void Copy(UniScanG.Data.Vision.RegionInfo srcRegionInfo)
        {
            base.Copy(srcRegionInfo);

            RegionInfoG regionInfo = srcRegionInfo as RegionInfoG;

            this.oddEvenPair = regionInfo.oddEvenPair;
            this.advancedPair = regionInfo.advancedPair;
            this.thumbnail = regionInfo.thumbnail?.Clone();
            this.alignInfoCollection = regionInfo.alignInfoCollection.Clone();
            this.patRegionList = new Rectangle[regionInfo.patRegionList.GetLength(0), regionInfo.patRegionList.GetLength(1)];
            Array.Copy(regionInfo.patRegionList, this.patRegionList, this.patRegionList.Length);

            this.adjPatRegionList = new Rectangle[regionInfo.adjPatRegionList.GetLength(0), regionInfo.adjPatRegionList.GetLength(1)];
            Array.Copy(regionInfo.adjPatRegionList, this.adjPatRegionList, this.adjPatRegionList.Length);

            this.blockRectList.Clear();
            this.blockRectList.AddRange(regionInfo.blockRectList);
            this.passRectList.Clear();
            this.passRectList.AddRange(regionInfo.passRectList);
            this.creticalPointList.Clear();
            this.creticalPointList.AddRange(regionInfo.creticalPointList);

            this.inspectElementList = new List<InspectElement>(regionInfo.inspectElementList);
            this.dontcarePatLocationList = new List<Point>(regionInfo.dontcarePatLocationList);

            this.groupDirection = regionInfo.groupDirection;

            this.slope = regionInfo.slope;
            this.poleAvg = regionInfo.poleAvg;
            this.dielectricAvg = regionInfo.dielectricAvg;
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            //if (this.trainImage != null)
            //{
            //    Bitmap bitmap = trainImage.ToBitmap();
            //    string imageString = ImageHelper.BitmapToBase64String(bitmap);
            //    bitmap.Dispose();
            //    XmlHelper.SetValue(algorithmElement, "TrainImage", imageString);
            //}

            if (this.thumbnail != null)
            {
                Bitmap bitmap = this.thumbnail.ToBitmap();
                string imageString = ImageHelper.BitmapToBase64String(bitmap);
                bitmap.Dispose();
                XmlHelper.SetValue(algorithmElement, "Thumbnail", imageString);
            }

            XmlHelper.SetValue(algorithmElement, "OddEvenPair", this.oddEvenPair.ToString());
            XmlHelper.SetValue(algorithmElement, "PatRegionList", this.patRegionList);
            XmlHelper.SetValue(algorithmElement, "AdjPatRegionList", this.adjPatRegionList);

            XmlElement inspRegionListElement = algorithmElement.OwnerDocument.CreateElement("InspRegionList2");
            algorithmElement.AppendChild(inspRegionListElement);
            //this.inspRegionList.ForEach(f => f.Save(inspRegionListElement, "InspRegion"));
            foreach (InspectElement inspRegion in this.inspectElementList)
            {
                inspRegion.Save(inspRegionListElement, "InspRegion");
            }

            XmlHelper.SetValue(algorithmElement, "DontCareLocationList", this.dontcarePatLocationList.ToArray());
            XmlHelper.SetValue(algorithmElement, "DontcareRectList", this.blockRectList.ToArray());
            XmlHelper.SetValue(algorithmElement, "PassRectList", this.passRectList.ToArray());
            XmlHelper.SetValue(algorithmElement, "CreticalPointList", this.creticalPointList.ToArray());

            XmlHelper.SetValue(algorithmElement, "GroupDirection", this.groupDirection);
            XmlHelper.SetValue(algorithmElement, "Slope", this.slope);
            XmlHelper.SetValue(algorithmElement, "IgnoreInnerChip", this.ignoreInnerChip);

            XmlHelper.SetValue(algorithmElement, "PoleAvg", poleAvg.ToString());
            XmlHelper.SetValue(algorithmElement, "DielectricAvg", dielectricAvg.ToString());

            this.alignInfoCollection.Save(algorithmElement, "AlignInfoCollection");
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            string thumbnailImageString = XmlHelper.GetValue(algorithmElement, "Thumbnail", "");
            if (string.IsNullOrEmpty(thumbnailImageString) == false)
            {
                Bitmap bitmap = ImageHelper.Base64StringToBitmap(thumbnailImageString);
                this.thumbnail = Image2D.FromBitmap(bitmap);
            }

            this.oddEvenPair = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "OddEvenPair", this.oddEvenPair.ToString()));
            XmlHelper.GetValue(algorithmElement, "PatRegionList", ref this.patRegionList);
            XmlHelper.GetValue(algorithmElement, "AdjPatRegionList", ref this.adjPatRegionList);

            this.inspectElementList = new List<InspectElement>();
            bool isRecentModel = XmlHelper.Exist(algorithmElement, "InspRegionList2");
            if (isRecentModel == false)
                return;

            XmlElement inspRegionListElement = algorithmElement["InspRegionList2"];
            XmlNodeList xmlNodeList = inspRegionListElement.SelectNodes("InspRegion");
            foreach (XmlElement xmlElement in xmlNodeList)
            {
                InspectElement inspRegion = InspectElement.Load(xmlElement);
                inspectElementList.Add(inspRegion);
            }

            Point[] points = null;
            if (XmlHelper.GetValue(algorithmElement, "DontCareLocationList", ref points) == false)
                XmlHelper.GetValue(algorithmElement, "SkipRegions", ref points);
            this.dontcarePatLocationList = new List<Point>(points);

            Rectangle[] rectangles = null;
            XmlHelper.GetValue(algorithmElement, "DontcareRectList", ref rectangles);
            this.blockRectList = new List<Rectangle>(rectangles);

            XmlHelper.GetValue(algorithmElement, "PassRectList", ref rectangles);
            this.passRectList = new List<Rectangle>(rectangles);

            XmlHelper.GetValue(algorithmElement, "CreticalPointList", ref rectangles);
            this.creticalPointList = new List<Rectangle>(rectangles);
            SortCreticalPointList();
            //for (int i = 0; i < this.creticalPointList.Count; i++)
            //{
            //    Rectangle rect = this.creticalPointList[i];
            //    int len = Math.Max(rect.Width, rect.Height);
            //    int diffX = (len - rect.Width) / 2;
            //    int diffY = (len - rect.Height) / 2;
            //    rect = Rectangle.Inflate(rect, diffX, diffY);
            //    rect.Width = rect.Height = Math.Max(rect.Width, rect.Height);
            //    this.creticalPointList[i] = rect;
            //}
            //imageScale = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "ImageScale", imageScale.ToString()));

            this.groupDirection = XmlHelper.GetValue(algorithmElement, "GroupDirection", GroupDirection.Vertical);
            this.slope = XmlHelper.GetValue(algorithmElement, "Slope", this.slope);
            this.ignoreInnerChip = XmlHelper.GetValue(algorithmElement, "IgnoreInnerChip", this.ignoreInnerChip);

            this.poleAvg = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "PoleAvg", "-1"));
            this.dielectricAvg = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "DielectricAvg", "-1"));

            this.alignInfoCollection.Load(algorithmElement, "AlignInfoCollection");
        }

        public override Figure GetFigure(bool drawDetail)
        {
            //bool drawDetail = DynMvp.Authentication.UserHandler.Instance().CurrentUser.SuperAccount;
            Rectangle baseRegion = new Rectangle(Point.Empty, region.Size);

            FigureGroup figureGroup = new FigureGroup();
            //figureGroup.AddFigure(new RectangleFigure(baseRegion, RegionPen, new SolidBrush(Color.FromArgb(50, Color.LightYellow))) { Selectable = false });
            figureGroup.AddFigure(new RectangleFigure(baseRegion, RegionPen) { Selectable = false });

            if (drawDetail)
                this.alignInfoCollection.AppendFigures(figureGroup, SearchRegionPen);
            //figureGroup.AddFigure(new RectangleFigure(SearchRect, SearchRegionPen));
            //figureGroup.AddFigure(new RectangleFigure(masterRect, SearchRegionPen));

            if (drawDetail)
            {
                if (this.patRegionList != null)
                {
                    foreach (Rectangle patRegion in this.patRegionList)
                        figureGroup.AddFigure(new RectangleFigure(patRegion, PatRegionInsidePen) { Selectable = false });
                }

                if (this.adjPatRegionList != null)
                {
                    foreach (Rectangle adjPatRegion in this.adjPatRegionList)
                        figureGroup.AddFigure(new RectangleFigure(adjPatRegion, PatRegionOutsidePen) { Selectable = false });
                }
            }

            if (true || drawDetail)
            {
                this.inspectElementList?.ForEach(f =>
                {
                    Point loc = f.Rectangle.Location;
                    //f.DontcareRectList.ForEach(g =>
                    //{
                    //    Rectangle rect = DrawingHelper.Offset(g, loc);
                    //    figureGroup.AddFigure(new RectangleFigure(rect, BlockRegionPen, BlockRegionBrush) { Selectable = false });
                    //});
                });
            }
            //else
            {
                if (this.dontcarePatLocationList != null)
                {
                    foreach (Point skipRegion in this.dontcarePatLocationList)
                    {
                        Rectangle rectangle = this.adjPatRegionList[skipRegion.Y, skipRegion.X];

                        //FigureGroup fg = new FigureGroup() { Selectable = false };
                        //fg.AddFigure(new RectangleFigure(rectangle, SkipRegionPen, SkipRegionBrush));
                        //fg.AddFigure(new LineFigure(new Point(rectangle.Left, rectangle.Top), new Point(rectangle.Right, rectangle.Bottom), SkipRegionPen));
                        //fg.AddFigure(new LineFigure(new Point(rectangle.Right, rectangle.Top), new Point(rectangle.Left, rectangle.Bottom), SkipRegionPen));
                        //figureGroup.AddFigure(fg);

                        figureGroup.AddFigure(new RectangleFigure(rectangle, BlockRegionPen, BlockRegionBrush) { Selectable = false });
                        //figureGroup.AddFigure(new LineFigure(new Point(rectangle.Left, rectangle.Top), new Point(rectangle.Right, rectangle.Bottom), BlockRegionPen) { Selectable = false });
                        //figureGroup.AddFigure(new LineFigure(new Point(rectangle.Right, rectangle.Top), new Point(rectangle.Left, rectangle.Bottom), BlockRegionPen) { Selectable = false });
                    }
                }
            }

            CalculatorModelParam calculatorModelParam = CalculatorBase.CalculatorParam.ModelParam;
            if (calculatorModelParam != null)
            {
                Rectangle[] boundaryRect = GetBlockRects(calculatorModelParam.BarBoundary);

                //FigureGroup boundaryFg = new FigureGroup() { Selectable = false };
                //Array.ForEach(boundaryRect, f => boundaryFg.AddFigure(new RectangleFigure(new RotatedRect(f, 0), SkipRegionPen, SkipRegionBrush)));
                //figureGroup.AddFigure(boundaryFg);
                Array.ForEach(boundaryRect, f => figureGroup.AddFigure(new RectangleFigure(new RotatedRect(f, 0), BlockRegionPen, BlockRegionBrush) { Selectable = false }));
            }

            if (this.inspectElementList != null)
            {
                for (int i = 0; i < this.inspectElementList.Count; i++)
                {
                    InspectElement inspRegion = this.inspectElementList[i];
                    if (drawDetail)
                    {
                        if (i % 2 == 0)

                            figureGroup.AddFigure(new RectangleFigure(DrawingHelper.Offset(inspRegion.Rectangle, new Size(0, i % 3 - 1)), InspRegionPen) { Selectable = false });
                        else
                            figureGroup.AddFigure(new RectangleFigure(DrawingHelper.Offset(inspRegion.Rectangle, new Size(0, i % 3 - 1)), InspRegionPen2) { Selectable = false });
                    }

                    figureGroup.AddFigure(new RectangleFigure(DrawingHelper.Offset(inspRegion.Dontcare, inspRegion.Rectangle.Location), BlockRegionPen, BlockRegionBrush));
                }
            }


            this.blockRectList?.ForEach(f => figureGroup.AddFigure(new RectangleFigure(f, BlockRegionPen, BlockRegionBrush) { Tag = new Tuple<string, Rectangle>("Block", f) }));

            this.passRectList?.ForEach(f =>
            {
                figureGroup.AddFigure(new RectangleFigure(f, PassRegionPen, PassRegionBrush) { Tag = new Tuple<string, Rectangle>("Pass", f) });
                figureGroup.AddFigure(new TextFigure(this.passRectList.IndexOf(f).ToString(), f.Location, new Font("맑은  고딕", 5), PassRegionPen.Color) { Tag = new Tuple<string, Rectangle>("Cretical", f) });
            });

            this.creticalPointList?.ForEach(f =>
            {
                figureGroup.AddFigure(new RectangleFigure(new RotatedRect(f, 45f), BlockRegionPen) { Tag = new Tuple<string, Rectangle>("Cretical", f) });
                figureGroup.AddFigure(new TextFigure(this.creticalPointList.IndexOf(f).ToString(), f.Location, new Font("맑은  고딕", 5), BlockRegionPen.Color) { Tag = new Tuple<string, Rectangle>("Cretical", f) });
            });

            if (!this.use)
            {
                figureGroup.AddFigure(new LineFigure(new PointF(baseRegion.Left, baseRegion.Top), new PointF(baseRegion.Right, baseRegion.Bottom), SkipRegionPen) { Selectable = false });
                figureGroup.AddFigure(new LineFigure(new PointF(baseRegion.Left, baseRegion.Bottom), new PointF(baseRegion.Right, baseRegion.Top), SkipRegionPen) { Selectable = false });
            }

            return figureGroup;
        }

        public Rectangle[] GetBlockRects(SizeF boundarySizeMul)
        {
            List<Rectangle> rectList = new List<Rectangle>();

            // Block
            rectList.AddRange(this.blockRectList);

            // Boundary
            Rectangle baseRegion = new Rectangle(Point.Empty, base.region.Size);
            Rectangle inflateSize = Rectangle.Round(DrawingHelper.Mul(this.inflateSize, boundarySizeMul));
            Rectangle inflateRegion = Rectangle.FromLTRB(
                baseRegion.Left + inflateSize.Left,
                baseRegion.Top + inflateSize.Top,
                baseRegion.Right - inflateSize.Right,
                baseRegion.Bottom - inflateSize.Bottom);

            Rectangle[] rectangles = new Rectangle[4]
            {
                // left
                Rectangle.FromLTRB(baseRegion.Left, baseRegion.Top, inflateRegion.Left,inflateRegion.Bottom),
                // Top
                Rectangle.FromLTRB(inflateRegion.Left, baseRegion.Top, baseRegion.Right,inflateRegion.Top),
                // right
                Rectangle.FromLTRB(inflateRegion.Right, inflateRegion.Top, baseRegion.Right,baseRegion.Bottom),
                // bottom
                Rectangle.FromLTRB(baseRegion.Left, inflateRegion.Bottom, inflateRegion.Right,baseRegion.Bottom)
            };
            rectList.AddRange(Array.FindAll(rectangles, f => f.Width > 0 && f.Height > 0));

            return rectList.ToArray();
        }

        public Rectangle[] GetDontcareRects()
        {
            List<Rectangle> dontcareRectList = new List<Rectangle>();
            this.dontcarePatLocationList.ForEach(f =>
            {
                dontcareRectList.Add(this.adjPatRegionList[f.Y, f.X]);
            });
            return dontcareRectList.ToArray();
        }

        public void SetSkipRegion(AlgoImage regionPatternImage)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(regionPatternImage);
            Rectangle imageRect = new Rectangle(Point.Empty, regionPatternImage.Size);
            //regionPatternImage.Save(@"C:\temp\regionPatternImage.bmp");

            //this.dontcarePatLocationList.Clear();
            for (int i = 0; i < this.patRegionList.GetLength(0); i++)
            {
                for (int j = 0; j < this.patRegionList.GetLength(1); j++)
                {
                    Rectangle patRect = this.patRegionList[i, j];
                    //Debug.Assert(subRect.Width > 0 && subRect.Height > 0);
                    if (patRect.Width <= 0 || patRect.Height <= 0 || Rectangle.Intersect(patRect, imageRect) != patRect)
                    {
                        //this.dontcarePatLocationList.Add(new Point(j, i));
                        continue;
                    }
                    using (AlgoImage subAlgoImage = regionPatternImage.GetSubImage(patRect))
                    {
                        int blk, gry, wht;
                        ip.Count(subAlgoImage, out blk, out gry, out wht);
                        float rate = wht * 1.0f / (wht + gry + blk);
                        //LogHelper.Debug(LoggerType.Inspection, $"RegionInfoG::SetSkipRegion - x: {j} y: {i} r: {rate}");
                        if (rate < 0.4)
                        {
                            Point location = new Point(j, i);
                            if(!this.dontcarePatLocationList.Contains(location))
                                this.dontcarePatLocationList.Add(location);

                            if (patRect.Width > patRect.Height * 1.2)
                            {
                                // 가로로 긴 패턴은 한칸 밑도 DontCare 추가.
                                Point location2 = new Point(j, i + 1);
                                if (!this.dontcarePatLocationList.Contains(location2))
                                    this.dontcarePatLocationList.Add(location2);
                            }
                        }
                    }
                }
            }
            //this.dontcarePatLocationList.RemoveAll(f => f.Y >= this.adjPatRegionList.GetLength(0) || f.X >= this.adjPatRegionList.GetLength(1));
        }

        public ImageD BuildSkippedImage()
        {
            //Image2D skippedImage = new Image2D(this.region.Width, this.region.Height, 1);

            //for (int y = 0; y < this.patRegionList.GetLength(0); y++)
            //{
            //    for (int x = 0; x < this.patRegionList.GetLength(1); x++)
            //    {
            //        Rectangle patRegion = this.patRegionList[y, x];
            //        bool isSkipped = this.skipPoints.Exists(f => f.X == x && f.Y == y);
            //        if (isSkipped)
            //            continue;

            //        Image2D subSkippedImage = new Image2D(patRegion.Width, patRegion.Height, 1);
            //        subSkippedImage.Clear();
            //        skippedImage.CopyFrom();

            //    }
            //}
            return null;
        }

        public Rectangle GetPatRect()
        {
            Rectangle patRect = this.patRegionList[0, 0];
            foreach (Rectangle patRegion in this.patRegionList)
                patRect = Rectangle.Union(patRect, patRegion);

            return patRect;
        }

        public string GetInfoString()
        {
            SizeF pelSize = new SizeF(14, 14);
            Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
            if (calibration != null)
                pelSize = calibration.PelSize;

            float widthMm = this.region.Width * pelSize.Width / 1000;
            float heigthMm = this.region.Height * pelSize.Height / 1000;
            float algleDeg = (float)(Math.Atan2(this.TotalSlope * pelSize.Width / 1000, widthMm) / Math.PI * 180);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("{0} [mm]", StringManager.GetString(this.GetType().FullName, "Width")));
            sb.AppendLine(widthMm.ToString("F2"));

            sb.AppendLine(string.Format("{0} [mm]", StringManager.GetString(this.GetType().FullName, "Height")));
            sb.AppendLine(heigthMm.ToString("F2"));

            sb.AppendLine(string.Format("{0} [EA]", StringManager.GetString(this.GetType().FullName, "Count")));
            sb.AppendLine(this.inspectElementList.Count.ToString());

            sb.AppendLine(string.Format("{0} [deg]", StringManager.GetString(this.GetType().FullName, "Rotate")));
            sb.AppendLine(algleDeg.ToString("F2"));
            return sb.ToString();
        }

        internal void AddDontcareRect(Rectangle rectangle)
        {
            List<Rectangle> intersectList = this.blockRectList.FindAll(f => rectangle.IntersectsWith(f));
            Rectangle unionRect = intersectList.Aggregate(rectangle, (f, g) => Rectangle.Union(f, g));

            this.blockRectList.RemoveAll(f => intersectList.Contains(f));
            this.blockRectList.Add(unionRect);
        }

        internal void AddCreticalPoint(Rectangle rectangle)
        {
            lock (this.creticalPointList)
            {
                this.creticalPointList.Add(rectangle);
                SortCreticalPointList();
            }
        }

        private void SortCreticalPointList()
        {
            if (this.creticalPointList.Count <= 1)
                return;

            List<Rectangle> tempList2 = new List<Rectangle>();

            if (true)
            {
                List<IGrouping<int, Rectangle>> groupList = this.creticalPointList.GroupBy(f => this.passRectList.FindIndex(g => g.IntersectsWith(f))).ToList();
                foreach (IGrouping<int, Rectangle> group in groupList)
                {
                    List<Rectangle> tempList = group.OrderBy(f => f.Left).ToList(); //this.creticalPointList.OrderBy(f => f.Left).ToList();
                    if (group.Count() <= 1)
                    {
                        tempList2.AddRange(group);
                        continue;
                    }

                    List<int> xDiff = new List<int>();
                    tempList.Aggregate((f, g) =>
                    {
                        xDiff.Add(g.Left - f.Left);
                        return g;
                    });
                    float threshold = Math.Max(50, (float)xDiff.Average());

                    int src = 0;
                    while (src <= xDiff.Count)
                    {
                        int dst = xDiff.FindIndex(src, f => f > threshold);
                        if (dst < 0)
                            dst = xDiff.Count;
                        tempList2.AddRange(tempList.GetRange(src, dst - src + 1).OrderBy(f => f.Top));
                        src = dst + 1;
                    }
                }
            }
            else
            {
                List<Rectangle> tempList = this.creticalPointList.OrderBy(f => f.Left).ToList(); //this.creticalPointList.OrderBy(f => f.Left).ToList();
                if (this.creticalPointList.Count() <= 1)
                {
                    tempList2.AddRange(this.creticalPointList);
                }
                else
                {

                    List<int> xDiff = new List<int>();
                    tempList.Aggregate((f, g) =>
                    {
                        xDiff.Add(g.Left - f.Left);
                        return g;
                    });
                    float threshold = Math.Max(50, (float)xDiff.Average());

                    int src = 0;
                    while (src <= xDiff.Count)
                    {
                        int dst = xDiff.FindIndex(src, f => f > threshold);
                        if (dst < 0)
                            dst = xDiff.Count;
                        tempList2.AddRange(tempList.GetRange(src, dst - src + 1).OrderBy(f => f.Top));
                        src = dst + 1;
                    }
                }
            }

            Debug.Assert(this.creticalPointList.Count == tempList2.Count);
            this.creticalPointList.Clear();
            this.creticalPointList.AddRange(tempList2);
        }
    }
}
