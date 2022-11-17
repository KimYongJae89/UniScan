using System;
using System.Linq;
using DynMvp.InspData;
using UniEye.Base.Inspect;
using DynMvp.Base;
using UniScanM.EDMS.Settings;
using UniScanM.EDMS.Algorithm;
using UniScanM.Operation;
using UniScanM.EDMS.Data;
using System.Collections.Generic;

namespace UniScanM.EDMS.State
{
    public class StateInspection : IState
    {
        double[] prevEdgePosition = new double[3];
        double biasFilmEdgePix = 0.0;
        double biasPrintingEdgePix = 0.0;
        List<Data.InspectionResult> movingAvgQue = new List<Data.InspectionResult>();

        List<double> recentDataQueT103 = new List<double>();  //after moving average 
        List<double> recentDataQueT105 = new List<double>();  //after moving average 

        public StateInspection(double BiasFilmEdgePix, double BiasPrintingEdgePix) : base()
        {
            biasFilmEdgePix = BiasFilmEdgePix;
            biasPrintingEdgePix = BiasPrintingEdgePix;
        }

        protected void Init()
        {
            biasFilmEdgePix = 0.0;
            biasPrintingEdgePix = 0.0;
            movingAvgQue.Clear();
            recentDataQueT103.Clear();
            recentDataQueT105.Clear();
        }

        void IState.Enter()
        {
            //bool senceing;
            //if (biasFilmEdgePix != 0.0)
            //{
            //    senceing = true;
            //}
            //else
            //{
            //    senceing = false;
            //}
        }

        IState IState.Handling(DynMvp.InspData.InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Operation, "StateInspection::Handling()");
            Data.InspectionResult edmsResult = (Data.InspectionResult)inspectionResult;
            float[] profile = edmsResult.Profile;

            double[] thresholdArray = new double[3];
            thresholdArray[0] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.FilmThreshold;
            thresholdArray[1] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.CoatingThreshold;
            thresholdArray[2] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.PrintingThreshold;

            int refSize = (int)(biasPrintingEdgePix - biasFilmEdgePix);

            if (OperationOption.Instance().OnTune == true)
                refSize = 0;

            double[] edgePos = EdgeFinderE.SheetEdgePosition(profile, thresholdArray, SearchDireciton.LeftToRight, refSize);

            StartMode startMode = SystemManager.Instance().InspectStarter.StartMode;
            //if (curPos < startPos)            {
            //    // 시작위치보다 현재위치가 이전인 경우 - ????
            //    inspectionResult2.Judgment = Judgment.Skip;  }
            {
                // 측정 단계
                edmsResult.State = Data.State_EDMS.Inspecting;
                bool ok = (Array.TrueForAll(edgePos, f => f > 0)) || (EDMSSettings.Instance().SheetOnlyMode && edgePos[0] > 0);
                //문턱값이 0보다 크면 반드시 포지션값이 있어야하고, 문턱값이 0이하면 포지션값은 없어도(-1 이여도) 상관없음..
                if ((edgePos[0] > 0 == thresholdArray[0] > 0) &&
                    (edgePos[1] > 0 == thresholdArray[1] > 0) &&
                    (edgePos[2] > 0 == thresholdArray[2] > 0)
                    ) ok=true;

                if (ok == false)
                {
                    edmsResult.Judgment = Judgment.Skip;
                }

            }
            double pelWidth = 5;// (SystemManager.Instance().DeviceBox.CameraCalibrationList.Count != 0) ? SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width : 5.0;

            edmsResult.FirstFilmEdge = biasFilmEdgePix;
            edmsResult.FirstPrintingEdge = biasPrintingEdgePix;

            edmsResult.EdgePositionResult = edgePos;
            edmsResult.AddEdgePositionResult(edgePos, pelWidth);

            //moving average ////////////////////////////////////////////////////////////////
            Settings.EDMSSettings edmsSetting = Settings.EDMSSettings.Instance();
            if (edmsSetting.UseMovingAvgLine && edmsSetting.MovingAvgPeriod > 1)
            {
                if (edmsResult.Judgment != Judgment.Skip)//when normal data
                {
                    movingAvgQue.Add(edmsResult);
                    while (movingAvgQue.Count > edmsSetting.MovingAvgPeriod)
                        movingAvgQue.RemoveAt(0);

                    if (movingAvgQue.Count == edmsSetting.MovingAvgPeriod)
                    {
                        SetMovingAvgPosition(movingAvgQue, edmsResult);
                    }
                    else edmsResult.Judgment = Judgment.Skip;
                }
            }

            if (edmsResult.Judgment != Judgment.Skip)
            {
                //recent data T103 store
                if (edmsSetting.T103AlarmRecentOutEnable && edmsSetting.T103RecentDataCount > 0)
                {
                    recentDataQueT103.Add(edmsResult.TotalEdgePositionResult[(int)DataType.FilmEdge_0]);

                    while (recentDataQueT103.Count > edmsSetting.T103RecentDataCount  ) 
                        recentDataQueT103.RemoveAt(0);

                    if (recentDataQueT103.Count == edmsSetting.T103RecentDataCount )
                    {
                         var list = recentDataQueT103.GetRange(0, (int)edmsSetting.T103RecentDataCount);
                        edmsResult.RecentFilmEdgeDiff = list.Max()-list.Min();
                    }
                }

                //recent data T105 store
                if (edmsSetting.T105AlarmRecentOutEnable && edmsSetting.T105RecentDataCount > 0)
                {
                    recentDataQueT105.Add(edmsResult.TotalEdgePositionResult[(int)DataType.Printing_FilmEdge_0]);
                    while (recentDataQueT105.Count > edmsSetting.T105RecentDataCount )
                        recentDataQueT105.RemoveAt(0);
                    if (recentDataQueT105.Count == edmsSetting.T105RecentDataCount )
                    {
                        var list = recentDataQueT105.GetRange(0, (int)edmsSetting.T105RecentDataCount);
                        edmsResult.RecentPrintingEdgeDiff = list.Max() - list.Min();
                    }
                }
            }

            //judge
            edmsResult.UpdateJudgement();

            //if no data has happend long time.. change to zeroingstate
            return null;
        }


        private void SetMovingAvgPosition(List<Data.InspectionResult> QueData, Data.InspectionResult edmsResult)
        {
            float sumPosition = 0.0f;
            for (int index = 0; index < edmsResult.TotalEdgePositionResult.Length; index++)
            {
                sumPosition = 0.0f;
                foreach (var result in QueData)
                    sumPosition += (float)result.TotalEdgePositionResult[index];

                edmsResult.TotalEdgePositionResult[index] = sumPosition / QueData.Count;
            }
        }
    }
}
