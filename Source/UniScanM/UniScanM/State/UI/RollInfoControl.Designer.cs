namespace UniScanM.UI
{
    partial class RollInfoControl
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.labelLotNo = new System.Windows.Forms.Label();
            this.labelModelName = new System.Windows.Forms.Label();
            this.labelOpName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelPaste = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.labelLotNo);
            this.splitContainer1.Panel1.Controls.Add(this.labelModelName);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.labelOpName);
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2.Controls.Add(this.labelPaste);
            this.splitContainer1.Size = new System.Drawing.Size(403, 70);
            this.splitContainer1.SplitterDistance = 189;
            this.splitContainer1.TabIndex = 8;
            // 
            // labelLotNo
            // 
            this.labelLotNo.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelLotNo.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelLotNo.Location = new System.Drawing.Point(0, 35);
            this.labelLotNo.Margin = new System.Windows.Forms.Padding(0);
            this.labelLotNo.Name = "labelLotNo";
            this.labelLotNo.Size = new System.Drawing.Size(189, 35);
            this.labelLotNo.TabIndex = 8;
            this.labelLotNo.Text = "Lot No :";
            this.labelLotNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelLotNo.Click += new System.EventHandler(this.labelLotNo_Click);
            // 
            // labelModelName
            // 
            this.labelModelName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelModelName.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelModelName.Location = new System.Drawing.Point(0, 0);
            this.labelModelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelModelName.Name = "labelModelName";
            this.labelModelName.Size = new System.Drawing.Size(189, 35);
            this.labelModelName.TabIndex = 7;
            this.labelModelName.Text = "Model  : ";
            this.labelModelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelModelName.Click += new System.EventHandler(this.labelModelName_Click);
            // 
            // labelOpName
            // 
            this.labelOpName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelOpName.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelOpName.Location = new System.Drawing.Point(45, 35);
            this.labelOpName.Margin = new System.Windows.Forms.Padding(0);
            this.labelOpName.Name = "labelOpName";
            this.labelOpName.Size = new System.Drawing.Size(165, 35);
            this.labelOpName.TabIndex = 10;
            this.labelOpName.Text = "Operator";
            this.labelOpName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::UniScanM.Properties.Resources.user;
            this.pictureBox1.Location = new System.Drawing.Point(0, 35);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(45, 35);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // labelPaste
            // 
            this.labelPaste.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPaste.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelPaste.Location = new System.Drawing.Point(0, 0);
            this.labelPaste.Margin = new System.Windows.Forms.Padding(0);
            this.labelPaste.Name = "labelPaste";
            this.labelPaste.Size = new System.Drawing.Size(210, 35);
            this.labelPaste.TabIndex = 8;
            this.labelPaste.Text = "Paste : ";
            this.labelPaste.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelPaste.Click += new System.EventHandler(this.labelPaste_Click);
            // 
            // RollInfoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "RollInfoControl";
            this.Size = new System.Drawing.Size(403, 70);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label labelLotNo;
        private System.Windows.Forms.Label labelModelName;
        private System.Windows.Forms.Label labelOpName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelPaste;
    }
}
