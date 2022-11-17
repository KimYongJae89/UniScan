using DynMvp.Base;
using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Device.MotionController.UI
{
    public partial class NetworkMotionInfoForm : Form
    {
        NetworkMotionInfo networkMotionInfo;
        public NetworkMotionInfo NetworkMotionInfo
        {
            get { return networkMotionInfo; }
            set { networkMotionInfo = value; }
        }

        public NetworkMotionInfoForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(this.GetType().FullName,labelName.Text);
            labelNumAxis.Text = StringManager.GetString(this.GetType().FullName,labelNumAxis.Text);
            labelPortNo.Text = StringManager.GetString(this.GetType().FullName,labelPortNo.Text);
            labelIpAddress.Text = StringManager.GetString(this.GetType().FullName,labelIpAddress.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

        }

        private void NetworkMotionInfoForm_Load(object sender, EventArgs e)
        {
            name.Text = networkMotionInfo.Name;
            numAxis.Value = networkMotionInfo.NumAxis;
            ipAddress.Text = networkMotionInfo.IpAddress;
            portNo.Value = networkMotionInfo.PortNo;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            networkMotionInfo.Name = name.Text;
            networkMotionInfo.NumAxis = (int)numAxis.Value;
            networkMotionInfo.IpAddress = ipAddress.Text;
            networkMotionInfo.PortNo = (byte)portNo.Value;
        }
    }
}
