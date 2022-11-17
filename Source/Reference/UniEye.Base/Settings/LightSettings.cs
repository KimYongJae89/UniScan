using DynMvp.Base;
using DynMvp.Devices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UniEye.Base.Settings
{
    public class LightSettings
    {
        private List<string> lightNameList = new List<string>();
        public List<string> LightNameList
        {
            get { return lightNameList; }
            set { lightNameList = value; }
        }

        // 최종 설정된 조명 설정값으로 적용
        private LightParamSet lightParamSet = new LightParamSet();
        public LightParamSet LightParamSet
        {
            get { return lightParamSet; }
            set { lightParamSet = value; }
        }

        int numLight;
        public int NumLight
        {
            get { return numLight; }
            set { numLight = value; }
        }

        int numDeviceLight;
        public int NumDeviceLight
        {
            get { return numDeviceLight; }
            set { numDeviceLight = value; }
        }

        private int numLightType = 1;
        public int NumLightType
        {
            get { return numLightType; }
            set { numLightType = value; }
        }

        static LightSettings _instance;
        public static LightSettings Instance()
        {
            if (_instance == null)
            {
                _instance = new LightSettings();
                _instance.LoadDefault();
                _instance.Load();
            }

            return _instance;
        }

        private LightSettings()
        {

        }

        void LoadDefault()
        {
            this.numLight = MachineSettings.Instance().NumLight;
            this.numLightType = MachineSettings.Instance().NumLightType;
            
            lightNameList.Clear();
            for (int i = 0; i < NumLight; i++)
            {
                lightNameList.Add(string.Format("Light {0}", i + 1));
            }

            lightParamSet.Initialize(numLight, numLightType);

            return;
        }

        public void Load()
        {
            string fileName = String.Format(@"{0}\LightConfig.xml", PathSettings.Instance().Config);

            lightParamSet.Initialize(numLight, numLightType);

            if (File.Exists(fileName) == false)
                return;

            XmlDocument xmlDocument = XmlHelper.Load(fileName);
            if (xmlDocument == null)
            {
                LoadDefault();
                return;
            }

            XmlElement lightNamesElement = xmlDocument["LightNames"];
            if (lightNamesElement != null)
            {
                lightNameList.Clear();
                for (int i = 0; i < NumLight; i++)
                {
                    lightNameList.Add(XmlHelper.GetValue(lightNamesElement, string.Format("LightName{0}", i), string.Format("Light {0}", i + 1)));
                }
            }

            XmlElement lightParamSetElement = xmlDocument["LightParamSet"];
            if (lightParamSetElement != null)
            {
                lightParamSet.Load(lightParamSetElement);
            }
        }

        public void UpdateLightNames()
        {
            List<string> lightNames = new List<string>();

            for (int i = 0; i < numLight; i++)
            {
                if (lightNameList.Count > i)
                    lightNames.Add(lightNameList[i]);
                else
                    lightNames.Add(String.Format("Light {0}", i + 1));
            }

            lightNameList = lightNames;
        }

        public void Save()
        {
            string fileName = String.Format(@"{0}\LightConfig.xml", PathSettings.Instance().Config);

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement lightNamesElement = xmlDocument.CreateElement("", "LightNames", "");
            xmlDocument.AppendChild(lightNamesElement);

            for (int i = 0; i < NumLight; i++)
            {
                if (i >= lightNameList.Count)
                    lightNameList.Add(string.Format("Light {0}", i));

                XmlHelper.SetValue(lightNamesElement, string.Format("LightName{0}", i), lightNameList[i]);
            }

            if (lightParamSet != null)
            {
                XmlElement lightParamSetElement = xmlDocument.CreateElement("", "LightParamSet", "");
                xmlDocument.AppendChild(lightParamSetElement);

                lightParamSet.Save(lightParamSetElement);
            }

            XmlHelper.Save(xmlDocument, fileName);
        }
    }
}
