using DynMvp.Base;
using DynMvp.Device.Device.Serial;
using DynMvp.Devices.Comm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Device.Serial
{

    public class SerialDevice
    {
        //public EncodePacketDelegate EncodePacket = null;
        //public DecodePacketDelegate DecodePacket = null;

        protected SerialDeviceInfo deviceInfo = null;
        public SerialDeviceInfo DeviceInfo
        {
            get { return deviceInfo; }
            set { deviceInfo = value; }
        }

        //public bool IsAlarmed => this.isAlarmed;
        //protected bool isAlarmed;

        protected SerialPortEx serialPortEx = null;
        public SerialPortEx SerialPortEx
        {
            get { return serialPortEx; }
        }

        object lockObject = new object();
        protected ManualResetEvent waitResponce = new ManualResetEvent(false);
        protected ReceivedPacket lastResponce = null;
        protected TimeOutTimer timeOutTimer = null;

        public SerialDevice(SerialDeviceInfo deviceInfo)
        {
            this.deviceInfo = deviceInfo;
            this.timeOutTimer = new TimeOutTimer();
            ErrorManager.Instance().OnStartAlarmState += ErrorManager_OnStartAlarm;
            ErrorManager.Instance().OnResetAlarmState += ErrorManager_OnResetAlarmStatus;
        }

        private void ErrorManager_OnStartAlarm()
        {
            //this.isAlarmed = true;
        }

        private void ErrorManager_OnResetAlarmStatus()
        {
            //this.isAlarmed = false;
        }

        public bool IsReady()
        {
            return this.deviceInfo.SerialPortInfo.IsVirtual || (this.serialPortEx != null && this.serialPortEx.IsOpen);
        }

        public virtual bool Initialize()
        {
            PacketParser packetParser = this.deviceInfo.CreatePacketParser();
            packetParser.DataReceived += packetParser_OnDataReceived;
            this.serialPortEx = new SerialPortEx(packetParser);

            if (this.deviceInfo.SerialPortInfo.IsVirtual)
            {
                return true;
            }

            serialPortEx.Open(deviceInfo.DeviceName, deviceInfo.SerialPortInfo);
            serialPortEx.StartListening();
            
            return true;
        }

        protected virtual void packetParser_OnDataReceived(ReceivedPacket receivedPacket)
        {
            this.lastResponce = receivedPacket;
            waitResponce.Set();
        }

        public virtual void Release()
        {
            serialPortEx.StopListening();
            serialPortEx.Close();
        }

        public virtual Enum GetCommand(string command) { throw new NotImplementedException(); }
        public virtual string MakePacket(string command, params string[] args) { throw new NotImplementedException(); }

        public string ExcuteCommand(Enum command, params string[] args)
        {
            string excuteResult = ExcuteCommand(command.ToString(), args);
            return excuteResult;
        }

        public string ExcuteCommand(string command, params string[] args)
        {
            string packetString = MakePacket(command, args);
            return ExcuteCommand(packetString);
        }

        public string ExcuteCommand(string packetString)
        {
            //if (this.isAlarmed)
            //    return null;

            lock (lockObject)
            {
                lastResponce = null;
                waitResponce.Reset();
                LogHelper.Debug(LoggerType.Serial, string.Format("SerialDevice::SendCommand - Name: {0}, Packet: {1}", this.deviceInfo.DeviceName, packetString.Trim()));
                bool sendOk = SendCommand(packetString);
                if (sendOk == false)
                    return null;

                bool waitOk = WaitResponce(500);
                if (waitOk == false)
                {
                    LogHelper.Error(LoggerType.Serial, string.Format("SerialDevice({0} responce timeout)", this.deviceInfo.DeviceName));
                    //this.isAlarmed = true;
                    //ErrorManager.Instance().Report(ErrorSections.Machine, (int)MachineError.Serial,
                    //   ErrorLevel.Fatal, MachineError.Serial.ToString(), this.deviceInfo.DeviceName, "Serial Device Responce Timeout");
                    //throw new TimeoutException(string.Format("SerialDevice {0}", this.deviceInfo.DeviceName));
                    return null;
                    //return new string[0];
                }

                string excuteResult = this.serialPortEx.PacketHandler.PacketParser.DecodePacket(lastResponce.ReceivedData);
                LogHelper.Debug(LoggerType.Serial, string.Format("SerialDevice::SendCommand - Name: {0}, Result: {1}", this.deviceInfo.DeviceName, excuteResult.Trim()));
                lastResponce = null;
                return excuteResult;
            }
        }

        protected virtual bool SendCommand(string packetString)
        {
            //if (this.isAlarmed)
            //    return false;

            byte[] packet = serialPortEx.PacketHandler.PacketParser.EncodePacket(packetString);
            return serialPortEx.WritePacket(packet, 0, packet.Length);
        }

        protected virtual bool SendCommand(byte[] bytes)
        {
            //if (this.isAlarmed)
            //    return false;

            this.waitResponce.Reset();
            return serialPortEx.WritePacket(bytes, 0, bytes.Length);
        }

        protected bool WaitResponce(int waitTimeMs = -1)
        {
            return waitResponce.WaitOne(waitTimeMs);
        }
    }

    public class SerialDeviceHandler : IEnumerable
    {
        List<SerialDevice> serialDeviceList = new List<SerialDevice>();

        public void Add(SerialDevice serialDevice)
        {
            serialDeviceList.Add(serialDevice);
        }

        public SerialDevice Find(Predicate<SerialDevice> p)
        {
            return serialDeviceList.Find(p);
        }

        public IEnumerator GetEnumerator()
        {
            return serialDeviceList.GetEnumerator();
        }

        public SerialDevice this[int i] { get => serialDeviceList[i]; set => serialDeviceList[i] = value; }

        public int Count { get => serialDeviceList.Count(); }

        public void Release()
        {
            foreach (SerialDevice serialDevice in serialDeviceList)
            {
                if (serialDevice.IsReady())
                    serialDevice.Release();
            }
            serialDeviceList.Clear();
        }
    }
}
