using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using DynMvp.Data;
using UniEye.Base.Settings;
using DynMvp.UI.Touch;

using System.Data;
using DynMvp.Devices.Comm;
using UniScan.UI;

namespace UniScan
{
    public class SpectrometerSetting
    {
        float value;

        public void Load(XmlElement xmlElement)
        {
            value = Convert.ToSingle(xmlElement.GetAttribute("Value", "0"));
        }

        public void Save(XmlElement xmlElement)
        {
            xmlElement.SetAttribute("Value", value.ToString());
        }
    }

    public class PageInfo
    {
        string title;
        List<ResultPanelInfo> resultPanelInfoList = new List<ResultPanelInfo>();

        public int NumPanel { get { return resultPanelInfoList.Count; } }
        public string Title { get { return title; } set { title = value; } }
        public List<ResultPanelInfo> ResultPanelInfoList
        {
            get { return resultPanelInfoList; }
            set { resultPanelInfoList = value; }
        }

        public PageInfo()
        {
            Title = "";
            ResultPanelInfoList = new List<ResultPanelInfo>();
        }

        public PageInfo Clone()
        {
            PageInfo pageInfo = new PageInfo();

            pageInfo.title = this.title;

            pageInfo.resultPanelInfoList = new List<ResultPanelInfo>();
            for (int i = 0; i < this.resultPanelInfoList.Count; i++)
            {
                ResultPanelInfo resultPanelInfo = this.resultPanelInfoList[i].Clone();

                pageInfo.resultPanelInfoList.Add(resultPanelInfo);
            }

            return pageInfo;
        }

        public void Load(XmlElement xmlElement)
        {
            title = xmlElement.GetAttribute("Title");

            foreach (XmlElement panelInfoElement in xmlElement)
            {
                if (panelInfoElement.Name != "ResultPanelInfo")
                    continue;

                PanelType panelType = (PanelType)Enum.Parse(typeof(PanelType), panelInfoElement.GetAttribute("PanelType"));
                ResultPanelInfo resultPanelInfo = ResultPanelInfoFactory.Create(panelType);
                resultPanelInfo.Load(panelInfoElement);

                resultPanelInfoList.Add(resultPanelInfo);
            }
        }

        public void Save(XmlElement xmlElement)
        {
            xmlElement.SetAttribute("Title", title);

            foreach (ResultPanelInfo resultPanelInfo in resultPanelInfoList)
            {
                XmlElement resultPanelInfoElement = xmlElement.OwnerDocument.CreateElement("ResultPanelInfo");
                xmlElement.AppendChild(resultPanelInfoElement);

                resultPanelInfo.Save(resultPanelInfoElement);
            }
        }
    }

    // 필요한 운영 환경 변수 추가하면 됨.
    public class SystemSettings
    {
        static SystemSettings _instance;
        static string configFileName;

        SpectrometerSetting spectrometerSetting;
        public SpectrometerSetting SpectrometerSetting { get { return spectrometerSetting; } set { spectrometerSetting = value; } }

        // Sample
        int testValue;
        public int TestValue
        {
            get { return testValue; }
            set { testValue = value; }
        }

        List<PageInfo> pageInfoList = new List<PageInfo>();
        public List<PageInfo> PageInfoList
        {
            get { return pageInfoList; }
            set { pageInfoList = value; }
        }

        List<string> powderList = new List<string>();
        public List<string> PowderList
        {
            get { return powderList; }
            set { powderList = value; }
        }

        List<string> petList = new List<string>();
        public List<string> PetList
        {
            get { return petList; }
            set { petList = value; }
        }

        public static SystemSettings Instance()
        {
            if (_instance == null)
            {
                _instance = new SystemSettings();
            }

            return _instance;
        }

        public void Load()
        {
            // Chart
            pageInfoList.Clear();
            pageInfoList = new List<PageInfo>();

            string configFileName = String.Format(@"{0}\customConfig.xml", PathSettings.Instance().Config);
            XmlDocument xmlDocument = XmlHelper.Load(configFileName);

            if (xmlDocument == null)
                return;

            XmlElement configElement = xmlDocument["Config"];

            XmlElement pageInfoListElement = configElement["PageInfoList"];
            if (pageInfoListElement != null)
            {
                foreach (XmlElement pageInfoElement in pageInfoListElement)
                {
                    //XmlElement pageInfoElement = pageInfoListElement["PageInfo"];
                    if (pageInfoElement != null)
                    {
                        PageInfo pageInfo = new PageInfo();
                        pageInfo.Load(pageInfoElement);

                        pageInfoList.Add(pageInfo);
                    }
                }
            }

            // Spectrometer
            XmlElement spectrometerElement = configElement["Spectrometer"];
            if (spectrometerElement != null)
            {
                //khy
                //spectrometerSetting.Load(spectrometerElement);
            }

            // Model
            powderList.Clear();
            powderList = new List<string>();

            petList.Clear();
            petList = new List<string>();

            XmlElement modelListElement = configElement["ModelList"];
            if (modelListElement != null)
            {
                XmlElement powderListElement = modelListElement["PowderList"];
                if (powderListElement != null)
                {
                    for (int i = 0; i < powderListElement.Attributes.Count; i++)
                        powderList.Add(powderListElement.GetAttribute("Powder" + i.ToString()));
                }

                XmlElement petListElement = modelListElement["PETList"];
                if (petListElement != null)
                {
                    for (int i = 0; i < petListElement.Attributes.Count; i++)
                        petList.Add(petListElement.GetAttribute("PET" + i.ToString()));
                }
            }
        }

        public void Save()
        {
            string configFileName = String.Format(@"{0}\customConfig.xml", PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement configElement = xmlDocument.CreateElement("", "Config", "");
            xmlDocument.AppendChild(configElement);

            // Chart
            XmlElement pageInfoListElement = xmlDocument.CreateElement("", "PageInfoList", "");
            configElement.AppendChild(pageInfoListElement);

            foreach (PageInfo pageInfo in pageInfoList)
            {
                XmlElement pageInfoElement = xmlDocument.CreateElement("", "PageInfo", "");
                pageInfoListElement.AppendChild(pageInfoElement);

                pageInfo.Save(pageInfoElement);
            }

            // Spectrometer
            XmlElement spectrometerElement = xmlDocument.CreateElement("", "Spectrometer", "");
            configElement.AppendChild(spectrometerElement);

            // Model
            XmlElement modelListElement = xmlDocument.CreateElement("", "ModelList", "");
            configElement.AppendChild(modelListElement);

            XmlElement powderListElement = xmlDocument.CreateElement("", "PowderList", "");
            modelListElement.AppendChild(powderListElement);

            for (int i = 0; i < powderList.Count; i++)
            {
                powderListElement.SetAttribute("Powder" + i.ToString(), powderList[i]);
            }

            XmlElement petListElement = xmlDocument.CreateElement("", "PETList", "");
            modelListElement.AppendChild(petListElement);

            for (int i = 0; i < PetList.Count; i++)
            {
                petListElement.SetAttribute("PET" + i.ToString(), PetList[i]);
            }

            // khy
            //spectrometerSetting.Save(spectrometerElement);




            XmlHelper.Save(xmlDocument, configFileName);
        }
    }
}
