using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanM.Gloss.Settings;

namespace UniScanM.Gloss.Device
{
    public class PortMap : UniEye.Base.Device.PortMap
    {
        internal enum IoPortName
        {
            OutMC
        }

        public PortMap() : base()
        {
            Clear();

            Initialize(typeof(IoPortName));
        }
    }
}
