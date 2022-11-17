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
using DynMvp.InspData;

namespace DynMvp.Inspection
{
    public class InteractiveTriggerInspectProcessor : InspectProcessor
    {
        public override void StartInspection()
        {
            inspectionResult = inspectProcessorExtender.BuildInspectionResult(this.InspectionNo);
        }

        public override void StopInspection()
        {
        }

        public override void RetryInspection()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            RequestStop = true;
        }

        public void CheckStop()
        {

        }

        public override bool InspectInspectionStep(InspectionStep inspectionStep, int sequenceNo)
        {
            //if (inspectionStep.NumTargets == 0)
            //    return true;

            LogHelper.Debug(LoggerType.Inspection, String.Format("Prepare Step {0} Inspection ", inspectionStep));

            inspectProcessorExtender.PrepareStepInspection(inspectionStep, inspectionResult);

            ErrorManager.Instance().ThrowIfAlarm();

            inspectProcessorExtender.Acquire(inspectionStep);

            ErrorManager.Instance().ThrowIfAlarm();

            LogHelper.Debug(LoggerType.Inspection, "Inspect Targets");
            bool result = InspectTargets(inspectionStep, sequenceNo);

            ErrorManager.Instance().ThrowIfAlarm();

            LogHelper.Debug(LoggerType.Inspection, "Post processing step inspection");
            inspectProcessorExtender.InspectionStepInspected(inspectionStep, sequenceNo, inspectionResult);

            return result;
        }

        protected bool InspectTargets(InspectionStep inspectionStep, int sequenceNo)
        {
            ImageAcquisition imageAcquisition = inspectProcessorExtender.ImageAcquisition;
            inspectionTargetGroupThreadList.Clear();

            foreach (TargetGroup targetGroup in inspectionStep)
            {
                int groupId = targetGroup.GroupId;

                DeviceImageSet deviceImageSet = imageAcquisition.ImageBuffer.GetDeviceImageSet(groupId);
                    
                inspectionResult.GrabImageList[groupId] = deviceImageSet.ImageList2D[0];

                inspectProcessorExtender.UpdateImage(deviceImageSet, inspectionStep.StepNo, targetGroup.GroupId, inspectionResult);

                InspectionOption inspectionOption = inspectProcessorExtender.GetInspectionOption(targetGroup.GroupId, sequenceNo);

                InspectionTargetGroupThread inspectionTargetGroupThread = new InspectionTargetGroupThread(targetGroup, deviceImageSet);
                inspectionTargetGroupThread.InspectionResult = inspectionResult;
                inspectionTargetGroupThread.TargetInspected = new TargetInspectedDelegate(InspectProcessor_TargetInspected);
                inspectionTargetGroupThread.TargetGroupInspectionResult.ResultPath = inspectionResult.ResultPath;
                inspectionTargetGroupThread.Start(inspectionOption);

                inspectionTargetGroupThreadList.Add(inspectionTargetGroupThread);
            }

            Thread.Sleep(500);

            LogHelper.Debug(LoggerType.Inspection, "Wait Inspection Done");

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            LogHelper.Debug(LoggerType.Inspection, "Wait Inspection StopWatch Started.");

            while (IsOnInspect())
            {
                Thread.Sleep(100);

                if (stopWatch.ElapsedMilliseconds > 10000)
                {
                    LogHelper.Debug(LoggerType.Inspection, "Terminate Thread(s)");
                    TerminateAllThread();
                    break;
                }
            }

            LogHelper.Debug(LoggerType.Inspection, "All Inspection Thread Done");

            LogHelper.Debug(LoggerType.Inspection, "Add Probe Result");

            bool result = true;
            foreach (InspectionTargetGroupThread inspectionTargetGroupThread in inspectionTargetGroupThreadList)
            {
                inspectionResult.AddProbeResult(inspectionTargetGroupThread.TargetGroupInspectionResult);
                result &= (inspectionTargetGroupThread.TargetGroupInspectionResult.Judgment == Judgment.Accept);
            }

            InspParam inspParam = new InspParam(inspectionResult);
            inspParam.TargetInspected = new TargetInspectedDelegate(InspectProcessor_TargetInspected);

            foreach (TargetGroup targetGroup in inspectionStep)
            {
                targetGroup.Compute(inspParam, inspectionResult);
            }

            LogHelper.Debug(LoggerType.Inspection, "All Probe Result Added");

            return result;
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
    }
}
