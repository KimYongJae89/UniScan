using System;
using System.Drawing;
using System.Xml;
using UniScanM.Gloss.Data;

namespace UniScanM.Gloss.Settings
{
    public class ChartSetting
    {
        #region 생성자
        public ChartSetting() { }
        #endregion

        #region 속성
        public Color AxisColor { get; set; } = Color.DarkGray;

        public Color BackColor { get; set; } = Color.Black;

        public Color GraphColor { get; set; } = Color.Chartreuse;

        public Color SubGraphColor { get; set; } = Color.Chartreuse;

        public int GraphThickness { get; set; } = 3;

        public Color LineStopColor { get; set; } = Color.Red;

        public int LineStopThickness { get; set; } = 2;

        public Color LineWarningColor { get; set; } = Color.Yellow;

        public int LineWarningThickness { get; set; } = 2;

        public Color LineCenterColor { get; set; } = Color.Gold;

        public int LineCenterThickness { get; set; } = 2;

        public bool UseTotalCenterLine { get; set; }

        public bool UseLineStop { get; set; } = true;

        public double LineStopRange { get; set; } = 0;
        
        public bool UseLineWarning { get; set; } = true;

        public double LineWarningRange { get; set; } = 0;
        
        public int XAxisInterval { get; set; } = 0;

        public int YAxisInterval { get; set; } = 0;

        public float YAxisRange { get; set; } = 0;

        public int OverlayCount { get; set; }

        public GlossScanWidth GlossScanWidth { get; set; }
        #endregion

        #region 메서드
        public ChartSetting Clone()
        {
            ChartSetting chartSetting = new ChartSetting();

            chartSetting.UseLineStop = this.UseLineStop;
            chartSetting.LineStopRange = this.LineStopRange;
            chartSetting.UseLineWarning = this.UseLineWarning;
            chartSetting.LineWarningRange = this.LineWarningRange;
            chartSetting.UseTotalCenterLine = this.UseTotalCenterLine;

            chartSetting.XAxisInterval = this.XAxisInterval;
            chartSetting.YAxisRange = this.YAxisRange;
            chartSetting.YAxisInterval = this.YAxisInterval;

            chartSetting.AxisColor = this.AxisColor;
            chartSetting.BackColor = this.BackColor;
            chartSetting.GraphColor = this.GraphColor;
            chartSetting.SubGraphColor = this.SubGraphColor;
            chartSetting.GraphThickness = this.GraphThickness;
            chartSetting.LineStopColor = this.LineStopColor;
            chartSetting.LineStopThickness = this.LineStopThickness;
            chartSetting.LineWarningColor = this.LineWarningColor;
            chartSetting.LineWarningThickness = this.LineWarningThickness;
            chartSetting.LineCenterColor = this.LineCenterColor;
            chartSetting.LineCenterThickness = this.LineCenterThickness;

            chartSetting.OverlayCount = this.OverlayCount;

            chartSetting.GlossScanWidth = this.GlossScanWidth.Clone();

            return chartSetting;
        }

        public void Save(XmlElement xmlElement)
        {
            xmlElement.SetAttribute("UseLineStop", UseLineStop.ToString());
            xmlElement.SetAttribute("LineStopRange", LineStopRange.ToString());
            xmlElement.SetAttribute("UseLineWarning", UseLineWarning.ToString());
            xmlElement.SetAttribute("LineWarningRange", LineWarningRange.ToString());
            xmlElement.SetAttribute("XAxisInterval", XAxisInterval.ToString());
            xmlElement.SetAttribute("YAxisRange", YAxisRange.ToString());
            xmlElement.SetAttribute("YAxisInterval", YAxisInterval.ToString());
            xmlElement.SetAttribute("UseTotalCenterLine", UseTotalCenterLine.ToString());

            xmlElement.SetAttribute("AxisColor", AxisColor.ToString());
            xmlElement.SetAttribute("BackColor", BackColor.ToString());
            xmlElement.SetAttribute("GraphColor", GraphColor.ToString());
            xmlElement.SetAttribute("SubGraphColor", SubGraphColor.ToString());
            xmlElement.SetAttribute("GraphThickness", GraphThickness.ToString());
            xmlElement.SetAttribute("LineStopColor", LineStopColor.ToString());
            xmlElement.SetAttribute("LineStopThickness", LineStopThickness.ToString());
            xmlElement.SetAttribute("LineWarningColor", LineWarningColor.ToString());
            xmlElement.SetAttribute("LineWarningThickness", LineWarningThickness.ToString());
            xmlElement.SetAttribute("LineCenterColor", LineCenterColor.ToString());
            xmlElement.SetAttribute("LineCenterThickness", LineCenterThickness.ToString());

            xmlElement.SetAttribute("OverlayCount", OverlayCount.ToString());

            GlossScanWidth.SaveXml(xmlElement);
        }

        public void Load(XmlElement xmlElement)
        {
            UseLineStop = Convert.ToBoolean(xmlElement.GetAttribute("UseLineStop"));
            LineStopRange = Convert.ToSingle(xmlElement.GetAttribute("LineStopRange"));
            UseLineWarning = Convert.ToBoolean(xmlElement.GetAttribute("UseLineWarning"));
            LineWarningRange = Convert.ToSingle(xmlElement.GetAttribute("LineWarningRange"));
            XAxisInterval = Convert.ToInt32(xmlElement.GetAttribute("XAxisInterval"));
            YAxisRange = Convert.ToSingle(xmlElement.GetAttribute("YAxisRange"));
            YAxisInterval = Convert.ToInt32(xmlElement.GetAttribute("YAxisInterval"));
            UseTotalCenterLine = Convert.ToBoolean(xmlElement.GetAttribute("UseTotalCenterLine"));

            AxisColor = Color.FromName(xmlElement.GetAttribute("AxisColor"));
            BackColor = Color.FromName(xmlElement.GetAttribute("BackColor"));
            GraphColor = Color.FromName(xmlElement.GetAttribute("GraphColor"));
            SubGraphColor = Color.FromName(xmlElement.GetAttribute("SubGraphColor"));
            GraphThickness = int.Parse(xmlElement.GetAttribute("GraphThickness"));
            LineStopColor = Color.FromName(xmlElement.GetAttribute("LineStopColor"));
            LineStopThickness = int.Parse(xmlElement.GetAttribute("LineStopThickness"));
            LineWarningColor = Color.FromName(xmlElement.GetAttribute("LineWarningColor"));
            LineWarningThickness = int.Parse(xmlElement.GetAttribute("LineWarningThickness"));
            LineCenterColor = Color.FromName(xmlElement.GetAttribute("LineCenterColor"));
            LineCenterThickness = int.Parse(xmlElement.GetAttribute("LineCenterThickness"));

            OverlayCount = int.Parse(xmlElement.GetAttribute("OverlayCount"));

            GlossScanWidth.LoadXml(xmlElement);
        }
        #endregion
    }
}
