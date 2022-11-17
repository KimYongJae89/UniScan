namespace UniScan.UI
{
    partial class TeachingPage
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.dustPosChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.imageSplitContainer = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripInspection = new System.Windows.Forms.ToolStripButton();
            this.toolStripGrab = new System.Windows.Forms.ToolStripButton();
            this.toolStripLoadImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSaveImage = new System.Windows.Forms.ToolStripButton();
            this.toolStripZoomInButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripZoomOutButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripZoomFitButton = new System.Windows.Forms.ToolStripButton();
            this.tabControlData = new System.Windows.Forms.TabControl();
            this.tabPageDustList = new System.Windows.Forms.TabPage();
            this.dustList = new System.Windows.Forms.DataGridView();
            this.columnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnPosX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnPosY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnWidthPx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnHeightPx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnMaxIntensity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageSummaryTable = new System.Windows.Forms.TabPage();
            this.summaryTable = new System.Windows.Forms.DataGridView();
            this.columnSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPagePosChart = new System.Windows.Forms.TabPage();
            this.tabPageSizeChart = new System.Windows.Forms.TabPage();
            this.chartSplitContainer = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.sectionComboBox = new System.Windows.Forms.ComboBox();
            this.dustSizeChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.labelNumSection = new System.Windows.Forms.Label();
            this.nudNumSection = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboLRselector = new System.Windows.Forms.ComboBox();
            this.groupBoxMinSize = new System.Windows.Forms.GroupBox();
            this.labelMinSize = new System.Windows.Forms.Label();
            this.nudMinSize = new System.Windows.Forms.NumericUpDown();
            this.nudMinSizeThreshold = new System.Windows.Forms.NumericUpDown();
            this.labelMinSizeThreshold = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonStart = new System.Windows.Forms.Button();
            this.checkBoxShowResult = new System.Windows.Forms.CheckBox();
            this.buttonSaveData = new Infragistics.Win.Misc.UltraButton();
            this.groupBoxScan = new System.Windows.Forms.GroupBox();
            this.nudScanEnd = new System.Windows.Forms.NumericUpDown();
            this.labelScanEnd = new System.Windows.Forms.Label();
            this.nudScanStart = new System.Windows.Forms.NumericUpDown();
            this.labelScanStart = new System.Windows.Forms.Label();
            this.groupboxSection = new System.Windows.Forms.GroupBox();
            this.nudSectionEnd = new System.Windows.Forms.NumericUpDown();
            this.labelSectionEnd = new System.Windows.Forms.Label();
            this.nudSectionStart = new System.Windows.Forms.NumericUpDown();
            this.labelSectionStart = new System.Windows.Forms.Label();
            this.checkBoxShowSection = new System.Windows.Forms.CheckBox();
            this.checkBoxDetectWhite = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nudMotionSpeed = new System.Windows.Forms.NumericUpDown();
            this.moveHome = new Infragistics.Win.Misc.UltraButton();
            this.labelLongHeight = new System.Windows.Forms.Label();
            this.labelResolution = new System.Windows.Forms.Label();
            this.labelThreshold = new System.Windows.Forms.Label();
            this.nudLongHeight = new System.Windows.Forms.NumericUpDown();
            this.nudResolution = new System.Windows.Forms.NumericUpDown();
            this.nudThreshold = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dustPosChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageSplitContainer)).BeginInit();
            this.imageSplitContainer.Panel1.SuspendLayout();
            this.imageSplitContainer.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabControlData.SuspendLayout();
            this.tabPageDustList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dustList)).BeginInit();
            this.tabPageSummaryTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.summaryTable)).BeginInit();
            this.tabPagePosChart.SuspendLayout();
            this.tabPageSizeChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSplitContainer)).BeginInit();
            this.chartSplitContainer.Panel1.SuspendLayout();
            this.chartSplitContainer.Panel2.SuspendLayout();
            this.chartSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dustSizeChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumSection)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBoxMinSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinSizeThreshold)).BeginInit();
            this.panelTop.SuspendLayout();
            this.groupBoxScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScanEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudScanStart)).BeginInit();
            this.groupboxSection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSectionEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSectionStart)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMotionSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLongHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudResolution)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshold)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dustPosChart
            // 
            chartArea1.Name = "ChartArea";
            this.dustPosChart.ChartAreas.Add(chartArea1);
            this.dustPosChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dustPosChart.Location = new System.Drawing.Point(3, 3);
            this.dustPosChart.Name = "dustPosChart";
            this.dustPosChart.Size = new System.Drawing.Size(664, 384);
            this.dustPosChart.TabIndex = 2;
            this.dustPosChart.Text = "Defect Chart";
            // 
            // imageSplitContainer
            // 
            this.imageSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.imageSplitContainer.Name = "imageSplitContainer";
            this.imageSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // imageSplitContainer.Panel1
            // 
            this.imageSplitContainer.Panel1.BackColor = System.Drawing.Color.White;
            this.imageSplitContainer.Panel1.Controls.Add(this.toolStrip1);
            this.imageSplitContainer.Size = new System.Drawing.Size(407, 801);
            this.imageSplitContainer.SplitterDistance = 71;
            this.imageSplitContainer.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(17, 17);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripInspection,
            this.toolStripGrab,
            this.toolStripLoadImage,
            this.toolStripSaveImage,
            this.toolStripZoomInButton,
            this.toolStripZoomOutButton,
            this.toolStripZoomFitButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(407, 71);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripInspection
            // 
            this.toolStripInspection.Font = new System.Drawing.Font("맑은 고딕", 12.11881F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripInspection.Image = global::UniScan.Properties.Resources.Inspect;
            this.toolStripInspection.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripInspection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripInspection.Name = "toolStripInspection";
            this.toolStripInspection.Size = new System.Drawing.Size(39, 68);
            this.toolStripInspection.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripInspection.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripInspection.Click += new System.EventHandler(this.toolStripInspection_Click);
            // 
            // toolStripGrab
            // 
            this.toolStripGrab.Font = new System.Drawing.Font("맑은 고딕", 12.11881F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripGrab.Image = global::UniScan.Properties.Resources.grab;
            this.toolStripGrab.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripGrab.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripGrab.Name = "toolStripGrab";
            this.toolStripGrab.Size = new System.Drawing.Size(46, 68);
            this.toolStripGrab.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripGrab.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripGrab.Click += new System.EventHandler(this.toolStripGrab_Click);
            // 
            // toolStripLoadImage
            // 
            this.toolStripLoadImage.Font = new System.Drawing.Font("맑은 고딕", 12.11881F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripLoadImage.Image = global::UniScan.Properties.Resources.LoadIMage;
            this.toolStripLoadImage.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripLoadImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripLoadImage.Name = "toolStripLoadImage";
            this.toolStripLoadImage.Size = new System.Drawing.Size(53, 68);
            this.toolStripLoadImage.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripLoadImage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripLoadImage.Click += new System.EventHandler(this.toolStripLoadImage_Click);
            // 
            // toolStripSaveImage
            // 
            this.toolStripSaveImage.Font = new System.Drawing.Font("맑은 고딕", 12.11881F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripSaveImage.Image = global::UniScan.Properties.Resources.SaveIMage;
            this.toolStripSaveImage.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripSaveImage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSaveImage.Name = "toolStripSaveImage";
            this.toolStripSaveImage.Size = new System.Drawing.Size(47, 68);
            this.toolStripSaveImage.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripSaveImage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripSaveImage.Click += new System.EventHandler(this.toolStripSaveImage_Click);
            // 
            // toolStripZoomInButton
            // 
            this.toolStripZoomInButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripZoomInButton.Font = new System.Drawing.Font("맑은 고딕", 12.11881F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripZoomInButton.Image = global::UniScan.Properties.Resources.zoom_in_32;
            this.toolStripZoomInButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripZoomInButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripZoomInButton.Name = "toolStripZoomInButton";
            this.toolStripZoomInButton.Size = new System.Drawing.Size(40, 68);
            this.toolStripZoomInButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripZoomInButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripZoomInButton.Click += new System.EventHandler(this.toolStripZoomInButton_Click);
            // 
            // toolStripZoomOutButton
            // 
            this.toolStripZoomOutButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripZoomOutButton.Font = new System.Drawing.Font("맑은 고딕", 12.11881F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripZoomOutButton.Image = global::UniScan.Properties.Resources.zoom_out_32;
            this.toolStripZoomOutButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripZoomOutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripZoomOutButton.Name = "toolStripZoomOutButton";
            this.toolStripZoomOutButton.Size = new System.Drawing.Size(40, 68);
            this.toolStripZoomOutButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripZoomOutButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripZoomOutButton.Click += new System.EventHandler(this.toolStripZoomOutButton_Click);
            // 
            // toolStripZoomFitButton
            // 
            this.toolStripZoomFitButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripZoomFitButton.Font = new System.Drawing.Font("맑은 고딕", 12.11881F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripZoomFitButton.Image = global::UniScan.Properties.Resources.zoom_fit_32;
            this.toolStripZoomFitButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripZoomFitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripZoomFitButton.Name = "toolStripZoomFitButton";
            this.toolStripZoomFitButton.Size = new System.Drawing.Size(40, 68);
            this.toolStripZoomFitButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripZoomFitButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripZoomFitButton.Click += new System.EventHandler(this.toolStripZoomFitButton_Click);
            // 
            // tabControlData
            // 
            this.tabControlData.Controls.Add(this.tabPageDustList);
            this.tabControlData.Controls.Add(this.tabPageSummaryTable);
            this.tabControlData.Controls.Add(this.tabPagePosChart);
            this.tabControlData.Controls.Add(this.tabPageSizeChart);
            this.tabControlData.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControlData.Location = new System.Drawing.Point(416, 14);
            this.tabControlData.Name = "tabControlData";
            this.tabControlData.SelectedIndex = 0;
            this.tabControlData.Size = new System.Drawing.Size(678, 790);
            this.tabControlData.TabIndex = 4;
            // 
            // tabPageDustList
            // 
            this.tabPageDustList.Controls.Add(this.dustList);
            this.tabPageDustList.Location = new System.Drawing.Point(4, 37);
            this.tabPageDustList.Name = "tabPageDustList";
            this.tabPageDustList.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDustList.Size = new System.Drawing.Size(670, 749);
            this.tabPageDustList.TabIndex = 0;
            this.tabPageDustList.Text = "Dust List";
            this.tabPageDustList.UseVisualStyleBackColor = true;
            // 
            // dustList
            // 
            this.dustList.AllowUserToAddRows = false;
            this.dustList.AllowUserToDeleteRows = false;
            this.dustList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dustList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnNo,
            this.columnPosX,
            this.columnPosY,
            this.columnWidth,
            this.columnHeight,
            this.columnArea,
            this.columnWidthPx,
            this.columnHeightPx,
            this.columnMaxIntensity});
            this.dustList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dustList.Location = new System.Drawing.Point(3, 3);
            this.dustList.MultiSelect = false;
            this.dustList.Name = "dustList";
            this.dustList.RowHeadersVisible = false;
            this.dustList.RowTemplate.Height = 23;
            this.dustList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dustList.Size = new System.Drawing.Size(664, 743);
            this.dustList.TabIndex = 0;
            this.dustList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dustList_CellClick);
            // 
            // columnNo
            // 
            this.columnNo.HeaderText = "No";
            this.columnNo.Name = "columnNo";
            this.columnNo.Width = 60;
            // 
            // columnPosX
            // 
            this.columnPosX.HeaderText = "X";
            this.columnPosX.Name = "columnPosX";
            this.columnPosX.Width = 90;
            // 
            // columnPosY
            // 
            this.columnPosY.HeaderText = "Y";
            this.columnPosY.Name = "columnPosY";
            this.columnPosY.Width = 90;
            // 
            // columnWidth
            // 
            this.columnWidth.HeaderText = "Width";
            this.columnWidth.Name = "columnWidth";
            this.columnWidth.Width = 90;
            // 
            // columnHeight
            // 
            this.columnHeight.HeaderText = "Height";
            this.columnHeight.Name = "columnHeight";
            this.columnHeight.Width = 90;
            // 
            // columnArea
            // 
            this.columnArea.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnArea.HeaderText = "Area";
            this.columnArea.Name = "columnArea";
            // 
            // columnWidthPx
            // 
            this.columnWidthPx.HeaderText = "Width(px)";
            this.columnWidthPx.Name = "columnWidthPx";
            // 
            // columnHeightPx
            // 
            this.columnHeightPx.HeaderText = "Height(px)";
            this.columnHeightPx.Name = "columnHeightPx";
            // 
            // columnMaxIntensity
            // 
            this.columnMaxIntensity.HeaderText = "Intensity";
            this.columnMaxIntensity.Name = "columnMaxIntensity";
            // 
            // tabPageSummaryTable
            // 
            this.tabPageSummaryTable.Controls.Add(this.summaryTable);
            this.tabPageSummaryTable.Location = new System.Drawing.Point(4, 37);
            this.tabPageSummaryTable.Name = "tabPageSummaryTable";
            this.tabPageSummaryTable.Size = new System.Drawing.Size(670, 390);
            this.tabPageSummaryTable.TabIndex = 3;
            this.tabPageSummaryTable.Text = "Summary Table";
            this.tabPageSummaryTable.UseVisualStyleBackColor = true;
            // 
            // summaryTable
            // 
            this.summaryTable.AllowUserToAddRows = false;
            this.summaryTable.AllowUserToDeleteRows = false;
            this.summaryTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.summaryTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnSize});
            this.summaryTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.summaryTable.Location = new System.Drawing.Point(0, 0);
            this.summaryTable.MultiSelect = false;
            this.summaryTable.Name = "summaryTable";
            this.summaryTable.RowHeadersVisible = false;
            this.summaryTable.RowTemplate.Height = 23;
            this.summaryTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.summaryTable.Size = new System.Drawing.Size(670, 390);
            this.summaryTable.TabIndex = 1;
            // 
            // columnSize
            // 
            this.columnSize.HeaderText = "Size";
            this.columnSize.Name = "columnSize";
            this.columnSize.Width = 60;
            // 
            // tabPagePosChart
            // 
            this.tabPagePosChart.Controls.Add(this.dustPosChart);
            this.tabPagePosChart.Location = new System.Drawing.Point(4, 37);
            this.tabPagePosChart.Name = "tabPagePosChart";
            this.tabPagePosChart.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePosChart.Size = new System.Drawing.Size(670, 390);
            this.tabPagePosChart.TabIndex = 1;
            this.tabPagePosChart.Text = "Pos Chart";
            this.tabPagePosChart.UseVisualStyleBackColor = true;
            // 
            // tabPageSizeChart
            // 
            this.tabPageSizeChart.Controls.Add(this.chartSplitContainer);
            this.tabPageSizeChart.Location = new System.Drawing.Point(4, 37);
            this.tabPageSizeChart.Name = "tabPageSizeChart";
            this.tabPageSizeChart.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSizeChart.Size = new System.Drawing.Size(670, 390);
            this.tabPageSizeChart.TabIndex = 2;
            this.tabPageSizeChart.Text = "Size Chart";
            this.tabPageSizeChart.UseVisualStyleBackColor = true;
            // 
            // chartSplitContainer
            // 
            this.chartSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.chartSplitContainer.Name = "chartSplitContainer";
            this.chartSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // chartSplitContainer.Panel1
            // 
            this.chartSplitContainer.Panel1.Controls.Add(this.label2);
            this.chartSplitContainer.Panel1.Controls.Add(this.sectionComboBox);
            // 
            // chartSplitContainer.Panel2
            // 
            this.chartSplitContainer.Panel2.Controls.Add(this.dustSizeChart);
            this.chartSplitContainer.Size = new System.Drawing.Size(664, 384);
            this.chartSplitContainer.SplitterDistance = 25;
            this.chartSplitContainer.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 28);
            this.label2.TabIndex = 1;
            this.label2.Text = "Section";
            // 
            // sectionComboBox
            // 
            this.sectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sectionComboBox.FormattingEnabled = true;
            this.sectionComboBox.Location = new System.Drawing.Point(91, 7);
            this.sectionComboBox.Name = "sectionComboBox";
            this.sectionComboBox.Size = new System.Drawing.Size(121, 36);
            this.sectionComboBox.TabIndex = 0;
            this.sectionComboBox.SelectedIndexChanged += new System.EventHandler(this.sectionComboBox_SelectedIndexChanged);
            // 
            // dustSizeChart
            // 
            chartArea2.Name = "ChartArea";
            this.dustSizeChart.ChartAreas.Add(chartArea2);
            this.dustSizeChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dustSizeChart.Location = new System.Drawing.Point(0, 0);
            this.dustSizeChart.Name = "dustSizeChart";
            this.dustSizeChart.Size = new System.Drawing.Size(664, 355);
            this.dustSizeChart.TabIndex = 4;
            this.dustSizeChart.Text = "Defect Chart";
            // 
            // labelNumSection
            // 
            this.labelNumSection.AutoSize = true;
            this.labelNumSection.Location = new System.Drawing.Point(7, 24);
            this.labelNumSection.Name = "labelNumSection";
            this.labelNumSection.Size = new System.Drawing.Size(57, 28);
            this.labelNumSection.TabIndex = 5;
            this.labelNumSection.Text = "Num";
            // 
            // nudNumSection
            // 
            this.nudNumSection.Location = new System.Drawing.Point(102, 20);
            this.nudNumSection.Name = "nudNumSection";
            this.nudNumSection.Size = new System.Drawing.Size(62, 34);
            this.nudNumSection.TabIndex = 6;
            this.nudNumSection.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.comboLRselector);
            this.panel1.Controls.Add(this.groupBoxMinSize);
            this.panel1.Controls.Add(this.panelTop);
            this.panel1.Controls.Add(this.checkBoxShowResult);
            this.panel1.Controls.Add(this.buttonSaveData);
            this.panel1.Controls.Add(this.groupBoxScan);
            this.panel1.Controls.Add(this.groupboxSection);
            this.panel1.Controls.Add(this.checkBoxShowSection);
            this.panel1.Controls.Add(this.checkBoxDetectWhite);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.labelLongHeight);
            this.panel1.Controls.Add(this.labelResolution);
            this.panel1.Controls.Add(this.labelThreshold);
            this.panel1.Controls.Add(this.nudLongHeight);
            this.panel1.Controls.Add(this.nudResolution);
            this.panel1.Controls.Add(this.nudThreshold);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(188, 807);
            this.panel1.TabIndex = 7;
            // 
            // comboLRselector
            // 
            this.comboLRselector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLRselector.FormattingEnabled = true;
            this.comboLRselector.Items.AddRange(new object[] {
            "Left To Right",
            "Right To Left"});
            this.comboLRselector.Location = new System.Drawing.Point(7, 429);
            this.comboLRselector.Name = "comboLRselector";
            this.comboLRselector.Size = new System.Drawing.Size(163, 36);
            this.comboLRselector.TabIndex = 0;
            // 
            // groupBoxMinSize
            // 
            this.groupBoxMinSize.BackColor = System.Drawing.Color.White;
            this.groupBoxMinSize.Controls.Add(this.labelMinSize);
            this.groupBoxMinSize.Controls.Add(this.nudMinSize);
            this.groupBoxMinSize.Controls.Add(this.nudMinSizeThreshold);
            this.groupBoxMinSize.Controls.Add(this.labelMinSizeThreshold);
            this.groupBoxMinSize.Location = new System.Drawing.Point(4, 215);
            this.groupBoxMinSize.Name = "groupBoxMinSize";
            this.groupBoxMinSize.Size = new System.Drawing.Size(171, 88);
            this.groupBoxMinSize.TabIndex = 14;
            this.groupBoxMinSize.TabStop = false;
            this.groupBoxMinSize.Text = "Min Defect";
            // 
            // labelMinSize
            // 
            this.labelMinSize.AutoSize = true;
            this.labelMinSize.Location = new System.Drawing.Point(5, 26);
            this.labelMinSize.Name = "labelMinSize";
            this.labelMinSize.Size = new System.Drawing.Size(48, 28);
            this.labelMinSize.TabIndex = 5;
            this.labelMinSize.Text = "Size";
            // 
            // nudMinSize
            // 
            this.nudMinSize.Location = new System.Drawing.Point(102, 21);
            this.nudMinSize.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMinSize.Name = "nudMinSize";
            this.nudMinSize.Size = new System.Drawing.Size(62, 34);
            this.nudMinSize.TabIndex = 6;
            this.nudMinSize.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // nudMinSizeThreshold
            // 
            this.nudMinSizeThreshold.Location = new System.Drawing.Point(102, 52);
            this.nudMinSizeThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMinSizeThreshold.Name = "nudMinSizeThreshold";
            this.nudMinSizeThreshold.Size = new System.Drawing.Size(62, 34);
            this.nudMinSizeThreshold.TabIndex = 6;
            this.nudMinSizeThreshold.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // labelMinSizeThreshold
            // 
            this.labelMinSizeThreshold.AutoSize = true;
            this.labelMinSizeThreshold.Location = new System.Drawing.Point(5, 57);
            this.labelMinSizeThreshold.Name = "labelMinSizeThreshold";
            this.labelMinSizeThreshold.Size = new System.Drawing.Size(103, 28);
            this.labelMinSizeThreshold.TabIndex = 5;
            this.labelMinSizeThreshold.Text = "Threshold";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.buttonStart);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(188, 116);
            this.panelTop.TabIndex = 13;
            // 
            // buttonStart
            // 
            this.buttonStart.BackColor = System.Drawing.Color.White;
            this.buttonStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStart.Image = global::UniScan.Properties.Resources.Start;
            this.buttonStart.Location = new System.Drawing.Point(3, 3);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(182, 110);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // checkBoxShowResult
            // 
            this.checkBoxShowResult.AutoSize = true;
            this.checkBoxShowResult.Checked = true;
            this.checkBoxShowResult.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowResult.Location = new System.Drawing.Point(10, 598);
            this.checkBoxShowResult.Name = "checkBoxShowResult";
            this.checkBoxShowResult.Size = new System.Drawing.Size(147, 32);
            this.checkBoxShowResult.TabIndex = 11;
            this.checkBoxShowResult.Text = "Show Result";
            this.checkBoxShowResult.UseVisualStyleBackColor = true;
            this.checkBoxShowResult.CheckedChanged += new System.EventHandler(this.checkBoxShowResult_CheckedChanged);
            // 
            // buttonSaveData
            // 
            this.buttonSaveData.Location = new System.Drawing.Point(4, 639);
            this.buttonSaveData.Name = "buttonSaveData";
            this.buttonSaveData.Size = new System.Drawing.Size(178, 46);
            this.buttonSaveData.TabIndex = 10;
            this.buttonSaveData.Text = "Save Data";
            this.buttonSaveData.Click += new System.EventHandler(this.buttonSaveData_Click);
            // 
            // groupBoxScan
            // 
            this.groupBoxScan.Controls.Add(this.nudScanEnd);
            this.groupBoxScan.Controls.Add(this.labelScanEnd);
            this.groupBoxScan.Controls.Add(this.nudScanStart);
            this.groupBoxScan.Controls.Add(this.labelScanStart);
            this.groupBoxScan.Location = new System.Drawing.Point(6, 459);
            this.groupBoxScan.Name = "groupBoxScan";
            this.groupBoxScan.Size = new System.Drawing.Size(171, 91);
            this.groupBoxScan.TabIndex = 9;
            this.groupBoxScan.TabStop = false;
            this.groupBoxScan.Text = "Scan";
            // 
            // nudScanEnd
            // 
            this.nudScanEnd.Location = new System.Drawing.Point(85, 54);
            this.nudScanEnd.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudScanEnd.Name = "nudScanEnd";
            this.nudScanEnd.Size = new System.Drawing.Size(79, 34);
            this.nudScanEnd.TabIndex = 6;
            this.nudScanEnd.Value = new decimal(new int[] {
            300000,
            0,
            0,
            0});
            // 
            // labelScanEnd
            // 
            this.labelScanEnd.AutoSize = true;
            this.labelScanEnd.Location = new System.Drawing.Point(7, 56);
            this.labelScanEnd.Name = "labelScanEnd";
            this.labelScanEnd.Size = new System.Drawing.Size(46, 28);
            this.labelScanEnd.TabIndex = 5;
            this.labelScanEnd.Text = "End";
            // 
            // nudScanStart
            // 
            this.nudScanStart.Location = new System.Drawing.Point(85, 23);
            this.nudScanStart.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudScanStart.Name = "nudScanStart";
            this.nudScanStart.Size = new System.Drawing.Size(79, 34);
            this.nudScanStart.TabIndex = 6;
            this.nudScanStart.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // labelScanStart
            // 
            this.labelScanStart.AutoSize = true;
            this.labelScanStart.Location = new System.Drawing.Point(7, 25);
            this.labelScanStart.Name = "labelScanStart";
            this.labelScanStart.Size = new System.Drawing.Size(54, 28);
            this.labelScanStart.TabIndex = 5;
            this.labelScanStart.Text = "Start";
            // 
            // groupboxSection
            // 
            this.groupboxSection.Controls.Add(this.nudNumSection);
            this.groupboxSection.Controls.Add(this.labelNumSection);
            this.groupboxSection.Controls.Add(this.nudSectionEnd);
            this.groupboxSection.Controls.Add(this.labelSectionEnd);
            this.groupboxSection.Controls.Add(this.nudSectionStart);
            this.groupboxSection.Controls.Add(this.labelSectionStart);
            this.groupboxSection.Location = new System.Drawing.Point(4, 306);
            this.groupboxSection.Name = "groupboxSection";
            this.groupboxSection.Size = new System.Drawing.Size(171, 117);
            this.groupboxSection.TabIndex = 9;
            this.groupboxSection.TabStop = false;
            this.groupboxSection.Text = "Section";
            // 
            // nudSectionEnd
            // 
            this.nudSectionEnd.Location = new System.Drawing.Point(102, 82);
            this.nudSectionEnd.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.nudSectionEnd.Name = "nudSectionEnd";
            this.nudSectionEnd.Size = new System.Drawing.Size(62, 34);
            this.nudSectionEnd.TabIndex = 6;
            this.nudSectionEnd.Value = new decimal(new int[] {
            18000,
            0,
            0,
            0});
            // 
            // labelSectionEnd
            // 
            this.labelSectionEnd.AutoSize = true;
            this.labelSectionEnd.Location = new System.Drawing.Point(7, 84);
            this.labelSectionEnd.Name = "labelSectionEnd";
            this.labelSectionEnd.Size = new System.Drawing.Size(46, 28);
            this.labelSectionEnd.TabIndex = 5;
            this.labelSectionEnd.Text = "End";
            // 
            // nudSectionStart
            // 
            this.nudSectionStart.Location = new System.Drawing.Point(102, 51);
            this.nudSectionStart.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.nudSectionStart.Name = "nudSectionStart";
            this.nudSectionStart.Size = new System.Drawing.Size(62, 34);
            this.nudSectionStart.TabIndex = 6;
            this.nudSectionStart.Value = new decimal(new int[] {
            1800,
            0,
            0,
            0});
            // 
            // labelSectionStart
            // 
            this.labelSectionStart.AutoSize = true;
            this.labelSectionStart.Location = new System.Drawing.Point(7, 53);
            this.labelSectionStart.Name = "labelSectionStart";
            this.labelSectionStart.Size = new System.Drawing.Size(54, 28);
            this.labelSectionStart.TabIndex = 5;
            this.labelSectionStart.Text = "Start";
            // 
            // checkBoxShowSection
            // 
            this.checkBoxShowSection.AutoSize = true;
            this.checkBoxShowSection.Checked = true;
            this.checkBoxShowSection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowSection.Location = new System.Drawing.Point(10, 575);
            this.checkBoxShowSection.Name = "checkBoxShowSection";
            this.checkBoxShowSection.Size = new System.Drawing.Size(158, 32);
            this.checkBoxShowSection.TabIndex = 8;
            this.checkBoxShowSection.Text = "Show Section";
            this.checkBoxShowSection.UseVisualStyleBackColor = true;
            // 
            // checkBoxDetectWhite
            // 
            this.checkBoxDetectWhite.AutoSize = true;
            this.checkBoxDetectWhite.Checked = true;
            this.checkBoxDetectWhite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDetectWhite.Location = new System.Drawing.Point(10, 552);
            this.checkBoxDetectWhite.Name = "checkBoxDetectWhite";
            this.checkBoxDetectWhite.Size = new System.Drawing.Size(154, 32);
            this.checkBoxDetectWhite.TabIndex = 8;
            this.checkBoxDetectWhite.Text = "Detect White";
            this.checkBoxDetectWhite.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.nudMotionSpeed);
            this.groupBox1.Controls.Add(this.moveHome);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 700);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(188, 107);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Robot";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(102, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 28);
            this.label1.TabIndex = 8;
            this.label1.Text = "[mm/s]";
            // 
            // nudMotionSpeed
            // 
            this.nudMotionSpeed.DecimalPlaces = 2;
            this.nudMotionSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudMotionSpeed.Location = new System.Drawing.Point(6, 26);
            this.nudMotionSpeed.Maximum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.nudMotionSpeed.Name = "nudMotionSpeed";
            this.nudMotionSpeed.Size = new System.Drawing.Size(90, 34);
            this.nudMotionSpeed.TabIndex = 7;
            this.nudMotionSpeed.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // moveHome
            // 
            this.moveHome.Location = new System.Drawing.Point(6, 61);
            this.moveHome.Name = "moveHome";
            this.moveHome.Size = new System.Drawing.Size(176, 39);
            this.moveHome.TabIndex = 1;
            this.moveHome.Text = "Home";
            this.moveHome.Click += new System.EventHandler(this.moveHome_Click);
            // 
            // labelLongHeight
            // 
            this.labelLongHeight.AutoSize = true;
            this.labelLongHeight.Location = new System.Drawing.Point(3, 186);
            this.labelLongHeight.Name = "labelLongHeight";
            this.labelLongHeight.Size = new System.Drawing.Size(126, 28);
            this.labelLongHeight.TabIndex = 5;
            this.labelLongHeight.Text = "Long Height";
            // 
            // labelResolution
            // 
            this.labelResolution.AutoSize = true;
            this.labelResolution.Location = new System.Drawing.Point(3, 155);
            this.labelResolution.Name = "labelResolution";
            this.labelResolution.Size = new System.Drawing.Size(109, 28);
            this.labelResolution.TabIndex = 5;
            this.labelResolution.Text = "Resolution";
            // 
            // labelThreshold
            // 
            this.labelThreshold.AutoSize = true;
            this.labelThreshold.Location = new System.Drawing.Point(4, 124);
            this.labelThreshold.Name = "labelThreshold";
            this.labelThreshold.Size = new System.Drawing.Size(103, 28);
            this.labelThreshold.TabIndex = 5;
            this.labelThreshold.Text = "Threshold";
            // 
            // nudLongHeight
            // 
            this.nudLongHeight.Location = new System.Drawing.Point(106, 181);
            this.nudLongHeight.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudLongHeight.Name = "nudLongHeight";
            this.nudLongHeight.Size = new System.Drawing.Size(66, 34);
            this.nudLongHeight.TabIndex = 6;
            this.nudLongHeight.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // nudResolution
            // 
            this.nudResolution.DecimalPlaces = 3;
            this.nudResolution.Location = new System.Drawing.Point(106, 150);
            this.nudResolution.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudResolution.Name = "nudResolution";
            this.nudResolution.Size = new System.Drawing.Size(66, 34);
            this.nudResolution.TabIndex = 6;
            this.nudResolution.Value = new decimal(new int[] {
            4219,
            0,
            0,
            196608});
            // 
            // nudThreshold
            // 
            this.nudThreshold.Location = new System.Drawing.Point(106, 119);
            this.nudThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudThreshold.Name = "nudThreshold";
            this.nudThreshold.Size = new System.Drawing.Size(66, 34);
            this.nudThreshold.TabIndex = 6;
            this.nudThreshold.Value = new decimal(new int[] {
            75,
            0,
            0,
            0});
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.73929F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.26071F));
            this.tableLayoutPanel1.Controls.Add(this.imageSplitContainer, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControlData, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(188, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1097, 807);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // TeachingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "TeachingPage";
            this.Size = new System.Drawing.Size(1285, 807);
            this.Load += new System.EventHandler(this.TeachingPage_Load);
            this.VisibleChanged += new System.EventHandler(this.TeachingPage_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dustPosChart)).EndInit();
            this.imageSplitContainer.Panel1.ResumeLayout(false);
            this.imageSplitContainer.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageSplitContainer)).EndInit();
            this.imageSplitContainer.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControlData.ResumeLayout(false);
            this.tabPageDustList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dustList)).EndInit();
            this.tabPageSummaryTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.summaryTable)).EndInit();
            this.tabPagePosChart.ResumeLayout(false);
            this.tabPageSizeChart.ResumeLayout(false);
            this.chartSplitContainer.Panel1.ResumeLayout(false);
            this.chartSplitContainer.Panel1.PerformLayout();
            this.chartSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartSplitContainer)).EndInit();
            this.chartSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dustSizeChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumSection)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBoxMinSize.ResumeLayout(false);
            this.groupBoxMinSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinSizeThreshold)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.groupBoxScan.ResumeLayout(false);
            this.groupBoxScan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudScanEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudScanStart)).EndInit();
            this.groupboxSection.ResumeLayout(false);
            this.groupboxSection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSectionEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSectionStart)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMotionSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLongHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudResolution)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThreshold)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart dustPosChart;
        private System.Windows.Forms.TabControl tabControlData;
        private System.Windows.Forms.TabPage tabPageDustList;
        private System.Windows.Forms.DataGridView dustList;
        private System.Windows.Forms.TabPage tabPagePosChart;
        private System.Windows.Forms.Label labelNumSection;
        private System.Windows.Forms.NumericUpDown nudNumSection;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Infragistics.Win.Misc.UltraButton moveHome;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudMotionSpeed;
        private System.Windows.Forms.Label labelThreshold;
        private System.Windows.Forms.NumericUpDown nudThreshold;
        private System.Windows.Forms.CheckBox checkBoxDetectWhite;
        private System.Windows.Forms.TabPage tabPageSizeChart;
        private System.Windows.Forms.TabPage tabPageSummaryTable;
        private System.Windows.Forms.DataGridView summaryTable;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnSize;
        private System.Windows.Forms.Label labelResolution;
        private System.Windows.Forms.NumericUpDown nudResolution;
        private System.Windows.Forms.Label labelSectionStart;
        private System.Windows.Forms.NumericUpDown nudSectionStart;
        private System.Windows.Forms.GroupBox groupboxSection;
        private System.Windows.Forms.NumericUpDown nudSectionEnd;
        private System.Windows.Forms.Label labelSectionEnd;
        private System.Windows.Forms.GroupBox groupBoxScan;
        private System.Windows.Forms.NumericUpDown nudScanEnd;
        private System.Windows.Forms.Label labelScanEnd;
        private System.Windows.Forms.NumericUpDown nudScanStart;
        private System.Windows.Forms.Label labelScanStart;
        private System.Windows.Forms.CheckBox checkBoxShowSection;
        private Infragistics.Win.Misc.UltraButton buttonSaveData;
        private System.Windows.Forms.SplitContainer imageSplitContainer;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripZoomInButton;
        private System.Windows.Forms.ToolStripButton toolStripZoomOutButton;
        private System.Windows.Forms.ToolStripButton toolStripZoomFitButton;
        private System.Windows.Forms.SplitContainer chartSplitContainer;
        private System.Windows.Forms.DataVisualization.Charting.Chart dustSizeChart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox sectionComboBox;
        private System.Windows.Forms.CheckBox checkBoxShowResult;
        private System.Windows.Forms.FlowLayoutPanel panelTop;
        private System.Windows.Forms.Label labelMinSize;
        private System.Windows.Forms.NumericUpDown nudMinSize;
        private System.Windows.Forms.GroupBox groupBoxMinSize;
        private System.Windows.Forms.NumericUpDown nudMinSizeThreshold;
        private System.Windows.Forms.Label labelMinSizeThreshold;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPosX;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnPosY;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnWidthPx;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnHeightPx;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnMaxIntensity;
        private System.Windows.Forms.Label labelLongHeight;
        private System.Windows.Forms.NumericUpDown nudLongHeight;
        private System.Windows.Forms.ToolStripButton toolStripInspection;
        private System.Windows.Forms.ToolStripButton toolStripGrab;
        private System.Windows.Forms.ToolStripButton toolStripLoadImage;
        private System.Windows.Forms.ToolStripButton toolStripSaveImage;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.ComboBox comboLRselector;
    }
}
