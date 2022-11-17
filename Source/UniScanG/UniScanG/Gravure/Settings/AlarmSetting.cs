using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanG.Gravure.Settings
{
    /// <summary>
    /// 알람 설정 관련
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AlarmSetting : SettingElement
    {
        [LocalizedDisplayNameAttributeUniScanG("Min Count"), LocalizedDescriptionAttributeUniScanG("Minimum Count")]
        public int Count { get; set; }

        protected string unit;

        public AlarmSetting(bool use, int count, string unit) : base(use)
        {
            this.Count = count;
            this.unit = unit;
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlHelper.SetValue(xmlElement, "Count", this.Count.ToString());
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            this.Count = XmlHelper.GetValue(xmlElement, "Count", this.Count);
        }

        public override string ToString()
        {
            return string.Format("{0} / {1}[{2}]", this.Use ? "Use" : "Unuse", this.Count, this.unit);
        }
    }

    /// <summary>
    /// 비율(%)로 알람 판정.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RelativeAlarmSetting : AlarmSetting
    {
        [LocalizedDisplayNameAttributeUniScanG("Min NG Ratio"), LocalizedDescriptionAttributeUniScanG("Minimum NG Ratio")]
        public double Percent { get; set; }

        public RelativeAlarmSetting(bool use, int count, string unit, double percent) : base(use, count, unit)
        {
            this.Percent = percent;
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);
                
            XmlHelper.SetValue(xmlElement, "Percent", this.Percent.ToString());
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            this.Percent = XmlHelper.GetValue(xmlElement, "Percent", this.Percent);
        }

        public override string ToString()
        {
            return string.Format("{0} / {1}[{2}] / {3:F2}[%]", this.Use ? "Use" : "Unuse", Count, unit, this.Percent);
        }
    }

    /// <summary>
    /// 일반불량알람
    /// </summary>
    public class NormalDefectAlarmSetting : RelativeAlarmSetting
    {
        [LocalizedDisplayNameAttributeUniScanG("Minimum Defects in a Pattern"), LocalizedDescriptionAttributeUniScanG("Minimum Defects in a Pattern")]
        public int MinimumDefects { get; set; }

        public NormalDefectAlarmSetting(bool use, int count, int minimumDefects, double percent) : base(use, count, "Patterns", percent)
        {
            this.MinimumDefects = minimumDefects;
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlHelper.SetValue(xmlElement, "MinimumDefects", this.MinimumDefects);
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            this.MinimumDefects = XmlHelper.GetValue(xmlElement, "MinimumDefects", this.MinimumDefects);
        }
    }

    /// <summary>
    /// 반복불량알람 모음
    /// </summary>
    public class RepeatedDefectAlarmSetting : RelativeAlarmSetting
    {
        [LocalizedDisplayNameAttributeUniScanG("Inflate Range W"), LocalizedDescriptionAttributeUniScanG("Inflate Range W")]
        public float InflateRangeWUm { get; set; }

        [LocalizedDisplayNameAttributeUniScanG("Inflate Range H"), LocalizedDescriptionAttributeUniScanG("Inflate Range H")]
        public float InflateRangeHUm { get; set; }

        public RepeatedDefectAlarmSetting(bool use, int count, string unit, double percent) : base(use, count, unit, percent)
        {
            this.InflateRangeWUm = 140;
            this.InflateRangeHUm = 140;
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);
            XmlHelper.SetValue(xmlElement, "InflateRangeW", this.InflateRangeWUm);
            XmlHelper.SetValue(xmlElement, "InflateRangeH", this.InflateRangeHUm);
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);
            this.InflateRangeWUm = XmlHelper.GetValue(xmlElement, "InflateRangeW", this.InflateRangeWUm);
            this.InflateRangeHUm = XmlHelper.GetValue(xmlElement, "InflateRangeH", this.InflateRangeHUm);
        }
    }

    /// <summary>
    /// 반복불량알람 모음
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RepeatedDefectAlarmSettingCollection : SettingElementCollection
    {
        public enum Types
        {
            Etcetera = -1,
            PinHole = UniScanG.Data.DefectType.PinHole,
            Spread = UniScanG.Data.DefectType.Spread,
            NoPrint = UniScanG.Data.DefectType.Noprint,
        }

        [LocalizedDisplayNameAttributeUniScanG("Etcetera"), LocalizedDescriptionAttributeUniScanG("Etcetera")]
        public RepeatedDefectAlarmSetting Etcetera => (RepeatedDefectAlarmSetting)this.dic[Types.Etcetera];

        [LocalizedDisplayNameAttributeUniScanG("PinHole"), LocalizedDescriptionAttributeUniScanG("PinHole")]
        public RepeatedDefectAlarmSetting PinHole => (RepeatedDefectAlarmSetting)this.dic[Types.PinHole];

        [LocalizedDisplayNameAttributeUniScanG("NoPrint"), LocalizedDescriptionAttributeUniScanG("NoPrint")]
        public RepeatedDefectAlarmSetting NoPrint => (RepeatedDefectAlarmSetting)this.dic[Types.NoPrint];

        [LocalizedDisplayNameAttributeUniScanG("Spread"), LocalizedDescriptionAttributeUniScanG("Spread")]
        public RepeatedDefectAlarmSetting Spread => (RepeatedDefectAlarmSetting)this.dic[Types.Spread];

        public RepeatedDefectAlarmSettingCollection(bool use, int count, string unit, double percent)
        {
            this.dic.Add(Types.Etcetera, new RepeatedDefectAlarmSetting(use, count, unit, percent));
            this.dic.Add(Types.PinHole, new RepeatedDefectAlarmSetting(use, count, unit, percent));
            this.dic.Add(Types.NoPrint, new RepeatedDefectAlarmSetting(use, count, unit, percent));
            this.dic.Add(Types.Spread, new RepeatedDefectAlarmSetting(use, count, unit, percent));
        }

        public RepeatedDefectAlarmSetting GetAlarmSetting(UniScanG.Data.DefectType defectType)
        {
            try
            {
                Types types = (Types)defectType;
                if (this.dic.ContainsKey(types))
                    return (RepeatedDefectAlarmSetting)this.dic[types];
            return (RepeatedDefectAlarmSetting)this.dic[Types.Etcetera];
            }
            catch 
            {
                return (RepeatedDefectAlarmSetting)this.dic[Types.Etcetera];
            }
        }

        public RepeatedDefectAlarmSetting GetAlarmSetting(Types type)
        {
            return (RepeatedDefectAlarmSetting)this.dic[type];
        }
    }

    /// <summary>
    /// 절대 개수(EA)로 알람 판정.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AbsoluteAlarmSetting : AlarmSetting
    {
        [LocalizedDisplayNameAttributeUniScanG("Min Difference Value"), LocalizedDescriptionAttributeUniScanG("Minimum Difference Value")]
        public double Value { get; set; }

        string unit2;

        public AbsoluteAlarmSetting(bool use, int count, string unit, double value, string unit2) : base(use, count, unit)
        {
            this.Value = value;
            this.unit2 = unit2;
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlHelper.SetValue(xmlElement, "Value", this.Value);
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            this.Value = XmlHelper.GetValue(xmlElement, "Value", this.Value);
        }

        public override string ToString()
        {
            return string.Format("{0} / {1}[{2}] / {3:F2}[{4}]", this.Use ? "Use" : "Unuse", Count, unit, Value, unit2);
        }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class StripeDefectAlarmSetting : RelativeAlarmSetting
    {
        [LocalizedDisplayNameAttributeUniScanG("Window Width"), LocalizedDescriptionAttributeUniScanG("Window Width")]
        public float WindowWidth { get; set; }

        public StripeDefectAlarmSetting(bool use, float windowWidth, double percent) : base(use, 1, "EA", percent)
        {
            this.WindowWidth = windowWidth;
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlHelper.SetValue(xmlElement, "WindowWidth", this.WindowWidth);
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            this.WindowWidth = XmlHelper.GetValue(xmlElement, "WindowWidth", this.WindowWidth);
        }

        public override string ToString()
        {
            return string.Format("{0} / {1:F1}[mm] / {2:F2}[%]", this.Use ? "Use" : "Unuse", this.WindowWidth, this.Percent);
        }
    }

    public class CriticalRollAlarmSetting : AlarmSetting
    {
        [Browsable(false)]
        public string LastAlarmedLotNo { get; set; }

        [LocalizedDisplayNameAttributeUniScanG("Defect Count"), LocalizedDescriptionAttributeUniScanG("Defect Count")]
        public float DefectCount { get; set; }

        [LocalizedDisplayNameAttributeUniScanG("Defect Length"), LocalizedDescriptionAttributeUniScanG("Defect Length")]
        public double DefectLength { get; set; }

        [LocalizedDisplayNameAttributeUniScanG("Notify Once"), LocalizedDescriptionAttributeUniScanG("Notify Once")]
        public bool NotifyOnce{ get; set; }

        public CriticalRollAlarmSetting(bool use, int patternCount, int defectCount, double defectLength) : base(use, patternCount, "EA")
        {
            this.DefectCount = defectCount;
            this.DefectLength = defectLength;
            this.NotifyOnce = false;
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlHelper.SetValue(xmlElement, "DefectCount", this.DefectCount);
            XmlHelper.SetValue(xmlElement, "DefectLength", this.DefectLength);
            XmlHelper.SetValue(xmlElement, "NotifyOnce", this.NotifyOnce);
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            this.DefectCount = XmlHelper.GetValue(xmlElement, "DefectCount", this.DefectCount);
            this.DefectLength = XmlHelper.GetValue(xmlElement, "DefectLength", this.DefectLength);
            this.NotifyOnce = XmlHelper.GetValue(xmlElement, "NotifyOnce", this.NotifyOnce);
        }
    }
}
