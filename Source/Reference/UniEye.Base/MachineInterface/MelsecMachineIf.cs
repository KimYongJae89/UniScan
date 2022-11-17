using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace UniEye.Base.MachineInterface
{
    //public class MelsecMachineIfProtocolComp : IComparer<MelsecMachineIfProtocol>
    //{
    //    public int Compare(MelsecMachineIfProtocol x, MelsecMachineIfProtocol y)
    //    {
    //        return x.Address.CompareTo(y.Address);
    //    }
    //}

    public class MelsecMachineIfProtocol : TcpIpMachineIfProtocol
    {
        public override bool IsValid => (!this.use) || (this.use && !string.IsNullOrEmpty(this.address) && this.sizeWord > 0);

        public string Address { get => this.address; set => this.address = value; }
        string address = "";

        public bool IsReadCommand { get => this.isReadCommand; set => this.isReadCommand = value; }
        bool isReadCommand;

        public int SizeWord { get => this.sizeWord; set => this.sizeWord = value; }
        int sizeWord;

        public MelsecMachineIfProtocol(Enum command) : base(command) { }

        public MelsecMachineIfProtocol(Enum command, bool use, int waitResponceMs, string address, bool isReadCommand, int sizeWord) : base(command, use, waitResponceMs)
        {
            if (address == null)
                address = "";
            this.use = use;
            this.address = address;
            this.isReadCommand = isReadCommand;
            this.sizeWord = sizeWord;
        }

        public override MachineIfProtocol Clone()
        {
            MelsecMachineIfProtocol melsecMachineIfProtocol = new MelsecMachineIfProtocol(this.command, this.use, this.waitResponceMs, this.address, this.isReadCommand, this.sizeWord);
            return melsecMachineIfProtocol;
        }

        protected override void SaveXml(XmlElement element)
        {
            base.SaveXml(element);

            XmlHelper.SetValue(element, "Address", address.ToString());
            XmlHelper.SetValue(element, "IsReadCommand", isReadCommand.ToString());
            XmlHelper.SetValue(element, "SizeWord", sizeWord.ToString());
            XmlHelper.SetValue(element, "Use", use.ToString());
        }

        protected override void LoadXml(XmlElement element)
        {
            base.LoadXml(element);

            address = XmlHelper.GetValue(element, "Address", address.ToString());
            isReadCommand = Convert.ToBoolean(XmlHelper.GetValue(element, "IsReadCommand", isReadCommand.ToString()));
            sizeWord = Convert.ToInt32(XmlHelper.GetValue(element, "SizeWord", sizeWord.ToString()));
        }

        public override string ToString()
        {
            return $"Comm: {this.command}, Addr: {this.address}, Size: {this.sizeWord}";
        }
    }

    public class MelsecInfo
    {
        byte networkNo;
        byte plcNo;
        short moduleIoNo;
        byte moduleStationNo;
        short waitTime;
        bool isAsciiType;

        public byte NetworkNo
        {
            get { return networkNo; }
            set { networkNo = value; }
        }

        public byte PlcNo
        {
            get { return plcNo; }
            set { plcNo = value; }
        }

        public short ModuleIoNo
        {
            get { return moduleIoNo; }
            set { moduleIoNo = value; }
        }

        public byte ModuleStationNo
        {
            get { return moduleStationNo; }
            set { moduleStationNo = value; }
        }

        public short WaitTime
        {
            get { return waitTime; }
            set { waitTime = value; }
        }

        public bool IsAsciiType
        {
            get { return isAsciiType; }
            set { isAsciiType = value; }
        }

        public MelsecInfo()
        {
            this.networkNo = 0x00;
            this.plcNo = 0xff;
            this.moduleIoNo = 0x03FF;
            this.moduleStationNo = 0x0000;
            this.waitTime = 0x0010;
            this.isAsciiType = false;
        }

        public MelsecInfo Clone()
        {
            MelsecInfo melsecInfo = new MelsecInfo();
            melsecInfo.CopyFrom(this);
            return melsecInfo;
        }

        private void CopyFrom(MelsecInfo melsecInfo)
        {
            networkNo = melsecInfo.networkNo;
            plcNo = melsecInfo.plcNo;
            moduleIoNo = melsecInfo.moduleIoNo;
            moduleStationNo = melsecInfo.moduleStationNo;
            waitTime = melsecInfo.waitTime;
            isAsciiType = melsecInfo.isAsciiType;
        }

        internal void SaveXml(XmlElement xmlElement, string key = null)
        {
            if (xmlElement == null)
                return;

            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                SaveXml(subElement);
                return;
            }

            XmlHelper.SetValue(xmlElement, "NetworkNo", networkNo.ToString("X02"));
            XmlHelper.SetValue(xmlElement, "PcNo", plcNo.ToString("X02"));
            XmlHelper.SetValue(xmlElement, "ModuleIoNo", moduleIoNo.ToString("X04"));
            XmlHelper.SetValue(xmlElement, "ModuleStationNo", moduleStationNo.ToString("X02"));
            XmlHelper.SetValue(xmlElement, "WaitTime", waitTime.ToString("X04"));
            XmlHelper.SetValue(xmlElement, "IsAsciiType", isAsciiType.ToString());
        }

        internal void LoadXml(XmlElement xmlElement, string key = null)
        {
            if (xmlElement == null)
                return;

            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = xmlElement[key];
                LoadXml(subElement);
                return;
            }

            networkNo = Convert.ToByte(XmlHelper.GetValue(xmlElement, "NetworkNo", networkNo.ToString()), 16);
            plcNo = Convert.ToByte(XmlHelper.GetValue(xmlElement, "PcNo", plcNo.ToString()), 16);
            moduleIoNo = Convert.ToInt16(XmlHelper.GetValue(xmlElement, "ModuleIoNo", moduleIoNo.ToString()), 16);
            moduleStationNo = Convert.ToByte(XmlHelper.GetValue(xmlElement, "ModuleStationNo", moduleStationNo.ToString()), 16);
            waitTime = Convert.ToInt16(XmlHelper.GetValue(xmlElement, "WaitTime", waitTime.ToString()), 16);
            isAsciiType = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "IsAsciiType", isAsciiType.ToString()));
        }

        public override bool Equals(object obj)
        {
            if (obj is MelsecInfo)
            {
                MelsecInfo melsecInfo = (MelsecInfo)obj;
                return (melsecInfo.networkNo == networkNo
                    && melsecInfo.plcNo == plcNo
                    && melsecInfo.moduleIoNo == moduleIoNo
                    && melsecInfo.moduleStationNo == moduleStationNo
                    && melsecInfo.isAsciiType == isAsciiType
                    );
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class MelsecMachineIfSetting : TcpIpMachineIfSetting
    {
        MelsecInfo melsecInfo = new MelsecInfo();

        public MelsecInfo MelsecInfo
        {
            get { return melsecInfo; }
            set { melsecInfo = value; }
        }

        public MelsecMachineIfSetting(MachineIfSetting machineIfSetting) : base(machineIfSetting) { }
        public MelsecMachineIfSetting() : base(MachineIfType.Melsec) { }

        public override MachineIfSetting Clone()
        {
            MelsecMachineIfSetting melsecMachineIfSetting = new MelsecMachineIfSetting(this);
            return melsecMachineIfSetting;
        }

        public override void CopyFrom(MachineIfSetting src)
        {
            base.CopyFrom(src);

            MelsecMachineIfSetting melsecMachineIfSetting = (MelsecMachineIfSetting)src;
            this.tcpIpInfo = melsecMachineIfSetting.tcpIpInfo.Clone();
            this.melsecInfo = melsecMachineIfSetting.melsecInfo.Clone();
        }


        protected override void LoadXml(XmlElement xmlElement)
        {
            tcpIpInfo.Load(xmlElement, "TcpIpInfo");
            melsecInfo.LoadXml(xmlElement, "MelsecInfo");
        }

        protected override void SaveXml(XmlElement xmlElement)
        {
            tcpIpInfo.Save(xmlElement, "TcpIpInfo");
            melsecInfo.SaveXml(xmlElement, "MelsecInfo");
        }
    }

    // TCP/IP 기반이므로 PacketParser 필요함
    public class MelsecMachineIfBinaryPacketParser : TcpIpMachineIfPacketParser
    {
        byte[] stx = new byte[2] { 0xD0, 0x00 };
        MelsecMachineIfSetting melsecMachineIfSetting = null;

        public MelsecMachineIfBinaryPacketParser(MelsecMachineIfSetting melsecMachineIfSetting) : base()
        {
            this.melsecMachineIfSetting = melsecMachineIfSetting;
        }

        public override PacketParser Clone()
        {
            return new MelsecMachineIfBinaryPacketParser((MelsecMachineIfSetting)melsecMachineIfSetting.Clone());
        }

        public override bool ParsePacket(byte[] packetContents, out int processedPacketCnt)
        {
            float matchScore = 0;

            int startPos = this.IndexOf(packetContents, stx, out matchScore);
            if (startPos < 0)
            {
                processedPacketCnt = packetContents.Length;
                return false;
            }
            if (matchScore < 1)
            {
                processedPacketCnt = startPos;
                return false;
            }

            byte[] contents = packetContents.Skip(startPos).ToArray();
            if (contents.Length < 11)
            {
                processedPacketCnt = startPos;
                return false;
            }

            // Parse Access route
            MelsecInfo melsecInfo = new MelsecInfo();
            melsecInfo.IsAsciiType = false;
            melsecInfo.NetworkNo = contents[2];
            melsecInfo.PlcNo = contents[3];
            melsecInfo.ModuleIoNo = BitConverter.ToInt16(contents.Skip(4).Take(2).ToArray(), 0);
            melsecInfo.ModuleStationNo = contents[6];

            // Parse Responce Data length
            short dataLength = BitConverter.ToInt16(contents.Skip(7).Take(2).ToArray(), 0);
            int realLenght = contents.Length - 6;
            if (dataLength > realLenght)
            {
                processedPacketCnt = startPos;
                return false;
            }

            // Parse End Code
            ushort endCode = BitConverter.ToUInt16(contents.Skip(9).Take(2).ToArray(), 0);
            if (endCode != 0)
            {
                LogHelper.Error(LoggerType.Error, string.Format("MelsecMachineIfPacketParser::ParsePacket - EndCode is {0}", endCode.ToString("X")));
            }

            byte[] data = contents.Skip(11).Take(dataLength - 2).ToArray();

            // Chack
            //Debug.WriteLine(string.Format("[{0}]MelsecMachineIfPacketParser::ParsedData - {1}", DateTime.Now.ToLongTimeString(), data));
            if (melsecMachineIfSetting.MelsecInfo.Equals(melsecInfo))
            {
                this.OnDataReceived(new ReceivedPacket(contents.Take(9 + dataLength).ToArray(), endCode.ToString("X04")) { Valid = true });
            }

            processedPacketCnt = startPos + 7 + dataLength; // packet last position
            return true;
        }

        public override string MakePacket(MachineIfProtocol machineIfProtocol, params string[] args)
        {
            MelsecMachineIfProtocol melsecMachineIfProtocol = (MelsecMachineIfProtocol)machineIfProtocol;

            byte deviceCode = GetDeviceCodeBinary(melsecMachineIfProtocol.Address);
            byte[] deviceNumber = BitConverter.GetBytes(int.Parse(melsecMachineIfProtocol.Address.Substring(1)));

            // Header
            // 3E Frame
            List<byte> frame3E = new List<byte>();

            List<byte> header = new List<byte>();
            header.AddRange(BitConverter.GetBytes(Convert.ToInt16("0050", 16)));
            header.Add(melsecMachineIfSetting.MelsecInfo.NetworkNo);
            header.Add(melsecMachineIfSetting.MelsecInfo.PlcNo);
            header.AddRange(BitConverter.GetBytes(melsecMachineIfSetting.MelsecInfo.ModuleIoNo));
            header.Add(melsecMachineIfSetting.MelsecInfo.ModuleStationNo);

            //string commandHeader = COMMAND_HEADER + melsecInfo.NetworkNo + melsecInfo.PlcNo + melsecInfo.ModuleIoNo + melsecInfo.ModuleDeviceNo;

            // Data
            List<byte> command = new List<byte>();
            int sizeWord = melsecMachineIfProtocol.SizeWord;
            if (melsecMachineIfProtocol.IsReadCommand)
            {
                command.AddRange(BitConverter.GetBytes(melsecMachineIfSetting.MelsecInfo.WaitTime));
                command.AddRange(BitConverter.GetBytes((short)0x0401));   // Batch Read
                command.AddRange(BitConverter.GetBytes((short)0x0000));   // SubCommand

                command.Add(deviceNumber[0]);
                command.Add(deviceNumber[1]);
                command.Add(deviceNumber[2]);
                command.Add(deviceCode);

                command.AddRange(BitConverter.GetBytes((short)sizeWord));
            }
            else
            {
                command.AddRange(BitConverter.GetBytes(melsecMachineIfSetting.MelsecInfo.WaitTime));

                command.AddRange(BitConverter.GetBytes((short)0x1401));   // Batch Write
                command.AddRange(BitConverter.GetBytes((short)0x0000));   // SubCommand

                command.Add(deviceNumber[0]);
                command.Add(deviceNumber[1]);
                command.Add(deviceNumber[2]);
                command.Add(deviceCode);

                command.AddRange(BitConverter.GetBytes((short)sizeWord));

                // Create Write Data
                List<byte> dataArray = new List<byte>();
                int sizeChar = sizeWord * 4;    // 0x0000 -> 4 chars

                string totalArgs = string.Join("", args);
                string inputData = totalArgs.PadLeft(sizeChar, '0');
                if (inputData.Length > sizeChar)
                    inputData = inputData.Substring(inputData.Length - sizeChar, sizeChar);
                for (int i = 0; i < inputData.Length; i += 4)
                {
                    try
                    {
                        dataArray.AddRange(BitConverter.GetBytes(Convert.ToInt16(inputData.Substring(i, 4), 16)));
                    }
                    catch (FormatException)
                    {
                        dataArray.Add(0x00);
                        dataArray.Add(0x00);
                    }
                }

                command.AddRange(dataArray);
            }

            frame3E.AddRange(header);
            frame3E.AddRange(BitConverter.GetBytes((short)command.Count));
            //frame3E.AddRange(BitConverter.GetBytes(melsecMachineIfSetting.MelsecInfo.WaitTime));
            frame3E.AddRange(command);
            return BitConverter.ToString(frame3E.ToArray());
        }

        public override MachineIfProtocolWithArguments BreakPacket(string packet)
        {
            //MelsecMachineIfProtocol melsecMachineIfProtocol = (MelsecMachineIfProtocol)SystemManager.Instance().MachineIfProtocolList.GetProtocol((Enum)null);
            MelsecMachineIfProtocol melsecMachineIfProtocol = new MelsecMachineIfProtocol(null);
            string data = packet.Replace("-", "").Substring(22);
            return new MachineIfProtocolWithArguments(melsecMachineIfProtocol,new string[] { data });
        }

        public override byte[] EncodePacket(string protocol)
        {
            string packet = protocol.Replace("-", "");
            List<byte> packets = new List<byte>();
            for (int i = 0; i < packet.Length; i += 2)
            {
                packets.Add(Convert.ToByte(packet.Substring(i, 2), 16));
            }
            return packets.ToArray();
        }

        public override string DecodePacket(byte[] packet)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(packet[0].ToString("X2"));
            sb.Append(packet[1].ToString("X2"));
            sb.Append(packet[2].ToString("X2"));
            sb.Append(packet[3].ToString("X2"));
            sb.Append(BitConverter.ToInt16(packet.Skip(4).Take(2).ToArray(), 0).ToString("X04"));
            sb.Append(packet[6].ToString("X2"));
            sb.Append(BitConverter.ToInt16(packet.Skip(7).Take(2).ToArray(), 0).ToString("X04"));
            sb.Append(BitConverter.ToInt16(packet.Skip(9).Take(2).ToArray(), 0).ToString("X04"));
            if (sb.ToString().Substring(sb.Length - 4) != "0000")
            {
                sb.Append(packet[11].ToString("X2"));
                sb.Append(packet[12].ToString("X2"));
                sb.Append(BitConverter.ToInt16(packet.Skip(13).Take(2).ToArray(), 0).ToString("X04"));
                sb.Append(packet[15].ToString("X2"));
                sb.Append(BitConverter.ToInt16(packet.Skip(16).Take(2).ToArray(), 0).ToString("X04"));
                sb.Append(BitConverter.ToInt16(packet.Skip(18).Take(2).ToArray(), 0).ToString("X04"));
            }
            else
            {
                for (int i = 11; i < packet.Length; i += 2)
                    sb.Append(BitConverter.ToInt16(packet.Skip(i).Take(2).ToArray(), 0).ToString("X04"));

            }
            return sb.ToString();
        }

        private byte GetDeviceCodeBinary(string deviceName)
        {
            switch (deviceName[0])
            {
                case 'D': return 0xA8;  // 데이터 레지스터
                case 'M': return 0x90;  // 내부 릴레이
                case 'X': return 0x9C;  // 입력 릴레이
                case 'Y': return 0x9D;  // 출력 릴레이
                case 'R': return 0xAF;  // 파일 레지스터
                case 'Z': return 0xB0;  // 파일 레지스터
            }

            throw new ArgumentOutOfRangeException();
        }
    }


    // TCP/IP 기반이므로 PacketParser 필요함
    public class MelsecMachineIfAsciiPacketParser : TcpIpMachineIfPacketParser
    {
        byte[] stx = Encoding.Default.GetBytes("D000");
        MelsecMachineIfSetting melsecMachineIfSetting = null;

        public MelsecMachineIfAsciiPacketParser(MelsecMachineIfSetting melsecMachineIfSetting) : base()
        {
            this.melsecMachineIfSetting = melsecMachineIfSetting;
        }

        public override PacketParser Clone()
        {
            return new MelsecMachineIfAsciiPacketParser((MelsecMachineIfSetting)melsecMachineIfSetting.Clone());
        }

        public override bool ParsePacket(byte[] packetContents, out int processedPacketCnt)
        {
            float matchScore = 0;
            string str = Encoding.Default.GetString(packetContents);
            int startPos = this.IndexOf(packetContents, stx, out matchScore);
            if (startPos < 0)
            {
                processedPacketCnt = packetContents.Length;
                return false;
            }
            if (matchScore < 1)
            {
                processedPacketCnt = startPos;
                return false;
            }

            byte[] skipContents = packetContents.Skip(startPos).ToArray();
            string clipContentsStr = Encoding.UTF8.GetString(skipContents);

            const int headerSize = 14;
            if (clipContentsStr.Length < headerSize)
            {
                processedPacketCnt = startPos;
                return false;
            }

            // Parse Access route
            MelsecInfo melsecInfo = new MelsecInfo();
            melsecInfo.IsAsciiType = true;
            melsecInfo.NetworkNo = Convert.ToByte(clipContentsStr.Substring(4, 2), 16);
            melsecInfo.PlcNo = Convert.ToByte(clipContentsStr.Substring(6, 2), 16);
            melsecInfo.ModuleIoNo = Convert.ToInt16(clipContentsStr.Substring(8, 4), 16);
            melsecInfo.ModuleStationNo = Convert.ToByte(clipContentsStr.Substring(12, 2), 16);

            // Parse Responce Data length
            int length = Convert.ToInt32(clipContentsStr.Substring(14, 4), 16);
            int remain = clipContentsStr.Substring(18).Length;
            if (length > remain)
            {
                processedPacketCnt = startPos;
                return false;
            }

            // Parse End Code
            string endCode = clipContentsStr.Substring(18, 4);
            if (endCode != "0000")
            {
                LogHelper.Error(LoggerType.Error, string.Format("MelsecMachineIfPacketParser::ParsePacket - EndCode is {0}", endCode));
                //   throw new Exception("EndCode is NOT 0");
            }

            string data = clipContentsStr.Substring(22, length - 4);

            // Chack
            //Debug.WriteLine(string.Format("[{0}]MelsecMachineIfPacketParser::ParsedData - {1}", DateTime.Now.ToLongTimeString(), data));
            if (melsecMachineIfSetting.MelsecInfo.Equals(melsecInfo))
                this.OnDataReceived(new ReceivedPacket(skipContents.Take(headerSize + 4 + length).ToArray()));

            processedPacketCnt = startPos + 4 + headerSize + length; // packet last position
            return true;
        }

        public override string MakePacket(MachineIfProtocol machineIfProtocol, params string[] args)
        {
            //Debug.WriteLine(string.Format("MelsecMachineIf::SendCommand: {0}", protocol.ToString()));

            MelsecMachineIfProtocol melsecMachineIfProtocol = (MelsecMachineIfProtocol)machineIfProtocol;

            string deviceCode = string.Format("{0}*", melsecMachineIfProtocol.Address.Substring(0, 1));
            int deviceNumber = int.Parse(melsecMachineIfProtocol.Address.Substring(1));
            //Array.ForEach(melsecMachineIfProtocol.Args, f => sendData += f);
            // Header
            // 3E Frame
            StringBuilder sbHeader = new StringBuilder();
            sbHeader.Append("5000");
            sbHeader.Append(melsecMachineIfSetting.MelsecInfo.NetworkNo.ToString("X02"));
            sbHeader.Append(melsecMachineIfSetting.MelsecInfo.PlcNo.ToString("X02"));
            sbHeader.Append(melsecMachineIfSetting.MelsecInfo.ModuleIoNo.ToString("X04"));
            sbHeader.Append(melsecMachineIfSetting.MelsecInfo.ModuleStationNo.ToString("X02"));
            string header = sbHeader.ToString();

            //string commandHeader = COMMAND_HEADER + melsecInfo.NetworkNo + melsecInfo.PlcNo + melsecInfo.ModuleIoNo + melsecInfo.ModuleDeviceNo;

            // Data
            StringBuilder sbData = new StringBuilder();
            int sizeWord = melsecMachineIfProtocol.SizeWord;
            if (melsecMachineIfProtocol.IsReadCommand)
            {
                sbData.Append(melsecMachineIfSetting.MelsecInfo.WaitTime.ToString("X04"));
                sbData.Append("0401");  // Batch Read
                sbData.Append("0000");
                sbData.Append(deviceCode);
                sbData.Append(string.Format("{0:D06}", deviceNumber));
                sbData.Append(sizeWord.ToString("X04"));
            }
            else
            {
                // Create Write Data
                int length = melsecMachineIfProtocol.SizeWord * 4;
                string writeData = new string('0', length);
                if (args != null && args.Length > 0)
                    writeData = args[0].PadLeft(length, '0');

                sbData.Append(melsecMachineIfSetting.MelsecInfo.WaitTime.ToString("X04"));
                sbData.Append("1401");  // Batch Write
                sbData.Append("0000");
                sbData.Append(deviceCode);
                sbData.Append(string.Format("{0:D06}", deviceNumber));
                sbData.Append(sizeWord.ToString("X04"));
                sbData.Append(writeData);
            }

            string data = sbData.ToString();
            string packet = header + data.Length.ToString("X4") + data;

            return packet;
        }

        public override MachineIfProtocolWithArguments BreakPacket(string packet)
        {
            MelsecMachineIfProtocol melsecMachineIfProtocol = new MelsecMachineIfProtocol(null);
            string data = packet.Substring(22);
            return new MachineIfProtocolWithArguments( melsecMachineIfProtocol, new string[] { data });
        }
    }

    public class MelsecMachineIf : TcpIpMachineIfClient
    {
        //MelsecMachineIfThread machineIfThread = null;

        public MelsecMachineIf(MachineIfSetting machineIfSetting) : base(machineIfSetting) { }

        public static MelsecMachineIf Create(MachineIfSetting machineIfSetting)
        {
            if (machineIfSetting.IsVirtualMode)
                return new MelsecMachineIfVirtual(machineIfSetting);
            else
                return new MelsecMachineIf(machineIfSetting);
        }

        public override void Initialize()
        {
            MelsecMachineIfSetting melsecMachineIfSetting = (MelsecMachineIfSetting)machineIfSetting;

            this.clientSocket = new SinglePortSocket(0, null);
            PacketParser packetParser;
            if (melsecMachineIfSetting.MelsecInfo.IsAsciiType)
                packetParser = new MelsecMachineIfAsciiPacketParser(melsecMachineIfSetting);
            else
                packetParser = new MelsecMachineIfBinaryPacketParser(melsecMachineIfSetting);

            BuildPacketParser(packetParser);
            this.clientSocket.PacketHandler = new PacketHandler(packetParser);
            this.clientSocket.Init(melsecMachineIfSetting.TcpIpInfo);

            this.clientSocket.StartConnection();            

        }

        protected override void BuildPacketParser(PacketParser packetParser)
        {
            TcpIpMachineIfPacketParser melsecMachineIfPacketParser = (TcpIpMachineIfPacketParser)packetParser;
            packetParser.DataReceived += packetParser_OnDataReceived;
            MakePacket = melsecMachineIfPacketParser.MakePacket;
            BreakPacket = melsecMachineIfPacketParser.BreakPacket;
        }

        private void packetParser_OnDataReceived(ReceivedPacket receivedPacket)
        {
            string packetString = this.clientSocket.PacketHandler.PacketParser.DecodePacket(receivedPacket.ReceivedData);
            MachineIfProtocolWithArguments machineIfProtocol = BreakPacket(packetString);

            string errCode = packetString.Substring(18, 4);
            string recivedData = packetString.Substring(22);
            if (protocolResponce != null || protocolResponce.IsResponced == false)
                protocolResponce.SetRecivedData(recivedData, errCode == "0000", receivedPacket);
            //protocolResponce = null;
        }

        /// <summary>
        /// Melsec은 클라이언트 이나, 응답을 대기한다!
        /// </summary>
        /// <param name="protocol"></param>
        /// <param name="args"></param>
        protected override bool Send(MachineIfProtocol protocol, params string[] args)
        {
            return base.Send(protocol, args);
        }
    }

    //public class MelsecMachineIfThread : ThreadHandler
    //{
    //    MelsecMachineIf melsecMachineIf = null;
    //    ManualResetEvent isStoped = new ManualResetEvent(true);

    //    public MelsecMachineIfThread(MelsecMachineIf melsecMachineIf)
    //    {
    //        this.melsecMachineIf = melsecMachineIf;
    //    }

    //    public new void Start()
    //    {
    //        Debug.WriteLine(string.Format("MelsecMachineIfThread::Start"));
    //        RequestStop = false;
    //        this.WorkingThread = new System.Threading.Thread(Proc);
    //        this.WorkingThread.Start();
    //    }

    //    private void Proc()
    //    {
    //        isStoped.Reset();
    //        while (RequestStop == false)
    //        {
    //            Thread.Sleep(500);

    //            if (melsecMachineIf.Connected == false)
    //                continue;

    //            MachineIfProtocol protocol = SystemManager.Instance().MachineIfProtocolList.GetProtocol(null);
    //            //melsecMachineIf.Send(protocol);


    //            //TcpIpMachineIfProtocolResponce tcpIpMachineIfProtocolResponce = melsecMachineIf.ProtocolResponce;
    //            //melsecMachineIf.ExecuteCommand(tcpIpMachineIfProtocolResponce.ReciveData);
    //        }
    //        isStoped.Set();
    //    }

    //    public new void Stop()
    //    {
    //        Debug.WriteLine(string.Format("MelsecMachineIfThread::Stop"));
    //        RequestStop = true;
    //    }

    //    public bool WaitStop(int waitTimeMs)
    //    {
    //        isStoped.WaitOne(waitTimeMs);
    //        return (this.WorkingThread.IsAlive == false);
    //    }
    //}

    public class MelsecMachineIfVirtual : MelsecMachineIf, IVirtualMachineIf
    {
        public override bool IsConnected => this.isConnected;
        bool isConnected = true;

        public MelsecMachineIfVirtual(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
        }

        public void SetStateConnect(bool connect)
        {
            this.isConnected = connect;
        }

        public override void Initialize()
        {
        }

        public override void Release()
        {
        }

        protected override bool Send(MachineIfProtocol protocol, params string[] args)
        {
            return false;
            return true;
        }
    }
}
