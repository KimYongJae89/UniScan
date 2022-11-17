namespace UniScanM.StillImage.Test
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridViewInsp = new System.Windows.Forms.DataGridView();
            this.columnSID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnGID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnBlot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnMargine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTeach = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewResult = new System.Windows.Forms.DataGridView();
            this.columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonClipSave = new System.Windows.Forms.Button();
            this.checkBoxFullScale = new System.Windows.Forms.CheckBox();
            this.buttonImageLoad = new System.Windows.Forms.Button();
            this.buttonTeaching = new System.Windows.Forms.Button();
            this.buttonSheetFind = new System.Windows.Forms.Button();
            this.buttonBrightness = new System.Windows.Forms.Button();
            this.buttonGetRoi = new System.Windows.Forms.Button();
            this.buttonInspect = new System.Windows.Forms.Button();
            this.buttonFigure = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInsp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeach)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(447, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(317, 816);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridViewInsp);
            this.panel2.Controls.Add(this.dataGridViewTeach);
            this.panel2.Controls.Add(this.dataGridViewResult);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(447, 816);
            this.panel2.TabIndex = 2;
            // 
            // dataGridViewInsp
            // 
            this.dataGridViewInsp.AllowUserToAddRows = false;
            this.dataGridViewInsp.AllowUserToDeleteRows = false;
            this.dataGridViewInsp.AllowUserToOrderColumns = true;
            this.dataGridViewInsp.AllowUserToResizeRows = false;
            this.dataGridViewInsp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInsp.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnSID,
            this.columnGID,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn1,
            this.columnBlot,
            this.columnMargine});
            this.dataGridViewInsp.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewInsp.Location = new System.Drawing.Point(0, 629);
            this.dataGridViewInsp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewInsp.MultiSelect = false;
            this.dataGridViewInsp.Name = "dataGridViewInsp";
            this.dataGridViewInsp.ReadOnly = true;
            this.dataGridViewInsp.RowHeadersVisible = false;
            this.dataGridViewInsp.RowTemplate.Height = 27;
            this.dataGridViewInsp.Size = new System.Drawing.Size(447, 221);
            this.dataGridViewInsp.TabIndex = 3;
            // 
            // columnSID
            // 
            this.columnSID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnSID.HeaderText = "SID";
            this.columnSID.MinimumWidth = 30;
            this.columnSID.Name = "columnSID";
            this.columnSID.ReadOnly = true;
            this.columnSID.Width = 49;
            // 
            // columnGID
            // 
            this.columnGID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnGID.HeaderText = "GID";
            this.columnGID.MinimumWidth = 30;
            this.columnGID.Name = "columnGID";
            this.columnGID.ReadOnly = true;
            this.columnGID.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Rect";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.HeaderText = "Area";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 30;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 56;
            // 
            // columnBlot
            // 
            this.columnBlot.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnBlot.HeaderText = "Blot";
            this.columnBlot.MinimumWidth = 30;
            this.columnBlot.Name = "columnBlot";
            this.columnBlot.ReadOnly = true;
            this.columnBlot.Width = 51;
            // 
            // columnMargine
            // 
            this.columnMargine.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnMargine.HeaderText = "Margine";
            this.columnMargine.MinimumWidth = 30;
            this.columnMargine.Name = "columnMargine";
            this.columnMargine.ReadOnly = true;
            this.columnMargine.Width = 76;
            // 
            // dataGridViewTeach
            // 
            this.dataGridViewTeach.AllowUserToAddRows = false;
            this.dataGridViewTeach.AllowUserToDeleteRows = false;
            this.dataGridViewTeach.AllowUserToOrderColumns = true;
            this.dataGridViewTeach.AllowUserToResizeRows = false;
            this.dataGridViewTeach.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTeach.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            this.dataGridViewTeach.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewTeach.Location = new System.Drawing.Point(0, 408);
            this.dataGridViewTeach.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewTeach.MultiSelect = false;
            this.dataGridViewTeach.Name = "dataGridViewTeach";
            this.dataGridViewTeach.ReadOnly = true;
            this.dataGridViewTeach.RowHeadersVisible = false;
            this.dataGridViewTeach.RowTemplate.Height = 27;
            this.dataGridViewTeach.Size = new System.Drawing.Size(447, 221);
            this.dataGridViewTeach.TabIndex = 4;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn3.HeaderText = "SID";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 30;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 49;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn4.HeaderText = "GID";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 30;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 50;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.HeaderText = "Rect";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn6.HeaderText = "Area";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 30;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 56;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn7.HeaderText = "Blot";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 30;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 51;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn8.HeaderText = "Margine";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 30;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 76;
            // 
            // dataGridViewResult
            // 
            this.dataGridViewResult.AllowUserToAddRows = false;
            this.dataGridViewResult.AllowUserToDeleteRows = false;
            this.dataGridViewResult.AllowUserToOrderColumns = true;
            this.dataGridViewResult.AllowUserToResizeRows = false;
            this.dataGridViewResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnName,
            this.columnPosition,
            this.columnValue});
            this.dataGridViewResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewResult.Location = new System.Drawing.Point(0, 186);
            this.dataGridViewResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridViewResult.MultiSelect = false;
            this.dataGridViewResult.Name = "dataGridViewResult";
            this.dataGridViewResult.ReadOnly = true;
            this.dataGridViewResult.RowHeadersVisible = false;
            this.dataGridViewResult.RowTemplate.Height = 27;
            this.dataGridViewResult.Size = new System.Drawing.Size(447, 222);
            this.dataGridViewResult.TabIndex = 0;
            // 
            // columnName
            // 
            this.columnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnName.HeaderText = "Name";
            this.columnName.MinimumWidth = 45;
            this.columnName.Name = "columnName";
            this.columnName.ReadOnly = true;
            this.columnName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnName.Width = 45;
            // 
            // columnPosition
            // 
            this.columnPosition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnPosition.HeaderText = "Position";
            this.columnPosition.MinimumWidth = 45;
            this.columnPosition.Name = "columnPosition";
            this.columnPosition.ReadOnly = true;
            this.columnPosition.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // columnValue
            // 
            this.columnValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnValue.HeaderText = "Value";
            this.columnValue.MinimumWidth = 45;
            this.columnValue.Name = "columnValue";
            this.columnValue.ReadOnly = true;
            this.columnValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.columnValue.Width = 45;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.buttonFigure);
            this.panel3.Controls.Add(this.buttonClipSave);
            this.panel3.Controls.Add(this.checkBoxFullScale);
            this.panel3.Controls.Add(this.buttonImageLoad);
            this.panel3.Controls.Add(this.buttonTeaching);
            this.panel3.Controls.Add(this.buttonSheetFind);
            this.panel3.Controls.Add(this.buttonBrightness);
            this.panel3.Controls.Add(this.buttonGetRoi);
            this.panel3.Controls.Add(this.buttonInspect);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(447, 186);
            this.panel3.TabIndex = 2;
            // 
            // buttonClipSave
            // 
            this.buttonClipSave.Location = new System.Drawing.Point(264, 48);
            this.buttonClipSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonClipSave.Name = "buttonClipSave";
            this.buttonClipSave.Size = new System.Drawing.Size(53, 34);
            this.buttonClipSave.TabIndex = 7;
            this.buttonClipSave.Text = "Save";
            this.buttonClipSave.UseVisualStyleBackColor = true;
            this.buttonClipSave.Click += new System.EventHandler(this.buttonClipSave_Click);
            // 
            // checkBoxFullScale
            // 
            this.checkBoxFullScale.AutoSize = true;
            this.checkBoxFullScale.Location = new System.Drawing.Point(9, 162);
            this.checkBoxFullScale.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxFullScale.Name = "checkBoxFullScale";
            this.checkBoxFullScale.Size = new System.Drawing.Size(80, 16);
            this.checkBoxFullScale.TabIndex = 6;
            this.checkBoxFullScale.Text = "Full Scale";
            this.checkBoxFullScale.UseVisualStyleBackColor = true;
            this.checkBoxFullScale.CheckedChanged += new System.EventHandler(this.checkBoxFullScale_CheckedChanged);
            // 
            // buttonImageLoad
            // 
            this.buttonImageLoad.Location = new System.Drawing.Point(10, 10);
            this.buttonImageLoad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonImageLoad.Name = "buttonImageLoad";
            this.buttonImageLoad.Size = new System.Drawing.Size(307, 34);
            this.buttonImageLoad.TabIndex = 0;
            this.buttonImageLoad.Text = "Loaddddd";
            this.buttonImageLoad.UseVisualStyleBackColor = true;
            this.buttonImageLoad.Click += new System.EventHandler(this.buttonImageLoad_Click);
            // 
            // buttonTeaching
            // 
            this.buttonTeaching.Location = new System.Drawing.Point(166, 86);
            this.buttonTeaching.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonTeaching.Name = "buttonTeaching";
            this.buttonTeaching.Size = new System.Drawing.Size(151, 34);
            this.buttonTeaching.TabIndex = 5;
            this.buttonTeaching.Text = "Teaching";
            this.buttonTeaching.UseVisualStyleBackColor = true;
            this.buttonTeaching.Click += new System.EventHandler(this.buttonTeaching_Click);
            // 
            // buttonSheetFind
            // 
            this.buttonSheetFind.Location = new System.Drawing.Point(10, 48);
            this.buttonSheetFind.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonSheetFind.Name = "buttonSheetFind";
            this.buttonSheetFind.Size = new System.Drawing.Size(121, 34);
            this.buttonSheetFind.TabIndex = 1;
            this.buttonSheetFind.Text = "Find Sheet";
            this.buttonSheetFind.UseVisualStyleBackColor = true;
            this.buttonSheetFind.Click += new System.EventHandler(this.buttonSheetFind_Click);
            // 
            // buttonBrightness
            // 
            this.buttonBrightness.Location = new System.Drawing.Point(9, 86);
            this.buttonBrightness.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonBrightness.Name = "buttonBrightness";
            this.buttonBrightness.Size = new System.Drawing.Size(151, 34);
            this.buttonBrightness.TabIndex = 4;
            this.buttonBrightness.Text = "Brightness";
            this.buttonBrightness.UseVisualStyleBackColor = true;
            this.buttonBrightness.Click += new System.EventHandler(this.buttonBrightness_Click);
            // 
            // buttonGetRoi
            // 
            this.buttonGetRoi.Location = new System.Drawing.Point(137, 48);
            this.buttonGetRoi.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonGetRoi.Name = "buttonGetRoi";
            this.buttonGetRoi.Size = new System.Drawing.Size(121, 34);
            this.buttonGetRoi.TabIndex = 2;
            this.buttonGetRoi.Text = "Get ROI";
            this.buttonGetRoi.UseVisualStyleBackColor = true;
            this.buttonGetRoi.Click += new System.EventHandler(this.buttonGetRoi_Click);
            // 
            // buttonInspect
            // 
            this.buttonInspect.Location = new System.Drawing.Point(10, 124);
            this.buttonInspect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonInspect.Name = "buttonInspect";
            this.buttonInspect.Size = new System.Drawing.Size(307, 34);
            this.buttonInspect.TabIndex = 3;
            this.buttonInspect.Text = "Inspect";
            this.buttonInspect.UseVisualStyleBackColor = true;
            this.buttonInspect.Click += new System.EventHandler(this.buttonInspect_Click);
            // 
            // buttonFigure
            // 
            this.buttonFigure.Location = new System.Drawing.Point(362, 11);
            this.buttonFigure.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonFigure.Name = "buttonFigure";
            this.buttonFigure.Size = new System.Drawing.Size(79, 34);
            this.buttonFigure.TabIndex = 8;
            this.buttonFigure.Text = "Figure";
            this.buttonFigure.UseVisualStyleBackColor = true;
            // 
            // SheetFIndTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 816);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SheetFIndTestForm";
            this.Text = "SheetFIndTest";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SheetFIndTestForm_FormClosing);
            this.Load += new System.EventHandler(this.SheetFIndTestForm_Load);
            this.SizeChanged += new System.EventHandler(this.SheetFIndTestForm_SizeChanged);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInsp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTeach)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonImageLoad;
        private System.Windows.Forms.Button buttonSheetFind;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonTeaching;
        private System.Windows.Forms.Button buttonBrightness;
        private System.Windows.Forms.Button buttonInspect;
        private System.Windows.Forms.DataGridView dataGridViewResult;
        private System.Windows.Forms.CheckBox checkBoxFullScale;
        private System.Windows.Forms.Button buttonGetRoi;
        private System.Windows.Forms.Button buttonClipSave;
        private System.Windows.Forms.DataGridView dataGridViewInsp;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnGID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnBlot;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnMargine;
        private System.Windows.Forms.DataGridView dataGridViewTeach;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.Button buttonFigure;
    }
}