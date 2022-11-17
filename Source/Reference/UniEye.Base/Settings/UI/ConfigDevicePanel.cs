using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DynMvp.Devices.Light;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.MotionController;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Daq;
using DynMvp.Device.FrameGrabber.UI;
using DynMvp.Devices.UI;
using DynMvp.Device.MotionController.UI;
using DynMvp.Device.Dio.UI;
using DynMvp.Device.Daq.UI;
using System.IO;
using DynMvp.Devices.Comm;
using DynMvp.UI;
using DynMvp.Base;
using UniEye.Base.Settings;
using UniEye.Base.Device;
using UniEye.Base.UI;
using DynMvp.UI.Touch;
using DynMvp.Device.Serial;
using UniEye.Base.UI.CameraCalibration;
using DynMvp.Vision;

namespace UniEye.Base.Settings.UI
{
    enum DeviceListType
    {
        Grabber, Motion, DigitalIo, LightController, Daq, DepthScanner, SerialDevice
    }

    public partial class ConfigDevicePanel : UserControl
    {
        string title;
        public string Title
        {
            set { title = value; }
        }

        DeviceListType deviceListType;

        GrabberInfoList grabberInfoList;
        MotionInfoList motionInfoList;
        DigitalIoInfoList digitalIoInfoList;
        LightCtrlInfoList lightCtrlInfoList;
        DaqChannelPropertyList daqChannelPropertyList;
        SerialDeviceInfoList serialDeviceInfoList;

        public ConfigDevicePanel()
        {
            InitializeComponent();

            buttonSelectGrabber.Text = StringManager.GetString(this.GetType().FullName, buttonSelectGrabber);
            buttonSelectMotion.Text = StringManager.GetString(this.GetType().FullName, buttonSelectMotion);
            buttonSelectDigitalIo.Text = StringManager.GetString(this.GetType().FullName, buttonSelectDigitalIo);
            buttonSelectLightCtrl.Text = StringManager.GetString(this.GetType().FullName, buttonSelectLightCtrl);
            buttonSelectDaq.Text = StringManager.GetString(this.GetType().FullName, buttonSelectDaq);
            buttonSelectDepthScanner.Text = StringManager.GetString(this.GetType().FullName, buttonSelectDepthScanner);
            addButton.Text = StringManager.GetString(this.GetType().FullName, addButton);
            editButton.Text = StringManager.GetString(this.GetType().FullName, editButton);
            deleteButton.Text = StringManager.GetString(this.GetType().FullName, deleteButton);
            buttonMoveUp.Text = StringManager.GetString(this.GetType().FullName, buttonMoveUp);
            buttonMoveDown.Text = StringManager.GetString(this.GetType().FullName, buttonMoveDown);

            MachineSettings machineSettings = MachineSettings.Instance();
            grabberInfoList = machineSettings.GrabberInfoList.Clone();
            motionInfoList = machineSettings.MotionInfoList.Clone();
            digitalIoInfoList = machineSettings.DigitalIoInfoList.Clone();
            lightCtrlInfoList = machineSettings.LightCtrlInfoList.Clone();
            daqChannelPropertyList = machineSettings.DaqChannelPropertyList.Clone();
            serialDeviceInfoList = machineSettings.SerialDeviceInfoList.Clone();

            checkUseEmergencyStop.Checked = machineSettings.UseOpPanel;
            checkUseAirPressure.Checked = machineSettings.UseAirPressure;
            checkUseDoorSensor.Checked = machineSettings.UseDoorSensor;
            checkUseTowerLamp.Checked = machineSettings.UseTowerLamp;

            checkBoxUseFovNavigator.Checked = CustomizeSettings.Instance().UseFovNavigator;
        }

        private void ConfigDevicePanel_Load(object sender, EventArgs e)
        {
            //MachineSettings machineSettings = MachineSettings.Instance();

            //grabberInfoList = machineSettings.GrabberInfoList.Clone();
            //motionInfoList = machineSettings.MotionInfoList.Clone();
            //digitalIoInfoList = machineSettings.DigitalIoInfoList.Clone();
            //lightCtrlInfoList = machineSettings.LightCtrlInfoList.Clone();
            //daqChannelPropertyList = machineSettings.DaqChannelPropertyList.Clone();
            //serialDeviceInfoList = machineSettings.SerialDeviceInfoList.Clone();

            //checkUseEmergencyStop.Checked = machineSettings.UseOpPanel;
            //checkUseAirPressure.Checked = machineSettings.UseAirPressure;
            //checkUseDoorSensor.Checked = machineSettings.UseDoorSensor;
            //checkUseTowerLamp.Checked = machineSettings.UseTowerLamp;

            //checkBoxUseFovNavigator.Checked = CustomizeSettings.Instance().UseFovNavigator;
            //useRobotStage.Checked = machineSettings.UseRobotStage;
            //useConveyor.Checked = machineSettings.UseConveyor;

            RefreshList(DeviceListType.Grabber);
        }

        public void SaveSetting()
        {
            MachineSettings machineSettings = MachineSettings.Instance();

            machineSettings.GrabberInfoList = grabberInfoList;
            machineSettings.MotionInfoList = motionInfoList;
            machineSettings.DigitalIoInfoList = digitalIoInfoList;
            machineSettings.LightCtrlInfoList = lightCtrlInfoList;
            machineSettings.SerialDeviceInfoList = serialDeviceInfoList;
            machineSettings.DaqChannelPropertyList = daqChannelPropertyList;
            CustomizeSettings.Instance().UseFovNavigator = checkBoxUseFovNavigator.Checked;
            //machineSettings.UseRobotStage = useRobotStage.Checked;
            //machineSettings.UseConveyor = useConveyor.Checked;

            machineSettings.UseOpPanel = checkUseEmergencyStop.Checked;
            machineSettings.UseAirPressure = checkUseAirPressure.Checked;
            machineSettings.UseDoorSensor = checkUseDoorSensor.Checked;
            machineSettings.UseTowerLamp = checkUseTowerLamp.Checked;
        }

        private void buttonSelectGrabber_Click(object sender, EventArgs e)
        {
            RefreshList(DeviceListType.Grabber);
        }

        private void buttonSelectMotion_Click(object sender, EventArgs e)
        {
            RefreshList(DeviceListType.Motion);
        }

        private void buttonSelectDigitalIo_Click(object sender, EventArgs e)
        {
            RefreshList(DeviceListType.DigitalIo);
        }

        private void buttonSelectLightCtrl_Click(object sender, EventArgs e)
        {
            RefreshList(DeviceListType.LightController);
        }

        private void buttonSelectDaq_Click(object sender, EventArgs e)
        {
            RefreshList(DeviceListType.Daq);
        }

        private void buttonSerial_Click(object sender, EventArgs e)
        {
            RefreshList(DeviceListType.SerialDevice);
        }

        private void RefreshList(DeviceListType deviceListType)
        {
            this.deviceListType = deviceListType;
            dataGridViewDeviceList.Rows.Clear();

            switch (deviceListType)
            {
                case DeviceListType.Grabber:
                    RefreshGrabberList();
                    break;
                case DeviceListType.Motion:
                    RefreshMotionList();
                    break;
                case DeviceListType.DigitalIo:
                    RefreshDigitalIoList();
                    break;
                case DeviceListType.LightController:
                    RefreshLightCtrlList();
                    break;
                case DeviceListType.Daq:
                    RefreshDaqChannelList();
                    break;
                case DeviceListType.SerialDevice:
                    RefreshSerialPortInfoList();
                    break;
            }
        }

        private void RefreshGrabberList()
        {
            dataGridViewDeviceList.Columns[3].HeaderText = "Num Camera";

            foreach (GrabberInfo grabberInfo in grabberInfoList)
            {
                AddGrabberInfo(grabberInfo);
            }
        }

        private void AddGrabberInfo(GrabberInfo grabberInfo)
        {
            int index = dataGridViewDeviceList.Rows.Count;

            dataGridViewDeviceList.Rows.Add((index + 1).ToString(), grabberInfo.Name, grabberInfo.Type.ToString(), grabberInfo.NumCamera);
            dataGridViewDeviceList.Rows[index].Tag = grabberInfo;
        }

        private void RefreshMotionList()
        {
            dataGridViewDeviceList.Columns[3].HeaderText = "Num Axis";

            foreach (MotionInfo motionInfo in motionInfoList)
            {
                AddMotionInfo(motionInfo);
            }
        }

        private void AddMotionInfo(MotionInfo motionInfo)
        {
            int index = dataGridViewDeviceList.Rows.Count;

            dataGridViewDeviceList.Rows.Add((index + 1).ToString(), motionInfo.Name, motionInfo.Type.ToString(), motionInfo.NumAxis);
            dataGridViewDeviceList.Rows[index].Tag = motionInfo;
        }

        private void RefreshDigitalIoList()
        {
            dataGridViewDeviceList.Columns[3].HeaderText = "Num Channel";

            foreach (DigitalIoInfo digitalIoInfo in digitalIoInfoList)
            {
                AddDigitalIoInfo(digitalIoInfo);
            }
        }

        private void AddDigitalIoInfo(DigitalIoInfo digitalIoInfo)
        {
            int index = dataGridViewDeviceList.Rows.Count;

            dataGridViewDeviceList.Rows.Add((index + 1).ToString(), digitalIoInfo.Name, digitalIoInfo.Type.ToString(), String.Format("I{0} / O{1}", digitalIoInfo.NumInPort, digitalIoInfo.NumOutPort));
            dataGridViewDeviceList.Rows[index].Tag = digitalIoInfo;

        }

        private void RefreshLightCtrlList()
        {
            dataGridViewDeviceList.Columns[3].HeaderText = "Num Light";

            foreach (LightCtrlInfo lightCtrlInfo in lightCtrlInfoList)
            {
                AddLightCtrlInfo(lightCtrlInfo);
            }
        }

        private void AddLightCtrlInfo(LightCtrlInfo lightCtrlInfo)
        {
            int index = dataGridViewDeviceList.Rows.Count;

            dataGridViewDeviceList.Rows.Add((index + 1).ToString(), lightCtrlInfo.Name, lightCtrlInfo.ControllerType.ToString(), lightCtrlInfo.NumChannel);
            dataGridViewDeviceList.Rows[index].Tag = lightCtrlInfo;
        }

        private void RefreshDaqChannelList()
        {
            dataGridViewDeviceList.Columns[3].HeaderText = "Channel Name";

            foreach (DaqChannelProperty daqChannelProperty in daqChannelPropertyList)
            {
                AddDaqChannel(daqChannelProperty);
            }
        }

        private void AddDaqChannel(DaqChannelProperty daqChannelProperty)
        {
            int index = dataGridViewDeviceList.Rows.Count;

            dataGridViewDeviceList.Rows.Add((index + 1).ToString(), daqChannelProperty.ChannelName, daqChannelProperty.DaqChannelType.ToString(), daqChannelProperty.ChannelName);
            dataGridViewDeviceList.Rows[index].Tag = daqChannelProperty;
        }

        private void RefreshSerialPortInfoList()
        {
            dataGridViewDeviceList.Columns[3].HeaderText = "Port No";

            foreach (SerialDeviceInfo serialPortInfo in serialDeviceInfoList)
            {
                AddSerialPortInfo(serialPortInfo);
            }
        }

        private void AddSerialPortInfo(ISerialDeviceInfo serialDeviceInfo)
        {
            int index = dataGridViewDeviceList.Rows.Count;

            dataGridViewDeviceList.Rows.Add((index + 1).ToString(), serialDeviceInfo.DeviceName, serialDeviceInfo.DeviceType, serialDeviceInfo.SerialPortInfo.PortNo.ToString());
            dataGridViewDeviceList.Rows[index].Tag = serialDeviceInfo;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            switch (deviceListType)
            {
                case DeviceListType.Grabber:
                    {
                        NewGrabberForm form = new NewGrabberForm();
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            GrabberInfo grabberInfo = new GrabberInfo(form.GrabberName, form.GrabberType, form.NumCamera);
                            grabberInfoList.Add(grabberInfo);
                            AddGrabberInfo(grabberInfo);

                            CameraConfiguration cameraConfiguration = new CameraConfiguration(grabberInfo.NumCamera);
                            for(int i=0; i < grabberInfo.NumCamera; i++)
                            {
                                CameraInfo cameraInfo = CameraInfo.Create(grabberInfo.Type);
                                cameraInfo.Index = i;
                                cameraConfiguration.SetCameraInfo(cameraInfo);
                            }

                            string filePath = DeviceBox.GetCameraConfigurationPath(grabberInfo.Name);
                            cameraConfiguration.SaveCameraConfiguration(filePath);
                        }
                    }
                    break;
                case DeviceListType.Motion:
                    {
                        NewMotionForm form = new NewMotionForm();
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            MotionInfo motionInfo = MotionInfoFactory.CreateMotionInfo(form.MotionType);
                            motionInfo.Name = form.MotionName;
                            motionInfoList.Add(motionInfo);
                            AddMotionInfo(motionInfo);
                        }
                    }
                    break;
                case DeviceListType.DigitalIo:
                    {
                        NewDigitalIoForm form = new NewDigitalIoForm();
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            digitalIoInfoList.Add(form.DigitalIoInfo);
                            AddDigitalIoInfo(form.DigitalIoInfo);
                        }
                    }
                    break;
                case DeviceListType.LightController:
                    {
                        DigitalIoHandler digitalIoHandler = new DigitalIoHandler();
                        digitalIoHandler.Build(digitalIoInfoList, motionInfoList);

                        LightConfigForm form = new LightConfigForm();
                        form.LightCtrlName = string.Format("Light {0}", dataGridViewDeviceList.RowCount + 1);
                        form.DigitalIoHandler = digitalIoHandler;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            lightCtrlInfoList.Add(form.LightCtrlInfo);
                            AddLightCtrlInfo(form.LightCtrlInfo);
                        }
                    }
                    break;
                case DeviceListType.Daq:
                    {
                        NewDaqChannelForm form = new NewDaqChannelForm();
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            DaqChannelProperty daqChannelProperty = DaqChannelPropertyFactory.Create(form.DaqChannelType);
                            daqChannelProperty.DaqChannelType = form.DaqChannelType;
                            daqChannelProperty.Name = form.DaqChannelName;
                            daqChannelPropertyList.Add(daqChannelProperty);
                            AddDaqChannel(daqChannelProperty);
                        }
                    }
                    break;
                case DeviceListType.SerialDevice:
                    {
                        SerialPortSettingForm form = new SerialPortSettingForm();
                        form.SerialDeviceInfo = null;
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            serialDeviceInfoList.Add(form.SerialDeviceInfo);
                            AddSerialPortInfo(form.SerialDeviceInfo);
                        }
                    }
                    break;
            }
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            if (dataGridViewDeviceList.SelectedRows.Count == 0)
                return;

            object editTarget = dataGridViewDeviceList.SelectedRows[0].Tag;

            switch (deviceListType)
            {
                case DeviceListType.Grabber:
                    EditGrabber((GrabberInfo)editTarget);
                    break;
                case DeviceListType.Motion:
                    EditMotionInfo((MotionInfo)editTarget);
                    break;
                case DeviceListType.DigitalIo:
                    EditDigitalIoInfo((DigitalIoInfo)editTarget);
                    break;
                case DeviceListType.LightController:
                    EditLightCtrlInfo((LightCtrlInfo)editTarget);
                    break;
                case DeviceListType.Daq:
                    EditDaqChannelProperty((DaqChannelProperty)editTarget);
                    break;
                case DeviceListType.SerialDevice:
                    EditSerialDeviceInfo((SerialDeviceInfo)editTarget);
                    break;
            }
        }

        private void EditSerialDeviceInfo(SerialDeviceInfo editTarget)
        {
            SerialPortSettingForm form = new SerialPortSettingForm();
            form.SerialDeviceInfo = (SerialDeviceInfo)editTarget;
            form.EditableDevice(true);
            if (form.ShowDialog() == DialogResult.OK)
            {
                dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = form.SerialDeviceInfo.DeviceName;
                dataGridViewDeviceList.SelectedRows[0].Cells[2].Value = form.SerialDeviceInfo.DeviceType;
                dataGridViewDeviceList.SelectedRows[0].Cells[3].Value = form.SerialDeviceInfo.SerialPortInfo.PortNo;
            }
        }

        private void EditGrabber(GrabberInfo grabberInfo)
        {
            //string filePath = String.Format("{0}\\CameraConfiguration_{1}.xml", PathSettings.Instance().Config, grabberInfo.Name);
            string filePath = DeviceBox.GetCameraConfigurationPath(grabberInfo.Name);
            CameraConfiguration cameraConfiguration = new CameraConfiguration(filePath);

            Grabber grabber = GrabberFactory.Create(grabberInfo);
            if (grabber.SetupCameraConfiguration((int)grabberInfo.NumCamera, cameraConfiguration))
                cameraConfiguration.SaveCameraConfiguration(filePath);
        }

        private void EditMotionInfo(MotionInfo motionInfo)
        {
            DialogResult dialogResult = DialogResult.Cancel;

            if (motionInfo is PciMotionInfo)
            {
                PciMotionInfoForm form = new PciMotionInfoForm();
                form.PciMotionInfo = (PciMotionInfo)motionInfo;
                dialogResult = form.ShowDialog();
            }
            else if (motionInfo is AjinMotionInfo)
            {
                AjinMotionInfoForm form = new AjinMotionInfoForm();
                form.AjinMotionInfo = (AjinMotionInfo)motionInfo;
                dialogResult = form.ShowDialog();
            }
            else if (motionInfo is NetworkMotionInfo)
            {
                NetworkMotionInfoForm form = new NetworkMotionInfoForm();
                form.NetworkMotionInfo = (NetworkMotionInfo)motionInfo;
                dialogResult = form.ShowDialog();
            }
            else if (motionInfo is SerialMotionInfo)
            {
                SerialMotionInfoForm form = new SerialMotionInfoForm();
                form.SerialMotionInfo = (SerialMotionInfo)motionInfo;
                dialogResult = form.ShowDialog();
            }
            else if (motionInfo is VirtualMotionInfo)
            {
                VirtualMotionInfoForm form = new VirtualMotionInfoForm();
                form.VirtualMotionInfo = (VirtualMotionInfo)motionInfo;
                dialogResult = form.ShowDialog();
            }

            if (dialogResult == DialogResult.OK)
            {
                dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = motionInfo.Name;
                dataGridViewDeviceList.SelectedRows[0].Cells[3].Value = motionInfo.NumAxis;
            }
        }

        private void EditDigitalIoInfo(DigitalIoInfo digitalIoInfo)
        {
            DialogResult dialogResult = DialogResult.Cancel;
            if (digitalIoInfo is PciDigitalIoInfo || digitalIoInfo is DigitalIoInfoVirtual)
            {
                PciDigitalIoInfoForm form = new PciDigitalIoInfoForm();
                form.PciDigitalIoInfo = (PciDigitalIoInfo)digitalIoInfo;
                dialogResult = form.ShowDialog();
            }
            else if (digitalIoInfo is SlaveDigitalIoInfo)
            {
                SlaveDigitalIoInfoForm form = new SlaveDigitalIoInfoForm();
                form.SlaveDigitalIoInfo = (SlaveDigitalIoInfo)digitalIoInfo;
                form.MotionInfoList = motionInfoList;
                dialogResult = form.ShowDialog();
            }
            else if (digitalIoInfo is SerialDigitalIoInfo)
            {
                SerialDigitalIoInfoForm form = new SerialDigitalIoInfoForm((SerialDigitalIoInfo)digitalIoInfo);
                dialogResult = form.ShowDialog();
            }

            if (dialogResult == DialogResult.OK)
            {
                dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = digitalIoInfo.Name;
            }
        }

        private void EditDaqChannelProperty(DaqChannelProperty daqChannelProperty)
        {
            if(daqChannelProperty.DaqChannelType == DaqChannelType.USB)
            {
                MessageBox.Show("편집할 속성이 존재하지 않는 디바이스입니다.");
                return;
            }

            DaqPropertyForm form = new DaqPropertyForm();
            form.DaqChannelProperty = (DaqChannelProperty)daqChannelProperty;
            if (form.ShowDialog() == DialogResult.OK)
            {
                dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = daqChannelProperty.Name;
            }
        }

        private void EditLightCtrlInfo(LightCtrlInfo lightCtrlInfo)
        {
            DigitalIoHandler digitalIoHandler = new DigitalIoHandler();
            //digitalIoHandler.Build(digitalIoInfoList, motionInfoList);

            LightConfigForm form = new LightConfigForm();
            form.LightCtrlInfo = lightCtrlInfo;
            form.DigitalIoHandler = digitalIoHandler;
            if (form.ShowDialog() == DialogResult.OK)
            {
                lightCtrlInfoList.Remove(lightCtrlInfo);

                lightCtrlInfo = form.LightCtrlInfo;

                lightCtrlInfoList.Add(lightCtrlInfo);

                dataGridViewDeviceList.SelectedRows[0].Cells[1].Value = lightCtrlInfo.Name;
                dataGridViewDeviceList.SelectedRows[0].Cells[2].Value = lightCtrlInfo.ControllerType.ToString();
                dataGridViewDeviceList.SelectedRows[0].Cells[3].Value = lightCtrlInfo.NumChannel.ToString();
                dataGridViewDeviceList.SelectedRows[0].Tag = lightCtrlInfo;
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridViewDeviceList.SelectedRows.Count == 0)
                return;

            if (MessageBox.Show("Do you want to delete the selected device", title, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                object editTarget = dataGridViewDeviceList.SelectedRows[0].Tag;

                dataGridViewDeviceList.Rows.Remove(dataGridViewDeviceList.SelectedRows[0]);

                switch (deviceListType)
                {
                    case DeviceListType.Grabber:
                        grabberInfoList.Remove((GrabberInfo)editTarget);
                        break;
                    case DeviceListType.Motion:
                        motionInfoList.Remove((MotionInfo)editTarget);
                        break;
                    case DeviceListType.DigitalIo:
                        digitalIoInfoList.Remove((DigitalIoInfo)editTarget);
                        break;
                    case DeviceListType.LightController:
                        lightCtrlInfoList.Remove((LightCtrlInfo)editTarget);
                        break;
                    case DeviceListType.Daq:
                        daqChannelPropertyList.Remove((DaqChannelProperty)editTarget);
                        break;
                    case DeviceListType.SerialDevice:
                        serialDeviceInfoList.Remove((SerialDeviceInfo)editTarget);
                        break;
                }
            }
        }

        private void dataGridViewDeviceList_SelectionChanged(object sender, EventArgs e)
        {
            bool enable = (dataGridViewDeviceList.SelectedRows.Count > 0);

            editButton.Enabled = enable;
            deleteButton.Enabled = enable;
            buttonMoveUp.Enabled = enable;
            buttonMoveDown.Enabled = enable;
        }

        private void buttonSelectDepthScanner_Click(object sender, EventArgs e)
        {
            RefreshList(DeviceListType.DepthScanner);
        }

        private void buttonAxisHandlerConfiguration_Click(object sender, EventArgs e)
        {
            if (motionInfoList.Count == 0)
            {
                MessageForm.Show(null, "There is no Motion device.");
                return;
            }

            MotionList motionList = new MotionList();
            try
            {
                motionList.Initialize(motionInfoList, false);
            }
            catch (AlarmException ex)
            {
                ex.ShowMessageBox();
                return;
            }
            motionList.TurnOnServo(true);

            List<string> axisHandlerNames = new List<string>();
            axisHandlerNames.Add("RobotStage");

            //if (MachineSettings.Instance().UseConveyor)
            //    axisHandlerNames.Add("Conveyor");

            AxisConfiguration axisConfiguration = new AxisConfiguration();
            //axisConfiguration.SetupAxisHandler(axisHandlerNames.ToArray());
            axisConfiguration.LoadConfiguration(motionList);

            AxisConfigurationForm form = new AxisConfigurationForm();

            form.Initialize(axisConfiguration, motionList);
            if (form.ShowDialog() == DialogResult.OK)
            {
                axisConfiguration.SaveConfiguration();
            }

            motionList.TurnOnServo(false);
            motionList.Release();
        }

        private void buttonBuildPortMap_Click(object sender, EventArgs e)
        {
            //if (digitalIoInfoList.Count == 0)
            //{
            //    MessageBox.Show("There is no Digital I/O device. Please, add Digital I/O device first.");
            //    return;
            //}

            //DigitalIoHandler digitalIoHandler = new DigitalIoHandler();
            //digitalIoHandler.Build(digitalIoInfoList, motionInfoList);

            //PortMap portMap = new UniEye.Device.PortMap();
            //portMap.SetupPorts();

            //PortMapBuilderForm form = new PortMapBuilderForm(digitalIoHandler, portMap);
            //form.ShowDialog();
        }

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            UiHelper.MoveUp(dataGridViewDeviceList);
        }

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            UiHelper.MoveDown(dataGridViewDeviceList);
        }

        private void buttonBarcodePrinter_Click(object sender, EventArgs e)
        {

        }

        private void buttonPortMap_Click(object sender, EventArgs e)
        {
            if (digitalIoInfoList.Count == 0)
            {
                MessageForm.Show(null, "There is no DIO installed.");
                return;
            }

            IDigitalIo dIo = new DigitalIoVirtual("Virtual");
            dIo.Initialize(digitalIoInfoList[0]);

            //IDigitalIo dIo = DigitalIoFactory.Create(digitalIoInfoList[0]);
            //if (dIo == null)
            //    return;

            PortMapBase portMap = SystemManager.Instance().DeviceBox.PortMap;
            //portMap.Load();

            PortMapBuilderForm form = new PortMapBuilderForm(dIo, portMap);
            if (form.ShowDialog() == DialogResult.OK)
            {
                portMap.Save();
            }

            dIo.Release();
        }

        private void buttonCameraCalibration_Click(object sender, EventArgs e)
        {
            DeviceBox deviceBox = SystemManager.Instance().DeviceBox;
            CameraCalibrationForm form = new CameraCalibrationForm();
            AlgorithmStrategy calibrationAlgorithmStrategy = new AlgorithmStrategy(Calibration.TypeName, ImagingLibrary.OpenCv, "");
            bool good = false;
            try
            {
                AlgorithmBuilder.AddStrategy(calibrationAlgorithmStrategy);
                AlgorithmBuilder.SetAlgorithmEnabled(Calibration.TypeName, true);

                SimpleProgressForm spForm = new SimpleProgressForm();

                spForm.Show(() =>
                {
                    try
                    {
                        deviceBox.InitializeCameraAndLight();
                        form.Initialize();
                        good = true;
                    }
                    catch(AlarmException aex)
                    {
                        good = false;
                        aex.ShowMessageBox();
                    }

                    //deviceBox.Initialize(null);
                    //form.Initialize();
                });

                if (good)
                    form?.ShowDialog();
            }
            finally
            {
                AlgorithmBuilder.RemoveStrategy(calibrationAlgorithmStrategy);
                deviceBox.Release();
            }
        }
    }
}
