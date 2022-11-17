using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

using DynMvp.Base;

namespace DynMvp.Devices.Comm
{
    public delegate void ClientEventDelegate(ClientHandlerSocket clientHandlerSocket);

    public class SimpleServerSocket
    {
        private string host = "localhost";
        private int dataPort;

        public PacketHandler ListeningPacketHandler { get => this.listeningPacketHandler; set => this.listeningPacketHandler = value; }
        private PacketHandler listeningPacketHandler = new PacketHandler();

        public event ClientEventDelegate ClientConnected;
        public event ClientEventDelegate ClientDisconnected;

        Mutex commandMutex = new Mutex();

        Socket listeningSocket = null;

        List<ClientHandlerSocket> clientList = new List<ClientHandlerSocket>();

        public SimpleServerSocket()
        {
        
        }

        public void Setup(TcpIpInfo tcpIpInfo)
        {
            Setup(tcpIpInfo.IpAddress, tcpIpInfo.PortNo);
        }

        public void Setup(string host, int dataPort)
        {
            if (String.IsNullOrEmpty(host))
            {
                LogHelper.Debug(LoggerType.Network, "Host address is empty");
                return;
            }

            this.host = host;
            this.dataPort = dataPort;
        }

        public void Close()
        {
            this.clientList.ForEach(f => f.BeginStop());
            this.clientList.Clear();
        }

        public void StartListening()
        {
            try
            {
                IPAddress ipAddr = IPAddress.Parse(host);
                
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, dataPort);
                LogHelper.Debug(LoggerType.Network, String.Format("SimpleServerSocket::StartListening - Host IP : {0}", ipEndPoint.Address));

                this.listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                this.listeningSocket.Bind(ipEndPoint);

                this.listeningSocket.Listen(50);

                this.listeningSocket.BeginAccept(new AsyncCallback(AcceptCallback), listeningSocket);
            }
            catch (SocketException e)
            {
                LogHelper.Error(LoggerType.Network, String.Format("SimpleServerSocket::StartListening - Server Error : {0}", e.Message));
            }
        }

        public void StopListening()
        {
            if (this.listeningSocket != null)
            {
                if (this.listeningSocket.Connected)
                    this.listeningSocket.Shutdown(SocketShutdown.Both);
                this.listeningSocket.Close();
            }
            this.listeningSocket = null;
        }

        public void SendCommand(PacketParser command)
        {
            foreach (ClientHandlerSocket handlerSocket in clientList)
                handlerSocket.SendCommand(command);
        }

        public bool SendCommand(byte[] commandPacket)
        {
            lock (clientList)
            {
                foreach (ClientHandlerSocket handlerSocket in clientList)
                    handlerSocket.SendCommand(commandPacket);
            }
            return true;
        }

        public void RemoveClientSocket(ClientHandlerSocket clientHandlerSocket)
        {
            lock (clientList)
                clientList.Remove(clientHandlerSocket);

            if (ClientDisconnected != null)
                ClientDisconnected(clientHandlerSocket);
        }

        public void Stop()
        {
            StopListening();
        }

        // 가상 모드의 동작을 위해 별도 함수로 분리
        public bool ProcessDataPacket(byte[] receiveBuf)
        {
            return true; 
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            //allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            try
            {
                Socket handler = listener.EndAccept(ar);

                List<byte> list = new List<byte>();
                list.AddRange((BitConverter.GetBytes(1)));
                list.AddRange(BitConverter.GetBytes(100)); //Send a packet once every 10 seconds.
                list.AddRange(BitConverter.GetBytes(100));  //If no response, resend every second.
                handler.IOControl(IOControlCode.KeepAliveValues, list.ToArray(), null);

                Accept(handler);
            }
            catch (ObjectDisposedException) { }
            this.listeningSocket?.BeginAccept(new AsyncCallback(AcceptCallback), listeningSocket);
        }

        private void Accept(Socket socket)
        {
            ClientHandlerSocket clientHandlerSocket = new ClientHandlerSocket(this);
            clientHandlerSocket.SetIAmHere(10, new byte[] { 0 });
            clientHandlerSocket.PacketHandler.PacketParser = listeningPacketHandler.PacketParser.Clone();
            clientHandlerSocket.OnSocketClosing += RemoveClientSocket;
            clientHandlerSocket.Start(socket);
            
            lock (clientList)
                clientList.Add(clientHandlerSocket);

            ClientConnected?.Invoke(clientHandlerSocket);
        }
    }
}
