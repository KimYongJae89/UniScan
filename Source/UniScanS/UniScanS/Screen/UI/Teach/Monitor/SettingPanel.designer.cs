namespace UniScanS.Screen.UI.Teach.Monitor
{
    partial class SettingPanel
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
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.layoutParam = new System.Windows.Forms.TableLayoutPanel();
            this.layoutLight = new System.Windows.Forms.TableLayoutPanel();
            this.labelTopLight = new System.Windows.Forms.Label();
            this.labelBottomLight = new System.Windows.Forms.Label();
            this.bottomLightValue = new System.Windows.Forms.NumericUpDown();
            this.topLightValue = new System.Windows.Forms.NumericUpDown();
            this.labelLightTitle = new System.Windows.Forms.Label();
            this.layoutMaster = new System.Windows.Forms.TableLayoutPanel();
            this.btnSetting = new Infragistics.Win.Misc.UltraButton();
            this.labelMaster = new System.Windows.Forms.Label();
            this.panelEmpty1 = new System.Windows.Forms.Panel();
            this.layoutDefectOutput = new System.Windows.Forms.TableLayoutPanel();
            this.errorNum = new System.Windows.Forms.NumericUpDown();
            this.panelEmpty2 = new System.Windows.Forms.Panel();
            this.labelErrorNum = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonEditDefectOutput = new Infragistics.Win.Misc.UltraButton();
            this.buttonAddDefectOutput = new Infragistics.Win.Misc.UltraButton();
            this.labelAlarmOutput = new System.Windows.Forms.Label();
            this.alarmCheckerGridView = new System.Windows.Forms.DataGridView();
            this.panelEmpty5 = new System.Windows.Forms.Panel();
            this.labelSignalTimeUnit = new System.Windows.Forms.Label();
            this.signalTime = new System.Windows.Forms.NumericUpDown();
            this.buttonDeleteDefectOutput = new Infragistics.Win.Misc.UltraButton();
            this.useAlarmOutput = new System.Windows.Forms.CheckBox();
            this.panelEmpty4 = new System.Windows.Forms.Panel();
            this.labelError = new System.Windows.Forms.Label();
            this.columnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnDefectOutputType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnUseDefectOutput = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.layoutParam.SuspendLayout();
            this.layoutLight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bottomLightValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topLightValue)).BeginInit();
            this.layoutMaster.SuspendLayout();
            this.panelEmpty1.SuspendLayout();
            this.layoutDefectOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alarmCheckerGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.signalTime)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutParam
            // 
            this.layoutParam.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutParam.ColumnCount = 1;
            this.layoutParam.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutParam.Controls.Add(this.layoutLight, 0, 1);
            this.layoutParam.Controls.Add(this.labelLightTitle, 0, 0);
            this.layoutParam.Controls.Add(this.layoutMaster, 0, 2);
            this.layoutParam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutParam.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold);
            this.layoutParam.Location = new System.Drawing.Point(0, 0);
            this.layoutParam.Margin = new System.Windows.Forms.Padding(0);
            this.layoutParam.Name = "layoutParam";
            this.layoutParam.RowCount = 4;
            this.layoutParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.layoutParam.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.layoutParam.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutParam.Size = new System.Drawing.Size(400, 737);
            this.layoutParam.TabIndex = 27;
            // 
            // layoutLight
            // 
            this.layoutLight.ColumnCount = 3;
            this.layoutLight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutLight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutLight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.layoutLight.Controls.Add(this.labelTopLight, 0, 0);
            this.layoutLight.Controls.Add(this.labelBottomLight, 0, 1);
            this.layoutLight.Controls.Add(this.bottomLightValue, 2, 1);
            this.layoutLight.Controls.Add(this.topLightValue, 2, 0);
            this.layoutLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutLight.Location = new System.Drawing.Point(1, 42);
            this.layoutLight.Margin = new System.Windows.Forms.Padding(0);
            this.layoutLight.Name = "layoutLight";
            this.layoutLight.RowCount = 2;
            this.layoutLight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutLight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.layoutLight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutLight.Size = new System.Drawing.Size(398, 62);
            this.layoutLight.TabIndex = 0;
            // 
            // labelTopLight
            // 
            this.labelTopLight.AutoSize = true;
            this.labelTopLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTopLight.Location = new System.Drawing.Point(0, 0);
            this.labelTopLight.Margin = new System.Windows.Forms.Padding(0);
            this.labelTopLight.Name = "labelTopLight";
            this.labelTopLight.Size = new System.Drawing.Size(100, 31);
            this.labelTopLight.TabIndex = 178;
            this.labelTopLight.Text = "Top";
            this.labelTopLight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelBottomLight
            // 
            this.labelBottomLight.AutoSize = true;
            this.labelBottomLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelBottomLight.Location = new System.Drawing.Point(0, 31);
            this.labelBottomLight.Margin = new System.Windows.Forms.Padding(0);
            this.labelBottomLight.Name = "labelBottomLight";
            this.labelBottomLight.Size = new System.Drawing.Size(100, 31);
            this.labelBottomLight.TabIndex = 179;
            this.labelBottomLight.Text = "Bottom";
            this.labelBottomLight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bottomLightValue
            // 
            this.bottomLightValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomLightValue.Location = new System.Drawing.Point(323, 31);
            this.bottomLightValue.Margin = new System.Windows.Forms.Padding(0);
            this.bottomLightValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.bottomLightValue.Name = "bottomLightValue";
            this.bottomLightValue.Size = new System.Drawing.Size(75, 32);
            this.bottomLightValue.TabIndex = 176;
            this.bottomLightValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.bottomLightValue.ValueChanged += new System.EventHandler(this.bottomLightValue_ValueChanged);
            // 
            // topLightValue
            // 
            this.topLightValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topLightValue.Location = new System.Drawing.Point(323, 0);
            this.topLightValue.Margin = new System.Windows.Forms.Padding(0);
            this.topLightValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.topLightValue.Name = "topLightValue";
            this.topLightValue.Size = new System.Drawing.Size(75, 32);
            this.topLightValue.TabIndex = 0;
            this.topLightValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.topLightValue.ValueChanged += new System.EventHandler(this.lightValue_ValueChanged);
            // 
            // labelLightTitle
            // 
            this.labelLightTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelLightTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelLightTitle.Font = new System.Drawing.Font("맑은 고딕", 16F, System.Drawing.FontStyle.Bold);
            this.labelLightTitle.Location = new System.Drawing.Point(1, 1);
            this.labelLightTitle.Margin = new System.Windows.Forms.Padding(0);
            this.labelLightTitle.Name = "labelLightTitle";
            this.labelLightTitle.Size = new System.Drawing.Size(398, 40);
            this.labelLightTitle.TabIndex = 26;
            this.labelLightTitle.Text = "Light";
            this.labelLightTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // layoutMaster
            // 
            this.layoutMaster.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutMaster.ColumnCount = 2;
            this.layoutMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMaster.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutMaster.Controls.Add(this.btnSetting, 1, 0);
            this.layoutMaster.Controls.Add(this.labelMaster, 0, 0);
            this.layoutMaster.Controls.Add(this.panelEmpty1, 0, 1);
            this.layoutMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutMaster.Location = new System.Drawing.Point(1, 105);
            this.layoutMaster.Margin = new System.Windows.Forms.Padding(0);
            this.layoutMaster.Name = "layoutMaster";
            this.layoutMaster.RowCount = 2;
            this.layoutMaster.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.layoutMaster.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutMaster.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutMaster.Size = new System.Drawing.Size(398, 500);
            this.layoutMaster.TabIndex = 178;
            // 
            // btnSetting
            // 
            appearance1.BackColor = System.Drawing.Color.White;
            appearance1.Image = global::UniScanS.Properties.Resources.teach_black_36;
            appearance1.ImageHAlign = Infragistics.Win.HAlign.Center;
            appearance1.ImageVAlign = Infragistics.Win.VAlign.Middle;
            this.btnSetting.Appearance = appearance1;
            this.btnSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSetting.ImageSize = new System.Drawing.Size(32, 32);
            this.btnSetting.Location = new System.Drawing.Point(357, 1);
            this.btnSetting.Margin = new System.Windows.Forms.Padding(0);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(40, 40);
            this.btnSetting.TabIndex = 181;
            this.btnSetting.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // labelMaster
            // 
            this.labelMaster.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.labelMaster.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaster.Location = new System.Drawing.Point(1, 1);
            this.labelMaster.Margin = new System.Windows.Forms.Padding(0);
            this.labelMaster.Name = "labelMaster";
            this.labelMaster.Size = new System.Drawing.Size(355, 40);
            this.labelMaster.TabIndex = 27;
            this.labelMaster.Text = "Master";
            this.labelMaster.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelEmpty1
            // 
            this.layoutMaster.SetColumnSpan(this.panelEmpty1, 2);
            this.panelEmpty1.Controls.Add(this.layoutDefectOutput);
            this.panelEmpty1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEmpty1.Location = new System.Drawing.Point(1, 42);
            this.panelEmpty1.Margin = new System.Windows.Forms.Padding(0);
            this.panelEmpty1.Name = "panelEmpty1";
            this.panelEmpty1.Padding = new System.Windows.Forms.Padding(5);
            this.panelEmpty1.Size = new System.Drawing.Size(396, 457);
            this.panelEmpty1.TabIndex = 182;
            // 
            // layoutDefectOutput
            // 
            this.layoutDefectOutput.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.layoutDefectOutput.ColumnCount = 5;
            this.layoutDefectOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutDefectOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutDefectOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.layoutDefectOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.layoutDefectOutput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.layoutDefectOutput.Controls.Add(this.errorNum, 4, 1);
            this.layoutDefectOutput.Controls.Add(this.panelEmpty2, 1, 1);
            this.layoutDefectOutput.Controls.Add(this.labelErrorNum, 0, 1);
            this.layoutDefectOutput.Controls.Add(this.buttonEditDefectOutput, 3, 4);
            this.layoutDefectOutput.Controls.Add(this.buttonAddDefectOutput, 2, 4);
            this.layoutDefectOutput.Controls.Add(this.labelAlarmOutput, 0, 2);
            this.layoutDefectOutput.Controls.Add(this.alarmCheckerGridView, 0, 5);
            this.layoutDefectOutput.Controls.Add(this.panelEmpty5, 0, 4);
            this.layoutDefectOutput.Controls.Add(this.buttonDeleteDefectOutput, 4, 4);
            this.layoutDefectOutput.Controls.Add(this.useAlarmOutput, 4, 2);
            this.layoutDefectOutput.Controls.Add(this.labelError, 0, 0);
            this.layoutDefectOutput.Controls.Add(this.label1, 0, 3);
            this.layoutDefectOutput.Controls.Add(this.panelEmpty4, 1, 3);
            this.layoutDefectOutput.Controls.Add(this.signalTime, 3, 3);
            this.layoutDefectOutput.Controls.Add(this.labelSignalTimeUnit, 4, 3);
            this.layoutDefectOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutDefectOutput.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            this.layoutDefectOutput.Location = new System.Drawing.Point(5, 5);
            this.layoutDefectOutput.Margin = new System.Windows.Forms.Padding(5);
            this.layoutDefectOutput.Name = "layoutDefectOutput";
            this.layoutDefectOutput.RowCount = 6;
            this.layoutDefectOutput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutDefectOutput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutDefectOutput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutDefectOutput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutDefectOutput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutDefectOutput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutDefectOutput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutDefectOutput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutDefectOutput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutDefectOutput.Size = new System.Drawing.Size(386, 447);
            this.layoutDefectOutput.TabIndex = 0;
            // 
            // errorNum
            // 
            this.errorNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorNum.Location = new System.Drawing.Point(320, 32);
            this.errorNum.Margin = new System.Windows.Forms.Padding(0);
            this.errorNum.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.errorNum.Name = "errorNum";
            this.errorNum.Size = new System.Drawing.Size(65, 29);
            this.errorNum.TabIndex = 262;
            this.errorNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.errorNum.ValueChanged += new System.EventHandler(this.errorNum_ValueChanged);
            // 
            // panelEmpty2
            // 
            this.layoutDefectOutput.SetColumnSpan(this.panelEmpty2, 3);
            this.panelEmpty2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEmpty2.Location = new System.Drawing.Point(102, 32);
            this.panelEmpty2.Margin = new System.Windows.Forms.Padding(0);
            this.panelEmpty2.Name = "panelEmpty2";
            this.panelEmpty2.Size = new System.Drawing.Size(217, 30);
            this.panelEmpty2.TabIndex = 261;
            // 
            // labelErrorNum
            // 
            this.labelErrorNum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelErrorNum.Location = new System.Drawing.Point(1, 32);
            this.labelErrorNum.Margin = new System.Windows.Forms.Padding(0);
            this.labelErrorNum.Name = "labelErrorNum";
            this.labelErrorNum.Size = new System.Drawing.Size(100, 30);
            this.labelErrorNum.TabIndex = 260;
            this.labelErrorNum.Text = "Num";
            this.labelErrorNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(1, 94);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 30);
            this.label1.TabIndex = 258;
            this.label1.Text = "Signal Time";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonEditDefectOutput
            // 
            appearance2.BackColor = System.Drawing.Color.White;
            this.buttonEditDefectOutput.Appearance = appearance2;
            this.buttonEditDefectOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEditDefectOutput.Location = new System.Drawing.Point(254, 125);
            this.buttonEditDefectOutput.Margin = new System.Windows.Forms.Padding(0);
            this.buttonEditDefectOutput.Name = "buttonEditDefectOutput";
            this.buttonEditDefectOutput.Size = new System.Drawing.Size(65, 30);
            this.buttonEditDefectOutput.TabIndex = 253;
            this.buttonEditDefectOutput.Text = "Edit";
            this.buttonEditDefectOutput.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonEditDefectOutput.Click += new System.EventHandler(this.buttonEditDefectOutput_Click);
            // 
            // buttonAddDefectOutput
            // 
            appearance3.BackColor = System.Drawing.Color.White;
            this.buttonAddDefectOutput.Appearance = appearance3;
            this.buttonAddDefectOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddDefectOutput.Location = new System.Drawing.Point(188, 125);
            this.buttonAddDefectOutput.Margin = new System.Windows.Forms.Padding(0);
            this.buttonAddDefectOutput.Name = "buttonAddDefectOutput";
            this.buttonAddDefectOutput.Size = new System.Drawing.Size(65, 30);
            this.buttonAddDefectOutput.TabIndex = 251;
            this.buttonAddDefectOutput.Text = "Add";
            this.buttonAddDefectOutput.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonAddDefectOutput.Click += new System.EventHandler(this.buttonAddDefectOutput_Click);
            // 
            // labelAlarmOutput
            // 
            this.labelAlarmOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.layoutDefectOutput.SetColumnSpan(this.labelAlarmOutput, 4);
            this.labelAlarmOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAlarmOutput.Location = new System.Drawing.Point(1, 63);
            this.labelAlarmOutput.Margin = new System.Windows.Forms.Padding(0);
            this.labelAlarmOutput.Name = "labelAlarmOutput";
            this.labelAlarmOutput.Size = new System.Drawing.Size(318, 30);
            this.labelAlarmOutput.TabIndex = 253;
            this.labelAlarmOutput.Text = "Alarm Output";
            this.labelAlarmOutput.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // alarmCheckerGridView
            // 
            this.alarmCheckerGridView.AllowUserToAddRows = false;
            this.alarmCheckerGridView.AllowUserToDeleteRows = false;
            this.alarmCheckerGridView.AllowUserToResizeColumns = false;
            this.alarmCheckerGridView.AllowUserToResizeRows = false;
            this.alarmCheckerGridView.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.alarmCheckerGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.alarmCheckerGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.alarmCheckerGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnNo,
            this.columnDefectOutputType,
            this.columnUseDefectOutput});
            this.layoutDefectOutput.SetColumnSpan(this.alarmCheckerGridView, 5);
            this.alarmCheckerGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alarmCheckerGridView.Location = new System.Drawing.Point(1, 156);
            this.alarmCheckerGridView.Margin = new System.Windows.Forms.Padding(0);
            this.alarmCheckerGridView.MultiSelect = false;
            this.alarmCheckerGridView.Name = "alarmCheckerGridView";
            this.alarmCheckerGridView.ReadOnly = true;
            this.alarmCheckerGridView.RowHeadersVisible = false;
            this.alarmCheckerGridView.RowTemplate.Height = 23;
            this.alarmCheckerGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.alarmCheckerGridView.Size = new System.Drawing.Size(384, 290);
            this.alarmCheckerGridView.TabIndex = 30;
            // 
            // panelEmpty5
            // 
            this.layoutDefectOutput.SetColumnSpan(this.panelEmpty5, 2);
            this.panelEmpty5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEmpty5.Location = new System.Drawing.Point(1, 125);
            this.panelEmpty5.Margin = new System.Windows.Forms.Padding(0);
            this.panelEmpty5.Name = "panelEmpty5";
            this.panelEmpty5.Size = new System.Drawing.Size(186, 30);
            this.panelEmpty5.TabIndex = 255;
            // 
            // labelSignalTimeUnit
            // 
            this.labelSignalTimeUnit.AutoSize = true;
            this.labelSignalTimeUnit.Location = new System.Drawing.Point(320, 94);
            this.labelSignalTimeUnit.Margin = new System.Windows.Forms.Padding(0);
            this.labelSignalTimeUnit.Name = "labelSignalTimeUnit";
            this.labelSignalTimeUnit.Size = new System.Drawing.Size(32, 21);
            this.labelSignalTimeUnit.TabIndex = 257;
            this.labelSignalTimeUnit.Text = "ms";
            this.labelSignalTimeUnit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // signalTime
            // 
            this.signalTime.Location = new System.Drawing.Point(254, 94);
            this.signalTime.Margin = new System.Windows.Forms.Padding(0);
            this.signalTime.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.signalTime.Name = "signalTime";
            this.signalTime.Size = new System.Drawing.Size(65, 29);
            this.signalTime.TabIndex = 256;
            this.signalTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.signalTime.ValueChanged += new System.EventHandler(this.signalTime_ValueChanged);
            // 
            // buttonDeleteDefectOutput
            // 
            appearance4.BackColor = System.Drawing.Color.White;
            this.buttonDeleteDefectOutput.Appearance = appearance4;
            this.buttonDeleteDefectOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDeleteDefectOutput.Location = new System.Drawing.Point(320, 125);
            this.buttonDeleteDefectOutput.Margin = new System.Windows.Forms.Padding(0);
            this.buttonDeleteDefectOutput.Name = "buttonDeleteDefectOutput";
            this.buttonDeleteDefectOutput.Size = new System.Drawing.Size(65, 30);
            this.buttonDeleteDefectOutput.TabIndex = 252;
            this.buttonDeleteDefectOutput.Text = "Del";
            this.buttonDeleteDefectOutput.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.buttonDeleteDefectOutput.Click += new System.EventHandler(this.buttonDeleteDefectOutput_Click);
            // 
            // useAlarmOutput
            // 
            this.useAlarmOutput.AutoSize = true;
            this.useAlarmOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.useAlarmOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.useAlarmOutput.Location = new System.Drawing.Point(320, 63);
            this.useAlarmOutput.Margin = new System.Windows.Forms.Padding(0);
            this.useAlarmOutput.Name = "useAlarmOutput";
            this.useAlarmOutput.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.useAlarmOutput.Size = new System.Drawing.Size(65, 30);
            this.useAlarmOutput.TabIndex = 254;
            this.useAlarmOutput.Text = "Use";
            this.useAlarmOutput.UseVisualStyleBackColor = false;
            this.useAlarmOutput.CheckedChanged += new System.EventHandler(this.useDefectOutput_CheckedChanged);
            // 
            // panelEmpty4
            // 
            this.layoutDefectOutput.SetColumnSpan(this.panelEmpty4, 2);
            this.panelEmpty4.Location = new System.Drawing.Point(102, 94);
            this.panelEmpty4.Margin = new System.Windows.Forms.Padding(0);
            this.panelEmpty4.Name = "panelEmpty4";
            this.panelEmpty4.Size = new System.Drawing.Size(151, 30);
            this.panelEmpty4.TabIndex = 178;
            // 
            // labelError
            // 
            this.labelError.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(230)))), ((int)(((byte)(240)))));
            this.layoutDefectOutput.SetColumnSpan(this.labelError, 5);
            this.labelError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelError.Location = new System.Drawing.Point(1, 1);
            this.labelError.Margin = new System.Windows.Forms.Padding(0);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(384, 30);
            this.labelError.TabIndex = 255;
            this.labelError.Text = "Error";
            this.labelError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // columnNo
            // 
            this.columnNo.HeaderText = "No";
            this.columnNo.Name = "columnNo";
            this.columnNo.ReadOnly = true;
            // 
            // columnDefectOutputType
            // 
            this.columnDefectOutputType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnDefectOutputType.HeaderText = "Type";
            this.columnDefectOutputType.Name = "columnDefectOutputType";
            this.columnDefectOutputType.ReadOnly = true;
            // 
            // columnUseDefectOutput
            // 
            this.columnUseDefectOutput.HeaderText = "IO";
            this.columnUseDefectOutput.Name = "columnUseDefectOutput";
            this.columnUseDefectOutput.ReadOnly = true;
            this.columnUseDefectOutput.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.columnUseDefectOutput.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SettingPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.layoutParam);
            this.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SettingPanel";
            this.Size = new System.Drawing.Size(400, 737);
            this.layoutParam.ResumeLayout(false);
            this.layoutLight.ResumeLayout(false);
            this.layoutLight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bottomLightValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topLightValue)).EndInit();
            this.layoutMaster.ResumeLayout(false);
            this.panelEmpty1.ResumeLayout(false);
            this.layoutDefectOutput.ResumeLayout(false);
            this.layoutDefectOutput.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alarmCheckerGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.signalTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel layoutParam;
        private System.Windows.Forms.NumericUpDown topLightValue;
        private System.Windows.Forms.Label labelLightTitle;
        private System.Windows.Forms.Label labelBottomLight;
        private System.Windows.Forms.NumericUpDown bottomLightValue;
        private System.Windows.Forms.Label labelTopLight;
        private System.Windows.Forms.Label labelMaster;
        private System.Windows.Forms.TableLayoutPanel layoutLight;
        private System.Windows.Forms.TableLayoutPanel layoutMaster;
        private Infragistics.Win.Misc.UltraButton btnSetting;
        private System.Windows.Forms.TableLayoutPanel layoutDefectOutput;
        public System.Windows.Forms.DataGridView alarmCheckerGridView;
        private Infragistics.Win.Misc.UltraButton buttonDeleteDefectOutput;
        private Infragistics.Win.Misc.UltraButton buttonAddDefectOutput;
        private System.Windows.Forms.Panel panelEmpty1;
        private System.Windows.Forms.Label labelAlarmOutput;
        private System.Windows.Forms.CheckBox useAlarmOutput;
        private System.Windows.Forms.Panel panelEmpty4;
        private System.Windows.Forms.NumericUpDown signalTime;
        private System.Windows.Forms.Label labelSignalTimeUnit;
        private System.Windows.Forms.Panel panelEmpty5;
        private Infragistics.Win.Misc.UltraButton buttonEditDefectOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown errorNum;
        private System.Windows.Forms.Panel panelEmpty2;
        private System.Windows.Forms.Label labelErrorNum;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnDefectOutputType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnUseDefectOutput;
    }
}
