namespace UniScanS.UI.Etc
{
    partial class StatusStripPanel
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
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.labelConnect = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelOpStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelInspectStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.White;
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.statusStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelConnect,
            this.labelOpStatus,
            this.labelInspectStatus});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(183, 25);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip";
            // 
            // labelConnect
            // 
            this.labelConnect.AutoSize = false;
            this.labelConnect.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.labelConnect.Margin = new System.Windows.Forms.Padding(0);
            this.labelConnect.Name = "labelConnect";
            this.labelConnect.Size = new System.Drawing.Size(45, 25);
            this.labelConnect.Spring = true;
            this.labelConnect.Text = "Cam #";
            // 
            // labelOpStatus
            // 
            this.labelOpStatus.AutoSize = false;
            this.labelOpStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.labelOpStatus.Margin = new System.Windows.Forms.Padding(0);
            this.labelOpStatus.Name = "labelOpStatus";
            this.labelOpStatus.Size = new System.Drawing.Size(45, 25);
            this.labelOpStatus.Spring = true;
            this.labelOpStatus.Text = "Wait";
            // 
            // labelInspectStatus
            // 
            this.labelInspectStatus.AutoSize = false;
            this.labelInspectStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.labelInspectStatus.Margin = new System.Windows.Forms.Padding(0);
            this.labelInspectStatus.Name = "labelInspectStatus";
            this.labelInspectStatus.Size = new System.Drawing.Size(45, 25);
            this.labelInspectStatus.Spring = true;
            this.labelInspectStatus.Text = "Run";
            // 
            // StatusStripPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "StatusStripPanel";
            this.Size = new System.Drawing.Size(183, 25);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel labelConnect;
        private System.Windows.Forms.ToolStripStatusLabel labelInspectStatus;
        private System.Windows.Forms.ToolStripStatusLabel labelOpStatus;
    }
}
