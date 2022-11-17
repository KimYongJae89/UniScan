using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Drawing;
using System.Threading;
using System.IO;
    
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.InspData;

namespace DynMvp.Data
{
    public enum TensionUnitType
    {
        Newton, mm
    }

    public class TensionSerialProbe : SerialProbe
    {
        ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        TensionReceivedPacket receivedPacket;

        private string tensionFilePath;
        public string TensionFilePath
        {
            get { return tensionFilePath; }
            set { tensionFilePath = value; }
        }
        Dictionary<float, int> tensionMap = new Dictionary<float, int>();
        public Dictionary<float, int> TensionMap
        {
            get { return tensionMap; }
            set { tensionMap = value; }
        }

        private TensionUnitType unitType = TensionUnitType.mm;
        public TensionUnitType UnitType
        {
            get { return unitType; }
            set { unitType = value; }
        }

        public override Object Clone()
        {
            TensionSerialProbe tensionSerialProbe = new TensionSerialProbe();
            tensionSerialProbe.Copy(this);

            return tensionSerialProbe;
        }

        public override void Copy(Probe probe)
        {
            base.Copy(probe);
            tensionFilePath = ((TensionSerialProbe)probe).tensionFilePath;
            tensionMap = ((TensionSerialProbe)probe).TensionMap;
            unitType = ((TensionSerialProbe)probe).UnitType;
        }

        public override void OnPreInspection()
        {
        }

        public override void OnPostInspection()
        {
            if (InspectionSerialPort == null)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Serial Port is not assigned. {0}", FullId));
                return;
            }
        }

        public override bool IsControllable()
        {
            return true;
        }

        public override List<ProbeResultValue> GetResultValues()
        {
            List<ProbeResultValue> resultValues = new List<ProbeResultValue>();
            resultValues = base.GetResultValues();
            return resultValues;
        }

        public override ProbeResult Inspect(InspParam inspParam)
        {
            if (InspectionSerialPort == null)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Serial Port is not assigned. {0}", FullId));
                return null;
            }
            InitTesnionMap();

            TensionSerialProbeResult serialProbeResult = new TensionSerialProbeResult(this, false);

            List<float> valueList = new List<float>();

            float maxValue = float.MinValue, minValue = float.MaxValue;

            //for (int i = 0; i < Configuration.NumSerialReading; i++)
            for (int i = 0; i < NumSerialReading; i++)
            {
                Thread.Sleep(500);
                float value = GetValue();
//                if (value > 0)
                {
                    valueList.Add(value);
                    if (value > maxValue) maxValue = value;
                    if (value < minValue) minValue = value;
                }
            }

            float resultValue = 0;

            if (valueList.Count > 0)
            {
                resultValue = valueList.Average();
            }

            float nResultValue = resultValue * (-1);
            int nNewtonValue = GetTensionDataConvertedLengthToNewton(nResultValue);

            bool result;
            if (unitType == TensionUnitType.mm)
            {
                result = (nResultValue >= LowerValue && nResultValue <= UpperValue);

                serialProbeResult.ResultValueList.Add(new ProbeResultValue("ValueType", "", "mm", true));
                serialProbeResult.ResultValueList.Add(new ProbeResultValue("Value", UpperValue, LowerValue, nResultValue, result));
                serialProbeResult.ShortResultMessage = String.Format("{0} : Value = {1}", result ? "Good" : "NG", nResultValue);
            }
            else
            {
                result = (nNewtonValue >= LowerValue && nNewtonValue <= UpperValue);

                serialProbeResult.ResultValueList.Add(new ProbeResultValue("ValueType", "", "Newton", true));
                serialProbeResult.ResultValueList.Add(new ProbeResultValue("Value", UpperValue, LowerValue, nNewtonValue, result));
                serialProbeResult.ShortResultMessage = String.Format("{0} : Value = {1}", result ? "Good" : "NG", nNewtonValue);
            }

            if (result == true)
                serialProbeResult.Judgment = Judgment.Accept;
            else
                serialProbeResult.Judgment = Judgment.Reject;

            serialProbeResult.ResultValueList.Add(new ProbeResultValue("Result", result));
            serialProbeResult.UnitType = unitType;

            return serialProbeResult;
        }

        public void DataReceived(ReceivedPacket receivedPacket, string errCode = null)
        {
            this.receivedPacket = (TensionReceivedPacket)receivedPacket;
            manualResetEvent.Set();
        }

        private float GetValue()
        {
            LogHelper.Debug(LoggerType.Inspection, "GetValue");
            
            PacketParser packetParser = InspectionSerialPort.PacketHandler.PacketParser;
            if (packetParser == null)
            {
                LogHelper.Error(LoggerType.Error, "Packet Parser is null");
                return 0;
            }
            packetParser.DataReceived += DataReceived;

            manualResetEvent.Reset();

            InspectionSerialPort.SendRequest();

            float resultValue = 0;
            
            
            if (manualResetEvent.WaitOne(5000) == true)
            {
                LogHelper.Debug(LoggerType.Inspection, "Packet Received");

                try
                {
                    resultValue = -Convert.ToSingle(receivedPacket.ResultValue);
                }
                catch ( InvalidCastException )
                {
                    LogHelper.Warn(LoggerType.Inspection, "Invalid Packet");
                    resultValue = 0;
                }
            }
            else
            {
                LogHelper.Warn(LoggerType.Inspection, "Packet loss");
            }

            packetParser.DataReceived -= DataReceived;

            return resultValue;
        }

        public override ProbeResult CreateDefaultResult()
        {
            return new TensionSerialProbeResult(this, false);
        }

        private void InitTesnionMap()
        {
            if (tensionMap.Count > 0)
                return;

            if (string.IsNullOrEmpty(tensionFilePath))
            {
                LogHelper.Debug(LoggerType.Inspection, "Can't find tension file.");
                return;
            }

            StreamReader reader = new StreamReader(File.OpenRead(tensionFilePath));

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');

                if (!String.IsNullOrEmpty(values[0]))
                {
                    float id = Convert.ToSingle(values[0]);

                    if (!String.IsNullOrEmpty(values[1]))
                    {
                        tensionMap.Add(id, Convert.ToInt32(values[1]));
                    }
                }
            }
        }

        private int GetTensionDataConvertedLengthToNewton(float resultData)
        {
            if (tensionMap == null)
                return 0;
            int result = 0;
            resultData = (float)Math.Round(resultData, 2);
            tensionMap.TryGetValue(resultData, out result);
            return result;
        }
    }

    public class TensionSerialProbeResult : ProbeResult
    {
        TensionUnitType unitType = TensionUnitType.mm;
        public TensionUnitType UnitType
        {
            get { return unitType; }
            set { unitType = value; }
        }

        public TensionSerialProbeResult()
        {

        }

        public TensionSerialProbeResult(Probe probe, bool good)
        {
            if (good == true)
                this.Judgment = Judgment.Accept;
            else
                this.Judgment = Judgment.Reject;

            this.Probe = probe;
        }

        public override void BuildResultMessage(MessageBuilder totalResultMessage)
        {
            resultMessage = new MessageBuilder();

            resultMessage.AddTextLine("---- Tension Check ----");

            resultMessage.BeginTable(null, "Item", "Value", "Good Range");

            foreach (ProbeResultValue resultValue in ResultValueList)
            {
                Color resultColor = (resultValue.Good ? Color.Transparent : Color.LightPink);

                if (MathHelper.IsNumeric(resultValue.Value) == true)
                    resultMessage.AddTableRow(resultColor, resultValue.Name, resultValue.Value.ToString(), String.Format("{0} ~ {1}", resultValue.Lcl, resultValue.Ucl));
                else
                    resultMessage.AddTableRow(resultColor, resultValue.Name, resultValue.Value.ToString());
            }

            resultMessage.EndTable();

            totalResultMessage.Append(resultMessage);
        }

        public override string ToString()
        {
            if (ResultValueList.Count > 0)
            {
                if (unitType == TensionUnitType.mm)
                    return String.Format("Result Value = {0} \r\nLength value = {1} ({2} ~ {3})", ResultValueList[1].Value, ResultValueList[1].Value, ResultValueList[1].Lcl, ResultValueList[1].Ucl);
                else
                    return String.Format("Result Value = {0} \r\nNewTon value = {1} ({2} ~ {3})", ResultValueList[1].Value, ResultValueList[1].Value, ResultValueList[1].Lcl, ResultValueList[1].Ucl);
            }
            else
                return String.Format("result value = Empty");
        }
    }
}
