using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanS.Screen.Vision
{
    class InspectorSetting
    {
        static InspectorSetting _instance;
        public static InspectorSetting Instance()
        {
            if (_instance == null)
                _instance = new InspectorSetting();

            return _instance;
        }

        int removalNum;
        public int RemovalNum
        {
            get { return removalNum; }
            set { removalNum = value; }
        }

        int gridColNum;
        public int GridColNum
        {
            get { return gridColNum; }
            set { gridColNum = value; }
        }

        int gridRowNum;
        public int GridRowNum
        {
            get { return gridRowNum; }
            set { gridRowNum = value; }
        }

        bool isFiducial;
        public bool IsFiducial
        {
            get { return isFiducial; }
            set { isFiducial = value; }
        }

        public InspectorSetting()
        {
            removalNum = 6;

            gridColNum = 10;
            gridRowNum = 10;
            
            isFiducial = false;
        }

        public void Load()
        {
            string configFileName = String.Format(@"{0}\InspectorSetting.xml", PathSettings.Instance().Config);
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;

            XmlElement configXmlDocument = xmlDocument["Algorithm"];

            removalNum = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "RemovalNum", "6"));
            gridColNum = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "GridColNum", "10"));
            gridRowNum = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "GridRowNum", "10"));
            
            isFiducial = Convert.ToBoolean(XmlHelper.GetValue(configXmlDocument, "IsFiducial", "false"));
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\InspectorSetting.xml", PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement configXmlDocument = xmlDocument.CreateElement("Algorithm");
            xmlDocument.AppendChild(configXmlDocument);

            XmlHelper.SetValue(configXmlDocument, "RemovalNum", removalNum.ToString());
            XmlHelper.SetValue(configXmlDocument, "GridColNum", gridColNum.ToString());
            XmlHelper.SetValue(configXmlDocument, "GridRowNum", gridRowNum.ToString());
            
            XmlHelper.SetValue(configXmlDocument, "IsFiducial", isFiducial.ToString());

            XmlHelper.Save(xmlDocument, configFileName);
        }
    }
}
