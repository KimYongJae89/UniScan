namespace UniScanM.CEDMS.UI
{
    partial class ReportPanel
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.tabPageZeroing = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.layoutRaw = new System.Windows.Forms.TableLayoutPanel();
            this.ultraTabSharedControlsPage2 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.settingTabControl = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.tabPageZeroing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingTabControl)).BeginInit();
            this.settingTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageZeroing
            // 
            this.tabPageZeroing.Controls.Add(this.layoutRaw);
            this.tabPageZeroing.Location = new System.Drawing.Point(1, 1);
            this.tabPageZeroing.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageZeroing.Name = "tabPageZeroing";
            this.tabPageZeroing.Size = new System.Drawing.Size(691, 389);
            // 
            // layoutRaw
            // 
            this.layoutRaw.ColumnCount = 2;
            this.layoutRaw.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutRaw.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutRaw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutRaw.Location = new System.Drawing.Point(0, 0);
            this.layoutRaw.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.layoutRaw.Name = "layoutRaw";
            this.layoutRaw.RowCount = 1;
            this.layoutRaw.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRaw.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 389F));
            this.layoutRaw.Size = new System.Drawing.Size(691, 389);
            this.layoutRaw.TabIndex = 26;
            // 
            // ultraTabSharedControlsPage2
            // 
            this.ultraTabSharedControlsPage2.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage2.Margin = new System.Windows.Forms.Padding(0);
            this.ultraTabSharedControlsPage2.Name = "ultraTabSharedControlsPage2";
            this.ultraTabSharedControlsPage2.Size = new System.Drawing.Size(691, 389);
            // 
            // settingTabControl
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            this.settingTabControl.ClientAreaAppearance = appearance1;
            this.settingTabControl.Controls.Add(this.ultraTabSharedControlsPage2);
            this.settingTabControl.Controls.Add(this.tabPageZeroing);
            this.settingTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingTabControl.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.settingTabControl.InterRowSpacing = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.InterTabSpacing = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.Location = new System.Drawing.Point(0, 0);
            this.settingTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.settingTabControl.Name = "settingTabControl";
            this.settingTabControl.SharedControlsPage = this.ultraTabSharedControlsPage2;
            this.settingTabControl.Size = new System.Drawing.Size(747, 393);
            this.settingTabControl.SpaceAfterTabs = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.SpaceBeforeTabs = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.PropertyPageSelected;
            appearance2.FontData.BoldAsString = "True";
            appearance2.FontData.Name = "맑은 고딕";
            this.settingTabControl.TabHeaderAreaAppearance = appearance2;
            this.settingTabControl.TabIndex = 27;
            this.settingTabControl.TabOrientation = Infragistics.Win.UltraWinTabs.TabOrientation.RightTop;
            this.settingTabControl.TabPadding = new System.Drawing.Size(10, 10);
            appearance3.BackColor = System.Drawing.Color.Transparent;
            ultraTab2.Appearance = appearance3;
            ultraTab2.FixedWidth = 140;
            ultraTab2.Key = "Data";
            ultraTab2.TabPage = this.tabPageZeroing;
            ultraTab2.Text = "Data";
            this.settingTabControl.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab2});
            this.settingTabControl.ViewStyle = Infragistics.Win.UltraWinTabControl.ViewStyle.Office2007;
            // 
            // ReportPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.settingTabControl);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "ReportPanel";
            this.Size = new System.Drawing.Size(747, 393);
            this.Load += new System.EventHandler(this.ReportPage_Load);
            this.tabPageZeroing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.settingTabControl)).EndInit();
            this.settingTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageZeroing;
        private System.Windows.Forms.TableLayoutPanel layoutRaw;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage2;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl settingTabControl;
    }
}
