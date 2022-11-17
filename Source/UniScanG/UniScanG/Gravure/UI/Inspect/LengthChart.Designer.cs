namespace UniScanG.Gravure.UI.Inspect
{
    partial class LengthChart
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.chartLength = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelTrandChartDefect = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelAverage = new System.Windows.Forms.Label();
            this.labelStdDev = new System.Windows.Forms.Label();
            this.labelMin = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.labelRange = new System.Windows.Forms.Label();
            this.average = new System.Windows.Forms.TextBox();
            this.labelAverageUnit = new System.Windows.Forms.Label();
            this.labelStdDevUnit = new System.Windows.Forms.Label();
            this.labelMinUnit = new System.Windows.Forms.Label();
            this.labelMaxUnit = new System.Windows.Forms.Label();
            this.labelRangeUnit = new System.Windows.Forms.Label();
            this.stdDev = new System.Windows.Forms.TextBox();
            this.min = new System.Windows.Forms.TextBox();
            this.max = new System.Windows.Forms.TextBox();
            this.range = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartLength)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chartLength
            // 
            this.chartLength.BackColor = System.Drawing.SystemColors.Control;
            chartArea1.AxisX.Title = "Sheet No";
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("맑은 고딕", 10F);
            chartArea1.AxisY.Title = "Length [mm]";
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("맑은 고딕", 10F);
            chartArea1.BackColor = System.Drawing.SystemColors.Control;
            chartArea1.Name = "ChartArea1";
            this.chartLength.ChartAreas.Add(chartArea1);
            this.chartLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartLength.Location = new System.Drawing.Point(0, 35);
            this.chartLength.Margin = new System.Windows.Forms.Padding(0);
            this.chartLength.Name = "chartLength";
            this.chartLength.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Fire;
            this.chartLength.Size = new System.Drawing.Size(631, 272);
            this.chartLength.TabIndex = 70;
            this.chartLength.Text = "chart1";
            this.chartLength.SizeChanged += new System.EventHandler(this.chartLength_SizeChanged);
            this.chartLength.Paint += new System.Windows.Forms.PaintEventHandler(this.chartLength_Paint);
            this.chartLength.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chartLength_MouseMove);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelTrandChartDefect, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chartLength, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(631, 374);
            this.tableLayoutPanel1.TabIndex = 71;
            // 
            // labelTrandChartDefect
            // 
            this.labelTrandChartDefect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelTrandChartDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTrandChartDefect.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelTrandChartDefect.Location = new System.Drawing.Point(0, 0);
            this.labelTrandChartDefect.Margin = new System.Windows.Forms.Padding(0);
            this.labelTrandChartDefect.Name = "labelTrandChartDefect";
            this.labelTrandChartDefect.Size = new System.Drawing.Size(631, 35);
            this.labelTrandChartDefect.TabIndex = 73;
            this.labelTrandChartDefect.Text = "Length Chart";
            this.labelTrandChartDefect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelTrandChartDefect.SizeChanged += new System.EventHandler(this.labelTrandChartDefect_SizeChanged);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.AliceBlue;
            this.tableLayoutPanel2.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel2.ColumnCount = 10;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel2.Controls.Add(this.labelAverage, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelStdDev, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelMin, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelMax, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelRange, 8, 0);
            this.tableLayoutPanel2.Controls.Add(this.average, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelAverageUnit, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelStdDevUnit, 3, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelMinUnit, 5, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelMaxUnit, 7, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelRangeUnit, 9, 1);
            this.tableLayoutPanel2.Controls.Add(this.stdDev, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.min, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.max, 6, 1);
            this.tableLayoutPanel2.Controls.Add(this.range, 8, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 310);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(625, 61);
            this.tableLayoutPanel2.TabIndex = 74;
            // 
            // labelAverage
            // 
            this.labelAverage.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.labelAverage, 2);
            this.labelAverage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAverage.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelAverage.Location = new System.Drawing.Point(1, 1);
            this.labelAverage.Margin = new System.Windows.Forms.Padding(0);
            this.labelAverage.Name = "labelAverage";
            this.labelAverage.Size = new System.Drawing.Size(123, 29);
            this.labelAverage.TabIndex = 49;
            this.labelAverage.Text = "Average";
            this.labelAverage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelStdDev
            // 
            this.labelStdDev.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.labelStdDev, 2);
            this.labelStdDev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStdDev.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelStdDev.Location = new System.Drawing.Point(125, 1);
            this.labelStdDev.Margin = new System.Windows.Forms.Padding(0);
            this.labelStdDev.Name = "labelStdDev";
            this.labelStdDev.Size = new System.Drawing.Size(123, 29);
            this.labelStdDev.TabIndex = 49;
            this.labelStdDev.Text = "StdDev";
            this.labelStdDev.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.labelMin, 2);
            this.labelMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMin.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelMin.Location = new System.Drawing.Point(249, 1);
            this.labelMin.Margin = new System.Windows.Forms.Padding(0);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(123, 29);
            this.labelMin.TabIndex = 49;
            this.labelMin.Text = "Min";
            this.labelMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.labelMax, 2);
            this.labelMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMax.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelMax.Location = new System.Drawing.Point(373, 1);
            this.labelMax.Margin = new System.Windows.Forms.Padding(0);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(123, 29);
            this.labelMax.TabIndex = 49;
            this.labelMax.Text = "Max";
            this.labelMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRange
            // 
            this.labelRange.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.labelRange, 2);
            this.labelRange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRange.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelRange.Location = new System.Drawing.Point(497, 1);
            this.labelRange.Margin = new System.Windows.Forms.Padding(0);
            this.labelRange.Name = "labelRange";
            this.labelRange.Size = new System.Drawing.Size(127, 29);
            this.labelRange.TabIndex = 49;
            this.labelRange.Text = "Range";
            this.labelRange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // average
            // 
            this.average.Dock = System.Windows.Forms.DockStyle.Fill;
            this.average.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.average.Location = new System.Drawing.Point(1, 31);
            this.average.Margin = new System.Windows.Forms.Padding(0);
            this.average.Name = "average";
            this.average.ReadOnly = true;
            this.average.Size = new System.Drawing.Size(89, 29);
            this.average.TabIndex = 51;
            this.average.Text = "0.000";
            this.average.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelAverageUnit
            // 
            this.labelAverageUnit.AutoSize = true;
            this.labelAverageUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAverageUnit.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelAverageUnit.Location = new System.Drawing.Point(91, 31);
            this.labelAverageUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelAverageUnit.Name = "labelAverageUnit";
            this.labelAverageUnit.Size = new System.Drawing.Size(33, 29);
            this.labelAverageUnit.TabIndex = 49;
            this.labelAverageUnit.Text = "[mm]";
            this.labelAverageUnit.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelStdDevUnit
            // 
            this.labelStdDevUnit.AutoSize = true;
            this.labelStdDevUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStdDevUnit.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelStdDevUnit.Location = new System.Drawing.Point(215, 31);
            this.labelStdDevUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelStdDevUnit.Name = "labelStdDevUnit";
            this.labelStdDevUnit.Size = new System.Drawing.Size(33, 29);
            this.labelStdDevUnit.TabIndex = 49;
            this.labelStdDevUnit.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelMinUnit
            // 
            this.labelMinUnit.AutoSize = true;
            this.labelMinUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMinUnit.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelMinUnit.Location = new System.Drawing.Point(339, 31);
            this.labelMinUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelMinUnit.Name = "labelMinUnit";
            this.labelMinUnit.Size = new System.Drawing.Size(33, 29);
            this.labelMinUnit.TabIndex = 49;
            this.labelMinUnit.Text = "[mm]";
            this.labelMinUnit.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelMaxUnit
            // 
            this.labelMaxUnit.AutoSize = true;
            this.labelMaxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaxUnit.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelMaxUnit.Location = new System.Drawing.Point(463, 31);
            this.labelMaxUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelMaxUnit.Name = "labelMaxUnit";
            this.labelMaxUnit.Size = new System.Drawing.Size(33, 29);
            this.labelMaxUnit.TabIndex = 49;
            this.labelMaxUnit.Text = "[mm]";
            this.labelMaxUnit.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelRangeUnit
            // 
            this.labelRangeUnit.AutoSize = true;
            this.labelRangeUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRangeUnit.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelRangeUnit.Location = new System.Drawing.Point(587, 31);
            this.labelRangeUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelRangeUnit.Name = "labelRangeUnit";
            this.labelRangeUnit.Size = new System.Drawing.Size(37, 29);
            this.labelRangeUnit.TabIndex = 49;
            this.labelRangeUnit.Text = "[mm]";
            this.labelRangeUnit.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // stdDev
            // 
            this.stdDev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stdDev.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.stdDev.Location = new System.Drawing.Point(125, 31);
            this.stdDev.Margin = new System.Windows.Forms.Padding(0);
            this.stdDev.Name = "stdDev";
            this.stdDev.ReadOnly = true;
            this.stdDev.Size = new System.Drawing.Size(89, 29);
            this.stdDev.TabIndex = 51;
            this.stdDev.Text = "0.000";
            this.stdDev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // min
            // 
            this.min.Dock = System.Windows.Forms.DockStyle.Fill;
            this.min.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.min.Location = new System.Drawing.Point(249, 31);
            this.min.Margin = new System.Windows.Forms.Padding(0);
            this.min.Name = "min";
            this.min.ReadOnly = true;
            this.min.Size = new System.Drawing.Size(89, 29);
            this.min.TabIndex = 51;
            this.min.Text = "0.00";
            this.min.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // max
            // 
            this.max.Dock = System.Windows.Forms.DockStyle.Fill;
            this.max.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.max.Location = new System.Drawing.Point(373, 31);
            this.max.Margin = new System.Windows.Forms.Padding(0);
            this.max.Name = "max";
            this.max.ReadOnly = true;
            this.max.Size = new System.Drawing.Size(89, 29);
            this.max.TabIndex = 51;
            this.max.Text = "0.00";
            this.max.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // range
            // 
            this.range.Dock = System.Windows.Forms.DockStyle.Fill;
            this.range.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.range.Location = new System.Drawing.Point(497, 31);
            this.range.Margin = new System.Windows.Forms.Padding(0);
            this.range.Name = "range";
            this.range.ReadOnly = true;
            this.range.Size = new System.Drawing.Size(89, 29);
            this.range.TabIndex = 51;
            this.range.Text = "0.00";
            this.range.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // LengthChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.Name = "LengthChart";
            this.Size = new System.Drawing.Size(631, 374);
            this.Load += new System.EventHandler(this.LengthChart_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartLength)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartLength;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelTrandChartDefect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labelAverage;
        private System.Windows.Forms.Label labelStdDev;
        private System.Windows.Forms.Label labelMin;
        private System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.Label labelRange;
        private System.Windows.Forms.TextBox average;
        private System.Windows.Forms.Label labelAverageUnit;
        private System.Windows.Forms.Label labelStdDevUnit;
        private System.Windows.Forms.Label labelMinUnit;
        private System.Windows.Forms.Label labelMaxUnit;
        private System.Windows.Forms.Label labelRangeUnit;
        private System.Windows.Forms.TextBox stdDev;
        private System.Windows.Forms.TextBox min;
        private System.Windows.Forms.TextBox max;
        private System.Windows.Forms.TextBox range;
    }
}
