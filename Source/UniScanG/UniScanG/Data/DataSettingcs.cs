using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;

namespace UniScanG.Data
{
    class DataSetting
    {
        int maxShowResultNum;
        public int MaxShowResultNum
        {
            get { return maxShowResultNum; }
            set { maxShowResultNum = value; }
        }

        static DataSetting _instance;
        public static DataSetting Instance()
        {
            if (_instance == null)
                _instance = new DataSetting();

            return _instance;
        }

        public DataSetting()
        {
            maxShowResultNum = 1000;
        }

        public void Load()
        {
            string configFileName = String.Format(@"{0}\DataSetting.xml", PathSettings.Instance().Config);
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;
            
            XmlElement configXmlDocument = xmlDocument["Data"];

            maxShowResultNum = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "MaxShowResultNum", "1000"));
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\DataSetting.xml", PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement configXmlDocument = xmlDocument.CreateElement("Data");
            xmlDocument.AppendChild(configXmlDocument);

            XmlHelper.SetValue(configXmlDocument, "MaxShowResultNum", maxShowResultNum.ToString());

            XmlHelper.Save(xmlDocument, configFileName);
        }
    }
}
