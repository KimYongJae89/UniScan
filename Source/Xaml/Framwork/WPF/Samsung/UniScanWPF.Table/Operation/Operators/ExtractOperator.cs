using DynMvp.Base;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using DynMvp.Vision.Matrox;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml;
using UniEye.Base.Settings;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Operation.Operators
{
    public class ExtractOperator : TaskOperator
    {        
        public ExtractOperatorSettings Settings { get => settings; }
        ExtractOperatorSettings settings;
        

        public ExtractOperator():base()
        {
            settings = new ExtractOperatorSettings();
        }

        public void StartExtract(OperatorResult operatorResult)
        {
            if (operatorResult.Type != ResultType.Scan)
                return;

            this.OperatorState = OperatorState.Run;
            ScanOperatorResult scanOperatorResult = (ScanOperatorResult)operatorResult;
            Task task = new Task(() =>
            {
                ExtractOperatorResult extractOperatorResult = Extract(scanOperatorResult);
                SystemManager.Instance().OperatorProcessed(extractOperatorResult);
                this.CurProgressSteps++;
            }, cancellationTokenSource.Token);

            AddTask(task);

            task.Start();
            if (scanOperatorResult.FlowPosition == 0)
                task.Wait();
        }

        private BlobRectList MultiRegionBlob(AlgoImage processBuffer, ImageProcessing imageProcessing, BlobParam blobParam)
        {
            bool eraseBorderBlob = blobParam.EraseBorderBlobs;
            if (eraseBorderBlob)
                blobParam.EraseBorderBlobs = false;

            Rectangle interestRect = new Rectangle(0, 0, processBuffer.Width, processBuffer.Height);

            int blobSectionCount = 10;
            RectangleF[] blobSectionRectArray = new RectangleF[blobSectionCount];

            int sectionHeight = interestRect.Height / blobSectionCount;
            for (int i = 0; i < blobSectionCount; i++)
            {
                blobSectionRectArray[i].Location = new PointF(interestRect.Left, 0 + (i * sectionHeight));
                blobSectionRectArray[i].Size = new SizeF(interestRect.Width, sectionHeight);
                if (i == blobSectionCount - 1)
                    blobSectionRectArray[i].Size = new SizeF(interestRect.Width, interestRect.Height - (i * sectionHeight));
            }

            List<Tuple<Rectangle, BlobRectList>> blobRectTupleList = new List<Tuple<Rectangle, BlobRectList>>();
            Parallel.ForEach(blobSectionRectArray, sectionRect =>
            {
                Rectangle rect = Rectangle.Truncate(sectionRect);
                AlgoImage inspectProcessImage = processBuffer.GetSubImage(rect);

                lock (blobRectTupleList)
                    blobRectTupleList.Add(new Tuple<Rectangle, BlobRectList>(rect, imageProcessing.Blob(inspectProcessImage, blobParam)));

                inspectProcessImage.Dispose();
            });
            
            blobRectTupleList = blobRectTupleList.OrderByDescending(tuple => tuple.Item1.Y).Reverse().ToList();

            Tuple<Rectangle, BlobRectList> blobTuple = blobRectTupleList.Aggregate((prev, next) =>
            {
                BlobRectList blobRectList = imageProcessing.BlobMerge(prev.Item2, next.Item2, blobParam);

                BufferManager.Instance().AddDispoableObj(prev.Item2);
                BufferManager.Instance().AddDispoableObj(next.Item2);

                return new Tuple<Rectangle, BlobRectList>(Rectangle.Union(prev.Item1, next.Item1), blobRectList);
            });

            BlobRectList mergeBlobRectList = blobTuple.Item2;
            
            if (eraseBorderBlob)
            {
                mergeBlobRectList = imageProcessing.EreseBorderBlobs(blobTuple.Item2, blobParam);
                blobParam.EraseBorderBlobs = true;
            }
            
            return mergeBlobRectList;
         }

        private ExtractOperatorResult Extract(ScanOperatorResult scanOperatorResult)
        {
            string debugContextSubPath = string.Format("ExtractOperator_{0}", scanOperatorResult.FlowPosition);
            DebugContext debugContext = this.GetDebugContext(debugContextSubPath);
            LogHelper.Debug(LoggerType.Operation, string.Format("ExtractOperator::Extract{0} - Start", scanOperatorResult.FlowPosition), true);

            AlgoImage topAlgoImage = scanOperatorResult.TopLightImage;
            int topBinValue = SystemManager.Instance().CurrentModel.BinarizeValueTop;
            AlgoImage backAlgoImage = scanOperatorResult.BackLightImage;
            //int backBinValue = (int)(SystemManager.Instance().OperatorManager.LightTuneOperator.Settings.InitialBackLightValue * 0.4);
            int backBinValue = SystemManager.Instance().CurrentModel.BinarizeValueBack;
            topAlgoImage.Save(@"topAlgoImage.bmp", debugImageScale, debugContext);
            backAlgoImage.Save(@"backAlgoImage.bmp", debugImageScale, debugContext);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(topAlgoImage);

            int width = scanOperatorResult.TopLightImage.Width;
            int height = scanOperatorResult.TopLightImage.Height;

            // 이진화 영상
            LogHelper.Debug(LoggerType.Operation, string.Format("ExtractOperator::Extract{0} - CreateSheetBuffer", scanOperatorResult.FlowPosition), true);

            // 백라이트 영상에서 추출한 전극
            AlgoImage sheetBuffer = BufferManager.Instance().GetSheetBuffer(scanOperatorResult.FlowPosition);
            imageProcessing.Binarize(backAlgoImage, sheetBuffer, backBinValue, true);
            sheetBuffer.Save(@"sheetBuffer1.bmp", debugImageScale, debugContext);
            imageProcessing.Close(sheetBuffer, 50); // Close 수행
            sheetBuffer.Save(@"sheetBuffer2.bmp", debugImageScale, debugContext);

            // 탑라이트 영상에서 추출한 전극
            AlgoImage maskBuffer = BufferManager.Instance().GetMaskBuffer(scanOperatorResult.FlowPosition);
            imageProcessing.Binarize(topAlgoImage, maskBuffer, topBinValue, true);
            maskBuffer.Save(@"maskBuffer1.bmp", debugImageScale, debugContext);

            if (true)
            {
                imageProcessing.Close(maskBuffer, 100); // Close 수행
            }
            else
            {
                // Close 수행
                imageProcessing.Dilate(maskBuffer, maskBuffer, (int)(settings.MaxMarginLength / DeveloperSettings.Instance.Resolution));
                maskBuffer.Save(@"maskBuffer2.bmp", debugImageScale, debugContext);
                imageProcessing.Erode(maskBuffer, (int)Math.Max(1, (settings.MaxMarginLength / DeveloperSettings.Instance.Resolution) + 0));
                maskBuffer.Save(@"maskBuffer4.bmp", debugImageScale, debugContext);
            }
            maskBuffer.Save(@"maskBuffer2.bmp", debugImageScale, debugContext);

            // sheetBuffer: 탑라이트 인쇄부 & 백라이트 인쇄부 -> 인쇄부 마스크
            imageProcessing.And(maskBuffer, sheetBuffer, sheetBuffer);
            sheetBuffer.Save(@"sheetBuffer3.bmp", debugImageScale, debugContext);

            // sheetBuffer에서 가장 큰 블랍만 추출
            LogHelper.Debug(LoggerType.Operation, string.Format("ExtractOperator::Extract{0} - Blob", scanOperatorResult.FlowPosition), true);
            BlobParam sheetBlobParam = new BlobParam()
            {
                SelectBorderBlobs = true,
                SelectLabelValue = true,
                Connectivity4 = true
            };
            BlobRectList sheetBlobRectList = imageProcessing.Blob(maskBuffer, sheetBlobParam);
            BlobRect maxAreaBlob = sheetBlobRectList.GetMaxAreaBlob();
            float areaRatio = maxAreaBlob == null ? 0 : (maxAreaBlob.Area * 100f) / (sheetBuffer.Width * sheetBuffer.Height);

            imageProcessing.Clear(sheetBuffer, 0);
            LogHelper.Debug(LoggerType.Operation, string.Format("ExtractOperator::Extract{0} - DrawBlob", scanOperatorResult.FlowPosition), true);
            imageProcessing.DrawBlob(sheetBuffer, sheetBlobRectList, new BlobRect[] { maxAreaBlob }, new DrawBlobOption() { SelectBlob = true });
            sheetBuffer.Save(@"sheetBuffer4.bmp", debugImageScale, debugContext);
            BufferManager.Instance().AddDispoableObj(sheetBlobRectList);

            // maskBuffer: 탑이미지 이진화 & 인쇄부 마스크 -> 탑라이트 전극
            imageProcessing.Binarize(topAlgoImage, maskBuffer, topBinValue, true);
            maskBuffer.Save(@"maskBuffer3.bmp", debugImageScale, debugContext);
            imageProcessing.Close(maskBuffer, 5);
            maskBuffer.Save(@"maskBuffer4.bmp", debugImageScale, debugContext);
            imageProcessing.And(maskBuffer, sheetBuffer, maskBuffer);
            maskBuffer.Save(@"maskBuffer5.bmp", debugImageScale, debugContext);

            List<BlobRect> blobRectList = null;
            AlgoImage blobAlgoImage = maskBuffer;
            bool resize = false;
            if (this.settings.BlobScaleFactor != 1)
            {
                Size blobResize = Size.Round(DrawingHelper.Mul(new Size(width, height), this.settings.BlobScaleFactor));
                blobAlgoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, blobResize);
                imageProcessing.Resize(maskBuffer, blobAlgoImage);
                resize = true;
            }

            LogHelper.Debug(LoggerType.Operation, string.Format("ExtractOperator::Extract{0} - Blob2", scanOperatorResult.FlowPosition), true);
            BlobParam blobParam = new BlobParam()
            {
                SelectArea = true,
                SelectBoundingRect = true,
                EraseBorderBlobs = false,
                SelectLabelValue = true,
                SelectCenterPt = true,
                Connectivity4 = true,
                SelectRotateRect = true,

                RotateHeightMin = settings.MinPatternLength / DeveloperSettings.Instance.Resolution,
                RotateWidthMin = settings.MinPatternLength / DeveloperSettings.Instance.Resolution,
                RotateHeightMax = (settings.UseMaxPatternLength) ? settings.MaxPatternLength / DeveloperSettings.Instance.Resolution : 0,
                RotateWidthMax = (settings.UseMaxPatternLength) ? settings.MaxPatternLength / DeveloperSettings.Instance.Resolution : 0
            };

            BlobRectList blobList = imageProcessing.Blob(blobAlgoImage, blobParam);
            //blobAlgoImage.Save(@"C:\temp\blobAlgoImage.bmp");
            blobRectList = blobList.GetList();

            imageProcessing.Clear(blobAlgoImage, 0);
            //blobAlgoImage.Save(@"C:\temp\blobAlgoImage2.bmp");
            LogHelper.Debug(LoggerType.Operation, string.Format("ExtractOperator::Extract{0} - DrawBlob2, {1}[EA]", scanOperatorResult.FlowPosition, blobRectList.Count), true);
            imageProcessing.DrawBlob(blobAlgoImage, blobList, null, new DrawBlobOption() { SelectBlob = true });
            //blobAlgoImage.Save(@"C:\temp\blobAlgoImage3.bmp");
            if (resize)
            {
                imageProcessing.Resize(blobAlgoImage, maskBuffer);
                blobAlgoImage.Dispose();
            }
            //maskBuffer.Save(string.Format("maskBuffer_{0}.bmp", scanOperatorResult.FlowPosition), 0.5f, new DebugContext(true, @"C:\temp"));

            blobRectList.RemoveAll(f => f.IsTouchBorder);
            BufferManager.Instance().AddDispoableObj(blobList);

            // 시트 외곽지점 줄임. 패턴/마진불량시 제외. Blob 후 줄여야 해상도 검사에 영향이 없음.
            sheetBuffer.Save(@"sheetBuffer5.bmp", debugImageScale, debugContext);
            imageProcessing.Erode(sheetBuffer, (int)Math.Max(1, (settings.MaxMarginLength / DeveloperSettings.Instance.Resolution)));
            sheetBuffer.Save(@"sheetBuffer5.bmp", debugImageScale, debugContext);

            // To Bitmap
            LogHelper.Debug(LoggerType.Operation, string.Format("ExtractOperator::Extract{0} - ToBitmap", scanOperatorResult.FlowPosition), true);
            Size bitmapResize = new Size((int)(width * DispResizeRatio), (int)(height * DispResizeRatio));
            BitmapSource maskBufferBitmap = BuildBitmapSource(maskBuffer, bitmapResize);
            BitmapSource sheetBufferBitmap = BuildBitmapSource(sheetBuffer, bitmapResize);

            //Helper.WPFImageHelper.SaveBitmapSource(@"C:\temp\maskBufferBitmap.bmp", maskBufferBitmap);


            // Get Sheet Rect
            RectangleF sheetRect = RectangleF.Empty;
            if (blobRectList.Count > 0)
                sheetRect = blobRectList.Aggregate<BlobRect, RectangleF>(blobRectList[0].BoundingRect, ((f, g) => RectangleF.Union(f, g.BoundingRect)));
            //blobRectList.ForEach(f => sheetRect = RectangleF.Union(sheetRect, f.BoundingRect));
            sheetRect = DrawingHelper.Mul(sheetRect, 1 / this.settings.BlobScaleFactor);
            //blobAlgoImage.Save(@"C:\temp\blobAlgoImage.bmp");
            //File.WriteAllText(@"C:\temp\blobAlgoImage.txt", $"{sheetRect.Left},{sheetRect.Bottom}");

            // Get Vertex Points
            //blobAlgoImage.Save(@"C:\temp\blobAlgoImage.bmp");
            Point[] vertexPoints = GetVertexPoints(blobRectList, debugContext);

            for (int i = 0; i < vertexPoints.Length; i++)
                vertexPoints[i] = DrawingHelper.Mul(vertexPoints[i], 1 / this.settings.BlobScaleFactor);
            LogHelper.Debug(LoggerType.Inspection, string.Format("FlowPosition {0} SheetRect is {1}", scanOperatorResult.FlowPosition, sheetRect));

            if (blobRectList.Count == 0)
            // 전극 없음
            {
                //return new ExtractOperatorResult(resultKey, new Exception("No Pattern exist"), scanOperatorResult);
                return new ExtractOperatorResult(resultKey, scanOperatorResult,
                    sheetBuffer, maskBuffer, sheetBufferBitmap, maskBufferBitmap,
                    Rectangle.Round(sheetRect),
                    blobRectList, vertexPoints, this.settings.BlobScaleFactor);
            }

            // 전극 있음
            // Calculate Slope
            if (scanOperatorResult.FlowPosition == 0)
            {
                List<Rectangle> pickList = new List<Rectangle>();
                using (AlgoImage pickImage = maskBuffer.Clone())
                {
                    List<Rectangle> boundRectList = blobRectList
                        .Select(f => Rectangle.Round(DrawingHelper.Mul(f.BoundingRect, 1 / this.settings.BlobScaleFactor)))
                        .OrderBy(f => MathHelper.GetLength(Point.Empty, f.Location))
                        .ToList();

                    Rectangle pick = boundRectList.First();
                    while (!pick.IsEmpty)
                    {
                        boundRectList.Remove(pick);
                        Rectangle lastPick = pickList.LastOrDefault();
                        if(!lastPick.IsEmpty)
                        {
                            //if((pick.Right  < lastPick.Left ) || (lastPick.Right < pick.Left))
                            //    break;
                        }
                        pickList.Add(pick);

                        imageProcessing.DrawText(pickImage, pick.Location, 127, pickList.IndexOf(pick).ToString(), 2);
                        //pickImage.Save(@"C:\temp\pickImage.bmp");

                        //boundRectList = boundRectList.OrderBy(f => Math.Abs(pick.Left - f.Left)).ToList();
                        boundRectList = boundRectList.OrderBy(f => MathHelper.GetLength(new Point(pick.Left, pick.Bottom), f.Location)).ToList();
                        pick = boundRectList.FirstOrDefault(f => pick.Bottom < f.Top);
                    }
                    pickImage.Save(@"C:\temp\pickImage.bmp", debugContext);
                }

                List<PointF> pickPointList = pickList.Select(f => new PointF(f.Left, (f.Top + f.Bottom) / 2)).ToList();
                double meanX = pickPointList.Average(f => f.X);
                double meanY = pickPointList.Average(f => f.Y);

                double nom = pickPointList.Sum(f => (f.Y - meanY) * (f.X - meanX));
                double denom = pickPointList.Sum(f => Math.Pow(f.X - meanX, 2));

                double angle = Math.Atan2(nom, denom) * 180 / Math.PI;
                if (angle < 0)
                    angle += 180;
                double angleDiff = Math.Abs(90 - angle);
                if (angleDiff > settings.AllowRotateLimitDeg)
                {
                    string format = WpfControlLibrary.Helper.LocalizeHelper.GetString("Sheet is rotated. ({0:F1}[deg])");
                    return new ExtractOperatorResult(resultKey, new Exception(string.Format(format, angle)), scanOperatorResult);
                }
                LogHelper.Debug(LoggerType.Inspection, $"ExtractOperator::Extract{scanOperatorResult.FlowPosition} - Rotate angle: {angle:F1}");
            }
            //

            double sheetHeightMm = maxAreaBlob.BoundingRect.Height * DeveloperSettings.Instance.Resolution / 1000.0;
            int patternCount = blobRectList.Count;

            LogHelper.Debug(LoggerType.Operation, string.Format("ExtractOperator::Extract{0} - End.", scanOperatorResult.FlowPosition), true);
            return new ExtractOperatorResult(resultKey, scanOperatorResult,
                sheetBuffer, maskBuffer, sheetBufferBitmap, maskBufferBitmap,
                Rectangle.Round(sheetRect),
                blobRectList, vertexPoints, this.settings.BlobScaleFactor);
        }

        private BitmapSource BuildBitmapSource(AlgoImage algoImage, Size resize)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
            BitmapSource bitmapSource = null;
            using (AlgoImage resizeAlgoImage = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, resize))
            {
                ip.Resize(algoImage, resizeAlgoImage);
                bitmapSource = resizeAlgoImage.ToBitmapSource();
            }
            return bitmapSource;
        }

        private Point[] GetVertexPoints(List<BlobRect> blobRectList, DebugContext debugContext)
        {
            List<Point> vertexPointList = new List<Point>();
            if (blobRectList.Count == 0)
                return vertexPointList.ToArray();

            // 블랍의 평균 크기
            double meanArea = blobRectList.Average(f => f.Area);
            if (meanArea < 100)
                return vertexPointList.ToArray();

            Size meanSize = new Size()
            {
                Width = (int)Math.Round(blobRectList.Average(f => f.BoundingRect.Width)),
                Height = (int)Math.Round(blobRectList.Average(f => f.BoundingRect.Height))
            };

            // 중심과 가장 가까이 있는 블랍을 가져온다.
            Point center = new Point()
            {
                X = (int)Math.Round(blobRectList.Average(f => f.CenterPt.X)),
                Y = (int)Math.Round(blobRectList.Average(f => f.CenterPt.Y))
            };
            List<Tuple<BlobRect, double>> tupleList = blobRectList.ConvertAll(f => new Tuple<BlobRect, double>(f, Math.Sqrt(Math.Pow(f.CenterPt.X - center.X, 2) + Math.Pow(f.CenterPt.Y - center.Y, 2))));
            tupleList.Sort((f, g) => f.Item2.CompareTo(g.Item2));
            BlobRect centerBlobRect = tupleList.First().Item1;

            // Rect간 거리가 ratio 이하인 블랍을 Union,
            float ratio = 0.8f;
            List<BlobRect> validBlobRectList = new List<BlobRect>();
            Rectangle rectangle = Rectangle.Round(centerBlobRect.BoundingRect);
            tupleList.ForEach(f =>
            {
                Rectangle blobRect = Rectangle.Round(f.Item1.BoundingRect);
                bool union = rectangle.IntersectsWith(blobRect);
                if (union == false)
                {
                    float diffL = Math.Max(0, rectangle.Left - blobRect.Right);
                    float diffT = Math.Max(0, rectangle.Top - blobRect.Bottom);
                    float diffR = Math.Max(0, blobRect.Left - rectangle.Right);
                    float diffB = Math.Max(0, blobRect.Top - rectangle.Bottom);

                    union = Math.Max(diffL, diffR) < meanSize.Width * ratio && Math.Max(diffT, diffB) < meanSize.Height * ratio;
                }

                if (union)
                {
                    rectangle = Rectangle.Union(rectangle, Rectangle.Round(f.Item1.BoundingRect));
                    validBlobRectList.Add(f.Item1);
                }
            });

            // 네 방향 꼭지점에 위치한 BlobRect를 찾는다.
            PointF[] refPt = new PointF[] {
                    new PointF(rectangle.Left, rectangle.Top),
                    new PointF(rectangle.Right, rectangle.Top),
                    new PointF(rectangle.Right, rectangle.Bottom),
                    new PointF(rectangle.Left, rectangle.Bottom)
                };

            for (int i = 0; i < refPt.Length; i++)
            {
                PointF pt1 = refPt[i];
                BlobRect vertexBlobRect = validBlobRectList.OrderBy(f =>
                {
                    PointF pt2 = DrawingHelper.GetPoints(f.BoundingRect, 0)[i];
                    return MathHelper.GetLength(pt1, pt2);
                }).FirstOrDefault();
                vertexPointList.Add(Point.Round(DrawingHelper.GetPoints(vertexBlobRect.BoundingRect, 0)[i]));
            }
            return vertexPointList.ToArray();
        }
    }

    public class ExtractOperatorResult : OperatorResult
    {
        ScanOperatorResult scanOperatorResult;
        List<BlobRect> blobRectList;
        AlgoImage sheetBuffer;
        AlgoImage maskBuffer;
        BitmapSource sheetBufferBitmap;
        BitmapSource maskBufferBitmap;

        Rectangle sheetRect;
        Point[] vertexPoints;
        float blobScaleFactor;

        public List<BlobRect> BlobRectList { get => blobRectList; }
        public ScanOperatorResult ScanOperatorResult { get => scanOperatorResult; }
        public AlgoImage SheetBuffer { get => sheetBuffer; }
        public AlgoImage MaskBuffer { get => maskBuffer; }
        public BitmapSource SheetBufferBitmap { get => sheetBufferBitmap; }
        public BitmapSource MaskBufferBitmap { get => maskBufferBitmap; }

        public Rectangle SheetRect { get => sheetRect; }
        public Point[] VertexPoints { get => vertexPoints; }
        public float BlobScaleFactor { get => blobScaleFactor; }

        public int PatternCount { get => blobRectList == null ? 0 : blobRectList.Count; }

        public ExtractOperatorResult(ResultKey resultKey, 
            ScanOperatorResult scanOperatorResult, 
            AlgoImage sheetBuffer, AlgoImage maskBuffer, BitmapSource sheetBufferBitmap, BitmapSource maskBufferBitmap,
            Rectangle sheetRect, List<BlobRect> blobRectList, Point[] vertexPoints, float blobScaleFactor)
            : base(ResultType.Extract, resultKey, DateTime.Now)
        {
            this.scanOperatorResult = scanOperatorResult;
            this.sheetBuffer = sheetBuffer;
            this.maskBuffer = maskBuffer;
            this.sheetBufferBitmap = sheetBufferBitmap;
            this.maskBufferBitmap = maskBufferBitmap;
            this.sheetRect = sheetRect;
            this.blobRectList = blobRectList;
            this.vertexPoints = vertexPoints;
            this.blobScaleFactor = blobScaleFactor;
        }

        public ExtractOperatorResult(ResultKey resultKey, Exception exception, 
            ScanOperatorResult scanOperatorResult)
            : base(ResultType.Extract, resultKey, DateTime.Now, exception)
        {
            this.scanOperatorResult = scanOperatorResult;
        }

        protected override string GetLogMessage()
        {
            return string.Format("FlowPosition,{0}", this.scanOperatorResult.FlowPosition);
        }
    }

    public class ExtractOperatorSettings : OperatorSettings
    {
        [CatecoryAttribute("Extract"), NameAttribute("Max Count")]
        public int MaxCount { get => maxCount; set => maxCount = value; }
        int maxCount = 50000;

        [CatecoryAttribute("Extract"), NameAttribute("Use Max Pattern Length")]
        public bool UseMaxPatternLength { get => useMaxPatternLength; set => useMaxPatternLength = value; }
        bool useMaxPatternLength = false;

        [CatecoryAttribute("Extract"), NameAttribute("Min Pattern Length")]
        public float MinPatternLength { get => minPatternLength; set => minPatternLength = value; }
        float minPatternLength = 100;

        [CatecoryAttribute("Extract"), NameAttribute("Max Pattern Length")]
        public float MaxPatternLength { get => maxPatternLength; set => maxPatternLength = value; }
        float maxPatternLength = 0;

        [CatecoryAttribute("Extract"), NameAttribute("Max Margin Length")]
        public int MaxMarginLength { get => maxMarginLength; set => maxMarginLength = value; }
        int maxMarginLength = 500;

        [CatecoryAttribute("Extract"), NameAttribute("Blobing Scale Factor")]
        public float BlobScaleFactor { get => this.blobScaleFactor; set => this.blobScaleFactor = value; }
        float blobScaleFactor = 0.5f;

        [CatecoryAttribute("Extract"), NameAttribute("Blobing Scale Factor")]
        public float AllowRotateLimitDeg { get => this.allowRotateLimitDeg; set => this.allowRotateLimitDeg = value; }
        float allowRotateLimitDeg = 2f;

        protected override void Initialize()
        {
            fileName = String.Format(@"{0}\{1}.xml", PathSettings.Instance().Config, "Extract");
        }

        public override void Load(XmlElement xmlElement)
        {
            this.maxCount = XmlHelper.GetValue(xmlElement, "MaxCount", this.maxCount);
            this.minPatternLength = XmlHelper.GetValue(xmlElement, "MinPatternLength", this.minPatternLength);
            this.maxPatternLength = XmlHelper.GetValue(xmlElement, "MaxPatternLength", this.maxPatternLength);
            this.useMaxPatternLength = XmlHelper.GetValue(xmlElement, "UseMaxPatternLength", this.useMaxPatternLength);
            this.maxMarginLength = XmlHelper.GetValue(xmlElement, "MaxMarginLength", this.maxMarginLength);
            this.blobScaleFactor = XmlHelper.GetValue(xmlElement, "BlobScaleFactor", this.blobScaleFactor);
            this.allowRotateLimitDeg = XmlHelper.GetValue(xmlElement, "AllowRotateLimitDeg", this.allowRotateLimitDeg);
        }

        public override void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "MaxCount", this.maxCount);
            XmlHelper.SetValue(xmlElement, "MinPatternLength", this.minPatternLength);
            XmlHelper.SetValue(xmlElement, "MaxPatternLength", this.maxPatternLength);
            XmlHelper.SetValue(xmlElement, "UseMaxPatternLength", this.useMaxPatternLength);
            XmlHelper.SetValue(xmlElement, "MaxMarginLength", this.maxMarginLength);
            XmlHelper.SetValue(xmlElement, "BlobScaleFactor", this.blobScaleFactor);
            XmlHelper.SetValue(xmlElement, "AllowRotateLimitDeg", this.allowRotateLimitDeg);
        }
    }
}
