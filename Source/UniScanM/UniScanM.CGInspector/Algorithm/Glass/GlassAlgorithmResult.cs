using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanM.CGInspector.Data;

namespace UniScanM.CGInspector.Algorithm.Glass
{
    class GlassAlgorithmResult : DynMvp.Vision.AlgorithmResult
    {
        public GlassAlgorithmParam AlgorithmParam { get => this.algorithmParam; set => this.algorithmParam = value; }
        GlassAlgorithmParam algorithmParam;

        public ImageD ResultImageD { get => this.resultImageD; set => this.resultImageD = value; }
        ImageD resultImageD;

        public RotatedRect ClipRect { get => this.clipRect; set => this.clipRect = value; }
        RotatedRect clipRect;

        public DefectOnScreen[] DefectOnScreens { get => this.defectOnScreens; set => this.defectOnScreens = value; }
        DefectOnScreen[] defectOnScreens;

        public GlassAlgorithmResult() : base(GlassAlgorithm.TypeName)
        {
        }

        public override void AppendResultFigures(FigureGroup figureGroup, PointF offset)
        {
            FigureGroup resultFigureGroup = new FigureGroup();

            Array.ForEach(this.defectOnScreens, f =>
            {
                Rectangle drawRect = f.Rectangle;
                RectangleFigure rectangleFigure = new RectangleFigure(drawRect, new Pen(Brushes.Red, 2));
                rectangleFigure.Selectable = false;
                rectangleFigure.Tag = f;
                resultFigureGroup.AddFigure(rectangleFigure);

                //PolygonFigure polygonFigure = new PolygonFigure(true, new Pen(Brushes.Blue, 1));
                //polygonFigure.AddPoints(Array.ConvertAll(f.Points, g => new PointF(g.X, g.Y)));
                //polygonFigure.Selectable = false;
                //polygonFigure.Tag = f;
                //resultFigureGroup.AddFigure(polygonFigure);
            });

            resultFigureGroup.Offset(offset.X,offset.Y);

            figureGroup.AddFigure(resultFigureGroup.FigureList.ToArray());
        }
    }
}
