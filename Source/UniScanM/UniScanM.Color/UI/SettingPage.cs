using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Device.Serial;
using System.IO;
using UniEye.Base.Settings;
using DynMvp.UI;
using UniEye.Base.UI;
using UniEye.Base.Settings.UI;
using System.Threading;
using UniEye.Base;
using UniScanM.ColorSens.Operation;
using DynMvp.Devices.UI;
using DynMvp.UI.Touch;
using DynMvp.Devices.MotionController;
using UniScanM.ColorSens.Settings;
using UniScanM.UI.ControlPanel;
using DynMvp.Authentication;

namespace UniScanM.ColorSens.UI
{
    public partial class SettingPage : UserControl, IMainTabPage, ISettingPage
    {
        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public SettingPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
        }

        private void SettingPanel_Load(object sender, EventArgs e)
        {
            propertyGrid_Config.SelectedObject = ColorSensorSettings.Instance();
        }

        public void EnableControls(UserType user)
        {
            if (user == UserType.Admin)
                ultraExpandableGroupBox1.Visible = true;
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            this.Visible = visibleFlag;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void SaveSettings()
        {
            throw new NotImplementedException();
        }

        private void buttonMachineIF_Click(object sender, EventArgs e)
        {
            MachineIfPanel machineIfPanel = new MachineIfPanel();
            machineIfPanel.Initialize(MachineSettings.Instance().MachineIfSetting);
            machineIfPanel.Dock = DockStyle.Fill;

            Form machineIfForm = new Form();
            machineIfForm.Controls.Add(machineIfPanel);
            machineIfForm.Size = machineIfPanel.Size;
            //machineIfForm.AutoSize = true;
            //machineIfForm.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            machineIfForm.TopMost = true;
            machineIfForm.Show();
        }

        private void buttonCamera_Click(object sender, EventArgs e)
        {
            CameraControlPanel cameraControlPanel = new CameraControlPanel();
            cameraControlPanel.Initialize(
                SystemManager.Instance().DeviceBox.ImageDeviceHandler, 
                SystemManager.Instance().DeviceBox.CameraCalibrationList, 
                SystemManager.Instance().DeviceBox.LightCtrlHandler
                );
            //cameraControlPanel.Dock = DockStyle.Fill;

            Form cameraControlForm = new Form();
            cameraControlForm.Controls.Add(cameraControlPanel);
            cameraControlForm.Size = cameraControlPanel.Size;
            cameraControlForm.AutoSize = true;
            cameraControlForm.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            cameraControlForm.TopMost = true;
            cameraControlForm.Show();
        }

        void IMainTabPage.EnableControls(UserType user)
        {
           // throw new NotImplementedException();
        }

        void IPage.UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        void IPage.PageVisibleChanged(bool visibleFlag)
        {
            this.Visible = visibleFlag;
        }

        private void button_Set_Click(object sender, EventArgs e)
        {
            ColorSensorSettings.Instance().Save();
        }

        private void button_Reset_Click(object sender, EventArgs e)
        {
            ColorSensorSettings.Instance().Load();
        }
    }
}
