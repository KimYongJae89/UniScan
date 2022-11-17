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
    public class WidthCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(WidthChecker))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is WidthChecker)
            {
                WidthChecker widthChecker = (WidthChecker)value;
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class WidthCheckerParam : AlgorithmParam
    {
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

        float maxWidthRatio;
        public float MaxWidthRatio
        {
            get { return maxWidthRatio; }
            set { maxWidthRatio = value; }
        }

        float maxCenterGap;
        public float MaxCenterGap
        {
            get { return maxCenterGap; }
            set { maxCenterGap = value; }
        }

        float minWidthRatio;
        public float MinWidthRatio
        {
            get { return minWidthRatio; }
            set { minWidthRatio = value; }
        }

        float scaleValue;
        public float ScaleValue
        {
            get { return scaleValue; }
            set { scaleValue = value; }
        }

        LineDetectorParam lineDetectorParam;
        public LineDetectorParam LineDetectorParam
        {
            get { return lineDetectorParam; }
            set { lineDetectorParam = value; }
        }

        public WidthCheckerParam()
        {
            minWidthRatio = 80;
            maxWidthRatio = 120;
            maxCenterGap = 50;

            edge1Type = EdgeType.LightToDark;
            edge2Type = EdgeType.DarkToLight;

            scaleValue = 1;

            lineDetectorParam = new LineDetectorParam();
        }

        public override AlgorithmParam Clone()
        {
            WidthCheckerParam param = new WidthCheckerParam();

            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            WidthCheckerParam param = (WidthCheckerParam)srcAlgorithmParam;

            minWidthRatio = param.minWidthRatio;
            maxWidthRatio = param.maxWidthRatio;
            edge1Type = param.edge1Type;
            edge2Type = param.edge2Type;
            scaleValue = param.scaleValue;
            maxCenterGap = param.maxCenterGap;

            lineDetectorParam.Copy(param.lineDetectorParam);
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            edge1Type = (EdgeType)Enum.Parse(typeof(EdgeType), XmlHelper.GetValue(algorithmElement, "Edge1Type", "LightToDark"));
            edge2Type = (EdgeType)Enum.Parse(typeof(EdgeType), XmlHelper.GetValue(algorithmElement, "Edge2Type", "DarkToLight"));

            minWidthRatio = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MinWidthRatio", "80"));
            maxWidthRatio = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MaxWidthRatio", "120"));
            maxCenterGap = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MaxCenterGap", "50"));
            scaleValue = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "ScaleValue", "1.0"));

            lineDetectorParam.LoadParam(algorithmElement);
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "Edge1Type", edge1Type.ToString());
            XmlHelper.SetValue(algorithmElement, "Edge2Type", edge2Type.ToString());

            XmlHelper.SetValue(algorithmElement, "MinWidthRatio", minWidthRatio.ToString());
            XmlHelper.SetValue(algorithmElement, "MaxWidthRatio", maxWidthRatio.ToString());
            XmlHelper.SetValue(algorithmElement, "MaxCenterGap", maxCenterGap.ToString());

            XmlHelper.SetValue(algorithmElement, "ScaleValue", scaleValue.ToString());

            lineDetectorParam.SaveParam(algorithmElement);
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    [TypeConverterAttribute(typeof(WidthCheckerConverter))]
    public class WidthChecker : Algorithm
    {
        public WidthChecker()
        {
            param = new WidthCheckerParam();
        }

        public override bool CanProcess3dImage()
        {
            return true;
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            PointF[] points = region.GetPoints();

            LineDetector lineDetector = AlgorithmBuilder.CreateLineDetector();
            if (lineDetector == null)
                return;

            lineDetector.Param = ((WidthCheckerParam)Param).LineDetectorParam;

            lineDetector.AppendLineDetectorFigures(figureGroup, points[0], points[3]);
            lineDetector.AppendLineDetectorFigures(figureGroup, points[2], points[1]);
        }

        public override Algorithm Clone()
        {
            WidthChecker widthDetector = new WidthChecker();
            widthDetector.CopyFrom(this);

            return widthDetector;
        }

        public override void CopyFrom(Algorithm algorithm)
        {
            base.CopyFrom(algorithm);

            WidthChecker widthDetector = (WidthChecker)algorithm;

            param.CopyFrom(widthDetector.param);
        }

        public static string TypeName
        {
            get { return "WidthChecker"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Width";
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            inspRegion.Inflate(((WidthCheckerParam)param).LineDetectorParam.SearchLength/2, 0);
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            WidthCheckerParam widthCheckerParam = (WidthCheckerParam)Param;

            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("Width Ratio", widthCheckerParam.MaxWidthRatio, widthCheckerParam.MinWidthRatio, 0));
            resultValues.Add(new AlgorithmResultValue("Width", widthCheckerParam.MaxWidthRatio, widthCheckerParam.MinWidthRatio, 0));
            resultValues.Add(new AlgorithmResultValue("Center Gap", widthCheckerParam.MaxCenterGap));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            bool on3dInspection = false;
            Image2D clipImage;
            if (inspectParam.ClipImage is Image3D)
            {
                clipImage = Image2D.FromBitmap(inspectParam.ClipImage.ToBitmap());
                on3dInspection = true;
            }
            else
            {
                clipImage = (Image2D)inspectParam.ClipImage;
            }

            WidthCheckerParam widthCheckerParam = (WidthCheckerParam)Param;

            DebugHelper.SaveImage(clipImage, "ClipImage.bmp", inspectParam.DebugContext);

            AlgoImage probeClipImage = ImageBuilder.Build(GetAlgorithmType(), clipImage, ImageType.Grey, param.ImageBand);
            Filter(probeClipImage);

            DebugHelper.SaveImage(probeClipImage, "ProbeClipImage.bmp", inspectParam.DebugContext);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect clipRegionInFov = inspectParam.ClipRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(clipRegionInFov, probeRegionInFov);

            bool result = false;

            PointF[] points = DrawingHelper.GetPoints(probeRegionInClip, 0);

            AlgorithmResult widthCheckerResult = CreateAlgorithmResult();
            widthCheckerResult.ResultRect = probeRegionInFov;

            float desiredLength = MathHelper.GetLength(DrawingHelper.CenterPoint(points[3], points[0]), DrawingHelper.CenterPoint(points[1], points[2]));

            WidthResult widthResult = null;

            if (((WidthCheckerParam)param).LineDetectorParam.NumEdgeDetector == 1)
            {
                widthResult = GetLengthWithEdge(probeClipImage, probeRegionInClip, debugContext);
            }
            else
            {
                widthResult = GetLengthWithLine(probeClipImage, probeRegionInClip, debugContext);
            }

            float measureLength = widthResult.MeasureLength;
            if (on3dInspection == true)
            {
                desiredLength *= 0.3f;
                measureLength *= 0.3f;
                widthResult.MeasureLength = measureLength;
            }
            widthResult.DesiredLength = desiredLength;

            if (result == true)
            {
                float ratio = widthResult.MeasureLength / desiredLength * 100;

                widthCheckerResult.Good = (ratio >= widthCheckerParam.MinWidthRatio) && (ratio <= widthCheckerParam.MaxWidthRatio); //  && gap <= maxCenterGap;

                PointF edgePoint1 = new PointF(widthResult.EdgePoint1.X + clipRegionInFov.X,
                                                                widthResult.EdgePoint1.Y + clipRegionInFov.Y);
                PointF edgePoint2 = new PointF(widthResult.EdgePoint2.X + clipRegionInFov.X,
                                                                widthResult.EdgePoint2.Y + clipRegionInFov.Y);

                widthCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Width Ratio", widthCheckerParam.MaxWidthRatio, widthCheckerParam.MinWidthRatio, ratio));
                widthCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Width", desiredLength * widthCheckerParam.MaxWidthRatio / 100,
                                                                desiredLength * widthCheckerParam.MinWidthRatio / 100, measureLength));
                widthCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Center Gap", widthCheckerParam.MaxCenterGap, 0, 0));

                widthCheckerResult.ResultFigureGroup.AddFigure(new CrossFigure(DrawingHelper.CenterPoint(edgePoint1, edgePoint2), 10, new Pen(Color.White, 1.0F)));
                widthCheckerResult.ResultFigureGroup.AddFigure(new CrossFigure(edgePoint1, 10, new Pen(Color.White, 1.0F)));
                widthCheckerResult.ResultFigureGroup.AddFigure(new CrossFigure(edgePoint2, 10, new Pen(Color.White, 1.0F)));
            }
            else
            {
                widthCheckerResult.Good = false;
                widthCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Width Ratio", widthCheckerParam.MaxWidthRatio, widthCheckerParam.MinWidthRatio, 0));
                widthCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Width", desiredLength * widthCheckerParam.MaxWidthRatio / 100,
                                                                desiredLength * widthCheckerParam.MinWidthRatio / 100, measureLength));
                widthCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Center Gap", widthCheckerParam.MaxCenterGap, 0, 0));
            }

            return widthCheckerResult;
        }

        private WidthResult GetLengthWithLine(AlgoImage probeClipImage, RectangleF probeRegion, DebugContext debugContext)
        {
            WidthResult widthResult = new WidthResult();

            LineDetector lineDetector = AlgorithmBuilder.CreateLineDetector();
            if (lineDetector == null)
                return widthResult;

            WidthCheckerParam widthCheckerParam = (WidthCheckerParam)Param;

            PointF[] points = DrawingHelper.GetPoints(probeRegion, 0);

            LineEq centerLineEq = new LineEq(DrawingHelper.CenterPoint(points[3], points[0]), DrawingHelper.CenterPoint(points[1], points[2]));

            lineDetector.Param = widthCheckerParam.LineDetectorParam;

            lineDetector.Param.EdgeType = widthCheckerParam.Edge1Type;
            LineEq startLineEq = lineDetector.Detect(probeClipImage, points[0], points[3], debugContext);

            lineDetector.Param.EdgeType = widthCheckerParam.Edge2Type;
            LineEq endLineEq = lineDetector.Detect(probeClipImage, points[2], points[1], debugContext);

            PointF pt1 = new PointF();
            PointF pt2 = new PointF();

            bool result1 = LineEq.GetIntersectPoint(centerLineEq, startLineEq, ref pt1);
            bool result2 = LineEq.GetIntersectPoint(centerLineEq, endLineEq, ref pt2);

            widthResult.Good = (result1 == true && result2 == true);
            if (widthResult.Good)
            {
                widthResult.MeasureLength = MathHelper.GetLength(pt1, pt2);
                widthResult.EdgePoint1 = pt1;
                widthResult.EdgePoint2 = pt2;
            }

            return widthResult;
        }

        public RotatedRect GetDetectorRect(PointF startPt, PointF endPt)
        {
            LineDetectorParam lineDetectorParam = ((WidthCheckerParam)Param).LineDetectorParam;

            float detectorHalfHeight = lineDetectorParam.ProjectionHeight / 2;
            float detectorHalfWidth = lineDetectorParam.SearchLength / 2;

            float theta = (float)(MathHelper.RadToDeg(MathHelper.arctan(endPt.Y - startPt.Y, endPt.X - startPt.X))) - 90;

            PointF centerPt = new PointF(startPt.X + (endPt.X - startPt.X) / 2, startPt.Y + (endPt.Y - startPt.Y) / 2);
            RotatedRect rectangle = new RotatedRect(centerPt.X - detectorHalfWidth, centerPt.Y - detectorHalfHeight,
                                                detectorHalfWidth * 2, detectorHalfHeight * 2, theta);

            return rectangle;
        }

        private WidthResult GetLengthWithEdge(AlgoImage probeClipImage, RectangleF probeRegion, DebugContext debugContext)
        {
            WidthResult widthResult = new WidthResult();

            EdgeDetector edgeDetector = AlgorithmBuilder.CreateEdgeDetector();
            if (edgeDetector == null)
                return widthResult;

            WidthCheckerParam widthCheckerParam = (WidthCheckerParam)Param;
            LineDetectorParam lineDetectorParam = widthCheckerParam.LineDetectorParam;

            PointF[] points = DrawingHelper.GetPoints(probeRegion, 1);

            lineDetectorParam.EdgeThreshold = 5;
            edgeDetector.Param = lineDetectorParam;

            RotatedRect rect1 = GetDetectorRect(points[0], points[3]);
            rect1.Angle = rect1.Angle + (lineDetectorParam.SearchAngle - 90);

            RotatedRect rect2 = GetDetectorRect(points[2], points[1]);
            rect2.Angle = rect2.Angle + (lineDetectorParam.SearchAngle - 90);

            edgeDetector.Param.EdgeType = widthCheckerParam.Edge1Type;
            EdgeDetectionResult result1 = edgeDetector.Detect(probeClipImage, rect1, debugContext);

            edgeDetector.Param.EdgeType = widthCheckerParam.Edge2Type;
            EdgeDetectionResult result2 = edgeDetector.Detect(probeClipImage, rect2, debugContext);

            if (result1.Result == true && result2.Result == true)
            {
                widthResult.MeasureLength = MathHelper.GetLength(result1.EdgePosition, result2.EdgePosition);
                widthResult.EdgePoint1 = result1.EdgePosition;
                widthResult.EdgePoint2 = result2.EdgePosition;
            }

            return widthResult;
        }
    }

    public class WidthResult
    {
        bool good;
        public bool Good
        {
            get { return good; }
            set { good = value; }
        }

        float desiredLength;
        public float DesiredLength
        {
            get { return desiredLength; }
            set { desiredLength = value; }
        }

        float measureLength;
        public float MeasureLength
        {
            get { return measureLength; }
            set { measureLength = value; }
        }

        float measureCenter;
        public float MeasureCenter
        {
            get { return measureCenter; }
            set { measureCenter = value; }
        }

        float desiredCenter;
        public float DesiredCenter
        {
            get { return desiredCenter; }
            set { desiredCenter = value; }
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
