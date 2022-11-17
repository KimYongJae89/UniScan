namespace DynMvp.Data.Forms
{
    partial class BinaryCounterParamControl
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
            this.labelPixelRatio = new System.Windows.Forms.Label();
            this.labelPercentage = new System.Windows.Forms.Label();
            this.minPixelRatio = new System.Windows.Forms.NumericUpDown();
            this.tapeInspection = new System.Windows.Forms.CheckBox();
            this.radioCountWhitePixel = new System.Windows.Forms.RadioButton();
            this.radioCountBlackPixel = new System.Windows.Forms.RadioButton();
            this.radioCountGreyPixel = new System.Windows.Forms.RadioButton();
            this.labelGridAcceptance = new System.Windows.Forms.GroupBox();
            this.useGrid = new System.Windows.Forms.CheckBox();
            this.gridRowCount = new System.Windows.Forms.NumericUpDown();
            this.gridCalcType = new System.Windows.Forms.ComboBox();
            this.labelGridRow = new System.Windows.Forms.Label();
            this.gridColumnCount = new System.Windows.Forms.NumericUpDown();
            this.labelGridColumn = new System.Windows.Forms.Label();
            this.gridScore = new System.Windows.Forms.NumericUpDown();
            this.labelGridScore = new System.Windows.Forms.Label();
            this.maxPixelRatio = new System.Windows.Forms.NumericUpDown();
            this.labelTilda = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.minPixelRatio)).BeginInit();
            this.labelGridAcceptance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRowCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridColumnCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxPixelRatio)).BeginInit();
            this.SuspendLayout();
            // 
            // labelPixelRatio
            // 
            this.labelPixelRatio.AutoSize = true;
            this.labelPixelRatio.Location = new System.Drawing.Point(4, 66);
            this.labelPixelRatio.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPixelRatio.Name = "labelPixelRatio";
            this.labelPixelRatio.Size = new System.Drawing.Size(81, 20);
            this.labelPixelRatio.TabIndex = 3;
            this.labelPixelRatio.Text = "Pixel Ratio";
            // 
            // labelPercentage
            // 
            this.labelPercentage.AutoSize = true;
            this.labelPercentage.Location = new System.Drawing.Point(272, 72);
            this.labelPercentage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPercentage.Name = "labelPercentage";
            this.labelPercentage.Size = new System.Drawing.Size(22, 20);
            this.labelPercentage.TabIndex = 5;
            this.labelPercentage.Text = "%";
            // 
            // minPixelRatio
            // 
            this.minPixelRatio.Location = new System.Drawing.Point(96, 65);
            this.minPixelRatio.Margin = new System.Windows.Forms.Padding(7);
            this.minPixelRatio.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.minPixelRatio.Name = "minPixelRatio";
            this.minPixelRatio.Size = new System.Drawing.Size(63, 27);
            this.minPixelRatio.TabIndex = 4;
            this.minPixelRatio.ValueChanged += new System.EventHandler(this.minPixelRatio_ValueChanged);
            this.minPixelRatio.Enter += new System.EventHandler(this.textBox_Enter);
            this.minPixelRatio.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // tapeInspection
            // 
            this.tapeInspection.AutoSize = true;
            this.tapeInspection.Location = new System.Drawing.Point(9, 195);
            this.tapeInspection.Margin = new System.Windows.Forms.Padding(2);
            this.tapeInspection.Name = "tapeInspection";
            this.tapeInspection.Size = new System.Drawing.Size(61, 24);
            this.tapeInspection.TabIndex = 6;
            this.tapeInspection.Text = "Tape";
            this.tapeInspection.UseVisualStyleBackColor = true;
            this.tapeInspection.CheckedChanged += new System.EventHandler(this.tapeInspection_CheckedChanged);
            // 
            // radioCountWhitePixel
            // 
            this.radioCountWhitePixel.AutoSize = true;
            this.radioCountWhitePixel.Location = new System.Drawing.Point(12, 14);
            this.radioCountWhitePixel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioCountWhitePixel.Name = "radioCountWhitePixel";
            this.radioCountWhitePixel.Size = new System.Drawing.Size(151, 24);
            this.radioCountWhitePixel.TabIndex = 7;
            this.radioCountWhitePixel.TabStop = true;
            this.radioCountWhitePixel.Text = "Count White Pixel";
            this.radioCountWhitePixel.UseVisualStyleBackColor = true;
            this.radioCountWhitePixel.CheckedChanged += new System.EventHandler(this.radioCountWhitePixel_CheckedChanged);
            // 
            // radioCountBlackPixel
            // 
            this.radioCountBlackPixel.AutoSize = true;
            this.radioCountBlackPixel.Location = new System.Drawing.Point(188, 14);
            this.radioCountBlackPixel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioCountBlackPixel.Name = "radioCountBlackPixel";
            this.radioCountBlackPixel.Size = new System.Drawing.Size(147, 24);
            this.radioCountBlackPixel.TabIndex = 8;
            this.radioCountBlackPixel.TabStop = true;
            this.radioCountBlackPixel.Text = "Count Black Pixel";
            this.radioCountBlackPixel.UseVisualStyleBackColor = true;
            this.radioCountBlackPixel.CheckedChanged += new System.EventHandler(this.radioCountBlackPixel_CheckedChanged);
            // 
            // radioCountGreyPixel
            // 
            this.radioCountGreyPixel.AutoSize = true;
            this.radioCountGreyPixel.Location = new System.Drawing.Point(12, 35);
            this.radioCountGreyPixel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.radioCountGreyPixel.Name = "radioCountGreyPixel";
            this.radioCountGreyPixel.Size = new System.Drawing.Size(142, 24);
            this.radioCountGreyPixel.TabIndex = 9;
            this.radioCountGreyPixel.TabStop = true;
            this.radioCountGreyPixel.Text = "Count Grey Pixel";
            this.radioCountGreyPixel.UseVisualStyleBackColor = true;
            this.radioCountGreyPixel.CheckedChanged += new System.EventHandler(this.radioCountGreyPixel_CheckedChanged);
            // 
            // labelGridAcceptance
            // 
            this.labelGridAcceptance.Controls.Add(this.useGrid);
            this.labelGridAcceptance.Controls.Add(this.gridRowCount);
            this.labelGridAcceptance.Controls.Add(this.gridCalcType);
            this.labelGridAcceptance.Controls.Add(this.labelGridRow);
            this.labelGridAcceptance.Controls.Add(this.gridColumnCount);
            this.labelGridAcceptance.Controls.Add(this.labelGridColumn);
            this.labelGridAcceptance.Controls.Add(this.gridScore);
            this.labelGridAcceptance.Controls.Add(this.labelGridScore);
            this.labelGridAcceptance.Location = new System.Drawing.Point(4, 95);
            this.labelGridAcceptance.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelGridAcceptance.Name = "labelGridAcceptance";
            this.labelGridAcceptance.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelGridAcceptance.Size = new System.Drawing.Size(357, 94);
            this.labelGridAcceptance.TabIndex = 10;
            this.labelGridAcceptance.TabStop = false;
            this.labelGridAcceptance.Text = "groupBox1";
            // 
            // useGrid
            // 
            this.useGrid.AutoSize = true;
            this.useGrid.Location = new System.Drawing.Point(6, 0);
            this.useGrid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.useGrid.Name = "useGrid";
            this.useGrid.Size = new System.Drawing.Size(87, 24);
            this.useGrid.TabIndex = 22;
            this.useGrid.Text = "Use Grid";
            this.useGrid.UseVisualStyleBackColor = true;
            this.useGrid.CheckedChanged += new System.EventHandler(this.useGrid_CheckedChanged);
            // 
            // gridRowCount
            // 
            this.gridRowCount.Location = new System.Drawing.Point(78, 25);
            this.gridRowCount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridRowCount.Name = "gridRowCount";
            this.gridRowCount.Size = new System.Drawing.Size(76, 27);
            this.gridRowCount.TabIndex = 20;
            this.gridRowCount.ValueChanged += new System.EventHandler(this.gridRowCount_ValueChanged);
            // 
            // gridCalcType
            // 
            this.gridCalcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gridCalcType.FormattingEnabled = true;
            this.gridCalcType.Items.AddRange(new object[] {
            "Ratio",
            "Count"});
            this.gridCalcType.Location = new System.Drawing.Point(180, 58);
            this.gridCalcType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridCalcType.Name = "gridCalcType";
            this.gridCalcType.Size = new System.Drawing.Size(90, 28);
            this.gridCalcType.TabIndex = 21;
            this.gridCalcType.SelectedIndexChanged += new System.EventHandler(this.gridCalcType_SelectedIndexChanged);
            // 
            // labelGridRow
            // 
            this.labelGridRow.AutoSize = true;
            this.labelGridRow.Location = new System.Drawing.Point(8, 26);
            this.labelGridRow.Name = "labelGridRow";
            this.labelGridRow.Size = new System.Drawing.Size(38, 20);
            this.labelGridRow.TabIndex = 17;
            this.labelGridRow.Text = "Row";
            // 
            // gridColumnCount
            // 
            this.gridColumnCount.Location = new System.Drawing.Point(272, 25);
            this.gridColumnCount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridColumnCount.Name = "gridColumnCount";
            this.gridColumnCount.Size = new System.Drawing.Size(76, 27);
            this.gridColumnCount.TabIndex = 18;
            this.gridColumnCount.ValueChanged += new System.EventHandler(this.gridColumnCount_ValueChanged);
            // 
            // labelGridColumn
            // 
            this.labelGridColumn.AutoSize = true;
            this.labelGridColumn.Location = new System.Drawing.Point(186, 26);
            this.labelGridColumn.Name = "labelGridColumn";
            this.labelGridColumn.Size = new System.Drawing.Size(63, 20);
            this.labelGridColumn.TabIndex = 16;
            this.labelGridColumn.Text = "Column";
            // 
            // gridScore
            // 
            this.gridScore.Location = new System.Drawing.Point(272, 58);
            this.gridScore.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.gridScore.Name = "gridScore";
            this.gridScore.Size = new System.Drawing.Size(76, 27);
            this.gridScore.TabIndex = 19;
            this.gridScore.ValueChanged += new System.EventHandler(this.gridScore_ValueChanged);
            // 
            // labelGridScore
            // 
            this.labelGridScore.AutoSize = true;
            this.labelGridScore.Location = new System.Drawing.Point(8, 60);
            this.labelGridScore.Name = "labelGridScore";
            this.labelGridScore.Size = new System.Drawing.Size(46, 20);
            this.labelGridScore.TabIndex = 15;
            this.labelGridScore.Text = "Score";
            // 
            // maxPixelRatio
            // 
            this.maxPixelRatio.Location = new System.Drawing.Point(194, 65);
            this.maxPixelRatio.Margin = new System.Windows.Forms.Padding(7);
            this.maxPixelRatio.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.maxPixelRatio.Name = "maxPixelRatio";
            this.maxPixelRatio.Size = new System.Drawing.Size(63, 27);
            this.maxPixelRatio.TabIndex = 4;
            this.maxPixelRatio.ValueChanged += new System.EventHandler(this.maxPixelRatio_ValueChanged);
            this.maxPixelRatio.Enter += new System.EventHandler(this.textBox_Enter);
            this.maxPixelRatio.Leave += new System.EventHandler(this.textBox_Leave);
            // 
            // labelTilda
            // 
            this.labelTilda.AutoSize = true;
            this.labelTilda.Location = new System.Drawing.Point(168, 74);
            this.labelTilda.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTilda.Name = "labelTilda";
            this.labelTilda.Size = new System.Drawing.Size(20, 20);
            this.labelTilda.TabIndex = 5;
            this.labelTilda.Text = "~";
            // 
            // BinaryCounterParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.Controls.Add(this.labelGridAcceptance);
            this.Controls.Add(this.radioCountGreyPixel);
            this.Controls.Add(this.radioCountBlackPixel);
            this.Controls.Add(this.radioCountWhitePixel);
            this.Controls.Add(this.tapeInspection);
            this.Controls.Add(this.maxPixelRatio);
            this.Controls.Add(this.minPixelRatio);
            this.Controls.Add(this.labelTilda);
            this.Controls.Add(this.labelPercentage);
            this.Controls.Add(this.labelPixelRatio);
            this.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "BinaryCounterParamControl";
            this.Size = new System.Drawing.Size(364, 279);
            ((System.ComponentModel.ISupportInitialize)(this.minPixelRatio)).EndInit();
            this.labelGridAcceptance.ResumeLayout(false);
            this.labelGridAcceptance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRowCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridColumnCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxPixelRatio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelPixelRatio;
        private System.Windows.Forms.Label labelPercentage;
        private System.Windows.Forms.NumericUpDown minPixelRatio;
        private System.Windows.Forms.CheckBox tapeInspection;
        private System.Windows.Forms.RadioButton radioCountWhitePixel;
        private System.Windows.Forms.RadioButton radioCountBlackPixel;
        private System.Windows.Forms.RadioButton radioCountGreyPixel;
        private System.Windows.Forms.GroupBox labelGridAcceptance;
        private System.Windows.Forms.CheckBox useGrid;
        private System.Windows.Forms.NumericUpDown gridRowCount;
        private System.Windows.Forms.ComboBox gridCalcType;
        private System.Windows.Forms.Label labelGridRow;
        private System.Windows.Forms.NumericUpDown gridColumnCount;
        private System.Windows.Forms.Label labelGridColumn;
        private System.Windows.Forms.NumericUpDown gridScore;
        private System.Windows.Forms.Label labelGridScore;
        private System.Windows.Forms.NumericUpDown maxPixelRatio;
        private System.Windows.Forms.Label labelTilda;
    }
}