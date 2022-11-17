using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Devices.Dio;
using DynMvp.Devices;

namespace DynMvp.Data
{
    public class ModelWriterBuilder
    {
        public static ModelWriter Create(float fileVersion)
        {
            if (fileVersion == 0)
                fileVersion = (float)2.0;  // Last Version

            ModelWriter modelWriter;
            if (fileVersion < 2)
            {
                modelWriter = new ModelWriterVer1();
            }
            else
            {
                modelWriter = new ModelWriterVer2();
            }

            return modelWriter;
        }
    }

    public abstract class ModelWriter
    {
        public void Write(Model model, string filePath, IReportProgress reportProgress)
        {
            XmlDocument xmlDocument = new XmlDocument();

            XmlElement modelElement = xmlDocument.CreateElement("", "Model", "");
            xmlDocument.AppendChild(modelElement);

            float versionNo = GetVersion();
            XmlHelper.SetValue(modelElement, "Version", versionNo.ToString("F1"));

            //모델의 LightParamSet을 저장 한다.
            XmlElement lightParamSetElement = xmlDocument.CreateElement("", "LightParamSet", "");
            modelElement.AppendChild(lightParamSetElement);
            model.LightParamSet.Save(lightParamSetElement);

            WriteImageBufferPathList(modelElement, model.ImageBufferPathList);

            WriteTargets(model, modelElement, reportProgress);

            XmlElement fiducialSetListElement = xmlDocument.CreateElement("", "FiducialSetList", "");
            modelElement.AppendChild(fiducialSetListElement);

            foreach(FiducialSet fiducialSet in model.FiducialSetList)
            {
                XmlElement fiducialSetElement = xmlDocument.CreateElement("", "FiducialSet", "");
                fiducialSetListElement.AppendChild(fiducialSetElement);

                WriteFiducialSet(fiducialSetElement, fiducialSet);
            }

            model.SaveModel(modelElement);

            XmlHelper.Save(xmlDocument, filePath);
        }

        public void WriteImageBufferPathList(XmlElement modelElement, List<string> imageBufferPathList)
        {
            XmlDocument xmlDocument = modelElement.OwnerDocument;
            XmlElement imageBufferFileListElement = xmlDocument.CreateElement("", "ImageBufferFileList", "");
            modelElement.AppendChild(imageBufferFileListElement);

            foreach (string fileName in imageBufferPathList)
            {
                XmlHelper.SetValue(imageBufferFileListElement, "ImageBufferFile", fileName);
            }
        }

        protected void WriteFiducialSet(XmlElement fiducialSetElement, FiducialSet fiducialSet)
        {
            XmlDocument xmlDocument = fiducialSetElement.OwnerDocument;
            XmlHelper.SetValue(fiducialSetElement, "Index", fiducialSet.Index.ToString());

            foreach (Probe probe in fiducialSet.Fiducials)
            {
                XmlElement fiducialElement = xmlDocument.CreateElement("", "Fiducial", "");
                fiducialSetElement.AppendChild(fiducialElement);

                XmlHelper.SetValue(fiducialElement, "StepName", probe.Target.TargetGroup.InspectionStep.StepName.ToString());
                XmlHelper.SetValue(fiducialElement, "TargetGroupId", probe.Target.TargetGroup.GroupId.ToString());
                XmlHelper.SetValue(fiducialElement, "TargetId", probe.Target.Id.ToString());
                XmlHelper.SetValue(fiducialElement, "ProbeId", probe.Id.ToString());
            }
        }

        protected void WriteTarget(XmlElement targetElement, Target target)
        {
            XmlDocument xmlDocument = targetElement.OwnerDocument;

            XmlHelper.SetValue(targetElement, "Id", target.Id.ToString());
            XmlHelper.SetValue(targetElement, "Name", target.Name);
            XmlHelper.SetValue(targetElement, "ModuleNo", target.ModuleNo.ToString());
            XmlHelper.SetValue(targetElement, "Type", target.TypeName);
            XmlHelper.SetValue(targetElement, "Image", target.ImageEncodedString);
            XmlHelper.SetValue(targetElement, "UseInspection", target.UseInspection.ToString());
            XmlHelper.SetValue(targetElement, "InspectionLogicType", target.InspectionLogicType.ToString());
            XmlHelper.SetValue(targetElement, "Region", target.BaseRegion);
            XmlHelper.SetValue(targetElement, "LightTypeIndex", target.LightTypeIndex.ToString());

            foreach (Probe probe in target.ProbeList)
            {
                XmlElement probeElement = xmlDocument.CreateElement("", "Probe", "");
                targetElement.AppendChild(probeElement);

                WriteProbe(probeElement, probe);
            }
        }

        protected void WriteProbe(XmlElement probeElement, Probe probe)
        {
            XmlHelper.SetValue(probeElement, "Id", probe.Id.ToString());
            XmlHelper.SetValue(probeElement, "Name", probe.Name.ToString());
            XmlHelper.SetValue(probeElement, "ProbeType", probe.ProbeType.ToString());
            XmlHelper.SetValue(probeElement, "ActAsFiducialProbe", probe.ActAsFiducialProbe.ToString());
            XmlHelper.SetValue(probeElement, "ActAsCalibrationProbe", probe.ActAsCalibrationProbe.ToString());
            XmlHelper.SetValue(probeElement, "FiducialProbeId", probe.FiducialProbeId.ToString());
            XmlHelper.SetValue(probeElement, "InverseResult", probe.InverseResult.ToString());
            XmlHelper.SetValue(probeElement, "ModelVerification", probe.ModelVerification.ToString());
            XmlHelper.SetValue(probeElement, "Region", probe.BaseRegion);
            XmlHelper.SetValue(probeElement, "StepBlocker", probe.StepBlocker.ToString());

            if (probe.FigureProperty != null)
                probe.FigureProperty.Save(probeElement);

            XmlHelper.SetValue(probeElement, "LightTypeIndex", probe.LightTypeIndex.ToString());

            switch (probe.ProbeType)
            {
                case ProbeType.Vision:
                    WriteVisionProbe(probeElement, (VisionProbe)probe);
                    break;
                case ProbeType.Io:
                    WriteIoProbe(probeElement, (IoProbe)probe);
                    break;
                case ProbeType.Serial:
                    WriteSerialProbe(probeElement, (SerialProbe)probe);
                    break;
                case ProbeType.Tension:
                    WriteSerialProbe(probeElement, (TensionSerialProbe)probe);
                    break;
                case ProbeType.Daq:
                    WriteDaqProbe(probeElement, (DaqProbe)probe);
                    break;
                case ProbeType.Marker:
                    WriteMarkerProbe(probeElement, (MarkerProbe)probe);
                    break;
                default:
                    throw new InvalidTypeException();
            }
        }

        protected void WriteMarkerProbe(XmlElement probeElement, MarkerProbe markerProbe)
        {
            XmlHelper.SetValue(probeElement, "MarkerType", markerProbe.MarkerType.ToString());
            XmlHelper.SetValue(probeElement, "MergeSourceId", markerProbe.MergeSourceId);
            XmlHelper.SetValue(probeElement, "MergeOffset", markerProbe.MergeOffset);
        }

        protected void WriteIoProbe(XmlElement probeElement, IoProbe ioProbe)
        {
            XmlHelper.SetValue(probeElement, "PortNo", ioProbe.PortNo.ToString());
            XmlHelper.SetValue(probeElement, "DigitalIoName", ioProbe.DigitalIoName);
        }

        protected void WriteSerialProbe(XmlElement probeElement, SerialProbe serialprobe)
        {
            XmlHelper.SetValue(probeElement, "PortName", serialprobe.PortName);
            XmlHelper.SetValue(probeElement, "UpperValue", serialprobe.UpperValue.ToString());
            XmlHelper.SetValue(probeElement, "LowerValue", serialprobe.LowerValue.ToString());
            XmlHelper.SetValue(probeElement, "NumSerialReading", serialprobe.NumSerialReading.ToString());
            if(serialprobe is TensionSerialProbe)
            {
                XmlHelper.SetValue(probeElement, "TensionFilePath", ((TensionSerialProbe)serialprobe).TensionFilePath.ToString());
                XmlHelper.SetValue(probeElement, "UnitType", ((TensionSerialProbe)serialprobe).UnitType.ToString());
            }
            XmlHelper.SetValue(probeElement, "WorldRegion", serialprobe.WorldRegion);
        }

        protected void WriteDaqProbe(XmlElement probeElement, DaqProbe daqProbe)
        {
            string channelName = "";
            if (daqProbe.DaqChannel != null)
                channelName = daqProbe.DaqChannel.Name;

            XmlHelper.SetValue(probeElement, "ChannelName", channelName);
            XmlHelper.SetValue(probeElement, "UpperValue", daqProbe.UpperValue.ToString());
            XmlHelper.SetValue(probeElement, "LowerValue", daqProbe.LowerValue.ToString());
            XmlHelper.SetValue(probeElement, "NumSample", daqProbe.NumSample.ToString());
            XmlHelper.SetValue(probeElement, "UseLocalScaleFactor", daqProbe.UseLocalScaleFactor.ToString());
            XmlHelper.SetValue(probeElement, "LocalScaleFactor", daqProbe.LocalScaleFactor.ToString());
            XmlHelper.SetValue(probeElement, "ValueOffset", daqProbe.ValueOffset.ToString());
            XmlHelper.SetValue(probeElement, "MeasureType", daqProbe.MeasureType.ToString());
            XmlHelper.SetValue(probeElement, "FilterType", daqProbe.FilterType.ToString());
            XmlHelper.SetValue(probeElement, "Target1Name", daqProbe.Target1Name);
            XmlHelper.SetValue(probeElement, "Target2Name", daqProbe.Target2Name);
        }

        protected void WriteComputeProbe(XmlElement probeElement, SerialProbe serialprobe)
        {
            XmlHelper.SetValue(probeElement, "PortName", serialprobe.PortName);
            XmlHelper.SetValue(probeElement, "UpperValue", serialprobe.UpperValue.ToString());
            XmlHelper.SetValue(probeElement, "LowerValue", serialprobe.LowerValue.ToString());
        }

        public abstract float GetVersion();
        public abstract void WriteTargets(Model model, XmlElement modelElement, IReportProgress reportProgress);
        public abstract void WriteVisionProbe(XmlElement probeElement, VisionProbe visionProbe);
        public abstract void WriteTargetGroup(XmlElement targetGroupElement, TargetGroup targetGroup);
        public abstract void WriteInspectionStep(XmlElement inspectionStepElement, InspectionStep inspectionStep);
    }

    class ModelWriterVer1 : ModelWriter
    {
        public override float GetVersion()
        {
            return (float)1.0;
        }

        public override void WriteTargets(Model model, XmlElement modelElement, IReportProgress reportProgress)
        {
            XmlDocument xmlDocument = modelElement.OwnerDocument;
            int count = 0;

            foreach (InspectionStep inspectionStep in model.InspectionStepList)
            {
                XmlElement inspectionStepElement = xmlDocument.CreateElement("", "InspectionStep", "");
                modelElement.AppendChild(inspectionStepElement);

                WriteInspectionStep(inspectionStepElement, inspectionStep);

                if (reportProgress != null)
                    reportProgress.ReportProgress(count * 10, "");

                count++;
            }
        }

        public override void WriteInspectionStep(XmlElement inspectionStepElement, InspectionStep inspectionStep)
        {
            XmlDocument xmlDocument = inspectionStepElement.OwnerDocument;
            foreach (TargetGroup targetGroup in inspectionStep.TargetGroupList)
            {
                XmlElement targetGroupElement = xmlDocument.CreateElement("", "TargetGroup", "");
                inspectionStepElement.AppendChild(targetGroupElement);

                WriteTargetGroup(targetGroupElement, targetGroup);
            }
        }

        public override void WriteTargetGroup(XmlElement targetGroupElement, TargetGroup targetGroup)
        {
            XmlDocument xmlDocument = targetGroupElement.OwnerDocument;
            XmlHelper.SetValue(targetGroupElement, "CamId", targetGroup.GroupId.ToString());

            XmlElement fiducialSetElement = xmlDocument.CreateElement("", "FiducialSet", "");
            targetGroupElement.AppendChild(fiducialSetElement);

            WriteFiducialSet(fiducialSetElement, targetGroup.FiducialSet);

            foreach (Target target in targetGroup.TargetList)
            {
                XmlElement targetElement = xmlDocument.CreateElement("", "Target", "");
                targetGroupElement.AppendChild(targetElement);

                WriteTarget(targetElement, target);
            }

            foreach(LightParam lightParam in targetGroup.LightParamSet.LightParamList)
            {
                XmlElement lightParamElement = xmlDocument.CreateElement("", "LightParam", "");
                targetGroupElement.AppendChild(lightParamElement);

                lightParam.Save(lightParamElement);
            }
        }

        public override void WriteVisionProbe(XmlElement probeElement, VisionProbe visionProbe)
        {
            XmlDocument xmlDocument = probeElement.OwnerDocument;
            XmlHelper.SetValue(probeElement, "AlgorithmType", visionProbe.InspAlgorithm.GetAlgorithmType().ToString());
            XmlHelper.SetValue(probeElement, "SourceImageType", visionProbe.InspAlgorithm.Param.SourceImageType.ToString());
            XmlHelper.SetValue(probeElement, "ImageBand", visionProbe.InspAlgorithm.Param.ImageBand.ToString());

            XmlElement algorithmElement = xmlDocument.CreateElement("", "Algorithm", "");
            probeElement.AppendChild(algorithmElement);

            AlgorithmArchiver.SaveAlgorithm(algorithmElement, visionProbe.InspAlgorithm);
        }
    }

    class ModelWriterVer2 : ModelWriter
    {
        public override float GetVersion()
        {
            return (float)2.0;
        }

        public override void WriteTargets(Model model, XmlElement modelElement, IReportProgress reportProgress)
        {
            XmlDocument xmlDocument = modelElement.OwnerDocument;
            int count = 0;

            foreach (InspectionStep inspectionStep in model.InspectionStepList)
            {
                XmlElement inspectionStepElement = xmlDocument.CreateElement("", "InspectionStep", "");
                modelElement.AppendChild(inspectionStepElement);

                WriteInspectionStep(inspectionStepElement, inspectionStep);

                if (reportProgress != null)
                    reportProgress.ReportProgress(count * 10, "");

                count++;
            }
        }

        public override void WriteInspectionStep(XmlElement inspectionStepElement, InspectionStep inspectionStep)
        {
            XmlDocument xmlDocument = inspectionStepElement.OwnerDocument;
            if (inspectionStep.BasePosition != null)
                inspectionStep.BasePosition.SetValue(inspectionStepElement, "AxisPosition");

            XmlHelper.SetValue(inspectionStepElement, "Name", inspectionStep.Name);
            XmlHelper.SetValue(inspectionStepElement, "LinkedCameraId", inspectionStep.LinkedCameraId.ToString());
            XmlHelper.SetValue(inspectionStepElement, "StepTypeNum", inspectionStep.StepType.ToString());
            XmlHelper.SetValue(inspectionStepElement, "FovRect", inspectionStep.FovRect);
            XmlHelper.SetValue(inspectionStepElement, "BlockFastStepChange", inspectionStep.BlockFastStepChange.ToString());

            foreach (TargetGroup targetGroup in inspectionStep.TargetGroupList)
            {
                XmlElement targetGroupElement = xmlDocument.CreateElement("", "TargetGroup", "");
                inspectionStepElement.AppendChild(targetGroupElement);

                WriteTargetGroup(targetGroupElement, targetGroup);
            }

            XmlElement lightParamSetElement = xmlDocument.CreateElement("", "LightParamSet", "");
            inspectionStepElement.AppendChild(lightParamSetElement);
            inspectionStep.LightParamSet.Save(lightParamSetElement);
        }

        public override void WriteTargetGroup(XmlElement targetGroupElement, TargetGroup targetGroup)
        {
            XmlDocument xmlDocument = targetGroupElement.OwnerDocument;
            XmlHelper.SetValue(targetGroupElement, "GroupId", targetGroup.GroupId.ToString());
            XmlHelper.SetValue(targetGroupElement, "LightParamSource", targetGroup.LightParamSource.ToString());

            XmlElement fiducialSetElement = xmlDocument.CreateElement("", "FiducialSet", "");
            targetGroupElement.AppendChild(fiducialSetElement);

            WriteFiducialSet(fiducialSetElement, targetGroup.FiducialSet);

            foreach (Target target in targetGroup.TargetList)
            {
                XmlElement targetElement = xmlDocument.CreateElement("", "Target", "");
                targetGroupElement.AppendChild(targetElement);

                WriteTarget(targetElement, target);
            }

            if (targetGroup.TransformDataList != null)
            {
                foreach (TransformData transformData in targetGroup.TransformDataList)
                {
                    XmlElement transformDataElement = xmlDocument.CreateElement("", "TransformData", "");
                    targetGroupElement.AppendChild(transformDataElement);

                    transformData.SaveData(transformDataElement);
                }
            }
            
            XmlElement lightParamSetElement = xmlDocument.CreateElement("", "LightParamSet", "");
            targetGroupElement.AppendChild(lightParamSetElement);
            targetGroup.LightParamSet.Save(lightParamSetElement);
        }

        public override void WriteVisionProbe(XmlElement probeElement, VisionProbe visionProbe)
        {
            XmlDocument xmlDocument = probeElement.OwnerDocument;
            XmlHelper.SetValue(probeElement, "WorldRegion", visionProbe.WorldRegion);

            if (visionProbe.MaskFigures.FigureExist)
            {
                XmlElement maskFiguresElement = probeElement.OwnerDocument.CreateElement("", "MaskFigures", "");
                probeElement.AppendChild(maskFiguresElement);

                visionProbe.MaskFigures.Save(maskFiguresElement);
            }

            XmlElement algorithmElement = xmlDocument.CreateElement("", "Algorithm", "");
            probeElement.AppendChild(algorithmElement);
            AlgorithmArchiver.SaveAlgorithm(algorithmElement, visionProbe.InspAlgorithm);
        }
    }
}
