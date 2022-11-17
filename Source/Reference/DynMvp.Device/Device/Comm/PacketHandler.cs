using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using DynMvp.Base;
using System.Diagnostics;
using DynMvp.Device.Device.Comm;

namespace DynMvp.Devices.Comm
{
    public class ReceivedPacket
    {
        // IP Address, Port Name....  Etc
        string senderInfo;
        public string SenderInfo
        {
            get => senderInfo;
            set => senderInfo = value;
        }

        bool valid = false;
        public bool Valid
        {
            get { return valid; }
            set { valid = value; }
        }

        protected byte[] receivedData;
        public byte[] ReceivedData
        {
            get { return receivedData; }
        }

        private string errCode;
        public string ErrCode
        {
            get { return errCode; }
        }

        public ReceivedPacket() { }
        public ReceivedPacket(byte[] receivedData, string errCode = null)
        {
            this.receivedData = receivedData;
            this.errCode = errCode;
        }
    }

    public class AsyncRecivedPacket : ReceivedPacket
    {
        SendPacket sendPacket;
        public SendPacket SendPacket
        {
            get { return sendPacket; }
            set { sendPacket = value; }
        }

        byte[] receivedDataByte;
        public byte[] ReceivedDataByte
        {
            get { return receivedDataByte; }
            set { receivedDataByte = value; }
        }

        string lastPacketName;
        public string LastPacketName
        {
            get { return lastPacketName; }
            set { lastPacketName = value; }
        }

        string receivedDataStr;
        public string ReceivedDataStr
        {
            get { return receivedDataStr; }
            set { receivedDataStr = value; }
        }

        string logString;
        public string LogString
        {
            get { return logString; }
            set { logString = value; }
        }
    }

    public delegate void DataReceivedDelegate(ReceivedPacket receivedPacket);

    public abstract class PacketParser
    {
        public event DataReceivedDelegate DataReceived;
        public event EventHandler<ReceivedPacket> DataRecived2;

        //public virtual byte[] GetInitPacket() { return null; }

        //public virtual byte[] GetRequestPacket() { return null; }

        /// <summary>
        /// listening Packet Parsing
        /// </summary>
        /// <param name="PacketContents"></param>
        /// <returns></returns>
        public abstract bool ParsePacket(byte[] PacketContents, out int processedPacketCnt);

        /// <summary>
        /// Protocol -> Packet
        /// </summary>
        /// <param name="protocol"></param>
        /// <returns></returns>
        public abstract byte[] EncodePacket(string protocol);

        public abstract string DecodePacket(byte[] packet);

        public abstract PacketParser Clone();

        public void OnDataReceived(ReceivedPacket receivedPacket)
        {
            DataReceived?.Invoke(receivedPacket);
        }

        protected int IndexOf(byte[] dataByte, byte[] searchByte, out float matchRate)
        {
            matchRate = 0;

            for (int dataIndex = 0; dataIndex < dataByte.Count() - searchByte.Count() + 1; dataIndex++)
            {
                for (int searchIndex = 0; searchIndex < searchByte.Count(); searchIndex++)
                {
                    if (dataByte[dataIndex + searchIndex] != searchByte[searchIndex])
                    {
                        matchRate = 0;
                        break;
                    }
                    else
                    {
                        matchRate++;
                    }
                }

                matchRate /= searchByte.Count();
                if (matchRate > 0)
                    return dataIndex;
            }

            return -1;
        }
    }

    public delegate byte[] EncodePacketDelegate(string protocol);
    public delegate string DecodePacketDelegate(byte[] packet);

    public class SimplePacketParser : PacketParser
    {
        public byte[] StartChar { get => this.startChar; set => this.startChar = value; }
        protected byte[] startChar;

        public byte[] EndChar { get => this.endChar; set => this.endChar = value; }
        protected byte[] endChar;

        public SimplePacketParser()
        {
            this.startChar = null;
            this.endChar = null;
        }

        public SimplePacketParser(byte[] startChar, byte[] endChar)
        {
            this.startChar = startChar;
            this.endChar = endChar;
        }

        public override bool ParsePacket(byte[] packetContents, out int processedPacketCnt)
        {
            int startPos, endPos;

            if (startChar == null && endChar == null)
            // 시작/종료 문자가 정의되지 않음 -> 받은 패킷 그대로 처리
            {
                startPos = 0;
                endPos = packetContents.Length;
                processedPacketCnt = endPos;
                OnDataReceived(new ReceivedPacket(packetContents) { Valid = true });
                return true;
            }
            else
            {
                startPos = -1;
                endPos = -1;

                float mathcRate = 0;

                // 시작문자가 정의됨 
                if (startChar != null)
                {
                    startPos = IndexOf(packetContents, startChar, out mathcRate);
                    if (startPos < 0)
                    {
                        // 시작문자 못찾음 => 버림.
                        processedPacketCnt = packetContents.Length;
                        return false;
                    }
                    else if (mathcRate < 1)
                    {
                        // case 3. 시작문자 일부만 찾음 => 시작문자 이전까지 버림.
                        processedPacketCnt = startPos;
                        return false;
                    }
                    else
                    {
                        // case 1. 시작문자 찾음. 계속 진행   
                    }
                }
                else
                {
                    //startPos = 0;
                }

                // 종료문자가 정의됨
                if (endChar != null)
                {
                    endPos = IndexOf(packetContents, endChar, out mathcRate);
                    if (startPos < 0)
                    {
                        if (endPos < 0)
                        {
                            // case 0. 시작 문자 정의 안 됨, 종료문자 못찾음 => 살림
                            processedPacketCnt = 0;
                            return false;
                        }
                        else
                        {
                            // case 1. 시작 문자 정의 안 됨, 종료문자 찾음 => 처리
                            // endPos 는 전체 패킷 기준 종료문자의 Index
                            endPos += endChar.Length;
                        }
                    }
                    else
                    {
                        if (endPos < 0 || mathcRate < 1)
                        {
                            // case 2. 시작 문자 찾음, 종료문자 몾찾음 => 시작 문자까지만 살림.
                            processedPacketCnt = startPos;
                            return false;
                        }
                        else
                        {
                            // case 3. 시작 문자 찾음, 종료문자 찾음 => 처리.
                            endPos += endChar.Length;
                        }
                    }
                }
                else
                {
                    endPos = packetContents.Length;
                }

                if (endPos < startPos)
                {
                    // 종료문자가 시작문자보다 먼저 나온 경우.
                    // 시작문자 이전까지의 패킷 버림
                    processedPacketCnt = startPos;
                    return false;
                }

                startPos = Math.Max(0, startPos);
                endPos = Math.Max(0, endPos);

                int length = endPos - startPos;
                byte[] parsedPacket = packetContents.Skip(startPos).Take(length).ToArray();
                this.OnDataReceived(new ReceivedPacket(parsedPacket));
                processedPacketCnt = endPos;
                return true;
            }
        }

        public override PacketParser Clone()
        {
            SimplePacketParser simplePacketParser = new SimplePacketParser();
            simplePacketParser.startChar = this.startChar;
            simplePacketParser.endChar = this.endChar;
            simplePacketParser.DataReceived += this.OnDataReceived;
            return simplePacketParser;
        }

        public override byte[] EncodePacket(string protocol)
        {
            List<Byte> byteList = new List<byte>();
            if (startChar != null)
                byteList.AddRange(startChar);

            byte[] packet = Encoding.Default.GetBytes(protocol);
            byteList.AddRange(packet);

            if (endChar != null)
                byteList.AddRange(endChar);

            byte[] fullPacket = byteList.ToArray();
            return fullPacket;
        }

        public override string DecodePacket(byte[] packet)
        {
            if (endChar != null)
                packet = packet.Take(packet.Length - endChar.Length).ToArray();
            if (startChar != null)
                packet = packet.Skip(startChar.Length).ToArray();

            string command = Encoding.Default.GetString(packet);
            return command;
        }
    }

    public class PacketData
    {
        private byte[] dataByteFull = null;
        public byte[] DataByteFull
        {
            get { return dataByteFull; }
            set { dataByteFull = value; }
        }
    }

    public class PacketHandler
    {
        public PacketParser PacketParser { get => this.packetParser; set => this.packetParser = value; }
        protected PacketParser packetParser = null;

        bool useLogReceivedPacket = false;
        public bool UseLogReceivedPacket
        {
            get { return useLogReceivedPacket; }
            set { useLogReceivedPacket = value; }
        }

        public PacketHandler(PacketParser packetParser = null)
        {
            if (packetParser == null)
                packetParser = new SimplePacketParser();

            this.packetParser = packetParser;
        }

        // The Mutex object that will protect image objects during processing
        private Mutex imageMutex = new Mutex();

        public bool ProcessPacket(byte[] newDataByte, PacketData packetData)
        {
            bool pakcetCompleted = false;

            Debug.Assert(packetParser != null);


            imageMutex.WaitOne();

            try
            {
                byte[] dataByteFull = MergeBuffer(packetData, newDataByte);

                do
                {
                    if (useLogReceivedPacket)
                    {
                        string fullString = System.Text.Encoding.Default.GetString(dataByteFull.ToArray());
                        int strLen = Math.Min(fullString.Length, 100);

                        LogHelper.Debug(LoggerType.Serial, String.Format("PacketHandler::ProcessPacket- {0}", fullString.Substring(0, strLen)));
                    }

                    int processedPacketCnt;
                    bool done = packetParser.ParsePacket(dataByteFull, out processedPacketCnt);
                    dataByteFull = dataByteFull.Skip(processedPacketCnt).ToArray();

                    if (dataByteFull.Length == 0)
                        dataByteFull = null;

                    if (dataByteFull == null || done == false)
                        break;

                } while (true);

                packetData.DataByteFull = dataByteFull;
            }
            finally
            {
                imageMutex.ReleaseMutex();
            }
            return pakcetCompleted;
        }

        private byte[] MergeBuffer(PacketData packetData, byte[] newDataByte)
        {
            byte[] dataByteFull = null;

            if (packetData.DataByteFull == null)
            // Old Data 존재 안 함
            {
                dataByteFull = new byte[newDataByte.Length];
                System.Buffer.BlockCopy(newDataByte, 0, dataByteFull, 0, newDataByte.Length);
            }
            else
            {
                dataByteFull = new byte[packetData.DataByteFull.Length + newDataByte.Length];
                System.Buffer.BlockCopy(packetData.DataByteFull, 0, dataByteFull, 0, packetData.DataByteFull.Length);
                System.Buffer.BlockCopy(newDataByte, 0, dataByteFull, packetData.DataByteFull.Length, newDataByte.Length);
            }
            return dataByteFull;
        }

        public void ClearParser()
        {
            packetParser = null;
        }
    }
}
