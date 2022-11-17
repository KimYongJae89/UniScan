using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Devices.Comm
{
    public delegate void OnSocketClosedDelegate(ClientHandlerSocket clientHandlerSocket);

    public class ClientHandlerSocket
    {
        public SimpleServerSocket ServerSocket => this.serverSocket;
        SimpleServerSocket serverSocket;

        Socket handlerSocket;
        private Task listeningTask;
        bool stopThreadFlag = false;

        public int IAmHereInterval { get => this.iAmHereInterval;}
        int iAmHereInterval = 500;

        public byte[] IAmHereBytes { get => this.iAmHereBytes; }
        byte[] iAmHereBytes = new byte[0];

        private PacketData packetData = new PacketData();
        public PacketData PacketData
        {
            get { return packetData; }
        }

        private PacketHandler packetHandler = new PacketHandler();
        public PacketHandler PacketHandler
        {
            get { return packetHandler; }
        }

        public event OnSocketClosedDelegate OnSocketClosing;

        public ClientHandlerSocket(SimpleServerSocket serverSocket)
        {
            this.serverSocket = serverSocket;
        }

        public void Start(Socket handlerSocket)
        {
            this.handlerSocket = handlerSocket;

            stopThreadFlag = false;
            listeningTask = new Task(new Action(ListeningProc));
            listeningTask.Start();
        }

        public void BeginStop()
        {
            stopThreadFlag = true;
        }

        public string GetRemoteIpAddress()
        {
            return handlerSocket.RemoteEndPoint.ToString().Split(':')[0];
        }

        public void SendCommand(PacketParser command)
        {
            try
            {
                if (handlerSocket != null)
                {
                    PacketHandler.PacketParser = command;
                    //LogHelper.Debug(LoggerType.Network, "Send Command : " + System.Text.Encoding.Default.GetString(command.GetRequestPacket()));

                    if (handlerSocket.Connected == true)
                    {
                        handlerSocket.Send(command.EncodePacket("Request"));
                    }
                }
            }
            catch (SocketException e)
            {
                LogHelper.Error(LoggerType.Error, "Socket Exception : " + e.Message);
            }
        }

        public bool SendCommand(byte[] commandPacket)
        {
            try
            {
                if (handlerSocket != null)
                {
                    //LogHelper.Debug(LoggerType.Network, "Send Command : " + System.Text.Encoding.Default.GetString(commandPacket));
                    lock (handlerSocket)
                    {
                        SocketError socketError;
                        int sendByte = handlerSocket.Send(commandPacket, 0, commandPacket.Length, SocketFlags.None, out socketError);
                        return (socketError != SocketError.Success) || (sendByte == commandPacket.Length);
                    }
                }
                return false;
            }
            catch (SocketException e)
            {
                LogHelper.Error(LoggerType.Error, "Socket Exception : " + e.Message);
                return false;
            }
        }

        private void ListeningProc()
        {
            const int maxBufferSize = 10240;

            try
            {
                int loop = 0;
                while (stopThreadFlag == false)
                {
                    if (iAmHereInterval > 0)
                    {
                        if ((loop = (loop + 1) % this.iAmHereInterval) == 0)
                        {
                            bool good = SendCommand(this.iAmHereBytes); // 통신 확인차 하나 보내봄.. 
                                                                  //handlerSocket.Send(new byte[1], 0, 1, SocketFlags.None, out socketError); // Send중인 데이터와 섞이지 않도록 조심..
                            if (!good || !handlerSocket.Connected)
                                break;
                        }
                    }

                    bool packetCompleted = false;
                    do
                    {
                        int dataAvailable = handlerSocket.Available;
                        if (dataAvailable > 0)
                        {
                            int dataToRead = Math.Min(dataAvailable, maxBufferSize);
                            byte[] receiveBuf = new byte[dataToRead];

                            handlerSocket.Receive(receiveBuf);
                            if (this.iAmHereBytes.Length > 0)
                            {
                                List<byte> byteList = receiveBuf.ToList();
                                int src = -1;
                                int cnt = this.iAmHereBytes.Length;
                                do
                                {
                                    src = byteList.IndexOf(this.iAmHereBytes[0], src + 1);
                                    if (src < 0 || (src + cnt) > byteList.Count)
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
                                //LogHelper.Debug(LoggerType.Network, string.Format("SinglePortSocket::ListeningProc - Received: {0}, 0x {1}",
                                //    handlerSocket.RemoteEndPoint,
                                //    string.Join(",", Array.ConvertAll(receiveBuf, f => f.ToString("X02")))));

                                packetCompleted = packetHandler.ProcessPacket(receiveBuf, packetData);
                            }
                        }
                    } while (packetCompleted);

                    Thread.Sleep(100);
                }
            }
            catch (SocketException e)
            {
                LogHelper.Error(LoggerType.Error, string.Format("Socket Exception : {0}", e.Message));
            }

            CloseSocket();
        }

        internal void SetIAmHere(int interval, byte[] bytes)
        {
            this.iAmHereInterval = interval;
            this.iAmHereBytes = bytes;
            if (this.iAmHereBytes == null)
                this.iAmHereBytes = new byte[0];
        }

        private void CloseSocket()
        {
            OnSocketClosing?.Invoke(this);
            handlerSocket.Shutdown(SocketShutdown.Receive);
            handlerSocket.Shutdown(SocketShutdown.Send);
            handlerSocket.Close();
            handlerSocket = null;
        }
    }
}
