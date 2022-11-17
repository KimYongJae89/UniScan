namespace UniScanG.Screen.Settings.Inspector.UI
{
    partial class SettingPage
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
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab2 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinTabControl.UltraTab ultraTab4 = new Infragistics.Win.UltraWinTabControl.UltraTab();
            this.tabPageInspector = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.layoutInspector = new System.Windows.Forms.FlowLayoutPanel();
            this.groupCalibration = new System.Windows.Forms.GroupBox();
            this.buttonCalibration = new System.Windows.Forms.Button();
            this.cameraResolution = new System.Windows.Forms.Label();
            this.labelCalibration = new System.Windows.Forms.Label();
            this.labelStartYPosition = new System.Windows.Forms.Label();
            this.startYPosition = new System.Windows.Forms.NumericUpDown();
            this.labelStartYPositionUnit = new System.Windows.Forms.Label();
            this.labelSheetHeightUnit = new System.Windows.Forms.Label();
            this.sheetHeight = new System.Windows.Forms.NumericUpDown();
            this.labelStartXPosition = new System.Windows.Forms.Label();
            this.labelFovXUnit = new System.Windows.Forms.Label();
            this.startXPosition = new System.Windows.Forms.NumericUpDown();
            this.fovX = new System.Windows.Forms.NumericUpDown();
            this.labelStartXPositionUnit = new System.Windows.Forms.Label();
            this.labelFovX = new System.Windows.Forms.Label();
            this.labelSheetHeight = new System.Windows.Forms.Label();
            this.tabPageDeveloper = new Infragistics.Win.UltraWinTabControl.UltraTabPageControl();
            this.layoutDeveloper = new System.Windows.Forms.FlowLayoutPanel();
            this.imageCheckInterval = new System.Windows.Forms.NumericUpDown();
            this.labelImageCheckInterval = new System.Windows.Forms.Label();
            this.labelSec = new System.Windows.Forms.Label();
            this.labelMs = new System.Windows.Forms.Label();
            this.settingTabControl = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.tabPageInspector.SuspendLayout();
            this.layoutInspector.SuspendLayout();
            this.groupCalibration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startYPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sheetHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startXPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fovX)).BeginInit();
            this.tabPageDeveloper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageCheckInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingTabControl)).BeginInit();
            this.settingTabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageInspector
            // 
            this.tabPageInspector.Controls.Add(this.layoutInspector);
            this.tabPageInspector.Location = new System.Drawing.Point(121, 0);
            this.tabPageInspector.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageInspector.Name = "tabPageInspector";
            this.tabPageInspector.Padding = new System.Windows.Forms.Padding(10);
            this.tabPageInspector.Size = new System.Drawing.Size(1096, 1637);
            // 
            // layoutInspector
            // 
            this.layoutInspector.Controls.Add(this.groupCalibration);
            this.layoutInspector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutInspector.Location = new System.Drawing.Point(10, 10);
            this.layoutInspector.Margin = new System.Windows.Forms.Padding(0);
            this.layoutInspector.Name = "layoutInspector";
            this.layoutInspector.Size = new System.Drawing.Size(1076, 1617);
            this.layoutInspector.TabIndex = 2;
            // 
            // groupCalibration
            // 
            this.groupCalibration.BackColor = System.Drawing.Color.Transparent;
            this.groupCalibration.Controls.Add(this.buttonCalibration);
            this.groupCalibration.Controls.Add(this.cameraResolution);
            this.groupCalibration.Controls.Add(this.labelCalibration);
            this.groupCalibration.Controls.Add(this.labelStartYPosition);
            this.groupCalibration.Controls.Add(this.startYPosition);
            this.groupCalibration.Controls.Add(this.labelStartYPositionUnit);
            this.groupCalibration.Controls.Add(this.labelSheetHeightUnit);
            this.groupCalibration.Controls.Add(this.sheetHeight);
            this.groupCalibration.Controls.Add(this.labelStartXPosition);
            this.groupCalibration.Controls.Add(this.labelFovXUnit);
            this.groupCalibration.Controls.Add(this.startXPosition);
            this.groupCalibration.Controls.Add(this.fovX);
            this.groupCalibration.Controls.Add(this.labelStartXPositionUnit);
            this.groupCalibration.Controls.Add(this.labelFovX);
            this.groupCalibration.Controls.Add(this.labelSheetHeight);
            this.groupCalibration.Location = new System.Drawing.Point(0, 0);
            this.groupCalibration.Margin = new System.Windows.Forms.Padding(0);
            this.groupCalibration.Name = "groupCalibration";
            this.groupCalibration.Padding = new System.Windows.Forms.Padding(0);
            this.groupCalibration.Size = new System.Drawing.Size(414, 275);
            this.groupCalibration.TabIndex = 0;
            this.groupCalibration.TabStop = false;
            this.groupCalibration.Text = "Calibration";
            // 
            // buttonCalibration
            // 
            this.buttonCalibration.Location = new System.Drawing.Point(261, 213);
            this.buttonCalibration.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonCalibration.Name = "buttonCalibration";
            this.buttonCalibration.Size = new System.Drawing.Size(132, 45);
            this.buttonCalibration.TabIndex = 1;
            this.buttonCalibration.Text = "Start";
            this.buttonCalibration.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonCalibration.UseVisualStyleBackColor = true;
            // 
            // cameraResolution
            // 
            this.cameraResolution.AutoSize = true;
            this.cameraResolution.Location = new System.Drawing.Point(262, 182);
            this.cameraResolution.Margin = new System.Windows.Forms.Padding(0);
            this.cameraResolution.Name = "cameraResolution";
            this.cameraResolution.Size = new System.Drawing.Size(131, 25);
            this.cameraResolution.TabIndex = 2;
            this.cameraResolution.Text = "00.00 [um/px]";
            // 
            // labelCalibration
            // 
            this.labelCalibration.AutoSize = true;
            this.labelCalibration.Location = new System.Drawing.Point(19, 182);
            this.labelCalibration.Margin = new System.Windows.Forms.Padding(0);
            this.labelCalibration.Name = "labelCalibration";
            this.labelCalibration.Size = new System.Drawing.Size(152, 25);
            this.labelCalibration.TabIndex = 3;
            this.labelCalibration.Text = "Cam Calibration";
            // 
            // labelStartYPosition
            // 
            this.labelStartYPosition.AutoSize = true;
            this.labelStartYPosition.Location = new System.Drawing.Point(19, 67);
            this.labelStartYPosition.Margin = new System.Windows.Forms.Padding(0);
            this.labelStartYPosition.Name = "labelStartYPosition";
            this.labelStartYPosition.Size = new System.Drawing.Size(147, 25);
            this.labelStartYPosition.TabIndex = 0;
            this.labelStartYPosition.Text = "Start Y Position";
            // 
            // startYPosition
            // 
            this.startYPosition.Location = new System.Drawing.Point(287, 67);
            this.startYPosition.Margin = new System.Windows.Forms.Padding(0);
            this.startYPosition.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.startYPosition.Name = "startYPosition";
            this.startYPosition.Size = new System.Drawing.Size(54, 32);
            this.startYPosition.TabIndex = 0;
            this.startYPosition.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // labelStartYPositionUnit
            // 
            this.labelStartYPositionUnit.AutoSize = true;
            this.labelStartYPositionUnit.Location = new System.Drawing.Point(347, 67);
            this.labelStartYPositionUnit.Name = "labelStartYPositionUnit";
            this.labelStartYPositionUnit.Size = new System.Drawing.Size(46, 25);
            this.labelStartYPositionUnit.TabIndex = 0;
            this.labelStartYPositionUnit.Text = "mm";
            // 
            // labelSheetHeightUnit
            // 
            this.labelSheetHeightUnit.AutoSize = true;
            this.labelSheetHeightUnit.Location = new System.Drawing.Point(347, 139);
            this.labelSheetHeightUnit.Name = "labelSheetHeightUnit";
            this.labelSheetHeightUnit.Size = new System.Drawing.Size(46, 25);
            this.labelSheetHeightUnit.TabIndex = 0;
            this.labelSheetHeightUnit.Text = "mm";
            // 
            // sheetHeight
            // 
            this.sheetHeight.Location = new System.Drawing.Point(287, 139);
            this.sheetHeight.Margin = new System.Windows.Forms.Padding(0);
            this.sheetHeight.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.sheetHeight.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.sheetHeight.Name = "sheetHeight";
            this.sheetHeight.Size = new System.Drawing.Size(54, 32);
            this.sheetHeight.TabIndex = 0;
            this.sheetHeight.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // labelStartXPosition
            // 
            this.labelStartXPosition.AutoSize = true;
            this.labelStartXPosition.Location = new System.Drawing.Point(19, 31);
            this.labelStartXPosition.Margin = new System.Windows.Forms.Padding(0);
            this.labelStartXPosition.Name = "labelStartXPosition";
            this.labelStartXPosition.Size = new System.Drawing.Size(147, 25);
            this.labelStartXPosition.TabIndex = 0;
            this.labelStartXPosition.Text = "Start X Position";
            // 
            // labelFovXUnit
            // 
            this.labelFovXUnit.AutoSize = true;
            this.labelFovXUnit.Location = new System.Drawing.Point(347, 103);
            this.labelFovXUnit.Name = "labelFovXUnit";
            this.labelFovXUnit.Size = new System.Drawing.Size(46, 25);
            this.labelFovXUnit.TabIndex = 0;
            this.labelFovXUnit.Text = "mm";
            // 
            // startXPosition
            // 
            this.startXPosition.Location = new System.Drawing.Point(287, 31);
            this.startXPosition.Margin = new System.Windows.Forms.Padding(0);
            this.startXPosition.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.startXPosition.Name = "startXPosition";
            this.startXPosition.Size = new System.Drawing.Size(54, 32);
            this.startXPosition.TabIndex = 0;
            this.startXPosition.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // fovX
            // 
            this.fovX.Location = new System.Drawing.Point(287, 103);
            this.fovX.Margin = new System.Windows.Forms.Padding(0);
            this.fovX.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.fovX.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.fovX.Name = "fovX";
            this.fovX.Size = new System.Drawing.Size(54, 32);
            this.fovX.TabIndex = 0;
            this.fovX.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // labelStartXPositionUnit
            // 
            this.labelStartXPositionUnit.AutoSize = true;
            this.labelStartXPositionUnit.Location = new System.Drawing.Point(347, 31);
            this.labelStartXPositionUnit.Name = "labelStartXPositionUnit";
            this.labelStartXPositionUnit.Size = new System.Drawing.Size(46, 25);
            this.labelStartXPositionUnit.TabIndex = 0;
            this.labelStartXPositionUnit.Text = "mm";
            // 
            // labelFovX
            // 
            this.labelFovX.AutoSize = true;
            this.labelFovX.Location = new System.Drawing.Point(19, 103);
            this.labelFovX.Margin = new System.Windows.Forms.Padding(0);
            this.labelFovX.Name = "labelFovX";
            this.labelFovX.Size = new System.Drawing.Size(66, 25);
            this.labelFovX.TabIndex = 0;
            this.labelFovX.Text = "FOV X";
            // 
            // labelSheetHeight
            // 
            this.labelSheetHeight.AutoSize = true;
            this.labelSheetHeight.Location = new System.Drawing.Point(19, 139);
            this.labelSheetHeight.Margin = new System.Windows.Forms.Padding(0);
            this.labelSheetHeight.Name = "labelSheetHeight";
            this.labelSheetHeight.Size = new System.Drawing.Size(125, 25);
            this.labelSheetHeight.TabIndex = 0;
            this.labelSheetHeight.Text = "Sheet Height";
            // 
            // tabPageDeveloper
            // 
            this.tabPageDeveloper.Controls.Add(this.layoutDeveloper);
            this.tabPageDeveloper.Location = new System.Drawing.Point(-10000, -10000);
            this.tabPageDeveloper.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageDeveloper.Name = "tabPageDeveloper";
            this.tabPageDeveloper.Padding = new System.Windows.Forms.Padding(10);
            this.tabPageDeveloper.Size = new System.Drawing.Size(1096, 1637);
            // 
            // layoutDeveloper
            // 
            this.layoutDeveloper.BackColor = System.Drawing.Color.White;
            this.layoutDeveloper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutDeveloper.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.layoutDeveloper.Location = new System.Drawing.Point(10, 10);
            this.layoutDeveloper.Margin = new System.Windows.Forms.Padding(0);
            this.layoutDeveloper.Name = "layoutDeveloper";
            this.layoutDeveloper.Size = new System.Drawing.Size(1076, 1617);
            this.layoutDeveloper.TabIndex = 1;
            // 
            // imageCheckInterval
            // 
            this.imageCheckInterval.Location = new System.Drawing.Point(211, 119);
            this.imageCheckInterval.Name = "imageCheckInterval";
            this.imageCheckInterval.Size = new System.Drawing.Size(68, 21);
            this.imageCheckInterval.TabIndex = 0;
            // 
            // labelImageCheckInterval
            // 
            this.labelImageCheckInterval.AutoSize = true;
            this.labelImageCheckInterval.Location = new System.Drawing.Point(15, 123);
            this.labelImageCheckInterval.Name = "labelImageCheckInterval";
            this.labelImageCheckInterval.Size = new System.Drawing.Size(151, 17);
            this.labelImageCheckInterval.TabIndex = 0;
            this.labelImageCheckInterval.Text = "Image Check Interval";
            // 
            // labelSec
            // 
            this.labelSec.AutoSize = true;
            this.labelSec.Location = new System.Drawing.Point(286, 126);
            this.labelSec.Name = "labelSec";
            this.labelSec.Size = new System.Drawing.Size(29, 17);
            this.labelSec.TabIndex = 0;
            this.labelSec.Text = "sec";
            // 
            // labelMs
            // 
            this.labelMs.Location = new System.Drawing.Point(0, 0);
            this.labelMs.Name = "labelMs";
            this.labelMs.Size = new System.Drawing.Size(100, 23);
            this.labelMs.TabIndex = 0;
            // 
            // settingTabControl
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            this.settingTabControl.ClientAreaAppearance = appearance1;
            this.settingTabControl.Controls.Add(this.ultraTabSharedControlsPage1);
            this.settingTabControl.Controls.Add(this.tabPageInspector);
            this.settingTabControl.Controls.Add(this.tabPageDeveloper);
            this.settingTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingTabControl.Font = new System.Drawing.Font("Malgun Gothic", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.settingTabControl.InterRowSpacing = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.InterTabSpacing = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.Location = new System.Drawing.Point(0, 0);
            this.settingTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.settingTabControl.Name = "settingTabControl";
            this.settingTabControl.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.settingTabControl.Size = new System.Drawing.Size(1217, 1637);
            this.settingTabControl.SpaceAfterTabs = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.SpaceBeforeTabs = new Infragistics.Win.DefaultableInteger(0);
            this.settingTabControl.Style = Infragistics.Win.UltraWinTabControl.UltraTabControlStyle.StateButtons;
            appearance2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(226)))), ((int)(((byte)(241)))));
            appearance2.FontData.BoldAsString = "True";
            appearance2.FontData.Name = "맑은 고딕";
            this.settingTabControl.TabHeaderAreaAppearance = appearance2;
            this.settingTabControl.TabIndex = 0;
            this.settingTabControl.TabOrientation = Infragistics.Win.UltraWinTabs.TabOrientation.LeftTop;
            this.settingTabControl.TabPadding = new System.Drawing.Size(10, 10);
            appearance3.BackColor = System.Drawing.Color.Transparent;
            ultraTab2.Appearance = appearance3;
            ultraTab2.Key = "Inspector";
            ultraTab2.TabPage = this.tabPageInspector;
            ultraTab2.Text = "Inspector";
            ultraTab4.Key = "Developer";
            ultraTab4.TabPage = this.tabPageDeveloper;
            ultraTab4.Text = "Developer";
            ultraTab4.Visible = false;
            this.settingTabControl.Tabs.AddRange(new Infragistics.Win.UltraWinTabControl.UltraTab[] {
            ultraTab2,
            ultraTab4});
            this.settingTabControl.TextOrientation = Infragistics.Win.UltraWinTabs.TextOrientation.Horizontal;
            this.settingTabControl.SelectedTabChanged += new Infragistics.Win.UltraWinTabControl.SelectedTabChangedEventHandler(this.tabControlParam_SelectedTabChanged);
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(-10000, -10000);
            this.ultraTabSharedControlsPage1.Margin = new System.Windows.Forms.Padding(0);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(1096, 1637);
            // 
            // SettingPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.settingTabControl);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SettingPage";
            this.Size = new System.Drawing.Size(1217, 1637);
            this.tabPageInspector.ResumeLayout(false);
            this.layoutInspector.ResumeLayout(false);
            this.groupCalibration.ResumeLayout(false);
            this.groupCalibration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startYPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sheetHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startXPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fovX)).EndInit();
            this.tabPageDeveloper.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageCheckInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.settingTabControl)).EndInit();
            this.settingTabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.NumericUpDown imageCheckInterval;
        private System.Windows.Forms.Label labelImageCheckInterval;
        private System.Windows.Forms.Label labelSec;
        private System.Windows.Forms.Label labelMs;
        private Infragistics.Win.UltraWinTabControl.UltraTabControl settingTabControl;
        private Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageInspector;
        private Infragistics.Win.UltraWinTabControl.UltraTabPageControl tabPageDeveloper;
        private System.Windows.Forms.FlowLayoutPanel layoutInspector;
        private System.Windows.Forms.GroupBox groupCalibration;
        private System.Windows.Forms.Label labelStartYPosition;
        private System.Windows.Forms.NumericUpDown startYPosition;
        private System.Windows.Forms.Label labelStartYPositionUnit;
        private System.Windows.Forms.Label labelSheetHeightUnit;
        private System.Windows.Forms.NumericUpDown sheetHeight;
        private System.Windows.Forms.Label labelStartXPosition;
        private System.Windows.Forms.Label labelFovXUnit;
        private System.Windows.Forms.NumericUpDown startXPosition;
        private System.Windows.Forms.NumericUpDown fovX;
        private System.Windows.Forms.Label labelStartXPositionUnit;
        private System.Windows.Forms.Label labelFovX;
        private System.Windows.Forms.Label labelSheetHeight;
        private System.Windows.Forms.FlowLayoutPanel layoutDeveloper;
        private System.Windows.Forms.Button buttonCalibration;
        private System.Windows.Forms.Label cameraResolution;
        private System.Windows.Forms.Label labelCalibration;
    }
}
