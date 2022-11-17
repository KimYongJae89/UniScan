using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DynMvp.Devices.Comm;
using DynMvp.Base;

namespace DynMvp.Devices.UI
{
    public partial class TcpIpSettingForm : Form
    {
        TcpIpInfo tcpIpInfo;
        public TcpIpInfo TcpIpInfo
        {
            get { return tcpIpInfo; }
            set { tcpIpInfo = value; }
        }

        public TcpIpSettingForm(TcpIpInfo tcpIpInfo)
        {
            InitializeComponent();
            
            labelIpAddress.Text = StringManager.GetString(this.GetType().FullName,labelIpAddress.Text);
            labelInPortNo.Text = StringManager.GetString(this.GetType().FullName,labelInPortNo.Text);
            labelOutPortNo.Text = StringManager.GetString(this.GetType().FullName,labelOutPortNo.Text);
            btnOK.Text = StringManager.GetString(this.GetType().FullName,btnOK.Text);
            btnCancel.Text = StringManager.GetString(this.GetType().FullName,btnCancel.Text);

            this.tcpIpInfo = tcpIpInfo;
        }

        private void TcpIpSettingForm_Load(object sender, EventArgs e)
        {
            ipAddress.Text = tcpIpInfo.IpAddress;
            inPortNo.Value = tcpIpInfo.PortNo;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            tcpIpInfo.IpAddress = ipAddress.Text.Replace(" ", "");
            tcpIpInfo.PortNo = (int)inPortNo.Value;
        }
    }
}
