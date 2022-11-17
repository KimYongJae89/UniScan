using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DynMvp.Base
{
    public class ValueTable
    {
        Dictionary<string, string> values = new Dictionary<string, string>();

        public string GetValue(string key)
        {
            try
            {
                return values[key];
            }
            catch (KeyNotFoundException)
            {
            }

            return "";
        }

        public int GetValue(string key, int defaultValue)
        {
            string tempStr = GetValue(key);
            if (tempStr != "")
            {
                return Convert.ToInt32(tempStr);
            }

            return defaultValue;
        }

        public void SetValue(string key, string value)
        {
            values[key] = value;
        }

        public void Load(XmlElement configElement)
        {
            XmlElement defaultValuesElement = configElement["DefaultValue"];
            if (defaultValuesElement == null)
                return;

            foreach (XmlElement valueElement in defaultValuesElement)
            {
                values[valueElement.Name] = valueElement.InnerText;
            }
        }

        public void Save(XmlElement configElement)
        {
            XmlElement defaultValuesElement = configElement.OwnerDocument.CreateElement("", "DefaultValue", "");
            configElement.AppendChild(defaultValuesElement);

            foreach (KeyValuePair<string, string> pair in values)
            {
                XmlHelper.SetValue(defaultValuesElement, pair.Key, pair.Value);
            }
        }
    }
}
