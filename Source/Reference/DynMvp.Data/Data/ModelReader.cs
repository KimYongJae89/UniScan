using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing.Imaging;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using DynMvp.Devices.Dio;
using DynMvp.Devices.Comm;
using DynMvp.Devices.Daq;
using DynMvp.Devices.MotionController;
using DynMvp.Devices;
using System.Drawing;

namespace DynMvp.Data
{
    public delegate ProbeCustomInfo CreateCustomInfoDelegate();

    public class ModelReaderBuilder
    {
        public static ModelReader Create(string modelPath)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(modelPath);

            XmlElement modelElement = xmlDocument.DocumentElement;

            float fileVersion = Convert.ToSingle(XmlHelper.GetValue(modelElement, "Version", "2.0"));

            ModelReader modelReader;
            if (fileVersion < 2)
            {
                modelReader = new ModelReaderVer1();
            }
            else
            {
                modelReader = new ModelReaderVer2();
            }

            return modelReader;
        }
    }

    public abstract class ModelReader
    {
        protected int numInspectionStep;
        protected int numCamera;
        protected int numLight;
        protected int numLightType;

        protected AlgorithmArchiver algorithmArchiver;

        protected StringBuilder errorLog = new StringBuilder();

        public void Initialize(AlgorithmArchiver algorithmArchiver, int numInspectionStep, int numCamera, int numLight, int numLightType)
        {
            this.algorithmArchiver = algorithmArchiver;
            this.numInspectionStep = numInspectionStep;
            this.numCamera = numCamera;
            this.numLight = numLight;
            this.numLightType = numLightType;
        }

        public void Load(Model model, string modelPath, IReportProgress reportProgress)
        {
            model.Setup(numCamera, numLight, numLightType);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(modelPath);

            XmlElement modelElement = xmlDocument.DocumentElement;

            model.FileVersion = Convert.ToSingle(XmlHelper.GetValue(modelElement, "Version", "1.0"));
            // 모델의 LightParamSet을 불러온다.
            XmlElement lightParamSetElement = modelElement["LightParamSet"];
            if (lightParamSetElement != null)
            {
                model.LightParamSet.Load(lightParamSetElement);
            }

            LoadImageBufferFileList(model, modelElement);

            
            LoadTargets(model, modelElement, reportProgress);

            XmlElement fiducialSetListElement = modelElement["FiducialSetList"];
            if (fiducialSetListElement != null)
            {
                foreach (XmlElement fiducialSetElement in fiducialSetListElement)
                {
                    if (fiducialSetElement.Name == "FiducialSet")
                    {
                        FiducialSet fiducialSet = new FiducialSet();
                        LoadFiducialSet(fiducialSet, fiducialSetElement);
                    }
                }
            }

            model.LoadModel(modelElement);
        }

        protected void LoadImageBufferFileList(Model model, XmlElement modelElement)
        {
            XmlElement imageBufferFileListElement = modelElement["ImageBufferFileList"];
            if (imageBufferFileListElement != null)
            {
                model.ImageBufferPathList.Clear();
                foreach (XmlElement imageBufferFileElement in imageBufferFileListElement)
                {
                    model.ImageBufferPathList.Add(imageBufferFileElement.InnerText);
                }
            }
        }

        protected void LoadFiducialSet(FiducialSet fiducialSet, XmlElement fiducialSetElement)
        {
            fiducialSet.Index = Convert.ToInt32(XmlHelper.GetValue(fiducialSetElement, "Index", "0"));

            foreach (XmlElement fiducialElement in fiducialSetElement)
            {
                if (fiducialElement.Name == "Fiducial")
                {
                    FiducialInfo fiducialInfo = new FiducialInfo();
                    fiducialInfo.Type = (FiducialSetType)Enum.Parse(typeof(FiducialSetType), XmlHelper.GetValue(fiducialSetElement, "Type", "TargetGroup"));
                    fiducialInfo.StepName = XmlHelper.GetValue(fiducialSetElement, "StepName", "");
                    fiducialInfo.TargetGroupId = Convert.ToInt32(XmlHelper.GetValue(fiducialSetElement, "TargetGroupId", "0"));
                    fiducialInfo.TargetId = Convert.ToInt32(XmlHelper.GetValue(fiducialElement, "TargetId", "1"));
                    fiducialInfo.ProbeId = Convert.ToInt32(XmlHelper.GetValue(fiducialElement, "ProbeId", "1"));

                    fiducialSet.FiducialInfoList.Add(fiducialInfo);
                }
            }
        }

        protected void LoadTarget(Target target, XmlElement targetElement)
        {
            if (targetElement == null)
                return;

            target.Id = Convert.ToInt32(XmlHelper.GetValue(targetElement, "Id", "0"));
            target.ImageEncodedString = XmlHelper.GetValue(targetElement, "Image", "");
            target.Name = XmlHelper.GetValue(targetElement, "Name", "");
            target.ModuleNo = Convert.ToInt32(XmlHelper.GetValue(targetElement, "ModuleNo", "0"));
            target.TypeName = XmlHelper.GetValue(targetElement, "Type", "");
            target.UseInspection = Convert.ToBoolean(XmlHelper.GetValue(targetElement, "UseInspection", "True"));
            target.InspectionLogicType = (InspectionLogicType)Enum.Parse(typeof(InspectionLogicType), XmlHelper.GetValue(targetElement, "InspectionLogicType", "And"));
            target.LightTypeIndex = Convert.ToInt32(XmlHelper.GetValue(targetElement, "LightTypeIndex", "0"));

            RotatedRect targetRegion = new RotatedRect();
            XmlHelper.GetValue(targetElement, "Region", ref targetRegion);
            target.BaseRegion = targetRegion;

            foreach (XmlElement probeElement in targetElement)
            {
                if (probeElement.Name == "Probe")
                {
                    string probeTypeStr = XmlHelper.GetValue(probeElement, "ProbeType", "");
                    if (probeTypeStr == "")
                        throw new InvalidDataException();

                    ProbeType probeType = (ProbeType)Enum.Parse(typeof(ProbeType), probeTypeStr);

                    try
                    {
                        Probe probe = ProbeFactory.Create(probeType);

                        LoadProbe(probe, probeElement);

                        target.AddProbe(probe);
                    }
                    catch (InvalidDataException ex)
                    {
                        errorLog.AppendFormat("Some error occurred on Target {0}\n", target.FullId);
                        errorLog.AppendLine(ex.Message);
                    }
                }
            }
        }

        protected void LoadProbe(Probe probe, XmlElement probeElement)
        {
            if (probeElement == null)
                return;

            probe.Id = Convert.ToInt32(XmlHelper.GetValue(probeElement, "Id", "0"));
            if (probe.Id == 0)
                throw new InvalidDataException("Probe ID 0 is invalid");

            probe.Name = XmlHelper.GetValue(probeElement, "Name", "");
            probe.ActAsFiducialProbe = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "ActAsFiducialProbe", "False"));
            probe.ActAsCalibrationProbe = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "ActAsCalibrationProbe", "False"));
            probe.FiducialProbeId = Convert.ToInt32(XmlHelper.GetValue(probeElement, "FiducialProbeId", "0"));
            probe.InverseResult = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "InverseResult", "False"));
            probe.ModelVerification = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "ModelVerification", "False"));
            probe.StepBlocker = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "StepBlocker", "False"));
            probe.LightTypeIndex = Convert.ToInt32(XmlHelper.GetValue(probeElement, "LightTypeIndex", "0"));

            if (probe.FigureProperty != null)
                probe.FigureProperty.Load(probeElement);

            //probe.IsRectFigure = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "IsRectFigure","false"));
            //probe.PointList.Add(new System.Drawing.PointF(0.0f, 0.5f));
            //probe.PointList.Add(new System.Drawing.PointF(1.0f, 0.5f));

            RotatedRect probeRegion = new RotatedRect();
            XmlHelper.GetValue(probeElement, "Region", ref probeRegion);
            probe.BaseRegion = probeRegion;

            switch (probe.ProbeType)
            {
                case ProbeType.Vision:
                    LoadVisionProbe((VisionProbe)probe, probeElement);
                    break;
                case ProbeType.Io:
                    LoadIoProbe((IoProbe)probe, probeElement);
                    break;
                case ProbeType.Serial:
                    LoadSerialProbe((SerialProbe)probe, probeElement);
                    break;
                case ProbeType.Tension:
                    LoadSerialProbe((TensionSerialProbe)probe, probeElement);
                    break;
                case ProbeType.Daq:
                    LoadDaqProbe((DaqProbe)probe, probeElement);
                    break;
                case ProbeType.Marker:
                    LoadMarkerProbe((MarkerProbe)probe, probeElement);
                    break;
                default:
                    throw new InvalidTypeException(String.Format("Invalid probe type : {0}", probe.ProbeType.ToString()));
            }

        }

        protected abstract void LoadVisionProbe(VisionProbe visionProbe, XmlElement probeElement);

        protected void LoadMarkerProbe(MarkerProbe markerProbe, XmlElement probeElement)
        {
            markerProbe.MarkerType = (MarkerType)Enum.Parse(typeof(MarkerType), XmlHelper.GetValue(probeElement, "MarkerType", MarkerType.MergeSource.ToString()));
            markerProbe.MergeSourceId = XmlHelper.GetValue(probeElement, "MergeSourceId", "");

            Point3d mergeOffset = new Point3d();
            XmlHelper.GetValue(probeElement, "MergeOffset", ref mergeOffset);
            markerProbe.MergeOffset = mergeOffset;
        }

        protected void LoadIoProbe(IoProbe ioProbe, XmlElement probeElement)
        {
            ioProbe.DigitalIoName = XmlHelper.GetValue(probeElement, "DigitalIoName", "Default");
            ioProbe.PortNo = Convert.ToInt32(XmlHelper.GetValue(probeElement, "PortNo", "0"));
        }

        protected void LoadSerialProbe(SerialProbe serialprobe, XmlElement probeElement)
        {
            RotatedRect worldRegion = new RotatedRect();
            XmlHelper.GetValue(probeElement, "WorldRegion", ref worldRegion);
            serialprobe.WorldRegion = worldRegion;

            serialprobe.PortName = XmlHelper.GetValue(probeElement, "PortName", "");
            serialprobe.UpperValue = Convert.ToSingle(XmlHelper.GetValue(probeElement, "UpperValue", "0.0"));
            serialprobe.LowerValue = Convert.ToSingle(XmlHelper.GetValue(probeElement, "LowerValue", "0.0"));
            serialprobe.NumSerialReading = Convert.ToInt32(XmlHelper.GetValue(probeElement, "NumSerialReading", "0"));
            serialprobe.InspectionSerialPort = (SerialPortEx)SerialPortManager.Instance().GetSerialPort(serialprobe.PortName);
            if(serialprobe is TensionSerialProbe)
            {
                ((TensionSerialProbe)serialprobe).TensionFilePath = XmlHelper.GetValue(probeElement, "TensionFilePath", "");
                ((TensionSerialProbe)serialprobe).UnitType = (TensionUnitType)Enum.Parse(typeof(TensionUnitType), XmlHelper.GetValue(probeElement, "UnitType", "Newton"));
            }
        }

        protected void LoadDaqProbe(DaqProbe daqProbe, XmlElement probeElement)
        {
            string channelName = XmlHelper.GetValue(probeElement, "ChannelName", "");
            daqProbe.DaqChannel = DaqChannelManager.Instance().GetDaqChannel(channelName);

            daqProbe.UpperValue = Convert.ToSingle(XmlHelper.GetValue(probeElement, "UpperValue", "0.0"));
            daqProbe.LowerValue = Convert.ToSingle(XmlHelper.GetValue(probeElement, "LowerValue", "0.0"));
            daqProbe.NumSample = Convert.ToInt32(XmlHelper.GetValue(probeElement, "NumSample", "100"));
            daqProbe.UseLocalScaleFactor = Convert.ToBoolean(XmlHelper.GetValue(probeElement, "UseLocalScaleFactor", "False"));
            daqProbe.LocalScaleFactor = Convert.ToSingle(XmlHelper.GetValue(probeElement, "LocalScaleFactor", "0.0"));
            daqProbe.ValueOffset = Convert.ToSingle(XmlHelper.GetValue(probeElement, "ValueOffset", "0.0"));
            daqProbe.MeasureType = (DaqMeasureType)Enum.Parse(typeof(DaqMeasureType), XmlHelper.GetValue(probeElement, "MeasureType", "Absolute"));
            daqProbe.FilterType = (DaqFilterType)Enum.Parse(typeof(DaqFilterType), XmlHelper.GetValue(probeElement, "FilterType", "Average"));
            daqProbe.Target1Name = XmlHelper.GetValue(probeElement, "Target1Name", "");
            daqProbe.Target2Name = XmlHelper.GetValue(probeElement, "Target2Name", "");
        }

        public abstract void LoadTargets(Model model, XmlElement modelElement, IReportProgress reportProgress);
        public abstract void LoadTargetGroup(int inspectionStep, TargetGroup targetGroup, XmlElement targetGroupElement);
        public abstract void LoadInspectionStep(InspectionStep inspectionStep, int numLight, XmlElement inspectionStepElement);
        public abstract void LoadTargetGroup(TargetGroup targetGroup, XmlElement targetGroupElement);
    }

    public class ModelReaderVer1 : ModelReader
    {
        public override void LoadTargets(Model model, XmlElement modelElement, IReportProgress reportProgress)
        {
            int count = 0;

            foreach (XmlElement targetGroupElement in modelElement)
            {
                if (targetGroupElement.Name == "TargetGroup")
                {
                    for (int i = 0; i < numInspectionStep; i++)
                    {
                        InspectionStep inspectionStep = model.GetInspectionStep(i);
                        if (inspectionStep == null)
                        {
                            inspectionStep = new InspectionStep(i, numLight, numLightType);
                            inspectionStep.OwnerModel = model;

                            model.AddInspectionStep(inspectionStep);
                        }

                        TargetGroup targetGroup = new TargetGroup(0, numLight, numLightType);
                        inspectionStep.AddTargetGroup(targetGroup);

                        LoadTargetGroup(i, targetGroup, targetGroupElement);
                    }

                    //if (reportProgress != null)
                    //    reportProgress.ReportProgress(count * 10, "");

                    count++;
                }
            }
        }

        public override void LoadTargetGroup(int inspectionStep, TargetGroup targetGroup, XmlElement targetGroupElement)
        {
            targetGroup.GroupId = Convert.ToInt32(XmlHelper.GetValue(targetGroupElement, "CamId", "0"));

            foreach (XmlElement subElement in targetGroupElement)
            {
                if (subElement.Name == "Target")
                {
                    int targetInspectionStep = Convert.ToInt32(XmlHelper.GetValue(subElement, "InspectionStep", "0"));

                    if (inspectionStep == targetInspectionStep)
                    {
                        Target target = new Target();
                        targetGroup.AddTarget(target);

                        LoadTarget(target, subElement);
                    }
                }
                else if (subElement.Name == "LightParam")
                {
                    int lightParamIndex = Convert.ToInt32(XmlHelper.GetValue(subElement, "Index", "0"));
                    if (inspectionStep == lightParamIndex)
                    {
                        targetGroup.LightParamSet.LightParamList[0].Load(subElement);
                    }
                }
            }

            LoadFiducialSet(targetGroup.FiducialSet, targetGroupElement["FiducialSet"]);
            targetGroup.FiducialSet.LinkFiducial(targetGroup);
        }

        protected override void LoadVisionProbe(VisionProbe visionProbe, XmlElement probeElement)
        {
            if (probeElement == null)
                return;

            XmlElement algorithmElement = probeElement["Algorithm"];

            Algorithm algorithm;
            algorithmArchiver.LoadAlgorithm(algorithmElement, out algorithm);
            visionProbe.InspAlgorithm = algorithm;

            visionProbe.InspAlgorithm.Param.SourceImageType = (ImageType)Enum.Parse(typeof(ImageType), XmlHelper.GetValue(probeElement, "SourceImageType", "Grey"));
            visionProbe.InspAlgorithm.Param.ImageBand = (ImageBandType)Enum.Parse(typeof(ImageBandType), XmlHelper.GetValue(probeElement, "ImageBand", "Luminance"));
        }

        public override void LoadInspectionStep(InspectionStep inspectionStep, int numLight, XmlElement inspectionStepElement)
        {
            throw new NotImplementedException();
        }

        public override void LoadTargetGroup(TargetGroup targetGroup, XmlElement targetGroupElement)
        {
            throw new NotImplementedException();
        }
    }

    class ModelReaderVer2 : ModelReader
    {
        public override void LoadTargets(Model model, XmlElement modelElement, IReportProgress reportProgress)
        {
            int count = 0;

            foreach (XmlElement inspectionStepElement in modelElement)
            {
                if (inspectionStepElement.Name == "InspectionStep")
                {
                    InspectionStep inspectionStep = new InspectionStep(count, numLight, numLightType);
                    inspectionStep.OwnerModel = model;

                    model.AddInspectionStep(inspectionStep);

                    LoadInspectionStep(inspectionStep, numLight, inspectionStepElement);

                    count++;
                }
            }
        }

        public override void LoadInspectionStep(InspectionStep inspectionStep, int numLight, XmlElement inspectionStepElement)
        {
            if (inspectionStepElement["AxisPosition"] != null)
            {
                AxisPosition basePosition = new AxisPosition();
                basePosition.GetValue(inspectionStepElement, "AxisPosition");

                inspectionStep.BasePosition = basePosition;
            }

            inspectionStep.Name = XmlHelper.GetValue(inspectionStepElement, "Name", "");
            inspectionStep.LinkedCameraId = Convert.ToInt32(XmlHelper.GetValue(inspectionStepElement, "LinkedCameraId", "-1"));
            inspectionStep.BlockFastStepChange = Convert.ToBoolean(XmlHelper.GetValue(inspectionStepElement, "BlockFastStepChange", "True"));
            string stepTypeNum = XmlHelper.GetValue(inspectionStepElement, "StepTypeNum", "");
            if (stepTypeNum != "")
            {
                inspectionStep.StepType = Convert.ToInt32(stepTypeNum);
            }
            else
            {
                string stepTypeName = XmlHelper.GetValue(inspectionStepElement, "StepType", "Normal");
                switch (stepTypeName)
                {
                    case "Normal": inspectionStep.StepType = 1; break;
                    case "Fiducial": inspectionStep.StepType = 2; break;
                    case "Tension": inspectionStep.StepType = 3; break;
                    default: inspectionStep.StepType = 0; break;
                }
            }

            RectangleF fovRect = new RectangleF();
            XmlHelper.GetValue(inspectionStepElement, "FovRect", ref fovRect);
            inspectionStep.FovRect = fovRect;

            XmlElement lightParamSetElement = inspectionStepElement["LightParamSet"];
            if (lightParamSetElement != null)
            {
                inspectionStep.LightParamSet.Load(lightParamSetElement);
            }

            foreach (XmlElement subElement in inspectionStepElement)
            {
                if (subElement.Name == "TargetGroup")
                {
                    TargetGroup targetGroup = inspectionStep.CreateTargetGroup();
                    LoadTargetGroup(targetGroup, subElement);
                }
            }
        }

        public override void LoadTargetGroup(TargetGroup targetGroup, XmlElement targetGroupElement)
        {
            targetGroup.GroupId = Convert.ToInt32(XmlHelper.GetValue(targetGroupElement, "GroupId", "0"));
            
            targetGroup.LightParamSource = (LightParamSource)Enum.Parse(typeof(LightParamSource), XmlHelper.GetValue(targetGroupElement, "LightParamSource", "Model"));
            
            XmlElement lightParamSetElement = targetGroupElement["LightParamSet"];
            if (lightParamSetElement != null)
            {
                targetGroup.LightParamSet.Load(lightParamSetElement);
            }

            int lightParamIndex = 0;
            foreach (XmlElement subElement in targetGroupElement)
            {
                if (subElement.Name == "Target")
                {
                    Target target = new Target();
                    targetGroup.AddTarget(target);

                    LoadTarget(target, subElement);
                }
                else if (subElement.Name == "LightParam")
                {
                    if (targetGroup.LightParamSet.LightParamList.Count > lightParamIndex)
                    {
                        targetGroup.LightParamSet.LightParamList[lightParamIndex].Load(subElement);
                        lightParamIndex++;
                    }
                }
                else if (subElement.Name == "TransformData")
                {
                    TransformData transformData = new TransformData();
                    transformData.LoadData(subElement);

                    if (targetGroup.TransformDataList == null)
                        targetGroup.TransformDataList = new TransformDataList();

                    targetGroup.TransformDataList.Add(transformData);
                }
            }

            LoadFiducialSet(targetGroup.FiducialSet, targetGroupElement["FiducialSet"]);
            targetGroup.FiducialSet.LinkFiducial(targetGroup);       
        }

        protected override void LoadVisionProbe(VisionProbe visionProbe, XmlElement probeElement)
        {
            if (probeElement == null)
                return;

            RotatedRect worldRegion = new RotatedRect();
            XmlHelper.GetValue(probeElement, "WorldRegion", ref worldRegion);
            visionProbe.WorldRegion = worldRegion;

            XmlElement maskFiguresElement = probeElement["MaskFigures"];
            if (maskFiguresElement != null)
            {
                visionProbe.MaskFigures.Load(maskFiguresElement);
            }

            XmlElement algorithmElement = probeElement["Algorithm"];
            if (algorithmElement == null)
                return;

            Algorithm algorithm;
            algorithmArchiver.LoadAlgorithm(algorithmElement, out algorithm);
            visionProbe.InspAlgorithm = algorithm;
        }

        public override void LoadTargetGroup(int inspectionStep, TargetGroup targetGroup, XmlElement targetGroupElement)
        {
            throw new NotImplementedException();
        }
    }
}
