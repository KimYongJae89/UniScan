using System;
using System.Windows.Forms;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.UI.Touch;
using UniEye.Base.Device;
using UniEye.Base.Data;
using System.Drawing;
using System.Threading;
using DynMvp.Authentication;
using UniEye.Base.UI;
using UniScanG;
using UniScanG.Common;
using UniScanG.Common.Exchange;
using UniEye.Base.Settings;
using System.Linq;
using System.Text;
using UniScanG.UI.Etc;

namespace UniScanG.Module.Inspector.UI
{
    public partial class InspectorMainForm : Form, IVisitListener, IMultiLanguageSupport
    {
        ContextMenu masterContextMenu = new ContextMenu();
        Label labelModeIndicator;

        IMainTabPage curSelectedPage = null;

        IMainTabPage inspectPanel;
        IMainTabPage modelPanel;
        IMainTabPage teachPanel;
        IMainTabPage logPanel;
        ISettingPage settingPanel;

        int showRunningModeCount = 0;

        public InspectorMainForm()
        {
            InitializeComponent();
            InitContextMenu();
            StringManager.AddListener(this);

            //System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens.FirstOrDefault(f => !f.Primary);
            //if (screen == null)
            //    screen = System.Windows.Forms.Screen.PrimaryScreen;

            //this.StartPosition = FormStartPosition.Manual;
            //this.Location = screen.WorkingArea.Location;
            //this.WindowState = FormWindowState.Maximized;
            //this.MinimumSize = new Size(1024, 768);
            //this.virtualIndicator.Size = new Size(1000, 100);

            InitPanels();

            IClientExchangeOperator clientExchangeOperator = (IClientExchangeOperator)SystemManager.Instance().ExchangeOperator;
            clientExchangeOperator.AddVisitListener(this);
        }

        private void InitPanels()
        {
            InspectorUiChangerG inspectorUiChanger = (InspectorUiChangerG)SystemManager.Instance().UiChanger;

            this.inspectPanel = inspectorUiChanger.CreateInspectPage();
            this.modelPanel = inspectorUiChanger.CreateModelPage();
            this.teachPanel = inspectorUiChanger.CreateTeachPage();
            this.logPanel = inspectorUiChanger.CreateLogPage();
            this.settingPanel = inspectorUiChanger.CreateSettingPage();

            this.labelModeIndicator.AutoSize = true;
            this.labelModeIndicator.Font = new System.Drawing.Font("Malgun Gothic", 25F, System.Drawing.FontStyle.Bold);
            this.labelModeIndicator.ForeColor = System.Drawing.Color.Red;
            this.labelModeIndicator.Location = new System.Drawing.Point(0, 0);
            this.labelModeIndicator.Text = MachineSettings.Instance().RunningMode.ToString();
            this.labelModeIndicator.Visible = false;
        }

        private void InitContextMenu()
        {
            //MenuItem versiontMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Version"), Version_Clicked);
            //MenuItem inspectMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Inspect"), Inspect_Clicked);
            //MenuItem modelMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Model"), Model_Clicked);
            //MenuItem teachMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Teach"), Teach_Clicked);
            //MenuItem reportMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Report"), Report_Clicked);
            //MenuItem settingMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Setting"), Setting_Clicked);
            //MenuItem closeMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Close"), Close_Clicked);
            //MenuItem exitMenuItem1 = new MenuItem(StringManager.GetString(this.GetType().FullName, "Exit"), Exit_Clicked);

            MenuItem[] menuItems = new MenuItem[]
            {
                new MenuItem("Version", Info_Clicked){ Name = "menuItemInfo"},
                new MenuItem("Console", Console_Clicked) {Name = "menuItemConsole", Visible=false},
                new MenuItem("Language", Language_Clicked) {Name = "menuItemLanguage"},
                new MenuItem("-"),
                new MenuItem("Inspect", Inspect_Clicked) {Name = "menuItemInspect"},
                new MenuItem("Model", Model_Clicked) {Name = "menuItemModel"},
                new MenuItem("Teach", Teach_Clicked) {Name = "menuItemTeach"},
                new MenuItem("Log", Log_Clicked) {Name = "menuItemLog"},
                new MenuItem("Setting", Setting_Clicked) {Name = "menuItemSetting"},
                new MenuItem("-"),
                new MenuItem("Hide", Close_Clicked) {Name = "menuItemClose"},
                new MenuItem("Exit", Exit_Clicked) {Name = "menuItemExit"}
            };

            masterContextMenu.MenuItems.AddRange(menuItems);
            notifyIcon.ContextMenu = masterContextMenu;
        }

        private void Language_Clicked(object sender, EventArgs e)
        {
            if (!UserHandler.Instance().CurrentUser.IsSuperAccount)
                return;

            string[] localeList = new string[] { "ko-kr", "zh-cn" };
            int curLocaleIdx = string.IsNullOrEmpty(StringManager.LocaleCode) ? -1 : Array.FindIndex(localeList, f => f == StringManager.LocaleCode);
            int nextLocaleIdx = (curLocaleIdx + 1) % localeList.Length;
            StringManager.ChangeLanguage(localeList[nextLocaleIdx]);
        }
        
        private void Info_Clicked(object sender, EventArgs e)
        {
            int camIdx = SystemManager.Instance().ExchangeOperator.GetCamIndex();
            int clientIdx = SystemManager.Instance().ExchangeOperator.GetClientIndex();
            string server = string.Format("{0}:{1}", AddressManager.Instance().GetMonitorAddress(), AddressManager.Instance().GetMonitorLinteningPort());
            string client = string.Format("{0}", AddressManager.Instance().GetInspectorAddress(camIdx, clientIdx));

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Version {VersionHelper.Instance().VersionString}, Build {VersionHelper.Instance().BuildString}, {string.Join(" ", Environment.GetCommandLineArgs().Skip(1))}");
            sb.AppendLine(string.Format("Cam ID: {0}, Client ID {1}, {2}", camIdx, clientIdx, SystemManager.Instance().ExchangeOperator.IsConnected ? "Connected" : "Disconnected"));
            sb.AppendLine(string.Format("Server Addr: {0}, Client Addr {1}", server, client));
            sb.AppendLine(string.Format("CurrentModel: {0}", SystemManager.Instance().CurrentModel?.Name));

            MessageForm.Show(this, sb.ToString());
        }

        private void Console_Clicked(object sender, EventArgs e)
        {
            if (DynMvp.ConsoleEx.IsAlloced)
                DynMvp.ConsoleEx.Free();
            else
                DynMvp.ConsoleEx.Alloc();
        }

        private void Inspect_Clicked(object sender, EventArgs e)
        {
            PreparePanel(ExchangeCommand.V_INSPECT);
            this.WindowState = FormWindowState.Maximized;
            this.Visible = true;
        }

        private void Model_Clicked(object sender, EventArgs e)
        {
            PreparePanel(ExchangeCommand.V_MODEL);
            this.WindowState = FormWindowState.Maximized;
            this.Visible = true;
        }

        private void Teach_Clicked(object sender, EventArgs e)
        {
            PreparePanel(ExchangeCommand.V_TEACH);
            this.WindowState = FormWindowState.Maximized;
            this.Visible = true;
        }

        private void Log_Clicked(object sender, EventArgs e)
        {
            PreparePanel(ExchangeCommand.V_LOG);
            this.WindowState = FormWindowState.Maximized;
            this.Visible = true;
        }

        private void Setting_Clicked(object sender, EventArgs e)
        {
            PreparePanel(ExchangeCommand.V_SETTING);
            this.WindowState = FormWindowState.Maximized;
            this.Visible = true;
        }

        private void Close_Clicked(object sender, EventArgs e)
        {
            //this.Show();
            this.WindowState = FormWindowState.Minimized;
            //this.Hide();
        }

        private void Exit_Clicked(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            LogInForm logInForm = new LogInForm();
            if (logInForm.ShowDialog() == DialogResult.OK)
                UserHandler.Instance().CurrentUser = logInForm.LogInUser;
        }

        private void TrayIconForm_Load(object sender, EventArgs e)
        {
#if DEBUG
            this.FormBorderStyle = FormBorderStyle.Sizable;
#endif

            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.AllScreens.FirstOrDefault(f => !f.Primary);
            if (screen == null)
                screen = System.Windows.Forms.Screen.PrimaryScreen;

            this.StartPosition = FormStartPosition.Manual;
            this.MinimumSize = screen.WorkingArea.Size;
            this.Location = screen.WorkingArea.Location;
            this.WindowState = FormWindowState.Maximized;

            // Add를 해야 각 Panel의 Size가 계산된다.
            panelContol.Controls.Add((Control)inspectPanel);
            panelContol.Controls.Add((Control)modelPanel);
            panelContol.Controls.Add((Control)teachPanel);
            panelContol.Controls.Add((Control)logPanel);
            panelContol.Controls.Clear();

            this.Controls.Add(this.labelModeIndicator);
            this.Controls.SetChildIndex(this.labelModeIndicator, 0);
            PreparePanel(ExchangeCommand.V_MODEL);
            //panelContol.Controls.Add((Control)modelPanel);

            string title = CustomizeSettings.Instance().ProgramTitle;
            string copyright = CustomizeSettings.Instance().Copyright;
            this.Text = SystemManager.Instance().UiChanger.GetMainformTitle();

            //labelMode.Text = MachineSettings.Instance().RunningMode.ToString();
            //this.Show();
            //this.Hide();
        }

        private void TrayIconForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogHelper.Debug(LoggerType.Operation, string.Format("InspectorMainForm::TrayIconForm_FormClosing - CloseReason: {0}", e.CloseReason));
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void TrayIconForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon.Dispose();
        }

        delegate void PreparePanelDelegate(ExchangeCommand eVisit);
        public void PreparePanel(ExchangeCommand eVisit)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new PreparePanelDelegate(PreparePanel), eVisit);
                return;
            }

            IMainTabPage selectedControl = null;
            switch (eVisit)
            {
                case ExchangeCommand.V_SHOW:
                    this.WindowState = FormWindowState.Maximized;
                    break;
                case ExchangeCommand.V_HIDE:
                    this.WindowState = FormWindowState.Minimized;
                    break;

                case ExchangeCommand.V_INSPECT:
                    selectedControl = inspectPanel;
                    break;
                case ExchangeCommand.V_MODEL:
                    selectedControl = modelPanel;
                    break;
                case ExchangeCommand.V_TEACH:
                    selectedControl = teachPanel;
                    break;
                case ExchangeCommand.V_LOG:
                    selectedControl = logPanel;
                    break;

                case ExchangeCommand.V_SETTING:
                    selectedControl = settingPanel;
                    break;

                // do nothing
                case ExchangeCommand.V_REPORT:
                    break;

                case ExchangeCommand.V_DONE:
                    //selectedControl = inspectPanel;
                    //this.Hide();
                    break;
            }

            if (selectedControl == null)
                return;

            if (curSelectedPage != null)
            {
                curSelectedPage.PageVisibleChanged(false);
                panelContol.Controls.Remove((Control)curSelectedPage);
            }

            panelContol.Controls.Add((Control)selectedControl);
            //this.virtualIndicator.Parent = (Control)selectedControl;
            selectedControl.PageVisibleChanged(true);

            curSelectedPage = selectedControl;
            //this.WindowState = FormWindowState.Maximized;
            //this.Show();
        }

        delegate void ClearDelegate();
        public void Clear()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new ClearDelegate(Clear));
                return;
            }

            //panelContol.Controls.Clear();
            this.Hide();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            foreach (MenuItem menuItem in masterContextMenu.MenuItems)
                if (!string.IsNullOrEmpty(menuItem.Name))
                    menuItem.Text = StringManager.GetString(this.GetType().FullName, menuItem.Name, menuItem.Text);
        }

        bool isConnected = false;
        private void timer_Tick(object sender, EventArgs e)
        {
            //((IClientExchangeOperator)SystemManager.Instance().ExchangeOperator).SendAlive();
            bool isConnected = ((IClientExchangeOperator)SystemManager.Instance().ExchangeOperator).IsConnected;
            if (this.isConnected != isConnected)
            {
                this.isConnected = isConnected;
                if (isConnected)
                    this.Show();
                else
                {
                    if (SystemState.Instance().OpState != OpState.Idle)
                        SystemManager.Instance().InspectRunner.ExitWaitInspection();
                    this.Hide();
                }
            }

            bool toggle = (MachineSettings.Instance().RunningMode != RunningMode.Real && this.showRunningModeCount == 0);
            if (toggle)
                this.labelModeIndicator.Visible = !this.labelModeIndicator.Visible;
            this.showRunningModeCount = (this.showRunningModeCount + timer.Interval) % 1000;
        }

        private void panelContol_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
