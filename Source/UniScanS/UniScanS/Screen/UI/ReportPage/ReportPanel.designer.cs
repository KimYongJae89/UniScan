namespace UniScanG.Screen.UI.Report
{
    partial class ReportPanel
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
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.layoutTop = new System.Windows.Forms.TableLayoutPanel();
            this.layoutFolder = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCam = new Infragistics.Win.Misc.UltraButton();
            this.labelFolder = new System.Windows.Forms.Label();
            this.layoutFilter = new System.Windows.Forms.TableLayoutPanel();
            this.layoutSelectFilter = new System.Windows.Forms.TableLayoutPanel();
            this.layoutType = new System.Windows.Forms.TableLayoutPanel();
            this.shape = new System.Windows.Forms.RadioButton();
            this.pinHole = new System.Windows.Forms.RadioButton();
            this.dielectric = new System.Windows.Forms.RadioButton();
            this.poleCircle = new System.Windows.Forms.RadioButton();
            this.poleLine = new System.Windows.Forms.RadioButton();
            this.sheetAttack = new System.Windows.Forms.RadioButton();
            this.total = new System.Windows.Forms.RadioButton();
            this.layoutSize = new System.Windows.Forms.TableLayoutPanel();
            this.labelMin = new System.Windows.Forms.Label();
            this.labelMaxUnit = new System.Windows.Forms.Label();
            this.sizeMax = new System.Windows.Forms.NumericUpDown();
            this.labelMinUnit = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.sizeMin = new System.Windows.Forms.NumericUpDown();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.labelFilterTitle = new System.Windows.Forms.Label();
            this.layoutbottom = new System.Windows.Forms.TableLayoutPanel();
            this.layoutImage = new System.Windows.Forms.TableLayoutPanel();
            this.layoutDefectNum = new System.Windows.Forms.TableLayoutPanel();
            this.poleLineNum = new System.Windows.Forms.Label();
            this.pinHoleNum = new System.Windows.Forms.Label();
            this.poleCircleNum = new System.Windows.Forms.Label();
            this.shapeNum = new System.Windows.Forms.Label();
            this.dielectricNum = new System.Windows.Forms.Label();
            this.sheetAttackNum = new System.Windows.Forms.Label();
            this.labelSheetAttack = new Infragistics.Win.Misc.UltraLabel();
            this.labelDielectric = new Infragistics.Win.Misc.UltraLabel();
            this.labelShape = new Infragistics.Win.Misc.UltraLabel();
            this.labelPinHole = new Infragistics.Win.Misc.UltraLabel();
            this.labelPoleLine = new Infragistics.Win.Misc.UltraLabel();
            this.labelPoleCircle = new Infragistics.Win.Misc.UltraLabel();
            this.labelImage = new System.Windows.Forms.Label();
            this.imagePanel = new System.Windows.Forms.Panel();
            this.layoutDefectList = new System.Windows.Forms.TableLayoutPanel();
            this.labelDefectImage = new System.Windows.Forms.Label();
            this.labelDefectList = new System.Windows.Forms.Label();
            this.defectList = new System.Windows.Forms.DataGridView();
            this.columnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDefectType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.defectImage = new System.Windows.Forms.PictureBox();
            this.layoutSheetList = new System.Windows.Forms.TableLayoutPanel();
            this.sheetList = new System.Windows.Forms.DataGridView();
            this.columnPattern = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelInfo = new System.Windows.Forms.Label();
            this.buttonSelectAll = new Infragistics.Win.Misc.UltraButton();
            this.labelSheetList = new System.Windows.Forms.Label();
            this.layoutResultFilter = new System.Windows.Forms.TableLayoutPanel();
            this.ngFilter = new System.Windows.Forms.CheckBox();
            this.okFilter = new System.Windows.Forms.CheckBox();
            this.layoutInfo = new System.Windows.Forms.TableLayoutPanel();
            this.labelSheetTotal = new System.Windows.Forms.Label();
            this.labelSheetNG = new System.Windows.Forms.Label();
            this.sheetNG = new System.Windows.Forms.Label();
            this.sheetTotal = new System.Windows.Forms.Label();
            this.sheetRatio = new System.Windows.Forms.Label();
            this.labelSheetRatio = new System.Windows.Forms.Label();
            this.labelUnit = new System.Windows.Forms.Label();
            this.useSize = new System.Windows.Forms.CheckBox();
            this.layoutMain.SuspendLayout();
            this.layoutTop.SuspendLayout();
            this.layoutFolder.SuspendLayout();
            this.layoutFilter.SuspendLayout();
            this.layoutSelectFilter.SuspendLayout();
            this.layoutType.SuspendLayout();
            this.layoutSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMin)).BeginInit();
            this.layoutbottom.SuspendLayout();
            this.layoutImage.SuspendLayout();
            this.layoutDefectNum.SuspendLayout();
            this.layoutDefectList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defectList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.defectImage)).BeginInit();
            this.layoutSheetList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetList)).BeginInit();
            this.layoutResultFilter.SuspendLayout();
            this.layoutInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.layoutTop, 0, 0);
            this.layoutMain.Controls.Add(this.layoutbottom, 0, 1);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 2;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Size = new System.Drawing.Size(1206, 734);
            this.layoutMain.TabIndex = 0;
            // 
            // layoutTop
            // 
            this.layoutTop.ColumnCount = 2;
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutTop.Controls.Add(this.layoutFolder, 1, 0);
            this.layoutTop.Controls.Add(this.layoutFilter, 0, 0);
            this.layoutTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutTop.Location = new System.Drawing.Point(0, 0);
            this.layoutTop.Margin = new System.Windows.Forms.Padding(0);
            this.layoutTop.Name = "layoutTop";
            this.layoutTop.RowCount = 1;
            this.layoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTop.Size = new System.Drawing.Size(1206, 125);
            this.layoutTop.TabIndex = 143;
            // 
            // layoutFolder
            // 
            this.layoutFolder.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutFolder.ColumnCount = 1;
            this.layoutFolder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutFolder.Controls.Add(this.buttonCam, 0, 1);
            this.layoutFolder.Controls.Add(this.labelFolder, 0, 0);
            this.layoutFolder.Location = new System.Drawing.Point(1106, 0);
            this.layoutFolder.Margin = new System.Windows.Forms.Padding(0);
            this.layoutFolder.Name = "layoutFolder";
            this.layoutFolder.RowCount = 2;
            this.layoutFolder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutFolder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutFolder.Size = new System.Drawing.Size(100, 125);
            this.layoutFolder.TabIndex = 112;
            this.layoutFolder.Visible = false;
            // 
            // buttonCam
            // 
            appearance9.BackColor = System.Drawing.Color.White;
            appearance9.Image = global::UniScanG.Properties.Resources.picture_folder_32;
            appearance9.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.buttonCam.Appearance = appearance9;
            this.buttonCam.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCam.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonCam.ImageSize = new System.Drawing.Size(40, 40);
            this.buttonCam.Location = new System.Drawing.Point(2, 63);
            this.buttonCam.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCam.Name = "buttonCam";
            this.buttonCam.Size = new System.Drawing.Size(96, 60);
            this.buttonCam.TabIndex = 146;
            this.buttonCam.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonCam.Click += new System.EventHandler(this.buttonCam_Click);
            // 
            // labelFolder
            // 
            this.labelFolder.AutoSize = true;
            this.labelFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFolder.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelFolder.Location = new System.Drawing.Point(2, 2);
            this.labelFolder.Margin = new System.Windows.Forms.Padding(0);
            this.labelFolder.Name = "labelFolder";
            this.labelFolder.Size = new System.Drawing.Size(96, 59);
            this.labelFolder.TabIndex = 2;
            this.labelFolder.Text = "Defect Folder";
            this.labelFolder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutFilter
            // 
            this.layoutFilter.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutFilter.ColumnCount = 1;
            this.layoutFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFilter.Controls.Add(this.layoutSelectFilter, 0, 1);
            this.layoutFilter.Controls.Add(this.labelFilterTitle, 0, 0);
            this.layoutFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutFilter.Location = new System.Drawing.Point(0, 0);
            this.layoutFilter.Margin = new System.Windows.Forms.Padding(0);
            this.layoutFilter.Name = "layoutFilter";
            this.layoutFilter.RowCount = 2;
            this.layoutFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutFilter.Size = new System.Drawing.Size(1106, 125);
            this.layoutFilter.TabIndex = 108;
            // 
            // layoutSelectFilter
            // 
            this.layoutSelectFilter.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutSelectFilter.ColumnCount = 2;
            this.layoutSelectFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutSelectFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSelectFilter.Controls.Add(this.layoutType, 1, 0);
            this.layoutSelectFilter.Controls.Add(this.layoutSize, 1, 1);
            this.layoutSelectFilter.Controls.Add(this.labelSize, 0, 1);
            this.layoutSelectFilter.Controls.Add(this.labelType, 0, 0);
            this.layoutSelectFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSelectFilter.Location = new System.Drawing.Point(2, 39);
            this.layoutSelectFilter.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSelectFilter.Name = "layoutSelectFilter";
            this.layoutSelectFilter.RowCount = 2;
            this.layoutSelectFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutSelectFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutSelectFilter.Size = new System.Drawing.Size(1102, 84);
            this.layoutSelectFilter.TabIndex = 2;
            // 
            // layoutType
            // 
            this.layoutType.ColumnCount = 8;
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutType.Controls.Add(this.shape, 6, 0);
            this.layoutType.Controls.Add(this.pinHole, 5, 0);
            this.layoutType.Controls.Add(this.dielectric, 4, 0);
            this.layoutType.Controls.Add(this.poleCircle, 3, 0);
            this.layoutType.Controls.Add(this.poleLine, 2, 0);
            this.layoutType.Controls.Add(this.sheetAttack, 1, 0);
            this.layoutType.Controls.Add(this.total, 0, 0);
            this.layoutType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutType.Location = new System.Drawing.Point(104, 2);
            this.layoutType.Margin = new System.Windows.Forms.Padding(0);
            this.layoutType.Name = "layoutType";
            this.layoutType.RowCount = 1;
            this.layoutType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutType.Size = new System.Drawing.Size(996, 39);
            this.layoutType.TabIndex = 86;
            // 
            // shape
            // 
            this.shape.Appearance = System.Windows.Forms.Appearance.Button;
            this.shape.AutoSize = true;
            this.shape.BackColor = System.Drawing.Color.Transparent;
            this.shape.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shape.FlatAppearance.BorderSize = 0;
            this.shape.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.shape.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.shape.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.shape.ForeColor = System.Drawing.Color.DarkGreen;
            this.shape.Location = new System.Drawing.Point(825, 0);
            this.shape.Margin = new System.Windows.Forms.Padding(0);
            this.shape.Name = "shape";
            this.shape.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.shape.Size = new System.Drawing.Size(100, 39);
            this.shape.TabIndex = 84;
            this.shape.Text = "Shape";
            this.shape.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.shape.UseVisualStyleBackColor = false;
            this.shape.CheckedChanged += new System.EventHandler(this.shape_CheckedChanged);
            // 
            // pinHole
            // 
            this.pinHole.Appearance = System.Windows.Forms.Appearance.Button;
            this.pinHole.AutoSize = true;
            this.pinHole.BackColor = System.Drawing.Color.Transparent;
            this.pinHole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pinHole.FlatAppearance.BorderSize = 0;
            this.pinHole.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.pinHole.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.pinHole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pinHole.ForeColor = System.Drawing.Color.DarkMagenta;
            this.pinHole.Location = new System.Drawing.Point(700, 0);
            this.pinHole.Margin = new System.Windows.Forms.Padding(0);
            this.pinHole.Name = "pinHole";
            this.pinHole.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.pinHole.Size = new System.Drawing.Size(125, 39);
            this.pinHole.TabIndex = 83;
            this.pinHole.Text = "Pin Hole";
            this.pinHole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.pinHole.UseVisualStyleBackColor = false;
            this.pinHole.CheckedChanged += new System.EventHandler(this.pinHole_CheckedChanged);
            // 
            // dielectric
            // 
            this.dielectric.Appearance = System.Windows.Forms.Appearance.Button;
            this.dielectric.AutoSize = true;
            this.dielectric.BackColor = System.Drawing.Color.Transparent;
            this.dielectric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dielectric.FlatAppearance.BorderSize = 0;
            this.dielectric.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.dielectric.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.dielectric.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.dielectric.ForeColor = System.Drawing.Color.Blue;
            this.dielectric.Location = new System.Drawing.Point(575, 0);
            this.dielectric.Margin = new System.Windows.Forms.Padding(0);
            this.dielectric.Name = "dielectric";
            this.dielectric.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.dielectric.Size = new System.Drawing.Size(125, 39);
            this.dielectric.TabIndex = 0;
            this.dielectric.Text = "Dielectric";
            this.dielectric.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dielectric.UseVisualStyleBackColor = false;
            this.dielectric.CheckedChanged += new System.EventHandler(this.dielectric_CheckedChanged);
            // 
            // poleCircle
            // 
            this.poleCircle.Appearance = System.Windows.Forms.Appearance.Button;
            this.poleCircle.AutoSize = true;
            this.poleCircle.BackColor = System.Drawing.Color.Transparent;
            this.poleCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.poleCircle.FlatAppearance.BorderSize = 0;
            this.poleCircle.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.poleCircle.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.poleCircle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.poleCircle.ForeColor = System.Drawing.Color.OrangeRed;
            this.poleCircle.Location = new System.Drawing.Point(425, 0);
            this.poleCircle.Margin = new System.Windows.Forms.Padding(0);
            this.poleCircle.Name = "poleCircle";
            this.poleCircle.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.poleCircle.Size = new System.Drawing.Size(150, 39);
            this.poleCircle.TabIndex = 85;
            this.poleCircle.Text = "Pole (Circle)";
            this.poleCircle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.poleCircle.UseVisualStyleBackColor = false;
            this.poleCircle.CheckedChanged += new System.EventHandler(this.poleCircle_CheckedChanged);
            // 
            // poleLine
            // 
            this.poleLine.Appearance = System.Windows.Forms.Appearance.Button;
            this.poleLine.AutoSize = true;
            this.poleLine.BackColor = System.Drawing.Color.Transparent;
            this.poleLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.poleLine.FlatAppearance.BorderSize = 0;
            this.poleLine.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.poleLine.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.poleLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.poleLine.ForeColor = System.Drawing.Color.Red;
            this.poleLine.Location = new System.Drawing.Point(275, 0);
            this.poleLine.Margin = new System.Windows.Forms.Padding(0);
            this.poleLine.Name = "poleLine";
            this.poleLine.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.poleLine.Size = new System.Drawing.Size(150, 39);
            this.poleLine.TabIndex = 81;
            this.poleLine.Text = "Pole (Line)";
            this.poleLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.poleLine.UseVisualStyleBackColor = false;
            this.poleLine.CheckedChanged += new System.EventHandler(this.poleLine_CheckedChanged);
            // 
            // sheetAttack
            // 
            this.sheetAttack.Appearance = System.Windows.Forms.Appearance.Button;
            this.sheetAttack.AutoSize = true;
            this.sheetAttack.BackColor = System.Drawing.Color.Transparent;
            this.sheetAttack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetAttack.FlatAppearance.BorderSize = 0;
            this.sheetAttack.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.sheetAttack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.sheetAttack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sheetAttack.ForeColor = System.Drawing.Color.Maroon;
            this.sheetAttack.Location = new System.Drawing.Point(125, 0);
            this.sheetAttack.Margin = new System.Windows.Forms.Padding(0);
            this.sheetAttack.Name = "sheetAttack";
            this.sheetAttack.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.sheetAttack.Size = new System.Drawing.Size(150, 39);
            this.sheetAttack.TabIndex = 86;
            this.sheetAttack.Text = "Sheet Attack";
            this.sheetAttack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.sheetAttack.UseVisualStyleBackColor = false;
            this.sheetAttack.CheckedChanged += new System.EventHandler(this.sheetAttack_CheckedChanged);
            // 
            // total
            // 
            this.total.Appearance = System.Windows.Forms.Appearance.Button;
            this.total.BackColor = System.Drawing.Color.Transparent;
            this.total.Checked = true;
            this.total.Dock = System.Windows.Forms.DockStyle.Fill;
            this.total.FlatAppearance.BorderSize = 0;
            this.total.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.total.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.total.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.total.Location = new System.Drawing.Point(0, 0);
            this.total.Margin = new System.Windows.Forms.Padding(0);
            this.total.Name = "total";
            this.total.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.total.Size = new System.Drawing.Size(125, 39);
            this.total.TabIndex = 81;
            this.total.TabStop = true;
            this.total.Text = "Total";
            this.total.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.total.UseVisualStyleBackColor = false;
            this.total.CheckedChanged += new System.EventHandler(this.total_CheckedChanged);
            // 
            // layoutSize
            // 
            this.layoutSize.ColumnCount = 8;
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSize.Controls.Add(this.labelMaxUnit, 6, 0);
            this.layoutSize.Controls.Add(this.sizeMax, 5, 0);
            this.layoutSize.Controls.Add(this.labelMinUnit, 3, 0);
            this.layoutSize.Controls.Add(this.labelMax, 4, 0);
            this.layoutSize.Controls.Add(this.sizeMin, 2, 0);
            this.layoutSize.Controls.Add(this.labelMin, 1, 0);
            this.layoutSize.Controls.Add(this.useSize, 0, 0);
            this.layoutSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSize.Location = new System.Drawing.Point(104, 43);
            this.layoutSize.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSize.Name = "layoutSize";
            this.layoutSize.RowCount = 1;
            this.layoutSize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSize.Size = new System.Drawing.Size(996, 39);
            this.layoutSize.TabIndex = 67;
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.BackColor = System.Drawing.Color.Transparent;
            this.labelMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMin.Location = new System.Drawing.Point(50, 0);
            this.labelMin.Margin = new System.Windows.Forms.Padding(0);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(60, 39);
            this.labelMin.TabIndex = 73;
            this.labelMin.Text = "Min";
            this.labelMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMaxUnit
            // 
            this.labelMaxUnit.AutoSize = true;
            this.labelMaxUnit.BackColor = System.Drawing.Color.Transparent;
            this.labelMaxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaxUnit.Location = new System.Drawing.Point(420, 0);
            this.labelMaxUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelMaxUnit.Name = "labelMaxUnit";
            this.labelMaxUnit.Size = new System.Drawing.Size(60, 39);
            this.labelMaxUnit.TabIndex = 0;
            this.labelMaxUnit.Text = "(um)";
            this.labelMaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sizeMax
            // 
            this.sizeMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sizeMax.Enabled = false;
            this.sizeMax.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.sizeMax.Location = new System.Drawing.Point(320, 5);
            this.sizeMax.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.sizeMax.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.sizeMax.Name = "sizeMax";
            this.sizeMax.Size = new System.Drawing.Size(100, 29);
            this.sizeMax.TabIndex = 0;
            this.sizeMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.sizeMax.ValueChanged += new System.EventHandler(this.sizeMax_ValueChanged);
            // 
            // labelMinUnit
            // 
            this.labelMinUnit.AutoSize = true;
            this.labelMinUnit.BackColor = System.Drawing.Color.Transparent;
            this.labelMinUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMinUnit.Location = new System.Drawing.Point(210, 0);
            this.labelMinUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelMinUnit.Name = "labelMinUnit";
            this.labelMinUnit.Size = new System.Drawing.Size(50, 39);
            this.labelMinUnit.TabIndex = 85;
            this.labelMinUnit.Text = "~";
            this.labelMinUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMax
            // 
            this.labelMax.AutoSize = true;
            this.labelMax.BackColor = System.Drawing.Color.Transparent;
            this.labelMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMax.Location = new System.Drawing.Point(260, 0);
            this.labelMax.Margin = new System.Windows.Forms.Padding(0);
            this.labelMax.Name = "labelMax";
            this.labelMax.Size = new System.Drawing.Size(60, 39);
            this.labelMax.TabIndex = 84;
            this.labelMax.Text = "Max";
            this.labelMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sizeMin
            // 
            this.sizeMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sizeMin.Enabled = false;
            this.sizeMin.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.sizeMin.Location = new System.Drawing.Point(110, 5);
            this.sizeMin.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.sizeMin.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.sizeMin.Name = "sizeMin";
            this.sizeMin.Size = new System.Drawing.Size(100, 29);
            this.sizeMin.TabIndex = 69;
            this.sizeMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.sizeMin.ValueChanged += new System.EventHandler(this.sizeMin_ValueChanged);
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSize.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelSize.Location = new System.Drawing.Point(2, 43);
            this.labelSize.Margin = new System.Windows.Forms.Padding(0);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(100, 39);
            this.labelSize.TabIndex = 0;
            this.labelSize.Text = "Size";
            this.labelSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelType.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelType.Location = new System.Drawing.Point(2, 2);
            this.labelType.Margin = new System.Windows.Forms.Padding(0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(100, 39);
            this.labelType.TabIndex = 0;
            this.labelType.Text = "Type";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFilterTitle
            // 
            this.labelFilterTitle.AutoSize = true;
            this.labelFilterTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelFilterTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFilterTitle.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelFilterTitle.Location = new System.Drawing.Point(2, 2);
            this.labelFilterTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelFilterTitle.Name = "labelFilterTitle";
            this.labelFilterTitle.Size = new System.Drawing.Size(1102, 35);
            this.labelFilterTitle.TabIndex = 1;
            this.labelFilterTitle.Text = "Filter";
            this.labelFilterTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutbottom
            // 
            this.layoutbottom.ColumnCount = 3;
            this.layoutbottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.layoutbottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.layoutbottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutbottom.Controls.Add(this.layoutImage, 0, 0);
            this.layoutbottom.Controls.Add(this.layoutDefectList, 0, 0);
            this.layoutbottom.Controls.Add(this.layoutSheetList, 0, 0);
            this.layoutbottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutbottom.Location = new System.Drawing.Point(0, 125);
            this.layoutbottom.Margin = new System.Windows.Forms.Padding(0);
            this.layoutbottom.Name = "layoutbottom";
            this.layoutbottom.RowCount = 1;
            this.layoutbottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutbottom.Size = new System.Drawing.Size(1206, 609);
            this.layoutbottom.TabIndex = 144;
            // 
            // layoutImage
            // 
            this.layoutImage.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutImage.ColumnCount = 1;
            this.layoutImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutImage.Controls.Add(this.layoutDefectNum, 0, 2);
            this.layoutImage.Controls.Add(this.labelImage, 0, 0);
            this.layoutImage.Controls.Add(this.imagePanel, 0, 1);
            this.layoutImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutImage.Location = new System.Drawing.Point(500, 0);
            this.layoutImage.Margin = new System.Windows.Forms.Padding(0);
            this.layoutImage.Name = "layoutImage";
            this.layoutImage.RowCount = 3;
            this.layoutImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.layoutImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutImage.Size = new System.Drawing.Size(706, 609);
            this.layoutImage.TabIndex = 90;
            // 
            // layoutDefectNum
            // 
            this.layoutDefectNum.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutDefectNum.ColumnCount = 6;
            this.layoutDefectNum.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutDefectNum.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutDefectNum.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutDefectNum.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutDefectNum.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutDefectNum.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutDefectNum.Controls.Add(this.poleLineNum, 3, 0);
            this.layoutDefectNum.Controls.Add(this.pinHoleNum, 3, 1);
            this.layoutDefectNum.Controls.Add(this.poleCircleNum, 5, 0);
            this.layoutDefectNum.Controls.Add(this.shapeNum, 5, 1);
            this.layoutDefectNum.Controls.Add(this.dielectricNum, 1, 1);
            this.layoutDefectNum.Controls.Add(this.sheetAttackNum, 1, 0);
            this.layoutDefectNum.Controls.Add(this.labelSheetAttack, 0, 0);
            this.layoutDefectNum.Controls.Add(this.labelDielectric, 0, 1);
            this.layoutDefectNum.Controls.Add(this.labelShape, 4, 1);
            this.layoutDefectNum.Controls.Add(this.labelPinHole, 2, 1);
            this.layoutDefectNum.Controls.Add(this.labelPoleLine, 2, 0);
            this.layoutDefectNum.Controls.Add(this.labelPoleCircle, 4, 0);
            this.layoutDefectNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutDefectNum.Location = new System.Drawing.Point(2, 552);
            this.layoutDefectNum.Margin = new System.Windows.Forms.Padding(0);
            this.layoutDefectNum.Name = "layoutDefectNum";
            this.layoutDefectNum.RowCount = 2;
            this.layoutDefectNum.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutDefectNum.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutDefectNum.Size = new System.Drawing.Size(702, 55);
            this.layoutDefectNum.TabIndex = 66;
            // 
            // poleLineNum
            // 
            this.poleLineNum.AutoSize = true;
            this.poleLineNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.poleLineNum.Location = new System.Drawing.Point(387, 2);
            this.poleLineNum.Margin = new System.Windows.Forms.Padding(0);
            this.poleLineNum.Name = "poleLineNum";
            this.poleLineNum.Size = new System.Drawing.Size(79, 24);
            this.poleLineNum.TabIndex = 71;
            this.poleLineNum.Text = "0";
            this.poleLineNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pinHoleNum
            // 
            this.pinHoleNum.AutoSize = true;
            this.pinHoleNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pinHoleNum.Location = new System.Drawing.Point(387, 28);
            this.pinHoleNum.Margin = new System.Windows.Forms.Padding(0);
            this.pinHoleNum.Name = "pinHoleNum";
            this.pinHoleNum.Size = new System.Drawing.Size(79, 25);
            this.pinHoleNum.TabIndex = 73;
            this.pinHoleNum.Text = "0";
            this.pinHoleNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // poleCircleNum
            // 
            this.poleCircleNum.AutoSize = true;
            this.poleCircleNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.poleCircleNum.Location = new System.Drawing.Point(620, 2);
            this.poleCircleNum.Margin = new System.Windows.Forms.Padding(0);
            this.poleCircleNum.Name = "poleCircleNum";
            this.poleCircleNum.Size = new System.Drawing.Size(80, 24);
            this.poleCircleNum.TabIndex = 68;
            this.poleCircleNum.Text = "0";
            this.poleCircleNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // shapeNum
            // 
            this.shapeNum.AutoSize = true;
            this.shapeNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shapeNum.Location = new System.Drawing.Point(620, 28);
            this.shapeNum.Margin = new System.Windows.Forms.Padding(0);
            this.shapeNum.Name = "shapeNum";
            this.shapeNum.Size = new System.Drawing.Size(80, 25);
            this.shapeNum.TabIndex = 72;
            this.shapeNum.Text = "0";
            this.shapeNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dielectricNum
            // 
            this.dielectricNum.AutoSize = true;
            this.dielectricNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dielectricNum.Location = new System.Drawing.Point(154, 28);
            this.dielectricNum.Margin = new System.Windows.Forms.Padding(0);
            this.dielectricNum.Name = "dielectricNum";
            this.dielectricNum.Size = new System.Drawing.Size(79, 25);
            this.dielectricNum.TabIndex = 70;
            this.dielectricNum.Text = "0";
            this.dielectricNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetAttackNum
            // 
            this.sheetAttackNum.AutoSize = true;
            this.sheetAttackNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetAttackNum.Location = new System.Drawing.Point(154, 2);
            this.sheetAttackNum.Margin = new System.Windows.Forms.Padding(0);
            this.sheetAttackNum.Name = "sheetAttackNum";
            this.sheetAttackNum.Size = new System.Drawing.Size(79, 24);
            this.sheetAttackNum.TabIndex = 66;
            this.sheetAttackNum.Text = "0";
            this.sheetAttackNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetAttack
            // 
            appearance10.FontData.Name = "Malgun Gothic";
            appearance10.FontData.SizeInPoints = 12F;
            appearance10.ForeColor = System.Drawing.Color.Maroon;
            appearance10.TextHAlignAsString = "Center";
            appearance10.TextVAlignAsString = "Middle";
            this.labelSheetAttack.Appearance = appearance10;
            this.labelSheetAttack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetAttack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelSheetAttack.Location = new System.Drawing.Point(2, 2);
            this.labelSheetAttack.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetAttack.Name = "labelSheetAttack";
            this.labelSheetAttack.Size = new System.Drawing.Size(150, 24);
            this.labelSheetAttack.TabIndex = 66;
            this.labelSheetAttack.Text = "Sheet Attack";
            // 
            // labelDielectric
            // 
            appearance11.FontData.Name = "Malgun Gothic";
            appearance11.FontData.SizeInPoints = 12F;
            appearance11.ForeColor = System.Drawing.Color.Blue;
            appearance11.TextHAlignAsString = "Center";
            appearance11.TextVAlignAsString = "Middle";
            this.labelDielectric.Appearance = appearance11;
            this.labelDielectric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDielectric.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelDielectric.Location = new System.Drawing.Point(2, 28);
            this.labelDielectric.Margin = new System.Windows.Forms.Padding(0);
            this.labelDielectric.Name = "labelDielectric";
            this.labelDielectric.Size = new System.Drawing.Size(150, 25);
            this.labelDielectric.TabIndex = 62;
            this.labelDielectric.Text = "Dielectric";
            // 
            // labelShape
            // 
            appearance12.FontData.Name = "Malgun Gothic";
            appearance12.FontData.SizeInPoints = 12F;
            appearance12.ForeColor = System.Drawing.Color.DarkGreen;
            appearance12.TextHAlignAsString = "Center";
            appearance12.TextVAlignAsString = "Middle";
            this.labelShape.Appearance = appearance12;
            this.labelShape.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelShape.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelShape.Location = new System.Drawing.Point(468, 28);
            this.labelShape.Margin = new System.Windows.Forms.Padding(0);
            this.labelShape.Name = "labelShape";
            this.labelShape.Size = new System.Drawing.Size(150, 25);
            this.labelShape.TabIndex = 64;
            this.labelShape.Text = "Shape";
            // 
            // labelPinHole
            // 
            appearance13.FontData.Name = "Malgun Gothic";
            appearance13.FontData.SizeInPoints = 12F;
            appearance13.ForeColor = System.Drawing.Color.DarkMagenta;
            appearance13.TextHAlignAsString = "Center";
            appearance13.TextVAlignAsString = "Middle";
            this.labelPinHole.Appearance = appearance13;
            this.labelPinHole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPinHole.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelPinHole.Location = new System.Drawing.Point(235, 28);
            this.labelPinHole.Margin = new System.Windows.Forms.Padding(0);
            this.labelPinHole.Name = "labelPinHole";
            this.labelPinHole.Size = new System.Drawing.Size(150, 25);
            this.labelPinHole.TabIndex = 65;
            this.labelPinHole.Text = "Pin Hole";
            // 
            // labelPoleLine
            // 
            appearance14.FontData.Name = "Malgun Gothic";
            appearance14.FontData.SizeInPoints = 12F;
            appearance14.ForeColor = System.Drawing.Color.Red;
            appearance14.TextHAlignAsString = "Center";
            appearance14.TextVAlignAsString = "Middle";
            this.labelPoleLine.Appearance = appearance14;
            this.labelPoleLine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPoleLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelPoleLine.Location = new System.Drawing.Point(235, 2);
            this.labelPoleLine.Margin = new System.Windows.Forms.Padding(0);
            this.labelPoleLine.Name = "labelPoleLine";
            this.labelPoleLine.Size = new System.Drawing.Size(150, 24);
            this.labelPoleLine.TabIndex = 67;
            this.labelPoleLine.Text = "Pole (Line)";
            // 
            // labelPoleCircle
            // 
            appearance15.FontData.Name = "Malgun Gothic";
            appearance15.FontData.SizeInPoints = 12F;
            appearance15.ForeColor = System.Drawing.Color.OrangeRed;
            appearance15.TextHAlignAsString = "Center";
            appearance15.TextVAlignAsString = "Middle";
            this.labelPoleCircle.Appearance = appearance15;
            this.labelPoleCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPoleCircle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelPoleCircle.Location = new System.Drawing.Point(468, 2);
            this.labelPoleCircle.Margin = new System.Windows.Forms.Padding(0);
            this.labelPoleCircle.Name = "labelPoleCircle";
            this.labelPoleCircle.Size = new System.Drawing.Size(150, 24);
            this.labelPoleCircle.TabIndex = 63;
            this.labelPoleCircle.Text = "Pole (Circle)";
            // 
            // labelImage
            // 
            this.labelImage.AutoSize = true;
            this.labelImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImage.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelImage.Location = new System.Drawing.Point(2, 2);
            this.labelImage.Margin = new System.Windows.Forms.Padding(0);
            this.labelImage.Name = "labelImage";
            this.labelImage.Size = new System.Drawing.Size(702, 35);
            this.labelImage.TabIndex = 53;
            this.labelImage.Text = "Image";
            this.labelImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imagePanel
            // 
            this.imagePanel.BackColor = System.Drawing.SystemColors.Control;
            this.imagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagePanel.Location = new System.Drawing.Point(2, 39);
            this.imagePanel.Margin = new System.Windows.Forms.Padding(0);
            this.imagePanel.Name = "imagePanel";
            this.imagePanel.Size = new System.Drawing.Size(702, 511);
            this.imagePanel.TabIndex = 0;
            // 
            // layoutDefectList
            // 
            this.layoutDefectList.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutDefectList.ColumnCount = 1;
            this.layoutDefectList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutDefectList.Controls.Add(this.labelDefectImage, 0, 2);
            this.layoutDefectList.Controls.Add(this.labelDefectList, 0, 0);
            this.layoutDefectList.Controls.Add(this.defectList, 0, 1);
            this.layoutDefectList.Controls.Add(this.defectImage, 0, 3);
            this.layoutDefectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutDefectList.Location = new System.Drawing.Point(250, 0);
            this.layoutDefectList.Margin = new System.Windows.Forms.Padding(0);
            this.layoutDefectList.Name = "layoutDefectList";
            this.layoutDefectList.RowCount = 4;
            this.layoutDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.layoutDefectList.Size = new System.Drawing.Size(250, 609);
            this.layoutDefectList.TabIndex = 88;
            // 
            // labelDefectImage
            // 
            this.labelDefectImage.AutoSize = true;
            this.labelDefectImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.labelDefectImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefectImage.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelDefectImage.Location = new System.Drawing.Point(2, 370);
            this.labelDefectImage.Margin = new System.Windows.Forms.Padding(0);
            this.labelDefectImage.Name = "labelDefectImage";
            this.labelDefectImage.Size = new System.Drawing.Size(246, 35);
            this.labelDefectImage.TabIndex = 64;
            this.labelDefectImage.Text = "Defect";
            this.labelDefectImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDefectList
            // 
            this.labelDefectList.AutoSize = true;
            this.labelDefectList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelDefectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefectList.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelDefectList.Location = new System.Drawing.Point(2, 2);
            this.labelDefectList.Margin = new System.Windows.Forms.Padding(0);
            this.labelDefectList.Name = "labelDefectList";
            this.labelDefectList.Size = new System.Drawing.Size(246, 35);
            this.labelDefectList.TabIndex = 0;
            this.labelDefectList.Text = "Defect List";
            this.labelDefectList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // defectList
            // 
            this.defectList.AllowUserToAddRows = false;
            this.defectList.AllowUserToDeleteRows = false;
            this.defectList.AllowUserToResizeColumns = false;
            this.defectList.AllowUserToResizeRows = false;
            this.defectList.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.defectList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.defectList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.defectList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnNo,
            this.columnDefectType});
            this.defectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.defectList.Location = new System.Drawing.Point(2, 39);
            this.defectList.Margin = new System.Windows.Forms.Padding(0);
            this.defectList.MultiSelect = false;
            this.defectList.Name = "defectList";
            dataGridViewCellStyle8.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.defectList.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.defectList.RowHeadersVisible = false;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.defectList.RowsDefaultCellStyle = dataGridViewCellStyle9;
            this.defectList.RowTemplate.Height = 23;
            this.defectList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.defectList.Size = new System.Drawing.Size(246, 329);
            this.defectList.TabIndex = 0;
            this.defectList.SelectionChanged += new System.EventHandler(this.defectList_SelectionChanged);
            this.defectList.Click += new System.EventHandler(this.defectList_Click);
            // 
            // columnNo
            // 
            this.columnNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnNo.FillWeight = 65.9137F;
            this.columnNo.HeaderText = "No.";
            this.columnNo.Name = "columnNo";
            this.columnNo.Width = 62;
            // 
            // columnDefectType
            // 
            this.columnDefectType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnDefectType.FillWeight = 139.0863F;
            this.columnDefectType.HeaderText = "Type";
            this.columnDefectType.Name = "columnDefectType";
            // 
            // defectImage
            // 
            this.defectImage.BackColor = System.Drawing.SystemColors.Control;
            this.defectImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.defectImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectImage.Location = new System.Drawing.Point(2, 407);
            this.defectImage.Margin = new System.Windows.Forms.Padding(0);
            this.defectImage.Name = "defectImage";
            this.defectImage.Size = new System.Drawing.Size(246, 200);
            this.defectImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.defectImage.TabIndex = 65;
            this.defectImage.TabStop = false;
            // 
            // layoutSheetList
            // 
            this.layoutSheetList.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutSheetList.ColumnCount = 1;
            this.layoutSheetList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSheetList.Controls.Add(this.sheetList, 0, 3);
            this.layoutSheetList.Controls.Add(this.labelInfo, 0, 0);
            this.layoutSheetList.Controls.Add(this.buttonSelectAll, 0, 5);
            this.layoutSheetList.Controls.Add(this.labelSheetList, 0, 2);
            this.layoutSheetList.Controls.Add(this.layoutResultFilter, 0, 4);
            this.layoutSheetList.Controls.Add(this.layoutInfo, 0, 1);
            this.layoutSheetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSheetList.Location = new System.Drawing.Point(0, 0);
            this.layoutSheetList.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSheetList.Name = "layoutSheetList";
            this.layoutSheetList.RowCount = 6;
            this.layoutSheetList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutSheetList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.layoutSheetList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutSheetList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSheetList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutSheetList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutSheetList.Size = new System.Drawing.Size(250, 609);
            this.layoutSheetList.TabIndex = 0;
            // 
            // sheetList
            // 
            this.sheetList.AllowUserToAddRows = false;
            this.sheetList.AllowUserToDeleteRows = false;
            this.sheetList.AllowUserToResizeColumns = false;
            this.sheetList.AllowUserToResizeRows = false;
            this.sheetList.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sheetList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.sheetList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sheetList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnPattern,
            this.columnQty});
            this.sheetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.sheetList.Location = new System.Drawing.Point(2, 168);
            this.sheetList.Margin = new System.Windows.Forms.Padding(0);
            this.sheetList.MultiSelect = false;
            this.sheetList.Name = "sheetList";
            dataGridViewCellStyle11.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.sheetList.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.sheetList.RowHeadersVisible = false;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.sheetList.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.sheetList.RowTemplate.Height = 23;
            this.sheetList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.sheetList.Size = new System.Drawing.Size(246, 365);
            this.sheetList.TabIndex = 1;
            this.sheetList.SelectionChanged += new System.EventHandler(this.sheetList_SelectionChanged);
            this.sheetList.Click += new System.EventHandler(this.sheetList_Click);
            // 
            // columnPattern
            // 
            this.columnPattern.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnPattern.FillWeight = 65.9137F;
            this.columnPattern.HeaderText = "Pattern";
            this.columnPattern.Name = "columnPattern";
            this.columnPattern.Width = 91;
            // 
            // columnQty
            // 
            this.columnQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnQty.FillWeight = 139.0863F;
            this.columnQty.HeaderText = "Qty.";
            this.columnQty.Name = "columnQty";
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelInfo.Location = new System.Drawing.Point(2, 2);
            this.labelInfo.Margin = new System.Windows.Forms.Padding(0);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(246, 35);
            this.labelInfo.TabIndex = 69;
            this.labelInfo.Text = "Info";
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSelectAll
            // 
            appearance16.BackColor = System.Drawing.Color.White;
            this.buttonSelectAll.Appearance = appearance16;
            this.buttonSelectAll.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonSelectAll.Location = new System.Drawing.Point(2, 572);
            this.buttonSelectAll.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(246, 35);
            this.buttonSelectAll.TabIndex = 66;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonSelectAll.Visible = false;
            // 
            // labelSheetList
            // 
            this.labelSheetList.AutoSize = true;
            this.labelSheetList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelSheetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetList.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelSheetList.Location = new System.Drawing.Point(2, 131);
            this.labelSheetList.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetList.Name = "labelSheetList";
            this.labelSheetList.Size = new System.Drawing.Size(246, 35);
            this.labelSheetList.TabIndex = 0;
            this.labelSheetList.Text = "Sheet List";
            this.labelSheetList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutResultFilter
            // 
            this.layoutResultFilter.ColumnCount = 2;
            this.layoutResultFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutResultFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutResultFilter.Controls.Add(this.ngFilter, 1, 0);
            this.layoutResultFilter.Controls.Add(this.okFilter, 0, 0);
            this.layoutResultFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutResultFilter.Location = new System.Drawing.Point(2, 535);
            this.layoutResultFilter.Margin = new System.Windows.Forms.Padding(0);
            this.layoutResultFilter.Name = "layoutResultFilter";
            this.layoutResultFilter.RowCount = 1;
            this.layoutResultFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutResultFilter.Size = new System.Drawing.Size(246, 35);
            this.layoutResultFilter.TabIndex = 63;
            // 
            // ngFilter
            // 
            this.ngFilter.AutoSize = true;
            this.ngFilter.Checked = true;
            this.ngFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ngFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ngFilter.Location = new System.Drawing.Point(126, 3);
            this.ngFilter.Name = "ngFilter";
            this.ngFilter.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.ngFilter.Size = new System.Drawing.Size(117, 29);
            this.ngFilter.TabIndex = 1;
            this.ngFilter.Text = "NG";
            this.ngFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ngFilter.UseVisualStyleBackColor = true;
            this.ngFilter.CheckedChanged += new System.EventHandler(this.ngFilter_CheckedChanged);
            // 
            // okFilter
            // 
            this.okFilter.AutoSize = true;
            this.okFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.okFilter.Location = new System.Drawing.Point(3, 3);
            this.okFilter.Name = "okFilter";
            this.okFilter.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.okFilter.Size = new System.Drawing.Size(117, 29);
            this.okFilter.TabIndex = 0;
            this.okFilter.Text = "OK";
            this.okFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.okFilter.UseVisualStyleBackColor = true;
            this.okFilter.CheckedChanged += new System.EventHandler(this.okFilter_CheckedChanged);
            // 
            // layoutInfo
            // 
            this.layoutInfo.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutInfo.ColumnCount = 2;
            this.layoutInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 67F));
            this.layoutInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutInfo.Controls.Add(this.labelSheetTotal, 0, 1);
            this.layoutInfo.Controls.Add(this.labelSheetNG, 0, 2);
            this.layoutInfo.Controls.Add(this.sheetNG, 1, 2);
            this.layoutInfo.Controls.Add(this.sheetTotal, 1, 1);
            this.layoutInfo.Controls.Add(this.sheetRatio, 1, 3);
            this.layoutInfo.Controls.Add(this.labelSheetRatio, 0, 3);
            this.layoutInfo.Controls.Add(this.labelUnit, 1, 0);
            this.layoutInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutInfo.Location = new System.Drawing.Point(2, 39);
            this.layoutInfo.Margin = new System.Windows.Forms.Padding(0);
            this.layoutInfo.Name = "layoutInfo";
            this.layoutInfo.RowCount = 4;
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutInfo.Size = new System.Drawing.Size(246, 90);
            this.layoutInfo.TabIndex = 70;
            // 
            // labelSheetTotal
            // 
            this.labelSheetTotal.AutoSize = true;
            this.labelSheetTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetTotal.Location = new System.Drawing.Point(2, 23);
            this.labelSheetTotal.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetTotal.Name = "labelSheetTotal";
            this.labelSheetTotal.Size = new System.Drawing.Size(67, 20);
            this.labelSheetTotal.TabIndex = 1;
            this.labelSheetTotal.Text = "Total";
            this.labelSheetTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetNG
            // 
            this.labelSheetNG.AutoSize = true;
            this.labelSheetNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetNG.Location = new System.Drawing.Point(2, 45);
            this.labelSheetNG.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetNG.Name = "labelSheetNG";
            this.labelSheetNG.Size = new System.Drawing.Size(67, 20);
            this.labelSheetNG.TabIndex = 3;
            this.labelSheetNG.Text = "NG";
            this.labelSheetNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetNG
            // 
            this.sheetNG.AutoSize = true;
            this.sheetNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetNG.Location = new System.Drawing.Point(71, 45);
            this.sheetNG.Margin = new System.Windows.Forms.Padding(0);
            this.sheetNG.Name = "sheetNG";
            this.sheetNG.Size = new System.Drawing.Size(173, 20);
            this.sheetNG.TabIndex = 4;
            this.sheetNG.Text = "0";
            this.sheetNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetTotal
            // 
            this.sheetTotal.AutoSize = true;
            this.sheetTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetTotal.Location = new System.Drawing.Point(71, 23);
            this.sheetTotal.Margin = new System.Windows.Forms.Padding(0);
            this.sheetTotal.Name = "sheetTotal";
            this.sheetTotal.Size = new System.Drawing.Size(173, 20);
            this.sheetTotal.TabIndex = 6;
            this.sheetTotal.Text = "0";
            this.sheetTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetRatio
            // 
            this.sheetRatio.AutoSize = true;
            this.sheetRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetRatio.Location = new System.Drawing.Point(71, 67);
            this.sheetRatio.Margin = new System.Windows.Forms.Padding(0);
            this.sheetRatio.Name = "sheetRatio";
            this.sheetRatio.Size = new System.Drawing.Size(173, 21);
            this.sheetRatio.TabIndex = 2;
            this.sheetRatio.Text = "0";
            this.sheetRatio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetRatio
            // 
            this.labelSheetRatio.AutoSize = true;
            this.labelSheetRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetRatio.Location = new System.Drawing.Point(2, 67);
            this.labelSheetRatio.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetRatio.Name = "labelSheetRatio";
            this.labelSheetRatio.Size = new System.Drawing.Size(67, 21);
            this.labelSheetRatio.TabIndex = 0;
            this.labelSheetRatio.Text = "Ratio";
            this.labelSheetRatio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelUnit
            // 
            this.labelUnit.AutoSize = true;
            this.labelUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUnit.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.labelUnit.ForeColor = System.Drawing.Color.Red;
            this.labelUnit.Location = new System.Drawing.Point(74, 2);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(167, 19);
            this.labelUnit.TabIndex = 68;
            this.labelUnit.Text = "Unit : Print";
            this.labelUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // useSize
            // 
            this.useSize.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.useSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.useSize.Location = new System.Drawing.Point(0, 0);
            this.useSize.Margin = new System.Windows.Forms.Padding(0);
            this.useSize.Name = "useSize";
            this.useSize.Size = new System.Drawing.Size(50, 39);
            this.useSize.TabIndex = 99;
            this.useSize.UseVisualStyleBackColor = true;
            this.useSize.CheckedChanged += new System.EventHandler(this.useSize_CheckedChanged);
            // 
            // ReportPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.layoutMain);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Name = "ReportPanel";
            this.Size = new System.Drawing.Size(1206, 734);
            this.layoutMain.ResumeLayout(false);
            this.layoutTop.ResumeLayout(false);
            this.layoutFolder.ResumeLayout(false);
            this.layoutFolder.PerformLayout();
            this.layoutFilter.ResumeLayout(false);
            this.layoutFilter.PerformLayout();
            this.layoutSelectFilter.ResumeLayout(false);
            this.layoutSelectFilter.PerformLayout();
            this.layoutType.ResumeLayout(false);
            this.layoutType.PerformLayout();
            this.layoutSize.ResumeLayout(false);
            this.layoutSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMin)).EndInit();
            this.layoutbottom.ResumeLayout(false);
            this.layoutImage.ResumeLayout(false);
            this.layoutImage.PerformLayout();
            this.layoutDefectNum.ResumeLayout(false);
            this.layoutDefectNum.PerformLayout();
            this.layoutDefectList.ResumeLayout(false);
            this.layoutDefectList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defectList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.defectImage)).EndInit();
            this.layoutSheetList.ResumeLayout(false);
            this.layoutSheetList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetList)).EndInit();
            this.layoutResultFilter.ResumeLayout(false);
            this.layoutResultFilter.PerformLayout();
            this.layoutInfo.ResumeLayout(false);
            this.layoutInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.TableLayoutPanel layoutbottom;
        private System.Windows.Forms.TableLayoutPanel layoutSheetList;
        private System.Windows.Forms.TableLayoutPanel layoutResultFilter;
        private System.Windows.Forms.CheckBox ngFilter;
        private System.Windows.Forms.CheckBox okFilter;
        private System.Windows.Forms.Label labelSheetList;
        private System.Windows.Forms.TableLayoutPanel layoutImage;
        private System.Windows.Forms.TableLayoutPanel layoutDefectNum;
        private Infragistics.Win.Misc.UltraLabel labelSheetAttack;
        private Infragistics.Win.Misc.UltraLabel labelDielectric;
        private Infragistics.Win.Misc.UltraLabel labelShape;
        private Infragistics.Win.Misc.UltraLabel labelPinHole;
        private Infragistics.Win.Misc.UltraLabel labelPoleCircle;
        private Infragistics.Win.Misc.UltraLabel labelPoleLine;
        private System.Windows.Forms.Label labelImage;
        private System.Windows.Forms.TableLayoutPanel layoutDefectList;
        private System.Windows.Forms.Label labelDefectImage;
        private System.Windows.Forms.Label labelDefectList;
        private System.Windows.Forms.DataGridView defectList;
        private System.Windows.Forms.PictureBox defectImage;
        private Infragistics.Win.Misc.UltraButton buttonSelectAll;
        private System.Windows.Forms.TableLayoutPanel layoutInfo;
        private System.Windows.Forms.Label labelSheetNG;
        private System.Windows.Forms.Label sheetRatio;
        private System.Windows.Forms.Label labelSheetTotal;
        private System.Windows.Forms.Label labelSheetRatio;
        private System.Windows.Forms.Label sheetNG;
        private System.Windows.Forms.Label sheetTotal;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.Label sheetAttackNum;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label shapeNum;
        private System.Windows.Forms.Label poleLineNum;
        private System.Windows.Forms.Label dielectricNum;
        private System.Windows.Forms.Label poleCircleNum;
        private System.Windows.Forms.Label pinHoleNum;
        private System.Windows.Forms.Panel imagePanel;
        private System.Windows.Forms.TableLayoutPanel layoutTop;
        private System.Windows.Forms.TableLayoutPanel layoutFolder;
        private Infragistics.Win.Misc.UltraButton buttonCam;
        private System.Windows.Forms.Label labelFolder;
        private System.Windows.Forms.TableLayoutPanel layoutFilter;
        private System.Windows.Forms.TableLayoutPanel layoutSelectFilter;
        private System.Windows.Forms.TableLayoutPanel layoutType;
        private System.Windows.Forms.RadioButton shape;
        private System.Windows.Forms.RadioButton pinHole;
        private System.Windows.Forms.RadioButton dielectric;
        private System.Windows.Forms.RadioButton poleCircle;
        private System.Windows.Forms.RadioButton poleLine;
        private System.Windows.Forms.RadioButton sheetAttack;
        private System.Windows.Forms.RadioButton total;
        private System.Windows.Forms.TableLayoutPanel layoutSize;
        private System.Windows.Forms.Label labelMin;
        private System.Windows.Forms.Label labelMaxUnit;
        private System.Windows.Forms.NumericUpDown sizeMax;
        private System.Windows.Forms.Label labelMinUnit;
        private System.Windows.Forms.Label labelMax;
        private System.Windows.Forms.NumericUpDown sizeMin;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.Label labelFilterTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDefectType;
        private System.Windows.Forms.DataGridView sheetList;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPattern;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnQty;
        private System.Windows.Forms.CheckBox useSize;
    }
}
