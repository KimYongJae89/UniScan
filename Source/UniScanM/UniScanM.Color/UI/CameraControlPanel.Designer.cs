
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.Devices;
using DynMvp.Vision;
using DynMvp.Devices.Light;
using DynMvp.Data.UI;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Base;
using System.Threading;
using UniEye.Base.UI;
using UniEye.Base.UI.CameraCalibration;


namespace UniScanM.ColorSens.UI
{
    partial class CameraControlPanel
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraControlPanel));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label_Result = new System.Windows.Forms.Label();
            this.button_Calculate = new System.Windows.Forms.Button();
            this.buttonCameraCalibration = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCameraFreq = new System.Windows.Forms.Label();
            this.buttonCameraGrabStop = new System.Windows.Forms.Button();
            this.comboBoxCameraDevice = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.buttonCameraGrabExposure = new System.Windows.Forms.NumericUpDown();
            this.buttonCameraGrabMulti = new System.Windows.Forms.Button();
            this.buttonCameraGrabOnce = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonLightAllOff = new System.Windows.Forms.Button();
            this.buttonLightAllOn = new System.Windows.Forms.Button();
            this.dataGridViewLight = new System.Windows.Forms.DataGridView();
            this.columnLightCtrlNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLightChannelNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLightValue = new Infragistics.Win.UltraDataGridView.UltraNumericEditorColumn(this.components);
            this.chart_Histogram = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_Projection = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chart_Result = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_LightTune = new System.Windows.Forms.Button();
            this.button_GetBrightness = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonCameraGrabExposure)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Histogram)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Projection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Result)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(3, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(667, 568);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_GetBrightness);
            this.groupBox2.Controls.Add(this.button_LightTune);
            this.groupBox2.Controls.Add(this.label_Result);
            this.groupBox2.Controls.Add(this.button_Calculate);
            this.groupBox2.Controls.Add(this.buttonCameraCalibration);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.labelCameraFreq);
            this.groupBox2.Controls.Add(this.buttonCameraGrabStop);
            this.groupBox2.Controls.Add(this.comboBoxCameraDevice);
            this.groupBox2.Controls.Add(this.numericUpDown1);
            this.groupBox2.Controls.Add(this.buttonCameraGrabExposure);
            this.groupBox2.Controls.Add(this.buttonCameraGrabMulti);
            this.groupBox2.Controls.Add(this.buttonCameraGrabOnce);
            this.groupBox2.Location = new System.Drawing.Point(676, 3);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(294, 174);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Camera Control";
            // 
            // label_Result
            // 
            this.label_Result.AutoSize = true;
            this.label_Result.Location = new System.Drawing.Point(143, 152);
            this.label_Result.Name = "label_Result";
            this.label_Result.Size = new System.Drawing.Size(38, 12);
            this.label_Result.TabIndex = 9;
            this.label_Result.Text = "label3";
            // 
            // button_Calculate
            // 
            this.button_Calculate.Location = new System.Drawing.Point(139, 117);
            this.button_Calculate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Calculate.Name = "button_Calculate";
            this.button_Calculate.Size = new System.Drawing.Size(92, 27);
            this.button_Calculate.TabIndex = 8;
            this.button_Calculate.Text = "Calculate";
            this.button_Calculate.UseVisualStyleBackColor = true;
            this.button_Calculate.Click += new System.EventHandler(this.button_Calculate_Click);
            // 
            // buttonCameraCalibration
            // 
            this.buttonCameraCalibration.Location = new System.Drawing.Point(139, 86);
            this.buttonCameraCalibration.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCameraCalibration.Name = "buttonCameraCalibration";
            this.buttonCameraCalibration.Size = new System.Drawing.Size(92, 27);
            this.buttonCameraCalibration.TabIndex = 8;
            this.buttonCameraCalibration.Text = "Calibration";
            this.buttonCameraCalibration.UseVisualStyleBackColor = true;
            this.buttonCameraCalibration.Click += new System.EventHandler(this.buttonCameraCalibration_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(226, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Hz";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(226, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "[us]";
            // 
            // labelCameraFreq
            // 
            this.labelCameraFreq.AutoSize = true;
            this.labelCameraFreq.Location = new System.Drawing.Point(137, 42);
            this.labelCameraFreq.Name = "labelCameraFreq";
            this.labelCameraFreq.Size = new System.Drawing.Size(38, 12);
            this.labelCameraFreq.TabIndex = 5;
            this.labelCameraFreq.Text = "label1";
            // 
            // buttonCameraGrabStop
            // 
            this.buttonCameraGrabStop.Location = new System.Drawing.Point(18, 118);
            this.buttonCameraGrabStop.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCameraGrabStop.Name = "buttonCameraGrabStop";
            this.buttonCameraGrabStop.Size = new System.Drawing.Size(92, 27);
            this.buttonCameraGrabStop.TabIndex = 4;
            this.buttonCameraGrabStop.Text = "Grab Stop";
            this.buttonCameraGrabStop.UseVisualStyleBackColor = true;
            this.buttonCameraGrabStop.Click += new System.EventHandler(this.buttonCameraGrabStop_Click);
            // 
            // comboBoxCameraDevice
            // 
            this.comboBoxCameraDevice.FormattingEnabled = true;
            this.comboBoxCameraDevice.Location = new System.Drawing.Point(18, 18);
            this.comboBoxCameraDevice.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBoxCameraDevice.Name = "comboBoxCameraDevice";
            this.comboBoxCameraDevice.Size = new System.Drawing.Size(106, 20);
            this.comboBoxCameraDevice.TabIndex = 3;
            this.comboBoxCameraDevice.SelectedIndexChanged += new System.EventHandler(this.comboBoxCameraDevice_SelectedIndexChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 4;
            this.numericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.numericUpDown1.Location = new System.Drawing.Point(139, 60);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(81, 21);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.buttonCameraGrabExposure_ValueChanged);
            // 
            // buttonCameraGrabExposure
            // 
            this.buttonCameraGrabExposure.DecimalPlaces = 4;
            this.buttonCameraGrabExposure.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.buttonCameraGrabExposure.Location = new System.Drawing.Point(139, 19);
            this.buttonCameraGrabExposure.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCameraGrabExposure.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.buttonCameraGrabExposure.Name = "buttonCameraGrabExposure";
            this.buttonCameraGrabExposure.Size = new System.Drawing.Size(81, 21);
            this.buttonCameraGrabExposure.TabIndex = 2;
            this.buttonCameraGrabExposure.ValueChanged += new System.EventHandler(this.buttonCameraGrabExposure_ValueChanged);
            // 
            // buttonCameraGrabMulti
            // 
            this.buttonCameraGrabMulti.Location = new System.Drawing.Point(18, 86);
            this.buttonCameraGrabMulti.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCameraGrabMulti.Name = "buttonCameraGrabMulti";
            this.buttonCameraGrabMulti.Size = new System.Drawing.Size(92, 27);
            this.buttonCameraGrabMulti.TabIndex = 1;
            this.buttonCameraGrabMulti.Text = "Grab Multi";
            this.buttonCameraGrabMulti.UseVisualStyleBackColor = true;
            this.buttonCameraGrabMulti.Click += new System.EventHandler(this.buttonCameraGrabMulti_Click);
            // 
            // buttonCameraGrabOnce
            // 
            this.buttonCameraGrabOnce.Location = new System.Drawing.Point(18, 54);
            this.buttonCameraGrabOnce.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonCameraGrabOnce.Name = "buttonCameraGrabOnce";
            this.buttonCameraGrabOnce.Size = new System.Drawing.Size(92, 27);
            this.buttonCameraGrabOnce.TabIndex = 0;
            this.buttonCameraGrabOnce.Text = "Grab Once";
            this.buttonCameraGrabOnce.UseVisualStyleBackColor = true;
            this.buttonCameraGrabOnce.Click += new System.EventHandler(this.buttonCameraGrabOnce_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonLightAllOff);
            this.groupBox3.Controls.Add(this.buttonLightAllOn);
            this.groupBox3.Controls.Add(this.dataGridViewLight);
            this.groupBox3.Location = new System.Drawing.Point(676, 174);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox3.Size = new System.Drawing.Size(294, 169);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Light Control";
            // 
            // buttonLightAllOff
            // 
            this.buttonLightAllOff.Location = new System.Drawing.Point(128, 23);
            this.buttonLightAllOff.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonLightAllOff.Name = "buttonLightAllOff";
            this.buttonLightAllOff.Size = new System.Drawing.Size(92, 27);
            this.buttonLightAllOff.TabIndex = 5;
            this.buttonLightAllOff.Text = "ALL OFF";
            this.buttonLightAllOff.UseVisualStyleBackColor = true;
            this.buttonLightAllOff.Click += new System.EventHandler(this.buttonLightAllOff_Click);
            // 
            // buttonLightAllOn
            // 
            this.buttonLightAllOn.Location = new System.Drawing.Point(18, 23);
            this.buttonLightAllOn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonLightAllOn.Name = "buttonLightAllOn";
            this.buttonLightAllOn.Size = new System.Drawing.Size(92, 27);
            this.buttonLightAllOn.TabIndex = 5;
            this.buttonLightAllOn.Text = "ALL ON";
            this.buttonLightAllOn.UseVisualStyleBackColor = true;
            this.buttonLightAllOn.Click += new System.EventHandler(this.buttonLightAllOn_Click);
            // 
            // dataGridViewLight
            // 
            this.dataGridViewLight.AllowUserToAddRows = false;
            this.dataGridViewLight.AllowUserToDeleteRows = false;
            this.dataGridViewLight.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridViewLight.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLight.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnLightCtrlNo,
            this.columnLightChannelNo,
            this.columnLightValue});
            this.dataGridViewLight.Location = new System.Drawing.Point(18, 57);
            this.dataGridViewLight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewLight.Name = "dataGridViewLight";
            this.dataGridViewLight.RowHeadersVisible = false;
            this.dataGridViewLight.RowTemplate.Height = 27;
            this.dataGridViewLight.Size = new System.Drawing.Size(261, 97);
            this.dataGridViewLight.TabIndex = 0;
            this.dataGridViewLight.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLight_CellValueChanged);
            // 
            // columnLightCtrlNo
            // 
            this.columnLightCtrlNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnLightCtrlNo.HeaderText = "Controller";
            this.columnLightCtrlNo.Name = "columnLightCtrlNo";
            this.columnLightCtrlNo.ReadOnly = true;
            this.columnLightCtrlNo.Width = 84;
            // 
            // columnLightChannelNo
            // 
            this.columnLightChannelNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnLightChannelNo.HeaderText = "Channel";
            this.columnLightChannelNo.Name = "columnLightChannelNo";
            this.columnLightChannelNo.ReadOnly = true;
            this.columnLightChannelNo.Width = 77;
            // 
            // columnLightValue
            // 
            this.columnLightValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnLightValue.DefaultNewRowValue = ((object)(resources.GetObject("columnLightValue.DefaultNewRowValue")));
            this.columnLightValue.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.columnLightValue.HeaderText = "Value";
            this.columnLightValue.MaskInput = null;
            this.columnLightValue.Name = "columnLightValue";
            this.columnLightValue.PadChar = '\0';
            this.columnLightValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.columnLightValue.SpinButtonAlignment = Infragistics.Win.SpinButtonDisplayStyle.None;
            // 
            // chart_Histogram
            // 
            chartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisX.LineWidth = 0;
            chartArea1.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisX2.IsMarginVisible = false;
            chartArea1.AxisX2.LineWidth = 0;
            chartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisY.IsMarginVisible = false;
            chartArea1.AxisY.LineWidth = 0;
            chartArea1.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisY2.IsLogarithmic = true;
            chartArea1.AxisY2.LineWidth = 0;
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.InnerPlotPosition.Auto = false;
            chartArea1.InnerPlotPosition.Height = 100F;
            chartArea1.InnerPlotPosition.Width = 100F;
            chartArea1.Name = "ChartArea1";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 100F;
            chartArea1.Position.Width = 100F;
            this.chart_Histogram.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.chart_Histogram.Legends.Add(legend1);
            this.chart_Histogram.Location = new System.Drawing.Point(676, 352);
            this.chart_Histogram.Name = "chart_Histogram";
            series1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            series1.IsVisibleInLegend = false;
            series1.Legend = "Legend1";
            series1.MarkerBorderWidth = 0;
            series1.MarkerSize = 1;
            series1.Name = "Series1";
            series1.ShadowColor = System.Drawing.Color.Transparent;
            series1.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series2.BorderColor = System.Drawing.Color.Red;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series2.Color = System.Drawing.Color.PaleVioletRed;
            series2.Legend = "Legend1";
            series2.Name = "Series2";
            this.chart_Histogram.Series.Add(series1);
            this.chart_Histogram.Series.Add(series2);
            this.chart_Histogram.Size = new System.Drawing.Size(294, 221);
            this.chart_Histogram.TabIndex = 3;
            this.chart_Histogram.Text = "chart1";
            // 
            // chart_Projection
            // 
            chartArea2.AlignmentOrientation = System.Windows.Forms.DataVisualization.Charting.AreaAlignmentOrientations.Horizontal;
            chartArea2.Area3DStyle.Inclination = 90;
            chartArea2.Area3DStyle.IsRightAngleAxes = false;
            chartArea2.Area3DStyle.PointDepth = 400;
            chartArea2.Area3DStyle.PointGapDepth = 1;
            chartArea2.Area3DStyle.Rotation = 90;
            chartArea2.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea2.AxisX.IsMarginVisible = false;
            chartArea2.AxisX.LineWidth = 0;
            chartArea2.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea2.AxisX2.IsMarginVisible = false;
            chartArea2.AxisX2.LineWidth = 0;
            chartArea2.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea2.AxisY.IsMarginVisible = false;
            chartArea2.AxisY.LineWidth = 0;
            chartArea2.AxisY.Maximum = 256D;
            chartArea2.AxisY.Minimum = 0D;
            chartArea2.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea2.AxisY2.LineWidth = 0;
            chartArea2.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea2.InnerPlotPosition.Auto = false;
            chartArea2.InnerPlotPosition.Height = 100F;
            chartArea2.InnerPlotPosition.Width = 100F;
            chartArea2.Name = "ChartArea1";
            chartArea2.Position.Auto = false;
            chartArea2.Position.Height = 100F;
            chartArea2.Position.Width = 100F;
            this.chart_Projection.ChartAreas.Add(chartArea2);
            legend2.Enabled = false;
            legend2.Name = "Legend1";
            this.chart_Projection.Legends.Add(legend2);
            this.chart_Projection.Location = new System.Drawing.Point(3, 578);
            this.chart_Projection.Name = "chart_Projection";
            series3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            series3.IsVisibleInLegend = false;
            series3.Legend = "Legend1";
            series3.MarkerBorderWidth = 0;
            series3.MarkerSize = 1;
            series3.Name = "projection";
            series3.ShadowColor = System.Drawing.Color.Transparent;
            series3.XAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            this.chart_Projection.Series.Add(series3);
            this.chart_Projection.Size = new System.Drawing.Size(667, 211);
            this.chart_Projection.TabIndex = 3;
            this.chart_Projection.Text = "chart1";
            // 
            // chart_Result
            // 
            chartArea3.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea3.AxisX.IsMarginVisible = false;
            chartArea3.AxisX.LineWidth = 0;
            chartArea3.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea3.AxisX2.IsMarginVisible = false;
            chartArea3.AxisX2.LineWidth = 0;
            chartArea3.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea3.AxisY.IsMarginVisible = false;
            chartArea3.AxisY.LineWidth = 0;
            chartArea3.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea3.AxisY2.IsLogarithmic = true;
            chartArea3.AxisY2.LineWidth = 0;
            chartArea3.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea3.InnerPlotPosition.Auto = false;
            chartArea3.InnerPlotPosition.Height = 100F;
            chartArea3.InnerPlotPosition.Width = 100F;
            chartArea3.Name = "ChartArea1";
            chartArea3.Position.Auto = false;
            chartArea3.Position.Height = 100F;
            chartArea3.Position.Width = 100F;
            this.chart_Result.ChartAreas.Add(chartArea3);
            legend3.Enabled = false;
            legend3.Name = "Legend1";
            this.chart_Result.Legends.Add(legend3);
            this.chart_Result.Location = new System.Drawing.Point(676, 579);
            this.chart_Result.Name = "chart_Result";
            series4.BorderColor = System.Drawing.Color.Black;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series4.Color = System.Drawing.SystemColors.ControlDark;
            series4.IsVisibleInLegend = false;
            series4.Legend = "Legend1";
            series4.MarkerBorderWidth = 0;
            series4.MarkerSize = 1;
            series4.Name = "Series1";
            series4.ShadowColor = System.Drawing.Color.Transparent;
            series4.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
            series5.BorderColor = System.Drawing.Color.Red;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series5.Color = System.Drawing.Color.PaleVioletRed;
            series5.IsVisibleInLegend = false;
            series5.Legend = "Legend1";
            series5.Name = "Series2";
            this.chart_Result.Series.Add(series4);
            this.chart_Result.Series.Add(series5);
            this.chart_Result.Size = new System.Drawing.Size(294, 210);
            this.chart_Result.TabIndex = 3;
            this.chart_Result.Text = "chart1";
            // 
            // button_LightTune
            // 
            this.button_LightTune.Location = new System.Drawing.Point(237, 86);
            this.button_LightTune.Name = "button_LightTune";
            this.button_LightTune.Size = new System.Drawing.Size(56, 27);
            this.button_LightTune.TabIndex = 10;
            this.button_LightTune.Text = "TuneL";
            this.button_LightTune.UseVisualStyleBackColor = true;
            this.button_LightTune.Click += new System.EventHandler(this.button_LightTune_Click);
            // 
            // button_GetBrightness
            // 
            this.button_GetBrightness.Location = new System.Drawing.Point(237, 117);
            this.button_GetBrightness.Name = "button_GetBrightness";
            this.button_GetBrightness.Size = new System.Drawing.Size(56, 28);
            this.button_GetBrightness.TabIndex = 11;
            this.button_GetBrightness.Text = "Bright";
            this.button_GetBrightness.UseVisualStyleBackColor = true;
            this.button_GetBrightness.Click += new System.EventHandler(this.button_GetBrightness_Click);
            // 
            // CameraControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chart_Projection);
            this.Controls.Add(this.chart_Result);
            this.Controls.Add(this.chart_Histogram);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "CameraControlPanel";
            this.Size = new System.Drawing.Size(975, 792);
            this.Load += new System.EventHandler(this.CameraControlPanel_Load);
            this.Leave += new System.EventHandler(this.CameraControlPanel_Leave);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonCameraGrabExposure)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Histogram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Projection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Result)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown buttonCameraGrabExposure;
        private System.Windows.Forms.Button buttonCameraGrabMulti;
        private System.Windows.Forms.Button buttonCameraGrabOnce;
        private System.Windows.Forms.DataGridView dataGridViewLight;
        private System.Windows.Forms.ComboBox comboBoxCameraDevice;
        private System.Windows.Forms.Button buttonCameraGrabStop;
        private System.Windows.Forms.Button buttonLightAllOff;
        private System.Windows.Forms.Button buttonLightAllOn;
        private System.Windows.Forms.Label labelCameraFreq;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCameraCalibration;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLightCtrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLightChannel;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLightCtrlNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLightChannelNo;
        private Infragistics.Win.UltraDataGridView.UltraNumericEditorColumn columnLightValue;
        private NumericUpDown numericUpDown1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Histogram;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Projection;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Result;
        private Button button_Calculate;
        private Label label_Result;
        private Button button_LightTune;
        private Button button_GetBrightness;
    }
}
