namespace UniScanX.MPAlignment.UI.Pages
{
    partial class UserConfigPage
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
            this.nudLightStableTime = new System.Windows.Forms.NumericUpDown();
            this.labelMs = new System.Windows.Forms.Label();
            this.lblLightStableTime = new System.Windows.Forms.Label();
            this.nudBeforMachineToEntry = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lblBoardFormRearTimeout = new System.Windows.Forms.Label();
            this.btnApplyConfig = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.nudFlushTimeout = new System.Windows.Forms.NumericUpDown();
            this.lblFlushTimeout = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nudEjectToNextMachine = new System.Windows.Forms.NumericUpDown();
            this.lblEjectToNextMachine = new System.Windows.Forms.Label();
            this.lblInspectReadyToEject = new System.Windows.Forms.Label();
            this.nudInspectReadyToEject = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.nudSlowdownToInspectReady = new System.Windows.Forms.NumericUpDown();
            this.lblEntryToInsepctionReadyTimeout = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nudEntryToSlowDown = new System.Windows.Forms.NumericUpDown();
            this.lblEntryToSlowDownTimeout = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblConveyorSetting = new System.Windows.Forms.Label();
            this.lblReviewMode = new System.Windows.Forms.Label();
            this.tgsReviewMode = new ReaLTaiizor.HopeToggle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblInspectOption = new System.Windows.Forms.Label();
            this.lblUseSmema = new System.Windows.Forms.Label();
            this.tgsUseSmema = new ReaLTaiizor.HopeToggle();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnResultSet = new System.Windows.Forms.Button();
            this.txtResultPath = new System.Windows.Forms.TextBox();
            this.lblInspResultPath = new System.Windows.Forms.Label();
            this.lblResultPath = new System.Windows.Forms.Label();
            this.button_MotionSetting = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudLightStableTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBeforMachineToEntry)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFlushTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEjectToNextMachine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInspectReadyToEject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSlowdownToInspectReady)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEntryToSlowDown)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // nudLightStableTime
            // 
            this.nudLightStableTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudLightStableTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudLightStableTime.ForeColor = System.Drawing.SystemColors.Window;
            this.nudLightStableTime.Location = new System.Drawing.Point(205, 43);
            this.nudLightStableTime.Margin = new System.Windows.Forms.Padding(4);
            this.nudLightStableTime.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nudLightStableTime.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudLightStableTime.Name = "nudLightStableTime";
            this.nudLightStableTime.Size = new System.Drawing.Size(93, 25);
            this.nudLightStableTime.TabIndex = 169;
            this.nudLightStableTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudLightStableTime.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // labelMs
            // 
            this.labelMs.AutoSize = true;
            this.labelMs.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelMs.Location = new System.Drawing.Point(308, 47);
            this.labelMs.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelMs.Name = "labelMs";
            this.labelMs.Size = new System.Drawing.Size(31, 21);
            this.labelMs.TabIndex = 167;
            this.labelMs.Text = "ms";
            // 
            // lblLightStableTime
            // 
            this.lblLightStableTime.AutoSize = true;
            this.lblLightStableTime.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLightStableTime.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblLightStableTime.Location = new System.Drawing.Point(12, 43);
            this.lblLightStableTime.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblLightStableTime.Name = "lblLightStableTime";
            this.lblLightStableTime.Size = new System.Drawing.Size(123, 20);
            this.lblLightStableTime.TabIndex = 168;
            this.lblLightStableTime.Text = "Light stable time";
            // 
            // nudBeforMachineToEntry
            // 
            this.nudBeforMachineToEntry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudBeforMachineToEntry.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudBeforMachineToEntry.ForeColor = System.Drawing.SystemColors.Window;
            this.nudBeforMachineToEntry.Location = new System.Drawing.Point(286, 41);
            this.nudBeforMachineToEntry.Margin = new System.Windows.Forms.Padding(4);
            this.nudBeforMachineToEntry.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.nudBeforMachineToEntry.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudBeforMachineToEntry.Name = "nudBeforMachineToEntry";
            this.nudBeforMachineToEntry.Size = new System.Drawing.Size(93, 25);
            this.nudBeforMachineToEntry.TabIndex = 172;
            this.nudBeforMachineToEntry.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudBeforMachineToEntry.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(389, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 21);
            this.label1.TabIndex = 170;
            this.label1.Text = "ms";
            // 
            // lblBoardFormRearTimeout
            // 
            this.lblBoardFormRearTimeout.AutoSize = true;
            this.lblBoardFormRearTimeout.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblBoardFormRearTimeout.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblBoardFormRearTimeout.Location = new System.Drawing.Point(25, 46);
            this.lblBoardFormRearTimeout.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblBoardFormRearTimeout.Name = "lblBoardFormRearTimeout";
            this.lblBoardFormRearTimeout.Size = new System.Drawing.Size(43, 20);
            this.lblBoardFormRearTimeout.TabIndex = 171;
            this.lblBoardFormRearTimeout.Text = "Entry";
            // 
            // btnApplyConfig
            // 
            this.btnApplyConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyConfig.Location = new System.Drawing.Point(610, 567);
            this.btnApplyConfig.Name = "btnApplyConfig";
            this.btnApplyConfig.Size = new System.Drawing.Size(109, 42);
            this.btnApplyConfig.TabIndex = 173;
            this.btnApplyConfig.Text = "Apply ";
            this.btnApplyConfig.UseVisualStyleBackColor = true;
            this.btnApplyConfig.Click += new System.EventHandler(this.btnApplyConfig_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.nudFlushTimeout);
            this.panel1.Controls.Add(this.lblFlushTimeout);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.nudEjectToNextMachine);
            this.panel1.Controls.Add(this.lblEjectToNextMachine);
            this.panel1.Controls.Add(this.lblInspectReadyToEject);
            this.panel1.Controls.Add(this.nudInspectReadyToEject);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.nudSlowdownToInspectReady);
            this.panel1.Controls.Add(this.lblEntryToInsepctionReadyTimeout);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.nudEntryToSlowDown);
            this.panel1.Controls.Add(this.lblEntryToSlowDownTimeout);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.lblConveyorSetting);
            this.panel1.Controls.Add(this.nudBeforMachineToEntry);
            this.panel1.Controls.Add(this.lblBoardFormRearTimeout);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(9, 92);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(455, 248);
            this.panel1.TabIndex = 174;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label6.Location = new System.Drawing.Point(389, 215);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(31, 21);
            this.label6.TabIndex = 188;
            this.label6.Text = "ms";
            // 
            // nudFlushTimeout
            // 
            this.nudFlushTimeout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudFlushTimeout.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudFlushTimeout.ForeColor = System.Drawing.SystemColors.Window;
            this.nudFlushTimeout.Location = new System.Drawing.Point(286, 211);
            this.nudFlushTimeout.Margin = new System.Windows.Forms.Padding(4);
            this.nudFlushTimeout.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.nudFlushTimeout.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudFlushTimeout.Name = "nudFlushTimeout";
            this.nudFlushTimeout.Size = new System.Drawing.Size(93, 25);
            this.nudFlushTimeout.TabIndex = 187;
            this.nudFlushTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudFlushTimeout.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblFlushTimeout
            // 
            this.lblFlushTimeout.AutoSize = true;
            this.lblFlushTimeout.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblFlushTimeout.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblFlushTimeout.Location = new System.Drawing.Point(27, 211);
            this.lblFlushTimeout.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblFlushTimeout.Name = "lblFlushTimeout";
            this.lblFlushTimeout.Size = new System.Drawing.Size(107, 20);
            this.lblFlushTimeout.TabIndex = 186;
            this.lblFlushTimeout.Text = "Flush time out";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(389, 179);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 21);
            this.label2.TabIndex = 185;
            this.label2.Text = "ms";
            // 
            // nudEjectToNextMachine
            // 
            this.nudEjectToNextMachine.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudEjectToNextMachine.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudEjectToNextMachine.ForeColor = System.Drawing.SystemColors.Window;
            this.nudEjectToNextMachine.Location = new System.Drawing.Point(286, 175);
            this.nudEjectToNextMachine.Margin = new System.Windows.Forms.Padding(4);
            this.nudEjectToNextMachine.Maximum = new decimal(new int[] {
            600000,
            0,
            0,
            0});
            this.nudEjectToNextMachine.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudEjectToNextMachine.Name = "nudEjectToNextMachine";
            this.nudEjectToNextMachine.Size = new System.Drawing.Size(93, 25);
            this.nudEjectToNextMachine.TabIndex = 184;
            this.nudEjectToNextMachine.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudEjectToNextMachine.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblEjectToNextMachine
            // 
            this.lblEjectToNextMachine.AutoSize = true;
            this.lblEjectToNextMachine.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblEjectToNextMachine.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblEjectToNextMachine.Location = new System.Drawing.Point(27, 175);
            this.lblEjectToNextMachine.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblEjectToNextMachine.Name = "lblEjectToNextMachine";
            this.lblEjectToNextMachine.Size = new System.Drawing.Size(157, 20);
            this.lblEjectToNextMachine.TabIndex = 183;
            this.lblEjectToNextMachine.Text = "Eject to next machine";
            // 
            // lblInspectReadyToEject
            // 
            this.lblInspectReadyToEject.AutoSize = true;
            this.lblInspectReadyToEject.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblInspectReadyToEject.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblInspectReadyToEject.Location = new System.Drawing.Point(25, 140);
            this.lblInspectReadyToEject.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblInspectReadyToEject.Name = "lblInspectReadyToEject";
            this.lblInspectReadyToEject.Size = new System.Drawing.Size(155, 20);
            this.lblInspectReadyToEject.TabIndex = 182;
            this.lblInspectReadyToEject.Text = "Inspect ready to eject";
            // 
            // nudInspectReadyToEject
            // 
            this.nudInspectReadyToEject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudInspectReadyToEject.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudInspectReadyToEject.ForeColor = System.Drawing.SystemColors.Window;
            this.nudInspectReadyToEject.Location = new System.Drawing.Point(286, 140);
            this.nudInspectReadyToEject.Margin = new System.Windows.Forms.Padding(4);
            this.nudInspectReadyToEject.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudInspectReadyToEject.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudInspectReadyToEject.Name = "nudInspectReadyToEject";
            this.nudInspectReadyToEject.Size = new System.Drawing.Size(93, 25);
            this.nudInspectReadyToEject.TabIndex = 181;
            this.nudInspectReadyToEject.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudInspectReadyToEject.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label7.Location = new System.Drawing.Point(389, 144);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(31, 21);
            this.label7.TabIndex = 179;
            this.label7.Text = "ms";
            // 
            // nudSlowdownToInspectReady
            // 
            this.nudSlowdownToInspectReady.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudSlowdownToInspectReady.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudSlowdownToInspectReady.ForeColor = System.Drawing.SystemColors.Window;
            this.nudSlowdownToInspectReady.Location = new System.Drawing.Point(286, 107);
            this.nudSlowdownToInspectReady.Margin = new System.Windows.Forms.Padding(4);
            this.nudSlowdownToInspectReady.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudSlowdownToInspectReady.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudSlowdownToInspectReady.Name = "nudSlowdownToInspectReady";
            this.nudSlowdownToInspectReady.Size = new System.Drawing.Size(93, 25);
            this.nudSlowdownToInspectReady.TabIndex = 178;
            this.nudSlowdownToInspectReady.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudSlowdownToInspectReady.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblEntryToInsepctionReadyTimeout
            // 
            this.lblEntryToInsepctionReadyTimeout.AutoSize = true;
            this.lblEntryToInsepctionReadyTimeout.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblEntryToInsepctionReadyTimeout.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblEntryToInsepctionReadyTimeout.Location = new System.Drawing.Point(25, 112);
            this.lblEntryToInsepctionReadyTimeout.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblEntryToInsepctionReadyTimeout.Name = "lblEntryToInsepctionReadyTimeout";
            this.lblEntryToInsepctionReadyTimeout.Size = new System.Drawing.Size(198, 20);
            this.lblEntryToInsepctionReadyTimeout.TabIndex = 177;
            this.lblEntryToInsepctionReadyTimeout.Text = "Slow down to inspect ready";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label5.Location = new System.Drawing.Point(389, 111);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 21);
            this.label5.TabIndex = 176;
            this.label5.Text = "ms";
            // 
            // nudEntryToSlowDown
            // 
            this.nudEntryToSlowDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nudEntryToSlowDown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudEntryToSlowDown.ForeColor = System.Drawing.SystemColors.Window;
            this.nudEntryToSlowDown.Location = new System.Drawing.Point(286, 74);
            this.nudEntryToSlowDown.Margin = new System.Windows.Forms.Padding(4);
            this.nudEntryToSlowDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.nudEntryToSlowDown.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudEntryToSlowDown.Name = "nudEntryToSlowDown";
            this.nudEntryToSlowDown.Size = new System.Drawing.Size(93, 25);
            this.nudEntryToSlowDown.TabIndex = 175;
            this.nudEntryToSlowDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudEntryToSlowDown.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // lblEntryToSlowDownTimeout
            // 
            this.lblEntryToSlowDownTimeout.AutoSize = true;
            this.lblEntryToSlowDownTimeout.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblEntryToSlowDownTimeout.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblEntryToSlowDownTimeout.Location = new System.Drawing.Point(25, 79);
            this.lblEntryToSlowDownTimeout.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblEntryToSlowDownTimeout.Name = "lblEntryToSlowDownTimeout";
            this.lblEntryToSlowDownTimeout.Size = new System.Drawing.Size(140, 20);
            this.lblEntryToSlowDownTimeout.TabIndex = 174;
            this.lblEntryToSlowDownTimeout.Text = "Entry to slow down";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(389, 78);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 21);
            this.label3.TabIndex = 173;
            this.label3.Text = "ms";
            // 
            // lblConveyorSetting
            // 
            this.lblConveyorSetting.AutoSize = true;
            this.lblConveyorSetting.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblConveyorSetting.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblConveyorSetting.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblConveyorSetting.Location = new System.Drawing.Point(0, 0);
            this.lblConveyorSetting.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblConveyorSetting.Name = "lblConveyorSetting";
            this.lblConveyorSetting.Size = new System.Drawing.Size(234, 20);
            this.lblConveyorSetting.TabIndex = 172;
            this.lblConveyorSetting.Text = "Conveyor action time out setting";
            // 
            // lblReviewMode
            // 
            this.lblReviewMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.lblReviewMode.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblReviewMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblReviewMode.Location = new System.Drawing.Point(26, 31);
            this.lblReviewMode.Margin = new System.Windows.Forms.Padding(5);
            this.lblReviewMode.Name = "lblReviewMode";
            this.lblReviewMode.Size = new System.Drawing.Size(105, 24);
            this.lblReviewMode.TabIndex = 176;
            this.lblReviewMode.Text = "Review mode";
            this.lblReviewMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tgsReviewMode
            // 
            this.tgsReviewMode.AutoSize = true;
            this.tgsReviewMode.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.tgsReviewMode.BaseColorA = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.tgsReviewMode.BaseColorB = System.Drawing.Color.SkyBlue;
            this.tgsReviewMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tgsReviewMode.HeadColorA = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.tgsReviewMode.HeadColorB = System.Drawing.Color.White;
            this.tgsReviewMode.HeadColorC = System.Drawing.Color.LawnGreen;
            this.tgsReviewMode.HeadColorD = System.Drawing.Color.LawnGreen;
            this.tgsReviewMode.Location = new System.Drawing.Point(139, 31);
            this.tgsReviewMode.Name = "tgsReviewMode";
            this.tgsReviewMode.Size = new System.Drawing.Size(48, 20);
            this.tgsReviewMode.TabIndex = 175;
            this.tgsReviewMode.Text = "hopeToggle1";
            this.tgsReviewMode.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.lblInspectOption);
            this.panel2.Controls.Add(this.lblUseSmema);
            this.panel2.Controls.Add(this.lblReviewMode);
            this.panel2.Controls.Add(this.tgsUseSmema);
            this.panel2.Controls.Add(this.tgsReviewMode);
            this.panel2.Location = new System.Drawing.Point(470, 93);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(228, 248);
            this.panel2.TabIndex = 189;
            // 
            // lblInspectOption
            // 
            this.lblInspectOption.AutoSize = true;
            this.lblInspectOption.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInspectOption.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblInspectOption.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblInspectOption.Location = new System.Drawing.Point(0, 0);
            this.lblInspectOption.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblInspectOption.Name = "lblInspectOption";
            this.lblInspectOption.Size = new System.Drawing.Size(107, 20);
            this.lblInspectOption.TabIndex = 172;
            this.lblInspectOption.Text = "Inspect option";
            // 
            // lblUseSmema
            // 
            this.lblUseSmema.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.lblUseSmema.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblUseSmema.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblUseSmema.Location = new System.Drawing.Point(26, 206);
            this.lblUseSmema.Margin = new System.Windows.Forms.Padding(5);
            this.lblUseSmema.Name = "lblUseSmema";
            this.lblUseSmema.Size = new System.Drawing.Size(105, 24);
            this.lblUseSmema.TabIndex = 176;
            this.lblUseSmema.Text = "Use SMEMA";
            this.lblUseSmema.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tgsUseSmema
            // 
            this.tgsUseSmema.AutoSize = true;
            this.tgsUseSmema.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.tgsUseSmema.BaseColorA = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.tgsUseSmema.BaseColorB = System.Drawing.Color.SkyBlue;
            this.tgsUseSmema.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tgsUseSmema.HeadColorA = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.tgsUseSmema.HeadColorB = System.Drawing.Color.White;
            this.tgsUseSmema.HeadColorC = System.Drawing.Color.LawnGreen;
            this.tgsUseSmema.HeadColorD = System.Drawing.Color.LawnGreen;
            this.tgsUseSmema.Location = new System.Drawing.Point(139, 206);
            this.tgsUseSmema.Name = "tgsUseSmema";
            this.tgsUseSmema.Size = new System.Drawing.Size(48, 20);
            this.tgsUseSmema.TabIndex = 175;
            this.tgsUseSmema.Text = "hopeToggle1";
            this.tgsUseSmema.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.btnResultSet);
            this.panel3.Controls.Add(this.txtResultPath);
            this.panel3.Controls.Add(this.lblInspResultPath);
            this.panel3.Controls.Add(this.lblResultPath);
            this.panel3.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
            this.panel3.Location = new System.Drawing.Point(9, 346);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(455, 130);
            this.panel3.TabIndex = 190;
            // 
            // btnResultSet
            // 
            this.btnResultSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResultSet.Location = new System.Drawing.Point(385, 35);
            this.btnResultSet.Name = "btnResultSet";
            this.btnResultSet.Size = new System.Drawing.Size(55, 27);
            this.btnResultSet.TabIndex = 191;
            this.btnResultSet.Text = "...";
            this.btnResultSet.UseVisualStyleBackColor = true;
            this.btnResultSet.Click += new System.EventHandler(this.btnResultSet_Click);
            // 
            // txtResultPath
            // 
            this.txtResultPath.Location = new System.Drawing.Point(127, 61);
            this.txtResultPath.Name = "txtResultPath";
            this.txtResultPath.Size = new System.Drawing.Size(252, 27);
            this.txtResultPath.TabIndex = 177;
            // 
            // lblInspResultPath
            // 
            this.lblInspResultPath.AutoSize = true;
            this.lblInspResultPath.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInspResultPath.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblInspResultPath.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblInspResultPath.Location = new System.Drawing.Point(0, 0);
            this.lblInspResultPath.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblInspResultPath.Name = "lblInspResultPath";
            this.lblInspResultPath.Size = new System.Drawing.Size(135, 20);
            this.lblInspResultPath.TabIndex = 172;
            this.lblInspResultPath.Text = "Inspect result path";
            // 
            // lblResultPath
            // 
            this.lblResultPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.lblResultPath.Font = new System.Drawing.Font("맑은 고딕", 11.25F);
            this.lblResultPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblResultPath.Location = new System.Drawing.Point(21, 46);
            this.lblResultPath.Margin = new System.Windows.Forms.Padding(5);
            this.lblResultPath.Name = "lblResultPath";
            this.lblResultPath.Size = new System.Drawing.Size(91, 48);
            this.lblResultPath.TabIndex = 176;
            this.lblResultPath.Text = "ResultPath ";
            this.lblResultPath.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_MotionSetting
            // 
            this.button_MotionSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_MotionSetting.Location = new System.Drawing.Point(9, 502);
            this.button_MotionSetting.Name = "button_MotionSetting";
            this.button_MotionSetting.Size = new System.Drawing.Size(106, 53);
            this.button_MotionSetting.TabIndex = 191;
            this.button_MotionSetting.Text = "Motion";
            this.button_MotionSetting.UseVisualStyleBackColor = true;
            this.button_MotionSetting.Click += new System.EventHandler(this.button_MotionSetting_Click);
            // 
            // UserConfigPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(59)))), ((int)(((byte)(68)))));
            this.Controls.Add(this.button_MotionSetting);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnApplyConfig);
            this.Controls.Add(this.nudLightStableTime);
            this.Controls.Add(this.labelMs);
            this.Controls.Add(this.lblLightStableTime);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "UserConfigPage";
            this.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Size = new System.Drawing.Size(1300, 1307);
            ((System.ComponentModel.ISupportInitialize)(this.nudLightStableTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudBeforMachineToEntry)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFlushTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEjectToNextMachine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInspectReadyToEject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSlowdownToInspectReady)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEntryToSlowDown)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudLightStableTime;
        private System.Windows.Forms.Label labelMs;
        private System.Windows.Forms.Label lblLightStableTime;
        private System.Windows.Forms.NumericUpDown nudBeforMachineToEntry;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblBoardFormRearTimeout;
        private System.Windows.Forms.Button btnApplyConfig;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown nudInspectReadyToEject;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudSlowdownToInspectReady;
        private System.Windows.Forms.Label lblEntryToInsepctionReadyTimeout;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudEntryToSlowDown;
        private System.Windows.Forms.Label lblEntryToSlowDownTimeout;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblConveyorSetting;
        private System.Windows.Forms.Label lblEjectToNextMachine;
        private System.Windows.Forms.Label lblInspectReadyToEject;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudEjectToNextMachine;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudFlushTimeout;
        private System.Windows.Forms.Label lblFlushTimeout;
        private System.Windows.Forms.Label lblReviewMode;
        private ReaLTaiizor.HopeToggle tgsReviewMode;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblInspectOption;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnResultSet;
        private System.Windows.Forms.TextBox txtResultPath;
        private System.Windows.Forms.Label lblInspResultPath;
        private System.Windows.Forms.Label lblResultPath;
        private System.Windows.Forms.Label lblUseSmema;
        private ReaLTaiizor.HopeToggle tgsUseSmema;
        private System.Windows.Forms.Button button_MotionSetting;
    }
}
