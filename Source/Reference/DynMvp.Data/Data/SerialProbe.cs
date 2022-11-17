using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Drawing;
using System.Threading;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.InspData;
using System.IO;
namespace DynMvp.Data
{
    public class SerialProbe : Probe
    {
        private string portName;
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }

        SerialPortEx inspectionSerialPort;
        public SerialPortEx InspectionSerialPort
        {
            get { return inspectionSerialPort; }
            set { inspectionSerialPort = value; }
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
        private int numSerialReading = 1;
        public int NumSerialReading
        {
            get { return numSerialReading; }
            set { numSerialReading = value; }
        }

       
        ManualResetEvent manualResetEvent = new ManualResetEvent(false);

        //ReceivedPacket receivedPacket = new TensionReceivedPacket();
        //CasLoadCellReceivedPacket receivedPacketd;
        TensionReceivedPacket tensionReceivedPacket;

        public override Object Clone()
        {
            SerialProbe serialProbe = new SerialProbe();
            serialProbe.Copy(this);

            return serialProbe;
        }

        public override void Copy(Probe probe)
        {
            base.Copy(probe);

            SerialProbe serialProbe = (SerialProbe)probe;

            portName = serialProbe.portName;
            inspectionSerialPort = serialProbe.inspectionSerialPort;
            upperValue = serialProbe.upperValue;
            lowerValue = serialProbe.lowerValue;
            numSerialReading = serialProbe.numSerialReading;
            
        }

        public override void OnPreInspection()
        {
        }

        public override void OnPostInspection()
        {
            if (inspectionSerialPort == null)
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
            resultValues.Add(new ProbeResultValue("Result", true));
            resultValues.Add(new ProbeResultValue("Value", upperValue, lowerValue));

            return resultValues;
        }

        public override ProbeResult Inspect(InspParam inspParam)
        {
            if (inspectionSerialPort == null)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Serial Port is not assigned. {0}", FullId));
                return null;
            }

            SerialProbeResult serialProbeResult = new SerialProbeResult(this, false);

            List<float> valueList = new List<float>();

            float maxValue = float.MinValue, minValue = float.MaxValue;

            for (int i = 0; i < numSerialReading; i++)
            {
                float value = GetValue();
                if (value > 0)
                {
                    valueList.Add(value);
                    if (value > maxValue) maxValue = value;
                    if (value < minValue) minValue = value;
                }
            }

            float resultValue = 0;

            if (valueList.Count > 0)
            {
                if (valueList.Count >= 5)
                {
                    resultValue = (valueList.Sum() - minValue - maxValue) / (valueList.Count - 2);
                }
                else
                {
                    resultValue = valueList.Average();
                }
            }

            bool result = (resultValue >= lowerValue && resultValue <= upperValue);

            if (result == true)
                serialProbeResult.Judgment = Judgment.Accept;
            else
                serialProbeResult.Judgment = Judgment.Reject;

            serialProbeResult.ResultValueList.Add(new ProbeResultValue("Result", result));
            serialProbeResult.ResultValueList.Add(new ProbeResultValue("Value", upperValue, lowerValue, resultValue));

            return serialProbeResult;
        }

        public void DataReceived(ReceivedPacket receivedPacket)
        {
            tensionReceivedPacket = (TensionReceivedPacket)receivedPacket;
            //if (ProbeType == ProbeType.Serial)
            //    this.receivedPacket = (CasLoadCellReceivedPacket)receivedPacket;
            //else
            //    this.receivedPacket = (TensionReceivedPacket)receivedPacket;
            manualResetEvent.Set();
        }

        private float GetValue()
        {
            LogHelper.Debug(LoggerType.Inspection, "GetValue");

            PacketParser packetParser = inspectionSerialPort.PacketHandler.PacketParser;
            if (packetParser == null)
            {
                LogHelper.Error(LoggerType.Error, "Packet Parser is null");
                return 0;
            }
            packetParser.DataReceived += DataReceived;

            manualResetEvent.Reset();

            inspectionSerialPort.SendRequest();

            float resultValue = 0;
            string resultString = "";

            if (manualResetEvent.WaitOne(5000) == true)
            {
                LogHelper.Debug(LoggerType.Inspection, "Packet Received");

                try
                {
                    resultString = Encoding.Default.GetString( tensionReceivedPacket.ReceivedData);
                }
                catch (InvalidCastException)
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
            return new SerialProbeResult(this, false);
        }

        
    }

    public class SerialProbeResult : ProbeResult
    {
        public SerialProbeResult()
        {

        }

        public SerialProbeResult(Probe probe, bool good)
        {
            if (good == true)
                this.Judgment = Judgment.Accept;
            else
                this.Judgment = Judgment.Reject;

            this.Probe = probe;
        }

        public override void BuildResultMessage(MessageBuilder totalResultMessage)
        {

        }

        public override string ToString()
        {
            return String.Format("result value = {0}", ResultValueList[1].Value);
        }
    }
}
