using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.InspData;
using UniEye.Base;
using UniEye.Base.MachineInterface;
using UniScanM.Operation;

namespace UniScanM.Pinhole.MachineIF
{
    public class MachineIfDataExporter : DynMvp.Data.DataExporter
    {
        Data.InspectionResult lastInspectionResult = null;

        public MachineIfDataExporter()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(50);

                    if (SystemManager.Instance().DeviceBox.MachineIf == null)
                        continue;

                    if (SystemManager.Instance().DeviceBox.MachineIf?.IsConnected == false)
                        continue;

                    if (OperationOption.Instance().OnTune)
                        continue;
                    
                    ExportToPLC();

                    
                }
            });
        }

        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            if (SystemManager.Instance().DeviceBox.MachineIf == null)
                return;

            if (SystemManager.Instance().DeviceBox.MachineIf.IsConnected == false)
                return;

            InspectionResult inspectionResult2 = (InspectionResult)inspectionResult;

            lastInspectionResult = (Data.InspectionResult)inspectionResult;
        }

        public void ExportToPLC()
        {
            if (lastInspectionResult == null)
                return;

            int result = lastInspectionResult.IsGood() ? 0 : 1;
            lastInspectionResult = null;

            //int totalDefect = Data.InspectionResult.TotalDefectCount;
            Pinhole.Data.Production production = (Pinhole.Data.Production)SystemManager.Instance().ProductionManager.CurProduction;
            int totalDefect = production.TotalNum;
            string data = string.Format("{0:X04}{1:X04}", result, totalDefect);

            {
                if (SystemManager.Instance().DeviceBox.MachineIf.IsConnected == false)
                    return;

                if (SystemManager.Instance().DeviceBox.MachineIf.IsIdle)
                {
                    SystemManager.Instance().DeviceBox.MachineIf?.SendCommand(UniScanMMachineIfPinholeCommand.SET_PINHOLE_GOOD, data);  // Update PLC
                }

            }
        }
    }
}
