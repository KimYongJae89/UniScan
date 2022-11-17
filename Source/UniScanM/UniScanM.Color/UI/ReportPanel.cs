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
//using UniScanM.Data;
using DynMvp.Base;
using System.IO;
using UniEye.Base.UI;
using DynMvp.Data.UI;
using DynMvp.UI;
using UniEye.Base;
using DynMvp.UI.Touch;
using UniScanM.ColorSens.Data;
using DynMvp.Data;
using UniScanM.UI;
using System.Windows.Forms.DataVisualization.Charting;
//using UniScanM.Data;

namespace UniScanM.ColorSens.UI
{
    public partial class ReportPanel : UserControl, IUniScanMReportPanel, IMultiLanguageSupport
    {
        //List<float> list_SheetBrightness = new List<float>();
        //List<double> list_GraphX_Time = new List<double>();
        public string AxisYFormat => "{F1}";

        public ReportPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            StringManager.AddListener(this);
        }
        
        private void ReportPanel_Load(object sender, EventArgs e)
        {
        }
        
        public bool ShowNgOnlyButton()
        {
            return true;
        }

        private void dataGridViewResult_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewResult.SelectedRows.Count == 0)
                return;

            ImageD imageD = null;
            ColorSens.Data.InspectionResult inspectionResult = (ColorSens.Data.InspectionResult)dataGridViewResult.SelectedRows[0].Tag;
            if (inspectionResult.GrabImageList.Count == 0)
            {
                string imagePath = Path.Combine(inspectionResult.ResultPath, string.Format("{0}.jpg", inspectionResult.InspectionNo));
                if (File.Exists(imagePath))
                {
                    imageD = new Image2D();
                    imageD.LoadImage(imagePath);
                    inspectionResult.GrabImageList.Add(imageD);
                }
            }
            else
                imageD = inspectionResult.GrabImageList[0];

            resultView.Image = imageD?.ToBitmap();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Initialize() { }
        public void Clear()
        {
            dataGridViewResult.Rows.Clear();
            resultView.Image = null;

        }
        public void Search(ProductionBase production) { }

        public void UpdateData(UniScanM.Data.DataImporter dataImporter, bool showNgOnly)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(UpdateData), dataImporter, showNgOnly);
                return;
            }

            //UiHelper.SuspendDrawing(dataGridViewResult);
            dataGridViewResult.SuspendLayout();
            Clear();

            List<InspectionResult> colorInspectionResultList = new List<InspectionResult>();
            if (dataImporter.InspectionResultList.Count > 0)
            {
                colorInspectionResultList = dataImporter.InspectionResultList.ConvertAll(f => (InspectionResult)f);
                if (showNgOnly)
                    colorInspectionResultList.RemoveAll(f => f.IsGood());
            }

            for (int i = 0; i < colorInspectionResultList.Count; i++)
            {
                InspectionResult colorInspectionResult = colorInspectionResultList[i];

                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                dataGridViewRow.CreateCells(
                    dataGridViewResult,
                    int.Parse(colorInspectionResult.InspectionNo),
                    colorInspectionResult.RollDistance.ToString("F0"),
                    colorInspectionResult.SheetBrightness.ToString("F2"),
                    colorInspectionResult.IsGood() ? "OK" : "NG",
                    colorInspectionResult.Lowerlimit.ToString("F3"),
                    colorInspectionResult.Uppperlimit.ToString("F3"),
                    colorInspectionResult.ReferenceBrightness.ToString("F3")
                    );

                if (colorInspectionResult.IsGood() == false)
                {
                    dataGridViewRow.DefaultCellStyle.BackColor = Color.Red;
                    dataGridViewRow.DefaultCellStyle.ForeColor = Color.White;
                }

                dataGridViewRow.Tag = colorInspectionResult;
                this.dataGridViewResult.Rows.Add(dataGridViewRow);
            }
            this.dataGridViewResult.AutoResizeColumns();

            //UiHelper.ResumeDrawing(dataGridViewResult);
            dataGridViewResult.ResumeLayout();
        }

        public Dictionary<string, List<DataPoint>> GetChartData(int startPos, int endPos, UniScanM.Data.DataImporter dataImporter)
        {
            List<InspectionResult> inspectionResult = dataImporter.InspectionResultList.ConvertAll(f => (InspectionResult)f);
            List<DataPoint> dpList = new List<DataPoint>();
            for (int i = startPos; i <= endPos; i++)
            {
                List<InspectionResult> sameDistanceList = inspectionResult.FindAll(f => f.RollDistance == i);
                for (int j = 0; j < sameDistanceList.Count; j++)
                {
                    float newDistance = i + j * 1.0f / sameDistanceList.Count;
                    float brightness = sameDistanceList[j].SheetBrightness;

                    DataPoint dp = new DataPoint(newDistance, brightness);
                    dpList.Add(dp);
                }
            }
            Dictionary<string, List<DataPoint>> result = new Dictionary<string, List<DataPoint>>();
            result.Add("Brightness", dpList);
            return result;
        }

        public UniScanM.Data.DataImporter CreateDataImprter()
        {
            return new DataImporter();
        }

        public bool InitializeChart(Chart chart)
        {
            Font font = new Font("맑은 고딕", 12);

            chart.ChartAreas[0].AxisX.Title = StringManager.GetString(this.GetType().FullName ,"Distance [m]");
            chart.ChartAreas[0].AxisY.Title = StringManager.GetString(this.GetType().FullName, "Brightness [Lv]");

            chart.ChartAreas[0].AxisX.TitleFont = font;
            chart.ChartAreas[0].AxisY.TitleFont = font;
            chart.Legends[0].Font = font;

            chart.Series.Clear();
            chart.Series.Add(new Series {Name= "graphdata", ChartType = SeriesChartType.Line, BorderWidth = 3, LegendText = "Brightness", YAxisType = AxisType.Primary });

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
