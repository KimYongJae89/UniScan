using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.Daq;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using DynMvp.Vision;
using UniEye.Base.Settings;
using DynMvp.Devices.Comm;
using DynMvp.Device.Serial;
using UniEye.Base.MachineInterface;
using DynMvp.UI.Touch;
using DynMvp.Device.Device.Serial;
using DynMvp.Device.Device.Dio;
using DynMvp.Device.Device.FrameGrabber;
using DynMvp.Device.Device.UPS;

namespace UniEye.Base.Device
{
    public class DeviceBox
    {
        PortMapBase portMap = null;
        public PortMapBase PortMap
        {
            get { return portMap; }
            set { portMap = value; }
        }

        DigitalIoHandler digitalIoHandler = null;
        public DigitalIoHandler DigitalIoHandler
        {
            get { return digitalIoHandler; }
        }

        MotionList motionList = null;
        public MotionList MotionList
        {
            get { return motionList; }
        }

        AxisConfiguration axisConfiguration = null;
        public AxisConfiguration AxisConfiguration
        {
            get { return axisConfiguration; }
        }

        GrabberList grabberList = null;
        public GrabberList GrabberList
        {
            get { return grabberList; }
        }

        ImageDeviceHandler imageDeviceHandler = null;
        public ImageDeviceHandler ImageDeviceHandler
        {
            get { return imageDeviceHandler; }
        }

        List<Calibration> cameraCalibrationList = new List<Calibration>();
        public List<Calibration> CameraCalibrationList
        {
            get { return cameraCalibrationList; }
        }

        LightCtrlHandler lightCtrlHandler = new LightCtrlHandler();
        public LightCtrlHandler LightCtrlHandler
        {
            get { return lightCtrlHandler; }
        }

        SerialDeviceHandler serialDeviceHandler = new SerialDeviceHandler();
        public SerialDeviceHandler SerialDeviceHandler
        {
            get { return serialDeviceHandler; }
        }

        private ImageAcquisition imageAcquisition;

        private IReportProgress reportProgress = null;
        public IReportProgress ReportProgress
        {
            get { return reportProgress; }
            set { reportProgress = value; }
        }

        private List<DaqChannel> daqChannelList;
        public List<DaqChannel> DaqChannelList
        {
            get { return daqChannelList; }
        }

        protected MachineIf machineIf;
        public MachineIf MachineIf
        {
            get { return machineIf; }
        }

        public Ups Ups => this.ups;
        protected Ups ups;

        public DeviceBox(PortMap portMap)
        {
            this.portMap = portMap;
            this.portMap.Load();
        }

        private void DoReportProgress(IReportProgress reportProgress, int percentage, string message)
        {
            LogHelper.Debug(LoggerType.StartUp, message);

            if (reportProgress != null)
                reportProgress.ReportProgress(percentage, StringManager.GetString(message));
        }

        public ImageAcquisition GetImageAcquisition()
        {
            if (imageAcquisition == null)
            {
                imageAcquisition = new ImageAcquisition();
                imageAcquisition.Initialize(imageDeviceHandler, lightCtrlHandler, MachineSettings.Instance().NumLightType, 24);
            }

            return imageAcquisition;
        }

        public void InitializeCameraAndLight()
        {
            LogHelper.Debug(LoggerType.StartUp, "Start - InitializeCameraAndLightMachine");

            MachineSettings machineSettings = MachineSettings.Instance();
            bool isVirtual = IsVirtualGrabber;

            grabberList = new GrabberList();
            grabberList.Initialize(machineSettings.GrabberInfoList, isVirtual);
            InitializeCamera(true);

            DoReportProgress(reportProgress, 35, "Initialize Light Ctrl");
            InitializeLightCtrl(machineSettings.LightCtrlInfoList, isVirtual);
        }

        protected virtual bool IsVirtualGrabber => MachineSettings.Instance().RunningMode != RunningMode.Real;
        protected virtual bool IsVirtualMotion => MachineSettings.Instance().RunningMode != RunningMode.Real;
        protected virtual bool IsVirtualDio => MachineSettings.Instance().RunningMode != RunningMode.Real;
        protected virtual bool IsVirtualLight => MachineSettings.Instance().RunningMode != RunningMode.Real;
        protected virtual bool IsVirtualDaq => MachineSettings.Instance().RunningMode != RunningMode.Real;
        protected virtual bool IsVirtualSerial => MachineSettings.Instance().RunningMode != RunningMode.Real;
        protected virtual bool IsVirtualMachineIf => MachineSettings.Instance().RunningMode != RunningMode.Real;

        public void Initialize(IReportProgress reportProgress)
        {
            LogHelper.Debug(LoggerType.StartUp, "Start - Initialize Machine");

            MachineSettings machineSettings = MachineSettings.Instance();

            DoReportProgress(reportProgress, 15, "Initialize Grabber");
            grabberList = new GrabberList();
            grabberList.Initialize(machineSettings.GrabberInfoList, IsVirtualGrabber);
            InitializeCamera(false);

            DoReportProgress(reportProgress, 20, "Initialize Motion");
            InitializeMotion(machineSettings.MotionInfoList, IsVirtualMotion);

            DoReportProgress(reportProgress, 25, "Initialize Digital IO");
            InitializeDigitalIo(machineSettings.DigitalIoInfoList, IsVirtualDio);

            DoReportProgress(reportProgress, 30, "Initialize Light Ctrl");
            InitializeLightCtrl(machineSettings.LightCtrlInfoList, IsVirtualLight);

            DoReportProgress(reportProgress, 35, "Initialize DAQ Device");
            InitializeDaqDevice(machineSettings.DaqChannelPropertyList, IsVirtualDaq);

            DoReportProgress(reportProgress, 40, "Initialize Serial Device");
            InitializeSerialDevice(machineSettings.SerialDeviceInfoList, IsVirtualSerial);

            DoReportProgress(reportProgress, 45, "Initialize MachineIF");
            InitializeMachineIF(machineSettings.MachineIfSetting, IsVirtualMachineIf);

            DoReportProgress(reportProgress, 50, "Initialize UPS Device");
            InitializeUPS(machineSettings.UpsSetting, IsVirtualMachineIf);

            //InitializeMachineIFList(machineSettings.MachineIfSettingList, isVirtual);

            PostInitialize();
        }

        public virtual void PostInitialize()
        {

        }

        public void InitializeMachineIF(MachineIfSetting machineIfSetting, bool isVirtual)
        {
            if (machineIfSetting == null)
                return;

            machineIf = MachineIf.Create(machineIfSetting, machineIfSetting.IsVirtualMode);
            if (machineIf != null)
            {
                MachineIfExecuter[] machineIfExecuters = SystemManager.Instance().CreateMachineIfExecuter();
                machineIf.AddExecuters(machineIfExecuters);
                machineIf.Initialize();
            }
        }

        private void InitializeSerialDevice(SerialDeviceInfoList serialDeviceInfoList, bool isVirtual)
        {
            foreach (SerialDeviceInfo serialDeviceInfo in serialDeviceInfoList)
            {
                SerialDevice serialDevice = serialDeviceInfo.CreateSerialDevice(isVirtual);
                if (serialDevice == null)
                {
                    throw new AlarmException(ErrorCodeSerial.Instance.FailToCreate, ErrorLevel.Fatal, serialDeviceInfo.DeviceName, "Fail To Create", null, "");
                    //string message = string.Format("Serial Device \'{0}\' Create Failed", serialDeviceInfo.DeviceName);
                    //LogHelper.Error(LoggerType.Device, message);
                    //ErrorManager.Instance().Report(ErrorCodeSerial.Instance.InvalidType, ErrorLevel.Fatal,
                    //   serialDevice.DeviceInfo.DeviceName, message);
                    continue;
                }

                if (serialDevice.Initialize(/*serialDeviceInfo, machineSettings.VirtualMode*/) == false)
                {
                    throw new AlarmException(ErrorCodeSerial.Instance.FailToInitialize, ErrorLevel.Fatal, serialDeviceInfo.DeviceName, "Fail to Initialize", null, "");
                    //string message = string.Format("Serial Device \'{0}\' Initialize Failed", serialDeviceInfo.DeviceName);
                    //LogHelper.Error(LoggerType.Device, message);
                    //ErrorManager.Instance().Report(ErrorCodeSerial.Instance.FailToInitialize, ErrorLevel.Fatal,
                    //   serialDevice.DeviceInfo.DeviceName, message);

                    //serialDevice = serialDeviceInfo.CreateSerialDevice(true);
                    //serialDevice.Initialize(/*serialDeviceInfo, true*/);
                }
                serialDeviceHandler.Add(serialDevice);
            }
        }

        private void InitializeUPS(UpsSetting upsSetting, bool isVirtualMachineIf)
        {
            this.ups = UpsFactory.Create(upsSetting);
            this.ups?.StartService();
        }

        public static string GetCameraConfigurationPath(string grabberName) => GetCameraConfigurationPath(grabberName, CameraConfiguration.ConfigFlag);
        
        public static string GetCameraConfigurationPath(string grabberName, string flag)
        {
            string filePath;
            if (string.IsNullOrEmpty(flag))
            {
                filePath = Path.Combine(PathSettings.Instance().Config, $"CameraConfiguration_{grabberName}.xml");
            }
            else
            {
                filePath = Path.Combine(PathSettings.Instance().Config, flag, $"CameraConfiguration_{grabberName}.xml");
                if (!File.Exists(filePath))
                {
                    string dirName = Path.GetDirectoryName(filePath);
                    Directory.CreateDirectory(dirName);
                    string src = GetCameraConfigurationPath(grabberName, "");
                    File.Copy(src, filePath, true);
                }
            }
            return filePath;
        }

        private void SaveCameraConfiguration(CameraConfiguration cameraConfiguration, string grabberName)
        {
            string filePath = GetCameraConfigurationPath(grabberName);
            cameraConfiguration.SaveCameraConfiguration(filePath);
        }

        private CameraConfiguration LoadCameraConfiguration(Grabber grabber)
        {
            CameraConfiguration cameraConfiguration = new CameraConfiguration(grabber.NumCamera);

            string filePath = GetCameraConfigurationPath(grabber.Name);
            try
            {
                cameraConfiguration.LoadCameraConfiguration(filePath);
            }
            catch
            {
                cameraConfiguration.SetDefault(grabber.Type);
            }
            return cameraConfiguration;
        }

        private void InitializeCamera(bool calibrationMode)
        {
            imageDeviceHandler = new ImageDeviceHandler();

            AddCamera(calibrationMode);

            LoadCameraCalibration();

            imageDeviceHandler.SetTriggerDelay(TimeSettings.Instance().TriggerDelayMs);
        }

        private void AddCamera(bool calibrationMode)
        {
            foreach (Grabber grabber in grabberList)
            {
                CameraConfiguration cameraConfiguration = LoadCameraConfiguration(grabber);
                if (cameraConfiguration.RequiredCameras != grabber.NumCamera)
                    throw new AlarmException(ErrorCodeGrabber.Instance.InvalidSetting, ErrorLevel.Fatal,
                        grabber.Name, "Required Camera Count Missmatch", null, "");

                foreach (CameraInfo cameraInfo in cameraConfiguration)
                {
                    if (!cameraInfo.Enabled)
                        return;

                    if (string.IsNullOrEmpty(cameraInfo.VirtualImagePath))
                        cameraInfo.VirtualImagePath = PathSettings.Instance().VirtualImage;

                    Camera camera = CreateCamera(grabber, cameraInfo);
                    if (camera == null)
                        throw new AlarmException(ErrorCodeGrabber.Instance.FailToCreate, ErrorLevel.Fatal,
                            cameraInfo.Name, "Camera Create Fail", null, "");

                    //cameraInfo.Index = index++;

                    LogHelper.Debug(LoggerType.StartUp, String.Format("Initialize camera [{0}]: {1} ({2})", cameraInfo.Index, cameraInfo.Name, camera.GetType().Name));
                    camera.Initialize(calibrationMode);
                    camera.UpdateState(DeviceState.Ready);

                    imageDeviceHandler.AddCamera(camera, cameraInfo);
                }
                //imageDeviceHandler.AddCamera(grabber, cameraConfiguration);
                //SaveCameraConfiguration(cameraConfiguration, grabber.Name);
            }
        }

        public virtual Camera CreateCamera(Grabber grabber, CameraInfo cameraInfo)
        {
            return grabber.CreateCamera(cameraInfo);
        }

        private void LoadCameraCalibration()
        {
            foreach (Camera camera in imageDeviceHandler)
            {
                //string datFileName = String.Format(@"{0}\Calibration{1}.xml", PathSettings.Instance().Config, camera.Index);
                //string gridFileName = String.Format(@"{0}\Calibration{1}.dat", PathSettings.Instance().Config, camera.Index);

                string path = Path.Combine(PathSettings.Instance().Config, CameraConfiguration.ConfigFlag);

                string datFileName = Path.Combine(path, $"Calibration{camera.Index}.xml");
                string gridFileName = Path.Combine(path, $"Calibration{camera.Index}.dat");

                Calibration calibration = AlgorithmBuilder.CreateCalibration();

                if (calibration != null)
                {
                    calibration.Initialize(camera.Index, datFileName, gridFileName);
                    cameraCalibrationList.Add(calibration);

                    camera.UpdateFovSize(calibration.PelSize);
                    calibration.UpdatePelSize(camera.ImageSize.Width, camera.ImageSize.Height);
                }
            }
        }

        private void InitializeMotion(MotionInfoList motionInfoList, bool isVirtual)
        {
            motionList = new MotionList();
            motionList.Initialize(motionInfoList, isVirtual);

            axisConfiguration = new AxisConfiguration();

            if (motionList.Count > 0)
            {
                if (axisConfiguration.LoadConfiguration(motionList) == false)
                {
                    axisConfiguration.Initialize(motionList);
                }
            }
        }

        public void InitAxisConfiguration(List<string> axisHandlerNames)
        {
            axisConfiguration.SetupAxisHandler(axisHandlerNames.ToArray());

        }

        private void InitializeDigitalIo(DigitalIoInfoList digitalIoInfoList, bool isVirtual)
        {
            digitalIoHandler = new DigitalIoHandler();

            IDigitalIo digitalIo;
            foreach (DigitalIoInfo digitalIoInfo in digitalIoInfoList)
            {
                if (isVirtual)
                {
                    DigitalIoInfo digitalIoInfo2 = new DigitalIoInfoVirtual(DigitalIoType.Virtual, digitalIoInfo.Index,
                        digitalIoInfo.NumInPortGroup, 0, digitalIoInfo.NumInPort,
                        digitalIoInfo.NumOutPortGroup, 0, digitalIoInfo.NumOutPort);

                    digitalIo = DigitalIoFactory.Create(digitalIoInfo2);
                }
                else
                {
                    if (DigitalIoFactory.IsSlaveDevice(digitalIoInfo.Type))
                    {
                        digitalIo = (IDigitalIo)motionList.GetMotion(digitalIoInfo.Index);

                        if (digitalIo == null)
                        {
                            ErrorManager.Instance().Report(ErrorCodeDigitalIo.Instance.InvalidType, ErrorLevel.Fatal,
                                digitalIoInfo.Name, "Fail to initialize Digital I/O. {0}", new object[] { digitalIoInfo.Type.ToString() });
                        }
                        else if (digitalIo.Initialize(digitalIoInfo) == false)
                        {
                            ErrorManager.Instance().Report(ErrorCodeDigitalIo.Instance.FailToInitialize, ErrorLevel.Fatal,
                                digitalIoInfo.Name, "Fail to initialize Digital I/O. {0}", new object[] { digitalIoInfo.Type.ToString() });
                            digitalIo.UpdateState(DeviceState.Error, "DigitalIo is invalid.");
                            continue;
                        }
                        else
                        {
                            digitalIo = (IDigitalIo)motionList.GetMotion(digitalIoInfo.Type.ToString());
                            //digitalIoHandler.Add((IDigitalIo)motionList.GetMotion(digitalIoInfo.Type.ToString()));
                        }
                    }
                    else
                    {
                        digitalIo = DigitalIoFactory.Create(digitalIoInfo);
                    }
                }

                if (digitalIo != null)
                {
                    digitalIoHandler.Add(digitalIo);
                }
            }

            InitializeDigitalIo();
        }

        protected virtual void InitializeDigitalIo()
        {}

        private void InitializeLightCtrl(LightCtrlInfoList lightCtrlInfoList, bool isVirtual)
        {
            foreach (LightCtrlInfo lightCtrlInfo in lightCtrlInfoList)
            {
                LightCtrl lightCtrl = LightCtrlFactory.Create(lightCtrlInfo, digitalIoHandler, isVirtual);

                if (lightCtrl != null)
                {
                    if (lightCtrlInfo.ControllerType == LightCtrlType.IO)
                    {
                        IoLightCtrlInfo ioLightCtrlInfo = (IoLightCtrlInfo)lightCtrlInfo;
                        portMap.GetIoLightPorts(ioLightCtrlInfo.LightCtrlIoPortList);
                    }

                    lightCtrl.LightStableTimeMs = TimeSettings.Instance().LightStableTimeMs;

                    lightCtrlHandler.AddLightCtrl(lightCtrl);
                }
            }

            LightSettings.Instance().Load();
        }

        private void InitializeDaqDevice(DaqChannelPropertyList daqChannelPropertyList, bool isVirtual)
        {
            daqChannelList = new List<DaqChannel>();

            foreach (DaqChannelProperty daqChannelProperty in daqChannelPropertyList)
            {
                DaqChannel daqChannel = DaqChannelManager.Instance().CreateDaqChannel(daqChannelProperty.DaqChannelType, "Daq Channel", isVirtual);

                if (daqChannel != null)
                {
                    daqChannel.Initialize(daqChannelProperty);
                    daqChannelList.Add(daqChannel);

                    DeviceManager.Instance().AddDevice(daqChannel);
                }
            }
        }

        public void Release()
        {
            foreach (Calibration calibration in cameraCalibrationList)
                calibration.Dispose();
            cameraCalibrationList.Clear();

            if (imageDeviceHandler != null)
                imageDeviceHandler.Release();

            if (grabberList != null)
                grabberList.Release();

            if (digitalIoHandler != null)
                digitalIoHandler.Release();

            if (motionList != null)
                motionList.Release();

            if (serialDeviceHandler != null)
                serialDeviceHandler.Release();

            if (lightCtrlHandler != null)
                lightCtrlHandler.Release();

            if (machineIf != null)
            {
                machineIf.Release();
                machineIf = null;
            }

            this.ups?.StopService();
        }
    }
}