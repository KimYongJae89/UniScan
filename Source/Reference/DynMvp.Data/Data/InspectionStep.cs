using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Drawing;

using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.MotionController;
using DynMvp.UI;

namespace DynMvp.Data
{
    public enum DefaultStepType
    {
        None = 0,
        Normal = 1,
        Fiducial = 2,
    }

    public class InspectionStep
    {
        public const int StepAll = 0xffff;

        private List<TargetGroup> targetGroupList = new List<TargetGroup>();
        public List<TargetGroup> TargetGroupList
        {
            get { return targetGroupList; }
        }

        InspectionStep reference;
        public InspectionStep Reference
        {
            get { return reference; }
            set { reference = value; }
        }

        int stepType;
        public int StepType
        {
            get { return stepType; }
            set { stepType = value; }
        }


        Model ownerModel;
        public Model OwnerModel
        {
            get { return ownerModel; }
            set { ownerModel = value; }
        }

        public string StepName
        {
            get
            {
                if (ownerModel != null && ownerModel.IsMasterModel)
                    return String.Format("M{0:00}", stepNo);

                return stepNo.ToString("00");
            }
        }
        
        private int stepNo;
        public int StepNo
        {
            get { return stepNo; }
            set { stepNo = value; }
        }

        public int NumTargets
        {
            get
            {
                int numTargets = 0;
                foreach (TargetGroup targetGroup in targetGroupList)
                {
                    numTargets += targetGroup.TargetList.Count;
                }

                return numTargets;
            }
        }

        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private RectangleF fovRect = RectangleF.Empty;
        public RectangleF FovRect
        {
            get { return fovRect; }
            set { fovRect = value; }
        }

        private AxisPosition basePosition = null;
        public AxisPosition BasePosition
        {
            get { return basePosition; }
            set
            {
                basePosition = value;
                alignedPosition = basePosition.Clone();
            }
        }

        // 마지막 보정 마크 스텝, H/W 구동 Step 등에서 이 플랙을 사용.
        // 카메라만 사용하는 스텝에서는 false를 만들어 Grab과 검사를 동시에 수행.
        private bool blockFastStepChange = true;
        public bool BlockFastStepChange
        {
            get { return blockFastStepChange; }
            set { blockFastStepChange = value; }
        }

        private AxisPosition alignedPosition = null;
        public AxisPosition AlignedPosition
        {
            get { return alignedPosition; }
            set { alignedPosition = value; }
        }

        // 특정 Step에서는 특정 카메라만 동작하도록 할 경우
        // Gsmatt 에서 사용됨.
        private int linkedCameraId = -1;
        public int LinkedCameraId
        {
            get { return linkedCameraId; }
            set { linkedCameraId = value; }
        }

        private LightParamSet lightParamSet = new LightParamSet();
        public LightParamSet LightParamSet
        {
            get { return lightParamSet; }
            set { lightParamSet = value; }
        }

        private bool useInspectionStepLight = false;
        public bool UseInspectionStepLight
        {
            get { return useInspectionStepLight; }
            set { useInspectionStepLight = value; }
        }

        public TargetGroup LastTargetGroup
        {
            get 
            {
                if (targetGroupList.Count > 0)
                    return targetGroupList.Last();

                return null;
            }
        }

        public InspectionStep(int stepNo, int numLight, int numLightType)
        {
            this.stepType = (int)DefaultStepType.Normal;
            this.stepNo = stepNo;

            lightParamSet.Initialize(numLight, numLightType);
        }

        public TargetGroup CreateTargetGroup()
        {
            TargetGroup newTargetGroup = new TargetGroup(0, lightParamSet.NumLight, lightParamSet.NumLightType);

            AddTargetGroup(newTargetGroup);

            return newTargetGroup;
        }

        public void CreateTargetGroups(int numGroup)
        {
            for (int i = 0; i < numGroup; i++)
            {
                TargetGroup newTargetGroup = new TargetGroup(i, lightParamSet.NumLight, lightParamSet.NumLightType);
                newTargetGroup.InspectionStep = this;
                if (useInspectionStepLight == true)
                    newTargetGroup.LightParamSource = LightParamSource.InspectionStep;

                targetGroupList.Add(newTargetGroup);
            }
        }

        public void Copy(InspectionStep srcInspectionStep)
        {
            Clear();

            lightParamSet = srcInspectionStep.LightParamSet.Clone();

            foreach (TargetGroup targetGroup in srcInspectionStep.TargetGroupList)
            {
                TargetGroup newTargetGroup = (TargetGroup)targetGroup.Clone();
                newTargetGroup.InspectionStep = this;
                targetGroupList.Add(newTargetGroup);
            }
        }

        public void SyncParam(Probe probe, IProbeFilter probeFilter)
        {
            for (int i=0; i<targetGroupList.Count; i++)
            {
                targetGroupList[i].SyncParam(probe, probeFilter);
            }
        }

        public void Clear()
        {
            foreach (TargetGroup targetGroup in targetGroupList)
                targetGroup.Clear();

            targetGroupList.Clear();  
        }

        public IEnumerator<TargetGroup> GetEnumerator()
        {
            return targetGroupList.GetEnumerator();
        }

        public void Add(System.Object obj)
        {
            TargetGroup targetGroup = (TargetGroup)obj;
            AddTargetGroup(targetGroup);
        }

        public void AddTargetGroup(TargetGroup targetGroup)
        {
            targetGroup.GroupId = targetGroupList.Count;
            if (useInspectionStepLight == true)
                targetGroup.LightParamSource = LightParamSource.InspectionStep;
            targetGroup.InspectionStep = this;

            targetGroupList.Add(targetGroup);
        }

        public void RemoveTargetGroup(TargetGroup targetGroup)
        {
            targetGroupList.Remove(targetGroup);
        }

        public TargetGroup GetTargetGroup(int camId, bool fCreateOnEmpty = false)
        {
            foreach (TargetGroup targetGroup in targetGroupList)
            {
                if (targetGroup.GroupId == camId)
                    return targetGroup;
            }

            if (fCreateOnEmpty)
            {
                TargetGroup targetGroup = new TargetGroup(camId, lightParamSet.NumLight, lightParamSet.NumLightType);
                AddTargetGroup(targetGroup);
                return targetGroup;
            }

            return null;
        }

        public void UpdateImageBuffer(ImageBuffer imageBuffer)
        {
            //int lightTypeIndex = 0;
            //foreach (LightParam lightParam in lightParamSet.LightParamList)
            //{
            //    foreach (TargetGroup targetGroup in targetGroupList)
            //    {
            //        if (targetGroup.UseTargetGroupLight == false)
            //        {
            //            ImageBuffer2dItem imageCell2d = imageBuffer.GetImageBuffer2dItem(targetGroup.GroupId, lightTypeIndex);
            //            if (imageCell2d != null)
            //            {
            //                if (imageCell2d.Image != null)
            //                    imageCell2d.Image.Clear();
            //                imageCell2d.LightParam = lightParam;
            //            }

            //            lightTypeIndex++;
            //        }
            //    }
            //}

            foreach (TargetGroup targetGroup in targetGroupList)
            {
                targetGroup.UpdateImageBuffer(imageBuffer);

                //LightParamSet lightParamSet = targetGroup.GetLightParamSet();

                //if (targetGroup.UseTargetGroupLight == true)
                //{
                //    targetGroup.UpdateImageBuffer(imageBuffer);
                //}
                //else
                //{
                //    ImageBuffer3dItem imageCell3d = imageBuffer.GetImageBuffer3dItem(targetGroup.GroupId);
                //    if (imageCell3d != null)
                //    {
                //        imageCell3d.ExposureTime3dUs = lightParamSet.LightParam3d.ExposureTime3dUs;
                //        imageCell3d.TransformDataList = targetGroup.TransformDataList;
                //    }
                //}
            }
        }

        public virtual void PrepareInspection()
        {
            foreach (TargetGroup targetGroup in targetGroupList)
            {
                targetGroup.PrepareInspection();
            }
        }

        public virtual void PostInspection()
        {

        }

        public Probe GetProbe(string probeFullIdOrName)
        {
            foreach (TargetGroup targetGroup in targetGroupList)
            {
                Probe probe = targetGroup.GetProbe(probeFullIdOrName);
                if (probe != null)
                    return probe;
            }

            return null;
        }

        public Probe GetProbe(int targetGroupId, int targetId, int probeId)
        {
            TargetGroup targetGroup = targetGroupList.Find(f => f.GroupId == targetGroupId);

            if (targetGroup == null)
                return null;

            return targetGroup.GetProbe(targetId, probeId);
        }

        public Target GetTarget(string targetFullIdOrName)
        {
            foreach (TargetGroup targetGroup in targetGroupList)
            {
                Target target = targetGroup.GetTarget(targetFullIdOrName);
                if (target != null)
                    return target;
            }

            return null;
        }

        public void GetTargets(List<Target> targetList)
        {
            foreach (TargetGroup targetGroup in targetGroupList)
            {
                targetGroup.GetTargets(targetList);
            }
        }

        public void GetTargetTypes(List<string> targetTypeList)
        {
            foreach (TargetGroup targetGroup in targetGroupList)
            {
                targetGroup.GetTargetTypes(targetTypeList);
            }
        }

        public void GetProbes(List<Probe> probeList)
        {
            foreach (TargetGroup targetGroup in targetGroupList)
            {
                targetGroup.GetProbes(probeList);
            }
        }

        public int GetNumTarget()
        {
            List<Target> targetList = new List<Target>();
            GetTargets(targetList);

            return targetList.Count;
        }

        public bool IsEmpty()
        {
            foreach (TargetGroup targetGroup in targetGroupList)
            {
                if (targetGroup.IsEmpty() == true)
                    return true;
            }

            return false;
        }

        public void CreateObjectFigures(FigureGroup figureGroup)
        {
            foreach (TargetGroup targetGroup in targetGroupList)
            {
                targetGroup.CreateObjectFigures(figureGroup);
            }
        }

        internal void AddSchemaFigure(Schema schema)
        {
            RectangleFigure rectangleFigure = new RectangleFigure(fovRect, (Pen)schema.DefaultFigureProperty.Pen.Clone(), null);
            rectangleFigure.Id = "rectangle";

            SchemaFigure schemaFigure = new SchemaFigure();
            schemaFigure.AddFigure(rectangleFigure);
            schemaFigure.Tag = StepName;

            schema.AddFigure(schemaFigure);

            foreach (TargetGroup targetGroup in targetGroupList)
            {
                targetGroup.AddSchemaFigure(schema);
            }
        }

        internal void AlignPosition(PositionAligner positionAligner)
        {
            if (basePosition == null)
                return;

            if (stepType == (int)DefaultStepType.Fiducial)
            {
                alignedPosition[0] = basePosition[0];
                alignedPosition[1] = basePosition[1];
            }
            else
            {
                Debug.Assert(positionAligner != null);

                PointF newPos = positionAligner.Align(basePosition.ToPointF());
                alignedPosition[0] = newPos.X;
                alignedPosition[1] = newPos.Y;

                foreach (TargetGroup targetGroup in targetGroupList)
                    targetGroup.AlignPosition(positionAligner);
            }
        }

        public int GetLightTypeIndex(int targetGroupIndex)
        {
            if (targetGroupList.Count > targetGroupIndex)
                return targetGroupList[targetGroupIndex].GetLightTypeIndex();

            return 0;
        }
    }
}
