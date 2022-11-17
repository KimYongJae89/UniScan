namespace UniScanS.Common.Settings.Monitor.UI
{
    partial class FovSettingForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelFov = new System.Windows.Forms.Label();
            this.labelImage = new System.Windows.Forms.Label();
            this.layoutInspector = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.matchingY = new System.Windows.Forms.NumericUpDown();
            this.score = new System.Windows.Forms.NumericUpDown();
            this.labelY = new System.Windows.Forms.Label();
            this.labelScore = new System.Windows.Forms.Label();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.matchingWidth = new System.Windows.Forms.NumericUpDown();
            this.labelHeight = new System.Windows.Forms.Label();
            this.labelMatchingWidth = new System.Windows.Forms.Label();
            this.height = new System.Windows.Forms.NumericUpDown();
            this.labelWIdth = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.NumericUpDown();
            this.buttonOverlap = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelMatching = new System.Windows.Forms.Label();
            this.labelMatchingHeight = new System.Windows.Forms.Label();
            this.matchingHeight = new System.Windows.Forms.NumericUpDown();
            this.buttonSaveImage = new System.Windows.Forms.Button();
            this.panelImage = new System.Windows.Forms.Panel();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.layoutMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.matchingY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.score)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchingWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchingHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutMain
            // 
            this.layoutMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutMain.ColumnCount = 2;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 166F));
            this.layoutMain.Controls.Add(this.labelFov, 1, 1);
            this.layoutMain.Controls.Add(this.labelImage, 0, 1);
            this.layoutMain.Controls.Add(this.layoutInspector, 0, 0);
            this.layoutMain.Controls.Add(this.tableLayoutPanel1, 1, 2);
            this.layoutMain.Controls.Add(this.panelImage, 0, 2);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(1, 30);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 3;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutMain.Size = new System.Drawing.Size(1211, 921);
            this.layoutMain.TabIndex = 0;
            // 
            // labelFov
            // 
            this.labelFov.AutoSize = true;
            this.labelFov.BackColor = System.Drawing.Color.Navy;
            this.labelFov.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFov.ForeColor = System.Drawing.Color.White;
            this.labelFov.Location = new System.Drawing.Point(1044, 445);
            this.labelFov.Margin = new System.Windows.Forms.Padding(0);
            this.labelFov.Name = "labelFov";
            this.labelFov.Size = new System.Drawing.Size(166, 30);
            this.labelFov.TabIndex = 17;
            this.labelFov.Text = "FOV";
            this.labelFov.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelImage
            // 
            this.labelImage.AutoSize = true;
            this.labelImage.BackColor = System.Drawing.Color.Navy;
            this.labelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelImage.ForeColor = System.Drawing.Color.White;
            this.labelImage.Location = new System.Drawing.Point(1, 445);
            this.labelImage.Margin = new System.Windows.Forms.Padding(0);
            this.labelImage.Name = "labelImage";
            this.labelImage.Size = new System.Drawing.Size(1042, 30);
            this.labelImage.TabIndex = 16;
            this.labelImage.Text = "Image";
            this.labelImage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutInspector
            // 
            this.layoutInspector.ColumnCount = 1;
            this.layoutMain.SetColumnSpan(this.layoutInspector, 2);
            this.layoutInspector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutInspector.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutInspector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutInspector.Location = new System.Drawing.Point(1, 1);
            this.layoutInspector.Margin = new System.Windows.Forms.Padding(0);
            this.layoutInspector.Name = "layoutInspector";
            this.layoutInspector.RowCount = 1;
            this.layoutInspector.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutInspector.Size = new System.Drawing.Size(1209, 443);
            this.layoutInspector.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.matchingY, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.score, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelY, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.labelScore, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonGenerate, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.matchingWidth, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.labelHeight, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelMatchingWidth, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.height, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelWIdth, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.width, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonOverlap, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.buttonSave, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.labelMatching, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelMatchingHeight, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.matchingHeight, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.buttonSaveImage, 0, 11);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1044, 476);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 12;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(166, 444);
            this.tableLayoutPanel1.TabIndex = 15;
            // 
            // matchingY
            // 
            this.matchingY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.matchingY.Location = new System.Drawing.Point(83, 126);
            this.matchingY.Margin = new System.Windows.Forms.Padding(0);
            this.matchingY.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.matchingY.Name = "matchingY";
            this.matchingY.Size = new System.Drawing.Size(83, 32);
            this.matchingY.TabIndex = 27;
            this.matchingY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.matchingY.ValueChanged += new System.EventHandler(this.matchingY_ValueChanged);
            // 
            // score
            // 
            this.score.Dock = System.Windows.Forms.DockStyle.Fill;
            this.score.Location = new System.Drawing.Point(83, 94);
            this.score.Margin = new System.Windows.Forms.Padding(0);
            this.score.Name = "score";
            this.score.Size = new System.Drawing.Size(83, 32);
            this.score.TabIndex = 26;
            this.score.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.score.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelY.Location = new System.Drawing.Point(0, 126);
            this.labelY.Margin = new System.Windows.Forms.Padding(0);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(83, 32);
            this.labelY.TabIndex = 22;
            this.labelY.Text = "Y";
            this.labelY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelScore
            // 
            this.labelScore.AutoSize = true;
            this.labelScore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelScore.Location = new System.Drawing.Point(0, 94);
            this.labelScore.Margin = new System.Windows.Forms.Padding(0);
            this.labelScore.Name = "labelScore";
            this.labelScore.Size = new System.Drawing.Size(83, 32);
            this.labelScore.TabIndex = 22;
            this.labelScore.Text = "Score";
            this.labelScore.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.buttonGenerate, 2);
            this.buttonGenerate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonGenerate.Location = new System.Drawing.Point(0, 284);
            this.buttonGenerate.Margin = new System.Windows.Forms.Padding(0);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(166, 40);
            this.buttonGenerate.TabIndex = 17;
            this.buttonGenerate.Text = "Generate";
            this.buttonGenerate.UseVisualStyleBackColor = false;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // matchingWidth
            // 
            this.matchingWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.matchingWidth.Location = new System.Drawing.Point(83, 158);
            this.matchingWidth.Margin = new System.Windows.Forms.Padding(0);
            this.matchingWidth.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.matchingWidth.Name = "matchingWidth";
            this.matchingWidth.Size = new System.Drawing.Size(83, 32);
            this.matchingWidth.TabIndex = 25;
            this.matchingWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.matchingWidth.ValueChanged += new System.EventHandler(this.matchingWidth_ValueChanged);
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeight.Location = new System.Drawing.Point(0, 32);
            this.labelHeight.Margin = new System.Windows.Forms.Padding(0);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(83, 32);
            this.labelHeight.TabIndex = 19;
            this.labelHeight.Text = "Height";
            this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMatchingWidth
            // 
            this.labelMatchingWidth.AutoSize = true;
            this.labelMatchingWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMatchingWidth.Location = new System.Drawing.Point(0, 158);
            this.labelMatchingWidth.Margin = new System.Windows.Forms.Padding(0);
            this.labelMatchingWidth.Name = "labelMatchingWidth";
            this.labelMatchingWidth.Size = new System.Drawing.Size(83, 32);
            this.labelMatchingWidth.TabIndex = 24;
            this.labelMatchingWidth.Text = "Width";
            this.labelMatchingWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // height
            // 
            this.height.Dock = System.Windows.Forms.DockStyle.Fill;
            this.height.Enabled = false;
            this.height.Location = new System.Drawing.Point(83, 32);
            this.height.Margin = new System.Windows.Forms.Padding(0);
            this.height.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.height.Name = "height";
            this.height.Size = new System.Drawing.Size(83, 32);
            this.height.TabIndex = 20;
            this.height.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelWIdth
            // 
            this.labelWIdth.AutoSize = true;
            this.labelWIdth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWIdth.Location = new System.Drawing.Point(0, 0);
            this.labelWIdth.Margin = new System.Windows.Forms.Padding(0);
            this.labelWIdth.Name = "labelWIdth";
            this.labelWIdth.Size = new System.Drawing.Size(83, 32);
            this.labelWIdth.TabIndex = 17;
            this.labelWIdth.Text = "Width";
            this.labelWIdth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // width
            // 
            this.width.Dock = System.Windows.Forms.DockStyle.Fill;
            this.width.Enabled = false;
            this.width.Location = new System.Drawing.Point(83, 0);
            this.width.Margin = new System.Windows.Forms.Padding(0);
            this.width.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.width.Name = "width";
            this.width.Size = new System.Drawing.Size(83, 32);
            this.width.TabIndex = 18;
            this.width.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // buttonOverlap
            // 
            this.buttonOverlap.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.buttonOverlap, 2);
            this.buttonOverlap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOverlap.Location = new System.Drawing.Point(0, 324);
            this.buttonOverlap.Margin = new System.Windows.Forms.Padding(0);
            this.buttonOverlap.Name = "buttonOverlap";
            this.buttonOverlap.Size = new System.Drawing.Size(166, 40);
            this.buttonOverlap.TabIndex = 16;
            this.buttonOverlap.Text = "Overlap";
            this.buttonOverlap.UseVisualStyleBackColor = false;
            this.buttonOverlap.Click += new System.EventHandler(this.buttonOverlap_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.buttonSave, 2);
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSave.Location = new System.Drawing.Point(0, 364);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(166, 40);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // labelMatching
            // 
            this.labelMatching.AutoSize = true;
            this.labelMatching.BackColor = System.Drawing.Color.Navy;
            this.tableLayoutPanel1.SetColumnSpan(this.labelMatching, 2);
            this.labelMatching.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMatching.ForeColor = System.Drawing.Color.White;
            this.labelMatching.Location = new System.Drawing.Point(0, 64);
            this.labelMatching.Margin = new System.Windows.Forms.Padding(0);
            this.labelMatching.Name = "labelMatching";
            this.labelMatching.Size = new System.Drawing.Size(166, 30);
            this.labelMatching.TabIndex = 23;
            this.labelMatching.Text = "Matching";
            this.labelMatching.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMatchingHeight
            // 
            this.labelMatchingHeight.AutoSize = true;
            this.labelMatchingHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMatchingHeight.Location = new System.Drawing.Point(0, 190);
            this.labelMatchingHeight.Margin = new System.Windows.Forms.Padding(0);
            this.labelMatchingHeight.Name = "labelMatchingHeight";
            this.labelMatchingHeight.Size = new System.Drawing.Size(83, 32);
            this.labelMatchingHeight.TabIndex = 21;
            this.labelMatchingHeight.Text = "Height";
            this.labelMatchingHeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // matchingHeight
            // 
            this.matchingHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.matchingHeight.Location = new System.Drawing.Point(83, 190);
            this.matchingHeight.Margin = new System.Windows.Forms.Padding(0);
            this.matchingHeight.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.matchingHeight.Name = "matchingHeight";
            this.matchingHeight.Size = new System.Drawing.Size(83, 32);
            this.matchingHeight.TabIndex = 22;
            this.matchingHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.matchingHeight.ValueChanged += new System.EventHandler(this.matchingHeight_ValueChanged);
            // 
            // buttonSaveImage
            // 
            this.buttonSaveImage.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.buttonSaveImage, 2);
            this.buttonSaveImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSaveImage.Location = new System.Drawing.Point(0, 404);
            this.buttonSaveImage.Margin = new System.Windows.Forms.Padding(0);
            this.buttonSaveImage.Name = "buttonSaveImage";
            this.buttonSaveImage.Size = new System.Drawing.Size(166, 40);
            this.buttonSaveImage.TabIndex = 2;
            this.buttonSaveImage.Text = "Save Image";
            this.buttonSaveImage.UseVisualStyleBackColor = false;
            this.buttonSaveImage.Click += new System.EventHandler(this.buttonSaveImage_Click);
            // 
            // panelImage
            // 
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(1, 476);
            this.panelImage.Margin = new System.Windows.Forms.Padding(0);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(1042, 444);
            this.panelImage.TabIndex = 18;
            // 
            // ultraFormManager
            // 
            this.ultraFormManager.Form = this;
            appearance1.BackColor = System.Drawing.Color.Black;
            appearance1.TextHAlignAsString = "Left";
            this.ultraFormManager.FormStyleSettings.CaptionAreaAppearance = appearance1;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.HotTrackAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(168)))), ((int)(((byte)(12)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.PressedAppearance = appearance4;
            this.ultraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2013;
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Top
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.Black;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1213, 30);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.Black;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 951);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1213, 1);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Left
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.Black;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 30);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 921);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.Black;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1212, 30);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 921);
            // 
            // FovSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 952);
            this.Controls.Add(this.layoutMain);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.Name = "FovSettingForm";
            this.Text = "FOV Setting";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.matchingY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.score)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchingWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.width)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.matchingHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.TableLayoutPanel layoutInspector;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonOverlap;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.NumericUpDown height;
        private System.Windows.Forms.Label labelWIdth;
        private System.Windows.Forms.NumericUpDown width;
        private System.Windows.Forms.Label labelFov;
        private System.Windows.Forms.Label labelImage;
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.NumericUpDown matchingWidth;
        private System.Windows.Forms.Label labelMatchingWidth;
        private System.Windows.Forms.Label labelMatching;
        private System.Windows.Forms.Label labelMatchingHeight;
        private System.Windows.Forms.NumericUpDown matchingHeight;
        private System.Windows.Forms.NumericUpDown matchingY;
        private System.Windows.Forms.NumericUpDown score;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelScore;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.Button buttonSaveImage;
    }
}