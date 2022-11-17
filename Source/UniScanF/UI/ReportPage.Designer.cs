namespace UniScan.UI
{
    partial class ReportPage
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
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            this.btnSearch = new System.Windows.Forms.Button();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.labelTilda = new System.Windows.Forms.Label();
            this.searchPanel = new System.Windows.Forms.Panel();
            this.labelSearchTime = new Infragistics.Win.Misc.UltraLabel();
            this.findModel = new System.Windows.Forms.TextBox();
            this.totalModel = new Infragistics.Win.Misc.UltraLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.totalLot = new Infragistics.Win.Misc.UltraLabel();
            this.modelList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.findLotNo = new System.Windows.Forms.TextBox();
            this.labelFindLotNo = new Infragistics.Win.Misc.UltraLabel();
            this.labelModelName = new Infragistics.Win.Misc.UltraLabel();
            this.lotNoList = new System.Windows.Forms.DataGridView();
            this.columnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.resultImageTable = new System.Windows.Forms.TableLayoutPanel();
            this.searchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotNoList)).BeginInit();
            this.leftPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F);
            this.btnSearch.Image = global::UniScan.Properties.Resources.test_32;
            this.btnSearch.Location = new System.Drawing.Point(288, 395);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(126, 39);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "Search";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // endDate
            // 
            this.endDate.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F);
            this.endDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDate.Location = new System.Drawing.Point(289, 40);
            this.endDate.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(125, 29);
            this.endDate.TabIndex = 13;
            this.endDate.ValueChanged += new System.EventHandler(this.endDate_ValueChanged);
            // 
            // startDate
            // 
            this.startDate.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F);
            this.startDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDate.Location = new System.Drawing.Point(58, 40);
            this.startDate.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(125, 29);
            this.startDate.TabIndex = 12;
            this.startDate.ValueChanged += new System.EventHandler(this.startDate_ValueChanged);
            // 
            // labelTilda
            // 
            this.labelTilda.AutoSize = true;
            this.labelTilda.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F);
            this.labelTilda.Location = new System.Drawing.Point(4, 42);
            this.labelTilda.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTilda.Name = "labelTilda";
            this.labelTilda.Size = new System.Drawing.Size(45, 21);
            this.labelTilda.TabIndex = 18;
            this.labelTilda.Text = "Start";
            this.labelTilda.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // searchPanel
            // 
            this.searchPanel.Controls.Add(this.labelSearchTime);
            this.searchPanel.Controls.Add(this.endDate);
            this.searchPanel.Controls.Add(this.findModel);
            this.searchPanel.Controls.Add(this.startDate);
            this.searchPanel.Controls.Add(this.totalModel);
            this.searchPanel.Controls.Add(this.label2);
            this.searchPanel.Controls.Add(this.totalLot);
            this.searchPanel.Controls.Add(this.labelTilda);
            this.searchPanel.Controls.Add(this.modelList);
            this.searchPanel.Controls.Add(this.label1);
            this.searchPanel.Controls.Add(this.findLotNo);
            this.searchPanel.Controls.Add(this.labelFindLotNo);
            this.searchPanel.Controls.Add(this.labelModelName);
            this.searchPanel.Controls.Add(this.btnSearch);
            this.searchPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchPanel.Location = new System.Drawing.Point(0, 0);
            this.searchPanel.Name = "searchPanel";
            this.searchPanel.Size = new System.Drawing.Size(418, 440);
            this.searchPanel.TabIndex = 21;
            // 
            // labelSearchTime
            // 
            appearance6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(240)))));
            appearance6.FontData.BoldAsString = "False";
            appearance6.FontData.Name = "Malgun Gothic";
            appearance6.FontData.SizeInPoints = 14F;
            appearance6.ForeColor = System.Drawing.Color.Black;
            appearance6.TextHAlignAsString = "Center";
            appearance6.TextVAlignAsString = "Middle";
            this.labelSearchTime.Appearance = appearance6;
            this.labelSearchTime.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.labelSearchTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelSearchTime.Location = new System.Drawing.Point(4, 3);
            this.labelSearchTime.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.labelSearchTime.Name = "labelSearchTime";
            this.labelSearchTime.Size = new System.Drawing.Size(410, 29);
            this.labelSearchTime.TabIndex = 52;
            this.labelSearchTime.Text = "Search Date";
            // 
            // findModel
            // 
            this.findModel.Font = new System.Drawing.Font("¸¼Àº °íµñ", 14F);
            this.findModel.Location = new System.Drawing.Point(116, 109);
            this.findModel.Name = "findModel";
            this.findModel.Size = new System.Drawing.Size(298, 32);
            this.findModel.TabIndex = 59;
            this.findModel.TextChanged += new System.EventHandler(this.findModel_TextChanged);
            // 
            // totalModel
            // 
            appearance7.FontData.Name = "Malgun Gothic";
            appearance7.FontData.SizeInPoints = 12F;
            appearance7.ForeColor = System.Drawing.Color.Black;
            appearance7.TextHAlignAsString = "Center";
            appearance7.TextVAlignAsString = "Middle";
            this.totalModel.Appearance = appearance7;
            this.totalModel.Font = new System.Drawing.Font("¸¼Àº °íµñ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalModel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.totalModel.Location = new System.Drawing.Point(3, 109);
            this.totalModel.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.totalModel.Name = "totalModel";
            this.totalModel.Size = new System.Drawing.Size(110, 35);
            this.totalModel.TabIndex = 58;
            this.totalModel.Text = "Total  : 100";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F);
            this.label2.Location = new System.Drawing.Point(204, 42);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 21);
            this.label2.TabIndex = 56;
            this.label2.Text = "~";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // totalLot
            // 
            appearance8.FontData.Name = "Malgun Gothic";
            appearance8.FontData.SizeInPoints = 12F;
            appearance8.ForeColor = System.Drawing.Color.Black;
            appearance8.TextHAlignAsString = "Center";
            appearance8.TextVAlignAsString = "Middle";
            this.totalLot.Appearance = appearance8;
            this.totalLot.Font = new System.Drawing.Font("¸¼Àº °íµñ", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.totalLot.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.totalLot.Location = new System.Drawing.Point(4, 395);
            this.totalLot.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.totalLot.Name = "totalLot";
            this.totalLot.Size = new System.Drawing.Size(109, 35);
            this.totalLot.TabIndex = 57;
            this.totalLot.Text = "Total  : 100";
            // 
            // modelList
            // 
            this.modelList.AllowUserToAddRows = false;
            this.modelList.AllowUserToDeleteRows = false;
            this.modelList.AllowUserToResizeRows = false;
            this.modelList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.modelList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.modelList.Location = new System.Drawing.Point(3, 146);
            this.modelList.MultiSelect = false;
            this.modelList.Name = "modelList";
            this.modelList.ReadOnly = true;
            this.modelList.RowHeadersVisible = false;
            this.modelList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.modelList.RowTemplate.Height = 23;
            this.modelList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.modelList.Size = new System.Drawing.Size(411, 211);
            this.modelList.TabIndex = 55;
            this.modelList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.modelList_CellClick);
            this.modelList.SelectionChanged += new System.EventHandler(this.modelList_SelectionChanged);
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn1.HeaderText = "No.";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Model";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Registration Date";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 105;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F);
            this.label1.Location = new System.Drawing.Point(243, 42);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 21);
            this.label1.TabIndex = 54;
            this.label1.Text = "End";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // findLotNo
            // 
            this.findLotNo.Font = new System.Drawing.Font("¸¼Àº °íµñ", 14F);
            this.findLotNo.Location = new System.Drawing.Point(116, 398);
            this.findLotNo.Name = "findLotNo";
            this.findLotNo.ReadOnly = true;
            this.findLotNo.Size = new System.Drawing.Size(164, 32);
            this.findLotNo.TabIndex = 51;
            // 
            // labelFindLotNo
            // 
            appearance9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(240)))));
            appearance9.FontData.BoldAsString = "False";
            appearance9.FontData.Name = "Malgun Gothic";
            appearance9.FontData.SizeInPoints = 14F;
            appearance9.ForeColor = System.Drawing.Color.Black;
            appearance9.TextHAlignAsString = "Center";
            appearance9.TextVAlignAsString = "Middle";
            this.labelFindLotNo.Appearance = appearance9;
            this.labelFindLotNo.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.labelFindLotNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelFindLotNo.Location = new System.Drawing.Point(3, 363);
            this.labelFindLotNo.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.labelFindLotNo.Name = "labelFindLotNo";
            this.labelFindLotNo.Size = new System.Drawing.Size(411, 29);
            this.labelFindLotNo.TabIndex = 53;
            this.labelFindLotNo.Text = "Lot No.";
            // 
            // labelModelName
            // 
            appearance10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(240)))));
            appearance10.FontData.BoldAsString = "False";
            appearance10.FontData.Name = "Malgun Gothic";
            appearance10.FontData.SizeInPoints = 14F;
            appearance10.ForeColor = System.Drawing.Color.Black;
            appearance10.TextHAlignAsString = "Center";
            appearance10.TextVAlignAsString = "Middle";
            this.labelModelName.Appearance = appearance10;
            this.labelModelName.BorderStyleInner = Infragistics.Win.UIElementBorderStyle.Solid;
            this.labelModelName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelModelName.Location = new System.Drawing.Point(3, 77);
            this.labelModelName.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.labelModelName.Name = "labelModelName";
            this.labelModelName.Size = new System.Drawing.Size(411, 29);
            this.labelModelName.TabIndex = 15;
            this.labelModelName.Text = "Model";
            // 
            // lotNoList
            // 
            this.lotNoList.AllowUserToAddRows = false;
            this.lotNoList.AllowUserToDeleteRows = false;
            this.lotNoList.AllowUserToResizeRows = false;
            this.lotNoList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lotNoList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnDate,
            this.columnModel,
            this.columnLotNo});
            this.lotNoList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lotNoList.Location = new System.Drawing.Point(0, 440);
            this.lotNoList.MultiSelect = false;
            this.lotNoList.Name = "lotNoList";
            this.lotNoList.ReadOnly = true;
            this.lotNoList.RowHeadersVisible = false;
            this.lotNoList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lotNoList.RowTemplate.Height = 23;
            this.lotNoList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.lotNoList.Size = new System.Drawing.Size(418, 112);
            this.lotNoList.TabIndex = 21;
            this.lotNoList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.lotNoList_CellClick);
            this.lotNoList.SelectionChanged += new System.EventHandler(this.serialDataGridView_SelectionChanged);
            // 
            // columnDate
            // 
            this.columnDate.HeaderText = "Date";
            this.columnDate.Name = "columnDate";
            this.columnDate.ReadOnly = true;
            this.columnDate.Width = 120;
            // 
            // columnModel
            // 
            this.columnModel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnModel.HeaderText = "Model";
            this.columnModel.Name = "columnModel";
            this.columnModel.ReadOnly = true;
            // 
            // columnLotNo
            // 
            this.columnLotNo.HeaderText = "Lot NO.";
            this.columnLotNo.Name = "columnLotNo";
            this.columnLotNo.ReadOnly = true;
            // 
            // leftPanel
            // 
            this.leftPanel.Controls.Add(this.lotNoList);
            this.leftPanel.Controls.Add(this.searchPanel);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 0);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(418, 552);
            this.leftPanel.TabIndex = 23;
            // 
            // resultImageTable
            // 
            this.resultImageTable.ColumnCount = 1;
            this.resultImageTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.resultImageTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultImageTable.Location = new System.Drawing.Point(418, 0);
            this.resultImageTable.Name = "resultImageTable";
            this.resultImageTable.RowCount = 1;
            this.resultImageTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.resultImageTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 552F));
            this.resultImageTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 552F));
            this.resultImageTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 552F));
            this.resultImageTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 552F));
            this.resultImageTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 552F));
            this.resultImageTable.Size = new System.Drawing.Size(831, 552);
            this.resultImageTable.TabIndex = 23;
            // 
            // ReportPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.resultImageTable);
            this.Controls.Add(this.leftPanel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.Name = "ReportPanel";
            this.Size = new System.Drawing.Size(1249, 552);
            this.searchPanel.ResumeLayout(false);
            this.searchPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lotNoList)).EndInit();
            this.leftPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DateTimePicker endDate;
        private System.Windows.Forms.DateTimePicker startDate;
        private System.Windows.Forms.Label labelTilda;
        private System.Windows.Forms.Panel searchPanel;
        private System.Windows.Forms.DataGridView lotNoList;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.TableLayoutPanel resultImageTable;
        private System.Windows.Forms.TextBox findLotNo;
        private Infragistics.Win.Misc.UltraLabel labelModelName;
        private Infragistics.Win.Misc.UltraLabel labelSearchTime;
        private Infragistics.Win.Misc.UltraLabel labelFindLotNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView modelList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox findModel;
        private Infragistics.Win.Misc.UltraLabel totalModel;
        private Infragistics.Win.Misc.UltraLabel totalLot;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNo;
    }
}
