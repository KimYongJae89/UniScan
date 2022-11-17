namespace UniScanM.RVMS.UI
{
    partial class InspectionPanelLeft
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
            this.VibrationViewPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panelManSide = new System.Windows.Forms.Panel();
            this.manRaw = new System.Windows.Forms.Label();
            this.labelMan = new System.Windows.Forms.Label();
            this.panelGearSide = new System.Windows.Forms.Panel();
            this.gearRaw = new System.Windows.Forms.Label();
            this.labelGear = new System.Windows.Forms.Label();
            this.VibrationViewPanel.SuspendLayout();
            this.panelManSide.SuspendLayout();
            this.panelGearSide.SuspendLayout();
            this.SuspendLayout();
            // 
            // VibrationViewPanel
            // 
            this.VibrationViewPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.VibrationViewPanel.ColumnCount = 2;
            this.VibrationViewPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.VibrationViewPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.VibrationViewPanel.Controls.Add(this.panelManSide, 1, 0);
            this.VibrationViewPanel.Controls.Add(this.panelGearSide, 0, 0);
            this.VibrationViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VibrationViewPanel.Location = new System.Drawing.Point(0, 0);
            this.VibrationViewPanel.Margin = new System.Windows.Forms.Padding(0);
            this.VibrationViewPanel.Name = "VibrationViewPanel";
            this.VibrationViewPanel.RowCount = 3;
            this.VibrationViewPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.VibrationViewPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 61.60337F));
            this.VibrationViewPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 38.39663F));
            this.VibrationViewPanel.Size = new System.Drawing.Size(715, 524);
            this.VibrationViewPanel.TabIndex = 29;
            // 
            // panelManSide
            // 
            this.panelManSide.Controls.Add(this.manRaw);
            this.panelManSide.Controls.Add(this.labelMan);
            this.panelManSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelManSide.Location = new System.Drawing.Point(361, 4);
            this.panelManSide.Name = "panelManSide";
            this.panelManSide.Size = new System.Drawing.Size(350, 54);
            this.panelManSide.TabIndex = 40;
            // 
            // manRaw
            // 
            this.manRaw.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.manRaw.Dock = System.Windows.Forms.DockStyle.Right;
            this.manRaw.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.manRaw.Location = new System.Drawing.Point(270, 0);
            this.manRaw.Margin = new System.Windows.Forms.Padding(0);
            this.manRaw.Name = "manRaw";
            this.manRaw.Size = new System.Drawing.Size(80, 54);
            this.manRaw.TabIndex = 38;
            this.manRaw.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // labelMan
            // 
            this.labelMan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelMan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMan.Font = new System.Drawing.Font("Malgun Gothic", 18F, System.Drawing.FontStyle.Bold);
            this.labelMan.Location = new System.Drawing.Point(0, 0);
            this.labelMan.Margin = new System.Windows.Forms.Padding(0);
            this.labelMan.Name = "labelMan";
            this.labelMan.Size = new System.Drawing.Size(350, 54);
            this.labelMan.TabIndex = 38;
            this.labelMan.Text = "Man Side";
            this.labelMan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelGearSide
            // 
            this.panelGearSide.Controls.Add(this.gearRaw);
            this.panelGearSide.Controls.Add(this.labelGear);
            this.panelGearSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGearSide.Location = new System.Drawing.Point(4, 4);
            this.panelGearSide.Name = "panelGearSide";
            this.panelGearSide.Size = new System.Drawing.Size(350, 54);
            this.panelGearSide.TabIndex = 41;
            // 
            // gearRaw
            // 
            this.gearRaw.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.gearRaw.Dock = System.Windows.Forms.DockStyle.Right;
            this.gearRaw.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.gearRaw.Location = new System.Drawing.Point(270, 0);
            this.gearRaw.Margin = new System.Windows.Forms.Padding(0);
            this.gearRaw.Name = "gearRaw";
            this.gearRaw.Size = new System.Drawing.Size(80, 54);
            this.gearRaw.TabIndex = 38;
            this.gearRaw.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // labelGear
            // 
            this.labelGear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelGear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGear.Font = new System.Drawing.Font("Malgun Gothic", 18F, System.Drawing.FontStyle.Bold);
            this.labelGear.Location = new System.Drawing.Point(0, 0);
            this.labelGear.Margin = new System.Windows.Forms.Padding(0);
            this.labelGear.Name = "labelGear";
            this.labelGear.Size = new System.Drawing.Size(350, 54);
            this.labelGear.TabIndex = 37;
            this.labelGear.Text = "Gear Side";
            this.labelGear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InspectionPanelLeft
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.VibrationViewPanel);
            this.Name = "InspectionPanelLeft";
            this.Size = new System.Drawing.Size(715, 524);
            this.VibrationViewPanel.ResumeLayout(false);
            this.panelManSide.ResumeLayout(false);
            this.panelGearSide.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel VibrationViewPanel;
        private System.Windows.Forms.Label labelGear;
        private System.Windows.Forms.Label labelMan;
        private System.Windows.Forms.Label manRaw;
        private System.Windows.Forms.Label gearRaw;
        private System.Windows.Forms.Panel panelManSide;
        private System.Windows.Forms.Panel panelGearSide;
    }
}
