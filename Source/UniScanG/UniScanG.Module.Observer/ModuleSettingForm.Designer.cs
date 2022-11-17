namespace UniScanG.Module.Observer
{
    partial class ModuleSettingForm
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.groupBoxResult = new System.Windows.Forms.GroupBox();
            this.storeIn = new System.Windows.Forms.NumericUpDown();
            this.buttonPathFind = new System.Windows.Forms.Button();
            this.labelStoreInUnit = new System.Windows.Forms.Label();
            this.labelStoreIn = new System.Windows.Forms.Label();
            this.labelPath = new System.Windows.Forms.Label();
            this.resultPath = new System.Windows.Forms.TextBox();
            this.groupBoxModule = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBoxResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.storeIn)).BeginInit();
            this.groupBoxModule.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 20);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(765, 256);
            this.dataGridView1.TabIndex = 0;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(12, 367);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(108, 43);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(159, 367);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(108, 43);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(296, 367);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(108, 43);
            this.buttonTest.TabIndex = 1;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // groupBoxResult
            // 
            this.groupBoxResult.Controls.Add(this.storeIn);
            this.groupBoxResult.Controls.Add(this.buttonPathFind);
            this.groupBoxResult.Controls.Add(this.labelStoreInUnit);
            this.groupBoxResult.Controls.Add(this.labelStoreIn);
            this.groupBoxResult.Controls.Add(this.labelPath);
            this.groupBoxResult.Controls.Add(this.resultPath);
            this.groupBoxResult.Location = new System.Drawing.Point(12, 13);
            this.groupBoxResult.Name = "groupBoxResult";
            this.groupBoxResult.Size = new System.Drawing.Size(777, 60);
            this.groupBoxResult.TabIndex = 2;
            this.groupBoxResult.TabStop = false;
            this.groupBoxResult.Text = "Result";
            // 
            // storeIn
            // 
            this.storeIn.Location = new System.Drawing.Point(646, 26);
            this.storeIn.Name = "storeIn";
            this.storeIn.Size = new System.Drawing.Size(64, 21);
            this.storeIn.TabIndex = 3;
            // 
            // buttonPathFind
            // 
            this.buttonPathFind.Location = new System.Drawing.Point(473, 25);
            this.buttonPathFind.Name = "buttonPathFind";
            this.buttonPathFind.Size = new System.Drawing.Size(36, 23);
            this.buttonPathFind.TabIndex = 2;
            this.buttonPathFind.Text = "...";
            this.buttonPathFind.UseVisualStyleBackColor = true;
            // 
            // labelStoreInUnit
            // 
            this.labelStoreInUnit.AutoSize = true;
            this.labelStoreInUnit.Location = new System.Drawing.Point(716, 30);
            this.labelStoreInUnit.Name = "labelStoreInUnit";
            this.labelStoreInUnit.Size = new System.Drawing.Size(44, 12);
            this.labelStoreInUnit.TabIndex = 1;
            this.labelStoreInUnit.Text = "Day(s)";
            // 
            // labelStoreIn
            // 
            this.labelStoreIn.AutoSize = true;
            this.labelStoreIn.Location = new System.Drawing.Point(592, 30);
            this.labelStoreIn.Name = "labelStoreIn";
            this.labelStoreIn.Size = new System.Drawing.Size(48, 12);
            this.labelStoreIn.TabIndex = 1;
            this.labelStoreIn.Text = "Store in";
            // 
            // labelPath
            // 
            this.labelPath.AutoSize = true;
            this.labelPath.Location = new System.Drawing.Point(19, 30);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(30, 12);
            this.labelPath.TabIndex = 1;
            this.labelPath.Text = "Path";
            // 
            // resultPath
            // 
            this.resultPath.Location = new System.Drawing.Point(55, 26);
            this.resultPath.Name = "resultPath";
            this.resultPath.Size = new System.Drawing.Size(412, 21);
            this.resultPath.TabIndex = 0;
            // 
            // groupBoxModule
            // 
            this.groupBoxModule.Controls.Add(this.dataGridView1);
            this.groupBoxModule.Location = new System.Drawing.Point(12, 79);
            this.groupBoxModule.Name = "groupBoxModule";
            this.groupBoxModule.Size = new System.Drawing.Size(777, 282);
            this.groupBoxModule.TabIndex = 3;
            this.groupBoxModule.TabStop = false;
            this.groupBoxModule.Text = "Module";
            // 
            // ModuleSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 416);
            this.Controls.Add(this.groupBoxModule);
            this.Controls.Add(this.groupBoxResult);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Name = "ModuleSettingForm";
            this.Text = "ModuleSettingForm";
            this.Load += new System.EventHandler(this.ModuleSettingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBoxResult.ResumeLayout(false);
            this.groupBoxResult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.storeIn)).EndInit();
            this.groupBoxModule.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.GroupBox groupBoxResult;
        private System.Windows.Forms.Button buttonPathFind;
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.TextBox resultPath;
        private System.Windows.Forms.NumericUpDown storeIn;
        private System.Windows.Forms.Label labelStoreInUnit;
        private System.Windows.Forms.Label labelStoreIn;
        private System.Windows.Forms.GroupBox groupBoxModule;
    }
}