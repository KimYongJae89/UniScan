using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.Dio
{
    public class ErrorCodeDigitalIo : ErrorCodes
    {
        public enum EIndexDigitalIo
        {
            CantFindMasterMotion = ECodeCommon.NEXT,
            InvalidMasterMotion,
        }

        public static ErrorCodeDigitalIo Instance => ErrorSectionDevice.Instance.DigitalIo;

        public ErrorCode CantFindMasterMotion => new ErrorCode(this, EIndexDigitalIo.CantFindMasterMotion);
        public ErrorCode InvalidMasterMotion => new ErrorCode(this, EIndexDigitalIo.InvalidMasterMotion);

        public ErrorCodeDigitalIo(ErrorSection errorSection)
            : base(errorSection, ErrorSectionDevice.ESectionDevice.DigitalIo) { }
    }
}
