using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.Cylinder
{
    public class ErrorCodeCylinder : ErrorCodes
    {
        public enum EIndexCylinder
        {
            Inject = ECodeCommon.NEXT,
            Eject = 01,
        }
        public static ErrorCodeCylinder Instance => new ErrorCodeCylinder();

        public ErrorCode Inject => new ErrorCode(this, EIndexCylinder.Inject);
        public ErrorCode Eject => new ErrorCode(this, EIndexCylinder.Eject);

        public ErrorCodeCylinder()
            : base(new ErrorSection(ErrorSections.ESection.Device), ErrorSectionDevice.ESectionDevice.Cylinder) { }
    }
}
