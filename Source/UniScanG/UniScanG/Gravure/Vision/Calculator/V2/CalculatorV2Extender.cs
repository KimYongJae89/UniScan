using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using UniScanG.Data;
using UniScanG.Data.Model;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Settings;

namespace UniScanG.Gravure.Vision.Calculator.V2
{
    internal class InspectRegion
    {
        public bool IsbarAlign => this.inBarAlign;
        bool inBarAlign;

        public float Scale { get => scale; }
        float scale;

        public Rectangle Rectangle { get => rectangle; }
        Rectangle rectangle;

        public AlgoImage AlgoImage { get => algoImage; }
        AlgoImage algoImage = null;

        public AlgoImage ThresholdMapImage { get => thresholdMapImage; }
        AlgoImage thresholdMapImage = null;

        public bool IsSet { get => isBuilded; }
        bool isBuilded;

        public AlgoImage DiffImage => this.diffImage;
        AlgoImage diffImage = null;

        public AlgoImage BinalImage => this.binalImage;
        AlgoImage binalImage = null;

        public AlgoImage EdgeMapImage => this.edgeMapImage;
        AlgoImage edgeMapImage = null;

        public AlgoImage MaskImage => this.maskImage;
        AlgoImage maskImage = null;

        public RegionInfoG RegionInfoG => this.regionInfoG;
        RegionInfoG regionInfoG = null;

        public InspectLine[] AllinspectLines { get => this.allinspectLines; }
        InspectLine[] allinspectLines;

        public InspectLineSet[] InspectLineSets { get => inspectLineSets; }
        InspectLineSet[] inspectLineSets;

        InBarAligner InBarAligner => this.inBarAligner;
        InBarAligner inBarAligner = null;

        CalculatorParam calculatorParam = null;
        SensitiveParam sensitiveParam = null;

        public bool Use => this.inspectLineSets.Length > 0;

        Rectangle[] clearRects = null;

        public InspectRegion(RegionInfoG regionInfoG, InspectLineSet[] inspectLineSets)
        {
            //Rectangle adjustRect = Rectangle.Round(new RectangleF(rectangle.X * scale, rectangle.Y * scale, rectangle.Width * scale, rectangle.Height * scale));
            this.scale = 1;
            this.regionInfoG = regionInfoG;
            this.rectangle = this.rectangle = regionInfoG.Region;
            this.inspectLineSets = inspectLineSets;
            this.allinspectLines = inspectLineSets.SelectMany(f=>f.InspectLines).OrderBy(f => MathHelper.GetLength(Rectangle.Location, Point.Empty)).ToArray();

            Array.ForEach(inspectLineSets, f => f.Parent = this);
            Rectangle[] dontCareRects = regionInfoG.GetDontcareRects();
            Array.ForEach(inspectLineSets, f => f.SetDontcare(dontCareRects));
        }

        public void Initialize(CalculatorParam calculatorParam, SensitiveParam sensitiveParam, float scale)
        {
            if (this.rectangle.Size.IsEmpty)
                return;

            if (this.inspectLineSets.Length == 0)
                return;

            this.inBarAlign = calculatorParam.InBarAlign;
            this.scale = scale;
            this.calculatorParam = calculatorParam;
            this.sensitiveParam = sensitiveParam;
            this.rectangle = DrawingHelper.Mul(this.rectangle, scale);

            List<Rectangle> clearRectList = new List<Rectangle>();
            clearRectList.AddRange(this.regionInfoG.GetBlockRects(calculatorParam.ModelParam.BarBoundary));
            clearRectList.AddRange(this.regionInfoG.BlockRectList);
            this.clearRects = clearRectList.Select(f => f = DrawingHelper.Mul(f, scale)).ToArray();

            this.thresholdMapImage = ImageBuilder.Build(CalculatorBase.TypeName, rectangle.Size);
            if (!sensitiveParam.Multi)
            // Single Threshold
            {
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(this.thresholdMapImage);
                byte binValueMax = sensitiveParam.Max;
                ip.Clear(this.thresholdMapImage, binValueMax);
            }
            else
            // Multi Threshold
            {
                ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(this.algoImage);
                byte binValueMin = Math.Min(this.sensitiveParam.Min, this.sensitiveParam.Max);
                byte binValueMax = Math.Max(this.sensitiveParam.Min, this.sensitiveParam.Max);

                int mul = binValueMax - binValueMin;
                int sum = 256 * binValueMin;
                int div = 256;
                ip.MulSumDiv(this.algoImage, this.thresholdMapImage, mul, sum, div);
            }

            this.inBarAligner = new InBarAligner(4);
            if (this.inBarAlign)
                this.inBarAligner.Initialize(regionInfoG.AlignInfoCollection.ToArray());

            Array.ForEach(this.inspectLineSets, f => f.Initialize(this.algoImage, this.diffImage, this.binalImage, this.thresholdMapImage, scale));
        }

        public OffsetStruct SetImage(AlgoImage fullImage, AlgoImage inspImage, AlgoImage diffImage, AlgoImage binalImage, AlgoImage edgeMapImage, AlgoImage maskImage, Point patternOffset, DebugContextG debugContextG)
        {
            if (!this.Use)
                return new OffsetStruct();

            if (this.rectangle.Size.IsEmpty)
            {
                this.isBuilded = false;
                return new OffsetStruct();
            }

            try
            {
                Stopwatch sw = Stopwatch.StartNew();

                Point adjLocation = DrawingHelper.Add(this.regionInfoG.Region.Location, patternOffset);

                OffsetStruct offsetResult = this.inBarAligner.Align(fullImage, adjLocation, calculatorParam.InBarAlignScore, debugContextG);

                if (!offsetResult.IsGood) // 실패시 직전 값 가져옴.
                {
                    offsetResult.CopyFrom(ProcessBufferSetG2.GlobalOffsetStructSet.LocalOffsets[debugContextG.RegionId]);
                    offsetResult.IsGood = false;
                }

                adjLocation.Offset(offsetResult.Offset);
                // Size Variation이 (-)면 검사영역 줄임. (+)면 기존 영역 유지.
                // Width방향은 스케일링 안함
                Size adjVariation = new Size(0, Math.Min(0, offsetResult.Variation.Height));
                Size adjSize = Size.Add(this.regionInfoG.Region.Size, adjVariation);

                debugContextG.ProcessTimeLog.Add(ProcessTimeLog.ProcessTimeLogItem.Align2, sw.ElapsedMilliseconds);
                sw.Restart();

                Rectangle instRect = new Rectangle(Point.Empty, inspImage.Size);
                Rectangle adjustRect = Rectangle.Round(DrawingHelper.Mul(new Rectangle(adjLocation, adjSize), this.scale));
                if (adjustRect != Rectangle.Intersect(instRect, adjustRect) || adjustRect.Width == 0 || adjustRect.Height == 0)
                    throw new Exception($"Invalid Rectangle. instRect: {instRect}, adjustRect: {adjustRect}");

                if (this.algoImage == null && this.diffImage == null)
                {
                    this.algoImage = inspImage.GetSubImage(adjustRect);
                    this.diffImage = diffImage.GetSubImage(adjustRect);
                    this.binalImage = binalImage.GetSubImage(adjustRect);
                    this.edgeMapImage = edgeMapImage?.GetSubImage(adjustRect);
                    this.maskImage = null;

                    if (this.regionInfoG.PassRectList.Count > 0)
                    {
                        this.maskImage = maskImage.GetSubImage(adjustRect);

                        ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(this.maskImage);
                        ip.Clear(this.maskImage, 0);
                        Point offset = new Point(-patternOffset.X, -patternOffset.Y);
                        offset = Point.Empty;
                        this.regionInfoG.PassRectList.ForEach(f => ip.Clear(this.maskImage, DrawingHelper.Offset(DrawingHelper.Mul(f, this.scale), offset), Color.White));
                        //this.algoImage.Save(@"C:\temp\algoImage.bmp");
                        //this.maskImage.Save(@"C:\temp\maskImage.bmp");
                    }
                }
                else
                {
                    if (this.algoImage.IsCompatible(inspImage))
                    {
                        //this.algoImage.Copy(inspImage, adjustRect);
                    }
                    else
                    {
                        AlgoImage subImage = inspImage.GetSubImage(adjustRect);
                        DynMvp.Vision.ImageConverter.Convert(subImage, this.algoImage);
                        subImage.Dispose();
                    }
                }

                //this.algoImage.Save($@"C:\temp\algoImage1_{debugContextG.RegionId}.bmp");
                // -----
                //bool setOk = Array.TrueForAll(this.inspectLineSets, f =>
                //{
                //    DebugContextG debugContextGG = new DebugContextG(debugContextG);
                //    debugContextGG.LineSetId = Array.IndexOf(this.inspectLineSets, f);
                //    return f.SetImage(this.algoImage, this.diffImage, this.binalImage, this.thresholdMapImage, this.edgeMapImage, this.maskImage, adjVariation, debugContextGG);
                //});
                // -----

                for (int i = 0; i < this.inspectLineSets.Length; i++)
                {
                    debugContextG.LineSetId = i;
                    this.inspectLineSets[i].SetImage(this.algoImage, this.diffImage, this.binalImage, this.thresholdMapImage, this.edgeMapImage, this.maskImage, adjVariation, debugContextG);
                }
                debugContextG.LineSetId = -1;
                // -----

                //this.algoImage.Save($@"C:\temp\algoImage2_{debugContextG.RegionId}.bmp");
                debugContextG.ProcessTimeLog.Add(ProcessTimeLog.ProcessTimeLogItem.Build, sw.ElapsedMilliseconds);

                isBuilded = true;
                return offsetResult;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, $"Exception in InspectRegion::SetImage - RegionId: {debugContextG.RegionId}",true);
                LogHelper.Error(LoggerType.Inspection, ex, true);

                this.isBuilded = false;
                return new OffsetStruct();
            }
        }

        public InspectLine GetFirstLine()
        {
            Func<InspectLineSet, InspectLine> func = new Func<InspectLineSet, InspectLine>(f => f.InspectLines.First());
            Comparison<Tuple<InspectLine, double>> comparison = new Comparison<Tuple<InspectLine, double>>((f, g) => f.Item2.CompareTo(g.Item2));
            return GetLine(func, comparison);
        }
        public InspectLine GetLastLine()
        {
            Func<InspectLineSet, InspectLine> func = new Func<InspectLineSet, InspectLine>(f => f.InspectLines.Last());
            Comparison<Tuple<InspectLine, double>> comparison = new Comparison<Tuple<InspectLine, double>>((f, g) => g.Item2.CompareTo(f.Item2));
            return GetLine(func, comparison);
        }

        private InspectLine GetLine(Func<InspectLineSet, InspectLine> func, Comparison<Tuple<InspectLine, double>> comparison)
        {
            List<InspectLine> line = this.inspectLineSets.Select(func).ToList();
            List<Tuple<InspectLine, double>> tupleList = line.ConvertAll<Tuple<InspectLine, double>>(f => new Tuple<InspectLine, double>(f, GetDist(f)));
            tupleList.Sort(comparison);
            return tupleList[0].Item1; ;
        }

        private double GetDist(InspectLine f)
        {
            return Math.Pow(f.Rectangle.X, 2) + Math.Pow(f.Rectangle.Y, 2);
        }

        public void ClearImage()
        {
            //this.localOffset = Point.Empty;

            Array.ForEach(this.inspectLineSets, f => f.ClearImage());

            this.algoImage?.Dispose();
            this.algoImage = null;

            this.diffImage?.Dispose();
            this.diffImage = null;

            this.binalImage?.Dispose();
            this.binalImage = null;

            this.edgeMapImage?.Dispose();
            this.edgeMapImage = null;

            this.maskImage?.Dispose();
            this.maskImage = null;

            this.isBuilded = false;
        }

        public void Dispose()
        {
            Array.ForEach(inspectLineSets, f => f.Dispose());

            this.algoImage?.Dispose();
            this.thresholdMapImage?.Dispose();
            this.diffImage?.Dispose();
            this.binalImage?.Dispose();
            this.edgeMapImage?.Dispose();
            this.maskImage?.Dispose();

            this.inBarAligner?.Dispose();

            Array.Resize(ref this.allinspectLines, 0);
        }

        internal void ClearDontcare()
        {
            //this.resultImage.Save(@"C:\temp\this.resultImage.bmp");
            Rectangle fullRect = new Rectangle(Point.Empty, this.binalImage.Size);

            Array.ForEach(this.clearRects, f =>
            {
                Rectangle subRect = Rectangle.Intersect(fullRect, f);
                if (subRect.Width > 0 && subRect.Height > 0)
                {
                    using (AlgoImage subAlgoImage = this.binalImage.GetSubImage(subRect))
                        subAlgoImage.Clear();
                }
            });
            //this.resultImage.Save(@"C:\temp\this.resultImage.bmp");
        }

        internal void CloseBianlImage(int num)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(this.binalImage);
            ip.Close(this.binalImage, num);
        }
        //internal void Threshold(int sensitivity)
        //{
        //    ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(this.diffImage);

        //    if (this.thresholdMapImage != null)
        //    {
        //        ip.Subtract(this.diffImage, this.thresholdMapImage, this.binalImage);
        //        ip.Binarize(this.binalImage, 0);
        //    }
        //    else
        //    {
        //        ip.Binarize(this.diffImage, this.binalImage, sensitivity);
        //    }
        //}
    }

    internal class InspectLineSet
    {
        public InspectRegion Parent { get => this.parent; set => this.parent = value; }
        protected InspectRegion parent;

        public InspectLine[] InspectLines { get => this.inspectLines; }
        InspectLine[] inspectLines;

        public InspectEdge[] InspectEdges { get => this.inspectEdges; }
        InspectEdge[] inspectEdges;

        public CalculatorParam.EIgnoreMethod IgnoreMethod => this.lineSetParam.IgnoreMethod;
        public bool IgnoreSideLine => this.lineSetParam.IgnoreSideLine;
        LineSetParam lineSetParam;

        public ImageProcessing ImageProcessing { get => imageProcessing; }
        ImageProcessing imageProcessing;

        public InspectLineSet(InspectLine[] inspectLines, LineSetParam lineSetParam, ImageProcessing imageProcessing)
        {
            Debug.Assert(inspectLines.Min(f => f.Rectangle.Width) == inspectLines.Max(f => f.Rectangle.Width));
            Debug.Assert(inspectLines.Min(f => f.Rectangle.Height) == inspectLines.Max(f => f.Rectangle.Height));
            Array.ForEach(inspectLines, f => f.Parent = this);

            this.inspectLines = inspectLines;
            this.inspectEdges = Array.FindAll(inspectLines, f => f is InspectEdge).Cast<InspectEdge>().ToArray();
            this.lineSetParam = lineSetParam;
            this.imageProcessing = imageProcessing;
        }

        internal void Initialize(AlgoImage inspImage, AlgoImage diffImage, AlgoImage binalImage, AlgoImage thresholdMapImagem, float scale)
        {
            Array.ForEach(this.inspectLines, f => f.Initialize(inspImage, diffImage, binalImage, thresholdMapImagem, this.imageProcessing, scale));
            Array.ForEach(this.inspectLines, f => f.Initialize2());
        }

        internal void SetImage(AlgoImage inspImage, AlgoImage diffImage, AlgoImage binalImage, AlgoImage thresholdMapImage, AlgoImage edgeMapImage, AlgoImage maskImage, Size variation, DebugContextG debugContextG)
        {
            for (int i = 0; i < this.inspectLines.Length; i++)
            {
                debugContextG.LineId = i;
                this.inspectLines[i].SetImage(inspImage, diffImage, binalImage, thresholdMapImage, edgeMapImage, maskImage, variation, debugContextG);
            }
            debugContextG.LineId = -1;

            //Array.ForEach(this.inspectLines, f =>
            //{
            //    DebugContextG debugContextGG = new DebugContextG(debugContextG);
            //    debugContextGG.LineId = Array.IndexOf(this.inspectLines, f);
            //    f.SetImage(inspImage, diffImage, binalImage, thresholdMapImage, edgeMapImage, maskImage, variation, debugContextGG);
            //});

            Array.ForEach(this.inspectLines, f => f.SetImage2(debugContextG));
        }

        internal void GetResult(AlgoImage diffImage, AlgoImage binalImage)
        {
            Array.ForEach(this.inspectLines, f =>
            {
                diffImage.Copy(f.DiffImage, Point.Empty, f.Rectangle.Location, f.Rectangle.Size);
                binalImage.Copy(f.BinalImage, Point.Empty, f.Rectangle.Location, f.Rectangle.Size);
            });
        }

        internal void ClearImage()
        {
            this.imageProcessing.WaitStream();
            Array.ForEach(this.inspectLines, f => f.ClearImage());
        }

        public void Dispose()
        {
            this.imageProcessing.WaitStream();
            Array.ForEach(this.inspectLines, f => f.Dispose());
        }

        internal void SetDontcare(Rectangle[] dontCareRects)
        {
            Array.ForEach(this.inspectLines, f => f.SetDontcare(dontCareRects));
            Array.ForEach(this.inspectLines, f => f.SetDontcare2(this.lineSetParam.IgnoreMethod));
        }
    }

    internal class InspectLine
    {
        public InspectLineSet Parent { get => parent; set => this.parent = value; }
        protected InspectLineSet parent;

        public SensitiveParam SensitiveParam => this.sensitiveParam;
        protected SensitiveParam sensitiveParam;

        public Rectangle Rectangle { get => rectangle; }
        protected Rectangle rectangle;

        public Rectangle DontCare { get; set; }

        public Rectangle[] IgnoreArea { get => ignoreArea; }
        protected Rectangle[] ignoreArea;

        protected InspectLine[] copyAreaFrom;

        public Rectangle[] OwnIgnoreArea => this.ownIgnoreArea;
        protected Rectangle[] ownIgnoreArea;

        public InspectLine PrevInspectLine { get => prevLineElement; }
        protected InspectLine prevLineElement;

        public InspectLine NextInspectLine { get => nextLineElement; }
        protected InspectLine nextLineElement;

        public InspectEdge EdgeLineElement { get => edgeLineElement; }
        protected InspectEdge edgeLineElement;

        public bool IsLinked { get => (this.prevLineElement != null && this.nextLineElement != null); }

        public bool IsDoncare { get => this.ownIgnoreArea.Length > 0; }

        public AlgoImage AlgoImage { get => algoImage; }
        protected AlgoImage algoImage = null;

        public AlgoImage[] BufImage { get => bufImage; }
        protected AlgoImage[] bufImage = null;

        public AlgoImage DiffImage { get => diffImage; }
        protected AlgoImage diffImage = null;

        public AlgoImage BinalImage { get => binalImage; }
        protected AlgoImage binalImage = null;

        public AlgoImage ThresholdMapImage { get => thresholdMapImage; }
        protected AlgoImage thresholdMapImage = null;

        public AlgoImage EdgeMapImage { get => edgeMapImage; }
        protected AlgoImage edgeMapImage = null;

        public AlgoImage MaskImage { get => maskImage; }
        protected AlgoImage maskImage = null;

        public float GreyAverage => this.greyAverage;
        protected float greyAverage;

        public bool IsSet => this.algoImage != null;

        protected ImageProcessing imageProcessing;

        public InspectLine(Rectangle rectangle, float greyAverage, SensitiveParam sensitiveParam)
        {
            this.rectangle = rectangle;
            this.ownIgnoreArea = new Rectangle[0];
            this.greyAverage = greyAverage;
            this.sensitiveParam = sensitiveParam;
        }

        public void Link(InspectLine prev, InspectLine next, InspectEdge edge)
        {
            this.prevLineElement = prev;
            this.nextLineElement = next;
            this.edgeLineElement = edge;
        }

        public virtual void Initialize(AlgoImage algoImage, AlgoImage diffImage, AlgoImage binalImage, AlgoImage thresholdMapImage, ImageProcessing imageProcessing, float scale)
        {
            this.imageProcessing = imageProcessing;

            this.rectangle = DrawingHelper.Mul(this.rectangle, scale);

            for (int i = 0; i < this.ignoreArea.Length; i++)
                this.ignoreArea[i] = DrawingHelper.Mul(this.ignoreArea[i], scale);

            this.bufImage = new AlgoImage[1];
            for (int i = 0; i < this.bufImage.Length; i++)
                this.bufImage[i] = ImageBuilder.Build(CalculatorBase.TypeName, rectangle.Size);
        }

        public void Initialize2()
        {
            if (IsLinked)
            {
                RegionInfoG regionInfoG = this.parent.Parent.RegionInfoG;
                float slope = regionInfoG.Slope;

                // 미검사 영역은 좌/우 영역에서 복사해옴.
                // 두 칸 떨어진 곳에서 복사해 와야 함.
                this.copyAreaFrom = new InspectLine[this.ignoreArea.Length];
                for (int i = 0; i < this.ignoreArea.Length; i++)
                {
                    Rectangle f = this.ignoreArea[i];

                    InspectLine lineElementP = this.prevLineElement.prevLineElement.prevLineElement;
                    InspectLine lineElementN = this.nextLineElement.nextLineElement.nextLineElement;
                    if (lineElementP == this) // 왼쪽 3개 줄은 자기 자신을 가르킴
                        lineElementP = lineElementN.nextLineElement;

                    if (lineElementN == this) // 오른쪽 3개 줄은 자기 자신을 가르킴
                        lineElementN = lineElementP.prevLineElement;

                    bool prevExist = Array.Exists(lineElementP.ignoreArea, g => g.IntersectsWith(f));
                    bool nextExist = Array.Exists(lineElementN.ignoreArea, g => g.IntersectsWith(f));
                    if (!prevExist)
                    {
                        // 좌측 영역에 DontCare 없음
                        this.copyAreaFrom[i] = lineElementP;
                    }
                    else if (!nextExist)
                    {
                        // 우측 영역에 DontCare 없음
                        this.copyAreaFrom[i] = lineElementN;
                        //CopyFrom(algoImage, f, lineElementN, true);
                    }
                    else
                    {
                        // 둘 다 DontCare 있음.
                    }
                }
            }
        }

        public virtual void SetImage(AlgoImage algoImage, AlgoImage diffImage, AlgoImage binalImage, AlgoImage thresholdMapImage, AlgoImage edgeMapImage, AlgoImage maskImage, Size variation, DebugContextG debugContextG)
        {
            if (algoImage.Size != diffImage.Size)
                throw new Exception("algoImage.Size != diffImage.Size");

            Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);
            //Size adjSize = this.rectangle.Size;
            Size adjSize = DrawingHelper.Add(this.rectangle.Size, variation);

            Rectangle adjRect = Rectangle.Intersect(imageRect, new Rectangle(this.rectangle.Location, adjSize));
            if (adjRect.Width == 0 || adjRect.Height == 0)
                throw new Exception("adjRect.Width == 0 || adjRect.Height == 0");

            this.algoImage = algoImage.GetSubImage(adjRect);
            this.diffImage = diffImage.GetSubImage(adjRect);
            this.binalImage = binalImage.GetSubImage(adjRect);
            this.thresholdMapImage = thresholdMapImage.GetSubImage(adjRect);
            this.edgeMapImage = edgeMapImage?.GetSubImage(adjRect);
            this.maskImage = maskImage?.GetSubImage(adjRect);
        }

        public virtual void SetImage2(DebugContextG debugContextG)
        {
            if (this.algoImage == null)
                throw new Exception("algoImage == null");

            Rectangle imageRect = new Rectangle(Point.Empty, this.algoImage.Size);
            for (int i = 0; i < this.ignoreArea.Length; i++)
            {
                Rectangle f = Rectangle.Intersect(imageRect, this.ignoreArea[i]);
                if (f.Width <= 0 || f.Height <= 0)
                    continue;

                InspectLine target = this.copyAreaFrom[i];
                if (target != null)
                {
                    Rectangle dstRect = f;
                    Rectangle srcRect = f;

                    Size size = new Size(Math.Min(srcRect.Width, dstRect.Width), Math.Min(srcRect.Height, dstRect.Height));
                    f.Intersect(new Rectangle(Point.Empty, target.algoImage.Size));
                    CopyFrom(dstRect.Location, target, srcRect.Location, size);
                }
            }
        }

        private void CopyFrom(Point dstLoc, InspectLine srcLine, Point srcLoc, Size size)
        {
            AlgoImage srcImage = srcLine.algoImage;
            if (srcImage == null)
                throw new Exception();

            //Rectangle srcRect = lineElement.rectangle;
            //Rectangle dstRect = this.rectangle;
            //Rectangle copyRect = Rectangle.Intersect(srcRect, dstRect);
            //copyRect.Intersect(ignoreRect);
            this.algoImage.Copy(srcImage, srcLoc, dstLoc, size);
        }

        public virtual void ClearImage()
        {
            this.algoImage?.Dispose();
            this.algoImage = null;

            this.diffImage?.Dispose();
            this.diffImage = null;

            this.binalImage?.Dispose();
            this.binalImage = null;

            this.thresholdMapImage?.Dispose();
            this.thresholdMapImage = null;

            this.edgeMapImage?.Dispose();
            this.edgeMapImage = null;

            this.maskImage?.Dispose();
            this.maskImage = null;

            if (this.bufImage != null)
                Array.ForEach(this.bufImage, f => f?.Clear());
        }

        public virtual void Dispose()
        {
            this.algoImage?.Dispose();
            this.algoImage = null;

            this.diffImage?.Dispose();
            this.diffImage = null;

            this.binalImage?.Dispose();
            this.binalImage = null;

            this.thresholdMapImage?.Dispose();
            this.thresholdMapImage = null;

            this.edgeMapImage?.Dispose();
            this.edgeMapImage = null;

            this.maskImage?.Dispose();
            this.maskImage = null;

            if (this.bufImage != null)
                Array.ForEach(this.bufImage, f => f?.Dispose());
            this.bufImage = null;
        }

        internal void SetDontcare(Rectangle[] dontCareRects)
        {
            List<Rectangle> rectangleList = dontCareRects.Select(f => Rectangle.Intersect(this.rectangle, f)).ToList();
            rectangleList.RemoveAll(f => f.Width == 0 || f.Height == 0);
            this.ownIgnoreArea = rectangleList.Select(f=>DrawingHelper.Offset(f, this.rectangle.Location, true)).OrderBy(f => f.Y).ToArray();
        }

        internal void SetDontcare2(CalculatorParam.EIgnoreMethod ignoreMethod)
        {
            List<Rectangle> ignoreList = new List<Rectangle>();
            List<Rectangle> temp = new List<Rectangle>();
            if (ignoreMethod == CalculatorParam.EIgnoreMethod.Basic)
            {
                temp.AddRange(ownIgnoreArea);
            }
            else if (ignoreMethod == CalculatorParam.EIgnoreMethod.Neighborhood)
            {
                temp.AddRange(this.ownIgnoreArea);
                temp.AddRange(this.prevLineElement.ownIgnoreArea);
                temp.AddRange(this.nextLineElement.ownIgnoreArea);
            }

            if (temp.Count > 0)
            {
                Size averageSize = Size.Round(new SizeF((float)temp.Average(f => f.Width), (float)temp.Average(f => f.Height)));

                // Merge
                while (temp.Count > 0)
                {
                    Rectangle rectangle = temp[0];
                    temp.RemoveAt(0);

                    List<Rectangle> intersectRectList = null;
                    do
                    {
                        intersectRectList = temp.FindAll(f => Rectangle.Inflate(rectangle, 1, 1).IntersectsWith(f));
                        temp.RemoveAll(f => intersectRectList.Contains(f));
                        rectangle = intersectRectList.Aggregate(rectangle, (f, g) => Rectangle.Union(f, g));
                    } while (intersectRectList.Count > 0);
                    ignoreList.Add(rectangle);

                    //List<Rectangle> intersectRectList = temp.FindAll(f => temp[0].IntersectsWith(f));
                    //temp.RemoveAll(f => intersectRectList.Contains(f));

                    //Rectangle rectangle = intersectRectList[0];
                    //intersectRectList.ForEach(f => rectangle = Rectangle.Union(rectangle, f));
                    ////rectangle.Inflate(averageSize);
                    //rectangle.Intersect(new Rectangle(Point.Empty, this.rectangle.Size));

                    //List<Rectangle> founded = ignoreList.FindAll(f => rectangle.IntersectsWith(f));
                    //ignoreList.RemoveAll(f => founded.Contains(f));
                    //founded.ForEach(f => rectangle = Rectangle.Union(rectangle, f));
                    //ignoreList.Add(rectangle);
                }
            }
            this.ignoreArea = ignoreList.ToArray();

            Size maxIgnoreSize = Size.Empty;
            if (this.ignoreArea.Length > 0)
            {
                int w = this.ignoreArea.Max(f => f.Width);
                int h = this.ignoreArea.Max(f => f.Height);
                maxIgnoreSize = new Size(w, h);
            }
        }
    }

    internal abstract class InspectEdge : InspectLine
    {
        public EdgeParam EdgeParam { get => edgeParam; }
        protected EdgeParam edgeParam;

        protected AlgoImage fullEdgeImage = null;

        public AlgoImage EdgeImage { get => this.isBuilded ? edgeImage : null; }
        protected AlgoImage edgeImage = null;

        public bool IsBuilded { get => isBuilded; }
        protected bool isBuilded;

        public static InspectEdge Create(Rectangle rectangle, float greyAverage, SensitiveParam sensitiveParam, EdgeParam edgeParam)
        {
            switch (edgeParam.EdgeFindMethod)
            {
                case EdgeParam.EEdgeFindMethod.Projection:
                    return new InspectEdgeProjection(rectangle, greyAverage, sensitiveParam, edgeParam);
                case EdgeParam.EEdgeFindMethod.Soble:
                    return new InspectEdgeSobel(rectangle, greyAverage, sensitiveParam, edgeParam);
                //case CalculatorParam.EEdgeFindMethod.Include:
                //    return new InspectEdgeInclude(rectangle, ignoreArea, greyAverage, edgeParam);
                default:
                    throw new NotImplementedException();
            }
        }

        public abstract void SetEdgeImage(DebugContext debugContext);

        public void ClearEdge(AlgoImage diffImage, DebugContextG debugContext)
        {
            if (this.isBuilded)
            {
                this.edgeImage.Save("5. Edge.bmp", debugContext);
                this.imageProcessing.Subtract(diffImage, this.edgeImage, diffImage);
            }
        }

        public abstract void ErodeEdge(AlgoImage binImage, DebugContextG debugContext);

        public InspectEdge(InspectLine inspectLine, SensitiveParam sensitiveParam, EdgeParam edgeParam)
            : base(inspectLine.Rectangle, inspectLine.GreyAverage, sensitiveParam)
        {
            this.edgeParam = edgeParam.Clone();
        }

        public InspectEdge(Rectangle rectangle, float greyAverage, SensitiveParam sensitiveParam, EdgeParam edgeParam)
            : base(rectangle, greyAverage, sensitiveParam)
        {
            this.edgeParam = edgeParam.Clone();
        }

        public override void Initialize(AlgoImage algoImage, AlgoImage diffImage, AlgoImage binalImage, AlgoImage thresholdMapImage, ImageProcessing imageProcessing, float scale)
        {
            base.Initialize(algoImage, diffImage, binalImage, thresholdMapImage, imageProcessing, scale);

            this.fullEdgeImage = ImageBuilder.Build(CalculatorBase.TypeName, this.rectangle.Size);

            this.isBuilded = false;
        }

        public override void SetImage(AlgoImage algoImage, AlgoImage diffImage, AlgoImage binalImage, AlgoImage thresholdMapImage, AlgoImage edgeMapImage, AlgoImage maskImage, Size variation, DebugContextG debugContextG)
        {
            base.SetImage(algoImage, diffImage, binalImage, thresholdMapImage, edgeMapImage, maskImage, variation, debugContextG);
        }

        public override void SetImage2(DebugContextG debugContextG)
        {
            if (this.edgeParam.Value == 0)
            {
                this.isBuilded = false;
                return;
            }

            SetEdgeImage(null);
            this.edgeImage = this.fullEdgeImage.GetSubImage(new Rectangle(Point.Empty, this.algoImage.Size));
            this.isBuilded = true;

            base.SetImage2(debugContextG);
        }

        public override void ClearImage()
        {
            base.ClearImage();

            this.isBuilded = false;
            this.fullEdgeImage?.Clear();

            this.edgeImage?.Dispose();
            this.edgeImage = null;
        }

        public override void Dispose()
        {
            base.Dispose();

            this.edgeImage?.Dispose();
            this.edgeImage = null;

            this.fullEdgeImage?.Dispose();
            this.fullEdgeImage = null;
        }

    }

    internal class InspectEdgeProjection : InspectEdge
    {
        public InspectEdgeProjection(Rectangle rectangle, float greyAverage, SensitiveParam sensitiveParam, EdgeParam edgeParam)
            : base(rectangle, greyAverage, sensitiveParam, edgeParam)
        {
        }

        public override void SetEdgeImage(DebugContext debugContext)
        {
            AlgoImage edgeImage = this.fullEdgeImage;
            //Debug.Assert(baseEdgeImage.Size == baseLineImage.Size);
            //baseLineImage.Save("BaseLineImage.bmp", debugContext);
            //baseLineImage.Save(@"d:\temp\BaseLineImage.bmp");

            Size drawSize = Size.Empty;
            Rectangle[] vertSkipRect = new Rectangle[2];

            float[] dataH = this.imageProcessing.Projection(this.algoImage, Direction.Horizontal, ProjectionType.Mean);
            //List<Point> hillListH = AlgorithmCommon.FindHill(dataH, -1, true,debugContext);
            List<Point> hillListH = AlgorithmCommon.FindHill2(dataH, 5, debugContext);
            drawSize = new Size(this.edgeParam.Width * 3, this.algoImage.Height);
            if (hillListH.Count > 0)
            {
                int maxWidth = hillListH.Max(f => f.Y - f.X);
                Point foundHill = hillListH.Find(f => (f.Y - f.X) == maxWidth);
                Rectangle srcEdgeRect = new Rectangle(new Point(foundHill.X - 2 * this.edgeParam.Width, 0), drawSize);
                this.imageProcessing.DrawRect(edgeImage, srcEdgeRect, this.edgeParam.Value, true);
                vertSkipRect[0] = new Rectangle(foundHill.X - 3, 0, 5, this.algoImage.Height);

                Rectangle dstEdgeRect = new Rectangle(new Point(foundHill.Y - this.edgeParam.Width, 0), drawSize);
                this.imageProcessing.DrawRect(edgeImage, dstEdgeRect, this.edgeParam.Value, true);
                vertSkipRect[1] = new Rectangle(foundHill.Y - 2, 0, 5, this.algoImage.Height);
            }

            float[] dataV = this.imageProcessing.Projection(this.algoImage, Direction.Vertical, ProjectionType.Mean);
            List<Point> hillListV = AlgorithmCommon.FindHill(dataV, -1, true);
            drawSize = new Size(this.algoImage.Width, this.edgeParam.Width * 3);
            foreach (Point hill in hillListV)
            {
                Rectangle srcEdgeRect = new Rectangle(new Point(0, hill.X - this.edgeParam.Width), drawSize);
                this.imageProcessing.DrawRect(edgeImage, srcEdgeRect, this.edgeParam.Value, true);
                this.imageProcessing.DrawRect(edgeImage, new Rectangle(0, hill.X - 3, this.algoImage.Width, 5), 255, true);

                Rectangle dstEdgeRect = new Rectangle(new Point(0, hill.Y - 2 * this.edgeParam.Width), drawSize);
                this.imageProcessing.DrawRect(edgeImage, dstEdgeRect, this.edgeParam.Value, true);
                this.imageProcessing.DrawRect(edgeImage, new Rectangle(0, hill.Y - 3, this.algoImage.Width, 5), 255, true);
            }


            Array.ForEach(vertSkipRect, f => this.imageProcessing.DrawRect(edgeImage, f, 255, true));
            //baseEdgeImage.Save("BaseEdgeImage.bmp", debugContext);
            //edgeImage.Save(@"d:\temp\BaseEdgeImage.bmp");
        }

        public override void ErodeEdge(AlgoImage binImage, DebugContextG debugContext)
        {

        }
    }

    internal class InspectEdgeSobel : InspectEdge
    {
        //int sobelBinValue = -1;

        protected AlgoImage tempImage = null;

        public InspectEdgeSobel(InspectLine inspectLine, SensitiveParam sensitiveParam, EdgeParam edgeParam)
            : base(inspectLine, sensitiveParam, edgeParam)
        {
        }

        public InspectEdgeSobel(Rectangle rectangle, float greyAverage, SensitiveParam sensitiveParam, EdgeParam edgeParam)
            : base(rectangle, greyAverage, sensitiveParam, edgeParam)
        {
        }

        public override void Initialize(AlgoImage algoImage, AlgoImage diffImage, AlgoImage binalImage, AlgoImage thresholdMapImage, ImageProcessing imageProcessing, float scale)
        {
            base.Initialize(algoImage, diffImage, binalImage, thresholdMapImage, imageProcessing, scale);

            this.tempImage = ImageBuilder.Build(CalculatorBase.TypeName, this.rectangle.Size);
        }

        protected void GetSobelImage(DebugContext debugContext)
        {
            AlgoImage edgeImage = this.fullEdgeImage;
            AlgoImage tempImage = this.tempImage;
            float binScale = (this.rectangle.Width < 40) ? 0.50f : 0.70f;
            int closeNum = (this.rectangle.Width < 40) ? 1 : 2;

            //lineImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-0Image.bmp");

            AlgoImage curImage = this.algoImage;
            AlgoImage prevImage = this.prevLineElement.AlgoImage;
            AlgoImage nextImage = this.nextLineElement.AlgoImage;
            this.imageProcessing.WeightedAdd(new AlgoImage[] { curImage, prevImage, nextImage }, tempImage);
            //this.tempImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-0WeightedSum.bmp");

            this.imageProcessing.Average(tempImage, edgeImage);
            //this.imageProcessing.Average(this.algoImage, edgeImage);
            //edgeImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-1Average.bmp");

            this.imageProcessing.Binarize(edgeImage, tempImage, true);
            //tempImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-2Binarize.bmp");

            this.imageProcessing.Erode(tempImage, 1);
            //tempImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-3Dilate.bmp");

            this.imageProcessing.Sobel(tempImage, edgeImage);
            //edgeImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-4Sobel.bmp");

            //if (this.sobelBinValue <= 0)
            //    this.sobelBinValue = (int)Math.Round(this.imageProcessing.Binarize(tempImage) / 2);
            //this.imageProcessing.Binarize(tempImage, edgeImage, this.sobelBinValue);
            //edgeImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-3Binarize.bmp");

            if (false)
            {
                this.imageProcessing.Close(edgeImage, tempImage, closeNum);
                tempImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-4Close.bmp");

                if (this.edgeParam.Width > 0)
                    this.imageProcessing.Dilate(tempImage, edgeImage, this.edgeParam.Width);
                else if (this.edgeParam.Width < 0)
                    this.imageProcessing.Erode(tempImage, edgeImage, -this.edgeParam.Width);
                else
                    edgeImage.Copy(tempImage);
            }
            else
            {
                if (this.edgeParam.Width != 0)
                {
                    if (this.edgeParam.Width > 0)
                        this.imageProcessing.Dilate(edgeImage, edgeImage, this.edgeParam.Width);
                    else if (this.edgeParam.Width < 0)
                        this.imageProcessing.Erode(edgeImage, edgeImage, -this.edgeParam.Width);
                }
            }

            //edgeImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-5Final.bmp");
        }

        public override void SetEdgeImage(DebugContext debugContext)
        {
            if (this.edgeParam.Value == 0)
            {
                return;
            }

            GetSobelImage(debugContext);
            this.imageProcessing.Clipping(this.fullEdgeImage, this.fullEdgeImage, 0, 0, this.edgeParam.Value, this.edgeParam.Value);
            //edgeImage.Save(@"C:\temp\GetEdgeImageSoble\EdgeImage-6Div.bmp");

        }

        public override void ClearImage()
        {
            base.ClearImage();

            this.tempImage?.Clear();
        }

        public override void Dispose()
        {
            base.Dispose();

            this.tempImage?.Dispose();
            this.tempImage = null;

            //this.sobelBinValue = -1;
        }


        public override void ErodeEdge(AlgoImage binImage, DebugContextG debugContext)
        {
            //binImage.Save(@"D:\temp\binImage.bmp");
            //this.edgeImage.Save(@"D:\temp\edgeImage.bmp");

            // 엣지 영역 마스킹
            this.imageProcessing.And(binImage, this.edgeImage, this.tempImage);
            //this.tempImage.Save(@"D:\temp\tempImage.bmp");

            // 침식
            this.imageProcessing.Morphology(this.tempImage, this.tempImage, ImageProcessing.MorphologyType.Erode, ImageProcessing.Kernal_3x3_Cross, 1);
            //this.tempImage.Save(@"D:\temp\tempImage.bmp");

            // 마스킹 영역만 원본 이미지에 덮어씀.
            binImage.MaskingCopy(this.tempImage, this.edgeImage);
            //binImage.Save(@"D:\temp\binImage.bmp");

            // 엣지 경계에서 끊긴 부분 매꿔야 함..
            this.imageProcessing.Close(binImage, 1);
            //binImage.Save(@"D:\temp\binImage.bmp");
        }
    }

    //internal class InspectEdgeInclude : InspectEdgeSobel
    //{
    //    AlgoImage maskImage = null;
    //    AlgoImage seedImage = null;
    //    BlobParam blobParam = null;
    //    BlobParam filterParam = null;
    //    ResconstructParam resconstructParam = null;

    //    public InspectEdgeInclude(InspectLine inspectLine, EdgeParam edgeParam)
    //        : base(inspectLine, edgeParam)
    //    {
    //    }

    //    public InspectEdgeInclude(Rectangle rectangle, Rectangle[] ignoreArea, float greyAverage, EdgeParam edgeParam)
    //        : base(rectangle, ignoreArea, greyAverage, edgeParam)
    //    {
    //    }

    //    public override void Initialize(AlgoImage algoImage, AlgoImage diffImage, AlgoImage binalImage, ImageProcessing imageProcessing, float scale)
    //    {
    //        base.Initialize(algoImage, diffImage, binalImage, imageProcessing, scale);

    //        this.maskImage = ImageBuilder.Build(CalculatorBase.TypeName, rectangle.Size);
    //        this.seedImage = ImageBuilder.Build(CalculatorBase.TypeName, rectangle.Size);

    //        this.blobParam = new BlobParam();
    //        this.filterParam = new BlobParam();
    //        this.resconstructParam = new ResconstructParam() { IsGrayImage = true, AllIncluded = true };
    //    }

    //    public override bool SetEdgeImage(AlgoImage lineImage, DebugContext debugContext)
    //    {
    //        bool ok = GetSobelImage(lineImage, debugContext);
    //        if (!ok)
    //            return false;

    //        this.imageProcessing.Not(this.edgeImage, this.maskImage);
    //        this.imageProcessing.Clipping(this.edgeImage, this.edgeImage, 0, 0, this.edgeParam.EdgeValue, this.edgeParam.EdgeValue);
    //        //this.edgeImage.Save(@"C:\temp\edgeImage2.bmp");

    //        return true;
    //    }

    //    public override void ClearImage()
    //    {
    //        base.ClearImage();

    //        this.maskImage?.Clear();
    //        this.seedImage?.Clear();
    //    }

    //    public override void Dispose()
    //    {
    //        base.Dispose();

    //        this.maskImage?.Dispose();
    //        this.maskImage = null;

    //        this.seedImage?.Dispose();
    //        this.seedImage = null;
    //    }

    //    public override void ClearEdge(AlgoImage diffImage, DebugContextG debugContext)
    //    {
    //        this.edgeImage.Save("5.0 Edge.bmp", debugContext);
    //        this.maskImage.Save("5.1 Mask.bmp", debugContext);

    //        this.imageProcessing.Subtract(diffImage, this.edgeImage, this.tempImage);
    //        this.tempImage.Save("5.2 Subtract.bmp", debugContext);

    //        this.imageProcessing.And(this.tempImage, this.maskImage, this.seedImage);
    //        this.imageProcessing.Binarize(this.seedImage, this.seedImage, 20);
    //        this.seedImage.Save("5.3 Mask.bmp", debugContext);

    //        this.imageProcessing.ReconstructIncludeBlob(this.tempImage, diffImage, this.seedImage, this.resconstructParam);
    //        diffImage.Save("5.4 Resconstruct.bmp", debugContext);
    //    }
    //}

    internal static class CalculatorV2Extender
    {
        public static InspectRegion[] Train(Model model, CalculatorParam calculatorParam, bool halfScale, float scale)
        {
            CalculatorModelParam calculatorModelParam = calculatorParam.ModelParam;
            LineSetParam lineSetParam = new LineSetParam(calculatorParam.AdaptivePairing, calculatorParam.BoundaryPairStep, calculatorModelParam.IgnoreMethod, calculatorModelParam.IgnoreSideLine);
            SensitiveParam sensitiveParam = model.CalculatorModelParam.SensitiveParam.Clone();
            EdgeParam edgeParam = model.CalculatorModelParam.EdgeParam.Clone();

            List<RegionInfoG> inspectRegionInfoG = model.CalculatorModelParam.RegionInfoCollection;
            int length = inspectRegionInfoG.Count;
            InspectRegion[] inspectRegions = new InspectRegion[length];

            for (int i = 0; i < length; i++)
            {
                RegionInfoG regionInfoG = inspectRegionInfoG[i];
                InspectRegion inspectRegion = GetInspectRegion(regionInfoG, lineSetParam, sensitiveParam, edgeParam);
                inspectRegion.Initialize(calculatorParam, sensitiveParam, scale);
                inspectRegions[i] = inspectRegion;
            }
            return inspectRegions;
        }

        private static InspectRegion GetInspectRegion(RegionInfoG regionInfoG, LineSetParam lineSetParam, SensitiveParam sensitiveParam, EdgeParam edgeParam)
        {
            if (!regionInfoG.Use)
                return new InspectRegion(regionInfoG, new InspectLineSet[0]);

            List<InspectElement>[] inspectElementLists = null;
            List<int> groupList = new List<int>();
            regionInfoG.InspectElementList.ForEach(f =>
            {
                if (!groupList.Contains(f.Group))
                    groupList.Add(f.Group);
            });

            if (regionInfoG.OddEvenPair)
            {
                inspectElementLists = new List<InspectElement>[groupList.Count * 2];
                groupList.ForEach(f =>
                {
                    List<InspectElement> list = regionInfoG.InspectElementList.FindAll(g => g.Group == f);
                    inspectElementLists[2 * f] = list.Where((g, i) => i % 2 == 0).ToList();
                    inspectElementLists[2 * f + 1] = list.Where((g, i) => i % 2 == 1).ToList();
                });
            }
            else
            {
                inspectElementLists = new List<InspectElement>[groupList.Count];
                groupList.ForEach(f =>
                {
                    inspectElementLists[f] = regionInfoG.InspectElementList.FindAll(g => g.Group == f);
                });
            }

            InspectLineSet[] inspectLineSets = GetInspectLineSet(inspectElementLists, lineSetParam, sensitiveParam, edgeParam);
            InspectRegion inspectRegion = new InspectRegion(regionInfoG, inspectLineSets);
            return inspectRegion;
        }

        private static InspectLineSet[] GetInspectLineSet(List<InspectElement>[] inspectElementLists, LineSetParam lineSetParam, SensitiveParam sensitiveParam, EdgeParam edgeParam)
        {
            List<InspectLineSet> inspectLineSetList = new List<InspectLineSet>();
            Array.ForEach(inspectElementLists, inspectElementList =>
            {
                if (inspectElementList.Count > 2)
                {
                    InspectLine[] inspectLines = GetInspectLines(inspectElementList, lineSetParam, sensitiveParam, edgeParam);
                    InspectEdge inspectEdge = (InspectEdge)Array.Find(inspectLines, f => f is InspectEdge);
                    ImageProcessing imageProcessing = ImageProcessing.Create(CalculatorBase.TypeName);

                    InspectLineSet inspectLineSet = new InspectLineSet(inspectLines, lineSetParam, imageProcessing);
                    inspectLineSetList.Add(inspectLineSet);
                }
            });

            return inspectLineSetList.ToArray();
        }

        private static InspectLine[] GetInspectLines(List<InspectElement> inspectElementList, LineSetParam lineSetParam, SensitiveParam sensitiveParam, EdgeParam edgeParam)
        {
            int first = 0;
            int last = inspectElementList.Count - 1;
            int src = lineSetParam.IgnoreSideLine ? 1 : 0;
            int dst = inspectElementList.Count - (lineSetParam.IgnoreSideLine ? 2 : 1);
            int mid = (src + dst) / 2;
            int cnt = dst - src + 1;
            int edgeCnt = edgeParam.Multi ? cnt / 10 + 1 : 1;
            int[] edgeIdxs = new int[edgeCnt];
            float step = cnt * 1f / (edgeCnt);
            for (int i = 0; i < edgeCnt; i++)
            {
                int idx = (int)Math.Round(step * (i + 0.5f)) + src;
                // 해당 라인이 Dontcare 일 경우 가까운 라인으로 이동.
                int offs = 0; // 0, +1, -1, +2, -2, +3, -3, ...
                while (inspectElementList[idx + offs].HasDontcare)
                {
                    offs = -offs + (offs <= 0 ? +1 : 0);
                    if (idx + offs < src || dst < idx + offs)
                    {
                        offs = 0;
                        break;
                    }
                }
                edgeIdxs[i] = idx + offs;
            }

            List<InspectLine> inspectLineList = new List<InspectLine>();
            for (int i = 0; i < inspectElementList.Count; i++)
            {
                InspectElement inspectElement = inspectElementList[i];

                InspectLine inspectLine = edgeIdxs.Contains(i) ?
                    InspectEdge.Create(inspectElement.Rectangle, inspectElement.GreyAverage, sensitiveParam, edgeParam) :
                    new InspectLine(inspectElement.Rectangle, inspectElement.GreyAverage, sensitiveParam);
                inspectLine.DontCare = inspectElement.Dontcare;
                inspectLineList.Add(inspectLine);
            }

            List<InspectLine> careLineElementList = inspectLineList.FindAll(f => f.IsDoncare == false);
            bool adabpable = careLineElementList.Count >= 3;
            int minCare = 0, maxCare = careLineElementList.Count - 1;
            for (int i = first; i <= last; i++)
            {
                int curIdx = i;
                int prevIdx, nextIdx;
                if (!adabpable || !lineSetParam.AdaptivePairing || inspectLineList[i].IsDoncare)
                {
                    prevIdx = (i != first) ? i - 1 : i + lineSetParam.BoundaryPairStep;
                    nextIdx = (i != last) ? i + 1 : i - lineSetParam.BoundaryPairStep;
                }
                else
                {
                    // Care 라인 끼리 링크
                    int careIdx = careLineElementList.IndexOf(inspectLineList[curIdx]);
                    int carePrevIdx = (careIdx != minCare) ? careIdx - 1 : careIdx + lineSetParam.BoundaryPairStep;
                    int careNextIdx = (careIdx != maxCare) ? careIdx + 1 : careIdx - lineSetParam.BoundaryPairStep;

                    prevIdx = inspectLineList.IndexOf(careLineElementList[carePrevIdx]);
                    nextIdx = inspectLineList.IndexOf(careLineElementList[careNextIdx]);
                }

                InspectEdge inspectEdge;
                int nextEdgeIdx = inspectLineList.FindIndex(i, f => f is InspectEdge);
                int prevEdgeIdx = inspectLineList.FindLastIndex(i, f => f is InspectEdge);
                if (nextEdgeIdx < 0 && prevEdgeIdx < 0)
                    inspectEdge = null;
                else if (nextEdgeIdx < 0)
                    inspectEdge = (InspectEdge)inspectLineList[prevEdgeIdx];
                else if (prevEdgeIdx < 0)
                    inspectEdge = (InspectEdge)inspectLineList[nextEdgeIdx];
                else
                    inspectEdge = (InspectEdge)inspectLineList[(nextEdgeIdx - i < i - prevEdgeIdx) ? nextEdgeIdx : prevEdgeIdx];
                //int dd = inspectLineList.IndexOf(inspectEdge);

                inspectLineList[i].Link(inspectLineList[prevIdx], inspectLineList[nextIdx], inspectEdge);
            }

            //inspectLineList.RemoveAll(f => f.IsLinked == false);
            return inspectLineList.ToArray();
        }
    }
}