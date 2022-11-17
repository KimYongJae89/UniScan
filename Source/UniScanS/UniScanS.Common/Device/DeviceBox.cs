using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Device;

namespace UniScanS.Common.Device
{
    public class DeviceBox : UniEye.Base.Device.DeviceBox
    {
        public DeviceBox(PortMap portMap) : base(portMap)
        {
        }

        public override Camera CreateCamera(Grabber grabber, CameraInfo cameraInfo)
        {
            switch (grabber.Type)
            {
                case DynMvp.Devices.FrameGrabber.GrabberType.GenTL:
                    return new CameraGenTL(cameraInfo);
                case DynMvp.Devices.FrameGrabber.GrabberType.Virtual:
                        return new CameraVirtualMS(cameraInfo);
                default:
                    return base.CreateCamera(grabber, cameraInfo);
            }
        }
    }
}
