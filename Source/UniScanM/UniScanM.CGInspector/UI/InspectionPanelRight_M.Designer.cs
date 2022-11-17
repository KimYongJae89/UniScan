namespace UniScanM.CGInspector.UI
{
    partial class InspectionPanelRight_M
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.checkOnTune = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cycleOnce = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDownLight = new System.Windows.Forms.NumericUpDown();
            this.Motion_Parameter = new System.Windows.Forms.GroupBox();
            this.endPosValue = new System.Windows.Forms.NumericUpDown();
            this.startPosValue = new System.Windows.Forms.NumericUpDown();
            this.cycleMulti = new System.Windows.Forms.Button();
            this.cycleStop = new System.Windows.Forms.Button();
            this.LightParameter = new System.Windows.Forms.GroupBox();
            this.lightSaveButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLight)).BeginInit();
            this.Motion_Parameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endPosValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startPosValue)).BeginInit();
            this.LightParameter.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkOnTune
            // 
            this.checkOnTune.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkOnTune.BackColor = System.Drawing.Color.Gray;
            this.checkOnTune.Checked = true;
            this.checkOnTune.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkOnTune.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.checkOnTune.FlatAppearance.CheckedBackColor = System.Drawing.Color.SeaGreen;
            this.checkOnTune.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkOnTune.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.checkOnTune.Location = new System.Drawing.Point(0, 507);
            this.checkOnTune.Name = "checkOnTune";
            this.checkOnTune.Size = new System.Drawing.Size(408, 50);
            this.checkOnTune.TabIndex = 10;
            this.checkOnTune.Text = "Comm Open/Close";
            this.checkOnTune.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkOnTune.UseVisualStyleBackColor = false;
            this.checkOnTune.CheckedChanged += new System.EventHandler(this.checkOnTune_CheckedChanged);
            // 
            // cycleOnce
            // 
            this.cycleOnce.Location = new System.Drawing.Point(14, 66);
            this.cycleOnce.Name = "cycleOnce";
            this.cycleOnce.Size = new System.Drawing.Size(167, 35);
            this.cycleOnce.TabIndex = 11;
            this.cycleOnce.Text = "Cycle-1";
            this.cycleOnce.UseVisualStyleBackColor = true;
            this.cycleOnce.Click += new System.EventHandler(this.cycleOnce_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 10F);
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "StartPos";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 10F);
            this.label2.Location = new System.Drawing.Point(198, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "EndPos";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 10F);
            this.label5.Location = new System.Drawing.Point(12, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 16);
            this.label5.TabIndex = 12;
            this.label5.Text = "Light";
            // 
            // numericUpDownLight
            // 
            this.numericUpDownLight.Location = new System.Drawing.Point(82, 28);
            this.numericUpDownLight.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownLight.Name = "numericUpDownLight";
            this.numericUpDownLight.Size = new System.Drawing.Size(100, 25);
            this.numericUpDownLight.TabIndex = 14;
            this.numericUpDownLight.Value = new decimal(new int[] {
            35,
            0,
            0,
            0});
            this.numericUpDownLight.ValueChanged += new System.EventHandler(this.numericUpDownLight_ValueChanged);
            // 
            // Motion_Parameter
            // 
            this.Motion_Parameter.Controls.Add(this.endPosValue);
            this.Motion_Parameter.Controls.Add(this.startPosValue);
            this.Motion_Parameter.Controls.Add(this.label1);
            this.Motion_Parameter.Controls.Add(this.label2);
            this.Motion_Parameter.Controls.Add(this.cycleMulti);
            this.Motion_Parameter.Controls.Add(this.cycleStop);
            this.Motion_Parameter.Controls.Add(this.cycleOnce);
            this.Motion_Parameter.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.Motion_Parameter.Location = new System.Drawing.Point(13, 33);
            this.Motion_Parameter.Name = "Motion_Parameter";
            this.Motion_Parameter.Size = new System.Drawing.Size(374, 150);
            this.Motion_Parameter.TabIndex = 15;
            this.Motion_Parameter.TabStop = false;
            this.Motion_Parameter.Text = "Motion Parameter";
            // 
            // endPosValue
            // 
            this.endPosValue.Location = new System.Drawing.Point(269, 31);
            this.endPosValue.Name = "endPosValue";
            this.endPosValue.Size = new System.Drawing.Size(99, 24);
            this.endPosValue.TabIndex = 14;
            // 
            // startPosValue
            // 
            this.startPosValue.Location = new System.Drawing.Point(82, 31);
            this.startPosValue.Name = "startPosValue";
            this.startPosValue.Size = new System.Drawing.Size(99, 24);
            this.startPosValue.TabIndex = 14;
            // 
            // cycleMulti
            // 
            this.cycleMulti.Location = new System.Drawing.Point(201, 66);
            this.cycleMulti.Name = "cycleMulti";
            this.cycleMulti.Size = new System.Drawing.Size(167, 35);
            this.cycleMulti.TabIndex = 11;
            this.cycleMulti.Text = "Cycle-N";
            this.cycleMulti.UseVisualStyleBackColor = true;
            this.cycleMulti.Click += new System.EventHandler(this.cycleMulti_Click);
            // 
            // cycleStop
            // 
            this.cycleStop.Location = new System.Drawing.Point(14, 107);
            this.cycleStop.Name = "cycleStop";
            this.cycleStop.Size = new System.Drawing.Size(354, 35);
            this.cycleStop.TabIndex = 11;
            this.cycleStop.Text = "Stop";
            this.cycleStop.UseVisualStyleBackColor = true;
            this.cycleStop.Click += new System.EventHandler(this.cycleStop_Click);
            // 
            // LightParameter
            // 
            this.LightParameter.Controls.Add(this.lightSaveButton);
            this.LightParameter.Controls.Add(this.label5);
            this.LightParameter.Controls.Add(this.numericUpDownLight);
            this.LightParameter.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LightParameter.Location = new System.Drawing.Point(13, 203);
            this.LightParameter.Name = "LightParameter";
            this.LightParameter.Size = new System.Drawing.Size(368, 72);
            this.LightParameter.TabIndex = 15;
            this.LightParameter.TabStop = false;
            this.LightParameter.Text = "Light Parameter";
            // 
            // lightSaveButton
            // 
            this.lightSaveButton.Location = new System.Drawing.Point(201, 28);
            this.lightSaveButton.Name = "lightSaveButton";
            this.lightSaveButton.Size = new System.Drawing.Size(161, 25);
            this.lightSaveButton.TabIndex = 16;
            this.lightSaveButton.Text = "Save";
            this.lightSaveButton.UseVisualStyleBackColor = true;
            this.lightSaveButton.Click += new System.EventHandler(this.lightSaveButton_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Cyan;
            this.button1.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.button1.ForeColor = System.Drawing.Color.HotPink;
            this.button1.Location = new System.Drawing.Point(118, 293);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(173, 40);
            this.button1.TabIndex = 16;
            this.button1.Text = "LightSetting";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // InspectionPanelRight_M
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LightParameter);
            this.Controls.Add(this.Motion_Parameter);
            this.Controls.Add(this.checkOnTune);
            this.Name = "InspectionPanelRight_M";
            this.Size = new System.Drawing.Size(408, 557);
            this.Load += new System.EventHandler(this.InspectionPanelRight_M_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLight)).EndInit();
            this.Motion_Parameter.ResumeLayout(false);
            this.Motion_Parameter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.endPosValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startPosValue)).EndInit();
            this.LightParameter.ResumeLayout(false);
            this.LightParameter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkOnTune;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button cycleOnce;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox Motion_Parameter;
        private System.Windows.Forms.GroupBox LightParameter;
        public System.Windows.Forms.Button lightSaveButton;
        public System.Windows.Forms.NumericUpDown numericUpDownLight;
        private System.Windows.Forms.Button cycleMulti;
        private System.Windows.Forms.NumericUpDown endPosValue;
        private System.Windows.Forms.NumericUpDown startPosValue;
        private System.Windows.Forms.Button cycleStop;
        private System.Windows.Forms.Button button1;
    }
}
