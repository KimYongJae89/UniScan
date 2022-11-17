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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.labelSearchDate = new System.Windows.Forms.Label();
            this.lotList = new System.Windows.Forms.DataGridView();
            this.modelList = new System.Windows.Forms.DataGridView();
            this.columnModelNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnModelModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnModelThickness = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnModelPaste = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelLotTotal = new System.Windows.Forms.Label();
            this.labelTotalModel = new System.Windows.Forms.Label();
            this.lotDataGridContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportContainer = new System.Windows.Forms.Panel();
            this.ultraSplitter1 = new Infragistics.Win.Misc.UltraSplitter();
            this.layoutPage = new System.Windows.Forms.TableLayoutPanel();
            this.layoutLot = new System.Windows.Forms.TableLayoutPanel();
            this.layoutModel = new System.Windows.Forms.TableLayoutPanel();
            this.columnLotDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotMachine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotLotNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotTotalCnt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNgNP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNgNPRatio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNgNPGrade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNgPH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNgPHRatio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNgPHGrade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNgSP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNgSA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNgCO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNgST = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNgMG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNgTF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNgRatio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNgGrade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotKill = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotKillGood = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotSpecChip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotSpecCoat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotSpdMpm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLotNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.layoutSearchDate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lotList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelList)).BeginInit();
            this.lotDataGridContextMenuStrip.SuspendLayout();
            this.layoutPage.SuspendLayout();
            this.layoutLot.SuspendLayout();
            this.layoutModel.SuspendLayout();
            this.SuspendLayout();
            // 
            // findModelName
            // 
            this.findModelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.findModelName.Location = new System.Drawing.Point(67, 36);
            this.findModelName.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.findModelName.Name = "findModelName";
            this.findModelName.Size = new System.Drawing.Size(1626, 29);
            this.findModelName.TabIndex = 0;
            this.findModelName.TextChanged += new System.EventHandler(this.findModelName_TextChanged);
            this.findModelName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.findModelName_KeyPress);
            // 
            // totalModel
            // 
            this.totalModel.AutoSize = true;
            this.totalModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totalModel.Location = new System.Drawing.Point(48, 35);
            this.totalModel.Margin = new System.Windows.Forms.Padding(0);
            this.totalModel.Name = "totalModel";
            this.totalModel.Size = new System.Drawing.Size(19, 30);
            this.totalModel.TabIndex = 0;
            this.totalModel.Text = "0";
            this.totalModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSearch
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.Image = global::UniScanG.Properties.Resources.Defect;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Left;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            appearance1.TextHAlignAsString = "Right";
            appearance1.TextVAlignAsString = "Middle";
            this.buttonSearch.Appearance = appearance1;
            this.buttonSearch.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSearch.ImageSize = new System.Drawing.Size(30, 30);
            this.buttonSearch.Location = new System.Drawing.Point(1526, 35);
            this.buttonSearch.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(167, 30);
            this.buttonSearch.TabIndex = 148;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // totalLot
            // 
            this.totalLot.AutoSize = true;
            this.totalLot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totalLot.Location = new System.Drawing.Point(48, 35);
            this.totalLot.Margin = new System.Windows.Forms.Padding(0);
            this.totalLot.Name = "totalLot";
            this.totalLot.Size = new System.Drawing.Size(19, 30);
            this.totalLot.TabIndex = 0;
            this.totalLot.Text = "0";
            this.totalLot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // findLotName
            // 
            this.findLotName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.findLotName.Location = new System.Drawing.Point(67, 36);
            this.findLotName.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.findLotName.Name = "findLotName";
            this.findLotName.Size = new System.Drawing.Size(1459, 29);
            this.findLotName.TabIndex = 0;
            this.findLotName.TextChanged += new System.EventHandler(this.findLotName_TextChanged);
            this.findLotName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.findLotName_KeyPress);
            // 
            // labelLot
            // 
            this.labelLot.AutoSize = true;
            this.labelLot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.layoutLot.SetColumnSpan(this.labelLot, 4);
            this.labelLot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLot.Font = new System.Drawing.Font("¸¼Àº °íµñ", 16F, System.Drawing.FontStyle.Bold);
            this.labelLot.Location = new System.Drawing.Point(0, 0);
            this.labelLot.Margin = new System.Windows.Forms.Padding(0);
            this.labelLot.Name = "labelLot";
            this.labelLot.Size = new System.Drawing.Size(1693, 35);
            this.labelLot.TabIndex = 0;
            this.labelLot.Text = "Lot";
            this.labelLot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelModel
            // 
            this.labelModel.AutoSize = true;
            this.labelModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.layoutModel.SetColumnSpan(this.labelModel, 3);
            this.labelModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModel.Font = new System.Drawing.Font("¸¼Àº °íµñ", 16F, System.Drawing.FontStyle.Bold);
            this.labelModel.Location = new System.Drawing.Point(0, 0);
            this.labelModel.Margin = new System.Windows.Forms.Padding(0);
            this.labelModel.Name = "labelModel";
            this.labelModel.Size = new System.Drawing.Size(1693, 35);
            this.labelModel.TabIndex = 150;
            this.labelModel.Text = "Model";
            this.labelModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutSearchDate
            // 
            this.layoutSearchDate.AutoSize = true;
            this.layoutSearchDate.ColumnCount = 5;
            this.layoutSearchDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.layoutSearchDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutSearchDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.layoutSearchDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.layoutSearchDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutSearchDate.Controls.Add(this.labelStart, 0, 1);
            this.layoutSearchDate.Controls.Add(this.startDate, 1, 1);
            this.layoutSearchDate.Controls.Add(this.endDate, 4, 1);
            this.layoutSearchDate.Controls.Add(this.labelTilda, 2, 1);
            this.layoutSearchDate.Controls.Add(this.labelEnd, 3, 1);
            this.layoutSearchDate.Controls.Add(this.labelSearchDate, 0, 0);
            this.layoutSearchDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutSearchDate.Location = new System.Drawing.Point(3, 3);
            this.layoutSearchDate.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSearchDate.Name = "layoutSearchDate";
            this.layoutSearchDate.RowCount = 2;
            this.layoutSearchDate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutSearchDate.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutSearchDate.Size = new System.Drawing.Size(1699, 66);
            this.layoutSearchDate.TabIndex = 0;
            // 
            // labelStart
            // 
            this.labelStart.AutoSize = true;
            this.labelStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelStart.Location = new System.Drawing.Point(0, 35);
            this.labelStart.Margin = new System.Windows.Forms.Padding(0);
            this.labelStart.Name = "labelStart";
            this.labelStart.Size = new System.Drawing.Size(55, 31);
            this.labelStart.TabIndex = 0;
            this.labelStart.Text = "Start";
            this.labelStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // startDate
            // 
            this.startDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.startDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDate.Location = new System.Drawing.Point(55, 37);
            this.startDate.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(783, 29);
            this.startDate.TabIndex = 0;
            this.startDate.ValueChanged += new System.EventHandler(this.startDate_ValueChanged);
            // 
            // endDate
            // 
            this.endDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.endDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDate.Location = new System.Drawing.Point(915, 37);
            this.endDate.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(784, 29);
            this.endDate.TabIndex = 0;
            this.endDate.ValueChanged += new System.EventHandler(this.endDate_ValueChanged);
            // 
            // labelTilda
            // 
            this.labelTilda.AutoSize = true;
            this.labelTilda.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTilda.Font = new System.Drawing.Font("¸¼Àº °íµñ", 14F, System.Drawing.FontStyle.Bold);
            this.labelTilda.Location = new System.Drawing.Point(838, 35);
            this.labelTilda.Margin = new System.Windows.Forms.Padding(0);
            this.labelTilda.Name = "labelTilda";
            this.labelTilda.Size = new System.Drawing.Size(32, 31);
            this.labelTilda.TabIndex = 0;
            this.labelTilda.Text = "~";
            this.labelTilda.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelEnd
            // 
            this.labelEnd.AutoSize = true;
            this.labelEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEnd.Location = new System.Drawing.Point(870, 35);
            this.labelEnd.Margin = new System.Windows.Forms.Padding(0);
            this.labelEnd.Name = "labelEnd";
            this.labelEnd.Size = new System.Drawing.Size(45, 31);
            this.labelEnd.TabIndex = 0;
            this.labelEnd.Text = "End";
            this.labelEnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSearchDate
            // 
            this.labelSearchDate.AutoSize = true;
            this.labelSearchDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.layoutSearchDate.SetColumnSpan(this.labelSearchDate, 5);
            this.labelSearchDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSearchDate.Font = new System.Drawing.Font("¸¼Àº °íµñ", 16F, System.Drawing.FontStyle.Bold);
            this.labelSearchDate.Location = new System.Drawing.Point(0, 0);
            this.labelSearchDate.Margin = new System.Windows.Forms.Padding(0);
            this.labelSearchDate.Name = "labelSearchDate";
            this.labelSearchDate.Size = new System.Drawing.Size(1699, 35);
            this.labelSearchDate.TabIndex = 149;
            this.labelSearchDate.Text = "Search Date";
            this.labelSearchDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lotList
            // 
            this.lotList.AllowUserToAddRows = false;
            this.lotList.AllowUserToDeleteRows = false;
            this.lotList.AllowUserToResizeRows = false;
            this.lotList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.lotList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.lotList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.lotList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.lotList.ColumnHeadersHeight = 49;
            this.lotList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.lotList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnLotDate,
            this.columnLotMachine,
            this.columnLotModel,
            this.columnLotLotNo,
            this.columnLotTotalCnt,
            this.columnLotNgNP,
            this.columnLotNgNPRatio,
            this.columnLotNgNPGrade,
            this.columnLotNgPH,
            this.columnLotNgPHRatio,
            this.columnLotNgPHGrade,
            this.columnLotNgSP,
            this.columnLotNgSA,
            this.columnLotNgCO,
            this.columnLotNgST,
            this.columnLotNgMG,
            this.columnLotNgTF,
            this.columnLotNg,
            this.columnLotNgRatio,
            this.columnLotNgGrade,
            this.columnLotKill,
            this.columnLotKillGood,
            this.columnLotSpecChip,
            this.columnLotSpecCoat,
            this.columnLotSpdMpm,
            this.columnLotNote});
            this.layoutLot.SetColumnSpan(this.lotList, 4);
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.lotList.DefaultCellStyle = dataGridViewCellStyle3;
            this.lotList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lotList.Location = new System.Drawing.Point(0, 65);
            this.lotList.Margin = new System.Windows.Forms.Padding(0);
            this.lotList.Name = "lotList";
            this.lotList.RowHeadersVisible = false;
            this.lotList.RowTemplate.Height = 23;
            this.lotList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.lotList.Size = new System.Drawing.Size(1693, 231);
            this.lotList.TabIndex = 0;
            this.lotList.ColumnHeadersHeightChanged += new System.EventHandler(this.lotList_ColumnHeadersHeightChanged);
            this.lotList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.lotList_CellDoubleClick);
            this.lotList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.lotList_CellValueChanged);
            // 
            // modelList
            // 
            this.modelList.AllowUserToAddRows = false;
            this.modelList.AllowUserToDeleteRows = false;
            this.modelList.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.modelList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.modelList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.modelList.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.modelList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.modelList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.modelList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnModelNo,
            this.columnModelModel,
            this.columnModelThickness,
            this.columnModelPaste});
            this.layoutModel.SetColumnSpan(this.modelList, 3);
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.modelList.DefaultCellStyle = dataGridViewCellStyle6;
            this.modelList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelList.Location = new System.Drawing.Point(0, 65);
            this.modelList.Margin = new System.Windows.Forms.Padding(0);
            this.modelList.MultiSelect = false;
            this.modelList.Name = "modelList";
            this.modelList.ReadOnly = true;
            this.modelList.RowHeadersVisible = false;
            this.modelList.RowTemplate.Height = 23;
            this.modelList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.modelList.Size = new System.Drawing.Size(1693, 200);
            this.modelList.TabIndex = 0;
            this.modelList.SelectionChanged += new System.EventHandler(this.modelList_SelectionChanged);
            this.modelList.Click += new System.EventHandler(this.modelList_Click);
            // 
            // columnModelNo
            // 
            this.columnModelNo.HeaderText = "No.";
            this.columnModelNo.Name = "columnModelNo";
            this.columnModelNo.ReadOnly = true;
            this.columnModelNo.Width = 62;
            // 
            // columnModelModel
            // 
            this.columnModelModel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnModelModel.HeaderText = "Model";
            this.columnModelModel.Name = "columnModelModel";
            this.columnModelModel.ReadOnly = true;
            // 
            // columnModelThickness
            // 
            this.columnModelThickness.HeaderText = "Thickness [um]";
            this.columnModelThickness.MinimumWidth = 100;
            this.columnModelThickness.Name = "columnModelThickness";
            this.columnModelThickness.ReadOnly = true;
            this.columnModelThickness.Width = 151;
            // 
            // columnModelPaste
            // 
            this.columnModelPaste.HeaderText = "Paste";
            this.columnModelPaste.MinimumWidth = 100;
            this.columnModelPaste.Name = "columnModelPaste";
            this.columnModelPaste.ReadOnly = true;
            // 
            // labelLotTotal
            // 
            this.labelLotTotal.AutoSize = true;
            this.labelLotTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLotTotal.Location = new System.Drawing.Point(0, 35);
            this.labelLotTotal.Margin = new System.Windows.Forms.Padding(0);
            this.labelLotTotal.Name = "labelLotTotal";
            this.labelLotTotal.Size = new System.Drawing.Size(48, 30);
            this.labelLotTotal.TabIndex = 4;
            this.labelLotTotal.Text = "Total";
            this.labelLotTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTotalModel
            // 
            this.labelTotalModel.AutoSize = true;
            this.labelTotalModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTotalModel.Location = new System.Drawing.Point(0, 35);
            this.labelTotalModel.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalModel.Name = "labelTotalModel";
            this.labelTotalModel.Size = new System.Drawing.Size(48, 30);
            this.labelTotalModel.TabIndex = 3;
            this.labelTotalModel.Text = "Total";
            this.labelTotalModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lotDataGridContextMenuStrip
            // 
            this.lotDataGridContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDirectoryToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteToolStripMenuItem,
            this.clearCacheToolStripMenuItem});
            this.lotDataGridContextMenuStrip.Name = "lotDataGridContextMenuStrip";
            this.lotDataGridContextMenuStrip.Size = new System.Drawing.Size(157, 76);
            // 
            // openDirectoryToolStripMenuItem
            // 
            this.openDirectoryToolStripMenuItem.Name = "openDirectoryToolStripMenuItem";
            this.openDirectoryToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.openDirectoryToolStripMenuItem.Text = "Open Directory";
            this.openDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openDirectoryToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(153, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // clearCacheToolStripMenuItem
            // 
            this.clearCacheToolStripMenuItem.Name = "clearCacheToolStripMenuItem";
            this.clearCacheToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.clearCacheToolStripMenuItem.Text = "Clear Cache";
            this.clearCacheToolStripMenuItem.Click += new System.EventHandler(this.clearCacheToolStripMenuItem_Click);
            // 
            // reportContainer
            // 
            this.reportContainer.Dock = System.Windows.Forms.DockStyle.Right;
            this.reportContainer.Location = new System.Drawing.Point(1720, 0);
            this.reportContainer.Margin = new System.Windows.Forms.Padding(0);
            this.reportContainer.Name = "reportContainer";
            this.reportContainer.Size = new System.Drawing.Size(0, 580);
            this.reportContainer.TabIndex = 0;
            // 
            // ultraSplitter1
            // 
            this.ultraSplitter1.BackColor = System.Drawing.Color.AliceBlue;
            this.ultraSplitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.ultraSplitter1.Location = new System.Drawing.Point(1705, 0);
            this.ultraSplitter1.Name = "ultraSplitter1";
            this.ultraSplitter1.RestoreExtent = 496;
            this.ultraSplitter1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ultraSplitter1.Size = new System.Drawing.Size(15, 580);
            this.ultraSplitter1.TabIndex = 1;
            // 
            // layoutPage
            // 
            this.layoutPage.AutoSize = true;
            this.layoutPage.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.layoutPage.ColumnCount = 1;
            this.layoutPage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPage.Controls.Add(this.layoutLot, 0, 2);
            this.layoutPage.Controls.Add(this.layoutSearchDate, 0, 0);
            this.layoutPage.Controls.Add(this.layoutModel, 0, 1);
            this.layoutPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPage.Location = new System.Drawing.Point(0, 0);
            this.layoutPage.Name = "layoutPage";
            this.layoutPage.RowCount = 3;
            this.layoutPage.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutPage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.layoutPage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.layoutPage.Size = new System.Drawing.Size(1705, 580);
            this.layoutPage.TabIndex = 2;
            // 
            // layoutLot
            // 
            this.layoutLot.AutoSize = true;
            this.layoutLot.ColumnCount = 4;
            this.layoutLot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutLot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutLot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutLot.Controls.Add(this.buttonSearch, 3, 1);
            this.layoutLot.Controls.Add(this.labelLot, 0, 0);
            this.layoutLot.Controls.Add(this.lotList, 0, 2);
            this.layoutLot.Controls.Add(this.findLotName, 2, 1);
            this.layoutLot.Controls.Add(this.totalLot, 1, 1);
            this.layoutLot.Controls.Add(this.labelLotTotal, 0, 1);
            this.layoutLot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutLot.Location = new System.Drawing.Point(6, 278);
            this.layoutLot.Name = "layoutLot";
            this.layoutLot.RowCount = 3;
            this.layoutLot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutLot.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutLot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLot.Size = new System.Drawing.Size(1693, 296);
            this.layoutLot.TabIndex = 3;
            // 
            // layoutModel
            // 
            this.layoutModel.AutoSize = true;
            this.layoutModel.ColumnCount = 3;
            this.layoutModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutModel.Controls.Add(this.findModelName, 2, 1);
            this.layoutModel.Controls.Add(this.labelModel, 0, 0);
            this.layoutModel.Controls.Add(this.totalModel, 1, 1);
            this.layoutModel.Controls.Add(this.labelTotalModel, 0, 1);
            this.layoutModel.Controls.Add(this.modelList, 0, 2);
            this.layoutModel.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutModel.Location = new System.Drawing.Point(6, 75);
            this.layoutModel.Name = "layoutModel";
            this.layoutModel.RowCount = 3;
            this.layoutModel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutModel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutModel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutModel.Size = new System.Drawing.Size(1693, 194);
            this.layoutModel.TabIndex = 3;
            // 
            // columnLotDate
            // 
            this.columnLotDate.HeaderText = "Date";
            this.columnLotDate.MinimumWidth = 150;
            this.columnLotDate.Name = "columnLotDate";
            this.columnLotDate.ReadOnly = true;
            this.columnLotDate.Width = 150;
            // 
            // columnLotMachine
            // 
            this.columnLotMachine.HeaderText = "Machine";
            this.columnLotMachine.MinimumWidth = 100;
            this.columnLotMachine.Name = "columnLotMachine";
            this.columnLotMachine.ReadOnly = true;
            // 
            // columnLotModel
            // 
            this.columnLotModel.HeaderText = "Model";
            this.columnLotModel.MinimumWidth = 200;
            this.columnLotModel.Name = "columnLotModel";
            this.columnLotModel.ReadOnly = true;
            this.columnLotModel.Width = 200;
            // 
            // columnLotLotNo
            // 
            this.columnLotLotNo.HeaderText = "Lot No.";
            this.columnLotLotNo.MinimumWidth = 125;
            this.columnLotLotNo.Name = "columnLotLotNo";
            this.columnLotLotNo.ReadOnly = true;
            this.columnLotLotNo.Width = 125;
            // 
            // columnLotTotalCnt
            // 
            this.columnLotTotalCnt.HeaderText = "Total";
            this.columnLotTotalCnt.MinimumWidth = 70;
            this.columnLotTotalCnt.Name = "columnLotTotalCnt";
            this.columnLotTotalCnt.ReadOnly = true;
            this.columnLotTotalCnt.Width = 70;
            // 
            // columnLotNgNP
            // 
            this.columnLotNgNP.HeaderText = "Noprint";
            this.columnLotNgNP.MinimumWidth = 70;
            this.columnLotNgNP.Name = "columnLotNgNP";
            this.columnLotNgNP.ReadOnly = true;
            this.columnLotNgNP.Width = 70;
            // 
            // columnLotNgNPRatio
            // 
            this.columnLotNgNPRatio.HeaderText = "Noprint [%]";
            this.columnLotNgNPRatio.MinimumWidth = 70;
            this.columnLotNgNPRatio.Name = "columnLotNgNPRatio";
            this.columnLotNgNPRatio.ReadOnly = true;
            this.columnLotNgNPRatio.Width = 70;
            // 
            // columnLotNgNPGrade
            // 
            this.columnLotNgNPGrade.HeaderText = "Noprint [G]";
            this.columnLotNgNPGrade.MinimumWidth = 70;
            this.columnLotNgNPGrade.Name = "columnLotNgNPGrade";
            this.columnLotNgNPGrade.ReadOnly = true;
            this.columnLotNgNPGrade.Width = 70;
            // 
            // columnLotNgPH
            // 
            this.columnLotNgPH.HeaderText = "Pinhole";
            this.columnLotNgPH.MinimumWidth = 70;
            this.columnLotNgPH.Name = "columnLotNgPH";
            this.columnLotNgPH.ReadOnly = true;
            this.columnLotNgPH.Width = 70;
            // 
            // columnLotNgPHRatio
            // 
            this.columnLotNgPHRatio.HeaderText = "Pinhole [%]";
            this.columnLotNgPHRatio.MinimumWidth = 70;
            this.columnLotNgPHRatio.Name = "columnLotNgPHRatio";
            this.columnLotNgPHRatio.ReadOnly = true;
            this.columnLotNgPHRatio.Width = 70;
            // 
            // columnLotNgPHGrade
            // 
            this.columnLotNgPHGrade.HeaderText = "Pinhole [G]";
            this.columnLotNgPHGrade.MinimumWidth = 70;
            this.columnLotNgPHGrade.Name = "columnLotNgPHGrade";
            this.columnLotNgPHGrade.ReadOnly = true;
            this.columnLotNgPHGrade.Width = 70;
            // 
            // columnLotNgSP
            // 
            this.columnLotNgSP.HeaderText = "Spread";
            this.columnLotNgSP.MinimumWidth = 70;
            this.columnLotNgSP.Name = "columnLotNgSP";
            this.columnLotNgSP.ReadOnly = true;
            this.columnLotNgSP.Width = 70;
            // 
            // columnLotNgSA
            // 
            this.columnLotNgSA.HeaderText = "Attack";
            this.columnLotNgSA.MinimumWidth = 70;
            this.columnLotNgSA.Name = "columnLotNgSA";
            this.columnLotNgSA.ReadOnly = true;
            this.columnLotNgSA.Width = 70;
            // 
            // columnLotNgCO
            // 
            this.columnLotNgCO.HeaderText = "Coating";
            this.columnLotNgCO.MinimumWidth = 70;
            this.columnLotNgCO.Name = "columnLotNgCO";
            this.columnLotNgCO.ReadOnly = true;
            this.columnLotNgCO.Width = 70;
            // 
            // columnLotNgST
            // 
            this.columnLotNgST.HeaderText = "Sticker";
            this.columnLotNgST.MinimumWidth = 70;
            this.columnLotNgST.Name = "columnLotNgST";
            this.columnLotNgST.ReadOnly = true;
            this.columnLotNgST.Width = 70;
            // 
            // columnLotNgMG
            // 
            this.columnLotNgMG.HeaderText = "Margin";
            this.columnLotNgMG.MinimumWidth = 70;
            this.columnLotNgMG.Name = "columnLotNgMG";
            this.columnLotNgMG.ReadOnly = true;
            this.columnLotNgMG.Width = 70;
            // 
            // columnLotNgOFS
            // 
            this.columnLotNgTF.HeaderText = "Trans";
            this.columnLotNgTF.MinimumWidth = 70;
            this.columnLotNgTF.Name = "columnLotNgTF";
            this.columnLotNgTF.ReadOnly = true;
            this.columnLotNgTF.Width = 70;
            // 
            // columnLotNg
            // 
            this.columnLotNg.HeaderText = "NG";
            this.columnLotNg.MinimumWidth = 70;
            this.columnLotNg.Name = "columnLotNg";
            this.columnLotNg.ReadOnly = true;
            this.columnLotNg.Width = 70;
            // 
            // columnLotNgRatio
            // 
            this.columnLotNgRatio.HeaderText = "Ng [%]";
            this.columnLotNgRatio.MinimumWidth = 70;
            this.columnLotNgRatio.Name = "columnLotNgRatio";
            this.columnLotNgRatio.ReadOnly = true;
            this.columnLotNgRatio.Width = 70;
            // 
            // columnLotNgGrade
            // 
            this.columnLotNgGrade.HeaderText = "NG [G]";
            this.columnLotNgGrade.MinimumWidth = 70;
            this.columnLotNgGrade.Name = "columnLotNgGrade";
            this.columnLotNgGrade.ReadOnly = true;
            this.columnLotNgGrade.Width = 70;
            // 
            // columnLotKill
            // 
            this.columnLotKill.HeaderText = "Kill";
            this.columnLotKill.MinimumWidth = 70;
            this.columnLotKill.Name = "columnLotKill";
            this.columnLotKill.ReadOnly = true;
            this.columnLotKill.Width = 70;
            // 
            // columnLotKillGood
            // 
            this.columnLotKillGood.HeaderText = "K.Good";
            this.columnLotKillGood.MinimumWidth = 70;
            this.columnLotKillGood.Name = "columnLotKillGood";
            this.columnLotKillGood.ReadOnly = true;
            this.columnLotKillGood.Width = 70;
            // 
            // columnLotSpecChip
            // 
            this.columnLotSpecChip.HeaderText = "Chip Spec. [um]";
            this.columnLotSpecChip.MinimumWidth = 80;
            this.columnLotSpecChip.Name = "columnLotSpecChip";
            this.columnLotSpecChip.ReadOnly = true;
            this.columnLotSpecChip.Width = 80;
            // 
            // columnLotSpecCoat
            // 
            this.columnLotSpecCoat.HeaderText = "Coating Spec. [um]";
            this.columnLotSpecCoat.MinimumWidth = 80;
            this.columnLotSpecCoat.Name = "columnLotSpecCoat";
            this.columnLotSpecCoat.ReadOnly = true;
            this.columnLotSpecCoat.Width = 80;
            // 
            // columnLotSpdMpm
            // 
            this.columnLotSpdMpm.HeaderText = "Speed [m/m]";
            this.columnLotSpdMpm.MinimumWidth = 65;
            this.columnLotSpdMpm.Name = "columnLotSpdMpm";
            this.columnLotSpdMpm.ReadOnly = true;
            this.columnLotSpdMpm.Width = 65;
            // 
            // columnLotNote
            // 
            this.columnLotNote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.columnLotNote.DefaultCellStyle = dataGridViewCellStyle2;
            this.columnLotNote.HeaderText = "Note";
            this.columnLotNote.MinimumWidth = 300;
            this.columnLotNote.Name = "columnLotNote";
            this.columnLotNote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ReportPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.layoutPage);
            this.Controls.Add(this.ultraSplitter1);
            this.Controls.Add(this.reportContainer);
            this.Font = new System.Drawing.Font("¸¼Àº °íµñ", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ReportPage";
            this.Size = new System.Drawing.Size(1720, 580);
            this.Load += new System.EventHandler(this.ReportPage_Load);
            this.VisibleChanged += new System.EventHandler(this.ReportPage_VisibleChanged);
            this.layoutSearchDate.ResumeLayout(false);
            this.layoutSearchDate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lotList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelList)).EndInit();
            this.lotDataGridContextMenuStrip.ResumeLayout(false);
            this.layoutPage.ResumeLayout(false);
            this.layoutPage.PerformLayout();
            this.layoutLot.ResumeLayout(false);
            this.layoutLot.PerformLayout();
            this.layoutModel.ResumeLayout(false);
            this.layoutModel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private Infragistics.Win.Misc.UltraSplitter ultraSplitter1;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnModelNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnModelModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnModelThickness;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnModelPaste;
        private System.Windows.Forms.ContextMenuStrip lotDataGridContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem openDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel layoutLot;
        private System.Windows.Forms.TableLayoutPanel layoutModel;
        private System.Windows.Forms.TableLayoutPanel layoutPage;
        private System.Windows.Forms.ToolStripMenuItem clearCacheToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotMachine;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotLotNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotTotalCnt;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNgNP;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNgNPRatio;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNgNPGrade;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNgPH;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNgPHRatio;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNgPHGrade;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNgSP;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNgSA;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNgCO;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNgST;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNgMG;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNgTF;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNg;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNgRatio;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNgGrade;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotKill;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotKillGood;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotSpecChip;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotSpecCoat;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotSpdMpm;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLotNote;
    }
}
