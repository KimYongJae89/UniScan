namespace UniScanM.CGInspector.UI
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
            this.dataGridViewResult = new System.Windows.Forms.DataGridView();
            this.columnInspNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnInspZone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnMargin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnBlot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDefect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelImage = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.dataGridViewResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewResult.Location = new System.Drawing.Point(4, 6);
            this.dataGridViewResult.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.dataGridViewResult.MultiSelect = false;
            this.dataGridViewResult.Name = "dataGridViewResult";
            this.dataGridViewResult.RowHeadersVisible = false;
            this.dataGridViewResult.RowTemplate.Height = 23;
            this.dataGridViewResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewResult.Size = new System.Drawing.Size(673, 355);
            this.dataGridViewResult.TabIndex = 1;
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.51762F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.48238F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridViewResult, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelImage, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1107, 367);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // panelImage
            // 
            this.panelImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(684, 3);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(420, 361);
            this.panelImage.TabIndex = 2;
            // 
            // ReportPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "ReportPanel";
            this.Size = new System.Drawing.Size(1107, 367);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridViewResult;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnInspNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnInspZone;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnMargin;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnBlot;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDefect;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnResult;
    }
}
