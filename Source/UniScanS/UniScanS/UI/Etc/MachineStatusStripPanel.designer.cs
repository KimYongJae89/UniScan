namespace UniScanS.UI.Etc
{
    partial class MachineStatusStripPanel
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
            this.components = new System.ComponentModel.Container();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.labelRun = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelRolling = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelStop = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelAlarm = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer = new System.Windows.Forms.Timer(this.components);
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
            this.labelRun,
            this.labelRolling,
            this.labelStop,
            this.labelAlarm});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(258, 25);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip";
            // 
            // labelRun
            // 
            this.labelRun.AutoSize = false;
            this.labelRun.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.labelRun.Margin = new System.Windows.Forms.Padding(0);
            this.labelRun.Name = "labelRun";
            this.labelRun.Size = new System.Drawing.Size(60, 25);
            this.labelRun.Spring = true;
            this.labelRun.Text = "Machine";
            // 
            // labelRolling
            // 
            this.labelRolling.AutoSize = false;
            this.labelRolling.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.labelRolling.Margin = new System.Windows.Forms.Padding(0);
            this.labelRolling.Name = "labelRolling";
            this.labelRolling.Size = new System.Drawing.Size(60, 25);
            this.labelRolling.Spring = true;
            this.labelRolling.Text = "Rolling";
            // 
            // labelStop
            // 
            this.labelStop.AutoSize = false;
            this.labelStop.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.labelStop.Margin = new System.Windows.Forms.Padding(0);
            this.labelStop.Name = "labelStop";
            this.labelStop.Size = new System.Drawing.Size(50, 25);
            this.labelStop.Spring = true;
            this.labelStop.Text = "Stop";
            // 
            // labelAlarm
            // 
            this.labelAlarm.AutoSize = false;
            this.labelAlarm.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.labelAlarm.Margin = new System.Windows.Forms.Padding(0);
            this.labelAlarm.Name = "labelAlarm";
            this.labelAlarm.Size = new System.Drawing.Size(40, 25);
            this.labelAlarm.Spring = true;
            this.labelAlarm.Text = "Alarm";
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 50;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // MachineStatusStripPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.statusStrip);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MachineStatusStripPanel";
            this.Size = new System.Drawing.Size(258, 25);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel labelRun;
        private System.Windows.Forms.ToolStripStatusLabel labelRolling;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ToolStripStatusLabel labelStop;
        private System.Windows.Forms.ToolStripStatusLabel labelAlarm;
    }
}
