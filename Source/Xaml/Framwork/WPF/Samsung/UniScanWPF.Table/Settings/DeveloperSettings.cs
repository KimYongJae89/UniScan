using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.Settings
{
    public class DeveloperSettings : ISavableObj
    {
        static DeveloperSettings _instance;

        string fileName;

        float resolution;
        int moveOffset;
        int bufferWidth;
        int bufferHeight;
        int scanNum;

        public float Resolution { get => resolution; }
        public int MoveOffset { get => moveOffset; }
        public int BufferWidth { get => bufferWidth; }
        public int BufferHeight { get => bufferHeight; }
        public int ScanNum { get => scanNum; }

        public static DeveloperSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DeveloperSettings();
                    _instance.Load();
                }

                return _instance;
            }
        }

        private DeveloperSettings()
        {
            fileName = String.Format(@"{0}\{1}.xml", PathSettings.Instance().Config, "Developer");

            resolution = 5.0f;
            moveOffset = 0;
            bufferWidth = 12280;
            bufferHeight = 100000;

            scanNum = 9;
        }

        public void Load()
        {
            if (File.Exists(fileName) == false)
            {
                Save();
                return;
            }

            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
                return;

            XmlElement settingsElement = xmlDocument["Settings"];
            if (settingsElement == null)
                return;

            this.resolution = XmlHelper.GetValue(settingsElement, "Resolution", this.resolution);
            this.moveOffset = XmlHelper.GetValue(settingsElement, "MoveOffset", this.moveOffset);
            this.bufferWidth = XmlHelper.GetValue(settingsElement, "BufferWidth", this.bufferWidth);
            this.bufferHeight = XmlHelper.GetValue(settingsElement, "BufferHeight", this.bufferHeight);
            this.scanNum = XmlHelper.GetValue(settingsElement, "ScanNum", this.scanNum);
        }

        public void Save(string fileName = "")
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = this.fileName;

            if (Directory.Exists(fileName) == false)
                Directory.CreateDirectory(fileName);

            XmlDocument xmlDocument = new XmlDocument();
            XmlElement settingsElement = xmlDocument.CreateElement("Settings");
            xmlDocument.AppendChild(settingsElement);

            XmlHelper.SetValue(settingsElement, "Resolution", this.resolution);
            XmlHelper.SetValue(settingsElement, "MoveOffset", this.moveOffset);
            XmlHelper.SetValue(settingsElement, "BufferWidth", this.bufferWidth);
            XmlHelper.SetValue(settingsElement, "BufferHeight", this.bufferHeight);
            XmlHelper.SetValue(settingsElement, "ScanNum", this.scanNum);

            xmlDocument.Save(fileName);
        }
    }
}
