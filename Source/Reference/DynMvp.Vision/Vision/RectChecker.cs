using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision.Planbss;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public class RectCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(RectChecker))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is RectChecker)
            {
                return "";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class RectCheckerParam : AlgorithmParam
    {
        EdgeType edgeType = 0;
        public EdgeType EdgeType
        {
            get { return edgeType; }
            set { edgeType = value; }
        }

        int edgeThickWidth = 1;
        public int EdgeThickWidth
        {
            get { return edgeThickWidth; }
            set { edgeThickWidth = value; }
        }

        int edgeThickHeight = 5;
        public int EdgeThickHeight
        {
            get { return edgeThickHeight; }
            set { edgeThickHeight = value; }
        }

        int edgeDistance = 5;
        public int EdgeDistance
        {
            get { return edgeDistance; }
            set { edgeDistance = value; }
        }

        int grayValue = 20;
        public int GrayValue
        {
            get { return grayValue; }
            set { grayValue = value; }
        }

        int projectionHeight = 3;
        public int ProjectionHeight
        {
            get { return projectionHeight; }
            set { projectionHeight = value; }
        }

        int passRate = 30;
        public int PassRate
        {
            get { return passRate; }
            set { passRate = value; }
        }

        CardinalPoint cardinalPoint = CardinalPoint.NorthWest;
        public CardinalPoint CardinalPoint
        {
            get { return cardinalPoint; }
            set { cardinalPoint = value; }
        }

        bool outToIn = true;
        public bool OutToIn
        {
            get { return outToIn; }
            set { outToIn = value; }
        }

        int searchRange = 20;
        public int SearchRange
        {
            get { return searchRange; }
            set { searchRange = value; }
        }

        int searchLength = 10;
        public int SearchLength
        {
            get { return searchLength; }
            set { searchLength = value; }
        }

        ConvexShape convexShape = ConvexShape.None;
        public ConvexShape ConvexShape
        {
            get { return convexShape; }
            set { convexShape = value; }
        }

        public RectCheckerParam()
        {
        }

        public override AlgorithmParam Clone()
        {
            RectCheckerParam param = new RectCheckerParam();

            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            RectCheckerParam param = (RectCheckerParam)srcAlgorithmParam;

            edgeType = param.edgeType;
            edgeThickWidth = param.edgeThickWidth;
            edgeThickHeight = param.edgeThickHeight;
            grayValue = param.grayValue;
            projectionHeight = param.projectionHeight;
            passRate = param.passRate;
            outToIn = param.outToIn;
            searchRange = param.searchRange;
            searchLength = param.searchLength;
            convexShape = param.convexShape;
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);

            edgeType  = (EdgeType)Enum.Parse(typeof(EdgeType), XmlHelper.GetValue(paramElement, "EdgeType", "Any"));
            edgeThickWidth = Convert.ToInt32(XmlHelper.GetValue(paramElement, "EdgeThickWidth", "1"));
            edgeThickHeight = Convert.ToInt32(XmlHelper.GetValue(paramElement, "EdgeThickHeight", "5"));
            grayValue = Convert.ToInt32(XmlHelper.GetValue(paramElement, "GrayValue", "20"));
            projectionHeight = Convert.ToInt32(XmlHelper.GetValue(paramElement, "ProjectionHeight", "3"));
            passRate = Convert.ToInt32(XmlHelper.GetValue(paramElement, "PassRate", "30"));
            outToIn = Convert.ToBoolean(XmlHelper.GetValue(paramElement, "OutToIn", "True"));
            searchRange = Convert.ToInt32(XmlHelper.GetValue(paramElement, "SearchRange", "20"));
            searchLength = Convert.ToInt32(XmlHelper.GetValue(paramElement, "Length", "70"));
            convexShape = (ConvexShape)Enum.Parse(typeof(ConvexShape), XmlHelper.GetValue(paramElement, "ConvexShape", "None"));
        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);

            XmlHelper.SetValue(paramElement, "SearchRange", searchRange.ToString());
            XmlHelper.SetValue(paramElement, "EdgeType", edgeType.ToString());
            XmlHelper.SetValue(paramElement, "EdgeThickWidth", EdgeThickWidth.ToString());
            XmlHelper.SetValue(paramElement, "EdgeThickHeight", EdgeThickHeight.ToString());
            XmlHelper.SetValue(paramElement, "GrayValue", grayValue.ToString());
            XmlHelper.SetValue(paramElement, "projectionHeightf", projectionHeight.ToString());
            XmlHelper.SetValue(paramElement, "PassRate", passRate.ToString());
            XmlHelper.SetValue(paramElement, "CardinalPoint", cardinalPoint.ToString());
            XmlHelper.SetValue(paramElement, "OutToIn", outToIn.ToString());
            XmlHelper.SetValue(paramElement, "SearchLength", searchLength.ToString());
            XmlHelper.SetValue(paramElement, "ConvexShape", convexShape.ToString());
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    [TypeConverterAttribute(typeof(RectCheckerConverter))]
    public class RectChecker : Algorithm
    {
        public RectChecker()
        {
            param = new RectCheckerParam();
        }

        public override Algorithm Clone()
        {
            RectChecker rectChecker = new RectChecker();
            rectChecker.CopyFrom(this);

            return rectChecker;
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {

        }

        public static string TypeName
        {
            get { return "RectChecker"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Rect";
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            //inspRegion.Inflate(param.SearchRange + 5, param.SearchRange + 5);
            inspRegion.Inflate(((RectCheckerParam)param).SearchLength + 5, ((RectCheckerParam)param).SearchLength + 5);
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("Angle", 0, 360, 0));
            resultValues.Add(new AlgorithmResultValue("CenterPos", new Point(0, 0)));
            //resultValues.Add(new AlgorithmResultValue("LeftTop", new Point(0, 0)));
            //resultValues.Add(new AlgorithmResultValue("RightTop", new Point(0, 0)));
            //resultValues.Add(new AlgorithmResultValue("RightBottom", new Point(0, 0)));
            //resultValues.Add(new AlgorithmResultValue("LeftBottom", new Point(0, 0)));

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

            AlgorithmResult algorithmResult = CreateAlgorithmResult();
            algorithmResult.ResultRect = probeRegionInFov;

            bool bAuto = false;  // Graphic use or ignore

        ///////////////////////////////////////////////////////////////////////////////////////////
        // Vision Initialize
        ///////////////////////////////////////////////////////////////////////////////////////////  
            VCFpcb      vcFpcb      = new VCFpcb();
            VCFpcbParam vcFpcbParam = new VCFpcbParam();
            HelpDraw    helpDraw    = bAuto ? null : new HelpDraw();

            VCLens.gsUser = eUSER.DEVELOPER;
            vcFpcbParam.gsMatch.gsUse = false;
            vcFpcbParam.gsQuadrangle.gsTcRoi = Rectangle.Truncate(probeRegionInClip);
            
        ///////////////////////////////////////////////////////////////////////////////////////////
        // Connect Parameter
        ///////////////////////////////////////////////////////////////////////////////////////////  
            switch (((RectCheckerParam)param).EdgeType)
            {
                case EdgeType.DarkToLight: vcFpcbParam.gsQuadrangle.gsWtoB = 2; break;
                case EdgeType.LightToDark: vcFpcbParam.gsQuadrangle.gsWtoB = 1; break;
                default:                   vcFpcbParam.gsQuadrangle.gsWtoB = 0;
                    break;
            }

            vcFpcbParam.gsQuadrangle.gsThickW   = ((RectCheckerParam)param).EdgeThickWidth;     // 1
            vcFpcbParam.gsQuadrangle.gsThickH   = ((RectCheckerParam)param).EdgeThickHeight;    // 5
            vcFpcbParam.gsQuadrangle.gsDistance = 5;                        // 5
            vcFpcbParam.gsQuadrangle.gsGv       = ((RectCheckerParam)param).GrayValue;          // 20
            vcFpcbParam.gsQuadrangle.gsScan     = ((RectCheckerParam)param).ProjectionHeight;   // 3
            vcFpcbParam.gsQuadrangle.SetRateIn(((RectCheckerParam)param).PassRate);             // 30
            vcFpcbParam.gsQuadrangle.gsCardinal = ((RectCheckerParam)param).CardinalPoint;      // NE
            vcFpcbParam.gsQuadrangle.gsOtuToIn  = ((RectCheckerParam)param).OutToIn;            // true
            vcFpcbParam.gsQuadrangle.gsRange    = ((RectCheckerParam)param).SearchRange;        // 20
            vcFpcbParam.gsQuadrangle.gsLength   = ((RectCheckerParam)param).SearchLength;       // 70
            vcFpcbParam.gsQuadrangle.gsConvex   = ((RectCheckerParam)param).ConvexShape;        // None
            
        ///////////////////////////////////////////////////////////////////////////////////////////
        // Vision Inspection and Result
        ///////////////////////////////////////////////////////////////////////////////////////////            
            int vResult = vcFpcb.Inspection(clipImage, vcFpcbParam, debugContext, helpDraw);

            int offsetX = (int)clipRegionInFov.X;
            int offsetY = (int)clipRegionInFov.Y;
            double outAngle = vcFpcbParam.gsQuadrangle.gsOutAngle;
            PointF outCenter = new PointF((float)vcFpcbParam.gsQuadrangle.gsOutPt.X + offsetX,
                                          (float)vcFpcbParam.gsQuadrangle.gsOutPt.Y + offsetY);
            
            algorithmResult.Good = (vResult == 1) ? true : false;
            if( algorithmResult.Good == false )
                algorithmResult.MessageBuilder.AddTextLine(vcFpcb.gsError.ToString());
            algorithmResult.ResultValueList.Add(new AlgorithmResultValue("CenterPos", outCenter));
            algorithmResult.ResultValueList.Add(new AlgorithmResultValue("Angle", outAngle));
 
            CopyGraphics(algorithmResult, helpDraw, offsetX, offsetY);            
            helpDraw.Clear();

            return algorithmResult;
        }
    }
}
