namespace UniScanG.Module.Controller.UI.Inspect
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelBufferUseTransfer = new Infragistics.Win.Misc.UltraLabel();
            this.bufferUsageTransfer = new Infragistics.Win.Misc.UltraLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelBufferUseTransfer, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.bufferUsageTransfer, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 31);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelBufferUseTransfer
            // 
            appearance3.FontData.BoldAsString = "True";
            appearance3.FontData.Name = "Malgun Gothic";
            appearance3.FontData.SizeInPoints = 12F;
            appearance3.ForeColor = System.Drawing.Color.Black;
            appearance3.TextHAlignAsString = "Center";
            appearance3.TextVAlignAsString = "Middle";
            this.labelBufferUseTransfer.Appearance = appearance3;
            this.labelBufferUseTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBufferUseTransfer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelBufferUseTransfer.Location = new System.Drawing.Point(0, 0);
            this.labelBufferUseTransfer.Margin = new System.Windows.Forms.Padding(0);
            this.labelBufferUseTransfer.Name = "labelBufferUseTransfer";
            this.labelBufferUseTransfer.Size = new System.Drawing.Size(100, 31);
            this.labelBufferUseTransfer.TabIndex = 16;
            this.labelBufferUseTransfer.Text = "Trasnfer";
            // 
            // bufferUsageTransfer
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.FontData.BoldAsString = "True";
            appearance4.FontData.Name = "Malgun Gothic";
            appearance4.FontData.SizeInPoints = 12F;
            appearance4.ForeColor = System.Drawing.Color.Black;
            appearance4.TextHAlignAsString = "Center";
            appearance4.TextVAlignAsString = "Middle";
            this.bufferUsageTransfer.Appearance = appearance4;
            this.bufferUsageTransfer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bufferUsageTransfer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bufferUsageTransfer.Location = new System.Drawing.Point(100, 0);
            this.bufferUsageTransfer.Margin = new System.Windows.Forms.Padding(0);
            this.bufferUsageTransfer.Name = "bufferUsageTransfer";
            this.bufferUsageTransfer.Size = new System.Drawing.Size(100, 31);
            this.bufferUsageTransfer.TabIndex = 17;
            this.bufferUsageTransfer.Text = "0";
            // 
            // InfoPanelBufferState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(200, 0);
            this.Name = "InfoPanelBufferState";
            this.Size = new System.Drawing.Size(200, 31);
            this.Load += new System.EventHandler(this.InfoPanelBufferState_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Infragistics.Win.Misc.UltraLabel bufferUsageTransfer;
        private Infragistics.Win.Misc.UltraLabel labelBufferUseTransfer;
    }
}
