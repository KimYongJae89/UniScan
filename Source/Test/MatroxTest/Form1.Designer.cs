namespace MatroxTest
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_Grab = new System.Windows.Forms.Button();
            this.button_Stop = new System.Windows.Forms.Button();
            this.button_Init = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.buttonOneShot = new System.Windows.Forms.Button();
            this.buttonSeQuenceGrab = new System.Windows.Forms.Button();
            this.nudSeQcount = new System.Windows.Forms.NumericUpDown();
            this.buttonClearListBox = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonSaveMerge = new System.Windows.Forms.Button();
            this.buttonSaveImage = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonDeleteSelectedList = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeQcount)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(651, 450);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button_Grab
            // 
            this.button_Grab.Location = new System.Drawing.Point(274, 15);
            this.button_Grab.Name = "button_Grab";
            this.button_Grab.Size = new System.Drawing.Size(111, 28);
            this.button_Grab.TabIndex = 1;
            this.button_Grab.Text = "MultiShot";
            this.button_Grab.UseVisualStyleBackColor = true;
            this.button_Grab.Click += new System.EventHandler(this.button_Grab_Click);
            // 
            // button_Stop
            // 
            this.button_Stop.Location = new System.Drawing.Point(141, 49);
            this.button_Stop.Name = "button_Stop";
            this.button_Stop.Size = new System.Drawing.Size(111, 28);
            this.button_Stop.TabIndex = 1;
            this.button_Stop.Text = "Stop";
            this.button_Stop.UseVisualStyleBackColor = true;
            this.button_Stop.Click += new System.EventHandler(this.button_Stop_Click);
            // 
            // button_Init
            // 
            this.button_Init.Enabled = false;
            this.button_Init.Location = new System.Drawing.Point(15, 15);
            this.button_Init.Name = "button_Init";
            this.button_Init.Size = new System.Drawing.Size(111, 28);
            this.button_Init.TabIndex = 1;
            this.button_Init.Text = "Init";
            this.button_Init.UseVisualStyleBackColor = true;
            this.button_Init.Click += new System.EventHandler(this.button_Init_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(283, 101);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(102, 16);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Update Image";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // buttonOneShot
            // 
            this.buttonOneShot.Location = new System.Drawing.Point(141, 15);
            this.buttonOneShot.Name = "buttonOneShot";
            this.buttonOneShot.Size = new System.Drawing.Size(111, 28);
            this.buttonOneShot.TabIndex = 1;
            this.buttonOneShot.Text = "OneShot";
            this.buttonOneShot.UseVisualStyleBackColor = true;
            this.buttonOneShot.Click += new System.EventHandler(this.buttonOneShot_Click);
            // 
            // buttonSeQuenceGrab
            // 
            this.buttonSeQuenceGrab.Location = new System.Drawing.Point(274, 49);
            this.buttonSeQuenceGrab.Name = "buttonSeQuenceGrab";
            this.buttonSeQuenceGrab.Size = new System.Drawing.Size(111, 28);
            this.buttonSeQuenceGrab.TabIndex = 1;
            this.buttonSeQuenceGrab.Text = "SeQ Grab";
            this.buttonSeQuenceGrab.UseVisualStyleBackColor = true;
            this.buttonSeQuenceGrab.Click += new System.EventHandler(this.buttonSeQuenceGrab_Click);
            // 
            // nudSeQcount
            // 
            this.nudSeQcount.Location = new System.Drawing.Point(274, 123);
            this.nudSeQcount.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudSeQcount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSeQcount.Name = "nudSeQcount";
            this.nudSeQcount.Size = new System.Drawing.Size(102, 21);
            this.nudSeQcount.TabIndex = 5;
            this.nudSeQcount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // buttonClearListBox
            // 
            this.buttonClearListBox.Location = new System.Drawing.Point(310, 172);
            this.buttonClearListBox.Name = "buttonClearListBox";
            this.buttonClearListBox.Size = new System.Drawing.Size(111, 28);
            this.buttonClearListBox.TabIndex = 1;
            this.buttonClearListBox.Text = "Clear List";
            this.buttonClearListBox.UseVisualStyleBackColor = true;
            this.buttonClearListBox.Click += new System.EventHandler(this.buttonClearListBox_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonSaveMerge);
            this.panel1.Controls.Add(this.buttonSaveImage);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.nudSeQcount);
            this.panel1.Controls.Add(this.button_Grab);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.buttonSeQuenceGrab);
            this.panel1.Controls.Add(this.buttonDeleteSelectedList);
            this.panel1.Controls.Add(this.buttonClearListBox);
            this.panel1.Controls.Add(this.buttonOneShot);
            this.panel1.Controls.Add(this.button_Init);
            this.panel1.Controls.Add(this.button_Stop);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(427, 450);
            this.panel1.TabIndex = 7;
            // 
            // buttonSaveMerge
            // 
            this.buttonSaveMerge.Location = new System.Drawing.Point(200, 171);
            this.buttonSaveMerge.Name = "buttonSaveMerge";
            this.buttonSaveMerge.Size = new System.Drawing.Size(89, 29);
            this.buttonSaveMerge.TabIndex = 7;
            this.buttonSaveMerge.Text = "SaveMerge";
            this.buttonSaveMerge.UseVisualStyleBackColor = true;
            this.buttonSaveMerge.Click += new System.EventHandler(this.buttonSaveMerge_Click);
            // 
            // buttonSaveImage
            // 
            this.buttonSaveImage.Location = new System.Drawing.Point(123, 171);
            this.buttonSaveImage.Name = "buttonSaveImage";
            this.buttonSaveImage.Size = new System.Drawing.Size(71, 29);
            this.buttonSaveImage.TabIndex = 7;
            this.buttonSaveImage.Text = "SaveAll";
            this.buttonSaveImage.UseVisualStyleBackColor = true;
            this.buttonSaveImage.Click += new System.EventHandler(this.buttonSaveImage_Click);
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(0, 206);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(427, 244);
            this.listBox1.TabIndex = 6;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // buttonDeleteSelectedList
            // 
            this.buttonDeleteSelectedList.Location = new System.Drawing.Point(3, 172);
            this.buttonDeleteSelectedList.Name = "buttonDeleteSelectedList";
            this.buttonDeleteSelectedList.Size = new System.Drawing.Size(111, 28);
            this.buttonDeleteSelectedList.TabIndex = 1;
            this.buttonDeleteSelectedList.Text = "Delete Selected";
            this.buttonDeleteSelectedList.UseVisualStyleBackColor = true;
            this.buttonDeleteSelectedList.Click += new System.EventHandler(this.buttonDeleteSelectedList_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(427, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(651, 450);
            this.panel2.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1078, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeQcount)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_Grab;
        private System.Windows.Forms.Button button_Stop;
        private System.Windows.Forms.Button button_Init;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button buttonOneShot;
        private System.Windows.Forms.Button buttonSeQuenceGrab;
        private System.Windows.Forms.NumericUpDown nudSeQcount;
        private System.Windows.Forms.Button buttonClearListBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonDeleteSelectedList;
        private System.Windows.Forms.Button buttonSaveMerge;
        private System.Windows.Forms.Button buttonSaveImage;
    }
}

