using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;

namespace UniScanWPF.Screen.PinHoleColor.Device
{
    public class PortMap : UniEye.Base.Device.PortMap
    {
        internal enum IoPortName
        {
            InRolling, InMachine,
            OutRun, OutSync, OutPinHole, OutColor
        }

        internal enum ModelPortName
        {
            InModel1, InModel2, InModel3
        }

        public PortMap() : base()
        {
            Clear();

            Initialize(typeof(IoPortName));
            Initialize(typeof(ModelPortName));
        }
    }
}
