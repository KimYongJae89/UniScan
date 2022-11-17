using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Devices.Daq;
using DynMvp.InspData;

namespace DynMvp.Data
{
    public enum DaqMeasureType
    {
        Absolute, Difference, Distance
    }

    public enum DaqFilterType
    {
        None, Average, Median
    }

    public enum DaqDataType
    {
        Voltage, Data
    }
    
    public class DaqProbe : Probe
    {
        DaqChannel daqChannel;
        public DaqChannel DaqChannel
        {
            get { return daqChannel; }
            set { daqChannel = value; }
        }

        int numSample = 100;
        public int NumSample
        {
            get { return numSample; }
            set { numSample = value; }
        }

        private float upperValue;
        public float UpperValue
        {
            get { return upperValue; }
            set { upperValue = value; }
        }

        private float lowerValue;
        public float LowerValue
        {
            get { return lowerValue; }
            set { lowerValue = value; }
        }

        private float localScaleFactor = 0;
        public float LocalScaleFactor
        {
            get { return localScaleFactor; }
            set { localScaleFactor = value; }
        }

        private float valueOffset = 0;
        public float ValueOffset
        {
            get { return valueOffset; }
            set { valueOffset = value; }
        }

        private bool useLocalScaleFactor;
        public bool UseLocalScaleFactor
        {
            get { return useLocalScaleFactor; }
            set { useLocalScaleFactor = value; }
        }

        private DaqMeasureType measureType;
        public DaqMeasureType MeasureType
        {
            get { return measureType; }
            set { measureType = value; }
        }

        private DaqDataType dataType;
        public DaqDataType DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        private string target1Name;
        public string Target1Name
        {
            get { return target1Name; }
            set { target1Name = value; }
        }

        private string target2Name;
        public string Target2Name
        {
            get { return target2Name; }
            set { target2Name = value; }
        }

        private DaqFilterType filterType = DaqFilterType.Average;
        public DaqFilterType FilterType
        {
            get { return filterType; }
            set { filterType = value; }
        }

        public override Object Clone()
        {
            DaqProbe daqProbe = new DaqProbe();
            daqProbe.Copy(this);

            return daqProbe;
        }

        public override void Copy(Probe probe)
        {
            base.Copy(probe);

            DaqProbe daqProbe = (DaqProbe)probe;

            numSample = daqProbe.numSample;
            upperValue = daqProbe.upperValue;
            lowerValue = daqProbe.lowerValue;
            useLocalScaleFactor = daqProbe.useLocalScaleFactor;
            localScaleFactor = daqProbe.localScaleFactor;
            valueOffset = daqProbe.ValueOffset;
            measureType = daqProbe.measureType;
            filterType = daqProbe.FilterType;
        }

        public override void OnPreInspection()
        {

        }

        public override void OnPostInspection()
        {

        }

        public override bool IsControllable()
        {
            return true;
        }

        public override List<ProbeResultValue> GetResultValues()
        {
            List<ProbeResultValue> resultValues = new List<ProbeResultValue>();
            resultValues.Add(new ProbeResultValue("Result", true));
            resultValues.Add(new ProbeResultValue("Value", upperValue, lowerValue));

            return resultValues;
        }

        public override ProbeResult Inspect(InspParam inspParam)
        {
            double value = 0;
            switch (measureType)
            {
                case DaqMeasureType.Absolute:
                    if (dataType == DaqDataType.Voltage)
                        value = GetVoltage();
                    else
                        value = GetData();
                    break;
                case DaqMeasureType.Distance:
                    value = GetDistance(inspParam.InspectionResult, inspParam.SensorDistance);
                    break;
                case DaqMeasureType.Difference:
                    value = GetDifference(inspParam.InspectionResult);
                    break;
            }

            bool result = (value >= lowerValue) && (value <= upperValue);

            LogHelper.Debug(LoggerType.Inspection, String.Format("DAQ Probe [{0}] Inspected. Result : {1:0.000}", FullId, value));

            DaqProbeResult daqProbeResult = new DaqProbeResult(this, result);
            daqProbeResult.ResultValueList.Add(new ProbeResultValue("Result", result));
            daqProbeResult.ResultValueList.Add(new ProbeResultValue("Value", upperValue, lowerValue, (float)value));

            return daqProbeResult;
        }

        private double GetDifference(InspectionResult inspectionResult)
        {
            ProbeResult probe1Result = inspectionResult.GetProbeResult(target1Name, 1);
            ProbeResult probe2Result = inspectionResult.GetProbeResult(target2Name, 1);

            double value = 0;
            if (probe1Result is DaqProbeResult && probe2Result is DaqProbeResult)
            {
                ProbeResultValue resultValue1 = probe1Result.GetResultValue("Value");
                ProbeResultValue resultValue2 = probe2Result.GetResultValue("Value");

                value = Math.Abs(Convert.ToSingle(resultValue1.Value) - Convert.ToSingle(resultValue2.Value));
            }

            return value;
        }

        private double GetDistance(InspectionResult inspectionResult, float laserDistance)
        {
            ProbeResult probe1Result = inspectionResult.GetProbeResult(target1Name, 1);
            ProbeResult probe2Result = inspectionResult.GetProbeResult(target2Name, 1);

            double value = 0;
            if (probe1Result is DaqProbeResult && probe2Result is DaqProbeResult)
            {
                ProbeResultValue resultValue1 = probe1Result.GetResultValue("Value");
                ProbeResultValue resultValue2 = probe2Result.GetResultValue("Value");

                value = laserDistance - Convert.ToSingle(resultValue1.Value) - Convert.ToSingle(resultValue2.Value);
            }

            return value;
        }

        private double GetVoltage()
        {
            if (daqChannel == null)
                return 0.0;

            double[] values = daqChannel.ReadVoltage(numSample);

            double averageRaw = 0;
            double average = 0;
            if (values != null && values.Count() > 0)
            {
                if (filterType == DaqFilterType.Average)
                {
                    averageRaw = values.Average();
                }
                else
                {
                    Array.Sort(values);
                    averageRaw = values[values.Length / 2];
                }

                if (daqChannel.ChannelProperty.UseCustomScale)
                {
                    average = (averageRaw - daqChannel.ChannelProperty.ValueOffset) / daqChannel.ChannelProperty.ScaleFactor;
                }

                if (useLocalScaleFactor == true)
                {
                    average = average * localScaleFactor + valueOffset;
                }
            }

            return average;
        }

        private double GetData()
        {
            if (daqChannel == null)
                return 0.0;

            double[] values = daqChannel.ReadData(numSample);

            double averageRaw = 0;
            double average = 0;
            if (values != null && values.Count() > 0)
            {
                if (filterType == DaqFilterType.Average)
                {
                    averageRaw = values.Average();
                }
                else
                {
                    Array.Sort(values);
                    averageRaw = values[values.Length / 2];
                }

                if (daqChannel.ChannelProperty.UseCustomScale)
                {
                    average = (averageRaw - daqChannel.ChannelProperty.ValueOffset) / daqChannel.ChannelProperty.ScaleFactor;
                }

                if (useLocalScaleFactor == true)
                {
                    average = average * localScaleFactor + valueOffset;
                }
            }

            return average;
        }


        public override ProbeResult CreateDefaultResult()
        {
            return new DaqProbeResult(this, false);
        }
    }

    public class DaqProbeResult : ProbeResult
    {
        public DaqProbeResult()
        {

        }

        public DaqProbeResult(Probe probe, bool good)
        {
            if (good == true)
                this.Judgment = Judgment.Accept;
            else
                this.Judgment = Judgment.Reject;

            this.Probe = probe;
        }

        public override void BuildResultMessage(DynMvp.UI.MessageBuilder totalResultMessage)
        {

        }

        public override string ToString()
        {
            if (ResultValueList.Count > 0)
                return String.Format("result value = {0}", ResultValueList[1].Value);

            return "";
        }

    }
}
