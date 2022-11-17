namespace UniScanX.MPAlignment
{
    partial class MainForm
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.btnLogs = new System.Windows.Forms.Button();
            this.btnUserConfig = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnTeach = new System.Windows.Forms.Button();
            this.btnInspect = new System.Windows.Forms.Button();
            this.btnProductManager = new System.Windows.Forms.Button();
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.picCompanyLogo = new System.Windows.Forms.PictureBox();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnMaximize = new System.Windows.Forms.Button();
            this.lblSelectedProduct = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnIoMonitor = new System.Windows.Forms.Button();
            this.pnlLeft.SuspendLayout();
            this.pnlMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCompanyLogo)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(90)))));
            this.pnlLeft.Controls.Add(this.btnLogs);
            this.pnlLeft.Controls.Add(this.btnUserConfig);
            this.pnlLeft.Controls.Add(this.btnReport);
            this.pnlLeft.Controls.Add(this.btnTeach);
            this.pnlLeft.Controls.Add(this.btnInspect);
            this.pnlLeft.Controls.Add(this.btnProductManager);
            this.pnlLeft.Controls.Add(this.pnlMenu);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(6);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(288, 939);
            this.pnlLeft.TabIndex = 2;
            // 
            // btnLogs
            // 
            this.btnLogs.AccessibleName = "Operation rate";
            this.btnLogs.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnLogs.FlatAppearance.BorderSize = 0;
            this.btnLogs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogs.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLogs.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnLogs.Image = global::UniScanX.MPAlignment.Properties.Resources.logs_48;
            this.btnLogs.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogs.Location = new System.Drawing.Point(0, 668);
            this.btnLogs.Margin = new System.Windows.Forms.Padding(0);
            this.btnLogs.Name = "btnLogs";
            this.btnLogs.Size = new System.Drawing.Size(288, 100);
            this.btnLogs.TabIndex = 14;
            this.btnLogs.Text = "Log";
            this.btnLogs.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLogs.UseVisualStyleBackColor = true;
            this.btnLogs.Click += new System.EventHandler(this.MenuButtonClicked);
            // 
            // btnUserConfig
            // 
            this.btnUserConfig.AccessibleName = "Operation rate";
            this.btnUserConfig.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUserConfig.FlatAppearance.BorderSize = 0;
            this.btnUserConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserConfig.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnUserConfig.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnUserConfig.Image = global::UniScanX.MPAlignment.Properties.Resources.report_48;
            this.btnUserConfig.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUserConfig.Location = new System.Drawing.Point(0, 568);
            this.btnUserConfig.Margin = new System.Windows.Forms.Padding(0);
            this.btnUserConfig.Name = "btnUserConfig";
            this.btnUserConfig.Size = new System.Drawing.Size(288, 100);
            this.btnUserConfig.TabIndex = 15;
            this.btnUserConfig.Text = "Setting";
            this.btnUserConfig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUserConfig.UseVisualStyleBackColor = true;
            this.btnUserConfig.Click += new System.EventHandler(this.MenuButtonClicked);
            // 
            // btnReport
            // 
            this.btnReport.AccessibleName = "Operation rate";
            this.btnReport.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnReport.FlatAppearance.BorderSize = 0;
            this.btnReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReport.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnReport.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnReport.Image = global::UniScanX.MPAlignment.Properties.Resources.report_48;
            this.btnReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnReport.Location = new System.Drawing.Point(0, 468);
            this.btnReport.Margin = new System.Windows.Forms.Padding(0);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(288, 100);
            this.btnReport.TabIndex = 13;
            this.btnReport.Text = "Report";
            this.btnReport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.MenuButtonClicked);
            // 
            // btnTeach
            // 
            this.btnTeach.AccessibleName = "Operation rate";
            this.btnTeach.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(90)))));
            this.btnTeach.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnTeach.Enabled = false;
            this.btnTeach.FlatAppearance.BorderSize = 0;
            this.btnTeach.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTeach.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnTeach.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnTeach.Image = global::UniScanX.MPAlignment.Properties.Resources.teach_48;
            this.btnTeach.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTeach.Location = new System.Drawing.Point(0, 368);
            this.btnTeach.Margin = new System.Windows.Forms.Padding(0);
            this.btnTeach.Name = "btnTeach";
            this.btnTeach.Size = new System.Drawing.Size(288, 100);
            this.btnTeach.TabIndex = 12;
            this.btnTeach.Text = "Teach";
            this.btnTeach.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTeach.UseVisualStyleBackColor = false;
            this.btnTeach.Click += new System.EventHandler(this.MenuButtonClicked);
            // 
            // btnInspect
            // 
            this.btnInspect.AccessibleName = "Inspect";
            this.btnInspect.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnInspect.Enabled = false;
            this.btnInspect.FlatAppearance.BorderSize = 0;
            this.btnInspect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInspect.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInspect.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnInspect.Image = global::UniScanX.MPAlignment.Properties.Resources.inspect_48;
            this.btnInspect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInspect.Location = new System.Drawing.Point(0, 268);
            this.btnInspect.Margin = new System.Windows.Forms.Padding(0);
            this.btnInspect.Name = "btnInspect";
            this.btnInspect.Size = new System.Drawing.Size(288, 100);
            this.btnInspect.TabIndex = 11;
            this.btnInspect.Text = "Inspect";
            this.btnInspect.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnInspect.UseVisualStyleBackColor = true;
            this.btnInspect.Click += new System.EventHandler(this.MenuButtonClicked);
            // 
            // btnProductManager
            // 
            this.btnProductManager.AccessibleName = "Operation rate";
            this.btnProductManager.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnProductManager.FlatAppearance.BorderSize = 0;
            this.btnProductManager.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProductManager.Font = new System.Drawing.Font("맑은 고딕", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnProductManager.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnProductManager.Image = global::UniScanX.MPAlignment.Properties.Resources.productmanage_48;
            this.btnProductManager.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProductManager.Location = new System.Drawing.Point(0, 168);
            this.btnProductManager.Margin = new System.Windows.Forms.Padding(0);
            this.btnProductManager.Name = "btnProductManager";
            this.btnProductManager.Size = new System.Drawing.Size(288, 100);
            this.btnProductManager.TabIndex = 10;
            this.btnProductManager.Text = "ProductManager";
            this.btnProductManager.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnProductManager.UseVisualStyleBackColor = true;
            this.btnProductManager.Click += new System.EventHandler(this.MenuButtonClicked);
            // 
            // pnlMenu
            // 
            this.pnlMenu.Controls.Add(this.picCompanyLogo);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMenu.Margin = new System.Windows.Forms.Padding(6);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(288, 168);
            this.pnlMenu.TabIndex = 0;
            // 
            // picCompanyLogo
            // 
            this.picCompanyLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picCompanyLogo.Image = global::UniScanX.MPAlignment.Properties.Resources.unieye;
            this.picCompanyLogo.InitialImage = global::UniScanX.MPAlignment.Properties.Resources.unieye;
            this.picCompanyLogo.Location = new System.Drawing.Point(0, 0);
            this.picCompanyLogo.Margin = new System.Windows.Forms.Padding(6);
            this.picCompanyLogo.Name = "picCompanyLogo";
            this.picCompanyLogo.Size = new System.Drawing.Size(288, 168);
            this.picCompanyLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picCompanyLogo.TabIndex = 0;
            this.picCompanyLogo.TabStop = false;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(90)))));
            this.pnlTop.Controls.Add(this.btnIoMonitor);
            this.pnlTop.Controls.Add(this.btnMinimize);
            this.pnlTop.Controls.Add(this.btnMaximize);
            this.pnlTop.Controls.Add(this.lblSelectedProduct);
            this.pnlTop.Controls.Add(this.btnClose);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(288, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(6);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1207, 74);
            this.pnlTop.TabIndex = 7;
            // 
            // btnMinimize
            // 
            this.btnMinimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimize.BackColor = System.Drawing.Color.Silver;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimize.Location = new System.Drawing.Point(1001, 4);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(6);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(59, 64);
            this.btnMinimize.TabIndex = 2;
            this.btnMinimize.Text = "-";
            this.btnMinimize.UseVisualStyleBackColor = false;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // btnMaximize
            // 
            this.btnMaximize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximize.BackColor = System.Drawing.Color.LightGray;
            this.btnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximize.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaximize.Location = new System.Drawing.Point(1071, 4);
            this.btnMaximize.Margin = new System.Windows.Forms.Padding(6);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(59, 64);
            this.btnMaximize.TabIndex = 1;
            this.btnMaximize.Text = "□";
            this.btnMaximize.UseVisualStyleBackColor = false;
            this.btnMaximize.Click += new System.EventHandler(this.btnMaximize_Click);
            // 
            // lblSelectedProduct
            // 
            this.lblSelectedProduct.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectedProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSelectedProduct.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSelectedProduct.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblSelectedProduct.Location = new System.Drawing.Point(0, 0);
            this.lblSelectedProduct.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblSelectedProduct.Name = "lblSelectedProduct";
            this.lblSelectedProduct.Padding = new System.Windows.Forms.Padding(37, 0, 0, 0);
            this.lblSelectedProduct.Size = new System.Drawing.Size(919, 74);
            this.lblSelectedProduct.TabIndex = 5;
            this.lblSelectedProduct.Text = "-";
            this.lblSelectedProduct.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(1142, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(59, 64);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.LightSlateGray;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(288, 74);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(6);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1207, 865);
            this.pnlMain.TabIndex = 8;
            // 
            // btnIoMonitor
            // 
            this.btnIoMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIoMonitor.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnIoMonitor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIoMonitor.Font = new System.Drawing.Font("Yu Gothic UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIoMonitor.Location = new System.Drawing.Point(930, 4);
            this.btnIoMonitor.Margin = new System.Windows.Forms.Padding(6);
            this.btnIoMonitor.Name = "btnIoMonitor";
            this.btnIoMonitor.Size = new System.Drawing.Size(59, 64);
            this.btnIoMonitor.TabIndex = 8;
            this.btnIoMonitor.Text = "IO";
            this.btnIoMonitor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnIoMonitor.UseVisualStyleBackColor = false;
            this.btnIoMonitor.Click += new System.EventHandler(this.btnIoMonitor_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(70)))), ((int)(((byte)(73)))));
            this.ClientSize = new System.Drawing.Size(1495, 939);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlLeft);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainForm";
            this.Text = "MP Alignment";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlLeft.ResumeLayout(false);
            this.pnlMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCompanyLogo)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Button btnLogs;
        private System.Windows.Forms.Button btnUserConfig;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Button btnTeach;
        private System.Windows.Forms.Button btnInspect;
        private System.Windows.Forms.Button btnProductManager;
        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.PictureBox picCompanyLogo;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnMaximize;
        private System.Windows.Forms.Label lblSelectedProduct;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Button btnIoMonitor;
    }
}

