using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Xml;
using DynMvp.Base;
using UniScanM.Gloss.Data;
using static UniScanM.Gloss.Settings.Attribute;

namespace UniScanM.Gloss.Settings
{
    public class GlossSettings : UniScanM.Settings.UniScanMSettings
    {
        private int stepCount;
        [LocalizedCategoryAttributeGloss("1. Sensor"),
        LocalizedDisplayNameAttributeGloss("Step Count (EA)"),
        LocalizedDescriptionAttributeGloss("Step Count (EA)")]
        public int StepCount { get => stepCount; set => stepCount = value; }

        private int avgCount;
        [LocalizedCategoryAttributeGloss("1. Sensor"),
        LocalizedDisplayNameAttributeGloss("Avg Count (EA)"),
        LocalizedDescriptionAttributeGloss("Avg Count (EA)")]
        public int AvgCount { get => avgCount; set => avgCount = value; }

        private bool useAutoCalibration;
        [LocalizedCategoryAttributeGloss("1. Sensor"),
        LocalizedDisplayNameAttributeGloss("UseAutoCalibration"),
        LocalizedDescriptionAttributeGloss("UseAutoCalibration")]
        public bool UseAutoCalibration { get => useAutoCalibration; set => useAutoCalibration = value; }

        private float distanceSensorTolerance;
        [LocalizedCategoryAttributeGloss("1. Sensor"),
        LocalizedDisplayNameAttributeGloss("Gloss Distance Sersor Tolerance (%)"),
        LocalizedDescriptionAttributeGloss("Gloss Distance Sersor Tolerance (%)")]
        public float DistanceSensorTolerance { get => distanceSensorTolerance; set => distanceSensorTolerance = value; }

        private float startEndDistanceSensorTolerance;
        [LocalizedCategoryAttributeGloss("1. Sensor"),
        LocalizedDisplayNameAttributeGloss("Start-End Distance Sersor Tolerance (%)"),
        LocalizedDescriptionAttributeGloss("Start-End Distance Sersor Tolerance (%)")]
        public float StartEndDistanceSensorTolerance { get => startEndDistanceSensorTolerance; set => startEndDistanceSensorTolerance = value; }

        private bool useProfileLineStop;
        [LocalizedCategoryAttributeGloss("2-1. Profile Graph"),
        LocalizedDisplayNameAttributeGloss("Use Line Stop"),
        LocalizedDescriptionAttributeGloss("Use Line Stop")]
        public bool UseProfileLineStop { get => useProfileLineStop; set => useProfileLineStop = value; }

        private double profileLineStopRange;
        [LocalizedCategoryAttributeGloss("2-1. Profile Graph"),
        LocalizedDisplayNameAttributeGloss("Line Stop Range (%)"),
        LocalizedDescriptionAttributeGloss("Line Stop Range (%)")]
        public double ProfileLineStopRange { get => profileLineStopRange; set => profileLineStopRange = value; }

        private bool useProfileLineWarning;
        [LocalizedCategoryAttributeGloss("2-1. Profile Graph"),
        LocalizedDisplayNameAttributeGloss("Use Line Warning"),
        LocalizedDescriptionAttributeGloss("Use Line Warning")]
        public bool UseProfileLineWarning { get => useProfileLineWarning; set => useProfileLineWarning = value; }

        private double profileLineWarningRange;
        [LocalizedCategoryAttributeGloss("2-1. Profile Graph"),
        LocalizedDisplayNameAttributeGloss("Line Warning Range (%)"),
        LocalizedDescriptionAttributeGloss("Line Warning Range (%)")]
        public double ProfileLineWarningRange { get => profileLineWarningRange; set => profileLineWarningRange = value; }

        private float profileYAxisRange;
        [LocalizedCategoryAttributeGloss("2-1. Profile Graph"),
        LocalizedDisplayNameAttributeGloss("Y Axis Range (%)"),
        LocalizedDescriptionAttributeGloss("Y Axis Range (%)")]
        public float ProfileYAxisRange { get => profileYAxisRange; set => profileYAxisRange = value; }

        private int profileXAxisInterval;
        [LocalizedCategoryAttributeGloss("2-1. Profile Graph"),
        LocalizedDisplayNameAttributeGloss("X Axis Interval (EA)"),
        LocalizedDescriptionAttributeGloss("X Axis Interval (EA)")]
        public int ProfileXAxisInterval { get => profileXAxisInterval; set => profileXAxisInterval = value; }

        private int profileYAxisInterval;
        [LocalizedCategoryAttributeGloss("2-1. Profile Graph"),
        LocalizedDisplayNameAttributeGloss("Y Axis Interval (EA)"),
        LocalizedDescriptionAttributeGloss("Y Axis Interval (EA)")]
        public int ProfileYAxisInterval { get => profileYAxisInterval; set => profileYAxisInterval = value; }

        private int overlayCount;
        [LocalizedCategoryAttributeGloss("2-1. Profile Graph"),
        LocalizedDisplayNameAttributeGloss("Overlay Count (EA)"),
        LocalizedDescriptionAttributeGloss("Overlay Count (EA)")]
        public int OverlayCount { get => overlayCount; set => overlayCount = value; }

        private bool useTrendLineStop;
        [LocalizedCategoryAttributeGloss("2-2. Trend Graph"),
        LocalizedDisplayNameAttributeGloss("Use Line Stop"),
        LocalizedDescriptionAttributeGloss("Use Line Stop")]
        public bool UseTrendLineStop { get => useTrendLineStop; set => useTrendLineStop = value; }

        private double trendLineStopRange;
        [LocalizedCategoryAttributeGloss("2-2. Trend Graph"),
        LocalizedDisplayNameAttributeGloss("Line Stop Range (%)"),
        LocalizedDescriptionAttributeGloss("Line Stop Range (%)")]
        public double TrendLineStopRange { get => trendLineStopRange; set => trendLineStopRange = value; }

        private bool useTrendLineWarning;
        [LocalizedCategoryAttributeGloss("2-2. Trend Graph"),
        LocalizedDisplayNameAttributeGloss("Use Line Warning"),
        LocalizedDescriptionAttributeGloss("Use Line Warning")]
        public bool UseTrendLineWarning { get => useTrendLineWarning; set => useTrendLineWarning = value; }

        private double trendLineWarningRange;
        [LocalizedCategoryAttributeGloss("2-2. Trend Graph"),
        LocalizedDisplayNameAttributeGloss("Line Warning Range (%)"),
        LocalizedDescriptionAttributeGloss("Line Warning Range (%)")]
        public double TrendLineWarningRange { get => trendLineWarningRange; set => trendLineWarningRange = value; }

        private float trendYAxisRange;
        [LocalizedCategoryAttributeGloss("2-2. Trend Graph"),
        LocalizedDisplayNameAttributeGloss("Y Axis Range (%)"),
        LocalizedDescriptionAttributeGloss("Y Axis Range (%)")]
        public float TrendYAxisRange { get => trendYAxisRange; set => trendYAxisRange = value; }

        private int trendXAxisInterval;
        [LocalizedCategoryAttributeGloss("2-2. Trend Graph"),
        LocalizedDisplayNameAttributeGloss("X Axis Interval (EA)"),
        LocalizedDescriptionAttributeGloss("X Axis Interval (EA)")]
        public int TrendXAxisInterval { get => trendXAxisInterval; set => trendXAxisInterval = value; }

        private int trendYAxisInterval;
        [LocalizedCategoryAttributeGloss("2-2. Trend Graph"),
        LocalizedDisplayNameAttributeGloss("Y Axis Interval (EA)"),
        LocalizedDescriptionAttributeGloss("Y Axis Interval (EA)")]
        public int TrendYAxisInterval { get => trendYAxisInterval; set => trendYAxisInterval = value; }

        private Color axisColor;
        [LocalizedCategoryAttributeGloss("3. Graph Figure"),
        LocalizedDisplayNameAttributeGloss("Axis Color"),
        LocalizedDescriptionAttributeGloss("Axis Color")]
        public Color AxisColor { get => axisColor; set => axisColor = value; }

        private Color backColor;
        [LocalizedCategoryAttributeGloss("3. Graph Figure"),
        LocalizedDisplayNameAttributeGloss("Background Color"),
        LocalizedDescriptionAttributeGloss("Background Color")]
        public Color BackColor { get => backColor; set => backColor = value; }

        private Color graphColor;
        [LocalizedCategoryAttributeGloss("3. Graph Figure"),
        LocalizedDisplayNameAttributeGloss("Graph Color"),
        LocalizedDescriptionAttributeGloss("Graph Color")]
        public Color GraphColor { get => graphColor; set => graphColor = value; }

        private Color overlayGraphColor;
        [LocalizedCategoryAttributeGloss("3. Graph Figure"),
        LocalizedDisplayNameAttributeGloss("Overlay Graph Color"),
        LocalizedDescriptionAttributeGloss("Overlay Graph Color")]
        public Color OverlayGraphColor { get => overlayGraphColor; set => overlayGraphColor = value; }

        private Color trendbarGraphColor;
        [LocalizedCategoryAttributeGloss("3. Graph Figure"),
        LocalizedDisplayNameAttributeGloss("Trendbar Graph Color"),
        LocalizedDescriptionAttributeGloss("Trendbar Graph Color")]
        public Color TrendbarGraphColor { get => trendbarGraphColor; set => trendbarGraphColor = value; }

        private int graphThickness;
        [LocalizedCategoryAttributeGloss("3. Graph Figure"),
        LocalizedDisplayNameAttributeGloss("Graph Thickness"),
        LocalizedDescriptionAttributeGloss("Graph Thickness")]
        public int GraphThickness { get => graphThickness; set => graphThickness = value; }

        private Color lineStopColor;
        [LocalizedCategoryAttributeGloss("3. Graph Figure"),
        LocalizedDisplayNameAttributeGloss("Line Stop Color"),
        LocalizedDescriptionAttributeGloss("Line Stop Color")]
        public Color LineStopColor { get => lineStopColor; set => lineStopColor = value; }

        private int lineStopThickness;
        [LocalizedCategoryAttributeGloss("3. Graph Figure"),
        LocalizedDisplayNameAttributeGloss("Line Stop Thickness"),
        LocalizedDescriptionAttributeGloss("Line Stop Thickness")]
        public int LineStopThickness { get => lineStopThickness; set => lineStopThickness = value; }

        private Color lineWarningColor;
        [LocalizedCategoryAttributeGloss("3. Graph Figure"),
        LocalizedDisplayNameAttributeGloss("Line Warning Color"),
        LocalizedDescriptionAttributeGloss("Line Warning Color")]
        public Color LineWarningColor { get => lineWarningColor; set => lineWarningColor = value; }

        private int lineWarningThickness;
        [LocalizedCategoryAttributeGloss("3. Graph Figure"),
        LocalizedDisplayNameAttributeGloss("Line Warning Thickness"),
        LocalizedDescriptionAttributeGloss("Line Warning Thickness")]
        public int LineWarningThickness { get => lineWarningThickness; set => lineWarningThickness = value; }

        private Color lineCenterColor;
        [LocalizedCategoryAttributeGloss("3. Graph Figure"),
        LocalizedDisplayNameAttributeGloss("Center Line Color"),
        LocalizedDescriptionAttributeGloss("Center Line Color")]
        public Color LineCenterColor { get => lineCenterColor; set => lineCenterColor = value; }

        private int lineCenterThickness;
        [LocalizedCategoryAttributeGloss("3. Graph Figure"),
        LocalizedDisplayNameAttributeGloss("Center Line Thickness"),
        LocalizedDescriptionAttributeGloss("Center Line Thickness")]
        public int LineCenterThickness { get => lineCenterThickness; set => lineCenterThickness = value; }

        private float homeStartSpeed;
        [LocalizedCategoryAttributeGloss("4. Robot"),
        LocalizedDisplayNameAttributeGloss("HomeStartSpeed (mm/s)"),
        LocalizedDescriptionAttributeGloss("HomeStartSpeed (mm/s)")]
        public float HomeStartSpeed { get => homeStartSpeed; set => homeStartSpeed = value; }

        private float homeEndSpeed;
        [LocalizedCategoryAttributeGloss("4. Robot"),
        LocalizedDisplayNameAttributeGloss("HomeEndSpeed (mm/s)"),
        LocalizedDescriptionAttributeGloss("HomeEndSpeed (mm/s)")]
        public float HomeEndSpeed { get => homeEndSpeed; set => homeEndSpeed = value; }

        private float jogSpeed;
        [LocalizedCategoryAttributeGloss("4. Robot"),
        LocalizedDisplayNameAttributeGloss("JogSpeed (mm/s)"),
        LocalizedDescriptionAttributeGloss("JogSpeed (mm/s)")]
        public float JogSpeed { get => jogSpeed; set => jogSpeed = value; }

        private float movingSpeed;
        [LocalizedCategoryAttributeGloss("4. Robot"),
        LocalizedDisplayNameAttributeGloss("MovingSpeed (mm/s)"),
        LocalizedDescriptionAttributeGloss("MovingSpeed (mm/s)")]
        public float MovingSpeed { get => movingSpeed; set => movingSpeed = value; }

        private float referenceMovingSpeed;
        [LocalizedCategoryAttributeGloss("4. Robot"),
        LocalizedDisplayNameAttributeGloss("ReferenceMovingSpeed (mm/s)"),
        LocalizedDescriptionAttributeGloss("ReferenceMovingSpeed (mm/s)")]
        public float ReferenceMovingSpeed { get => referenceMovingSpeed; set => referenceMovingSpeed = value; }

        private bool revesePosition { get; set; } = true;
        [LocalizedCategoryAttributeGloss("4. Robot"),
        LocalizedDisplayNameAttributeGloss("RevesePosition"),
        LocalizedDescriptionAttributeGloss("RevesePosition")]
        public bool RevesePosition { get => revesePosition; set => revesePosition = value; }

        private float positionOffset;
        [LocalizedCategoryAttributeGloss("4. Robot"),
        LocalizedDisplayNameAttributeGloss("PositionOffset (mm)"),
        LocalizedDescriptionAttributeGloss("PositionOffset (mm)")]
        public float PositionOffset { get => positionOffset; set => positionOffset = value; }

        private float farFromMirrorPosition;
        [LocalizedCategoryAttributeGloss("4. Robot"),
        LocalizedDisplayNameAttributeGloss("FarFromMirrorPosition (mm)"),
        LocalizedDescriptionAttributeGloss("FarFromMirrorPosition (mm)")]
        public float FarFromMirrorPosition { get => farFromMirrorPosition; set => farFromMirrorPosition = value; }

        private float nearToMirrorLeftPosition;
        [LocalizedCategoryAttributeGloss("4. Robot"),
        LocalizedDisplayNameAttributeGloss("NearToMirrorLeftPosition (mm)"),
        LocalizedDescriptionAttributeGloss("NearToMirrorLeftPosition (mm)")]
        public float NearToMirrorLeftPosition { get => nearToMirrorLeftPosition; set => nearToMirrorLeftPosition = value; }

        private float nearToMirrorRightPosition;
        [LocalizedCategoryAttributeGloss("4. Robot"),
        LocalizedDisplayNameAttributeGloss("NearToMirrorRightPosition (mm)"),
        LocalizedDescriptionAttributeGloss("NearToMirrorRightPosition (mm)")]
        public float NearToMirrorRightPosition { get => nearToMirrorRightPosition; set => nearToMirrorRightPosition = value; }

        private float calibrationPosition;
        [LocalizedCategoryAttributeGloss("4. Robot"),
        LocalizedDisplayNameAttributeGloss("CalibrationPosition (mm)"),
        LocalizedDescriptionAttributeGloss("CalibrationPosition (mm)")]
        public float CalibrationPosition { get => calibrationPosition; set => calibrationPosition = value; }

        private float safetyPosition;
        [LocalizedCategoryAttributeGloss("4. Robot"),
        LocalizedDisplayNameAttributeGloss("SafetyPosition (mm)"),
        LocalizedDescriptionAttributeGloss("SafetyPosition (mm)")]
        public float SafetyPosition { get => safetyPosition; set => safetyPosition = value; }

        [LocalizedCategoryAttributeGloss("4. Robot"),
        LocalizedDisplayNameAttributeGloss("ScanWidth (mm)"),
        LocalizedDescriptionAttributeGloss("ScanWidth (mm)")]
        public List<GlossScanWidth> GlossScanWidthList { get; set; } = new List<GlossScanWidth>();

        [LocalizedCategoryAttributeGloss("5. System"),
        LocalizedDisplayNameAttributeGloss("Device Code"),
        LocalizedDescriptionAttributeGloss("Device Code")]
        public string DeviceCode { get; set; } = "";

        [LocalizedCategoryAttributeGloss("5. System"),
        LocalizedDisplayNameAttributeGloss("Workplace Code"),
        LocalizedDescriptionAttributeGloss("Workplace Code")]
        public string WorkplaceCode { get; set; } = "";

        [LocalizedCategoryAttributeGloss("5. System"),
        LocalizedDisplayNameAttributeGloss("Calibration"),
        LocalizedDescriptionAttributeGloss("Calibration")]
        public List<GlossCalibrationData> GlossCalibrationParamList { get; set; } = new List<GlossCalibrationData>();

        [LocalizedCategoryAttributeGloss("5. System"),
        LocalizedDisplayNameAttributeGloss("Use Module Mode"),
        LocalizedDescriptionAttributeGloss("Use Module Mode")]
        public bool UseModuleMode { get; set; } = false;

        [LocalizedCategoryAttributeGloss("5. System"),
        LocalizedDisplayNameAttributeGloss("MQTT IP Address"),
        LocalizedDescriptionAttributeGloss("MQTT IP Address")]
        public string CMMQTTIpAddress { get; set; } = "127.0.0.1";

        [Browsable(false)]
        public string CMTopicName { get; set; } = "UniscanC.CM";

        [Browsable(false)]
        public string GMTopicName { get; set; } = "UniscanC.GM";

        [LocalizedCategoryAttributeGloss("5. System"),
        LocalizedDisplayNameAttributeGloss("Database IP Address"),
        LocalizedDescriptionAttributeGloss("Database IP Address")]
        public string CMDBIpAddress { get; set; } = "127.0.0.1";

        [Browsable(false)]
        public string CMDBName { get; set; } = "UniScan";

        [Browsable(false)]
        public string CMDBUserName { get; set; } = "postgres";

        [Browsable(false)]
        public string CMDBPassword { get; set; } = "masterkey";

        [Browsable(false)]
        public GlossScanWidth SelectedGlossScanWidth { get; set; } = new GlossScanWidth();

        [Browsable(false)]
        public GlossCalibrationData SelectedGlossCalibrationParam { get; set; } = new GlossCalibrationData();

        [Browsable(false)]
        public float AxisXDiffDegree { get; set; } = 1000;

        [Browsable(false)]
        public float AxisYDiffDegree { get; set; } = 1000;

        [Browsable(false)]
        public float FarFromMirrorSensorValue { get; set; } = 0;

        [Browsable(false)]
        public float NearToMirrorSensorLeftSideValue { get; set; } = 0;

        [Browsable(false)]
        public float NearToMirrorSensorRightSideValue { get; set; } = 0;

        [Browsable(false)]
        public float BlackMirrorValue { get; set; } = 0;

        [Browsable(false)]
        public int OutMCPort { get; set; } = 4;

        [Browsable(false)]
        public string ManualLotNo { get; set; } = string.Empty;

        protected GlossSettings()
        {
            StepCount = 10;
            AvgCount = 2;
            UseAutoCalibration = false;
            DistanceSensorTolerance = 1;
            StartEndDistanceSensorTolerance = 1;
            AxisXDiffDegree = 0;
            AxisYDiffDegree = 0;

            // 3point 센서값
            FarFromMirrorSensorValue = 0;
            NearToMirrorSensorLeftSideValue = 0;
            NearToMirrorSensorRightSideValue = 0;

            BlackMirrorValue = 0;

            UseProfileLineStop = true;
            ProfileLineStopRange = 4.0f;
            UseProfileLineWarning = true;
            ProfileLineWarningRange = 3.0f;
            ProfileXAxisInterval = 8;
            ProfileYAxisRange = 5.0f;
            ProfileYAxisInterval = 6;
            OverlayCount = 10;

            UseTrendLineStop = true;
            TrendLineStopRange = 4.0f;
            UseTrendLineWarning = true;
            TrendLineWarningRange = 3.0f;
            TrendXAxisInterval = 8;
            TrendYAxisRange = 5.0f;
            TrendYAxisInterval = 6;

            axisColor = Color.DarkGray;
            backColor = Color.Black;
            graphColor = Color.Chartreuse;
            overlayGraphColor = Color.Pink;
            trendbarGraphColor = Color.Blue;
            graphThickness = 3;
            lineStopColor = Color.Red;
            lineStopThickness = 2;
            lineWarningColor = Color.Yellow;
            lineWarningThickness = 2;
            lineCenterColor = Color.Gold;
            lineCenterThickness = 2;

            HomeStartSpeed = 100;
            HomeEndSpeed = 10;
            JogSpeed = 100;
            MovingSpeed = 100;
            ReferenceMovingSpeed = 100;
            RevesePosition = true;
            PositionOffset = 0;

            // 3point position
            FarFromMirrorPosition = 0;
            NearToMirrorLeftPosition = 0;
            NearToMirrorRightPosition = 0;

            CalibrationPosition = 0;
            SafetyPosition = 0;

            OutMCPort = 4;
        }

        public static GlossSettings Instance()
        {
            return instance as GlossSettings;
        }

        public static new void CreateInstance()
        {
            if (instance == null)
            {
                instance = new GlossSettings();
            }
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            if (xmlElement == null)
            {
                return;
            }

            XmlElement sensorElement = xmlElement.OwnerDocument.CreateElement("", "Sensor", "");
            xmlElement.AppendChild(sensorElement);
            XmlHelper.SetValue(sensorElement, "StepCount", StepCount.ToString());
            XmlHelper.SetValue(sensorElement, "AvgCount", AvgCount.ToString());
            XmlHelper.SetValue(sensorElement, "UseAutoCalibration", UseAutoCalibration.ToString());
            XmlHelper.SetValue(sensorElement, "DistanceSensorTolerance", DistanceSensorTolerance.ToString());
            XmlHelper.SetValue(sensorElement, "AxisXDiffDegree", AxisXDiffDegree.ToString());
            XmlHelper.SetValue(sensorElement, "AxisYDiffDegree", AxisYDiffDegree.ToString());
            XmlHelper.SetValue(sensorElement, "FarFromMirrorSensorValue", FarFromMirrorSensorValue.ToString());
            XmlHelper.SetValue(sensorElement, "NearToMirrorSensorLeftSideValue", NearToMirrorSensorLeftSideValue.ToString());
            XmlHelper.SetValue(sensorElement, "NearToMirrorSensorRightSideValue", NearToMirrorSensorRightSideValue.ToString());
            XmlHelper.SetValue(sensorElement, "BlackMirrorValue", BlackMirrorValue.ToString());

            XmlElement profileGraphElement = xmlElement.OwnerDocument.CreateElement("", "ProfileGraph", "");
            xmlElement.AppendChild(profileGraphElement);
            XmlHelper.SetValue(profileGraphElement, "UseProfileLineStop", useProfileLineStop.ToString());
            XmlHelper.SetValue(profileGraphElement, "ProfileLineStopRange", profileLineStopRange.ToString());
            XmlHelper.SetValue(profileGraphElement, "UseProfileLineWarning", useProfileLineWarning.ToString());
            XmlHelper.SetValue(profileGraphElement, "ProfileLineWarningRange", profileLineWarningRange.ToString());
            XmlHelper.SetValue(profileGraphElement, "ProfileXAxisInterval", profileXAxisInterval.ToString());
            XmlHelper.SetValue(profileGraphElement, "ProfileYAxisRange", profileYAxisRange.ToString());
            XmlHelper.SetValue(profileGraphElement, "ProfileYAxisInterval", profileYAxisInterval.ToString());
            XmlHelper.SetValue(profileGraphElement, "OverlayCount", OverlayCount.ToString());

            XmlElement trendGraphElement = xmlElement.OwnerDocument.CreateElement("", "TrendGraph", "");
            xmlElement.AppendChild(trendGraphElement);
            XmlHelper.SetValue(trendGraphElement, "UseTrendLineStop", useTrendLineStop.ToString());
            XmlHelper.SetValue(trendGraphElement, "TrendLineStopRange", trendLineStopRange.ToString());
            XmlHelper.SetValue(trendGraphElement, "UseTrendLineWarning", useTrendLineWarning.ToString());
            XmlHelper.SetValue(trendGraphElement, "TrendLineWarningRange", trendLineWarningRange.ToString());
            XmlHelper.SetValue(trendGraphElement, "TrendXAxisInterval", trendXAxisInterval.ToString());
            XmlHelper.SetValue(trendGraphElement, "TrendYAxisRange", trendYAxisRange.ToString());
            XmlHelper.SetValue(trendGraphElement, "TrendYAxisInterval", trendYAxisInterval.ToString());

            XmlElement graphFigureElement = xmlElement.OwnerDocument.CreateElement("", "GraphFigure", "");
            xmlElement.AppendChild(graphFigureElement);
            XmlHelper.SetValue(graphFigureElement, "AxisColor", axisColor.Name);
            XmlHelper.SetValue(graphFigureElement, "BackColor", backColor.Name);
            XmlHelper.SetValue(graphFigureElement, "GraphColor", graphColor.Name);
            XmlHelper.SetValue(graphFigureElement, "OverlayGraphColor", overlayGraphColor.Name);
            XmlHelper.SetValue(graphFigureElement, "TrendbarGraphColor", trendbarGraphColor.Name);
            XmlHelper.SetValue(graphFigureElement, "GraphThickness", graphThickness.ToString());
            XmlHelper.SetValue(graphFigureElement, "LineStopColor", lineStopColor.Name);
            XmlHelper.SetValue(graphFigureElement, "LineStopThickness", lineStopThickness.ToString());
            XmlHelper.SetValue(graphFigureElement, "LineWarningColor", lineWarningColor.Name);
            XmlHelper.SetValue(graphFigureElement, "LineWarningThickness", lineWarningThickness.ToString());
            XmlHelper.SetValue(graphFigureElement, "LineTotalGraphCenterColor", lineCenterColor.Name);
            XmlHelper.SetValue(graphFigureElement, "LineTotalGraphCenterThickness", lineCenterThickness.ToString());

            XmlElement robotElement = xmlElement.OwnerDocument.CreateElement("", "Robot", "");
            xmlElement.AppendChild(robotElement);
            XmlHelper.SetValue(robotElement, "HomeStartSpeed", homeStartSpeed);
            XmlHelper.SetValue(robotElement, "homeEndSpeed", homeEndSpeed);
            XmlHelper.SetValue(robotElement, "JogSpeed", jogSpeed);
            XmlHelper.SetValue(robotElement, "MovingSpeed", movingSpeed);
            XmlHelper.SetValue(robotElement, "ReferenceMovingSpeed", referenceMovingSpeed);
            XmlHelper.SetValue(robotElement, "RevesePosition", revesePosition);
            XmlHelper.SetValue(robotElement, "PositionOffset", positionOffset);
            XmlHelper.SetValue(robotElement, "FarFromMirrorPosition", FarFromMirrorPosition);
            XmlHelper.SetValue(robotElement, "NearToMirrorLeftPosition", NearToMirrorLeftPosition);
            XmlHelper.SetValue(robotElement, "NearToMirrorRightPosition", NearToMirrorRightPosition);
            XmlHelper.SetValue(robotElement, "CalibrationPosition", calibrationPosition);
            XmlHelper.SetValue(robotElement, "SafetyPosition", safetyPosition);

            XmlElement scanWidthElement = robotElement.OwnerDocument.CreateElement("", "ScanWidth", "");
            robotElement.AppendChild(scanWidthElement);
            for (int i = 0; i < GlossScanWidthList.Count; i++)
            {
                XmlElement tempScanWidthElement = scanWidthElement.OwnerDocument.CreateElement("", "ScanWidth" + i.ToString(), "");
                scanWidthElement.AppendChild(tempScanWidthElement);
                GlossScanWidthList[i].SaveXml(tempScanWidthElement);
            }

            XmlElement systemElement = xmlElement.OwnerDocument.CreateElement("", "System", "");
            xmlElement.AppendChild(systemElement);
            XmlHelper.SetValue(systemElement, "DeviceCode", DeviceCode);
            XmlHelper.SetValue(systemElement, "WorkplaceCode", WorkplaceCode);

            XmlElement calibrationElement = systemElement.OwnerDocument.CreateElement("", "Calibration", "");
            systemElement.AppendChild(calibrationElement);
            for (int i = 0; i < GlossCalibrationParamList.Count; i++)
            {
                XmlElement tempCalibrationElement = calibrationElement.OwnerDocument.CreateElement("", "Calibration" + i.ToString(), "");
                calibrationElement.AppendChild(tempCalibrationElement);
                GlossCalibrationParamList[i].SaveXml(tempCalibrationElement);
            }

            XmlHelper.SetValue(systemElement, "UseModuleMode", UseModuleMode);
            XmlHelper.SetValue(systemElement, "CMMQTTIpAddress", CMMQTTIpAddress);
            XmlHelper.SetValue(systemElement, "CMDBIpAddress", CMDBIpAddress);

            XmlElement ioElement = xmlElement.OwnerDocument.CreateElement("", "IO", "");
            xmlElement.AppendChild(ioElement);
            XmlHelper.SetValue(ioElement, "OutMCPort", OutMCPort);
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            if (xmlElement == null)
            {
                return;
            }

            XmlElement sensorElement = xmlElement["Sensor"];
            if (sensorElement != null)
            {
                StepCount = int.Parse(XmlHelper.GetValue(sensorElement, "StepCount", "20"));
                AvgCount = int.Parse(XmlHelper.GetValue(sensorElement, "AvgCount", "1"));
                UseAutoCalibration = bool.Parse(XmlHelper.GetValue(sensorElement, "UseAutoCalibration", "false"));
                DistanceSensorTolerance = float.Parse(XmlHelper.GetValue(sensorElement, "DistanceSensorTolerance", "50"));
                StartEndDistanceSensorTolerance = float.Parse(XmlHelper.GetValue(sensorElement, "StartEndDistanceSensorTolerance", "50"));
                AxisXDiffDegree = float.Parse(XmlHelper.GetValue(sensorElement, "AxisXDiffDegree", "10"));
                AxisYDiffDegree = float.Parse(XmlHelper.GetValue(sensorElement, "AxisYDiffDegree", "10"));
                FarFromMirrorSensorValue = float.Parse(XmlHelper.GetValue(sensorElement, "FarFromMirrorSensorValue", "0"));
                NearToMirrorSensorLeftSideValue = float.Parse(XmlHelper.GetValue(sensorElement, "NearToMirrorSensorLeftSideValue", "0"));
                NearToMirrorSensorRightSideValue = float.Parse(XmlHelper.GetValue(sensorElement, "NearToMirrorSensorRightSideValue", "0"));
                BlackMirrorValue = float.Parse(XmlHelper.GetValue(sensorElement, "BlackMirrorValue", "0"));
            }

            XmlElement profileGraphElement = xmlElement["ProfileGraph"];
            if (profileGraphElement != null)
            {
                UseProfileLineStop = bool.Parse(XmlHelper.GetValue(profileGraphElement, "UseProfileLineStop", "true"));
                ProfileLineStopRange = double.Parse(XmlHelper.GetValue(profileGraphElement, "ProfileLineStopRange", "4"));
                UseProfileLineWarning = bool.Parse(XmlHelper.GetValue(profileGraphElement, "UseProfileLineWarning", "true"));
                ProfileLineWarningRange = double.Parse(XmlHelper.GetValue(profileGraphElement, "ProfileLineWarningRange", "3"));
                ProfileXAxisInterval = int.Parse(XmlHelper.GetValue(profileGraphElement, "ProfileXAxisInterval", "8"));
                ProfileYAxisRange = float.Parse(XmlHelper.GetValue(profileGraphElement, "ProfileYAxisRange", "5"));
                ProfileYAxisInterval = int.Parse(XmlHelper.GetValue(profileGraphElement, "ProfileYAxisInterval", "6"));
                OverlayCount = int.Parse(XmlHelper.GetValue(profileGraphElement, "OverlayCount", "10"));
            }

            XmlElement trendGraphElement = xmlElement["TrendGraph"];
            if (trendGraphElement != null)
            {
                UseTrendLineStop = bool.Parse(XmlHelper.GetValue(trendGraphElement, "UseTrendLineStop", "true"));
                TrendLineStopRange = double.Parse(XmlHelper.GetValue(trendGraphElement, "TrendLineStopRange", "4"));
                UseTrendLineWarning = bool.Parse(XmlHelper.GetValue(trendGraphElement, "UseTrendLineWarning", "true"));
                TrendLineWarningRange = double.Parse(XmlHelper.GetValue(trendGraphElement, "TrendLineWarningRange", "3"));
                TrendXAxisInterval = int.Parse(XmlHelper.GetValue(trendGraphElement, "TrendXAxisInterval", "8"));
                TrendYAxisRange = float.Parse(XmlHelper.GetValue(trendGraphElement, "TrendYAxisRange", "5"));
                TrendYAxisInterval = int.Parse(XmlHelper.GetValue(trendGraphElement, "TrendYAxisInterval", "6"));
            }

            XmlElement graphFigureElement = xmlElement["GraphFigure"];
            if (graphFigureElement != null)
            {
                ColorConverter converter = new ColorConverter();

                axisColor = Color.FromName(XmlHelper.GetValue(graphFigureElement, "AxisColor", "DarkGray"));
                backColor = Color.FromName(XmlHelper.GetValue(graphFigureElement, "BackColor", "White"));
                graphColor = Color.FromName(XmlHelper.GetValue(graphFigureElement, "GraphColor", "Black"));
                overlayGraphColor = Color.FromName(XmlHelper.GetValue(graphFigureElement, "OverlayGraphColor", "Pink"));
                trendbarGraphColor = Color.FromName(XmlHelper.GetValue(graphFigureElement, "TrendbarGraphColor", "Blue"));
                graphThickness = int.Parse(XmlHelper.GetValue(graphFigureElement, "GraphThickness", "3"));
                lineStopColor = Color.FromName(XmlHelper.GetValue(graphFigureElement, "LineStopColor", "Red"));
                lineStopThickness = int.Parse(XmlHelper.GetValue(graphFigureElement, "LineStopThickness", "2"));
                lineWarningColor = Color.FromName(XmlHelper.GetValue(graphFigureElement, "LineWarningColor", "Red"));
                lineWarningThickness = int.Parse(XmlHelper.GetValue(graphFigureElement, "LineWarningThickness", "2"));
                lineCenterColor = Color.FromName(XmlHelper.GetValue(graphFigureElement, "LineTotalGraphCenterColor", "Gold"));
                lineCenterThickness = int.Parse(XmlHelper.GetValue(graphFigureElement, "LineTotalGraphCenterThickness", "2"));
            }

            XmlElement robotElement = xmlElement["Robot"];
            if (robotElement != null)
            {
                HomeStartSpeed = Convert.ToSingle(XmlHelper.GetValue(robotElement, "HomeStartSpeed", "100"));
                HomeEndSpeed = Convert.ToSingle(XmlHelper.GetValue(robotElement, "HomeEndSpeed", "10"));
                JogSpeed = Convert.ToSingle(XmlHelper.GetValue(robotElement, "JogSpeed", "100"));
                MovingSpeed = Convert.ToSingle(XmlHelper.GetValue(robotElement, "MovingSpeed", "100"));
                ReferenceMovingSpeed = Convert.ToSingle(XmlHelper.GetValue(robotElement, "ReferenceMovingSpeed", "100"));
                RevesePosition = Convert.ToBoolean(XmlHelper.GetValue(robotElement, "RevesePosition", "True"));
                PositionOffset = Convert.ToSingle(XmlHelper.GetValue(robotElement, "PositionOffset", "0"));
                FarFromMirrorPosition = Convert.ToSingle(XmlHelper.GetValue(robotElement, "FarFromMirrorPosition", "0"));
                NearToMirrorLeftPosition = Convert.ToSingle(XmlHelper.GetValue(robotElement, "NearToMirrorLeftPosition", "0"));
                NearToMirrorRightPosition = Convert.ToSingle(XmlHelper.GetValue(robotElement, "NearToMirrorRightPosition", "0"));
                CalibrationPosition = Convert.ToSingle(XmlHelper.GetValue(robotElement, "CalibrationPosition", "0"));
                SafetyPosition = Convert.ToSingle(XmlHelper.GetValue(robotElement, "SafetyPosition", "0"));

                XmlElement scanWidthElement = robotElement["ScanWidth"];
                if (scanWidthElement != null)
                {
                    GlossScanWidthList.Clear();
                    foreach (XmlElement childScanWidthElement in scanWidthElement)
                    {
                        for (int i = 0; i < scanWidthElement.ChildNodes.Count; i++)
                        {
                            if (childScanWidthElement.Name == "ScanWidth" + i.ToString())
                            {
                                GlossScanWidth tempGlossScanWidth = new GlossScanWidth();

                                tempGlossScanWidth.LoadXml(childScanWidthElement);
                                GlossScanWidthList.Add(tempGlossScanWidth);
                            }
                        }
                    }
                }
            }

            XmlElement systemElement = xmlElement["System"];
            if (systemElement != null)
            {
                DeviceCode = XmlHelper.GetValue(systemElement, "DeviceCode", "");
                WorkplaceCode = XmlHelper.GetValue(systemElement, "WorkplaceCode", "");

                XmlElement calibrationElement = systemElement["Calibration"];
                if (calibrationElement != null)
                {
                    GlossCalibrationParamList.Clear();
                    foreach (XmlElement childCalibrationElementElement in calibrationElement)
                    {
                        for (int i = 0; i < calibrationElement.ChildNodes.Count; i++)
                        {
                            if (childCalibrationElementElement.Name == "Calibration" + i.ToString())
                            {
                                GlossCalibrationData tempGlossCalibrationData = new GlossCalibrationData();

                                tempGlossCalibrationData.LoadXml(childCalibrationElementElement);
                                GlossCalibrationParamList.Add(tempGlossCalibrationData);
                            }
                        }
                    }
                }

                UseModuleMode = Convert.ToBoolean(XmlHelper.GetValue(systemElement, "UseModuleMode", "false"));
                CMMQTTIpAddress = XmlHelper.GetValue(systemElement, "CMMQTTIpAddress", "127.0.0.1");
                CMDBIpAddress = XmlHelper.GetValue(systemElement, "CMDBIpAddress", "127.0.0.1");
            }

            XmlElement ioElement = xmlElement["IO"];
            if (ioElement != null)
            {
                OutMCPort = Convert.ToInt32(XmlHelper.GetValue(ioElement, "OutMCPort", "4"));
            }
        }
    }
}
