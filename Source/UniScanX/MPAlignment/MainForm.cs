using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanX.MPAlignment.UI.Pages;
using UniEye.Base.UI;
using DynMvp.Authentication;
using UniScanX.MPAlignment.Data;

namespace UniScanX.MPAlignment
{
    public partial class MainForm : Form, IMainForm
    {
        DynMvp.Devices.UI.IoPortViewer ioMonitorView = null;
        ProductModelManagePage productModelManagePage;
        UI.Pages.InspectPage InspectPage;
        MPAlignment.UI.Pages.ModellerPage modellerPage =null;
        UI.Pages.ReportPage reportPage;
        UserConfigPage userConfigPage;
        UI.Pages.LogPage logPage;
        List<System.Windows.Forms.Button> menuButtons = new List<System.Windows.Forms.Button>();

        static Color selectedColor = Color.Gray;
        static Color unSelectedColor = Color.FromArgb(60, 70, 90);

        IInspectionPage IMainForm.InspectPage => throw new NotImplementedException();

        public ITeachPage TeachPage => throw new NotImplementedException();

        public IModelManagerPage ModelManagerPage => throw new NotImplementedException();

        public IReportPage ReportPage => throw new NotImplementedException();

        public ISettingPage SettingPage => throw new NotImplementedException();

        public MainForm()
        {
            InitializeComponent();
            InitPages();
            InitMenuButtons();

            void InitPages()
            {
                productModelManagePage = new ProductModelManagePage();
                productModelManagePage.Name = "ProductModelManagePage";
                productModelManagePage.Dock = DockStyle.Fill;
                productModelManagePage.AccessibleName = "ProductModelManagePage";
                pnlMain.Controls.Add(productModelManagePage);

                InspectPage = new UI.Pages.InspectPage();
                InspectPage.Name = "InspectPage";
                InspectPage.Dock = DockStyle.Fill;
                InspectPage.AccessibleName = "InspectPage";
                pnlMain.Controls.Add(InspectPage);

                modellerPage = new MPAlignment.UI.Pages.ModellerPage();
                modellerPage.Name = "teachPage";
                modellerPage.Dock = DockStyle.Fill;
                modellerPage.AccessibleName = "TeachPage";
                pnlMain.Controls.Add(modellerPage);

                reportPage = new UI.Pages.ReportPage();
                reportPage.Name = "reportPage";
                reportPage.Dock = DockStyle.Fill;
                reportPage.AccessibleName = "ReportPage";
                pnlMain.Controls.Add(reportPage);

                userConfigPage = new UI.Pages.UserConfigPage();
                userConfigPage.Name = "userConfigPage";
                userConfigPage.Dock = DockStyle.Fill;
                userConfigPage.AccessibleName = "ReportPage";
                pnlMain.Controls.Add(userConfigPage);

                logPage = new UI.Pages.LogPage();
                logPage.Name = "logPage";
                logPage.Dock = DockStyle.Fill;
                logPage.AccessibleName = "LogPage";
                pnlMain.Controls.Add(logPage);
            }
            void InitMenuButtons()
            {
                btnProductManager.Tag = productModelManagePage;
                btnInspect.Tag = InspectPage;
                btnTeach.Tag = modellerPage;
                btnReport.Tag = reportPage;
                btnUserConfig.Tag = userConfigPage;
                btnLogs.Tag = logPage;

                menuButtons.Add(btnProductManager);
                menuButtons.Add(btnInspect);
                menuButtons.Add(btnTeach);
                menuButtons.Add(btnReport);
                menuButtons.Add(btnUserConfig);
                menuButtons.Add(btnLogs);
            }
           ((MPAlignment.Data.ModelManager)(SystemManager.Instance().ModelManager)).ModelLoaded += OnModelChanged;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //string title = StringManager.GetString("Info", "Info");
            //string message = StringManager.GetString("Do you want to close Uni AoiA?");
            //var messageForm = new AoiMessageForm(title, message);
            //if (messageForm.ShowDialog() == DialogResult.OK)
            //{
            //    ModelManager.Instance.CloseCurrentModel();
            //    ModelManager.Instance.CloseCapturedModel();
            //    this.Close();
            //}
            this.Close();
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void MenuButtonClicked(object sender, EventArgs e)
        {
            System.Windows.Forms.Button selectedButton = sender as System.Windows.Forms.Button;
            //        LogHelper.Debug(LoggerType.Operation, $"MenuClick - {selectedButton.Name}");
            this.SuspendLayout();

            foreach (var button in menuButtons)
            {
                button.BackColor = unSelectedColor;
                if (button.Tag !=null)
                {
                    var page = button.Tag as UserControl;
                    if (page != null)
                    {
                        if (button == selectedButton)
                        {
                            button.BackColor = selectedColor;
                            page.Visible = true;
                        }
                        else page.Visible = false;
                    }
                }
            }
            this.ResumeLayout();
        }

        public void OnModelChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            var currentModel = ((ModelManager)(SystemManager.Instance().ModelManager)).CurrentModel;
            if (currentModel == null)
            {
                EnableControl(false);
                lblSelectedProduct.Text = "-";
                return;
            }
            lblSelectedProduct.Text = ((ModelDescription)currentModel.ModelDescription).Name;
            InspectPage.Initialize();
            modellerPage.Initialize(currentModel);
            if (currentModel.IsTaught())
            {
               // ChangeSelection(1);
            }
            else
            {
            //    ChangeSelection(2);
            }
            EnableControl();

            void EnableControl(bool enable = true)
            {
                btnInspect.Enabled = true;
                btnTeach.Enabled = true;
            }
        }


        public void EnableTabs()
        {
            throw new NotImplementedException();
        }

        public void PageChange(IMainTabPage page, UserType userType = UserType.Maintrance)
        {
            throw new NotImplementedException();
        }



        public void WorkerChanged(string opName)
        {
            throw new NotImplementedException();
        }

        public void ModifyTeaching(string imageFileName)
        {
            throw new NotImplementedException();
        }

        private void btnIoMonitor_Click(object sender, EventArgs e)
        {
            if (ioMonitorView != null)
            {
                ioMonitorView.Close();
                ioMonitorView.Dispose();
            }
            
            ioMonitorView = new DynMvp.Devices.UI.IoPortViewer(
                SystemManager.Instance().DeviceBox.DigitalIoHandler,
                SystemManager.Instance().DeviceBox.PortMap
                );
            ioMonitorView.Show();
        }

        public void OnModelChanged()
        {
            throw new NotImplementedException();
        }
    }
}
