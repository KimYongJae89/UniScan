namespace UniScanM.Gloss.UI
{
    partial class SettingPage
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
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonOK = new Infragistics.Win.Misc.UltraButton();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.buttonWidthSetting = new Infragistics.Win.Misc.UltraButton();
            this.layoutMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Margin = new System.Windows.Forms.Padding(0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(456, 434);
            this.propertyGrid.TabIndex = 10;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            // 
            // buttonOK
            // 
            appearance1.FontData.BoldAsString = "False";
            appearance1.FontData.Name = "맑은 고딕";
            appearance1.FontData.SizeInPoints = 12F;
            this.buttonOK.Appearance = appearance1;
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOK.Location = new System.Drawing.Point(3, 437);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(450, 36);
            this.buttonOK.TabIndex = 12;
            this.buttonOK.Text = "Apply";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.buttonWidthSetting, 0, 1);
            this.layoutMain.Controls.Add(this.buttonOK, 0, 2);
            this.layoutMain.Controls.Add(this.propertyGrid, 0, 0);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 3;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.layoutMain.Size = new System.Drawing.Size(456, 476);
            this.layoutMain.TabIndex = 13;
            // 
            // buttonWidthSetting
            // 
            appearance2.FontData.BoldAsString = "False";
            appearance2.FontData.Name = "맑은 고딕";
            appearance2.FontData.SizeInPoints = 12F;
            this.buttonWidthSetting.Appearance = appearance2;
            this.buttonWidthSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonWidthSetting.Location = new System.Drawing.Point(3, 437);
            this.buttonWidthSetting.Name = "buttonWidthSetting";
            this.buttonWidthSetting.Size = new System.Drawing.Size(450, 1);
            this.buttonWidthSetting.TabIndex = 13;
            this.buttonWidthSetting.Text = "Width Setting";
            this.buttonWidthSetting.Click += new System.EventHandler(this.buttonWidthSetting_Click);
            // 
            // SettingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutMain);
            this.Name = "SettingPage";
            this.Size = new System.Drawing.Size(456, 476);
            this.layoutMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        private Infragistics.Win.Misc.UltraButton buttonOK;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private Infragistics.Win.Misc.UltraButton buttonWidthSetting;
    }
}
