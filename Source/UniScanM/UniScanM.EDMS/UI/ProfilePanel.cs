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

namespace UniScanM.EDMS.UI
{
    public partial class ProfilePanel : UserControl, IMultiLanguageSupport
    {
        private int needRevisonUI = 0;

        ScatterLineSeries seriesLine;
        ScatterLineSeries seriesLine2;
        ScatterLineSeries seriesUpperStop;
        ScatterLineSeries seriesLowerStop;
        ScatterLineSeries seriesUpperWarn;
        ScatterLineSeries seriesLowerWarn;

        bool isReport;
        DataType dataType;
        List<DataPoint> dataList = new List<DataPoint>();
        List<DataPoint> dataList2 = new List<DataPoint>();

        public DataType DataType { get => dataType; set => dataType = value; }

        public ProfilePanel(DataType dataType)
        {
            InitializeComponent();
            this.SuspendLayout();
            this.Dock = DockStyle.Fill;
            
            this.dataType = dataType;
            Initialize();
            this.ResumeLayout(false);
            StringManager.AddListener(this);

            
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
                case DataType.FilmEdge0WithPrintingEdge0:
                    panelTypeStr = "T103 Film And T104 Printing";
                    break;
            }

            return panelTypeStr;
        }

        private bool isDisplayErrorLine()
        {
            switch (DataType)
            {
                case DataType.FilmEdge:
                case DataType.Coating_Film:
                case DataType.Printing_Coating:
                case DataType.PrintingEdge_0:
                case DataType.FilmEdge0WithPrintingEdge0:
                    return false;
                case DataType.FilmEdge_0:
                    return EDMSSettings.Instance().T103ErrorDisplay;
                case DataType.Printing_FilmEdge_0:
                    return EDMSSettings.Instance().T105ErrorDisplay;
            }

            return false;
        }

        private bool isDisplayWorningLine()
        {
            switch (DataType)
            {
                case DataType.FilmEdge:
                case DataType.Coating_Film:
                case DataType.Printing_Coating:
                case DataType.PrintingEdge_0:
                case DataType.FilmEdge0WithPrintingEdge0:
                    return false;

                case DataType.FilmEdge_0:
                    return EDMSSettings.Instance().T103WarningDisplay;
                    
                case DataType.Printing_FilmEdge_0:
                    return EDMSSettings.Instance().T105WarningDisplay;
                    //return true;//EDMSSettings.Instance().UseLineStop == true ? true : false;
            }

            return false;
        }

        public void Initialize()
        {
            try
            {
                UiHelper.SuspendDrawing(profileChart);
                profileChart.Axes.Clear();
                profileChart.Series.Clear();

                EDMSSettings setting = EDMSSettings.Instance();

                profileChart.PlotAreaBackground = new SolidBrush(setting.BackColor);

                NumericYAxis yAxis = new NumericYAxis();

                if (dataType == DataType.Printing_FilmEdge_0)
                {
                    yAxis.Interval = (double)(setting.YAxisRangeUM * 2.0) / setting.YAxisInterval;
                    yAxis.MaximumValue = Math.Abs(setting.YAxisRangeUM);
                    yAxis.MinimumValue = -Math.Abs(setting.YAxisRangeUM);
                }
                else
                {
                    yAxis.Interval = (double)(setting.YAxisRangeMM * 2.0) / setting.YAxisInterval;
                    yAxis.MaximumValue = Math.Abs(setting.YAxisRangeMM);
                    yAxis.MinimumValue = -Math.Abs(setting.YAxisRangeMM);
                }

                yAxis.MajorStrokeThickness = 1;
                yAxis.MajorStroke = new SolidBrush(setting.AxisColor);
                yAxis.StrokeThickness = 1;
                yAxis.Stroke = new SolidBrush(setting.AxisColor);
                yAxis.TickStrokeThickness = 1;
                yAxis.TickStroke = new SolidBrush(setting.AxisColor);
                yAxis.MinorStroke = null;
                yAxis.LabelExtent = 70;

                yAxis.FormatLabel += FormatLabelY;
                yAxis.LabelTextStyle = FontStyle.Bold;
                yAxis.LabelTextColor = new SolidBrush(Color.Black);

                NumericXAxis xAxis = new NumericXAxis();
                xAxis.MinimumValue = 0;
                xAxis.MaximumValue = setting.XAxisDisplayDistance;
                xAxis.Interval = (setting.XAxisDisplayDistance * 2) / setting.XAxisInterval;
                xAxis.MajorStrokeThickness = 1;
                xAxis.MajorStroke = new SolidBrush(setting.AxisColor);
                xAxis.StrokeThickness = 1;
                xAxis.Stroke = new SolidBrush(setting.AxisColor);
                xAxis.TickStrokeThickness = 1;
                xAxis.TickStroke = new SolidBrush(setting.AxisColor);
                xAxis.MinorStroke = null;
                xAxis.LabelExtent = 40;
                xAxis.FormatLabel += FormatLabelX;
                xAxis.LabelTextStyle = FontStyle.Bold;
                xAxis.LabelTextColor = new SolidBrush(Color.Black);

                List<PointF> upperStoplist = new List<PointF>();
                List<PointF> lowerErrorlist = new List<PointF>();

                seriesUpperStop = new ScatterLineSeries();
                seriesLowerStop = new ScatterLineSeries();

                seriesUpperStop.XMemberPath = "X";
                seriesUpperStop.YMemberPath = "Y";
                seriesUpperStop.XAxis = xAxis;
                seriesUpperStop.YAxis = yAxis;
                seriesUpperStop.Brush = new SolidBrush(setting.LineStopColor);
                seriesUpperStop.Thickness = setting.LineStopThickness;

                seriesLowerStop.XMemberPath = "X";
                seriesLowerStop.YMemberPath = "Y";
                seriesLowerStop.XAxis = xAxis;
                seriesLowerStop.YAxis = yAxis;
                seriesLowerStop.Brush = new SolidBrush(setting.LineStopColor);
                seriesLowerStop.Thickness = setting.LineStopThickness;

                seriesUpperStop.DataSource = upperStoplist;
                seriesLowerStop.DataSource = lowerErrorlist;

                //Error Line
                if (this.dataType == DataType.FilmEdge_0 && setting.T103ErrorDisplay)
                {
                    upperStoplist.Add(new PointF(0, (float)setting.T103ErrorRange));
                    upperStoplist.Add(new PointF((float)xAxis.MaximumValue, (float)setting.T103ErrorRange));
                    lowerErrorlist.Add(new PointF(0, -(float)setting.T103ErrorRange));
                    lowerErrorlist.Add(new PointF((float)xAxis.MaximumValue, -(float)setting.T103ErrorRange));
                }
                if (this.dataType == DataType.Printing_FilmEdge_0 && setting.T105ErrorDisplay)
                {
                    upperStoplist.Add(new PointF(0, (float)setting.T105ErrorRange));
                    upperStoplist.Add(new PointF((float)xAxis.MaximumValue, (float)setting.T105ErrorRange));
                    lowerErrorlist.Add(new PointF(0, -(float)setting.T105ErrorRange));
                    lowerErrorlist.Add(new PointF((float)xAxis.MaximumValue, -(float)setting.T105ErrorRange));
                }

                if (isDisplayErrorLine())
                {
                    profileChart.Series.Add(seriesUpperStop);
                    profileChart.Series.Add(seriesLowerStop);
                }

                //워닝라인 START
                List<PointF> upperWarnlist = new List<PointF>();
                List<PointF> lowerWarninglist = new List<PointF>();

                seriesUpperWarn = new ScatterLineSeries();
                seriesLowerWarn = new ScatterLineSeries();

                seriesUpperWarn.XMemberPath = "X";
                seriesUpperWarn.YMemberPath = "Y";
                seriesUpperWarn.XAxis = xAxis;
                seriesUpperWarn.YAxis = yAxis;
                seriesUpperWarn.Brush = new SolidBrush(setting.LineWarningColor);
                seriesUpperWarn.Thickness = setting.LineWarningThickness;

                seriesLowerWarn.XMemberPath = "X";
                seriesLowerWarn.YMemberPath = "Y";
                seriesLowerWarn.XAxis = xAxis;
                seriesLowerWarn.YAxis = yAxis;
                seriesLowerWarn.Brush = new SolidBrush(setting.LineWarningColor);
                seriesLowerWarn.Thickness = setting.LineWarningThickness;

                seriesUpperWarn.DataSource = upperWarnlist;
                seriesLowerWarn.DataSource = lowerWarninglist;

                if (this.dataType == DataType.FilmEdge_0 && setting.T103WarningDisplay)
                {
                    upperWarnlist.Add(new PointF(0, (float)setting.T103WarningRange));
                    upperWarnlist.Add(new PointF((float)xAxis.MaximumValue, (float)setting.T103WarningRange));
                    lowerWarninglist.Add(new PointF(0, -(float)setting.T103WarningRange));
                    lowerWarninglist.Add(new PointF((float)xAxis.MaximumValue, -(float)setting.T103WarningRange));
                }

                if (this.dataType == DataType.Printing_FilmEdge_0 && setting.T105WarningDisplay)
                {
                    upperWarnlist.Add(new PointF(0, (float)setting.T105WarningRange));
                    upperWarnlist.Add(new PointF((float)xAxis.MaximumValue, (float)setting.T105WarningRange));
                    lowerWarninglist.Add(new PointF(0, -(float)setting.T105WarningRange));
                    lowerWarninglist.Add(new PointF((float)xAxis.MaximumValue, -(float)setting.T105WarningRange));
                }

                if (isDisplayWorningLine())
                {
                    profileChart.Series.Add(seriesUpperWarn);
                    profileChart.Series.Add(seriesLowerWarn);
                }
                //워닝라인 END

                //데이터라인 begin
                seriesLine = new ScatterLineSeries();

                seriesLine.XMemberPath = "X";
                seriesLine.YMemberPath = "Y";
                seriesLine.XAxis = xAxis;
                seriesLine.YAxis = yAxis;
                seriesLine.Brush = new SolidBrush(setting.GraphColor);
                seriesLine.Thickness = setting.GraphThickness;

                //
                seriesLine2 = new ScatterLineSeries();

                seriesLine2.XMemberPath = "X";
                seriesLine2.YMemberPath = "Y";
                seriesLine2.XAxis = xAxis;
                seriesLine2.YAxis = yAxis;
                Color reverseColor =  Color.FromArgb(255 - setting.GraphColor.R, 255 - setting.GraphColor.G, 255 - setting.GraphColor.B);
                seriesLine2.Brush = new SolidBrush(reverseColor);
                seriesLine2.Thickness = setting.GraphThickness;
                

                profileChart.Axes.Add(xAxis);
                profileChart.Axes.Add(yAxis);

                profileChart.HorizontalZoomable = true;
                profileChart.VerticalZoomable = true;

                profileChart.Series.Add(seriesLine);
                profileChart.Series.Add(seriesLine2);

                UiHelper.ResumeDrawing(profileChart);
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                LogHelper.Error(LoggerType.Error, message);
                ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Initialize.FailToInitialize, ErrorLevel.Fatal,
                    this.GetPanelTypeString(), "{0} Chart Initizlize Fail", new object[] { this.GetPanelTypeString() }, message);
            }
        }

        private string FormatLabelX(Infragistics.Win.DataVisualization.AxisLabelInfo info)
        {
            return string.Format("{0:0}m", info.Value);
        }

        private string FormatLabelY(Infragistics.Win.DataVisualization.AxisLabelInfo info)
        {
            switch (dataType)
            {
                case DataType.FilmEdge:
                case DataType.Coating_Film:
                case DataType.Printing_Coating:
                    return string.Format("{0:0.00}", info.Value);

                case DataType.FilmEdge_0:
                case DataType.PrintingEdge_0:
                case DataType.FilmEdge0WithPrintingEdge0:
                    return string.Format("{0:0.000}", info.Value);

                case DataType.Printing_FilmEdge_0:
                    return string.Format("{0:0}", info.Value);
            }

            return null;
        }
        private void timer_Update_Tick(object sender, EventArgs e)
        {
            timer_Update.Stop();
            DisplayResult();
        }
        public void AddChartData(DataPoint chartData)
        {
            //return;
            //if (dataList.Exists(data => data.XValue == chartData.XValue))
            //    return;

            lock (dataList)
            {
                if (dataList.Count > 0 && chartData.XValue < dataList.Max(f => f.XValue))
                    return;

                double minDistance = chartData.XValue - EDMSSettings.Instance().XAxisDisplayDistance;
                dataList.RemoveAll(data => data.XValue < minDistance);
                dataList.Add(chartData);
                timer_Update.Start(); //////////
            }
            //DisplayResult(); //문제있어서 타이머로 처리..
        }

        public void AddChartDataList(List<DataPoint> chartDataList)
        {
            if (chartDataList.Count == 0)
                return;
            lock (dataList)
            {
                this.dataList = chartDataList;
            }
            DisplayResult(true);
        }

        public void AddChartDataList2(List<DataPoint> chartDataList, List<DataPoint> chartDataList2) // T103 & T104
        {
            if (chartDataList.Count == 0)
                return;
            lock (dataList)
            {
                this.dataList = chartDataList;
                this.dataList2 = chartDataList2;
            }
            DisplayResult(true);
        }

        void AllControl_SuspendLayout()
        {
            txtAvg.SuspendLayout();
            txtMin.SuspendLayout();
            txtMax.SuspendLayout();
            labelAvg.SuspendLayout();
            labelMin2.SuspendLayout();
            labelMax2.SuspendLayout();
            labelPanel.SuspendLayout();
            layoutMain.SuspendLayout();
            profileChart.SuspendLayout();
            labelTitle.SuspendLayout();
            labelUnit.SuspendLayout();
            txtVar.SuspendLayout();
            labelCur.SuspendLayout();
            txtCur.SuspendLayout();
            labelDiff.SuspendLayout();
            txtMinMax.SuspendLayout();
            labelVar.SuspendLayout();
            //private Infragistics.Win.Misc.UltraButton ultraButtonZoomReset;
            //private System.Windows.Forms.Panel panel1;
        }
        void AllControl_ResumeLayout()
        {
            txtAvg.ResumeLayout();
            txtMin.ResumeLayout();
            txtMax.ResumeLayout();
            labelAvg.ResumeLayout();
            labelMin2.ResumeLayout();
            labelMax2.ResumeLayout();
            labelPanel.ResumeLayout();
            layoutMain.ResumeLayout();
            profileChart.ResumeLayout();
            labelTitle.ResumeLayout();
            labelUnit.ResumeLayout();
            txtVar.ResumeLayout();
            labelCur.ResumeLayout();
            txtCur.ResumeLayout();
            labelDiff.ResumeLayout();
            txtMinMax.ResumeLayout();
            labelVar.ResumeLayout();
        }
   

        delegate void DisplayResultDelegate(bool whole_X = false);
        public void DisplayResult(bool whole_X=false)
        {
            if (InvokeRequired)
            {
                Invoke(new DisplayResultDelegate(DisplayResult),whole_X);
                return;
            }


            AllControl_SuspendLayout();
            EDMSSettings setting = EDMSSettings.Instance();
            
            lock (dataList)
            {

                if (dataList.Count() > 0)
                {
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    if (whole_X == true)
                    {
                        double minDistance = dataList.Min(data => data.XValue);
                        double maxDistance = dataList.Max(data => data.XValue);

                        seriesLine.XAxis.MinimumValue = minDistance;
                        seriesLine.XAxis.MaximumValue = maxDistance;
                        seriesLine.XAxis.Interval = (maxDistance - minDistance) / EDMSSettings.Instance().XAxisInterval;

                        if (dataType == DataType.FilmEdge0WithPrintingEdge0)
                        {
                            seriesLine2.XAxis.MinimumValue = minDistance;
                            seriesLine2.XAxis.MaximumValue = maxDistance;
                            seriesLine2.XAxis.Interval = (maxDistance - minDistance) / EDMSSettings.Instance().XAxisInterval;
                        }
                    }
                    else
                    {
                        DataPoint chartData = dataList.Last<DataPoint>();
                        double minDistance = chartData.XValue - EDMSSettings.Instance().XAxisDisplayDistance;
                        double minValidDistance = dataList.Min(data => data.XValue);
                        if (chartData.XValue - minValidDistance < EDMSSettings.Instance().XAxisDisplayDistance)
                        {
                            seriesLine.XAxis.MinimumValue = minValidDistance;
                            seriesLine.XAxis.MaximumValue = minValidDistance + EDMSSettings.Instance().XAxisDisplayDistance;
                        }
                        else
                        {
                            seriesLine.XAxis.MinimumValue = minDistance;
                            seriesLine.XAxis.MaximumValue = chartData.XValue;
                        }

                        seriesLine.XAxis.Interval = (chartData.XValue - minDistance) / EDMSSettings.Instance().XAxisInterval;
                    }
                    ////////////////////////////////////////////////////////////////////////////////////////////
                    if (isDisplayErrorLine() == true)
                    {
                        List<PointF> upperStoplist = new List<PointF>();
                        List<PointF> lowerStoplist = new List<PointF>();
                        if (this.dataType == DataType.FilmEdge_0 && setting.T103ErrorDisplay)
                        {
                            upperStoplist.Add(new PointF((float)seriesLine.XAxis.MinimumValue, (float)setting.T103ErrorRange));
                            upperStoplist.Add(new PointF((float)seriesLine.XAxis.MaximumValue, (float)setting.T103ErrorRange));
                            lowerStoplist.Add(new PointF((float)seriesLine.XAxis.MinimumValue, -(float)setting.T103ErrorRange));
                            lowerStoplist.Add(new PointF((float)seriesLine.XAxis.MaximumValue, -(float)setting.T103ErrorRange));
                        }
                        else if (this.dataType == DataType.Printing_FilmEdge_0 && setting.T105ErrorDisplay)
                        {
                            upperStoplist.Add(new PointF((float)seriesLine.XAxis.MinimumValue, (float)setting.T105ErrorRange));
                            upperStoplist.Add(new PointF((float)seriesLine.XAxis.MaximumValue, (float)setting.T105ErrorRange));
                            lowerStoplist.Add(new PointF((float)seriesLine.XAxis.MinimumValue, -(float)setting.T105ErrorRange));
                            lowerStoplist.Add(new PointF((float)seriesLine.XAxis.MaximumValue, -(float)setting.T105ErrorRange));
                        }

                        seriesUpperStop.DataSource = upperStoplist;
                        seriesLowerStop.DataSource = lowerStoplist;
                    }

                    if (isDisplayWorningLine() == true)
                    {
                        List<PointF> upperWarninglist = new List<PointF>();
                        List<PointF> lowerWarnlist = new List<PointF>();

                        if (this.dataType == DataType.FilmEdge_0 && setting.T103WarningDisplay)
                        {
                            upperWarninglist.Add(new PointF((float)seriesLine.XAxis.MinimumValue, (float)setting.T103WarningRange));
                            upperWarninglist.Add(new PointF((float)seriesLine.XAxis.MaximumValue, (float)setting.T103WarningRange));
                            lowerWarnlist.Add(new PointF((float)seriesLine.XAxis.MinimumValue, -(float)setting.T103WarningRange));
                            lowerWarnlist.Add(new PointF((float)seriesLine.XAxis.MaximumValue, -(float)setting.T103WarningRange));
                        }
                        else if (this.dataType == DataType.Printing_FilmEdge_0 && setting.T105WarningDisplay)
                        {
                            upperWarninglist.Add(new PointF((float)seriesLine.XAxis.MinimumValue, (float)setting.T105WarningRange));
                            upperWarninglist.Add(new PointF((float)seriesLine.XAxis.MaximumValue, (float)setting.T105WarningRange));
                            lowerWarnlist.Add(new PointF((float)seriesLine.XAxis.MinimumValue, -(float)setting.T105WarningRange));
                            lowerWarnlist.Add(new PointF((float)seriesLine.XAxis.MaximumValue, -(float)setting.T105WarningRange));
                        }

                        seriesUpperWarn.DataSource = upperWarninglist;
                        seriesLowerWarn.DataSource = lowerWarnlist;
                    }

                    float yRange = dataType != DataType.Printing_FilmEdge_0 ? setting.YAxisRangeMM : setting.YAxisRangeUM;

                    List<PointF> newDataList = new List<PointF>();
                    List<PointF> newDataList2 = new List<PointF>();

                    switch (dataType)
                    {
                        case DataType.FilmEdge:
                        case DataType.Coating_Film:
                        case DataType.Printing_Coating:
                            seriesLine.YAxis.MaximumValue = dataList.Max(data => data.YValues.Max()) + Math.Abs(yRange);
                            seriesLine.YAxis.MinimumValue = dataList.Min(data => data.YValues.Min()) - Math.Abs(yRange);
                            this.dataList.ForEach(data => newDataList.AddRange(GetPoints(data)));//todo System.InvalidOperationException: 컬렉션이 수정되었습니다. 열거 작업이 실행되지 않을 수도 있습니다.
                            break;

                        case DataType.FilmEdge0WithPrintingEdge0:
                            double fmax = dataList.Max(data => data.YValues.Max());
                            double fmin = dataList.Min(data => data.YValues.Min());
                            double fmax2 = dataList2.Max(data => data.YValues.Max());
                            double fmin2 = dataList2.Min(data => data.YValues.Min());
                            seriesLine.YAxis.MaximumValue = Math.Max(fmax, fmax2) + Math.Abs(yRange);
                            seriesLine.YAxis.MinimumValue = Math.Min(fmin, fmin2) - Math.Abs(yRange);
                            seriesLine2.YAxis.MaximumValue = Math.Max(fmax, fmax2) + Math.Abs(yRange);
                            seriesLine2.YAxis.MinimumValue = Math.Min(fmin, fmin2) - Math.Abs(yRange);

                            this.dataList.ForEach(data => newDataList.AddRange(GetPoints(data)));
                            this.dataList2.ForEach(data => newDataList2.AddRange(GetPoints(data)));
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
                            seriesLine.YAxis.MaximumValue = Math.Abs(range);
                            seriesLine.YAxis.MinimumValue = -Math.Abs(range);
                            break;
                    }

                    seriesLine.YAxis.Interval = (double)(seriesLine.YAxis.MaximumValue - seriesLine.YAxis.MinimumValue) / setting.YAxisInterval;
                    seriesLine.DataSource = GetShowDataList(newDataList);

                    if(dataType == DataType.FilmEdge0WithPrintingEdge0)
                    {
                        seriesLine2.YAxis.Interval = (double)(seriesLine2.YAxis.MaximumValue - seriesLine2.YAxis.MinimumValue) / setting.YAxisInterval;
                        seriesLine2.DataSource = GetShowDataList(newDataList2);
                    }
                }
                else
                {
                    seriesLine.DataSource = null;
                    seriesLine2.DataSource = null;
                }

                DisplayLabel();
            }//lock (dataList)
            AllControl_ResumeLayout();
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
                Invoke(new DisplayLabelDelegate(DisplayLabel));
                return;
            }
            //layoutMain.SuspendLayout();
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

            //layoutMain.ResumeLayout();
        }

        public float GetStdDev()
        {
            if (dataList.Count < 1)
                return 0;

            double var = 0f;
            double avg = dataList.Average(data => data.YValues.Average());

            foreach (DataPoint data in dataList)
                foreach (double y in data.YValues)
                    var += Math.Pow((double)(y - avg), 2);
            
            return (float)(var / (double)dataList.Count);
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

        private void ultraButtonZoomReset_Click(object sender, EventArgs e)
        {
            profileChart.ResetZoom();
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


    }
}
