namespace UniScanM.ColorSens.UI
{
    partial class ReportPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.resultView = new System.Windows.Forms.PictureBox();
            this.dataGridViewResult = new System.Windows.Forms.DataGridView();
            this.columnIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnBrightness = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnJudgement = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLsl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnUsl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnReference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55.86207F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 44.13793F));
            this.tableLayoutPanel1.Controls.Add(this.resultView, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridViewResult, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1015, 596);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // resultView
            // 
            this.resultView.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.resultView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultView.Location = new System.Drawing.Point(566, 0);
            this.resultView.Margin = new System.Windows.Forms.Padding(0);
            this.resultView.Name = "resultView";
            this.resultView.Size = new System.Drawing.Size(449, 596);
            this.resultView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.resultView.TabIndex = 11;
            this.resultView.TabStop = false;
            // 
            // dataGridViewResult
            // 
            this.dataGridViewResult.AllowUserToAddRows = false;
            this.dataGridViewResult.AllowUserToDeleteRows = false;
            this.dataGridViewResult.AllowUserToOrderColumns = true;
            this.dataGridViewResult.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnIndex,
            this.columnDistance,
            this.columnBrightness,
            this.columnJudgement,
            this.columnLsl,
            this.columnUsl,
            this.columnReference});
            this.dataGridViewResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewResult.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewResult.MultiSelect = false;
            this.dataGridViewResult.Name = "dataGridViewResult";
            this.dataGridViewResult.RowHeadersVisible = false;
            this.dataGridViewResult.RowTemplate.Height = 23;
            this.dataGridViewResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewResult.Size = new System.Drawing.Size(560, 590);
            this.dataGridViewResult.TabIndex = 1;
            this.dataGridViewResult.SelectionChanged += new System.EventHandler(this.dataGridViewResult_SelectionChanged);
            // 
            // columnIndex
            // 
            this.columnIndex.HeaderText = "Index";
            this.columnIndex.Name = "columnIndex";
            this.columnIndex.ReadOnly = true;
            this.columnIndex.Width = 61;
            // 
            // columnDistance
            // 
            this.columnDistance.HeaderText = "Distance [m]";
            this.columnDistance.Name = "columnDistance";
            this.columnDistance.ReadOnly = true;
            this.columnDistance.Width = 106;
            // 
            // columnBrightness
            // 
            this.columnBrightness.HeaderText = "Brightness [lv]";
            this.columnBrightness.Name = "columnBrightness";
            this.columnBrightness.ReadOnly = true;
            this.columnBrightness.Width = 115;
            // 
            // columnJudgement
            // 
            this.columnJudgement.HeaderText = "Judge";
            this.columnJudgement.Name = "columnJudgement";
            this.columnJudgement.ReadOnly = true;
            this.columnJudgement.Width = 64;
            // 
            // columnLsl
            // 
            this.columnLsl.HeaderText = "LSL [lv]";
            this.columnLsl.Name = "columnLsl";
            this.columnLsl.ReadOnly = true;
            this.columnLsl.Width = 77;
            // 
            // columnUsl
            // 
            this.columnUsl.HeaderText = "USL [lv]";
            this.columnUsl.Name = "columnUsl";
            this.columnUsl.ReadOnly = true;
            this.columnUsl.Width = 78;
            // 
            // columnReference
            // 
            this.columnReference.HeaderText = "Reference [lv]";
            this.columnReference.Name = "columnReference";
            this.columnReference.ReadOnly = true;
            this.columnReference.Width = 112;
            // 
            // ReportPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ReportPanel";
            this.Size = new System.Drawing.Size(1015, 596);
            this.Load += new System.EventHandler(this.ReportPanel_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resultView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridViewResult;
        private System.Windows.Forms.PictureBox resultView;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnBrightness;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnJudgement;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLsl;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnUsl;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnReference;
    }
}
