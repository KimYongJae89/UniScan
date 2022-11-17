using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using DynMvp.UI;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Gravure.Vision;

namespace UniScanG.Gravure.Settings
{

    public enum EAsyncMode
    {
        False = System.Windows.Forms.CheckState.Unchecked,
        True = System.Windows.Forms.CheckState.Checked,
        Auto = System.Windows.Forms.CheckState.Indeterminate
    };

    public class AdditionalSettings : UniEye.Base.Settings.AdditionalSettings
    {
        //[LocalizedCategoryAttributeUniScanG("Alarm"), LocalizedDisplayNameAttributeUniScanG("Normal Defect Alarm")]
        [LocalizedCategoryAttributeUniScanG("UI"), LocalizedDisplayNameAttributeUniScanG("Defects Color"), LocalizedDescriptionAttributeUniScanG("Defects Color")]
        public Data.ColorSet ColorSet { get; } = Data.ColorTable.ColorSet;

        [LocalizedCategoryAttributeUniScanG("Grade"), LocalizedDisplayNameAttributeUniScanG("Noprint"), LocalizedDescriptionAttributeUniScanG("Noprint Grade")]
        [Browsable(false)]
        public GradeSetting GradeNP { get; set; }

        [LocalizedCategoryAttributeUniScanG("Grade"), LocalizedDisplayNameAttributeUniScanG("Pinhole"), LocalizedDescriptionAttributeUniScanG("Pinhole Grade")]
        [Browsable(false)]
        public GradeSetting GradePH { get; set; }

        [LocalizedCategoryAttributeUniScanG("Grade"), LocalizedDisplayNameAttributeUniScanG("Overall"), LocalizedDescriptionAttributeUniScanG("Overall Grade")]
        [Browsable(false)]
        public GradeSetting Grade { get; set; }

        // 티칭
        [Browsable(false)]
        [LocalizedCategoryAttributeUniScanG("Teach"), LocalizedDisplayNameAttributeUniScanG("Fast AutoTeach"), LocalizedDescriptionAttributeUniScanG("Fast AutoTeach")]
        public bool FastAutoTeach { get => fastAutoTeach; set => fastAutoTeach = value; }
        bool fastAutoTeach = false;

        [Browsable(false)]
        [LocalizedCategoryAttributeUniScanG("Teach"), LocalizedDisplayNameAttributeUniScanG("Auto Ligth"), LocalizedDescriptionAttributeUniScanG("Auto Ligth")]
        public bool AutoLight { get => autoLight; set => autoLight = value; }
        bool autoLight = false;

        [LocalizedCategoryAttributeUniScanG("Teach"), LocalizedDisplayNameAttributeUniScanG("Auto Teach"), LocalizedDescriptionAttributeUniScanG("Auto Teach")]
        public bool AutoTeach { get => autoTeach; set => autoTeach = value; }
        bool autoTeach = false;

        [LocalizedCategoryAttributeUniScanG("Teach"), LocalizedDisplayNameAttributeUniScanG("Auto Teach Timeout"), LocalizedDescriptionAttributeUniScanG("Auto Teach Timeout")]
        public int AutoTeachTimeout { get => autoTeachTimeout; set => autoTeachTimeout = value; }
        int autoTeachTimeout = 60000;

        // 그랩
        [LocalizedCategoryAttributeUniScanG("Grab"), LocalizedDisplayNameAttributeUniScanG("Async Grab Use"), LocalizedDescriptionAttributeUniScanG("Async Grab Use")]
        public EAsyncMode AsyncMode { get => asyncMode; set => asyncMode = value; }
        EAsyncMode asyncMode = EAsyncMode.Auto;

        [LocalizedCategoryAttributeUniScanG("Grab"), LocalizedDisplayNameAttributeUniScanG("Async Grab Speed [m/m]"), LocalizedDescriptionAttributeUniScanG("Async Grab Speed [m/m]")]
        public float AsyncGrabMpm { get => asyncGrabMpm; set => asyncGrabMpm = value; }
        float asyncGrabMpm = 70;

        //[Browsable(false)]
        //[LocalizedCategoryAttributeUniScanG("Grab"), LocalizedDisplayNameAttributeUniScanG("Async Grab Speed [Hz]"), LocalizedDescriptionAttributeUniScanG("Async Grab Speed [Hz]")]
        //public float AsyncGrabHz=> ConvertMpm2Hz(asyncGrabMpm);

        [LocalizedCategoryAttributeUniScanG("Grab"), LocalizedDisplayNameAttributeUniScanG("Precision Time Trace"), LocalizedDescriptionAttributeUniScanG("Precision Time Trace")]
        public bool PrecisionTimeTrace { get => precisionTimeTrace; set => precisionTimeTrace = value; }
        bool precisionTimeTrace = false;

        // 일반 불량 알람 (N개 패턴 연속)
        [LocalizedCategoryAttributeUniScanG("Alarm"), LocalizedDisplayNameAttributeUniScanG("Normal Defect Alarm")]
        public NormalDefectAlarmSetting NormalDefectAlarm { get => normalDefectAlarm; set => normalDefectAlarm = value; }
        NormalDefectAlarmSetting normalDefectAlarm;

        // 불량 개수 알람 (1개 패턴에 결함 N개 이상)
        [LocalizedCategoryAttributeUniScanG("Alarm"), LocalizedDisplayNameAttributeUniScanG("Defect Count Alarm")]
        public AlarmSetting DefectCountAlarm { get => defectCountAlarm; set => defectCountAlarm = value; }
        AlarmSetting defectCountAlarm;

        // 동일 위치 불량 알람 (동일 위치에 불량이 N개 패턴 연속)
        [LocalizedCategoryAttributeUniScanG("Alarm"), LocalizedDisplayNameAttributeUniScanG("Repeated Defect Alarm")]
        public RepeatedDefectAlarmSettingCollection RepeatedDefectAlarm { get => repeatedDefectAlarm; set => repeatedDefectAlarm = value; }
        RepeatedDefectAlarmSettingCollection repeatedDefectAlarm;

        // 시트 길이 불량 알람.
        [LocalizedCategoryAttributeUniScanG("Alarm"), LocalizedDisplayNameAttributeUniScanG("Sheet Length Alarm")]
        public AbsoluteAlarmSetting SheetLengthAlarm { get => sheetLengthAlarm; set => sheetLengthAlarm = value; }
        AbsoluteAlarmSetting sheetLengthAlarm;

        // 스트라이프 번짐 알람.
        [LocalizedCategoryAttributeUniScanG("Alarm"), LocalizedDisplayNameAttributeUniScanG("Stripe Defect Alarm")]
        public StripeDefectAlarmSetting StripeDefectAlarm { get => stripeDefectAlarm; set => stripeDefectAlarm = value; }
        StripeDefectAlarmSetting stripeDefectAlarm;

        // 롤 불량 알람
        [LocalizedCategoryAttributeUniScanG("Alarm"), LocalizedDisplayNameAttributeUniScanG("Critical Roll Alarm")]
        public CriticalRollAlarmSetting CriticalRollAlarm { get => criticalRollAlarm; set => criticalRollAlarm = value; }
        CriticalRollAlarmSetting criticalRollAlarm;

        // 마진 불량 알람
        [LocalizedCategoryAttributeUniScanG("Alarm"), LocalizedDisplayNameAttributeUniScanG("Margin Length Alarm")]
        [Browsable(true)]
        public AbsoluteAlarmSetting MarginLengthAlarm { get => marginLengthAlarm; set => marginLengthAlarm = value; }
        AbsoluteAlarmSetting marginLengthAlarm;

        // 검사
        [LocalizedCategoryAttributeUniScanG("Inspection"), LocalizedDisplayNameAttributeUniScanG("Use MultiLayer Buffer"), LocalizedDescriptionAttributeUniScanG("Use MultiLayer Buffer")]
        public bool MultiLayerBuffer { get => multiLayerBuffer; set => multiLayerBuffer = value; }
        bool multiLayerBuffer;

        [LocalizedCategoryAttributeUniScanG("Inspection"), LocalizedDisplayNameAttributeUniScanG("Auto Resolution Scale"), LocalizedDescriptionAttributeUniScanG("Auto Resolution Scale")]
        public bool AutoResolutionScale { get => autoResolutionScale; set => autoResolutionScale = value; }
        bool autoResolutionScale;

        [Browsable(false)]
        [LocalizedCategoryAttributeUniScanG("Inspection"), LocalizedDisplayNameAttributeUniScanG("Default Light Value"), LocalizedDescriptionAttributeUniScanG("Default Light Value")]
        public LightParamList DefaultLightParamList { get => defaultLightParamList; set => defaultLightParamList = value; }
        LightParamList defaultLightParamList;

        [LocalizedCategoryAttributeUniScanG("Inspection"), LocalizedDisplayNameAttributeUniScanG("Buffer Allocation Timeout"), LocalizedDescriptionAttributeUniScanG("Buffer Allocation Timeout")]
        public int BufferAllocTimeout { get => bufferAllocTimeout; set => bufferAllocTimeout = value; }
        int bufferAllocTimeout = 30000;

        [LocalizedCategoryAttributeUniScanG("Inspection"), LocalizedDisplayNameAttributeUniScanG("Auto Operation"), LocalizedDescriptionAttributeUniScanG("Auto Operation")]
        public bool AutoOperation { get => autoOperation; set => autoOperation = value; }
        bool autoOperation;

        [LocalizedCategoryAttributeUniScanG("Inspection"), LocalizedDisplayNameAttributeUniScanG("Ask before running"), LocalizedDescriptionAttributeUniScanG("Ask before running")]
        public bool StartUserQuary { get => this.startUserQuary; set => this.startUserQuary = value; }
        bool startUserQuary;

        [LocalizedCategoryAttributeUniScanG("Inspection"), LocalizedDisplayNameAttributeUniScanG("Save Good Pattern Image"), LocalizedDescriptionAttributeUniScanG("Save Good Pattern Image")]
        public bool SaveGoodPatternImage { get => saveGoodPatternImage; set => saveGoodPatternImage = value; }
        bool saveGoodPatternImage;

        [LocalizedCategoryAttributeUniScanG("Inspection"), LocalizedDisplayNameAttributeUniScanG("Minimum Line Speed [m/m]"), LocalizedDescriptionAttributeUniScanG("Minimum Line Speed [m/m]")]
        public float MinimumLineSpeed { get => minimumLineSpeed; set => minimumLineSpeed = value; }
        float minimumLineSpeed;

        [LocalizedCategoryAttributeUniScanG("Inspection"), LocalizedDisplayNameAttributeUniScanG("Maximum Line Speed [m/m]"), LocalizedDescriptionAttributeUniScanG("Maximum Line Speed [m/m]")]
        //public float MaximumLineSpeed { get => maximumLineSpeed; set => maximumLineSpeed = value; }
        public float MaximumLineSpeed
        {
            get
            {
                if (DynMvp.Devices.FrameGrabber.CameraConfiguration.ConfigFlag == "FASTMODE")
                    return 150;
                else
                    return 80;
            }
        }
        float maximumLineSpeed;

        // 확장기능
        [LocalizedCategoryAttributeUniScanG("Extension"), LocalizedDisplayNameAttributeUniScanG("StopImage Function"), LocalizedDescriptionAttributeUniScanG("Use StopImage Function")]
        [Browsable(true)]
        public bool StopImageUse { get => this.stopImageUse; set => this.stopImageUse = value; }
        bool stopImageUse;

        [LocalizedCategoryAttributeUniScanG("Extension"), LocalizedDisplayNameAttributeUniScanG("Margin Measure"), LocalizedDescriptionAttributeUniScanG("Use Margin Measure")]
        [Browsable(true)]
        public bool MarginUse { get => this.marginUse; set => this.marginUse = value; }
        bool marginUse;

        [LocalizedCategoryAttributeUniScanG("Extension"), LocalizedDisplayNameAttributeUniScanG("Margin Measure Offset"), LocalizedDescriptionAttributeUniScanG("Margin Offset")]
        [Browsable(true)]
        public SizeF MarginOffset { get => this.marginOffsetUm; set => this.marginOffsetUm = value; }
        SizeF marginOffsetUm;

        [LocalizedCategoryAttributeUniScanG("Extension"), LocalizedDisplayNameAttributeUniScanG("Transform Measure"), LocalizedDescriptionAttributeUniScanG("Use Transform Measure")]
        [Browsable(true)]
        public bool TransformUse { get => this.transformUse; set => this.transformUse = value; }
        bool transformUse;

        [LocalizedCategoryAttributeUniScanG("Extension"), LocalizedDisplayNameAttributeUniScanG("Transform Offset"), LocalizedDescriptionAttributeUniScanG("Transform Offset")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Browsable(true)]
        public RectangleF TransformOffset { get => this.transformOffsetUm; set => this.transformOffsetUm = value; }
        RectangleF transformOffsetUm;

        // 주변 장비
        [LocalizedCategoryAttributeUniScanG("Peripheral"), LocalizedDisplayNameAttributeUniScanG("Laser")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Browsable(true)]
        public LaserSetting LaserSetting { get => laserSetting; set => laserSetting = value; }
        LaserSetting laserSetting;

        // 개발자
        [LocalizedCategoryAttributeUniScanG("Develop"), LocalizedDisplayNameAttributeUniScanG("Debug_SheetGrabProcesser")]
        public bool DebugSheetGrabProcesser { get => debugSheetGrabProcesser; set => debugSheetGrabProcesser = value; }
        bool debugSheetGrabProcesser;

        [LocalizedCategoryAttributeUniScanG("Develop"), LocalizedDisplayNameAttributeUniScanG("Debug_SpreadTrace")]
        public bool DebugSpreadTrace { get => debugSpreadTrace; set => debugSpreadTrace = value; }
        bool debugSpreadTrace;

        [LocalizedCategoryAttributeUniScanG("Develop"), LocalizedDisplayNameAttributeUniScanG("Debug_OffsetLog")]
        public bool DebugOffsetLog { get => debugOffsetLog; set => debugOffsetLog = value; }
        bool debugOffsetLog;

        [LocalizedCategoryAttributeUniScanG("Develop"), LocalizedDisplayNameAttributeUniScanG("Debug_AutoLotChange")]
        public bool DebugAutoLotChange { get => debugAutoLotChange; set => debugAutoLotChange = value; }
        bool debugAutoLotChange;

        [LocalizedCategoryAttributeUniScanG("Develop"), LocalizedDisplayNameAttributeUniScanG("Debug_SaveRawImage")]
        public DebugSaveRawImageSettings DebugSaveRawImage { get => this.debugSaveRawImage; set => this.debugSaveRawImage = value; }
        DebugSaveRawImageSettings debugSaveRawImage;

        protected AdditionalSettings()
        {
            asyncMode = EAsyncMode.Auto;
            asyncGrabMpm = 70;

            this.Grade = new GradeSetting(false, 1, 3, 5);
            this.GradeNP = new GradeSetting(false, 0.5f, 1f, 1.5f);
            this.GradePH = new GradeSetting(false, 5, 10, 15);

            this.normalDefectAlarm = new NormalDefectAlarmSetting(false, 20, 1, 100);
            this.defectCountAlarm = new AlarmSetting(false, 50, "Defects");
            this.repeatedDefectAlarm = new RepeatedDefectAlarmSettingCollection(false, 10, "Patterns", 100);
            this.sheetLengthAlarm = new AbsoluteAlarmSetting(false, 100, "Patterns", 100, "um");
            this.marginLengthAlarm = new AbsoluteAlarmSetting(false, 10, "Patterns", 500f, "um");
            this.stripeDefectAlarm = new StripeDefectAlarmSetting(false, 25, 0.2);
            this.criticalRollAlarm = new CriticalRollAlarmSetting(false, 30, 10, 200);

            this.autoOperation = false;
            this.startUserQuary = true;
            this.saveGoodPatternImage = false;
            this.minimumLineSpeed = 20;
            this.maximumLineSpeed = 80;

            this.multiLayerBuffer = false;
            this.autoResolutionScale = false;

            this.stopImageUse = true;
            this.marginUse = true;
            this.marginOffsetUm = SizeF.Empty;
            this.transformUse = true;
            this.transformOffsetUm = RectangleF.Empty;

            this.defaultLightParamList = new LightParamList();
            this.laserSetting = new LaserSetting();

            this.debugSheetGrabProcesser = false;
            this.debugOffsetLog = true;
            this.debugSpreadTrace = false;
            this.debugAutoLotChange = false;
            this.debugSaveRawImage = new DebugSaveRawImageSettings();

            UpdateBrawserable();
        }

        public static new void CreateInstance()
        {
            if (instance == null)
            {
                instance = new AdditionalSettings();
                instance.Load();
            }
        }

        public static new AdditionalSettings Instance()
        {
            return instance as AdditionalSettings;
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            this.ColorSet.Save(xmlElement, "ColorSet");

            this.Grade.Save(xmlElement, "Grade");
            this.GradeNP.Save(xmlElement, "GradeNP");
            this.GradePH.Save(xmlElement, "GradePH");

            XmlHelper.SetValue(xmlElement, "FastAutoTeach", fastAutoTeach.ToString());
            XmlHelper.SetValue(xmlElement, "UseAsyncMode", asyncMode.ToString());
            XmlHelper.SetValue(xmlElement, "AsyncGrabMpm", asyncGrabMpm.ToString());
            XmlHelper.SetValue(xmlElement, "PrecisionTimeTrace", this.precisionTimeTrace);

            XmlHelper.SetValue(xmlElement, "AutoTeach", autoTeach);
            XmlHelper.SetValue(xmlElement, "AutoLight", autoLight);
            XmlHelper.SetValue(xmlElement, "AutoTeachTimeout", autoTeachTimeout.ToString());
            XmlHelper.SetValue(xmlElement, "BufferAllocTimeout", bufferAllocTimeout.ToString());

            this.defaultLightParamList.Save(xmlElement, "DefaultLightParamList");

            this.normalDefectAlarm.Save(xmlElement, "NormalDefectAlarm");
            this.defectCountAlarm.Save(xmlElement, "DefectCountAlarm");
            this.repeatedDefectAlarm.Save(xmlElement, "RepeatedDefectAlarm");
            this.sheetLengthAlarm.Save(xmlElement, "SheetLengthAlarm");
            this.stripeDefectAlarm.Save(xmlElement, "StripeDefectAlarm");
            this.criticalRollAlarm.Save(xmlElement, "CriticalRollAlarm");
            this.marginLengthAlarm.Save(xmlElement, "MarginLengthAlarm");

            XmlHelper.SetValue(xmlElement, "AutoOperation", this.autoOperation);
            XmlHelper.SetValue(xmlElement, "StartUserQuary", this.startUserQuary);
            XmlHelper.SetValue(xmlElement, "SaveGoodPatternImage", this.saveGoodPatternImage);
            XmlHelper.SetValue(xmlElement, "MinimumLineSpeed", this.minimumLineSpeed);
            XmlHelper.SetValue(xmlElement, "MaximumLineSpeed", this.maximumLineSpeed);

            XmlHelper.SetValue(xmlElement, "IsMultiLayerBuffer", multiLayerBuffer.ToString());
            XmlHelper.SetValue(xmlElement, "AutoResolutionScale", autoResolutionScale.ToString());

            XmlHelper.SetValue(xmlElement, "StopImageUse", this.stopImageUse);
            XmlHelper.SetValue(xmlElement, "MarginUse", this.marginUse);
            XmlHelper.SetValue(xmlElement, "MarginOffset", this.marginOffsetUm);
            XmlHelper.SetValue(xmlElement, "TransformUse", this.transformUse);
            XmlHelper.SetValue(xmlElement, "TransformOffset", this.transformOffsetUm);

            this.laserSetting.Save(xmlElement, "LaserSettingElement");
            //XmlHelper.SetValue(xmlElement, "UseLaser", useLaser.ToString());
            //XmlHelper.SetValue(xmlElement, "LaserDistanceM", laserDistanceM.ToString());
            //XmlHelper.SetValue(xmlElement, "SafeDistanceM", safeDistanceM.ToString());

            XmlHelper.SetValue(xmlElement, "DebugSheetGrabProcesser", this.debugSheetGrabProcesser);
            XmlHelper.SetValue(xmlElement, "DebugOffsetLog", this.debugOffsetLog);
            XmlHelper.SetValue(xmlElement, "DebugSpreadTrace", this.debugSpreadTrace);
            XmlHelper.SetValue(xmlElement, "DebugAutoLotChange", this.debugAutoLotChange);
            this.debugSaveRawImage.Save(xmlElement, "DebugSaveRawImage");
        }

        protected override void PostSave()
        {
            base.PostSave();

            try
            {
                IServerExchangeOperator server = SystemManager.Instance()?.ExchangeOperator as IServerExchangeOperator;
                if (server == null)
                    return;

                List<InspectorObj> inspectorObjList = server.GetInspectorList();

                string src = Path.Combine(PathSettings.Instance().Config, AdditionalSettings.FileName);
                foreach (InspectorObj inspectorObj in inspectorObjList)
                {
                    if (inspectorObj.CommState == CommState.OFFLINE)
                        continue;

                    string dst = Path.Combine(inspectorObj.Info.Path, "Config", AdditionalSettings.FileName);
                    FileHelper.CopyFile(src, dst, true);
                }
                
                SystemManager.Instance()?.ExchangeOperator?.SendCommand(UniScanG.Common.Exchange.ExchangeCommand.C_SYNC);
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, ex);
                string msg = StringManager.GetString("Setting Sync Fail");
                MessageForm.Show(ConfigHelper.Instance().MainForm, $"{msg}{Environment.NewLine}{ex.GetType().Name}: {ex.Message}");
            }
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            Data.ColorTable.Load(xmlElement, "ColorSet");

            this.Grade.TryLoad(xmlElement, "Grade");
            this.GradeNP.TryLoad(xmlElement, "GradeNP");
            this.GradePH.TryLoad(xmlElement, "GradePH");

            this.fastAutoTeach = XmlHelper.GetValue(xmlElement, "FastAutoTeach", this.fastAutoTeach);
            this.asyncMode = XmlHelper.GetValue(xmlElement, "UseAsyncMode", asyncMode);
            this.asyncGrabMpm = float.Parse(XmlHelper.GetValue(xmlElement, "AsyncGrabMpm", asyncGrabMpm.ToString()));
            this.precisionTimeTrace = XmlHelper.GetValue(xmlElement, "PrecisionTimeTrace", this.precisionTimeTrace);

            this.autoTeach = XmlHelper.GetValue(xmlElement, "AutoTeach", autoTeach);
            this.autoLight = XmlHelper.GetValue(xmlElement, "AutoLight", autoLight);
            this.autoTeachTimeout = XmlHelper.GetValue(xmlElement, "AutoTeachTimeout", autoTeachTimeout);
            this.bufferAllocTimeout = XmlHelper.GetValue(xmlElement, "BufferAllocTimeout", bufferAllocTimeout);

            this.stopImageUse = XmlHelper.GetValue(xmlElement, "StopImageUse", this.stopImageUse);
            this.marginUse = XmlHelper.GetValue(xmlElement, "MarginUse", this.marginUse);
            this.marginOffsetUm = XmlHelper.GetValue(xmlElement, "MarginOffset", this.marginOffsetUm);
            this.transformUse = XmlHelper.GetValue(xmlElement, "TransformUse", this.transformUse);
            this.transformOffsetUm = XmlHelper.GetValue(xmlElement, "TransformOffsetUm", this.transformOffsetUm);

            this.defaultLightParamList.Load(xmlElement, "DefaultLightParamList");
            if (this.defaultLightParamList.Count < 3)
            {
                this.defaultLightParamList.Clear();
                this.defaultLightParamList.AddLightValue(new LightParam(new LightValue(10, 10, 10, 0)) { Name = "10.0" });
                this.defaultLightParamList.AddLightValue(new LightParam(new LightValue(120, 120, 120, 0)) { Name = "40.0" });
                this.defaultLightParamList.AddLightValue(new LightParam(new LightValue(120, 120, 120, 0)) { Name = "60.0" });
            }

            bool load;

            load = this.normalDefectAlarm.TryLoad(xmlElement, "NormalDefectAlarm");
            if (load == false)
            {
                this.normalDefectAlarm.Use = XmlHelper.GetValue(xmlElement, "UseNormalDefectAlaram", this.normalDefectAlarm.Use);
                this.normalDefectAlarm.Count = XmlHelper.GetValue(xmlElement, "NormalN", this.normalDefectAlarm.Count);
                //this.normalDefectAlarm.Ratio = XmlHelper.GetValue(xmlElement, "NormalR", this.normalDefectAlarm.Ratio);
            }

            this.defectCountAlarm.TryLoad(xmlElement, "DefectCountAlarm");

            load = this.repeatedDefectAlarm.TryLoad(xmlElement, "RepeatedDefectAlarm");
            if (load == false)
            {
                this.repeatedDefectAlarm.NoPrint.Use = this.repeatedDefectAlarm.PinHole.Use = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "UseRepeatDefectAlaram", false));
                this.repeatedDefectAlarm.NoPrint.Count = this.repeatedDefectAlarm.PinHole.Count = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "RepeatN", 10));
            }

            load = this.sheetLengthAlarm.TryLoad(xmlElement, "SheetLengthAlarm");
            if (load == false)
            {
                this.sheetLengthAlarm.Use = XmlHelper.GetValue(xmlElement, "UseSheeetLengthAlarm", this.sheetLengthAlarm.Use);
                this.sheetLengthAlarm.Count = XmlHelper.GetValue(xmlElement, "SheeetLengthN", this.sheetLengthAlarm.Count);
                this.sheetLengthAlarm.Value = XmlHelper.GetValue(xmlElement, "SheeetLengthD", this.sheetLengthAlarm.Value);
            }

            this.marginLengthAlarm.TryLoad(xmlElement, "MarginLengthAlarm");
            this.stripeDefectAlarm.TryLoad(xmlElement, "StripeDefectAlarm");
            this.criticalRollAlarm.TryLoad(xmlElement, "CriticalRollAlarm");

            this.autoOperation = XmlHelper.GetValue(xmlElement, "AutoOperation", this.autoOperation);
            this.startUserQuary = XmlHelper.GetValue(xmlElement, "StartUserQuary", this.startUserQuary);
            this.saveGoodPatternImage = XmlHelper.GetValue(xmlElement, "SaveGoodPatternImage", this.saveGoodPatternImage);
            this.minimumLineSpeed = XmlHelper.GetValue(xmlElement, "MinimumLineSpeed", this.minimumLineSpeed);
            this.maximumLineSpeed = XmlHelper.GetValue(xmlElement, "MaximumLineSpeed", this.maximumLineSpeed);

            multiLayerBuffer = XmlHelper.GetValue(xmlElement, "IsMultiLayerBuffer", multiLayerBuffer);
            autoResolutionScale = XmlHelper.GetValue(xmlElement, "AutoResolutionScale", autoResolutionScale);

            load = this.laserSetting.TryLoad(xmlElement, "LaserSettingElement");
            if (load == false)
            {
                this.laserSetting.Use = XmlHelper.GetValue(xmlElement, "UseLaser", this.laserSetting.Use);
                this.laserSetting.DistanceM = XmlHelper.GetValue(xmlElement, "LaserDistanceM", this.laserSetting.DistanceM);
                this.laserSetting.SafeDistanceM = XmlHelper.GetValue(xmlElement, "SafeDistanceM", this.laserSetting.SafeDistanceM);
            }

            this.debugSheetGrabProcesser = XmlHelper.GetValue(xmlElement, "DebugSheetGrabProcesser", this.debugSheetGrabProcesser);
            this.debugOffsetLog = XmlHelper.GetValue(xmlElement, "DebugOffsetLog", this.debugOffsetLog);
            this.debugSpreadTrace = XmlHelper.GetValue(xmlElement, "DebugSpreadTrace", this.debugSpreadTrace);
            this.debugAutoLotChange = XmlHelper.GetValue(xmlElement, "DebugAutoLotChange", this.debugAutoLotChange);
            this.debugSaveRawImage.TryLoad(xmlElement, "DebugSaveRawImage");
        }

        public float ConvertMpm2Hz(float mpm)
        {
            if (mpm < 0)
                mpm = this.asyncGrabMpm;

            float resUm = 14.0f;
            DynMvp.Vision.Calibration calibration = SystemManager.Instance().DeviceBox.CameraCalibrationList.FirstOrDefault();
            if (calibration != null)
                resUm = calibration.PelSize.Height;

            float spdHz = mpm / 60 * 1000 * 1000 / resUm;
            return spdHz;
        }

        public void SetBrawserable(string propName, bool visible)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(GetType());
            UiHelper.SetBrowsableAttributeValue(properties[propName], visible);
        }

        private void UpdateBrawserable()
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(GetType());

            //UiHelper.SetBrowsableAttributeValue(properties["LaserSetting"], !useLaserBurner);

            UiHelper.SetBrowsableAttributeValue(properties["Grade"], DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.DecGrade));
            UiHelper.SetBrowsableAttributeValue(properties["GradeNP"], DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.DecGrade));
            UiHelper.SetBrowsableAttributeValue(properties["GradePH"], DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.DecGrade));

            UiHelper.SetBrowsableAttributeValue(properties["StopImageUse"], DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.ExtStopImg));

            UiHelper.SetBrowsableAttributeValue(properties["MarginUse"], DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.ExtMargin));
            UiHelper.SetBrowsableAttributeValue(properties["MarginOffset"], DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.ExtMargin));
            UiHelper.SetBrowsableAttributeValue(properties["MarginLengthAlarm"], DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.ExtMargin));

            UiHelper.SetBrowsableAttributeValue(properties["TransformUse"], DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.ExtTransfrom));
            UiHelper.SetBrowsableAttributeValue(properties["TransformOffset"], DynMvp.Base.LicenseManager.Exist(DynMvp.Base.LicenseManager.ELicenses.ExtTransfrom));
        }
    }
}
