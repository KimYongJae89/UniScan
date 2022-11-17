using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanG.Gravure.Settings
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public  class DebugSaveRawImageSettings : SettingElement
    {
        public int Period { get; set; } = -1;
        public string CM_FileShareId { get; set; }
        public string CM_FileSharePW { get; set; }

        public DebugSaveRawImageSettings() : base(true) { }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlHelper.SetValue(xmlElement, "Period", this.Period);
            XmlHelper.SetValue(xmlElement, "CM_FileShareId", this.CM_FileShareId);
            XmlHelper.SetValue(xmlElement, "CM_FileSharePW", this.CM_FileSharePW);
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            this.Period = XmlHelper.GetValue(xmlElement, "Period", this.Period);
            this.CM_FileShareId = XmlHelper.GetValue(xmlElement, "CM_FileShareId", this.CM_FileShareId);
            this.CM_FileSharePW = XmlHelper.GetValue(xmlElement, "CM_FileSharePW", this.CM_FileSharePW);
        }
    }
}
