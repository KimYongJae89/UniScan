using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;

namespace DynMvp.Devices.Dio
{
    public enum IoGroup { General, Door, Light }
    public class IoPort
    {
        public const int UNUSED_PORT_NO = -1;
        public static IoPort UnkownPort = new IoPort("Unknown");

        private int deviceNo;
        public int DeviceNo
        {
            get { return deviceNo; }
            set { deviceNo = value; }
        }

        private int groupNo;
        public int GroupNo
        {
            get { return groupNo; }
            set { groupNo = value; }
        }

        private int portNo = UNUSED_PORT_NO;
        public int PortNo
        {
            get { return portNo; }
            set { portNo = value; }
        }

        private bool activeLow = false;
        public bool ActiveLow
        {
            get { return activeLow; }
            set { activeLow = value; }
        }

        private IoGroup group = IoGroup.General;
        public IoGroup Group
        {
            get { return group; }
            set { group = value; }
        }

        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        //public IoPort(IoOutPortName portName)
        //{
        //    this.name = portName.ToString();
        //    this.portNo = UNUSED_PORT_NO;
        //    this.deviceNo = 0;
        //    this.groupNo = 0;
        //}
        private IoPort() { }
        public IoPort(Enum ioPortName, int portNo = UNUSED_PORT_NO, int deviceNo = 0, int groupNo = 0, bool invert = false)
        {
            this.name = ioPortName.ToString();
            this.portNo = portNo;
            this.deviceNo = deviceNo;
            this.groupNo = groupNo;
            this.activeLow = invert;
        }

        public IoPort(string name, int portNo = UNUSED_PORT_NO, int deviceNo = 0, int groupNo = 0, bool invert = false)
        {
            this.name = name;
            this.portNo = portNo;
            this.deviceNo = deviceNo;
            this.groupNo = groupNo;
            this.activeLow = invert;
        }

        public void Set(int portNo, int deviceNo = 0)
        {
            this.portNo = portNo;
            this.groupNo = 0;
            this.deviceNo = deviceNo;
        }

        public void Set(int portNo, int groupNo, int deviceNo)
        {
            this.portNo = portNo;
            this.groupNo = groupNo;
            this.deviceNo = deviceNo;
        }

        public bool IsValid()
        {
            return portNo != UNUSED_PORT_NO;
        }

        public void SaveXml(XmlElement ioPortElement, string key = null)
        {
            if (ioPortElement == null)
                return;

            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = ioPortElement.OwnerDocument.CreateElement(key);
                ioPortElement.AppendChild(subElement);
                SaveXml(subElement);
                return;
            }

            XmlHelper.SetValue(ioPortElement, "Name", name);
            XmlHelper.SetValue(ioPortElement, "PortNo", portNo.ToString());
            XmlHelper.SetValue(ioPortElement, "GroupNo", groupNo.ToString());
            XmlHelper.SetValue(ioPortElement, "DeviceNo", deviceNo.ToString());
            XmlHelper.SetValue(ioPortElement, "Invert", activeLow.ToString());
            XmlHelper.SetValue(ioPortElement, "Group", group.ToString());
        }

        public void LoadXml(XmlElement ioPortElement, string key = null)
        {
            if (ioPortElement == null)
                return;

            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = ioPortElement[key];
                LoadXml(subElement);
                return;
            }

            name = XmlHelper.GetValue(ioPortElement, "Name", "");
            portNo = Convert.ToInt32(XmlHelper.GetValue(ioPortElement, "PortNo", "0"));
            groupNo = Convert.ToInt32(XmlHelper.GetValue(ioPortElement, "GroupNo", "0"));
            deviceNo = Convert.ToInt32(XmlHelper.GetValue(ioPortElement, "DeviceNo", "0"));
            activeLow = Convert.ToBoolean(XmlHelper.GetValue(ioPortElement, "Invert", "false"));
            group = (IoGroup)Enum.Parse(typeof(IoGroup), XmlHelper.GetValue(ioPortElement, "Group", IoGroup.General.ToString()));
        }

        public static IoPort Load(XmlElement xmlElement, string key = null)
        {
            IoPort ioPort = new IoPort();
            ioPort.LoadXml(xmlElement, key);
            return ioPort;
        }

        public override bool Equals(object obj)
        {
            if (obj is IoPort)
            {
                IoPort ioPort = (IoPort)obj;
                return ioPort.portNo == this.portNo
                    && ioPort.groupNo == this.groupNo
                    && ioPort.deviceNo == this.deviceNo
                    && ioPort.portNo == this.portNo
                    && ioPort.group == this.group;
            }
            return false;
        }

        public IoPort Clone()
        {
            IoPort ioPort = new IoPort();
            ioPort.CopyFrom(this);
            return ioPort;
        }

        internal void CopyFrom(IoPort ioPort)
        {

            this.deviceNo = ioPort.deviceNo;
            this.groupNo = ioPort.groupNo;
            this.portNo = ioPort.portNo;
            this.activeLow = ioPort.activeLow;
            this.group = ioPort.group;
            this.name = ioPort.name;
        }
    }
}