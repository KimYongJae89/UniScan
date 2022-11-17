namespace UniScanS.Screen.UI.Report
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.layoutInfo = new System.Windows.Forms.TableLayoutPanel();
            this.layoutGradeInfo = new System.Windows.Forms.TableLayoutPanel();
            this.gradeSheetAttackTotal = new System.Windows.Forms.Label();
            this.labelGradeTotal = new System.Windows.Forms.Label();
            this.labelGradeSheetAttack = new System.Windows.Forms.Label();
            this.labelGradeSheet = new System.Windows.Forms.Label();
            this.gradeSheetAttackSheet = new System.Windows.Forms.Label();
            this.gradePoleTotal = new System.Windows.Forms.Label();
            this.gradePoleSheet = new System.Windows.Forms.Label();
            this.gradeDielectricTotal = new System.Windows.Forms.Label();
            this.gradeDielectricSheet = new System.Windows.Forms.Label();
            this.gradePinHoleTotal = new System.Windows.Forms.Label();
            this.gradePinHoleSheet = new System.Windows.Forms.Label();
            this.gradeShapeTotal = new System.Windows.Forms.Label();
            this.gradeShapeSheet = new System.Windows.Forms.Label();
            this.labelGradePole = new System.Windows.Forms.Label();
            this.labelGradeDielectric = new System.Windows.Forms.Label();
            this.labelGradePinHole = new System.Windows.Forms.Label();
            this.labelGradeShape = new System.Windows.Forms.Label();
            this.labelGradeDefect = new System.Windows.Forms.Label();
            this.gradeDefectTotal = new System.Windows.Forms.Label();
            this.gradeDefectSheet = new System.Windows.Forms.Label();
            this.labelTeachInfo = new System.Windows.Forms.Label();
            this.layoutModel = new System.Windows.Forms.TableLayoutPanel();
            this.labelModelName = new System.Windows.Forms.Label();
            this.labelModelThickness = new System.Windows.Forms.Label();
            this.modelPaste = new System.Windows.Forms.Label();
            this.labelModelPaste = new System.Windows.Forms.Label();
            this.labelModel = new System.Windows.Forms.Label();
            this.labelProduction = new System.Windows.Forms.Label();
            this.layoutProduction = new System.Windows.Forms.TableLayoutPanel();
            this.labelProductionLotName = new System.Windows.Forms.Label();
            this.labelProductionStartTime = new System.Windows.Forms.Label();
            this.productionStartTime = new System.Windows.Forms.Label();
            this.productionLotName = new System.Windows.Forms.Label();
            this.productionEndTime = new System.Windows.Forms.Label();
            this.labelProductionEndTime = new System.Windows.Forms.Label();
            this.layoutTop = new System.Windows.Forms.TableLayoutPanel();
            this.buttonCapture = new Infragistics.Win.Misc.UltraButton();
            this.buttonFolder = new Infragistics.Win.Misc.UltraButton();
            this.labelCapture = new System.Windows.Forms.Label();
            this.labelFilterTitle = new System.Windows.Forms.Label();
            this.labelFolder = new System.Windows.Forms.Label();
            this.labelCam = new System.Windows.Forms.Label();
            this.labelSize = new System.Windows.Forms.Label();
            this.panelSelectCam = new System.Windows.Forms.Panel();
            this.checkBoxCam = new System.Windows.Forms.CheckBox();
            this.labelType = new System.Windows.Forms.Label();
            this.layoutType = new System.Windows.Forms.TableLayoutPanel();
            this.shape = new System.Windows.Forms.RadioButton();
            this.pinHole = new System.Windows.Forms.RadioButton();
            this.dielectric = new System.Windows.Forms.RadioButton();
            this.pole = new System.Windows.Forms.RadioButton();
            this.sheetAttack = new System.Windows.Forms.RadioButton();
            this.total = new System.Windows.Forms.RadioButton();
            this.layoutSize = new System.Windows.Forms.TableLayoutPanel();
            this.labelMaxUnit = new System.Windows.Forms.Label();
            this.sizeMax = new System.Windows.Forms.NumericUpDown();
            this.labelMinUnit = new System.Windows.Forms.Label();
            this.labelMax = new System.Windows.Forms.Label();
            this.sizeMin = new System.Windows.Forms.NumericUpDown();
            this.labelMin = new System.Windows.Forms.Label();
            this.useSize = new System.Windows.Forms.CheckBox();
            this.layoutBottom = new System.Windows.Forms.TableLayoutPanel();
            this.layoutSheetList = new System.Windows.Forms.TableLayoutPanel();
            this.sheetList = new System.Windows.Forms.DataGridView();
            this.columnPattern = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonSelectAll = new Infragistics.Win.Misc.UltraButton();
            this.layoutResultFilter = new System.Windows.Forms.TableLayoutPanel();
            this.errorFilter = new System.Windows.Forms.CheckBox();
            this.ngFilter = new System.Windows.Forms.CheckBox();
            this.okFilter = new System.Windows.Forms.CheckBox();
            this.layoutSheetInfo = new System.Windows.Forms.TableLayoutPanel();
            this.labelSheetTotal = new System.Windows.Forms.Label();
            this.labelSheetNG = new System.Windows.Forms.Label();
            this.sheetNG = new System.Windows.Forms.Label();
            this.sheetTotal = new System.Windows.Forms.Label();
            this.sheetRatio = new System.Windows.Forms.Label();
            this.labelSheetRatio = new System.Windows.Forms.Label();
            this.labelUnit = new System.Windows.Forms.Label();
            this.labelSheetList = new System.Windows.Forms.Label();
            this.layoutImage = new System.Windows.Forms.TableLayoutPanel();
            this.labelImage = new System.Windows.Forms.Label();
            this.imagePanel = new System.Windows.Forms.Panel();
            this.layoutDefectList = new System.Windows.Forms.TableLayoutPanel();
            this.defectImage = new System.Windows.Forms.PictureBox();
            this.labelDefectImage = new System.Windows.Forms.Label();
            this.defectList = new System.Windows.Forms.DataGridView();
            this.columnCamIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDefectType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelDefectList = new System.Windows.Forms.Label();
            this.layoutDefectInfo = new System.Windows.Forms.TableLayoutPanel();
            this.panelShape = new System.Windows.Forms.Panel();
            this.layoutShape = new System.Windows.Forms.TableLayoutPanel();
            this.labelCenterX = new System.Windows.Forms.Label();
            this.labelHeight = new System.Windows.Forms.Label();
            this.labelWidth = new System.Windows.Forms.Label();
            this.labelCenterY = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.Label();
            this.height = new System.Windows.Forms.Label();
            this.centerX = new System.Windows.Forms.Label();
            this.centerY = new System.Windows.Forms.Label();
            this.area = new System.Windows.Forms.Label();
            this.labelArea = new System.Windows.Forms.Label();
            this.panelCommon = new System.Windows.Forms.Panel();
            this.layoutCommonDefect = new System.Windows.Forms.TableLayoutPanel();
            this.labelLengthUnit = new System.Windows.Forms.Label();
            this.labelElongation = new System.Windows.Forms.Label();
            this.labelUpper = new System.Windows.Forms.Label();
            this.labelLower = new System.Windows.Forms.Label();
            this.labelCompactness = new System.Windows.Forms.Label();
            this.lower = new System.Windows.Forms.Label();
            this.upper = new System.Windows.Forms.Label();
            this.elongation = new System.Windows.Forms.Label();
            this.compactness = new System.Windows.Forms.Label();
            this.lenght = new System.Windows.Forms.Label();
            this.labelLength = new System.Windows.Forms.Label();
            this.modelThickness = new System.Windows.Forms.Label();
            this.modelName = new System.Windows.Forms.Label();
            this.layoutMain.SuspendLayout();
            this.layoutInfo.SuspendLayout();
            this.layoutGradeInfo.SuspendLayout();
            this.layoutModel.SuspendLayout();
            this.layoutProduction.SuspendLayout();
            this.layoutTop.SuspendLayout();
            this.panelSelectCam.SuspendLayout();
            this.layoutType.SuspendLayout();
            this.layoutSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMin)).BeginInit();
            this.layoutBottom.SuspendLayout();
            this.layoutSheetList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetList)).BeginInit();
            this.layoutResultFilter.SuspendLayout();
            this.layoutSheetInfo.SuspendLayout();
            this.layoutImage.SuspendLayout();
            this.layoutDefectList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defectImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.defectList)).BeginInit();
            this.layoutDefectInfo.SuspendLayout();
            this.panelShape.SuspendLayout();
            this.layoutShape.SuspendLayout();
            this.panelCommon.SuspendLayout();
            this.layoutCommonDefect.SuspendLayout();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 2;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.layoutInfo, 0, 0);
            this.layoutMain.Controls.Add(this.layoutTop, 1, 0);
            this.layoutMain.Controls.Add(this.layoutBottom, 1, 1);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 2;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Size = new System.Drawing.Size(1206, 928);
            this.layoutMain.TabIndex = 0;
            // 
            // layoutInfo
            // 
            this.layoutInfo.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutInfo.ColumnCount = 1;
            this.layoutInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutInfo.Controls.Add(this.layoutGradeInfo, 0, 5);
            this.layoutInfo.Controls.Add(this.labelTeachInfo, 0, 4);
            this.layoutInfo.Controls.Add(this.layoutModel, 0, 1);
            this.layoutInfo.Controls.Add(this.labelModel, 0, 0);
            this.layoutInfo.Controls.Add(this.labelProduction, 0, 2);
            this.layoutInfo.Controls.Add(this.layoutProduction, 0, 3);
            this.layoutInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutInfo.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.layoutInfo.Location = new System.Drawing.Point(0, 0);
            this.layoutInfo.Margin = new System.Windows.Forms.Padding(0);
            this.layoutInfo.Name = "layoutInfo";
            this.layoutInfo.RowCount = 7;
            this.layoutMain.SetRowSpan(this.layoutInfo, 2);
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 325F));
            this.layoutInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutInfo.Size = new System.Drawing.Size(250, 928);
            this.layoutInfo.TabIndex = 145;
            // 
            // layoutGradeInfo
            // 
            this.layoutGradeInfo.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutGradeInfo.ColumnCount = 3;
            this.layoutGradeInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.layoutGradeInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutGradeInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutGradeInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutGradeInfo.Controls.Add(this.gradeSheetAttackTotal, 1, 2);
            this.layoutGradeInfo.Controls.Add(this.labelGradeTotal, 1, 0);
            this.layoutGradeInfo.Controls.Add(this.labelGradeSheetAttack, 0, 2);
            this.layoutGradeInfo.Controls.Add(this.labelGradeSheet, 2, 0);
            this.layoutGradeInfo.Controls.Add(this.gradeSheetAttackSheet, 2, 2);
            this.layoutGradeInfo.Controls.Add(this.gradePoleTotal, 1, 3);
            this.layoutGradeInfo.Controls.Add(this.gradePoleSheet, 2, 3);
            this.layoutGradeInfo.Controls.Add(this.gradeDielectricTotal, 1, 4);
            this.layoutGradeInfo.Controls.Add(this.gradeDielectricSheet, 2, 4);
            this.layoutGradeInfo.Controls.Add(this.gradePinHoleTotal, 1, 5);
            this.layoutGradeInfo.Controls.Add(this.gradePinHoleSheet, 2, 5);
            this.layoutGradeInfo.Controls.Add(this.gradeShapeTotal, 1, 6);
            this.layoutGradeInfo.Controls.Add(this.gradeShapeSheet, 2, 6);
            this.layoutGradeInfo.Controls.Add(this.labelGradePole, 0, 3);
            this.layoutGradeInfo.Controls.Add(this.labelGradeDielectric, 0, 4);
            this.layoutGradeInfo.Controls.Add(this.labelGradePinHole, 0, 5);
            this.layoutGradeInfo.Controls.Add(this.labelGradeShape, 0, 6);
            this.layoutGradeInfo.Controls.Add(this.labelGradeDefect, 0, 1);
            this.layoutGradeInfo.Controls.Add(this.gradeDefectTotal, 1, 1);
            this.layoutGradeInfo.Controls.Add(this.gradeDefectSheet, 2, 1);
            this.layoutGradeInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutGradeInfo.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.layoutGradeInfo.Location = new System.Drawing.Point(1, 304);
            this.layoutGradeInfo.Margin = new System.Windows.Forms.Padding(0);
            this.layoutGradeInfo.Name = "layoutGradeInfo";
            this.layoutGradeInfo.RowCount = 7;
            this.layoutGradeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.layoutGradeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66528F));
            this.layoutGradeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66695F));
            this.layoutGradeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66694F));
            this.layoutGradeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66694F));
            this.layoutGradeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66694F));
            this.layoutGradeInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66694F));
            this.layoutGradeInfo.Size = new System.Drawing.Size(248, 325);
            this.layoutGradeInfo.TabIndex = 183;
            // 
            // gradeSheetAttackTotal
            // 
            this.gradeSheetAttackTotal.BackColor = System.Drawing.Color.White;
            this.gradeSheetAttackTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradeSheetAttackTotal.Location = new System.Drawing.Point(77, 122);
            this.gradeSheetAttackTotal.Margin = new System.Windows.Forms.Padding(0);
            this.gradeSheetAttackTotal.Name = "gradeSheetAttackTotal";
            this.gradeSheetAttackTotal.Size = new System.Drawing.Size(84, 39);
            this.gradeSheetAttackTotal.TabIndex = 4;
            this.gradeSheetAttackTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGradeTotal
            // 
            this.labelGradeTotal.AutoSize = true;
            this.labelGradeTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGradeTotal.Location = new System.Drawing.Point(77, 1);
            this.labelGradeTotal.Margin = new System.Windows.Forms.Padding(0);
            this.labelGradeTotal.Name = "labelGradeTotal";
            this.labelGradeTotal.Size = new System.Drawing.Size(84, 80);
            this.labelGradeTotal.TabIndex = 15;
            this.labelGradeTotal.Text = "Total (EA)";
            this.labelGradeTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGradeSheetAttack
            // 
            this.labelGradeSheetAttack.AutoSize = true;
            this.labelGradeSheetAttack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGradeSheetAttack.Location = new System.Drawing.Point(1, 122);
            this.labelGradeSheetAttack.Margin = new System.Windows.Forms.Padding(0);
            this.labelGradeSheetAttack.Name = "labelGradeSheetAttack";
            this.labelGradeSheetAttack.Size = new System.Drawing.Size(75, 39);
            this.labelGradeSheetAttack.TabIndex = 1;
            this.labelGradeSheetAttack.Text = "Sheet Attack";
            this.labelGradeSheetAttack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGradeSheet
            // 
            this.labelGradeSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGradeSheet.Location = new System.Drawing.Point(162, 1);
            this.labelGradeSheet.Margin = new System.Windows.Forms.Padding(0);
            this.labelGradeSheet.Name = "labelGradeSheet";
            this.labelGradeSheet.Size = new System.Drawing.Size(85, 80);
            this.labelGradeSheet.TabIndex = 14;
            this.labelGradeSheet.Text = "Sheet (Print)";
            this.labelGradeSheet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradeSheetAttackSheet
            // 
            this.gradeSheetAttackSheet.BackColor = System.Drawing.Color.White;
            this.gradeSheetAttackSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradeSheetAttackSheet.Location = new System.Drawing.Point(162, 122);
            this.gradeSheetAttackSheet.Margin = new System.Windows.Forms.Padding(0);
            this.gradeSheetAttackSheet.Name = "gradeSheetAttackSheet";
            this.gradeSheetAttackSheet.Size = new System.Drawing.Size(85, 39);
            this.gradeSheetAttackSheet.TabIndex = 2;
            this.gradeSheetAttackSheet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradePoleTotal
            // 
            this.gradePoleTotal.BackColor = System.Drawing.Color.White;
            this.gradePoleTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradePoleTotal.Location = new System.Drawing.Point(77, 162);
            this.gradePoleTotal.Margin = new System.Windows.Forms.Padding(0);
            this.gradePoleTotal.Name = "gradePoleTotal";
            this.gradePoleTotal.Size = new System.Drawing.Size(84, 39);
            this.gradePoleTotal.TabIndex = 9;
            this.gradePoleTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradePoleSheet
            // 
            this.gradePoleSheet.BackColor = System.Drawing.Color.White;
            this.gradePoleSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradePoleSheet.Location = new System.Drawing.Point(162, 162);
            this.gradePoleSheet.Margin = new System.Windows.Forms.Padding(0);
            this.gradePoleSheet.Name = "gradePoleSheet";
            this.gradePoleSheet.Size = new System.Drawing.Size(85, 39);
            this.gradePoleSheet.TabIndex = 6;
            this.gradePoleSheet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradeDielectricTotal
            // 
            this.gradeDielectricTotal.BackColor = System.Drawing.Color.White;
            this.gradeDielectricTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradeDielectricTotal.Location = new System.Drawing.Point(77, 202);
            this.gradeDielectricTotal.Margin = new System.Windows.Forms.Padding(0);
            this.gradeDielectricTotal.Name = "gradeDielectricTotal";
            this.gradeDielectricTotal.Size = new System.Drawing.Size(84, 39);
            this.gradeDielectricTotal.TabIndex = 12;
            this.gradeDielectricTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradeDielectricSheet
            // 
            this.gradeDielectricSheet.BackColor = System.Drawing.Color.White;
            this.gradeDielectricSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradeDielectricSheet.Location = new System.Drawing.Point(162, 202);
            this.gradeDielectricSheet.Margin = new System.Windows.Forms.Padding(0);
            this.gradeDielectricSheet.Name = "gradeDielectricSheet";
            this.gradeDielectricSheet.Size = new System.Drawing.Size(85, 39);
            this.gradeDielectricSheet.TabIndex = 8;
            this.gradeDielectricSheet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradePinHoleTotal
            // 
            this.gradePinHoleTotal.BackColor = System.Drawing.Color.White;
            this.gradePinHoleTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradePinHoleTotal.Location = new System.Drawing.Point(77, 242);
            this.gradePinHoleTotal.Margin = new System.Windows.Forms.Padding(0);
            this.gradePinHoleTotal.Name = "gradePinHoleTotal";
            this.gradePinHoleTotal.Size = new System.Drawing.Size(84, 39);
            this.gradePinHoleTotal.TabIndex = 7;
            this.gradePinHoleTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradePinHoleSheet
            // 
            this.gradePinHoleSheet.BackColor = System.Drawing.Color.White;
            this.gradePinHoleSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradePinHoleSheet.Location = new System.Drawing.Point(162, 242);
            this.gradePinHoleSheet.Margin = new System.Windows.Forms.Padding(0);
            this.gradePinHoleSheet.Name = "gradePinHoleSheet";
            this.gradePinHoleSheet.Size = new System.Drawing.Size(85, 39);
            this.gradePinHoleSheet.TabIndex = 10;
            this.gradePinHoleSheet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradeShapeTotal
            // 
            this.gradeShapeTotal.BackColor = System.Drawing.Color.White;
            this.gradeShapeTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradeShapeTotal.Location = new System.Drawing.Point(77, 282);
            this.gradeShapeTotal.Margin = new System.Windows.Forms.Padding(0);
            this.gradeShapeTotal.Name = "gradeShapeTotal";
            this.gradeShapeTotal.Size = new System.Drawing.Size(84, 42);
            this.gradeShapeTotal.TabIndex = 12;
            this.gradeShapeTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradeShapeSheet
            // 
            this.gradeShapeSheet.BackColor = System.Drawing.Color.White;
            this.gradeShapeSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradeShapeSheet.Location = new System.Drawing.Point(162, 282);
            this.gradeShapeSheet.Margin = new System.Windows.Forms.Padding(0);
            this.gradeShapeSheet.Name = "gradeShapeSheet";
            this.gradeShapeSheet.Size = new System.Drawing.Size(85, 42);
            this.gradeShapeSheet.TabIndex = 13;
            this.gradeShapeSheet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGradePole
            // 
            this.labelGradePole.AutoSize = true;
            this.labelGradePole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGradePole.Location = new System.Drawing.Point(1, 162);
            this.labelGradePole.Margin = new System.Windows.Forms.Padding(0);
            this.labelGradePole.Name = "labelGradePole";
            this.labelGradePole.Size = new System.Drawing.Size(75, 39);
            this.labelGradePole.TabIndex = 3;
            this.labelGradePole.Text = "Pole";
            this.labelGradePole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGradeDielectric
            // 
            this.labelGradeDielectric.AutoSize = true;
            this.labelGradeDielectric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGradeDielectric.Location = new System.Drawing.Point(1, 202);
            this.labelGradeDielectric.Margin = new System.Windows.Forms.Padding(0);
            this.labelGradeDielectric.Name = "labelGradeDielectric";
            this.labelGradeDielectric.Size = new System.Drawing.Size(75, 39);
            this.labelGradeDielectric.TabIndex = 0;
            this.labelGradeDielectric.Text = "Dielectric";
            this.labelGradeDielectric.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGradePinHole
            // 
            this.labelGradePinHole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGradePinHole.Location = new System.Drawing.Point(1, 242);
            this.labelGradePinHole.Margin = new System.Windows.Forms.Padding(0);
            this.labelGradePinHole.Name = "labelGradePinHole";
            this.labelGradePinHole.Size = new System.Drawing.Size(75, 39);
            this.labelGradePinHole.TabIndex = 10;
            this.labelGradePinHole.Text = "Pin Hole";
            this.labelGradePinHole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGradeShape
            // 
            this.labelGradeShape.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGradeShape.Location = new System.Drawing.Point(1, 282);
            this.labelGradeShape.Margin = new System.Windows.Forms.Padding(0);
            this.labelGradeShape.Name = "labelGradeShape";
            this.labelGradeShape.Size = new System.Drawing.Size(75, 42);
            this.labelGradeShape.TabIndex = 7;
            this.labelGradeShape.Text = "Shape";
            this.labelGradeShape.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelGradeDefect
            // 
            this.labelGradeDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGradeDefect.Location = new System.Drawing.Point(1, 82);
            this.labelGradeDefect.Margin = new System.Windows.Forms.Padding(0);
            this.labelGradeDefect.Name = "labelGradeDefect";
            this.labelGradeDefect.Size = new System.Drawing.Size(75, 39);
            this.labelGradeDefect.TabIndex = 8;
            this.labelGradeDefect.Text = "Defect";
            this.labelGradeDefect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradeDefectTotal
            // 
            this.gradeDefectTotal.BackColor = System.Drawing.Color.White;
            this.gradeDefectTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradeDefectTotal.Location = new System.Drawing.Point(77, 82);
            this.gradeDefectTotal.Margin = new System.Windows.Forms.Padding(0);
            this.gradeDefectTotal.Name = "gradeDefectTotal";
            this.gradeDefectTotal.Size = new System.Drawing.Size(84, 39);
            this.gradeDefectTotal.TabIndex = 15;
            this.gradeDefectTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gradeDefectSheet
            // 
            this.gradeDefectSheet.BackColor = System.Drawing.Color.White;
            this.gradeDefectSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradeDefectSheet.Location = new System.Drawing.Point(162, 82);
            this.gradeDefectSheet.Margin = new System.Windows.Forms.Padding(0);
            this.gradeDefectSheet.Name = "gradeDefectSheet";
            this.gradeDefectSheet.Size = new System.Drawing.Size(85, 39);
            this.gradeDefectSheet.TabIndex = 16;
            this.gradeDefectSheet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTeachInfo
            // 
            this.labelTeachInfo.AutoSize = true;
            this.labelTeachInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.layoutInfo.SetColumnSpan(this.labelTeachInfo, 2);
            this.labelTeachInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTeachInfo.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelTeachInfo.Location = new System.Drawing.Point(1, 268);
            this.labelTeachInfo.Margin = new System.Windows.Forms.Padding(0);
            this.labelTeachInfo.Name = "labelTeachInfo";
            this.labelTeachInfo.Size = new System.Drawing.Size(248, 35);
            this.labelTeachInfo.TabIndex = 182;
            this.labelTeachInfo.Text = "Grade Info";
            this.labelTeachInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutModel
            // 
            this.layoutModel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutModel.ColumnCount = 2;
            this.layoutModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.layoutModel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutModel.Controls.Add(this.labelModelName, 0, 0);
            this.layoutModel.Controls.Add(this.labelModelThickness, 0, 1);
            this.layoutModel.Controls.Add(this.modelThickness, 1, 1);
            this.layoutModel.Controls.Add(this.modelName, 1, 0);
            this.layoutModel.Controls.Add(this.modelPaste, 1, 2);
            this.layoutModel.Controls.Add(this.labelModelPaste, 0, 2);
            this.layoutModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutModel.Location = new System.Drawing.Point(1, 37);
            this.layoutModel.Margin = new System.Windows.Forms.Padding(0);
            this.layoutModel.Name = "layoutModel";
            this.layoutModel.RowCount = 3;
            this.layoutModel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.layoutModel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.layoutModel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.layoutModel.Size = new System.Drawing.Size(248, 93);
            this.layoutModel.TabIndex = 73;
            // 
            // labelModelName
            // 
            this.labelModelName.AutoSize = true;
            this.labelModelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModelName.Location = new System.Drawing.Point(1, 1);
            this.labelModelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelModelName.Name = "labelModelName";
            this.labelModelName.Size = new System.Drawing.Size(75, 29);
            this.labelModelName.TabIndex = 1;
            this.labelModelName.Text = "Name";
            this.labelModelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelModelThickness
            // 
            this.labelModelThickness.AutoSize = true;
            this.labelModelThickness.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModelThickness.Location = new System.Drawing.Point(1, 31);
            this.labelModelThickness.Margin = new System.Windows.Forms.Padding(0);
            this.labelModelThickness.Name = "labelModelThickness";
            this.labelModelThickness.Size = new System.Drawing.Size(75, 29);
            this.labelModelThickness.TabIndex = 3;
            this.labelModelThickness.Text = "Thickness";
            this.labelModelThickness.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // modelPaste
            // 
            this.modelPaste.BackColor = System.Drawing.Color.White;
            this.modelPaste.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelPaste.Location = new System.Drawing.Point(77, 61);
            this.modelPaste.Margin = new System.Windows.Forms.Padding(0);
            this.modelPaste.Name = "modelPaste";
            this.modelPaste.Size = new System.Drawing.Size(170, 31);
            this.modelPaste.TabIndex = 2;
            this.modelPaste.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelModelPaste
            // 
            this.labelModelPaste.AutoSize = true;
            this.labelModelPaste.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModelPaste.Location = new System.Drawing.Point(1, 61);
            this.labelModelPaste.Margin = new System.Windows.Forms.Padding(0);
            this.labelModelPaste.Name = "labelModelPaste";
            this.labelModelPaste.Size = new System.Drawing.Size(75, 31);
            this.labelModelPaste.TabIndex = 0;
            this.labelModelPaste.Text = "Paste";
            this.labelModelPaste.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelModel
            // 
            this.labelModel.AutoSize = true;
            this.labelModel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModel.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelModel.Location = new System.Drawing.Point(1, 1);
            this.labelModel.Margin = new System.Windows.Forms.Padding(0);
            this.labelModel.Name = "labelModel";
            this.labelModel.Size = new System.Drawing.Size(248, 35);
            this.labelModel.TabIndex = 72;
            this.labelModel.Text = "Model";
            this.labelModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProduction
            // 
            this.labelProduction.AutoSize = true;
            this.labelProduction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelProduction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProduction.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelProduction.Location = new System.Drawing.Point(1, 131);
            this.labelProduction.Margin = new System.Windows.Forms.Padding(0);
            this.labelProduction.Name = "labelProduction";
            this.labelProduction.Size = new System.Drawing.Size(248, 35);
            this.labelProduction.TabIndex = 70;
            this.labelProduction.Text = "Production";
            this.labelProduction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutProduction
            // 
            this.layoutProduction.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutProduction.ColumnCount = 2;
            this.layoutProduction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.layoutProduction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutProduction.Controls.Add(this.labelProductionLotName, 0, 0);
            this.layoutProduction.Controls.Add(this.labelProductionStartTime, 0, 1);
            this.layoutProduction.Controls.Add(this.productionStartTime, 1, 1);
            this.layoutProduction.Controls.Add(this.productionLotName, 1, 0);
            this.layoutProduction.Controls.Add(this.productionEndTime, 1, 2);
            this.layoutProduction.Controls.Add(this.labelProductionEndTime, 0, 2);
            this.layoutProduction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutProduction.Location = new System.Drawing.Point(1, 167);
            this.layoutProduction.Margin = new System.Windows.Forms.Padding(0);
            this.layoutProduction.Name = "layoutProduction";
            this.layoutProduction.RowCount = 3;
            this.layoutProduction.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.layoutProduction.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.layoutProduction.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.layoutProduction.Size = new System.Drawing.Size(248, 100);
            this.layoutProduction.TabIndex = 71;
            // 
            // labelProductionLotName
            // 
            this.labelProductionLotName.AutoSize = true;
            this.labelProductionLotName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProductionLotName.Location = new System.Drawing.Point(1, 1);
            this.labelProductionLotName.Margin = new System.Windows.Forms.Padding(0);
            this.labelProductionLotName.Name = "labelProductionLotName";
            this.labelProductionLotName.Size = new System.Drawing.Size(75, 31);
            this.labelProductionLotName.TabIndex = 1;
            this.labelProductionLotName.Text = "Lot";
            this.labelProductionLotName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProductionStartTime
            // 
            this.labelProductionStartTime.AutoSize = true;
            this.labelProductionStartTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProductionStartTime.Location = new System.Drawing.Point(1, 33);
            this.labelProductionStartTime.Margin = new System.Windows.Forms.Padding(0);
            this.labelProductionStartTime.Name = "labelProductionStartTime";
            this.labelProductionStartTime.Size = new System.Drawing.Size(75, 32);
            this.labelProductionStartTime.TabIndex = 3;
            this.labelProductionStartTime.Text = "Start";
            this.labelProductionStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // productionStartTime
            // 
            this.productionStartTime.BackColor = System.Drawing.Color.White;
            this.productionStartTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productionStartTime.Location = new System.Drawing.Point(77, 33);
            this.productionStartTime.Margin = new System.Windows.Forms.Padding(0);
            this.productionStartTime.Name = "productionStartTime";
            this.productionStartTime.Size = new System.Drawing.Size(170, 32);
            this.productionStartTime.TabIndex = 4;
            this.productionStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // productionLotName
            // 
            this.productionLotName.BackColor = System.Drawing.Color.White;
            this.productionLotName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productionLotName.Location = new System.Drawing.Point(77, 1);
            this.productionLotName.Margin = new System.Windows.Forms.Padding(0);
            this.productionLotName.Name = "productionLotName";
            this.productionLotName.Size = new System.Drawing.Size(170, 31);
            this.productionLotName.TabIndex = 6;
            this.productionLotName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // productionEndTime
            // 
            this.productionEndTime.BackColor = System.Drawing.Color.White;
            this.productionEndTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.productionEndTime.Location = new System.Drawing.Point(77, 66);
            this.productionEndTime.Margin = new System.Windows.Forms.Padding(0);
            this.productionEndTime.Name = "productionEndTime";
            this.productionEndTime.Size = new System.Drawing.Size(170, 33);
            this.productionEndTime.TabIndex = 2;
            this.productionEndTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProductionEndTime
            // 
            this.labelProductionEndTime.AutoSize = true;
            this.labelProductionEndTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProductionEndTime.Location = new System.Drawing.Point(1, 66);
            this.labelProductionEndTime.Margin = new System.Windows.Forms.Padding(0);
            this.labelProductionEndTime.Name = "labelProductionEndTime";
            this.labelProductionEndTime.Size = new System.Drawing.Size(75, 33);
            this.labelProductionEndTime.TabIndex = 0;
            this.labelProductionEndTime.Text = "End";
            this.labelProductionEndTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutTop
            // 
            this.layoutTop.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutTop.ColumnCount = 4;
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.layoutTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.layoutTop.Controls.Add(this.buttonCapture, 2, 2);
            this.layoutTop.Controls.Add(this.buttonFolder, 3, 2);
            this.layoutTop.Controls.Add(this.labelCapture, 2, 0);
            this.layoutTop.Controls.Add(this.labelFilterTitle, 0, 0);
            this.layoutTop.Controls.Add(this.labelFolder, 3, 0);
            this.layoutTop.Controls.Add(this.labelCam, 0, 1);
            this.layoutTop.Controls.Add(this.labelSize, 0, 3);
            this.layoutTop.Controls.Add(this.panelSelectCam, 1, 1);
            this.layoutTop.Controls.Add(this.labelType, 0, 2);
            this.layoutTop.Controls.Add(this.layoutType, 1, 2);
            this.layoutTop.Controls.Add(this.layoutSize, 1, 3);
            this.layoutTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutTop.Location = new System.Drawing.Point(250, 0);
            this.layoutTop.Margin = new System.Windows.Forms.Padding(0);
            this.layoutTop.Name = "layoutTop";
            this.layoutTop.RowCount = 4;
            this.layoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutTop.Size = new System.Drawing.Size(956, 130);
            this.layoutTop.TabIndex = 143;
            // 
            // buttonCapture
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.Image = global::UniScanS.Properties.Resources.Cam;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.buttonCapture.Appearance = appearance1;
            this.buttonCapture.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonCapture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCapture.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonCapture.ImageSize = new System.Drawing.Size(50, 50);
            this.buttonCapture.Location = new System.Drawing.Point(784, 68);
            this.buttonCapture.Margin = new System.Windows.Forms.Padding(0);
            this.buttonCapture.Name = "buttonCapture";
            this.layoutTop.SetRowSpan(this.buttonCapture, 2);
            this.buttonCapture.Size = new System.Drawing.Size(85, 61);
            this.buttonCapture.TabIndex = 148;
            this.buttonCapture.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonCapture.Click += new System.EventHandler(this.buttonCapture_Click);
            // 
            // buttonFolder
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            appearance2.Image = global::UniScanS.Properties.Resources.picture_folder_32;
            appearance2.ImageHAlign = Infragistics.Win.HAlign.Center;
            this.buttonFolder.Appearance = appearance2;
            this.buttonFolder.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFolder.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonFolder.ImageSize = new System.Drawing.Size(40, 40);
            this.buttonFolder.Location = new System.Drawing.Point(870, 68);
            this.buttonFolder.Margin = new System.Windows.Forms.Padding(0);
            this.buttonFolder.Name = "buttonFolder";
            this.layoutTop.SetRowSpan(this.buttonFolder, 2);
            this.buttonFolder.Size = new System.Drawing.Size(85, 61);
            this.buttonFolder.TabIndex = 146;
            this.buttonFolder.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonFolder.Click += new System.EventHandler(this.buttonCam_Click);
            // 
            // labelCapture
            // 
            this.labelCapture.AutoSize = true;
            this.labelCapture.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelCapture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCapture.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelCapture.Location = new System.Drawing.Point(784, 1);
            this.labelCapture.Margin = new System.Windows.Forms.Padding(0);
            this.labelCapture.Name = "labelCapture";
            this.layoutTop.SetRowSpan(this.labelCapture, 2);
            this.labelCapture.Size = new System.Drawing.Size(85, 66);
            this.labelCapture.TabIndex = 147;
            this.labelCapture.Text = "Capture";
            this.labelCapture.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFilterTitle
            // 
            this.labelFilterTitle.AutoSize = true;
            this.labelFilterTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.layoutTop.SetColumnSpan(this.labelFilterTitle, 2);
            this.labelFilterTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFilterTitle.Font = new System.Drawing.Font("맑은 고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelFilterTitle.Location = new System.Drawing.Point(1, 1);
            this.labelFilterTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelFilterTitle.Name = "labelFilterTitle";
            this.labelFilterTitle.Size = new System.Drawing.Size(782, 35);
            this.labelFilterTitle.TabIndex = 1;
            this.labelFilterTitle.Text = "Filter";
            this.labelFilterTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFolder
            // 
            this.labelFolder.AutoSize = true;
            this.labelFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFolder.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelFolder.Location = new System.Drawing.Point(870, 1);
            this.labelFolder.Margin = new System.Windows.Forms.Padding(0);
            this.labelFolder.Name = "labelFolder";
            this.layoutTop.SetRowSpan(this.labelFolder, 2);
            this.labelFolder.Size = new System.Drawing.Size(85, 66);
            this.labelFolder.TabIndex = 2;
            this.labelFolder.Text = "Defect Folder";
            this.labelFolder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCam
            // 
            this.labelCam.AutoSize = true;
            this.labelCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCam.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelCam.Location = new System.Drawing.Point(1, 37);
            this.labelCam.Margin = new System.Windows.Forms.Padding(0);
            this.labelCam.Name = "labelCam";
            this.labelCam.Size = new System.Drawing.Size(100, 30);
            this.labelCam.TabIndex = 88;
            this.labelCam.Text = "Cam";
            this.labelCam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSize.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelSize.Location = new System.Drawing.Point(1, 99);
            this.labelSize.Margin = new System.Windows.Forms.Padding(0);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(100, 30);
            this.labelSize.TabIndex = 0;
            this.labelSize.Text = "Size";
            this.labelSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelSelectCam
            // 
            this.panelSelectCam.Controls.Add(this.checkBoxCam);
            this.panelSelectCam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSelectCam.Location = new System.Drawing.Point(102, 37);
            this.panelSelectCam.Margin = new System.Windows.Forms.Padding(0);
            this.panelSelectCam.Name = "panelSelectCam";
            this.panelSelectCam.Size = new System.Drawing.Size(681, 30);
            this.panelSelectCam.TabIndex = 87;
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
            this.checkBoxCam.Size = new System.Drawing.Size(76, 30);
            this.checkBoxCam.TabIndex = 0;
            this.checkBoxCam.Text = "Defualt";
            this.checkBoxCam.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxCam.UseVisualStyleBackColor = true;
            this.checkBoxCam.Visible = false;
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelType.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelType.Location = new System.Drawing.Point(1, 68);
            this.labelType.Margin = new System.Windows.Forms.Padding(0);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(100, 30);
            this.labelType.TabIndex = 0;
            this.labelType.Text = "Type";
            this.labelType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutType
            // 
            this.layoutType.ColumnCount = 7;
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutType.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutType.Controls.Add(this.shape, 5, 0);
            this.layoutType.Controls.Add(this.pinHole, 4, 0);
            this.layoutType.Controls.Add(this.dielectric, 3, 0);
            this.layoutType.Controls.Add(this.pole, 2, 0);
            this.layoutType.Controls.Add(this.sheetAttack, 1, 0);
            this.layoutType.Controls.Add(this.total, 0, 0);
            this.layoutType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutType.Location = new System.Drawing.Point(102, 68);
            this.layoutType.Margin = new System.Windows.Forms.Padding(0);
            this.layoutType.Name = "layoutType";
            this.layoutType.RowCount = 1;
            this.layoutType.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutType.Size = new System.Drawing.Size(681, 30);
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
            this.shape.Location = new System.Drawing.Point(550, 0);
            this.shape.Margin = new System.Windows.Forms.Padding(0);
            this.shape.Name = "shape";
            this.shape.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.shape.Size = new System.Drawing.Size(100, 30);
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
            this.pinHole.Location = new System.Drawing.Point(450, 0);
            this.pinHole.Margin = new System.Windows.Forms.Padding(0);
            this.pinHole.Name = "pinHole";
            this.pinHole.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.pinHole.Size = new System.Drawing.Size(100, 30);
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
            this.dielectric.Location = new System.Drawing.Point(350, 0);
            this.dielectric.Margin = new System.Windows.Forms.Padding(0);
            this.dielectric.Name = "dielectric";
            this.dielectric.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.dielectric.Size = new System.Drawing.Size(100, 30);
            this.dielectric.TabIndex = 0;
            this.dielectric.Text = "Dielectric";
            this.dielectric.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dielectric.UseVisualStyleBackColor = false;
            this.dielectric.CheckedChanged += new System.EventHandler(this.dielectric_CheckedChanged);
            // 
            // pole
            // 
            this.pole.Appearance = System.Windows.Forms.Appearance.Button;
            this.pole.AutoSize = true;
            this.pole.BackColor = System.Drawing.Color.Transparent;
            this.pole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pole.FlatAppearance.BorderSize = 0;
            this.pole.FlatAppearance.CheckedBackColor = System.Drawing.Color.CornflowerBlue;
            this.pole.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.pole.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pole.ForeColor = System.Drawing.Color.Red;
            this.pole.Location = new System.Drawing.Point(250, 0);
            this.pole.Margin = new System.Windows.Forms.Padding(0);
            this.pole.Name = "pole";
            this.pole.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.pole.Size = new System.Drawing.Size(100, 30);
            this.pole.TabIndex = 81;
            this.pole.Text = "Pole";
            this.pole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.pole.UseVisualStyleBackColor = false;
            this.pole.CheckedChanged += new System.EventHandler(this.pole_CheckedChanged);
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
            this.sheetAttack.Location = new System.Drawing.Point(100, 0);
            this.sheetAttack.Margin = new System.Windows.Forms.Padding(0);
            this.sheetAttack.Name = "sheetAttack";
            this.sheetAttack.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.sheetAttack.Size = new System.Drawing.Size(150, 30);
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
            this.total.Size = new System.Drawing.Size(100, 30);
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
            this.layoutSize.Location = new System.Drawing.Point(102, 99);
            this.layoutSize.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSize.Name = "layoutSize";
            this.layoutSize.RowCount = 1;
            this.layoutSize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSize.Size = new System.Drawing.Size(681, 30);
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
            this.labelMaxUnit.Size = new System.Drawing.Size(60, 30);
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
            this.sizeMax.Location = new System.Drawing.Point(320, 2);
            this.sizeMax.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
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
            this.labelMinUnit.Size = new System.Drawing.Size(50, 30);
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
            this.labelMax.Size = new System.Drawing.Size(60, 30);
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
            this.sizeMin.Location = new System.Drawing.Point(110, 2);
            this.sizeMin.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
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
            // labelMin
            // 
            this.labelMin.AutoSize = true;
            this.labelMin.BackColor = System.Drawing.Color.Transparent;
            this.labelMin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMin.Location = new System.Drawing.Point(50, 0);
            this.labelMin.Margin = new System.Windows.Forms.Padding(0);
            this.labelMin.Name = "labelMin";
            this.labelMin.Size = new System.Drawing.Size(60, 30);
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
            this.useSize.Size = new System.Drawing.Size(50, 30);
            this.useSize.TabIndex = 99;
            this.useSize.UseVisualStyleBackColor = true;
            this.useSize.CheckedChanged += new System.EventHandler(this.useSize_CheckedChanged);
            // 
            // layoutBottom
            // 
            this.layoutBottom.ColumnCount = 3;
            this.layoutBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 225F));
            this.layoutBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 275F));
            this.layoutBottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutBottom.Controls.Add(this.layoutSheetList, 0, 0);
            this.layoutBottom.Controls.Add(this.layoutImage, 2, 0);
            this.layoutBottom.Controls.Add(this.layoutDefectList, 1, 0);
            this.layoutBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutBottom.Location = new System.Drawing.Point(250, 130);
            this.layoutBottom.Margin = new System.Windows.Forms.Padding(0);
            this.layoutBottom.Name = "layoutBottom";
            this.layoutBottom.RowCount = 1;
            this.layoutBottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutBottom.Size = new System.Drawing.Size(956, 798);
            this.layoutBottom.TabIndex = 144;
            // 
            // layoutSheetList
            // 
            this.layoutSheetList.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutSheetList.ColumnCount = 1;
            this.layoutSheetList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSheetList.Controls.Add(this.sheetList, 0, 2);
            this.layoutSheetList.Controls.Add(this.buttonSelectAll, 0, 4);
            this.layoutSheetList.Controls.Add(this.layoutResultFilter, 0, 3);
            this.layoutSheetList.Controls.Add(this.layoutSheetInfo, 0, 1);
            this.layoutSheetList.Controls.Add(this.labelSheetList, 0, 0);
            this.layoutSheetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSheetList.Location = new System.Drawing.Point(0, 0);
            this.layoutSheetList.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSheetList.Name = "layoutSheetList";
            this.layoutSheetList.RowCount = 5;
            this.layoutSheetList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutSheetList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutSheetList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSheetList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutSheetList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutSheetList.Size = new System.Drawing.Size(225, 798);
            this.layoutSheetList.TabIndex = 0;
            // 
            // sheetList
            // 
            this.sheetList.AllowUserToAddRows = false;
            this.sheetList.AllowUserToDeleteRows = false;
            this.sheetList.AllowUserToResizeColumns = false;
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
            this.columnQty});
            this.sheetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.sheetList.Location = new System.Drawing.Point(1, 138);
            this.sheetList.Margin = new System.Windows.Forms.Padding(0);
            this.sheetList.Name = "sheetList";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.sheetList.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.sheetList.RowHeadersVisible = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.sheetList.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.sheetList.RowTemplate.Height = 23;
            this.sheetList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.sheetList.Size = new System.Drawing.Size(223, 587);
            this.sheetList.TabIndex = 1;
            this.sheetList.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.sheetList_CellMouseDown);
            this.sheetList.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.sheetList_CellMouseUp);
            this.sheetList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sheetList_KeyDown);
            this.sheetList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.sheetList_KeyUp);
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
            // buttonSelectAll
            // 
            appearance3.BackColor = System.Drawing.Color.White;
            this.buttonSelectAll.Appearance = appearance3;
            this.buttonSelectAll.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2013Button;
            this.buttonSelectAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSelectAll.Location = new System.Drawing.Point(1, 762);
            this.buttonSelectAll.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(223, 35);
            this.buttonSelectAll.TabIndex = 66;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // layoutResultFilter
            // 
            this.layoutResultFilter.ColumnCount = 3;
            this.layoutResultFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutResultFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutResultFilter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutResultFilter.Controls.Add(this.errorFilter, 2, 0);
            this.layoutResultFilter.Controls.Add(this.ngFilter, 1, 0);
            this.layoutResultFilter.Controls.Add(this.okFilter, 0, 0);
            this.layoutResultFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutResultFilter.Location = new System.Drawing.Point(1, 726);
            this.layoutResultFilter.Margin = new System.Windows.Forms.Padding(0);
            this.layoutResultFilter.Name = "layoutResultFilter";
            this.layoutResultFilter.RowCount = 1;
            this.layoutResultFilter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutResultFilter.Size = new System.Drawing.Size(223, 35);
            this.layoutResultFilter.TabIndex = 63;
            // 
            // errorFilter
            // 
            this.errorFilter.AutoSize = true;
            this.errorFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorFilter.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.errorFilter.Location = new System.Drawing.Point(151, 3);
            this.errorFilter.Name = "errorFilter";
            this.errorFilter.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.errorFilter.Size = new System.Drawing.Size(69, 29);
            this.errorFilter.TabIndex = 2;
            this.errorFilter.Text = "Error";
            this.errorFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.errorFilter.UseVisualStyleBackColor = true;
            this.errorFilter.CheckedChanged += new System.EventHandler(this.errorFilter_CheckedChanged);
            // 
            // ngFilter
            // 
            this.ngFilter.AutoSize = true;
            this.ngFilter.Checked = true;
            this.ngFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ngFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ngFilter.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.ngFilter.Location = new System.Drawing.Point(77, 3);
            this.ngFilter.Name = "ngFilter";
            this.ngFilter.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.ngFilter.Size = new System.Drawing.Size(68, 29);
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
            this.okFilter.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Bold);
            this.okFilter.Location = new System.Drawing.Point(3, 3);
            this.okFilter.Name = "okFilter";
            this.okFilter.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.okFilter.Size = new System.Drawing.Size(68, 29);
            this.okFilter.TabIndex = 0;
            this.okFilter.Text = "OK";
            this.okFilter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.okFilter.UseVisualStyleBackColor = true;
            this.okFilter.CheckedChanged += new System.EventHandler(this.okFilter_CheckedChanged);
            // 
            // layoutSheetInfo
            // 
            this.layoutSheetInfo.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutSheetInfo.ColumnCount = 2;
            this.layoutSheetInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.layoutSheetInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSheetInfo.Controls.Add(this.labelSheetTotal, 0, 1);
            this.layoutSheetInfo.Controls.Add(this.labelSheetNG, 0, 2);
            this.layoutSheetInfo.Controls.Add(this.sheetNG, 1, 2);
            this.layoutSheetInfo.Controls.Add(this.sheetTotal, 1, 1);
            this.layoutSheetInfo.Controls.Add(this.sheetRatio, 1, 3);
            this.layoutSheetInfo.Controls.Add(this.labelSheetRatio, 0, 3);
            this.layoutSheetInfo.Controls.Add(this.labelUnit, 1, 0);
            this.layoutSheetInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutSheetInfo.Location = new System.Drawing.Point(1, 37);
            this.layoutSheetInfo.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSheetInfo.Name = "layoutSheetInfo";
            this.layoutSheetInfo.RowCount = 4;
            this.layoutSheetInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813F));
            this.layoutSheetInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.layoutSheetInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063F));
            this.layoutSheetInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063F));
            this.layoutSheetInfo.Size = new System.Drawing.Size(223, 100);
            this.layoutSheetInfo.TabIndex = 70;
            // 
            // labelSheetTotal
            // 
            this.labelSheetTotal.AutoSize = true;
            this.labelSheetTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetTotal.Location = new System.Drawing.Point(1, 25);
            this.labelSheetTotal.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetTotal.Name = "labelSheetTotal";
            this.labelSheetTotal.Size = new System.Drawing.Size(70, 23);
            this.labelSheetTotal.TabIndex = 1;
            this.labelSheetTotal.Text = "Total";
            this.labelSheetTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetNG
            // 
            this.labelSheetNG.AutoSize = true;
            this.labelSheetNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetNG.Location = new System.Drawing.Point(1, 49);
            this.labelSheetNG.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetNG.Name = "labelSheetNG";
            this.labelSheetNG.Size = new System.Drawing.Size(70, 23);
            this.labelSheetNG.TabIndex = 3;
            this.labelSheetNG.Text = "NG";
            this.labelSheetNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetNG
            // 
            this.sheetNG.AutoSize = true;
            this.sheetNG.BackColor = System.Drawing.Color.White;
            this.sheetNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetNG.Location = new System.Drawing.Point(72, 49);
            this.sheetNG.Margin = new System.Windows.Forms.Padding(0);
            this.sheetNG.Name = "sheetNG";
            this.sheetNG.Size = new System.Drawing.Size(150, 23);
            this.sheetNG.TabIndex = 4;
            this.sheetNG.Text = "0";
            this.sheetNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetTotal
            // 
            this.sheetTotal.AutoSize = true;
            this.sheetTotal.BackColor = System.Drawing.Color.White;
            this.sheetTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetTotal.Location = new System.Drawing.Point(72, 25);
            this.sheetTotal.Margin = new System.Windows.Forms.Padding(0);
            this.sheetTotal.Name = "sheetTotal";
            this.sheetTotal.Size = new System.Drawing.Size(150, 23);
            this.sheetTotal.TabIndex = 6;
            this.sheetTotal.Text = "0";
            this.sheetTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sheetRatio
            // 
            this.sheetRatio.AutoSize = true;
            this.sheetRatio.BackColor = System.Drawing.Color.White;
            this.sheetRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sheetRatio.Location = new System.Drawing.Point(72, 73);
            this.sheetRatio.Margin = new System.Windows.Forms.Padding(0);
            this.sheetRatio.Name = "sheetRatio";
            this.sheetRatio.Size = new System.Drawing.Size(150, 26);
            this.sheetRatio.TabIndex = 2;
            this.sheetRatio.Text = "0";
            this.sheetRatio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetRatio
            // 
            this.labelSheetRatio.AutoSize = true;
            this.labelSheetRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetRatio.Location = new System.Drawing.Point(1, 73);
            this.labelSheetRatio.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetRatio.Name = "labelSheetRatio";
            this.labelSheetRatio.Size = new System.Drawing.Size(70, 26);
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
            this.labelUnit.Location = new System.Drawing.Point(75, 1);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(144, 23);
            this.labelUnit.TabIndex = 68;
            this.labelUnit.Text = "Unit : Print";
            this.labelUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSheetList
            // 
            this.labelSheetList.AutoSize = true;
            this.labelSheetList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelSheetList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSheetList.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelSheetList.Location = new System.Drawing.Point(1, 1);
            this.labelSheetList.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetList.Name = "labelSheetList";
            this.labelSheetList.Size = new System.Drawing.Size(223, 35);
            this.labelSheetList.TabIndex = 0;
            this.labelSheetList.Text = "Sheet List";
            this.labelSheetList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutImage
            // 
            this.layoutImage.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutImage.ColumnCount = 1;
            this.layoutImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutImage.Controls.Add(this.labelImage, 0, 0);
            this.layoutImage.Controls.Add(this.imagePanel, 0, 1);
            this.layoutImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutImage.Location = new System.Drawing.Point(500, 0);
            this.layoutImage.Margin = new System.Windows.Forms.Padding(0);
            this.layoutImage.Name = "layoutImage";
            this.layoutImage.RowCount = 2;
            this.layoutImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutImage.Size = new System.Drawing.Size(456, 798);
            this.layoutImage.TabIndex = 90;
            // 
            // labelImage
            // 
            this.labelImage.AutoSize = true;
            this.labelImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImage.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelImage.Location = new System.Drawing.Point(1, 1);
            this.labelImage.Margin = new System.Windows.Forms.Padding(0);
            this.labelImage.Name = "labelImage";
            this.labelImage.Size = new System.Drawing.Size(454, 35);
            this.labelImage.TabIndex = 53;
            this.labelImage.Text = "Image";
            this.labelImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // imagePanel
            // 
            this.imagePanel.BackColor = System.Drawing.SystemColors.Control;
            this.imagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imagePanel.Location = new System.Drawing.Point(1, 37);
            this.imagePanel.Margin = new System.Windows.Forms.Padding(0);
            this.imagePanel.Name = "imagePanel";
            this.imagePanel.Size = new System.Drawing.Size(454, 760);
            this.imagePanel.TabIndex = 0;
            // 
            // layoutDefectList
            // 
            this.layoutDefectList.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutDefectList.ColumnCount = 1;
            this.layoutDefectList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutDefectList.Controls.Add(this.defectImage, 0, 3);
            this.layoutDefectList.Controls.Add(this.labelDefectImage, 0, 2);
            this.layoutDefectList.Controls.Add(this.defectList, 0, 1);
            this.layoutDefectList.Controls.Add(this.labelDefectList, 0, 0);
            this.layoutDefectList.Controls.Add(this.layoutDefectInfo, 0, 4);
            this.layoutDefectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutDefectList.Location = new System.Drawing.Point(225, 0);
            this.layoutDefectList.Margin = new System.Windows.Forms.Padding(0);
            this.layoutDefectList.Name = "layoutDefectList";
            this.layoutDefectList.RowCount = 5;
            this.layoutDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 246F));
            this.layoutDefectList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutDefectList.Size = new System.Drawing.Size(275, 798);
            this.layoutDefectList.TabIndex = 88;
            // 
            // defectImage
            // 
            this.defectImage.BackColor = System.Drawing.SystemColors.Control;
            this.defectImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.defectImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectImage.Location = new System.Drawing.Point(1, 337);
            this.defectImage.Margin = new System.Windows.Forms.Padding(0);
            this.defectImage.Name = "defectImage";
            this.defectImage.Size = new System.Drawing.Size(273, 246);
            this.defectImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.defectImage.TabIndex = 65;
            this.defectImage.TabStop = false;
            // 
            // labelDefectImage
            // 
            this.labelDefectImage.AutoSize = true;
            this.labelDefectImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelDefectImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefectImage.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelDefectImage.Location = new System.Drawing.Point(1, 301);
            this.labelDefectImage.Margin = new System.Windows.Forms.Padding(0);
            this.labelDefectImage.Name = "labelDefectImage";
            this.labelDefectImage.Size = new System.Drawing.Size(273, 35);
            this.labelDefectImage.TabIndex = 64;
            this.labelDefectImage.Text = "Defect Image";
            this.labelDefectImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // defectList
            // 
            this.defectList.AllowUserToAddRows = false;
            this.defectList.AllowUserToDeleteRows = false;
            this.defectList.AllowUserToResizeColumns = false;
            this.defectList.AllowUserToResizeRows = false;
            this.defectList.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.defectList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.defectList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.defectList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnCamIndex,
            this.columnNo,
            this.columnDefectType});
            this.defectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defectList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.defectList.Location = new System.Drawing.Point(1, 37);
            this.defectList.Margin = new System.Windows.Forms.Padding(0);
            this.defectList.MultiSelect = false;
            this.defectList.Name = "defectList";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.defectList.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.defectList.RowHeadersVisible = false;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.defectList.RowsDefaultCellStyle = dataGridViewCellStyle6;
            this.defectList.RowTemplate.Height = 23;
            this.defectList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.defectList.Size = new System.Drawing.Size(273, 263);
            this.defectList.TabIndex = 0;
            this.defectList.SelectionChanged += new System.EventHandler(this.defectList_SelectionChanged);
            this.defectList.Click += new System.EventHandler(this.defectList_Click);
            // 
            // columnCamIndex
            // 
            this.columnCamIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnCamIndex.HeaderText = "Cam";
            this.columnCamIndex.Name = "columnCamIndex";
            this.columnCamIndex.Width = 69;
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
            // labelDefectList
            // 
            this.labelDefectList.AutoSize = true;
            this.labelDefectList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelDefectList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDefectList.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelDefectList.Location = new System.Drawing.Point(1, 1);
            this.labelDefectList.Margin = new System.Windows.Forms.Padding(0);
            this.labelDefectList.Name = "labelDefectList";
            this.labelDefectList.Size = new System.Drawing.Size(273, 35);
            this.labelDefectList.TabIndex = 0;
            this.labelDefectList.Text = "Defect List";
            this.labelDefectList.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutDefectInfo
            // 
            this.layoutDefectInfo.AutoSize = true;
            this.layoutDefectInfo.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutDefectInfo.ColumnCount = 4;
            this.layoutDefectInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.layoutDefectInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutDefectInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.layoutDefectInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutDefectInfo.Controls.Add(this.panelShape, 0, 1);
            this.layoutDefectInfo.Controls.Add(this.panelCommon, 0, 0);
            this.layoutDefectInfo.Location = new System.Drawing.Point(1, 584);
            this.layoutDefectInfo.Margin = new System.Windows.Forms.Padding(0);
            this.layoutDefectInfo.Name = "layoutDefectInfo";
            this.layoutDefectInfo.RowCount = 2;
            this.layoutDefectInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutDefectInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutDefectInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutDefectInfo.Size = new System.Drawing.Size(273, 213);
            this.layoutDefectInfo.TabIndex = 0;
            // 
            // panelShape
            // 
            this.panelShape.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutDefectInfo.SetColumnSpan(this.panelShape, 4);
            this.panelShape.Controls.Add(this.layoutShape);
            this.panelShape.Location = new System.Drawing.Point(1, 107);
            this.panelShape.Margin = new System.Windows.Forms.Padding(0);
            this.panelShape.Name = "panelShape";
            this.panelShape.Size = new System.Drawing.Size(271, 105);
            this.panelShape.TabIndex = 1;
            this.panelShape.Visible = false;
            // 
            // layoutShape
            // 
            this.layoutShape.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutShape.ColumnCount = 4;
            this.layoutShape.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.layoutShape.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutShape.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.layoutShape.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutShape.Controls.Add(this.labelCenterX, 0, 2);
            this.layoutShape.Controls.Add(this.labelHeight, 2, 1);
            this.layoutShape.Controls.Add(this.labelWidth, 0, 1);
            this.layoutShape.Controls.Add(this.labelCenterY, 2, 2);
            this.layoutShape.Controls.Add(this.width, 1, 1);
            this.layoutShape.Controls.Add(this.height, 3, 1);
            this.layoutShape.Controls.Add(this.centerX, 1, 2);
            this.layoutShape.Controls.Add(this.centerY, 3, 2);
            this.layoutShape.Controls.Add(this.area, 1, 0);
            this.layoutShape.Controls.Add(this.labelArea, 0, 0);
            this.layoutShape.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutShape.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.layoutShape.Location = new System.Drawing.Point(0, 0);
            this.layoutShape.Margin = new System.Windows.Forms.Padding(0);
            this.layoutShape.Name = "layoutShape";
            this.layoutShape.RowCount = 3;
            this.layoutShape.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutShape.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutShape.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutShape.Size = new System.Drawing.Size(271, 105);
            this.layoutShape.TabIndex = 7;
            // 
            // labelCenterX
            // 
            this.labelCenterX.AutoSize = true;
            this.labelCenterX.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelCenterX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCenterX.Location = new System.Drawing.Point(1, 69);
            this.labelCenterX.Margin = new System.Windows.Forms.Padding(0);
            this.labelCenterX.Name = "labelCenterX";
            this.labelCenterX.Size = new System.Drawing.Size(75, 35);
            this.labelCenterX.TabIndex = 11;
            this.labelCenterX.Text = "Center X";
            this.labelCenterX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeight.Location = new System.Drawing.Point(136, 35);
            this.labelHeight.Margin = new System.Windows.Forms.Padding(0);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(75, 33);
            this.labelHeight.TabIndex = 10;
            this.labelHeight.Text = "Height";
            this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWidth.Location = new System.Drawing.Point(1, 35);
            this.labelWidth.Margin = new System.Windows.Forms.Padding(0);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(75, 33);
            this.labelWidth.TabIndex = 9;
            this.labelWidth.Text = "Width";
            this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCenterY
            // 
            this.labelCenterY.AutoSize = true;
            this.labelCenterY.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelCenterY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCenterY.Location = new System.Drawing.Point(136, 69);
            this.labelCenterY.Margin = new System.Windows.Forms.Padding(0);
            this.labelCenterY.Name = "labelCenterY";
            this.labelCenterY.Size = new System.Drawing.Size(75, 35);
            this.labelCenterY.TabIndex = 5;
            this.labelCenterY.Text = "Center Y";
            this.labelCenterY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // width
            // 
            this.width.AutoSize = true;
            this.width.Dock = System.Windows.Forms.DockStyle.Fill;
            this.width.Location = new System.Drawing.Point(77, 35);
            this.width.Margin = new System.Windows.Forms.Padding(0);
            this.width.Name = "width";
            this.width.Size = new System.Drawing.Size(58, 33);
            this.width.TabIndex = 12;
            this.width.Text = "0";
            this.width.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // height
            // 
            this.height.AutoSize = true;
            this.height.Dock = System.Windows.Forms.DockStyle.Fill;
            this.height.Location = new System.Drawing.Point(212, 35);
            this.height.Margin = new System.Windows.Forms.Padding(0);
            this.height.Name = "height";
            this.height.Size = new System.Drawing.Size(58, 33);
            this.height.TabIndex = 13;
            this.height.Text = "0";
            this.height.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // centerX
            // 
            this.centerX.AutoSize = true;
            this.centerX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centerX.Location = new System.Drawing.Point(77, 69);
            this.centerX.Margin = new System.Windows.Forms.Padding(0);
            this.centerX.Name = "centerX";
            this.centerX.Size = new System.Drawing.Size(58, 35);
            this.centerX.TabIndex = 14;
            this.centerX.Text = "0";
            this.centerX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // centerY
            // 
            this.centerY.AutoSize = true;
            this.centerY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.centerY.Location = new System.Drawing.Point(212, 69);
            this.centerY.Margin = new System.Windows.Forms.Padding(0);
            this.centerY.Name = "centerY";
            this.centerY.Size = new System.Drawing.Size(58, 35);
            this.centerY.TabIndex = 15;
            this.centerY.Text = "0";
            this.centerY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // area
            // 
            this.area.AutoSize = true;
            this.layoutShape.SetColumnSpan(this.area, 3);
            this.area.Dock = System.Windows.Forms.DockStyle.Fill;
            this.area.Location = new System.Drawing.Point(77, 1);
            this.area.Margin = new System.Windows.Forms.Padding(0);
            this.area.Name = "area";
            this.area.Size = new System.Drawing.Size(193, 33);
            this.area.TabIndex = 4;
            this.area.Text = "0";
            this.area.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelArea
            // 
            this.labelArea.AutoSize = true;
            this.labelArea.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelArea.Location = new System.Drawing.Point(1, 1);
            this.labelArea.Margin = new System.Windows.Forms.Padding(0);
            this.labelArea.Name = "labelArea";
            this.labelArea.Size = new System.Drawing.Size(75, 33);
            this.labelArea.TabIndex = 3;
            this.labelArea.Text = "Area";
            this.labelArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelCommon
            // 
            this.panelCommon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutDefectInfo.SetColumnSpan(this.panelCommon, 4);
            this.panelCommon.Controls.Add(this.layoutCommonDefect);
            this.panelCommon.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold);
            this.panelCommon.Location = new System.Drawing.Point(1, 1);
            this.panelCommon.Margin = new System.Windows.Forms.Padding(0);
            this.panelCommon.Name = "panelCommon";
            this.panelCommon.Size = new System.Drawing.Size(271, 105);
            this.panelCommon.TabIndex = 0;
            // 
            // layoutCommonDefect
            // 
            this.layoutCommonDefect.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutCommonDefect.ColumnCount = 4;
            this.layoutCommonDefect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.layoutCommonDefect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutCommonDefect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.layoutCommonDefect.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutCommonDefect.Controls.Add(this.labelLengthUnit, 3, 0);
            this.layoutCommonDefect.Controls.Add(this.labelElongation, 0, 2);
            this.layoutCommonDefect.Controls.Add(this.labelUpper, 2, 1);
            this.layoutCommonDefect.Controls.Add(this.labelLower, 0, 1);
            this.layoutCommonDefect.Controls.Add(this.labelCompactness, 2, 2);
            this.layoutCommonDefect.Controls.Add(this.lower, 1, 1);
            this.layoutCommonDefect.Controls.Add(this.upper, 3, 1);
            this.layoutCommonDefect.Controls.Add(this.elongation, 1, 2);
            this.layoutCommonDefect.Controls.Add(this.compactness, 3, 2);
            this.layoutCommonDefect.Controls.Add(this.lenght, 1, 0);
            this.layoutCommonDefect.Controls.Add(this.labelLength, 0, 0);
            this.layoutCommonDefect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutCommonDefect.Location = new System.Drawing.Point(0, 0);
            this.layoutCommonDefect.Margin = new System.Windows.Forms.Padding(0);
            this.layoutCommonDefect.Name = "layoutCommonDefect";
            this.layoutCommonDefect.RowCount = 3;
            this.layoutCommonDefect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutCommonDefect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutCommonDefect.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutCommonDefect.Size = new System.Drawing.Size(271, 105);
            this.layoutCommonDefect.TabIndex = 7;
            // 
            // labelLengthUnit
            // 
            this.labelLengthUnit.AutoSize = true;
            this.labelLengthUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLengthUnit.Location = new System.Drawing.Point(222, 1);
            this.labelLengthUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelLengthUnit.Name = "labelLengthUnit";
            this.labelLengthUnit.Size = new System.Drawing.Size(48, 33);
            this.labelLengthUnit.TabIndex = 4;
            this.labelLengthUnit.Text = "um";
            this.labelLengthUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelElongation
            // 
            this.labelElongation.AutoSize = true;
            this.labelElongation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelElongation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelElongation.Location = new System.Drawing.Point(1, 69);
            this.labelElongation.Margin = new System.Windows.Forms.Padding(0);
            this.labelElongation.Name = "labelElongation";
            this.labelElongation.Size = new System.Drawing.Size(90, 35);
            this.labelElongation.TabIndex = 11;
            this.labelElongation.Text = "Elongation";
            this.labelElongation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelUpper
            // 
            this.labelUpper.AutoSize = true;
            this.labelUpper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelUpper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUpper.Location = new System.Drawing.Point(141, 35);
            this.labelUpper.Margin = new System.Windows.Forms.Padding(0);
            this.labelUpper.Name = "labelUpper";
            this.labelUpper.Size = new System.Drawing.Size(80, 33);
            this.labelUpper.TabIndex = 10;
            this.labelUpper.Text = "Upper";
            this.labelUpper.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLower
            // 
            this.labelLower.AutoSize = true;
            this.labelLower.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelLower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLower.Location = new System.Drawing.Point(1, 35);
            this.labelLower.Margin = new System.Windows.Forms.Padding(0);
            this.labelLower.Name = "labelLower";
            this.labelLower.Size = new System.Drawing.Size(90, 33);
            this.labelLower.TabIndex = 9;
            this.labelLower.Text = "Lower";
            this.labelLower.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCompactness
            // 
            this.labelCompactness.AutoSize = true;
            this.labelCompactness.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelCompactness.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCompactness.Location = new System.Drawing.Point(141, 69);
            this.labelCompactness.Margin = new System.Windows.Forms.Padding(0);
            this.labelCompactness.Name = "labelCompactness";
            this.labelCompactness.Size = new System.Drawing.Size(80, 35);
            this.labelCompactness.TabIndex = 5;
            this.labelCompactness.Text = "Compact";
            this.labelCompactness.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lower
            // 
            this.lower.AutoSize = true;
            this.lower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lower.Location = new System.Drawing.Point(92, 35);
            this.lower.Margin = new System.Windows.Forms.Padding(0);
            this.lower.Name = "lower";
            this.lower.Size = new System.Drawing.Size(48, 33);
            this.lower.TabIndex = 12;
            this.lower.Text = "0";
            this.lower.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // upper
            // 
            this.upper.AutoSize = true;
            this.upper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upper.Location = new System.Drawing.Point(222, 35);
            this.upper.Margin = new System.Windows.Forms.Padding(0);
            this.upper.Name = "upper";
            this.upper.Size = new System.Drawing.Size(48, 33);
            this.upper.TabIndex = 13;
            this.upper.Text = "0";
            this.upper.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // elongation
            // 
            this.elongation.AutoSize = true;
            this.elongation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elongation.Location = new System.Drawing.Point(92, 69);
            this.elongation.Margin = new System.Windows.Forms.Padding(0);
            this.elongation.Name = "elongation";
            this.elongation.Size = new System.Drawing.Size(48, 35);
            this.elongation.TabIndex = 14;
            this.elongation.Text = "0";
            this.elongation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // compactness
            // 
            this.compactness.AutoSize = true;
            this.compactness.Dock = System.Windows.Forms.DockStyle.Fill;
            this.compactness.Location = new System.Drawing.Point(222, 69);
            this.compactness.Margin = new System.Windows.Forms.Padding(0);
            this.compactness.Name = "compactness";
            this.compactness.Size = new System.Drawing.Size(48, 35);
            this.compactness.TabIndex = 15;
            this.compactness.Text = "0";
            this.compactness.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lenght
            // 
            this.lenght.AutoSize = true;
            this.layoutCommonDefect.SetColumnSpan(this.lenght, 2);
            this.lenght.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lenght.Location = new System.Drawing.Point(92, 1);
            this.lenght.Margin = new System.Windows.Forms.Padding(0);
            this.lenght.Name = "lenght";
            this.lenght.Size = new System.Drawing.Size(129, 33);
            this.lenght.TabIndex = 4;
            this.lenght.Text = "0";
            this.lenght.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelLength
            // 
            this.labelLength.AutoSize = true;
            this.labelLength.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLength.Location = new System.Drawing.Point(1, 1);
            this.labelLength.Margin = new System.Windows.Forms.Padding(0);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(90, 33);
            this.labelLength.TabIndex = 3;
            this.labelLength.Text = "Length";
            this.labelLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // modelThickness
            // 
            this.modelThickness.BackColor = System.Drawing.Color.White;
            this.modelThickness.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelThickness.Location = new System.Drawing.Point(77, 31);
            this.modelThickness.Margin = new System.Windows.Forms.Padding(0);
            this.modelThickness.Name = "modelThickness";
            this.modelThickness.Size = new System.Drawing.Size(170, 29);
            this.modelThickness.TabIndex = 4;
            this.modelThickness.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // modelName
            // 
            this.modelName.BackColor = System.Drawing.Color.White;
            this.modelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modelName.Location = new System.Drawing.Point(77, 1);
            this.modelName.Margin = new System.Windows.Forms.Padding(0);
            this.modelName.Name = "modelName";
            this.modelName.Size = new System.Drawing.Size(170, 29);
            this.modelName.TabIndex = 6;
            this.modelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ReportPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.layoutMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Name = "ReportPanel";
            this.Size = new System.Drawing.Size(1206, 928);
            this.layoutMain.ResumeLayout(false);
            this.layoutInfo.ResumeLayout(false);
            this.layoutInfo.PerformLayout();
            this.layoutGradeInfo.ResumeLayout(false);
            this.layoutGradeInfo.PerformLayout();
            this.layoutModel.ResumeLayout(false);
            this.layoutModel.PerformLayout();
            this.layoutProduction.ResumeLayout(false);
            this.layoutProduction.PerformLayout();
            this.layoutTop.ResumeLayout(false);
            this.layoutTop.PerformLayout();
            this.panelSelectCam.ResumeLayout(false);
            this.panelSelectCam.PerformLayout();
            this.layoutType.ResumeLayout(false);
            this.layoutType.PerformLayout();
            this.layoutSize.ResumeLayout(false);
            this.layoutSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sizeMin)).EndInit();
            this.layoutBottom.ResumeLayout(false);
            this.layoutSheetList.ResumeLayout(false);
            this.layoutSheetList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sheetList)).EndInit();
            this.layoutResultFilter.ResumeLayout(false);
            this.layoutResultFilter.PerformLayout();
            this.layoutSheetInfo.ResumeLayout(false);
            this.layoutSheetInfo.PerformLayout();
            this.layoutImage.ResumeLayout(false);
            this.layoutImage.PerformLayout();
            this.layoutDefectList.ResumeLayout(false);
            this.layoutDefectList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.defectImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.defectList)).EndInit();
            this.layoutDefectInfo.ResumeLayout(false);
            this.panelShape.ResumeLayout(false);
            this.layoutShape.ResumeLayout(false);
            this.layoutShape.PerformLayout();
            this.panelCommon.ResumeLayout(false);
            this.layoutCommonDefect.ResumeLayout(false);
            this.layoutCommonDefect.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.TableLayoutPanel layoutBottom;
        private System.Windows.Forms.TableLayoutPanel layoutSheetList;
        private System.Windows.Forms.TableLayoutPanel layoutResultFilter;
        private System.Windows.Forms.CheckBox ngFilter;
        private System.Windows.Forms.CheckBox okFilter;
        private System.Windows.Forms.TableLayoutPanel layoutImage;
        private System.Windows.Forms.Label labelImage;
        private System.Windows.Forms.TableLayoutPanel layoutDefectList;
        private System.Windows.Forms.Label labelDefectImage;
        private System.Windows.Forms.Label labelDefectList;
        private System.Windows.Forms.DataGridView defectList;
        private System.Windows.Forms.PictureBox defectImage;
        private Infragistics.Win.Misc.UltraButton buttonSelectAll;
        private System.Windows.Forms.TableLayoutPanel layoutSheetInfo;
        private System.Windows.Forms.Label labelSheetNG;
        private System.Windows.Forms.Label sheetRatio;
        private System.Windows.Forms.Label labelSheetTotal;
        private System.Windows.Forms.Label labelSheetRatio;
        private System.Windows.Forms.Label sheetNG;
        private System.Windows.Forms.Label sheetTotal;
        private System.Windows.Forms.Label labelUnit;
        private System.Windows.Forms.Panel imagePanel;
        private System.Windows.Forms.TableLayoutPanel layoutTop;
        private Infragistics.Win.Misc.UltraButton buttonFolder;
        private System.Windows.Forms.Label labelFolder;
        private System.Windows.Forms.TableLayoutPanel layoutType;
        private System.Windows.Forms.RadioButton shape;
        private System.Windows.Forms.RadioButton pinHole;
        private System.Windows.Forms.RadioButton dielectric;
        private System.Windows.Forms.RadioButton pole;
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
        private System.Windows.Forms.DataGridView sheetList;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPattern;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnQty;
        private System.Windows.Forms.CheckBox useSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCamIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDefectType;
        private System.Windows.Forms.TableLayoutPanel layoutDefectInfo;
        private System.Windows.Forms.TableLayoutPanel layoutProduction;
        private System.Windows.Forms.Label labelProductionLotName;
        private System.Windows.Forms.Label labelProductionStartTime;
        private System.Windows.Forms.Label productionStartTime;
        private System.Windows.Forms.Label productionLotName;
        private System.Windows.Forms.Label productionEndTime;
        private System.Windows.Forms.Label labelProductionEndTime;
        private System.Windows.Forms.Label labelProduction;
        private System.Windows.Forms.TableLayoutPanel layoutCommonDefect;
        private System.Windows.Forms.Label labelElongation;
        private System.Windows.Forms.Label labelUpper;
        private System.Windows.Forms.Label labelLower;
        private System.Windows.Forms.Label labelCompactness;
        private System.Windows.Forms.Label lower;
        private System.Windows.Forms.Label upper;
        private System.Windows.Forms.Label elongation;
        private System.Windows.Forms.Label compactness;
        private System.Windows.Forms.Label lenght;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.Panel panelCommon;
        private System.Windows.Forms.Panel panelShape;
        private System.Windows.Forms.TableLayoutPanel layoutShape;
        private System.Windows.Forms.Label labelCenterX;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.Label labelCenterY;
        private System.Windows.Forms.Label width;
        private System.Windows.Forms.Label height;
        private System.Windows.Forms.Label centerX;
        private System.Windows.Forms.Label centerY;
        private System.Windows.Forms.Label area;
        private System.Windows.Forms.Label labelArea;
        private System.Windows.Forms.CheckBox errorFilter;
        private System.Windows.Forms.Label labelCam;
        private System.Windows.Forms.Panel panelSelectCam;
        private System.Windows.Forms.CheckBox checkBoxCam;
        private System.Windows.Forms.Label labelLengthUnit;
        private Infragistics.Win.Misc.UltraButton buttonCapture;
        private System.Windows.Forms.Label labelCapture;
        private System.Windows.Forms.TableLayoutPanel layoutInfo;
        private System.Windows.Forms.TableLayoutPanel layoutModel;
        private System.Windows.Forms.Label labelModelName;
        private System.Windows.Forms.Label labelModelThickness;
        private System.Windows.Forms.Label modelPaste;
        private System.Windows.Forms.Label labelModelPaste;
        private System.Windows.Forms.Label labelModel;
        private System.Windows.Forms.Label labelSheetList;
        private System.Windows.Forms.TableLayoutPanel layoutGradeInfo;
        private System.Windows.Forms.Label gradeSheetAttackTotal;
        private System.Windows.Forms.Label labelGradeTotal;
        private System.Windows.Forms.Label labelGradeSheetAttack;
        private System.Windows.Forms.Label labelGradeSheet;
        private System.Windows.Forms.Label gradeSheetAttackSheet;
        private System.Windows.Forms.Label gradePoleTotal;
        private System.Windows.Forms.Label gradePoleSheet;
        private System.Windows.Forms.Label gradeDielectricTotal;
        private System.Windows.Forms.Label gradeDielectricSheet;
        private System.Windows.Forms.Label gradePinHoleTotal;
        private System.Windows.Forms.Label gradePinHoleSheet;
        private System.Windows.Forms.Label gradeShapeTotal;
        private System.Windows.Forms.Label gradeShapeSheet;
        private System.Windows.Forms.Label labelGradePole;
        private System.Windows.Forms.Label labelGradeDielectric;
        private System.Windows.Forms.Label labelGradePinHole;
        private System.Windows.Forms.Label labelGradeShape;
        private System.Windows.Forms.Label labelGradeDefect;
        private System.Windows.Forms.Label gradeDefectTotal;
        private System.Windows.Forms.Label gradeDefectSheet;
        private System.Windows.Forms.Label labelTeachInfo;
        private System.Windows.Forms.Label modelThickness;
        private System.Windows.Forms.Label modelName;
    }
}
