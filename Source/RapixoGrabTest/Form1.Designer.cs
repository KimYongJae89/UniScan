namespace RapixoGrabTest
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
            this.buttonInitMaster = new System.Windows.Forms.Button();
            this.buttonGrabSingle = new System.Windows.Forms.Button();
            this.buttonGrabMulti = new System.Windows.Forms.Button();
            this.buttonInitSlave = new System.Windows.Forms.Button();
            this.buttonRelease = new System.Windows.Forms.Button();
            this.buttonGrabStop = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // buttonInitMaster
            // 
            this.buttonInitMaster.Location = new System.Drawing.Point(12, 12);
            this.buttonInitMaster.Name = "buttonInitMaster";
            this.buttonInitMaster.Size = new System.Drawing.Size(83, 65);
            this.buttonInitMaster.TabIndex = 0;
            this.buttonInitMaster.Text = "Init Master";
            this.buttonInitMaster.UseVisualStyleBackColor = true;
            this.buttonInitMaster.Click += new System.EventHandler(this.buttonInitMaster_Click);
            // 
            // buttonGrabSingle
            // 
            this.buttonGrabSingle.Location = new System.Drawing.Point(154, 12);
            this.buttonGrabSingle.Name = "buttonGrabSingle";
            this.buttonGrabSingle.Size = new System.Drawing.Size(83, 65);
            this.buttonGrabSingle.TabIndex = 0;
            this.buttonGrabSingle.Text = "Single";
            this.buttonGrabSingle.UseVisualStyleBackColor = true;
            this.buttonGrabSingle.Click += new System.EventHandler(this.buttonGrabSingle_Click);
            // 
            // buttonGrabMulti
            // 
            this.buttonGrabMulti.Location = new System.Drawing.Point(243, 12);
            this.buttonGrabMulti.Name = "buttonGrabMulti";
            this.buttonGrabMulti.Size = new System.Drawing.Size(83, 65);
            this.buttonGrabMulti.TabIndex = 0;
            this.buttonGrabMulti.Text = "Multi";
            this.buttonGrabMulti.UseVisualStyleBackColor = true;
            this.buttonGrabMulti.Click += new System.EventHandler(this.buttonGrabMulti_Click);
            // 
            // buttonInitSlave
            // 
            this.buttonInitSlave.Location = new System.Drawing.Point(12, 83);
            this.buttonInitSlave.Name = "buttonInitSlave";
            this.buttonInitSlave.Size = new System.Drawing.Size(83, 65);
            this.buttonInitSlave.TabIndex = 0;
            this.buttonInitSlave.Text = "Init Slave";
            this.buttonInitSlave.UseVisualStyleBackColor = true;
            this.buttonInitSlave.Click += new System.EventHandler(this.buttonInitSlave_Click);
            // 
            // buttonRelease
            // 
            this.buttonRelease.Location = new System.Drawing.Point(12, 154);
            this.buttonRelease.Name = "buttonRelease";
            this.buttonRelease.Size = new System.Drawing.Size(83, 65);
            this.buttonRelease.TabIndex = 0;
            this.buttonRelease.Text = "Release";
            this.buttonRelease.UseVisualStyleBackColor = true;
            this.buttonRelease.Click += new System.EventHandler(this.buttonRelease_Click);
            // 
            // buttonGrabStop
            // 
            this.buttonGrabStop.Location = new System.Drawing.Point(332, 12);
            this.buttonGrabStop.Name = "buttonGrabStop";
            this.buttonGrabStop.Size = new System.Drawing.Size(83, 65);
            this.buttonGrabStop.TabIndex = 0;
            this.buttonGrabStop.Text = "Stop";
            this.buttonGrabStop.UseVisualStyleBackColor = true;
            this.buttonGrabStop.Click += new System.EventHandler(this.buttonGrabStop_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(482, 12);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(306, 276);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Items.AddRange(new object[] {
            "111",
            "222",
            "3",
            "44",
            "5",
            "6",
            "7",
            "8",
            "9",
            "0"});
            this.listBox1.Location = new System.Drawing.Point(12, 307);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(776, 136);
            this.listBox1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.buttonGrabStop);
            this.Controls.Add(this.buttonGrabMulti);
            this.Controls.Add(this.buttonGrabSingle);
            this.Controls.Add(this.buttonRelease);
            this.Controls.Add(this.buttonInitSlave);
            this.Controls.Add(this.buttonInitMaster);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonInitMaster;
        private System.Windows.Forms.Button buttonGrabSingle;
        private System.Windows.Forms.Button buttonGrabMulti;
        private System.Windows.Forms.Button buttonInitSlave;
        private System.Windows.Forms.Button buttonRelease;
        private System.Windows.Forms.Button buttonGrabStop;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ListBox listBox1;
    }
}

