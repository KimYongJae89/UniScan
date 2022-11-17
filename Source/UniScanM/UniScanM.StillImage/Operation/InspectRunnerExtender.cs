using DynMvp.Data;
using DynMvp.Devices.MotionController;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanM.MachineIF;

namespace UniScanM.StillImage.Operation
{
    public class InspectRunnerExtender: UniScanM.Operation.InspectRunnerExtender
    {
        protected override InspectionResult CreateInspectionResult()
        {
            Data.InspectionResult inspectionResult = new UniScanM.StillImage.Data.InspectionResult();
            return inspectionResult;
        }
    }
}
