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
    public class StickerSensorSetting : SettingElement
    {
        [LocalizedDisplayNameAttributeUniScanG("Before Erase Length[m]"), LocalizedDescriptionAttributeUniScanG("Before Erase Length[m]")]
        public float BeforeEraseLengthM { get => beforeEraseLengthM; set => beforeEraseLengthM = value; }
        float beforeEraseLengthM;

        [LocalizedDisplayNameAttributeUniScanG("After Erase Length[m]"), LocalizedDescriptionAttributeUniScanG("After Erase Length[m]")]
        public float AfterEraseLengthM { get => afterEraseLengthM; set => afterEraseLengthM = value; }
        float afterEraseLengthM;

        [LocalizedDisplayNameAttributeUniScanG("Sticker Length[mm]"), LocalizedDescriptionAttributeUniScanG("Sticker Length[mm]")]
        public float StickerLengthMm { get => stickerLengthMm; set => stickerLengthMm = value; }
        float stickerLengthMm;

        public float EraseLengthM => this.beforeEraseLengthM + this.afterEraseLengthM;

        public StickerSensorSetting() : base(false)
        {
            this.beforeEraseLengthM = this.beforeEraseLengthM = this.stickerLengthMm = 0;
        }

        public StickerSensorSetting(bool use, float beforeEraseLengthM, float afterEraseLengthM, float stickerLengthMm) : base(use)
        {
            this.beforeEraseLengthM = beforeEraseLengthM;
            this.afterEraseLengthM = afterEraseLengthM;
            this.stickerLengthMm = stickerLengthMm;
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlHelper.SetValue(xmlElement, "BeforeEraseLengthM", beforeEraseLengthM.ToString());
            XmlHelper.SetValue(xmlElement, "AfterEraseLengthM", afterEraseLengthM.ToString());
            XmlHelper.SetValue(xmlElement, "StickerLengthMm", stickerLengthMm.ToString());
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            this.beforeEraseLengthM = XmlHelper.GetValue(xmlElement, "BeforeEraseLengthM", beforeEraseLengthM);
            this.afterEraseLengthM = XmlHelper.GetValue(xmlElement, "AfterEraseLengthM", afterEraseLengthM);
            this.stickerLengthMm = XmlHelper.GetValue(xmlElement, "StickerLengthMm", stickerLengthMm);
        }

        public override string ToString()
        {
            //return string.Format("Pinhole {0} / Noprint {1} / Coating {2} / Sheetattack {3}", this.pinhole, this.noprint, this.coating, this.sheetattack);
            return this.Use ? "Use" : "Unuse";
        }
    }
}
