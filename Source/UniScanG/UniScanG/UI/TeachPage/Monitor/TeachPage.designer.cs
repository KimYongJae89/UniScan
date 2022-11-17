namespace UniScanG.UI.TeachPage.Monitor
{
    partial class TeachPage
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
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.remoteTeachingPanel = new Infragistics.Win.Misc.UltraPanel();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.settingPanel = new System.Windows.Forms.Panel();
            this.prevImage = new System.Windows.Forms.PictureBox();
            this.remoteTeachingPanel.ClientArea.SuspendLayout();
            this.remoteTeachingPanel.SuspendLayout();
            this.layoutMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.prevImage)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonPanel
            // 
            this.buttonPanel.AutoSize = true;
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonPanel.Location = new System.Drawing.Point(0, 0);
            this.buttonPanel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(0, 506);
            this.buttonPanel.TabIndex = 0;
            // 
            // remoteTeachingPanel
            // 
            // 
            // remoteTeachingPanel.ClientArea
            // 
            this.remoteTeachingPanel.ClientArea.Controls.Add(this.prevImage);
            this.remoteTeachingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.remoteTeachingPanel.Font = new System.Drawing.Font("맑은 고딕", 14F);
            this.remoteTeachingPanel.Location = new System.Drawing.Point(1, 1);
            this.remoteTeachingPanel.Margin = new System.Windows.Forms.Padding(0);
            this.remoteTeachingPanel.Name = "remoteTeachingPanel";
            this.remoteTeachingPanel.Size = new System.Drawing.Size(377, 504);
            this.remoteTeachingPanel.TabIndex = 2;
            // 
            // layoutMain
            // 
            this.layoutMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutMain.ColumnCount = 2;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutMain.Controls.Add(this.remoteTeachingPanel, 0, 0);
            this.layoutMain.Controls.Add(this.settingPanel, 1, 0);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 1;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Size = new System.Drawing.Size(680, 506);
            this.layoutMain.TabIndex = 3;
            // 
            // settingPanel
            // 
            this.settingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingPanel.Location = new System.Drawing.Point(379, 1);
            this.settingPanel.Margin = new System.Windows.Forms.Padding(0);
            this.settingPanel.Name = "settingPanel";
            this.settingPanel.Size = new System.Drawing.Size(300, 504);
            this.settingPanel.TabIndex = 0;
            // 
            // prevImage
            // 
            this.prevImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.prevImage.Location = new System.Drawing.Point(0, 0);
            this.prevImage.Margin = new System.Windows.Forms.Padding(0);
            this.prevImage.Name = "prevImage";
            this.prevImage.Size = new System.Drawing.Size(377, 504);
            this.prevImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.prevImage.TabIndex = 29;
            this.prevImage.TabStop = false;
            // 
            // TeachPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.layoutMain);
            this.Controls.Add(this.buttonPanel);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TeachPage";
            this.Size = new System.Drawing.Size(680, 506);
            this.remoteTeachingPanel.ClientArea.ResumeLayout(false);
            this.remoteTeachingPanel.ResumeLayout(false);
            this.layoutMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.prevImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
        private Infragistics.Win.Misc.UltraPanel remoteTeachingPanel;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.Panel settingPanel;
        private System.Windows.Forms.PictureBox prevImage;
    }
}
