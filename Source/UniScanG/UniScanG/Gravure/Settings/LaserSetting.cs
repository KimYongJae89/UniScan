using DynMvp.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Data;

namespace UniScanG.Gravure.Settings
{

    public class LaserSetting : SettingElement
    {
        public enum EPrinterUseLogic { None, And, Or }

        [LocalizedDisplayNameAttributeUniScanG("Laser Distance[m]"), LocalizedDescriptionAttributeUniScanG("Laser Distance[m]")]
        public double DistanceM { get => distanceM; set => distanceM = value; }
        double distanceM = 3.3;

        [LocalizedDisplayNameAttributeUniScanG("Safe Distance[m]"), LocalizedDescriptionAttributeUniScanG("Safe Distance[m]")]
        public double SafeDistanceM { get => safeDistanceM; set => safeDistanceM = value; }
        double safeDistanceM = 0.3;

        [LocalizedDisplayNameAttributeUniScanG("Hold Time[ms]"), LocalizedDescriptionAttributeUniScanG("I/O Hold Time[m]")]
        public int HoldTimeMs { get => holdTimeMs; set => holdTimeMs = value; }
        int holdTimeMs = 50;

        [LocalizedDisplayNameAttributeUniScanG("Condition"), LocalizedDescriptionAttributeUniScanG("Condition")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ConditionSettingCollection ConditionSettingCollection { get => conditionSettingCollection; }
        ConditionSettingCollection conditionSettingCollection = new ConditionSettingCollection();

        [Browsable(false)]
        [LocalizedDisplayNameAttributeUniScanG("Sticker Sensor"), LocalizedDescriptionAttributeUniScanG("Sticker Sensor")]
        public StickerSensorSetting StickerSensorSetting { get => stickerSensorSetting; }
        StickerSensorSetting stickerSensorSetting = new StickerSensorSetting(false, 0, 0, 0);

        [LocalizedDisplayNameAttributeUniScanG("Printer Use Logic"), LocalizedDescriptionAttributeUniScanG("Printer Use Logic")]
        public EPrinterUseLogic PrinterUseLogic { get => this.printerUseLogic; set => this.printerUseLogic = value; }
        EPrinterUseLogic printerUseLogic = EPrinterUseLogic.And;

        public LaserSetting() : base(false)
        {
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlHelper.SetValue(xmlElement, "DistanceM", this.distanceM);
            XmlHelper.SetValue(xmlElement, "SafeDistanceM", this.safeDistanceM);
            XmlHelper.SetValue(xmlElement, "HoldTimeMs", this.holdTimeMs);

            this.conditionSettingCollection.Save(xmlElement, "ConditionSettingCollection");
            this.stickerSensorSetting.Save(xmlElement, "StickerSensorSetting");

            XmlHelper.SetValue(xmlElement, "PrinterUseLogic", this.printerUseLogic);
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            this.distanceM = XmlHelper.GetValue(xmlElement, "DistanceM", this.distanceM);
            this.safeDistanceM = XmlHelper.GetValue(xmlElement, "SafeDistanceM", this.safeDistanceM);
            this.holdTimeMs = XmlHelper.GetValue(xmlElement, "HoldTimeMs", this.holdTimeMs);

            this.conditionSettingCollection.Load(xmlElement, "ConditionSettingCollection");
            this.stickerSensorSetting.Load(xmlElement, "StickerSensorSetting");

            this.printerUseLogic = XmlHelper.GetValue(xmlElement, "PrinterUseLogic", this.printerUseLogic);
        }

        public override string ToString()
        {
            //return string.Format("Pinhole {0} / Noprint {1} / Coating {2} / Sheetattack {3}", this.pinhole, this.noprint, this.coating, this.sheetattack);
            return this.Use ? "Use" : "Unuse";
        }
    }


    public class ConditionSettingCollection : CollectionBase, ICustomTypeDescriptor
    {
        public ConditionSetting this[int i] => (ConditionSetting)this.InnerList[i];

        public override string ToString()
        {
            return string.Format("{0} EA", this.InnerList.Count);
        }

        public void Save(XmlElement xmlElement, string key = "")
        {
            if (xmlElement == null)
                return;

            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement.OwnerDocument.CreateElement(key);
                xmlElement.AppendChild(subElement);
                Save(subElement);
                return;
            }

            foreach (var inner in this.InnerList)
            {
                ConditionSetting conditionSetting = inner as ConditionSetting;
                if (conditionSetting != null)
                    conditionSetting.Save(xmlElement, "ConditionSetting");
            }
        }

        internal bool Load(XmlElement xmlElement, string key = "")
        {
            if (xmlElement == null)
                return false;

            if (!string.IsNullOrEmpty(key))
            {
                XmlElement subElement = xmlElement[key];
                return Load(subElement);
            }

            this.InnerList.Clear();
            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("ConditionSetting");
            foreach (XmlElement subElement in xmlNodeList)
            {
                ConditionSetting conditionSetting = new ConditionSetting();
                conditionSetting.Load(subElement);
                this.InnerList.Add(conditionSetting);
            }
            return true;
        }

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);

        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);

        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);

        }

        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);

        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);

        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);

        }

        public PropertyDescriptorCollection GetProperties()
        {
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);
            for (int i = 0; i < this.List.Count; i++)
                pds.Add(new ConditionSettingDescriptor(this, i));

            return pds;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
    }

    public enum Target
    {
        Total = UniScanG.Data.DefectType.Total,
        Noprint = UniScanG.Data.DefectType.Noprint,
        Pinhole = UniScanG.Data.DefectType.PinHole,
        Spread = UniScanG.Data.DefectType.Spread,
        Coating = UniScanG.Data.DefectType.Coating,
        SheetAttack = UniScanG.Data.DefectType.Attack,
        Sticker = UniScanG.Data.DefectType.Sticker
    }
    public class ConditionSettingDescriptor : PropertyDescriptor
    {
        private ConditionSettingCollection collection = null;
        int index = -1;

        public ConditionSettingDescriptor(ConditionSettingCollection coll, int idx) :
            base(string.Format("#{0}", idx), null)
        {
            this.collection = coll;
            this.index = idx;
        }

        public override AttributeCollection Attributes => new AttributeCollection(null);

        public override Type ComponentType => this.collection.GetType();

        public override bool IsReadOnly => true;

        public override Type PropertyType => this.collection[this.index].GetType();

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            return this.collection[this.index];
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {

        }

        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }

        public override string DisplayName => "";
        //public override string DisplayName => this.collection[this.index].ToString();
        public override string Description => base.Description;
    }

    public class ConditionSetting : SettingElement
    {
        [LocalizedDisplayNameAttributeUniScanG("Target"), LocalizedDescriptionAttributeUniScanG("Target")]
        public Target Target { get => this.target; set => this.target = value; }
        Target target;

        [LocalizedDisplayNameAttributeUniScanG("Min Size[um]"), LocalizedDescriptionAttributeUniScanG("Min Size[um]")]
        public float MinSize { get => this.minSize; set => this.minSize = value; }
        float minSize;

        [LocalizedDisplayNameAttributeUniScanG("Min Count[EA]"), LocalizedDescriptionAttributeUniScanG("Min Count[EA]")]
        public int MinCount { get => this.minCount; set => this.minCount = value; }
        int minCount;

        //[LocalizedDisplayNameAttributeUniScanG("Min Pinhole Count"), LocalizedDescriptionAttributeUniScanG("Min Pinhole Count")]
        //public int Pinhole { get => pinhole; set => pinhole = value; }
        //int pinhole;

        //[LocalizedDisplayNameAttributeUniScanG("Min Noprint Count"), LocalizedDescriptionAttributeUniScanG("Min Noprint Count")]
        //public int Noprint { get => noprint; set => noprint = value; }
        //int noprint;
        //[LocalizedDisplayNameAttributeUniScanG("Min Coating Count"), LocalizedDescriptionAttributeUniScanG("Min Coating Count")]
        //public int Coating { get => coating; set => coating = value; }
        //int coating;

        //[LocalizedDisplayNameAttributeUniScanG("Min Sheetattack Count"), LocalizedDescriptionAttributeUniScanG("Min Sheetattack Count")]
        //public int Sheetattack { get => sheetattack; set => sheetattack = value; }
        //int sheetattack;

        public ConditionSetting() : base(false)
        {
            this.target = Target.Pinhole;
            this.minSize = 0;
            this.minCount = 0;
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlHelper.SetValue(xmlElement, "Target", target.ToString());
            XmlHelper.SetValue(xmlElement, "MinSize", minSize.ToString());
            XmlHelper.SetValue(xmlElement, "MinCount", minCount.ToString());
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            this.target = XmlHelper.GetValue(xmlElement, "Target", target);
            this.minSize = XmlHelper.GetValue(xmlElement, "MinSize", minSize);
            this.minCount = XmlHelper.GetValue(xmlElement, "MinCount", minCount);
        }

        public override string ToString()
        {
            return string.Format("{0} / {1} / {2}[um] / {3}[EA]", this.Use ? "Use" : "Unuse", this.target, this.minSize, this.minCount);
        }

        public bool Check(List<FoundedObjInPattern> sheetSubResultList)
        {
            if (this.Use == false)
                return false;

            int count = sheetSubResultList.Count(f =>
            {
                DefectType target = (DefectType)this.Target;
                return (target == DefectType.Total || f.GetDefectType() == target) && f.PostProcessSize > this.MinSize;
            });

            if (count >= this.MinCount)
                return true;

            return false;
        }
    }
}
