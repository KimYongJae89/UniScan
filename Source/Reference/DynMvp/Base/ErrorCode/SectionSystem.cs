using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Base
{

    public class ErrorSectionSystem : ErrorSubSections
    {
        public enum ESubSectionSystem
        {
            Initialize = 000,
            User = 100,
            Model = 200,
            Teach = 300,
            Inspect = 400,
            Comms = 500,
        }

        public static ErrorSectionSystem Instance { get; } = new ErrorSectionSystem();

        public ErrorSubSectionInitialize Initialize { get; private set; } 
        public ErrorCodes User { get; private set; }
        public ErrorCodes Model { get; private set; } 
        public ErrorCodes Teach { get; private set; }
        public ErrorCodes Inspect { get; private set; }
        public ErrorSubSectionComms Comms { get; private set; } 

        public ErrorSectionSystem() : base(ErrorSections.ESection.System)
        {
            Initialize = new ErrorSubSectionInitialize(this);
            User = new ErrorCodes(this, ESubSectionSystem.User);
            Model = new ErrorCodes(this, ESubSectionSystem.Model);
            Teach = new ErrorCodes(this, ESubSectionSystem.Teach);
            Inspect = new ErrorCodes(this, ESubSectionSystem.Inspect);
            Comms = new ErrorSubSectionComms(this);
        }
    }

    public class ErrorSubSectionInitialize : ErrorCodes
    {
        public enum ESubSectionSystemInitialize
        {
            StartUp = ECodeCommon.NEXT,
            Release,
            DataRemove
        }

        public ErrorCode Initialize => new ErrorCode(this, ESubSectionSystemInitialize.StartUp);
        public ErrorCode Release => new ErrorCode(this, ESubSectionSystemInitialize.Release);
        public ErrorCode DataRemove => new ErrorCode(this, ESubSectionSystemInitialize.DataRemove);

        public ErrorSubSectionInitialize(ErrorSection errorSection)
            : base(errorSection, ErrorSectionSystem.ESubSectionSystem.Initialize) { }
    }

    public class ErrorSubSectionComms : ErrorCodes
    {
        public enum ESubSectionSystemComms
        {
            Connected = ECodeCommon.NEXT,
            Disconnected
        }
        public ErrorCode Connected => new ErrorCode(this, ESubSectionSystemComms.Connected);
        public ErrorCode Disconnected => new ErrorCode(this, ESubSectionSystemComms.Disconnected);

        public ErrorSubSectionComms(ErrorSection errorSection)
            : base(errorSection, ErrorSectionSystem.ESubSectionSystem.Comms) { }
    }
}
