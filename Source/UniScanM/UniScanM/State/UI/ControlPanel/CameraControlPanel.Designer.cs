namespace UniScanM.UI.ControlPanel
{
    partial class CameraControlPanel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CameraControlPanel));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonCameraCalibration = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCameraFreq = new System.Windows.Forms.Label();
            this.buttonCameraGrabStop = new System.Windows.Forms.Button();
            this.comboBoxCameraDevice = new System.Windows.Forms.ComboBox();
            this.buttonCameraGrabExposure = new System.Windows.Forms.NumericUpDown();
            this.buttonCameraGrabMulti = new System.Windows.Forms.Button();
            this.buttonCameraGrabOnce = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonLightAllOff = new System.Windows.Forms.Button();
            this.buttonLightAllOn = new System.Windows.Forms.Button();
            this.dataGridViewLight = new System.Windows.Forms.DataGridView();
            this.columnLightCtrlNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLightChannelNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnLightValue = new Infragistics.Win.UltraDataGridView.UltraNumericEditorColumn(this.components);
            this.labelGrabDoneState = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buttonCameraGrabExposure)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLight)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(494, 674);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelGrabDoneState);
            this.groupBox2.Controls.Add(this.buttonCameraCalibration);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.labelCameraFreq);
            this.groupBox2.Controls.Add(this.buttonCameraGrabStop);
            this.groupBox2.Controls.Add(this.comboBoxCameraDevice);
            this.groupBox2.Controls.Add(this.buttonCameraGrabExposure);
            this.groupBox2.Controls.Add(this.buttonCameraGrabMulti);
            this.groupBox2.Controls.Add(this.buttonCameraGrabOnce);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(494, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(353, 273);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Camera Control";
            // 
            // buttonCameraCalibration
            // 
            this.buttonCameraCalibration.Location = new System.Drawing.Point(200, 207);
            this.buttonCameraCalibration.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonCameraCalibration.Name = "buttonCameraCalibration";
            this.buttonCameraCalibration.Size = new System.Drawing.Size(132, 48);
            this.buttonCameraCalibration.TabIndex = 8;
            this.buttonCameraCalibration.Text = "Calibration";
            this.buttonCameraCalibration.UseVisualStyleBackColor = true;
            this.buttonCameraCalibration.Click += new System.EventHandler(this.buttonCameraCalibration_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(241, 67);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 21);
            this.label2.TabIndex = 7;
            this.label2.Text = "kHz";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(241, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 21);
            this.label1.TabIndex = 6;
            this.label1.Text = "Frames/s";
            // 
            // labelCameraFreq
            // 
            this.labelCameraFreq.Location = new System.Drawing.Point(166, 67);
            this.labelCameraFreq.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCameraFreq.Name = "labelCameraFreq";
            this.labelCameraFreq.Size = new System.Drawing.Size(76, 21);
            this.labelCameraFreq.TabIndex = 5;
            this.labelCameraFreq.Text = "000.000";
            this.labelCameraFreq.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonCameraGrabStop
            // 
            this.buttonCameraGrabStop.Location = new System.Drawing.Point(24, 207);
            this.buttonCameraGrabStop.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonCameraGrabStop.Name = "buttonCameraGrabStop";
            this.buttonCameraGrabStop.Size = new System.Drawing.Size(132, 48);
            this.buttonCameraGrabStop.TabIndex = 4;
            this.buttonCameraGrabStop.Text = "Grab Stop";
            this.buttonCameraGrabStop.UseVisualStyleBackColor = true;
            this.buttonCameraGrabStop.Click += new System.EventHandler(this.buttonCameraGrabStop_Click);
            // 
            // comboBoxCameraDevice
            // 
            this.comboBoxCameraDevice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCameraDevice.FormattingEnabled = true;
            this.comboBoxCameraDevice.Location = new System.Drawing.Point(24, 31);
            this.comboBoxCameraDevice.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBoxCameraDevice.Name = "comboBoxCameraDevice";
            this.comboBoxCameraDevice.Size = new System.Drawing.Size(132, 29);
            this.comboBoxCameraDevice.TabIndex = 3;
            this.comboBoxCameraDevice.SelectedIndexChanged += new System.EventHandler(this.comboBoxCameraDevice_SelectedIndexChanged);
            // 
            // buttonCameraGrabExposure
            // 
            this.buttonCameraGrabExposure.DecimalPlaces = 3;
            this.buttonCameraGrabExposure.Increment = new decimal(new int[] {
            1,
            0,
            0,
            262144});
            this.buttonCameraGrabExposure.Location = new System.Drawing.Point(166, 31);
            this.buttonCameraGrabExposure.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonCameraGrabExposure.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.buttonCameraGrabExposure.Name = "buttonCameraGrabExposure";
            this.buttonCameraGrabExposure.Size = new System.Drawing.Size(76, 29);
            this.buttonCameraGrabExposure.TabIndex = 2;
            this.buttonCameraGrabExposure.ValueChanged += new System.EventHandler(this.buttonCameraGrabExposure_ValueChanged);
            // 
            // buttonCameraGrabMulti
            // 
            this.buttonCameraGrabMulti.Location = new System.Drawing.Point(24, 151);
            this.buttonCameraGrabMulti.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonCameraGrabMulti.Name = "buttonCameraGrabMulti";
            this.buttonCameraGrabMulti.Size = new System.Drawing.Size(132, 48);
            this.buttonCameraGrabMulti.TabIndex = 1;
            this.buttonCameraGrabMulti.Text = "Grab Multi";
            this.buttonCameraGrabMulti.UseVisualStyleBackColor = true;
            this.buttonCameraGrabMulti.Click += new System.EventHandler(this.buttonCameraGrabMulti_Click);
            // 
            // buttonCameraGrabOnce
            // 
            this.buttonCameraGrabOnce.Location = new System.Drawing.Point(24, 97);
            this.buttonCameraGrabOnce.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonCameraGrabOnce.Name = "buttonCameraGrabOnce";
            this.buttonCameraGrabOnce.Size = new System.Drawing.Size(132, 48);
            this.buttonCameraGrabOnce.TabIndex = 0;
            this.buttonCameraGrabOnce.Text = "Grab Once";
            this.buttonCameraGrabOnce.UseVisualStyleBackColor = true;
            this.buttonCameraGrabOnce.Click += new System.EventHandler(this.buttonCameraGrabOnce_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonLightAllOff);
            this.groupBox3.Controls.Add(this.buttonLightAllOn);
            this.groupBox3.Controls.Add(this.dataGridViewLight);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(494, 273);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Size = new System.Drawing.Size(353, 401);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Light Control";
            // 
            // buttonLightAllOff
            // 
            this.buttonLightAllOff.Location = new System.Drawing.Point(200, 28);
            this.buttonLightAllOff.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonLightAllOff.Name = "buttonLightAllOff";
            this.buttonLightAllOff.Size = new System.Drawing.Size(132, 48);
            this.buttonLightAllOff.TabIndex = 5;
            this.buttonLightAllOff.Text = "ALL OFF";
            this.buttonLightAllOff.UseVisualStyleBackColor = true;
            this.buttonLightAllOff.Click += new System.EventHandler(this.buttonLightAllOff_Click);
            // 
            // buttonLightAllOn
            // 
            this.buttonLightAllOn.Location = new System.Drawing.Point(24, 28);
            this.buttonLightAllOn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonLightAllOn.Name = "buttonLightAllOn";
            this.buttonLightAllOn.Size = new System.Drawing.Size(132, 48);
            this.buttonLightAllOn.TabIndex = 5;
            this.buttonLightAllOn.Text = "ALL ON";
            this.buttonLightAllOn.UseVisualStyleBackColor = true;
            this.buttonLightAllOn.Click += new System.EventHandler(this.buttonLightAllOn_Click);
            // 
            // dataGridViewLight
            // 
            this.dataGridViewLight.AllowUserToAddRows = false;
            this.dataGridViewLight.AllowUserToDeleteRows = false;
            this.dataGridViewLight.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridViewLight.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLight.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnLightCtrlNo,
            this.columnLightChannelNo,
            this.columnLightValue});
            this.dataGridViewLight.Location = new System.Drawing.Point(24, 82);
            this.dataGridViewLight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.dataGridViewLight.Name = "dataGridViewLight";
            this.dataGridViewLight.RowHeadersVisible = false;
            this.dataGridViewLight.RowTemplate.Height = 27;
            this.dataGridViewLight.Size = new System.Drawing.Size(308, 313);
            this.dataGridViewLight.TabIndex = 0;
            this.dataGridViewLight.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLight_CellValueChanged);
            // 
            // columnLightCtrlNo
            // 
            this.columnLightCtrlNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnLightCtrlNo.HeaderText = "Controller";
            this.columnLightCtrlNo.Name = "columnLightCtrlNo";
            this.columnLightCtrlNo.ReadOnly = true;
            this.columnLightCtrlNo.Width = 110;
            // 
            // columnLightChannelNo
            // 
            this.columnLightChannelNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnLightChannelNo.HeaderText = "Channel";
            this.columnLightChannelNo.Name = "columnLightChannelNo";
            this.columnLightChannelNo.ReadOnly = true;
            this.columnLightChannelNo.Width = 97;
            // 
            // columnLightValue
            // 
            this.columnLightValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnLightValue.DefaultNewRowValue = ((object)(resources.GetObject("columnLightValue.DefaultNewRowValue")));
            this.columnLightValue.DisplayStyle = Infragistics.Win.EmbeddableElementDisplayStyle.Default;
            this.columnLightValue.HeaderText = "Value";
            this.columnLightValue.MaskInput = null;
            this.columnLightValue.MinimumWidth = 3;
            this.columnLightValue.Name = "columnLightValue";
            this.columnLightValue.PadChar = '\0';
            this.columnLightValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.columnLightValue.SpinButtonAlignment = Infragistics.Win.SpinButtonDisplayStyle.None;
            // 
            // labelGrabDoneState
            // 
            this.labelGrabDoneState.Location = new System.Drawing.Point(214, 151);
            this.labelGrabDoneState.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelGrabDoneState.Name = "labelGrabDoneState";
            this.labelGrabDoneState.Size = new System.Drawing.Size(118, 48);
            this.labelGrabDoneState.TabIndex = 9;
            this.labelGrabDoneState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CameraControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Malgun Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CameraControlPanel";
            this.Size = new System.Drawing.Size(847, 674);
            this.Load += new System.EventHandler(this.CameraControlPanel_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.buttonCameraGrabExposure)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown buttonCameraGrabExposure;
        private System.Windows.Forms.Button buttonCameraGrabMulti;
        private System.Windows.Forms.Button buttonCameraGrabOnce;
        private System.Windows.Forms.DataGridView dataGridViewLight;
        private System.Windows.Forms.ComboBox comboBoxCameraDevice;
        private System.Windows.Forms.Button buttonCameraGrabStop;
        private System.Windows.Forms.Button buttonLightAllOff;
        private System.Windows.Forms.Button buttonLightAllOn;
        private System.Windows.Forms.Label labelCameraFreq;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCameraCalibration;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLightCtrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLightChannel;
        private System.Windows.Forms.Label labelGrabDoneState;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLightCtrlNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnLightChannelNo;
        private Infragistics.Win.UltraDataGridView.UltraNumericEditorColumn columnLightValue;
    }
}
