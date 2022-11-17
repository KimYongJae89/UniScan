using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Device
{
    public abstract class DeviceController : UniEye.Base.Device.DeviceController
    {
        public abstract void Launch(string imName, string[] args);
        public abstract void Shutdown(string imName, bool restart);
        public abstract void Startup(string imName);
        public abstract void InitializeInspectModule(System.Windows.Forms.IWin32Window parent, string imName);

    }
}
