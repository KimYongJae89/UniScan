namespace UniScanM
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.panelBody = new System.Windows.Forms.Panel();
            this.imageListBottom = new System.Windows.Forms.ImageList(this.components);
            this.imageListSide = new System.Windows.Forms.ImageList(this.components);
            this.panelHeader = new System.Windows.Forms.Panel();
            this.tableLayoutPanelHeader = new System.Windows.Forms.TableLayoutPanel();
            this.pictureCompanyLogo = new System.Windows.Forms.PictureBox();
            this.panelClock = new System.Windows.Forms.Panel();
            this.panelMHeader = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelInfoHeader = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.buttonReport = new Infragistics.Win.Misc.UltraButton();
            this.buttonTeach = new Infragistics.Win.Misc.UltraButton();
            this.buttonInspection = new Infragistics.Win.Misc.UltraButton();
            this.buttonModelManager = new Infragistics.Win.Misc.UltraButton();
            this.buttonSetting = new Infragistics.Win.Misc.UltraButton();
            this.buttonExit = new Infragistics.Win.Misc.UltraButton();
            this.panelPLCStatus = new System.Windows.Forms.Panel();
            this.panelCompanyLogo = new System.Windows.Forms.Panel();
            this.tableLayoutPanelLHeader = new System.Windows.Forms.TableLayoutPanel();
            this.panelHeader.SuspendLayout();
            this.tableLayoutPanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureCompanyLogo)).BeginInit();
            this.panelMHeader.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelCompanyLogo.SuspendLayout();
            this.tableLayoutPanelLHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBody
            // 
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 76);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(1575, 490);
            this.panelBody.TabIndex = 2;
            // 
            // imageListBottom
            // 
            this.imageListBottom.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListBottom.ImageStream")));
            this.imageListBottom.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListBottom.Images.SetKeyName(0, "Model.png");
            this.imageListBottom.Images.SetKeyName(1, "Monitoring.png");
            this.imageListBottom.Images.SetKeyName(2, "Teach.png");
            this.imageListBottom.Images.SetKeyName(3, "Report2.png");
            this.imageListBottom.Images.SetKeyName(4, "Setting.png");
            this.imageListBottom.Images.SetKeyName(5, "Exit2.png");
            // 
            // imageListSide
            // 
            this.imageListSide.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSide.ImageStream")));
            this.imageListSide.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSide.Images.SetKeyName(0, "Auto2.png");
            this.imageListSide.Images.SetKeyName(1, "st3.png");
            this.imageListSide.Images.SetKeyName(2, "Stop2.png");
            this.imageListSide.Images.SetKeyName(3, "Round.png");
            // 
            // panelHeader
            // 
            this.panelHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelHeader.BackgroundImage")));
            this.panelHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelHeader.Controls.Add(this.tableLayoutPanelHeader);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1575, 76);
            this.panelHeader.TabIndex = 0;
            // 
            // tableLayoutPanelHeader
            // 
            this.tableLayoutPanelHeader.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelHeader.ColumnCount = 3;
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanelHeader.Controls.Add(this.tableLayoutPanelLHeader, 0, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.panelMHeader, 1, 0);
            this.tableLayoutPanelHeader.Controls.Add(this.panelInfoHeader, 2, 0);
            this.tableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelHeader.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelHeader.Name = "tableLayoutPanelHeader";
            this.tableLayoutPanelHeader.RowCount = 1;
            this.tableLayoutPanelHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelHeader.Size = new System.Drawing.Size(1575, 76);
            this.tableLayoutPanelHeader.TabIndex = 2;
            // 
            // pictureCompanyLogo
            // 
            this.pictureCompanyLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureCompanyLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureCompanyLogo.Image = global::UniScanM.Properties.Resources.samsung2;
            this.pictureCompanyLogo.Location = new System.Drawing.Point(0, 0);
            this.pictureCompanyLogo.Margin = new System.Windows.Forms.Padding(0);
            this.pictureCompanyLogo.Name = "pictureCompanyLogo";
            this.pictureCompanyLogo.Size = new System.Drawing.Size(315, 76);
            this.pictureCompanyLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureCompanyLogo.TabIndex = 3;
            this.pictureCompanyLogo.TabStop = false;
            this.pictureCompanyLogo.WaitOnLoad = true;
            // 
            // panelClock
            // 
            this.panelClock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelClock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelClock.Location = new System.Drawing.Point(315, 0);
            this.panelClock.Margin = new System.Windows.Forms.Padding(0);
            this.panelClock.Name = "panelClock";
            this.panelClock.Size = new System.Drawing.Size(210, 76);
            this.panelClock.TabIndex = 2;
            // 
            // panelMHeader
            // 
            this.panelMHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelMHeader.Controls.Add(this.labelTitle);
            this.panelMHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMHeader.Location = new System.Drawing.Point(525, 0);
            this.panelMHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelMHeader.Name = "panelMHeader";
            this.panelMHeader.Size = new System.Drawing.Size(525, 76);
            this.panelMHeader.TabIndex = 3;
            // 
            // labelTitle
            // 
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("맑은 고딕", 33F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTitle.ForeColor = System.Drawing.Color.DarkBlue;
            this.labelTitle.Image = ((System.Drawing.Image)(resources.GetObject("labelTitle.Image")));
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(525, 76);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelInfoHeader
            // 
            this.panelInfoHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInfoHeader.Location = new System.Drawing.Point(1050, 0);
            this.panelInfoHeader.Margin = new System.Windows.Forms.Padding(0);
            this.panelInfoHeader.Name = "panelInfoHeader";
            this.panelInfoHeader.Size = new System.Drawing.Size(525, 76);
            this.panelInfoHeader.TabIndex = 4;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.Transparent;
            this.panelBottom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelBottom.BackgroundImage")));
            this.panelBottom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelBottom.Controls.Add(this.buttonReport);
            this.panelBottom.Controls.Add(this.buttonTeach);
            this.panelBottom.Controls.Add(this.buttonInspection);
            this.panelBottom.Controls.Add(this.buttonModelManager);
            this.panelBottom.Controls.Add(this.buttonSetting);
            this.panelBottom.Controls.Add(this.buttonExit);
            this.panelBottom.Controls.Add(this.panelPLCStatus);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 566);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1575, 114);
            this.panelBottom.TabIndex = 13;
            // 
            // buttonReport
            // 
            appearance7.BackColor = System.Drawing.Color.Transparent;
            appearance7.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance7.FontData.BoldAsString = "True";
            appearance7.FontData.Name = "맑은 고딕";
            appearance7.FontData.SizeInPoints = 12F;
            appearance7.Image = ((object)(resources.GetObject("appearance7.Image")));
            appearance7.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance7.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance7.TextHAlignAsString = "Center";
            appearance7.TextVAlignAsString = "Bottom";
            appearance7.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonReport.Appearance = appearance7;
            this.buttonReport.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonReport.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonReport.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonReport.Location = new System.Drawing.Point(330, 0);
            this.buttonReport.Margin = new System.Windows.Forms.Padding(0);
            this.buttonReport.Name = "buttonReport";
            this.buttonReport.Size = new System.Drawing.Size(110, 80);
            this.buttonReport.TabIndex = 24;
            this.buttonReport.Text = "Report";
            this.buttonReport.Click += new System.EventHandler(this.PageButton_Click);
            // 
            // buttonTeach
            // 
            appearance8.BackColor = System.Drawing.Color.Transparent;
            appearance8.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance8.FontData.BoldAsString = "True";
            appearance8.FontData.Name = "맑은 고딕";
            appearance8.FontData.SizeInPoints = 12F;
            appearance8.Image = ((object)(resources.GetObject("appearance8.Image")));
            appearance8.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance8.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance8.TextHAlignAsString = "Center";
            appearance8.TextVAlignAsString = "Bottom";
            appearance8.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonTeach.Appearance = appearance8;
            this.buttonTeach.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonTeach.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonTeach.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonTeach.Location = new System.Drawing.Point(220, 0);
            this.buttonTeach.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTeach.Name = "buttonTeach";
            this.buttonTeach.Size = new System.Drawing.Size(110, 80);
            this.buttonTeach.TabIndex = 23;
            this.buttonTeach.Text = "Teach";
            this.buttonTeach.Click += new System.EventHandler(this.PageButton_Click);
            // 
            // buttonInspection
            // 
            appearance9.BackColor = System.Drawing.Color.Transparent;
            appearance9.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance9.FontData.BoldAsString = "True";
            appearance9.FontData.Name = "맑은 고딕";
            appearance9.FontData.SizeInPoints = 12F;
            appearance9.Image = ((object)(resources.GetObject("appearance9.Image")));
            appearance9.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance9.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance9.TextHAlignAsString = "Center";
            appearance9.TextVAlignAsString = "Bottom";
            appearance9.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonInspection.Appearance = appearance9;
            this.buttonInspection.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonInspection.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonInspection.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonInspection.Location = new System.Drawing.Point(110, 0);
            this.buttonInspection.Margin = new System.Windows.Forms.Padding(0);
            this.buttonInspection.Name = "buttonInspection";
            this.buttonInspection.Size = new System.Drawing.Size(110, 80);
            this.buttonInspection.TabIndex = 20;
            this.buttonInspection.Text = "Inspection";
            this.buttonInspection.Click += new System.EventHandler(this.PageButton_Click);
            // 
            // buttonModelManager
            // 
            appearance10.BackColor = System.Drawing.Color.Transparent;
            appearance10.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance10.FontData.BoldAsString = "True";
            appearance10.FontData.Name = "맑은 고딕";
            appearance10.FontData.SizeInPoints = 12F;
            appearance10.Image = ((object)(resources.GetObject("appearance10.Image")));
            appearance10.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance10.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance10.TextHAlignAsString = "Center";
            appearance10.TextVAlignAsString = "Bottom";
            appearance10.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonModelManager.Appearance = appearance10;
            this.buttonModelManager.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonModelManager.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonModelManager.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonModelManager.Location = new System.Drawing.Point(0, 0);
            this.buttonModelManager.Margin = new System.Windows.Forms.Padding(0);
            this.buttonModelManager.Name = "buttonModelManager";
            this.buttonModelManager.Size = new System.Drawing.Size(110, 80);
            this.buttonModelManager.TabIndex = 19;
            this.buttonModelManager.Text = "Model";
            this.buttonModelManager.Visible = false;
            this.buttonModelManager.Click += new System.EventHandler(this.PageButton_Click);
            // 
            // buttonSetting
            // 
            appearance11.BackColor = System.Drawing.Color.Transparent;
            appearance11.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance11.FontData.BoldAsString = "True";
            appearance11.FontData.Name = "맑은 고딕";
            appearance11.FontData.SizeInPoints = 12F;
            appearance11.Image = ((object)(resources.GetObject("appearance11.Image")));
            appearance11.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance11.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance11.TextHAlignAsString = "Center";
            appearance11.TextVAlignAsString = "Bottom";
            appearance11.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonSetting.Appearance = appearance11;
            this.buttonSetting.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonSetting.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonSetting.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonSetting.Location = new System.Drawing.Point(1351, 0);
            this.buttonSetting.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSetting.Name = "buttonSetting";
            this.buttonSetting.Size = new System.Drawing.Size(110, 80);
            this.buttonSetting.TabIndex = 22;
            this.buttonSetting.Text = "Setting";
            this.buttonSetting.Click += new System.EventHandler(this.PageButton_Click);
            // 
            // buttonExit
            // 
            appearance12.BackColor = System.Drawing.Color.Transparent;
            appearance12.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance12.FontData.BoldAsString = "True";
            appearance12.FontData.Name = "맑은 고딕";
            appearance12.FontData.SizeInPoints = 12F;
            appearance12.Image = ((object)(resources.GetObject("appearance12.Image")));
            appearance12.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance12.ImageVAlign = Infragistics.Win.VAlign.Top;
            appearance12.TextHAlignAsString = "Center";
            appearance12.TextVAlignAsString = "Bottom";
            appearance12.ThemedElementAlpha = Infragistics.Win.Alpha.Transparent;
            this.buttonExit.Appearance = appearance12;
            this.buttonExit.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonExit.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonExit.ImageSize = new System.Drawing.Size(45, 45);
            this.buttonExit.Location = new System.Drawing.Point(1461, 0);
            this.buttonExit.Margin = new System.Windows.Forms.Padding(0);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(110, 80);
            this.buttonExit.TabIndex = 21;
            this.buttonExit.Text = "Exit";
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // panelPLCStatus
            // 
            this.panelPLCStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelPLCStatus.Location = new System.Drawing.Point(0, 80);
            this.panelPLCStatus.Margin = new System.Windows.Forms.Padding(0);
            this.panelPLCStatus.Name = "panelPLCStatus";
            this.panelPLCStatus.Size = new System.Drawing.Size(1571, 30);
            this.panelPLCStatus.TabIndex = 18;
            // 
            // panelCompanyLogo
            // 
            this.panelCompanyLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelCompanyLogo.Controls.Add(this.pictureCompanyLogo);
            this.panelCompanyLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCompanyLogo.Location = new System.Drawing.Point(0, 0);
            this.panelCompanyLogo.Margin = new System.Windows.Forms.Padding(0);
            this.panelCompanyLogo.Name = "panelCompanyLogo";
            this.panelCompanyLogo.Size = new System.Drawing.Size(315, 76);
            this.panelCompanyLogo.TabIndex = 3;
            // 
            // tableLayoutPanelLHeader
            // 
            this.tableLayoutPanelLHeader.ColumnCount = 2;
            this.tableLayoutPanelLHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanelLHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelLHeader.Controls.Add(this.panelCompanyLogo, 0, 0);
            this.tableLayoutPanelLHeader.Controls.Add(this.panelClock, 1, 0);
            this.tableLayoutPanelLHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLHeader.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelLHeader.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelLHeader.Name = "tableLayoutPanelLHeader";
            this.tableLayoutPanelLHeader.RowCount = 1;
            this.tableLayoutPanelLHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLHeader.Size = new System.Drawing.Size(525, 76);
            this.tableLayoutPanelLHeader.TabIndex = 4;
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1575, 680);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResizeBegin += new System.EventHandler(this.MainForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.MainForm_ResizeEnd);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.panelHeader.ResumeLayout(false);
            this.tableLayoutPanelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureCompanyLogo)).EndInit();
            this.panelMHeader.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.panelCompanyLogo.ResumeLayout(false);
            this.tableLayoutPanelLHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelHeader;
        private System.Windows.Forms.Panel panelMHeader;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.ImageList imageListBottom;
        private System.Windows.Forms.ImageList imageListSide;
        private System.Windows.Forms.Panel panelInfoHeader;
        private System.Windows.Forms.Panel panelPLCStatus;
        private Infragistics.Win.Misc.UltraButton buttonSetting;
        private Infragistics.Win.Misc.UltraButton buttonExit;
        private Infragistics.Win.Misc.UltraButton buttonTeach;
        private Infragistics.Win.Misc.UltraButton buttonInspection;
        private Infragistics.Win.Misc.UltraButton buttonModelManager;
        private Infragistics.Win.Misc.UltraButton buttonReport;
        private System.Windows.Forms.Panel panelClock;
        private System.Windows.Forms.PictureBox pictureCompanyLogo;
        private System.Windows.Forms.Panel panelCompanyLogo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLHeader;
    }
}