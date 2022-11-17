using DynMvp.Base;
using DynMvp.Device.Device.Light;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DynMvp.Devices.Light
{
    public class SerialLightUniSensor :SerialLightCtrl
    {
        //int[] m_SWOnOff = null;

        public SerialLightUniSensor(SerialLightCtrlInfo serialLightCtrlInfo)
            : base(serialLightCtrlInfo)
        {
            responseReceived = new ManualResetEvent(false);
        }

        public override int GetMaxLightLevel()
        {
            return 1000;
        }

        public override bool Initialize()
        {
            try
            {
                SerialLightCtrlInfo serialLightCtrlInfo = (SerialLightCtrlInfo)this.lightCtrlInfo;

                lightSerialPort = new SerialPortEx(new SimplePacketParser(null, new byte[] { 0x0A }));
                lightSerialPort.PacketHandler.PacketParser.DataReceived += lightSerialPort_PacketReceived;
                lightSerialPort.Open(serialLightCtrlInfo.Name, serialLightCtrlInfo.SerialPortInfo);
                lightSerialPort.StartListening();
                //lightSerialPort.OnPacketReceived += lightSerialPort_PacketReceived;

                //m_SWOnOff = Enumerable.Repeat<int>(-1, serialLightCtrlInfo.NumChannel).ToArray<int>();

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Can't open serial port. {0}", ex.Message));

                lightSerialPort = null;
            }

            return false;
        }

        private void lightSerialPort_PacketReceived(ReceivedPacket receivedPacket)
        {
            //일단 라인피트만 검색
            //if (Array.IndexOf<byte>(dataByte, 0x0A) >= 0)
                responseReceived.Set();
        }

        public override void Release()
        {
            base.Release();

            lightSerialPort.StopListening();
            lightSerialPort.Close();
        }
        private int convert_255toMax(int value)
        {
            return value * GetMaxLightLevel() / 255;
        }
        protected override bool SetLightValue(LightValue lightValue) //0~255 입력
        {
            if (lightSerialPort != null)
            {
                //base.TurnOn();
                if (lastLightValue == null)
                {
                    lastLightValue = new LightValue(NumChannel);
                    for (int i = 0; i < NumChannel; i++)
                        lastLightValue.Value[i] = GetMaxLightLevel();
                }


                try
                {
                    SerialLightCtrlInfo serialLightCtrlInfo = (SerialLightCtrlInfo)this.lightCtrlInfo;
                    for (int i = 0; i < serialLightCtrlInfo.NumChannel; i++)
                    {
                        // memory last value only over zero
                        this.LastLightValue.Value[i] = lightValue.Value[i];

                        string packet = String.Format("I{0},{1}\r\n", i, convert_255toMax(lightValue.Value[i]));
                        byte[] command = Encoding.ASCII.GetBytes(packet);
                        if (sendCommandSync(command) == false)
                            throw new Exception("Light Contoller has no responce 000");

                        int onoff = lightValue.Value[i] > 0 ? 1 : 0;
                        packet = String.Format("P{0},{1}\r\n", i, onoff);
                        command = Encoding.ASCII.GetBytes(packet);
                        if (sendCommandSync(command) == false)
                            throw new Exception("Light Contoller has no responce 001");
                    }
                }
                catch (Exception ex)
                {
                    ErrorManager.Instance().Report(ErrorCodeLight.Instance.FailToWriteValue, ErrorLevel.Error,
                        this.name, ex.Message, null, "Please, Check the Light Controller");
                }
            }
            Thread.Sleep(lightStableTimeMs);
            return true;
        }
        private bool sendCommandSync(byte[] command)
        {
            responseReceived.Reset();
            lightSerialPort.WritePacket(command, 0, command.Length);
            //if (serialLightCtrlInfo.ResponceTimeoutMs != 0 && this.responseReceived.WaitOne(serialLightCtrlInfo.ResponceTimeoutMs) == false)
            return this.responseReceived.WaitOne(500);
        }

        public override LightValue GetLightValue()
        {
            return curLightValue.Clone();
        }
    }
}
