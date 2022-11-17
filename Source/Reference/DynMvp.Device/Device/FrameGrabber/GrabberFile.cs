using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Devices.FrameGrabber
{
    public class GrabberFile : GrabberVirtual
    {
        public GrabberFile(string name) : base(GrabberType.File, name)
        {
            LogHelper.Debug(LoggerType.StartUp, "File Camera Manager Created");
        }

        public override Camera CreateCamera(CameraInfo cameraInfo)
        {
            return new CameraFile(cameraInfo);
        }

        public override bool Initialize(GrabberInfo grabberInfo)
        {
            return true;
        }
    }
}
