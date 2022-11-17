namespace UniScanX.MPAlignment.UI.Pages
{
    partial class ReportPage
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
            this.lblSearchStartTime = new System.Windows.Forms.Label();
            this.lblSearchEnd = new System.Windows.Forms.Label();
            this.btnInspect = new System.Windows.Forms.Button();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.dgvSearchResult = new System.Windows.Forms.DataGridView();
            this.inspectNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlSummary = new System.Windows.Forms.Panel();
            this.tlpSummary = new System.Windows.Forms.TableLayoutPanel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblGood = new System.Windows.Forms.Label();
            this.lblNg = new System.Windows.Forms.Label();
            this.lblResultTotal = new System.Windows.Forms.Label();
            this.lblResultGood = new System.Windows.Forms.Label();
            this.lblResultNg = new System.Windows.Forms.Label();
            this.pnlLeftTop = new System.Windows.Forms.Panel();
            this.cmbProductList = new System.Windows.Forms.ComboBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.dtpSearchEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpSearchStart = new System.Windows.Forms.DateTimePicker();
            this.picImageResult = new System.Windows.Forms.PictureBox();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResult)).BeginInit();
            this.pnlSummary.SuspendLayout();
            this.tlpSummary.SuspendLayout();
            this.pnlLeftTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImageResult)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSearchStartTime
            // 
            this.lblSearchStartTime.AutoSize = true;
            this.lblSearchStartTime.Location = new System.Drawing.Point(24, 48);
            this.lblSearchStartTime.Name = "lblSearchStartTime";
            this.lblSearchStartTime.Size = new System.Drawing.Size(45, 21);
            this.lblSearchStartTime.TabIndex = 2;
            this.lblSearchStartTime.Text = "Start";
            // 
            // lblSearchEnd
            // 
            this.lblSearchEnd.AutoSize = true;
            this.lblSearchEnd.Location = new System.Drawing.Point(24, 85);
            this.lblSearchEnd.Name = "lblSearchEnd";
            this.lblSearchEnd.Size = new System.Drawing.Size(37, 21);
            this.lblSearchEnd.TabIndex = 3;
            this.lblSearchEnd.Text = "End";
            // 
            // btnInspect
            // 
            this.btnInspect.BackColor = System.Drawing.Color.SlateBlue;
            this.btnInspect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInspect.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInspect.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnInspect.Image = global::UniScanX.MPAlignment.Properties.Resources.insepct_48;
            this.btnInspect.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnInspect.Location = new System.Drawing.Point(304, 42);
            this.btnInspect.Name = "btnInspect";
            this.btnInspect.Size = new System.Drawing.Size(75, 68);
            this.btnInspect.TabIndex = 160;
            this.btnInspect.Text = "Search";
            this.btnInspect.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnInspect.UseVisualStyleBackColor = false;
            this.btnInspect.Click += new System.EventHandler(this.btnInspect_Click);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.dgvSearchResult);
            this.pnlLeft.Controls.Add(this.pnlSummary);
            this.pnlLeft.Controls.Add(this.pnlLeftTop);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(405, 1388);
            this.pnlLeft.TabIndex = 161;
            // 
            // dgvSearchResult
            // 
            this.dgvSearchResult.AllowUserToAddRows = false;
            this.dgvSearchResult.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvSearchResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSearchResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.inspectNo,
            this.result});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSearchResult.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSearchResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSearchResult.Location = new System.Drawing.Point(0, 216);
            this.dgvSearchResult.Name = "dgvSearchResult";
            this.dgvSearchResult.RowHeadersVisible = false;
            this.dgvSearchResult.RowTemplate.Height = 23;
            this.dgvSearchResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSearchResult.Size = new System.Drawing.Size(405, 1172);
            this.dgvSearchResult.TabIndex = 161;
            this.dgvSearchResult.SelectionChanged += new System.EventHandler(this.dgvSearchResult_SelectionChanged);
            // 
            // inspectNo
            // 
            this.inspectNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.inspectNo.DataPropertyName = "InspectionNo";
            this.inspectNo.HeaderText = "Inspect No";
            this.inspectNo.Name = "inspectNo";
            this.inspectNo.ReadOnly = true;
            // 
            // result
            // 
            this.result.DataPropertyName = "Judgment";
            this.result.HeaderText = "Result";
            this.result.Name = "result";
            this.result.ReadOnly = true;
            // 
            // pnlSummary
            // 
            this.pnlSummary.Controls.Add(this.tlpSummary);
            this.pnlSummary.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSummary.Location = new System.Drawing.Point(0, 134);
            this.pnlSummary.Name = "pnlSummary";
            this.pnlSummary.Size = new System.Drawing.Size(405, 82);
            this.pnlSummary.TabIndex = 163;
            // 
            // tlpSummary
            // 
            this.tlpSummary.ColumnCount = 2;
            this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSummary.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSummary.Controls.Add(this.lblTotal, 0, 0);
            this.tlpSummary.Controls.Add(this.lblGood, 0, 1);
            this.tlpSummary.Controls.Add(this.lblNg, 0, 2);
            this.tlpSummary.Controls.Add(this.lblResultTotal, 1, 0);
            this.tlpSummary.Controls.Add(this.lblResultGood, 1, 1);
            this.tlpSummary.Controls.Add(this.lblResultNg, 1, 2);
            this.tlpSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSummary.Location = new System.Drawing.Point(0, 0);
            this.tlpSummary.Name = "tlpSummary";
            this.tlpSummary.RowCount = 3;
            this.tlpSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpSummary.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSummary.Size = new System.Drawing.Size(405, 82);
            this.tlpSummary.TabIndex = 0;
            // 
            // lblTotal
            // 
            this.lblTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTotal.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.lblTotal.Location = new System.Drawing.Point(3, 0);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(196, 27);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "Total";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGood
            // 
            this.lblGood.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGood.ForeColor = System.Drawing.Color.LawnGreen;
            this.lblGood.Location = new System.Drawing.Point(3, 27);
            this.lblGood.Name = "lblGood";
            this.lblGood.Size = new System.Drawing.Size(196, 27);
            this.lblGood.TabIndex = 5;
            this.lblGood.Text = "Good";
            this.lblGood.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNg
            // 
            this.lblNg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblNg.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblNg.Location = new System.Drawing.Point(3, 54);
            this.lblNg.Name = "lblNg";
            this.lblNg.Size = new System.Drawing.Size(196, 28);
            this.lblNg.TabIndex = 6;
            this.lblNg.Text = "Ng";
            this.lblNg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblResultTotal
            // 
            this.lblResultTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResultTotal.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.lblResultTotal.Location = new System.Drawing.Point(205, 0);
            this.lblResultTotal.Name = "lblResultTotal";
            this.lblResultTotal.Size = new System.Drawing.Size(197, 27);
            this.lblResultTotal.TabIndex = 7;
            this.lblResultTotal.Text = "0 (0.00%)";
            this.lblResultTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblResultGood
            // 
            this.lblResultGood.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResultGood.ForeColor = System.Drawing.Color.LawnGreen;
            this.lblResultGood.Location = new System.Drawing.Point(205, 27);
            this.lblResultGood.Name = "lblResultGood";
            this.lblResultGood.Size = new System.Drawing.Size(197, 27);
            this.lblResultGood.TabIndex = 8;
            this.lblResultGood.Text = "0 (0.00%)";
            this.lblResultGood.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblResultNg
            // 
            this.lblResultNg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblResultNg.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblResultNg.Location = new System.Drawing.Point(205, 54);
            this.lblResultNg.Name = "lblResultNg";
            this.lblResultNg.Size = new System.Drawing.Size(197, 28);
            this.lblResultNg.TabIndex = 9;
            this.lblResultNg.Text = "0 (0.00%)";
            this.lblResultNg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlLeftTop
            // 
            this.pnlLeftTop.Controls.Add(this.cmbProductList);
            this.pnlLeftTop.Controls.Add(this.lblProduct);
            this.pnlLeftTop.Controls.Add(this.dtpSearchEnd);
            this.pnlLeftTop.Controls.Add(this.dtpSearchStart);
            this.pnlLeftTop.Controls.Add(this.btnInspect);
            this.pnlLeftTop.Controls.Add(this.lblSearchEnd);
            this.pnlLeftTop.Controls.Add(this.lblSearchStartTime);
            this.pnlLeftTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLeftTop.Location = new System.Drawing.Point(0, 0);
            this.pnlLeftTop.Name = "pnlLeftTop";
            this.pnlLeftTop.Size = new System.Drawing.Size(405, 134);
            this.pnlLeftTop.TabIndex = 162;
            // 
            // cmbProductList
            // 
            this.cmbProductList.FormattingEnabled = true;
            this.cmbProductList.Items.AddRange(new object[] {
            "All"});
            this.cmbProductList.Location = new System.Drawing.Point(93, 12);
            this.cmbProductList.Name = "cmbProductList";
            this.cmbProductList.Size = new System.Drawing.Size(196, 29);
            this.cmbProductList.TabIndex = 164;
            // 
            // lblProduct
            // 
            this.lblProduct.AutoSize = true;
            this.lblProduct.Location = new System.Drawing.Point(24, 12);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(68, 21);
            this.lblProduct.TabIndex = 163;
            this.lblProduct.Text = "Product";
            // 
            // dtpSearchEnd
            // 
            this.dtpSearchEnd.CalendarFont = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpSearchEnd.CustomFormat = "yyyy-MM-dd hh:mm";
            this.dtpSearchEnd.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpSearchEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSearchEnd.Location = new System.Drawing.Point(93, 79);
            this.dtpSearchEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpSearchEnd.MinDate = new System.DateTime(2020, 1, 1, 0, 0, 0, 0);
            this.dtpSearchEnd.Name = "dtpSearchEnd";
            this.dtpSearchEnd.Size = new System.Drawing.Size(196, 27);
            this.dtpSearchEnd.TabIndex = 162;
            this.dtpSearchEnd.Value = new System.DateTime(2020, 3, 2, 12, 0, 0, 0);
            // 
            // dtpSearchStart
            // 
            this.dtpSearchStart.CalendarFont = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpSearchStart.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpSearchStart.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtpSearchStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSearchStart.Location = new System.Drawing.Point(93, 44);
            this.dtpSearchStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpSearchStart.MinDate = new System.DateTime(2020, 1, 1, 0, 0, 0, 0);
            this.dtpSearchStart.Name = "dtpSearchStart";
            this.dtpSearchStart.Size = new System.Drawing.Size(196, 27);
            this.dtpSearchStart.TabIndex = 161;
            this.dtpSearchStart.Value = new System.DateTime(2020, 3, 2, 12, 0, 0, 0);
            // 
            // picImageResult
            // 
            this.picImageResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picImageResult.Location = new System.Drawing.Point(405, 0);
            this.picImageResult.Name = "picImageResult";
            this.picImageResult.Size = new System.Drawing.Size(1157, 1388);
            this.picImageResult.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImageResult.TabIndex = 162;
            this.picImageResult.TabStop = false;
            // 
            // ReportPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.Controls.Add(this.picImageResult);
            this.Controls.Add(this.pnlLeft);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ReportPage";
            this.Size = new System.Drawing.Size(1562, 1388);
            this.Load += new System.EventHandler(this.ReportPage_Load);
            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSearchResult)).EndInit();
            this.pnlSummary.ResumeLayout(false);
            this.tlpSummary.ResumeLayout(false);
            this.pnlLeftTop.ResumeLayout(false);
            this.pnlLeftTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImageResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblSearchStartTime;
        private System.Windows.Forms.Label lblSearchEnd;
        private System.Windows.Forms.Button btnInspect;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.DataGridView dgvSearchResult;
        private System.Windows.Forms.Panel pnlSummary;
        private System.Windows.Forms.Panel pnlLeftTop;
        private System.Windows.Forms.PictureBox picImageResult;
        private System.Windows.Forms.TableLayoutPanel tlpSummary;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblGood;
        private System.Windows.Forms.Label lblNg;
        private System.Windows.Forms.Label lblResultTotal;
        private System.Windows.Forms.Label lblResultGood;
        private System.Windows.Forms.Label lblResultNg;
        private System.Windows.Forms.DateTimePicker dtpSearchEnd;
        private System.Windows.Forms.DateTimePicker dtpSearchStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn inspectNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn result;
        private System.Windows.Forms.ComboBox cmbProductList;
        private System.Windows.Forms.Label lblProduct;
    }
}
