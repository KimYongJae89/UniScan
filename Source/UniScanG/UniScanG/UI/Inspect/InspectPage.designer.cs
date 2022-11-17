namespace UniScanG.UI.Inspect
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InspectPage));
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.menuPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonStart = new Infragistics.Win.Misc.UltraButton();
            this.buttonPause = new Infragistics.Win.Misc.UltraButton();
            this.buttonStop = new Infragistics.Win.Misc.UltraButton();
            this.buttonReset = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonUpdate = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonObserver = new Infragistics.Win.Misc.UltraButton();
            this.ultraButtonAlarm = new Infragistics.Win.Misc.UltraButton();
            this.layoutInspect = new System.Windows.Forms.TableLayoutPanel();
            this.panelImage = new System.Windows.Forms.Panel();
            this.layoutInspect2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.defectPanel = new System.Windows.Forms.Panel();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.panelLengthChart = new System.Windows.Forms.Panel();
            this.panelExtraInfo = new System.Windows.Forms.Panel();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.buttonBlinkTimer = new System.Windows.Forms.Timer(this.components);
            this.menuPanel.SuspendLayout();
            this.layoutInspect.SuspendLayout();
            this.layoutInspect2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuPanel
            // 
            this.menuPanel.AutoSize = true;
            this.menuPanel.Controls.Add(this.buttonStart);
            this.menuPanel.Controls.Add(this.buttonPause);
            this.menuPanel.Controls.Add(this.buttonStop);
            this.menuPanel.Controls.Add(this.buttonReset);
            this.menuPanel.Controls.Add(this.ultraButtonUpdate);
            this.menuPanel.Controls.Add(this.ultraButtonObserver);
            this.menuPanel.Controls.Add(this.ultraButtonAlarm);
            this.menuPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.menuPanel.Location = new System.Drawing.Point(1063, 0);
            this.menuPanel.Margin = new System.Windows.Forms.Padding(0);
            this.menuPanel.Name = "menuPanel";
            this.menuPanel.Size = new System.Drawing.Size(84, 575);
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
            this.buttonStart.ImageSize = new System.Drawing.Size(50, 50);
            this.buttonStart.Location = new System.Drawing.Point(0, 0);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(84, 80);
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
            this.buttonPause.ImageSize = new System.Drawing.Size(50, 50);
            this.buttonPause.Location = new System.Drawing.Point(0, 81);
            this.buttonPause.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(84, 80);
            this.buttonPause.TabIndex = 152;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
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
            this.buttonStop.ImageSize = new System.Drawing.Size(50, 50);
            this.buttonStop.Location = new System.Drawing.Point(0, 162);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(84, 80);
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
            this.buttonReset.ImageSize = new System.Drawing.Size(50, 50);
            this.buttonReset.Location = new System.Drawing.Point(0, 243);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(84, 80);
            this.buttonReset.TabIndex = 151;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // ultraButtonUpdate
            // 
            appearance5.BackColor = System.Drawing.Color.White;
            appearance5.FontData.BoldAsString = "True";
            appearance5.FontData.Name = "Malgun Gothic";
            appearance5.FontData.SizeInPoints = 12F;
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            appearance5.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance5.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance5.TextVAlignAsString = "Bottom";
            this.ultraButtonUpdate.Appearance = appearance5;
            this.ultraButtonUpdate.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.ultraButtonUpdate.ImageSize = new System.Drawing.Size(50, 50);
            this.ultraButtonUpdate.Location = new System.Drawing.Point(0, 324);
            this.ultraButtonUpdate.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.ultraButtonUpdate.Name = "ultraButtonUpdate";
            this.ultraButtonUpdate.Size = new System.Drawing.Size(84, 80);
            this.ultraButtonUpdate.TabIndex = 153;
            this.ultraButtonUpdate.Text = "Update";
            this.ultraButtonUpdate.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraButtonUpdate.Click += new System.EventHandler(this.ultraButtonUpdate_Click);
            // 
            // ultraButtonObserver
            // 
            appearance6.BackColor = System.Drawing.Color.White;
            appearance6.FontData.BoldAsString = "True";
            appearance6.FontData.Name = "Malgun Gothic";
            appearance6.FontData.SizeInPoints = 12F;
            appearance6.Image = ((object)(resources.GetObject("appearance6.Image")));
            appearance6.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance6.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance6.TextVAlignAsString = "Bottom";
            this.ultraButtonObserver.Appearance = appearance6;
            this.ultraButtonObserver.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.ultraButtonObserver.ImageSize = new System.Drawing.Size(50, 50);
            this.ultraButtonObserver.Location = new System.Drawing.Point(0, 405);
            this.ultraButtonObserver.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.ultraButtonObserver.Name = "ultraButtonObserver";
            this.ultraButtonObserver.Size = new System.Drawing.Size(84, 80);
            this.ultraButtonObserver.TabIndex = 154;
            this.ultraButtonObserver.Text = "Observer";
            this.ultraButtonObserver.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraButtonObserver.Click += new System.EventHandler(this.ultraButtonObserver_Click);
            // 
            // ultraButtonAlarm
            // 
            appearance7.BackColor = System.Drawing.Color.White;
            appearance7.FontData.BoldAsString = "True";
            appearance7.FontData.Name = "Malgun Gothic";
            appearance7.FontData.SizeInPoints = 12F;
            appearance7.Image = global::UniScanG.Properties.Resources.alert;
            appearance7.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance7.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance7.TextVAlignAsString = "Bottom";
            this.ultraButtonAlarm.Appearance = appearance7;
            this.ultraButtonAlarm.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.ultraButtonAlarm.ImageSize = new System.Drawing.Size(50, 50);
            this.ultraButtonAlarm.Location = new System.Drawing.Point(0, 486);
            this.ultraButtonAlarm.Margin = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.ultraButtonAlarm.Name = "ultraButtonAlarm";
            this.ultraButtonAlarm.Size = new System.Drawing.Size(84, 80);
            this.ultraButtonAlarm.TabIndex = 155;
            this.ultraButtonAlarm.Text = "Alarm";
            this.ultraButtonAlarm.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.ultraButtonAlarm.Click += new System.EventHandler(this.ultraButtonAlarm_Click);
            // 
            // layoutInspect
            // 
            this.layoutInspect.AutoSize = true;
            this.layoutInspect.ColumnCount = 3;
            this.layoutInspect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutInspect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutInspect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.layoutInspect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutInspect.Controls.Add(this.panelImage, 0, 0);
            this.layoutInspect.Controls.Add(this.layoutInspect2, 1, 0);
            this.layoutInspect.Controls.Add(this.panelInfo, 2, 0);
            this.layoutInspect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutInspect.Location = new System.Drawing.Point(0, 0);
            this.layoutInspect.Margin = new System.Windows.Forms.Padding(0);
            this.layoutInspect.Name = "layoutInspect";
            this.layoutInspect.RowCount = 1;
            this.layoutInspect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutInspect.Size = new System.Drawing.Size(1063, 575);
            this.layoutInspect.TabIndex = 6;
            // 
            // panelImage
            // 
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(0, 0);
            this.panelImage.Margin = new System.Windows.Forms.Padding(0);
            this.panelImage.MinimumSize = new System.Drawing.Size(150, 150);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(663, 575);
            this.panelImage.TabIndex = 156;
            // 
            // layoutInspect2
            // 
            this.layoutInspect2.AutoSize = true;
            this.layoutInspect2.ColumnCount = 1;
            this.layoutInspect2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutInspect2.Controls.Add(this.panel1, 0, 1);
            this.layoutInspect2.Controls.Add(this.panelExtraInfo, 0, 0);
            this.layoutInspect2.Dock = System.Windows.Forms.DockStyle.Right;
            this.layoutInspect2.Location = new System.Drawing.Point(663, 0);
            this.layoutInspect2.Margin = new System.Windows.Forms.Padding(0);
            this.layoutInspect2.Name = "layoutInspect2";
            this.layoutInspect2.RowCount = 2;
            this.layoutInspect2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutInspect2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutInspect2.Size = new System.Drawing.Size(150, 575);
            this.layoutInspect2.TabIndex = 154;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.defectPanel);
            this.panel1.Controls.Add(this.ultraSplitter1);
            this.panel1.Controls.Add(this.panelLengthChart);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 150);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(150, 425);
            this.panel1.TabIndex = 0;
            // 
            // defectPanel
            // 
            this.defectPanel.AutoSize = true;
            this.defectPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectPanel.Location = new System.Drawing.Point(0, 0);
            this.defectPanel.Margin = new System.Windows.Forms.Padding(0);
            this.defectPanel.MinimumSize = new System.Drawing.Size(150, 0);
            this.defectPanel.Name = "defectPanel";
            this.defectPanel.Size = new System.Drawing.Size(150, 410);
            this.defectPanel.TabIndex = 14;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.Color.AliceBlue;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ultraSplitter1.Location = new System.Drawing.Point(0, 410);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 150;
            this.ultraSplitter1.Size = new System.Drawing.Size(150, 15);
            this.ultraSplitter1.TabIndex = 111;
            this.ultraSplitter1.Visible = false;
            this.ultraSplitter1.CollapsedChanged += new System.EventHandler(this.ultraSplitter1_CollapsedChanged);
            // 
            // panelLengthChart
            // 
            this.panelLengthChart.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLengthChart.Location = new System.Drawing.Point(0, 425);
            this.panelLengthChart.Margin = new System.Windows.Forms.Padding(0);
            this.panelLengthChart.Name = "panelLengthChart";
            this.panelLengthChart.Size = new System.Drawing.Size(150, 0);
            this.panelLengthChart.TabIndex = 110;
            this.panelLengthChart.Visible = false;
            // 
            // panelExtraInfo
            // 
            this.panelExtraInfo.AutoSize = true;
            this.panelExtraInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelExtraInfo.Location = new System.Drawing.Point(0, 0);
            this.panelExtraInfo.Margin = new System.Windows.Forms.Padding(0);
            this.panelExtraInfo.MinimumSize = new System.Drawing.Size(150, 150);
            this.panelExtraInfo.Name = "panelExtraInfo";
            this.panelExtraInfo.Size = new System.Drawing.Size(150, 150);
            this.panelExtraInfo.TabIndex = 156;
            this.panelExtraInfo.Visible = false;
            // 
            // panelInfo
            // 
            this.panelInfo.AutoScroll = true;
            this.panelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfo.Location = new System.Drawing.Point(813, 0);
            this.panelInfo.Margin = new System.Windows.Forms.Padding(0);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(250, 575);
            this.panelInfo.TabIndex = 153;
            // 
            // buttonBlinkTimer
            // 
            this.buttonBlinkTimer.Interval = 240;
            this.buttonBlinkTimer.Tick += new System.EventHandler(this.buttonBlinkTimer_Tick);
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
            this.Size = new System.Drawing.Size(1147, 575);
            this.Load += new System.EventHandler(this.InspectPage_Load);
            this.menuPanel.ResumeLayout(false);
            this.layoutInspect.ResumeLayout(false);
            this.layoutInspect.PerformLayout();
            this.layoutInspect2.ResumeLayout(false);
            this.layoutInspect2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private Infragistics.Win.Misc.UltraButton ultraButtonUpdate;
        private Infragistics.Win.Misc.UltraButton ultraButtonObserver;
        private Infragistics.Win.Misc.UltraButton ultraButtonAlarm;
        private System.Windows.Forms.Timer buttonBlinkTimer;
        private System.Windows.Forms.Panel defectPanel;
        private System.Windows.Forms.TableLayoutPanel layoutInspect2;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.Panel panelExtraInfo;
        private System.Windows.Forms.Panel panelLengthChart;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
    }
}
