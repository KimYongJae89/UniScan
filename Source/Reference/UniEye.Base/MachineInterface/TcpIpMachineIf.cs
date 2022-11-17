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
    public class TcpIpMachineIfProtocol : MachineIfProtocol
    {
        public override bool IsValid => true;

        public TcpIpMachineIfProtocol(Enum command) : base(command, false, 2000)
        {
        }

        public TcpIpMachineIfProtocol(Enum command, bool use, int waitResponceMs) : base(command, use, waitResponceMs)
        {
        }

        public override MachineIfProtocol Clone()
        {
            TcpIpMachineIfProtocol tcpIpMachineIfProtocol = new TcpIpMachineIfProtocol(this.command, this.use, this.waitResponceMs);
            return tcpIpMachineIfProtocol;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(command);
            return sb.ToString();
        }

    }

    public class TcpIpMachineIfSetting : MachineIfSetting
    {
        protected TcpIpInfo tcpIpInfo;

        public TcpIpInfo TcpIpInfo
        {
            get { return tcpIpInfo; }
            set { tcpIpInfo = value; }
        }

        public TcpIpMachineIfSetting(MachineIfSetting machineIfSetting) : base(machineIfSetting) { }
        public TcpIpMachineIfSetting(MachineIfType machineIfType) : base(machineIfType)
        {
            this.tcpIpInfo = new TcpIpInfo("0.0.0.0", 0);
        }

        public override MachineIfSetting Clone()
        {
            TcpIpMachineIfSetting newSettings = new TcpIpMachineIfSetting(this);
            return newSettings;
        }

        public override void CopyFrom(MachineIfSetting src)
        {
            base.CopyFrom(src);

            TcpIpMachineIfSetting tcpIpMachineIfSetting = (TcpIpMachineIfSetting)src;
            this.tcpIpInfo = tcpIpMachineIfSetting.tcpIpInfo.Clone();
        }

        protected override void LoadXml(XmlElement xmlElement)
        {
            tcpIpInfo.Load(xmlElement, "TcpIpInfo");
        }

        protected override void SaveXml(XmlElement xmlElement)
        {
            tcpIpInfo.Save(xmlElement, "TcpIpInfo");
        }
    }

    public class TcpIpMachineIfPacketParser : SimplePacketParser
    {
        public TcpIpMachineIfPacketParser()
        {
        }

        public virtual string MakePacket(MachineIfProtocol machineIfProtocol, params string[] args)
        {
            TcpIpMachineIfProtocol tcpIpMachineIfProcotol = (TcpIpMachineIfProtocol)machineIfProtocol;
            StringBuilder sb = new StringBuilder();
            sb.Append(tcpIpMachineIfProcotol.Command.ToString());
            if (args != null)
            {
                foreach (string arg in args)
                    sb.AppendFormat(",{0}", arg);
            }
            string packet = sb.ToString();
            return packet;
        }

        public virtual MachineIfProtocolWithArguments BreakPacket(string packet)
        {
            string[] token = packet.Split(',');

            MachineIfProtocolList list = SystemManager.Instance().DeviceBox.MachineIf.MachineIfSetting.MachineIfProtocolList;
            //Enum e = (Enum)Enum.Parse(SystemManager.Instance().MachineIfProtocolList.ProtocolListType, token[0]);
            Enum e = list.GetEnum(token[0]);
            TcpIpMachineIfProtocol tcpIpMachineIfProcotol
                = (TcpIpMachineIfProtocol)list.GetProtocol(e);
            token = token.Skip(1).ToArray();

            return new MachineIfProtocolWithArguments(tcpIpMachineIfProcotol, token);
        }
    }

    public delegate string MakePacketDelegate(MachineIfProtocol machineIfProtocol, params string[] args);
    public delegate MachineIfProtocolWithArguments BreakPacketDelegate(string pakcet);
    public abstract class TcpIpMachineIf : MachineIf
    {
        protected MakePacketDelegate MakePacket = null;
        protected BreakPacketDelegate BreakPacket = null;

        public TcpIpMachineIf(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
        }

        protected virtual TcpIpMachineIfPacketParser CreatePacketParser()
        {
            return new TcpIpMachineIfPacketParser();
        }

        protected virtual void BuildPacketParser(PacketParser packetParser)
        {
            TcpIpMachineIfPacketParser tcpIpMachineIfPacketParser = (TcpIpMachineIfPacketParser)packetParser;
            tcpIpMachineIfPacketParser.StartChar = Encoding.ASCII.GetBytes("<START>");
            tcpIpMachineIfPacketParser.EndChar = Encoding.ASCII.GetBytes("<END>");

            MakePacket = tcpIpMachineIfPacketParser.MakePacket;
            BreakPacket = tcpIpMachineIfPacketParser.BreakPacket;
        }
    }

    public class TcpIpMachineIfClient : TcpIpMachineIf
    {
        public new TcpIpMachineIfSetting MachineIfSetting { get => (TcpIpMachineIfSetting)this.machineIfSetting; }

        public SinglePortSocket ClientSocket => this.clientSocket;
        protected SinglePortSocket clientSocket;

        public override bool IsConnected
        {
            get { return clientSocket == null ? false : this.clientSocket.Connected; }
        }

        public TcpIpMachineIfClient(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
        }

        public override void Initialize()
        {
            TcpIpInfo tcpIpInfo = ((TcpIpMachineIfSetting)this.machineIfSetting).TcpIpInfo;

            this.clientSocket = new SinglePortSocket(10, new byte[1] { 0 });

            TcpIpMachineIfPacketParser tcpIpMachineIfPacketParser = CreatePacketParser();
            BuildPacketParser(tcpIpMachineIfPacketParser);
            tcpIpMachineIfPacketParser.DataReceived += tcpIpMachineIfPacketParser_OnDataReceived;
            this.clientSocket.PacketHandler.PacketParser = tcpIpMachineIfPacketParser;
            this.clientSocket.Init(tcpIpInfo);
            this.clientSocket.OnConnectAsyncCompleted += ClientSocket_OnConnectAsyncCompleted;
            this.clientSocket.StartConnection();
        }

        protected virtual void ClientSocket_OnConnectAsyncCompleted() { }

        protected void tcpIpMachineIfPacketParser_OnDataReceived(ReceivedPacket receivedPacket)
        {
            string receivedString = Encoding.Default.GetString(receivedPacket.ReceivedData);
            //LogHelper.Debug(LoggerType.Network, string.Format("TcpIpMachineIfClient::OnDataReceived - {0}", receivedString));

            MachineIfProtocolWithArguments machineIfProtocol = BreakPacket(receivedString);
            this.ExecuteCommand(machineIfProtocol);
        }

        public override void Release()
        {
            clientSocket.StopConnection(true);
            clientSocket.StopListening(true);

            clientSocket.Close();
        }

        protected override bool Send(MachineIfProtocol protocol, params string[] args)
        {
            if (MakePacket == null)
                return false;

            string packetString = MakePacket(protocol, args);
            if (string.IsNullOrEmpty(packetString))
                return false;

            byte[] packetByte = this.clientSocket.PacketHandler.PacketParser.EncodePacket(packetString);
            return this.clientSocket.SendCommand(packetByte);
        }

        //public override void SendCommand(byte[] bytes)
        //{
        //    this.clientSocket.SendCommand(bytes);
        //}
    }

    public class TcpIpMachineIfServer : TcpIpMachineIf
    {
        protected SimpleServerSocket serverSocket;

        /// <summary>
        /// Server Open State
        /// </summary>
        public override bool IsConnected
        {
            get { return true; }
        }

        public TcpIpMachineIfServer(MachineIfSetting machineIfSetting) : base(machineIfSetting)
        {
            this.serverSocket = new SimpleServerSocket();
        }

        ~TcpIpMachineIfServer()
        {
            Release();
        }

        public override void Initialize()
        {
            TcpIpInfo tcpIpInfo = ((TcpIpMachineIfSetting)this.machineIfSetting).TcpIpInfo;

            TcpIpMachineIfPacketParser tcpIpMachineIfPacketParser = CreatePacketParser();
            BuildPacketParser(tcpIpMachineIfPacketParser);
            tcpIpMachineIfPacketParser.DataReceived += tcpIpMachineIfPacketParser_OnDataReceived;
            this.serverSocket.ListeningPacketHandler.PacketParser = tcpIpMachineIfPacketParser;

            this.serverSocket.Setup(tcpIpInfo);

            this.serverSocket.StartListening();
        }

        private void tcpIpMachineIfPacketParser_OnDataReceived(ReceivedPacket receivedPacket)
        {
            string receivedString = Encoding.Default.GetString(receivedPacket.ReceivedData);
            //LogHelper.Debug(LoggerType.Network, string.Format("TcpIpMachineIfServer::OnDataReceived - {0}", receivedString));

            MachineIfProtocolWithArguments machineIfProtocol = BreakPacket(receivedString);
            //protocolResponce = new MachineIfProtocolResponce();
            //protocolResponce.SetRecivedData(receivedString, false);
            this.ExecuteCommand(machineIfProtocol);
        }

        public override void Release()
        {
            serverSocket?.Stop();
            serverSocket?.Close();
        }

        protected override bool Send(MachineIfProtocol protocol, params string[] args)
        {
            string packetString = MakePacket?.Invoke(protocol, args);
            if (string.IsNullOrEmpty(packetString))
                return false;

            byte[] packetByte = this.serverSocket.ListeningPacketHandler.PacketParser.EncodePacket(packetString);

            return serverSocket.SendCommand(packetByte);
        }

        //public override void SendCommand(byte[] bytes)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
