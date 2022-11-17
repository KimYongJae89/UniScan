using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Infragistics.Win.DataVisualization;
using System.Xml;
using Infragistics.UltraChart.Shared.Styles;
using System.Windows.Forms.DataVisualization.Charting;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Core.Layers;
using DynMvp.Base;
using UniScanM.CEDMS.Settings;
using UniScanM.CEDMS.Data;
using DynMvp.UI;

namespace UniScanM.CEDMS.UI.Chart
{
    public partial class ProfilePanel : UserControl, IMultiLanguageSupport
    {
        bool isTotal;
        public bool IsTotal { get => isTotal; set => isTotal = value; }

        List<CEDMSScanData> scanDataList = new List<CEDMSScanData>();

        ScatterLineSeries seriesLine;

        ChartSetting chartSetting;

        bool isReport;
        private ProfileOption profileOption;
        string title;

        public ProfilePanel(string title, bool isTotal, bool isReport, ChartSetting chartSetting = null, ProfileOption profileOption = null)
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;
            this.chartSetting = chartSetting;
            this.title = title;
            this.isTotal = isTotal;
            this.isReport = isReport;
            this.profileOption = profileOption == null ? new ProfileOption() : profileOption;
            Initialize(chartSetting);

            StringManager.AddListener(this);
        }

        public void Initialize(ChartSetting chartSetting = null)
        {
            profileChart.Axes.Clear();
            profileChart.Series.Clear();

            if (chartSetting == null)
            {
                chartSetting = this.chartSetting.Clone();
            }
            else
            {
                CEDMSSettings tempSetting = (CEDMSSettings)CEDMSSettings.Instance();


                chartSetting.AxisColor = tempSetting.AxisColor;
                chartSetting.BackColor = tempSetting.BackColor;
                chartSetting.GraphColor = tempSetting.GraphColor;
                chartSetting.LineStopColor = tempSetting.LineStopColor;
            }
            
            profileChart.PlotAreaBackground = new SolidBrush(chartSetting.BackColor);
            
            NumericYAxis yAxis = new NumericYAxis();

            yAxis.Interval = (Math.Abs(this.chartSetting.YAxisRange) * 2.0) / this.chartSetting.YAxisInterval;
            yAxis.MaximumValue = Math.Abs(this.chartSetting.YAxisRange);
            yAxis.MinimumValue = -Math.Abs(this.chartSetting.YAxisRange);
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
            yAxis.IsInverted = profileOption.YInvert;

            NumericXAxis xAxis = new NumericXAxis();
            xAxis.MinimumValue = 0;

            if (isTotal == true)
            {
                xAxis.MaximumValue = this.chartSetting.XAxisDisplayTotalLength;
            }
            else
            {
                xAxis.MaximumValue = this.chartSetting.XAxisDisplayLength;
            }

            xAxis.Interval = (xAxis.MaximumValue - xAxis.MinimumValue) / this.chartSetting.XAxisInterval;
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
            if (isReport == false)
                xAxis.IsInverted = false;
            if (profileOption != null)
            {
                xAxis.IsInverted = profileOption.XInvert;
            }

            List<PointF> upperStoplist = new List<PointF>();
            float value = 0;
            value = Convert.ToSingle(this.chartSetting.LineStopUpper);
            upperStoplist.Add(new PointF(0, value));
            upperStoplist.Add(new PointF(float.MaxValue, value));

            ScatterLineSeries seriesUpperStop = new ScatterLineSeries();

            seriesUpperStop.Name = "UpperStop";
            seriesUpperStop.XMemberPath = "X";
            seriesUpperStop.YMemberPath = "Y";
            seriesUpperStop.XAxis = xAxis;
            seriesUpperStop.YAxis = yAxis;
            seriesUpperStop.Brush = new SolidBrush(chartSetting.LineStopColor);
            seriesUpperStop.Thickness = chartSetting.LineStopThickness;

            seriesUpperStop.DataSource = upperStoplist;

            List<PointF> upperWarninglist = new List<PointF>();
            value = Convert.ToSingle(this.chartSetting.LineWarningUpper);
            upperWarninglist.Add(new PointF(0, value));
            upperWarninglist.Add(new PointF(float.MaxValue, value));

            ScatterLineSeries seriesUpperWarning = new ScatterLineSeries();

            seriesUpperWarning.Name = "UpperWarning";
            seriesUpperWarning.XMemberPath = "X";
            seriesUpperWarning.YMemberPath = "Y";
            seriesUpperWarning.XAxis = xAxis;
            seriesUpperWarning.YAxis = yAxis;
            seriesUpperWarning.Brush = new SolidBrush(chartSetting.LineWarningColor);
            seriesUpperWarning.Thickness = chartSetting.LineWarningThickness;

            seriesUpperWarning.DataSource = upperWarninglist;

            ScatterLineSeries seriesCenterLine = new ScatterLineSeries();

            seriesCenterLine.Name = "CenterLine";
            seriesCenterLine.XMemberPath = "X";
            seriesCenterLine.YMemberPath = "Y";
            seriesCenterLine.XAxis = xAxis;
            seriesCenterLine.YAxis = yAxis;
            seriesCenterLine.Brush = new SolidBrush(chartSetting.LineTotalGraphCenterColor);
            seriesCenterLine.Thickness = chartSetting.LineTotalGraphCenterThickness;


            List<PointF> centerList = new List<PointF>();
            centerList.Add(new PointF(0, 0));
            centerList.Add(new PointF(float.MaxValue, 0));

            seriesCenterLine.DataSource = centerList;
            if (isTotal == true)
            {
                if (this.chartSetting.UseTotalCenterLine)
                    profileChart.Series.Add(seriesCenterLine);
            }
            else
            {
                if (this.chartSetting.UseLineStop)
                    profileChart.Series.Add(seriesUpperStop);

                if (this.chartSetting.UseLineWarning)
                    profileChart.Series.Add(seriesUpperWarning);
            }

            List<PointF> lowerErrorlist = new List<PointF>();
            value = -Convert.ToSingle(this.chartSetting.LineStopLower);
            lowerErrorlist.Add(new PointF(0, value));
            lowerErrorlist.Add(new PointF(float.MaxValue, value));

            ScatterLineSeries seriesLowerStop = new ScatterLineSeries();
            seriesLowerStop.Name = "LowerStop";
            seriesLowerStop.XMemberPath = "X";
            seriesLowerStop.YMemberPath = "Y";
            seriesLowerStop.XAxis = xAxis;
            seriesLowerStop.YAxis = yAxis;
            seriesLowerStop.Brush = new SolidBrush(chartSetting.LineStopColor);
            seriesLowerStop.Thickness = chartSetting.LineStopThickness;

            seriesLowerStop.DataSource = lowerErrorlist;

            List<PointF> lowerWarninglist = new List<PointF>();
            value = -Convert.ToSingle(this.chartSetting.LineWarningLower);
            lowerWarninglist.Add(new PointF(0, value));
            lowerWarninglist.Add(new PointF(float.MaxValue, value));

            ScatterLineSeries seriesLowerWarning = new ScatterLineSeries();

            seriesLowerWarning.Name = "LowerWarning";
            seriesLowerWarning.XMemberPath = "X";
            seriesLowerWarning.YMemberPath = "Y";
            seriesLowerWarning.XAxis = xAxis;
            seriesLowerWarning.YAxis = yAxis;
            seriesLowerWarning.Brush = new SolidBrush(chartSetting.LineWarningColor);
            seriesLowerWarning.Thickness = chartSetting.LineWarningThickness;

            seriesLowerWarning.DataSource = lowerWarninglist;

            if (isTotal == false)
            {
                if (this.chartSetting.UseLineStop)
                    profileChart.Series.Add(seriesLowerStop);

                if (this.chartSetting.UseLineWarning)
                    profileChart.Series.Add(seriesLowerWarning);
            }

            seriesLine = new ScatterLineSeries();
            seriesLine.Name = "Data";
            seriesLine.XMemberPath = "X";
            seriesLine.YMemberPath = "Y";
            seriesLine.XAxis = xAxis;
            seriesLine.YAxis = yAxis;
            seriesLine.Brush = new SolidBrush(chartSetting.GraphColor);
            seriesLine.Thickness = chartSetting.GraphThickness / (this.isTotal && this.isReport == false ? 2 : 1);

            profileChart.Axes.Add(xAxis);
            profileChart.Axes.Add(yAxis);
            //profileChart.Axes.Add(yAxisTemp);

            profileChart.HorizontalZoomable = true;
            profileChart.VerticalZoomable = true;

            profileChart.Series.Add(seriesLine);

            if (isTotal == true && isReport == false)
                labelPanel.Visible = false;
        }

        private string FormatLabelX(Infragistics.Win.DataVisualization.AxisLabelInfo info)
        {
            return info.Value.ToString("F0") + "m";
        }

        private string FormatLabelY(Infragistics.Win.DataVisualization.AxisLabelInfo info)
        {
            var value = info.Value;
            if (profileOption.YInvert)
                value *= -1;

            string format = "F2";
            bool isMm = this.chartSetting.YAxisUnit == YAxisUnitCEDMS.Mm;
            if (!isMm)
            {
                labelMM.Text = "[um]";
                value *= 1000;
                format = "F0";
            }
            else
                labelMM.Text = "[mm]";


            return value.ToString(format);
        }

        public void AddValue(CEDMSScanData scanData)
        {
            scanDataList.Add(scanData);

            DisplayResult();
        }

        public void AddScanDataList(List<CEDMSScanData> scanDataList)
        {
            this.scanDataList.Clear();
            this.scanDataList = scanDataList;

            if (scanDataList.Count == 0)
                return;

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
                lock (scanDataList)
                {
                    List<PointF> displayScanDataList = new List<PointF>();

                    if (scanDataList.Count() > 0)
                    {
                        if (isTotal == true)
                        {
                            // X
                            double axisMinLen = scanDataList.Min(f => f.NowDistance);
                            double axisMaxLen = scanDataList.Max(f => f.NowDistance);

                            double axisMin = Math.Max(0, axisMaxLen - chartSetting.XAxisDisplayTotalLength);
                            double axisMax = Math.Max(chartSetting.XAxisDisplayTotalLength, axisMaxLen);

                            if (isReport)
                                axisMin = axisMinLen;

                            seriesLine.XAxis.MaximumValue = axisMax;
                            seriesLine.XAxis.MinimumValue = axisMin;
                            seriesLine.XAxis.Interval = (double)(seriesLine.XAxis.MaximumValue - seriesLine.XAxis.MinimumValue) / chartSetting.XAxisInterval;

                            // Y
                            if (displayScanDataList.Count > 0 && profileOption.AutoScaleY)
                            {
                                seriesLine.YAxis.MaximumValue = displayScanDataList.Max(f => f.Y) + Math.Abs(chartSetting.YAxisRange / 4);
                                seriesLine.YAxis.MinimumValue = displayScanDataList.Min(f => f.Y) - Math.Abs(chartSetting.YAxisRange / 4);
                            }
                            else
                            {
                                seriesLine.YAxis.MaximumValue = 0 + Math.Abs(chartSetting.YAxisRange);
                                seriesLine.YAxis.MinimumValue = 0 - Math.Abs(chartSetting.YAxisRange);
                            }
                            seriesLine.YAxis.Interval = (double)(seriesLine.YAxis.MaximumValue - seriesLine.YAxis.MinimumValue) / chartSetting.YAxisInterval;

                            scanDataList.ForEach(scanData =>
                            {
                                float x = scanData.NowDistance;
                                float y = scanData.YRaw;
                                if (profileOption.AutoScaleY == false)
                                    y = scanData.YRaw < 0 ? (float)Math.Max(scanData.YRaw, -Math.Abs(chartSetting.YAxisRange - 0.001)) : (float)Math.Min(scanData.YRaw, Math.Abs(chartSetting.YAxisRange - 0.001));
                                if (axisMin <= x && x <= axisMax)
                                    displayScanDataList.Add(new PointF(x, y));
                            });

                        }
                        else
                        {
                            // X
                            double axisLen = scanDataList.Max(f => f.NowDistance);

                            double axisMin = Math.Max(0, axisLen - chartSetting.XAxisDisplayLength);
                            double axisMax = Math.Max(chartSetting.XAxisDisplayLength, axisLen);

                            seriesLine.XAxis.MaximumValue = axisMax;
                            seriesLine.XAxis.MinimumValue = axisMin;
                            seriesLine.XAxis.Interval = (double)(seriesLine.XAxis.MaximumValue - seriesLine.XAxis.MinimumValue) / chartSetting.XAxisInterval;

                            // Y
                            seriesLine.YAxis.MaximumValue = Math.Abs(chartSetting.YAxisRange);
                            seriesLine.YAxis.MinimumValue = -Math.Abs(chartSetting.YAxisRange);
                            seriesLine.YAxis.Interval = (double)(seriesLine.YAxis.MaximumValue - seriesLine.YAxis.MinimumValue) / chartSetting.YAxisInterval;

                            scanDataList.ForEach(scanData =>
                            {
                                float x = scanData.NowDistance;
                                float y = scanData.YRaw;

                                if (axisMin <= x && x <= axisMax)
                                    displayScanDataList.Add(new PointF(x, y));
                            });
                        }

                        if (title.Contains("Certain"))
                        {
                            int count = profileChart.Series.Count;
                        }
                        seriesLine.DataSource = displayScanDataList;
                    }
                    else
                        seriesLine.DataSource = null;

                    DisplayLabel(displayScanDataList);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, ex.Message);
            }
        }

        delegate void DisplayLabelDelegate(List<PointF> displayScanDataList);
        private void DisplayLabel(List<PointF> displayScanDataList)
        {
            if (InvokeRequired)
            {
                Invoke(new DisplayLabelDelegate(DisplayLabel));
                return;
            }

            Label[] labels = new Label[] { txtMin, txtMax, txtAvg, txtCur, txtStd, txtDiff };
            int digit = 3;

            layoutMain.SuspendLayout();
            Array.ForEach(labels, f => UiHelper.SuspendDrawing(f));

            if (displayScanDataList.Count() > 0)
            {
                double min = Math.Round(displayScanDataList.Min(scanData => scanData.Y), digit, MidpointRounding.AwayFromZero);
                double max = Math.Round(displayScanDataList.Max(scanData => scanData.Y), digit, MidpointRounding.AwayFromZero);
                double[] values = new double[6]
                {
                    min,max,
                    Math.Round(displayScanDataList.Average(f => f.Y),digit),
                    Math.Round(displayScanDataList.Last().Y,digit),
                    Math.Round(GetStdDev(displayScanDataList),digit),
                    Math.Round(max-min,digit)
                };

                bool isMm = chartSetting.YAxisUnit == YAxisUnitCEDMS.Mm;
                string format = string.Format("F{0}", digit);
                for (int i = 0; i < labels.Length; i++)
                    labels[i].Text = isMm ? values[i].ToString(format) : (values[i] * 1000).ToString();
            }
            else
            {
                float value = 0;
                Array.ForEach(labels, f => f.Text = value.ToString());
            }
            //profileChart.PlotAreaBackground = new SolidBrush(setting.BackColor);

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

        public float GetStdDev(List<CEDMSScanData> scanDataValidList)
        {
            if (scanDataValidList.Count < 1)
                return 0;

            double var = 0f;
            float avg = scanDataValidList.Average(scanData => scanData.YRaw);

            foreach (CEDMSScanData scanData in scanDataValidList)
            {
                var += Math.Pow((double)(scanData.YRaw - avg), 2);
            }

            return (float)Math.Sqrt(var / (double)scanDataValidList.Count);
            //return (float)(var / (double)scanDataValidList.Count);
        }

        public void ClearPanel()
        {
            scanDataList.Clear();

            DisplayResult();
        }

        private void ultraButtonZoomReset_Click(object sender, EventArgs e)
        {
            profileChart.ResetZoom();
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);

            this.labelTitle.Text = StringManager.GetString(this.GetType().FullName, this.title);
        }

        void LineShow()
        {
            foreach (ScatterLineSeries series in profileChart.Series)
            {
                if (series.Name != null)
                {
                    if (series.Name.Contains("Warning"))
                    {
                        List<PointF> lowerWarninglist = new List<PointF>();
                        float value = -Convert.ToSingle(chartSetting.LineWarningLower);
                        lowerWarninglist.Add(new PointF(0, value));
                        lowerWarninglist.Add(new PointF((float)seriesLine.XAxis.MaximumValue, value));

                        series.DataSource = lowerWarninglist;
                    }
                }
            }
        }
    }
    public class ProfileOption
    {
        bool xInvert = false;
        bool yInvert = false;
        bool autoScaleY = false;
        public bool XInvert { get => xInvert; set => xInvert = value; }
        public bool YInvert { get => yInvert; set => yInvert = value; }
        public bool AutoScaleY { get => autoScaleY; set => autoScaleY = value; }

        public ProfileOption()
        {

        }

        public ProfileOption(bool xInvert, bool yInvert, bool autoScaleY)
        {
            this.xInvert = xInvert;
            this.yInvert = yInvert;
            this.autoScaleY = autoScaleY;
        }
    }
}
