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
using Infragistics.UltraChart.Shared.Styles;
using System.Windows.Forms.DataVisualization.Charting;
using Infragistics.UltraChart.Resources.Appearance;
using Infragistics.UltraChart.Core.Layers;

namespace UniScan.UI
{
    public partial class ProfilePanel : UserControl, IResultPanel
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
                PanelType = PanelType.Profile;
                chartSetting = new ChartSetting();
            }

            public override ResultPanelInfo Clone()
            {
                Info profilePanelInfo = new Info();

                profilePanelInfo.PanelType = this.PanelType;
                profilePanelInfo.ValueType = this.ValueType;

                profilePanelInfo.chartSetting = this.chartSetting.Clone();

                return profilePanelInfo;
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

        List<ScanData> scanDataList = new List<ScanData>();

        ScatterLineSeries seriesLine;

        ScatterLineSeries seriesUpperError;
        ScatterLineSeries seriesUpperWarning;
        ScatterLineSeries seriesLowerWarning;
        ScatterLineSeries seriesLowerError;

        DataTable dataTable;

        public ProfilePanel()
        {
            InitializeComponent();
        }

        Info info;

        public void Initialize(ResultPanelInfo resultPanelInfo)
        {
            info = (Info)resultPanelInfo;
            ChartSetting setting = info.ChartSetting;

            Infragistics.Win.DataVisualization.LinearGradientBrush brush = new Infragistics.Win.DataVisualization.LinearGradientBrush();

            GradientStopCollection gradientStopCollection = new GradientStopCollection();

            GradientStop gradientStop1 = new GradientStop(Color.Gray, 0.0f);
            GradientStop gradientStop2 = new GradientStop(Color.Gray, (setting.ValidStartPos - setting.StartPos - 0.1) / (setting.EndPos - setting.StartPos));
            GradientStop gradientStop3 = new GradientStop(Color.White, (setting.ValidStartPos - setting.StartPos) / (setting.EndPos - setting.StartPos));
            GradientStop gradientStop4 = new GradientStop(Color.White, (setting.ValidEndPos - setting.StartPos) / (setting.EndPos - setting.StartPos));
            GradientStop gradientStop5 = new GradientStop(Color.Gray, (setting.ValidEndPos - setting.StartPos + 0.1) / (setting.EndPos - setting.StartPos));
            GradientStop gradientStop6 = new GradientStop(Color.Gray, 1.0f);

            gradientStopCollection.Add(gradientStop1);
            gradientStopCollection.Add(gradientStop2);
            gradientStopCollection.Add(gradientStop3);
            gradientStopCollection.Add(gradientStop4);
            gradientStopCollection.Add(gradientStop5);
            gradientStopCollection.Add(gradientStop6);

            brush.GradientStops = gradientStopCollection;

            profileChart.PlotAreaBackground = brush;

            

            NumericYAxis yAxis = new NumericYAxis();

            yAxis.MaximumValue = setting.MaxValue;
            yAxis.MinimumValue = setting.MinValue;
            yAxis.Visibility = Infragistics.Portable.Components.UI.Visibility.Collapsed;
            yAxis.MinorStroke = null;
            yAxis.MajorStroke = null;
            yAxis.LabelsVisible = false;



            NumericXAxis xAxis = new NumericXAxis();

            xAxis.MaximumValue = setting.EndPos;
            xAxis.MinimumValue = setting.StartPos;
            xAxis.Interval = (setting.EndPos - setting.StartPos) / 5;
            xAxis.CrossingAxis = yAxis;
            xAxis.CrossingValue = setting.ValueUpperWarning;
            xAxis.MajorStrokeThickness = 1;
            xAxis.MajorStroke = new SolidBrush(Color.Black);
            xAxis.StrokeThickness = 1;
            xAxis.Stroke = new SolidBrush(Color.Black);
            xAxis.TickStrokeThickness = 1;
            xAxis.TickStroke = new SolidBrush(Color.Black);
            xAxis.MinorStroke = null;
            xAxis.LabelTextColor = new SolidBrush(Color.Black);
            xAxis.LabelExtent = 20;



            seriesLine = new ScatterLineSeries();
            
            seriesLine.XMemberPath = "X";
            seriesLine.YMemberPath = "Y";
            seriesLine.XAxis = xAxis;
            seriesLine.YAxis = yAxis;
            seriesLine.Brush = new SolidBrush(Color.Red);
            seriesLine.Thickness = 2;



            profileChart.Axes.Add(xAxis);
            profileChart.Axes.Add(yAxis);

            profileChart.Series.Add(seriesLine);



            List<PointF> upperErrorlist = new List<PointF>();

            upperErrorlist.Add(new PointF(setting.StartPos, setting.ValueUpperError));
            upperErrorlist.Add(new PointF(setting.EndPos, setting.ValueUpperError));

            seriesUpperError = new ScatterLineSeries();

            seriesUpperError.XMemberPath = "X";
            seriesUpperError.YMemberPath = "Y";
            seriesUpperError.XAxis = xAxis;
            seriesUpperError.YAxis = yAxis;
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
            seriesUpperWarning.XAxis = xAxis;
            seriesUpperWarning.YAxis = yAxis;
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
            seriesLowerWarning.XAxis = xAxis;
            seriesLowerWarning.YAxis = yAxis;
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
            seriesLowerError.XAxis = xAxis;
            seriesLowerError.YAxis = yAxis;
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
            scanDataList.Add(scanData);
        }

        public void AddValue(float position, float sheetThickness, float petThickness)
        {
            
        }

        public void DisplayResult()
        {
            if (scanDataList.Count() > 0)
            {
                List<PointF> pointList = scanDataList.Last().SheetThicknessData.ValueList;
                if (pointList.Count() > 0)
                    seriesLine.DataSource = pointList;
            }
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
