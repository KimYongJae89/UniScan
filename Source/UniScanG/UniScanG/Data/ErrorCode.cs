using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Data
{
    public class ErrorSectionSystemG : ErrorSectionSystem
    {
        public new static ErrorSectionSystemG Instance { get; } = new ErrorSectionSystemG();

        public new ErrorCodeModel Model { get; private set; }
        public new ErrorCodeTeach Teach { get; private set; }
        public new ErrorCodeInspect Inspect { get; private set; }

        public ErrorSectionSystemG() : base()
        {
            Model = new ErrorCodeModel(this);
            Teach = new ErrorCodeTeach(this);
            Inspect = new ErrorCodeInspect(this);
        }
    }

    public class ErrorCodeModel : ErrorCodes
    {
        public static ErrorCodeModel Instance => ErrorSectionSystemG.Instance.Model;

        public ErrorCodeModel(ErrorSection errorSection)
            : base(errorSection, ErrorSectionSystem.ESubSectionSystem.Model)
        {
        }
    }

    public class ErrorCodeTeach : ErrorCodes
    {
        public static ErrorCodeTeach Instance => ErrorSectionSystemG.Instance.Teach;

        public ErrorCode AutoTeach => new ErrorCode(this, (int)ECodeCommon.NEXT + 0, "AutoTeach");
        public ErrorCode AutoLight => new ErrorCode(this, (int)ECodeCommon.NEXT + 1, "AutoLight");

        public ErrorCodeTeach(ErrorSection errorSection)
            : base(errorSection, ErrorSectionSystem.ESubSectionSystem.Teach)
        {
        }
    }

    public class ErrorCodeInspect : ErrorCodes
    {
        public static ErrorCodeInspect Instance => ErrorSectionSystemG.Instance.Inspect;

        public ErrorCode BufferInitialize => new ErrorCode(this, (int)ECodeCommon.NEXT + 0, "BufferInitialize");
        public ErrorCode EnterWaitInspection => new ErrorCode(this, (int)ECodeCommon.NEXT + 1, "BeginInspect");
        public ErrorCode ExitWaitInspection => new ErrorCode(this, (int)ECodeCommon.NEXT + 2, "StopInspect");
        public ErrorCode EnterPauseInspection => new ErrorCode(this, (int)ECodeCommon.NEXT + 3, "PauseInspect");
        public ErrorCode ExitPauseInspection => new ErrorCode(this, (int)ECodeCommon.NEXT + 4, "Exit Pause Fail");

        public ErrorCodeInspect(ErrorSection errorSection)
            : base(errorSection, ErrorSectionSystem.ESubSectionSystem.Inspect)
        {
        }
    }
}
