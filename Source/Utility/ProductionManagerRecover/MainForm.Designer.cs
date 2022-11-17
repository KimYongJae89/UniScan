namespace ProductionManagerRecover
{
    partial class MainForm
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
            this.buttonLoadFromXml = new System.Windows.Forms.Button();
            this.buttonLoadFromResult = new System.Windows.Forms.Button();
            this.buttonSaveToXml = new System.Windows.Forms.Button();
            this.buttonAppendFromXml = new System.Windows.Forms.Button();
            this.buttonAppendFromResult = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(596, 526);
            this.dataGridView1.TabIndex = 0;
            // 
            // buttonLoadFromXml
            // 
            this.buttonLoadFromXml.Location = new System.Drawing.Point(614, 12);
            this.buttonLoadFromXml.Name = "buttonLoadFromXml";
            this.buttonLoadFromXml.Size = new System.Drawing.Size(134, 68);
            this.buttonLoadFromXml.TabIndex = 1;
            this.buttonLoadFromXml.Text = "Load From Xml";
            this.buttonLoadFromXml.UseVisualStyleBackColor = true;
            this.buttonLoadFromXml.Click += new System.EventHandler(this.buttonLoadFromXml_Click);
            // 
            // buttonLoadFromResult
            // 
            this.buttonLoadFromResult.Location = new System.Drawing.Point(614, 86);
            this.buttonLoadFromResult.Name = "buttonLoadFromResult";
            this.buttonLoadFromResult.Size = new System.Drawing.Size(134, 68);
            this.buttonLoadFromResult.TabIndex = 1;
            this.buttonLoadFromResult.Text = "Load From Result";
            this.buttonLoadFromResult.UseVisualStyleBackColor = true;
            this.buttonLoadFromResult.Click += new System.EventHandler(this.buttonLoadFromResult_Click);
            // 
            // buttonSaveToXml
            // 
            this.buttonSaveToXml.Location = new System.Drawing.Point(614, 220);
            this.buttonSaveToXml.Name = "buttonSaveToXml";
            this.buttonSaveToXml.Size = new System.Drawing.Size(134, 68);
            this.buttonSaveToXml.TabIndex = 1;
            this.buttonSaveToXml.Text = "Save To XML";
            this.buttonSaveToXml.UseVisualStyleBackColor = true;
            this.buttonSaveToXml.Click += new System.EventHandler(this.buttonSaveToXml_Click);
            // 
            // buttonAppendFromXml
            // 
            this.buttonAppendFromXml.Location = new System.Drawing.Point(754, 12);
            this.buttonAppendFromXml.Name = "buttonAppendFromXml";
            this.buttonAppendFromXml.Size = new System.Drawing.Size(134, 68);
            this.buttonAppendFromXml.TabIndex = 1;
            this.buttonAppendFromXml.Text = "Append From Xml";
            this.buttonAppendFromXml.UseVisualStyleBackColor = true;
            this.buttonAppendFromXml.Click += new System.EventHandler(this.buttonAppendFromXml_Click);
            // 
            // buttonAppendFromResult
            // 
            this.buttonAppendFromResult.Location = new System.Drawing.Point(754, 86);
            this.buttonAppendFromResult.Name = "buttonAppendFromResult";
            this.buttonAppendFromResult.Size = new System.Drawing.Size(134, 68);
            this.buttonAppendFromResult.TabIndex = 1;
            this.buttonAppendFromResult.Text = "Append From Result";
            this.buttonAppendFromResult.UseVisualStyleBackColor = true;
            this.buttonAppendFromResult.Click += new System.EventHandler(this.buttonAppendFromResult_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(754, 220);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(134, 68);
            this.buttonClear.TabIndex = 1;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.textBox2.Location = new System.Drawing.Point(682, 368);
            this.textBox2.Margin = new System.Windows.Forms.Padding(0);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(121, 20);
            this.textBox2.TabIndex = 2;
            this.textBox2.Text = "ABCD123";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 550);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonSaveToXml);
            this.Controls.Add(this.buttonAppendFromResult);
            this.Controls.Add(this.buttonLoadFromResult);
            this.Controls.Add(this.buttonAppendFromXml);
            this.Controls.Add(this.buttonLoadFromXml);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MainForm";
            this.Text = "PM Recover @ UniEye";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonLoadFromXml;
        private System.Windows.Forms.Button buttonLoadFromResult;
        private System.Windows.Forms.Button buttonSaveToXml;
        private System.Windows.Forms.Button buttonAppendFromXml;
        private System.Windows.Forms.Button buttonAppendFromResult;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.TextBox textBox2;
    }
}

