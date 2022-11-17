using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;

namespace DynMvp.Devices.Comm
{
    public struct TcpIpInfo
    {
        string ipAddress;
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        int portNo;
        public int PortNo
        {
            get { return portNo; }
            set { portNo = value; }
        }
        
        public TcpIpInfo(string ipAddress, int portNo)
        {
            this.ipAddress = ipAddress;
            this.portNo = portNo;
        }

        public TcpIpInfo Clone()
        {
            return new TcpIpInfo(this.ipAddress, this.portNo);
        }

        public void Load(XmlElement xmlElement, string key = null)
        {
            if (xmlElement == null)
                return;

            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = xmlElement[key];
                Load(subElement);
                return;
            }

            ipAddress = XmlHelper.GetValue(xmlElement, "IpAddress", "");
            portNo = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "PortNo", "0"));
        }

        public void Save(XmlElement xmlElement, string key = null)
        {
            if (xmlElement == null)
                return;

            if (string.IsNullOrEmpty(key) == false)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            XmlHelper.SetValue(xmlElement, "IpAddress", ipAddress);
            XmlHelper.SetValue(xmlElement, "PortNo", portNo.ToString());
        }
    }
}
