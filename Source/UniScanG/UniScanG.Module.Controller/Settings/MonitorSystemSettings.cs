using DynMvp.Base;
using DynMvp.Devices.Comm;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;

namespace UniScanG.Module.Controller.Settings.Monitor
{
    public enum LaserMode { None, Use, Virtual }
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

        public List<InspectorInfo> InspectorInfoList => this.inspectorInfoList;
        List<InspectorInfo> inspectorInfoList = new List<InspectorInfo>();

        string vncPath;
        public string VncPath
        {
            get { return vncPath; }
            set { vncPath = value; }
        }

        

        public bool UseTestbedStage { get => useTestbedStage; set => useTestbedStage = value; }
        bool useTestbedStage = false;

        public LaserMode UseLaserBurner { get => this.useLaserBurner; set => this.useLaserBurner = value; }
        LaserMode useLaserBurner = LaserMode.None;

        public bool UseStickerSensor { get => this.useStickerSensor; set => this.useStickerSensor = value; }
        bool useStickerSensor = false;

        public bool EnableImsPowControl { get => this.enableImsPowControl; set => this.enableImsPowControl = value; }
        bool enableImsPowControl = false;

        public MonitorSystemSettings()
        {
            ExchangeProtocolList exchangeProtocolList = new ExchangeProtocolList(typeof(ExchangeCommand));
            exchangeProtocolList.Initialize(MachineIfType.TcpServer);

            this.serverSetting = new TcpIpMachineIfSetting(MachineIfType.TcpServer);
            //serverSetting.TcpIpInfo = new TcpIpInfo(AddressManager.Instance().GetMonitorAddress(), 6000);
            this.serverSetting.MachineIfProtocolList = exchangeProtocolList;

            this.vncPath = @"C:\Program Files\RealVNC\VNC4\vncviewer.exe";
        }

        public void Load()
        {
            string configFileName = String.Format(@"{0}\SystemConfig(Monitor).xml", PathSettings.Instance().Config);
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;
            
            XmlElement configXmlElement = xmlDocument["Config"];

            this.vncPath = XmlHelper.GetValue(configXmlElement, "VncPath", this.vncPath);
            this.useTestbedStage = XmlHelper.GetValue(configXmlElement, "UseTestbedStage", this.useTestbedStage);

            this.useLaserBurner = XmlHelper.GetValue(configXmlElement, "UseLaserBurner", this.useLaserBurner);
            this.useStickerSensor = XmlHelper.GetValue(configXmlElement, "UseStickerSensor", this.useStickerSensor);
            this.enableImsPowControl = XmlHelper.GetValue(configXmlElement, "EnableImsPowControl", this.enableImsPowControl);
            
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
            
            XmlHelper.SetValue(configXmlElement, "VncPath", this.vncPath);
            XmlHelper.SetValue(configXmlElement, "UseTestbedStage", this.useTestbedStage.ToString());

            XmlHelper.SetValue(configXmlElement, "UseLaserBurner", this.useLaserBurner.ToString());
            XmlHelper.SetValue(configXmlElement, "UseStickerSensor", this.useStickerSensor.ToString());
            XmlHelper.SetValue(configXmlElement, "EnableImsPowControl", this.enableImsPowControl);

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
