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
    public partial class FinsSettingForm : Form
    {
        FinsInfo finsInfo;
        public FinsInfo FinsInfo
        {
            get { return finsInfo; }
            set { finsInfo = value; }
        }

        public FinsSettingForm()
        {
            InitializeComponent();

            labelIpAddress.Text = StringManager.GetString(this.GetType().FullName,labelIpAddress.Text);
            labelPortNo.Text = StringManager.GetString(this.GetType().FullName,labelPortNo.Text);
            labelNetworkNo.Text = StringManager.GetString(this.GetType().FullName,labelNetworkNo.Text);

            labelPlcStateAddress.Text = StringManager.GetString(this.GetType().FullName,labelPlcStateAddress.Text);
            labelPcStateAddress.Text = StringManager.GetString(this.GetType().FullName,labelPcStateAddress.Text);
            labelResultAddress.Text = StringManager.GetString(this.GetType().FullName,labelResultAddress.Text);

            btnOK.Text = StringManager.GetString(this.GetType().FullName,btnOK.Text);
            btnCancel.Text = StringManager.GetString(this.GetType().FullName,btnCancel.Text);

        }

        private void FinsSettingForm_Load(object sender, EventArgs e)
        {
            ipAddress.Text = finsInfo.IpAddress;
            portNo.Value = finsInfo.PortNo;
            networkNo.Value = finsInfo.NetworkNo;
            pcStateAddress.Value = finsInfo.PcStateAddress;
            plcStateAddress.Value = finsInfo.PlcStateAddress;
            resultAddress.Value = finsInfo.ResultAddress;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            finsInfo.IpAddress = ipAddress.Text.Replace(" ", "");
            finsInfo.PortNo = (int)portNo.Value;
            finsInfo.NetworkNo = (int)networkNo.Value;
            finsInfo.PcStateAddress = (int)pcStateAddress.Value;
            finsInfo.PlcStateAddress = (int)plcStateAddress.Value;
            finsInfo.ResultAddress = (int)resultAddress.Value;
        }
    }
}
