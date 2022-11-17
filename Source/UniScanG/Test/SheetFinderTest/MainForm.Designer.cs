namespace SheetFinderTest
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel3 = new System.Windows.Forms.Panel();
            this.sheetFinderSelector = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.debugPath = new System.Windows.Forms.TextBox();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonOpenDebugPath = new System.Windows.Forms.Button();
            this.buttonFindDebugPath = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.labelDebugPath = new System.Windows.Forms.Label();
            this.labelFrameHeight = new System.Windows.Forms.Label();
            this.frameHeight = new System.Windows.Forms.NumericUpDown();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.columnIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnBasePos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnParents = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelPattern = new System.Windows.Forms.Panel();
            this.panelFrameNew = new System.Windows.Forms.Panel();
            this.panelFrameOld = new System.Windows.Forms.Panel();
            this.pixelRes = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pixelRes)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelPattern, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelFrameNew, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelFrameOld, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1057, 548);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(741, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView);
            this.tableLayoutPanel1.SetRowSpan(this.splitContainer1, 2);
            this.splitContainer1.Size = new System.Drawing.Size(313, 542);
            this.splitContainer1.SplitterDistance = 348;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.propertyGrid1);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(313, 348);
            this.panel1.TabIndex = 0;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 134);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(313, 214);
            this.propertyGrid1.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.AutoSize = true;
            this.panel3.Controls.Add(this.sheetFinderSelector);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 107);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(313, 27);
            this.panel3.TabIndex = 8;
            // 
            // sheetFinderSelector
            // 
            this.sheetFinderSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sheetFinderSelector.FormattingEnabled = true;
            this.sheetFinderSelector.Location = new System.Drawing.Point(6, 4);
            this.sheetFinderSelector.Name = "sheetFinderSelector";
            this.sheetFinderSelector.Size = new System.Drawing.Size(298, 20);
            this.sheetFinderSelector.TabIndex = 6;
            this.sheetFinderSelector.SelectedIndexChanged += new System.EventHandler(this.sheetFinderSelector_SelectedIndexChanged);
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.pixelRes);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.debugPath);
            this.panel2.Controls.Add(this.buttonLoad);
            this.panel2.Controls.Add(this.buttonOpenDebugPath);
            this.panel2.Controls.Add(this.buttonFindDebugPath);
            this.panel2.Controls.Add(this.buttonStart);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.labelDebugPath);
            this.panel2.Controls.Add(this.labelFrameHeight);
            this.panel2.Controls.Add(this.frameHeight);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(313, 107);
            this.panel2.TabIndex = 7;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(5, 86);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(91, 16);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Save Image";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // debugPath
            // 
            this.debugPath.Location = new System.Drawing.Point(84, 56);
            this.debugPath.Name = "debugPath";
            this.debugPath.Size = new System.Drawing.Size(135, 21);
            this.debugPath.TabIndex = 6;
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(3, 3);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 48);
            this.buttonLoad.TabIndex = 0;
            this.buttonLoad.Text = "LoadImg";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonOpenDebugPath
            // 
            this.buttonOpenDebugPath.Location = new System.Drawing.Point(258, 54);
            this.buttonOpenDebugPath.Name = "buttonOpenDebugPath";
            this.buttonOpenDebugPath.Size = new System.Drawing.Size(46, 23);
            this.buttonOpenDebugPath.TabIndex = 0;
            this.buttonOpenDebugPath.Text = "Open";
            this.buttonOpenDebugPath.UseVisualStyleBackColor = true;
            this.buttonOpenDebugPath.Click += new System.EventHandler(this.buttonOpenDebugPath_Click);
            // 
            // buttonFindDebugPath
            // 
            this.buttonFindDebugPath.Location = new System.Drawing.Point(225, 54);
            this.buttonFindDebugPath.Name = "buttonFindDebugPath";
            this.buttonFindDebugPath.Size = new System.Drawing.Size(25, 23);
            this.buttonFindDebugPath.TabIndex = 0;
            this.buttonFindDebugPath.Text = "...";
            this.buttonFindDebugPath.UseVisualStyleBackColor = true;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(84, 3);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 48);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start/Stop";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // labelDebugPath
            // 
            this.labelDebugPath.AutoSize = true;
            this.labelDebugPath.Location = new System.Drawing.Point(3, 61);
            this.labelDebugPath.Name = "labelDebugPath";
            this.labelDebugPath.Size = new System.Drawing.Size(70, 12);
            this.labelDebugPath.TabIndex = 5;
            this.labelDebugPath.Text = "Debug Path";
            // 
            // labelFrameHeight
            // 
            this.labelFrameHeight.AutoSize = true;
            this.labelFrameHeight.Location = new System.Drawing.Point(175, 6);
            this.labelFrameHeight.Name = "labelFrameHeight";
            this.labelFrameHeight.Size = new System.Drawing.Size(80, 12);
            this.labelFrameHeight.TabIndex = 5;
            this.labelFrameHeight.Text = "Frame Height";
            // 
            // frameHeight
            // 
            this.frameHeight.Location = new System.Drawing.Point(229, 19);
            this.frameHeight.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.frameHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.frameHeight.Name = "frameHeight";
            this.frameHeight.Size = new System.Drawing.Size(75, 21);
            this.frameHeight.TabIndex = 4;
            this.frameHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnIndex,
            this.columnHeight,
            this.columnBasePos,
            this.columnParents});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.MultiSelect = false;
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(313, 190);
            this.dataGridView.TabIndex = 6;
            // 
            // columnIndex
            // 
            this.columnIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnIndex.HeaderText = "Index";
            this.columnIndex.Name = "columnIndex";
            this.columnIndex.ReadOnly = true;
            this.columnIndex.Width = 61;
            // 
            // columnHeight
            // 
            this.columnHeight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnHeight.HeaderText = "Height";
            this.columnHeight.Name = "columnHeight";
            this.columnHeight.ReadOnly = true;
            this.columnHeight.Width = 65;
            // 
            // columnBasePos
            // 
            this.columnBasePos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnBasePos.HeaderText = "BasePosition";
            this.columnBasePos.Name = "columnBasePos";
            this.columnBasePos.ReadOnly = true;
            this.columnBasePos.Width = 104;
            // 
            // columnParents
            // 
            this.columnParents.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnParents.HeaderText = "Parents";
            this.columnParents.Name = "columnParents";
            this.columnParents.ReadOnly = true;
            this.columnParents.Width = 73;
            // 
            // panelPattern
            // 
            this.panelPattern.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPattern.Location = new System.Drawing.Point(372, 3);
            this.panelPattern.Name = "panelPattern";
            this.tableLayoutPanel1.SetRowSpan(this.panelPattern, 2);
            this.panelPattern.Size = new System.Drawing.Size(363, 542);
            this.panelPattern.TabIndex = 7;
            // 
            // panelFrameNew
            // 
            this.panelFrameNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFrameNew.Location = new System.Drawing.Point(3, 277);
            this.panelFrameNew.Name = "panelFrameNew";
            this.panelFrameNew.Size = new System.Drawing.Size(363, 268);
            this.panelFrameNew.TabIndex = 1;
            // 
            // panelFrameOld
            // 
            this.panelFrameOld.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFrameOld.Location = new System.Drawing.Point(3, 3);
            this.panelFrameOld.Name = "panelFrameOld";
            this.panelFrameOld.Size = new System.Drawing.Size(363, 268);
            this.panelFrameOld.TabIndex = 1;
            // 
            // pixelRes
            // 
            this.pixelRes.Location = new System.Drawing.Point(195, 83);
            this.pixelRes.Name = "pixelRes";
            this.pixelRes.Size = new System.Drawing.Size(60, 21);
            this.pixelRes.TabIndex = 9;
            this.pixelRes.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(261, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "um/px";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(130, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Pixel Res";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 548);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pixelRes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Panel panelFrameNew;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Panel panelPattern;
        private System.Windows.Forms.Label labelFrameHeight;
        private System.Windows.Forms.NumericUpDown frameHeight;
        private System.Windows.Forms.Panel panelFrameOld;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnBasePos;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnParents;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox debugPath;
        private System.Windows.Forms.Button buttonFindDebugPath;
        private System.Windows.Forms.Label labelDebugPath;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button buttonOpenDebugPath;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox sheetFinderSelector;
        private System.Windows.Forms.NumericUpDown pixelRes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

