namespace UniEye.Base.UI
{
    partial class ModellerPage
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
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab1 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModellerPage));
            this.ultraTabPageCamera = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.ultraTabPage3dViewer = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.toolTip = new System.Windows.Forms.ToolTip();
            this.saveButton = new System.Windows.Forms.Button();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.cameraImagePanel = new System.Windows.Forms.Panel();
            this.mainTabView = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.mainContainer = new System.Windows.Forms.SplitContainer();
            this.paramContainer = new System.Windows.Forms.SplitContainer();
            this.panelAlign = new System.Windows.Forms.Panel();
            this.fidAngle = new System.Windows.Forms.TextBox();
            this.fidOffset = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxFidDistance = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxDesiredDistance = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labelFidAngle = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelFidOffset = new System.Windows.Forms.Label();
            this.labelDesiredDistance = new System.Windows.Forms.Label();
            this.labelFidDistance = new System.Windows.Forms.Label();
            this.numericUpDownDistanceOffset = new System.Windows.Forms.NumericUpDown();
            this.labelDistanceOffset = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelUm = new System.Windows.Forms.Label();
            this.buttonAlign = new System.Windows.Forms.Button();
            this.tabControlUtil = new System.Windows.Forms.TabControl();
            this.tabPageResult = new System.Windows.Forms.TabPage();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageModel = new System.Windows.Forms.TabPage();
            this.toolStripModel = new System.Windows.Forms.ToolStrip();
            this.resetModelButton = new System.Windows.Forms.ToolStripButton();
            this.importGerberButton = new System.Windows.Forms.ToolStripButton();
            this.modelPropertyButton = new System.Windows.Forms.ToolStripButton();
            this.exportFormatButton = new System.Windows.Forms.ToolStripButton();
            this.editSchemaButton = new System.Windows.Forms.ToolStripButton();
            this.scanButton = new System.Windows.Forms.ToolStripButton();
            this.tabPageFov = new System.Windows.Forms.TabPage();
            this.toolStripFov = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelStep = new System.Windows.Forms.ToolStripLabel();
            this.comboStep = new System.Windows.Forms.ToolStripComboBox();
            this.movePrevStepButton = new System.Windows.Forms.ToolStripButton();
            this.moveNextStepButton = new System.Windows.Forms.ToolStripButton();
            this.addStepButton = new System.Windows.Forms.ToolStripButton();
            this.deleteStepButton = new System.Windows.Forms.ToolStripButton();
            this.editStepButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.selectCameraButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.loadImageSetToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.selectPrevImageSetToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.selectNextImageSetToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.selectLightButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.showLightPanelToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.grabProcessToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.singleShotToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.multiShotToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.calibration3dButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAlign = new System.Windows.Forms.ToolStripButton();
            this.tabPageProbe = new System.Windows.Forms.TabPage();
            this.toolStripProbe = new System.Windows.Forms.ToolStrip();
            this.addProbeToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyProbeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pasteProbeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteProbeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.groupProbeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ungroupProbeToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorGroup = new System.Windows.Forms.ToolStripSeparator();
            this.setFiducialToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.setTargetCalibrationToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorCalibration = new System.Windows.Forms.ToolStripSeparator();
            this.syncParamToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.syncAllToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.dontMoveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.addFiducialToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteFiducialToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toggleFiducialToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorFiducial = new System.Windows.Forms.ToolStripSeparator();
            this.undoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.RedoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.tabPageView = new System.Windows.Forms.TabPage();
            this.toolStripView = new System.Windows.Forms.ToolStrip();
            this.zoomInToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.zoomOutToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.zoomFitToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.previewToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.previewTypeToolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.tabPageRobot = new System.Windows.Forms.TabPage();
            this.toolStripRobot = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOrigin = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonJoystick = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRobotSetting = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFineMove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.panelMenu = new System.Windows.Forms.Panel();
            this.inspectionButton = new System.Windows.Forms.Button();
            this.panelSubMenu = new System.Windows.Forms.TableLayoutPanel();
            this.grab3dToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.cameraImagePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainTabView)).BeginInit();
            this.mainTabView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).BeginInit();
            this.mainContainer.Panel1.SuspendLayout();
            this.mainContainer.Panel2.SuspendLayout();
            this.mainContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paramContainer)).BeginInit();
            this.paramContainer.Panel1.SuspendLayout();
            this.paramContainer.Panel2.SuspendLayout();
            this.paramContainer.SuspendLayout();
            this.panelAlign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDistanceOffset)).BeginInit();
            this.tabControlUtil.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPageModel.SuspendLayout();
            this.toolStripModel.SuspendLayout();
            this.tabPageFov.SuspendLayout();
            this.toolStripFov.SuspendLayout();
            this.tabPageProbe.SuspendLayout();
            this.toolStripProbe.SuspendLayout();
            this.tabPageView.SuspendLayout();
            this.toolStripView.SuspendLayout();
            this.tabPageRobot.SuspendLayout();
            this.toolStripRobot.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.panelSubMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // ultraTabPageCamera
            // 
            this.ultraTabPageCamera.Location = new System.Drawing.Point(1, 28);
            this.ultraTabPageCamera.Name = "ultraTabPageCamera";
            this.ultraTabPageCamera.Size = new System.Drawing.Size(949, 531);
            // 
            // ultraTabPage3dViewer
            // 
            this.ultraTabPage3dViewer.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabPage3dViewer.Name = "ultraTabPage3dViewer";
            this.ultraTabPage3dViewer.Size = new System.Drawing.Size(949, 531);
            // 
            // toolTip
            // 
            this.toolTip.IsBalloon = true;
            // 
            // saveButton
            // 
            this.saveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.saveButton.Image = global::UniEye.Base.Properties.Resources.save_32;
            this.saveButton.Location = new System.Drawing.Point(0, 0);
            this.saveButton.Margin = new System.Windows.Forms.Padding(0);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(53, 49);
            this.saveButton.TabIndex = 0;
            this.toolTip.SetToolTip(this.saveButton, "Save");
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // buttonPreview
            // 
            this.buttonPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPreview.Image = global::UniEye.Base.Properties.Resources.preview_32;
            this.buttonPreview.Location = new System.Drawing.Point(0, 49);
            this.buttonPreview.Margin = new System.Windows.Forms.Padding(0);
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(53, 49);
            this.buttonPreview.TabIndex = 1;
            this.buttonPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonPreview, "Preview");
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // cameraImagePanel
            // 
            this.cameraImagePanel.AutoSize = true;
            this.cameraImagePanel.Controls.Add(this.mainTabView);
            this.cameraImagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraImagePanel.Location = new System.Drawing.Point(0, 0);
            this.cameraImagePanel.Name = "cameraImagePanel";
            this.cameraImagePanel.Size = new System.Drawing.Size(953, 562);
            this.cameraImagePanel.TabIndex = 113;
            // 
            // mainTabView
            // 
            this.mainTabView.Controls.Add(this.ultraTabSharedControlsPage1);
            this.mainTabView.Controls.Add(this.ultraTabPageCamera);
            this.mainTabView.Controls.Add(this.ultraTabPage3dViewer);
            this.mainTabView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabView.Location = new System.Drawing.Point(0, 0);
            this.mainTabView.Name = "mainTabView";
            this.mainTabView.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.mainTabView.Size = new System.Drawing.Size(953, 562);
            this.mainTabView.TabIndex = 125;
            ultraTab1.Key = "Camera";
            ultraTab1.TabPage = this.ultraTabPageCamera;
            ultraTab1.Text = "Camera";
            ultraTab2.Key = "3DViewer";
            ultraTab2.TabPage = this.ultraTabPage3dViewer;
            ultraTab2.Text = "3D Viewer";
            this.mainTabView.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab1,
            ultraTab2});
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(949, 531);
            // 
            // mainContainer
            // 
            this.mainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.mainContainer.Location = new System.Drawing.Point(0, 98);
            this.mainContainer.Name = "mainContainer";
            // 
            // mainContainer.Panel1
            // 
            this.mainContainer.Panel1.Controls.Add(this.cameraImagePanel);
            // 
            // mainContainer.Panel2
            // 
            this.mainContainer.Panel2.Controls.Add(this.paramContainer);
            this.mainContainer.Size = new System.Drawing.Size(1455, 562);
            this.mainContainer.SplitterDistance = 953;
            this.mainContainer.TabIndex = 133;
            // 
            // paramContainer
            // 
            this.paramContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramContainer.Location = new System.Drawing.Point(0, 0);
            this.paramContainer.Name = "paramContainer";
            this.paramContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // paramContainer.Panel1
            // 
            this.paramContainer.Panel1.Controls.Add(this.panelAlign);
            // 
            // paramContainer.Panel2
            // 
            this.paramContainer.Panel2.Controls.Add(this.tabControlUtil);
            this.paramContainer.Size = new System.Drawing.Size(498, 562);
            this.paramContainer.SplitterDistance = 316;
            this.paramContainer.TabIndex = 121;
            // 
            // panelAlign
            // 
            this.panelAlign.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelAlign.Controls.Add(this.fidAngle);
            this.panelAlign.Controls.Add(this.fidOffset);
            this.panelAlign.Controls.Add(this.label3);
            this.panelAlign.Controls.Add(this.textBoxFidDistance);
            this.panelAlign.Controls.Add(this.label2);
            this.panelAlign.Controls.Add(this.textBoxDesiredDistance);
            this.panelAlign.Controls.Add(this.label5);
            this.panelAlign.Controls.Add(this.labelFidAngle);
            this.panelAlign.Controls.Add(this.label4);
            this.panelAlign.Controls.Add(this.labelFidOffset);
            this.panelAlign.Controls.Add(this.labelDesiredDistance);
            this.panelAlign.Controls.Add(this.labelFidDistance);
            this.panelAlign.Controls.Add(this.numericUpDownDistanceOffset);
            this.panelAlign.Controls.Add(this.labelDistanceOffset);
            this.panelAlign.Controls.Add(this.buttonClose);
            this.panelAlign.Controls.Add(this.labelUm);
            this.panelAlign.Controls.Add(this.buttonAlign);
            this.panelAlign.Location = new System.Drawing.Point(4, 6);
            this.panelAlign.Name = "panelAlign";
            this.panelAlign.Size = new System.Drawing.Size(290, 224);
            this.panelAlign.TabIndex = 0;
            this.panelAlign.Visible = false;
            // 
            // fidAngle
            // 
            this.fidAngle.Location = new System.Drawing.Point(154, 138);
            this.fidAngle.Name = "fidAngle";
            this.fidAngle.ReadOnly = true;
            this.fidAngle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.fidAngle.Size = new System.Drawing.Size(100, 26);
            this.fidAngle.TabIndex = 167;
            // 
            // fidOffset
            // 
            this.fidOffset.Location = new System.Drawing.Point(154, 106);
            this.fidOffset.Name = "fidOffset";
            this.fidOffset.ReadOnly = true;
            this.fidOffset.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.fidOffset.Size = new System.Drawing.Size(100, 26);
            this.fidOffset.TabIndex = 167;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(254, 138);
            this.label3.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 20);
            this.label3.TabIndex = 165;
            this.label3.Text = "˚";
            // 
            // textBoxFidDistance
            // 
            this.textBoxFidDistance.Location = new System.Drawing.Point(154, 74);
            this.textBoxFidDistance.Name = "textBoxFidDistance";
            this.textBoxFidDistance.ReadOnly = true;
            this.textBoxFidDistance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxFidDistance.Size = new System.Drawing.Size(100, 26);
            this.textBoxFidDistance.TabIndex = 167;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(254, 106);
            this.label2.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 20);
            this.label2.TabIndex = 165;
            this.label2.Text = "um";
            // 
            // textBoxDesiredDistance
            // 
            this.textBoxDesiredDistance.Location = new System.Drawing.Point(154, 42);
            this.textBoxDesiredDistance.Name = "textBoxDesiredDistance";
            this.textBoxDesiredDistance.ReadOnly = true;
            this.textBoxDesiredDistance.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBoxDesiredDistance.Size = new System.Drawing.Size(100, 26);
            this.textBoxDesiredDistance.TabIndex = 166;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(254, 74);
            this.label5.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 20);
            this.label5.TabIndex = 165;
            this.label5.Text = "um";
            // 
            // labelFidAngle
            // 
            this.labelFidAngle.AutoSize = true;
            this.labelFidAngle.Location = new System.Drawing.Point(10, 138);
            this.labelFidAngle.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelFidAngle.Name = "labelFidAngle";
            this.labelFidAngle.Size = new System.Drawing.Size(80, 20);
            this.labelFidAngle.TabIndex = 162;
            this.labelFidAngle.Text = "Fid. Angle";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(254, 45);
            this.label4.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 20);
            this.label4.TabIndex = 164;
            this.label4.Text = "um";
            // 
            // labelFidOffset
            // 
            this.labelFidOffset.AutoSize = true;
            this.labelFidOffset.Location = new System.Drawing.Point(10, 106);
            this.labelFidOffset.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelFidOffset.Name = "labelFidOffset";
            this.labelFidOffset.Size = new System.Drawing.Size(83, 20);
            this.labelFidOffset.TabIndex = 162;
            this.labelFidOffset.Text = "Fid. Offset";
            // 
            // labelDesiredDistance
            // 
            this.labelDesiredDistance.AutoSize = true;
            this.labelDesiredDistance.Location = new System.Drawing.Point(10, 45);
            this.labelDesiredDistance.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelDesiredDistance.Name = "labelDesiredDistance";
            this.labelDesiredDistance.Size = new System.Drawing.Size(131, 20);
            this.labelDesiredDistance.TabIndex = 163;
            this.labelDesiredDistance.Text = "Desired Distance";
            // 
            // labelFidDistance
            // 
            this.labelFidDistance.AutoSize = true;
            this.labelFidDistance.Location = new System.Drawing.Point(10, 74);
            this.labelFidDistance.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelFidDistance.Name = "labelFidDistance";
            this.labelFidDistance.Size = new System.Drawing.Size(102, 20);
            this.labelFidDistance.TabIndex = 162;
            this.labelFidDistance.Text = "Fid. Distance";
            // 
            // numericUpDownDistanceOffset
            // 
            this.numericUpDownDistanceOffset.Location = new System.Drawing.Point(154, 9);
            this.numericUpDownDistanceOffset.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownDistanceOffset.Name = "numericUpDownDistanceOffset";
            this.numericUpDownDistanceOffset.Size = new System.Drawing.Size(100, 26);
            this.numericUpDownDistanceOffset.TabIndex = 161;
            this.numericUpDownDistanceOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownDistanceOffset.ValueChanged += new System.EventHandler(this.numericUpDownDistanceOffset_ValueChanged);
            // 
            // labelDistanceOffset
            // 
            this.labelDistanceOffset.AutoSize = true;
            this.labelDistanceOffset.Location = new System.Drawing.Point(10, 11);
            this.labelDistanceOffset.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelDistanceOffset.Name = "labelDistanceOffset";
            this.labelDistanceOffset.Size = new System.Drawing.Size(120, 20);
            this.labelDistanceOffset.TabIndex = 160;
            this.labelDistanceOffset.Text = "Distance Offset";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(145, 172);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(102, 42);
            this.buttonClose.TabIndex = 159;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // labelUm
            // 
            this.labelUm.AutoSize = true;
            this.labelUm.Location = new System.Drawing.Point(254, 11);
            this.labelUm.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelUm.Name = "labelUm";
            this.labelUm.Size = new System.Drawing.Size(31, 20);
            this.labelUm.TabIndex = 159;
            this.labelUm.Text = "um";
            // 
            // buttonAlign
            // 
            this.buttonAlign.Location = new System.Drawing.Point(41, 172);
            this.buttonAlign.Name = "buttonAlign";
            this.buttonAlign.Size = new System.Drawing.Size(102, 42);
            this.buttonAlign.TabIndex = 160;
            this.buttonAlign.Text = "Align";
            this.buttonAlign.UseVisualStyleBackColor = true;
            this.buttonAlign.Click += new System.EventHandler(this.buttonAlign_Click);
            // 
            // tabControlUtil
            // 
            this.tabControlUtil.Controls.Add(this.tabPageResult);
            this.tabControlUtil.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlUtil.Location = new System.Drawing.Point(0, 0);
            this.tabControlUtil.Name = "tabControlUtil";
            this.tabControlUtil.SelectedIndex = 0;
            this.tabControlUtil.Size = new System.Drawing.Size(498, 242);
            this.tabControlUtil.TabIndex = 0;
            // 
            // tabPageResult
            // 
            this.tabPageResult.Location = new System.Drawing.Point(4, 29);
            this.tabPageResult.Name = "tabPageResult";
            this.tabPageResult.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageResult.Size = new System.Drawing.Size(490, 209);
            this.tabPageResult.TabIndex = 0;
            this.tabPageResult.Text = "Result";
            this.tabPageResult.UseVisualStyleBackColor = true;
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPageModel);
            this.tabControlMain.Controls.Add(this.tabPageFov);
            this.tabControlMain.Controls.Add(this.tabPageProbe);
            this.tabControlMain.Controls.Add(this.tabPageView);
            this.tabControlMain.Controls.Add(this.tabPageRobot);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlMain.ItemSize = new System.Drawing.Size(70, 25);
            this.tabControlMain.Location = new System.Drawing.Point(175, 0);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1280, 98);
            this.tabControlMain.TabIndex = 134;
            this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
            // 
            // tabPageModel
            // 
            this.tabPageModel.Controls.Add(this.toolStripModel);
            this.tabPageModel.Location = new System.Drawing.Point(4, 29);
            this.tabPageModel.Name = "tabPageModel";
            this.tabPageModel.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageModel.Size = new System.Drawing.Size(1272, 65);
            this.tabPageModel.TabIndex = 5;
            this.tabPageModel.Text = "Model";
            this.tabPageModel.UseVisualStyleBackColor = true;
            // 
            // toolStripModel
            // 
            this.toolStripModel.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripModel.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripModel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetModelButton,
            this.importGerberButton,
            this.modelPropertyButton,
            this.exportFormatButton,
            this.editSchemaButton,
            this.scanButton});
            this.toolStripModel.Location = new System.Drawing.Point(3, 3);
            this.toolStripModel.Name = "toolStripModel";
            this.toolStripModel.Size = new System.Drawing.Size(1266, 60);
            this.toolStripModel.TabIndex = 2;
            this.toolStripModel.Text = "toolStrip1";
            this.toolStripModel.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStripModel_ItemClicked);
            // 
            // resetModelButton
            // 
            this.resetModelButton.Image = global::UniEye.Base.Properties.Resources.refresh_32;
            this.resetModelButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.resetModelButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resetModelButton.Name = "resetModelButton";
            this.resetModelButton.Size = new System.Drawing.Size(55, 57);
            this.resetModelButton.Text = "Reset";
            this.resetModelButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.resetModelButton.Visible = false;
            this.resetModelButton.Click += new System.EventHandler(this.resetModelButton_Click);
            // 
            // importGerberButton
            // 
            this.importGerberButton.Image = global::UniEye.Base.Properties.Resources.open_folder_32;
            this.importGerberButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.importGerberButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importGerberButton.Name = "importGerberButton";
            this.importGerberButton.Size = new System.Drawing.Size(121, 57);
            this.importGerberButton.Text = "Import Gerber";
            this.importGerberButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // modelPropertyButton
            // 
            this.modelPropertyButton.Image = global::UniEye.Base.Properties.Resources.property_32;
            this.modelPropertyButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.modelPropertyButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.modelPropertyButton.Name = "modelPropertyButton";
            this.modelPropertyButton.Size = new System.Drawing.Size(78, 57);
            this.modelPropertyButton.Text = "Property";
            this.modelPropertyButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.modelPropertyButton.Click += new System.EventHandler(this.modelPropertyButton_Click);
            // 
            // exportFormatButton
            // 
            this.exportFormatButton.Image = global::UniEye.Base.Properties.Resources.format_32;
            this.exportFormatButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.exportFormatButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exportFormatButton.Name = "exportFormatButton";
            this.exportFormatButton.Size = new System.Drawing.Size(119, 57);
            this.exportFormatButton.Text = "Export Format";
            this.exportFormatButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.exportFormatButton.Click += new System.EventHandler(this.exportFormatButton_Click);
            // 
            // editSchemaButton
            // 
            this.editSchemaButton.Image = global::UniEye.Base.Properties.Resources.schema_32;
            this.editSchemaButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.editSchemaButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editSchemaButton.Name = "editSchemaButton";
            this.editSchemaButton.Size = new System.Drawing.Size(71, 57);
            this.editSchemaButton.Text = "Schema";
            this.editSchemaButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.editSchemaButton.Click += new System.EventHandler(this.editSchemaButton_Click);
            // 
            // scanButton
            // 
            this.scanButton.Image = global::UniEye.Base.Properties.Resources.multi_shot_32;
            this.scanButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.scanButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(48, 57);
            this.scanButton.Text = "Scan";
            this.scanButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.scanButton.Click += new System.EventHandler(this.scanButton_Click);
            // 
            // tabPageFov
            // 
            this.tabPageFov.Controls.Add(this.toolStripFov);
            this.tabPageFov.Location = new System.Drawing.Point(4, 29);
            this.tabPageFov.Name = "tabPageFov";
            this.tabPageFov.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFov.Size = new System.Drawing.Size(1272, 65);
            this.tabPageFov.TabIndex = 1;
            this.tabPageFov.Text = "FOV";
            this.tabPageFov.UseVisualStyleBackColor = true;
            // 
            // toolStripFov
            // 
            this.toolStripFov.AutoSize = false;
            this.toolStripFov.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripFov.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripFov.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelStep,
            this.comboStep,
            this.movePrevStepButton,
            this.moveNextStepButton,
            this.addStepButton,
            this.deleteStepButton,
            this.editStepButton,
            this.toolStripSeparator7,
            this.selectCameraButton,
            this.loadImageSetToolStripButton,
            this.selectPrevImageSetToolStripButton,
            this.selectNextImageSetToolStripButton,
            this.toolStripSeparator8,
            this.selectLightButton,
            this.showLightPanelToolStripButton,
            this.grabProcessToolStripButton,
            this.singleShotToolStripButton,
            this.multiShotToolStripButton,
            this.toolStripSeparator9,
            this.calibration3dButton,
            this.toolStripButtonAlign});
            this.toolStripFov.Location = new System.Drawing.Point(3, 3);
            this.toolStripFov.Name = "toolStripFov";
            this.toolStripFov.Size = new System.Drawing.Size(1266, 61);
            this.toolStripFov.TabIndex = 1;
            this.toolStripFov.Text = "toolStrip2";
            // 
            // toolStripLabelStep
            // 
            this.toolStripLabelStep.Name = "toolStripLabelStep";
            this.toolStripLabelStep.Size = new System.Drawing.Size(44, 58);
            this.toolStripLabelStep.Text = "Step";
            // 
            // comboStep
            // 
            this.comboStep.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStep.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.comboStep.Name = "comboStep";
            this.comboStep.Size = new System.Drawing.Size(121, 61);
            this.comboStep.SelectedIndexChanged += new System.EventHandler(this.comboStep_SelectedIndexChanged);
            // 
            // movePrevStepButton
            // 
            this.movePrevStepButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.movePrevStepButton.Image = global::UniEye.Base.Properties.Resources.arrow_left_32;
            this.movePrevStepButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.movePrevStepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.movePrevStepButton.Name = "movePrevStepButton";
            this.movePrevStepButton.Size = new System.Drawing.Size(36, 58);
            this.movePrevStepButton.Text = "toolStripButton1";
            this.movePrevStepButton.Click += new System.EventHandler(this.movePrevStepButton_Click);
            // 
            // moveNextStepButton
            // 
            this.moveNextStepButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.moveNextStepButton.Image = global::UniEye.Base.Properties.Resources.arrow_right_32;
            this.moveNextStepButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.moveNextStepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.moveNextStepButton.Name = "moveNextStepButton";
            this.moveNextStepButton.Size = new System.Drawing.Size(36, 58);
            this.moveNextStepButton.Text = "toolStripButton2";
            this.moveNextStepButton.Click += new System.EventHandler(this.moveNextStepButton_Click);
            // 
            // addStepButton
            // 
            this.addStepButton.Image = global::UniEye.Base.Properties.Resources.add_32;
            this.addStepButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.addStepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addStepButton.Name = "addStepButton";
            this.addStepButton.Size = new System.Drawing.Size(45, 58);
            this.addStepButton.Text = "Add";
            this.addStepButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.addStepButton.Click += new System.EventHandler(this.addStepButton_Click);
            // 
            // deleteStepButton
            // 
            this.deleteStepButton.Image = global::UniEye.Base.Properties.Resources.delete_32;
            this.deleteStepButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.deleteStepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteStepButton.Name = "deleteStepButton";
            this.deleteStepButton.Size = new System.Drawing.Size(62, 58);
            this.deleteStepButton.Text = "Delete";
            this.deleteStepButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.deleteStepButton.Click += new System.EventHandler(this.deleteStepButton_Click);
            // 
            // editStepButton
            // 
            this.editStepButton.Image = global::UniEye.Base.Properties.Resources.edit_32;
            this.editStepButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.editStepButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editStepButton.Name = "editStepButton";
            this.editStepButton.Size = new System.Drawing.Size(42, 58);
            this.editStepButton.Text = "Edit";
            this.editStepButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.editStepButton.Click += new System.EventHandler(this.editStepButton_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 61);
            // 
            // selectCameraButton
            // 
            this.selectCameraButton.AutoSize = false;
            this.selectCameraButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.selectCameraButton.Image = ((System.Drawing.Image)(resources.GetObject("selectCameraButton.Image")));
            this.selectCameraButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectCameraButton.Name = "selectCameraButton";
            this.selectCameraButton.Size = new System.Drawing.Size(100, 58);
            this.selectCameraButton.Text = "Camera";
            // 
            // loadImageSetToolStripButton
            // 
            this.loadImageSetToolStripButton.Image = global::UniEye.Base.Properties.Resources.picture_folder_32;
            this.loadImageSetToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.loadImageSetToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadImageSetToolStripButton.Name = "loadImageSetToolStripButton";
            this.loadImageSetToolStripButton.Size = new System.Drawing.Size(59, 58);
            this.loadImageSetToolStripButton.Text = "Image";
            this.loadImageSetToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.loadImageSetToolStripButton.ToolTipText = "Select Image Folder";
            this.loadImageSetToolStripButton.Click += new System.EventHandler(this.loadImageSetToolStripButton_Click);
            // 
            // selectPrevImageSetToolStripButton
            // 
            this.selectPrevImageSetToolStripButton.Image = global::UniEye.Base.Properties.Resources.arrow_left_32;
            this.selectPrevImageSetToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.selectPrevImageSetToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectPrevImageSetToolStripButton.Name = "selectPrevImageSetToolStripButton";
            this.selectPrevImageSetToolStripButton.Size = new System.Drawing.Size(76, 58);
            this.selectPrevImageSetToolStripButton.Text = "Prev Set";
            this.selectPrevImageSetToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.selectPrevImageSetToolStripButton.ToolTipText = "Select Image Folder";
            this.selectPrevImageSetToolStripButton.Click += new System.EventHandler(this.selectPrevImageSetToolStripButton_Click);
            // 
            // selectNextImageSetToolStripButton
            // 
            this.selectNextImageSetToolStripButton.Image = global::UniEye.Base.Properties.Resources.arrow_right_32;
            this.selectNextImageSetToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.selectNextImageSetToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectNextImageSetToolStripButton.Name = "selectNextImageSetToolStripButton";
            this.selectNextImageSetToolStripButton.Size = new System.Drawing.Size(78, 58);
            this.selectNextImageSetToolStripButton.Text = "Next Set";
            this.selectNextImageSetToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.selectNextImageSetToolStripButton.ToolTipText = "Select Image Folder";
            this.selectNextImageSetToolStripButton.Click += new System.EventHandler(this.selectNextImageSetToolStripButton_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 61);
            // 
            // selectLightButton
            // 
            this.selectLightButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.selectLightButton.Image = ((System.Drawing.Image)(resources.GetObject("selectLightButton.Image")));
            this.selectLightButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.selectLightButton.Name = "selectLightButton";
            this.selectLightButton.Size = new System.Drawing.Size(102, 58);
            this.selectLightButton.Text = "Light Type";
            this.selectLightButton.DropDownOpening += new System.EventHandler(this.selectLightButton_DropDownOpening);
            // 
            // showLightPanelToolStripButton
            // 
            this.showLightPanelToolStripButton.Image = global::UniEye.Base.Properties.Resources.light_32;
            this.showLightPanelToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.showLightPanelToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.showLightPanelToolStripButton.Name = "showLightPanelToolStripButton";
            this.showLightPanelToolStripButton.Size = new System.Drawing.Size(51, 58);
            this.showLightPanelToolStripButton.Text = "Light";
            this.showLightPanelToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.showLightPanelToolStripButton.ToolTipText = "Show Light Panel";
            this.showLightPanelToolStripButton.Click += new System.EventHandler(this.showLightPanelToolStripButton_Click);
            // 
            // grabProcessToolStripButton
            // 
            this.grabProcessToolStripButton.Image = global::UniEye.Base.Properties.Resources.process_shot_32;
            this.grabProcessToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.grabProcessToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.grabProcessToolStripButton.Name = "grabProcessToolStripButton";
            this.grabProcessToolStripButton.Size = new System.Drawing.Size(49, 58);
            this.grabProcessToolStripButton.Text = "Grab";
            this.grabProcessToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.grabProcessToolStripButton.ToolTipText = "Single Grab";
            this.grabProcessToolStripButton.Click += new System.EventHandler(this.grabProcessToolStripButton_Click);
            // 
            // singleShotToolStripButton
            // 
            this.singleShotToolStripButton.Image = global::UniEye.Base.Properties.Resources.single_shot_32;
            this.singleShotToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.singleShotToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.singleShotToolStripButton.Name = "singleShotToolStripButton";
            this.singleShotToolStripButton.Size = new System.Drawing.Size(99, 58);
            this.singleShotToolStripButton.Text = "Single Shot";
            this.singleShotToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.singleShotToolStripButton.ToolTipText = "Select Image Folder";
            this.singleShotToolStripButton.Click += new System.EventHandler(this.singleShotToolStripButton_Click);
            // 
            // multiShotToolStripButton
            // 
            this.multiShotToolStripButton.Image = global::UniEye.Base.Properties.Resources.multi_shot_32;
            this.multiShotToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.multiShotToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.multiShotToolStripButton.Name = "multiShotToolStripButton";
            this.multiShotToolStripButton.Size = new System.Drawing.Size(92, 58);
            this.multiShotToolStripButton.Text = "Multi Shot";
            this.multiShotToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.multiShotToolStripButton.ToolTipText = "Select Image Folder";
            this.multiShotToolStripButton.Click += new System.EventHandler(this.multiShotToolStripButton_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 61);
            // 
            // calibration3dButton
            // 
            this.calibration3dButton.Image = global::UniEye.Base.Properties.Resources.dot_grid_32;
            this.calibration3dButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.calibration3dButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.calibration3dButton.Name = "calibration3dButton";
            this.calibration3dButton.Size = new System.Drawing.Size(76, 58);
            this.calibration3dButton.Text = "Calib 3D";
            this.calibration3dButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButtonAlign
            // 
            this.toolStripButtonAlign.Image = global::UniEye.Base.Properties.Resources.gun_sight_32;
            this.toolStripButtonAlign.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonAlign.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAlign.Name = "toolStripButtonAlign";
            this.toolStripButtonAlign.Size = new System.Drawing.Size(52, 58);
            this.toolStripButtonAlign.Text = "Align";
            this.toolStripButtonAlign.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonAlign.ToolTipText = "Align";
            this.toolStripButtonAlign.Click += new System.EventHandler(this.toolStripButtonAlign_Click);
            // 
            // tabPageProbe
            // 
            this.tabPageProbe.Controls.Add(this.toolStripProbe);
            this.tabPageProbe.Location = new System.Drawing.Point(4, 29);
            this.tabPageProbe.Name = "tabPageProbe";
            this.tabPageProbe.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProbe.Size = new System.Drawing.Size(1272, 65);
            this.tabPageProbe.TabIndex = 4;
            this.tabPageProbe.Text = "Probe";
            this.tabPageProbe.UseVisualStyleBackColor = true;
            // 
            // toolStripProbe
            // 
            this.toolStripProbe.AutoSize = false;
            this.toolStripProbe.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripProbe.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripProbe.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addProbeToolStripButton,
            this.copyProbeToolStripButton,
            this.pasteProbeToolStripButton,
            this.deleteProbeToolStripButton,
            this.toolStripSeparator3,
            this.groupProbeToolStripButton,
            this.ungroupProbeToolStripButton,
            this.toolStripSeparatorGroup,
            this.setFiducialToolStripButton,
            this.setTargetCalibrationToolStripButton,
            this.toolStripSeparatorCalibration,
            this.syncParamToolStripButton,
            this.syncAllToolStripButton,
            this.dontMoveToolStripButton,
            this.toolStripSeparator10,
            this.addFiducialToolStripButton,
            this.deleteFiducialToolStripButton,
            this.toggleFiducialToolStripButton,
            this.toolStripSeparatorFiducial,
            this.undoToolStripButton,
            this.RedoToolStripButton});
            this.toolStripProbe.Location = new System.Drawing.Point(3, 3);
            this.toolStripProbe.Name = "toolStripProbe";
            this.toolStripProbe.Size = new System.Drawing.Size(1266, 60);
            this.toolStripProbe.TabIndex = 2;
            this.toolStripProbe.Text = "toolStrip1";
            // 
            // addProbeToolStripButton
            // 
            this.addProbeToolStripButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.addProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.add_32;
            this.addProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.addProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addProbeToolStripButton.Name = "addProbeToolStripButton";
            this.addProbeToolStripButton.Size = new System.Drawing.Size(54, 57);
            this.addProbeToolStripButton.Text = "Add";
            this.addProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.AutoSize = false;
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.testToolStripMenuItem.Text = "Testfgdfgdfgdfgdfgdfg";
            // 
            // copyProbeToolStripButton
            // 
            this.copyProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.copy_32;
            this.copyProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.copyProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copyProbeToolStripButton.Name = "copyProbeToolStripButton";
            this.copyProbeToolStripButton.Size = new System.Drawing.Size(52, 57);
            this.copyProbeToolStripButton.Text = "Copy";
            this.copyProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.copyProbeToolStripButton.Click += new System.EventHandler(this.copyProbeToolStripButton_Click);
            // 
            // pasteProbeToolStripButton
            // 
            this.pasteProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.paste_32;
            this.pasteProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.pasteProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.pasteProbeToolStripButton.Name = "pasteProbeToolStripButton";
            this.pasteProbeToolStripButton.Size = new System.Drawing.Size(53, 57);
            this.pasteProbeToolStripButton.Text = "Paste";
            this.pasteProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.pasteProbeToolStripButton.Click += new System.EventHandler(this.pasteProbeToolStripButton_Click);
            // 
            // deleteProbeToolStripButton
            // 
            this.deleteProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.delete_32;
            this.deleteProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.deleteProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteProbeToolStripButton.Name = "deleteProbeToolStripButton";
            this.deleteProbeToolStripButton.Size = new System.Drawing.Size(62, 57);
            this.deleteProbeToolStripButton.Text = "Delete";
            this.deleteProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.deleteProbeToolStripButton.Click += new System.EventHandler(this.deleteProbeToolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 60);
            // 
            // groupProbeToolStripButton
            // 
            this.groupProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.group_32;
            this.groupProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.groupProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.groupProbeToolStripButton.Name = "groupProbeToolStripButton";
            this.groupProbeToolStripButton.Size = new System.Drawing.Size(60, 57);
            this.groupProbeToolStripButton.Text = "Group";
            this.groupProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.groupProbeToolStripButton.Click += new System.EventHandler(this.groupProbeToolStripButton_Click);
            // 
            // ungroupProbeToolStripButton
            // 
            this.ungroupProbeToolStripButton.Image = global::UniEye.Base.Properties.Resources.ungroup_32;
            this.ungroupProbeToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ungroupProbeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ungroupProbeToolStripButton.Name = "ungroupProbeToolStripButton";
            this.ungroupProbeToolStripButton.Size = new System.Drawing.Size(79, 57);
            this.ungroupProbeToolStripButton.Text = "Ungroup";
            this.ungroupProbeToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ungroupProbeToolStripButton.Click += new System.EventHandler(this.ungroupProbeToolStripButton_Click);
            // 
            // toolStripSeparatorGroup
            // 
            this.toolStripSeparatorGroup.Name = "toolStripSeparatorGroup";
            this.toolStripSeparatorGroup.Size = new System.Drawing.Size(6, 60);
            // 
            // setFiducialToolStripButton
            // 
            this.setFiducialToolStripButton.Image = global::UniEye.Base.Properties.Resources.gun_sight_32;
            this.setFiducialToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.setFiducialToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.setFiducialToolStripButton.Name = "setFiducialToolStripButton";
            this.setFiducialToolStripButton.Size = new System.Drawing.Size(69, 57);
            this.setFiducialToolStripButton.Text = "Fiducial";
            this.setFiducialToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.setFiducialToolStripButton.Click += new System.EventHandler(this.setFiducialToolStripButton_Click);
            // 
            // setTargetCalibrationToolStripButton
            // 
            this.setTargetCalibrationToolStripButton.Image = global::UniEye.Base.Properties.Resources.dot_grid_32;
            this.setTargetCalibrationToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.setTargetCalibrationToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.setTargetCalibrationToolStripButton.Name = "setTargetCalibrationToolStripButton";
            this.setTargetCalibrationToolStripButton.Size = new System.Drawing.Size(93, 57);
            this.setTargetCalibrationToolStripButton.Text = "Calibration";
            this.setTargetCalibrationToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.setTargetCalibrationToolStripButton.Click += new System.EventHandler(this.setTargetCalibrationToolStripButton_Click);
            // 
            // toolStripSeparatorCalibration
            // 
            this.toolStripSeparatorCalibration.Name = "toolStripSeparatorCalibration";
            this.toolStripSeparatorCalibration.Size = new System.Drawing.Size(6, 60);
            // 
            // syncParamToolStripButton
            // 
            this.syncParamToolStripButton.Image = global::UniEye.Base.Properties.Resources.sync_32;
            this.syncParamToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.syncParamToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.syncParamToolStripButton.Name = "syncParamToolStripButton";
            this.syncParamToolStripButton.Size = new System.Drawing.Size(48, 57);
            this.syncParamToolStripButton.Text = "Sync";
            this.syncParamToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.syncParamToolStripButton.Click += new System.EventHandler(this.syncParamToolStripButton_Click);
            // 
            // syncAllToolStripButton
            // 
            this.syncAllToolStripButton.Image = global::UniEye.Base.Properties.Resources.sync_32;
            this.syncAllToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.syncAllToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.syncAllToolStripButton.Name = "syncAllToolStripButton";
            this.syncAllToolStripButton.Size = new System.Drawing.Size(73, 57);
            this.syncAllToolStripButton.Text = "Sync All";
            this.syncAllToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.syncAllToolStripButton.Click += new System.EventHandler(this.syncAllToolStripButton_Click);
            // 
            // dontMoveToolStripButton
            // 
            this.dontMoveToolStripButton.Image = global::UniEye.Base.Properties.Resources.no_entry_32;
            this.dontMoveToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.dontMoveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dontMoveToolStripButton.Name = "dontMoveToolStripButton";
            this.dontMoveToolStripButton.Size = new System.Drawing.Size(102, 57);
            this.dontMoveToolStripButton.Text = "Don\'t Move";
            this.dontMoveToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.dontMoveToolStripButton.ToolTipText = "Don\'t Move";
            this.dontMoveToolStripButton.Click += new System.EventHandler(this.dontMoveToolStripButton_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 60);
            // 
            // addFiducialToolStripButton
            // 
            this.addFiducialToolStripButton.Image = global::UniEye.Base.Properties.Resources.add_32;
            this.addFiducialToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.addFiducialToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addFiducialToolStripButton.Name = "addFiducialToolStripButton";
            this.addFiducialToolStripButton.Size = new System.Drawing.Size(73, 57);
            this.addFiducialToolStripButton.Text = "Add Fid";
            this.addFiducialToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.addFiducialToolStripButton.ToolTipText = "Add Fidutial";
            this.addFiducialToolStripButton.Click += new System.EventHandler(this.addFiducialToolStripButton_Click);
            // 
            // deleteFiducialToolStripButton
            // 
            this.deleteFiducialToolStripButton.Image = global::UniEye.Base.Properties.Resources.delete_fiducial_32;
            this.deleteFiducialToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.deleteFiducialToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteFiducialToolStripButton.Name = "deleteFiducialToolStripButton";
            this.deleteFiducialToolStripButton.Size = new System.Drawing.Size(66, 57);
            this.deleteFiducialToolStripButton.Text = "Del Fid";
            this.deleteFiducialToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.deleteFiducialToolStripButton.ToolTipText = "Don\'t Move";
            this.deleteFiducialToolStripButton.Click += new System.EventHandler(this.deleteFiducialToolStripButtonToolStripButton_Click);
            // 
            // toggleFiducialToolStripButton
            // 
            this.toggleFiducialToolStripButton.Image = global::UniEye.Base.Properties.Resources.toggle_32;
            this.toggleFiducialToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toggleFiducialToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toggleFiducialToolStripButton.Name = "toggleFiducialToolStripButton";
            this.toggleFiducialToolStripButton.Size = new System.Drawing.Size(94, 57);
            this.toggleFiducialToolStripButton.Text = "Toggle Fid";
            this.toggleFiducialToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toggleFiducialToolStripButton.Click += new System.EventHandler(this.toggleFiducialToolStripButton_Click);
            // 
            // toolStripSeparatorFiducial
            // 
            this.toolStripSeparatorFiducial.Name = "toolStripSeparatorFiducial";
            this.toolStripSeparatorFiducial.Size = new System.Drawing.Size(6, 60);
            // 
            // undoToolStripButton
            // 
            this.undoToolStripButton.Image = global::UniEye.Base.Properties.Resources.undo_32;
            this.undoToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.undoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.undoToolStripButton.Name = "undoToolStripButton";
            this.undoToolStripButton.Size = new System.Drawing.Size(54, 57);
            this.undoToolStripButton.Text = "Undo";
            this.undoToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.undoToolStripButton.ToolTipText = "Undo";
            this.undoToolStripButton.Visible = false;
            this.undoToolStripButton.Click += new System.EventHandler(this.undoToolStripButton_Click);
            // 
            // RedoToolStripButton
            // 
            this.RedoToolStripButton.Image = global::UniEye.Base.Properties.Resources.redo_32;
            this.RedoToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.RedoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RedoToolStripButton.Name = "RedoToolStripButton";
            this.RedoToolStripButton.Size = new System.Drawing.Size(53, 57);
            this.RedoToolStripButton.Text = "Redo";
            this.RedoToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.RedoToolStripButton.Visible = false;
            this.RedoToolStripButton.Click += new System.EventHandler(this.RedoToolStripButton_Click);
            // 
            // tabPageView
            // 
            this.tabPageView.Controls.Add(this.toolStripView);
            this.tabPageView.Location = new System.Drawing.Point(4, 29);
            this.tabPageView.Name = "tabPageView";
            this.tabPageView.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageView.Size = new System.Drawing.Size(1272, 65);
            this.tabPageView.TabIndex = 2;
            this.tabPageView.Text = "View";
            this.tabPageView.UseVisualStyleBackColor = true;
            // 
            // toolStripView
            // 
            this.toolStripView.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripView.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoomInToolStripButton,
            this.zoomOutToolStripButton,
            this.zoomFitToolStripButton,
            this.previewToolStripButton,
            this.previewTypeToolStripButton});
            this.toolStripView.Location = new System.Drawing.Point(3, 3);
            this.toolStripView.Name = "toolStripView";
            this.toolStripView.Size = new System.Drawing.Size(1266, 61);
            this.toolStripView.TabIndex = 1;
            this.toolStripView.Text = "toolStrip1";
            // 
            // zoomInToolStripButton
            // 
            this.zoomInToolStripButton.Image = global::UniEye.Base.Properties.Resources.zoom_in_32;
            this.zoomInToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.zoomInToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomInToolStripButton.Name = "zoomInToolStripButton";
            this.zoomInToolStripButton.Size = new System.Drawing.Size(76, 58);
            this.zoomInToolStripButton.Text = "Zoom In";
            this.zoomInToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.zoomInToolStripButton.Click += new System.EventHandler(this.zoomInToolStripButton_Click);
            // 
            // zoomOutToolStripButton
            // 
            this.zoomOutToolStripButton.Image = global::UniEye.Base.Properties.Resources.zoom_out_32;
            this.zoomOutToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.zoomOutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomOutToolStripButton.Name = "zoomOutToolStripButton";
            this.zoomOutToolStripButton.Size = new System.Drawing.Size(90, 58);
            this.zoomOutToolStripButton.Text = "Zoom Out";
            this.zoomOutToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.zoomOutToolStripButton.Click += new System.EventHandler(this.zoomOutToolStripButton_Click);
            // 
            // zoomFitToolStripButton
            // 
            this.zoomFitToolStripButton.Image = global::UniEye.Base.Properties.Resources.zoom_fit_32;
            this.zoomFitToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.zoomFitToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomFitToolStripButton.Name = "zoomFitToolStripButton";
            this.zoomFitToolStripButton.Size = new System.Drawing.Size(81, 58);
            this.zoomFitToolStripButton.Text = "Zoom Fit";
            this.zoomFitToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.zoomFitToolStripButton.Click += new System.EventHandler(this.zoomFitToolStripButton_Click);
            // 
            // previewToolStripButton
            // 
            this.previewToolStripButton.Image = global::UniEye.Base.Properties.Resources.preview_32;
            this.previewToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.previewToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.previewToolStripButton.Name = "previewToolStripButton";
            this.previewToolStripButton.Size = new System.Drawing.Size(71, 58);
            this.previewToolStripButton.Text = "Preview";
            this.previewToolStripButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.previewToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.previewToolStripButton.Click += new System.EventHandler(this.previewToolStripButton_Click_1);
            // 
            // previewTypeToolStripButton
            // 
            this.previewTypeToolStripButton.AutoSize = false;
            this.previewTypeToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.previewTypeToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("previewTypeToolStripButton.Image")));
            this.previewTypeToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.previewTypeToolStripButton.Name = "previewTypeToolStripButton";
            this.previewTypeToolStripButton.Size = new System.Drawing.Size(100, 58);
            this.previewTypeToolStripButton.Text = "Preview";
            // 
            // tabPageRobot
            // 
            this.tabPageRobot.Controls.Add(this.toolStripRobot);
            this.tabPageRobot.Location = new System.Drawing.Point(4, 29);
            this.tabPageRobot.Name = "tabPageRobot";
            this.tabPageRobot.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRobot.Size = new System.Drawing.Size(1272, 65);
            this.tabPageRobot.TabIndex = 6;
            this.tabPageRobot.Text = "Robot";
            this.tabPageRobot.UseVisualStyleBackColor = true;
            // 
            // toolStripRobot
            // 
            this.toolStripRobot.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripRobot.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripRobot.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOrigin,
            this.toolStripButtonJoystick,
            this.toolStripButtonRobotSetting,
            this.toolStripButtonStop,
            this.toolStripButtonFineMove,
            this.toolStripSeparator6});
            this.toolStripRobot.Location = new System.Drawing.Point(3, 3);
            this.toolStripRobot.Name = "toolStripRobot";
            this.toolStripRobot.Size = new System.Drawing.Size(1266, 60);
            this.toolStripRobot.TabIndex = 2;
            this.toolStripRobot.Text = "toolStrip1";
            // 
            // toolStripButtonOrigin
            // 
            this.toolStripButtonOrigin.Image = global::UniEye.Base.Properties.Resources.gun_sight_32;
            this.toolStripButtonOrigin.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonOrigin.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOrigin.Name = "toolStripButtonOrigin";
            this.toolStripButtonOrigin.Size = new System.Drawing.Size(59, 57);
            this.toolStripButtonOrigin.Text = "Origin";
            this.toolStripButtonOrigin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonOrigin.Click += new System.EventHandler(this.toolStripButtonOrigin_Click);
            // 
            // toolStripButtonJoystick
            // 
            this.toolStripButtonJoystick.Image = global::UniEye.Base.Properties.Resources.arrow_all;
            this.toolStripButtonJoystick.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonJoystick.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonJoystick.Name = "toolStripButtonJoystick";
            this.toolStripButtonJoystick.Size = new System.Drawing.Size(71, 57);
            this.toolStripButtonJoystick.Text = "Joystick";
            this.toolStripButtonJoystick.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonJoystick.Click += new System.EventHandler(this.toolStripButtonJoystick_Click);
            // 
            // toolStripButtonRobotSetting
            // 
            this.toolStripButtonRobotSetting.Image = global::UniEye.Base.Properties.Resources.config_32;
            this.toolStripButtonRobotSetting.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonRobotSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRobotSetting.Name = "toolStripButtonRobotSetting";
            this.toolStripButtonRobotSetting.Size = new System.Drawing.Size(67, 57);
            this.toolStripButtonRobotSetting.Text = "Setting";
            this.toolStripButtonRobotSetting.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonRobotSetting.Click += new System.EventHandler(this.toolStripButtonRobotSetting_Click);
            // 
            // toolStripButtonStop
            // 
            this.toolStripButtonStop.Image = global::UniEye.Base.Properties.Resources.stop_32;
            this.toolStripButtonStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStop.Name = "toolStripButtonStop";
            this.toolStripButtonStop.Size = new System.Drawing.Size(49, 57);
            this.toolStripButtonStop.Text = "Stop";
            this.toolStripButtonStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonStop.Click += new System.EventHandler(this.toolStripButtonStop_Click);
            // 
            // toolStripButtonFineMove
            // 
            this.toolStripButtonFineMove.Image = global::UniEye.Base.Properties.Resources.fine_move_32;
            this.toolStripButtonFineMove.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonFineMove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFineMove.Name = "toolStripButtonFineMove";
            this.toolStripButtonFineMove.Size = new System.Drawing.Size(86, 57);
            this.toolStripButtonFineMove.Text = "FineMove";
            this.toolStripButtonFineMove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolStripButtonFineMove.Click += new System.EventHandler(this.toolStripButtonFineMove_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 60);
            // 
            // panelMenu
            // 
            this.panelMenu.Controls.Add(this.tabControlMain);
            this.panelMenu.Controls.Add(this.inspectionButton);
            this.panelMenu.Controls.Add(this.panelSubMenu);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(1455, 98);
            this.panelMenu.TabIndex = 136;
            // 
            // inspectionButton
            // 
            this.inspectionButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.inspectionButton.Image = global::UniEye.Base.Properties.Resources.test_32;
            this.inspectionButton.Location = new System.Drawing.Point(53, 0);
            this.inspectionButton.Name = "inspectionButton";
            this.inspectionButton.Size = new System.Drawing.Size(122, 98);
            this.inspectionButton.TabIndex = 135;
            this.inspectionButton.Text = "Inspection";
            this.inspectionButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.inspectionButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.inspectionButton.UseVisualStyleBackColor = true;
            this.inspectionButton.Click += new System.EventHandler(this.inspectionButton_Click);
            // 
            // panelSubMenu
            // 
            this.panelSubMenu.ColumnCount = 1;
            this.panelSubMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelSubMenu.Controls.Add(this.saveButton, 0, 0);
            this.panelSubMenu.Controls.Add(this.buttonPreview, 0, 1);
            this.panelSubMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSubMenu.Location = new System.Drawing.Point(0, 0);
            this.panelSubMenu.Name = "panelSubMenu";
            this.panelSubMenu.RowCount = 2;
            this.panelSubMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelSubMenu.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelSubMenu.Size = new System.Drawing.Size(53, 98);
            this.panelSubMenu.TabIndex = 137;
            // 
            // grab3dToolStripButton
            // 
            this.grab3dToolStripButton.Image = global::UniEye.Base.Properties.Resources.cube3d_32;
            this.grab3dToolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.grab3dToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.grab3dToolStripButton.Name = "grab3dToolStripButton";
            this.grab3dToolStripButton.Size = new System.Drawing.Size(69, 58);
            this.grab3dToolStripButton.Text = "Grab3D";
            this.grab3dToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.grab3dToolStripButton.Click += new System.EventHandler(this.grab3dToolStripButton_Click);
            // 
            // ModellerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainContainer);
            this.Controls.Add(this.panelMenu);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ModellerPage";
            this.Size = new System.Drawing.Size(1455, 660);
            this.Load += new System.EventHandler(this.ModellerPage_Load);
            this.VisibleChanged += new System.EventHandler(this.ModellerPage_VisibleChanged);
            this.cameraImagePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainTabView)).EndInit();
            this.mainTabView.ResumeLayout(false);
            this.mainContainer.Panel1.ResumeLayout(false);
            this.mainContainer.Panel1.PerformLayout();
            this.mainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainContainer)).EndInit();
            this.mainContainer.ResumeLayout(false);
            this.paramContainer.Panel1.ResumeLayout(false);
            this.paramContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.paramContainer)).EndInit();
            this.paramContainer.ResumeLayout(false);
            this.panelAlign.ResumeLayout(false);
            this.panelAlign.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDistanceOffset)).EndInit();
            this.tabControlUtil.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageModel.ResumeLayout(false);
            this.tabPageModel.PerformLayout();
            this.toolStripModel.ResumeLayout(false);
            this.toolStripModel.PerformLayout();
            this.tabPageFov.ResumeLayout(false);
            this.toolStripFov.ResumeLayout(false);
            this.toolStripFov.PerformLayout();
            this.tabPageProbe.ResumeLayout(false);
            this.toolStripProbe.ResumeLayout(false);
            this.toolStripProbe.PerformLayout();
            this.tabPageView.ResumeLayout(false);
            this.tabPageView.PerformLayout();
            this.toolStripView.ResumeLayout(false);
            this.toolStripView.PerformLayout();
            this.tabPageRobot.ResumeLayout(false);
            this.tabPageRobot.PerformLayout();
            this.toolStripRobot.ResumeLayout(false);
            this.toolStripRobot.PerformLayout();
            this.panelMenu.ResumeLayout(false);
            this.panelSubMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }        
        #endregion

        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.SplitContainer mainContainer;
        private System.Windows.Forms.Panel cameraImagePanel;
        private System.Windows.Forms.SplitContainer paramContainer;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPageFov;
        private System.Windows.Forms.ToolStrip toolStripFov;
        private System.Windows.Forms.ToolStripDropDownButton selectCameraButton;
        private System.Windows.Forms.ToolStripButton grabProcessToolStripButton;
        private System.Windows.Forms.ToolStripButton showLightPanelToolStripButton;
        private System.Windows.Forms.ToolStripButton loadImageSetToolStripButton;
        private System.Windows.Forms.ToolStripButton selectPrevImageSetToolStripButton;
        private System.Windows.Forms.ToolStripButton selectNextImageSetToolStripButton;
        private System.Windows.Forms.ToolStripButton singleShotToolStripButton;
        private System.Windows.Forms.ToolStripButton multiShotToolStripButton;
        private System.Windows.Forms.TabPage tabPageView;
        private System.Windows.Forms.ToolStrip toolStripView;
        private System.Windows.Forms.ToolStripButton zoomInToolStripButton;
        private System.Windows.Forms.ToolStripButton zoomOutToolStripButton;
        private System.Windows.Forms.ToolStripButton zoomFitToolStripButton;
        private System.Windows.Forms.ToolStripDropDownButton previewTypeToolStripButton;
        private System.Windows.Forms.Button inspectionButton;
        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.TabPage tabPageProbe;
        private System.Windows.Forms.ToolStrip toolStripProbe;
        private System.Windows.Forms.ToolStripButton copyProbeToolStripButton;
        private System.Windows.Forms.ToolStripButton pasteProbeToolStripButton;
        private System.Windows.Forms.ToolStripButton deleteProbeToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton syncParamToolStripButton;
        private System.Windows.Forms.ToolStripButton syncAllToolStripButton;
        private System.Windows.Forms.ToolStripDropDownButton addProbeToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton grab3dToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton dontMoveToolStripButton;
        private System.Windows.Forms.TabPage tabPageModel;
        private System.Windows.Forms.ToolStrip toolStripModel;
        private System.Windows.Forms.TabPage tabPageRobot;
        private System.Windows.Forms.TabControl tabControlUtil;
        private System.Windows.Forms.TabPage tabPageResult;
        private System.Windows.Forms.ToolStrip toolStripRobot;
        private System.Windows.Forms.ToolStripButton toolStripButtonOrigin;
        private System.Windows.Forms.ToolStripButton toolStripButtonJoystick;
        private System.Windows.Forms.ToolStripButton toolStripButtonRobotSetting;
        private System.Windows.Forms.ToolStripButton modelPropertyButton;
        private System.Windows.Forms.ToolStripButton addStepButton;
        private System.Windows.Forms.ToolStripButton deleteStepButton;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl mainTabView;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPageCamera;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl ultraTabPage3dViewer;
        private System.Windows.Forms.ToolStripButton calibration3dButton;
        private System.Windows.Forms.ToolStripButton editStepButton;
        private System.Windows.Forms.ToolStripButton toolStripButtonStop;
        private System.Windows.Forms.ToolStripButton movePrevStepButton;
        private System.Windows.Forms.ToolStripButton moveNextStepButton;
        private System.Windows.Forms.ToolStripLabel toolStripLabelStep;
        private System.Windows.Forms.ToolStripComboBox comboStep;
        private System.Windows.Forms.ToolStripButton editSchemaButton;
        private System.Windows.Forms.ToolStripButton toolStripButtonFineMove;
        private System.Windows.Forms.TableLayoutPanel panelSubMenu;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.ToolStripButton previewToolStripButton;
        private System.Windows.Forms.ToolStripButton toolStripButtonAlign;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.Panel panelAlign;
        private System.Windows.Forms.Label labelDesiredDistance;
        private System.Windows.Forms.Label labelFidDistance;
        private System.Windows.Forms.NumericUpDown numericUpDownDistanceOffset;
        private System.Windows.Forms.Label labelDistanceOffset;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Label labelUm;
        private System.Windows.Forms.Button buttonAlign;
        private System.Windows.Forms.TextBox textBoxFidDistance;
        private System.Windows.Forms.TextBox textBoxDesiredDistance;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripDropDownButton selectLightButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton resetModelButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.TextBox fidAngle;
        private System.Windows.Forms.TextBox fidOffset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelFidAngle;
        private System.Windows.Forms.Label labelFidOffset;
        public System.Windows.Forms.ToolStripButton importGerberButton;
        public System.Windows.Forms.ToolStripButton undoToolStripButton;
        public System.Windows.Forms.ToolStripButton RedoToolStripButton;
        public System.Windows.Forms.ToolStripButton exportFormatButton;
        public System.Windows.Forms.ToolStripButton groupProbeToolStripButton;
        public System.Windows.Forms.ToolStripButton ungroupProbeToolStripButton;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparatorGroup;
        public System.Windows.Forms.ToolStripButton setFiducialToolStripButton;
        public System.Windows.Forms.ToolStripButton setTargetCalibrationToolStripButton;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparatorCalibration;
        public System.Windows.Forms.ToolStripButton addFiducialToolStripButton;
        public System.Windows.Forms.ToolStripButton deleteFiducialToolStripButton;
        public System.Windows.Forms.ToolStripButton toggleFiducialToolStripButton;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparatorFiducial;
        private System.Windows.Forms.ToolStripButton scanButton;
    }
}
