using DynMvp.Devices;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanM.StillImage.Algorithm;
//using DynMvp.InspData;
using UniEye.Base;
using UniEye.Base.Inspect;
using DynMvp.Base;
using DynMvp.Devices.Light;
using DynMvp.Devices.FrameGrabber;
using UniScanM.StillImage.Data;
using UniScanM.StillImage.Settings;
using System.Diagnostics;
//using UniScanM.Settings;

namespace UniScanM.StillImage.State
{
    public class LightTuneState : UniscanState
    {
        LightTuner lightTuner = null;
        FindingSheet findingSheet = null;
        int tryCount = 0;
        List<int> sheetHeightPxList = null;

        public override bool IsTeachState
        {
            get { return true; }
        }

        public LightTuneState() : base()
        {
            findingSheet = new FindingSheet();
            inspectState = UniEye.Base.Data.InspectState.Scan;
            lightTuner = LightTuner.Create(1);
            sheetHeightPxList = new List<int>();
            this.imageSequnece = 0;

            Initialize();
        }

        protected override void Init()
        {
            LogHelper.Debug(LoggerType.Inspection, "LightTuneState::Init");
            Model model = SystemManager.Instance().CurrentModel as Model;
            model.SheetHeigthPx = 0;

            tryCount = 0;

            LightParam modelLightParam = SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0];
            LightValue lightValue = modelLightParam.LightValue;

            int initTopValue = StillImageSettings.Instance().InitialTopLightValue;
            int initTBottomValue = (int)Math.Round(StillImageSettings.Instance().InitialTopLightValue * StillImageSettings.Instance().BackLightMultiplier);
            int[] initLightValue = new int[] { initTopValue, 0, 0, initTBottomValue };

            Array.Copy(initLightValue, 0, lightValue.Value, 0, Math.Min(initLightValue.Length, lightValue.NumLight));

            CameraGenTL cameraGenTL = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0) as CameraGenTL;
            if (cameraGenTL != null)
                cameraGenTL.UpdateBuffer(0, 0, 0);

            AxisPosition[] limitPosition = SystemManager.Instance().DeviceController.RobotStage?.GetLimitPos();
            AxisPosition midPos = AxisPosition.GetMidPos(limitPosition);
            SystemManager.Instance().DeviceController.RobotStage?.Move(midPos);
            SystemManager.Instance().DeviceController.RobotStage?.WaitMoveDone();
        }

        public override void PreProcess()
        {
        }

        public override void OnProcess(ImageD imageD, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            LogHelper.Debug(LoggerType.Inspection, "LightTuneState::OnProcess");

            InspectionResult inspectionResult2 = (InspectionResult)inspectionResult;
            AlgoImage frameImage = null;
            AlgoImage sheetImage = null;
            Task saveTask = null;
            try
            {
                frameImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);
                Debug.Assert(frameImage != null);

                saveTask = Task.Run(() =>
                {
                    frameImage.Save(SystemManager.Instance().CurrentModel.GetImagePathName(0, 0, 0));   // 0번은 LightTune영상. 1번부터 Teach 영상
                });

                this.lightTuner.Tune(frameImage, inspectionResult2);
                //algoImage.Save(@"D:\temp\algoImage.bmp");

                bool isGood = inspectionResult2.LightTuneResult.IsGood == true;
                bool isTotaliFail = !isGood && inspectionResult2.LightTuneResult.OffsetLevel == 0;
                bool lightTuneResult = isGood || isTotaliFail;
                LogHelper.Debug(LoggerType.Inspection, $"LightTuneState::OnProcess - lightTuneResult: {lightTuneResult}");

                bool isVirtual = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0) is CameraVirtual;
                if (lightTuneResult || isVirtual)
                {
                    sheetImage = findingSheet.GetSheetImage(frameImage, null);
                    //sheetImage?.Save(@"D:\temp\sheetImage.bmp");
                    if (sheetImage != null)
                    {
                        LogHelper.Debug(LoggerType.Inspection, $"LightTuneState::OnProcess - sheetImage.Height: {sheetImage.Height}");
                        sheetHeightPxList.Add(sheetImage.Height);
                    }
                    else
                    {
                        LogHelper.Debug(LoggerType.Inspection, $"LightTuneState::OnProcess - sheetImage is null");
                        inspectionResult2.SetDefect();
                        sheetHeightPxList.Clear();
                    }

                    sheetImage?.Dispose();
                }
                else
                {
                    inspectionResult2.SetDefect();
                    sheetHeightPxList.Clear();
                }

            }
            finally
            {
                inspectionResult2.LightTuneResult.TryCount = ++tryCount;
                saveTask?.Wait();
                frameImage?.Dispose();
                sheetImage?.Dispose();
            }

            inspectionResult2.InspZone = -1;
            inspectionResult2.InspectState = "LightTune";
            return;
        }

        public override void PostProcess(DynMvp.InspData.InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Inspection, "LightTuneState::PostProcess");
            LightTuneResult lightTuneResult = ((InspectionResult)inspectionResult).LightTuneResult;
            if (inspectionResult.IsDefected())
            {
                if (SystemManager.Instance().DeviceBox.LightCtrlHandler.NumLightCtrl > 0)
                {                 // Light Calibrate
                    LightCtrl lightCtrl = SystemManager.Instance().DeviceBox.LightCtrlHandler.GetLightCtrl(0);
                    int maxLevel = lightCtrl.GetMaxLightLevel();

                    //int levelCalibCount = 0;
                    //int calibFailCount = 0;
                    int offLevel = lightTuneResult.OffsetLevel;
                    if (offLevel == 0)
                    {
                    }

                    LightParamSet curLightParamSet = SystemManager.Instance().CurrentModel.LightParamSet;
                    for (int i = 0; i < curLightParamSet.LightParamList.Count; i++)
                    {
                        LightParam lightParam = curLightParamSet.LightParamList[i];
                        LightValue lightValue = lightParam.LightValue;
                        bool ok = AdjustLight(lightValue, offLevel, maxLevel);
                    }
                    SystemManager.Instance().CurrentModel.LightParamSet = curLightParamSet;
                }
            }
        }

        private bool AdjustLight(LightValue lightValue, int offLevel, int maxLevel)
        {
            bool done = false;
            int offsetValue = offLevel * 1;

            if (lightValue.Value.Length > 2)
                lightValue.Value[1] = 0;
            if (lightValue.Value.Length > 3)
                lightValue.Value[2] = 0;

            // Adjust Top Light
            int topValue = lightValue.Value[0];
            int newTopValue = AddValue(topValue, offsetValue, 0, maxLevel);
            if (newTopValue != topValue)
            {
                lightValue.Value[0] = newTopValue;
                done = true;
            }

            // Adjust Bottom Light
            if (lightValue.Value.Length >= 4)
                lightValue.Value[3] = (int)Math.Round(lightValue.Value[0] * StillImageSettings.Instance().BackLightMultiplier);
            return done;
        }

        private int AddValue(int value, int offset, int min, int max)
        {
            int newValue = value + offset;
            newValue = Math.Min(newValue, max);
            newValue = Math.Max(newValue, min);
            return newValue;
        }

        public override UniscanState GetNextState(DynMvp.InspData.InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Inspection, "LightTuneState::GetNextState");
            LightTuneResult lightTuneResult = ((InspectionResult)inspectionResult).LightTuneResult;

            bool moveNext = sheetHeightPxList.Count >= 2;
            if (moveNext)
            // 다음 상태로 이동
            {
                Model model = SystemManager.Instance().CurrentModel as Model;
                model.SheetHeigthPx = (int)(sheetHeightPxList.Average());

                CameraGenTL cameraGenTL = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0) as CameraGenTL;
                if (cameraGenTL != null)
                {
                    //int grabLength = cameraGenTL.IsBinningVirtical() ? (int)(model.SheetHeigthPx * 2.5 / 2.0) : (int)(model.SheetHeigthPx * 2.5);
                    int grabLength = (int)(model.SheetHeigthPx * 2.5);
                    cameraGenTL.UpdateBuffer(0, grabLength, 0);
                }

                Settings.StillImageSettings additionalSettings = Settings.StillImageSettings.Instance() as Settings.StillImageSettings;
                if (additionalSettings.InspectionMode == EInspectionMode.Inspect)
                {
                    //if(model.IsTaught())
                    //    return new State.InspectionState(model.TeachDataDic, (StillImage.Data.InspectParam)model.InspectParam);
                    //else

                    return new TeachState();
                    /*/
                    InspectParam inspectParam = (StillImage.Data.InspectParam)((Model)(SystemManager.Instance().CurrentModel)).InspectParam;
                    return new InspectionState(model.TeachDataDic, inspectParam);
                    //*/
                }
                else if (additionalSettings.InspectionMode == EInspectionMode.Monitor)
                    return new MonitoringState(model.TeachDataDic);
            }

            // 티칭 안 됨.
            return this;
        }

    }

}
