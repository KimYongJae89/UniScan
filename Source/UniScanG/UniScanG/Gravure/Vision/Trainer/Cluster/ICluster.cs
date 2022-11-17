using DynMvp;
using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.SheetFinder;

namespace UniScanG.Gravure.Vision.Trainer.Cluster
{
    interface IBarCluster
    {
        TrainerParam TrainerParam { get; }

        List<RegionInfoG> FindRegionInfoList(AlgoImage trainImage, AlgoImage patternImage, List<SheetPatternGroup> majorSheetPatternGroupList, DebugContext debugContext);
    }

    struct BuildReionInfoParam
    {
        public Rectangle RegionRect { get; set; }
        public Rectangle InflateSize { get; set; }
        public Size BlobSize { get; set; }
        public AlgoImage RegionTrainImage { get; set; }
        public AlgoImage RegionPatternImage { get; set; }
        public List<BlobRect> BlobRectList { get; set; }

        public BuildReionInfoParam(Rectangle regionRect, Rectangle inflateSize, Size blobSize, AlgoImage regionTrainImage, AlgoImage regionPatternImage, List<BlobRect> nearBlobRectList)
        {
            this.RegionRect = regionRect;
            this.InflateSize = inflateSize;
            this.BlobSize = blobSize;
            this.RegionTrainImage = regionTrainImage;
            this.RegionPatternImage = regionPatternImage;
            this.BlobRectList = nearBlobRectList;
        }
    }

    internal abstract class BarCluster : IBarCluster
    {
        public TrainerParam TrainerParam => this.trainerParam;
        protected TrainerParam trainerParam;

        private BackgroundWorker backgroundWorker;

        public BarCluster(TrainerParam trainerParam, BackgroundWorker backgroundWorker)
        {
            this.trainerParam = trainerParam;
            this.backgroundWorker = backgroundWorker;
        }

        protected void ThrowIfCancellationPending()
        {
            if (this.backgroundWorker != null && this.backgroundWorker.CancellationPending)
                throw new OperationCanceledException();
        }

        public static BarCluster GetBarCluster(ETrainerVersion trainerVersion, TrainerParam trainerParam, BackgroundWorker backgroundWorker)
        {
            switch (trainerVersion)
            {
                default:
                case ETrainerVersion.V1:
                    return new V1.BarClusterV1(trainerParam, backgroundWorker);
                case ETrainerVersion.V2:
                    return new V2.BarClusterV2(trainerParam, backgroundWorker);
            }
        }

        protected abstract Rectangle[,] GetSubRegion(BuildReionInfoParam buildReionInfoParam, DebugContext debugContext);

        public List<RegionInfoG> FindRegionInfoList(AlgoImage trainImage, AlgoImage patternImage, List<SheetPatternGroup> majorSheetPatternGroupList, DebugContext debugContext)
        {
            Rectangle fullRect = new Rectangle(Point.Empty, patternImage.Size);
            float margin = 0.005f; // 좌/우 0.5% 마진 -> 17824[px] * 0.005 -> 89[px] -> 1.246[mm]
            Size invalidSize = Size.Round(new SizeF(trainImage.Width * margin, 0));
            Rectangle validRectangle = Rectangle.Inflate(new Rectangle(Point.Empty, trainImage.Size), -invalidSize.Width, -invalidSize.Height);

            SheetFinderBaseParam sheetFinderBaseParam = AlgorithmPool.Instance().GetAlgorithm(SheetFinderBase.TypeName).Param as SheetFinderBaseParam;
            //가정: 인접한 패턴간의 거리(d_w,d_h)는 현재 패턴의 크기(w,h)의 (N_w,N_h)배보다 가깝다.
            // d_w < N*w, d_h < M*h
            float Nw = 3.0f, Nh = 3.0f;

            List<RegionInfoG> regionInfoList = new List<RegionInfoG>();
            //Point basePos = sheetFinderBaseParam.BaseXSearchDir == BaseXSearchDir.Left2Right ? Point.Empty : new Point(patternImage.Size);
            Point basePos = Point.Empty;

            List<BlobRect> blobRectList = new List<BlobRect>();
            majorSheetPatternGroupList.ForEach(f => blobRectList.AddRange(f.PatternList));
            blobRectList = blobRectList.OrderBy(f => MathHelper.GetLength(basePos, f.BoundingRect.Location)).ToList();

            List<Task> taskList = new List<Task>();
            while (blobRectList.Count > 0)
            {
                List<BlobRect> nearBlobRectList = new List<BlobRect>(); // 인접하여 그룹으로 묶인 블랍 목록
                List<BlobRect> searchBlobRectList = new List<BlobRect>();   // 탐색을 할블랍 목록

                searchBlobRectList.Add(blobRectList[0]);
                while (searchBlobRectList.Count > 0)
                {
                    BlobRect baseBlobRect = searchBlobRectList[0];
                    searchBlobRectList.RemoveAt(0);
                    nearBlobRectList.Add(baseBlobRect);
                    blobRectList.Remove(baseBlobRect);

                    RectangleF boundingRect = baseBlobRect.BoundingRect;
                    //RectangleF findRectF = RectangleF.Inflate(boundingRect, boundingRect.Width * (Nw + 1)/2, boundingRect.Height * (Nh + 1)/2);
                    //RectangleF findRectF = RectangleF.Inflate(boundingRect, boundingRect.Width * (Nw + 1), boundingRect.Height * (Nh + 1));
                    float inflateX = Math.Min(boundingRect.Width, boundingRect.Height) * (Nw + 1) / 2;
                    float inflateY = Math.Max(boundingRect.Width, boundingRect.Height) * (Nh + 1) / 2;
                    RectangleF findRectF = RectangleF.Inflate(boundingRect, inflateX, inflateY);
                    List<BlobRect> foundBlobRectList = blobRectList.FindAll(f => findRectF.IntersectsWith(f.BoundingRect));

                    // 가로/세로 떨어진 거리 구함
                    List<float> wList = new List<float>();
                    List<float> hList = new List<float>();
                    foundBlobRectList.ForEach(f =>
                    {
                        PointF posDiff = new PointF(
                            Math.Max(0, Math.Max(boundingRect.Left - f.BoundingRect.Right, f.BoundingRect.Left - boundingRect.Right)),
                            Math.Max(0, Math.Max(boundingRect.Top - f.BoundingRect.Bottom, f.BoundingRect.Top - boundingRect.Bottom)));

                        if (posDiff.X > 0)
                            wList.Add(posDiff.X);
                        if (posDiff.Y > 0)
                            hList.Add(posDiff.Y);
                    });

                    if (wList.Count > 0)
                        Nw = (Nw + (wList.Min() / boundingRect.Width)) / 2.0f;
                    if (hList.Count > 0)
                        Nh = (Nh + (hList.Min() / boundingRect.Height)) / 2.0f;
                    foundBlobRectList.RemoveAll(f => nearBlobRectList.Contains(f) || searchBlobRectList.Contains(f));

                    searchBlobRectList.AddRange(foundBlobRectList);
                }

                nearBlobRectList.RemoveAll(f => RectangleF.Intersect(validRectangle, f.BoundingRect) != f.BoundingRect);

                if (nearBlobRectList.Count >= 5)
                {
                    ThrowIfCancellationPending();

                    Size averageBlobSize;
                    List<RectangleF> rectangleList = nearBlobRectList.Select(f => f.BoundingRect).ToList();
                    Tuple<Rectangle, Rectangle>[] regionRectList = GetRegionRectList(rectangleList, fullRect, out averageBlobSize);

#if DEBUG
                    //Array.ForEach(regionRectList, f =>
                    //{
                    //    int index = Array.IndexOf(regionRectList, f);
                    //    using (AlgoImage subAlgoImage = trainImage.GetSubImage(f.Item1))
                    //        subAlgoImage.Save($@"C:\temp\{index}.bmp");
                    //    using (AlgoImage subAlgoImage = patternImage.GetSubImage(f.Item1))
                    //        subAlgoImage.Save($@"C:\temp\{index}P.bmp");
                    //});
#endif

                    Task task = Task.Run(() =>
                    {
                        Array.ForEach(regionRectList, f =>
                         {
                             Rectangle inflate = f.Item2;
                             Rectangle regionRect = Rectangle.FromLTRB(
                                 f.Item1.Left - inflate.Left,
                                 f.Item1.Top - inflate.Top,
                                 f.Item1.Right + inflate.Right,
                                 f.Item1.Bottom + inflate.Bottom);
                             regionRect.Intersect(fullRect);
                             //regionRect.Width = regionRect.Width / 4 * 4;

                             AlgoImage regionTrainImage = null, regionPatternImage = null;
                             try
                             {
                                 if (regionRect.Width > 0 && regionRect.Height > 0)
                                 {
                                     regionTrainImage = trainImage.GetSubImage(regionRect);
                                     regionPatternImage = patternImage.GetSubImage(regionRect);
                                     string debugSubPath = string.Format("RegionInfoG_{0}", regionInfoList.Count);

                                     DebugContext newDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, debugSubPath));
                                     BuildReionInfoParam buildReionInfoParam = new BuildReionInfoParam(regionRect, inflate, averageBlobSize, regionTrainImage, regionPatternImage, nearBlobRectList);
                                     RegionInfoG regionInfo = GetRegionInfo(buildReionInfoParam, newDebugContext);
                                     lock (regionInfoList)
                                         regionInfoList.Add(regionInfo);
                                 }
                             }
                             catch (Exception ex)
                             {
                                 LogHelper.Error(LoggerType.Error, $"Exception in BarCluster::FindRegionInfoList - {ex.GetType().Name}: {ex.Message}");
                             }
                             finally
                             {
                                 regionPatternImage?.Dispose();
                                 regionTrainImage?.Dispose();
                             }
                         });
                    });
                    taskList.Add(task);
                }
            }
            taskList.ForEach(f => f.Wait());
            regionInfoList.RemoveAll(f => f == null || f.InspectElementList.Count == 0);

            // 좌측 상단부터 정렬
            List<RegionInfoG> sortedRegionInfoG = new List<RegionInfoG>();
            regionInfoList = regionInfoList.OrderBy(f => MathHelper.GetLength(Point.Empty, f.Region.Location)).ToList();
            while (regionInfoList.Count > 0)
            {
                RegionInfoG regionInfoG = regionInfoList.First();
                List<RegionInfoG> list = regionInfoList.FindAll(f => MathHelper.IsInRange((f.Region.Top + f.Region.Bottom) / 2, regionInfoG.Region.Top, regionInfoG.Region.Bottom)).OrderBy(f => f.Region.X).ToList();
                sortedRegionInfoG.AddRange(list);
                regionInfoList.RemoveAll(f => list.Contains(f));
            }

            return sortedRegionInfoG;
        }

        private Tuple<Rectangle, Rectangle>[] GetRegionRectList(List<RectangleF> rectangleList, Rectangle fullRect, out Size averageBlobSize)
        {
            Rectangle unionRect = Rectangle.Round(rectangleList.Aggregate((f, g) => RectangleF.Union(f, g)));

            // inflateSize
            averageBlobSize = Size.Round(new SizeF(rectangleList.Average(f => f.Width), rectangleList.Average(f => f.Height)));
            Size inflateSize;
            if (false)
            {
                // 패턴 크기만큼 늘림
                float inflateMul = trainerParam.AlignLocalSearch;
                float lengthW = averageBlobSize.Width * (Math.Max(0, (inflateMul - 1)) / 2);
                float lengthH = averageBlobSize.Height * (Math.Max(0, (inflateMul - 1)) / 2);
                inflateSize = Size.Round(new SizeF(Math.Min(lengthW, lengthH), Math.Max(lengthW, lengthH)));
            }
            else
            {
                // 가로방향 40%, 세로방향 10% 중 작은 값 만큼 늘림.
                //int w = (int)Math.Max(0, averageBlobSize.Width * 0.50f);
                //int h = (int)Math.Max(0, averageBlobSize.Height * 0.10f);
                //int min = Math.Min(w, h);

                float ratio = unionRect.Width * 1f / unionRect.Height;
                long rectSize = unionRect.Width * unionRect.Height; // 바 전체 면적
                long blobSize = rectangleList.Sum(f => (long)(f.Width * f.Height)); // 바 중 전극의 면적
                long marginSize = rectSize - blobSize; // 바 중 마진의 면적
                float meanMarginSize = (float)marginSize / rectangleList.Count();

                // 가로:세로 비율이 일정할 때, 마진의 면적으로 마진의 크기를 구함.
                float h = (float)Math.Sqrt(meanMarginSize / ratio);
                float w = h * ratio;

                // 장축 50%, 단축 50% 중 작은 값 만큼 확장.
                int @long = (int)(Math.Max(w, h) * 0.50f);
                int @short = (int)(Math.Min(w, h) * 0.50f);

                int min = Math.Min(@long, @short);

                inflateSize = new Size(min, min);
                //inflateSize = Size.Empty;
            }

            if (!this.trainerParam.SplitLargeBar)
            {
                return new Tuple<Rectangle, Rectangle>[]
                {
                    new Tuple<Rectangle, Rectangle>(unionRect, Rectangle.FromLTRB(inflateSize.Width, inflateSize.Height, inflateSize.Width, inflateSize.Height))
                };
            }

            // 크기가 너무 크면 쪼갠다!
            {
                List<int> splitYList = new List<int>();
                if (unionRect.Height > fullRect.Height / 2)
                //if (averageBlobSize.Height > unionRect.Height / 4)
                {
                    int splitY = (unionRect.Top + unionRect.Bottom) / 2;
                    bool done = false;
                    int prevOffsetAbs = int.MaxValue;
                    for (int i = 0; i < 5; i++)
                    {
                        List<RectangleF> crossingList = rectangleList.FindAll(f => splitY > f.Top && splitY < f.Bottom);
                        if (crossingList.Count == 0)
                        {
                            done = true;
                            break;
                        }
                        int dYT = (int)crossingList.Max(f => splitY - f.Top);
                        int dYB = (int)crossingList.Max(f => f.Bottom - splitY);

                        int offset = (dYT < dYB) ? -(dYT + 1) : (dYB + 1);
                        splitY += offset;

                        int offsetAbs = Math.Abs(offset);
                        if (prevOffsetAbs <= offsetAbs)
                            break;
                        prevOffsetAbs = offsetAbs;
                    }

                    if (done)
                    {
                        float[] upperBottoms = rectangleList.FindAll(f => f.Bottom < splitY).Select(f => f.Bottom).ToArray();
                        float[] lowerTops = rectangleList.FindAll(f => f.Top > splitY).Select(f => f.Top).ToArray();

                        if (upperBottoms.Length == 0 && lowerTops.Length == 0)
                        {
                            int dyT = (int)(upperBottoms.Max() - splitY);
                            int dyB = (int)(lowerTops.Min() - splitY);

                            int offset = (dyT + dyB) / 2;
                            splitY += offset;
                        }
                    }
                }
                splitYList.Insert(0, unionRect.Top);
                splitYList.Add(unionRect.Bottom);

                List<int> splitXList = new List<int>();
                if (unionRect.Width > fullRect.Width / 2)
                {
                    // 잠시 보류
                    //int splitX = (unionRect.Left + unionRect.Right) / 2;
                    //List<RectangleF> corssRectList = rectangleList.FindAll(f => splitX > f.Left && splitX < f.Right);
                    //if (corssRectList.Count > 0)
                    //{
                    //    int dYL = (int)corssRectList.Average(f => splitX - f.Left);
                    //    int dYR = (int)corssRectList.Average(f => f.Right - splitX);
                    //    if (dYL < dYR)
                    //        splitX -= dYL;
                    //    else
                    //        splitX += dYR;
                    //}

                    //for (int iterate = 0; iterate < 10; iterate++)
                    //{
                    //    int leftR = (int)rectangleList.FindAll(f => f.Left < splitX).Max(f => f.Right);
                    //    int rightL= (int)rectangleList.FindAll(f => f.Right > splitX).Min(f => f.Left);

                    //    int dyL = leftR - splitX;
                    //    int dyR = rightL - splitX;

                    //    int offset = dyL + dyR;
                    //    if (offset == 0)
                    //    {
                    //        splitYList.Add(splitX);
                    //        break;
                    //    }
                    //    splitX += (offset / 2);
                    //}
                }
                splitXList.Insert(0, unionRect.Left);
                splitXList.Add(unionRect.Right);

                List<Tuple<Rectangle, Rectangle>> regionRectList = new List<Tuple<Rectangle, Rectangle>>();
                //if (splitX.Count > 2 || splitY.Count > 2)
                {
                    splitYList.Aggregate((y0, y1) =>
                    {
                        splitXList.Aggregate((x0, x1) =>
                        {
                            int inflateL = (x0 == unionRect.Left ? inflateSize.Width : 0);
                            int inflateT = (y0 == unionRect.Top ? inflateSize.Height : 0);
                            int inflateR = (x1 == unionRect.Right ? inflateSize.Width : 0);
                            int inflateB = (y1 == unionRect.Bottom ? inflateSize.Height : 0);

                            regionRectList.Add(new Tuple<Rectangle, Rectangle>(Rectangle.FromLTRB(x0, y0, x1, y1), Rectangle.FromLTRB(inflateL, inflateT, inflateR, inflateB)));
                            return x1;
                        });
                        return y1;
                    });
                }
                return regionRectList.ToArray();
            }
        }

        private RegionInfoG GetRegionInfo(BuildReionInfoParam buildReionInfoParam, DebugContext debugContext)
        {
            Rectangle patternImageRect = new Rectangle(Point.Empty, buildReionInfoParam.RegionPatternImage.Size);
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(buildReionInfoParam.RegionPatternImage);
            buildReionInfoParam.RegionPatternImage.Save("0 regionPatternImage.bmp", debugContext);

            // 썸네일 이미지 제작
            ImageD thumbnailImageD = null;
            float thumbnailScaleFactor = 512.0f / Math.Max(patternImageRect.Width, patternImageRect.Height);
            Size thumbnailSize = DrawingHelper.Mul(patternImageRect.Size, thumbnailScaleFactor);
            if (thumbnailSize.Width == 0 || thumbnailSize.Height == 0)
                thumbnailSize = patternImageRect.Size;

            using (AlgoImage scaledImage = ImageBuilder.Build(TrainerBase.TypeName, ImageType.Grey, thumbnailSize.Width, thumbnailSize.Height))
            {
                ip.Resize(buildReionInfoParam.RegionPatternImage, scaledImage);
                thumbnailImageD = scaledImage.ToImageD();
            }

            // 전극 밝기 평균, 성형 밝기 평균 획득
            float poleAvg = 0, dielectricAvg = 255;
            GetAvgValue(buildReionInfoParam.RegionTrainImage, ref poleAvg, ref dielectricAvg);

            // 전극 주기 파악
            Rectangle[,] rectangles = GetSubRegion(buildReionInfoParam, debugContext);
            if (rectangles == null || (rectangles.GetLength(0) < 3 && rectangles.GetLength(1) < 3))
            {
                return null;
            }

            // 빈 공간 없도록 전극 Rect를 채움.
            Rectangle[,] adjustSubRegion = AdjustRect(rectangles, debugContext);
            //adjustSubRegion = rectangles;

            // 템플릿매칭 영역 및 마스터 이미지 획득
            AlignInfo[] alignInfos = GetInBarAlignElement(buildReionInfoParam, debugContext);

            RegionInfoG regionInfo = new RegionInfoG(buildReionInfoParam.RegionRect, buildReionInfoParam.InflateSize, thumbnailImageD, alignInfos, rectangles, adjustSubRegion, poleAvg, dielectricAvg);
            regionInfo.SetSkipRegion(buildReionInfoParam.RegionPatternImage);

            // 최상단 주기영역의 블랍
            List<BlobRect> orderedList = buildReionInfoParam.BlobRectList.OrderBy(f => f.BoundingRect.Top).ToList();
            int width = rectangles.GetLength(1);
            List<Point> pointList = new List<Point>();
            for (int i = 0; i < width; i++)
            {
                RectangleF rectangle = DrawingHelper.Offset(rectangles[0, i], buildReionInfoParam.RegionRect.Location);
                BlobRect topBlobRect = buildReionInfoParam.BlobRectList.Find(f => f.BoundingRect.IntersectsWith(rectangle));
                if (topBlobRect != null)
                    pointList.Add(new Point(i, (int)topBlobRect.BoundingRect.Top));
            }
            double meanX = pointList.Average(f => f.X);
            double meanY = pointList.Average(f => f.Y);
            double s = pointList.Select(f => (f.X - meanX) * (f.Y - meanY)).Sum() / pointList.Select(f => Math.Pow(f.X - meanX, 2)).Sum();
            regionInfo.Slope = (float)s;

            ImageD regionTrainImageD = buildReionInfoParam.RegionTrainImage.ToImageD();
            ImageD regionPatternImageD = buildReionInfoParam.RegionPatternImage.ToImageD();

            regionInfo.IgnoreInnerChip = trainerParam.ModelParam.IgnoreInnerChip;
            regionInfo.GroupDirection = trainerParam.GroupDirection;
            regionInfo.OddEvenPair = trainerParam.ModelParam.IsCrisscross;
            regionInfo.BuildInspRegion(regionTrainImageD, regionPatternImageD);

            return regionInfo;
        }

        private void GetAvgValue(AlgoImage regionTrainImage, ref float poleAvg, ref float dielectricAvg)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(regionTrainImage);
            using (AlgoImage regionAvgImage = regionTrainImage.Clone())
            {
                ip.Binarize(regionAvgImage, true);

                poleAvg = ip.GetGreyAverage(regionTrainImage, regionAvgImage);

                ip.Not(regionAvgImage);
                dielectricAvg = ip.GetGreyAverage(regionTrainImage, regionAvgImage);
            }
        }


        private Rectangle[,] AdjustRect(Rectangle[,] rectangles, DebugContext debugContext)
        {
            //return rectangles;
            // 평균 마진 찾기
            StringBuilder debugStringBuilder = new StringBuilder();
            float averageMarginSizeW = 0, averageMarginSizeH = 0;

            debugStringBuilder.AppendLine("Pattern Area Height");
            int cntH = rectangles.GetLength(0) - 1;
            for (int y = 0; y < cntH; y++)
            {
                int margin = rectangles[y + 1, 0].Top - rectangles[y, 0].Bottom;
                debugStringBuilder.Append(string.Format("{0},", margin));
                averageMarginSizeH += margin;
            }
            debugStringBuilder.AppendLine();
            debugStringBuilder.AppendLine("Pattern Area Width");
            int cntW = rectangles.GetLength(1) - 1;
            for (int x = 0; x < cntW; x++)
            {
                int margin = rectangles[0, x].Left - rectangles[0, x].Right;
                debugStringBuilder.Append(string.Format("{0},", margin));
                averageMarginSizeW += margin;
            }

            if (debugContext.SaveDebugImage)
                File.WriteAllText(Path.Combine(debugContext.FullPath, "debug.txt"), debugStringBuilder.ToString());

            averageMarginSizeH /= (Math.Max(1, cntH) * 2);
            averageMarginSizeW /= (Math.Max(1, cntW) * 2);

            Point centerPos = new Point(rectangles.GetLength(1) / 2, rectangles.GetLength(0) / 2);
            //Thread th = new Thread(() => AdjustRect(debugImage, rectangles, centerPos, hillLists, inflateSize, -1), 1024 * 1024 * 10);
            //th.Start();
            //th.Join();

            return AdjustRect2(rectangles, centerPos);
        }

        private Rectangle[,] AdjustRect2(Rectangle[,] rectangles, Point posIdx)
        {
            // 마진 맵
            Size size = new Size(rectangles.GetLength(1), rectangles.GetLength(0));
            Rectangle[,] margin = new Rectangle[rectangles.GetLength(0), rectangles.GetLength(1)];
            for (int x = 0; x < size.Width; x++)
            {
                for (int y = 0; y < size.Height; y++)
                {
                    int l = x > 0 ? rectangles[y, x].Left - rectangles[y, x - 1].Right : -1;
                    int t = y > 0 ? rectangles[y, x].Top - rectangles[y - 1, x].Bottom : -1;
                    int r = x < size.Width - 1 ? rectangles[y, x + 1].Left - rectangles[y, x].Right : -1;
                    int b = y < size.Height - 1 ? rectangles[y + 1, x].Top - rectangles[y, x].Bottom : -1;
                    Rectangle rectangle = Rectangle.FromLTRB(l, t, r, b);
                    margin[y, x] = rectangle;
                }
            }

            Rectangle[,] adjustRectangles = new Rectangle[rectangles.GetLength(0), rectangles.GetLength(1)];

            if (true)
            {
                AverageCalculator[] averageCalculators = new AverageCalculator[2] { new AverageCalculator(), new AverageCalculator() };
                foreach (Rectangle rect in margin)
                {
                    if (rect.Left >= 0)
                        averageCalculators[0].Add(rect.Left);
                    if (rect.Right >= 0)
                        averageCalculators[0].Add(rect.Right);

                    if (rect.Top >= 0)
                        averageCalculators[1].Add(rect.Top);
                    if (rect.Bottom >= 0)
                        averageCalculators[1].Add(rect.Bottom);
                }

                int marginW2 = (int)Math.Round(averageCalculators[0].MaxValue / 2);
                int marginH2 = (int)Math.Round(averageCalculators[1].MaxValue / 2);
                for (int x = 0; x < size.Width; x++)
                {
                    for (int y = 0; y < size.Height; y++)
                    {
                        Rectangle rectangle = rectangles[y, x];
                        rectangle.Inflate(marginW2, marginH2);

                        //if(x>0)
                        //{
                        //    Rectangle rectangleX = adjustRectangles[y, x-1];
                        //    rectangle.X = rectangleX.Right;
                        //}
                        //if(y>0)
                        //{
                        //    Rectangle rectangleY = adjustRectangles[y-1, x];
                        //    rectangle.Y = rectangleY.Bottom;
                        //}
                        adjustRectangles[y, x] = rectangle;
                    }
                }
            }
            else
            {
                Array.Copy(rectangles, adjustRectangles, Math.Min(rectangles.Length, adjustRectangles.Length));
                for (int x = 0; x < size.Width; x++)
                {
                    Point averageMarginValue = Point.Empty;
                    Point averageMarginCount = Point.Empty;
                    for (int y = 0; y < size.Height; y++)
                    {
                        if (margin[y, x].Left >= 0)
                        {
                            averageMarginValue.X += margin[y, x].Left;
                            averageMarginCount.X++;
                        }
                        if (margin[y, x].Right >= 0)
                        {
                            averageMarginValue.Y += margin[y, x].Right;
                            averageMarginCount.Y++;
                        }
                    }

                    averageMarginValue.X /= (Math.Max(1, averageMarginCount.X) * 2);
                    averageMarginValue.Y /= (Math.Max(1, averageMarginCount.Y) * 2);
                    for (int y = 0; y < size.Height; y++)
                    {
                        int l;
                        if (x == 0)
                            l = adjustRectangles[y, x].Left - (averageMarginValue.X == 0 ? averageMarginValue.Y : averageMarginValue.X);
                        else
                            l = adjustRectangles[y, x - 1].Right;

                        int t = adjustRectangles[y, x].Top;
                        int r = adjustRectangles[y, x].Right + (averageMarginValue.Y == 0 ? averageMarginValue.X : averageMarginValue.Y);
                        int b = adjustRectangles[y, x].Bottom;
                        adjustRectangles[y, x] = Rectangle.FromLTRB(l, t, r, b);
                    }
                }

                for (int y = 0; y < size.Height; y++)
                {
                    Point averageMarginValue = Point.Empty;
                    Point averageMarginCount = Point.Empty;
                    for (int x = 0; x < size.Width; x++)
                    {
                        if (margin[y, x].Top >= 0)
                        {
                            averageMarginValue.X += margin[y, x].Top;
                            averageMarginCount.X++;
                        }
                        if (margin[y, x].Bottom >= 0)
                        {
                            averageMarginValue.Y += margin[y, x].Bottom;
                            averageMarginCount.Y++;
                        }
                    }

                    averageMarginValue.X /= (Math.Max(1, averageMarginCount.X) * 2);
                    averageMarginValue.Y /= (Math.Max(1, averageMarginCount.Y) * 2);
                    for (int x = 0; x < size.Width; x++)
                    {
                        int l = adjustRectangles[y, x].Left;
                        int t;
                        if (y == 0)
                            t = adjustRectangles[y, x].Top - (averageMarginValue.X == 0 ? averageMarginValue.Y : averageMarginValue.X);
                        else
                            t = adjustRectangles[y - 1, x].Bottom;

                        int r = adjustRectangles[y, x].Right;
                        int b = adjustRectangles[y, x].Bottom + (averageMarginValue.Y == 0 ? averageMarginValue.X : averageMarginValue.Y);
                        adjustRectangles[y, x] = Rectangle.FromLTRB(l, t, r, b);
                    }
                }

                //for (int i = 1; i < adjustRectangles.GetLength(0); i++)
                //{
                //    int a = adjustRectangles[i - 1, 0].Bottom;
                //    int b = adjustRectangles[i, 0].Top;
                //    Debug.Assert(adjustRectangles[i - 1, 0].Bottom == adjustRectangles[i, 0].Top);
                //}

                //for (int i = 1; i < adjustRectangles.GetLength(1); i++)
                //{
                //    int a = adjustRectangles[0, i - 1].Right;
                //    int b = adjustRectangles[0, i].Left;
                //    Debug.Assert(adjustRectangles[0, i - 1].Right == adjustRectangles[0, i].Left);
                //}
            }

            return adjustRectangles;
        }

        private AlignInfo[] GetInBarAlignElement(BuildReionInfoParam buildReionInfoParam, DebugContext debugContext)
        {
            Rectangle imageRect = new Rectangle(Point.Empty, buildReionInfoParam.RegionTrainImage.Size);

            int alignerCount = 2;
            Size blobSize = buildReionInfoParam.BlobSize;
            int maxSize = Math.Min(blobSize.Width, blobSize.Height) * 3;
            blobSize.Width = Math.Min(blobSize.Width, maxSize);
            blobSize.Height = Math.Min(blobSize.Height, maxSize);
            //if (blobSize.Height > maxSize && blobSize.Height > buildReionInfoParam.RegionRect.Height / 3)
            //    blobSize.Height = maxSize;

            Size refSize = new Size(Math.Min(blobSize.Width, blobSize.Height), Math.Max(blobSize.Width, blobSize.Height));
            AlignInfo[] alignInfos = new AlignInfo[alignerCount];
            for (int i = 0; i < alignerCount; i++)
            {
                char[] positions = new char[] { 'L', 'T' };
                Size pmSearchSize = DrawingHelper.Mul(refSize, trainerParam.AlignLocalSearch);
                Rectangle pmSearchRect = new Rectangle(Point.Empty, pmSearchSize);

                Point pmSearchRectOffset = new Point(0, 0);
                SheetFinderBaseParam sheetFinderParam = AlgorithmSetting.Instance().SheetFinderBaseParam;
                if (sheetFinderParam.GetBaseXSearchDir() == BaseXSearchDir.Right2Left)
                {
                    pmSearchRectOffset.X = imageRect.Width - pmSearchRect.Width;
                    positions[0] = 'R';
                }

                if (alignerCount == 2 && i == 1)
                {
                    pmSearchRectOffset.Y = imageRect.Height - pmSearchRect.Height;
                    positions[1] = 'B';
                }

                pmSearchRect.Offset(pmSearchRectOffset);
                pmSearchRect.Intersect(imageRect);
                using (AlgoImage pmSearchImage = buildReionInfoParam.RegionTrainImage.GetSubImage(pmSearchRect))
                    pmSearchImage.Save(@"pmSearchImage.bmp", debugContext);

                //int inflate = (int)Math.Floor(((trainerParam.AlignLocalMaster - 1) / trainerParam.AlignLocalMaster) * s / 2);
                //Rectangle pmMasterRect = Rectangle.Inflate(pmSearchRect, -inflate, -(int)(inflate * 1.2));
                Size pmMasterSize = DrawingHelper.Mul(pmSearchRect.Size, 1 / trainerParam.AlignLocalMaster);
                Size diffSize = Size.Subtract(pmMasterSize, pmSearchRect.Size);
                Rectangle pmMasterRect = Rectangle.Inflate(pmSearchRect, diffSize.Width / 2, diffSize.Height / 2);
                pmMasterRect.Intersect(pmSearchRect);

                ImageD pmMasterImageD = null;
                if (pmMasterRect.Width > 0 && pmMasterRect.Height > 0)
                {
                    using (AlgoImage pmMasterImage = buildReionInfoParam.RegionTrainImage.GetSubImage(pmMasterRect))
                    {
                        pmMasterImage.Save("0 pmMasterImage.bmp", debugContext);
                        pmMasterImageD = pmMasterImage.ToImageD();
                    }
                }
                alignInfos[i] = new AlignInfo(new string(positions), pmMasterImageD, pmMasterRect, pmSearchRect);
            }

            return alignInfos;
        }
    }
}
