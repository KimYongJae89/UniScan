using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.InspData;
using UniScanG.Module.Monitor.Device;
using UniScanG.Module.Monitor.MachineIF;

namespace UniScanG.Module.Monitor.Exporter
{
    public class MelsecDataExporter : DynMvp.Data.DataExporter
    {
        MachineIfData machineIfData = null;
        public bool BlockUpdate { get; set; }

        public MelsecDataExporter()
        {
            this.machineIfData = (SystemManager.Instance().DeviceController as DeviceController)?.MachineIfMonitor?.MachineIfData as MachineIfData;
        }

        public override void Export(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            if (BlockUpdate)
            {
                machineIfData.SET_VISION_GRAVURE_MONITORING_RESULT = false;

                machineIfData.SET_VISION_GRAVURE_MONITORING_MARGIN_W
                    = machineIfData.SET_VISION_GRAVURE_MONITORING_MARGIN_L
                    = machineIfData.SET_VISION_GRAVURE_MONITORING_BLOT_W
                    = machineIfData.SET_VISION_GRAVURE_MONITORING_BLOT_L
                    = machineIfData.SET_VISION_GRAVURE_MONITORING_DEFECT_W
                    = machineIfData.SET_VISION_GRAVURE_MONITORING_DEFECT_L
                    = 0;
            }
            else
            {

                machineIfData.SET_VISION_GRAVURE_MONITORING_RESULT = !inspectionResult.IsGood(); // true(1) 일 때 불량. false(0) 일 때 정상.

                Inspect.InspectionResult inspectionResult2 = inspectionResult as Inspect.InspectionResult;
                machineIfData.SET_VISION_GRAVURE_MONITORING_MARGIN_W = inspectionResult2.MarginSize.Width;
                machineIfData.SET_VISION_GRAVURE_MONITORING_MARGIN_L = inspectionResult2.MarginSize.Height;
                machineIfData.SET_VISION_GRAVURE_MONITORING_BLOT_W = inspectionResult2.BlotSize.Width;
                machineIfData.SET_VISION_GRAVURE_MONITORING_BLOT_L = inspectionResult2.BlotSize.Height;
                machineIfData.SET_VISION_GRAVURE_MONITORING_DEFECT_W = inspectionResult2.MaxDefectSize.Width;
                machineIfData.SET_VISION_GRAVURE_MONITORING_DEFECT_L = inspectionResult2.MaxDefectSize.Height;
            }
        }
    }
}
