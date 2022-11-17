using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;
using DynMvp.Devices.Comm;
using System.Drawing;
using DynMvp.Data.Forms;
using System.Threading;
using DynMvp.Vision;
using System.IO;
using DynMvp.InspData;
using DynMvp.Data.UI;

namespace DynMvp.Data
{
    public class ValueData
    {
        string objectName;
        public string ObjectName
        {
            get { return objectName; }
            set { objectName = value; }
        }

        string valueName;
        public string ValueName
        {
            get { return valueName; }
            set { valueName = value; }
        }

        public ValueData()
        {
        }

        public ValueData(string objectName, string valueName)
        {
            this.objectName = objectName;
            this.valueName = valueName;
        }

        public void Load(XmlElement valueDataElement)
        {
            objectName = XmlHelper.GetValue(valueDataElement, "ObjectName", "");
            valueName = XmlHelper.GetValue(valueDataElement, "ValueName", "");
        }

        public void PacketLoad(XmlElement valueDataElement)
        {
            objectName = XmlHelper.GetValue(valueDataElement, "ObjectName", "");
            valueName = XmlHelper.GetValue(valueDataElement, "ValueName", "");
        }

        public void Save(XmlElement valueDataElement)
        {
            XmlHelper.SetValue(valueDataElement, "ObjectName", objectName.ToString());
            XmlHelper.SetValue(valueDataElement, "ValueName", valueName.ToString());
        }

        internal ValueData Clone()
        {
            ValueData valueData = new ValueData();
            valueData.objectName = this.objectName;

            valueData.valueName = this.valueName;

            return valueData;
        }
    }

    public enum DelimiterType
    {
        Ascii, Hex
    }

    public class ExportPacketFormat
    {
        DelimiterType packetStartType = DelimiterType.Ascii;
        public DelimiterType PacketStartType
        {
            get { return packetStartType; }
            set { packetStartType = value; }
        }

        string packetStart = "<START>";
        public string PacketStart
        {
            get { return packetStart; }
            set { packetStart = value; }
        }

        DelimiterType packetEndType = DelimiterType.Ascii;
        public DelimiterType PacketEndType
        {
            get { return packetEndType; }
            set { packetEndType = value; }
        }

        string packetEnd = "<END>";
        public string PacketEnd
        {
            get { return packetEnd; }
            set { packetEnd = value; }
        }

        bool useCheckSum;
        public bool UseCheckSum
        {
            get { return useCheckSum; }
            set { useCheckSum = value; }
        }

        int checksumSize;
        public int ChecksumSize
        {
            get { return checksumSize; }
            set { checksumSize = value; }
        }

        DelimiterType separatorType = DelimiterType.Ascii;
        public DelimiterType SeparatorType
        {
            get { return separatorType; }
            set { separatorType = value; }
        }

        string separator = ",";
        public string Separator
        {
            get { return separator; }
            set { separator = value; }
        }

        List<ValueData> valueDataList = new List<ValueData>();
        public List<ValueData> ValueDataList
        {
            get { return valueDataList; }
            set { valueDataList = value; }
        }

        internal ExportPacketFormat Clone()
        {
            ExportPacketFormat exportPacketFormat = new ExportPacketFormat();

            exportPacketFormat.packetStartType = this.packetEndType;
            exportPacketFormat.packetStart = this.packetStart;
            exportPacketFormat.packetEndType = this.packetEndType;
            exportPacketFormat.packetEnd = this.packetEnd;
            exportPacketFormat.useCheckSum = this.useCheckSum;
            exportPacketFormat.checksumSize = this.checksumSize;
            exportPacketFormat.separatorType = this.separatorType;
            exportPacketFormat.separator = this.separator;

            foreach (ValueData f in this.valueDataList)
                exportPacketFormat.valueDataList.Add(f.Clone());

            return exportPacketFormat;
        }

        private string GetPacketStartString()
        {
            if (packetStartType == DelimiterType.Hex)
                return StringHelper.HexToString(packetStart);
            else
                return packetStart;
        }

        private string GetPacketEndString()
        {
            if (packetEndType == DelimiterType.Hex)
                return StringHelper.HexToString(packetEnd);
            else
                return packetEnd;
        }

        private char GetSeparatorChar()
        {
            if (separatorType == DelimiterType.Hex)
                return StringHelper.HexToString(separator)[0];
            else
                return separator[0];
        }

        public byte[] GetStartChar()
        {
            return Encoding.ASCII.GetBytes(GetPacketStartString());
        }

        public byte[] GetEndChar()
        {
            return Encoding.ASCII.GetBytes(GetPacketEndString());
        }

        public string GetPacketTitle()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetPacketStartString());

            char separatorCh = GetSeparatorChar();

            foreach (ValueData valueData in valueDataList)
            {
                sb.Append(valueData.ObjectName);
                sb.Append(separatorCh);
            }

            return sb.ToString().TrimEnd(separatorCh) + GetPacketEndString();
        }

        string GetValueString(object value)
        {
            string valueStr = "";
            if (value is float)
            {
                valueStr = ((float)value).ToString("+000.000;-000.000;+000.000");
            }
            else if (value is int)
            {
                valueStr = ((int)value).ToString();
            }
            else if (value is bool)
            {
                valueStr = ((bool)value).ToString();
            }
            else if (value is Point)
            {
                Point pt = (Point)value;
                valueStr = String.Format("{0},{1}", pt.X, pt.Y);;
            }
            else if (value is PointF)
            {
                PointF pt = (PointF)value;
                valueStr = String.Format("{0},{1}", pt.X, pt.Y); ;
            }
            else if (value is ResultValueItem)
            {
                ResultValueItem resultValueItem = (ResultValueItem)value;
                valueStr = resultValueItem.GetValueString();
            }

            return valueStr;
        }

        public string GetPacket(InspectionResult inspectionResult)
        {
            StringBuilder sb = new StringBuilder();

            char separatorCh = GetSeparatorChar();

            foreach (ValueData valueData in valueDataList)
            {
                InspectionResult targetInspectionResult = inspectionResult.GetTargetResult(valueData.ObjectName);
                if (targetInspectionResult.Count() > 0)
                {
                    sb.Append(targetInspectionResult.Judgment.ToString());
                    sb.Append(separatorCh);
                    continue;
                }

                ProbeResult probeResult = inspectionResult.GetProbeResult(valueData.ObjectName);
                if (probeResult != null)
                {
                    ProbeResultValue probeResultValue = probeResult.GetResultValue(valueData.ValueName);
                    if (probeResultValue != null)
                    {
                        sb.Append(GetValueString(probeResultValue.Value));
                        sb.Append(separatorCh);
                    }
                }

                if (valueData.ObjectName == "Model")
                {
                    object value = inspectionResult.GetExtraResult(valueData.ValueName);
                    if (value != null)
                    {
                        sb.Append(GetValueString(value));
                        sb.Append(separatorCh);
                    }
                }
            }

//            string packetData = "DONE,0," + (inspectionResult.Judgment == Judgment.Accept ? "OK," : "NG,") + sb.ToString().TrimEnd(separatorCh);
            string packetData = sb.ToString().TrimEnd(separatorCh);

            string checksum = StringHelper.GetChecksum(packetData, checksumSize);

            return GetPacketStartString() + packetData + GetPacketEndString();
        }

        public void Load(XmlElement modelDescElement)
        {
            packetStartType = (DelimiterType)Enum.Parse(typeof(DelimiterType), XmlHelper.GetValue(modelDescElement, "PacketStartType", "Ascii"));
            packetStart = XmlHelper.GetValue(modelDescElement, "PacketStart", "");
            packetEndType = (DelimiterType)Enum.Parse(typeof(DelimiterType), XmlHelper.GetValue(modelDescElement, "PacketEndType", "Ascii"));
            packetEnd = XmlHelper.GetValue(modelDescElement, "PacketEnd", "");
            separatorType = (DelimiterType)Enum.Parse(typeof(DelimiterType), XmlHelper.GetValue(modelDescElement, "SeparatorType", "Ascii"));
            separator = XmlHelper.GetValue(modelDescElement, "Separator", ",");
            useCheckSum = Convert.ToBoolean(XmlHelper.GetValue(modelDescElement, "UseChecksum", "False"));
            checksumSize = Convert.ToInt32(XmlHelper.GetValue(modelDescElement, "ChecksumSize", "2"));

            XmlElement valueDataListElement = modelDescElement["ValueDataList"];
            if (valueDataListElement != null)
            {
                valueDataList.Clear();
                foreach (XmlElement valueDataElement in valueDataListElement)
                {
                    if (valueDataElement.Name == "ValueData")
                    {
                        ValueData valueData = new ValueData();
                        valueData.Load(valueDataElement);

                        valueDataList.Add(valueData);
                    }
                }
            }
        }

        public void Save(XmlElement modelDescElement)
        {
            XmlHelper.SetValue(modelDescElement, "PacketStartType", packetStartType.ToString());
            XmlHelper.SetValue(modelDescElement, "PacketStart", packetStart);
            XmlHelper.SetValue(modelDescElement, "PacketEndType", packetEndType.ToString());
            XmlHelper.SetValue(modelDescElement, "PacketEnd", packetEnd);
            XmlHelper.SetValue(modelDescElement, "SeparatorType", separatorType.ToString());
            XmlHelper.SetValue(modelDescElement, "Separator", separator.ToString());
            XmlHelper.SetValue(modelDescElement, "UseChecksum", useCheckSum.ToString());
            XmlHelper.SetValue(modelDescElement, "ChecksumSize", checksumSize.ToString());

            XmlElement valueDataListElement = modelDescElement.OwnerDocument.CreateElement("", "ValueDataList", "");
            modelDescElement.AppendChild(valueDataListElement);

            foreach (ValueData valueData in valueDataList)
            {
                XmlElement valueDataElement = modelDescElement.OwnerDocument.CreateElement("", "ValueData", "");
                valueDataListElement.AppendChild(valueDataElement);

                valueData.Save(valueDataElement);
            }
        }
    }

    public abstract class DataExporter
    {
        protected AlignDataInterfaceInfo alignDataInterfaceInfo;
        public AlignDataInterfaceInfo AlignDataInterfaceInfo
        {
            get { return alignDataInterfaceInfo; }
            set { alignDataInterfaceInfo = value; }
        }

        protected ExportPacketFormat exportPacketFormat;
        public ExportPacketFormat ExportPacketFormat
        {
            get { return exportPacketFormat;  }
            set { exportPacketFormat = value; }
        }

        public void Export(InspectionResult inspectionResult)
        {
            Export(inspectionResult, new CancellationToken(false));
        }

        public abstract void Export(InspectionResult inspectionResult, CancellationToken cancellationToken);
    }

    public class TextProductOverviewDataExport : DataExporter
    {
        string resultPath;
        object fileLock = new object();

        public TextProductOverviewDataExport(string resultPath)
        {
            this.resultPath = resultPath;
        }

        public override void Export(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            string shortTime = inspectionResult.InspectionStartTime.ToString("yyyyMMdd");
            string resultFile = String.Format("{0}\\{1}_{2}.csv", resultPath, shortTime, inspectionResult.ModelName);

            lock (fileLock)
            {
                FileStream fs = new FileStream(resultFile, FileMode.OpenOrCreate);

                if (fs != null)
                {
                    fs.Seek(0, SeekOrigin.End);

                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);

                    string resultStr = String.Format("{0},{1}", inspectionResult.InspectionNo, inspectionResult.Judgment.ToString());
                    sw.WriteLine(resultStr);

                    sw.Close();
                    fs.Close();
                }
            }
        }
    }

    public class TextProductResultDataExport : DataExporter
    {
        private TextInspResultArchiver dataTextResult = new TextInspResultArchiver();
        internal TextInspResultArchiver DataTextResult
        {
            get { return DataTextResult; }
        }

        public override void Export(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            LogHelper.Debug(LoggerType.Inspection, "SaveResult");

            dataTextResult.Save(inspectionResult, cancellationToken);
        }
    }

    public class SerialDataExporter : DataExporter
    {
        SerialPortEx serialPortEx;

        public SerialDataExporter(SerialPortEx serialPortEx)
        {
            this.serialPortEx = serialPortEx;
        }

        public override void Export(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            if (exportPacketFormat != null)
                serialPortEx.WritePacket(exportPacketFormat.GetPacket(inspectionResult));
        }
    }

    public class TcpIpServerDataExporter : DataExporter
    {
        SimpleServerSocket simpleServerSocket;

        public TcpIpServerDataExporter(SimpleServerSocket simpleServerSocket)
        {
            this.simpleServerSocket = simpleServerSocket;
        }

        public override void Export(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            //if (exportPacketFormat != null)
            //    simpleServerSocket.SendCommand(new SimplePacketParser(exportPacketFormat.GetPacket(inspectionResult))); // server Socket
        }
    }

    public class TcpIpClientDataExporter : DataExporter
    {
        SinglePortSocket singlePortSocket;

        public TcpIpClientDataExporter(SinglePortSocket singlePortSocket)
        {
            this.singlePortSocket = singlePortSocket;
        }

        public override void Export(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            //if (exportPacketFormat != null)
            //    singlePortSocket.SendCommand(new SimplePacketParser(exportPacketFormat.GetPacket(inspectionResult)));
        }

        //public override bool UpdateResult(InspectionResult inspectionResult, PacketParser packetParser)
        //{
        //    LogHelper.Debug(LoggerType.Inspection, "Begin Update Result");

        //    if (exportPacketFormat != null)
        //    {
        //        LogHelper.Debug(LoggerType.Inspection, "Send Result");

        //        SimplePacketParser simplePacketParser = (SimplePacketParser)singlePortSocket.PacketHandler.PacketParser;
        //        simplePacketParser.StartChar = exportPacketFormat.GetStartChar();
        //        simplePacketParser.EndChar = exportPacketFormat.GetEndChar();
        //        singlePortSocket.SendCommand(packetParser);

        //        LogHelper.Debug(LoggerType.Inspection, "Wait Response");

        //        if (packetParser.WaitResponse(10000) == false)
        //        {
        //            if (singlePortSocket.PacketData.DataByteFull != null)
        //            {
        //                if (singlePortSocket.PacketData.DataByteFull.Count() > 0)
        //                {
        //                    string fullString = System.Text.Encoding.Default.GetString(singlePortSocket.PacketData.DataByteFull.ToArray());
        //                    int strLength = fullString.Length;
        //                    if (strLength > 100)
        //                        strLength = 100;
        //                    LogHelper.Debug(LoggerType.Inspection, String.Format("Communication Buffer : {0} ... ", fullString.Substring(0, strLength)));
        //                }
        //            }
        //            else
        //            {
        //                LogHelper.Debug(LoggerType.Inspection, "Communication Buffer : null");
        //            }
        //            singlePortSocket.PacketHandler.ClearParser();
        //            return false;
        //        }

        //        LogHelper.Debug(LoggerType.Inspection, "End Send Result");
        //    }

        //    singlePortSocket.PacketHandler.ClearParser();

        //    LogHelper.Debug(LoggerType.Inspection, "End Update Result");

        //    return true;
        //}
    }

    public class FinsDataExporter : DataExporter
    {
        FinsMonitor finsMonitor;
        int numInspectionStep;

        public FinsDataExporter(FinsMonitor finsMonitor, int numInspectionStep)
        {
            this.finsMonitor = finsMonitor;
            this.numInspectionStep = numInspectionStep;
        }

        public override void Export(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            string totalGoodResult = "";
            string totalNgResult = "";

            for (int i = 2; i < numInspectionStep; i++)
            {
                InspectionResult targetGroupResult = new InspectionResult();
                inspectionResult.GetTargetGroupResult(i.ToString("00"), 0, targetGroupResult, true);

                if (targetGroupResult.Count() > 0)
                {
                    if (targetGroupResult.Judgment == Judgment.Accept)
                    {
                        totalGoodResult += '1'; //  (targetGroupResult.Judgment == Judgment.Accept ? "1" : "0");
                        totalNgResult += '0'; //  (targetGroupResult.Judgment == Judgment.Accept ? "1" : "0");
                    }
                    else
                    {
                        totalGoodResult += '0'; //  (targetGroupResult.Judgment == Judgment.Accept ? "1" : "0");
                        totalNgResult += '1'; //  (targetGroupResult.Judgment == Judgment.Accept ? "1" : "0");
                    }
                }
                else
                {
                    totalGoodResult += '0'; //  (targetGroupResult.Judgment == Judgment.Accept ? "1" : "0");
                    totalNgResult += '0'; //  (targetGroupResult.Judgment == Judgment.Accept ? "1" : "0");
                }
            }

            totalGoodResult += new string('0', 160 - totalGoodResult.Length);
            totalNgResult += new string('0', 160 - totalNgResult.Length);

            byte[] byteArray = StringHelper.BinaryStringToByteArray(totalGoodResult + totalNgResult, true);
            string hexString = StringHelper.ByteArrayToHexString(byteArray);
            hexString = StringHelper.SwapWordHex(hexString);

            finsMonitor.TotalResult = hexString;
        }
    }

    public class AlignDataInterfaceInfo
    {
        int offsetXAddress1;
        public int OffsetXAddress1
        {
            get { return offsetXAddress1; }
            set { offsetXAddress1 = value; }
        }

        int offsetYAddress1;
        public int OffsetYAddress1
        {
            get { return offsetYAddress1; }
            set { offsetYAddress1 = value; }
        }

        int angleAddress;
        public int AngleAddress
        {
            get { return angleAddress; }
            set { angleAddress = value; }
        }

        int offsetXAddress2;
        public int OffsetXAddress2
        {
            get { return offsetXAddress2; }
            set { offsetXAddress2 = value; }
        }

        int offsetYAddress2;
        public int OffsetYAddress2
        {
            get { return offsetYAddress2; }
            set { offsetYAddress2 = value; }
        }

        float xAxisCalibration = 1.0f;
        public float XAxisCalibration
        {
            get { return xAxisCalibration; }
            set { xAxisCalibration = value; }
        }

        float yAxisCalibration = 1.0f;
        public float YAxisCalibration
        {
            get { return yAxisCalibration; }
            set { yAxisCalibration = value; }
        }

        float rAxisCalibration = 1.0f;
        public float RAxisCalibration
        {
            get { return rAxisCalibration; }
            set { rAxisCalibration = value; }
        }

        public void Save(XmlElement configElement, string sectionName)
        {
            XmlElement modbusAlignerElement = configElement.OwnerDocument.CreateElement("", sectionName, "");
            configElement.AppendChild(modbusAlignerElement);

            XmlHelper.SetValue(modbusAlignerElement, "OffsetXAddress1", offsetXAddress1.ToString());
            XmlHelper.SetValue(modbusAlignerElement, "OffsetYAddress1", offsetYAddress1.ToString());
            XmlHelper.SetValue(modbusAlignerElement, "AngleAddress", angleAddress.ToString());
            XmlHelper.SetValue(modbusAlignerElement, "OffsetXAddress2", offsetXAddress2.ToString());
            XmlHelper.SetValue(modbusAlignerElement, "OffsetYAddress2", offsetYAddress2.ToString());


            XmlHelper.SetValue(modbusAlignerElement, "XAxisCalibration", xAxisCalibration.ToString());
            XmlHelper.SetValue(modbusAlignerElement, "YAxisCalibration", yAxisCalibration.ToString());
            XmlHelper.SetValue(modbusAlignerElement, "RAxisCalibration", rAxisCalibration.ToString());
        }

        public void Load(XmlElement configElement, string sectionName)
        {
            XmlElement modbusAlignerElement = configElement[sectionName];
            if (modbusAlignerElement == null)
                return;

            offsetXAddress1 = Convert.ToInt32(XmlHelper.GetValue(modbusAlignerElement, "OffsetXAddress1", "100"));
            offsetYAddress1 = Convert.ToInt32(XmlHelper.GetValue(modbusAlignerElement, "OffsetYAddress1", "102"));
            angleAddress = Convert.ToInt32(XmlHelper.GetValue(modbusAlignerElement, "AngleAddress", "104"));
            offsetXAddress2 = Convert.ToInt32(XmlHelper.GetValue(modbusAlignerElement, "OffsetXAddress2", "100"));
            offsetYAddress2 = Convert.ToInt32(XmlHelper.GetValue(modbusAlignerElement, "OffsetYAddress2", "102"));

            xAxisCalibration = Convert.ToSingle(XmlHelper.GetValue(modbusAlignerElement, "XAxisCalibration", "1"));
            yAxisCalibration = Convert.ToSingle(XmlHelper.GetValue(modbusAlignerElement, "YAxisCalibration", "1"));
            rAxisCalibration = Convert.ToSingle(XmlHelper.GetValue(modbusAlignerElement, "RAxisCalibration", "1"));
        }
    }

    //public class MelsecConnectionInfo
    //{
    //    string ipAddress = String.Empty;
    //    public string IpAddress
    //    {
    //        get { return ipAddress; }
    //        set { ipAddress = value; }
    //    }

    //    int port = 2005;
    //    public int Port
    //    {
    //        get { return port; }
    //        set { port = value; }
    //    }

    //    int stationNumber = 5;
    //    public int StationNumber
    //    {
    //        get { return stationNumber; }
    //        set { stationNumber = value; }
    //    }

    //    int pcStatusAddress;
    //    public int PcStatusAddress
    //    {
    //        get { return pcStatusAddress; }
    //        set { pcStatusAddress = value; }
    //    }

    //    int plcStatusAddress;
    //    public int PlcStatusAddress
    //    {
    //        get { return plcStatusAddress; }
    //        set { plcStatusAddress = value; }
    //    }

    //    public void Save(XmlElement configElement, string sectionName)
    //    {
    //        XmlElement melsecElement = configElement.OwnerDocument.CreateElement("", sectionName, "");
    //        configElement.AppendChild(melsecElement);

    //        XmlHelper.SetValue(melsecElement, "IpAddress", ipAddress);
    //        XmlHelper.SetValue(melsecElement, "Port", port.ToString());

    //        XmlHelper.SetValue(melsecElement, "PcStatusAddress", pcStatusAddress.ToString());
    //        XmlHelper.SetValue(melsecElement, "PlcStatusAddress", plcStatusAddress.ToString());

    //        XmlHelper.SetValue(melsecElement, "StationNumber", stationNumber.ToString());
    //    }

    //    public void Load(XmlElement configElement, string sectionName)
    //    {
    //        XmlElement melsecElement = configElement[sectionName];
    //        if (melsecElement == null)
    //            return;

    //        ipAddress = XmlHelper.GetValue(melsecElement, "IpAddress", "");
    //        port = Convert.ToInt32(XmlHelper.GetValue(melsecElement, "Port", "2005"));

    //        pcStatusAddress = Convert.ToInt32(XmlHelper.GetValue(melsecElement, "PcStatusAddress", "110"));
    //        plcStatusAddress = Convert.ToInt32(XmlHelper.GetValue(melsecElement, "PlcStatusAddress", "111"));

    //        stationNumber = Convert.ToInt32(XmlHelper.GetValue(melsecElement, "StationNumber", "1"));
    //    }
    //}

    //public class MelsecDataExporter : DataExporter
    //{
    //    bool plcConnected;
    //    MelsecConnectionInfo melsecConnectionInfo;
    //    AxActUtlTypeLib.AxActUtlType melsecUtil = null;

    //    public MelsecDataExporter(MelsecConnectionInfo melsecConnectionInfo)
    //    {
    //        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MelsecConnectionInfoForm));

    //        melsecUtil = new AxActUtlTypeLib.AxActUtlType();

    //        melsecUtil.Enabled = true;
    //        melsecUtil.Name = "melsecUtil";
    //        melsecUtil.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axActUtl.OcxState")));

    //        this.melsecConnectionInfo = melsecConnectionInfo;

    //        ConnectPlc();
    //    }

    //    public void ConnectPlc()
    //    {
    //        int iReturnCode = 0;
    //        try
    //        {
    //            melsecUtil.ActLogicalStationNumber = melsecConnectionInfo.StationNumber;
    //            iReturnCode = melsecUtil.Open();
    //        }
    //        catch (Exception exception)
    //        {
    //            //MessageBox.Show(exception.Message, Name,MessageBoxButtons.OK, MessageBoxIcon.Error);
    //            LogHelper.Debug(LoggerType.Network, exception.Message);
    //            plcConnected = false;
    //            return;
    //        }
    //        if (iReturnCode == 0)
    //        {
    //            LogHelper.Debug(LoggerType.Network, String.Format("PLC Connect Success : 0x{0:x8}", iReturnCode));
    //            plcConnected = true;
    //        }
    //        else
    //        {
    //            LogHelper.Debug(LoggerType.Network, String.Format("PLC Connect ERROR : 0x{0:x8}", iReturnCode));
    //            plcConnected = false;
    //        }
    //    }

    //    public override void Export(InspectionResult inspectionResult, CancellationToken cancellationToken)
    //    {
    //        if (plcConnected == false )
    //            return;

    //        if (alignDataInterfaceInfo != null)
    //        {
    //            int iReturnCode = 0;
    //            short[] arrDeviceValue = new short[6];		    //Data for 'DeviceValue
    //            try
    //            {
    //                string szDeviceName = String.Format("D{0}\nD{1}\nD{2}\nD{3}\nD{4}\nD{5}", alignDataInterfaceInfo.OffsetXAddress1, alignDataInterfaceInfo.OffsetXAddress1 + 1,
    //                                alignDataInterfaceInfo.OffsetYAddress1, alignDataInterfaceInfo.OffsetYAddress1 + 1, alignDataInterfaceInfo.AngleAddress, alignDataInterfaceInfo.AngleAddress + 1);

    //                int alignmentX = (int)(Convert.ToSingle(inspectionResult.GetExtraResult("AlignmentX")) * alignDataInterfaceInfo.XAxisCalibration);
    //                int alignmentY = (int)(Convert.ToSingle(inspectionResult.GetExtraResult("AlignmentY")) * alignDataInterfaceInfo.YAxisCalibration);
    //                int alignmentAngle = (int)(Convert.ToSingle(inspectionResult.GetExtraResult("AlignmentAngle")) * alignDataInterfaceInfo.RAxisCalibration);

    //                arrDeviceValue[0] = (short)(alignmentX & 0xffff);
    //                arrDeviceValue[1] = (short)((alignmentX >> 16) & 0xffff);
    //                arrDeviceValue[2] = (short)(alignmentY & 0xffff);
    //                arrDeviceValue[3] = (short)((alignmentY >> 16) & 0xffff);
    //                arrDeviceValue[4] = (short)(alignmentAngle & 0xffff);
    //                arrDeviceValue[5] = (short)((alignmentAngle >> 16) & 0xffff);

    //                iReturnCode = melsecUtil.WriteDeviceRandom2(szDeviceName, 6, ref arrDeviceValue[0]);

    //                if (iReturnCode == 0)
    //                {
    //                    LogHelper.Debug(LoggerType.Network, String.Format("Write Alignment Data : {0}, {1}, {2}, {3}, {4}, {5}", arrDeviceValue[0], arrDeviceValue[1], arrDeviceValue[2], arrDeviceValue[3], arrDeviceValue[4], arrDeviceValue[5]));
    //                }
    //                else
    //                {
    //                    LogHelper.Debug(LoggerType.Network, String.Format("PLC ERROR : 0x{0:x8}", iReturnCode));
    //                }
    //            }
    //            catch (Exception exception)
    //            {
    //                LogHelper.Debug(LoggerType.Network, exception.Message);
    //                return;
    //            }
    //        }
    //    }

    //    public override bool UpdateResult(InspectionResult inspectionResult, PacketParser packetParser)
    //    {
    //        return false;
    //    }
    //}

    public class XgtAlignerDataExporter : DataExporter
    {
        SerialPortEx serialPort = null;

        public XgtAlignerDataExporter(SerialPortEx serialPort)
        {
            this.serialPort = serialPort;
        }

        private byte[] MakePacket(int address, int value)
        {
            string valueHexStr = value.ToString("X08");
            string packet = String.Format("000WSS0208%RW{0:D5}{1}08%RW{2:D5}{3}0", address, valueHexStr.Substring(4), address+1, valueHexStr.Substring(0, 4));
            byte[] data = Encoding.ASCII.GetBytes(packet);
            data[0] = 5;
            data[data.Length - 1] = 4;
            return data; 
        }

        public override void Export(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            if (alignDataInterfaceInfo != null)
            {
                int alignmentX1 = (int)(Convert.ToSingle(inspectionResult.GetExtraResult("AlignmentX1")) * alignDataInterfaceInfo.XAxisCalibration);
                int alignmentY1 = (int)(Convert.ToSingle(inspectionResult.GetExtraResult("AlignmentY1")) * alignDataInterfaceInfo.YAxisCalibration);
                int alignmentAngle = (int)(Convert.ToSingle(inspectionResult.GetExtraResult("AlignmentAngle")) * alignDataInterfaceInfo.RAxisCalibration);
                int alignmentX2 = (int)(Convert.ToSingle(inspectionResult.GetExtraResult("AlignmentX2")) * alignDataInterfaceInfo.XAxisCalibration);
                int alignmentY2 = (int)(Convert.ToSingle(inspectionResult.GetExtraResult("AlignmentY2")) * alignDataInterfaceInfo.YAxisCalibration);
                if(inspectionResult.Judgment == Judgment.Accept)
                {
                    byte[] data = MakePacket(alignDataInterfaceInfo.OffsetXAddress1, alignmentX1);
                    serialPort.WritePacket(data, 0, data.Length);
                    string dataStr = Encoding.UTF8.GetString(data);
                    LogHelper.Debug(LoggerType.Inspection, string.Format("alignDataInterfaceInfo.OffsetXAddress1 X1 : {0}", dataStr));
                    Thread.Sleep(50);

                    data = MakePacket(alignDataInterfaceInfo.OffsetYAddress1, alignmentY1);
                    serialPort.WritePacket(data, 0, data.Length);
                    dataStr = Encoding.UTF8.GetString(data);
                    LogHelper.Debug(LoggerType.Inspection, string.Format("alignDataInterfaceInfo.OffsetXAddress1  Y1: {0}", dataStr));
                    Thread.Sleep(50);

                    data = MakePacket(alignDataInterfaceInfo.AngleAddress, alignmentAngle);
                    serialPort.WritePacket(data, 0, data.Length);
                    dataStr = Encoding.UTF8.GetString(data);
                    LogHelper.Debug(LoggerType.Inspection, string.Format("alignDataInterfaceInfo.AngleAddress : {0}", dataStr));
                    Thread.Sleep(50);

                    data = MakePacket(alignDataInterfaceInfo.OffsetXAddress2, alignmentX2);
                    serialPort.WritePacket(data, 0, data.Length);
                    dataStr = Encoding.UTF8.GetString(data);
                    LogHelper.Debug(LoggerType.Inspection, string.Format("alignDataInterfaceInfo.OffsetXAddress2 X2 : {0}", dataStr));
                    Thread.Sleep(50);

                    data = MakePacket(alignDataInterfaceInfo.OffsetYAddress2, alignmentY2);
                    serialPort.WritePacket(data, 0, data.Length);
                    dataStr = Encoding.UTF8.GetString(data);
                    LogHelper.Debug(LoggerType.Inspection, string.Format("alignDataInterfaceInfo.OffsetXAddress2 Y2 : {0}", dataStr));
                    Thread.Sleep(50);
                }
            }
        }
    }
}
