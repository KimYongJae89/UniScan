using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Inspect;
using DynMvp.InspData;
using UniScanG.Gravure.Data;
using UniScanG.Data;
using System.Collections.ObjectModel;
using DynMvp.Base;
using UniScanG.Module.Controller.Inspect;
using UniScanG.UI;

namespace UniScanG.Module.Controller.UI.Inspect
{
    public partial class MarginPanel : UserControl, IInspectExtraPanel, IMultiLanguageSupport
    {
        AlarmManager alarmManager = (AlarmManager)SystemManager.Instance().InspectRunnerG.AlarmManager;

        TextBox[] txts;
        public MarginPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Top;

            this.txts = new TextBox[]
            {
                txtFirstPatternNo,
                txtFirstW0,txtFirstH0,txtFirstW1,txtFirstH1,txtFirstW2,txtFirstH2,txtFirstW3, txtFirstH3,txtFirstW4,txtFirstH4,
                txtFirstWMin,txtFirstHMin,txtFirstWMax,txtFirstHMax,txtFirstWMean,txtFirstHMean,
                txtLastPatternNo,
                txtLastW0,txtLastH0,txtLastW1,txtLastH1,txtLastW2,txtLastH2,txtLastW3,txtLastH3,txtLastW4,txtLastH4,
                txtLastWMin,txtLastHMin,txtLastWMax,txtLastHMax,txtLastWMean,txtLastHMean,
            };

            SystemManager.Instance().ProductionManager.OnLotChanged += ProductionManager_OnLotChanged;
            //((UnitBaseInspectRunner)SystemManager.Instance().InspectRunnerG).AddInspectDoneDelegate(InspectDone);
            StringManager.AddListener(this);
        }

        private void ProductionManager_OnLotChanged()
        {
            Clear();
        }

        private void MarginPanel_Load(object sender, EventArgs e)
        {
            MarginPanelItem._Item firstItem = alarmManager.MarginPanelItemList.First;
            SetDataBinding(txtFirstPatternNo, new Binding("Text", firstItem, "PatternNo"), "D1");
            SetDataBinding(txtFirstW0, new Binding("Text", firstItem, "MarginSizeUm0.Width"), "F01");
            SetDataBinding(txtFirstH0, new Binding("Text", firstItem, "MarginSizeUm0.Height"), "F01");
            SetDataBinding(txtFirstW1, new Binding("Text", firstItem, "MarginSizeUm1.Width"), "F01");
            SetDataBinding(txtFirstH1, new Binding("Text", firstItem, "MarginSizeUm1.Height"), "F01");
            SetDataBinding(txtFirstW2, new Binding("Text", firstItem, "MarginSizeUm2.Width"), "F01");
            SetDataBinding(txtFirstH2, new Binding("Text", firstItem, "MarginSizeUm2.Height"), "F01");
            SetDataBinding(txtFirstW3, new Binding("Text", firstItem, "MarginSizeUm3.Width"), "F01");
            SetDataBinding(txtFirstH3, new Binding("Text", firstItem, "MarginSizeUm3.Height"), "F01");
            SetDataBinding(txtFirstW4, new Binding("Text", firstItem, "MarginSizeUm4.Width"), "F01");
            SetDataBinding(txtFirstH4, new Binding("Text", firstItem, "MarginSizeUm4.Height"), "F01");
            SetDataBinding(txtFirstWMin, new Binding("Text", firstItem, "Min.Width"), "F01");
            SetDataBinding(txtFirstHMin, new Binding("Text", firstItem, "Min.Height"), "F01");
            SetDataBinding(txtFirstWMax, new Binding("Text", firstItem, "Max.Width"), "F01");
            SetDataBinding(txtFirstHMax, new Binding("Text", firstItem, "Max.Height"), "F01");
            SetDataBinding(txtFirstWMean, new Binding("Text", firstItem, "Mean.Width"), "F02");
            SetDataBinding(txtFirstHMean, new Binding("Text", firstItem, "Mean.Height"), "F02");

            MarginPanelItem._Item lastItem = alarmManager.MarginPanelItemList.Last;
            SetDataBinding(txtLastPatternNo, new Binding("Text", lastItem, "PatternNo"), "D1");
            SetDataBinding(txtLastW0, new Binding("Text", lastItem, "MarginSizeUm0.Width"), "F01");
            SetDataBinding(txtLastH0, new Binding("Text", lastItem, "MarginSizeUm0.Height"), "F01");
            SetDataBinding(txtLastW1, new Binding("Text", lastItem, "MarginSizeUm1.Width"), "F01");
            SetDataBinding(txtLastH1, new Binding("Text", lastItem, "MarginSizeUm1.Height"), "F01");
            SetDataBinding(txtLastW2, new Binding("Text", lastItem, "MarginSizeUm2.Width"), "F01");
            SetDataBinding(txtLastH2, new Binding("Text", lastItem, "MarginSizeUm2.Height"), "F01");
            SetDataBinding(txtLastW3, new Binding("Text", lastItem, "MarginSizeUm3.Width"), "F01");
            SetDataBinding(txtLastH3, new Binding("Text", lastItem, "MarginSizeUm3.Height"), "F01");
            SetDataBinding(txtLastW4, new Binding("Text", lastItem, "MarginSizeUm4.Width"), "F01");
            SetDataBinding(txtLastH4, new Binding("Text", lastItem, "MarginSizeUm4.Height"), "F01");
            SetDataBinding(txtLastWMin, new Binding("Text", lastItem, "Min.Width"), "F01");
            SetDataBinding(txtLastHMin, new Binding("Text", lastItem, "Min.Height"), "F01");
            SetDataBinding(txtLastWMax, new Binding("Text", lastItem, "Max.Width"), "F01");
            SetDataBinding(txtLastHMax, new Binding("Text", lastItem, "Max.Height"), "F01");
            SetDataBinding(txtLastWMean, new Binding("Text", lastItem, "Mean.Width"), "F02");
            SetDataBinding(txtLastHMean, new Binding("Text", lastItem, "Mean.Height"), "F02");
        }

        private void SetDataBinding(TextBox textBox, Binding binding, string formatString)
        {
            binding.FormattingEnabled = true;
            binding.FormatString = formatString;
            textBox.DataBindings.Add(binding);
        }

        private void InspectDone(InspectionResult inspectionResult)
        {
            UpdateData();
        }

        private void Clear()
        {
            alarmManager.MarginPanelItemList.Clear();
            UpdateData();
        }

        private delegate void UpdateDataSourceDelegate();
        private void UpdateData()
        {
            if(this.InvokeRequired)
            {
                Invoke(new MethodInvoker(UpdateData));
                return;
            }
            Array.ForEach(this.txts, f =>f.DataBindings[0].ReadValue());
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        public void UpdateResult(InspectionResult inspectionResult)
        {
            UpdateData();
        }

        public void UpdateVisible()
        {
            this.Visible = true;
        }
    }
}
