namespace UniScanG.Gravure.UI.Report
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
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.layoutTop = new System.Windows.Forms.TableLayoutPanel();
            this.layoutAdvance = new System.Windows.Forms.TableLayoutPanel();
            this.buttonExport = new Infragistics.Win.Misc.UltraButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.defectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lengthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.marginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.offsetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelExport = new System.Windows.Forms.Label();
            this.labelWindowCapture = new System.Windows.Forms.Label();
            this.buttonWindowCapture = new Infragistics.Win.Misc.UltraButton();
            this.layoutFilter = new System.Windows.Forms.TableLayoutPanel();
            this.layoutSelectFilter = new System.Windows.Forms.TableLayoutPanel();
            this.panelSelectCam = new System.Windows.Forms.Panel();
            this.checkBoxCam = new System.Windows.Forms.CheckBox();
            this.labelCam = new System.Windows.Forms.Label();
            this.layoutSize = new System.Windows.Forms.TableLayoutPanel();
            this.labelMaxUnit = new System.Windows.Forms.Label();
            this.sizeMax = new System.Windows.Forms.NumericUpDown();
            this.labelMinUnit = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.sizeMin = new System.Windows.Forms.NumericUpDown();
            this.labelMin = new System.Windows.Forms.Label();
            this.useSize = new System.Windows.Forms.CheckBox();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.labelFilterRect = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rectCY = new System.Windows.Forms.NumericUpDown();
            this.useRect = new System.Windows.Forms.CheckBox();
            this.rectCX = new System.Windows.Forms.NumericUpDown();
            this.rectH = new System.Windows.Forms.NumericUpDown();
            this.rectW = new System.Windows.Forms.NumericUpDown();
            this.panelSelectType = new System.Windows.Forms.Panel();
            this.labelFilterTitle = new System.Windows.Forms.Label();
            this.layoutbottom = new System.Windows.Forms.TableLayoutPanel();
            this.layoutLeft = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelSheetList = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelSheetInspector = new System.Windows.Forms.Label();
            this.sheetInspector = new System.Windows.Forms.Label();
            this.sheetRatio = new System.Windows.Forms.Label();
            this.labelSheetEraser = new System.Windows.Forms.Label();
            this.labelSheetRatio = new System.Windows.Forms.Label();
            this.sheetEraser = new System.Windows.Forms.Label();
            this.sheetList = new System.Windows.Forms.DataGridView();
            this.columnPattern = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnErased = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.labelPatternList = new System.Windows.Forms.Label();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.okFilter = new System.Windows.Forms.CheckBox();
            this.ngFilter = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelSheetLengthUnit = new System.Windows.Forms.Label();
            this.labelSheetLength = new System.Windows.Forms.Label();
            this.sheetLength = new System.Windows.Forms.TableLayoutPanel();
            this.labelSheetLengthLow10p = new System.Windows.Forms.Label();
            this.infoHeight1 = new System.Windows.Forms.Label();
            this.infoHeight2 = new System.Windows.Forms.Label();
            this.infoHeight3 = new System.Windows.Forms.Label();
            this.labelSheetLengthMid80p = new System.Windows.Forms.Label();
            this.labelSheetLengthHigh10p = new System.Windows.Forms.Label();
            this.tableLayoutPanelSheetLenDetail = new System.Windows.Forms.TableLayoutPanel();
            this.labelSheetLengthMean = new System.Windows.Forms.Label();
            this.infoHeightMean = new System.Windows.Forms.Label();
            this.labelSheetLengthStdDev = new System.Windows.Forms.Label();
            this.infoHeightStdDev = new System.Windows.Forms.Label();
            this.labelSheetLengthMax = new System.Windows.Forms.Label();
            this.infoHeightMax = new System.Windows.Forms.Label();
            this.labelSheetLengthMin = new System.Windows.Forms.Label();
            this.infoHeightMin = new System.Windows.Forms.Label();
            this.labelSheetLengthDiff = new System.Windows.Forms.Label();
            this.infoHeightRange = new System.Windows.Forms.Label();
            this.labelInfo = new System.Windows.Forms.Label();
            this.panelProductionTargetSpd = new System.Windows.Forms.Panel();
            this.labelProductionTargetSpdUnit = new System.Windows.Forms.Label();
            this.productionTargetSpd = new System.Windows.Forms.Label();
            this.labelProductionModelName = new System.Windows.Forms.Label();
            this.labelProductionLotName = new System.Windows.Forms.Label();
            this.labelProductionTime = new System.Windows.Forms.Label();
            this.labelProductionTargetSpd = new System.Windows.Forms.Label();
            this.productionModelName = new System.Windows.Forms.TextBox();
            this.productionTime = new System.Windows.Forms.TextBox();
            this.productionLotName = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelDefectList = new System.Windows.Forms.TableLayoutPanel();
            this.defectInfoPanel = new System.Windows.Forms.Panel();
            this.labelDefectsUnit = new System.Windows.Forms.Label();
            this.labelDefects = new System.Windows.Forms.Label();
            this.patternRadioButton = new System.Windows.Forms.RadioButton();
            this.defectRadioButton = new System.Windows.Forms.RadioButton();
            this.labelNoPrint = new System.Windows.Forms.Label();
            this.labelPinHole = new System.Windows.Forms.Label();
            this.sheetAttackNum = new System.Windows.Forms.Label();
            this.labelSheetAttack = new System.Windows.Forms.Label();
            this.labelSpread = new System.Windows.Forms.Label();
            this.labelDielectric = new System.Windows.Forms.Label();
            this.labelSticker = new System.Windows.Forms.Label();
            this.labelMargin = new System.Windows.Forms.Label();
            this.noPrintNum = new System.Windows.Forms.Label();
            this.pinHoleNum = new System.Windows.Forms.Label();
            this.spreadNum = new System.Windows.Forms.Label();
            this.marginNum = new System.Windows.Forms.Label();
            this.stickerNum = new System.Windows.Forms.Label();
            this.dielectricNum = new System.Windows.Forms.Label();
            this.panelResult = new System.Windows.Forms.Panel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ultraFlowLayoutManager1 = new Infragistics.Win.Misc.UltraFlowLayoutManager(this.components);
            this.partialProjectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutMain.SuspendLayout();
            this.layoutTop.SuspendLayout();
            this.layoutAdvance.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.layoutFilter.SuspendLayout();
            this.layoutSelectFilter.SuspendLayout();
            this.panelSelectCam.SuspendLayout();
            this.layoutSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMin)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rectCY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rectCX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rectH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rectW)).BeginInit();
            this.layoutbottom.SuspendLayout();
            this.layoutLeft.SuspendLayout();
            this.tableLayoutPanelSheetList.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetList)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.sheetLength.SuspendLayout();
            this.tableLayoutPanelSheetLenDetail.SuspendLayout();
            this.panelProductionTargetSpd.SuspendLayout();
            this.tableLayoutPanelDefectList.SuspendLayout();
            this.defectInfoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFlowLayoutManager1)).BeginInit();
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
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Size = new System.Drawing.Size(1206, 831);
            this.layoutMain.TabIndex = 0;
            // 
            // layoutTop
            // 
            this.layoutTop.ColumnCount = 2;
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutTop.Controls.Add(this.layoutAdvance, 1, 0);
            this.layoutTop.Controls.Add(this.layoutFilter, 0, 0);
            this.layoutTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutTop.Location = new System.Drawing.Point(0, 0);
            this.layoutTop.Margin = new System.Windows.Forms.Padding(0);
            this.layoutTop.Name = "layoutTop";
            this.layoutTop.RowCount = 1;
            this.layoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTop.Size = new System.Drawing.Size(1206, 150);
            this.layoutTop.TabIndex = 143;
            // 
            // layoutAdvance
            // 
            this.layoutAdvance.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutAdvance.ColumnCount = 2;
            this.layoutAdvance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutAdvance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutAdvance.Controls.Add(this.buttonExport, 1, 1);
            this.layoutAdvance.Controls.Add(this.labelExport, 1, 0);
            this.layoutAdvance.Controls.Add(this.labelWindowCapture, 0, 0);
            this.layoutAdvance.Controls.Add(this.buttonWindowCapture, 0, 1);
            this.layoutAdvance.Dock = System.Windows.Forms.DockStyle.Right;
            this.layoutAdvance.Location = new System.Drawing.Point(1046, 0);
            this.layoutAdvance.Margin = new System.Windows.Forms.Padding(0);
            this.layoutAdvance.Name = "layoutAdvance";
            this.layoutAdvance.RowCount = 2;
            this.layoutAdvance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.layoutAdvance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutAdvance.Size = new System.Drawing.Size(160, 150);
            this.layoutAdvance.TabIndex = 112;
            // 
            // buttonExport
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.Image = global::UniScanG.Properties.Resources.export;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.buttonExport.Appearance = appearance1;
            this.buttonExport.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonExport.ContextMenuStrip = this.contextMenuStrip1;
            this.buttonExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonExport.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonExport.ImageSize = new System.Drawing.Size(40, 40);
            this.buttonExport.Location = new System.Drawing.Point(81, 74);
            this.buttonExport.Margin = new System.Windows.Forms.Padding(0);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(77, 74);
            this.buttonExport.TabIndex = 0;
            this.buttonExport.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.defectsToolStripMenuItem,
            this.lengthToolStripMenuItem,
            this.marginToolStripMenuItem,
            this.offsetToolStripMenuItem,
            this.partialProjectionToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 136);
            // 
            // defectsToolStripMenuItem
            // 
            this.defectsToolStripMenuItem.Name = "defectsToolStripMenuItem";
            this.defectsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.defectsToolStripMenuItem.Text = "Defects";
            this.defectsToolStripMenuItem.Click += new System.EventHandler(this.buttonExportSheet_Click);
            // 
            // lengthToolStripMenuItem
            // 
            this.lengthToolStripMenuItem.Name = "lengthToolStripMenuItem";
            this.lengthToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.lengthToolStripMenuItem.Text = "Length";
            this.lengthToolStripMenuItem.Click += new System.EventHandler(this.buttonExportLength_Click);
            // 
            // marginToolStripMenuItem
            // 
            this.marginToolStripMenuItem.Name = "marginToolStripMenuItem";
            this.marginToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.marginToolStripMenuItem.Text = "Margin";
            this.marginToolStripMenuItem.Click += new System.EventHandler(this.buttonExportMargin_Click);
            // 
            // offsetToolStripMenuItem
            // 
            this.offsetToolStripMenuItem.Name = "offsetToolStripMenuItem";
            this.offsetToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.offsetToolStripMenuItem.Text = "Offset";
            this.offsetToolStripMenuItem.Click += new System.EventHandler(this.buttonExportOffset_Click);
            // 
            // labelExport
            // 
            this.labelExport.AutoSize = true;
            this.labelExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelExport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelExport.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelExport.Location = new System.Drawing.Point(81, 2);
            this.labelExport.Margin = new System.Windows.Forms.Padding(0);
            this.labelExport.Name = "labelExport";
            this.labelExport.Size = new System.Drawing.Size(77, 70);
            this.labelExport.TabIndex = 2;
            this.labelExport.Text = "Export";
            this.labelExport.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelWindowCapture
            // 
            this.labelWindowCapture.AutoSize = true;
            this.labelWindowCapture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelWindowCapture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWindowCapture.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelWindowCapture.Location = new System.Drawing.Point(2, 2);
            this.labelWindowCapture.Margin = new System.Windows.Forms.Padding(0);
            this.labelWindowCapture.Name = "labelWindowCapture";
            this.labelWindowCapture.Size = new System.Drawing.Size(77, 70);
            this.labelWindowCapture.TabIndex = 147;
            this.labelWindowCapture.Text = "Window Capture";
            this.labelWindowCapture.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonWindowCapture
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.Image = global::UniScanG.Properties.Resources.Cam;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.buttonWindowCapture.Appearance = appearance2;
            this.buttonWindowCapture.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonWindowCapture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonWindowCapture.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonWindowCapture.ImageSize = new System.Drawing.Size(40, 40);
            this.buttonWindowCapture.Location = new System.Drawing.Point(2, 74);
            this.buttonWindowCapture.Margin = new System.Windows.Forms.Padding(0);
            this.buttonWindowCapture.Name = "buttonWindowCapture";
            this.buttonWindowCapture.Size = new System.Drawing.Size(77, 74);
            this.buttonWindowCapture.TabIndex = 148;
            this.buttonWindowCapture.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonWindowCapture.Click += new System.EventHandler(this.buttonCapture_Click);
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
            this.layoutFilter.Size = new System.Drawing.Size(1046, 150);
            this.layoutFilter.TabIndex = 108;
            // 
            // layoutSelectFilter
            // 
            this.layoutSelectFilter.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.layoutSelectFilter.ColumnCount = 4;
            this.layoutSelectFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutSelectFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.layoutSelectFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutSelectFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.layoutSelectFilter.Controls.Add(this.panelSelectCam, 1, 0);
            this.layoutSelectFilter.Controls.Add(this.labelCam, 0, 0);
            this.layoutSelectFilter.Controls.Add(this.layoutSize, 1, 2);
            this.layoutSelectFilter.Controls.Add(this.labelSize, 0, 2);
            this.layoutSelectFilter.Controls.Add(this.labelType, 0, 1);
            this.layoutSelectFilter.Controls.Add(this.labelFilterRect, 2, 0);
            this.layoutSelectFilter.Controls.Add(this.tableLayoutPanel2, 3, 0);
            this.layoutSelectFilter.Controls.Add(this.panelSelectType, 1, 1);
            this.layoutSelectFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSelectFilter.Location = new System.Drawing.Point(2, 39);
            this.layoutSelectFilter.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSelectFilter.Name = "layoutSelectFilter";
            this.layoutSelectFilter.RowCount = 3;
            this.layoutSelectFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.7757F));
            this.layoutSelectFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.64486F));
            this.layoutSelectFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutSelectFilter.Size = new System.Drawing.Size(1042, 109);
            this.layoutSelectFilter.TabIndex = 2;
            // 
            // panelSelectCam
            // 
            this.panelSelectCam.Controls.Add(this.checkBoxCam);
            this.panelSelectCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSelectCam.Location = new System.Drawing.Point(104, 2);
            this.panelSelectCam.Margin = new System.Windows.Forms.Padding(0);
            this.panelSelectCam.Name = "panelSelectCam";
            this.panelSelectCam.Size = new System.Drawing.Size(499, 32);
            this.panelSelectCam.TabIndex = 88;
            // 
            // checkBoxCam
            // 
            this.checkBoxCam.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCam.AutoSize = true;
            this.checkBoxCam.Checked = true;
            this.checkBoxCam.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCam.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxCam.FlatAppearance.BorderSize = 0;
            this.checkBoxCam.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCam.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.checkBoxCam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxCam.Location = new System.Drawing.Point(0, 0);
            this.checkBoxCam.Margin = new System.Windows.Forms.Padding(0);
            this.checkBoxCam.Name = "checkBoxCam";
            this.checkBoxCam.Size = new System.Drawing.Size(76, 32);
            this.checkBoxCam.TabIndex = 0;
            this.checkBoxCam.Text = "Defualt";
            this.checkBoxCam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxCam.UseVisualStyleBackColor = true;
            this.checkBoxCam.Visible = false;
            // 
            // labelCam
            // 
            this.labelCam.AutoSize = true;
            this.labelCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCam.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelCam.Location = new System.Drawing.Point(2, 2);
            this.labelCam.Margin = new System.Windows.Forms.Padding(0);
            this.labelCam.Name = "labelCam";
            this.labelCam.Size = new System.Drawing.Size(100, 32);
            this.labelCam.TabIndex = 89;
            this.labelCam.Text = "Cam";
            this.labelCam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.layoutSize.Location = new System.Drawing.Point(104, 72);
            this.layoutSize.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSize.Name = "layoutSize";
            this.layoutSize.RowCount = 1;
            this.layoutSize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSize.Size = new System.Drawing.Size(499, 35);
            this.layoutSize.TabIndex = 67;
            // 
            // labelMaxUnit
            // 
            this.labelMaxUnit.AutoSize = true;
            this.labelMaxUnit.BackColor = System.Drawing.Color.Transparent;
            this.labelMaxUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaxUnit.Location = new System.Drawing.Point(420, 0);
            this.labelMaxUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelMaxUnit.Name = "labelMaxUnit";
            this.labelMaxUnit.Size = new System.Drawing.Size(60, 35);
            this.labelMaxUnit.TabIndex = 0;
            this.labelMaxUnit.Text = "[um]";
            this.labelMaxUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sizeMax
            // 
            this.sizeMax.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.sizeMax.ValueChanged += new System.EventHandler(this.size_ValueChanged);
            // 
            // labelMinUnit
            // 
            this.labelMinUnit.AutoSize = true;
            this.labelMinUnit.BackColor = System.Drawing.Color.Transparent;
            this.labelMinUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMinUnit.Location = new System.Drawing.Point(210, 0);
            this.labelMinUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelMinUnit.Name = "labelMinUnit";
            this.labelMinUnit.Size = new System.Drawing.Size(50, 35);
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
            this.labelMax.Size = new System.Drawing.Size(60, 35);
            this.labelMax.TabIndex = 84;
            this.labelMax.Text = "Max";
            this.labelMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sizeMin
            // 
            this.sizeMin.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.sizeMin.ValueChanged += new System.EventHandler(this.size_ValueChanged);
            // 
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.BackColor = System.Drawing.Color.Transparent;
            this.labelMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMin.Location = new System.Drawing.Point(50, 0);
            this.labelMin.Margin = new System.Windows.Forms.Padding(0);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(60, 35);
            this.labelMin.TabIndex = 73;
            this.labelMin.Text = "Min";
            this.labelMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // useSize
            // 
            this.useSize.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.useSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.useSize.Location = new System.Drawing.Point(0, 0);
            this.useSize.Margin = new System.Windows.Forms.Padding(0);
            this.useSize.Name = "useSize";
            this.useSize.Size = new System.Drawing.Size(50, 35);
            this.useSize.TabIndex = 99;
            this.useSize.UseVisualStyleBackColor = true;
            this.useSize.CheckedChanged += new System.EventHandler(this.useSize_CheckedChanged);
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSize.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelSize.Location = new System.Drawing.Point(2, 72);
            this.labelSize.Margin = new System.Windows.Forms.Padding(0);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(100, 35);
            this.labelSize.TabIndex = 0;
            this.labelSize.Text = "Size";
            this.labelSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelType.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelType.Location = new System.Drawing.Point(2, 36);
            this.labelType.Margin = new System.Windows.Forms.Padding(0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(100, 34);
            this.labelType.TabIndex = 0;
            this.labelType.Text = "Type";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFilterRect
            // 
            this.labelFilterRect.AutoSize = true;
            this.labelFilterRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFilterRect.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelFilterRect.Location = new System.Drawing.Point(605, 2);
            this.labelFilterRect.Margin = new System.Windows.Forms.Padding(0);
            this.labelFilterRect.Name = "labelFilterRect";
            this.labelFilterRect.Size = new System.Drawing.Size(100, 32);
            this.labelFilterRect.TabIndex = 89;
            this.labelFilterRect.Text = "Rect";
            this.labelFilterRect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelFilterRect.Visible = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Controls.Add(this.rectCY, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.useRect, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rectCX, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.rectH, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.rectW, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(707, 2);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(333, 32);
            this.tableLayoutPanel2.TabIndex = 67;
            this.tableLayoutPanel2.Visible = false;
            // 
            // rectCY
            // 
            this.rectCY.DecimalPlaces = 1;
            this.rectCY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rectCY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.rectCY.Location = new System.Drawing.Point(132, 0);
            this.rectCY.Margin = new System.Windows.Forms.Padding(0);
            this.rectCY.Name = "rectCY";
            this.rectCY.Size = new System.Drawing.Size(66, 29);
            this.rectCY.TabIndex = 69;
            this.rectCY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.rectCY.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.rectCY.ValueChanged += new System.EventHandler(this.rect_ValueChanged);
            // 
            // useRect
            // 
            this.useRect.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.useRect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.useRect.Location = new System.Drawing.Point(0, 0);
            this.useRect.Margin = new System.Windows.Forms.Padding(0);
            this.useRect.Name = "useRect";
            this.useRect.Size = new System.Drawing.Size(66, 32);
            this.useRect.TabIndex = 99;
            this.useRect.UseVisualStyleBackColor = true;
            this.useRect.CheckedChanged += new System.EventHandler(this.useRect_CheckedChanged);
            // 
            // rectCX
            // 
            this.rectCX.DecimalPlaces = 1;
            this.rectCX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rectCX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.rectCX.Location = new System.Drawing.Point(66, 0);
            this.rectCX.Margin = new System.Windows.Forms.Padding(0);
            this.rectCX.Name = "rectCX";
            this.rectCX.Size = new System.Drawing.Size(66, 29);
            this.rectCX.TabIndex = 69;
            this.rectCX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.rectCX.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.rectCX.ValueChanged += new System.EventHandler(this.rect_ValueChanged);
            // 
            // rectH
            // 
            this.rectH.DecimalPlaces = 1;
            this.rectH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rectH.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.rectH.Location = new System.Drawing.Point(264, 0);
            this.rectH.Margin = new System.Windows.Forms.Padding(0);
            this.rectH.Name = "rectH";
            this.rectH.Size = new System.Drawing.Size(69, 29);
            this.rectH.TabIndex = 69;
            this.rectH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.rectH.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.rectH.ValueChanged += new System.EventHandler(this.rect_ValueChanged);
            // 
            // rectW
            // 
            this.rectW.DecimalPlaces = 1;
            this.rectW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rectW.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.rectW.Location = new System.Drawing.Point(198, 0);
            this.rectW.Margin = new System.Windows.Forms.Padding(0);
            this.rectW.Name = "rectW";
            this.rectW.Size = new System.Drawing.Size(66, 29);
            this.rectW.TabIndex = 69;
            this.rectW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.rectW.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.rectW.ValueChanged += new System.EventHandler(this.rect_ValueChanged);
            // 
            // panelSelectType
            // 
            this.layoutSelectFilter.SetColumnSpan(this.panelSelectType, 3);
            this.panelSelectType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSelectType.Location = new System.Drawing.Point(104, 36);
            this.panelSelectType.Margin = new System.Windows.Forms.Padding(0);
            this.panelSelectType.Name = "panelSelectType";
            this.panelSelectType.Size = new System.Drawing.Size(936, 34);
            this.panelSelectType.TabIndex = 90;
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
            this.labelFilterTitle.Size = new System.Drawing.Size(1042, 35);
            this.labelFilterTitle.TabIndex = 1;
            this.labelFilterTitle.Text = "Filter";
            this.labelFilterTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutbottom
            // 
            this.layoutbottom.ColumnCount = 2;
            this.layoutbottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 330F));
            this.layoutbottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutbottom.Controls.Add(this.layoutLeft, 0, 0);
            this.layoutbottom.Controls.Add(this.panelResult, 1, 0);
            this.layoutbottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutbottom.Location = new System.Drawing.Point(0, 150);
            this.layoutbottom.Margin = new System.Windows.Forms.Padding(0);
            this.layoutbottom.Name = "layoutbottom";
            this.layoutbottom.RowCount = 1;
            this.layoutbottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutbottom.Size = new System.Drawing.Size(1206, 681);
            this.layoutbottom.TabIndex = 144;
            // 
            // layoutLeft
            // 
            this.layoutLeft.AutoSize = true;
            this.layoutLeft.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutLeft.ColumnCount = 1;
            this.layoutLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLeft.Controls.Add(this.tableLayoutPanelSheetList, 0, 2);
            this.layoutLeft.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.layoutLeft.Controls.Add(this.tableLayoutPanelDefectList, 0, 1);
            this.layoutLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutLeft.Location = new System.Drawing.Point(0, 0);
            this.layoutLeft.Margin = new System.Windows.Forms.Padding(0);
            this.layoutLeft.Name = "layoutLeft";
            this.layoutLeft.RowCount = 3;
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLeft.Size = new System.Drawing.Size(330, 681);
            this.layoutLeft.TabIndex = 92;
            // 
            // tableLayoutPanelSheetList
            // 
            this.tableLayoutPanelSheetList.AutoSize = true;
            this.tableLayoutPanelSheetList.ColumnCount = 3;
            this.tableLayoutPanelSheetList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelSheetList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSheetList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelSheetList.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanelSheetList.Controls.Add(this.sheetList, 1, 1);
            this.tableLayoutPanelSheetList.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanelSheetList.Controls.Add(this.buttonSelectAll, 0, 2);
            this.tableLayoutPanelSheetList.Controls.Add(this.okFilter, 1, 2);
            this.tableLayoutPanelSheetList.Controls.Add(this.ngFilter, 2, 2);
            this.tableLayoutPanelSheetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSheetList.Location = new System.Drawing.Point(1, 436);
            this.tableLayoutPanelSheetList.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelSheetList.Name = "tableLayoutPanelSheetList";
            this.tableLayoutPanelSheetList.RowCount = 3;
            this.tableLayoutPanelSheetList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelSheetList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSheetList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSheetList.Size = new System.Drawing.Size(328, 244);
            this.tableLayoutPanelSheetList.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelSheetInspector, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.sheetInspector, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.sheetRatio, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.labelSheetEraser, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelSheetRatio, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.sheetEraser, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 40);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(100, 169);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // labelSheetInspector
            // 
            this.labelSheetInspector.AutoSize = true;
            this.labelSheetInspector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetInspector.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelSheetInspector.Location = new System.Drawing.Point(0, 0);
            this.labelSheetInspector.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetInspector.MinimumSize = new System.Drawing.Size(0, 30);
            this.labelSheetInspector.Name = "labelSheetInspector";
            this.labelSheetInspector.Size = new System.Drawing.Size(100, 30);
            this.labelSheetInspector.TabIndex = 1;
            this.labelSheetInspector.Text = "Total / NG";
            this.labelSheetInspector.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetInspector
            // 
            this.sheetInspector.AutoSize = true;
            this.sheetInspector.BackColor = System.Drawing.Color.White;
            this.sheetInspector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetInspector.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.sheetInspector.Location = new System.Drawing.Point(0, 30);
            this.sheetInspector.Margin = new System.Windows.Forms.Padding(0);
            this.sheetInspector.MinimumSize = new System.Drawing.Size(20, 0);
            this.sheetInspector.Name = "sheetInspector";
            this.sheetInspector.Size = new System.Drawing.Size(100, 20);
            this.sheetInspector.TabIndex = 6;
            this.sheetInspector.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetRatio
            // 
            this.sheetRatio.AutoSize = true;
            this.sheetRatio.BackColor = System.Drawing.Color.White;
            this.sheetRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetRatio.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.sheetRatio.Location = new System.Drawing.Point(0, 130);
            this.sheetRatio.Margin = new System.Windows.Forms.Padding(0);
            this.sheetRatio.MinimumSize = new System.Drawing.Size(20, 0);
            this.sheetRatio.Name = "sheetRatio";
            this.sheetRatio.Size = new System.Drawing.Size(100, 20);
            this.sheetRatio.TabIndex = 2;
            this.sheetRatio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetEraser
            // 
            this.labelSheetEraser.AutoSize = true;
            this.labelSheetEraser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetEraser.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelSheetEraser.Location = new System.Drawing.Point(0, 50);
            this.labelSheetEraser.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetEraser.MinimumSize = new System.Drawing.Size(0, 30);
            this.labelSheetEraser.Name = "labelSheetEraser";
            this.labelSheetEraser.Size = new System.Drawing.Size(100, 30);
            this.labelSheetEraser.TabIndex = 3;
            this.labelSheetEraser.Text = "Erase / Done";
            this.labelSheetEraser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetRatio
            // 
            this.labelSheetRatio.AutoSize = true;
            this.labelSheetRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetRatio.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelSheetRatio.Location = new System.Drawing.Point(0, 100);
            this.labelSheetRatio.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetRatio.MinimumSize = new System.Drawing.Size(0, 30);
            this.labelSheetRatio.Name = "labelSheetRatio";
            this.labelSheetRatio.Size = new System.Drawing.Size(100, 30);
            this.labelSheetRatio.TabIndex = 0;
            this.labelSheetRatio.Text = "Ratio";
            this.labelSheetRatio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetEraser
            // 
            this.sheetEraser.AutoSize = true;
            this.sheetEraser.BackColor = System.Drawing.Color.White;
            this.sheetEraser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetEraser.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.sheetEraser.Location = new System.Drawing.Point(0, 80);
            this.sheetEraser.Margin = new System.Windows.Forms.Padding(0);
            this.sheetEraser.MinimumSize = new System.Drawing.Size(20, 0);
            this.sheetEraser.Name = "sheetEraser";
            this.sheetEraser.Size = new System.Drawing.Size(100, 20);
            this.sheetEraser.TabIndex = 4;
            this.sheetEraser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetList
            // 
            this.sheetList.AllowUserToAddRows = false;
            this.sheetList.AllowUserToDeleteRows = false;
            this.sheetList.AllowUserToResizeRows = false;
            this.sheetList.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.sheetList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.sheetList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sheetList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnPattern,
            this.columnTime,
            this.columnQty,
            this.columnErased});
            this.tableLayoutPanelSheetList.SetColumnSpan(this.sheetList, 2);
            this.sheetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.sheetList.Location = new System.Drawing.Point(100, 40);
            this.sheetList.Margin = new System.Windows.Forms.Padding(0);
            this.sheetList.Name = "sheetList";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.sheetList.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.sheetList.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.sheetList.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.sheetList.RowTemplate.Height = 23;
            this.sheetList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.sheetList.Size = new System.Drawing.Size(228, 169);
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
            // columnTime
            // 
            this.columnTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Format = "F02";
            this.columnTime.DefaultCellStyle = dataGridViewCellStyle2;
            this.columnTime.HeaderText = "Time[s]";
            this.columnTime.MinimumWidth = 30;
            this.columnTime.Name = "columnTime";
            this.columnTime.Width = 91;
            // 
            // columnQty
            // 
            this.columnQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnQty.FillWeight = 139.0863F;
            this.columnQty.HeaderText = "Qty.";
            this.columnQty.Name = "columnQty";
            // 
            // columnErased
            // 
            this.columnErased.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnErased.HeaderText = "Erased";
            this.columnErased.Name = "columnErased";
            this.columnErased.Width = 85;
            // 
            // panel1
            // 
            this.tableLayoutPanelSheetList.SetColumnSpan(this.panel1, 3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.labelPatternList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(328, 40);
            this.panel1.TabIndex = 64;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.label4.Dock = System.Windows.Forms.DockStyle.Right;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.label4.Location = new System.Drawing.Point(295, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.MinimumSize = new System.Drawing.Size(0, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 35);
            this.label4.TabIndex = 0;
            this.label4.Text = "[EA]";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // labelPatternList
            // 
            this.labelPatternList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelPatternList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPatternList.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelPatternList.Location = new System.Drawing.Point(0, 0);
            this.labelPatternList.Margin = new System.Windows.Forms.Padding(0);
            this.labelPatternList.Name = "labelPatternList";
            this.labelPatternList.Size = new System.Drawing.Size(328, 40);
            this.labelPatternList.TabIndex = 0;
            this.labelPatternList.Text = "Sheet List";
            this.labelPatternList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSelectAll.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.buttonSelectAll.Location = new System.Drawing.Point(0, 209);
            this.buttonSelectAll.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSelectAll.MinimumSize = new System.Drawing.Size(0, 30);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(100, 35);
            this.buttonSelectAll.TabIndex = 0;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // okFilter
            // 
            this.okFilter.AutoSize = true;
            this.okFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.okFilter.Location = new System.Drawing.Point(103, 212);
            this.okFilter.Name = "okFilter";
            this.okFilter.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.okFilter.Size = new System.Drawing.Size(108, 29);
            this.okFilter.TabIndex = 0;
            this.okFilter.Text = "OK";
            this.okFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.okFilter.UseVisualStyleBackColor = true;
            this.okFilter.CheckedChanged += new System.EventHandler(this.okFilter_CheckedChanged);
            // 
            // ngFilter
            // 
            this.ngFilter.AutoSize = true;
            this.ngFilter.Checked = true;
            this.ngFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ngFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ngFilter.Location = new System.Drawing.Point(217, 212);
            this.ngFilter.Name = "ngFilter";
            this.ngFilter.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.ngFilter.Size = new System.Drawing.Size(108, 29);
            this.ngFilter.TabIndex = 1;
            this.ngFilter.Text = "NG";
            this.ngFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ngFilter.UseVisualStyleBackColor = true;
            this.ngFilter.CheckedChanged += new System.EventHandler(this.ngFilter_CheckedChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.panel2, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.sheetLength, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanelSheetLenDetail, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.labelInfo, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.panelProductionTargetSpd, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.labelProductionModelName, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.labelProductionLotName, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.labelProductionTime, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.labelProductionTargetSpd, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.productionModelName, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.productionTime, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.productionLotName, 1, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 7;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(328, 232);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel2.Controls.Add(this.labelSheetLengthUnit);
            this.panel2.Controls.Add(this.labelSheetLength);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 132);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(100, 54);
            this.panel2.TabIndex = 0;
            // 
            // labelSheetLengthUnit
            // 
            this.labelSheetLengthUnit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelSheetLengthUnit.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.labelSheetLengthUnit.Location = new System.Drawing.Point(0, 41);
            this.labelSheetLengthUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetLengthUnit.Name = "labelSheetLengthUnit";
            this.labelSheetLengthUnit.Size = new System.Drawing.Size(100, 13);
            this.labelSheetLengthUnit.TabIndex = 0;
            this.labelSheetLengthUnit.Text = "[mm]";
            this.labelSheetLengthUnit.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // labelSheetLength
            // 
            this.labelSheetLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetLength.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelSheetLength.Location = new System.Drawing.Point(0, 0);
            this.labelSheetLength.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetLength.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelSheetLength.Name = "labelSheetLength";
            this.labelSheetLength.Size = new System.Drawing.Size(100, 54);
            this.labelSheetLength.TabIndex = 0;
            this.labelSheetLength.Text = "Sheet Len.";
            this.labelSheetLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetLength
            // 
            this.sheetLength.AutoSize = true;
            this.sheetLength.ColumnCount = 3;
            this.sheetLength.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.sheetLength.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.sheetLength.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.sheetLength.Controls.Add(this.labelSheetLengthLow10p, 0, 0);
            this.sheetLength.Controls.Add(this.infoHeight1, 0, 1);
            this.sheetLength.Controls.Add(this.infoHeight2, 1, 1);
            this.sheetLength.Controls.Add(this.infoHeight3, 2, 1);
            this.sheetLength.Controls.Add(this.labelSheetLengthMid80p, 1, 0);
            this.sheetLength.Controls.Add(this.labelSheetLengthHigh10p, 2, 0);
            this.sheetLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetLength.Location = new System.Drawing.Point(100, 132);
            this.sheetLength.Margin = new System.Windows.Forms.Padding(0);
            this.sheetLength.Name = "sheetLength";
            this.sheetLength.RowCount = 2;
            this.sheetLength.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.sheetLength.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.sheetLength.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.sheetLength.Size = new System.Drawing.Size(228, 54);
            this.sheetLength.TabIndex = 186;
            // 
            // labelSheetLengthLow10p
            // 
            this.labelSheetLengthLow10p.AutoSize = true;
            this.labelSheetLengthLow10p.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetLengthLow10p.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.labelSheetLengthLow10p.Location = new System.Drawing.Point(0, 0);
            this.labelSheetLengthLow10p.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetLengthLow10p.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelSheetLengthLow10p.Name = "labelSheetLengthLow10p";
            this.labelSheetLengthLow10p.Size = new System.Drawing.Size(75, 23);
            this.labelSheetLengthLow10p.TabIndex = 0;
            this.labelSheetLengthLow10p.Text = "Low 10%";
            this.labelSheetLengthLow10p.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infoHeight1
            // 
            this.infoHeight1.BackColor = System.Drawing.Color.White;
            this.infoHeight1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoHeight1.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.infoHeight1.Location = new System.Drawing.Point(0, 23);
            this.infoHeight1.Margin = new System.Windows.Forms.Padding(0);
            this.infoHeight1.MinimumSize = new System.Drawing.Size(0, 23);
            this.infoHeight1.Name = "infoHeight1";
            this.infoHeight1.Size = new System.Drawing.Size(75, 31);
            this.infoHeight1.TabIndex = 0;
            this.infoHeight1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infoHeight2
            // 
            this.infoHeight2.BackColor = System.Drawing.Color.White;
            this.infoHeight2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoHeight2.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.infoHeight2.Location = new System.Drawing.Point(75, 23);
            this.infoHeight2.Margin = new System.Windows.Forms.Padding(0);
            this.infoHeight2.MinimumSize = new System.Drawing.Size(0, 23);
            this.infoHeight2.Name = "infoHeight2";
            this.infoHeight2.Size = new System.Drawing.Size(76, 31);
            this.infoHeight2.TabIndex = 0;
            this.infoHeight2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infoHeight3
            // 
            this.infoHeight3.BackColor = System.Drawing.Color.White;
            this.infoHeight3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoHeight3.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.infoHeight3.Location = new System.Drawing.Point(151, 23);
            this.infoHeight3.Margin = new System.Windows.Forms.Padding(0);
            this.infoHeight3.MinimumSize = new System.Drawing.Size(0, 23);
            this.infoHeight3.Name = "infoHeight3";
            this.infoHeight3.Size = new System.Drawing.Size(77, 31);
            this.infoHeight3.TabIndex = 0;
            this.infoHeight3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetLengthMid80p
            // 
            this.labelSheetLengthMid80p.AutoSize = true;
            this.labelSheetLengthMid80p.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetLengthMid80p.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.labelSheetLengthMid80p.Location = new System.Drawing.Point(75, 0);
            this.labelSheetLengthMid80p.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetLengthMid80p.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelSheetLengthMid80p.Name = "labelSheetLengthMid80p";
            this.labelSheetLengthMid80p.Size = new System.Drawing.Size(76, 23);
            this.labelSheetLengthMid80p.TabIndex = 0;
            this.labelSheetLengthMid80p.Text = "Mid 80%";
            this.labelSheetLengthMid80p.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetLengthHigh10p
            // 
            this.labelSheetLengthHigh10p.AutoSize = true;
            this.labelSheetLengthHigh10p.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetLengthHigh10p.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.labelSheetLengthHigh10p.Location = new System.Drawing.Point(151, 0);
            this.labelSheetLengthHigh10p.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetLengthHigh10p.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelSheetLengthHigh10p.Name = "labelSheetLengthHigh10p";
            this.labelSheetLengthHigh10p.Size = new System.Drawing.Size(77, 23);
            this.labelSheetLengthHigh10p.TabIndex = 0;
            this.labelSheetLengthHigh10p.Text = "High 10%";
            this.labelSheetLengthHigh10p.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanelSheetLenDetail
            // 
            this.tableLayoutPanelSheetLenDetail.AutoSize = true;
            this.tableLayoutPanelSheetLenDetail.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelSheetLenDetail.ColumnCount = 5;
            this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanelSheetLenDetail, 2);
            this.tableLayoutPanelSheetLenDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelSheetLenDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelSheetLenDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelSheetLenDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelSheetLenDetail.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelSheetLenDetail.Controls.Add(this.labelSheetLengthMean, 0, 0);
            this.tableLayoutPanelSheetLenDetail.Controls.Add(this.infoHeightMean, 0, 1);
            this.tableLayoutPanelSheetLenDetail.Controls.Add(this.labelSheetLengthStdDev, 1, 0);
            this.tableLayoutPanelSheetLenDetail.Controls.Add(this.infoHeightStdDev, 1, 1);
            this.tableLayoutPanelSheetLenDetail.Controls.Add(this.labelSheetLengthMax, 2, 0);
            this.tableLayoutPanelSheetLenDetail.Controls.Add(this.infoHeightMax, 2, 1);
            this.tableLayoutPanelSheetLenDetail.Controls.Add(this.labelSheetLengthMin, 3, 0);
            this.tableLayoutPanelSheetLenDetail.Controls.Add(this.infoHeightMin, 3, 1);
            this.tableLayoutPanelSheetLenDetail.Controls.Add(this.labelSheetLengthDiff, 4, 0);
            this.tableLayoutPanelSheetLenDetail.Controls.Add(this.infoHeightRange, 4, 1);
            this.tableLayoutPanelSheetLenDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelSheetLenDetail.Location = new System.Drawing.Point(0, 186);
            this.tableLayoutPanelSheetLenDetail.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelSheetLenDetail.Name = "tableLayoutPanelSheetLenDetail";
            this.tableLayoutPanelSheetLenDetail.RowCount = 2;
            this.tableLayoutPanelSheetLenDetail.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSheetLenDetail.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSheetLenDetail.Size = new System.Drawing.Size(328, 46);
            this.tableLayoutPanelSheetLenDetail.TabIndex = 91;
            this.tableLayoutPanelSheetLenDetail.Visible = false;
            // 
            // labelSheetLengthMean
            // 
            this.labelSheetLengthMean.AutoSize = true;
            this.labelSheetLengthMean.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetLengthMean.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.labelSheetLengthMean.Location = new System.Drawing.Point(0, 0);
            this.labelSheetLengthMean.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetLengthMean.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelSheetLengthMean.Name = "labelSheetLengthMean";
            this.labelSheetLengthMean.Size = new System.Drawing.Size(65, 23);
            this.labelSheetLengthMean.TabIndex = 0;
            this.labelSheetLengthMean.Text = "Mean";
            this.labelSheetLengthMean.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infoHeightMean
            // 
            this.infoHeightMean.AutoSize = true;
            this.infoHeightMean.BackColor = System.Drawing.Color.White;
            this.infoHeightMean.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoHeightMean.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.infoHeightMean.Location = new System.Drawing.Point(0, 23);
            this.infoHeightMean.Margin = new System.Windows.Forms.Padding(0);
            this.infoHeightMean.MinimumSize = new System.Drawing.Size(10, 23);
            this.infoHeightMean.Name = "infoHeightMean";
            this.infoHeightMean.Size = new System.Drawing.Size(65, 23);
            this.infoHeightMean.TabIndex = 0;
            this.infoHeightMean.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetLengthStdDev
            // 
            this.labelSheetLengthStdDev.AutoSize = true;
            this.labelSheetLengthStdDev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetLengthStdDev.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.labelSheetLengthStdDev.Location = new System.Drawing.Point(65, 0);
            this.labelSheetLengthStdDev.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetLengthStdDev.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelSheetLengthStdDev.Name = "labelSheetLengthStdDev";
            this.labelSheetLengthStdDev.Size = new System.Drawing.Size(65, 23);
            this.labelSheetLengthStdDev.TabIndex = 0;
            this.labelSheetLengthStdDev.Text = "S.D.";
            this.labelSheetLengthStdDev.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infoHeightStdDev
            // 
            this.infoHeightStdDev.AutoSize = true;
            this.infoHeightStdDev.BackColor = System.Drawing.Color.White;
            this.infoHeightStdDev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoHeightStdDev.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.infoHeightStdDev.Location = new System.Drawing.Point(65, 23);
            this.infoHeightStdDev.Margin = new System.Windows.Forms.Padding(0);
            this.infoHeightStdDev.MinimumSize = new System.Drawing.Size(10, 23);
            this.infoHeightStdDev.Name = "infoHeightStdDev";
            this.infoHeightStdDev.Size = new System.Drawing.Size(65, 23);
            this.infoHeightStdDev.TabIndex = 0;
            this.infoHeightStdDev.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetLengthMax
            // 
            this.labelSheetLengthMax.AutoSize = true;
            this.labelSheetLengthMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetLengthMax.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.labelSheetLengthMax.Location = new System.Drawing.Point(130, 0);
            this.labelSheetLengthMax.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetLengthMax.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelSheetLengthMax.Name = "labelSheetLengthMax";
            this.labelSheetLengthMax.Size = new System.Drawing.Size(65, 23);
            this.labelSheetLengthMax.TabIndex = 0;
            this.labelSheetLengthMax.Text = "Max";
            this.labelSheetLengthMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infoHeightMax
            // 
            this.infoHeightMax.AutoSize = true;
            this.infoHeightMax.BackColor = System.Drawing.Color.White;
            this.infoHeightMax.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoHeightMax.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.infoHeightMax.Location = new System.Drawing.Point(130, 23);
            this.infoHeightMax.Margin = new System.Windows.Forms.Padding(0);
            this.infoHeightMax.MinimumSize = new System.Drawing.Size(10, 23);
            this.infoHeightMax.Name = "infoHeightMax";
            this.infoHeightMax.Size = new System.Drawing.Size(65, 23);
            this.infoHeightMax.TabIndex = 0;
            this.infoHeightMax.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetLengthMin
            // 
            this.labelSheetLengthMin.AutoSize = true;
            this.labelSheetLengthMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetLengthMin.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.labelSheetLengthMin.Location = new System.Drawing.Point(195, 0);
            this.labelSheetLengthMin.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetLengthMin.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelSheetLengthMin.Name = "labelSheetLengthMin";
            this.labelSheetLengthMin.Size = new System.Drawing.Size(65, 23);
            this.labelSheetLengthMin.TabIndex = 0;
            this.labelSheetLengthMin.Text = "Min";
            this.labelSheetLengthMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infoHeightMin
            // 
            this.infoHeightMin.AutoSize = true;
            this.infoHeightMin.BackColor = System.Drawing.Color.White;
            this.infoHeightMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoHeightMin.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.infoHeightMin.Location = new System.Drawing.Point(195, 23);
            this.infoHeightMin.Margin = new System.Windows.Forms.Padding(0);
            this.infoHeightMin.MinimumSize = new System.Drawing.Size(10, 23);
            this.infoHeightMin.Name = "infoHeightMin";
            this.infoHeightMin.Size = new System.Drawing.Size(65, 23);
            this.infoHeightMin.TabIndex = 0;
            this.infoHeightMin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetLengthDiff
            // 
            this.labelSheetLengthDiff.AutoSize = true;
            this.labelSheetLengthDiff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetLengthDiff.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.labelSheetLengthDiff.Location = new System.Drawing.Point(260, 0);
            this.labelSheetLengthDiff.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetLengthDiff.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelSheetLengthDiff.Name = "labelSheetLengthDiff";
            this.labelSheetLengthDiff.Size = new System.Drawing.Size(68, 23);
            this.labelSheetLengthDiff.TabIndex = 0;
            this.labelSheetLengthDiff.Text = "Diff";
            this.labelSheetLengthDiff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // infoHeightRange
            // 
            this.infoHeightRange.AutoSize = true;
            this.infoHeightRange.BackColor = System.Drawing.Color.White;
            this.infoHeightRange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoHeightRange.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.infoHeightRange.Location = new System.Drawing.Point(260, 23);
            this.infoHeightRange.Margin = new System.Windows.Forms.Padding(0);
            this.infoHeightRange.MinimumSize = new System.Drawing.Size(10, 23);
            this.infoHeightRange.Name = "infoHeightRange";
            this.infoHeightRange.Size = new System.Drawing.Size(68, 23);
            this.infoHeightRange.TabIndex = 0;
            this.infoHeightRange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInfo
            // 
            this.labelInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.tableLayoutPanel3.SetColumnSpan(this.labelInfo, 2);
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelInfo.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelInfo.Location = new System.Drawing.Point(0, 0);
            this.labelInfo.Margin = new System.Windows.Forms.Padding(0);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(328, 40);
            this.labelInfo.TabIndex = 69;
            this.labelInfo.Text = "Info";
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelProductionTargetSpd
            // 
            this.panelProductionTargetSpd.Controls.Add(this.labelProductionTargetSpdUnit);
            this.panelProductionTargetSpd.Controls.Add(this.productionTargetSpd);
            this.panelProductionTargetSpd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelProductionTargetSpd.Location = new System.Drawing.Point(100, 109);
            this.panelProductionTargetSpd.Margin = new System.Windows.Forms.Padding(0);
            this.panelProductionTargetSpd.Name = "panelProductionTargetSpd";
            this.panelProductionTargetSpd.Size = new System.Drawing.Size(228, 23);
            this.panelProductionTargetSpd.TabIndex = 0;
            // 
            // labelProductionTargetSpdUnit
            // 
            this.labelProductionTargetSpdUnit.AutoSize = true;
            this.labelProductionTargetSpdUnit.BackColor = System.Drawing.Color.White;
            this.labelProductionTargetSpdUnit.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelProductionTargetSpdUnit.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.labelProductionTargetSpdUnit.Location = new System.Drawing.Point(191, 0);
            this.labelProductionTargetSpdUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelProductionTargetSpdUnit.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelProductionTargetSpdUnit.Name = "labelProductionTargetSpdUnit";
            this.labelProductionTargetSpdUnit.Size = new System.Drawing.Size(37, 23);
            this.labelProductionTargetSpdUnit.TabIndex = 0;
            this.labelProductionTargetSpdUnit.Text = "[m/m]";
            this.labelProductionTargetSpdUnit.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // productionTargetSpd
            // 
            this.productionTargetSpd.BackColor = System.Drawing.Color.White;
            this.productionTargetSpd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productionTargetSpd.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.productionTargetSpd.Location = new System.Drawing.Point(0, 0);
            this.productionTargetSpd.Margin = new System.Windows.Forms.Padding(0);
            this.productionTargetSpd.Name = "productionTargetSpd";
            this.productionTargetSpd.Size = new System.Drawing.Size(228, 23);
            this.productionTargetSpd.TabIndex = 2;
            this.productionTargetSpd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProductionModelName
            // 
            this.labelProductionModelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProductionModelName.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelProductionModelName.Location = new System.Drawing.Point(0, 40);
            this.labelProductionModelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelProductionModelName.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelProductionModelName.Name = "labelProductionModelName";
            this.labelProductionModelName.Size = new System.Drawing.Size(100, 23);
            this.labelProductionModelName.TabIndex = 1;
            this.labelProductionModelName.Text = "Model";
            this.labelProductionModelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProductionLotName
            // 
            this.labelProductionLotName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProductionLotName.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelProductionLotName.Location = new System.Drawing.Point(0, 63);
            this.labelProductionLotName.Margin = new System.Windows.Forms.Padding(0);
            this.labelProductionLotName.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelProductionLotName.Name = "labelProductionLotName";
            this.labelProductionLotName.Size = new System.Drawing.Size(100, 23);
            this.labelProductionLotName.TabIndex = 1;
            this.labelProductionLotName.Text = "Lot";
            this.labelProductionLotName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProductionTime
            // 
            this.labelProductionTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProductionTime.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelProductionTime.Location = new System.Drawing.Point(0, 86);
            this.labelProductionTime.Margin = new System.Windows.Forms.Padding(0);
            this.labelProductionTime.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelProductionTime.Name = "labelProductionTime";
            this.labelProductionTime.Size = new System.Drawing.Size(100, 23);
            this.labelProductionTime.TabIndex = 3;
            this.labelProductionTime.Text = "Start/End";
            this.labelProductionTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProductionTargetSpd
            // 
            this.labelProductionTargetSpd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProductionTargetSpd.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelProductionTargetSpd.Location = new System.Drawing.Point(0, 109);
            this.labelProductionTargetSpd.Margin = new System.Windows.Forms.Padding(0);
            this.labelProductionTargetSpd.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelProductionTargetSpd.Name = "labelProductionTargetSpd";
            this.labelProductionTargetSpd.Size = new System.Drawing.Size(100, 23);
            this.labelProductionTargetSpd.TabIndex = 0;
            this.labelProductionTargetSpd.Text = "P.G. Speed";
            this.labelProductionTargetSpd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // productionModelName
            // 
            this.productionModelName.BackColor = System.Drawing.Color.White;
            this.productionModelName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.productionModelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productionModelName.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.productionModelName.Location = new System.Drawing.Point(100, 40);
            this.productionModelName.Margin = new System.Windows.Forms.Padding(0);
            this.productionModelName.Multiline = true;
            this.productionModelName.Name = "productionModelName";
            this.productionModelName.ReadOnly = true;
            this.productionModelName.Size = new System.Drawing.Size(228, 23);
            this.productionModelName.TabIndex = 6;
            this.productionModelName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // productionTime
            // 
            this.productionTime.BackColor = System.Drawing.Color.White;
            this.productionTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.productionTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productionTime.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.productionTime.Location = new System.Drawing.Point(100, 86);
            this.productionTime.Margin = new System.Windows.Forms.Padding(0);
            this.productionTime.Multiline = true;
            this.productionTime.Name = "productionTime";
            this.productionTime.ReadOnly = true;
            this.productionTime.Size = new System.Drawing.Size(228, 23);
            this.productionTime.TabIndex = 4;
            this.productionTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // productionLotName
            // 
            this.productionLotName.BackColor = System.Drawing.Color.White;
            this.productionLotName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.productionLotName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productionLotName.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.productionLotName.Location = new System.Drawing.Point(100, 63);
            this.productionLotName.Margin = new System.Windows.Forms.Padding(0);
            this.productionLotName.Multiline = true;
            this.productionLotName.Name = "productionLotName";
            this.productionLotName.ReadOnly = true;
            this.productionLotName.Size = new System.Drawing.Size(228, 23);
            this.productionLotName.TabIndex = 6;
            this.productionLotName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tableLayoutPanelDefectList
            // 
            this.tableLayoutPanelDefectList.AutoSize = true;
            this.tableLayoutPanelDefectList.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelDefectList.ColumnCount = 2;
            this.tableLayoutPanelDefectList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanelDefectList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDefectList.Controls.Add(this.defectInfoPanel, 0, 0);
            this.tableLayoutPanelDefectList.Controls.Add(this.labelNoPrint, 0, 1);
            this.tableLayoutPanelDefectList.Controls.Add(this.labelPinHole, 0, 2);
            this.tableLayoutPanelDefectList.Controls.Add(this.sheetAttackNum, 1, 4);
            this.tableLayoutPanelDefectList.Controls.Add(this.labelSheetAttack, 0, 4);
            this.tableLayoutPanelDefectList.Controls.Add(this.labelSpread, 0, 3);
            this.tableLayoutPanelDefectList.Controls.Add(this.labelDielectric, 0, 5);
            this.tableLayoutPanelDefectList.Controls.Add(this.labelSticker, 0, 6);
            this.tableLayoutPanelDefectList.Controls.Add(this.labelMargin, 0, 7);
            this.tableLayoutPanelDefectList.Controls.Add(this.noPrintNum, 1, 1);
            this.tableLayoutPanelDefectList.Controls.Add(this.pinHoleNum, 1, 2);
            this.tableLayoutPanelDefectList.Controls.Add(this.spreadNum, 1, 3);
            this.tableLayoutPanelDefectList.Controls.Add(this.marginNum, 1, 7);
            this.tableLayoutPanelDefectList.Controls.Add(this.stickerNum, 1, 6);
            this.tableLayoutPanelDefectList.Controls.Add(this.dielectricNum, 1, 5);
            this.tableLayoutPanelDefectList.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelDefectList.Location = new System.Drawing.Point(1, 234);
            this.tableLayoutPanelDefectList.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelDefectList.Name = "tableLayoutPanelDefectList";
            this.tableLayoutPanelDefectList.RowCount = 8;
            this.tableLayoutPanelDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDefectList.Size = new System.Drawing.Size(328, 201);
            this.tableLayoutPanelDefectList.TabIndex = 2;
            // 
            // defectInfoPanel
            // 
            this.tableLayoutPanelDefectList.SetColumnSpan(this.defectInfoPanel, 2);
            this.defectInfoPanel.Controls.Add(this.labelDefectsUnit);
            this.defectInfoPanel.Controls.Add(this.labelDefects);
            this.defectInfoPanel.Controls.Add(this.patternRadioButton);
            this.defectInfoPanel.Controls.Add(this.defectRadioButton);
            this.defectInfoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectInfoPanel.Location = new System.Drawing.Point(0, 0);
            this.defectInfoPanel.Margin = new System.Windows.Forms.Padding(0);
            this.defectInfoPanel.Name = "defectInfoPanel";
            this.defectInfoPanel.Size = new System.Drawing.Size(328, 40);
            this.defectInfoPanel.TabIndex = 180;
            // 
            // labelDefectsUnit
            // 
            this.labelDefectsUnit.AutoSize = true;
            this.labelDefectsUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelDefectsUnit.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelDefectsUnit.Font = new System.Drawing.Font("맑은 고딕", 8F);
            this.labelDefectsUnit.Location = new System.Drawing.Point(197, 0);
            this.labelDefectsUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelDefectsUnit.MinimumSize = new System.Drawing.Size(0, 35);
            this.labelDefectsUnit.Name = "labelDefectsUnit";
            this.labelDefectsUnit.Size = new System.Drawing.Size(26, 35);
            this.labelDefectsUnit.TabIndex = 174;
            this.labelDefectsUnit.Text = "[EA]";
            this.labelDefectsUnit.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // labelDefects
            // 
            this.labelDefects.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelDefects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefects.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelDefects.Location = new System.Drawing.Point(0, 0);
            this.labelDefects.Margin = new System.Windows.Forms.Padding(0);
            this.labelDefects.Name = "labelDefects";
            this.labelDefects.Size = new System.Drawing.Size(223, 40);
            this.labelDefects.TabIndex = 174;
            this.labelDefects.Text = "Defects";
            this.labelDefects.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // patternRadioButton
            // 
            this.patternRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.patternRadioButton.AutoSize = true;
            this.patternRadioButton.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.patternRadioButton.Checked = true;
            this.patternRadioButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.patternRadioButton.FlatAppearance.BorderSize = 0;
            this.patternRadioButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.patternRadioButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.patternRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.patternRadioButton.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.patternRadioButton.Location = new System.Drawing.Point(223, 0);
            this.patternRadioButton.Margin = new System.Windows.Forms.Padding(0);
            this.patternRadioButton.Name = "patternRadioButton";
            this.patternRadioButton.Size = new System.Drawing.Size(55, 40);
            this.patternRadioButton.TabIndex = 176;
            this.patternRadioButton.TabStop = true;
            this.patternRadioButton.Text = "Pattern";
            this.patternRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.patternRadioButton.UseVisualStyleBackColor = false;
            this.patternRadioButton.CheckedChanged += new System.EventHandler(this.patternRadioButton_CheckedChanged);
            // 
            // defectRadioButton
            // 
            this.defectRadioButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.defectRadioButton.AutoSize = true;
            this.defectRadioButton.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.defectRadioButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.defectRadioButton.FlatAppearance.BorderSize = 0;
            this.defectRadioButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.defectRadioButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.defectRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.defectRadioButton.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.defectRadioButton.Location = new System.Drawing.Point(278, 0);
            this.defectRadioButton.Margin = new System.Windows.Forms.Padding(0);
            this.defectRadioButton.Name = "defectRadioButton";
            this.defectRadioButton.Size = new System.Drawing.Size(50, 40);
            this.defectRadioButton.TabIndex = 175;
            this.defectRadioButton.Text = "Defect";
            this.defectRadioButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.defectRadioButton.UseVisualStyleBackColor = false;
            this.defectRadioButton.CheckedChanged += new System.EventHandler(this.totalRadioButton_CheckedChanged);
            // 
            // labelNoPrint
            // 
            this.labelNoPrint.BackColor = System.Drawing.Color.Transparent;
            this.labelNoPrint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNoPrint.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelNoPrint.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelNoPrint.Location = new System.Drawing.Point(0, 40);
            this.labelNoPrint.Margin = new System.Windows.Forms.Padding(0);
            this.labelNoPrint.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelNoPrint.Name = "labelNoPrint";
            this.labelNoPrint.Size = new System.Drawing.Size(100, 23);
            this.labelNoPrint.TabIndex = 182;
            this.labelNoPrint.Text = "Noprint";
            this.labelNoPrint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPinHole
            // 
            this.labelPinHole.BackColor = System.Drawing.Color.Transparent;
            this.labelPinHole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPinHole.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelPinHole.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelPinHole.Location = new System.Drawing.Point(0, 63);
            this.labelPinHole.Margin = new System.Windows.Forms.Padding(0);
            this.labelPinHole.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelPinHole.Name = "labelPinHole";
            this.labelPinHole.Size = new System.Drawing.Size(100, 23);
            this.labelPinHole.TabIndex = 182;
            this.labelPinHole.Text = "PinHole";
            this.labelPinHole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetAttackNum
            // 
            this.sheetAttackNum.AutoSize = true;
            this.sheetAttackNum.BackColor = System.Drawing.Color.White;
            this.sheetAttackNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetAttackNum.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.sheetAttackNum.Location = new System.Drawing.Point(100, 109);
            this.sheetAttackNum.Margin = new System.Windows.Forms.Padding(0);
            this.sheetAttackNum.MinimumSize = new System.Drawing.Size(20, 20);
            this.sheetAttackNum.Name = "sheetAttackNum";
            this.sheetAttackNum.Size = new System.Drawing.Size(228, 23);
            this.sheetAttackNum.TabIndex = 182;
            this.sheetAttackNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetAttack
            // 
            this.labelSheetAttack.BackColor = System.Drawing.Color.Transparent;
            this.labelSheetAttack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetAttack.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelSheetAttack.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelSheetAttack.Location = new System.Drawing.Point(0, 109);
            this.labelSheetAttack.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetAttack.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelSheetAttack.Name = "labelSheetAttack";
            this.labelSheetAttack.Size = new System.Drawing.Size(100, 23);
            this.labelSheetAttack.TabIndex = 182;
            this.labelSheetAttack.Text = "SheetAttack";
            this.labelSheetAttack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSpread
            // 
            this.labelSpread.BackColor = System.Drawing.Color.Transparent;
            this.labelSpread.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSpread.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelSpread.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelSpread.Location = new System.Drawing.Point(0, 86);
            this.labelSpread.Margin = new System.Windows.Forms.Padding(0);
            this.labelSpread.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelSpread.Name = "labelSpread";
            this.labelSpread.Size = new System.Drawing.Size(100, 23);
            this.labelSpread.TabIndex = 182;
            this.labelSpread.Text = "Spread";
            this.labelSpread.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDielectric
            // 
            this.labelDielectric.BackColor = System.Drawing.Color.Transparent;
            this.labelDielectric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDielectric.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelDielectric.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelDielectric.Location = new System.Drawing.Point(0, 132);
            this.labelDielectric.Margin = new System.Windows.Forms.Padding(0);
            this.labelDielectric.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelDielectric.Name = "labelDielectric";
            this.labelDielectric.Size = new System.Drawing.Size(100, 23);
            this.labelDielectric.TabIndex = 182;
            this.labelDielectric.Text = "Dielectric";
            this.labelDielectric.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSticker
            // 
            this.labelSticker.BackColor = System.Drawing.Color.Transparent;
            this.labelSticker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSticker.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelSticker.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelSticker.Location = new System.Drawing.Point(0, 155);
            this.labelSticker.Margin = new System.Windows.Forms.Padding(0);
            this.labelSticker.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelSticker.Name = "labelSticker";
            this.labelSticker.Size = new System.Drawing.Size(100, 23);
            this.labelSticker.TabIndex = 182;
            this.labelSticker.Text = "Sticker";
            this.labelSticker.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMargin
            // 
            this.labelMargin.BackColor = System.Drawing.Color.Transparent;
            this.labelMargin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMargin.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold);
            this.labelMargin.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelMargin.Location = new System.Drawing.Point(0, 178);
            this.labelMargin.Margin = new System.Windows.Forms.Padding(0);
            this.labelMargin.MinimumSize = new System.Drawing.Size(0, 23);
            this.labelMargin.Name = "labelMargin";
            this.labelMargin.Size = new System.Drawing.Size(100, 23);
            this.labelMargin.TabIndex = 182;
            this.labelMargin.Text = "Margin";
            this.labelMargin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // noPrintNum
            // 
            this.noPrintNum.AutoSize = true;
            this.noPrintNum.BackColor = System.Drawing.Color.White;
            this.noPrintNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noPrintNum.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.noPrintNum.Location = new System.Drawing.Point(100, 40);
            this.noPrintNum.Margin = new System.Windows.Forms.Padding(0);
            this.noPrintNum.MinimumSize = new System.Drawing.Size(20, 20);
            this.noPrintNum.Name = "noPrintNum";
            this.noPrintNum.Size = new System.Drawing.Size(228, 23);
            this.noPrintNum.TabIndex = 182;
            this.noPrintNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pinHoleNum
            // 
            this.pinHoleNum.AutoSize = true;
            this.pinHoleNum.BackColor = System.Drawing.Color.White;
            this.pinHoleNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pinHoleNum.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.pinHoleNum.Location = new System.Drawing.Point(100, 63);
            this.pinHoleNum.Margin = new System.Windows.Forms.Padding(0);
            this.pinHoleNum.MinimumSize = new System.Drawing.Size(20, 20);
            this.pinHoleNum.Name = "pinHoleNum";
            this.pinHoleNum.Size = new System.Drawing.Size(228, 23);
            this.pinHoleNum.TabIndex = 184;
            this.pinHoleNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // spreadNum
            // 
            this.spreadNum.AutoSize = true;
            this.spreadNum.BackColor = System.Drawing.Color.White;
            this.spreadNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadNum.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.spreadNum.Location = new System.Drawing.Point(100, 86);
            this.spreadNum.Margin = new System.Windows.Forms.Padding(0);
            this.spreadNum.MinimumSize = new System.Drawing.Size(20, 20);
            this.spreadNum.Name = "spreadNum";
            this.spreadNum.Size = new System.Drawing.Size(228, 23);
            this.spreadNum.TabIndex = 183;
            this.spreadNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // marginNum
            // 
            this.marginNum.AutoSize = true;
            this.marginNum.BackColor = System.Drawing.Color.White;
            this.marginNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.marginNum.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.marginNum.Location = new System.Drawing.Point(100, 178);
            this.marginNum.Margin = new System.Windows.Forms.Padding(0);
            this.marginNum.MinimumSize = new System.Drawing.Size(20, 20);
            this.marginNum.Name = "marginNum";
            this.marginNum.Size = new System.Drawing.Size(228, 23);
            this.marginNum.TabIndex = 183;
            this.marginNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // stickerNum
            // 
            this.stickerNum.AutoSize = true;
            this.stickerNum.BackColor = System.Drawing.Color.White;
            this.stickerNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stickerNum.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.stickerNum.Location = new System.Drawing.Point(100, 155);
            this.stickerNum.Margin = new System.Windows.Forms.Padding(0);
            this.stickerNum.MinimumSize = new System.Drawing.Size(20, 20);
            this.stickerNum.Name = "stickerNum";
            this.stickerNum.Size = new System.Drawing.Size(228, 23);
            this.stickerNum.TabIndex = 183;
            this.stickerNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dielectricNum
            // 
            this.dielectricNum.AutoSize = true;
            this.dielectricNum.BackColor = System.Drawing.Color.White;
            this.dielectricNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dielectricNum.Font = new System.Drawing.Font("맑은 고딕", 11F);
            this.dielectricNum.Location = new System.Drawing.Point(100, 132);
            this.dielectricNum.Margin = new System.Windows.Forms.Padding(0);
            this.dielectricNum.MinimumSize = new System.Drawing.Size(20, 20);
            this.dielectricNum.Name = "dielectricNum";
            this.dielectricNum.Size = new System.Drawing.Size(228, 23);
            this.dielectricNum.TabIndex = 183;
            this.dielectricNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelResult
            // 
            this.panelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelResult.Location = new System.Drawing.Point(330, 0);
            this.panelResult.Margin = new System.Windows.Forms.Padding(0);
            this.panelResult.Name = "panelResult";
            this.panelResult.Size = new System.Drawing.Size(876, 681);
            this.panelResult.TabIndex = 91;
            // 
            // partialProjectionToolStripMenuItem
            // 
            this.partialProjectionToolStripMenuItem.Name = "partialProjectionToolStripMenuItem";
            this.partialProjectionToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.partialProjectionToolStripMenuItem.Text = "PartialProjection";
            this.partialProjectionToolStripMenuItem.Click += new System.EventHandler(this.buttonExportPartialProjection_Click);
            // 
            // ReportPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.layoutMain);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Name = "ReportPanel";
            this.Size = new System.Drawing.Size(1206, 831);
            this.Load += new System.EventHandler(this.ReportPanel_Load);
            this.layoutMain.ResumeLayout(false);
            this.layoutTop.ResumeLayout(false);
            this.layoutAdvance.ResumeLayout(false);
            this.layoutAdvance.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.layoutFilter.ResumeLayout(false);
            this.layoutFilter.PerformLayout();
            this.layoutSelectFilter.ResumeLayout(false);
            this.layoutSelectFilter.PerformLayout();
            this.panelSelectCam.ResumeLayout(false);
            this.panelSelectCam.PerformLayout();
            this.layoutSize.ResumeLayout(false);
            this.layoutSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMin)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rectCY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rectCX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rectH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rectW)).EndInit();
            this.layoutbottom.ResumeLayout(false);
            this.layoutbottom.PerformLayout();
            this.layoutLeft.ResumeLayout(false);
            this.layoutLeft.PerformLayout();
            this.tableLayoutPanelSheetList.ResumeLayout(false);
            this.tableLayoutPanelSheetList.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.sheetLength.ResumeLayout(false);
            this.sheetLength.PerformLayout();
            this.tableLayoutPanelSheetLenDetail.ResumeLayout(false);
            this.tableLayoutPanelSheetLenDetail.PerformLayout();
            this.panelProductionTargetSpd.ResumeLayout(false);
            this.panelProductionTargetSpd.PerformLayout();
            this.tableLayoutPanelDefectList.ResumeLayout(false);
            this.tableLayoutPanelDefectList.PerformLayout();
            this.defectInfoPanel.ResumeLayout(false);
            this.defectInfoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFlowLayoutManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.TableLayoutPanel layoutbottom;
        private System.Windows.Forms.CheckBox ngFilter;
        private System.Windows.Forms.CheckBox okFilter;
        private System.Windows.Forms.Label labelPatternList;
        private System.Windows.Forms.Label labelSheetEraser;
        private System.Windows.Forms.Label sheetRatio;
        private System.Windows.Forms.Label labelSheetInspector;
        private System.Windows.Forms.Label labelSheetRatio;
        private System.Windows.Forms.Label sheetEraser;
        private System.Windows.Forms.Label sheetInspector;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.TableLayoutPanel layoutTop;
        private System.Windows.Forms.TableLayoutPanel layoutAdvance;
        private System.Windows.Forms.Label labelExport;
        private System.Windows.Forms.TableLayoutPanel layoutFilter;
        private System.Windows.Forms.TableLayoutPanel layoutSelectFilter;
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
        private System.Windows.Forms.DataGridView sheetList;
        private System.Windows.Forms.CheckBox useSize;
        private System.Windows.Forms.Label labelProductionLotName;
        private System.Windows.Forms.Label labelProductionTime;
        private System.Windows.Forms.TextBox productionTime;
        private System.Windows.Forms.TextBox productionLotName;
        private System.Windows.Forms.Label labelCam;
        private System.Windows.Forms.Panel panelSelectCam;
        private System.Windows.Forms.CheckBox checkBoxCam;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label productionTargetSpd;
        private System.Windows.Forms.Label labelProductionTargetSpd;
        private System.Windows.Forms.Panel panelResult;
        private System.Windows.Forms.Label labelProductionModelName;
        private System.Windows.Forms.TextBox productionModelName;
        private Infragistics.Win.Misc.UltraButton buttonWindowCapture;
        private System.Windows.Forms.Label labelWindowCapture;
        private System.Windows.Forms.TableLayoutPanel layoutLeft;
        private System.Windows.Forms.Label noPrintNum;
        private System.Windows.Forms.Label dielectricNum;
        private System.Windows.Forms.Label pinHoleNum;
        private System.Windows.Forms.Label sheetAttackNum;
        private System.Windows.Forms.Label labelPinHole;
        private System.Windows.Forms.Label labelDielectric;
        private System.Windows.Forms.Label labelNoPrint;
        private System.Windows.Forms.Panel defectInfoPanel;
        public System.Windows.Forms.Label labelDefects;
        private System.Windows.Forms.RadioButton patternRadioButton;
        private System.Windows.Forms.RadioButton defectRadioButton;
        private System.Windows.Forms.Label labelSheetAttack;
        private Infragistics.Win.Misc.UltraFlowLayoutManager ultraFlowLayoutManager1;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Label labelSheetLength;
        private System.Windows.Forms.TableLayoutPanel sheetLength;
        private System.Windows.Forms.Label infoHeight1;
        private System.Windows.Forms.Label infoHeight2;
        private System.Windows.Forms.Label infoHeight3;
        private System.Windows.Forms.Label labelSticker;
        private System.Windows.Forms.Label stickerNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPattern;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnErased;
        private System.Windows.Forms.Label labelSpread;
        private System.Windows.Forms.Label spreadNum;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem defectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lengthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem marginToolStripMenuItem;
        private Infragistics.Win.Misc.UltraButton buttonExport;
        private System.Windows.Forms.Label marginNum;
        private System.Windows.Forms.Label labelMargin;
        private System.Windows.Forms.ToolStripMenuItem offsetToolStripMenuItem;
        private System.Windows.Forms.Label labelFilterRect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.NumericUpDown rectCY;
        private System.Windows.Forms.CheckBox useRect;
        private System.Windows.Forms.NumericUpDown rectCX;
        private System.Windows.Forms.NumericUpDown rectH;
        private System.Windows.Forms.NumericUpDown rectW;
        private System.Windows.Forms.Panel panelSelectType;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSheetList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDefectList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        public System.Windows.Forms.Label labelDefectsUnit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelProductionTargetSpdUnit;
        private System.Windows.Forms.Panel panelProductionTargetSpd;
        private System.Windows.Forms.Label labelSheetLengthMean;
        private System.Windows.Forms.Label labelSheetLengthStdDev;
        private System.Windows.Forms.Label labelSheetLengthMin;
        private System.Windows.Forms.Label labelSheetLengthMax;
        private System.Windows.Forms.Label labelSheetLengthDiff;
        private System.Windows.Forms.Label infoHeightMean;
        private System.Windows.Forms.Label infoHeightStdDev;
        private System.Windows.Forms.Label infoHeightMax;
        private System.Windows.Forms.Label infoHeightMin;
        private System.Windows.Forms.Label infoHeightRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSheetLenDetail;
        private System.Windows.Forms.Label labelSheetLengthLow10p;
        private System.Windows.Forms.Label labelSheetLengthMid80p;
        private System.Windows.Forms.Label labelSheetLengthHigh10p;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelSheetLengthUnit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolStripMenuItem partialProjectionToolStripMenuItem;
    }
}
