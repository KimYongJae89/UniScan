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
using UniScanM.RVMS.Settings;
using UniScanM.RVMS.Data;
using DynMvp.UI;

namespace UniScanM.RVMS.UI.Chart
{
    public partial class ProfilePanel : UserControl, IMultiLanguageSupport
    {
        bool isTotal;
        public bool IsTotal { get => isTotal; set => isTotal = value; }

        bool isPatten;
        public bool IsPatten { get => isPatten; set => isPatten = value; }

        public bool IsMm { get => (isPatten ? (setting.YAxisUnitPL == YAxisUnitRVMS.Mm) : (setting.YAxisUnit == YAxisUnitRVMS.Mm)); }

        List<ScanData> scanDataList = new List<ScanData>();

        ScatterLineSeries seriesLine;

        RVMSSettings setting;

        DateTime minXAxisTime;

        bool isReport;
        private ProfileOption profileOption;
        string title;

        public ProfilePanel(string title, bool isTotal, bool isPatten, bool isReport, RVMSSettings reportSetting = null, ProfileOption profileOption = null)
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            this.title = title;
            this.isTotal = isTotal;
            this.isPatten = isPatten;
            this.isReport = isReport;
            this.profileOption = profileOption == null ? new ProfileOption() : profileOption;
            Initialize(reportSetting);

            StringManager.AddListener(this);

            if (IsMm) //todo fun...
                labelMM.Text = "[mm]";
            else
                labelMM.Text = "[mm]";
        }
        
        public void Initialize(RVMSSettings reportSetting = null)
        {
            profileChart.Axes.Clear();
            profileChart.Series.Clear();

            if (reportSetting == null)
            {
                setting = (RVMSSettings)RVMSSettings.Instance();
            }
            else
            {
                RVMSSettings tempSetting = (RVMSSettings)RVMSSettings.Instance();

                setting = reportSetting;

                setting.AxisColor = tempSetting.AxisColor;
                setting.BackColor = tempSetting.BackColor;
                setting.GraphColor = tempSetting.GraphColor;
                setting.LineStopColor = tempSetting.LineStopColor;
            }
            
            profileChart.PlotAreaBackground = new SolidBrush(setting.BackColor);

            NumericYAxis yAxis = new NumericYAxis();
            
            if (isPatten == false)
            {
                yAxis.Interval = (Math.Abs(setting.YAxisRange) * 2.0) / setting.YAxisInterval;
                yAxis.MaximumValue = Math.Abs(setting.YAxisRange);
                yAxis.MinimumValue = -Math.Abs(setting.YAxisRange);
            }
            else
            {
                yAxis.Interval = (Math.Abs(setting.YAxisRangePL) * 2.0) / setting.YAxisIntervalPL;
                yAxis.MaximumValue = Math.Abs(setting.YAxisRangePL);
                yAxis.MinimumValue = -Math.Abs(setting.YAxisRangePL);
            }

            yAxis.MajorStrokeThickness = 1;
            yAxis.MajorStroke = new SolidBrush(setting.AxisColor);
            yAxis.StrokeThickness = 1;
            yAxis.Stroke = new SolidBrush(setting.AxisColor);
            yAxis.TickStrokeThickness = 1;
            yAxis.TickStroke = new SolidBrush(setting.AxisColor);
            yAxis.MinorStroke = null;
            yAxis.LabelExtent = 75;
            yAxis.FormatLabel += FormatLabelY;
            yAxis.LabelTextStyle = FontStyle.Bold;
            yAxis.LabelTextColor = new SolidBrush(Color.Black);
            yAxis.IsInverted = profileOption.YInvert;
            
            NumericXAxis xAxis = new NumericXAxis();
            xAxis.MinimumValue = 0;
            if (isPatten == false)
            {
                if (isTotal == true)
                {
                    xAxis.MaximumValue = setting.XAxisDisplayTimeTotalGraph * 1000 * 60;
                }
                else
                {
                    xAxis.MaximumValue = setting.XAxisDisplayTime * 1000;
                }

                xAxis.Interval = (xAxis.MaximumValue - xAxis.MinimumValue) / setting.XAxisInterval;
            }
            else
            {
                xAxis.MaximumValue = setting.XAxisDisplayTimePL * 1000;
                xAxis.Interval = xAxis.MaximumValue / setting.XAxisIntervalPL;
            }
            
            xAxis.MajorStrokeThickness = 1;
            xAxis.MajorStroke = new SolidBrush(setting.AxisColor);
            xAxis.StrokeThickness = 1;
            xAxis.Stroke = new SolidBrush(setting.AxisColor);
            xAxis.TickStrokeThickness = 1;
            xAxis.TickStroke = new SolidBrush(setting.AxisColor);
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
            value = isPatten == false ? Convert.ToSingle(setting.LineStopUpper) : Convert.ToSingle(setting.LineStopPL);
            upperStoplist.Add(new PointF(0, value));
            upperStoplist.Add(new PointF(float.MaxValue, value));
            
            ScatterLineSeries seriesUpperStop = new ScatterLineSeries();

            seriesUpperStop.Name = "UpperStop";
            seriesUpperStop.XMemberPath = "X";
            seriesUpperStop.YMemberPath = "Y";
            seriesUpperStop.XAxis = xAxis;
            seriesUpperStop.YAxis = yAxis;
            seriesUpperStop.Brush = new SolidBrush(setting.LineStopColor);
            seriesUpperStop.Thickness = setting.LineStopThickness;

            seriesUpperStop.DataSource = upperStoplist;

            List<PointF> upperWarninglist = new List<PointF>();
            value = Convert.ToSingle(setting.LineWarningUpper);
            upperWarninglist.Add(new PointF(0, value));
            upperWarninglist.Add(new PointF(float.MaxValue, value));

            ScatterLineSeries seriesUpperWarning = new ScatterLineSeries();

            seriesUpperWarning.Name = "UpperWarning";
            seriesUpperWarning.XMemberPath = "X";
            seriesUpperWarning.YMemberPath = "Y";
            seriesUpperWarning.XAxis = xAxis;
            seriesUpperWarning.YAxis = yAxis;
            seriesUpperWarning.Brush = new SolidBrush(setting.LineWarningColor);
            seriesUpperWarning.Thickness = setting.LineWarningThickness;

            seriesUpperWarning.DataSource = upperWarninglist;

            ScatterLineSeries seriesCenterLine = new ScatterLineSeries();

            seriesCenterLine.Name = "CenterLine";
            seriesCenterLine.XMemberPath = "X";
            seriesCenterLine.YMemberPath = "Y";
            seriesCenterLine.XAxis = xAxis;
            seriesCenterLine.YAxis = yAxis;
            seriesCenterLine.Brush = new SolidBrush(setting.LineTotalGraphCenterColor);
            seriesCenterLine.Thickness = setting.LineTotalGraphCenterThickness;


            List<PointF> centerList = new List<PointF>();
            centerList.Add(new PointF(0, 0));
            centerList.Add(new PointF(float.MaxValue, 0));

            seriesCenterLine.DataSource = centerList;
            if (isTotal)
            {
                if (setting.UseTotalCenterLine)
                    profileChart.Series.Add(seriesCenterLine);
            }

            if (isTotal == false)
            {
                if (isPatten == false)
                {
                    //if (setting.UseLineStop)
                    profileChart.Series.Add(seriesUpperStop);

                    if (setting.UseLineWarning)
                        profileChart.Series.Add(seriesUpperWarning);
                }
                else
                {
                    if (setting.UseLineStopPL)
                        profileChart.Series.Add(seriesUpperStop);
                }
            }


            List<PointF> lowerErrorlist = new List<PointF>();
            value = isPatten == false ? -Convert.ToSingle(setting.LineStopLower) : -Convert.ToSingle(setting.LineStopPL);
            lowerErrorlist.Add(new PointF(0, value));
            lowerErrorlist.Add(new PointF(float.MaxValue, value));

            ScatterLineSeries seriesLowerStop = new ScatterLineSeries();
            seriesLowerStop.Name = "LowerStop";
            seriesLowerStop.XMemberPath = "X";
            seriesLowerStop.YMemberPath = "Y";
            seriesLowerStop.XAxis = xAxis;
            seriesLowerStop.YAxis = yAxis;
            seriesLowerStop.Brush = new SolidBrush(setting.LineStopColor);
            seriesLowerStop.Thickness = setting.LineStopThickness;

            seriesLowerStop.DataSource = lowerErrorlist;

            List<PointF> lowerWarninglist = new List<PointF>();
            value = -Convert.ToSingle(setting.LineWarningLower);
            lowerWarninglist.Add(new PointF(0, value));
            lowerWarninglist.Add(new PointF(float.MaxValue, value));

            ScatterLineSeries seriesLowerWarning = new ScatterLineSeries();

            seriesLowerWarning.Name = "LowerWarning";
            seriesLowerWarning.XMemberPath = "X";
            seriesLowerWarning.YMemberPath = "Y";
            seriesLowerWarning.XAxis = xAxis;
            seriesLowerWarning.YAxis = yAxis;
            seriesLowerWarning.Brush = new SolidBrush(setting.LineWarningColor);
            seriesLowerWarning.Thickness = setting.LineWarningThickness;

            seriesLowerWarning.DataSource = lowerWarninglist;

            if (isTotal == false)
            {
                if (isPatten == false)
                {
                    //if (setting.UseLineStop)
                        profileChart.Series.Add(seriesLowerStop);

                    if (setting.UseLineWarning)
                        profileChart.Series.Add(seriesLowerWarning);
                }
                else
                {
                    if (setting.UseLineStopPL)
                        profileChart.Series.Add(seriesLowerStop);
                }
            }

            seriesLine = new ScatterLineSeries();
            seriesLine.Name = "Data";
            seriesLine.XMemberPath = "X";
            seriesLine.YMemberPath = "Y";
            seriesLine.XAxis = xAxis;
            seriesLine.YAxis = yAxis;
            seriesLine.Brush = new SolidBrush(setting.GraphColor);
            seriesLine.Thickness = setting.GraphThickness / (this.isTotal && this.isReport == false ? 2 : 1);

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
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(info.Value);

            if (isReport == true)
            {
                //TimeSpan timeSpan = TimeSpan.FromMilliseconds(info.Value);
                DateTime dateTime = new DateTime(timeSpan.Ticks + minXAxisTime.Ticks);
                return dateTime.ToString("HH:mm:ss");//string.Format("{0}.{1:00}.{2:00} {3:00}:{4:00}:{5:00}:m", timeSpan., (int)(value / 60 / 1000) % 60, (int)(value / 1000) % 60);
            }

            if (timeSpan.Hours == 0)
                return string.Format("{0:00}:{1:00}s", timeSpan.Minutes, timeSpan.Seconds);
            else
                return string.Format("{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);


            // 기존
            var value = seriesLine.XAxis.MaximumValue - info.Value;

            // 수정
            DateTime dateTimeNow = new DateTime(timeSpan.Ticks);

            if (isTotal == true)
            {
                int hour = (int)(info.Value / 3600 / 1000);
                int min = (int)(info.Value / 60 / 1000) % 60;
                int sec = (int)(info.Value / 1000) % 60;

                string str = "";
                //if (hour == 0)
                //    str = string.Format("{0:00}:{1:00}s", min, sec);
                //else
                    str = string.Format("{0:00}:{1:00}m", hour, min);

                return str;
            }
            else if (isPatten)
            {
                int hour = (int)(info.Value / 3600 / 1000);
                int min = (int)(info.Value / 60 / 1000) % 60;
                int sec = (int)(info.Value / 1000) % 60;

                string str = "";
                if (hour > 0)
                    str = string.Format("{0:00}:{1:00}:{2:00}",hour, min, sec);
                else
                    str = string.Format("{0:00}:{1:00}s", min, sec);

                return str;
            }
            else
            {
                return dateTimeNow.ToString("hh:mm:ss");
            }
        }

        private string FormatLabelY(Infragistics.Win.DataVisualization.AxisLabelInfo info)
        {
            var value = info.Value;
            if (profileOption.YInvert)
                value *= -1;


            string format = "F2";
            int decimalpoint = (int)RVMSSettings.Instance().DecimalpointPl;
            if (isPatten == true)
                format = string.Format("F{0}", decimalpoint);

            bool isMm = this.isPatten ? (this.setting.YAxisUnitPL == YAxisUnitRVMS.Mm) : (this.setting.YAxisUnit == YAxisUnitRVMS.Mm);
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

        public void AddValue(ScanData scanData)
        {
            lock (scanDataList)
            {
                if (scanDataList.Count == 0)
                    minXAxisTime = scanData.X;

                //if (isPatten == false)
                //{
                //    if (isTotal == false)
                //        minXAxisTime = scanData.X.AddSeconds(-RVMSSettings.Instance().Setting.XAxisDisplayTime);
                //    else
                //        minXAxisTime = scanData.X.AddMinutes(-RVMSSettings.Instance().Setting.XAxisDisplayTimeTotalGraph);
                //        //minXAxisTime = scanData.X.AddMinutes(0);
                //}
                //else
                //    minXAxisTime = scanData.X.AddSeconds(-RVMSSettings.Instance().Setting.XAxisDisplayTimePL);
            }

            //scanDataList.RemoveAll(data => data.X < minXAxisTime);
            scanDataList.Add(scanData);

            DisplayResult();
        }

        public void AddScanDataList(List<ScanData> scanDataList)
        {
            this.scanDataList.Clear();
            this.scanDataList = scanDataList;

            if (scanDataList.Count == 0)
                return;

            minXAxisTime = scanDataList.Min(x => x.X);
       
            seriesLine.XAxis.MinimumValue = (scanDataList.Min(x => x.X) - minXAxisTime).TotalMilliseconds;
            seriesLine.XAxis.MaximumValue = (scanDataList.Max(x => x.X) - minXAxisTime).TotalMilliseconds;
            seriesLine.XAxis.Interval = (double)(seriesLine.XAxis.MaximumValue - seriesLine.XAxis.MinimumValue) / setting.XAxisInterval;
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
                            double spanTime = (scanDataList.Max(f => f.X) - this.minXAxisTime).TotalMilliseconds;
                            double axisLen = this.setting.XAxisDisplayTimeTotalGraph * 60 * 1000;
                            if (isReport)
                                axisLen = spanTime;

                            double axisMin = Math.Max(0, spanTime - axisLen);
                            double axisMax = Math.Max(spanTime, axisLen);
                            seriesLine.XAxis.MaximumValue = axisMax;
                            seriesLine.XAxis.MinimumValue = axisMin;

                            scanDataList.ForEach(scanData =>
                            {
                                float x = Math.Abs((float)(scanData.X - minXAxisTime).TotalMilliseconds);
                                float y = scanData.Y;
                                if (profileOption.AutoScaleY == false)
                                    y = scanData.Y < 0 ? (float)Math.Max(scanData.Y, -Math.Abs(setting.YAxisRange - 0.001)) : (float)Math.Min(scanData.Y, Math.Abs(setting.YAxisRange - 0.001));
                                if (axisMin <= x && x <= axisMax)
                                    displayScanDataList.Add(new PointF(x, y));
                            });

                            // Y
                            if (displayScanDataList.Count > 0 && profileOption.AutoScaleY)
                            {
                                seriesLine.YAxis.MaximumValue = displayScanDataList.Max(f => f.Y) + Math.Abs(setting.YAxisRange / 4);
                                seriesLine.YAxis.MinimumValue = displayScanDataList.Min(f => f.Y) - Math.Abs(setting.YAxisRange / 4);
                            }
                            else
                            {
                                seriesLine.YAxis.MaximumValue = 0 + Math.Abs(setting.YAxisRange);
                                seriesLine.YAxis.MinimumValue = 0 - Math.Abs(setting.YAxisRange);
                            }
                            seriesLine.YAxis.Interval = (double)(seriesLine.YAxis.MaximumValue - seriesLine.YAxis.MinimumValue) / setting.YAxisInterval;

                        }
                        else if (isPatten == true)
                        {
                            // x axis
                            double spanTime = (scanDataList.Max(f => f.X) - this.minXAxisTime).TotalMilliseconds;
                            double axisLen = this.setting.XAxisDisplayTimePL * 1000;

                            double axisMin = Math.Max(0, spanTime - axisLen);
                            double axisMax = Math.Max(spanTime, axisLen);
                            seriesLine.XAxis.MaximumValue = axisMax;
                            seriesLine.XAxis.MinimumValue = axisMin;
                            seriesLine.XAxis.Interval = (double)(seriesLine.XAxis.MaximumValue - seriesLine.XAxis.MinimumValue) / setting.XAxisIntervalPL;

                            scanDataList.ForEach(scanData =>
                            {
                                float x = Math.Abs((float)(scanData.X - minXAxisTime).TotalMilliseconds);
                                float y = scanData.Y;
                                if (axisMin <= x && x <= axisMax)
                                    displayScanDataList.Add(new PointF(x, y));
                            });

                            // y axis
                            if (displayScanDataList.Count > 0 && profileOption.AutoScaleY)
                            {
                                seriesLine.YAxis.MaximumValue = displayScanDataList.Max(f => f.Y) + Math.Abs(setting.YAxisRangePL / 4);
                                seriesLine.YAxis.MinimumValue = displayScanDataList.Min(f => f.Y) - Math.Abs(setting.YAxisRangePL / 4);
                            }
                            else
                            {
                                seriesLine.YAxis.MaximumValue = 0 + Math.Abs(setting.YAxisRangePL);
                                seriesLine.YAxis.MinimumValue = 0 - Math.Abs(setting.YAxisRangePL);
                            }
                            seriesLine.YAxis.Interval = (double)(seriesLine.YAxis.MaximumValue - seriesLine.YAxis.MinimumValue) / setting.YAxisIntervalPL;
                        }
                        else
                        {
                            seriesLine.YAxis.MaximumValue = Math.Abs(setting.YAxisRange);
                            seriesLine.YAxis.MinimumValue = -Math.Abs(setting.YAxisRange);
                            seriesLine.YAxis.Interval = (double)(seriesLine.YAxis.MaximumValue - seriesLine.YAxis.MinimumValue) / setting.YAxisInterval;

                            double spanTime = (scanDataList.Max(f => f.X) - this.minXAxisTime).TotalMilliseconds;
                            double axisLen = this.setting.XAxisDisplayTime * 1000;

                            double axisMin = Math.Max(0, spanTime - axisLen);
                            double axisMax = Math.Max(spanTime, axisLen);

                            seriesLine.XAxis.MaximumValue = axisMax;
                            seriesLine.XAxis.MinimumValue = axisMin;
                            seriesLine.XAxis.Interval = (double)(seriesLine.XAxis.MaximumValue - seriesLine.XAxis.MinimumValue) / setting.XAxisInterval;

                            scanDataList.ForEach(scanData =>
                            {
                                float x = Math.Abs((float)(scanData.X - minXAxisTime).TotalMilliseconds);
                                float y = scanData.Y;
                            
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

            int digit = this.isPatten ? 2 : 3;
            if (this.isPatten == true)
                digit = (int)RVMSSettings.Instance().DecimalpointPl;

            layoutMain.SuspendLayout();
            //Array.ForEach(labels, f => UiHelper.SuspendDrawing(f));

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

                bool isMm = this.isPatten ? (this.setting.YAxisUnitPL == YAxisUnitRVMS.Mm) : (this.setting.YAxisUnit == YAxisUnitRVMS.Mm);
                string format = string.Format("F{0}", digit);
                for (int i = 0; i < labels.Length; i++)
                    labels[i].Text = isMm ? values[i].ToString(format) : (values[i] * 1000).ToString();
            }
            else
            {
                float value = 0;
                Array.ForEach(labels, f => f.Text = value.ToString());
            }
            profileChart.PlotAreaBackground = new SolidBrush(setting.BackColor);

            //Array.ForEach(labels, f => UiHelper.ResumeDrawing(f));
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

        public float GetStdDev(List<ScanData> scanDataValidList)
        {
            if (scanDataValidList.Count < 1)
                return 0;

            double var = 0f;
            float avg = scanDataValidList.Average(scanData => scanData.Y);

            foreach (ScanData scanData in scanDataValidList)
            {
                var += Math.Pow((double)(scanData.Y - avg), 2);
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
                        float value = -Convert.ToSingle(setting.LineWarningLower);
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
