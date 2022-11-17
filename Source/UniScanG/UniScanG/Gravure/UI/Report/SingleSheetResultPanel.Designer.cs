namespace UniScanG.Gravure.UI.Report
{
    partial class SingleSheetResultPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelR = new System.Windows.Forms.TableLayoutPanel();
            this.labelImage = new System.Windows.Forms.Label();
            this.imagePanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanelL = new System.Windows.Forms.TableLayoutPanel();
            this.defectList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelDefectImage = new System.Windows.Forms.Label();
            this.labelDefectList = new System.Windows.Forms.Label();
            this.tableDefectSizeW = new System.Windows.Forms.TableLayoutPanel();
            this.labelDefectSizeW = new System.Windows.Forms.Label();
            this.labelDefectSizeH = new System.Windows.Forms.Label();
            this.defectSizeW = new System.Windows.Forms.Label();
            this.defectSizeH = new System.Windows.Forms.Label();
            this.tableDefectSizeH = new System.Windows.Forms.TableLayoutPanel();
            this.defectListSelectAll = new System.Windows.Forms.Button();
            this.defectImage = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel.SuspendLayout();
            this.tableLayoutPanelR.SuspendLayout();
            this.tableLayoutPanelL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defectList)).BeginInit();
            this.tableDefectSizeW.SuspendLayout();
            this.tableDefectSizeH.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defectImage)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanelR, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanelL, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1030, 571);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // tableLayoutPanelR
            // 
            this.tableLayoutPanelR.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanelR.ColumnCount = 1;
            this.tableLayoutPanelR.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelR.Controls.Add(this.labelImage, 0, 0);
            this.tableLayoutPanelR.Controls.Add(this.imagePanel, 0, 1);
            this.tableLayoutPanelR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelR.Location = new System.Drawing.Point(300, 0);
            this.tableLayoutPanelR.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelR.Name = "tableLayoutPanelR";
            this.tableLayoutPanelR.RowCount = 2;
            this.tableLayoutPanelR.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelR.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelR.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelR.Size = new System.Drawing.Size(730, 571);
            this.tableLayoutPanelR.TabIndex = 90;
            // 
            // labelImage
            // 
            this.labelImage.AutoSize = true;
            this.labelImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImage.Font = new System.Drawing.Font("Malgun Gothic", 16F, System.Drawing.FontStyle.Bold);
            this.labelImage.Location = new System.Drawing.Point(2, 2);
            this.labelImage.Margin = new System.Windows.Forms.Padding(0);
            this.labelImage.Name = "labelImage";
            this.labelImage.Size = new System.Drawing.Size(726, 35);
            this.labelImage.TabIndex = 53;
            this.labelImage.Text = "Image";
            this.labelImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imagePanel
            // 
            this.imagePanel.BackColor = System.Drawing.SystemColors.Control;
            this.imagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagePanel.Location = new System.Drawing.Point(2, 39);
            this.imagePanel.Margin = new System.Windows.Forms.Padding(0);
            this.imagePanel.Name = "imagePanel";
            this.imagePanel.Size = new System.Drawing.Size(726, 530);
            this.imagePanel.TabIndex = 0;
            // 
            // tableLayoutPanelL
            // 
            this.tableLayoutPanelL.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanelL.ColumnCount = 2;
            this.tableLayoutPanelL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelL.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelL.Controls.Add(this.tableDefectSizeH, 1, 5);
            this.tableLayoutPanelL.Controls.Add(this.defectList, 0, 2);
            this.tableLayoutPanelL.Controls.Add(this.labelDefectImage, 0, 3);
            this.tableLayoutPanelL.Controls.Add(this.labelDefectList, 0, 0);
            this.tableLayoutPanelL.Controls.Add(this.defectImage, 0, 4);
            this.tableLayoutPanelL.Controls.Add(this.tableDefectSizeW, 0, 5);
            this.tableLayoutPanelL.Controls.Add(this.defectListSelectAll, 0, 1);
            this.tableLayoutPanelL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelL.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelL.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelL.Name = "tableLayoutPanelL";
            this.tableLayoutPanelL.RowCount = 6;
            this.tableLayoutPanelL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanelL.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanelL.Size = new System.Drawing.Size(300, 571);
            this.tableLayoutPanelL.TabIndex = 88;
            // 
            // defectList
            // 
            this.defectList.AllowUserToAddRows = false;
            this.defectList.AllowUserToDeleteRows = false;
            this.defectList.AllowUserToResizeColumns = false;
            this.defectList.AllowUserToResizeRows = false;
            this.defectList.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.defectList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.defectList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.defectList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.tableLayoutPanelL.SetColumnSpan(this.defectList, 2);
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.defectList.DefaultCellStyle = dataGridViewCellStyle6;
            this.defectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.defectList.Location = new System.Drawing.Point(2, 71);
            this.defectList.Margin = new System.Windows.Forms.Padding(0);
            this.defectList.Name = "defectList";
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.defectList.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.defectList.RowHeadersVisible = false;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.defectList.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.defectList.RowTemplate.Height = 23;
            this.defectList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.defectList.Size = new System.Drawing.Size(296, 222);
            this.defectList.TabIndex = 66;
            this.defectList.SelectionChanged += new System.EventHandler(this.defectList_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.HeaderText = "Cam";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 67;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn2.FillWeight = 65.9137F;
            this.dataGridViewTextBoxColumn2.HeaderText = "No.";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 61;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.FillWeight = 139.0863F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Type";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // labelDefectImage
            // 
            this.labelDefectImage.AutoSize = true;
            this.labelDefectImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.tableLayoutPanelL.SetColumnSpan(this.labelDefectImage, 2);
            this.labelDefectImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefectImage.Font = new System.Drawing.Font("Malgun Gothic", 16F, System.Drawing.FontStyle.Bold);
            this.labelDefectImage.Location = new System.Drawing.Point(2, 295);
            this.labelDefectImage.Margin = new System.Windows.Forms.Padding(0);
            this.labelDefectImage.Name = "labelDefectImage";
            this.labelDefectImage.Size = new System.Drawing.Size(296, 35);
            this.labelDefectImage.TabIndex = 64;
            this.labelDefectImage.Text = "Defect Image";
            this.labelDefectImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDefectList
            // 
            this.labelDefectList.AutoSize = true;
            this.labelDefectList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.tableLayoutPanelL.SetColumnSpan(this.labelDefectList, 2);
            this.labelDefectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefectList.Font = new System.Drawing.Font("Malgun Gothic", 16F, System.Drawing.FontStyle.Bold);
            this.labelDefectList.Location = new System.Drawing.Point(2, 2);
            this.labelDefectList.Margin = new System.Windows.Forms.Padding(0);
            this.labelDefectList.Name = "labelDefectList";
            this.labelDefectList.Size = new System.Drawing.Size(296, 35);
            this.labelDefectList.TabIndex = 0;
            this.labelDefectList.Text = "Defect List";
            this.labelDefectList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableDefectSizeW
            // 
            this.tableDefectSizeW.ColumnCount = 2;
            this.tableDefectSizeW.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableDefectSizeW.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableDefectSizeW.Controls.Add(this.labelDefectSizeW, 0, 0);
            this.tableDefectSizeW.Controls.Add(this.defectSizeW, 1, 0);
            this.tableDefectSizeW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableDefectSizeW.Location = new System.Drawing.Point(2, 534);
            this.tableDefectSizeW.Margin = new System.Windows.Forms.Padding(0);
            this.tableDefectSizeW.Name = "tableDefectSizeW";
            this.tableDefectSizeW.RowCount = 1;
            this.tableDefectSizeW.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDefectSizeW.Size = new System.Drawing.Size(147, 35);
            this.tableDefectSizeW.TabIndex = 67;
            // 
            // labelDefectSizeW
            // 
            this.labelDefectSizeW.AutoSize = true;
            this.labelDefectSizeW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefectSizeW.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.labelDefectSizeW.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelDefectSizeW.Location = new System.Drawing.Point(3, 0);
            this.labelDefectSizeW.Name = "labelDefectSizeW";
            this.labelDefectSizeW.Size = new System.Drawing.Size(38, 35);
            this.labelDefectSizeW.TabIndex = 68;
            this.labelDefectSizeW.Text = "W";
            this.labelDefectSizeW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDefectSizeH
            // 
            this.labelDefectSizeH.AutoSize = true;
            this.labelDefectSizeH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefectSizeH.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.labelDefectSizeH.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelDefectSizeH.Location = new System.Drawing.Point(3, 0);
            this.labelDefectSizeH.Name = "labelDefectSizeH";
            this.labelDefectSizeH.Size = new System.Drawing.Size(38, 35);
            this.labelDefectSizeH.TabIndex = 68;
            this.labelDefectSizeH.Text = "H";
            this.labelDefectSizeH.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // defectSizeW
            // 
            this.defectSizeW.AutoSize = true;
            this.defectSizeW.BackColor = System.Drawing.Color.White;
            this.defectSizeW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectSizeW.Location = new System.Drawing.Point(44, 0);
            this.defectSizeW.Margin = new System.Windows.Forms.Padding(0);
            this.defectSizeW.Name = "defectSizeW";
            this.defectSizeW.Size = new System.Drawing.Size(103, 35);
            this.defectSizeW.TabIndex = 6;
            this.defectSizeW.Text = "0";
            this.defectSizeW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // defectSizeH
            // 
            this.defectSizeH.AutoSize = true;
            this.defectSizeH.BackColor = System.Drawing.Color.White;
            this.defectSizeH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectSizeH.Location = new System.Drawing.Point(44, 0);
            this.defectSizeH.Margin = new System.Windows.Forms.Padding(0);
            this.defectSizeH.Name = "defectSizeH";
            this.defectSizeH.Size = new System.Drawing.Size(103, 35);
            this.defectSizeH.TabIndex = 6;
            this.defectSizeH.Text = "0";
            this.defectSizeH.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableDefectSizeH
            // 
            this.tableDefectSizeH.ColumnCount = 2;
            this.tableDefectSizeH.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableDefectSizeH.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableDefectSizeH.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableDefectSizeH.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableDefectSizeH.Controls.Add(this.labelDefectSizeH, 0, 0);
            this.tableDefectSizeH.Controls.Add(this.defectSizeH, 1, 0);
            this.tableDefectSizeH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableDefectSizeH.Location = new System.Drawing.Point(151, 534);
            this.tableDefectSizeH.Margin = new System.Windows.Forms.Padding(0);
            this.tableDefectSizeH.Name = "tableDefectSizeH";
            this.tableDefectSizeH.RowCount = 1;
            this.tableDefectSizeH.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableDefectSizeH.Size = new System.Drawing.Size(147, 35);
            this.tableDefectSizeH.TabIndex = 68;
            // 
            // defectListSelectAll
            // 
            this.tableLayoutPanelL.SetColumnSpan(this.defectListSelectAll, 2);
            this.defectListSelectAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectListSelectAll.Location = new System.Drawing.Point(2, 39);
            this.defectListSelectAll.Margin = new System.Windows.Forms.Padding(0);
            this.defectListSelectAll.Name = "defectListSelectAll";
            this.defectListSelectAll.Size = new System.Drawing.Size(296, 30);
            this.defectListSelectAll.TabIndex = 69;
            this.defectListSelectAll.Text = "Select All";
            this.defectListSelectAll.UseVisualStyleBackColor = true;
            this.defectListSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // defectImage
            // 
            this.defectImage.BackColor = System.Drawing.SystemColors.Control;
            this.defectImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanelL.SetColumnSpan(this.defectImage, 2);
            this.defectImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectImage.Location = new System.Drawing.Point(2, 332);
            this.defectImage.Margin = new System.Windows.Forms.Padding(0);
            this.defectImage.Name = "defectImage";
            this.defectImage.Size = new System.Drawing.Size(296, 200);
            this.defectImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.defectImage.TabIndex = 65;
            this.defectImage.TabStop = false;
            // 
            // SingleSheetResultPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "SingleSheetResultPanel";
            this.Size = new System.Drawing.Size(1030, 571);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanelR.ResumeLayout(false);
            this.tableLayoutPanelR.PerformLayout();
            this.tableLayoutPanelL.ResumeLayout(false);
            this.tableLayoutPanelL.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defectList)).EndInit();
            this.tableDefectSizeW.ResumeLayout(false);
            this.tableDefectSizeW.PerformLayout();
            this.tableDefectSizeH.ResumeLayout(false);
            this.tableDefectSizeH.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defectImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelL;
        private System.Windows.Forms.DataGridView defectList;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Label labelDefectImage;
        private System.Windows.Forms.Label labelDefectList;
        private System.Windows.Forms.PictureBox defectImage;
        private System.Windows.Forms.TableLayoutPanel tableDefectSizeW;
        private System.Windows.Forms.Label labelDefectSizeW;
        private System.Windows.Forms.Label labelDefectSizeH;
        private System.Windows.Forms.Label defectSizeW;
        private System.Windows.Forms.Label defectSizeH;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelR;
        private System.Windows.Forms.Label labelImage;
        private System.Windows.Forms.Panel imagePanel;
        private System.Windows.Forms.TableLayoutPanel tableDefectSizeH;
        private System.Windows.Forms.Button defectListSelectAll;
    }
}
