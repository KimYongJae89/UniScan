using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Module.Controller.Data;

namespace UniScanG.Module.Controller.Device.Laser
{
    public class ErrorCodeLaser : ErrorCodes
    {
        public enum ECodeLaser
        {
            Alive = ECodeCommon.NEXT,
            Ready,
            OutOfRange,
            FailToDetection
        }

        public static ErrorCodeLaser Instance => ErrorSubSectionDeviceG.Instance.Laser;

        public ErrorCode Alive => new ErrorCode(this, ECodeLaser.Alive);
        public ErrorCode Ready => new ErrorCode(this, ECodeLaser.Ready);
        public ErrorCode OutOfRange => new ErrorCode(this, ECodeLaser.OutOfRange);
        public ErrorCode FailToDetection => new ErrorCode(this, ECodeLaser.FailToDetection);

        public ErrorCodeLaser(ErrorSection errorSection)
            : base(errorSection, ErrorSubSectionDeviceG.ESubSectionDeviceG.Laser) { }
    }
}
