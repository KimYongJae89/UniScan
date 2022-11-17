namespace UniEye.Base.UI
{
    partial class RobotCalibrationForm
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
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.tabPageCamera = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.tabPageMap = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.CameraCalibrationForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.buttonStart = new System.Windows.Forms.Button();
            this.cmbJigType = new System.Windows.Forms.ComboBox();
            this.labelEaY = new System.Windows.Forms.Label();
            this.labelUnitStartY = new System.Windows.Forms.Label();
            this.labelUnitYStep = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelStartY = new System.Windows.Forms.Label();
            this.labelYStep = new System.Windows.Forms.Label();
            this.labelUnitCircleDiameter = new System.Windows.Forms.Label();
            this.labelEaX = new System.Windows.Forms.Label();
            this.labelUnitStartX = new System.Windows.Forms.Label();
            this.labelUnitXStep = new System.Windows.Forms.Label();
            this.labelJigType = new System.Windows.Forms.Label();
            this.labelCircleDiameter = new System.Windows.Forms.Label();
            this.labelNumPosition = new System.Windows.Forms.Label();
            this.labelStartPosition = new System.Windows.Forms.Label();
            this.labelMovingStep = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelStartX = new System.Windows.Forms.Label();
            this.labelXStep = new System.Windows.Forms.Label();
            this.panelParam = new System.Windows.Forms.Panel();
            this.groupTest = new Infragistics.Win.Misc.UltraGroupBox();
            this.buttonTestGrab = new System.Windows.Forms.Button();
            this.buttonExportTestData = new System.Windows.Forms.Button();
            this.joystickPanel = new System.Windows.Forms.Panel();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonOrigin = new System.Windows.Forms.Button();
            this.circleDiameter = new System.Windows.Forms.NumericUpDown();
            this.numPosY = new System.Windows.Forms.NumericUpDown();
            this.numPosX = new System.Windows.Forms.NumericUpDown();
            this.txtStartY = new System.Windows.Forms.TextBox();
            this.txtStartX = new System.Windows.Forms.TextBox();
            this.txtYStep = new System.Windows.Forms.TextBox();
            this.txtXStep = new System.Windows.Forms.TextBox();
            this.buttonUpdateStartPos = new System.Windows.Forms.Button();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.mainTab = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Point = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.refPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.realPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOffsetX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnOffsetY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.CameraCalibrationForm_Fill_Panel.SuspendLayout();
            this.panelParam.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupTest)).BeginInit();
            this.groupTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.circleDiameter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPosY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPosX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainTab)).BeginInit();
            this.mainTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabPageCamera
            // 
            this.tabPageCamera.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPageCamera.Location = new System.Drawing.Point(1, 23);
            this.tabPageCamera.Name = "tabPageCamera";
            this.tabPageCamera.Size = new System.Drawing.Size(540, 746);
            // 
            // tabPageMap
            // 
            this.tabPageMap.Location = new System.Drawing.Point(-8750, -8000);
            this.tabPageMap.Name = "tabPageMap";
            this.tabPageMap.Size = new System.Drawing.Size(542, 753);
            // 
            // ultraFormManager
            // 
            this.ultraFormManager.Form = this;
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
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
            this._ConfigPage_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(1244, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 829);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(1244, 1);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Left
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 798);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(1243, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 798);
            // 
            // CameraCalibrationForm_Fill_Panel
            // 
            this.CameraCalibrationForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.CameraCalibrationForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CameraCalibrationForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.CameraCalibrationForm_Fill_Panel.Name = "CameraCalibrationForm_Fill_Panel";
            this.CameraCalibrationForm_Fill_Panel.Size = new System.Drawing.Size(1242, 798);
            this.CameraCalibrationForm_Fill_Panel.TabIndex = 17;
            // 
            // progressBar
            // 
            this.progressBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar.Location = new System.Drawing.Point(164, 803);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1079, 26);
            this.progressBar.TabIndex = 24;
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(16, 359);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(129, 39);
            this.buttonStart.TabIndex = 3;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // cmbJigType
            // 
            this.cmbJigType.FormattingEnabled = true;
            this.cmbJigType.Items.AddRange(new object[] {
            "Type 1"});
            this.cmbJigType.Location = new System.Drawing.Point(34, 24);
            this.cmbJigType.Name = "cmbJigType";
            this.cmbJigType.Size = new System.Drawing.Size(98, 20);
            this.cmbJigType.TabIndex = 2;
            // 
            // labelEaY
            // 
            this.labelEaY.AutoSize = true;
            this.labelEaY.Location = new System.Drawing.Point(124, 245);
            this.labelEaY.Name = "labelEaY";
            this.labelEaY.Size = new System.Drawing.Size(21, 12);
            this.labelEaY.TabIndex = 1;
            this.labelEaY.Text = "EA";
            // 
            // labelUnitStartY
            // 
            this.labelUnitStartY.AutoSize = true;
            this.labelUnitStartY.Location = new System.Drawing.Point(124, 171);
            this.labelUnitStartY.Name = "labelUnitStartY";
            this.labelUnitStartY.Size = new System.Drawing.Size(23, 12);
            this.labelUnitStartY.TabIndex = 1;
            this.labelUnitStartY.Text = "um";
            // 
            // labelUnitYStep
            // 
            this.labelUnitYStep.AutoSize = true;
            this.labelUnitYStep.Location = new System.Drawing.Point(124, 95);
            this.labelUnitYStep.Name = "labelUnitYStep";
            this.labelUnitYStep.Size = new System.Drawing.Size(23, 12);
            this.labelUnitYStep.TabIndex = 1;
            this.labelUnitYStep.Text = "um";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 245);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "Y";
            // 
            // labelStartY
            // 
            this.labelStartY.AutoSize = true;
            this.labelStartY.Location = new System.Drawing.Point(14, 171);
            this.labelStartY.Name = "labelStartY";
            this.labelStartY.Size = new System.Drawing.Size(13, 12);
            this.labelStartY.TabIndex = 1;
            this.labelStartY.Text = "Y";
            // 
            // labelYStep
            // 
            this.labelYStep.AutoSize = true;
            this.labelYStep.Location = new System.Drawing.Point(14, 95);
            this.labelYStep.Name = "labelYStep";
            this.labelYStep.Size = new System.Drawing.Size(13, 12);
            this.labelYStep.TabIndex = 1;
            this.labelYStep.Text = "Y";
            // 
            // labelUnitCircleDiameter
            // 
            this.labelUnitCircleDiameter.AutoSize = true;
            this.labelUnitCircleDiameter.Location = new System.Drawing.Point(124, 293);
            this.labelUnitCircleDiameter.Name = "labelUnitCircleDiameter";
            this.labelUnitCircleDiameter.Size = new System.Drawing.Size(23, 12);
            this.labelUnitCircleDiameter.TabIndex = 1;
            this.labelUnitCircleDiameter.Text = "um";
            // 
            // labelEaX
            // 
            this.labelEaX.AutoSize = true;
            this.labelEaX.Location = new System.Drawing.Point(124, 221);
            this.labelEaX.Name = "labelEaX";
            this.labelEaX.Size = new System.Drawing.Size(21, 12);
            this.labelEaX.TabIndex = 1;
            this.labelEaX.Text = "EA";
            // 
            // labelUnitStartX
            // 
            this.labelUnitStartX.AutoSize = true;
            this.labelUnitStartX.Location = new System.Drawing.Point(124, 147);
            this.labelUnitStartX.Name = "labelUnitStartX";
            this.labelUnitStartX.Size = new System.Drawing.Size(23, 12);
            this.labelUnitStartX.TabIndex = 1;
            this.labelUnitStartX.Text = "um";
            // 
            // labelUnitXStep
            // 
            this.labelUnitXStep.AutoSize = true;
            this.labelUnitXStep.Location = new System.Drawing.Point(124, 71);
            this.labelUnitXStep.Name = "labelUnitXStep";
            this.labelUnitXStep.Size = new System.Drawing.Size(23, 12);
            this.labelUnitXStep.TabIndex = 1;
            this.labelUnitXStep.Text = "um";
            // 
            // labelJigType
            // 
            this.labelJigType.AutoSize = true;
            this.labelJigType.Location = new System.Drawing.Point(9, 9);
            this.labelJigType.Name = "labelJigType";
            this.labelJigType.Size = new System.Drawing.Size(54, 12);
            this.labelJigType.TabIndex = 1;
            this.labelJigType.Text = "Jig Type";
            // 
            // labelCircleDiameter
            // 
            this.labelCircleDiameter.AutoSize = true;
            this.labelCircleDiameter.Location = new System.Drawing.Point(9, 271);
            this.labelCircleDiameter.Name = "labelCircleDiameter";
            this.labelCircleDiameter.Size = new System.Drawing.Size(92, 12);
            this.labelCircleDiameter.TabIndex = 1;
            this.labelCircleDiameter.Text = "Circle Diameter";
            // 
            // labelNumPosition
            // 
            this.labelNumPosition.AutoSize = true;
            this.labelNumPosition.Location = new System.Drawing.Point(9, 199);
            this.labelNumPosition.Name = "labelNumPosition";
            this.labelNumPosition.Size = new System.Drawing.Size(81, 12);
            this.labelNumPosition.TabIndex = 1;
            this.labelNumPosition.Text = "Num Position";
            // 
            // labelStartPosition
            // 
            this.labelStartPosition.AutoSize = true;
            this.labelStartPosition.Location = new System.Drawing.Point(9, 125);
            this.labelStartPosition.Name = "labelStartPosition";
            this.labelStartPosition.Size = new System.Drawing.Size(79, 12);
            this.labelStartPosition.TabIndex = 1;
            this.labelStartPosition.Text = "Start Position";
            // 
            // labelMovingStep
            // 
            this.labelMovingStep.AutoSize = true;
            this.labelMovingStep.Location = new System.Drawing.Point(9, 49);
            this.labelMovingStep.Name = "labelMovingStep";
            this.labelMovingStep.Size = new System.Drawing.Size(75, 12);
            this.labelMovingStep.TabIndex = 1;
            this.labelMovingStep.Text = "Moving Step";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 221);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "X";
            // 
            // labelStartX
            // 
            this.labelStartX.AutoSize = true;
            this.labelStartX.Location = new System.Drawing.Point(14, 147);
            this.labelStartX.Name = "labelStartX";
            this.labelStartX.Size = new System.Drawing.Size(13, 12);
            this.labelStartX.TabIndex = 1;
            this.labelStartX.Text = "X";
            // 
            // labelXStep
            // 
            this.labelXStep.AutoSize = true;
            this.labelXStep.Location = new System.Drawing.Point(14, 71);
            this.labelXStep.Name = "labelXStep";
            this.labelXStep.Size = new System.Drawing.Size(13, 12);
            this.labelXStep.TabIndex = 1;
            this.labelXStep.Text = "X";
            // 
            // panelParam
            // 
            this.panelParam.Controls.Add(this.groupTest);
            this.panelParam.Controls.Add(this.joystickPanel);
            this.panelParam.Controls.Add(this.buttonStop);
            this.panelParam.Controls.Add(this.buttonOrigin);
            this.panelParam.Controls.Add(this.circleDiameter);
            this.panelParam.Controls.Add(this.numPosY);
            this.panelParam.Controls.Add(this.numPosX);
            this.panelParam.Controls.Add(this.txtStartY);
            this.panelParam.Controls.Add(this.txtStartX);
            this.panelParam.Controls.Add(this.txtYStep);
            this.panelParam.Controls.Add(this.txtXStep);
            this.panelParam.Controls.Add(this.buttonUpdateStartPos);
            this.panelParam.Controls.Add(this.buttonStart);
            this.panelParam.Controls.Add(this.cmbJigType);
            this.panelParam.Controls.Add(this.labelEaY);
            this.panelParam.Controls.Add(this.labelUnitStartY);
            this.panelParam.Controls.Add(this.labelUnitYStep);
            this.panelParam.Controls.Add(this.label4);
            this.panelParam.Controls.Add(this.labelStartY);
            this.panelParam.Controls.Add(this.labelYStep);
            this.panelParam.Controls.Add(this.labelUnitCircleDiameter);
            this.panelParam.Controls.Add(this.labelEaX);
            this.panelParam.Controls.Add(this.labelUnitStartX);
            this.panelParam.Controls.Add(this.labelUnitXStep);
            this.panelParam.Controls.Add(this.labelJigType);
            this.panelParam.Controls.Add(this.labelCircleDiameter);
            this.panelParam.Controls.Add(this.labelNumPosition);
            this.panelParam.Controls.Add(this.labelStartPosition);
            this.panelParam.Controls.Add(this.labelMovingStep);
            this.panelParam.Controls.Add(this.label1);
            this.panelParam.Controls.Add(this.labelStartX);
            this.panelParam.Controls.Add(this.labelXStep);
            this.panelParam.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelParam.Location = new System.Drawing.Point(1, 31);
            this.panelParam.Name = "panelParam";
            this.panelParam.Size = new System.Drawing.Size(163, 798);
            this.panelParam.TabIndex = 22;
            // 
            // groupTest
            // 
            this.groupTest.BorderStyle = Infragistics.Win.Misc.GroupBoxBorderStyle.Rectangular3D;
            this.groupTest.Controls.Add(this.buttonTestGrab);
            this.groupTest.Controls.Add(this.buttonExportTestData);
            this.groupTest.Location = new System.Drawing.Point(3, 490);
            this.groupTest.Name = "groupTest";
            this.groupTest.Size = new System.Drawing.Size(155, 114);
            this.groupTest.TabIndex = 44;
            this.groupTest.Text = "Test";
            // 
            // buttonTestGrab
            // 
            this.buttonTestGrab.Location = new System.Drawing.Point(13, 19);
            this.buttonTestGrab.Name = "buttonTestGrab";
            this.buttonTestGrab.Size = new System.Drawing.Size(129, 39);
            this.buttonTestGrab.TabIndex = 29;
            this.buttonTestGrab.Text = "Test";
            this.buttonTestGrab.UseVisualStyleBackColor = true;
            this.buttonTestGrab.Click += new System.EventHandler(this.buttonTestGrab_Click);
            // 
            // buttonExportTestData
            // 
            this.buttonExportTestData.Location = new System.Drawing.Point(13, 64);
            this.buttonExportTestData.Name = "buttonExportTestData";
            this.buttonExportTestData.Size = new System.Drawing.Size(129, 39);
            this.buttonExportTestData.TabIndex = 29;
            this.buttonExportTestData.Text = "Export";
            this.buttonExportTestData.UseVisualStyleBackColor = true;
            this.buttonExportTestData.Click += new System.EventHandler(this.buttonExportTestData_Click);
            // 
            // joystickPanel
            // 
            this.joystickPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.joystickPanel.Location = new System.Drawing.Point(0, 610);
            this.joystickPanel.Name = "joystickPanel";
            this.joystickPanel.Size = new System.Drawing.Size(163, 188);
            this.joystickPanel.TabIndex = 43;
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(16, 439);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(129, 39);
            this.buttonStop.TabIndex = 38;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonOrigin
            // 
            this.buttonOrigin.Location = new System.Drawing.Point(16, 399);
            this.buttonOrigin.Name = "buttonOrigin";
            this.buttonOrigin.Size = new System.Drawing.Size(129, 39);
            this.buttonOrigin.TabIndex = 38;
            this.buttonOrigin.Text = "Origin";
            this.buttonOrigin.UseVisualStyleBackColor = true;
            this.buttonOrigin.Click += new System.EventHandler(this.buttonOrigin_Click);
            // 
            // circleDiameter
            // 
            this.circleDiameter.Location = new System.Drawing.Point(34, 288);
            this.circleDiameter.Name = "circleDiameter";
            this.circleDiameter.Size = new System.Drawing.Size(85, 21);
            this.circleDiameter.TabIndex = 37;
            this.circleDiameter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.circleDiameter.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // numPosY
            // 
            this.numPosY.Location = new System.Drawing.Point(34, 244);
            this.numPosY.Name = "numPosY";
            this.numPosY.Size = new System.Drawing.Size(85, 21);
            this.numPosY.TabIndex = 37;
            this.numPosY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPosY.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // numPosX
            // 
            this.numPosX.Location = new System.Drawing.Point(34, 219);
            this.numPosX.Name = "numPosX";
            this.numPosX.Size = new System.Drawing.Size(85, 21);
            this.numPosX.TabIndex = 36;
            this.numPosX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numPosX.Value = new decimal(new int[] {
            19,
            0,
            0,
            0});
            // 
            // txtStartY
            // 
            this.txtStartY.Location = new System.Drawing.Point(33, 168);
            this.txtStartY.Name = "txtStartY";
            this.txtStartY.Size = new System.Drawing.Size(85, 21);
            this.txtStartY.TabIndex = 35;
            this.txtStartY.Text = "-5342";
            // 
            // txtStartX
            // 
            this.txtStartX.Location = new System.Drawing.Point(33, 144);
            this.txtStartX.Name = "txtStartX";
            this.txtStartX.Size = new System.Drawing.Size(85, 21);
            this.txtStartX.TabIndex = 34;
            this.txtStartX.Text = "-5634";
            // 
            // txtYStep
            // 
            this.txtYStep.Location = new System.Drawing.Point(34, 92);
            this.txtYStep.Name = "txtYStep";
            this.txtYStep.Size = new System.Drawing.Size(85, 21);
            this.txtYStep.TabIndex = 33;
            this.txtYStep.Text = "20000";
            // 
            // txtXStep
            // 
            this.txtXStep.Location = new System.Drawing.Point(34, 68);
            this.txtXStep.Name = "txtXStep";
            this.txtXStep.Size = new System.Drawing.Size(84, 21);
            this.txtXStep.TabIndex = 32;
            this.txtXStep.Text = "20000";
            // 
            // buttonUpdateStartPos
            // 
            this.buttonUpdateStartPos.Location = new System.Drawing.Point(16, 319);
            this.buttonUpdateStartPos.Name = "buttonUpdateStartPos";
            this.buttonUpdateStartPos.Size = new System.Drawing.Size(129, 39);
            this.buttonUpdateStartPos.TabIndex = 3;
            this.buttonUpdateStartPos.Text = "Update Start Pos";
            this.buttonUpdateStartPos.UseVisualStyleBackColor = true;
            this.buttonUpdateStartPos.Click += new System.EventHandler(this.buttonUpdateStartPos_Click);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(540, 746);
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.ultraTabSharedControlsPage1);
            this.mainTab.Controls.Add(this.tabPageCamera);
            this.mainTab.Controls.Add(this.tabPageMap);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(164, 31);
            this.mainTab.Name = "mainTab";
            this.mainTab.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.mainTab.Size = new System.Drawing.Size(544, 772);
            this.mainTab.TabIndex = 23;
            ultraTab1.Key = "Camera";
            ultraTab1.TabPage = this.tabPageCamera;
            ultraTab1.Text = "Camera";
            ultraTab2.Key = "Map";
            ultraTab2.TabPage = this.tabPageMap;
            ultraTab2.Text = "Map";
            this.mainTab.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Point,
            this.refPosition,
            this.realPosition,
            this.columnOffsetX,
            this.columnOffsetY,
            this.length});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Right;
            this.dataGridView1.Location = new System.Drawing.Point(708, 31);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(535, 772);
            this.dataGridView1.TabIndex = 29;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // Point
            // 
            this.Point.HeaderText = "Point";
            this.Point.Name = "Point";
            this.Point.ReadOnly = true;
            this.Point.Width = 60;
            // 
            // refPosition
            // 
            this.refPosition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.refPosition.HeaderText = "Ref. Position";
            this.refPosition.Name = "refPosition";
            this.refPosition.ReadOnly = true;
            // 
            // realPosition
            // 
            this.realPosition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.realPosition.HeaderText = "Real Position";
            this.realPosition.Name = "realPosition";
            // 
            // columnOffsetX
            // 
            this.columnOffsetX.HeaderText = "Offset X";
            this.columnOffsetX.Name = "columnOffsetX";
            this.columnOffsetX.ReadOnly = true;
            this.columnOffsetX.Width = 80;
            // 
            // columnOffsetY
            // 
            this.columnOffsetY.HeaderText = "Offset Y";
            this.columnOffsetY.Name = "columnOffsetY";
            this.columnOffsetY.Width = 80;
            // 
            // length
            // 
            this.length.HeaderText = "Length";
            this.length.Name = "length";
            this.length.ReadOnly = true;
            this.length.Width = 80;
            // 
            // RobotCalibrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1244, 830);
            this.Controls.Add(this.mainTab);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.panelParam);
            this.Controls.Add(this.CameraCalibrationForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Name = "RobotCalibrationForm";
            this.Text = "Robot Calibration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RobotCalibrationForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.CameraCalibrationForm_Fill_Panel.ResumeLayout(false);
            this.panelParam.ResumeLayout(false);
            this.panelParam.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupTest)).EndInit();
            this.groupTest.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.circleDiameter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPosY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPosX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainTab)).EndInit();
            this.mainTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel CameraCalibrationForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Panel panelParam;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.ComboBox cmbJigType;
        private System.Windows.Forms.Label labelEaY;
        private System.Windows.Forms.Label labelUnitStartY;
        private System.Windows.Forms.Label labelUnitYStep;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelStartY;
        private System.Windows.Forms.Label labelYStep;
        private System.Windows.Forms.Label labelUnitCircleDiameter;
        private System.Windows.Forms.Label labelEaX;
        private System.Windows.Forms.Label labelUnitStartX;
        private System.Windows.Forms.Label labelUnitXStep;
        private System.Windows.Forms.Label labelJigType;
        private System.Windows.Forms.Label labelCircleDiameter;
        private System.Windows.Forms.Label labelNumPosition;
        private System.Windows.Forms.Label labelStartPosition;
        private System.Windows.Forms.Label labelMovingStep;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelStartX;
        private System.Windows.Forms.Label labelXStep;
        private System.Windows.Forms.Button buttonTestGrab;
        private System.Windows.Forms.TextBox txtXStep;
        private System.Windows.Forms.TextBox txtYStep;
        private System.Windows.Forms.TextBox txtStartX;
        private System.Windows.Forms.NumericUpDown numPosX;
        private System.Windows.Forms.TextBox txtStartY;
        private System.Windows.Forms.NumericUpDown numPosY;
        private System.Windows.Forms.Button buttonOrigin;
        private System.Windows.Forms.Panel joystickPanel;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl mainTab;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageCamera;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageMap;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.NumericUpDown circleDiameter;
        private System.Windows.Forms.Button buttonUpdateStartPos;
        private Infragistics.Win.Misc.UltraGroupBox groupTest;
        private System.Windows.Forms.Button buttonExportTestData;
        private System.Windows.Forms.DataGridViewTextBoxColumn Point;
        private System.Windows.Forms.DataGridViewTextBoxColumn refPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn realPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOffsetX;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnOffsetY;
        private System.Windows.Forms.DataGridViewTextBoxColumn length;
    }
}