namespace UniScanM.StillImage.UI.MenuPage.SettingPanel
{
    partial class SettingPageMonitoringPanel
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
            this.panelHeader = new System.Windows.Forms.Panel();
            this.panelDrawBox = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(608, 53);
            this.panelHeader.TabIndex = 0;
            // 
            // panelDrawBox
            // 
            this.panelDrawBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDrawBox.Location = new System.Drawing.Point(0, 53);
            this.panelDrawBox.Name = "panelDrawBox";
            this.panelDrawBox.Size = new System.Drawing.Size(608, 457);
            this.panelDrawBox.TabIndex = 1;
            // 
            // SettingPageMonitoringPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelDrawBox);
            this.Controls.Add(this.panelHeader);
            this.Name = "SettingPageMonitoringPanel";
            this.Size = new System.Drawing.Size(608, 510);
            this.Load += new System.EventHandler(this.SettingPageMonitoringPanel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelDrawBox;
    }
}
