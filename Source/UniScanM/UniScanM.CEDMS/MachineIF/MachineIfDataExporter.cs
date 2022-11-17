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
using UniScanM.Data;
using UniScanM.CEDMS.Settings;

namespace UniScanM.CEDMS.MachineIF
{
    public class MachineIfDataExporter : DynMvp.Data.DataExporter
    {
        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            if (SystemManager.Instance().DeviceBox.MachineIf == null)
                return;

            if (SystemManager.Instance().DeviceBox.MachineIf.IsConnected == false)
                return;

            UniScanM.CEDMS.Data.InspectionResult myInspectionResult = (UniScanM.CEDMS.Data.InspectionResult)inspectionResult;

            int resultInt = 0;
            CEDMSSettings cedmsSettings = (CEDMSSettings.Instance());
            if (cedmsSettings.AlarmUse)
            {
                resultInt = inspectionResult.Judgment == Judgment.Reject ? 1 : 0;
            }

            string mergeString =
                string.Format("{0}{1}{2}"
                , string.Format("{0:X04}", (short)resultInt)
                , string.Format("{0:X04}", (short)(myInspectionResult.InFeed.Y * 1000))
                , string.Format("{0:X04}", (short)(myInspectionResult.OutFeed.Y * 1000))
                );

            //string good = string.Format("{0:X04}", (short)1);
            //string gearSideY = string.Format("{0:X04}", (short)5100);
            //string manSideY = string.Format("{0:X04}", (short)5200);
            //string noneData = "0000"; 
            
            //string mergeString = string.Format("{0}{1}{2}{3}", good, gearSideY, manSideY, noneData);
            SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfCEDMSCommand.SET_CEDMS, mergeString);

            //machineIfProtocolResponce = SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfEDMSCommand.SET_EDMS_RUN, 500, );

            //if (machineIfProtocolResponce.IsResponced==false)
            //    LogHelper.Warn(LoggerType.Network, "MachineIfDataExporter::Export - no responce");
            //else if(machineIfProtocolResponce.IsGood==false)
            //    LogHelper.Warn(LoggerType.Network, "MachineIfDataExporter::Export - result is not good");

        }
    }
}
