using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.Light;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using UniScanWPF.Screen.PinHoleColor.Color.Inspect;
using UniScanWPF.Screen.PinHoleColor.Device;
using UniScanWPF.Screen.PinHoleColor.Inspect;
using UniScanWPF.Screen.PinHoleColor.PinHole.Inspect;
using UniScanWPF.Screen.PinHoleColor.Properties;

namespace UniScanWPF.Screen.PinHoleColor.Data
{
    public enum DetectorType
    {
        PinHole, Color
    }

    public class Model : DynMvp.Data.Model
    {
        private Dictionary<ImageDevice, DetectorParam> deviceDictionary = new Dictionary<ImageDevice, DetectorParam>();

        string[] visibleStr;
        public string[] VisibleStr
        {
            get { return visibleStr; }
        }

        Brush[] brushes;
        public Brush[] Brushes
        {
            get { return brushes; }
        }

        public override DynMvp.Data.ModelDescription ModelDescription
        {
            get { return modelDescription; }
            set
            {
                modelDescription = value;
                for (int i = 0; i < modelDescription.Name.Length; i++)
                {
                    if (modelDescription.Name[i] == '0')
                    {
                        visibleStr[i] = null;
                        brushes[i] = System.Windows.Media.Brushes.LightGray;// App.Current.Resources["RedBrush"] as Brush;
                    }
                    else
                    {
                        visibleStr[i] = (i + 1).ToString();
                        brushes[i] = App.Current.Resources["GreenBrush"] as Brush;
                    }
                        
                }
            }
        }
        
        public Dictionary<ImageDevice, DetectorParam> DeviceDictionary
        {
            get => deviceDictionary;
            set => deviceDictionary = value;
        }

        public int PinHoleLightValue
        {
            get
            {
                if (this.LightParamSet.LightParamList[0].LightValue.NumLight < 1)
                    return -1;

                return this.LightParamSet.LightParamList[0].LightValue.Value[0];
            }

            set
            {
                if (this.LightParamSet.LightParamList[0].LightValue.NumLight < 1)
                    return;

                this.LightParamSet.LightParamList[0].LightValue.Value[0] = value;
                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn(this.LightParamSet.LightParamList[0]);
            }
        }

        public int ColorLightValue
        {
            get
            {
                if (this.LightParamSet.LightParamList[0].LightValue.NumLight < 2)
                    return -1;

                return this.LightParamSet.LightParamList[0].LightValue.Value[1];
            }

            set
            {
                if (this.LightParamSet.LightParamList[0].LightValue.NumLight < 2)
                    return;

                this.LightParamSet.LightParamList[0].LightValue.Value[1] = value;
                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOn(this.LightParamSet.LightParamList[0]);
            }
        }

        public Model()
        {
            brushes = new Brush[3];
            visibleStr = new string[3];

            foreach (ImageDevice device in SystemManager.Instance().DeviceBox.ImageDeviceHandler)
            {
                deviceDictionary.Add(device, new PinHoleDetectorParam());
            }

            this.LightParamSet.LightParam3d = new LightParam(SystemManager.Instance().DeviceBox.LightCtrlHandler.NumLight, 100);
            this.LightParamSet.LightParamList.Add(new LightParam(SystemManager.Instance().DeviceBox.LightCtrlHandler.NumLight, 100));
        }

        public override bool IsTaught()
        {
            return deviceDictionary.All(x => x.Value != null);
        }
        
        private void LoadDeviceElement(XmlElement deviceElement)
        {
            Dictionary<ImageDevice, DetectorParam> loadedDeviceDictionary = new Dictionary<ImageDevice, DetectorParam>();

            foreach (ImageDevice device in deviceDictionary.Keys)
            {
                XmlElement detectorElement = deviceElement[string.Format("Camera{0}", device.Index)];
                if (detectorElement == null)
                {
                    loadedDeviceDictionary.Add(device, new PinHoleDetectorParam());
                    continue;
                }

                DetectorType type = (DetectorType)Enum.Parse(typeof(DetectorType), XmlHelper.GetValue(detectorElement, "DetectorType", DetectorType.PinHole.ToString()));
                DetectorParam detectorParam = null;
                switch (type)
                {
                    case DetectorType.PinHole:
                        detectorParam = new PinHoleDetectorParam();
                        break;
                    case DetectorType.Color:
                        detectorParam = new ColorDetectorParam();
                        break;
                }

                detectorParam.Load(detectorElement);
                loadedDeviceDictionary.Add(device, detectorParam);
            }

            deviceDictionary = loadedDeviceDictionary;
        }

        private void LoadLightElement(XmlElement lightElement)
        {
            this.LightParamSet.Load(lightElement);
        }

        public override void LoadModel(XmlElement xmlElement)
        {
            XmlElement deviceElement = xmlElement["Device"];
            if (deviceElement != null)
                LoadDeviceElement(deviceElement);

            XmlElement lightElement = xmlElement["Light"];
            if (lightElement != null)
                LoadLightElement(lightElement);
        }

        private void SaveDeviceElement(XmlElement deviceElement)
        {
            foreach (ImageDevice device in deviceDictionary.Keys)
            {
                XmlElement detectorElement = deviceElement.OwnerDocument.CreateElement("", string.Format("Camera{0}", device.Index), "");
                if (deviceDictionary[device] is PinHoleDetectorParam)
                    XmlHelper.SetValue(detectorElement, "DetectorType", DetectorType.PinHole.ToString());
                else
                    XmlHelper.SetValue(detectorElement, "DetectorType", DetectorType.Color.ToString());

                deviceDictionary[device].Save(detectorElement);;

                deviceElement.AppendChild(detectorElement);
            }
        }

        private void SaveLightElement(XmlElement lightElement)
        {
            this.LightParamSet.Save(lightElement);
        }

        public override void SaveModel(XmlElement xmlElement)
        {
            XmlElement deviceElement = xmlElement.OwnerDocument.CreateElement("", "Device", "");
            SaveDeviceElement(deviceElement);
            xmlElement.AppendChild(deviceElement);

            XmlElement lightElement = xmlElement.OwnerDocument.CreateElement("", "Light", "");
            SaveLightElement(lightElement);
            xmlElement.AppendChild(lightElement);
        }
    }
}
