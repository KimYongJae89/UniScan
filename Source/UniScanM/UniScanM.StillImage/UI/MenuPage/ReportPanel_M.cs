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
using UniScanM.StillImage.Data;
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
using UniScanM.StillImage.Settings;
//using UniScanM.Data;

namespace UniScanM.StillImage.UI
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

        private void Update_PatternLengthChart(UniScanM.Data.DataImporter dataImporter, bool showNgOnly)
        {//chart_PatternLength
            if (InvokeRequired)
            {
                Invoke(new UpdateDataDelegate(Update_PatternLengthChart), dataImporter, showNgOnly);
                return;
            }
            List<InspectionResult> inspectionResultList = new List<InspectionResult>();
            if (dataImporter.Count > 0)
            {
                inspectionResultList = dataImporter.InspectionResultList.ConvertAll(f => (InspectionResult)f);
            }

            List<double>[] listZone = new List<double>[6];
            for(int i=0; i<6; i++)
            {
                listZone[i] = new List<double>();
            }

            double fmin = 9999999, fmax = -9999999;
            foreach( var result  in inspectionResultList)
            {
                int ZoneIndex = result.InspZone;
                if (ZoneIndex < 0 || ZoneIndex > 5) continue;

                //****************************************************************//
                double value = 0;
                switch (GetPrintLengthMode())
                {
                    case Settings.PrintingLengthMode.PrintingPeriod:
                        value = result.PrintingPeriod;
                        break;

                    case Settings.PrintingLengthMode.PrintingLength:
                        value = result.PrintingLength;
                        break;

                    case Settings.PrintingLengthMode.UnPrintLength:
                        value = result.PrintingPeriod - result.PrintingLength;
                        break;
                }
                
                listZone[ZoneIndex].Add(value);
                if(ZoneIndex==0 || ZoneIndex==5) //1번과 6번은 데이터가 절반이라 두번씩 add 해서 데이터 길이를 나머지랑 비슷하게 맞춤.

                    listZone[ZoneIndex].Add(value);
                if (fmin > value) fmin = value;
                if (fmax < value) fmax = value;
            }
            double fdiff = fmax - fmin;
            if (fdiff != 0)
            {
                chart_PatternLength.ChartAreas[0].AxisY.Maximum = fmax + fdiff * 0.2;
                chart_PatternLength.ChartAreas[0].AxisY.Minimum = fmin - fdiff * 0.2;
            }

            for (int i = 0; i < 6; i++)
            {
                chart_PatternLength.Series[i].Points.DataBindY(listZone[i]);
            }

            Update_PatternLengthGridView(listZone);
        }

        private void dataGridViewResult_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewResult.SelectedRows.Count == 0)
                return;

            ImageD imageD = null;
            Data.InspectionResult inspectionResult = (Data.InspectionResult)dataGridViewResult.SelectedRows[0].Tag;
            if (inspectionResult == null)
                return;

            string imagePath = Path.Combine(inspectionResult.ResultPath, inspectionResult.GetFullImageFileName());
            if (File.Exists(imagePath))
            {
                imageD = new Image2D();
                imageD.LoadImage(imagePath);
                inspectionResult.GrabImageList.Add(imageD);
                imageD.Tag = imagePath;
            }

            this.drawBox.FigureGroup.Clear();
            this.drawBox.UpdateImage(imageD?.ToBitmap());
            this.drawBox.ZoomFit();
            this.drawBox.Tag = imageD?.Tag;
            this.drawBox.Invalidate();
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

            this.dataGridViewResult.Rows.Clear();
            drawBox.UpdateImage(null);
            SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;

            List<InspectionResult> inspectionResultList = new List<InspectionResult>();
            if (dataImporter.Count > 0)
            {
                inspectionResultList = dataImporter.InspectionResultList.ConvertAll(f => (InspectionResult)f);
                if (showNgOnly)
                    inspectionResultList.RemoveAll(f => f.IsGood());
            }

            for (int i = 0; i < inspectionResultList.Count; i++)
            {
                InspectionResult inspectionResult = inspectionResultList[i];
                if (inspectionResult == null)
                    continue;

                int zone = (int)inspectionResult.InspZone;
                int rollDist = (int)inspectionResult.GetExtraResult("RollDistance");
                Feature offsetFeature = (Feature)inspectionResult.GetExtraResult("OffsetFeature");
                Feature inspFeature = (Feature)inspectionResult.GetExtraResult("InspFeature");
                Rectangle defect = inspectionResult.ProcessResultList.DefectRectList.FirstOrDefault();
                bool result = inspectionResult.IsGood();

                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                dataGridViewRow.CreateCells(dataGridViewResult, i, zone + 1, rollDist,
                    string.Format("{0:F1}({1:F1}) / {2:F1}({3:F1})", inspFeature.Margin.Width, offsetFeature.Margin.Width,inspFeature.Margin.Height, offsetFeature.Margin.Height),
                    string.Format("{0:F1} / {1:F1}", offsetFeature.Blot.Width, offsetFeature.Blot.Height),
                    string.Format("{0:F1} / {1:F1}", defect.Width * pelSize.Width, defect.Height * pelSize.Height),
                    result ? "GOOD" : "NG");

                if (result == false)
                {
                    dataGridViewRow.DefaultCellStyle.BackColor = Color.Red;
                    dataGridViewRow.DefaultCellStyle.ForeColor = Color.White;
                }

                dataGridViewRow.Tag = inspectionResult;
                this.dataGridViewResult.Rows.Add(dataGridViewRow);
            }
            //DataPoint dp = new DataPoint(pair.Key, pair.Value.ConvertAll<double>(f => f.Margin.Width).ToArray());
            //chart.Series[0].Points.Add(dp);
            //chart.ChartAreas[0].AxisY.Minimum = pair.Value.Min(f => f.Margin.Width);
            //chart.ChartAreas[0].AxisY.Maximum = pair.Value.Max(f => f.Margin.Width);
            //int x = pair.Key;
            //float y1 = pair.Value.Max();
            //float y1 = pair.Value.Min();
            //float[] y = ;
            //Array.ForEach(y, f => chart.Series[0].Points.AddXY(pair.Key, f));

            //chart.Series[0].Points.DataBindXY(chartX, featureList.ConvertAll(f => f.ConvertAll(g=>g.Margin.Width)));
            //chart.Series[1].Points.DataBindXY(chartX, featureList.ConvertAll(f => f.Margin.Height).ToArray());
            //chart.Series[2].Points.DataBindXY(chartX, featureList.ConvertAll(f => f.Blot.Width).ToArray());
            //chart.Series[3].Points.DataBindXY(chartX, featureList.ConvertAll(f => f.Blot.Height).ToArray());
            Update_PatternLengthChart(dataImporter, showNgOnly);
        }

        public Dictionary<string, List<DataPoint>> GetChartData(int srcPos, int dstPos, UniScanM.Data.DataImporter dataImporter)
        {
            Dictionary<int, List<Feature>> seriesData = new Dictionary<int, List<Feature>>();

            int maxRollDist = -1;
            for (int i = 0; i < dataImporter.Count; i++)
            {
                InspectionResult inspectionResult = dataImporter.GetInspectionResult(i) as InspectionResult;
                if (inspectionResult == null)
                    continue;

                int rollDist = (int)inspectionResult.GetExtraResult("RollDistance");
                Feature feature = (Feature)inspectionResult.GetExtraResult("Result");

                if (seriesData.ContainsKey(rollDist) == false)
                    seriesData.Add(rollDist, new List<Feature>());
                seriesData[rollDist].Add(feature);
                maxRollDist = Math.Max(maxRollDist, rollDist);
            }

            if(dstPos< srcPos)
            {
                dstPos = maxRollDist;
            }

            Func<Feature, float>[] extractFunc = new Func<Feature, float>[]
            {
                new Func<Feature, float>(f=>f.Margin.Width),
                new Func<Feature, float>(f=>f.Margin.Height),
                new Func<Feature, float>(f=>f.Blot.Width),
                new Func<Feature, float>(f=>f.Blot.Height)
            };
            string[] seriesLabel = new string[] { "Margin W", "Margin L", "Blot W", "Blot L" };

            Dictionary<string, List<DataPoint>> dic = new Dictionary<string, List<DataPoint>>();
            for (int i = 0; i < 4; i++)
            {
                dic.Add(seriesLabel[i], new List<DataPoint>());
                for (int j = srcPos; j <= dstPos; j++)
                {
                    if (seriesData.ContainsKey(j))
                    {
                        List<Feature> valueList = seriesData[j];
                        for (int k = 0; k < valueList.Count; k++)
                        {
                            float dist = j + k * 1.0f / valueList.Count;
                            float value = extractFunc[i](valueList[k]);

                            DataPoint dp = new DataPoint(dist, value);
                            dic[seriesLabel[i]].Add(dp);
                            //chart.Series[i].Points.Add(dp);
                        }
                    }
                }
            }
            
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
