using System;
using System.Windows.Forms;
using System.Threading;
using System.IO;

using Infragistics.Win.UltraWinTabControl;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.UI.Touch;
using System.Drawing;
using UniEye.Base.Settings;
using DynMvp.Device.UI;
using UniEye.Base.Data;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using DynMvp.Devices.Dio;
using DynMvp.Device.Device;
using DynMvp.Authentication;

namespace UniEye.Base.UI
{
    public partial class DefaultMainForm : Form, LoggingTarget, IMainForm
    {
        private ModellerPage modellerPage;
        private InspectionPage inspectionPage;
        private ModelTileControl modelTileControl;
        private ReportPage reportPage;
        private ISettingPage settingPage;
        private LivePage livePage;

        AlarmMessageForm alarmMessageForm;

        public IInspectionPage MonitoringPage
        {
            get { return inspectionPage; }
        }
        public ITeachPage ModellerPage
        {
            get { return modellerPage; }
        }

        public IReportPage ReportPage
        {
            get { return reportPage; }
        }

        public ISettingPage SettingPage
        {
            get { return settingPage; }
        }

        public IInspectionPage InspectPage => throw new NotImplementedException();

        public ISettingPage ModelManagerPage => throw new NotImplementedException();

        public ISettingPage TeachPage => throw new NotImplementedException();

        IModelManagerPage IMainForm.ModelManagerPage => throw new NotImplementedException();

        ITeachPage IMainForm.TeachPage => throw new NotImplementedException();

        public DefaultMainForm()
        {
            LogHelper.Debug(LoggerType.StartUp, "Init MainForm");

            InitializeComponent();

            LogHelper.LoggingTarget = this;

            modelTileControl = new ModelTileControl();
            modellerPage = new ModellerPage();
            inspectionPage = new InspectionPage();
            reportPage = new ReportPage();
            livePage = new LivePage();

            alarmMessageForm = new AlarmMessageForm();

            this.panelModelList.Controls.Add(this.modelTileControl);
            this.tabPageInspection.Controls.Add(this.inspectionPage);
            this.tabPageModeller.Controls.Add(this.modellerPage);
            this.tabPageReport.Controls.Add(this.reportPage);
            this.tabPageLive.Controls.Add(this.livePage);

            LogHelper.Debug(LoggerType.StartUp, "Init Modeller Page");

            this.modellerPage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.modellerPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modellerPage.Location = new System.Drawing.Point(0, 313);
            this.modellerPage.Name = "modellerPage";
            this.modellerPage.Size = new System.Drawing.Size(466, 359);
            this.modellerPage.TabIndex = 0;
            this.modellerPage.Initialize();

            LogHelper.Debug(LoggerType.StartUp, "Init Inspection Page");

            this.inspectionPage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.inspectionPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.inspectionPage.Location = new System.Drawing.Point(0, 313);
            this.inspectionPage.Name = "inspectionPage";
            this.inspectionPage.Size = new System.Drawing.Size(466, 359);
            this.inspectionPage.TabIndex = 0;
            this.inspectionPage.Initialize();

            LogHelper.Debug(LoggerType.StartUp, "Init Live Page");

            this.livePage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.livePage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.livePage.Location = new System.Drawing.Point(0, 313);
            this.livePage.Name = "livePage";
            this.livePage.Size = new System.Drawing.Size(466, 359);
            this.livePage.TabIndex = 0;
            this.livePage.Initialize();

            LogHelper.Debug(LoggerType.StartUp, "Init Report Page");

            this.reportPage.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.reportPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportPage.Location = new System.Drawing.Point(0, 313);
            this.reportPage.Name = "reportPage";
            this.reportPage.Size = new System.Drawing.Size(466, 359);
            this.reportPage.TabIndex = 0;
            this.reportPage.Initialize();

            LogHelper.Debug(LoggerType.StartUp, "Init Home Page");

            this.modelTileControl.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.modelTileControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelTileControl.Location = new System.Drawing.Point(0, 313);
            this.modelTileControl.Name = "modelTileControl";
            this.modelTileControl.Size = new System.Drawing.Size(466, 359);
            this.modelTileControl.TabIndex = 0;
            this.modelTileControl.ModelSelected += modelTileControl_ModelSelected;
            this.modelTileControl.CloseModel += modelTileControl_CloseModel;

            LogHelper.Debug(LoggerType.StartUp, "Init Setting Page");

            settingPage = (ISettingPage)SystemManager.Instance().UiChanger.CreateSettingPage();

            this.tabPageSetting.Controls.Add((UserControl)settingPage);

            LogHelper.Debug(LoggerType.StartUp, "Enable Tabs");

            tabControlMain.Tabs["Live"].Visible = CustomizeSettings.Instance().UseLiveCam;
            tabControlMain.Tabs["Reports"].Visible = CustomizeSettings.Instance().UseReportPage;

            EnableTabs();

            LogHelper.Debug(LoggerType.StartUp, "Update Tab Text");

            foreach (Infragistics.Win.UltraWinTabControl.UltraTab tab in tabControlMain.Tabs)
            {
                tab.Text = StringManager.GetString(this.GetType().FullName,tab.Text);
            }

            LogHelper.Debug(LoggerType.StartUp, "Show Company Logo");

            string companyLogoPath = PathSettings.Instance().CompanyLogo;
            if (File.Exists(companyLogoPath) == true)
                this.companyLogo.Image = new Bitmap(companyLogoPath);

            LogHelper.Debug(LoggerType.StartUp, "Show Product Logo");

            string productLogoPath = PathSettings.Instance().ProductLogo;
            if (File.Exists(productLogoPath) == true)
                this.productLogo.Image = new Bitmap(productLogoPath);

            LogHelper.Debug(LoggerType.StartUp, "Update Program Title");

            Text = CustomizeSettings.Instance().ProgramTitle;

            if (MachineSettings.Instance().MachineIfSetting != null &&  MachineSettings.Instance().MachineIfSetting.MachineIfType == MachineInterface.MachineIfType.None)
            {
                statusLabelVisionReady.Visible = false;
                statusLabelTrigger1.Visible = false;
                statusLabelTrigger2.Visible = false;
                statusLabelInspection.Visible = false;
                statusLabelGrabDone.Visible = false;
                statusLabelVisionComplete.Visible = false;
                statusLabelMachineDone.Visible = false;
            }

            LogHelper.Debug(LoggerType.StartUp, "End MainForm::Ctor");
        }

        private void modelTileControl_CloseModel()
        {
            SystemManager.Instance().CloseModel();

            title.Text = CustomizeSettings.Instance().Title;

            inspectionPage.UpdatePage();

            EnableTabs();

            //tabControlMain.Tabs["Inspection"].Enabled = false;
            //tabControlMain.Tabs["Model"].Enabled = false;
        }

        //커서 마우스 이벤트 

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

        public int CurrentTab()
        {
            return tabControlMain.ActiveTab.Index;
        }

        private bool IsAdminUser()
        {
            bool adminUser = true;
            if (OperationSettings.Instance().UseUserManager)
                adminUser = UserHandler.Instance().CurrentUser.Id != "op";

            return adminUser;
        }

        public void EnableTabs()
        {
            bool inspectionStarted = SystemState.Instance().IsInspection;
            bool modelLoaded = (SystemManager.Instance().CurrentModel != null);
            bool livemode = SystemManager.Instance().LiveMode;

            tabControlMain.Tabs["Home"].Enabled = !inspectionStarted && !livemode;
            tabControlMain.Tabs["Inspection"].Enabled = modelLoaded && !livemode;
            tabControlMain.Tabs["Live"].Enabled = !inspectionStarted;
            tabControlMain.Tabs["Model"].Enabled = modelLoaded && !inspectionStarted && !livemode && IsAdminUser();
            tabControlMain.Tabs["Reports"].Enabled = !inspectionStarted && !livemode;
        }

        //public void ChangeToInspectionTab()
        //{
        //    tabControlMain.SelectedTab = tabControlMain.Tabs["Inspection"];
        //}

        private void modelTileControl_ModelSelected(ModelDescription modelDescription)
        {
            if (SystemManager.Instance().CurrentModel != null)
            {
                DialogResult result = MessageForm.Show(this, String.Format("Do you want to close the current model[{0}]", SystemManager.Instance().CurrentModel.Name), MessageFormType.YesNo);
                if (result == DialogResult.No)
                    return;
            }

            SystemManager.Instance().LoadModel(modelDescription);

            title.Text = CustomizeSettings.Instance().Title + " - " + modelDescription.Name;

            inspectionPage.UpdatePage();

            EnableTabs();

            if (SystemManager.Instance().CurrentModel.IsEmpty() == true)
            {
                if (IsAdminUser())
                {
                    tabControlMain.SelectedTab = tabControlMain.Tabs["Model"];
                }
                else
                {
                    MessageBox.Show("There is no teaching data. Please, make the teaching data by Administrator.");
                }
            }
            else
            {
                tabControlMain.SelectedTab = tabControlMain.Tabs["Inspection"];
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LogHelper.Debug(LoggerType.StartUp, "Start MainForm_Load");

            title.Text = CustomizeSettings.Instance().Title;

            settingPage.Initialize();

            //TriggerSource triggerSource = SystemManager.Instance().MachineIf.GetTriggerSource();
            //if (triggerSource != null)
            //{
            //    triggerSource.StateChanged += triggerSource_StateChanged;
            //}

            tabControlMain.SelectedTab = tabControlMain.Tabs["Home"];
            //ReleaseIo();

            alarmMessageForm.Show();
            ErrorManager.Instance().OnResetAlarmState += errorManager_ResetAlarmStatus;

            statusLabelLicense.Text = OperationSettings.Instance().ImagingLibrary.ToString().Substring(0, 1) + AlgorithmBuilder.LicenseErrorCount.ToString();

            if (SystemManager.Instance().DeviceController.RobotStage != null)
            {
                positionUpdateTimer.Start();
                RobotOrigin();
            }
            else
            {
                statusLabelPosition.Visible = false;
            }

            LogHelper.Debug(LoggerType.StartUp, "End MainForm_Load");
        }

        private void errorManager_ResetAlarmStatus()
        {
            //SystemManager.Instance().DeviceController.EmergencyButton.ResetState();
            IoButtonEventHandler ioButtonEventHandler = (IoButtonEventHandler)SystemManager.Instance().DeviceController.GetIoEventHandler("Emergency");
            ioButtonEventHandler?.ResetState();
        }

        public void RobotOrigin()
        {
            AxisHandler robotStage = SystemManager.Instance().DeviceController.RobotStage;
            if (robotStage == null)
                return;

            LogHelper.Debug(LoggerType.StartUp, "Start RobotOrigin");

            string message = StringManager.GetString(this.GetType().FullName, "Do you want the robot move to origin?");

            if (MessageForm.Show(this.ParentForm, message, MessageFormType.YesNo) == DialogResult.Yes)
            //if (MessageBox.Show(message, caption, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (SystemManager.Instance().DeviceBox.MachineIf != null)
                {
                    while (true)
                    {
                        // 장비 내 물체가 존재하면 원점 이동 안 함(요청사항)
                        //string exist = SystemManager.Instance().MachineIf.GetMachineState("Exist");
                        //if (exist == null || exist == "0")
                        //{
                        //    break;
                        //}

                        string message2 = StringManager.GetString(this.GetType().FullName, "Keep clear in the machine.");
                        if (MessageForm.Show(this.ParentForm, message2, MessageFormType.RetryCancel) == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                }

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                SimpleProgressForm loadingForm = new SimpleProgressForm("Move to origin");
                loadingForm.Show(new Action( () => { 
                 robotStage.HomeMove(cancellationTokenSource);
                }), cancellationTokenSource);
            }
        }

        //private delegate void triggerSource_StateChangedDelegate();
        //private void triggerSource_StateChanged()
        //{
        //    if (InvokeRequired)
        //    {
        //        /* If called from a different thread, we must use the Invoke method to marshal the call to the proper thread. */
        //        BeginInvoke(new triggerSource_StateChangedDelegate(triggerSource_StateChanged));
        //        return;
        //    }
        //    TriggerSource triggerSource = SystemManager.Instance().MachineIf.GetTriggerSource();
        //    if (triggerSource == null)
        //        return;

        //    VisionState visionState = triggerSource.VisionState;
        //    switch (visionState)
        //    {
        //        case VisionState.VisionReady:
        //            statusLabelVisionReady.BackColor = Color.LimeGreen;
        //            statusLabelVisionComplete.BackColor = Color.White;
        //            statusLabelInspection.BackColor = Color.White;
        //            break;
        //        case VisionState.Inspect:
        //            statusLabelVisionReady.BackColor = Color.White;
        //            statusLabelVisionComplete.BackColor = Color.White;
        //            statusLabelInspection.BackColor = Color.LimeGreen;
        //            break;

        //        case VisionState.Complete:
        //            statusLabelVisionReady.BackColor = Color.LimeGreen;
        //            statusLabelVisionComplete.BackColor = Color.White;
        //            statusLabelInspection.BackColor = Color.White;
        //            break;
        //        default:
        //            statusLabelVisionReady.BackColor = Color.White;
        //            statusLabelVisionComplete.BackColor = Color.White;
        //            statusLabelInspection.BackColor = Color.White;
        //            break;
        //    }

        //    MachineState machineState = triggerSource.MachineState;

        //    switch (machineState)
        //    {
        //        case MachineState.Trigger0:
        //            statusLabelTrigger1.BackColor = Color.LimeGreen;
        //            statusLabelTrigger2.BackColor = Color.White;
        //            statusLabelMachineDone.BackColor = Color.White;
        //            break;
        //        case MachineState.Trigger1:
        //            statusLabelTrigger1.BackColor = Color.White;
        //            statusLabelTrigger2.BackColor = Color.LimeGreen;
        //            statusLabelMachineDone.BackColor = Color.White;
        //            break;
        //        case MachineState.MachineDone:
        //            statusLabelTrigger1.BackColor = Color.White;
        //            statusLabelTrigger2.BackColor = Color.White;
        //            statusLabelMachineDone.BackColor = Color.LimeGreen;
        //            break;
        //        default:
        //            statusLabelTrigger1.BackColor = Color.White;
        //            statusLabelTrigger2.BackColor = Color.White;
        //            statusLabelMachineDone.BackColor = Color.White;
        //            break;

        //    }
        //}

        private void tabControlMain_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "Start tabControlMain_SelectedTabChanged.");

            if (e.PreviousSelectedTab != null)
            {
                Infragistics.Win.UltraWinTabControl.UltraTabPageControl preTabControl = (Infragistics.Win.UltraWinTabControl.UltraTabPageControl)tabControlMain.Controls[e.PreviousSelectedTab.Index];

                if (preTabControl.Controls.Count > 0 && preTabControl.Controls[0] is IMainTabPage)
                {
                    IMainTabPage preTabPage = (IMainTabPage)preTabControl.Controls[0];
                    preTabPage.PageVisibleChanged(true);
                }
            }

            Infragistics.Win.UltraWinTabControl.UltraTabPageControl curTabControl = (Infragistics.Win.UltraWinTabControl.UltraTabPageControl)tabControlMain.Controls[e.Tab.Index];

            if (curTabControl.Controls.Count > 0 && curTabControl.Controls[0] is IMainTabPage)
            {
                IMainTabPage curTabPage = (IMainTabPage)curTabControl.Controls[0];
                curTabPage.PageVisibleChanged(true);
            }

            if (e.PreviousSelectedTab != null)
            {
                if (SystemManager.Instance().CurrentModel != null && SystemManager.Instance().CurrentModel.IsEmpty() == false)
                {
                    //if (e.Tab.Key != "Inspection")
                    //{
                    //    SystemManager.Instance().OnInspectionPage = false;
                    //}
                    //if (e.Tab.Key == "Model")
                    //{
                    //    SystemManager.Instance().OnInspectionPage = false;
                    //    //if (OperationSettings.Instance().SystemType == SystemType.DrugPackaging)
                    //    //{
                    //    //    this.Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                    //    //    modellerPage.TeachBox.ZoomFit();
                    //    //}
                    //    //if (OperationSettings.Instance().SystemType == SystemType.MaskInspector)
                    //    //{
                    //    //    modellerPage.TeachBox.ZoomFit();
                    //    //    modellerPage.PrepareProcess();
                    //    //}

                    //}
                    //if (e.Tab.Key == "Inspection")
                    //{
                    //    SystemManager.Instance().OnInspectionPage = true;
                    //    if (OperationSettings.Instance().SystemType == SystemType.DrugPackaging)
                    //    {
                    //        this.Size = new Size(640, 1024);
                    //    }

                    //    if (OperationSettings.Instance().ImagingLibrary == DynMvp.Vision.ImagingLibrary.CognexVisionPro) // 알고리즘 타입이 비전 프로일시, 사전 검사를 시행 한다.
                    //        SystemManager.Instance().InspectPreparation();
                    //}
                }
            }

            LogHelper.Debug(LoggerType.Operation, "End tabControlMain_SelectedTabChanged.");
        }

        delegate void OnPreInspectionDelegate();
        public void OnPreInspection()
        {
            if (InvokeRequired)
            {
                Invoke(new OnPreInspectionDelegate(OnPreInspection));
                return;
            }

            inspectionPage.OnPreInspection();
        }

        public void ProductInspected(InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Inspection, "Begin InspectionPage - ProductInspected");

            inspectionPage.ProductInspected(inspectionResult);
        }

        delegate void OnPostInspectionDelegate();
        public void OnPostInspection()
        {
            if (InvokeRequired)
            {
                Invoke(new OnPostInspectionDelegate(OnPostInspection));
                return;
            }

            LogHelper.Debug(LoggerType.Inspection, "Begin InspectionPage - PostInspection");

            inspectionPage.OnPostInspection();
        }

        public void TargetInspected(Target target, InspectionResult targetInspectionResult)
        {
            inspectionPage.TargetInspected(target, targetInspectionResult);
        }

        public void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult)
        {
            inspectionPage.TargetGroupInspected(targetGroup, inspectionResult, objectInspectionResult);
        }

        public void InspectionStepInspected(InspectionStep inspectionStep, int groupId, InspectionResult inspectionResult)
        {
            inspectionPage.InspectionStepInspected(inspectionStep, groupId, inspectionResult);
        }

        public void UpdateInspectionNo(string inspectionNo)
        {
            inspectionPage.UpdateInspectionNo(inspectionNo);
        }

        //public void InspectionStarted()
        //{
        //    inspectionPage.UpdateButtons();
        //    inspectionPage.ChangeInspectionStatus(InspectionState.Waitting);
        //}

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !CheckFormCloseing();
        }

        private bool CheckFormCloseing()
        {
            if (SystemState.Instance().IsInspection)
            {
                MessageForm.Show(null, "Please, Stop the inspection.");
                return false;
            }

            if (SystemManager.Instance().CurrentModel != null)
                SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);

            if (MessageForm.Show(this, "Do you want to exit program?", MessageFormType.YesNo) == DialogResult.No)
            {
                return false;
            }
            return true;
        }

        public void ReleaseIo()
        {
            if (SystemManager.Instance().DeviceBox.DigitalIoHandler != null)
            {
                try
                {
                    PortList portList = SystemManager.Instance().DeviceBox.PortMap.OutPortList;
                    foreach (IoPort port in portList)
                    {
                        SystemManager.Instance().DeviceBox.DigitalIoHandler.WriteOutput(port, false);
                    }
                }
                catch (InvalidCastException)
                {
                    return;
                }
            }

            if (SystemManager.Instance().DeviceBox.LightCtrlHandler != null)
                SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();

            SystemManager.Instance().DeviceController?.Stop();
        }

        public void ModifyTeaching(string imageFileName)
        {
            tabControlMain.SelectedTab = tabControlMain.Tabs["Model"];
            //    modellerPage.ModifyTeaching(imageFileName);
        }

        public void ChangeInspectionSize()
        {
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (tabControlMain.SelectedTab.Key == "Model")
            {
                modellerPage.ProcessKeyDown(e);
            }
        }

        private void tabControlMain_SelectedTabChanging(object sender, SelectedTabChangingEventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, "Start tabControlMain_SelectedTabChanging.");

            if (e.Tab.Key == "Exit")
            {
                e.Cancel = true;
                this.Close();
                return;
            }

            if (e.Tab.Key == "Setting")
            {
                e.Cancel = true;
                DynMvp.UI.LogInForm loginForm = new DynMvp.UI.LogInForm();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    if (loginForm.LogInUser.Id == "master" || loginForm.LogInUser.Id == "developer")
                        e.Cancel = false;
                }
            }
        }

        private void positionUpdateTimer_Tick(object sender, EventArgs e)
        {
            AxisPosition axisPosition = SystemManager.Instance().DeviceController.RobotStage.GetActualPos();
            statusLabelPosition.Text = axisPosition.ToString();
        }

        public void Load2dImage(int cameraIndex, int stepIndex, int lightTypeIndex)
        {
            inspectionPage.Load2dImage(cameraIndex, stepIndex, lightTypeIndex);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ReleaseIo();
            Application.ExitThread();
//            Environment.Exit(0);
        }

        public void StopInspection()
        {
            throw new NotImplementedException();
        }

        public void Teach()
        {
            modellerPage.Teach();
        }

        public void Scan()
        {
            modellerPage.Scan();
        }

        public void TeachInspect()
        {
            modellerPage.Inspect();
        }

        public void ChangeMode(string modeStr)
        {
            throw new NotImplementedException();
        }

        public void Grab()
        {
            throw new NotImplementedException();
        }

        public void StopGrab()
        {
            throw new NotImplementedException();
        }

        public void UpdateControl(string item, string state)
        {
            
        }

        public void UpdateStatusPanel(int pos, string text)
        {
            throw new NotImplementedException();
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
