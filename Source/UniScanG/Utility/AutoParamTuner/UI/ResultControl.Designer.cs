namespace AutoParamTuner.UI
{
    partial class ResultControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBoxValue = new System.Windows.Forms.GroupBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.groupBoxTR = new System.Windows.Forms.GroupBox();
            this.listViewTrue = new System.Windows.Forms.ListView();
            this.groupBoxNC = new System.Windows.Forms.GroupBox();
            this.listViewUnknown = new System.Windows.Forms.ListView();
            this.groupBoxFR = new System.Windows.Forms.GroupBox();
            this.listViewFalse = new System.Windows.Forms.ListView();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxValue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.groupBoxTR.SuspendLayout();
            this.groupBoxNC.SuspendLayout();
            this.groupBoxFR.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.groupBoxValue, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxTR, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxNC, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBoxFR, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.chart1, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(868, 536);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBoxValue
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBoxValue, 2);
            this.groupBoxValue.Controls.Add(this.dataGridView);
            this.groupBoxValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxValue.Location = new System.Drawing.Point(3, 3);
            this.groupBoxValue.Name = "groupBoxValue";
            this.groupBoxValue.Size = new System.Drawing.Size(572, 315);
            this.groupBoxValue.TabIndex = 1;
            this.groupBoxValue.TabStop = false;
            this.groupBoxValue.Text = "검사값";
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(3, 17);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(566, 295);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.SelectionChanged += new System.EventHandler(this.dataGridView_SelectionChanged);
            // 
            // groupBoxTR
            // 
            this.groupBoxTR.Controls.Add(this.listViewTrue);
            this.groupBoxTR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxTR.Location = new System.Drawing.Point(3, 324);
            this.groupBoxTR.Name = "groupBoxTR";
            this.groupBoxTR.Size = new System.Drawing.Size(283, 209);
            this.groupBoxTR.TabIndex = 1;
            this.groupBoxTR.TabStop = false;
            this.groupBoxTR.Text = "True NG";
            // 
            // listViewTrue
            // 
            this.listViewTrue.AllowDrop = true;
            this.listViewTrue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewTrue.HideSelection = false;
            this.listViewTrue.Location = new System.Drawing.Point(3, 17);
            this.listViewTrue.Name = "listViewTrue";
            this.listViewTrue.Size = new System.Drawing.Size(277, 189);
            this.listViewTrue.TabIndex = 0;
            this.listViewTrue.UseCompatibleStateImageBehavior = false;
            this.listViewTrue.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewNone_ItemDrag);
            this.listViewTrue.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listViewTrue.SizeChanged += new System.EventHandler(this.listView_SizeChanged);
            this.listViewTrue.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewNone_DragDrop);
            this.listViewTrue.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewNone_DragEnter);
            this.listViewTrue.DragLeave += new System.EventHandler(this.listViewNone_DragLeave);
            this.listViewTrue.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.listView_PreviewKeyDown);
            // 
            // groupBoxNC
            // 
            this.groupBoxNC.Controls.Add(this.listViewUnknown);
            this.groupBoxNC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxNC.Location = new System.Drawing.Point(292, 324);
            this.groupBoxNC.Name = "groupBoxNC";
            this.groupBoxNC.Size = new System.Drawing.Size(283, 209);
            this.groupBoxNC.TabIndex = 1;
            this.groupBoxNC.TabStop = false;
            this.groupBoxNC.Text = "Unknown";
            // 
            // listViewUnknown
            // 
            this.listViewUnknown.AllowDrop = true;
            this.listViewUnknown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewUnknown.HideSelection = false;
            this.listViewUnknown.Location = new System.Drawing.Point(3, 17);
            this.listViewUnknown.Name = "listViewUnknown";
            this.listViewUnknown.Size = new System.Drawing.Size(277, 189);
            this.listViewUnknown.TabIndex = 0;
            this.listViewUnknown.UseCompatibleStateImageBehavior = false;
            this.listViewUnknown.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewNone_ItemDrag);
            this.listViewUnknown.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listViewUnknown.SizeChanged += new System.EventHandler(this.listView_SizeChanged);
            this.listViewUnknown.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewNone_DragDrop);
            this.listViewUnknown.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewNone_DragEnter);
            this.listViewUnknown.DragLeave += new System.EventHandler(this.listViewNone_DragLeave);
            this.listViewUnknown.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.listView_PreviewKeyDown);
            // 
            // groupBoxFR
            // 
            this.groupBoxFR.Controls.Add(this.listViewFalse);
            this.groupBoxFR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxFR.Location = new System.Drawing.Point(581, 324);
            this.groupBoxFR.Name = "groupBoxFR";
            this.groupBoxFR.Size = new System.Drawing.Size(284, 209);
            this.groupBoxFR.TabIndex = 1;
            this.groupBoxFR.TabStop = false;
            this.groupBoxFR.Text = "False NG";
            // 
            // listViewFalse
            // 
            this.listViewFalse.AllowDrop = true;
            this.listViewFalse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewFalse.HideSelection = false;
            this.listViewFalse.Location = new System.Drawing.Point(3, 17);
            this.listViewFalse.Name = "listViewFalse";
            this.listViewFalse.Size = new System.Drawing.Size(278, 189);
            this.listViewFalse.TabIndex = 0;
            this.listViewFalse.UseCompatibleStateImageBehavior = false;
            this.listViewFalse.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listViewNone_ItemDrag);
            this.listViewFalse.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listViewFalse.SizeChanged += new System.EventHandler(this.listView_SizeChanged);
            this.listViewFalse.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewNone_DragDrop);
            this.listViewFalse.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewNone_DragEnter);
            this.listViewFalse.DragLeave += new System.EventHandler(this.listViewNone_DragLeave);
            this.listViewFalse.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.listView_PreviewKeyDown);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(578, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            series1.Legend = "Legend1";
            series1.Name = "TrueNg";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            series2.Legend = "Legend1";
            series2.Name = "FalseNg";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.StackedColumn;
            series3.Legend = "Legend1";
            series3.Name = "Unknown";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            this.chart1.Size = new System.Drawing.Size(290, 321);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            // 
            // ResultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ResultControl";
            this.Size = new System.Drawing.Size(868, 536);
            this.Load += new System.EventHandler(this.ResultControl_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBoxValue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.groupBoxTR.ResumeLayout(false);
            this.groupBoxNC.ResumeLayout(false);
            this.groupBoxFR.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBoxValue;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.GroupBox groupBoxTR;
        private System.Windows.Forms.ListView listViewTrue;
        private System.Windows.Forms.GroupBox groupBoxNC;
        private System.Windows.Forms.ListView listViewUnknown;
        private System.Windows.Forms.GroupBox groupBoxFR;
        private System.Windows.Forms.ListView listViewFalse;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}
