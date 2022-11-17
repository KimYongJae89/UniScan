namespace UniScanG.Module.Controller.UI
{
    partial class MonitorMainform
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonitorMainform));
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.panelTop = new System.Windows.Forms.TableLayoutPanel();
            this.panelTopTime = new System.Windows.Forms.TableLayoutPanel();
            this.dateLabel = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.panelTopMode = new System.Windows.Forms.Panel();
            this.panelTopCenter = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelTopLogo = new System.Windows.Forms.PictureBox();
            this.panelTopLogo2 = new System.Windows.Forms.Panel();
            this.panelTopUser = new System.Windows.Forms.Panel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.stripPanelRight = new System.Windows.Forms.Panel();
            this.panelBottom = new System.Windows.Forms.TableLayoutPanel();
            this.panelBottomCenter = new System.Windows.Forms.Panel();
            this.labelBottomCenter = new System.Windows.Forms.Label();
            this.stripPanelLeft = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.panelTopTime.SuspendLayout();
            this.panelTopCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelTopLogo)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.panelBottomCenter.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // panelTop
            // 
            this.panelTop.BackgroundImage = global::UniScanG.Properties.Resources.title_dummy;
            this.panelTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelTop.ColumnCount = 8;
            this.panelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280F));
            this.panelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.panelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.panelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 700F));
            this.panelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.panelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.panelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.panelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 108F));
            this.panelTop.Controls.Add(this.panelTopTime, 1, 0);
            this.panelTop.Controls.Add(this.panelTopMode, 6, 0);
            this.panelTop.Controls.Add(this.panelTopCenter, 3, 0);
            this.panelTop.Controls.Add(this.panelTopLogo, 0, 0);
            this.panelTop.Controls.Add(this.panelTopLogo2, 7, 0);
            this.panelTop.Controls.Add(this.panelTopUser, 5, 0);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(0);
            this.panelTop.Name = "panelTop";
            this.panelTop.RowCount = 1;
            this.panelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelTop.Size = new System.Drawing.Size(1908, 75);
            this.panelTop.TabIndex = 0;
            // 
            // panelTopTime
            // 
            this.panelTopTime.BackColor = System.Drawing.Color.Transparent;
            this.panelTopTime.ColumnCount = 1;
            this.panelTopTime.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelTopTime.Controls.Add(this.dateLabel, 0, 0);
            this.panelTopTime.Controls.Add(this.timeLabel, 0, 1);
            this.panelTopTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTopTime.Location = new System.Drawing.Point(280, 0);
            this.panelTopTime.Margin = new System.Windows.Forms.Padding(0);
            this.panelTopTime.Name = "panelTopTime";
            this.panelTopTime.Padding = new System.Windows.Forms.Padding(13, 14, 13, 14);
            this.panelTopTime.RowCount = 2;
            this.panelTopTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelTopTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelTopTime.Size = new System.Drawing.Size(170, 75);
            this.panelTopTime.TabIndex = 0;
            // 
            // dateLabel
            // 
            this.dateLabel.BackColor = System.Drawing.Color.Transparent;
            this.dateLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateLabel.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dateLabel.Location = new System.Drawing.Point(13, 14);
            this.dateLabel.Margin = new System.Windows.Forms.Padding(0);
            this.dateLabel.Name = "dateLabel";
            this.dateLabel.Size = new System.Drawing.Size(144, 23);
            this.dateLabel.TabIndex = 0;
            this.dateLabel.Text = "yyyy - MM - dd";
            this.dateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timeLabel
            // 
            this.timeLabel.BackColor = System.Drawing.Color.Transparent;
            this.timeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeLabel.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.timeLabel.Location = new System.Drawing.Point(13, 37);
            this.timeLabel.Margin = new System.Windows.Forms.Padding(0);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(144, 24);
            this.timeLabel.TabIndex = 0;
            this.timeLabel.Text = "HH : mm : ss";
            this.timeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTopMode
            // 
            this.panelTopMode.BackColor = System.Drawing.Color.Transparent;
            this.panelTopMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTopMode.Location = new System.Drawing.Point(1500, 0);
            this.panelTopMode.Margin = new System.Windows.Forms.Padding(0);
            this.panelTopMode.Name = "panelTopMode";
            this.panelTopMode.Size = new System.Drawing.Size(300, 75);
            this.panelTopMode.TabIndex = 2;
            // 
            // panelTopCenter
            // 
            this.panelTopCenter.BackgroundImage = global::UniScanG.Properties.Resources.title_bar2;
            this.panelTopCenter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelTopCenter.Controls.Add(this.labelTitle);
            this.panelTopCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTopCenter.Location = new System.Drawing.Point(555, 0);
            this.panelTopCenter.Margin = new System.Windows.Forms.Padding(0);
            this.panelTopCenter.Name = "panelTopCenter";
            this.panelTopCenter.Size = new System.Drawing.Size(700, 75);
            this.panelTopCenter.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("Malgun Gothic", 26F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.Black;
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(700, 75);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelTopLogo
            // 
            this.panelTopLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTopLogo.Image = global::UniScanG.Properties.Resources.samsung_logo;
            this.panelTopLogo.Location = new System.Drawing.Point(0, 0);
            this.panelTopLogo.Margin = new System.Windows.Forms.Padding(0);
            this.panelTopLogo.Name = "panelTopLogo";
            this.panelTopLogo.Size = new System.Drawing.Size(280, 75);
            this.panelTopLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.panelTopLogo.TabIndex = 0;
            this.panelTopLogo.TabStop = false;
            this.panelTopLogo.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTopLogo_Paint);
            // 
            // panelTopLogo2
            // 
            this.panelTopLogo2.BackgroundImage = global::UniScanG.Properties.Resources.GTC_Edit;
            this.panelTopLogo2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelTopLogo2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTopLogo2.Location = new System.Drawing.Point(1806, 7);
            this.panelTopLogo2.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.panelTopLogo2.Name = "panelTopLogo2";
            this.panelTopLogo2.Size = new System.Drawing.Size(96, 61);
            this.panelTopLogo2.TabIndex = 0;
            this.panelTopLogo2.Click += new System.EventHandler(this.gtcLogoPanel_Click);
            // 
            // panelTopUser
            // 
            this.panelTopUser.BackColor = System.Drawing.Color.Transparent;
            this.panelTopUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTopUser.Location = new System.Drawing.Point(1300, 0);
            this.panelTopUser.Margin = new System.Windows.Forms.Padding(0);
            this.panelTopUser.Name = "panelTopUser";
            this.panelTopUser.Size = new System.Drawing.Size(200, 75);
            this.panelTopUser.TabIndex = 1;
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 75);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1908, 940);
            this.mainPanel.TabIndex = 3;
            // 
            // stripPanelRight
            // 
            this.stripPanelRight.BackColor = System.Drawing.Color.Transparent;
            this.stripPanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stripPanelRight.Location = new System.Drawing.Point(1104, 0);
            this.stripPanelRight.Margin = new System.Windows.Forms.Padding(0);
            this.stripPanelRight.Name = "stripPanelRight";
            this.stripPanelRight.Size = new System.Drawing.Size(804, 30);
            this.stripPanelRight.TabIndex = 4;
            // 
            // panelBottom
            // 
            this.panelBottom.BackgroundImage = global::UniScanG.Properties.Resources.title_dummy_Invert;
            this.panelBottom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBottom.ColumnCount = 3;
            this.panelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.panelBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelBottom.Controls.Add(this.panelBottomCenter, 1, 0);
            this.panelBottom.Controls.Add(this.stripPanelLeft, 0, 0);
            this.panelBottom.Controls.Add(this.stripPanelRight, 2, 0);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 1015);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.RowCount = 1;
            this.panelBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelBottom.Size = new System.Drawing.Size(1908, 30);
            this.panelBottom.TabIndex = 6;
            // 
            // panelBottomCenter
            // 
            this.panelBottomCenter.BackgroundImage = global::UniScanG.Properties.Resources.title_bar2_Invert;
            this.panelBottomCenter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelBottomCenter.Controls.Add(this.labelBottomCenter);
            this.panelBottomCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottomCenter.Location = new System.Drawing.Point(807, 3);
            this.panelBottomCenter.Name = "panelBottomCenter";
            this.panelBottomCenter.Size = new System.Drawing.Size(294, 24);
            this.panelBottomCenter.TabIndex = 0;
            // 
            // labelBottomCenter
            // 
            this.labelBottomCenter.BackColor = System.Drawing.Color.Transparent;
            this.labelBottomCenter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.labelBottomCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBottomCenter.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.labelBottomCenter.Location = new System.Drawing.Point(0, 0);
            this.labelBottomCenter.Margin = new System.Windows.Forms.Padding(0);
            this.labelBottomCenter.Name = "labelBottomCenter";
            this.labelBottomCenter.Size = new System.Drawing.Size(294, 24);
            this.labelBottomCenter.TabIndex = 5;
            this.labelBottomCenter.Text = "S  A  M  S  U  N  G";
            this.labelBottomCenter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stripPanelLeft
            // 
            this.stripPanelLeft.BackColor = System.Drawing.Color.Transparent;
            this.stripPanelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stripPanelLeft.Location = new System.Drawing.Point(0, 0);
            this.stripPanelLeft.Margin = new System.Windows.Forms.Padding(0);
            this.stripPanelLeft.Name = "stripPanelLeft";
            this.stripPanelLeft.Size = new System.Drawing.Size(804, 30);
            this.stripPanelLeft.TabIndex = 4;
            // 
            // MonitorMainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.ClientSize = new System.Drawing.Size(1908, 1045);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelBottom);
            this.Font = new System.Drawing.Font("Malgun Gothic", 12F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "MonitorMainform";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MonitorMainform_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MonitorMainform_SizeChanged);
            this.panelTop.ResumeLayout(false);
            this.panelTopTime.ResumeLayout(false);
            this.panelTopCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelTopLogo)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.panelBottomCenter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.TableLayoutPanel panelTop;
        private System.Windows.Forms.PictureBox panelTopLogo;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Panel panelTopLogo2;
        private System.Windows.Forms.TableLayoutPanel panelTopTime;
        private System.Windows.Forms.Panel panelTopUser;
        private System.Windows.Forms.Panel panelTopMode;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel panelTopCenter;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel stripPanelRight;
        private System.Windows.Forms.Label labelBottomCenter;
        private System.Windows.Forms.TableLayoutPanel panelBottom;
        private System.Windows.Forms.Panel panelBottomCenter;
        private System.Windows.Forms.Panel stripPanelLeft;
    }
}