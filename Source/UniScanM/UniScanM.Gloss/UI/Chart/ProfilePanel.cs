using DynMvp.Base;
using DynMvp.UI;
using Infragistics.Win.DataVisualization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UniScanM.Gloss.Data;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss.UI.Chart
{
    public partial class ProfilePanel : UserControl, IMultiLanguageSupport
    {
        #region 필드
        #endregion

        #region 생성자
        public ProfilePanel(string title, bool isReport, ChartSetting chartSetting = null, ChartOption chartOption = null)
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;
            this.ChartSetting = chartSetting;
            this.Title = title;
            this.IsReport = isReport;
            this.ChartOption = chartOption == null ? new ChartOption() : chartOption;
            Initialize(ChartSetting);

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

        private string FormatLabelX(Infragistics.Win.DataVisualization.AxisLabelInfo info)
        {
            return info.Value.ToString("F0") + "mm";
        }

        private string FormatLabelY(Infragistics.Win.DataVisualization.AxisLabelInfo info)
        {
            var value = info.Value;
            if (ChartOption.YInvert)
                value *= -1;

            string format = "F2";

            return value.ToString(format);
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

        private List<GlossScanData> ScanDataList { get; set; } = new List<GlossScanData>();

        private ScatterLineSeries SeriesLine { get; set; }

        private ScatterLineSeries SeriesOverlayLine { get; set; }

        private ScatterLineSeries SeriesCenterLine { get; set; }

        private ScatterLineSeries SeriesUpperStop { get; set; }

        private ScatterLineSeries SeriesLowerStop { get; set; }

        private ScatterLineSeries SeriesUpperWarning { get; set; }

        private ScatterLineSeries SeriesLowerWarning { get; set; }

        private ChartSetting ChartSetting { get; set; }

        private ChartOption ChartOption { get; set; }

        private string Title { get; set; }

        private bool IsReport { get; set; }
        #endregion

        #region 인덱서
        #endregion

        #region 메서드
        public void ClearPanel()
        {
            ScanDataList.Clear();
            DisplayResult();
        }

        public void Initialize(ChartSetting chartSetting = null)
        {
            profileChart.Axes.Clear();
            profileChart.Series.Clear();

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
                ChartSetting.GlossScanWidth = tempSetting.SelectedGlossScanWidth;
            }

            profileChart.PlotAreaBackground = new SolidBrush(chartSetting.BackColor);

            NumericYAxis yAxis = new NumericYAxis();
            yAxis.Interval = (Math.Abs(this.ChartSetting.YAxisRange) * 2.0) / this.ChartSetting.YAxisInterval;
            yAxis.MaximumValue = Math.Abs(this.ChartSetting.YAxisRange);
            yAxis.MinimumValue = -Math.Abs(this.ChartSetting.YAxisRange);
            yAxis.MajorStrokeThickness = 1;
            yAxis.MajorStroke = new SolidBrush(chartSetting.AxisColor);
            yAxis.StrokeThickness = 1;
            yAxis.Stroke = new SolidBrush(chartSetting.AxisColor);
            yAxis.TickStrokeThickness = 1;
            yAxis.TickStroke = new SolidBrush(chartSetting.AxisColor);
            yAxis.MinorStroke = null;
            yAxis.LabelExtent = 75;
            yAxis.FormatLabel += FormatLabelY;
            yAxis.LabelTextStyle = FontStyle.Bold;
            yAxis.LabelTextColor = new SolidBrush(Color.Black);
            yAxis.IsInverted = ChartOption.YInvert;

            NumericXAxis xAxis = new NumericXAxis();
            xAxis.MinimumValue = this.ChartSetting.GlossScanWidth.Start;
            xAxis.MaximumValue = this.ChartSetting.GlossScanWidth.End;
            xAxis.Interval = (xAxis.MaximumValue - xAxis.MinimumValue) / this.ChartSetting.XAxisInterval;
            xAxis.MajorStrokeThickness = 1;
            xAxis.MajorStroke = new SolidBrush(chartSetting.AxisColor);
            xAxis.StrokeThickness = 1;
            xAxis.Stroke = new SolidBrush(chartSetting.AxisColor);
            xAxis.TickStrokeThickness = 1;
            xAxis.TickStroke = new SolidBrush(chartSetting.AxisColor);
            xAxis.MinorStroke = null;
            xAxis.LabelExtent = 50;
            xAxis.FormatLabel += FormatLabelX;
            xAxis.LabelTextStyle = FontStyle.Bold;
            xAxis.LabelTextColor = new SolidBrush(Color.Black);
            if (IsReport == false)
                xAxis.IsInverted = false;
            if (ChartOption != null)
            {
                xAxis.IsInverted = ChartOption.XInvert;
            }

            SeriesUpperStop = new ScatterLineSeries();
            SeriesUpperStop.Name = "UpperStop";
            SeriesUpperStop.XMemberPath = "X";
            SeriesUpperStop.YMemberPath = "Y";
            SeriesUpperStop.XAxis = xAxis;
            SeriesUpperStop.YAxis = yAxis;
            SeriesUpperStop.Brush = new SolidBrush(chartSetting.LineStopColor);
            SeriesUpperStop.Thickness = chartSetting.LineStopThickness;

            SeriesUpperWarning = new ScatterLineSeries();
            SeriesUpperWarning.Name = "UpperWarning";
            SeriesUpperWarning.XMemberPath = "X";
            SeriesUpperWarning.YMemberPath = "Y";
            SeriesUpperWarning.XAxis = xAxis;
            SeriesUpperWarning.YAxis = yAxis;
            SeriesUpperWarning.Brush = new SolidBrush(chartSetting.LineWarningColor);
            SeriesUpperWarning.Thickness = chartSetting.LineWarningThickness;

            SeriesCenterLine = new ScatterLineSeries();
            SeriesCenterLine.Name = "CenterLine";
            SeriesCenterLine.XMemberPath = "X";
            SeriesCenterLine.YMemberPath = "Y";
            SeriesCenterLine.XAxis = xAxis;
            SeriesCenterLine.YAxis = yAxis;
            SeriesCenterLine.Brush = new SolidBrush(chartSetting.LineCenterColor);
            SeriesCenterLine.Thickness = chartSetting.LineCenterThickness;

            SeriesLowerStop = new ScatterLineSeries();
            SeriesLowerStop.Name = "LowerStop";
            SeriesLowerStop.XMemberPath = "X";
            SeriesLowerStop.YMemberPath = "Y";
            SeriesLowerStop.XAxis = xAxis;
            SeriesLowerStop.YAxis = yAxis;
            SeriesLowerStop.Brush = new SolidBrush(chartSetting.LineStopColor);
            SeriesLowerStop.Thickness = chartSetting.LineStopThickness;

            SeriesLowerWarning = new ScatterLineSeries();
            SeriesLowerWarning.Name = "LowerWarning";
            SeriesLowerWarning.XMemberPath = "X";
            SeriesLowerWarning.YMemberPath = "Y";
            SeriesLowerWarning.XAxis = xAxis;
            SeriesLowerWarning.YAxis = yAxis;
            SeriesLowerWarning.Brush = new SolidBrush(chartSetting.LineWarningColor);
            SeriesLowerWarning.Thickness = chartSetting.LineWarningThickness;

            SeriesLine = new ScatterLineSeries();
            SeriesLine.Name = "Data";
            SeriesLine.XMemberPath = "X";
            SeriesLine.YMemberPath = "Y";
            SeriesLine.XAxis = xAxis;
            SeriesLine.YAxis = yAxis;
            SeriesLine.Brush = new SolidBrush(chartSetting.GraphColor);
            SeriesLine.Thickness = chartSetting.GraphThickness / (this.IsReport == false ? 2 : 1);

            SeriesOverlayLine = new ScatterLineSeries();
            SeriesOverlayLine.Name = "Data";
            SeriesOverlayLine.XMemberPath = "X";
            SeriesOverlayLine.YMemberPath = "Y";
            SeriesOverlayLine.XAxis = xAxis;
            SeriesOverlayLine.YAxis = yAxis;
            SeriesOverlayLine.Brush = new SolidBrush(chartSetting.SubGraphColor);
            SeriesOverlayLine.Thickness = chartSetting.GraphThickness / (this.IsReport == false ? 2 : 1);

            profileChart.Axes.Add(xAxis);
            profileChart.Axes.Add(yAxis);

            profileChart.Series.Add(SeriesOverlayLine);
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
            ScanDataList.Add(scanData);
        }

        public void AddScanDataList(List<GlossScanData> scanDataList)
        {
            this.ScanDataList.Clear();

            foreach (GlossScanData scanData in scanDataList)
                AddScanData(scanData);
        }

        public void DisplayResult(/*IndexData indexData = null, */int rollIndex = -1)
        {
            if (ScanDataList.Count() < 1)
                return;

            GlossScanData scanData = new GlossScanData();

            if (rollIndex == -1)
                scanData = ScanDataList.Last();
            else if (ScanDataList.Count() > rollIndex)
                scanData = ScanDataList[rollIndex];

            SeriesLine.DataSource = scanData;

            int count = 0;
            if (ScanDataList.Count() > 1)
            {
                List<GlossData> pointOverlayList = new List<GlossData>();

                for (int i = 0; i < ScanDataList.Count() - 1; i++)
                {
                    if (count >= ChartSetting.OverlayCount)
                        break;

                    // Serise 하나로 누적그래프를 표현하기 위해서 지그재그로 데이터를 넣는다.
                    // 그래서 Reverse를 사용하여 넣는다.
                    if (rollIndex == -1)
                    {
                        if ((float)count % 2 == 0)
                            pointOverlayList.AddRange(ScanDataList[ScanDataList.Count() - 2 - i].GlossDatas);
                        else
                            pointOverlayList.AddRange(ScanDataList[ScanDataList.Count() - 2 - i].GlossDatas.Reverse<GlossData>());
                    }
                    else
                    {
                        if (rollIndex - 1 - i < 0)
                            break;

                        if ((float)count % 2 == 0)
                            pointOverlayList.AddRange(ScanDataList[rollIndex - 1 - i].GlossDatas);
                        else
                            pointOverlayList.AddRange(ScanDataList[rollIndex - 1 - i].GlossDatas.Reverse<GlossData>());
                    }

                    count++;
                }

                if (pointOverlayList.Count() > 0)
                    SeriesOverlayLine.DataSource = pointOverlayList;
            }

            DisplayLabel(rollIndex);

            SeriesLine.YAxis.MaximumValue = scanData.AvgGloss * (1 + ChartSetting.YAxisRange / 100.0f);
            SeriesLine.YAxis.MinimumValue = scanData.AvgGloss * (1 - ChartSetting.YAxisRange / 100.0f);
            SeriesLine.YAxis.Interval = (double)(SeriesLine.YAxis.MaximumValue - SeriesLine.YAxis.MinimumValue) / ChartSetting.YAxisInterval;

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
            if (ScanDataList.Count() < 1 || ScanDataList.Count() <= rollIndex)
                return;

            if (rollIndex == -1)
            {
                txtMin.Text = String.Format("{0:0.000}", this.ScanDataList.Last().MinGloss);
                txtMax.Text = String.Format("{0:0.000}", this.ScanDataList.Last().MaxGloss);
                txtAvg.Text = String.Format("{0:0.000}", this.ScanDataList.Last().AvgGloss);
                txtDev.Text = String.Format("{0:0.000}", this.ScanDataList.Last().DevGloss);
            }
            else
            {
                txtMin.Text = String.Format("{0:0.000}", this.ScanDataList[rollIndex].MinGloss);
                txtMax.Text = String.Format("{0:0.000}", this.ScanDataList[rollIndex].MaxGloss);
                txtAvg.Text = String.Format("{0:0.000}", this.ScanDataList[rollIndex].AvgGloss);
                txtDev.Text = String.Format("{0:0.000}", this.ScanDataList[rollIndex].DevGloss);
            }
        }
        #endregion

        #region 구조체
        #endregion

        #region 클레스
        #endregion
    }
}
