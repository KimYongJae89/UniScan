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

namespace UniScanM.StillImage.MachineIF
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

            if (myInspectionResult.ProcessResultList != null)
            {
                string good = "", marginW = "", marginL = "", blotW = "", blotL = "", defectW = "", defectL = "";

                SizeF pelSize = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize;
                Data.ProcessResult processResult = myInspectionResult.ProcessResultList.InterestProcessResult;
                //if (processResult != null)
                //{
                //    Data.Feature inspFeature = processResult.InspPatternInfo.TeachInfo.Feature.Mul(pelSize);
                //    Data.Feature offsetFeature = processResult.OffsetValue.Mul(pelSize);

                //    marginW = string.Format("{0:X04}", (int)(inspFeature.Margin.Width* 10));//MarginW
                //    marginL = string.Format("{0:X04}", (int)(inspFeature.Margin.Height * 10));//MarginW
                //    blotW = string.Format("{0:X04}", (int)(offsetFeature.Blot.Width * 10));//MarginW
                //    blotL = string.Format("{0:X04}", (int)(offsetFeature.Blot.Height * 10));//MarginW
                //}
                //SizeF defectSize = myInspectionResult.ProcessResultList.GetMaxSizeDefectRect().Size;
                //defectSize.Width *= pelSize.Width;
                //defectSize.Height*= pelSize.Height;
                //defectW = string.Format("{0:X04}", (int)(defectSize.Width * 1000));//MarginW
                //defectL = string.Format("{0:X04}", (int)(defectSize.Height * 1000));//MarginW

                //good = myInspectionResult.IsGood() ? "0000" : "0001";
                //MachineIfProtocolResponce machineIfProtocolResponce = null; ;
                //machineIfProtocolResponce = SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfStillImageCommand.SET_STILLIMAGE_GOOD, 500, good);
                //machineIfProtocolResponce = SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfStillImageCommand.SET_MARGIN_W, 500, marginW);
                //machineIfProtocolResponce = SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfStillImageCommand.SET_MARGIN_L, 500, marginL);
                //machineIfProtocolResponce = SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfStillImageCommand.SET_BLOT_W, 500, blotW);
                //machineIfProtocolResponce = SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfStillImageCommand.SET_BLOT_L, 500, blotL);
                //machineIfProtocolResponce = SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfStillImageCommand.SET_DEFECT_W, 500, defectW);
                //machineIfProtocolResponce = SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfStillImageCommand.SET_DEFECT_L, 500, defectL);

                //All write
                //ADDED
                if (processResult != null)
                {
                    Data.Feature inspFeature = processResult.InspPatternInfo.TeachInfo.Feature.Mul(pelSize);
                    Data.Feature offsetFeature = processResult.OffsetValue.Mul(pelSize);
                    
                    marginW = MelsecDataConverter.WInt((int)(inspFeature.Margin.Width * 10));//MarginW
                    marginL = MelsecDataConverter.WInt((int)(inspFeature.Margin.Height * 10));//MarginW
                    blotW = MelsecDataConverter.WInt((int)(offsetFeature.Blot.Width * 10));//MarginW
                    blotL = MelsecDataConverter.WInt((int)(offsetFeature.Blot.Height * 10));//MarginW
                }
                else
                {
                    marginW = MelsecDataConverter.WInt(0);
                    marginL = MelsecDataConverter.WInt(0);
                    blotW = MelsecDataConverter.WInt(0);
                    blotL = MelsecDataConverter.WInt(0);
                }

                SizeF defectSize = myInspectionResult.ProcessResultList.GetMaxSizeDefectRect().Size;
                defectSize.Width *= pelSize.Width;
                defectSize.Height *= pelSize.Height;
                defectW = MelsecDataConverter.WInt((int)(defectSize.Width * 100));//MarginW
                defectL = MelsecDataConverter.WInt((int)(defectSize.Height * 100));//MarginW

                if (OperationOption.Instance().OnTune == false)
                {
                    string judge = getNGSignal(myInspectionResult);

                    string sendData = string.Format("{0}0000{1}{2}{3}{4}{5}{6}",
                    judge, marginW, marginL, blotW, blotL, defectW, defectL
                    );
                    if (SystemManager.Instance().DeviceBox.MachineIf.IsIdle)
                        SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfStillImageCommand.SET_STILLIMAGE, sendData);
                }
                //ADDED

                //if (machineIfProtocolResponce.IsResponced==false)
                //    LogHelper.Warn(LoggerType.Network, "MachineIfDataExporter::Export - no responce");
                //else if(machineIfProtocolResponce.IsGood==false)
                //    LogHelper.Warn(LoggerType.Network, "MachineIfDataExporter::Export - result is not good");

            }
        }

        private string getNGSignal(DynMvp.InspData.InspectionResult inspectionResult)
        {
            Data.InspectionResult stillImageinspectionResult = (Data.InspectionResult)inspectionResult;
            int zoneNum = stillImageinspectionResult.InspZone;
            if (stillImageinspectionResult.InspectState != "Inspection")
            {
                sheetNgCntZone[zoneNum] = 0;
            }
            else
            {
                if (stillImageinspectionResult.Judgment == DynMvp.InspData.Judgment.Accept)
                    sheetNgCntZone[zoneNum] = 0;
                else
                    sheetNgCntZone[zoneNum]++;
            }
            stillImageinspectionResult.SheetNgCnt = sheetNgCntZone[zoneNum];

            string NG = Settings.StillImageSettings.Instance().UserStopMode ?
                        (stillImageinspectionResult.SheetNgCnt < Settings.StillImageSettings.Instance().StopDefectSheetCnt ? "0000" : "0001")
                        : "0000";
            return NG;
        }


    }
}
