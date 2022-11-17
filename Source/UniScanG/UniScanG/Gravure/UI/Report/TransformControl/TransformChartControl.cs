using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.UI;
using UniScanG.Gravure.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace UniScanG.Gravure.UI.Report.TransformControl
{
    public partial class TransformChartControl : UserControl, IMultiLanguageSupport, ITransformControl
    {
        public bool UseOffsetScale { get; set; } = true;
        public float OffsetScale { get; set; } = 20;
        public bool ShowGuildCircle { get; set; } = true;
        public float GuideCircleSize { get; set; } = 500;
        public bool DrawLines { get; set; } = true;
        public bool ShowLabels { get; set; } = true;

        Tuple<PointF, SizeF>[][] pointss = new Tuple<PointF, SizeF>[0][];
        RectangleF axisRange = RectangleF.Empty;

        public TransformChartControl()
        {
            InitializeComponent();
            StringManager.AddListener(this);

            this.Dock = DockStyle.Fill;

            this.chart.ChartAreas[0].AxisX.StripLines.Add(new System.Windows.Forms.DataVisualization.Charting.StripLine() { Interval = 110, IntervalOffset = 0, BorderWidth = 2, BorderColor = Color.Black});
            this.chart.ChartAreas[0].AxisX.LabelStyle.Format = "{0.0}";
            this.chart.ChartAreas[0].AxisX.Interval = 110/ 4f;

            this.chart.ChartAreas[0].AxisY.StripLines.Add(new System.Windows.Forms.DataVisualization.Charting.StripLine() { Interval = 110, IntervalOffset = 0, BorderWidth = 2, BorderColor = Color.Black });
            this.chart.ChartAreas[0].AxisY.LabelStyle.Format = "{0.0}";
            this.chart.ChartAreas[0].AxisY.Interval = 110/ 4f;

        }

        private void TransformChartControl_Load(object sender, EventArgs e)
        {
            this.useScale.DataBindings.Add(new Binding("Checked", this, "UseOffsetScale"));

            //this.offsetScale.Minimum = 1;
            this.offsetScale.Maximum = decimal.MaxValue;
            this.offsetScale.DataBindings.Add(new Binding("Value", this, "OffsetScale"));

            this.drawLines.DataBindings.Add(new Binding("Checked", this, "DrawLines"));

            this.showGuideCircle.DataBindings.Add(new Binding("Checked", this, "ShowGuildCircle", false, DataSourceUpdateMode.OnPropertyChanged));

            //this.guideCircleSize.Minimum = 1;
            this.guideCircleSize.Maximum = decimal.MaxValue;
            this.guideCircleSize.DataBindings.Add(new Binding("Value", this, "GuideCircleSize"));
        }

        public void ClearAll()
        {
            throw new NotImplementedException();
        }

        public void Update(List<Tuple<CalcResult, OffsetObj[]>> tupleList, float baseCircleRadUm)
        {
            SizeF pelPerUmSize = UpdateValue(tupleList.Select(f => f.Item1).ToArray());
            SizeF pelPerMmSize= DrawingHelper.Div(pelPerUmSize, 1000f);
            UpdateChart(tupleList.Select(f => f.Item2).ToList(), pelPerMmSize);
        }

        private void UpdateChart(List<OffsetObj[]> list, SizeF pelSize)
        {
            if (list.Count == 0 || list.Sum(f => f.Length) == 0)
                return;

            // 전체 차트영역 범위
            RectangleF axisRange = list
                .SelectMany(f => f.Select(g => DrawingHelper.FromCenterSize(g.RefPoint, new SizeF(Math.Abs(g.MatchingOffsetPx.Width) * 2, Math.Abs(g.MatchingOffsetPx.Height) * 2))))
                .Aggregate((f, g) => RectangleF.Union(f, g));
            PointF offset = DrawingHelper.CenterPoint(axisRange);
            axisRange.Offset(-offset.X, -offset.Y);

            float targetSize = Math.Max(axisRange.Width, axisRange.Height) * 1.2f;
            SizeF inflate = new SizeF((targetSize - axisRange.Width) / 2, (targetSize - axisRange.Height) / 2);
            axisRange.Inflate(inflate);
            axisRange = DrawingHelper.Mul(axisRange, pelSize);

            // 점들...
            Tuple<PointF, SizeF>[][] pointss = list.Select(f =>
             {
                 Tuple<PointF, SizeF>[] tuples = f.Select(g =>
                 {
                     PointF basePoint = DrawingHelper.Subtract(g.RefPoint, offset);
                     SizeF dataSize = g.MatchingOffsetPx;
                     return new Tuple<PointF, SizeF>(DrawingHelper.Mul(basePoint, pelSize), DrawingHelper.Mul(dataSize, pelSize));
                 }).OrderBy(g => Math.Atan2(g.Item1.Y, g.Item1.X)).ToArray();
                 return tuples;
             }).ToArray();

            this.pointss = pointss;
            this.axisRange = axisRange;
            UpdateChart();
        }

        private void UpdateChart()
        {
            if(this.chart.InvokeRequired)
            {
                this.chart.BeginInvoke(new MethodInvoker(UpdateChart));
                return;
            }

            //this.chart.Series[0].Points.Clear();
            //this.chart.Series[1].Points.Clear();

            this.chart.ChartAreas[0].AxisX.Minimum = axisRange.Left;
            this.chart.ChartAreas[0].AxisX.Maximum = axisRange.Right;
            this.chart.ChartAreas[0].AxisY.Minimum = axisRange.Top;
            this.chart.ChartAreas[0].AxisY.Maximum = axisRange.Bottom;

            this.chart.Series.Clear();

            if (this.pointss.Length > 0)
            {
                // Base Points
                Tuple<PointF, SizeF>[] bases = this.pointss[0];
                PointF[] basePoints = bases.Select(f => f.Item1).ToArray();
                float[] baseX = basePoints.Select(f => f.X).ToArray();
                float[] baseY = basePoints.Select(f => f.Y).ToArray();
                Series seriesBase = new Series()
                {
                    ChartType = SeriesChartType.Point,
                    Color = Color.Red,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerBorderWidth = 1,

                    MarkerColor = GetBaseColor(),
                    MarkerSize = 5,
                    MarkerBorderColor = GetBaseColor()
                };
                seriesBase.Points.DataBindXY(baseX, baseY);
                this.chart.Series.Add(seriesBase);

                // Data Points
                foreach (Tuple<PointF, SizeF>[] points in this.pointss)
                {
                    int idx = Array.IndexOf(this.pointss, points);
                    Series series = new Series()
                    {
                        ChartType = SeriesChartType.Point,
                        Color = Color.Red,
                        MarkerStyle = MarkerStyle.Circle,
                        MarkerBorderWidth = 1,

                        MarkerColor = GetDataColor(idx),
                        MarkerSize = 8,
                        MarkerBorderColor = GetDataColor(idx)
                    };

                    PointF[] dataPoints = points.Select(f => PointF.Add(f.Item1, DrawingHelper.Mul(f.Item2, (this.UseOffsetScale ? this.OffsetScale : 1)))).ToArray();
                    float[] dataX = dataPoints.Select(f => f.X).ToArray();
                    float[] dataY = dataPoints.Select(f => f.Y).ToArray();
                    series.Points.DataBindXY(dataX, dataY);

                    if (this.ShowLabels)
                    {
                        float[] dL = points.Select(f => (float)(Math.Sqrt(Math.Pow(f.Item2.Width, 2) + Math.Pow(f.Item2.Height, 2)) * 1000)).ToArray();
                        for (int i = 0; i < series.Points.Count; i++)
                        {
                            series.Points[i].IsValueShownAsLabel = true;
                            series.Points[i].Label = dL[i].ToString("F01");
                        }
                    }
                    this.chart.Series.Add(series);
                }
            }

            double intervalX = this.chart.ChartAreas[0].AxisX.Interval;
            this.chart.ChartAreas[0].AxisX.IntervalOffset = -((axisRange.Left / intervalX) - (int)(axisRange.Left / intervalX)) * intervalX;
            this.chart.ChartAreas[0].AxisX.StripLines[0].IntervalOffset = -((axisRange.Left / 110) - (int)(axisRange.Left / 110)) * 110;

            double intervalY = this.chart.ChartAreas[0].AxisY.Interval;
            this.chart.ChartAreas[0].AxisY.IntervalOffset = +((axisRange.Bottom / intervalY) - (int)(axisRange.Bottom / intervalY)) * intervalY;
            this.chart.ChartAreas[0].AxisY.StripLines[0].IntervalOffset = +((axisRange.Bottom / 110) - (int)(axisRange.Bottom / 110)) * 110;

            this.chart.Invalidate();
        }

        private Color GetBaseColor()
        {
            return Color.Red;
        }

        private Color GetDataColor(int idx)
        {
            switch(idx)
            {
                case 0:
                    return Color.Orange;
                case 1:
                    return Color.Green;
                case 2:
                    return Color.Blue;
                case 3:
                    return Color.Navy;
                case 4:
                    return Color.Purple;
                default:
                    return Color.DarkSlateGray;
            }
        }

        public void UpdateImage(Bitmap bitmap)
        {
            
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        public SizeF UpdateValue(CalcResult[] calcResults)
        {
            CalcResult calcResult = new CalcResult();
            if (calcResults != null && calcResults.Length > 0)
                calcResult = CalcResult.GetAverage(calcResults);

            UiHelper.SetControlText(this.translationX, calcResult.dX.ToString("F2"));
            UiHelper.SetControlText(this.translationY, calcResult.dY.ToString("F2"));
            UiHelper.SetControlText(this.rotation, calcResult.dT.ToString("F2"));
            UiHelper.SetControlText(this.skewnessLtrb, calcResult.dL1.ToString("F2"));
            UiHelper.SetControlText(this.skewnessRtlb, calcResult.dL2.ToString("F2"));
            UiHelper.SetControlText(this.sizeW, calcResult.dW.ToString("F2"));
            UiHelper.SetControlText(this.sizeH, calcResult.dH.ToString("F2"));
            return calcResult.pelSize;
        }

        public void ZoomFit()
        {

        }

        private void chart_Paint(object sender, PaintEventArgs e)
        {
            ChartArea ca = this.chart.ChartAreas[0];
            float guideLength = this.GuideCircleSize / 1000;
            float radX = (float)Math.Abs((ca.AxisX.ValueToPixelPosition(guideLength) - ca.AxisX.ValueToPixelPosition(0)));
            float radY = (float)Math.Abs((ca.AxisY.ValueToPixelPosition(guideLength) - ca.AxisY.ValueToPixelPosition(0)));
            SizeF markerRadPx = DrawingHelper.Mul(new SizeF(radX, radY), 2 * (this.UseOffsetScale ? this.OffsetScale : 1));

            if (this.DrawLines)
            {
                PointF drawCenter = DrawingHelper.CenterPoint(e.Graphics.VisibleClipBounds);
                PointF baseLine = PointF.Add(drawCenter, new Size(1, 0));
                foreach (Series series in this.chart.Series)
                {
                    List<PointF> pointList = series.Points.Select(f => PointFromDataPoint(f)).ToList();
                    //Dictionary<PointF, double> dic = pointList.ToDictionary(f => f, f => MathHelper.GetAngle360(drawCenter, baseLine, f));
                    //PointF[] points = dic.OrderBy(f => f.Value).Select(f => f.Key).ToArray();
                    PointF[] points = pointList.ToArray();

                    //series.MarkerColor = Color.Blue;
                    //series.MarkerSize = 10;
                    //series.MarkerBorderWidth = 1;
                    Pen pen = new Pen(series.MarkerColor, series.MarkerSize / 3);
                    e.Graphics.DrawPolygon(pen, points);
                }
            }

            if (this.ShowGuildCircle)
            {
                Series series = this.chart.Series.FirstOrDefault();
                if (series != null)
                {
                    PointF[] basePoints = series.Points.Select(f => PointFromDataPoint(f)).ToArray();
                    Array.ForEach(basePoints, f => e.Graphics.DrawEllipse(Pens.Blue, DrawingHelper.FromCenterSize(f, markerRadPx)));
                }
            }
        }

        private PointF PointFromDataPoint(DataPoint pt)
        {
            Axis ax = this.chart.ChartAreas[0].AxisX;
            Axis ay = this.chart.ChartAreas[0].AxisY;
            float x = (float)ax.ValueToPixelPosition(pt.XValue);
            float y = (float)ay.ValueToPixelPosition(pt.YValues[0]);

            return new PointF(x, y);
        }

        private void showGuideCircle_CheckedChanged(object sender, EventArgs e)
        {
            this.ShowGuildCircle = this.showGuideCircle.Checked;
            UpdateChart();
        }

        private void guideCircleSize_ValueChanged(object sender, EventArgs e)
        {
            this.GuideCircleSize = (float)this.guideCircleSize.Value;
            UpdateChart();
        }

        private void useScale_CheckedChanged(object sender, EventArgs e)
        {
            this.UseOffsetScale = this.useScale.Checked;
            UpdateChart();
        }

        private void offsetScale_ValueChanged(object sender, EventArgs e)
        {
            this.OffsetScale = (float)this.offsetScale.Value;
            UpdateChart();
        }

        private void drawLines_CheckedChanged(object sender, EventArgs e)
        {
            this.DrawLines = this.drawLines.Checked;
            this.chart.Invalidate(false);
        }

        private void showLabels_CheckedChanged(object sender, EventArgs e)
        {
            this.ShowLabels = showLabels.Checked;
            UpdateChart();
        }
    }
}
