using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanS.Common.Data;
using UniScanS.Common.Exchange;

namespace UniScanS.Common.Settings.Monitor
{
    public class MonitorSystemSettings
    {
        TcpIpMachineIfSetting serverSetting;
        public TcpIpMachineIfSetting ServerSetting
        {
            get { return serverSetting; }
            set { serverSetting = value; }
        }

        static MonitorSystemSettings _instance;
        public static MonitorSystemSettings Instance()
        {
            if (_instance == null)
                _instance = new MonitorSystemSettings();

            return _instance;
        }

        List<InspectorInfo> inspectorInfoList = new List<InspectorInfo>();
        public List<InspectorInfo> InspectorInfoList
        {
            get { return inspectorInfoList; }
            set { inspectorInfoList = value; }
        }

        string vncPath;
        public string VncPath
        {
            get { return vncPath; }
            set { vncPath = value; }
        }
        
        public MonitorSystemSettings()
        {
            serverSetting = new TcpIpMachineIfSetting(MachineIfType.TcpServer);
            serverSetting.TcpIpInfo = new TcpIpInfo(AddressManager.GetMonitorAddress(), 6000);

            ExchangeProtocolList exchangeProtocolList = new ExchangeProtocolList(typeof(ExchangeCommand));
            exchangeProtocolList.Initialize(MachineIfType.TcpServer);

            serverSetting.MachineIfProtocolList = exchangeProtocolList;
        }

        public void Load()
        {
            string configFileName = String.Format(@"{0}\SystemConfig(Monitor).xml", PathSettings.Instance().Config);
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;
            
            XmlElement configXmlElement = xmlDocument["Config"];
            
            vncPath = XmlHelper.GetValue(configXmlElement, "VncPath", "");

            foreach (XmlElement inspectorInfoElement in configXmlElement)
            {
                if (inspectorInfoElement.Name == "InspectorInfo")
                {
                    InspectorInfo inspectorInfo = new InspectorInfo();
                    inspectorInfo.Load(inspectorInfoElement);
                    inspectorInfoList.Add(inspectorInfo);
                }
            }

            XmlElement serverXmlElement = configXmlElement["Server"];
            serverSetting.Load(serverXmlElement);
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\SystemConfig(Monitor).xml", PathSettings.Instance().Config);
            
            XmlDocument xmlDocument = new XmlDocument();
            
            XmlElement configXmlElement = xmlDocument.CreateElement("", "Config", "");
            xmlDocument.AppendChild(configXmlElement);
            
            XmlHelper.SetValue(configXmlElement, "VncPath", vncPath);

            foreach (InspectorInfo inspectorInfo in inspectorInfoList)
            {
                XmlElement inspectorInfoElement = xmlDocument.CreateElement("", "InspectorInfo", "");
                configXmlElement.AppendChild(inspectorInfoElement);

                inspectorInfo.Save(inspectorInfoElement);
            }

            XmlElement serverXmlElement = xmlDocument.CreateElement("", "Server", "");
            configXmlElement.AppendChild(serverXmlElement);
            serverSetting.Save(serverXmlElement);

            XmlHelper.Save(xmlDocument, configFileName);
        }
    }
}
