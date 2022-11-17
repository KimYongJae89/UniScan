using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;
using DynMvp.Devices.Light;

namespace DynMvp.Devices
{
    public enum LightParamType
    {
        Value, Composite
    }

    public class LightParam : ICloneable
    {
        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        LightParamType lightParamType = LightParamType.Value;
        public LightParamType LightParamType
        {
            get { return lightParamType; }
            set { lightParamType = value; }
        }

        private int lightStableTimeMs = 0;
        public int LightStableTimeMs
        {
            get { return lightStableTimeMs; }
            set { lightStableTimeMs = value; }
        }

        private int exposureTimeUs = 50;
        public int ExposureTimeUs
        {
            get { return exposureTimeUs; }
            set { exposureTimeUs = value; }
        }

        private int exposureTime3dUs = 50;
        public int ExposureTime3dUs
        {
            get { return exposureTime3dUs; }
            set { exposureTime3dUs = value; }
        }

        private LightValue lightValue;
        public LightValue LightValue
        {
            get { return lightValue; }
            set { lightValue = value; }
        }

        ImageOperationType operationType;
        public ImageOperationType OperationType
        {
            get { return operationType; }
            set { operationType = value; }
        }

        private int firstImageIndex = 0;
        public int FirstImageIndex
        {
            get { return firstImageIndex; }
            set { firstImageIndex = value; }
        }

        private int secondImageIndex = 0;
        public int SecondImageIndex
        {
            get { return secondImageIndex; }
            set { secondImageIndex = value; }
        }

        public string KeyValue
        {
            get
            {
                string keyValue = "";
                if (lightValue != null)
                {
                    if (this.LightParamType == LightParamType.Value)
                    {
                        keyValue = string.Format("V{0}", lightValue.KeyValue);
                    }
                    else
                    {
                        keyValue = string.Format("C{0}", lightValue.KeyValue);
                    }
                }
                return keyValue;
            }
        }

        public LightParam()
        {
            lightValue = new LightValue(0);
        }

        public LightParam(int numLight)
        {
            lightValue = new LightValue(numLight);
        }

        public LightParam(int numLight, int defaultValue)
        {
            lightValue = new LightValue(numLight, defaultValue);
        }

        public LightParam(LightValue lightValue)
        {
            this.lightValue = lightValue.Clone();
        }

        public object Clone()
        {
            LightParam lightParam = new LightParam(lightValue.NumLight);
            lightParam.Copy(this);

            return lightParam;
        }

        public void Copy(LightParam lightParam)
        {
            name = lightParam.name;

            lightParamType = lightParam.lightParamType;
            lightStableTimeMs = lightParam.lightStableTimeMs;
            exposureTimeUs = lightParam.exposureTimeUs;
            exposureTime3dUs = lightParam.exposureTime3dUs;

            lightValue = new LightValue(lightParam.LightValue.NumLight);
            lightValue.Copy(lightParam.LightValue);

            operationType = lightParam.operationType;
            firstImageIndex = lightParam.firstImageIndex;
            secondImageIndex = lightParam.secondImageIndex;
        }

        public void Save(XmlElement lightParamElement, string keyName)
        {
            XmlElement subElement = lightParamElement.OwnerDocument.CreateElement(keyName);
            lightParamElement.AppendChild(subElement);

            Save(subElement);
        }

        public void Save(XmlElement lightParamElement)
        {
            XmlHelper.SetValue(lightParamElement, "Name", name.ToString());

            XmlHelper.SetValue(lightParamElement, "LightParamType", lightParamType.ToString());
            XmlHelper.SetValue(lightParamElement, "LightStableTimeMs", lightStableTimeMs.ToString());
            XmlHelper.SetValue(lightParamElement, "ExposureTimeUs", exposureTimeUs.ToString());
            XmlHelper.SetValue(lightParamElement, "ExposureTime3dUs", exposureTime3dUs.ToString());

            lightValue.Save(lightParamElement);

            XmlHelper.SetValue(lightParamElement, "OperationType", operationType.ToString());
            XmlHelper.SetValue(lightParamElement, "FirstImageIndex", firstImageIndex.ToString());
            XmlHelper.SetValue(lightParamElement, "SecondImageIndex", secondImageIndex.ToString());
        }

        public void Load(XmlElement lightParamElement, string keyName)
        {
            XmlElement subElement = lightParamElement[keyName];
            if (subElement == null)
                return;

            Load(subElement);
        }

        public void Load(XmlElement lightParamElement)
        {
            name = XmlHelper.GetValue(lightParamElement, "Name", "");

            lightParamType = (LightParamType)Enum.Parse(typeof(LightParamType), XmlHelper.GetValue(lightParamElement, "LightParamType", "Value"));
            exposureTime3dUs = Convert.ToInt32(XmlHelper.GetValue(lightParamElement, "ExposureTime3dUs", "50"));

            string exposureTimeStr = XmlHelper.GetValue(lightParamElement, "ExposureTimeUs", "");
            if (String.IsNullOrEmpty(exposureTimeStr))
            {
                exposureTimeStr = XmlHelper.GetValue(lightParamElement, "ExposureTime", "50");
                exposureTimeUs = Convert.ToInt32(exposureTimeStr) * 1000;
            }
            else
            {
                exposureTimeUs = Convert.ToInt32(exposureTimeStr);
            }

            lightStableTimeMs = Convert.ToInt32(XmlHelper.GetValue(lightParamElement, "LightStableTimeMs", "20"));

            lightValue.Load(lightParamElement);

            operationType = (ImageOperationType)Enum.Parse(typeof(ImageOperationType), XmlHelper.GetValue(lightParamElement, "OperationType", "Subtract"));
            firstImageIndex = Convert.ToInt32(XmlHelper.GetValue(lightParamElement, "FirstImageIndex", "0"));
            secondImageIndex = Convert.ToInt32(XmlHelper.GetValue(lightParamElement, "SecondImageIndex", "1"));
        }

        public override string ToString()
        {
            return this.name;
        }
    }

    public class LightParamSet
    {
        private List<LightParam> lightParamList = new List<LightParam>();
        public List<LightParam> LightParamList
        {
            get { return lightParamList; }
            set { lightParamList = value; }
        }

        private LightParam lightParam3d = new LightParam(1);
        public LightParam LightParam3d
        {
            get { return lightParam3d; }
            set { lightParam3d = value; }
        }

        public int NumLight
        {
            get
            {
                if (lightParam3d != null)
                    return lightParam3d.LightValue.NumLight;
                return 0;
            }
        }

        public int NumLightType
        {
            get
            {
                if (lightParamList != null)
                    return lightParamList.Count;

                return 0;
            }
        }


        public void Initialize(int numLight, int numLightType)
        {
            lightParamList = new List<LightParam>();

            for (int i = 0; i < numLightType; i++)
                lightParamList.Add(new LightParam((int)numLight));

            lightParam3d = new LightParam(numLight);
        }

        public LightParamSet Clone()
        {
            LightParamSet lightParamSet = new LightParamSet();

            foreach (LightParam lightParam in lightParamList)
                lightParamSet.LightParamList.Add((LightParam)lightParam.Clone());

            lightParamSet.LightParam3d = (LightParam)lightParam3d.Clone();

            return lightParamSet;
        }

        public void Load(XmlElement lightParamSetElement)
        {
            XmlNodeList list = lightParamSetElement.GetElementsByTagName("LightParam");
            //for (int i = 0; i < Math.Min(lightParamList.Count, list.Count); i++)
            for (int i = 0; i < list.Count; i++)
            {
                XmlElement lightParamElement = list[i] as XmlElement;
                LightParam lightParam = new LightParam(NumLight);
                lightParam.Load(lightParamElement);

                if (String.IsNullOrEmpty(lightParam.Name))
                    lightParam.Name = String.Format("LightType {0}", lightParamList.Count);

                if (lightParamList.Count <= i)
                    lightParamList.Add(null);

                lightParamList[i] = lightParam;

                //if (lightParamIndex >= lightParamList.Count)
                //    break;
            }

            XmlElement lightParam3dElement = lightParamSetElement["LightParam3d"];
            if (lightParam3dElement != null)
            {
                LightParam3d.Load(lightParam3dElement);
                if (String.IsNullOrEmpty(LightParam3d.Name))
                {
                    LightParam3d.Name = "LightType 3D";
                }
            }
        }

        public void Save(XmlElement lightParamSetElement)
        {
            foreach (LightParam lightParam in lightParamList)
            {
                XmlElement lightParamElement = lightParamSetElement.OwnerDocument.CreateElement("", "LightParam", "");
                lightParamSetElement.AppendChild(lightParamElement);

                lightParam.Save(lightParamElement);
            }

            XmlElement lightParam3dElement = lightParamSetElement.OwnerDocument.CreateElement("", "LightParam3d", "");
            LightParam3d.Save(lightParam3dElement);
        }
    }
}
