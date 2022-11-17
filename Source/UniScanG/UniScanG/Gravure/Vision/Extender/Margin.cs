using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.Vision.Calculator.V2;
using UniScanG.Gravure.Vision.SheetFinder;
using UniScanG.Vision;

namespace UniScanG.Gravure.Vision.Extender
{
    public class Margin : ExtItem
    {
        public enum EMarginPos { CM, ST, SB, CUSTOM }

        public EMarginPos MarginPos => this.marginPos;
        EMarginPos marginPos;

        private SizeF targetMarginSize = SizeF.Empty;
        private List<SizeF> targetMarginSizeList = new List<SizeF>();

        public Margin(EMarginPos marginPos = EMarginPos.CUSTOM) : base(ExtType.Margin, DynMvp.Base.LicenseManager.ELicenses.ExtMargin)
        {
            this.marginPos = marginPos;
        }

        public override void PrepareInspection()
        {
            this.targetMarginSize = SizeF.Empty;
            this.targetMarginSizeList.Clear();
        }

        protected override ExtItem CloneItem()
        {
            return new Margin(EMarginPos.CUSTOM);
        }

        public override bool Import(XmlElement element)
        {
            base.Import(element);
            this.marginPos = XmlHelper.GetValue(element, "MarginPos", EMarginPos.CUSTOM);
            return true;
        }

        public override void Export(XmlElement element)
        {
            base.Export(element);
            XmlHelper.SetValue(element, "MarginPos", this.marginPos);
        }

        public override void CopyFrom(ExtItem watchItem)
        {
            base.CopyFrom(watchItem);

            Margin margin = (Margin)watchItem;
            this.marginPos = margin.marginPos;
        }

        protected override void UpdateClipRectangle()
        {
            PointF centerPoint = DrawingHelper.CenterPoint(this.MasterRectangleUm);
            if (false)
            {
                // 폭,너비가 1.5배
                RectangleF inflateRect = RectangleF.Inflate(this.MasterRectangleUm, this.MasterRectangleUm.Width / 2, this.MasterRectangleUm.Height / 2);

                // 가로:세로 비율을 1:1
                float unit = Math.Max(inflateRect.Width / 1f, inflateRect.Height / 1f);
                RectangleF clipRectangle = DrawingHelper.FromCenterSize(centerPoint, new SizeF(unit * 1, unit * 1));

                this.ClipRectangleUm = Rectangle.Round(clipRectangle);
            }
            else
            {
                // 가로:세로 비율을 1:1
                float unit = Math.Max(this.MasterRectangleUm.Width / 1f, this.MasterRectangleUm.Height / 1f);
                RectangleF clipRectangleR = DrawingHelper.FromCenterSize(centerPoint, new SizeF(unit * 1, unit * 1));

                this.ClipRectangleUm = clipRectangleR;
            }
        }

        public FoundedObjInPattern Measure(AlgoImage algoImage, Point offsetPx, MarginParam marginParam, Calibration calibration)
        {
            DebugContext debugContext = new DebugContext(false, $@"C:\temp\{this.Name}");
            Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);
            PointF offsetUm = calibration.PixelToWorld(offsetPx);

            RectangleF adjClipRectUm = DrawingHelper.Offset(this.ClipRectangleUm, offsetUm);
            Rectangle adjClipRect = Rectangle.Round(calibration.WorldToPixel(adjClipRectUm));

            RectangleF adjMasterRectUm = DrawingHelper.Offset(this.MasterRectangleUm, offsetUm);
            Rectangle adjMasterRect = Rectangle.Round(calibration.WorldToPixel(adjMasterRectUm));

            Point drawOffset = DrawingHelper.Subtract(adjMasterRect.Location, adjClipRect.Location);
            MarginObj marginObj = new MarginObj()
            {
                Name = this.Name,
                Region = adjMasterRect,
                RealRegion = calibration.PixelToWorld(adjMasterRect),
                MarginSize = Size.Empty,
                MarginSizeUm = SizeF.Empty,
                MarginPos = this.marginPos
            };

            if (Rectangle.Intersect(imageRect, adjClipRect) != adjClipRect)
            {
                LogHelper.Debug(LoggerType.Inspection, $"Margin::Measure - Rectangle Size Missmatch. imageRect: {imageRect}, adjMasterRect: {adjMasterRect}");
                return marginObj;
            }

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
            BlobParam blobParam = new BlobParam();

            using (AlgoImage inspImage = algoImage.Clip(adjMasterRect))
            {
                inspImage.Save("inspImage.bmp", debugContext);

                Rectangle subRect = new Rectangle(Point.Empty, inspImage.Size);
                PointF subRectCenter = DrawingHelper.CenterPoint(subRect);

                List<BlobRect> overallBlobRectList;
                using (AlgoImage binalImage = ImageBuilder.BuildSameTypeSize(inspImage))
                {
                    if (marginParam.AverageCoating < 0 || marginParam.AveragePole < 0)
                    {
                        ip.Binarize(inspImage, binalImage, true);
                    }
                    else
                    {
                        int bb = (int)((marginParam.AverageCoating + marginParam.AveragePole) / 2);
                        ip.Binarize(inspImage, binalImage, bb, true);
                        ip.Erode(binalImage, 2);
                    }
                    binalImage.Save(@"inspBinalImage.bmp", debugContext);

                    using (BlobRectList blobRectList = ip.Blob(binalImage, blobParam))
                        overallBlobRectList = blobRectList.GetList().OrderBy(f => MathHelper.GetLength(subRectCenter, f.CenterPt)).ToList();
                }

                BlobRect centerBlobRect = overallBlobRectList.FirstOrDefault();
                if (centerBlobRect == null || !centerBlobRect.BoundingRect.Contains(subRectCenter))
                {
                    LogHelper.Debug(LoggerType.Inspection, $"Margin::Measure - Center blob is not exist.");
                    return marginObj;
                }

                BlobRect[] neighborBlobRects = overallBlobRectList.FindAll(f => f.IsTouchBorder).ToArray();
                //BlobRect[] neighborBlobRects = overallBlobRectList.FindAll(f => f.Area>200).ToArray();

                int size2 = 3;
                Rectangle boundingRect = Rectangle.Round(centerBlobRect.BoundingRect);
                Func<Rectangle, Rectangle>[] func ={
                        new Func<Rectangle,Rectangle>(f=>Rectangle.FromLTRB(f.Left-size2 ,f.Top,f.Left+size2 ,f.Bottom)), // 블랍 왼쪽 면.
                        new Func<Rectangle,Rectangle>(f=>Rectangle.FromLTRB(f.Left,f.Top-size2 ,f.Right,f.Top+size2 )), // 블랍 상단 면.
                        new Func<Rectangle,Rectangle>(f=>Rectangle.FromLTRB(f.Right-size2 ,f.Top,f.Right+size2 ,f.Bottom)), // 블랍 오른쪽 면.
                        new Func<Rectangle,Rectangle>(f=>Rectangle.FromLTRB(f.Left,f.Bottom-size2 ,f.Right,f.Bottom+size2 ))}; // 블랍 하단 면.

                Tuple<Func<Rectangle, Rectangle>, Func<Rectangle, Rectangle>, Rectangle, Direction, Func<PointF, PointF[], float>>[] tuples
                    = new Tuple<Func<Rectangle, Rectangle>, Func<Rectangle, Rectangle>, Rectangle, Direction, Func<PointF, PointF[], float>>[]{
                    new Tuple<Func<Rectangle,Rectangle>,Func<Rectangle,Rectangle>, Rectangle,Direction, Func<PointF, PointF[], float>>(// l
                        func[0],func[2], // 중심 패턴의 왼쪽 면. 인근 패턴의 오른쪽 면
                        Rectangle.FromLTRB(0, boundingRect.Top, boundingRect.Left, boundingRect.Bottom), // 인근 패턴 탐색 영역
                        Direction.Horizontal,new Func<PointF, PointF[], float>((f1,f2)=>f1.X - f2.Max(f=>f.X))),// 길이 계산 식

                    new Tuple<Func<Rectangle,Rectangle>,Func<Rectangle,Rectangle>, Rectangle,Direction, Func<PointF, PointF[], float>>(// t
                        func[1],func[3],
                        Rectangle.FromLTRB(boundingRect.Left, 0, boundingRect.Right, boundingRect.Top),
                        Direction.Vertical,new Func<PointF, PointF[], float>((f,g)=>f.Y-g.Max(h=>h.Y))),

                    new Tuple<Func<Rectangle,Rectangle>,Func<Rectangle,Rectangle>, Rectangle,Direction, Func<PointF, PointF[], float>>(// r
                        func[2],func[0],
                        Rectangle.FromLTRB(boundingRect.Right, boundingRect.Top, inspImage.Width, boundingRect.Bottom),
                     Direction.Horizontal,   new Func<PointF, PointF[], float>((f,g)=>g.Min(h=>h.X) -  f.X)),

                    new Tuple<Func<Rectangle,Rectangle>,Func<Rectangle,Rectangle>, Rectangle,Direction, Func<PointF, PointF[], float>>(// b
                        func[3],func[1],
                        Rectangle.FromLTRB(boundingRect.Left, boundingRect.Bottom, boundingRect.Right, inspImage.Height),
                      Direction.Vertical,  new Func<PointF, PointF[], float>((f,g)=>g.Min(h=>h.Y) -  f.Y)),
                };

                // 이미지에 그리기...
                PointF marginPx;
                using (AlgoImage clip = algoImage.Clip(adjClipRect))
                {
                    using (AlgoImage drawImage = clip.ConvertTo(ImageType.Color))
                    {
                        Size drawSize = new Size(5, 5);
                        Point centerPt = Point.Round(DrawingHelper.Add(DrawingHelper.CenterPoint(boundingRect), drawOffset));
                        int length = (int)Math.Max(5, Math.Min(boundingRect.Size.Width, boundingRect.Size.Height) * 0.2f);
                        ip.DrawRect(drawImage, DrawingHelper.FromCenterSize(centerPt, new Size(length, 1)), Color.Yellow.ToArgb(), false);
                        ip.DrawRect(drawImage, DrawingHelper.FromCenterSize(centerPt, new Size(1, length)), Color.Yellow.ToArgb(), false);

                        float[] distPx = tuples.Select((f,i) =>
                        {
                            LogHelper.Debug(LoggerType.Inspection, $"Margin::Measure - Side: {i}");
                            Rectangle baseRect = f.Item1(boundingRect);
                            DebugContext debugContext2 = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, i.ToString(), "Base"));
                            PointF basePoint = GetPoint(inspImage, baseRect, f.Item4, debugContext2);
                            ip.DrawRect(drawImage, Rectangle.Round(DrawingHelper.FromCenterSize(DrawingHelper.Add(basePoint, drawOffset), drawSize)), Color.Red.ToArgb(), false);
                            LogHelper.Debug(LoggerType.Inspection, $"Margin::Measure - {(i % 2 == 0 ? $"BasePoint.X: {basePoint.X}" : $"BasePoint.Y: {basePoint.Y}")}");

                            List<Rectangle> rectangleList = neighborBlobRects.Select(g => Rectangle.Round(RectangleF.Intersect(f.Item3, g.BoundingRect))).ToList();
                            rectangleList.RemoveAll(g => g.Width == 0 || g.Height == 0);
                            PointF[] points = rectangleList.Select((g, j) =>
                            {
                                DebugContext debugContext3 = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, i.ToString(), j.ToString()));
                                return GetPoint(inspImage, f.Item2(g), f.Item4, debugContext3);
                            }).ToArray();
                            if (points.Length == 0)
                                return 0;

                            Array.ForEach(points, g => ip.DrawRect(drawImage, Rectangle.Round(DrawingHelper.FromCenterSize(DrawingHelper.Add(g, drawOffset), drawSize)), Color.Blue.ToArgb(), false));
                            return f.Item5(basePoint, points);
                        }).ToArray();
                        marginObj.Image = drawImage.ToBitmap();
                        drawImage.Save(@"drawImage.bmp", debugContext);
                        //marginObj.Image.Save(@"C:\temp\drawImage2.jpg");

                        float marginPxW = 0;
                        if (float.IsNaN(distPx[0]) || distPx[0] == 0)
                            marginPxW = distPx[2];
                        else if (float.IsNaN(distPx[2]) || distPx[2] == 0)
                            marginPxW = distPx[0];
                        else
                            marginPxW = Math.Min(distPx[0], distPx[2]);

                        float marginPxH = 0;
                        if (float.IsNaN(distPx[1]) || distPx[1] == 0)
                            marginPxH = distPx[3];
                        else if (float.IsNaN(distPx[3]) || distPx[3] == 0)
                            marginPxH = distPx[1];
                        else
                            marginPxH = Math.Min(distPx[1], distPx[3]);

                        marginPx = new PointF(marginPxW, marginPxH);
                    }
                }

                Rectangle region = DrawingHelper.Offset(boundingRect, adjMasterRect.Location);
                marginObj.Region = region;
                marginObj.RealRegion = calibration.PixelToWorld(region);

                marginObj.MarginSize = new SizeF(marginPx);
                PointF realMargin = calibration.PixelToWorld(marginPx);
                marginObj.MarginSizeUm = DrawingHelper.Add(new SizeF(realMargin), AdditionalSettings.Instance().MarginOffset);

                if (this.targetMarginSize.IsEmpty)
                {
                    this.targetMarginSizeList.Add(marginObj.MarginSizeUm);
                    if (this.targetMarginSizeList.Count > 5)
                    {
                        this.targetMarginSize.Width = this.targetMarginSizeList.Average(f => f.Width);
                        this.targetMarginSize.Height = this.targetMarginSizeList.Average(f => f.Height);
                    }
                }

                if (!this.targetMarginSize.IsEmpty)
                {
                    //double defectSize = AdditionalSettings.Instance().MarginLengthAlarm.Value;
                    double defectSize = marginParam.AbsDefectSize;
                    marginObj.DiffMarginSizeUm = DrawingHelper.Subtract(marginObj.MarginSizeUm, this.targetMarginSize);
                    bool isDefect =
                        Math.Abs(marginObj.DiffMarginSizeUm.Width) > defectSize ||
                        Math.Abs(marginObj.DiffMarginSizeUm.Height) > defectSize;
                    marginObj.SetDefect(isDefect);
                }
            }
            return marginObj;
        }

        private PointF GetPoint(AlgoImage algoImage, Rectangle rectangle, Direction direction, DebugContext debugContext)
        {
            Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

            rectangle.Intersect(imageRect);
            if (rectangle.Width == 0 || rectangle.Height == 0)
                return direction == Direction.Horizontal ?
                new PointF((rectangle.Left + rectangle.Right) / 2, -1) :
                new PointF(-1, (rectangle.Top + rectangle.Bottom) / 2);

            PointF foundPoint;
            algoImage.Save(@"1.algoImage.bmp", debugContext);
            using (AlgoImage clipImage = algoImage.Clip(rectangle))
            {
                clipImage.Save(@"2.clipImage.bmp", debugContext);
                int maxIdx = 0;
                using (AlgoImage binalImage = clipImage.Clone())
                {
                    ip.Binarize(clipImage, binalImage, true);
                    binalImage.Save(@"3.binalImage.bmp", debugContext);

                    //float[] prj = ip.Projection(binalImage, direction.Invert(), ProjectionType.Mean);
                    //float maxValue = prj.Max();

                    float[] prj = ip.Projection(clipImage, direction.Invert(), ProjectionType.Mean);

                    // 선형보간
                    double meanX = prj.Length / 2f;
                    double meanY = prj.Average();
                    double nom = prj.Select((f, i) => (f - meanY) * (i - meanX)).Sum();
                    double denom = prj.Select((f, i) => Math.Pow(i - meanX, 2)).Sum();
                    if (denom > 0)
                    {
                        double slp = nom / denom;
                        for (int i = 0; i < prj.Length; i++)
                            prj[i] -= (float)(i * slp);
                        //Array.ForEach(prj, f => Debug.WriteLine(f));
                    }
                    float maxValue = prj.Min();

                    // 최대값의 인덱스들
                    List<int> maxPrjIdxList = prj.Select((f, i) => f == maxValue ? i : -1).ToList();
                    maxPrjIdxList.RemoveAll(f => f < 0);

                    // 연속된 인덱스를 찾음
                    int[] flatValues = maxPrjIdxList.Select((f, i) => f - i).ToArray();
                    var groups = flatValues.GroupBy(f => f);    // flatValue로 그룹화.

                    // 연속된 인덱스 중 가장 긴 구역의 중간값 사용
                    int[] counts = groups.Select(f => f.Count()).ToArray();
                    int maxCount = counts.Max();
                    int maxLength = Array.FindIndex(counts, 0, f => f == maxCount);

                    var element = groups.ElementAt(maxLength);
                    int offset = counts.Take(maxLength).Sum();
                    int floatValue = element.Key;
                    int src = floatValue + offset;
                    int dst = src + element.Count();

                    maxIdx = (src + dst) / 2;

                    //maxIdx = Array.IndexOf(prj, maxValue);
                }
                foundPoint = direction == Direction.Horizontal ?
                    new PointF((rectangle.Left + rectangle.Right) / 2, rectangle.Top + maxIdx) :
                    new PointF(rectangle.Left + maxIdx, (rectangle.Top + rectangle.Bottom) / 2);
            }

            Rectangle sobelRect = Rectangle.Round(DrawingHelper.FromCenterSize(foundPoint, new Size(10, 10)));
            sobelRect.Intersect(imageRect);
            if (sobelRect.Width == 0 || sobelRect.Height == 0)
                return foundPoint;

            //AlgoImage sobelAlgoImage = ImageBuilder.BuildSameTypeSize(algoImage);
            AlgoImage sobelAlgoImage = ImageBuilder.Build(algoImage.LibraryType, ImageType.Depth, algoImage.Size);
            sobelAlgoImage.Clear();
            ip.Sobel(algoImage, sobelAlgoImage);
            sobelAlgoImage.Save(@"4.sobelAlgoImage.bmp", debugContext);
            using (AlgoImage subSobelImage = sobelAlgoImage.GetSubImage(sobelRect))
            {
                subSobelImage.Save(@"5.subSobelImage.bmp", debugContext);
                float[] prj = ip.Projection(subSobelImage, direction, ProjectionType.Mean);
                float cog = prj.Select((f, i) => f * i).Sum() / prj.Sum();
				LogHelper.Debug(LoggerType.Inspection, $"Margin::GetPoint - CoG: {cog}");

                float x = direction == Direction.Horizontal ? sobelRect.Left + cog : foundPoint.X;
                float y = direction == Direction.Vertical ? sobelRect.Top + cog : foundPoint.Y;

                foundPoint = new PointF(x, y);
            }
            sobelAlgoImage.Dispose();
            return foundPoint;
        }
    }

    public class MarginParam : ExtParam
    {
        public override bool Use => AdditionalSettings.Instance().MarginUse;

        public int Count { get => this.count; set => this.count = value; }
        private int count;

        public float AbsDefectSize { get => this.absDefectSize; set => this.absDefectSize = value; }
        float absDefectSize;

        public float AveragePole { get => this.averagePole; set => this.averagePole = value; }
        float averagePole;

        public float AverageCoating { get => this.averageCoating; set => this.averageCoating = value; }
        float averageCoating;

        public MarginParam(bool available) : base(available)
        {
            this.count = 3;
            this.absDefectSize = 100;
            this.averagePole = -1;
            this.averageCoating = -1;
        }

        public override ExtParam Clone()
        {
            return new MarginParam(this.Available)
            {
                count = this.count,
                absDefectSize = this.absDefectSize,
                averagePole = this.averagePole,
                averageCoating = this.averageCoating
            };
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);
            this.count = XmlHelper.GetValue(xmlElement, "Count", this.count);
            this.absDefectSize = XmlHelper.GetValue(xmlElement, "AbsDefectSize", this.absDefectSize);
            this.averagePole = XmlHelper.GetValue(xmlElement, "AveragePole", this.averagePole);
            this.averageCoating = XmlHelper.GetValue(xmlElement, "AverageCoating", this.averageCoating);
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);
            XmlHelper.SetValue(xmlElement, "Count", this.count);
            XmlHelper.SetValue(xmlElement, "AbsDefectSize", this.absDefectSize);
            XmlHelper.SetValue(xmlElement, "AveragePole", this.averagePole);
            XmlHelper.SetValue(xmlElement, "AverageCoating", this.averageCoating);
        }
    }

    public class MarginCollection : ExtCollection
    {
        public MarginCollection() : base(ExtType.Margin)
        {
            this.param = new MarginParam(DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.ExtMargin));
        }

        public MarginParam Param => (MarginParam)this.param;

        public override ExtItem CreateItem()
        {
            return new Margin(Margin.EMarginPos.CUSTOM);
        }

        public override ExtCollection Clone()
        {
            MarginCollection marginCollection = new MarginCollection();
            marginCollection.param = this.param.Clone();
            this.items.ForEach(f => marginCollection.Add(f.Clone()));
            return marginCollection;
        }

        public ExtItem CreateItem(Margin.EMarginPos marginPos)
        {
            return new Margin(marginPos);
        }

        public override void Train(ExtCollectionTrainParam trainParam, Action ProgressUpdated, DebugContext debugContext)
        {
            int count = this.Param.Active ? this.Param.Count : 0;
            List<RegionInfoG> useRegionInfoList = trainParam.RegionInfoList.FindAll(f => f.Use);

            if (useRegionInfoList.Count > 0 && count > 0)
            {
                this.Param.AveragePole = useRegionInfoList.Average(f => f.PoleAvg);
                this.Param.AverageCoating = useRegionInfoList.Average(f => f.DielectricAvg);

                int sideXPos = 0;
                int centerXPos = trainParam.TrainImage.Width;
                float sideXLim = 0.15f;
                float centerXLim = 1 - sideXLim;

                BaseXSearchDir baseXSearchDir = SheetFinderBase.SheetFinderBaseParam.GetBaseXSearchDir();
                if (baseXSearchDir == BaseXSearchDir.Right2Left)
                {
                    sideXPos = trainParam.TrainImage.Width;
                    centerXPos = 0;
                    sideXLim = 0.9f;
                    centerXLim = 0.1f;
                }

                //marginCollection.Clear();
                RemoveAll(f => ((Margin)f).MarginPos != Margin.EMarginPos.CUSTOM);

                Point refPos;
                PointF position = PointF.Empty;
                for (int i = 0; i < count; i++)
                {
                    DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, string.Format("MarginItems_{0}", i)));
                    Margin.EMarginPos marginPos = Margin.EMarginPos.CUSTOM;
                    switch (i)
                    {
                        case 0:
                            // Center-Middle
                            marginPos = Margin.EMarginPos.CM;
                            refPos = new Point(centerXPos, trainParam.TrainImage.Height / 2);
                            position = new PointF(centerXLim, -1f);
                            break;
                        case 1:
                            // Side-Top
                            marginPos = Margin.EMarginPos.ST;
                            refPos = new Point(sideXPos, 0);
                            position = new PointF(sideXLim, 0.2f);
                            break;
                        case 2:
                            // Side-Bottom
                            marginPos = Margin.EMarginPos.SB;
                            refPos = new Point(sideXPos, trainParam.TrainImage.Height);
                            position = new PointF(sideXLim, 0.8f);
                            break;
                        default:
                            continue;
                    }
                    float[] distances = useRegionInfoList.Select(f => MathHelper.GetLength(f.Region.Location, refPos)).ToArray();
                    float minimumDist = distances.Min();
                    int minimumIdx = Array.FindIndex(distances, f => f == minimumDist);
                    RegionInfoG regionInfoG = useRegionInfoList[minimumIdx];
                    if (position.Y < 0)
                    // 바 내부에서 가장 적절한 위치를 찾음 (세로방향)
                    {
                        float hLimit = Math.Max(sideXLim, centerXLim);
                        float lLimit = Math.Min(sideXLim, centerXLim);
                        float h = (hLimit - lLimit) / (regionInfoG.Region.Height) * (refPos.Y - regionInfoG.Region.Top) + lLimit;
                        position.Y = Math.Min(hLimit, Math.Max(lLimit, h));
                    }

                    ExtItem extItem = CreateItem(marginPos);
                    if (extItem.BuildItem(trainParam.TrainImage, regionInfoG, position, trainParam.Calibration, newDebugContext))
                    {
                        extItem.Name = marginPos.ToString();
                        extItem.Index = i;
                        extItem.ContainerIndex = trainParam.RegionInfoList.IndexOf(regionInfoG);
                        Add(extItem);
                    }
                    //curStep++;
                    //worker.ReportProgress((int)Math.Round(start + (step * curStep)),
                    //    string.Format(StringManager.GetString(this.GetType().FullName, "Monitoring Point ({0}/{1})"), curStep, totalSteps));
                    //Thread.Sleep(sleepMs);
                }
                Sort();
                //curStep = monChipCount + marginCollection.Param.Count;
            }
            else
            {
                Clear();
                this.Param.AveragePole = this.Param.AverageCoating = -1;
            }
        }

        public override FoundedObjInPattern[] Inspect(SheetInspectParam inspectParam)
        {
            if (!this.param.Active)
                return new FoundedObjInPattern[0];

            Calibration calibration = inspectParam.CameraCalibration;
            AlgoImage algoImage = inspectParam.AlgoImage;
            Point patternOffset = Point.Empty;
            ProcessBufferSetG processBufferSetG = inspectParam.ProcessBufferSet as ProcessBufferSetG;
            if (processBufferSetG != null)
            {
                algoImage = processBufferSetG.AlgoImage;
                patternOffset = processBufferSetG.OffsetStructSet.PatternOffset.Offset;
            }

            DebugContextG debugContextG = inspectParam.DebugContext as DebugContextG;
            Stopwatch sw = Stopwatch.StartNew();

            List<Margin> marginList = this.items.ConvertAll(f => (Margin)f);
            FoundedObjInPattern[] foundedObjInPatterns = marginList.Select(f =>
            {
                Point localOffset = Point.Empty;
                if (processBufferSetG != null)
                {
                    PointF localOffsetF = processBufferSetG.OffsetStructSet.GetLocalOffset(f.ContainerIndex);
                    localOffset = Point.Round(localOffsetF);
                }
                LogHelper.Debug(LoggerType.Inspection, string.Format("MarginCollection::Inspect - Type: {0}, ID: {1}, PatternOffset: {2}, LocalOffset: {3}",
                    f.ExtType, f.Index, patternOffset, localOffset));
                return f.Measure(algoImage, DrawingHelper.Add(patternOffset, localOffset), (MarginParam)this.param, calibration);
            }).ToArray();

            sw.Stop();
            debugContextG.ProcessTimeLog.Add(ProcessTimeLog.ProcessTimeLogItem.Extend_Margin, sw.ElapsedMilliseconds);

            return foundedObjInPatterns;
        }
    }
}