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
using UniScanS.Common;
using UniScanS.Common.Util;
using UniScanS.UI.Etc;
using UniScanS.Common.Data;
using System.Collections.Generic;
using UniScanS.Common.Settings;

namespace UniScanS.UI
{
    public enum MainTabKey
    {
        Inspect, Model, Teach, Report, Log, Exit
    }

    public partial class MonitorMainform : Form, IMainForm, IMultiLanguageSupport
    {
        List<StatusStripPanel> statusPanelList = new List<StatusStripPanel>();
        AlarmMessageForm alarmMessageForm = new AlarmMessageForm();

        public MonitorMainform(IUiControlPanel mainTabPanel)
        {
            InitializeComponent();
            StringManager.AddListener(this);
            //UpdateLanguage();

            if (mainTabPanel != null)
                this.mainPanel.Controls.Add((Control)mainTabPanel);

            userPanel.Controls.Add(new UserPanel());
            modelPanel.Controls.Add(new ModelPanel());
            InitStatusStrip();
            timer.Start();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            alarmMessageForm.Show();

            this.MaximizedBounds = System.Windows.Forms.Screen.FromHandle(this.Handle).WorkingArea;
            this.WindowState = FormWindowState.Maximized;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
        
        public void InitStatusStrip()
        {
            InitScreenStatus();
        }

        private void InitScreenStatus()
        {
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> list = server.GetInspectorList();

            for (int i = list.Count - 1; i >= 0; i--)
            {
                StatusStripPanel statusStripPanel = new StatusStripPanel(list[i]);
                layoutStatusStrip.Controls.Add(statusStripPanel);
                statusPanelList.Add(statusStripPanel);
            }

            IoPort ioRollingPort = SystemManager.Instance().DeviceBox.PortMap.GetInPort(UniScanS.Screen.Device.PortMap.IoPortName.InMachineRolling);
            IoPort ioRunPort = SystemManager.Instance().DeviceBox.PortMap.GetInPort(UniScanS.Screen.Device.PortMap.IoPortName.InMachineRun);
            IoPort ioStopPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(UniScanS.Screen.Device.PortMap.IoPortName.OutStop);
            IoPort ioAlarmPort = SystemManager.Instance().DeviceBox.PortMap.GetOutPort(UniScanS.Screen.Device.PortMap.IoPortName.OutAlarm);
            MachineStatusStripPanel machineStatusStripPanel = new MachineStatusStripPanel(ioRunPort, ioRollingPort, ioStopPort, ioAlarmPort);
            layoutStatusStrip.Controls.Add(machineStatusStripPanel);
        }

        
        private void timer_Tick(object sender, EventArgs e)
        {
            DateTime curTime = DateTime.Now;
            dateLabel.Text = curTime.ToString("yyyy - MM - dd");
            timeLabel.Text = curTime.ToString("HH : mm : ss");

            statusPanelList.ForEach(s => s.StateUpdate());
        }

        private void userNameLabel_Click(object sender, EventArgs e)
        {
            LogInForm loginForm = new LogInForm();
            if (loginForm.ShowDialog() == DialogResult.OK)
                UserHandler.Instance().CurrentUser = loginForm.LogInUser;
        }
        
        private void MainForm_VisibleChanged(object sender, EventArgs e)
        {

        }

        public IInspectionPage InspectionPage { get { return null; } }

        public IReportPage ReportPage { get { throw new NotImplementedException(); } }

        public ISettingPage SettingPage { get { throw new NotImplementedException(); } }

        public IModellerPage ModellerPage { get { throw new NotImplementedException(); } }

        public IInspectionPage InspectPage => throw new NotImplementedException();

        public ITeachPage TeachPage => throw new NotImplementedException();

        public IModelManagerPage ModelManagerPage => throw new NotImplementedException();

        public void EnableTabs() { }
        public void Teach() { }
        public void Scan() { }
        public void UpdateControl(string item, object value) { }
        public void ModifyTeaching(string imagePath) { }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            // 뭐 없네
        }

        public void PageChange(IMainTabPage page, UserType userType = UserType.Maintrance)
        {
            throw new NotImplementedException();
        }

        public void OnModelChanged()
        {
            throw new NotImplementedException();
        }

        public void WorkerChanged(string opName)
        {
            throw new NotImplementedException();
        }
    }
}
