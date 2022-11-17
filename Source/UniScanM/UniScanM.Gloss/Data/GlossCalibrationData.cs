using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;

namespace UniScanM.Gloss.Data
{
    public class GlossCalibrationData
    {
        #region 생성자
        public GlossCalibrationData()
        {
            Name = "Param";
        }

        public GlossCalibrationData(string name)
        {
            Name = name;
        }

        public GlossCalibrationData(string name, float c3, float c2, float c1, float c0)
        {
            Name = name;
            C3 = c3;
            C2 = c2;
            C1 = c1;
            C0 = c0;
        }
        #endregion

        #region 속성
        public string Name { get; set; } = "Param";
        public float C3 { get; set; } = 0;
        public float C2 { get; set; } = 0;
        public float C1 { get; set; } = 1;
        public float C0 { get; set; } = 0;
        #endregion

        #region 메서드
        public GlossCalibrationData Clone()
        {
            GlossCalibrationData scanWidth = new GlossCalibrationData();

            scanWidth.CopyFrom(this);

            return scanWidth;
        }

        public void CopyFrom(GlossCalibrationData dstScanWidth)
        {
            Name = dstScanWidth.Name;
            C3 = dstScanWidth.C3;
            C2 = dstScanWidth.C2;
            C1 = dstScanWidth.C1;
            C0 = dstScanWidth.C0;
        }

        internal void SaveXml(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Name", Name.ToString());
            XmlHelper.SetValue(xmlElement, "C3", C3.ToString());
            XmlHelper.SetValue(xmlElement, "C2", C2.ToString());
            XmlHelper.SetValue(xmlElement, "C1", C1.ToString());
            XmlHelper.SetValue(xmlElement, "C0", C0.ToString());
        }

        internal void LoadXml(XmlElement xmlElement)
        {
            Name = XmlHelper.GetValue(xmlElement, "Name", "Param");
            C3 = float.Parse(XmlHelper.GetValue(xmlElement, "C3", "0"));
            C2 = float.Parse(XmlHelper.GetValue(xmlElement, "C2", "0"));
            C1 = float.Parse(XmlHelper.GetValue(xmlElement, "C1", "1"));
            C0 = float.Parse(XmlHelper.GetValue(xmlElement, "C0", "0"));
        }
        #endregion
    }
}