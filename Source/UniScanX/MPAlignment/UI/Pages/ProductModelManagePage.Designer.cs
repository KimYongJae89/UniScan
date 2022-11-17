namespace UniScanX.MPAlignment.UI.Pages
{
    partial class ProductModelManagePage
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnCloseModel = new System.Windows.Forms.Button();
            this.btnNewModel = new System.Windows.Forms.Button();
            this.flpBoardModelList = new System.Windows.Forms.FlowLayoutPanel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblDirectory = new System.Windows.Forms.Label();
            this.picModelImage = new System.Windows.Forms.PictureBox();
            this.pnlTop.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picModelImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnCloseModel);
            this.pnlTop.Controls.Add(this.btnNewModel);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1317, 79);
            this.pnlTop.TabIndex = 0;
            // 
            // btnCloseModel
            // 
            this.btnCloseModel.AccessibleName = "Operation rate";
            this.btnCloseModel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnCloseModel.FlatAppearance.BorderSize = 0;
            this.btnCloseModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseModel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCloseModel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnCloseModel.Image = global::UniScanX.MPAlignment.Properties.Resources.close_48;
            this.btnCloseModel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseModel.Location = new System.Drawing.Point(1052, 0);
            this.btnCloseModel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCloseModel.Name = "btnCloseModel";
            this.btnCloseModel.Size = new System.Drawing.Size(265, 79);
            this.btnCloseModel.TabIndex = 12;
            this.btnCloseModel.Text = "Close Model";
            this.btnCloseModel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCloseModel.UseVisualStyleBackColor = true;
            this.btnCloseModel.Click += new System.EventHandler(this.btnCloseModel_Click);
            // 
            // btnNewModel
            // 
            this.btnNewModel.AccessibleName = "Operation rate";
            this.btnNewModel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnNewModel.FlatAppearance.BorderSize = 0;
            this.btnNewModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewModel.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnNewModel.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnNewModel.Image = global::UniScanX.MPAlignment.Properties.Resources.addproduct_48;
            this.btnNewModel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewModel.Location = new System.Drawing.Point(0, 0);
            this.btnNewModel.Margin = new System.Windows.Forms.Padding(0);
            this.btnNewModel.Name = "btnNewModel";
            this.btnNewModel.Size = new System.Drawing.Size(267, 79);
            this.btnNewModel.TabIndex = 11;
            this.btnNewModel.Text = "New Model";
            this.btnNewModel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNewModel.UseVisualStyleBackColor = true;
            this.btnNewModel.Click += new System.EventHandler(this.btnAddProduct_Click);
            // 
            // flpBoardModelList
            // 
            this.flpBoardModelList.AutoScroll = true;
            this.flpBoardModelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpBoardModelList.Location = new System.Drawing.Point(0, 157);
            this.flpBoardModelList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.flpBoardModelList.Name = "flpBoardModelList";
            this.flpBoardModelList.Size = new System.Drawing.Size(866, 768);
            this.flpBoardModelList.TabIndex = 1;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblDirectory);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 79);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(866, 78);
            this.panelTop.TabIndex = 4;
            // 
            // lblDirectory
            // 
            this.lblDirectory.AutoSize = true;
            this.lblDirectory.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblDirectory.ForeColor = System.Drawing.Color.White;
            this.lblDirectory.Location = new System.Drawing.Point(6, 8);
            this.lblDirectory.Name = "lblDirectory";
            this.lblDirectory.Size = new System.Drawing.Size(128, 36);
            this.lblDirectory.TabIndex = 4;
            this.lblDirectory.Text = "Directory";
            // 
            // picModelImage
            // 
            this.picModelImage.Dock = System.Windows.Forms.DockStyle.Right;
            this.picModelImage.Image = global::UniScanX.MPAlignment.Properties.Resources.machineImage2;
            this.picModelImage.Location = new System.Drawing.Point(866, 79);
            this.picModelImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picModelImage.Name = "picModelImage";
            this.picModelImage.Size = new System.Drawing.Size(451, 846);
            this.picModelImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picModelImage.TabIndex = 2;
            this.picModelImage.TabStop = false;
            // 
            // ProductModelManagePage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.Controls.Add(this.flpBoardModelList);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.picModelImage);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ProductModelManagePage";
            this.Size = new System.Drawing.Size(1317, 925);
            this.pnlTop.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picModelImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnNewModel;
        private System.Windows.Forms.FlowLayoutPanel flpBoardModelList;
        private System.Windows.Forms.Button btnCloseModel;
        private System.Windows.Forms.PictureBox picModelImage;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblDirectory;
    }
}
