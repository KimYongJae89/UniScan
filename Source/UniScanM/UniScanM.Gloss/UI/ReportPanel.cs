using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using UniScanM.Gloss.Data;
using UniScanM.Gloss.Settings;
using UniScanM.Gloss.UI.Chart;
using UniScanM.UI;

namespace UniScanM.Gloss.UI
{
    public partial class ReportPanel : UserControl, IUniScanMReportPanel, IMultiLanguageSupport
    {
        List<DirectoryInfo> findedDataList = new List<DirectoryInfo>();

        int rollIndex = -1;
        int rollPosition = -1;

        ChartSetting ProfileChartSetting = new ChartSetting();
        ChartSetting TrendChartSetting = new ChartSetting();
        ProfilePanel profilePanel;
        TrendPanel trendPanel;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public string AxisYFormat => "{F3}";

        public ReportPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            Initialize();

            GlossSettings.Instance().AdditionalSettingChangedDelegate += UpdatePanel;

            StringManager.AddListener(this);
        }

        public void Initialize()
        {
            ProfileChartSetting.UseLineStop = GlossSettings.Instance().UseProfileLineStop;
            ProfileChartSetting.LineStopRange = GlossSettings.Instance().ProfileLineStopRange;
            ProfileChartSetting.UseLineWarning = GlossSettings.Instance().UseProfileLineWarning;
            ProfileChartSetting.LineWarningRange = GlossSettings.Instance().ProfileLineWarningRange;
            ProfileChartSetting.XAxisInterval = GlossSettings.Instance().ProfileXAxisInterval;
            ProfileChartSetting.YAxisRange = GlossSettings.Instance().ProfileYAxisRange;
            ProfileChartSetting.YAxisInterval = GlossSettings.Instance().ProfileYAxisInterval;

            ProfileChartSetting.AxisColor = GlossSettings.Instance().AxisColor;
            ProfileChartSetting.BackColor = GlossSettings.Instance().BackColor;
            ProfileChartSetting.GraphColor = GlossSettings.Instance().GraphColor;
            ProfileChartSetting.SubGraphColor = GlossSettings.Instance().OverlayGraphColor;
            ProfileChartSetting.GraphThickness = GlossSettings.Instance().GraphThickness;
            ProfileChartSetting.LineStopColor = GlossSettings.Instance().LineStopColor;
            ProfileChartSetting.LineStopThickness = GlossSettings.Instance().LineStopThickness;
            ProfileChartSetting.LineWarningColor = GlossSettings.Instance().LineWarningColor;
            ProfileChartSetting.LineWarningThickness = GlossSettings.Instance().LineWarningThickness;
            ProfileChartSetting.LineCenterColor = GlossSettings.Instance().LineCenterColor;
            ProfileChartSetting.LineCenterThickness = GlossSettings.Instance().LineCenterThickness;
            ProfileChartSetting.OverlayCount = GlossSettings.Instance().OverlayCount;

            TrendChartSetting.UseLineStop = GlossSettings.Instance().UseTrendLineStop;
            TrendChartSetting.LineStopRange = GlossSettings.Instance().TrendLineStopRange;
            TrendChartSetting.UseLineWarning = GlossSettings.Instance().UseTrendLineWarning;
            TrendChartSetting.LineWarningRange = GlossSettings.Instance().TrendLineWarningRange;
            TrendChartSetting.XAxisInterval = GlossSettings.Instance().TrendXAxisInterval;
            TrendChartSetting.YAxisRange = GlossSettings.Instance().TrendYAxisRange;
            TrendChartSetting.YAxisInterval = GlossSettings.Instance().TrendYAxisInterval;

            TrendChartSetting.AxisColor = GlossSettings.Instance().AxisColor;
            TrendChartSetting.BackColor = GlossSettings.Instance().BackColor;
            TrendChartSetting.GraphColor = GlossSettings.Instance().GraphColor;
            TrendChartSetting.SubGraphColor = GlossSettings.Instance().TrendbarGraphColor;
            TrendChartSetting.GraphThickness = GlossSettings.Instance().GraphThickness;
            TrendChartSetting.LineStopColor = GlossSettings.Instance().LineStopColor;
            TrendChartSetting.LineStopThickness = GlossSettings.Instance().LineStopThickness;
            TrendChartSetting.LineWarningColor = GlossSettings.Instance().LineWarningColor;
            TrendChartSetting.LineWarningThickness = GlossSettings.Instance().LineWarningThickness;
            TrendChartSetting.LineCenterColor = GlossSettings.Instance().LineCenterColor;
            TrendChartSetting.LineCenterThickness = GlossSettings.Instance().LineCenterThickness;

            profilePanel = new ProfilePanel("Gloss", true, ProfileChartSetting, new ChartOption(false, false, true));
            trendPanel = new TrendPanel("Gloss", true, TrendChartSetting, new ChartOption(false, false, true));
        }

        private void ReportPage_Load(object sender, EventArgs e)
        {
            layoutRaw.Controls.Add(profilePanel, 1, 0);
            layoutRaw.Controls.Add(trendPanel, 1, 1);
        }

        public bool ShowNgOnlyButton()
        {
            return false;
        }

        private void chartTabControl_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {

        }
        public void PageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == true)
            {
                profilePanel.Initialize(ProfileChartSetting);
                trendPanel.Initialize(TrendChartSetting);
                profilePanel.ClearPanel();
                trendPanel.ClearPanel();
            }
        }

        private void UpdatePanel()
        {
            ProfileChartSetting.UseLineStop = GlossSettings.Instance().UseProfileLineStop;
            ProfileChartSetting.LineStopRange = GlossSettings.Instance().ProfileLineStopRange;
            ProfileChartSetting.UseLineWarning = GlossSettings.Instance().UseProfileLineWarning;
            ProfileChartSetting.LineWarningRange = GlossSettings.Instance().ProfileLineWarningRange;
            ProfileChartSetting.XAxisInterval = GlossSettings.Instance().ProfileXAxisInterval;
            ProfileChartSetting.YAxisRange = GlossSettings.Instance().ProfileYAxisRange;
            ProfileChartSetting.YAxisInterval = GlossSettings.Instance().ProfileYAxisInterval;

            ProfileChartSetting.AxisColor = GlossSettings.Instance().AxisColor;
            ProfileChartSetting.BackColor = GlossSettings.Instance().BackColor;
            ProfileChartSetting.GraphColor = GlossSettings.Instance().GraphColor;
            ProfileChartSetting.SubGraphColor = GlossSettings.Instance().OverlayGraphColor;
            ProfileChartSetting.GraphThickness = GlossSettings.Instance().GraphThickness;
            ProfileChartSetting.LineStopColor = GlossSettings.Instance().LineStopColor;
            ProfileChartSetting.LineStopThickness = GlossSettings.Instance().LineStopThickness;
            ProfileChartSetting.LineWarningColor = GlossSettings.Instance().LineWarningColor;
            ProfileChartSetting.LineWarningThickness = GlossSettings.Instance().LineWarningThickness;
            ProfileChartSetting.LineCenterColor = GlossSettings.Instance().LineCenterColor;
            ProfileChartSetting.LineCenterThickness = GlossSettings.Instance().LineCenterThickness;
            ProfileChartSetting.OverlayCount = GlossSettings.Instance().OverlayCount;

            TrendChartSetting.UseLineStop = GlossSettings.Instance().UseTrendLineStop;
            TrendChartSetting.LineStopRange = GlossSettings.Instance().TrendLineStopRange;
            TrendChartSetting.UseLineWarning = GlossSettings.Instance().UseTrendLineWarning;
            TrendChartSetting.LineWarningRange = GlossSettings.Instance().TrendLineWarningRange;
            TrendChartSetting.XAxisInterval = GlossSettings.Instance().TrendXAxisInterval;
            TrendChartSetting.YAxisRange = GlossSettings.Instance().TrendYAxisRange;
            TrendChartSetting.YAxisInterval = GlossSettings.Instance().TrendYAxisInterval;

            TrendChartSetting.AxisColor = GlossSettings.Instance().AxisColor;
            TrendChartSetting.BackColor = GlossSettings.Instance().BackColor;
            TrendChartSetting.GraphColor = GlossSettings.Instance().GraphColor;
            TrendChartSetting.SubGraphColor = GlossSettings.Instance().TrendbarGraphColor;
            TrendChartSetting.GraphThickness = GlossSettings.Instance().GraphThickness;
            TrendChartSetting.LineStopColor = GlossSettings.Instance().LineStopColor;
            TrendChartSetting.LineStopThickness = GlossSettings.Instance().LineStopThickness;
            TrendChartSetting.LineWarningColor = GlossSettings.Instance().LineWarningColor;
            TrendChartSetting.LineWarningThickness = GlossSettings.Instance().LineWarningThickness;
            TrendChartSetting.LineCenterColor = GlossSettings.Instance().LineCenterColor;
            TrendChartSetting.LineCenterThickness = GlossSettings.Instance().LineCenterThickness;

            profilePanel.Initialize(ProfileChartSetting);
            trendPanel.Initialize(TrendChartSetting);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public double[] GetChartData()
        {
            return new double[] { 1, 5, 30, 500 };
        }

        public void Clear()
        {
            profilePanel.Initialize(ProfileChartSetting);
            trendPanel.Initialize(TrendChartSetting);
            profilePanel.ClearPanel();
            trendPanel.ClearPanel();
        }

        public void Search(DynMvp.Data.ProductionBase production)
        {
            throw new NotImplementedException();
        }
        List<GlossScanData> GlossScanDatas = new List<GlossScanData>();

        public void UpdateData(UniScanM.Data.DataImporter dataImporter, bool showNgOnly)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(UpdateData), dataImporter, showNgOnly);
                return;
            }

            List<InspectionResult> inspectionResultList = new List<InspectionResult>();
            if (dataImporter.Count > 0)
            {
                inspectionResultList = dataImporter.InspectionResultList.ConvertAll(f => (InspectionResult)f);
                inspectionResultList = inspectionResultList.OrderBy((f) => f.InspectionStartTime).ToList();
                if (showNgOnly)
                    inspectionResultList.RemoveAll(f => f.IsGood());
            }

            //scandataListView.Rows.Clear();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Roll Pos");
            GlossScanDatas.Clear();
            //List<DataGridViewRow> dataGridViewRowList = new List<DataGridViewRow>();
            foreach (Gloss.Data.InspectionResult ir in inspectionResultList)
            {
                dataTable.Rows.Add(ir.GlossScanData.RollPosition);
                //DataGridViewRow dataGridViewRow = new DataGridViewRow();
                //DataGridViewTextBoxCell dataGridViewTextBoxCell = new DataGridViewTextBoxCell();
                //dataGridViewTextBoxCell.Value = ir.GlossScanData.RollPosition;
                //dataGridViewRow.Cells.Add(dataGridViewTextBoxCell);
                //dataGridViewRowList.Add(dataGridViewRow);

                GlossScanDatas.Add(ir.GlossScanData);
            }
            scandataListView.DataSource = dataTable;
            //scandataListView.Rows.AddRange(dataGridViewRowList.ToArray());

            UpdatePanel();

            profilePanel.AddScanDataList(GlossScanDatas);
            trendPanel.AddScanDataList(GlossScanDatas);

            profilePanel.DisplayResult(0);
            trendPanel.DisplayResult(0, 0);
        }

        public Dictionary<string, List<DataPoint>> GetChartData(int srcPos, int dstPos, UniScanM.Data.DataImporter dataImporter)
        {
            List<InspectionResult> inspectionResultList = dataImporter.InspectionResultList.ConvertAll(f => (InspectionResult)f);
            inspectionResultList.Sort((f, g) => f.InspectionStartTime.CompareTo(g.InspectionStartTime));
            if (inspectionResultList.Count == 0)
                return null;

            DateTime minDateTime = inspectionResultList[0].InspectionStartTime;

            List<DataPoint> dpList = new List<DataPoint>();
            foreach (InspectionResult inspectionResult in inspectionResultList)
            {
                foreach (var glossData in inspectionResult.GlossScanData.GlossDatas)
                {
                    DataPoint dp = new DataPoint(glossData.X, glossData.Y);
                    dpList.Add(dp);
                }
            }

            Dictionary<string, List<DataPoint>> result = new Dictionary<string, List<DataPoint>>();
            result.Add("Difference", dpList);
            return result;
        }

        public UniScanM.Data.DataImporter CreateDataImporter()
        {
            return new DataImporter();
        }

        public bool InitializeChart(System.Windows.Forms.DataVisualization.Charting.Chart chart)
        {
            //Font font = new Font("맑은 고딕", 12);

            //chart.ChartAreas[0].AxisX.Title = StringManager.GetString(this.GetType().FullName, "Distance [m]");
            //chart.ChartAreas[0].AxisY.Title = StringManager.GetString(this.GetType().FullName, "Difference [um]");

            //chart.ChartAreas[0].AxisX.TitleFont = font;
            //chart.ChartAreas[0].AxisY.TitleFont = font;
            //chart.Legends[0].Font = font;

            //chart.Series.Clear();
            //chart.Series.Add(new Series { Name = "graphdata", ChartType = SeriesChartType.Line, BorderWidth = 3, LegendText = "Difference", YAxisType = AxisType.Primary });

            //Random rand = new Random();
            //chart.Series["graphdata"].Points.Clear();
            //for (int i = 0; i < 256; i++)
            //{
            //    chart.Series["graphdata"].Points.Add(rand.Next(100));
            //}

            bool useDefaultReportPanel = false;

            return useDefaultReportPanel;
        }

        public UniScanM.Data.DataImporter CreateDataImprter()
        {
            return new DataImporter();
        }

        private void scandataListView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (scandataListView.RowCount == 0)
                return;

            if (scandataListView.SelectedCells[0].RowIndex > -1)
            {
                rollIndex = Convert.ToInt32(scandataListView.SelectedCells[0].RowIndex);
                rollPosition = Convert.ToInt32(scandataListView.SelectedCells[0].Value);
                List<GlossScanData> scanDataClone = GlossScanDatas.ToList();

                profilePanel.DisplayResult(rollIndex);
                trendPanel.DisplayResult(rollIndex, rollPosition);
            }
        }
    }
}

