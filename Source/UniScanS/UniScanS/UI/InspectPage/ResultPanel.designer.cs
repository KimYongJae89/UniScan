namespace UniScanG.UI.InspectPage
{
    partial class ResultPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.labelCam = new System.Windows.Forms.Label();
            this.labelTaught = new System.Windows.Forms.TextBox();
            this.layoutResult = new System.Windows.Forms.TableLayoutPanel();
            this.panelSheet = new System.Windows.Forms.Panel();
            this.sheetResultGrid = new System.Windows.Forms.DataGridView();
            this.columnIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnInspTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnExportTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelSheetResult = new System.Windows.Forms.Label();
            this.layoutFiducial = new System.Windows.Forms.TableLayoutPanel();
            this.fidResultGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelFiducialResult = new System.Windows.Forms.Label();
            this.layoutResult.SuspendLayout();
            this.panelSheet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetResultGrid)).BeginInit();
            this.layoutFiducial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fidResultGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // labelCam
            // 
            this.labelCam.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelCam.Font = new System.Drawing.Font("맑은 고딕", 20F);
            this.labelCam.Location = new System.Drawing.Point(0, 0);
            this.labelCam.Name = "labelCam";
            this.labelCam.Size = new System.Drawing.Size(119, 52);
            this.labelCam.TabIndex = 137;
            this.labelCam.Text = "Cam 1 :";
            // 
            // labelTaught
            // 
            this.labelTaught.BackColor = System.Drawing.Color.Green;
            this.labelTaught.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelTaught.Font = new System.Drawing.Font("맑은 고딕", 22F, System.Drawing.FontStyle.Bold);
            this.labelTaught.ForeColor = System.Drawing.Color.White;
            this.labelTaught.Location = new System.Drawing.Point(121, 0);
            this.labelTaught.Name = "labelTaught";
            this.labelTaught.ReadOnly = true;
            this.labelTaught.Size = new System.Drawing.Size(170, 47);
            this.labelTaught.TabIndex = 136;
            this.labelTaught.Text = "Finished";
            this.labelTaught.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // layoutResult
            // 
            this.layoutResult.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutResult.ColumnCount = 2;
            this.layoutResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutResult.Controls.Add(this.panelSheet, 1, 0);
            this.layoutResult.Controls.Add(this.layoutFiducial, 0, 0);
            this.layoutResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutResult.Location = new System.Drawing.Point(0, 0);
            this.layoutResult.Margin = new System.Windows.Forms.Padding(0);
            this.layoutResult.Name = "layoutResult";
            this.layoutResult.RowCount = 1;
            this.layoutResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutResult.Size = new System.Drawing.Size(651, 253);
            this.layoutResult.TabIndex = 51;
            // 
            // panelSheet
            // 
            this.panelSheet.Controls.Add(this.sheetResultGrid);
            this.panelSheet.Controls.Add(this.labelSheetResult);
            this.panelSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSheet.Location = new System.Drawing.Point(252, 1);
            this.panelSheet.Margin = new System.Windows.Forms.Padding(0);
            this.panelSheet.Name = "panelSheet";
            this.panelSheet.Size = new System.Drawing.Size(398, 251);
            this.panelSheet.TabIndex = 56;
            // 
            // sheetResultGrid
            // 
            this.sheetResultGrid.AllowUserToAddRows = false;
            this.sheetResultGrid.AllowUserToDeleteRows = false;
            this.sheetResultGrid.AllowUserToResizeColumns = false;
            this.sheetResultGrid.AllowUserToResizeRows = false;
            this.sheetResultGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.sheetResultGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sheetResultGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.sheetResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sheetResultGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnIndex,
            this.columnQty,
            this.columnInspTime,
            this.columnExportTime});
            this.sheetResultGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetResultGrid.Location = new System.Drawing.Point(0, 60);
            this.sheetResultGrid.Margin = new System.Windows.Forms.Padding(0);
            this.sheetResultGrid.MultiSelect = false;
            this.sheetResultGrid.Name = "sheetResultGrid";
            this.sheetResultGrid.ReadOnly = true;
            this.sheetResultGrid.RowHeadersVisible = false;
            this.sheetResultGrid.RowTemplate.Height = 23;
            this.sheetResultGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.sheetResultGrid.Size = new System.Drawing.Size(398, 191);
            this.sheetResultGrid.TabIndex = 56;
            // 
            // columnIndex
            // 
            this.columnIndex.HeaderText = "Pattern";
            this.columnIndex.Name = "columnIndex";
            this.columnIndex.ReadOnly = true;
            // 
            // columnQty
            // 
            this.columnQty.HeaderText = "Qty.";
            this.columnQty.Name = "columnQty";
            this.columnQty.ReadOnly = true;
            this.columnQty.Width = 60;
            // 
            // columnInspTime
            // 
            this.columnInspTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnInspTime.HeaderText = "Insp time";
            this.columnInspTime.Name = "columnInspTime";
            this.columnInspTime.ReadOnly = true;
            // 
            // columnExportTime
            // 
            this.columnExportTime.HeaderText = "Export time";
            this.columnExportTime.Name = "columnExportTime";
            this.columnExportTime.ReadOnly = true;
            this.columnExportTime.Width = 150;
            // 
            // labelSheetResult
            // 
            this.labelSheetResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelSheetResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSheetResult.Font = new System.Drawing.Font("맑은 고딕", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSheetResult.Location = new System.Drawing.Point(0, 0);
            this.labelSheetResult.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetResult.Name = "labelSheetResult";
            this.labelSheetResult.Size = new System.Drawing.Size(398, 60);
            this.labelSheetResult.TabIndex = 55;
            this.labelSheetResult.Text = "Sheet";
            this.labelSheetResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutFiducial
            // 
            this.layoutFiducial.ColumnCount = 1;
            this.layoutFiducial.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFiducial.Controls.Add(this.fidResultGrid, 0, 1);
            this.layoutFiducial.Controls.Add(this.labelFiducialResult, 0, 0);
            this.layoutFiducial.Dock = System.Windows.Forms.DockStyle.Left;
            this.layoutFiducial.Location = new System.Drawing.Point(1, 1);
            this.layoutFiducial.Margin = new System.Windows.Forms.Padding(0);
            this.layoutFiducial.Name = "layoutFiducial";
            this.layoutFiducial.RowCount = 2;
            this.layoutFiducial.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutFiducial.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFiducial.Size = new System.Drawing.Size(250, 251);
            this.layoutFiducial.TabIndex = 57;
            // 
            // fidResultGrid
            // 
            this.fidResultGrid.AllowUserToAddRows = false;
            this.fidResultGrid.AllowUserToDeleteRows = false;
            this.fidResultGrid.AllowUserToResizeColumns = false;
            this.fidResultGrid.AllowUserToResizeRows = false;
            this.fidResultGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.fidResultGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.fidResultGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.fidResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fidResultGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn3});
            this.fidResultGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fidResultGrid.Location = new System.Drawing.Point(0, 60);
            this.fidResultGrid.Margin = new System.Windows.Forms.Padding(0);
            this.fidResultGrid.MultiSelect = false;
            this.fidResultGrid.Name = "fidResultGrid";
            this.fidResultGrid.ReadOnly = true;
            this.fidResultGrid.RowHeadersVisible = false;
            this.fidResultGrid.RowTemplate.Height = 23;
            this.fidResultGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.fidResultGrid.Size = new System.Drawing.Size(250, 191);
            this.fidResultGrid.TabIndex = 55;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Pattern";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "Insp time";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // labelFiducialResult
            // 
            this.labelFiducialResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelFiducialResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFiducialResult.Font = new System.Drawing.Font("맑은 고딕", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelFiducialResult.Location = new System.Drawing.Point(0, 0);
            this.labelFiducialResult.Margin = new System.Windows.Forms.Padding(0);
            this.labelFiducialResult.Name = "labelFiducialResult";
            this.labelFiducialResult.Size = new System.Drawing.Size(250, 60);
            this.labelFiducialResult.TabIndex = 54;
            this.labelFiducialResult.Text = "Fiducial";
            this.labelFiducialResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ResultPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.layoutResult);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ResultPanel";
            this.Size = new System.Drawing.Size(651, 253);
            this.VisibleChanged += new System.EventHandler(this.ResultPanel_VisibleChanged);
            this.layoutResult.ResumeLayout(false);
            this.panelSheet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sheetResultGrid)).EndInit();
            this.layoutFiducial.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fidResultGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelCam;
        private System.Windows.Forms.TextBox labelTaught;
        private System.Windows.Forms.TableLayoutPanel layoutResult;
        private System.Windows.Forms.Label labelSheetResult;
        private System.Windows.Forms.Panel panelSheet;
        private System.Windows.Forms.DataGridView sheetResultGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnInspTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnExportTime;
        private System.Windows.Forms.TableLayoutPanel layoutFiducial;
        private System.Windows.Forms.DataGridView fidResultGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Label labelFiducialResult;
    }
}
