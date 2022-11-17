namespace UniScanS.Data.UI
{
    partial class AlarmCheckerForm
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.layoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelIOType = new System.Windows.Forms.Label();
            this.layoutButton = new System.Windows.Forms.TableLayoutPanel();
            this.btnApply = new Infragistics.Win.Misc.UltraButton();
            this.btnCancel = new Infragistics.Win.Misc.UltraButton();
            this.labelTitle = new System.Windows.Forms.Label();
            this.alarmType = new System.Windows.Forms.ComboBox();
            this.labelAlarmType = new System.Windows.Forms.Label();
            this.layoutParam = new System.Windows.Forms.TableLayoutPanel();
            this.layoutSamePoint = new System.Windows.Forms.TableLayoutPanel();
            this.labelSamePointShape = new System.Windows.Forms.Label();
            this.labelSamePointNumUnit = new System.Windows.Forms.Label();
            this.samePointNum = new System.Windows.Forms.NumericUpDown();
            this.labelSamePointNum = new System.Windows.Forms.Label();
            this.labelSamePointSheetAttack = new System.Windows.Forms.Label();
            this.labelSamePointPole = new System.Windows.Forms.Label();
            this.labelSamePointDielectric = new System.Windows.Forms.Label();
            this.labelSamePointPinHole = new System.Windows.Forms.Label();
            this.samePointSheetAttack = new System.Windows.Forms.CheckBox();
            this.samePointPole = new System.Windows.Forms.CheckBox();
            this.samePointDielectric = new System.Windows.Forms.CheckBox();
            this.samePointPinHole = new System.Windows.Forms.CheckBox();
            this.samePointShape = new System.Windows.Forms.CheckBox();
            this.layoutRecent = new System.Windows.Forms.TableLayoutPanel();
            this.labelRecentShape = new System.Windows.Forms.Label();
            this.labelRecentRatioUnit = new System.Windows.Forms.Label();
            this.recentRatio = new System.Windows.Forms.NumericUpDown();
            this.labelRecentRatio = new System.Windows.Forms.Label();
            this.labelRecentSheetAttack = new System.Windows.Forms.Label();
            this.labelRecentPole = new System.Windows.Forms.Label();
            this.labelRecentDielectric = new System.Windows.Forms.Label();
            this.labelRecentPinHole = new System.Windows.Forms.Label();
            this.recentSheetAttack = new System.Windows.Forms.CheckBox();
            this.recentPole = new System.Windows.Forms.CheckBox();
            this.recentDielectric = new System.Windows.Forms.CheckBox();
            this.recentPinHole = new System.Windows.Forms.CheckBox();
            this.recentShape = new System.Windows.Forms.CheckBox();
            this.labelRecentNum = new System.Windows.Forms.Label();
            this.recentNum = new System.Windows.Forms.NumericUpDown();
            this.labelRecentNumUnit = new System.Windows.Forms.Label();
            this.layoutCheckPoint = new System.Windows.Forms.TableLayoutPanel();
            this.labelCheckPointIndexUnit = new System.Windows.Forms.Label();
            this.checkPointIndex = new System.Windows.Forms.NumericUpDown();
            this.labelCheckPointIndex = new System.Windows.Forms.Label();
            this.labelCheckPointRatio = new System.Windows.Forms.Label();
            this.checkPointRatio = new System.Windows.Forms.NumericUpDown();
            this.labelCheckPointRatioUnit = new System.Windows.Forms.Label();
            this.radioAlarm = new System.Windows.Forms.RadioButton();
            this.radioNG = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.layoutMain.SuspendLayout();
            this.layoutButton.SuspendLayout();
            this.layoutParam.SuspendLayout();
            this.layoutSamePoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samePointNum)).BeginInit();
            this.layoutRecent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recentRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.recentNum)).BeginInit();
            this.layoutCheckPoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkPointIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkPointRatio)).BeginInit();
            this.SuspendLayout();
            // 
            // ultraFormManager
            // 
            this.ultraFormManager.Form = this;
            appearance3.TextHAlignAsString = "Left";
            this.ultraFormManager.FormStyleSettings.CaptionAreaAppearance = appearance3;
            appearance4.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.Appearance = appearance4;
            appearance5.BackColor = System.Drawing.Color.Transparent;
            appearance5.ForeColor = System.Drawing.Color.White;
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.HotTrackAppearance = appearance5;
            appearance6.BackColor = System.Drawing.Color.Transparent;
            appearance6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(168)))), ((int)(((byte)(12)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.PressedAppearance = appearance6;
            this.ultraFormManager.FormStyleSettings.FormDisplayStyle = Infragistics.Win.UltraWinToolbars.FormDisplayStyle.Standard;
            this.ultraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2013;
            this.ultraFormManager.UseOsThemes = Infragistics.Win.DefaultableBoolean.True;
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Top
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(474, 0);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 823);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(474, 0);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Left
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(0, 823);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(474, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(0, 823);
            // 
            // layoutMain
            // 
            this.layoutMain.AutoSize = true;
            this.layoutMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.layoutMain.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutMain.ColumnCount = 4;
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.layoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.layoutMain.Controls.Add(this.labelIOType, 0, 1);
            this.layoutMain.Controls.Add(this.layoutButton, 0, 4);
            this.layoutMain.Controls.Add(this.labelTitle, 0, 0);
            this.layoutMain.Controls.Add(this.alarmType, 3, 2);
            this.layoutMain.Controls.Add(this.labelAlarmType, 0, 2);
            this.layoutMain.Controls.Add(this.layoutParam, 0, 3);
            this.layoutMain.Controls.Add(this.radioAlarm, 2, 1);
            this.layoutMain.Controls.Add(this.radioNG, 3, 1);
            this.layoutMain.Location = new System.Drawing.Point(0, 0);
            this.layoutMain.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMain.Name = "layoutMain";
            this.layoutMain.RowCount = 5;
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutMain.Size = new System.Drawing.Size(474, 620);
            this.layoutMain.TabIndex = 191;
            // 
            // labelIOType
            // 
            this.labelIOType.AutoSize = true;
            this.labelIOType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIOType.Location = new System.Drawing.Point(1, 42);
            this.labelIOType.Margin = new System.Windows.Forms.Padding(0);
            this.labelIOType.Name = "labelIOType";
            this.labelIOType.Size = new System.Drawing.Size(100, 32);
            this.labelIOType.TabIndex = 196;
            this.labelIOType.Text = "IO";
            this.labelIOType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutButton
            // 
            this.layoutButton.ColumnCount = 5;
            this.layoutMain.SetColumnSpan(this.layoutButton, 4);
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.layoutButton.Controls.Add(this.btnApply, 1, 0);
            this.layoutButton.Controls.Add(this.btnCancel, 3, 0);
            this.layoutButton.Location = new System.Drawing.Point(1, 569);
            this.layoutButton.Margin = new System.Windows.Forms.Padding(0);
            this.layoutButton.Name = "layoutButton";
            this.layoutButton.RowCount = 1;
            this.layoutButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutButton.Size = new System.Drawing.Size(472, 50);
            this.layoutButton.TabIndex = 178;
            // 
            // btnApply
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            this.btnApply.Appearance = appearance1;
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnApply.Location = new System.Drawing.Point(95, 5);
            this.btnApply.Margin = new System.Windows.Forms.Padding(5);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(90, 40);
            this.btnApply.TabIndex = 180;
            this.btnApply.Text = "Apply";
            this.btnApply.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            this.btnCancel.Appearance = appearance2;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Location = new System.Drawing.Point(285, 5);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 40);
            this.btnCancel.TabIndex = 179;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(200)))), ((int)(((byte)(220)))));
            this.layoutMain.SetColumnSpan(this.labelTitle, 4);
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTitle.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTitle.Location = new System.Drawing.Point(1, 1);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(472, 40);
            this.labelTitle.TabIndex = 181;
            this.labelTitle.Text = "Alarm Setting";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // alarmType
            // 
            this.alarmType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alarmType.FormattingEnabled = true;
            this.alarmType.Location = new System.Drawing.Point(341, 75);
            this.alarmType.Margin = new System.Windows.Forms.Padding(0);
            this.alarmType.Name = "alarmType";
            this.alarmType.Size = new System.Drawing.Size(132, 33);
            this.alarmType.TabIndex = 182;
            this.alarmType.SelectedIndexChanged += new System.EventHandler(this.alarmType_SelectedIndexChanged);
            // 
            // labelAlarmType
            // 
            this.labelAlarmType.AutoSize = true;
            this.labelAlarmType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAlarmType.Location = new System.Drawing.Point(1, 75);
            this.labelAlarmType.Margin = new System.Windows.Forms.Padding(0);
            this.labelAlarmType.Name = "labelAlarmType";
            this.labelAlarmType.Size = new System.Drawing.Size(100, 32);
            this.labelAlarmType.TabIndex = 183;
            this.labelAlarmType.Text = "Type";
            this.labelAlarmType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutParam
            // 
            this.layoutParam.AutoSize = true;
            this.layoutParam.ColumnCount = 1;
            this.layoutMain.SetColumnSpan(this.layoutParam, 4);
            this.layoutParam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutParam.Controls.Add(this.layoutSamePoint, 0, 2);
            this.layoutParam.Controls.Add(this.layoutRecent, 0, 1);
            this.layoutParam.Controls.Add(this.layoutCheckPoint, 0, 0);
            this.layoutParam.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.layoutParam.Location = new System.Drawing.Point(1, 108);
            this.layoutParam.Margin = new System.Windows.Forms.Padding(0);
            this.layoutParam.Name = "layoutParam";
            this.layoutParam.Padding = new System.Windows.Forms.Padding(5);
            this.layoutParam.RowCount = 3;
            this.layoutParam.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutParam.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutParam.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutParam.Size = new System.Drawing.Size(472, 460);
            this.layoutParam.TabIndex = 0;
            // 
            // layoutSamePoint
            // 
            this.layoutSamePoint.ColumnCount = 4;
            this.layoutSamePoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.layoutSamePoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutSamePoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.layoutSamePoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutSamePoint.Controls.Add(this.labelSamePointShape, 0, 5);
            this.layoutSamePoint.Controls.Add(this.labelSamePointNumUnit, 3, 0);
            this.layoutSamePoint.Controls.Add(this.samePointNum, 2, 0);
            this.layoutSamePoint.Controls.Add(this.labelSamePointNum, 0, 0);
            this.layoutSamePoint.Controls.Add(this.labelSamePointSheetAttack, 0, 1);
            this.layoutSamePoint.Controls.Add(this.labelSamePointPole, 0, 2);
            this.layoutSamePoint.Controls.Add(this.labelSamePointDielectric, 0, 3);
            this.layoutSamePoint.Controls.Add(this.labelSamePointPinHole, 0, 4);
            this.layoutSamePoint.Controls.Add(this.samePointSheetAttack, 2, 1);
            this.layoutSamePoint.Controls.Add(this.samePointPole, 2, 2);
            this.layoutSamePoint.Controls.Add(this.samePointDielectric, 2, 3);
            this.layoutSamePoint.Controls.Add(this.samePointPinHole, 2, 4);
            this.layoutSamePoint.Controls.Add(this.samePointShape, 2, 5);
            this.layoutSamePoint.Location = new System.Drawing.Point(5, 275);
            this.layoutSamePoint.Margin = new System.Windows.Forms.Padding(0);
            this.layoutSamePoint.Name = "layoutSamePoint";
            this.layoutSamePoint.RowCount = 6;
            this.layoutSamePoint.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.layoutSamePoint.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.layoutSamePoint.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.layoutSamePoint.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.layoutSamePoint.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.layoutSamePoint.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.layoutSamePoint.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutSamePoint.Size = new System.Drawing.Size(462, 180);
            this.layoutSamePoint.TabIndex = 2;
            // 
            // labelSamePointShape
            // 
            this.labelSamePointShape.AutoSize = true;
            this.labelSamePointShape.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSamePointShape.Location = new System.Drawing.Point(0, 145);
            this.labelSamePointShape.Margin = new System.Windows.Forms.Padding(0);
            this.labelSamePointShape.Name = "labelSamePointShape";
            this.labelSamePointShape.Size = new System.Drawing.Size(125, 35);
            this.labelSamePointShape.TabIndex = 193;
            this.labelSamePointShape.Text = "Shape";
            this.labelSamePointShape.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSamePointNumUnit
            // 
            this.labelSamePointNumUnit.AutoSize = true;
            this.labelSamePointNumUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSamePointNumUnit.Location = new System.Drawing.Point(412, 0);
            this.labelSamePointNumUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelSamePointNumUnit.Name = "labelSamePointNumUnit";
            this.labelSamePointNumUnit.Size = new System.Drawing.Size(50, 29);
            this.labelSamePointNumUnit.TabIndex = 60;
            this.labelSamePointNumUnit.Text = "EA";
            this.labelSamePointNumUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // samePointNum
            // 
            this.samePointNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.samePointNum.Location = new System.Drawing.Point(327, 0);
            this.samePointNum.Margin = new System.Windows.Forms.Padding(0);
            this.samePointNum.Name = "samePointNum";
            this.samePointNum.Size = new System.Drawing.Size(85, 29);
            this.samePointNum.TabIndex = 187;
            this.samePointNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.samePointNum.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // labelSamePointNum
            // 
            this.labelSamePointNum.AutoSize = true;
            this.labelSamePointNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSamePointNum.Location = new System.Drawing.Point(0, 0);
            this.labelSamePointNum.Margin = new System.Windows.Forms.Padding(0);
            this.labelSamePointNum.Name = "labelSamePointNum";
            this.labelSamePointNum.Size = new System.Drawing.Size(125, 29);
            this.labelSamePointNum.TabIndex = 184;
            this.labelSamePointNum.Text = "Same Num";
            this.labelSamePointNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSamePointSheetAttack
            // 
            this.labelSamePointSheetAttack.AutoSize = true;
            this.labelSamePointSheetAttack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSamePointSheetAttack.Location = new System.Drawing.Point(0, 29);
            this.labelSamePointSheetAttack.Margin = new System.Windows.Forms.Padding(0);
            this.labelSamePointSheetAttack.Name = "labelSamePointSheetAttack";
            this.labelSamePointSheetAttack.Size = new System.Drawing.Size(125, 29);
            this.labelSamePointSheetAttack.TabIndex = 188;
            this.labelSamePointSheetAttack.Text = "Sheet Attack";
            this.labelSamePointSheetAttack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSamePointPole
            // 
            this.labelSamePointPole.AutoSize = true;
            this.labelSamePointPole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSamePointPole.Location = new System.Drawing.Point(0, 58);
            this.labelSamePointPole.Margin = new System.Windows.Forms.Padding(0);
            this.labelSamePointPole.Name = "labelSamePointPole";
            this.labelSamePointPole.Size = new System.Drawing.Size(125, 29);
            this.labelSamePointPole.TabIndex = 189;
            this.labelSamePointPole.Text = "Pole (Line)";
            this.labelSamePointPole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSamePointDielectric
            // 
            this.labelSamePointDielectric.AutoSize = true;
            this.labelSamePointDielectric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSamePointDielectric.Location = new System.Drawing.Point(0, 87);
            this.labelSamePointDielectric.Margin = new System.Windows.Forms.Padding(0);
            this.labelSamePointDielectric.Name = "labelSamePointDielectric";
            this.labelSamePointDielectric.Size = new System.Drawing.Size(125, 29);
            this.labelSamePointDielectric.TabIndex = 191;
            this.labelSamePointDielectric.Text = "Dielectric";
            this.labelSamePointDielectric.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSamePointPinHole
            // 
            this.labelSamePointPinHole.AutoSize = true;
            this.labelSamePointPinHole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSamePointPinHole.Location = new System.Drawing.Point(0, 116);
            this.labelSamePointPinHole.Margin = new System.Windows.Forms.Padding(0);
            this.labelSamePointPinHole.Name = "labelSamePointPinHole";
            this.labelSamePointPinHole.Size = new System.Drawing.Size(125, 29);
            this.labelSamePointPinHole.TabIndex = 192;
            this.labelSamePointPinHole.Text = "Pin Hole";
            this.labelSamePointPinHole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // samePointSheetAttack
            // 
            this.samePointSheetAttack.AutoSize = true;
            this.samePointSheetAttack.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.samePointSheetAttack.Checked = true;
            this.samePointSheetAttack.CheckState = System.Windows.Forms.CheckState.Checked;
            this.samePointSheetAttack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.samePointSheetAttack.Location = new System.Drawing.Point(330, 32);
            this.samePointSheetAttack.Name = "samePointSheetAttack";
            this.samePointSheetAttack.Size = new System.Drawing.Size(79, 23);
            this.samePointSheetAttack.TabIndex = 194;
            this.samePointSheetAttack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.samePointSheetAttack.UseVisualStyleBackColor = true;
            // 
            // samePointPole
            // 
            this.samePointPole.AutoSize = true;
            this.samePointPole.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.samePointPole.Checked = true;
            this.samePointPole.CheckState = System.Windows.Forms.CheckState.Checked;
            this.samePointPole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.samePointPole.Location = new System.Drawing.Point(330, 61);
            this.samePointPole.Name = "samePointPole";
            this.samePointPole.Size = new System.Drawing.Size(79, 23);
            this.samePointPole.TabIndex = 195;
            this.samePointPole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.samePointPole.UseVisualStyleBackColor = true;
            // 
            // samePointDielectric
            // 
            this.samePointDielectric.AutoSize = true;
            this.samePointDielectric.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.samePointDielectric.Checked = true;
            this.samePointDielectric.CheckState = System.Windows.Forms.CheckState.Checked;
            this.samePointDielectric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.samePointDielectric.Location = new System.Drawing.Point(330, 90);
            this.samePointDielectric.Name = "samePointDielectric";
            this.samePointDielectric.Size = new System.Drawing.Size(79, 23);
            this.samePointDielectric.TabIndex = 197;
            this.samePointDielectric.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.samePointDielectric.UseVisualStyleBackColor = true;
            // 
            // samePointPinHole
            // 
            this.samePointPinHole.AutoSize = true;
            this.samePointPinHole.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.samePointPinHole.Checked = true;
            this.samePointPinHole.CheckState = System.Windows.Forms.CheckState.Checked;
            this.samePointPinHole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.samePointPinHole.Location = new System.Drawing.Point(330, 119);
            this.samePointPinHole.Name = "samePointPinHole";
            this.samePointPinHole.Size = new System.Drawing.Size(79, 23);
            this.samePointPinHole.TabIndex = 198;
            this.samePointPinHole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.samePointPinHole.UseVisualStyleBackColor = true;
            // 
            // samePointShape
            // 
            this.samePointShape.AutoSize = true;
            this.samePointShape.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.samePointShape.Checked = true;
            this.samePointShape.CheckState = System.Windows.Forms.CheckState.Checked;
            this.samePointShape.Dock = System.Windows.Forms.DockStyle.Fill;
            this.samePointShape.Location = new System.Drawing.Point(330, 148);
            this.samePointShape.Name = "samePointShape";
            this.samePointShape.Size = new System.Drawing.Size(79, 29);
            this.samePointShape.TabIndex = 199;
            this.samePointShape.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.samePointShape.UseVisualStyleBackColor = true;
            // 
            // layoutRecent
            // 
            this.layoutRecent.ColumnCount = 4;
            this.layoutRecent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.layoutRecent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutRecent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.layoutRecent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutRecent.Controls.Add(this.labelRecentShape, 0, 6);
            this.layoutRecent.Controls.Add(this.labelRecentRatioUnit, 3, 1);
            this.layoutRecent.Controls.Add(this.recentRatio, 2, 1);
            this.layoutRecent.Controls.Add(this.labelRecentRatio, 0, 1);
            this.layoutRecent.Controls.Add(this.labelRecentSheetAttack, 0, 2);
            this.layoutRecent.Controls.Add(this.labelRecentPole, 0, 3);
            this.layoutRecent.Controls.Add(this.labelRecentDielectric, 0, 4);
            this.layoutRecent.Controls.Add(this.labelRecentPinHole, 0, 5);
            this.layoutRecent.Controls.Add(this.recentSheetAttack, 2, 2);
            this.layoutRecent.Controls.Add(this.recentPole, 2, 3);
            this.layoutRecent.Controls.Add(this.recentDielectric, 2, 4);
            this.layoutRecent.Controls.Add(this.recentPinHole, 2, 5);
            this.layoutRecent.Controls.Add(this.recentShape, 2, 6);
            this.layoutRecent.Controls.Add(this.labelRecentNum, 0, 0);
            this.layoutRecent.Controls.Add(this.recentNum, 2, 0);
            this.layoutRecent.Controls.Add(this.labelRecentNumUnit, 3, 0);
            this.layoutRecent.Location = new System.Drawing.Point(5, 65);
            this.layoutRecent.Margin = new System.Windows.Forms.Padding(0);
            this.layoutRecent.Name = "layoutRecent";
            this.layoutRecent.RowCount = 7;
            this.layoutRecent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.layoutRecent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.layoutRecent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.layoutRecent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.layoutRecent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.layoutRecent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.layoutRecent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.layoutRecent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutRecent.Size = new System.Drawing.Size(462, 210);
            this.layoutRecent.TabIndex = 1;
            // 
            // labelRecentShape
            // 
            this.labelRecentShape.AutoSize = true;
            this.labelRecentShape.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRecentShape.Location = new System.Drawing.Point(0, 174);
            this.labelRecentShape.Margin = new System.Windows.Forms.Padding(0);
            this.labelRecentShape.Name = "labelRecentShape";
            this.labelRecentShape.Size = new System.Drawing.Size(125, 36);
            this.labelRecentShape.TabIndex = 193;
            this.labelRecentShape.Text = "Shape";
            this.labelRecentShape.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRecentRatioUnit
            // 
            this.labelRecentRatioUnit.AutoSize = true;
            this.labelRecentRatioUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRecentRatioUnit.Location = new System.Drawing.Point(412, 29);
            this.labelRecentRatioUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelRecentRatioUnit.Name = "labelRecentRatioUnit";
            this.labelRecentRatioUnit.Size = new System.Drawing.Size(50, 29);
            this.labelRecentRatioUnit.TabIndex = 60;
            this.labelRecentRatioUnit.Text = "%";
            this.labelRecentRatioUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // recentRatio
            // 
            this.recentRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recentRatio.Location = new System.Drawing.Point(327, 29);
            this.recentRatio.Margin = new System.Windows.Forms.Padding(0);
            this.recentRatio.Name = "recentRatio";
            this.recentRatio.Size = new System.Drawing.Size(85, 29);
            this.recentRatio.TabIndex = 186;
            this.recentRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.recentRatio.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // labelRecentRatio
            // 
            this.labelRecentRatio.AutoSize = true;
            this.labelRecentRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRecentRatio.Location = new System.Drawing.Point(0, 29);
            this.labelRecentRatio.Margin = new System.Windows.Forms.Padding(0);
            this.labelRecentRatio.Name = "labelRecentRatio";
            this.labelRecentRatio.Size = new System.Drawing.Size(125, 29);
            this.labelRecentRatio.TabIndex = 185;
            this.labelRecentRatio.Text = "Ratio";
            this.labelRecentRatio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRecentSheetAttack
            // 
            this.labelRecentSheetAttack.AutoSize = true;
            this.labelRecentSheetAttack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRecentSheetAttack.Location = new System.Drawing.Point(0, 58);
            this.labelRecentSheetAttack.Margin = new System.Windows.Forms.Padding(0);
            this.labelRecentSheetAttack.Name = "labelRecentSheetAttack";
            this.labelRecentSheetAttack.Size = new System.Drawing.Size(125, 29);
            this.labelRecentSheetAttack.TabIndex = 188;
            this.labelRecentSheetAttack.Text = "Sheet Attack";
            this.labelRecentSheetAttack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRecentPole
            // 
            this.labelRecentPole.AutoSize = true;
            this.labelRecentPole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRecentPole.Location = new System.Drawing.Point(0, 87);
            this.labelRecentPole.Margin = new System.Windows.Forms.Padding(0);
            this.labelRecentPole.Name = "labelRecentPole";
            this.labelRecentPole.Size = new System.Drawing.Size(125, 29);
            this.labelRecentPole.TabIndex = 189;
            this.labelRecentPole.Text = "Pole";
            this.labelRecentPole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRecentDielectric
            // 
            this.labelRecentDielectric.AutoSize = true;
            this.labelRecentDielectric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRecentDielectric.Location = new System.Drawing.Point(0, 116);
            this.labelRecentDielectric.Margin = new System.Windows.Forms.Padding(0);
            this.labelRecentDielectric.Name = "labelRecentDielectric";
            this.labelRecentDielectric.Size = new System.Drawing.Size(125, 29);
            this.labelRecentDielectric.TabIndex = 191;
            this.labelRecentDielectric.Text = "Dielectric";
            this.labelRecentDielectric.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelRecentPinHole
            // 
            this.labelRecentPinHole.AutoSize = true;
            this.labelRecentPinHole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRecentPinHole.Location = new System.Drawing.Point(0, 145);
            this.labelRecentPinHole.Margin = new System.Windows.Forms.Padding(0);
            this.labelRecentPinHole.Name = "labelRecentPinHole";
            this.labelRecentPinHole.Size = new System.Drawing.Size(125, 29);
            this.labelRecentPinHole.TabIndex = 192;
            this.labelRecentPinHole.Text = "Pin Hole";
            this.labelRecentPinHole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // recentSheetAttack
            // 
            this.recentSheetAttack.AutoSize = true;
            this.recentSheetAttack.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.recentSheetAttack.Checked = true;
            this.recentSheetAttack.CheckState = System.Windows.Forms.CheckState.Checked;
            this.recentSheetAttack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recentSheetAttack.Location = new System.Drawing.Point(330, 61);
            this.recentSheetAttack.Name = "recentSheetAttack";
            this.recentSheetAttack.Size = new System.Drawing.Size(79, 23);
            this.recentSheetAttack.TabIndex = 194;
            this.recentSheetAttack.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.recentSheetAttack.UseVisualStyleBackColor = true;
            // 
            // recentPole
            // 
            this.recentPole.AutoSize = true;
            this.recentPole.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.recentPole.Checked = true;
            this.recentPole.CheckState = System.Windows.Forms.CheckState.Checked;
            this.recentPole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recentPole.Location = new System.Drawing.Point(330, 90);
            this.recentPole.Name = "recentPole";
            this.recentPole.Size = new System.Drawing.Size(79, 23);
            this.recentPole.TabIndex = 195;
            this.recentPole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.recentPole.UseVisualStyleBackColor = true;
            // 
            // recentDielectric
            // 
            this.recentDielectric.AutoSize = true;
            this.recentDielectric.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.recentDielectric.Checked = true;
            this.recentDielectric.CheckState = System.Windows.Forms.CheckState.Checked;
            this.recentDielectric.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recentDielectric.Location = new System.Drawing.Point(330, 119);
            this.recentDielectric.Name = "recentDielectric";
            this.recentDielectric.Size = new System.Drawing.Size(79, 23);
            this.recentDielectric.TabIndex = 197;
            this.recentDielectric.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.recentDielectric.UseVisualStyleBackColor = true;
            // 
            // recentPinHole
            // 
            this.recentPinHole.AutoSize = true;
            this.recentPinHole.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.recentPinHole.Checked = true;
            this.recentPinHole.CheckState = System.Windows.Forms.CheckState.Checked;
            this.recentPinHole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recentPinHole.Location = new System.Drawing.Point(330, 148);
            this.recentPinHole.Name = "recentPinHole";
            this.recentPinHole.Size = new System.Drawing.Size(79, 23);
            this.recentPinHole.TabIndex = 198;
            this.recentPinHole.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.recentPinHole.UseVisualStyleBackColor = true;
            // 
            // recentShape
            // 
            this.recentShape.AutoSize = true;
            this.recentShape.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.recentShape.Checked = true;
            this.recentShape.CheckState = System.Windows.Forms.CheckState.Checked;
            this.recentShape.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recentShape.Location = new System.Drawing.Point(330, 177);
            this.recentShape.Name = "recentShape";
            this.recentShape.Size = new System.Drawing.Size(79, 30);
            this.recentShape.TabIndex = 199;
            this.recentShape.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.recentShape.UseVisualStyleBackColor = true;
            // 
            // labelRecentNum
            // 
            this.labelRecentNum.AutoSize = true;
            this.labelRecentNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRecentNum.Location = new System.Drawing.Point(0, 0);
            this.labelRecentNum.Margin = new System.Windows.Forms.Padding(0);
            this.labelRecentNum.Name = "labelRecentNum";
            this.labelRecentNum.Size = new System.Drawing.Size(125, 29);
            this.labelRecentNum.TabIndex = 184;
            this.labelRecentNum.Text = "Recent Num";
            this.labelRecentNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // recentNum
            // 
            this.recentNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recentNum.Location = new System.Drawing.Point(327, 0);
            this.recentNum.Margin = new System.Windows.Forms.Padding(0);
            this.recentNum.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.recentNum.Name = "recentNum";
            this.recentNum.Size = new System.Drawing.Size(85, 29);
            this.recentNum.TabIndex = 187;
            this.recentNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.recentNum.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // labelRecentNumUnit
            // 
            this.labelRecentNumUnit.AutoSize = true;
            this.labelRecentNumUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRecentNumUnit.Location = new System.Drawing.Point(412, 0);
            this.labelRecentNumUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelRecentNumUnit.Name = "labelRecentNumUnit";
            this.labelRecentNumUnit.Size = new System.Drawing.Size(50, 29);
            this.labelRecentNumUnit.TabIndex = 60;
            this.labelRecentNumUnit.Text = "EA";
            this.labelRecentNumUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutCheckPoint
            // 
            this.layoutCheckPoint.ColumnCount = 4;
            this.layoutCheckPoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.layoutCheckPoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutCheckPoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.layoutCheckPoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutCheckPoint.Controls.Add(this.labelCheckPointIndexUnit, 3, 0);
            this.layoutCheckPoint.Controls.Add(this.checkPointIndex, 2, 0);
            this.layoutCheckPoint.Controls.Add(this.labelCheckPointIndex, 0, 0);
            this.layoutCheckPoint.Controls.Add(this.labelCheckPointRatio, 0, 1);
            this.layoutCheckPoint.Controls.Add(this.checkPointRatio, 2, 1);
            this.layoutCheckPoint.Controls.Add(this.labelCheckPointRatioUnit, 3, 1);
            this.layoutCheckPoint.Location = new System.Drawing.Point(5, 5);
            this.layoutCheckPoint.Margin = new System.Windows.Forms.Padding(0);
            this.layoutCheckPoint.Name = "layoutCheckPoint";
            this.layoutCheckPoint.RowCount = 2;
            this.layoutCheckPoint.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutCheckPoint.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.00001F));
            this.layoutCheckPoint.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutCheckPoint.Size = new System.Drawing.Size(462, 60);
            this.layoutCheckPoint.TabIndex = 0;
            // 
            // labelCheckPointIndexUnit
            // 
            this.labelCheckPointIndexUnit.AutoSize = true;
            this.labelCheckPointIndexUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCheckPointIndexUnit.Location = new System.Drawing.Point(412, 0);
            this.labelCheckPointIndexUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelCheckPointIndexUnit.Name = "labelCheckPointIndexUnit";
            this.labelCheckPointIndexUnit.Size = new System.Drawing.Size(50, 29);
            this.labelCheckPointIndexUnit.TabIndex = 60;
            this.labelCheckPointIndexUnit.Text = "EA";
            this.labelCheckPointIndexUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkPointIndex
            // 
            this.checkPointIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkPointIndex.Location = new System.Drawing.Point(327, 0);
            this.checkPointIndex.Margin = new System.Windows.Forms.Padding(0);
            this.checkPointIndex.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.checkPointIndex.Name = "checkPointIndex";
            this.checkPointIndex.Size = new System.Drawing.Size(85, 29);
            this.checkPointIndex.TabIndex = 187;
            this.checkPointIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.checkPointIndex.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // labelCheckPointIndex
            // 
            this.labelCheckPointIndex.AutoSize = true;
            this.labelCheckPointIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCheckPointIndex.Location = new System.Drawing.Point(0, 0);
            this.labelCheckPointIndex.Margin = new System.Windows.Forms.Padding(0);
            this.labelCheckPointIndex.Name = "labelCheckPointIndex";
            this.labelCheckPointIndex.Size = new System.Drawing.Size(125, 29);
            this.labelCheckPointIndex.TabIndex = 184;
            this.labelCheckPointIndex.Text = "Check Index";
            this.labelCheckPointIndex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelCheckPointRatio
            // 
            this.labelCheckPointRatio.AutoSize = true;
            this.labelCheckPointRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCheckPointRatio.Location = new System.Drawing.Point(0, 29);
            this.labelCheckPointRatio.Margin = new System.Windows.Forms.Padding(0);
            this.labelCheckPointRatio.Name = "labelCheckPointRatio";
            this.labelCheckPointRatio.Size = new System.Drawing.Size(125, 31);
            this.labelCheckPointRatio.TabIndex = 185;
            this.labelCheckPointRatio.Text = "Ratio";
            this.labelCheckPointRatio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkPointRatio
            // 
            this.checkPointRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkPointRatio.Location = new System.Drawing.Point(327, 29);
            this.checkPointRatio.Margin = new System.Windows.Forms.Padding(0);
            this.checkPointRatio.Name = "checkPointRatio";
            this.checkPointRatio.Size = new System.Drawing.Size(85, 29);
            this.checkPointRatio.TabIndex = 186;
            this.checkPointRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.checkPointRatio.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // labelCheckPointRatioUnit
            // 
            this.labelCheckPointRatioUnit.AutoSize = true;
            this.labelCheckPointRatioUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCheckPointRatioUnit.Location = new System.Drawing.Point(412, 29);
            this.labelCheckPointRatioUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelCheckPointRatioUnit.Name = "labelCheckPointRatioUnit";
            this.labelCheckPointRatioUnit.Size = new System.Drawing.Size(50, 31);
            this.labelCheckPointRatioUnit.TabIndex = 59;
            this.labelCheckPointRatioUnit.Text = "%";
            this.labelCheckPointRatioUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioAlarm
            // 
            this.radioAlarm.AutoSize = true;
            this.radioAlarm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioAlarm.Location = new System.Drawing.Point(230, 45);
            this.radioAlarm.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.radioAlarm.Name = "radioAlarm";
            this.radioAlarm.Size = new System.Drawing.Size(107, 26);
            this.radioAlarm.TabIndex = 197;
            this.radioAlarm.TabStop = true;
            this.radioAlarm.Text = "Alarm";
            this.radioAlarm.UseVisualStyleBackColor = true;
            // 
            // radioNG
            // 
            this.radioNG.AutoSize = true;
            this.radioNG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioNG.Location = new System.Drawing.Point(361, 45);
            this.radioNG.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            this.radioNG.Name = "radioNG";
            this.radioNG.Size = new System.Drawing.Size(109, 26);
            this.radioNG.TabIndex = 198;
            this.radioNG.TabStop = true;
            this.radioNG.Text = "NG";
            this.radioNG.UseVisualStyleBackColor = true;
            // 
            // AlarmCheckerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(474, 823);
            this.ControlBox = false;
            this.Controls.Add(this.layoutMain);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "AlarmCheckerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.layoutMain.ResumeLayout(false);
            this.layoutMain.PerformLayout();
            this.layoutButton.ResumeLayout(false);
            this.layoutParam.ResumeLayout(false);
            this.layoutSamePoint.ResumeLayout(false);
            this.layoutSamePoint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.samePointNum)).EndInit();
            this.layoutRecent.ResumeLayout(false);
            this.layoutRecent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recentRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.recentNum)).EndInit();
            this.layoutCheckPoint.ResumeLayout(false);
            this.layoutCheckPoint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkPointIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkPointRatio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.TableLayoutPanel layoutMain;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ComboBox alarmType;
        private System.Windows.Forms.Label labelAlarmType;
        private System.Windows.Forms.TableLayoutPanel layoutButton;
        private Infragistics.Win.Misc.UltraButton btnApply;
        private Infragistics.Win.Misc.UltraButton btnCancel;
        private System.Windows.Forms.TableLayoutPanel layoutParam;
        private System.Windows.Forms.TableLayoutPanel layoutSamePoint;
        private System.Windows.Forms.Label labelSamePointShape;
        private System.Windows.Forms.Label labelSamePointNumUnit;
        private System.Windows.Forms.NumericUpDown samePointNum;
        private System.Windows.Forms.Label labelSamePointNum;
        private System.Windows.Forms.Label labelSamePointSheetAttack;
        private System.Windows.Forms.Label labelSamePointPole;
        private System.Windows.Forms.Label labelSamePointDielectric;
        private System.Windows.Forms.Label labelSamePointPinHole;
        private System.Windows.Forms.CheckBox samePointSheetAttack;
        private System.Windows.Forms.CheckBox samePointPole;
        private System.Windows.Forms.CheckBox samePointDielectric;
        private System.Windows.Forms.CheckBox samePointPinHole;
        private System.Windows.Forms.CheckBox samePointShape;
        private System.Windows.Forms.TableLayoutPanel layoutRecent;
        private System.Windows.Forms.Label labelRecentShape;
        private System.Windows.Forms.Label labelRecentRatioUnit;
        private System.Windows.Forms.NumericUpDown recentRatio;
        private System.Windows.Forms.Label labelRecentRatio;
        private System.Windows.Forms.Label labelRecentSheetAttack;
        private System.Windows.Forms.Label labelRecentPole;
        private System.Windows.Forms.Label labelRecentDielectric;
        private System.Windows.Forms.Label labelRecentPinHole;
        private System.Windows.Forms.CheckBox recentSheetAttack;
        private System.Windows.Forms.CheckBox recentPole;
        private System.Windows.Forms.CheckBox recentDielectric;
        private System.Windows.Forms.CheckBox recentPinHole;
        private System.Windows.Forms.CheckBox recentShape;
        private System.Windows.Forms.Label labelRecentNum;
        private System.Windows.Forms.NumericUpDown recentNum;
        private System.Windows.Forms.Label labelRecentNumUnit;
        private System.Windows.Forms.TableLayoutPanel layoutCheckPoint;
        private System.Windows.Forms.Label labelCheckPointIndexUnit;
        private System.Windows.Forms.NumericUpDown checkPointIndex;
        private System.Windows.Forms.Label labelCheckPointIndex;
        private System.Windows.Forms.Label labelCheckPointRatio;
        private System.Windows.Forms.NumericUpDown checkPointRatio;
        private System.Windows.Forms.Label labelCheckPointRatioUnit;
        private System.Windows.Forms.Label labelIOType;
        private System.Windows.Forms.RadioButton radioAlarm;
        private System.Windows.Forms.RadioButton radioNG;
    }
}