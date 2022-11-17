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
using UniScanM.EDMSW.Data;
using UniScanM.UI;
using System.Windows.Forms.DataVisualization.Charting;
//using UniScanM.Data;

namespace UniScanM.EDMSW.UI
{
    public partial class ReportPanel : UserControl, IUniScanMReportPanel, IMultiLanguageSupport
    {
        List<DirectoryInfo> findedDataList = new List<DirectoryInfo>();
        List<IProfilePanel> profilePanelList = new List<IProfilePanel>();

        ProfilePanel t100;
        ProfilePanel t101;
        ProfilePanel t102;
        ProfilePanel t103;
        ProfilePanel t104;
        ProfilePanel t105;

        ProfilePanel t200;
        ProfilePanel t201;
        ProfilePanel t202;
        ProfilePanel t203;
        ProfilePanel t204;
        ProfilePanel t205;

        ProfilePanel_W W100;
        ProfilePanel_W W101;
        ProfilePanel_W W102;
        ProfilePanel_W L100;
        ProfilePanel_W L200;
        ProfilePanel_W LDIFF;

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

            t200 = new ProfilePanel(Data.DataType.FilmEdge);
            t201 = new ProfilePanel(Data.DataType.Coating_Film);
            t202 = new ProfilePanel(Data.DataType.Printing_Coating);
            t203 = new ProfilePanel(Data.DataType.FilmEdge_0);
            t204 = new ProfilePanel(Data.DataType.PrintingEdge_0);
            t205 = new ProfilePanel(Data.DataType.Printing_FilmEdge_0);

            W100 = new ProfilePanel_W(Data.DataType_Length.W100);
            W101 = new ProfilePanel_W(Data.DataType_Length.W101);
            W102 = new ProfilePanel_W(Data.DataType_Length.W102);
            L100 = new ProfilePanel_W(Data.DataType_Length.L100);
            L200 = new ProfilePanel_W(Data.DataType_Length.L200);
            LDIFF = new ProfilePanel_W(Data.DataType_Length.LDIFF);

            profilePanelList.Add(t100);
            profilePanelList.Add(t101);
            profilePanelList.Add(t102);
            profilePanelList.Add(t103);
            profilePanelList.Add(t104);
            profilePanelList.Add(t105);

            profilePanelList.Add(t200);
            profilePanelList.Add(t201);
            profilePanelList.Add(t202);
            profilePanelList.Add(t203);
            profilePanelList.Add(t204);
            profilePanelList.Add(t205);

            profilePanelList.Add(W100);
            profilePanelList.Add(W101);
            profilePanelList.Add(W102);
            profilePanelList.Add(L100);
            profilePanelList.Add(L200);
            profilePanelList.Add(LDIFF);

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

            tabPageT200.Controls.Add(t200);
            tabPageT201.Controls.Add(t201);
            tabPageT202.Controls.Add(t202);
            tabPageT203.Controls.Add(t203);
            tabPageT204.Controls.Add(t204);
            tabPageT205.Controls.Add(t205);

            tabPageW100.Controls.Add(W100);
            tabPageW101.Controls.Add(W101);
            tabPageW102.Controls.Add(W102);
            tabPageL100.Controls.Add(L100);
            tabPageL200.Controls.Add(L200);
            tabPageLDIFF.Controls.Add(LDIFF); 

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
                new List<DataPoint>(),
                new List<DataPoint>(),
                new List<DataPoint>(),
                new List<DataPoint>(),
                new List<DataPoint>(),
                new List<DataPoint>(),
            
                new List<DataPoint>(),
                new List<DataPoint>(),
                new List<DataPoint>(),
                new List<DataPoint>(),
                new List<DataPoint>(),
                new List<DataPoint>(),
            
                new List<DataPoint>(),
                new List<DataPoint>(),
                new List<DataPoint>(),
                new List<DataPoint>(),
                new List<DataPoint>(),
                new List<DataPoint>()};

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

                double[] resultArray = edmsnspectionResult.TotalEdgePositionResultLeft;  ////Left
                int j = 0;
                for (int c = 0 ; c < (int)DataType.ENUM_MAX; c++, j++)
                {
                    dataLists[j].Add(new DataPoint(edmsnspectionResult.RollDistance, resultArray[c]));
                }

                resultArray = edmsnspectionResult.TotalEdgePositionResultRight;  ////right는?
                for (int c = 0; c < (int)DataType.ENUM_MAX; c++, j++)
                {
                    dataLists[j].Add(new DataPoint(edmsnspectionResult.RollDistance, resultArray[c]));
                }

                resultArray = edmsnspectionResult.TotalLengthData;  ////right는?
                for ( int c = 0; c < (int)DataType_Length.ENUM_MAX; c++, j++)
                {
                    dataLists[j].Add(new DataPoint(edmsnspectionResult.RollDistance, resultArray[c]));
                }

            }

            profilePanelList.ForEach(panel => panel.Initialize());
            profilePanelList.ForEach(panel => panel.ClearPanel());

            int k = 0;
            for (k = 0; k < (int)(DataType.ENUM_MAX)*3; k++)
            {
                dataLists[k] = dataLists[k].OrderBy((f) => f.XValue).ToList();
                profilePanelList[k].AddChartDataList(dataLists[k]);
            }


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
                    double gap = 
                        sameDistanceList[j].TotalEdgePositionResultLeft[(int)DataType.Printing_Coating] + 
                        sameDistanceList[j].TotalEdgePositionResultLeft[(int)DataType.Coating_Film];

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
