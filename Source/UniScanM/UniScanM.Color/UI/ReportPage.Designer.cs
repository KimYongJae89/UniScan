namespace UniScanM.ColorSens.UI
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataGridViewSearch = new System.Windows.Forms.DataGridView();
            this.columnStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Model = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewResult = new System.Windows.Forms.DataGridView();
            this.columnInspNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnInspZone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnMargin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnBlot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDefect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.buttonStartSearch = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboAutoManual = new System.Windows.Forms.ComboBox();
            this.labelAutoManual = new System.Windows.Forms.Label();
            this.buttonExplorer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chart_FullTime = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.resultView = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_FullTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewSearch
            // 
            this.dataGridViewSearch.AllowUserToAddRows = false;
            this.dataGridViewSearch.AllowUserToDeleteRows = false;
            this.dataGridViewSearch.AllowUserToOrderColumns = true;
            this.dataGridViewSearch.AllowUserToResizeRows = false;
            this.dataGridViewSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSearch.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnStart,
            this.Model,
            this.columnModel,
            this.columnLot});
            this.dataGridViewSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSearch.Location = new System.Drawing.Point(3, 126);
            this.dataGridViewSearch.MultiSelect = false;
            this.dataGridViewSearch.Name = "dataGridViewSearch";
            this.dataGridViewSearch.RowHeadersVisible = false;
            this.dataGridViewSearch.RowTemplate.Height = 23;
            this.dataGridViewSearch.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSearch.Size = new System.Drawing.Size(544, 171);
            this.dataGridViewSearch.TabIndex = 0;
            this.dataGridViewSearch.SelectionChanged += new System.EventHandler(this.dataGridViewSearch_SelectionChanged);
            // 
            // columnStart
            // 
            this.columnStart.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnStart.HeaderText = "Date";
            this.columnStart.Name = "columnStart";
            this.columnStart.ReadOnly = true;
            // 
            // Model
            // 
            this.Model.HeaderText = "Model";
            this.Model.Name = "Model";
            // 
            // columnModel
            // 
            this.columnModel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnModel.HeaderText = "Auto/Man";
            this.columnModel.Name = "columnModel";
            this.columnModel.ReadOnly = true;
            // 
            // columnLot
            // 
            this.columnLot.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnLot.HeaderText = "Lot";
            this.columnLot.Name = "columnLot";
            this.columnLot.ReadOnly = true;
            // 
            // dataGridViewResult
            // 
            this.dataGridViewResult.AllowUserToAddRows = false;
            this.dataGridViewResult.AllowUserToDeleteRows = false;
            this.dataGridViewResult.AllowUserToOrderColumns = true;
            this.dataGridViewResult.AllowUserToResizeRows = false;
            this.dataGridViewResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnInspNo,
            this.columnInspZone,
            this.columnMargin,
            this.columnBlot,
            this.columnDefect,
            this.columnResult});
            this.dataGridViewResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewResult.Location = new System.Drawing.Point(3, 303);
            this.dataGridViewResult.MultiSelect = false;
            this.dataGridViewResult.Name = "dataGridViewResult";
            this.dataGridViewResult.RowHeadersVisible = false;
            this.dataGridViewResult.RowTemplate.Height = 23;
            this.dataGridViewResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewResult.Size = new System.Drawing.Size(544, 480);
            this.dataGridViewResult.TabIndex = 1;
            this.dataGridViewResult.SelectionChanged += new System.EventHandler(this.dataGridViewResult_SelectionChanged);
            // 
            // columnInspNo
            // 
            this.columnInspNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnInspNo.HeaderText = "InspNo";
            this.columnInspNo.Name = "columnInspNo";
            this.columnInspNo.ReadOnly = true;
            this.columnInspNo.Width = 70;
            // 
            // columnInspZone
            // 
            this.columnInspZone.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnInspZone.HeaderText = "Brightness";
            this.columnInspZone.Name = "columnInspZone";
            this.columnInspZone.ReadOnly = true;
            this.columnInspZone.Width = 90;
            // 
            // columnMargin
            // 
            this.columnMargin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnMargin.HeaderText = "Judge";
            this.columnMargin.Name = "columnMargin";
            this.columnMargin.ReadOnly = true;
            this.columnMargin.Width = 64;
            // 
            // columnBlot
            // 
            this.columnBlot.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnBlot.HeaderText = "LSL";
            this.columnBlot.Name = "columnBlot";
            this.columnBlot.ReadOnly = true;
            this.columnBlot.Width = 52;
            // 
            // columnDefect
            // 
            this.columnDefect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnDefect.HeaderText = "USL";
            this.columnDefect.Name = "columnDefect";
            this.columnDefect.ReadOnly = true;
            this.columnDefect.Width = 53;
            // 
            // columnResult
            // 
            this.columnResult.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnResult.HeaderText = "Reference";
            this.columnResult.Name = "columnResult";
            this.columnResult.ReadOnly = true;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "";
            this.dateTimePicker1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dateTimePicker1.Location = new System.Drawing.Point(65, 11);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(202, 26);
            this.dateTimePicker1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(12, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "End";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dateTimePicker2.Location = new System.Drawing.Point(65, 48);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(202, 26);
            this.dateTimePicker2.TabIndex = 4;
            // 
            // buttonStartSearch
            // 
            this.buttonStartSearch.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonStartSearch.Location = new System.Drawing.Point(283, 11);
            this.buttonStartSearch.Name = "buttonStartSearch";
            this.buttonStartSearch.Size = new System.Drawing.Size(119, 29);
            this.buttonStartSearch.TabIndex = 6;
            this.buttonStartSearch.Text = "Search";
            this.buttonStartSearch.UseVisualStyleBackColor = true;
            this.buttonStartSearch.Click += new System.EventHandler(this.buttonStartSearch_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboAutoManual);
            this.panel1.Controls.Add(this.labelAutoManual);
            this.panel1.Controls.Add(this.buttonExplorer);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonStartSearch);
            this.panel1.Controls.Add(this.dateTimePicker1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dateTimePicker2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(544, 117);
            this.panel1.TabIndex = 7;
            // 
            // comboAutoManual
            // 
            this.comboAutoManual.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAutoManual.Font = new System.Drawing.Font("굴림", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.comboAutoManual.FormattingEnabled = true;
            this.comboAutoManual.Items.AddRange(new object[] {
            "All",
            "Auto",
            "Manual"});
            this.comboAutoManual.Location = new System.Drawing.Point(65, 80);
            this.comboAutoManual.Name = "comboAutoManual";
            this.comboAutoManual.Size = new System.Drawing.Size(202, 22);
            this.comboAutoManual.TabIndex = 66;
            // 
            // labelAutoManual
            // 
            this.labelAutoManual.AutoSize = true;
            this.labelAutoManual.Font = new System.Drawing.Font("굴림", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelAutoManual.Location = new System.Drawing.Point(10, 83);
            this.labelAutoManual.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAutoManual.Name = "labelAutoManual";
            this.labelAutoManual.Size = new System.Drawing.Size(48, 15);
            this.labelAutoManual.TabIndex = 65;
            this.labelAutoManual.Text = "Mode";
            this.labelAutoManual.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonExplorer
            // 
            this.buttonExplorer.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonExplorer.Location = new System.Drawing.Point(415, 11);
            this.buttonExplorer.Name = "buttonExplorer";
            this.buttonExplorer.Size = new System.Drawing.Size(112, 29);
            this.buttonExplorer.TabIndex = 7;
            this.buttonExplorer.Text = "Explorer";
            this.buttonExplorer.UseVisualStyleBackColor = true;
            this.buttonExplorer.Click += new System.EventHandler(this.buttonExplorer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Start";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 550F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.dataGridViewResult, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridViewSearch, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 123F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 177F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(998, 786);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.resultView);
            this.panel2.Controls.Add(this.chart_FullTime);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Enabled = false;
            this.panel2.Location = new System.Drawing.Point(553, 3);
            this.panel2.Name = "panel2";
            this.tableLayoutPanel1.SetRowSpan(this.panel2, 3);
            this.panel2.Size = new System.Drawing.Size(442, 780);
            this.panel2.TabIndex = 11;
            // 
            // chart_FullTime
            // 
            chartArea2.AxisX.LineColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea2.AxisX.MajorGrid.Enabled = false;
            chartArea2.AxisX.Title = "Time";
            chartArea2.AxisX.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea2.AxisY.IsLabelAutoFit = false;
            chartArea2.AxisY.LabelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            chartArea2.AxisY.LabelStyle.ForeColor = System.Drawing.Color.Blue;
            chartArea2.AxisY.LabelStyle.Format = "{0:0}";
            chartArea2.AxisY.LineColor = System.Drawing.SystemColors.MenuHighlight;
            chartArea2.AxisY.MajorGrid.LineColor = System.Drawing.SystemColors.ActiveCaption;
            chartArea2.AxisY.Title = "Brightness";
            chartArea2.AxisY.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea2.Name = "ChartArea1";
            this.chart_FullTime.ChartAreas.Add(chartArea2);
            this.chart_FullTime.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chart_FullTime.Location = new System.Drawing.Point(0, 426);
            this.chart_FullTime.Margin = new System.Windows.Forms.Padding(0);
            this.chart_FullTime.Name = "chart_FullTime";
            series2.BorderColor = System.Drawing.Color.CornflowerBlue;
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.LightSkyBlue;
            series2.MarkerBorderColor = System.Drawing.Color.Blue;
            series2.Name = "graphdata";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Time;
            this.chart_FullTime.Series.Add(series2);
            this.chart_FullTime.Size = new System.Drawing.Size(442, 354);
            this.chart_FullTime.TabIndex = 10;
            this.chart_FullTime.Text = "chart1";
            // 
            // resultView
            // 
            this.resultView.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.resultView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultView.Location = new System.Drawing.Point(0, 0);
            this.resultView.Margin = new System.Windows.Forms.Padding(0);
            this.resultView.Name = "resultView";
            this.resultView.Size = new System.Drawing.Size(442, 426);
            this.resultView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.resultView.TabIndex = 11;
            this.resultView.TabStop = false;
            // 
            // ReportPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ReportPage";
            this.Size = new System.Drawing.Size(998, 786);
            this.Load += new System.EventHandler(this.ReportPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_FullTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewSearch;
        private System.Windows.Forms.DataGridView dataGridViewResult;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Button buttonStartSearch;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonExplorer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_FullTime;
        private System.Windows.Forms.ComboBox comboAutoManual;
        private System.Windows.Forms.Label labelAutoManual;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnInspNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnInspZone;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnMargin;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnBlot;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDefect;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn Model;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLot;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox resultView;
    }
}
