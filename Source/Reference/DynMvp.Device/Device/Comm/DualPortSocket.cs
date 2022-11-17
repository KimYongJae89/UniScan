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
    public class DualPortSocket
    {
        private string host;
        private int dataPort = 0;
        private int commandPort = 0;

        private bool stopThreadFlag = false;
        
        private Thread dataListeningThread;
        private Thread commandProcessingThread;

        PacketData dataPacketData = new PacketData();
        public PacketData DataPacketData
        {
            get { return dataPacketData; }
            set { dataPacketData = value; }
        }

        private PacketHandler dataListeningPacketHandler = new PacketHandler();
        public PacketHandler DataListeningPacketHandler
        {
            get { return dataListeningPacketHandler; }
            set { dataListeningPacketHandler = value; }
        }

        PacketData commandPacketData = new PacketData();
        public PacketData CommandPacketData
        {
            get { return commandPacketData; }
            set { commandPacketData = value; }
        }

        private PacketHandler commandProcessingPacketHandler = new PacketHandler();
        public PacketHandler CommandProcessingPacketHandler
        {
            get { return commandProcessingPacketHandler; }
            set { commandProcessingPacketHandler = value; }
        }

        Mutex commandMutex = new Mutex();

        Socket dataListeningSocket = null;
        Socket commandSocket = null;
        bool listenConnected = false;
        public bool ListenConnected
        {
            get { return listenConnected; }
        }

        bool commandConnected = false;
        public bool CommandConnected
        {
            get { return commandConnected; }
        }

        public DualPortSocket()
        {
            
        }

        public void Connect(TcpIpInfo tcpIpInfo)
        {
            Connect(tcpIpInfo);
        }

        public void Listen(TcpIpInfo tcpIpInfo)
        {
            Listen(tcpIpInfo);
        }

        public void Connect(string host, int dataPort, int commandPort)
        {
            if (host == "")
                return;

            this.host = host;
            this.dataPort = dataPort;
            this.commandPort = commandPort;

            try
            {
                dataListeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                dataListeningSocket.Connect(host, dataPort);
                listenConnected = true;
            }
            catch (SocketException ex)
            {
                LogHelper.Error(LoggerType.Error, "Socket Error : " + ex.Message);
                listenConnected = false;
            }
        }

        public void Listen(string host, int dataPort, int commandPort)
        {
            if (host == "")
                return;


            this.host = host;
            this.dataPort = dataPort;
            this.commandPort = commandPort;

            try
            {
                dataListeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                dataListeningSocket.Connect(host, dataPort);
                listenConnected = true;

            }
            catch (SocketException ex)
            {
                LogHelper.Error(LoggerType.Error, "Socket Error : " + ex.Message);
                listenConnected = false;
            }
        }

        public void Close()
        {
            if (dataListeningSocket != null && listenConnected == true)
            {
                dataListeningSocket.Shutdown(SocketShutdown.Both);
                dataListeningSocket.Close();
                dataListeningSocket = null;
                listenConnected = false;
            }
        }

        public void StartDataListening()
        {
            stopThreadFlag = false;

            if (dataListeningSocket != null && listenConnected == true)
            {
                dataListeningThread = new Thread(new ThreadStart(DataListeningProc));
                dataListeningThread.IsBackground = true;
                dataListeningThread.Start();
            }
        }

        public void StartCommandListening()
        {
            if (commandPort == 0)
                return;

            stopThreadFlag = false;

            if (dataListeningSocket != null)
            {
                commandProcessingThread = new Thread(new ThreadStart(CommandProcessingProc));
                commandProcessingThread.IsBackground = true;
                commandProcessingThread.Start();
            }
        }

        public void StopListening()
        {
            stopThreadFlag = true;

            if (dataListeningSocket != null)
            {
                if (dataListeningThread != null)
                {
                    while (dataListeningThread.IsAlive)
                        Thread.Sleep(100);
                }

                if (commandProcessingThread != null)
                {
                    while (commandProcessingThread.IsAlive)
                        Thread.Sleep(100);
                }
            }
        }

        public void SendCommand(PacketParser command)
        {
            if (commandMutex.WaitOne(2000) == false)
            {
                LogHelper.Error(LoggerType.Error, "Wait failed. Command is not completed.");
                return;
            }

            commandProcessingPacketHandler.PacketParser = command;
        }

        public void SendResponse(PacketParser command)
        {
            commandProcessingPacketHandler.PacketParser = command;
        }

        public void Stop()
        {
            stopThreadFlag = true;
        }

        // 가상 모드의 동작을 위해 별도 함수로 분리
        public bool ProcessDataPacket(byte[] receiveBuf)
        {
            return dataListeningPacketHandler.ProcessPacket(receiveBuf, dataPacketData);
        }

        private void DataListeningProc()
        {
            const int maxBufferSize = 10240;

            while (stopThreadFlag == false)
            {
                try
                {
                    bool packetCompleted = false;
                    while (packetCompleted == false)
                    {
                        if (stopThreadFlag == true)
                            break;

                        int dataAvailable = dataListeningSocket.Available;

                        if (dataAvailable > 0)
                        {
//                            Trace.WriteLine(String.Format("Data available : Size {0}", dataAvailable));

                            int dataToRead = Math.Min(dataAvailable, maxBufferSize);
                            byte[] receiveBuf = new byte[dataToRead];

                            dataListeningSocket.Receive(receiveBuf);
//                            Trace.WriteLine("Data Received");

                            packetCompleted = ProcessDataPacket(receiveBuf);
                        }
                        Thread.Sleep(1);
                    }
                }
                catch (SocketException ex)
                {
                    LogHelper.Error(LoggerType.Error, "Socket Error : " + ex.Message);
                }

                Thread.Sleep(10);
            }
        }

        private void CommandProcessingProc()
        {
            while (stopThreadFlag == false)
            {
                PacketParser packetParser = commandProcessingPacketHandler.PacketParser;

                if (packetParser != null)
                {
                    LogHelper.Debug(LoggerType.Network, String.Format("[{0}] Detect Packet Parser", host));

                    LogHelper.Debug(LoggerType.Network, String.Format("[{0}] Start Packet Parser", host));

                    byte[] packet = packetParser.EncodePacket("Request");

                    commandSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    try
                    {
                        commandSocket.Connect(host, commandPort);
                        commandSocket.Send(packet);

                        LogHelper.Debug(LoggerType.Network, String.Format("[{0}] Send Request Packet", host));

                        Thread.Sleep(10);

                        bool packetCompleted = false;
                        while (packetCompleted == false)
                        {
                            if (stopThreadFlag == true)
                                break;

                            int dataAvailable = commandSocket.Available;

                            if (dataAvailable > 0)
                            {
                                byte[] receiveBuf = new byte[dataAvailable];

                                commandSocket.Receive(receiveBuf);

                                packetCompleted = commandProcessingPacketHandler.ProcessPacket(receiveBuf, commandPacketData);
                            }
                        }

                        commandSocket.Shutdown(SocketShutdown.Both);
                        commandSocket.Close();
                        commandSocket = null;

                        LogHelper.Debug(LoggerType.Network, String.Format("[{0}] Finish Packet Process", host));
                    }
                    catch ( SocketException ex)
                    {
                        Trace.WriteLine("Socket Exception : " + ex.Message);
                    }

                    LogHelper.Debug(LoggerType.Network, String.Format("[{0}] Release Mutex", host));
                    commandMutex.ReleaseMutex();

                    LogHelper.Debug(LoggerType.Network, String.Format("[{0}] Remove Head", host));
                    commandProcessingPacketHandler.PacketParser = null;
                }

                Thread.Sleep(1);
            }
        }
    }
}
