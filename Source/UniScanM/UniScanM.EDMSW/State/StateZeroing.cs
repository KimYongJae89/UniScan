using System.Collections.Generic;
using System.Linq;
using DynMvp.InspData;
using UniEye.Base.Inspect;
using DynMvp.Base;
using UniScanM.EDMSW.Settings;
using UniScanM.EDMSW.Algorithm;


namespace UniScanM.EDMSW.State
{

    public interface IState
    {
        IState Handling(DynMvp.InspData.InspectionResult inspectionResult);
        void Enter();
    }

    public class StateZeroing : IState
    {
        double[] prevEdgePosition = new double[3];


        //Left
        int firstResultCountLeft = 0;
        double biasFilmEdgePixLeft = 0.0;
        double biasPrintingEdgePixLeft = 0.0;
        List<double> firstFilmEdgeListLeft = new List<double>();
        List<double> firstPrintingEdgeListLeft = new List<double>();
        //Rgiht
        int firstResultCountRight = 0;
        double biasFilmEdgePixRight = 0.0;
        double biasPrintingEdgePixRight = 0.0;
        List<double> firstFilmEdgeListRight = new List<double>();
        List<double> firstPrintingEdgeListRight = new List<double>();
        /// <summary>
        /// /
        /// </summary>

        public StateZeroing() : base()
        {
            Init();
        }

        protected void Init()
        {
            firstResultCountLeft = 0;
            biasFilmEdgePixLeft = 0.0;
            biasPrintingEdgePixLeft = 0.0;
            firstFilmEdgeListLeft.Clear();
            firstPrintingEdgeListLeft.Clear();

            firstResultCountRight = 0;
            biasFilmEdgePixRight = 0.0;
            biasPrintingEdgePixRight = 0.0;
            firstFilmEdgeListRight.Clear();
            firstPrintingEdgeListRight.Clear();
        }

        void IState.Enter()
        {
            Init();
        }

        IState IState.Handling(DynMvp.InspData.InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Operation, "StateZeroing::OnProcess");
            StartMode startMode = SystemManager.Instance().InspectStarter.StartMode;

            Data.InspectionResult edmsResult = (Data.InspectionResult)inspectionResult;
            edmsResult.Judgment = Judgment.Skip;
            edmsResult.State = Data.State_EDMS.Zeroing;

            double pelWidth = 5;// (SystemManager.Instance().DeviceBox.CameraCalibrationList.Count != 0) ? SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width : 5.0;

            double[] thresholdArray = new double[3];
            thresholdArray[0] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.FilmThreshold;
            thresholdArray[1] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.CoatingThreshold;
            thresholdArray[2] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.PrintingThreshold;

            float[] profileLeft = edmsResult.ProfileLeftHor;
            double[] positionLeft = EdgeFinderE.SheetEdgePosition(profileLeft, thresholdArray, SearchDireciton.LeftToRight, 0);
            AddZeroSetDataLeft(positionLeft); //여기서 에러처리...//todo 중간에 짤리면 처음부터..
            edmsResult.EdgePositionResultLeft = positionLeft;
            edmsResult.AddEdgePositionResultLeft(positionLeft, pelWidth);

            float[] profileRight = edmsResult.ProfileRightHor;
            double[] positionRight = EdgeFinderE.SheetEdgePosition(profileRight, thresholdArray, SearchDireciton.RightToLeft, 0);
            AddZeroSetDataRight(positionRight); //여기서 에러처리...//todo 중간에 짤리면 처음부터..
            edmsResult.EdgePositionResultRight = positionRight;
            edmsResult.AddEdgePositionResultRight(positionRight, pelWidth);

            //double[] position = new double[3] { 100, 500, 1000 };
            edmsResult.ZeroingNum = firstResultCountLeft;
            
            if ((firstResultCountLeft >= EDMSSettings.Instance().ZeroingCount) &&
                (firstResultCountRight >= EDMSSettings.Instance().ZeroingCount) )
            {
                return new StateInspection(biasFilmEdgePixLeft, biasPrintingEdgePixLeft,
                                            biasFilmEdgePixRight, biasPrintingEdgePixRight);
            }
            return null;
        }

        public void AddZeroSetDataLeft(double[] position)
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
                firstResultCountLeft++;
                firstFilmEdgeListLeft.Add(position[0]);
                firstPrintingEdgeListLeft.Add(position[2]);

                if (firstResultCountLeft == EDMSSettings.Instance().ZeroingCount) //todo median 필터좀 넣든가..
                {
                    biasFilmEdgePixLeft = firstFilmEdgeListLeft.Average();
                    biasPrintingEdgePixLeft = firstPrintingEdgeListLeft.Average();
                    return;
                }
                return;
            }
            else//todo 중간에 짤리면 ? 초기화
            {

            }
        }
        public void AddZeroSetDataRight(double[] position)
        {
            double[] thresholdArray = new double[3];
            thresholdArray[0] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.FilmThreshold;
            thresholdArray[1] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.CoatingThreshold;
            thresholdArray[2] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.PrintingThreshold;

            bool sheetOnlyMode = EDMSSettings.Instance().SheetOnlyMode;
            int ok = 0;
            if (sheetOnlyMode) //Find only Film Edge
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

            if (ok > 0)
            {
                firstResultCountRight++;
                firstFilmEdgeListRight.Add(position[0]);
                firstPrintingEdgeListRight.Add(position[2]);

                if (firstResultCountRight == EDMSSettings.Instance().ZeroingCount) //todo median 필터좀 넣든가..
                {
                    biasFilmEdgePixRight = firstFilmEdgeListRight.Average();
                    biasPrintingEdgePixRight = firstPrintingEdgeListRight.Average();
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
