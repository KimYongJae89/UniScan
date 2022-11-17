namespace UniScanX.MPAlignment.Algo.UI
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
            this.useHistogramEqualization = new System.Windows.Forms.CheckBox();
            this.useEdgeExtraction = new System.Windows.Forms.CheckBox();
            this.stepBlocker = new System.Windows.Forms.CheckBox();
            this.numDilate = new System.Windows.Forms.NumericUpDown();
            this.numErode = new System.Windows.Forms.NumericUpDown();
            this.lableDilate = new System.Windows.Forms.Label();
            this.labelErode = new System.Windows.Forms.Label();
            this.imageBand = new System.Windows.Forms.ComboBox();
            this.modelVerification = new System.Windows.Forms.CheckBox();
            this.useBinarization = new System.Windows.Forms.CheckBox();
            this.labelTilda = new System.Windows.Forms.Label();
            this.radioAutoThreshold = new System.Windows.Forms.RadioButton();
            this.radioDoubleThreshold = new System.Windows.Forms.RadioButton();
            this.radioSingleThreshold = new System.Windows.Forms.RadioButton();
            this.thresholdUpper = new System.Windows.Forms.NumericUpDown();
            this.inverseResult = new System.Windows.Forms.CheckBox();
            this.thresholdLower = new System.Windows.Forms.NumericUpDown();
            this.probeHeight = new System.Windows.Forms.NumericUpDown();
            this.probeWidth = new System.Windows.Forms.NumericUpDown();
            this.probePosX = new System.Windows.Forms.NumericUpDown();
            this.probePosY = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelPos = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.labelW = new System.Windows.Forms.Label();
            this.labelH = new System.Windows.Forms.Label();
            this.panelCommonParam = new System.Windows.Forms.Panel();
            this.lblProbeId = new System.Windows.Forms.Label();
            this.probeId = new System.Windows.Forms.Label();
            this.probeType = new System.Windows.Forms.Label();
            this.algorithmParamPanel = new System.Windows.Forms.Panel();
            this.panelPosition = new System.Windows.Forms.Panel();
            this.probeAngle = new System.Windows.Forms.NumericUpDown();
            this.labelR = new System.Windows.Forms.Label();
            this.stepBlockType = new System.Windows.Forms.ComboBox();
            this.panelOption = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numDilate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numErode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdLower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.probeHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.probeWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosY)).BeginInit();
            this.panelCommonParam.SuspendLayout();
            this.panelPosition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.probeAngle)).BeginInit();
            this.panelOption.SuspendLayout();
            this.SuspendLayout();
            // 
            // useHistogramEqualization
            // 
            this.useHistogramEqualization.AutoSize = true;
            this.useHistogramEqualization.Enabled = false;
            this.useHistogramEqualization.Location = new System.Drawing.Point(9, 64);
            this.useHistogramEqualization.Name = "useHistogramEqualization";
            this.useHistogramEqualization.Size = new System.Drawing.Size(158, 25);
            this.useHistogramEqualization.TabIndex = 2;
            this.useHistogramEqualization.Text = "Adjust Brightness";
            this.useHistogramEqualization.UseVisualStyleBackColor = true;
            this.useHistogramEqualization.Visible = false;
            this.useHistogramEqualization.CheckedChanged += new System.EventHandler(this.useHistogramEqualization_CheckedChanged);
            // 
            // useEdgeExtraction
            // 
            this.useEdgeExtraction.AutoSize = true;
            this.useEdgeExtraction.Location = new System.Drawing.Point(199, 64);
            this.useEdgeExtraction.Name = "useEdgeExtraction";
            this.useEdgeExtraction.Size = new System.Drawing.Size(144, 25);
            this.useEdgeExtraction.TabIndex = 4;
            this.useEdgeExtraction.Text = "Edge Extraction";
            this.useEdgeExtraction.UseVisualStyleBackColor = true;
            this.useEdgeExtraction.CheckedChanged += new System.EventHandler(this.useEdgeExtraction_CheckedChanged);
            // 
            // stepBlocker
            // 
            this.stepBlocker.AutoSize = true;
            this.stepBlocker.Location = new System.Drawing.Point(9, 118);
            this.stepBlocker.Name = "stepBlocker";
            this.stepBlocker.Size = new System.Drawing.Size(123, 25);
            this.stepBlocker.TabIndex = 17;
            this.stepBlocker.Text = "Step Blocker";
            this.stepBlocker.UseVisualStyleBackColor = true;
            this.stepBlocker.Visible = false;
            this.stepBlocker.CheckedChanged += new System.EventHandler(this.stepBlocker_CheckedChanged);
            // 
            // numDilate
            // 
            this.numDilate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numDilate.Location = new System.Drawing.Point(628, 121);
            this.numDilate.Name = "numDilate";
            this.numDilate.Size = new System.Drawing.Size(75, 29);
            this.numDilate.TabIndex = 16;
            this.numDilate.ValueChanged += new System.EventHandler(this.numDilate_ValueChanged);
            // 
            // numErode
            // 
            this.numErode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numErode.Location = new System.Drawing.Point(473, 121);
            this.numErode.Name = "numErode";
            this.numErode.Size = new System.Drawing.Size(75, 29);
            this.numErode.TabIndex = 16;
            this.numErode.ValueChanged += new System.EventHandler(this.numErode_ValueChanged);
            // 
            // lableDilate
            // 
            this.lableDilate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lableDilate.AutoSize = true;
            this.lableDilate.Location = new System.Drawing.Point(570, 123);
            this.lableDilate.Name = "lableDilate";
            this.lableDilate.Size = new System.Drawing.Size(52, 21);
            this.lableDilate.TabIndex = 15;
            this.lableDilate.Text = "Dilate";
            // 
            // labelErode
            // 
            this.labelErode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelErode.AutoSize = true;
            this.labelErode.Location = new System.Drawing.Point(414, 121);
            this.labelErode.Name = "labelErode";
            this.labelErode.Size = new System.Drawing.Size(53, 21);
            this.labelErode.TabIndex = 14;
            this.labelErode.Text = "Erode";
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
            this.imageBand.Location = new System.Drawing.Point(152, 27);
            this.imageBand.Name = "imageBand";
            this.imageBand.Size = new System.Drawing.Size(196, 29);
            this.imageBand.TabIndex = 1;
            this.imageBand.SelectedIndexChanged += new System.EventHandler(this.colorBand_SelectedIndexChanged);
            // 
            // modelVerification
            // 
            this.modelVerification.AutoSize = true;
            this.modelVerification.Location = new System.Drawing.Point(199, 89);
            this.modelVerification.Name = "modelVerification";
            this.modelVerification.Size = new System.Drawing.Size(169, 25);
            this.modelVerification.TabIndex = 6;
            this.modelVerification.Text = "Use Model Verifier";
            this.modelVerification.UseVisualStyleBackColor = true;
            this.modelVerification.Visible = false;
            this.modelVerification.CheckedChanged += new System.EventHandler(this.modelVerification_CheckedChanged);
            // 
            // useBinarization
            // 
            this.useBinarization.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.useBinarization.AutoSize = true;
            this.useBinarization.Location = new System.Drawing.Point(473, 31);
            this.useBinarization.Name = "useBinarization";
            this.useBinarization.Size = new System.Drawing.Size(113, 25);
            this.useBinarization.TabIndex = 7;
            this.useBinarization.Text = "Binarization";
            this.useBinarization.UseVisualStyleBackColor = true;
            this.useBinarization.CheckedChanged += new System.EventHandler(this.useBinarization_CheckedChanged);
            // 
            // labelTilda
            // 
            this.labelTilda.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTilda.AutoSize = true;
            this.labelTilda.Location = new System.Drawing.Point(576, 89);
            this.labelTilda.Name = "labelTilda";
            this.labelTilda.Size = new System.Drawing.Size(21, 21);
            this.labelTilda.TabIndex = 13;
            this.labelTilda.Text = "~";
            // 
            // radioAutoThreshold
            // 
            this.radioAutoThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioAutoThreshold.AutoSize = true;
            this.radioAutoThreshold.Enabled = false;
            this.radioAutoThreshold.Location = new System.Drawing.Point(637, 50);
            this.radioAutoThreshold.Name = "radioAutoThreshold";
            this.radioAutoThreshold.Size = new System.Drawing.Size(64, 25);
            this.radioAutoThreshold.TabIndex = 10;
            this.radioAutoThreshold.TabStop = true;
            this.radioAutoThreshold.Text = "Auto";
            this.radioAutoThreshold.UseVisualStyleBackColor = true;
            this.radioAutoThreshold.Visible = false;
            this.radioAutoThreshold.CheckedChanged += new System.EventHandler(this.radioAutoThreshold_CheckedChanged);
            // 
            // radioDoubleThreshold
            // 
            this.radioDoubleThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioDoubleThreshold.AutoSize = true;
            this.radioDoubleThreshold.Enabled = false;
            this.radioDoubleThreshold.Location = new System.Drawing.Point(554, 50);
            this.radioDoubleThreshold.Name = "radioDoubleThreshold";
            this.radioDoubleThreshold.Size = new System.Drawing.Size(81, 25);
            this.radioDoubleThreshold.TabIndex = 9;
            this.radioDoubleThreshold.TabStop = true;
            this.radioDoubleThreshold.Text = "Double";
            this.radioDoubleThreshold.UseVisualStyleBackColor = true;
            this.radioDoubleThreshold.CheckedChanged += new System.EventHandler(this.radioDoubleThreshold_CheckedChanged);
            // 
            // radioSingleThreshold
            // 
            this.radioSingleThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioSingleThreshold.AutoSize = true;
            this.radioSingleThreshold.Location = new System.Drawing.Point(475, 51);
            this.radioSingleThreshold.Name = "radioSingleThreshold";
            this.radioSingleThreshold.Size = new System.Drawing.Size(73, 25);
            this.radioSingleThreshold.TabIndex = 8;
            this.radioSingleThreshold.TabStop = true;
            this.radioSingleThreshold.Text = "Single";
            this.radioSingleThreshold.UseVisualStyleBackColor = true;
            this.radioSingleThreshold.CheckedChanged += new System.EventHandler(this.radioSingleThreshold_CheckedChanged);
            // 
            // thresholdUpper
            // 
            this.thresholdUpper.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thresholdUpper.Enabled = false;
            this.thresholdUpper.Location = new System.Drawing.Point(626, 81);
            this.thresholdUpper.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.thresholdUpper.Name = "thresholdUpper";
            this.thresholdUpper.Size = new System.Drawing.Size(75, 29);
            this.thresholdUpper.TabIndex = 0;
            this.thresholdUpper.ValueChanged += new System.EventHandler(this.thresholdUpper_ValueChanged);
            this.thresholdUpper.Enter += new System.EventHandler(this.textBox_Enter);
            this.thresholdUpper.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // inverseResult
            // 
            this.inverseResult.AutoSize = true;
            this.inverseResult.Location = new System.Drawing.Point(9, 89);
            this.inverseResult.Name = "inverseResult";
            this.inverseResult.Size = new System.Drawing.Size(132, 25);
            this.inverseResult.TabIndex = 5;
            this.inverseResult.Text = "Inverse Result";
            this.inverseResult.UseVisualStyleBackColor = true;
            this.inverseResult.CheckedChanged += new System.EventHandler(this.inverseResult_CheckedChanged);
            // 
            // thresholdLower
            // 
            this.thresholdLower.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.thresholdLower.Location = new System.Drawing.Point(473, 81);
            this.thresholdLower.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.thresholdLower.Name = "thresholdLower";
            this.thresholdLower.Size = new System.Drawing.Size(75, 29);
            this.thresholdLower.TabIndex = 12;
            this.thresholdLower.ValueChanged += new System.EventHandler(this.thresholdLower_ValueChanged);
            this.thresholdLower.Enter += new System.EventHandler(this.textBox_Enter);
            this.thresholdLower.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // probeHeight
            // 
            this.probeHeight.Location = new System.Drawing.Point(498, 10);
            this.probeHeight.Name = "probeHeight";
            this.probeHeight.Size = new System.Drawing.Size(87, 29);
            this.probeHeight.TabIndex = 9;
            this.probeHeight.ValueChanged += new System.EventHandler(this.probeHeight_ValueChanged);
            this.probeHeight.Enter += new System.EventHandler(this.textBox_Enter);
            this.probeHeight.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // probeWidth
            // 
            this.probeWidth.Location = new System.Drawing.Point(366, 10);
            this.probeWidth.Name = "probeWidth";
            this.probeWidth.Size = new System.Drawing.Size(87, 29);
            this.probeWidth.TabIndex = 7;
            this.probeWidth.ValueChanged += new System.EventHandler(this.probeWidth_ValueChanged);
            this.probeWidth.Enter += new System.EventHandler(this.textBox_Enter);
            this.probeWidth.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // probePosX
            // 
            this.probePosX.Location = new System.Drawing.Point(112, 10);
            this.probePosX.Name = "probePosX";
            this.probePosX.Size = new System.Drawing.Size(82, 29);
            this.probePosX.TabIndex = 2;
            this.probePosX.ValueChanged += new System.EventHandler(this.probePosX_ValueChanged);
            this.probePosX.Enter += new System.EventHandler(this.textBox_Enter);
            this.probePosX.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // probePosY
            // 
            this.probePosY.Location = new System.Drawing.Point(239, 10);
            this.probePosY.Name = "probePosY";
            this.probePosY.Size = new System.Drawing.Size(82, 29);
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
            this.labelPos.Location = new System.Drawing.Point(3, 14);
            this.labelPos.Name = "labelPos";
            this.labelPos.Size = new System.Drawing.Size(69, 21);
            this.labelPos.TabIndex = 0;
            this.labelPos.Text = "Position";
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(86, 13);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(20, 21);
            this.labelX.TabIndex = 1;
            this.labelX.Text = "X";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(214, 13);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(19, 21);
            this.labelY.TabIndex = 3;
            this.labelY.Text = "Y";
            // 
            // labelW
            // 
            this.labelW.AutoSize = true;
            this.labelW.Location = new System.Drawing.Point(338, 13);
            this.labelW.Name = "labelW";
            this.labelW.Size = new System.Drawing.Size(25, 21);
            this.labelW.TabIndex = 6;
            this.labelW.Text = "W";
            // 
            // labelH
            // 
            this.labelH.AutoSize = true;
            this.labelH.Location = new System.Drawing.Point(474, 13);
            this.labelH.Name = "labelH";
            this.labelH.Size = new System.Drawing.Size(22, 21);
            this.labelH.TabIndex = 8;
            this.labelH.Text = "H";
            // 
            // panelCommonParam
            // 
            this.panelCommonParam.BackColor = System.Drawing.Color.SlateGray;
            this.panelCommonParam.Controls.Add(this.lblProbeId);
            this.panelCommonParam.Controls.Add(this.probeId);
            this.panelCommonParam.Controls.Add(this.probeType);
            this.panelCommonParam.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCommonParam.Location = new System.Drawing.Point(0, 0);
            this.panelCommonParam.Name = "panelCommonParam";
            this.panelCommonParam.Size = new System.Drawing.Size(730, 45);
            this.panelCommonParam.TabIndex = 0;
            // 
            // lblProbeId
            // 
            this.lblProbeId.AutoSize = true;
            this.lblProbeId.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblProbeId.Location = new System.Drawing.Point(12, 10);
            this.lblProbeId.Name = "lblProbeId";
            this.lblProbeId.Size = new System.Drawing.Size(65, 25);
            this.lblProbeId.TabIndex = 15;
            this.lblProbeId.Text = "Probe";
            // 
            // probeId
            // 
            this.probeId.BackColor = System.Drawing.Color.Transparent;
            this.probeId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.probeId.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.probeId.Location = new System.Drawing.Point(90, 11);
            this.probeId.Margin = new System.Windows.Forms.Padding(0);
            this.probeId.Name = "probeId";
            this.probeId.Size = new System.Drawing.Size(101, 23);
            this.probeId.TabIndex = 16;
            this.probeId.Text = "ID";
            this.probeId.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // probeType
            // 
            this.probeType.BackColor = System.Drawing.Color.Transparent;
            this.probeType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.probeType.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.probeType.Location = new System.Drawing.Point(197, 11);
            this.probeType.Margin = new System.Windows.Forms.Padding(0);
            this.probeType.Name = "probeType";
            this.probeType.Size = new System.Drawing.Size(124, 24);
            this.probeType.TabIndex = 17;
            this.probeType.Text = "Type";
            this.probeType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // algorithmParamPanel
            // 
            this.algorithmParamPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.algorithmParamPanel.Location = new System.Drawing.Point(0, 255);
            this.algorithmParamPanel.Name = "algorithmParamPanel";
            this.algorithmParamPanel.Size = new System.Drawing.Size(730, 290);
            this.algorithmParamPanel.TabIndex = 1;
            // 
            // panelPosition
            // 
            this.panelPosition.Controls.Add(this.probeAngle);
            this.panelPosition.Controls.Add(this.labelR);
            this.panelPosition.Controls.Add(this.probePosX);
            this.panelPosition.Controls.Add(this.labelPos);
            this.panelPosition.Controls.Add(this.probePosY);
            this.panelPosition.Controls.Add(this.labelX);
            this.panelPosition.Controls.Add(this.probeWidth);
            this.panelPosition.Controls.Add(this.labelW);
            this.panelPosition.Controls.Add(this.probeHeight);
            this.panelPosition.Controls.Add(this.labelH);
            this.panelPosition.Controls.Add(this.labelY);
            this.panelPosition.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPosition.Location = new System.Drawing.Point(0, 45);
            this.panelPosition.Name = "panelPosition";
            this.panelPosition.Size = new System.Drawing.Size(730, 49);
            this.panelPosition.TabIndex = 2;
            // 
            // probeAngle
            // 
            this.probeAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.probeAngle.DecimalPlaces = 2;
            this.probeAngle.Location = new System.Drawing.Point(630, 10);
            this.probeAngle.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.probeAngle.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.probeAngle.Name = "probeAngle";
            this.probeAngle.Size = new System.Drawing.Size(82, 29);
            this.probeAngle.TabIndex = 12;
            this.probeAngle.ThousandsSeparator = true;
            this.probeAngle.ValueChanged += new System.EventHandler(this.probeAngle_ValueChanged);
            // 
            // labelR
            // 
            this.labelR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelR.AutoSize = true;
            this.labelR.Location = new System.Drawing.Point(609, 13);
            this.labelR.Name = "labelR";
            this.labelR.Size = new System.Drawing.Size(20, 21);
            this.labelR.TabIndex = 10;
            this.labelR.Text = "R";
            // 
            // stepBlockType
            // 
            this.stepBlockType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stepBlockType.FormattingEnabled = true;
            this.stepBlockType.Items.AddRange(new object[] {
            "NG",
            "Align"});
            this.stepBlockType.Location = new System.Drawing.Point(199, 118);
            this.stepBlockType.Name = "stepBlockType";
            this.stepBlockType.Size = new System.Drawing.Size(105, 29);
            this.stepBlockType.TabIndex = 19;
            this.stepBlockType.Visible = false;
            this.stepBlockType.SelectedIndexChanged += new System.EventHandler(this.stepBlockType_SelectedIndexChanged);
            // 
            // panelOption
            // 
            this.panelOption.Controls.Add(this.label1);
            this.panelOption.Controls.Add(this.stepBlockType);
            this.panelOption.Controls.Add(this.stepBlocker);
            this.panelOption.Controls.Add(this.imageBand);
            this.panelOption.Controls.Add(this.labelTilda);
            this.panelOption.Controls.Add(this.numDilate);
            this.panelOption.Controls.Add(this.radioAutoThreshold);
            this.panelOption.Controls.Add(this.useHistogramEqualization);
            this.panelOption.Controls.Add(this.useBinarization);
            this.panelOption.Controls.Add(this.numErode);
            this.panelOption.Controls.Add(this.radioDoubleThreshold);
            this.panelOption.Controls.Add(this.useEdgeExtraction);
            this.panelOption.Controls.Add(this.modelVerification);
            this.panelOption.Controls.Add(this.lableDilate);
            this.panelOption.Controls.Add(this.radioSingleThreshold);
            this.panelOption.Controls.Add(this.thresholdLower);
            this.panelOption.Controls.Add(this.thresholdUpper);
            this.panelOption.Controls.Add(this.labelErode);
            this.panelOption.Controls.Add(this.inverseResult);
            this.panelOption.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelOption.ForeColor = System.Drawing.Color.White;
            this.panelOption.Location = new System.Drawing.Point(0, 94);
            this.panelOption.Name = "panelOption";
            this.panelOption.Size = new System.Drawing.Size(730, 161);
            this.panelOption.TabIndex = 3;
            this.panelOption.TabStop = false;
            this.panelOption.Text = "Inspection Option";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 21);
            this.label1.TabIndex = 20;
            this.label1.Text = "Image Band";
            // 
            // VisionParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.Controls.Add(this.algorithmParamPanel);
            this.Controls.Add(this.panelOption);
            this.Controls.Add(this.panelPosition);
            this.Controls.Add(this.panelCommonParam);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Name = "VisionParamControl";
            this.Size = new System.Drawing.Size(730, 545);
            ((System.ComponentModel.ISupportInitialize)(this.numDilate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numErode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thresholdLower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.probeHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.probeWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.probePosY)).EndInit();
            this.panelCommonParam.ResumeLayout(false);
            this.panelCommonParam.PerformLayout();
            this.panelPosition.ResumeLayout(false);
            this.panelPosition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.probeAngle)).EndInit();
            this.panelOption.ResumeLayout(false);
            this.panelOption.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox useHistogramEqualization;
        private System.Windows.Forms.CheckBox useEdgeExtraction;
        private System.Windows.Forms.NumericUpDown probeHeight;
        private System.Windows.Forms.NumericUpDown probeWidth;
        private System.Windows.Forms.CheckBox inverseResult;
        private System.Windows.Forms.NumericUpDown probePosX;
        private System.Windows.Forms.NumericUpDown probePosY;
        private System.Windows.Forms.Label labelTilda;
        private System.Windows.Forms.RadioButton radioDoubleThreshold;
        private System.Windows.Forms.RadioButton radioSingleThreshold;
        private System.Windows.Forms.NumericUpDown thresholdUpper;
        private System.Windows.Forms.NumericUpDown thresholdLower;
        private System.Windows.Forms.CheckBox useBinarization;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox modelVerification;
        private System.Windows.Forms.RadioButton radioAutoThreshold;
        private System.Windows.Forms.Label labelPos;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelW;
        private System.Windows.Forms.Label labelH;
        private System.Windows.Forms.Panel panelCommonParam;
        private System.Windows.Forms.Panel algorithmParamPanel;
        private System.Windows.Forms.ComboBox imageBand;
        private System.Windows.Forms.Label lableDilate;
        private System.Windows.Forms.Label labelErode;
        private System.Windows.Forms.NumericUpDown numDilate;
        private System.Windows.Forms.NumericUpDown numErode;
        private System.Windows.Forms.CheckBox stepBlocker;
        private System.Windows.Forms.Panel panelPosition;
        private System.Windows.Forms.ComboBox stepBlockType;
        private System.Windows.Forms.Label labelR;
        private System.Windows.Forms.NumericUpDown probeAngle;
        private System.Windows.Forms.GroupBox panelOption;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblProbeId;
        private System.Windows.Forms.Label probeId;
        private System.Windows.Forms.Label probeType;
    }
}