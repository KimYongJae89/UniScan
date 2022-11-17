namespace UniScanM.ColorSens.UI
{
    partial class InspectionPanelRightSetting
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label_chartCertainTime = new System.Windows.Forms.Label();
            this.panelParam = new System.Windows.Forms.Panel();
            this.button_AutoLight = new System.Windows.Forms.Button();
            this.rollerDia = new System.Windows.Forms.NumericUpDown();
            this.ngRange = new System.Windows.Forms.NumericUpDown();
            this.button_GetRollerDiaFromPLC = new System.Windows.Forms.Button();
            this.lightValue = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chart_CertainTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label_chartFullTime = new System.Windows.Forms.Label();
            this.chart_FullTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.checkOnTune = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panelParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rollerDia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ngRange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lightValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_CertainTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_FullTime)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Controls.Add(this.checkOnTune);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(485, 590);
            this.panel1.TabIndex = 8;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label_chartCertainTime, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panelParam, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.chart_CertainTime, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label_chartFullTime, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.chart_FullTime, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.Size = new System.Drawing.Size(485, 540);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // label_chartCertainTime
            // 
            this.label_chartCertainTime.BackColor = System.Drawing.Color.Black;
            this.label_chartCertainTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_chartCertainTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_chartCertainTime.Font = new System.Drawing.Font("Malgun Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_chartCertainTime.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label_chartCertainTime.Location = new System.Drawing.Point(0, 0);
            this.label_chartCertainTime.Margin = new System.Windows.Forms.Padding(0);
            this.label_chartCertainTime.Name = "label_chartCertainTime";
            this.label_chartCertainTime.Size = new System.Drawing.Size(485, 40);
            this.label_chartCertainTime.TabIndex = 3;
            this.label_chartCertainTime.Text = "Certain Time";
            this.label_chartCertainTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelParam
            // 
            this.panelParam.AutoSize = true;
            this.panelParam.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelParam.Controls.Add(this.button_AutoLight);
            this.panelParam.Controls.Add(this.rollerDia);
            this.panelParam.Controls.Add(this.ngRange);
            this.panelParam.Controls.Add(this.button_GetRollerDiaFromPLC);
            this.panelParam.Controls.Add(this.lightValue);
            this.panelParam.Controls.Add(this.label7);
            this.panelParam.Controls.Add(this.label4);
            this.panelParam.Controls.Add(this.label9);
            this.panelParam.Controls.Add(this.label5);
            this.panelParam.Controls.Add(this.label10);
            this.panelParam.Controls.Add(this.label2);
            this.panelParam.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelParam.Enabled = false;
            this.panelParam.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.panelParam.Location = new System.Drawing.Point(0, 430);
            this.panelParam.Margin = new System.Windows.Forms.Padding(0);
            this.panelParam.Name = "panelParam";
            this.panelParam.Size = new System.Drawing.Size(485, 110);
            this.panelParam.TabIndex = 11;
            // 
            // button_AutoLight
            // 
            this.button_AutoLight.Location = new System.Drawing.Point(306, 43);
            this.button_AutoLight.Margin = new System.Windows.Forms.Padding(0);
            this.button_AutoLight.Name = "button_AutoLight";
            this.button_AutoLight.Size = new System.Drawing.Size(133, 29);
            this.button_AutoLight.TabIndex = 0;
            this.button_AutoLight.Text = "Auto Light";
            this.button_AutoLight.UseVisualStyleBackColor = true;
            this.button_AutoLight.Click += new System.EventHandler(this.button_AutoLight_Click);
            // 
            // rollerDia
            // 
            this.rollerDia.DecimalPlaces = 2;
            this.rollerDia.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.rollerDia.Location = new System.Drawing.Point(145, 78);
            this.rollerDia.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.rollerDia.Name = "rollerDia";
            this.rollerDia.Size = new System.Drawing.Size(103, 29);
            this.rollerDia.TabIndex = 7;
            this.rollerDia.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.rollerDia.ValueChanged += new System.EventHandler(this.Recipe_ValueChanged);
            // 
            // ngRange
            // 
            this.ngRange.DecimalPlaces = 2;
            this.ngRange.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ngRange.Location = new System.Drawing.Point(145, 4);
            this.ngRange.Name = "ngRange";
            this.ngRange.Size = new System.Drawing.Size(103, 29);
            this.ngRange.TabIndex = 6;
            this.ngRange.ValueChanged += new System.EventHandler(this.Recipe_ValueChanged);
            // 
            // button_GetRollerDiaFromPLC
            // 
            this.button_GetRollerDiaFromPLC.Location = new System.Drawing.Point(306, 80);
            this.button_GetRollerDiaFromPLC.Margin = new System.Windows.Forms.Padding(0);
            this.button_GetRollerDiaFromPLC.Name = "button_GetRollerDiaFromPLC";
            this.button_GetRollerDiaFromPLC.Size = new System.Drawing.Size(133, 29);
            this.button_GetRollerDiaFromPLC.TabIndex = 0;
            this.button_GetRollerDiaFromPLC.Text = "<< PLC";
            this.button_GetRollerDiaFromPLC.UseVisualStyleBackColor = true;
            this.button_GetRollerDiaFromPLC.Click += new System.EventHandler(this.button_GetRollerDiaFromPLC_Click);
            // 
            // lightValue
            // 
            this.lightValue.DecimalPlaces = 2;
            this.lightValue.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.lightValue.Location = new System.Drawing.Point(145, 41);
            this.lightValue.Name = "lightValue";
            this.lightValue.Size = new System.Drawing.Size(103, 29);
            this.lightValue.TabIndex = 4;
            this.lightValue.ValueChanged += new System.EventHandler(this.Recipe_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(254, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 21);
            this.label7.TabIndex = 1;
            this.label7.Text = "[DN]";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 21);
            this.label4.TabIndex = 1;
            this.label4.Text = "Light";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(254, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 21);
            this.label9.TabIndex = 1;
            this.label9.Text = "[%]";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 21);
            this.label5.TabIndex = 1;
            this.label5.Text = "Roller Dia";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(254, 82);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 21);
            this.label10.TabIndex = 1;
            this.label10.Text = "[㎜]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "NG Range";
            // 
            // chart_CertainTime
            // 
            chartArea1.AxisX.LabelStyle.Format = "{0:0.}";
            chartArea1.AxisX.LabelStyle.Interval = 0D;
            chartArea1.AxisX.LabelStyle.IntervalOffset = 0D;
            chartArea1.AxisX.LabelStyle.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea1.AxisX.LineColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.Maximum = 100D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.Title = "[ m ]";
            chartArea1.AxisX.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea1.AxisY.IsInterlaced = true;
            chartArea1.AxisY.IsMarginVisible = false;
            chartArea1.AxisY.IsStartedFromZero = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.Blue;
            chartArea1.AxisY.LabelStyle.Format = "{0:0.0}";
            chartArea1.AxisY.LabelStyle.Interval = 0D;
            chartArea1.AxisY.LabelStyle.IntervalOffset = 0D;
            chartArea1.AxisY.LineColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.SystemColors.ActiveCaption;
            chartArea1.AxisY.Maximum = 1D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.AxisY.ScaleView.MinSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisY.ScaleView.Zoomable = false;
            chartArea1.AxisY.ScrollBar.Enabled = false;
            chartArea1.AxisY.Title = "Brightness";
            chartArea1.Name = "ChartArea1";
            this.chart_CertainTime.ChartAreas.Add(chartArea1);
            this.chart_CertainTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_CertainTime.Location = new System.Drawing.Point(0, 40);
            this.chart_CertainTime.Margin = new System.Windows.Forms.Padding(0);
            this.chart_CertainTime.Name = "chart_CertainTime";
            series1.BorderColor = System.Drawing.Color.CornflowerBlue;
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.SystemColors.Highlight;
            series1.IsVisibleInLegend = false;
            series1.MarkerBorderColor = System.Drawing.Color.Blue;
            series1.Name = "graphdata";
            this.chart_CertainTime.Series.Add(series1);
            this.chart_CertainTime.Size = new System.Drawing.Size(485, 210);
            this.chart_CertainTime.TabIndex = 4;
            this.chart_CertainTime.Text = "chart1";
            // 
            // label_chartFullTime
            // 
            this.label_chartFullTime.BackColor = System.Drawing.Color.Black;
            this.label_chartFullTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_chartFullTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_chartFullTime.Font = new System.Drawing.Font("Malgun Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_chartFullTime.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label_chartFullTime.Location = new System.Drawing.Point(0, 250);
            this.label_chartFullTime.Margin = new System.Windows.Forms.Padding(0);
            this.label_chartFullTime.Name = "label_chartFullTime";
            this.label_chartFullTime.Size = new System.Drawing.Size(485, 40);
            this.label_chartFullTime.TabIndex = 3;
            this.label_chartFullTime.Text = "Whole Distance";
            this.label_chartFullTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chart_FullTime
            // 
            chartArea2.AxisX.LabelStyle.Format = "{0:0.}";
            chartArea2.AxisX.LineColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisX.Maximum = 5000D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.AxisX.Title = "[ m ]";
            chartArea2.AxisX.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea2.AxisY.IsLabelAutoFit = false;
            chartArea2.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea2.AxisY.LabelStyle.ForeColor = System.Drawing.Color.Blue;
            chartArea2.AxisY.LabelStyle.Format = "{0:0.0}";
            chartArea2.AxisY.LineColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.SystemColors.ActiveCaption;
            chartArea2.AxisY.Title = "Brightness";
            chartArea2.AxisY.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea2.Name = "ChartArea1";
            this.chart_FullTime.ChartAreas.Add(chartArea2);
            this.chart_FullTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_FullTime.Location = new System.Drawing.Point(0, 290);
            this.chart_FullTime.Margin = new System.Windows.Forms.Padding(0);
            this.chart_FullTime.Name = "chart_FullTime";
            series2.BorderColor = System.Drawing.Color.CornflowerBlue;
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Blue;
            series2.MarkerBorderColor = System.Drawing.Color.Blue;
            series2.Name = "graphdata";
            this.chart_FullTime.Series.Add(series2);
            this.chart_FullTime.Size = new System.Drawing.Size(485, 140);
            this.chart_FullTime.TabIndex = 4;
            this.chart_FullTime.Text = "chart1";
            // 
            // checkOnTune
            // 
            this.checkOnTune.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkOnTune.BackColor = System.Drawing.Color.Gray;
            this.checkOnTune.Checked = true;
            this.checkOnTune.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkOnTune.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkOnTune.FlatAppearance.CheckedBackColor = System.Drawing.Color.SeaGreen;
            this.checkOnTune.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkOnTune.Font = new System.Drawing.Font("Malgun Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkOnTune.Location = new System.Drawing.Point(0, 540);
            this.checkOnTune.Name = "checkOnTune";
            this.checkOnTune.Size = new System.Drawing.Size(485, 50);
            this.checkOnTune.TabIndex = 10;
            this.checkOnTune.Text = "Comm Open/Close";
            this.checkOnTune.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkOnTune.UseVisualStyleBackColor = false;
            this.checkOnTune.CheckedChanged += new System.EventHandler(this.checkOnTune_CheckedChanged);
            // 
            // InspectionPanelRightSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "InspectionPanelRightSetting";
            this.Size = new System.Drawing.Size(485, 590);
            this.Load += new System.EventHandler(this.InspectPage_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panelParam.ResumeLayout(false);
            this.panelParam.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rollerDia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ngRange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lightValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_CertainTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_FullTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_chartCertainTime;
        private System.Windows.Forms.Label label_chartFullTime;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_FullTime;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_CertainTime;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button_AutoLight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_GetRollerDiaFromPLC;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown rollerDia;
        private System.Windows.Forms.NumericUpDown ngRange;
        private System.Windows.Forms.NumericUpDown lightValue;
        private System.Windows.Forms.CheckBox checkOnTune;
        private System.Windows.Forms.Panel panelParam;
    }
}
