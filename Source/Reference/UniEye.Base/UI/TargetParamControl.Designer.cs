namespace UniEye.Base.UI
{
    partial class TargetParamControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.probeId = new System.Windows.Forms.Label();
            this.labelProbeId = new System.Windows.Forms.Label();
            this.panelCommonParam = new System.Windows.Forms.Panel();
            this.probeFullId = new System.Windows.Forms.TextBox();
            this.labelProbeType = new System.Windows.Forms.Label();
            this.labelProbe = new System.Windows.Forms.Label();
            this.probeType = new System.Windows.Forms.Label();
            this.panelParam = new System.Windows.Forms.Panel();
            this.probeNamePanel = new System.Windows.Forms.Panel();
            this.labelProbeName = new System.Windows.Forms.Label();
            this.comboBoxProbeName = new System.Windows.Forms.ComboBox();
            this.panelTarget = new System.Windows.Forms.Panel();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.targetPictureBox = new System.Windows.Forms.PictureBox();
            this.txtTargetName = new System.Windows.Forms.TextBox();
            this.labelTargetName = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelMaskInspecter = new System.Windows.Forms.Panel();
            this.txtLocationNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelLocationNumber = new System.Windows.Forms.Label();
            this.padId = new System.Windows.Forms.Label();
            this.padType = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panelCommonParam.SuspendLayout();
            this.probeNamePanel.SuspendLayout();
            this.panelTarget.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.targetPictureBox)).BeginInit();
            this.panelMaskInspecter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(-2, -3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 0;
            // 
            // probeId
            // 
            this.probeId.BackColor = System.Drawing.Color.Lavender;
            this.probeId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.probeId.ForeColor = System.Drawing.Color.Black;
            this.probeId.Location = new System.Drawing.Point(117, 4);
            this.probeId.Margin = new System.Windows.Forms.Padding(0);
            this.probeId.Name = "probeId";
            this.probeId.Size = new System.Drawing.Size(50, 26);
            this.probeId.TabIndex = 11;
            this.probeId.Text = "ID";
            this.probeId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProbeId
            // 
            this.labelProbeId.AutoSize = true;
            this.labelProbeId.Location = new System.Drawing.Point(88, 10);
            this.labelProbeId.Name = "labelProbeId";
            this.labelProbeId.Size = new System.Drawing.Size(26, 20);
            this.labelProbeId.TabIndex = 10;
            this.labelProbeId.Text = "ID";
            // 
            // panelCommonParam
            // 
            this.panelCommonParam.Controls.Add(this.probeFullId);
            this.panelCommonParam.Controls.Add(this.labelProbeType);
            this.panelCommonParam.Controls.Add(this.probeId);
            this.panelCommonParam.Controls.Add(this.labelProbe);
            this.panelCommonParam.Controls.Add(this.labelProbeId);
            this.panelCommonParam.Controls.Add(this.probeType);
            this.panelCommonParam.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCommonParam.Location = new System.Drawing.Point(0, 96);
            this.panelCommonParam.Name = "panelCommonParam";
            this.panelCommonParam.Size = new System.Drawing.Size(445, 61);
            this.panelCommonParam.TabIndex = 0;
            // 
            // probeFullId
            // 
            this.probeFullId.Location = new System.Drawing.Point(169, 4);
            this.probeFullId.Name = "probeFullId";
            this.probeFullId.ReadOnly = true;
            this.probeFullId.Size = new System.Drawing.Size(169, 26);
            this.probeFullId.TabIndex = 14;
            // 
            // labelProbeType
            // 
            this.labelProbeType.AutoSize = true;
            this.labelProbeType.Location = new System.Drawing.Point(71, 34);
            this.labelProbeType.Name = "labelProbeType";
            this.labelProbeType.Size = new System.Drawing.Size(43, 20);
            this.labelProbeType.TabIndex = 12;
            this.labelProbeType.Text = "Type";
            this.labelProbeType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelProbe
            // 
            this.labelProbe.AutoSize = true;
            this.labelProbe.Location = new System.Drawing.Point(3, 17);
            this.labelProbe.Name = "labelProbe";
            this.labelProbe.Size = new System.Drawing.Size(51, 20);
            this.labelProbe.TabIndex = 9;
            this.labelProbe.Text = "Probe";
            // 
            // probeType
            // 
            this.probeType.BackColor = System.Drawing.Color.Lavender;
            this.probeType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.probeType.ForeColor = System.Drawing.Color.Black;
            this.probeType.Location = new System.Drawing.Point(116, 32);
            this.probeType.Margin = new System.Windows.Forms.Padding(0);
            this.probeType.Name = "probeType";
            this.probeType.Size = new System.Drawing.Size(222, 26);
            this.probeType.TabIndex = 13;
            this.probeType.Text = "Type";
            this.probeType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelParam
            // 
            this.panelParam.AutoScroll = true;
            this.panelParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelParam.Location = new System.Drawing.Point(0, 271);
            this.panelParam.Name = "panelParam";
            this.panelParam.Size = new System.Drawing.Size(445, 198);
            this.panelParam.TabIndex = 1;
            // 
            // probeNamePanel
            // 
            this.probeNamePanel.Controls.Add(this.labelProbeName);
            this.probeNamePanel.Controls.Add(this.comboBoxProbeName);
            this.probeNamePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.probeNamePanel.Location = new System.Drawing.Point(0, 157);
            this.probeNamePanel.Name = "probeNamePanel";
            this.probeNamePanel.Size = new System.Drawing.Size(445, 42);
            this.probeNamePanel.TabIndex = 2;
            // 
            // labelProbeName
            // 
            this.labelProbeName.AutoSize = true;
            this.labelProbeName.Location = new System.Drawing.Point(3, 9);
            this.labelProbeName.Name = "labelProbeName";
            this.labelProbeName.Size = new System.Drawing.Size(97, 20);
            this.labelProbeName.TabIndex = 10;
            this.labelProbeName.Text = "Probe Name";
            // 
            // comboBoxProbeName
            // 
            this.comboBoxProbeName.FormattingEnabled = true;
            this.comboBoxProbeName.Location = new System.Drawing.Point(117, 6);
            this.comboBoxProbeName.Name = "comboBoxProbeName";
            this.comboBoxProbeName.Size = new System.Drawing.Size(221, 28);
            this.comboBoxProbeName.TabIndex = 0;
            this.comboBoxProbeName.SelectedIndexChanged += new System.EventHandler(this.comboBoxProbeName_SelectedIndexChanged);
            this.comboBoxProbeName.TextChanged += new System.EventHandler(this.comboBoxProbeName_TextChanged);
            // 
            // panelTarget
            // 
            this.panelTarget.Controls.Add(this.buttonRefresh);
            this.panelTarget.Controls.Add(this.targetPictureBox);
            this.panelTarget.Controls.Add(this.txtTargetName);
            this.panelTarget.Controls.Add(this.labelTargetName);
            this.panelTarget.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTarget.Location = new System.Drawing.Point(0, 0);
            this.panelTarget.Name = "panelTarget";
            this.panelTarget.Size = new System.Drawing.Size(445, 96);
            this.panelTarget.TabIndex = 11;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(321, 3);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(100, 39);
            this.buttonRefresh.TabIndex = 13;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // targetPictureBox
            // 
            this.targetPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.targetPictureBox.Location = new System.Drawing.Point(182, 3);
            this.targetPictureBox.Name = "targetPictureBox";
            this.targetPictureBox.Size = new System.Drawing.Size(135, 89);
            this.targetPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.targetPictureBox.TabIndex = 12;
            this.targetPictureBox.TabStop = false;
            // 
            // txtTargetName
            // 
            this.txtTargetName.Location = new System.Drawing.Point(7, 32);
            this.txtTargetName.Name = "txtTargetName";
            this.txtTargetName.Size = new System.Drawing.Size(159, 26);
            this.txtTargetName.TabIndex = 11;
            this.txtTargetName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTargetName_KeyDown);
            // 
            // labelTargetName
            // 
            this.labelTargetName.AutoSize = true;
            this.labelTargetName.Location = new System.Drawing.Point(3, 9);
            this.labelTargetName.Name = "labelTargetName";
            this.labelTargetName.Size = new System.Drawing.Size(101, 20);
            this.labelTargetName.TabIndex = 10;
            this.labelTargetName.Text = "Target Name";
            // 
            // panelMaskInspecter
            // 
            this.panelMaskInspecter.Controls.Add(this.txtLocationNumber);
            this.panelMaskInspecter.Controls.Add(this.label1);
            this.panelMaskInspecter.Controls.Add(this.labelLocationNumber);
            this.panelMaskInspecter.Controls.Add(this.padId);
            this.panelMaskInspecter.Controls.Add(this.padType);
            this.panelMaskInspecter.Controls.Add(this.label5);
            this.panelMaskInspecter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMaskInspecter.Location = new System.Drawing.Point(0, 199);
            this.panelMaskInspecter.Name = "panelMaskInspecter";
            this.panelMaskInspecter.Size = new System.Drawing.Size(445, 72);
            this.panelMaskInspecter.TabIndex = 12;
            this.panelMaskInspecter.Visible = false;
            // 
            // txtLocationNumber
            // 
            this.txtLocationNumber.Location = new System.Drawing.Point(139, 6);
            this.txtLocationNumber.Name = "txtLocationNumber";
            this.txtLocationNumber.Size = new System.Drawing.Size(291, 26);
            this.txtLocationNumber.TabIndex = 11;
            this.txtLocationNumber.TextChanged += new System.EventHandler(this.txtPartName_TextChanged);
            this.txtLocationNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPartName_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(227, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Type";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelLocationNumber
            // 
            this.labelLocationNumber.AutoSize = true;
            this.labelLocationNumber.Location = new System.Drawing.Point(3, 9);
            this.labelLocationNumber.Name = "labelLocationNumber";
            this.labelLocationNumber.Size = new System.Drawing.Size(130, 20);
            this.labelLocationNumber.TabIndex = 10;
            this.labelLocationNumber.Text = "Location Number";
            // 
            // padId
            // 
            this.padId.BackColor = System.Drawing.Color.Lavender;
            this.padId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.padId.ForeColor = System.Drawing.Color.Black;
            this.padId.Location = new System.Drawing.Point(140, 38);
            this.padId.Margin = new System.Windows.Forms.Padding(0);
            this.padId.Name = "padId";
            this.padId.Size = new System.Drawing.Size(50, 26);
            this.padId.TabIndex = 11;
            this.padId.Text = "ID";
            this.padId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // padType
            // 
            this.padType.BackColor = System.Drawing.Color.Lavender;
            this.padType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.padType.ForeColor = System.Drawing.Color.Black;
            this.padType.Location = new System.Drawing.Point(273, 38);
            this.padType.Margin = new System.Windows.Forms.Padding(0);
            this.padType.Name = "padType";
            this.padType.Size = new System.Drawing.Size(157, 26);
            this.padType.TabIndex = 13;
            this.padType.Text = "Type";
            this.padType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Pad";
            // 
            // TargetParamControl
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.panelParam);
            this.Controls.Add(this.panelMaskInspecter);
            this.Controls.Add(this.probeNamePanel);
            this.Controls.Add(this.panelCommonParam);
            this.Controls.Add(this.panelTarget);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TargetParamControl";
            this.Size = new System.Drawing.Size(445, 469);
            this.panelCommonParam.ResumeLayout(false);
            this.panelCommonParam.PerformLayout();
            this.probeNamePanel.ResumeLayout(false);
            this.probeNamePanel.PerformLayout();
            this.panelTarget.ResumeLayout(false);
            this.panelTarget.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.targetPictureBox)).EndInit();
            this.panelMaskInspecter.ResumeLayout(false);
            this.panelMaskInspecter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label probeId;
        private System.Windows.Forms.Label labelProbeId;
        private System.Windows.Forms.Panel panelParam;
        private System.Windows.Forms.Label labelProbe;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelProbeType;
        private System.Windows.Forms.Label probeType;
        private System.Windows.Forms.Label labelProbeName;
        private System.Windows.Forms.ComboBox comboBoxProbeName;
        private System.Windows.Forms.Label labelTargetName;
        private System.Windows.Forms.TextBox txtTargetName;
        private System.Windows.Forms.PictureBox targetPictureBox;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.TextBox probeFullId;
        private System.Windows.Forms.TextBox txtLocationNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelLocationNumber;
        private System.Windows.Forms.Label padId;
        private System.Windows.Forms.Label padType;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.Panel panelCommonParam;
        public System.Windows.Forms.Panel probeNamePanel;
        public System.Windows.Forms.Panel panelTarget;
        public System.Windows.Forms.Panel panelMaskInspecter;
    }
}