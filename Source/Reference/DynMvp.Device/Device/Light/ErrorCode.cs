using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.Light
{
    public class ErrorCodeLight : ErrorCodes
    {
        public enum EErrorCodeLight
        {
        }

        public static ErrorCodeLight Instance => new ErrorCodeLight();

        public ErrorCodeLight()
            : base(new ErrorSection(ErrorSections.ESection.Device), ErrorSectionDevice.ESectionDevice.Light) { }
    }
}
