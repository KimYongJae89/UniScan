using DynMvp.Base;
using DynMvp.Device.Device;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base;
using UniEye.Base.Device;
using DynMvp.Device.Device.MotionController;
using UniEye.Base.Settings;
using DynMvp.Devices.MotionController;
using UniEye.Base.UI;
using System.Threading;
using DynMvp.Device.Serial;
using DynMvp.Devices.Light;
using DynMvp.Devices;
using DynMvp.Data;
using UniScanG.Gravure.Data;
using UniScanG.Common.Settings;
using UniScanG.Module.Controller.MachineIF;

namespace UniScanG.Module.Controller.Device
{
    public abstract class DeviceControllerExtender
    {
        protected UniEye.Base.Device.DeviceController deviceController = null;

        public DeviceControllerExtender(DeviceController deviceController)
        {
            this.deviceController = deviceController;
        }

        public abstract void Initialize(DeviceBox deviceBox);
        public abstract void Update(MachineIfData machineIfData);
        public abstract void Apply(MachineIfData machineIfData);
    }
}
