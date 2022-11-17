namespace UniScanX.MPAlignment.UI.Pages
{
    partial class ConfigForm
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
            this.tpMain = new System.Windows.Forms.TabControl();
            this.tbGeneral = new System.Windows.Forms.TabPage();
            this.virtualMode = new System.Windows.Forms.CheckBox();
            this.programTitle = new System.Windows.Forms.TextBox();
            this.labelProgramTitle = new System.Windows.Forms.Label();
            this.imagingLibrary = new System.Windows.Forms.ComboBox();
            this.labelImagingLibrary = new System.Windows.Forms.Label();
            this.tbDevice = new System.Windows.Forms.TabPage();
            this.lblVirtualImageInfo = new System.Windows.Forms.Label();
            this.txtVirtualImagePath = new System.Windows.Forms.TextBox();
            this.labelDioType = new System.Windows.Forms.Label();
            this.cmbDioType = new System.Windows.Forms.ComboBox();
            this.buttonVirtual = new System.Windows.Forms.Button();
            this.cmbGrabberType = new System.Windows.Forms.ComboBox();
            this.nudCamera = new System.Windows.Forms.NumericUpDown();
            this.lblCamera = new System.Windows.Forms.Label();
            this.buttonCameraConfiguration = new System.Windows.Forms.Button();
            this.tbResult = new System.Windows.Forms.TabPage();
            this.lblPostgresSetting = new System.Windows.Forms.Label();
            this.btnPostgresSetting = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.tpMain.SuspendLayout();
            this.tbGeneral.SuspendLayout();
            this.tbDevice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCamera)).BeginInit();
            this.tbResult.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpMain
            // 
            this.tpMain.Controls.Add(this.tbGeneral);
            this.tpMain.Controls.Add(this.tbDevice);
            this.tpMain.Controls.Add(this.tbResult);
            this.tpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpMain.Location = new System.Drawing.Point(0, 0);
            this.tpMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tpMain.Name = "tpMain";
            this.tpMain.SelectedIndex = 0;
            this.tpMain.Size = new System.Drawing.Size(531, 507);
            this.tpMain.TabIndex = 0;
            // 
            // tbGeneral
            // 
            this.tbGeneral.Controls.Add(this.virtualMode);
            this.tbGeneral.Controls.Add(this.programTitle);
            this.tbGeneral.Controls.Add(this.labelProgramTitle);
            this.tbGeneral.Controls.Add(this.imagingLibrary);
            this.tbGeneral.Controls.Add(this.labelImagingLibrary);
            this.tbGeneral.Location = new System.Drawing.Point(8, 59);
            this.tbGeneral.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbGeneral.Name = "tbGeneral";
            this.tbGeneral.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbGeneral.Size = new System.Drawing.Size(515, 440);
            this.tbGeneral.TabIndex = 0;
            this.tbGeneral.Text = "General";
            this.tbGeneral.UseVisualStyleBackColor = true;
            // 
            // virtualMode
            // 
            this.virtualMode.AutoSize = true;
            this.virtualMode.Location = new System.Drawing.Point(19, 132);
            this.virtualMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.virtualMode.Name = "virtualMode";
            this.virtualMode.Size = new System.Drawing.Size(241, 49);
            this.virtualMode.TabIndex = 164;
            this.virtualMode.Text = "Virtual Mode";
            this.virtualMode.UseVisualStyleBackColor = true;
            // 
            // programTitle
            // 
            this.programTitle.Location = new System.Drawing.Point(236, 7);
            this.programTitle.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.programTitle.Name = "programTitle";
            this.programTitle.Size = new System.Drawing.Size(268, 50);
            this.programTitle.TabIndex = 161;
            // 
            // labelProgramTitle
            // 
            this.labelProgramTitle.AutoSize = true;
            this.labelProgramTitle.Location = new System.Drawing.Point(11, 13);
            this.labelProgramTitle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.labelProgramTitle.Name = "labelProgramTitle";
            this.labelProgramTitle.Size = new System.Drawing.Size(215, 45);
            this.labelProgramTitle.TabIndex = 160;
            this.labelProgramTitle.Text = "Program Title";
            // 
            // imagingLibrary
            // 
            this.imagingLibrary.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.imagingLibrary.FormattingEnabled = true;
            this.imagingLibrary.Items.AddRange(new object[] {
            "Open CV",
            "Open eVision",
            "VisionPro",
            "MIL",
            "Halcon",
            "Custom"});
            this.imagingLibrary.Location = new System.Drawing.Point(266, 64);
            this.imagingLibrary.Margin = new System.Windows.Forms.Padding(8, 14, 8, 14);
            this.imagingLibrary.Name = "imagingLibrary";
            this.imagingLibrary.Size = new System.Drawing.Size(201, 53);
            this.imagingLibrary.TabIndex = 163;
            // 
            // labelImagingLibrary
            // 
            this.labelImagingLibrary.AutoSize = true;
            this.labelImagingLibrary.Location = new System.Drawing.Point(12, 64);
            this.labelImagingLibrary.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelImagingLibrary.Name = "labelImagingLibrary";
            this.labelImagingLibrary.Size = new System.Drawing.Size(247, 45);
            this.labelImagingLibrary.TabIndex = 162;
            this.labelImagingLibrary.Text = "Imaging Library";
            // 
            // tbDevice
            // 
            this.tbDevice.Controls.Add(this.lblVirtualImageInfo);
            this.tbDevice.Controls.Add(this.txtVirtualImagePath);
            this.tbDevice.Controls.Add(this.labelDioType);
            this.tbDevice.Controls.Add(this.cmbDioType);
            this.tbDevice.Controls.Add(this.buttonVirtual);
            this.tbDevice.Controls.Add(this.cmbGrabberType);
            this.tbDevice.Controls.Add(this.nudCamera);
            this.tbDevice.Controls.Add(this.lblCamera);
            this.tbDevice.Controls.Add(this.buttonCameraConfiguration);
            this.tbDevice.Location = new System.Drawing.Point(8, 59);
            this.tbDevice.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbDevice.Name = "tbDevice";
            this.tbDevice.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbDevice.Size = new System.Drawing.Size(515, 440);
            this.tbDevice.TabIndex = 1;
            this.tbDevice.Text = "Device";
            this.tbDevice.UseVisualStyleBackColor = true;
            // 
            // lblVirtualImageInfo
            // 
            this.lblVirtualImageInfo.AutoSize = true;
            this.lblVirtualImageInfo.Location = new System.Drawing.Point(8, 125);
            this.lblVirtualImageInfo.Name = "lblVirtualImageInfo";
            this.lblVirtualImageInfo.Size = new System.Drawing.Size(191, 45);
            this.lblVirtualImageInfo.TabIndex = 170;
            this.lblVirtualImageInfo.Text = "Virtual path";
            // 
            // txtVirtualImagePath
            // 
            this.txtVirtualImagePath.Location = new System.Drawing.Point(271, 125);
            this.txtVirtualImagePath.Name = "txtVirtualImagePath";
            this.txtVirtualImagePath.Size = new System.Drawing.Size(244, 50);
            this.txtVirtualImagePath.TabIndex = 169;
            // 
            // labelDioType
            // 
            this.labelDioType.AutoSize = true;
            this.labelDioType.Location = new System.Drawing.Point(8, 188);
            this.labelDioType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDioType.Name = "labelDioType";
            this.labelDioType.Size = new System.Drawing.Size(157, 45);
            this.labelDioType.TabIndex = 167;
            this.labelDioType.Text = "DIO Type";
            // 
            // cmbDioType
            // 
            this.cmbDioType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDioType.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDioType.FormattingEnabled = true;
            this.cmbDioType.Items.AddRange(new object[] {
            "None",
            "Adlink7230",
            "Adlink7432",
            "AlphaMotion302",
            "AlphaMotion304",
            "AlphaMotion314",
            "SusiGpio",
            "FastechEziMotionPlusR",
            "Modubus",
            "ComizoaSd424f",
            "TmcAexxx",
            "Adlink7433",
            "Adlink7434",
            "TmcAfxxx",
            "EziMotionPlusE",
            "AlphaMotionBBx"});
            this.cmbDioType.Location = new System.Drawing.Point(217, 193);
            this.cmbDioType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbDioType.Name = "cmbDioType";
            this.cmbDioType.Size = new System.Drawing.Size(298, 45);
            this.cmbDioType.TabIndex = 168;
            // 
            // buttonVirtual
            // 
            this.buttonVirtual.Location = new System.Drawing.Point(388, 64);
            this.buttonVirtual.Name = "buttonVirtual";
            this.buttonVirtual.Size = new System.Drawing.Size(127, 55);
            this.buttonVirtual.TabIndex = 163;
            this.buttonVirtual.Text = "...";
            this.buttonVirtual.UseVisualStyleBackColor = true;
            this.buttonVirtual.Click += new System.EventHandler(this.buttonVirtual_Click);
            // 
            // cmbGrabberType
            // 
            this.cmbGrabberType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGrabberType.FormattingEnabled = true;
            this.cmbGrabberType.Items.AddRange(new object[] {
            "Virtual",
            "Pylon",
            "MultiCam",
            "uEye",
            "MIL",
            "Capture",
            "WebcamAV",
            "WebcamCapture",
            "Cresvis",
            "PylonV2"});
            this.cmbGrabberType.Location = new System.Drawing.Point(144, 8);
            this.cmbGrabberType.Name = "cmbGrabberType";
            this.cmbGrabberType.Size = new System.Drawing.Size(171, 53);
            this.cmbGrabberType.TabIndex = 165;
            // 
            // nudCamera
            // 
            this.nudCamera.Location = new System.Drawing.Point(321, 8);
            this.nudCamera.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.nudCamera.Name = "nudCamera";
            this.nudCamera.Size = new System.Drawing.Size(57, 50);
            this.nudCamera.TabIndex = 162;
            // 
            // lblCamera
            // 
            this.lblCamera.AutoSize = true;
            this.lblCamera.Location = new System.Drawing.Point(8, 11);
            this.lblCamera.Name = "lblCamera";
            this.lblCamera.Size = new System.Drawing.Size(130, 45);
            this.lblCamera.TabIndex = 161;
            this.lblCamera.Text = "Camera";
            // 
            // buttonCameraConfiguration
            // 
            this.buttonCameraConfiguration.Location = new System.Drawing.Point(388, 5);
            this.buttonCameraConfiguration.Name = "buttonCameraConfiguration";
            this.buttonCameraConfiguration.Size = new System.Drawing.Size(127, 53);
            this.buttonCameraConfiguration.TabIndex = 164;
            this.buttonCameraConfiguration.Text = "Config";
            this.buttonCameraConfiguration.UseVisualStyleBackColor = true;
            this.buttonCameraConfiguration.Click += new System.EventHandler(this.buttonCameraConfiguration_Click);
            // 
            // tbResult
            // 
            this.tbResult.Controls.Add(this.lblPostgresSetting);
            this.tbResult.Controls.Add(this.btnPostgresSetting);
            this.tbResult.Location = new System.Drawing.Point(8, 59);
            this.tbResult.Name = "tbResult";
            this.tbResult.Padding = new System.Windows.Forms.Padding(3);
            this.tbResult.Size = new System.Drawing.Size(515, 440);
            this.tbResult.TabIndex = 2;
            this.tbResult.Text = "Result";
            this.tbResult.UseVisualStyleBackColor = true;
            // 
            // lblPostgresSetting
            // 
            this.lblPostgresSetting.AutoSize = true;
            this.lblPostgresSetting.Location = new System.Drawing.Point(8, 18);
            this.lblPostgresSetting.Name = "lblPostgresSetting";
            this.lblPostgresSetting.Size = new System.Drawing.Size(253, 45);
            this.lblPostgresSetting.TabIndex = 172;
            this.lblPostgresSetting.Text = "Postgres setting";
            // 
            // btnPostgresSetting
            // 
            this.btnPostgresSetting.Location = new System.Drawing.Point(346, 14);
            this.btnPostgresSetting.Name = "btnPostgresSetting";
            this.btnPostgresSetting.Size = new System.Drawing.Size(137, 29);
            this.btnPostgresSetting.TabIndex = 171;
            this.btnPostgresSetting.Text = "Set";
            this.btnPostgresSetting.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.buttonCancel);
            this.pnlBottom.Controls.Add(this.buttonOk);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 507);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(531, 76);
            this.pnlBottom.TabIndex = 1;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonCancel.BackColor = System.Drawing.Color.White;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Location = new System.Drawing.Point(333, 8);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(142, 56);
            this.buttonCancel.TabIndex = 154;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonOk.BackColor = System.Drawing.Color.White;
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOk.Location = new System.Drawing.Point(105, 8);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(143, 56);
            this.buttonOk.TabIndex = 155;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = false;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(18F, 45F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(531, 583);
            this.Controls.Add(this.tpMain);
            this.Controls.Add(this.pnlBottom);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConfigForm";
            this.tpMain.ResumeLayout(false);
            this.tbGeneral.ResumeLayout(false);
            this.tbGeneral.PerformLayout();
            this.tbDevice.ResumeLayout(false);
            this.tbDevice.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCamera)).EndInit();
            this.tbResult.ResumeLayout(false);
            this.tbResult.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tpMain;
        private System.Windows.Forms.TabPage tbGeneral;
        private System.Windows.Forms.TabPage tbDevice;
        private System.Windows.Forms.CheckBox virtualMode;
        private System.Windows.Forms.TextBox programTitle;
        private System.Windows.Forms.Label labelProgramTitle;
        private System.Windows.Forms.ComboBox imagingLibrary;
        private System.Windows.Forms.Label labelImagingLibrary;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonVirtual;
        private System.Windows.Forms.ComboBox cmbGrabberType;
        private System.Windows.Forms.NumericUpDown nudCamera;
        private System.Windows.Forms.Label lblCamera;
        private System.Windows.Forms.Button buttonCameraConfiguration;
        private System.Windows.Forms.Label labelDioType;
        private System.Windows.Forms.ComboBox cmbDioType;
        private System.Windows.Forms.Label lblVirtualImageInfo;
        private System.Windows.Forms.TextBox txtVirtualImagePath;
        private System.Windows.Forms.TabPage tbResult;
        private System.Windows.Forms.Label lblPostgresSetting;
        private System.Windows.Forms.Button btnPostgresSetting;
    }
}