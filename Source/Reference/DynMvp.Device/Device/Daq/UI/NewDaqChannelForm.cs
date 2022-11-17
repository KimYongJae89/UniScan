using DynMvp.Base;
using DynMvp.Devices.Daq;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Device.Daq.UI
{
    public partial class NewDaqChannelForm : Form
    {
        string daqChannelName;
        public string DaqChannelName
        {
            get { return daqChannelName; }
            set { daqChannelName = value; }
        }

        DaqChannelType daqChannelType;
        public DaqChannelType DaqChannelType
        {
            get { return daqChannelType;  }
            set { daqChannelType = value; }
        }

        public NewDaqChannelForm()
        {
            InitializeComponent();

            labelMotionName.Text = StringManager.GetString(this.GetType().FullName, labelMotionName.Text);
            labelDaqChannelType.Text = StringManager.GetString(this.GetType().FullName, labelDaqChannelType.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName, buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName, buttonCancel.Text);

            cmbDaqChannelType.DataSource = Enum.GetNames(typeof(DaqChannelType));
        }

        private void GrabberInfoForm_Load(object sender, EventArgs e)
        {
            cmbDaqChannelType.SelectedIndex = (int)daqChannelType;
            txtName.Text = daqChannelName;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            daqChannelName = txtName.Text;
            daqChannelType = (DaqChannelType)cmbDaqChannelType.SelectedIndex;
        }
    }
}
