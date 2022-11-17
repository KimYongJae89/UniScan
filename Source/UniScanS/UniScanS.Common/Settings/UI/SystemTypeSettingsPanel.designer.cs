namespace UniScanS.Common.Settings.UI
{
    partial class SystemTypeSettingPanel
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
            this.systemType = new System.Windows.Forms.ComboBox();
            this.labelSystemType = new System.Windows.Forms.Label();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.subPanel = new System.Windows.Forms.Panel();
            this.labelResizeRatio = new System.Windows.Forms.Label();
            this.resizeRatio = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resizeRatio)).BeginInit();
            this.SuspendLayout();
            // 
            // systemType
            // 
            this.systemType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.systemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.systemType.FormattingEnabled = true;
            this.systemType.Items.AddRange(new object[] {
            "Gravure",
            "Screen"});
            this.systemType.Location = new System.Drawing.Point(252, 14);
            this.systemType.Margin = new System.Windows.Forms.Padding(0);
            this.systemType.Name = "systemType";
            this.systemType.Size = new System.Drawing.Size(110, 20);
            this.systemType.TabIndex = 0;
            // 
            // labelSystemType
            // 
            this.labelSystemType.AutoSize = true;
            this.labelSystemType.Location = new System.Drawing.Point(19, 17);
            this.labelSystemType.Margin = new System.Windows.Forms.Padding(0);
            this.labelSystemType.Name = "labelSystemType";
            this.labelSystemType.Size = new System.Drawing.Size(81, 12);
            this.labelSystemType.TabIndex = 0;
            this.labelSystemType.Text = "System Type";
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.resizeRatio);
            this.splitContainer.Panel1.Controls.Add(this.labelResizeRatio);
            this.splitContainer.Panel1.Controls.Add(this.systemType);
            this.splitContainer.Panel1.Controls.Add(this.labelSystemType);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.subPanel);
            this.splitContainer.Size = new System.Drawing.Size(376, 443);
            this.splitContainer.SplitterDistance = 84;
            this.splitContainer.TabIndex = 0;
            // 
            // subPanel
            // 
            this.subPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subPanel.Location = new System.Drawing.Point(0, 0);
            this.subPanel.Margin = new System.Windows.Forms.Padding(0);
            this.subPanel.Name = "subPanel";
            this.subPanel.Size = new System.Drawing.Size(374, 353);
            this.subPanel.TabIndex = 0;
            // 
            // labelResizeRatio
            // 
            this.labelResizeRatio.AutoSize = true;
            this.labelResizeRatio.Location = new System.Drawing.Point(19, 46);
            this.labelResizeRatio.Margin = new System.Windows.Forms.Padding(0);
            this.labelResizeRatio.Name = "labelResizeRatio";
            this.labelResizeRatio.Size = new System.Drawing.Size(76, 12);
            this.labelResizeRatio.TabIndex = 1;
            this.labelResizeRatio.Text = "Resize Ratio";
            // 
            // resizeRatio
            // 
            this.resizeRatio.DecimalPlaces = 2;
            this.resizeRatio.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.resizeRatio.Location = new System.Drawing.Point(302, 44);
            this.resizeRatio.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.resizeRatio.Name = "resizeRatio";
            this.resizeRatio.Size = new System.Drawing.Size(60, 21);
            this.resizeRatio.TabIndex = 2;
            this.resizeRatio.ValueChanged += new System.EventHandler(this.resizeRatio_ValueChanged);
            // 
            // SystemTypeSettingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.splitContainer);
            this.Name = "SystemTypeSettingPanel";
            this.Size = new System.Drawing.Size(376, 443);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resizeRatio)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox systemType;
        private System.Windows.Forms.Label labelSystemType;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel subPanel;
        private System.Windows.Forms.NumericUpDown resizeRatio;
        private System.Windows.Forms.Label labelResizeRatio;
    }
}
