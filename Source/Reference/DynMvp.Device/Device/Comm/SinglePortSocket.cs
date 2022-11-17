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
    public delegate void ConnectAsyncCompletedDelegate();
    public class SinglePortSocket
    {
        public event ConnectAsyncCompletedDelegate OnConnectAsyncCompleted;

        private TcpIpInfo tcpIpInfo;

        private byte[] iAmHereBytes = new byte[0];
        private int iAmHereInterval = -1;

        private bool stopConnectionThreadFlag = false;
        private Thread connectionThread;

        private bool stopListeningThreadFlag = false;
        private Thread listeningThread;

        PacketData packetData = new PacketData();
        public PacketData PacketData
        {
            get { return packetData; }
        }

        private PacketHandler packetHandler = new PacketHandler();
        public PacketHandler PacketHandler
        {
            get { return packetHandler; }
            set { packetHandler = value; }
        }

        IPEndPoint ipEndPoint = null;

        Mutex commandMutex = new Mutex();

        public Socket ClientSocket => this.clientSocket;
        Socket clientSocket = null;

        public bool Connected
        {
            get { return clientSocket == null ? false : clientSocket.Connected; }
        }

        public SinglePortSocket(int iAmHereInterval, byte[] iAmHereData)
        {
            this.iAmHereInterval = iAmHereInterval;
            if (iAmHereData == null)
                iAmHereData = new byte[0];
            this.iAmHereBytes = iAmHereData;
        }

        ~SinglePortSocket()
        {
            Close();
        }

        public void Init(TcpIpInfo tcpIpInfo)
        {
            stopConnectionThreadFlag = false;

            this.tcpIpInfo = tcpIpInfo;

            if (tcpIpInfo.IpAddress == null)
                return;

            //1.
            //IPHostEntry ipHost = Dns.GetHostEntry(tcpIpInfo.IpAddress);
            //IPAddress ipAddr = ipHost.AddressList[0];

            //2.
            //IPHostEntry ipHost = Dns.Resolve(tcpIpInfo.IpAddress);
            //IPAddress ipAddr = ipHost.AddressList[0];

            // 3.
            IPAddress ipAddr;
            bool ok = IPAddress.TryParse(tcpIpInfo.IpAddress, out ipAddr);
            if (ok == false)
            {
                DynMvp.UI.Touch.MessageForm.Show(null, "Host Ip is invalid");
                return;
            }

            ipEndPoint = new IPEndPoint(ipAddr, tcpIpInfo.PortNo);

            //clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void StartConnection()
        {
            LogHelper.Debug(LoggerType.Network, "SinglePortSocket::StartConnection");

            stopConnectionThreadFlag = false;

            connectionThread = new Thread(new ThreadStart(ConnectionProc));
            connectionThread.Start();
        }

        private void ConnectionProc()
        {
            LogHelper.Debug(LoggerType.Network, "SinglePortSocket::ConnectionProc - Start");
            while (!stopConnectionThreadFlag)
            {
                Thread.Sleep(3000);
                //Debug.WriteLine(string.Format("SinglePortSocket::ConnectionProc - Create Socket"));
                Socket connectionSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                e.RemoteEndPoint = ipEndPoint;
                e.Completed += ConnectAsync_Completed;
                e.AcceptSocket = connectionSocket;
                e.SocketError = SocketError.TimedOut;

                manualReset.Reset();
                connectionSocket.ConnectAsync(e);
                manualReset.WaitOne();

                if (e.SocketError != SocketError.Success)
                    connectionSocket.Dispose();

            }
            LogHelper.Debug(LoggerType.Network, "SinglePortSocket::ConnectionProc - End");
        }

        ManualResetEvent manualReset = new ManualResetEvent(false);
        private void ConnectAsync_Completed(object sender, SocketAsyncEventArgs e)
        {
            //Debug.WriteLine(string.Format("SinglePortSocket::ConnectAsync_Completed - SocketError: {0}", e.SocketError));
            LogHelper.Debug(LoggerType.Network, string.Format("SinglePortSocket::ConnectAsync_Completed - {0}", e.SocketError.ToString()));

            if (e.SocketError == SocketError.Success)
            {
                StopConnection(false);
                this.clientSocket = e.ConnectSocket;

                //List<byte> list = new List<byte>();
                //list.AddRange((BitConverter.GetBytes(1)));
                //list.AddRange(BitConverter.GetBytes(100)); //Send a packet once every 10 seconds.
                //list.AddRange(BitConverter.GetBytes(100));  //If no response, resend every second.
                //clientSocket.IOControl(IOControlCode.KeepAliveValues, list.ToArray(), null);

                OnConnectAsyncCompleted?.Invoke();

                StartListening();
            }

            manualReset.Set();
        }

        public void Connect()
        {
            if (tcpIpInfo.IpAddress == "" || tcpIpInfo.IpAddress == null)
                return;

            LogHelper.Debug(LoggerType.Network, "Connect Socket");

            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                clientSocket.Connect(ipEndPoint);
                StartListening();

                LogHelper.Debug(LoggerType.Network, "Socket Connected");
            }
            catch (SocketException ex)
            {
                LogHelper.Error(LoggerType.Error, "Socket Error : " + ex.Message);
                //throw ex;
            }
        }

        public void StopConnection(bool wait)
        {
            LogHelper.Debug(LoggerType.Network, "SinglePortSocket::StopConnection");

            stopConnectionThreadFlag = true;

            if (wait)
            {
                if (connectionThread != null && connectionThread.IsAlive)
                    Thread.Sleep(100);
            }
        }

        public void Close()
        {
            LogHelper.Debug(LoggerType.Network, "SinglePortSocket::Close");

            StopConnection(true);
            StopListening(true);

            CloseSocket();
        }

        private void CloseSocket()
        {
            if (clientSocket != null)
            {
                LogHelper.Debug(LoggerType.Network, "SinglePortSocket::CloseSocket");

                try
                {
                    clientSocket.Shutdown(SocketShutdown.Receive);
                    clientSocket.Shutdown(SocketShutdown.Send);
                    clientSocket.Disconnect(false);
                }
                catch (SocketException)
                {
                    LogHelper.Error(LoggerType.Error, "Client socket shutdown fail.");
                }
                clientSocket.Close();
                clientSocket.Dispose();
                clientSocket = null;
            }
        }

        public void ClearBuffer()
        {
            if (clientSocket != null)
            {
                int dataAvailable = clientSocket.Available;

                if (dataAvailable > 0)
                {
                    byte[] receiveBuf = new byte[dataAvailable];
                    clientSocket.Receive(receiveBuf);
                }
            }
        }

        public void StartListening()
        {
            LogHelper.Debug(LoggerType.Network, "SinglePortSocket::StartListening");

            stopListeningThreadFlag = false;

            listeningThread = new Thread(new ThreadStart(ListeningProc));
            listeningThread.Start();
        }

        public void StopListening(bool wait)
        {
            LogHelper.Debug(LoggerType.Network, "SinglePortSocket::StopListening");

            stopListeningThreadFlag = true;

            if (wait)
            {
                while (listeningThread != null && listeningThread.IsAlive)
                    Thread.Sleep(100);
            }
        }

        public void SendCommand(PacketParser command)
        {
            packetHandler.PacketParser = command;
            try
            {
                clientSocket.Send(command.EncodePacket("Request"));
            }
            catch (SocketException)
            {
                Close();
            }
        }

        public void SendCommand(string commandString)
        {
            SendCommand(Encoding.ASCII.GetBytes(commandString));
        }

        public bool SendCommand(byte[] commandPacket)
        {
            try
            {
                if (this.clientSocket == null)
                    return false;

                //LogHelper.Debug(LoggerType.Network, $"SinglePortSocket::SendCommand - Length: {commandPacket.Length}");

                lock (this.clientSocket)
                {
                    //clientSocket.Send(new byte[1], 0, 1, SocketFlags.None, out socketError); // Send중인 데이터와 섞이지 않도록 조심..
                    SocketError socketError;
                    bool ok = commandPacket.Length == this.clientSocket.Send(commandPacket, 0, commandPacket.Length, SocketFlags.None, out socketError);
                    if (socketError != SocketError.Success)
                    {
                        LogHelper.Error(LoggerType.Network, string.Format("SinglePortSocket::SendCommand - SocketError: {0}", socketError));
                        return false;
                    }
                    return ok;
                }
            }
            catch (SocketException ex)
            {
                LogHelper.Error(LoggerType.Network, string.Format("SinglePortSocket::SendCommand - SocketException: {0}", ex.Message));
                return false;
            }
        }

        public bool ProcessDataPacket(byte[] receiveBuf)
        {
            return packetHandler.ProcessPacket(receiveBuf, packetData);
        }

        private void ListeningProc()
        {
            const int maxBufferSize = 10240;
            bool packetCompleted = false;

            int loop = 0;
            try
            {
                while (stopListeningThreadFlag == false)
                {
                    if (this.iAmHereInterval > 0)
                    {
                        if ((loop = (loop + 1) % this.iAmHereInterval) == 0)
                        {
                            // 연결 확인 차원에서 보내봄.
                            SendCommand(this.iAmHereBytes);
                            //bool good = SendCommand(this.iAmHereBytes);
                            //if (!good || !clientSocket.Connected)
                            //    break;
                        }
                    }

                    if (!this.clientSocket.Connected)
                        throw new Exception("Socket Disconnected");

                    do
                    {
                        int dataAvailable = clientSocket.Available;
                        if (dataAvailable == 0)
                            break;

                        //LogHelper.Debug(LoggerType.Inspection, String.Format("Data Available : {0}", dataAvailable));

                        int dataToRead = Math.Min(dataAvailable, maxBufferSize);
                        byte[] receiveBuf = new byte[dataToRead];
                        clientSocket.Receive(receiveBuf);

                        if (this.iAmHereBytes.Length > 0)
                        {
                            List<byte> byteList = receiveBuf.ToList();
                            int src = -1;
                            int cnt = this.iAmHereBytes.Length;
                            do
                            {
                                src = byteList.IndexOf(this.iAmHereBytes[0], src + 1);
                                if (src < 0 || (src + cnt) >= byteList.Count)
                                    break;

                                List<byte> partialList = byteList.GetRange(src, cnt);
                                if (this.iAmHereBytes.SequenceEqual(partialList))
                                {
                                    byteList.RemoveRange(src, cnt);
                                    src--;
                                }
                            } while (true);
                            receiveBuf = byteList.ToArray();
                        }

                        if (receiveBuf.Length > 0)
                        {
                            //LogHelper.Debug(LoggerType.Network, string.Format("SinglePortSocket::ListeningProc - Received: {0}",
                            //string.Join(",", Array.ConvertAll(receiveBuf, f => f.ToString("X02")))));

                            packetCompleted = ProcessDataPacket(receiveBuf);
                            //LogHelper.Debug(LoggerType.Inspection, "Exit Process Data Packet");
                        }

                    } while (packetCompleted);

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Network, string.Format("SinglePortSocket::ListeningProc - {0} - Message: {1}", ex.GetType().Name, ex.Message));
            }
            finally
            {
                CloseSocket();
                if (!stopListeningThreadFlag)    // 비정상 종료
                    StartConnection();
            }
        }
    }
}
