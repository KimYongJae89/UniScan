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

namespace UniScanM.EDMS.Settings
{
    public class EDMSSettings : UniScanM.Settings.UniScanMSettings
    {
        #region //1. Data
        private int bufferingCount=5;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Buffering Count"),
        LocalizedDescriptionAttributeEDMS("3 < value < 20")]
        public int BufferingCount
        {
            get => bufferingCount;
            set
            {
                if(value < 3)
                    bufferingCount = 3;
                else if(value > 20)
                    bufferingCount = 20;
                else bufferingCount = value;
            }
        }

        private bool sheetOnlyMode;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Sheet Only Mode"),
        LocalizedDescriptionAttributeEDMS("Sheet Only Mode")]
        public bool SheetOnlyMode { get => sheetOnlyMode; set => sheetOnlyMode = value; }

        private int skipLength;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Skip Length"),
        LocalizedDescriptionAttributeEDMS("Skip Length")]
        public int SkipLength { get => skipLength; set => skipLength = value; }

        private int skipDiffBrightness=50;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Skip Brightness difference"),
        LocalizedDescriptionAttributeEDMS("brightness difference value to skip [0~255]")]
        public int SkipDiffBrightness { get => skipDiffBrightness; set => skipDiffBrightness = value; }

        private int zeroingCount;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Zeroing Count"),
        LocalizedDescriptionAttributeEDMS("Zeroing Count")]
        public int ZeroingCount { get => zeroingCount; set => zeroingCount = value; }

        private bool autoLight;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Auto Light"),
        LocalizedDescriptionAttributeEDMS("Auto Light")]
        [Browsable(false)]
        public bool AutoLight { get => autoLight; set => autoLight = value; }

        private int autoLightOffsetTop;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Auto Light Offset Front"),
        LocalizedDescriptionAttributeEDMS("Auto Light Offset Front")]
        [Browsable(false)]
        public int AutoLightOffsetTop { get => autoLightOffsetTop; set => autoLightOffsetTop = value; }

        private int autoLightOffsetBottom;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Auto Light Offset Back"),
        LocalizedDescriptionAttributeEDMS("Auto Light Offset Back")]
        [Browsable(false)]
        public int AutoLightOffsetBottom { get => autoLightOffsetBottom; set => autoLightOffsetBottom = value; }

        private int maxMeasCountPerSec;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Maximum Measure Count per sec"),
        LocalizedDescriptionAttributeEDMS("Maximum Measure Count per sec")]
        public int MaxMeasCountPerSec { get => maxMeasCountPerSec; set => maxMeasCountPerSec = value; }

        private int imageSavingInterval;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Image Save Interval"),
        LocalizedDescriptionAttributeEDMS("Image Save Interval")]
        public int ImageSavingInterval { get => imageSavingInterval; set => imageSavingInterval = value; }

        private bool isFrontPosition;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Front Position"),
        LocalizedDescriptionAttributeEDMS("Front Position")]
        public bool IsFrontPosition { get => isFrontPosition; set => isFrontPosition = value; }

        private int repetitionCount;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Repetition Error count"),
        LocalizedDescriptionAttributeEDMS("If it is set to 5, NG signal is generated only after 5 consecutive defects occur.")]
        public int RepetitionCount { get => repetitionCount; set => repetitionCount = value; }

        private bool clearProductionValueData;
        [LocalizedCategoryAttributeEDMS("1. Data"),
        LocalizedDisplayNameAttributeEDMS("Clear Production Value Data"),
        LocalizedDescriptionAttributeEDMS("Clear 'Difference' Value in Report (Restart required)")]
        public bool ClearProductionValueData { get => clearProductionValueData; set => clearProductionValueData = value; }
        #endregion

        #region //2. Y축 표시 범위
        //2. Y축 표시 범위 //////////////////////////////////////////////////////////////////////////////////////
        private float yAxisRangeMM; //T100~T104 Y축 범위 (mm)
        [LocalizedCategoryAttributeEDMS("2. Y Axis display range"),
        LocalizedDisplayNameAttributeEDMS("T100~ T104 Y Axis Range (mm)"),
        LocalizedDescriptionAttributeEDMS("T100~ T104 Y Axis Range (mm)")]
        public float YAxisRangeMM { get => yAxisRangeMM; set => yAxisRangeMM = value; }

        private float yAxisRangeUM; //T105 Y축 범위 (㎛)
        [LocalizedCategoryAttributeEDMS("2. Y Axis display range"),
        LocalizedDisplayNameAttributeEDMS("T105 Y Axis Range (um)"),
        LocalizedDescriptionAttributeEDMS("T105 Y Axis Range (um)")]
        public float YAxisRangeUM { get => yAxisRangeUM; set => yAxisRangeUM = value; }
        #endregion

        #region //3. T103 에러 설정 
        //3. T103 에러 설정 ///////////////////////////////////////////////////////////////////////////////
        //경고 선 범위(mm)
        private double t103WarningRange;
        [LocalizedCategoryAttributeEDMS("3. T103 error setting"),
        LocalizedDisplayNameAttributeEDMS("Warning Line Range (mm)"),
        LocalizedDescriptionAttributeEDMS("Warning Line Range (mm)")]
        public double T103WarningRange { get => t103WarningRange; set => t103WarningRange = value; }

        //경고 선 표시여부  
        private bool t103WarningDisplay;
        [LocalizedCategoryAttributeEDMS("3. T103 error setting"),
        LocalizedDisplayNameAttributeEDMS("Enable warning line"),
        LocalizedDescriptionAttributeEDMS("Enable warning line")]
        public bool T103WarningDisplay { get => t103WarningDisplay; set => t103WarningDisplay = value; }  

        //에러 선 범위(mm)
        private double t103ErrorRange;
        [LocalizedCategoryAttributeEDMS("3. T103 error setting"),
        LocalizedDisplayNameAttributeEDMS("Error line range (mm)"),
        LocalizedDescriptionAttributeEDMS("Error line range (mm)")]
        public double T103ErrorRange { get => t103ErrorRange; set => t103ErrorRange = value; } 

        //에러 선 표시여부
        private bool t103ErrorDisplay;
        [LocalizedCategoryAttributeEDMS("3. T103 error setting"),
        LocalizedDisplayNameAttributeEDMS("Enable error line"),
        LocalizedDescriptionAttributeEDMS("Enable error line")]
        public bool T103ErrorDisplay { get => t103ErrorDisplay; set => t103ErrorDisplay = value; }  

        // 원점기준 알람 발생 여부 T103 에러 선 사용 여부-----------------------------------------------------------------------------------------
        private bool t103AlarmOriginOutEnable;
        [LocalizedCategoryAttributeEDMS("3. T103 error setting"),
        LocalizedDisplayNameAttributeEDMS("Enable alarm with origin data"),
        LocalizedDescriptionAttributeEDMS("Enable alarm with origin data")]
        public bool T103AlarmOriginOutEnable { get => t103AlarmOriginOutEnable; set => t103AlarmOriginOutEnable = value; }  

        // 최근 N 포인트 수-----------------------------------------------------------------------------------------
        private uint t103RecentDataCount;
        [LocalizedCategoryAttributeEDMS("3. T103 error setting"),
        LocalizedDisplayNameAttributeEDMS("Recent data count"),
        LocalizedDescriptionAttributeEDMS("Recent data count")]
        public uint T103RecentDataCount { get => t103RecentDataCount; set => t103RecentDataCount = value; }


        // 최근 기준 알람 발생 여부 -----------------------------------------------------------------------------------------
        private bool t103AlarmRecentOutEnable;
        [LocalizedCategoryAttributeEDMS("3. T103 error setting"),
        LocalizedDisplayNameAttributeEDMS("Enable alarm with recent data"),
        LocalizedDescriptionAttributeEDMS("Enable alarm with recent data")]
        public bool T103AlarmRecentOutEnable { get => t103AlarmRecentOutEnable; set => t103AlarmRecentOutEnable = value; }

        //에러 선 범위(mm)
        private double t103RecentErrorRange;
        [LocalizedCategoryAttributeEDMS("3. T103 error setting"),
        LocalizedDisplayNameAttributeEDMS("Recent Error line range (mm)"),
        LocalizedDescriptionAttributeEDMS("Recent Error line range (mm)")]
        public double T103RecentErrorRange { get => t103RecentErrorRange; set => t103RecentErrorRange = value; }


        #endregion

        #region //4. T105에러 설정 
        //경고 선 범위(㎛)
        private double t105WarningRange;
        [LocalizedCategoryAttributeEDMS("4. T105 error setting"),
        LocalizedDisplayNameAttributeEDMS("Warning Line Range (um)"),
        LocalizedDescriptionAttributeEDMS("Warning Line Range (um)")]
        public double T105WarningRange { get => t105WarningRange; set => t105WarningRange = value; } //패널에 그림, Result판단용

        //경고 선 사용여부
        private bool t105WarningDisplay;
        [LocalizedCategoryAttributeEDMS("4. T105 error setting"),
        LocalizedDisplayNameAttributeEDMS("Enable warning line"),
        LocalizedDescriptionAttributeEDMS("Enable warning line")]
        public bool T105WarningDisplay { get => t105WarningDisplay; set => t105WarningDisplay = value; }  //사용안함

        //에러 선 범위(㎛)
        private double t105ErrorRange;
        [LocalizedCategoryAttributeEDMS("4. T105 error setting"),
        LocalizedDisplayNameAttributeEDMS("Error line range (um)"),
        LocalizedDescriptionAttributeEDMS("Error line range (um)")]
        public double T105ErrorRange { get => t105ErrorRange; set => t105ErrorRange = value; }  // 패널에 그리고, Result판단용

        //에러 선 사용여부
        private bool t105ErrorDisplay;
        [LocalizedCategoryAttributeEDMS("4. T105 error setting"),
        LocalizedDisplayNameAttributeEDMS("Enable error line"),
        LocalizedDescriptionAttributeEDMS("Enable error line")]
        public bool T105ErrorDisplay { get => t105ErrorDisplay; set => t105ErrorDisplay = value; } //MCIF 판단용

        //원점기준 알람 발생 여부 -----------------------------------------------------------------------------------------
        private bool t105AlarmOriginOutEnable;
        [LocalizedCategoryAttributeEDMS("4. T105 error setting"),
        LocalizedDisplayNameAttributeEDMS("Enable alarm with origin data"),
        LocalizedDescriptionAttributeEDMS("Enable alarm with origin data")]
        public bool T105AlarmOriginOutEnable { get => t105AlarmOriginOutEnable; set => t105AlarmOriginOutEnable = value; }

        // 최근 N 포인트 수------------------------------------------------------------------------------------------------
        private uint t105RecentDataCount;
        [LocalizedCategoryAttributeEDMS("4. T105 error setting"),
        LocalizedDisplayNameAttributeEDMS("Recent data count"),
        LocalizedDescriptionAttributeEDMS("Recent data count")]
        public uint T105RecentDataCount { get => t105RecentDataCount; set => t105RecentDataCount = value; }

        // 최근 기준 알람 발생 여부---------------------------------------------------------------------------------------
        private bool t105AlarmRecentOutEnable;
        [LocalizedCategoryAttributeEDMS("4. T105 error setting"),
        LocalizedDisplayNameAttributeEDMS("Enable alarm with recent data"),
        LocalizedDescriptionAttributeEDMS("Enable alarm with recent data")]
        public bool T105AlarmRecentOutEnable { get => t105AlarmRecentOutEnable; set => t105AlarmRecentOutEnable = value; }

        //Recent에러 선 범위(㎛)
        private double t105RecnetErrorRange;
        [LocalizedCategoryAttributeEDMS("4. T105 error setting"),
        LocalizedDisplayNameAttributeEDMS("Recent Error line range (um)"),
        LocalizedDescriptionAttributeEDMS("Recent Error line range (um)")]
        public double T105RecentErrorRange { get => t105RecnetErrorRange; set => t105RecnetErrorRange = value; }  // 패널에 그리고, Result판단용
        #endregion

        #region //5. Graph
        //5 그래프  //////////////////////////////////////////////////////////////////////////////////////////
        //X 축 간격
        private int xAxisInterval;
        [LocalizedCategoryAttributeEDMS("5. Chart"),
        LocalizedDisplayNameAttributeEDMS("X Axis Interval"),
        LocalizedDescriptionAttributeEDMS("X Axis Interval")]
        public int XAxisInterval { get => xAxisInterval; set => xAxisInterval = value; }
        //X 축 표시 길이(m)
        private int xAxisDisplayDistance;
        [LocalizedCategoryAttributeEDMS("5. Chart"),
        LocalizedDisplayNameAttributeEDMS("X Axis Display Distance (M)"),
        LocalizedDescriptionAttributeEDMS("X Axis Display Distance (M)")]
        public int XAxisDisplayDistance { get => xAxisDisplayDistance; set => xAxisDisplayDistance = value; }
        //Y축 간격
        private int yAxisInterval;
        [LocalizedCategoryAttributeEDMS("5. Chart"),
        LocalizedDisplayNameAttributeEDMS("Y Axis Interval"),
        LocalizedDescriptionAttributeEDMS("Y Axis Interval")]
        public int YAxisInterval { get => yAxisInterval; set => yAxisInterval = value; }
        #endregion

        #region //6 이동평균선
        //6 이동평균선
        //이동 평균주기
        private int movingAvgPeriod;
        [LocalizedCategoryAttributeEDMS("6. Moving Average"),
        LocalizedDisplayNameAttributeEDMS("Moving Average Period"),
        LocalizedDescriptionAttributeEDMS("Moving Average Period")]
        public int MovingAvgPeriod { get => movingAvgPeriod; set => movingAvgPeriod = value; }
        //이동 평균선 사용여부
        private bool useMovingAvgLine;
        [LocalizedCategoryAttributeEDMS("6. Moving Average"),
        LocalizedDisplayNameAttributeEDMS("Use Moving Average Line"),
        LocalizedDescriptionAttributeEDMS("Use Moving Average Line")]
        public bool UseMovingAvgLine { get => useMovingAvgLine; set => useMovingAvgLine = value; }
        #endregion

        #region //7. Appearance
        //7. Appearance /////////////////////////////////////////////////////////////////////////
        private Color axisColor;
        [LocalizedCategoryAttributeEDMS("7. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Axis Color"),
        LocalizedDescriptionAttributeEDMS("Axis Color")]
        public Color AxisColor { get => axisColor; set => axisColor = value; }

        private Color backColor;
        [LocalizedCategoryAttributeEDMS("7. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Background Color"),
        LocalizedDescriptionAttributeEDMS("Background Color")]
        public Color BackColor { get => backColor; set => backColor = value; }

        private Color graphColor;
        [LocalizedCategoryAttributeEDMS("7. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Graph Color"),
        LocalizedDescriptionAttributeEDMS("Graph Color")]
        public Color GraphColor { get => graphColor; set => graphColor = value; }

        private int graphThickness;
        [LocalizedCategoryAttributeEDMS("7. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Graph Thickness"),
        LocalizedDescriptionAttributeEDMS("Graph Thickness")]
        public int GraphThickness
        {
            get => graphThickness;
            set => graphThickness = value;
        }

        private Color lineStopColor;
        [LocalizedCategoryAttributeEDMS("7. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Line Stop Color"),
        LocalizedDescriptionAttributeEDMS("Line Stop Color")]
        public Color LineStopColor { get => lineStopColor; set => lineStopColor = value; }

        private int lineStopThickness;
        [LocalizedCategoryAttributeEDMS("7. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Line Stop Thickness"),
        LocalizedDescriptionAttributeEDMS("Line Stop Thickness")]
        public int LineStopThickness { get => lineStopThickness; set => lineStopThickness = value; }

        private Color lineWarningColor;
        [LocalizedCategoryAttributeEDMS("7. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Line Warning Color"),
        LocalizedDescriptionAttributeEDMS("Line Warning Color")]
        public Color LineWarningColor { get => lineWarningColor; set => lineWarningColor = value; }

        private int lineWarningThickness;
        [LocalizedCategoryAttributeEDMS("7. Appearance"),
        LocalizedDisplayNameAttributeEDMS("Line Warning Thickness"),
        LocalizedDescriptionAttributeEDMS("Line Warning Thickness")]
        public int LineWarningThickness { get => lineWarningThickness; set => lineWarningThickness = value; }
        #endregion

        protected EDMSSettings()
        {
            bufferingCount = 5;
            sheetOnlyMode = false;
            skipLength = 20;
            skipDiffBrightness = 50;
            zeroingCount = 10;
            autoLight = false;
            autoLightOffsetBottom = 0;
            autoLightOffsetTop = 0;
            maxMeasCountPerSec = 2;
            imageSavingInterval = 50;
            isFrontPosition = false;
            RepetitionCount = 5;

            t105ErrorDisplay = false;
            t105ErrorRange = 200;
            t105WarningDisplay = true;
            t105WarningRange = 100;
            xAxisDisplayDistance = 30;
            xAxisInterval = 4;
            yAxisRangeMM = 0.5f;
            yAxisRangeUM = 300f;
            yAxisInterval = 6;
            t103AlarmOriginOutEnable = false;
            t103WarningDisplay = true;
            t103ErrorDisplay = true;
            t103WarningRange = 0.25f;
            t103ErrorRange = 0.3f;
            useMovingAvgLine = false;
            movingAvgPeriod = 4;

            // 5. Graph Color
            axisColor = Color.DarkGray;
            backColor = Color.Black;
            graphColor = Color.LawnGreen;
            graphThickness = 3;
            lineStopColor = Color.Red;
            lineStopThickness = 2;
            lineWarningColor = Color.Gold;
            lineWarningThickness = 2;

            //add
            t103RecentDataCount = 5;
            t103AlarmRecentOutEnable = true;
            t105AlarmOriginOutEnable = true;
            t105RecentDataCount = 5;
            t105AlarmRecentOutEnable = true;
        }

        public static new EDMSSettings Instance()
        {
            return instance as EDMSSettings;
        }

        public static new void CreateInstance()
        {
            if (instance == null)
                instance = new EDMSSettings();
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            if (xmlElement == null)
                return;
            
            XmlElement dataElement = xmlElement.OwnerDocument.CreateElement("", "Data", "");
            xmlElement.AppendChild(dataElement);
            XmlHelper.SetValue(dataElement, "BufferingCount",       bufferingCount.ToString());
            XmlHelper.SetValue(dataElement, "SheetOnlyMode",        sheetOnlyMode.ToString());
            XmlHelper.SetValue(dataElement, "SkipLength",           skipLength.ToString());
            XmlHelper.SetValue(dataElement, "SkipDiffBrightness",   skipDiffBrightness.ToString());
            XmlHelper.SetValue(dataElement, "ZeroingCount",         zeroingCount.ToString());
            XmlHelper.SetValue(dataElement, "AutoLight",            autoLight.ToString());
            XmlHelper.SetValue(dataElement, "AutoLightOffsetBottom", autoLightOffsetBottom.ToString());
            XmlHelper.SetValue(dataElement, "AutoLightOffsetTop",   autoLightOffsetTop.ToString());
            XmlHelper.SetValue(dataElement, "MaxMeasCountPerSec",   maxMeasCountPerSec.ToString());
            XmlHelper.SetValue(dataElement, "ImageSavingInterval",  imageSavingInterval.ToString());
            XmlHelper.SetValue(dataElement, "IsFrontPosition",      isFrontPosition.ToString());
            XmlHelper.SetValue(dataElement, "RepetitionCount",      RepetitionCount.ToString());
            XmlHelper.SetValue(dataElement, "ClearProductionValueData", ClearProductionValueData.ToString());

            // 3. Vibration Graph
            XmlElement vibrationGraphElement = xmlElement.OwnerDocument.CreateElement("", "Chart", "");
            xmlElement.AppendChild(vibrationGraphElement);
            XmlHelper.SetValue(vibrationGraphElement, "UseLineStop",        t105ErrorDisplay.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "UseLineWarning",     t105WarningDisplay.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "LineStop",           t105ErrorRange.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "LineWarning",        t105WarningRange.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "XAxisDisplayDistance", xAxisDisplayDistance.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "XAxisInterval", xAxisInterval.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "YAxisRangeMM", yAxisRangeMM.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "YAxisRangeUM", yAxisRangeUM.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "YAxisInterval", yAxisInterval.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "NeedLineStopT103", t103AlarmOriginOutEnable.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "UseT103LineWarning", t103WarningDisplay.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "UseT103LineStop", t103ErrorDisplay.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "T103WarningRange", t103WarningRange.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "T103StopRange", t103ErrorRange.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "UseMovingAvgLine", useMovingAvgLine.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "MovingAvgPeriod", movingAvgPeriod.ToString());

            //add
             XmlHelper.SetValue(vibrationGraphElement, "t103RecentDataCount",       t103RecentDataCount.ToString());
             XmlHelper.SetValue(vibrationGraphElement, "t103AlarmRecentOutEnable", t103AlarmRecentOutEnable.ToString());
             XmlHelper.SetValue(vibrationGraphElement, "t105AlarmOriginOutEnable", t105AlarmOriginOutEnable.ToString());
             XmlHelper.SetValue(vibrationGraphElement, "t105RecentDataCount",       t105RecentDataCount.ToString());
             XmlHelper.SetValue(vibrationGraphElement, "t105AlarmRecentOutEnable", t105AlarmRecentOutEnable.ToString());

            XmlHelper.SetValue(vibrationGraphElement, "t103RecentErrorRange", t103RecentErrorRange.ToString());
            XmlHelper.SetValue(vibrationGraphElement, "t105RecnetErrorRange", t105RecnetErrorRange.ToString());


            // 5. Graph Color
            XmlElement graphColorElement = xmlElement.OwnerDocument.CreateElement("", "Appearance", "");
            xmlElement.AppendChild(graphColorElement);

            XmlHelper.SetValue(graphColorElement, "AxisColor", axisColor.Name);
            XmlHelper.SetValue(graphColorElement, "BackColor", backColor.Name);
            XmlHelper.SetValue(graphColorElement, "GraphColor", graphColor.Name);
            XmlHelper.SetValue(graphColorElement, "GraphThickness", graphThickness.ToString());
            XmlHelper.SetValue(graphColorElement, "LineStopColor", lineStopColor.Name);
            XmlHelper.SetValue(graphColorElement, "LineStopThickness", lineStopThickness.ToString());
            XmlHelper.SetValue(graphColorElement, "LineWarningColor", lineWarningColor.Name);
            XmlHelper.SetValue(graphColorElement, "LineWarningThickness", lineWarningThickness.ToString());
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            if (xmlElement == null)
                return;

            XmlElement dataElement = xmlElement["Data"];
            if (dataElement != null)
            {
                bufferingCount = XmlHelper.GetValue(dataElement, "BufferingCount", bufferingCount);
                sheetOnlyMode = XmlHelper.GetValue(dataElement, "SheetOnlyMode", sheetOnlyMode);
                skipLength = XmlHelper.GetValue(dataElement, "SkipLength", skipLength);
                skipDiffBrightness = XmlHelper.GetValue(dataElement, "SkipDiffBrightness", skipDiffBrightness);
                zeroingCount = XmlHelper.GetValue(dataElement, "ZeroingCount", zeroingCount);
                autoLight = XmlHelper.GetValue(dataElement, "AutoLight", autoLight);
                autoLightOffsetBottom = XmlHelper.GetValue(dataElement, "AutoLightOffsetBottom", autoLightOffsetBottom);
                autoLightOffsetTop = XmlHelper.GetValue(dataElement, "AutoLightOffsetTop", autoLightOffsetTop);
                maxMeasCountPerSec = XmlHelper.GetValue(dataElement, "MaxMeasCountPerSec", maxMeasCountPerSec);
                imageSavingInterval = XmlHelper.GetValue(dataElement, "ImageSavingInterval", imageSavingInterval);
                isFrontPosition = XmlHelper.GetValue(dataElement, "IsFrontPosition", isFrontPosition);
                repetitionCount = XmlHelper.GetValue(dataElement, "RepetitionCount", repetitionCount);
                ClearProductionValueData = XmlHelper.GetValue(dataElement, "ClearProductionValueData", ClearProductionValueData);
            }

            // 3. Vibration Graph
            XmlElement vibrationGraphElement = xmlElement["Chart"];
            if (vibrationGraphElement != null)
            {
                //useLineWarning = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "UseLineWarning", "false"));
                //lineWarningLSL = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineWarningLSL", "-0.5"));
                //lineWarningUSL = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineWarningUSL", "0.5"));
                t105ErrorDisplay = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "UseLineStop", "false"));
                t105WarningDisplay = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "UseLineWarning", "false"));
                t105ErrorRange = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineStop", "0.7"));
                t105WarningRange = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "LineWarning", "0.6"));
                xAxisDisplayDistance = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "XAxisDisplayDistance", "1"));
                xAxisInterval = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "XAxisInterval", "6"));
                yAxisRangeMM = float.Parse(XmlHelper.GetValue(vibrationGraphElement, "YAxisRangeMM", "-1"));
                yAxisRangeUM = float.Parse(XmlHelper.GetValue(vibrationGraphElement, "YAxisRangeUM", "-1"));
                yAxisInterval = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "YAxisInterval", "6"));

                t103AlarmOriginOutEnable = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "NeedLineStopT103", "false"));
                t103WarningDisplay = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "UseT103LineWarning", "true"));
                t103ErrorDisplay = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "UseT103LineStop", "true"));
                t103WarningRange = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "T103WarningRange", "0.25"));
                t103ErrorRange = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "T103StopRange", "0.3"));

                useMovingAvgLine = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "UseMovingAvgLine", "false"));
                movingAvgPeriod = int.Parse(XmlHelper.GetValue(vibrationGraphElement, "MovingAvgPeriod", "4"));

                //add
                t103RecentDataCount = uint.Parse(XmlHelper.GetValue(vibrationGraphElement, "t103RecentDataCount", "10") );
                t103AlarmRecentOutEnable = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "t103AlarmRecentOutEnable", "false")); 
                t105AlarmOriginOutEnable = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "t105AlarmOriginOutEnable", "false"));
                t105RecentDataCount = uint.Parse(XmlHelper.GetValue(vibrationGraphElement, "t105RecentDataCount", "10"));
                t105AlarmRecentOutEnable = bool.Parse(XmlHelper.GetValue(vibrationGraphElement, "t105AlarmRecentOutEnable", "false"));

                t103RecentErrorRange = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "t103RecentErrorRange", "0.35"));
                t105RecnetErrorRange = double.Parse(XmlHelper.GetValue(vibrationGraphElement, "t105RecnetErrorRange", "350"));
            }

            // 5. Graph Color
            XmlElement graphColorElement = xmlElement["Appearance"];
            if (graphColorElement != null)
            {
                ColorConverter converter = new ColorConverter();

                axisColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "AxisColor", "DarkGray"));
                backColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "BackColor", "White"));
                graphColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "GraphColor", "Black"));
                graphThickness = int.Parse(XmlHelper.GetValue(graphColorElement, "GraphThickness", "3"));
                lineStopColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "LineStopColor", "Red"));
                lineStopThickness = int.Parse(XmlHelper.GetValue(graphColorElement, "LineStopThickness", "2"));
                lineWarningColor = Color.FromName(XmlHelper.GetValue(graphColorElement, "LineWarningColor", "Gold"));
                lineWarningThickness = int.Parse(XmlHelper.GetValue(graphColorElement, "LineWarningThickness", "2"));
               }
        }
    }
}
