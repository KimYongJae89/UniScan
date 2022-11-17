using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.FrameGrabber
{
    public class ErrorCodeGrabber : ErrorCodes
    {
        public enum EErrorCodeGrabber        {        }

        public static ErrorCodeGrabber Instance => ErrorSectionDevice.Instance.Grabber;

        public ErrorCodeGrabber(ErrorSection errorSection)
            : base(errorSection, ErrorSectionDevice.ESectionDevice.Grabber) { }
    }
}
