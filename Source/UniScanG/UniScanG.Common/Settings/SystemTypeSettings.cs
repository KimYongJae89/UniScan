using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml;
using UniEye.Base.Settings;
using UniScanG.Common.Data;

namespace UniScanG.Common.Settings
{
    public class DriveInfoComparer : EqualityComparer<DriveInfo>
    {
        public override bool Equals(DriveInfo x, DriveInfo y)
        {
            return x.RootDirectory.Name == y.RootDirectory.Name;
        }

        public override int GetHashCode(DriveInfo obj)
        {
            return base.GetHashCode();
        }
    }

    public class SystemTypeSettings
    {
        static SystemTypeSettings _instance;
        public static SystemTypeSettings Instance()
        {
            if (_instance == null)
                _instance = new SystemTypeSettings();

            return _instance;
        }
        
        float resizeRatio = 0.1f;
        public float ResizeRatio
        {
            get { return resizeRatio; }
            set { resizeRatio = value; }
        }

        public bool ShowSimpleReportLotList { get => this.showSimpleReportLotList; set => this.showSimpleReportLotList = value; }
        bool showSimpleReportLotList;

        public bool LocalExchangeMode { get => this.localExchangeMode; set => this.localExchangeMode = value; }
        bool localExchangeMode;
        
        public RectangleF MonitorFov { get => this.monitorFov; set => this.monitorFov = value; }
        RectangleF monitorFov = new RectangleF();

        public SystemTypeSettings()
        {
            //foreach (DriveInfo info in DriveInfo.GetDrives())
            //    driveInfoList.Add(info, false);
        }

        public void Load()
        {
            string configFileName = String.Format(@"{0}\SystemTypeConfig.xml", PathSettings.Instance().Config);
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;

            XmlElement configXmlElement = xmlDocument["Config"];

            this.resizeRatio = XmlHelper.GetValue(configXmlElement, "ResizeRatio", this.resizeRatio);
            this.showSimpleReportLotList = XmlHelper.GetValue(configXmlElement, "ShowSimpleReportLotList", this.showSimpleReportLotList);
            this.localExchangeMode = XmlHelper.GetValue(configXmlElement, "LocalExchangeMode", this.localExchangeMode);

            this.monitorFov = XmlHelper.GetValue(configXmlElement, "MonitorFov", RectangleF.Empty);
            if (this.monitorFov.IsEmpty)
            {
                float fovX = Convert.ToSingle(XmlHelper.GetValue(configXmlElement, "FovX", "0"));
                float fovY = Convert.ToSingle(XmlHelper.GetValue(configXmlElement, "FovY", "0"));
                float fovW = Convert.ToSingle(XmlHelper.GetValue(configXmlElement, "FovWidth", "0"));
                float fovH = Convert.ToSingle(XmlHelper.GetValue(configXmlElement, "FovHeight", "0"));
                this.monitorFov = new RectangleF(fovX, fovY, fovW, fovH);
            }
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\SystemTypeConfig.xml", PathSettings.Instance().Config);
            
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement configXmlElement = xmlDocument.CreateElement("", "Config", "");
            xmlDocument.AppendChild(configXmlElement);

            XmlHelper.SetValue(configXmlElement, "ResizeRatio", this.resizeRatio);
            XmlHelper.SetValue(configXmlElement, "ShowSimpleReportLotList", this.showSimpleReportLotList);
            XmlHelper.SetValue(configXmlElement, "LocalExchangeMode", this.localExchangeMode);

            XmlHelper.SetValue(configXmlElement, "MonitorFov", this.monitorFov);

            //XmlHelper.SetValue(configXmlElement, "FovX", monitorFov.X.ToString());
            //XmlHelper.SetValue(configXmlElement, "FovY", monitorFov.Y.ToString());
            //XmlHelper.SetValue(configXmlElement, "FovWidth", monitorFov.Width.ToString());
            //XmlHelper.SetValue(configXmlElement, "FovHeight", monitorFov.Height.ToString());

            XmlHelper.Save(xmlDocument, configFileName);
        }
    }
}
