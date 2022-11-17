using System;
using System.Drawing;
using System.Linq;
using System.Xml;

using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Daq;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Device.Serial;
using DynMvp.Devices.MotionController;

using UniEye.Base.Device;
using UniEye.Base.Inspect;
using UniEye.Base.MachineInterface;
using System.Collections.Generic;
using DynMvp.Device.Device.Light;
using DynMvp.Device.Device.UPS;
using DynMvp.Device.Serial.Sensor;

namespace UniEye.Base.Settings
{
    public enum InterfaceType
    {
        None, Serial, TcpIpServer, TcpIpClient, FINS, Melsec, Xgt
    }

    public enum RunningMode
    {
        Real, Virtual, Demo
    }

    public class LightTypeData
    {
        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        LightParamType lightParamType;
        public LightParamType LightParamType
        {
            get { return lightParamType; }
            set { lightParamType = value; }
        }

        public LightTypeData(string name, LightParamType lightParamType)
        {
            this.name = name;
            this.lightParamType = lightParamType;
        }
    }

    [Serializable]
    public class MachineSettings
    {
        private int version;
        public int Version
        {
            get { return version; }
            set { version = value; }
        }

        // Virtual Mode가 설정되면, 카메라와 I/O가 각각 VirtualCamera
        private RunningMode runningMode;
        public RunningMode RunningMode
        {
            get { return runningMode; }
            set { runningMode = value; }
        }

        DigitalIoInfoList digitalIoInfoList = new DigitalIoInfoList();
        public DigitalIoInfoList DigitalIoInfoList
        {
            get { return digitalIoInfoList; }
            set { digitalIoInfoList = value; }
        }

        GrabberInfoList grabberInfoList = new GrabberInfoList();
        public GrabberInfoList GrabberInfoList
        {
            get { return grabberInfoList; }
            set { grabberInfoList = value; }
        }

        MotionInfoList motionInfoList = new MotionInfoList();
        public MotionInfoList MotionInfoList
        {
            get { return motionInfoList; }
            set { motionInfoList = value; }
        }

        public int NumCamera
        {
            get
            {
                return grabberInfoList.NumCamera;
            }
        }

        private int defaultExposureTimeMs = 2;
        public int DefaultExposureTimeMs
        {
            get { return defaultExposureTimeMs; }
            set { defaultExposureTimeMs = value; }
        }

        private int defaultLightStableTimeMs = 20;
        public int DefaultLightStableTimeMs
        {
            get { return defaultLightStableTimeMs; }
            set { defaultLightStableTimeMs = value; }
        }

        public int NumLight
        {
            get
            {
                int numLight = lightCtrlInfoList.Sum(x => x.NumChannel);
                return numLight;
                //return (numLight == 0 ? 1 : numLight);
            }
        }

        public int NumDeviceLight
        {
            get
            {
                return lightCtrlInfoList.Sum(x => x.NumChannel);
            }
        }

        private int numLightType = 1;
        public int NumLightType
        {
            get { return numLightType; }
            set { numLightType = value; }
        }

        //private InterfaceType interfaceType;
        //public InterfaceType InterfaceType
        //{
        //    get { return interfaceType; }
        //    set { interfaceType = value; }
        //}

        //SerialPortInfo barcodeReaderPortInfo = new SerialPortInfo();
        //public SerialPortInfo BarcodeReaderPortInfo
        //{
        //    get { return barcodeReaderPortInfo; }
        //    set { barcodeReaderPortInfo = value; }
        //}

        //private bool useModelBarcode;
        //public bool UseModelBarcode
        //{
        //    get { return useModelBarcode; }
        //    set { useModelBarcode = value; }
        //}

        private bool useTowerLamp;
        public bool UseTowerLamp
        {
            get { return useTowerLamp; }
            set { useTowerLamp = value; }
        }

        private bool useSoundBuzzer;
        public bool UseSoundBuzzer
        {
            get { return useSoundBuzzer; }
            set { useSoundBuzzer = value; }
        }

        private bool useDoorSensor = false;
        public bool UseDoorSensor
        {
            get { return useDoorSensor; }
            set { useDoorSensor = value; }
        }

        //private bool useBarcodeReader = false;
        //public bool UseBarcodeReader
        //{
        //    get { return useBarcodeReader; }
        //    set { useBarcodeReader = value; }
        //}

        //private BarcodeReaderType barcodeReaderType = BarcodeReaderType.Serial;
        //public BarcodeReaderType BarcodeReaderType
        //{
        //    get { return barcodeReaderType; }
        //    set { barcodeReaderType = value; }
        //}

        SerialDeviceInfoList serialDeviceInfoList = new SerialDeviceInfoList();
        public SerialDeviceInfoList SerialDeviceInfoList
        {
            get { return serialDeviceInfoList; }
            set { serialDeviceInfoList = value; }
        }

        //SerialPortInfo serialInterfacePortInfo = new SerialPortInfo();
        //public SerialPortInfo SerialInterfacePortInfo
        //{
        //    get { return serialInterfacePortInfo; }
        //    set { serialInterfacePortInfo = value; }
        //}

        //TcpIpInfo tcpIpInfo = new TcpIpInfo();
        //public TcpIpInfo TcpIpInfo
        //{
        //    get { return tcpIpInfo; }
        //    set { tcpIpInfo = value; }
        //}

        //FinsInfo finsInfo = new FinsInfo();
        //public FinsInfo FinsInfo
        //{
        //    get { return finsInfo; }
        //    set { finsInfo = value; }
        //}

        //AlignDataInterfaceInfo alignDataInterfaceInfo = new AlignDataInterfaceInfo();
        //public AlignDataInterfaceInfo AlignDataInterfaceInfo
        //{
        //    get { return alignDataInterfaceInfo; }
        //    set { alignDataInterfaceInfo = value; }
        //}

        //MelsecConnectionInfo melsecConnectionInfo = new MelsecConnectionInfo();
        //public MelsecConnectionInfo MelsecConnectionInfo
        //{
        //    get { return melsecConnectionInfo; }
        //    set { melsecConnectionInfo = value; }
        //}

        //SerialPortInfo xgtInterfacePortInfo = new SerialPortInfo();
        //public SerialPortInfo XgtInterfacePortInfo
        //{
        //    get { return xgtInterfacePortInfo; }
        //    set { xgtInterfacePortInfo = value; }
        //}

        //private TriggerSourceType triggerSourceType;
        //public TriggerSourceType TriggerSourceType
        //{
        //    get { return triggerSourceType; }
        //    set { triggerSourceType = value; }
        //}

        //private bool interactiveTrigger;
        //public bool InteractiveTrigger
        //{
        //    get { return interactiveTrigger; }
        //    set { interactiveTrigger = value; }
        //}

        //private bool useRobotStage;
        //public bool UseRobotStage
        //{
        //    get { return useRobotStage; }
        //    set { useRobotStage = value; }
        //}

        private bool useOpPanel;
        public bool UseOpPanel
        {
            get { return useOpPanel; }
            set { useOpPanel = value; }
        }

        private bool useAirPressure;
        public bool UseAirPressure
        {
            get { return useAirPressure; }
            set { useAirPressure = value; }
        }

        //private bool useConveyor;
        //public bool UseConveyor
        //{
        //    get { return useConveyor; }
        //    set { useConveyor = value; }
        //}

        LightCtrlInfoList lightCtrlInfoList = new LightCtrlInfoList();
        public LightCtrlInfoList LightCtrlInfoList
        {
            get { return lightCtrlInfoList; }
            set { lightCtrlInfoList = value; }
        }

        DaqChannelPropertyList daqChannelPropertyList = new DaqChannelPropertyList();
        public DaqChannelPropertyList DaqChannelPropertyList
        {
            get { return daqChannelPropertyList; }
            set { daqChannelPropertyList = value; }
        }

        //public bool IsUseBarcodeReader()
        //{
        //    return String.IsNullOrEmpty(barcodeReaderPortInfo.PortName) == false;
        //}

        //public bool IsUseSerialPort()
        //{
        //    return String.IsNullOrEmpty(serialPortInfo.PortName) == false;
        //}

        //MachineIfType machineIfType;
        //public MachineIfType MachineIfType
        //{
        //    get { return machineIfType; }
        //    set { machineIfType = value; }
        //}

        MachineIfSetting machineIfSetting;
        public MachineIfSetting MachineIfSetting
        {
            get { return machineIfSetting; }
            set { machineIfSetting = value; }
        }


        float pixelRes3D = 1.0f;
        public float PixelRes3D
        {
            get { return pixelRes3D; }
            set { pixelRes3D = value; }
        }

        PointF cameraOffset;
        public PointF CameraOffset
        {
            get { return cameraOffset; }
            set { cameraOffset = value; }
        }

        UpsSetting upsSetting = new UpsSetting(UpsType.None);
        public UpsSetting UpsSetting
        {
            get { return upsSetting; }
            set { upsSetting = value; }
        }

        private static MachineSettings _instance = null;
        public static MachineSettings Instance()
        {
            if (_instance == null)
            {
                _instance = new MachineSettings();
                _instance.Load();
            }

            return _instance;
        }

        public MachineSettings()
        {
        }

        public SerialPortInfo GetSerialPortInfo(string portName)
        {
            return serialDeviceInfoList.Find(f => f.SerialPortInfo.PortName == portName).SerialPortInfo;
        }

        public void Save()
        {
            string fileName = System.IO.Path.Combine(PathSettings.Instance().Config, $"machine.xml");

            XmlDocument xmlDocument = new XmlDocument();

            XmlElement machineElement = xmlDocument.CreateElement("", "Machine", "");
            xmlDocument.AppendChild(machineElement);

            XmlHelper.SetValue(machineElement, "RunningMode", runningMode.ToString());

            foreach (DigitalIoInfo digitalIoInfo in digitalIoInfoList)
            {
                XmlElement digitalIoInfoElement = machineElement.OwnerDocument.CreateElement("", "DigitalIoInfo", "");
                machineElement.AppendChild(digitalIoInfoElement);

                digitalIoInfo.SaveXml(digitalIoInfoElement);
            }

            foreach (MotionInfo motionInfo in motionInfoList)
            {
                XmlElement motionInfoElement = machineElement.OwnerDocument.CreateElement("", "MotionInfo", "");
                machineElement.AppendChild(motionInfoElement);

                motionInfo.SaveXml(motionInfoElement);
            }

            foreach (GrabberInfo grabberInfo in grabberInfoList)
            {
                XmlElement grabberInfoElement = machineElement.OwnerDocument.CreateElement("", "GrabberInfo", "");
                machineElement.AppendChild(grabberInfoElement);

                grabberInfo.SaveXml(grabberInfoElement);
            }

            foreach (LightCtrlInfo lightCtrlInfo in lightCtrlInfoList)
            {
                XmlElement lightElement = machineElement.OwnerDocument.CreateElement("", "LightInfo", "");
                machineElement.AppendChild(lightElement);

                lightCtrlInfo.SaveXml(lightElement);
            }

            foreach (SerialDeviceInfo serialDeviceInfo in this.serialDeviceInfoList)
            {
                XmlElement serialPortElement = machineElement.OwnerDocument.CreateElement("", "SerialDeviceInfo", "");
                machineElement.AppendChild(serialPortElement);

                serialDeviceInfo.Save(serialPortElement);
            }

            foreach (DaqChannelProperty daqChannelProperty in daqChannelPropertyList)
            {
                XmlElement daqChannelPropertyElement = machineElement.OwnerDocument.CreateElement("", "DaqChannelProperty", "");
                machineElement.AppendChild(daqChannelPropertyElement);

                daqChannelProperty.SaveXml(daqChannelPropertyElement);
            }

            XmlHelper.SetValue(machineElement, "DefaultExposureTime", defaultExposureTimeMs.ToString());
            XmlHelper.SetValue(machineElement, "NumLightType", numLightType.ToString());
            //XmlHelper.SetValue(machineElement, "InteractiveTrigger", interactiveTrigger.ToString());
            //XmlHelper.SetValue(machineElement, "UseModelBarcode", useModelBarcode.ToString());
            XmlHelper.SetValue(machineElement, "PixelRes3D", pixelRes3D.ToString());
            XmlHelper.SetValue(machineElement, "CameraOffset", cameraOffset);

            //barcodeReaderPortInfo.Save(machineElement, "SerialBarcodeReader");
            //serialPortInfo.Save(machineElement, "SerialPortInfo");

            XmlHelper.SetValue(machineElement, "UseOpPanel", useOpPanel.ToString());
            XmlHelper.SetValue(machineElement, "UseAirPressure", useAirPressure.ToString());
            XmlHelper.SetValue(machineElement, "UseDoorSensor", useDoorSensor.ToString());
            XmlHelper.SetValue(machineElement, "UseTowerLamp", useTowerLamp.ToString());
            //XmlHelper.SetValue(machineElement, "UseRobotStage", useRobotStage.ToString());
            //XmlHelper.SetValue(machineElement, "UseConvayer", useConveyor.ToString());

            //XmlHelper.SetValue(machineElement, "MachineIfType", machineIfType.ToString());
            if (machineIfSetting != null)
                machineIfSetting.Save(machineElement, "MachineIfSettings");

            this.upsSetting.Save(machineElement, "UPSSetting");


            //XmlHelper.SetValue(machineElement, "InterfaceType", interfaceType.ToString());
            //serialInterfacePortInfo.Save(machineElement, "SerialInterface");
            //tcpIpInfo.Save(machineElement, "TcpIpInterface");
            //finsInfo.Save(machineElement, "FinsInterface");
            //xgtInterfacePortInfo.Save(machineElement, "XgtSerialInterface");
            //alignDataInterfaceInfo.Save(machineElement, "AlignDataInterfaceInfo");
            //melsecConnectionInfo.Save(machineElement, "MelsecConnectionInfo");

            //FileHelper.SafeSave(fileName, string.Format("{0}\\machine_{1}.xml.bak", PathSettings.Instance().Config, DateTime.Now.ToString("yyMMddHHmmss")), fileName);
            XmlHelper.Save(xmlDocument, fileName);
        }

        public void Load()
        {
            string fileName = System.IO.Path.Combine(PathSettings.Instance().Config, $"machine.xml");

            try
            {
                XmlDocument xmlDocument = XmlHelper.Load(fileName);
                if (xmlDocument == null)
                    return;

                XmlElement machineElement = xmlDocument["Machine"];
                if (machineElement == null)
                    return;

                this.runningMode = XmlHelper.GetValue(machineElement, "RunningMode", RunningMode);

                LoadDigitalIoInfo(machineElement);
                LoadMotionInfo(machineElement);
                LoadGrabberInfo(machineElement);

                numLightType = Convert.ToInt32(XmlHelper.GetValue(machineElement, "NumLightType", "1"));
                //            interactiveTrigger = Convert.ToBoolean(XmlHelper.GetValue(machineElement, "InteractiveTrigger", "False"));

                //machineIfType = (MachineIfType)Enum.Parse(typeof(MachineIfType), XmlHelper.GetValue(machineElement, "MachineIfType", MachineIfType.None.ToString()));
                XmlElement machineIfElement = machineElement["MachineIfSettings"];
                machineIfSetting = MachineIfSetting.LoadSettings(machineIfElement);

                this.upsSetting = UpsSetting.Load(machineElement, "UPSSetting");

                useOpPanel = Convert.ToBoolean(XmlHelper.GetValue(machineElement, "UseOpPanel", "false"));
                useAirPressure = Convert.ToBoolean(XmlHelper.GetValue(machineElement, "UseAirPressure", "false"));
                useDoorSensor = Convert.ToBoolean(XmlHelper.GetValue(machineElement, "UseDoorSensor", "false"));
                useTowerLamp = Convert.ToBoolean(XmlHelper.GetValue(machineElement, "UseTowerLamp", "false"));

                //this.useRobotStage =Convert.ToBoolean( XmlHelper.GetValue(machineElement, "UseRobotStage","false"));
                //this.useConveyor = Convert.ToBoolean(XmlHelper.GetValue(machineElement, "UseConvayer", "false"));


                //            useModelBarcode = Convert.ToBoolean(XmlHelper.GetValue(machineElement, "UseModelBarcode", "False"));

                LoadLightInfo(machineElement);

                //interfaceType = (InterfaceType)Enum.Parse(typeof(InterfaceType), XmlHelper.GetValue(machineElement, "InterfaceType", "None"));
                //serialInterfacePortInfo.Load(machineElement, "SerialInterface");
                //tcpIpInfo.Load(machineElement, "TcpIpInterface");
                //finsInfo.Load(machineElement, "FinsInterface");

                //xgtInterfacePortInfo.Load(machineElement, "XgtSerialInterface");
                //alignDataInterfaceInfo.Load(machineElement, "AlignDataInterfaceInfo");
                //melsecConnectionInfo.Load(machineElement, "MelsecConnectionInfo");
                LoadSerialInfoList(machineElement);
                LoadDaqChannelProperty(machineElement);
            }
            catch
            {
                Save();
            }
        }

        private void LoadSerialInfoList(XmlElement machineElement)
        {
            serialDeviceInfoList.Clear();

            XmlNodeList serialPortInfoNodeList = machineElement.GetElementsByTagName("SerialDeviceInfo");
            foreach (XmlElement serialPortNode in serialPortInfoNodeList)
            {
                ESerialDeviceType deviceType = XmlHelper.GetValue(serialPortNode, "DeviceType", ESerialDeviceType.Light);
                ESerialSensorType sensorType = XmlHelper.GetValue(serialPortNode, "SerialSensorType", ESerialSensorType.None);

                SerialDeviceInfo serialDeviceInfo = SerialDeviceInfoFactory.CreateSerialDeviceInfo(deviceType, sensorType);
                serialDeviceInfo.Load(serialPortNode);

                serialDeviceInfoList.Add(serialDeviceInfo);
            }
        }

        private void LoadDaqChannelProperty(XmlElement machineElement)
        {
            daqChannelPropertyList.Clear();

            XmlNodeList daqChannelPropertyNodeList = machineElement.GetElementsByTagName("DaqChannelProperty");
            if (daqChannelPropertyNodeList.Count > 0)
            {
                foreach (XmlNode daqChannelPropertyNode in daqChannelPropertyNodeList)
                {
                    DaqChannelProperty daqChannelProperty = new DaqChannelProperty();
                    daqChannelProperty.LoadXml((XmlElement)daqChannelPropertyNode);

                    if (daqChannelProperty.DaqChannelType != DaqChannelType.None)
                        daqChannelPropertyList.Add(daqChannelProperty);
                }
            }
        }

        private void LoadDigitalIoInfo(XmlElement machineElement)
        {
            digitalIoInfoList.Clear();

            DigitalIoType digitalIoType = (DigitalIoType)Enum.Parse(typeof(DigitalIoType), XmlHelper.GetValue(machineElement, "DigitalIoType", "Adlink7432"));

            //XmlNodeList digitalIoInfoNodeList = machineElement.GetElementsByTagName("DigitalIoInfoList");
            XmlNodeList digitalIoInfoNodeList = machineElement.GetElementsByTagName("DigitalIoInfo");
            if (digitalIoInfoNodeList.Count > 0)
            {
                foreach (XmlNode digitalIoInfoNode in digitalIoInfoNodeList)
                {
                    DigitalIoType type = (DigitalIoType)Enum.Parse(typeof(DigitalIoType), XmlHelper.GetValue((XmlElement)digitalIoInfoNode, "Type", "Adlink7230"));

                    if (DigitalIoFactory.IsSlaveDevice(type))
                    {
                        SlaveDigitalIoInfo digitalIoInfo = new SlaveDigitalIoInfo(type);
                        digitalIoInfo.LoadXml((XmlElement)digitalIoInfoNode);

                        digitalIoInfoList.Add(digitalIoInfo);
                    }
                    else
                    {
                        DigitalIoInfo digitalIoInfo = DigitalIoInfoFactory.Create(type);
                        digitalIoInfo.LoadXml((XmlElement)digitalIoInfoNode);

                        digitalIoInfoList.Add(digitalIoInfo);
                    }
                }
            }
        }

        private void LoadMotionInfo(XmlElement machineElement)
        {
            motionInfoList.Clear();

            XmlNodeList motionInfoNodeList = machineElement.GetElementsByTagName("MotionInfo");
            if (motionInfoNodeList.Count > 0)
            {
                foreach (XmlNode motionInfoNode in motionInfoNodeList)
                {
                    MotionType motionType = (MotionType)Enum.Parse(typeof(MotionType), XmlHelper.GetValue((XmlElement)motionInfoNode, "Type", "None"));

                    MotionInfo motionInfo = MotionInfoFactory.CreateMotionInfo(motionType);
                    motionInfo.LoadXml((XmlElement)motionInfoNode);

                    motionInfoList.Add(motionInfo);
                }
            }
        }

        private void LoadGrabberInfo(XmlElement machineElement)
        {
            grabberInfoList.Clear();

            GrabberType grabberType = (GrabberType)Enum.Parse(typeof(GrabberType), XmlHelper.GetValue(machineElement, "CameraManagerType", "Pylon"));
            XmlNodeList grabberInfoNodeList = machineElement.GetElementsByTagName("GrabberInfo");
            if (grabberInfoNodeList.Count > 0)
            {
                foreach (XmlNode grabberInfoNode in grabberInfoNodeList)
                {
                    GrabberInfo grabberInfo = new GrabberInfo();
                    grabberInfo.LoadXml((XmlElement)grabberInfoNode);

                    grabberInfoList.Add(grabberInfo);
                }
            }
        }

        private void LoadLightInfo(XmlElement machineElement)
        {
            lightCtrlInfoList.Clear();

            XmlNodeList lightInfoNodeList = machineElement.GetElementsByTagName("LightInfo");
            if (lightInfoNodeList == null)
                return;

            foreach (XmlNode lightInfoNode in lightInfoNodeList)
            {
                //LightCtrlType lightCtrlType;
                //string lightCtrlTypeStr = XmlHelper.GetValue((XmlElement)lightInfoNode, "LightCtrlType", "None");
                //try
                //{
                //    lightCtrlType = (LightCtrlType)Enum.Parse(typeof(LightCtrlType), lightCtrlTypeStr);
                //}
                //catch (Exception)
                //{
                //    ErrorManager.Instance().Report(ErrorCodeLight.Instance.FailToReadParamFile, ErrorLevel.Error,
                //        lightCtrlTypeStr, "Light Type : {0}", new object[] { lightCtrlTypeStr });
                //    continue;
                //}

                //LightCtrlInfo lightCtrlInfo = LightCtrlInfoFactory.Create(lightCtrlType);
                //if (lightCtrlInfo != null)
                //{
                //    lightCtrlInfo.LoadXml((XmlElement)lightInfoNode);
                //    lightCtrlInfoList.Add(lightCtrlInfo);
                //}

                LightCtrlInfo lightCtrlInfo = LightCtrlInfoFactory.Create((XmlElement)lightInfoNode);
                if(lightCtrlInfo!=null)
                    lightCtrlInfoList.Add(lightCtrlInfo);
            }
        }
    }
}
