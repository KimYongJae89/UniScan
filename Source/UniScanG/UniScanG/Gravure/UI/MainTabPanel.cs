using System.Windows.Forms;
using DynMvp.Base;
using Infragistics.Win.UltraWinTabControl;
using UniEye.Base.Data;
using DynMvp.UI.Touch;
using UniScanG.Common.UI;
using UniEye.Base.UI;
using UniScanG.Common;
using UniScanG.UI;
using DynMvp.Authentication;
using System;
using UniScanG.Data.Model;
using UniScanG.Common.Data;
using System.Collections.Generic;
using System.IO;
using System.Drawing;

namespace UniScanG.Gravure.UI
{
    public partial class MainTabPanel : UserControl, IUiControlPanel, LoggingTarget, IModelListener, IUserHandlerListener, IMultiLanguageSupport
    {
        public MainTabPanel(Control inspectPage, Control modelPage, Control teachPage, Control reportPage, Control settingPage, Control logPage)
        {
            InitializeComponent();
            StringManager.AddListener(this);

            this.Dock = DockStyle.Fill;

            this.inspectPage.Controls.Add(inspectPage);
            this.modelPage.Controls.Add(modelPage);
            this.teachPage.Controls.Add(teachPage);
            this.reportPage.Controls.Add(reportPage);
            this.settingPage.Controls.Add(settingPage);
            this.logPage.Controls.Add(logPage);

            LogHelper.LoggingTarget = this;
            
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            UserHandler.Instance().AddListener(this);

            UserChanged();
            UpdateTab(false);
        }
        
        public void Log(string messgae)
        {
        }

        private void mainTab_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            if (e.PreviousSelectedTab != null)
            {
                UltraTabPageControl prevTabPage = e.PreviousSelectedTab.TabPage;
                if (prevTabPage.Controls.Count > 0 && prevTabPage.Controls[0] is IMainTabPage)
                {
                    IMainTabPage mainTabPage = (IMainTabPage)prevTabPage.Controls[0];
                    mainTabPage.PageVisibleChanged(false);
                }
            }

            UltraTabPageControl curTabPage = e.Tab.TabPage;
            if (curTabPage.Controls.Count > 0 && curTabPage.Controls[0] is IMainTabPage)
            {
                IMainTabPage mainTabPage = (IMainTabPage)curTabPage.Controls[0];
                mainTabPage.PageVisibleChanged(true);
            }

            if (SystemManager.Instance().UiController != null)
                SystemManager.Instance().UiController.TabChanged(e.Tab.Key);
        }

        private void mainTab_SelectedTabChanging(object sender, SelectedTabChangingEventArgs e)
        {
            //if (e.Tab.Key == MainTabKey.Setting.ToString())
            //{
            //    e.Cancel = true;
            //    Form form = new Form();
            //    UniScanG.Settings.UI.MachineIfViewPanel machineIfViewPanel = new UniScanG.Settings.UI.MachineIfViewPanel();
            //    machineIfViewPanel.Initialize(UniEye.Base.Settings.MachineSettings.Instance().MachineIfSetting);
            //    machineIfViewPanel.Dock = DockStyle.Fill;
            //    form.Controls.Add(machineIfViewPanel);
            //    form.ShowDialog();
            //}

            if (e.Tab.Key == MainTabKey.Exit.ToString())
            {
                e.Cancel = true;

                if (CheckFormCloseing() == true)
                {
                    this.ParentForm.Close();
                }

                return;
            }
        }

        private bool CheckFormCloseing()
        {
            if (SystemState.Instance().OpState != OpState.Idle)
            {
                MessageForm.Show(this.ParentForm, StringManager.GetString("Please, Stop the inspection."), "UniEye");
                return false;
            }

            if (MessageForm.Show(this.ParentForm, StringManager.GetString("Message", "Do you want to exit program?"), MessageFormType.YesNo) == DialogResult.No)
            {
                return false;
            }

            //UniScanG.Data.Model.Model model = SystemManager.Instance().CurrentModel;
            //if (model != null && model.Modified)
            //{
            //    SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
            //}

            return true;
        }

        public string GetCurrentTabKey()
        {
            if (mainTab.SelectedTab == null)
                return null;

            return mainTab.SelectedTab.Key;
        }

        delegate void TrainedDelegate(bool trained);
        public void UpdateTab(bool trained)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new TrainedDelegate(UpdateTab), trained);
                return;
            }
            
            if (SystemManager.Instance().CurrentModel == null)
            {
                mainTab.Tabs[MainTabKey.Inspect.ToString()].Enabled = false;
                mainTab.Tabs[MainTabKey.Teach.ToString()].Enabled = false;
                return;
            }

            mainTab.Tabs[MainTabKey.Teach.ToString()].Enabled = true;
            mainTab.Tabs[MainTabKey.Inspect.ToString()].Enabled = true;
        }
        
        delegate void TabDelegate(string key);
        public void EnableTab(string key)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new TabDelegate(EnableTab), key);
                return;
            }

            if (mainTab.Tabs[key] == null)
                return;

            mainTab.Tabs[key].Enabled = true;
        }

        public void DisableTab(string key)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new TabDelegate(EnableTab), key);
                return;
            }

            if (mainTab.Tabs[key] == null)
                return;

            mainTab.Tabs[key].Enabled = false;
        }
        
        public void VisibleTab(string key)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new TabDelegate(VisibleTab), key);
                return;
            }

            if (mainTab.Tabs[key] == null)
                return;

            mainTab.Tabs[key].Visible = true;
        }

        public void InvisibleTab(string key)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new TabDelegate(InvisibleTab), key);
                return;
            }

            if (mainTab.Tabs[key] == null)
                return;

            mainTab.Tabs[key].Visible = false;
        }

        public void ChangeTab(string key)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new TabDelegate(ChangeTab), key);
                return;
            }

            if (mainTab.Tabs[key] == null)
                return;

            if (mainTab.Tabs[key].Enabled == false)
                return;

            mainTab.Tabs[key].Selected = true;
            return;
        }
        
        public void ModelChanged()
        {
            TabStateChange();
        }

        public void ModelTeachDone(int camId)
        {
            TabStateChange();
        }

        public void ModelRefreshed() { }

        private void TabStateChange()
        {
            if (SystemManager.Instance().CurrentModel == null)
            {
                DisableTab(MainTabKey.Inspect.ToString());
                DisableTab(MainTabKey.Teach.ToString());
            }
            else
            {
                EnableTab(MainTabKey.Teach.ToString());

                if (SystemManager.Instance().ExchangeOperator.ModelTrained(SystemManager.Instance().CurrentModel.ModelDescription) == true)
                    EnableTab(MainTabKey.Inspect.ToString());
                else
                    DisableTab(MainTabKey.Inspect.ToString());
            }
        }

        public void UserChanged()
        {
            User curUser = UserHandler.Instance().CurrentUser;
            if (curUser == null)
                return;

            if (curUser.IsMasterAccount)
            {
                VisibleTab(MainTabKey.Setting.ToString());
                //VisibleTab(MainTabKey.Log.ToString());
            }
            else
            {
                InvisibleTab(MainTabKey.Setting.ToString());
                //InvisibleTab(MainTabKey.Log.ToString());
            }
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            // Infragistics Tab Controls
            foreach (string tabKey in Enum.GetNames(typeof(MainTabKey)))
                mainTab.Tabs[tabKey].Text = StringManager.GetString(this.GetType().FullName, mainTab.Tabs[tabKey].Key);
        }

        string message = "";
        private void mainTab_Paint(object sender, PaintEventArgs e)
        {
            if (e.ClipRectangle.Size != mainTab.Size)
                return;

            string message = DynMvp.Devices.FrameGrabber.CameraConfiguration.ConfigFlag.ToUpper();
            
            Graphics g = e.Graphics;
            float emSize = 28;
            Font font = new Font(FontFamily.GenericSansSerif, emSize, FontStyle.Bold);

            float x = e.ClipRectangle.Right - ((message.Length + 0.5f) * font.SizeInPoints);
            float y = e.ClipRectangle.Bottom - (1.5f * font.SizeInPoints);

            g.DrawString(message, font, new SolidBrush(Color.Red), x, y);
            
        }
    }
}
