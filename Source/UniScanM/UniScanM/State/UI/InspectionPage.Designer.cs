namespace UniScanM.UI
{
    partial class InspectionPage
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InspectionPage));
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.result = new System.Windows.Forms.Label();
            this.panelInspectRight = new System.Windows.Forms.Panel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.panelInspectLeft = new System.Windows.Forms.Panel();
            this.panelStatusTop = new System.Windows.Forms.Panel();
            this.statusTable = new System.Windows.Forms.TableLayoutPanel();
            this.labelPVPos = new System.Windows.Forms.Label();
            this.labelResult = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.pVPos = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.panelSideButton = new System.Windows.Forms.Panel();
            this.buttonManStop = new Infragistics.Win.Misc.UltraButton();
            this.buttonManStart = new Infragistics.Win.Misc.UltraButton();
            this.buttonAutoManSwitch = new Infragistics.Win.Misc.UltraButton();
            this.appStylistRuntime1 = new Infragistics.Win.AppStyling.Runtime.AppStylistRuntime(this.components);
            this.appStylistRuntime2 = new Infragistics.Win.AppStyling.Runtime.AppStylistRuntime(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panelStatusTop.SuspendLayout();
            this.statusTable.SuspendLayout();
            this.panelSideButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // result
            // 
            this.result.BackColor = System.Drawing.Color.SkyBlue;
            this.result.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.result.Font = new System.Drawing.Font("맑은 고딕", 27F, System.Drawing.FontStyle.Bold);
            this.result.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.result.Location = new System.Drawing.Point(169, 54);
            this.result.Margin = new System.Windows.Forms.Padding(0);
            this.result.Name = "result";
            this.result.Size = new System.Drawing.Size(169, 54);
            this.result.TabIndex = 3;
            this.result.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelInspectRight
            // 
            this.panelInspectRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInspectRight.Location = new System.Drawing.Point(0, 162);
            this.panelInspectRight.Margin = new System.Windows.Forms.Padding(0);
            this.panelInspectRight.Name = "panelInspectRight";
            this.panelInspectRight.Size = new System.Drawing.Size(338, 285);
            this.panelInspectRight.TabIndex = 4;
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.panelInspectLeft);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.panelInspectRight);
            this.splitContainer.Panel2.Controls.Add(this.panelStatusTop);
            this.splitContainer.Size = new System.Drawing.Size(924, 449);
            this.splitContainer.SplitterDistance = 580;
            this.splitContainer.TabIndex = 0;
            // 
            // panelInspectLeft
            // 
            this.panelInspectLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInspectLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInspectLeft.Location = new System.Drawing.Point(0, 0);
            this.panelInspectLeft.Margin = new System.Windows.Forms.Padding(0);
            this.panelInspectLeft.Name = "panelInspectLeft";
            this.panelInspectLeft.Size = new System.Drawing.Size(578, 447);
            this.panelInspectLeft.TabIndex = 5;
            // 
            // panelStatusTop
            // 
            this.panelStatusTop.Controls.Add(this.statusTable);
            this.panelStatusTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStatusTop.Location = new System.Drawing.Point(0, 0);
            this.panelStatusTop.Margin = new System.Windows.Forms.Padding(0);
            this.panelStatusTop.Name = "panelStatusTop";
            this.panelStatusTop.Size = new System.Drawing.Size(338, 162);
            this.panelStatusTop.TabIndex = 5;
            // 
            // statusTable
            // 
            this.statusTable.ColumnCount = 2;
            this.statusTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.statusTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.statusTable.Controls.Add(this.labelPVPos, 0, 2);
            this.statusTable.Controls.Add(this.labelResult, 0, 1);
            this.statusTable.Controls.Add(this.labelStatus, 0, 0);
            this.statusTable.Controls.Add(this.pVPos, 1, 2);
            this.statusTable.Controls.Add(this.result, 1, 1);
            this.statusTable.Controls.Add(this.status, 1, 0);
            this.statusTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusTable.Location = new System.Drawing.Point(0, 0);
            this.statusTable.Name = "statusTable";
            this.statusTable.RowCount = 3;
            this.statusTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.statusTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.statusTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.statusTable.Size = new System.Drawing.Size(338, 162);
            this.statusTable.TabIndex = 0;
            // 
            // labelPVPos
            // 
            this.labelPVPos.BackColor = System.Drawing.Color.MidnightBlue;
            this.labelPVPos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelPVPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPVPos.Font = new System.Drawing.Font("맑은 고딕", 27F, System.Drawing.FontStyle.Bold);
            this.labelPVPos.ForeColor = System.Drawing.Color.White;
            this.labelPVPos.Location = new System.Drawing.Point(0, 108);
            this.labelPVPos.Margin = new System.Windows.Forms.Padding(0);
            this.labelPVPos.Name = "labelPVPos";
            this.labelPVPos.Size = new System.Drawing.Size(169, 54);
            this.labelPVPos.TabIndex = 7;
            this.labelPVPos.Text = "Length";
            this.labelPVPos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelResult
            // 
            this.labelResult.BackColor = System.Drawing.Color.SkyBlue;
            this.labelResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelResult.Font = new System.Drawing.Font("맑은 고딕", 27F, System.Drawing.FontStyle.Bold);
            this.labelResult.ForeColor = System.Drawing.Color.White;
            this.labelResult.Location = new System.Drawing.Point(0, 54);
            this.labelResult.Margin = new System.Windows.Forms.Padding(0);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(169, 54);
            this.labelResult.TabIndex = 6;
            this.labelResult.Text = "RESULT";
            this.labelResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelStatus
            // 
            this.labelStatus.BackColor = System.Drawing.Color.CornflowerBlue;
            this.labelStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStatus.Font = new System.Drawing.Font("맑은 고딕", 27F, System.Drawing.FontStyle.Bold);
            this.labelStatus.ForeColor = System.Drawing.Color.White;
            this.labelStatus.Location = new System.Drawing.Point(0, 0);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(0);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(169, 54);
            this.labelStatus.TabIndex = 5;
            this.labelStatus.Text = "STATUS";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pVPos
            // 
            this.pVPos.BackColor = System.Drawing.Color.MidnightBlue;
            this.pVPos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pVPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pVPos.Font = new System.Drawing.Font("맑은 고딕", 27F, System.Drawing.FontStyle.Bold);
            this.pVPos.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.pVPos.Location = new System.Drawing.Point(169, 108);
            this.pVPos.Margin = new System.Windows.Forms.Padding(0);
            this.pVPos.Name = "pVPos";
            this.pVPos.Size = new System.Drawing.Size(169, 54);
            this.pVPos.TabIndex = 4;
            this.pVPos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // status
            // 
            this.status.BackColor = System.Drawing.Color.CornflowerBlue;
            this.status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.status.Dock = System.Windows.Forms.DockStyle.Fill;
            this.status.Font = new System.Drawing.Font("맑은 고딕", 27F, System.Drawing.FontStyle.Bold);
            this.status.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.status.Location = new System.Drawing.Point(169, 0);
            this.status.Margin = new System.Windows.Forms.Padding(0);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(169, 54);
            this.status.TabIndex = 2;
            this.status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelSideButton
            // 
            this.panelSideButton.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelSideButton.Controls.Add(this.buttonManStop);
            this.panelSideButton.Controls.Add(this.buttonManStart);
            this.panelSideButton.Controls.Add(this.buttonAutoManSwitch);
            this.panelSideButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSideButton.Location = new System.Drawing.Point(924, 0);
            this.panelSideButton.Name = "panelSideButton";
            this.panelSideButton.Size = new System.Drawing.Size(80, 449);
            this.panelSideButton.TabIndex = 15;
            // 
            // buttonManStop
            // 
            appearance1.BackColor = System.Drawing.Color.Transparent;
            appearance1.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance1.FontData.BoldAsString = "True";
            appearance1.FontData.Name = "맑은 고딕";
            appearance1.FontData.SizeInPoints = 12F;
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Bottom";
            appearance1.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonManStop.Appearance = appearance1;
            this.buttonManStop.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonManStop.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonManStop.ImageSize = new System.Drawing.Size(60, 60);
            this.buttonManStop.Location = new System.Drawing.Point(0, 152);
            this.buttonManStop.Margin = new System.Windows.Forms.Padding(0);
            this.buttonManStop.Name = "buttonManStop";
            this.buttonManStop.Size = new System.Drawing.Size(76, 76);
            this.buttonManStop.TabIndex = 20;
            this.buttonManStop.Click += new System.EventHandler(this.buttonAutoManSwitch_Click);
            // 
            // buttonManStart
            // 
            appearance2.BackColor = System.Drawing.Color.Transparent;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.FontData.BoldAsString = "True";
            appearance2.FontData.Name = "맑은 고딕";
            appearance2.FontData.SizeInPoints = 12F;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Bottom";
            appearance2.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonManStart.Appearance = appearance2;
            this.buttonManStart.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonManStart.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonManStart.ImageSize = new System.Drawing.Size(60, 60);
            this.buttonManStart.Location = new System.Drawing.Point(0, 76);
            this.buttonManStart.Margin = new System.Windows.Forms.Padding(0);
            this.buttonManStart.Name = "buttonManStart";
            this.buttonManStart.Size = new System.Drawing.Size(76, 76);
            this.buttonManStart.TabIndex = 19;
            this.buttonManStart.Click += new System.EventHandler(this.buttonAutoManSwitch_Click);
            // 
            // buttonAutoManSwitch
            // 
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance3.FontData.BoldAsString = "True";
            appearance3.FontData.Name = "맑은 고딕";
            appearance3.FontData.SizeInPoints = 12F;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            appearance3.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance3.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Bottom";
            appearance3.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonAutoManSwitch.Appearance = appearance3;
            this.buttonAutoManSwitch.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonAutoManSwitch.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonAutoManSwitch.ImageSize = new System.Drawing.Size(60, 60);
            this.buttonAutoManSwitch.Location = new System.Drawing.Point(0, 0);
            this.buttonAutoManSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAutoManSwitch.Name = "buttonAutoManSwitch";
            this.buttonAutoManSwitch.Size = new System.Drawing.Size(76, 76);
            this.buttonAutoManSwitch.TabIndex = 18;
            this.buttonAutoManSwitch.Click += new System.EventHandler(this.buttonAutoManSwitch_Click);
            // 
            // InspectionPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.panelSideButton);
            this.DoubleBuffered = true;
            this.Name = "InspectionPage";
            this.Size = new System.Drawing.Size(1004, 449);
            this.Load += new System.EventHandler(this.MonitoringPage_Load);
            this.Resize += new System.EventHandler(this.InspectionPage_Resize);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panelStatusTop.ResumeLayout(false);
            this.statusTable.ResumeLayout(false);
            this.panelSideButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label result;
        private System.Windows.Forms.Panel panelInspectRight;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Label pVPos;
        private System.Windows.Forms.Panel panelInspectLeft;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Panel panelSideButton;
        private Infragistics.Win.Misc.UltraButton buttonManStop;
        private Infragistics.Win.Misc.UltraButton buttonManStart;
        public Infragistics.Win.Misc.UltraButton buttonAutoManSwitch;
        private System.Windows.Forms.Panel panelStatusTop;
        private System.Windows.Forms.TableLayoutPanel statusTable;
        private System.Windows.Forms.Label labelPVPos;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Label labelStatus;
        private Infragistics.Win.AppStyling.Runtime.AppStylistRuntime appStylistRuntime1;
        private Infragistics.Win.AppStyling.Runtime.AppStylistRuntime appStylistRuntime2;
    }
}
