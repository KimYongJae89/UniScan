using System;
using System.Linq;
using DynMvp.InspData;
using UniEye.Base.Inspect;
using DynMvp.Base;
using UniScanM.EDMSW.Settings;
using UniScanM.EDMSW.Algorithm;
using UniScanM.Operation;
using UniScanM.EDMSW.Data;
using System.Collections.Generic;

namespace UniScanM.EDMSW.State
{
    public class StateInspection : IState
    {
        double[] prevEdgePosition = new double[3];
        double m_RollerDiameter = 140;

        double biasFilmEdgePixLeft = 0.0;
        double biasPrintingEdgePixLeft = 0.0;
        double biasFilmEdgePixRight = 0.0;
        double biasPrintingEdgePixRight = 0.0;

        List<Data.InspectionResult> movingAvgQue = new List<Data.InspectionResult>();

        List<double> recentDataQueT103Left = new List<double>();  
        List<double> recentDataQueT105Left = new List<double>();  

        List<double> recentDataQueT103Right = new List<double>();  
        List<double> recentDataQueT105Right = new List<double>();  

        public StateInspection(double BiasFilmEdgePixLeft, double BiasPrintingEdgePixLeft,
            double BiasFilmEdgePixRight, double BiasPrintingEdgePixRight) : base()
        {
            biasFilmEdgePixLeft = BiasFilmEdgePixLeft;
            biasPrintingEdgePixLeft = BiasPrintingEdgePixLeft;

            biasFilmEdgePixRight = BiasFilmEdgePixRight;
            biasPrintingEdgePixRight = BiasPrintingEdgePixRight;

            m_RollerDiameter = (float)SystemManager.Instance().InspectStarter.GetRollerDia();
        }

        protected void Init()
        {
            biasFilmEdgePixLeft = 0.0;
            biasPrintingEdgePixLeft = 0.0;
            biasFilmEdgePixRight = 0;
            biasPrintingEdgePixRight = 0;

            movingAvgQue.Clear();

            recentDataQueT103Left.Clear();
            recentDataQueT105Left.Clear();
            recentDataQueT103Right.Clear();
            recentDataQueT105Right.Clear();
        }

        void IState.Enter()
        {

            double m_RollerDiameter = SystemManager.Instance().InspectStarter.GetRollerDia();
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

            StartMode startMode = SystemManager.Instance().InspectStarter.StartMode;

            Data.InspectionResult edmsResult = (Data.InspectionResult)inspectionResult;
            float[] profileLeft = edmsResult.ProfileLeftHor;
            float[] profileRight = edmsResult.ProfileRightHor;

            double[] thresholdArray = new double[3];
            thresholdArray[0] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.FilmThreshold;
            thresholdArray[1] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.CoatingThreshold;
            thresholdArray[2] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.PrintingThreshold;

            int refSizeLeft = (int)(biasPrintingEdgePixLeft - biasFilmEdgePixLeft);
            int refSizeRight = (int)(biasPrintingEdgePixRight - biasFilmEdgePixRight);

            if (OperationOption.Instance().OnTune == true)
            {
                refSizeLeft = 0;
                refSizeRight = 0;
            }
                

            double[] edgePosLeft = EdgeFinderE.SheetEdgePosition(profileLeft, thresholdArray, SearchDireciton.LeftToRight, refSizeLeft);
            double[] edgePosRight = EdgeFinderE.SheetEdgePosition(profileRight, thresholdArray, SearchDireciton.RightToLeft, refSizeRight);

            //var LengthEdgeLeft = EdgeFinderE.FindBlackEdges(edmsResult.ProfileLeftVer, mergeRadius, removeSize);
            //var LengthEdgeRight = EdgeFinderE.FindBlackEdges(edmsResult.ProfileRightVer, mergeRadius, removeSize);
            int mergeSize = 2000; //2000*5um = 10.0mm
            int removeSize = 200;   //200*5um  = 1.0mm
            var LengthEdgeLeft = EdgeFinderE.FindWhiteEdges(edmsResult.ProfileLeftVer, mergeSize, removeSize);
            var LengthEdgeRight = EdgeFinderE.FindWhiteEdges(edmsResult.ProfileRightVer, mergeSize, removeSize);

            //if (curPos < startPos){
            // 시작위치보다 현재위치가 이전인 경우 - ????

            // 문턱값과 데이터의 유효성 확인 >> 유효하지 않으면 Judgment.Skip 처리
            {
                // 측정 단계
                edmsResult.State = Data.State_EDMS.Inspecting;

                bool okLeft = (Array.TrueForAll(edgePosLeft, f => f > 0)) || (EDMSSettings.Instance().SheetOnlyMode && edgePosLeft[0] > 0);
                //문턱값이 0보다 크면 반드시 포지션값이 있어야하고, 문턱값이 0이하면 포지션값은 없어도(-1 이여도) 상관없음..
                if ((edgePosLeft[0] > 0 == thresholdArray[0] > 0) &&
                    (edgePosLeft[1] > 0 == thresholdArray[1] > 0) &&
                    (edgePosLeft[2] > 0 == thresholdArray[2] > 0)
                    ) okLeft = true;

                bool okRight = (Array.TrueForAll(edgePosRight, f => f > 0)) || (EDMSSettings.Instance().SheetOnlyMode && edgePosRight[0] > 0);
                //문턱값이 0보다 크면 반드시 포지션값이 있어야하고, 문턱값이 0이하면 포지션값은 없어도(-1 이여도) 상관없음..
                if ((edgePosRight[0] > 0 == thresholdArray[0] > 0) &&
                    (edgePosRight[1] > 0 == thresholdArray[1] > 0) &&
                    (edgePosRight[2] > 0 == thresholdArray[2] > 0)
                    ) okRight = true;

                if (okLeft == false || okRight ==false)
                {
                    edmsResult.Judgment = Judgment.Skip;
                }

            }

            // Result 에 값 써넣기.
            {
                double pelWidth = 5;// (SystemManager.Instance().DeviceBox.CameraCalibrationList.Count != 0) ? SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width : 5.0;

                edmsResult.FirstFilmEdgeLeft = biasFilmEdgePixLeft;
                edmsResult.FirstPrintingEdgeLeft = biasPrintingEdgePixLeft;
                edmsResult.EdgePositionResultLeft = edgePosLeft;
                edmsResult.AddEdgePositionResultLeft(edgePosLeft, pelWidth);

                edmsResult.FirstFilmEdgeRight = biasFilmEdgePixRight;
                edmsResult.FirstPrintingEdgeRight = biasPrintingEdgePixRight;
                edmsResult.EdgePositionResultRight = edgePosRight;
                edmsResult.AddEdgePositionResultRight(edgePosRight, pelWidth);

                float OffsetWidth = EDMSSettings.Instance().Calibration440;
                //if() 모드가 440이면, OffsetWidth = EDMSSettings.Instance().Calibration440;
                if (m_RollerDiameter > 130) //330이면 105파이, 440이면 140파이 정도 이므로 그중간 122이로 나눔
                    OffsetWidth = EDMSSettings.Instance().Calibration440;
                else OffsetWidth = EDMSSettings.Instance().Calibration330;

                edmsResult.LengthDataPix = new double[6] { 0, 0, 0, 0, 0, 0 };
                edmsResult.LengthDataPix[(int)DataType_Length.W100] = edgePosLeft[0] + edgePosRight[0];
                edmsResult.LengthDataPix[(int)DataType_Length.W101] = edgePosLeft[1] + edgePosRight[1];
                edmsResult.LengthDataPix[(int)DataType_Length.W102] = edgePosLeft[2] + edgePosRight[2];
                edmsResult.LengthDataPix[(int)DataType_Length.L100] = edgePosLeft[0] + edgePosRight[0];
                edmsResult.LengthDataPix[(int)DataType_Length.L200] = edgePosLeft[1] + edgePosRight[1];
                edmsResult.LengthDataPix[(int)DataType_Length.LDIFF] = edgePosLeft[2] + edgePosRight[2];

                edmsResult.TotalLengthData = new double[6] { 0, 0, 0, 0, 0, 0 };
                edmsResult.TotalLengthData[(int)DataType_Length.W100] = OffsetWidth - (edgePosLeft[0] + edgePosRight[0]) * pelWidth / 1000;
                edmsResult.TotalLengthData[(int)DataType_Length.W101] = OffsetWidth - (edgePosLeft[1] + edgePosRight[1]) * pelWidth / 1000;
                edmsResult.TotalLengthData[(int)DataType_Length.W102] = OffsetWidth - (edgePosLeft[2] + edgePosRight[2]) * pelWidth / 1000;


                if (LengthEdgeLeft != null && LengthEdgeRight != null)
                {
                    //const byte BEGIN_EDGE = 100;  //falling 인쇄시작
                    //const byte END_EDGE = 200;     //rising 인쇄 끝

                    ////left length
                    //var begin = LengthEdgeLeft.Find(f => f.X == BEGIN_EDGE); //시작
                    //var end = LengthEdgeLeft.Find(f => f.X == END_EDGE && f.Y > begin.Y); //끝
                    //if (begin != null && end != null && begin.Y < end.Y)
                    //    edmsResult.TotalLengthData[(int)DataType_Length.L100] = (end.Y - begin.Y) * pelWidth * 2 / 1000;
                    //else
                    //    edmsResult.TotalLengthData[(int)DataType_Length.L100] = 0;

                    ////right lenth
                    //begin = LengthEdgeRight.Find(f => f.X == BEGIN_EDGE); //시작
                    //end = LengthEdgeRight.Find(f => f.X == END_EDGE && f.Y > begin.Y); //끝

                    //if (begin != null && end != null && begin.Y < end.Y)
                    //    edmsResult.TotalLengthData[(int)DataType_Length.L200] = (end.Y - begin.Y) * pelWidth * 2 / 1000;
                    //else
                    //    edmsResult.TotalLengthData[(int)DataType_Length.L200] = 0;

                    //edmsResult.TotalLengthData[(int)DataType_Length.LDIFF] =
                    //   (edmsResult.TotalLengthData[(int)DataType_Length.L100] - edmsResult.TotalLengthData[(int)DataType_Length.L200]) * 1000;
                }

                if (LengthEdgeLeft != null && LengthEdgeLeft.Count >= 2 &&
                    LengthEdgeRight != null && LengthEdgeRight.Count >=2      )
                {
                    //left length
                    var begin = LengthEdgeLeft[0]; //시작
                    var end = LengthEdgeLeft[1]; //끝
                    double Llength = (end.X - begin.Y) * pelWidth * 2 / 1000;
                    edmsResult.TotalLengthData[(int)DataType_Length.L100] = Llength;

                    //right length
                    begin = LengthEdgeRight[0]; //시작
                    end = LengthEdgeRight[1]; //끝
                    double Rlength = (end.X - begin.Y) * pelWidth * 2 / 1000;
                    edmsResult.TotalLengthData[(int)DataType_Length.L200] = Rlength;

                    //diff
                    edmsResult.TotalLengthData[(int)DataType_Length.LDIFF] = Llength - Rlength;
                }

            }

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

            //recent data -----------------------------------------------------------------------------------//
            if (edmsResult.Judgment != Judgment.Skip)
            {
                //left recent data T103 store
                if (edmsSetting.T103AlarmRecentOutEnable && edmsSetting.T103RecentDataCount > 0)
                {
                    recentDataQueT103Left.Add(edmsResult.TotalEdgePositionResultLeft[(int)DataType.FilmEdge_0]);

                    while (recentDataQueT103Left.Count > edmsSetting.T103RecentDataCount  ) 
                        recentDataQueT103Left.RemoveAt(0);

                    if (recentDataQueT103Left.Count == edmsSetting.T103RecentDataCount )
                    {
                         var list = recentDataQueT103Left.GetRange(0, (int)edmsSetting.T103RecentDataCount);
                        edmsResult.RecentFilmEdgeDiffLeft = list.Max()-list.Min();
                    }
                }

                //left recent data T105 store
                if (edmsSetting.T105AlarmRecentOutEnable && edmsSetting.T105RecentDataCount > 0)
                {
                    recentDataQueT105Left.Add(edmsResult.TotalEdgePositionResultLeft[(int)DataType.Printing_FilmEdge_0]);
                    while (recentDataQueT105Left.Count > edmsSetting.T105RecentDataCount )
                        recentDataQueT105Left.RemoveAt(0);

                    if (recentDataQueT105Left.Count == edmsSetting.T105RecentDataCount )
                    {
                        var list = recentDataQueT105Left.GetRange(0, (int)edmsSetting.T105RecentDataCount);
                        edmsResult.RecentPrintingEdgeDiffLeft = list.Max() - list.Min();
                    }
                }

                //right recent data T103 store
                if (edmsSetting.T103AlarmRecentOutEnable && edmsSetting.T103RecentDataCount > 0)
                {
                    recentDataQueT103Right.Add(edmsResult.TotalEdgePositionResultRight[(int)DataType.FilmEdge_0]);

                    while (recentDataQueT103Right.Count > edmsSetting.T103RecentDataCount)
                        recentDataQueT103Right.RemoveAt(0);

                    if (recentDataQueT103Right.Count == edmsSetting.T103RecentDataCount)
                    {
                        var list = recentDataQueT103Right.GetRange(0, (int)edmsSetting.T103RecentDataCount);
                        edmsResult.RecentFilmEdgeDiffRight = list.Max() - list.Min();
                    }
                }

                //right recent data T105 store
                if (edmsSetting.T105AlarmRecentOutEnable && edmsSetting.T105RecentDataCount > 0)
                {
                    recentDataQueT105Right.Add(edmsResult.TotalEdgePositionResultRight[(int)DataType.Printing_FilmEdge_0]);
                    while (recentDataQueT105Right.Count > edmsSetting.T105RecentDataCount)
                        recentDataQueT105Right.RemoveAt(0);
                    if (recentDataQueT105Right.Count == edmsSetting.T105RecentDataCount)
                    {
                        var list = recentDataQueT105Right.GetRange(0, (int)edmsSetting.T105RecentDataCount);
                        edmsResult.RecentPrintingEdgeDiffRight = list.Max() - list.Min();
                    }
                }
            }

            //judge
            edmsResult.UpdateJudgement();

            //if no data has happend long time.. change to zeroing-state
            return null;
        }


        private void SetMovingAvgPosition(List<Data.InspectionResult> QueData, Data.InspectionResult edmsResult)
        {
            float sumPosition = 0.0f;
            for (int index = 0; index < edmsResult.TotalEdgePositionResultLeft.Length; index++)
            {
                //left
                sumPosition = 0.0f;
                foreach (var result in QueData)
                    sumPosition += (float)result.TotalEdgePositionResultLeft[index];

                edmsResult.TotalEdgePositionResultLeft[index] = sumPosition / QueData.Count;

                //right
                sumPosition = 0.0f;
                foreach (var result in QueData)
                    sumPosition += (float)result.TotalEdgePositionResultRight[index];
                edmsResult.TotalEdgePositionResultRight[index] = sumPosition / QueData.Count;
            }
        }
    }
}
