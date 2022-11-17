namespace UniEye.Base.MachineInterface.UI
{
    partial class AllenBreadleyMachineIfPanel
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
            this.groupBoxTcpIp = new System.Windows.Forms.GroupBox();
            this.labelIpAddress = new System.Windows.Forms.Label();
            this.labelCpuType = new System.Windows.Forms.Label();
            this.ipAddress = new System.Windows.Forms.TextBox();
            this.cpuType = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.plcPath = new System.Windows.Forms.TextBox();
            this.labelPlcPath = new System.Windows.Forms.Label();
            this.groupBoxTcpIp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxTcpIp
            // 
            this.groupBoxTcpIp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTcpIp.Controls.Add(this.labelIpAddress);
            this.groupBoxTcpIp.Controls.Add(this.labelPlcPath);
            this.groupBoxTcpIp.Controls.Add(this.labelCpuType);
            this.groupBoxTcpIp.Controls.Add(this.ipAddress);
            this.groupBoxTcpIp.Controls.Add(this.plcPath);
            this.groupBoxTcpIp.Controls.Add(this.cpuType);
            this.groupBoxTcpIp.Location = new System.Drawing.Point(5, 5);
            this.groupBoxTcpIp.Margin = new System.Windows.Forms.Padding(5);
            this.groupBoxTcpIp.Name = "groupBoxTcpIp";
            this.groupBoxTcpIp.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxTcpIp.Size = new System.Drawing.Size(285, 114);
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
            // labelCpuType
            // 
            this.labelCpuType.AutoSize = true;
            this.labelCpuType.Location = new System.Drawing.Point(13, 52);
            this.labelCpuType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelCpuType.Name = "labelCpuType";
            this.labelCpuType.Size = new System.Drawing.Size(30, 12);
            this.labelCpuType.TabIndex = 0;
            this.labelCpuType.Text = "CPU";
            // 
            // ipAddress
            // 
            this.ipAddress.Location = new System.Drawing.Point(138, 18);
            this.ipAddress.Name = "ipAddress";
            this.ipAddress.Size = new System.Drawing.Size(115, 21);
            this.ipAddress.TabIndex = 1;
            // 
            // cpuType
            // 
            this.cpuType.Location = new System.Drawing.Point(138, 47);
            this.cpuType.Name = "cpuType";
            this.cpuType.Size = new System.Drawing.Size(115, 21);
            this.cpuType.TabIndex = 1;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // plcPath
            // 
            this.plcPath.Location = new System.Drawing.Point(138, 74);
            this.plcPath.Name = "plcPath";
            this.plcPath.Size = new System.Drawing.Size(115, 21);
            this.plcPath.TabIndex = 1;
            // 
            // labelPlcPath
            // 
            this.labelPlcPath.AutoSize = true;
            this.labelPlcPath.Location = new System.Drawing.Point(13, 79);
            this.labelPlcPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPlcPath.Name = "labelPlcPath";
            this.labelPlcPath.Size = new System.Drawing.Size(30, 12);
            this.labelPlcPath.TabIndex = 0;
            this.labelPlcPath.Text = "Path";
            // 
            // AllenBreadleyMachineIfPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.groupBoxTcpIp);
            this.Name = "AllenBreadleyMachineIfPanel";
            this.Size = new System.Drawing.Size(295, 288);
            this.Load += new System.EventHandler(this.MelsecConnectionInfoPanel_Load);
            this.groupBoxTcpIp.ResumeLayout(false);
            this.groupBoxTcpIp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxTcpIp;
        private System.Windows.Forms.Label labelIpAddress;
        private System.Windows.Forms.Label labelCpuType;
        private System.Windows.Forms.TextBox ipAddress;
        private System.Windows.Forms.TextBox cpuType;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label labelPlcPath;
        private System.Windows.Forms.TextBox plcPath;
    }
}
