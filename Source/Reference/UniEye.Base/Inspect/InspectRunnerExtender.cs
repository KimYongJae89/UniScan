using System;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.Comm;
using DynMvp.Vision;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Light;

using DynMvp.UI.Touch;
using DynMvp.InspData;
using DynMvp.Devices.MotionController;
using UniEye.Base.Settings;
using UniEye.Base.Data;
using UniEye.Base.Device;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Authentication;
using System.IO;

namespace UniEye.Base.Inspect
{
    public delegate void UpdateGrabImageDelegate();

    public class InspectRunnerExtender : IDisposable
    {
        public void Dispose()
        {
        }

        protected virtual InspectionResult CreateInspectionResult()
        {
            return new InspectionResult();
        }

        public virtual InspectionResult BuildInspectionResult(string extendInfo = "")
        {
            //LogHelper.Debug(LoggerType.Inspection, "CreateInspectionResult");
            InspectionResult inspectionResult = CreateInspectionResult();

            inspectionResult.ModelName = SystemManager.Instance().CurrentModel.Name;

            inspectionResult.InspectionTime = new TimeSpan(0);
            inspectionResult.ExportTime = new TimeSpan(0);
            inspectionResult.InspectionStartTime = DateTime.Now;
            inspectionResult.InspectionEndTime = DateTime.Now;
            inspectionResult.JobOperator = UserHandler.Instance().CurrentUser.Id;
            inspectionResult.GrabImageList = new List<ImageD>();
            inspectionResult.InspectionNo = GetInspectionNo();

            ProductionBase production = SystemManager.Instance().ProductionManager.CurProduction;
            if (production != null)
            {
                inspectionResult.LotNo = production.LotNo;
                inspectionResult.ResultPath = production.ResultPath;
                Directory.CreateDirectory(inspectionResult.ResultPath);
            }



            //for (int i = 0; i < SystemManager.Instance().DeviceBox.ImageDeviceHandler.Count; i++)
            //{
            //    inspectionResult.GrabImageList.Add(null);
            //}

            //inspectionResult.InputBarcode = "";


            LogHelper.Debug(LoggerType.Inspection, String.Format("Create Inspection Result: {0}", inspectionResult.InspectionNo));

            return inspectionResult;
        }

        protected virtual string GetResultPath(string modelName, DateTime startTime, string lotNo, string inspectionNo = null)
        {
            return PathManager.GetResultPath(modelName, startTime, lotNo, inspectionNo);
        }

        protected virtual string GetInspectionNo()
        {
            return DateTime.Now.ToString("yyyyMMdd.HHmmss.fff");
        }

        protected virtual string GetLotNo()
        {
            ProductionBase production = SystemManager.Instance().ProductionManager.CurProduction;
            if (production == null)
                return "";
            return production.LotNo;
        }

        public virtual InspectOption BuildInspectOption(ImageDevice imageDevice, IntPtr ptr)
        {
            return new InspectOption(imageDevice, ptr);
        }

        public virtual void OnPreInspection()
        {

        }


        public virtual void OnPostInspection()
        {
        }


        //public virtual void StopInspection()
        //{

        //}

        //public virtual int GetInspectionStepNo() { return 0; }

        ///// <summary>
        ///// 검사 시작 전
        ///// </summary>
        //public virtual void OnPreInspection()
        //{
        //    LogHelper.Debug(LoggerType.Inspection, "PrepareInspection");

        //    SystemManager.Instance().MainForm.InspectionPage.InspectionPanel.OnPreInspection();

        //    Thread.Sleep(TimeSettings.Instance().InspectionDelay); // 늘려진 검사 시간 만큼 대기 한다.
        //}

        //public virtual InspectionResult CreateInspectionResult()
        //{
        //    return new InspectionResult();
        //}

        //public virtual string GetInspectionNo(InspectionResult inspectionResult)
        //{
        //    if (production == null || String.IsNullOrEmpty(production.LotNo) == true)
        //        return inspectionResult.InspectionStartTime.ToString("yyyyMMddHHmmssfff");
        //    else
        //    {
        //        int tempSequence = production.LastSequenceNo + 1;
        //        production.LastSequenceNo++;
        //        return string.Format("{0}_{1:00000}", production.LotNo, tempSequence);
        //    }
        //}



        //public virtual void PreStepInspection(InspectionStep inspectionStep, InspectionResult inspectionResult)
        //{
        //    LogHelper.Debug(LoggerType.Inspection, "PrepareStepInspection");

        //    AxisHandler robotStage = SystemManager.Instance().DeviceController.RobotStage;

        //    if (robotStage != null && inspectionStep.BasePosition != null)
        //    {
        //        AxisPosition alignedPosition = new AxisPosition(inspectionStep.BasePosition.GetPosition());
        //        AlignPosition(alignedPosition);

        //        robotStage.Move(alignedPosition);

        //        inspectionStep.AlignedPosition = alignedPosition;
        //    }
        //}

        //public virtual void PostStepInspection(InspectionStep inspectionStep, InspectionResult inspectionResult)
        //{
        //OutPortListIoTrigger outPortList = (OutPortListIoTrigger)portMap.outPort;

        //digitalIoHandler.WriteOutput(outPortList.ResultNg, inspectionResult.Judgment == Judgment.Reject);

        //if (inspectionStep.StepNo == 0)
        //    digitalIoHandler.WriteOutput(outPortList.VisionStep1Completed, true);
        //else
        //    digitalIoHandler.WriteOutput(outPortList.VisionStep2Completed, true);
        //}

        //public virtual bool EnableInspectionStep(InspectionStep inspectionStep)
        //{
        //    return true;
        //}

        //public virtual void Acquire(InspectionStep inspectionStep)
        //{
        //    ImageAcquisition imageAcquisition = SystemManager.Instance().DeviceBox.GetImageAcquisition();
        //    inspectionStep.UpdateImageBuffer(imageAcquisition.ImageBuffer);
        //    imageAcquisition.ImageBuffer.PixelRes3D = MachineSettings.Instance().PixelRes3D;
        //    imageAcquisition.Acquire(inspectionStep.StepNo);

        //    if (MachineSettings.Instance().VirtualMode == true)
        //    {
        //        Load2dImage(inspectionStep.StepNo);
        //    }
        //}

        //public void Load2dImage(int stepIndex)
        //{
        //    for (int lightTypeIndex = 0; lightTypeIndex < MachineSettings.Instance().NumLightType; lightTypeIndex++)
        //    {
        //        //SystemManager.Instance().MainForm.Load2dImage(0, stepIndex, lightTypeIndex);
        //    }
        //}

        //public virtual void TargetInspected(Target target, InspectionResult targetInspectionResult)
        //{
        //    SystemManager.Instance().MainForm.InspectionPage.InspectionPanel.TargetInspected(target, targetInspectionResult);
        //}

        //public virtual void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult)
        //{
        //    SystemManager.Instance().MainForm.InspectionPage.InspectionPanel.TargetGroupInspected(targetGroup, inspectionResult, objectInspectionResult);
        //}

        //public virtual void InspectionStepInspected(InspectionStep inspectionStep, int inspectCount, InspectionResult inspectionResult)
        //{
        //    LogHelper.Debug(LoggerType.Inspection, "InspectionStepInspected");
        //    SystemManager.Instance().MainForm.InspectionPage.InspectionPanel.InspectionStepInspected(inspectionStep, inspectCount, inspectionResult);
        //}

        //public virtual void ProductInspected(InspectionResult inspectionResult)
        //{
        //    LogHelper.Debug(LoggerType.Inspection, "ProductInspected");

        //    SystemManager.Instance().MainForm.InspectionPage.InspectionPanel.ProductInspected(inspectionResult);
        //}

        //public virtual void OnPostInspection()
        //{
        //    SystemManager.Instance().DeviceBox.LightCtrlHandler.TurnOff();
        //    SystemManager.Instance().MainForm.InspectionPage.InspectionPanel.OnPostInspection();
        //}

        //public virtual void PreTargetGroupInspection(TargetGroup targetGroup, DeviceImageSet deviceImageSet, InspectionResult inspectionResult)
        //{

        //}

        //public virtual void UpdateImage(DeviceImageSet deviceImageSet, int stepNo, int groupId, InspectionResult inspectionResult)
        //{
        //    SystemManager.Instance().MainForm.InspectionPage.InspectionPanel.UpdateImage(deviceImageSet, groupId, inspectionResult);
        //}

        //public virtual void ResetState()
        //{          

        //}

        //public virtual void AlignPosition(AxisPosition axisPosition)
        //{
        //    if (positionAligner != null)
        //    {
        //        PointF newPos = positionAligner.Align(axisPosition.ToPointF());
        //        axisPosition[0] = newPos.X;
        //        axisPosition[1] = newPos.Y;
        //    }
        //}

        //public virtual void Dispose()
        //{

        //}
    }
}
