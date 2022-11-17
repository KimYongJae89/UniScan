using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;

using Shared;

namespace DynMvp.Devices.Dio
{
    class DigitalIoUniSensor : DigitalIo
    {
        public DigitalIoUniSensor(string name)
            : base(DigitalIoType.UniSensorDIO, name)
        {
        }

        public override uint ReadInputGroup(int groupNo)
        {
            throw new NotImplementedException();
        }

        public override uint ReadOutputGroup(int groupNo)
        {
            throw new NotImplementedException();
        }

        public override void WriteInputGroup(int groupNo, uint inputPortStatus)
        {
            throw new NotImplementedException();
        }

        public override void WriteOutputGroup(int groupNo, uint outputPortStatus)
        {
            throw new NotImplementedException();
        }
    }
}
