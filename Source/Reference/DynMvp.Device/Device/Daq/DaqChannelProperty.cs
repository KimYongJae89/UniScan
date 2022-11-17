using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;

namespace DynMvp.Devices.Daq
{
    public class DaqChannelProperty
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private DaqChannelType daqChannelType;
        public DaqChannelType DaqChannelType
        {
            get { return daqChannelType; }
            set { daqChannelType = value; }
        }

        double samplingHz = 25000.0;
        public double SamplingHz
        {
            get { return samplingHz; }
            set { samplingHz = value; }
        }

        double minValue = -10;
        public double MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }

        double maxValue = 10;
        public double MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        double scaleFactor = 1;
        public double ScaleFactor
        {
            get { return scaleFactor; }
            set { scaleFactor = value; }
        }

        double valueOffset = 0;
        public double ValueOffset
        {
            get { return valueOffset; }
            set { valueOffset = value; }
        }

        string channelName = "dev1/ai0";
        public string ChannelName
        {
            get { return channelName; }
            set { channelName = value; }
        }

        string deviceName = "";
        public string DeviceName
        {
            get { return deviceName; }
            set { deviceName = value; }
        }

        int resisterValue;
        public int ResisterValue
        {
            get { return resisterValue; }
            set { resisterValue = value; }
        }

        bool useCustomScale;
        public bool UseCustomScale
        {
            get { return useCustomScale; }
            set { useCustomScale = value; }
        }

        public void Load(XmlElement xmlElement, string keyName)
        {
            XmlElement daqPropertyElement = xmlElement[keyName];
            if (daqPropertyElement == null)
                return;

            LoadXml(daqPropertyElement);
        }

        public virtual void LoadXml(XmlElement daqPropertyElement)
        {
            name = XmlHelper.GetValue(daqPropertyElement, "Name", "");
            daqChannelType = (DaqChannelType)Enum.Parse(typeof(DaqChannelType), XmlHelper.GetValue(daqPropertyElement, "DaqChannelType", "Nidaq"));
            samplingHz = Convert.ToDouble(XmlHelper.GetValue(daqPropertyElement, "SamplingHz", "1000"));
            minValue = Convert.ToDouble(XmlHelper.GetValue(daqPropertyElement, "MinValue", "-10"));
            maxValue = Convert.ToDouble(XmlHelper.GetValue(daqPropertyElement, "MaxValue", "10"));
            channelName = XmlHelper.GetValue(daqPropertyElement, "ChannelName", "");
            deviceName = XmlHelper.GetValue(daqPropertyElement, "DeviceName", "");
            scaleFactor = Convert.ToDouble(XmlHelper.GetValue(daqPropertyElement, "ScaleFactor", "1"));
            valueOffset = Convert.ToDouble(XmlHelper.GetValue(daqPropertyElement, "ValueOffset", "0"));
            resisterValue = Convert.ToInt32(XmlHelper.GetValue(daqPropertyElement, "ResistanceValue", "0"));
            useCustomScale = Convert.ToBoolean(XmlHelper.GetValue(daqPropertyElement, "UseCustomScale", "false"));
        }

        public void Save(XmlElement xmlElement, string keyName)
        {
            XmlElement daqPropertyElement = xmlElement.OwnerDocument.CreateElement("", keyName, "");
            xmlElement.AppendChild(daqPropertyElement);

            SaveXml(daqPropertyElement);
        }

        public virtual void SaveXml(XmlElement daqPropertyElement)
        {
            XmlHelper.SetValue(daqPropertyElement, "Name", name);
            XmlHelper.SetValue(daqPropertyElement, "DaqChannelType", daqChannelType.ToString());
            XmlHelper.SetValue(daqPropertyElement, "SamplingHz", samplingHz.ToString());
            XmlHelper.SetValue(daqPropertyElement, "MinValue", minValue.ToString());
            XmlHelper.SetValue(daqPropertyElement, "MaxValue", maxValue.ToString());
            XmlHelper.SetValue(daqPropertyElement, "ChannelName", channelName);
            XmlHelper.SetValue(daqPropertyElement, "DeviceName", deviceName.ToString());
            XmlHelper.SetValue(daqPropertyElement, "ScaleFactor", scaleFactor.ToString());
            XmlHelper.SetValue(daqPropertyElement, "ValueOffset", valueOffset.ToString());
            XmlHelper.SetValue(daqPropertyElement, "ResistanceValue", resisterValue.ToString());
            XmlHelper.SetValue(daqPropertyElement, "UseCustomScale", useCustomScale.ToString());
        }

        public DaqChannelProperty Clone()
        {
            DaqChannelProperty daqChannelProperty = new DaqChannelProperty();
            daqChannelProperty.Copy(this);

            return daqChannelProperty;
        }

        public void Copy(DaqChannelProperty srcDaqChannelProperty)
        {
            name = srcDaqChannelProperty.name;
            daqChannelType = srcDaqChannelProperty.daqChannelType;
            samplingHz = srcDaqChannelProperty.samplingHz;
            minValue = srcDaqChannelProperty.minValue;
            maxValue = srcDaqChannelProperty.maxValue;
            channelName = srcDaqChannelProperty.channelName;
            deviceName = srcDaqChannelProperty.deviceName;
            scaleFactor = srcDaqChannelProperty.scaleFactor;
            valueOffset = srcDaqChannelProperty.valueOffset;
            resisterValue = srcDaqChannelProperty.resisterValue;
            useCustomScale = srcDaqChannelProperty.useCustomScale;
        }
    }

    public class DaqChannelPropertyFactory
    {
        public static DaqChannelProperty Create(DaqChannelType daqChannelType)
        {
            switch(daqChannelType)
            {
                case DaqChannelType.Daqmx:
                    return new DaqChannelProperty();
                case DaqChannelType.MeDAQ:
                    return new DaqChannelMedaqProperty();
                case DaqChannelType.USB:
                    return new DaqChannelProperty();
            }
            
            return null;
        }
    }

    public class DaqChannelPropertyList : List<DaqChannelProperty>
    {
        public DaqChannelPropertyList Clone()
        {
            DaqChannelPropertyList newLightCtrlInfoList = new DaqChannelPropertyList();

            foreach (DaqChannelProperty daqChannelProperty in this)
            {
                newLightCtrlInfoList.Add(daqChannelProperty.Clone());
            }

            return newLightCtrlInfoList;
        }
    }
}
