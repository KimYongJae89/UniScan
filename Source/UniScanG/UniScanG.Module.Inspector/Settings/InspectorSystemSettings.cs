using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;
using UniScanG.Common.Settings;

namespace UniScanG.Module.Inspector.Settings.Inspector
{
    public class InspectorSystemSettings
    {
        public TcpIpMachineIfSetting ClientSetting { get => this.clientSetting; }
        TcpIpMachineIfSetting clientSetting = new TcpIpMachineIfSetting(MachineIfType.TcpClient);

        static InspectorSystemSettings _instance;
        public static InspectorSystemSettings Instance()
        {
            if (_instance == null)
                _instance = new InspectorSystemSettings();

            return _instance;
        }

        public int CamIndex { get => this.camIndex; set => this.camIndex = value; }
        int camIndex;

        public int ClientIndex { get => this.clientIndex; set => this.clientIndex = value; }
        int clientIndex;
        
        public bool SplitExactPattern { get; set; } = false;

        public InspectorSystemSettings()
        {
            ExchangeProtocolList exchangeProtocolList = new ExchangeProtocolList(typeof(ExchangeCommand));
            exchangeProtocolList.Initialize(MachineIfType.TcpClient);

            clientSetting.MachineIfProtocolList = exchangeProtocolList;
        }

        public void Load()
        {
            string configFileName = String.Format(@"{0}\SystemConfig.xml", PathSettings.Instance().Config);
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;
            
            XmlElement configXmlDocument = xmlDocument["Config"];

            XmlElement clientXmlElement = configXmlDocument["Client"];
            clientSetting.Load(clientXmlElement);

            this.camIndex = XmlHelper.GetValue(configXmlDocument, "CamIndex", this.camIndex);
            this.clientIndex = XmlHelper.GetValue(configXmlDocument, "ClientIndex", this.clientIndex);
            this.SplitExactPattern = XmlHelper.GetValue(configXmlDocument, "SplitExactPattern", this.SplitExactPattern);
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\SystemConfig.xml", PathSettings.Instance().Config);
            
            XmlDocument xmlDocument = new XmlDocument();
            
            XmlElement configXmlDocument = xmlDocument.CreateElement("", "Config", "");
            xmlDocument.AppendChild(configXmlDocument);

            XmlElement clientXmlElement = xmlDocument.CreateElement("", "Client", "");
            configXmlDocument.AppendChild(clientXmlElement);
            clientSetting.Save(clientXmlElement);

            XmlHelper.SetValue(configXmlDocument, "CamIndex", this.camIndex);
            XmlHelper.SetValue(configXmlDocument, "ClientIndex", this.clientIndex);
            XmlHelper.SetValue(configXmlDocument, "SplitExactPattern", this.SplitExactPattern);

            XmlHelper.Save(xmlDocument, configFileName);
        }
    }
}
