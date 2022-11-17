using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Devices;
using DynMvp.Vision;
using DynMvp.InspData;
using DynMvp.Data.UI;

namespace DynMvp.Data
{
    public enum LightParamSource
    {
        Model,
        InspectionStep,
        TargetGroup
    }

    public class TargetGroup : ICloneable
    {
        private List<Target> targetList = new List<Target>();
        public List<Target> TargetList
        {
            get { return targetList; }
        }

        private int modelFiducialId = -1;
        public int ModelFiducialId
        {
            get { return modelFiducialId; }
            set { modelFiducialId = value; }
        }

        private FiducialSet fiducialSet = new FiducialSet();
        public FiducialSet FiducialSet
        {
            get { return fiducialSet; }
        }

        Probe localCalibrationProbe;
        public Probe LocalCalibrationProbe
        {
            get { return localCalibrationProbe; }
            set { localCalibrationProbe = value; }
        }

        private InspectionStep inspectionStep;
        public InspectionStep InspectionStep
        {
            get { return inspectionStep; }
            set { inspectionStep = value; }
        }

        private int groupId;
        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        TransformDataList transformDataList;
        public TransformDataList TransformDataList
        {
            get { return transformDataList; }
            set { transformDataList = value; }
        }


        public string FullId
        {
            get
            {
                return String.Format("{0}.{1:00}", inspectionStep.StepName, groupId);
            }
        }

        private LightParamSet lightParamSet = new LightParamSet();
        public LightParamSet LightParamSet
        {
            get { return lightParamSet; }
            set { lightParamSet = value; }
        }

        public TargetGroup(int groupId, int numLight, int numLightType)
        {
            lightParamSet.Initialize(numLight, numLightType);
            this.GroupId = groupId;
        }

        private LightParamSource lightParamSource = LightParamSource.Model;
        public LightParamSource LightParamSource
        {
            get { return lightParamSource; }
            set { lightParamSource = value; }
        }

        public IEnumerator<Target> GetEnumerator()
        {
            return targetList.GetEnumerator();
        }

        public void Clear()
        {
            foreach (Target target in targetList)
                target.Clear();
            targetList.Clear();
        }

        public Object Clone()
        {
            TargetGroup targetGroup = new TargetGroup(0, lightParamSet.NumLight, lightParamSet.NumLightType);
            targetGroup.Copy(this);

            return targetGroup;
        }

        public void Copy(TargetGroup srcTargetGroup)
        {
            groupId = srcTargetGroup.GroupId;
            inspectionStep = null;
            fiducialSet = (FiducialSet)srcTargetGroup.fiducialSet.Clone();

            if (localCalibrationProbe != null)
                localCalibrationProbe = (VisionProbe)srcTargetGroup.localCalibrationProbe.Clone();

            lightParamSet = srcTargetGroup.LightParamSet.Clone();

            foreach (Target target in srcTargetGroup.targetList)
            {
                AddTarget((Target)target.Clone());
            }
        }

        public void SyncParam(Probe probe, IProbeFilter probeFilter)
        {
            for (int i = 0; i < targetList.Count; i++)
            {
                targetList[i].SyncParam(probe, probeFilter);
            }
        }

        public LightParamSet GetLightParamSet()
        {
            switch (lightParamSource)
            {
                case LightParamSource.Model:
                    return inspectionStep.OwnerModel.LightParamSet;
                case LightParamSource.InspectionStep:
                    return inspectionStep.LightParamSet;
                default:
                case LightParamSource.TargetGroup:
                    return lightParamSet;
            }
        }

        public void AddTarget(Target target)
        {
            target.Id = CreateTargetId();
            target.Name = String.Format("Target{0}", target.Id);
            target.TargetGroup = this;
            targetList.Add(target);
        }

        public Target CreateTarget()
        {
            Target target = new Target();

            target.Id = CreateTargetId();
            target.Name = String.Format("Target{0}", target.Id);
            target.TargetGroup = this;
            targetList.Add(target);

            return target;
        }

        public int GetNumTarget()
        {
            return targetList.Count;
        }

        private int CreateTargetId()
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                if (GetTarget(i) == null)
                    return i;
            }

            throw new TooManyItemsException();
        }

        public void PrepareInspection()
        {
            foreach (Target target in targetList)
            {
                target.PrepareInspection();
            }
        }

        public void RemoveTarget(Target target)
        {
            target.RemoveFiducial();
            targetList.Remove(target);


        }

        public Target GetTarget(string targetFullIdOrName)
        {
            foreach (Target target in targetList)
            {
                if (target.FullId == targetFullIdOrName || target.Name == targetFullIdOrName)
                    return target;
            }

            return null;
        }

        public Target GetTarget(int id)
        {
            foreach (Target target in targetList)
            {
                if (target.Id == id)
                    return target;
            }

            return null;
        }

        public Probe GetProbe(string probeFullIdOrName)
        {
            foreach (Target target in targetList)
            {
                Probe probe = target.GetProbe(probeFullIdOrName);
                if (probe != null)
                    return probe;
            }

            return null;
        }

        public Probe GetProbe(int targetId, int probeId)
        {
            Target target = targetList.Find(f => f.Id == targetId);
            if (target == null)
                return null;

            return target.GetProbe(probeId);
        }



        public void GetTargets(List<Target> targetList)
        {
            foreach (Target target in this.targetList)
            {
                targetList.Add(target);
            }
        }

        public void GetTargetTypes(List<string> targetTypeList)
        {
            foreach (Target target in this.targetList)
            {
                if (String.IsNullOrEmpty(target.TypeName))
                    continue;

                int index = targetTypeList.IndexOf(target.TypeName);
                if (index == -1)
                    targetTypeList.Add(target.TypeName);
            }
        }

        public void GetProbes(List<Probe> probeList)
        {
            foreach (Target target in this.targetList)
            {
                target.GetProbes(probeList);
            }
        }

        public List<Target> GetTargetList()
        {
            return targetList;
        }

        // 이미지 버퍼의 조명 설정을 검사 단계에 맞춰 변경해 주는 함수
        // 동작의 모호성이 상당히 높고, Target Group의 운영 상태에따라 적절하지 않은 상황이 있다. 변경 필요
        public void UpdateImageBuffer(ImageBuffer imageBuffer)
        {
            LightParamSet lightParamSet = GetLightParamSet();

            int lightTypeIndex = 0;
            foreach (LightParam lightParam in lightParamSet.LightParamList)
            {
                ImageBuffer2dItem imageCell2d = imageBuffer.GetImageBuffer2dItem(groupId, lightTypeIndex);
                if (imageCell2d != null)
                {
                    if (imageCell2d.Image != null)
                        imageCell2d.Image.Clear();
                    imageCell2d.LightParam = lightParam;
                }

                lightTypeIndex++;
            }

            ImageBuffer3dItem imageCell3d = imageBuffer.GetImageBuffer3dItem(groupId);
            if (imageCell3d != null)
            {
                imageCell3d.LightParam = lightParamSet.LightParam3d;
                imageCell3d.TransformDataList = TransformDataList;
            }
        }

        internal void CreateObjectFigures(FigureGroup figureGroup)
        {
            foreach (Target target in this.targetList)
            {
                target.CreateObjectFigures(figureGroup);
            }
        }

        public void Compute(InspParam inspParam, InspectionResult inspectionResult)
        {
            foreach (Target target in this.targetList)
            {
                target.Compute(inspParam, inspectionResult);
            }
        }

        public void Inspect(InspParam inspParam, InspectionResult inspectionResult)
        {
            LogHelper.Debug(LoggerType.Inspection, String.Format("Start Target Group Inspect. {0}", groupId));

            Stopwatch sw = new Stopwatch();
            sw.Start();

            foreach (Target target in targetList)
            {
                target.OnPreInspection();
            }

            LogHelper.Debug(LoggerType.Inspection, String.Format("Check Fiducial. {0}", groupId));

            if (modelFiducialId == -1)
            {
                if (inspParam.PositionAligner == null)
                {
                    LogHelper.Debug(LoggerType.Inspection, String.Format("Inspect Fiducial. {0}", groupId));

                    fiducialSet.Inspect(inspParam, inspectionResult);

                    inspParam.PositionAligner = fiducialSet.Calculate(inspectionResult);
                }
            }

            if (localCalibrationProbe != null)
            {
                if (localCalibrationProbe.Inspect(inspParam, inspectionResult) == false)
                {
                    return;
                }

                ProbeResult probeResult = inspectionResult.GetProbeResult(localCalibrationProbe);

                if (probeResult != null && probeResult is VisionProbeResult == true)
                {
                    VisionProbeResult calibrationProbeResult = (VisionProbeResult)probeResult;
                    Calibration cameraCalibration = (Calibration)calibrationProbeResult.GetResultValue("Calibration").Value;
                    if (cameraCalibration != null)
                        inspParam.CameraCalibration = cameraCalibration;
                }
            }

            MarkerProbe margeTarget = GetMarkerProbe(MarkerType.MergeTarget);
            if (margeTarget != null)
            {
                margeTarget.Inspect(inspParam, inspectionResult);
            }

            MarkerProbe margeSource = GetMarkerProbe(MarkerType.MergeSource);
            if (margeSource != null)
            {
                margeSource.Inspect(inspParam, inspectionResult);
            }

            LogHelper.Debug(LoggerType.Inspection, String.Format("Start Inspection with Target List. {0}", groupId));

            foreach (Target target in targetList)
            {
                if (fiducialSet.IsContained(target) == false)
                {
                    target.Inspect(inspParam, inspectionResult);
                }
                else
                {
                    LogHelper.Debug(LoggerType.Inspection, String.Format("Inspection Skipped. {0}", target.FullId));
                }
            }

            if (inspParam.SaveTargetGroupImage == true)
            {
                if (Directory.Exists(inspectionResult.ResultPath) == false)
                    Directory.CreateDirectory(inspectionResult.ResultPath);

                SaveImage(inspParam.DeviceImageSet, inspectionResult.ResultPath, inspParam.TargetGroupImageFormat);
            }

            LogHelper.Debug(LoggerType.Inspection, String.Format("TargetGroup Inspect Time : {0} ms", sw.ElapsedMilliseconds));
        }

        public void SaveImage(DeviceImageSet deviceImageSet, string resultPath, ImageFormat imageFormat)
        {
            if (deviceImageSet.Image3D != null)
            {
                string fileName = String.Format("{0}\\Image_C{1:00}_S{2:000}.3d", resultPath, groupId, inspectionStep.StepNo);

                if (File.Exists(fileName) == true)
                    File.Delete(fileName);

                deviceImageSet.Image3D.Save(fileName);
            }
            else
            {
                for (int lightIndex = 0; lightIndex < deviceImageSet.ImageList2D.Count; lightIndex++)
                {
                    ImageD grabImage = deviceImageSet.ImageList2D[lightIndex];
                    if (grabImage != null)
                    {
                        string fileName = String.Format("{0}\\{1}", resultPath, ImageBuffer.GetImage2dFileName(groupId, inspectionStep.StepNo, lightIndex, imageFormat));

                        grabImage.SaveImage(fileName, imageFormat);
                    }
                }
            }
        }

        internal void AddSchemaFigure(Schema schema)
        {
            foreach (Target target in targetList)
            {
                target.AddSchemaFigure(schema);
            }
        }

        public int GetLightTypeIndex()
        {
            foreach (Target target in targetList)
            {
                if (target.ProbeList.Count > 0)
                {
                    int lightTypeIndex = target.GetLightTypeIndex();
                    if (lightTypeIndex != -1)
                        return lightTypeIndex;
                }
            }

            return 0;
        }

        public MarkerProbe GetMarkerProbe(MarkerType markerType)
        {
            foreach (Target target in targetList)
            {
                MarkerProbe markerProbe = target.GetMarkerProbe(markerType);
                if (markerProbe != null)
                    return markerProbe;
            }

            return null;
        }

        public bool IsEmpty()
        {
            return targetList.Count == 0;
        }

        public FigureGroup AppendFigures(FigureGroup figureGroup, Pen pen, bool includeProbe = false)
        {
            foreach (Target target in targetList)
            {
                target.AppendFigures(figureGroup, pen, includeProbe);
            }

            return figureGroup;
        }

        internal void AlignPosition(PositionAligner positionAligner)
        {
            foreach (Target target in targetList)
            {
                target.AlignPosition(positionAligner);
            }
        }
    }
}
