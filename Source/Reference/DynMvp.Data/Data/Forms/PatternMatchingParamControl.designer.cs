namespace DynMvp.Data.Forms
{
    partial class PatternMatchingParamControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.addPatternButton = new System.Windows.Forms.Button();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelH = new System.Windows.Forms.Label();
            this.labelW = new System.Windows.Forms.Label();
            this.searchRangeHeight = new System.Windows.Forms.NumericUpDown();
            this.searchRangeWidth = new System.Windows.Forms.NumericUpDown();
            this.labelScore = new System.Windows.Forms.Label();
            this.matchScore = new System.Windows.Forms.NumericUpDown();
            this.patternImageSelector = new System.Windows.Forms.DataGridView();
            this.ColumnPatternImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.refreshPatternButton = new System.Windows.Forms.Button();
            this.deletePatternButton = new System.Windows.Forms.Button();
            this.editMaskButton = new System.Windows.Forms.Button();
            this.patternType = new System.Windows.Forms.ComboBox();
            this.maxScale = new System.Windows.Forms.NumericUpDown();
            this.maxAngle = new System.Windows.Forms.NumericUpDown();
            this.minScale = new System.Windows.Forms.NumericUpDown();
            this.minAngle = new System.Windows.Forms.NumericUpDown();
            this.labelScale = new System.Windows.Forms.Label();
            this.labelAngle = new System.Windows.Forms.Label();
            this.labelScaleMax = new System.Windows.Forms.Label();
            this.labelAngleMax = new System.Windows.Forms.Label();
            this.labelScaleMin = new System.Windows.Forms.Label();
            this.labelAngleMin = new System.Windows.Forms.Label();
            this.fiducialProbe = new System.Windows.Forms.CheckBox();
            this.centerOffset = new System.Windows.Forms.CheckBox();
            this.useWholeImage = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternImageSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minScale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minAngle)).BeginInit();
            this.SuspendLayout();
            // 
            // addPatternButton
            // 
            this.addPatternButton.Location = new System.Drawing.Point(213, 8);
            this.addPatternButton.Margin = new System.Windows.Forms.Padding(4);
            this.addPatternButton.Name = "addPatternButton";
            this.addPatternButton.Size = new System.Drawing.Size(74, 27);
            this.addPatternButton.TabIndex = 1;
            this.addPatternButton.Text = "Add";
            this.addPatternButton.UseVisualStyleBackColor = true;
            this.addPatternButton.Click += new System.EventHandler(this.addPatternButton_Click);
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Location = new System.Drawing.Point(304, 7);
            this.labelSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(112, 20);
            this.labelSize.TabIndex = 6;
            this.labelSize.Text = "Search Range";
            // 
            // labelH
            // 
            this.labelH.AutoSize = true;
            this.labelH.Location = new System.Drawing.Point(310, 57);
            this.labelH.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelH.Name = "labelH";
            this.labelH.Size = new System.Drawing.Size(21, 20);
            this.labelH.TabIndex = 9;
            this.labelH.Text = "H";
            this.labelH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelW
            // 
            this.labelW.AutoSize = true;
            this.labelW.Location = new System.Drawing.Point(310, 30);
            this.labelW.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelW.Name = "labelW";
            this.labelW.Size = new System.Drawing.Size(24, 20);
            this.labelW.TabIndex = 7;
            this.labelW.Text = "W";
            this.labelW.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // searchRangeHeight
            // 
            this.searchRangeHeight.Location = new System.Drawing.Point(358, 55);
            this.searchRangeHeight.Margin = new System.Windows.Forms.Padding(4);
            this.searchRangeHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.searchRangeHeight.Name = "searchRangeHeight";
            this.searchRangeHeight.Size = new System.Drawing.Size(59, 26);
            this.searchRangeHeight.TabIndex = 10;
            this.searchRangeHeight.ValueChanged += new System.EventHandler(this.searchRangeHeight_ValueChanged);
            this.searchRangeHeight.Enter += new System.EventHandler(this.textBox_Enter);
            this.searchRangeHeight.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // searchRangeWidth
            // 
            this.searchRangeWidth.Location = new System.Drawing.Point(358, 28);
            this.searchRangeWidth.Margin = new System.Windows.Forms.Padding(4);
            this.searchRangeWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.searchRangeWidth.Name = "searchRangeWidth";
            this.searchRangeWidth.Size = new System.Drawing.Size(59, 26);
            this.searchRangeWidth.TabIndex = 8;
            this.searchRangeWidth.ValueChanged += new System.EventHandler(this.searchRangeWidth_ValueChanged);
            this.searchRangeWidth.Enter += new System.EventHandler(this.textBox_Enter);
            this.searchRangeWidth.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // labelScore
            // 
            this.labelScore.AutoSize = true;
            this.labelScore.Location = new System.Drawing.Point(293, 92);
            this.labelScore.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelScore.Name = "labelScore";
            this.labelScore.Size = new System.Drawing.Size(51, 20);
            this.labelScore.TabIndex = 11;
            this.labelScore.Text = "Score";
            // 
            // matchScore
            // 
            this.matchScore.Location = new System.Drawing.Point(358, 92);
            this.matchScore.Margin = new System.Windows.Forms.Padding(4);
            this.matchScore.Name = "matchScore";
            this.matchScore.Size = new System.Drawing.Size(59, 26);
            this.matchScore.TabIndex = 12;
            this.matchScore.ValueChanged += new System.EventHandler(this.matchScore_ValueChanged);
            this.matchScore.Enter += new System.EventHandler(this.textBox_Enter);
            this.matchScore.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // patternImageSelector
            // 
            this.patternImageSelector.AllowUserToAddRows = false;
            this.patternImageSelector.AllowUserToDeleteRows = false;
            this.patternImageSelector.AllowUserToResizeColumns = false;
            this.patternImageSelector.AllowUserToResizeRows = false;
            this.patternImageSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.patternImageSelector.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.patternImageSelector.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.patternImageSelector.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnPatternImage});
            this.patternImageSelector.Location = new System.Drawing.Point(6, 7);
            this.patternImageSelector.Margin = new System.Windows.Forms.Padding(2);
            this.patternImageSelector.MultiSelect = false;
            this.patternImageSelector.Name = "patternImageSelector";
            this.patternImageSelector.ReadOnly = true;
            this.patternImageSelector.RowHeadersVisible = false;
            this.patternImageSelector.RowTemplate.Height = 23;
            this.patternImageSelector.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.patternImageSelector.Size = new System.Drawing.Size(203, 338);
            this.patternImageSelector.TabIndex = 0;
            this.patternImageSelector.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.patternImageSelector_CellClick);
            // 
            // ColumnPatternImage
            // 
            this.ColumnPatternImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnPatternImage.HeaderText = "Pattern Image";
            this.ColumnPatternImage.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.ColumnPatternImage.Name = "ColumnPatternImage";
            this.ColumnPatternImage.ReadOnly = true;
            this.ColumnPatternImage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ColumnPatternImage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // refreshPatternButton
            // 
            this.refreshPatternButton.Location = new System.Drawing.Point(213, 36);
            this.refreshPatternButton.Margin = new System.Windows.Forms.Padding(4);
            this.refreshPatternButton.Name = "refreshPatternButton";
            this.refreshPatternButton.Size = new System.Drawing.Size(74, 27);
            this.refreshPatternButton.TabIndex = 2;
            this.refreshPatternButton.Text = "Refresh";
            this.refreshPatternButton.UseVisualStyleBackColor = true;
            this.refreshPatternButton.Click += new System.EventHandler(this.refreshPatternButton_Click);
            // 
            // deletePatternButton
            // 
            this.deletePatternButton.Location = new System.Drawing.Point(213, 63);
            this.deletePatternButton.Margin = new System.Windows.Forms.Padding(4);
            this.deletePatternButton.Name = "deletePatternButton";
            this.deletePatternButton.Size = new System.Drawing.Size(74, 27);
            this.deletePatternButton.TabIndex = 3;
            this.deletePatternButton.Text = "Delete";
            this.deletePatternButton.UseVisualStyleBackColor = true;
            this.deletePatternButton.Click += new System.EventHandler(this.deletePatternButton_Click);
            // 
            // editMaskButton
            // 
            this.editMaskButton.Location = new System.Drawing.Point(213, 92);
            this.editMaskButton.Margin = new System.Windows.Forms.Padding(4);
            this.editMaskButton.Name = "editMaskButton";
            this.editMaskButton.Size = new System.Drawing.Size(74, 27);
            this.editMaskButton.TabIndex = 4;
            this.editMaskButton.Text = "Mask";
            this.editMaskButton.UseVisualStyleBackColor = true;
            this.editMaskButton.Click += new System.EventHandler(this.editMaskButton_Click);
            // 
            // patternType
            // 
            this.patternType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.patternType.FormattingEnabled = true;
            this.patternType.Items.AddRange(new object[] {
            "Good",
            "NG"});
            this.patternType.Location = new System.Drawing.Point(213, 121);
            this.patternType.Margin = new System.Windows.Forms.Padding(2);
            this.patternType.Name = "patternType";
            this.patternType.Size = new System.Drawing.Size(74, 28);
            this.patternType.TabIndex = 5;
            this.patternType.SelectedIndexChanged += new System.EventHandler(this.patternType_SelectedIndexChanged);
            // 
            // maxScale
            // 
            this.maxScale.Location = new System.Drawing.Point(358, 244);
            this.maxScale.Margin = new System.Windows.Forms.Padding(4);
            this.maxScale.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.maxScale.Name = "maxScale";
            this.maxScale.Size = new System.Drawing.Size(59, 26);
            this.maxScale.TabIndex = 21;
            this.maxScale.ValueChanged += new System.EventHandler(this.maxScale_ValueChanged);
            // 
            // maxAngle
            // 
            this.maxAngle.Location = new System.Drawing.Point(358, 167);
            this.maxAngle.Margin = new System.Windows.Forms.Padding(4);
            this.maxAngle.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.maxAngle.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.maxAngle.Name = "maxAngle";
            this.maxAngle.Size = new System.Drawing.Size(59, 26);
            this.maxAngle.TabIndex = 22;
            this.maxAngle.ValueChanged += new System.EventHandler(this.maxAngle_ValueChanged);
            // 
            // minScale
            // 
            this.minScale.Location = new System.Drawing.Point(358, 217);
            this.minScale.Margin = new System.Windows.Forms.Padding(4);
            this.minScale.Name = "minScale";
            this.minScale.Size = new System.Drawing.Size(59, 26);
            this.minScale.TabIndex = 17;
            this.minScale.ValueChanged += new System.EventHandler(this.minScale_ValueChanged);
            // 
            // minAngle
            // 
            this.minAngle.Location = new System.Drawing.Point(358, 140);
            this.minAngle.Margin = new System.Windows.Forms.Padding(4);
            this.minAngle.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.minAngle.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.minAngle.Name = "minAngle";
            this.minAngle.Size = new System.Drawing.Size(59, 26);
            this.minAngle.TabIndex = 18;
            this.minAngle.ValueChanged += new System.EventHandler(this.minAngle_ValueChanged);
            // 
            // labelScale
            // 
            this.labelScale.AutoSize = true;
            this.labelScale.Location = new System.Drawing.Point(293, 197);
            this.labelScale.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelScale.Name = "labelScale";
            this.labelScale.Size = new System.Drawing.Size(49, 20);
            this.labelScale.TabIndex = 14;
            this.labelScale.Text = "Scale";
            // 
            // labelAngle
            // 
            this.labelAngle.AutoSize = true;
            this.labelAngle.Location = new System.Drawing.Point(293, 117);
            this.labelAngle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAngle.Name = "labelAngle";
            this.labelAngle.Size = new System.Drawing.Size(50, 20);
            this.labelAngle.TabIndex = 13;
            this.labelAngle.Text = "Angle";
            // 
            // labelScaleMax
            // 
            this.labelScaleMax.AutoSize = true;
            this.labelScaleMax.Location = new System.Drawing.Point(307, 246);
            this.labelScaleMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelScaleMax.Name = "labelScaleMax";
            this.labelScaleMax.Size = new System.Drawing.Size(38, 20);
            this.labelScaleMax.TabIndex = 20;
            this.labelScaleMax.Text = "Max";
            this.labelScaleMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAngleMax
            // 
            this.labelAngleMax.AutoSize = true;
            this.labelAngleMax.Location = new System.Drawing.Point(307, 168);
            this.labelAngleMax.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAngleMax.Name = "labelAngleMax";
            this.labelAngleMax.Size = new System.Drawing.Size(38, 20);
            this.labelAngleMax.TabIndex = 19;
            this.labelAngleMax.Text = "Max";
            this.labelAngleMax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelScaleMin
            // 
            this.labelScaleMin.AutoSize = true;
            this.labelScaleMin.Location = new System.Drawing.Point(310, 219);
            this.labelScaleMin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelScaleMin.Name = "labelScaleMin";
            this.labelScaleMin.Size = new System.Drawing.Size(34, 20);
            this.labelScaleMin.TabIndex = 16;
            this.labelScaleMin.Text = "Min";
            this.labelScaleMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAngleMin
            // 
            this.labelAngleMin.AutoSize = true;
            this.labelAngleMin.Location = new System.Drawing.Point(310, 142);
            this.labelAngleMin.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAngleMin.Name = "labelAngleMin";
            this.labelAngleMin.Size = new System.Drawing.Size(34, 20);
            this.labelAngleMin.TabIndex = 15;
            this.labelAngleMin.Text = "Min";
            this.labelAngleMin.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // fiducialProbe
            // 
            this.fiducialProbe.AutoSize = true;
            this.fiducialProbe.Location = new System.Drawing.Point(213, 153);
            this.fiducialProbe.Margin = new System.Windows.Forms.Padding(2);
            this.fiducialProbe.Name = "fiducialProbe";
            this.fiducialProbe.Size = new System.Drawing.Size(82, 24);
            this.fiducialProbe.TabIndex = 23;
            this.fiducialProbe.Text = "Fiducial";
            this.fiducialProbe.UseVisualStyleBackColor = true;
            this.fiducialProbe.CheckedChanged += new System.EventHandler(this.fiducialProbe_CheckedChanged);
            // 
            // centerOffset
            // 
            this.centerOffset.AutoSize = true;
            this.centerOffset.Location = new System.Drawing.Point(213, 176);
            this.centerOffset.Margin = new System.Windows.Forms.Padding(2);
            this.centerOffset.Name = "centerOffset";
            this.centerOffset.Size = new System.Drawing.Size(76, 24);
            this.centerOffset.TabIndex = 23;
            this.centerOffset.Text = "Center";
            this.centerOffset.UseVisualStyleBackColor = true;
            this.centerOffset.CheckedChanged += new System.EventHandler(this.centerOffset_CheckedChanged);
            // 
            // useWholeImage
            // 
            this.useWholeImage.AutoSize = true;
            this.useWholeImage.Location = new System.Drawing.Point(213, 199);
            this.useWholeImage.Name = "useWholeImage";
            this.useWholeImage.Size = new System.Drawing.Size(73, 24);
            this.useWholeImage.TabIndex = 24;
            this.useWholeImage.Text = "Whole";
            this.useWholeImage.UseVisualStyleBackColor = true;
            this.useWholeImage.CheckedChanged += new System.EventHandler(this.useWholeImage_CheckedChanged);
            // 
            // PatternMatchingParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.useWholeImage);
            this.Controls.Add(this.centerOffset);
            this.Controls.Add(this.fiducialProbe);
            this.Controls.Add(this.maxScale);
            this.Controls.Add(this.maxAngle);
            this.Controls.Add(this.minScale);
            this.Controls.Add(this.minAngle);
            this.Controls.Add(this.labelScale);
            this.Controls.Add(this.labelAngle);
            this.Controls.Add(this.labelScaleMax);
            this.Controls.Add(this.labelAngleMax);
            this.Controls.Add(this.labelScaleMin);
            this.Controls.Add(this.labelAngleMin);
            this.Controls.Add(this.patternType);
            this.Controls.Add(this.patternImageSelector);
            this.Controls.Add(this.matchScore);
            this.Controls.Add(this.searchRangeHeight);
            this.Controls.Add(this.searchRangeWidth);
            this.Controls.Add(this.labelScore);
            this.Controls.Add(this.labelSize);
            this.Controls.Add(this.labelH);
            this.Controls.Add(this.labelW);
            this.Controls.Add(this.editMaskButton);
            this.Controls.Add(this.deletePatternButton);
            this.Controls.Add(this.refreshPatternButton);
            this.Controls.Add(this.addPatternButton);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PatternMatchingParamControl";
            this.Size = new System.Drawing.Size(424, 352);
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchRangeWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.patternImageSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minScale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minAngle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addPatternButton;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelH;
        private System.Windows.Forms.Label labelW;
        private System.Windows.Forms.NumericUpDown searchRangeHeight;
        private System.Windows.Forms.NumericUpDown searchRangeWidth;
        private System.Windows.Forms.Label labelScore;
        private System.Windows.Forms.NumericUpDown matchScore;
        private System.Windows.Forms.DataGridView patternImageSelector;
        private System.Windows.Forms.Button refreshPatternButton;
        private System.Windows.Forms.Button deletePatternButton;
        private System.Windows.Forms.Button editMaskButton;
        private System.Windows.Forms.ComboBox patternType;
        private System.Windows.Forms.DataGridViewImageColumn ColumnPatternImage;
        private System.Windows.Forms.NumericUpDown maxScale;
        private System.Windows.Forms.NumericUpDown maxAngle;
        private System.Windows.Forms.NumericUpDown minScale;
        private System.Windows.Forms.NumericUpDown minAngle;
        private System.Windows.Forms.Label labelScale;
        private System.Windows.Forms.Label labelAngle;
        private System.Windows.Forms.Label labelScaleMax;
        private System.Windows.Forms.Label labelAngleMax;
        private System.Windows.Forms.Label labelScaleMin;
        private System.Windows.Forms.Label labelAngleMin;
        private System.Windows.Forms.CheckBox fiducialProbe;
        private System.Windows.Forms.CheckBox centerOffset;
        private System.Windows.Forms.CheckBox useWholeImage;
    }
}