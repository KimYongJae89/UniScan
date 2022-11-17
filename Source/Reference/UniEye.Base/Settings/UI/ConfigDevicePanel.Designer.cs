namespace UniEye.Base.Settings.UI
{
    partial class ConfigDevicePanel
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
            this.buttonSelectGrabber = new System.Windows.Forms.Button();
            this.buttonSelectMotion = new System.Windows.Forms.Button();
            this.buttonSelectLightCtrl = new System.Windows.Forms.Button();
            this.buttonSelectDigitalIo = new System.Windows.Forms.Button();
            this.buttonSelectDaq = new System.Windows.Forms.Button();
            this.dataGridViewDeviceList = new System.Windows.Forms.DataGridView();
            this.columnNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnNumPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonSelectDepthScanner = new System.Windows.Forms.Button();
            this.buttonSerial = new System.Windows.Forms.Button();
            this.checkUseDoorSensor = new System.Windows.Forms.CheckBox();
            this.checkModelBarcode = new System.Windows.Forms.CheckBox();
            this.groupLight = new System.Windows.Forms.GroupBox();
            this.checkBoxUseFovNavigator = new System.Windows.Forms.CheckBox();
            this.buttonAxisHandlerConfiguration = new System.Windows.Forms.Button();
            this.buttonPortMap = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.buttonCameraCalibration = new System.Windows.Forms.Button();
            this.checkUseAirPressure = new System.Windows.Forms.CheckBox();
            this.checkUseEmergencyStop = new System.Windows.Forms.CheckBox();
            this.checkUseTowerLamp = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDeviceList)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSelectGrabber
            // 
            this.buttonSelectGrabber.Location = new System.Drawing.Point(-1, -1);
            this.buttonSelectGrabber.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSelectGrabber.Name = "buttonSelectGrabber";
            this.buttonSelectGrabber.Size = new System.Drawing.Size(131, 50);
            this.buttonSelectGrabber.TabIndex = 157;
            this.buttonSelectGrabber.Text = "Grabber";
            this.buttonSelectGrabber.UseVisualStyleBackColor = true;
            this.buttonSelectGrabber.Click += new System.EventHandler(this.buttonSelectGrabber_Click);
            // 
            // buttonSelectMotion
            // 
            this.buttonSelectMotion.Location = new System.Drawing.Point(-1, 50);
            this.buttonSelectMotion.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSelectMotion.Name = "buttonSelectMotion";
            this.buttonSelectMotion.Size = new System.Drawing.Size(131, 50);
            this.buttonSelectMotion.TabIndex = 157;
            this.buttonSelectMotion.Text = "Motion";
            this.buttonSelectMotion.UseVisualStyleBackColor = true;
            this.buttonSelectMotion.Click += new System.EventHandler(this.buttonSelectMotion_Click);
            // 
            // buttonSelectLightCtrl
            // 
            this.buttonSelectLightCtrl.Location = new System.Drawing.Point(-1, 152);
            this.buttonSelectLightCtrl.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSelectLightCtrl.Name = "buttonSelectLightCtrl";
            this.buttonSelectLightCtrl.Size = new System.Drawing.Size(131, 50);
            this.buttonSelectLightCtrl.TabIndex = 157;
            this.buttonSelectLightCtrl.Text = "Light";
            this.buttonSelectLightCtrl.UseVisualStyleBackColor = true;
            this.buttonSelectLightCtrl.Click += new System.EventHandler(this.buttonSelectLightCtrl_Click);
            // 
            // buttonSelectDigitalIo
            // 
            this.buttonSelectDigitalIo.Location = new System.Drawing.Point(-1, 101);
            this.buttonSelectDigitalIo.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSelectDigitalIo.Name = "buttonSelectDigitalIo";
            this.buttonSelectDigitalIo.Size = new System.Drawing.Size(131, 50);
            this.buttonSelectDigitalIo.TabIndex = 157;
            this.buttonSelectDigitalIo.Text = "DIO";
            this.buttonSelectDigitalIo.UseVisualStyleBackColor = true;
            this.buttonSelectDigitalIo.Click += new System.EventHandler(this.buttonSelectDigitalIo_Click);
            // 
            // buttonSelectDaq
            // 
            this.buttonSelectDaq.Location = new System.Drawing.Point(-1, 203);
            this.buttonSelectDaq.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSelectDaq.Name = "buttonSelectDaq";
            this.buttonSelectDaq.Size = new System.Drawing.Size(131, 50);
            this.buttonSelectDaq.TabIndex = 157;
            this.buttonSelectDaq.Text = "DAQ";
            this.buttonSelectDaq.UseVisualStyleBackColor = true;
            this.buttonSelectDaq.Click += new System.EventHandler(this.buttonSelectDaq_Click);
            // 
            // dataGridViewDeviceList
            // 
            this.dataGridViewDeviceList.AllowUserToAddRows = false;
            this.dataGridViewDeviceList.AllowUserToDeleteRows = false;
            this.dataGridViewDeviceList.AllowUserToResizeColumns = false;
            this.dataGridViewDeviceList.AllowUserToResizeRows = false;
            this.dataGridViewDeviceList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewDeviceList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDeviceList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnNo,
            this.columnName,
            this.columnType,
            this.columnNumPort});
            this.dataGridViewDeviceList.Location = new System.Drawing.Point(133, 50);
            this.dataGridViewDeviceList.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridViewDeviceList.Name = "dataGridViewDeviceList";
            this.dataGridViewDeviceList.ReadOnly = true;
            this.dataGridViewDeviceList.RowHeadersVisible = false;
            this.dataGridViewDeviceList.RowTemplate.Height = 23;
            this.dataGridViewDeviceList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewDeviceList.Size = new System.Drawing.Size(451, 254);
            this.dataGridViewDeviceList.TabIndex = 158;
            this.dataGridViewDeviceList.SelectionChanged += new System.EventHandler(this.dataGridViewDeviceList_SelectionChanged);
            // 
            // columnNo
            // 
            this.columnNo.HeaderText = "No";
            this.columnNo.Name = "columnNo";
            this.columnNo.ReadOnly = true;
            this.columnNo.Width = 50;
            // 
            // columnName
            // 
            this.columnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnName.HeaderText = "Name";
            this.columnName.Name = "columnName";
            this.columnName.ReadOnly = true;
            // 
            // columnType
            // 
            this.columnType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnType.HeaderText = "Type";
            this.columnType.Name = "columnType";
            this.columnType.ReadOnly = true;
            // 
            // columnNumPort
            // 
            this.columnNumPort.HeaderText = "Num Port";
            this.columnNumPort.Name = "columnNumPort";
            this.columnNumPort.ReadOnly = true;
            this.columnNumPort.Width = 150;
            // 
            // buttonSelectDepthScanner
            // 
            this.buttonSelectDepthScanner.Location = new System.Drawing.Point(-1, 254);
            this.buttonSelectDepthScanner.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSelectDepthScanner.Name = "buttonSelectDepthScanner";
            this.buttonSelectDepthScanner.Size = new System.Drawing.Size(131, 50);
            this.buttonSelectDepthScanner.TabIndex = 157;
            this.buttonSelectDepthScanner.Text = "Depth Scanner";
            this.buttonSelectDepthScanner.UseVisualStyleBackColor = true;
            this.buttonSelectDepthScanner.Click += new System.EventHandler(this.buttonSelectDepthScanner_Click);
            // 
            // buttonSerial
            // 
            this.buttonSerial.Location = new System.Drawing.Point(0, 306);
            this.buttonSerial.Margin = new System.Windows.Forms.Padding(4);
            this.buttonSerial.Name = "buttonSerial";
            this.buttonSerial.Size = new System.Drawing.Size(131, 50);
            this.buttonSerial.TabIndex = 157;
            this.buttonSerial.Text = "Serial";
            this.buttonSerial.UseVisualStyleBackColor = true;
            this.buttonSerial.Click += new System.EventHandler(this.buttonSerial_Click);
            // 
            // checkUseDoorSensor
            // 
            this.checkUseDoorSensor.AutoSize = true;
            this.checkUseDoorSensor.Location = new System.Drawing.Point(461, 460);
            this.checkUseDoorSensor.Name = "checkUseDoorSensor";
            this.checkUseDoorSensor.Size = new System.Drawing.Size(120, 22);
            this.checkUseDoorSensor.TabIndex = 166;
            this.checkUseDoorSensor.Text = "Use Doorlock";
            this.checkUseDoorSensor.UseVisualStyleBackColor = true;
            // 
            // checkModelBarcode
            // 
            this.checkModelBarcode.AutoSize = true;
            this.checkModelBarcode.Location = new System.Drawing.Point(138, 523);
            this.checkModelBarcode.Name = "checkModelBarcode";
            this.checkModelBarcode.Size = new System.Drawing.Size(157, 22);
            this.checkModelBarcode.TabIndex = 166;
            this.checkModelBarcode.Text = "Use model barcode";
            this.checkModelBarcode.UseVisualStyleBackColor = true;
            // 
            // groupLight
            // 
            this.groupLight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupLight.Location = new System.Drawing.Point(133, 306);
            this.groupLight.Name = "groupLight";
            this.groupLight.Size = new System.Drawing.Size(448, 140);
            this.groupLight.TabIndex = 167;
            this.groupLight.TabStop = false;
            this.groupLight.Text = "Light Option";
            // 
            // checkBoxUseFovNavigator
            // 
            this.checkBoxUseFovNavigator.AutoSize = true;
            this.checkBoxUseFovNavigator.Location = new System.Drawing.Point(301, 523);
            this.checkBoxUseFovNavigator.Name = "checkBoxUseFovNavigator";
            this.checkBoxUseFovNavigator.Size = new System.Drawing.Size(146, 22);
            this.checkBoxUseFovNavigator.TabIndex = 166;
            this.checkBoxUseFovNavigator.Text = "Use FovNavigator";
            this.checkBoxUseFovNavigator.UseVisualStyleBackColor = true;
            // 
            // buttonAxisHandlerConfiguration
            // 
            this.buttonAxisHandlerConfiguration.Location = new System.Drawing.Point(-1, 396);
            this.buttonAxisHandlerConfiguration.Margin = new System.Windows.Forms.Padding(4);
            this.buttonAxisHandlerConfiguration.Name = "buttonAxisHandlerConfiguration";
            this.buttonAxisHandlerConfiguration.Size = new System.Drawing.Size(131, 50);
            this.buttonAxisHandlerConfiguration.TabIndex = 168;
            this.buttonAxisHandlerConfiguration.Text = "Axis Handler";
            this.buttonAxisHandlerConfiguration.UseVisualStyleBackColor = true;
            this.buttonAxisHandlerConfiguration.Click += new System.EventHandler(this.buttonAxisHandlerConfiguration_Click);
            // 
            // buttonPortMap
            // 
            this.buttonPortMap.Location = new System.Drawing.Point(0, 449);
            this.buttonPortMap.Margin = new System.Windows.Forms.Padding(4);
            this.buttonPortMap.Name = "buttonPortMap";
            this.buttonPortMap.Size = new System.Drawing.Size(131, 50);
            this.buttonPortMap.TabIndex = 169;
            this.buttonPortMap.Text = "IO Port Map";
            this.buttonPortMap.UseVisualStyleBackColor = true;
            this.buttonPortMap.Click += new System.EventHandler(this.buttonPortMap_Click);
            // 
            // editButton
            // 
            this.editButton.Enabled = false;
            this.editButton.Image = global::UniEye.Base.Properties.Resources.edit_32;
            this.editButton.Location = new System.Drawing.Point(221, -1);
            this.editButton.Margin = new System.Windows.Forms.Padding(4);
            this.editButton.Name = "editButton";
            this.editButton.Size = new System.Drawing.Size(79, 50);
            this.editButton.TabIndex = 159;
            this.editButton.Text = "Edit";
            this.editButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.editButton.UseVisualStyleBackColor = true;
            this.editButton.Click += new System.EventHandler(this.editButton_Click);
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Image = global::UniEye.Base.Properties.Resources.arrow_down;
            this.buttonMoveDown.Location = new System.Drawing.Point(494, -1);
            this.buttonMoveDown.Margin = new System.Windows.Forms.Padding(4);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(91, 50);
            this.buttonMoveDown.TabIndex = 160;
            this.buttonMoveDown.Text = "Down";
            this.buttonMoveDown.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Image = global::UniEye.Base.Properties.Resources.arrow_up;
            this.buttonMoveUp.Location = new System.Drawing.Point(417, -1);
            this.buttonMoveUp.Margin = new System.Windows.Forms.Padding(4);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(69, 50);
            this.buttonMoveUp.TabIndex = 160;
            this.buttonMoveUp.Text = "Up";
            this.buttonMoveUp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Enabled = false;
            this.deleteButton.Image = global::UniEye.Base.Properties.Resources.delete_32;
            this.deleteButton.Location = new System.Drawing.Point(300, -1);
            this.deleteButton.Margin = new System.Windows.Forms.Padding(4);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(92, 50);
            this.deleteButton.TabIndex = 160;
            this.deleteButton.Text = "Delete";
            this.deleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // addButton
            // 
            this.addButton.Image = global::UniEye.Base.Properties.Resources.add_32;
            this.addButton.Location = new System.Drawing.Point(133, -1);
            this.addButton.Margin = new System.Windows.Forms.Padding(4);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(88, 50);
            this.addButton.TabIndex = 161;
            this.addButton.Text = "Add";
            this.addButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // buttonCameraCalibration
            // 
            this.buttonCameraCalibration.Location = new System.Drawing.Point(-1, 502);
            this.buttonCameraCalibration.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCameraCalibration.Name = "buttonCameraCalibration";
            this.buttonCameraCalibration.Size = new System.Drawing.Size(131, 50);
            this.buttonCameraCalibration.TabIndex = 170;
            this.buttonCameraCalibration.Text = "Camera Calibration";
            this.buttonCameraCalibration.UseVisualStyleBackColor = true;
            this.buttonCameraCalibration.Click += new System.EventHandler(this.buttonCameraCalibration_Click);
            // 
            // checkUseAirPressure
            // 
            this.checkUseAirPressure.AutoSize = true;
            this.checkUseAirPressure.Location = new System.Drawing.Point(312, 460);
            this.checkUseAirPressure.Name = "checkUseAirPressure";
            this.checkUseAirPressure.Size = new System.Drawing.Size(139, 22);
            this.checkUseAirPressure.TabIndex = 171;
            this.checkUseAirPressure.Text = "Use Air Pressure";
            this.checkUseAirPressure.UseVisualStyleBackColor = true;
            // 
            // checkUseEmergencyStop
            // 
            this.checkUseEmergencyStop.AutoSize = true;
            this.checkUseEmergencyStop.Location = new System.Drawing.Point(138, 460);
            this.checkUseEmergencyStop.Name = "checkUseEmergencyStop";
            this.checkUseEmergencyStop.Size = new System.Drawing.Size(168, 22);
            this.checkUseEmergencyStop.TabIndex = 172;
            this.checkUseEmergencyStop.Text = "Use Emergency Stop";
            this.checkUseEmergencyStop.UseVisualStyleBackColor = true;
            // 
            // checkUseTowerLamp
            // 
            this.checkUseTowerLamp.AutoSize = true;
            this.checkUseTowerLamp.Location = new System.Drawing.Point(138, 487);
            this.checkUseTowerLamp.Name = "checkUseTowerLamp";
            this.checkUseTowerLamp.Size = new System.Drawing.Size(137, 22);
            this.checkUseTowerLamp.TabIndex = 173;
            this.checkUseTowerLamp.Text = "Use TowerLamp";
            this.checkUseTowerLamp.UseVisualStyleBackColor = true;
            // 
            // ConfigDevicePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkUseTowerLamp);
            this.Controls.Add(this.checkUseEmergencyStop);
            this.Controls.Add(this.checkUseAirPressure);
            this.Controls.Add(this.buttonCameraCalibration);
            this.Controls.Add(this.buttonPortMap);
            this.Controls.Add(this.buttonAxisHandlerConfiguration);
            this.Controls.Add(this.checkBoxUseFovNavigator);
            this.Controls.Add(this.groupLight);
            this.Controls.Add(this.checkModelBarcode);
            this.Controls.Add(this.checkUseDoorSensor);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.buttonMoveDown);
            this.Controls.Add(this.buttonMoveUp);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.dataGridViewDeviceList);
            this.Controls.Add(this.buttonSerial);
            this.Controls.Add(this.buttonSelectDepthScanner);
            this.Controls.Add(this.buttonSelectDaq);
            this.Controls.Add(this.buttonSelectLightCtrl);
            this.Controls.Add(this.buttonSelectDigitalIo);
            this.Controls.Add(this.buttonSelectMotion);
            this.Controls.Add(this.buttonSelectGrabber);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ConfigDevicePanel";
            this.Size = new System.Drawing.Size(588, 564);
            this.Load += new System.EventHandler(this.ConfigDevicePanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDeviceList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSelectGrabber;
        private System.Windows.Forms.Button buttonSelectMotion;
        private System.Windows.Forms.Button buttonSelectLightCtrl;
        private System.Windows.Forms.Button buttonSelectDigitalIo;
        private System.Windows.Forms.Button buttonSelectDaq;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.DataGridView dataGridViewDeviceList;
        private System.Windows.Forms.Button buttonSelectDepthScanner;
        private System.Windows.Forms.Button buttonMoveUp;
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnNumPort;
        private System.Windows.Forms.Button buttonSerial;
        private System.Windows.Forms.CheckBox checkUseDoorSensor;
        private System.Windows.Forms.CheckBox checkModelBarcode;
        private System.Windows.Forms.GroupBox groupLight;
        private System.Windows.Forms.CheckBox checkBoxUseFovNavigator;
        private System.Windows.Forms.Button buttonAxisHandlerConfiguration;
        private System.Windows.Forms.Button buttonPortMap;
        private System.Windows.Forms.Button buttonCameraCalibration;
        private System.Windows.Forms.CheckBox checkUseAirPressure;
        private System.Windows.Forms.CheckBox checkUseEmergencyStop;
        private System.Windows.Forms.CheckBox checkUseTowerLamp;
    }
}
