using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.MotionController
{
    public class ErrorCodeMotion : ErrorCodes
    {
        public enum ECodeMotion
        {
            Homing = ECodeCommon.NEXT,
            HomingTimeOut,
            Moving,
            MovingTimeOut,
            ContinuousMoving,
            StopMove,
            EmergencyStop,
            PosLimit,
            NegLimit,
            AmpFault,
            ServoOff,
            HomeFound,
            CantFindNegLimit,
            CantFindPosLimit,
        }

        public static ErrorCodeMotion Instance => ErrorSectionDevice.Instance.Motion;

        public ErrorCode Homing { get; private set; }
        public ErrorCode HomingTimeOut { get; private set; }
        public ErrorCode Moving { get; private set; }
        public ErrorCode MovingTimeOut { get; private set; }
        public ErrorCode ContinuousMoving { get; private set; }
        public ErrorCode StopMove { get; private set; }
        public ErrorCode EmergencyStop { get; private set; }
        public ErrorCode PosLimit { get; private set; }
        public ErrorCode NegLimit { get; private set; }
        public ErrorCode AmpFault { get; private set; }
        public ErrorCode ServoOff { get; private set; }
        public ErrorCode HomeFound { get; private set; }
        public ErrorCode CantFindNegLimit { get; private set; }
        public ErrorCode CantFindPosLimit { get; private set; }

        public ErrorCodeMotion(ErrorSection errorSection)
            : base(errorSection, ErrorSectionDevice.ESectionDevice.Motion)
        {
            Homing = new ErrorCode(this, ECodeMotion.Homing);
            HomingTimeOut = new ErrorCode(this, ECodeMotion.HomingTimeOut);
            Moving = new ErrorCode(this, ECodeMotion.Moving);
            MovingTimeOut = new ErrorCode(this, ECodeMotion.MovingTimeOut);
            ContinuousMoving = new ErrorCode(this, ECodeMotion.ContinuousMoving);
            StopMove = new ErrorCode(this, ECodeMotion.StopMove);
            EmergencyStop = new ErrorCode(this, ECodeMotion.EmergencyStop);
            PosLimit = new ErrorCode(this, ECodeMotion.PosLimit);
            NegLimit = new ErrorCode(this, ECodeMotion.NegLimit);
            AmpFault = new ErrorCode(this, ECodeMotion.AmpFault);
            ServoOff = new ErrorCode(this, ECodeMotion.ServoOff);
            HomeFound = new ErrorCode(this, ECodeMotion.HomeFound);
            CantFindNegLimit = new ErrorCode(this, ECodeMotion.CantFindNegLimit);
            CantFindPosLimit = new ErrorCode(this, ECodeMotion.CantFindPosLimit);
        }
    }
}
