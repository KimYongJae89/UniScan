namespace UniScanM.Pinhole.UI.MenuPage
{
    partial class LastDefectPanel
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
            this.panelDefectInfo = new System.Windows.Forms.Panel();
            this.labelLastDefectTime = new System.Windows.Forms.Label();
            this.panelLastFovImage = new System.Windows.Forms.Panel();
            this.panelDefectInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelDefectInfo
            // 
            this.panelDefectInfo.Controls.Add(this.labelLastDefectTime);
            this.panelDefectInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDefectInfo.Location = new System.Drawing.Point(0, 0);
            this.panelDefectInfo.Name = "panelDefectInfo";
            this.panelDefectInfo.Size = new System.Drawing.Size(520, 45);
            this.panelDefectInfo.TabIndex = 1;
            // 
            // labelLastDefectTime
            // 
            this.labelLastDefectTime.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLastDefectTime.BackColor = System.Drawing.Color.Gold;
            this.labelLastDefectTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLastDefectTime.Location = new System.Drawing.Point(0, 0);
            this.labelLastDefectTime.Name = "labelLastDefectTime";
            this.labelLastDefectTime.Size = new System.Drawing.Size(520, 45);
            this.labelLastDefectTime.TabIndex = 0;
            this.labelLastDefectTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelLastFovImage
            // 
            this.panelLastFovImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLastFovImage.Location = new System.Drawing.Point(0, 45);
            this.panelLastFovImage.Margin = new System.Windows.Forms.Padding(0);
            this.panelLastFovImage.Name = "panelLastFovImage";
            this.panelLastFovImage.Size = new System.Drawing.Size(520, 367);
            this.panelLastFovImage.TabIndex = 0;
            // 
            // LastDefectPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelLastFovImage);
            this.Controls.Add(this.panelDefectInfo);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LastDefectPanel";
            this.Size = new System.Drawing.Size(520, 412);
            this.panelDefectInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelDefectInfo;
        private System.Windows.Forms.Panel panelLastFovImage;
        private System.Windows.Forms.Label labelLastDefectTime;
    }
}
