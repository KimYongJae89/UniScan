using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Device.Serial
{
    public enum ESerialDeviceType { Light, SerialEncoder, BarcodeReader, SerialSensor }

    public interface ISerialDeviceInfo
    {
        string DeviceName { get; set; }

        ESerialDeviceType DeviceType { get; }

        SerialPortInfo SerialPortInfo { get; }

        int ResponceTimeoutMs { get; set; }

        PacketParser CreatePacketParser();

        string GetDeviceString();

        string GetPortFindString();
        bool IsPortFound(byte[] responce);
    }

    public abstract class SerialDeviceInfo : ISerialDeviceInfo
    {
        public string DeviceName { get; set; }

        public ESerialDeviceType DeviceType { get; private set; }

        public SerialPortInfo SerialPortInfo { get; private set; } = new SerialPortInfo();

        public int ResponceTimeoutMs { get; set; } = 0;

        public SerialDeviceInfo(ESerialDeviceType deviceType)
        {
            this.DeviceType = deviceType;
        }

        public abstract SerialDevice CreateSerialDevice(bool virtualMode);

        public abstract SerialDeviceInfo Clone();

        public abstract string GetDeviceString();

        public abstract string GetPortFindString();
        public abstract bool IsPortFound(byte[] responce);
        public abstract PacketParser CreatePacketParser();

        public virtual void CopyFrom(SerialDeviceInfo serialDeviceInfo)
        {
            this.DeviceName = serialDeviceInfo.DeviceName;
            this.DeviceType = serialDeviceInfo.DeviceType;
            this.ResponceTimeoutMs = serialDeviceInfo.ResponceTimeoutMs;
            this.SerialPortInfo.CopyFrom(serialDeviceInfo.SerialPortInfo);
        }

        public void Save(XmlElement xmlElement, string subKey = null)
        {
            if (xmlElement == null) return;
            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(subKey);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }
            SaveXml(xmlElement);
        }

        public virtual void SaveXml(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "DeviceName", this.DeviceName);
            XmlHelper.SetValue(xmlElement, "DeviceType", this.DeviceType);
            XmlHelper.SetValue(xmlElement, "ResponceTimeoutMs", this.ResponceTimeoutMs);
            this.SerialPortInfo.Save(xmlElement, "SerialPortInfo");
        }

        public void Load(XmlElement xmlElement, string subKey = null)
        {
            if (xmlElement == null) return;
            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = xmlElement[subKey];
                Load(subElement);
                return;
            }
            LoadXml(xmlElement);
        }

        public virtual void LoadXml(XmlElement xmlElement)
        {
            this.DeviceName = XmlHelper.GetValue(xmlElement, "DeviceName", "");
            this.DeviceType = XmlHelper.GetValue(xmlElement, "DeviceType", ESerialDeviceType.Light);
            this.ResponceTimeoutMs = XmlHelper.GetValue(xmlElement, "ResponceTimeoutMs", ResponceTimeoutMs);
            this.SerialPortInfo.Load(xmlElement, "SerialPortInfo");
        }
    }

    public class SerialDeviceInfoList : List<ISerialDeviceInfo>
    {
        public SerialDeviceInfoList Clone()
        {
            SerialDeviceInfoList newSerialPortInfoList = new SerialDeviceInfoList();

            foreach (SerialDeviceInfo serialPortInfo in this)
            {
                newSerialPortInfoList.Add(serialPortInfo.Clone());
            }

            return newSerialPortInfoList;
        }
    }

}
