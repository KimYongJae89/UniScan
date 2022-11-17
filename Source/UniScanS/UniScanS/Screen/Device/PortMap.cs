using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanS.Screen.Device
{
    public class PortMap : UniEye.Base.Device.PortMap
    {
        public enum IoPortName
        {
            InMachineRolling,
            InMachineRun,
            OutStop,
            OutAlarm
        }

        public PortMap() : base()
        {
            Initialize(typeof(IoPortName));
        }
    }
}
