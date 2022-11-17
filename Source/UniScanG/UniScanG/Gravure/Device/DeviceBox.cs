using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Device;
using UniEye.Base.Settings;

namespace UniScanG.Gravure.Device
{

    public class DeviceBox : UniEye.Base.Device.DeviceBox
    {
        protected override bool IsVirtualGrabber => MachineSettings.Instance().RunningMode != RunningMode.Real;
        protected override bool IsVirtualMotion => MachineSettings.Instance().RunningMode == RunningMode.Virtual;
        protected override bool IsVirtualDio => MachineSettings.Instance().RunningMode == RunningMode.Virtual;
        protected override bool IsVirtualLight => MachineSettings.Instance().RunningMode == RunningMode.Virtual;
        protected override bool IsVirtualDaq => MachineSettings.Instance().RunningMode == RunningMode.Virtual;
        protected override bool IsVirtualSerial => MachineSettings.Instance().RunningMode == RunningMode.Virtual;
        protected override bool IsVirtualMachineIf => MachineSettings.Instance().RunningMode != RunningMode.Real;

        public DeviceBox(UniEye.Base.Device.PortMap portMap) : base(portMap) { }

        public override Camera CreateCamera(Grabber grabber, CameraInfo cameraInfo)
        {
            //string virtualSourceImageNameFilter = string.Format("Image_C{0:00}_S???_L??.bmp", SystemManager.Instance().ExchangeOperator.GetCamIndex());
            //cameraInfo.VirtualImageNameFormat = virtualSourceImageNameFilter;

            Camera camera = base.CreateCamera(grabber, cameraInfo);
            return camera;
        }
    }
}
