using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;

namespace DynMvp.Devices.Comm
{
    public enum FinsCommandType
    {
        RequestAddress, ReadData, WriteData
    }

    public enum FinsDataType
    {
        CIO_Bit = 0x30,
        WR_Bit = 0x31,
        HR_Bit = 0x32,
        AR_Bit = 0x33,
        CIO_Word = 0xB0,
        WR_Word = 0xB1,
        HR_Word = 0xB2,
        AR_Word = 0xB3,
        TIM_PV = 0x89,
        DM_Bit = 0x02,
        DM_Word = 0x82,
        EMx_Bit = 0x20,
        EMx_Word = 0xA0,
        EM_Current = 0x98,
        IR_PV = 0xDC,
        DR_PV = 0xBC
    }

    public struct FinsInfo
    {
        int networkNo;
        public int NetworkNo
        {
            get { return networkNo; }
            set { networkNo = value; }
        }

        long plcStateAddress;
        public long PlcStateAddress
        {
            get { return plcStateAddress; }
            set { plcStateAddress = value; }
        }

        long pcStateAddress;
        public long PcStateAddress
        {
            get { return pcStateAddress; }
            set { pcStateAddress = value; }
        }

        long resultAddress;
        public long ResultAddress
        {
            get { return resultAddress; }
            set { resultAddress = value; }
        }

        string ipAddress;
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        int portNo;
        public int PortNo
        {
            get { return portNo; }
            set { portNo = value; }
        }

        public void Load(XmlElement xmlElement, string keyName)
        {
            XmlElement finsElement = xmlElement[keyName];
            if (finsElement == null)
                return;

            networkNo = Convert.ToInt32(XmlHelper.GetValue(finsElement, "NetworkNo", ""));
            plcStateAddress = Convert.ToInt64(XmlHelper.GetValue(finsElement, "PlcStateAddress", ""));
            pcStateAddress = Convert.ToInt64(XmlHelper.GetValue(finsElement, "PcStateAddress", ""));
            resultAddress = Convert.ToInt64(XmlHelper.GetValue(finsElement, "ResultAddress", ""));
            ipAddress = XmlHelper.GetValue(finsElement, "IpAddress", "");
            portNo = Convert.ToInt32(XmlHelper.GetValue(finsElement, "PortNo", ""));
        }

        public void Save(XmlElement xmlElement, string keyName)
        {
            XmlElement finsElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(finsElement);

            XmlHelper.SetValue(finsElement, "NetworkNo", networkNo.ToString());
            XmlHelper.SetValue(finsElement, "PlcStateAddress", plcStateAddress.ToString());
            XmlHelper.SetValue(finsElement, "PcStateAddress", pcStateAddress.ToString());
            XmlHelper.SetValue(finsElement, "ResultAddress", resultAddress.ToString());
            if (String.IsNullOrEmpty(ipAddress))
                XmlHelper.SetValue(finsElement, "IpAddress", "");
            else
                XmlHelper.SetValue(finsElement, "IpAddress", ipAddress);
            XmlHelper.SetValue(finsElement, "PortNo", portNo.ToString());
        }
    }

    public class FinsReceivedPacket : ReceivedPacket
    {
        FinsCommandType requestCommand;
        public FinsCommandType RequestCommand
        {
            get { return requestCommand; }
        }
        FinsDataType dataType;
        public FinsDataType DataType
        {
            get { return dataType; }
        }
        long dataAddress;
        public long DataAddress
        {
            get { return dataAddress; }
        }
        int plcNodeNo = 0;
        public int PlcNodeNo
        {
            get { return plcNodeNo; }
        }
        int pcNodeNo = 0;
        public int PcNodeNo
        {
            get { return pcNodeNo; }
        }
        byte[] binaryData;
        public byte[] BinaryData
        {
            get { return binaryData; }
        }

        public FinsReceivedPacket(int plcNodeNo, int pcNodeNo, FinsCommandType requestCommand, FinsDataType dataType, long dataAddress, byte[] stringData, byte[] binaryData)
        {
            this.plcNodeNo = plcNodeNo;
            this.pcNodeNo = pcNodeNo;
            this.requestCommand = requestCommand;
            this.dataType = dataType;
            this.dataAddress = dataAddress;
            this.receivedData = stringData;
            this.binaryData = binaryData;
        }
    }

    public class FinsPacketParser : PacketParser
    {
        FinsCommandType requestCommand;
        public FinsCommandType RequestCommand
        {
            get { return requestCommand; }
        }
        FinsDataType dataType;
        int plcNetworkNo;
        int plcNodeNo;
        public int PlcNodeNo
        {
            get { return plcNodeNo; }
        }
        int pcNetworkNo;
        int pcNodeNo;
        public int PcNodeNo
        {
            get { return pcNodeNo; }
        }
        long dataAddress;
        public long DataAddress
        {
            get { return dataAddress; }
        }
        int bitAddress;
        int dataSize;
        string data;

        int sequence = 0;
        static int nextSequence = 0;
        string lastSendPacket;

        bool successed;
        public bool Successed
        {
            get { return successed; }
            set { successed = value; }
        }

        public FinsPacketParser()
        {
            this.requestCommand = FinsCommandType.RequestAddress;
        }

        public FinsPacketParser(int plcNetworkNo, int plcNodeNo, int pcNetworkNo, int pcNodeNo, FinsDataType dataType, long dataAddress, int bitAddress, int dataSize)
        {
            this.requestCommand = FinsCommandType.ReadData;
            this.plcNetworkNo = plcNetworkNo;
            this.plcNodeNo = plcNodeNo;
            this.pcNetworkNo = pcNetworkNo;
            this.pcNodeNo = pcNodeNo;
            this.dataType = dataType;
            this.dataAddress = dataAddress;
            this.bitAddress = bitAddress;
            this.dataSize = dataSize;

            sequence = nextSequence;
            nextSequence++;
            nextSequence = nextSequence % 255;
        }

        public FinsPacketParser(int plcNetworkNo, int plcNodeNo, int pcNetworkNo, int pcNodeNo, FinsDataType dataType, long dataAddress, int bitAddress, string data)
        {
            this.requestCommand = FinsCommandType.WriteData;
            this.plcNetworkNo = plcNetworkNo;
            this.plcNodeNo = plcNodeNo;
            this.pcNetworkNo = pcNetworkNo;
            this.pcNodeNo = pcNodeNo;
            this.dataType = dataType;
            this.dataAddress = dataAddress;
            this.bitAddress = bitAddress;
            this.data = data;

            sequence = nextSequence;
            nextSequence++;
            nextSequence = nextSequence % 255;
        }

        //        public override byte[] GetRequestPacket()
        //        {
        //            successed = false;
        //            string packet = "";

        //            string sequenceStr = sequence.ToString("X2");

        //            switch (requestCommand)
        //            {
        //                case FinsCommandType.RequestAddress:
        //                    {
        //                        string commandString = "00000000";
        //                        string errorCode = "00000000";
        //                        string clientNodeAddress = "00000000";

        //                        packet = commandString + errorCode + clientNodeAddress;
        //                    }
        //                    break;
        //                case FinsCommandType.ReadData:
        //                    {
        //                        string commandString = "00000002";
        //                        string errorCode = "00000000";
        //                        string finsCommand = "0101";

        //                        packet = commandString + errorCode + "800002" + plcNetworkNo.ToString("X02") + plcNodeNo.ToString("X02")
        //                                            + "00" // PLC CPU Unit 번호
        //                                            + pcNetworkNo.ToString("X02") + pcNodeNo.ToString("X02")
        //                                            + "00" // PLC CPU Unit 번호
        //                                            + sequenceStr // Sequence. 자동 증가
        //                                            + finsCommand + ((int)dataType).ToString("X02") + dataAddress.ToString("X04") + bitAddress.ToString("X02") + dataSize.ToString("X04");
        //                    } 
        //                    break;
        //                case FinsCommandType.WriteData:
        //                    {
        //                        string commandString = "00000002";
        //                        string errorCode = "00000000";
        //                        string finsCommand = "0102";

        //                        int dataSize = data.Length;

        //                        packet = commandString + errorCode + "800002" + plcNetworkNo.ToString("X02") + plcNodeNo.ToString("X02")
        //                                            + "00" // PLC CPU Unit 번호
        //                                            + pcNetworkNo.ToString("X02") + pcNodeNo.ToString("X02")
        //                                            + "00" // PLC CPU Unit 번호
        //                                            + sequenceStr // Sequence. 자동 증가
        //                                            + finsCommand + ((int)dataType).ToString("X02") + dataAddress.ToString("X04") + 
        //                                            bitAddress.ToString("X02") + (dataSize/4).ToString("X04") + data;
        //                    }
        //                    break;
        //            }

        //            if (String.IsNullOrEmpty(packet))
        //                return null;

        //            lastSendPacket = "46494E53" + (packet.Length/2).ToString("X08") + packet;

        //            List<byte> valueList = new List<byte>();
        //            for (int i=0; i< lastSendPacket.Length; i+=2)
        //            {
        //                // Convert the number expressed in base-16 to an integer.
        //                byte value = Convert.ToByte(lastSendPacket.Substring(i, 2), 16);
        //                valueList.Add(value);
        //            }

        ////            LogHelper.Debug(LoggerType.Network, String.Format("Send Packet {0}", lastSendPacket));

        //            byte[] bytePacket = valueList.ToArray();

        //            return bytePacket;
        //        }

        public override bool ParsePacket(byte[] packetContents, out int processedPacketCnt)
        {
            processedPacketCnt = packetContents.Length;
            return false;
            //            string currentString = ""; //  System.Text.Encoding.Default.GetString(packetContents.ToArray(), );

            //            for (int i = 0; i < packetContents.Length; i++)
            //            {
            //                currentString += packetContents[i].ToString("X02");
            //            }

            ////            LogHelper.Debug(LoggerType.Network, String.Format("Receive Packet {0}", currentString));

            //            string header = currentString.Substring(0, 8);
            //            if (header.ToUpper() != "46494E53")
            //                return false;

            //            string packetSizeStr = currentString.Substring(8, 8);
            //            if (packetSizeStr.Length < 8)
            //                return false;

            //            int packetSize = Convert.ToInt32(packetSizeStr, 16);
            //            string packet = currentString.Substring(16);
            //            if (packetSize*2 < packet.Length)
            //                return false;

            //            string command = packet.Substring(0, 8);
            //            string errorCode = packet.Substring(8, 8);

            //            if (errorCode != "00000000")
            //            {
            //                LogHelper.Debug(LoggerType.Network, String.Format("Error Occurred. {0}", errorCode));
            //                return true;
            //            }

            //            string receivedData = "";
            //            byte[] receivedBinaryData = null;

            //            switch (requestCommand)
            //            {
            //                case FinsCommandType.RequestAddress:
            //                    {
            //                        string pcNodeNoStr = packet.Substring(16, 8);
            //                        string plcNodeNoStr = packet.Substring(24, 8);
            //                        pcNodeNo = Convert.ToInt32(pcNodeNoStr, 16);
            //                        plcNodeNo = Convert.ToInt32(plcNodeNoStr, 16);

            //                        successed = true;
            //                    }
            //                    break;
            //                case FinsCommandType.ReadData:
            //                    {
            //                        string returnCommand = packet.Substring(36, 4);
            //                        string frameErrorCode = packet.Substring(40, 4);

            //                        if (returnCommand != "0101")
            //                        {
            //                            LogHelper.Debug(LoggerType.Network, String.Format("Frame Command is Invalid. {0}", returnCommand));
            //                            LogHelper.Debug(LoggerType.Network, String.Format("Send Packet. {0}", lastSendPacket));
            //                            LogHelper.Debug(LoggerType.Network, String.Format("Return Packet. {0}", currentString));
            //                            return true;
            //                        }

            //                        if (frameErrorCode != "0000")
            //                        {
            //                            LogHelper.Debug(LoggerType.Network, String.Format("Frame Error. {0}", frameErrorCode));
            //                            return true;
            //                        }

            //                        receivedData = packet.Substring(44);
            //                        receivedBinaryData = packetContents.Skip(30).Take(4).ToArray();
            //                        successed = true;
            //                    }
            //                    break;
            //                case FinsCommandType.WriteData:
            //                    {
            //                        string returnCommand = packet.Substring(36, 4);
            //                        string frameErrorCode = packet.Substring(40, 4);

            //                        if (returnCommand != "0102")
            //                        {
            //                            LogHelper.Debug(LoggerType.Network, String.Format("Frame Command is Invalid. {0}", returnCommand));
            //                            LogHelper.Debug(LoggerType.Network, String.Format("Send Packet. {0}", lastSendPacket));
            //                            LogHelper.Debug(LoggerType.Network, String.Format("Return Packet. {0}", currentString));
            //                            return true;
            //                        }

            //                        if (frameErrorCode != "0000")
            //                        {
            //                            LogHelper.Debug(LoggerType.Network, String.Format("Frame Error. {0}", frameErrorCode));
            //                            return true;
            //                        }

            //                        successed = true;
            //                    }
            //                    break;
            //            }

            //            if (OnDataReceived != null)
            //            {
            //                FinsReceivedPacket receivedPacket = new FinsReceivedPacket(plcNodeNo, pcNodeNo, requestCommand, dataType, dataAddress, Encoding.Default.GetBytes(receivedData), receivedBinaryData);
            //                OnDataReceived(receivedPacket);
            //            }

            //            return true;
        }

        public override PacketParser Clone()
        {
            throw new NotImplementedException();
        }

        public override byte[] EncodePacket(string protocol)
        {
            throw new NotImplementedException();
        }

        public override string DecodePacket(byte[] packet)
        {
            throw new NotImplementedException();
        }
    }
}
