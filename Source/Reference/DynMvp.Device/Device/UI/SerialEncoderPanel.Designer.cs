﻿namespace DynMvp.Devices.UI
{ 
    partial class SerialEncoderPanel
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonEn0 = new System.Windows.Forms.Button();
            this.buttonEn1 = new System.Windows.Forms.Button();
            this.buttonIn1 = new System.Windows.Forms.Button();
            this.buttonIn0 = new System.Windows.Forms.Button();
            this.buttonCp = new System.Windows.Forms.Button();
            this.buttonCc = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelUmPerPulse = new System.Windows.Forms.Label();
            this.umPerPulse = new System.Windows.Forms.NumericUpDown();
            this.checkBoxAutoUpdate = new System.Windows.Forms.CheckBox();
            this.buttonGr = new System.Windows.Forms.Button();
            this.buttonAp = new System.Windows.Forms.Button();
            this.buttonPc = new System.Windows.Forms.Button();
            this.manualCommand = new System.Windows.Forms.TextBox();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.umPerPulse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(501, 288);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.VirtualMode = true;
            this.dataGridView1.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dataGridView1_CellValueNeeded);
            this.dataGridView1.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dataGridView1_CellValuePushed);
            // 
            // buttonEn0
            // 
            this.buttonEn0.Location = new System.Drawing.Point(7, 14);
            this.buttonEn0.Name = "buttonEn0";
            this.buttonEn0.Size = new System.Drawing.Size(73, 31);
            this.buttonEn0.TabIndex = 1;
            this.buttonEn0.Text = "EN = 0";
            this.buttonEn0.UseVisualStyleBackColor = true;
            this.buttonEn0.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonEn1
            // 
            this.buttonEn1.Location = new System.Drawing.Point(85, 14);
            this.buttonEn1.Name = "buttonEn1";
            this.buttonEn1.Size = new System.Drawing.Size(73, 31);
            this.buttonEn1.TabIndex = 2;
            this.buttonEn1.Text = "EN = 1";
            this.buttonEn1.UseVisualStyleBackColor = true;
            this.buttonEn1.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonIn1
            // 
            this.buttonIn1.Location = new System.Drawing.Point(85, 51);
            this.buttonIn1.Name = "buttonIn1";
            this.buttonIn1.Size = new System.Drawing.Size(73, 31);
            this.buttonIn1.TabIndex = 4;
            this.buttonIn1.Text = "IN = 1";
            this.buttonIn1.UseVisualStyleBackColor = true;
            this.buttonIn1.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonIn0
            // 
            this.buttonIn0.Location = new System.Drawing.Point(7, 51);
            this.buttonIn0.Name = "buttonIn0";
            this.buttonIn0.Size = new System.Drawing.Size(73, 31);
            this.buttonIn0.TabIndex = 3;
            this.buttonIn0.Text = "IN = 0";
            this.buttonIn0.UseVisualStyleBackColor = true;
            this.buttonIn0.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonCp
            // 
            this.buttonCp.Location = new System.Drawing.Point(85, 125);
            this.buttonCp.Name = "buttonCp";
            this.buttonCp.Size = new System.Drawing.Size(73, 31);
            this.buttonCp.TabIndex = 5;
            this.buttonCp.Text = "CP";
            this.buttonCp.UseVisualStyleBackColor = true;
            this.buttonCp.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonCc
            // 
            this.buttonCc.Location = new System.Drawing.Point(85, 162);
            this.buttonCc.Name = "buttonCc";
            this.buttonCc.Size = new System.Drawing.Size(73, 31);
            this.buttonCc.TabIndex = 6;
            this.buttonCc.Text = "CC";
            this.buttonCc.UseVisualStyleBackColor = true;
            this.buttonCc.Click += new System.EventHandler(this.button_Click);
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(668, 145);
            this.textBox1.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelUmPerPulse);
            this.panel1.Controls.Add(this.umPerPulse);
            this.panel1.Controls.Add(this.checkBoxAutoUpdate);
            this.panel1.Controls.Add(this.buttonGr);
            this.panel1.Controls.Add(this.buttonEn0);
            this.panel1.Controls.Add(this.buttonCc);
            this.panel1.Controls.Add(this.buttonEn1);
            this.panel1.Controls.Add(this.buttonAp);
            this.panel1.Controls.Add(this.buttonPc);
            this.panel1.Controls.Add(this.buttonCp);
            this.panel1.Controls.Add(this.buttonIn0);
            this.panel1.Controls.Add(this.buttonIn1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(501, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(167, 288);
            this.panel1.TabIndex = 8;
            // 
            // labelUmPerPulse
            // 
            this.labelUmPerPulse.AutoSize = true;
            this.labelUmPerPulse.Location = new System.Drawing.Point(6, 203);
            this.labelUmPerPulse.Name = "labelUmPerPulse";
            this.labelUmPerPulse.Size = new System.Drawing.Size(61, 12);
            this.labelUmPerPulse.TabIndex = 10;
            this.labelUmPerPulse.Text = "um/Pulse";
            // 
            // umPerPulse
            // 
            this.umPerPulse.DecimalPlaces = 2;
            this.umPerPulse.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.umPerPulse.Location = new System.Drawing.Point(66, 218);
            this.umPerPulse.Name = "umPerPulse";
            this.umPerPulse.Size = new System.Drawing.Size(104, 21);
            this.umPerPulse.TabIndex = 9;
            // 
            // checkBoxAutoUpdate
            // 
            this.checkBoxAutoUpdate.AutoSize = true;
            this.checkBoxAutoUpdate.Location = new System.Drawing.Point(66, 245);
            this.checkBoxAutoUpdate.Name = "checkBoxAutoUpdate";
            this.checkBoxAutoUpdate.Size = new System.Drawing.Size(92, 16);
            this.checkBoxAutoUpdate.TabIndex = 8;
            this.checkBoxAutoUpdate.Text = "Auto Update";
            this.checkBoxAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // buttonGr
            // 
            this.buttonGr.Location = new System.Drawing.Point(7, 88);
            this.buttonGr.Name = "buttonGr";
            this.buttonGr.Size = new System.Drawing.Size(151, 31);
            this.buttonGr.TabIndex = 7;
            this.buttonGr.Text = "GR";
            this.buttonGr.UseVisualStyleBackColor = true;
            this.buttonGr.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonAp
            // 
            this.buttonAp.Location = new System.Drawing.Point(8, 125);
            this.buttonAp.Name = "buttonAp";
            this.buttonAp.Size = new System.Drawing.Size(73, 31);
            this.buttonAp.TabIndex = 5;
            this.buttonAp.Text = "AP";
            this.buttonAp.UseVisualStyleBackColor = true;
            this.buttonAp.Click += new System.EventHandler(this.button_Click);
            // 
            // buttonPc
            // 
            this.buttonPc.Location = new System.Drawing.Point(8, 162);
            this.buttonPc.Name = "buttonPc";
            this.buttonPc.Size = new System.Drawing.Size(73, 31);
            this.buttonPc.TabIndex = 5;
            this.buttonPc.Text = "PC";
            this.buttonPc.UseVisualStyleBackColor = true;
            this.buttonPc.Click += new System.EventHandler(this.button_Click);
            // 
            // manualCommand
            // 
            this.manualCommand.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.manualCommand.Location = new System.Drawing.Point(0, 145);
            this.manualCommand.Name = "manualCommand";
            this.manualCommand.Size = new System.Drawing.Size(668, 21);
            this.manualCommand.TabIndex = 10;
            this.manualCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ManualCommand_KeyDown);
            // 
            // splitContainer
            // 
            this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dataGridView1);
            this.splitContainer.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.textBox1);
            this.splitContainer.Panel2.Controls.Add(this.manualCommand);
            this.splitContainer.Size = new System.Drawing.Size(670, 463);
            this.splitContainer.SplitterDistance = 290;
            this.splitContainer.SplitterWidth = 5;
            this.splitContainer.TabIndex = 12;
            // 
            // SerialEncoderPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.splitContainer);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SerialEncoderPanel";
            this.Size = new System.Drawing.Size(670, 463);
            this.Load += new System.EventHandler(this.SerialEncoderPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.umPerPulse)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonEn0;
        private System.Windows.Forms.Button buttonEn1;
        private System.Windows.Forms.Button buttonIn1;
        private System.Windows.Forms.Button buttonIn0;
        private System.Windows.Forms.Button buttonCp;
        private System.Windows.Forms.Button buttonCc;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox manualCommand;
        private System.Windows.Forms.Button buttonGr;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.CheckBox checkBoxAutoUpdate;
        private System.Windows.Forms.Label labelUmPerPulse;
        private System.Windows.Forms.NumericUpDown umPerPulse;
        private System.Windows.Forms.Button buttonPc;
        private System.Windows.Forms.Button buttonAp;
    }
}
