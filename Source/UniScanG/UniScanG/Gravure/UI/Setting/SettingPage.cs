using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.UI.Touch;
using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniScanG.Gravure.Vision;

namespace UniScanG.Gravure.UI.Setting
{
    public partial class SettingPage : UserControl, IMainTabPage, ISettingPage, IUserHandlerListener, IMultiLanguageSupport
    {
        Dictionary<Button, ISettingSubPage> navigator = new Dictionary<Button, ISettingSubPage>();
        ISettingPageExtender extender = null;

        public SettingPage(ISettingPageExtender settingPageExtender)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.extender = settingPageExtender;

            navigator.Add(buttonNavigateAlarm, this.extender.CreateAlarmPage());
            navigator.Add(buttonNavigateComm, this.extender.CreateCommPage());
            navigator.Add(buttonNavigateGrade, this.extender.CreateGradePage());
            navigator.Add(buttonNavigateGeneral, this.extender.CreateGeneralPage());

            this.buttonCollectLog.Visible = this.extender.IsVisibleCollectLogButton;

            Initialize();

            UserHandler.Instance().AddListener(this);
            StringManager.AddListener(this);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Initialize()
        {
            foreach (KeyValuePair<Button, ISettingSubPage> pair in this.navigator)
            {
                pair.Key.Visible = !(pair.Value == null);
                pair.Value?.Initialize();
            }

            AdditionalSettings.Instance().AdditionalSettingChangedDelegate += new AdditionalSettingChangedDelegate(() => UpdateData());
        }
        
        public void EnableControls(UserType userType)
        {

        }

        public void TabPageVisibleChanged(bool visibleFlag)
        {   
         
        }

        public void UpdateData()
        {
            foreach (ISettingSubPage settingSubPage in this.navigator.Values)
                settingSubPage?.UpdateData();
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

        public void UserChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UserChangedDelegate(UserChanged));
                return;
            }

            User curUser = UserHandler.Instance().CurrentUser;
            if (curUser == null)
                return;
            //settingTabControl.Tabs[ultraTabPageMonitoring.Tab.Key].Visible = curUser.Id.ToUpper() == "DEVELOPER";
        }

        public void UpdateControl(string item, object value)
        {
            throw new NotImplementedException();
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == true)
                UpdateData();
        }

        private void SettingPage_Load(object sender, EventArgs e)
        {
            Navigate(navigator.ElementAt(0).Key);
        }

        private void buttonNavigate_Click(object sender, EventArgs e)
        {
            Navigate((Button)sender);
        }

        private void Navigate(Button button)
        {
            foreach (KeyValuePair<Button, ISettingSubPage> pair in navigator)
            {
                if (pair.Key.Name != button.Name)
                {
                    pair.Key.BackColor = DefaultBackColor;
                    continue;
                }

                pair.Key.BackColor = System.Drawing.Color.LightGreen;
                this.panelMain.Controls.Clear();

                pair.Value?.UpdateData();
                this.panelMain.Controls.Add((UserControl)pair.Value);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            new SimpleProgressForm("Save").Show(() =>
            {
                MachineSettings.Instance().Save();
                AdditionalSettings.Instance().Save();
                Common.ExchangeOperator op = SystemManager.Instance().ExchangeOperator;
                //AdditionalSettings.Instance().Load();
            });
            //MessageForm.Show(this, StringManager.GetString("Save Done"));
        }

        private void buttonOpenConsole_Click(object sender, EventArgs e)
        {
            if (DynMvp.ConsoleEx.IsAlloced)
                DynMvp.ConsoleEx.Free();
            else
                DynMvp.ConsoleEx.Alloc();
        }

        private void buttonCollectLog_Click(object sender, EventArgs e)
        {
            this.extender.CollectLog(this);
        }
    }
}
