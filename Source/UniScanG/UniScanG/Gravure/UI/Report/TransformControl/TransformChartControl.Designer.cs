namespace UniScanG.Gravure.UI.Report.TransformControl
{
    partial class TransformChartControl
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.layoutTransformMap = new System.Windows.Forms.TableLayoutPanel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.layoutTransformValue = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutSize = new System.Windows.Forms.TableLayoutPanel();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelSizeUnit = new System.Windows.Forms.Label();
            this.labelSizeW = new System.Windows.Forms.Label();
            this.labelSizeH = new System.Windows.Forms.Label();
            this.sizeW = new System.Windows.Forms.TextBox();
            this.sizeH = new System.Windows.Forms.TextBox();
            this.tableLayoutTranslation = new System.Windows.Forms.TableLayoutPanel();
            this.labelTranslationUnit = new System.Windows.Forms.Label();
            this.labelTranslationX = new System.Windows.Forms.Label();
            this.labelTranslationY = new System.Windows.Forms.Label();
            this.translationX = new System.Windows.Forms.TextBox();
            this.translationY = new System.Windows.Forms.TextBox();
            this.labelTranslation = new System.Windows.Forms.Label();
            this.tableLayoutSkewness = new System.Windows.Forms.TableLayoutPanel();
            this.labelSkewness = new System.Windows.Forms.Label();
            this.labelSkewnessUnit = new System.Windows.Forms.Label();
            this.labelSkewnessLtrb = new System.Windows.Forms.Label();
            this.labelSkewnessRtlb = new System.Windows.Forms.Label();
            this.skewnessLtrb = new System.Windows.Forms.TextBox();
            this.skewnessRtlb = new System.Windows.Forms.TextBox();
            this.tableLayoutRotation = new System.Windows.Forms.TableLayoutPanel();
            this.labelRotation = new System.Windows.Forms.Label();
            this.labelRotationUnit = new System.Windows.Forms.Label();
            this.rotation = new System.Windows.Forms.TextBox();
            this.layoutViewControl = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.showGuideCircle = new System.Windows.Forms.CheckBox();
            this.guideCircleSize = new System.Windows.Forms.NumericUpDown();
            this.labelGuideLineLengthUm = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.drawLines = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.labelScaleUnit = new System.Windows.Forms.Label();
            this.offsetScale = new System.Windows.Forms.NumericUpDown();
            this.useScale = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.showLabels = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelController = new System.Windows.Forms.Panel();
            this.layoutTransformMap.SuspendLayout();
            this.layoutTransformValue.SuspendLayout();
            this.tableLayoutSize.SuspendLayout();
            this.tableLayoutTranslation.SuspendLayout();
            this.tableLayoutSkewness.SuspendLayout();
            this.tableLayoutRotation.SuspendLayout();
            this.layoutViewControl.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guideCircleSize)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.offsetScale)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutTransformMap
            // 
            this.layoutTransformMap.BackColor = System.Drawing.SystemColors.Control;
            this.layoutTransformMap.ColumnCount = 1;
            this.layoutTransformMap.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTransformMap.Controls.Add(this.labelTitle, 0, 0);
            this.layoutTransformMap.Controls.Add(this.layoutTransformValue, 0, 3);
            this.layoutTransformMap.Controls.Add(this.layoutViewControl, 0, 1);
            this.layoutTransformMap.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.layoutTransformMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutTransformMap.Location = new System.Drawing.Point(0, 0);
            this.layoutTransformMap.Margin = new System.Windows.Forms.Padding(0);
            this.layoutTransformMap.Name = "layoutTransformMap";
            this.layoutTransformMap.RowCount = 4;
            this.layoutTransformMap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.layoutTransformMap.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutTransformMap.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutTransformMap.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutTransformMap.Size = new System.Drawing.Size(725, 510);
            this.layoutTransformMap.TabIndex = 72;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("Malgun Gothic", 16F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(0, 0);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(725, 35);
            this.labelTitle.TabIndex = 1;
            this.labelTitle.Text = "Transform";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutTransformValue
            // 
            this.layoutTransformValue.AutoSize = true;
            this.layoutTransformValue.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.layoutTransformValue.BackColor = System.Drawing.SystemColors.Control;
            this.layoutTransformValue.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutTransformValue.ColumnCount = 4;
            this.layoutTransformValue.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutTransformValue.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutTransformValue.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutTransformValue.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.layoutTransformValue.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutTransformValue.Controls.Add(this.tableLayoutSize, 3, 0);
            this.layoutTransformValue.Controls.Add(this.tableLayoutTranslation, 0, 0);
            this.layoutTransformValue.Controls.Add(this.tableLayoutSkewness, 2, 0);
            this.layoutTransformValue.Controls.Add(this.tableLayoutRotation, 1, 0);
            this.layoutTransformValue.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.layoutTransformValue.Location = new System.Drawing.Point(0, 442);
            this.layoutTransformValue.Margin = new System.Windows.Forms.Padding(0);
            this.layoutTransformValue.Name = "layoutTransformValue";
            this.layoutTransformValue.RowCount = 1;
            this.layoutTransformValue.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutTransformValue.Size = new System.Drawing.Size(725, 68);
            this.layoutTransformValue.TabIndex = 70;
            // 
            // tableLayoutSize
            // 
            this.tableLayoutSize.AutoSize = true;
            this.tableLayoutSize.ColumnCount = 3;
            this.tableLayoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutSize.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutSize.Controls.Add(this.labelSize, 0, 0);
            this.tableLayoutSize.Controls.Add(this.labelSizeUnit, 2, 2);
            this.tableLayoutSize.Controls.Add(this.labelSizeW, 0, 1);
            this.tableLayoutSize.Controls.Add(this.labelSizeH, 0, 2);
            this.tableLayoutSize.Controls.Add(this.sizeW, 1, 1);
            this.tableLayoutSize.Controls.Add(this.sizeH, 1, 2);
            this.tableLayoutSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutSize.Location = new System.Drawing.Point(544, 1);
            this.tableLayoutSize.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutSize.Name = "tableLayoutSize";
            this.tableLayoutSize.RowCount = 3;
            this.tableLayoutSize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutSize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutSize.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutSize.Size = new System.Drawing.Size(180, 66);
            this.tableLayoutSize.TabIndex = 3;
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.tableLayoutSize.SetColumnSpan(this.labelSize, 3);
            this.labelSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSize.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSize.Location = new System.Drawing.Point(4, 0);
            this.labelSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(172, 22);
            this.labelSize.TabIndex = 0;
            this.labelSize.Text = "Size";
            this.labelSize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSizeUnit
            // 
            this.labelSizeUnit.AutoSize = true;
            this.labelSizeUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSizeUnit.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSizeUnit.Location = new System.Drawing.Point(147, 44);
            this.labelSizeUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelSizeUnit.Name = "labelSizeUnit";
            this.labelSizeUnit.Size = new System.Drawing.Size(33, 22);
            this.labelSizeUnit.TabIndex = 2;
            this.labelSizeUnit.Text = "[um]";
            this.labelSizeUnit.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelSizeW
            // 
            this.labelSizeW.AutoSize = true;
            this.labelSizeW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSizeW.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSizeW.Location = new System.Drawing.Point(0, 22);
            this.labelSizeW.Margin = new System.Windows.Forms.Padding(0);
            this.labelSizeW.Name = "labelSizeW";
            this.labelSizeW.Size = new System.Drawing.Size(22, 22);
            this.labelSizeW.TabIndex = 2;
            this.labelSizeW.Text = "W";
            this.labelSizeW.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSizeH
            // 
            this.labelSizeH.AutoSize = true;
            this.labelSizeH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSizeH.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSizeH.Location = new System.Drawing.Point(0, 44);
            this.labelSizeH.Margin = new System.Windows.Forms.Padding(0);
            this.labelSizeH.Name = "labelSizeH";
            this.labelSizeH.Size = new System.Drawing.Size(22, 22);
            this.labelSizeH.TabIndex = 2;
            this.labelSizeH.Text = "H";
            this.labelSizeH.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sizeW
            // 
            this.sizeW.BackColor = System.Drawing.Color.White;
            this.sizeW.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sizeW.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sizeW.Location = new System.Drawing.Point(22, 22);
            this.sizeW.Margin = new System.Windows.Forms.Padding(0);
            this.sizeW.Name = "sizeW";
            this.sizeW.ReadOnly = true;
            this.sizeW.Size = new System.Drawing.Size(125, 22);
            this.sizeW.TabIndex = 1;
            this.sizeW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // sizeH
            // 
            this.sizeH.BackColor = System.Drawing.Color.White;
            this.sizeH.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sizeH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sizeH.Location = new System.Drawing.Point(22, 44);
            this.sizeH.Margin = new System.Windows.Forms.Padding(0);
            this.sizeH.Name = "sizeH";
            this.sizeH.ReadOnly = true;
            this.sizeH.Size = new System.Drawing.Size(125, 22);
            this.sizeH.TabIndex = 1;
            this.sizeH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tableLayoutTranslation
            // 
            this.tableLayoutTranslation.AutoSize = true;
            this.tableLayoutTranslation.ColumnCount = 3;
            this.tableLayoutTranslation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutTranslation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutTranslation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutTranslation.Controls.Add(this.labelTranslationUnit, 2, 2);
            this.tableLayoutTranslation.Controls.Add(this.labelTranslationX, 0, 1);
            this.tableLayoutTranslation.Controls.Add(this.labelTranslationY, 0, 2);
            this.tableLayoutTranslation.Controls.Add(this.translationX, 1, 1);
            this.tableLayoutTranslation.Controls.Add(this.translationY, 1, 2);
            this.tableLayoutTranslation.Controls.Add(this.labelTranslation, 0, 0);
            this.tableLayoutTranslation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutTranslation.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutTranslation.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutTranslation.Name = "tableLayoutTranslation";
            this.tableLayoutTranslation.RowCount = 3;
            this.tableLayoutTranslation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutTranslation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutTranslation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutTranslation.Size = new System.Drawing.Size(180, 66);
            this.tableLayoutTranslation.TabIndex = 3;
            // 
            // labelTranslationUnit
            // 
            this.labelTranslationUnit.AutoSize = true;
            this.labelTranslationUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTranslationUnit.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTranslationUnit.Location = new System.Drawing.Point(147, 44);
            this.labelTranslationUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelTranslationUnit.Name = "labelTranslationUnit";
            this.labelTranslationUnit.Size = new System.Drawing.Size(33, 22);
            this.labelTranslationUnit.TabIndex = 2;
            this.labelTranslationUnit.Text = "[um]";
            this.labelTranslationUnit.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelTranslationX
            // 
            this.labelTranslationX.AutoSize = true;
            this.labelTranslationX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTranslationX.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTranslationX.Location = new System.Drawing.Point(0, 22);
            this.labelTranslationX.Margin = new System.Windows.Forms.Padding(0);
            this.labelTranslationX.Name = "labelTranslationX";
            this.labelTranslationX.Size = new System.Drawing.Size(17, 22);
            this.labelTranslationX.TabIndex = 2;
            this.labelTranslationX.Text = "X";
            this.labelTranslationX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTranslationY
            // 
            this.labelTranslationY.AutoSize = true;
            this.labelTranslationY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTranslationY.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTranslationY.Location = new System.Drawing.Point(0, 44);
            this.labelTranslationY.Margin = new System.Windows.Forms.Padding(0);
            this.labelTranslationY.Name = "labelTranslationY";
            this.labelTranslationY.Size = new System.Drawing.Size(17, 22);
            this.labelTranslationY.TabIndex = 2;
            this.labelTranslationY.Text = "Y";
            this.labelTranslationY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // translationX
            // 
            this.translationX.BackColor = System.Drawing.Color.White;
            this.translationX.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.translationX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.translationX.Location = new System.Drawing.Point(17, 22);
            this.translationX.Margin = new System.Windows.Forms.Padding(0);
            this.translationX.Name = "translationX";
            this.translationX.ReadOnly = true;
            this.translationX.Size = new System.Drawing.Size(130, 22);
            this.translationX.TabIndex = 1;
            this.translationX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // translationY
            // 
            this.translationY.BackColor = System.Drawing.Color.White;
            this.translationY.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.translationY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.translationY.Location = new System.Drawing.Point(17, 44);
            this.translationY.Margin = new System.Windows.Forms.Padding(0);
            this.translationY.Name = "translationY";
            this.translationY.ReadOnly = true;
            this.translationY.Size = new System.Drawing.Size(130, 22);
            this.translationY.TabIndex = 1;
            this.translationY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelTranslation
            // 
            this.labelTranslation.AutoSize = true;
            this.tableLayoutTranslation.SetColumnSpan(this.labelTranslation, 3);
            this.labelTranslation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTranslation.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTranslation.Location = new System.Drawing.Point(4, 0);
            this.labelTranslation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTranslation.Name = "labelTranslation";
            this.labelTranslation.Size = new System.Drawing.Size(172, 22);
            this.labelTranslation.TabIndex = 0;
            this.labelTranslation.Text = "Translation";
            this.labelTranslation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutSkewness
            // 
            this.tableLayoutSkewness.AutoSize = true;
            this.tableLayoutSkewness.ColumnCount = 3;
            this.tableLayoutSkewness.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutSkewness.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutSkewness.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutSkewness.Controls.Add(this.labelSkewness, 0, 0);
            this.tableLayoutSkewness.Controls.Add(this.labelSkewnessUnit, 2, 2);
            this.tableLayoutSkewness.Controls.Add(this.labelSkewnessLtrb, 0, 1);
            this.tableLayoutSkewness.Controls.Add(this.labelSkewnessRtlb, 0, 2);
            this.tableLayoutSkewness.Controls.Add(this.skewnessLtrb, 1, 1);
            this.tableLayoutSkewness.Controls.Add(this.skewnessRtlb, 1, 2);
            this.tableLayoutSkewness.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutSkewness.Location = new System.Drawing.Point(363, 1);
            this.tableLayoutSkewness.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutSkewness.Name = "tableLayoutSkewness";
            this.tableLayoutSkewness.RowCount = 3;
            this.tableLayoutSkewness.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutSkewness.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutSkewness.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutSkewness.Size = new System.Drawing.Size(180, 66);
            this.tableLayoutSkewness.TabIndex = 3;
            // 
            // labelSkewness
            // 
            this.labelSkewness.AutoSize = true;
            this.tableLayoutSkewness.SetColumnSpan(this.labelSkewness, 3);
            this.labelSkewness.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSkewness.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSkewness.Location = new System.Drawing.Point(4, 0);
            this.labelSkewness.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSkewness.Name = "labelSkewness";
            this.labelSkewness.Size = new System.Drawing.Size(172, 22);
            this.labelSkewness.TabIndex = 0;
            this.labelSkewness.Text = "Skewness";
            this.labelSkewness.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSkewnessUnit
            // 
            this.labelSkewnessUnit.AutoSize = true;
            this.labelSkewnessUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSkewnessUnit.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSkewnessUnit.Location = new System.Drawing.Point(147, 44);
            this.labelSkewnessUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelSkewnessUnit.Name = "labelSkewnessUnit";
            this.labelSkewnessUnit.Size = new System.Drawing.Size(33, 22);
            this.labelSkewnessUnit.TabIndex = 2;
            this.labelSkewnessUnit.Text = "[um]";
            this.labelSkewnessUnit.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelSkewnessLtrb
            // 
            this.labelSkewnessLtrb.AutoSize = true;
            this.labelSkewnessLtrb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSkewnessLtrb.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSkewnessLtrb.Location = new System.Drawing.Point(0, 22);
            this.labelSkewnessLtrb.Margin = new System.Windows.Forms.Padding(0);
            this.labelSkewnessLtrb.Name = "labelSkewnessLtrb";
            this.labelSkewnessLtrb.Size = new System.Drawing.Size(40, 22);
            this.labelSkewnessLtrb.TabIndex = 2;
            this.labelSkewnessLtrb.Text = "LTRB";
            this.labelSkewnessLtrb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSkewnessRtlb
            // 
            this.labelSkewnessRtlb.AutoSize = true;
            this.labelSkewnessRtlb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSkewnessRtlb.Font = new System.Drawing.Font("Malgun Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSkewnessRtlb.Location = new System.Drawing.Point(0, 44);
            this.labelSkewnessRtlb.Margin = new System.Windows.Forms.Padding(0);
            this.labelSkewnessRtlb.Name = "labelSkewnessRtlb";
            this.labelSkewnessRtlb.Size = new System.Drawing.Size(40, 22);
            this.labelSkewnessRtlb.TabIndex = 2;
            this.labelSkewnessRtlb.Text = "RTLB";
            this.labelSkewnessRtlb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // skewnessLtrb
            // 
            this.skewnessLtrb.BackColor = System.Drawing.Color.White;
            this.skewnessLtrb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skewnessLtrb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skewnessLtrb.Location = new System.Drawing.Point(40, 22);
            this.skewnessLtrb.Margin = new System.Windows.Forms.Padding(0);
            this.skewnessLtrb.Name = "skewnessLtrb";
            this.skewnessLtrb.ReadOnly = true;
            this.skewnessLtrb.Size = new System.Drawing.Size(107, 22);
            this.skewnessLtrb.TabIndex = 1;
            this.skewnessLtrb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // skewnessRtlb
            // 
            this.skewnessRtlb.BackColor = System.Drawing.Color.White;
            this.skewnessRtlb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skewnessRtlb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skewnessRtlb.Location = new System.Drawing.Point(40, 44);
            this.skewnessRtlb.Margin = new System.Windows.Forms.Padding(0);
            this.skewnessRtlb.Name = "skewnessRtlb";
            this.skewnessRtlb.ReadOnly = true;
            this.skewnessRtlb.Size = new System.Drawing.Size(107, 22);
            this.skewnessRtlb.TabIndex = 1;
            this.skewnessRtlb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tableLayoutRotation
            // 
            this.tableLayoutRotation.AutoSize = true;
            this.tableLayoutRotation.ColumnCount = 2;
            this.tableLayoutRotation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutRotation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutRotation.Controls.Add(this.labelRotation, 0, 0);
            this.tableLayoutRotation.Controls.Add(this.labelRotationUnit, 1, 1);
            this.tableLayoutRotation.Controls.Add(this.rotation, 0, 1);
            this.tableLayoutRotation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutRotation.Location = new System.Drawing.Point(182, 1);
            this.tableLayoutRotation.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutRotation.Name = "tableLayoutRotation";
            this.tableLayoutRotation.RowCount = 2;
            this.tableLayoutRotation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutRotation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutRotation.Size = new System.Drawing.Size(180, 66);
            this.tableLayoutRotation.TabIndex = 3;
            // 
            // labelRotation
            // 
            this.labelRotation.AutoSize = true;
            this.tableLayoutRotation.SetColumnSpan(this.labelRotation, 2);
            this.labelRotation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRotation.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelRotation.Location = new System.Drawing.Point(4, 0);
            this.labelRotation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRotation.Name = "labelRotation";
            this.labelRotation.Size = new System.Drawing.Size(172, 22);
            this.labelRotation.TabIndex = 0;
            this.labelRotation.Text = "Rotation";
            this.labelRotation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRotationUnit
            // 
            this.labelRotationUnit.AutoSize = true;
            this.labelRotationUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRotationUnit.Font = new System.Drawing.Font("Malgun Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelRotationUnit.Location = new System.Drawing.Point(145, 22);
            this.labelRotationUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelRotationUnit.Name = "labelRotationUnit";
            this.labelRotationUnit.Size = new System.Drawing.Size(35, 44);
            this.labelRotationUnit.TabIndex = 2;
            this.labelRotationUnit.Text = "[deg]";
            this.labelRotationUnit.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // rotation
            // 
            this.rotation.BackColor = System.Drawing.Color.White;
            this.rotation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rotation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rotation.Location = new System.Drawing.Point(0, 22);
            this.rotation.Margin = new System.Windows.Forms.Padding(0);
            this.rotation.Multiline = true;
            this.rotation.Name = "rotation";
            this.rotation.ReadOnly = true;
            this.rotation.Size = new System.Drawing.Size(145, 44);
            this.rotation.TabIndex = 1;
            this.rotation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // layoutViewControl
            // 
            this.layoutViewControl.AutoSize = true;
            this.layoutViewControl.ColumnCount = 4;
            this.layoutViewControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.12245F));
            this.layoutViewControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutViewControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.layoutViewControl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.87755F));
            this.layoutViewControl.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.layoutViewControl.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.layoutViewControl.Controls.Add(this.tableLayoutPanel4, 3, 0);
            this.layoutViewControl.Controls.Add(this.tableLayoutPanel5, 2, 0);
            this.layoutViewControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutViewControl.Font = new System.Drawing.Font("Malgun Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.layoutViewControl.Location = new System.Drawing.Point(0, 35);
            this.layoutViewControl.Margin = new System.Windows.Forms.Padding(0);
            this.layoutViewControl.Name = "layoutViewControl";
            this.layoutViewControl.RowCount = 1;
            this.layoutViewControl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutViewControl.Size = new System.Drawing.Size(725, 27);
            this.layoutViewControl.TabIndex = 73;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.showGuideCircle, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.guideCircleSize, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelGuideLineLengthUm, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(268, 27);
            this.tableLayoutPanel2.TabIndex = 74;
            // 
            // showGuideCircle
            // 
            this.showGuideCircle.AutoSize = true;
            this.showGuideCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showGuideCircle.Location = new System.Drawing.Point(0, 0);
            this.showGuideCircle.Margin = new System.Windows.Forms.Padding(0);
            this.showGuideCircle.Name = "showGuideCircle";
            this.showGuideCircle.Size = new System.Drawing.Size(112, 27);
            this.showGuideCircle.TabIndex = 1;
            this.showGuideCircle.Text = "Guide Circle";
            this.showGuideCircle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showGuideCircle.UseVisualStyleBackColor = true;
            this.showGuideCircle.CheckedChanged += new System.EventHandler(this.showGuideCircle_CheckedChanged);
            // 
            // guideCircleSize
            // 
            this.guideCircleSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guideCircleSize.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.guideCircleSize.Location = new System.Drawing.Point(112, 0);
            this.guideCircleSize.Margin = new System.Windows.Forms.Padding(0);
            this.guideCircleSize.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.guideCircleSize.Name = "guideCircleSize";
            this.guideCircleSize.Size = new System.Drawing.Size(65, 27);
            this.guideCircleSize.TabIndex = 2;
            this.guideCircleSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.guideCircleSize.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.guideCircleSize.ValueChanged += new System.EventHandler(this.guideCircleSize_ValueChanged);
            // 
            // labelGuideLineLengthUm
            // 
            this.labelGuideLineLengthUm.AutoSize = true;
            this.labelGuideLineLengthUm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelGuideLineLengthUm.Location = new System.Drawing.Point(177, 0);
            this.labelGuideLineLengthUm.Margin = new System.Windows.Forms.Padding(0);
            this.labelGuideLineLengthUm.Name = "labelGuideLineLengthUm";
            this.labelGuideLineLengthUm.Size = new System.Drawing.Size(41, 27);
            this.labelGuideLineLengthUm.TabIndex = 3;
            this.labelGuideLineLengthUm.Text = "[um]";
            this.labelGuideLineLengthUm.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AutoSize = true;
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.drawLines, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(278, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(112, 27);
            this.tableLayoutPanel3.TabIndex = 74;
            // 
            // drawLines
            // 
            this.drawLines.AutoSize = true;
            this.drawLines.Checked = true;
            this.drawLines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.drawLines.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawLines.Location = new System.Drawing.Point(5, 0);
            this.drawLines.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.drawLines.Name = "drawLines";
            this.drawLines.Size = new System.Drawing.Size(102, 27);
            this.drawLines.TabIndex = 1;
            this.drawLines.Text = "Draw Lines";
            this.drawLines.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.drawLines.UseVisualStyleBackColor = true;
            this.drawLines.CheckedChanged += new System.EventHandler(this.drawLines_CheckedChanged);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.AutoScroll = true;
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.labelScaleUnit, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.offsetScale, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.useScale, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(511, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(209, 27);
            this.tableLayoutPanel4.TabIndex = 74;
            // 
            // labelScaleUnit
            // 
            this.labelScaleUnit.AutoSize = true;
            this.labelScaleUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelScaleUnit.Location = new System.Drawing.Point(183, 0);
            this.labelScaleUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelScaleUnit.Name = "labelScaleUnit";
            this.labelScaleUnit.Size = new System.Drawing.Size(26, 27);
            this.labelScaleUnit.TabIndex = 3;
            this.labelScaleUnit.Text = "[x]";
            this.labelScaleUnit.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // offsetScale
            // 
            this.offsetScale.DecimalPlaces = 1;
            this.offsetScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.offsetScale.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.offsetScale.Location = new System.Drawing.Point(118, 0);
            this.offsetScale.Margin = new System.Windows.Forms.Padding(0);
            this.offsetScale.Name = "offsetScale";
            this.offsetScale.Size = new System.Drawing.Size(65, 27);
            this.offsetScale.TabIndex = 2;
            this.offsetScale.ValueChanged += new System.EventHandler(this.offsetScale_ValueChanged);
            // 
            // useScale
            // 
            this.useScale.AutoSize = true;
            this.useScale.Dock = System.Windows.Forms.DockStyle.Fill;
            this.useScale.Location = new System.Drawing.Point(55, 0);
            this.useScale.Margin = new System.Windows.Forms.Padding(0);
            this.useScale.Name = "useScale";
            this.useScale.Size = new System.Drawing.Size(63, 27);
            this.useScale.TabIndex = 1;
            this.useScale.Text = "Scale";
            this.useScale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.useScale.UseVisualStyleBackColor = true;
            this.useScale.CheckedChanged += new System.EventHandler(this.useScale_CheckedChanged);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.AutoSize = true;
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.showLabels, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(390, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(116, 27);
            this.tableLayoutPanel5.TabIndex = 74;
            // 
            // showLabels
            // 
            this.showLabels.AutoSize = true;
            this.showLabels.Checked = true;
            this.showLabels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showLabels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showLabels.Location = new System.Drawing.Point(5, 0);
            this.showLabels.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.showLabels.Name = "showLabels";
            this.showLabels.Size = new System.Drawing.Size(106, 27);
            this.showLabels.TabIndex = 1;
            this.showLabels.Text = "Show Label";
            this.showLabels.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.showLabels.UseVisualStyleBackColor = true;
            this.showLabels.CheckedChanged += new System.EventHandler(this.showLabels_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0F));
            this.tableLayoutPanel1.Controls.Add(this.chart, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelController, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 62);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(725, 380);
            this.tableLayoutPanel1.TabIndex = 71;
            // 
            // chart
            // 
            chartArea3.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea3);
            this.chart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart.Location = new System.Drawing.Point(0, 0);
            this.chart.Margin = new System.Windows.Forms.Padding(0);
            this.chart.Name = "chart";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series5.Name = "SeriesBase";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series6.Name = "SeriesData0";
            this.chart.Series.Add(series5);
            this.chart.Series.Add(series6);
            this.chart.Size = new System.Drawing.Size(725, 380);
            this.chart.TabIndex = 71;
            this.chart.Text = "chart1";
            this.chart.Paint += new System.Windows.Forms.PaintEventHandler(this.chart_Paint);
            // 
            // panelController
            // 
            this.panelController.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelController.Location = new System.Drawing.Point(725, 0);
            this.panelController.Margin = new System.Windows.Forms.Padding(0);
            this.panelController.Name = "panelController";
            this.panelController.Size = new System.Drawing.Size(1, 380);
            this.panelController.TabIndex = 72;
            // 
            // TransformChartControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutTransformMap);
            this.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "TransformChartControl";
            this.Size = new System.Drawing.Size(725, 510);
            this.Load += new System.EventHandler(this.TransformChartControl_Load);
            this.layoutTransformMap.ResumeLayout(false);
            this.layoutTransformMap.PerformLayout();
            this.layoutTransformValue.ResumeLayout(false);
            this.layoutTransformValue.PerformLayout();
            this.tableLayoutSize.ResumeLayout(false);
            this.tableLayoutSize.PerformLayout();
            this.tableLayoutTranslation.ResumeLayout(false);
            this.tableLayoutTranslation.PerformLayout();
            this.tableLayoutSkewness.ResumeLayout(false);
            this.tableLayoutSkewness.PerformLayout();
            this.tableLayoutRotation.ResumeLayout(false);
            this.tableLayoutRotation.PerformLayout();
            this.layoutViewControl.ResumeLayout(false);
            this.layoutViewControl.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.guideCircleSize)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.offsetScale)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutTransformMap;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TableLayoutPanel layoutTransformValue;
        private System.Windows.Forms.TableLayoutPanel tableLayoutSize;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelSizeUnit;
        private System.Windows.Forms.Label labelSizeW;
        private System.Windows.Forms.Label labelSizeH;
        private System.Windows.Forms.TextBox sizeW;
        private System.Windows.Forms.TextBox sizeH;
        private System.Windows.Forms.TableLayoutPanel tableLayoutTranslation;
        private System.Windows.Forms.Label labelTranslationUnit;
        private System.Windows.Forms.Label labelTranslationX;
        private System.Windows.Forms.Label labelTranslationY;
        private System.Windows.Forms.TextBox translationX;
        private System.Windows.Forms.TextBox translationY;
        private System.Windows.Forms.Label labelTranslation;
        private System.Windows.Forms.TableLayoutPanel tableLayoutSkewness;
        private System.Windows.Forms.Label labelSkewness;
        private System.Windows.Forms.Label labelSkewnessUnit;
        private System.Windows.Forms.Label labelSkewnessLtrb;
        private System.Windows.Forms.Label labelSkewnessRtlb;
        private System.Windows.Forms.TextBox skewnessLtrb;
        private System.Windows.Forms.TextBox skewnessRtlb;
        private System.Windows.Forms.TableLayoutPanel tableLayoutRotation;
        private System.Windows.Forms.Label labelRotation;
        private System.Windows.Forms.Label labelRotationUnit;
        private System.Windows.Forms.TextBox rotation;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelController;
        private System.Windows.Forms.TableLayoutPanel layoutViewControl;
        private System.Windows.Forms.CheckBox showGuideCircle;
        private System.Windows.Forms.NumericUpDown guideCircleSize;
        private System.Windows.Forms.Label labelGuideLineLengthUm;
        private System.Windows.Forms.Label labelScaleUnit;
        private System.Windows.Forms.NumericUpDown offsetScale;
        private System.Windows.Forms.CheckBox drawLines;
        private System.Windows.Forms.CheckBox useScale;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.CheckBox showLabels;
    }
}
