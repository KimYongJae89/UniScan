namespace UniScanG.Screen.UI.TeachPage.Inspector
{
    partial class DefectTypePanel
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
            this.layoutTotal = new System.Windows.Forms.TableLayoutPanel();
            this.layoutSubType = new System.Windows.Forms.TableLayoutPanel();
            this.typePoleCircle = new System.Windows.Forms.RadioButton();
            this.typeSheetAttack = new System.Windows.Forms.RadioButton();
            this.typeShape = new System.Windows.Forms.RadioButton();
            this.typePoleLine = new System.Windows.Forms.RadioButton();
            this.typePinHole = new System.Windows.Forms.RadioButton();
            this.typeDielectric = new System.Windows.Forms.RadioButton();
            this.typeTotal = new System.Windows.Forms.RadioButton();
            this.layoutTotal.SuspendLayout();
            this.layoutSubType.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutTotal
            // 
            this.layoutTotal.ColumnCount = 2;
            this.layoutTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.layoutTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTotal.Controls.Add(this.layoutSubType, 0, 0);
            this.layoutTotal.Controls.Add(this.typeTotal, 0, 0);
            this.layoutTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutTotal.Location = new System.Drawing.Point(0, 0);
            this.layoutTotal.Margin = new System.Windows.Forms.Padding(10, 5, 5, 5);
            this.layoutTotal.Name = "layoutTotal";
            this.layoutTotal.RowCount = 1;
            this.layoutTotal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTotal.Size = new System.Drawing.Size(442, 76);
            this.layoutTotal.TabIndex = 57;
            // 
            // layoutSubType
            // 
            this.layoutSubType.ColumnCount = 4;
            this.layoutSubType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.layoutSubType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.layoutSubType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.layoutSubType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSubType.Controls.Add(this.typePoleCircle, 2, 0);
            this.layoutSubType.Controls.Add(this.typeSheetAttack, 0, 0);
            this.layoutSubType.Controls.Add(this.typeShape, 2, 1);
            this.layoutSubType.Controls.Add(this.typePoleLine, 1, 0);
            this.layoutSubType.Controls.Add(this.typePinHole, 1, 1);
            this.layoutSubType.Controls.Add(this.typeDielectric, 0, 1);
            this.layoutSubType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSubType.Location = new System.Drawing.Point(80, 0);
            this.layoutSubType.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSubType.Name = "layoutSubType";
            this.layoutSubType.RowCount = 2;
            this.layoutSubType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutSubType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutSubType.Size = new System.Drawing.Size(362, 76);
            this.layoutSubType.TabIndex = 83;
            // 
            // typePoleCircle
            // 
            this.typePoleCircle.AutoSize = true;
            this.typePoleCircle.BackColor = System.Drawing.Color.Transparent;
            this.typePoleCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typePoleCircle.ForeColor = System.Drawing.Color.OrangeRed;
            this.typePoleCircle.Location = new System.Drawing.Point(240, 0);
            this.typePoleCircle.Margin = new System.Windows.Forms.Padding(0);
            this.typePoleCircle.Name = "typePoleCircle";
            this.typePoleCircle.Size = new System.Drawing.Size(120, 38);
            this.typePoleCircle.TabIndex = 85;
            this.typePoleCircle.Text = "Pole (Circle)";
            this.typePoleCircle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.typePoleCircle.UseVisualStyleBackColor = false;
            // 
            // typeSheetAttack
            // 
            this.typeSheetAttack.AutoSize = true;
            this.typeSheetAttack.BackColor = System.Drawing.Color.Transparent;
            this.typeSheetAttack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typeSheetAttack.ForeColor = System.Drawing.Color.Maroon;
            this.typeSheetAttack.Location = new System.Drawing.Point(0, 0);
            this.typeSheetAttack.Margin = new System.Windows.Forms.Padding(0);
            this.typeSheetAttack.Name = "typeSheetAttack";
            this.typeSheetAttack.Size = new System.Drawing.Size(120, 38);
            this.typeSheetAttack.TabIndex = 86;
            this.typeSheetAttack.Text = "Sheet Attack";
            this.typeSheetAttack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.typeSheetAttack.UseVisualStyleBackColor = false;
            // 
            // typeShape
            // 
            this.typeShape.AutoSize = true;
            this.typeShape.BackColor = System.Drawing.Color.Transparent;
            this.typeShape.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typeShape.ForeColor = System.Drawing.Color.DarkGreen;
            this.typeShape.Location = new System.Drawing.Point(240, 38);
            this.typeShape.Margin = new System.Windows.Forms.Padding(0);
            this.typeShape.Name = "typeShape";
            this.typeShape.Size = new System.Drawing.Size(120, 38);
            this.typeShape.TabIndex = 84;
            this.typeShape.Text = "Shape";
            this.typeShape.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.typeShape.UseVisualStyleBackColor = false;
            // 
            // typePoleLine
            // 
            this.typePoleLine.AutoSize = true;
            this.typePoleLine.BackColor = System.Drawing.Color.Transparent;
            this.typePoleLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typePoleLine.ForeColor = System.Drawing.Color.Red;
            this.typePoleLine.Location = new System.Drawing.Point(120, 0);
            this.typePoleLine.Margin = new System.Windows.Forms.Padding(0);
            this.typePoleLine.Name = "typePoleLine";
            this.typePoleLine.Size = new System.Drawing.Size(120, 38);
            this.typePoleLine.TabIndex = 81;
            this.typePoleLine.Text = "Pole (Line)";
            this.typePoleLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.typePoleLine.UseVisualStyleBackColor = false;
            // 
            // typePinHole
            // 
            this.typePinHole.AutoSize = true;
            this.typePinHole.BackColor = System.Drawing.Color.Transparent;
            this.typePinHole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typePinHole.ForeColor = System.Drawing.Color.DarkMagenta;
            this.typePinHole.Location = new System.Drawing.Point(120, 38);
            this.typePinHole.Margin = new System.Windows.Forms.Padding(0);
            this.typePinHole.Name = "typePinHole";
            this.typePinHole.Size = new System.Drawing.Size(120, 38);
            this.typePinHole.TabIndex = 83;
            this.typePinHole.Text = "Pin Hole";
            this.typePinHole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.typePinHole.UseVisualStyleBackColor = false;
            // 
            // typeDielectric
            // 
            this.typeDielectric.AutoSize = true;
            this.typeDielectric.BackColor = System.Drawing.Color.Transparent;
            this.typeDielectric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typeDielectric.ForeColor = System.Drawing.Color.Blue;
            this.typeDielectric.Location = new System.Drawing.Point(0, 38);
            this.typeDielectric.Margin = new System.Windows.Forms.Padding(0);
            this.typeDielectric.Name = "typeDielectric";
            this.typeDielectric.Size = new System.Drawing.Size(120, 38);
            this.typeDielectric.TabIndex = 0;
            this.typeDielectric.Text = "White";
            this.typeDielectric.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.typeDielectric.UseVisualStyleBackColor = false;
            // 
            // typeTotal
            // 
            this.typeTotal.BackColor = System.Drawing.Color.Transparent;
            this.typeTotal.Checked = true;
            this.typeTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.typeTotal.Location = new System.Drawing.Point(0, 0);
            this.typeTotal.Margin = new System.Windows.Forms.Padding(0);
            this.typeTotal.Name = "typeTotal";
            this.typeTotal.Size = new System.Drawing.Size(80, 76);
            this.typeTotal.TabIndex = 82;
            this.typeTotal.TabStop = true;
            this.typeTotal.Text = "Total";
            this.typeTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.typeTotal.UseVisualStyleBackColor = false;
            // 
            // DefectTypePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutTotal);
            this.Name = "DefectTypePanel";
            this.Size = new System.Drawing.Size(442, 76);
            this.layoutTotal.ResumeLayout(false);
            this.layoutSubType.ResumeLayout(false);
            this.layoutSubType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutTotal;
        private System.Windows.Forms.TableLayoutPanel layoutSubType;
        private System.Windows.Forms.RadioButton typePoleCircle;
        private System.Windows.Forms.RadioButton typeSheetAttack;
        private System.Windows.Forms.RadioButton typeShape;
        private System.Windows.Forms.RadioButton typePoleLine;
        private System.Windows.Forms.RadioButton typePinHole;
        private System.Windows.Forms.RadioButton typeDielectric;
        private System.Windows.Forms.RadioButton typeTotal;
    }
}
