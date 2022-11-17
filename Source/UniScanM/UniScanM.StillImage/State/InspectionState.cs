using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
//using UniScanM.Algorithm;
//using UniScanM.Data;
//using DynMvp.InspData;
using UniEye.Base;
using UniEye.Base.Inspect;
using DynMvp.Base;
using System.Drawing;
using System.Diagnostics;
using UniScanM.StillImage.Algorithm;
using UniScanM.StillImage.Data;

namespace UniScanM.StillImage.State
{
    public class InspectionState : UniscanState
    {
        int repeatCountByZone = 0;

        FindingSheet findingSheet = null;
        Inspector inspector = null;

        Dictionary<AxisPosition, TeachData> teachDetaList;
        InspectParam inspectParam;

        /// <summary>
        /// 검사위치(1~6)별 티칭위치 MAP
        /// </summary>
        int[] inspSeqMap = null;

        /// <summary>
        /// 다음 검사위치까지 인덱스 변화량 (1: 정방향, -1: 역방향)
        /// </summary>
        int sequenceOrderIncStep = 1;

        /// <summary>
        /// 현재 검사위치
        /// </summary>
        int curSeqOrder = -1;

        /// <summary>
        /// 다음 검사위치
        /// </summary>
        int nextSeqOrder = -1;

        /// <summary>
        /// Operation Mode가 Random 일 때.
        /// </summary>
        Random random = null;

        public override bool IsTeachState
        {
            get { return false; }
        }

        public InspectionState(Dictionary<AxisPosition, TeachData> teachDetaList, InspectParam inspectParam) : base()
        {
            Func<KeyValuePair<AxisPosition, TeachData>, bool> func = new Func<KeyValuePair<AxisPosition, TeachData>, bool>(f => f.Value != null && f.Value.TeachDone && f.Value.IsInspectable);

            inspectState = UniEye.Base.Data.InspectState.Run;
            findingSheet = new FindingSheet();
            inspector = Inspector.Create(0);
            random = new Random();

            this.teachDetaList = teachDetaList;
            this.inspectParam = inspectParam;

            int realDataCount = teachDetaList.Count(func); //정상 티칭된 개수
            Debug.Assert(realDataCount > 0);
            int sequenceCount = Math.Min(realDataCount, 6);
            inspSeqMap = new int[sequenceCount];

            List<KeyValuePair<AxisPosition, TeachData>> list = teachDetaList.ToList();
            if (sequenceCount == 1)
            {
                inspSeqMap[0] = list.FindIndex(f=>func(f));
            }
            else
            {
                // tempMap: 티칭된 위치 인덱스와 실 위치간 Map
                int[] tempMap = new int[realDataCount];
                for (int i = 0; i < realDataCount; i++)
                {
                    int startFindIdx = i == 0 ? 0 :( tempMap[i - 1] + 1);
                    tempMap[i] = list.FindIndex(startFindIdx, f => f.Value != null && f.Value.TeachDone && f.Value.IsInspectable);
                    tempMap[i] += 0;    
                }

                // tempMpa2: 검사할 위치 인덱스와 티칭된 위치 인덱스간 Map
                int[] tempMap2 = new int[sequenceCount];
                float sequenceStep = (realDataCount- 1) / (float)(sequenceCount - 1);
                for (int i = 0; i < sequenceCount; i++)
                {
                    tempMap2[i] = (int)Math.Round(i * sequenceStep);
                    inspSeqMap[i] = tempMap[tempMap2[i]];
                }
            }

            curSeqOrder = nextSeqOrder = -1;
            sequenceOrderIncStep = 1;
                this.imageSequnece = inspSeqMap[0] + 1;// 0번은 LightTune 이미지, 1번부터 스캔 이미지.

            Initialize();
            //SystemManager.Instance().MainForm.SettingPage.UpdateControl("InspPos", inspSeqMap);
        }

        protected override void Init()
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectionState::Init");
            int absPosidx = inspSeqMap[0];
            AxisPosition position = this.teachDetaList.Keys.ElementAt(absPosidx);
            SystemManager.Instance().DeviceController.RobotStage.Move(position);
            curSeqOrder = 0;
            nextSeqOrder = 0;
        }

        public override void PreProcess()
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectionState::PreProcess");

              UniScanM.StillImage.Settings.StillImageSettings additionalSettings = AdditionalSettings.Instance() as UniScanM.StillImage.Settings.StillImageSettings;
            //Number of test repetitions by location
            repeatCountByZone++;
            if (repeatCountByZone >= additionalSettings.RepeatInspectbyZone)
            {
                repeatCountByZone = 0;

                nextSeqOrder = (curSeqOrder + sequenceOrderIncStep) % inspSeqMap.Count();

                // 좌/우 끝에 닿으면 이동방향 반대로
                if (nextSeqOrder == 0)
                    sequenceOrderIncStep = 1;
                else if (nextSeqOrder == inspSeqMap.Count() - 1)
                    sequenceOrderIncStep = -1;
            }
            //////////////////////////////////////////////////////////////////////////////////
            int nextInspectPositionIndex = inspSeqMap[nextSeqOrder];
            AxisPosition axisPosition = teachDetaList.Keys.ElementAt(nextInspectPositionIndex);

            AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            axisHandler.StartMove(axisPosition);
        }


        public override void OnProcess(ImageD imageD, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectionState::OnProcess");
            InspectionResult inspectionResult2 = (InspectionResult)inspectionResult;

            inspectionResult2.InspectState = "Inspection";
            inspectionResult2.InspZone = curSeqOrder;

            TeachData teachData = teachDetaList.Values.ElementAt(inspSeqMap[curSeqOrder]);

            Model model = SystemManager.Instance().CurrentModel as Model;
            Size inspSize = model == null ? Size.Empty : model.InspSize;
            if (inspSize.IsEmpty)
                inspSize = teachData.InspSize;
            if (inspSize.IsEmpty)
            {
                int size = Math.Min(imageD.Width, imageD.Height);
                inspSize = new Size(size, size);
            }

            inspectionResult2.TeachData = teachData;
            inspectionResult2.RollDistance = (int)SystemManager.Instance().InspectStarter.GetPosition();

            //AlgoImage sheetImage = findingSheet.GetSheetImage(algoImage, null);
            //sheetImage.Save(string.Format(@"d:\temp\SHEET_POS{0}_NO{1}.bmp", curSeqOrder, inspectionResult.InspectionNo));
            //sheetImage.Dispose();

            AlgoImage frameImage = null;
            AlgoImage inspImage = null;
            try
            {
                frameImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);
                Debug.Assert(frameImage != null);
                Debug.WriteLine("▣ **** Inspect ****..");
                inspImage = findingSheet.GetInspImage(frameImage, inspSize, inspectionResult);

                //inspImage.Save(string.Format(@"d:\temp\inspImage_POS{0}_NO{1}.bmp", curSeqOrder, inspectionResult.InspectionNo));
                if (inspImage != null)
                {
                    inspector.Inspect(inspImage, this.inspectParam, inspectionResult2);//■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
                    inspectionResult2.UpdateJudgement();
                    inspectionResult2.DisplayBitmap = inspImage.ToImageD().ToBitmap();

                    ProcessResult intersetProcessResult = inspectionResult2.ProcessResultList.InterestProcessResult;
                    //if (intersetProcessResult == null || intersetProcessResult.IsGood == false || inspectionResult2.ProcessResultList.DefectRectList.Count > 0)
                    //    inspectionResult2.SetDefect();

                    if (intersetProcessResult != null)
                    {
                        Rectangle blotMarginInspRect = intersetProcessResult.InspPatternInfo.ShapeInfo.BaseRect;
                        inspectionResult2.BlotRectInInsp = blotMarginInspRect;

                        Size marginSize = Size.Round(intersetProcessResult.InspPatternInfo.TeachInfo.Feature.Margin);
                        blotMarginInspRect.Inflate(marginSize);
                        inspectionResult2.MarginRectInInsp = blotMarginInspRect;
                    }
                    inspImage.Dispose();
                }
                else
                {
                    inspectionResult2.ProcessResultList = new ProcessResultList(null);
                    inspectionResult2.Judgment = DynMvp.InspData.Judgment.Skip;

                    AlgoImage displayImage = frameImage.GetSubImage(new Rectangle(Point.Empty, inspSize));
                    inspectionResult2.DisplayBitmap = displayImage.ToImageD().ToBitmap();
                    displayImage.Dispose();
                }
            }
            finally
            {
                frameImage?.Dispose();
                inspImage?.Dispose();
            }
  

            //inspectionResult2.AddExtraResult("InspectState", "Inspection");
            //inspectionResult2.AddExtraResult("InspectPosition", this.inspSeqMap[curSeqOrder]);
            //inspectionResult2.AddExtraResult("InspectSequence", curSeqOrder);

            //inspectionResult2.InspectionNo = inspectCount.ToString();
            inspectionResult2.InspectionNo = (SystemManager.Instance().ProductionManager.GetProduction(inspectionResult2).LastInspectionNo + 1).ToString();
        }

        public override void PostProcess(DynMvp.InspData.InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectionState::PostProcess");
            AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            axisHandler.WaitMoveDone();

            curSeqOrder = nextSeqOrder;
            this.imageSequnece = inspSeqMap[curSeqOrder] + 1;
        }

        public override UniscanState GetNextState(DynMvp.InspData.InspectionResult inspectionResult)
        {
            return this;
        }
        public void InspectDiff(AlgoImage inspImage, InspectParam inspectParam, InspectionResult inspectionResult)
        {

        }
    }
}
