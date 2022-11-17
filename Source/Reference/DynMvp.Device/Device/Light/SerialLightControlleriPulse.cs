using DynMvp.Devices.Light;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.Light
{
    public class IPulseLightCtrlInfo : SerialLightCtrlInfo
    { }

    public class IPulseLightController : SerialLightCtrl
    {
        public IPulseLightController(string name) : base(name)
        {
        }
    }
}
