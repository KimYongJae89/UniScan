namespace UniScanG.Module.Controller.UI.Settings
{
    partial class SettingGradePage
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
            this.groupBoxOverall = new System.Windows.Forms.GroupBox();
            this.groupBoxNoprint = new System.Windows.Forms.GroupBox();
            this.groupBoxPinhole = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // groupBoxOverall
            // 
            this.groupBoxOverall.AutoSize = true;
            this.groupBoxOverall.Location = new System.Drawing.Point(29, 5);
            this.groupBoxOverall.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxOverall.MinimumSize = new System.Drawing.Size(200, 30);
            this.groupBoxOverall.Name = "groupBoxOverall";
            this.groupBoxOverall.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxOverall.Size = new System.Drawing.Size(200, 30);
            this.groupBoxOverall.TabIndex = 0;
            this.groupBoxOverall.TabStop = false;
            this.groupBoxOverall.Text = "Roll Grade";
            // 
            // groupBoxNoprint
            // 
            this.groupBoxNoprint.AutoSize = true;
            this.groupBoxNoprint.Location = new System.Drawing.Point(29, 251);
            this.groupBoxNoprint.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxNoprint.MinimumSize = new System.Drawing.Size(200, 30);
            this.groupBoxNoprint.Name = "groupBoxNoprint";
            this.groupBoxNoprint.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxNoprint.Size = new System.Drawing.Size(200, 30);
            this.groupBoxNoprint.TabIndex = 0;
            this.groupBoxNoprint.TabStop = false;
            this.groupBoxNoprint.Text = "Noprint Grade";
            // 
            // groupBoxPinhole
            // 
            this.groupBoxPinhole.AutoSize = true;
            this.groupBoxPinhole.Location = new System.Drawing.Point(29, 497);
            this.groupBoxPinhole.Margin = new System.Windows.Forms.Padding(0);
            this.groupBoxPinhole.MinimumSize = new System.Drawing.Size(200, 30);
            this.groupBoxPinhole.Name = "groupBoxPinhole";
            this.groupBoxPinhole.Padding = new System.Windows.Forms.Padding(0);
            this.groupBoxPinhole.Size = new System.Drawing.Size(200, 30);
            this.groupBoxPinhole.TabIndex = 0;
            this.groupBoxPinhole.TabStop = false;
            this.groupBoxPinhole.Text = "Pinhole Grade";
            // 
            // SettingGradePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxPinhole);
            this.Controls.Add(this.groupBoxNoprint);
            this.Controls.Add(this.groupBoxOverall);
            this.Font = new System.Drawing.Font("Gulim", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SettingGradePage";
            this.Size = new System.Drawing.Size(948, 763);
            this.Load += new System.EventHandler(this.SettingGradePage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxOverall;
        private System.Windows.Forms.GroupBox groupBoxNoprint;
        private System.Windows.Forms.GroupBox groupBoxPinhole;
    }
}
