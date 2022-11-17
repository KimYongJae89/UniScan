using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniEye.Base.MachineInterface.UI
{
    public partial class MachineIfForm : Form
    {
        IMachineIfPanel machineIfPanel = null;
        IMachineIfPanel protocolPanel = null;

        public MachineIfSetting MachineIfSetting => this.machineIfSetting;
        MachineIfSetting machineIfSetting;

        public MachineIfForm(MachineIfSetting machineIfSetting)
        {
            InitializeComponent();

            buttonOk.Text = StringManager.GetString(this.GetType().FullName,buttonOk.Text);
            buttonCancel.Text = StringManager.GetString(this.GetType().FullName,buttonCancel.Text);
            buttonDefault.Text = StringManager.GetString(this.GetType().FullName, buttonDefault.Text);

            this.machineIfSetting = machineIfSetting.Clone();

            UpdateMachineIfPanel(this.machineIfSetting);

            ProtocolPanel protocolPanel = new ProtocolPanel();
            protocolPanel.Dock = DockStyle.Fill;
            protocolPanel.Initialize(this.machineIfSetting);
            panelProtocol.Controls.Add(protocolPanel);

            this.protocolPanel = protocolPanel;
        }
        
        private void UpdateMachineIfPanel(MachineIfSetting settings)
        {
            switch (settings.MachineIfType)
            {
                case MachineIfType.TcpClient:
                case MachineIfType.TcpServer:
                    machineIfPanel = new TcpIpMachineIfPanel();
                    break;
                case MachineIfType.Melsec:
                    machineIfPanel = new MelsecMachineIfPanel();
                    break;
                case MachineIfType.AllenBreadley:
                    machineIfPanel = new AllenBreadleyMachineIfPanel();
                    break;
            }

            machineIfPanel?.Initialize(settings);
            UserControl userControl = (UserControl)machineIfPanel;
            if (userControl != null)
            {
                userControl.Dock = DockStyle.Fill;
                this.panelMachineIF.Controls.Add(userControl);
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            bool good = protocolPanel.Verify();
            if (machineIfPanel != null)
                good &= machineIfPanel.Verify();

            if (good)
            {
                machineIfPanel?.Apply();
                protocolPanel.Apply();
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                DynMvp.UI.Touch.MessageForm.Show(this, "Some Input is Invalid");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonDefault_Click(object sender, EventArgs e)
        {
            protocolPanel.SetDefault();
            protocolPanel.Initialize();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }


    }
}
