namespace UniScanG.UI.Inspect
{
    partial class ImagePanel
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
            this.layoutImage = new System.Windows.Forms.TableLayoutPanel();
            this.labelImage = new System.Windows.Forms.Label();
            this.image = new System.Windows.Forms.Panel();
            this.layoutImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutImage
            // 
            this.layoutImage.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutImage.ColumnCount = 1;
            this.layoutImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutImage.Controls.Add(this.labelImage, 0, 0);
            this.layoutImage.Controls.Add(this.image, 0, 1);
            this.layoutImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutImage.Location = new System.Drawing.Point(0, 0);
            this.layoutImage.Margin = new System.Windows.Forms.Padding(0);
            this.layoutImage.Name = "layoutImage";
            this.layoutImage.RowCount = 2;
            this.layoutImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutImage.Size = new System.Drawing.Size(672, 588);
            this.layoutImage.TabIndex = 0;
            // 
            // labelImage
            // 
            this.labelImage.AutoSize = true;
            this.labelImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImage.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelImage.Location = new System.Drawing.Point(1, 1);
            this.labelImage.Margin = new System.Windows.Forms.Padding(0);
            this.labelImage.Name = "labelImage";
            this.labelImage.Size = new System.Drawing.Size(670, 35);
            this.labelImage.TabIndex = 3;
            this.labelImage.Text = "Image";
            this.labelImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // image
            // 
            this.image.Dock = System.Windows.Forms.DockStyle.Fill;
            this.image.Location = new System.Drawing.Point(1, 37);
            this.image.Margin = new System.Windows.Forms.Padding(0);
            this.image.Name = "image";
            this.image.Size = new System.Drawing.Size(670, 550);
            this.image.TabIndex = 4;
            // 
            // ImagePanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.layoutImage);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ImagePanel";
            this.Size = new System.Drawing.Size(672, 588);
            this.Load += new System.EventHandler(this.ImagePanel_Load);
            this.layoutImage.ResumeLayout(false);
            this.layoutImage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutImage;
        private System.Windows.Forms.Label labelImage;
        private System.Windows.Forms.Panel image;
    }
}
