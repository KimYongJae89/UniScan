using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Base;

namespace UniScanG.Module.Controller.UI.Settings
{
    public partial class SettingGradeControl : UserControl, IMultiLanguageSupport
    {
        Gravure.Settings.GradeSetting grade;

        public SettingGradeControl(Gravure.Settings.GradeSetting grade)
        {
            InitializeComponent();

            this.grade = grade;
            StringManager.AddListener(this);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void SettingGradeControl_Load(object sender, EventArgs e)
        {
            this.use.DataBindings.Add(new Binding("Checked", this.grade, "Use"));

            this.gradeATo.DataBindings.Add(new Binding("Value", this.grade, "ScoreA", false, DataSourceUpdateMode.OnPropertyChanged));
            this.gradeBFrom.DataBindings.Add(new Binding("Value", this.grade, "ScoreA", false, DataSourceUpdateMode.OnPropertyChanged));
            this.labelGradeA.DataBindings.Add(new Binding("ForeColor", this.grade, "ColorA", false, DataSourceUpdateMode.OnPropertyChanged));

            this.gradeBTo.DataBindings.Add(new Binding("Value", this.grade, "ScoreB", false, DataSourceUpdateMode.OnPropertyChanged));
            this.gradeCFrom.DataBindings.Add(new Binding("Value", this.grade, "ScoreB", false, DataSourceUpdateMode.OnPropertyChanged));
            this.labelGradeB.DataBindings.Add(new Binding("ForeColor", this.grade, "ColorB", false, DataSourceUpdateMode.OnPropertyChanged));

            this.gradeCTo.DataBindings.Add(new Binding("Value", this.grade, "ScoreC", false, DataSourceUpdateMode.OnPropertyChanged));
            this.gradeDFrom.DataBindings.Add(new Binding("Value", this.grade, "ScoreC", false, DataSourceUpdateMode.OnPropertyChanged));
            this.labelGradeC.DataBindings.Add(new Binding("ForeColor", this.grade, "ColorC", false, DataSourceUpdateMode.OnPropertyChanged));

            this.labelGradeD.DataBindings.Add(new Binding("ForeColor", this.grade, "ColorD", false, DataSourceUpdateMode.OnPropertyChanged));
        }

        private void color_DoubleClick(object sender, EventArgs e)
        {

        }
    }
}
