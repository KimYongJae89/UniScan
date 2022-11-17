using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Vision.RCI.Trainer;

namespace UniScanG.Gravure.Vision.RCI.Calculator
{
    public class BlockResult
    {
        public int Index { get; private set; }
        public Rectangle TargetRect { get; private set; }
        public Rectangle ModelRect { get; private set; }
        public int Column { get; private set; }
        public int Row { get; private set; }

        public Point Offset { get; private set; }
        public Size Diff => DrawingHelper.Subtract(this.ModelRect.Size, this.TargetRect.Size);

        public float MaxValue { get; set; }
        public float MeanBgValue { get; set; }
        public bool HasReference { get; set; }

        public BlockResult(int index, Rectangle targetRect, Rectangle modelRect, Point rectOffset, int column, int row)
        {
            this.Index = index;
            this.TargetRect = targetRect;
            this.ModelRect = modelRect;
            this.Offset = rectOffset;
            this.Column = column;
            this.Row = row;
        }
    }


    public class HighDiffBlock
    {
        public int Index { get; set; }
        public Rectangle TargetRectangle { get; set; }
        public Rectangle ModelRectangle { get; set; }
        public PointF TmOffsetPoint { get; set; }
        public int MaxValue { get; set; }
        public int Threshold { get; set; }
        public ImageD Model { get; set; }
        public ImageD Target { get; set; }
        public ImageD Weight { get; set; }
        public ImageD Result { get; set; }
        public ImageD Binalize { get; set; }

        public ProjectionData ModelProjection { get; set; }
        public ProjectionData TargetProjection { get; set; }
        public float[] ScoreX { get; set; }
        public float[] ScoreY { get; set; }

        public HighDiffBlock() { }
        public void Dispose()
        {
            Model?.Dispose();
            Target?.Dispose();
            Weight?.Dispose();
            Result?.Dispose();

            TargetProjection = null;
            ScoreX = null;
            ScoreY = null;
        }

        public void Save(string path)
        {
            Model.SaveImage(System.IO.Path.Combine(path, $"{Index}_1ModelSubImage.png"));
            Target.SaveImage(System.IO.Path.Combine(path, $"{Index}_2TargetSubImage.png"));
            Binalize.SaveImage(System.IO.Path.Combine(path, $"{Index}_3Binalize{Threshold}.png"));
            Weight.SaveImage(System.IO.Path.Combine(path, $"{Index}_4WeightImage.png"));
            Result.SaveImage(System.IO.Path.Combine(path, $"{Index}_5ResultSubImage.png"));

            if (ScoreX != null)
                RCIHelper.SaveTmOffsetDebugData(System.IO.Path.Combine(path, $"{Index}_TmOffsetX.txt"), ModelProjection.PrjH, TargetProjection.PrjH, ScoreX, (int)Math.Round(TmOffsetPoint.X));

            if (ScoreY != null)
                RCIHelper.SaveTmOffsetDebugData(System.IO.Path.Combine(path, $"{Index}_TmOffsetY.txt"), ModelProjection.PrjV, TargetProjection.PrjV, ScoreY, (int)Math.Round(TmOffsetPoint.Y));
        }
    }


    public class PTMLogger : IDisposable
    {
        public int Index => this.WorkPoint.Index;
        public Point GridLocation => new Point(this.WorkPoint.Column, this.WorkPoint.Row);

        public ImageD ModelImageD { get; set; }
        public ImageD TargetImageD { get; set; }
        public WorkPoint WorkPoint { get; set; }
        public Direction Direction { get; set; }
        public int TargetOffset { get; set; }
        public float[] ModelPrj { get; set; }
        public float[] TargetPrj { get; set; }
        public float[] Scores { get; set; }
        public float MaxPos { get; set; }

        public void Dispose()
        {
            this.ModelImageD?.Dispose();
            this.TargetImageD?.Dispose();
        }

        public void Save(string path)
        {
            this.ModelImageD.SaveImage(System.IO.Path.Combine(path, $"{Index}_ModelImageD.png"));
            this.TargetImageD.SaveImage(System.IO.Path.Combine(path, $"{Index}_TargetImageD.png"));

            RCIHelper.SaveTmOffsetDebugData(System.IO.Path.Combine(path, $"{Index}_TmOffset_{this.Direction.ToString().Substring(0, 1)}.txt"), ModelPrj, TargetPrj, Scores, (int)Math.Round(MaxPos));
        }
    }


    public class BlockProjection : IDisposable
    {
        public AlgoImage Horizental { get; private set; }
        public AlgoImage Veritical { get; private set; }
        public Size Inflate { get; private set; }

        public AlgoImage HorizentalBuildBuffer { get; private set; }
        public AlgoImage VeriticalBuildBuffer { get; private set; }

        public BlockProjection(AlgoImage horizental, AlgoImage veritical, Size inflate)
        {
            this.Horizental = horizental;
            this.Veritical = veritical;
            this.Inflate = inflate;

            this.HorizentalBuildBuffer = ImageBuilder.Build(horizental.LibraryType, ImageType.Depth, horizental.Width, horizental.Height);
            this.VeriticalBuildBuffer = ImageBuilder.Build(veritical.LibraryType, ImageType.Depth, veritical.Width, veritical.Height);
        }

        public void Build(AlgoImage algoImage)
        {
            RCIHelper.BuildSoblePrj(algoImage, this.Horizental, this.HorizentalBuildBuffer, this.Inflate, Direction.Horizontal);
            RCIHelper.BuildSoblePrj(algoImage, this.Veritical, this.VeriticalBuildBuffer, this.Inflate, Direction.Vertical);
        }

        public void Build(AlgoImage algoImage, Direction direction)
        {
            if (direction == Direction.Horizontal)
                RCIHelper.BuildSoblePrj(algoImage, this.Horizental, this.HorizentalBuildBuffer, this.Inflate, Direction.Horizontal);

            if (direction == Direction.Vertical)
                RCIHelper.BuildSoblePrj(algoImage, this.Veritical, this.VeriticalBuildBuffer, this.Inflate, Direction.Vertical);
        }

        public void Dispose()
        {
            this.HorizentalBuildBuffer?.Dispose();
            this.VeriticalBuildBuffer?.Dispose();

            this.Horizental?.Dispose();
            this.Veritical?.Dispose();

            this.Inflate = Size.Empty;
        }

        public ProjectionData GetWorkRectProjection()
        {
            float[] prjH, prjV;
            if (this.Horizental.ImageType == ImageType.Grey)
                prjH = this.Horizental.GetByte().Select(f => (float)f).ToArray();
            else
                prjH = RCIHelper.GetSingles(this.Horizental.GetByte());

            if (this.Veritical.ImageType == ImageType.Grey)
                prjV = this.Veritical.GetByte().Select(f => (float)f).ToArray();
            else
                prjV = RCIHelper.GetSingles(this.Veritical.GetByte());

            //return new WorkRectProjection(this.Inflate, RCIHelper.GetSingles(this.Horizental.GetByte()), RCIHelper.GetSingles(this.Veritical.GetByte()));
            return new ProjectionData(this.Inflate, prjH, prjV);
        }
    }
}
