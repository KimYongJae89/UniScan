using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Settings;
using UniScanM.Data;
using DynMvp.Base;
using System.IO;
using UniEye.Base.UI;
using DynMvp.Data.UI;
using DynMvp.UI;
using UniEye.Base;
using DynMvp.UI.Touch;
using UniScanM.UI.MenuPage;
using System.Diagnostics;
using DynMvp.Authentication;
using System.Windows.Forms.DataVisualization.Charting;

namespace UniScanM.UI
{
    public partial class ReportPage : UserControl, IReportPage, IMultiLanguageSupport
    {
        List<DirectoryInfo> findedDataList = new List<DirectoryInfo>();
        List<string> pathList = new List<string>();

        IUniScanMReportPanel reportPanel;
        ReportPageController reportPageController;
        DataImporter lastDataImporter = null;
        
        public ReportPage()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            reportPageController = SystemManager.Instance().UiChanger.CreateReportPageController();
            reportPageController.Initialize(lotNoList);

            reportPanel = SystemManager.Instance().UiChanger.CreateReportPanel() as IUniScanMReportPanel;
            panelDetail.Controls.Add((Control)reportPanel);

            this.showNgOnly.Visible = reportPanel.ShowNgOnlyButton();
            StringManager.AddListener(this);

            this.Clear();
        }

        private void ReportPanel_Load(object sender, EventArgs e)
        {
            startDate.Format = DateTimePickerFormat.Custom;
            startDate.CustomFormat = "yyyy. MM. dd.";
            startDate.Value = DateTime.Now.Date - new TimeSpan(1, 0, 0, 0);
            endDate.Format = DateTimePickerFormat.Custom;
            endDate.CustomFormat = "yyyy. MM. dd.";
            endDate.Value = DateTime.Now.Date;
        }

        private void Date_ValueChanged(object sender, EventArgs e)
        {
            if (endDate.Value < startDate.Value)
            {
                endDate.Value = startDate.Value;
                return;
            }

            Search();
        }

        private void lot_TextChanged(object sender, EventArgs e)
        {
            string upper = lot.Text.ToUpper();
            if (lot.Text != upper)
            {
                lot.Text = upper;
                lot.SelectionStart = lot.Text.Length;
            }
            else
                Search();
        }

        void Search()
        {
            DateTime startTime = new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day, 0, 0, 0);
            DateTime endTime = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 23, 59, 59);
            string[] format = { "yyyyMMdd" };

            ProductionManager productionManager = SystemManager.Instance().ProductionManager;
            reportPageController.Search(startTime, endTime, lot.Text, lotNoList);
        }

        private delegate void UpdateChartDelegate(Dictionary<string, List<DataPoint>> dictionary);
        private void UpdateChart(Dictionary<string, List<DataPoint>> dictionary)
        {
            if(InvokeRequired)
            {
                Invoke(new UpdateChartDelegate(UpdateChart), dictionary);
                return;
            }

            if (dictionary == null)
                return;

            //chart1.Series.Clear();
            double minDist = double.MaxValue, maxDist = 0;
            double minValue = double.MaxValue, maxValue = 0;

            for (int i = 0; i < dictionary.Count; i++)
            {
                KeyValuePair<string, List<DataPoint>> pair = dictionary.ElementAt(i);
                Series series = chart1.Series[i];
                series.Points.Clear();
                series.LegendText = pair.Key;

                pair.Value.ForEach(f => series.Points.Add(f));
                if (pair.Value.Count > 0)
                {
                    minDist = Math.Min(minDist, pair.Value.Min(f => f.XValue));
                    maxDist = Math.Max(maxDist, pair.Value.Max(f => f.XValue));
                    minValue = Math.Min(minValue, pair.Value.Min(f => f.YValues.Min()));
                    maxValue = Math.Max(maxValue, pair.Value.Max(f => f.YValues.Max()));
                }
            }

            chart1.ChartAreas[0].AxisX.Minimum = minDist;
            chart1.ChartAreas[0].AxisX.Maximum = maxDist;
            double diffX = maxDist - minDist;
            if (diffX < 10)
                chart1.ChartAreas[0].AxisX.Interval = 1;
            else
                chart1.ChartAreas[0].AxisX.Interval = diffX / 10;

            double diffY = maxValue - minValue;
            if (diffY == 0)
                maxValue = minValue + 1;
            chart1.ChartAreas[0].AxisY.Minimum = minValue - Math.Abs(minValue) * 0.1;
            chart1.ChartAreas[0].AxisY.Maximum = maxValue + Math.Abs(minValue) * 0.1;
         

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "{F1}";
            chart1.ChartAreas[0].AxisY.LabelStyle.Format = reportPanel.AxisYFormat;
        }

        public void Initialize() { }
        public void ModelAutoSelector() { }
        public void Clear()
        {
            reportPanel?.Clear();
            for (int i = 0; i < chart1.Series.Count; i++)
                chart1.Series[i].Points.Clear();
        }

        public void EnableControls(UserType user) { }
        public void UpdateControl(string item, object value) { }
        public void PageVisibleChanged(bool visibleFlag)
        {
            this.Visible = visibleFlag;
            if (visibleFlag == true)
                Search();
        }

        private void buttonOpenExel_Click(object sender, EventArgs e)
        {
            if (lotNoList.SelectedRows.Count == 0)
                return;

            if (lotNoList.SelectedRows[0].Tag == null)
                return;

            string tagString = ((UniScanM.Data.Production)lotNoList.SelectedRows[0].Tag).GetResultPath();
            string reportPath = Path.Combine(tagString.Replace("Result", "Report"), "Report.xlsx");

            if (File.Exists(reportPath)==false)
            {
                MessageForm.Show(null, string.Format(StringManager.GetString("The report File [{0}] is not exist."), reportPath));
                return;
            }
            Process.Start(reportPath);
        }

        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            if (lotNoList.SelectedRows.Count == 0)
                return;

            if (lotNoList.SelectedRows[0].Tag == null)
                return;

            string tagString = ((UniScanM.Data.Production)lotNoList.SelectedRows[0].Tag).GetResultPath();
            string reportPath = Path.Combine(tagString.Replace("Result", "Report"));
            
            if (Directory.Exists(reportPath) == false)
            {
                MessageForm.Show(null, string.Format(StringManager.GetString("The report Folder [{0}] is not exist."), reportPath));
                return;
            }

            Process.Start(reportPath);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            if (reportPanel?.InitializeChart(chart1) == false)
                tableChart.RowStyles[1].Height = 0;
        }

        private void lotNoList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void lotNoList_KeyDown(object sender, KeyEventArgs e)
        {
            if (lotNoList.SelectedRows.Count == 0)
                return;

            Production production = lotNoList.SelectedRows[0]?.Tag as Production;
            if (production == null)
                return;


            if (e.KeyData == (Keys.Control | Keys.Delete))
            {
                e.Handled = true;
                DialogResult dialogResult = MessageForm.Show(null, string.Format(StringManager.GetString(this.GetType().FullName, "Do you want to REMOVE selected result [{0}]?"), production.LotNo), MessageFormType.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SystemManager.Instance().ProductionManager.RemoveProduction(production);
                    Search();
                }
            }
        }

        private void showNgOnly_CheckedChanged(object sender, EventArgs e)
        {
            if (lastDataImporter == null)
                return;

            if (lastDataImporter.Count > 0)
                reportPanel.UpdateData(lastDataImporter, showNgOnly.Checked);
        }

        private void lotNoList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (lotNoList.RowCount == 0)
                return;

            DataGridViewRow row = lotNoList.SelectedRows[0];
            if (row.Tag == null)
                return;

            Production production = row.Tag as Production;
            if (production == null)
                return;

            string resultPath = production.GetResultPath();

            Clear();
            lastDataImporter = reportPanel.CreateDataImprter();
            bool ok = lastDataImporter.Import(production);
            Debug.WriteLine(string.Format("Load Result {0}", resultPath));
            if (ok == false)
            {
                MessageForm.Show(null, StringManager.GetString("Can not find Result File."));
                return;
            }

            if (lastDataImporter.Count == 0)
                MessageForm.Show(null, StringManager.GetString("There is no NG Data."));

            //SimpleProgressForm spf = new SimpleProgressForm();
            //spf.Show(() =>
            //{
            reportPageController.BuildRowData(row, production);
            reportPanel.UpdateData(lastDataImporter, showNgOnly.Checked);
            this.UpdateChart(reportPanel.GetChartData(production.StartPosition, production.EndPosition, lastDataImporter));
            //});
        }
    }
}
