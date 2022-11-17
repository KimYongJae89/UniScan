namespace UniSensorLed
{
    partial class LED_Driver
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LED_Driver));
            this.label_Break = new System.Windows.Forms.Label();
            this.label_DCD = new System.Windows.Forms.Label();
            this.label_RI = new System.Windows.Forms.Label();
            this.label_DSR = new System.Windows.Forms.Label();
            this.label_CTS = new System.Windows.Forms.Label();
            this.checkBox_DTR = new System.Windows.Forms.CheckBox();
            this.checkBox_RTS = new System.Windows.Forms.CheckBox();
            this.comboBox_Com = new System.Windows.Forms.ComboBox();
            this.button_Close = new System.Windows.Forms.Button();
            this.button_Open = new System.Windows.Forms.Button();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            this.trackBar4 = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).BeginInit();
            this.SuspendLayout();
            // 
            // label_Break
            // 
            this.label_Break.AutoSize = true;
            this.label_Break.BackColor = System.Drawing.Color.Gray;
            this.label_Break.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_Break.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_Break.ForeColor = System.Drawing.Color.White;
            this.label_Break.Location = new System.Drawing.Point(379, 211);
            this.label_Break.Name = "label_Break";
            this.label_Break.Size = new System.Drawing.Size(50, 15);
            this.label_Break.TabIndex = 10;
            this.label_Break.Text = "Break";
            // 
            // label_DCD
            // 
            this.label_DCD.AutoSize = true;
            this.label_DCD.BackColor = System.Drawing.Color.Gray;
            this.label_DCD.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_DCD.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_DCD.ForeColor = System.Drawing.Color.White;
            this.label_DCD.Location = new System.Drawing.Point(334, 211);
            this.label_DCD.Name = "label_DCD";
            this.label_DCD.Size = new System.Drawing.Size(39, 15);
            this.label_DCD.TabIndex = 11;
            this.label_DCD.Text = "DCD";
            // 
            // label_RI
            // 
            this.label_RI.AutoSize = true;
            this.label_RI.BackColor = System.Drawing.Color.Gray;
            this.label_RI.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_RI.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_RI.ForeColor = System.Drawing.Color.White;
            this.label_RI.Location = new System.Drawing.Point(305, 210);
            this.label_RI.Name = "label_RI";
            this.label_RI.Size = new System.Drawing.Size(23, 15);
            this.label_RI.TabIndex = 12;
            this.label_RI.Text = "RI";
            // 
            // label_DSR
            // 
            this.label_DSR.AutoSize = true;
            this.label_DSR.BackColor = System.Drawing.Color.Gray;
            this.label_DSR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_DSR.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_DSR.ForeColor = System.Drawing.Color.White;
            this.label_DSR.Location = new System.Drawing.Point(260, 212);
            this.label_DSR.Name = "label_DSR";
            this.label_DSR.Size = new System.Drawing.Size(39, 15);
            this.label_DSR.TabIndex = 13;
            this.label_DSR.Text = "DSR";
            // 
            // label_CTS
            // 
            this.label_CTS.AutoSize = true;
            this.label_CTS.BackColor = System.Drawing.Color.Gray;
            this.label_CTS.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label_CTS.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_CTS.ForeColor = System.Drawing.Color.White;
            this.label_CTS.Location = new System.Drawing.Point(216, 212);
            this.label_CTS.Name = "label_CTS";
            this.label_CTS.Size = new System.Drawing.Size(38, 15);
            this.label_CTS.TabIndex = 14;
            this.label_CTS.Text = "CTS";
            // 
            // checkBox_DTR
            // 
            this.checkBox_DTR.AutoSize = true;
            this.checkBox_DTR.BackColor = System.Drawing.Color.Gray;
            this.checkBox_DTR.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_DTR.ForeColor = System.Drawing.Color.White;
            this.checkBox_DTR.Location = new System.Drawing.Point(155, 210);
            this.checkBox_DTR.Name = "checkBox_DTR";
            this.checkBox_DTR.Size = new System.Drawing.Size(55, 17);
            this.checkBox_DTR.TabIndex = 8;
            this.checkBox_DTR.Text = "DTR";
            this.checkBox_DTR.UseVisualStyleBackColor = false;
            this.checkBox_DTR.CheckedChanged += new System.EventHandler(this.checkBox_DTR_CheckedChanged);
            // 
            // checkBox_RTS
            // 
            this.checkBox_RTS.AutoSize = true;
            this.checkBox_RTS.BackColor = System.Drawing.Color.Gray;
            this.checkBox_RTS.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkBox_RTS.ForeColor = System.Drawing.Color.White;
            this.checkBox_RTS.Location = new System.Drawing.Point(94, 210);
            this.checkBox_RTS.Name = "checkBox_RTS";
            this.checkBox_RTS.Size = new System.Drawing.Size(55, 17);
            this.checkBox_RTS.TabIndex = 9;
            this.checkBox_RTS.Text = "RTS";
            this.checkBox_RTS.UseVisualStyleBackColor = false;
            this.checkBox_RTS.CheckedChanged += new System.EventHandler(this.checkBox_RTS_CheckedChanged);
            // 
            // comboBox_Com
            // 
            this.comboBox_Com.FormattingEnabled = true;
            this.comboBox_Com.Location = new System.Drawing.Point(12, 9);
            this.comboBox_Com.Name = "comboBox_Com";
            this.comboBox_Com.Size = new System.Drawing.Size(72, 20);
            this.comboBox_Com.TabIndex = 7;
            // 
            // button_Close
            // 
            this.button_Close.Location = new System.Drawing.Point(144, 9);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(51, 22);
            this.button_Close.TabIndex = 6;
            this.button_Close.Text = "Close";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // button_Open
            // 
            this.button_Open.Location = new System.Drawing.Point(87, 7);
            this.button_Open.Name = "button_Open";
            this.button_Open.Size = new System.Drawing.Size(51, 23);
            this.button_Open.TabIndex = 5;
            this.button_Open.Text = "Open";
            this.button_Open.UseVisualStyleBackColor = true;
            this.button_Open.Click += new System.EventHandler(this.button_Open_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 49);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(47, 16);
            this.checkBox1.TabIndex = 16;
            this.checkBox1.Text = "CH0";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(12, 90);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(47, 16);
            this.checkBox2.TabIndex = 16;
            this.checkBox2.Text = "CH1";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(12, 133);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(47, 16);
            this.checkBox3.TabIndex = 16;
            this.checkBox3.Text = "CH2";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(12, 168);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(47, 16);
            this.checkBox4.TabIndex = 16;
            this.checkBox4.Text = "CH3";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(71, 49);
            this.trackBar1.Maximum = 1000;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(313, 45);
            this.trackBar1.TabIndex = 17;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(71, 90);
            this.trackBar2.Maximum = 1000;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(313, 45);
            this.trackBar2.TabIndex = 17;
            this.trackBar2.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // trackBar3
            // 
            this.trackBar3.Location = new System.Drawing.Point(71, 133);
            this.trackBar3.Maximum = 1000;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Size = new System.Drawing.Size(313, 45);
            this.trackBar3.TabIndex = 17;
            this.trackBar3.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // trackBar4
            // 
            this.trackBar4.Location = new System.Drawing.Point(71, 168);
            this.trackBar4.Maximum = 1000;
            this.trackBar4.Name = "trackBar4";
            this.trackBar4.Size = new System.Drawing.Size(313, 45);
            this.trackBar4.TabIndex = 17;
            this.trackBar4.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(390, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(390, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 12);
            this.label2.TabIndex = 18;
            this.label2.Text = "label1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(390, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 12);
            this.label3.TabIndex = 18;
            this.label3.Text = "label1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(390, 168);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 12);
            this.label4.TabIndex = 18;
            this.label4.Text = "label1";
            // 
            // LED_Driver
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 238);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBar4);
            this.Controls.Add(this.trackBar3);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label_Break);
            this.Controls.Add(this.label_DCD);
            this.Controls.Add(this.label_RI);
            this.Controls.Add(this.label_DSR);
            this.Controls.Add(this.label_CTS);
            this.Controls.Add(this.checkBox_DTR);
            this.Controls.Add(this.checkBox_RTS);
            this.Controls.Add(this.comboBox_Com);
            this.Controls.Add(this.button_Close);
            this.Controls.Add(this.button_Open);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LED_Driver";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_RS232c_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Break;
        private System.Windows.Forms.Label label_DCD;
        private System.Windows.Forms.Label label_RI;
        private System.Windows.Forms.Label label_DSR;
        private System.Windows.Forms.Label label_CTS;
        private System.Windows.Forms.CheckBox checkBox_DTR;
        private System.Windows.Forms.CheckBox checkBox_RTS;
        private System.Windows.Forms.ComboBox comboBox_Com;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.Button button_Open;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.TrackBar trackBar3;
        private System.Windows.Forms.TrackBar trackBar4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

