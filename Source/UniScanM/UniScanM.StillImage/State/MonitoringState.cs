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
using DynMvp.InspData;
using UniEye.Base;
using UniEye.Base.Inspect;
using DynMvp.Base;
using System.Drawing;
using System.Diagnostics;
using UniScanM.StillImage.Data;
using UniScanM.StillImage.Algorithm;

namespace UniScanM.StillImage.State
{
    public class MonitoringState : UniscanState
    {
        FindingSheet findingSheet = null;
        int inspectCount;

        Dictionary<AxisPosition, TeachData> teachDataDic = null;

        List<AxisPosition> posOrderList;
        List<int> seqOrderList;

        /// <summary>
        /// 최초 탐색 중
        /// </summary>
        bool scanning = true;

        /// <summary>
        /// 다음 위치까지 인덱스 변화량 (1: 정방향, -1: 역방향)
        /// </summary>
        int sequenceOrderIncStep = 1;

        /// <summary>
        /// 현재 위치
        /// </summary>
        int curPosOrder = -1;
        int curSeqOrder = -1;

        /// <summary>
        /// Operation Mode가 Random 일 때.
        /// </summary>
        Random random = null;

        public override bool IsTeachState
        {
            get { return false; }
        }

        public MonitoringState(Dictionary<AxisPosition, TeachData> teachDataDic) : base()
        {
            inspectState = UniEye.Base.Data.InspectState.Run;
            findingSheet = new FindingSheet();
            posOrderList = new List<AxisPosition>();
            seqOrderList = new List<int>();
            random = new Random();

            this.teachDataDic = teachDataDic;
            if (teachDataDic == null || teachDataDic.Count == 0)
            {
                curPosOrder = 0;
                curSeqOrder = -1;
                scanning = true;
            }
            else
            {
                posOrderList = teachDataDic.Keys.ToList();
                int srcIdx = teachDataDic.Values.ToList().FindIndex(f => f.TeachDone);
                int dstIdx = teachDataDic.Values.ToList().FindLastIndex(f => f.TeachDone);
                for (int i = srcIdx; i <= dstIdx; i++)
                    seqOrderList.Add(i);
                curSeqOrder = 0;
                curPosOrder = seqOrderList.First();
                scanning = false;
            }
            sequenceOrderIncStep = 1;
            this.imageSequnece = curPosOrder + 1;// 0번은 LightTune 이미지, 1번부터 스캔 이미지.

            Initialize();
            //SystemManager.Instance().MainForm.SettingPage.UpdateControl("InspPos", inspSeqMap);
        }

        protected override void Init()
        {
            LogHelper.Debug(LoggerType.Inspection, "MonitoringState::Init");
            AxisPosition initialPosition;
            if (scanning)
            {
                initialPosition = SystemManager.Instance().DeviceController.RobotStage.GetLimitPos()[0];
            }
            else
            {
                initialPosition = posOrderList[seqOrderList[curSeqOrder]];
            }
            SystemManager.Instance().DeviceController.RobotStage.Move(initialPosition);
        }

        public override void PreProcess()
        {
            LogHelper.Debug(LoggerType.Inspection, "MonitoringState::PreProcess");

            if(this.scanning)
            {
                AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
                this.posOrderList.Add(axisHandler.GetActualPos());
                this.curPosOrder = this.posOrderList.Count - 1;
            }
        }

        public override void OnProcess(ImageD imageD, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            LogHelper.Debug(LoggerType.Inspection, "MonitoringState::OnProcess");
            StillImage.Data.InspectionResult myInspectionResult = (StillImage.Data.InspectionResult)inspectionResult;

            AlgoImage frameImage = null;
            AlgoImage dispImage = null;

            try
            {
                frameImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);
                Debug.Assert(frameImage != null);

                Model model = SystemManager.Instance().CurrentModel as Model;
                Size inspSize = model == null ? Size.Empty : model.InspSize;
                if (inspSize.IsEmpty)
                {
                    int size = Math.Min(frameImage.Width, frameImage.Height);
                    inspSize = new Size(size, size);
                }

                dispImage = findingSheet.GetInspImage(frameImage, inspSize, inspectionResult);
                if (dispImage != null)
                {
                    myInspectionResult.DisplayBitmap = dispImage.ToImageD().ToBitmap();
                    myInspectionResult.SetGood();
                }
                else
                {
                    myInspectionResult.SetSkip();

                    // Clip display image
                    Rectangle inspRect = SheetFinder.GetInspRect(frameImage.Size, inspSize);
                    if (inspRect.Width > 0 && inspRect.Height > 0)
                    {
                        myInspectionResult.InspRectInSheet = inspRect;
                        AlgoImage clipImage = frameImage.GetSubImage(inspRect);
                        myInspectionResult.DisplayBitmap = clipImage.ToImageD().ToBitmap();
                        clipImage.Dispose();
                    }
                }
            }
            finally
            {
                inspectCount++;
                dispImage?.Dispose();
                frameImage?.Dispose();
            }
        }

        public override void PostProcess(DynMvp.InspData.InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Inspection, "MonitoringState::PostProcess");
            StillImage.Data.InspectionResult myInspectionResult = (StillImage.Data.InspectionResult)inspectionResult;

            if(this.scanning && myInspectionResult.IsGood())
            {
                AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
                this.seqOrderList.Add(curPosOrder);
                this.curSeqOrder = this.seqOrderList.Count - 1;
            }

            myInspectionResult.InspZone = curSeqOrder;
            myInspectionResult.InspectState = "Monitoring";
            //myInspectionResult.AddExtraResult("InspectState", "Monitoring");
            //myInspectionResult.AddExtraResult("InspectPosition", curPosOrder);
            //myInspectionResult.AddExtraResult("InspectSequence", curSeqOrder);
            myInspectionResult.InspectionNo = inspectCount.ToString();

            if (this.scanning)
            {
                MoveNextFov();
            }
            else
            {
                MoveNextPos();
            }

            this.imageSequnece = curPosOrder + 1;
        }

        private void MoveNextPos()
        {
            int nextSeqOrder = this.curSeqOrder + this.sequenceOrderIncStep;
            if (nextSeqOrder <= 0)
            {
                nextSeqOrder = 0;
                sequenceOrderIncStep = 1;
            }
            else if (nextSeqOrder >= this.seqOrderList.Count() - 1)
            {
                nextSeqOrder = this.seqOrderList.Count() - 1;
                sequenceOrderIncStep = -1;
            }
            MoveSeq(nextSeqOrder);
        }

        private void MoveNextFov()
        {
            Settings.StillImageSettings additionalSettings = AdditionalSettings.Instance() as Settings.StillImageSettings;
            float mul = additionalSettings.FovMultipication;

            int fovWidthPx = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0).ImageSize.Width;
            float pelWidth = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width;
            float fovWidthUm = fovWidthPx * pelWidth * sequenceOrderIncStep * mul;

            AxisHandler axisHandler = SystemManager.Instance().DeviceController.RobotStage;
            AxisPosition axisPosition = axisHandler.GetActualPos().Clone();
            axisPosition.Add(fovWidthUm);
            if (SystemManager.Instance().DeviceController.RobotStage.IsLimit(axisPosition))
            {
                this.scanning = (this.seqOrderList.Count == 0);
                if (this.scanning == false)
                {
                    this.sequenceOrderIncStep = -1;
                    MoveSeq(this.seqOrderList.Count() - 1);
                    return;
                }
                else
                {
                    AxisPosition initialPosition = SystemManager.Instance().DeviceController.RobotStage.GetLimitPos()[0];
                    SystemManager.Instance().DeviceController.RobotStage.Move(initialPosition);
                    return;
                }
            }

            SystemManager.Instance().DeviceController.RobotStage.Move(axisPosition);
        }

        private void MoveSeq(int seqOrder)
        {
            curSeqOrder = seqOrder;
            curPosOrder = this.seqOrderList[seqOrder];
            AxisPosition axisPosition = this.posOrderList[curPosOrder];
            SystemManager.Instance().DeviceController.RobotStage.Move(axisPosition);
        }

        public override UniscanState GetNextState(DynMvp.InspData.InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Inspection, "MonitoringState::GetNextState");
            return this;
        }
    }
}
