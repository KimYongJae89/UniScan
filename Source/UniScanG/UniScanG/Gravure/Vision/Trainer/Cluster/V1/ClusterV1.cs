using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp;
using DynMvp.Base;
using DynMvp.Vision;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.SheetFinder;

namespace UniScanG.Gravure.Vision.Trainer.Cluster.V1
{
    internal class BarClusterV1 : BarCluster
    {
        public BarClusterV1(TrainerParam trainerParam, BackgroundWorker backgroundWorker) : base(trainerParam, backgroundWorker) { }

        /// <summary>
        /// 패턴이 속하는 Grid 생성.
        /// </summary>
        /// <param name="regionPatternImage"></param>
        /// <param name="debugContext"></param>
        /// <returns></returns>
        protected override Rectangle[,] GetSubRegion(BuildReionInfoParam buildReionInfoParam, DebugContext debugContext)
        {
            AlgoImage regionPatternImage = buildReionInfoParam.RegionPatternImage;
            //regionPatternImage.Save(@"C:\temp\regionPatternImage.bmp");

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(regionPatternImage);
            Rectangle imageRect = new Rectangle(Point.Empty, regionPatternImage.Size);
            bool isCrisscross = this.trainerParam.ModelParam.IsCrisscross;
            bool wholeProjection = this.trainerParam.ModelParam.WholeRegionProjection;
            bool fixMissingTooth = this.trainerParam.ModelParam.FixMissingTooth;
            
            // 중앙영역 Projection. 패턴 주기 파악
            int ii = wholeProjection ? 1 : 5;
            float[][] projData = new float[2][];
            for (int i = 0; i < 2; i++)
            // i==0 -> 중앙 가로방향 사각형 이미지. 상->하로 투영
            // i==1 -> 중앙 세로방향 사각형 이미지. 좌->우로 투영
            {
                //int projRegionW = regionPatternImage.Width / (i == 0 ? 1 : 5);
                //int projRegionH = regionPatternImage.Height / (i == 0 ? 5 : 1);
                int projRegionW = regionPatternImage.Width / (i == 0 ? 1 : ii);
                int projRegionH = regionPatternImage.Height / (i == 0 ? ii : 1);

                //int projRegionW = regionPatternImage.Width;
                //int projRegionH = regionPatternImage.Height;

                Rectangle projRegion = new Rectangle(Point.Empty, regionPatternImage.Size);
                projRegion.Inflate(-(regionPatternImage.Width - projRegionW) / 2, -(regionPatternImage.Height - projRegionH) / 2);

                if (i == 2)
                    projRegion.X = 0;
                else if (i == 3)
                    projRegion.X = regionPatternImage.Width - projRegion.Width;

                using (AlgoImage projImage = regionPatternImage.Clip(projRegion))
                {
                    //debugContext = new DebugContext(true, debugContext.FullPath);
                    //ip.Average(projImage);
                    //projImage.Save(string.Format("0 projImage_{0}.bmp", i), debugContext);
                    projData[i] = ip.Projection(projImage, i == 1 ? Direction.Vertical : Direction.Horizontal, ProjectionType.Mean);

                    if (debugContext.SaveDebugImage)
                    {
                        StringBuilder sb = new StringBuilder();
                        Array.ForEach(projData[i], f => sb.AppendLine(f.ToString()));
                        File.WriteAllText(Path.Combine(debugContext.FullPath, string.Format("projData{0}.txt", i)), sb.ToString());
                    }
                }
            }
            
            // SubRegion 찾기
            Rectangle[,] subRegions = GetSubRegion(projData[0], projData[1], isCrisscross, fixMissingTooth, debugContext);
            if (debugContext.SaveDebugImage)
            {
                using (AlgoImage projImage = regionPatternImage.Clone())
                {
                    var dd = subRegions.GetEnumerator();
                    while (dd.MoveNext())
                    {
                        Rectangle rr = (Rectangle)dd.Current;
                        ip.DrawRect(projImage, Rectangle.Inflate(rr, -rr.Width / 3, -rr.Height / 3), 128, true);
                    }
                    projImage.Save(@"projImage.bmp", debugContext);
                }
            }

            int h = subRegions.GetLength(0);
            int w = subRegions.GetLength(1);
            // 비틀림 보정
            if ((h > 1 && w > 1))
            {
                Size projRectSize = new Size(regionPatternImage.Width / 5, regionPatternImage.Height);
                List<Point>[] hillLists = new List<Point>[2];
                for (int i = 0; i < 2; i++)
                {
                    Point projRectLoc = new Point((i == 0 ? 0 : regionPatternImage.Width - projRectSize.Width), 0);
                    Rectangle projRegion = new Rectangle(projRectLoc.X, projRectLoc.Y, projRectSize.Width, projRectSize.Height);
                    AlgoImage projImage = regionPatternImage.GetSubImage(projRegion);
                    float[] sizeProj = ip.Projection(projImage, Direction.Vertical, ProjectionType.Mean);
                    projImage.Dispose();

                    if (isCrisscross)
                        hillLists[i] = AlgorithmCommon.FindHill3(sizeProj, trainerParam.KernalSize, trainerParam.MinLineIntensity, trainerParam.DiffrentialThreshold);
                    else
                        hillLists[i] = AlgorithmCommon.FindHill(sizeProj, trainerParam.MinLineIntensity);
                }

                //regionPatternImage.Save(@"d:\temp\tt.bmp");
                if (hillLists[0].Count == hillLists[1].Count && hillLists[0].Count > 0)
                {
                    float offset = hillLists[1][0].X - hillLists[0][0].X;
                    for (int x = 0; x < subRegions.GetLength(1); x++)
                    {
                        int subOffset = (int)Math.Round(offset * x / subRegions.GetLength(1) - offset / 2);
                        for (int y = 0; y < subRegions.GetLength(0); y++)
                        {
                            Rectangle subRegion = subRegions[y, x];
                            subRegion.Offset(0, subOffset);
                            subRegions[y, x] = subRegion;
                        }
                    }
                }
            }

            // 이미지 영역을 벗어나면 잘라냄
            for (int i = 0; i < subRegions.Length; i++)
            {
                Rectangle rect = subRegions[i / w, i % w];
                subRegions[i / w, i % w] = Rectangle.Intersect(rect, imageRect);
            }

            if (subRegions.Length == 0)
            {
                LogHelper.Debug(LoggerType.Inspection, $"BarClusterV1::GetSubRegion - Length is 0");
                return null;
            }

            return subRegions;
        }

        private Rectangle[,] GetSubRegion(float[] projDataX, float[] projDataY, bool isCrisscross, bool fixMissingTooth, DebugContext debugContext)
        {
            List<Point>[] hillLists = new List<Point>[2];
            List<Point> hillListX, hillListY;

            // White Area의 시작점/끝점을 찾음
            DebugContext hillListXDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, "HillListX"));
            //float thresholdX2 = projDataX.Average() / 3 * 2;
            float thresholdX = GetThresholdX(projDataX);
            hillListX = hillLists[0] = AlgorithmCommon.FindHill(projDataX, thresholdX, false, hillListXDebugContext);

            //hillListX = hillLists[0] = AlgorithmCommon.FindHill3(projDataX, 50, 10,50, hillListXDebugContext);
            //hillListX.ForEach(f => System.Diagnostics.Debug.WriteLine(string.Format("hillListX: L,{0},R,{1},W,{2}", f.X, f.Y ,f.Y - f.X)));
            if (fixMissingTooth)
                FixBrokenTooth(hillListX);

            if (hillListX.Count > 0)
            {
                float ddAver = (float)hillListX.Average(f => f.Y - f.X);
                hillListX.RemoveAll(f => Math.Abs(ddAver - (f.Y - f.X)) > ddAver / 2);
            }

            DebugContext hillListYDebugContext = new DebugContext(debugContext.SaveDebugImage, Path.Combine(debugContext.FullPath, "HillListY"));
            if (isCrisscross)
                hillListY = hillLists[1] = AlgorithmCommon.FindHill3(projDataY, trainerParam.KernalSize, trainerParam.MinLineIntensity, trainerParam.DiffrentialThreshold, hillListYDebugContext);
            else
                hillListY = hillLists[1] = AlgorithmCommon.FindHill(projDataY, trainerParam.MinLineIntensity, false, hillListYDebugContext);
            //hillListY.ForEach(f => Debug.WriteLine(string.Format("hillListY: Size {0}", f.Y - f.X)));

            if (hillListX.Count == 0 || hillListY.Count == 0)
                return new Rectangle[0, 0];

            // 패턴 격자 생성
            //AlgoImage debugImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new Size(projDataX.Length, projDataY.Length));
            //debugImage.Dispose();
            Rectangle[,] rectangles = new Rectangle[hillListY.Count, hillListX.Count];
            Size size = new Size((int)Math.Round(hillListX.Average(f => f.Y - f.X)), (int)Math.Round(hillListY.Average(f => f.Y - f.X)));

            for (int x = 0; x < hillLists[0].Count; x++)
            {
                for (int y = 0; y < hillLists[1].Count; y++)
                {
                    Rectangle rectangle = Rectangle.FromLTRB(hillLists[0][x].X, hillLists[1][y].X, hillLists[0][x].Y, hillLists[1][y].Y);
                    rectangles[y, x] = rectangle;
                }
            }
            return rectangles;
        }

        private void FixBrokenTooth(List<Point> hillListX)
        {
            double width = hillListX.Average(f => f.Y - f.X);

            double[] dist = new double[hillListX.Count - 1];
            for (int i = 0; i < hillListX.Count - 1; i++)
                dist[i] = hillListX[i + 1].X - hillListX[i].X;

            if (dist.Length == 0)
                return;

            double mean = dist.Average();
            double var = dist.Average(f => Math.Pow(f - mean, 2));
            double std = Math.Sqrt(var);
            double lim = std * 5;

            for (int i = 1; i < hillListX.Count; i++)
            {
                int src = hillListX[i - 1].X;
                int dst = hillListX[i].X;
                int d = dst - src;
                double err = d - mean;
                if(err > lim)
                {
                    int x = (int)Math.Round(src + mean);
                    int y = (int)Math.Round(x + width);
                    hillListX.Insert(i, new Point(x, y));
                }
            }
        }

        /// <summary>
        /// 투영 데이터로 히스토그램을 그리고 Otsu로 이진화
        /// </summary>
        /// <param name="projDataX"></param>
        /// <returns></returns>
        private float GetThresholdX(float[] projDataX)
        {
            long[] histo = new long[256];
            Array.Clear(histo, 0, 256);

            int skip = (int)(projDataX.Length * 0.01);
            int take = projDataX.Length - (2 * skip);
            projDataX = projDataX.Skip(skip).Take(take).ToArray();

            Array.ForEach(projDataX, f => histo[(int)Math.Round(f)]++);
            //Array.ForEach(histo, f => Console.WriteLine(f));

            return ImageProcessing.Otsu(histo);
        }
    }
}
