namespace DynMvp.Devices.UI
{
    partial class SerialPortSettingForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            this.labelBaudRate = new System.Windows.Forms.Label();
            this.comboBaudRate = new System.Windows.Forms.ComboBox();
            this.labelDataBits = new System.Windows.Forms.Label();
            this.comboDataBits = new System.Windows.Forms.ComboBox();
            this.labelParity = new System.Windows.Forms.Label();
            this.comboParity = new System.Windows.Forms.ComboBox();
            this.labelStopBits = new System.Windows.Forms.Label();
            this.comboStopBits = new System.Windows.Forms.ComboBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.checkDtrEnable = new System.Windows.Forms.CheckBox();
            this.checkRtsEnable = new System.Windows.Forms.CheckBox();
            this.comboHandshake = new System.Windows.Forms.ComboBox();
            this.labelHandshake = new System.Windows.Forms.Label();
            this.labelPortNo = new System.Windows.Forms.Label();
            this.comboPortName = new System.Windows.Forms.ComboBox();
            this.ultraFormManager = new Infragistics.Win.UltraWinForm.UltraFormManager(this.components);
            this._ConfigPage_UltraFormManager_Dock_Area_Top = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Left = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this._ConfigPage_UltraFormManager_Dock_Area_Right = new Infragistics.Win.UltraWinForm.UltraFormDockArea();
            this.SerialPortSettingForm_Fill_Panel = new Infragistics.Win.Misc.UltraPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBoxProperty = new Infragistics.Win.Misc.UltraGroupBox();
            this.btnFindPort = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboSensorType = new System.Windows.Forms.ComboBox();
            this.comboDeviceType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).BeginInit();
            this.SerialPortSettingForm_Fill_Panel.ClientArea.SuspendLayout();
            this.SerialPortSettingForm_Fill_Panel.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxProperty)).BeginInit();
            this.groupBoxProperty.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelBaudRate
            // 
            this.labelBaudRate.AutoSize = true;
            this.labelBaudRate.Location = new System.Drawing.Point(7, 66);
            this.labelBaudRate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelBaudRate.Name = "labelBaudRate";
            this.labelBaudRate.Size = new System.Drawing.Size(77, 18);
            this.labelBaudRate.TabIndex = 0;
            this.labelBaudRate.Text = "Baud Rate";
            // 
            // comboBaudRate
            // 
            this.comboBaudRate.FormattingEnabled = true;
            this.comboBaudRate.Items.AddRange(new object[] {
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "1250000"});
            this.comboBaudRate.Location = new System.Drawing.Point(93, 62);
            this.comboBaudRate.Margin = new System.Windows.Forms.Padding(4);
            this.comboBaudRate.Name = "comboBaudRate";
            this.comboBaudRate.Size = new System.Drawing.Size(94, 26);
            this.comboBaudRate.TabIndex = 1;
            // 
            // labelDataBits
            // 
            this.labelDataBits.AutoSize = true;
            this.labelDataBits.Location = new System.Drawing.Point(7, 101);
            this.labelDataBits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDataBits.Name = "labelDataBits";
            this.labelDataBits.Size = new System.Drawing.Size(68, 18);
            this.labelDataBits.TabIndex = 0;
            this.labelDataBits.Text = "Data Bits";
            // 
            // comboDataBits
            // 
            this.comboDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDataBits.FormattingEnabled = true;
            this.comboDataBits.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.comboDataBits.Location = new System.Drawing.Point(93, 97);
            this.comboDataBits.Margin = new System.Windows.Forms.Padding(4);
            this.comboDataBits.Name = "comboDataBits";
            this.comboDataBits.Size = new System.Drawing.Size(130, 26);
            this.comboDataBits.TabIndex = 1;
            // 
            // labelParity
            // 
            this.labelParity.AutoSize = true;
            this.labelParity.Location = new System.Drawing.Point(7, 134);
            this.labelParity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelParity.Name = "labelParity";
            this.labelParity.Size = new System.Drawing.Size(45, 18);
            this.labelParity.TabIndex = 0;
            this.labelParity.Text = "Parity";
            // 
            // comboParity
            // 
            this.comboParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboParity.FormattingEnabled = true;
            this.comboParity.Items.AddRange(new object[] {
            "None",
            "Even",
            "Odd",
            "Mark",
            "Space"});
            this.comboParity.Location = new System.Drawing.Point(93, 130);
            this.comboParity.Margin = new System.Windows.Forms.Padding(4);
            this.comboParity.Name = "comboParity";
            this.comboParity.Size = new System.Drawing.Size(130, 26);
            this.comboParity.TabIndex = 1;
            // 
            // labelStopBits
            // 
            this.labelStopBits.AutoSize = true;
            this.labelStopBits.Location = new System.Drawing.Point(7, 167);
            this.labelStopBits.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStopBits.Name = "labelStopBits";
            this.labelStopBits.Size = new System.Drawing.Size(68, 18);
            this.labelStopBits.TabIndex = 0;
            this.labelStopBits.Text = "Stop Bits";
            // 
            // comboStopBits
            // 
            this.comboStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboStopBits.FormattingEnabled = true;
            this.comboStopBits.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.comboStopBits.Location = new System.Drawing.Point(93, 164);
            this.comboStopBits.Margin = new System.Windows.Forms.Padding(4);
            this.comboStopBits.Name = "comboStopBits";
            this.comboStopBits.Size = new System.Drawing.Size(130, 26);
            this.comboStopBits.TabIndex = 1;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(39, 8);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 34);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(117, 8);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 34);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // checkDtrEnable
            // 
            this.checkDtrEnable.AutoSize = true;
            this.checkDtrEnable.Location = new System.Drawing.Point(116, 233);
            this.checkDtrEnable.Name = "checkDtrEnable";
            this.checkDtrEnable.Size = new System.Drawing.Size(107, 22);
            this.checkDtrEnable.TabIndex = 5;
            this.checkDtrEnable.Text = "DTR Enable";
            this.checkDtrEnable.UseVisualStyleBackColor = true;
            // 
            // checkRtsEnable
            // 
            this.checkRtsEnable.AutoSize = true;
            this.checkRtsEnable.Location = new System.Drawing.Point(9, 233);
            this.checkRtsEnable.Name = "checkRtsEnable";
            this.checkRtsEnable.Size = new System.Drawing.Size(106, 22);
            this.checkRtsEnable.TabIndex = 4;
            this.checkRtsEnable.Text = "RTS Enable";
            this.checkRtsEnable.UseVisualStyleBackColor = true;
            // 
            // comboHandshake
            // 
            this.comboHandshake.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboHandshake.FormattingEnabled = true;
            this.comboHandshake.Items.AddRange(new object[] {
            "None",
            "RequestToSend",
            "RequestToSendXOnXOff",
            "Handshake.XOnXOff"});
            this.comboHandshake.Location = new System.Drawing.Point(93, 198);
            this.comboHandshake.Margin = new System.Windows.Forms.Padding(4);
            this.comboHandshake.Name = "comboHandshake";
            this.comboHandshake.Size = new System.Drawing.Size(130, 26);
            this.comboHandshake.TabIndex = 3;
            // 
            // labelHandshake
            // 
            this.labelHandshake.AutoSize = true;
            this.labelHandshake.Location = new System.Drawing.Point(7, 201);
            this.labelHandshake.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelHandshake.Name = "labelHandshake";
            this.labelHandshake.Size = new System.Drawing.Size(83, 18);
            this.labelHandshake.TabIndex = 2;
            this.labelHandshake.Text = "Handshake";
            // 
            // labelPortNo
            // 
            this.labelPortNo.AutoSize = true;
            this.labelPortNo.Location = new System.Drawing.Point(7, 30);
            this.labelPortNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPortNo.Name = "labelPortNo";
            this.labelPortNo.Size = new System.Drawing.Size(60, 18);
            this.labelPortNo.TabIndex = 0;
            this.labelPortNo.Text = "Port No";
            // 
            // comboPortName
            // 
            this.comboPortName.FormattingEnabled = true;
            this.comboPortName.Items.AddRange(new object[] {
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.comboPortName.Location = new System.Drawing.Point(93, 27);
            this.comboPortName.Margin = new System.Windows.Forms.Padding(4);
            this.comboPortName.Name = "comboPortName";
            this.comboPortName.Size = new System.Drawing.Size(94, 26);
            this.comboPortName.TabIndex = 1;
            // 
            // ultraFormManager
            // 
            this.ultraFormManager.Form = this;
            appearance1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            appearance1.TextHAlignAsString = "Left";
            this.ultraFormManager.FormStyleSettings.CaptionAreaAppearance = appearance1;
            appearance2.BorderAlpha = Infragistics.Win.Alpha.Transparent;
            appearance2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(120)))), ((int)(((byte)(120)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.Appearance = appearance2;
            appearance3.BackColor = System.Drawing.Color.Transparent;
            appearance3.ForeColor = System.Drawing.Color.White;
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.HotTrackAppearance = appearance3;
            appearance4.BackColor = System.Drawing.Color.Transparent;
            appearance4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(168)))), ((int)(((byte)(12)))));
            this.ultraFormManager.FormStyleSettings.CaptionButtonsAppearances.DefaultButtonAppearances.PressedAppearance = appearance4;
            this.ultraFormManager.FormStyleSettings.Style = Infragistics.Win.UltraWinForm.UltraFormStyle.Office2013;
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Top
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Top;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Name = "_ConfigPage_UltraFormManager_Dock_Area_Top";
            this._ConfigPage_UltraFormManager_Dock_Area_Top.Size = new System.Drawing.Size(236, 31);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Bottom
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Bottom;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 447);
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Name = "_ConfigPage_UltraFormManager_Dock_Area_Bottom";
            this._ConfigPage_UltraFormManager_Dock_Area_Bottom.Size = new System.Drawing.Size(236, 1);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Left
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Left;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Location = new System.Drawing.Point(0, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Name = "_ConfigPage_UltraFormManager_Dock_Area_Left";
            this._ConfigPage_UltraFormManager_Dock_Area_Left.Size = new System.Drawing.Size(1, 416);
            // 
            // _ConfigPage_UltraFormManager_Dock_Area_Right
            // 
            this._ConfigPage_UltraFormManager_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this._ConfigPage_UltraFormManager_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinForm.DockedPosition.Right;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.FormManager = this.ultraFormManager;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.InitialResizeAreaExtent = 1;
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Location = new System.Drawing.Point(235, 31);
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Name = "_ConfigPage_UltraFormManager_Dock_Area_Right";
            this._ConfigPage_UltraFormManager_Dock_Area_Right.Size = new System.Drawing.Size(1, 416);
            // 
            // SerialPortSettingForm_Fill_Panel
            // 
            this.SerialPortSettingForm_Fill_Panel.AutoSize = true;
            this.SerialPortSettingForm_Fill_Panel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            // 
            // SerialPortSettingForm_Fill_Panel.ClientArea
            // 
            this.SerialPortSettingForm_Fill_Panel.ClientArea.Controls.Add(this.panel1);
            this.SerialPortSettingForm_Fill_Panel.ClientArea.Controls.Add(this.groupBoxProperty);
            this.SerialPortSettingForm_Fill_Panel.ClientArea.Controls.Add(this.panel2);
            this.SerialPortSettingForm_Fill_Panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.SerialPortSettingForm_Fill_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SerialPortSettingForm_Fill_Panel.Location = new System.Drawing.Point(1, 31);
            this.SerialPortSettingForm_Fill_Panel.Name = "SerialPortSettingForm_Fill_Panel";
            this.SerialPortSettingForm_Fill_Panel.Size = new System.Drawing.Size(234, 416);
            this.SerialPortSettingForm_Fill_Panel.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 366);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(234, 49);
            this.panel1.TabIndex = 6;
            // 
            // groupBoxProperty
            // 
            this.groupBoxProperty.Controls.Add(this.btnFindPort);
            this.groupBoxProperty.Controls.Add(this.checkDtrEnable);
            this.groupBoxProperty.Controls.Add(this.labelPortNo);
            this.groupBoxProperty.Controls.Add(this.checkRtsEnable);
            this.groupBoxProperty.Controls.Add(this.comboDataBits);
            this.groupBoxProperty.Controls.Add(this.comboHandshake);
            this.groupBoxProperty.Controls.Add(this.comboBaudRate);
            this.groupBoxProperty.Controls.Add(this.labelHandshake);
            this.groupBoxProperty.Controls.Add(this.comboPortName);
            this.groupBoxProperty.Controls.Add(this.comboParity);
            this.groupBoxProperty.Controls.Add(this.labelBaudRate);
            this.groupBoxProperty.Controls.Add(this.labelStopBits);
            this.groupBoxProperty.Controls.Add(this.labelDataBits);
            this.groupBoxProperty.Controls.Add(this.comboStopBits);
            this.groupBoxProperty.Controls.Add(this.labelParity);
            this.groupBoxProperty.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxProperty.Location = new System.Drawing.Point(0, 101);
            this.groupBoxProperty.Name = "groupBoxProperty";
            this.groupBoxProperty.Size = new System.Drawing.Size(234, 265);
            this.groupBoxProperty.TabIndex = 7;
            this.groupBoxProperty.Text = "Property";
            // 
            // btnFindPort
            // 
            this.btnFindPort.Location = new System.Drawing.Point(195, 27);
            this.btnFindPort.Margin = new System.Windows.Forms.Padding(4);
            this.btnFindPort.Name = "btnFindPort";
            this.btnFindPort.Size = new System.Drawing.Size(28, 61);
            this.btnFindPort.TabIndex = 2;
            this.btnFindPort.Text = "?";
            this.btnFindPort.UseVisualStyleBackColor = true;
            this.btnFindPort.Click += new System.EventHandler(this.btnFindPort_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.comboSensorType);
            this.panel2.Controls.Add(this.comboDeviceType);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.textBoxName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(234, 101);
            this.panel2.TabIndex = 3;
            // 
            // comboSensorType
            // 
            this.comboSensorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSensorType.FormattingEnabled = true;
            this.comboSensorType.Location = new System.Drawing.Point(93, 68);
            this.comboSensorType.Margin = new System.Windows.Forms.Padding(4);
            this.comboSensorType.Name = "comboSensorType";
            this.comboSensorType.Size = new System.Drawing.Size(130, 26);
            this.comboSensorType.TabIndex = 7;
            this.comboSensorType.SelectedValueChanged += new System.EventHandler(this.comboSensorType_SelectedValueChanged);
            // 
            // comboDeviceType
            // 
            this.comboDeviceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboDeviceType.FormattingEnabled = true;
            this.comboDeviceType.Location = new System.Drawing.Point(93, 38);
            this.comboDeviceType.Margin = new System.Windows.Forms.Padding(4);
            this.comboDeviceType.Name = "comboDeviceType";
            this.comboDeviceType.Size = new System.Drawing.Size(130, 26);
            this.comboDeviceType.TabIndex = 7;
            this.comboDeviceType.SelectedValueChanged += new System.EventHandler(this.comboDeviceType_SelectedValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 18);
            this.label1.TabIndex = 4;
            this.label1.Text = "Name";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(93, 10);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(130, 24);
            this.textBoxName.TabIndex = 5;
            // 
            // SerialPortSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(236, 448);
            this.Controls.Add(this.SerialPortSettingForm_Fill_Panel);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Left);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Right);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Top);
            this.Controls.Add(this._ConfigPage_UltraFormManager_Dock_Area_Bottom);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(236, 39);
            this.Name = "SerialPortSettingForm";
            this.Text = "Port Settings";
            this.Load += new System.EventHandler(this.SerialPortSettingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ultraFormManager)).EndInit();
            this.SerialPortSettingForm_Fill_Panel.ClientArea.ResumeLayout(false);
            this.SerialPortSettingForm_Fill_Panel.ResumeLayout(false);
            this.SerialPortSettingForm_Fill_Panel.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupBoxProperty)).EndInit();
            this.groupBoxProperty.ResumeLayout(false);
            this.groupBoxProperty.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelBaudRate;
        private System.Windows.Forms.ComboBox comboBaudRate;
        private System.Windows.Forms.Label labelDataBits;
        private System.Windows.Forms.ComboBox comboDataBits;
        private System.Windows.Forms.Label labelParity;
        private System.Windows.Forms.ComboBox comboParity;
        private System.Windows.Forms.Label labelStopBits;
        private System.Windows.Forms.ComboBox comboStopBits;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label labelPortNo;
        private System.Windows.Forms.ComboBox comboPortName;
        private Infragistics.Win.UltraWinForm.UltraFormManager ultraFormManager;
        private Infragistics.Win.Misc.UltraPanel SerialPortSettingForm_Fill_Panel;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Left;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Right;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Top;
        private Infragistics.Win.UltraWinForm.UltraFormDockArea _ConfigPage_UltraFormManager_Dock_Area_Bottom;
        private System.Windows.Forms.ComboBox comboHandshake;
        private System.Windows.Forms.Label labelHandshake;
        private System.Windows.Forms.CheckBox checkDtrEnable;
        private System.Windows.Forms.CheckBox checkRtsEnable;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox comboDeviceType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private Infragistics.Win.Misc.UltraGroupBox groupBoxProperty;
        private System.Windows.Forms.ComboBox comboSensorType;
        private System.Windows.Forms.Button btnFindPort;
    }
}