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
using UniScanM.ColorSens.Data;
using UniScanM.Operation;
using UniEye.Base.Data;

namespace UniScanM.ColorSens.MachineIF
{
    public class MachineIfDataExporter : UniScanM.MachineIF.DataExporterM
    {
        public override void SetVisionState()
        {
            MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            if (machineIf == null)
                return;

            if (machineIf.IsConnected == false)
                return;

            string ready = "0001";
            string run = SystemState.Instance().IsInspectOrWait ? "0001" : "0000";
            //string run = SystemManager.Instance().InspectStarter.StartMode == StartMode.Auto ? "0001" : "0000";

            string sendData = string.Format("{0}{1}", ready, run);
            machineIf.SendCommand(UniScanMMachineIfColorSensorCommand.SET_VISION_STATE, sendData);
        }

        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            if (SystemManager.Instance().DeviceBox.MachineIf == null)
                return;

            if (SystemManager.Instance().DeviceBox.MachineIf.IsConnected == false)
                return;

            if (OperationOption.Instance().OnTune)
                return;

            UniScanM.ColorSens.Data.InspectionResult myInspectionResult = inspectionResult as UniScanM.ColorSens.Data.InspectionResult;
            if (myInspectionResult != null)
            {
                MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;

                string good = "", sheetBrightness = "";
                good = myInspectionResult.IsGood() ? "0000" : "0001";
                int sheetBrghtnessInt = (int)myInspectionResult.SheetBrightness * 10;
                if (sheetBrghtnessInt >= 1000 || sheetBrghtnessInt < 0)
                {
                    LogHelper.Debug(LoggerType.Operation, "sheetBrghtness is overflow set 0");
                    sheetBrghtnessInt = 0;
                }


                sheetBrightness = string.Format("{0:X04}", sheetBrghtnessInt);//sheetBrightness

                string[] sendData = new string[] { good, sheetBrightness };
                machineIf.SendCommand(UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR, sendData);

                //machineIfProtocolResponce = machineIf.SendCommand(UniScanMMachineIfColorSensorCommand.SET_COLORSENSOR_NG, 500, good);
                //machineIfProtocolResponce = machineIf.SendCommand(UniScanMMachineIfColorSensorCommand.SET_SHEET_BRIGHTNESS, 500, sheetBrightness);
            }
        }
    }
}
