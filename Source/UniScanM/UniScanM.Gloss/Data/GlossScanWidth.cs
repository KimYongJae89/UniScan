using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;

namespace UniScanM.Gloss.Data
{
    public class GlossScanWidth
    {
        #region 생성자
        public GlossScanWidth()
        {
            Name = "Width";
            Start = 0;
            End = 360;
            ValidStart = 30;
            ValidEnd = 330;
        }

        public GlossScanWidth(string name)
        {
            Name = name;
            Start = 0;
            End = 360;
            ValidStart = 30;
            ValidEnd = 330;
        }

        public GlossScanWidth(string name, float start, float end, float validStart, float validEnd)
        {
            Name = name;
            Start = start;
            End = end;
            ValidStart = validStart;
            ValidEnd = validEnd;
        }
        #endregion

        #region 속성
        public string Name { get; set; } = "Width";
        public float Start { get; set; } = 0;
        public float End { get; set; } = 0;
        public float ValidStart { get; set; } = 0;
        public float ValidEnd { get; set; } = 0;
        #endregion

        #region 메서드
        public GlossScanWidth Clone()
        {
            GlossScanWidth scanWidth = new GlossScanWidth();

            scanWidth.CopyFrom(this);

            return scanWidth;
        }

        public void CopyFrom(GlossScanWidth dstScanWidth)
        {
            Name = dstScanWidth.Name;
            Start = dstScanWidth.Start;
            End = dstScanWidth.End;
            ValidStart = dstScanWidth.ValidStart;
            ValidEnd = dstScanWidth.ValidEnd;
        }

        internal void SaveXml(XmlElement xmlElement)
        {
            XmlHelper.SetValue(xmlElement, "Name", Name.ToString());
            XmlHelper.SetValue(xmlElement, "Start", Start.ToString());
            XmlHelper.SetValue(xmlElement, "End", End.ToString());
            XmlHelper.SetValue(xmlElement, "ValidStart", ValidStart.ToString());
            XmlHelper.SetValue(xmlElement, "ValidEnd", ValidEnd.ToString());
        }

        internal void LoadXml(XmlElement xmlElement)
        {
            Name = XmlHelper.GetValue(xmlElement, "Name", "Width");
            Start = float.Parse(XmlHelper.GetValue(xmlElement, "Start", "0"));
            End = float.Parse(XmlHelper.GetValue(xmlElement, "End", "360"));
            ValidStart = float.Parse(XmlHelper.GetValue(xmlElement, "ValidStart", "30"));
            ValidEnd = float.Parse(XmlHelper.GetValue(xmlElement, "ValidEnd", "330"));
        }
        #endregion
    }
}
