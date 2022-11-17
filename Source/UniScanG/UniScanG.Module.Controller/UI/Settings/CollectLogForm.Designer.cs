namespace UniScanG.Module.Controller.UI.Settings
{
    partial class CollectLogForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonStart = new System.Windows.Forms.Button();
            this.comboBoxDays = new System.Windows.Forms.ComboBox();
            this.labelDays = new System.Windows.Forms.Label();
            this.buttonUpdateSize = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 38);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(776, 151);
            this.dataGridView1.TabIndex = 0;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(673, 195);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(115, 53);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // comboBoxDays
            // 
            this.comboBoxDays.DropDownHeight = 100;
            this.comboBoxDays.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDays.FormattingEnabled = true;
            this.comboBoxDays.IntegralHeight = false;
            this.comboBoxDays.Location = new System.Drawing.Point(98, 12);
            this.comboBoxDays.Name = "comboBoxDays";
            this.comboBoxDays.Size = new System.Drawing.Size(71, 20);
            this.comboBoxDays.TabIndex = 2;
            // 
            // labelDays
            // 
            this.labelDays.AutoSize = true;
            this.labelDays.Location = new System.Drawing.Point(12, 15);
            this.labelDays.Name = "labelDays";
            this.labelDays.Size = new System.Drawing.Size(80, 12);
            this.labelDays.TabIndex = 3;
            this.labelDays.Text = "Backup Days";
            // 
            // buttonUpdateSize
            // 
            this.buttonUpdateSize.Location = new System.Drawing.Point(552, 195);
            this.buttonUpdateSize.Name = "buttonUpdateSize";
            this.buttonUpdateSize.Size = new System.Drawing.Size(115, 53);
            this.buttonUpdateSize.TabIndex = 1;
            this.buttonUpdateSize.Text = "Update Size";
            this.buttonUpdateSize.UseVisualStyleBackColor = true;
            this.buttonUpdateSize.Click += new System.EventHandler(this.buttonUpdateSize_Click);
            // 
            // CollectLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 256);
            this.Controls.Add(this.labelDays);
            this.Controls.Add(this.comboBoxDays);
            this.Controls.Add(this.buttonUpdateSize);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.dataGridView1);
            this.Name = "CollectLogForm";
            this.Text = "CollectLogForm";
            this.Load += new System.EventHandler(this.CollectLogForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.ComboBox comboBoxDays;
        private System.Windows.Forms.Label labelDays;
        private System.Windows.Forms.Button buttonUpdateSize;
    }
}