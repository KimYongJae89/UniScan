using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanWPF.Screen.PinHoleColor.Color.Settings
{
    public class ColorSettings
    {
        protected static ColorSettings instance = null;
        protected string fileName = String.Format(@"{0}\ColorSettings.xml", PathSettings.Instance().Config);

        int column;
        int row;
        int bufferNum;
        int ngCount;
        int signalTime;

        private float minWidthRatio;
        private float minHeightRatio;

        public int NgCount { get => ngCount; set => ngCount = value; }
        public int SignalTime { get => signalTime; set => signalTime = value; }

        public int Column { get => column; set => column = value; }
        public int Row { get => row; set => row = value; }

        public int BufferNum { get => bufferNum; set => bufferNum = value; }
        public float MinHeightRatio { get => minHeightRatio; set => minHeightRatio = value; }
        public float MinWidthRatio { get => minWidthRatio; set => minWidthRatio = value; }
        
        public static ColorSettings Instance()
        {
            if (instance == null)
                instance = new ColorSettings();

            return (ColorSettings)instance;
        }

        public void Load()
        {
            bool ok = false;
            try
            {
                XmlDocument xmlDocument = XmlHelper.Load(fileName);
                if (xmlDocument == null)
                    return;

                XmlElement operationElement = xmlDocument["Additional"];
                if (operationElement == null)
                    return;

                this.Load(operationElement);
            }
            finally
            {
                if (ok == false)
                    Save();
            }
        }

        public void Save()
        {
            if (Directory.Exists(PathSettings.Instance().Config) == false)
                Directory.CreateDirectory(PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();
            XmlElement operationElement = xmlDocument.CreateElement("Additional");
            xmlDocument.AppendChild(operationElement);

            this.Save(operationElement);

            xmlDocument.Save(fileName);
        }

        public void Load(XmlElement xmlElement)
        {
            column = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "Column", "20"));
            row = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "Row", "20"));
            bufferNum = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "BufferNum", "5"));
            minWidthRatio = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "MinWidthRatio", "0"));
            minHeightRatio = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "MinHeightRatio", "0"));
            ngCount = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "NgCount", "3"));
            signalTime = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "SignalTime", "100"));
        }
        
        public void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Column", column.ToString());
            XmlHelper.SetValue(xmlElement, "Row", row.ToString());
            XmlHelper.SetValue(xmlElement, "BufferNum", bufferNum.ToString());
            XmlHelper.SetValue(xmlElement, "MinWidthRatio", minWidthRatio.ToString());
            XmlHelper.SetValue(xmlElement, "MinHeightRatio", minHeightRatio.ToString());
            XmlHelper.SetValue(xmlElement, "NgCount", ngCount.ToString());
            XmlHelper.SetValue(xmlElement, "SignalTime", signalTime.ToString());
        }
    }
}
