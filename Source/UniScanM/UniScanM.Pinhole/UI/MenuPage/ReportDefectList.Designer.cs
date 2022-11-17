namespace UniScanM.Pinhole.UI.MenuPage
{
    partial class ReportDefectList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridDetail = new System.Windows.Forms.DataGridView();
            this.camIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yPos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.width = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.height = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnSheetLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridDetail
            // 
            this.dataGridDetail.AllowUserToAddRows = false;
            this.dataGridDetail.AllowUserToDeleteRows = false;
            this.dataGridDetail.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.camIndex,
            this.type,
            this.xPos,
            this.yPos,
            this.width,
            this.height,
            this.columnSheetLength,
            this.Image});
            this.dataGridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridDetail.Location = new System.Drawing.Point(0, 0);
            this.dataGridDetail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dataGridDetail.Name = "dataGridDetail";
            this.dataGridDetail.ReadOnly = true;
            this.dataGridDetail.RowHeadersVisible = false;
            this.dataGridDetail.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.dataGridDetail.RowTemplate.Height = 23;
            this.dataGridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridDetail.Size = new System.Drawing.Size(1176, 531);
            this.dataGridDetail.TabIndex = 27;
            this.dataGridDetail.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridDetail_CellDoubleClick);
            // 
            // camIndex
            // 
            this.camIndex.HeaderText = "Cam";
            this.camIndex.Name = "camIndex";
            this.camIndex.ReadOnly = true;
            this.camIndex.Width = 150;
            // 
            // type
            // 
            this.type.HeaderText = "Type";
            this.type.Name = "type";
            this.type.ReadOnly = true;
            // 
            // xPos
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.xPos.DefaultCellStyle = dataGridViewCellStyle2;
            this.xPos.HeaderText = "X [mm]";
            this.xPos.Name = "xPos";
            this.xPos.ReadOnly = true;
            this.xPos.Width = 150;
            // 
            // yPos
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.yPos.DefaultCellStyle = dataGridViewCellStyle3;
            this.yPos.HeaderText = "Y [mm]";
            this.yPos.Name = "yPos";
            this.yPos.ReadOnly = true;
            this.yPos.Width = 150;
            // 
            // width
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.width.DefaultCellStyle = dataGridViewCellStyle4;
            this.width.HeaderText = "Width [um]";
            this.width.Name = "width";
            this.width.ReadOnly = true;
            this.width.Width = 140;
            // 
            // height
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.height.DefaultCellStyle = dataGridViewCellStyle5;
            this.height.HeaderText = "Height [um]";
            this.height.Name = "height";
            this.height.ReadOnly = true;
            this.height.Width = 140;
            // 
            // columnSheetLength
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.columnSheetLength.DefaultCellStyle = dataGridViewCellStyle6;
            this.columnSheetLength.HeaderText = "Sheet Length [mm]";
            this.columnSheetLength.Name = "columnSheetLength";
            this.columnSheetLength.ReadOnly = true;
            this.columnSheetLength.Width = 150;
            // 
            // Image
            // 
            this.Image.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Image.HeaderText = "Image";
            this.Image.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Image.Name = "Image";
            this.Image.ReadOnly = true;
            // 
            // ReportDefectList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridDetail);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ReportDefectList";
            this.Size = new System.Drawing.Size(1176, 531);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn camIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn xPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn yPos;
        private System.Windows.Forms.DataGridViewTextBoxColumn width;
        private System.Windows.Forms.DataGridViewTextBoxColumn height;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSheetLength;
        private System.Windows.Forms.DataGridViewImageColumn Image;
    }
}
