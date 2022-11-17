using DynMvp.Base;
using DynMvp.Device.Device.Light.SerialLigth.iCore;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Light;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace DynMvp.Device.Device.Light
{
    public class SerialLightCtrlInfoIPulse : SerialLightCtrlInfo
    {
        public byte SlaveId { get => this.slaveId; set => this.slaveId = value; }
        byte slaveId;

        public OperationMode OperationMode { get => this.operationMode; set => this.operationMode = value; }
        OperationMode operationMode;

        public TriggerInputSource TriggerInputSource { get => this.triggerInputSource; set => this.triggerInputSource = value; }
        TriggerInputSource triggerInputSource;

        public TriggerInputActivation TriggerInputActivation { get => this.triggerInputActivation; set => this.triggerInputActivation = value; }
        TriggerInputActivation triggerInputActivation;

        public TiggerOutputSource TiggerOutputSource { get => this.tiggerOutputSource; set => this.tiggerOutputSource = value; }
        TiggerOutputSource tiggerOutputSource;

        public TiggerOutputInverter TiggerOutputInverter { get => this.tiggerOutputInverter; set => this.tiggerOutputInverter = value; }
        TiggerOutputInverter tiggerOutputInverter;

        public ushort MaxVoltage { get => this.maxVoltage; set => this.maxVoltage = value; }
        ushort maxVoltage;

        public float TimeDuration { get => this.timeDuration; set => this.timeDuration = value; }
        float timeDuration;

        public bool LPFMode { get => this.lPFMode; set => this.lPFMode = value; }
        bool lPFMode;

        public SeqInfo SequenceInfo { get => this.sequenceInfo; }
        SeqInfo sequenceInfo;

        public SerialLightCtrlInfoIPulse() : base(LightControllerVender.iCore)
        {
            this.slaveId = 0;
            this.operationMode = OperationMode.Continuous;
            this.triggerInputSource = TriggerInputSource.DigitalIO;
            this.triggerInputActivation = TriggerInputActivation.Rising;
            this.tiggerOutputSource = TiggerOutputSource.LED_Output_Sync;
            this.tiggerOutputInverter = TiggerOutputInverter.None;

            this.sequenceInfo = new SeqInfo();
            this.maxVoltage = 24;
            this.timeDuration = 5.0f;

            this.lPFMode = false;
        }

        public override PacketParser CreatePacketParser()
        {
            PacketParser packetParser = new SimplePacketParser();
            return packetParser;
        }

        public override string GetPortFindString()
        {
            throw new NotImplementedException();
        }

        public override bool IsPortFound(byte[] responce)
        {
            throw new NotImplementedException();
        }


        public override LightCtrlInfo Clone()
        {
            SerialLightCtrlInfoIPulse serialLightCtrlInfoIPulse = new SerialLightCtrlInfoIPulse();
            serialLightCtrlInfoIPulse.CopyFrom(this);
            return serialLightCtrlInfoIPulse;
        }

        public override void SaveXml(XmlElement lightInfoElement)
        {
            base.SaveXml(lightInfoElement);

            XmlHelper.SetValue(lightInfoElement, "SlaveId", this.slaveId);
            XmlHelper.SetValue(lightInfoElement, "OperationMode", this.operationMode);
            XmlHelper.SetValue(lightInfoElement, "TriggerInputSource", this.triggerInputSource);
            XmlHelper.SetValue(lightInfoElement, "TriggerInputActivation", this.triggerInputActivation);
            XmlHelper.SetValue(lightInfoElement, "TiggerOutputSource", this.tiggerOutputSource);
            XmlHelper.SetValue(lightInfoElement, "TiggerOutputInverter", this.tiggerOutputInverter);

            XmlHelper.SetValue(lightInfoElement, "MaxVoltage", this.maxVoltage);
            XmlHelper.SetValue(lightInfoElement, "TimeDuration", this.timeDuration);

            XmlHelper.SetValue(lightInfoElement, "LPFMode", this.lPFMode);

            this.sequenceInfo.SaveXml(lightInfoElement, "SequenceInfo");
        }

        public override void LoadXml(XmlElement lightInfoElement)
        {
            base.LoadXml(lightInfoElement);

            this.slaveId = XmlHelper.GetValue(lightInfoElement, "SlaveId", this.slaveId);

            this.operationMode = XmlHelper.GetValue(lightInfoElement, "OperationMode", this.operationMode);
            this.triggerInputSource = XmlHelper.GetValue(lightInfoElement, "TriggerInputSource", this.triggerInputSource);
            this.triggerInputActivation = XmlHelper.GetValue(lightInfoElement, "TriggerInputActivation", this.triggerInputActivation);
            this.tiggerOutputSource = XmlHelper.GetValue(lightInfoElement, "TiggerOutputSource", this.tiggerOutputSource);
            this.tiggerOutputInverter = XmlHelper.GetValue(lightInfoElement, "TiggerOutputInverter", this.tiggerOutputInverter);

            this.maxVoltage = XmlHelper.GetValue(lightInfoElement, "MaxVoltage", this.maxVoltage);
            this.timeDuration = XmlHelper.GetValue(lightInfoElement, "TimeDuration", this.timeDuration);

            this.lPFMode = XmlHelper.GetValue(lightInfoElement, "LPFMode", this.lPFMode);

            this.sequenceInfo.LoadXml(lightInfoElement, "SequenceInfo");
        }

        public override void CopyFrom(LightCtrlInfo srcInfo)
        {
            base.CopyFrom(srcInfo);

            SerialLightCtrlInfoIPulse serialLightInfoIPulse = (SerialLightCtrlInfoIPulse)srcInfo;
            
            this.slaveId = serialLightInfoIPulse.slaveId;
            this.operationMode = serialLightInfoIPulse.operationMode;

            this.maxVoltage = serialLightInfoIPulse.maxVoltage;
            this.timeDuration = serialLightInfoIPulse.timeDuration;

            this.lPFMode = serialLightInfoIPulse.lPFMode;

            this.sequenceInfo.CopyFrom(serialLightInfoIPulse.sequenceInfo);
        }

        public override Form GetAdvancedConfigForm()
        {
            return new iCoreConfigForm(this);
        }
    }

    public class SerialLightIPulse : SerialLightCtrl
    {
        List<byte> recivedByteList = null;

        private SerialLightCtrlInfoIPulse info => (SerialLightCtrlInfoIPulse)this.lightCtrlInfo;

        public override int GetMaxLightLevel()
        {
            return info.OperationMode == OperationMode.Continuous ? 100 : 1000;
        }

        public SerialLightIPulse(SerialLightCtrlInfo serialLightCtrlInfo) : base(serialLightCtrlInfo)
        {
            this.recivedByteList = new List<byte>();
        }

        public override bool Initialize()
        {
            bool ok = base.Initialize();
            if (!ok)
                return false;

            /*
             * this.slaveId = 0;
             * this.operationMode = OperationMode.Continuous;
             * this.triggerInputSource = TriggerInputSource.DigitalIO;
             * this.triggerInputActivation = TriggerInputActivation.Rising;
             * this.tiggerOutputSource = TiggerOutputSource.LED_Output_Sync;
             * this.tiggerOutputInverter = TiggerOutputInverter.None;
             */

            List<IPulseFrame> frameList = new List<IPulseFrame>();
            SendCommand(IPulseFrame.CreateWFrame(info.SlaveId, Address.OperationMode_RW, info.OperationMode));
            SendCommand(IPulseFrame.CreateWFrame(info.SlaveId, Address.Voltage_RW, info.MaxVoltage));
            SendCommand(IPulseFrame.CreateWFrame(info.SlaveId, Address.Duration_RW, info.TimeDuration));
            SendCommand(IPulseFrame.CreateWFrame(info.SlaveId, Address.LPFMode_RW, info.LPFMode));
            SendCommand(IPulseFrame.CreateWFrame(info.SlaveId, Address.TriggerInputSource_RW, info.TriggerInputSource));

            SendCommand(IPulseFrame.CreateWFrame(info.SlaveId, Address.SequenceMode_RW, info.SequenceInfo.Mode));
            if (info.SequenceInfo.Mode != SequenceMode.Off)
            {
                SendCommand(IPulseFrame.CreateWFrame(info.SlaveId, Address.SequenceNumber_RW, info.SequenceInfo.Count));
                SendCommand(IPulseFrame.CreateWFrame(info.SlaveId, Address.SequenceData_RW, info.SequenceInfo.Sequences.Take(info.SequenceInfo.Count).ToArray()));
            }

            SendCommand(frameList.ToArray());
            return true;
        }

        protected override void lightSerialPort_PacketReceived(ReceivedPacket receivedPacket)
        {
            //string[] ss = Array.ConvertAll(receivedPacket.ReceivedData, f => f.ToString("X02"));
            //System.Diagnostics.Debug.WriteLine(string.Format("lightSerialPort_PacketReceived - [{0}] {1}", ss.Length, string.Join(", ", ss)));
            byte[] sent = (byte[])this.sent;
            if (sent == null)
                return;

            this.recivedByteList.AddRange(receivedPacket.ReceivedData);
            if (this.recivedByteList.Count > 3)
            {
                // 받은 명령어의 앞 2개 바이트가 보낸 명령어의 앞 2개 바이트와 일치하는지 확인.
                int idx0 = this.recivedByteList.IndexOf(sent[0]);
                if (idx0 < 0)
                    return;
                int idx1 = this.recivedByteList.IndexOf(sent[1], idx0);
                if (idx1 != idx0 + 1)
                    return;

                int totlaLength;
                if (sent[1] == (byte)Function.Write)
                {
                    // Write 응답: 받은 명령어의 전체 길이와 보낸 명령어의 전체 길이 확인.
                    if (this.recivedByteList.Count - idx0 < sent.Length)
                        return;

                    totlaLength = sent.Length;
                }
                else
                {
                    // Read 응답: 받은 명령어의 3번째 바이트의 값과 전체 데이터의 길이 확인.
                    if (this.recivedByteList.Count < idx0 + 3)
                        return;

                    int dataLength = this.recivedByteList[idx1 + 1];
                    totlaLength = 2 + 1 + dataLength + 2; // 헤더 2, 길이 1, 데이터 n, crc 2
                }

                if (this.recivedByteList.Count < idx0 + totlaLength)
                    return;
                this.responce = this.recivedByteList.GetRange(idx0, totlaLength).ToArray();
                this.recivedByteList.RemoveRange(idx0, totlaLength);
                this.responseReceived.Set();
            }
        }

        public IPulseFrame[] SendCommand(IPulseFrame[] frames)
        {
            SerialLightCtrlInfo serialLightCtrlInfo = (SerialLightCtrlInfo)this.lightCtrlInfo;
            IPulseFrame[] responces = frames.Select(f => SendCommand(f)).ToArray();
            return responces;
        }

        public IPulseFrame SendCommand(IPulseFrame frame)
        {
            this.responseReceived.Reset();
            this.responce = null;
            this.sent = null;

            byte[] bytes = frame.ToBytes();

            //string[] ss = Array.ConvertAll(bytes, g => g.ToString("X02"));
            //System.Diagnostics.Debug.WriteLine(string.Format("SerialLightCtrlIPulse          - [{0}] {1}", ss.Length, string.Join(", ", ss)));

            this.sent = bytes;
            this.LightSerialPort.WritePacket(bytes, 0, bytes.Length);
            if (frame.IsRead || this.SerialLightCtrlInfo.ResponceTimeoutMs != 0)
            // Wait Responce
            {
                int timeout = this.SerialLightCtrlInfo.ResponceTimeoutMs;
                if (timeout == 0)
                    timeout = 1000;
                bool waitDone = this.responseReceived.WaitOne(timeout);
                if (!waitDone)
                    throw new Exception("Light Contoller has no responce");

                IPulseFrame recivedFrame = new IPulseFrame(frame.IsRead, (byte[])this.responce);
                return recivedFrame;
            }
            return null;
        }

        protected override bool SetLightValue(LightValue lightValue)
        {
            SerialLightCtrlInfo serialLightCtrlInfo = (SerialLightCtrlInfo)this.lightCtrlInfo;
            lightValue.Clip(GetMaxLightLevel());

            List<IPulseFrame> frameList = new List<IPulseFrame>();
            frameList.Add(IPulseFrame.CreateWFrame(info.SlaveId, Address.OperationMode_RW, (ushort)info.OperationMode));
            Address startAddress = info.OperationMode == OperationMode.Continuous ? Address.LED1CurrentRateContinuous_RW : Address.LED1CurrentRatePulse_RW;

            for (int i = 0; i < lightValue.NumLight; i++)
            {
                bool turnOn = lightValue.Value[i] > 0;
                frameList.Add(IPulseFrame.CreateWFrame(info.SlaveId, Address.LED1Enable_RW + i, turnOn));
                if (turnOn)
                    frameList.Add(IPulseFrame.CreateWFrame(info.SlaveId, startAddress + i, (ushort)lightValue.Value[i]));
            }

            SendCommand(frameList.ToArray());
            return true;
        }

        public override LightValue GetLightValue()
        {
            List<IPulseFrame> frameList = new List<IPulseFrame>();
            for (int i = 0; i < this.lightCtrlInfo.NumChannel; i++)
                frameList.Add(IPulseFrame.CreateRFrame(9, Address.LED1Enable_RW + i, 0x0001));
            return base.GetLightValue();
        }
    }
}
