namespace UniScanG.UI.Etc
{
    partial class ContextInfoForm
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
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.image = new System.Windows.Forms.PictureBox();
            this.defectType = new System.Windows.Forms.Label();
            this.infoText = new System.Windows.Forms.Label();
            this.buttonCapture = new System.Windows.Forms.Button();
            this.layoutMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.image)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.BackColor = System.Drawing.Color.AliceBlue;
            this.layoutMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Outset;
            this.layoutMain.ColumnCount = 3;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.5F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.layoutMain.Controls.Add(this.image, 0, 0);
            this.layoutMain.Controls.Add(this.defectType, 1, 0);
            this.layoutMain.Controls.Add(this.infoText, 1, 1);
            this.layoutMain.Controls.Add(this.buttonCapture, 2, 0);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 2;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.73737F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 76.26263F));
            this.layoutMain.Size = new System.Drawing.Size(569, 200);
            this.layoutMain.TabIndex = 0;
            // 
            // image
            // 
            this.image.Dock = System.Windows.Forms.DockStyle.Fill;
            this.image.Location = new System.Drawing.Point(2, 2);
            this.image.Margin = new System.Windows.Forms.Padding(0);
            this.image.Name = "image";
            this.layoutMain.SetRowSpan(this.image, 2);
            this.image.Size = new System.Drawing.Size(193, 196);
            this.image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.image.TabIndex = 2;
            this.image.TabStop = false;
            this.image.Click += new System.EventHandler(this.image_Click);
            // 
            // defectType
            // 
            this.defectType.AutoSize = true;
            this.defectType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.defectType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectType.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.defectType.Location = new System.Drawing.Point(197, 2);
            this.defectType.Margin = new System.Windows.Forms.Padding(0);
            this.defectType.Name = "defectType";
            this.defectType.Size = new System.Drawing.Size(321, 46);
            this.defectType.TabIndex = 0;
            this.defectType.Text = "Defect Type";
            this.defectType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infoText
            // 
            this.infoText.AutoSize = true;
            this.layoutMain.SetColumnSpan(this.infoText, 2);
            this.infoText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoText.Location = new System.Drawing.Point(197, 50);
            this.infoText.Margin = new System.Windows.Forms.Padding(0);
            this.infoText.Name = "infoText";
            this.infoText.Size = new System.Drawing.Size(370, 148);
            this.infoText.TabIndex = 1;
            this.infoText.Text = "Type";
            this.infoText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonCapture
            // 
            this.buttonCapture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCapture.Location = new System.Drawing.Point(523, 5);
            this.buttonCapture.Name = "buttonCapture";
            this.buttonCapture.Size = new System.Drawing.Size(41, 40);
            this.buttonCapture.TabIndex = 3;
            this.buttonCapture.Text = "C";
            this.buttonCapture.UseVisualStyleBackColor = true;
            this.buttonCapture.Click += new System.EventHandler(this.buttonCapture_Click);
            // 
            // ContextInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 200);
            this.Controls.Add(this.layoutMain);
            this.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.Name = "ContextInfoForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ContextInfoForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ContextInfoForm_Load);
            this.MouseLeave += new System.EventHandler(this.ContextInfoForm_MouseLeave);
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.image)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.Label defectType;
        private System.Windows.Forms.Label infoText;
        private System.Windows.Forms.PictureBox image;
        private System.Windows.Forms.Button buttonCapture;
    }
}