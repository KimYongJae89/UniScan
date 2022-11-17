using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanG.Gravure.Data;
using UniScanG.MachineIF;

namespace UniScanG.Module.Monitor.MachineIF
{
    enum UniScanGMelsecProtocol { SET_VISION_STATE };

    internal class MachineIfMonitor : UniScanG.MachineIF.MachineIfMonitor
    {
        public MachineIfMonitor(MachineIfDataAdapterMM adapter) : base(adapter) { }

        public override void PropagateData()
        {
            
        }

        public override void ApplyData()
        {
            MachineIfData machineIfData = (MachineIfData)this.adapter.MachineIfData;

            machineIfData.SET_VISION_GRAVURE_MONITORING_READY = true;
            machineIfData.SET_VISION_GRAVURE_MONITORING_RUN = !(UniEye.Base.Data.SystemState.Instance().OpState == UniEye.Base.Data.OpState.Idle);
        }
    }
}
