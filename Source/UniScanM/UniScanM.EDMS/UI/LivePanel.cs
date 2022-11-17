using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.DataVisualization;
using DynMvp.Base;
using DynMvp.UI;
using UniScanM.EDMS.Settings;
using UniScanM.EDMS.Data;
using System.Windows.Forms.DataVisualization.Charting;
using LiveCharts;
using LiveCharts.Wpf;
using System.Threading;
using LiveCharts.Defaults;

namespace UniScanM.EDMS.UI
{
    public partial class LivePanel : UserControl, IMultiLanguageSupport
    {
        DataType dataType;
        List<DataPoint> dataList = new List<DataPoint>();
        List<double> datas = new List<double>();
        ChartValues<double> chartValues = new ChartValues<double>();

        ChartValues<ObservablePoint> observablePoints = new ChartValues<ObservablePoint>();
                    

        LiveCharts.Wpf.Axis yAxis = new LiveCharts.Wpf.Axis();
        LiveCharts.Wpf.Axis xAxis = new LiveCharts.Wpf.Axis();

        public DataType DataType { get => dataType; set => dataType = value; }

        public LivePanel(DataType dataType)
        {
            InitializeComponent();
            this.SuspendLayout();
            this.Dock = DockStyle.Fill;

            this.dataType = dataType;

            InitLineChart();
            SetDoubleBuffered(this);
            SetDoubleBuffered(lineChart);

            SetDoubleBuffered(txtMin);
            SetDoubleBuffered(txtMax);
            SetDoubleBuffered(txtAvg);

            SetDoubleBuffered(txtCur);
            SetDoubleBuffered(txtVar);
            SetDoubleBuffered(txtMinMax);

            this.ResumeLayout(false);
            StringManager.AddListener(this);
        }

        void InitLineChart()
        {
            lineChart.Zoom = ZoomingOptions.None;
            lineChart.Hoverable = false;
            lineChart.DataTooltip = null;
            lineChart.DisableAnimations = true;

        }

        delegate void UpdateTitleDelegate(string title);
        public void UpdateTitle(string title)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateTitleDelegate(UpdateTitle), title);
                return;
            }

            this.labelTitle.Text = title;
        }

        private string GetPanelTypeString()
        {
            string panelTypeStr = "Film";
            switch (DataType)
            {
                case DataType.FilmEdge:
                    panelTypeStr = "T100 Film";
                    break;
                case DataType.Coating_Film:
                    panelTypeStr = "T101 Film ~ Coating";
                    break;
                case DataType.Printing_Coating:
                    panelTypeStr = "T102 Coating ~ Printing";
                    break;
                case DataType.FilmEdge_0:
                    panelTypeStr = "T103 Film (Variance)";
                    break;
                case DataType.PrintingEdge_0:
                    panelTypeStr = "T104 Printing (Variance)";
                    break;
                case DataType.Printing_FilmEdge_0:
                    panelTypeStr = "T105 Film ~ Printing (Variance)";
                    break;
            }

            return panelTypeStr;
        }

        private bool NeedStopLine()
        {
            switch (DataType)
            {
                case DataType.FilmEdge:
                case DataType.Coating_Film:
                case DataType.Printing_Coating:
                case DataType.PrintingEdge_0:
                    return false;
                case DataType.FilmEdge_0:
                case DataType.Printing_FilmEdge_0:
                    return true;
            }

            return false;
        }

        private bool NeedWarningLine()
        {
            switch (DataType)
            {
                case DataType.FilmEdge:
                case DataType.Coating_Film:
                case DataType.Printing_Coating:
                case DataType.PrintingEdge_0:
                    return false;
                case DataType.FilmEdge_0:
                case DataType.Printing_FilmEdge_0:
                    return true;
            }

            return false;
        }

        void ClearChart()
        {
            lineChart.AxisX.Clear();
            lineChart.AxisY.Clear();
            lineChart.Series.Clear();   
        }
        public void Initialize()
        {
            EDMSSettings setting = EDMSSettings.Instance();
            lineChart.SuspendLayout();
            ClearChart();
            xAxis = new LiveCharts.Wpf.Axis
            {
                Title = "m",
                MinValue = 0,
                MaxValue = setting.XAxisDisplayDistance,
                FontWeight = System.Windows.FontWeights.Bold,
                LabelFormatter = (x) => string.Format("{0:0}m", x),
                //Labels = new List<string> { "0", "100", "200", "300", "400", "500", "600" },
                Separator = new Separator
                {
                    Step = (setting.XAxisDisplayDistance * 2) / setting.XAxisInterval,
                    Stroke = System.Windows.Media.Brushes.Yellow,
                    StrokeThickness = 1,
                    Margin = new System.Windows.Thickness(40),
                    StrokeDashArray = new System.Windows.Media.DoubleCollection(new double[] { 4 }),
                    Visibility = System.Windows.Visibility.Visible,
                },
            };

            yAxis = new LiveCharts.Wpf.Axis
            {
                Title = "mm",
                FontWeight = System.Windows.FontWeights.Bold,
                Separator = new Separator
                {
                    Stroke = System.Windows.Media.Brushes.Yellow,
                    StrokeThickness = 1,
                    Margin = new System.Windows.Thickness(70),
                },
                Sections = new SectionsCollection { }
            };
            try
            {
                lineChart.BackColor = Color.Black;
                lineChart.AxisX.Add(xAxis);
                lineChart.AxisY.Add(yAxis);
                lineChart.Series = new LiveCharts.SeriesCollection
                {
                    new LiveCharts.Wpf.LineSeries
                    {
                        Values = chartValues,
                        //Values = observablePoints,
                        Stroke = System.Windows.Media.Brushes.LawnGreen,
                        StrokeThickness = 3,
                        FontWeight = System.Windows.FontWeights.Bold,
                        Margin = new System.Windows.Thickness(40),
                        DataLabels = false,
                        PointGeometry = null,
                        Fill = System.Windows.Media.Brushes.Transparent,
                    }
                };
                Func<double, string> formatFunc1 = (x) => string.Format("{0:0}", x);
                Func<double, string> formatFunc2 = (x) => string.Format("{0:0.000}", x);
                Func<double, string> formatFunc3 = (x) => string.Format("{0:0.00}", x);
                if (dataType == DataType.Printing_FilmEdge_0)
                {
                    yAxis.Separator.Step = double.Parse(((setting.YAxisRangeUM * 2.0) / setting.YAxisInterval).ToString("F0"));
                    yAxis.MaxValue = Math.Abs(setting.YAxisRangeUM);
                    yAxis.MinValue = -Math.Abs(setting.YAxisRangeUM);
                    yAxis.LabelFormatter = formatFunc1;
                    yAxis.Title = "um";
                }
                else if (dataType == DataType.FilmEdge_0 || dataType == DataType.PrintingEdge_0)
                {
                    yAxis.Separator.Step = double.Parse(((setting.YAxisRangeMM * 2.0) / setting.YAxisInterval).ToString("F3"));
                    yAxis.MaxValue = Math.Abs(setting.YAxisRangeMM);
                    yAxis.MinValue = -Math.Abs(setting.YAxisRangeMM);
                    yAxis.LabelFormatter = formatFunc2;
                }
                else
                {
                    yAxis.Separator.Step = double.Parse(((setting.YAxisRangeMM * 2.0) / setting.YAxisInterval).ToString("F2"));
                    yAxis.MaxValue = Math.Abs(setting.YAxisRangeMM);
                    yAxis.MinValue = -Math.Abs(setting.YAxisRangeMM);
                    yAxis.LabelFormatter = formatFunc3;
                }
                if (dataType == DataType.Printing_FilmEdge_0 && setting.T103AlarmOriginOutEnable)
                {
                    yAxis.Sections.Add(new AxisSection
                    {
                        Value = -(float)setting.T103ErrorRange,
                        Stroke = System.Windows.Media.Brushes.Red,
                        StrokeThickness = 2,
                    });
                    yAxis.Sections.Add(new AxisSection
                    {
                        Value = (float)setting.T103ErrorRange,
                        Stroke = System.Windows.Media.Brushes.Red,
                        StrokeThickness = 2,
                    });
                }
                else
                {
                    yAxis.Sections.Add(new AxisSection
                    {
                        Value = -(float)setting.T105ErrorRange,
                        Stroke = System.Windows.Media.Brushes.Red,
                        StrokeThickness = 2,
                    });
                    yAxis.Sections.Add(new AxisSection
                    {
                        Value = (float)setting.T105ErrorRange,
                        Stroke = System.Windows.Media.Brushes.Red,
                        StrokeThickness = 2,
                    });
                }
                if (dataType == DataType.Printing_FilmEdge_0 && setting.T103AlarmOriginOutEnable)
                {
                    yAxis.Sections.Add(new AxisSection
                    {
                        Value = -(float)setting.T103WarningRange,
                        Stroke = System.Windows.Media.Brushes.Gold,
                        StrokeThickness = 2,
                    });
                    yAxis.Sections.Add(new AxisSection
                    {
                        Value = (float)setting.T103WarningRange,
                        Stroke = System.Windows.Media.Brushes.Gold,
                        StrokeThickness = 2,
                    });
                }
                else
                {
                    yAxis.Sections.Add(new AxisSection
                    {
                        Value = -(float)setting.T105WarningRange,
                        Stroke = System.Windows.Media.Brushes.Gold,
                        StrokeThickness = 2,
                    });
                    yAxis.Sections.Add(new AxisSection
                    {
                        Value = (float)setting.T105WarningRange,
                        Stroke = System.Windows.Media.Brushes.Gold,
                        StrokeThickness = 2,
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

            }
            lineChart.ResumeLayout();
        }

        public override void Refresh()
        {
            this.SuspendLayout();
            base.Refresh();
            lineChart.Refresh();
            txtMin.Text = "0.000";
            txtMax.Text = "0.000";
            txtAvg.Text = "0.000";
            txtCur.Text = "0.000";
            txtVar.Text = "0.000";
            txtMinMax.Text = "0.000";
            this.ResumeLayout();
        }

        public void AddChartData(DataPoint chartData)
        {
            lock (dataList)
            {
                if (dataList.Count > 0 && chartData.XValue < dataList.Max(f => f.XValue))
                    return;
            }

            double minDistance = chartData.XValue - EDMSSettings.Instance().XAxisDisplayDistance;
            double minValidDistance;
            lock (dataList)
            {
                dataList.RemoveAll(data => data.XValue < minDistance);
                dataList.Add(chartData);
                minValidDistance = dataList.Min(data => data.XValue);
            }

            //if (chartData.XValue == 0)
            //    chartValues.Clear();
            //if (chartValues.Count() < EDMSSettings.Instance().XAxisDisplayDistance)
            //{
            //    xAxis.MaxValue = 100;
            //    xAxis.MinValue = 0;
            //}
            //else
            //{
            //    if (dataList.Count() > 1)
            //    {
            //        xAxis.MaxValue = chartValues.Count();
            //        xAxis.MinValue = xAxis.MaxValue - EDMSSettings.Instance().XAxisDisplayDistance;
            //    }
            //}
            if (chartValues.Count() > EDMSSettings.Instance().XAxisDisplayDistance)
            {
                xAxis.MaxValue = chartValues.Count();
                xAxis.MinValue = xAxis.MaxValue - EDMSSettings.Instance().XAxisDisplayDistance;
            }

            //xAxis.Separator.Step = (chartData.XValue - minDistance) / EDMSSettings.Instance().XAxisInterval;
            xAxis.Separator.Step = EDMSSettings.Instance().XAxisInterval;
            //DisplayResult();
        }

        public void ChartPush()
        {
            if (chartValues.Count() > EDMSSettings.Instance().XAxisDisplayDistance)
            {
                xAxis.MaxValue = chartValues.Count();
                xAxis.MinValue = xAxis.MaxValue - EDMSSettings.Instance().XAxisDisplayDistance;
            }

            //xAxis.Separator.Step = (chartData.XValue - minDistance) / EDMSSettings.Instance().XAxisInterval;
            xAxis.Separator.Step = EDMSSettings.Instance().XAxisInterval;
        }



        public void AddChartDataList(List<DataPoint> chartDataList)
        {
            if (chartDataList.Count == 0)
                return;

            double minDistance = chartDataList.Min(data => data.XValue);
            double maxDistance = chartDataList.Max(data => data.XValue);
            lock (dataList)
            {
                this.dataList = chartDataList;
            }
            DisplayResult();
        }

        delegate void DisplayResultDelegate();
        public void DisplayResult()
        {
            EDMSSettings setting = EDMSSettings.Instance();
            lock (dataList)
            {
                lineChart.SuspendLayout();
                
                if (dataList.Count() > 0)
                {
                    float yRange = dataType != DataType.Printing_FilmEdge_0 ? setting.YAxisRangeMM : setting.YAxisRangeUM;

                    List<PointF> newDataList = new List<PointF>();

                    switch (dataType)
                    {
                        case DataType.FilmEdge:
                        case DataType.Coating_Film:
                        case DataType.Printing_Coating:
                            yAxis.MaxValue = dataList.Max(data => data.YValues.Max()) + Math.Abs(yRange);
                            yAxis.MinValue = dataList.Min(data => data.YValues.Min()) - Math.Abs(yRange);
                            this.dataList.ForEach(data => newDataList.AddRange(GetPoints(data)));
                            break;
                        case DataType.FilmEdge_0:
                        case DataType.PrintingEdge_0:
                        case DataType.Printing_FilmEdge_0:
                            this.dataList.ForEach(data =>
                            {
                                PointF[] points = GetPoints(data);
                                Array.ForEach(points, f =>
                                {
                                    float newY = f.Y < 0 ? (float)Math.Max(f.Y, -Math.Abs(yRange + 0.001)) : (float)Math.Min(f.Y, Math.Abs(yRange - 0.001));
                                    newDataList.Add(new PointF(f.X, newY));
                                });
                            });

                            float rangeP = Math.Max(yRange, newDataList.Max(f => f.Y));
                            float rangeN = Math.Min(-yRange, newDataList.Min(f => f.Y));
                            float range = Math.Max(rangeP, -rangeN);
                            yAxis.MaxValue = Math.Abs(range);
                            yAxis.MinValue = -Math.Abs(range);
                            break;
                    }
                    
                    #region 수정전
                    if (chartValues.Count() < xAxis.MinValue)
                    {
                        for (int i = 0; i < xAxis.MinValue; i++)
                        {
                            chartValues.Add(double.NaN);
                        }
                    }
                    //if (newDataList[newDataList.Count() - 1].X == newDataList[countValue - 1].X) cv[cv.Count() - 1] = dataList.Last().YValues.Average();
                    //else cv.Add(averValue);
                    if (newDataList.Count > 1)
                    {
                        if (newDataList[newDataList.Count - 1].X != newDataList[newDataList.Count - 2].X)
                        {
                            float averValue = newDataList.Where(x => newDataList[newDataList.Count - 1].X == x.X).Select(x => x.Y).Average();
                            chartValues.Add(averValue);
                            //if (chartValues.Count > EDMSSettings.Instance().XAxisDisplayDistance + 10)
                            //    chartValues.RemoveAt(0);
                        }
                    }

                    yAxis.Separator.Step = (double)(yAxis.MaxValue - yAxis.MinValue) / setting.YAxisInterval;
                    #endregion


                    //if (newDataList.Count() > 1)
                    //{
                    //    if (newDataList[newDataList.Count() - 1].X != newDataList[newDataList.Count() - 2].X)
                    //    {
                    //        if (observablePoints.Count > EDMSSettings.Instance().XAxisDisplayDistance + 10)
                    //            observablePoints.RemoveAt(0);
                    //        float averValue = newDataList.Where(x => newDataList[newDataList.Count() - 1].X == x.X).Select(x => x.Y).Average();
                    //        float xpos = newDataList.Where(x => newDataList[newDataList.Count() - 1].X == x.X).Select(x => x.X).Average();

                    //        observablePoints.Add(new ObservablePoint(xpos, averValue));
                    //    }
                    //}
                    //yAxis.Separator.Step = (double)(yAxis.MaxValue - yAxis.MinValue) / setting.YAxisInterval;
                    
                }
                lineChart.ResumeLayout();
                DisplayLabel();
            }
        }

        public LiveCharts.SeriesCollection AddLineChartData(LiveCharts.SeriesCollection series, string title, IChartValues value)
        {
            LiveCharts.Wpf.LineSeries ChartData = (new LiveCharts.Wpf.LineSeries
            {
                Title = title,
                Values = value,
                DataLabels = false,
                MinHeight = 0.0f,
                PointGeometry = null,
            });
            series.Add(ChartData);
            return series;
        }

        /// <summary>
        /// 중복된 X를 편다..
        /// </summary>
        /// <param name="newDataList"></param>
        /// <returns></returns>
        private List<PointF> GetShowDataList(List<PointF> dataList)
        {
            List<PointF> showDataList = new List<PointF>();

            while (dataList.Count > 0)
            {
                PointF data = dataList.First();
                List<PointF> sameData = dataList.FindAll(f => f.X == data.X);
                int sameCount = sameData.Count;
                for (int i = 0; i < sameCount; i++)
                {
                    float newX = data.X + (i * 1.0f / (sameCount + 1));
                    showDataList.Add(new PointF(newX, sameData[i].Y));
                };
                dataList.RemoveAll(f => sameData.Contains(f));
            }

            return showDataList;
        }

        private PointF[] GetPoints(DataPoint data)
        {
            List<PointF> pointList = new List<PointF>();
            Array.ForEach(data.YValues, f => pointList.Add(new PointF((float)data.XValue, (float)f)));
            return pointList.ToArray();
        }

        delegate void DisplayLabelDelegate();
        private void DisplayLabel()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new DisplayLabelDelegate(DisplayLabel));
                return;
            }
            //UiHelper.SuspendDrawing(txtMin);
            //UiHelper.SuspendDrawing(txtMax);
            //UiHelper.SuspendDrawing(txtAvg);
            //UiHelper.SuspendDrawing(txtCur);
            //UiHelper.SuspendDrawing(txtVar);
            //UiHelper.SuspendDrawing(txtMinMax);

            if (dataList.Count() > 0)
            {
                double min = dataList.Min(data => data.YValues.Min());
                double max = dataList.Max(data => data.YValues.Max());
                double avg = dataList.Average(data => data.YValues.Average());
                double cur = dataList.Last().YValues.Average();

                txtMin.Text = String.Format("{0:0.000}", min);
                txtMax.Text = String.Format("{0:0.000}", max);
                txtAvg.Text = String.Format("{0:0.000}", avg);
                txtCur.Text = String.Format("{0:0.000}", cur);
                txtMinMax.Text = String.Format("{0:0.000}", max - min);
                txtVar.Text = String.Format("{0:0.000}", GetStdDev());
            }
            else
            {
                txtMin.Text = "0.000";
                txtMax.Text = "0.000";
                txtAvg.Text = "0.000";
                txtCur.Text = "0.000";
                txtVar.Text = "0.000";
                txtMinMax.Text = "0.000";
            }

            //UiHelper.ResumeDrawing(txtMin);
            //UiHelper.ResumeDrawing(txtMax);
            //UiHelper.ResumeDrawing(txtAvg);
            //UiHelper.ResumeDrawing(txtCur);
            //UiHelper.ResumeDrawing(txtVar);
            //UiHelper.ResumeDrawing(txtMinMax);

        }

        public float GetStdDev()
        {
            lock (dataList)
            {
                if (dataList.Count < 1)
                    return 0;

                double var = 0f;
                double avg = dataList.Average(data => data.YValues.Average());

                foreach (DataPoint data in dataList)
                    foreach (double y in data.YValues)
                        var += Math.Pow((double)(y - avg), 2);

                return (float)(var / (double)dataList.Count);
            }

            //return (float)Math.Sqrt(var / (double)dataList.Count);
        }

        public void ClearPanel()
        {
            lock (dataList)
            {
                dataList.Clear();
            }
            DisplayResult();
        }

        public void ClearPanel2(double rollingDistance)
        {
            lock (dataList)
            {
                dataList.Clear();
                chartValues = new ChartValues<double>();
            }
            xAxis.MaxValue = rollingDistance + EDMSSettings.Instance().XAxisDisplayDistance;
            xAxis.MinValue = xAxis.MaxValue - EDMSSettings.Instance().XAxisDisplayDistance;
            DisplayResult();
        }

        private void ultraButtonZoomReset_Click(object sender, EventArgs e)
        {

        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            labelTitle.Text = StringManager.GetString(this.GetType().FullName, GetPanelTypeString());
            labelUnit.Text = GetPanelTypeUnit();
        }

        private string GetPanelTypeUnit()
        {
            if (dataType == DataType.Printing_FilmEdge_0)
                return "[um]";
            return "[mm]";
        }

        public static void SetDoubleBuffered(System.Windows.Forms.Control c)
        {
            if (System.Windows.Forms.SystemInformation.TerminalServerSession)
                return;
            System.Reflection.PropertyInfo aProp = typeof(System.Windows.Forms.Control).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            aProp.SetValue(c, true, null);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
