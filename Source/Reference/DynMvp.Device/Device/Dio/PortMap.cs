using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using DynMvp.Base;
using DynMvp.Devices;
using System.Diagnostics;

namespace DynMvp.Devices.Dio
{
    public class PortList
    {
        protected List<IoPort> ioPortList = new List<IoPort>();
        public List<IoPort> IoPortList
        {
            get { return ioPortList; }
        }

        public IEnumerator<IoPort> GetEnumerator()
        {
            return ioPortList.GetEnumerator();
        }
        
        public IoPort GetIoPort(string portName)
        {
            IoPort ioPort = ioPortList.Find(x => x.Name == portName);

            //Debug.Assert(ioPort != null, String.Format("I/O Port {0} is not defined.", portName));

            return ioPort;
        }

        public List<IoPort> GetIoPorts(string portName)
        {
            List<IoPort> ioPortList = this.ioPortList.FindAll(x => x.Name.Contains(portName));

            Debug.Assert(ioPortList != null, String.Format("I/O Port {0} is not defined.", portName));

            return ioPortList;
        }

        public List<IoPort> GetIoPorts(IoGroup group)
        {
            List<IoPort> ioPortList = this.ioPortList.FindAll(x => x.Group == group);

            //Debug.Assert(ioPortList.Count > 0, String.Format("I/O Port {0} is not defined.", group.ToString()));

            return ioPortList;
        }

        public IoPort GetIoPort(int portNo, int groupNo = 0, int deviceNo = 0)
        {
            IoPort ioPort = ioPortList.Find(x =>
            {
                return (x.PortNo == portNo) && (x.GroupNo == groupNo) && (x.DeviceNo == deviceNo);
            });

            return ioPort;
        }

        public void AddIoPort(IoPort ioPort)
        {
            //if (ioPortList.Exists(f => f.Equals(ioPort) == false))
                ioPortList.Add(ioPort);
        }

        public string GetPortName(int deviceNo, int portNo)
        {
            IoPort ioPort = ioPortList.Find(x => x.DeviceNo == deviceNo && x.PortNo == portNo);

            if (ioPort == null)
                return "";

            return ioPort.Name;
        }

        public bool SetPort(string portName, int deviceNo, int portNo, bool activeLow)
        {
            IoPort ioPort = ioPortList.Find(x => x.Name == portName);

            if (ioPort == null)
            {
                ioPort = new IoPort(portName);
                this.AddIoPort(ioPort);
            }

            ioPort.DeviceNo = deviceNo;
            ioPort.PortNo = portNo;
            ioPort.ActiveLow = activeLow;

            return true;
        }

        public void ClearPort()
        {
            ioPortList.Clear();
        }

        public void ClearPort(string portName)
        {
            ioPortList.RemoveAll(x => x.Name == portName);
        }

        public void ClearPort(int portNo)
        {
            ioPortList.RemoveAll(x => x.PortNo == portNo);
        }

        private int GetMaxPortNo()
        {
            return ioPortList.Max(x => x.PortNo);
        }

        public List<string> GetPortNames(int deviceIndex, int groupIndex, int numPorts)
        {
            string[] portNames = new string[numPorts];
            ioPortList.ForEach(x =>
            {
                if (x.PortNo != IoPort.UNUSED_PORT_NO && x.PortNo <numPorts&& x.GroupNo == groupIndex && x.DeviceNo == deviceIndex)
                {
                    if (string.IsNullOrEmpty(portNames[x.PortNo]))
                        portNames[x.PortNo] = x.Name;
                    else
                        portNames[x.PortNo] = string.Join(", ", new string[] { portNames[x.PortNo], x.Name });
                }
            });

            return portNames.ToList();
        }

        public List<string> GetPortNames()
        {
            List<string> portNames = new List<string>();
            ioPortList.ForEach(x => { portNames.Add(x.Name); });

            return portNames;
        }

        public void Save(XmlElement portListElement)
        {
            foreach (IoPort ioPort in ioPortList)
            {
                XmlElement portElement = portListElement.OwnerDocument.CreateElement("", "Port", "");
                portListElement.AppendChild(portElement);

                XmlHelper.SetValue(portElement, "Name", ioPort.Name);
                XmlHelper.SetValue(portElement, "DeviceNo", ioPort.DeviceNo.ToString());
                XmlHelper.SetValue(portElement, "PortNo", ioPort.PortNo.ToString());
                XmlHelper.SetValue(portElement, "ActiveLow", ioPort.ActiveLow.ToString());
            }
        }

        public void Load(XmlElement portListElement)
        {
            if (portListElement == null)
                return;

            foreach (XmlElement portElement in portListElement)
            {
                if (portElement.Name == "Port")
                {
                    IoPort xmlIoPort = IoPort.Load(portElement);
                    //string portName = XmlHelper.GetValue(portElement, "Name", "");
                    //int deviceNo = Convert.ToInt32(XmlHelper.GetValue(portElement, "DeviceNo", "0"));
                    //int portNo = Convert.ToInt32(XmlHelper.GetValue(portElement, "PortNo", "-1"));
                    //bool activeLow = Convert.ToBoolean(XmlHelper.GetValue(portElement, "ActiveLow", "false"));

                    // ioPortList에 존재하는 것만 로드함 -> 정의되지 않은 포트 이름은 무시
                    //if (String.IsNullOrEmpty(portName) == false)
                    //    SetPort(portName, deviceNo, portNo, invert);

                    IoPort ioPort = ioPortList.Find(f => f.Name == xmlIoPort.Name);
                    if (ioPort != null)
                        ioPort.CopyFrom(xmlIoPort);
                }
            }
        }

        internal void Clear()
        {
            ioPortList.Clear();
        }
    }



    public class PortMapBase
    {
        protected string xmlFilePath;
        //Type ioPortNameType=null;
        //public Type IoPortNameType
        //{
        //    get { return ioPortNameType; }
        //    set { ioPortNameType = value; }
        //}

        PortList inPortList = new PortList();
        public PortList InPortList
        {
            get { return inPortList; }
            set { inPortList = value; }
        }

        PortList outPortList = new PortList();
        public PortList OutPortList
        {
            get { return outPortList; }
            set { outPortList = value; }
        }

        public PortMapBase(string xmlFilePath)
        {
            this.xmlFilePath = xmlFilePath;
        }

        public virtual void Initialize(Type ioPortNameType)
        {
            AddInPorts(ioPortNameType);
            AddOutPorts(ioPortNameType);
        }
        
        private void AddInPorts(Type ioPortNameType)
        {
            string[] names = Enum.GetNames(ioPortNameType);
            string[] inNames = Array.FindAll(names, f => f.StartsWith("In"));
            Array.ForEach(inNames, f => AddInPort(new IoPort(f, IoPort.UNUSED_PORT_NO)));
            //int index = 0;
            //foreach(string name in names)
            //    if()
            //        AddInPort(new IoPort(name, index++));
        }

        private void AddOutPorts(Type ioPortNameType)
        {
            string[] names = Enum.GetNames(ioPortNameType);
            string[] outNames = Array.FindAll(names, f => f.StartsWith("Out"));
            Array.ForEach(outNames, f => AddOutPort(new IoPort(f, IoPort.UNUSED_PORT_NO)));
            //foreach (string name in names)
            //    if(name.Substring(0,3)=="Out")
            //        AddOutPort(new IoPort(name));
        }

        public IoPort GetInPort(Enum ioPortName)
        {
            return inPortList.GetIoPort(ioPortName.ToString());
        }

        public IoPort GetInPort(string portName)
        {
            return inPortList.GetIoPort(portName);
        }

        public IoPort GetInPort(int portNo, int groupNo = 0, int deviceNo = 0)
        {
            return inPortList.GetIoPort(portNo, groupNo, deviceNo);
        }

        public IoPort GetOutPort(Enum ioPortName)
        {
            return outPortList.GetIoPort(ioPortName.ToString());
        }

        public IoPort GetOutPort(string portName)
        {
            return outPortList.GetIoPort(portName);
        }
        
        public IoPort GetOutPort(int portNo, int groupNo = 0, int deviceNo = 0)
        {
            return outPortList.GetIoPort(portNo, groupNo, deviceNo);
        }

        public IoPort AddInPort(IoPort ioPort)
        {
            inPortList.AddIoPort(ioPort);
            return ioPort;
        }

        public IoPort AddOutPort(IoPort ioPort)
        {
            outPortList.AddIoPort(ioPort);
            return ioPort;
        }

        public virtual IoPort[] GetTowerLampPort()
        {
            return null;
        }

        public void Save(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = this.xmlFilePath;

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement portListElement = xmlDocument.CreateElement("", "PortList", "");
            xmlDocument.AppendChild(portListElement);

            Save(portListElement);

            XmlHelper.Save(xmlDocument, fileName);
        }

        public void Load(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = this.xmlFilePath;

            if (!File.Exists(fileName))
                return;

            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            XmlElement portListElement = xmlDocument?.DocumentElement;

            if (portListElement == null)
                return;

            Load(portListElement);
            //if (inPortList != null)
            //    inPortList.Load(portListElement["Inport"]);

            //if (outPortList != null)
            //    outPortList.Load(portListElement["Outport"]);
        }

        public virtual void Load(XmlElement xmlElement)
        {
            if (inPortList != null)
                inPortList.Load(xmlElement["Inport"]);

            if (outPortList != null)
                outPortList.Load(xmlElement["Outport"]);
        }

        public virtual void Save(XmlElement xmlElement)
        {
            if (inPortList != null)
            {
                XmlElement inportListElement = xmlElement.OwnerDocument.CreateElement("", "Inport", "");
                xmlElement.AppendChild(inportListElement);

                inPortList.Save(inportListElement);
            }

            if (outPortList != null)
            {
                XmlElement outportListElement = xmlElement.OwnerDocument.CreateElement("", "Outport", "");
                xmlElement.AppendChild(outportListElement);

                outPortList.Save(outportListElement);
            }

        }

        public virtual void GetIoLightPorts(List<IoPort> lightPortList)
        {

        }

        public virtual void GetInDoorPorts(List<IoPort> doorPortList)
        {
            doorPortList.AddRange(InPortList.GetIoPorts(IoGroup.Door));
            doorPortList.AddRange(InPortList.GetIoPorts("Door"));
        }

        public virtual void GetOutDoorPorts(List<IoPort> doorPortList)
        {
            doorPortList.AddRange(outPortList.GetIoPorts(IoGroup.Door));
            doorPortList.AddRange(outPortList.GetIoPorts("Door"));
        }

    }
}
