namespace DynMvp.Data.Forms
{
    partial class ColorCheckerParamControl
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorCheckerParamControl));
            this.colorGrid = new System.Windows.Forms.DataGridView();
            this.columnColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnR = new Infragistics.Win.UltraDataGridView.UltraNumericEditorColumn(this.components);
            this.columnG = new Infragistics.Win.UltraDataGridView.UltraNumericEditorColumn(this.components);
            this.columnB = new Infragistics.Win.UltraDataGridView.UltraNumericEditorColumn(this.components);
            this.addColorButton = new System.Windows.Forms.Button();
            this.deleteColorButton = new System.Windows.Forms.Button();
            this.labelAcceptanceScore = new System.Windows.Forms.Label();
            this.acceptanceScore = new System.Windows.Forms.NumericUpDown();
            this.labelScoreWeight = new System.Windows.Forms.Label();
            this.scoreWeightValue1 = new System.Windows.Forms.NumericUpDown();
            this.scoreWeightValue2 = new System.Windows.Forms.NumericUpDown();
            this.scoreWeightValue3 = new System.Windows.Forms.NumericUpDown();
            this.labelScoreWeightValue1 = new System.Windows.Forms.Label();
            this.labelScoreWeightValue2 = new System.Windows.Forms.Label();
            this.labelScoreWeightValue3 = new System.Windows.Forms.Label();
            this.labelColorSpace = new System.Windows.Forms.Label();
            this.colorSpace = new System.Windows.Forms.ComboBox();
            this.segmentScore = new System.Windows.Forms.NumericUpDown();
            this.segmentCalcType = new System.Windows.Forms.ComboBox();
            this.useSegmentation = new System.Windows.Forms.CheckBox();
            this.labelGridAcceptance = new System.Windows.Forms.GroupBox();
            this.useGrid = new System.Windows.Forms.CheckBox();
            this.gridRowCount = new System.Windows.Forms.NumericUpDown();
            this.gridCalcType = new System.Windows.Forms.ComboBox();
            this.labelGridRow = new System.Windows.Forms.Label();
            this.gridColumnCount = new System.Windows.Forms.NumericUpDown();
            this.labelGridColumn = new System.Windows.Forms.Label();
            this.gridScore = new System.Windows.Forms.NumericUpDown();
            this.labelGridScore = new System.Windows.Forms.Label();
            this.selectColorButton = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.colorGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.acceptanceScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scoreWeightValue1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scoreWeightValue2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scoreWeightValue3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.segmentScore)).BeginInit();
            this.labelGridAcceptance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRowCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridColumnCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridScore)).BeginInit();
            this.SuspendLayout();
            // 
            // colorGrid
            // 
            this.colorGrid.AllowUserToAddRows = false;
            this.colorGrid.AllowUserToDeleteRows = false;
            this.colorGrid.AllowUserToResizeColumns = false;
            this.colorGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.colorGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.colorGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.colorGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnColor,
            this.columnR,
            this.columnG,
            this.columnB});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.colorGrid.DefaultCellStyle = dataGridViewCellStyle3;
            this.colorGrid.Location = new System.Drawing.Point(101, 5);
            this.colorGrid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.colorGrid.MultiSelect = false;
            this.colorGrid.Name = "colorGrid";
            this.colorGrid.RowHeadersWidth = 20;
            this.colorGrid.RowTemplate.Height = 23;
            this.colorGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.colorGrid.Size = new System.Drawing.Size(263, 126);
            this.colorGrid.TabIndex = 0;
            this.colorGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.colorGrid_CellValueChanged);
            // 
            // columnColor
            // 
            this.columnColor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnColor.HeaderText = "Color";
            this.columnColor.Name = "columnColor";
            // 
            // columnR
            // 
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = null;
            this.columnR.DefaultCellStyle = dataGridViewCellStyle2;
            this.columnR.DefaultNewRowValue = ((object)(resources.GetObject("columnR.DefaultNewRowValue")));
            this.columnR.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.columnR.HeaderText = "R";
            this.columnR.MaskClipMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.columnR.MaskDisplayMode = Infragistics.Win.UltraWinMaskedEdit.MaskMode.Raw;
            this.columnR.MaskInput = null;
            this.columnR.Name = "columnR";
            this.columnR.PadChar = '\0';
            this.columnR.PromptChar = ' ';
            this.columnR.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnR.ShowInkButton = Infragistics.Win.ShowInkButton.Never;
            this.columnR.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.columnR.SpinButtonAlignment = Infragistics.Win.SpinButtonDisplayStyle.OnRight;
            this.columnR.Width = 60;
            // 
            // columnG
            // 
            this.columnG.DefaultNewRowValue = ((object)(resources.GetObject("columnG.DefaultNewRowValue")));
            this.columnG.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.columnG.HeaderText = "G";
            this.columnG.MaskInput = null;
            this.columnG.Name = "columnG";
            this.columnG.PadChar = '\0';
            this.columnG.PromptChar = ' ';
            this.columnG.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.columnG.SpinButtonAlignment = Infragistics.Win.SpinButtonDisplayStyle.OnRight;
            this.columnG.Width = 60;
            // 
            // columnB
            // 
            this.columnB.DefaultNewRowValue = ((object)(resources.GetObject("columnB.DefaultNewRowValue")));
            this.columnB.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.columnB.HeaderText = "B";
            this.columnB.MaskInput = null;
            this.columnB.Name = "columnB";
            this.columnB.PadChar = '\0';
            this.columnB.PromptChar = ' ';
            this.columnB.ShowInkButton = Infragistics.Win.ShowInkButton.Never;
            this.columnB.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.columnB.SpinButtonAlignment = Infragistics.Win.SpinButtonDisplayStyle.OnRight;
            this.columnB.Width = 60;
            // 
            // addColorButton
            // 
            this.addColorButton.Location = new System.Drawing.Point(2, 3);
            this.addColorButton.Name = "addColorButton";
            this.addColorButton.Size = new System.Drawing.Size(92, 30);
            this.addColorButton.TabIndex = 1;
            this.addColorButton.Text = "Add";
            this.addColorButton.UseVisualStyleBackColor = true;
            this.addColorButton.Click += new System.EventHandler(this.addColorButton_Click);
            // 
            // deleteColorButton
            // 
            this.deleteColorButton.Location = new System.Drawing.Point(2, 70);
            this.deleteColorButton.Name = "deleteColorButton";
            this.deleteColorButton.Size = new System.Drawing.Size(92, 30);
            this.deleteColorButton.TabIndex = 1;
            this.deleteColorButton.Text = "Delete";
            this.deleteColorButton.UseVisualStyleBackColor = true;
            this.deleteColorButton.Click += new System.EventHandler(this.deleteColorButton_Click);
            // 
            // labelAcceptanceScore
            // 
            this.labelAcceptanceScore.AutoSize = true;
            this.labelAcceptanceScore.Location = new System.Drawing.Point(3, 192);
            this.labelAcceptanceScore.Name = "labelAcceptanceScore";
            this.labelAcceptanceScore.Size = new System.Drawing.Size(129, 20);
            this.labelAcceptanceScore.TabIndex = 3;
            this.labelAcceptanceScore.Text = "Acceptance Score";
            // 
            // acceptanceScore
            // 
            this.acceptanceScore.Location = new System.Drawing.Point(292, 189);
            this.acceptanceScore.Name = "acceptanceScore";
            this.acceptanceScore.Size = new System.Drawing.Size(68, 27);
            this.acceptanceScore.TabIndex = 4;
            this.acceptanceScore.ValueChanged += new System.EventHandler(this.acceptanceScore_ValueChanged);
            // 
            // labelScoreWeight
            // 
            this.labelScoreWeight.AutoSize = true;
            this.labelScoreWeight.Location = new System.Drawing.Point(3, 161);
            this.labelScoreWeight.Name = "labelScoreWeight";
            this.labelScoreWeight.Size = new System.Drawing.Size(100, 20);
            this.labelScoreWeight.TabIndex = 3;
            this.labelScoreWeight.Text = "Score Weight";
            // 
            // scoreWeightValue1
            // 
            this.scoreWeightValue1.DecimalPlaces = 2;
            this.scoreWeightValue1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.scoreWeightValue1.Location = new System.Drawing.Point(149, 159);
            this.scoreWeightValue1.Name = "scoreWeightValue1";
            this.scoreWeightValue1.Size = new System.Drawing.Size(68, 27);
            this.scoreWeightValue1.TabIndex = 4;
            this.scoreWeightValue1.ValueChanged += new System.EventHandler(this.scoreWeightValue1_ValueChanged);
            // 
            // scoreWeightValue2
            // 
            this.scoreWeightValue2.DecimalPlaces = 2;
            this.scoreWeightValue2.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.scoreWeightValue2.Location = new System.Drawing.Point(221, 159);
            this.scoreWeightValue2.Name = "scoreWeightValue2";
            this.scoreWeightValue2.Size = new System.Drawing.Size(68, 27);
            this.scoreWeightValue2.TabIndex = 4;
            this.scoreWeightValue2.ValueChanged += new System.EventHandler(this.scoreWeightValue2_ValueChanged);
            // 
            // scoreWeightValue3
            // 
            this.scoreWeightValue3.DecimalPlaces = 2;
            this.scoreWeightValue3.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.scoreWeightValue3.Location = new System.Drawing.Point(292, 159);
            this.scoreWeightValue3.Name = "scoreWeightValue3";
            this.scoreWeightValue3.Size = new System.Drawing.Size(68, 27);
            this.scoreWeightValue3.TabIndex = 4;
            this.scoreWeightValue3.ValueChanged += new System.EventHandler(this.scoreWeightValue3_ValueChanged);
            // 
            // labelScoreWeightValue1
            // 
            this.labelScoreWeightValue1.AutoSize = true;
            this.labelScoreWeightValue1.Location = new System.Drawing.Point(179, 136);
            this.labelScoreWeightValue1.Name = "labelScoreWeightValue1";
            this.labelScoreWeightValue1.Size = new System.Drawing.Size(18, 20);
            this.labelScoreWeightValue1.TabIndex = 3;
            this.labelScoreWeightValue1.Text = "R";
            this.labelScoreWeightValue1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelScoreWeightValue2
            // 
            this.labelScoreWeightValue2.AutoSize = true;
            this.labelScoreWeightValue2.Location = new System.Drawing.Point(248, 136);
            this.labelScoreWeightValue2.Name = "labelScoreWeightValue2";
            this.labelScoreWeightValue2.Size = new System.Drawing.Size(20, 20);
            this.labelScoreWeightValue2.TabIndex = 3;
            this.labelScoreWeightValue2.Text = "G";
            this.labelScoreWeightValue2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelScoreWeightValue3
            // 
            this.labelScoreWeightValue3.AutoSize = true;
            this.labelScoreWeightValue3.Location = new System.Drawing.Point(318, 136);
            this.labelScoreWeightValue3.Name = "labelScoreWeightValue3";
            this.labelScoreWeightValue3.Size = new System.Drawing.Size(18, 20);
            this.labelScoreWeightValue3.TabIndex = 3;
            this.labelScoreWeightValue3.Text = "B";
            this.labelScoreWeightValue3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelColorSpace
            // 
            this.labelColorSpace.AutoSize = true;
            this.labelColorSpace.Location = new System.Drawing.Point(3, 105);
            this.labelColorSpace.Name = "labelColorSpace";
            this.labelColorSpace.Size = new System.Drawing.Size(91, 20);
            this.labelColorSpace.TabIndex = 3;
            this.labelColorSpace.Text = "Color Space";
            // 
            // colorSpace
            // 
            this.colorSpace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.colorSpace.FormattingEnabled = true;
            this.colorSpace.Items.AddRange(new object[] {
            "RGB",
            "HSI"});
            this.colorSpace.Location = new System.Drawing.Point(2, 128);
            this.colorSpace.Name = "colorSpace";
            this.colorSpace.Size = new System.Drawing.Size(92, 28);
            this.colorSpace.TabIndex = 6;
            this.colorSpace.SelectedIndexChanged += new System.EventHandler(this.colorSpace_SelectedIndexChanged);
            // 
            // segmentScore
            // 
            this.segmentScore.Location = new System.Drawing.Point(292, 220);
            this.segmentScore.Name = "segmentScore";
            this.segmentScore.Size = new System.Drawing.Size(68, 27);
            this.segmentScore.TabIndex = 4;
            this.segmentScore.ValueChanged += new System.EventHandler(this.segmentScore_ValueChanged);
            // 
            // segmentCalcType
            // 
            this.segmentCalcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.segmentCalcType.FormattingEnabled = true;
            this.segmentCalcType.Items.AddRange(new object[] {
            "Ratio",
            "Count"});
            this.segmentCalcType.Location = new System.Drawing.Point(199, 220);
            this.segmentCalcType.Name = "segmentCalcType";
            this.segmentCalcType.Size = new System.Drawing.Size(90, 28);
            this.segmentCalcType.TabIndex = 6;
            this.segmentCalcType.SelectedIndexChanged += new System.EventHandler(this.segmentCalcType_SelectedIndexChanged);
            // 
            // useSegmentation
            // 
            this.useSegmentation.AutoSize = true;
            this.useSegmentation.Location = new System.Drawing.Point(7, 224);
            this.useSegmentation.Name = "useSegmentation";
            this.useSegmentation.Size = new System.Drawing.Size(123, 24);
            this.useSegmentation.TabIndex = 8;
            this.useSegmentation.Text = "Segmentation";
            this.useSegmentation.UseVisualStyleBackColor = true;
            this.useSegmentation.CheckedChanged += new System.EventHandler(this.useSegmentation_CheckedChanged);
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
            this.labelGridAcceptance.Location = new System.Drawing.Point(3, 251);
            this.labelGridAcceptance.Name = "labelGridAcceptance";
            this.labelGridAcceptance.Size = new System.Drawing.Size(357, 93);
            this.labelGridAcceptance.TabIndex = 9;
            this.labelGridAcceptance.TabStop = false;
            this.labelGridAcceptance.Text = "groupBox1";
            // 
            // useGrid
            // 
            this.useGrid.AutoSize = true;
            this.useGrid.Location = new System.Drawing.Point(6, 0);
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
            this.gridRowCount.Name = "gridRowCount";
            this.gridRowCount.Size = new System.Drawing.Size(77, 27);
            this.gridRowCount.TabIndex = 20;
            this.gridRowCount.ValueChanged += new System.EventHandler(this.girdRowCount_ValueChanged);
            // 
            // gridCalcType
            // 
            this.gridCalcType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gridCalcType.FormattingEnabled = true;
            this.gridCalcType.Items.AddRange(new object[] {
            "Ratio",
            "Count"});
            this.gridCalcType.Location = new System.Drawing.Point(180, 58);
            this.gridCalcType.Name = "gridCalcType";
            this.gridCalcType.Size = new System.Drawing.Size(90, 28);
            this.gridCalcType.TabIndex = 21;
            this.gridCalcType.SelectedIndexChanged += new System.EventHandler(this.gridCalcType_SelectedIndexChanged);
            // 
            // labelGridRow
            // 
            this.labelGridRow.AutoSize = true;
            this.labelGridRow.Location = new System.Drawing.Point(8, 27);
            this.labelGridRow.Name = "labelGridRow";
            this.labelGridRow.Size = new System.Drawing.Size(38, 20);
            this.labelGridRow.TabIndex = 17;
            this.labelGridRow.Text = "Row";
            // 
            // gridColumnCount
            // 
            this.gridColumnCount.Location = new System.Drawing.Point(272, 25);
            this.gridColumnCount.Name = "gridColumnCount";
            this.gridColumnCount.Size = new System.Drawing.Size(77, 27);
            this.gridColumnCount.TabIndex = 18;
            this.gridColumnCount.ValueChanged += new System.EventHandler(this.gridColumnCount_ValueChanged);
            // 
            // labelGridColumn
            // 
            this.labelGridColumn.AutoSize = true;
            this.labelGridColumn.Location = new System.Drawing.Point(186, 27);
            this.labelGridColumn.Name = "labelGridColumn";
            this.labelGridColumn.Size = new System.Drawing.Size(63, 20);
            this.labelGridColumn.TabIndex = 16;
            this.labelGridColumn.Text = "Column";
            // 
            // gridScore
            // 
            this.gridScore.Location = new System.Drawing.Point(272, 58);
            this.gridScore.Name = "gridScore";
            this.gridScore.Size = new System.Drawing.Size(77, 27);
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
            // selectColorButton
            // 
            this.selectColorButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.selectColorButton.Location = new System.Drawing.Point(2, 36);
            this.selectColorButton.Name = "selectColorButton";
            this.selectColorButton.Size = new System.Drawing.Size(92, 30);
            this.selectColorButton.TabIndex = 10;
            this.selectColorButton.Text = "Select";
            this.selectColorButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.selectColorButton.UseVisualStyleBackColor = true;
            // 
            // ColorCheckerParamControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.selectColorButton);
            this.Controls.Add(this.labelGridAcceptance);
            this.Controls.Add(this.useSegmentation);
            this.Controls.Add(this.segmentScore);
            this.Controls.Add(this.segmentCalcType);
            this.Controls.Add(this.colorSpace);
            this.Controls.Add(this.scoreWeightValue3);
            this.Controls.Add(this.scoreWeightValue2);
            this.Controls.Add(this.scoreWeightValue1);
            this.Controls.Add(this.acceptanceScore);
            this.Controls.Add(this.labelScoreWeightValue3);
            this.Controls.Add(this.labelScoreWeightValue2);
            this.Controls.Add(this.labelScoreWeightValue1);
            this.Controls.Add(this.labelScoreWeight);
            this.Controls.Add(this.labelColorSpace);
            this.Controls.Add(this.labelAcceptanceScore);
            this.Controls.Add(this.deleteColorButton);
            this.Controls.Add(this.addColorButton);
            this.Controls.Add(this.colorGrid);
            this.Font = new System.Drawing.Font("Malgun Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ColorCheckerParamControl";
            this.Size = new System.Drawing.Size(368, 349);
            ((System.ComponentModel.ISupportInitialize)(this.colorGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.acceptanceScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scoreWeightValue1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scoreWeightValue2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scoreWeightValue3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.segmentScore)).EndInit();
            this.labelGridAcceptance.ResumeLayout(false);
            this.labelGridAcceptance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRowCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridColumnCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridScore)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView colorGrid;
        private System.Windows.Forms.Button addColorButton;
        private System.Windows.Forms.Button deleteColorButton;
        private System.Windows.Forms.Label labelAcceptanceScore;
        private System.Windows.Forms.NumericUpDown acceptanceScore;
        private System.Windows.Forms.Label labelScoreWeight;
        private System.Windows.Forms.NumericUpDown scoreWeightValue1;
        private System.Windows.Forms.NumericUpDown scoreWeightValue2;
        private System.Windows.Forms.NumericUpDown scoreWeightValue3;
        private System.Windows.Forms.Label labelScoreWeightValue1;
        private System.Windows.Forms.Label labelScoreWeightValue2;
        private System.Windows.Forms.Label labelScoreWeightValue3;
        private System.Windows.Forms.Label labelColorSpace;
        private System.Windows.Forms.ComboBox colorSpace;
        private System.Windows.Forms.NumericUpDown segmentScore;
        private System.Windows.Forms.ComboBox segmentCalcType;
        private System.Windows.Forms.CheckBox useSegmentation;
        private System.Windows.Forms.GroupBox labelGridAcceptance;
        private System.Windows.Forms.CheckBox useGrid;
        private System.Windows.Forms.NumericUpDown gridRowCount;
        private System.Windows.Forms.ComboBox gridCalcType;
        private System.Windows.Forms.Label labelGridRow;
        private System.Windows.Forms.NumericUpDown gridColumnCount;
        private System.Windows.Forms.Label labelGridColumn;
        private System.Windows.Forms.NumericUpDown gridScore;
        private System.Windows.Forms.Label labelGridScore;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnColor;
        private Infragistics.Win.UltraDataGridView.UltraNumericEditorColumn columnR;
        private Infragistics.Win.UltraDataGridView.UltraNumericEditorColumn columnG;
        private Infragistics.Win.UltraDataGridView.UltraNumericEditorColumn columnB;
        private System.Windows.Forms.CheckBox selectColorButton;
    }
}
