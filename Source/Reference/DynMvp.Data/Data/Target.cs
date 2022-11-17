using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml.Serialization;
using System.IO;

using DynMvp.Base;
using DynMvp.Vision;
using DynMvp.UI;
using DynMvp.Data.UI;
using DynMvp.InspData;

namespace DynMvp.Data
{
    public enum InspectionLogicType
    {
        And, Or, Nand, Nor, Custom
    }

    public class Target : ICloneable, ITrackTarget, ITeachObject
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        int moduleNo;
        public int ModuleNo
        {
            get { return moduleNo; }
            set { moduleNo = value; }
        }

        public string FullId
        {
            get
            {
                return String.Format("{0:00}.{1:00}.{2:000}", targetGroup.InspectionStep.StepName, targetGroup.GroupId, id);
            }
        }

        private InspectionLogicType inspectionLogicType;
        public InspectionLogicType InspectionLogicType
        {
            get { return inspectionLogicType; }
            set { inspectionLogicType = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string typeName;
        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        private TargetGroup targetGroup;
        public TargetGroup TargetGroup
        {
            get { return targetGroup; }
            set { targetGroup = value; }
        }

        List<Figure> shemaFigures;
        public List<Figure> ShemaFigures
        {
            get { return shemaFigures; }
            set { shemaFigures = value; }
        }

        // 실제 영역 : 기본 micron 단위
        private RotatedRect worldRegion;
        public RotatedRect WorldRegion
        {
            get { return worldRegion; }
            set { worldRegion = value; }
        }

        private RotatedRect baseRegion;
        public RotatedRect BaseRegion
        {
            get { return baseRegion; }
            set { baseRegion = alignedRegion = value; }
        }

        private RotatedRect alignedRegion;
        public RotatedRect AlignedRegion
        {
            get { return alignedRegion; }
            set { alignedRegion = value; }
        }

        private bool useInspection = true;
        public bool UseInspection
        {
            get { return useInspection; }
            set { useInspection = value; }
        }

        private Image2D image = null;
        public Image2D Image
        {
            get { return image; }
            set { image = value; }
        }

        private int lightTypeIndex = 0;
        public int LightTypeIndex
        {
            get { return lightTypeIndex; }
            set { lightTypeIndex = value; }
        }

        public string ImageEncodedString
        {
            get
            {
                if (Image == null)
                    return null;

                Bitmap bitmap = image.ToBitmap();
                string resultString = ImageHelper.BitmapToBase64String(bitmap);
                bitmap.Dispose();

                return resultString;
            }
            set
            {
                Bitmap bitmap = ImageHelper.Base64StringToBitmap(value);
                if (bitmap != null)
                {
                    image = Image2D.FromBitmap(bitmap);
                    bitmap.Dispose();
                }
            }
        }

        private List<Probe> probeList = new List<Probe>();
        public List<Probe> ProbeList
        {
            get { return probeList; }
        }

        public IEnumerator<Probe> GetEnumerator()
        {
            return probeList.GetEnumerator();
        }

        public void Clear()
        {
            foreach (Probe probe in probeList)
                probe.Clear();
            probeList.Clear();

            if (image != null)
                image.Dispose();
        }

        public Object Clone()
        {
            Target target = new Target();
            target.Copy(this);

            return target;
        }

        public int GetNumBand()
        {
            if (image != null)
                return image.NumBand;

            return 1;
        }

        public void UpdateTargetImage(Image2D targetImage, int lightTypeIndex)
        {
            image = targetImage;
            this.lightTypeIndex = lightTypeIndex;
        }

        public void Copy(Target target)
        {
            id = target.Id;
            inspectionLogicType = target.inspectionLogicType;
            name = target.Name;
            typeName = target.typeName;
            baseRegion = target.BaseRegion;
            alignedRegion = target.AlignedRegion;
            if (target.Image != null)
                image = (Image2D)target.Image.Clone();
            else
                image = null;

            useInspection = target.useInspection;

            foreach (Probe probe in target.probeList)
            {
                Probe cloneProbe = (Probe)probe.Clone();
                AddProbe(cloneProbe);
            }
        }

        public void SyncParam(Probe srcProbe, IProbeFilter probeFilter)
        {
            for (int i = 0; i < probeList.Count; i++)
            {
                if (srcProbe.GetType() == probeList[i].GetType())
                {
                    probeList[i].SyncParam(srcProbe, probeFilter);
                }
            }
        }

        public void Add(System.Object obj)
        {
            probeList.Add((Probe)obj);
        }

        public void AddProbe(List<Probe> probeList)
        {
            foreach (Probe probe in probeList)
                AddProbe(probe);
        }

        public void AddProbe(Probe probe)
        {
            probe.Id = CreateProbeId();
            probe.Target = this;
            //if (probe.FiducialProbeId == 0)
            //    probe.FiducialProbeId = GetFiducialProbe();
            probeList.Add(probe);
            UpdateRegion(null);
        }

        public void GetProbes(List<Probe> probeList)
        {
            foreach (Probe probe in this.probeList)
            {
                probeList.Add(probe);
            }
        }

        public Probe GetProbe(int id)
        {
            foreach (Probe probe in probeList)
            {
                if (probe.Id == id)
                    return probe;
            }

            return null;
        }

        public Probe GetProbe(string probeFullIdOrName)
        {
            foreach (Probe probe in probeList)
            {
                if (probe.FullId == probeFullIdOrName || probe.Name == probeFullIdOrName)
                    return probe;
            }

            return null;
        }

        private int CreateProbeId()
        {
            for (int i = 1; i < int.MaxValue; i++)
            {
                if (GetProbe(i) == null)
                    return i;
            }

            throw new TooManyItemsException();
        }

        public void RemoveProbe(Probe probe)
        {
            probe.Clear();
            probeList.Remove(probe);
            TargetGroup.FiducialSet.RemoveFiducial(probe);

            UpdateRegion(null);
        }

        public void RemoveFiducial()
        {
            foreach (Probe probe in probeList)
            {
                TargetGroup.FiducialSet.RemoveFiducial(probe);
            }
        }

        internal void CreateObjectFigures(FigureGroup figureGroup)
        {
            Figure targetFigure = new RectangleFigure(worldRegion, new Pen(Color.Cyan));
            targetFigure.Tag = this;
            figureGroup.AddFigure(targetFigure);

            foreach (Probe probe in probeList)
            {
                Figure probeFigure = new RectangleFigure(probe.WorldRegion, new Pen(Color.Yellow));
                probeFigure.Tag = this;
                figureGroup.AddFigure(probeFigure);
            }
        }

        public void PrepareInspection()
        {
            foreach (Probe probe in probeList)
            {
                probe.PrepareInspection();
            }
        }

        public void SelectFiducialProbe(int probeId)
        {
            foreach (Probe probe in probeList)
            {
                if (probe.Id == probeId)
                    probe.ActAsFiducialProbe = true;
                else if (probe.ActAsFiducialProbe == false && probe.FiducialProbeId == 0)
                {
                    probe.FiducialProbeId = probeId;
                }
            }
        }

        public void DeselectFiducialProbe(int probeId)
        {
            foreach (Probe probe in probeList)
            {
                if (probe.Id == probeId)
                    probe.ActAsFiducialProbe = false;
                else if (probe.FiducialProbeId == probeId)
                {
                    probe.FiducialProbeId = 0;
                }
            }
        }

        public int GetFiducialProbe()
        {
            foreach (Probe probe in probeList)
            {
                if (probe.ActAsFiducialProbe == true)
                    return probe.Id;
            }

            return 0;
        }

        public Probe GetAlignmentProbe()
        {
            foreach (Probe probe in probeList)
            {
                if (probe.ProbeType == ProbeType.Vision)
                {
                    VisionProbe visionProbe = (VisionProbe)probe;
                    if (visionProbe.InspAlgorithm.GetAlgorithmType() == PatternMatching.TypeName)
                    {
                        return visionProbe;
                    }
                }
            }

            return null;
        }

        public void UpdateRegion(PositionAligner positionAligner)
        {
            RectangleF newRegion = RectangleF.Empty;
            foreach (Probe probe in probeList)
            {
                if (newRegion == RectangleF.Empty)
                    newRegion = probe.BaseRegion.GetBoundRect();
                else
                    newRegion = RectangleF.Union(newRegion, probe.BaseRegion.GetBoundRect());
            }
            baseRegion = new RotatedRect(newRegion, 0);

            if (positionAligner != null)
                alignedRegion = positionAligner.AlignFov(baseRegion);
            else
                alignedRegion = baseRegion;
        }

        public void UpdateRegion(RotatedRect newRegion, PositionAligner positionAligner)
        {
            newRegion.Inflate(-10, -10);

            SizeF offset = new SizeF(newRegion.X - baseRegion.X, newRegion.Y - baseRegion.Y);
            baseRegion = newRegion;

            foreach (Probe probe in probeList)
            {
                probe.Offset(offset, positionAligner);
            }

            if (positionAligner != null)
                alignedRegion = positionAligner.AlignFov(baseRegion);
        }

        public void AppendFigures(FigureGroup figureGroup, Pen pen, bool includeProbe = false)
        {
            RotatedRect drawRegion = this.baseRegion;
            drawRegion.Inflate(10, 10);

            if (pen == null)
            {
                pen = new Pen(Color.LightGreen, 1.0F);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            }

            Figure targetFigure = figureGroup.GetFigureByTag(this);
            if(targetFigure==null)
            {
                targetFigure = new RectangleFigure(RotatedRect.Empty, pen);
                targetFigure.Tag = this;
                figureGroup.AddFigure(targetFigure);
            }
            targetFigure.SetRectangle(drawRegion);
            targetFigure.Name = this.name;
            targetFigure.Selectable = false;

            if (includeProbe)
                probeList.ForEach(f => f.AppendFigures(figureGroup, null));
        }

        public MarkerProbe GetMarkerProbe(MarkerType markerType)
        {
            foreach (Probe probe in probeList)
            {
                if (probe is MarkerProbe)
                {
                    MarkerProbe markerProbe = (MarkerProbe)probe;
                    if (markerProbe != null && markerProbe.MarkerType == markerType)
                        return markerProbe;
                }
            }

            return null;
        }

        internal int GetLightTypeIndex()
        {
            foreach (Probe probe in probeList)
            {
                if (probe is VisionProbe)
                {
                    return ((VisionProbe)probe).LightTypeIndex;
                }
            }

            return -1;
        }

        internal void AddSchemaFigure(Schema schema)
        {
            foreach (Probe probe in probeList)
            {
                RectangleFigure rectangleFigure = new RectangleFigure(probe.WorldRegion, (Pen)schema.DefaultFigureProperty.Pen.Clone(), null);
                rectangleFigure.Id = "rectangle";

                SchemaFigure schemaFigure = new SchemaFigure();
                schemaFigure.AddFigure(rectangleFigure);
                //schemaFigure.Tag = probe.Target.FullId;
                schemaFigure.Tag = probe.FullId;

                schema.AddFigure(schemaFigure);
            }
        }

        public void AppendAdditionalFigures(FigureGroup figureGroup)
        {

        }

        private List<Probe> GetComputeProbeList()
        {
            List<Probe> computeProbeList = new List<Probe>();

            foreach (Probe probe in probeList)
            {
                if (probe is ComputeProbe)
                    computeProbeList.Add(probe);
            }

            return computeProbeList;
        }

        private List<Probe> GetSortedProbeList()
        {
            return GetSortedProbeList(probeList);
        }

        private List<Probe> GetSortedProbeList(List<Probe> probeList)
        {
            List<Probe> fiducialProbeList = probeList.FindAll(f => f.ActAsFiducialProbe);
            fiducialProbeList.Sort((f, g) => g.FiducialProbeId.CompareTo(f.FiducialProbeId));

            List<Probe> calibrationProbeList = probeList.FindAll(f => f.ActAsCalibrationProbe);

            List<Probe> normalProbeList = probeList.FindAll(f => !fiducialProbeList.Contains(f) && !calibrationProbeList.Contains(f));

            List<Probe> sortedProbeList = new List<Probe>();
            sortedProbeList.AddRange(fiducialProbeList);
            sortedProbeList.AddRange(calibrationProbeList);
            sortedProbeList.AddRange(normalProbeList);

            //foreach (Probe probe in probeList)
            //{
            //    if (probe is ComputeProbe || probe is MarkerProbe)
            //        continue;

            //    if (probe.ActAsFiducialProbe == true)
            //        sortedProbeList.Add(probe);
            //}

            //foreach (Probe probe in probeList)
            //{
            //    if (probe is ComputeProbe || probe is MarkerProbe)
            //        continue;

            //    if (probe.ActAsFiducialProbe == false && probe != calibrationProbe)
            //        sortedProbeList.Add(probe);
            //}

            return sortedProbeList;
        }

        public void OnPreInspection()
        {
            foreach (Probe probe in probeList)
            {
                probe.OnPreInspection();
            }
        }

        public void Inspect(InspParam inspParam, InspectionResult inspectionResult)
        {
            if (this.useInspection == false)
            {
                LogHelper.Debug(LoggerType.Inspection, String.Format("Inspection Skipped. {0} {1}", FullId, Name));
                return;
            }

            if (this.baseRegion.IsEmpty)
            {
                LogHelper.Debug(LoggerType.Inspection, String.Format("Target Region is Empty. {0} {1}", FullId, Name));
                return;
            }

            LogHelper.Debug(LoggerType.Inspection, String.Format("On Inspect. {0} {1}", FullId, Name));

            List<Probe> sortedProbeList = null;
            if (inspParam.SelectedProbeList.Count == 0)
                sortedProbeList = GetSortedProbeList();
            else
                sortedProbeList = GetSortedProbeList(inspParam.SelectedProbeList);

            InspectionResult targetInspectionResult = new InspectionResult();

            bool fiducialResult = true;
            bool result = true;
            if (inspectionLogicType == InspectionLogicType.Or)
                result = false;

            foreach (Probe probe in sortedProbeList)
            {
                if (probe.FiducialProbeId > 0)
                {
                    try
                    {
                        VisionProbeResult probeResult = (VisionProbeResult)targetInspectionResult.GetProbeResult(probe.FiducialProbeId);
                        if (probeResult != null)
                        {
                            inspParam.FiducialProbeOffset = probeResult.AlgorithmResult.OffsetFound;
                            inspParam.FiducialProbeAngle = probeResult.AlgorithmResult.AngleFound;
                            inspParam.FiducialProbeRect = probeResult.RegionInFov;
                            LogHelper.Debug(LoggerType.Inspection, String.Format("Fiducial Offset : {0}", inspParam.FiducialProbeOffset.ToString()));
                        }
                        else
                        {
                            LogHelper.Debug(LoggerType.Inspection, "Local Fiducial Error");
                        }
                    }
                    catch (InvalidCastException)
                    {
                        LogHelper.Debug(LoggerType.Inspection, "Local Fiducial Cast Error");
                    }
                }
                else
                {
                    inspParam.FiducialProbeOffset = new SizeF(0, 0);
                }

                if (probe.ActAsFiducialProbe)
                    fiducialResult &= probe.Inspect(inspParam, targetInspectionResult);
                else if (inspectionLogicType == InspectionLogicType.Or)
                    result |= probe.Inspect(inspParam, targetInspectionResult);
                else
                    result &= probe.Inspect(inspParam, targetInspectionResult);

                if (probe.ActAsCalibrationProbe)
                {
                    ProbeResult probeResult = targetInspectionResult.GetProbeResult(probe);

                    if (probeResult != null && probeResult is VisionProbeResult == true)
                    {
                        VisionProbeResult calibrationProbeResult = (VisionProbeResult)probeResult;
                        Calibration cameraCalibration = (Calibration)calibrationProbeResult.GetResultValue("Calibration").Value;
                        if (cameraCalibration != null)
                        {
                            inspParam.CameraCalibration = cameraCalibration;
                        }
                    }
                }
            }

            result &= fiducialResult;

            if (result && inspectionLogicType == InspectionLogicType.Or)
            {
                foreach (ProbeResult probeResult in targetInspectionResult)
                {
                    if (probeResult.Judgment == Judgment.Reject)
                        probeResult.Judgment = Judgment.FalseReject;
                }
            }

            LogHelper.Debug(LoggerType.Inspection, String.Format("Inspection Result. {0} {1} {2}", FullId, Name, result));

            if (inspParam.DeviceImageSet.ImageList2D.Count > lightTypeIndex)
            {
                Image2D cameraImage = (Image2D)inspParam.DeviceImageSet.ImageList2D[lightTypeIndex];

                Rectangle bmpRect = new Rectangle(new Point(0, 0), cameraImage.Size);

                if (cameraImage != null)
                {
                    if (cameraImage.IsUseIntPtr() == true || cameraImage.Data != null)
                    {
                        Image2D targetImage = (Image2D)cameraImage.ClipImage(Rectangle.Ceiling(alignedRegion.GetBoundRect()));

                        if (targetImage != null)
                        {
                            bmpRect = new Rectangle(new Point(0, 0), cameraImage.Size);

                            targetInspectionResult.AddTargetImage(FullId, lightTypeIndex, targetImage);

                            if (inspParam.SaveTargetImage)
                            {
                                {
                                    string fileName = String.Format("{0}\\{1}_{2}.jpg", Configuration.TempFolder, FullId, (result == true ? "G" : "N"));

                                    if (File.Exists(fileName) == true)
                                        File.Delete(fileName);

                                    targetImage.SaveImage(fileName, ImageFormat.Bmp);
                                }
                            }
                        }
                    }
                }
            }

            inspectionResult.AddProbeResult(targetInspectionResult);

            if (inspParam.TargetInspected != null)
                inspParam.TargetInspected(this, targetInspectionResult);

            //LogHelper.Debug(LoggerType.Inspection, String.Format("Target error"));
        }

        public void Compute(InspParam inspParam, InspectionResult inspectionResult)
        {
            if (useInspection == false)
                return;

            LogHelper.Debug(LoggerType.Inspection, String.Format("On Inspect. {0} {1}", FullId, Name));

            List<Probe> computeProbeList = GetComputeProbeList();

            InspectionResult targetInspectionResult = new InspectionResult();
            bool result = true;
            bool exist = false;
            foreach (Probe probe in computeProbeList)
            {
                result &= probe.Inspect(inspParam, targetInspectionResult);
                exist = true;
            }

            if (exist == true && inspParam.TargetInspected != null)
                inspParam.TargetInspected(this, targetInspectionResult);

            inspectionResult.AddProbeResult(targetInspectionResult);

            LogHelper.Debug(LoggerType.Inspection, String.Format("Inspection Result. {0} {1} {2}", FullId, Name, result));
        }

        public void SetCalibrationProbe(Probe probe)
        {
            Probe calibrationProbe = this.probeList.Find(f => f.ActAsCalibrationProbe);
            if (calibrationProbe != null)
                calibrationProbe.ActAsCalibrationProbe = false;

            if (probe == null)
                return;

            if (!this.probeList.Contains(probe))
                AddProbe(probe);
            probe.ActAsCalibrationProbe = true;
        }

        public bool IsSizable()
        {
            return false;
        }

        public bool IsRotatable()
        {
            return false;
        }

        public bool IsContainer()
        {
            return true;
        }

        internal void AlignPosition(PositionAligner positionAligner)
        {
            alignedRegion = positionAligner.AlignFov(BaseRegion);

            foreach (Probe probe in probeList)
            {
                probe.AlignPosition(positionAligner);
            }
        }
    }
}