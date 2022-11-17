using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DynMvp.Base;
using UniScanG.UI;
using DynMvp.InspData;
using DynMvp.UI;
using UniScanG.Gravure.Inspect;

namespace UniScanG.Gravure.UI.Inspect
{
    public partial class LengthChart : UserControl, IInspectExtraPanel, IMultiLanguageSupport
    {
        public SortedList<int, double> DataSource { get; } = new SortedList<int, double>();
        Series series;

        int intervalCount = 1;
        double averageSlope = 0;

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public LengthChart()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            this.series = new Series
            {
                Name = "Length",
                Color = Color.Red,
                MarkerStyle = MarkerStyle.Circle,
                MarkerSize = 5,
                BorderWidth = 3,
                BorderDashStyle = ChartDashStyle.Solid,
                ChartType = SeriesChartType.Line
            };
            this.chartLength.Series.Add(series);

        
            StringManager.AddListener(this);
        }

        public delegate void UpdateResultDelegate(InspectionResult inspectionResult);
        public void UpdateResult(InspectionResult inspectionResult)
        {
            if(InvokeRequired)
            {
                Invoke(new UpdateResultDelegate(UpdateResult), inspectionResult);
                return;
            }

            if (inspectionResult == null)
            {
                this.DataSource.Clear();
                this.series.Points.Clear();
                UpdateAxis();
#if DEBUG

                // test
                Random random = new Random((int)DateTime.Now.Ticks);
                for (int i = 0; i < 50; i++)
                {
                    double y = 240.7 + ((random.NextDouble() - 0.5) * 0.8);
                    //double y = 240.7 + i*0.1;
                    //AddPoint(i, y);
                }
                UpdateAxis();

#endif
                return;
            }

            if (!int.TryParse(inspectionResult.InspectionNo, out int patternNo))
                return;

            if (inspectionResult.AlgorithmResultLDic.ContainsKey(Vision.Detector.Detector.TypeName))
            {
                Vision.Detector.DetectorResult detectorResult = inspectionResult.AlgorithmResultLDic[Vision.Detector.Detector.TypeName] as Vision.Detector.DetectorResult;
                if (detectorResult != null)
                {
                    AddPoint(patternNo, detectorResult.SheetSize.Height);
                    return;
                }
            }

            if (inspectionResult.AlgorithmResultLDic.ContainsKey(UniScanG.Data.SheetCombiner.TypeName))
            {
                UniScanG.Data.MergeSheetResult mergeSheetResult = inspectionResult.AlgorithmResultLDic[UniScanG.Data.SheetCombiner.TypeName] as UniScanG.Data.MergeSheetResult;
                if (mergeSheetResult != null)
                {
                    AddPoint(patternNo, mergeSheetResult.SheetSize.Height);
                    return;
                }
            }
        }

        delegate void AddPointDelegate(int x, double y);
        private void AddPoint(int x, double y)
        {
            if (this.InvokeRequired)
            {
                Invoke(new AddPointDelegate(AddPoint), x, y);
                return;
            }

            this.DataSource.Add(x, y);

            int[] xs = this.DataSource.Select(f => f.Key).ToArray();
            double[] ys = this.DataSource.Select(f => f.Value).ToArray();
            this.series.Points.DataBindXY(xs, ys);

            //List<DataPoint> list = series.Points.ToList();
            //list.Add(new DataPoint(x, y));
            //list.Sort((f, g) => f.XValue.CompareTo(g.XValue));

            //series.Points.Clear();
            //series.Points.DataBind()

            //series.Points.AddXY(x, y);

            UpdateSlope();
            UpdateAxis();
            UpdateValues();
        }

        private void UpdateValues()
        {
            AlarmManager alarmManager = SystemManager.Instance().InspectRunnerG.AlarmManager;
            UiHelper.SetControlText(this.average, alarmManager.Mean.ToString("F3"));
            UiHelper.SetControlText(this.stdDev, alarmManager.StdDev.ToString("F3"));
            UiHelper.SetControlText(this.min, alarmManager.Min.ToString("F2"));
            UiHelper.SetControlText(this.max, alarmManager.Max.ToString("F2"));
            UiHelper.SetControlText(this.range, alarmManager.Range.ToString("F2"));
        }

        private void chartLength_SizeChanged(object sender, EventArgs e)
        {
            if (this.Height == 0)
                return;

            this.intervalCount = Math.Max(1, this.Height / 100);
            UpdateAxis();
        }

        public void UpdateVisible()
        {
            throw new NotImplementedException();
        }

        private void LengthChart_Load(object sender, EventArgs e)
        {
            this.chartLength.ChartAreas[0].AxisY.LabelStyle.Format = "F1";
            this.chartLength.ChartAreas[0].AxisX.Minimum = 0;
        }

        private void UpdateSlope()
        {
            if (this.DataSource.Count < 10)
            {
                this.averageSlope = double.NaN;
                return;
            }

            IEnumerable<PointF> points = this.DataSource.Select(f => new PointF((float)f.Key, (float)(f.Value * 1000)));
            MathHelper.Regression1(points, out double coff1, out double coff0);
            this.averageSlope = (float)coff1;
        }

        private void UpdateAxis()
        {
            double yMin = 0;
            double yMax = 1;
            if (this.DataSource.Count > 0)
            {
                yMin = Math.Floor(this.DataSource.Min(f => f.Value) * 10) / 10;
                yMax = Math.Ceiling(this.DataSource.Max(f => f.Value) * 10) / 10;
            }

            double diff = yMax - yMin;
            if (diff < 0.4)
            {
                double mean = (yMax + yMin) / 2;
                yMin = mean - 0.2;
                yMax = mean + 0.2;
            }

            double center = Math.Round((yMax + yMin) / 2*10);
            int mod = ((int)center) % 5;
            if (mod < 3)
                center = center - mod;
            else
                center = center + (5 - mod);
            center /= 10;

            //center = Math.Round((yMax + yMin) / 2, 1);

            double interval = (yMax - yMin) / this.intervalCount;
            double minorInterval = 0;
            if (interval < 0.05)
            {
                interval = 0.05;
                minorInterval = 0;
            }
            else if (interval < 0.1)
            {
                interval = 0.1;
                minorInterval = 0.02;
            }
            else
            {
                interval = Math.Ceiling(interval * 10) / 10.0;
                minorInterval = interval / (int)Math.Round(interval / 0.1);
            }
            this.chartLength.ChartAreas[0].AxisY.Interval = interval;
            this.chartLength.ChartAreas[0].AxisY.Minimum = center - (interval * (this.intervalCount) /2);
            this.chartLength.ChartAreas[0].AxisY.Maximum = center + (interval * (this.intervalCount) /2);

            //this.chartLength.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
            this.chartLength.ChartAreas[0].AxisY.MinorGrid.Enabled = (minorInterval>0);
            this.chartLength.ChartAreas[0].AxisY.MinorGrid.Interval = minorInterval;
            this.chartLength.ChartAreas[0].AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dot;

            double xMin = 0;
            double xMax = 1;
            if (this.DataSource.Count > 0)
            {
                xMin = this.DataSource.Min(f => f.Key);
                xMax = this.DataSource.Max(f => f.Key);
            }
            this.chartLength.ChartAreas[0].AxisX.Minimum = Math.Floor(xMin / 10) * 10;
            this.chartLength.ChartAreas[0].AxisX.Maximum = Math.Floor(xMax / 10 + 1) * 10;
        }

        private void labelTrandChartDefect_SizeChanged(object sender, EventArgs e)
        {
            //Console.WriteLine($"labelTrandChartDefect_SizeChanged - {labelTrandChartDefect.Size}");
        }

        private void chartLength_MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = e.Location;
            HitTestResult hitTestResult = this.chartLength.HitTest(pos.X, pos.Y);
            if (hitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                DataPoint dp = series.Points[hitTestResult.PointIndex];
                series.ToolTip = $"Pattern: {dp.XValue}{Environment.NewLine}Length: {dp.YValues.FirstOrDefault():F2}[mm]";
            }
            else
            {
                series.ToolTip = "";
            }
        }

        private void chartLength_Paint(object sender, PaintEventArgs e)
        {
            if (double.IsNaN(this.averageSlope) || double.IsInfinity(this.averageSlope))
                return;
            int count = 100;
            Font font = chartLength.ChartAreas[0].AxisX.TitleFont;
            e.Graphics.DrawString($"{this.averageSlope * count:F0}[um] per {count} patterns", font, new SolidBrush(Color.Black), new Point(0, this.chartLength.Height - 20));
        }
    }
}
