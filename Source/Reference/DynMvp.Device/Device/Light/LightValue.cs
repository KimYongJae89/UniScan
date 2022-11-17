using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;

namespace DynMvp.Devices.Light
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class LightValue
    {
        private int[] value;
        public int[] Value
        {
            get { return value; }
        }

        public LightValue(int numLight, int defaultValue = 0)
        {
            value = new int[numLight];
            for (int i = 0; i < numLight; i++)
            {
                value[i] = defaultValue;
            }
        }

        public LightValue(params int[] values)
        {
            value = new int[values.Length];
            Array.Copy(values, value, values.Length);
        }

        public LightValue Clone()
        {
            LightValue lightValue = new LightValue(value.Count());
            lightValue.Copy(this);

            return lightValue;
        }

        internal void Clip(int value) //What is means?
        {
            for (int i = 0; i < this.value.Length; i++)
            {
                this.value[i] = Math.Min(this.value[i], value);
            }
        }

        public void Copy(LightValue lightValue)
        {
            value = new int[lightValue.NumLight];
            for (int i = 0; i < lightValue.NumLight; i++)
            {
                value[i] = lightValue.Value[i];
            }
        }

        public void Save(XmlElement xmlElement, string key = null)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            for (int i = 0; i < this.NumLight; i++)
            {
                XmlHelper.SetValue(xmlElement, String.Format("LightOnValue{0}", i), this.Value[i].ToString());
            }
        }

        public void Load(XmlElement xmlElement, string key = null)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement[key];
                if (subElement != null)
                    Load(subElement);
                return;
            }

            this.NumLight = XmlHelper.GetValue(xmlElement, "NumLight", 0);
            if (this.NumLight == 0)
            {
                List<int> lightList = new List<int>();
                while (true)
                {
                    int value = XmlHelper.GetValue(xmlElement, String.Format("LightOnValue{0}", lightList.Count), -1);
                    if (value < 0)
                        break;

                    lightList.Add(value);
                }
                this.value = lightList.ToArray();
            }
            else
            {
                for (int i = 0; i < this.NumLight; i++)
                {
                    this.Value[i] = XmlHelper.GetValue(xmlElement, String.Format("LightOnValue{0}", i), 0);
                }
            }
        }

        public int NumLight
        {
            get { return value.Count(); }
            set { Array.Resize(ref this.value, value); }
        }

        public string KeyValue
        {
            get
            {
                return string.Concat(Array.ConvertAll(value, f => f.ToString("X2")));
                //string keyValue = "";

                //for (int i = 0; i < value.Count(); i++)
                //{
                //    keyValue += value[i].ToString("x2");
                //}

                //return keyValue;
            }
        }
    }

    public class LightParamList : CollectionBase
    {
        public LightParam this[int i] => (LightParam)this.InnerList[i];

        public void AddLightValue(LightParam lightParam)
        {
            if (IsContained(lightParam) == false)
                this.InnerList.Add(lightParam);
        }

        public bool IsContained(LightParam lightParam)
        {
            return this.InnerList.Contains(lightParam);
        }

        public void Save(XmlElement xmlElement, string key = null)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            foreach (LightParam lightParam in this.InnerList)
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement("LightParam");
                xmlElement.AppendChild(subElement);
                lightParam.Save(subElement);
            }
        }

        public bool Load(XmlElement xmlElement, string key = null)
        {
            this.InnerList.Clear();
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement[key];
                if (subElement == null)
                    return false;

                return Load(subElement);
            }

            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("LightParam");
            foreach (XmlElement subElement in xmlNodeList)
            {
                LightParam lightParam = new LightParam(0);
                lightParam.Load(subElement);
                this.InnerList.Add(lightParam);
            }

            return true;
        }

        public LightParam[] ToArray()
        {
            return (LightParam[])this.InnerList.ToArray(typeof(LightParam));
        }
    }
}
