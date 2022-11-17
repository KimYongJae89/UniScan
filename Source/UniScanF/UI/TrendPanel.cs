using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScan.Data;
using Infragistics.Win.DataVisualization;
using System.Xml;

namespace UniScan.UI
{
    public partial class TrendPanel : UserControl, IResultPanel
    {
        public class Info : ResultPanelInfo
        {
            int overlayCount;
            public int OverlayCount
            {
                get { return overlayCount; }
                set { overlayCount = value; }
            }

            ChartSetting chartSetting;
            public ChartSetting ChartSetting
            {
                get { return chartSetting; }
                set { chartSetting = value; }
            }

            public Info()
            {
                PanelType = PanelType.Trend;
                chartSetting = new ChartSetting();
            }

            public override ResultPanelInfo Clone()
            {
                Info trendPanelInfo = new Info();

                trendPanelInfo.PanelType = this.PanelType;
                trendPanelInfo.ValueType = this.ValueType;

                trendPanelInfo.chartSetting = this.chartSetting.Clone();

                return trendPanelInfo;
            }

            public override void Save(XmlElement xmlElement)
            {
                base.Save(xmlElement);

                xmlElement.SetAttribute("OverlayCount", overlayCount.ToString());

                XmlElement chartSettingElement = xmlElement.OwnerDocument.CreateElement("ChartSetting");
                xmlElement.AppendChild(chartSettingElement);

                chartSetting.Save(chartSettingElement);
            }

            public override void Load(XmlElement xmlElement)
            {
                base.Load(xmlElement);

                overlayCount = Convert.ToInt32(xmlElement.GetAttribute("OverlayCount"));

                XmlElement chartSettingElement = xmlElement["ChartSetting"];
                chartSetting.Load(chartSettingElement);
            }
        }



        List<ScanData> rangeColumnScanDataList = new List<ScanData>();
        List<ScanData> lineScanDataList = new List<ScanData>();

        RangeColumnSeries rangeColumn;
        LineSeries line;

        ScatterLineSeries seriesUpperError;
        ScatterLineSeries seriesUpperWarning;
        ScatterLineSeries seriesLowerWarning;
        ScatterLineSeries seriesLowerError;

        public TrendPanel()
        {
            InitializeComponent();
        }

        Info info;

        public void Initialize(ResultPanelInfo resultPanelInfo)
        {
            info = (Info)resultPanelInfo;
            ChartSetting setting = info.ChartSetting;

            NumericXAxis numXAxis = new NumericXAxis();

            numXAxis.MaximumValue = setting.EndPos;
            numXAxis.MinimumValue = setting.StartPos;
            numXAxis.Interval = (setting.EndPos - setting.StartPos) / 5;
            numXAxis.MajorStrokeThickness = 1;
            numXAxis.MajorStroke = new SolidBrush(Color.Black);
            numXAxis.StrokeThickness = 1;
            numXAxis.Stroke = new SolidBrush(Color.Black);
            numXAxis.TickStrokeThickness = 1;
            numXAxis.TickStroke = new SolidBrush(Color.Black);
            numXAxis.MinorStroke = null;
            numXAxis.LabelTextColor = new SolidBrush(Color.Black);
            numXAxis.LabelExtent = 20;

            List<float> lotLength = new List<float>();
            for (int i = (int)setting.StartPos; i < (int)setting.EndPos; i++)
            {
                lotLength.Add(i);
            }

            CategoryXAxis xAxis = new CategoryXAxis();

            xAxis.Interval = lotLength.Count / 5;
            xAxis.MajorStrokeThickness = 1;
            xAxis.MajorStroke = new SolidBrush(Color.Black);
            xAxis.StrokeThickness = 1;
            xAxis.Stroke = new SolidBrush(Color.Black);
            xAxis.TickStrokeThickness = 1;
            xAxis.TickStroke = new SolidBrush(Color.Black);
            xAxis.MinorStroke = null;
            xAxis.DataSource = lotLength;
            xAxis.LabelsVisible = false;

            NumericYAxis rangeColumnYAxis = new NumericYAxis();

            rangeColumnYAxis.MaximumValue = setting.MaxValue;
            rangeColumnYAxis.MinimumValue = setting.MinValue;
            rangeColumnYAxis.Interval = (setting.MaxValue - setting.MinValue) / 4;
            rangeColumnYAxis.Visibility = Infragistics.Portable.Components.UI.Visibility.Collapsed;
            rangeColumnYAxis.MajorStroke = null;
            rangeColumnYAxis.MinorStroke = null;
            rangeColumnYAxis.LabelsVisible = false;

            NumericYAxis lineYAxis = new NumericYAxis();

            lineYAxis.MaximumValue = setting.MaxValue;
            lineYAxis.MinimumValue = setting.MinValue;
            lineYAxis.Interval = (setting.MaxValue - setting.MinValue) / 4;
            lineYAxis.Visibility = Infragistics.Portable.Components.UI.Visibility.Collapsed;
            lineYAxis.MajorStroke = null;
            lineYAxis.MinorStroke = null;
            lineYAxis.LabelsVisible = false;

            rangeColumn = new RangeColumnSeries();

            rangeColumn.HighMemberPath = "SheetMax";
            rangeColumn.LowMemberPath = "SheetMin";
            rangeColumn.XAxis = xAxis;
            rangeColumn.YAxis = rangeColumnYAxis;
            rangeColumn.Brush = new SolidBrush(Color.Blue);
            rangeColumn.Outline = new SolidBrush(Color.White);
            rangeColumn.Resolution = 0.1;
            rangeColumn.Thickness = 0.1;

            line = new LineSeries();

            line.ValueMemberPath = "SheetAverage";
            line.XAxis = xAxis;
            line.YAxis = lineYAxis;
            line.Brush = new SolidBrush(Color.Red);
            line.Thickness = 2;

            profileChart.Axes.Add(numXAxis);
            profileChart.Axes.Add(rangeColumnYAxis);
            profileChart.Axes.Add(lineYAxis);
            profileChart.Axes.Add(xAxis);

            profileChart.Series.Add(rangeColumn);
            profileChart.Series.Add(line);



            List<PointF> upperErrorlist = new List<PointF>();

            upperErrorlist.Add(new PointF(setting.StartPos, setting.ValueUpperError));
            upperErrorlist.Add(new PointF(setting.EndPos, setting.ValueUpperError));

            seriesUpperError = new ScatterLineSeries();

            seriesUpperError.XMemberPath = "X";
            seriesUpperError.YMemberPath = "Y";
            seriesUpperError.XAxis = numXAxis;
            seriesUpperError.YAxis = lineYAxis;
            seriesUpperError.Brush = new SolidBrush(Color.Red);
            seriesUpperError.Thickness = 2;

            seriesUpperError.DataSource = upperErrorlist;

            profileChart.Series.Add(seriesUpperError);

            int positionUpperError = (int)(profileChart.Location.Y + (profileChart.Size.Height - profileChart.Axes[0].LabelExtent - profileChart.Margin.Vertical)
                * (setting.MaxValue - setting.ValueUpperError) / (setting.MaxValue - setting.MinValue));
            labelUpperError.Location = new System.Drawing.Point(labelMin.Location.X, positionUpperError);
            labelUpperError.Text = setting.ValueUpperError.ToString();



            List<PointF> upperWarninglist = new List<PointF>();

            upperWarninglist.Add(new PointF(setting.StartPos, setting.ValueUpperWarning));
            upperWarninglist.Add(new PointF(setting.EndPos, setting.ValueUpperWarning));

            seriesUpperWarning = new ScatterLineSeries();

            seriesUpperWarning.XMemberPath = "X";
            seriesUpperWarning.YMemberPath = "Y";
            seriesUpperWarning.XAxis = numXAxis;
            seriesUpperWarning.YAxis = lineYAxis;
            seriesUpperWarning.Brush = new SolidBrush(Color.Green);
            seriesUpperWarning.Thickness = 2;

            seriesUpperWarning.DataSource = upperWarninglist;

            profileChart.Series.Add(seriesUpperWarning);

            int positionUpperWarning = (int)(profileChart.Location.Y + (profileChart.Size.Height - profileChart.Axes[0].LabelExtent - profileChart.Margin.Vertical)
                * (setting.MaxValue - setting.ValueUpperWarning) / (setting.MaxValue - setting.MinValue));
            labelUpperWarning.Location = new System.Drawing.Point(labelMin.Location.X, positionUpperWarning);
            labelUpperWarning.Text = setting.ValueUpperWarning.ToString();



            List<PointF> lowerWarninglist = new List<PointF>();

            lowerWarninglist.Add(new PointF(setting.StartPos, setting.ValueLowerWarning));
            lowerWarninglist.Add(new PointF(setting.EndPos, setting.ValueLowerWarning));

            seriesLowerWarning = new ScatterLineSeries();

            seriesLowerWarning.XMemberPath = "X";
            seriesLowerWarning.YMemberPath = "Y";
            seriesLowerWarning.XAxis = numXAxis;
            seriesLowerWarning.YAxis = lineYAxis;
            seriesLowerWarning.Brush = new SolidBrush(Color.Green);
            seriesLowerWarning.Thickness = 2;

            seriesLowerWarning.DataSource = lowerWarninglist;

            profileChart.Series.Add(seriesLowerWarning);

            int positionLowerWarning = (int)(profileChart.Location.Y + (profileChart.Size.Height - profileChart.Axes[0].LabelExtent - profileChart.Margin.Vertical)
                * (setting.MaxValue - setting.ValueLowerWarning) / (setting.MaxValue - setting.MinValue));
            labelLowerWarning.Location = new System.Drawing.Point(labelMin.Location.X, positionLowerWarning);
            labelLowerWarning.Text = setting.ValueLowerWarning.ToString();



            List<PointF> lowerErrorlist = new List<PointF>();

            lowerErrorlist.Add(new PointF(setting.StartPos, setting.ValueLowerError));
            lowerErrorlist.Add(new PointF(setting.EndPos, setting.ValueLowerError));

            seriesLowerError = new ScatterLineSeries();

            seriesLowerError.XMemberPath = "X";
            seriesLowerError.YMemberPath = "Y";
            seriesLowerError.XAxis = numXAxis;
            seriesLowerError.YAxis = lineYAxis;
            seriesLowerError.Brush = new SolidBrush(Color.Red);
            seriesLowerError.Thickness = 2;

            seriesLowerError.DataSource = lowerErrorlist;

            profileChart.Series.Add(seriesLowerError);

            int positionLowerError = (int)(profileChart.Location.Y + (profileChart.Size.Height - profileChart.Axes[0].LabelExtent - profileChart.Margin.Vertical)
                * (setting.MaxValue - setting.ValueLowerError) / (setting.MaxValue - setting.MinValue));
            labelLowerError.Location = new System.Drawing.Point(labelMin.Location.X, positionLowerError);
            labelLowerError.Text = setting.ValueLowerError.ToString();
        }

        public void AddScanData(ScanData scanData)
        {
            rangeColumnScanDataList.Add(scanData);
            lineScanDataList.Add(scanData);
        }

        public void AddValue(float position, float sheetThickness, float petThickness)
        {
        }

        public void DisplayResult()
        {
            rangeColumn.DataSource = rangeColumnScanDataList;
            line.DataSource = lineScanDataList;
        }

        private void btnChartSetting_Click(object sender, EventArgs e)
        {
            ChartSettingForm form = new ChartSettingForm(info);

            if (form.ShowDialog() == DialogResult.OK)
            {
                SystemSettings.Instance().Save();

                profileChart.Axes.Clear();
                profileChart.Series.Clear();

                Initialize(info);
            }
        }
    }
}
