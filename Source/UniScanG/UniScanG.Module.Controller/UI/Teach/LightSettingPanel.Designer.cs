namespace UniScanG.Module.Controller.UI.Teach
{
    partial class LightSettingPanel
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
            this.numericLightTopLeft = new System.Windows.Forms.NumericUpDown();
            this.trackBarLightTopLeft = new System.Windows.Forms.TrackBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelLightTop = new System.Windows.Forms.Label();
            this.labelLightBottom = new System.Windows.Forms.Label();
            this.trackBarLightTopMiddle = new System.Windows.Forms.TrackBar();
            this.trackBarLightTopRight = new System.Windows.Forms.TrackBar();
            this.trackBarLightBottom = new System.Windows.Forms.TrackBar();
            this.labelLightTopLeft = new System.Windows.Forms.Label();
            this.labelLightTopMiddle = new System.Windows.Forms.Label();
            this.labelLightTopRight = new System.Windows.Forms.Label();
            this.lightTopLeft = new System.Windows.Forms.Label();
            this.lightTopMiddle = new System.Windows.Forms.Label();
            this.lightTopRight = new System.Windows.Forms.Label();
            this.lightBottom = new System.Windows.Forms.Label();
            this.numericLightTopMiddle = new System.Windows.Forms.NumericUpDown();
            this.numericLightTopRight = new System.Windows.Forms.NumericUpDown();
            this.numericLightBottom = new System.Windows.Forms.NumericUpDown();
            this.buttonOn = new System.Windows.Forms.Button();
            this.buttonOnAuto = new System.Windows.Forms.Button();
            this.buttonOff = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.columnSpd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLightTopL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLightTopM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLightTopR = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLightBottom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.numericLightTopLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightTopLeft)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightTopMiddle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightTopRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLightTopMiddle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLightTopRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLightBottom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // numericLightTopLeft
            // 
            this.numericLightTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericLightTopLeft.Location = new System.Drawing.Point(485, 6);
            this.numericLightTopLeft.Margin = new System.Windows.Forms.Padding(5);
            this.numericLightTopLeft.Name = "numericLightTopLeft";
            this.numericLightTopLeft.Size = new System.Drawing.Size(50, 29);
            this.numericLightTopLeft.TabIndex = 0;
            this.numericLightTopLeft.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // trackBarLightTopLeft
            // 
            this.trackBarLightTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarLightTopLeft.Location = new System.Drawing.Point(176, 6);
            this.trackBarLightTopLeft.Margin = new System.Windows.Forms.Padding(5);
            this.trackBarLightTopLeft.Name = "trackBarLightTopLeft";
            this.trackBarLightTopLeft.Size = new System.Drawing.Size(298, 30);
            this.trackBarLightTopLeft.TabIndex = 1;
            this.trackBarLightTopLeft.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel1.Controls.Add(this.trackBarLightTopLeft, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelLightTop, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelLightBottom, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.trackBarLightTopMiddle, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.trackBarLightTopRight, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.trackBarLightBottom, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelLightTopLeft, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelLightTopMiddle, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelLightTopRight, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lightTopLeft, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lightTopMiddle, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.lightTopRight, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lightBottom, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.numericLightTopLeft, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericLightTopMiddle, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.numericLightTopRight, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.numericLightBottom, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonOn, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonOnAuto, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonOff, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonAdd, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonSave, 5, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(645, 165);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // labelLightTop
            // 
            this.labelLightTop.AutoSize = true;
            this.labelLightTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLightTop.Location = new System.Drawing.Point(1, 1);
            this.labelLightTop.Margin = new System.Windows.Forms.Padding(0);
            this.labelLightTop.Name = "labelLightTop";
            this.tableLayoutPanel1.SetRowSpan(this.labelLightTop, 3);
            this.labelLightTop.Size = new System.Drawing.Size(70, 122);
            this.labelLightTop.TabIndex = 4;
            this.labelLightTop.Text = "Top";
            this.labelLightTop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLightBottom
            // 
            this.labelLightBottom.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.labelLightBottom, 2);
            this.labelLightBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLightBottom.Location = new System.Drawing.Point(1, 124);
            this.labelLightBottom.Margin = new System.Windows.Forms.Padding(0);
            this.labelLightBottom.Name = "labelLightBottom";
            this.labelLightBottom.Size = new System.Drawing.Size(121, 40);
            this.labelLightBottom.TabIndex = 4;
            this.labelLightBottom.Text = "Bottom";
            this.labelLightBottom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // trackBarLightTopMiddle
            // 
            this.trackBarLightTopMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarLightTopMiddle.Location = new System.Drawing.Point(176, 47);
            this.trackBarLightTopMiddle.Margin = new System.Windows.Forms.Padding(5);
            this.trackBarLightTopMiddle.Name = "trackBarLightTopMiddle";
            this.trackBarLightTopMiddle.Size = new System.Drawing.Size(298, 30);
            this.trackBarLightTopMiddle.TabIndex = 1;
            this.trackBarLightTopMiddle.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // trackBarLightTopRight
            // 
            this.trackBarLightTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarLightTopRight.Location = new System.Drawing.Point(176, 88);
            this.trackBarLightTopRight.Margin = new System.Windows.Forms.Padding(5);
            this.trackBarLightTopRight.Name = "trackBarLightTopRight";
            this.trackBarLightTopRight.Size = new System.Drawing.Size(298, 30);
            this.trackBarLightTopRight.TabIndex = 1;
            this.trackBarLightTopRight.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // trackBarLightBottom
            // 
            this.trackBarLightBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBarLightBottom.Location = new System.Drawing.Point(176, 129);
            this.trackBarLightBottom.Margin = new System.Windows.Forms.Padding(5);
            this.trackBarLightBottom.Name = "trackBarLightBottom";
            this.trackBarLightBottom.Size = new System.Drawing.Size(298, 30);
            this.trackBarLightBottom.TabIndex = 1;
            this.trackBarLightBottom.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // labelLightTopLeft
            // 
            this.labelLightTopLeft.AutoSize = true;
            this.labelLightTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLightTopLeft.Location = new System.Drawing.Point(72, 1);
            this.labelLightTopLeft.Margin = new System.Windows.Forms.Padding(0);
            this.labelLightTopLeft.Name = "labelLightTopLeft";
            this.labelLightTopLeft.Size = new System.Drawing.Size(50, 40);
            this.labelLightTopLeft.TabIndex = 4;
            this.labelLightTopLeft.Text = "L";
            this.labelLightTopLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLightTopMiddle
            // 
            this.labelLightTopMiddle.AutoSize = true;
            this.labelLightTopMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLightTopMiddle.Location = new System.Drawing.Point(72, 42);
            this.labelLightTopMiddle.Margin = new System.Windows.Forms.Padding(0);
            this.labelLightTopMiddle.Name = "labelLightTopMiddle";
            this.labelLightTopMiddle.Size = new System.Drawing.Size(50, 40);
            this.labelLightTopMiddle.TabIndex = 4;
            this.labelLightTopMiddle.Text = "M";
            this.labelLightTopMiddle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLightTopRight
            // 
            this.labelLightTopRight.AutoSize = true;
            this.labelLightTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLightTopRight.Location = new System.Drawing.Point(72, 83);
            this.labelLightTopRight.Margin = new System.Windows.Forms.Padding(0);
            this.labelLightTopRight.Name = "labelLightTopRight";
            this.labelLightTopRight.Size = new System.Drawing.Size(50, 40);
            this.labelLightTopRight.TabIndex = 4;
            this.labelLightTopRight.Text = "R";
            this.labelLightTopRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lightTopLeft
            // 
            this.lightTopLeft.AutoSize = true;
            this.lightTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightTopLeft.Location = new System.Drawing.Point(128, 6);
            this.lightTopLeft.Margin = new System.Windows.Forms.Padding(5);
            this.lightTopLeft.Name = "lightTopLeft";
            this.lightTopLeft.Size = new System.Drawing.Size(37, 30);
            this.lightTopLeft.TabIndex = 4;
            this.lightTopLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lightTopMiddle
            // 
            this.lightTopMiddle.AutoSize = true;
            this.lightTopMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightTopMiddle.Location = new System.Drawing.Point(128, 47);
            this.lightTopMiddle.Margin = new System.Windows.Forms.Padding(5);
            this.lightTopMiddle.Name = "lightTopMiddle";
            this.lightTopMiddle.Size = new System.Drawing.Size(37, 30);
            this.lightTopMiddle.TabIndex = 4;
            this.lightTopMiddle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lightTopRight
            // 
            this.lightTopRight.AutoSize = true;
            this.lightTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightTopRight.Location = new System.Drawing.Point(128, 88);
            this.lightTopRight.Margin = new System.Windows.Forms.Padding(5);
            this.lightTopRight.Name = "lightTopRight";
            this.lightTopRight.Size = new System.Drawing.Size(37, 30);
            this.lightTopRight.TabIndex = 4;
            this.lightTopRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lightBottom
            // 
            this.lightBottom.AutoSize = true;
            this.lightBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightBottom.Location = new System.Drawing.Point(128, 129);
            this.lightBottom.Margin = new System.Windows.Forms.Padding(5);
            this.lightBottom.Name = "lightBottom";
            this.lightBottom.Size = new System.Drawing.Size(37, 30);
            this.lightBottom.TabIndex = 4;
            this.lightBottom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericLightTopMiddle
            // 
            this.numericLightTopMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericLightTopMiddle.Location = new System.Drawing.Point(485, 47);
            this.numericLightTopMiddle.Margin = new System.Windows.Forms.Padding(5);
            this.numericLightTopMiddle.Name = "numericLightTopMiddle";
            this.numericLightTopMiddle.Size = new System.Drawing.Size(50, 29);
            this.numericLightTopMiddle.TabIndex = 0;
            this.numericLightTopMiddle.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // numericLightTopRight
            // 
            this.numericLightTopRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericLightTopRight.Location = new System.Drawing.Point(485, 88);
            this.numericLightTopRight.Margin = new System.Windows.Forms.Padding(5);
            this.numericLightTopRight.Name = "numericLightTopRight";
            this.numericLightTopRight.Size = new System.Drawing.Size(50, 29);
            this.numericLightTopRight.TabIndex = 0;
            this.numericLightTopRight.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // numericLightBottom
            // 
            this.numericLightBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericLightBottom.Location = new System.Drawing.Point(485, 129);
            this.numericLightBottom.Margin = new System.Windows.Forms.Padding(5);
            this.numericLightBottom.Name = "numericLightBottom";
            this.numericLightBottom.Size = new System.Drawing.Size(50, 29);
            this.numericLightBottom.TabIndex = 0;
            this.numericLightBottom.ValueChanged += new System.EventHandler(this.numericUpDown_ValueChanged);
            // 
            // buttonOn
            // 
            this.buttonOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOn.Location = new System.Drawing.Point(543, 44);
            this.buttonOn.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOn.Name = "buttonOn";
            this.buttonOn.Size = new System.Drawing.Size(46, 36);
            this.buttonOn.TabIndex = 2;
            this.buttonOn.Text = "ON";
            this.buttonOn.UseVisualStyleBackColor = true;
            this.buttonOn.Click += new System.EventHandler(this.buttonOn_Click);
            // 
            // buttonOnAuto
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.buttonOnAuto, 2);
            this.buttonOnAuto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOnAuto.Location = new System.Drawing.Point(543, 3);
            this.buttonOnAuto.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOnAuto.Name = "buttonOnAuto";
            this.buttonOnAuto.Size = new System.Drawing.Size(99, 36);
            this.buttonOnAuto.TabIndex = 2;
            this.buttonOnAuto.Text = "Auto";
            this.buttonOnAuto.UseVisualStyleBackColor = true;
            this.buttonOnAuto.Click += new System.EventHandler(this.buttonOnAuto_Click);
            // 
            // buttonOff
            // 
            this.buttonOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOff.Location = new System.Drawing.Point(594, 44);
            this.buttonOff.Margin = new System.Windows.Forms.Padding(2);
            this.buttonOff.Name = "buttonOff";
            this.buttonOff.Size = new System.Drawing.Size(48, 36);
            this.buttonOff.TabIndex = 3;
            this.buttonOff.Text = "OFF";
            this.buttonOff.UseVisualStyleBackColor = true;
            this.buttonOff.Click += new System.EventHandler(this.buttonOff_Click);
            // 
            // buttonAdd
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.buttonAdd, 2);
            this.buttonAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAdd.Location = new System.Drawing.Point(543, 85);
            this.buttonAdd.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(99, 36);
            this.buttonAdd.TabIndex = 3;
            this.buttonAdd.Text = "Add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonSave
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.buttonSave, 2);
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSave.Location = new System.Drawing.Point(543, 126);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(99, 36);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnSpd,
            this.columnLightTopL,
            this.columnLightTopM,
            this.columnLightTopR,
            this.columnLightBottom});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 165);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(645, 169);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dataGridView1_RowsRemoved);
            // 
            // columnSpd
            // 
            this.columnSpd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnSpd.HeaderText = "Speed";
            this.columnSpd.Name = "columnSpd";
            // 
            // columnLightTopL
            // 
            this.columnLightTopL.HeaderText = "Top Left";
            this.columnLightTopL.Name = "columnLightTopL";
            this.columnLightTopL.Width = 120;
            // 
            // columnLightTopM
            // 
            this.columnLightTopM.HeaderText = "Top Middle";
            this.columnLightTopM.Name = "columnLightTopM";
            this.columnLightTopM.Width = 120;
            // 
            // columnLightTopR
            // 
            this.columnLightTopR.HeaderText = "Top Right";
            this.columnLightTopR.Name = "columnLightTopR";
            this.columnLightTopR.Width = 120;
            // 
            // columnLightBottom
            // 
            this.columnLightBottom.HeaderText = "Bottom";
            this.columnLightBottom.Name = "columnLightBottom";
            this.columnLightBottom.Width = 120;
            // 
            // LightSettingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LightSettingPanel";
            this.Size = new System.Drawing.Size(645, 334);
            this.Load += new System.EventHandler(this.LightSettingPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericLightTopLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightTopLeft)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightTopMiddle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightTopRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLightBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLightTopMiddle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLightTopRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericLightBottom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numericLightTopLeft;
        private System.Windows.Forms.TrackBar trackBarLightTopLeft;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelLightTop;
        private System.Windows.Forms.Label labelLightBottom;
        private System.Windows.Forms.TrackBar trackBarLightTopMiddle;
        private System.Windows.Forms.TrackBar trackBarLightTopRight;
        private System.Windows.Forms.TrackBar trackBarLightBottom;
        private System.Windows.Forms.NumericUpDown numericLightTopMiddle;
        private System.Windows.Forms.NumericUpDown numericLightTopRight;
        private System.Windows.Forms.NumericUpDown numericLightBottom;
        private System.Windows.Forms.Button buttonOn;
        private System.Windows.Forms.Button buttonOff;
        private System.Windows.Forms.Label labelLightTopLeft;
        private System.Windows.Forms.Label labelLightTopMiddle;
        private System.Windows.Forms.Label labelLightTopRight;
        private System.Windows.Forms.Label lightTopMiddle;
        private System.Windows.Forms.Label lightTopRight;
        private System.Windows.Forms.Label lightBottom;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonOnAuto;
        private System.Windows.Forms.Label lightTopLeft;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSpd;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLightTopL;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLightTopM;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLightTopR;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLightBottom;
        private System.Windows.Forms.Button buttonSave;
    }
}
