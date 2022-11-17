using DynMvp.Authentication;
using DynMvp.Base;
using DynMvp.Data;
using System;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss.UI
{
    public partial class SettingPage : UserControl, ISettingPage, IMultiLanguageSupport
    {
        #region 생성자
        public SettingPage(GlossSettings setting)
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;
            this.Text = StringManager.GetString(this.GetType().FullName, this.Text);

            this.Setting = setting;
            propertyGrid.SelectedObject = setting;

            StringManager.AddListener(this);
        }
        #endregion

        #region 이벤트
        private void buttonOK_Click(object sender, EventArgs e)
        {
            GlossSettings.Instance().Save();
        }

        private void buttonWidthSetting_Click(object sender, EventArgs e)
        {
            var form = new ScanWidthSettingForm(GlossSettings.Instance().GlossScanWidthList);
            form.ShowDialog(this);
        }

        private void propertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            GlossSettings.Instance().OnChanged();
        }
        #endregion

        #region 인터페이스
        public void Initialize() { }

        public void SaveSettings() { }

        public void EnableControls(UserType user) { }

        public void UpdateControl(string item, object value) { }

        public void PageVisibleChanged(bool visibleFlag)
        {
            this.Visible = visibleFlag;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
        #endregion

        #region 속성
        public Control ShowHideControl { get; set; }

        private GlossSettings Setting { get; set; }
        #endregion

        #region 메서드

        #endregion
    }
}
