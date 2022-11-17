using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;
using UniScanS.Common;

namespace UniScanS.Screen.Vision
{
    public class AlgorithmSetting : IAlgorithmParamChangedListener
    {
        static AlgorithmSetting _instance;
        public static AlgorithmSetting Instance()
        {
            if (_instance == null)
                _instance = new AlgorithmSetting();

            return _instance;
        }
        
        int maxDefectNum;
        public int MaxDefectNum
        {
            get { return maxDefectNum; }
            set { maxDefectNum = value; }
        }
        
        int poleLowerWeight;
        public int PoleLowerWeight
        {
            get { return poleLowerWeight; }
            set { poleLowerWeight = value; }
        }

        int poleUpperWeight;
        public int PoleUpperWeight
        {
            get { return poleUpperWeight; }
            set { poleUpperWeight = value; }
        }

        float dielectricCompactness;
        public float DielectricCompactness
        {
            get { return dielectricCompactness; }
            set { dielectricCompactness = value; }
        }

        float poleCompactness;
        public float PoleCompactness
        {
            get { return poleCompactness; }
            set { poleCompactness = value; }
        }

        float dielectricElongation;
        public float DielectricElongation
        {
            get { return dielectricElongation; }
            set { dielectricElongation = value; }
        }

        float poleElongation;
        public float PoleElongation
        {
            get { return poleElongation; }
            set { poleElongation = value; }
        }

        int dielectricLowerWeight;
        public int DielectricLowerWeight
        {
            get { return dielectricLowerWeight; }
            set { dielectricLowerWeight = value; }
        }

        int dielectricUpperWeight;
        public int DielectricUpperWeight
        {
            get { return dielectricUpperWeight; }
            set { dielectricUpperWeight = value; }
        }

        float xPixelCal;
        public float XPixelCal
        {
            get { return xPixelCal; }
            set { xPixelCal = value; }
        }

        float yPixelCal;
        public float YPixelCal
        {
            get { return yPixelCal; }
            set { yPixelCal = value; }
        }

        int sheetAttackMinSize;
        public int SheetAttackMinSize
        {
            get { return sheetAttackMinSize; }
            set { sheetAttackMinSize = value; }
        }

        int poleMinSize;
        public int PoleMinSize
        {
            get { return poleMinSize; }
            set { poleMinSize = value; }
        }

        int dielectricMinSize;
        public int DielectricMinSize
        {
            get { return dielectricMinSize; }
            set { dielectricMinSize = value; }
        }

        int pinHoleMinSize;
        public int PinHoleMinSize
        {
            get { return pinHoleMinSize; }
            set { pinHoleMinSize = value; }
        }

        float defectDistance;
        public float DefectDistance
        {
            get { return defectDistance; }
            set { defectDistance = value; }
        }

        public AlgorithmSetting()
        {
            maxDefectNum = 1000;
            
            poleLowerWeight = 110;
            poleUpperWeight = 150;

            dielectricLowerWeight = 150;
            dielectricUpperWeight = 150;

            xPixelCal = 10;
            yPixelCal = 10;

            sheetAttackMinSize = 120;
            poleMinSize = 70;
            dielectricMinSize = 60;
            pinHoleMinSize = 60;

            poleCompactness = 1.5f;
            dielectricCompactness = 1.5f;

            poleElongation = 1.5f;
            dielectricElongation = 1.5f;

            defectDistance = 100;

            if (SystemManager.Instance().ExchangeOperator is IClientExchangeOperator)
            {
                IClientExchangeOperator client = SystemManager.Instance().ExchangeOperator as IClientExchangeOperator;
                client.AddAlgorithmParamChangedListener(this);
            }
        }

        public void Load(string filePath = null)
        {
            string configFileName = String.Format(@"{0}\AlgorithmSetting.xml", PathSettings.Instance().Config);
            if (filePath != null)
            {
                String.Format(@"{0}\AlgorithmSetting.xml", PathSettings.Instance().Config);
            }

            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;

            XmlElement configXmlDocument = xmlDocument["Algorithm"];
            
            maxDefectNum = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "MaxDefectNum", "1000"));
            
            poleLowerWeight = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "PoleLowerWeight", "100"));
            poleUpperWeight = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "PoleUpperWeight", "100"));

            dielectricLowerWeight = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "DielectricLowerWeight", "100"));
            dielectricUpperWeight = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "DielectricUpperWeight", "100"));

            xPixelCal = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "XPixelCal", "10"));
            yPixelCal = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "YPixelCal", "10"));

            sheetAttackMinSize = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "SheetAttackMinSize", "40"));
            poleMinSize = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "PoleMinSize", "40"));
            dielectricMinSize = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "DielectricMinSize", "40"));
            pinHoleMinSize = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "PinHoleMinSize", "40"));
            
            poleCompactness = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "PoleCompactness", "1"));
            dielectricCompactness = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "DielectricCompactness", "1"));

            poleElongation = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "PoleElongation", "1"));
            dielectricElongation = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "DielectricElongation", "1"));

            defectDistance = Convert.ToSingle(XmlHelper.GetValue(configXmlDocument, "DefectDistance", "100"));
        }

        public void Save(string path = null)
        {
            string configFileName = String.Format(@"{0}\AlgorithmSetting.xml", PathSettings.Instance().Config);

            if (path != null)
            {
                configFileName = String.Format(@"{0}\Config\AlgorithmSetting.xml", path);
            }

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement configXmlDocument = xmlDocument.CreateElement("Algorithm");
            xmlDocument.AppendChild(configXmlDocument);
            
            XmlHelper.SetValue(configXmlDocument, "MaxDefectNum", maxDefectNum.ToString());
            
            XmlHelper.SetValue(configXmlDocument, "PoleLowerWeight", poleLowerWeight.ToString());
            XmlHelper.SetValue(configXmlDocument, "PoleUpperWeight", poleUpperWeight.ToString());

            XmlHelper.SetValue(configXmlDocument, "DielectricLowerWeight", dielectricLowerWeight.ToString());
            XmlHelper.SetValue(configXmlDocument, "DielectricUpperWeight", dielectricUpperWeight.ToString());

            XmlHelper.SetValue(configXmlDocument, "SheetAttackMinSize", sheetAttackMinSize.ToString());
            XmlHelper.SetValue(configXmlDocument, "PoleMinSize", poleMinSize.ToString());
            XmlHelper.SetValue(configXmlDocument, "DielectricMinSize", dielectricMinSize.ToString());
            XmlHelper.SetValue(configXmlDocument, "PinHoleMinSize", pinHoleMinSize.ToString());
            XmlHelper.SetValue(configXmlDocument, "XPixelCal", xPixelCal.ToString());
            XmlHelper.SetValue(configXmlDocument, "YPixelCal", yPixelCal.ToString());
            
            XmlHelper.SetValue(configXmlDocument, "PoleCompactness", poleCompactness.ToString());
            XmlHelper.SetValue(configXmlDocument, "DielectricCompactness", dielectricCompactness.ToString());

            XmlHelper.SetValue(configXmlDocument, "PoleElongation", poleElongation.ToString());
            XmlHelper.SetValue(configXmlDocument, "DielectricElongation", dielectricElongation.ToString());

            XmlHelper.SetValue(configXmlDocument, "DefectDistance", defectDistance.ToString());

            XmlHelper.Save(xmlDocument, configFileName);
        }

        public void AlgorithmParamChanged()
        {
            Load();
        }
    }
}
