namespace UniScanS.UI.Inspect
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
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            this.buttonStart = new Infragistics.Win.Misc.UltraButton();
            this.buttonPause = new Infragistics.Win.Misc.UltraButton();
            this.buttonStop = new Infragistics.Win.Misc.UltraButton();
            this.buttonReset = new Infragistics.Win.Misc.UltraButton();
            this.layoutInspect = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSplitter = new Infragistics.Win.Misc.UltraButton();
            this.defectPanel = new System.Windows.Forms.Panel();
            this.imagePanel = new System.Windows.Forms.Panel();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.layoutInspect.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.FontData.BoldAsString = "True";
            appearance6.FontData.Name = "Malgun Gothic";
            appearance6.FontData.SizeInPoints = 12F;
            appearance6.Image = global::UniScanS.Properties.Resources.Start;
            appearance6.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance6.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance6.TextVAlignAsString = "Bottom";
            this.buttonStart.Appearance = appearance6;
            this.buttonStart.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonStart.Dock = System.Windows.Forms.DockStyle.Top;
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
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.FontData.BoldAsString = "True";
            appearance7.FontData.Name = "Malgun Gothic";
            appearance7.FontData.SizeInPoints = 12F;
            appearance7.Image = global::UniScanS.Properties.Resources.Pause;
            appearance7.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance7.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance7.TextVAlignAsString = "Bottom";
            this.buttonPause.Appearance = appearance7;
            this.buttonPause.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonPause.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonPause.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonPause.Location = new System.Drawing.Point(0, 80);
            this.buttonPause.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(80, 80);
            this.buttonPause.TabIndex = 152;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            // 
            // buttonStop
            // 
            appearance8.BackColor = System.Drawing.Color.White;
            appearance8.FontData.BoldAsString = "True";
            appearance8.FontData.Name = "Malgun Gothic";
            appearance8.FontData.SizeInPoints = 12F;
            appearance8.Image = global::UniScanS.Properties.Resources.Stop;
            appearance8.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance8.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance8.TextVAlignAsString = "Bottom";
            this.buttonStop.Appearance = appearance8;
            this.buttonStop.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonStop.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonStop.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonStop.Location = new System.Drawing.Point(0, 160);
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
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.FontData.BoldAsString = "True";
            appearance9.FontData.Name = "Malgun Gothic";
            appearance9.FontData.SizeInPoints = 12F;
            appearance9.Image = global::UniScanS.Properties.Resources.Reset;
            appearance9.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance9.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance9.TextVAlignAsString = "Bottom";
            this.buttonReset.Appearance = appearance9;
            this.buttonReset.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonReset.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonReset.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonReset.Location = new System.Drawing.Point(0, 654);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(80, 80);
            this.buttonReset.TabIndex = 151;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
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
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            appearance10.FontData.BoldAsString = "True";
            appearance10.FontData.Name = "Malgun Gothic";
            appearance10.FontData.SizeInPoints = 16F;
            appearance10.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance10.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance10.TextVAlignAsString = "Bottom";
            this.buttonSplitter.Appearance = appearance10;
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
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonReset);
            this.panel1.Controls.Add(this.buttonStop);
            this.panel1.Controls.Add(this.buttonPause);
            this.panel1.Controls.Add(this.buttonStart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1447, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(80, 734);
            this.panel1.TabIndex = 0;
            // 
            // InspectPage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.layoutInspect);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "InspectPage";
            this.Size = new System.Drawing.Size(1527, 734);
            this.layoutInspect.ResumeLayout(false);
            this.layoutInspect.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Infragistics.Win.Misc.UltraButton buttonStart;
        private Infragistics.Win.Misc.UltraButton buttonStop;
        private Infragistics.Win.Misc.UltraButton buttonReset;
        private System.Windows.Forms.TableLayoutPanel layoutInspect;
        private Infragistics.Win.Misc.UltraButton buttonPause;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Panel imagePanel;
        private System.Windows.Forms.Panel defectPanel;
        private Infragistics.Win.Misc.UltraButton buttonSplitter;
        private System.Windows.Forms.Panel panel1;
    }
}
