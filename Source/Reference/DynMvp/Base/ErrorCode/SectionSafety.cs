using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Base
{
    public class ErrorSectionSafety : ErrorSubSections
    {
        enum SafetyError
        {
            DoorOpen,
            EmergencySwitch,
            AreaSensor
        }

        public static ErrorSectionSafety Instance { get; } = new ErrorSectionSafety();

        public ErrorCodes DoorOpen { get; private set; }
        public ErrorCodes EmergencySwitch { get; private set; }
        public ErrorCodes AreaSensor { get; private set; }

        public ErrorSectionSafety() : base(ErrorSections.ESection.Safety)
        {
            DoorOpen = new ErrorCodes(this, SafetyError.DoorOpen);
            EmergencySwitch = new ErrorCodes(this, SafetyError.EmergencySwitch);
            AreaSensor = new ErrorCodes(this, SafetyError.AreaSensor);
        }
    }
}
