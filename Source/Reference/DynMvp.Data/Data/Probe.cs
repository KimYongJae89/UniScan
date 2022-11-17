using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Drawing;
using System.ComponentModel;

using DynMvp.Base;
using DynMvp;
using DynMvp.UI;

using DynMvp.Vision;
using DynMvp.Data.UI;
using DynMvp.InspData;
using System.Xml;

namespace DynMvp.Data
{
    public enum ProbeType
    {
        Vision, Io, Serial, Daq, Compute, Marker, Tension
    }

    public enum VisionType
    {
        Binary, Patter, OCR, Barcode, Brightness, Circle, Rect
    }
    public enum SerialType
    {
        Serial, BarcodeReader, Loadcell, Tension
    }

    public enum ProbeResultType
    {
        Judgement, Value
    }

    public enum ProbeFigureType
    {
        Rectangle, Elipse, Line
    }

    public interface IProbeFilter
    {
        bool IsValid(Probe probe);
    }

    public abstract class ProbeCustomInfo
    {
        public abstract ProbeCustomInfo Clone();
        public abstract void Save(XmlElement xmlElement);
        public abstract void Load(XmlElement xmlElement);
    }

    public abstract class Probe : ICloneable, ITeachObject
    {
        private int id;
        [BrowsableAttribute(false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string name;
        public string Name
        {
            get 
            {
                if (String.IsNullOrEmpty(name))
                    return FullId;

                return name;
            }

            set { name = value; }
        }

        private Target target;
        [BrowsableAttribute(false)]
        public Target Target
        {
            get { return target; }
            set { target = value; }
        }

        private ProbeType probeType;
        [BrowsableAttribute(false)]
        public ProbeType ProbeType
        {
            get { return probeType; }
            set { probeType = value; }
        }

        private ProbeResultType probeResultType;
        public ProbeResultType ProbeResultType
        {
            get { return probeResultType; }
            set { probeResultType = value; }
        }

        private bool stepBlocker;
        public bool StepBlocker
        {
            get { return stepBlocker; }
            set { stepBlocker = value; }
        }

        private int lightTypeIndex = 0;
        public int LightTypeIndex
        {
            get { return lightTypeIndex; }
            set { lightTypeIndex = value; }
        }

        public virtual string GetProbeTypeDetailed()
        {
            return probeType.ToString();
        }

        public virtual string GetProbeTypeShortName()
        {
            return probeType.ToString();
        }

        // Target의 fiducial로 동작하는 Probe일 경우, true이다.
        private bool actAsFiducialProbe;
        public bool ActAsFiducialProbe
        {
            get { return actAsFiducialProbe; }
            set { actAsFiducialProbe = value; }
        }

        private bool actAsCalibrationProbe;
        public bool ActAsCalibrationProbe
        {
            get { return actAsCalibrationProbe; }
            set { actAsCalibrationProbe = value; }
        }

        private FigureProperty figureProperty = null;
        public FigureProperty FigureProperty
        {
            get { return figureProperty; }
            set { figureProperty = value; }
        }

        private bool isSelectable = true;
        public bool IsSelectable
        {
            get { return isSelectable; }
            set { isSelectable = value; }
        }

        private bool isRotateable = true;
        public bool IsRotateable
        {
            get { return isRotateable; }
            set { isRotateable = value; }
        }

        [BrowsableAttribute(false)]
        public bool Adjustable
        {
            get
            {
                return (actAsFiducialProbe == false) && (fiducialProbeId !=  0);
            }
        }

        // probe의 위치를 보정할 기준 fiducial probe의 id
        // 이 값이 -1 일 경우, fiducial probe를 사용하지 않는 것으로 한다.
        private int fiducialProbeId = 0;
        public int FiducialProbeId
        {
            get { return fiducialProbeId; }
            set { fiducialProbeId = value; }
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

        [BrowsableAttribute(false)]
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

        public int X { set { baseRegion.X = value; } }
        public int Y { set { baseRegion.Y = value; } }
        public int Width { set { baseRegion.Width = value; } }
        public int Height { set { baseRegion.Height = value; } }
        public int Angle { set { baseRegion.Angle = value; } }

        private bool inverseResult;
        public bool InverseResult
        {
            get { return inverseResult; }
            set { inverseResult = value; }
        }

        private bool modelVerification;
        public bool ModelVerification
        {
            get { return modelVerification; }
            set { modelVerification = value; }
        }

        public abstract bool IsControllable();
        public abstract List<ProbeResultValue> GetResultValues();

        [BrowsableAttribute(false)]
        public string FullId
        {
            get { return String.Format("{0}.{1:000}", Target.FullId, Id); }
        }

        ProbeCustomInfo customInfo = null;
        public ProbeCustomInfo CustomInfo
        {
            get { return customInfo; }
            set { customInfo = value; }
        }

        public abstract Object Clone();

        public virtual void Clear()
        {

        }

        public virtual void Copy(Probe probe)
        {
            id = probe.id;
            target = probe.target;
            probeType = probe.probeType;
            stepBlocker = probe.stepBlocker;
            fiducialProbeId = probe.fiducialProbeId;
            baseRegion = new RotatedRect(probe.baseRegion);
            alignedRegion = new RotatedRect(probe.alignedRegion);
            inverseResult = probe.inverseResult;
            lightTypeIndex = probe.LightTypeIndex;
            modelVerification = probe.modelVerification;
            actAsCalibrationProbe = probe.actAsCalibrationProbe;
            actAsFiducialProbe = probe.actAsFiducialProbe;
            figureProperty = probe.figureProperty.Clone();
        }

        public virtual bool SyncParam(Probe srcProbe, IProbeFilter probeFilter)
        {
            if (probeFilter?.IsValid(this) == false)
                return false;

            stepBlocker = srcProbe.stepBlocker;
            inverseResult = srcProbe.inverseResult;
            modelVerification = srcProbe.modelVerification;
            lightTypeIndex = srcProbe.LightTypeIndex;
            figureProperty = srcProbe.figureProperty.Clone();

            return true;
        }

        public void UpdateTargetImage(Image2D targetImage, int lightTypeIndex)
        {
            target.Image = targetImage;
            target.LightTypeIndex = lightTypeIndex;
        }

        public void Offset(SizeF offset, PositionAligner positionAligner)
        {
            baseRegion.Offset(offset.Width, offset.Height);

            if (positionAligner != null)
                alignedRegion = positionAligner.AlignFov(baseRegion);
            else
                alignedRegion.Offset(offset.Width, offset.Height);
        }

        public void UpdateWorldRegion(Calibration calibration)
        {
            PointF[] points = baseRegion.GetPoints();
            PointF[] realPoints = new PointF[4];

            for (int i=0; i<4; i++)
                realPoints[i] = calibration.PixelToWorld(points[i]);

            PointF centerPt = calibration.PixelToWorld(DrawingHelper.CenterPoint(baseRegion));

            worldRegion =new RotatedRect(DrawingHelper.FromCenterSize(centerPt, DrawingHelper.ToSize(DrawingHelper.Subtract(realPoints[2], realPoints[0]))), 0);
        }

        public virtual void AppendFigures(FigureGroup figureGroup, Pen pen)
        {
            if (pen == null)
            {
                FigureProperty figureProperty = FigurePropertyPool.Instance().GetFigureProperty(probeType.ToString());
                if (figureProperty.Pen != null)
                    pen = figureProperty.Pen;
            }

            Figure probeFigure = figureGroup.GetFigureByTag(this);
            if (probeFigure == null)
            {
                probeFigure = new RectangleFigure(RotatedRect.Empty, pen);
                probeFigure.Tag = this;
                figureGroup.AddFigure(probeFigure);
            }
            probeFigure.Name = this.name;
            probeFigure.SetRectangle(this.baseRegion);
            probeFigure.Selectable = isSelectable;
            //probeFigure.Selectable = true;
        }

        public virtual void AppendAdditionalFigures(FigureGroup figureGroup)
        {

        }

        public void UpdateRegion(RotatedRect rotatedRect, PositionAligner positionAligner)
        {
            if (positionAligner != null)
            {
                alignedRegion = rotatedRect;
                baseRegion = positionAligner.InvAlignFov(rotatedRect);
            }
            else
            {
                baseRegion = alignedRegion = rotatedRect;
            }

            alignedRegion = rotatedRect;
            //target.UpdateRegion(positionAligner);
            target.UpdateRegion(positionAligner);
        }

        public virtual string[] GetPreviewNames()
        {
            return new string[] { "None" };
        }

        public virtual ImageD PreviewFilterResult(ImageD imageD, int previewFilterType, bool useTargetCoordinate)
        {
            LogHelper.Debug(LoggerType.Operation, "Probe - PreviewFilterResult");
            
            return imageD;
        }

        public bool Inspect(InspParam inspParam, InspectionResult inspectionResult)
        {
            ProbeResult probeResult;
            try
            {
                probeResult = Inspect(inspParam);
                if (probeResult == null)
                {
                    LogHelper.Warn(LoggerType.Inspection, String.Format("Invalid Inspection. Probe Result is null. Probe - {0}.{1}", Target.FullId, id));
                    inspectionResult.AddProbeResult(CreateDefaultResult());
                    return false;
                }
            }
#if DEBUG==false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Inspection, String.Format("Inspection Exception :. Probe - {0}.{1} / Msg - {2}", Target.FullId, id, ex.ToString()));
                inspectionResult.AddProbeResult(CreateDefaultResult());
                return false;
            }
#endif
            finally { }

            if ((probeResult is VisionProbeResult) == false)
            {
                probeResult.RegionInFov = alignedRegion;
                probeResult.RegionInProbe = new RotatedRect(0, 0, alignedRegion.Width, alignedRegion.Height, 0);
            }

            probeResult.TargetRegion = target.AlignedRegion;
            probeResult.RegionInTarget = new RotatedRect(probeResult.RegionInFov.X - target.AlignedRegion.X,
                        probeResult.RegionInFov.Y - target.AlignedRegion.Y, AlignedRegion.Width, AlignedRegion.Height, 0);

            if (probeResultType == ProbeResultType.Judgement)
            {
                if (InverseResult)
                {
                    probeResult.InvertJudgment();
                }

                if (ModelVerification && probeResult.Judgment == Judgment.Reject)
                {
                    probeResult.DifferentProductDetected = true;
                }
            }

            //probeResult.SequenceNo = inspParam.SequenceNo;
            inspectionResult.AddProbeResult(probeResult);

            return probeResult.Judgment == Judgment.Accept;
        }

        public virtual void PrepareInspection()
        {

        }

        public abstract void OnPreInspection();
        public abstract ProbeResult CreateDefaultResult();
        public abstract ProbeResult Inspect(InspParam inspParam);
        public abstract void OnPostInspection();

        // ID가 같으면 같은 프로브이다.
        public override bool Equals(object obj)
        {
            if(!(obj is Probe))
            {
                return false;
            }

            Probe probe = (Probe)obj;
            return this.FullId == probe.FullId;
        }

        internal void AlignPosition(PositionAligner positionAligner)
        {
            alignedRegion = positionAligner.AlignFov(BaseRegion);

            AlignFigures(positionAligner);
        }

        public virtual void AlignFigures(PositionAligner positionAligner)
        {

        }
    }
}
