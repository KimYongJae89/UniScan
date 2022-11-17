using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.Device;
using UniScanG.Gravure.Device;
using UniScanG.Module.Monitor.MachineIF;
using UniScanG.Module.Monitor.Operation;

namespace UniScanG.Module.Monitor.Device
{
    public class DeviceController: DeviceControllerG
    {
        public InspectStarter InspectStarter =>this.inspectStarter;
        InspectStarter inspectStarter = null;

        public override void Initialize(UniEye.Base.Device.DeviceBox deviceBox)
        {
            base.Initialize(deviceBox);

            //if (deviceBox.MachineIf != null)
            {
                MachineIfDataAdapterMM adapter = new MachineIfDataAdapterMM(new MachineIfData());
                this.machineIfMonitor = new MachineIfMonitor(adapter);
                this.machineIfMonitor.Start();
            }

            this.inspectStarter = new InspectStarter();
            //this.inspectStarter.Start();
        }

        public override bool OnEnterWaitInspection(params object[] args)
        {
            this.inspectStarter.LotChange();

            return true;
        }

        protected override double CalculateOutputDelayMs(float lineSpdMpm)
        {
            throw new NotImplementedException();
        }

        protected override int GetOutputHoldTimeMs(float lineSpdMpm)
        {
            throw new NotImplementedException();
        }

        protected override bool SetIo(bool active)
        {
            throw new NotImplementedException();
        }

        public override void Startup(string imName) { }
        public override void Shutdown(string imName, bool restart) { }
        public override void Launch(string imName, string[] args) { }
        public override void InitializeInspectModule(IWin32Window parent, string imName) { }
    }
}
