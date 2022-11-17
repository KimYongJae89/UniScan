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
    public partial class VirtualMotionInfoForm : Form
    {
        VirtualMotionInfo virtualMotionInfo;
        public VirtualMotionInfo VirtualMotionInfo
        {
            get { return virtualMotionInfo; }
            set { virtualMotionInfo = value;  }
        }

        public VirtualMotionInfoForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(this.GetType().FullName,labelName.Text);
            labelNumAxis.Text = StringManager.GetString(this.GetType().FullName,labelNumAxis.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

        }

        private void VirtualMotionInfoForm_Load(object sender, EventArgs e)
        {
            name.Text = virtualMotionInfo.Name;
            numAxis.Value = virtualMotionInfo.NumAxis;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            virtualMotionInfo.Name = name.Text;
            virtualMotionInfo.NumAxis = (int)numAxis.Value;
        }
    }
}
