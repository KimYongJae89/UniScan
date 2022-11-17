using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Gravure.UI.Setting;
using DynMvp.Base;
using UniScanG.Gravure.Settings;

namespace UniScanG.Module.Controller.UI.Settings
{
    public partial class SettingGradePage : UserControl, ISettingSubPage, IMultiLanguageSupport
    {
        public SettingGradePage()
        {
            InitializeComponent();

            StringManager.AddListener(this);
        }

        public void Initialize()
        {
            this.groupBoxOverall.Controls.Add(new SettingGradeControl(AdditionalSettings.Instance().Grade) { Location = new Point(25, 25) });
            this.groupBoxNoprint.Controls.Add(new SettingGradeControl(AdditionalSettings.Instance().GradeNP) { Location = new Point(25, 25) });
            this.groupBoxPinhole.Controls.Add(new SettingGradeControl(AdditionalSettings.Instance().GradePH) { Location = new Point(25, 25) });
        }

        public void UpdateData()
        {

        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void SettingGradePage_Load(object sender, EventArgs e)
        {
        }
    }
}
