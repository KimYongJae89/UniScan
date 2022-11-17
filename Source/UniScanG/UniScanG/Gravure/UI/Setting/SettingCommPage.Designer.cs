namespace UniScanG.Gravure.UI.Setting
{
    partial class SettingCommPage
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
            this.checkBoxAutoOperation = new System.Windows.Forms.CheckBox();
            this.openIoViewer = new System.Windows.Forms.Button();
            this.openEncoderSetting = new System.Windows.Forms.Button();
            this.shutdownIms = new System.Windows.Forms.Button();
            this.startIms = new System.Windows.Forms.Button();
            this.resetIMs = new System.Windows.Forms.Button();
            this.launchImsArgs = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // checkBoxAutoOperation
            // 
            this.checkBoxAutoOperation.AutoSize = true;
            this.checkBoxAutoOperation.Location = new System.Drawing.Point(3, 3);
            this.checkBoxAutoOperation.Name = "checkBoxAutoOperation";
            this.checkBoxAutoOperation.Size = new System.Drawing.Size(145, 25);
            this.checkBoxAutoOperation.TabIndex = 0;
            this.checkBoxAutoOperation.Text = "Auto Operation";
            this.checkBoxAutoOperation.UseVisualStyleBackColor = true;
            this.checkBoxAutoOperation.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // openIoViewer
            // 
            this.openIoViewer.Location = new System.Drawing.Point(3, 34);
            this.openIoViewer.Name = "openIoViewer";
            this.openIoViewer.Size = new System.Drawing.Size(192, 47);
            this.openIoViewer.TabIndex = 3;
            this.openIoViewer.Text = "Open I/O Viewer";
            this.openIoViewer.UseVisualStyleBackColor = true;
            this.openIoViewer.Click += new System.EventHandler(this.openIoViewer_Click);
            // 
            // openEncoderSetting
            // 
            this.openEncoderSetting.Location = new System.Drawing.Point(3, 87);
            this.openEncoderSetting.Name = "openEncoderSetting";
            this.openEncoderSetting.Size = new System.Drawing.Size(192, 47);
            this.openEncoderSetting.TabIndex = 3;
            this.openEncoderSetting.Text = "Open Encoder Setting";
            this.openEncoderSetting.UseVisualStyleBackColor = true;
            this.openEncoderSetting.Click += new System.EventHandler(this.openEncoderSetting_Click);
            // 
            // shutdownIms
            // 
            this.shutdownIms.Location = new System.Drawing.Point(201, 189);
            this.shutdownIms.Name = "shutdownIms";
            this.shutdownIms.Size = new System.Drawing.Size(192, 47);
            this.shutdownIms.TabIndex = 3;
            this.shutdownIms.Text = "Shutdown All IMs";
            this.shutdownIms.UseVisualStyleBackColor = true;
            this.shutdownIms.Click += new System.EventHandler(this.shutdownIms_Click);
            // 
            // startIms
            // 
            this.startIms.Location = new System.Drawing.Point(201, 242);
            this.startIms.Name = "startIms";
            this.startIms.Size = new System.Drawing.Size(192, 47);
            this.startIms.TabIndex = 3;
            this.startIms.Text = "Start All IMs";
            this.startIms.UseVisualStyleBackColor = true;
            this.startIms.Click += new System.EventHandler(this.startIms_Click);
            // 
            // resetIMs
            // 
            this.resetIMs.Location = new System.Drawing.Point(3, 189);
            this.resetIMs.Name = "resetIMs";
            this.resetIMs.Size = new System.Drawing.Size(192, 47);
            this.resetIMs.TabIndex = 3;
            this.resetIMs.Text = "Restart All IMs";
            this.resetIMs.UseVisualStyleBackColor = true;
            this.resetIMs.Click += new System.EventHandler(this.resetIMs_Click);
            // 
            // launchImsArgs
            // 
            this.launchImsArgs.Location = new System.Drawing.Point(3, 295);
            this.launchImsArgs.Name = "launchImsArgs";
            this.launchImsArgs.Size = new System.Drawing.Size(390, 29);
            this.launchImsArgs.TabIndex = 4;
            this.launchImsArgs.KeyDown += new System.Windows.Forms.KeyEventHandler(this.launchImsArgs_KeyDown);
            // 
            // SettingCommPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.launchImsArgs);
            this.Controls.Add(this.resetIMs);
            this.Controls.Add(this.startIms);
            this.Controls.Add(this.shutdownIms);
            this.Controls.Add(this.openEncoderSetting);
            this.Controls.Add(this.openIoViewer);
            this.Controls.Add(this.checkBoxAutoOperation);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "SettingCommPage";
            this.Size = new System.Drawing.Size(949, 493);
            this.Load += new System.EventHandler(this.SettingCommPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxAutoOperation;
        private System.Windows.Forms.Button openIoViewer;
        private System.Windows.Forms.Button openEncoderSetting;
        private System.Windows.Forms.Button shutdownIms;
        private System.Windows.Forms.Button startIms;
        private System.Windows.Forms.Button resetIMs;
        private System.Windows.Forms.TextBox launchImsArgs;
    }
}
