

namespace UniScanX.MPAlignment.UI.Components
{
    partial class LoadingForm
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
            this.progressIndicator1 = new ReaLTaiizor.ProgressIndicator();
            this.lblNowProgress = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressIndicator1
            // 
            this.progressIndicator1.Location = new System.Drawing.Point(190, 76);
            this.progressIndicator1.MinimumSize = new System.Drawing.Size(80, 80);
            this.progressIndicator1.Name = "progressIndicator1";
            this.progressIndicator1.P_AnimationColor = System.Drawing.Color.White;
            this.progressIndicator1.P_AnimationSpeed = 100;
            this.progressIndicator1.P_BaseColor = System.Drawing.Color.PaleGreen;
            this.progressIndicator1.Size = new System.Drawing.Size(103, 103);
            this.progressIndicator1.TabIndex = 0;
            this.progressIndicator1.Text = "progressIndicator1";
            // 
            // lblNowProgress
            // 
            this.lblNowProgress.AutoSize = true;
            this.lblNowProgress.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblNowProgress.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblNowProgress.Location = new System.Drawing.Point(174, 221);
            this.lblNowProgress.Name = "lblNowProgress";
            this.lblNowProgress.Size = new System.Drawing.Size(144, 25);
            this.lblNowProgress.TabIndex = 1;
            this.lblNowProgress.Text = "Now Progress...";
            // 
            // LoadingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(495, 298);
            this.Controls.Add(this.lblNowProgress);
            this.Controls.Add(this.progressIndicator1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(261, 61);
            this.Name = "LoadingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "t";
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ReaLTaiizor.ProgressIndicator progressIndicator1;
        private System.Windows.Forms.Label lblNowProgress;
    }
}