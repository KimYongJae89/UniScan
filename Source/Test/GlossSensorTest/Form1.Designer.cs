namespace GlossSensorTest
{
    partial class Form1
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
            this.btnOnceMeasure = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnRepeat = new System.Windows.Forms.Button();
            this.MeasureCnt = new System.Windows.Forms.NumericUpDown();
            this.btnLongRunStart = new System.Windows.Forms.Button();
            this.btnLongRunStop = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtTilting = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSampleName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.endDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.startDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExport = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblValue = new System.Windows.Forms.Label();
            this.btnLaserOn = new System.Windows.Forms.Button();
            this.btnLaserOff = new System.Windows.Forms.Button();
            this.btnLaserMeasure = new System.Windows.Forms.Button();
            this.btnLaserInit = new System.Windows.Forms.Button();
            this.lblLaserValue = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.MeasureCnt)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOnceMeasure
            // 
            this.btnOnceMeasure.Location = new System.Drawing.Point(9, 20);
            this.btnOnceMeasure.Name = "btnOnceMeasure";
            this.btnOnceMeasure.Size = new System.Drawing.Size(101, 27);
            this.btnOnceMeasure.TabIndex = 5;
            this.btnOnceMeasure.Text = "OnceMeasure";
            this.btnOnceMeasure.UseVisualStyleBackColor = true;
            this.btnOnceMeasure.Click += new System.EventHandler(this.btnOnceMeasure_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(116, 20);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(101, 27);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(9, 20);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(101, 27);
            this.btnOpen.TabIndex = 3;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnRepeat
            // 
            this.btnRepeat.Location = new System.Drawing.Point(225, 22);
            this.btnRepeat.Name = "btnRepeat";
            this.btnRepeat.Size = new System.Drawing.Size(101, 27);
            this.btnRepeat.TabIndex = 6;
            this.btnRepeat.Text = "반복성 test";
            this.btnRepeat.UseVisualStyleBackColor = true;
            this.btnRepeat.Click += new System.EventHandler(this.btnRepeat_Click);
            // 
            // MeasureCnt
            // 
            this.MeasureCnt.Location = new System.Drawing.Point(106, 89);
            this.MeasureCnt.Name = "MeasureCnt";
            this.MeasureCnt.Size = new System.Drawing.Size(106, 21);
            this.MeasureCnt.TabIndex = 7;
            this.MeasureCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MeasureCnt.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnLongRunStart
            // 
            this.btnLongRunStart.Location = new System.Drawing.Point(116, 20);
            this.btnLongRunStart.Name = "btnLongRunStart";
            this.btnLongRunStart.Size = new System.Drawing.Size(101, 27);
            this.btnLongRunStart.TabIndex = 8;
            this.btnLongRunStart.Text = "LongRun Start";
            this.btnLongRunStart.UseVisualStyleBackColor = true;
            this.btnLongRunStart.Click += new System.EventHandler(this.btnLongRunStart_Click);
            // 
            // btnLongRunStop
            // 
            this.btnLongRunStop.Location = new System.Drawing.Point(223, 20);
            this.btnLongRunStop.Name = "btnLongRunStop";
            this.btnLongRunStop.Size = new System.Drawing.Size(101, 27);
            this.btnLongRunStop.TabIndex = 9;
            this.btnLongRunStop.Text = "LongRun Stop";
            this.btnLongRunStop.UseVisualStyleBackColor = true;
            this.btnLongRunStop.Click += new System.EventHandler(this.btnLongRunStop_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.Controls.Add(this.btnOpen);
            this.groupBox1.Location = new System.Drawing.Point(15, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(337, 58);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Initialize";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtTilting);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.btnRepeat);
            this.groupBox2.Controls.Add(this.txtSampleName);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.MeasureCnt);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(15, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(337, 129);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "RepeatTest";
            // 
            // txtTilting
            // 
            this.txtTilting.Location = new System.Drawing.Point(106, 49);
            this.txtTilting.Name = "txtTilting";
            this.txtTilting.Size = new System.Drawing.Size(106, 21);
            this.txtTilting.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(43, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "틸팅 각도";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // txtSampleName
            // 
            this.txtSampleName.Location = new System.Drawing.Point(106, 22);
            this.txtSampleName.Name = "txtSampleName";
            this.txtSampleName.Size = new System.Drawing.Size(106, 21);
            this.txtSampleName.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "측정 반복 횟수";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 12);
            this.label8.TabIndex = 11;
            this.label8.Text = "측정 시료 이름";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.groupBox1);
            this.groupBox3.Controls.Add(this.groupBox2);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Location = new System.Drawing.Point(25, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(369, 447);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "GlossSensor";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnLaserMeasure);
            this.groupBox4.Controls.Add(this.btnLaserInit);
            this.groupBox4.Controls.Add(this.btnLaserOn);
            this.groupBox4.Controls.Add(this.btnLaserOff);
            this.groupBox4.Location = new System.Drawing.Point(415, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(244, 133);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "LaserSensor";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.endDate);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.startDate);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.btnExport);
            this.groupBox5.Location = new System.Drawing.Point(15, 309);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(337, 126);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Export";
            // 
            // endDate
            // 
            this.endDate.Location = new System.Drawing.Point(20, 86);
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(192, 21);
            this.endDate.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 12);
            this.label2.TabIndex = 16;
            this.label2.Text = "End Date";
            // 
            // startDate
            // 
            this.startDate.Location = new System.Drawing.Point(20, 37);
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(192, 21);
            this.startDate.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "Start Date";
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(225, 37);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(101, 27);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(430, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "Gloss Value : ";
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblValue.Location = new System.Drawing.Point(522, 161);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(61, 21);
            this.lblValue.TabIndex = 15;
            this.lblValue.Text = "result";
            // 
            // btnLaserOn
            // 
            this.btnLaserOn.Location = new System.Drawing.Point(17, 83);
            this.btnLaserOn.Name = "btnLaserOn";
            this.btnLaserOn.Size = new System.Drawing.Size(101, 27);
            this.btnLaserOn.TabIndex = 5;
            this.btnLaserOn.Text = "Laser ON";
            this.btnLaserOn.UseVisualStyleBackColor = true;
            this.btnLaserOn.Click += new System.EventHandler(this.btnLaserOn_Click);
            // 
            // btnLaserOff
            // 
            this.btnLaserOff.Location = new System.Drawing.Point(124, 83);
            this.btnLaserOff.Name = "btnLaserOff";
            this.btnLaserOff.Size = new System.Drawing.Size(101, 27);
            this.btnLaserOff.TabIndex = 16;
            this.btnLaserOff.Text = "Laser OFF";
            this.btnLaserOff.UseVisualStyleBackColor = true;
            this.btnLaserOff.Click += new System.EventHandler(this.btnLaserOff_Click);
            // 
            // btnLaserMeasure
            // 
            this.btnLaserMeasure.Location = new System.Drawing.Point(124, 40);
            this.btnLaserMeasure.Name = "btnLaserMeasure";
            this.btnLaserMeasure.Size = new System.Drawing.Size(101, 27);
            this.btnLaserMeasure.TabIndex = 18;
            this.btnLaserMeasure.Text = "Measure";
            this.btnLaserMeasure.UseVisualStyleBackColor = true;
            this.btnLaserMeasure.Click += new System.EventHandler(this.btnLaserMeasure_Click);
            // 
            // btnLaserInit
            // 
            this.btnLaserInit.Location = new System.Drawing.Point(17, 40);
            this.btnLaserInit.Name = "btnLaserInit";
            this.btnLaserInit.Size = new System.Drawing.Size(101, 27);
            this.btnLaserInit.TabIndex = 17;
            this.btnLaserInit.Text = "Initialize";
            this.btnLaserInit.UseVisualStyleBackColor = true;
            this.btnLaserInit.Click += new System.EventHandler(this.btnLaserInit_Click);
            // 
            // lblLaserValue
            // 
            this.lblLaserValue.AutoSize = true;
            this.lblLaserValue.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLaserValue.Location = new System.Drawing.Point(522, 191);
            this.lblLaserValue.Name = "lblLaserValue";
            this.lblLaserValue.Size = new System.Drawing.Size(61, 21);
            this.lblLaserValue.TabIndex = 17;
            this.lblLaserValue.Text = "result";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(450, 199);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "Distance : ";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnLongRunStart);
            this.groupBox6.Controls.Add(this.btnOnceMeasure);
            this.groupBox6.Controls.Add(this.btnLongRunStop);
            this.groupBox6.Location = new System.Drawing.Point(15, 94);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(337, 58);
            this.groupBox6.TabIndex = 11;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Measure";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 473);
            this.Controls.Add(this.lblLaserValue);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblValue);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.MeasureCnt)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOnceMeasure;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnRepeat;
        private System.Windows.Forms.NumericUpDown MeasureCnt;
        private System.Windows.Forms.Button btnLongRunStart;
        private System.Windows.Forms.Button btnLongRunStop;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.DateTimePicker endDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker startDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.TextBox txtSampleName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtTilting;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnLaserOn;
        private System.Windows.Forms.Button btnLaserOff;
        private System.Windows.Forms.Button btnLaserInit;
        private System.Windows.Forms.Button btnLaserMeasure;
        private System.Windows.Forms.Label lblLaserValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox6;
    }
}

