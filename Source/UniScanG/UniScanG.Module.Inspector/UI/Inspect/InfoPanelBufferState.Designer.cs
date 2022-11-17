namespace UniScanG.Module.Inspector.UI.Inspect
{
    partial class InfoPanelBufferState
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.bufferUsageTransfer = new Infragistics.Win.Misc.UltraLabel();
            this.labelBufferUseTransfer = new Infragistics.Win.Misc.UltraLabel();
            this.bufferUsageInsp = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
            this.labelBufferUseInsp = new Infragistics.Win.Misc.UltraLabel();
            this.bufferUsageGrab = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
            this.labelBufferUseGrab = new Infragistics.Win.Misc.UltraLabel();
            this.labelLoadPrep = new Infragistics.Win.Misc.UltraLabel();
            this.loadUsagePrep = new Infragistics.Win.UltraWinProgressBar.UltraProgressBar();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.bufferUsageTransfer, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelBufferUseTransfer, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.bufferUsageInsp, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelBufferUseInsp, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.bufferUsageGrab, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelBufferUseGrab, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelLoadPrep, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.loadUsagePrep, 1, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 120);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // bufferUsageTransfer
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.FontData.BoldAsString = "True";
            appearance1.FontData.Name = "Malgun Gothic";
            appearance1.FontData.SizeInPoints = 12F;
            appearance1.ForeColor = System.Drawing.Color.Black;
            appearance1.TextHAlignAsString = "Center";
            appearance1.TextVAlignAsString = "Middle";
            this.bufferUsageTransfer.Appearance = appearance1;
            this.bufferUsageTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bufferUsageTransfer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bufferUsageTransfer.Location = new System.Drawing.Point(100, 60);
            this.bufferUsageTransfer.Margin = new System.Windows.Forms.Padding(0);
            this.bufferUsageTransfer.Name = "bufferUsageTransfer";
            this.bufferUsageTransfer.Size = new System.Drawing.Size(100, 30);
            this.bufferUsageTransfer.TabIndex = 15;
            this.bufferUsageTransfer.Text = "0";
            // 
            // labelBufferUseTransfer
            // 
            appearance2.FontData.BoldAsString = "True";
            appearance2.FontData.Name = "Malgun Gothic";
            appearance2.FontData.SizeInPoints = 12F;
            appearance2.ForeColor = System.Drawing.Color.Black;
            appearance2.TextHAlignAsString = "Center";
            appearance2.TextVAlignAsString = "Middle";
            this.labelBufferUseTransfer.Appearance = appearance2;
            this.labelBufferUseTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBufferUseTransfer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelBufferUseTransfer.Location = new System.Drawing.Point(0, 60);
            this.labelBufferUseTransfer.Margin = new System.Windows.Forms.Padding(0);
            this.labelBufferUseTransfer.Name = "labelBufferUseTransfer";
            this.labelBufferUseTransfer.Size = new System.Drawing.Size(100, 30);
            this.labelBufferUseTransfer.TabIndex = 15;
            this.labelBufferUseTransfer.Text = "Save";
            // 
            // bufferUsageInsp
            // 
            appearance3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            this.bufferUsageInsp.Appearance = appearance3;
            this.bufferUsageInsp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bufferUsageInsp.Location = new System.Drawing.Point(100, 30);
            this.bufferUsageInsp.Margin = new System.Windows.Forms.Padding(0);
            this.bufferUsageInsp.Name = "bufferUsageInsp";
            this.bufferUsageInsp.Size = new System.Drawing.Size(100, 30);
            this.bufferUsageInsp.TabIndex = 0;
            this.bufferUsageInsp.Text = "[Formatted]";
            this.bufferUsageInsp.Value = 50;
            // 
            // labelBufferUseInsp
            // 
            appearance4.FontData.BoldAsString = "True";
            appearance4.FontData.Name = "Malgun Gothic";
            appearance4.FontData.SizeInPoints = 12F;
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.TextHAlignAsString = "Center";
            appearance4.TextVAlignAsString = "Middle";
            this.labelBufferUseInsp.Appearance = appearance4;
            this.labelBufferUseInsp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBufferUseInsp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelBufferUseInsp.Location = new System.Drawing.Point(0, 30);
            this.labelBufferUseInsp.Margin = new System.Windows.Forms.Padding(0);
            this.labelBufferUseInsp.Name = "labelBufferUseInsp";
            this.labelBufferUseInsp.Size = new System.Drawing.Size(100, 30);
            this.labelBufferUseInsp.TabIndex = 15;
            this.labelBufferUseInsp.Text = "Inspect";
            // 
            // bufferUsageGrab
            // 
            this.bufferUsageGrab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bufferUsageGrab.Location = new System.Drawing.Point(100, 0);
            this.bufferUsageGrab.Margin = new System.Windows.Forms.Padding(0);
            this.bufferUsageGrab.Name = "bufferUsageGrab";
            this.bufferUsageGrab.Size = new System.Drawing.Size(100, 30);
            this.bufferUsageGrab.TabIndex = 0;
            this.bufferUsageGrab.Text = "[Formatted]";
            this.bufferUsageGrab.Value = 50;
            // 
            // labelBufferUseGrab
            // 
            appearance5.FontData.BoldAsString = "True";
            appearance5.FontData.Name = "Malgun Gothic";
            appearance5.FontData.SizeInPoints = 12F;
            appearance5.ForeColor = System.Drawing.Color.Black;
            appearance5.TextHAlignAsString = "Center";
            appearance5.TextVAlignAsString = "Middle";
            this.labelBufferUseGrab.Appearance = appearance5;
            this.labelBufferUseGrab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBufferUseGrab.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelBufferUseGrab.Location = new System.Drawing.Point(0, 0);
            this.labelBufferUseGrab.Margin = new System.Windows.Forms.Padding(0);
            this.labelBufferUseGrab.Name = "labelBufferUseGrab";
            this.labelBufferUseGrab.Size = new System.Drawing.Size(100, 30);
            this.labelBufferUseGrab.TabIndex = 15;
            this.labelBufferUseGrab.Text = "Grab";
            // 
            // labelLoadPrep
            // 
            appearance6.FontData.BoldAsString = "True";
            appearance6.FontData.Name = "Malgun Gothic";
            appearance6.FontData.SizeInPoints = 12F;
            appearance6.ForeColor = System.Drawing.Color.Black;
            appearance6.TextHAlignAsString = "Center";
            appearance6.TextVAlignAsString = "Middle";
            this.labelLoadPrep.Appearance = appearance6;
            this.labelLoadPrep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLoadPrep.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelLoadPrep.Location = new System.Drawing.Point(0, 90);
            this.labelLoadPrep.Margin = new System.Windows.Forms.Padding(0);
            this.labelLoadPrep.Name = "labelLoadPrep";
            this.labelLoadPrep.Size = new System.Drawing.Size(100, 30);
            this.labelLoadPrep.TabIndex = 15;
            this.labelLoadPrep.Text = "Preprocess";
            // 
            // loadUsagePrep
            // 
            appearance7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            appearance7.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            this.loadUsagePrep.Appearance = appearance7;
            this.loadUsagePrep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loadUsagePrep.Location = new System.Drawing.Point(100, 90);
            this.loadUsagePrep.Margin = new System.Windows.Forms.Padding(0);
            this.loadUsagePrep.Maximum = 1000;
            this.loadUsagePrep.Name = "loadUsagePrep";
            this.loadUsagePrep.Size = new System.Drawing.Size(100, 30);
            this.loadUsagePrep.TabIndex = 0;
            this.loadUsagePrep.Text = "[Formatted]";
            this.loadUsagePrep.Value = 50;
            // 
            // InfoPanelBufferState
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(200, 0);
            this.Name = "InfoPanelBufferState";
            this.Size = new System.Drawing.Size(200, 120);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Infragistics.Win.Misc.UltraLabel labelBufferUseGrab;
        private Infragistics.Win.Misc.UltraLabel labelBufferUseInsp;
        private Infragistics.Win.Misc.UltraLabel labelBufferUseTransfer;
        private Infragistics.Win.UltraWinProgressBar.UltraProgressBar bufferUsageGrab;
        private Infragistics.Win.UltraWinProgressBar.UltraProgressBar bufferUsageInsp;
        private Infragistics.Win.Misc.UltraLabel bufferUsageTransfer;
        private Infragistics.Win.Misc.UltraLabel labelLoadPrep;
        private Infragistics.Win.UltraWinProgressBar.UltraProgressBar loadUsagePrep;
    }
}
