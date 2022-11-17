using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DynMvp.Base;
using UniEye.Base.Settings.UI;

namespace UniScanG.Common.Settings.UI
{
    public interface ICustomConfigPage
    {
        void UpdateData();
        bool SaveData();
    }

    public partial class SystemTypeSettingPanel : UserControl, UniEye.Base.Settings.UI.ICustomConfigPage
    {
        ICustomConfigPage customConfigSubPage;

        public SystemTypeSettingPanel(ICustomConfigPage customConfigSubPage)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            if (customConfigSubPage != null)
            {
                this.customConfigSubPage = customConfigSubPage;
                subPanel.Controls.Add((Control)customConfigSubPage);
            }
        }

        public void UpdateData()
        {
            this.resizeRatio.Value = (decimal)SystemTypeSettings.Instance().ResizeRatio;
            this.simpleReportLotList.Checked = SystemTypeSettings.Instance().ShowSimpleReportLotList;
            this.localExchangeMode.Checked = SystemTypeSettings.Instance().LocalExchangeMode;

            if (customConfigSubPage != null)
                customConfigSubPage.UpdateData();
        }

        public bool SaveData()
        {
            SystemTypeSettings.Instance().ResizeRatio = (float)resizeRatio.Value;
            SystemTypeSettings.Instance().ShowSimpleReportLotList = this.simpleReportLotList.Checked;
            SystemTypeSettings.Instance().LocalExchangeMode = this.localExchangeMode.Checked;

            SystemTypeSettings.Instance().Save();

            LicenseManager.Save();

            return customConfigSubPage.SaveData();
        }

        private void resizeRatio_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}
