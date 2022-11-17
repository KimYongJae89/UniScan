using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Windows.Forms.DataVisualization.Charting;
using DynMvp.Base;
using DynMvp.UI;

namespace UniScanM.UI.Graph
{
    public partial class ProfilePanel : UserControl, IMultiLanguageSupport
    {
        List<Tuple<StripLine, StripProperty>> stripTupleList = null;
        List<Tuple<Series, SeriesProperty>> seriesTupleList = null;
        List<PointF> pointList = new List<PointF>();

        ProfileOption profileOption;
        string title;

        public ProfilePanel(string title, ProfileOption profileOption)
        {
            InitializeComponent();

            this.title = title;
            this.profileOption = profileOption;

            Initialize();

            StringManager.AddListener(this);
        }

        public void Initialize()
        {
            chart.BackColor = this.profileOption.BackColor;

            Axis xAxis = new Axis(chart.ChartAreas[0], AxisName.X);
            UpdateAxis(xAxis, this.profileOption.AxisX);
            chart.ChartAreas[0].AxisX = xAxis;

            Axis yAxis = new Axis(chart.ChartAreas[0], AxisName.Y);
            UpdateAxis(yAxis, this.profileOption.AxisX);
            chart.ChartAreas[0].AxisY = yAxis;

            this.stripTupleList = new List<Tuple<StripLine, StripProperty>>();
            foreach (StripProperty stripProperty in this.profileOption.StripPropertyCollection)
                this.stripTupleList.Add(new Tuple<StripLine, StripProperty>(new StripLine(), stripProperty));
            this.stripTupleList.ForEach( f =>UpdateStripLine(f.Item1,f.Item2));
            this.stripTupleList.ConvertAll(f => f.Item1).ForEach(f => chart.ChartAreas[0].AxisY.StripLines.Add(f));


            this.seriesTupleList = new List<Tuple<Series, SeriesProperty>>();
            foreach (SeriesProperty seriesProperty in this.profileOption.SeriesPropertyCollection)
                this.seriesTupleList.Add(new Tuple<Series, SeriesProperty>(new Series(), seriesProperty));
            this.seriesTupleList.ForEach(f => UpdateSeries(f.Item1, f.Item2));

            this.seriesTupleList.ConvertAll(f => f.Item1).ForEach(f => chart.Series.Add(f));
            chart.DataSource = this.pointList;
            chart.DataBind();
        }

        private void UpdateSeries(Series series, SeriesProperty seriesProperty)
        {
            series.XValueMember = "X";
            series.YValueMembers = "Y";

            series.BorderWidth = (int)seriesProperty.Thickness;
            //series.BorderColor = seriesProperty.Color;
            series.Color = seriesProperty.Color;

            series.ChartType = (SeriesChartType)seriesProperty.ChartType;

            series.LegendText = seriesProperty.Name;
        }

        private void UpdateStripLine(StripLine stripLine, StripProperty stripProperty)
        {
            stripLine.Interval = 0;
            stripLine.StripWidth = 0;
            stripLine.BorderColor = stripProperty.Color;
            stripLine.BorderWidth = stripProperty.Visiblity ? (int)stripProperty.Thickness : 0;
            stripLine.IntervalOffset = stripProperty.Offset;
        }

        private void UpdateAxis(Axis axis, AxisProperty axisProperty)
        {
            if (axisProperty.AutoScale)
            {
                if (axis.ScaleView.IsZoomed)
                {
                    axis.ScaleView.ZoomReset();
                    axis.ScaleView.SmallScrollSize = double.NaN;
                }

                axis.IntervalAutoMode = IntervalAutoMode.VariableCount;
                axis.Maximum = double.NaN;
                axis.Minimum = double.NaN;
            }
            else
            {
                //if (!axis.ScaleView.IsZoomed)
                {
                    double size = axisProperty.Length;
                    double pos = axis.Maximum - axisProperty.Length;
                    double rate = axis.ScaleView.ViewMaximum / axis.Maximum * 100;
                    //System.Diagnostics.Debug.WriteLine(rate.ToString());
                    if (!axis.ScaleView.IsZoomed || rate > 95)
                        axis.ScaleView.Zoom(pos, pos + size);
                    axis.ScaleView.SmallScrollSize = size;
                    axis.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
                }

                axis.IntervalAutoMode = IntervalAutoMode.VariableCount;
                //axis.Maximum = axisProperty.Maximum;
                //axis.Minimum = axisProperty.Minimum;

            }

            axis.Interval = axisProperty.Interval;
            axis.MaximumAutoSize = 50;
            axis.LabelStyle.Format = string.Format("F{0}", axisProperty.Decimals);

            axis.IsMarginVisible = false;

            axis.IsLabelAutoFit = true;
            axis.LabelAutoFitMaxFontSize = 10;
            axis.LabelAutoFitMinFontSize = 5;
            axis.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep45;
            axis.LabelStyle.IsStaggered = false;
            axis.LabelStyle.IsEndLabelVisible = false;
            axis.LabelStyle.TruncatedLabels = true;
            axis.IsInterlaced = true;
            //axis.IsInterlaced = axis.AxisName == AxisName.Y;

            axis.LineWidth = (int)(axisProperty.Thickness);
            axis.LabelStyle.ForeColor = axisProperty.Color;
            axis.LineColor = axisProperty.Color;
            axis.InterlacedColor = Color.FromArgb(64, axisProperty.Color);
            axis.MajorGrid.LineWidth = 1;
            axis.MajorGrid.LineColor = Color.FromArgb(128, axisProperty.Color);

            axis.Title = axisProperty.Name;
            axis.TitleFont = new Font("Arial", 10, FontStyle.Bold);
            axis.TitleForeColor = axisProperty.Color;

            axis.Enabled = axisProperty.Visiblity ? AxisEnabled.True : AxisEnabled.False;
            axis.MajorGrid.Enabled = axisProperty.Visiblity;

            axis.IsReversed = axisProperty.Invert;
        }

        public void AddValue(int series, float x, float y)
        {
            this.pointList.Add(new PointF(x, y));
            //DataPoint dataPoint = new DataPoint(x, y);
            //AddValue(series, dataPoint);
            DisplayResult();
        }

        public void AddValue(int series, DataPoint dataPoint)
        {
            this.pointList.Add(new PointF((float)dataPoint.XValue, (float)dataPoint.YValues[0]));
            //this.series[series].Points.AddXY(dataPoint.XValue, dataPoint.YValues);
            DisplayResult();
        }

        delegate void DisplayResultDelegate();
        public void DisplayResult()
        {
            if (InvokeRequired)
            {
                Invoke(new DisplayResultDelegate(DisplayResult));
                return;
            }

            try
            {
                UpdateAxis(this.chart.ChartAreas[0].AxisX, this.profileOption.AxisX);
                UpdateAxis(this.chart.ChartAreas[0].AxisY, this.profileOption.AxisY);

                foreach (Tuple<StripLine, StripProperty> tuple in this.stripTupleList)
                    UpdateStripLine(tuple.Item1, tuple.Item2);

                foreach (Tuple<Series, SeriesProperty> tuple in this.seriesTupleList)
                    UpdateSeries(tuple.Item1, tuple.Item2);

                chart.DataBind();

                this.BackColor = this.profileOption.BackColor;
                this.chart.BackColor = this.profileOption.BackColor;
                this.chart.ChartAreas[0].BackColor = this.profileOption.BackColor;
                //chart.Invalidate(true);
                DisplayLabel();
            }
            catch { }
        }

        delegate void DisplayLabelDelegate();
        private void DisplayLabel()
        {
            if (InvokeRequired)
            {
                Invoke(new DisplayLabelDelegate(DisplayLabel));
                return;
            }

            Label[] labels = new Label[] { txtMin, txtMax, txtAvg, txtCur, txtStd, txtDiff };

            layoutMain.SuspendLayout();
            Array.ForEach(labels, f => UiHelper.SuspendDrawing(f));

            if (this.pointList.Count() > 0)
            {
                double min = Math.Round(this.pointList.Min(scanData => scanData.Y), 2, MidpointRounding.AwayFromZero);
                double max = Math.Round(this.pointList.Max(scanData => scanData.Y), 2, MidpointRounding.AwayFromZero);
                double[] values = new double[6]
                {
                    min,max,
                    Math.Round(this.pointList.Average(f => f.Y),2),
                    Math.Round(this.pointList.Last().Y,2),
                    Math.Round(GetStdDev(this.pointList),2),
                    Math.Round(max-min,2)
                };

                for (int i = 0; i < labels.Length; i++)
                    labels[i].Text = values[i].ToString();
            }
            else
            {
                float value = 0;
                Array.ForEach(labels, f => f.Text = value.ToString());
            }

            Array.ForEach(labels, f => UiHelper.ResumeDrawing(f));
            layoutMain.ResumeLayout();
        }

        private double GetStdDev(List<PointF> displayScanDataList)
        {
            if (displayScanDataList.Count < 1)
                return 0;

            double var = 0f;
            float average = displayScanDataList.Average(scanData => scanData.Y);

            foreach (PointF scanData in displayScanDataList)
            {
                var += Math.Pow((double)(scanData.Y - average), 2);
            }

            return (float)Math.Sqrt(var / (double)displayScanDataList.Count);
        }

        public void ClearPanel()
        {
            this.pointList.Clear();

            DisplayResult();
            DisplayLabel();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            this.labelTitle.Text = StringManager.GetString(this.GetType().FullName, this.title);
        }
    }
}
