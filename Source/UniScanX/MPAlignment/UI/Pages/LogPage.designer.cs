namespace UniScanX.MPAlignment.UI.Pages
{
    partial class LogPage
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
            this.lsbLogs = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lsbLogs
            // 
            this.lsbLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.lsbLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsbLogs.Font = new System.Drawing.Font("새굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lsbLogs.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lsbLogs.FormattingEnabled = true;
            this.lsbLogs.ItemHeight = 26;
            this.lsbLogs.Location = new System.Drawing.Point(0, 0);
            this.lsbLogs.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.lsbLogs.Name = "lsbLogs";
            this.lsbLogs.Size = new System.Drawing.Size(1584, 1198);
            this.lsbLogs.TabIndex = 0;
            // 
            // LogPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lsbLogs);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "LogPage";
            this.Size = new System.Drawing.Size(1584, 1198);
            this.VisibleChanged += new System.EventHandler(this.LogPage_VisibleChanged);
            this.Enter += new System.EventHandler(this.LogPage_Enter);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lsbLogs;
    }
}
