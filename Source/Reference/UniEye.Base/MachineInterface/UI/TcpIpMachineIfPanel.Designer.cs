namespace UniEye.Base.MachineInterface.UI
{
    partial class TcpIpMachineIfPanel
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
            this.labelPortNo = new System.Windows.Forms.Label();
            this.ipAddress = new System.Windows.Forms.TextBox();
            this.portNo = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBoxTcpIp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxTcpIp
            // 
            this.groupBoxTcpIp.Controls.Add(this.labelIpAddress);
            this.groupBoxTcpIp.Controls.Add(this.labelPortNo);
            this.groupBoxTcpIp.Controls.Add(this.ipAddress);
            this.groupBoxTcpIp.Controls.Add(this.portNo);
            this.groupBoxTcpIp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxTcpIp.Location = new System.Drawing.Point(0, 0);
            this.groupBoxTcpIp.Margin = new System.Windows.Forms.Padding(5);
            this.groupBoxTcpIp.Name = "groupBoxTcpIp";
            this.groupBoxTcpIp.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxTcpIp.Size = new System.Drawing.Size(281, 85);
            this.groupBoxTcpIp.TabIndex = 10;
            this.groupBoxTcpIp.TabStop = false;
            this.groupBoxTcpIp.Text = "TCP/IP";
            // 
            // labelIpAddress
            // 
            this.labelIpAddress.AutoSize = true;
            this.labelIpAddress.Location = new System.Drawing.Point(13, 21);
            this.labelIpAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelIpAddress.Name = "labelIpAddress";
            this.labelIpAddress.Size = new System.Drawing.Size(67, 12);
            this.labelIpAddress.TabIndex = 0;
            this.labelIpAddress.Text = "IP Address";
            // 
            // labelPortNo
            // 
            this.labelPortNo.AutoSize = true;
            this.labelPortNo.Location = new System.Drawing.Point(13, 50);
            this.labelPortNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelPortNo.Name = "labelPortNo";
            this.labelPortNo.Size = new System.Drawing.Size(47, 12);
            this.labelPortNo.TabIndex = 0;
            this.labelPortNo.Text = "Port No";
            // 
            // ipAddress
            // 
            this.ipAddress.Location = new System.Drawing.Point(135, 18);
            this.ipAddress.Name = "ipAddress";
            this.ipAddress.Size = new System.Drawing.Size(115, 21);
            this.ipAddress.TabIndex = 1;
            // 
            // portNo
            // 
            this.portNo.Location = new System.Drawing.Point(135, 47);
            this.portNo.Name = "portNo";
            this.portNo.Size = new System.Drawing.Size(115, 21);
            this.portNo.TabIndex = 1;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // TcpIpMachineIfPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxTcpIp);
            this.Name = "TcpIpMachineIfPanel";
            this.Size = new System.Drawing.Size(281, 85);
            this.Load += new System.EventHandler(this.TcpIpMachineIfPanel_Load);
            this.groupBoxTcpIp.ResumeLayout(false);
            this.groupBoxTcpIp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTcpIp;
        private System.Windows.Forms.Label labelIpAddress;
        private System.Windows.Forms.Label labelPortNo;
        private System.Windows.Forms.TextBox ipAddress;
        private System.Windows.Forms.TextBox portNo;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}
