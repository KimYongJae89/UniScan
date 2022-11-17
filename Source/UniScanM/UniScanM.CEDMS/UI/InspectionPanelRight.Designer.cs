namespace UniScanM.CEDMS.UI
{
    partial class InspectionPanelRight
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.layoutPatternView = new System.Windows.Forms.TableLayoutPanel();
            this.labelState = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBarZeroing = new System.Windows.Forms.ProgressBar();
            this.buttonStateReset = new Infragistics.Win.Misc.UltraButton();
            this.panelAfter = new System.Windows.Forms.Panel();
            this.checkOnTune = new System.Windows.Forms.CheckBox();
            this.panelBefore = new System.Windows.Forms.Panel();
            this.groupJudegementLevel = new System.Windows.Forms.GroupBox();
            this.outfeedValue = new System.Windows.Forms.NumericUpDown();
            this.labelOutfeed = new System.Windows.Forms.Label();
            this.infeedValue = new System.Windows.Forms.NumericUpDown();
            this.labelInfeed = new System.Windows.Forms.Label();
            this.layoutPatternView.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelAfter.SuspendLayout();
            this.panelBefore.SuspendLayout();
            this.groupJudegementLevel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outfeedValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infeedValue)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutPatternView
            // 
            this.layoutPatternView.ColumnCount = 1;
            this.layoutPatternView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPatternView.Controls.Add(this.labelState, 0, 0);
            this.layoutPatternView.Controls.Add(this.panel1, 0, 1);
            this.layoutPatternView.Controls.Add(this.panelAfter, 0, 3);
            this.layoutPatternView.Controls.Add(this.panelBefore, 0, 2);
            this.layoutPatternView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPatternView.Location = new System.Drawing.Point(0, 0);
            this.layoutPatternView.Margin = new System.Windows.Forms.Padding(0);
            this.layoutPatternView.Name = "layoutPatternView";
            this.layoutPatternView.RowCount = 4;
            this.layoutPatternView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutPatternView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutPatternView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutPatternView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutPatternView.Size = new System.Drawing.Size(499, 431);
            this.layoutPatternView.TabIndex = 30;
            // 
            // labelState
            // 
            this.labelState.BackColor = System.Drawing.Color.Black;
            this.labelState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelState.Font = new System.Drawing.Font("맑은 고딕", 26F, System.Drawing.FontStyle.Bold);
            this.labelState.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.labelState.Location = new System.Drawing.Point(0, 0);
            this.labelState.Margin = new System.Windows.Forms.Padding(0);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(499, 50);
            this.labelState.TabIndex = 44;
            this.labelState.Text = "State";
            this.labelState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.progressBarZeroing);
            this.panel1.Controls.Add(this.buttonStateReset);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 50);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(499, 40);
            this.panel1.TabIndex = 0;
            // 
            // progressBarZeroing
            // 
            this.progressBarZeroing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarZeroing.Location = new System.Drawing.Point(0, 0);
            this.progressBarZeroing.Margin = new System.Windows.Forms.Padding(0);
            this.progressBarZeroing.Name = "progressBarZeroing";
            this.progressBarZeroing.Size = new System.Drawing.Size(414, 40);
            this.progressBarZeroing.TabIndex = 45;
            // 
            // buttonStateReset
            // 
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance2.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonStateReset.Appearance = appearance2;
            this.buttonStateReset.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.buttonStateReset.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStateReset.ImageSize = new System.Drawing.Size(20, 20);
            this.buttonStateReset.Location = new System.Drawing.Point(414, 0);
            this.buttonStateReset.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStateReset.Name = "buttonStateReset";
            this.buttonStateReset.Size = new System.Drawing.Size(85, 40);
            this.buttonStateReset.TabIndex = 46;
            this.buttonStateReset.Text = "Zeroing";
            this.buttonStateReset.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonStateReset.Click += new System.EventHandler(this.buttonStateReset_Click);
            // 
            // panelAfter
            // 
            this.panelAfter.Controls.Add(this.checkOnTune);
            this.panelAfter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAfter.Location = new System.Drawing.Point(0, 260);
            this.panelAfter.Margin = new System.Windows.Forms.Padding(0);
            this.panelAfter.Name = "panelAfter";
            this.panelAfter.Size = new System.Drawing.Size(499, 171);
            this.panelAfter.TabIndex = 43;
            // 
            // checkOnTune
            // 
            this.checkOnTune.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkOnTune.BackColor = System.Drawing.Color.Gray;
            this.checkOnTune.Checked = true;
            this.checkOnTune.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkOnTune.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkOnTune.FlatAppearance.CheckedBackColor = System.Drawing.Color.SeaGreen;
            this.checkOnTune.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkOnTune.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkOnTune.Location = new System.Drawing.Point(0, 121);
            this.checkOnTune.Name = "checkOnTune";
            this.checkOnTune.Size = new System.Drawing.Size(499, 50);
            this.checkOnTune.TabIndex = 13;
            this.checkOnTune.Text = "Sensitivity Enable / Disable";
            this.checkOnTune.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkOnTune.UseVisualStyleBackColor = false;
            this.checkOnTune.CheckedChanged += new System.EventHandler(this.checkOnTune_CheckedChanged);
            // 
            // panelBefore
            // 
            this.panelBefore.Controls.Add(this.groupJudegementLevel);
            this.panelBefore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBefore.Location = new System.Drawing.Point(0, 90);
            this.panelBefore.Margin = new System.Windows.Forms.Padding(0);
            this.panelBefore.Name = "panelBefore";
            this.panelBefore.Size = new System.Drawing.Size(499, 170);
            this.panelBefore.TabIndex = 42;
            // 
            // groupJudegementLevel
            // 
            this.groupJudegementLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupJudegementLevel.Controls.Add(this.outfeedValue);
            this.groupJudegementLevel.Controls.Add(this.labelOutfeed);
            this.groupJudegementLevel.Controls.Add(this.infeedValue);
            this.groupJudegementLevel.Controls.Add(this.labelInfeed);
            this.groupJudegementLevel.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupJudegementLevel.Location = new System.Drawing.Point(5, 6);
            this.groupJudegementLevel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupJudegementLevel.Name = "groupJudegementLevel";
            this.groupJudegementLevel.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupJudegementLevel.Size = new System.Drawing.Size(489, 134);
            this.groupJudegementLevel.TabIndex = 5;
            this.groupJudegementLevel.TabStop = false;
            this.groupJudegementLevel.Text = "Judegement Level (%)";
            this.groupJudegementLevel.UseCompatibleTextRendering = true;
            // 
            // outfeedValue
            // 
            this.outfeedValue.Location = new System.Drawing.Point(110, 80);
            this.outfeedValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.outfeedValue.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.outfeedValue.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.outfeedValue.Name = "outfeedValue";
            this.outfeedValue.Size = new System.Drawing.Size(153, 33);
            this.outfeedValue.TabIndex = 3;
            this.outfeedValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.outfeedValue.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.outfeedValue.ValueChanged += new System.EventHandler(this.outfeedValue_ValueChanged);
            // 
            // labelOutfeed
            // 
            this.labelOutfeed.AutoSize = true;
            this.labelOutfeed.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelOutfeed.Location = new System.Drawing.Point(22, 84);
            this.labelOutfeed.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelOutfeed.Name = "labelOutfeed";
            this.labelOutfeed.Size = new System.Drawing.Size(85, 25);
            this.labelOutfeed.TabIndex = 2;
            this.labelOutfeed.Text = "Outfeed";
            this.labelOutfeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infeedValue
            // 
            this.infeedValue.Location = new System.Drawing.Point(110, 35);
            this.infeedValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.infeedValue.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.infeedValue.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.infeedValue.Name = "infeedValue";
            this.infeedValue.Size = new System.Drawing.Size(153, 33);
            this.infeedValue.TabIndex = 1;
            this.infeedValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.infeedValue.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.infeedValue.ValueChanged += new System.EventHandler(this.infeedValue_ValueChanged);
            // 
            // labelInfeed
            // 
            this.labelInfeed.AutoSize = true;
            this.labelInfeed.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelInfeed.Location = new System.Drawing.Point(22, 39);
            this.labelInfeed.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelInfeed.Name = "labelInfeed";
            this.labelInfeed.Size = new System.Drawing.Size(69, 25);
            this.labelInfeed.TabIndex = 0;
            this.labelInfeed.Text = "Infeed";
            this.labelInfeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InspectionPanelRight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutPatternView);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "InspectionPanelRight";
            this.Size = new System.Drawing.Size(499, 431);
            this.layoutPatternView.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panelAfter.ResumeLayout(false);
            this.panelBefore.ResumeLayout(false);
            this.groupJudegementLevel.ResumeLayout(false);
            this.groupJudegementLevel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outfeedValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infeedValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel layoutPatternView;
        private System.Windows.Forms.Panel panelBefore;
        private System.Windows.Forms.Panel panelAfter;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.ProgressBar progressBarZeroing;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraButton buttonStateReset;
        private System.Windows.Forms.GroupBox groupJudegementLevel;
        private System.Windows.Forms.NumericUpDown outfeedValue;
        private System.Windows.Forms.Label labelOutfeed;
        private System.Windows.Forms.NumericUpDown infeedValue;
        private System.Windows.Forms.Label labelInfeed;
        private System.Windows.Forms.CheckBox checkOnTune;
    }
}
