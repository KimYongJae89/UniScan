using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace DynMvp.Device.Device.Comm
{
    public class AsyncClientSocket : IDisposable
    {
        private TcpIpInfo tcpIpInfo;
        private string name = "";
        public string Name
        {
            get { return name; }
        }

        public DataReceivedDelegate DataReceived;

        PacketBuffer packetBuffer = new PacketBuffer();
        public PacketBuffer PacketBuffer
        {
            get { return packetBuffer; }
        }

        private IProtocol protocol;

        Mutex commandMutex = new Mutex();

        Socket socket = null;
        Socket callbackSocket = null;

        bool onWaitResponse = false;
        bool onReconect = false;
        public bool OnReconnect
        {
            get { return onReconect; }
        }
        List<SendPacket> sendPacketList = new List<SendPacket>();
        object clienctLockObj = new object();
        SendPacket lastSendPacket;

        private byte[] receiveBuffer;
        private const int MAXSIZE = 8192;

        Task reconnectTask;
        Task sendTask;

        bool connected = false;
        public bool Connected
        {
            get
            {
                if (connected == false)
                {
                    if (socket != null && socket.Connected)
                    {
                        Thread.Sleep(1000);
                        connected = true;
                    }
                }

                return connected;
            }
        }

        public AsyncClientSocket(IProtocol protocol)
        {
            this.protocol = protocol;
            receiveBuffer = new byte[MAXSIZE];
        }
        public AsyncClientSocket(IProtocol protocol, string name)
        {
            this.protocol = protocol;
            receiveBuffer = new byte[MAXSIZE];
            this.name = name;
        }

        ~AsyncClientSocket()
        {
            Close(true);
        }

        public void Dispose()
        {
            Close(true);
        }

        public void Connect(TcpIpInfo tcpIpInfo)
        {
            this.tcpIpInfo = tcpIpInfo;
            Connect();
        }

        public void Connect()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.NoDelay = true;
            BeginConnect();
        }

        public void Reconnect()
        {
            if (onReconect)
                return;
            LogHelper.Debug(LoggerType.Network, string.Format("socket try reconnect. : {0}", name));
            onReconect = true;
            reconnectTask = new Task(new Action(ReconnectProc));
            reconnectTask.Start();
        }

        void ReconnectProc()
        {
            while (true)
            {
                if (IsPingAlive() == false)
                {
                    LogHelper.Debug(LoggerType.Network, string.Format("Ping time out : {0}", name));
                    continue;
                }
                try
                {
                    if (socket != null)
                    {
                        socket.Disconnect(true);
                        socket.Close();
                        socket.Dispose();
                        if (callbackSocket != null)
                        {
                            callbackSocket.Disconnect(true);
                            callbackSocket.Close();
                            callbackSocket.Dispose();
                        }

                        connected = false;
                        socket = null;
                        callbackSocket = null;
                    }

                }
                catch (Exception e)
                {
                    LogHelper.Debug(LoggerType.Network, "socket disconnect failed.");
                    if (socket != null)
                        socket.Dispose();
                    if (callbackSocket != null)
                        callbackSocket.Dispose();
                    socket = null;
                    callbackSocket = null;
                    connected = false;
                }
                if (IsPingAlive())
                {
                    Thread.Sleep(1000);
                    Connect();
                    Thread.Sleep(1500);

                }
                if (socket != null && callbackSocket != null)
                    if (socket.Connected && callbackSocket.Connected)
                        break;
                LogHelper.Debug(LoggerType.Error, string.Format("Reconnect Time out - {0}", name));
            }
            lock (clienctLockObj)
            {
                receiveBuffer = new byte[MAXSIZE];
                sendPacketList.Clear();
                onWaitResponse = false;
                LogHelper.Debug(LoggerType.Network, string.Format("Reconnect Success! - {0}", name));
                onReconect = false;
                connected = true;
            }
        }

        public void BeginConnect()
        {
            LogHelper.Debug(LoggerType.Network, string.Format("Wait connect the server - {0}", name));

            try
            {
                socket.BeginConnect(tcpIpInfo.IpAddress, tcpIpInfo.PortNo, new AsyncCallback(ConnectCallback), socket);
            }
            catch (SocketException se)
            {
                LogHelper.Error(LoggerType.Network, string.Format("Can't connect the server - {0} : {1}", name, se.Message));
                Reconnect();
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket tempSock = (Socket)ar.AsyncState;
                tempSock.EndConnect(ar);
                callbackSocket = tempSock;
                receiveBuffer = new byte[MAXSIZE];
                callbackSocket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None,
                                    new AsyncCallback(OnReceiveCallBack), callbackSocket);

            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Network, string.Format("Can't connect the server - {0} : {1}", name, e.Message));
                Reconnect();
            }
        }

        public void Close(bool bFinalize)
        {
            LogHelper.Debug(LoggerType.Network, "Close Socket");

            if (socket != null)
            {
                try
                {
                    socket.Close();
                    connected = false;
                }
                catch (SocketException se)
                {
                    LogHelper.Error(LoggerType.Network, string.Format("Fail to disconnect the server - {0}",name, se.Message));
                }
                socket = null;
            }
        }

        public void ClearBuffer()
        {
            if (socket != null)
            {
                lastSendPacket = null;
                sendPacketList.Clear();

                int dataAvailable = socket.Available;

                if (dataAvailable > 0)
                {
                    byte[] receiveBuf = new byte[dataAvailable];
                    socket.Receive(receiveBuf);
                }
            }
        }

        public void SendCommand(SendPacket sendPacket)
        {
            if (sendPacket.IsValid() == false)
                return;

            if (onReconect)
                return;
            lock (sendPacketList)
            {
                sendPacketList.Add(sendPacket);
            }
            SendCommand();
        }

        private void SendCommand()
        {
            if (onReconect)
                return;
            try
            {
                if (sendPacketList.Count > 20)
                    LogHelper.Debug(LoggerType.Network, string.Format("sendPacketList : {0} - {1}", sendPacketList.Count, name));
                if (IsPingAlive() == false)
                {
                    LogHelper.Error(LoggerType.Network, string.Format("Can't connect the server. Try to reconnect - {0}", name));
                    Reconnect();
                    return;
                }

                if (Connected)
                {
                    if (onWaitResponse == false && sendPacketList.Count > 0)
                    {

                        if (sendPacketList.Count == 0)
                            return;
                        SendPacket sendPacket = sendPacketList[0];
                        Thread.Sleep(10);
                        if (sendPacket == null)
                        {
                            if (sendPacketList.Count != 0)
                            {
                                LogHelper.Debug(LoggerType.Network, string.Format("Send packet is null"));
                                sendPacketList.RemoveAt(0);

                            }
                            return;
                        }

                        byte[] sendData = protocol.MakePacket(sendPacket);
                        socket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None,
                                            new AsyncCallback(SendCallBack), sendData);

                        lock (sendPacketList)
                        {
                            if (sendPacketList.Count != 0)
                            {
                                sendPacketList.RemoveAt(0);

                            }
                        }
                        lastSendPacket = sendPacket;
                        onWaitResponse = lastSendPacket.WaitResponse;

                    }
                }
                else
                {
                    Reconnect();
                }
            }
            catch (SocketException e)
            {
                LogHelper.Error(LoggerType.Network, string.Format("Fail to send data : {0}", e.Message));
                Reconnect();
            }
        }

        private void SendCallBack(IAsyncResult IAR)
        {
            if (IAR.IsCompleted == true)
            {
                byte[] message = (byte[])IAR.AsyncState;

                string fullString = Encoding.UTF8.GetString(message);
                int strLen = Math.Min(fullString.Length, 100);
            }
            else
            {
                LogHelper.Debug(LoggerType.Network, "Packet Sent Error");
            }
        }

        public void Receive()
        {
            if (onReconect)
                return;
            callbackSocket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None,
                                 new AsyncCallback(OnReceiveCallBack), callbackSocket);
        }

        private void OnReceiveCallBack(IAsyncResult iAr)
        {
            if (onReconect)
                return;
            try
            {
                Socket tempSock = (Socket)iAr.AsyncState;
                if (tempSock.Connected)
                {
                    int dataAvailable = tempSock.EndReceive(iAr);
                    if (dataAvailable != 0)
                    {
                        string message = new UTF8Encoding().GetString(receiveBuffer, 0, dataAvailable);
                        //LogHelper.Debug(LoggerType.Network, "Data received : " + message);

                        if (protocol != null)
                        {
                            packetBuffer.AppendData(receiveBuffer, dataAvailable);

                            ProcessPacketResult result;
                            AsyncRecivedPacket receivedPacket;

                            do
                            {
                                result = protocol.ParsePacket(packetBuffer, out receivedPacket);
                                if (result == ProcessPacketResult.Complete)
                                {
                                    if (DataReceived != null && receivedPacket.Valid)
                                    {
                                        receivedPacket.SendPacket = lastSendPacket;
                                        DataReceived(receivedPacket);
                                        receivedPacket.Valid = false;
                                    }
                                }

                                if (packetBuffer.Empty == true)
                                    break;
                            }
                            while (result == ProcessPacketResult.Complete);

                            if (result != ProcessPacketResult.Incomplete)
                            {
                                onWaitResponse = false;
                                lastSendPacket = null;
                                SendCommand();
                            }
                        }
                    }

                    this.Receive();
                }
                if(sendPacketList.Count > 0) // 만약에 보낼 데이터가 남아 있을 경우 다시 보낸다.
                    SendCommand();
            }
            catch (SocketException se)
            {
                LogHelper.Debug(LoggerType.Network, "OnReceiveCallBack Reconnect.");
                Reconnect();
            }
        }

        public bool IsIdle()
        {
            return onWaitResponse == false && sendPacketList.Count == 0;
        }

        public bool IsPingAlive()
        {
            bool ping = false;
            try
            {
                ping = new Microsoft.VisualBasic.Devices.Network().Ping(tcpIpInfo.IpAddress, 1000);
            }
            catch (Exception e)
            {
                LogHelper.Error(LoggerType.Network, string.Format("{0} is ping dead.", name));
                return false;
            }
            return ping;

        }
    }
}
