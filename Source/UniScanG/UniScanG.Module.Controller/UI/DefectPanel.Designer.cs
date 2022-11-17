namespace MLCCS.Operation.UI.Monitor
{
    partial class DefectPanel
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.layoutDefect = new System.Windows.Forms.TableLayoutPanel();
            this.lastDefectView = new System.Windows.Forms.DataGridView();
            this.layoutTop = new System.Windows.Forms.TableLayoutPanel();
            this.groupFilter = new System.Windows.Forms.GroupBox();
            this.filterPanel = new System.Windows.Forms.Panel();
            this.groupType = new System.Windows.Forms.GroupBox();
            this.radioBlackCircle = new System.Windows.Forms.RadioButton();
            this.radioSheetAttack = new System.Windows.Forms.RadioButton();
            this.radioBlackLine = new System.Windows.Forms.RadioButton();
            this.radioShape = new System.Windows.Forms.RadioButton();
            this.radioPinHole = new System.Windows.Forms.RadioButton();
            this.radioTotal = new System.Windows.Forms.RadioButton();
            this.radioWhite = new System.Windows.Forms.RadioButton();
            this.groupBoxSizeFilter = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.defectSizeMax1 = new System.Windows.Forms.NumericUpDown();
            this.defectSizeMin1 = new System.Windows.Forms.NumericUpDown();
            this.useSizeFilter = new System.Windows.Forms.CheckBox();
            this.layoutTotal = new System.Windows.Forms.TableLayoutPanel();
            this.totalDefect = new System.Windows.Forms.Label();
            this.labelTotalDefect = new System.Windows.Forms.Label();
            this.columnIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnImage = new System.Windows.Forms.DataGridViewImageColumn();
            this.layoutDefect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lastDefectView)).BeginInit();
            this.layoutTop.SuspendLayout();
            this.groupFilter.SuspendLayout();
            this.filterPanel.SuspendLayout();
            this.groupType.SuspendLayout();
            this.groupBoxSizeFilter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defectSizeMax1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.defectSizeMin1)).BeginInit();
            this.layoutTotal.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutDefect
            // 
            this.layoutDefect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(240)))));
            this.layoutDefect.ColumnCount = 1;
            this.layoutDefect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutDefect.Controls.Add(this.lastDefectView, 0, 1);
            this.layoutDefect.Controls.Add(this.layoutTop, 0, 0);
            this.layoutDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutDefect.Location = new System.Drawing.Point(0, 0);
            this.layoutDefect.Margin = new System.Windows.Forms.Padding(0);
            this.layoutDefect.Name = "layoutDefect";
            this.layoutDefect.RowCount = 2;
            this.layoutDefect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.layoutDefect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutDefect.Size = new System.Drawing.Size(1109, 771);
            this.layoutDefect.TabIndex = 105;
            // 
            // lastDefectView
            // 
            this.lastDefectView.AllowUserToAddRows = false;
            this.lastDefectView.AllowUserToDeleteRows = false;
            this.lastDefectView.AllowUserToResizeColumns = false;
            this.lastDefectView.AllowUserToResizeRows = false;
            this.lastDefectView.BackgroundColor = System.Drawing.Color.White;
            this.lastDefectView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.lastDefectView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.lastDefectView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lastDefectView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnIndex,
            this.columnQty,
            this.columnType,
            this.columnInfo,
            this.columnImage});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.lastDefectView.DefaultCellStyle = dataGridViewCellStyle2;
            this.lastDefectView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lastDefectView.Location = new System.Drawing.Point(0, 180);
            this.lastDefectView.Margin = new System.Windows.Forms.Padding(0);
            this.lastDefectView.MultiSelect = false;
            this.lastDefectView.Name = "lastDefectView";
            this.lastDefectView.ReadOnly = true;
            this.lastDefectView.RowHeadersVisible = false;
            this.lastDefectView.RowTemplate.Height = 23;
            this.lastDefectView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.lastDefectView.Size = new System.Drawing.Size(1109, 591);
            this.lastDefectView.TabIndex = 49;
            // 
            // layoutTop
            // 
            this.layoutTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(198)))), ((int)(((byte)(220)))));
            this.layoutTop.ColumnCount = 2;
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTop.Controls.Add(this.groupFilter, 0, 0);
            this.layoutTop.Controls.Add(this.layoutTotal, 0, 0);
            this.layoutTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutTop.Location = new System.Drawing.Point(0, 0);
            this.layoutTop.Margin = new System.Windows.Forms.Padding(0);
            this.layoutTop.Name = "layoutTop";
            this.layoutTop.RowCount = 1;
            this.layoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTop.Size = new System.Drawing.Size(1109, 180);
            this.layoutTop.TabIndex = 85;
            // 
            // groupFilter
            // 
            this.groupFilter.BackColor = System.Drawing.Color.Transparent;
            this.groupFilter.Controls.Add(this.filterPanel);
            this.groupFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupFilter.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupFilter.Location = new System.Drawing.Point(180, 0);
            this.groupFilter.Margin = new System.Windows.Forms.Padding(0);
            this.groupFilter.Name = "groupFilter";
            this.groupFilter.Size = new System.Drawing.Size(929, 180);
            this.groupFilter.TabIndex = 106;
            this.groupFilter.TabStop = false;
            this.groupFilter.Text = "Filter";
            // 
            // filterPanel
            // 
            this.filterPanel.BackColor = System.Drawing.Color.Transparent;
            this.filterPanel.Controls.Add(this.groupType);
            this.filterPanel.Controls.Add(this.groupBoxSizeFilter);
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterPanel.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.filterPanel.Location = new System.Drawing.Point(3, 35);
            this.filterPanel.Margin = new System.Windows.Forms.Padding(0);
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Size = new System.Drawing.Size(923, 142);
            this.filterPanel.TabIndex = 1;
            // 
            // groupType
            // 
            this.groupType.Controls.Add(this.radioBlackCircle);
            this.groupType.Controls.Add(this.radioSheetAttack);
            this.groupType.Controls.Add(this.radioBlackLine);
            this.groupType.Controls.Add(this.radioShape);
            this.groupType.Controls.Add(this.radioPinHole);
            this.groupType.Controls.Add(this.radioTotal);
            this.groupType.Controls.Add(this.radioWhite);
            this.groupType.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupType.Location = new System.Drawing.Point(16, 5);
            this.groupType.Name = "groupType";
            this.groupType.Size = new System.Drawing.Size(615, 61);
            this.groupType.TabIndex = 87;
            this.groupType.TabStop = false;
            this.groupType.Text = "Type";
            // 
            // radioBlackCircle
            // 
            this.radioBlackCircle.AutoSize = true;
            this.radioBlackCircle.BackColor = System.Drawing.Color.Transparent;
            this.radioBlackCircle.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioBlackCircle.ForeColor = System.Drawing.Color.Red;
            this.radioBlackCircle.Location = new System.Drawing.Point(288, 28);
            this.radioBlackCircle.Name = "radioBlackCircle";
            this.radioBlackCircle.Size = new System.Drawing.Size(108, 25);
            this.radioBlackCircle.TabIndex = 68;
            this.radioBlackCircle.Text = "전극 (원형)";
            this.radioBlackCircle.UseVisualStyleBackColor = false;
            // 
            // radioSheetAttack
            // 
            this.radioSheetAttack.AutoSize = true;
            this.radioSheetAttack.BackColor = System.Drawing.Color.Transparent;
            this.radioSheetAttack.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioSheetAttack.ForeColor = System.Drawing.Color.Maroon;
            this.radioSheetAttack.Location = new System.Drawing.Point(76, 28);
            this.radioSheetAttack.Name = "radioSheetAttack";
            this.radioSheetAttack.Size = new System.Drawing.Size(92, 25);
            this.radioSheetAttack.TabIndex = 79;
            this.radioSheetAttack.Text = "시트어택";
            this.radioSheetAttack.UseVisualStyleBackColor = false;
            // 
            // radioBlackLine
            // 
            this.radioBlackLine.AutoSize = true;
            this.radioBlackLine.BackColor = System.Drawing.Color.Transparent;
            this.radioBlackLine.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioBlackLine.ForeColor = System.Drawing.Color.OrangeRed;
            this.radioBlackLine.Location = new System.Drawing.Point(174, 28);
            this.radioBlackLine.Name = "radioBlackLine";
            this.radioBlackLine.Size = new System.Drawing.Size(108, 25);
            this.radioBlackLine.TabIndex = 64;
            this.radioBlackLine.Text = "전극 (선형)";
            this.radioBlackLine.UseVisualStyleBackColor = false;
            // 
            // radioShape
            // 
            this.radioShape.AutoSize = true;
            this.radioShape.BackColor = System.Drawing.Color.Transparent;
            this.radioShape.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioShape.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(0)))));
            this.radioShape.Location = new System.Drawing.Point(534, 28);
            this.radioShape.Name = "radioShape";
            this.radioShape.Size = new System.Drawing.Size(76, 25);
            this.radioShape.TabIndex = 67;
            this.radioShape.Text = "해상도";
            this.radioShape.UseVisualStyleBackColor = false;
            // 
            // radioPinHole
            // 
            this.radioPinHole.AutoSize = true;
            this.radioPinHole.BackColor = System.Drawing.Color.Transparent;
            this.radioPinHole.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioPinHole.ForeColor = System.Drawing.Color.Magenta;
            this.radioPinHole.Location = new System.Drawing.Point(468, 28);
            this.radioPinHole.Name = "radioPinHole";
            this.radioPinHole.Size = new System.Drawing.Size(60, 25);
            this.radioPinHole.TabIndex = 66;
            this.radioPinHole.Text = "핀홀";
            this.radioPinHole.UseVisualStyleBackColor = false;
            // 
            // radioTotal
            // 
            this.radioTotal.AutoSize = true;
            this.radioTotal.BackColor = System.Drawing.Color.Transparent;
            this.radioTotal.Checked = true;
            this.radioTotal.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioTotal.Location = new System.Drawing.Point(10, 28);
            this.radioTotal.Name = "radioTotal";
            this.radioTotal.Size = new System.Drawing.Size(60, 25);
            this.radioTotal.TabIndex = 58;
            this.radioTotal.TabStop = true;
            this.radioTotal.Text = "전체";
            this.radioTotal.UseVisualStyleBackColor = false;
            // 
            // radioWhite
            // 
            this.radioWhite.AutoSize = true;
            this.radioWhite.BackColor = System.Drawing.Color.Transparent;
            this.radioWhite.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.radioWhite.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(0)))));
            this.radioWhite.Location = new System.Drawing.Point(402, 28);
            this.radioWhite.Name = "radioWhite";
            this.radioWhite.Size = new System.Drawing.Size(60, 25);
            this.radioWhite.TabIndex = 65;
            this.radioWhite.Text = "성형";
            this.radioWhite.UseVisualStyleBackColor = false;
            // 
            // groupBoxSizeFilter
            // 
            this.groupBoxSizeFilter.Controls.Add(this.label2);
            this.groupBoxSizeFilter.Controls.Add(this.label1);
            this.groupBoxSizeFilter.Controls.Add(this.label7);
            this.groupBoxSizeFilter.Controls.Add(this.label3);
            this.groupBoxSizeFilter.Controls.Add(this.defectSizeMax1);
            this.groupBoxSizeFilter.Controls.Add(this.defectSizeMin1);
            this.groupBoxSizeFilter.Controls.Add(this.useSizeFilter);
            this.groupBoxSizeFilter.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBoxSizeFilter.Location = new System.Drawing.Point(16, 66);
            this.groupBoxSizeFilter.Name = "groupBoxSizeFilter";
            this.groupBoxSizeFilter.Size = new System.Drawing.Size(338, 68);
            this.groupBoxSizeFilter.TabIndex = 84;
            this.groupBoxSizeFilter.TabStop = false;
            this.groupBoxSizeFilter.Text = "Size";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(133, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 21);
            this.label2.TabIndex = 85;
            this.label2.Text = "~";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(158, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 21);
            this.label1.TabIndex = 84;
            this.label1.Text = "Max";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(277, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 21);
            this.label7.TabIndex = 83;
            this.label7.Text = "(um)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(16, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 21);
            this.label3.TabIndex = 73;
            this.label3.Text = "Min";
            // 
            // defectSizeMax1
            // 
            this.defectSizeMax1.Enabled = false;
            this.defectSizeMax1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.defectSizeMax1.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.defectSizeMax1.Location = new System.Drawing.Point(204, 30);
            this.defectSizeMax1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.defectSizeMax1.Name = "defectSizeMax1";
            this.defectSizeMax1.Size = new System.Drawing.Size(67, 29);
            this.defectSizeMax1.TabIndex = 72;
            // 
            // defectSizeMin1
            // 
            this.defectSizeMin1.Enabled = false;
            this.defectSizeMin1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.defectSizeMin1.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.defectSizeMin1.Location = new System.Drawing.Point(60, 30);
            this.defectSizeMin1.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.defectSizeMin1.Name = "defectSizeMin1";
            this.defectSizeMin1.Size = new System.Drawing.Size(67, 29);
            this.defectSizeMin1.TabIndex = 69;
            // 
            // useSizeFilter
            // 
            this.useSizeFilter.AutoSize = true;
            this.useSizeFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(218)))), ((int)(((byte)(240)))));
            this.useSizeFilter.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.useSizeFilter.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.useSizeFilter.Location = new System.Drawing.Point(49, 8);
            this.useSizeFilter.Name = "useSizeFilter";
            this.useSizeFilter.Size = new System.Drawing.Size(15, 14);
            this.useSizeFilter.TabIndex = 80;
            this.useSizeFilter.UseVisualStyleBackColor = false;
            // 
            // layoutTotal
            // 
            this.layoutTotal.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutTotal.ColumnCount = 1;
            this.layoutTotal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutTotal.Controls.Add(this.totalDefect, 0, 1);
            this.layoutTotal.Controls.Add(this.labelTotalDefect, 0, 0);
            this.layoutTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutTotal.Location = new System.Drawing.Point(0, 0);
            this.layoutTotal.Margin = new System.Windows.Forms.Padding(0);
            this.layoutTotal.Name = "layoutTotal";
            this.layoutTotal.RowCount = 2;
            this.layoutTotal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTotal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.layoutTotal.Size = new System.Drawing.Size(180, 180);
            this.layoutTotal.TabIndex = 105;
            // 
            // totalDefect
            // 
            this.totalDefect.AutoSize = true;
            this.totalDefect.BackColor = System.Drawing.Color.White;
            this.totalDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.totalDefect.Font = new System.Drawing.Font("맑은 고딕", 20F);
            this.totalDefect.Location = new System.Drawing.Point(1, 99);
            this.totalDefect.Margin = new System.Windows.Forms.Padding(0);
            this.totalDefect.Name = "totalDefect";
            this.totalDefect.Size = new System.Drawing.Size(178, 80);
            this.totalDefect.TabIndex = 102;
            this.totalDefect.Text = "no";
            this.totalDefect.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelTotalDefect
            // 
            this.labelTotalDefect.AutoSize = true;
            this.labelTotalDefect.BackColor = System.Drawing.Color.Red;
            this.labelTotalDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTotalDefect.Font = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Bold);
            this.labelTotalDefect.ForeColor = System.Drawing.Color.White;
            this.labelTotalDefect.Location = new System.Drawing.Point(1, 1);
            this.labelTotalDefect.Margin = new System.Windows.Forms.Padding(0);
            this.labelTotalDefect.Name = "labelTotalDefect";
            this.labelTotalDefect.Size = new System.Drawing.Size(178, 97);
            this.labelTotalDefect.TabIndex = 100;
            this.labelTotalDefect.Text = "Total Defect";
            this.labelTotalDefect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // columnIndex
            // 
            this.columnIndex.HeaderText = "Pattern";
            this.columnIndex.Name = "columnIndex";
            this.columnIndex.ReadOnly = true;
            this.columnIndex.Width = 110;
            // 
            // columnQty
            // 
            this.columnQty.HeaderText = "Qty.";
            this.columnQty.Name = "columnQty";
            this.columnQty.ReadOnly = true;
            this.columnQty.Width = 90;
            // 
            // columnType
            // 
            this.columnType.HeaderText = "Type";
            this.columnType.Name = "columnType";
            this.columnType.ReadOnly = true;
            this.columnType.Width = 140;
            // 
            // columnInfo
            // 
            this.columnInfo.HeaderText = "Info";
            this.columnInfo.Name = "columnInfo";
            this.columnInfo.ReadOnly = true;
            this.columnInfo.Width = 270;
            // 
            // columnImage
            // 
            this.columnImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnImage.HeaderText = "Image";
            this.columnImage.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.columnImage.Name = "columnImage";
            this.columnImage.ReadOnly = true;
            // 
            // DefectViewPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutDefect);
            this.Name = "DefectViewPanel";
            this.Size = new System.Drawing.Size(1109, 771);
            this.layoutDefect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lastDefectView)).EndInit();
            this.layoutTop.ResumeLayout(false);
            this.groupFilter.ResumeLayout(false);
            this.filterPanel.ResumeLayout(false);
            this.groupType.ResumeLayout(false);
            this.groupType.PerformLayout();
            this.groupBoxSizeFilter.ResumeLayout(false);
            this.groupBoxSizeFilter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defectSizeMax1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.defectSizeMin1)).EndInit();
            this.layoutTotal.ResumeLayout(false);
            this.layoutTotal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel layoutDefect;
        private System.Windows.Forms.TableLayoutPanel layoutTop;
        private System.Windows.Forms.DataGridView lastDefectView;
        private System.Windows.Forms.TableLayoutPanel layoutTotal;
        public System.Windows.Forms.Label totalDefect;
        public System.Windows.Forms.Label labelTotalDefect;
        private System.Windows.Forms.GroupBox groupFilter;
        private System.Windows.Forms.Panel filterPanel;
        private System.Windows.Forms.GroupBox groupType;
        private System.Windows.Forms.RadioButton radioBlackCircle;
        private System.Windows.Forms.RadioButton radioSheetAttack;
        private System.Windows.Forms.RadioButton radioBlackLine;
        private System.Windows.Forms.RadioButton radioShape;
        private System.Windows.Forms.RadioButton radioPinHole;
        private System.Windows.Forms.RadioButton radioTotal;
        private System.Windows.Forms.RadioButton radioWhite;
        private System.Windows.Forms.GroupBox groupBoxSizeFilter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown defectSizeMax1;
        private System.Windows.Forms.NumericUpDown defectSizeMin1;
        private System.Windows.Forms.CheckBox useSizeFilter;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnInfo;
        private System.Windows.Forms.DataGridViewImageColumn columnImage;
    }
}
