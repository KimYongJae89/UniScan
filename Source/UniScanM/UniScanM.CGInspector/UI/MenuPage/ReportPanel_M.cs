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
using UniScanM.CGInspector.Settings;
//using UniScanM.Data;

namespace UniScanM.CGInspector.UI
{
    public enum GridRow { Spec, Average, Max, Min, Diff, STDEV, Variance, LastEnum };

    public partial class ReportPanel_M : UserControl, IUniScanMReportPanel, IMultiLanguageSupport
    {
         private DrawBox drawBox = null;
        private UniScanM.Data.FigureDrawOption figureDrawOption = null;

        private int dist;

        public string AxisYFormat => "{F1}";

        public ReportPanel_M()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.drawBox = new DrawBox();
            this.drawBox.Dock = DockStyle.Fill;
            this.drawBox.AutoFitStyle = AutoFitStyle.KeepRatio;
            this.drawBox.MouseDoubleClicked += DrawBox_MouseDoubleClick;
            this.panelImage.Controls.Add(this.drawBox);
            this.drawBox.Enable = true;

            Initialize_PatternLengthTab();

            StringManager.AddListener(this);
        }

        
        private void Initialize_PatternLengthTab()
        {
            //dgvReport_PatternLength; ///////////////////////////////////////
            
            string[] titles = new string[] { "spec", "Avg", "Max", "Min","M-m", "Stdev", "Var(%)" };

            //DataGridViewRow row = this.dgvReport_PatternLength.RowTemplate;
            //row.DefaultCellStyle.BackColor = Color.Bisque;
            //row.Height = 55;
            //row.MinimumHeight = 33;

            for (int i = 0; i < 7; i++)
            {
                double val = (i + 1) + (i + 1) / 10.0;
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                if(i>3)
                {
                    dataGridViewRow.DefaultCellStyle.Format = "0.000";
                }
                dataGridViewRow.Height =45;

                dataGridViewRow.CreateCells(dgvReport_PatternLength, val * 1, val * 2, val * 3, val * 4, val * 5, val * 6);
                dgvReport_PatternLength.Rows.Add(dataGridViewRow);
                dgvReport_PatternLength.Rows[i].HeaderCell.Value = titles[i];
            }

            dgvReport_PatternLength.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            dgvReport_PatternLength.ClearSelection();


            //chart_PatternLength;////////////////////////////////////////////////
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
                chart_PatternLength.Series[i].Points.Clear();

            double Max = 90;
            int Distance = 100;
            for (int series = 0; series < 6; series++)
            {
                for (int i = 0; i < Distance; i++)
                {
                    double value = Math.Sin((double)(i+series*15 ) / 100 * Math.PI * 2) * Max / 2 + Max / 2 + 10;
                    chart_PatternLength.Series[series].Points.Add(value);             
                }
            }
        }

        private void Update_PatternLengthGridView(UniScanM.Data.DataImporter dataImporter, bool showNgOnly)
        {
            //dgvReport_PatternLength
            if (InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(Update_PatternLengthGridView), dataImporter, showNgOnly);
                return;
            }
            List<InspectionResult> inspectionResultList = new List<InspectionResult>();
            if (dataImporter.Count > 0)
            {
                inspectionResultList = dataImporter.InspectionResultList.ConvertAll(f => (InspectionResult)f);
            }
        }

        private void Update_PatternLengthGridView(List<double>[] listZone)
        {//private enum GridRow { Spec, Average, Max, Min, Diff, STDEV, Variance, LastEnum }
         //dgvReport_PatternLength

            double[] data = new double[7];
            double max=0, min = 0, std =0, avg=0;
            for(int zone = 0; zone < listZone.Length; zone++)
            {
                data[(int)GridRow.Spec] = 0;
                data[(int)GridRow.Average] = avg = listZone[zone].Average();
                data[(int)GridRow.Max] = max = listZone[zone].Max();
                data[(int)GridRow.Min] = min = listZone[zone].Min();
                data[(int)GridRow.Diff] = max - min;
                data[(int)GridRow.STDEV] = std =StdDev(listZone[zone]);
                data[(int)GridRow.Variance] = std/avg * 100;  // 변동 계수 CV (상대표준편차) //표준편차 ÷ 평균 *100

                for (int i = 0; i < (int)GridRow.LastEnum; i++)
                {
                    dgvReport_PatternLength[zone, i].Value = data[i]; //데이터셀만 0 base {열, 행}
                }
            }
        }

        public double StdDev(IEnumerable<double> values)
        {
            double ret = 0;
            int count = values.Count();
            if (count > 1)
            {
                //Compute the Average
                double avg = values.Average();

                //Perform the Sum of (value-avg)^2
                double sum = values.Sum(d => (d - avg) * (d - avg));

                //Put it all together
                ret = Math.Sqrt(sum / count);
            }
            return ret;
        }
 
   
        private PrintingLengthMode GetPrintLengthMode()
        {
            return Settings.StillImageSettings.Instance().PrintingLengthMeasurMode;
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


        private void DrawBox_MouseDoubleClick(object sender)
        {
            DrawBox drawbox = (DrawBox)sender;
            if (drawbox.Image == null)
                return;

            string imageFile = (string)drawbox.Tag;

            if (File.Exists(imageFile))
                System.Diagnostics.Process.Start(imageFile);
            else
                MessageForm.Show(null, string.Format(StringManager.GetString("Can not Fouund Image. [{0}]"), imageFile));
        }

    }
}
