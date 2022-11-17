using DynMvp.Base;
using System;
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

    public class GradeSettingTypeConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return "";
        }
    }

    [TypeConverter(typeof(GradeSettingTypeConverter))]
    public class GradeSetting : SettingElement
    {
        [LocalizedDisplayNameAttributeUniScanG("Value of A [%]"), LocalizedDescriptionAttributeUniScanG("<= Value for Grade A [%]")]
        public float ScoreA
        {
            get => this.scoreA;
            set
            {
                this.scoreA = value;
                if (this.scoreA > this.scoreB)
                    this.ScoreB = this.scoreA;
            }
        }
        float scoreA;

        [LocalizedDisplayNameAttributeUniScanG("Color of A"), LocalizedDescriptionAttributeUniScanG("Color of A")]
        public Color ColorA { get; set; } = Color.Black;
        Color colorA = Color.Green;

        [LocalizedDisplayNameAttributeUniScanG("Value of B [%]"), LocalizedDescriptionAttributeUniScanG("<= Value for Grade B [%]")]
        public float ScoreB
        {
            get => this.scoreB;
            set
            {
                this.scoreB = value;
                if (this.scoreB > this.scoreC)
                    this.ScoreC = this.scoreB;
                if (this.scoreB < this.scoreA)
                    this.ScoreA = this.scoreB;
            }
        }
        float scoreB;

        [LocalizedDisplayNameAttributeUniScanG("Color of B"), LocalizedDescriptionAttributeUniScanG("Color of B")]
        public Color ColorB { get => this.colorB; set => this.colorB = value; }
        Color colorB = Color.Black;

        [LocalizedDisplayNameAttributeUniScanG("Value of C [%]"), LocalizedDescriptionAttributeUniScanG("<= Value for Grade C [%]")]
        public float ScoreC
        {
            get => this.scoreC;
            set
            {
                this.scoreC = value;
                if (this.scoreC < this.scoreB)
                    this.ScoreB = this.scoreC;
            }
        }
        float scoreC;

        [LocalizedDisplayNameAttributeUniScanG("Color of C"), LocalizedDescriptionAttributeUniScanG("Color of C")]
        public Color ColorC { get => this.colorC; set => this.colorC = value; }
        Color colorC = Color.Red;

        [LocalizedDisplayNameAttributeUniScanG("Color of D"), LocalizedDescriptionAttributeUniScanG("Color of D")]
        public Color ColorD { get => this.colorD; set => this.colorD = value; }
        Color colorD = Color.Red;

        //public GradeSetting(bool use) : base(use) { }

        public GradeSetting(bool use, float scoreA, float scoreB, float scoreC) : base(use)
        {
            ScoreA = scoreA;
            ScoreB = scoreB;
            ScoreC = scoreC;
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            XmlHelper.SetValue(xmlElement, "ScoreA", this.scoreA);
            XmlHelper.SetValue(xmlElement, "ScoreB", this.scoreB);
            XmlHelper.SetValue(xmlElement, "ScoreC", this.scoreC);

            XmlHelper.SetValue(xmlElement, "ColorA", this.ColorA);
            XmlHelper.SetValue(xmlElement, "ColorB", this.ColorB);
            XmlHelper.SetValue(xmlElement, "ColorC", this.ColorC);
            XmlHelper.SetValue(xmlElement, "ColorD", this.ColorD);
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            this.scoreA = XmlHelper.GetValue(xmlElement, "ScoreA", this.scoreA);
            this.scoreB = XmlHelper.GetValue(xmlElement, "ScoreB", this.scoreB);
            this.scoreC = XmlHelper.GetValue(xmlElement, "ScoreC", this.scoreC);

            try
            {
                XmlHelper.GetValue(xmlElement, "ColorA", ref this.colorA);
                XmlHelper.GetValue(xmlElement, "ColorB", ref this.colorB);
                XmlHelper.GetValue(xmlElement, "ColorC", ref this.colorC);
                XmlHelper.GetValue(xmlElement, "ColorD", ref this.colorD);
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, ex);
                //LogHelper.Error(LoggerType.Error, $"GradeSetting::Load - {ex.GetType().Name}: {ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }
        }

        public bool IsValid()
        {
            return (0 <= this.scoreA) && (this.scoreA <= this.scoreB) && (this.scoreB <= this.scoreC) && (this.scoreC <= 100);
        }

        internal Color GetColor(string grade)
        {
           switch(grade)
            {
                case "A":
                    return ColorA;
                case "B":
                    return ColorB;
                case "C":
                    return ColorC;
                case "D":
                    return ColorD;
            }
            return Color.Black;
        }
    }
}
