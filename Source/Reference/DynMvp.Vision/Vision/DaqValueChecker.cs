using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

using DynMvp.Base;
using DynMvp.UI;

namespace DynMvp.Vision
{
    class DaqValueChecker : Algorithm
    {
        public DaqValueChecker()
        {
        }

        public override Algorithm Clone()
        {
            DaqValueChecker daqValueChecker = new DaqValueChecker();
            daqValueChecker.CopyFrom(this);

            return daqValueChecker;
        }

        public override void CopyFrom(Algorithm algorithm)
        {
            base.CopyFrom(algorithm);
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();

            return resultValues;
        }

        public static string TypeName
        {
            get { return "DaqValueChecker"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "DaqValue";
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            //PointF[] points = region.GetPoints();

            //Pen pen = new Pen(Color.Cyan, 1.0F);
            //pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            //switch (offsetAxisType)
            //{
            //    case Vision.OffsetAxisType.xOffsetAxis:
            //        figureGroup.AddFigure(new LineFigure(new PointF(points[0].X + desiredOffset, points[0].Y), new PointF(points[3].X + desiredOffset, points[3].Y), pen));
            //        break;
            //    case Vision.OffsetAxisType.yOffsetAxis:
            //        figureGroup.AddFigure(new LineFigure(new PointF(points[0].X, points[0].Y + desiredOffset), new PointF(points[1].X, points[1].Y + desiredOffset), pen));
            //        break;
            //}

        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            //AlgoImage probeClipImage = inspectParam.ProbeClipImage;
            //RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            //RectangleF imageRegionInFov = inspectParam.ImageRegionInFov;
            //DebugContext debugContext = inspectParam.DebugContext;

            //RotatedRect probeRegionInImage = new RotatedRect(probeRegionInFov);
            //probeRegionInImage.Offset(-imageRegionInFov.X, -imageRegionInFov.Y);

            //EdgeCheckerResult edgeCheckerResult = new EdgeCheckerResult();
            //edgeCheckerResult.ResultRect = probeRegionInFov.GetBoundRect();

            //bool result = false;

            //EdgeDetector edgeDetector = AlgorithmBuilder.CreateEdgeDetector();
            //if (edgeDetector == null)
            //    result = false;

            //EdgeDetectionResult edgeResult = null;

            //if (result == true)
            //{
            //    PointF[] points = probeRegionInImage.GetPoints();

            //    edgeDetector.Param = param;

            //    float detectorHalfHeight = MathHelper.GetLength(points[0], points[3]) / 2;
            //    float detectorHalfWidth = MathHelper.GetLength(points[0], points[1]) / 2;

            //    PointF centerPt = DrawingHelper.CenterPoint(probeRegionInImage);
            //    RotatedRect rectangle = new RotatedRect(centerPt.X - detectorHalfWidth, centerPt.Y - detectorHalfHeight,
            //                                        detectorHalfWidth * 2, detectorHalfHeight * 2, 270);

            //    edgeResult = edgeDetector.Detect(probeClipImage, rectangle, debugContext);
            //    result &= edgeResult.Result;
            //}

            //if (result == true)
            //{
            //    float gap = 0;

            //    switch (offsetAxisType)
            //    {
            //        case Vision.OffsetAxisType.xOffsetAxis:
            //            gap = Math.Abs(desiredOffset - edgeResult.EdgePosition.X);
            //            break;
            //        case Vision.OffsetAxisType.yOffsetAxis:
            //            gap = Math.Abs(desiredOffset - edgeResult.EdgePosition.Y);
            //            break;
            //    }

            //    edgeCheckerResult.Good = gap <= maxOffsetGap;
            //}
            //else
            //{
            //}

            //return edgeCheckerResult;
            return null;
        }
    }
}
