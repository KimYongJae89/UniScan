using System.Collections.Generic;
using System.Linq;
using DynMvp.InspData;
using UniEye.Base.Inspect;
using DynMvp.Base;
using UniScanM.EDMS.Settings;
using UniScanM.EDMS.Algorithm;


namespace UniScanM.EDMS.State
{

    public interface IState
    {
        IState Handling(DynMvp.InspData.InspectionResult inspectionResult);
        void Enter();
    }

    public class StateZeroing : IState
    {
        double[] prevEdgePosition = new double[3];

        int firstResultCount = 0;
        double biasFilmEdgePix = 0.0;
        double biasPrintingEdgePix = 0.0;
        List<double> firstFilmEdgeList = new List<double>();
        List<double> firstPrintingEdgeList = new List<double>();
   
        public StateZeroing() : base()
        {
            Init();
        }

        protected void Init()
        {
            firstResultCount = 0;
            biasFilmEdgePix = 0.0;
            biasPrintingEdgePix = 0.0;
            firstFilmEdgeList.Clear();
            firstPrintingEdgeList.Clear();
        }

        void IState.Enter()
        {
            Init();
        }

        IState IState.Handling(DynMvp.InspData.InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Operation, "StateZeroing::OnProcess");

            Data.InspectionResult edmsResult = (Data.InspectionResult)inspectionResult;
            float[] profile = edmsResult.Profile;

            double[] thresholdArray = new double[3];
            thresholdArray[0] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.FilmThreshold;
            thresholdArray[1] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.CoatingThreshold;
            thresholdArray[2] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.PrintingThreshold;

             double[] position = EdgeFinderE.SheetEdgePosition(profile, thresholdArray, SearchDireciton.LeftToRight, 0);
           // if (EDMSSettings.Instance().SheetOnlyMode)        Array.Clear(position, 1, position.Length - 1);


            StartMode startMode = SystemManager.Instance().InspectStarter.StartMode;

            edmsResult.Judgment = Judgment.Skip;
            edmsResult.State = Data.State_EDMS.Zeroing;

            AddZeroSetData(position); //여기서 에러처리...//todo 중간에 짤리면 처음부터..

            double pelWidth = 5;// (SystemManager.Instance().DeviceBox.CameraCalibrationList.Count != 0) ? SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width : 5.0;


            //double[] position = new double[3] { 100, 500, 1000 };

            edmsResult.ZeroingNum = firstResultCount;
            edmsResult.EdgePositionResult = position;
            edmsResult.AddEdgePositionResult(position, pelWidth);
            
            if (firstResultCount >= EDMSSettings.Instance().ZeroingCount)
            {
                return new StateInspection(biasFilmEdgePix, biasPrintingEdgePix);
            }
            return null;
        }
        public void AddZeroSetData(double[] position)
        {
            double[] thresholdArray = new double[3];
            thresholdArray[0] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.FilmThreshold;
            thresholdArray[1] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.CoatingThreshold;
            thresholdArray[2] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.PrintingThreshold;

            bool sheetOnlyMode = EDMSSettings.Instance().SheetOnlyMode;
            int ok = 0;
            if (sheetOnlyMode) //Find only File Edge
            {
                if (position[0] > 0) ok++;
            }
            else //normal mode
            { //문턱값이 0보다 크면 반드시 포지션값이 있어야하고, 문턱값이 0이하면 포지션값은 없어도(-1 이여도) 상관없음..
                if ((position[0] > 0 == thresholdArray[0] > 0) &&
                    (position[1] > 0 == thresholdArray[1] > 0) &&
                    (position[2] > 0 == thresholdArray[2] > 0)
                    ) ok++;
            }

            if (ok >0)
            {
                firstResultCount++;
                firstFilmEdgeList.Add(position[0]);
                firstPrintingEdgeList.Add(position[2]);

                if (firstResultCount == EDMSSettings.Instance().ZeroingCount) //todo median 필터좀 넣든가..
                {
                    biasFilmEdgePix = firstFilmEdgeList.Average();
                    biasPrintingEdgePix = firstPrintingEdgeList.Average();
                    return;
                }
                return;
            }
            else//todo 중간에 짤리면 ? 초기화
            {

            }
        }

    }
}
