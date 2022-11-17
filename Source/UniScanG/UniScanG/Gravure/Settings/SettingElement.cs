using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanG.Gravure.Settings
{
    public abstract class SettingElement
    {
        [LocalizedDisplayNameAttributeUniScanG("Use"), LocalizedDescriptionAttributeUniScanG("Use Alaram")]
        public bool Use { get => use; set => use = value; }
        protected bool use;

        public SettingElement(bool use)
        {
            this.use = use;
        }

        public virtual void Save(XmlElement xmlElement)
        {
            if (xmlElement == null)
                throw new NullReferenceException("SettingElement::Load - XmlElement is null");

            XmlHelper.SetValue(xmlElement, "Use", use.ToString());
        }

        public void Save(XmlElement xmlElement, string subElementName)
        {
            if (!string.IsNullOrEmpty(subElementName))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(subElementName);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            Save(xmlElement);
        }

        public bool TrySave(XmlElement xmlElement, string subElementName)
        {
            try
            {
                Save(xmlElement, subElementName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual void Load(XmlElement xmlElement)
        {
            if (xmlElement == null)
                throw new NullReferenceException("SettingElement::Load - XmlElement is null");

            this.use = XmlHelper.GetValue(xmlElement, "Use", use);
        }

        public bool TryLoad(XmlElement xmlElement, string subElementName)
        {
            try
            {
                Load(xmlElement, subElementName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Load(XmlElement xmlElement, string subElementName)
        {
            if (!string.IsNullOrEmpty(subElementName))
            {
                XmlElement subElement = xmlElement[subElementName];
                Load(subElement);
                return;
            }

            Load(xmlElement);
        }
    }

    public abstract class SettingElementCollection
    {
        protected Dictionary<object, SettingElement> dic;

        public SettingElementCollection()
        {
            this.dic = new Dictionary<object, SettingElement>();
        }

        public void Save(XmlElement xmlElement)
        {
            foreach (KeyValuePair<object, SettingElement> pair in dic)
                pair.Value.Save(xmlElement, pair.Key.ToString());
        }

        public void Save(XmlElement xmlElement, string subElementName)
        {
            if (!string.IsNullOrEmpty(subElementName))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(subElementName);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            Save(xmlElement);
        }

        public bool TrySave(XmlElement xmlElement, string subElementName)
        {
            try
            {
                Save(xmlElement, subElementName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Load(XmlElement xmlElement)
        {
            foreach (KeyValuePair<object, SettingElement> pair in dic)
                pair.Value.Load(xmlElement, pair.Key.ToString());
        }

        public void Load(XmlElement xmlElement, string subElementName)
        {
            if (!string.IsNullOrEmpty(subElementName))
            {
                XmlElement subElement = xmlElement[subElementName];
                Load(subElement);
                return;
            }

            Load(xmlElement);
        }

        public bool TryLoad(XmlElement xmlElement, string subElementName)
        {
            try
            {
                Load(xmlElement, subElementName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override string ToString()
        {
            return "...";
        }
    }

}
