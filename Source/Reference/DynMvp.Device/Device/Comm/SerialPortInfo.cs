using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO.Ports;

using DynMvp.Base;
using DynMvp.Device.Serial;

namespace DynMvp.Devices.Comm
{
    public class SerialPortInfo
    {
        string portName;
        public string PortName
        {
            get { return portName; }
            set { portName = value; }
        }

        public int PortNo
        {
            get
            {
                if (portName == "None"|| portName == "Virtual")
                    return -1;
                return int.Parse(string.Join("", portName.Where(Char.IsDigit)));
            }
        }

        int baudRate;
        public int BaudRate
        {
            get { return baudRate; }
            set { baudRate = value; }
        }

        StopBits stopBits;
        public StopBits StopBits
        {
            get { return stopBits; }
            set { stopBits = value; }
        }

        Parity parity;
        public Parity Parity
        {
            get { return parity; }
            set { parity = value; }
        }

        int dataBits;
        public int DataBits
        {
            get { return dataBits; }
            set { dataBits = value; }
        }

        private Handshake handshake;
        public Handshake Handshake
        {
          get { return handshake; }
          set { handshake = value; }
        }

        private bool rtsEnable = false;
        public bool RtsEnable
        {
            get { return rtsEnable; }
            set { rtsEnable = value; }
        }

        private bool dtrEnable = false;
        public bool DtrEnable
        {
            get { return dtrEnable; }
            set { dtrEnable = value; }
        }

        public bool IsVirtual => this.PortNo < 0;

        public SerialPortInfo(string portName = "None", int baudRate = 9600, StopBits stopBits = StopBits.One, Parity parity = Parity.None, int dataBits = 8,
            Handshake handshake = Handshake.None, bool rtsEnable = false, bool dtrEnable = false)
        {
            Initialize(portName, baudRate, stopBits, parity, dataBits, handshake, rtsEnable, dtrEnable);
        }

        public void Initialize(string portName = "None", int baudRate = 9600, StopBits stopBits = StopBits.One, Parity parity = Parity.None, int dataBits = 8,
            Handshake handshake = Handshake.None, bool rtsEnable = false, bool dtrEnable = false)
        { 
            this.portName = portName;
            this.baudRate = baudRate;
            this.stopBits = stopBits;
            this.parity = parity;
            this.dataBits = dataBits;
            this.handshake = handshake;
            this.rtsEnable = rtsEnable;
            this.dtrEnable = dtrEnable;
        }

        public SerialPortInfo Clone()
        {
            SerialPortInfo serialPortInfo = new SerialPortInfo();
            serialPortInfo.CopyFrom(this);

            return serialPortInfo;
        }

        public void CopyFrom(SerialPortInfo srcInfo)
        {
            portName = srcInfo.portName;
            baudRate = srcInfo.baudRate;
            stopBits = srcInfo.stopBits;
            parity = srcInfo.parity;
            dataBits = srcInfo.dataBits;
            handshake = srcInfo.handshake;
            rtsEnable = srcInfo.rtsEnable;
            dtrEnable = srcInfo.dtrEnable;
        }

        public void Load(XmlElement xmlElement, string keyName)
        {
            XmlElement serialPortElement = xmlElement[keyName];
            if (serialPortElement == null)
                return;

            Load(serialPortElement);
        }

        public void Load(XmlElement xmlElement)
        {
            portName = XmlHelper.GetValue(xmlElement, "PortName", "None");
            baudRate = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "BaudRate", "9600"));
            stopBits = (StopBits)Enum.Parse(typeof(StopBits), XmlHelper.GetValue(xmlElement, "StopBits", "One"));
            parity = (Parity)Enum.Parse(typeof(Parity), XmlHelper.GetValue(xmlElement, "Parity", "None"));
            dataBits = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "DataBits", "8"));
            handshake = (Handshake)Enum.Parse(typeof(Handshake), XmlHelper.GetValue(xmlElement, "HandShake", "None"));
            rtsEnable = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "RtsEnable", "False"));
            dtrEnable = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "DtrEnable", "False"));
        }

        public void Save(XmlElement xmlElement, string keyName)
        {
            XmlElement serialPortElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(serialPortElement);

            Save(serialPortElement);
        }

        public void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "PortName", portName);
            XmlHelper.SetValue(xmlElement, "BaudRate", baudRate.ToString());
            XmlHelper.SetValue(xmlElement, "StopBits", stopBits.ToString());
            XmlHelper.SetValue(xmlElement, "Parity", parity.ToString());
            XmlHelper.SetValue(xmlElement, "DataBits", dataBits.ToString());
            XmlHelper.SetValue(xmlElement, "HandShake", handshake.ToString());
            XmlHelper.SetValue(xmlElement, "RtsEnable", rtsEnable.ToString());
            XmlHelper.SetValue(xmlElement, "DtrEnable", dtrEnable.ToString());
        }

        public override string ToString()
        {
            return String.Format("{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}",portName, baudRate.ToString(), stopBits.ToString(), parity.ToString(), dataBits.ToString(),
                handshake.ToString(), rtsEnable.ToString(), dtrEnable.ToString());
        }
    }
}
