using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Devices;
using DynMvp.InspData;
using DynMvp.Vision;

namespace UniEye.Base.Inspect
{
    //public class InspectionTask
    //{
    //    private InspectionResult inspectionResult = null;
    //    public InspectionResult InspectionResult
    //    {
    //        get { return inspectionResult; }
    //        set { inspectionResult = value; }
    //    }

    //    private InspectionResult targetGroupInspectionResult = new InspectionResult();
    //    public InspectionResult TargetGroupInspectionResult
    //    {
    //        get { return targetGroupInspectionResult; }
    //    }

    //    public InspectionStepInspectedDelegate InspectionStepInspected = null;
    //    public TargetGroupInspectedDelegate TargetGroupInspected = null;
    //    public TargetInspectedDelegate TargetInspected = null;

    //    private DeviceImageSet deviceImageSet;
    //    private bool imageOwner = false;
    //    private Stopwatch stopWatch = null;

    //    InspectionOption inspectionOption;
    //    private Task workingTask;

    //    public InspectionTargetGroupTask(TargetGroup targetGroup, DeviceImageSet deviceImageSet, bool imageOwner = false, Stopwatch stopWatch = null)
    //    {
    //        this.targetGroup = targetGroup;
    //        this.deviceImageSet = deviceImageSet;
    //        this.imageOwner = imageOwner;
    //        this.stopWatch = stopWatch;
    //    }

    //    public void Start(InspectionOption inspectionOption)
    //    {
    //        LogHelper.Debug(LoggerType.Inspection, "InspectionTargetGroupTask::Start");
    //        this.inspectionOption = inspectionOption;

    //        workingTask = new Task(new Action(InspectTargetGroupProc));
    //        workingTask.Start();
    //    }

    //    private void InspectTargetGroupProc()
    //    {
    //        if (inspectionOption == null)
    //            return;

    //        LogHelper.Debug(LoggerType.Inspection, "Thread Started : InspectTargetGroupProc");

    //        int groupId = targetGroup.GroupId;

    //        InspParam inspParam = new InspParam(inspectionOption.SequenceNo, deviceImageSet,
    //                inspectionOption.SaveDebugImage, inspectionOption.SaveProbeImage, inspectionOption.SaveTargetImage,
    //                inspectionOption.SaveTargetGroupImage, inspectionOption.TargetGroupImageFormat, 0);
    //        inspParam.PositionAligner = inspectionOption.PositionAligner;

    //        if (inspectionOption.CameraCalibration != null && inspectionOption.CameraCalibration.IsCalibrated())
    //            inspParam.CameraCalibration = inspectionOption.CameraCalibration;
    //        inspParam.DigitalIoHandler = inspectionOption.DigitalIoHandler;
    //        inspParam.InspectionResult = inspectionResult;
    //        inspParam.PixelRes3d = inspectionOption.PixelRes3d;
    //        inspParam.TargetInspected = TargetInspected;

    //        targetGroupInspectionResult.ResultPath = inspectionResult.ResultPath;

    //        targetGroup.Inspect(inspParam, targetGroupInspectionResult);

    //        SystemState.Instance().InspectionResult = targetGroupInspectionResult.Judgment;

    //        if (deviceImageSet.Image3D != null)
    //            inspectionResult.GrabImageList[(int)targetGroup.GroupId] = deviceImageSet.Image3D.Clone();

    //        LogHelper.Debug(LoggerType.Inspection, "Thread Finished : InspectTargetGroupProc");

    //        if (TargetGroupInspected != null)
    //            TargetGroupInspected(targetGroup, inspectionResult, targetGroupInspectionResult);

    //        if (InspectionStepInspected != null)
    //            InspectionStepInspected(targetGroup.InspectionStep, 0, inspectionResult);

    //        if (stopWatch != null)
    //            LogHelper.Debug(LoggerType.Inspection, String.Format("Total Inspection Time : {0} ms", stopWatch.ElapsedMilliseconds));

    //        SystemState.Instance().SetInspectState(InspectState.Done);
    //        SystemState.Instance().SetWait();
    //    }

    //    public bool IsAlive
    //    {
    //        get { return workingTask.IsCompleted == false; }
    //    }

    //    public void Terminate()
    //    {
    //        inspectionOption.CancellationTokenSource?.Cancel();
    //    }

    //    public int GetInspectionResultCount()
    //    {
    //        return targetGroupInspectionResult.Count();
    //    }
    //}


    /// <summary>
    /// 검사 객체를 Task로 관리
    /// </summary>
    class TaskBaseInspectRunner: InspectRunner
    {
        private List<ProcessTask> inspectionTaskList = new List<ProcessTask>();

        public override void Inspect(ImageDevice imageDevice, IntPtr ptr, InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            throw new NotImplementedException();
        }

        public override bool PostEnterWaitInspection()
        {
            throw new NotImplementedException();
        }

        public override void PreExitWaitInspection()
        {
            throw new NotImplementedException();
        }
        
        //public virtual bool StartInspection(InspectState inspectState /*= InspectState.Run*/)
        //{
        //    cancellationTokenSource = new CancellationTokenSource();

        //    inspectRunnerExtender.OnPreInspection();

        //    //LogHelper.Debug(LoggerType.Inspection, "CreateInspectionResult");
        //    inspectionResult = inspectRunnerExtender.BuildInspectionResult();
        //    if (inspectionResult == null)
        //    {
        //        LogHelper.Debug(LoggerType.Inspection, "Inspection cancel");
        //        return false;
        //    }

        //    SystemState.Instance().SetInspectState(inspectState);
        //    return true;
        //}

        //public virtual bool IsOnInspect()
        //{
        //    foreach (InspectionTargetGroupTask inspectionTargetGroupTask in inspectionTargetGroupTaskList)
        //    {
        //        if (inspectionTargetGroupTask.IsAlive == true)
        //            return true;
        //    }

        //    return false;
        //}
    }
}
