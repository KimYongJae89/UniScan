namespace UniScanM.UI
{
    partial class ReportPage
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.lot = new System.Windows.Forms.TextBox();
            this.showNgOnly = new System.Windows.Forms.CheckBox();
            this.labelSearchTime = new Infragistics.Win.Misc.UltraLabel();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.labelLot = new System.Windows.Forms.Label();
            this.labelStartDate = new System.Windows.Forms.Label();
            this.labelEndDate = new System.Windows.Forms.Label();
            this.buttonOpenFolder = new System.Windows.Forms.Button();
            this.buttonOpenExel = new System.Windows.Forms.Button();
            this.lotNoList = new System.Windows.Forms.DataGridView();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableChart = new System.Windows.Forms.TableLayoutPanel();
            this.searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lotNoList)).BeginInit();
            this.leftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.tableChart.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelDetail
            // 
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetail.Location = new System.Drawing.Point(5, 6);
            this.panelDetail.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(573, 223);
            this.panelDetail.TabIndex = 8;
            // 
            // searchPanel
            // 
            this.searchPanel.Controls.Add(this.lot);
            this.searchPanel.Controls.Add(this.showNgOnly);
            this.searchPanel.Controls.Add(this.labelSearchTime);
            this.searchPanel.Controls.Add(this.endDate);
            this.searchPanel.Controls.Add(this.startDate);
            this.searchPanel.Controls.Add(this.labelLot);
            this.searchPanel.Controls.Add(this.labelStartDate);
            this.searchPanel.Controls.Add(this.labelEndDate);
            this.searchPanel.Controls.Add(this.buttonOpenFolder);
            this.searchPanel.Controls.Add(this.buttonOpenExel);
            this.searchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchPanel.Location = new System.Drawing.Point(0, 0);
            this.searchPanel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(645, 136);
            this.searchPanel.TabIndex = 21;
            // 
            // lot
            // 
            this.lot.Location = new System.Drawing.Point(288, 53);
            this.lot.Name = "lot";
            this.lot.Size = new System.Drawing.Size(157, 29);
            this.lot.TabIndex = 56;
            this.lot.TextChanged += new System.EventHandler(this.lot_TextChanged);
            // 
            // showNgOnly
            // 
            this.showNgOnly.AutoSize = true;
            this.showNgOnly.Location = new System.Drawing.Point(288, 99);
            this.showNgOnly.Name = "showNgOnly";
            this.showNgOnly.Size = new System.Drawing.Size(137, 25);
            this.showNgOnly.TabIndex = 55;
            this.showNgOnly.Text = "Show NG Only";
            this.showNgOnly.UseVisualStyleBackColor = true;
            this.showNgOnly.CheckedChanged += new System.EventHandler(this.showNgOnly_CheckedChanged);
            // 
            // labelSearchTime
            // 
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(240)))));
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.Name = "Malgun Gothic";
            appearance1.FontData.SizeInPoints = 14F;
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.labelSearchTime.Appearance = appearance1;
            this.labelSearchTime.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.labelSearchTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSearchTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelSearchTime.Location = new System.Drawing.Point(0, 0);
            this.labelSearchTime.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.labelSearchTime.Name = "labelSearchTime";
            this.labelSearchTime.Size = new System.Drawing.Size(645, 45);
            this.labelSearchTime.TabIndex = 52;
            this.labelSearchTime.Text = "Search Date";
            // 
            // endDate
            // 
            this.endDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDate.Location = new System.Drawing.Point(66, 96);
            this.endDate.Margin = new System.Windows.Forms.Padding(8, 17, 8, 17);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(153, 29);
            this.endDate.TabIndex = 13;
            this.endDate.ValueChanged += new System.EventHandler(this.Date_ValueChanged);
            // 
            // startDate
            // 
            this.startDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDate.Location = new System.Drawing.Point(66, 52);
            this.startDate.Margin = new System.Windows.Forms.Padding(8, 17, 8, 17);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(153, 29);
            this.startDate.TabIndex = 12;
            this.startDate.ValueChanged += new System.EventHandler(this.Date_ValueChanged);
            // 
            // labelLot
            // 
            this.labelLot.Location = new System.Drawing.Point(239, 56);
            this.labelLot.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelLot.Name = "labelLot";
            this.labelLot.Size = new System.Drawing.Size(45, 21);
            this.labelLot.TabIndex = 18;
            this.labelLot.Text = "Lot";
            this.labelLot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelStartDate
            // 
            this.labelStartDate.Location = new System.Drawing.Point(8, 56);
            this.labelStartDate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelStartDate.Name = "labelStartDate";
            this.labelStartDate.Size = new System.Drawing.Size(45, 21);
            this.labelStartDate.TabIndex = 18;
            this.labelStartDate.Text = "Start";
            this.labelStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelEndDate
            // 
            this.labelEndDate.Location = new System.Drawing.Point(8, 100);
            this.labelEndDate.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelEndDate.Name = "labelEndDate";
            this.labelEndDate.Size = new System.Drawing.Size(45, 21);
            this.labelEndDate.TabIndex = 54;
            this.labelEndDate.Text = "End";
            this.labelEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonOpenFolder
            // 
            this.buttonOpenFolder.Location = new System.Drawing.Point(488, 94);
            this.buttonOpenFolder.Margin = new System.Windows.Forms.Padding(8, 17, 8, 17);
            this.buttonOpenFolder.Name = "buttonOpenFolder";
            this.buttonOpenFolder.Size = new System.Drawing.Size(146, 32);
            this.buttonOpenFolder.TabIndex = 14;
            this.buttonOpenFolder.Text = "Open Folder";
            this.buttonOpenFolder.UseVisualStyleBackColor = true;
            this.buttonOpenFolder.Click += new System.EventHandler(this.buttonOpenFolder_Click);
            // 
            // buttonOpenExel
            // 
            this.buttonOpenExel.Location = new System.Drawing.Point(488, 50);
            this.buttonOpenExel.Margin = new System.Windows.Forms.Padding(8, 17, 8, 17);
            this.buttonOpenExel.Name = "buttonOpenExel";
            this.buttonOpenExel.Size = new System.Drawing.Size(146, 32);
            this.buttonOpenExel.TabIndex = 14;
            this.buttonOpenExel.Text = "Open Excel";
            this.buttonOpenExel.UseVisualStyleBackColor = true;
            this.buttonOpenExel.Click += new System.EventHandler(this.buttonOpenExel_Click);
            // 
            // lotNoList
            // 
            this.lotNoList.AllowUserToAddRows = false;
            this.lotNoList.AllowUserToDeleteRows = false;
            this.lotNoList.AllowUserToResizeRows = false;
            this.lotNoList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lotNoList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lotNoList.Location = new System.Drawing.Point(0, 136);
            this.lotNoList.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.lotNoList.MultiSelect = false;
            this.lotNoList.Name = "lotNoList";
            this.lotNoList.ReadOnly = true;
            this.lotNoList.RowHeadersVisible = false;
            this.lotNoList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.lotNoList.RowTemplate.Height = 23;
            this.lotNoList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.lotNoList.Size = new System.Drawing.Size(645, 334);
            this.lotNoList.TabIndex = 21;
            this.lotNoList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.lotNoList_CellDoubleClick);
            this.lotNoList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lotNoList_KeyDown);
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.lotNoList);
            this.leftPanel.Controls.Add(this.searchPanel);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.leftPanel.Location = new System.Drawing.Point(0, 0);
            this.leftPanel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(645, 470);
            this.leftPanel.TabIndex = 24;
            // 
            // chart1
            // 
            chartArea1.AxisX.Title = "Length";
            chartArea1.AxisY.Title = "Value";
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(3, 238);
            this.chart1.Name = "chart1";
            series1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDot;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.LegendText = "LL";
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.LegendText = "LL2";
            series2.Name = "Series2";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(577, 229);
            this.chart1.TabIndex = 25;
            this.chart1.Text = "chart1";
            // 
            // tableChart
            // 
            this.tableChart.ColumnCount = 1;
            this.tableChart.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableChart.Controls.Add(this.panelDetail, 0, 0);
            this.tableChart.Controls.Add(this.chart1, 0, 1);
            this.tableChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableChart.Location = new System.Drawing.Point(645, 0);
            this.tableChart.Name = "tableChart";
            this.tableChart.RowCount = 2;
            this.tableChart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableChart.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableChart.Size = new System.Drawing.Size(583, 470);
            this.tableChart.TabIndex = 26;
            // 
            // ReportPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableChart);
            this.Controls.Add(this.leftPanel);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "ReportPage";
            this.Size = new System.Drawing.Size(1228, 470);
            this.Load += new System.EventHandler(this.ReportPanel_Load);
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lotNoList)).EndInit();
            this.leftPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.tableChart.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelDetail;
        private System.Windows.Forms.Panel searchPanel;
        private Infragistics.Win.Misc.UltraLabel labelSearchTime;
        private System.Windows.Forms.DateTimePicker endDate;
        private System.Windows.Forms.DateTimePicker startDate;
        private System.Windows.Forms.Label labelStartDate;
        private System.Windows.Forms.Label labelEndDate;
        private System.Windows.Forms.Button buttonOpenExel;
        private System.Windows.Forms.DataGridView lotNoList;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.TableLayoutPanel tableChart;
        private System.Windows.Forms.CheckBox showNgOnly;
        private System.Windows.Forms.Button buttonOpenFolder;
        private System.Windows.Forms.TextBox lot;
        private System.Windows.Forms.Label labelLot;
    }
}
