using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using System.IO;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Vision;
using System.Drawing.Imaging;
using DynMvp.Devices.Dio;
using DynMvp.InspData;
using DynMvp.Devices.MotionController;

namespace DynMvp.Inspection
{
    public class InspectionOption
    {
        bool saveDebugImage;
        public bool SaveDebugImage
        {
            get { return saveDebugImage; }
        }

        bool saveProbeImage;
        public bool SaveProbeImage
        {
            get { return saveProbeImage; }
        }

        bool saveTargetImage;
        public bool SaveTargetImage
        {
            get { return saveTargetImage; }
        }

        bool saveTargetGroupImage;
        public bool SaveTargetGroupImage
        {
            get { return saveTargetGroupImage; }
        }

        ImageFormat targetGroupImageFormat;
        public ImageFormat TargetGroupImageFormat
        {
            get { return targetGroupImageFormat; }
        }

        int stepDelayMs;
        public int StepDelayMs
        {
            get { return stepDelayMs; }
        }

        Calibration cameraCalibration;
        public Calibration CameraCalibration
        {
            get { return cameraCalibration; }
        }

        PositionAligner positionAligner = null;
        public PositionAligner PositionAligner
        {
            get { return positionAligner; }
        }

        int sequenceNo;
        public int SequenceNo
        {
            get { return sequenceNo; }
            set { sequenceNo = value; }
        }

        DigitalIoHandler digitalIoHandler;
        public DigitalIoHandler DigitalIoHandler
        {
            get { return digitalIoHandler; }
        }

        float pixelRes3d;
        public float PixelRes3d
        {
            get { return pixelRes3d; }
            set { pixelRes3d = value; }
        }

        int clipExtendSize;
        public int ClipExtendSize
        {
            get { return clipExtendSize; }
        }

        public InspectionOption()
        {
        }

        public void SetInspParam(int sequenceNo, bool saveDebugImage, bool saveProbeImage,
            bool saveTargetImage, bool saveTargetGroupImage, ImageFormat targetGroupImageFormat, 
            int stepDelayMs, PositionAligner positionAligner, int clipExtendSize)
        {
            this.sequenceNo = sequenceNo;
            this.saveDebugImage = saveDebugImage;
            this.saveProbeImage = saveProbeImage;
            this.saveTargetImage = saveTargetImage;
            this.saveTargetGroupImage = saveTargetGroupImage;
            this.targetGroupImageFormat = targetGroupImageFormat;
            this.stepDelayMs = stepDelayMs;
            this.positionAligner = positionAligner;
            this.clipExtendSize = clipExtendSize;
        }

        public void SetDeviceParam(Calibration cameraCalibration, DigitalIoHandler digitalIoHandler)
        {
            this.cameraCalibration = cameraCalibration;
            this.digitalIoHandler = digitalIoHandler;
        }
    }

    public delegate void InspectionEventHandler(object hintObj);

    public class InspectionTargetGroupThread : ThreadHandler
    {
        public enum InspectionEventType
        {
            InspectionStarted
        }

        private TargetGroup targetGroup;
        public TargetGroup TargetGroup
        {
            get { return targetGroup; }
        }

        private InspectionResult inspectionResult = new InspectionResult();
        public InspectionResult InspectionResult
        {
            get { return inspectionResult; }
            set { inspectionResult = value; }
        }

        private InspectionResult targetGroupInspectionResult = new InspectionResult();
        public InspectionResult TargetGroupInspectionResult
        {
            get { return targetGroupInspectionResult; }
        }

        public InspectionStepInspectedDelegate InspectionStepInspected = null;
        public TargetGroupInspectedDelegate TargetGroupInspected = null;
        public TargetInspectedDelegate TargetInspected = null;

        private DeviceImageSet deviceImageSet;
        private bool imageOwner = false;
        private Stopwatch stopWatch = null;

        public InspectionTargetGroupThread(TargetGroup targetGroup, DeviceImageSet deviceImageSet, bool imageOwner = false, Stopwatch stopWatch = null) : base("InspectionTargetGroupThread")
        {
            this.targetGroup = targetGroup;
            this.deviceImageSet = deviceImageSet;
            this.imageOwner = imageOwner;
            this.stopWatch = stopWatch;
        }

        public void Start(InspectionOption inspectionOption)
        {
            WorkingThread = new Thread(new ParameterizedThreadStart(InspectTargetGroupProc));
            WorkingThread.IsBackground = true;
            WorkingThread.Start(inspectionOption);

            ThreadManager.AddThread(this);
        }

        private void InspectTargetGroupProc(object obj)
        {
            InspectionOption inspectionOption = (InspectionOption)obj;

            InspectTargetGroup(inspectionOption);
        }

        public void InspectTargetGroup(InspectionOption inspectionOption)
        {
            if (inspectionOption == null)
                return;
            
            LogHelper.Debug(LoggerType.Inspection, "Thread Started : InspectTargetGroupProc");

            int groupId = targetGroup.GroupId;

            InspParam inspParam = new InspParam(inspectionOption.SequenceNo, deviceImageSet, 
                    inspectionOption.SaveDebugImage, inspectionOption.SaveProbeImage, inspectionOption.SaveTargetImage, 
                    inspectionOption.SaveTargetGroupImage, inspectionOption.TargetGroupImageFormat, inspectionOption.ClipExtendSize);
            inspParam.PositionAligner = inspectionOption.PositionAligner;
            if (inspectionOption.CameraCalibration != null && inspectionOption.CameraCalibration.IsCalibrated())
                inspParam.CameraCalibration = inspectionOption.CameraCalibration;
            inspParam.DigitalIoHandler = inspectionOption.DigitalIoHandler;
            inspParam.InspectionResult = inspectionResult;
            inspParam.PixelRes3d = inspectionOption.PixelRes3d;
            inspParam.TargetInspected = TargetInspected;

            targetGroupInspectionResult.ResultPath = inspectionResult.ResultPath;

            targetGroup.Inspect(inspParam, targetGroupInspectionResult);

            if (deviceImageSet.Image3D != null)
                inspectionResult.GrabImageList[(int)targetGroup.GroupId] = deviceImageSet.Image3D.Clone();

            LogHelper.Debug(LoggerType.Inspection, "Thread Finished : InspectTargetGroupProc");

            if (TargetGroupInspected != null)
                TargetGroupInspected(targetGroup, inspectionResult, targetGroupInspectionResult);

            if (InspectionStepInspected != null)
                InspectionStepInspected(targetGroup.InspectionStep, 0, inspectionResult);

            ThreadManager.RemoveThread(this);

            if (stopWatch != null)
                LogHelper.Debug(LoggerType.Inspection, String.Format("Total Inspection Time : {0} ms", stopWatch.ElapsedMilliseconds));
        }

        public bool IsAlive
        {
            get { return WorkingThread.IsAlive; }
        }

        public void Terminate()
        {
            WorkingThread.Abort();
        }

        public int GetInspectionResultCount()
        {
            return targetGroupInspectionResult.Count();
        }
    }

    public abstract class InspectProcessorExtender
    {
        protected ImageDeviceHandler imageDeviceHandler;
        public ImageDeviceHandler ImageDeviceHandler
        {
            get { return imageDeviceHandler; }
        }

        protected ImageAcquisition imageAcquisition;
        public ImageAcquisition ImageAcquisition
        {
            get { return imageAcquisition; }
        }

        public abstract Model GetCurrentModel();
        public abstract InspectionOption GetInspectionOption(int cameraIndex, int sequenceNo = -1);
        public virtual int GetInspectionStepNo() { return 0; }
        public abstract void PrepareInspection();
        public abstract void InspectionStarted(InspectionResult inpsectionResult);
        public abstract InspectionResult NewInspectionResult();
        public abstract InspectionResult BuildInspectionResult(string inspectionNo = null);
        public abstract void PrepareStepInspection(InspectionStep inspectionStep, InspectionResult inspectionResult);
        public abstract void PostStepInspection(InspectionStep inspectionStep, InspectionResult inspectionResult);
        public abstract void Acquire(InspectionStep inspectionStep);
        //public abstract void Acquire(InspectionStep inspectionStep, int exposureIndex);
        public abstract void TargetInspected(Target target, InspectionResult targetInspectionResult);
        public abstract void TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult);
        public abstract void InspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, InspectionResult inspectionResult);
        public abstract void ProductInspected(InspectionResult inspectionResult);
        public abstract void PostInspection(InspectionResult inspectionResult);
        public abstract void ResetState();
        public abstract void UpdateImage(DeviceImageSet deviceImageSet, int stepNo, int groupId, InspectionResult inspectionResult);
        public abstract void TurnOnResultLamp(InspectionResult inspectionResult);
        public abstract bool EnableInspectionStep(InspectionStep inspectionStep);

        public virtual void AlignPosition(AxisPosition axisPosition) { }
    }

    /** Sample --------------

     * class CaviInspectProcessorExtender : InspectProcessorExtender
     * {
            Model GetCurrentModel()
     *      {
     *          
     *      }
     *      InspectionOption GetInspectionOption()
     *      {
     *      
     *      }

     *      public PrepareInspection()
     *      {
                ResetResult();
     * 
                LogHelper.Debug(LoggerType.Inspection,"Start & Tower Lamp On");
                ioHandler.TowerLampWorking();
     * 
     *          try
                {
                    if (InspectionResult.RetryInspection)
                        ioHandler.LoadProduct(LoadProductStage.RetryInspection);
                    else
                        ioHandler.LoadProduct(LoadProductStage.Inspection);
                }
                catch (ActionTimeoutException ex)
                {
                    ioHandler.SetError(ex.Message);
                }

     *          ioHandler.SetupEnvironment(currentModel.ModelDescription);
 
                Thread.Sleep(Settings.Instance().Time.AirActionStableTimeMs);

     *      }   

     *      InspectionResult CreateInspectionResult()
     *      {
     *              
     *      }
 
     *      public void PrepareStepInspection(InspectionStep inspectionStep)
            {

                LogHelper.Debug(LoggerType.Inspection,String.Format("Step - {0} : Prepare Inspection.", inspectionStep));

                ioHandler.MakeStepStatus(inspectionStep);

                LogHelper.Debug(LoggerType.Inspection,String.Format("Step - {0} : Preparation for Inspection is finished.", inspectionStep));
            }

     *      public void Acquire(InspectionStep inspectionStep)
     *      {
     *          inspectionStep.UpdateImageBuffer(imageSource.ImageBuffer);
     *          imageSource.Acquire(inspectionStep.StepNo, lightCtrl, Settings.Instance().Time.LightStableTimeMs);
     *      }
     *      
     *      public void TargetInspected(string targetId, bool result, InspectionResult targetInspectionResult)
     *      {
     *      
     *      }
 
            public void InspectionStepInspected(InspectionStep inspectionStep)
            {
                    Thread.Sleep(Settings.Instance().Time.StepDelayS);
            }
     * 
     * 
     *      public void ProductInspected(InspectionResult inspectionResult)
     *      {
     *          // Review 대화 창 등.... 
     *      }
     *      
     *      public void PostInspection()
            {
                lightCtrl.TurnOff();

                try
                {
                    Thread.Sleep(2000);

                    ioHandler.PullAllPusher();
                    ioHandler.UnloadProduct(LoadProductStage.Inspection);
                }
                catch (ActionTimeoutException ex)
                {
                    ioHandler.SetError(ex.Message);
                }
            }
     * 
     */

    public enum InspectionState
    {
        Stopped,        // 검사 정지
        Waitting,        // 검사 중. 제품 대기
        Inspecting,     // 검사 중. 
        Good,           // 검사 결과 Good
        FalseReject,    // 직전 검사결과 FalseReject
        NG,             // 직전 검사결과 NG
        NotTrained,      // 제곧네
        Alarm      // 제곧네
    }

    public delegate void InspectStatusChagedDelegate(InspectionState inspectionState);
    public abstract class InspectProcessor : ThreadHandler
    {
        public InspectStatusChagedDelegate InspectStatusChaged = null;

        protected InspectProcessorExtender inspectProcessorExtender;
        public InspectProcessorExtender InspectProcessorExtender
        {
            get { return inspectProcessorExtender; }
            set { inspectProcessorExtender = value; }
        }

        protected bool onInspection = false;
        public bool OnInspection
        {
            get { return onInspection; }
        }

        protected bool onWaitInspection = false;

        protected InspectionResult inspectionResult;
        public InspectionResult InspectionResult
        {
            get { return inspectionResult; }
            set { inspectionResult = value; }
        }

        protected bool processingOnly = false;
        public bool ProcessingOnly
        {
            get { return processingOnly; }
            set { processingOnly = value; }
        }

        string inspectionNo = null;
        public string InspectionNo
        {
            get { return inspectionNo; }
            set { inspectionNo = value; }
        }

        object tag = null;
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        protected List<InspectionTargetGroupThread> inspectionTargetGroupThreadList = new List<InspectionTargetGroupThread>();

        public InspectProcessor():base("InspectProcesser")
        {

        }

        public abstract void StartInspection();
        public abstract void StopInspection();
        public abstract void RetryInspection();
        public virtual bool InspectInspectionStep(InspectionStep inspectionStep, int sequenceNo = -1) { return true;  } 

        public void PauseInspection()
        {
            onWaitInspection = false;
        }

        public void ResumeInspection()
        {
            onWaitInspection = true;
        }

        protected bool IsOnInspect()
        {
            foreach (InspectionTargetGroupThread inspectionTargetGroupThread in inspectionTargetGroupThreadList)
            {
                if (inspectionTargetGroupThread.IsAlive == true)
                    return true;
            }

            return false;
        }

        public void InspectProcessor_TargetInspected(Target target, InspectionResult targetInspectionResult)
        {
            inspectProcessorExtender.TargetInspected(target, targetInspectionResult);
        }

        public void InspectionThread_TargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult objectInspectionResult)
        {
            inspectProcessorExtender.TargetGroupInspected(targetGroup, inspectionResult, objectInspectionResult);
        }

        public int GetInspectionResultCount()
        {
            int sumCount = 0;
            foreach (InspectionTargetGroupThread inspectionTargetGroupThread in inspectionTargetGroupThreadList)
            {
                sumCount += inspectionTargetGroupThread.GetInspectionResultCount();
            }

            return sumCount;
        }
    }
}
