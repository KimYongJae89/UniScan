using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DynMvp.Device.Device.UPS
{
    public enum UpsType { None, ACPI, Remote }

    public class SystemPowerStatus
    {
        public bool IsBattaryExist { get; private set; }
        public bool IsPowerOnline { get; private set; }
        public int BattaryLifePercent { get; private set; }
        public int BattaryRemainTimeSec { get; private set; }

        public SystemPowerStatus(bool isBattaryExist, bool isPowerOnline, int battaryLifePercent, int battaryRemainTimeSec)
        {
            this.IsBattaryExist = isBattaryExist;
            this.IsPowerOnline = isPowerOnline;
            this.BattaryLifePercent = battaryLifePercent;
            this.BattaryRemainTimeSec = battaryRemainTimeSec;
        }

        public string ToLine(string id)
        {
            return $"{id}:{this.IsBattaryExist}:{this.IsPowerOnline}:{this.BattaryLifePercent}:{this.BattaryRemainTimeSec}";
        }

        public static SystemPowerStatus FromLine(string line, out string id)
        {
            id = null;
            string[] tokens = line.Split(':');
            if (tokens.Length != 5)
                return null;

            id = tokens[0];

            bool isBattaryExist = bool.Parse(tokens[1]);
            bool isPowerOnline = bool.Parse(tokens[2]);
            int battaryLifePercent = int.Parse(tokens[3]);
            int battaryRemainTimeSec = int.Parse(tokens[4]);
            SystemPowerStatus status = new SystemPowerStatus(isBattaryExist, isPowerOnline, battaryLifePercent, battaryRemainTimeSec);
            return status;
        }
    }

    public class UpsSetting
    {
        public UpsType UpsType { get; private set; } = UpsType.None;

        public int BattaryCriticalLevel { get; set; } = 80;

        public UpsSetting(UpsType upsType) { UpsType = upsType; }

        public void Save(XmlElement xmlElement, string key = null)
        {
            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            SaveXml(xmlElement);
        }

        protected virtual void SaveXml(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "UpsType", this.UpsType);
            XmlHelper.SetValue(xmlElement, "BattaryCriticalLevel", this.BattaryCriticalLevel);
        }

        public static UpsSetting Load(XmlElement xmlElement, string key = null)
        {
            if (xmlElement == null)
                return UpsFactory.CreateSetting(UpsType.None);

            if (!string.IsNullOrEmpty(key))
                return Load(xmlElement[key]);

            UpsType type = XmlHelper.GetValue(xmlElement, "UpsType", UpsType.None);
            UpsSetting upsSetting = UpsFactory.CreateSetting(type);
            upsSetting.LoadXml(xmlElement);
            return upsSetting;
        }

        protected virtual void LoadXml(XmlElement xmlElement)
        {
            this.UpsType = XmlHelper.GetValue(xmlElement, "UpsType", this.UpsType);
            this.BattaryCriticalLevel = XmlHelper.GetValue(xmlElement, "BattaryCriticalLevel", this.BattaryCriticalLevel);
        }
    }



    public abstract class Ups
    {
        public UpsSetting UpsSetting { get; private set; }

        public Ups(UpsSetting upsSetting) { this.UpsSetting = upsSetting; }

        public abstract SystemPowerStatus GetPowerState();

        public virtual void StartService() { }
        public virtual void StopService() { }
    }

    public static class UpsFactory
    {
        public static Ups Create(UpsSetting upsSetting)
        {
            switch (upsSetting.UpsType)
            {
                case UpsType.None:
                default:
                    return new NullUps(upsSetting);

                case UpsType.ACPI:
                    return new AcpiUps((AcpiUpsSetting)upsSetting);

                case UpsType.Remote:
                    return new RemoteUps((UdpUpsSetting)upsSetting);
            }
        }

        public static UpsSetting CreateSetting(UpsType upsType)
        {
            switch (upsType)
            {
                case UpsType.None:
                default:
                    return new UpsSetting(UpsType.None);

                case UpsType.ACPI:
                    return new AcpiUpsSetting();

                case UpsType.Remote:
                    return new UdpUpsSetting();
            }
        }
    }
}
