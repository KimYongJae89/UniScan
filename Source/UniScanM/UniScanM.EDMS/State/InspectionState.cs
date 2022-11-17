using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanM.Algorithm;
using UniScanM.Data;
using DynMvp.InspData;
using UniEye.Base;
using UniEye.Base.Inspect;
using DynMvp.Base;
using UniScanM.State;
using UniScanM.EDMS.Data;
using UniScanM.EDMS.Settings;
using System.Diagnostics;

namespace UniScanM.EDMS.State
{
    public class InspectionState : UniScanState
    {
        EdgePositionFinder edgePositionFinder;

        double[] prevEdgePosition = new double[3];
        bool firstResult = false;
        public bool FirstResult
        {
            get { return firstResult; }
            set { firstResult = value; }
        }

        bool inSkipLength = true;
        int firstResultCount = 0;
        double firstFilmEdge = 0.0;
        double firstPrintingEdge = 0.0;
        List<double> firstFileEdgeList = new List<double>();
        List<double> firstPrintingEdgeList = new List<double>();

        public override bool IsTeachState
        {
            get { return false; }
        }

        public InspectionState() : base()
        {
            firstResult = false;

            edgePositionFinder = new EdgePositionFinder();
        }

        protected override void Init()
        {
            firstResult = false;
            firstResultCount = 0;
            firstFilmEdge = 0.0;
            firstPrintingEdge = 0.0;
            firstFileEdgeList.Clear();
            firstPrintingEdgeList.Clear();
        }

        public override void PreProcess()
        {
            //firstResult = false;
        }


        public override void OnProcess(ImageD imageD, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            LogHelper.Debug(LoggerType.Operation, "MonitoringState::OnProcess");

            Data.InspectionResult inspectionResult2 = (Data.InspectionResult)inspectionResult;

            if (inspectionResult2.ResetZeroing)
            {
                firstResult = false;
                firstResultCount = 0;
                firstFileEdgeList.Clear();
                firstPrintingEdgeList.Clear();
            }

            double[] thresholdArray = new double[3];
            thresholdArray[0] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.FilmThreshold;
            thresholdArray[1] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.CoatingThreshold;
            thresholdArray[2] = ((Data.Model)SystemManager.Instance().CurrentModel).InspectParam.PrintingThreshold;

            int refSize = 0;
            if (EDMSSettings.Instance().ZeroingCount <= firstFileEdgeList.Count)
                refSize = (int)(firstPrintingEdgeList.Average() - firstFileEdgeList.Average());

            AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);
            Debug.Assert(algoImage != null);

            double[] position = this.edgePositionFinder.SheetEdgePosition(algoImage, thresholdArray, SearchDireciton.LeftToRight, refSize);
            if (EDMSSettings.Instance().SheetOnlyMode)
                Array.Clear(position, 1, position.Length - 1);

            int startPos = SystemManager.Instance().ProductionManager.CurProduction.LastStartPosition;
            int curPos = inspectionResult2.RollDistance;
            int skipRemain = (startPos + EDMSSettings.Instance().SkipLength) - curPos;
            if (inSkipLength)
            {
                inSkipLength = skipRemain > 0;
                if (inspectionResult2.ResetZeroing)
                    inSkipLength = false;
            }

            StartMode startMode = SystemManager.Instance().InspectStarter.StartMode;
            if (startMode == StartMode.Auto && inSkipLength)
            {
                // 스킵 단계
                inspectionResult2.Judgment = Judgment.Skip;
                inspectionResult2.State = State_EDMS.Waiting;
                inspectionResult2.RemainWaitDist = skipRemain;
            }
            else if (firstResult == false)
            {
                // 제로링 단계
                inspectionResult2.Judgment = Judgment.Skip;
                inspectionResult2.State = State_EDMS.Zeroing;
                ZeroSetting(position);
            }
            else if (curPos < startPos)
            {
                // 시작위치보다 현재위치가 이전인 경우 - ????
                inspectionResult2.Judgment = Judgment.Skip;
            }
            else
            {
                // 측정 단계
                inspectionResult2.State = State_EDMS.Inspecting;
                bool ok = (EDMSSettings.Instance().SheetOnlyMode && position[0] > 0)
                    || (!EDMSSettings.Instance().SheetOnlyMode && Array.TrueForAll(position, f => f > 0));
                if (ok == false)
                {
                    inspectionResult2.Judgment = Judgment.Skip;
                }
                else
                {
                    inspectionResult2.FirstFilmEdge = firstFilmEdge;
                    inspectionResult2.FirstPrintingEdge = firstPrintingEdge;
                }
            }

            double pelWidth = 5;// (SystemManager.Instance().DeviceBox.CameraCalibrationList.Count != 0) ? SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width : 5.0;

            ImageD displayImage = algoImage.ToImageD();
            
            if (EDMSSettings.Instance().IsFrontPosition)
                displayImage = algoImage.ToImageD().FlipX();

            inspectionResult2.ZeroingNum = firstResultCount;
            inspectionResult2.EdgePositionResult = position;
            inspectionResult2.AddEdgePositionResult(position, pelWidth);
            inspectionResult2.DisplayBitmap = displayImage.ToBitmap();

            algoImage.Dispose();
            //inspectionResult2.UpdateJudgement();
            //if (inspectionResult2.IsZeroingState == false && inspectionResult2.Judgment != Judgment.Skip)
            //{
            //    EDMS.Settings.Setting setting = ((EDMSSettings)AdditionalSettings.Instance()).Setting;
            //    if (Math.Abs(inspectionResult2.TotalEdgePositionResult[(int)DataType.Printing_FilmEdge_0]) > Math.Abs(setting.LineStop))
            //        inspectionResult2.Judgment = Judgment.Reject;
            //    else if (Math.Abs(inspectionResult2.TotalEdgePositionResult[(int)DataType.Printing_FilmEdge_0]) > Math.Abs(setting.LineWarning))
            //        inspectionResult2.Judgment = Judgment.Warn;
            //}
        }

        public void ZeroSetting(double[] position)
        {
            bool sheetOnlyMode = EDMSSettings.Instance().SheetOnlyMode;
            bool ok = (sheetOnlyMode && position[0] > 0)
                || (!sheetOnlyMode && position[0] > 0 && position[1] > 0 && position[2] > 0);

            if (ok)
            {
                firstFileEdgeList.Add(position[0]);
                firstPrintingEdgeList.Add(position[2]);

                if (firstResultCount == EDMSSettings.Instance().ZeroingCount)
                {
                    firstFilmEdge = firstFileEdgeList.Average();
                    firstPrintingEdge = firstPrintingEdgeList.Average();
                    firstResult = true;
                    return;
                }

                firstResultCount++;
                return;
            }
        }

        public override void PostProcess(DynMvp.InspData.InspectionResult inspectionResult)
        {
        }

        public override UniScanState GetNextState(DynMvp.InspData.InspectionResult inspectionResult)
        {
            return this;
        }

    }
}
