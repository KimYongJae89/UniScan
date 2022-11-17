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
    public enum CornerType
    {
        LeftTop, RightTop, LeftBottom, RightBottom
    }

    public enum RectType
    {
        Vertical, Horizontal
    }

    public class CornerCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(CornerChecker))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is CornerChecker)
            {
                CornerChecker widthChecker = (CornerChecker)value;
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }    

    public class CornerCheckerParam : AlgorithmParam
    {
        CornerType cornerType = CornerType.LeftTop;
        public CornerType CornerType
        {
            get { return cornerType; }
            set { cornerType = value; }
        }

        private EdgeType edge1Type = EdgeType.DarkToLight;
        public EdgeType Edge1Type
        {
            get { return edge1Type; }
            set { edge1Type = value; }
        }

        private EdgeType edge2Type = EdgeType.DarkToLight;
        public EdgeType Edge2Type
        {
            get { return edge2Type; }
            set { edge2Type = value; }
        }

        LineDetectorParam lineDetectorParam = new LineDetectorParam();
        public LineDetectorParam LineDetectorParam
        {
            get { return lineDetectorParam; }
            set { lineDetectorParam = value; }
        }

        public override AlgorithmParam Clone()
        {
            CornerCheckerParam param = new CornerCheckerParam();

            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            CornerCheckerParam param = (CornerCheckerParam)srcAlgorithmParam;

            cornerType = param.cornerType;
            edge1Type = param.edge1Type;
            edge2Type = param.edge2Type;
            lineDetectorParam = (LineDetectorParam)param.lineDetectorParam.Clone();
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            cornerType = (CornerType)Enum.Parse(typeof(CornerType), XmlHelper.GetValue(algorithmElement, "CornerType", CornerType.LeftTop.ToString()));
            edge1Type = (EdgeType)Enum.Parse(typeof(EdgeType), XmlHelper.GetValue(algorithmElement, "Edge1Type", EdgeType.DarkToLight.ToString()));
            edge2Type = (EdgeType)Enum.Parse(typeof(EdgeType), XmlHelper.GetValue(algorithmElement, "Edge2Type", EdgeType.DarkToLight.ToString()));
            lineDetectorParam.LoadParam(algorithmElement);
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "CornerType", cornerType.ToString());
            XmlHelper.SetValue(algorithmElement, "Edge1Type", edge1Type.ToString());
            XmlHelper.SetValue(algorithmElement, "Edge2Type", edge2Type.ToString());
            lineDetectorParam.SaveParam(algorithmElement);
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    [TypeConverterAttribute(typeof(WidthCheckerConverter))]
    public class CornerChecker : Algorithm
    {
        public CornerChecker()
        {
            param = new CornerCheckerParam();
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            PointF[] points = region.GetPoints();

            LineDetector lineDetector = AlgorithmBuilder.CreateLineDetector();
            //((LineDetectorParam)lineDetector). = param ;

            PointF centerPoint = DrawingHelper.CenterPoint(points[2], points[0]);
            PointF topCenterPoint = DrawingHelper.CenterPoint(points[1], points[0]);
            PointF bottomCenterPoint = DrawingHelper.CenterPoint(points[3], points[2]);
            PointF leftCenterPoint = DrawingHelper.CenterPoint(points[3], points[0]);
            PointF rightCenterPoint = DrawingHelper.CenterPoint(points[2], points[1]);

            CornerCheckerParam cornerCheckerParam = (CornerCheckerParam)param;

            switch (cornerCheckerParam.CornerType)
            {
                case CornerType.RightBottom:
                    lineDetector.AppendLineDetectorFigures(figureGroup, leftCenterPoint, centerPoint);
                    lineDetector.AppendLineDetectorFigures(figureGroup, topCenterPoint, centerPoint);
                    break;
                case CornerType.RightTop:
                    lineDetector.AppendLineDetectorFigures(figureGroup, rightCenterPoint, centerPoint);
                    lineDetector.AppendLineDetectorFigures(figureGroup, topCenterPoint, centerPoint);
                    break;
                case CornerType.LeftBottom:
                    lineDetector.AppendLineDetectorFigures(figureGroup, leftCenterPoint, centerPoint);
                    lineDetector.AppendLineDetectorFigures(figureGroup, bottomCenterPoint, centerPoint);
                    break;
                case CornerType.LeftTop:
                    lineDetector.AppendLineDetectorFigures(figureGroup, rightCenterPoint, centerPoint);
                    lineDetector.AppendLineDetectorFigures(figureGroup, bottomCenterPoint, centerPoint);
                    break;
            }
        }

        public override Algorithm Clone()
        {
            CornerChecker cornerChecker = new CornerChecker();
            cornerChecker.CopyFrom(this);

            return cornerChecker;
        }

        public override void CopyFrom(Algorithm algorithm)
        {
            base.CopyFrom(algorithm);

            CornerChecker cornerChecker = (CornerChecker)algorithm;

            param = (CornerCheckerParam)cornerChecker.Param.Clone();
        }

        public static string TypeName
        {
            get { return "CornerChecker"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Corner";
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            inspRegion.Inflate(((CornerCheckerParam)param).LineDetectorParam.SearchLength / 2, 0);
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("CornerPoint", new PointF(0, 0)));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.ClipImage, ImageType.Grey, param.ImageBand);
            Filter(clipImage);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect clipRegionInFov = inspectParam.ClipRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(clipRegionInFov, probeRegionInFov);

            PointF[] points = DrawingHelper.GetPoints(probeRegionInClip, 0);

            AlgorithmResult cornerCheckerResult = CreateAlgorithmResult();
            cornerCheckerResult.ResultRect = probeRegionInFov;

            CornerResult cornerResult = GetCornerWithLine(clipImage, probeRegionInClip, debugContext);

            if (cornerResult.Good == true)
            {
                PointF centerPoint = DrawingHelper.CenterPoint(points[2], points[0]);

                SizeF offset = new SizeF();
                offset.Width = cornerResult.CornerPoint.X - centerPoint.X;
                offset.Height = cornerResult.CornerPoint.Y - centerPoint.Y;

                cornerCheckerResult.OffsetFound = offset;
                PointF cornerPoint = new PointF(cornerResult.CornerPoint.X + clipRegionInFov.X,
                                                                cornerResult.CornerPoint.Y + clipRegionInFov.Y);

                cornerCheckerResult.ResultValueList.Add(new AlgorithmResultValue("CornerPoint", cornerPoint));
                cornerCheckerResult.ResultFigureGroup.AddFigure(new CrossFigure(cornerPoint, 10, new Pen(Color.White, 1.0F)));
            }
            else
            {
                cornerCheckerResult.OffsetFound = new SizeF(0, 0);
                cornerCheckerResult.ResultValueList.Add(new AlgorithmResultValue("CornerPoint", new PointF(0, 0)));
            }

            return cornerCheckerResult;
        }

        private CornerResult GetCornerWithLine(AlgoImage probeClipImage, RectangleF probeRegionInClip, DebugContext debugContext)
        {
            CornerResult cornerResult = new CornerResult();

            LineDetector lineDetector = AlgorithmBuilder.CreateLineDetector();

            PointF[] points = DrawingHelper.GetPoints(probeRegionInClip, 0);

            CornerCheckerParam cornerCheckerParam = (CornerCheckerParam)param;
            lineDetector.Param = cornerCheckerParam.LineDetectorParam;

            PointF centerPoint = DrawingHelper.CenterPoint(points[2], points[0]);
            PointF topCenterPoint = DrawingHelper.CenterPoint(points[1], points[0]);
            PointF bottomCenterPoint = DrawingHelper.CenterPoint(points[3], points[2]);
            PointF leftCenterPoint = DrawingHelper.CenterPoint(points[3], points[0]);
            PointF rightCenterPoint = DrawingHelper.CenterPoint(points[2], points[1]);

            LineEq lineEq1 = new LineEq(), lineEq2 = new LineEq();

            switch (cornerCheckerParam.CornerType)
            {
                case CornerType.RightBottom:
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge1Type;
                    lineEq1 = lineDetector.Detect(probeClipImage, leftCenterPoint, centerPoint, debugContext);
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge2Type;
                    lineEq2 = lineDetector.Detect(probeClipImage, topCenterPoint, centerPoint, debugContext);
                    break;
                case CornerType.RightTop:
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge1Type;
                    lineEq1 = lineDetector.Detect(probeClipImage, rightCenterPoint, centerPoint, debugContext);
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge2Type;
                    lineEq2 = lineDetector.Detect(probeClipImage, topCenterPoint, centerPoint, debugContext);
                    break;
                case CornerType.LeftBottom:
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge1Type;
                    lineEq1 = lineDetector.Detect(probeClipImage, leftCenterPoint, centerPoint, debugContext);
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge2Type;
                    lineEq2 = lineDetector.Detect(probeClipImage, bottomCenterPoint, centerPoint, debugContext);
                    break;
                case CornerType.LeftTop:
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge1Type;
                    lineEq1 = lineDetector.Detect(probeClipImage, rightCenterPoint, centerPoint, debugContext);
                    lineDetector.Param.EdgeType = cornerCheckerParam.Edge2Type;
                    lineEq2 = lineDetector.Detect(probeClipImage, bottomCenterPoint, centerPoint, debugContext);
                    break;
            }

            PointF pt = new PointF();
            cornerResult.Good = LineEq.GetIntersectPoint(lineEq1, lineEq2, ref pt);
            if (cornerResult.Good == true)
                cornerResult.CornerPoint = pt;

            return cornerResult;
        }
    }

    public class CornerResult
    {
        bool good;
        public bool Good
        {
            get { return good; }
            set { good = value; }
        }

        PointF cornerPoint;
        public PointF CornerPoint
        {
            get { return cornerPoint; }
            set { cornerPoint = value; }
        }
    }
}
