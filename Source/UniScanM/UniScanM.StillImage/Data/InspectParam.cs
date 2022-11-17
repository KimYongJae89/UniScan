using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanM.Data;

namespace UniScanM.StillImage.Data
{
    public class InspectParam : UniScanM.Data.InspectParam, IExportable, IImportable
    {
        bool isRelativeOffset = false;
        Feature offsetRange;  // 마진 블랏 픽셀단위
        float matchRatio = 0.8f;
       // double printingLengthWarnLevelum = 1000; //um

        int inspectionLevelMin = 10; // insp  DN
        int inspectionLevelMax = 50;// insp   DN
        int inspectionSizeMin = 10;// insp//um
        int inspectionSizeMax = 1000;// insp//um
        int inspectionRefEdgeLevel = 22;// insp

        public int InspectionRefEdgeLevel
        {
            get { return inspectionRefEdgeLevel; }
            set { inspectionRefEdgeLevel = value; }
        }

        public int InspectionSizeMax
        {
            get { return inspectionSizeMax; }
            set { inspectionSizeMax = value; }
        }

        public int InspectionSizeMin
        {
            get { return inspectionSizeMin; }
            set { inspectionSizeMin = value; }
        }
        //public double PrintingLengthWarnLevelum
        //{
        //    get { return printingLengthWarnLevelum; }
        //    set { printingLengthWarnLevelum = value; }
        //}
        public int InspectionLevelMin
        {
            get { return inspectionLevelMin; }
            set { inspectionLevelMin = value; }
        }
        public int InspectionLevelMax
        {
            get { return inspectionLevelMax; }
            set { inspectionLevelMax = value; }
        }
        public bool IsRelativeOffset
        {
            get { return isRelativeOffset; }
            set { isRelativeOffset = value; }
        }

        public Feature OffsetRange
        {
            get { return offsetRange; }
            set { offsetRange = value; }
        }


        public float MatchRatio
        {
            get { return matchRatio; }
            set { matchRatio = value; }
        }

        SizeF sheetSize = new SizeF(0, 0);
        public SizeF SheetSize { get => sheetSize; set => sheetSize = value; }

        public InspectParam()
        {
            isRelativeOffset = false;
            offsetRange = new Feature();
            offsetRange.Area = 20;
            offsetRange.Margin = new SizeF(20, 20);
            offsetRange.Blot = new SizeF(20, 20);
        }

        public InspectParam(bool isRelative, Feature offsetRange)
        {
            this.isRelativeOffset = isRelative;
            this.offsetRange = offsetRange;
        }

        public override void Import(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element[subKey];
                Import(subElement);
                return;
            }

            isRelativeOffset = Convert.ToBoolean(XmlHelper.GetValue(element, "IsRelative", "false"));
            matchRatio = Convert.ToSingle(XmlHelper.GetValue(element, "MatchRatio", matchRatio.ToString()));
            offsetRange.Import(element, "OffsetRange");

            //PrintingLengthWarnLevelum = Convert.ToSingle(XmlHelper.GetValue(element, "PrintingLengthWarnLevelum", PrintingLengthWarnLevelum.ToString()));

            InspectionLevelMin = Convert.ToInt32(XmlHelper.GetValue(element, "InspectionLevelMin", InspectionLevelMin.ToString()));
            InspectionLevelMax = Convert.ToInt32(XmlHelper.GetValue(element, "InspectionLevelMax", InspectionLevelMax.ToString()));
            InspectionSizeMax = Convert.ToInt32(XmlHelper.GetValue(element, "InspectionSizeMax", InspectionSizeMax.ToString()));
            InspectionSizeMin = Convert.ToInt32(XmlHelper.GetValue(element, "InspectionSizeMin", InspectionSizeMin.ToString()));
            InspectionRefEdgeLevel = Convert.ToInt32(XmlHelper.GetValue(element, "InspectionRefEdgeLevel", InspectionRefEdgeLevel.ToString()));            
        }

        public override void Export(XmlElement element, string subKey = null)
        {
            if (element == null)
                return;

            if (string.IsNullOrEmpty(subKey) == false)
            {
                XmlElement subElement = element.OwnerDocument.CreateElement(subKey);
                element.AppendChild(subElement);
                Export(subElement);
                return;
            }

            XmlHelper.SetValue(element, "IsRelative", isRelativeOffset.ToString());
            XmlHelper.SetValue(element, "MatchRatio", matchRatio.ToString());
            offsetRange.Export(element, "OffsetRange");

            //XmlHelper.SetValue(element, "PrintingLengthWarnLevelum", PrintingLengthWarnLevelum.ToString());
            XmlHelper.SetValue(element, "InspectionLevelMin", InspectionLevelMin.ToString());
            XmlHelper.SetValue(element, "InspectionLevelMax", InspectionLevelMax.ToString());
            XmlHelper.SetValue(element, "InspectionSizeMax", InspectionSizeMax.ToString());
            XmlHelper.SetValue(element, "InspectionSizeMin", InspectionSizeMin.ToString());
            XmlHelper.SetValue(element, "InspectionRefEdgeLevel", InspectionRefEdgeLevel.ToString());
    
        }

        public static InspectParam Load(XmlElement paramElement, string subKey = null)
        {
            InspectParam param = new InspectParam();
            param.Import(paramElement, subKey);
            return param;
        }
    }
}
