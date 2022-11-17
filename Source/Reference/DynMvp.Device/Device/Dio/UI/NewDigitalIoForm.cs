using DynMvp.Base;
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

namespace DynMvp.Device.Dio.UI
{
    public partial class NewDigitalIoForm : Form
    {
        DigitalIoType digitalIoType;
        public DigitalIoType DigitalIoType
        {
            set { digitalIoType = value; }
        }

        string digitalIoName;
        public string DigitalIoName
        {
            set { digitalIoName = value; }
        }

        // Return Value
        DigitalIoInfo digitalIoInfo;
        public DigitalIoInfo DigitalIoInfo
        {
            get { return digitalIoInfo;  }
            set { digitalIoInfo = value; }
        }

        public NewDigitalIoForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(this.GetType().FullName,labelName.Text);
            labelDigitalIoType.Text = StringManager.GetString(this.GetType().FullName,labelDigitalIoType.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

            cmbDigitalIoType.DataSource = Enum.GetNames(typeof(DigitalIoType));
        }

        private void GrabberInfoForm_Load(object sender, EventArgs e)
        {
            cmbDigitalIoType.SelectedIndex = (int)digitalIoType;
            txtName.Text = digitalIoName;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            digitalIoInfo = DigitalIoInfoFactory.Create((DigitalIoType)cmbDigitalIoType.SelectedIndex);
            digitalIoInfo.Name = txtName.Text;
            //digitalIoInfo.NumInPortGroup = (int)numInPortGroup.Value;
            //digitalIoInfo.InPortStartGroupIndex = (int)inPortStartGroupIndex.Value;
            //digitalIoInfo.NumInPort = (int)numInPort.Value;
            //digitalIoInfo.NumOutPortGroup = (int)numOutPortGroup.Value;
            //digitalIoInfo.OutPortStartGroupIndex = (int)outPortStartGroupIndex.Value;
            //digitalIoInfo.NumOutPort = (int)numOutPort.Value;
        }

        private void cmbDigitalIoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(txtName.Text))
            {
                digitalIoName = ((DigitalIoType)cmbDigitalIoType.SelectedIndex).ToString();
                txtName.Text = digitalIoName;
            }
        }
    }
}
