using DynMvp.Base;
using DynMvp.Device.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Data;
using UniScanG.Module.Controller.Device.Laser;

namespace UniScanG.Module.Controller.Data
{
    public class ErrorSubSectionDeviceG : ErrorSectionDevice
    {
        public enum ESubSectionDeviceG
        {
            Laser = ErrorSectionDevice.ESectionDevice.NEXT,
        }

        public new static ErrorSubSectionDeviceG Instance { get; } = new ErrorSubSectionDeviceG();

        public ErrorCodeLaser Laser { get; private set; }

        public ErrorSubSectionDeviceG() : base()
        {
            Laser = new ErrorCodeLaser(this);
        }
    }

}
