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
    public class LineDetectorParam : EdgeDetectorParam
    {
        private int numEdgeDetector;
        public int NumEdgeDetector
        {
            get { return numEdgeDetector; }
            set { numEdgeDetector = value; }
        }

        private int searchLength;
        public int SearchLength
        {
            get { return searchLength; }
            set { searchLength = value; }
        }

        private int projectionHeight;
        public int ProjectionHeight
        {
            get { return projectionHeight; }
            set { projectionHeight = value; }
        }

        private float searchAngle;
        public float SearchAngle
        {
            get { return searchAngle; }
            set { searchAngle = value; }
        }

        public LineDetectorParam() : base()
        {
            numEdgeDetector = 1;
            searchLength = 60;
            projectionHeight = 10;
            searchAngle = 90;
        }

        public override EdgeDetectorParam Clone()
        {
            LineDetectorParam param = new LineDetectorParam();

            param.Copy(this);

            return param;
        }

        public override void Copy(EdgeDetectorParam srcAlgorithmParam)
        {
            base.Copy(srcAlgorithmParam);

            LineDetectorParam param = (LineDetectorParam)srcAlgorithmParam;

            numEdgeDetector = param.numEdgeDetector;
            projectionHeight = param.projectionHeight;
            searchLength = param.searchLength;
            searchAngle = param.searchAngle;
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);

            numEdgeDetector = Convert.ToInt32(XmlHelper.GetValue(paramElement, "NumEdgeDetector", "1"));
            searchLength = Convert.ToInt32(XmlHelper.GetValue(paramElement, "SearchLength", "60"));
            projectionHeight = Convert.ToInt32(XmlHelper.GetValue(paramElement, "ProjectionHeight", "10"));
            searchAngle = Convert.ToSingle(XmlHelper.GetValue(paramElement, "SearchAngle", "90"));
        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);

            XmlHelper.SetValue(paramElement, "NumEdgeDetector", numEdgeDetector.ToString());
            XmlHelper.SetValue(paramElement, "SearchLength", searchLength.ToString());
            XmlHelper.SetValue(paramElement, "ProjectionHeight", projectionHeight.ToString());
            XmlHelper.SetValue(paramElement, "SearchAngle", searchAngle.ToString());
        }
    }

    public abstract class LineDetector
    {
        LineDetectorParam param;
        public LineDetectorParam Param
        {
            get { return param; }
            set { param = value; }
        }

        public static string TypeName
        {
            get { return "LineDetector"; }
        }

        public void AppendLineDetectorFigures(FigureGroup figureGroup, PointF startPt, PointF endPt)
        {
            float xStep = (endPt.X - startPt.X) / param.NumEdgeDetector;
            float yStep = (endPt.Y - startPt.Y) / param.NumEdgeDetector;

            float detectorHalfHeight = param.ProjectionHeight / 2;
            float detectorHalfWidth = param.SearchLength / 2;

            float theta = (float)MathHelper.RadToDeg(MathHelper.arctan(startPt.Y - endPt.Y, endPt.X - startPt.X)) + 90 + (param.SearchAngle - 90);

            for (int i = 0; i < param.NumEdgeDetector; i++)
            {
                PointF centerPt = new PointF(startPt.X + xStep * (float)(i + 0.5), startPt.Y + yStep * (float)(i + 0.5));
                RotatedRect rectangle = new RotatedRect(centerPt.X - detectorHalfWidth, centerPt.Y - detectorHalfHeight,
                                                    detectorHalfWidth * 2, detectorHalfHeight * 2, theta);
                figureGroup.AddFigure(new RectangleFigure(rectangle, new Pen(Color.Cyan, 1.0F)));

                PointF[] rectPos = rectangle.GetPoints();
                figureGroup.AddFigure(new LineFigure(rectPos[0], centerPt, new Pen(Color.Cyan, 1.0F)));
                figureGroup.AddFigure(new LineFigure(rectPos[3], centerPt, new Pen(Color.Cyan, 1.0F)));
            }
        }

        public abstract LineEq Detect(AlgoImage algoImage, PointF startPt, PointF endPt, DebugContext debugContext);
    }
}
