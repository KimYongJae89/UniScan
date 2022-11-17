using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base.Settings;
using UniScanS.Common;
using UniScanS.Data;

namespace UniScanS.Screen.Data
{
    public class MonitorSetting
    {
        ErrorChecker errorChecker = new ErrorChecker();

        List<AlarmChecker> alarmCheckerList;

        int signalTime;
        bool useAlarmOutput;
        int errorNum;
        float sameDistance;

        public List<AlarmChecker> AlarmCheckerList { get => alarmCheckerList; set => alarmCheckerList = value; }

        public int SignalTime { get => signalTime; set => signalTime = value; }
        public int ErrorNum { get => errorNum; set => errorNum = value; }
        public bool UseAlarmOutput { get => useAlarmOutput; set => useAlarmOutput = value; }

        public ErrorChecker ErrorChecker { get => errorChecker; set => errorChecker = value; }

        static MonitorSetting _instance;
        public static MonitorSetting Instance()
        {
            if (_instance == null)
                _instance = new MonitorSetting();

            return _instance;
        }
        
        public MonitorSetting()
        {
            AlarmCheckerList = new List<AlarmChecker>();

            signalTime = 10;
            useAlarmOutput = false;
        }

        public void Load()
        {
            string configFileName = String.Format(@"{0}\MonitorSetting.xml", PathSettings.Instance().Config);

            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;

            XmlElement configXmlDocument = xmlDocument["Monitor"];

            signalTime = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "SignalTime", "10"));
            useAlarmOutput = Convert.ToBoolean(XmlHelper.GetValue(configXmlDocument, "UseAlarmOutput", "true"));
            errorNum = Convert.ToInt32(XmlHelper.GetValue(configXmlDocument, "ErrorNum", "0"));
            foreach (XmlElement alarmElement in configXmlDocument)
            {
                if (alarmElement.Name == "Alarm")
                {
                    alarmCheckerList.Add(AlarmChecker.LoadAlarmChecker(alarmElement));
                }
            }
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\MonitorSetting.xml", PathSettings.Instance().Config);
            
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement configXmlDocument = xmlDocument.CreateElement("Monitor");
            xmlDocument.AppendChild(configXmlDocument);
            
            XmlHelper.SetValue(configXmlDocument, "SignalTime", signalTime.ToString());
            XmlHelper.SetValue(configXmlDocument, "UseAlarmOutput", useAlarmOutput.ToString());
            XmlHelper.SetValue(configXmlDocument, "ErrorNum", errorNum.ToString());

            foreach (AlarmChecker alarmChecker in alarmCheckerList)
            {
                XmlElement alarmXmlDocument = xmlDocument.CreateElement("Alarm");
                alarmChecker.Save(alarmXmlDocument);
                configXmlDocument.AppendChild(alarmXmlDocument);
            }

            XmlHelper.Save(xmlDocument, configFileName);
        }
    }
}
