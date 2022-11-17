using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using DynMvp.Base;

namespace DynMvp.Devices.Comm
{
    public class VisorInfo
    {
        string name = String.Empty;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string ipAddress = String.Empty;
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }

        int dataPort = 2005;
        public int DataPort
        {
            get { return dataPort; }
            set { dataPort = value; }
        }

        int commandPort = 2006;
        public int CommandPort
        {
            get { return commandPort; }
            set { commandPort = value; }
        }

        string resultPath;
        public string ResultPath
        {
            get { return resultPath; }
            set { resultPath = value; }
        }

        public VisorInfo()
        {
            resultPath = String.Format("{0}\\..\\Result\\Visor", Environment.CurrentDirectory);
            if (Directory.Exists(resultPath) == false)
                Directory.CreateDirectory(resultPath);
        }


        public void Load(XmlElement xmlElement)
        {
            name = XmlHelper.GetValue(xmlElement, "Name", "");
            ipAddress = XmlHelper.GetValue(xmlElement, "IpAddress", "");
            dataPort = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "DataPort", "2005"));
            commandPort = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "CommandPort", "2006"));

            string resultPath = XmlHelper.GetValue(xmlElement, "VisorResult", this.resultPath);
            if (Directory.Exists(resultPath) == true)
                this.resultPath = resultPath;
        }

        public void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Name", name);
            XmlHelper.SetValue(xmlElement, "IpAddress", IpAddress);
            XmlHelper.SetValue(xmlElement, "DataPort", dataPort.ToString());
            XmlHelper.SetValue(xmlElement, "CommandPort", commandPort.ToString());
            XmlHelper.SetValue(xmlElement, "VisorResult", resultPath);
        }

        public VisorInfo Clone()
        {
            VisorInfo cloneVisor = new VisorInfo();
            cloneVisor.Name = name;
            cloneVisor.IpAddress = ipAddress;
            cloneVisor.DataPort = dataPort;
            cloneVisor.CommandPort = commandPort;
            cloneVisor.resultPath = resultPath;
            
            return cloneVisor;
        }
    }
}
