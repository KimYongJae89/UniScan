using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.Serial
{
    public class ErrorCodeSerial : ErrorCodes
    {
        public static ErrorCodeSerial Instance => ErrorSectionDevice.Instance.Serial;

        public ErrorCodeSerial(ErrorSection errorSection) 
            : base(errorSection, ErrorSectionDevice.ESectionDevice.Serial) { }
    }
}
