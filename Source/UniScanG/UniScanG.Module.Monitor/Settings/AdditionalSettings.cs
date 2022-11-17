using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Common.Data;
using UniScanG.Gravure.Settings;
using WpfControlLibrary.Helper;

namespace UniScanG.Module.Monitor.Settings
{

    public class LocalizedCategoryAttribute : CategoryAttribute
    {
        string key;
        public LocalizedCategoryAttribute(string key, string value) : base(value)
        {
            this.key = key;
        }

        protected override string GetLocalizedString(string value)
        {
            return LocalizeHelper.GetString(key, value);
        }
    }

    public class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        string key;
        public LocalizedDisplayNameAttribute(string key, string value)
        {
            this.key = key;
            base.DisplayNameValue = LocalizeHelper.GetString(key, value);
        }
    }

    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        string key;
        public LocalizedDescriptionAttribute(string key, string value)
        {
            this.key = key;
            base.DescriptionValue = LocalizeHelper.GetString(key, value);
        }
    }


    class AdditionalSettings : UniEye.Base.Settings.AdditionalSettings
    {
        [LocalizedCategoryAttribute("LocalizedCategoryAttributeUniScanG", "Teach"), LocalizedDisplayNameAttribute("LocalizedDisplayNameAttributeUniScanG", "Teach Interval"), LocalizedDescriptionAttribute("LocalizedDescriptionAttributeUniScanG", "Teach Interval")]
        public int TeachInterval { get; set; } = 2;

        [LocalizedCategoryAttribute("LocalizedCategoryAttributeUniScanG", "Update"), LocalizedDisplayNameAttribute("LocalizedDisplayNameAttributeUniScanG", "Refresh Time [ms]"), LocalizedDescriptionAttribute("LocalizedDescriptionAttributeUniScanG", "Refresh Time [ms]")]
        public int RefreshTimeMs { get; set; } = -1;
        
        public static new void CreateInstance()
        {
            if (instance == null)
                instance = new AdditionalSettings();
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlElement teachElement = (XmlElement)xmlElement.AppendChild(xmlElement.OwnerDocument.CreateElement("Teach"));
            XmlHelper.SetValue(teachElement, "TeachInterval", this.TeachInterval.ToString());

            XmlElement updateElement = (XmlElement)xmlElement.AppendChild(xmlElement.OwnerDocument.CreateElement("Update"));
            XmlHelper.SetValue(updateElement, "RefreshTimeMs", this.RefreshTimeMs.ToString());
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            XmlElement teachElement = xmlElement["Teach"];
            if(teachElement!=null)
            {
                this.TeachInterval = XmlHelper.GetValue(teachElement, "TeachInterval", this.TeachInterval);
            }

            XmlElement updateElement = xmlElement["Update"];
            if (teachElement != null)
            {
                this.RefreshTimeMs = XmlHelper.GetValue(updateElement, "RefreshTimeMs", this.RefreshTimeMs);
            }
        }
    }
}
