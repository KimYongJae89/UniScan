namespace AlphaMotionMove
{
    partial class MainForm
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.runInTime = new System.Windows.Forms.NumericUpDown();
            this.stopInTime = new System.Windows.Forms.NumericUpDown();
            this.infoIoList = new System.Windows.Forms.CheckedListBox();
            this.infoIo = new System.Windows.Forms.TextBox();
            this.runInTimeUnit = new System.Windows.Forms.Label();
            this.infoSpeed = new System.Windows.Forms.TextBox();
            this.stopInTimeUnit = new System.Windows.Forms.Label();
            this.autoStopCnt = new System.Windows.Forms.TextBox();
            this.infoTimes = new System.Windows.Forms.TextBox();
            this.infoDistance = new System.Windows.Forms.TextBox();
            this.useRunIn = new System.Windows.Forms.CheckBox();
            this.infoTime = new System.Windows.Forms.TextBox();
            this.labelRunning = new System.Windows.Forms.Label();
            this.labelAutoStopCnt = new System.Windows.Forms.Label();
            this.useStopIn = new System.Windows.Forms.CheckBox();
            this.labelInfoTimes = new System.Windows.Forms.Label();
            this.labelInfoIo = new System.Windows.Forms.Label();
            this.labelInfoSpeed = new System.Windows.Forms.Label();
            this.labelAutoStopCntUnit = new System.Windows.Forms.Label();
            this.labelInfoDistance = new System.Windows.Forms.Label();
            this.labelInfoTimesUnit = new System.Windows.Forms.Label();
            this.labelInfoSpeedUnit = new System.Windows.Forms.Label();
            this.labelInfoDistanceUnit = new System.Windows.Forms.Label();
            this.labelInfoTime = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.endPosition = new System.Windows.Forms.NumericUpDown();
            this.labelEndDelay = new System.Windows.Forms.Label();
            this.labelEndIo = new System.Windows.Forms.Label();
            this.labelEndPosition = new System.Windows.Forms.Label();
            this.labelEndDelayUnit = new System.Windows.Forms.Label();
            this.labelEndPositionUnit = new System.Windows.Forms.Label();
            this.endIo = new System.Windows.Forms.CheckBox();
            this.endDelay = new System.Windows.Forms.NumericUpDown();
            this.buttonMove = new System.Windows.Forms.Button();
            this.groupBoxStart = new System.Windows.Forms.GroupBox();
            this.startPosition = new System.Windows.Forms.NumericUpDown();
            this.labelStartDelay = new System.Windows.Forms.Label();
            this.labelStartIo = new System.Windows.Forms.Label();
            this.labelStartPosition = new System.Windows.Forms.Label();
            this.labelStartDelayUnit = new System.Windows.Forms.Label();
            this.labelStartPositionUnit = new System.Windows.Forms.Label();
            this.startIo = new System.Windows.Forms.CheckBox();
            this.startDelay = new System.Windows.Forms.NumericUpDown();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.labelBackwardSpd = new System.Windows.Forms.Label();
            this.backwardAcc = new System.Windows.Forms.NumericUpDown();
            this.labelBackwardIo = new System.Windows.Forms.Label();
            this.labelBackwardAcc = new System.Windows.Forms.Label();
            this.backwardSpd = new System.Windows.Forms.NumericUpDown();
            this.labelBackwardAccUnit = new System.Windows.Forms.Label();
            this.labelBackwardSpdUnit = new System.Windows.Forms.Label();
            this.backwardIo = new System.Windows.Forms.CheckBox();
            this.buttonEMG = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelForwardSpd = new System.Windows.Forms.Label();
            this.forwardAcc = new System.Windows.Forms.NumericUpDown();
            this.labelForwardIo = new System.Windows.Forms.Label();
            this.labelForwardAcc = new System.Windows.Forms.Label();
            this.forwardSpd = new System.Windows.Forms.NumericUpDown();
            this.labelForwardAccUnit = new System.Windows.Forms.Label();
            this.labelForwardSpdUnit = new System.Windows.Forms.Label();
            this.forwardIo = new System.Windows.Forms.CheckBox();
            this.buttonServo = new System.Windows.Forms.Button();
            this.timerUiUpdate = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runInTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopInTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endDelay)).BeginInit();
            this.groupBoxStart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startDelay)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backwardAcc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backwardSpd)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.forwardAcc)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.forwardSpd)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.runInTime);
            this.groupBox1.Controls.Add(this.stopInTime);
            this.groupBox1.Controls.Add(this.infoIoList);
            this.groupBox1.Controls.Add(this.infoIo);
            this.groupBox1.Controls.Add(this.runInTimeUnit);
            this.groupBox1.Controls.Add(this.infoSpeed);
            this.groupBox1.Controls.Add(this.stopInTimeUnit);
            this.groupBox1.Controls.Add(this.autoStopCnt);
            this.groupBox1.Controls.Add(this.infoTimes);
            this.groupBox1.Controls.Add(this.infoDistance);
            this.groupBox1.Controls.Add(this.useRunIn);
            this.groupBox1.Controls.Add(this.infoTime);
            this.groupBox1.Controls.Add(this.labelRunning);
            this.groupBox1.Controls.Add(this.labelAutoStopCnt);
            this.groupBox1.Controls.Add(this.useStopIn);
            this.groupBox1.Controls.Add(this.labelInfoTimes);
            this.groupBox1.Controls.Add(this.labelInfoIo);
            this.groupBox1.Controls.Add(this.labelInfoSpeed);
            this.groupBox1.Controls.Add(this.labelAutoStopCntUnit);
            this.groupBox1.Controls.Add(this.labelInfoDistance);
            this.groupBox1.Controls.Add(this.labelInfoTimesUnit);
            this.groupBox1.Controls.Add(this.labelInfoSpeedUnit);
            this.groupBox1.Controls.Add(this.labelInfoDistanceUnit);
            this.groupBox1.Controls.Add(this.labelInfoTime);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.buttonMove);
            this.groupBox1.Controls.Add(this.groupBoxStart);
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Controls.Add(this.buttonEMG);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.buttonServo);
            this.groupBox1.Font = new System.Drawing.Font("맑은 고딕", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(11, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(690, 496);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Motion";
            // 
            // runInTime
            // 
            this.runInTime.DecimalPlaces = 1;
            this.runInTime.Location = new System.Drawing.Point(578, 75);
            this.runInTime.Name = "runInTime";
            this.runInTime.Size = new System.Drawing.Size(81, 32);
            this.runInTime.TabIndex = 13;
            // 
            // stopInTime
            // 
            this.stopInTime.DecimalPlaces = 1;
            this.stopInTime.Location = new System.Drawing.Point(578, 40);
            this.stopInTime.Name = "stopInTime";
            this.stopInTime.Size = new System.Drawing.Size(81, 32);
            this.stopInTime.TabIndex = 13;
            // 
            // infoIoList
            // 
            this.infoIoList.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.infoIoList.FormattingEnabled = true;
            this.infoIoList.Items.AddRange(new object[] {
            "a",
            "b",
            "c"});
            this.infoIoList.Location = new System.Drawing.Point(15, 395);
            this.infoIoList.Name = "infoIoList";
            this.infoIoList.Size = new System.Drawing.Size(216, 70);
            this.infoIoList.TabIndex = 18;
            this.infoIoList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox1_ItemCheck);
            // 
            // infoIo
            // 
            this.infoIo.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.infoIo.Location = new System.Drawing.Point(158, 352);
            this.infoIo.Name = "infoIo";
            this.infoIo.ReadOnly = true;
            this.infoIo.Size = new System.Drawing.Size(73, 29);
            this.infoIo.TabIndex = 17;
            this.infoIo.Text = "0x0000";
            this.infoIo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // runInTimeUnit
            // 
            this.runInTimeUnit.AutoSize = true;
            this.runInTimeUnit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.runInTimeUnit.Location = new System.Drawing.Point(657, 88);
            this.runInTimeUnit.Name = "runInTimeUnit";
            this.runInTimeUnit.Size = new System.Drawing.Size(15, 19);
            this.runInTimeUnit.TabIndex = 12;
            this.runInTimeUnit.Text = "s";
            // 
            // infoSpeed
            // 
            this.infoSpeed.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.infoSpeed.Location = new System.Drawing.Point(561, 457);
            this.infoSpeed.Name = "infoSpeed";
            this.infoSpeed.ReadOnly = true;
            this.infoSpeed.Size = new System.Drawing.Size(73, 29);
            this.infoSpeed.TabIndex = 17;
            this.infoSpeed.Text = "000";
            this.infoSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // stopInTimeUnit
            // 
            this.stopInTimeUnit.AutoSize = true;
            this.stopInTimeUnit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.stopInTimeUnit.Location = new System.Drawing.Point(657, 53);
            this.stopInTimeUnit.Name = "stopInTimeUnit";
            this.stopInTimeUnit.Size = new System.Drawing.Size(15, 19);
            this.stopInTimeUnit.TabIndex = 12;
            this.stopInTimeUnit.Text = "s";
            // 
            // autoStopCnt
            // 
            this.autoStopCnt.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.autoStopCnt.Location = new System.Drawing.Point(578, 113);
            this.autoStopCnt.Name = "autoStopCnt";
            this.autoStopCnt.ReadOnly = true;
            this.autoStopCnt.Size = new System.Drawing.Size(56, 29);
            this.autoStopCnt.TabIndex = 17;
            this.autoStopCnt.Text = "000";
            this.autoStopCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // infoTimes
            // 
            this.infoTimes.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.infoTimes.Location = new System.Drawing.Point(561, 387);
            this.infoTimes.Name = "infoTimes";
            this.infoTimes.ReadOnly = true;
            this.infoTimes.Size = new System.Drawing.Size(73, 29);
            this.infoTimes.TabIndex = 17;
            this.infoTimes.Text = "000";
            this.infoTimes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // infoDistance
            // 
            this.infoDistance.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.infoDistance.Location = new System.Drawing.Point(561, 422);
            this.infoDistance.Name = "infoDistance";
            this.infoDistance.ReadOnly = true;
            this.infoDistance.Size = new System.Drawing.Size(73, 29);
            this.infoDistance.TabIndex = 17;
            this.infoDistance.Text = "000";
            this.infoDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // useRunIn
            // 
            this.useRunIn.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.useRunIn.Location = new System.Drawing.Point(469, 75);
            this.useRunIn.Name = "useRunIn";
            this.useRunIn.Size = new System.Drawing.Size(97, 32);
            this.useRunIn.TabIndex = 0;
            this.useRunIn.Text = "Run  In";
            this.useRunIn.UseVisualStyleBackColor = true;
            // 
            // infoTime
            // 
            this.infoTime.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.infoTime.Location = new System.Drawing.Point(526, 352);
            this.infoTime.Name = "infoTime";
            this.infoTime.ReadOnly = true;
            this.infoTime.Size = new System.Drawing.Size(147, 29);
            this.infoTime.TabIndex = 17;
            this.infoTime.Text = "00 H 00 m 00 s";
            this.infoTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelRunning
            // 
            this.labelRunning.Font = new System.Drawing.Font("맑은 고딕", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelRunning.Location = new System.Drawing.Point(11, 117);
            this.labelRunning.Name = "labelRunning";
            this.labelRunning.Size = new System.Drawing.Size(208, 58);
            this.labelRunning.TabIndex = 9;
            this.labelRunning.Text = "Running";
            this.labelRunning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAutoStopCnt
            // 
            this.labelAutoStopCnt.AutoSize = true;
            this.labelAutoStopCnt.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelAutoStopCnt.Location = new System.Drawing.Point(478, 117);
            this.labelAutoStopCnt.Name = "labelAutoStopCnt";
            this.labelAutoStopCnt.Size = new System.Drawing.Size(88, 21);
            this.labelAutoStopCnt.TabIndex = 9;
            this.labelAutoStopCnt.Text = "Auto Stop";
            // 
            // useStopIn
            // 
            this.useStopIn.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.useStopIn.Location = new System.Drawing.Point(469, 40);
            this.useStopIn.Name = "useStopIn";
            this.useStopIn.Size = new System.Drawing.Size(97, 32);
            this.useStopIn.TabIndex = 0;
            this.useStopIn.Text = "Stop In";
            this.useStopIn.UseVisualStyleBackColor = true;
            // 
            // labelInfoTimes
            // 
            this.labelInfoTimes.AutoSize = true;
            this.labelInfoTimes.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelInfoTimes.Location = new System.Drawing.Point(464, 389);
            this.labelInfoTimes.Name = "labelInfoTimes";
            this.labelInfoTimes.Size = new System.Drawing.Size(65, 25);
            this.labelInfoTimes.TabIndex = 9;
            this.labelInfoTimes.Text = "Times";
            // 
            // labelInfoIo
            // 
            this.labelInfoIo.AutoSize = true;
            this.labelInfoIo.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelInfoIo.Location = new System.Drawing.Point(10, 354);
            this.labelInfoIo.Name = "labelInfoIo";
            this.labelInfoIo.Size = new System.Drawing.Size(113, 25);
            this.labelInfoIo.TabIndex = 9;
            this.labelInfoIo.Text = "I/O Output";
            // 
            // labelInfoSpeed
            // 
            this.labelInfoSpeed.AutoSize = true;
            this.labelInfoSpeed.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelInfoSpeed.Location = new System.Drawing.Point(464, 459);
            this.labelInfoSpeed.Name = "labelInfoSpeed";
            this.labelInfoSpeed.Size = new System.Drawing.Size(67, 25);
            this.labelInfoSpeed.TabIndex = 9;
            this.labelInfoSpeed.Text = "Speed";
            // 
            // labelAutoStopCntUnit
            // 
            this.labelAutoStopCntUnit.AutoSize = true;
            this.labelAutoStopCntUnit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelAutoStopCntUnit.Location = new System.Drawing.Point(636, 123);
            this.labelAutoStopCntUnit.Name = "labelAutoStopCntUnit";
            this.labelAutoStopCntUnit.Size = new System.Drawing.Size(48, 19);
            this.labelAutoStopCntUnit.TabIndex = 12;
            this.labelAutoStopCntUnit.Text = "Times";
            // 
            // labelInfoDistance
            // 
            this.labelInfoDistance.AutoSize = true;
            this.labelInfoDistance.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelInfoDistance.Location = new System.Drawing.Point(464, 424);
            this.labelInfoDistance.Name = "labelInfoDistance";
            this.labelInfoDistance.Size = new System.Drawing.Size(88, 25);
            this.labelInfoDistance.TabIndex = 9;
            this.labelInfoDistance.Text = "Distance";
            // 
            // labelInfoTimesUnit
            // 
            this.labelInfoTimesUnit.AutoSize = true;
            this.labelInfoTimesUnit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelInfoTimesUnit.Location = new System.Drawing.Point(634, 397);
            this.labelInfoTimesUnit.Name = "labelInfoTimesUnit";
            this.labelInfoTimesUnit.Size = new System.Drawing.Size(27, 19);
            this.labelInfoTimesUnit.TabIndex = 12;
            this.labelInfoTimesUnit.Text = "EA";
            // 
            // labelInfoSpeedUnit
            // 
            this.labelInfoSpeedUnit.AutoSize = true;
            this.labelInfoSpeedUnit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelInfoSpeedUnit.Location = new System.Drawing.Point(634, 467);
            this.labelInfoSpeedUnit.Name = "labelInfoSpeedUnit";
            this.labelInfoSpeedUnit.Size = new System.Drawing.Size(41, 19);
            this.labelInfoSpeedUnit.TabIndex = 12;
            this.labelInfoSpeedUnit.Text = "m/m";
            // 
            // labelInfoDistanceUnit
            // 
            this.labelInfoDistanceUnit.AutoSize = true;
            this.labelInfoDistanceUnit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelInfoDistanceUnit.Location = new System.Drawing.Point(634, 432);
            this.labelInfoDistanceUnit.Name = "labelInfoDistanceUnit";
            this.labelInfoDistanceUnit.Size = new System.Drawing.Size(22, 19);
            this.labelInfoDistanceUnit.TabIndex = 12;
            this.labelInfoDistanceUnit.Text = "m";
            // 
            // labelInfoTime
            // 
            this.labelInfoTime.AutoSize = true;
            this.labelInfoTime.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelInfoTime.Location = new System.Drawing.Point(464, 354);
            this.labelInfoTime.Name = "labelInfoTime";
            this.labelInfoTime.Size = new System.Drawing.Size(56, 25);
            this.labelInfoTime.TabIndex = 9;
            this.labelInfoTime.Text = "Time";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(237, 181);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(216, 150);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 16;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.endPosition);
            this.groupBox4.Controls.Add(this.labelEndDelay);
            this.groupBox4.Controls.Add(this.labelEndIo);
            this.groupBox4.Controls.Add(this.labelEndPosition);
            this.groupBox4.Controls.Add(this.labelEndDelayUnit);
            this.groupBox4.Controls.Add(this.labelEndPositionUnit);
            this.groupBox4.Controls.Add(this.endIo);
            this.groupBox4.Controls.Add(this.endDelay);
            this.groupBox4.Location = new System.Drawing.Point(459, 181);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(216, 150);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "End";
            // 
            // endPosition
            // 
            this.endPosition.Location = new System.Drawing.Point(83, 32);
            this.endPosition.Name = "endPosition";
            this.endPosition.Size = new System.Drawing.Size(81, 32);
            this.endPosition.TabIndex = 13;
            // 
            // labelEndDelay
            // 
            this.labelEndDelay.AutoSize = true;
            this.labelEndDelay.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelEndDelay.Location = new System.Drawing.Point(24, 75);
            this.labelEndDelay.Name = "labelEndDelay";
            this.labelEndDelay.Size = new System.Drawing.Size(53, 21);
            this.labelEndDelay.TabIndex = 10;
            this.labelEndDelay.Text = "Delay";
            // 
            // labelEndIo
            // 
            this.labelEndIo.AutoSize = true;
            this.labelEndIo.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelEndIo.Location = new System.Drawing.Point(43, 114);
            this.labelEndIo.Name = "labelEndIo";
            this.labelEndIo.Size = new System.Drawing.Size(34, 21);
            this.labelEndIo.TabIndex = 10;
            this.labelEndIo.Text = "I/O";
            // 
            // labelEndPosition
            // 
            this.labelEndPosition.AutoSize = true;
            this.labelEndPosition.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelEndPosition.Location = new System.Drawing.Point(6, 38);
            this.labelEndPosition.Name = "labelEndPosition";
            this.labelEndPosition.Size = new System.Drawing.Size(71, 21);
            this.labelEndPosition.TabIndex = 10;
            this.labelEndPosition.Text = "Position";
            // 
            // labelEndDelayUnit
            // 
            this.labelEndDelayUnit.AutoSize = true;
            this.labelEndDelayUnit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelEndDelayUnit.Location = new System.Drawing.Point(162, 83);
            this.labelEndDelayUnit.Name = "labelEndDelayUnit";
            this.labelEndDelayUnit.Size = new System.Drawing.Size(28, 19);
            this.labelEndDelayUnit.TabIndex = 12;
            this.labelEndDelayUnit.Text = "ms";
            // 
            // labelEndPositionUnit
            // 
            this.labelEndPositionUnit.AutoSize = true;
            this.labelEndPositionUnit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelEndPositionUnit.Location = new System.Drawing.Point(162, 45);
            this.labelEndPositionUnit.Name = "labelEndPositionUnit";
            this.labelEndPositionUnit.Size = new System.Drawing.Size(35, 19);
            this.labelEndPositionUnit.TabIndex = 12;
            this.labelEndPositionUnit.Text = "mm";
            // 
            // endIo
            // 
            this.endIo.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.endIo.Location = new System.Drawing.Point(83, 108);
            this.endIo.Name = "endIo";
            this.endIo.Size = new System.Drawing.Size(81, 32);
            this.endIo.TabIndex = 0;
            this.endIo.UseVisualStyleBackColor = true;
            // 
            // endDelay
            // 
            this.endDelay.Location = new System.Drawing.Point(83, 70);
            this.endDelay.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.endDelay.Name = "endDelay";
            this.endDelay.Size = new System.Drawing.Size(81, 32);
            this.endDelay.TabIndex = 13;
            // 
            // buttonMove
            // 
            this.buttonMove.Location = new System.Drawing.Point(15, 78);
            this.buttonMove.Name = "buttonMove";
            this.buttonMove.Size = new System.Drawing.Size(99, 32);
            this.buttonMove.TabIndex = 15;
            this.buttonMove.Text = "Move";
            this.buttonMove.UseVisualStyleBackColor = true;
            this.buttonMove.Click += new System.EventHandler(this.buttonMove_Click);
            // 
            // groupBoxStart
            // 
            this.groupBoxStart.Controls.Add(this.startPosition);
            this.groupBoxStart.Controls.Add(this.labelStartDelay);
            this.groupBoxStart.Controls.Add(this.labelStartIo);
            this.groupBoxStart.Controls.Add(this.labelStartPosition);
            this.groupBoxStart.Controls.Add(this.labelStartDelayUnit);
            this.groupBoxStart.Controls.Add(this.labelStartPositionUnit);
            this.groupBoxStart.Controls.Add(this.startIo);
            this.groupBoxStart.Controls.Add(this.startDelay);
            this.groupBoxStart.Location = new System.Drawing.Point(15, 181);
            this.groupBoxStart.Name = "groupBoxStart";
            this.groupBoxStart.Size = new System.Drawing.Size(216, 150);
            this.groupBoxStart.TabIndex = 14;
            this.groupBoxStart.TabStop = false;
            this.groupBoxStart.Text = "Start";
            // 
            // startPosition
            // 
            this.startPosition.Location = new System.Drawing.Point(83, 32);
            this.startPosition.Name = "startPosition";
            this.startPosition.Size = new System.Drawing.Size(81, 32);
            this.startPosition.TabIndex = 13;
            // 
            // labelStartDelay
            // 
            this.labelStartDelay.AutoSize = true;
            this.labelStartDelay.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelStartDelay.Location = new System.Drawing.Point(24, 75);
            this.labelStartDelay.Name = "labelStartDelay";
            this.labelStartDelay.Size = new System.Drawing.Size(53, 21);
            this.labelStartDelay.TabIndex = 10;
            this.labelStartDelay.Text = "Delay";
            // 
            // labelStartIo
            // 
            this.labelStartIo.AutoSize = true;
            this.labelStartIo.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelStartIo.Location = new System.Drawing.Point(43, 114);
            this.labelStartIo.Name = "labelStartIo";
            this.labelStartIo.Size = new System.Drawing.Size(34, 21);
            this.labelStartIo.TabIndex = 10;
            this.labelStartIo.Text = "I/O";
            // 
            // labelStartPosition
            // 
            this.labelStartPosition.AutoSize = true;
            this.labelStartPosition.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelStartPosition.Location = new System.Drawing.Point(6, 38);
            this.labelStartPosition.Name = "labelStartPosition";
            this.labelStartPosition.Size = new System.Drawing.Size(71, 21);
            this.labelStartPosition.TabIndex = 10;
            this.labelStartPosition.Text = "Position";
            // 
            // labelStartDelayUnit
            // 
            this.labelStartDelayUnit.AutoSize = true;
            this.labelStartDelayUnit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelStartDelayUnit.Location = new System.Drawing.Point(162, 83);
            this.labelStartDelayUnit.Name = "labelStartDelayUnit";
            this.labelStartDelayUnit.Size = new System.Drawing.Size(28, 19);
            this.labelStartDelayUnit.TabIndex = 12;
            this.labelStartDelayUnit.Text = "ms";
            // 
            // labelStartPositionUnit
            // 
            this.labelStartPositionUnit.AutoSize = true;
            this.labelStartPositionUnit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelStartPositionUnit.Location = new System.Drawing.Point(162, 45);
            this.labelStartPositionUnit.Name = "labelStartPositionUnit";
            this.labelStartPositionUnit.Size = new System.Drawing.Size(35, 19);
            this.labelStartPositionUnit.TabIndex = 12;
            this.labelStartPositionUnit.Text = "mm";
            // 
            // startIo
            // 
            this.startIo.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.startIo.Location = new System.Drawing.Point(83, 108);
            this.startIo.Name = "startIo";
            this.startIo.Size = new System.Drawing.Size(81, 32);
            this.startIo.TabIndex = 0;
            this.startIo.UseVisualStyleBackColor = true;
            // 
            // startDelay
            // 
            this.startDelay.Location = new System.Drawing.Point(83, 70);
            this.startDelay.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.startDelay.Name = "startDelay";
            this.startDelay.Size = new System.Drawing.Size(81, 32);
            this.startDelay.TabIndex = 13;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.labelBackwardSpd);
            this.groupBox6.Controls.Add(this.backwardAcc);
            this.groupBox6.Controls.Add(this.labelBackwardIo);
            this.groupBox6.Controls.Add(this.labelBackwardAcc);
            this.groupBox6.Controls.Add(this.backwardSpd);
            this.groupBox6.Controls.Add(this.labelBackwardAccUnit);
            this.groupBox6.Controls.Add(this.labelBackwardSpdUnit);
            this.groupBox6.Controls.Add(this.backwardIo);
            this.groupBox6.Location = new System.Drawing.Point(237, 337);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(216, 150);
            this.groupBox6.TabIndex = 14;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "End -> Start";
            // 
            // labelBackwardSpd
            // 
            this.labelBackwardSpd.AutoSize = true;
            this.labelBackwardSpd.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelBackwardSpd.Location = new System.Drawing.Point(20, 38);
            this.labelBackwardSpd.Name = "labelBackwardSpd";
            this.labelBackwardSpd.Size = new System.Drawing.Size(57, 21);
            this.labelBackwardSpd.TabIndex = 7;
            this.labelBackwardSpd.Text = "Speed";
            // 
            // backwardAcc
            // 
            this.backwardAcc.Location = new System.Drawing.Point(83, 70);
            this.backwardAcc.Name = "backwardAcc";
            this.backwardAcc.Size = new System.Drawing.Size(81, 32);
            this.backwardAcc.TabIndex = 13;
            // 
            // labelBackwardIo
            // 
            this.labelBackwardIo.AutoSize = true;
            this.labelBackwardIo.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelBackwardIo.Location = new System.Drawing.Point(43, 114);
            this.labelBackwardIo.Name = "labelBackwardIo";
            this.labelBackwardIo.Size = new System.Drawing.Size(34, 21);
            this.labelBackwardIo.TabIndex = 10;
            this.labelBackwardIo.Text = "I/O";
            // 
            // labelBackwardAcc
            // 
            this.labelBackwardAcc.AutoSize = true;
            this.labelBackwardAcc.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelBackwardAcc.Location = new System.Drawing.Point(40, 75);
            this.labelBackwardAcc.Name = "labelBackwardAcc";
            this.labelBackwardAcc.Size = new System.Drawing.Size(37, 21);
            this.labelBackwardAcc.TabIndex = 10;
            this.labelBackwardAcc.Text = "Acc";
            // 
            // backwardSpd
            // 
            this.backwardSpd.Location = new System.Drawing.Point(83, 32);
            this.backwardSpd.Name = "backwardSpd";
            this.backwardSpd.Size = new System.Drawing.Size(81, 32);
            this.backwardSpd.TabIndex = 13;
            // 
            // labelBackwardAccUnit
            // 
            this.labelBackwardAccUnit.AutoSize = true;
            this.labelBackwardAccUnit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelBackwardAccUnit.Location = new System.Drawing.Point(162, 83);
            this.labelBackwardAccUnit.Name = "labelBackwardAccUnit";
            this.labelBackwardAccUnit.Size = new System.Drawing.Size(28, 19);
            this.labelBackwardAccUnit.TabIndex = 12;
            this.labelBackwardAccUnit.Text = "ms";
            // 
            // labelBackwardSpdUnit
            // 
            this.labelBackwardSpdUnit.AutoSize = true;
            this.labelBackwardSpdUnit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelBackwardSpdUnit.Location = new System.Drawing.Point(162, 45);
            this.labelBackwardSpdUnit.Name = "labelBackwardSpdUnit";
            this.labelBackwardSpdUnit.Size = new System.Drawing.Size(47, 19);
            this.labelBackwardSpdUnit.TabIndex = 9;
            this.labelBackwardSpdUnit.Text = "mm/s";
            // 
            // backwardIo
            // 
            this.backwardIo.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.backwardIo.Location = new System.Drawing.Point(83, 108);
            this.backwardIo.Name = "backwardIo";
            this.backwardIo.Size = new System.Drawing.Size(81, 32);
            this.backwardIo.TabIndex = 0;
            this.backwardIo.UseVisualStyleBackColor = true;
            // 
            // buttonEMG
            // 
            this.buttonEMG.Location = new System.Drawing.Point(120, 40);
            this.buttonEMG.Name = "buttonEMG";
            this.buttonEMG.Size = new System.Drawing.Size(99, 70);
            this.buttonEMG.TabIndex = 14;
            this.buttonEMG.Text = "OMG!";
            this.buttonEMG.UseVisualStyleBackColor = true;
            this.buttonEMG.Click += new System.EventHandler(this.buttonEMG_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelForwardSpd);
            this.groupBox3.Controls.Add(this.forwardAcc);
            this.groupBox3.Controls.Add(this.labelForwardIo);
            this.groupBox3.Controls.Add(this.labelForwardAcc);
            this.groupBox3.Controls.Add(this.forwardSpd);
            this.groupBox3.Controls.Add(this.labelForwardAccUnit);
            this.groupBox3.Controls.Add(this.labelForwardSpdUnit);
            this.groupBox3.Controls.Add(this.forwardIo);
            this.groupBox3.Location = new System.Drawing.Point(237, 25);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(216, 150);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Start -> End";
            // 
            // labelForwardSpd
            // 
            this.labelForwardSpd.AutoSize = true;
            this.labelForwardSpd.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelForwardSpd.Location = new System.Drawing.Point(20, 38);
            this.labelForwardSpd.Name = "labelForwardSpd";
            this.labelForwardSpd.Size = new System.Drawing.Size(57, 21);
            this.labelForwardSpd.TabIndex = 7;
            this.labelForwardSpd.Text = "Speed";
            // 
            // forwardAcc
            // 
            this.forwardAcc.Location = new System.Drawing.Point(83, 70);
            this.forwardAcc.Name = "forwardAcc";
            this.forwardAcc.Size = new System.Drawing.Size(81, 32);
            this.forwardAcc.TabIndex = 13;
            // 
            // labelForwardIo
            // 
            this.labelForwardIo.AutoSize = true;
            this.labelForwardIo.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelForwardIo.Location = new System.Drawing.Point(43, 114);
            this.labelForwardIo.Name = "labelForwardIo";
            this.labelForwardIo.Size = new System.Drawing.Size(34, 21);
            this.labelForwardIo.TabIndex = 10;
            this.labelForwardIo.Text = "I/O";
            // 
            // labelForwardAcc
            // 
            this.labelForwardAcc.AutoSize = true;
            this.labelForwardAcc.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelForwardAcc.Location = new System.Drawing.Point(40, 75);
            this.labelForwardAcc.Name = "labelForwardAcc";
            this.labelForwardAcc.Size = new System.Drawing.Size(37, 21);
            this.labelForwardAcc.TabIndex = 10;
            this.labelForwardAcc.Text = "Acc";
            // 
            // forwardSpd
            // 
            this.forwardSpd.Location = new System.Drawing.Point(83, 32);
            this.forwardSpd.Name = "forwardSpd";
            this.forwardSpd.Size = new System.Drawing.Size(81, 32);
            this.forwardSpd.TabIndex = 13;
            // 
            // labelForwardAccUnit
            // 
            this.labelForwardAccUnit.AutoSize = true;
            this.labelForwardAccUnit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelForwardAccUnit.Location = new System.Drawing.Point(162, 83);
            this.labelForwardAccUnit.Name = "labelForwardAccUnit";
            this.labelForwardAccUnit.Size = new System.Drawing.Size(28, 19);
            this.labelForwardAccUnit.TabIndex = 12;
            this.labelForwardAccUnit.Text = "ms";
            // 
            // labelForwardSpdUnit
            // 
            this.labelForwardSpdUnit.AutoSize = true;
            this.labelForwardSpdUnit.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelForwardSpdUnit.Location = new System.Drawing.Point(162, 45);
            this.labelForwardSpdUnit.Name = "labelForwardSpdUnit";
            this.labelForwardSpdUnit.Size = new System.Drawing.Size(47, 19);
            this.labelForwardSpdUnit.TabIndex = 9;
            this.labelForwardSpdUnit.Text = "mm/s";
            // 
            // forwardIo
            // 
            this.forwardIo.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.forwardIo.Location = new System.Drawing.Point(83, 112);
            this.forwardIo.Name = "forwardIo";
            this.forwardIo.Size = new System.Drawing.Size(81, 32);
            this.forwardIo.TabIndex = 0;
            this.forwardIo.UseVisualStyleBackColor = true;
            // 
            // buttonServo
            // 
            this.buttonServo.Location = new System.Drawing.Point(15, 40);
            this.buttonServo.Name = "buttonServo";
            this.buttonServo.Size = new System.Drawing.Size(99, 32);
            this.buttonServo.TabIndex = 13;
            this.buttonServo.Text = "ON/OFF";
            this.buttonServo.UseVisualStyleBackColor = true;
            this.buttonServo.Click += new System.EventHandler(this.buttonServo_Click);
            // 
            // timerUiUpdate
            // 
            this.timerUiUpdate.Enabled = true;
            this.timerUiUpdate.Tick += new System.EventHandler(this.timerUiUpdate_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 513);
            this.Controls.Add(this.groupBox1);
            this.Name = "MainForm";
            this.Text = "AlpahMotion Axis Mover";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.runInTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stopInTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endDelay)).EndInit();
            this.groupBoxStart.ResumeLayout(false);
            this.groupBoxStart.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startDelay)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.backwardAcc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backwardSpd)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.forwardAcc)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.forwardSpd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonEMG;
        private System.Windows.Forms.Button buttonServo;
        private System.Windows.Forms.Label labelForwardAccUnit;
        private System.Windows.Forms.Label labelForwardAcc;
        private System.Windows.Forms.Label labelForwardSpdUnit;
        private System.Windows.Forms.Label labelForwardSpd;
        private System.Windows.Forms.Button buttonMove;
        private System.Windows.Forms.Timer timerUiUpdate;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown endPosition;
        private System.Windows.Forms.Label labelEndDelay;
        private System.Windows.Forms.Label labelEndIo;
        private System.Windows.Forms.Label labelEndPosition;
        private System.Windows.Forms.Label labelEndDelayUnit;
        private System.Windows.Forms.Label labelEndPositionUnit;
        private System.Windows.Forms.CheckBox endIo;
        private System.Windows.Forms.NumericUpDown endDelay;
        private System.Windows.Forms.GroupBox groupBoxStart;
        private System.Windows.Forms.NumericUpDown startPosition;
        private System.Windows.Forms.Label labelStartDelay;
        private System.Windows.Forms.Label labelStartIo;
        private System.Windows.Forms.Label labelStartPosition;
        private System.Windows.Forms.Label labelStartDelayUnit;
        private System.Windows.Forms.Label labelStartPositionUnit;
        private System.Windows.Forms.CheckBox startIo;
        private System.Windows.Forms.NumericUpDown startDelay;
        private System.Windows.Forms.Label labelInfoSpeed;
        private System.Windows.Forms.Label labelInfoDistance;
        private System.Windows.Forms.Label labelInfoTime;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label labelBackwardSpd;
        private System.Windows.Forms.NumericUpDown backwardAcc;
        private System.Windows.Forms.Label labelBackwardIo;
        private System.Windows.Forms.Label labelBackwardAcc;
        private System.Windows.Forms.NumericUpDown backwardSpd;
        private System.Windows.Forms.Label labelBackwardAccUnit;
        private System.Windows.Forms.Label labelBackwardSpdUnit;
        private System.Windows.Forms.CheckBox backwardIo;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown forwardAcc;
        private System.Windows.Forms.Label labelForwardIo;
        private System.Windows.Forms.NumericUpDown forwardSpd;
        private System.Windows.Forms.CheckBox forwardIo;
        private System.Windows.Forms.TextBox infoSpeed;
        private System.Windows.Forms.TextBox infoDistance;
        private System.Windows.Forms.TextBox infoTime;
        private System.Windows.Forms.Label labelInfoSpeedUnit;
        private System.Windows.Forms.Label labelInfoDistanceUnit;
        private System.Windows.Forms.TextBox infoTimes;
        private System.Windows.Forms.Label labelInfoTimes;
        private System.Windows.Forms.Label labelInfoTimesUnit;
        private System.Windows.Forms.TextBox infoIo;
        private System.Windows.Forms.CheckedListBox infoIoList;
        private System.Windows.Forms.Label labelInfoIo;
        private System.Windows.Forms.NumericUpDown runInTime;
        private System.Windows.Forms.NumericUpDown stopInTime;
        private System.Windows.Forms.Label runInTimeUnit;
        private System.Windows.Forms.Label stopInTimeUnit;
        private System.Windows.Forms.CheckBox useRunIn;
        private System.Windows.Forms.CheckBox useStopIn;
        private System.Windows.Forms.TextBox autoStopCnt;
        private System.Windows.Forms.Label labelAutoStopCnt;
        private System.Windows.Forms.Label labelAutoStopCntUnit;
        private System.Windows.Forms.Label labelRunning;
    }
}

