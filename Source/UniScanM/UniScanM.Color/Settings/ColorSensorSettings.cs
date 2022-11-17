using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanM.ColorSens.Settings
{
    public class ColorSensorSettings : UniScanM.Settings.UniScanMSettings
    {
        protected ColorSensorSettings() { }

        public static ColorSensorSettings Instance()
        {
            return instance as ColorSensorSettings;
        }

        public static new void CreateInstance()
        {
            if (instance == null)
                instance = new ColorSensorSettings();
        }

        //***************************************************************************************//
        double opticResolution_umPerPixel = 235.0;
        [LocalizedCategoryAttributeColor("HardWare Setting"),
            LocalizedDisplayNameAttributeColor("Optical Resolution [um/pixel]")]
        public double OpticResolution_umPerPixel
        {
            get { return opticResolution_umPerPixel; }
            set { opticResolution_umPerPixel = value; }
        }
        
        int integrateReferenceFrame = 20;
        [LocalizedCategoryAttributeColor("Inspection"),
            LocalizedDisplayNameAttributeColor("Integrate Reference Frame Count")]
        public int IntegrateReferenceFrame
        {
            get { return integrateReferenceFrame; }
            set { integrateReferenceFrame = Math.Max(5, value); }
        }

        UInt32 graphCertain_XDistance = 100;
        [LocalizedCategoryAttributeColor("Graph"),
            LocalizedDisplayNameAttributeColor("Certain Graph X-Axis Distance [m]")]
        public UInt32 GraphCertain_XDistance
        {
            get { return graphCertain_XDistance; }
            set { graphCertain_XDistance = value; }
        }

        UInt32 graphCertain_YLength = 10;
        [LocalizedCategoryAttributeColor("Graph"),
            LocalizedDisplayNameAttributeColor("Certain Graph Y-Axis Interval [DN]")]
        public UInt32 GraphCertain_YLength
        {
            get { return graphCertain_YLength; }
            set { graphCertain_YLength = value; }
        }

        int graphCertain_Thick = 2;
        [LocalizedCategoryAttributeColor("Graph"),
            LocalizedDisplayNameAttributeColor("Certain Graph Thickness [pt]")]
        public int GraphCertain_Thick
        {
            get { return graphCertain_Thick; }
            set { graphCertain_Thick = value; }
        }

        UInt32 graphWhole_XDistance = 5000;
        [LocalizedCategoryAttributeColor("Graph"),
            LocalizedDisplayNameAttributeColor("whole Graph X-Axis Distance [m]")]
        public UInt32 GraphWhole_XDistance
        {
            get { return graphWhole_XDistance; }
            set { graphWhole_XDistance = value; }
        }

        UInt32 graphWhole_YLength = 10;
        [LocalizedCategoryAttributeColor("Graph"),
            LocalizedDisplayNameAttributeColor("whole Graph Y-Axis Interval [DN]")]
        public UInt32 GraphWhole_YLength
        {
            get { return graphWhole_YLength; }
            set { graphWhole_YLength = value; }
        }

        int graphWhole_Thick = 2;
        [LocalizedCategoryAttributeColor("Graph"),
            LocalizedDisplayNameAttributeColor("whole Graph Thickness [pt]")]
        public int GraphWhole_Thick
        {
            get { return graphWhole_Thick; }
            set { graphWhole_Thick = value; }
        }

        int graphWhole_denominator = 20;
        [LocalizedCategoryAttributeColor("Graph"),
            LocalizedDisplayNameAttributeColor("whole Graph Denominator [pt]")]
        public int GraphWhole_denominator
        {
            get { return graphWhole_denominator; }
            set { graphWhole_denominator = value; }
        }
        

        //***************************************************************************************//
        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            if (xmlElement == null)
                return;

            XmlHelper.SetValue(xmlElement, "opticResolution_umPerPixel", opticResolution_umPerPixel.ToString());
            XmlHelper.SetValue(xmlElement, "graphCertain_XDistance", graphCertain_XDistance.ToString());
            XmlHelper.SetValue(xmlElement, "graphCertain_YLength", graphCertain_YLength.ToString());
            XmlHelper.SetValue(xmlElement, "graphCertain_Thick", graphCertain_Thick.ToString());
            XmlHelper.SetValue(xmlElement, "graphWhole_XDistance", graphWhole_XDistance.ToString());
            XmlHelper.SetValue(xmlElement, "graphWhole_YLength", graphWhole_YLength.ToString());
            XmlHelper.SetValue(xmlElement, "graphWhole_Thick", graphWhole_Thick.ToString());
            XmlHelper.SetValue(xmlElement, "graphWhole_denominator", graphWhole_denominator.ToString());

            XmlHelper.SetValue(xmlElement, "IntegrateReferenceFrame", IntegrateReferenceFrame.ToString());
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            if (xmlElement == null)
                return;

            //encoderResolution = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "EncoderUmPerPulse", encoderResolution.ToString()));
            ////autoStartMethod = (EAutoStartMethod)Enum.Parse(typeof(EAutoStartMethod), XmlHelper.GetValue(xmlElement, "AutoStartMethod", autoStartMethod.ToString()));
            //asyncMode = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "AsyncMode", asyncMode.ToString()));
            //asyncGrabHz = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "AsyncGrabHz", asyncGrabHz.ToString()));
            //rollTotalLength = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "RollTotalLength", rollTotalLength.ToString()));

            //XmlHelper.SetValue(xmlElement, "opticResolution_umPerPixel", opticResolution_umPerPixel.ToString());
            opticResolution_umPerPixel = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "opticResolution_umPerPixel", "235"));

            //XmlHelper.SetValue(xmlElement, "graphCertain_XDistance", graphCertain_XDistance.ToString());
            graphCertain_XDistance = (uint)Convert.ToInt32(XmlHelper.GetValue(xmlElement, "graphCertain_XDistance", "100"));

            //XmlHelper.SetValue(xmlElement, "graphCertain_YMargin", graphCertain_YMargin.ToString());
            graphCertain_YLength = (uint)Convert.ToInt32(XmlHelper.GetValue(xmlElement, "graphCertain_YLength", "10"));
            graphCertain_Thick = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "graphCertain_Thick", graphCertain_Thick.ToString()));

            //XmlHelper.SetValue(xmlElement, "graphWhole_XDistance", graphWhole_XDistance.ToString());
            graphWhole_XDistance = (uint)Convert.ToInt32(XmlHelper.GetValue(xmlElement, "graphWhole_XDistance", "5000"));

            //XmlHelper.SetValue(xmlElement, "graphWhole_YMargin", graphWhole_YMargin.ToString());
            graphWhole_YLength = (uint)Convert.ToInt32(XmlHelper.GetValue(xmlElement, "graphWhole_YLength", "10"));

            graphWhole_Thick= Convert.ToInt32(XmlHelper.GetValue(xmlElement, "graphWhole_Thick", graphWhole_Thick.ToString()));
            graphWhole_denominator = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "graphWhole_denominator", graphWhole_denominator.ToString()));
            
            //XmlHelper.SetValue(xmlElement, "IntegrateReferenceFrame", IntegrateReferenceFrame.ToString());
            integrateReferenceFrame = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "integrateReferenceFrame", "20"));
        }
    }
}
