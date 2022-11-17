namespace UniScanG.UI.InspectPage
{
    partial class InspectPage
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            this.menuPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonStart = new Infragistics.Win.Misc.UltraButton();
            this.buttonPause = new Infragistics.Win.Misc.UltraButton();
            this.buttonStop = new Infragistics.Win.Misc.UltraButton();
            this.buttonReset = new Infragistics.Win.Misc.UltraButton();
            this.layoutInspect = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSplitter = new Infragistics.Win.Misc.UltraButton();
            this.defectPanel = new System.Windows.Forms.Panel();
            this.imagePanel = new System.Windows.Forms.Panel();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.menuPanel.SuspendLayout();
            this.layoutInspect.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuPanel
            // 
            this.menuPanel.AutoSize = true;
            this.menuPanel.Controls.Add(this.buttonStart);
            this.menuPanel.Controls.Add(this.buttonPause);
            this.menuPanel.Controls.Add(this.buttonStop);
            this.menuPanel.Controls.Add(this.buttonReset);
            this.menuPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.menuPanel.Location = new System.Drawing.Point(1447, 0);
            this.menuPanel.Margin = new System.Windows.Forms.Padding(0);
            this.menuPanel.Name = "menuPanel";
            this.menuPanel.Size = new System.Drawing.Size(80, 734);
            this.menuPanel.TabIndex = 3;
            // 
            // buttonStart
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.FontData.BoldAsString = "True";
            appearance1.FontData.Name = "Malgun Gothic";
            appearance1.FontData.SizeInPoints = 12F;
            appearance1.Image = global::UniScanG.Properties.Resources.Start;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance1.TextVAlignAsString = "Bottom";
            this.buttonStart.Appearance = appearance1;
            this.buttonStart.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonStart.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonStart.Location = new System.Drawing.Point(0, 0);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(80, 80);
            this.buttonStart.TabIndex = 148;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // buttonPause
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.FontData.BoldAsString = "True";
            appearance2.FontData.Name = "Malgun Gothic";
            appearance2.FontData.SizeInPoints = 12F;
            appearance2.Image = global::UniScanG.Properties.Resources.Pause;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance2.TextVAlignAsString = "Bottom";
            this.buttonPause.Appearance = appearance2;
            this.buttonPause.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonPause.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonPause.Location = new System.Drawing.Point(0, 81);
            this.buttonPause.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(80, 80);
            this.buttonPause.TabIndex = 152;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // buttonStop
            // 
            appearance3.BackColor = System.Drawing.Color.White;
            appearance3.FontData.BoldAsString = "True";
            appearance3.FontData.Name = "Malgun Gothic";
            appearance3.FontData.SizeInPoints = 12F;
            appearance3.Image = global::UniScanG.Properties.Resources.Stop;
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance3.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance3.TextVAlignAsString = "Bottom";
            this.buttonStop.Appearance = appearance3;
            this.buttonStop.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonStop.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonStop.Location = new System.Drawing.Point(0, 162);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(80, 80);
            this.buttonStop.TabIndex = 149;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonReset
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.FontData.BoldAsString = "True";
            appearance4.FontData.Name = "Malgun Gothic";
            appearance4.FontData.SizeInPoints = 12F;
            appearance4.Image = global::UniScanG.Properties.Resources.Reset;
            appearance4.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance4.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance4.TextVAlignAsString = "Bottom";
            this.buttonReset.Appearance = appearance4;
            this.buttonReset.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonReset.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonReset.Location = new System.Drawing.Point(0, 243);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(80, 80);
            this.buttonReset.TabIndex = 151;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // layoutInspect
            // 
            this.layoutInspect.AutoSize = true;
            this.layoutInspect.ColumnCount = 4;
            this.layoutInspect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutInspect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.layoutInspect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutInspect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.layoutInspect.Controls.Add(this.buttonSplitter, 1, 0);
            this.layoutInspect.Controls.Add(this.defectPanel, 2, 0);
            this.layoutInspect.Controls.Add(this.imagePanel, 0, 0);
            this.layoutInspect.Controls.Add(this.panelInfo, 3, 0);
            this.layoutInspect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutInspect.Location = new System.Drawing.Point(0, 0);
            this.layoutInspect.Margin = new System.Windows.Forms.Padding(0);
            this.layoutInspect.Name = "layoutInspect";
            this.layoutInspect.RowCount = 1;
            this.layoutInspect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutInspect.Size = new System.Drawing.Size(1447, 734);
            this.layoutInspect.TabIndex = 6;
            // 
            // buttonSplitter
            // 
            appearance5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            appearance5.FontData.BoldAsString = "True";
            appearance5.FontData.Name = "Malgun Gothic";
            appearance5.FontData.SizeInPoints = 16F;
            appearance5.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance5.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance5.TextVAlignAsString = "Bottom";
            this.buttonSplitter.Appearance = appearance5;
            this.buttonSplitter.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Windows8Button;
            this.buttonSplitter.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.buttonSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSplitter.Location = new System.Drawing.Point(1182, 0);
            this.buttonSplitter.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSplitter.Name = "buttonSplitter";
            this.buttonSplitter.Size = new System.Drawing.Size(15, 734);
            this.buttonSplitter.TabIndex = 1;
            this.buttonSplitter.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonSplitter.Click += new System.EventHandler(this.buttonSplitter_Click);
            // 
            // defectPanel
            // 
            this.defectPanel.AutoSize = true;
            this.defectPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectPanel.Location = new System.Drawing.Point(1197, 0);
            this.defectPanel.Margin = new System.Windows.Forms.Padding(0);
            this.defectPanel.Name = "defectPanel";
            this.defectPanel.Size = new System.Drawing.Size(1, 734);
            this.defectPanel.TabIndex = 14;
            // 
            // imagePanel
            // 
            this.imagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagePanel.Location = new System.Drawing.Point(0, 0);
            this.imagePanel.Margin = new System.Windows.Forms.Padding(0);
            this.imagePanel.Name = "imagePanel";
            this.imagePanel.Size = new System.Drawing.Size(1182, 734);
            this.imagePanel.TabIndex = 155;
            // 
            // panelInfo
            // 
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfo.Location = new System.Drawing.Point(1197, 0);
            this.panelInfo.Margin = new System.Windows.Forms.Padding(0);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(250, 734);
            this.panelInfo.TabIndex = 153;
            // 
            // InspectPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.layoutInspect);
            this.Controls.Add(this.menuPanel);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "InspectPage";
            this.Size = new System.Drawing.Size(1527, 734);
            this.menuPanel.ResumeLayout(false);
            this.layoutInspect.ResumeLayout(false);
            this.layoutInspect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel menuPanel;
        private Infragistics.Win.Misc.UltraButton buttonStart;
        private Infragistics.Win.Misc.UltraButton buttonStop;
        private Infragistics.Win.Misc.UltraButton buttonReset;
        private System.Windows.Forms.TableLayoutPanel layoutInspect;
        private Infragistics.Win.Misc.UltraButton buttonPause;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Panel imagePanel;
        private System.Windows.Forms.Panel defectPanel;
        private Infragistics.Win.Misc.UltraButton buttonSplitter;
    }
}
