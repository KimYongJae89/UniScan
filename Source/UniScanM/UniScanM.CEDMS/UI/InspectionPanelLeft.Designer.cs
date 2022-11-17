namespace UniScanM.CEDMS.UI
{
    partial class InspectionPanelLeft
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
            this.VibrationViewPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panelInFeed = new System.Windows.Forms.Panel();
            this.inFeedRaw = new System.Windows.Forms.Label();
            this.lblInFeed = new System.Windows.Forms.Label();
            this.panelOutFeed = new System.Windows.Forms.Panel();
            this.outFeedRaw = new System.Windows.Forms.Label();
            this.lblOutFeed = new System.Windows.Forms.Label();
            this.VibrationViewPanel.SuspendLayout();
            this.panelInFeed.SuspendLayout();
            this.panelOutFeed.SuspendLayout();
            this.SuspendLayout();
            // 
            // VibrationViewPanel
            // 
            this.VibrationViewPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.VibrationViewPanel.ColumnCount = 2;
            this.VibrationViewPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.VibrationViewPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.VibrationViewPanel.Controls.Add(this.panelInFeed, 0, 0);
            this.VibrationViewPanel.Controls.Add(this.panelOutFeed, 1, 0);
            this.VibrationViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VibrationViewPanel.Location = new System.Drawing.Point(0, 0);
            this.VibrationViewPanel.Margin = new System.Windows.Forms.Padding(0);
            this.VibrationViewPanel.Name = "VibrationViewPanel";
            this.VibrationViewPanel.RowCount = 3;
            this.VibrationViewPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.VibrationViewPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 61.6F));
            this.VibrationViewPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38.4F));
            this.VibrationViewPanel.Size = new System.Drawing.Size(715, 524);
            this.VibrationViewPanel.TabIndex = 0;
            // 
            // panelInFeed
            // 
            this.panelInFeed.Controls.Add(this.inFeedRaw);
            this.panelInFeed.Controls.Add(this.lblInFeed);
            this.panelInFeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInFeed.Location = new System.Drawing.Point(4, 4);
            this.panelInFeed.Name = "panelInFeed";
            this.panelInFeed.Size = new System.Drawing.Size(350, 54);
            this.panelInFeed.TabIndex = 42;
            // 
            // inFeedRaw
            // 
            this.inFeedRaw.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.inFeedRaw.Dock = System.Windows.Forms.DockStyle.Right;
            this.inFeedRaw.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.inFeedRaw.Location = new System.Drawing.Point(270, 0);
            this.inFeedRaw.Margin = new System.Windows.Forms.Padding(0);
            this.inFeedRaw.Name = "inFeedRaw";
            this.inFeedRaw.Size = new System.Drawing.Size(80, 54);
            this.inFeedRaw.TabIndex = 38;
            this.inFeedRaw.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lblInFeed
            // 
            this.lblInFeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.lblInFeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInFeed.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold);
            this.lblInFeed.Location = new System.Drawing.Point(0, 0);
            this.lblInFeed.Margin = new System.Windows.Forms.Padding(0);
            this.lblInFeed.Name = "lblInFeed";
            this.lblInFeed.Size = new System.Drawing.Size(350, 54);
            this.lblInFeed.TabIndex = 37;
            this.lblInFeed.Text = "In Feed";
            this.lblInFeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelOutFeed
            // 
            this.panelOutFeed.Controls.Add(this.outFeedRaw);
            this.panelOutFeed.Controls.Add(this.lblOutFeed);
            this.panelOutFeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOutFeed.Location = new System.Drawing.Point(361, 4);
            this.panelOutFeed.Name = "panelOutFeed";
            this.panelOutFeed.Size = new System.Drawing.Size(350, 54);
            this.panelOutFeed.TabIndex = 43;
            // 
            // outFeedRaw
            // 
            this.outFeedRaw.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.outFeedRaw.Dock = System.Windows.Forms.DockStyle.Right;
            this.outFeedRaw.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.outFeedRaw.Location = new System.Drawing.Point(270, 0);
            this.outFeedRaw.Margin = new System.Windows.Forms.Padding(0);
            this.outFeedRaw.Name = "outFeedRaw";
            this.outFeedRaw.Size = new System.Drawing.Size(80, 54);
            this.outFeedRaw.TabIndex = 39;
            this.outFeedRaw.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // lblOutFeed
            // 
            this.lblOutFeed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.lblOutFeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblOutFeed.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold);
            this.lblOutFeed.Location = new System.Drawing.Point(0, 0);
            this.lblOutFeed.Margin = new System.Windows.Forms.Padding(0);
            this.lblOutFeed.Name = "lblOutFeed";
            this.lblOutFeed.Size = new System.Drawing.Size(350, 54);
            this.lblOutFeed.TabIndex = 38;
            this.lblOutFeed.Text = "Out Feed";
            this.lblOutFeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InspectionPanelLeft
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.VibrationViewPanel);
            this.Name = "InspectionPanelLeft";
            this.Size = new System.Drawing.Size(715, 524);
            this.VibrationViewPanel.ResumeLayout(false);
            this.panelInFeed.ResumeLayout(false);
            this.panelOutFeed.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel VibrationViewPanel;
        private System.Windows.Forms.Panel panelInFeed;
        private System.Windows.Forms.Label inFeedRaw;
        private System.Windows.Forms.Label lblInFeed;
        private System.Windows.Forms.Panel panelOutFeed;
        private System.Windows.Forms.Label outFeedRaw;
        private System.Windows.Forms.Label lblOutFeed;
    }
}
