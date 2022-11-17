using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RCITest.Controls
{
    class CustomChartControl : Chart
    {
        public bool IsRotate90 { get; set; } = false;
        public bool Zoomable { get; set; } = false;

        public CustomChartControl(string[] seriesName) : base()
        {
            this.Legends.Clear();

            ChartArea chartArea = this.ChartAreas.FirstOrDefault();
            if (chartArea == null)
                this.ChartAreas.Add(chartArea = new ChartArea());

            this.Series.Clear();
            Array.ForEach(seriesName, f =>
            {
                Series series = new Series(f)
                {
                    ChartType = SeriesChartType.Line
                };
                this.Series.Add(series);
            });

            this.Padding = new Padding(0);
            this.Margin = new Padding(0);

            MouseWheel += CustomChartControl_MouseWheel;
            MouseDoubleClick += CustomChartControl_MouseDoubleClick;
        }

        private void CustomChartControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!Zoomable)
                return;

            FitAxis();
        }

        private void CustomChartControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!Zoomable)
                return;

            //HitTestResult res = this.HitTest(e.X, e.Y);

            ChartArea chartArea = this.ChartAreas.FirstOrDefault();
            if (chartArea == null)
                return;

            Axis axisX = IsRotate90 ? chartArea.AxisY : chartArea.AxisX;
            double axisLength = axisX.Maximum - axisX.Minimum;
            double mp = (e.X * 1.0 / this.Width) * axisLength;

            chartArea.CursorX.AutoScroll = true;
            axisX.LabelStyle.Enabled = Zoomable;
            axisX.ScaleView.Zoomable = true;
            axisX.ScaleView.SizeType = DateTimeIntervalType.Auto;

            double viewLength = axisX.ScaleView.ViewMaximum - axisX.ScaleView.ViewMinimum;
            mp = (e.X * 1.0 / this.Width) * viewLength + axisX.ScaleView.ViewMinimum;
            double viewMin = 0;
            double viewMax = 0;
            
            if (e.Delta > 0)
            {
                viewMin = mp - viewLength / 4;
                viewMax = mp + viewLength / 4;
                viewLength = viewMax - viewMin;
            }
            else
            {
                viewMin = mp - viewLength * 2;
                viewMax = mp + viewLength * 2;
                viewLength = viewMax - viewMin;
            }

            if (viewLength > axisLength)
                axisX.ScaleView.ZoomReset();
            else
            {
                if (viewMin < 0)
                {
                    viewMin = 0;
                    viewMax = viewLength;
                }
                else if (viewMax > axisLength)
                {
                    viewMax = axisLength;
                    viewMin = axisLength - viewLength;
                }
                axisX.ScaleView.Zoom(viewMin, viewMax);
            }

            //axisX.ScaleView.ZoomReset();
            axisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            axisX.ScaleView.SmallScrollSize = 100;
        }

        public CustomChartControl() : this(new string[] { "" }) { }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            ChartArea chartArea = this.ChartAreas.FirstOrDefault();
            if (chartArea != null)
            {
                //chartArea.Position.Width = this.Width;
                //chartArea.Position.Height = this.Height;
            }
        }

        internal void SetData(string seriseName, float[] data)
        {
            Series series = this.Series.FindByName(seriseName);
            if (series == null)
            {
                series = AddSeries(seriseName);
            }

            SetData(series, data);
        }

        private delegate Series AddSeriesDelegate(string seriseName);
        private Series AddSeries(string seriseName)
        {
            if (InvokeRequired)
            {
                return (Series)this.Invoke(new AddSeriesDelegate(AddSeries), seriseName);
            }

            Series series;
            this.Series.Add(series = new System.Windows.Forms.DataVisualization.Charting.Series(seriseName));
            series.ChartType = SeriesChartType.Line;
            return series;
        }

        internal void SetData(float[] data)
        {
            Series s = this.Series.FirstOrDefault();
            SetData(s, data);
        }

        private delegate void SetDataDelegate(Series series, float[] data);
        private void SetData(Series series, float[] data)
        {
            if(InvokeRequired)
            {
                this.Invoke(new SetDataDelegate(SetData), series, data);
                return;
            }

            series.Points.Clear();
            if (!IsRotate90)
            {
                series.Points.DataBindY(data);
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                    series.Points.AddXY(data[i], i);
            }
            SetAxis(series);
        }

        public void FitAxis()
        {
            double m = 0;
            double M = 1;
            if (this.Series.Count > 0)
            {
                m = this.Series.Min(f => f.Points.Count == 0 ? 0 : f.Points.Min(g => IsRotate90 ? g.XValue : g.YValues.Min()));
                M = this.Series.Min(f => f.Points.Count == 0 ? 0 : f.Points.Max(g => IsRotate90 ? g.XValue : g.YValues.Max()));
            }
            SetAxis(m, M);
        }

        public void SetAxis(double small, double large)
        {
            ChartArea chartArea = this.ChartAreas.FirstOrDefault();
            if (chartArea == null)
                return;

            chartArea.AlignmentStyle = AreaAlignmentStyles.AxesView;

            chartArea.AxisX.IsMarginVisible = false;
            chartArea.AxisY.IsMarginVisible = false;
            Axis axisX = IsRotate90 ? chartArea.AxisY : chartArea.AxisX;
            axisX.Minimum = Math.Min(small, large);
            axisX.Maximum = Math.Max(small, large);
        }

        private void SetAxis(Series series)
        {
            int yStep = 256;
            ChartArea chartArea = this.ChartAreas[series.ChartArea];

            chartArea.AxisY.IsReversed = IsRotate90;
            Axis axisX = IsRotate90 ? chartArea.AxisY : chartArea.AxisX;
            Axis axisY = IsRotate90 ? chartArea.AxisX : chartArea.AxisY;

            axisX.LabelStyle.Enabled = Zoomable;
            axisX.LabelStyle.Format = "D";
            axisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            axisX.Minimum = 0;
            axisX.Maximum = series.Points.Count;

            double m = series.Points.Min(f => IsRotate90 ? f.XValue : f.YValues.Min());
            double M = series.Points.Max(f => IsRotate90 ? f.XValue : f.YValues.Max());
            int yMin = (int)(m / yStep - 1) * yStep;
            int yMax = (int)(M / yStep + 1) * yStep-1;
            axisY.LabelStyle.Enabled = false;
            axisY.IntervalAutoMode = IntervalAutoMode.FixedCount;
            axisY.Minimum = yMin;
            axisY.Maximum = yMax;

            chartArea.InnerPlotPosition.Auto = this.Zoomable;
            if (!chartArea.InnerPlotPosition.Auto)
            {
                chartArea.InnerPlotPosition.X = chartArea.InnerPlotPosition.Y = 0;
                chartArea.InnerPlotPosition.Width = chartArea.InnerPlotPosition.Height = 100;
            }

            chartArea.Position.Auto = true;
            chartArea.Position.X = chartArea.Position.Y = 0;
            chartArea.Position.Width = 100;
            chartArea.Position.Height = 100;
            chartArea.BorderWidth = 0;
        }

        internal void Clear()
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(Clear));
                return;
            }

            foreach (Series s in this.Series)
                s.Points.Clear();
            //FitAxis();
        }
    }
}
