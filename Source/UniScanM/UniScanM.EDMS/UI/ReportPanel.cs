using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.UI;
using DynMvp.UI.Touch;

using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniEye.Base;
using UniEye.Base.Data;

//using UniScanM.Data;
using System.Globalization;
using UniEye.Base.MachineInterface;
using UniScanM.EDMS.Data;
using UniScanM.UI;
using System.Windows.Forms.DataVisualization.Charting;
//using UniScanM.Data;

namespace UniScanM.EDMS.UI
{
    public partial class ReportPanel : UserControl, IUniScanMReportPanel, IMultiLanguageSupport
    {
        List<DirectoryInfo> findedDataList = new List<DirectoryInfo>();
        List<ProfilePanel> profilePanelList = new List<ProfilePanel>();

        ProfilePanel t100;
        ProfilePanel t101;
        ProfilePanel t102;

        ProfilePanel t103;
        ProfilePanel t104;
        ProfilePanel t105;
        ProfilePanel t134;

        public string AxisYFormat => "{F1}";

        public ReportPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Initialize();

            StringManager.AddListener(this);
        }

        public void Initialize()
        {
            t100 = new ProfilePanel(Data.DataType.FilmEdge);
            t101 = new ProfilePanel(Data.DataType.Coating_Film);
            t102 = new ProfilePanel(Data.DataType.Printing_Coating);
            t103 = new ProfilePanel(Data.DataType.FilmEdge_0);
            t104 = new ProfilePanel(Data.DataType.PrintingEdge_0);
            t105 = new ProfilePanel(Data.DataType.Printing_FilmEdge_0);
            t134 = new ProfilePanel(Data.DataType.FilmEdge0WithPrintingEdge0);

            profilePanelList.Add(t100);
            profilePanelList.Add(t101);
            profilePanelList.Add(t102);
            profilePanelList.Add(t103);
            profilePanelList.Add(t104);
            profilePanelList.Add(t105);
            profilePanelList.Add(t134);
        }

        private void ReportPage_Load(object sender, EventArgs e)
        {
            this.settingTabControl.SuspendLayout();
            this.SuspendLayout();

            tabPageT100.Controls.Add(t100);
            tabPageT101.Controls.Add(t101);
            tabPageT102.Controls.Add(t102);
            tabPageT103.Controls.Add(t103);
            tabPageT104.Controls.Add(t104);
            tabPageT105.Controls.Add(t105);
            tabPageT134.Controls.Add(t134);

            this.settingTabControl.ResumeLayout();
            this.ResumeLayout();
        }

        public bool ShowNgOnlyButton()
        {
            return false;
        }

        public void PageVisibleChanged(bool visibleFlag)
        {
            if (visibleFlag == true)
            {
                profilePanelList.ForEach(panel => panel.Initialize());
                profilePanelList.ForEach(panel => panel.ClearPanel());
            }
        }
        
        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }
        
        public void Clear()
        {
            profilePanelList.ForEach(panel => panel.ClearPanel());
        }
        public void Search(DynMvp.Data.ProductionBase production) { }

        private void settingTabControl_SelectedTabChanged(object sender, Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventArgs e)
        {

        }

        public void UpdateData(UniScanM.Data.DataImporter dataImporter, bool showNgOnly)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(UpdateData), dataImporter, showNgOnly);
                return;
            }

            List<DataPoint>[] dataLists = new List<DataPoint>[] {
                new List<DataPoint>(),new List<DataPoint>(),new List<DataPoint>(),new List<DataPoint>(),new List<DataPoint>(),new List<DataPoint>()};

            Clear();

            List<Data.InspectionResult> edmsInspectionResultList = new List<InspectionResult>(); ;
            if (dataImporter.InspectionResultList.Count > 0)
            {
                edmsInspectionResultList = dataImporter.InspectionResultList.ConvertAll(f => (Data.InspectionResult)f);
                if (showNgOnly)
                    edmsInspectionResultList.RemoveAll(f => f.IsGood());
            }

            for (int i = 0; i < edmsInspectionResultList.Count; i++)
            {
                InspectionResult edmsnspectionResult = edmsInspectionResultList[i];

                double[] resultArray = edmsnspectionResult.TotalEdgePositionResult;
                for (int j = 0; j < (int)DataType.ENUM_MAX-1; j++)
                {
                    dataLists[j].Add(new DataPoint(edmsnspectionResult.RollDistance, resultArray[j]));
                }
                //t100DataList.Add(new DataPoint(edmsnspectionResult.RollDistance, resultArray[(int)DataType.FilmEdge]));
                //t100DataList.Add(new DataPoint(edmsnspectionResult.RollDistance, resultArray[(int)DataType.Coating_Film]));
                //t100DataList.Add(new DataPoint(edmsnspectionResult.RollDistance, resultArray[(int)DataType.Printing_Coating]));
                //t100DataList.Add(new DataPoint(edmsnspectionResult.RollDistance, resultArray[(int)DataType.FilmEdge_0]));
                //t100DataList.Add(new DataPoint(edmsnspectionResult.RollDistance, resultArray[(int)DataType.PrintingEdge_0]));
                //t100DataList.Add(new DataPoint(edmsnspectionResult.RollDistance, resultArray[(int)DataType.Printing_FilmEdge_0]));
            }

            profilePanelList.ForEach(panel => panel.Initialize());
            profilePanelList.ForEach(panel => panel.ClearPanel());

            int k = 0;
            for (k = 0; k < (int)DataType.ENUM_MAX-1; k++)
            {
                dataLists[k] = dataLists[k].OrderBy((f) => f.XValue).ToList();
                profilePanelList[k].AddChartDataList(dataLists[k]);
            }
            profilePanelList[k].AddChartDataList2(dataLists[(int)(DataType.FilmEdge_0)], dataLists[(int)(DataType.PrintingEdge_0)]);

            //t100DataList.Sort((ChartData x, ChartData y) => x.Distance.CompareTo(y.Distance));
            //t101DataList.Sort((ChartData x, ChartData y) => x.Distance.CompareTo(y.Distance));
            //t102DataList.Sort((ChartData x, ChartData y) => x.Distance.CompareTo(y.Distance));
            //t103DataList.Sort((ChartData x, ChartData y) => x.Distance.CompareTo(y.Distance));
            //t104DataList.Sort((ChartData x, ChartData y) => x.Distance.CompareTo(y.Distance));
            //t105DataList.Sort((ChartData x, ChartData y) => x.Distance.CompareTo(y.Distance));

            //t100.AddChartDataList(t100DataList);
            //t101.AddChartDataList(t101DataList);
            //t102.AddChartDataList(t102DataList);
            //t103.AddChartDataList(t103DataList);
            //t104.AddChartDataList(t104DataList);
            //t105.AddChartDataList(t105DataList);

            profilePanelList.ForEach(panel => panel.DisplayResult(true));
        }

        public bool InitializeChart(Chart chart)
        {
            Font font = new Font("맑은 고딕", 12);

            chart.ChartAreas[0].AxisX.Title = StringManager.GetString(this.GetType().FullName, "Distance [m]");
            chart.ChartAreas[0].AxisY.Title = StringManager.GetString(this.GetType().FullName, "Film - Print Distance [um]");

            chart.ChartAreas[0].AxisX.TitleFont = font;
            chart.ChartAreas[0].AxisY.TitleFont = font;
            chart.Legends[0].Font = font;

            chart.Series.Clear();
            chart.Series.Add(new Series { Name = "graphdata", ChartType = SeriesChartType.Line, BorderWidth = 3, LegendText = "Distance", YAxisType = AxisType.Primary });

            Random rand = new Random();
            chart.Series["graphdata"].Points.Clear();
            for (int i = 0; i < 256; i++)
            {
                chart.Series["graphdata"].Points.Add(rand.Next(100));
            }

            bool useDefaultReportPanel = true;

            return useDefaultReportPanel;
        }
        
        public Dictionary<string, List<DataPoint>> GetChartData(int srcPos, int dstPos, UniScanM.Data.DataImporter dataImporter)
        {
            List<DataPoint> dpList = new List<DataPoint>();

            List<Data.InspectionResult> edmsInspectionResultList = dataImporter.InspectionResultList.ConvertAll(f => (Data.InspectionResult)f);
            int minDistance = srcPos;
            int maxDistance = dstPos;
            for (int i = minDistance; i <= maxDistance; i++)
            {
                List<InspectionResult> sameDistanceList = edmsInspectionResultList.FindAll(f => f.RollDistance == i);

                for (int j = 0; j < sameDistanceList.Count; j++)
                {
                    double newDistance = i + j * 1.0f / sameDistanceList.Count;
                    double gap = sameDistanceList[j].TotalEdgePositionResult[(int)DataType.Printing_Coating] + sameDistanceList[j].TotalEdgePositionResult[(int)DataType.Coating_Film];

                    DataPoint dp = new DataPoint(newDistance, gap);
                    dpList.Add(dp);
                }
            }

            Dictionary<string, List<DataPoint>> result = new Dictionary<string, List<DataPoint>>();
            result.Add("gapDistance", dpList);
            return result;
        }

        UniScanM.Data.DataImporter IUniScanMReportPanel.CreateDataImprter()
        {
            return new DataImporter();
        }
    }
}
