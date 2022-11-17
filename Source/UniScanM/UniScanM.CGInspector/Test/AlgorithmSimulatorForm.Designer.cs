namespace UniScanM.CGInspector.Test
{
    partial class AlgorithmSimulatorForm
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
            this.panelCanvas = new System.Windows.Forms.Panel();
            this.panelResult = new System.Windows.Forms.Panel();
            this.panelCommand = new System.Windows.Forms.Panel();
            this.fastDraw = new System.Windows.Forms.CheckBox();
            this.blobMax = new System.Windows.Forms.NumericUpDown();
            this.binMax = new System.Windows.Forms.NumericUpDown();
            this.blobMin = new System.Windows.Forms.NumericUpDown();
            this.closeNum = new System.Windows.Forms.NumericUpDown();
            this.binMin = new System.Windows.Forms.NumericUpDown();
            this.roiH = new System.Windows.Forms.NumericUpDown();
            this.roiR = new System.Windows.Forms.NumericUpDown();
            this.roiW = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.roiY = new System.Windows.Forms.NumericUpDown();
            this.labelBinMax = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelCloseNum = new System.Windows.Forms.Label();
            this.roiX = new System.Windows.Forms.NumericUpDown();
            this.labelBinMin = new System.Windows.Forms.Label();
            this.labelRoiR = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelRoiW = new System.Windows.Forms.Label();
            this.labelRoiY = new System.Windows.Forms.Label();
            this.labelBlob = new System.Windows.Forms.Label();
            this.labelRoiX = new System.Windows.Forms.Label();
            this.labelBinValue = new System.Windows.Forms.Label();
            this.labelROI = new System.Windows.Forms.Label();
            this.buttonAddRoi = new System.Windows.Forms.Button();
            this.buttonDoProcess = new System.Windows.Forms.Button();
            this.buttonImageLoad = new System.Windows.Forms.Button();
            this.buttonSaveResult = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelResultCount = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelCommand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blobMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.binMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blobMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.binMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roiH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roiR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roiW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roiY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.roiX)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelCanvas
            // 
            this.panelCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCanvas.Location = new System.Drawing.Point(280, 2);
            this.panelCanvas.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelCanvas.Name = "panelCanvas";
            this.tableLayoutPanel1.SetRowSpan(this.panelCanvas, 2);
            this.panelCanvas.Size = new System.Drawing.Size(460, 812);
            this.panelCanvas.TabIndex = 1;
            // 
            // panelResult
            // 
            this.panelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelResult.Location = new System.Drawing.Point(746, 2);
            this.panelResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelResult.Name = "panelResult";
            this.tableLayoutPanel1.SetRowSpan(this.panelResult, 2);
            this.panelResult.Size = new System.Drawing.Size(461, 812);
            this.panelResult.TabIndex = 2;
            // 
            // panelCommand
            // 
            this.panelCommand.Controls.Add(this.fastDraw);
            this.panelCommand.Controls.Add(this.blobMax);
            this.panelCommand.Controls.Add(this.binMax);
            this.panelCommand.Controls.Add(this.blobMin);
            this.panelCommand.Controls.Add(this.closeNum);
            this.panelCommand.Controls.Add(this.binMin);
            this.panelCommand.Controls.Add(this.roiH);
            this.panelCommand.Controls.Add(this.roiR);
            this.panelCommand.Controls.Add(this.roiW);
            this.panelCommand.Controls.Add(this.label4);
            this.panelCommand.Controls.Add(this.roiY);
            this.panelCommand.Controls.Add(this.labelBinMax);
            this.panelCommand.Controls.Add(this.label3);
            this.panelCommand.Controls.Add(this.labelCloseNum);
            this.panelCommand.Controls.Add(this.roiX);
            this.panelCommand.Controls.Add(this.labelBinMin);
            this.panelCommand.Controls.Add(this.labelRoiR);
            this.panelCommand.Controls.Add(this.label2);
            this.panelCommand.Controls.Add(this.labelRoiW);
            this.panelCommand.Controls.Add(this.labelRoiY);
            this.panelCommand.Controls.Add(this.labelBlob);
            this.panelCommand.Controls.Add(this.labelRoiX);
            this.panelCommand.Controls.Add(this.labelBinValue);
            this.panelCommand.Controls.Add(this.labelROI);
            this.panelCommand.Controls.Add(this.buttonAddRoi);
            this.panelCommand.Controls.Add(this.buttonDoProcess);
            this.panelCommand.Controls.Add(this.buttonImageLoad);
            this.panelCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCommand.Location = new System.Drawing.Point(3, 2);
            this.panelCommand.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelCommand.Name = "panelCommand";
            this.panelCommand.Size = new System.Drawing.Size(271, 404);
            this.panelCommand.TabIndex = 2;
            // 
            // fastDraw
            // 
            this.fastDraw.AutoSize = true;
            this.fastDraw.Checked = true;
            this.fastDraw.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fastDraw.Location = new System.Drawing.Point(191, 19);
            this.fastDraw.Name = "fastDraw";
            this.fastDraw.Size = new System.Drawing.Size(77, 16);
            this.fastDraw.TabIndex = 6;
            this.fastDraw.Text = "FastDraw";
            this.fastDraw.UseVisualStyleBackColor = true;
            this.fastDraw.CheckedChanged += new System.EventHandler(this.fastDraw_CheckedChanged);
            // 
            // blobMax
            // 
            this.blobMax.Location = new System.Drawing.Point(199, 333);
            this.blobMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.blobMax.Name = "blobMax";
            this.blobMax.Size = new System.Drawing.Size(60, 21);
            this.blobMax.TabIndex = 5;
            // 
            // binMax
            // 
            this.binMax.Location = new System.Drawing.Point(199, 238);
            this.binMax.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.binMax.Name = "binMax";
            this.binMax.Size = new System.Drawing.Size(60, 21);
            this.binMax.TabIndex = 5;
            this.binMax.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // blobMin
            // 
            this.blobMin.Location = new System.Drawing.Point(94, 333);
            this.blobMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.blobMin.Name = "blobMin";
            this.blobMin.Size = new System.Drawing.Size(60, 21);
            this.blobMin.TabIndex = 4;
            this.blobMin.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // closeNum
            // 
            this.closeNum.Location = new System.Drawing.Point(94, 265);
            this.closeNum.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.closeNum.Name = "closeNum";
            this.closeNum.Size = new System.Drawing.Size(60, 21);
            this.closeNum.TabIndex = 4;
            this.closeNum.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // binMin
            // 
            this.binMin.Location = new System.Drawing.Point(94, 238);
            this.binMin.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.binMin.Name = "binMin";
            this.binMin.Size = new System.Drawing.Size(60, 21);
            this.binMin.TabIndex = 4;
            this.binMin.Value = new decimal(new int[] {
            140,
            0,
            0,
            0});
            // 
            // roiH
            // 
            this.roiH.Location = new System.Drawing.Point(199, 151);
            this.roiH.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.roiH.Name = "roiH";
            this.roiH.Size = new System.Drawing.Size(60, 21);
            this.roiH.TabIndex = 3;
            this.roiH.Value = new decimal(new int[] {
            13080,
            0,
            0,
            0});
            this.roiH.ValueChanged += new System.EventHandler(this.roi_ValueChanged);
            // 
            // roiR
            // 
            this.roiR.Location = new System.Drawing.Point(94, 178);
            this.roiR.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.roiR.Name = "roiR";
            this.roiR.Size = new System.Drawing.Size(60, 21);
            this.roiR.TabIndex = 2;
            this.roiR.ValueChanged += new System.EventHandler(this.roi_ValueChanged);
            // 
            // roiW
            // 
            this.roiW.Location = new System.Drawing.Point(94, 151);
            this.roiW.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.roiW.Name = "roiW";
            this.roiW.Size = new System.Drawing.Size(60, 21);
            this.roiW.TabIndex = 2;
            this.roiW.Value = new decimal(new int[] {
            7248,
            0,
            0,
            0});
            this.roiW.ValueChanged += new System.EventHandler(this.roi_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(166, 337);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "Max";
            // 
            // roiY
            // 
            this.roiY.Location = new System.Drawing.Point(199, 123);
            this.roiY.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.roiY.Name = "roiY";
            this.roiY.Size = new System.Drawing.Size(60, 21);
            this.roiY.TabIndex = 1;
            this.roiY.Value = new decimal(new int[] {
            4352,
            0,
            0,
            0});
            this.roiY.ValueChanged += new System.EventHandler(this.roi_ValueChanged);
            // 
            // labelBinMax
            // 
            this.labelBinMax.AutoSize = true;
            this.labelBinMax.Location = new System.Drawing.Point(166, 242);
            this.labelBinMax.Name = "labelBinMax";
            this.labelBinMax.Size = new System.Drawing.Size(30, 12);
            this.labelBinMax.TabIndex = 1;
            this.labelBinMax.Text = "Max";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(65, 337);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "Min";
            // 
            // labelCloseNum
            // 
            this.labelCloseNum.AutoSize = true;
            this.labelCloseNum.Location = new System.Drawing.Point(53, 269);
            this.labelCloseNum.Name = "labelCloseNum";
            this.labelCloseNum.Size = new System.Drawing.Size(38, 12);
            this.labelCloseNum.TabIndex = 1;
            this.labelCloseNum.Text = "Close";
            // 
            // roiX
            // 
            this.roiX.Location = new System.Drawing.Point(94, 123);
            this.roiX.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.roiX.Name = "roiX";
            this.roiX.Size = new System.Drawing.Size(60, 21);
            this.roiX.TabIndex = 0;
            this.roiX.Value = new decimal(new int[] {
            3264,
            0,
            0,
            0});
            this.roiX.ValueChanged += new System.EventHandler(this.roi_ValueChanged);
            // 
            // labelBinMin
            // 
            this.labelBinMin.AutoSize = true;
            this.labelBinMin.Location = new System.Drawing.Point(65, 242);
            this.labelBinMin.Name = "labelBinMin";
            this.labelBinMin.Size = new System.Drawing.Size(26, 12);
            this.labelBinMin.TabIndex = 1;
            this.labelBinMin.Text = "Min";
            // 
            // labelRoiR
            // 
            this.labelRoiR.AutoSize = true;
            this.labelRoiR.Location = new System.Drawing.Point(76, 182);
            this.labelRoiR.Name = "labelRoiR";
            this.labelRoiR.Size = new System.Drawing.Size(13, 12);
            this.labelRoiR.TabIndex = 1;
            this.labelRoiR.Text = "R";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(183, 155);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "H";
            // 
            // labelRoiW
            // 
            this.labelRoiW.AutoSize = true;
            this.labelRoiW.Location = new System.Drawing.Point(76, 155);
            this.labelRoiW.Name = "labelRoiW";
            this.labelRoiW.Size = new System.Drawing.Size(15, 12);
            this.labelRoiW.TabIndex = 1;
            this.labelRoiW.Text = "W";
            // 
            // labelRoiY
            // 
            this.labelRoiY.AutoSize = true;
            this.labelRoiY.Location = new System.Drawing.Point(183, 127);
            this.labelRoiY.Name = "labelRoiY";
            this.labelRoiY.Size = new System.Drawing.Size(13, 12);
            this.labelRoiY.TabIndex = 1;
            this.labelRoiY.Text = "Y";
            // 
            // labelBlob
            // 
            this.labelBlob.AutoSize = true;
            this.labelBlob.Location = new System.Drawing.Point(19, 314);
            this.labelBlob.Name = "labelBlob";
            this.labelBlob.Size = new System.Drawing.Size(30, 12);
            this.labelBlob.TabIndex = 1;
            this.labelBlob.Text = "Blob";
            // 
            // labelRoiX
            // 
            this.labelRoiX.AutoSize = true;
            this.labelRoiX.Location = new System.Drawing.Point(78, 127);
            this.labelRoiX.Name = "labelRoiX";
            this.labelRoiX.Size = new System.Drawing.Size(13, 12);
            this.labelRoiX.TabIndex = 1;
            this.labelRoiX.Text = "X";
            // 
            // labelBinValue
            // 
            this.labelBinValue.AutoSize = true;
            this.labelBinValue.Location = new System.Drawing.Point(19, 219);
            this.labelBinValue.Name = "labelBinValue";
            this.labelBinValue.Size = new System.Drawing.Size(55, 12);
            this.labelBinValue.TabIndex = 1;
            this.labelBinValue.Text = "BinValue";
            // 
            // labelROI
            // 
            this.labelROI.AutoSize = true;
            this.labelROI.Location = new System.Drawing.Point(19, 108);
            this.labelROI.Name = "labelROI";
            this.labelROI.Size = new System.Drawing.Size(25, 12);
            this.labelROI.TabIndex = 1;
            this.labelROI.Text = "ROI";
            // 
            // buttonAddRoi
            // 
            this.buttonAddRoi.Location = new System.Drawing.Point(10, 123);
            this.buttonAddRoi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonAddRoi.Name = "buttonAddRoi";
            this.buttonAddRoi.Size = new System.Drawing.Size(60, 49);
            this.buttonAddRoi.TabIndex = 0;
            this.buttonAddRoi.Text = "Add";
            this.buttonAddRoi.UseVisualStyleBackColor = true;
            this.buttonAddRoi.Click += new System.EventHandler(this.buttonAddRoi_Click);
            // 
            // buttonDoProcess
            // 
            this.buttonDoProcess.Location = new System.Drawing.Point(10, 62);
            this.buttonDoProcess.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonDoProcess.Name = "buttonDoProcess";
            this.buttonDoProcess.Size = new System.Drawing.Size(101, 34);
            this.buttonDoProcess.TabIndex = 0;
            this.buttonDoProcess.Text = "Do Process";
            this.buttonDoProcess.UseVisualStyleBackColor = true;
            this.buttonDoProcess.Click += new System.EventHandler(this.buttonDoProcess_Click);
            // 
            // buttonImageLoad
            // 
            this.buttonImageLoad.Location = new System.Drawing.Point(10, 9);
            this.buttonImageLoad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonImageLoad.Name = "buttonImageLoad";
            this.buttonImageLoad.Size = new System.Drawing.Size(101, 34);
            this.buttonImageLoad.TabIndex = 0;
            this.buttonImageLoad.Text = "Load Image";
            this.buttonImageLoad.UseVisualStyleBackColor = true;
            this.buttonImageLoad.Click += new System.EventHandler(this.buttonImageLoad_Click);
            // 
            // buttonSaveResult
            // 
            this.buttonSaveResult.Location = new System.Drawing.Point(10, 11);
            this.buttonSaveResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonSaveResult.Name = "buttonSaveResult";
            this.buttonSaveResult.Size = new System.Drawing.Size(101, 34);
            this.buttonSaveResult.TabIndex = 0;
            this.buttonSaveResult.Text = "Save Result";
            this.buttonSaveResult.UseVisualStyleBackColor = true;
            this.buttonSaveResult.Click += new System.EventHandler(this.buttonSaveResult_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 277F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panelResult, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelCanvas, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelCommand, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1210, 816);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelResultCount);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.buttonSaveResult);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 411);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(271, 402);
            this.panel1.TabIndex = 3;
            // 
            // labelResultCount
            // 
            this.labelResultCount.AutoSize = true;
            this.labelResultCount.Location = new System.Drawing.Point(221, 33);
            this.labelResultCount.Name = "labelResultCount";
            this.labelResultCount.Size = new System.Drawing.Size(38, 12);
            this.labelResultCount.TabIndex = 2;
            this.labelResultCount.Text = "label1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(10, 50);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(249, 343);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // AlgorithmSimulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1210, 816);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AlgorithmSimulatorForm";
            this.Text = "SheetFIndTest";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AlgorithmSimulatorForm_FormClosing);
            this.Load += new System.EventHandler(this.AlgorithmSimulatorForm_Load);
            this.SizeChanged += new System.EventHandler(this.AlgorithmSimulatorForm_SizeChanged);
            this.panelCommand.ResumeLayout(false);
            this.panelCommand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.blobMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.binMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blobMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.binMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roiH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roiR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roiW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roiY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.roiX)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelCanvas;
        private System.Windows.Forms.Panel panelResult;
        private System.Windows.Forms.Button buttonImageLoad;
        private System.Windows.Forms.Panel panelCommand;
        private System.Windows.Forms.Button buttonSaveResult;
        private System.Windows.Forms.Button buttonDoProcess;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown roiH;
        private System.Windows.Forms.NumericUpDown roiW;
        private System.Windows.Forms.NumericUpDown roiY;
        private System.Windows.Forms.NumericUpDown roiX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelRoiW;
        private System.Windows.Forms.Label labelRoiY;
        private System.Windows.Forms.Label labelRoiX;
        private System.Windows.Forms.Label labelROI;
        private System.Windows.Forms.NumericUpDown binMax;
        private System.Windows.Forms.NumericUpDown binMin;
        private System.Windows.Forms.Label labelBinMax;
        private System.Windows.Forms.Label labelBinMin;
        private System.Windows.Forms.Label labelBinValue;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelResultCount;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.NumericUpDown blobMax;
        private System.Windows.Forms.NumericUpDown blobMin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelBlob;
        private System.Windows.Forms.NumericUpDown closeNum;
        private System.Windows.Forms.Label labelCloseNum;
        private System.Windows.Forms.Button buttonAddRoi;
        private System.Windows.Forms.NumericUpDown roiR;
        private System.Windows.Forms.Label labelRoiR;
        private System.Windows.Forms.CheckBox fastDraw;
    }
}