using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniScanM.CEDMS.Settings
{
    public class ChartSetting
    {
        private bool useLineStop;
        private double lineStopLower;
        private double lineStopUpper;
        private bool useLineWarning;
        private double lineWarningLower;
        private double lineWarningUpper;
        private int xAxisDisplayLength;
        private int xAxisDisplayTotalLength;
        private int xAxisInterval;
        private float yAxisRange;
        YAxisUnitCEDMS yAxisUnit;
        private int yAxisInterval;
        private bool useTotalCenterLine;

        private Color axisColor;
        private Color backColor;
        private Color graphColor;
        private int graphThickness;
        private Color lineStopColor;
        private int lineStopThickness;
        private Color lineWarningColor;
        private int lineWarningThickness;
        private Color lineTotalGraphCenterColor;
        private int lineTotalGraphCenterThickness;

        public Color AxisColor { get => axisColor; set => axisColor = value; }
        public Color BackColor { get => backColor; set => backColor = value; }
        public Color GraphColor { get => graphColor; set => graphColor = value; }
        public int GraphThickness { get => graphThickness; set => graphThickness = value; }
        public Color LineStopColor { get => lineStopColor; set => lineStopColor = value; }
        public int LineStopThickness { get => lineStopThickness; set => lineStopThickness = value; }
        public Color LineWarningColor { get => lineWarningColor; set => lineWarningColor = value; }
        public int LineWarningThickness { get => lineWarningThickness; set => lineWarningThickness = value; }
        public Color LineTotalGraphCenterColor { get => lineTotalGraphCenterColor; set => lineTotalGraphCenterColor = value; }
        public int LineTotalGraphCenterThickness { get => lineTotalGraphCenterThickness; set => lineTotalGraphCenterThickness = value; }

        public bool UseLineStop { get => useLineStop; set => useLineStop = value; }
        public double LineStopLower { get => lineStopLower; set => lineStopLower = value; }
        public double LineStopUpper { get => lineStopUpper; set => lineStopUpper = value; }
        public bool UseLineWarning { get => useLineWarning; set => useLineWarning = value; }
        public double LineWarningLower { get => lineWarningLower; set => lineWarningLower = value; }
        public double LineWarningUpper { get => lineWarningUpper; set => lineWarningUpper = value; }
        public int XAxisDisplayLength { get => xAxisDisplayLength; set => xAxisDisplayLength = value; }
        public int XAxisDisplayTotalLength { get => xAxisDisplayTotalLength; set => xAxisDisplayTotalLength = value; }
        public int XAxisInterval { get => xAxisInterval; set => xAxisInterval = value; }
        public float YAxisRange { get => yAxisRange; set => yAxisRange = value; }
        public YAxisUnitCEDMS YAxisUnit { get => yAxisUnit; set => yAxisUnit = value; }
        public int YAxisInterval { get => yAxisInterval; set => yAxisInterval = value; }
        public bool UseTotalCenterLine { get => useTotalCenterLine; set => useTotalCenterLine = value; }

        public ChartSetting()
        {
            useLineStop = true;
            lineStopLower = 0;
            lineStopUpper = 0;
            useLineWarning = true;
            lineWarningLower = 0;
            lineWarningUpper = 0;
            xAxisDisplayLength = 0;
            xAxisDisplayTotalLength = 0;
            xAxisInterval = 0;
            yAxisRange = 0;
            yAxisUnit = 0;
            yAxisInterval = 0;
            useTotalCenterLine = true;

            axisColor = Color.DarkGray;
            backColor = Color.Black;
            graphColor = Color.Chartreuse;
            graphThickness = 3;
            lineStopColor = Color.Red;
            lineStopThickness = 2;

            lineWarningColor = Color.Yellow;
            lineWarningThickness = 2;

            lineTotalGraphCenterColor = Color.Gold;
            lineTotalGraphCenterThickness = 2;
        }

        public ChartSetting Clone()
        {
            ChartSetting chartSetting = new ChartSetting();

            chartSetting.useLineStop = this.useLineStop;
            chartSetting.lineStopLower = this.lineStopLower;
            chartSetting.lineStopUpper = this.lineStopUpper;
            chartSetting.useLineWarning = this.useLineWarning;
            chartSetting.lineWarningLower = this.lineWarningLower;
            chartSetting.lineWarningUpper = this.lineWarningUpper;
            chartSetting.xAxisDisplayLength = this.xAxisDisplayLength;
            chartSetting.xAxisDisplayTotalLength = this.xAxisDisplayTotalLength;
            chartSetting.xAxisInterval = this.xAxisInterval;
            chartSetting.yAxisRange = this.yAxisRange;
            chartSetting.yAxisUnit = this.yAxisUnit;
            chartSetting.yAxisInterval = this.yAxisInterval;
            chartSetting.useTotalCenterLine = this.useTotalCenterLine;

            chartSetting.axisColor = this.axisColor;
            chartSetting.backColor = this.backColor;
            chartSetting.graphColor = this.graphColor;
            chartSetting.graphThickness = this.graphThickness;
            chartSetting.lineStopColor = this.lineStopColor;
            chartSetting.lineStopThickness = this.lineStopThickness;
            chartSetting.lineWarningColor = this.lineWarningColor;
            chartSetting.lineWarningThickness = this.lineWarningThickness;
            chartSetting.lineTotalGraphCenterColor = this.lineTotalGraphCenterColor;
            chartSetting.lineTotalGraphCenterThickness = this.lineTotalGraphCenterThickness;

            return chartSetting;
        }

        public void Save(XmlElement xmlElement)
        {
            xmlElement.SetAttribute("UseLineStop", useLineStop.ToString());
            xmlElement.SetAttribute("LineStopLower", lineStopLower.ToString());
            xmlElement.SetAttribute("LineStopUpper", lineStopUpper.ToString());
            xmlElement.SetAttribute("UseLineWarning", useLineWarning.ToString());
            xmlElement.SetAttribute("LineWarningLower", lineWarningLower.ToString());
            xmlElement.SetAttribute("LineWarningUpper", lineWarningUpper.ToString());
            xmlElement.SetAttribute("XAxisDisplayLength", xAxisDisplayLength.ToString());
            xmlElement.SetAttribute("XAxisDisplayTotalLength", xAxisDisplayTotalLength.ToString());
            xmlElement.SetAttribute("XAxisInterval", xAxisInterval.ToString());
            xmlElement.SetAttribute("YAxisRange", yAxisRange.ToString());
            xmlElement.SetAttribute("YAxisUnit", yAxisUnit.ToString());
            xmlElement.SetAttribute("YAxisInterval", yAxisInterval.ToString());
            xmlElement.SetAttribute("UseTotalCenterLine", useTotalCenterLine.ToString());

            xmlElement.SetAttribute("AxisColor", axisColor.ToString());
            xmlElement.SetAttribute("BackColor", backColor.ToString());
            xmlElement.SetAttribute("GraphColor", graphColor.ToString());
            xmlElement.SetAttribute("GraphThickness", graphThickness.ToString());
            xmlElement.SetAttribute("LineStopColor", lineStopColor.ToString());
            xmlElement.SetAttribute("LineStopThickness", lineStopThickness.ToString());
            xmlElement.SetAttribute("LineWarningColor", lineWarningColor.ToString());
            xmlElement.SetAttribute("LineWarningThickness", lineWarningThickness.ToString());
            xmlElement.SetAttribute("LineTotalGraphCenterColor", lineTotalGraphCenterColor.ToString());
            xmlElement.SetAttribute("LineTotalGraphCenterThickness", lineTotalGraphCenterThickness.ToString());
        }

        public void Load(XmlElement xmlElement)
        {
            useLineStop = Convert.ToBoolean(xmlElement.GetAttribute("UseLineStop"));
            lineStopLower = Convert.ToSingle(xmlElement.GetAttribute("LineStopLower"));
            lineStopUpper = Convert.ToSingle(xmlElement.GetAttribute("LineStopUpper"));
            useLineWarning = Convert.ToBoolean(xmlElement.GetAttribute("UseLineWarning"));
            lineWarningLower = Convert.ToSingle(xmlElement.GetAttribute("LineWarningLower"));
            lineWarningUpper = Convert.ToSingle(xmlElement.GetAttribute("LineWarningUpper"));
            xAxisDisplayLength = Convert.ToInt32(xmlElement.GetAttribute("XAxisDisplayLength"));
            xAxisDisplayTotalLength = Convert.ToInt32(xmlElement.GetAttribute("XAxisDisplayTotalLength"));
            xAxisInterval = Convert.ToInt32(xmlElement.GetAttribute("XAxisInterval"));
            yAxisRange = Convert.ToSingle(xmlElement.GetAttribute("YAxisRange"));
            yAxisUnit = (YAxisUnitCEDMS)Enum.Parse(typeof(YAxisUnitCEDMS), xmlElement.GetAttribute("YAxisUnit"));
            yAxisInterval = Convert.ToInt32(xmlElement.GetAttribute("YAxisInterval"));
            useTotalCenterLine = Convert.ToBoolean(xmlElement.GetAttribute("UseTotalCenterLine"));

            axisColor = Color.FromName(xmlElement.GetAttribute("AxisColor"));
            backColor = Color.FromName(xmlElement.GetAttribute("BackColor"));
            graphColor = Color.FromName(xmlElement.GetAttribute("GraphColor"));
            graphThickness = int.Parse(xmlElement.GetAttribute("GraphThickness"));
            lineStopColor = Color.FromName(xmlElement.GetAttribute("LineStopColor"));
            lineStopThickness = int.Parse(xmlElement.GetAttribute("LineStopThickness"));
            lineWarningColor = Color.FromName(xmlElement.GetAttribute("LineWarningColor"));
            lineWarningThickness = int.Parse(xmlElement.GetAttribute("LineWarningThickness"));
            lineTotalGraphCenterColor = Color.FromName(xmlElement.GetAttribute("LineTotalGraphCenterColor"));
            lineTotalGraphCenterThickness = int.Parse(xmlElement.GetAttribute("LineTotalGraphCenterThickness"));
        }
    }
}
