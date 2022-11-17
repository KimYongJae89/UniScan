using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace UniEye.Base.Settings
{
    // UI Customize Options
    // 장비 운영 중 변경되지 않아야 하는 설정. 장비 타입 및 운영 환경 설정에 필요한 파라미터
    public class CustomizeSettings
    {
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string programTitle;
        public string ProgramTitle
        {
            get { return programTitle; }
            set { programTitle = value; }
        }

        private string copyright;
        public string Copyright
        {
            get { return copyright; }// string.IsNullOrEmpty(copyright) ? "UniEye" : copyright; }
            set { copyright = value; }
        }

        /// <summary>
        /// Touch UI 적용시 화면 키보드 사용 여부 설정
        /// </summary>
        private bool useUpDownControl;
        public bool UseUpDownControl
        {
            get { return useUpDownControl; }
            set { useUpDownControl = value; }
        }

        private bool useReportPage;
        public bool UseReportPage
        {
            get { return useReportPage; }
            set { useReportPage = value; }
        }

        private bool useLiveCam;
        public bool UseLiveCam
        {
            get { return useLiveCam; }
            set { useLiveCam = value; }
        }

        private bool showSelector;
        public bool ShowSelector
        {
            get { return showSelector; }
            set { showSelector = value; }
        }

        private int numOfResultView = 1;
        public int NumOfResultView
        {
            get { return numOfResultView; }
            set { numOfResultView = value; }
        }

        bool useFovNavigator;
        public bool UseFovNavigator
        {
            get { return useFovNavigator; }
            set { useFovNavigator = value; }
        }

        static CustomizeSettings _instance;
        public static CustomizeSettings Instance()
        {
            if (_instance == null)
            {
                _instance = new CustomizeSettings();
                _instance.Load();
            }

            return _instance;
        }

        private CustomizeSettings()
        {

        }

        public void Save()
        {
            string fileName = String.Format(@"{0}\customSettings.xml", PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement customizeElement = xmlDocument.CreateElement("", "Customize", "");
            xmlDocument.AppendChild(customizeElement);

            XmlHelper.SetValue(customizeElement, "UseUpDownControl", useUpDownControl.ToString());
            XmlHelper.SetValue(customizeElement, "Title", title);
            XmlHelper.SetValue(customizeElement, "ProgramTitle", programTitle);
            XmlHelper.SetValue(customizeElement, "Copyright", copyright);
            XmlHelper.SetValue(customizeElement, "UseReportPage", useReportPage.ToString());
            XmlHelper.SetValue(customizeElement, "UseLiveCam", useLiveCam.ToString());
            XmlHelper.SetValue(customizeElement, "ShowSelector", showSelector.ToString());
            XmlHelper.SetValue(customizeElement, "NumOfResultView", numOfResultView.ToString());
            XmlHelper.SetValue(customizeElement, "UseFovNavigator", useFovNavigator.ToString());

            XmlHelper.Save(xmlDocument, fileName);
        }

        public void Load()
        {
            string fileName = String.Format(@"{0}\customSettings.xml", PathSettings.Instance().Config);

            try
            {
                XmlDocument xmlDocument = XmlHelper.Load(fileName);
                if (xmlDocument == null)
                    return;

                XmlElement customizeElement = xmlDocument["Customize"];
                if (customizeElement == null)
                    return;

                useUpDownControl = Convert.ToBoolean(XmlHelper.GetValue(customizeElement, "UseUpDownControl", "False"));
                title = XmlHelper.GetValue(customizeElement, "Title", "UniEye");
                programTitle = XmlHelper.GetValue(customizeElement, "ProgramTitle", "UniEye");
                useReportPage = Convert.ToBoolean(XmlHelper.GetValue(customizeElement, "UseReportPage", "False"));
                useLiveCam = Convert.ToBoolean(XmlHelper.GetValue(customizeElement, "UseLiveCam", "False"));
                copyright = XmlHelper.GetValue(customizeElement, "Copyright", "2019 UniEye");
                showSelector = Convert.ToBoolean(XmlHelper.GetValue(customizeElement, "ShowSelector", "False"));
                numOfResultView = Convert.ToInt32(XmlHelper.GetValue(customizeElement, "NumOfResultView", "1"));
                useFovNavigator = Convert.ToBoolean(XmlHelper.GetValue(customizeElement, "UseFovNavigator", "true"));

                if (string.IsNullOrEmpty(copyright))
                    copyright = "2019 UniEye";

                MessageForm.DefaultTitleText = programTitle;
            }
            catch
            {
                Save();
            }
        }
    }
}
