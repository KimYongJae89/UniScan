using DynMvp.Base;
using DynMvp.UI;
using Infragistics.Win.DataVisualization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UniScanM.Gloss.Data;
using UniScanM.Gloss.MachineIF;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss.UI.Chart
{
    public partial class TrendPanel : UserControl, IMultiLanguageSupport
    {
        #region 필드
        #endregion

        #region 생성자
        public TrendPanel(string title, bool isReport, ChartSetting chartSetting = null, ChartOption chartOption = null)
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;
            this.ChartSetting = chartSetting;
            this.Title = title;
            this.IsReport = isReport;
            this.ChartOption = chartOption == null ? new ChartOption() : chartOption;
            Initialize(chartSetting);

            StringManager.AddListener(this);
        }
        #endregion

        #region 소멸자
        #endregion

        #region 대리자
        #endregion

        #region 이벤트
        private void ultraButtonZoomReset_Click(object sender, EventArgs e)
        {
            profileChart.ResetZoom();
        }

        private string FormatLabelX(AxisLabelInfo info)
        {
            var value = info.Value;
            return string.Format("{0:0}", value);
        }

        private string FormatLabelY(Infragistics.Win.DataVisualization.AxisLabelInfo info)
        {
            var value = info.Value;
            return string.Format("{0:0.000}", value);
        }

        private string FormatLabelYSpace(Infragistics.Win.DataVisualization.AxisLabelInfo info)
        {
            return string.Format("");
        }
        #endregion

        #region 열거형
        #endregion

        #region 인터페이스
        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
            this.labelTitle.Text = StringManager.GetString(this.GetType().FullName, this.Title);
        }
        #endregion

        #region 속성
        private List<GlossScanData> RangeColumnScanDataList { get; set; } = new List<GlossScanData>();

        private List<GlossScanData> LineScanDataList { get; set; } = new List<GlossScanData>();

        private RangeColumnSeries RangeColumn { get; set; }

        private ScatterLineSeries SeriesLine { get; set; }

        private ScatterLineSeries SeriesCenterLine { get; set; }

        private ScatterLineSeries SeriesUpperStop { get; set; }

        private ScatterLineSeries SeriesLowerStop { get; set; }

        private ScatterLineSeries SeriesUpperWarning { get; set; }

        private ScatterLineSeries SeriesLowerWarning { get; set; }

        private ChartSetting ChartSetting { get; set; }

        private ChartOption ChartOption { get; set; }

        private PLCInspectStarter PLCInspectStarter { get => SystemManager.Instance().InspectStarter as UniScanM.Gloss.MachineIF.PLCInspectStarter; }

        private float GraphScale { get; set; } = 0.7f;

        private string Title { get; set; }

        private bool IsReport { get; set; }

        private float CurrentReelPosition { get; set; }
        #endregion

        #region 인덱서
        #endregion

        #region 메서드
        public void ClearPanel()
        {
            RangeColumnScanDataList.Clear();
            LineScanDataList.Clear();
            DisplayResult();
        }

        public void Initialize(ChartSetting chartSetting = null)
        {
            profileChart.Axes.Clear();
            profileChart.Series.Clear();

            labelChart.Axes.Clear();
            labelChart.Series.Clear();

            CurrentReelPosition = 0;

            if (chartSetting == null)
            {
                chartSetting = this.ChartSetting.Clone();
            }
            else
            {
                GlossSettings tempSetting = (GlossSettings)GlossSettings.Instance();

                chartSetting.AxisColor = tempSetting.AxisColor;
                chartSetting.BackColor = tempSetting.BackColor;
                chartSetting.GraphColor = tempSetting.GraphColor;
                chartSetting.LineStopColor = tempSetting.LineStopColor;
            }

            int endPos = 10000;
            if (this.IsReport == false)
            {
                //if (PLCInspectStarter != null && PLCInspectStarter.MelsecMonitor.State.IsConnected == true)
                //    endPos = SystemManager.Instance().InspectStarter.GetPosition();
                //else
                endPos = 10000;
            }
            else
            {
                //endPos = indexData.TotalDistance;
                if (endPos < 10000)
                    endPos = 10000;
            }

            profileChart.Size = new Size((int)((float)endPos * GraphScale) + 20, chartPanel.Size.Height - 18);
            if (chartPanel.HorizontalScroll.Visible == true)
                labelChart.Size = new Size(65, chartLabelPanel.Size.Height - 36);
            else
                labelChart.Size = new Size(65, chartLabelPanel.Size.Height - 18);

            profileChart.PlotAreaBackground = new SolidBrush(chartSetting.BackColor);

            // 차트 왼쪽의 Y 축 라벨 값을 표시 하기 위한 그래프
            NumericYAxis lineYAxisLabel = new NumericYAxis();
            lineYAxisLabel.MaximumValue = Math.Abs(this.ChartSetting.YAxisRange);
            lineYAxisLabel.MinimumValue = -Math.Abs(this.ChartSetting.YAxisRange);
            lineYAxisLabel.Interval = (Math.Abs(this.ChartSetting.YAxisRange) * 2.0) / this.ChartSetting.YAxisInterval;
            lineYAxisLabel.MajorStrokeThickness = 1;
            lineYAxisLabel.MajorStroke = new SolidBrush(this.ChartSetting.AxisColor);
            lineYAxisLabel.StrokeThickness = 1;
            lineYAxisLabel.Stroke = new SolidBrush(this.ChartSetting.AxisColor);
            lineYAxisLabel.TickStrokeThickness = 1;
            lineYAxisLabel.TickStroke = new SolidBrush(this.ChartSetting.AxisColor);
            lineYAxisLabel.MinorStroke = null;
            lineYAxisLabel.LabelHorizontalAlignment = Infragistics.Portable.Components.UI.HorizontalAlignment.Left;
            lineYAxisLabel.LabelTextColor = new SolidBrush(Color.Black);
            lineYAxisLabel.LabelExtent = 100;
            lineYAxisLabel.FormatLabel += FormatLabelY;
            lineYAxisLabel.LabelTextStyle = FontStyle.Bold;

            labelChart.Axes.Add(lineYAxisLabel);

            // 실제로 데이터가 들어가는 그래프
            NumericXAxis numXAxis = new NumericXAxis();
            numXAxis.MaximumValue = Convert.ToDouble(endPos);
            numXAxis.MinimumValue = 0;
            numXAxis.Interval = (float)endPos / chartSetting.XAxisInterval;
            numXAxis.MajorStrokeThickness = 1;
            numXAxis.MajorStroke = new SolidBrush(this.ChartSetting.AxisColor);
            numXAxis.StrokeThickness = 1;
            numXAxis.Stroke = new SolidBrush(this.ChartSetting.AxisColor);
            numXAxis.TickStrokeThickness = 1;
            numXAxis.TickStroke = new SolidBrush(this.ChartSetting.AxisColor);
            numXAxis.MinorStroke = null;
            numXAxis.LabelTextColor = new SolidBrush(Color.Black);
            numXAxis.LabelExtent = 25;
            numXAxis.FormatLabel += FormatLabelX;
            numXAxis.LabelTextStyle = FontStyle.Bold;

            List<float> lotLength = new List<float>();
            for (int i = 0; i <= endPos; i++)
            {
                lotLength.Add(i);
            }

            CategoryXAxis xAxis = new CategoryXAxis();
            xAxis.MajorStroke = null;
            xAxis.Stroke = null;
            xAxis.TickStroke = null;
            xAxis.MinorStroke = null;
            xAxis.DataSource = lotLength;
            xAxis.LabelsVisible = false;

            NumericYAxis rangeColumnYAxis = new NumericYAxis();
            rangeColumnYAxis.MaximumValue = Math.Abs(this.ChartSetting.YAxisRange);
            rangeColumnYAxis.MinimumValue = -Math.Abs(this.ChartSetting.YAxisRange);
            rangeColumnYAxis.Interval = (Math.Abs(this.ChartSetting.YAxisRange) * 2.0) / this.ChartSetting.YAxisInterval;
            rangeColumnYAxis.Visibility = Infragistics.Portable.Components.UI.Visibility.Collapsed;
            rangeColumnYAxis.MajorStroke = null;
            rangeColumnYAxis.MinorStroke = null;
            rangeColumnYAxis.LabelsVisible = false;

            NumericYAxis lineYAxis = new NumericYAxis();
            lineYAxis.MaximumValue = Math.Abs(this.ChartSetting.YAxisRange);
            lineYAxis.MinimumValue = -Math.Abs(this.ChartSetting.YAxisRange);
            lineYAxis.Interval = (Math.Abs(this.ChartSetting.YAxisRange) * 2.0) / this.ChartSetting.YAxisInterval;
            lineYAxis.MajorStrokeThickness = 1;
            lineYAxis.MajorStroke = new SolidBrush(this.ChartSetting.AxisColor);
            lineYAxis.StrokeThickness = 1;
            lineYAxis.Stroke = new SolidBrush(this.ChartSetting.AxisColor);
            lineYAxis.TickStrokeThickness = 1;
            lineYAxis.TickStroke = new SolidBrush(this.ChartSetting.AxisColor);
            lineYAxis.MinorStroke = null;
            lineYAxis.LabelTextColor = new SolidBrush(Color.Black);
            lineYAxis.LabelExtent = 5;
            lineYAxis.FormatLabel += FormatLabelYSpace;

            SeriesUpperStop = new ScatterLineSeries();
            SeriesUpperStop.Name = "UpperStop";
            SeriesUpperStop.XMemberPath = "X";
            SeriesUpperStop.YMemberPath = "Y";
            SeriesUpperStop.XAxis = numXAxis;
            SeriesUpperStop.YAxis = lineYAxis;
            SeriesUpperStop.Brush = new SolidBrush(chartSetting.LineStopColor);
            SeriesUpperStop.Thickness = chartSetting.LineStopThickness;

            SeriesUpperWarning = new ScatterLineSeries();
            SeriesUpperWarning.Name = "UpperWarning";
            SeriesUpperWarning.XMemberPath = "X";
            SeriesUpperWarning.YMemberPath = "Y";
            SeriesUpperWarning.XAxis = numXAxis;
            SeriesUpperWarning.YAxis = lineYAxis;
            SeriesUpperWarning.Brush = new SolidBrush(chartSetting.LineWarningColor);
            SeriesUpperWarning.Thickness = chartSetting.LineWarningThickness;

            SeriesCenterLine = new ScatterLineSeries();
            SeriesCenterLine.Name = "CenterLine";
            SeriesCenterLine.XMemberPath = "X";
            SeriesCenterLine.YMemberPath = "Y";
            SeriesCenterLine.XAxis = numXAxis;
            SeriesCenterLine.YAxis = lineYAxis;
            SeriesCenterLine.Brush = new SolidBrush(chartSetting.LineCenterColor);
            SeriesCenterLine.Thickness = chartSetting.LineCenterThickness;

            SeriesLowerStop = new ScatterLineSeries();
            SeriesLowerStop.Name = "LowerStop";
            SeriesLowerStop.XMemberPath = "X";
            SeriesLowerStop.YMemberPath = "Y";
            SeriesLowerStop.XAxis = numXAxis;
            SeriesLowerStop.YAxis = lineYAxis;
            SeriesLowerStop.Brush = new SolidBrush(chartSetting.LineStopColor);
            SeriesLowerStop.Thickness = chartSetting.LineStopThickness;

            SeriesLowerWarning = new ScatterLineSeries();
            SeriesLowerWarning.Name = "LowerWarning";
            SeriesLowerWarning.XMemberPath = "X";
            SeriesLowerWarning.YMemberPath = "Y";
            SeriesLowerWarning.XAxis = numXAxis;
            SeriesLowerWarning.YAxis = lineYAxis;
            SeriesLowerWarning.Brush = new SolidBrush(chartSetting.LineWarningColor);
            SeriesLowerWarning.Thickness = chartSetting.LineWarningThickness;

            RangeColumn = new RangeColumnSeries();
            RangeColumn.HighMemberPath = "MaxGloss";
            RangeColumn.LowMemberPath = "MinGloss";
            RangeColumn.XAxis = xAxis;
            RangeColumn.YAxis = rangeColumnYAxis;
            RangeColumn.Brush = new SolidBrush(this.ChartSetting.SubGraphColor);
            RangeColumn.Outline = new SolidBrush(Color.Blue);
            RangeColumn.Thickness = 0.5;

            SeriesLine = new ScatterLineSeries();
            SeriesLine.XMemberPath = "RollPosition";
            SeriesLine.YMemberPath = "AvgGloss";
            SeriesLine.XAxis = numXAxis;
            SeriesLine.YAxis = lineYAxis;
            SeriesLine.Brush = new SolidBrush(this.ChartSetting.GraphColor);
            SeriesLine.Thickness = chartSetting.GraphThickness;

            profileChart.Axes.Add(numXAxis);
            profileChart.Axes.Add(rangeColumnYAxis);
            profileChart.Axes.Add(lineYAxis);
            profileChart.Axes.Add(xAxis);

            profileChart.Series.Add(RangeColumn);
            profileChart.Series.Add(SeriesLine);

            profileChart.HorizontalZoomable = true;
            profileChart.VerticalZoomable = true;

            if (this.ChartSetting.UseTotalCenterLine)
                profileChart.Series.Add(SeriesCenterLine);
            if (this.ChartSetting.UseLineStop)
                profileChart.Series.Add(SeriesUpperStop);
            if (this.ChartSetting.UseLineWarning)
                profileChart.Series.Add(SeriesUpperWarning);
            if (this.ChartSetting.UseLineStop)
                profileChart.Series.Add(SeriesLowerStop);
            if (this.ChartSetting.UseLineWarning)
                profileChart.Series.Add(SeriesLowerWarning);
        }

        public void AddScanData(GlossScanData scanData)
        {
            if (CurrentReelPosition == scanData.RollPosition)
                return;

            for (int i = RangeColumnScanDataList.Count; i < scanData.RollPosition; i++)
            {
                if (RangeColumnScanDataList.Count == 0)
                    RangeColumnScanDataList.Add(new GlossScanData(scanData.AvgGloss));

                RangeColumnScanDataList.Add(new GlossScanData(RangeColumnScanDataList.Last().AvgGloss));
            }

            RangeColumnScanDataList.Add(scanData);
            LineScanDataList.Add(scanData);

            CurrentReelPosition = scanData.RollPosition;
        }

        public void AddScanDataList(List<GlossScanData> scanDataList)
        {
            RangeColumnScanDataList.Clear();
            LineScanDataList.Clear();

            foreach (GlossScanData scanData in scanDataList)
                AddScanData(scanData);
        }

        public void DisplayResult(/*IndexData indexData = null, */int rollIndex = -1, int rollPosition = -1)
        {
            if (LineScanDataList.Count() < 1)
                return;

            GlossScanData scanData = new GlossScanData();
            if (rollIndex == -1)
                scanData = LineScanDataList.Last();
            else if (LineScanDataList.Count() > rollIndex)
                scanData = LineScanDataList[rollIndex];

            RangeColumn.DataSource = RangeColumnScanDataList;
            SeriesLine.DataSource = LineScanDataList;

            DisplayLabel(rollIndex);

            ((NumericYAxis)labelChart.Axes[0]).MaximumValue = scanData.AvgGloss * (1 + ChartSetting.YAxisRange / 100);
            ((NumericYAxis)labelChart.Axes[0]).MinimumValue = scanData.AvgGloss * (1 - ChartSetting.YAxisRange / 100);
            ((NumericYAxis)labelChart.Axes[0]).Interval = (double)(SeriesLine.YAxis.MaximumValue - SeriesLine.YAxis.MinimumValue) / ChartSetting.YAxisInterval;

            ((NumericYAxis)profileChart.Axes[1]).MaximumValue = scanData.AvgGloss * (1 + ChartSetting.YAxisRange / 100);
            ((NumericYAxis)profileChart.Axes[1]).MinimumValue = scanData.AvgGloss * (1 - ChartSetting.YAxisRange / 100);
            ((NumericYAxis)profileChart.Axes[1]).Interval = (double)(SeriesLine.YAxis.MaximumValue - SeriesLine.YAxis.MinimumValue) / ChartSetting.YAxisInterval;

            ((NumericYAxis)profileChart.Axes[2]).MaximumValue = scanData.AvgGloss * (1 + ChartSetting.YAxisRange / 100);
            ((NumericYAxis)profileChart.Axes[2]).MinimumValue = scanData.AvgGloss * (1 - ChartSetting.YAxisRange / 100);
            ((NumericYAxis)profileChart.Axes[2]).Interval = (double)(SeriesLine.YAxis.MaximumValue - SeriesLine.YAxis.MinimumValue) / ChartSetting.YAxisInterval;

            if (RangeColumnScanDataList.Count() > 0)
            {
                if (rollIndex == -1)
                {
                    chartPanel.AutoScrollPosition = new System.Drawing.Point(
                        Math.Max(Convert.ToInt32(Convert.ToSingle(RangeColumnScanDataList.Count) * GraphScale) - chartPanel.Size.Width + 15, 0), 0);
                }
                else
                {
                    chartPanel.AutoScrollPosition = new System.Drawing.Point(
                        Math.Max(Convert.ToInt32(Convert.ToSingle(rollPosition + 1) * GraphScale) - chartPanel.Size.Width + 15, 0), 0);
                }
            }

            float lowerStopValue = scanData.AvgGloss * (1 - (float)ChartSetting.LineStopRange / 100.0f);
            float upperStopValue = scanData.AvgGloss * (1 + (float)ChartSetting.LineStopRange / 100.0f);
            float lowerWarningValue = scanData.AvgGloss * (1 - (float)ChartSetting.LineWarningRange / 100.0f);
            float upperWarningValue = scanData.AvgGloss * (1 + (float)ChartSetting.LineWarningRange / 100.0f);

            List<PointF> centerlist = new List<PointF>();
            centerlist.Add(new PointF(0, scanData.AvgGloss));
            centerlist.Add(new PointF(float.MaxValue, scanData.AvgGloss));
            SeriesCenterLine.DataSource = centerlist;

            List<PointF> lowerStoplist = new List<PointF>();
            lowerStoplist.Add(new PointF(0, lowerStopValue));
            lowerStoplist.Add(new PointF(float.MaxValue, lowerStopValue));
            SeriesLowerStop.DataSource = lowerStoplist;

            List<PointF> upperStoplist = new List<PointF>();
            upperStoplist.Add(new PointF(0, upperStopValue));
            upperStoplist.Add(new PointF(float.MaxValue, upperStopValue));
            SeriesUpperStop.DataSource = upperStoplist;

            List<PointF> lowerWarninglist = new List<PointF>();
            lowerWarninglist.Add(new PointF(0, lowerWarningValue));
            lowerWarninglist.Add(new PointF(float.MaxValue, lowerWarningValue));
            SeriesLowerWarning.DataSource = lowerWarninglist;

            List<PointF> upperWarninglist = new List<PointF>();
            upperWarninglist.Add(new PointF(0, upperWarningValue));
            upperWarninglist.Add(new PointF(float.MaxValue, upperWarningValue));
            SeriesUpperWarning.DataSource = upperWarninglist;
        }

        private void DisplayLabel(int rollIndex = -1)
        {
            if (LineScanDataList.Count() < 1 || LineScanDataList.Count() <= rollIndex)
                return;

            if (rollIndex == -1)
            {
                txtMin.Text = String.Format("{0:0.000}", this.LineScanDataList.Last().MinGloss);
                txtMax.Text = String.Format("{0:0.000}", this.LineScanDataList.Last().MaxGloss);
                txtAvg.Text = String.Format("{0:0.000}", this.LineScanDataList.Last().AvgGloss);
                txtDev.Text = String.Format("{0:0.000}", this.LineScanDataList.Last().DevGloss);
            }
            else
            {
                txtMin.Text = String.Format("{0:0.000}", this.LineScanDataList[rollIndex].MinGloss);
                txtMax.Text = String.Format("{0:0.000}", this.LineScanDataList[rollIndex].MaxGloss);
                txtAvg.Text = String.Format("{0:0.000}", this.LineScanDataList[rollIndex].AvgGloss);
                txtDev.Text = String.Format("{0:0.000}", this.LineScanDataList[rollIndex].DevGloss);
            }
        }
        #endregion

        #region 구조체
        #endregion

        #region 클레스
        #endregion
    }
}
