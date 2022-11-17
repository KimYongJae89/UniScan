using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.InspData;
using UniEye.Base;
using UniEye.Base.MachineInterface;
using UniScanM.Data;
using UniScanM.Operation;

namespace UniScanM.CGInspector.MachineIF
{
    public class MachineIfDataExporter : DynMvp.Data.DataExporter
    {
        int[] sheetNgCntZone = new int[]{0,0,0,0,0, 0,0,0,0,0,0};

        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            if (SystemManager.Instance().DeviceBox.MachineIf == null)
                return;

            if (SystemManager.Instance().DeviceBox.MachineIf.IsConnected == false)
                return;

            Data.InspectionResult myInspectionResult = (Data.InspectionResult)inspectionResult;

        }
    }
}
