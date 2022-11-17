using System;
using System.Windows.Forms;
using System.Diagnostics;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Exchange;
using DynMvp.Devices.Light;
using DynMvp.Devices;
using DynMvp.UI.Touch;
using DynMvp.Base;
using UniScanG.Gravure.Data;
using UniScanG.MachineIF;
using UniScanG.Gravure.Device;

namespace UniScanG.Module.Controller.UI.Teach
{
    public partial class AutoTeachButton : UserControl,IMultiLanguageSupport
    {
        public AutoTeachButton()
        {
            InitializeComponent();

            this.TabIndex = 0;
            StringManager.AddListener(this);
        }

        public void UpdateLanguage()
        {
            StringManager.UpdateString(this);
        }

        private void button_Click(object sender, EventArgs e)
        {
            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            bool disconnectedExist = Array.Exists(server.Inspectors, f => !f.IsConnected);
            if(disconnectedExist)
            {
                MessageForm.Show(this, StringManager.GetString("Inspector is not connected."));
                return;
            }

            MachineIfData machineIfData = (SystemManager.Instance().DeviceController as DeviceControllerG)?.MachineIfMonitor?.MachineIfData as MachineIfData;
            if (machineIfData == null)
                return;

            if (machineIfData.GET_PRESENT_SPEED_REAL <= 0)
            {
                MessageForm.Show(this, StringManager.GetString("Machine is not Running."));
                return;
            }

            Controller.Inspect.InspectRunnerMonitorG inspectRunner = SystemManager.Instance().InspectRunner as Controller.Inspect.InspectRunnerMonitorG;
            inspectRunner?.AutoTeach(machineIfData.GET_PRESENT_SPEED_REAL);
        }
    }
}
