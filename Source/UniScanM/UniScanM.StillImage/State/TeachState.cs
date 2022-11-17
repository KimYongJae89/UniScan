using DynMvp.Base;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanM.StillImage.Algorithm;
using UniScanM.StillImage.Data;
//using DynMvp.InspData;
using UniEye.Base;
using UniScanM;
using UniEye.Base.Inspect;

namespace UniScanM.StillImage.State
{
    public class TeachState : UniscanState
    {

        Dictionary<AxisPosition, TeachData> teachDetaDic;
        AxisPosition curTeachPos;
        Task workingTask = null;

        FindingSheet findingSheet = null;
        Teacher teacher = null;

        bool teachDone = false;

        public override bool IsTeachState
        {
            get { return true; }
        }

        public TeachState() : base()
        {
            inspectState = UniEye.Base.Data.InspectState.Scan;
            findingSheet = new FindingSheet();
            teacher = Teacher.Create(0);
            this.imageSequnece = 1;

            this.teachDetaDic = new Dictionary<AxisPosition, TeachData>();
            curTeachPos = null;
            teachDone = false;

            Initialize();
        }

        protected override void Init()
        {
            LogHelper.Debug(LoggerType.Operation, "TeachState::Init");
            this.OnAsyncProcessDone = SystemManager.Instance().InspectRunner.ProductInspected;

            AxisPosition[] limitPosition = SystemManager.Instance().DeviceController.RobotStage.GetLimitPos();
            SystemManager.Instance().DeviceController.RobotStage.Move(limitPosition[0]);
            curTeachPos = SystemManager.Instance().DeviceController.RobotStage.GetActualPos();
            this.imageSequnece = 1;

            //모델 내의 BMP 파일 지우기
            Model model = SystemManager.Instance().CurrentModel as Model;
            model.CleanImage();
        }

        public override void PreProcess()
        {
            LogHelper.Debug(LoggerType.Operation, "TeachState::PreProcess");

            if (this.workingTask != null)
                this.workingTask.Wait();

            if (teachDone == false)
                curTeachPos = SystemManager.Instance().DeviceController.RobotStage.GetActualPos();
        }

        public override void OnProcess(ImageD imageD, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            LogHelper.Debug(LoggerType.Operation, "TeachState::OnProcess");

            //algoImage.Save(SystemManager.Instance().CurrentModel.GetImagePathName(0, this.imageSequnece, 0));   // 0번은 LightTune영상. 1번부터 Teach 영상
            workingTask = Task.Run(() => OnProcessProc(imageD, inspectionResult, inspectionOption));
            return;
        }

        private void OnProcessProc(ImageD imageD, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            Debug.WriteLine("TeachState::OnProcessProc Start");
            InspectionResult inspectionResult2 = (InspectionResult)inspectionResult;
            inspectionResult2.InspZone = imageSequnece;
            inspectionResult2.AddExtraResult("Sequnece", imageSequnece - 1);  // 0-Base
            AlgoImage frameImage = null;
            AlgoImage sheetImage = null;
            //AlgoImage teachImage = null;
            AlgoImage saveImage = null;
            Task saveTask = null;
            try
            {
                frameImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);
                Debug.Assert(frameImage != null);

                // 0번은 LightTune영상. 1번부터 Teach 영상
                saveImage = frameImage.Clone();
                saveTask = Task.Run(() => saveImage.Save(SystemManager.Instance().CurrentModel.GetImagePathName(0, this.imageSequnece, 0)));
        
                sheetImage = findingSheet.GetSheetImage(frameImage, inspectionResult2);

                if (sheetImage != null)
                {
                    teacher.Teach(sheetImage, inspectionResult2);
                }
                else
                {
                    inspectionResult2.TeachData = new TeachData(-1, null);

                    int aSize = Math.Min(frameImage.Width, frameImage.Height);
                    inspectionResult2.InspRectInSheet = SheetFinder.GetInspRect(frameImage.Size, new Size(aSize, aSize));
                }

                if (teachDetaDic.ContainsKey(curTeachPos) == false)
                    teachDetaDic.Add(curTeachPos, inspectionResult2.TeachData);
                //inspectionResult2.TeachData.ScaledImage?.SaveImage(@"d:\temp\tt.bmp");
            }
            finally
            {
                inspectionResult2.InspZone = -1;
                inspectionResult2.InspectState = "Teaching";

                saveTask?.Wait();
                saveImage?.Dispose();
                //teachImage?.Dispose();
                sheetImage?.Dispose();
                frameImage?.Dispose(); // 비동기 임으로 여기서 Dispose 해야 함.

                OnAsyncProcessDone?.Invoke(inspectionResult);
            }
            Debug.WriteLine("TeachState::OnProcessProc End");
        }

        public override void PostProcess(DynMvp.InspData.InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Operation, "TeachState::PostProcess");
            Settings.StillImageSettings additionalSettings = AdditionalSettings.Instance() as Settings.StillImageSettings;
            float mul = additionalSettings.FovMultipication;
            int fovWidthPx = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(0).ImageSize.Width;
            float pelWidth = SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Width;
            float fovWidthUm = fovWidthPx * pelWidth * mul;

            AxisPosition axisPosition = curTeachPos.Clone();
            axisPosition.Add(fovWidthUm);
            if (SystemManager.Instance().DeviceController.RobotStage.IsLimit(axisPosition))//티칭 끝 위치에 도달하면.
            {
                workingTask.Wait();
                if (teachDone == false)
                    ValidCheck();//다른 위치에서의 측정형상과, 주인쇄형상이랑 유사한 정도를 판단하여 IsInspectable=true 처리함.
                teachDone = true;
            }
            else // 계속 티칭
            {
                SystemManager.Instance().DeviceController.RobotStage.StartMove(axisPosition);
                SystemManager.Instance().DeviceController.RobotStage.WaitMoveDone();
                this.imageSequnece++;
            }
        }
        private void ValidCheck()
        {
            List<AxisPosition> maxSimTeachDataKeyList = null;
            int maxSimCount = 0;
            foreach (KeyValuePair<AxisPosition, TeachData> pair in teachDetaDic)
            {
                //pair.Value.ScaledImage?.SaveImage(@"d:\temp\ScaledImage.bmp");

                if (pair.Value.PatternInfoGroupList.Count == 0)
                    continue;

                if (pair.Value.IsInspectable == false)
                    continue;


                double area = pair.Value.PatternInfoGroupList[0].ShapeInfoList.Average(f => f.Area);
                double width = pair.Value.PatternInfoGroupList[0].ShapeInfoList.Average(f => f.Width);
                double height = pair.Value.PatternInfoGroupList[0].ShapeInfoList.Average(f => f.Height);
                double waist = pair.Value.PatternInfoGroupList[0].ShapeInfoList.Average(f => f.Waist);
                int simCount = 0;
                List<AxisPosition> simTeachDataKeyList = new List<AxisPosition>();

                foreach (KeyValuePair<AxisPosition, TeachData> pair2 in teachDetaDic)
                {
                    if (pair2.Value.PatternInfoGroupList.Count == 0)
                        continue;
                    if (pair2.Value.IsInspectable == false)
                        continue;

                    double area2 = pair2.Value.PatternInfoGroupList[0].ShapeInfoList.Average(f => f.Area);
                    double width2 = pair2.Value.PatternInfoGroupList[0].ShapeInfoList.Average(f => f.Width);
                    double height2 = pair2.Value.PatternInfoGroupList[0].ShapeInfoList.Average(f => f.Height);
                    double waist2 = pair2.Value.PatternInfoGroupList[0].ShapeInfoList.Average(f => f.Waist);

                    double[] score = new double[]{
                    Math.Min(area, area2) / Math.Max(area, area2),
                    Math.Min(width, width2) / Math.Max(width, width2),
                    Math.Min(height, height2) / Math.Max(height, height2),
                    Math.Min(waist, waist2) / Math.Max(waist, waist2)
                    };

                    if (Array.TrueForAll(score, s=>(0.95 <s)))
                    {
                        simCount++;
                        simTeachDataKeyList.Add(pair2.Key);
                    }
                }

                if (simCount > maxSimCount)
                {
                    maxSimCount = simCount;
                    maxSimTeachDataKeyList = simTeachDataKeyList;
                }
            }


            if (maxSimTeachDataKeyList != null && maxSimTeachDataKeyList.Count > 1)
            {
                teachDetaDic.ToList().ForEach(f => f.Value.IsInspectable = false);
                //int count = maxSimTeachDataKeyList.Count;
                //Size inspSize = Size.Empty;
                //float area = 0;
                //SizeF margin = Size.Empty;
                //SizeF blot = Size.Empty;
                foreach (AxisPosition axisPosition in maxSimTeachDataKeyList)
                {
                    teachDetaDic[axisPosition].IsInspectable = true;
                    //inspSize = Size.Add(inspSize, teachDetaDic[axisPosition].InspSize);
                    //area += teachDetaDic[axisPosition].PatternInfoGroupList[0].TeachInfo.Feature.Area;
                    //margin = SizeF.Add(margin, teachDetaDic[axisPosition].PatternInfoGroupList[0].TeachInfo.Feature.Margin);
                    //blot = SizeF.Add(blot, teachDetaDic[axisPosition].PatternInfoGroupList[0].TeachInfo.Feature.Blot);
                }

                //Model model = SystemManager.Instance().CurrentModel as Model;
                //model.InspSize = new Size(inspSize.Width / count, inspSize.Height / count);
                //model.Feature = new Feature(area / count, new SizeF(margin.Width / count, margin.Height / count), new SizeF(blot.Width / count, blot.Height / count));

                //foreach (AxisPosition axisPosition in maxSimTeachDataKeyList)
                //    teachDetaDic[axisPosition].PatternInfoGroupList[0].TeachInfo.Feature.Update(model.Feature);
            }
            else
            {
                teachDetaDic.All(f => f.Value.IsInspectable = true);
            }
        }
        
        public override UniscanState GetNextState(DynMvp.InspData.InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Operation, "TeachState::GetNextState");
            if (teachDone)
            {
                int count = teachDetaDic.Values.Count(f => f != null && f.TeachDone);
                if (count == 0)
                {
                    return new TeachState();
                }
                else
                {
                    List<TeachData> validTeachDeta = teachDetaDic.Values.ToList().FindAll(f => f != null && f.TeachDone);
                    if (validTeachDeta.Count > 0)
                    {
                        Model model = SystemManager.Instance().CurrentModel as Model;
                        model.TeachDataDic = teachDetaDic;
                        DynMvp.UI.Touch.SimpleProgressForm form = new DynMvp.UI.Touch.SimpleProgressForm("Save Data");
                        form.Show(() =>
                        {
                            model.UpdateFullImage();
                            SystemManager.Instance().ModelManager.SaveModel(SystemManager.Instance().CurrentModel);
                        });

                        InspectParam inspectParam = (StillImage.Data.InspectParam)((Model)(SystemManager.Instance().CurrentModel)).InspectParam;
                        return new InspectionState(teachDetaDic, inspectParam);
                    }
                }
            }
            return this;
        }
    }
}
