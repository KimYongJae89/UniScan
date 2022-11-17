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
using UniScanM.CGInspector.Data;
using DynMvp.Base;
using System.IO;
using UniEye.Base.UI;
using DynMvp.Data.UI;
using DynMvp.UI;
using UniEye.Base;
using DynMvp.UI.Touch;
using System.Threading;
using Infragistics.Win.Misc;
using Infragistics.Win;
using DynMvp.Data;
using UniScanM.UI;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;
//using UniScanM.Data;

namespace UniScanM.CGInspector.UI
{
    public partial class ReportPanel : UserControl, IUniScanMReportPanel, IMultiLanguageSupport
    {
        private DrawBox drawBox = null;
        private UniScanM.Data.FigureDrawOption figureDrawOption = null;

        private int dist;

        public string AxisYFormat => "{F1}";

        public ReportPanel()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.drawBox = new DrawBox();
            this.drawBox.Dock = DockStyle.Fill;
            this.drawBox.AutoFitStyle = AutoFitStyle.KeepRatio;
            this.drawBox.MouseDoubleClick += DrawBox_MouseDoubleClick;
            this.panelImage.Controls.Add(this.drawBox);

            StringManager.AddListener(this);
        }

        public bool ShowNgOnlyButton()
        {
            return true;
        }

        public UniScanM.Data.DataImporter CreateDataImprter()
        {
            return new DataImporter();
        }

        public void UpdateData(UniScanM.Data.DataImporter dataImporter, bool showNgOnly)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(UpdateData), dataImporter, showNgOnly);
                return;
            }

        }

        public Dictionary<string, List<DataPoint>> GetChartData(int srcPos, int dstPos, UniScanM.Data.DataImporter dataImporter)
        {
           
            Dictionary<string, List<DataPoint>> dic = new Dictionary<string, List<DataPoint>>();
            
            return dic;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public void Search(ProductionBase production) { }
        public void Initialize() { }
        public void Clear()
        {
            //구우우우우우혀혀녀녀ㅕ년연안이ㅏ먼암넝;ㅣ마넝
        }

        public bool InitializeChart(Chart chart)
        {
            Font font = new Font("맑은 고딕", 12);

            chart.ChartAreas[0].AxisX.Title = StringManager.GetString(this.GetType().FullName, "Distance [m]");
            chart.ChartAreas[0].AxisY.Title = StringManager.GetString(this.GetType().FullName, "Margin [um]");
            chart.ChartAreas[0].AxisY2.Title = StringManager.GetString(this.GetType().FullName, "Blot [um]");
            chart.ChartAreas[0].AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.DashDotDot;
            chart.ChartAreas[0].AxisY2.Maximum = 50;
            chart.ChartAreas[0].AxisX.TitleFont =
                chart.ChartAreas[0].AxisY.TitleFont =
                chart.ChartAreas[0].AxisY2.TitleFont = font;

            chart.Legends[0].Font = new Font("맑은 고딕", 12);

            chart.Series.Clear();
            chart.Series.Add(new Series { ChartType = SeriesChartType.Line, BorderDashStyle = ChartDashStyle.Solid, BorderWidth = 2, LegendText = "Margin W", YAxisType = AxisType.Primary });
            chart.Series.Add(new Series { ChartType = SeriesChartType.Line, BorderDashStyle = ChartDashStyle.Solid, BorderWidth = 2, LegendText = "Margin L", YAxisType = AxisType.Primary });
            chart.Series.Add(new Series { ChartType = SeriesChartType.Line, BorderDashStyle = ChartDashStyle.Dash, BorderWidth = 2, LegendText = "Blot W", YAxisType = AxisType.Secondary });
            chart.Series.Add(new Series { ChartType = SeriesChartType.Line, BorderDashStyle = ChartDashStyle.Dash, BorderWidth = 2, LegendText = "Blot L", YAxisType = AxisType.Secondary });

            bool useDefaultReportPanel = true;

            return useDefaultReportPanel;
        }


        private void DrawBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CanvasPanel canvasPanel = sender as CanvasPanel;
            if(canvasPanel != null)
            {
                if (canvasPanel.Image == null)
                    return;

                string imageFile = (string)canvasPanel.Tag;

                if (File.Exists(imageFile))
                    System.Diagnostics.Process.Start(imageFile);
                else
                    MessageForm.Show(null, string.Format(StringManager.GetString("Can not Fouund Image. [{0}]"), imageFile));
            }
            //
        }

    }
}
