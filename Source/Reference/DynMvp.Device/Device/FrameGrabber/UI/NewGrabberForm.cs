using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DynMvp.Device.FrameGrabber.UI
{
    public partial class NewGrabberForm : Form
    {
        GrabberType grabberType;
        public GrabberType GrabberType
        {
            get { return grabberType;  }
            set { grabberType = value; }
        }

        string grabberName;
        public string GrabberName
        {
            get { return grabberName; }
            set { grabberName = value; }
        }
        int numCamera;
        public int NumCamera
        {
            get { return numCamera; }
            set { numCamera = value; }
        }

        public NewGrabberForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(this.GetType().FullName,labelName.Text);
            labelGrabberType.Text = StringManager.GetString(this.GetType().FullName,labelGrabberType.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

            cmbGrabberType.DataSource = Enum.GetNames(typeof(GrabberType));
        }

        private void GrabberInfoForm_Load(object sender, EventArgs e)
        {
            cmbGrabberType.SelectedIndex = (int)grabberType;
            txtName.Text = grabberName;
            txtNumCamera.Value = numCamera;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            grabberType = (GrabberType)cmbGrabberType.SelectedIndex;
            grabberName = txtName.Text;
            numCamera = (int)txtNumCamera.Value;
        }

        private void cmbGrabberType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtName.Text))
            {
                txtName.Text = ((GrabberType)cmbGrabberType.SelectedIndex).ToString();
            }
        }

        private void txtNumCamera_ValueChanged(object sender, EventArgs e)
        {
            numCamera = (int)txtNumCamera.Value;
        }
    }
}
