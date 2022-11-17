using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.ComponentModel;
using System.Globalization;
using System.Diagnostics;

using DynMvp.Base;
using DynMvp.UI;

namespace DynMvp.Vision
{
    public class CalibrationCheckerConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                            System.Type destinationType)
        {
            if (destinationType == typeof(CalibrationChecker))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                        CultureInfo culture,
                                        object value,
                                        System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                    value is CalibrationChecker)
            {
                return "";
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class CalibrationCheckerParam : AlgorithmParam
    {
        int numRow = 3;
        public int NumRow
        {
            get { return numRow; }
            set { numRow = value; }
        }

        int numCol = 3;
        public int NumCol
        {
            get { return numCol; }
            set { numCol = value; }
        }

        float rowSpace = 1.0f;
        public float RowSpace
        {
            get { return rowSpace; }
            set { rowSpace = value; }
        }

        float colSpace = 1.0f;
        public float ColSpace
        {
            get { return colSpace; }
            set { colSpace = value; }
        }

        public override AlgorithmParam Clone()
        {
            CalibrationCheckerParam param = new CalibrationCheckerParam();

            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            CalibrationCheckerParam param = (CalibrationCheckerParam)srcAlgorithmParam;

            numRow = param.numRow;
            numCol = param.numCol;
            rowSpace = param.rowSpace;
            colSpace = param.colSpace;
        }

        public override void LoadParam(XmlElement paramElement)
        {
            base.LoadParam(paramElement);

            numRow = Convert.ToInt32(XmlHelper.GetValue(paramElement, "NumRow", "3"));
            numCol = Convert.ToInt32(XmlHelper.GetValue(paramElement, "NumCol", "3"));
            rowSpace = Convert.ToSingle(XmlHelper.GetValue(paramElement, "RowSpace", "1.0"));
            colSpace = Convert.ToSingle(XmlHelper.GetValue(paramElement, "ColSpace", "1.0"));
        }

        public override void SaveParam(XmlElement paramElement)
        {
            base.SaveParam(paramElement);

            XmlHelper.SetValue(paramElement, "NumRow", numRow.ToString());
            XmlHelper.SetValue(paramElement, "NumCol", numCol.ToString());
            XmlHelper.SetValue(paramElement, "RowSpace", rowSpace.ToString());
            XmlHelper.SetValue(paramElement, "ColSpace", colSpace.ToString());
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class CalibrationChecker : Algorithm
    {
        public CalibrationChecker()
        {
            param = new CalibrationCheckerParam();
        }

        public override Algorithm Clone()
        {
            CalibrationChecker calibrationChecker = new CalibrationChecker();
            calibrationChecker.CopyFrom(this);

            return calibrationChecker;
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {

        }

        public static string TypeName
        {
            get { return "CalibrationChecker"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Calibration";
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {

        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("Calibration", null));

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect clipRegionInFov = inspectParam.ClipRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;

            AlgorithmResult algorithmResult = CreateAlgorithmResult();
            algorithmResult.ResultRect = probeRegionInFov;

            Calibration calibration = AlgorithmBuilder.CreateCalibration();

            calibration.CalibrationType = CalibrationType.Grid;

            CalibrationCheckerParam calibCheckerParam = (CalibrationCheckerParam)Param;

            algorithmResult.Good = calibration.Calibrate(inspectParam.ClipImage, CalibrationGridType.Dots,
                calibCheckerParam.NumRow, calibCheckerParam.NumCol, calibCheckerParam.RowSpace, calibCheckerParam.ColSpace).IsGood;

            if (algorithmResult.Good == false)
            {
                algorithmResult.MessageBuilder.AddTextLine("Fail to calibration.");
            }
            else
            {
                algorithmResult.ResultValueList.Add(new AlgorithmResultValue("ScaleX", calibration.PelSize.Width));
                algorithmResult.ResultValueList.Add(new AlgorithmResultValue("ScaleY", calibration.PelSize.Height));
                algorithmResult.ResultValueList.Add(new AlgorithmResultValue("Calibration", calibration));
            }

            return algorithmResult;
        }
    }
}
