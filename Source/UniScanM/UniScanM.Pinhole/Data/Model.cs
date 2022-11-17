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
using UniScanM.Data;
using System.ComponentModel;
using UniScanM.Pinhole.Settings;

namespace UniScanM.Pinhole.Data
{
    public enum NGType
    {
        Any, Bright, Dark
    }

    public class PinholeParamValue
    {
        
        int edgeThreshold = 10;
        public int EdgeThreshold { get => edgeThreshold; set => edgeThreshold = value; }

        int defectThreshold = 30;
        public int DefectThreshold { get => defectThreshold; set => defectThreshold = value; }

        public void Load(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            edgeThreshold = Convert.ToInt32(XmlHelper.GetValue(element, "EdgeThreshold", "10"));
            defectThreshold = Convert.ToInt32(XmlHelper.GetValue(element, "DefectThreshold", "30"));
        }

        public void Save(XmlElement element, string subKey = null)
        {
            XmlHelper.SetValue(element, "EdgeThreshold", edgeThreshold.ToString());
            XmlHelper.SetValue(element, "DefectThreshold", defectThreshold.ToString());
        }
    }

    public class PinholeParam : InspectParam
    {
        PinholeParamValue pinholeParamValue1 = new PinholeParamValue();
        [Browsable(false)]
        public PinholeParamValue PinholeParamValue1 { get => pinholeParamValue1; }

        [LocalizedCategoryAttributePinhole("Pinhole Param 1"),
        LocalizedDisplayNameAttributePinhole("Edge Threshold [D.N.]"),
        LocalizedDescriptionAttributePinhole("Edge Threshold [D.N.]")]
        public int EdgeThreshold1 { get => pinholeParamValue1.EdgeThreshold; }

        [LocalizedCategoryAttributePinhole("Pinhole Param 1"),
        LocalizedDisplayNameAttributePinhole("Defect Threshold [D.N.]"),
        LocalizedDescriptionAttributePinhole("Defect Threshold [D.N.]")]
        public int DefectThreshold1 { get => pinholeParamValue1.DefectThreshold; }

        PinholeParamValue pinholeParamValue2 = new PinholeParamValue();

        [Browsable(false)]
        public PinholeParamValue PinholeParamValue2 { get => pinholeParamValue2; }

        [LocalizedCategoryAttributePinhole("Pinhole Param 2"),
        LocalizedDisplayNameAttributePinhole("Edge Threshold [D.N.]"),
        LocalizedDescriptionAttributePinhole("Edge Threshold [D.N.]")]
        public int EdgeThreshold2 { get => pinholeParamValue2.EdgeThreshold; }

        [LocalizedCategoryAttributePinhole("Pinhole Param 2"),
        LocalizedDisplayNameAttributePinhole("Defect Threshold [D.N.]"),
        LocalizedDescriptionAttributePinhole("Defect Threshold [D.N.]")]
        public int DefectThreshold2 { get => pinholeParamValue2.DefectThreshold; }

        //
        [LocalizedCategoryAttributePinhole("Pinhole NG Type"),
        LocalizedDisplayNameAttributePinhole("NG Type"),
        LocalizedDescriptionAttributePinhole("NG Type")]
        public NGType NGType {
            get => this.eNGType;
            set => this.eNGType = value;
        }
        private NGType eNGType =  NGType.Any;



        public override void Export(XmlElement element, string subKey = null)
        {
            XmlElement paramElement1 = element.OwnerDocument.CreateElement("Param1");
            pinholeParamValue1.Save(paramElement1);
            element.AppendChild(paramElement1);

            XmlElement paramElement2 = element.OwnerDocument.CreateElement("Param2");
            pinholeParamValue2.Save(paramElement2);
            element.AppendChild(paramElement2);

            XmlHelper.SetValue(element, "NGType", eNGType.ToString());
        }

        public override void Import(XmlElement element, string subKey = null)
        {
            pinholeParamValue1.Load(element["Param1"]);
            pinholeParamValue2.Load(element["Param2"]);
            XmlHelper.SetValue(element, "NGTye", eNGType.ToString());
            eNGType = ( NGType) Enum.Parse(typeof(NGType), (XmlHelper.GetValue(element, "NGType", "Any")) );
        } 
    }

    public class Model : UniScanM.Data.Model
    {
        public Model() : base()
        {
            inspectParam = new PinholeParam();
        }

        public new PinholeParam InspectParam
        {
            get { return (PinholeParam)inspectParam; }
        }
    }
}
