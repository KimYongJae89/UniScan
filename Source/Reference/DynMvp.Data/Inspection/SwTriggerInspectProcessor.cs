using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Drawing;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Devices.MotionController;
using DynMvp.InspData;

namespace DynMvp.Inspection
{
    public class SwTriggerInspectProcessor : InspectProcessor
    {
        bool singleStepMode;
        public bool SingleStepMode
        {
            get { return singleStepMode; }
            set { singleStepMode = value; }
        }

        bool fastMode;
        public bool FastMode
        {
            get { return fastMode; }
            set { fastMode = value; }
        }

        int inspectionStepTypeMask = InspectionStep.StepAll;
        public int InspectionStepTypeMask
        {
            get { return inspectionStepTypeMask;  }
            set { inspectionStepTypeMask = value;  }
        }

        CancellationTokenSource cancellationTokenSource;

        //AxisHandler robotStage;
        //public AxisHandler RobotStage
        //{
        //    set { robotStage = value; }
        //}

        public SwTriggerInspectProcessor(CancellationTokenSource cancellationTokenSource = null)
        {
            this.cancellationTokenSource = cancellationTokenSource;
        }

        public override void StartInspection()
        {
            if (onInspection)
                return;

            RequestStop = false;
            onInspection = true;

            if (InspectStatusChaged != null)
            {
                InspectStatusChaged(InspectionState.Inspecting);
            }
            
            if (this.InspectionNo != null)
            {
                inspectionResult = inspectProcessorExtender.BuildInspectionResult(this.InspectionNo);
            }
            else
            {
                inspectionResult = inspectProcessorExtender.BuildInspectionResult();
            }

            if (inspectionResult == null)
            {
                onInspection = false;
                return;
            }

            if (this.Tag is string) {
                inspectionResult.InspectionNo = (string)this.Tag;
            }

            LogHelper.Debug(LoggerType.Inspection, "CreateInspectionResult");
            if (inspectionResult == null)
            {
                LogHelper.Debug(LoggerType.Inspection, "Inspection cancel");
                onInspection = false;
                return;
            }
            LogHelper.Debug(LoggerType.Inspection, "WorkingThread Setup.");
            if (singleStepMode == true)
                WorkingThread = new Thread(new ThreadStart(SingleStepInspectionThreadFunc));
            else
                WorkingThread = new Thread(new ThreadStart(InspectionThreadFunc));

            WorkingThread.IsBackground = true;
            LogHelper.Debug(LoggerType.Inspection, "WorkingThread Start");
            WorkingThread.Start();   
        }
       
        public override void StopInspection()
        {
            onWaitInspection = false;
            RequestStop = true;
        }

        public void Stop()
        {
            onWaitInspection = false;
            RequestStop = true;
            onInspection = false;
        }

        public override void RetryInspection()
        {
            if (inspectionResult.RetryInspection == false)
                return;
            onInspection = true;
            
        }

        public void CheckStop()
        {

        }

        public override bool InspectInspectionStep(InspectionStep inspectionStep, int sequenceNo = -1)
        {
            if (inspectionStep.NumTargets == 0)
                return true;

            if (((int)inspectionStep.StepType & inspectionStepTypeMask) != (int)inspectionStep.StepType)
                return true;

            if (inspectProcessorExtender.EnableInspectionStep(inspectionStep) == false)
                return true;

            LogHelper.Debug(LoggerType.Inspection, String.Format("Prepare Step {0} Inspection ", inspectionStep));

            inspectProcessorExtender.PrepareStepInspection(inspectionStep, inspectionResult);

            ErrorManager.Instance().ThrowIfAlarm();

            inspectProcessorExtender.Acquire(inspectionStep);

            ErrorManager.Instance().ThrowIfAlarm();

            LogHelper.Debug(LoggerType.Inspection, "Inspect Targets");
            InspectTargets(inspectionStep, sequenceNo);

            ErrorManager.Instance().ThrowIfAlarm();

            if (inspectionStep.BlockFastStepChange || fastMode == false)
            {
                LogHelper.Debug(LoggerType.Inspection, "Post processing step inspection");
                inspectProcessorExtender.InspectionStepInspected(inspectionStep, sequenceNo, inspectionResult);
                inspectProcessorExtender.PostStepInspection(inspectionStep, inspectionResult);
            }

            return true;
        }

        public void InspectionThreadFunc()
        {
            onInspection = true;

            if (inspectProcessorExtender == null)
            {
                onInspection = false;
                return;
            }

            try
            {
                LogHelper.Debug(LoggerType.Inspection, "Start Inspection");

                Model currentModel = inspectProcessorExtender.GetCurrentModel();

                if (processingOnly == false)
                {
                    inspectProcessorExtender.InspectionStarted(InspectionResult);
                }

                inspectProcessorExtender.PrepareInspection();

                foreach (InspectionStep inspectionStep in currentModel.InspectionStepList)
                {
                    if (RequestStop == true)
                    {
                        onInspection = false;
                        break;
                    }

                    ErrorManager.Instance().ThrowIfAlarm();

                    if (InspectInspectionStep(inspectionStep) == false)
                    {
                        onInspection = false;
                        break;
                    }

                    if (InspectionResult.StepBlockRequired)
                    {
                        onInspection = false;
                        break;
                    }
                }

                //if (ErrorManager.Instance().IsAlarmed())
                //    throw new AlarmOnException("Alarm On");
                //if (AlarmState.Instance().OnAlarm)
                //{
                //    LogHelper.Debug(LoggerType.Inspection, "Machine Alarm is occurred. Inspection is aborted.");
                //    InspectionResult.MachineAlarm = true;
                //}

                if (processingOnly == false && onInspection == true)
                {
                    inspectProcessorExtender.ProductInspected(inspectionResult);
                }
            }
            catch (ActionTimeoutException ex)
            {
                LogHelper.Error(LoggerType.Error, ex.Message);
            }
            catch (AlarmException ex)
            {
                LogHelper.Error(LoggerType.Error, ex.Message);
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            while (IsOnInspect())
            {
                Thread.Sleep(100);

#if DEBUG == false
                    if (stopWatch.ElapsedMilliseconds > 10000)
                    {
                        LogHelper.Debug(LoggerType.Inspection, "Terminate Thread(s)");
                        TerminateAllThread();
                        break;
                    }
#endif
            }

            if (RequestStop == true)
            {
                inspectionResult.Clear();
            }
            else
            {
                inspectProcessorExtender.PostInspection(inspectionResult);
                FinishInspection();
            }
            onInspection = false;

            LogHelper.Debug(LoggerType.Inspection, "Thread Stopped : InspectionThreadFunc");           
        }

        public void SingleStepInspectionThreadFunc()
        {
            if (inspectProcessorExtender == null)
                return;

            Model currentModel = inspectProcessorExtender.GetCurrentModel();

            inspectProcessorExtender.PrepareInspection();

            if (currentModel.InspectionStepList.Count < 1)
                return;

            InspectionStep inspectionStep = currentModel.InspectionStepList[0];
            if (inspectionStep.NumTargets == 0)
                return;

            string orgResultPath = inspectionResult.ResultPath;

            try
            {
                LogHelper.Debug(LoggerType.Inspection, "Start Inspection");

                int sequenceNo = 0;
                while (true)
                {
                    ErrorManager.Instance().ThrowIfAlarm();

                    inspectionResult.ResultPath = orgResultPath + "\\" + sequenceNo.ToString();
                    Directory.CreateDirectory(inspectionResult.ResultPath);

                    if (InspectInspectionStep(inspectionStep, sequenceNo) == false)
                        break;

                    if (InspectionResult.StepBlockRequired)
                        break;

                    sequenceNo++;
                }

                sequenceNo = 0;

                inspectionResult.ResultPath = orgResultPath;

                inspectProcessorExtender.ProductInspected(inspectionResult);

                inspectProcessorExtender.PostInspection(inspectionResult);

                FinishInspection();

                onInspection = false;
            }
            catch (ActionTimeoutException ex)
            {
                LogHelper.Error(LoggerType.Error, ex.Message);
            }
            catch (AlarmException ex)
            {
                LogHelper.Error(LoggerType.Error, ex.Message);
            }

            LogHelper.Debug(LoggerType.Inspection, "Thread Stopped : InspectionThreadFunc");
            if (InspectStatusChaged != null)
            {
                InspectStatusChaged(InspectionState.Waitting);
            }
        }

        public void RetryInspectionThreadFunc()
        {
            if (inspectProcessorExtender == null)
                return;
            Model currentModel = inspectProcessorExtender.GetCurrentModel();

            inspectProcessorExtender.PrepareInspection();

            foreach (InspectionStep inspectionStep in currentModel.InspectionStepList)
            {
                
                if (onInspection == false)
                {
                    inspectionResult.Clear();
                    return;
                }

                if (InspectInspectionStep(inspectionStep) == false)
                    break;

                if (InspectionResult.StepBlockRequired)
                    break;

                if (ErrorManager.Instance().IsAlarmed())
                    break;
            }

            if (ErrorManager.Instance().IsAlarmed())
            {
                LogHelper.Debug(LoggerType.Inspection, "Machine Alarm is occurred. Inspection is aborted.");
                InspectionResult.MachineAlarm = true;
            }
            else
            {
                inspectProcessorExtender.ProductInspected(inspectionResult);
            }

            inspectProcessorExtender.PostInspection(inspectionResult);

            onInspection = false;
        }

        protected void InspectTargets(InspectionStep inspectionStep, int inspectionCount)
        {
            ImageAcquisition imageAcquisition = inspectProcessorExtender.ImageAcquisition;
            inspectionTargetGroupThreadList.Clear();

            LogHelper.Debug(LoggerType.Inspection, String.Format("InspectTargets : InspectionStep {0}", inspectionStep.StepNo));

            foreach (TargetGroup targetGroup in inspectionStep)
            {
                if (targetGroup.TargetList.Count == 0)
                    continue;

                InspectionOption inspectionOption = inspectProcessorExtender.GetInspectionOption(targetGroup.GroupId, inspectionCount);

                //List<ImageD> grabImageList = new List<ImageD>();
                int groupId = targetGroup.GroupId;
                //for (int lightTypeIndex = 0; lightTypeIndex < numLightType; lightTypeIndex++)
                //{
                //    ImageBuffer2dItem imageCell = imageAcquisition.ImageBuffer.GetImageBuffer2dItem(groupId, lightTypeIndex);
                //    if (imageCell == null)
                //        continue;

                //    grabImageList.Add(imageCell.Image.Clone());
                //}

                DeviceImageSet deviceImageSet;

                if (fastMode == true && inspectionStep.BlockFastStepChange == false)
                {
                    deviceImageSet = imageAcquisition.ImageBuffer.GetDeviceImageSet(targetGroup.GroupId).Clone();

                    if (deviceImageSet.Image3D != null)
                        inspectionResult.GrabImageList[(int)targetGroup.GroupId] = deviceImageSet.Image3D;
                    else
                        inspectionResult.GrabImageList[(int)targetGroup.GroupId] = deviceImageSet.ImageList2D[0];
                }
                else
                {
                    deviceImageSet = imageAcquisition.ImageBuffer.GetDeviceImageSet(targetGroup.GroupId);

                    if (deviceImageSet.Image3D != null)
                        inspectionResult.GrabImageList[(int)targetGroup.GroupId] = deviceImageSet.Image3D.Clone();
                    else
                        inspectionResult.GrabImageList[(int)targetGroup.GroupId] = deviceImageSet.ImageList2D[0].Clone();
                }

                inspectProcessorExtender.UpdateImage(deviceImageSet, inspectionStep.StepNo, groupId, inspectionResult);

                LogHelper.Debug(LoggerType.Inspection, String.Format("Setup InspectionTargetGroupThread {0}", targetGroup.FullId));

                InspectionTargetGroupThread inspectionTargetGroupThread = new InspectionTargetGroupThread(targetGroup, deviceImageSet);
                inspectionTargetGroupThread.InspectionResult = inspectionResult;

                if (fastMode == true && inspectionStep.BlockFastStepChange == false)
                {
                    inspectionTargetGroupThread.TargetInspected = new TargetInspectedDelegate(InspectProcessor_FastModeTargetInspected);
                    inspectionTargetGroupThread.TargetGroupInspected += new TargetGroupInspectedDelegate(InspectionThread_FastModeTargetGroupInspected);

                    if (inspectionStep.LastTargetGroup == targetGroup)
                    {
                        inspectionTargetGroupThread.InspectionStepInspected += InspectionThread_FastModeInspectionStepInspected;
                    }
                }
                else
                {
                    inspectionTargetGroupThread.TargetInspected = new TargetInspectedDelegate(InspectProcessor_TargetInspected);
                    inspectionTargetGroupThread.TargetGroupInspected += new TargetGroupInspectedDelegate(InspectionThread_TargetGroupInspected);
                }

                LogHelper.Debug(LoggerType.Inspection, String.Format("Start InspectionTargetGroupThread {0}", targetGroup.FullId));

                inspectionTargetGroupThread.Start(inspectionOption);

                inspectionTargetGroupThreadList.Add(inspectionTargetGroupThread);
            }

            Thread.Sleep(500);

            if (fastMode == false || inspectionStep.BlockFastStepChange == true)
            {
                LogHelper.Debug(LoggerType.Inspection, "Wait Inspection Done");

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                while (IsOnInspect())
                {
                    Thread.Sleep(100);

#if DEBUG == false
                    if (stopWatch.ElapsedMilliseconds > 10000)
                    {
                        LogHelper.Debug(LoggerType.Inspection, "Terminate Thread(s)");
                        TerminateAllThread();
                        break;
                    }
#endif
                }

                LogHelper.Debug(LoggerType.Inspection, "All Inspection Thread Done");

                LogHelper.Debug(LoggerType.Inspection, "Add Probe Result");

                foreach (InspectionTargetGroupThread inspectionTargetGroupThread in inspectionTargetGroupThreadList)
                {
                    inspectionResult.AddProbeResult(inspectionTargetGroupThread.TargetGroupInspectionResult);
                }

                InspParam inspParam = new InspParam(inspectionResult);
                inspParam.TargetInspected = new TargetInspectedDelegate(InspectProcessor_TargetInspected);

                foreach (TargetGroup targetGroup in inspectionStep)
                {
                    targetGroup.Compute(inspParam, inspectionResult);
                }

                LogHelper.Debug(LoggerType.Inspection, "All Probe Result Added");
            }
        }

        private void InspectionThread_FastModeInspectionStepInspected(InspectionStep inspectionStep, int sequenceNo, InspectionResult inspectionResult)
        {
            lock (inspectionResult)
            {
                InspParam inspParam = new InspParam(inspectionResult);
                foreach (TargetGroup targetGroup in inspectionStep)
                {
                    targetGroup.Compute(inspParam, inspectionResult);
                }
            }

            inspectProcessorExtender.InspectionStepInspected(inspectionStep, sequenceNo, inspectionResult);
            inspectProcessorExtender.PostStepInspection(inspectionStep, inspectionResult);
        }

        private void InspectionThread_FastModeTargetGroupInspected(TargetGroup targetGroup, InspectionResult inspectionResult, InspectionResult targetGroupInspectionResult)
        {
            lock(inspectionResult)
            {
                inspectionResult.AddProbeResult(targetGroupInspectionResult);
            }
            inspectProcessorExtender.TargetGroupInspected(targetGroup, inspectionResult, targetGroupInspectionResult);
        }

        private void InspectProcessor_FastModeTargetInspected(Target target, InspectionResult objectInspectionResult)
        {

        }

        private void TerminateAllThread()
        {
            foreach (InspectionTargetGroupThread inspectionTargetGroupThread in inspectionTargetGroupThreadList)
            {
                if (inspectionTargetGroupThread.IsAlive == true)
                {
                    inspectionTargetGroupThread.Terminate();
                }
            }
        }

        public void FinishInspection()
        {
            if (InspectStatusChaged != null)
            {
                InspectStatusChaged(InspectionState.Waitting);
            }
        }
    }
}
