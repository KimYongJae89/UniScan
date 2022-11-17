namespace UniEye.Base.UI.ParamControl
{
    partial class VisionParamControl
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
            this.imageBandLabel = new System.Windows.Forms.Label();
            this.imageBand = new System.Windows.Forms.ComboBox();
            this.inverseResult = new System.Windows.Forms.CheckBox();
            this.probeHeight = new System.Windows.Forms.NumericUpDown();
            this.probeWidth = new System.Windows.Forms.NumericUpDown();
            this.probePosX = new System.Windows.Forms.NumericUpDown();
            this.probePosY = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelPos = new System.Windows.Forms.Label();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.labelW = new System.Windows.Forms.Label();
            this.labelH = new System.Windows.Forms.Label();
            this.probePosR = new System.Windows.Forms.NumericUpDown();
            this.labelR = new System.Windows.Forms.Label();
            this.lightTypeCombo = new System.Windows.Forms.ComboBox();
            this.labelLightType = new System.Windows.Forms.Label();
            this.comboFiducialProbe = new System.Windows.Forms.ComboBox();
            this.labelFiducialProbe = new System.Windows.Forms.Label();
            this.algorithmParamPanel = new System.Windows.Forms.Panel();
            this.buttonFilterUp = new System.Windows.Forms.Button();
            this.buttonFilterDown = new System.Windows.Forms.Button();
            this.buttonDeleteFilter = new System.Windows.Forms.Button();
            this.buttonAddFilter = new System.Windows.Forms.Button();
            this.panelFilterParamControl = new System.Windows.Forms.Panel();
            this.filterListBox = new System.Windows.Forms.ListBox();
            this.contextMenuStripAddFilter = new System.Windows.Forms.ContextMenuStrip();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.tabPageFilter = new System.Windows.Forms.TabPage();
            this.tabPageInspection = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.probeHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.probeWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosR)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageFilter.SuspendLayout();
            this.tabPageInspection.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageBandLabel
            // 
            this.imageBandLabel.AutoSize = true;
            this.imageBandLabel.Location = new System.Drawing.Point(192, 11);
            this.imageBandLabel.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.imageBandLabel.Name = "imageBandLabel";
            this.imageBandLabel.Size = new System.Drawing.Size(96, 20);
            this.imageBandLabel.TabIndex = 0;
            this.imageBandLabel.Text = "Image Band";
            // 
            // imageBand
            // 
            this.imageBand.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.imageBand.FormattingEnabled = true;
            this.imageBand.Items.AddRange(new object[] {
            "Luminance",
            "Red",
            "Green",
            "Blue"});
            this.imageBand.Location = new System.Drawing.Point(292, 6);
            this.imageBand.Name = "imageBand";
            this.imageBand.Size = new System.Drawing.Size(128, 28);
            this.imageBand.TabIndex = 1;
            this.imageBand.SelectedIndexChanged += new System.EventHandler(this.colorBand_SelectedIndexChanged);
            // 
            // inverseResult
            // 
            this.inverseResult.AutoSize = true;
            this.inverseResult.Location = new System.Drawing.Point(13, 125);
            this.inverseResult.Name = "inverseResult";
            this.inverseResult.Size = new System.Drawing.Size(130, 24);
            this.inverseResult.TabIndex = 5;
            this.inverseResult.Text = "Inverse Result";
            this.inverseResult.UseVisualStyleBackColor = true;
            this.inverseResult.CheckedChanged += new System.EventHandler(this.inverseResult_CheckedChanged);
            // 
            // probeHeight
            // 
            this.probeHeight.Location = new System.Drawing.Point(342, 69);
            this.probeHeight.Name = "probeHeight";
            this.probeHeight.Size = new System.Drawing.Size(82, 26);
            this.probeHeight.TabIndex = 9;
            this.probeHeight.ValueChanged += new System.EventHandler(this.probeHeight_ValueChanged);
            this.probeHeight.Enter += new System.EventHandler(this.textBox_Enter);
            this.probeHeight.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // probeWidth
            // 
            this.probeWidth.Location = new System.Drawing.Point(342, 39);
            this.probeWidth.Name = "probeWidth";
            this.probeWidth.Size = new System.Drawing.Size(82, 26);
            this.probeWidth.TabIndex = 7;
            this.probeWidth.ValueChanged += new System.EventHandler(this.probeWidth_ValueChanged);
            this.probeWidth.Enter += new System.EventHandler(this.textBox_Enter);
            this.probeWidth.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // probePosX
            // 
            this.probePosX.Location = new System.Drawing.Point(135, 39);
            this.probePosX.Name = "probePosX";
            this.probePosX.Size = new System.Drawing.Size(82, 26);
            this.probePosX.TabIndex = 2;
            this.probePosX.ValueChanged += new System.EventHandler(this.probePosX_ValueChanged);
            this.probePosX.Enter += new System.EventHandler(this.textBox_Enter);
            this.probePosX.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // probePosY
            // 
            this.probePosY.Location = new System.Drawing.Point(135, 67);
            this.probePosY.Name = "probePosY";
            this.probePosY.Size = new System.Drawing.Size(82, 26);
            this.probePosY.TabIndex = 4;
            this.probePosY.ValueChanged += new System.EventHandler(this.probePosY_ValueChanged);
            this.probePosY.Enter += new System.EventHandler(this.textBox_Enter);
            this.probePosY.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 100);
            this.panel1.TabIndex = 0;
            // 
            // labelPos
            // 
            this.labelPos.AutoSize = true;
            this.labelPos.Location = new System.Drawing.Point(9, 41);
            this.labelPos.Name = "labelPos";
            this.labelPos.Size = new System.Drawing.Size(65, 20);
            this.labelPos.TabIndex = 0;
            this.labelPos.Text = "Position";
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Location = new System.Drawing.Point(235, 42);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(40, 20);
            this.labelSize.TabIndex = 5;
            this.labelSize.Text = "Size";
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(86, 42);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(20, 20);
            this.labelX.TabIndex = 1;
            this.labelX.Text = "X";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(86, 70);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(20, 20);
            this.labelY.TabIndex = 3;
            this.labelY.Text = "Y";
            // 
            // labelW
            // 
            this.labelW.AutoSize = true;
            this.labelW.Location = new System.Drawing.Point(287, 41);
            this.labelW.Name = "labelW";
            this.labelW.Size = new System.Drawing.Size(24, 20);
            this.labelW.TabIndex = 6;
            this.labelW.Text = "W";
            // 
            // labelH
            // 
            this.labelH.AutoSize = true;
            this.labelH.Location = new System.Drawing.Point(287, 76);
            this.labelH.Name = "labelH";
            this.labelH.Size = new System.Drawing.Size(21, 20);
            this.labelH.TabIndex = 8;
            this.labelH.Text = "H";
            // 
            // probePosR
            // 
            this.probePosR.Location = new System.Drawing.Point(135, 95);
            this.probePosR.Name = "probePosR";
            this.probePosR.Size = new System.Drawing.Size(82, 26);
            this.probePosR.TabIndex = 161;
            this.probePosR.ValueChanged += new System.EventHandler(this.probePosR_ValueChanged);
            // 
            // labelR
            // 
            this.labelR.AutoSize = true;
            this.labelR.Location = new System.Drawing.Point(86, 98);
            this.labelR.Name = "labelR";
            this.labelR.Size = new System.Drawing.Size(21, 20);
            this.labelR.TabIndex = 160;
            this.labelR.Text = "R";
            // 
            // lightTypeCombo
            // 
            this.lightTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lightTypeCombo.FormattingEnabled = true;
            this.lightTypeCombo.Location = new System.Drawing.Point(135, 7);
            this.lightTypeCombo.Name = "lightTypeCombo";
            this.lightTypeCombo.Size = new System.Drawing.Size(155, 28);
            this.lightTypeCombo.TabIndex = 159;
            this.lightTypeCombo.DropDown += new System.EventHandler(this.lightTypeCombo_DropDown);
            this.lightTypeCombo.SelectedIndexChanged += new System.EventHandler(this.lightTypeCombo_SelectedIndexChanged);
            // 
            // labelLightType
            // 
            this.labelLightType.AutoSize = true;
            this.labelLightType.Location = new System.Drawing.Point(10, 10);
            this.labelLightType.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelLightType.Name = "labelLightType";
            this.labelLightType.Size = new System.Drawing.Size(82, 20);
            this.labelLightType.TabIndex = 158;
            this.labelLightType.Text = "Light Type";
            // 
            // comboFiducialProbe
            // 
            this.comboFiducialProbe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboFiducialProbe.FormattingEnabled = true;
            this.comboFiducialProbe.Items.AddRange(new object[] {
            "Luminance",
            "Red",
            "Green",
            "Blue"});
            this.comboFiducialProbe.Location = new System.Drawing.Point(343, 122);
            this.comboFiducialProbe.Name = "comboFiducialProbe";
            this.comboFiducialProbe.Size = new System.Drawing.Size(80, 28);
            this.comboFiducialProbe.TabIndex = 1;
            this.comboFiducialProbe.SelectedIndexChanged += new System.EventHandler(this.comboFiducialProbe_SelectedIndexChanged);
            // 
            // labelFiducialProbe
            // 
            this.labelFiducialProbe.AutoSize = true;
            this.labelFiducialProbe.Location = new System.Drawing.Point(228, 127);
            this.labelFiducialProbe.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelFiducialProbe.Name = "labelFiducialProbe";
            this.labelFiducialProbe.Size = new System.Drawing.Size(109, 20);
            this.labelFiducialProbe.TabIndex = 0;
            this.labelFiducialProbe.Text = "Fiducial Probe";
            // 
            // algorithmParamPanel
            // 
            this.algorithmParamPanel.AutoScroll = true;
            this.algorithmParamPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.algorithmParamPanel.Location = new System.Drawing.Point(3, 3);
            this.algorithmParamPanel.Name = "algorithmParamPanel";
            this.algorithmParamPanel.Size = new System.Drawing.Size(429, 399);
            this.algorithmParamPanel.TabIndex = 1;
            // 
            // buttonFilterUp
            // 
            this.buttonFilterUp.Location = new System.Drawing.Point(6, 52);
            this.buttonFilterUp.Name = "buttonFilterUp";
            this.buttonFilterUp.Size = new System.Drawing.Size(22, 30);
            this.buttonFilterUp.TabIndex = 6;
            this.buttonFilterUp.Text = "U";
            this.buttonFilterUp.UseVisualStyleBackColor = true;
            this.buttonFilterUp.Click += new System.EventHandler(this.buttonFilterUp_Click);
            // 
            // buttonFilterDown
            // 
            this.buttonFilterDown.Location = new System.Drawing.Point(6, 98);
            this.buttonFilterDown.Name = "buttonFilterDown";
            this.buttonFilterDown.Size = new System.Drawing.Size(22, 30);
            this.buttonFilterDown.TabIndex = 5;
            this.buttonFilterDown.Text = "D";
            this.buttonFilterDown.UseVisualStyleBackColor = true;
            this.buttonFilterDown.Click += new System.EventHandler(this.buttonFilterDown_Click);
            // 
            // buttonDeleteFilter
            // 
            this.buttonDeleteFilter.Location = new System.Drawing.Point(95, 6);
            this.buttonDeleteFilter.Name = "buttonDeleteFilter";
            this.buttonDeleteFilter.Size = new System.Drawing.Size(88, 30);
            this.buttonDeleteFilter.TabIndex = 4;
            this.buttonDeleteFilter.Text = "Delete";
            this.buttonDeleteFilter.UseVisualStyleBackColor = true;
            this.buttonDeleteFilter.Click += new System.EventHandler(this.buttonDeleteFilter_Click);
            // 
            // buttonAddFilter
            // 
            this.buttonAddFilter.Location = new System.Drawing.Point(6, 6);
            this.buttonAddFilter.Name = "buttonAddFilter";
            this.buttonAddFilter.Size = new System.Drawing.Size(83, 30);
            this.buttonAddFilter.TabIndex = 4;
            this.buttonAddFilter.Text = "Add";
            this.buttonAddFilter.UseVisualStyleBackColor = true;
            this.buttonAddFilter.Click += new System.EventHandler(this.buttonAddFilter_Click);
            // 
            // panelFilterParamControl
            // 
            this.panelFilterParamControl.Location = new System.Drawing.Point(187, 40);
            this.panelFilterParamControl.Name = "panelFilterParamControl";
            this.panelFilterParamControl.Size = new System.Drawing.Size(232, 103);
            this.panelFilterParamControl.TabIndex = 3;
            // 
            // filterListBox
            // 
            this.filterListBox.FormattingEnabled = true;
            this.filterListBox.ItemHeight = 20;
            this.filterListBox.Location = new System.Drawing.Point(34, 40);
            this.filterListBox.Name = "filterListBox";
            this.filterListBox.Size = new System.Drawing.Size(149, 104);
            this.filterListBox.TabIndex = 2;
            this.filterListBox.SelectedIndexChanged += new System.EventHandler(this.filterListBox_SelectedIndexChanged);
            // 
            // contextMenuStripAddFilter
            // 
            this.contextMenuStripAddFilter.Name = "contextMenuStripAddFilter";
            this.contextMenuStripAddFilter.Size = new System.Drawing.Size(61, 4);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageFilter);
            this.tabControl1.Controls.Add(this.tabPageInspection);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(443, 438);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.probePosR);
            this.tabPageGeneral.Controls.Add(this.labelFiducialProbe);
            this.tabPageGeneral.Controls.Add(this.labelR);
            this.tabPageGeneral.Controls.Add(this.labelX);
            this.tabPageGeneral.Controls.Add(this.lightTypeCombo);
            this.tabPageGeneral.Controls.Add(this.labelPos);
            this.tabPageGeneral.Controls.Add(this.labelLightType);
            this.tabPageGeneral.Controls.Add(this.labelW);
            this.tabPageGeneral.Controls.Add(this.comboFiducialProbe);
            this.tabPageGeneral.Controls.Add(this.probeHeight);
            this.tabPageGeneral.Controls.Add(this.inverseResult);
            this.tabPageGeneral.Controls.Add(this.labelH);
            this.tabPageGeneral.Controls.Add(this.labelY);
            this.tabPageGeneral.Controls.Add(this.probePosY);
            this.tabPageGeneral.Controls.Add(this.probePosX);
            this.tabPageGeneral.Controls.Add(this.probeWidth);
            this.tabPageGeneral.Controls.Add(this.labelSize);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 29);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(435, 405);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // tabPageFilter
            // 
            this.tabPageFilter.Controls.Add(this.buttonFilterUp);
            this.tabPageFilter.Controls.Add(this.buttonAddFilter);
            this.tabPageFilter.Controls.Add(this.buttonFilterDown);
            this.tabPageFilter.Controls.Add(this.imageBandLabel);
            this.tabPageFilter.Controls.Add(this.buttonDeleteFilter);
            this.tabPageFilter.Controls.Add(this.imageBand);
            this.tabPageFilter.Controls.Add(this.filterListBox);
            this.tabPageFilter.Controls.Add(this.panelFilterParamControl);
            this.tabPageFilter.Location = new System.Drawing.Point(4, 22);
            this.tabPageFilter.Name = "tabPageFilter";
            this.tabPageFilter.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFilter.Size = new System.Drawing.Size(435, 407);
            this.tabPageFilter.TabIndex = 1;
            this.tabPageFilter.Text = "Filter";
            this.tabPageFilter.UseVisualStyleBackColor = true;
            // 
            // tabPageInspection
            // 
            this.tabPageInspection.Controls.Add(this.algorithmParamPanel);
            this.tabPageInspection.Location = new System.Drawing.Point(4, 29);
            this.tabPageInspection.Name = "tabPageInspection";
            this.tabPageInspection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageInspection.Size = new System.Drawing.Size(435, 405);
            this.tabPageInspection.TabIndex = 2;
            this.tabPageInspection.Text = "Inspection";
            this.tabPageInspection.UseVisualStyleBackColor = true;
            // 
            // VisionParamControl
            // 
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "VisionParamControl";
            this.Size = new System.Drawing.Size(443, 438);
            this.Load += new System.EventHandler(this.VisionParamControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.probeHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.probeWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosR)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.tabPageFilter.ResumeLayout(false);
            this.tabPageFilter.PerformLayout();
            this.tabPageInspection.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NumericUpDown probeHeight;
        private System.Windows.Forms.NumericUpDown probeWidth;
        private System.Windows.Forms.CheckBox inverseResult;
        private System.Windows.Forms.NumericUpDown probePosX;
        private System.Windows.Forms.NumericUpDown probePosY;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelPos;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelW;
        private System.Windows.Forms.Label labelH;
        private System.Windows.Forms.Panel algorithmParamPanel;
        private System.Windows.Forms.Label imageBandLabel;
        private System.Windows.Forms.ComboBox imageBand;
        private System.Windows.Forms.ComboBox comboFiducialProbe;
        private System.Windows.Forms.Label labelFiducialProbe;
        private System.Windows.Forms.ListBox filterListBox;
        private System.Windows.Forms.Panel panelFilterParamControl;
        private System.Windows.Forms.Button buttonAddFilter;
        private System.Windows.Forms.Button buttonDeleteFilter;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripAddFilter;
        private System.Windows.Forms.ComboBox lightTypeCombo;
        private System.Windows.Forms.Label labelLightType;
        private System.Windows.Forms.NumericUpDown probePosR;
        private System.Windows.Forms.Label labelR;
        private System.Windows.Forms.Button buttonFilterUp;
        private System.Windows.Forms.Button buttonFilterDown;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageFilter;
        private System.Windows.Forms.TabPage tabPageInspection;
    }
}