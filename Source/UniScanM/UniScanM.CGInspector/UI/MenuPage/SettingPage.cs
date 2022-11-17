using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanM.CGInspector.UI.ControlPanel;
using DynMvp.Device.Serial;
using System.IO;
using UniEye.Base.Settings;
using DynMvp.UI;
using UniEye.Base.UI;
using UniEye.Base.Settings.UI;
using UniScanM.CGInspector.UI.MenuPage.SettingPanel;
using System.Threading;
using UniEye.Base;
using UniScanM.CGInspector.Operation;
using DynMvp.Devices.UI;
using DynMvp.UI.Touch;
using DynMvp.Devices.MotionController;
using UniEye.Base.Data;
using DynMvp.Authentication;
using Infragistics.Win.Misc;
using Infragistics.Win;
using DynMvp.Base;

namespace UniScanM.CGInspector.UI.MenuPage
{
    public partial class SettingPage : UserControl, IMainTabPage, ISettingPage, IOpStateListener, IUserHandlerListener, IMultiLanguageSupport
    {
        //Timer speedTimer = null;

        SettingPageParamPanel settingPageParamPanel = null;
        SettingPageMonitoringPanel settingPageMonitoringPanel = null;
        int oldSelectedTabIndex = -1;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public SettingPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.settingPageParamPanel = new UniScanM.CGInspector.UI.MenuPage.SettingPanel.SettingPageParamPanel();
            this.settingPageParamPanel.Dock = DockStyle.Fill;
            tabPageParam.Controls.Add(settingPageParamPanel);

            this.settingPageMonitoringPanel = new UniScanM.CGInspector.UI.MenuPage.SettingPanel.SettingPageMonitoringPanel();
            this.settingPageMonitoringPanel.Dock = DockStyle.Fill;
            tabPageView.Controls.Add(settingPageMonitoringPanel);

            SystemState.Instance().AddOpListener(this);
            UserHandler.Instance().AddListener(this);
            StringManager.AddListener(this);
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
        }

        private void SettingPanel_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            oldSelectedTabIndex = 0;
        }

        private void buttonEncoder_Click(object sender, EventArgs e)
        {
            SerialEncoder serialEncoder = SystemManager.Instance().DeviceBox.SerialDeviceHandler.Find(f => f.DeviceInfo.DeviceType == ESerialDeviceType.SerialEncoder) as SerialEncoder;
            if (serialEncoder == null)
            {
                MessageForm.Show(null, "There is no Encoder Device");
                return;
            }

            Form encoderForm = new Form();
            SerialEncoderPanel virtualControlPanel = new SerialEncoderPanel();
            virtualControlPanel.Initialize(serialEncoder);
            encoderForm.Size = virtualControlPanel.Size;
            virtualControlPanel.Dock = DockStyle.Fill;
            encoderForm.Controls.Add(virtualControlPanel);
            encoderForm.Text = "Encoder Setting Form";
            encoderForm.TopMost = true;
            encoderForm.Show();
        }

        private void buttonMotion_Click(object sender, EventArgs e)
        {
            AxisConfiguration axisConfiguration = SystemManager.Instance().DeviceBox.AxisConfiguration;
            if (axisConfiguration.Count == 0)
            {
                MessageForm.Show(null, "There is no Axis");
                return;
            }

            //((MainForm)SystemManager.Instance().MainForm).ChangeStartMode(StartMode.Stop);

            MotionControlForm motionControlForm = new MotionControlForm();
            motionControlForm.Intialize(axisConfiguration);
            motionControlForm.Show();
        }

        private void buttonCamera_Click(object sender, EventArgs e)
        {
            //((MainForm)SystemManager.Instance().MainForm).ChangeStartMode(StartMode.Stop);

            CameraControlPanel cameraControlPanel = new CameraControlPanel();
            cameraControlPanel.Initialize(SystemManager.Instance().DeviceBox.ImageDeviceHandler, SystemManager.Instance().DeviceBox.CameraCalibrationList, SystemManager.Instance().DeviceBox.LightCtrlHandler);

            Form cameraControlForm = new Form();
            cameraControlForm.Controls.Add(cameraControlPanel);
            cameraControlForm.Size = cameraControlPanel.Size;
            cameraControlForm.AutoSize = true;
            cameraControlForm.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            cameraControlForm.TopMost = true;
            cameraControlForm.Show();
        }

        private void buttonSimulator_Click(object sender, EventArgs e)
        {
            Test.AlgorithmSimulatorForm sheetFIndTestForm = new Test.AlgorithmSimulatorForm();
            sheetFIndTestForm.ShowDialog();
        }
        
        public void EnableControls(UserType user)
        {

        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            Color buttonBackColor = visibleFlag ? Color.CornflowerBlue : Color.Transparent;
            if (this.showHideControl != null)
                ((UltraButton)this.showHideControl).Appearance.BackColor = buttonBackColor;

            this.Visible = visibleFlag;
            if (visibleFlag == false)
            {
                UserHandler.Instance().CurrentUser = UserHandler.Instance().GetUser("op");

                if (oldSelectedTabIndex >= 0)
                {
                    UniEye.Base.UI.IPage oldPage = (UniEye.Base.UI.IPage)tabControl1.TabPages[oldSelectedTabIndex].Controls[0];
                    oldPage.PageVisibleChanged(false);
                }
            }
            else
            {
                if (oldSelectedTabIndex >= 0)
                {
                    UniEye.Base.UI.IPage oldPage = (UniEye.Base.UI.IPage)tabControl1.TabPages[oldSelectedTabIndex].Controls[0];
                    oldPage.PageVisibleChanged(true);
                }
            }
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void SaveSettings()
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AdditionalSettingsForm form = new AdditionalSettingsForm();
            form.SetInstance(AdditionalSettings.Instance());
            form.Size = new Size(512, 640);
            form.ShowDialog();

            AdditionalSettings.Instance().Save();
        }

        private void buttonEncVerify_Click(object sender, EventArgs e)
        {
            Test.EncoderVerifier form = new Test.EncoderVerifier();
            form.Initialize(SystemManager.Instance().DeviceController.Convayor, SystemManager.Instance().DeviceBox.SerialDeviceHandler.Find(f => f.DeviceInfo.DeviceType == ESerialDeviceType.SerialEncoder));
            form.ShowDialog();
        }

        public void UpdateControl(string item, object value)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new UpdateControlDelegate(UpdateControl), item, value);
                return;
            }

            switch(item)
            {
                case "SheetLength":
                    this.settingPageParamPanel.UpdateControl(item, value);
                    break;
                case "FullImage":
                case "InspPos":
                    this.settingPageMonitoringPanel.UpdateControl(item, value);
                    break;
            }
        }
        
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (oldSelectedTabIndex >= 0)
            {
                UniEye.Base.UI.IPage oldPage = (UniEye.Base.UI.IPage)tabControl1.TabPages[oldSelectedTabIndex].Controls[0];
                oldPage.PageVisibleChanged(false);
            }
            int newSelectedTabIndex = e.TabPageIndex;
            UniEye.Base.UI.IPage newPage = (UniEye.Base.UI.IPage)tabControl1.TabPages[newSelectedTabIndex].Controls[0];
            newPage.PageVisibleChanged(true);

            oldSelectedTabIndex = newSelectedTabIndex;

        }

        private void buttonMachineIF_Click(object sender, EventArgs e)
        {
            SettingPageMachineIfPanel machineIfPanel = new SettingPageMachineIfPanel();
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

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            ultraExpandableGroupBoxSettings.Expanded = false;
            ultraExpandableGroupBoxDeveloper.Expanded = false;

            UserHandler.Instance().CurrentUser = UserHandler.Instance().GetUser("op");
        }

        private void ultraExpandableGroupBox_ExpandedStateChanging(object sender, CancelEventArgs e)
        {
            //    UltraExpandableGroupBox box = sender as UltraExpandableGroupBox;
            //    if (box.Expanded == false)
            //    // 열고자 할 때만 작동
            //    {
            //        User user = UserHandler.Instance().CurrentUser;
            //        if (UserHandler.Instance().CurrentUser.SuperAccount == false)
            //        {
            //            LogInForm loginForm = new LogInForm();
            //            loginForm.ShowDialog();
            //            if (loginForm.DialogResult == DialogResult.OK)
            //                UserHandler.Instance().CurrentUser = loginForm.LogInUser;
            //        }

            //        if (UserHandler.Instance().CurrentUser.SuperAccount == false)
            //            e.Cancel = true;
            //    }
        }

        public void UserChanged()
        {
            if(InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegate(UserChanged));
                return;
            }
            bool open = UserHandler.Instance().CurrentUser.IsSuperAccount;

                ultraExpandableGroupBoxSettings.Expanded = open;
                ultraExpandableGroupBoxDeveloper.Expanded = open;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
    }
}
