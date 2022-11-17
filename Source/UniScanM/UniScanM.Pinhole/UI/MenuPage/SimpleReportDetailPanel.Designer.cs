namespace UniScanM.Pinhole.UI.MenuPage
{
    partial class SimpleReportDetailPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelDefectImage = new System.Windows.Forms.Panel();
            this.buttonShortCut = new System.Windows.Forms.Button();
            this.layoutDefectView = new System.Windows.Forms.TableLayoutPanel();
            this.reportDefectListPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panelReportInfo = new System.Windows.Forms.Panel();
            this.sectionList = new System.Windows.Forms.DataGridView();
            this.columnCam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.panelDefectImage.SuspendLayout();
            this.layoutDefectView.SuspendLayout();
            this.panelReportInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionList)).BeginInit();
            this.SuspendLayout();
            // 
            // panelDefectImage
            // 
            this.panelDefectImage.Controls.Add(this.buttonShortCut);
            this.panelDefectImage.Controls.Add(this.layoutDefectView);
            this.panelDefectImage.Controls.Add(this.label2);
            this.panelDefectImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDefectImage.Location = new System.Drawing.Point(297, 0);
            this.panelDefectImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelDefectImage.Name = "panelDefectImage";
            this.panelDefectImage.Size = new System.Drawing.Size(396, 329);
            this.panelDefectImage.TabIndex = 2;
            // 
            // buttonShortCut
            // 
            this.buttonShortCut.BackColor = System.Drawing.Color.LightBlue;
            this.buttonShortCut.Image = global::UniScanM.Pinhole.Properties.Resources.open_folder_32;
            this.buttonShortCut.Location = new System.Drawing.Point(0, 0);
            this.buttonShortCut.Name = "buttonShortCut";
            this.buttonShortCut.Size = new System.Drawing.Size(48, 42);
            this.buttonShortCut.TabIndex = 30;
            this.buttonShortCut.UseVisualStyleBackColor = false;
            this.buttonShortCut.Click += new System.EventHandler(this.buttonShortCut_Click);
            // 
            // layoutDefectView
            // 
            this.layoutDefectView.ColumnCount = 2;
            this.layoutDefectView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutDefectView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutDefectView.Controls.Add(this.reportDefectListPanel, 0, 1);
            this.layoutDefectView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutDefectView.Location = new System.Drawing.Point(0, 41);
            this.layoutDefectView.Margin = new System.Windows.Forms.Padding(0);
            this.layoutDefectView.Name = "layoutDefectView";
            this.layoutDefectView.RowCount = 2;
            this.layoutDefectView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutDefectView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutDefectView.Size = new System.Drawing.Size(396, 288);
            this.layoutDefectView.TabIndex = 0;
            // 
            // reportDefectListPanel
            // 
            this.layoutDefectView.SetColumnSpan(this.reportDefectListPanel, 2);
            this.reportDefectListPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportDefectListPanel.Location = new System.Drawing.Point(0, 144);
            this.reportDefectListPanel.Margin = new System.Windows.Forms.Padding(0);
            this.reportDefectListPanel.Name = "reportDefectListPanel";
            this.reportDefectListPanel.Size = new System.Drawing.Size(396, 144);
            this.reportDefectListPanel.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightBlue;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(396, 41);
            this.label2.TabIndex = 29;
            this.label2.Text = "Defect View";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelReportInfo
            // 
            this.panelReportInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelReportInfo.Controls.Add(this.sectionList);
            this.panelReportInfo.Controls.Add(this.label1);
            this.panelReportInfo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelReportInfo.Location = new System.Drawing.Point(0, 0);
            this.panelReportInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelReportInfo.Name = "panelReportInfo";
            this.panelReportInfo.Size = new System.Drawing.Size(297, 329);
            this.panelReportInfo.TabIndex = 27;
            // 
            // sectionList
            // 
            this.sectionList.AllowUserToAddRows = false;
            this.sectionList.AllowUserToDeleteRows = false;
            this.sectionList.AllowUserToResizeColumns = false;
            this.sectionList.AllowUserToResizeRows = false;
            this.sectionList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.sectionList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sectionList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.sectionList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sectionList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnCam,
            this.columnType});
            this.sectionList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionList.Location = new System.Drawing.Point(0, 41);
            this.sectionList.Margin = new System.Windows.Forms.Padding(0);
            this.sectionList.MultiSelect = false;
            this.sectionList.Name = "sectionList";
            this.sectionList.ReadOnly = true;
            this.sectionList.RowHeadersVisible = false;
            this.sectionList.RowTemplate.Height = 23;
            this.sectionList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.sectionList.Size = new System.Drawing.Size(295, 286);
            this.sectionList.TabIndex = 29;
            this.sectionList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.sectionList_CellClick);
            this.sectionList.SelectionChanged += new System.EventHandler(this.sectionList_SelectionChanged);
            // 
            // columnCam
            // 
            this.columnCam.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.columnCam.DefaultCellStyle = dataGridViewCellStyle2;
            this.columnCam.HeaderText = "Section";
            this.columnCam.Name = "columnCam";
            this.columnCam.ReadOnly = true;
            // 
            // columnType
            // 
            this.columnType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.columnType.DefaultCellStyle = dataGridViewCellStyle3;
            this.columnType.HeaderText = "Count";
            this.columnType.Name = "columnType";
            this.columnType.ReadOnly = true;
            this.columnType.Width = 79;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightBlue;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(295, 41);
            this.label1.TabIndex = 28;
            this.label1.Text = "Result List";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SimpleReportDetailPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelDefectImage);
            this.Controls.Add(this.panelReportInfo);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SimpleReportDetailPanel";
            this.Size = new System.Drawing.Size(693, 329);
            this.Load += new System.EventHandler(this.SimpleReportDetailPanel_Load);
            this.panelDefectImage.ResumeLayout(false);
            this.layoutDefectView.ResumeLayout(false);
            this.panelReportInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelDefectImage;
        private System.Windows.Forms.Panel panelReportInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel layoutDefectView;
        private System.Windows.Forms.Panel reportDefectListPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView sectionList;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCam;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnType;
        private System.Windows.Forms.Button buttonShortCut;
    }
}
