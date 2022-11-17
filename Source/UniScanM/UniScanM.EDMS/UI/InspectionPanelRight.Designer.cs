namespace UniScanM.EDMS.UI
{
    partial class InspectionPanelRight
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
            this.panelImage = new System.Windows.Forms.Panel();
            this.panelParam = new System.Windows.Forms.Panel();
            this.cb_Preset = new System.Windows.Forms.ComboBox();
            this.checkOnTune = new System.Windows.Forms.CheckBox();
            this.groupThresHold = new System.Windows.Forms.GroupBox();
            this.numericUpDownPrintingThreshold = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDownCoatingThreshold = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownFilmThreshold = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.groupLightParameter = new System.Windows.Forms.GroupBox();
            this.frontLightValue = new System.Windows.Forms.NumericUpDown();
            this.labelFront = new System.Windows.Forms.Label();
            this.backLightValue = new System.Windows.Forms.NumericUpDown();
            this.labelBack = new System.Windows.Forms.Label();
            this.label_Preset = new System.Windows.Forms.Label();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelState = new System.Windows.Forms.Label();
            this.buttonStateReset = new Infragistics.Win.Misc.UltraButton();
            this.tableLayoutPanelCurrentValue = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.labelFilmValue = new System.Windows.Forms.Label();
            this.labelFilm = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelPrintingValue = new System.Windows.Forms.Label();
            this.labelCoatingValue = new System.Windows.Forms.Label();
            this.labelPrintingPos = new System.Windows.Forms.Label();
            this.progressBarZeroing = new System.Windows.Forms.ProgressBar();
            this.timer_GUIRevison = new System.Windows.Forms.Timer(this.components);
            this.panelParam.SuspendLayout();
            this.groupThresHold.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrintingThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCoatingThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFilmThreshold)).BeginInit();
            this.groupLightParameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frontLightValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backLightValue)).BeginInit();
            this.layoutMain.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanelCurrentValue.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelImage
            // 
            this.panelImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImage.Location = new System.Drawing.Point(0, 80);
            this.panelImage.Margin = new System.Windows.Forms.Padding(0);
            this.panelImage.Name = "panelImage";
            this.panelImage.Size = new System.Drawing.Size(672, 240);
            this.panelImage.TabIndex = 2;
            // 
            // panelParam
            // 
            this.panelParam.Controls.Add(this.cb_Preset);
            this.panelParam.Controls.Add(this.checkOnTune);
            this.panelParam.Controls.Add(this.groupThresHold);
            this.panelParam.Controls.Add(this.buttonSave);
            this.panelParam.Controls.Add(this.groupLightParameter);
            this.panelParam.Controls.Add(this.label_Preset);
            this.panelParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelParam.Location = new System.Drawing.Point(5, 386);
            this.panelParam.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.panelParam.Name = "panelParam";
            this.panelParam.Size = new System.Drawing.Size(662, 411);
            this.panelParam.TabIndex = 1;
            // 
            // cb_Preset
            // 
            this.cb_Preset.Font = new System.Drawing.Font("맑은 고딕", 12.75F, System.Drawing.FontStyle.Bold);
            this.cb_Preset.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.cb_Preset.FormattingEnabled = true;
            this.cb_Preset.Location = new System.Drawing.Point(126, 7);
            this.cb_Preset.Name = "cb_Preset";
            this.cb_Preset.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cb_Preset.Size = new System.Drawing.Size(88, 31);
            this.cb_Preset.TabIndex = 13;
            this.cb_Preset.SelectedIndexChanged += new System.EventHandler(this.cb_Preset_SelectedIndexChanged);
            // 
            // checkOnTune
            // 
            this.checkOnTune.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkOnTune.BackColor = System.Drawing.Color.Gray;
            this.checkOnTune.Checked = true;
            this.checkOnTune.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkOnTune.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkOnTune.FlatAppearance.CheckedBackColor = System.Drawing.Color.SeaGreen;
            this.checkOnTune.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkOnTune.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkOnTune.Location = new System.Drawing.Point(0, 361);
            this.checkOnTune.Name = "checkOnTune";
            this.checkOnTune.Size = new System.Drawing.Size(662, 50);
            this.checkOnTune.TabIndex = 12;
            this.checkOnTune.Text = "Comm Open/Close";
            this.checkOnTune.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkOnTune.UseVisualStyleBackColor = false;
            this.checkOnTune.CheckedChanged += new System.EventHandler(this.checkOnTune_CheckedChanged);
            // 
            // groupThresHold
            // 
            this.groupThresHold.Controls.Add(this.numericUpDownPrintingThreshold);
            this.groupThresHold.Controls.Add(this.label9);
            this.groupThresHold.Controls.Add(this.numericUpDownCoatingThreshold);
            this.groupThresHold.Controls.Add(this.numericUpDownFilmThreshold);
            this.groupThresHold.Controls.Add(this.label7);
            this.groupThresHold.Controls.Add(this.label8);
            this.groupThresHold.Enabled = false;
            this.groupThresHold.Font = new System.Drawing.Font("맑은 고딕", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupThresHold.Location = new System.Drawing.Point(5, 45);
            this.groupThresHold.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupThresHold.Name = "groupThresHold";
            this.groupThresHold.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupThresHold.Size = new System.Drawing.Size(227, 151);
            this.groupThresHold.TabIndex = 11;
            this.groupThresHold.TabStop = false;
            this.groupThresHold.Text = "Threshold";
            // 
            // numericUpDownPrintingThreshold
            // 
            this.numericUpDownPrintingThreshold.DecimalPlaces = 1;
            this.numericUpDownPrintingThreshold.Location = new System.Drawing.Point(121, 111);
            this.numericUpDownPrintingThreshold.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.numericUpDownPrintingThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownPrintingThreshold.Name = "numericUpDownPrintingThreshold";
            this.numericUpDownPrintingThreshold.Size = new System.Drawing.Size(88, 30);
            this.numericUpDownPrintingThreshold.TabIndex = 10;
            this.numericUpDownPrintingThreshold.ValueChanged += new System.EventHandler(this.numericUpDownPrintingThreshold_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(35, 113);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 23);
            this.label9.TabIndex = 9;
            this.label9.Text = "Printing";
            // 
            // numericUpDownCoatingThreshold
            // 
            this.numericUpDownCoatingThreshold.DecimalPlaces = 1;
            this.numericUpDownCoatingThreshold.Location = new System.Drawing.Point(121, 72);
            this.numericUpDownCoatingThreshold.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.numericUpDownCoatingThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownCoatingThreshold.Name = "numericUpDownCoatingThreshold";
            this.numericUpDownCoatingThreshold.Size = new System.Drawing.Size(88, 30);
            this.numericUpDownCoatingThreshold.TabIndex = 8;
            this.numericUpDownCoatingThreshold.ValueChanged += new System.EventHandler(this.numericUpDownCoatingThreshold_ValueChanged);
            // 
            // numericUpDownFilmThreshold
            // 
            this.numericUpDownFilmThreshold.DecimalPlaces = 1;
            this.numericUpDownFilmThreshold.Location = new System.Drawing.Point(121, 37);
            this.numericUpDownFilmThreshold.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.numericUpDownFilmThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownFilmThreshold.Name = "numericUpDownFilmThreshold";
            this.numericUpDownFilmThreshold.Size = new System.Drawing.Size(88, 30);
            this.numericUpDownFilmThreshold.TabIndex = 6;
            this.numericUpDownFilmThreshold.ValueChanged += new System.EventHandler(this.numericUpDownFilmThreshold_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(35, 74);
            this.label7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(74, 23);
            this.label7.TabIndex = 7;
            this.label7.Text = "Coating";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(35, 39);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 23);
            this.label8.TabIndex = 5;
            this.label8.Text = "Film";
            // 
            // buttonSave
            // 
            this.buttonSave.Enabled = false;
            this.buttonSave.Font = new System.Drawing.Font("맑은 고딕", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.buttonSave.Location = new System.Drawing.Point(240, 7);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(146, 29);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Parameter Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Visible = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // groupLightParameter
            // 
            this.groupLightParameter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupLightParameter.Controls.Add(this.frontLightValue);
            this.groupLightParameter.Controls.Add(this.labelFront);
            this.groupLightParameter.Controls.Add(this.backLightValue);
            this.groupLightParameter.Controls.Add(this.labelBack);
            this.groupLightParameter.Enabled = false;
            this.groupLightParameter.Font = new System.Drawing.Font("맑은 고딕", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupLightParameter.Location = new System.Drawing.Point(429, 45);
            this.groupLightParameter.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupLightParameter.Name = "groupLightParameter";
            this.groupLightParameter.Padding = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.groupLightParameter.Size = new System.Drawing.Size(228, 151);
            this.groupLightParameter.TabIndex = 4;
            this.groupLightParameter.TabStop = false;
            this.groupLightParameter.Text = "Light Parameter";
            // 
            // frontLightValue
            // 
            this.frontLightValue.Location = new System.Drawing.Point(117, 74);
            this.frontLightValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.frontLightValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.frontLightValue.Name = "frontLightValue";
            this.frontLightValue.Size = new System.Drawing.Size(88, 30);
            this.frontLightValue.TabIndex = 3;
            this.frontLightValue.ValueChanged += new System.EventHandler(this.lightValue_ValueChanged);
            // 
            // labelFront
            // 
            this.labelFront.AutoSize = true;
            this.labelFront.Location = new System.Drawing.Point(48, 78);
            this.labelFront.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelFront.Name = "labelFront";
            this.labelFront.Size = new System.Drawing.Size(54, 23);
            this.labelFront.TabIndex = 2;
            this.labelFront.Text = "Front";
            // 
            // backLightValue
            // 
            this.backLightValue.Location = new System.Drawing.Point(117, 35);
            this.backLightValue.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.backLightValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.backLightValue.Name = "backLightValue";
            this.backLightValue.Size = new System.Drawing.Size(88, 30);
            this.backLightValue.TabIndex = 1;
            this.backLightValue.ValueChanged += new System.EventHandler(this.lightValue_ValueChanged);
            // 
            // labelBack
            // 
            this.labelBack.AutoSize = true;
            this.labelBack.Location = new System.Drawing.Point(48, 39);
            this.labelBack.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelBack.Name = "labelBack";
            this.labelBack.Size = new System.Drawing.Size(47, 23);
            this.labelBack.TabIndex = 0;
            this.labelBack.Text = "Back";
            // 
            // label_Preset
            // 
            this.label_Preset.AutoSize = true;
            this.label_Preset.Location = new System.Drawing.Point(40, 10);
            this.label_Preset.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label_Preset.Name = "label_Preset";
            this.label_Preset.Size = new System.Drawing.Size(65, 25);
            this.label_Preset.TabIndex = 5;
            this.label_Preset.Text = "Preset";
            // 
            // layoutMain
            // 
            this.layoutMain.ColumnCount = 1;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.Controls.Add(this.panel1, 0, 0);
            this.layoutMain.Controls.Add(this.panelParam, 0, 4);
            this.layoutMain.Controls.Add(this.panelImage, 0, 2);
            this.layoutMain.Controls.Add(this.tableLayoutPanelCurrentValue, 0, 3);
            this.layoutMain.Controls.Add(this.progressBarZeroing, 0, 1);
            this.layoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 5;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 240F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.layoutMain.Size = new System.Drawing.Size(672, 803);
            this.layoutMain.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelState);
            this.panel1.Controls.Add(this.buttonStateReset);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(672, 50);
            this.panel1.TabIndex = 49;
            // 
            // labelState
            // 
            this.labelState.BackColor = System.Drawing.Color.Black;
            this.labelState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelState.Font = new System.Drawing.Font("맑은 고딕", 27F, System.Drawing.FontStyle.Bold);
            this.labelState.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.labelState.Location = new System.Drawing.Point(0, 0);
            this.labelState.Margin = new System.Windows.Forms.Padding(0);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(523, 50);
            this.labelState.TabIndex = 44;
            this.labelState.Text = "State";
            this.labelState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonStateReset
            // 
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.buttonStateReset.Appearance = appearance1;
            this.buttonStateReset.ButtonStyle = Infragistics.Win.UIElementButtonStyle.Office2003ToolbarButton;
            this.buttonStateReset.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStateReset.ImageSize = new System.Drawing.Size(20, 20);
            this.buttonStateReset.Location = new System.Drawing.Point(523, 0);
            this.buttonStateReset.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStateReset.Name = "buttonStateReset";
            this.buttonStateReset.Size = new System.Drawing.Size(149, 50);
            this.buttonStateReset.TabIndex = 46;
            this.buttonStateReset.Text = "Zero Setting";
            this.buttonStateReset.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonStateReset.Click += new System.EventHandler(this.buttonStateReset_Click);
            // 
            // tableLayoutPanelCurrentValue
            // 
            this.tableLayoutPanelCurrentValue.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanelCurrentValue.ColumnCount = 4;
            this.tableLayoutPanelCurrentValue.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCurrentValue.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCurrentValue.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCurrentValue.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanelCurrentValue.Controls.Add(this.label6, 3, 0);
            this.tableLayoutPanelCurrentValue.Controls.Add(this.labelFilmValue, 0, 1);
            this.tableLayoutPanelCurrentValue.Controls.Add(this.labelFilm, 0, 0);
            this.tableLayoutPanelCurrentValue.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanelCurrentValue.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanelCurrentValue.Controls.Add(this.labelPrintingValue, 2, 1);
            this.tableLayoutPanelCurrentValue.Controls.Add(this.labelCoatingValue, 1, 1);
            this.tableLayoutPanelCurrentValue.Controls.Add(this.labelPrintingPos, 3, 1);
            this.tableLayoutPanelCurrentValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelCurrentValue.Location = new System.Drawing.Point(0, 320);
            this.tableLayoutPanelCurrentValue.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelCurrentValue.Name = "tableLayoutPanelCurrentValue";
            this.tableLayoutPanelCurrentValue.RowCount = 2;
            this.tableLayoutPanelCurrentValue.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCurrentValue.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelCurrentValue.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelCurrentValue.Size = new System.Drawing.Size(672, 60);
            this.tableLayoutPanelCurrentValue.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.AliceBlue;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(502, 1);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(169, 28);
            this.label6.TabIndex = 12;
            this.label6.Text = "Printing Pos";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFilmValue
            // 
            this.labelFilmValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFilmValue.Font = new System.Drawing.Font("맑은 고딕", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelFilmValue.Location = new System.Drawing.Point(6, 30);
            this.labelFilmValue.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelFilmValue.Name = "labelFilmValue";
            this.labelFilmValue.Size = new System.Drawing.Size(156, 29);
            this.labelFilmValue.TabIndex = 10;
            this.labelFilmValue.Text = "XXX.XXX";
            this.labelFilmValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelFilm
            // 
            this.labelFilm.BackColor = System.Drawing.Color.AliceBlue;
            this.labelFilm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelFilm.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelFilm.Location = new System.Drawing.Point(1, 1);
            this.labelFilm.Margin = new System.Windows.Forms.Padding(0);
            this.labelFilm.Name = "labelFilm";
            this.labelFilm.Size = new System.Drawing.Size(166, 28);
            this.labelFilm.TabIndex = 1;
            this.labelFilm.Text = "Film (T100)";
            this.labelFilm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.AliceBlue;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(168, 1);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 28);
            this.label1.TabIndex = 2;
            this.label1.Text = "Coating (T101)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.AliceBlue;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(335, 1);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 28);
            this.label2.TabIndex = 3;
            this.label2.Text = "Printing (T102)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPrintingValue
            // 
            this.labelPrintingValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPrintingValue.Font = new System.Drawing.Font("맑은 고딕", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelPrintingValue.Location = new System.Drawing.Point(340, 30);
            this.labelPrintingValue.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelPrintingValue.Name = "labelPrintingValue";
            this.labelPrintingValue.Size = new System.Drawing.Size(156, 29);
            this.labelPrintingValue.TabIndex = 8;
            this.labelPrintingValue.Text = "XXX.XXX";
            this.labelPrintingValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCoatingValue
            // 
            this.labelCoatingValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCoatingValue.Font = new System.Drawing.Font("맑은 고딕", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelCoatingValue.Location = new System.Drawing.Point(173, 30);
            this.labelCoatingValue.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelCoatingValue.Name = "labelCoatingValue";
            this.labelCoatingValue.Size = new System.Drawing.Size(156, 29);
            this.labelCoatingValue.TabIndex = 9;
            this.labelCoatingValue.Text = "XXX.XXX";
            this.labelCoatingValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPrintingPos
            // 
            this.labelPrintingPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPrintingPos.Font = new System.Drawing.Font("맑은 고딕", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelPrintingPos.Location = new System.Drawing.Point(507, 30);
            this.labelPrintingPos.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelPrintingPos.Name = "labelPrintingPos";
            this.labelPrintingPos.Size = new System.Drawing.Size(159, 29);
            this.labelPrintingPos.TabIndex = 11;
            this.labelPrintingPos.Text = "XXX.XXX";
            this.labelPrintingPos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBarZeroing
            // 
            this.progressBarZeroing.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarZeroing.Location = new System.Drawing.Point(0, 50);
            this.progressBarZeroing.Margin = new System.Windows.Forms.Padding(0);
            this.progressBarZeroing.Name = "progressBarZeroing";
            this.progressBarZeroing.Size = new System.Drawing.Size(672, 30);
            this.progressBarZeroing.TabIndex = 48;
            // 
            // timer_GUIRevison
            // 
            this.timer_GUIRevison.Tick += new System.EventHandler(this.timer_GUIRevison_Tick);
            // 
            // InspectionPanelRight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.layoutMain);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "InspectionPanelRight";
            this.Size = new System.Drawing.Size(672, 803);
            this.Load += new System.EventHandler(this.InspectionPanelRight_Load);
            this.VisibleChanged += new System.EventHandler(this.InspectionPanelRight_VisibleChanged);
            this.panelParam.ResumeLayout(false);
            this.panelParam.PerformLayout();
            this.groupThresHold.ResumeLayout(false);
            this.groupThresHold.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPrintingThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCoatingThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFilmThreshold)).EndInit();
            this.groupLightParameter.ResumeLayout(false);
            this.groupLightParameter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frontLightValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backLightValue)).EndInit();
            this.layoutMain.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanelCurrentValue.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelImage;
        private System.Windows.Forms.Panel panelParam;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelCurrentValue;
        private System.Windows.Forms.Label labelFilm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelFilmValue;
        private System.Windows.Forms.Label labelCoatingValue;
        private System.Windows.Forms.Label labelPrintingValue;
        private System.Windows.Forms.Label labelBack;
        private System.Windows.Forms.GroupBox groupLightParameter;
        private System.Windows.Forms.NumericUpDown frontLightValue;
        private System.Windows.Forms.Label labelFront;
        private System.Windows.Forms.NumericUpDown backLightValue;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.NumericUpDown numericUpDownPrintingThreshold;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDownCoatingThreshold;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDownFilmThreshold;
        private System.Windows.Forms.GroupBox groupThresHold;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.ProgressBar progressBarZeroing;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelState;
        private Infragistics.Win.Misc.UltraButton buttonStateReset;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelPrintingPos;
        private System.Windows.Forms.CheckBox checkOnTune;
        private System.Windows.Forms.Timer timer_GUIRevison;
        private System.Windows.Forms.ComboBox cb_Preset;
        private System.Windows.Forms.Label label_Preset;
    }
}
