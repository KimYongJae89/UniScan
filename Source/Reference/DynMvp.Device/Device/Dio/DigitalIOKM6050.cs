using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using DynMvp.Base;
using DynMvp.Devices.Comm;

using ModbusRTU;

namespace DynMvp.Devices.Dio
{
    public class DigitalIoKM6050 : DigitalIo
    {
        object lockObject = new object();
        SerialPortEx serialDIO;
        PacketParser packetParser;

        string readInputData = "";

        uint readOutput = 0;
        uint readInput = 0;

        public DigitalIoKM6050(string name)
            : base(DigitalIoType.KM6050, name)
        {
            NumInPort = 7;
            NumOutPort = 8;
            //this.serialPortInfo = serialPortInfo;
        }

        public override bool Initialize(DigitalIoInfo digitalIoInfo)
        {
            bool ok = base.Initialize(digitalIoInfo);
            if (!ok)
                return false;

            SerialPortInfo serialPortInfo = new SerialPortInfo("COM1", 115200);
            SimplePacketParser packetParser = new SimplePacketParser();
            packetParser.DataReceived += DataRecived;
            packetParser.EndChar = new byte[1] { (byte)'\r' };
            this.packetParser = packetParser;

            serialDIO = new SerialPortEx(packetParser);
            //serialDIO.PacketHandler.PacketParser = packetParser;
            serialDIO.Open("KM6050", serialPortInfo);
            serialDIO.StartListening();

            UpdateState(DeviceState.Ready);
            return true;
        }

        private void DataRecived(ReceivedPacket receivedPacket)
        {
            //lock(readInputData)
            //{
            readInputData = Encoding.Default.GetString(receivedPacket.ReceivedData);
            //}
        }

        public override void Release()
        {
            base.Release();
            serialDIO.Close();
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            LogHelper.Debug(LoggerType.IO, "WriteOutputGroup");
            try
            {
                lock (lockObject)
                {
                    LogHelper.Debug(LoggerType.IO, "Start WriteOutputGroup");

                    bool[] value = new bool[NumOutPort];
                    //Write
                    string packet = string.Format("#01000{0}", outputPortStatus);
                    serialDIO.WritePacket(packet);

                    serialDIO.WritePacket(new byte[] { 13 }, 0, 1);

                    LogHelper.Debug(LoggerType.IO, "End WriteOutputGroup");
                    readOutput = outputPortStatus;
                }
            }
            catch (TimeoutException ex)
            {
                LogHelper.Error(LoggerType.Error, String.Format("KM6050 Timeout Error : WriteOutputGroup {0}", ex.Message));
            }
            catch (ModbusLib.CRCErrorException ex)
            {
                LogHelper.Error(LoggerType.Error, String.Format("KM6050 CRC Error : WriteOutputGroup {0}", ex.Message));
            }
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            LogHelper.Debug(LoggerType.IO, "ReadOutputGroup");
            return readOutput;
        }

        public override uint ReadInputGroup(int groupNo)
        {
            LogHelper.Debug(LoggerType.IO, "ReadInputGroup");

            try
            {
                lock (lockObject)
                {
                    LogHelper.Debug(LoggerType.IO, "Start ReadInputGroup");

                    string packet = string.Format("$016");
                    serialDIO.WritePacket(packet);
                    serialDIO.WritePacket(new byte[] { 13 }, 0, 1);
                    Thread.Sleep(50);

                    if (string.IsNullOrEmpty(readInputData))
                        return 0;

                    GetData();

                    //uint data = Convert.ToUInt32(readResult);

                    return readInput;
                }
            }
            catch (TimeoutException ex)
            {
                LogHelper.Error(LoggerType.Error, String.Format("KM6050 Timeout Error : ReadInputGroup {0}", ex.Message));
            }
            catch (ModbusLib.CRCErrorException ex)
            {
                LogHelper.Error(LoggerType.Error, String.Format("KM6050 CRC Error : ReadInputGroup {0}", ex.Message));
            }

            return 0;
        }

        private void GetData()
        {
            string tempReadInputData;
            //lock (readInputData)
            // {
            tempReadInputData = readInputData;
            //}
            if (tempReadInputData.Length != 7)
                return;
            if (tempReadInputData[0] != '!')
                return;

            string result = tempReadInputData;
            result = result.Substring(3, 2);

            uint result2 = Convert.ToUInt32(result.ToString(), 16);

            //readInput = 15 - result2;
            readInput = result2;
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            // Do nothing
        }

    }
}
