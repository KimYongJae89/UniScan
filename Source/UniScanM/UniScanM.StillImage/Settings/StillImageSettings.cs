using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;
using UniScanM.Settings;

namespace UniScanM.StillImage.Settings
{
    public enum EAutoStartMethod { Encoder, PLC }
    public enum EInspectionMode { Inspect, Monitor }
    public enum EOperationMode { Sequencial, Random }

    public enum PrintingLengthMode { PrintingPeriod, PrintingLength, UnPrintLength }

    public class StillImageSettings : UniScanM.Settings.UniScanMSettings
    {
        float encoderResolution = 7.0f;
        EAutoStartMethod autoStartMethod = EAutoStartMethod.Encoder;
        float minimumLineSpeed = 10.0f;
        float speedStableVariation = 0.1f;
        EInspectionMode inspectionMode = EInspectionMode.Inspect;
        EOperationMode operationMode = EOperationMode.Sequencial;
        float fovMultipication = 1.0f;
        bool asyncMode = false;
        float asyncGrabHz = -1; // -1이면 속도에 맞춰 동기화
        bool autoResetAlarm = false;
        bool modelAutoChange = true;
        int targetIntensity = 230;
        int targetIntensityVal = 20;
        int initialTopLightValue = 32;
        float backLightMultiplier = 2.0f;
        int invalidateBound = 5;
        bool userStopMode = false;
        int stopDefectSheetCnt = 20;

        int repeatInspectbyZone = 1; //Number of test repetitions by location
        [Category("Operation"), DisplayName("Repetions Inspect by zone"), Description("Number of Inspecton repetitions by Zone(location)")]
        public int RepeatInspectbyZone
        {
            get { return repeatInspectbyZone; }
            set { repeatInspectbyZone = value; }
        }

        PrintingLengthMode printingLengthMeasurMode = PrintingLengthMode.PrintingPeriod;
        [Category("Display"), DisplayName("Measurement Printing Mode"), Description("Measurement Printing Mode")]
        public PrintingLengthMode PrintingLengthMeasurMode
        {
            get { return printingLengthMeasurMode; }
            set { printingLengthMeasurMode = value; }
        }

        [Category("Encoder"), DisplayName("Encoder Resolution"), Description("Encoder Resolution [um/pls]")]
        public float EncoderResolution
        {
            get { return encoderResolution; }
            set { encoderResolution = value; }
        }

        [Category("LightTune"), DisplayName("Target Intensity"), Description("Target Intensity")]
        public int TargetIntensity
        {
            get { return targetIntensity; }
            set { targetIntensity = value; }
        }

        [Category("LightTune"), DisplayName("Target Intensity Variate"), Description("Target Intensity Variate")]
        public int TargetIntensityVal
        {
            get { return targetIntensityVal; }
            set { targetIntensityVal = value; }
        }

        [Category("LightTune"), DisplayName("Initial TopLight Value"), Description("Initial Top Light Value")]
        public int InitialTopLightValue
        {
            get { return initialTopLightValue; }
            set { initialTopLightValue = value; }
        }

        [Category("LightTune"), DisplayName("BackLight Multiplier"), Description("Back Light Multiplier")]
        public float BackLightMultiplier
        {
            get { return backLightMultiplier; }
            set { backLightMultiplier = value; }
        }

        [Category("Operation"), DisplayName("Start / Stop Method"), Description("Start / Stop Method")]
        public EAutoStartMethod AutoStartMethod
        {
            get { return autoStartMethod; }
            set { autoStartMethod = value; }
        }

        [Category("Operation"), DisplayName("Inspection Mode"), Description("Inspection Mode")]
        public EInspectionMode InspectionMode
        {
            get { return inspectionMode; }
            set { inspectionMode = value; }
        }

        [Category("Operation"), DisplayName("Operation Mode"), Description("Operation Mode")]
        public EOperationMode OperationMode
        {
            get { return operationMode; }
            set { operationMode = value; }
        }

        [Category("Operation"), DisplayName("Minimum Line Speed"), Description("Minimum Line Speed")]
        public float MinimumLineSpeed
        {
            get { return minimumLineSpeed; }
            set { minimumLineSpeed = value; }
        }

        [Category("Operation"), DisplayName("Speed Stable Variation"), Description("Speed Stable Variation")]
        public float SpeedStableVariation
        {
            get { return speedStableVariation; }
            set { speedStableVariation = value; }
        }

        [Category("Operation"), DisplayName("Fov Multipication"), Description("Multifacation Factor of FOV Size as Move")]
        public float FovMultipication
        {
            get { return fovMultipication; }
            set { fovMultipication = value; }
        }

        [Category("Operation"), DisplayName("Camera Async Mode"), Description("Camera Async Mode")]
        public bool AsyncMode
        {
            get { return asyncMode; }
            set { asyncMode = value; }
        }

        [Category("Operation"), DisplayName("Camera Async Grab Speed"), Description("Camera Async Grab Speed")]
        public float AsyncGrabHz
        {
            get { return asyncGrabHz; }
            set { asyncGrabHz = value; }
        }

        [Category("Operation"), DisplayName("Invalidate Boundary"), Description("Invalidate Boundary Size")]
        public int InvalidateBound
        {
            get { return invalidateBound; }
            set { invalidateBound = value; }
        }

        [Category("Operation"), DisplayName("User Stop Mode"), Description("Use User Stop Mode")]
        public bool UserStopMode
        {
            get { return userStopMode; }
            set { userStopMode = value; }
        }

        [Category("Operation"), DisplayName("User Stop Sheet"), Description("Set User Stop Sheet count")]
        public int StopDefectSheetCnt
        {
            get { return stopDefectSheetCnt; }
            set { stopDefectSheetCnt = value; }
        }

        [Category("Error"), DisplayName("Auto Reset Alarm"), Description("Auto Reset Alarm")]
        public bool AutoResetAlarm
        {
            get { return autoResetAlarm; }
            set { autoResetAlarm = value; }
        }


        //티칭
        float teach_AreaTolerence; //um
        float teach_WaistTolerence; //um
        int teach_ignoreCount;
        float teach_RefMarginRate; //0.5~0.7~1;


        //검사



        //[Category("Model"), DisplayName("Model Auto Change"), Description("Model Auto Change")]
        //public bool ModelAutoChange
        //{
        //    get { return modelAutoChange; }
        //    set { modelAutoChange = value; }
        //}

        protected StillImageSettings() { }

        public static StillImageSettings Instance()
        {
            return instance as StillImageSettings;
        }

        public static new void CreateInstance()
        {
            if (instance == null)
                instance = new StillImageSettings();
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            if (xmlElement == null)
                return;

            XmlHelper.SetValue(xmlElement, "EncoderUmPerPulse", encoderResolution.ToString());
            XmlHelper.SetValue(xmlElement, "AutoStartMethod", autoStartMethod.ToString());

            XmlHelper.SetValue(xmlElement, "TargetIntensity", targetIntensity.ToString());
            XmlHelper.SetValue(xmlElement, "TargetIntensityVal", targetIntensityVal.ToString());
            XmlHelper.SetValue(xmlElement, "InitialTopLightValue", initialTopLightValue.ToString());
            XmlHelper.SetValue(xmlElement, "BackLightMultiplier", backLightMultiplier.ToString());

            XmlHelper.SetValue(xmlElement, "MinimumLineSpeed", minimumLineSpeed.ToString());
            XmlHelper.SetValue(xmlElement, "SpeedStableVariation", speedStableVariation.ToString());
            XmlHelper.SetValue(xmlElement, "InspectionMode", inspectionMode.ToString());
            XmlHelper.SetValue(xmlElement, "OperationMode", operationMode.ToString());
            XmlHelper.SetValue(xmlElement, "FovMultipication", fovMultipication.ToString());
            XmlHelper.SetValue(xmlElement, "InvalidateBound", invalidateBound.ToString());

            XmlHelper.SetValue(xmlElement, "AsyncMode", asyncMode.ToString());
            XmlHelper.SetValue(xmlElement, "AsyncGrabHz", asyncGrabHz.ToString());

            XmlHelper.SetValue(xmlElement, "AutoResetAlarm", autoResetAlarm.ToString());
            XmlHelper.SetValue(xmlElement, "ModelAutoChange", modelAutoChange.ToString());

            XmlHelper.SetValue(xmlElement, "UserStopMode", userStopMode.ToString());
            XmlHelper.SetValue(xmlElement, "StopDefectSheetCount", stopDefectSheetCnt.ToString());

            XmlHelper.SetValue(xmlElement, "PrintingLengthMeasurMode", PrintingLengthMeasurMode.ToString());

            XmlHelper.SetValue(xmlElement, "RepeatInspectbyZone", repeatInspectbyZone.ToString());

            
        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            if (xmlElement == null)
                return;

            encoderResolution = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "EncoderUmPerPulse", encoderResolution.ToString()));
            autoStartMethod = (EAutoStartMethod)Enum.Parse(typeof(EAutoStartMethod), XmlHelper.GetValue(xmlElement, "AutoStartMethod", autoStartMethod.ToString()));

            targetIntensity = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "TargetIntensity", targetIntensity.ToString()));
            targetIntensityVal = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "TargetIntensityVal", targetIntensityVal.ToString()));
            initialTopLightValue = XmlHelper.GetValue(xmlElement, "InitialTopLightValue", initialTopLightValue);
            backLightMultiplier = XmlHelper.GetValue(xmlElement, "BackLightMultiplier", backLightMultiplier);

            minimumLineSpeed = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "MinimumLineSpeed", minimumLineSpeed.ToString()));
            speedStableVariation = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "SpeedStableVariation", speedStableVariation.ToString()));
            inspectionMode = (EInspectionMode)Enum.Parse(typeof(EInspectionMode), XmlHelper.GetValue(xmlElement, "InspectionMode", inspectionMode.ToString()));
            operationMode = (EOperationMode)Enum.Parse(typeof(EOperationMode), XmlHelper.GetValue(xmlElement, "OperationMode", operationMode.ToString()));
            fovMultipication = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "FovMultipication", fovMultipication.ToString()));
            invalidateBound = XmlHelper.GetValue(xmlElement, "InvalidateBound", invalidateBound);

            asyncMode = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "AsyncMode", asyncMode.ToString()));
            asyncGrabHz = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "AsyncGrabHz", asyncGrabHz.ToString()));

            this.autoResetAlarm = XmlHelper.GetValue(xmlElement, "AutoResetAlarm", this.autoResetAlarm);
            this.modelAutoChange = XmlHelper.GetValue(xmlElement, "ModelAutoChange", this.modelAutoChange);

            userStopMode = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "UserStopMode", userStopMode.ToString()));
            stopDefectSheetCnt = XmlHelper.GetValue(xmlElement, "StopDefectSheetCount", stopDefectSheetCnt);

            PrintingLengthMeasurMode = (PrintingLengthMode)Enum.Parse(
                typeof(PrintingLengthMode), 
                XmlHelper.GetValue(xmlElement, "PrintingLengthMeasurMode", PrintingLengthMeasurMode.ToString())
                );
  
            repeatInspectbyZone = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "RepeatInspectbyZone", repeatInspectbyZone.ToString()));

            if (asyncGrabHz == 1000.0f)
                asyncGrabHz = -1;
        }
    }
}
