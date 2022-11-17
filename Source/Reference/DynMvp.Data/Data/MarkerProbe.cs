using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.Vision.Matrox;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DynMvp.UI;

namespace DynMvp.Data
{
    public enum MarkerType
    {
        MergeSource, MergeTarget
    }

    public class MarkerProbe : Probe
    {
        MarkerType markerType;
        public MarkerType MarkerType
        {
            get { return markerType; }
            set { markerType = value; }
        }

        string mergeSourceId = "";
        public string MergeSourceId
        {
            get { return mergeSourceId; }
            set { mergeSourceId = value; }
        }

        Point3d mergeOffset = new Point3d();
        public Point3d MergeOffset
        {
            get { return mergeOffset; }
            set { mergeOffset = value; }
        }

        public override Object Clone()
        {
            MarkerProbe markerProbe = new MarkerProbe();
            markerProbe.Copy(this);

            return markerProbe;
        }

        public override void Copy(Probe probe)
        {
            base.Copy(probe);

            MarkerProbe markerProbe = (MarkerProbe)probe;
            mergeSourceId = markerProbe.mergeSourceId;
        }

        public override void OnPreInspection()
        {

        }

        public override void OnPostInspection()
        {

        }

        public override bool IsControllable()
        {
            return false;
        }

        public override List<ProbeResultValue> GetResultValues()
        {
            List<ProbeResultValue> resultValues = new List<ProbeResultValue>();
            resultValues.Add(new ProbeResultValue("Result", true));

            return resultValues;
        }

        public Image3D Mapping(Image3D srcImage3d, Image3D targetImage3d, float pixelRes3d)
        {
            Point3d[] srcPointArray = srcImage3d.PointArray;
            Point3d[] targetPointArray = targetImage3d.PointArray;

            RectangleF mappingRect = srcImage3d.MappingRect;
            Size mappingSize = new Size();

            if (mergeOffset.IsEmpty() == false)
            {
                List<Point3d> pointList = new List<Point3d>();
                foreach (Point3d point in targetPointArray)
                {
                    pointList.Add(new Point3d(point.X + mergeOffset.X, point.Y + mergeOffset.Y, point.Z + mergeOffset.Z));
                }
                targetPointArray = pointList.ToArray();

                RectangleF targetMappingRect = targetImage3d.MappingRect;
                targetMappingRect.Offset(new PointF((float)mergeOffset.X, (float)mergeOffset.Y));

                mappingRect = RectangleF.Union(mappingRect, targetMappingRect);
                mappingSize = new Size((int)(mappingRect.Width / pixelRes3d), (int)(mappingRect.Height / pixelRes3d));
            }
            else
            {
                mappingSize = new Size(srcImage3d.Width, srcImage3d.Height);
            }

            Point3d[] pointArray = srcPointArray.Concat(targetPointArray).ToArray();

            ImageMapper imageMapper = new ImageMapper();
            imageMapper.Initialize(mappingRect, pixelRes3d);
            imageMapper.Mapping(pointArray);

            imageMapper.Image3d.MappingRect = mappingRect;
            imageMapper.Image3d.PointArray = pointArray;

            return imageMapper.Image3d;
        }

        public override ProbeResult Inspect(InspParam inspParam)
        {
            MarkerProbeResult markerProbeResult = new MarkerProbeResult(this);
            markerProbeResult.Judgment = Judgment.Reject;

            if (inspParam.DeviceImageSet.Image3D != null)
            {
                if (markerType == MarkerType.MergeSource)
                {
                    if (inspParam.DeviceImageSet.Image3D.PointArray != null)
                    {
                        markerProbeResult.Judgment = Judgment.Accept;
                        markerProbeResult.Image3d = (Image3D)inspParam.DeviceImageSet.Image3D.Clone();
                    }
                    else
                    {
                        ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Teach.InvalidState, ErrorLevel.Error,
                            "Empty Data", "Scan data of merge source is empty.", null, "Please, Scan first.");
                    }
                }
                else if (markerType == MarkerType.MergeTarget)
                {
                    InspectionResult inspectionResult = inspParam.InspectionResult;
                    if (inspectionResult == null)
                    {
                        ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Teach.InvalidState, ErrorLevel.Error,
                            "Empty Data", "There is no inspection result", null);
                        return markerProbeResult;
                    }

                    MarkerProbeResult mergeSourceResult = (MarkerProbeResult)inspectionResult.GetProbeResult(mergeSourceId);
                    if (mergeSourceResult == null)
                    {
                        ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Teach.InvalidState, ErrorLevel.Error,
                            "Empty Data", "There is no merge source", null);
                        return null;
                    }

                    if (inspParam.DeviceImageSet.Image3D.PointArray == null)
                    {
                        ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Teach.InvalidState, ErrorLevel.Error,
                            "Empty Data", "Scan data of merge source is empty.", null, "Please, Scan first.");
                        return null;
                    }

                    if (mergeSourceResult.Image3d.PointArray == null)
                    {
                        ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Teach.InvalidState, ErrorLevel.Error,
                            "Empty Data", "Scan data of merge source is empty.", null, "Please, Scan first.");
                        return null;
                    }

                    Image3D image3d;

                    if (mergeOffset.IsEmpty() == true)
                    {
                        Image3D alignedCadData = (Image3D)Mil3dMeasure.Alignment(mergeSourceResult.Image3d, inspParam.DeviceImageSet.Image3D).FlipX();
                        image3d = Image3D.Average(mergeSourceResult.Image3d, alignedCadData);
                    }
                    else
                    {
                        image3d = Mapping(mergeSourceResult.Image3d, inspParam.DeviceImageSet.Image3D, inspParam.PixelRes3d);
                    }

                    inspParam.DeviceImageSet.UpdateImage3D(image3d);

                    markerProbeResult.Image3d = (Image3D)image3d.Clone();

                    markerProbeResult.Judgment = Judgment.Accept;
                }
            }

            return markerProbeResult;
        }

        public override ProbeResult CreateDefaultResult()
        {
            return new IoProbeResult(this, false);
        }
    }

    public class MarkerProbeResult : ProbeResult
    {
        Image3D image3d;
        public Image3D Image3d
        {
            get { return image3d; }
            set { image3d = value; }
        }

        public MarkerProbeResult(Probe probe)
        {
            this.Judgment = Judgment.Accept;
            this.Probe = probe;
        }

        public override string ToString()
        {
            return "Marker";
        }

        public override void BuildResultMessage(DynMvp.UI.MessageBuilder totalResultMessage)
        {

        }
    }
}
