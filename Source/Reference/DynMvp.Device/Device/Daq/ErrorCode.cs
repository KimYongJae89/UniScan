using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.Daq
{
    public class ErrorCodeDaq : ErrorCodes
    {
        public enum EErrorCodeDaq
        {
        }

        public static ErrorCodeDaq Instance => new ErrorCodeDaq();

        public ErrorCodeDaq()
            : base(new ErrorSection(ErrorSections.ESection.Device), ErrorSectionDevice.ESectionDevice.DAQ) { }
    }
}
