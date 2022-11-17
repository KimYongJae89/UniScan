using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

using DynMvp.Base;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.InspData;

namespace DynMvp.Inspection
{
    public class HwTriggerInspectProcessor : InspectProcessor
    {
        int stepNo = 0;
        public int StepNo
        {
            get { return stepNo; }
            set { stepNo = value; }
        }

        public override void StartInspection()
        {
            onWaitInspection = true;

            ImageDeviceHandler imageDeviceHandler = inspectProcessorExtender.ImageDeviceHandler;
            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                imageDevice.ImageGrabbed += ImageGrabbed;
            }
        }

        public override void StopInspection()
        {
            onWaitInspection = false;

            ImageDeviceHandler imageDeviceHandler = inspectProcessorExtender.ImageDeviceHandler;
            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                imageDevice.ImageGrabbed -= ImageGrabbed;
            }
        }
        public override void RetryInspection()
        {
            throw new NotImplementedException();
        }

        public void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            if (onWaitInspection == false)
                return;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            LogHelper.Debug(LoggerType.Inspection, String.Format("InspectProcessor.Image Grabbed - ImageDevice {0}", imageDevice.Index));

            LogHelper.Debug(LoggerType.Inspection, String.Format("Inspection Time ( Phase 1 ) : {0} ms", stopWatch.ElapsedMilliseconds));

            Model currentModel = inspectProcessorExtender.GetCurrentModel();
            //TargetGroup targetGroup = currentModel.GetInspectionStep(stepNo).GetTargetGroup((int)camera.Index);

            LogHelper.Debug(LoggerType.Inspection, "Create Inspection Result");

            InspectionResult inspectionResult = inspectProcessorExtender.BuildInspectionResult();
            if (inspectionResult == null)
                return;

            lock (inspectionResult)
            {
                LogHelper.Debug(LoggerType.Inspection, String.Format("Inspection Time ( Phase 2 ) : {0} ms", stopWatch.ElapsedMilliseconds));

                ImageD grabImage = imageDevice.GetGrabbedImage(ptr);

                LogHelper.Debug(LoggerType.Inspection, String.Format("Inspection Time ( Phase 2 ) : {0} ms", stopWatch.ElapsedMilliseconds));

                inspectProcessorExtender.InspectionStarted(inspectionResult);

                LogHelper.Debug(LoggerType.Inspection, String.Format("Inspection Time ( Phase 3 ) : {0} ms", stopWatch.ElapsedMilliseconds));

                DeviceImageSet deviceImageSet = new DeviceImageSet();
                deviceImageSet.ImageList2D.Add((Image2D)grabImage);

                int stepNo = inspectProcessorExtender.GetInspectionStepNo();
                TargetGroup targetGroup = currentModel.GetInspectionStep(stepNo).GetTargetGroup(imageDevice.Index);

                inspectionResult.GrabImageList[imageDevice.Index] = grabImage;

                InspectionOption inspectionOption = inspectProcessorExtender.GetInspectionOption(imageDevice.Index);

                InspectionTargetGroupThread inspectionTargetGroupThread = new InspectionTargetGroupThread(targetGroup, deviceImageSet, true, stopWatch);
                inspectionTargetGroupThread.InspectionResult = inspectionResult;
                inspectionTargetGroupThread.TargetGroupInspected += new TargetGroupInspectedDelegate(InspectionThread_TargetGroupInspected);

                LogHelper.Debug(LoggerType.Inspection, String.Format("Inspection Time ( Prepare ) : {0} ms", stopWatch.ElapsedMilliseconds));

                inspectionTargetGroupThread.Start(inspectionOption);

                lock (inspectionTargetGroupThreadList)
                {
                    inspectionTargetGroupThreadList.Add(inspectionTargetGroupThread);
                }
            }
        }

        public void InspectionThread_TargetGroupInspected(int groupId, InspectionResult inspectionResult, InspectionResult objectInspectionResult)
        {
            LogHelper.Debug(LoggerType.Inspection, "InspectionThread_InspectionFinished");

            Model currentModel = inspectProcessorExtender.GetCurrentModel();
            
            TargetGroup targetGroup = currentModel.GetInspectionStep(stepNo).GetTargetGroup(groupId);

            lock (inspectionResult)
            {
                inspectionResult.AddProbeResult(objectInspectionResult);
                inspectionResult.TargetGroupInspected.Add(groupId);
                inspectProcessorExtender.ProductInspected(inspectionResult);
            }

            lock (inspectionTargetGroupThreadList)
            {
                foreach (InspectionTargetGroupThread inspectionTargetGroupThread in inspectionTargetGroupThreadList)
                {
                    if (inspectionTargetGroupThread.TargetGroup.GroupId == groupId)
                    {
                        inspectionTargetGroupThreadList.Remove(inspectionTargetGroupThread);
                        break;
                    }
                }
            }
        }
    }
}
