using System.Collections.Generic;
using DynMvp.InspData;
using UniEye.Base.Inspect;
using DynMvp.Base;
using UniScanM.EDMSW.Settings;
using UniScanM.EDMSW.Algorithm;


namespace UniScanM.EDMSW.State
{
    public class StateSkip : IState
    {
        public void Enter()
        {
 
        }
        public IState Handling(DynMvp.InspData.InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Operation, "StateSkip::Handling(...)");
            Data.InspectionResult edmsResult = (Data.InspectionResult)inspectionResult;

            //float[] profile = edmsResult.Profile;

            double[] position = new double[3] { 0, 0, 0 };

            //if (EDMSSettings.Instance().SheetOnlyMode)          Array.Clear(position, 1, position.Length - 1);

            int startPos = SystemManager.Instance().ProductionManager.CurProduction.LastStartPosition;
            int curPos = edmsResult.RollDistance;
            int skipRemain = (startPos + EDMSSettings.Instance().SkipLength) - curPos;

            StartMode startMode = SystemManager.Instance().InspectStarter.StartMode;
            //if (startMode == StartMode.Auto && inSkipLength)
            {
                // 스킵 단계
                edmsResult.Judgment = Judgment.Skip;
                edmsResult.State = Data.State_EDMS.Waiting;
                edmsResult.RemainWaitDist = skipRemain;

            }

            double pelWidth = 5;// (SystemManager.Instance().DeviceBox.CameraCalibrationList.Count != 0) ? SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width : 5.0;

            edmsResult.ZeroingNum = 0;
            edmsResult.EdgePositionResultLeft = position;
            edmsResult.AddEdgePositionResultLeft(position, pelWidth);

            if (skipRemain <= 0) // past skiplength
            {
                return new StateZeroing();
            }
            return null;
        }
    }
}
