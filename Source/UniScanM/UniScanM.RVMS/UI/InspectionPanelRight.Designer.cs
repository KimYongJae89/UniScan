namespace UniScanM.RVMS.UI
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.layoutPatternView = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelState = new System.Windows.Forms.Label();
            this.buttonStateReset = new Infragistics.Win.Misc.UltraButton();
            this.panelAfter = new System.Windows.Forms.Panel();
            this.panelBefore = new System.Windows.Forms.Panel();
            this.progressBarZeroing = new System.Windows.Forms.ProgressBar();
            this.layoutPatternView.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutPatternView
            // 
            this.layoutPatternView.ColumnCount = 1;
            this.layoutPatternView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPatternView.Controls.Add(this.labelState, 0, 0);
            this.layoutPatternView.Controls.Add(this.panelAfter, 0, 3);
            this.layoutPatternView.Controls.Add(this.panelBefore, 0, 2);
            this.layoutPatternView.Controls.Add(this.panel1, 0, 1);
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
            // buttonStateReset
            // 
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonStateReset.Appearance = appearance1;
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
            this.panelAfter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAfter.Location = new System.Drawing.Point(0, 260);
            this.panelAfter.Margin = new System.Windows.Forms.Padding(0);
            this.panelAfter.Name = "panelAfter";
            this.panelAfter.Size = new System.Drawing.Size(499, 171);
            this.panelAfter.TabIndex = 43;
            // 
            // panelBefore
            // 
            this.panelBefore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBefore.Location = new System.Drawing.Point(0, 90);
            this.panelBefore.Margin = new System.Windows.Forms.Padding(0);
            this.panelBefore.Name = "panelBefore";
            this.panelBefore.Size = new System.Drawing.Size(499, 170);
            this.panelBefore.TabIndex = 42;
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
    }
}
