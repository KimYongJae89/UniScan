namespace DynMvp.UI
{
    partial class SplashForm
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
            this.title = new System.Windows.Forms.Label();
            this.copyrightText = new System.Windows.Forms.Label();
            this.buildText = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressMessage = new System.Windows.Forms.Label();
            this.splashActionTimer = new System.Windows.Forms.Timer(this.components);
            this.versionText = new System.Windows.Forms.Label();
            this.productLogo = new System.Windows.Forms.PictureBox();
            this.companyLogo = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.productLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyLogo)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.title.Location = new System.Drawing.Point(16, 108);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(537, 89);
            this.title.TabIndex = 0;
            this.title.Text = "UniEye";
            // 
            // copyrightText
            // 
            this.copyrightText.AutoSize = true;
            this.copyrightText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.copyrightText.Location = new System.Drawing.Point(27, 322);
            this.copyrightText.Name = "copyrightText";
            this.copyrightText.Size = new System.Drawing.Size(239, 12);
            this.copyrightText.TabIndex = 1;
            this.copyrightText.Text = "©2015 PlanB Solutions. All right reserved.";
            // 
            // buildText
            // 
            this.buildText.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buildText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buildText.Location = new System.Drawing.Point(379, 265);
            this.buildText.Name = "buildText";
            this.buildText.Size = new System.Drawing.Size(178, 16);
            this.buildText.TabIndex = 1;
            this.buildText.Text = "Build yyyyMMdd.HHmm";
            this.buildText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(29, 286);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(344, 22);
            this.progressBar.TabIndex = 4;
            // 
            // progressMessage
            // 
            this.progressMessage.AutoSize = true;
            this.progressMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.progressMessage.Location = new System.Drawing.Point(27, 271);
            this.progressMessage.Name = "progressMessage";
            this.progressMessage.Size = new System.Drawing.Size(62, 12);
            this.progressMessage.TabIndex = 1;
            this.progressMessage.Text = "Loading...";
            // 
            // splashActionTimer
            // 
            this.splashActionTimer.Interval = 5000;
            this.splashActionTimer.Tick += new System.EventHandler(this.splashActionTimer_Tick);
            // 
            // versionText
            // 
            this.versionText.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.versionText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.versionText.Location = new System.Drawing.Point(379, 248);
            this.versionText.Name = "versionText";
            this.versionText.Size = new System.Drawing.Size(178, 16);
            this.versionText.TabIndex = 1;
            this.versionText.Text = "Version x.y";
            this.versionText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // productLogo
            // 
            this.productLogo.Location = new System.Drawing.Point(490, 9);
            this.productLogo.Name = "productLogo";
            this.productLogo.Size = new System.Drawing.Size(67, 56);
            this.productLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.productLogo.TabIndex = 2;
            this.productLogo.TabStop = false;
            // 
            // companyLogo
            // 
            this.companyLogo.Location = new System.Drawing.Point(379, 286);
            this.companyLogo.Name = "companyLogo";
            this.companyLogo.Size = new System.Drawing.Size(178, 48);
            this.companyLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.companyLogo.TabIndex = 2;
            this.companyLogo.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::DynMvp.Properties.Resources.sumnale;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.progressBar);
            this.panel1.Controls.Add(this.title);
            this.panel1.Controls.Add(this.productLogo);
            this.panel1.Controls.Add(this.copyrightText);
            this.panel1.Controls.Add(this.companyLogo);
            this.panel1.Controls.Add(this.progressMessage);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.versionText);
            this.panel1.Controls.Add(this.buildText);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(566, 341);
            this.panel1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label1.Location = new System.Drawing.Point(379, 231);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Version x.y";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(566, 341);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashForm";
            this.Load += new System.EventHandler(this.SplashForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SplashForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.productLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.companyLogo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressBar;
        public System.Windows.Forms.Label title;
        public System.Windows.Forms.Label buildText;
        public System.Windows.Forms.Label copyrightText;
        public System.Windows.Forms.PictureBox companyLogo;
        public System.Windows.Forms.Label progressMessage;
        private System.Windows.Forms.Timer splashActionTimer;
        public System.Windows.Forms.PictureBox productLogo;
        public System.Windows.Forms.Label versionText;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Label label1;
    }
}