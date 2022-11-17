using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace DynMvp.Data.UI
{
    public class TeachHandlerTarget : TeachHandler
    {
        private Model model = null;
        public Model Model
        {
            set { model = value; }
        }

        private int inspectionStepNo;
        public int InspectionStepNo
        {
            set { inspectionStepNo = value; }
        }

        private TargetGroup targetGroup = null;
        public TargetGroup TargetGroup
        {
            set { targetGroup = value; }
        }

        private List<Target> selectedTargets = new List<Target>();
        public List<Target> SelectedTargets
        {
            get { return selectedTargets; }
            set { selectedTargets = value; }
        }

        PositionAligner positionAligner;
        public PositionAligner PositionAligner
        {
            set { positionAligner = value; }
        }

        public override bool IsEditable()
        {
            return targetGroup != null;
        }

        public override bool IsSingleSelected()
        {
            return selectedTargets.Count == 1;
        }

        public override bool IsSelectable(Figure figure)
        {
            return true;
        }

        public override RectangleF GetBoundRect()
        {
            RectangleF unionRect = RectangleF.Empty;
            foreach(Target target in selectedTargets)
            {
                unionRect = RectangleF.Union(unionRect, target.BaseRegion.ToRectangleF());
            }

            return unionRect;
        }

        public override void GetFigures(FigureGroup activeFigures, FigureGroup backgroundFigures, FigureGroup tempFigureGroup, InspectionResult inspectionResult)
        {
            activeFigures.Selectable = false;
            targetGroup.AppendFigures(activeFigures, null);

            foreach (InspectionStep inspectionStep in model)
            {
                if (inspectionStep.StepNo == inspectionStepNo)
                    continue;

                Pen pen = new Pen(Color.LightCyan, 1.0F);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                inspectionStep.GetTargetGroup(targetGroup.GroupId).AppendFigures(backgroundFigures, pen);
            }

            targetGroup.FiducialSet.AppendFigures(activeFigures);

            if (inspectionResult != null)
            {
                Pen redPen = new Pen(Color.Red);
                Pen yellowPen = new Pen(Color.Yellow);
                foreach (Target target in targetGroup)
                {
                    if (inspectionResult.IsDefected(target))
                    {
                        target.AppendFigures(tempFigureGroup, redPen, false);
                    }
                    else if (inspectionResult.IsPass(target))
                    {
                        target.AppendFigures(tempFigureGroup, yellowPen, false);
                    }
                }
            }
        }

        public override void ClearSelection()
        {
            selectedTargets.Clear();
        }

        public override void Copy(List<Figure> figureList)
        {
            selectedTargets.Clear();

            List<Target> newTargets = new List<Target>();

            foreach (Figure figure in figureList)
            {
                Target target = figure.Tag as Target;
                if (target != null)
                {
                    Target newTarget = (Target)target.Clone();

                    RotatedRect figureRect = figure.GetRectangle();
                    if (boundary.Contains(figureRect.ToRectangle()) == false)
                    {
                        figure.SetRectangle(target.BaseRegion);
                        continue;
                    }

                    newTarget.UpdateRegion(figureRect, positionAligner);

                    targetGroup.AddTarget(newTarget);

                    selectedTargets.Add(newTarget);
                }
            }
        }

        public override void ShowTracker(DrawBox drawBox)
        {
            foreach (Target target in selectedTargets)
            {
                drawBox.SelectFigureByTag(target);
            }
        }

        public override void Select(Figure figure)
        {
            Target target = figure.Tag as Target;
            if (target != null)
            {
                selectedTargets.Add(target);
            }
        }

        public void Select(Target target)
        {
            if (target == null)
                return;

            selectedTargets.Add(target);
        }

        public override void Move(List<Figure> figureList)
        {
            foreach (Figure figure in figureList)
            {
                Target target = figure.Tag as Target;
                if (target != null)
                {
                    RotatedRect rectangle = figure.GetRectangle();

                    Rectangle figureRect = rectangle.ToRectangle();
                    if (boundary.Contains(figureRect) == false)
                    {
                        figure.SetRectangle(target.BaseRegion);
                        continue;
                    }

                    target.UpdateRegion(rectangle, positionAligner);
                }
            }
        }

        public override void AddObject(Rectangle rectangle, Point startPos, Point endPos, Bitmap wholeImage)
        {
            if (rectangle.IsEmpty == true)
                return;

            RotatedRect targetRegion = new RotatedRect(DrawingHelper.ToRectF(rectangle), 0);

            Target target = new Target();

            target.BaseRegion = targetRegion;

            target.UpdateRegion(targetRegion, positionAligner);
            target.Image = Image2D.FromBitmap(ImageHelper.ClipImage(wholeImage, targetRegion.ToRectangle()));

            targetGroup.AddTarget(target);

            selectedTargets.Clear();
            selectedTargets.Add(target);
        }

        public override bool IsSelected()
        {
            return selectedTargets.Count > 0;
        }

        public override void DeleteObject()
        {
            foreach (Target target in selectedTargets)
            {
                targetGroup.RemoveTarget(target);
            }

            selectedTargets.Clear();
        }

        public override InspectionResult Inspect(DeviceImageSet deviceImageSet, bool saveDebugImage, Calibration calibration, DigitalIoHandler digitalIoHandler, InspectionResult inspectionResult)
        { 
            InspectionResult inspectionResultTargets = new InspectionResult();
            foreach (Target target in selectedTargets)
            {
                target.OnPreInspection();

                LogHelper.Debug(LoggerType.Operation, "ModellerPage - testTargetButton_Click");

                int groupId = targetGroup.GroupId;

                InspParam inspParam = new InspParam(ProbeResult.DefaultSequence, deviceImageSet, 
                    saveDebugImage, false, false, false, ImageFormat.Jpeg, 0);
                inspParam.InspectionResult = inspectionResult;
                inspParam.CameraCalibration = calibration;
                inspParam.DigitalIoHandler = digitalIoHandler;
                inspParam.TeachMode = true;

                InspectionResult targetInspectionResult = new InspectionResult();

                FiducialSet fiducialSet = targetGroup.FiducialSet;
                fiducialSet.Inspect(inspParam, targetInspectionResult);

                inspParam.PositionAligner = targetGroup.FiducialSet.Calculate(targetInspectionResult);

                if (fiducialSet.IsContained(target) == false)
                    target.Inspect(inspParam, targetInspectionResult);

                inspectionResultTargets.AddProbeResult(targetInspectionResult);

                //ShowInspResult(selectedTargetInspectionResult);

                //InspectionResult lastTargetResult = null;

                //if (lastTargetResult != null)
                //{
                //    lastTargetResult = lastInspectionResult.GetTargetResult(target);
                //}

                //tryInspectionResultView.AddResult(selectedTargetInspectionResult, lastTargetResult);

                //RotatedRect targetRegion = inspParam.PositionAligner.Align(target.Region);
                //targetRegion.Offset(inspParam.FiducialProbeOffset.Width, inspParam.FiducialProbeOffset.Height);

                ////                RectangleF targetClipRegion = targetRegion.GetBoundRect();

                //Bitmap regionImage = ImageHelper.ClipImage((Bitmap)cameraImage.Image, targetRegion);
                //targetParamControl.UpdateTargetImage(regionImage);
            }

            return inspectionResultTargets;
        }

        public override void Unselect(Figure figure)
        {
            throw new NotImplementedException();
        }
    }
}
