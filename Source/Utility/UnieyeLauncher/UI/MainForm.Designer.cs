namespace UnieyeLauncher
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
            this.buttonLaunch = new System.Windows.Forms.Button();
            this.buttonBackup = new System.Windows.Forms.Button();
            this.buttonRestore = new System.Windows.Forms.Button();
            this.comboBoxExcutable = new System.Windows.Forms.ComboBox();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.versionStringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.panelSetting = new System.Windows.Forms.Panel();
            this.buttonRemoveStartupMenu = new System.Windows.Forms.Button();
            this.buttonAddStartupMenu = new System.Windows.Forms.Button();
            this.buttonSettingSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonKill = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStrip.SuspendLayout();
            this.panelSetting.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonLaunch
            // 
            resources.ApplyResources(this.buttonLaunch, "buttonLaunch");
            this.buttonLaunch.Name = "buttonLaunch";
            this.buttonLaunch.UseVisualStyleBackColor = true;
            this.buttonLaunch.Click += new System.EventHandler(this.buttonLaunch_Click);
            // 
            // buttonBackup
            // 
            resources.ApplyResources(this.buttonBackup, "buttonBackup");
            this.buttonBackup.Name = "buttonBackup";
            this.buttonBackup.UseVisualStyleBackColor = true;
            this.buttonBackup.Click += new System.EventHandler(this.buttonBackup_Click);
            // 
            // buttonRestore
            // 
            resources.ApplyResources(this.buttonRestore, "buttonRestore");
            this.buttonRestore.Name = "buttonRestore";
            this.buttonRestore.UseVisualStyleBackColor = true;
            this.buttonRestore.Click += new System.EventHandler(this.buttonRestore_Click);
            // 
            // comboBoxExcutable
            // 
            this.comboBoxExcutable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExcutable.FormattingEnabled = true;
            resources.ApplyResources(this.comboBoxExcutable, "comboBoxExcutable");
            this.comboBoxExcutable.Name = "comboBoxExcutable";
            this.comboBoxExcutable.SelectedIndexChanged += new System.EventHandler(this.comboBoxExcutable_SelectedIndexChanged);
            // 
            // propertyGrid1
            // 
            resources.ApplyResources(this.propertyGrid1, "propertyGrid1");
            this.propertyGrid1.Name = "propertyGrid1";
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            resources.ApplyResources(this.notifyIcon, "notifyIcon");
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.versionStringToolStripMenuItem,
            this.settingToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            // 
            // versionStringToolStripMenuItem
            // 
            resources.ApplyResources(this.versionStringToolStripMenuItem, "versionStringToolStripMenuItem");
            this.versionStringToolStripMenuItem.Name = "versionStringToolStripMenuItem";
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            resources.ApplyResources(this.settingToolStripMenuItem, "settingToolStripMenuItem");
            this.settingToolStripMenuItem.Click += new System.EventHandler(this.settingToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            resources.ApplyResources(this.statusStrip, "statusStrip");
            this.statusStrip.Name = "statusStrip";
            // 
            // panelSetting
            // 
            this.panelSetting.Controls.Add(this.groupBox1);
            this.panelSetting.Controls.Add(this.propertyGrid1);
            this.panelSetting.Controls.Add(this.buttonSettingSave);
            resources.ApplyResources(this.panelSetting, "panelSetting");
            this.panelSetting.Name = "panelSetting";
            // 
            // buttonRemoveStartupMenu
            // 
            resources.ApplyResources(this.buttonRemoveStartupMenu, "buttonRemoveStartupMenu");
            this.buttonRemoveStartupMenu.Name = "buttonRemoveStartupMenu";
            this.buttonRemoveStartupMenu.UseVisualStyleBackColor = true;
            this.buttonRemoveStartupMenu.Click += new System.EventHandler(this.buttonRemoveStartupMenu_Click);
            // 
            // buttonAddStartupMenu
            // 
            resources.ApplyResources(this.buttonAddStartupMenu, "buttonAddStartupMenu");
            this.buttonAddStartupMenu.Name = "buttonAddStartupMenu";
            this.buttonAddStartupMenu.UseVisualStyleBackColor = true;
            this.buttonAddStartupMenu.Click += new System.EventHandler(this.buttonAddStartupMenu_Click);
            // 
            // buttonSettingSave
            // 
            resources.ApplyResources(this.buttonSettingSave, "buttonSettingSave");
            this.buttonSettingSave.Name = "buttonSettingSave";
            this.buttonSettingSave.UseVisualStyleBackColor = true;
            this.buttonSettingSave.Click += new System.EventHandler(this.buttonSettingSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonBackup);
            this.panel1.Controls.Add(this.buttonRestore);
            this.panel1.Controls.Add(this.comboBoxExcutable);
            this.panel1.Controls.Add(this.buttonKill);
            this.panel1.Controls.Add(this.buttonLaunch);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // buttonKill
            // 
            resources.ApplyResources(this.buttonKill, "buttonKill");
            this.buttonKill.Name = "buttonKill";
            this.buttonKill.UseVisualStyleBackColor = true;
            this.buttonKill.Click += new System.EventHandler(this.buttonKill_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonAddStartupMenu);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.buttonRemoveStartupMenu);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonClearStartupMenu_Click);
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelSetting);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.VisibleChanged += new System.EventHandler(this.MainForm_VisibleChanged);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.contextMenuStrip.ResumeLayout(false);
            this.panelSetting.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonLaunch;
        private System.Windows.Forms.Button buttonBackup;
        private System.Windows.Forms.Button buttonRestore;
        private System.Windows.Forms.ComboBox comboBoxExcutable;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.Panel panelSetting;
        private System.Windows.Forms.Button buttonSettingSave;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.Button buttonKill;
        private System.Windows.Forms.ToolStripMenuItem versionStringToolStripMenuItem;
        private System.Windows.Forms.Button buttonRemoveStartupMenu;
        private System.Windows.Forms.Button buttonAddStartupMenu;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
    }
}