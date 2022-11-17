using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanG.Module.Monitor.Data
{
    public class TeachData
    {
        public float MarginLimitW { get; set; } = 30;
        public float MarginLimitL { get; set; } = 30;

        public float BlotLimitW { get; set; } = 20;
        public float BlotLimitL { get; set; } = 20;

        public float MinDefectLimitW { get; set; } = 10;
        public float MinDefectLimitL { get; set; } = 10;

        //public SizeF MarginLimit { get; set; }
        //public bool IsMarginP { get; set; }

        //public SizeF BlotLimit { get; set; }
        //public bool IsblotP { get; set; }

        //public SizeF DefectMinSize { get; set; }

        public TeachData()
        {
            
        }

        public TeachData Clone()
        {
            TeachData newTeachData= new TeachData()
            {
                MarginLimitW = this.MarginLimitW,
                MarginLimitL = this.MarginLimitL,
                BlotLimitW = this.BlotLimitW,
                BlotLimitL = this.BlotLimitL,
                MinDefectLimitW = this.MinDefectLimitW,
                MinDefectLimitL = this.MinDefectLimitL
            };

            return newTeachData;
        }

        internal void Save(XmlElement xmlElement, string key = null)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            XmlHelper.SetValue(xmlElement, "MarginLimitW", this.MarginLimitW.ToString());
            XmlHelper.SetValue(xmlElement, "MarginLimitL", this.MarginLimitL.ToString());

            XmlHelper.SetValue(xmlElement, "BlotLimitW", this.BlotLimitW.ToString());
            XmlHelper.SetValue(xmlElement, "BlotLimitL", this.BlotLimitL.ToString());

            XmlHelper.SetValue(xmlElement, "DefectMinSizeW", this.MinDefectLimitW.ToString());
            XmlHelper.SetValue(xmlElement, "DefectMinSizeL", this.MinDefectLimitL.ToString());
        }

        internal void Load(XmlElement xmlElement, string key = null)
        {
            if (xmlElement == null)
                return;

            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement[key];
                Load(subElement);
                return;
            }

            this.MarginLimitW = XmlHelper.GetValue(xmlElement, "MarginLimitW", 10);
            this.MarginLimitL = XmlHelper.GetValue(xmlElement, "MarginLimitL", 10);

            this.BlotLimitW = XmlHelper.GetValue(xmlElement, "BlotLimitW", 10);
            this.BlotLimitL = XmlHelper.GetValue(xmlElement, "BlotLimitL", 10);

            this.MinDefectLimitW = XmlHelper.GetValue(xmlElement, "DefectMinSizeW", 0);
            this.MinDefectLimitL = XmlHelper.GetValue(xmlElement, "DefectMinSizeL", 0);
        }
    }
}
