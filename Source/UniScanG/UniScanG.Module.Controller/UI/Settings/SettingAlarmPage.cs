using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.Gravure.Vision;
using DynMvp.Base;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.UI.Setting;

namespace UniScanG.Module.Controller.UI.Setting
{
    public partial class SettingAlarmPage : UserControl, ISettingSubPage, IMultiLanguageSupport
    {
        public SettingAlarmPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            panelMarginLength.Visible = AlgorithmSetting.Instance().UseExtMargin;

            StringManager.AddListener(this);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Initialize()
        {
            // EA
            normalN.Minimum = normalM.Minimum = repeatedN.Minimum = sheetLengthN.Minimum = marginLengthN.Minimum = defectCountN.Minimum = criticalRollM.Minimum = criticalRollN.Minimum = 1;
            normalN.Maximum = normalM.Maximum = repeatedN.Maximum = sheetLengthN.Maximum = marginLengthN.Maximum = defectCountN.Maximum = criticalRollM.Maximum = criticalRollN.Maximum = decimal.MaxValue;

            // mm
            sheetLengthD.Minimum = stripeBlotW.Minimum = (decimal)0.01;
            sheetLengthD.Maximum = stripeBlotW.Maximum = decimal.MaxValue;

            // um
            repeatedW.Minimum = repeatedH.Minimum = 0;
            marginLengthD.Minimum = criticalRollD.Minimum = (decimal).1;
            repeatedW.Maximum = repeatedH.Maximum = marginLengthD.Maximum = criticalRollD.Maximum = decimal.MaxValue;

            // %
            normalR.Minimum = repeatedR.Minimum = stripeBlotR.Minimum = (decimal)0.1;
            normalR.Maximum = repeatedR.Maximum = stripeBlotR.Maximum = 100;

            //UpdateData();

            AdditionalSettings settings = (AdditionalSettings)AdditionalSettings.Instance();

            this.useNormalDefectAlarm.DataBindings.Add("Checked", settings.NormalDefectAlarm, "Use", true, DataSourceUpdateMode.OnPropertyChanged);
            this.normalN.DataBindings.Add("Value", settings.NormalDefectAlarm, "Count", true, DataSourceUpdateMode.OnPropertyChanged);
            this.normalM.DataBindings.Add("Value", settings.NormalDefectAlarm, "MinimumDefects", true, DataSourceUpdateMode.OnPropertyChanged);
            this.normalR.DataBindings.Add("Value", settings.NormalDefectAlarm, "Percent", true, DataSourceUpdateMode.OnPropertyChanged);

            this.repeatedCombo.DataSource = Enum.GetValues(typeof(RepeatedDefectAlarmSettingCollection.Types));
            this.repeatedCombo.SelectedIndex = 0;

            //this.useRepeatedDefectAlarm.DataBindings.Add("Checked", settings.RepeatedDefectAlarm, "Use", true, DataSourceUpdateMode.OnPropertyChanged);
            //this.repeatedN.DataBindings.Add("Value", settings.RepeatedDefectAlarm, "Count", true, DataSourceUpdateMode.OnPropertyChanged);
            //this.repeatedR.DataBindings.Add("Value", settings.RepeatedDefectAlarm, "Percent", true, DataSourceUpdateMode.OnPropertyChanged);

            this.useSheetLengthAlarm.DataBindings.Add("Checked", settings.SheetLengthAlarm, "Use", true, DataSourceUpdateMode.OnPropertyChanged);
            this.sheetLengthN.DataBindings.Add("Value", settings.SheetLengthAlarm, "Count", true, DataSourceUpdateMode.OnPropertyChanged);
            this.sheetLengthD.DataBindings.Add("Value", settings.SheetLengthAlarm, "Value", true, DataSourceUpdateMode.OnPropertyChanged);

            this.useDefectCountAlarm.DataBindings.Add("Checked", settings.DefectCountAlarm, "Use", true, DataSourceUpdateMode.OnPropertyChanged);
            this.defectCountN.DataBindings.Add("Value", settings.DefectCountAlarm, "Count", true, DataSourceUpdateMode.OnPropertyChanged);

            this.useMarginLengthAlarm.DataBindings.Add("Checked", settings.MarginLengthAlarm, "Use", true, DataSourceUpdateMode.OnPropertyChanged);
            this.marginLengthN.DataBindings.Add("Value", settings.MarginLengthAlarm, "Count", true, DataSourceUpdateMode.OnPropertyChanged);
            this.marginLengthD.DataBindings.Add("Value", settings.MarginLengthAlarm, "Value", true, DataSourceUpdateMode.OnPropertyChanged);

            this.useStripeBlot.DataBindings.Add("Checked", settings.StripeDefectAlarm, "Use", true, DataSourceUpdateMode.OnPropertyChanged);
            this.stripeBlotW.DataBindings.Add("Value", settings.StripeDefectAlarm, "WindowWidth", true, DataSourceUpdateMode.OnPropertyChanged);
            this.stripeBlotR.DataBindings.Add("Value", settings.StripeDefectAlarm, "Percent", true, DataSourceUpdateMode.OnPropertyChanged);

            this.useCriticalRollAlarm.DataBindings.Add("Checked", settings.CriticalRollAlarm, "Use", true, DataSourceUpdateMode.OnPropertyChanged);
            this.criticalRollN.DataBindings.Add("Value", settings.CriticalRollAlarm, "Count", true, DataSourceUpdateMode.OnPropertyChanged);
            this.criticalRollM.DataBindings.Add("Value", settings.CriticalRollAlarm, "DefectCount", true, DataSourceUpdateMode.OnPropertyChanged);
            this.criticalRollD.DataBindings.Add("Value", settings.CriticalRollAlarm, "DefectLength", true, DataSourceUpdateMode.OnPropertyChanged);
            this.notifyOnceCriticalRollAlarm.DataBindings.Add("Checked", settings.CriticalRollAlarm, "NotifyOnce", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        public void UpdateData()
        {
        }

        private void repeatedCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.useRepeatedDefectAlarm.DataBindings.Clear();
            this.repeatedN.DataBindings.Clear();
            this.repeatedR.DataBindings.Clear();
            this.repeatedW.DataBindings.Clear();
            this.repeatedH.DataBindings.Clear();

            RepeatedDefectAlarmSettingCollection.Types type = (RepeatedDefectAlarmSettingCollection.Types)repeatedCombo.SelectedItem;
            RepeatedDefectAlarmSetting settings = AdditionalSettings.Instance().RepeatedDefectAlarm.GetAlarmSetting(type);
            this.useRepeatedDefectAlarm.DataBindings.Add("Checked", settings, "Use", true, DataSourceUpdateMode.OnPropertyChanged);
            this.repeatedN.DataBindings.Add("Value", settings, "Count", true, DataSourceUpdateMode.OnPropertyChanged);
            this.repeatedR.DataBindings.Add("Value", settings, "Percent", true, DataSourceUpdateMode.OnPropertyChanged);
            this.repeatedW.DataBindings.Add("Value", settings, "InflateRangeWUm", true, DataSourceUpdateMode.OnPropertyChanged);
            this.repeatedH.DataBindings.Add("Value", settings, "InflateRangeHUm", true, DataSourceUpdateMode.OnPropertyChanged);
        }
    }
}
