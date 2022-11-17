using DynMvp.Base;
using DynMvp.Devices.Dio;
using DynMvp.Devices.MotionController;
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
    public partial class PciDigitalIoInfoForm : Form
    {
        PortSettingPanel portSettingPanel;

        PciDigitalIoInfo pciDigitalIoInfo;
        public PciDigitalIoInfo PciDigitalIoInfo
        {
            get { return pciDigitalIoInfo; }
            set { pciDigitalIoInfo = value;  }
        }

        public PciDigitalIoInfoForm()
        {
            InitializeComponent();

            labelName.Text = StringManager.GetString(this.GetType().FullName,labelName.Text);
            labelBoardIndex.Text = StringManager.GetString(this.GetType().FullName,labelBoardIndex.Text);
            buttonOK.Text = StringManager.GetString(this.GetType().FullName,buttonOK.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);

            this.portSettingPanel = new PortSettingPanel();
            panel1.Controls.Add(this.portSettingPanel);
        }

        private void PciMotionInfoForm_Load(object sender, EventArgs e)
        {
            name.Text = pciDigitalIoInfo.Name;
            boardIndex.Value = pciDigitalIoInfo.Index;
            this.portSettingPanel.UpdateData(pciDigitalIoInfo);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            pciDigitalIoInfo.Name = name.Text;
            pciDigitalIoInfo.Index = (int)boardIndex.Value;
            this.portSettingPanel.ApplyData(pciDigitalIoInfo);
        }
    }
}
