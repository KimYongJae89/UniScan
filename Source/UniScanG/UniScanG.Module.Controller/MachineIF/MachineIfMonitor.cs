using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanG.Gravure.Data;
using UniScanG.MachineIF;
using UniScanG.Module.Controller.Device;
using UniScanG.Module.Controller.Device.Laser;
using UniScanG.Module.Controller.Settings.Monitor;

namespace UniScanG.Module.Controller.MachineIF
{

    public class MachineIfMonitor : UniScanG.MachineIF.MachineIfMonitor
    {
        public MachineIfMonitor(UniScanG.MachineIF.MachineIfDataAdapterG adapter) : base(adapter) { }

        public override void PropagateData()
        {
            MachineIfData machineIfData = (MachineIfData)this.adapter.MachineIfData;

            HanbitLaser hanbitLaser = ((DeviceController)SystemManager.Instance().DeviceController).HanbitLaser;
            hanbitLaser?.Update(machineIfData);
        }

        public override void ApplyData()
        {
            MachineIfData machineIfData = (MachineIfData)this.adapter.MachineIfData;

            machineIfData.SET_VISION_GRAVURE_INSP_READY = true;
            machineIfData.SET_VISION_GRAVURE_INSP_RUNNING = !(UniEye.Base.Data.SystemState.Instance().OpState == UniEye.Base.Data.OpState.Idle);

            ProductionG productionG = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
            machineIfData?.SetInspCnt(productionG);

            if (MonitorSystemSettings.Instance().UseLaserBurner == LaserMode.Virtual)
            {
                HanbitLaser hanbitLaser = ((DeviceController)SystemManager.Instance().DeviceController).HanbitLaser;
                hanbitLaser?.Apply(machineIfData);
            }
        }
    }
}
