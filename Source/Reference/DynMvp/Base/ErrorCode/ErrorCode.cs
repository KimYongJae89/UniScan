using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Base
{
    public enum ErrorLevel
    {
        /// <summary>
        /// 프로그램의 재시동이 필요한 오류. 
        /// HW 초기화 및 SW 로드 실패 등.
        /// </summary>
        Fatal,

        /// <summary>
        /// 현재의 동작을 정지해야 할 오류. 
        /// 설비 정지 조건의 알람 발생 등.
        /// </summary>
        Error,

        /// <summary>
        /// 사용자 통보가 필요한 경고. 
        /// 설비 미정지 조건의 알람 발생 등.
        /// </summary>
        Warning,

        /// <summary>
        /// 사용자 통보 없이 사후 조회가 필요한 정보. 
        /// 검사 시작/정지 기록, 주요 상태 변화 등.
        /// </summary>
        Info
    }

    public static class ErrorLevelMethod
    {
        public static bool IsAlarm(this ErrorLevel errorLevel) => (errorLevel == ErrorLevel.Fatal || errorLevel == ErrorLevel.Error);
    }

    public abstract class ErrorSectionBase
    {
        public virtual int Code => this.code;
        protected int code;

        public string Message => this.message;
        protected string message;

        public ErrorSectionBase(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public override string ToString()
        {
            return string.Format("[{0}:{1}]", code, message);
        }
    }

    public class ErrorSection : ErrorSectionBase
    {
        public ErrorSection(Enum section) : base(Convert.ToInt32(section), section.ToString()) { }
    }

    public class ErrorSubSection : ErrorSectionBase
    {
        public ErrorSection ErrorSection => errorSection;
        protected ErrorSection errorSection;

        public override int Code => errorSection.Code + this.code;

        public ErrorSubSection(ErrorSection errorSection, Enum subSection) : base(Convert.ToInt32(subSection), subSection.ToString())
        {
            this.errorSection = errorSection;

        }

        public ErrorSubSection(ErrorSection errorSection, int subSectionCode, string subSectionName) : base(subSectionCode, subSectionName)
        {
            this.errorSection = errorSection;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", errorSection.ToString(), base.ToString());
        }
    }

    public class ErrorCode : ErrorSectionBase
    {
        public ErrorSubSection ErrorSubSection => errorSubSection;
        protected ErrorSubSection errorSubSection;

        public override int Code => errorSubSection.Code + this.code;

        public ErrorCode(ErrorSubSection errorSubSection, Enum index) : base(Convert.ToInt32(index), index.ToString())
        {
            this.errorSubSection = errorSubSection;
        }

        public ErrorCode(ErrorSubSection errorSubSection, int code, string message) : base(code, message)
        {
            this.errorSubSection = errorSubSection;
        }

        public override string ToString()
        {
            return string.Format("{0}{1}", errorSubSection.ToString(), base.ToString());
        }
    }

    public class ErrorSections
    {
        public enum ESection
        {
            System = 0,
            Device = 1000,
            Safety = 3000,
        }
    }

    public class ErrorSubSections : ErrorSection
    {
        public ErrorSubSections(ErrorSections.ESection section) : base(section) { }
    }

    public class ErrorCodes : ErrorSubSection
    {
        protected enum ECodeCommon
        {
            Information,
            InvalidType,
            FailToCreate,
            FailToInitialize,
            InvalidSetting,
            FailToReadParamFile,
            FailToReadParam,
            FailToWriteParam,
            FailToReadValue,
            FailToWriteValue,
            FailToRelease,
            InvalidState,
            Timeout,
            NEXT,
        }

        public ErrorCode Information => new ErrorCode(this, ECodeCommon.Information);
        public ErrorCode InvalidType => new ErrorCode(this, ECodeCommon.InvalidType);
        public ErrorCode FailToCreate => new ErrorCode(this, ECodeCommon.FailToCreate);
        public ErrorCode FailToInitialize => new ErrorCode(this, ECodeCommon.FailToInitialize);
        public ErrorCode InvalidSetting => new ErrorCode(this, ECodeCommon.InvalidSetting);
        public ErrorCode FailToReadParamFile => new ErrorCode(this, ECodeCommon.FailToReadParamFile);
        public ErrorCode FailToReadParam => new ErrorCode(this, ECodeCommon.FailToReadParam);
        public ErrorCode FailToWriteParam => new ErrorCode(this, ECodeCommon.FailToWriteParam);
        public ErrorCode FailToReadValue => new ErrorCode(this, ECodeCommon.FailToReadValue);
        public ErrorCode FailToWriteValue => new ErrorCode(this, ECodeCommon.FailToWriteValue);
        public ErrorCode FailToRelease => new ErrorCode(this, ECodeCommon.FailToRelease);
        public ErrorCode InvalidState => new ErrorCode(this, ECodeCommon.InvalidState);
        public ErrorCode Timeout => new ErrorCode(this, ECodeCommon.Timeout);

        public ErrorCodes(ErrorSection errorSection, Enum subSection) : base(errorSection, subSection) { }
        public ErrorCodes(ErrorSection errorSection, int subSectionCode, string subSectionName) : base(errorSection, subSectionCode, subSectionName) { }

    }

    //public class ErrorSubSectionMachine 
    //{
    //    enum MachineError
    //    {
    //        CylinderInjection,
    //        CylinderEjection,
    //        ConveyorMovingTimeOut,
    //        CantDetectPart,
    //        InitTimeOut,
    //        AirPressure,
    //        VaccumOn,
    //        Light,
    //        Serial
    //    }

    //    public static ErrorSubSection CylinderInjection => ErrorSubSections.GetSubSection(MachineError.CylinderInjection);
    //    public static ErrorSubSection CylinderEjection => ErrorSubSections.GetSubSection(MachineError.CylinderEjection);
    //    public static ErrorSubSection ConveyorMovingTimeOut => ErrorSubSections.GetSubSection(MachineError.ConveyorMovingTimeOut);
    //    public static ErrorSubSection CantDetectPart => ErrorSubSections.GetSubSection(MachineError.CantDetectPart);
    //    public static ErrorSubSection InitTimeOut => ErrorSubSections.GetSubSection(MachineError.InitTimeOut);
    //    public static ErrorSubSection AirPressure => ErrorSubSections.GetSubSection(MachineError.AirPressure);
    //    public static ErrorSubSection VaccumOn => ErrorSubSections.GetSubSection(MachineError.VaccumOn);
    //    public static ErrorSubSection Light => ErrorSubSections.GetSubSection(MachineError.Light);
    //    public static ErrorSubSection Serial => ErrorSubSections.GetSubSection(MachineError.Serial);
    //}

    //public class ErrorSubSectionExternalIf 
    //{
    //    enum ExternalIfError
    //    {
    //        CommunicationTimeout,
    //        InvalidRemoteIp
    //    }

    //    public static ErrorSubSection CommunicationTimeout => ErrorSubSections.GetSubSection(ExternalIfError.CommunicationTimeout);
    //    public static ErrorSubSection InvalidRemoteIp => ErrorSubSections.GetSubSection(ExternalIfError.InvalidRemoteIp);
    //}

    //public class ErrorSubSectionInspect
    //{
    //    enum InspectError
    //    {
    //        FiducialError,
    //        FiducialLengthError
    //    }

    //    public static ErrorSubSection FiducialError => ErrorSubSections.GetSubSection(InspectError.FiducialError);
    //    public static ErrorSubSection FiducialLengthError => ErrorSubSections.GetSubSection(InspectError.FiducialLengthError);
    //}
}
