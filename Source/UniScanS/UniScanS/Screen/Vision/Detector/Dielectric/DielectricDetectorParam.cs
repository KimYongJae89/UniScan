using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;

namespace UniScanS.Screen.Vision.Detector.Dielectric
{
    public class DielectricInspectorParam
    {
        string name = "Dielectric";

        bool useLower;
        public bool UseLower
        {
            get { return useLower; }
            set { useLower = value; }
        }

        bool useUpper;
        public bool UseUpper
        {
            get { return useUpper; }
            set { useUpper = value; }
        }

        int lowerThreshold;
        public int LowerThreshold
        {
            get { return lowerThreshold; }
            set { lowerThreshold = value; }
        }

        int upperThreshold;
        public int UpperThreshold
        {
            get { return upperThreshold; }
            set { upperThreshold = value; }
        }

        public DielectricInspectorParam()
        {
            useLower = true;
            useUpper = true;
            lowerThreshold = 0;
            upperThreshold = 0;
        }

        public DielectricInspectorParam Clone()
        {
            DielectricInspectorParam clone = new DielectricInspectorParam();
            clone.Copy(this);

            return clone;
        }

        public void Copy(DielectricInspectorParam srcParam)
        {
            this.lowerThreshold = srcParam.lowerThreshold;
            this.upperThreshold = srcParam.upperThreshold;
        }

        public void SaveParam(XmlElement algorithmElement)
        {
            XmlElement dielectricElement = algorithmElement.OwnerDocument.CreateElement(name);
            algorithmElement.AppendChild(dielectricElement);

            XmlHelper.SetValue(dielectricElement, "UseLower", useLower.ToString());
            XmlHelper.SetValue(dielectricElement, "UseUpper", useUpper.ToString());
            XmlHelper.SetValue(dielectricElement, "LowerThreshold", lowerThreshold.ToString());
            XmlHelper.SetValue(dielectricElement, "UpperThreshold", upperThreshold.ToString());
        }

        public void LoadParam(XmlElement algorithmElement)
        {
            XmlElement dielectricElement = algorithmElement[name];

            if (dielectricElement == null)
                return;

            useLower = Convert.ToBoolean(XmlHelper.GetValue(dielectricElement, "UseLower", "true"));
            useUpper = Convert.ToBoolean(XmlHelper.GetValue(dielectricElement, "UseUpper", "true"));
            lowerThreshold = Convert.ToInt32(XmlHelper.GetValue(dielectricElement, "LowerThreshold", "0"));
            upperThreshold = Convert.ToInt32(XmlHelper.GetValue(dielectricElement, "UpperThreshold", "0"));
        }
    }
}
