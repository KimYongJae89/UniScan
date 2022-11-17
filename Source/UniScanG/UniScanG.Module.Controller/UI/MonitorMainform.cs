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
using UniEye.Base.Device;
using UniEye.Base.Data;
using System.Drawing;
using System.Threading;
using DynMvp.Authentication;
using Infragistics.Win.UltraWinTabControl;
using UniEye.Base.UI;
using UniScanG.Common;
using UniScanG.Common.Util;
using UniScanG.UI.Etc;
using UniScanG.Common.Data;
using System.Collections.Generic;
using System.IO;
using UniScanG.Common.Settings;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using System.Linq;
using UniScanG.MachineIF;
using UniScanG.Gravure.Device;

namespace UniScanG.Module.Controller.UI
{
    public partial class MonitorMainform : Form, IMainForm, IMultiLanguageSupport
    {
        List<IStatusStripPanel> statusPanelList = new List<IStatusStripPanel>();
        AlarmMessageForm alarmMessageForm = null;
        int showRunningModeCount = 0;
        string titleText;

        public MonitorMainform(IUiControlPanel mainTabPanel, string title)
        {
            InitializeComponent();
            this.titleText = title;
            
            StringManager.AddListener(this);

            if (mainTabPanel != null)
            {
                this.mainPanel.Controls.Add((Control)mainTabPanel);
                //SystemManager.Instance().UiController.ChangeTab(MainTabKey.Model.ToString());
            }
            panelTopUser.Controls.Add(new UserPanel());
            panelTopMode.Controls.Add(new ModelPanel());
            InitStatusStrip();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
//#if !DEBUG
//            this.FormBorderStyle = FormBorderStyle.None;
//#endif
            alarmMessageForm = new AlarmMessageForm();
            alarmMessageForm?.Show();

            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens.FirstOrDefault(f => !f.Primary);
            if (screen == null)
                screen = System.Windows.Forms.Screen.PrimaryScreen;

            this.StartPosition = FormStartPosition.Manual;
            this.MinimumSize = screen.WorkingArea.Size;
            this.Location = screen.WorkingArea.Location;
            this.WindowState = FormWindowState.Maximized;

            this.Text = SystemManager.Instance().UiChanger.GetMainformTitle();

            timer.Start();

            ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Initialize.Information, ErrorLevel.Info, "Initializing System.", null, ""));

            if (LicenseManager.Exist(LicenseManager.ELicenses.EnvArgs))
            {
                if (MessageForm.Show(this, StringManager.GetString("Do you want Initialize IM Devices?"), MessageFormType.YesNo) == DialogResult.Yes)
                {
                    List<InspectorObj> list = ((IServerExchangeOperator)SystemManager.Instance().ExchangeOperator).GetInspectorList();
                    list.ForEach(f =>
                    {
                    ((UniScanG.Module.Controller.Device.DeviceController)SystemManager.Instance().DeviceController).InitializeInspectModule(this, f.Info.GetName());
                    });
                }
            }
        }

        public void InitStatusStrip()
        {
            statusPanelList.Clear();

            List<IStatusStripPanel> statusStripList = SystemManager.Instance().UiChanger.GetStatusStrip();
            statusPanelList.AddRange(statusStripList);

            stripPanelRight.Controls.AddRange(statusStripList.ConvertAll(f => (Control)f).FindAll(f => f.Dock == DockStyle.Right).ToArray());
            stripPanelLeft.Controls.AddRange(statusStripList.ConvertAll(f => (Control)f).FindAll(f => f.Dock == DockStyle.Left).ToArray());
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            DateTime curTime = DateTime.Now;
            dateLabel.Text = curTime.ToString("yyyy - MM - dd");
            timeLabel.Text = curTime.ToString("HH : mm : ss");

            statusPanelList.ForEach(s => s.StateUpdate());

            this.showRunningModeCount = (this.showRunningModeCount + timer.Interval) % 1000;
            this.panelTopLogo.Invalidate();
        }

        private void userNameLabel_Click(object sender, EventArgs e)
        {
            LogInForm loginForm = new LogInForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
                UserHandler.Instance().CurrentUser = loginForm.LogInUser;
        }

        private void MainForm_VisibleChanged(object sender, EventArgs e) { }

        public IInspectionPage MonitoringPage { get { return null; } }

        public IReportPage ReportPage { get { throw new NotImplementedException(); } }

        public ISettingPage SettingPage { get { throw new NotImplementedException(); } }

        public IInspectionPage InspectPage => throw new NotImplementedException();

        IModelManagerPage IMainForm.ModelManagerPage => throw new NotImplementedException();

        ITeachPage IMainForm.TeachPage => throw new NotImplementedException();

        public void EnableTabs() { }
        public void Teach() { }
        public void Scan() { }
        public void UpdateControl(string item, object value) { }
        public void ModifyTeaching(string imagePath) { }

        public void UpdateLanguage()
        {
            this.labelTitle.Text = "";

            StringManager.UpdateString(this);
            //this.labelTitle.Text = StringManager.GetString(this.GetType().FullName, this.titleText);
            this.labelTitle.Text = this.titleText;
        }

        private void gtcLogoPanel_Click(object sender, EventArgs e)
        {
            if (!UserHandler.Instance().CurrentUser.IsSuperAccount)
                return;

            string[] localeList = new string[] { "ko-kr", "zh-cn" };
            int curLocaleIdx = string.IsNullOrEmpty(StringManager.LocaleCode) ? -1 : Array.FindIndex(localeList, f => f == StringManager.LocaleCode);
            int nextLocaleIdx = (curLocaleIdx + 1) % localeList.Length;
            StringManager.ChangeLanguage(localeList[nextLocaleIdx]);
        }

        public void PageChange(IMainTabPage page)
        {
            throw new NotImplementedException();
        }

        public void OnModelChanged()
        {

        }

        public void OnLotChanged()
        {

        }

        public void WorkerChanged(string OpName)
        {
            throw new NotImplementedException();
        }

        public void PageChange(IMainTabPage page, UserType userType = UserType.Maintrance) { }

        bool drawRunningMode = false;
        private void panelTopLogo_Paint(object sender, PaintEventArgs e)
        {
            bool toggle = (MachineSettings.Instance().RunningMode != RunningMode.Real && this.showRunningModeCount == 0);
            if (toggle)
                this.drawRunningMode = !this.drawRunningMode;

            if (this.drawRunningMode)
                e.Graphics.DrawString(MachineSettings.Instance().RunningMode.ToString(), new Font("Arial", 25, FontStyle.Bold), Brushes.Red, PointF.Empty);
        }

        private void MonitorMainform_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            LogHelper.Debug(LoggerType.Operation, string.Format("MonitorMainform::MonitorMainform_FormClosing - CloseReason: {0}", e.CloseReason));

            SystemManager.Instance().ExitWaitInspection();
            ErrorManager.Instance().Report(new AlarmException(ErrorSectionSystem.Instance.Initialize.Information, ErrorLevel.Info, "Terminating System. ({0})", new string[] { e.CloseReason.ToString() }, ""));
            ErrorManager.Instance().WaitReportDone();
        }

        private void MonitorMainform_SizeChanged(object sender, EventArgs e)
        {
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            Common.Exchange.ExchangeCommand visitCommand = this.WindowState == FormWindowState.Minimized ? Common.Exchange.ExchangeCommand.V_HIDE : Common.Exchange.ExchangeCommand.V_SHOW;
            server.SendCommand(visitCommand);
        }
    }
}
