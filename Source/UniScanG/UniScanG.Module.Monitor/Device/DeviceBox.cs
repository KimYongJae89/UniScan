using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Data;

namespace UniScanG.Module.Monitor.Device
{
    public class DeviceBox : UniEye.Base.Device.DeviceBox
    {
        public DeviceBox(UniEye.Base.Device.PortMap portMap) : base(portMap) { }

        public override Camera CreateCamera(Grabber grabber, CameraInfo cameraInfo)
        {
            //"Image_C{0:00}_S{1:000}_L{2:00}.{3}";
            //DynMvp.Devices.ImageBuffer.GetImage2dFileName(camIndex, (int)extType, this.index, System.Drawing.Imaging.ImageFormat.Bmp);

            string virtualImageNameFormat = string.Format("Image_C??_S{0:000}_L??.bmp", (int)ExtType.StopIMG);
            cameraInfo.VirtualImageNameFormat = virtualImageNameFormat;

            Camera camera = base.CreateCamera(grabber, cameraInfo);
            return camera;
        }
    }
}
