using System;
using System.Windows.Forms;
using DynMvp.Authentication;
using DynMvp.UI.Touch;
using UniEye.Base.Device;
using UniEye.Base.UI;
using UniEye.Base.UI.CameraCalibration;

namespace UniScanS.Screen.Settings.Inspector.UI
{
    public partial class SettingPage : UserControl, ISettingPage, IMainTabPage, IUserHandlerListener
    {
        string developerTabKey = "Developer";
        
        public SettingPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            UserHandler.Instance().AddListener(this);
        }

        public void Initialize()
        {

        }

        private void buttonSelectLocalFolder_Click(object sender, EventArgs e)
        {
        //    FolderBrowserDialog dlg = new FolderBrowserDialog();
        //    if (dlg.ShowDialog() == DialogResult.OK)
        //    {
        //        localResultPath.Text = dlg.SelectedPath;
        //    }
        }

        public void EnableControls()
        {

        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {   
            if (visibleFlag == true)
            {
                UpdateData();
            }
        //    if (visibleFlag == true)
        //    {
        //        LoadSettings();
        //    }
        //    else
        //    {
        //        SaveSettings();
        //        if (CustomSettings.Instance().SystemType == SystemType.Monitor)
        //        {
        //            ((MpisMonitorSystemManager)SystemManager.Instance()).MonitoringServer.SendSyncSetting();
        //            ((Operation.UI.MLCCSMainForm)this.ParentForm).WaitJobDone("Inspector Sync..");
        //        }
        //    }
        }

        public void UpdateData()
        {
        }

        public void SetData()
        {

        }
        
        public void LoadSettings()
        {

        }

        public void SaveSettings()
        {

        }

        delegate void UserChangedDelegatge();
        public void UserChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegatge(UserChanged));
                return;
            }

            User curUser = UserHandler.Instance().CurrentUser;

            settingTabControl.Tabs[tabPageDeveloper.Tab.Key].Visible = curUser.Id.ToUpper() == "DEVELOPER";
        }

        private void tabControlParam_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {

        }

        private void storingDays_ValueChanged(object sender, EventArgs e)
        {
        }

        private void startYPosition_ValueChanged(object sender, EventArgs e)
        {

        }

        private void buttonCalibration_Click(object sender, EventArgs e)
        {
            //CameraCalibrationForm form = new CameraCalibrationForm();
            //form.Initialize();
            //form.ShowDialog();

            //cameraResolution.Text = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width.ToString("0.000 [um/px]");
        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            throw new NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeviceBox deviceBox = SystemManager.Instance().DeviceBox;
            CameraCalibrationForm form = new CameraCalibrationForm();

            form.Initialize();
            form?.ShowDialog();
        }

        public void EnableControls(UserType userType)
        {
            throw new NotImplementedException();
        }
    }
}
