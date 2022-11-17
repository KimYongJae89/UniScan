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
using UniScanM.RVMS.UI.Chart;
using UniScanM.RVMS.Settings;
using System.Globalization;
using UniScanM.RVMS.Data;
using DynMvp.Data;
using UniEye.Base.MachineInterface;
using UniScanM.UI;
using System.Windows.Forms.DataVisualization.Charting;

namespace UniScanM.RVMS.UI
{
    public partial class ReportPanel : UserControl, IUniScanMReportPanel, IMultiLanguageSupport
    {
        List<DirectoryInfo> findedDataList = new List<DirectoryInfo>();

        RVMSSettings setting;

        List<ProfilePanel> profilePanelList = new List<ProfilePanel>();
        ProfilePanel zeroingGear;
        ProfilePanel zeroingMan;

        ProfilePanel rawGear;
        ProfilePanel rawMan;

        ProfilePanel beforePattern;
        ProfilePanel afterPattern;

        Control showHideControl;
        public Control ShowHideControl { get => showHideControl; set => showHideControl = value; }

        public string AxisYFormat => "{F3}";

        public ReportPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            Initialize();

            StringManager.AddListener(this);
        }

        public void Initialize()
        {
            zeroingGear = new ProfilePanel("Zeroing Gear Side", true, false, true, null, new ProfileOption(false, false, false));
            zeroingMan = new ProfilePanel("Zeroing Man Side", true, false, true, null, new ProfileOption(false, false, false));

            rawGear = new ProfilePanel("Raw Gear Side", true, false, true, null, new ProfileOption(false, false, true));
            rawMan = new ProfilePanel("Raw Man Side", true, false, true, null, new ProfileOption(false, false, true));

            beforePattern = new ProfilePanel("Before", true, true, true, null, new ProfileOption(false, false, true));
            afterPattern = new ProfilePanel("After", true, true, true, null, new ProfileOption(false, false, true));

            profilePanelList.Add(zeroingGear);
            profilePanelList.Add(zeroingMan);
            profilePanelList.Add(rawGear);
            profilePanelList.Add(rawMan);
            profilePanelList.Add(beforePattern);
            profilePanelList.Add(afterPattern);
        }

        private void ReportPage_Load(object sender, EventArgs e)
        {
            layoutZeroing.Controls.Add(zeroingGear, 0, 0);
            layoutZeroing.Controls.Add(zeroingMan, 1, 0);

            layoutRaw.Controls.Add(rawGear, 0, 0);
            layoutRaw.Controls.Add(rawMan, 1, 0);

            layoutPattern.Controls.Add(beforePattern, 0, 0);
            layoutPattern.Controls.Add(afterPattern, 1, 0);
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
                profilePanelList.ForEach(panel => panel.Initialize(setting));
                profilePanelList.ForEach(panel => panel.ClearPanel());
            }
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        delegate void UpdateDataDelegate(string path);
        public void UpdateData(string path)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(UpdateData), path);
                return;
            }

            string filePath = Path.Combine(path, "Result.csv");

            if (File.Exists(filePath) == false)
                return;

            string[] lines = File.ReadAllLines(filePath);

            lines = lines.Skip(Math.Min(lines.Length, 5)).ToArray();

            List<ScanData> zeroingGearDataList = new List<ScanData>();
            List<ScanData> zeroingManDataList = new List<ScanData>();

            List<ScanData> rawGearDataList = new List<ScanData>();
            List<ScanData> rawManDataList = new List<ScanData>();

            List<ScanData> beforePatternDataList = new List<ScanData>();
            List<ScanData> afterPatternDataList = new List<ScanData>();

            string format = "yyyyMMdd.HHmmss.fff";
            MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            foreach (string line in lines)
            {
                DateTime curTime;

                string[] lineToken = line.Split(',');

                if (DateTime.TryParseExact(lineToken[0], format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out curTime) == false)
                    continue;

                float gearSideData = Convert.ToSingle(lineToken[4]);
                float gearSideRawData = Convert.ToSingle(lineToken[5]);
                float manSideData = Convert.ToSingle(lineToken[6]);
                float manSideRawData = Convert.ToSingle(lineToken[7]);

                zeroingGearDataList.Add(new ScanData(curTime, gearSideData, gearSideRawData, 0));
                zeroingManDataList.Add(new ScanData(curTime, manSideData, manSideRawData, 0));

                rawGearDataList.Add(new ScanData(curTime, gearSideRawData, gearSideRawData, 0));
                rawManDataList.Add(new ScanData(curTime, manSideRawData, manSideRawData, 0));

                if (machineIf != null)
                {
                    float before = Convert.ToSingle(lineToken[8]);
                    float after = Convert.ToSingle(lineToken[9]);

                    beforePatternDataList.Add(new ScanData(curTime, before, before, 0));
                    afterPatternDataList.Add(new ScanData(curTime, after, after, 0));
                }
            }

            profilePanelList.ForEach(panel => panel.Initialize());
            profilePanelList.ForEach(panel => panel.ClearPanel());

            zeroingGearDataList.Sort((ScanData x, ScanData y) => x.X.CompareTo(y.X));
            zeroingManDataList.Sort((ScanData x, ScanData y) => x.X.CompareTo(y.X));
            rawGearDataList.Sort((ScanData x, ScanData y) => x.X.CompareTo(y.X));
            rawManDataList.Sort((ScanData x, ScanData y) => x.X.CompareTo(y.X));

            zeroingGear.AddScanDataList(zeroingGearDataList);
            zeroingMan.AddScanDataList(zeroingManDataList);

            rawGear.AddScanDataList(rawGearDataList);
            rawMan.AddScanDataList(rawManDataList);

            if (machineIf != null)
            {
                beforePatternDataList.Sort((ScanData x, ScanData y) => x.X.CompareTo(y.X));
                afterPatternDataList.Sort((ScanData x, ScanData y) => x.X.CompareTo(y.X));

                beforePattern.AddScanDataList(beforePatternDataList);
                afterPattern.AddScanDataList(afterPatternDataList);
            }

            profilePanelList.ForEach(panel => panel.DisplayResult());
        }

        public double[] GetChartData()
        {
            return new double[] { 1, 5, 30, 500 };
        }

        public void Clear()
        {
            profilePanelList.ForEach(panel => panel.Initialize(setting));
            profilePanelList.ForEach(panel => panel.ClearPanel());
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

            List<ScanData> zeroingGearDataList = new List<ScanData>(); //inspectionResultList.ConvertAll(f => f.GearSide);
            List<ScanData> zeroingManDataList = new List<ScanData>(); //inspectionResultList.ConvertAll(f => f.ManSide);

            List<ScanData> rawGearDataList = new List<ScanData>(); //new List<ScanData>(zeroingGearDataList);
            List<ScanData> rawManDataList = new List<ScanData>(); //new List<ScanData>(zeroingManDataList);

            List<ScanData> beforePatternDataList = new List<ScanData>(); //inspectionResultList.ConvertAll(f => f.BeforePattern);
            List<ScanData> afterPatternDataList = new List<ScanData>(); //inspectionResultList.ConvertAll(f => f.AffterPattern);

            foreach (InspectionResult ir in inspectionResultList)
            {
                DateTime dateTime = ir.InspectionStartTime;

                zeroingGearDataList.Add(new ScanData(dateTime, ir.GearSide.Y, 0, 0));
                zeroingManDataList.Add(new ScanData(dateTime, ir.ManSide.Y, 0, 0));

                rawGearDataList.Add(new ScanData(dateTime, ir.GearSide.YRaw, 0, 0));
                rawManDataList.Add(new ScanData(dateTime, ir.ManSide.YRaw, 0, 0));

                beforePatternDataList.Add(new ScanData(dateTime, ir.BeforePattern.Y, 0, 0));
                afterPatternDataList.Add(new ScanData(dateTime, ir.BeforePattern.Y, 0, 0));
            }

            profilePanelList.ForEach(panel => panel.Initialize());
            profilePanelList.ForEach(panel => panel.ClearPanel());

            zeroingGear.AddScanDataList(zeroingGearDataList);
            zeroingMan.AddScanDataList(zeroingManDataList);

            rawGear.AddScanDataList(rawGearDataList);
            rawMan.AddScanDataList(rawManDataList);

            beforePattern.AddScanDataList(beforePatternDataList);
            afterPattern.AddScanDataList(afterPatternDataList);

            profilePanelList.ForEach(panel => panel.DisplayResult());
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
                double timeSpan = (inspectionResult.InspectionStartTime - minDateTime).TotalSeconds;
                double value = Math.Abs(inspectionResult.GearSide.Y - inspectionResult.ManSide.Y);
                DataPoint dp = new DataPoint(timeSpan, value);
                dpList.Add(dp);
            }

            Dictionary<string, List<DataPoint>> result = new Dictionary<string, List<DataPoint>>();
            result.Add("Brightness", dpList);
            return result;
        }

        public UniScanM.Data.DataImporter CreateDataImprter()
        {
            return new DataImporter();
        }

        public bool InitializeChart(System.Windows.Forms.DataVisualization.Charting.Chart chart)
        {
            Font font = new Font("맑은 고딕", 12);

            chart.ChartAreas[0].AxisX.Title = StringManager.GetString(this.GetType().FullName, "Time [s]");
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

