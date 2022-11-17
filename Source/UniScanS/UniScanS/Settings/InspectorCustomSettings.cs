using DynMvp.Base;
using System;
using System.IO;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanS.Inspector.Settings
{
    public class InspectorCustomSettings
    {
        static InspectorCustomSettings _instance;
        public static InspectorCustomSettings Instance()
        {
            if (_instance == null)
                _instance = new InspectorCustomSettings();

            return _instance;
        }
        
        float startXPosition;
        public float StartXPosition
        {
            get { return startXPosition; }
            set { startXPosition = value; }
        }

        float startYPosition;
        public float StartYPosition
        {
            get { return startYPosition; }
            set { startYPosition = value; }
        }

        float fovX = 100;
        public float FovX
        {
            get { return fovX; }
            set { fovX = value; }
        }

        float sheetHeight = 220;
        public float SheetHeight
        {
            get { return sheetHeight; }
            set { sheetHeight = value; }
        }
        
        public void Load()
        {
            string configFileName = String.Format(@"{0}\CustomConfig(Inspector).xml", PathSettings.Instance().Config);
            
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;

            XmlElement configXmlDocument = xmlDocument["Config"];
            
            startXPosition = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "StartXPosition", "0"));
            startYPosition = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "StartYPosition", "0"));
            fovX = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "FovX", "100"));
            sheetHeight = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "SheetHeight", "220"));
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\CustomConfig(Inspector).xml", PathSettings.Instance().Config);
            
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement configXmlDocument = xmlDocument.CreateElement("", "Config", "");
            xmlDocument.AppendChild(configXmlDocument);
            
            XmlHelper.SetValue(configXmlDocument, "StartXPosition", startXPosition.ToString());
            XmlHelper.SetValue(configXmlDocument, "StartYPosition", startYPosition.ToString());
            XmlHelper.SetValue(configXmlDocument, "FovX", fovX.ToString());
            XmlHelper.SetValue(configXmlDocument, "SheetHeight", sheetHeight.ToString());

            XmlHelper.Save(xmlDocument, configFileName);
        }
    }
}
