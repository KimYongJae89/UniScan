namespace UniScanX.MPAlignment.UI.Components
{
    partial class ProductModelCard
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
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnCopyModel = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblLastUpdateTime = new System.Windows.Forms.Label();
            this.lblLastModify = new System.Windows.Forms.Label();
            this.pnlLeftDeco = new System.Windows.Forms.Panel();
            this.lblRailSizeTag = new System.Windows.Forms.Label();
            this.lblSize = new System.Windows.Forms.Label();
            this.lblModelName = new System.Windows.Forms.Label();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.DimGray;
            this.pnlMain.Controls.Add(this.btnCopyModel);
            this.pnlMain.Controls.Add(this.btnEdit);
            this.pnlMain.Controls.Add(this.btnDelete);
            this.pnlMain.Controls.Add(this.lblLastUpdateTime);
            this.pnlMain.Controls.Add(this.lblLastModify);
            this.pnlMain.Controls.Add(this.pnlLeftDeco);
            this.pnlMain.Controls.Add(this.lblRailSizeTag);
            this.pnlMain.Controls.Add(this.lblSize);
            this.pnlMain.Controls.Add(this.lblModelName);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(374, 141);
            this.pnlMain.TabIndex = 0;
            this.pnlMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlMain_Paint);
            this.pnlMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ProductModelCaptured);
            this.pnlMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ProductModelSelected);
            this.pnlMain.MouseEnter += new System.EventHandler(this.pnlMain_MouseEnter);
            this.pnlMain.MouseLeave += new System.EventHandler(this.pnlMain_MouseLeave);
            // 
            // btnCopyModel
            // 
            this.btnCopyModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnCopyModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopyModel.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopyModel.Image = global::UniScanX.MPAlignment.Properties.Resources.copy_32;
            this.btnCopyModel.Location = new System.Drawing.Point(337, 66);
            this.btnCopyModel.Name = "btnCopyModel";
            this.btnCopyModel.Size = new System.Drawing.Size(33, 33);
            this.btnCopyModel.TabIndex = 22;
            this.btnCopyModel.UseVisualStyleBackColor = false;
            this.btnCopyModel.Click += new System.EventHandler(this.btnCopyModel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.BackColor = System.Drawing.Color.SteelBlue;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEdit.Image = global::UniScanX.MPAlignment.Properties.Resources.pen_32;
            this.btnEdit.Location = new System.Drawing.Point(337, 29);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(33, 33);
            this.btnEdit.TabIndex = 19;
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            this.btnEdit.MouseLeave += new System.EventHandler(this.This_MouseLeave);
            this.btnEdit.MouseHover += new System.EventHandler(this.This_MouseHover);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Image = global::UniScanX.MPAlignment.Properties.Resources.wastebasket_32;
            this.btnDelete.Location = new System.Drawing.Point(337, 105);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(33, 33);
            this.btnDelete.TabIndex = 18;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            this.btnDelete.MouseLeave += new System.EventHandler(this.This_MouseLeave);
            this.btnDelete.MouseHover += new System.EventHandler(this.This_MouseHover);
            // 
            // lblLastUpdateTime
            // 
            this.lblLastUpdateTime.AutoSize = true;
            this.lblLastUpdateTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblLastUpdateTime.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastUpdateTime.ForeColor = System.Drawing.Color.Azure;
            this.lblLastUpdateTime.Location = new System.Drawing.Point(108, 93);
            this.lblLastUpdateTime.Name = "lblLastUpdateTime";
            this.lblLastUpdateTime.Size = new System.Drawing.Size(109, 17);
            this.lblLastUpdateTime.TabIndex = 17;
            this.lblLastUpdateTime.Text = "2019-12-12 09:25";
            this.lblLastUpdateTime.Visible = false;
            this.lblLastUpdateTime.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ProductModelSelected);
            this.lblLastUpdateTime.MouseLeave += new System.EventHandler(this.This_MouseLeave);
            this.lblLastUpdateTime.MouseHover += new System.EventHandler(this.This_MouseHover);
            // 
            // lblLastModify
            // 
            this.lblLastModify.AutoSize = true;
            this.lblLastModify.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblLastModify.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastModify.ForeColor = System.Drawing.Color.Azure;
            this.lblLastModify.Location = new System.Drawing.Point(18, 93);
            this.lblLastModify.Name = "lblLastModify";
            this.lblLastModify.Size = new System.Drawing.Size(84, 17);
            this.lblLastModify.TabIndex = 16;
            this.lblLastModify.Text = "Last updated";
            this.lblLastModify.Visible = false;
            this.lblLastModify.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ProductModelSelected);
            this.lblLastModify.MouseLeave += new System.EventHandler(this.This_MouseLeave);
            this.lblLastModify.MouseHover += new System.EventHandler(this.This_MouseHover);
            // 
            // pnlLeftDeco
            // 
            this.pnlLeftDeco.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.pnlLeftDeco.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeftDeco.Location = new System.Drawing.Point(0, 0);
            this.pnlLeftDeco.Name = "pnlLeftDeco";
            this.pnlLeftDeco.Size = new System.Drawing.Size(15, 141);
            this.pnlLeftDeco.TabIndex = 15;
            this.pnlLeftDeco.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ProductModelSelected);
            this.pnlLeftDeco.MouseLeave += new System.EventHandler(this.This_MouseLeave);
            this.pnlLeftDeco.MouseHover += new System.EventHandler(this.This_MouseHover);
            // 
            // lblRailSizeTag
            // 
            this.lblRailSizeTag.AutoSize = true;
            this.lblRailSizeTag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRailSizeTag.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRailSizeTag.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblRailSizeTag.Location = new System.Drawing.Point(13, 97);
            this.lblRailSizeTag.Name = "lblRailSizeTag";
            this.lblRailSizeTag.Size = new System.Drawing.Size(0, 17);
            this.lblRailSizeTag.TabIndex = 14;
            // 
            // lblSize
            // 
            this.lblSize.AutoSize = true;
            this.lblSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSize.Font = new System.Drawing.Font("Yu Gothic UI", 10.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSize.ForeColor = System.Drawing.Color.Azure;
            this.lblSize.Location = new System.Drawing.Point(21, 52);
            this.lblSize.Name = "lblSize";
            this.lblSize.Size = new System.Drawing.Size(96, 19);
            this.lblSize.TabIndex = 11;
            this.lblSize.Text = "Size(250,250)";
            this.lblSize.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ProductModelSelected);
            this.lblSize.MouseLeave += new System.EventHandler(this.This_MouseLeave);
            this.lblSize.MouseHover += new System.EventHandler(this.This_MouseHover);
            // 
            // lblModelName
            // 
            this.lblModelName.AutoSize = true;
            this.lblModelName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblModelName.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModelName.ForeColor = System.Drawing.Color.Azure;
            this.lblModelName.Location = new System.Drawing.Point(21, 7);
            this.lblModelName.Name = "lblModelName";
            this.lblModelName.Size = new System.Drawing.Size(58, 21);
            this.lblModelName.TabIndex = 11;
            this.lblModelName.Text = "Model";
            this.lblModelName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ProductModelSelected);
            this.lblModelName.MouseLeave += new System.EventHandler(this.This_MouseLeave);
            this.lblModelName.MouseHover += new System.EventHandler(this.This_MouseHover);
            // 
            // ProductModelCard
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnlMain);
            this.Name = "ProductModelCard";
            this.Size = new System.Drawing.Size(374, 141);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblLastUpdateTime;
        private System.Windows.Forms.Label lblLastModify;
        private System.Windows.Forms.Panel pnlLeftDeco;
        private System.Windows.Forms.Label lblRailSizeTag;
        private System.Windows.Forms.Label lblModelName;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnCopyModel;
        private System.Windows.Forms.Label lblSize;
    }
}
