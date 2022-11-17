using DynMvp.Base;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniScanM.Gloss.Data;
using WPF.UniScanCM.Manager;

namespace UniScanM.Gloss.MachineIF
{
    public class MachineIfDataExporter : DynMvp.Data.DataExporter
    {
        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            var machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            if (machineIf == null || machineIf.IsConnected == false)
                return;

            int index = 0;
            string[] mergeString = new string[24];
            var glossInspectionResult = (UniScanM.Gloss.Data.InspectionResult)inspectionResult;
            var glossScanData = glossInspectionResult.GlossScanData;

            if (machineIf.MachineIfSetting.MachineIfType == UniEye.Base.MachineInterface.MachineIfType.Melsec)
            {
                mergeString = new string[24];
                mergeString[index++] = ((int)(glossScanData.MinGloss * 10)).ToString("X04");
                mergeString[index++] = ((int)(glossScanData.MaxGloss * 10)).ToString("X04");
                mergeString[index++] = ((int)(glossScanData.AvgGloss * 10)).ToString("X04");
                mergeString[index++] = ((int)(glossScanData.DevGloss * 10)).ToString("X04");
                foreach (var glossData in glossInspectionResult.GlossScanData.GlossDatas)
                {
                    mergeString[index++] = ((int)(glossData.Y * 10)).ToString("X04");
                    if (index >= 24)
                        break;
                }
            }
            else if (machineIf.MachineIfSetting.MachineIfType == UniEye.Base.MachineInterface.MachineIfType.AllenBreadley)
            {
                mergeString = new string[23];
                mergeString[index++] = ((int)(glossScanData.AvgGloss * 10)).ToString("X04");
                mergeString[index++] = ((int)(glossScanData.MinGloss * 10)).ToString("X04");
                mergeString[index++] = ((int)(glossScanData.MaxGloss * 10)).ToString("X04");
                //mergeString[index++] = ((int)(glossScanData.DevGloss * 10)).ToString("X04");
                foreach (var glossData in glossInspectionResult.GlossScanData.GlossDatas)
                {
                    mergeString[index++] = ((int)(glossData.Y * 10)).ToString("X04");
                    if (index >= 23)
                        break;
                }
            }

            SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfGlossCommand.SET_GLOSS, mergeString);
        }

        public void ExportVisionState()
        {
            var machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            if (machineIf == null || machineIf.IsConnected == false)
                return;

            string[] mergeString = new string[3];
            mergeString[0] = (AliveService.Heart ? 1 : 0).ToString("X04");
            mergeString[1] = (SystemState.Instance().InspectState == InspectState.Run ? 1 : 0).ToString("X04");
            mergeString[2] = (ErrorManager.Instance().IsAlarmed() ? 1 : 0).ToString("X04");

            SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfGlossCommand.SET_VISION_STATE, mergeString);
        }

        public void ExportLotTotalResult(List<Data.InspectionResult> inspectionResultList)
        {
            var machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            if (machineIf == null || machineIf.IsConnected == false)
                return;

            if (inspectionResultList == null || inspectionResultList.Count == 0)
                return;

            List<GlossScanData> glossScanDataList = inspectionResultList.ConvertAll(x => x.GlossScanData);
            float gMin = glossScanDataList.Min(x => x.MinGloss);
            float gMax = glossScanDataList.Max(x => x.MaxGloss);
            float gAvg = glossScanDataList.Average(x => x.AvgGloss);
            float dMin = glossScanDataList.Min(x => x.MinDistance);
            float dMax = glossScanDataList.Max(x => x.MaxDistance);
            float dAvg = glossScanDataList.Average(x => x.AvgDistance);

            var gloassDataList = new List<float>();
            var distanceDataList = new List<float>();
            foreach (var glossData in glossScanDataList)
            {
                gloassDataList.AddRange(glossData.GlossDatas.ConvertAll<float>(x => x.Y));
                distanceDataList.AddRange(glossData.GlossDatas.ConvertAll<float>(x => x.Distance));
            }
            float gDev = DynMvp.Vision.DataProcessing.StdDev(gloassDataList.ToArray());
            float dDev = DynMvp.Vision.DataProcessing.StdDev(distanceDataList.ToArray());

            int index = 0;
            string[] mergeString = new string[24];
            if (machineIf.MachineIfSetting.MachineIfType == UniEye.Base.MachineInterface.MachineIfType.Melsec)
            {
                mergeString[index++] = ((int)(gMin * 10)).ToString("X04");
                mergeString[index++] = ((int)(gMax * 10)).ToString("X04");
                mergeString[index++] = ((int)(gAvg * 10)).ToString("X04");
                mergeString[index++] = ((int)(gDev * 10)).ToString("X04");
            }
            else if (machineIf.MachineIfSetting.MachineIfType == UniEye.Base.MachineInterface.MachineIfType.AllenBreadley)
            {
                mergeString[index++] = ((int)(gAvg * 10)).ToString("X04");
                mergeString[index++] = ((int)(gMin * 10)).ToString("X04");
                mergeString[index++] = ((int)(gMax * 10)).ToString("X04");
                //mergeString[index++] = ((int)(gDev * 10)).ToString("X04");
            }

            SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfGlossCommand.SET_TOTAL_GLOSS, mergeString);
        }
    }
}