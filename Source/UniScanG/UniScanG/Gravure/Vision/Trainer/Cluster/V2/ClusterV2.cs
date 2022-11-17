using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Vision;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Vision.Trainer.Cluster.V1;

namespace UniScanG.Gravure.Vision.Trainer.Cluster.V2
{
    internal class BarClusterV2 : BarCluster
    {

        internal BarClusterV2(TrainerParam trainerParam, BackgroundWorker backgroundWorker) : base(trainerParam, backgroundWorker) { }

        protected override Rectangle[,] GetSubRegion(BuildReionInfoParam buildReionInfoParam, DebugContext debugContext)
        {
            List<BlobRect> blobRectList = buildReionInfoParam.BlobRectList;
            Size offset = new Size(DrawingHelper.Mul(buildReionInfoParam.RegionRect.Location, -1));
            List<RectangleF> rectangleList = blobRectList.Select(f => DrawingHelper.Offset(f.BoundingRect, offset)).ToList();

            // (0,0) 기준 거리순 정렬
            rectangleList.OrderBy(f => MathHelper.GetLength(Point.Empty, f.Location));

            // 패턴 주기 파악
            // 가로 W개, 세로 H개. 전극 개수 N < W*H
            RangeCollection rangeCollectionX = new RangeCollection();
            RangeCollection rangeCollectionY = new RangeCollection();
            rectangleList.ForEach(f =>
            {
                rangeCollectionX.Add(new Range(f.Left, f.Right));
                rangeCollectionY.Add(new Range(f.Top, f.Bottom));
            });

            Rectangle[,] rectangles = GetRectangles(rangeCollectionX, rangeCollectionY);
            return rectangles;
        }

        private Rectangle[,] GetRectangles(RangeCollection rangeCollectionX, RangeCollection rangeCollectionY)
        {
            Range[] xx = rangeCollectionX.GetRanges();

            if(rangeCollectionY.IsCross())
                rangeCollectionY.Split();
            Range[] yy = rangeCollectionY.GetRanges();

            Rectangle[,] rectangles = new Rectangle[yy.Length, xx.Length];
            for (int x = 0; x < xx.Length; x++)
            {
                for (int y = 0; y < yy.Length; y++)
                    rectangles[y, x] = Rectangle.Round(RectangleF.FromLTRB(xx[x].Src, yy[y].Src, xx[x].Dst, yy[y].Dst));
            }

            return rectangles;
        }
    }
}
