using System;
using System.Windows.Forms;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.UI.Touch;
using UniEye.Base;
using UniEye.Base.UI;
using UniEye.Base.Device;
using UniEye.Base.Data;
using UniEye.Base.Settings;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using DynMvp.Authentication;
using Infragistics.Win.Misc;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace UniScan.UI
{
    public partial class MainForm : Form, LoggingTarget, IMainForm, IUserHandlerListener
    {
        bool onRemoteTeachingMode = false;
        bool onRemoteCommand = false;

        CancellationTokenSource cancellationTokenSource;

        private ModelManagePage modelManagerPage;
        public ModelManagePage ModelManagerPage
        {
            get { return modelManagerPage; }
        }

        private ReportPage reportPage;
        public IReportPage ReportPage
        {
            get { return reportPage; }
        }

        private SettingPage settingPage;
        public ISettingPage SettingPage
        {
            get { return settingPage; }
        }
        /*
        private ReportPage reportPage;
        public ReportPage ReportPage
        {
            get { return reportPage; }
        }

        private SettingPage settingPage;
        public SettingPage SettingPage
        {
            get { return settingPage; }
        }
        */

        private TeachingPage teachingPage;
        public TeachingPage TeachingPage
        {
            get { return teachingPage; }
        }

        private InspectionPage inspectionPage;
        public IInspectionPage MonitoringPage
        {
            get { return inspectionPage; }
        }

        private AlarmMessageForm alarmMessageForm = new AlarmMessageForm();
        public AlarmMessageForm AlarmMessageForm
        {
            get { return alarmMessageForm; }
        }

        IReportPage IMainForm.ReportPage => throw new NotImplementedException();

        ISettingPage IMainForm.SettingPage => throw new NotImplementedException();

        //IModellerPage IMainForm.ModellerPage => throw new NotImplementedException();

        public IInspectionPage InspectPage => throw new NotImplementedException();

        public ISettingPage TeachPage => throw new NotImplementedException();

        IModelManagerPage IMainForm.ModelManagerPage => throw new NotImplementedException();

        ITeachPage IMainForm.TeachPage => throw new NotImplementedException();

        public MainForm()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            LogHelper.LoggingTarget = this;

            modelManagerPage = new ModelManagePage();
            reportPage = new ReportPage();
            settingPage = new SettingPage();

            this.tabPageModel.Controls.Add(this.modelManagerPage);
            this.tabPageReport.Controls.Add(this.reportPage);
            this.tabPageSetting.Controls.Add(this.settingPage);

            LogHelper.Debug(LoggerType.StartUp, "Init Model Page");

            this.modelManagerPage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.modelManagerPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelManagerPage.Location = new System.Drawing.Point(0, 313);
            this.modelManagerPage.Name = "modelManagerPage";
            this.modelManagerPage.Size = new System.Drawing.Size(466, 359);
            this.modelManagerPage.TabIndex = 0;

            LogHelper.Debug(LoggerType.StartUp, "Init Report Page");

            this.reportPage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.reportPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportPage.Location = new System.Drawing.Point(0, 313);
            this.reportPage.Name = "reportPage";
            this.reportPage.Size = new System.Drawing.Size(466, 359);
            this.reportPage.TabIndex = 0;
            this.reportPage.Initialize();

            LogHelper.Debug(LoggerType.StartUp, "Init setting Page");

            this.settingPage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.settingPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingPage.Location = new System.Drawing.Point(0, 313);
            this.settingPage.Name = "settingPage";
            this.settingPage.Size = new System.Drawing.Size(466, 359);
            this.settingPage.TabIndex = 0;
            this.settingPage.Initialize();
            this.settingPage.LoadSettings();

            this.alarmMessageForm.Hide();

            teachingPage = new TeachingPage();
            this.tabPageTeach.Controls.Add(this.teachingPage);

            LogHelper.Debug(LoggerType.StartUp, "Init Teaching Page");

            this.teachingPage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.teachingPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.teachingPage.Location = new System.Drawing.Point(0, 313);
            this.teachingPage.Name = "TeachingPage";
            this.teachingPage.Size = new System.Drawing.Size(466, 359);
            this.teachingPage.TabIndex = 0;

            inspectionPage = new InspectionPage();
            //this.tabPageInspect.Controls.Add(this.inspectionPage);

            LogHelper.Debug(LoggerType.StartUp, "Init Inspection Page");

            this.inspectionPage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.inspectionPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inspectionPage.Location = new System.Drawing.Point(0, 313);
            this.inspectionPage.Name = "Inspection Page";
            this.inspectionPage.Size = new System.Drawing.Size(466, 359);
            this.inspectionPage.TabIndex = 0;

            tableLayoutPanel2.RowStyles[0].Height = 0;

            timer.Start();

            UserHandler.Instance().AddListener(this);
            User curUser = UserHandler.Instance().CurrentUser;
#if DEBUG
            curUser = new User("Developer", "", UserType.Admin);
#else
            curUser= new User("Op", "", UserType.Operator);
#endif
            UserHandler.Instance().CurrentUser = curUser;

            EnableTabs("Inspect", false);
            //EnableTabs("Teach", false);
        }

        public void Log(string messgae)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new LogDelegate(Log), messgae);
                return;
            }

            if (logList.Items.Count > 2000)
                logList.Items.RemoveAt(0);

            int index = logList.Items.Add(messgae);
            logList.TopIndex = index;
        }

        public void ChangeInspectionSize()
        {
            throw new NotImplementedException();
        }

        public void EnableTabs()
        {

        }

        delegate void EnableTabsDelegate2(string key, bool enable);
        public void EnableTabs(string key, bool enable)
        {
            if (InvokeRequired)
            {
                Invoke(new EnableTabsDelegate2(EnableTabs), key, enable);
                return;
            }

            tabControlMain.Tabs[key].Enabled = enable;
        }

        public void InspectionFinished(InspectionResult inspectionResult)
        {
            throw new NotImplementedException();
        }

        public void InspectionStarted()
        {

        }

        public void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, InspectionResult inspectionResult)
        {
            throw new NotImplementedException();
        }

        public void Load2dImage(int cameraIndex, int stepIndex, int lightTypeIndex)
        {
            throw new NotImplementedException();
        }

        public void ModifyTeaching(string imagePath)
        {
            throw new NotImplementedException();
        }

        public void PrepareInspection()
        {
            //throw new NotImplementedException();
        }

        public void ProductInspected(InspectionResult inspectionResult)
        {
            inspectionPage.ProductInspected(inspectionResult);
        }

        public void PostInspection(InspectionResult inspectionResult)
        {

        }

        public void StopInspection()
        {
            //throw new NotImplementedException();
        }

        public void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult)
        {

        }

        public void TargetInspected(Target target, InspectionResult targetInspectionResult)
        {

        }

        public void UpdateButton()
        {
        }

        public void UpdateImage(DeviceImageSet deviceImageSet, int groupId, InspectionResult inspectionResult)
        {

        }

        public void UpdateInspectionNo(string inspectionNo)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Size = new System.Drawing.Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            this.WindowState = FormWindowState.Maximized;

            alarmMessageForm.ShowDialog();
        }

        private void errorManager_ResetAlarmStatus()
        {
            throw new NotImplementedException();
        }

        delegate void UpdateMainTabDelegate(bool Teached);
        public void UpdateMainTab(bool Teached)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateMainTabDelegate(UpdateMainTab), Teached);
                return;
            }

            if (SystemManager.Instance().CurrentModel == null)
            {
                EnableTabs("Teach", false);
                EnableTabs("Inspect", false);
                TabChange("Model");
                return;
            }
            
            //EnableTabs("Teach", true);
            if (Teached == true)
            {
                EnableTabs("Inspect", true);
            }
        }

        private void tabControlMain_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            LogHelper.Debug(LoggerType.Function, "Start tabControlMain_SelectedTabChanged.");

            if (e.PreviousSelectedTab != null)
            {
                Infragistics.Win.UltraWinTabControl.UltraTabPageControl preTabControl = e.PreviousSelectedTab.TabPage;

                if (preTabControl.Controls.Count > 0 && preTabControl.Controls[0] is IMainTabPage)
                {
                    IMainTabPage preTabPage = (IMainTabPage)preTabControl.Controls[0];
                    //preTabPage.TabPageVisibleChanged(false);
                }
            }
            
            Infragistics.Win.UltraWinTabControl.UltraTabPageControl curTabControl = e.Tab.TabPage;
            if (curTabControl.Controls.Count > 0 && curTabControl.Controls[0] is IMainTabPage)
            {                
                IMainTabPage curTabPage = (IMainTabPage)curTabControl.Controls[0];
                //curTabPage.TabPageVisibleChanged(true);
            }

            LogHelper.Debug(LoggerType.Function, "End tabControlMain_SelectedTabChanged.");
        }

        private void tabControlMain_SelectedTabChanging(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangingEventArgs e)
        {
            if (onRemoteTeachingMode == true)
            {
                e.Cancel = true;
            }
            else if (e.Tab.Key == "Exit")
            {
                e.Cancel = true;
                this.Close();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ReleaseIo();
        }

        public void ReleaseIo()
        {
            DeviceBox deviceBox = SystemManager.Instance().DeviceBox;
            SystemManager.Instance().DeviceController.TowerLamp?.Stop();
            try
            {
                PortList portList = deviceBox.PortMap.OutPortList;
                foreach (IoPort port in portList)
                {
                    deviceBox.DigitalIoHandler.WriteOutput(port, false);
                }
                deviceBox.Release();
                SystemManager.Instance().DeviceController.Release();
            }
            catch (InvalidCastException)
            {
                return;
            }

            if (deviceBox.LightCtrlHandler != null)
                deviceBox.LightCtrlHandler.TurnOff();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CheckFormCloseing();
        }

        private bool CheckFormCloseing()
        {
            if (SystemState.Instance().OnInspectOrWait)
            {
                MessageForm.Show(this, "Please, Stop the inspection.", "UniEye");
                return false;
            }

            if (MessageForm.Show(this, "Do you want to exit program?", MessageFormType.YesNo) == DialogResult.No)
            {
                return false;
            }

            UniScan.Data.Model model = (UniScan.Data.Model)SystemManager.Instance().CurrentModel;
            //if (model != null && model.Modified)

            return true;
        }

        delegate void TabChangeDelegate(string key);
        public void TabChange(string key)
        {
            if (InvokeRequired)
            {
                Invoke(new TabChangeDelegate(TabChange), key);
                return;
            }

            if (tabControlMain.Tabs[key].Visible == true && tabControlMain.Tabs[key].Enabled == true)
                tabControlMain.SelectedTab = tabControlMain.Tabs[key];
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            DateTime curTime = DateTime.Now;
            labelDate.Text = curTime.ToString("yyyy - MM - dd");
            labelTime.Text = curTime.ToString("HH : mm");
        }

        private void labelUserName_Click(object sender, EventArgs e)
        {
            LogInForm loginForm = new LogInForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                UserHandler.Instance().CurrentUser = loginForm.LogInUser;
            }
        }

        delegate void EnableTabsDelegate(bool enable);
        public void EnableTabs(bool enable)
        {
            if (InvokeRequired)
            {
                Invoke(new EnableTabsDelegate(EnableTabs), enable);
                return;
            }

            foreach (Infragistics.Win.UltraWinTabControl.UltraTab tab in tabControlMain.Tabs)
            {
                tab.Enabled = enable;
            }
        }

        public void UserChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegate(UserChanged));
                return;
            }

            User user = UserHandler.Instance().CurrentUser;
            labelUserName.Text = user.Id;
            tabControlMain.Tabs["Setting"].Visible = UserHandler.Instance().CurrentUser.SuperAccount;

            if (user.SuperAccount == false)
            {
                if (tabControlMain.SelectedTab != null && tabControlMain.SelectedTab.Key == "Setting")
                {
                    tabControlMain.SelectedTab = tabControlMain.Tabs["Model"];
                }
            }
            //if (MachineSettings.Instance().VirtualMode)
            //    tabControlMain.Tabs["Setting"].Visible = true;
        }

        delegate void LoadTransferSettingsDelegate();
        public void LoadTransferSettings()
        {
            if (InvokeRequired)
            {
                Invoke(new LoadTransferSettingsDelegate(LoadTransferSettings));
                return;
            }
            settingPage.LoadSettings();
        }
        
        delegate void UpdateMachineStateDelegate(bool isRun);
        public void UpdateMachineState(bool isRun)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateMachineStateDelegate(UpdateMachineState), isRun);
                return;
            }

            if (isRun == true)
            {
                labelMachineState.Text = "Run";
                labelMachineState.ForeColor = Color.Black;
                labelMachineState.BackColor = Color.LightGreen;
            }
            else
            {
                labelMachineState.Text = "Stop";
                labelMachineState.ForeColor = Color.White;
                labelMachineState.BackColor = Color.Red;
            }
        }

        public void Teach()
        {
        }

        public void Scan()
        {

        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        public void PageChange(IMainTabPage page, UserType userType = UserType.Maintrance)
        {
            throw new NotImplementedException();
        }

        public void OnModelChanged()
        {
            throw new NotImplementedException();
        }

        public void OnLotChanged()
        {
            throw new NotImplementedException();
        }

        public void WorkerChanged(string OpName)
        {
            throw new NotImplementedException();
        }
    }
}
