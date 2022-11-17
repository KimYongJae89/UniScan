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
    public class BrightnessCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(BrightnessChecker))
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
                BrightnessChecker brightnessChecker = (BrightnessChecker)value;

                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }


    [TypeConverterAttribute(typeof(BrightnessCheckerConverter))]
    public class BrightnessCheckerParam : AlgorithmParam
    {
        private int lowerValue = 100;
        public int LowerValue
        {
            get { return lowerValue; }
            set { lowerValue = value; }
        }

        private int upperValue = 200;
        public int UpperValue
        {
            get { return upperValue; }
            set { upperValue = value; }
        }

        public override AlgorithmParam Clone()
        {
            BrightnessCheckerParam param = new BrightnessCheckerParam();

            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            BrightnessCheckerParam param = (BrightnessCheckerParam)srcAlgorithmParam;

            lowerValue = param.lowerValue;
            upperValue = param.upperValue;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            lowerValue = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "LowerValue", "100"));
            upperValue = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "UpperValue", "200"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "LowerValue", lowerValue.ToString());
            XmlHelper.SetValue(algorithmElement, "UpperValue", upperValue.ToString());
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    [TypeConverterAttribute(typeof(BrightnessCheckerConverter))]
    public class BrightnessChecker : Algorithm
    {
        public BrightnessChecker()
        {
            param = new BrightnessCheckerParam();
        }

        public override bool CanProcess3dImage()
        {
            return false;
        }

        public override Algorithm Clone()
        {
            BrightnessChecker brightnessChecker = new BrightnessChecker();
            brightnessChecker.CopyFrom(this);

            return brightnessChecker;
        }

        public static string TypeName
        {
            get { return "BrightnessChecker"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Bright";
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {

        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {

        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("Brightness", ((BrightnessCheckerParam)param).UpperValue, ((BrightnessCheckerParam)param).LowerValue, 0));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            ImageType imageType = (inspectParam.ClipImage is Image3D ? ImageType.Depth : ImageType.Grey);

            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.ClipImage, imageType, param.ImageBand);
            Filter(clipImage);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(clipImage);

            float average = imageProcessing.GetGreyAverage(clipImage);

            AlgorithmResult brightnessCheckerResult = CreateAlgorithmResult();
            brightnessCheckerResult.ResultRect = probeRegionInFov;
            brightnessCheckerResult.Good = (average >= ((BrightnessCheckerParam)param).LowerValue) && (average <= ((BrightnessCheckerParam)param).UpperValue);

            brightnessCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Brightness", ((BrightnessCheckerParam)param).UpperValue, ((BrightnessCheckerParam)param).LowerValue, average));

            return brightnessCheckerResult;
        }
    }
}
