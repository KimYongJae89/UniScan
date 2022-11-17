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


namespace UniScanX.MPAlignment.Operation
{
    public class InspectRunnerExtender : UniEye.Base.Inspect.InspectRunnerExtender
    {
        protected override InspectionResult CreateInspectionResult()
        {
            Data.InspectionResult inspectionResult = new UniScanX.MPAlignment.Data.InspectionResult();
            return inspectionResult;
        }
    }
}
