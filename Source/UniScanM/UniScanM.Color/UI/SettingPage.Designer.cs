namespace UniScanM.ColorSens.UI
{
    partial class SettingPage
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
            this.panelCommand = new System.Windows.Forms.Panel();
            this.ultraExpandableGroupBox1 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.buttonCamera = new System.Windows.Forms.Button();
            this.panel_Bottom = new System.Windows.Forms.Panel();
            this.button_Set = new System.Windows.Forms.Button();
            this.button_Reset = new System.Windows.Forms.Button();
            this.panel_Center = new System.Windows.Forms.Panel();
            this.propertyGrid_Config = new System.Windows.Forms.PropertyGrid();
            this.panelCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).BeginInit();
            this.ultraExpandableGroupBox1.SuspendLayout();
            this.ultraExpandableGroupBoxPanel1.SuspendLayout();
            this.panel_Bottom.SuspendLayout();
            this.panel_Center.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCommand
            // 
            this.panelCommand.AutoSize = true;
            this.panelCommand.Controls.Add(this.ultraExpandableGroupBox1);
            this.panelCommand.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCommand.Location = new System.Drawing.Point(0, 0);
            this.panelCommand.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelCommand.Name = "panelCommand";
            this.panelCommand.Size = new System.Drawing.Size(516, 22);
            this.panelCommand.TabIndex = 4;
            // 
            // ultraExpandableGroupBox1
            // 
            this.ultraExpandableGroupBox1.Controls.Add(this.ultraExpandableGroupBoxPanel1);
            this.ultraExpandableGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraExpandableGroupBox1.Expanded = false;
            this.ultraExpandableGroupBox1.ExpandedSize = new System.Drawing.Size(516, 129);
            this.ultraExpandableGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.ultraExpandableGroupBox1.Name = "ultraExpandableGroupBox1";
            this.ultraExpandableGroupBox1.Size = new System.Drawing.Size(516, 22);
            this.ultraExpandableGroupBox1.TabIndex = 13;
            this.ultraExpandableGroupBox1.Text = "Device";
            this.ultraExpandableGroupBox1.Visible = false;
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.buttonCamera);
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraExpandableGroupBoxPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(510, 106);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            this.ultraExpandableGroupBoxPanel1.Visible = false;
            // 
            // buttonCamera
            // 
            this.buttonCamera.Location = new System.Drawing.Point(21, 3);
            this.buttonCamera.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonCamera.Name = "buttonCamera";
            this.buttonCamera.Size = new System.Drawing.Size(109, 71);
            this.buttonCamera.TabIndex = 8;
            this.buttonCamera.Text = "Camera";
            this.buttonCamera.UseVisualStyleBackColor = true;
            this.buttonCamera.Click += new System.EventHandler(this.buttonCamera_Click);
            // 
            // panel_Bottom
            // 
            this.panel_Bottom.Controls.Add(this.button_Set);
            this.panel_Bottom.Controls.Add(this.button_Reset);
            this.panel_Bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_Bottom.Location = new System.Drawing.Point(0, 571);
            this.panel_Bottom.Margin = new System.Windows.Forms.Padding(4);
            this.panel_Bottom.Name = "panel_Bottom";
            this.panel_Bottom.Size = new System.Drawing.Size(516, 112);
            this.panel_Bottom.TabIndex = 5;
            // 
            // button_Set
            // 
            this.button_Set.Location = new System.Drawing.Point(389, 7);
            this.button_Set.Name = "button_Set";
            this.button_Set.Size = new System.Drawing.Size(118, 46);
            this.button_Set.TabIndex = 0;
            this.button_Set.Text = "Set";
            this.button_Set.UseVisualStyleBackColor = true;
            this.button_Set.Click += new System.EventHandler(this.button_Set_Click);
            // 
            // button_Reset
            // 
            this.button_Reset.Location = new System.Drawing.Point(6, 7);
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.Size = new System.Drawing.Size(118, 46);
            this.button_Reset.TabIndex = 0;
            this.button_Reset.Text = "Reset";
            this.button_Reset.UseVisualStyleBackColor = true;
            this.button_Reset.Click += new System.EventHandler(this.button_Reset_Click);
            // 
            // panel_Center
            // 
            this.panel_Center.Controls.Add(this.propertyGrid_Config);
            this.panel_Center.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_Center.Location = new System.Drawing.Point(0, 22);
            this.panel_Center.Margin = new System.Windows.Forms.Padding(4);
            this.panel_Center.Name = "panel_Center";
            this.panel_Center.Size = new System.Drawing.Size(516, 549);
            this.panel_Center.TabIndex = 6;
            // 
            // propertyGrid_Config
            // 
            this.propertyGrid_Config.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid_Config.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid_Config.Margin = new System.Windows.Forms.Padding(4);
            this.propertyGrid_Config.Name = "propertyGrid_Config";
            this.propertyGrid_Config.Size = new System.Drawing.Size(516, 549);
            this.propertyGrid_Config.TabIndex = 0;
            // 
            // SettingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel_Center);
            this.Controls.Add(this.panel_Bottom);
            this.Controls.Add(this.panelCommand);
            this.Font = new System.Drawing.Font("Gulim", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SettingPage";
            this.Size = new System.Drawing.Size(516, 683);
            this.Load += new System.EventHandler(this.SettingPanel_Load);
            this.panelCommand.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).EndInit();
            this.ultraExpandableGroupBox1.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.ResumeLayout(false);
            this.panel_Bottom.ResumeLayout(false);
            this.panel_Center.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelCommand;
        private System.Windows.Forms.Button buttonCamera;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox1;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private System.Windows.Forms.Panel panel_Bottom;
        private System.Windows.Forms.Panel panel_Center;
        private System.Windows.Forms.PropertyGrid propertyGrid_Config;
        private System.Windows.Forms.Button button_Set;
        private System.Windows.Forms.Button button_Reset;
    }
}
