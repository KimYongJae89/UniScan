namespace UniScanG.Module.Controller.Device.Laser
{
    partial class LaserStatusStripPanel
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
            this.labelLaser = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelReady = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelState = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.statusStrip.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labelLaser,
            this.labelReady,
            this.labelState});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(158, 25);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip";
            // 
            // labelLaser
            // 
            this.labelLaser.AutoSize = false;
            this.labelLaser.Margin = new System.Windows.Forms.Padding(0);
            this.labelLaser.Name = "labelLaser";
            this.labelLaser.Size = new System.Drawing.Size(40, 25);
            this.labelLaser.Spring = true;
            this.labelLaser.Text = "LASER";
            this.labelLaser.Click += new System.EventHandler(this.labelLaser_DoubleClick);
            this.labelLaser.DoubleClick += new System.EventHandler(this.labelLaser_DoubleClick);
            // 
            // labelReady
            // 
            this.labelReady.AutoSize = false;
            this.labelReady.Margin = new System.Windows.Forms.Padding(0);
            this.labelReady.Name = "labelReady";
            this.labelReady.Size = new System.Drawing.Size(35, 25);
            // 
            // labelState
            // 
            this.labelState.AutoSize = false;
            this.labelState.Margin = new System.Windows.Forms.Padding(0);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(35, 25);
            // 
            // LaserStatusStripPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.statusStrip);
            this.Name = "LaserStatusStripPanel";
            this.Size = new System.Drawing.Size(158, 25);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel labelLaser;
        private System.Windows.Forms.ToolStripStatusLabel labelReady;
        private System.Windows.Forms.ToolStripStatusLabel labelState;
    }
}
