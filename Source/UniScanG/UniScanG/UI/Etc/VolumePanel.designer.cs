namespace UniScanG.UI.Etc
{
    partial class VolumePanel
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
            this.backupStrip = new System.Windows.Forms.StatusStrip();
            this.textLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.volumeBar = new System.Windows.Forms.ToolStripProgressBar();
            this.backupStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // backupStrip
            // 
            this.backupStrip.BackColor = System.Drawing.Color.Transparent;
            this.backupStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.backupStrip.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.backupStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.backupStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.textLabel,
            this.volumeBar});
            this.backupStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.backupStrip.Location = new System.Drawing.Point(0, 0);
            this.backupStrip.Name = "backupStrip";
            this.backupStrip.ShowItemToolTips = true;
            this.backupStrip.Size = new System.Drawing.Size(118, 25);
            this.backupStrip.TabIndex = 0;
            this.backupStrip.Text = "statusStrip";
            // 
            // textLabel
            // 
            this.textLabel.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.textLabel.Name = "textLabel";
            this.textLabel.Size = new System.Drawing.Size(18, 20);
            this.textLabel.Text = "#";
            // 
            // volumeBar
            // 
            this.volumeBar.Name = "volumeBar";
            this.volumeBar.Size = new System.Drawing.Size(50, 19);
            this.volumeBar.Value = 80;
            // 
            // VolumePanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.backupStrip);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "VolumePanel";
            this.Size = new System.Drawing.Size(118, 25);
            this.backupStrip.ResumeLayout(false);
            this.backupStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip backupStrip;
        private System.Windows.Forms.ToolStripStatusLabel textLabel;
        private System.Windows.Forms.ToolStripProgressBar volumeBar;
    }
}
