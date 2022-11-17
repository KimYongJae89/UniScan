using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanM.CEDMS.Settings
{
    public enum YAxisUnitCEDMS
    {
        Um, Mm
    };


    public class CEDMSSettings : UniScanM.Settings.UniScanMSettings
    {
        private int dataGatheringCountPerSec;
        [LocalizedCategoryAttributeCEDMS("1. Sensor"),
        LocalizedDisplayNameAttributeCEDMS("Data Gathering Count Per Sec"),
        LocalizedDescriptionAttributeCEDMS("Data Gathering Count Per Sec")]
        public int DataGatheringCountPerSec { get => dataGatheringCountPerSec; set => dataGatheringCountPerSec = value; }

        private int dataCountForZeroSetting;
        [LocalizedCategoryAttributeCEDMS("1. Sensor"),
        LocalizedDisplayNameAttributeCEDMS("Data Count For Zero Setting"),
        LocalizedDescriptionAttributeCEDMS("Data Count For Zero Setting")]
        public int DataCountForZeroSetting { get => dataCountForZeroSetting; set => dataCountForZeroSetting = value; }

        private float inFeedOffset;
        [LocalizedCategoryAttributeCEDMS("1. Sensor"),
        LocalizedDisplayNameAttributeCEDMS("InFeed Sensor Offset"),
        LocalizedDescriptionAttributeCEDMS("InFeed Sensor Offset")]
        public float InFeedOffset { get => inFeedOffset; set => inFeedOffset = value; }

        private float outFeedOffset;
        [LocalizedCategoryAttributeCEDMS("1. Sensor"),
        LocalizedDisplayNameAttributeCEDMS("OutFeed Sensor Offset"),
        LocalizedDescriptionAttributeCEDMS("OutFeed Sensor Offset")]
        public float OutFeedOffset { get => outFeedOffset; set => outFeedOffset = value; }

        private bool alarmUse;
        [LocalizedCategoryAttributeCEDMS("1. Sensor"),
        LocalizedDisplayNameAttributeCEDMS("Use Alarm"),
        LocalizedDescriptionAttributeCEDMS("Use Alarm")]
        public bool AlarmUse { get => alarmUse; set => alarmUse = value; }

        private bool inFeedUseLineStop;
        [LocalizedCategoryAttributeCEDMS("2-1. CEDMS InFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Use Line Stop"),
        LocalizedDescriptionAttributeCEDMS("Use Line Stop")]
        public bool InFeedUseLineStop { get => inFeedUseLineStop; set => inFeedUseLineStop = value; }

        private double inFeedLineStopLower;
        [LocalizedCategoryAttributeCEDMS("2-1. CEDMS InFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Line Stop Lower (mm)"),
        LocalizedDescriptionAttributeCEDMS("Line Stop Lower (mm)")]
        public double InFeedLineStopLower { get => inFeedLineStopLower; set => inFeedLineStopLower = value; }

        private double inFeedLineStopUpper;
        [LocalizedCategoryAttributeCEDMS("2-1. CEDMS InFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Line Stop Upper (mm)"),
        LocalizedDescriptionAttributeCEDMS("Line Stop Upper (mm)")]
        public double InFeedLineStopUpper { get => inFeedLineStopUpper; set => inFeedLineStopUpper = value; }

        private bool inFeedUseLineWarning;
        [LocalizedCategoryAttributeCEDMS("2-1. CEDMS InFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Use Line Warning"),
        LocalizedDescriptionAttributeCEDMS("Use Line Warning")]
        public bool InFeedUseLineWarning { get => inFeedUseLineWarning; set => inFeedUseLineWarning = value; }

        private double inFeedLineWarningLower;
        [LocalizedCategoryAttributeCEDMS("2-1. CEDMS InFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Line Warning Lower (mm)"),
        LocalizedDescriptionAttributeCEDMS("Line Warning Lower (mm)")]
        public double InFeedLineWarningLower { get => inFeedLineWarningLower; set => inFeedLineWarningLower = value; }

        private double inFeedLineWarningUpper;
        [LocalizedCategoryAttributeCEDMS("2-1. CEDMS InFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Line Warning Upper (mm)"),
        LocalizedDescriptionAttributeCEDMS("Line Warning Upper (mm)")]
        public double InFeedLineWarningUpper { get => inFeedLineWarningUpper; set => inFeedLineWarningUpper = value; }

        private int inFeedXAxisDisplayLength;
        [LocalizedCategoryAttributeCEDMS("2-1. CEDMS InFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("X Axis Display Length (m)"),
        LocalizedDescriptionAttributeCEDMS("X Axis Display Length (m)")]
        public int InFeedXAxisDisplayLength { get => inFeedXAxisDisplayLength; set => inFeedXAxisDisplayLength = value; }

        private int inFeedXAxisDisplayTotalLength;
        [LocalizedCategoryAttributeCEDMS("2-1. CEDMS InFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("X Axis Display Length Total Graph (m)"),
        LocalizedDescriptionAttributeCEDMS("X Axis Display Length Total Graph (m)")]
        public int InFeedXAxisDisplayTotalLength { get => inFeedXAxisDisplayTotalLength; set => inFeedXAxisDisplayTotalLength = value; }

        private int inFeedXAxisInterval;
        [LocalizedCategoryAttributeCEDMS("2-1. CEDMS InFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("X Axis Interval"),
        LocalizedDescriptionAttributeCEDMS("X Axis Interval")]
        public int InFeedXAxisInterval { get => inFeedXAxisInterval; set => inFeedXAxisInterval = value; }

        private float inFeedYAxisRange;
        [LocalizedCategoryAttributeCEDMS("2-1. CEDMS InFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Y Axis Range (mm)"),
        LocalizedDescriptionAttributeCEDMS("Y Axis Range (mm)")]
        public float InFeedYAxisRange { get => inFeedYAxisRange; set => inFeedYAxisRange = value; }

        YAxisUnitCEDMS inFeedYAxisUnit;
        [LocalizedCategoryAttributeCEDMS("2-1. CEDMS InFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Y Axis Unit"),
        LocalizedDescriptionAttributeCEDMS("Y Axis Unit")]
        public YAxisUnitCEDMS InFeedYAxisUnit { get => inFeedYAxisUnit; set => inFeedYAxisUnit = value; }

        private int inFeedYAxisInterval;
        [LocalizedCategoryAttributeCEDMS("2-1. CEDMS InFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Y Axis Interval"),
        LocalizedDescriptionAttributeCEDMS("Y Axis Interval")]
        public int InFeedYAxisInterval { get => inFeedYAxisInterval; set => inFeedYAxisInterval = value; }

        private bool inFeedUseTotalCenterLine;
        [LocalizedCategoryAttributeCEDMS("2-1. CEDMS InFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Use Total Graph Center Line"),
        LocalizedDescriptionAttributeCEDMS("Use Total Graph Center Line")]
        public bool InFeedUseTotalCenterLine { get => inFeedUseTotalCenterLine; set => inFeedUseTotalCenterLine = value; }

        private bool outFeedUseLineStop;
        [LocalizedCategoryAttributeCEDMS("2-2. CEDMS OutFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Use Line Stop"),
        LocalizedDescriptionAttributeCEDMS("Use Line Stop")]
        public bool OutFeedUseLineStop { get => outFeedUseLineStop; set => outFeedUseLineStop = value; }

        private double outFeedLineStopLower;
        [LocalizedCategoryAttributeCEDMS("2-2. CEDMS OutFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Line Stop Lower (mm)"),
        LocalizedDescriptionAttributeCEDMS("Line Stop Lower (mm)")]
        public double OutFeedLineStopLower { get => outFeedLineStopLower; set => outFeedLineStopLower = value; }

        private double outFeedLineStopUpper;
        [LocalizedCategoryAttributeCEDMS("2-2. CEDMS OutFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Line Stop Upper (mm)"),
        LocalizedDescriptionAttributeCEDMS("Line Stop Upper (mm)")]
        public double OutFeedLineStopUpper { get => outFeedLineStopUpper; set => outFeedLineStopUpper = value; }

        private bool outFeedUseLineWarning;
        [LocalizedCategoryAttributeCEDMS("2-2. CEDMS OutFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Use Line Warning"),
        LocalizedDescriptionAttributeCEDMS("Use Line Warning")]
        public bool OutFeedUseLineWarning { get => outFeedUseLineWarning; set => outFeedUseLineWarning = value; }

        private double outFeedLineWarningLower;
        [LocalizedCategoryAttributeCEDMS("2-2. CEDMS OutFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Line Warning Lower (mm)"),
        LocalizedDescriptionAttributeCEDMS("Line Warning Lower (mm)")]
        public double OutFeedLineWarningLower { get => outFeedLineWarningLower; set => outFeedLineWarningLower = value; }

        private double outFeedLineWarningUpper;
        [LocalizedCategoryAttributeCEDMS("2-2. CEDMS OutFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Line Warning Upper (mm)"),
        LocalizedDescriptionAttributeCEDMS("Line Warning Upper (mm)")]
        public double OutFeedLineWarningUpper { get => outFeedLineWarningUpper; set => outFeedLineWarningUpper = value; }

        private int outFeedXAxisDisplayLength;
        [LocalizedCategoryAttributeCEDMS("2-2. CEDMS OutFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("X Axis Display Length (m)"),
        LocalizedDescriptionAttributeCEDMS("X Axis Display Length (m)")]
        public int OutFeedXAxisDisplayLength { get => outFeedXAxisDisplayLength; set => outFeedXAxisDisplayLength = value; }

        private int outFeedXAxisDisplayTotalLength;
        [LocalizedCategoryAttributeCEDMS("2-2. CEDMS OutFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("X Axis Display Length Total Graph (m)"),
        LocalizedDescriptionAttributeCEDMS("X Axis Display Length Total Graph (m)")]
        public int OutFeedXAxisDisplayTotalLength { get => outFeedXAxisDisplayTotalLength; set => outFeedXAxisDisplayTotalLength = value; }

        private int outFeedXAxisInterval;
        [LocalizedCategoryAttributeCEDMS("2-2. CEDMS OutFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("X Axis Interval"),
        LocalizedDescriptionAttributeCEDMS("X Axis Interval")]
        public int OutFeedXAxisInterval { get => outFeedXAxisInterval; set => outFeedXAxisInterval = value; }

        private float outFeedYAxisRange;
        [LocalizedCategoryAttributeCEDMS("2-2. CEDMS OutFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Y Axis Range (mm)"),
        LocalizedDescriptionAttributeCEDMS("Y Axis Range (mm)")]
        public float OutFeedYAxisRange { get => outFeedYAxisRange; set => outFeedYAxisRange = value; }

        YAxisUnitCEDMS outFeedYAxisUnit;
        [LocalizedCategoryAttributeCEDMS("2-2. CEDMS OutFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Y Axis Unit"),
        LocalizedDescriptionAttributeCEDMS("Y Axis Unit")]
        public YAxisUnitCEDMS OutFeedYAxisUnit { get => outFeedYAxisUnit; set => outFeedYAxisUnit = value; }

        private int outFeedYAxisInterval;
        [LocalizedCategoryAttributeCEDMS("2-2. CEDMS OutFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Y Axis Interval"),
        LocalizedDescriptionAttributeCEDMS("Y Axis Interval")]
        public int OutFeedYAxisInterval { get => outFeedYAxisInterval; set => outFeedYAxisInterval = value; }

        private bool outFeedUseTotalCenterLine;
        [LocalizedCategoryAttributeCEDMS("2-2. CEDMS OutFeed Graph"),
        LocalizedDisplayNameAttributeCEDMS("Use Total Graph Center Line"),
        LocalizedDescriptionAttributeCEDMS("Use Total Graph Center Line")]
        public bool OutFeedUseTotalCenterLine { get => outFeedUseTotalCenterLine; set => outFeedUseTotalCenterLine = value; }

        private Color axisColor;
        [LocalizedCategoryAttributeCEDMS("3. Graph Figure"),
        LocalizedDisplayNameAttributeCEDMS("Axis Color"),
        LocalizedDescriptionAttributeCEDMS("Axis Color")]
        public Color AxisColor { get => axisColor; set => axisColor = value; }

        private Color backColor;
        [LocalizedCategoryAttributeCEDMS("3. Graph Figure"),
        LocalizedDisplayNameAttributeCEDMS("Background Color"),
        LocalizedDescriptionAttributeCEDMS("Background Color")]
        public Color BackColor { get => backColor; set => backColor = value; }

        private Color graphColor;
        [LocalizedCategoryAttributeCEDMS("3. Graph Figure"),
        LocalizedDisplayNameAttributeCEDMS("Graph Color"),
        LocalizedDescriptionAttributeCEDMS("Graph Color")]
        public Color GraphColor { get => graphColor; set => graphColor = value; }

        private int graphThickness;
        [LocalizedCategoryAttributeCEDMS("3. Graph Figure"),
        LocalizedDisplayNameAttributeCEDMS("Graph Thickness"),
        LocalizedDescriptionAttributeCEDMS("Graph Thickness")]
        public int GraphThickness { get => graphThickness; set => graphThickness = value; }

        private Color lineStopColor;
        [LocalizedCategoryAttributeCEDMS("3. Graph Figure"),
        LocalizedDisplayNameAttributeCEDMS("Line Stop Color"),
        LocalizedDescriptionAttributeCEDMS("Line Stop Color")]
        public Color LineStopColor { get => lineStopColor; set => lineStopColor = value; }

        private int lineStopThickness;
        [LocalizedCategoryAttributeCEDMS("3. Graph Figure"),
        LocalizedDisplayNameAttributeCEDMS("Line Stop Thickness"),
        LocalizedDescriptionAttributeCEDMS("Line Stop Thickness")]
        public int LineStopThickness { get => lineStopThickness; set => lineStopThickness = value; }

        private Color lineWarningColor;
        [LocalizedCategoryAttributeCEDMS("3. Graph Figure"),
        LocalizedDisplayNameAttributeCEDMS("Line Warning Color"),
        LocalizedDescriptionAttributeCEDMS("Line Warning Color")]
        public Color LineWarningColor { get => lineWarningColor; set => lineWarningColor = value; }

        private int lineWarningThickness;
        [LocalizedCategoryAttributeCEDMS("3. Graph Figure"),
        LocalizedDisplayNameAttributeCEDMS("Line Warning Thickness"),
        LocalizedDescriptionAttributeCEDMS("Line Warning Thickness")]
        public int LineWarningThickness { get => lineWarningThickness; set => lineWarningThickness = value; }

        private Color lineTotalGraphCenterColor;
        [LocalizedCategoryAttributeCEDMS("3. Graph Figure"),
        LocalizedDisplayNameAttributeCEDMS("Total Graph Center Line Color"),
        LocalizedDescriptionAttributeCEDMS("Total Graph Center Line Color")]
        public Color LineTotalGraphCenterColor { get => lineTotalGraphCenterColor; set => lineTotalGraphCenterColor = value; }

        private int lineTotalGraphCenterThickness;
        [LocalizedCategoryAttributeCEDMS("3. Graph Figure"),
        LocalizedDisplayNameAttributeCEDMS("Total Graph Center Line Thickness"),
        LocalizedDescriptionAttributeCEDMS("Total Graph Center Line Thickness")]
        public int LineTotalGraphCenterThickness { get => lineTotalGraphCenterThickness; set => lineTotalGraphCenterThickness = value; }

        protected CEDMSSettings()
        {
            dataGatheringCountPerSec = 2;
            dataCountForZeroSetting = 10;
            inFeedOffset = 0;
            outFeedOffset = 0;

            alarmUse = false;

            inFeedUseLineStop = true;
            inFeedLineStopUpper = 10.06;
            inFeedLineStopLower = 0.06;

            inFeedUseLineWarning = true;
            inFeedLineWarningUpper = 0.04;
            inFeedLineWarningLower = 0.04;

            inFeedXAxisDisplayLength = 30;
            inFeedXAxisDisplayTotalLength = 90;
            inFeedXAxisInterval = 6;
            inFeedYAxisRange = 0.07f;
            inFeedYAxisInterval = 6;
            inFeedYAxisUnit = YAxisUnitCEDMS.Mm;

            inFeedUseTotalCenterLine = true;

            outFeedUseLineStop = true;
            outFeedLineStopUpper = 10.06;
            outFeedLineStopLower = 0.06;

            outFeedUseLineWarning = true;
            outFeedLineWarningUpper = 0.04;
            outFeedLineWarningLower = 0.04;

            outFeedXAxisDisplayLength = 30;
            outFeedXAxisDisplayTotalLength = 90;
            outFeedXAxisInterval = 6;
            outFeedYAxisRange = 0.07f;
            outFeedYAxisInterval = 6;
            outFeedYAxisUnit = YAxisUnitCEDMS.Mm;

            outFeedUseTotalCenterLine = true;

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

        public static CEDMSSettings Instance()
        {
            return instance as CEDMSSettings;
        }

        public static new void CreateInstance()
        {
            if (instance == null)
                instance = new CEDMSSettings();
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            if (xmlElement == null)
                return;
            
            XmlElement sensorElement = xmlElement.OwnerDocument.CreateElement("", "Sensor", "");
            xmlElement.AppendChild(sensorElement);

            XmlHelper.SetValue(sensorElement, "DataGatheringCountPerSec", dataGatheringCountPerSec.ToString());
            XmlHelper.SetValue(sensorElement, "DataCountForZeroSetting", dataCountForZeroSetting.ToString());
            XmlHelper.SetValue(sensorElement, "InFeedOffset", inFeedOffset.ToString());
            XmlHelper.SetValue(sensorElement, "OutFeedOffset", outFeedOffset.ToString());

            XmlHelper.SetValue(sensorElement, "AlarmUse", alarmUse.ToString());
            
            XmlElement vibrationGraphElement = xmlElement.OwnerDocument.CreateElement("", "CEDMSGraph", "");
            xmlElement.AppendChild(vibrationGraphElement);

            XmlHelper.SetValue(vibrationGraphElement, "InFeedUseLineStop", inFeedUseLineStop.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "InFeedLineStopLower", inFeedLineStopLower.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "InFeedLineStopUpper", inFeedLineStopUpper.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "InFeedUseLineWarning", inFeedUseLineWarning.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "InFeedLineWarningLower", inFeedLineWarningLower.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "InFeedLineWarningUpper", inFeedLineWarningUpper.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "InFeedXAxisDisplayLength", inFeedXAxisDisplayLength.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "InFeedXAxisDisplayTotalLength", inFeedXAxisDisplayTotalLength.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "InFeedXAxisInterval", inFeedXAxisInterval.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "InFeedYAxisRange", inFeedYAxisRange.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "InFeedYAxisInterval", inFeedYAxisInterval.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "InFeedYAxisUnit", inFeedYAxisUnit.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "InFeedUseTotalGraphCenterLine", inFeedUseTotalCenterLine.ToString());

            XmlHelper.SetValue(vibrationGraphElement, "OutFeedUseLineStop", outFeedUseLineStop.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "OutFeedLineStopLower", outFeedLineStopLower.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "OutFeedLineStopUpper", outFeedLineStopUpper.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "OutFeedUseLineWarning", outFeedUseLineWarning.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "OutFeedLineWarningLower", outFeedLineWarningLower.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "OutFeedLineWarningUpper", outFeedLineWarningUpper.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "OutFeedXAxisDisplayLength", outFeedXAxisDisplayLength.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "OutFeedXAxisDisplayTotalLength", outFeedXAxisDisplayTotalLength.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "OutFeedXAxisInterval", outFeedXAxisInterval.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "OutFeedYAxisRange", outFeedYAxisRange.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "OutFeedYAxisInterval", outFeedYAxisInterval.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "OutFeedYAxisUnit", outFeedYAxisUnit.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "OutFeedUseTotalGraphCenterLine", outFeedUseTotalCenterLine.ToString());

            XmlElement graphColorElement = xmlElement.OwnerDocument.CreateElement("", "GraphFigure", "");
            xmlElement.AppendChild(graphColorElement);

            XmlHelper.SetValue(graphColorElement, "AxisColor", axisColor.Name);
            XmlHelper.SetValue(graphColorElement, "BackColor", backColor.Name);
            XmlHelper.SetValue(graphColorElement, "GraphColor", graphColor.Name);
            XmlHelper.SetValue(graphColorElement, "GraphThickness", graphThickness.ToString());
            XmlHelper.SetValue(graphColorElement, "LineStopColor", lineStopColor.Name);
            XmlHelper.SetValue(graphColorElement, "LineStopThickness", lineStopThickness.ToString());

            XmlHelper.SetValue(graphColorElement, "LineWarningColor", lineWarningColor.Name);
            XmlHelper.SetValue(graphColorElement, "LineWarningThickness", lineWarningThickness.ToString());

            XmlHelper.SetValue(graphColorElement, "LineTotalGraphCenterColor", lineTotalGraphCenterColor.Name);
            XmlHelper.SetValue(graphColorElement, "LineTotalGraphCenterThickness", lineTotalGraphCenterThickness.ToString());
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            if (xmlElement == null)
                return;
            
            XmlElement sensorElement = xmlElement["Sensor"];
            if (sensorElement != null)
            {
                dataGatheringCountPerSec = int.Parse(XmlHelper.GetValue(sensorElement, "DataGatheringCountPerSec", "5"));
                dataCountForZeroSetting = int.Parse(XmlHelper.GetValue(sensorElement, "DataCountForZeroSetting", "10"));
                inFeedOffset = float.Parse(XmlHelper.GetValue(sensorElement, "InFeedSideOffset", "0"));
                outFeedOffset = float.Parse(XmlHelper.GetValue(sensorElement, "OutFeedSideOffset", "0"));
                alarmUse = bool.Parse(XmlHelper.GetValue(sensorElement, "AlarmUse", "false"));
            }
            
            XmlElement vibrationGraphElement = xmlElement["CEDMSGraph"];
            if (vibrationGraphElement != null)
            {
                inFeedUseLineStop = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "InFeedUseLineStop", "true"));
                inFeedLineStopLower = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "InFeedLineStopLower", "0.2"));
                inFeedLineStopUpper = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "InFeedLineStopUpper", "0.2"));


                inFeedUseLineWarning = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "InFeedUseLineWarning", "true"));
                inFeedLineWarningLower = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "InFeedLineWarningLower", "0.1"));
                inFeedLineWarningUpper = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "InFeedLineWarningUpper", "0.1"));
                inFeedXAxisDisplayLength = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "InFeedXAxisDisplayLength", "1"));
                inFeedXAxisDisplayTotalLength = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "InFeedXAxisDisplayTotalLength", "60"));
                inFeedXAxisInterval = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "InFeedXAxisInterval", "6"));
                inFeedYAxisRange = float.Parse(XmlHelper.GetValue(vibrationGraphElement, "InFeedYAxisRange", "-1"));
                inFeedYAxisInterval = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "InFeedYAxisInterval", "6"));
                inFeedYAxisUnit = XmlHelper.GetValue(vibrationGraphElement, "InFeedYAxisUnit", YAxisUnitCEDMS.Mm);
                inFeedUseTotalCenterLine = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "InFeedUseTotalGraphCenterLine", "true"));

                outFeedUseLineStop = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "OutFeedUseLineStop", "true"));
                outFeedLineStopLower = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "OutFeedLineStopLower", "0.2"));
                outFeedLineStopUpper = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "OutFeedLineStopUpper", "0.2"));

                outFeedUseLineWarning = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "OutFeedUseLineWarning", "true"));
                outFeedLineWarningLower = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "OutFeedLineWarningLower", "0.1"));
                outFeedLineWarningUpper = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "OutFeedLineWarningUpper", "0.1"));
                outFeedXAxisDisplayLength = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "OutFeedXAxisDisplayLength", "1"));
                outFeedXAxisDisplayTotalLength = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "OutFeedXAxisDisplayTotalLength", "60"));
                outFeedXAxisInterval = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "OutFeedXAxisInterval", "6"));
                outFeedYAxisRange = float.Parse(XmlHelper.GetValue(vibrationGraphElement, "OutFeedYAxisRange", "-1"));
                outFeedYAxisInterval = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "OutFeedYAxisInterval", "6"));
                outFeedYAxisUnit = XmlHelper.GetValue(vibrationGraphElement, "OutFeedYAxisUnit", YAxisUnitCEDMS.Mm);
                outFeedUseTotalCenterLine = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "OutFeedUseTotalGraphCenterLine", "true"));
            }
            
            XmlElement graphColorElement = xmlElement["GraphFigure"];
            if (graphColorElement != null)
            {
                ColorConverter converter = new ColorConverter();

                axisColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "AxisColor", "DarkGray"));
                backColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "BackColor", "White"));
                graphColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "GraphColor", "Black"));
                graphThickness = int.Parse(XmlHelper.GetValue(graphColorElement, "GraphThickness", "3"));
                lineStopColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "LineStopColor", "Red"));
                lineStopThickness = int.Parse(XmlHelper.GetValue(graphColorElement, "LineStopThickness", "2"));
                lineWarningColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "LineWarningColor", "Red"));
                lineWarningThickness = int.Parse(XmlHelper.GetValue(graphColorElement, "LineWarningThickness", "2"));
                lineTotalGraphCenterColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "LineTotalGraphCenterColor", "Gold"));
                lineTotalGraphCenterThickness = int.Parse(XmlHelper.GetValue(graphColorElement, "LineTotalGraphCenterThickness", "2"));
            }
        }
    }
}
