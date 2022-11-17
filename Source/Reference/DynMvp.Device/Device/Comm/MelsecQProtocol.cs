using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Devices.Comm;

namespace DynMvp.Device.Device.Comm
{
    public class MelsecQSendPacket : SendPacket
    {
        string command;
        public string Command
        {
            get { return command; }
        }

        string subCommand;
        public string SubCommand
        {
            get { return subCommand; }
        }

        string deviceName;
        public string DeviceName
        {
            get { return deviceName; }
        }

        public bool IsReadCommand()
        {
            return command == "0401";
        }

        int dataLength = 0;
        public int DataLength
        {
            get { return dataLength; }
        }

        StringBuilder dataList = new StringBuilder();
        public string Data
        {
            get { return dataList.ToString(); }
        }

        public MelsecQSendPacket()
        {
            WaitResponse = true;
        }

        public override bool IsValid()
        {
            return string.IsNullOrEmpty(Data) == false;
        }

        public void SetupBlockReadWord(string deviceName, int numSize)
        {
            command = "0401";
            subCommand = "0000";

            this.deviceName = deviceName;
            dataList.Append(numSize.ToString("X4"));
        }

        public void SetupBlockWriteWord(string deviceName, string data = "")
        {
            command = "1401";
            subCommand = "0000";

            this.deviceName = deviceName;
        }

        public void AddData(int value)
        {
            string hexStr = "";
            byte[] valueByte = BitConverter.GetBytes(value);
            for (int i = 3; i >= 0; i--)
                hexStr += ((int)valueByte[i]).ToString("X2");
            dataList.Append(hexStr);

            dataLength += hexStr.Length;
        }

        public void AddDataSwap(int value)
        {
            string hexStr = "";
            byte[] valueByte = BitConverter.GetBytes(value);
            for (int i = 3; i >= 0; i--)
                hexStr += ((int)valueByte[i]).ToString("X2");

            string swapResult = "";
            swapResult += hexStr.Substring(4, 4);
            swapResult += hexStr.Substring(0, 4);
            dataList.Append(swapResult);
            dataLength += swapResult.Length;
        }

        public void AddData(short value)
        {
            string hexStr = "";
            byte[] valueByte = BitConverter.GetBytes(value);
            for (int i = 1; i >= 0; i--)
                hexStr += ((int)valueByte[i]).ToString("X2");
            dataList.Append(hexStr);

            dataLength += hexStr.Length;
        }

        public void AddData(string data)
        {
            string hexStr = "";
            foreach (char ch in data)
            {
                hexStr += ((int)ch).ToString("X2");
            }
            dataList.Append(hexStr);

            dataLength += hexStr.Length;
        }

        public void AddDataSwap(string data)
        {
            if (data.Length % 2 == 1)
            {
                data += " ";
            }

            string swapString = "";

            for (int i = 0; i < data.Length; i += 2)
            {
                swapString += data[i + 1];
                swapString += data[i];
            }

            AddData(swapString);
        }

        public void AddData(byte[] byteData)
        {
            string hexStr = "";
            foreach (byte bt in byteData)
            {
                hexStr += ((int)bt).ToString("X2");
            }
            dataList.Append(hexStr);

            dataLength += hexStr.Length;
        }

        public void ClearData()
        {
            dataLength = 0;
            dataList.Clear();
        }
    }


    public class MelsecQReceivedPacket : AsyncRecivedPacket
    {
        public short GetShort(int startIndex)
        {
            byte[] valueByte = new byte[2];

            valueByte[0] = ReceivedDataByte[startIndex + 1];
            valueByte[1] = ReceivedDataByte[startIndex];

            return BitConverter.ToInt16(valueByte, 0);
        }

        public int GetInt(int startIndex)
        {
            byte[] valueByte = new byte[4];

            for (int i = 0; i < 3; i++)
                valueByte[i] = ReceivedDataByte[startIndex + 3 - i];

            return BitConverter.ToInt32(valueByte, 0);
        }

        public int GetSwapInt(int startIndex)
        {
            byte[] valueByte = new byte[4];
            byte[] valueByteResult = new byte[4];
            for (int i = 0; i < 3; i++)
                valueByte[i] = ReceivedDataByte[startIndex + 3 - i];
            valueByteResult[0] = valueByte[2];
            valueByteResult[1] = valueByte[3];
            valueByteResult[2] = valueByte[0];
            valueByteResult[3] = valueByte[1];

            return BitConverter.ToInt32(valueByte, 0);
        }

        public string GetString(int startIndex, int length)
        {
            StringBuilder valueStrBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
                valueStrBuilder.Append((char)ReceivedDataByte[startIndex + i]);

            return valueStrBuilder.ToString();
        }

        public string GetSwapString(int startIndex, int length)
        {
            StringBuilder valueStrBuilder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                valueStrBuilder.Append((char)ReceivedDataByte[startIndex + i]);
            }
            string result = valueStrBuilder.ToString();
            char[] charResult = result.ToArray();


            if (result.Length % 2 == 0)
            {
                for (int i = 0; i < result.Length; i = i + 2)
                {
                    char temp = charResult[i];
                    charResult[i] = charResult[i + 1];
                    charResult[i + 1] = temp;
                }
            }
            else
            {
                for (int i = 0; i < result.Length - 1; i = i + 2)
                {
                    char temp = charResult[i];
                    charResult[i] = charResult[i + 1];
                    charResult[i + 1] = temp;
                }
            }

            string returnResult = "";
            for (int i = 0; i < charResult.Length; i++)
            {
                returnResult += charResult[i].ToString();
            }


            return returnResult;
        }

        public byte[] GetByte(int startIndex, int length)
        {
            byte[] getByte = new byte[length];
            for (int i = 0; i < length; i++)
                getByte[i] = ReceivedDataByte[startIndex + i];
            return getByte;
        }

        public byte[] GetSwapBit(byte[] data)
        {
            for (int i = 0; i < data.Length; i = i + 2)
            {
                byte temp = data[i];
                data[i] = data[i + 1];
                data[i + 1] = temp;
            }
            return data;
        }

        public byte[] GetSwapBit(byte[] data, int startIndex, int length)
        {
            for (int i = startIndex; i < length; i = i + 2)
            {
                byte temp = data[i];
                data[i] = data[i + 1];
                data[i + 1] = temp;
            }
            return data;
        }
    }

    public class MelsecInfo
    {
        TcpIpInfo tcpIpInfo = new TcpIpInfo();
        public TcpIpInfo TcpIpInfo
        {
            get { return tcpIpInfo; }
            set { tcpIpInfo = value; }
        }

        string networkNo = "00";
        public string NetworkNo
        {
            get { return networkNo; }
            set { networkNo = value; }
        }

        string plcNo = "FF";
        public string PlcNo
        {
            get { return plcNo; }
            set { plcNo = value; }
        }

        string moduleIoNo = "03FF";
        public string ModuleIoNo
        {
            get { return moduleIoNo; }
            set { moduleIoNo = value; }
        }

        string moduleDeviceNo = "00";
        public string ModuleDeviceNo
        {
            get { return moduleDeviceNo; }
            set { moduleDeviceNo = value; }
        }

        string cpuInspectorData = "000A";
        public string CpuInspectorData
        {
            get { return cpuInspectorData; }
            set { cpuInspectorData = value; }
        }

        public MelsecInfo()
        {

        }

        public MelsecInfo(string ipAddress, int portNo, string networkNo = "00", string plcNo = "FF", string moduleIoNo = "03FF",
                                string moduleDeviceNo = "00", string cpuInspectorData = "000A")
        {
            tcpIpInfo = new TcpIpInfo(ipAddress, portNo);

            this.networkNo = networkNo;
            this.plcNo = plcNo;
            this.moduleIoNo = moduleIoNo;
            this.moduleDeviceNo = moduleDeviceNo;
            this.cpuInspectorData = cpuInspectorData;
        }

        public void Load(XmlElement xmlElement, string keyName)
        {
            XmlElement tcpIpElement = xmlElement[keyName];
            if (tcpIpElement == null)
                return;

            tcpIpInfo.Load(tcpIpElement, "TcpIp");
            networkNo = XmlHelper.GetValue(tcpIpElement, "NetworkNo", "");
            plcNo = XmlHelper.GetValue(tcpIpElement, "PlcNo", "0");
            moduleIoNo = XmlHelper.GetValue(tcpIpElement, "ModuleIoNo", "0");
            moduleDeviceNo = XmlHelper.GetValue(tcpIpElement, "ModuleDeviceNo", "0");
            cpuInspectorData = XmlHelper.GetValue(tcpIpElement, "CpuInspectorData", "0");
        }

        public void Save(XmlElement xmlElement, string keyName)
        {
            XmlElement tcpIpElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(tcpIpElement);

            tcpIpInfo.Save(tcpIpElement, "TcpIp");
            XmlHelper.SetValue(tcpIpElement, "NetworkNo", networkNo);
            XmlHelper.SetValue(tcpIpElement, "PlcNo", plcNo.ToString());
            XmlHelper.SetValue(tcpIpElement, "ModuleIoNo", moduleIoNo.ToString());
            XmlHelper.SetValue(tcpIpElement, "ModuleDeviceNo", moduleDeviceNo.ToString());
            XmlHelper.SetValue(tcpIpElement, "CpuInspectorData", cpuInspectorData.ToString());
        }
    }

    public class MelsecQProtocol : IProtocol
    {
        MelsecInfo melsecInfo;
        private bool binaryMode;
        private bool readMode = false;

        const string COMMAND_HEADER = "5000";
        const string RESPONSE_HEADER = "D000";

        public MelsecQProtocol(MelsecInfo melsecInfo)
        {
            this.melsecInfo = melsecInfo;
        }

        public MelsecQProtocol(MelsecInfo melsecInfo, bool binaryMode = false)
        {
            this.melsecInfo = melsecInfo;
            this.binaryMode = binaryMode;
        }

        private string GetDeviceCode(string deviceName)
        {
            if (string.IsNullOrEmpty(deviceName))
            {
                LogHelper.Debug(LoggerType.Error, "MelsecQProtocol - GetDeviceCode : null");
            }
            switch (deviceName[0])
            {
                case 'D': return "D*";  // 데이터 레지스터
                case 'M': return "M*";  // 내부 릴레이
                case 'X': return "X*";  // 입력 릴레이
                case 'Y': return "Y*";  // 출력 릴레이
                case 'R': return "R*";  // 파일 레지스터
            }

            throw new ArgumentOutOfRangeException();
        }

        private string GetDeviceCodeBinary(string deviceName)
        {
            switch (deviceName[0])
            {
                case 'D': return "A8";  // 데이터 레지스터
                case 'M': return "90";  // 내부 릴레이
                case 'X': return "9C";  // 입력 릴레이
                case 'Y': return "9D";  // 출력 릴레이
                case 'R': return "AF";  // 파일 레지스터
                case 'Z': return "B0";  // 파일 레지스터
            }

            throw new ArgumentOutOfRangeException();
        }

        public string GetCmdBlockReadWord()
        {
            string cmdBlockReadWord = "0401";
            string cmdSubCommand = "0000";

            return cmdBlockReadWord + cmdSubCommand;
        }

        byte[] MakePacketAscii(SendPacket sendPacket)
        {
            MelsecQSendPacket melsecQSendPacket = (MelsecQSendPacket)sendPacket;

            string deviceName = melsecQSendPacket.DeviceName;
            string deviceCode = GetDeviceCode(deviceName);
            string deviceNumber = new string('0', 6 - (deviceName.Count() - 1)) + (deviceName.Substring(1));

            string commandHeader = COMMAND_HEADER + melsecInfo.NetworkNo + melsecInfo.PlcNo + melsecInfo.ModuleIoNo + melsecInfo.ModuleDeviceNo;
            string commandData = "";
            if (melsecQSendPacket.IsReadCommand())
            {
                commandData = melsecInfo.CpuInspectorData + melsecQSendPacket.Command + melsecQSendPacket.SubCommand +
                                    deviceCode + deviceNumber + melsecQSendPacket.Data;
            }
            else
            {
                commandData = melsecInfo.CpuInspectorData + melsecQSendPacket.Command + melsecQSendPacket.SubCommand +
                                    deviceCode + deviceNumber + (melsecQSendPacket.Data.Length / 4).ToString("X4") + melsecQSendPacket.Data;
            }

            string command = commandHeader + commandData.Length.ToString("X4") + commandData;

            return Encoding.UTF8.GetBytes(command);
        }

        byte[] MakePacketBinary(SendPacket sendPacket)
        {
            MelsecQSendPacket melsecQSendPacket = (MelsecQSendPacket)sendPacket;

            string deviceName = melsecQSendPacket.DeviceName;
            string deviceCode = GetDeviceCodeBinary(deviceName);
            string deviceNumber =
            StringHelper.ByteArrayToHexString(BitConverter.GetBytes(Convert.ToInt32((deviceName.Substring(1)))).Take(3).ToArray());

            string commandHeader = COMMAND_HEADER +
                                   melsecInfo.NetworkNo + melsecInfo.PlcNo +
                                   StringHelper.SwapWordHex(melsecInfo.ModuleIoNo) + melsecInfo.ModuleDeviceNo;
            string commandData = "";
            if (melsecQSendPacket.IsReadCommand())
            {
                commandData = StringHelper.SwapWordHex(melsecInfo.CpuInspectorData +
                              melsecQSendPacket.Command + melsecQSendPacket.SubCommand) + deviceNumber + deviceCode +
                              StringHelper.SwapWordHex(melsecQSendPacket.Data);
            }
            else
            {
                commandData = StringHelper.SwapWordHex(melsecInfo.CpuInspectorData +
                              melsecQSendPacket.Command + melsecQSendPacket.SubCommand) + deviceNumber + deviceCode +
                              StringHelper.SwapWordHex((melsecQSendPacket.Data.Length / 4).ToString("X4") + melsecQSendPacket.Data);
            }

            string command = commandHeader + StringHelper.SwapWordHex((commandData.Length / 2).ToString("X4")) + commandData;

            return StringHelper.HexStringToByteArray(command);

        }

        public ProcessPacketResult ParsePacketAscii(PacketBuffer packetBuffer, out AsyncRecivedPacket receivedPacket)
        {
            receivedPacket = new MelsecQReceivedPacket();

            string receivedStr = Encoding.UTF8.GetString(packetBuffer.DataByteFull);
            int startPos = receivedStr.IndexOf("D000");
            if (startPos == -1)
                return ProcessPacketResult.Incomplete;

            const int subHeaderSize = 22;

            if ((startPos + subHeaderSize) > receivedStr.Length)
                return ProcessPacketResult.Incomplete;

            string rNetworkNo = receivedStr.Substring(startPos + 4, 2);
            string rPlcNo = receivedStr.Substring(startPos + 6, 2);
            string rModuleIoNo = receivedStr.Substring(startPos + 8, 4);
            string rModuleDeviceNo = receivedStr.Substring(startPos + 12, 2);
            int length = Convert.ToInt32(receivedStr.Substring(startPos + 14, 4), 16);
            string rEndCode = receivedStr.Substring(startPos + 18, 4);

            int lastPos = startPos + subHeaderSize + length - 4;
            if (lastPos > receivedStr.Length)
                return ProcessPacketResult.Incomplete;

            if (rNetworkNo != melsecInfo.NetworkNo || rPlcNo != melsecInfo.PlcNo ||
                        rModuleIoNo != melsecInfo.ModuleIoNo || rModuleDeviceNo != melsecInfo.ModuleDeviceNo)
            {
                packetBuffer.RemoveData(lastPos);
                return ProcessPacketResult.OtherTarget;
            }

            receivedPacket.Valid = true;
            receivedPacket.ReceivedDataStr = receivedStr.Substring(startPos + 22, length - 4);
            receivedPacket.ReceivedDataByte = StringHelper.HexStringToByteArray(receivedPacket.ReceivedDataStr);
            receivedPacket.LogString = receivedPacket.ReceivedDataStr;
            packetBuffer.RemoveData(lastPos);

            return ProcessPacketResult.Complete;
        }

        public ProcessPacketResult ParsePacketBinary(PacketBuffer packetBuffer, out AsyncRecivedPacket receivedPacket)
        {
            receivedPacket = new MelsecQReceivedPacket();

            string receivedStr = StringHelper.ByteArrayToHexString(packetBuffer.DataByteFull);
            int startPos = receivedStr.IndexOf("D000");
            if (startPos == -1)
                return ProcessPacketResult.Incomplete;

            const int subHeaderSize = 22;

            if ((startPos + subHeaderSize) > receivedStr.Length)
                return ProcessPacketResult.Incomplete;

            string rNetworkNo = receivedStr.Substring(startPos + 4, 2);
            string rPlcNo = receivedStr.Substring(startPos + 6, 2);
            string rModuleIoNo = StringHelper.SwapWordHex(receivedStr.Substring(startPos + 8, 4));
            string rModuleDeviceNo = receivedStr.Substring(startPos + 12, 2);
            int length = Convert.ToInt32(StringHelper.SwapWordHex(receivedStr.Substring(startPos + 14, 4)), 16) * 2;
            string rEndCode = StringHelper.SwapWordHex(receivedStr.Substring(startPos + 18, 4));

            int lastPos = startPos + subHeaderSize + length - 4;
            if (lastPos > receivedStr.Length)
                return ProcessPacketResult.Incomplete;

            if (rNetworkNo != melsecInfo.NetworkNo || rPlcNo != melsecInfo.PlcNo || rModuleIoNo != melsecInfo.ModuleIoNo || rModuleDeviceNo != melsecInfo.ModuleDeviceNo)
            {
                packetBuffer.RemoveData(lastPos);
                return ProcessPacketResult.OtherTarget;
            }

            receivedPacket.Valid = true;
            receivedPacket.ReceivedDataStr = StringHelper.SwapWordHex(receivedStr.Substring(startPos + 22, length - 4));
            receivedPacket.ReceivedDataByte = StringHelper.HexStringToByteArray(receivedPacket.ReceivedDataStr);
            receivedPacket.LogString = receivedPacket.ReceivedDataStr;
            packetBuffer.RemoveData(lastPos);

            return ProcessPacketResult.Complete;
        }

        public byte[] MakePacket(SendPacket sendPacket)
        {
            if (binaryMode)
                return MakePacketBinary(sendPacket);
            else
                return MakePacketAscii(sendPacket);
        }

        public ProcessPacketResult ParsePacket(PacketBuffer packetBuffer, out AsyncRecivedPacket receivedPacket)
        {
            if (binaryMode)
                return ParsePacketBinary(packetBuffer, out receivedPacket);
            else
                return ParsePacketAscii(packetBuffer, out receivedPacket);
        }
    }
    

}
