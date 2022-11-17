using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
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
    public partial class NewMotionForm : Form
    {
        MotionType motionType;
        public MotionType MotionType
        {
            get { return motionType;  }
            set { motionType = value; }
        }

        string motionName;
        public string MotionName
        {
            get { return motionName; }
            set { motionName = value; }
        }

        public NewMotionForm()
        {
            InitializeComponent();

            labelMotionName.Text = StringManager.GetString(this.GetType().FullName,labelMotionName.Text);
            labelMotionType.Text = StringManager.GetString(this.GetType().FullName,labelMotionType.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

            cmbMotionType.DataSource = Enum.GetNames(typeof(MotionType));
        }

        private void GrabberInfoForm_Load(object sender, EventArgs e)
        {
            cmbMotionType.SelectedIndex = (int)motionType;
            txtMotionName.Text = motionName;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            motionType = (MotionType)cmbMotionType.SelectedIndex;
            motionName = txtMotionName.Text;
        }

        private void cmbMotionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtMotionName.Text))
            {
                txtMotionName.Text = ((MotionType)cmbMotionType.SelectedIndex).ToString();
            }
        }
    }
}
