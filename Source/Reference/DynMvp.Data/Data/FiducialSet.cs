using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using DynMvp.UI;
using DynMvp.Base;
using DynMvp.Vision;
using DynMvp.Devices.FrameGrabber;
using DynMvp.InspData;

namespace DynMvp.Data
{
    public enum FiducialSetType
    {
        TargetGroup, Model
    }

    public struct FiducialInfo
    {
        FiducialSetType type;
        public FiducialSetType Type
        {
            get { return type; }
            set { type = value; }
        }

        string stepName;
        public string StepName
        {
            get { return stepName; }
            set { stepName = value; }
        }

        int targetGroupId;
        public int TargetGroupId
        {
            get { return targetGroupId; }
            set { targetGroupId = value; }
        }

        int targetId;
        public int TargetId
        {
            get { return targetId; }
            set { targetId = value; }
        }

        int probeId;
        public int ProbeId
        {
            get { return probeId; }
            set { probeId = value; }
        }
    }

    public class FiducialSet : ICloneable
    {
        int index;
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public bool Valid
        {
            get { return fiducials.Count == 2;  } 
        }

        private List<FiducialInfo> fiducialInfoList = new List<FiducialInfo>();
        public List<FiducialInfo> FiducialInfoList
        {
            get { return fiducialInfoList; }
            set { fiducialInfoList = value; }
        }

        private List<Probe> fiducials = new List<Probe>();
        public List<Probe> Fiducials
        {
            get { return fiducials; }
            set { fiducials = value; }
        }

        private SizeF secondFiducialOffset;
        public SizeF SecondFiducialOffset
        {
            get { return secondFiducialOffset; }
            set { secondFiducialOffset = value; }
        }

        public int Count
        {
            get { return fiducials.Count;  }
        }

        public void Clear()
        {
            fiducials.Clear();
        }

        public Object Clone()
        {
            FiducialSet fiducialSet = new FiducialSet();
            fiducialSet.Copy(this);

            return fiducialSet;
        }

        public void LinkFiducial(Model model)
        {
            fiducials.Clear();

            foreach (FiducialInfo fiducialInfo in fiducialInfoList)
            {
                InspectionStep inspectionStep = model.GetInspectionStep(fiducialInfo.StepName);
                if (inspectionStep == null)
                    continue;

                TargetGroup targetGroup = inspectionStep.GetTargetGroup(fiducialInfo.TargetGroupId);
                if (targetGroup == null)
                    continue;

                Target target = targetGroup.GetTarget(fiducialInfo.TargetId);
                if (target != null)
                {
                    Probe probe = target.GetProbe(fiducialInfo.ProbeId);
                    AddFiducial(probe);
                }
            }
        }

        public void LinkFiducial(TargetGroup targetGroup)
        {
            fiducials.Clear();

            foreach (FiducialInfo fiducialInfo in fiducialInfoList)
            {
                Target target = targetGroup.GetTarget(fiducialInfo.TargetId);
                if (target != null)
                {
                    Probe probe = target.GetProbe(fiducialInfo.ProbeId);
                    AddFiducial(probe);
                }
            }
        }

        public void Copy(FiducialSet fiducialSet)
        {
            foreach (Probe probe in fiducialSet.fiducials)
            {
                AddFiducial(probe);
            }
        }

        public void AddFiducial(Probe probe)
        {
            if (fiducials.IndexOf(probe) == -1)
            {
                fiducials.Add(probe);
                if (fiducials.Count > 2)
                    fiducials.RemoveAt(0);
            }
        }

        public bool FiducialExist(Probe probe)
        {
            int index = fiducials.IndexOf(probe);
            if (index == -1)
            {
                return false;
            }
            return true;
        }

        public void RemoveFiducial(Probe probe)
        {
            fiducials.Remove(probe);
        }

        public void AppendFigures(FigureGroup figureGroup)
        {
            int index = 1;
            foreach (Probe probe in fiducials)
            {
                TextFigure figure = new TextFigure(index.ToString(), new Point((int)probe.BaseRegion.Left, (int)probe.BaseRegion.Top), 
                                        new Font("Arial", 30, FontStyle.Bold), Color.Red);
                index++;
                figure.Selectable = false;

                figureGroup.AddFigure(figure);
            }
        }

        public bool IsContained(Target target)
        {
            foreach (Probe probe in fiducials)
            {
                if (target == probe.Target)
                    return true;
            }

            return false;
        }

        public void Inspect(InspParam inspParam, InspectionResult inspectionResult)
        {
            foreach (Probe probe in fiducials)
            {
                probe.Inspect(inspParam, inspectionResult);
            }
        }

        public bool IsFiducialInspected(InspectionResult inspectionResult)
        {
            if (fiducials.Count() == 0)
                return false;

            for (int i = 0; i < fiducials.Count(); i++)
            {
                VisionProbeResult fidProbeResult = inspectionResult.GetProbeResult(fiducials[i]) as VisionProbeResult;
                if (fidProbeResult == null)
                    return false;
            }

            return true;
        }

        public PositionAligner Calculate(InspectionResult inspectionResult)
        {
            PositionAligner positionAligner = new PositionAligner();

            if (fiducials.Count() == 0)
                return positionAligner;

            PointF firstFiducialPos = DrawingHelper.CenterPoint(fiducials[0].BaseRegion);

            try
            {
                VisionProbeResult fid1ProbeResult;
                fid1ProbeResult = inspectionResult.GetProbeResult(fiducials[0]) as VisionProbeResult;
                if (fid1ProbeResult == null || fid1ProbeResult is VisionProbeResult == false)
                    return positionAligner;

                positionAligner.Offset = fid1ProbeResult.AlgorithmResult.OffsetFound;
                positionAligner.Angle = -fid1ProbeResult.AlgorithmResult.AngleFound;
                positionAligner.RotationCenter = firstFiducialPos + positionAligner.Offset;

                if (fiducials.Count > 1)
                {
                    VisionProbeResult fid2ProbeResult = inspectionResult.GetProbeResult(fiducials[1]) as VisionProbeResult;
                    if (fid2ProbeResult != null)
                    {
                        PointF secondFiducialOrgPos = DrawingHelper.CenterPoint(fiducials[1].BaseRegion);
                        PointF secondFiducialOffsetPos = secondFiducialOrgPos + positionAligner.Offset;
                        PointF secondFiducialPosFinded = secondFiducialOrgPos + fid2ProbeResult.AlgorithmResult.OffsetFound.ToSize();

                        positionAligner.Angle = (float)MathHelper.GetAngle360(positionAligner.RotationCenter, secondFiducialOffsetPos, secondFiducialPosFinded);
                    }
                }
            }
            catch (InvalidCastException)
            {

            }

            return positionAligner;
        }

        private PointF GetGlobalPosition(Probe probe, Calibration calibration)
        {
            PointF probePos = calibration.PixelToWorld(DrawingHelper.CenterPoint(probe.BaseRegion));
            PointF fovCenterPos = calibration.GetFovCenterPos();

            probePos = PointF.Subtract(probePos, new SizeF(fovCenterPos));
            probePos = new PointF(probePos.X, -probePos.Y);

            return PointF.Add(probe.Target.TargetGroup.InspectionStep.BasePosition.ToPointF(), new SizeF(probePos));
        }

        // 일반 버전용
        public PositionAligner Calculate2Fid(InspectionResult inspectionResult, Calibration calibration, Camera camera)
        {
            PositionAligner positionAligner = new PositionAligner();

            if (fiducials.Count() < 2)
                return positionAligner;

            positionAligner.FovCenter = calibration.GetFovCenterPos();

            try
            {
                VisionProbeResult fid1ProbeResult = inspectionResult.GetProbeResult(fiducials[0].FullId) as VisionProbeResult;
                VisionProbeResult fid2ProbeResult = inspectionResult.GetProbeResult(fiducials[1].FullId) as VisionProbeResult;
                if (fid1ProbeResult == null || fid1ProbeResult is VisionProbeResult == false || fid1ProbeResult.Judgment == Judgment.Reject)
                {
                    positionAligner.Fid1Error = true;
                    return positionAligner;
                }

                if (fid2ProbeResult == null || fid2ProbeResult is VisionProbeResult == false || fid2ProbeResult.Judgment == Judgment.Reject)
                {
                    positionAligner.Fid2Error = true;
                    return positionAligner;
                }

                PointF fid1Pos = GetGlobalPosition(fid1ProbeResult.Probe, calibration);
                PointF fid2Pos = GetGlobalPosition(fid2ProbeResult.Probe, calibration);

                positionAligner.Offset = fid1ProbeResult.AlgorithmResult.RealOffsetFound;
                positionAligner.Offset = new SizeF(positionAligner.Offset.Width, -positionAligner.Offset.Height);

                float fidDistance = MathHelper.GetLength(fid1Pos, fid2Pos);
                positionAligner.DesiredFiducialDistance = fidDistance;

                PointF fid1OffsetPos = PointF.Add(fid1Pos, positionAligner.Offset);
                PointF fid2OffsetPos = PointF.Add(fid2Pos, positionAligner.Offset);

                //SizeF fid2Offset = new SizeF(fid2ProbeResult.AlgorithmResult.RealOffsetFound.Width,
                //                               -fid2ProbeResult.AlgorithmResult.RealOffsetFound.Height);
                SizeF fid2Offset = new SizeF(fid2ProbeResult.AlgorithmResult.RealOffsetFound.Width - positionAligner.Offset.Width,
                                              -fid2ProbeResult.AlgorithmResult.RealOffsetFound.Height - positionAligner.Offset.Height);
                //PointF fid2FoundPos = PointF.Add(fid2Pos, fid2Offset);
                PointF fid2FoundPos = PointF.Add(fid2OffsetPos, fid2Offset);

                float fidOffsetDistance = MathHelper.GetLength(fid1OffsetPos, fid2FoundPos);
                positionAligner.FiducialDistance = fidOffsetDistance;

                positionAligner.RotationCenter = fid1OffsetPos;
                positionAligner.Angle = (float)MathHelper.GetAngle360(fid1OffsetPos, fid2OffsetPos, fid2FoundPos);
            }
            catch (InvalidCastException)
            {

            }

            return positionAligner;
        }

        public PositionAligner Calculate2Fov(InspectionResult inspectionResult, Calibration calibration, Camera camera)
        {
            PositionAligner positionAligner = new PositionAligner();

            if (fiducials.Count() < 2)
                return positionAligner;

            positionAligner.FovCenter = new PointF(camera.ImageSize.Width/2, camera.ImageSize.Height / 2);

            try
            {
                VisionProbeResult fid1ProbeResult = inspectionResult.GetProbeResult(fiducials[0].FullId) as VisionProbeResult;
                VisionProbeResult fid2ProbeResult = inspectionResult.GetProbeResult(fiducials[1].FullId) as VisionProbeResult;
                if (fid1ProbeResult == null || fid1ProbeResult is VisionProbeResult == false || fid1ProbeResult.Judgment == Judgment.Reject)
                {
                    positionAligner.Fid1Error = true;
                    return positionAligner;
                }

                if (fid2ProbeResult == null || fid2ProbeResult is VisionProbeResult == false || fid2ProbeResult.Judgment == Judgment.Reject)
                {
                    positionAligner.Fid2Error = true;
                    return positionAligner;
                }

                PointF imageCenter = calibration.PixelToWorld(new PointF(camera.CameraInfo.Width / 2, camera.CameraInfo.Height / 2));
                PointF fid1CenterPos = calibration.PixelToWorld(DrawingHelper.CenterPoint(fid1ProbeResult.Probe.BaseRegion));
                PointF fid2CenterPos = calibration.PixelToWorld(DrawingHelper.CenterPoint(fid2ProbeResult.Probe.BaseRegion));

                PointF fid1CenterOffset = DrawingHelper.Subtract(fid1CenterPos, imageCenter);
                PointF fid2CenterOffset = DrawingHelper.Subtract(fid2CenterPos, imageCenter);

                fid1CenterOffset = new PointF(fid1CenterOffset.X, fid1CenterOffset.Y * (-1));
                fid2CenterOffset = new PointF(fid2CenterOffset.X, fid2CenterOffset.Y * (-1));

                SizeF realOffset1 = fid1ProbeResult.AlgorithmResult.RealOffsetFound;
                realOffset1 = new SizeF(realOffset1.Width, realOffset1.Height * (-1));
                SizeF realOffset2 = fid2ProbeResult.AlgorithmResult.RealOffsetFound;
                realOffset2 = new SizeF(realOffset2.Width, realOffset2.Height * (-1));
                positionAligner.Offset = realOffset1;

                PointF fid1RobotPos = fid1ProbeResult.Probe.Target.TargetGroup.InspectionStep.BasePosition.ToPointF();
                PointF fid1TeachPos = DrawingHelper.Add(fid1RobotPos, fid1CenterOffset);

                PointF fid2RobotPos = fid2ProbeResult.Probe.Target.TargetGroup.InspectionStep.BasePosition.ToPointF();
                PointF fid2TeachPos = DrawingHelper.Add(fid2RobotPos, fid2CenterOffset);

                positionAligner.DesiredFiducialDistance = MathHelper.GetLength(fid2TeachPos, fid1TeachPos);

                PointF fid1Pos = PointF.Add(fid1TeachPos, realOffset1);
                PointF fid2Pos = PointF.Add(fid2TeachPos, realOffset2);

                positionAligner.FiducialDistance = MathHelper.GetLength(fid1Pos, fid2Pos);
                positionAligner.FiducialDistanceOffset = positionAligner.FiducialDistance - positionAligner.DesiredFiducialDistance;

                positionAligner.RotationCenter = fid1Pos;

                SizeF fid2ActualOffset = DrawingHelper.Subtract(realOffset2, realOffset1);
                PointF fid2OffsetPos = PointF.Add(fid2TeachPos, fid2ActualOffset);

                positionAligner.Angle = (float)MathHelper.GetAngle360(fid1TeachPos, fid2TeachPos, fid2OffsetPos);
            }
            catch (InvalidCastException)
            {

            }

            return positionAligner;
        }

        // GSMAT용
        public PositionAligner Calculate2Gsmatt(InspectionResult inspectionResult, Calibration calibration, Camera camera)
        {
            PositionAligner positionAligner = new PositionAligner();

            if (fiducials.Count() < 2)
                return positionAligner;

            try
            {
                VisionProbeResult fid1ProbeResult = inspectionResult.GetProbeResult(fiducials[0].FullId) as VisionProbeResult;
                VisionProbeResult fid2ProbeResult = inspectionResult.GetProbeResult(fiducials[1].FullId) as VisionProbeResult;
                if (fid1ProbeResult == null || fid1ProbeResult is VisionProbeResult == false || fid1ProbeResult.Judgment == Judgment.Reject)
                {
                    positionAligner.Fid1Error = true;
                    return positionAligner;
                }

                if (fid2ProbeResult == null || fid2ProbeResult is VisionProbeResult == false || fid2ProbeResult.Judgment == Judgment.Reject)
                {
                    positionAligner.Fid2Error = true;
                    return positionAligner;
                }

                PointF imageCenter = calibration.PixelToWorld(new PointF(camera.CameraInfo.Width / 2, camera.CameraInfo.Height / 2));
                PointF fid1CenterPos = calibration.PixelToWorld(DrawingHelper.CenterPoint(fid1ProbeResult.Probe.BaseRegion));
                PointF fid2CenterPos = calibration.PixelToWorld(DrawingHelper.CenterPoint(fid2ProbeResult.Probe.BaseRegion));

                PointF fid1CenterOffset = DrawingHelper.Subtract(fid1CenterPos, imageCenter);
                PointF fid2CenterOffset = DrawingHelper.Subtract(fid2CenterPos, imageCenter);

                positionAligner.Offset = fid2ProbeResult.AlgorithmResult.RealOffsetFound;

                SizeF fidDistance = new SizeF(fid2CenterOffset.X + secondFiducialOffset.Width - fid1CenterOffset.X, fid2CenterOffset.Y + secondFiducialOffset.Height - fid1CenterOffset.Y);

                PointF fid2Pos = DrawingHelper.ToPointF(fid2ProbeResult.AlgorithmResult.RealOffsetFound);
                PointF fid1Pos = PointF.Add(fid2Pos, fidDistance);
                SizeF fid1ActualOffset = DrawingHelper.Subtract(fid1ProbeResult.AlgorithmResult.RealOffsetFound, fid2ProbeResult.AlgorithmResult.RealOffsetFound);
                PointF fid1OffsetPos = PointF.Add(fid1Pos, fid1ActualOffset);

                positionAligner.RotationCenter = fid2Pos;

                positionAligner.Angle = (float)MathHelper.GetAngle360(fid2Pos, fid1Pos, fid1OffsetPos);
            }
            catch (InvalidCastException)
            {

            }

            return positionAligner;
        }
    }
}
