using System.Windows.Forms;
using DynMvp.Base;
using Infragistics.Win.UltraWinTabControl;
using UniEye.Base.Data;
using DynMvp.UI.Touch;
using UniScanS.Common.UI;
using UniEye.Base.UI;
using UniScanS.Common;
using UniScanS.UI;
using DynMvp.Authentication;
using System;

namespace UniScanS.Screen.UI
{
    public partial class MainTabPanel : UserControl, IUiControlPanel, LoggingTarget, IModelListener, IUserHandlerListener, IMultiLanguageSupport, IOpStateListener
    {
        public MainTabPanel(Control inspectPage, Control modelPage, Control teachPage, Control reportPage)
        {
            InitializeComponent();
            StringManager.AddListener(this);

            this.Dock = DockStyle.Fill;

            this.inspectPage.Controls.Add(inspectPage);
            this.modelPage.Controls.Add(modelPage);
            this.teachPage.Controls.Add(teachPage);
            this.reportPage.Controls.Add(reportPage);
            
            LogHelper.LoggingTarget = this;
            
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
            UserHandler.Instance().AddListener(this);
            SystemState.Instance().AddOpListener(this);
            UserChanged();
            UpdateTab(false);
        }
        
        public void Log(string messgae)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new LogDelegate(Log), messgae);
                return;
            }
        }

        private void mainTab_SelectedTabChanged(object sender, SelectedTabChangedEventArgs e)
        {
            if (e.PreviousSelectedTab != null)
            {
                UltraTabPageControl prevTabPage = e.PreviousSelectedTab.TabPage;

                if (prevTabPage.Controls.Count > 0 && prevTabPage.Controls[0] is IMainTabPage)
                {
                    IMainTabPage mainTabPage = (IMainTabPage)prevTabPage.Controls[0];
                    //mainTabPage.TabPageVisibleChanged(false);
                }
            }

            UltraTabPageControl curTabPage = e.Tab.TabPage;
            if (curTabPage.Controls.Count > 0 && curTabPage.Controls[0] is IMainTabPage)
            {
                IMainTabPage mainTabPage = (IMainTabPage)curTabPage.Controls[0];
                //mainTabPage.TabPageVisibleChanged(true);
            }

            if (SystemManager.Instance().UiController != null)
                SystemManager.Instance().UiController.TabChanged(e.Tab.Key);
        }

        private void mainTab_SelectedTabChanging(object sender, SelectedTabChangingEventArgs e)
        {
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
                MessageForm.Show(this.ParentForm, StringManager.GetString("Message", "Please, Stop the inspection."), "UniEye");
                return false;
            }

            if (MessageForm.Show(this.ParentForm, StringManager.GetString("Message", "Do you want to exit program?"), MessageFormType.YesNo) == DialogResult.No)
            {
                return false;
            }

            UniScanS.Data.Model.Model model = SystemManager.Instance().CurrentModel;
            if (model != null && model.Modified)
            {
                SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
            }

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
                BeginInvoke(new TabDelegate(DisableTab), key);
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

        public void ModelTeachDone()
        {
            TabStateChange();
        }

        private void TabStateChange()
        {
            if (SystemManager.Instance().CurrentModel == null)
            {
                ChangeTab(MainTabKey.Model.ToString());
                DisableTab(MainTabKey.Inspect.ToString());
                DisableTab(MainTabKey.Teach.ToString());
                return;
            }

            EnableTab(MainTabKey.Teach.ToString());

            if (SystemManager.Instance().ExchangeOperator.ModelTrained(SystemManager.Instance().CurrentModel.ModelDescription) == true)
                EnableTab(MainTabKey.Inspect.ToString());
            else
                DisableTab(MainTabKey.Inspect.ToString());
        }

        public void UserChanged()
        {
            if (UserHandler.Instance().CurrentUser.IsSuperAccount)
            {
                VisibleTab(MainTabKey.Log.ToString());
            }
            else
            {
                InvisibleTab(MainTabKey.Log.ToString());
            }
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            // Infragistics Tab Controls
            foreach (string tabKey in Enum.GetNames(typeof(MainTabKey)))
                mainTab.Tabs[tabKey].Text = StringManager.GetString(this.GetType().FullName, mainTab.Tabs[tabKey].Text);
        }

        public void OpStatusChanged(OpState curOpState, OpState prevOpState)
        {
            if (InvokeRequired)
            {
                Invoke(new OpStateChangedDelegate(OpStatusChanged), curOpState, prevOpState);
                return;
            }

            TabStateChange();

            switch (curOpState)
            {
                case OpState.Idle:
                    SetIdleTabState();
                    break;
                case OpState.Inspect:
                    SetInspectTabState();
                    break;
            }
        }

        private void SetIdleTabState()
        {
            EnableTab(MainTabKey.Model.ToString());
            EnableTab(MainTabKey.Teach.ToString());
            EnableTab(MainTabKey.Exit.ToString());
        }

        private void SetInspectTabState()
        {
            DisableTab(MainTabKey.Model.ToString());
            DisableTab(MainTabKey.Teach.ToString());
            DisableTab(MainTabKey.Exit.ToString());
        }

        public void OpStateChanged(OpState curOpState, OpState prevOpState)
        {
            throw new NotImplementedException();
        }
    }
}
