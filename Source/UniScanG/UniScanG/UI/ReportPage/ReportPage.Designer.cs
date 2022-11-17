namespace UniScanG.UI.Report
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
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.findModelName = new System.Windows.Forms.TextBox();
            this.totalModel = new System.Windows.Forms.Label();
            this.buttonSearch = new Infragistics.Win.Misc.UltraButton();
            this.totalLot = new System.Windows.Forms.Label();
            this.findLotName = new System.Windows.Forms.TextBox();
            this.labelLot = new System.Windows.Forms.Label();
            this.labelModel = new System.Windows.Forms.Label();
            this.layoutSearchDate = new System.Windows.Forms.TableLayoutPanel();
            this.labelStart = new System.Windows.Forms.Label();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.labelTilda = new System.Windows.Forms.Label();
            this.labelEnd = new System.Windows.Forms.Label();
            this.lotList = new System.Windows.Forms.DataGridView();
            this.columnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modelList = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnThickness = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnPaste = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelSearchDate = new System.Windows.Forms.Label();
            this.labelLotTotal = new System.Windows.Forms.Label();
            this.labelTotalModel = new System.Windows.Forms.Label();
            this.reportContainer = new System.Windows.Forms.Panel();
            this.layoutPanel.SuspendLayout();
            this.layoutSearchDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lotList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelList)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutPanel
            // 
            this.layoutPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutPanel.ColumnCount = 4;
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 111F));
            this.layoutPanel.Controls.Add(this.findModelName, 2, 3);
            this.layoutPanel.Controls.Add(this.totalModel, 1, 3);
            this.layoutPanel.Controls.Add(this.buttonSearch, 3, 6);
            this.layoutPanel.Controls.Add(this.totalLot, 1, 6);
            this.layoutPanel.Controls.Add(this.findLotName, 2, 6);
            this.layoutPanel.Controls.Add(this.labelLot, 0, 5);
            this.layoutPanel.Controls.Add(this.labelModel, 0, 2);
            this.layoutPanel.Controls.Add(this.layoutSearchDate, 0, 1);
            this.layoutPanel.Controls.Add(this.lotList, 0, 7);
            this.layoutPanel.Controls.Add(this.modelList, 0, 4);
            this.layoutPanel.Controls.Add(this.labelSearchDate, 0, 0);
            this.layoutPanel.Controls.Add(this.labelLotTotal, 0, 6);
            this.layoutPanel.Controls.Add(this.labelTotalModel, 0, 3);
            this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.layoutPanel.Location = new System.Drawing.Point(0, 0);
            this.layoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.RowCount = 8;
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.Size = new System.Drawing.Size(600, 580);
            this.layoutPanel.TabIndex = 0;
            // 
            // findModelName
            // 
            this.layoutPanel.SetColumnSpan(this.findModelName, 2);
            this.findModelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.findModelName.Location = new System.Drawing.Point(153, 95);
            this.findModelName.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.findModelName.Name = "findModelName";
            this.findModelName.Size = new System.Drawing.Size(446, 29);
            this.findModelName.TabIndex = 0;
            this.findModelName.TextChanged += new System.EventHandler(this.findModelName_TextChanged);
            // 
            // totalModel
            // 
            this.totalModel.AutoSize = true;
            this.totalModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totalModel.Location = new System.Drawing.Point(102, 94);
            this.totalModel.Margin = new System.Windows.Forms.Padding(0);
            this.totalModel.Name = "totalModel";
            this.totalModel.Size = new System.Drawing.Size(50, 30);
            this.totalModel.TabIndex = 0;
            this.totalModel.Text = "0";
            this.totalModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSearch
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            appearance4.Image = global::UniScanG.Properties.Resources.Defect;
            appearance4.ImageHAlign = Infragistics.Win.HAlign.Left;
            appearance4.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance4.TextHAlignAsString = "Right";
            appearance4.TextVAlignAsString = "Middle";
            this.buttonSearch.Appearance = appearance4;
            this.buttonSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSearch.ImageSize = new System.Drawing.Size(30, 30);
            this.buttonSearch.Location = new System.Drawing.Point(488, 357);
            this.buttonSearch.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(111, 30);
            this.buttonSearch.TabIndex = 148;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // totalLot
            // 
            this.totalLot.AutoSize = true;
            this.totalLot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totalLot.Location = new System.Drawing.Point(102, 357);
            this.totalLot.Margin = new System.Windows.Forms.Padding(0);
            this.totalLot.Name = "totalLot";
            this.totalLot.Size = new System.Drawing.Size(50, 30);
            this.totalLot.TabIndex = 0;
            this.totalLot.Text = "0";
            this.totalLot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // findLotName
            // 
            this.findLotName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.findLotName.Location = new System.Drawing.Point(153, 358);
            this.findLotName.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.findLotName.Name = "findLotName";
            this.findLotName.Size = new System.Drawing.Size(334, 29);
            this.findLotName.TabIndex = 0;
            this.findLotName.TextChanged += new System.EventHandler(this.findLotName_TextChanged);
            // 
            // labelLot
            // 
            this.labelLot.AutoSize = true;
            this.labelLot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.layoutPanel.SetColumnSpan(this.labelLot, 4);
            this.labelLot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLot.Font = new System.Drawing.Font("¸¼Àº °íµñ", 16F, System.Drawing.FontStyle.Bold);
            this.labelLot.Location = new System.Drawing.Point(1, 326);
            this.labelLot.Margin = new System.Windows.Forms.Padding(0);
            this.labelLot.Name = "labelLot";
            this.labelLot.Size = new System.Drawing.Size(598, 30);
            this.labelLot.TabIndex = 0;
            this.labelLot.Text = "Lot";
            this.labelLot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelModel
            // 
            this.labelModel.AutoSize = true;
            this.labelModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.layoutPanel.SetColumnSpan(this.labelModel, 4);
            this.labelModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModel.Font = new System.Drawing.Font("¸¼Àº °íµñ", 16F, System.Drawing.FontStyle.Bold);
            this.labelModel.Location = new System.Drawing.Point(1, 63);
            this.labelModel.Margin = new System.Windows.Forms.Padding(0);
            this.labelModel.Name = "labelModel";
            this.labelModel.Size = new System.Drawing.Size(598, 30);
            this.labelModel.TabIndex = 150;
            this.labelModel.Text = "Model";
            this.labelModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutSearchDate
            // 
            this.layoutSearchDate.ColumnCount = 5;
            this.layoutPanel.SetColumnSpan(this.layoutSearchDate, 4);
            this.layoutSearchDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.layoutSearchDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutSearchDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.layoutSearchDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.layoutSearchDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutSearchDate.Controls.Add(this.labelStart, 0, 0);
            this.layoutSearchDate.Controls.Add(this.startDate, 1, 0);
            this.layoutSearchDate.Controls.Add(this.endDate, 4, 0);
            this.layoutSearchDate.Controls.Add(this.labelTilda, 2, 0);
            this.layoutSearchDate.Controls.Add(this.labelEnd, 3, 0);
            this.layoutSearchDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSearchDate.Location = new System.Drawing.Point(1, 32);
            this.layoutSearchDate.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSearchDate.Name = "layoutSearchDate";
            this.layoutSearchDate.RowCount = 1;
            this.layoutSearchDate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSearchDate.Size = new System.Drawing.Size(598, 30);
            this.layoutSearchDate.TabIndex = 0;
            // 
            // labelStart
            // 
            this.labelStart.AutoSize = true;
            this.labelStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStart.Location = new System.Drawing.Point(0, 0);
            this.labelStart.Margin = new System.Windows.Forms.Padding(0);
            this.labelStart.Name = "labelStart";
            this.labelStart.Size = new System.Drawing.Size(55, 30);
            this.labelStart.TabIndex = 0;
            this.labelStart.Text = "Start";
            this.labelStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // startDate
            // 
            this.startDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDate.Location = new System.Drawing.Point(55, 2);
            this.startDate.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(233, 29);
            this.startDate.TabIndex = 0;
            this.startDate.ValueChanged += new System.EventHandler(this.startDate_ValueChanged);
            // 
            // endDate
            // 
            this.endDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.endDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDate.Location = new System.Drawing.Point(365, 2);
            this.endDate.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(233, 29);
            this.endDate.TabIndex = 0;
            this.endDate.ValueChanged += new System.EventHandler(this.endDate_ValueChanged);
            // 
            // labelTilda
            // 
            this.labelTilda.AutoSize = true;
            this.labelTilda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTilda.Font = new System.Drawing.Font("¸¼Àº °íµñ", 14F, System.Drawing.FontStyle.Bold);
            this.labelTilda.Location = new System.Drawing.Point(288, 0);
            this.labelTilda.Margin = new System.Windows.Forms.Padding(0);
            this.labelTilda.Name = "labelTilda";
            this.labelTilda.Size = new System.Drawing.Size(32, 30);
            this.labelTilda.TabIndex = 0;
            this.labelTilda.Text = "~";
            this.labelTilda.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelEnd
            // 
            this.labelEnd.AutoSize = true;
            this.labelEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEnd.Location = new System.Drawing.Point(320, 0);
            this.labelEnd.Margin = new System.Windows.Forms.Padding(0);
            this.labelEnd.Name = "labelEnd";
            this.labelEnd.Size = new System.Drawing.Size(45, 30);
            this.labelEnd.TabIndex = 0;
            this.labelEnd.Text = "End";
            this.labelEnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lotList
            // 
            this.lotList.AllowUserToAddRows = false;
            this.lotList.AllowUserToDeleteRows = false;
            this.lotList.AllowUserToResizeRows = false;
            this.lotList.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F, System.Drawing.FontStyle.Bold);
            this.lotList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.lotList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lotList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnDate,
            this.columnModel,
            this.columnLotNo});
            this.layoutPanel.SetColumnSpan(this.lotList, 4);
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.lotList.DefaultCellStyle = dataGridViewCellStyle17;
            this.lotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lotList.Location = new System.Drawing.Point(1, 388);
            this.lotList.Margin = new System.Windows.Forms.Padding(0);
            this.lotList.MultiSelect = false;
            this.lotList.Name = "lotList";
            this.lotList.ReadOnly = true;
            this.lotList.RowHeadersVisible = false;
            this.lotList.RowTemplate.Height = 23;
            this.lotList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.lotList.Size = new System.Drawing.Size(598, 191);
            this.lotList.TabIndex = 0;
            // 
            // columnDate
            // 
            this.columnDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnDate.HeaderText = "Date";
            this.columnDate.Name = "columnDate";
            this.columnDate.ReadOnly = true;
            this.columnDate.Width = 71;
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
            this.columnLotNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnLotNo.HeaderText = "Lot No.";
            this.columnLotNo.Name = "columnLotNo";
            this.columnLotNo.ReadOnly = true;
            this.columnLotNo.Width = 92;
            // 
            // modelList
            // 
            this.modelList.AllowUserToAddRows = false;
            this.modelList.AllowUserToDeleteRows = false;
            this.modelList.AllowUserToResizeRows = false;
            this.modelList.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.modelList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.modelList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.modelList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.columnThickness,
            this.columnPaste});
            this.layoutPanel.SetColumnSpan(this.modelList, 4);
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.modelList.DefaultCellStyle = dataGridViewCellStyle20;
            this.modelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelList.Location = new System.Drawing.Point(1, 125);
            this.modelList.Margin = new System.Windows.Forms.Padding(0);
            this.modelList.MultiSelect = false;
            this.modelList.Name = "modelList";
            this.modelList.ReadOnly = true;
            this.modelList.RowHeadersVisible = false;
            this.modelList.RowTemplate.Height = 23;
            this.modelList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.modelList.Size = new System.Drawing.Size(598, 200);
            this.modelList.TabIndex = 0;
            this.modelList.SelectionChanged += new System.EventHandler(this.modelList_SelectionChanged);
            this.modelList.Click += new System.EventHandler(this.modelList_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle19;
            this.dataGridViewTextBoxColumn1.HeaderText = "No.";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 62;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Model";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // columnThickness
            // 
            this.columnThickness.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnThickness.HeaderText = "Thickness";
            this.columnThickness.Name = "columnThickness";
            this.columnThickness.ReadOnly = true;
            this.columnThickness.Width = 108;
            // 
            // columnPaste
            // 
            this.columnPaste.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnPaste.HeaderText = "Paste";
            this.columnPaste.Name = "columnPaste";
            this.columnPaste.ReadOnly = true;
            this.columnPaste.Width = 76;
            // 
            // labelSearchDate
            // 
            this.labelSearchDate.AutoSize = true;
            this.labelSearchDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.layoutPanel.SetColumnSpan(this.labelSearchDate, 4);
            this.labelSearchDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSearchDate.Font = new System.Drawing.Font("¸¼Àº °íµñ", 16F, System.Drawing.FontStyle.Bold);
            this.labelSearchDate.Location = new System.Drawing.Point(1, 1);
            this.labelSearchDate.Margin = new System.Windows.Forms.Padding(0);
            this.labelSearchDate.Name = "labelSearchDate";
            this.labelSearchDate.Size = new System.Drawing.Size(598, 30);
            this.labelSearchDate.TabIndex = 149;
            this.labelSearchDate.Text = "Search Date";
            this.labelSearchDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLotTotal
            // 
            this.labelLotTotal.AutoSize = true;
            this.labelLotTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLotTotal.Location = new System.Drawing.Point(1, 357);
            this.labelLotTotal.Margin = new System.Windows.Forms.Padding(0);
            this.labelLotTotal.Name = "labelLotTotal";
            this.labelLotTotal.Size = new System.Drawing.Size(100, 30);
            this.labelLotTotal.TabIndex = 4;
            this.labelLotTotal.Text = "Total";
            this.labelLotTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTotalModel
            // 
            this.labelTotalModel.AutoSize = true;
            this.labelTotalModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTotalModel.Location = new System.Drawing.Point(1, 94);
            this.labelTotalModel.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalModel.Name = "labelTotalModel";
            this.labelTotalModel.Size = new System.Drawing.Size(100, 30);
            this.labelTotalModel.TabIndex = 3;
            this.labelTotalModel.Text = "Total";
            this.labelTotalModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // reportContainer
            // 
            this.reportContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportContainer.Location = new System.Drawing.Point(600, 0);
            this.reportContainer.Margin = new System.Windows.Forms.Padding(0);
            this.reportContainer.Name = "reportContainer";
            this.reportContainer.Size = new System.Drawing.Size(788, 580);
            this.reportContainer.TabIndex = 0;
            // 
            // ReportPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.reportContainer);
            this.Controls.Add(this.layoutPanel);
            this.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ReportPage";
            this.Size = new System.Drawing.Size(1388, 580);
            this.VisibleChanged += new System.EventHandler(this.ReportPage_VisibleChanged);
            this.layoutPanel.ResumeLayout(false);
            this.layoutPanel.PerformLayout();
            this.layoutSearchDate.ResumeLayout(false);
            this.layoutSearchDate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lotList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutPanel;
        private Infragistics.Win.Misc.UltraButton buttonSearch;
        private System.Windows.Forms.Label labelLotTotal;
        private System.Windows.Forms.TextBox findLotName;
        private System.Windows.Forms.Label totalLot;
        private System.Windows.Forms.TextBox findModelName;
        private System.Windows.Forms.Label labelTotalModel;
        private System.Windows.Forms.Label totalModel;
        private System.Windows.Forms.Label labelLot;
        private System.Windows.Forms.Label labelModel;
        private System.Windows.Forms.Label labelStart;
        private System.Windows.Forms.DateTimePicker startDate;
        private System.Windows.Forms.Label labelEnd;
        private System.Windows.Forms.DateTimePicker endDate;
        private System.Windows.Forms.Label labelTilda;
        private System.Windows.Forms.DataGridView lotList;
        private System.Windows.Forms.DataGridView modelList;
        private System.Windows.Forms.Label labelSearchDate;
        private System.Windows.Forms.Panel reportContainer;
        private System.Windows.Forms.TableLayoutPanel layoutSearchDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnThickness;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPaste;
    }
}
