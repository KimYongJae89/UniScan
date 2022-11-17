using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;
using DynMvp.Vision;

namespace UniScanS.Screen.Vision.Detector.Pole
{
    public class PoleInspectorParam
    {
        string name = "Pole";

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

        public PoleInspectorParam()
        {
            useLower = true;
            useUpper = true;
            lowerThreshold = 0;
            upperThreshold = 0;
        }

        public PoleInspectorParam Clone()
        {
            PoleInspectorParam clone = new PoleInspectorParam();
            clone.Copy(this);

            return clone;
        }

        public void Copy(PoleInspectorParam srcParam)
        {
            this.lowerThreshold = srcParam.lowerThreshold;
            this.upperThreshold = srcParam.upperThreshold;
        }

        public void SaveParam(XmlElement algorithmElement)
        {
            XmlElement poleElement = algorithmElement.OwnerDocument.CreateElement(name);
            algorithmElement.AppendChild(poleElement);

            XmlHelper.SetValue(poleElement, "UseLower", useLower.ToString());
            XmlHelper.SetValue(poleElement, "UseUpper", useUpper.ToString());
            XmlHelper.SetValue(poleElement, "LowerThreshold", lowerThreshold.ToString());
            XmlHelper.SetValue(poleElement, "UpperThreshold", upperThreshold.ToString());
        }

        public void LoadParam(XmlElement algorithmElement)
        {
            XmlElement poleElement = algorithmElement[name];

            if (poleElement == null)
                return;

            useLower = Convert.ToBoolean(XmlHelper.GetValue(poleElement, "UseLower", "true"));
            useUpper = Convert.ToBoolean(XmlHelper.GetValue(poleElement, "UseUpper", "true"));
            lowerThreshold = Convert.ToInt32(XmlHelper.GetValue(poleElement, "LowerThreshold", "0"));
            upperThreshold = Convert.ToInt32(XmlHelper.GetValue(poleElement, "UpperThreshold", "0"));
        }
    }
}
