using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScan
{
    class PortMap : UniEye.Base.Device.PortMap
    {
        public PortMap() : base() { }

        public override void SetupPorts()
        {
            OutVisionReady.Set(0);
            OutOnWorking.Set(1);
            OutComplete.Set(2);
            OutResultNg.Set(3);

            InTrigger.Set(0);
            InCommandDone.Set(1);
        }
    }
}
