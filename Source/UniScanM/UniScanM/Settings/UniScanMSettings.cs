using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanM.Settings
{
    public enum EAutoStartMethod { Encoder, PLC }
    public enum EInspectionMode { Inspect, Monitor }
    public enum EOperationMode { Sequencial, Random }
    public abstract class UniScanMSettings : UniEye.Base.Settings.AdditionalSettings
    {
        float maximumLineSpeed = 80.0f;
        bool manaulStartAuthrizeCheck = true;
        bool programStartWithAutoMode = true;

        [LocalizedCategoryAttributeUniScanM("Base"), LocalizedDisplayNameAttributeUniScanM("Maximum Line Speed [m/m]")]
        public float MaximumLineSpeed
        {
            get { return maximumLineSpeed; }
           // set { maximumLineSpeed = value; }
        }

        [LocalizedCategoryAttributeUniScanM("Base"), LocalizedDisplayNameAttributeUniScanM("Save Debug Image")]
        public bool SaveDebugImage
        {
            get { return OperationSettings.Instance().SaveDebugImage; }
            set { OperationSettings.Instance().SaveDebugImage = value; }
        }

        // [Hyunseok] 메뉴얼 시작 시 로그인 및 경고 문구를 안보고 시작하고 싶다는 VOC에 의한 옵션
        [LocalizedCategoryAttributeUniScanM("Base"), LocalizedDisplayNameAttributeUniScanM("Manaul Start Authrize Check")]
        public bool ManaulStartAuthrizeCheck
        {
            get { return manaulStartAuthrizeCheck; }
            set { manaulStartAuthrizeCheck = value; }
        }

        // [Hyunseok] 프로그램 시작 시 Auto 모드로 시작할지에 대한 옵션
        [LocalizedCategoryAttributeUniScanM("Base"), LocalizedDisplayNameAttributeUniScanM("Program Start with AutoMode")]
        public bool ProgramStartWithAutoMode
        {
            get { return programStartWithAutoMode; }
            set { programStartWithAutoMode = value; }
        }

        public new static UniScanMSettings Instance()
        {
            return instance as UniScanMSettings;
        }

        public override void Save(XmlElement xmlElement)
        {
            if (xmlElement == null)
                return;

            XmlHelper.SetValue(xmlElement, "MaximumLineSpeed", maximumLineSpeed.ToString());
            XmlHelper.SetValue(xmlElement, "ManaulStartAuthrizeCheck", manaulStartAuthrizeCheck.ToString());
            XmlHelper.SetValue(xmlElement, "ProgramStartWithAutoMode", programStartWithAutoMode.ToString());
        }

        public override void Load(XmlElement xmlElement)
        {
            if (xmlElement == null)
                return;

            maximumLineSpeed = XmlHelper.GetValue(xmlElement, "MaximumLineSpeed", maximumLineSpeed);
            manaulStartAuthrizeCheck = XmlHelper.GetValue(xmlElement, "ManaulStartAuthrizeCheck", manaulStartAuthrizeCheck);
            programStartWithAutoMode = XmlHelper.GetValue(xmlElement, "ProgramStartWithAutoMode", programStartWithAutoMode);
        }
    }
}
