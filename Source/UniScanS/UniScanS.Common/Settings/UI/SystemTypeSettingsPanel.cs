using System;
using System.Windows.Forms;
using DynMvp.Base;
using UniEye.Base.Settings.UI;

namespace UniScanS.Common.Settings.UI
{
    public interface ICustomConfigPage
    {
        void UpdateData();
        void SaveData();
    }

    public partial class SystemTypeSettingPanel : UserControl, UniEye.Base.Settings.UI.ICustomConfigPage
    {
        ICustomConfigPage customConfigSubPage;

        public SystemTypeSettingPanel(ICustomConfigPage customConfigSubPage)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            
            labelSystemType.Text = StringManager.GetString(this.GetType().FullName, labelSystemType.Text);

            if (customConfigSubPage != null)
            {
                this.customConfigSubPage = customConfigSubPage;
                subPanel.Controls.Add((Control)customConfigSubPage);
            }
        }

        public void UpdateData()
        {
            resizeRatio.Value = (decimal)SystemTypeSettings.Instance().ResizeRatio;
            if (customConfigSubPage != null)
                customConfigSubPage.UpdateData();
        }

        public bool SaveData()
        {
            SystemTypeSettings.Instance().Save();

            customConfigSubPage.SaveData();

            return true;
        }

        private void resizeRatio_ValueChanged(object sender, EventArgs e)
        {
            SystemTypeSettings.Instance().ResizeRatio = (float)resizeRatio.Value;
        }
    }
}
