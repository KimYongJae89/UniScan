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
using UniScanS;
using UniScanS.Common;
using UniScanS.Common.Exchange;

namespace UniScanS.UI
{
    public partial class InspectorMainForm : Form, IVisitListener, IMultiLanguageSupport
    {
        LogInForm loginForm = new LogInForm();

        ContextMenu opContextMenu = new ContextMenu();
        ContextMenu masterContextMenu = new ContextMenu();

        Control inspectPanel;
        Control modelPanel;
        Control teachPanel;
        Control reportPanel;
        ISettingPage settingPanel;
        
        public InspectorMainForm()
        {
            InitializeComponent();
            StringManager.AddListener(this);

            this.WindowState = FormWindowState.Maximized;
            //loginForm.TopMost = true;

            InitPanels();

            InitContextMenu();

            IClientExchangeOperator clientExchangeOperator = (IClientExchangeOperator)SystemManager.Instance().ExchangeOperator;
            clientExchangeOperator.AddVisitListener(this);
        }

        private void InitPanels()
        {
            InspectorUiChanger inspectorUiChanger = (InspectorUiChanger)SystemManager.Instance().UiChanger;

            inspectPanel = SystemManager.Instance().UiChanger.CreateInspectPage();
            modelPanel = inspectorUiChanger.CreateModelPage();
            teachPanel = SystemManager.Instance().UiChanger.CreateTeachPage();
            reportPanel = SystemManager.Instance().UiChanger.CreateReportPage();
            //settingPanel = SystemManager.Instance().UiChanger.CreateSettingPage();
        }

        private void InitContextMenu()
        {
            MenuItem inspectMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Inspect"), Inspect_Clicked);
            MenuItem modelMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Model"), Model_Clicked);
            MenuItem teachMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Teach"), Teach_Clicked);
            MenuItem reportMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Report"), Report_Clicked);
            MenuItem settingMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Setting"), Setting_Clicked);
            MenuItem closeMenuItem = new MenuItem(StringManager.GetString(this.GetType().FullName, "Close"), Close_Clicked);
            MenuItem exitMenuItem1 = new MenuItem(StringManager.GetString(this.GetType().FullName, "Exit"), Exit_Clicked);
            MenuItem exitMenuItem2 = new MenuItem(StringManager.GetString(this.GetType().FullName, "Exit"), Exit_Clicked);

            masterContextMenu.MenuItems.Add(inspectMenuItem);
            masterContextMenu.MenuItems.Add(modelMenuItem);
            masterContextMenu.MenuItems.Add(teachMenuItem);
            masterContextMenu.MenuItems.Add(reportMenuItem);
            masterContextMenu.MenuItems.Add(settingMenuItem);
            masterContextMenu.MenuItems.Add(new MenuItem("-"));
            masterContextMenu.MenuItems.Add(closeMenuItem);
            masterContextMenu.MenuItems.Add(exitMenuItem1);

            opContextMenu.MenuItems.Add(exitMenuItem2);
            notifyIcon.ContextMenu = opContextMenu;
        }
        
        private void Inspect_Clicked(object sender, EventArgs e)
        {
            PreparePanel(ExchangeCommand.V_INSPECT);
        }

        private void Model_Clicked(object sender, EventArgs e)
        {
            PreparePanel(ExchangeCommand.V_MODEL);
        }

        private void Teach_Clicked(object sender, EventArgs e)
        {
            PreparePanel(ExchangeCommand.V_TEACH);
        }

        private void Report_Clicked(object sender, EventArgs e)
        {
            PreparePanel(ExchangeCommand.V_REPORT);
        }

        private void Setting_Clicked(object sender, EventArgs e)
        {
            PreparePanel(ExchangeCommand.V_SETTING);
        }

        private void Close_Clicked(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Exit_Clicked(object sender, EventArgs e)
        {
            this.Close();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (loginForm.Visible)
                return;

            if (loginForm.ShowDialog() == DialogResult.OK)
                UserHandler.Instance().CurrentUser = loginForm.LogInUser;

            if (UserHandler.Instance().CurrentUser.IsSuperAccount == true)
                notifyIcon.ContextMenu = masterContextMenu;
            else
                notifyIcon.ContextMenu = opContextMenu;
        }

        private void TrayIconForm_Load(object sender, EventArgs e)
        {
            panelContol.Controls.Add(inspectPanel);
            panelContol.Controls.Add(modelPanel);
            panelContol.Controls.Add(teachPanel);
            panelContol.Controls.Add(reportPanel);

            this.Show();

            panelContol.Controls.Clear();
            panelContol.Controls.Add(modelPanel);

            SystemManager.Instance().ExchangeOperator.Start();
            //this.Hide();
        }

        private void TrayIconForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
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
                Invoke(new PreparePanelDelegate(PreparePanel), eVisit);
                return;
            }
            
            panelContol.Controls.Clear();

            switch (eVisit)
            {
                case ExchangeCommand.V_INSPECT:
                    panelContol.Controls.Add(inspectPanel);
                    break;
                case ExchangeCommand.V_MODEL:
                    panelContol.Controls.Add(modelPanel);
                    break;
                case ExchangeCommand.V_TEACH:
                    if(SystemManager.Instance().CurrentModel==null)
                    {
                        MessageForm.Show(this, "Model is NOT Selected");
                        break;
                    }
                    panelContol.Controls.Add(teachPanel);
                    break;
                case ExchangeCommand.V_REPORT:
                    panelContol.Controls.Add(reportPanel);
                    break;
                case ExchangeCommand.V_SETTING:
                    //panelContol.Controls.Add(settingPanel);
                    break;
            }

            this.Show();

            //this.TopMost = true;

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
            // 뭐 없네
        }
    }
}
