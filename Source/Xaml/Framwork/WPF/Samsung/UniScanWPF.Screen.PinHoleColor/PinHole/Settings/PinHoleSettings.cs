using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanWPF.Screen.PinHoleColor.PinHole.Settings
{
    public class PinHoleSettings
    {
        protected static PinHoleSettings instance = null;
        protected string fileName = String.Format(@"{0}\PinHoleSettings.xml", PathSettings.Instance().Config);

        int maxDefectNum;
        float pixelResolution;
        float minSize;
        int clipSize;
        int bufferNum;
        int signalTime;

        public float MinSize { get => minSize; set => minSize = value; }
        public int SignalTime { get => signalTime; set => signalTime = value; }
        public int MaxDefectNum { get => maxDefectNum; set => maxDefectNum = value; }
        public float PixelResolution { get => pixelResolution; set => pixelResolution = value; }

        public int ClipSize { get => clipSize; set => clipSize = value; }
        public int BufferNum { get => bufferNum; set => bufferNum = value; }

        public static PinHoleSettings Instance()
        {
            if (instance == null)
                instance = new PinHoleSettings();

            return instance;
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
            maxDefectNum = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "MaxDefectNum", "100"));
            pixelResolution = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "PixelResolution", "10"));
            minSize = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "MinSize", "1000"));
            clipSize = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "ClipSize", "25"));
            bufferNum = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "BufferNum", "5"));
            signalTime = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "SignalTime", "100"));
        }
        
        public void Save(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "MaxDefectNum", maxDefectNum.ToString());
            XmlHelper.SetValue(xmlElement, "PixelResolution", pixelResolution.ToString());
            XmlHelper.SetValue(xmlElement, "MinSize", minSize.ToString());
            XmlHelper.SetValue(xmlElement, "ClipSize", clipSize.ToString());
            XmlHelper.SetValue(xmlElement, "BufferNum", bufferNum.ToString());
            XmlHelper.SetValue(xmlElement, "SignalTime", signalTime.ToString());
        }
    }
}
