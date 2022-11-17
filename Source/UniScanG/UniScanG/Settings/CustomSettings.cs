using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanG.Settings
{
    public class CheckPointSetting
    {
        int num;
        float ratio;

        public int Num
        {
            get { return num; }
            set { num = value; }
        }
        public float Ratio
        {
            get { return ratio; }
            set { ratio = value; }
        }
        public void Load(XmlElement checkPointSettingElement)
        {
            if (checkPointSettingElement == null)
                return;
            
            num = Convert.ToInt32(XmlHelper.GetValue(checkPointSettingElement, "Num", "0"));
            ratio = Convert.ToSingle(XmlHelper.GetValue(checkPointSettingElement, "Ratio", "0"));
        }

        public void Save(XmlElement checkPointSettingElement)
        {
            if (checkPointSettingElement == null)
                return;

            XmlHelper.SetValue(checkPointSettingElement, "Num", num.ToString());
            XmlHelper.SetValue(checkPointSettingElement, "Ratio", ratio.ToString());
        }
    }

    public class SamePointSetting
    {
        int num;
        float ratio;
        bool useSheetAttack;
        bool useBlackCircle;
        bool useBlackLine;
        bool usePinHole;
        bool useWhite;
        bool useShape;

        public int Num
        {
            get { return num; }
            set { num = value; }
        }
        public float Ratio
        {
            get { return ratio; }
            set { ratio = value; }
        }
        public bool UseSheetAttack
        {
            get { return useSheetAttack; }
            set { useSheetAttack = value; }
        }
        public bool UseBlackCircle
        {
            get { return useBlackCircle; }
            set { useBlackCircle = value; }
        }
        public bool UseBlackLine
        {
            get { return useBlackLine; }
            set { useBlackLine = value; }
        }
        public bool UsePinHole
        {
            get { return usePinHole; }
            set { usePinHole = value; }
        }
        public bool UseWhite
        {
            get { return useWhite; }
            set { useWhite = value; }
        }
        public bool UseShape
        {
            get { return useShape; }
            set { useShape = value; }
        }

        public void Load(XmlElement samePointSettingElement)
        {
            if (samePointSettingElement == null)
                return;

            num = Convert.ToInt32(XmlHelper.GetValue(samePointSettingElement, "Num", "0"));
            ratio = Convert.ToSingle(XmlHelper.GetValue(samePointSettingElement, "Ratio", "0"));

            useSheetAttack = Convert.ToBoolean(XmlHelper.GetValue(samePointSettingElement, "UseSheetAttack", "false"));
            useBlackCircle = Convert.ToBoolean(XmlHelper.GetValue(samePointSettingElement, "UseBlackCircle", "false"));
            useBlackLine = Convert.ToBoolean(XmlHelper.GetValue(samePointSettingElement, "UseBlackLine", "false"));
            usePinHole = Convert.ToBoolean(XmlHelper.GetValue(samePointSettingElement, "UsePinHole", "false"));
            useWhite = Convert.ToBoolean(XmlHelper.GetValue(samePointSettingElement, "UseWhite", "false"));
            useShape = Convert.ToBoolean(XmlHelper.GetValue(samePointSettingElement, "UseShape", "false"));
        }

        public void Save(XmlElement checkPointSettingElement)
        {
            if (checkPointSettingElement == null)
                return;

            XmlHelper.SetValue(checkPointSettingElement, "Num", num.ToString());
            XmlHelper.SetValue(checkPointSettingElement, "Ratio", ratio.ToString());
        }
    }

    public class CustomSettings
    {
        static CustomSettings _instance;
        public static CustomSettings Instance()
        {
            if (_instance == null)
                _instance = new CustomSettings();

            return _instance;
        }

        int maxShowDefectNum = 100;
        public int MaxShowDefectNum
        {
            get { return maxShowDefectNum; }
            set { maxShowDefectNum = value; }
        }

        bool useIOOutput;
        public bool UseIOOutput
        {
            get { return useIOOutput; }
            set { useIOOutput = value; }
        }

        bool useAlarmForm;
        public bool UseAlarmForm
        {
            get { return useAlarmForm; }
            set { useAlarmForm = value; }
        }

        bool showSamePointRadius;
        public bool ShowSamePointRadius
        {
            get { return showSamePointRadius; }
            set { showSamePointRadius = value; }
        }

        float samePointRadius;
        public float SamePointRadius
        {
            get { return samePointRadius; }
            set { samePointRadius = value; }
        }

        bool useWholeDefectIO;
        public bool UseWholeDefectIO
        {
            get { return useWholeDefectIO; }
            set { useWholeDefectIO = value; }
        }

        int wholeDefectNum;
        public int WholeDefectNum
        {
            get { return wholeDefectNum; }
            set { wholeDefectNum = value; }
        }

        float wholeDefectRatio;
        public float WholeDefectRatio
        {
            get { return wholeDefectRatio; }
            set { wholeDefectRatio = value; }
        }

        List<CheckPointSetting> checkPointSettingList = new List<CheckPointSetting>();
        public List<CheckPointSetting> CheckPointSettingList
        {
            get { return checkPointSettingList; }
            set { checkPointSettingList = value; }
        }

        List<SamePointSetting> samePointSettingList = new List<SamePointSetting>();
        public List<SamePointSetting> SamePointSettingList
        {
            get { return samePointSettingList; }
            set { samePointSettingList = value; }
        }

        public void Load()
        {
            string configFileName = String.Format(@"{0}\CustomConfig(Monitoring).xml", PathSettings.Instance().Config);
            
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;
                                
            XmlElement configXmlDocument = xmlDocument["Config"];

            maxShowDefectNum = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "MaxShowDefectNum", "100"));

            useIOOutput = Convert.ToBoolean(XmlHelper.GetValue(configXmlDocument, "UseIOOutput", "false"));
            useAlarmForm = Convert.ToBoolean(XmlHelper.GetValue(configXmlDocument, "UseAlarmForm", "false"));

            useWholeDefectIO = Convert.ToBoolean(XmlHelper.GetValue(configXmlDocument, "UseWholeDefectIO", "false"));
            wholeDefectNum = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "WholeDefectNum", "50"));
            wholeDefectRatio = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "WholeDefectRatio", "80"));
            
            showSamePointRadius = Convert.ToBoolean(XmlHelper.GetValue(configXmlDocument, "ShowSamePointRadius", "false"));
            samePointRadius = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "SamePointRadius", "100"));
            
            foreach (XmlElement checkPointSettingElement in configXmlDocument)
            {
                if (checkPointSettingElement.Name == "CheckPointSetting")
                {
                    CheckPointSetting checkPointSetting = new CheckPointSetting();
                    checkPointSetting.Load(checkPointSettingElement);

                    checkPointSettingList.Add(checkPointSetting);
                }
            }

            foreach (XmlElement samePointSettingElement in configXmlDocument)
            {
                if (samePointSettingElement.Name == "SamePointSetting")
                {
                    SamePointSetting samePointSetting = new SamePointSetting();
                    samePointSetting.Load(samePointSettingElement);

                    samePointSettingList.Add(samePointSetting);
                }
            }
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\CustomConfig(Monitoring).xml", PathSettings.Instance().Config);
            
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement configXmlDocument = xmlDocument.CreateElement("", "Config", "");
            xmlDocument.AppendChild(configXmlDocument);

            XmlHelper.SetValue(configXmlDocument, "MaxShowDefectNum", maxShowDefectNum.ToString());

            XmlHelper.SetValue(configXmlDocument, "UseIOOutput", useIOOutput.ToString());
            XmlHelper.SetValue(configXmlDocument, "UseAlarmForm", useAlarmForm.ToString());

            XmlHelper.SetValue(configXmlDocument, "WholeDefectNum", wholeDefectNum.ToString());
            XmlHelper.SetValue(configXmlDocument, "WholeDefectRatio", wholeDefectRatio.ToString());
            XmlHelper.SetValue(configXmlDocument, "UseWholeDefectIO", useWholeDefectIO.ToString());

            XmlHelper.SetValue(configXmlDocument, "ShowSamePointRadius", showSamePointRadius.ToString());
            XmlHelper.SetValue(configXmlDocument, "SamePointRadius", samePointRadius.ToString());

            foreach (CheckPointSetting checkPointSetting in checkPointSettingList)
            {
                XmlElement checkPointSettingElement = xmlDocument.CreateElement("", "CheckPointSetting", "");
                configXmlDocument.AppendChild(checkPointSettingElement);

                checkPointSetting.Save(checkPointSettingElement);
            }

            foreach (SamePointSetting samePointSetting in samePointSettingList)
            {
                XmlElement samePointSettingElement = xmlDocument.CreateElement("", "SamePointSetting", "");
                configXmlDocument.AppendChild(samePointSettingElement);

                samePointSetting.Save(samePointSettingElement);
            }

            XmlHelper.Save(xmlDocument, configFileName);
        }
    }
}
