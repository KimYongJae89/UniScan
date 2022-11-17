using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.UI;
using System.Drawing;
using DynMvp.Devices.MotionController;
using DynMvp.Devices;
using System.ComponentModel;
using UniScanM.Data;

namespace UniScanM.ColorSens.Data
{
    public class ColorSensorParam : InspectParam
    {
        double patternLength = 330;  //검사에 영향 전혀 없음 . 나중에 참고나 할까나?
        [LocalizedCategoryAttribute("ColorSensorParam", "Inspection Setting"),
        LocalizedDisplayNameAttribute("ColorSensorParam", "Pattern Length [mm]"),
        LocalizedDescriptionAttribute("ColorSensorParam", "Pattern Length [mm]")]
        public double PatternLength { get => patternLength; set => patternLength = value; }

        double patternPeriod = 350;  //★ 검사에 중요한 조건, 값이 잘못입력되면 정상시트여도 Fluntuation 큼
        [LocalizedCategoryAttribute("ColorSensorParam", "Inspection Setting"),
        LocalizedDisplayNameAttribute("ColorSensorParam", "Pattern Period [mm]"),
        LocalizedDescriptionAttribute("ColorSensorParam", "Pattern Period = Roller Diameter * PI")]
        public double PatternPeriod { get => patternPeriod; set => patternPeriod = value; }

        double averageBrightnessSheet = 128;  //한장의 시트 밝기 8Bit D.N. 값
        [LocalizedCategoryAttribute("ColorSensorParam", "Inspection Setting"),
        LocalizedDisplayNameAttribute("ColorSensorParam", "Average Pattern Brightness [D.N.]"),
        LocalizedDescriptionAttribute("ColorSensorParam", "Average Pattern Brightness [D.N.]")]
        public double AverageBrightnessSheet { get => averageBrightnessSheet; set => averageBrightnessSheet = value; }

        double specLimit = 1;  //검사조건 D.N. 이거보다 작아지면 에러
        [LocalizedCategoryAttribute("ColorSensorParam", "Inspection Setting"),
        LocalizedDisplayNameAttribute("ColorSensorParam", "NG Range[D.N.]"),
        LocalizedDescriptionAttribute("ColorSensorParam", "NG Range[D.N.]")]
        public double SpecLimit { get => specLimit; set => specLimit = value; }
                                      
        const int localAreaCount = 1;


        uint lightValue = 255;
        [LocalizedDisplayNameAttribute("ColorSensorParam", "Light Value"),
        LocalizedDescriptionAttribute("ColorSensorParam", "Light Value")]
        public uint LightValue { get => lightValue; set => lightValue = value; }
        
        double lineSpeed = 50; //m/min
                               // [Category("Inspection Setting"), Description("Local Area Brightness[mm]")]

        [LocalizedDisplayNameAttribute("ColorSensorParam", "Line Speed"),
        LocalizedDescriptionAttribute("ColorSensorParam", "Line Speed")]
        public double LineSpeed
        {
            get
            {
                return lineSpeed;
            }
            set
            {
                lineSpeed = value;
            }
        }

        public override void Export(XmlElement element, string subKey = null)
        {
            XmlHelper.SetValue(element, "PatternLength", patternLength.ToString());
            XmlHelper.SetValue(element, "PatternPeriod", patternPeriod.ToString());
            XmlHelper.SetValue(element, "AverageBrightnessSheet", averageBrightnessSheet.ToString());
            XmlHelper.SetValue(element, "SpecLimit", specLimit.ToString());
            XmlHelper.SetValue(element, "LightValue", lightValue.ToString());
            XmlHelper.SetValue(element, "LineSpeed", lineSpeed.ToString());
        }

        public override void Import(XmlElement element, string subKey = null)
        {
            patternLength = Convert.ToSingle(XmlHelper.GetValue(element, "PatternLength", "0"));
            patternPeriod = Convert.ToSingle(XmlHelper.GetValue(element, "PatternPeriod", "0"));
            averageBrightnessSheet = Convert.ToSingle(XmlHelper.GetValue(element, "AverageBrightnessSheet", "0"));
            specLimit = Convert.ToSingle(XmlHelper.GetValue(element, "SpecLimit", "1"));

            lightValue = Convert.ToUInt32(XmlHelper.GetValue(element, "LightValue", "255"));
            lineSpeed = Convert.ToSingle(XmlHelper.GetValue(element, "LineSpeed", "50"));
        }
    }

    public class Model : UniScanM.Data.Model
    {
        public Model() : base()
        {
            inspectParam = new ColorSensorParam();
        }

        public new ColorSensorParam InspectParam
        {
            get { return (ColorSensorParam)inspectParam; }
        }
    }
}
