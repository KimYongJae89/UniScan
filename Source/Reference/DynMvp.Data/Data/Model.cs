using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;
using System.IO;

using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Vision;
using DynMvp.UI;
using System.Drawing;
using System.Xml;
using System.ComponentModel;

namespace DynMvp.Data
{
    public class Model
    {
        [Browsable(false)]
        public string Name
        {
            get { return ModelDescription.Name; }
        }

        private string modelPath;

        [Browsable(false)]
        public string ModelPath
        {
            get { return modelPath; }
            set { modelPath = Path.GetFullPath(value); }
        }

        private float fileVersion;

        [Browsable(false)]
        public float FileVersion
        {
            get { return fileVersion; }
            set { fileVersion = value; }
        }

        protected bool modified;

        [Browsable(false)]
        public bool Modified
        {
            get { return modified; }
            set { modified = value; }
        }

        private bool isMasterModel;

        [Browsable(false)]
        public bool IsMasterModel
        {
            get { return isMasterModel; }
            set { isMasterModel = value; }
        }

        // Instance 생성용 임시 변수
        private int numCamera;

        [Browsable(false)]
        public int NumCamera
        {
            get { return numCamera; }
            set { numCamera = value; }
        }

        public string GetImagePath()
        {
            return Path.Combine(modelPath, "Image");
        }

        public string GetImageName(int cameraIdx, int stepIdx, int lightIdx, string extension = "bmp")
        {
            return string.Format("Image_C{0:00}_S{1:000}_L{2:00}.{3}", cameraIdx, stepIdx, lightIdx, extension);
        }

        public string GetImagePathName(int cameraIdx, int stepIdx, int lightIdx, string extension = "bmp")
        {
            return Path.Combine(GetImagePath(), GetImageName(cameraIdx, stepIdx, lightIdx, extension));
        }

        private LightParamSet lightParamSet = new LightParamSet();

        [Browsable(false)]
        public LightParamSet LightParamSet
        {
            get { return lightParamSet; }
            set { lightParamSet = value; }
        }

        [Browsable(false)]
        public int NumInspectionStep
        {
            get { return inspectionStepList.Count; }
        }

        public int numAxis;

        [Browsable(false)]
        public int NumAxis
        {
            get { return numAxis; }
            set { numAxis = value; }
        }

        private Schema modelSchema = new Schema();

        [Browsable(false)]
        public Schema ModelSchema
        {
            get { return modelSchema; }
            set { modelSchema = value; }
        }

        protected List<InspectionStep> inspectionStepList = new List<InspectionStep>();

        [Browsable(false)]
        public List<InspectionStep> InspectionStepList
        {
            get { return inspectionStepList; }
        }

        protected ModelDescription modelDescription;

        [Browsable(false)]
        public virtual ModelDescription ModelDescription
        {
            get { return modelDescription; }
            set { modelDescription = value; }
        }

        private List<string> imageBufferPathList = new List<string>();

        [Browsable(false)]
        public List<string> ImageBufferPathList
        {
            get { return imageBufferPathList; }
        }

        private List<FiducialSet> fiducialSetList = new List<FiducialSet>();

        [Browsable(false)]
        public List<FiducialSet> FiducialSetList
        {
            get { return fiducialSetList; }
        }

        Model masterModel;

        [Browsable(false)]
        public Model MasterModel
        {
            get { return masterModel; }
            set { masterModel = value; }
        }

        [Browsable(false)]
        public int NumLight
        {
            get
            {
                if (lightParamSet != null)
                    return lightParamSet.NumLight;
                return 0;
            }
        }

        [Browsable(false)]
        public int NumLightType
        {
            get
            {
                if (lightParamSet != null)
                    return lightParamSet.NumLightType;

                return 0;
            }
        }
        
        public virtual void Setup(int numCamera, int numLight, int numLightType)
        {
            this.numCamera = numCamera;
            lightParamSet.Initialize(numLight, numLightType);
            //CreateProduction();
        }

        public void Clear()
        {
            imageBufferPathList.Clear();

            foreach (InspectionStep inspectionStep in inspectionStepList)
                inspectionStep.Clear();

            inspectionStepList.Clear();
        }

        public IEnumerator<InspectionStep> GetEnumerator()
        {
            return inspectionStepList.GetEnumerator();
        }

        public virtual bool IsTaught()
        {
            int teachCount = 0;
            foreach (InspectionStep inspectionStep in inspectionStepList)
            {
                teachCount += inspectionStep.TargetGroupList.Count();
            }

            return teachCount > 0;
        }

        public bool IsEmpty()
        {
            if (GetInspectionStep(0) == null)
                return true;

            if (GetInspectionStep(0).GetTargetGroup(0) == null)
                return true;

            return GetInspectionStep(0).GetTargetGroup(0).GetNumTarget() == 0;
        }

        public void Add(System.Object obj)
        {
            inspectionStepList.Add((InspectionStep)obj);
        }

        public void AddInspectionStep(InspectionStep inspectionStep)
        {
            inspectionStepList.Add(inspectionStep);
        }

        public void RemoveInspectionStep(InspectionStep inspectionStepDel)
        {
            inspectionStepList.Remove(inspectionStepDel);

            int index = 0;
            foreach (InspectionStep inspectionStep in inspectionStepList)
            {
                inspectionStep.StepNo = index;
                index++;
            }
        }

        public void CreateInspectionStepList(int numInspectionStep)
        {
            inspectionStepList.Clear();

            for (int i = 0; i < numInspectionStep; i++)
            {
                InspectionStep inspectionStep = new InspectionStep(i, NumLight, NumLightType);
                inspectionStep.CreateTargetGroups(numCamera);
                inspectionStep.OwnerModel = this;

                inspectionStepList.Add(inspectionStep);
            }
        }

        public virtual InspectionStep CreateInspectionStep()
        {
            InspectionStep inspectionStep = new InspectionStep(inspectionStepList.Count, NumLight, NumLightType);
            inspectionStep.CreateTargetGroups(numCamera);
            inspectionStep.OwnerModel = this;

            inspectionStepList.Add(inspectionStep);

            return inspectionStep;
        }

        public void PrepareInspection()
        {
            foreach (InspectionStep inspectionStep in inspectionStepList)
            {
                inspectionStep.PrepareInspection();
            }
        }

        public void LinkFiducial()
        {
            foreach (FiducialSet fiducialSet in fiducialSetList)
            {
                fiducialSet.LinkFiducial(this);
            }
        }

        public InspectionStep GetInspectionStep(string stepName)
        {
            if (stepName[0] == 'M')
            {
                if (masterModel == null)
                    return null;

                int masterStepNo = Convert.ToInt32(stepName.Substring(1));

                return masterModel.GetInspectionStep(masterStepNo);
            }

            int stepNo = Convert.ToInt32(stepName);

            foreach (InspectionStep inspectionStep in inspectionStepList)
            {
                if (inspectionStep.StepNo == stepNo)
                    return inspectionStep;
            }

            return null;
        }

        public InspectionStep GetInspectionStep(int inspectionStepNo/*, bool fCreateOnEmpty = false*/)
        {
            foreach (InspectionStep inspectionStep in inspectionStepList)
            {
                if (inspectionStep.StepNo == inspectionStepNo)
                    return inspectionStep;
            }

            //if (fCreateOnEmpty)
            //{
            //    InspectionStep inspectionStep = new InspectionStep(inspectionStepNo, NumLight, NumLightType);
            //    inspectionStep.CreateTargetGroups(numCamera);
            //    inspectionStep.OwnerModel = this;

            //    inspectionStepList.Add(inspectionStep);

            //    return inspectionStep;
            //}

            return null;
        }

        public Probe GetProbe(string probeFullIdOrName)
        {
            foreach (InspectionStep inspectionStep in inspectionStepList)
            {
                Probe probe = inspectionStep.GetProbe(probeFullIdOrName);
                if (probe != null)
                    return probe;
            }

            return null;
        }

        public Probe GetProbe(int stepNo, int targetGroupNo, int targetNo, int probeNo)
        {
            try
            {
                return GetInspectionStep(stepNo).GetTargetGroup(targetGroupNo).GetTarget(targetNo).GetProbe(probeNo);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public Target GetTarget(string targetFullIdOrName)
        {
            foreach (InspectionStep inspectionStep in inspectionStepList)
            {
                Target target = inspectionStep.GetTarget(targetFullIdOrName);
                if (target != null)
                    return target;
            }

            if (masterModel != null)
                masterModel.GetTarget(targetFullIdOrName);

            return null;
        }

        public Target GetTarget(int stepNo, int targetGroupNo, int targetNo)
        {
            try
            {
                return GetInspectionStep(stepNo).GetTargetGroup(targetGroupNo).GetTarget(targetNo);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        public void GetTargets(List<Target> targetList)
        {
            foreach (InspectionStep inspectionStep in inspectionStepList)
            {
                inspectionStep.GetTargets(targetList);
            }
        }

        public void GetTargetTypes(List<string> targetTypeList)
        {
            foreach (InspectionStep inspectionStep in inspectionStepList)
            {
                inspectionStep.GetTargetTypes(targetTypeList);
            }
        }

        public void GetProbes(List<Probe> probeList)
        {
            foreach (InspectionStep inspectionStep in inspectionStepList)
            {
                inspectionStep.GetProbes(probeList);
            }
        }

        public int GetNumTarget()
        {
            List<Target> targetList = new List<Target>();
            GetTargets(targetList);

            return targetList.Count;
        }
        
        public virtual void CloseModel()
        {
            foreach (InspectionStep inspectionStep in this.inspectionStepList)
                inspectionStep.Clear();

            this.inspectionStepList.Clear();
        }

        public virtual void SaveModel(XmlElement xmlElement)
        {
        }

        public virtual void LoadModel(XmlElement xmlElement)
        {

        }

        public void LoadModelSchema()
        {
            string filePath = String.Format("{0}\\ModelSchema.xml", ModelPath);
            if (File.Exists(filePath))
            {
                modelSchema.Load(filePath);
            }
        }

        public bool SaveModelSchema()
        {
            string filePath = String.Format("{0}\\ModelSchema.xml", ModelPath);
            return modelSchema.Save(filePath);
        }

        public void AlignPosition(PositionAligner positionAligner)
        {
            foreach (InspectionStep inspectionStep in inspectionStepList)
            {
                inspectionStep.AlignPosition(positionAligner);
            }
        }

        public void CleanImage()
        {
            string imagePath = Path.Combine(ModelPath, "Image");
            if (Directory.Exists(imagePath) == true)
            {
                Directory.Delete(imagePath, true);
            }
        }

        public RotatedRect GetDefaultProbeRegion(Calibration cameraCalibration)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - GetDefaultProbeRegion");

            Rectangle rectangle = new Rectangle(0, 0, cameraCalibration.ImageSize.Width, cameraCalibration.ImageSize.Height);

            float centerX = cameraCalibration.ImageSize.Width / 2;
            float centerY = cameraCalibration.ImageSize.Height / 2;

            float width = cameraCalibration.ImageSize.Width / 4;
            float height = cameraCalibration.ImageSize.Height / 4;

            float left = centerX - width / 2;
            float top = centerY - height / 2;
            return new RotatedRect(left, top, width, height, 0);
        }

        protected IFilter AutoAddFilter(FilterType filterType)
        {
            IFilter filter = null;

            switch (filterType)
            {
                case FilterType.Binarize:
                    filter = new BinarizeFilter(BinarizationType.SingleThreshold, 40, 40);
                    break;
                case FilterType.Average:
                    filter = new AverageFilter();
                    break;
                case FilterType.EdgeExtraction:
                    filter = new EdgeExtractionFilter(3);
                    break;
                case FilterType.HistogramEqualization:
                    filter = new HistogramEqualizationFilter();
                    break;
                case FilterType.Morphology:
                    filter = new MorphologyFilter(MorphologyType.Erode, 3);
                    break;
            }
            return filter;
        }
    }
}
