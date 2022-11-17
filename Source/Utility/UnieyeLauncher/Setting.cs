using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnieyeLauncher.Operation;

namespace UnieyeLauncher
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public abstract class SubSettings
    {
        public bool Use { get; set; }

        public SubSettings(bool use)
        {
            this.Use = use;
        }

        protected virtual void Save(XmlElement xmlElement)
        {
            XmlElement subElementUse= (XmlElement)xmlElement.AppendChild(xmlElement.OwnerDocument.CreateElement("Use"));
            subElementUse.InnerText = this.Use.ToString();
        }

        public void Save(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
                xmlElement = (XmlElement)xmlElement.AppendChild(xmlElement.OwnerDocument.CreateElement(key));
            Save(xmlElement);
        }

        protected virtual void Load(XmlElement xmlElement)
        {
            this.Use = Convert.ToBoolean(xmlElement["Use"]?.InnerText);
        }

        public void Load(XmlElement xmlElement, string key)
        {
            if (!string.IsNullOrEmpty(key))
                xmlElement = xmlElement[key];

            if (xmlElement == null)
                return;

            Load(xmlElement);
        }
    }

    [Serializable]
    class UniEyeLauncherSetting
    {
        public static string SettingFileName => "UniEyeLauncherSetting.xml";

        public static string SettingFileName2 => "UniEyeLauncherSettingV2.xml";

        public static string SettingFileNameS => "UniEyeLauncher.scfg";

        public bool Silent { get => this.silent; set => this.silent = value; }
        bool silent = true;

        SubSettings[] settings = new SubSettings[] { new LaunchSettings(), new WatchdogSettings(), new PatchSettings(), new UPSSettings(), new ArchiveSettings(), new RemoteSettings() };


        public LaunchSettings LaunchSettings { get => (LaunchSettings)this.settings[0]; }

        public WatchdogSettings WatchdogSettings { get => (WatchdogSettings)this.settings[1]; }

        public PatchSettings PatchSettings { get => (PatchSettings)this.settings[2]; }

        public UPSSettings UPSSettings { get => (UPSSettings)this.settings[3]; }

        public ArchiveSettings ArchiveSettings { get => (ArchiveSettings)this.settings[4]; }

        public RemoteSettings RemoteSettings { get => (RemoteSettings)this.settings[5]; }

        public UniEyeLauncherSetting() { }

        public void SaveSerialize(string fullPathName)
        {
            byte[] bytes = this.Serialize();
            using (FileStream fs = new FileStream(fullPathName, FileMode.Create))
                fs.Write(bytes, 0, bytes.Length);
        }

        public static UniEyeLauncherSetting LoadSerialize(string fullPathName)
        {
            byte[] bytes = null;
            using (FileStream fs = new FileStream(fullPathName, FileMode.Open))
            {
                bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
            }
            return UniEyeLauncherSetting.Deserialize(bytes);
        }

        public byte[] Serialize()
        {
            MemoryStream stream = new MemoryStream();

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, this);

            return stream.ToArray();
        }

        public static UniEyeLauncherSetting Deserialize(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            object obj = binaryFormatter.Deserialize(stream);

            return (UniEyeLauncherSetting)obj;
        }

        public void Load(string fullPathName)
        {
            if (string.IsNullOrEmpty(fullPathName))
                return;

            if (!File.Exists(fullPathName))
                return;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fullPathName);

            XmlElement xmlElement = xmlDocument["Settings"];
            if (xmlElement == null)
                return;

            XmlElement silentElement = xmlElement["Silent"];
            if(silentElement!=null)
                bool.TryParse(silentElement.InnerText, out this.silent);

            Array.ForEach(this.settings, f => f.Load(xmlElement, f.GetType().Name));
        }

        public void Save(string fullPathName)
        {
            if (string.IsNullOrEmpty(fullPathName))
                return;

            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlElement = xmlDoc.CreateElement("Settings");
            xmlDoc.AppendChild(xmlElement);

            Array.ForEach(this.settings, f => f.Save(xmlElement, f.GetType().Name));

            xmlDoc.Save(fullPathName);
        }
    }
}
