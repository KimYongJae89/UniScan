using DynMvp.Device.Serial;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;

namespace UniScanM.CGInspector.Device
{
    public class DeviceBox : UniEye.Base.Device.DeviceBox
    {
        public DeviceBox(UniEye.Base.Device.PortMap portMap) : base(portMap)
        {
        }

        public override void PostInitialize()
        {
            SystemVersion stillImageVersion;
            bool ok = Enum.TryParse(OperationSettings.Instance().SystemType, out stillImageVersion);
            if (ok == false)
                return;

            SerialDevice sd = SystemManager.Instance().DeviceBox.SerialDeviceHandler.Find(f => f.DeviceInfo.DeviceType == DynMvp.Device.Serial.ESerialDeviceType.SerialEncoder);
                sd?.ExcuteCommand(SerialEncoderV105.ECommand.DV, "10");  //7
        }

        public override Camera CreateCamera(Grabber grabber, CameraInfo cameraInfo)
        {
            switch (grabber.Type)
            {
                case DynMvp.Devices.FrameGrabber.GrabberType.GenTL:
                    return new CameraGenTL(cameraInfo);
                case DynMvp.Devices.FrameGrabber.GrabberType.Virtual:
                    //return new CameraVirtualMSExtenderGM();                        
                    return new CameraVirtual(cameraInfo);
                default:
                    return base.CreateCamera(grabber, cameraInfo);
            }
        }
    }
}
