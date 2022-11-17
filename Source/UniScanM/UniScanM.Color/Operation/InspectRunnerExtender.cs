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
using UniEye.Base.Settings;
using UniScanM.ColorSens.Data;

namespace UniScanM.ColorSens.Operation
{
    public class InspectRunnerExtender: UniScanM.Operation.InspectRunnerExtender
    {
        protected override DynMvp.InspData.InspectionResult CreateInspectionResult()
        {
            return new ColorSens.Data.InspectionResult();
        }
    }
}
