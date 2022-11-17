using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.UPS
{
    class NullUps : Ups
    {
        public NullUps(UpsSetting upsSetting) : base(upsSetting) { }

        public override SystemPowerStatus GetPowerState()
        {
            return null;
        }
    }
}
