namespace UniScanX.MPAlignment.UI.Pages
{
    partial class InspectPage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtBarcodeNo = new System.Windows.Forms.Label();
            this.labelBarcodeNo = new System.Windows.Forms.Label();
            this.txtInspectionNo = new System.Windows.Forms.Label();
            this.labelInspectionNo = new System.Windows.Forms.Label();
            this.txtModelName = new System.Windows.Forms.Label();
            this.labelModelName = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lblContinuousDefect = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelResult = new System.Windows.Forms.Label();
            this.paneLeft = new System.Windows.Forms.Panel();
            this.lblTestMode = new System.Windows.Forms.Label();
 //           this.tgsTestMode = new ReaLTaiizor.HopeToggle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelModules = new System.Windows.Forms.Label();
            this.labelInspectResult = new System.Windows.Forms.Label();
            this.txtTotalModules = new System.Windows.Forms.Label();
            this.txtInspResult = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblIsPassMode = new System.Windows.Forms.Label();
            this.labelCycleTime = new System.Windows.Forms.Label();
            this.txtCycleTime = new System.Windows.Forms.Label();
            this.btnResetCount = new System.Windows.Forms.Button();
            this.labelInspTotal = new System.Windows.Forms.Label();
            this.labelAccept = new System.Windows.Forms.Label();
            this.labelInspectionTime = new System.Windows.Forms.Label();
            this.txtInspectionTime = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.Label();
            this.txtGood = new System.Windows.Forms.Label();
            this.txtNg = new System.Windows.Forms.Label();
            this.labelDefect = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnStartInspection = new System.Windows.Forms.Button();
            this.viewContainer = new System.Windows.Forms.Panel();
            this.viewContainerPanel = new System.Windows.Forms.Panel();
            this.panelMainView = new System.Windows.Forms.Panel();
            this.panelLargeView = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pnlConveyorSystem = new System.Windows.Forms.Panel();
            this.panelViewRight = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.dgvLastInspectionResult = new System.Windows.Forms.DataGridView();
            this.inspectionNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.paneLeft.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.viewContainer.SuspendLayout();
            this.viewContainerPanel.SuspendLayout();
            this.panelMainView.SuspendLayout();
            this.panelViewRight.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLastInspectionResult)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.69136F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.30864F));
            this.tableLayoutPanel1.Controls.Add(this.txtBarcodeNo, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelBarcodeNo, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtInspectionNo, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelInspectionNo, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtModelName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelModelName, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(606, 100);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // txtBarcodeNo
            // 
            this.txtBarcodeNo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBarcodeNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtBarcodeNo.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtBarcodeNo.ForeColor = System.Drawing.Color.LawnGreen;
            this.txtBarcodeNo.Location = new System.Drawing.Point(154, 71);
            this.txtBarcodeNo.Margin = new System.Windows.Forms.Padding(5);
            this.txtBarcodeNo.Name = "txtBarcodeNo";
            this.txtBarcodeNo.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.txtBarcodeNo.Size = new System.Drawing.Size(447, 24);
            this.txtBarcodeNo.TabIndex = 5;
            this.txtBarcodeNo.Text = "None";
            this.txtBarcodeNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelBarcodeNo
            // 
            this.labelBarcodeNo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBarcodeNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelBarcodeNo.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelBarcodeNo.ForeColor = System.Drawing.Color.LawnGreen;
            this.labelBarcodeNo.Location = new System.Drawing.Point(5, 71);
            this.labelBarcodeNo.Margin = new System.Windows.Forms.Padding(5);
            this.labelBarcodeNo.Name = "labelBarcodeNo";
            this.labelBarcodeNo.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.labelBarcodeNo.Size = new System.Drawing.Size(139, 24);
            this.labelBarcodeNo.TabIndex = 4;
            this.labelBarcodeNo.Text = "Barcode No.";
            this.labelBarcodeNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtInspectionNo
            // 
            this.txtInspectionNo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInspectionNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtInspectionNo.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtInspectionNo.ForeColor = System.Drawing.Color.LawnGreen;
            this.txtInspectionNo.Location = new System.Drawing.Point(154, 38);
            this.txtInspectionNo.Margin = new System.Windows.Forms.Padding(5);
            this.txtInspectionNo.Name = "txtInspectionNo";
            this.txtInspectionNo.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.txtInspectionNo.Size = new System.Drawing.Size(447, 23);
            this.txtInspectionNo.TabIndex = 3;
            this.txtInspectionNo.Text = "None";
            this.txtInspectionNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelInspectionNo
            // 
            this.labelInspectionNo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelInspectionNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelInspectionNo.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelInspectionNo.ForeColor = System.Drawing.Color.LawnGreen;
            this.labelInspectionNo.Location = new System.Drawing.Point(5, 38);
            this.labelInspectionNo.Margin = new System.Windows.Forms.Padding(5);
            this.labelInspectionNo.Name = "labelInspectionNo";
            this.labelInspectionNo.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.labelInspectionNo.Size = new System.Drawing.Size(139, 23);
            this.labelInspectionNo.TabIndex = 2;
            this.labelInspectionNo.Text = "Inspection No.";
            this.labelInspectionNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtModelName
            // 
            this.txtModelName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtModelName.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtModelName.ForeColor = System.Drawing.Color.LawnGreen;
            this.txtModelName.Location = new System.Drawing.Point(154, 5);
            this.txtModelName.Margin = new System.Windows.Forms.Padding(5);
            this.txtModelName.Name = "txtModelName";
            this.txtModelName.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.txtModelName.Size = new System.Drawing.Size(447, 23);
            this.txtModelName.TabIndex = 1;
            this.txtModelName.Text = "1111";
            this.txtModelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelModelName
            // 
            this.labelModelName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelModelName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelModelName.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelModelName.ForeColor = System.Drawing.Color.LawnGreen;
            this.labelModelName.Location = new System.Drawing.Point(5, 5);
            this.labelModelName.Margin = new System.Windows.Forms.Padding(5);
            this.labelModelName.Name = "labelModelName";
            this.labelModelName.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.labelModelName.Size = new System.Drawing.Size(139, 23);
            this.labelModelName.TabIndex = 0;
            this.labelModelName.Text = "Model";
            this.labelModelName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.tableLayoutPanel1);
            this.panelTop.Controls.Add(this.panel1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(171, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1162, 100);
            this.panelTop.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(606, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(556, 100);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.69136F));
            this.tableLayoutPanel3.Controls.Add(this.lblContinuousDefect, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.labelStatus, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelResult, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(10);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(556, 100);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // lblContinuousDefect
            // 
            this.lblContinuousDefect.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblContinuousDefect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblContinuousDefect.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblContinuousDefect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContinuousDefect.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.lblContinuousDefect.Location = new System.Drawing.Point(4, 70);
            this.lblContinuousDefect.Margin = new System.Windows.Forms.Padding(4);
            this.lblContinuousDefect.Name = "lblContinuousDefect";
            this.lblContinuousDefect.Size = new System.Drawing.Size(548, 26);
            this.lblContinuousDefect.TabIndex = 45;
            this.lblContinuousDefect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelStatus.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.labelStatus.Location = new System.Drawing.Point(4, 4);
            this.labelStatus.Margin = new System.Windows.Forms.Padding(4);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(548, 25);
            this.labelStatus.TabIndex = 43;
            this.labelStatus.Text = "Stopped";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelResult
            // 
            this.labelResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelResult.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResult.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.labelResult.Location = new System.Drawing.Point(4, 37);
            this.labelResult.Margin = new System.Windows.Forms.Padding(4);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(548, 25);
            this.labelResult.TabIndex = 44;
            this.labelResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // paneLeft
            // 
            this.paneLeft.Controls.Add(this.lblTestMode);
//            this.paneLeft.Controls.Add(this.tgsTestMode);
            this.paneLeft.Controls.Add(this.panel2);
            this.paneLeft.Controls.Add(this.panel3);
            this.paneLeft.Controls.Add(this.panel6);
            this.paneLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.paneLeft.Location = new System.Drawing.Point(0, 0);
            this.paneLeft.Name = "paneLeft";
            this.paneLeft.Size = new System.Drawing.Size(171, 983);
            this.paneLeft.TabIndex = 1;
            // 
            // lblTestMode
            // 
            this.lblTestMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTestMode.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTestMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblTestMode.Location = new System.Drawing.Point(8, 650);
            this.lblTestMode.Margin = new System.Windows.Forms.Padding(5);
            this.lblTestMode.Name = "lblTestMode";
            this.lblTestMode.Size = new System.Drawing.Size(81, 24);
            this.lblTestMode.TabIndex = 67;
            this.lblTestMode.Text = "Test mode";
            this.lblTestMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTestMode.Visible = false;
            // 
            // tgsTestMode
            // 
            //this.tgsTestMode.AutoSize = true;
            //this.tgsTestMode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            //this.tgsTestMode.BaseColorA = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            //this.tgsTestMode.BaseColorB = System.Drawing.Color.SkyBlue;
            //this.tgsTestMode.Cursor = System.Windows.Forms.Cursors.Hand;
            //this.tgsTestMode.HeadColorA = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            //this.tgsTestMode.HeadColorB = System.Drawing.Color.White;
            //this.tgsTestMode.HeadColorC = System.Drawing.Color.LawnGreen;
            //this.tgsTestMode.HeadColorD = System.Drawing.Color.LawnGreen;
            //this.tgsTestMode.Location = new System.Drawing.Point(114, 653);
            //this.tgsTestMode.Name = "tgsTestMode";
            //this.tgsTestMode.Size = new System.Drawing.Size(48, 20);
            //this.tgsTestMode.TabIndex = 61;
            //this.tgsTestMode.Text = "hopeToggle1";
            //this.tgsTestMode.UseVisualStyleBackColor = true;
            //this.tgsTestMode.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelModules);
            this.panel2.Controls.Add(this.labelInspectResult);
            this.panel2.Controls.Add(this.txtTotalModules);
            this.panel2.Controls.Add(this.txtInspResult);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 522);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(171, 125);
            this.panel2.TabIndex = 54;
            // 
            // labelModules
            // 
            this.labelModules.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelModules.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelModules.ForeColor = System.Drawing.Color.LawnGreen;
            this.labelModules.Location = new System.Drawing.Point(12, 8);
            this.labelModules.Margin = new System.Windows.Forms.Padding(5);
            this.labelModules.Name = "labelModules";
            this.labelModules.Size = new System.Drawing.Size(151, 23);
            this.labelModules.TabIndex = 5;
            this.labelModules.Text = "Total Modules";
            this.labelModules.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelModules.Visible = false;
            // 
            // labelInspectResult
            // 
            this.labelInspectResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelInspectResult.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelInspectResult.ForeColor = System.Drawing.Color.LawnGreen;
            this.labelInspectResult.Location = new System.Drawing.Point(12, 66);
            this.labelInspectResult.Margin = new System.Windows.Forms.Padding(5);
            this.labelInspectResult.Name = "labelInspectResult";
            this.labelInspectResult.Size = new System.Drawing.Size(151, 23);
            this.labelInspectResult.TabIndex = 6;
            this.labelInspectResult.Text = "Insp. Result";
            this.labelInspectResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtTotalModules
            // 
            this.txtTotalModules.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtTotalModules.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotalModules.ForeColor = System.Drawing.Color.LawnGreen;
            this.txtTotalModules.Location = new System.Drawing.Point(12, 34);
            this.txtTotalModules.Margin = new System.Windows.Forms.Padding(5);
            this.txtTotalModules.Name = "txtTotalModules";
            this.txtTotalModules.Size = new System.Drawing.Size(151, 24);
            this.txtTotalModules.TabIndex = 15;
            this.txtTotalModules.Text = "0 / 0";
            this.txtTotalModules.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtTotalModules.Visible = false;
            // 
            // txtInspResult
            // 
            this.txtInspResult.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtInspResult.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtInspResult.ForeColor = System.Drawing.Color.LawnGreen;
            this.txtInspResult.Location = new System.Drawing.Point(12, 93);
            this.txtInspResult.Margin = new System.Windows.Forms.Padding(5);
            this.txtInspResult.Name = "txtInspResult";
            this.txtInspResult.Size = new System.Drawing.Size(151, 24);
            this.txtInspResult.TabIndex = 14;
            this.txtInspResult.Text = "0 / 0";
            this.txtInspResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblIsPassMode);
            this.panel3.Controls.Add(this.labelCycleTime);
            this.panel3.Controls.Add(this.txtCycleTime);
            this.panel3.Controls.Add(this.btnResetCount);
            this.panel3.Controls.Add(this.labelInspTotal);
            this.panel3.Controls.Add(this.labelAccept);
            this.panel3.Controls.Add(this.labelInspectionTime);
            this.panel3.Controls.Add(this.txtInspectionTime);
            this.panel3.Controls.Add(this.txtTotal);
            this.panel3.Controls.Add(this.txtGood);
            this.panel3.Controls.Add(this.txtNg);
            this.panel3.Controls.Add(this.labelDefect);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 117);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(171, 405);
            this.panel3.TabIndex = 0;
            // 
            // lblIsPassMode
            // 
            this.lblIsPassMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblIsPassMode.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblIsPassMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblIsPassMode.Location = new System.Drawing.Point(11, 5);
            this.lblIsPassMode.Margin = new System.Windows.Forms.Padding(5);
            this.lblIsPassMode.Name = "lblIsPassMode";
            this.lblIsPassMode.Size = new System.Drawing.Size(151, 24);
            this.lblIsPassMode.TabIndex = 66;
            this.lblIsPassMode.Text = "Pass Mode";
            this.lblIsPassMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblIsPassMode.Click += new System.EventHandler(this.lblIsPassMode_Click);
            // 
            // labelCycleTime
            // 
            this.labelCycleTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelCycleTime.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelCycleTime.ForeColor = System.Drawing.Color.Gold;
            this.labelCycleTime.Location = new System.Drawing.Point(11, 295);
            this.labelCycleTime.Margin = new System.Windows.Forms.Padding(5);
            this.labelCycleTime.Name = "labelCycleTime";
            this.labelCycleTime.Size = new System.Drawing.Size(152, 24);
            this.labelCycleTime.TabIndex = 54;
            this.labelCycleTime.Text = "Cycle Time";
            this.labelCycleTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelCycleTime.Visible = false;
            // 
            // txtCycleTime
            // 
            this.txtCycleTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtCycleTime.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtCycleTime.ForeColor = System.Drawing.Color.Gold;
            this.txtCycleTime.Location = new System.Drawing.Point(11, 323);
            this.txtCycleTime.Margin = new System.Windows.Forms.Padding(5);
            this.txtCycleTime.Name = "txtCycleTime";
            this.txtCycleTime.Size = new System.Drawing.Size(152, 24);
            this.txtCycleTime.TabIndex = 55;
            this.txtCycleTime.Text = "00 : 00";
            this.txtCycleTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtCycleTime.Visible = false;
            // 
            // btnResetCount
            // 
            this.btnResetCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnResetCount.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnResetCount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetCount.ForeColor = System.Drawing.Color.White;
            this.btnResetCount.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnResetCount.Location = new System.Drawing.Point(10, 359);
            this.btnResetCount.Margin = new System.Windows.Forms.Padding(0);
            this.btnResetCount.Name = "btnResetCount";
            this.btnResetCount.Size = new System.Drawing.Size(154, 36);
            this.btnResetCount.TabIndex = 53;
            this.btnResetCount.Text = "Reset";
            this.btnResetCount.UseVisualStyleBackColor = false;
            this.btnResetCount.Click += new System.EventHandler(this.btnResetCount_Click);
            // 
            // labelInspTotal
            // 
            this.labelInspTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelInspTotal.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelInspTotal.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.labelInspTotal.Location = new System.Drawing.Point(11, 38);
            this.labelInspTotal.Margin = new System.Windows.Forms.Padding(5);
            this.labelInspTotal.Name = "labelInspTotal";
            this.labelInspTotal.Size = new System.Drawing.Size(152, 23);
            this.labelInspTotal.TabIndex = 5;
            this.labelInspTotal.Text = "Insp. Total ";
            this.labelInspTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAccept
            // 
            this.labelAccept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelAccept.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelAccept.ForeColor = System.Drawing.Color.Lime;
            this.labelAccept.Location = new System.Drawing.Point(11, 101);
            this.labelAccept.Margin = new System.Windows.Forms.Padding(5);
            this.labelAccept.Name = "labelAccept";
            this.labelAccept.Size = new System.Drawing.Size(152, 23);
            this.labelAccept.TabIndex = 6;
            this.labelAccept.Text = "Good";
            this.labelAccept.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInspectionTime
            // 
            this.labelInspectionTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelInspectionTime.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelInspectionTime.ForeColor = System.Drawing.Color.Gold;
            this.labelInspectionTime.Location = new System.Drawing.Point(11, 233);
            this.labelInspectionTime.Margin = new System.Windows.Forms.Padding(5);
            this.labelInspectionTime.Name = "labelInspectionTime";
            this.labelInspectionTime.Size = new System.Drawing.Size(152, 24);
            this.labelInspectionTime.TabIndex = 8;
            this.labelInspectionTime.Text = "Insp. Time";
            this.labelInspectionTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtInspectionTime
            // 
            this.txtInspectionTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtInspectionTime.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtInspectionTime.ForeColor = System.Drawing.Color.Gold;
            this.txtInspectionTime.Location = new System.Drawing.Point(11, 261);
            this.txtInspectionTime.Margin = new System.Windows.Forms.Padding(5);
            this.txtInspectionTime.Name = "txtInspectionTime";
            this.txtInspectionTime.Size = new System.Drawing.Size(152, 24);
            this.txtInspectionTime.TabIndex = 12;
            this.txtInspectionTime.Text = "00 : 00";
            this.txtInspectionTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtTotal
            // 
            this.txtTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtTotal.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtTotal.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.txtTotal.Location = new System.Drawing.Point(11, 64);
            this.txtTotal.Margin = new System.Windows.Forms.Padding(5);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(152, 24);
            this.txtTotal.TabIndex = 15;
            this.txtTotal.Text = "0";
            this.txtTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtGood
            // 
            this.txtGood.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtGood.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtGood.ForeColor = System.Drawing.Color.Lime;
            this.txtGood.Location = new System.Drawing.Point(11, 128);
            this.txtGood.Margin = new System.Windows.Forms.Padding(5);
            this.txtGood.Name = "txtGood";
            this.txtGood.Size = new System.Drawing.Size(152, 24);
            this.txtGood.TabIndex = 14;
            this.txtGood.Text = "0";
            this.txtGood.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtNg
            // 
            this.txtNg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtNg.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtNg.ForeColor = System.Drawing.Color.OrangeRed;
            this.txtNg.Location = new System.Drawing.Point(11, 193);
            this.txtNg.Margin = new System.Windows.Forms.Padding(5);
            this.txtNg.Name = "txtNg";
            this.txtNg.Size = new System.Drawing.Size(152, 24);
            this.txtNg.TabIndex = 13;
            this.txtNg.Text = "0";
            this.txtNg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDefect
            // 
            this.labelDefect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.labelDefect.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelDefect.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelDefect.Location = new System.Drawing.Point(11, 165);
            this.labelDefect.Margin = new System.Windows.Forms.Padding(5);
            this.labelDefect.Name = "labelDefect";
            this.labelDefect.Size = new System.Drawing.Size(152, 24);
            this.labelDefect.TabIndex = 7;
            this.labelDefect.Text = "Ng";
            this.labelDefect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnStartInspection);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(171, 117);
            this.panel6.TabIndex = 60;
            // 
            // btnStartInspection
            // 
            this.btnStartInspection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.btnStartInspection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnStartInspection.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnStartInspection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartInspection.Image = global::UniScanX.MPAlignment.Properties.Resources.start_128;
            this.btnStartInspection.Location = new System.Drawing.Point(8, 5);
            this.btnStartInspection.Margin = new System.Windows.Forms.Padding(20);
            this.btnStartInspection.Name = "btnStartInspection";
            this.btnStartInspection.Size = new System.Drawing.Size(155, 95);
            this.btnStartInspection.TabIndex = 48;
            this.btnStartInspection.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStartInspection.UseVisualStyleBackColor = false;
            this.btnStartInspection.Click += new System.EventHandler(this.buttonStartInspection_Click);
            // 
            // viewContainer
            // 
            this.viewContainer.Controls.Add(this.viewContainerPanel);
            this.viewContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewContainer.Location = new System.Drawing.Point(171, 100);
            this.viewContainer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.viewContainer.Name = "viewContainer";
            this.viewContainer.Size = new System.Drawing.Size(1162, 883);
            this.viewContainer.TabIndex = 21;
            // 
            // viewContainerPanel
            // 
            this.viewContainerPanel.Controls.Add(this.panelMainView);
            this.viewContainerPanel.Controls.Add(this.panelViewRight);
            this.viewContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewContainerPanel.Location = new System.Drawing.Point(0, 0);
            this.viewContainerPanel.Name = "viewContainerPanel";
            this.viewContainerPanel.Size = new System.Drawing.Size(1162, 883);
            this.viewContainerPanel.TabIndex = 6;
            // 
            // panelMainView
            // 
            this.panelMainView.Controls.Add(this.panelLargeView);
            this.panelMainView.Controls.Add(this.panel4);
            this.panelMainView.Controls.Add(this.pnlConveyorSystem);
            this.panelMainView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMainView.Location = new System.Drawing.Point(0, 0);
            this.panelMainView.Margin = new System.Windows.Forms.Padding(20);
            this.panelMainView.Name = "panelMainView";
            this.panelMainView.Size = new System.Drawing.Size(606, 883);
            this.panelMainView.TabIndex = 0;
            // 
            // panelLargeView
            // 
            this.panelLargeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLargeView.Location = new System.Drawing.Point(0, 17);
            this.panelLargeView.Margin = new System.Windows.Forms.Padding(20);
            this.panelLargeView.Name = "panelLargeView";
            this.panelLargeView.Size = new System.Drawing.Size(606, 703);
            this.panelLargeView.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Margin = new System.Windows.Forms.Padding(20);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(606, 17);
            this.panel4.TabIndex = 3;
            // 
            // pnlConveyorSystem
            // 
            this.pnlConveyorSystem.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlConveyorSystem.Location = new System.Drawing.Point(0, 720);
            this.pnlConveyorSystem.Name = "pnlConveyorSystem";
            this.pnlConveyorSystem.Size = new System.Drawing.Size(606, 163);
            this.pnlConveyorSystem.TabIndex = 0;
            // 
            // panelViewRight
            // 
            this.panelViewRight.Controls.Add(this.panel7);
            this.panelViewRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelViewRight.Location = new System.Drawing.Point(606, 0);
            this.panelViewRight.Name = "panelViewRight";
            this.panelViewRight.Size = new System.Drawing.Size(556, 883);
            this.panelViewRight.TabIndex = 0;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.dgvLastInspectionResult);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Margin = new System.Windows.Forms.Padding(20);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(556, 883);
            this.panel7.TabIndex = 2;
            // 
            // dgvLastInspectionResult
            // 
            this.dgvLastInspectionResult.AllowUserToAddRows = false;
            this.dgvLastInspectionResult.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvLastInspectionResult.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLastInspectionResult.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLastInspectionResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLastInspectionResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.inspectionNo,
            this.modelName,
            this.result});
            this.dgvLastInspectionResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLastInspectionResult.EnableHeadersVisualStyles = false;
            this.dgvLastInspectionResult.GridColor = System.Drawing.Color.Black;
            this.dgvLastInspectionResult.Location = new System.Drawing.Point(0, 0);
            this.dgvLastInspectionResult.Margin = new System.Windows.Forms.Padding(0);
            this.dgvLastInspectionResult.MultiSelect = false;
            this.dgvLastInspectionResult.Name = "dgvLastInspectionResult";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLastInspectionResult.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLastInspectionResult.RowHeadersVisible = false;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            this.dgvLastInspectionResult.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvLastInspectionResult.RowTemplate.Height = 23;
            this.dgvLastInspectionResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLastInspectionResult.Size = new System.Drawing.Size(556, 883);
            this.dgvLastInspectionResult.TabIndex = 1;
            // 
            // inspectionNo
            // 
            this.inspectionNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.inspectionNo.DataPropertyName = "InspNo";
            this.inspectionNo.HeaderText = "Insp. No.";
            this.inspectionNo.Name = "inspectionNo";
            this.inspectionNo.ReadOnly = true;
            // 
            // modelName
            // 
            this.modelName.DataPropertyName = "Model";
            this.modelName.HeaderText = "Model";
            this.modelName.Name = "modelName";
            this.modelName.ReadOnly = true;
            // 
            // result
            // 
            this.result.DataPropertyName = "Result";
            this.result.HeaderText = "Result";
            this.result.Name = "result";
            this.result.ReadOnly = true;
            // 
            // InspectPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.Controls.Add(this.viewContainer);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.paneLeft);
            this.DoubleBuffered = true;
            this.Name = "InspectPage";
            this.Size = new System.Drawing.Size(1333, 983);
            this.VisibleChanged += new System.EventHandler(this.InspectPage_VisibleChanged);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.paneLeft.ResumeLayout(false);
            this.paneLeft.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.viewContainer.ResumeLayout(false);
            this.viewContainerPanel.ResumeLayout(false);
            this.panelMainView.ResumeLayout(false);
            this.panelViewRight.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLastInspectionResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel paneLeft;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnStartInspection;
        private System.Windows.Forms.Panel viewContainer;
        private System.Windows.Forms.Panel viewContainerPanel;
        private System.Windows.Forms.Label txtBarcodeNo;
        private System.Windows.Forms.Label labelBarcodeNo;
        private System.Windows.Forms.Label txtInspectionNo;
        private System.Windows.Forms.Label labelInspectionNo;
        private System.Windows.Forms.Label txtModelName;
        private System.Windows.Forms.Label labelModelName;

        private System.Windows.Forms.Panel panelMainView;
        private System.Windows.Forms.Panel panelViewRight;

        private System.Windows.Forms.Label labelDefect;
        private System.Windows.Forms.Label labelAccept;
        private System.Windows.Forms.Label labelInspTotal;
        private System.Windows.Forms.Label txtTotal;
        private System.Windows.Forms.Label txtGood;
        private System.Windows.Forms.Label txtNg;
        private System.Windows.Forms.Label txtInspectionTime;
        private System.Windows.Forms.Label labelInspectionTime;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnResetCount;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelModules;
        private System.Windows.Forms.Label txtTotalModules;
        private System.Windows.Forms.Label labelInspectResult;
        private System.Windows.Forms.Label txtInspResult;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lblContinuousDefect;

        private System.Windows.Forms.Panel pnlConveyorSystem;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label labelCycleTime;
        private System.Windows.Forms.Label txtCycleTime;

        private System.Windows.Forms.Label lblIsPassMode;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.DataGridView dgvLastInspectionResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn inspectionNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn modelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn result;
        private System.Windows.Forms.Panel panelLargeView;
        private System.Windows.Forms.Label lblTestMode;
        private ReaLTaiizor.HopeToggle tgsTestMode;
    }
}
