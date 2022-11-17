using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using DynMvp.Base;
using DynMvp.Devices.Comm;
using System.Xml;
using DynMvp.Device.Device.Light;
using System.Windows.Forms;
using DynMvp.Device.Serial;

namespace DynMvp.Devices.Light
{
    public class SerialLightCtrlInfo : LightCtrlInfo, ISerialDeviceInfo
    {
        public string DeviceName { get => this.Name; set => this.Name = value; }

        public ESerialDeviceType DeviceType => ESerialDeviceType.Light;

        public SerialPortInfo SerialPortInfo { get; private set; } = new SerialPortInfo();

        public int ResponceTimeoutMs { get => this.responceTimeoutMs; set => this.responceTimeoutMs = value; }
        int responceTimeoutMs = 0;

        public static LightCtrlInfo Create(LightControllerVender controllerVender)
        {
            switch (controllerVender)
            {
                case LightControllerVender.Unknown:
                    break;

                case LightControllerVender.Iovis:
                case LightControllerVender.Movis:
                case LightControllerVender.AltSystem:
                case LightControllerVender.LFine:
                case LightControllerVender.Lvs:
                case LightControllerVender.PSCC:
                case LightControllerVender.VIT:
                case LightControllerVender.UniSensor:
                    return new SerialLightCtrlInfo(controllerVender);

                case LightControllerVender.iCore:
                    return new SerialLightCtrlInfoIPulse();
            }

            throw new NotImplementedException();
        }

        protected SerialLightCtrlInfo(LightControllerVender controllerVender) : base(controllerVender)
        {
            this.controllerType = LightCtrlType.Serial;
        }

        public override Form GetAdvancedConfigForm()
        {
            return null;
        }

        public override void SaveXml(XmlElement lightInfoElement)
        {
            base.SaveXml(lightInfoElement);

            //XmlHelper.SetValue(lightInfoElement, "ResponceTimeoutMs", this.responceTimeoutMs);
            this.SerialPortInfo.Save(lightInfoElement, "SerialLightController");
        }

        public override void LoadXml(XmlElement lightInfoElement)
        {
            base.LoadXml(lightInfoElement);

            //this.responceTimeoutMs = XmlHelper.GetValue(lightInfoElement, "ResponceTimeoutMs", this.responceTimeoutMs);
            this.SerialPortInfo.Load(lightInfoElement, "SerialLightController");
        }

        public override LightCtrlInfo Clone()
        {
            SerialLightCtrlInfo serialLightCtrlInfo = new SerialLightCtrlInfo(this.controllerVender);
            serialLightCtrlInfo.CopyFrom(this);

            return serialLightCtrlInfo;
        }

        public override void CopyFrom(LightCtrlInfo srcInfo)
        {
            base.CopyFrom(srcInfo);

            SerialLightCtrlInfo serialLightCtrlInfo = (SerialLightCtrlInfo)srcInfo;

            this.responceTimeoutMs = serialLightCtrlInfo.responceTimeoutMs;
            this.SerialPortInfo.CopyFrom(serialLightCtrlInfo.SerialPortInfo);
        }

        public virtual PacketParser CreatePacketParser()
        {
            PacketParser packetParser = new SimplePacketParser();
            return packetParser;
        }

        public virtual string GetPortFindString()
        {
            switch (this.ControllerVender)
            {
                case LightControllerVender.Unknown:
                    break;
                case LightControllerVender.Iovis:
                    break;
                case LightControllerVender.Movis:
                    break;
                case LightControllerVender.AltSystem:
                    break;
                case LightControllerVender.LFine:
                    break;
                case LightControllerVender.Lvs:
                    break;
                case LightControllerVender.PSCC:
                    break;
                case LightControllerVender.VIT:
                    return "ROONF\r\n";
                case LightControllerVender.UniSensor:
                    break;
                case LightControllerVender.iCore:
                    break;
            }
            return "";
        }

        public virtual bool IsPortFound(byte[] responce)
        {
            switch (this.ControllerVender)
            {
                case LightControllerVender.Unknown:
                    break;
                case LightControllerVender.Iovis:
                    break;
                case LightControllerVender.Movis:
                    break;
                case LightControllerVender.AltSystem:
                    break;
                case LightControllerVender.LFine:
                    break;
                case LightControllerVender.Lvs:
                    break;
                case LightControllerVender.PSCC:
                    break;
                case LightControllerVender.VIT:
                    {
                        string str = Encoding.Default.GetString(responce);
                        return str.StartsWith("ONF");
                    }
                    break;

                case LightControllerVender.UniSensor:
                    break;
                case LightControllerVender.iCore:
                    break;
            }
            throw new NotImplementedException();
        }

        public string GetDeviceString()
        {
            return $"{this.DeviceType};{this.controllerVender}";
        }
    }

    public class SerialLightCtrl : LightCtrl
    {
        protected SerialLightCtrlInfo SerialLightCtrlInfo => (SerialLightCtrlInfo)this.lightCtrlInfo;

        public SerialPortEx LightSerialPort => lightSerialPort;
        protected SerialPortEx lightSerialPort = null;

        protected ManualResetEvent responseReceived = null;
        protected object responce = null;
        protected object sent = null;

        public SerialLightCtrl(SerialLightCtrlInfo serialLightCtrlInfo)
            : base(serialLightCtrlInfo)
        {
            this.responseReceived = new ManualResetEvent(false);
        }

        public override int GetMaxLightLevel()
        {
            switch (this.lightCtrlInfo.ControllerVender)
            {
                case LightControllerVender.LFine:
                    return 1023;
                default:
                    return 255;
            }
        }

        public override bool Initialize()
        {
            SerialLightCtrlInfo serialLightCtrlInfo = (SerialLightCtrlInfo)lightCtrlInfo;
            try
            {
                PacketParser packetParser = serialLightCtrlInfo.CreatePacketParser();
                packetParser.DataReceived += lightSerialPort_PacketReceived;

                this.lightSerialPort = new SerialPortEx(packetParser);
                this.lightSerialPort.Open(serialLightCtrlInfo.Name, serialLightCtrlInfo.SerialPortInfo);
                this.lightSerialPort.StartListening();

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, String.Format("Can't open serial port. {0}", ex.Message));
                throw new AlarmException(ErrorCodeLight.Instance.FailToInitialize, ErrorLevel.Fatal,
                    lightCtrlInfo.Name, "Fail To Open serial port {0}", new object[] { this.SerialLightCtrlInfo.SerialPortInfo.PortName }, "");
                lightSerialPort = null;
            }

            return false;
        }

        protected virtual void lightSerialPort_PacketReceived(ReceivedPacket receivedPacket)
        {            
            string[] ss = Array.ConvertAll(receivedPacket.ReceivedData, f => f.ToString("X02"));
            System.Diagnostics.Debug.WriteLine(string.Format("lightSerialPort_PacketReceived - {0}", string.Join(", ", ss)));

            // 아무거나 응답이 오면 수신 확인됨 체크.
            this.responce = receivedPacket;
            this.responseReceived.Set();
        }

        public override void Release()
        {
            base.Release();

            lightSerialPort.StopListening();
            lightSerialPort.Close();
        }

        protected override bool SetLightValue(LightValue lightValue)
        {
            if (lightSerialPort != null)
            {
                this.responseReceived.Reset();
                this.responce = null;
                this.sent = null;
                try
                {
                    SerialLightCtrlInfo serialLightCtrlInfo = (SerialLightCtrlInfo)this.lightCtrlInfo;
                    switch (serialLightCtrlInfo.ControllerVender)
                    {
                        case LightControllerVender.Iovis:

                            for (int i = 0; i < this.NumChannel; i++)
                            {
                                string packet = String.Format("#CH{0:00}BW{1:0000}E", i + 1, lightValue.Value[StartChannelIndex + i]);
                                lightSerialPort.WritePacket(packet);
                                //Thread.Sleep(10);
                            }
                            break;

                        case LightControllerVender.Movis:
                            for (int i = 0; i < this.NumChannel; i++)
                            {
                                byte[] bytePacket = new byte[6];
                                bytePacket[0] = 0x95;
                                bytePacket[1] = 0x2;
                                bytePacket[2] = (byte)(i + 1);
                                bytePacket[3] = (byte)(lightValue.Value[StartChannelIndex + i] > 0 ? 1 : 0);
                                bytePacket[4] = (byte)(lightValue.Value[StartChannelIndex + i]);
                                bytePacket[5] = (byte)(bytePacket[0] + bytePacket[1] + bytePacket[2] + bytePacket[3] + bytePacket[4]);

                                lightSerialPort.WritePacket(bytePacket, 0, 6);
                                //Thread.Sleep(25);
                            }
                            break;

                        case LightControllerVender.AltSystem:

                            for (int i = 1; i < this.NumChannel; i++)
                            {
                                string packet = String.Format("L{0}{1:000}\r\n", i, lightValue.Value[StartChannelIndex + i]);
                                byte[] StrByte = Encoding.UTF8.GetBytes(packet);
                                lightSerialPort.WritePacket(StrByte, 0, 7);
                                //Thread.Sleep(20);
                            }
                            break;

                        case LightControllerVender.Lvs:
                            for (int i = 0; i < this.NumChannel; i++)
                            {
                                string packet = String.Format("L{0}{1:000}\r\n", i + 1, lightValue.Value[StartChannelIndex + i]);
                                byte[] StrByte = Encoding.UTF8.GetBytes(packet);
                                lightSerialPort.WritePacket(StrByte, 0, 7);
                                //Thread.Sleep(20);
                            }
                            break;

                        case LightControllerVender.LFine:
                            for (int i = 0; i < this.NumChannel; i++)
                            {
                                string valueStr = lightValue.Value[StartChannelIndex + i].ToString("0000");

                                byte[] bytePacket = new byte[8];
                                bytePacket[0] = 0x02;
                                bytePacket[1] = (byte)(i + '0');
                                bytePacket[2] = (byte)'w';
                                bytePacket[3] = (byte)valueStr[0];
                                bytePacket[4] = (byte)valueStr[1];
                                bytePacket[5] = (byte)valueStr[2];
                                bytePacket[6] = (byte)valueStr[3];
                                bytePacket[7] = 0x03;

                                lightSerialPort.WritePacket(bytePacket, 0, 8);
                                //Thread.Sleep(10);
                            }
                            break;

                        case LightControllerVender.PSCC:
                            for (int i = 0; i < this.NumChannel; i++)
                            {
                                string turnOnPacket = String.Format("@00L1007D\r\n");
                                byte[] turnOnByte = Encoding.UTF8.GetBytes(turnOnPacket);
                                lightSerialPort.WritePacket(turnOnByte, 0, turnOnByte.Length);
                                Thread.Sleep(100);

                                string preparePacket = String.Format("@00F{0:000}00", lightValue.Value[StartChannelIndex + i]);

                                byte[] toByte = Encoding.Default.GetBytes(preparePacket);
                                int sum = 0;
                                for (int b = 0; b < toByte.Length; b++)
                                {
                                    sum += toByte[b];
                                }

                                string checkSum = string.Format("{0:x2}", sum);
                                checkSum = checkSum.ToUpper();
                                if (checkSum.Length > 2)
                                    checkSum = checkSum.Remove(0, 1);

                                string packet = string.Format("{0}{1}\r\n", preparePacket, checkSum);

                                byte[] StrByte = Encoding.UTF8.GetBytes(packet);
                                lightSerialPort.WritePacket(StrByte, 0, StrByte.Length);
                            }
                            break;

                        case LightControllerVender.VIT:
                            for (int i = 0; i < this.NumChannel; i++)
                            {
                                string packet = String.Format("C{0}{1:000}\r\n", i + 1, lightValue.Value[i]);
                                byte[] turnOnByte = Encoding.ASCII.GetBytes(packet);
                                lightSerialPort.WritePacket(turnOnByte, 0, turnOnByte.Length);
                            }
                            break;
                        case LightControllerVender.iCore:
                            for(int i = 0;i < this.NumChannel; i++ )
                            {

                            }
                            break;
                    }

                    if (serialLightCtrlInfo.ResponceTimeoutMs != 0 && this.responseReceived.WaitOne(serialLightCtrlInfo.ResponceTimeoutMs) == false)
                        throw new Exception("Light Contoller has no responce");
                }
                catch (Exception ex)
                {
                    ErrorManager.Instance().Report(ErrorCodeLight.Instance.FailToWriteValue, ErrorLevel.Error, this.name, ex.Message, null, "Please, Check the Light Controller");
                }
            }
            Thread.Sleep(lightStableTimeMs);
            return true;
        }

        public override LightValue GetLightValue()
        {
            return curLightValue;
        }
    }
}
