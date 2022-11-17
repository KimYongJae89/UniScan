using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using DynMvp.Devices.UI;
using DynMvp.UI;
using DynMvp.Authentication;
using DynMvp.Base;
using UniEye.Base.Settings;
using UniEye.Base.UI.CameraCalibration;

namespace UniEye.Base.UI
{
    public partial class SettingPage : UserControl, ISettingPage, IMainTabPage
    {
        bool resultNgValue = false;

        // modaless로 구동하기 위함.
        IoPortViewer ioPortViewer = null;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public SettingPage()
        {
            InitializeComponent();
            checkBoxShowCenterGuide.Text = StringManager.GetString(this.GetType().FullName, checkBoxShowCenterGuide.Text);
            checkBoxDebugMode.Text = StringManager.GetString(this.GetType().FullName, checkBoxDebugMode.Text);
            useDefectReview.Text = StringManager.GetString(this.GetType().FullName, useDefectReview.Text);
            labelCenterGuideOffsetX.Text = StringManager.GetString(this.GetType().FullName, labelCenterGuideOffsetX.Text);
            labelCenterGuideOffsetY.Text = StringManager.GetString(this.GetType().FullName, labelCenterGuideOffsetY.Text);
            labelCenterGuideThickness.Text = StringManager.GetString(this.GetType().FullName, labelCenterGuideThickness.Text);
            changeUserButton.Text = StringManager.GetString(this.GetType().FullName, changeUserButton.Text);
            saveButton.Text = StringManager.GetString(this.GetType().FullName, saveButton.Text);
            labelLocalResultPath.Text = StringManager.GetString(this.GetType().FullName, labelLocalResultPath.Text);
            groupBoxRemoteBackup.Text = StringManager.GetString(this.GetType().FullName, groupBoxRemoteBackup.Text);
            useRemoteBackup.Text = StringManager.GetString(this.GetType().FullName, useRemoteBackup.Text);
            labelRemoteResultPath.Text = StringManager.GetString(this.GetType().FullName, labelRemoteResultPath.Text);
            useNetworkFolder.Text = StringManager.GetString(this.GetType().FullName, useNetworkFolder.Text);
            label3.Text = StringManager.GetString(this.GetType().FullName, label3.Text);
            buttonCameraCalibration.Text = StringManager.GetString(this.GetType().FullName, buttonCameraCalibration.Text);
            buttonCameraCalibration.Text = StringManager.GetString(this.GetType().FullName, buttonCameraCalibration.Text);
            buttonShowIoPortViewer.Text = StringManager.GetString(this.GetType().FullName, buttonShowIoPortViewer.Text);
            buttonUserManager.Text = StringManager.GetString(this.GetType().FullName, buttonUserManager.Text);
            labelTrigDelay.Text = StringManager.GetString(this.GetType().FullName, labelTrigDelay.Text);
            useRejectPusher.Text = StringManager.GetString(this.GetType().FullName, useRejectPusher.Text);

            groupRejectPusher.Text = StringManager.GetString(this.GetType().FullName, groupRejectPusher.Text);
            tabPageData.Text = StringManager.GetString(this.GetType().FullName,tabPageData.Text);
            tabPageGeneral.Text = StringManager.GetString(this.GetType().FullName, tabPageGeneral.Text);
            tabPageMachine.Text = StringManager.GetString(this.GetType().FullName, tabPageMachine.Text);

            // ChangeVisibleControl();
        }

        public void Initialize()
        {

        }

        private void buttonSelectRemoteFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                remoteResultPath.Text = dlg.SelectedPath;
            }
        }

        public void SaveSettings()
        {
            PathSettings.Instance().RemoteResult = remoteResultPath.Text;
            OperationSettings.Instance().UseRemoteBackup = useRemoteBackup.Checked;
            PathSettings.Instance().Result = localResultPath.Text;
            OperationSettings.Instance().CpuUsage = Convert.ToInt32(cpuUsage.Text);
            OperationSettings.Instance().UseDefectReview = useDefectReview.Checked;
            PathSettings.Instance().UseNetworkFolder = useNetworkFolder.Checked;

            OperationSettings.Instance().ShowCenterGuide = checkBoxShowCenterGuide.Checked;
            OperationSettings.Instance().UseRejectPusher = useRejectPusher.Checked;

            MachineSettings.Instance().PixelRes3D = (float)pixelRes3d.Value;
            MachineSettings.Instance().DefaultExposureTimeMs = (int)defaultExposureTime.Value;

            OperationSettings.Instance().CenterGuidePos = new Point(Convert.ToInt32(txtCenterGuideOffsetX.Text), Convert.ToInt32(txtCenterGuideOffsetY.Text));
            OperationSettings.Instance().CenterGuideThickness = Convert.ToInt32(txtCenterGuideThickness.Text);

            TimeSettings.Instance().RejectWaitTimeMs = Convert.ToInt32(rejectWaitTime.Text);
            TimeSettings.Instance().RejectPusherPushTimeMs = Convert.ToInt32(rejectPusherPushTime.Text);
            TimeSettings.Instance().RejectPusherPullTimeMs = Convert.ToInt32(rejectPusherPullTime.Text);

            TimeSettings.Instance().TriggerDelayMs = Convert.ToInt32(triggerDelay.Text);
            TimeSettings.Instance().InspectionDelay = Convert.ToInt32(inspectionDelayTime.Value);
            SystemManager.Instance().DeviceBox.ImageDeviceHandler.SetTriggerDelay(TimeSettings.Instance().TriggerDelayMs);

            OperationSettings.Instance().Save();
            MachineSettings.Instance().Save();
            TimeSettings.Instance().Save();
        }

        private void buttonSelectLocalFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                localResultPath.Text = dlg.SelectedPath;
            }
        }

        private void LoadSettings()
        {
            cpuUsage.Text = OperationSettings.Instance().CpuUsage.ToString();
            useRemoteBackup.Checked = OperationSettings.Instance().UseRemoteBackup;
            localResultPath.Text = PathSettings.Instance().Result;
            remoteResultPath.Text = PathSettings.Instance().RemoteResult;
            useDefectReview.Checked = OperationSettings.Instance().UseDefectReview;
            useNetworkFolder.Checked = PathSettings.Instance().UseNetworkFolder;
            checkBoxShowCenterGuide.Checked = OperationSettings.Instance().ShowCenterGuide;
            useRejectPusher.Checked = OperationSettings.Instance().UseRejectPusher;

            txtCenterGuideOffsetX.Text = OperationSettings.Instance().CenterGuidePos.X.ToString();
            txtCenterGuideOffsetY.Text = OperationSettings.Instance().CenterGuidePos.Y.ToString();
            txtCenterGuideThickness.Text = OperationSettings.Instance().CenterGuideThickness.ToString();

            rejectWaitTime.Text = TimeSettings.Instance().RejectWaitTimeMs.ToString();
            rejectPusherPushTime.Text = TimeSettings.Instance().RejectPusherPushTimeMs.ToString();
            rejectPusherPullTime.Text = TimeSettings.Instance().RejectPusherPullTimeMs.ToString();

            triggerDelay.Text = TimeSettings.Instance().TriggerDelayMs.ToString();
            inspectionDelayTime.Value = TimeSettings.Instance().InspectionDelay;
            defaultExposureTime.Value = MachineSettings.Instance().DefaultExposureTimeMs;
            pixelRes3d.Value = (Decimal)MachineSettings.Instance().PixelRes3D;
        }

        //private void ChangeVisibleControl()
        //{
        //    if(OperationSettings.Instance().SystemType == SystemType.UniEyeS)
        //    {
        //        groupRejectPusher.Visible = false;
        //        useRejectPusher.Visible = false;
        //        labelTrigDelay.Visible = false;
        //        triggerDelay.Visible = false;
        //        checkBoxDebugMode.Visible = false;
        //    }
        //}

        private void buttonUserManager_Click(object sender, EventArgs e)
        {
            if (File.Exists("UserManager.exe") == false)
            {
                MessageBox.Show("Can't find UserManager.exe");
                return;
            }

            Process.Start("UserManager.exe", "nologinneeded");
        }

        private void checkBoxDebugMode_CheckedChanged(object sender, EventArgs e)
        {
            OperationSettings.Instance().SaveDebugImage = checkBoxDebugMode.Checked;
        }

        private void checkBoxShowCenterLine_CheckedChanged(object sender, EventArgs e)
        {
            OperationSettings.Instance().ShowCenterGuide = checkBoxShowCenterGuide.Checked;
        }

        private void buttonShowIoPortViewer_Click(object sender, EventArgs e)
        {
            if (ioPortViewer == null)
            {
                ioPortViewer = new IoPortViewer(SystemManager.Instance().DeviceBox.DigitalIoHandler, SystemManager.Instance().DeviceBox.PortMap);
                ioPortViewer.TopMost = true;
            }

            if (ioPortViewer.Visible)
            {
                return;
            }
            ioPortViewer.Show(this);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        private void changeUserButton_Click(object sender, EventArgs e)
        {
            LogInForm loginForm = new LogInForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                UserHandler.Instance().CurrentUser = loginForm.LogInUser;
                SystemManager.Instance().MainForm.EnableTabs();
            }
        }

        private void useRejectCylinder_CheckedChanged(object sender, EventArgs e)
        {
            OperationSettings.Instance().UseRejectPusher = useRejectPusher.Checked;
        }

        private void buttonCameraAlign_Click(object sender, EventArgs e)
        {
            OverlayForm form = new OverlayForm();
            form.ShowDialog();
        }

        private void buttonMotionController_Click(object sender, EventArgs e)
        {
            MotionControlForm motionController = new MotionControlForm();
            motionController.Intialize(SystemManager.Instance().DeviceBox.AxisConfiguration);
            motionController.ShowDialog();
        }

        private void buttonTowerLampConfig_Click(object sender, EventArgs e)
        {
            //TowerLampConfigForm form = new TowerLampConfigForm();
            //form.TowerLamp = machine.TowerLamp;
            //DialogResult res = form.ShowDialog();
            //if (res == DialogResult.OK)
            //{
            //    form.TowerLamp.Save();
            //}
        }

        private void buttonRobotCalibration_Click(object sender, EventArgs e)
        {
            RobotCalibrationForm robotMapForm = new RobotCalibrationForm();
            robotMapForm.Initialize();
            if (robotMapForm.ShowDialog() == DialogResult.OK)
            {
                // Do something
            }
        }

        public void EnableControls(UserType userType)
        {

        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == false)
                SaveSettings();
            else
                LoadSettings();
        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }
    }
}
