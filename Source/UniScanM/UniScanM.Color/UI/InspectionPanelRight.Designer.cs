namespace UniScanM.ColorSens.UI
{
    partial class InspectionPanelRight
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
            this.chart_FullTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label_chartFullTime = new System.Windows.Forms.Label();
            this.label_chartCertainTime = new System.Windows.Forms.Label();
            this.chart_CertainTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_FullTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_CertainTime)).BeginInit();
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
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(791, 546);
            this.panel1.TabIndex = 8;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.chart_FullTime, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label_chartFullTime, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label_chartCertainTime, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.chart_CertainTime, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(791, 546);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // chart_FullTime
            // 
            chartArea1.AxisX.LabelStyle.Format = "{0:0.}";
            chartArea1.AxisX.LineColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.Maximum = 5000D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.Title = "[ m ]";
            chartArea1.AxisX.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea1.AxisY.LabelStyle.ForeColor = System.Drawing.Color.Blue;
            chartArea1.AxisY.LabelStyle.Format = "{0:0.0}";
            chartArea1.AxisY.LineColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.SystemColors.ActiveCaption;
            chartArea1.AxisY.Title = "Brightness";
            chartArea1.AxisY.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea1.Name = "ChartArea1";
            this.chart_FullTime.ChartAreas.Add(chartArea1);
            this.chart_FullTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_FullTime.Location = new System.Drawing.Point(0, 323);
            this.chart_FullTime.Margin = new System.Windows.Forms.Padding(0);
            this.chart_FullTime.Name = "chart_FullTime";
            series1.BorderColor = System.Drawing.Color.CornflowerBlue;
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Color = System.Drawing.Color.Blue;
            series1.MarkerBorderColor = System.Drawing.Color.Blue;
            series1.Name = "graphdata";
            this.chart_FullTime.Series.Add(series1);
            this.chart_FullTime.Size = new System.Drawing.Size(791, 223);
            this.chart_FullTime.TabIndex = 4;
            this.chart_FullTime.Text = "chart1";
            // 
            // label_chartFullTime
            // 
            this.label_chartFullTime.BackColor = System.Drawing.Color.Black;
            this.label_chartFullTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_chartFullTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label_chartFullTime.Font = new System.Drawing.Font("Malgun Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_chartFullTime.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label_chartFullTime.Location = new System.Drawing.Point(0, 273);
            this.label_chartFullTime.Margin = new System.Windows.Forms.Padding(0);
            this.label_chartFullTime.Name = "label_chartFullTime";
            this.label_chartFullTime.Size = new System.Drawing.Size(791, 50);
            this.label_chartFullTime.TabIndex = 3;
            this.label_chartFullTime.Text = "Whole Distance";
            this.label_chartFullTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.label_chartCertainTime.Size = new System.Drawing.Size(791, 50);
            this.label_chartCertainTime.TabIndex = 3;
            this.label_chartCertainTime.Text = "Certain Time";
            this.label_chartCertainTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chart_CertainTime
            // 
            chartArea2.AxisX.LabelStyle.Format = "{0:0.}";
            chartArea2.AxisX.LabelStyle.Interval = 0D;
            chartArea2.AxisX.LabelStyle.IntervalOffset = 0D;
            chartArea2.AxisX.LabelStyle.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Auto;
            chartArea2.AxisX.LineColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisX.Maximum = 100D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.AxisX.Title = "[ m ]";
            chartArea2.AxisX.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea2.AxisY.IsMarginVisible = false;
            chartArea2.AxisY.IsStartedFromZero = false;
            chartArea2.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea2.AxisY.LabelStyle.ForeColor = System.Drawing.Color.Blue;
            chartArea2.AxisY.LabelStyle.Format = "{0:0.0}";
            chartArea2.AxisY.LabelStyle.Interval = 0D;
            chartArea2.AxisY.LabelStyle.IntervalOffset = 0D;
            chartArea2.AxisY.LineColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.SystemColors.ActiveCaption;
            chartArea2.AxisY.Maximum = 100D;
            chartArea2.AxisY.Minimum = 0D;
            chartArea2.AxisY.ScaleView.MinSizeType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea2.AxisY.ScaleView.Zoomable = false;
            chartArea2.AxisY.ScrollBar.Enabled = false;
            chartArea2.AxisY.Title = "Brightness";
            chartArea2.AxisY.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea2.Name = "ChartArea1";
            this.chart_CertainTime.ChartAreas.Add(chartArea2);
            this.chart_CertainTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart_CertainTime.Location = new System.Drawing.Point(0, 50);
            this.chart_CertainTime.Margin = new System.Windows.Forms.Padding(0);
            this.chart_CertainTime.Name = "chart_CertainTime";
            series2.BorderColor = System.Drawing.Color.CornflowerBlue;
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.Red;
            series2.MarkerBorderColor = System.Drawing.Color.Blue;
            series2.Name = "graphdata";
            this.chart_CertainTime.Series.Add(series2);
            this.chart_CertainTime.Size = new System.Drawing.Size(791, 223);
            this.chart_CertainTime.TabIndex = 4;
            this.chart_CertainTime.Text = "chart1";
            // 
            // InspectionPanelRight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "InspectionPanelRight";
            this.Size = new System.Drawing.Size(791, 546);
            this.Load += new System.EventHandler(this.InspectPage_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_FullTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_CertainTime)).EndInit();
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
    }
}
