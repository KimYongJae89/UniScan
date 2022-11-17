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
    public partial class PciMotionInfoForm : Form
    {
        PciMotionInfo pciMotionInfo;
        public PciMotionInfo PciMotionInfo
        {
            get { return pciMotionInfo; }
            set { pciMotionInfo = value;  }
        }

        public PciMotionInfoForm()
        {
            InitializeComponent();


            labelName.Text = StringManager.GetString(this.GetType().FullName,labelName.Text);
            labelNumAxis.Text = StringManager.GetString(this.GetType().FullName,labelNumAxis.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            labelBoardIndex.Text = StringManager.GetString(this.GetType().FullName,labelBoardIndex.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

        }

        private void PciMotionInfoForm_Load(object sender, EventArgs e)
        {
            name.Text = pciMotionInfo.Name;
            numAxis.Value = pciMotionInfo.NumAxis;
            boardIndex.Value = pciMotionInfo.Index;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            pciMotionInfo.Name = name.Text;
            pciMotionInfo.NumAxis = (int)numAxis.Value;
            pciMotionInfo.Index = (int)boardIndex.Value;
        }
    }
}
