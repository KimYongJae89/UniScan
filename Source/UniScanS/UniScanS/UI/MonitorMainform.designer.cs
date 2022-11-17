namespace UniScanS.UI
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
            this.topPanel = new System.Windows.Forms.Panel();
            this.topLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.layoutTime = new System.Windows.Forms.TableLayoutPanel();
            this.dateLabel = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.modelPanel = new System.Windows.Forms.Panel();
            this.titlePanel = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.samsungLogoPictureBox = new System.Windows.Forms.PictureBox();
            this.gtcLogoPanel = new System.Windows.Forms.Panel();
            this.userPanel = new System.Windows.Forms.Panel();
            this.layoutStatusStrip = new System.Windows.Forms.FlowLayoutPanel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.topPanel.SuspendLayout();
            this.topLayoutPanel.SuspendLayout();
            this.layoutTime.SuspendLayout();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samsungLogoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.topLayoutPanel);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1924, 75);
            this.topPanel.TabIndex = 1;
            // 
            // topLayoutPanel
            // 
            this.topLayoutPanel.BackgroundImage = global::UniScanS.Properties.Resources.title_dummy;
            this.topLayoutPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.topLayoutPanel.ColumnCount = 8;
            this.topLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280F));
            this.topLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 170F));
            this.topLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 87.5F));
            this.topLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 700F));
            this.topLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.topLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.topLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.topLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.topLayoutPanel.Controls.Add(this.layoutTime, 1, 0);
            this.topLayoutPanel.Controls.Add(this.modelPanel, 6, 0);
            this.topLayoutPanel.Controls.Add(this.titlePanel, 3, 0);
            this.topLayoutPanel.Controls.Add(this.samsungLogoPictureBox, 0, 0);
            this.topLayoutPanel.Controls.Add(this.gtcLogoPanel, 7, 0);
            this.topLayoutPanel.Controls.Add(this.userPanel, 5, 0);
            this.topLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.topLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.topLayoutPanel.Name = "topLayoutPanel";
            this.topLayoutPanel.RowCount = 1;
            this.topLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.topLayoutPanel.Size = new System.Drawing.Size(1924, 75);
            this.topLayoutPanel.TabIndex = 0;
            // 
            // layoutTime
            // 
            this.layoutTime.BackColor = System.Drawing.Color.Transparent;
            this.layoutTime.ColumnCount = 1;
            this.layoutTime.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutTime.Controls.Add(this.dateLabel, 0, 0);
            this.layoutTime.Controls.Add(this.timeLabel, 0, 1);
            this.layoutTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutTime.Location = new System.Drawing.Point(280, 0);
            this.layoutTime.Margin = new System.Windows.Forms.Padding(0);
            this.layoutTime.Name = "layoutTime";
            this.layoutTime.Padding = new System.Windows.Forms.Padding(13, 14, 13, 14);
            this.layoutTime.RowCount = 2;
            this.layoutTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutTime.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutTime.Size = new System.Drawing.Size(170, 75);
            this.layoutTime.TabIndex = 0;
            // 
            // dateLabel
            // 
            this.dateLabel.BackColor = System.Drawing.Color.Transparent;
            this.dateLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateLabel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            this.timeLabel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.timeLabel.Location = new System.Drawing.Point(13, 37);
            this.timeLabel.Margin = new System.Windows.Forms.Padding(0);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(144, 24);
            this.timeLabel.TabIndex = 0;
            this.timeLabel.Text = "HH : mm : ss";
            this.timeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // modelPanel
            // 
            this.modelPanel.BackColor = System.Drawing.Color.Transparent;
            this.modelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPanel.Location = new System.Drawing.Point(1521, 0);
            this.modelPanel.Margin = new System.Windows.Forms.Padding(0);
            this.modelPanel.Name = "modelPanel";
            this.modelPanel.Size = new System.Drawing.Size(300, 75);
            this.modelPanel.TabIndex = 2;
            // 
            // titlePanel
            // 
            this.titlePanel.BackgroundImage = global::UniScanS.Properties.Resources.title_bar2;
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.titlePanel.Controls.Add(this.labelTitle);
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titlePanel.Location = new System.Drawing.Point(600, 0);
            this.titlePanel.Margin = new System.Windows.Forms.Padding(0);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(700, 75);
            this.titlePanel.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("맑은 고딕", 26F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.Black;
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(700, 75);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "MLCC Print Scan";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // samsungLogoPictureBox
            // 
            this.samsungLogoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.samsungLogoPictureBox.Image = global::UniScanS.Properties.Resources.samsung_logo;
            this.samsungLogoPictureBox.Location = new System.Drawing.Point(0, 0);
            this.samsungLogoPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.samsungLogoPictureBox.Name = "samsungLogoPictureBox";
            this.samsungLogoPictureBox.Size = new System.Drawing.Size(280, 75);
            this.samsungLogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.samsungLogoPictureBox.TabIndex = 0;
            this.samsungLogoPictureBox.TabStop = false;
            // 
            // gtcLogoPanel
            // 
            this.gtcLogoPanel.BackgroundImage = global::UniScanS.Properties.Resources.GTC_Edit;
            this.gtcLogoPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.gtcLogoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gtcLogoPanel.Location = new System.Drawing.Point(1827, 7);
            this.gtcLogoPanel.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.gtcLogoPanel.Name = "gtcLogoPanel";
            this.gtcLogoPanel.Size = new System.Drawing.Size(91, 61);
            this.gtcLogoPanel.TabIndex = 0;
            // 
            // userPanel
            // 
            this.userPanel.BackColor = System.Drawing.Color.Transparent;
            this.userPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userPanel.Location = new System.Drawing.Point(1321, 0);
            this.userPanel.Margin = new System.Windows.Forms.Padding(0);
            this.userPanel.Name = "userPanel";
            this.userPanel.Size = new System.Drawing.Size(200, 75);
            this.userPanel.TabIndex = 1;
            // 
            // layoutStatusStrip
            // 
            this.layoutStatusStrip.AutoSize = true;
            this.layoutStatusStrip.BackColor = System.Drawing.Color.White;
            this.layoutStatusStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.layoutStatusStrip.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.layoutStatusStrip.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.layoutStatusStrip.Location = new System.Drawing.Point(0, 1061);
            this.layoutStatusStrip.Margin = new System.Windows.Forms.Padding(0);
            this.layoutStatusStrip.Name = "layoutStatusStrip";
            this.layoutStatusStrip.Size = new System.Drawing.Size(1924, 0);
            this.layoutStatusStrip.TabIndex = 2;
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 75);
            this.mainPanel.Margin = new System.Windows.Forms.Padding(0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1924, 986);
            this.mainPanel.TabIndex = 3;
            // 
            // MonitorMainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.ClientSize = new System.Drawing.Size(1924, 1061);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.layoutStatusStrip);
            this.Controls.Add(this.topPanel);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "MonitorMainform";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.VisibleChanged += new System.EventHandler(this.MainForm_VisibleChanged);
            this.topPanel.ResumeLayout(false);
            this.topLayoutPanel.ResumeLayout(false);
            this.layoutTime.ResumeLayout(false);
            this.titlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.samsungLogoPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.TableLayoutPanel topLayoutPanel;
        private System.Windows.Forms.PictureBox samsungLogoPictureBox;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label dateLabel;
        private System.Windows.Forms.Panel gtcLogoPanel;
        private System.Windows.Forms.TableLayoutPanel layoutTime;
        private System.Windows.Forms.Panel userPanel;
        private System.Windows.Forms.Panel modelPanel;
        private System.Windows.Forms.FlowLayoutPanel layoutStatusStrip;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Panel titlePanel;
        private System.Windows.Forms.Label labelTitle;
    }
}