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

namespace UniScanM.RVMS.Settings
{
        public enum YAxisUnitRVMS { Um,Mm};
    public class RVMSSettings : UniScanM.Settings.UniScanMSettings
    {
        private int dataGatheringCountPerSec;
        [LocalizedCategoryAttributeRVMS("1. Sensor"),
        LocalizedDisplayNameAttributeRVMS("Data Gathering Count Per Sec"),
        LocalizedDescriptionAttributeRVMS("Data Gathering Count Per Sec")]
        public int DataGatheringCountPerSec { get => dataGatheringCountPerSec; set => dataGatheringCountPerSec = value; }

        private int dataCountForZeroSetting;
        [LocalizedCategoryAttributeRVMS("1. Sensor"),
        LocalizedDisplayNameAttributeRVMS("Data Count For Zero Setting"),
        LocalizedDescriptionAttributeRVMS("Data Count For Zero Setting")]
        public int DataCountForZeroSetting { get => dataCountForZeroSetting; set => dataCountForZeroSetting = value; }

        private float gearSideOffset;
        [LocalizedCategoryAttributeRVMS("1. Sensor"),
        LocalizedDisplayNameAttributeRVMS("Gear Side Sensor Offset"),
        LocalizedDescriptionAttributeRVMS("Gear Side Sensor Offset")]
        public float GearSideOffset { get => gearSideOffset; set => gearSideOffset = value; }

        private float manSideOffset;
        [LocalizedCategoryAttributeRVMS("1. Sensor"),
        LocalizedDisplayNameAttributeRVMS("Man Side Sensor Offset"),
        LocalizedDescriptionAttributeRVMS("Man Side Sensor Offset")]
        public float ManSideOffset { get => manSideOffset; set => manSideOffset = value; }

        private bool useLineStop;
        [LocalizedCategoryAttributeRVMS("2. Vibration Graph"),
        LocalizedDisplayNameAttributeRVMS("Use Line Stop"),
        LocalizedDescriptionAttributeRVMS("Use Line Stop")]
        public bool UseLineStop { get => useLineStop; set => useLineStop = value; }

        private double lineStopLower;
        [LocalizedCategoryAttributeRVMS("2. Vibration Graph"),
        LocalizedDisplayNameAttributeRVMS("Line Stop Lower (mm)"),
        LocalizedDescriptionAttributeRVMS("Line Stop Lower (mm)")]
        public double LineStopLower { get => lineStopLower; set => lineStopLower = value; }

        private double lineStopUpper;
        [LocalizedCategoryAttributeRVMS("2. Vibration Graph"),
        LocalizedDisplayNameAttributeRVMS("Line Stop Upper (mm)"),
        LocalizedDescriptionAttributeRVMS("Line Stop Upper (mm)")]
        public double LineStopUpper { get => lineStopUpper; set => lineStopUpper = value; }

        private bool useLineWarning;
        [LocalizedCategoryAttributeRVMS("2. Vibration Graph"),
        LocalizedDisplayNameAttributeRVMS("Use Line Warning"),
        LocalizedDescriptionAttributeRVMS("Use Line Warning")]
        public bool UseLineWarning { get => useLineWarning; set => useLineWarning = value; }

        private double lineWarningLower;
        [LocalizedCategoryAttributeRVMS("2. Vibration Graph"),
        LocalizedDisplayNameAttributeRVMS("Line Warning Lower (mm)"),
        LocalizedDescriptionAttributeRVMS("Line Warning Lower (mm)")]
        public double LineWarningLower { get => lineWarningLower; set => lineWarningLower = value; }

        private double lineWarningUpper;
        [LocalizedCategoryAttributeRVMS("2. Vibration Graph"),
        LocalizedDisplayNameAttributeRVMS("Line Warning Upper (mm)"),
        LocalizedDescriptionAttributeRVMS("Line Warning Upper (mm)")]
        public double LineWarningUpper { get => lineWarningUpper; set => lineWarningUpper = value; }

        private int xAxisDisplayTime;
        [LocalizedCategoryAttributeRVMS("2. Vibration Graph"),
        LocalizedDisplayNameAttributeRVMS("X Axis Display Time (sec)"),
        LocalizedDescriptionAttributeRVMS("X Axis Display Time (sec)")]
        public int XAxisDisplayTime { get => xAxisDisplayTime; set => xAxisDisplayTime = value; }

        private int xAxisDisplayTimeTotalGraph;
        [LocalizedCategoryAttributeRVMS("2. Vibration Graph"),
        LocalizedDisplayNameAttributeRVMS("X Axis Display Time Total Graph (min)"),
        LocalizedDescriptionAttributeRVMS("X Axis Display Time Total Graph (min)")]
        public int XAxisDisplayTimeTotalGraph { get => xAxisDisplayTimeTotalGraph; set => xAxisDisplayTimeTotalGraph = value; }

        private int xAxisInterval;
        [LocalizedCategoryAttributeRVMS("2. Vibration Graph"),
        LocalizedDisplayNameAttributeRVMS("X Axis Interval"),
        LocalizedDescriptionAttributeRVMS("X Axis Interval")]
        public int XAxisInterval { get => xAxisInterval; set => xAxisInterval = value; }

        private float yAxisRange;
        [LocalizedCategoryAttributeRVMS("2. Vibration Graph"),
         LocalizedDisplayNameAttributeRVMS("Y Axis Range (mm)"),
        LocalizedDescriptionAttributeRVMS("Y Axis Range (mm)")]
        public float YAxisRange { get => yAxisRange; set => yAxisRange = value; }

        YAxisUnitRVMS yAxisUnit;
        [LocalizedCategoryAttributeRVMS("2. Vibration Graph"),
            LocalizedDisplayNameAttributeRVMS("Y Axis Unit"),
            LocalizedDescriptionAttributeRVMS("Y Axis Unit")]
        public YAxisUnitRVMS YAxisUnit { get => yAxisUnit; set => yAxisUnit = value; }

        private int yAxisInterval;
        [LocalizedCategoryAttributeRVMS("2. Vibration Graph"),
        LocalizedDisplayNameAttributeRVMS("Y Axis Interval"),
        LocalizedDescriptionAttributeRVMS("Y Axis Interval")]
        public int YAxisInterval { get => yAxisInterval; set => yAxisInterval = value; }

        private bool useTotalCenterLine;
        [LocalizedCategoryAttributeRVMS("2. Vibration Graph"),
        LocalizedDisplayNameAttributeRVMS("Use Total Graph Center Line"),
        LocalizedDescriptionAttributeRVMS("Use Total Graph Center Line")]
        public bool UseTotalCenterLine { get => useTotalCenterLine; set => useTotalCenterLine = value; }

        private bool useLineStopPL;
        [LocalizedCategoryAttributeRVMS("3. Pattern Length Graph"),
        LocalizedDisplayNameAttributeRVMS("Use Line Stop"),
        LocalizedDescriptionAttributeRVMS("Use Line Stop")]
        public bool UseLineStopPL { get => useLineStopPL; set => useLineStopPL = value; }

        private double lineStopPL;
        [LocalizedCategoryAttributeRVMS("3. Pattern Length Graph"),
        LocalizedDisplayNameAttributeRVMS("Line Stop (mm)"),
        LocalizedDescriptionAttributeRVMS("Line Stop (mm)")]
        public double LineStopPL { get => lineStopPL; set => lineStopPL = value; }

        private int xAxisDisplayTimePL;
        [LocalizedCategoryAttributeRVMS("3. Pattern Length Graph"),
        LocalizedDisplayNameAttributeRVMS("X Axis Display Time (sec)"),
        LocalizedDescriptionAttributeRVMS("X Axis Display Time (sec)")]
        public int XAxisDisplayTimePL { get => xAxisDisplayTimePL; set => xAxisDisplayTimePL = value; }

        private int xAxisIntervalPL;
        [LocalizedCategoryAttributeRVMS("3. Pattern Length Graph"),
        LocalizedDisplayNameAttributeRVMS("X Axis Interval"),
        LocalizedDescriptionAttributeRVMS("X Axis Interval")]
        public int XAxisIntervalPL { get => xAxisIntervalPL; set => xAxisIntervalPL = value; }

        private float yAxisRangePL;
        [LocalizedCategoryAttributeRVMS("3. Pattern Length Graph"),
        LocalizedDisplayNameAttributeRVMS("Y Axis Range (mm)"),
        LocalizedDescriptionAttributeRVMS("Y Axis Range (mm)")]
        public float YAxisRangePL { get => yAxisRangePL; set => yAxisRangePL = value; }

        private int yAxisIntervalPL;
        [LocalizedCategoryAttributeRVMS("3. Pattern Length Graph"),
        LocalizedDisplayNameAttributeRVMS("Y Axis Interval"),
        LocalizedDescriptionAttributeRVMS("Y Axis Interval")]
        public int YAxisIntervalPL { get => yAxisIntervalPL; set => yAxisIntervalPL = value; }

        YAxisUnitRVMS yAxisUnitPL;
        [LocalizedCategoryAttributeRVMS("3. Pattern Length Graph"),
            LocalizedDisplayNameAttributeRVMS("Y Axis Unit"),
            LocalizedDescriptionAttributeRVMS("Y Axis Unit")]
        public YAxisUnitRVMS YAxisUnitPL { get => yAxisUnitPL; set => yAxisUnitPL = value; }

        bool visibilityPlGraph;
        [LocalizedCategoryAttributeRVMS("3. Pattern Length Graph"),
            LocalizedDisplayNameAttributeRVMS("Visibility"),
            LocalizedDescriptionAttributeRVMS("Visibility")]
        public bool VisibilityPlGraph { get => visibilityPlGraph; set => visibilityPlGraph = value; }

        private uint decimalpointPl=2;
        [LocalizedCategoryAttributeRVMS("3. Pattern Length Graph"),
            LocalizedDisplayNameAttributeRVMS("Decimal Point"),
            LocalizedDescriptionAttributeRVMS("Decimal Point")]
        public uint DecimalpointPl { get => decimalpointPl; set => decimalpointPl = value; }



        private Color axisColor;
        [LocalizedCategoryAttributeRVMS("4. Graph"),
        LocalizedDisplayNameAttributeRVMS("Axis Color"),
        LocalizedDescriptionAttributeRVMS("Axis Color")]
        public Color AxisColor { get => axisColor; set => axisColor = value; }

        private Color backColor;
        [LocalizedCategoryAttributeRVMS("4. Graph"),
        LocalizedDisplayNameAttributeRVMS("Background Color"),
        LocalizedDescriptionAttributeRVMS("Background Color")]
        public Color BackColor { get => backColor; set => backColor = value; }

        private Color graphColor;
        [LocalizedCategoryAttributeRVMS("4. Graph"),
        LocalizedDisplayNameAttributeRVMS("Graph Color"),
        LocalizedDescriptionAttributeRVMS("Graph Color")]
        public Color GraphColor { get => graphColor; set => graphColor = value; }

        private int graphThickness;
        [LocalizedCategoryAttributeRVMS("4. Graph"),
        LocalizedDisplayNameAttributeRVMS("Graph Thickness"),
        LocalizedDescriptionAttributeRVMS("Graph Thickness")]
        public int GraphThickness { get => graphThickness; set => graphThickness = value; }

        private Color lineStopColor;
        [LocalizedCategoryAttributeRVMS("4. Graph"),
        LocalizedDisplayNameAttributeRVMS("Line Stop Color"),
        LocalizedDescriptionAttributeRVMS("Line Stop Color")]
        public Color LineStopColor { get => lineStopColor; set => lineStopColor = value; }

        private int lineStopThickness;
        [LocalizedCategoryAttributeRVMS("4. Graph"),
        LocalizedDisplayNameAttributeRVMS("Line Stop Thickness"),
        LocalizedDescriptionAttributeRVMS("Line Stop Thickness")]
        public int LineStopThickness { get => lineStopThickness; set => lineStopThickness = value; }

        private Color lineWarningColor;
        [LocalizedCategoryAttributeRVMS("4. Graph"),
        LocalizedDisplayNameAttributeRVMS("Line Warning Color"),
        LocalizedDescriptionAttributeRVMS("Line Warning Color")]
        public Color LineWarningColor { get => lineWarningColor; set => lineWarningColor = value; }

        private int lineWarningThickness;
        [LocalizedCategoryAttributeRVMS("4. Graph"),
        LocalizedDisplayNameAttributeRVMS("Line Warning Thickness"),
        LocalizedDescriptionAttributeRVMS("Line Warning Thickness")]
        public int LineWarningThickness { get => lineWarningThickness; set => lineWarningThickness = value; }

        private Color lineTotalGraphCenterColor;
        [LocalizedCategoryAttributeRVMS("4. Graph"),
        LocalizedDisplayNameAttributeRVMS("Total Graph Center Line Color"),
        LocalizedDescriptionAttributeRVMS("Total Graph Center Line Color")]
        public Color LineTotalGraphCenterColor { get => lineTotalGraphCenterColor; set => lineTotalGraphCenterColor = value; }

        private int lineTotalGraphCenterThickness;
        [LocalizedCategoryAttributeRVMS("4. Graph"),
        LocalizedDisplayNameAttributeRVMS("Total Graph Center Line Thickness"),
        LocalizedDescriptionAttributeRVMS("Total Graph Center Line Thickness")]
        public int LineTotalGraphCenterThickness { get => lineTotalGraphCenterThickness; set => lineTotalGraphCenterThickness = value; }

        protected RVMSSettings()
        {
            dataGatheringCountPerSec = 2;
            dataCountForZeroSetting = 10;
            gearSideOffset = 0;
            manSideOffset = 0;

            useLineStop = true;
            lineStopUpper = 0.06;
            lineStopLower = 0.06;

            useLineWarning = true;
            lineWarningUpper = 0.04;
            lineWarningLower = 0.04;

            xAxisDisplayTime = 30;
            xAxisDisplayTimeTotalGraph = 90;
            xAxisInterval = 6;
            yAxisRange = 0.07f;
            yAxisInterval = 6;
            yAxisUnit = YAxisUnitRVMS.Mm;

            useTotalCenterLine = true;

            useLineStopPL = false;
            lineStopPL = 0.7;
            xAxisDisplayTimePL = 30;
            xAxisIntervalPL = 6;
            yAxisRangePL = 0.03f;
            yAxisIntervalPL = 6;
            yAxisUnitPL = YAxisUnitRVMS.Mm;

            // 5. Graph Color
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

        public static RVMSSettings Instance()
        {
            return instance as RVMSSettings;
        }

        public static new void CreateInstance()
        {
            if (instance == null)
                instance = new RVMSSettings();
        }
        
        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            if (xmlElement == null)
                return;
            
            // 2. Sensor
            XmlElement sensorElement = xmlElement.OwnerDocument.CreateElement("", "Sensor", "");
            xmlElement.AppendChild(sensorElement);

            XmlHelper.SetValue(sensorElement, "DataGatheringCountPerSec", dataGatheringCountPerSec.ToString());
            XmlHelper.SetValue(sensorElement, "DataCountForZeroSetting", dataCountForZeroSetting.ToString());
            XmlHelper.SetValue(sensorElement, "GearSideOffset", gearSideOffset.ToString());
            XmlHelper.SetValue(sensorElement, "ManSideOffset", manSideOffset.ToString());

            // 3. Vibration Graph
            XmlElement vibrationGraphElement = xmlElement.OwnerDocument.CreateElement("", "VibrationGraph", "");
            xmlElement.AppendChild(vibrationGraphElement);

            XmlHelper.SetValue(vibrationGraphElement, "UseLineStop", useLineStop.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "LineStopLower", lineStopLower.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "LineStopUpper", lineStopUpper.ToString());

            XmlHelper.SetValue(vibrationGraphElement, "UseLineWarning", useLineWarning.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "LineWarningLower", lineWarningLower.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "LineWarningUpper", lineWarningUpper.ToString());

            XmlHelper.SetValue(vibrationGraphElement, "XAxisDisplayTime", XAxisDisplayTime.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "XAxisDisplayTimeTotalGraph", xAxisDisplayTimeTotalGraph.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "XAxisInterval", xAxisInterval.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "YAxisRange", yAxisRange.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "YAxisInterval", yAxisInterval.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "YAxisUnit", yAxisUnit.ToString());

            XmlHelper.SetValue(vibrationGraphElement, "UseTotalGraphCenterLine", useTotalCenterLine.ToString());

            // 4. Pattern Length Graph
            XmlElement patternLengthGraphElement = xmlElement.OwnerDocument.CreateElement("", "PatternLengthGraph", "");
            xmlElement.AppendChild(patternLengthGraphElement);

            XmlHelper.SetValue(patternLengthGraphElement, "UseLineStop", useLineStopPL.ToString());
            XmlHelper.SetValue(patternLengthGraphElement, "LineStop", lineStopPL.ToString());
            XmlHelper.SetValue(patternLengthGraphElement, "XAxisDisplayTime", XAxisDisplayTimePL.ToString());
            XmlHelper.SetValue(patternLengthGraphElement, "XAxisInterval", xAxisIntervalPL.ToString());
            XmlHelper.SetValue(patternLengthGraphElement, "YAxisRange", yAxisRangePL.ToString());
            XmlHelper.SetValue(patternLengthGraphElement, "YAxisInterval", yAxisIntervalPL.ToString());
            XmlHelper.SetValue(patternLengthGraphElement, "YAxisUnitPL", yAxisUnitPL.ToString());
            XmlHelper.SetValue(patternLengthGraphElement, "VisibilityPlGraph", visibilityPlGraph.ToString());
            XmlHelper.SetValue(patternLengthGraphElement, "DecimalpointPl", decimalpointPl.ToString());

            

            // 5. Graph Color
            XmlElement graphColorElement = xmlElement.OwnerDocument.CreateElement("", "GraphColor", "");
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

            // 2. Sensor
            XmlElement sensorElement = xmlElement["Sensor"];
            if (sensorElement != null)
            {
                dataGatheringCountPerSec = int.Parse(XmlHelper.GetValue(sensorElement, "DataGatheringCountPerSec", "5"));
                dataCountForZeroSetting = int.Parse(XmlHelper.GetValue(sensorElement, "DataCountForZeroSetting", "10"));
                gearSideOffset = float.Parse(XmlHelper.GetValue(sensorElement, "GearSideOffset", "0"));
                manSideOffset = float.Parse(XmlHelper.GetValue(sensorElement, "ManSideOffset", "0"));
            }

            // 3. Vibration Graph
            XmlElement vibrationGraphElement = xmlElement["VibrationGraph"];
            if (vibrationGraphElement != null)
            {
                useLineStop = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "UseLineStop", "true"));
                lineStopLower = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineStopLower", "0.2"));
                lineStopUpper = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineStopUpper", "0.2"));

                useLineWarning = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "UseLineWarning", "true"));
                lineWarningLower = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineWarningLower", "0.1"));
                lineWarningUpper = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineWarningUpper", "0.1"));

                XAxisDisplayTime = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "XAxisDisplayTime", "1"));
                xAxisDisplayTimeTotalGraph = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "XAxisDisplayTimeTotalGraph", "60"));
                xAxisInterval = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "XAxisInterval", "6"));
                yAxisRange = float.Parse(XmlHelper.GetValue(vibrationGraphElement, "YAxisRange", "-1"));
                yAxisInterval = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "YAxisInterval", "6"));
                yAxisUnit = XmlHelper.GetValue(vibrationGraphElement, "YAxisUnit", YAxisUnitRVMS.Mm);
                useTotalCenterLine = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "UseTotalGraphCenterLine", "true"));
            }

            // 4. Pattern Length Graph
            XmlElement patternLengthElement = xmlElement["PatternLengthGraph"];
            if (patternLengthElement != null)
            {
                useLineStopPL =     bool.Parse(XmlHelper.GetValue(patternLengthElement, "UseLineStop", "false"));
                lineStopPL =        double.Parse(XmlHelper.GetValue(patternLengthElement, "LineStop", "0.7"));
                XAxisDisplayTimePL = int.Parse(XmlHelper.GetValue(patternLengthElement, "XAxisDisplayTime", "1"));
                xAxisIntervalPL =   int.Parse(XmlHelper.GetValue(patternLengthElement, "XAxisInterval", "6"));
                yAxisRangePL =      float.Parse(XmlHelper.GetValue(patternLengthElement, "YAxisRange", "-1"));
                yAxisIntervalPL =   int.Parse(XmlHelper.GetValue(patternLengthElement, "YAxisInterval", "6"));
                yAxisUnitPL =       XmlHelper.GetValue(patternLengthElement, "YAxisUnitPL", YAxisUnitRVMS.Mm);
                visibilityPlGraph = XmlHelper.GetValue(patternLengthElement, "VisibilityPlGraph", visibilityPlGraph);
                decimalpointPl =    (uint)XmlHelper.GetValue(patternLengthElement, "DecimalpointPl", 2); 
             }

            // 5. Graph Color
            XmlElement graphColorElement = xmlElement["GraphColor"];
            if (graphColorElement != null)
            {
                ColorConverter converter = new ColorConverter();

                axisColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "AxisColor", "DarkGray"));
                backColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "BackColor", "White"));
                graphColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "GraphColor", "Black"));
                graphThickness = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "GraphThickness", "3"));
                lineStopColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "LineStopColor", "Red"));
                lineStopThickness = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineStopThickness", "2"));
                lineWarningColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "LineWarningColor", "Red"));
                lineWarningThickness = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineWarningThickness", "2"));
                lineTotalGraphCenterColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "LineTotalGraphCenterColor", "Gold"));
                lineTotalGraphCenterThickness = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineTotalGraphCenterThickness", "2"));
            }
        }
    }
}
