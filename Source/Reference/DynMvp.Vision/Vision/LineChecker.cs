using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.ComponentModel;
using System.Globalization;

using DynMvp.Base;
using DynMvp.UI;

namespace DynMvp.Vision
{
    public class LineCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(LineChecker))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is LineChecker)
            {
                LineChecker lineChecker = (LineChecker)value;
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class LineCheckerParam : AlgorithmParam
    {
        LineDetectorParam lineDetectorParam = new LineDetectorParam();
        public LineDetectorParam LineDetectorParam
        {
            get { return lineDetectorParam; }
            set { lineDetectorParam = value; }
        }

        float startPosition;
        public float StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        float endPosition;
        public float EndPosition
        {
            get { return endPosition; }
            set { endPosition = value; }
        }

        float scaleValue;
        public float ScaleValue
        {
            get { return scaleValue; }
            set { scaleValue = value; }
        }

        public LineCheckerParam()
        {
            startPosition = 80;
            endPosition = 120;

            scaleValue = 1;
        }

        public override AlgorithmParam Clone()
        {
            LineCheckerParam param = new LineCheckerParam();

            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            LineCheckerParam param = (LineCheckerParam)srcAlgorithmParam;
            lineDetectorParam = (LineDetectorParam)param.lineDetectorParam.Clone();

            startPosition = param.startPosition;
            endPosition = param.endPosition;
            scaleValue = param.scaleValue;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            startPosition = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "StartPosition", "80"));
            endPosition = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "EndPosition", "120"));
            scaleValue = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "ScaleValue", "1.0"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "StartPosition", startPosition.ToString());
            XmlHelper.SetValue(algorithmElement, "EndPosition", endPosition.ToString());
            XmlHelper.SetValue(algorithmElement, "ScaleValue", scaleValue.ToString());
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    [TypeConverterAttribute(typeof(LineCheckerConverter))]
    public class LineChecker : Algorithm
    {
        public LineChecker()
        {
            param = new LineCheckerParam();
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            LineDetector lineDetector = AlgorithmBuilder.CreateLineDetector();
            lineDetector.Param = ((LineCheckerParam)param).LineDetectorParam;

            lineDetector.AppendLineDetectorFigures(figureGroup, new PointF(region.Left, region.Top), new PointF(region.Right, region.Bottom));
        }

        public override Algorithm Clone()
        {
            LineChecker lineChecker = new LineChecker();
            lineChecker.CopyFrom(this);

            return lineChecker;
        }

        public static string TypeName
        {
            get { return "LineChecker"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Line";
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            LineDetectorParam lineDetectorParam = ((LineCheckerParam)param).LineDetectorParam;
            inspRegion.Inflate(lineDetectorParam.SearchLength/2, 0);
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            LineCheckerParam lineCheckerParam = (LineCheckerParam)param;

            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("Line Position", lineCheckerParam.EndPosition, lineCheckerParam.StartPosition, 0));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.ClipImage, ImageType.Grey, param.ImageBand);
            Filter(clipImage);

            LineCheckerParam lineCheckerParam = (LineCheckerParam)param;

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect clipRegionInFov = inspectParam.ClipRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(clipRegionInFov, probeRegionInFov);

            AlgorithmResult lineCheckerResult = CreateAlgorithmResult();
            lineCheckerResult.ResultRect = probeRegionInFov;

            LineDetectorResult lineDetectorResult = GetLengthWithLine(clipImage, probeRegionInClip, debugContext);
            if (lineDetectorResult.Good == true)
            {
                lineCheckerResult.Good = (lineDetectorResult.Position >= lineCheckerParam.StartPosition) && (lineDetectorResult.Position <= lineCheckerParam.EndPosition);
                lineCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Line Position", lineCheckerParam.EndPosition, lineCheckerParam.StartPosition, lineDetectorResult.Position));

                lineCheckerResult.ResultFigureGroup.AddFigure(new LineFigure(lineDetectorResult.EdgePoint1, lineDetectorResult.EdgePoint2, new Pen(Color.White, 1.0F)));
                lineCheckerResult.ResultFigureGroup.AddFigure(new CrossFigure(lineDetectorResult.EdgePoint1, 10, new Pen(Color.White, 1.0F)));
                lineCheckerResult.ResultFigureGroup.AddFigure(new CrossFigure(lineDetectorResult.EdgePoint2, 10, new Pen(Color.White, 1.0F)));
            }
            else
            {
                lineCheckerResult.Good = false;
                lineCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Line Position", lineCheckerParam.EndPosition, lineCheckerParam.StartPosition, 0));
            }

            return lineCheckerResult;
        }

        private LineDetectorResult GetLengthWithLine(AlgoImage clipImage, RectangleF probeRegion, DebugContext debugContext)
        {
            LineDetectorResult lineDetectorResult = new LineDetectorResult();

            LineDetector lineDetector = AlgorithmBuilder.CreateLineDetector();
            lineDetector.Param = ((LineCheckerParam)param).LineDetectorParam;

            PointF startPoint = new PointF(probeRegion.Left, probeRegion.Top);
            PointF endPoint = new PointF(probeRegion.Right, probeRegion.Bottom);

            LineEq findLineEq = lineDetector.Detect(clipImage, startPoint, endPoint, debugContext);

            PointF[] points = DrawingHelper.GetPoints(probeRegion, 0);
            LineEq startLineEq = new LineEq(points[0], points[3]);

            LineEq centerLineEq = new LineEq(DrawingHelper.CenterPoint(points[3], points[0]), DrawingHelper.CenterPoint(points[1], points[2]));

            PointF pt1 = new PointF();
            PointF pt2 = new PointF();

            bool result1 = LineEq.GetIntersectPoint(centerLineEq, startLineEq, ref pt1);
            bool result2 = LineEq.GetIntersectPoint(centerLineEq, findLineEq, ref pt2);

            if (result1 == true && result2 == true)
            {
                lineDetectorResult.Good = true;
                lineDetectorResult.Position = MathHelper.GetLength(pt1, pt2);
                lineDetectorResult.EdgePoint1 = pt1;
                lineDetectorResult.EdgePoint2 = pt2;
            }

            return lineDetectorResult;
        }
    }

    public class LineDetectorResult
    {
        private bool good = false;
        public bool Good
        {
            get { return good; }
            set { good = value; }
        }

        private float position;
        public float Position
        {
            get { return position; }
            set { position = value; }
        }

        PointF edgePoint1;
        public PointF EdgePoint1
        {
            get { return edgePoint1; }
            set { edgePoint1 = value; }
        }

        PointF edgePoint2;
        public PointF EdgePoint2
        {
            get { return edgePoint2; }
            set { edgePoint2 = value; }
        }
    }
}
