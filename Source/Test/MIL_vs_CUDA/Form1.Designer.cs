namespace MIL_vs_CUDA
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonMilStart = new System.Windows.Forms.Button();
            this.buttonEmguStart = new System.Windows.Forms.Button();
            this.buttonLogClear = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonLogSave = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.repeatCount = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonMilTestSerial = new System.Windows.Forms.Button();
            this.buttonCudaTestSerial = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.buttonMilTestParallel = new System.Windows.Forms.Button();
            this.buttonCudaTestParallel = new System.Windows.Forms.Button();
            this.splitCount = new System.Windows.Forms.NumericUpDown();
            this.labelRepeat = new System.Windows.Forms.Label();
            this.labelSplit = new System.Windows.Forms.Label();
            this.labelProgress = new System.Windows.Forms.Label();
            this.buttonTransferTest = new System.Windows.Forms.Button();
            this.includeSave = new System.Windows.Forms.CheckBox();
            this.buttonExport = new System.Windows.Forms.Button();
            this.multiLayerBuffer = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonConvertTest = new System.Windows.Forms.Button();
            this.buttonChildImageTest = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.calculatorVersion = new System.Windows.Forms.ComboBox();
            this.useParallel = new System.Windows.Forms.CheckBox();
            this.usePinnedMem = new System.Windows.Forms.CheckBox();
            this.buttonAllStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.useGPU = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repeatCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitCount)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonMilStart
            // 
            this.buttonMilStart.Location = new System.Drawing.Point(17, 47);
            this.buttonMilStart.Name = "buttonMilStart";
            this.buttonMilStart.Size = new System.Drawing.Size(120, 40);
            this.buttonMilStart.TabIndex = 0;
            this.buttonMilStart.Text = "Start MIL";
            this.buttonMilStart.UseVisualStyleBackColor = true;
            this.buttonMilStart.Click += new System.EventHandler(this.buttonMilStart_Click);
            // 
            // buttonEmguStart
            // 
            this.buttonEmguStart.Location = new System.Drawing.Point(148, 47);
            this.buttonEmguStart.Name = "buttonEmguStart";
            this.buttonEmguStart.Size = new System.Drawing.Size(120, 40);
            this.buttonEmguStart.TabIndex = 0;
            this.buttonEmguStart.Text = "Start EmguCv+GPU";
            this.buttonEmguStart.UseVisualStyleBackColor = true;
            this.buttonEmguStart.Click += new System.EventHandler(this.buttonEmguStart_Click);
            // 
            // buttonLogClear
            // 
            this.buttonLogClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLogClear.Location = new System.Drawing.Point(943, 679);
            this.buttonLogClear.Name = "buttonLogClear";
            this.buttonLogClear.Size = new System.Drawing.Size(149, 23);
            this.buttonLogClear.TabIndex = 0;
            this.buttonLogClear.Text = "Log Clear";
            this.buttonLogClear.UseVisualStyleBackColor = true;
            this.buttonLogClear.Click += new System.EventHandler(this.buttonLogClear_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(1078, 202);
            this.dataGridView1.TabIndex = 2;
            // 
            // buttonLogSave
            // 
            this.buttonLogSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLogSave.Location = new System.Drawing.Point(944, 650);
            this.buttonLogSave.Name = "buttonLogSave";
            this.buttonLogSave.Size = new System.Drawing.Size(149, 23);
            this.buttonLogSave.TabIndex = 0;
            this.buttonLogSave.Text = "Log Save";
            this.buttonLogSave.UseVisualStyleBackColor = true;
            this.buttonLogSave.Click += new System.EventHandler(this.buttonLogSave_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(3, 211);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(1078, 203);
            this.dataGridView2.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridView2, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 227);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1084, 417);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // buttonSelect
            // 
            this.buttonSelect.Location = new System.Drawing.Point(12, 12);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(71, 209);
            this.buttonSelect.TabIndex = 0;
            this.buttonSelect.Text = "Select Model";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // repeatCount
            // 
            this.repeatCount.Location = new System.Drawing.Point(77, 21);
            this.repeatCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.repeatCount.Name = "repeatCount";
            this.repeatCount.Size = new System.Drawing.Size(60, 21);
            this.repeatCount.TabIndex = 4;
            this.repeatCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(12, 679);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(925, 23);
            this.textBox1.TabIndex = 5;
            // 
            // buttonMilTestSerial
            // 
            this.buttonMilTestSerial.Location = new System.Drawing.Point(15, 54);
            this.buttonMilTestSerial.Name = "buttonMilTestSerial";
            this.buttonMilTestSerial.Size = new System.Drawing.Size(130, 39);
            this.buttonMilTestSerial.TabIndex = 0;
            this.buttonMilTestSerial.Text = "Test Mil Serial";
            this.buttonMilTestSerial.UseVisualStyleBackColor = true;
            this.buttonMilTestSerial.Click += new System.EventHandler(this.buttonMilTestSerial_Click);
            // 
            // buttonCudaTestSerial
            // 
            this.buttonCudaTestSerial.Location = new System.Drawing.Point(287, 54);
            this.buttonCudaTestSerial.Name = "buttonCudaTestSerial";
            this.buttonCudaTestSerial.Size = new System.Drawing.Size(130, 39);
            this.buttonCudaTestSerial.TabIndex = 0;
            this.buttonCudaTestSerial.Text = "Test CUDA Serial";
            this.buttonCudaTestSerial.UseVisualStyleBackColor = true;
            this.buttonCudaTestSerial.Click += new System.EventHandler(this.buttonCudaTestSerial_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(12, 650);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(853, 23);
            this.progressBar1.TabIndex = 6;
            // 
            // buttonMilTestParallel
            // 
            this.buttonMilTestParallel.Location = new System.Drawing.Point(151, 54);
            this.buttonMilTestParallel.Name = "buttonMilTestParallel";
            this.buttonMilTestParallel.Size = new System.Drawing.Size(130, 39);
            this.buttonMilTestParallel.TabIndex = 0;
            this.buttonMilTestParallel.Text = "Test Mil Parallel";
            this.buttonMilTestParallel.UseVisualStyleBackColor = true;
            this.buttonMilTestParallel.Click += new System.EventHandler(this.buttonMilTestParallel_Click);
            // 
            // buttonCudaTestParallel
            // 
            this.buttonCudaTestParallel.Location = new System.Drawing.Point(423, 54);
            this.buttonCudaTestParallel.Name = "buttonCudaTestParallel";
            this.buttonCudaTestParallel.Size = new System.Drawing.Size(130, 39);
            this.buttonCudaTestParallel.TabIndex = 0;
            this.buttonCudaTestParallel.Text = "Test CUDA Parallel";
            this.buttonCudaTestParallel.UseVisualStyleBackColor = true;
            this.buttonCudaTestParallel.Click += new System.EventHandler(this.buttonCudaTestParallel_Click);
            // 
            // splitCount
            // 
            this.splitCount.Location = new System.Drawing.Point(45, 24);
            this.splitCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.splitCount.Name = "splitCount";
            this.splitCount.Size = new System.Drawing.Size(75, 21);
            this.splitCount.TabIndex = 4;
            this.splitCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelRepeat
            // 
            this.labelRepeat.Location = new System.Drawing.Point(15, 21);
            this.labelRepeat.Name = "labelRepeat";
            this.labelRepeat.Size = new System.Drawing.Size(60, 21);
            this.labelRepeat.TabIndex = 7;
            this.labelRepeat.Text = "Repeat";
            this.labelRepeat.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSplit
            // 
            this.labelSplit.AutoSize = true;
            this.labelSplit.Location = new System.Drawing.Point(13, 28);
            this.labelSplit.Name = "labelSplit";
            this.labelSplit.Size = new System.Drawing.Size(29, 12);
            this.labelSplit.TabIndex = 7;
            this.labelSplit.Text = "Split";
            // 
            // labelProgress
            // 
            this.labelProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProgress.Location = new System.Drawing.Point(871, 650);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(66, 23);
            this.labelProgress.TabIndex = 7;
            this.labelProgress.Text = "0/0";
            this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonTransferTest
            // 
            this.buttonTransferTest.Location = new System.Drawing.Point(6, 21);
            this.buttonTransferTest.Name = "buttonTransferTest";
            this.buttonTransferTest.Size = new System.Drawing.Size(120, 40);
            this.buttonTransferTest.TabIndex = 0;
            this.buttonTransferTest.Text = "Transfer";
            this.buttonTransferTest.UseVisualStyleBackColor = true;
            this.buttonTransferTest.Click += new System.EventHandler(this.buttonTransferTest_Click);
            // 
            // includeSave
            // 
            this.includeSave.Location = new System.Drawing.Point(151, 23);
            this.includeSave.Name = "includeSave";
            this.includeSave.Size = new System.Drawing.Size(120, 21);
            this.includeSave.TabIndex = 8;
            this.includeSave.Text = "Include Save";
            this.includeSave.UseVisualStyleBackColor = true;
            this.includeSave.CheckedChanged += new System.EventHandler(this.includeSave_CheckedChanged);
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(142, 53);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(120, 40);
            this.buttonExport.TabIndex = 0;
            this.buttonExport.Text = "Export Last Result";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // multiLayerBuffer
            // 
            this.multiLayerBuffer.Location = new System.Drawing.Point(151, 21);
            this.multiLayerBuffer.Name = "multiLayerBuffer";
            this.multiLayerBuffer.Size = new System.Drawing.Size(120, 21);
            this.multiLayerBuffer.TabIndex = 8;
            this.multiLayerBuffer.Text = "MultiLayer Buffer";
            this.multiLayerBuffer.UseVisualStyleBackColor = true;
            this.multiLayerBuffer.CheckedChanged += new System.EventHandler(this.includeSave_CheckedChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(6, 53);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(120, 40);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonConvertTest
            // 
            this.buttonConvertTest.Location = new System.Drawing.Point(132, 21);
            this.buttonConvertTest.Name = "buttonConvertTest";
            this.buttonConvertTest.Size = new System.Drawing.Size(120, 40);
            this.buttonConvertTest.TabIndex = 0;
            this.buttonConvertTest.Text = "Convert";
            this.buttonConvertTest.UseVisualStyleBackColor = true;
            this.buttonConvertTest.Click += new System.EventHandler(this.buttonConvertTest_Click);
            // 
            // buttonChildImageTest
            // 
            this.buttonChildImageTest.Location = new System.Drawing.Point(258, 21);
            this.buttonChildImageTest.Name = "buttonChildImageTest";
            this.buttonChildImageTest.Size = new System.Drawing.Size(120, 40);
            this.buttonChildImageTest.TabIndex = 0;
            this.buttonChildImageTest.Text = "Child";
            this.buttonChildImageTest.UseVisualStyleBackColor = true;
            this.buttonChildImageTest.Click += new System.EventHandler(this.buttonChildImageTest_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.splitCount);
            this.groupBox1.Controls.Add(this.includeSave);
            this.groupBox1.Controls.Add(this.buttonMilTestSerial);
            this.groupBox1.Controls.Add(this.buttonMilTestParallel);
            this.groupBox1.Controls.Add(this.buttonCudaTestSerial);
            this.groupBox1.Controls.Add(this.labelSplit);
            this.groupBox1.Controls.Add(this.buttonCudaTestParallel);
            this.groupBox1.Location = new System.Drawing.Point(89, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(563, 103);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Processing Test";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.calculatorVersion);
            this.groupBox2.Controls.Add(this.buttonMilStart);
            this.groupBox2.Controls.Add(this.useParallel);
            this.groupBox2.Controls.Add(this.useGPU);
            this.groupBox2.Controls.Add(this.usePinnedMem);
            this.groupBox2.Controls.Add(this.multiLayerBuffer);
            this.groupBox2.Controls.Add(this.buttonAllStart);
            this.groupBox2.Controls.Add(this.buttonEmguStart);
            this.groupBox2.Controls.Add(this.repeatCount);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.labelRepeat);
            this.groupBox2.Location = new System.Drawing.Point(89, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(583, 100);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Inspection";
            // 
            // calculatorVersion
            // 
            this.calculatorVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.calculatorVersion.FormattingEnabled = true;
            this.calculatorVersion.Location = new System.Drawing.Point(471, 58);
            this.calculatorVersion.Name = "calculatorVersion";
            this.calculatorVersion.Size = new System.Drawing.Size(64, 20);
            this.calculatorVersion.TabIndex = 9;
            this.calculatorVersion.SelectedIndexChanged += new System.EventHandler(this.calculatorVersion_SelectedIndexChanged);
            this.calculatorVersion.DataSourceChanged += new System.EventHandler(this.calculatorVersion_DataSourceChanged);
            // 
            // useParallel
            // 
            this.useParallel.Checked = true;
            this.useParallel.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.useParallel.Location = new System.Drawing.Point(461, 20);
            this.useParallel.Name = "useParallel";
            this.useParallel.Size = new System.Drawing.Size(120, 21);
            this.useParallel.TabIndex = 8;
            this.useParallel.Text = "Parallel";
            this.useParallel.ThreeState = true;
            this.useParallel.UseVisualStyleBackColor = true;
            this.useParallel.CheckedChanged += new System.EventHandler(this.includeSave_CheckedChanged);
            // 
            // usePinnedMem
            // 
            this.usePinnedMem.Checked = true;
            this.usePinnedMem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.usePinnedMem.Location = new System.Drawing.Point(279, 21);
            this.usePinnedMem.Name = "usePinnedMem";
            this.usePinnedMem.Size = new System.Drawing.Size(120, 21);
            this.usePinnedMem.TabIndex = 8;
            this.usePinnedMem.Text = "Pinned Mem";
            this.usePinnedMem.UseVisualStyleBackColor = true;
            this.usePinnedMem.CheckedChanged += new System.EventHandler(this.includeSave_CheckedChanged);
            // 
            // buttonAllStart
            // 
            this.buttonAllStart.Location = new System.Drawing.Point(277, 47);
            this.buttonAllStart.Name = "buttonAllStart";
            this.buttonAllStart.Size = new System.Drawing.Size(120, 40);
            this.buttonAllStart.TabIndex = 0;
            this.buttonAllStart.Text = "Start ALL";
            this.buttonAllStart.UseVisualStyleBackColor = true;
            this.buttonAllStart.Click += new System.EventHandler(this.buttonAllStart_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(405, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Calc. Ver";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.buttonTransferTest);
            this.groupBox3.Controls.Add(this.buttonConvertTest);
            this.groupBox3.Controls.Add(this.buttonChildImageTest);
            this.groupBox3.Location = new System.Drawing.Point(705, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(391, 100);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Etc. Test";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.buttonCancel);
            this.groupBox4.Controls.Add(this.buttonExport);
            this.groupBox4.Location = new System.Drawing.Point(705, 118);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(391, 103);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Control";
            // 
            // useGPU
            // 
            this.useGPU.AutoSize = true;
            this.useGPU.Location = new System.Drawing.Point(380, 23);
            this.useGPU.Name = "useGPU";
            this.useGPU.Size = new System.Drawing.Size(75, 16);
            this.useGPU.TabIndex = 8;
            this.useGPU.Text = "Use GPU";
            this.useGPU.UseVisualStyleBackColor = true;
            this.useGPU.CheckedChanged += new System.EventHandler(this.includeSave_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 707);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelProgress);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.buttonLogSave);
            this.Controls.Add(this.buttonLogClear);
            this.Controls.Add(this.buttonSelect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.repeatCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitCount)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonMilStart;
        private System.Windows.Forms.Button buttonEmguStart;
        private System.Windows.Forms.Button buttonLogClear;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonLogSave;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.NumericUpDown repeatCount;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonMilTestSerial;
        private System.Windows.Forms.Button buttonCudaTestSerial;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button buttonMilTestParallel;
        private System.Windows.Forms.Button buttonCudaTestParallel;
        private System.Windows.Forms.NumericUpDown splitCount;
        private System.Windows.Forms.Label labelRepeat;
        private System.Windows.Forms.Label labelSplit;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Button buttonTransferTest;
        private System.Windows.Forms.CheckBox includeSave;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.CheckBox multiLayerBuffer;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonConvertTest;
        private System.Windows.Forms.Button buttonChildImageTest;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox usePinnedMem;
        private System.Windows.Forms.Button buttonAllStart;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox calculatorVersion;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox useParallel;
        private System.Windows.Forms.CheckBox useGPU;
    }
}

