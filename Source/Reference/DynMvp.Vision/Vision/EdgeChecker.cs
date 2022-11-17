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
    public enum OffsetAxisType
    {
        xOffsetAxis, yOffsetAxis
    }

    public class EdgeCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(EdgeChecker))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is EdgeChecker)
            {
                EdgeChecker edgeChecker = (EdgeChecker)value;
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class EdgeCheckerParam : AlgorithmParam
    {
        EdgeDetectorParam edgeDetectorParam = new EdgeDetectorParam();
        public EdgeDetectorParam EdgeDetectorParam
        {
            get { return edgeDetectorParam; }
            set { edgeDetectorParam = value; }
        }

        OffsetAxisType offsetAxisType;
        public OffsetAxisType OffsetAxisType
        {
            get { return offsetAxisType; }
            set { offsetAxisType = value; }
        }

        private float desiredOffset;
        public float DesiredOffset
        {
            get { return desiredOffset; }
            set { desiredOffset = value; }
        }

        private float maxOffsetGap;
        public float MaxOffsetGap
        {
            get { return maxOffsetGap; }
            set { maxOffsetGap = value; }
        }

        public EdgeCheckerParam()
        {
            offsetAxisType = OffsetAxisType.yOffsetAxis;
            desiredOffset = 50;
            maxOffsetGap = 10;
        }

        public override AlgorithmParam Clone()
        {
            EdgeCheckerParam param = new EdgeCheckerParam();

            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            EdgeCheckerParam param = (EdgeCheckerParam)srcAlgorithmParam;
            edgeDetectorParam = (LineDetectorParam)param.edgeDetectorParam.Clone();

            offsetAxisType = param.offsetAxisType;
            desiredOffset = param.desiredOffset;
            maxOffsetGap = param.maxOffsetGap;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            edgeDetectorParam.LoadParam(algorithmElement);
            offsetAxisType = (OffsetAxisType)Enum.Parse(typeof(OffsetAxisType), XmlHelper.GetValue(algorithmElement, "OffsetAxisType", "yOffsetAxis"));
            desiredOffset = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "DesiredOffset", "50"));
            maxOffsetGap = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MaxOffsetGap", "10"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            edgeDetectorParam.SaveParam(algorithmElement);
            XmlHelper.SetValue(algorithmElement, "OffsetAxisType", offsetAxisType.ToString());
            XmlHelper.SetValue(algorithmElement, "DesiredOffset", desiredOffset.ToString());
            XmlHelper.SetValue(algorithmElement, "MaxOffsetGap", maxOffsetGap.ToString());
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    [TypeConverterAttribute(typeof(EdgeCheckerConverter))]
    public class EdgeChecker : Algorithm
    {
        public EdgeChecker()
        {
            param = new EdgeCheckerParam();
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            PointF[] points = region.GetPoints();
            EdgeCheckerParam edgeCheckerParam = (EdgeCheckerParam)param;

            Pen pen = new Pen(Color.Cyan, 1.0F);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            float desiredOffset = edgeCheckerParam.DesiredOffset;

            switch (edgeCheckerParam.OffsetAxisType)
            {
                case OffsetAxisType.xOffsetAxis :
                    figureGroup.AddFigure(new LineFigure(new PointF(points[0].X + desiredOffset, points[0].Y), new PointF(points[3].X + desiredOffset, points[3].Y), pen));
                    break;
                case OffsetAxisType.yOffsetAxis :
                    figureGroup.AddFigure(new LineFigure(new PointF(points[0].X, points[0].Y + desiredOffset), new PointF(points[1].X, points[1].Y + desiredOffset), pen));
                    break;
            }
            
        }

        public override Algorithm Clone()
        {
            EdgeChecker edgeChecker = new EdgeChecker();
            edgeChecker.CopyFrom(this);

            return edgeChecker;
        }

        public static string TypeName
        {
            get { return "EdgeChecker"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Edge";
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            EdgeCheckerParam edgeCheckerParam = (EdgeCheckerParam)param;

            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("EdgePoint", new PointF(0, 0)));
            resultValues.Add(new AlgorithmResultValue("Gap", edgeCheckerParam.MaxOffsetGap, 0, 0));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.ClipImage, ImageType.Grey, param.ImageBand);
            Filter(clipImage);

            EdgeCheckerParam edgeCheckerParam = (EdgeCheckerParam)param;

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect clipRegionInFov = inspectParam.ClipRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            RectangleF probeRegionInClip = DrawingHelper.FovToClip(clipRegionInFov, probeRegionInFov);

            AlgorithmResult edgeCheckerResult = CreateAlgorithmResult();
            edgeCheckerResult.ResultRect = probeRegionInFov;

            bool result = false;

            EdgeDetector edgeDetector = AlgorithmBuilder.CreateEdgeDetector();
            if (edgeDetector == null)
                result = false;

            EdgeDetectionResult edgeResult = null;

            if (result == true)
            {
                PointF[] points = DrawingHelper.GetPoints(probeRegionInClip, 0);

                edgeDetector.Param = edgeCheckerParam.EdgeDetectorParam;

                float detectorHalfHeight = MathHelper.GetLength(points[0], points[3]) / 2;
                float detectorHalfWidth = MathHelper.GetLength(points[0], points[1]) / 2;

                PointF centerPt = DrawingHelper.CenterPoint(probeRegionInClip);
                RotatedRect rectangle = new RotatedRect(centerPt.X - detectorHalfWidth, centerPt.Y - detectorHalfHeight,
                                                    detectorHalfWidth * 2, detectorHalfHeight * 2, 270);

                edgeResult = edgeDetector.Detect(clipImage, rectangle, debugContext);
                result &= edgeResult.Result;
            }

            if (result == true)
            {
                float gap = 0;

                switch(edgeCheckerParam.OffsetAxisType)
                {
                    case OffsetAxisType.xOffsetAxis :
                        gap = Math.Abs(edgeCheckerParam.DesiredOffset - edgeResult.EdgePosition.X);
                        break;
                    case OffsetAxisType.yOffsetAxis :
                        gap = Math.Abs(edgeCheckerParam.DesiredOffset - edgeResult.EdgePosition.Y);
                        break;
                }

                edgeCheckerResult.Good = gap <= edgeCheckerParam.MaxOffsetGap;
                edgeCheckerResult.ResultValueList.Add(new AlgorithmResultValue("EdgePoint", edgeResult.EdgePosition));
                edgeCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Gap", edgeCheckerParam.MaxOffsetGap, 0, gap));
            }
            else
            {
                edgeCheckerResult.Good = false;
                edgeCheckerResult.ResultValueList.Add(new AlgorithmResultValue("EdgePoint", new PointF(0,0)));
                edgeCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Gap", edgeCheckerParam.MaxOffsetGap, 0, 0));
            }

            return edgeCheckerResult;
        }
    }
}
