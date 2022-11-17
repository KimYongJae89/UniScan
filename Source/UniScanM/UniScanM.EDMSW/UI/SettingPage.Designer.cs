namespace UniScanM.EDMSW.UI
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
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.buttonOK = new Infragistics.Win.Misc.UltraButton();
            this.layoutButton = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCamera = new System.Windows.Forms.Button();
            this.layoutButton.SuspendLayout();
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
            this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid.Size = new System.Drawing.Size(1755, 868);
            this.propertyGrid.TabIndex = 10;
            this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
            this.propertyGrid.Click += new System.EventHandler(this.propertyGrid_Click);
            // 
            // buttonOK
            // 
            appearance2.FontData.BoldAsString = "False";
            appearance2.FontData.Name = "맑은 고딕";
            appearance2.FontData.SizeInPoints = 12F;
            this.buttonOK.Appearance = appearance2;
            this.buttonOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOK.Location = new System.Drawing.Point(744, 6);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(267, 72);
            this.buttonOK.TabIndex = 12;
            this.buttonOK.Text = "Apply";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // layoutButton
            // 
            this.layoutButton.ColumnCount = 3;
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 279F));
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.layoutButton.Controls.Add(this.buttonCamera, 0, 0);
            this.layoutButton.Controls.Add(this.buttonOK, 1, 0);
            this.layoutButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.layoutButton.Location = new System.Drawing.Point(0, 868);
            this.layoutButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.layoutButton.Name = "layoutButton";
            this.layoutButton.RowCount = 1;
            this.layoutButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutButton.Size = new System.Drawing.Size(1755, 84);
            this.layoutButton.TabIndex = 13;
            // 
            // buttonCamera
            // 
            this.buttonCamera.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonCamera.Location = new System.Drawing.Point(6, 4);
            this.buttonCamera.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.buttonCamera.Name = "buttonCamera";
            this.buttonCamera.Size = new System.Drawing.Size(158, 76);
            this.buttonCamera.TabIndex = 13;
            this.buttonCamera.Text = "Camera";
            this.buttonCamera.UseVisualStyleBackColor = true;
            this.buttonCamera.Click += new System.EventHandler(this.buttonCamera_Click);
            // 
            // SettingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.propertyGrid);
            this.Controls.Add(this.layoutButton);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "SettingPage";
            this.Size = new System.Drawing.Size(1755, 952);
            this.layoutButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGrid;
        private Infragistics.Win.Misc.UltraButton buttonOK;
        private System.Windows.Forms.TableLayoutPanel layoutButton;
        private System.Windows.Forms.Button buttonCamera;
    }
}
