namespace UniScanM.StillImage.UI
{
    partial class ReportPanel_M
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewResult = new System.Windows.Forms.DataGridView();
            this.columnInspNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnInspZone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnMargin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnBlot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDefect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelImage = new System.Windows.Forms.Panel();
            this.tabControl_Report = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chart_PatternLength = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvReport_PatternLength = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).BeginInit();
            this.tabControl_Report.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_PatternLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport_PatternLength)).BeginInit();
            this.SuspendLayout();
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
            this.columnDistance,
            this.columnMargin,
            this.columnBlot,
            this.columnDefect,
            this.columnResult});
            this.dataGridViewResult.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridViewResult.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewResult.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.dataGridViewResult.MultiSelect = false;
            this.dataGridViewResult.Name = "dataGridViewResult";
            this.dataGridViewResult.RowHeadersVisible = false;
            this.dataGridViewResult.RowTemplate.Height = 23;
            this.dataGridViewResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewResult.Size = new System.Drawing.Size(707, 327);
            this.dataGridViewResult.TabIndex = 1;
            this.dataGridViewResult.SelectionChanged += new System.EventHandler(this.dataGridViewResult_SelectionChanged);
            // 
            // columnInspNo
            // 
            this.columnInspNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnInspNo.HeaderText = "No";
            this.columnInspNo.Name = "columnInspNo";
            this.columnInspNo.ReadOnly = true;
            this.columnInspNo.Width = 57;
            // 
            // columnInspZone
            // 
            this.columnInspZone.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnInspZone.HeaderText = "Zone";
            this.columnInspZone.Name = "columnInspZone";
            this.columnInspZone.ReadOnly = true;
            this.columnInspZone.Width = 72;
            // 
            // columnDistance
            // 
            this.columnDistance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnDistance.HeaderText = "Distance [m]";
            this.columnDistance.Name = "columnDistance";
            this.columnDistance.ReadOnly = true;
            this.columnDistance.Width = 127;
            // 
            // columnMargin
            // 
            this.columnMargin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnMargin.HeaderText = "Margin [um]";
            this.columnMargin.Name = "columnMargin";
            this.columnMargin.ReadOnly = true;
            this.columnMargin.Width = 126;
            // 
            // columnBlot
            // 
            this.columnBlot.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnBlot.HeaderText = "Blot [um]";
            this.columnBlot.Name = "columnBlot";
            this.columnBlot.ReadOnly = true;
            this.columnBlot.Width = 103;
            // 
            // columnDefect
            // 
            this.columnDefect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnDefect.HeaderText = "Defect [um]";
            this.columnDefect.Name = "columnDefect";
            this.columnDefect.ReadOnly = true;
            this.columnDefect.Width = 122;
            // 
            // columnResult
            // 
            this.columnResult.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnResult.HeaderText = "Result";
            this.columnResult.Name = "columnResult";
            this.columnResult.ReadOnly = true;
            // 
            // panelImage
            // 
            this.panelImage.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(710, 3);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(326, 327);
            this.panelImage.TabIndex = 2;
            // 
            // tabControl_Report
            // 
            this.tabControl_Report.Controls.Add(this.tabPage1);
            this.tabControl_Report.Controls.Add(this.tabPage2);
            this.tabControl_Report.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Report.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Report.Name = "tabControl_Report";
            this.tabControl_Report.SelectedIndex = 0;
            this.tabControl_Report.Size = new System.Drawing.Size(1047, 367);
            this.tabControl_Report.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panelImage);
            this.tabPage1.Controls.Add(this.dataGridViewResult);
            this.tabPage1.Location = new System.Drawing.Point(4, 30);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1039, 333);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Inspection";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chart_PatternLength);
            this.tabPage2.Controls.Add(this.dgvReport_PatternLength);
            this.tabPage2.Location = new System.Drawing.Point(4, 30);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1039, 333);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Pattern Length";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chart_PatternLength
            // 
            this.chart_PatternLength.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AxisY.LabelStyle.Format = "{0:0.0}";
            chartArea1.Name = "ChartArea1";
            this.chart_PatternLength.ChartAreas.Add(chartArea1);
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "Legend1";
            legend1.Title = "Pattern Length";
            this.chart_PatternLength.Legends.Add(legend1);
            this.chart_PatternLength.Location = new System.Drawing.Point(665, 3);
            this.chart_PatternLength.Name = "chart_PatternLength";
            series1.BorderWidth = 2;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Zone 1";
            series2.BorderWidth = 2;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Zone 2";
            series3.BorderWidth = 2;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "Zone 3";
            series4.BorderWidth = 2;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.Legend = "Legend1";
            series4.Name = "Zone 4";
            series5.BorderWidth = 2;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Legend = "Legend1";
            series5.Name = "Zone 5";
            series6.BorderWidth = 2;
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Legend = "Legend1";
            series6.Name = "Zone 6";
            this.chart_PatternLength.Series.Add(series1);
            this.chart_PatternLength.Series.Add(series2);
            this.chart_PatternLength.Series.Add(series3);
            this.chart_PatternLength.Series.Add(series4);
            this.chart_PatternLength.Series.Add(series5);
            this.chart_PatternLength.Series.Add(series6);
            this.chart_PatternLength.Size = new System.Drawing.Size(368, 324);
            this.chart_PatternLength.TabIndex = 13;
            this.chart_PatternLength.Text = "chart_PatternLength";
            // 
            // dgvReport_PatternLength
            // 
            this.dgvReport_PatternLength.AllowUserToAddRows = false;
            this.dgvReport_PatternLength.AllowUserToDeleteRows = false;
            this.dgvReport_PatternLength.AllowUserToResizeColumns = false;
            this.dgvReport_PatternLength.AllowUserToResizeRows = false;
            this.dgvReport_PatternLength.CausesValidation = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReport_PatternLength.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReport_PatternLength.ColumnHeadersHeight = 40;
            this.dgvReport_PatternLength.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvReport_PatternLength.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.Orange;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReport_PatternLength.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvReport_PatternLength.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvReport_PatternLength.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvReport_PatternLength.EnableHeadersVisualStyles = false;
            this.dgvReport_PatternLength.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dgvReport_PatternLength.Location = new System.Drawing.Point(3, 3);
            this.dgvReport_PatternLength.Margin = new System.Windows.Forms.Padding(1);
            this.dgvReport_PatternLength.MultiSelect = false;
            this.dgvReport_PatternLength.Name = "dgvReport_PatternLength";
            this.dgvReport_PatternLength.ReadOnly = true;
            this.dgvReport_PatternLength.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dgvReport_PatternLength.RowHeadersWidth = 88;
            this.dgvReport_PatternLength.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle9.Format = "N1";
            dataGridViewCellStyle9.NullValue = null;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvReport_PatternLength.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvReport_PatternLength.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvReport_PatternLength.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgvReport_PatternLength.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvReport_PatternLength.RowTemplate.Height = 44;
            this.dgvReport_PatternLength.RowTemplate.ReadOnly = true;
            this.dgvReport_PatternLength.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReport_PatternLength.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvReport_PatternLength.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.ColumnHeaderSelect;
            this.dgvReport_PatternLength.ShowCellErrors = false;
            this.dgvReport_PatternLength.ShowCellToolTips = false;
            this.dgvReport_PatternLength.ShowEditingIcon = false;
            this.dgvReport_PatternLength.ShowRowErrors = false;
            this.dgvReport_PatternLength.Size = new System.Drawing.Size(661, 327);
            this.dgvReport_PatternLength.TabIndex = 12;
            this.dgvReport_PatternLength.TabStop = false;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.HeaderText = "Zone 1";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column2.HeaderText = "Zone 2";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column3.HeaderText = "Zone 3";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = null;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column4.HeaderText = "Zone 4";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = null;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column5.HeaderText = "Zone 5";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.NullValue = null;
            this.Column6.DefaultCellStyle = dataGridViewCellStyle7;
            this.Column6.HeaderText = "Zone 6";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ReportPanel_M
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl_Report);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "ReportPanel_M";
            this.Size = new System.Drawing.Size(1047, 367);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).EndInit();
            this.tabControl_Report.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart_PatternLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport_PatternLength)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridViewResult;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnInspNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnInspZone;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnMargin;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnBlot;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDefect;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnResult;
        private System.Windows.Forms.TabControl tabControl_Report;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvReport_PatternLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_PatternLength;
    }
}
