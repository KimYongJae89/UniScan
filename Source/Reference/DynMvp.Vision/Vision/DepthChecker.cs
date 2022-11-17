using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Vision
{
    public class DepthCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(DepthChecker))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is BrightnessChecker)
            {
                DepthChecker depthChecker = (DepthChecker)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public enum DepthCheckType
    {
        None, HeightAverage, HeightMax, HeightMin, Volume
    }

    [TypeConverterAttribute(typeof(DepthCheckerConverter))]
    public class DepthCheckerParam : AlgorithmParam
    {
        private DepthCheckType type;
        public DepthCheckType Type
        {
            get { return type; }
            set { type = value; }
        }

        private float lowerValue = 100;
        public float LowerValue
        {
            get { return lowerValue; }
            set { lowerValue = value; }
        }

        private float upperValue = 200;
        public float UpperValue
        {
            get { return upperValue; }
            set { upperValue = value; }
        }
        public override AlgorithmParam Clone()
        {
            DepthCheckerParam param = new DepthCheckerParam();

            param.CopyFrom(this);

            return param;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            type = (DepthCheckType)Enum.Parse(typeof(DepthCheckType), XmlHelper.GetValue(algorithmElement, "DepthCheckType", "HeightAverage"));
            lowerValue = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "LowerValue", "100"));
            upperValue = Convert.ToSingle(XmlHelper.GetValue(algorithmElement, "UpperValue", "200"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "DepthCheckType", type.ToString());
            XmlHelper.SetValue(algorithmElement, "LowerValue", lowerValue.ToString());
            XmlHelper.SetValue(algorithmElement, "UpperValue", upperValue.ToString());
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class DepthChecker : Algorithm
    {
        public DepthChecker()
        {
            param = new DepthCheckerParam();
        }

        public override bool CanProcess3dImage()
        {
            return true;
        }

        public override Algorithm Clone()
        {
            DepthChecker depthChecker = new DepthChecker();
            depthChecker.CopyFrom(this);

            return depthChecker;
        }

        public override void CopyFrom(Algorithm algorithm)
        {
            base.CopyFrom(algorithm);

            DepthChecker depthChecker = (DepthChecker)algorithm;

            param = (DepthCheckerParam)depthChecker.Param.Clone();
        }

        public static string TypeName
        {
            get { return "DepthChecker"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Depth";
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {

        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {

        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            DepthCheckerParam depthCheckerParam = (DepthCheckerParam)param;

            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("Value", depthCheckerParam.UpperValue, depthCheckerParam.LowerValue, 0));
            resultValues.Add(new AlgorithmResultValue("ValueType", depthCheckerParam.Type.ToString()));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.ClipImage, ImageType.Depth);
            Filter(clipImage);

            DepthCheckerParam depthCheckerParam = (DepthCheckerParam)param;

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(clipImage);
            float resultValue = 0;

            switch(depthCheckerParam.Type)
            {
                case DepthCheckType.None:
                    break;
                default:
                case DepthCheckType.Volume:
                case DepthCheckType.HeightAverage:
                    if (probeRegionInFov.Angle != 0)
                    {
                        ImageD rotatedMask = ImageHelper.GetRotateMask(clipImage.Width, clipImage.Height, probeRegionInFov);
                        AlgoImage rotatedAlgoMask = ImageBuilder.Build(BinaryCounter.TypeName, rotatedMask, ImageType.Grey, ImageBandType.Luminance);
                        resultValue = imageProcessing.GetGreyAverage(clipImage, rotatedAlgoMask);
                    }
                    else
                    {
                        resultValue = imageProcessing.GetGreyAverage(clipImage);
                    }

                    if (depthCheckerParam.Type == DepthCheckType.Volume)
                    {
                        resultValue = resultValue * (inspectParam.PixelRes3D * clipImage.Width) * (inspectParam.PixelRes3D * clipImage.Height);
                    }
                    break;
                case DepthCheckType.HeightMax:
                    resultValue = ((Image3D)inspectParam.ClipImage).Data.Max();
                    break;
                case DepthCheckType.HeightMin:
                    resultValue = ((Image3D)inspectParam.ClipImage).Data.Min();
                    break;
            }

            AlgorithmResult algorithmResult = CreateAlgorithmResult();
            algorithmResult.ResultValueList.Add(new AlgorithmResultValue("ValueType", depthCheckerParam.Type.ToString()));

            if (depthCheckerParam.Type == DepthCheckType.None)
            {
                algorithmResult.Good = false;
                algorithmResult.ResultValueList.Add(new AlgorithmResultValue("Value", depthCheckerParam.UpperValue, depthCheckerParam.LowerValue, 0));
                return algorithmResult;
            }

            algorithmResult.ResultRect = probeRegionInFov;
            algorithmResult.Good = (resultValue >= depthCheckerParam.LowerValue) && (resultValue <= depthCheckerParam.UpperValue);

            algorithmResult.ResultValueList.Add(new AlgorithmResultValue("Value", depthCheckerParam.UpperValue, depthCheckerParam.LowerValue, resultValue));

            return algorithmResult;
        }
    }
}
