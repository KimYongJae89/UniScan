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

using Microsoft.VisualBasic.FileIO;

using DynMvp.Base;
using DynMvp.Data.UI;
using DynMvp.UI;
using DynMvp.UI.Touch;

using UniEye.Base.Settings;
using UniEye.Base.UI;
using UniEye.Base;
using UniEye.Base.Data;

//using UniScanM.Data;
using UniScanM.CEDMS.UI.Chart;
using UniScanM.CEDMS.Settings;
using System.Globalization;
using UniScanM.CEDMS.Data;
using DynMvp.Data;
using UniEye.Base.MachineInterface;
using UniScanM.UI;
using System.Windows.Forms.DataVisualization.Charting;

namespace UniScanM.CEDMS.UI
{
    public partial class ReportPanel : UserControl, IUniScanMReportPanel, IMultiLanguageSupport
    {
        List<DirectoryInfo> findedDataList = new List<DirectoryInfo>();

        ChartSetting inFeedChartSetting = new ChartSetting();
        ChartSetting outFeedChartSetting = new ChartSetting();

        List<ProfilePanel> infeedProfilePanelList = new List<ProfilePanel>();
        List<ProfilePanel> outfeedProfilePanelList = new List<ProfilePanel>();

        ProfilePanel rawInFeed;
        ProfilePanel rawOutFeed;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }
        
        public string AxisYFormat => "{F3}";

        public ReportPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            Initialize();

            CEDMSSettings.Instance().AdditionalSettingChangedDelegate += UpdatePanel;

            StringManager.AddListener(this);
        }

        public void Initialize()
        {
            inFeedChartSetting.UseLineStop = CEDMSSettings.Instance().InFeedUseLineStop;
            inFeedChartSetting.LineStopLower = CEDMSSettings.Instance().InFeedLineStopLower;
            inFeedChartSetting.LineStopUpper = CEDMSSettings.Instance().InFeedLineStopUpper;
            inFeedChartSetting.UseLineWarning = CEDMSSettings.Instance().InFeedUseLineWarning;
            inFeedChartSetting.LineWarningLower = CEDMSSettings.Instance().InFeedLineWarningLower;
            inFeedChartSetting.LineWarningUpper = CEDMSSettings.Instance().InFeedLineWarningUpper;
            inFeedChartSetting.XAxisDisplayLength = CEDMSSettings.Instance().InFeedXAxisDisplayLength;
            inFeedChartSetting.XAxisDisplayTotalLength = CEDMSSettings.Instance().InFeedXAxisDisplayTotalLength;
            inFeedChartSetting.XAxisInterval = CEDMSSettings.Instance().InFeedXAxisInterval;
            inFeedChartSetting.YAxisRange = CEDMSSettings.Instance().InFeedYAxisRange;
            inFeedChartSetting.YAxisUnit = CEDMSSettings.Instance().InFeedYAxisUnit;
            inFeedChartSetting.YAxisInterval = CEDMSSettings.Instance().InFeedYAxisInterval;
            inFeedChartSetting.UseTotalCenterLine = CEDMSSettings.Instance().InFeedUseTotalCenterLine;

            outFeedChartSetting.UseLineStop = CEDMSSettings.Instance().OutFeedUseLineStop;
            outFeedChartSetting.LineStopLower = CEDMSSettings.Instance().OutFeedLineStopLower;
            outFeedChartSetting.LineStopUpper = CEDMSSettings.Instance().OutFeedLineStopUpper;
            outFeedChartSetting.UseLineWarning = CEDMSSettings.Instance().OutFeedUseLineWarning;
            outFeedChartSetting.LineWarningLower = CEDMSSettings.Instance().OutFeedLineWarningLower;
            outFeedChartSetting.LineWarningUpper = CEDMSSettings.Instance().OutFeedLineWarningUpper;
            outFeedChartSetting.XAxisDisplayLength = CEDMSSettings.Instance().OutFeedXAxisDisplayLength;
            outFeedChartSetting.XAxisDisplayTotalLength = CEDMSSettings.Instance().OutFeedXAxisDisplayTotalLength;
            outFeedChartSetting.XAxisInterval = CEDMSSettings.Instance().OutFeedXAxisInterval;
            outFeedChartSetting.YAxisRange = CEDMSSettings.Instance().OutFeedYAxisRange;
            outFeedChartSetting.YAxisUnit = CEDMSSettings.Instance().OutFeedYAxisUnit;
            outFeedChartSetting.YAxisInterval = CEDMSSettings.Instance().OutFeedYAxisInterval;
            outFeedChartSetting.UseTotalCenterLine = CEDMSSettings.Instance().OutFeedUseTotalCenterLine;

            inFeedChartSetting.AxisColor = CEDMSSettings.Instance().AxisColor;
            inFeedChartSetting.BackColor = CEDMSSettings.Instance().BackColor;
            inFeedChartSetting.GraphColor = CEDMSSettings.Instance().GraphColor;
            inFeedChartSetting.GraphThickness = CEDMSSettings.Instance().GraphThickness;
            inFeedChartSetting.LineStopColor = CEDMSSettings.Instance().LineStopColor;
            inFeedChartSetting.LineStopThickness = CEDMSSettings.Instance().LineStopThickness;
            inFeedChartSetting.LineWarningColor = CEDMSSettings.Instance().LineWarningColor;
            inFeedChartSetting.LineWarningThickness = CEDMSSettings.Instance().LineWarningThickness;
            inFeedChartSetting.LineTotalGraphCenterColor = CEDMSSettings.Instance().LineTotalGraphCenterColor;
            inFeedChartSetting.LineTotalGraphCenterThickness = CEDMSSettings.Instance().LineTotalGraphCenterThickness;

            outFeedChartSetting.AxisColor = CEDMSSettings.Instance().AxisColor;
            outFeedChartSetting.BackColor = CEDMSSettings.Instance().BackColor;
            outFeedChartSetting.GraphColor = CEDMSSettings.Instance().GraphColor;
            outFeedChartSetting.GraphThickness = CEDMSSettings.Instance().GraphThickness;
            outFeedChartSetting.LineStopColor = CEDMSSettings.Instance().LineStopColor;
            outFeedChartSetting.LineStopThickness = CEDMSSettings.Instance().LineStopThickness;
            outFeedChartSetting.LineWarningColor = CEDMSSettings.Instance().LineWarningColor;
            outFeedChartSetting.LineWarningThickness = CEDMSSettings.Instance().LineWarningThickness;
            outFeedChartSetting.LineTotalGraphCenterColor = CEDMSSettings.Instance().LineTotalGraphCenterColor;
            outFeedChartSetting.LineTotalGraphCenterThickness = CEDMSSettings.Instance().LineTotalGraphCenterThickness;


            rawInFeed = new ProfilePanel("In Feed", true, true, inFeedChartSetting, new ProfileOption(false, false, true));
            rawOutFeed = new ProfilePanel("Out Feed", true, true, outFeedChartSetting, new ProfileOption(false, false, true));

            infeedProfilePanelList.Add(rawInFeed);
            outfeedProfilePanelList.Add(rawOutFeed);
        }

        private void ReportPage_Load(object sender, EventArgs e)
        {
            //layoutZeroing.Controls.Add(zeroingInFeed, 0, 0);
            //layoutZeroing.Controls.Add(zeroingOutFeed, 1, 0);

            layoutRaw.Controls.Add(rawInFeed, 0, 0);
            layoutRaw.Controls.Add(rawOutFeed, 1, 0);
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
                infeedProfilePanelList.ForEach(panel => panel.Initialize(inFeedChartSetting));
                outfeedProfilePanelList.ForEach(panel => panel.Initialize(outFeedChartSetting));
                infeedProfilePanelList.ForEach(panel => panel.ClearPanel());
                outfeedProfilePanelList.ForEach(panel => panel.ClearPanel());
            }
        }

        private void UpdatePanel()
        {
            inFeedChartSetting.UseLineStop = CEDMSSettings.Instance().InFeedUseLineStop;
            inFeedChartSetting.LineStopLower = CEDMSSettings.Instance().InFeedLineStopLower;
            inFeedChartSetting.LineStopUpper = CEDMSSettings.Instance().InFeedLineStopUpper;
            inFeedChartSetting.UseLineWarning = CEDMSSettings.Instance().InFeedUseLineWarning;
            inFeedChartSetting.LineWarningLower = CEDMSSettings.Instance().InFeedLineWarningLower;
            inFeedChartSetting.LineWarningUpper = CEDMSSettings.Instance().InFeedLineWarningUpper;
            inFeedChartSetting.XAxisDisplayLength = CEDMSSettings.Instance().InFeedXAxisDisplayLength;
            inFeedChartSetting.XAxisDisplayTotalLength = CEDMSSettings.Instance().InFeedXAxisDisplayTotalLength;
            inFeedChartSetting.XAxisInterval = CEDMSSettings.Instance().InFeedXAxisInterval;
            inFeedChartSetting.YAxisRange = CEDMSSettings.Instance().InFeedYAxisRange;
            inFeedChartSetting.YAxisUnit = CEDMSSettings.Instance().InFeedYAxisUnit;
            inFeedChartSetting.YAxisInterval = CEDMSSettings.Instance().InFeedYAxisInterval;
            inFeedChartSetting.UseTotalCenterLine = CEDMSSettings.Instance().InFeedUseTotalCenterLine;

            outFeedChartSetting.UseLineStop = CEDMSSettings.Instance().OutFeedUseLineStop;
            outFeedChartSetting.LineStopLower = CEDMSSettings.Instance().OutFeedLineStopLower;
            outFeedChartSetting.LineStopUpper = CEDMSSettings.Instance().OutFeedLineStopUpper;
            outFeedChartSetting.UseLineWarning = CEDMSSettings.Instance().OutFeedUseLineWarning;
            outFeedChartSetting.LineWarningLower = CEDMSSettings.Instance().OutFeedLineWarningLower;
            outFeedChartSetting.LineWarningUpper = CEDMSSettings.Instance().OutFeedLineWarningUpper;
            outFeedChartSetting.XAxisDisplayLength = CEDMSSettings.Instance().OutFeedXAxisDisplayLength;
            outFeedChartSetting.XAxisDisplayTotalLength = CEDMSSettings.Instance().OutFeedXAxisDisplayTotalLength;
            outFeedChartSetting.XAxisInterval = CEDMSSettings.Instance().OutFeedXAxisInterval;
            outFeedChartSetting.YAxisRange = CEDMSSettings.Instance().OutFeedYAxisRange;
            outFeedChartSetting.YAxisUnit = CEDMSSettings.Instance().OutFeedYAxisUnit;
            outFeedChartSetting.YAxisInterval = CEDMSSettings.Instance().OutFeedYAxisInterval;
            outFeedChartSetting.UseTotalCenterLine = CEDMSSettings.Instance().OutFeedUseTotalCenterLine;

            inFeedChartSetting.AxisColor = CEDMSSettings.Instance().AxisColor;
            inFeedChartSetting.BackColor = CEDMSSettings.Instance().BackColor;
            inFeedChartSetting.GraphColor = CEDMSSettings.Instance().GraphColor;
            inFeedChartSetting.GraphThickness = CEDMSSettings.Instance().GraphThickness;
            inFeedChartSetting.LineStopColor = CEDMSSettings.Instance().LineStopColor;
            inFeedChartSetting.LineStopThickness = CEDMSSettings.Instance().LineStopThickness;
            inFeedChartSetting.LineWarningColor = CEDMSSettings.Instance().LineWarningColor;
            inFeedChartSetting.LineWarningThickness = CEDMSSettings.Instance().LineWarningThickness;
            inFeedChartSetting.LineTotalGraphCenterColor = CEDMSSettings.Instance().LineTotalGraphCenterColor;
            inFeedChartSetting.LineTotalGraphCenterThickness = CEDMSSettings.Instance().LineTotalGraphCenterThickness;

            outFeedChartSetting.AxisColor = CEDMSSettings.Instance().AxisColor;
            outFeedChartSetting.BackColor = CEDMSSettings.Instance().BackColor;
            outFeedChartSetting.GraphColor = CEDMSSettings.Instance().GraphColor;
            outFeedChartSetting.GraphThickness = CEDMSSettings.Instance().GraphThickness;
            outFeedChartSetting.LineStopColor = CEDMSSettings.Instance().LineStopColor;
            outFeedChartSetting.LineStopThickness = CEDMSSettings.Instance().LineStopThickness;
            outFeedChartSetting.LineWarningColor = CEDMSSettings.Instance().LineWarningColor;
            outFeedChartSetting.LineWarningThickness = CEDMSSettings.Instance().LineWarningThickness;
            outFeedChartSetting.LineTotalGraphCenterColor = CEDMSSettings.Instance().LineTotalGraphCenterColor;
            outFeedChartSetting.LineTotalGraphCenterThickness = CEDMSSettings.Instance().LineTotalGraphCenterThickness;

            infeedProfilePanelList.ForEach(panel => panel.Initialize(inFeedChartSetting));
            outfeedProfilePanelList.ForEach(panel => panel.Initialize(outFeedChartSetting));
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
            infeedProfilePanelList.ForEach(panel => panel.Initialize(inFeedChartSetting));
            infeedProfilePanelList.ForEach(panel => panel.ClearPanel());

            outfeedProfilePanelList.ForEach(panel => panel.Initialize(outFeedChartSetting));
            outfeedProfilePanelList.ForEach(panel => panel.ClearPanel());
        }

        public void Search(DynMvp.Data.ProductionBase production)
        {
            throw new NotImplementedException();
        }

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

            List<CEDMSScanData> zeroingInFeedDataList = new List<CEDMSScanData>(); //inspectionResultList.ConvertAll(f => f.GearSide);
            List<CEDMSScanData> zeroingOutFeedDataList = new List<CEDMSScanData>(); //inspectionResultList.ConvertAll(f => f.ManSide);

            List<CEDMSScanData> rawInFeedDataList = new List<CEDMSScanData>(); //new List<ScanData>(zeroingInFeedDataList);
            List<CEDMSScanData> rawOutFeedDataList = new List<CEDMSScanData>(); //new List<ScanData>(zeroingOutFeedDataList);

            foreach (InspectionResult ir in inspectionResultList)
            {
                int rollDistance = ir.RollDistance;

                //zeroingInFeedDataList.Add(new CEDMSScanData(rollDistance, ir.InFeed.Y, 0, 0));
                //zeroingOutFeedDataList.Add(new CEDMSScanData(rollDistance, ir.OutFeed.Y, 0, 0));

                rawInFeedDataList.Add(new CEDMSScanData(rollDistance, 0, ir.InFeed.YRaw, 0));
                rawOutFeedDataList.Add(new CEDMSScanData(rollDistance, 0, ir.OutFeed.YRaw, 0));
            }

            infeedProfilePanelList.ForEach(panel => panel.Initialize(inFeedChartSetting));
            infeedProfilePanelList.ForEach(panel => panel.ClearPanel());

            outfeedProfilePanelList.ForEach(panel => panel.Initialize(outFeedChartSetting));
            outfeedProfilePanelList.ForEach(panel => panel.ClearPanel());

            //zeroingInFeed.AddScanDataList(zeroingInFeedDataList);
            //zeroingOutFeed.AddScanDataList(zeroingOutFeedDataList);

            rawInFeed.AddScanDataList(rawInFeedDataList);
            rawOutFeed.AddScanDataList(rawOutFeedDataList);

            infeedProfilePanelList.ForEach(panel => panel.DisplayResult());
            outfeedProfilePanelList.ForEach(panel => panel.DisplayResult());
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
                double distance = inspectionResult.RollDistance;
                //double timeSpan = (inspectionResult.InspectionStartTime - minDateTime).TotalSeconds;
                double value = Math.Abs(inspectionResult.InFeed.YRaw - inspectionResult.OutFeed.YRaw);
                DataPoint dp = new DataPoint(distance, value);
                dpList.Add(dp);
            }

            Dictionary<string, List<DataPoint>> result = new Dictionary<string, List<DataPoint>>();
            result.Add("Difference", dpList);
            return result;
        }

        public UniScanM.Data.DataImporter CreateDataImprter()
        {
            return new DataImporter();
        }

        public bool InitializeChart(System.Windows.Forms.DataVisualization.Charting.Chart chart)
        {
            Font font = new Font("맑은 고딕", 12);

            chart.ChartAreas[0].AxisX.Title = StringManager.GetString(this.GetType().FullName, "Distance [m]");
            chart.ChartAreas[0].AxisY.Title = StringManager.GetString(this.GetType().FullName, "Difference [um]");

            chart.ChartAreas[0].AxisX.TitleFont = font;
            chart.ChartAreas[0].AxisY.TitleFont = font;
            chart.Legends[0].Font = font;

            chart.Series.Clear();
            chart.Series.Add(new Series { Name = "graphdata", ChartType = SeriesChartType.Line, BorderWidth = 3, LegendText = "Difference", YAxisType = AxisType.Primary });

            Random rand = new Random();
            chart.Series["graphdata"].Points.Clear();
            for (int i = 0; i < 256; i++)
            {
                chart.Series["graphdata"].Points.Add(rand.Next(100));
            }

            bool useDefaultReportPanel = true;

            return useDefaultReportPanel;
        }
    }
}

