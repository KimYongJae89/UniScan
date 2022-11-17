namespace UniScanM.Gloss.UI
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabPageZeroing = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.layoutRaw = new System.Windows.Forms.TableLayoutPanel();
            this.ultraTabSharedControlsPage2 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.settingTabControl = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.scandataListView = new System.Windows.Forms.DataGridView();
            this.tabPageZeroing.SuspendLayout();
            this.layoutRaw.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.settingTabControl)).BeginInit();
            this.settingTabControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scandataListView)).BeginInit();
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
            this.layoutRaw.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutRaw.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRaw.Controls.Add(this.scandataListView, 0, 0);
            this.layoutRaw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutRaw.Location = new System.Drawing.Point(0, 0);
            this.layoutRaw.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.layoutRaw.Name = "layoutRaw";
            this.layoutRaw.RowCount = 2;
            this.layoutRaw.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutRaw.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
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
            // scandataListView
            // 
            this.scandataListView.AllowUserToAddRows = false;
            this.scandataListView.AllowUserToDeleteRows = false;
            this.scandataListView.AllowUserToResizeColumns = false;
            this.scandataListView.AllowUserToResizeRows = false;
            this.scandataListView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.scandataListView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.scandataListView.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.scandataListView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.scandataListView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.scandataListView.DefaultCellStyle = dataGridViewCellStyle2;
            this.scandataListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scandataListView.Location = new System.Drawing.Point(0, 0);
            this.scandataListView.Margin = new System.Windows.Forms.Padding(0);
            this.scandataListView.Name = "scandataListView";
            this.scandataListView.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.scandataListView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.scandataListView.RowHeadersVisible = false;
            this.layoutRaw.SetRowSpan(this.scandataListView, 2);
            this.scandataListView.RowTemplate.Height = 23;
            this.scandataListView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.scandataListView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.scandataListView.Size = new System.Drawing.Size(150, 389);
            this.scandataListView.TabIndex = 0;
            this.scandataListView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.scandataListView_CellClick);
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
            this.layoutRaw.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.settingTabControl)).EndInit();
            this.settingTabControl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scandataListView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageZeroing;
        private System.Windows.Forms.TableLayoutPanel layoutRaw;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage2;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl settingTabControl;
        private System.Windows.Forms.DataGridView scandataListView;
    }
}
