using DynMvp.Device.Serial;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;

namespace UniScanM.ColorSens.Device
{
    public class DeviceBox : UniEye.Base.Device.DeviceBox
    {
        public DeviceBox(UniEye.Base.Device.PortMap portMap) : base(portMap)
        {
        }

        public override Camera CreateCamera(Grabber grabber, CameraInfo cameraInfo)
        {
            switch (grabber.Type)
            {
                case DynMvp.Devices.FrameGrabber.GrabberType.Virtual:
                    return new CameraVirtualMS(cameraInfo);

                case DynMvp.Devices.FrameGrabber.GrabberType.Pylon:
                    return new CameraPylon(cameraInfo);

                default:
                    return base.CreateCamera(grabber, cameraInfo);
            }
        }
    }
}
