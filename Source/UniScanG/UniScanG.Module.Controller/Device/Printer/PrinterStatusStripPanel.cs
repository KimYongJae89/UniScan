using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniScanG.UI.Etc;
using UniScanG.Common.Util;
using UniScanG.Module.Controller.MachineIF;
using UniScanG.Gravure.Device;
using DynMvp.Base;
using UniEye.Base.MachineInterface;
//using UniScanG.MachineIF;

namespace UniScanG.Module.Controller.Device.Printer
{
    public partial class PrinterStatusStripPanel : UserControl, IStatusStripPanel,IMultiLanguageSupport
    {
        MachineIF.MachineIfData machineIfData = null;
        PrinterControlFrom form = null;

        public PrinterStatusStripPanel()
        {
            InitializeComponent();

            if (SystemManager.Instance().DeviceBox.MachineIf.IsVirtual)
            {
                this.labelConnection.ToolTipText = "Virtual";
                ((UniEye.Base.MachineInterface.IVirtualMachineIf)SystemManager.Instance().DeviceBox.MachineIf).SetStateConnect(true);
            }

            this.machineIfData = (SystemManager.Instance().DeviceController as DeviceControllerG)?.MachineIfMonitor?.MachineIfData as MachineIF.MachineIfData;
            StateUpdate();

            StringManager.AddListener(this);
        }

        public void StateUpdate()
        {
            if (this.machineIfData == null)
                return;

            this.labelConnection.BackColor = SystemManager.Instance().DeviceBox.MachineIf.IsConnected ? Colors.Connected : Colors.Disconnected;
            this.labelState.Text = StringManager.GetString(this.GetType().FullName, machineIfData.GET_START_GRAVURE_INSPECTOR ? "RUN" : "RDY");
            this.labelState.BackColor = machineIfData.GET_START_GRAVURE_INSPECTOR ? Colors.Run : Colors.Idle;
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void labelConnection_Click(object sender, EventArgs e)
        {
            if (this.form == null || this.form.IsDisposed)
                this.form = new PrinterControlFrom(SystemManager.Instance().DeviceBox.MachineIf.IsVirtual, machineIfData);

            if (this.form.Visible)
                form.Focus();
            else
                form.Show();
        }
    }
}
