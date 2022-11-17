using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniView.Client.Protocol
{
    public enum UniViewProtocolType
    {
        State,      // 상태 전송
        Result,     // 결과 전송
        Command     // 장비에 커맨드 전달
    }

    public enum MachineStateType
    {
        Run, Idle, Error, Teach, OnInspect, Unknown
    }

    public enum JudgementType
    {
        Ok, Ng, Overkill, Unknown
    }

    #region 확장 메서드
    public static class JudgementTypeExtensions
    {
        public static char ToChar(this JudgementType judgement)
        {
            switch (judgement)
            {
                case JudgementType.Ok:
                    return 'G';
                case JudgementType.Ng:
                    return 'N';
                case JudgementType.Overkill:
                    return 'O';
                default:
                    return 'U';
            }
        }

        public static JudgementType ToJudgement(this char resultChar)
        {
            switch (resultChar)
            {
                case 'G':
                    return JudgementType.Ok;
                case 'N':
                    return JudgementType.Ng;
                case 'O':
                    return JudgementType.Overkill;
                default:
                    return JudgementType.Unknown;
            }
        }
    }
    #endregion

    public enum VisionCommandType
    {
        Start, Stop, Skip, Pass, SetOk, SetNg, Reset, Unknown
    }

    public enum MachineCommandType
    {
        Start, Stop, Reset, Unknown
    }
}
