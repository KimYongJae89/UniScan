namespace UniScanG.Gravure.UI.Inspect
{
    partial class InspectObserverPanel
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
            this.name = new System.Windows.Forms.Label();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label12 = new System.Windows.Forms.Label();
            this.term = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.time = new System.Windows.Forms.Label();
            this.listBox = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.count = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // name
            // 
            this.name.AutoSize = true;
            this.name.Location = new System.Drawing.Point(5, 0);
            this.name.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(85, 25);
            this.name.TabIndex = 31;
            this.name.Text = "Grabbed";
            this.name.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // chart
            // 
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            this.chart.Location = new System.Drawing.Point(5, 342);
            this.chart.Name = "chart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            this.chart.Series.Add(series1);
            this.chart.Size = new System.Drawing.Size(312, 165);
            this.chart.TabIndex = 30;
            this.chart.Text = "chart1";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(0, 314);
            this.label12.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 25);
            this.label12.TabIndex = 29;
            this.label12.Text = "Term";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // term
            // 
            this.term.AutoSize = true;
            this.term.Location = new System.Drawing.Point(252, 311);
            this.term.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.term.Name = "term";
            this.term.Size = new System.Drawing.Size(66, 25);
            this.term.TabIndex = 28;
            this.term.Text = "00.000";
            this.term.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(0, 286);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 25);
            this.label8.TabIndex = 27;
            this.label8.Text = "Last Time";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // time
            // 
            this.time.AutoSize = true;
            this.time.Location = new System.Drawing.Point(203, 286);
            this.time.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(114, 25);
            this.time.TabIndex = 26;
            this.time.Text = "00:00:00.000";
            this.time.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // listBox
            // 
            this.listBox.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox.FormattingEnabled = true;
            this.listBox.ItemHeight = 25;
            this.listBox.Location = new System.Drawing.Point(5, 75);
            this.listBox.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(312, 204);
            this.listBox.TabIndex = 25;
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuSave});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(100, 26);
            // 
            // toolStripMenuSave
            // 
            this.toolStripMenuSave.Name = "toolStripMenuSave";
            this.toolStripMenuSave.Size = new System.Drawing.Size(99, 22);
            this.toolStripMenuSave.Text = "Save";
            this.toolStripMenuSave.Click += new System.EventHandler(this.toolStripMenuSave_Click);
            // 
            // count
            // 
            this.count.Location = new System.Drawing.Point(232, 0);
            this.count.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.count.Name = "count";
            this.count.Size = new System.Drawing.Size(85, 25);
            this.count.TabIndex = 32;
            this.count.Text = "00000";
            this.count.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(5, 32);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(312, 33);
            this.comboBox1.TabIndex = 33;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // InspectObserverPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.count);
            this.Controls.Add(this.name);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.term);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.time);
            this.Controls.Add(this.listBox);
            this.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "InspectObserverPanel";
            this.Size = new System.Drawing.Size(323, 515);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label name;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label term;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label time;
        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.Label count;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuSave;
    }
}
