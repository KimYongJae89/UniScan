using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniEye.Base.Settings
{
    public delegate void AdditionalSettingChangedDelegate();

    public class AdditionalSettings
    {
        public event AdditionalSettingChangedDelegate AdditionalSettingChangedDelegate = null;

        protected static AdditionalSettings instance = null;
        //protected string fileName = String.Format(@"{0}\AdditionalSettings.xml", PathSettings.Instance().Config);
        public static string FileName = @"AdditionalSettings.xml";

        protected AdditionalSettings() { }

        public static AdditionalSettings Instance()
        {
            return instance;
        }

        public static void CreateInstance()
        {
            if (instance == null)
                instance = new AdditionalSettings();
        }

        public void OnChanged()
        {
            AdditionalSettingChangedDelegate?.Invoke();
        }

        public void Load()
        {
            //bool ok = false;
            try
            {
                string file = Path.Combine(PathSettings.Instance().Config, FileName);
                if (!File.Exists(file))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.AppendChild(xmlDoc.CreateElement("Additional"));
                    xmlDoc.Save(file);
                }

                XmlDocument xmlDocument = XmlHelper.Load(file);
                if (xmlDocument == null)
                    return;

                XmlElement operationElement = xmlDocument["Additional"];
                if (operationElement == null)
                    return;

                this.Load(operationElement);
            }
            finally
            {
                //if (ok == false)
                //    Save();

                this.AdditionalSettingChangedDelegate?.Invoke();
            }
        }

        public virtual void Load(XmlElement xmlElement) { }

        public void Save()
        {
            if (Directory.Exists(PathSettings.Instance().Config) == false)
                Directory.CreateDirectory(PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();
            XmlElement operationElement = xmlDocument.CreateElement("Additional");
            xmlDocument.AppendChild(operationElement);

            this.Save(operationElement);

            xmlDocument.Save(Path.Combine(PathSettings.Instance().Config, FileName));

            PostSave();
        }

        public virtual void Save(XmlElement xmlElement) { }


        protected virtual void PostSave()
        {
            this.Load();
        }
    }
}
