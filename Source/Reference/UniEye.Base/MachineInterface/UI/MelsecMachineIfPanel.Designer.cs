namespace UniEye.Base.MachineInterface.UI
{
    partial class MelsecMachineIfPanel
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
            this.groupBoxPlc = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkAsciiType = new System.Windows.Forms.CheckBox();
            this.labelModuleDeviceNo = new System.Windows.Forms.Label();
            this.moduleDeviceNo = new System.Windows.Forms.TextBox();
            this.cpuInspectData = new System.Windows.Forms.TextBox();
            this.labelCpuInspectData = new System.Windows.Forms.Label();
            this.labelModuleIoNo = new System.Windows.Forms.Label();
            this.moduleIoNo = new System.Windows.Forms.TextBox();
            this.labelNetworkNo = new System.Windows.Forms.Label();
            this.labelPlcNo = new System.Windows.Forms.Label();
            this.networkNo = new System.Windows.Forms.TextBox();
            this.plcNo = new System.Windows.Forms.TextBox();
            this.groupBoxTcpIp = new System.Windows.Forms.GroupBox();
            this.labelIpAddress = new System.Windows.Forms.Label();
            this.labelPortNo = new System.Windows.Forms.Label();
            this.ipAddress = new System.Windows.Forms.TextBox();
            this.portNo = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBoxPlc.SuspendLayout();
            this.groupBoxTcpIp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxPlc
            // 
            this.groupBoxPlc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPlc.Controls.Add(this.label1);
            this.groupBoxPlc.Controls.Add(this.chkAsciiType);
            this.groupBoxPlc.Controls.Add(this.labelModuleDeviceNo);
            this.groupBoxPlc.Controls.Add(this.moduleDeviceNo);
            this.groupBoxPlc.Controls.Add(this.cpuInspectData);
            this.groupBoxPlc.Controls.Add(this.labelCpuInspectData);
            this.groupBoxPlc.Controls.Add(this.labelModuleIoNo);
            this.groupBoxPlc.Controls.Add(this.moduleIoNo);
            this.groupBoxPlc.Controls.Add(this.labelNetworkNo);
            this.groupBoxPlc.Controls.Add(this.labelPlcNo);
            this.groupBoxPlc.Controls.Add(this.networkNo);
            this.groupBoxPlc.Controls.Add(this.plcNo);
            this.groupBoxPlc.Location = new System.Drawing.Point(5, 96);
            this.groupBoxPlc.Margin = new System.Windows.Forms.Padding(5);
            this.groupBoxPlc.Name = "groupBoxPlc";
            this.groupBoxPlc.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxPlc.Size = new System.Drawing.Size(285, 187);
            this.groupBoxPlc.TabIndex = 10;
            this.groupBoxPlc.TabStop = false;
            this.groupBoxPlc.Text = "PLC";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 157);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "ASCII Type Protocol";
            // 
            // chkAsciiType
            // 
            this.chkAsciiType.AutoSize = true;
            this.chkAsciiType.Location = new System.Drawing.Point(183, 157);
            this.chkAsciiType.Name = "chkAsciiType";
            this.chkAsciiType.Size = new System.Drawing.Size(15, 14);
            this.chkAsciiType.TabIndex = 8;
            this.chkAsciiType.UseVisualStyleBackColor = true;
            // 
            // labelModuleDeviceNo
            // 
            this.labelModuleDeviceNo.AutoSize = true;
            this.labelModuleDeviceNo.Location = new System.Drawing.Point(13, 102);
            this.labelModuleDeviceNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelModuleDeviceNo.Name = "labelModuleDeviceNo";
            this.labelModuleDeviceNo.Size = new System.Drawing.Size(109, 12);
            this.labelModuleDeviceNo.TabIndex = 6;
            this.labelModuleDeviceNo.Text = "Module Device No";
            // 
            // moduleDeviceNo
            // 
            this.moduleDeviceNo.Location = new System.Drawing.Point(138, 99);
            this.moduleDeviceNo.Name = "moduleDeviceNo";
            this.moduleDeviceNo.Size = new System.Drawing.Size(115, 21);
            this.moduleDeviceNo.TabIndex = 7;
            // 
            // cpuInspectData
            // 
            this.cpuInspectData.Location = new System.Drawing.Point(138, 126);
            this.cpuInspectData.Name = "cpuInspectData";
            this.cpuInspectData.Size = new System.Drawing.Size(115, 21);
            this.cpuInspectData.TabIndex = 5;
            // 
            // labelCpuInspectData
            // 
            this.labelCpuInspectData.AutoSize = true;
            this.labelCpuInspectData.Location = new System.Drawing.Point(13, 129);
            this.labelCpuInspectData.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCpuInspectData.Name = "labelCpuInspectData";
            this.labelCpuInspectData.Size = new System.Drawing.Size(115, 12);
            this.labelCpuInspectData.TabIndex = 4;
            this.labelCpuInspectData.Text = "CPU Inspector Data";
            // 
            // labelModuleIoNo
            // 
            this.labelModuleIoNo.AutoSize = true;
            this.labelModuleIoNo.Location = new System.Drawing.Point(13, 75);
            this.labelModuleIoNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelModuleIoNo.Name = "labelModuleIoNo";
            this.labelModuleIoNo.Size = new System.Drawing.Size(89, 12);
            this.labelModuleIoNo.TabIndex = 2;
            this.labelModuleIoNo.Text = "Module I/O No";
            // 
            // moduleIoNo
            // 
            this.moduleIoNo.Location = new System.Drawing.Point(138, 72);
            this.moduleIoNo.Name = "moduleIoNo";
            this.moduleIoNo.Size = new System.Drawing.Size(115, 21);
            this.moduleIoNo.TabIndex = 3;
            // 
            // labelNetworkNo
            // 
            this.labelNetworkNo.AutoSize = true;
            this.labelNetworkNo.Location = new System.Drawing.Point(13, 21);
            this.labelNetworkNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNetworkNo.Name = "labelNetworkNo";
            this.labelNetworkNo.Size = new System.Drawing.Size(71, 12);
            this.labelNetworkNo.TabIndex = 0;
            this.labelNetworkNo.Text = "Network No";
            // 
            // labelPlcNo
            // 
            this.labelPlcNo.AutoSize = true;
            this.labelPlcNo.Location = new System.Drawing.Point(13, 48);
            this.labelPlcNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPlcNo.Name = "labelPlcNo";
            this.labelPlcNo.Size = new System.Drawing.Size(49, 12);
            this.labelPlcNo.TabIndex = 0;
            this.labelPlcNo.Text = "PLC No";
            // 
            // networkNo
            // 
            this.networkNo.Location = new System.Drawing.Point(138, 18);
            this.networkNo.Name = "networkNo";
            this.networkNo.Size = new System.Drawing.Size(115, 21);
            this.networkNo.TabIndex = 1;
            // 
            // plcNo
            // 
            this.plcNo.Location = new System.Drawing.Point(138, 45);
            this.plcNo.Name = "plcNo";
            this.plcNo.Size = new System.Drawing.Size(115, 21);
            this.plcNo.TabIndex = 1;
            // 
            // groupBoxTcpIp
            // 
            this.groupBoxTcpIp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTcpIp.Controls.Add(this.labelIpAddress);
            this.groupBoxTcpIp.Controls.Add(this.labelPortNo);
            this.groupBoxTcpIp.Controls.Add(this.ipAddress);
            this.groupBoxTcpIp.Controls.Add(this.portNo);
            this.groupBoxTcpIp.Location = new System.Drawing.Point(5, 5);
            this.groupBoxTcpIp.Margin = new System.Windows.Forms.Padding(5);
            this.groupBoxTcpIp.Name = "groupBoxTcpIp";
            this.groupBoxTcpIp.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxTcpIp.Size = new System.Drawing.Size(285, 81);
            this.groupBoxTcpIp.TabIndex = 9;
            this.groupBoxTcpIp.TabStop = false;
            this.groupBoxTcpIp.Text = "TCP/IP";
            // 
            // labelIpAddress
            // 
            this.labelIpAddress.AutoSize = true;
            this.labelIpAddress.Location = new System.Drawing.Point(13, 23);
            this.labelIpAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIpAddress.Name = "labelIpAddress";
            this.labelIpAddress.Size = new System.Drawing.Size(67, 12);
            this.labelIpAddress.TabIndex = 0;
            this.labelIpAddress.Text = "IP Address";
            // 
            // labelPortNo
            // 
            this.labelPortNo.AutoSize = true;
            this.labelPortNo.Location = new System.Drawing.Point(13, 52);
            this.labelPortNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPortNo.Name = "labelPortNo";
            this.labelPortNo.Size = new System.Drawing.Size(47, 12);
            this.labelPortNo.TabIndex = 0;
            this.labelPortNo.Text = "Port No";
            // 
            // ipAddress
            // 
            this.ipAddress.Location = new System.Drawing.Point(138, 18);
            this.ipAddress.Name = "ipAddress";
            this.ipAddress.Size = new System.Drawing.Size(115, 21);
            this.ipAddress.TabIndex = 1;
            // 
            // portNo
            // 
            this.portNo.Location = new System.Drawing.Point(138, 47);
            this.portNo.Name = "portNo";
            this.portNo.Size = new System.Drawing.Size(115, 21);
            this.portNo.TabIndex = 1;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // MelsecMachineIfPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.groupBoxPlc);
            this.Controls.Add(this.groupBoxTcpIp);
            this.Name = "MelsecMachineIfPanel";
            this.Size = new System.Drawing.Size(295, 288);
            this.Load += new System.EventHandler(this.MelsecConnectionInfoPanel_Load);
            this.groupBoxPlc.ResumeLayout(false);
            this.groupBoxPlc.PerformLayout();
            this.groupBoxTcpIp.ResumeLayout(false);
            this.groupBoxTcpIp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxPlc;
        private System.Windows.Forms.Label labelModuleDeviceNo;
        private System.Windows.Forms.TextBox moduleDeviceNo;
        private System.Windows.Forms.TextBox cpuInspectData;
        private System.Windows.Forms.Label labelCpuInspectData;
        private System.Windows.Forms.Label labelModuleIoNo;
        private System.Windows.Forms.TextBox moduleIoNo;
        private System.Windows.Forms.Label labelNetworkNo;
        private System.Windows.Forms.Label labelPlcNo;
        private System.Windows.Forms.TextBox networkNo;
        private System.Windows.Forms.TextBox plcNo;
        private System.Windows.Forms.GroupBox groupBoxTcpIp;
        private System.Windows.Forms.Label labelIpAddress;
        private System.Windows.Forms.Label labelPortNo;
        private System.Windows.Forms.TextBox ipAddress;
        private System.Windows.Forms.TextBox portNo;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkAsciiType;
    }
}
