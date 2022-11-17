using System;
using DynMvp.Base;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

using System.Drawing;

namespace UniScanX.MPAlignment.Settings
{
    class MPSettings : UniEye.Base.Settings.AdditionalSettings
    {
        private PointF tableOffset = new PointF(175,78); //mm
        public PointF TableOffset //mm
        {
            get => tableOffset;
            set => tableOffset = value;
        }

        private SizeF tableSize = new SizeF(500, 500); //mm
        public SizeF TableSize //mm
        {
            get => tableSize;
            set => tableSize = value;
        }


        int repeatInspectbyZone = 1; //Number of test repetitions by location
        [Category("Operation"), DisplayName("Repetions Inspect by zone"), Description("Number of Inspecton repetitions by Zone(location)")]
        public int RepeatInspectbyZone
        {
            get { return repeatInspectbyZone; }
            set { repeatInspectbyZone = value; }
        }

        //티칭
        float teach_AreaTolerence; //um
        float teach_WaistTolerence; //um
        int teach_ignoreCount;
        float teach_RefMarginRate; //0.5~0.7~1;

        //검사

        protected MPSettings() { }

        new public static MPSettings Instance()
        {
            return instance as MPSettings;
        }

        public static new void CreateInstance()
        {
            if (instance == null)
                instance = new MPSettings();
        }

        public override void Save(XmlElement xmlElement)
        {
            base.Save(xmlElement);

            if (xmlElement == null)
                return;



            //XmlHelper.SetValue(xmlElement, "TargetIntensity", targetIntensity.ToString());
            //XmlHelper.SetValue(xmlElement, "TargetIntensityVal", targetIntensityVal.ToString());
            //XmlHelper.SetValue(xmlElement, "InitialTopLightValue", initialTopLightValue.ToString());
            //XmlHelper.SetValue(xmlElement, "BackLightMultiplier", backLightMultiplier.ToString());


            //XmlHelper.SetValue(xmlElement, "FovMultipication", fovMultipication.ToString());
            //XmlHelper.SetValue(xmlElement, "InvalidateBound", invalidateBound.ToString());

            //XmlHelper.SetValue(xmlElement, "AsyncMode", asyncMode.ToString());
            //XmlHelper.SetValue(xmlElement, "AsyncGrabHz", asyncGrabHz.ToString());

            //XmlHelper.SetValue(xmlElement, "AutoResetAlarm", autoResetAlarm.ToString());
            //XmlHelper.SetValue(xmlElement, "ModelAutoChange", modelAutoChange.ToString());

            //XmlHelper.SetValue(xmlElement, "UserStopMode", userStopMode.ToString());
            //XmlHelper.SetValue(xmlElement, "StopDefectSheetCount", stopDefectSheetCnt.ToString());

            XmlHelper.SetValue(xmlElement, "RepeatInspectbyZone", repeatInspectbyZone.ToString());


        }

        public override void Load(XmlElement xmlElement)
        {
            base.Load(xmlElement);

            if (xmlElement == null)
                return;


            //targetIntensity = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "TargetIntensity", targetIntensity.ToString()));
            //targetIntensityVal = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "TargetIntensityVal", targetIntensityVal.ToString()));
            //initialTopLightValue = XmlHelper.GetValue(xmlElement, "InitialTopLightValue", initialTopLightValue);
            //backLightMultiplier = XmlHelper.GetValue(xmlElement, "BackLightMultiplier", backLightMultiplier);
            //fovMultipication = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "FovMultipication", fovMultipication.ToString()));
            //invalidateBound = XmlHelper.GetValue(xmlElement, "InvalidateBound", invalidateBound);

            //asyncMode = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "AsyncMode", asyncMode.ToString()));
            //asyncGrabHz = Convert.ToSingle(XmlHelper.GetValue(xmlElement, "AsyncGrabHz", asyncGrabHz.ToString()));

            //this.autoResetAlarm = XmlHelper.GetValue(xmlElement, "AutoResetAlarm", this.autoResetAlarm);
            //this.modelAutoChange = XmlHelper.GetValue(xmlElement, "ModelAutoChange", this.modelAutoChange);

            //userStopMode = Convert.ToBoolean(XmlHelper.GetValue(xmlElement, "UserStopMode", userStopMode.ToString()));
            //stopDefectSheetCnt = XmlHelper.GetValue(xmlElement, "StopDefectSheetCount", stopDefectSheetCnt);

            repeatInspectbyZone = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "RepeatInspectbyZone", repeatInspectbyZone.ToString()));

        }
    }
}
