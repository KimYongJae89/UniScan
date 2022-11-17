using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

using DynMvp.UI;
using DynMvp.Base;
using DynMvp.Vision;
using System.Diagnostics;
using DynMvp.Devices.Dio;
using DynMvp.Devices;
using DynMvp.InspData;
using DynMvp.Authentication;

namespace DynMvp.Data.UI
{
    public class TeachHandlerProbe : TeachHandler
    {
        private string probeTypeName;
        public string ProbeTypeName
        {
            get { return probeTypeName; }
            set { probeTypeName = value; }
        }

        private string algorithmTypeName;
        public string AlgorithmTypeName
        {
            get { return algorithmTypeName; }
            set { algorithmTypeName = value; }
        }

        bool singleTargetMode;
        public bool SingleTargetMode
        {
            get { return singleTargetMode; }
            set { singleTargetMode = value; }
        }

        PositionAligner positionAligner;
        public PositionAligner PositionAligner
        {
            set { positionAligner = value; }
        }

        public override bool IsEditable()
        {
            return true;
        }

        public override bool IsSingleSelected()
        {
            return selectedObjs.Count == 1;
        }

        public ITeachObject GetSingleSelected()
        {
            lock(selectedObjs)
            {
                if (selectedObjs.Count == 1)
                    return selectedObjs[0];
            }

            return null;
        }

        public override RectangleF GetBoundRect()
        {
            RectangleF unionRect = RectangleF.Empty;
            foreach (ITeachObject teachObject in selectedObjs)
            {
                unionRect = RectangleF.Union(unionRect, teachObject.BaseRegion.ToRectangleF());
            }

            return unionRect;
        }

        public Probe GetFirstSelectedProbe()
        {
            foreach (ITeachObject teachObject in selectedObjs)
            {
                if (teachObject is Probe)
                {
                    return (Probe)teachObject;
                }
            }

            return null;
        }

        public List<Probe> GetSelectedProbe()
        {
            List<Probe> selectedPorbeList = new List<Probe>();
            foreach (ITeachObject teachObject in selectedObjs)
            {
                if (teachObject is Probe)
                {
                    selectedPorbeList.Add((Probe)teachObject);
                }
            }
            
            return selectedPorbeList;
        }

        public Probe GetSingleSelectedProbe()
        {
            Probe setSingleSelectedProbe = null;
            int probeCount = 0;
            foreach (ITeachObject teachObject in selectedObjs)
            {
                if (teachObject is Probe)
                {
                    if (setSingleSelectedProbe == null)
                        setSingleSelectedProbe = (Probe)teachObject;

                    probeCount++;
                }
            }

            if (probeCount == 1)
                return setSingleSelectedProbe;

            return null;
        }

        public List<Target> GetTargetList()
        {
            List<Target> targetList = new List<Target>();
            foreach (ITeachObject teachObject in selectedObjs)
            {
                if (teachObject is Probe)
                {
                    Probe probe = (Probe)teachObject;

                    if (targetList.IndexOf(probe.Target) == -1)
                        targetList.Add(probe.Target);
                }
                else if (teachObject is Target)
                {
                    Target target = (Target)teachObject;

                    if (targetList.IndexOf(target) == -1)
                        targetList.Add(target);
                }
            }

            return targetList;
        }

        public override void GetFigures(FigureGroup activeFigures, FigureGroup backgroundFigures, FigureGroup tempFigureGroup, InspectionResult inspectionResult)
        {
            activeFigures.Selectable = false;

            foreach (ITeachObject teachObject in selectedObjs)
            {
                teachObject.AppendAdditionalFigures(tempFigureGroup);
            }

            if (inspectionResult != null)
                // Inspection Result를 Temp Figure로 얻어온다.
            {
                Pen redPen = new Pen(Color.Red);
                Pen yellowPen = new Pen(Color.Yellow);

                foreach (ITeachObject teachObject in selectedObjs)
                {
                    if (teachObject is Probe)
                    {
                        Probe probe = (Probe)teachObject;

                        ProbeResult probeResult = inspectionResult.GetProbeResult(probe);
                        if (probeResult != null)
                        {
                            probeResult.AppendResultFigures(tempFigureGroup);
                        }
                    }
                    else if (teachObject is Target)
                    {
                        Target target = (Target)teachObject;
                        
                        foreach(Probe probe in target.ProbeList)
                        {
                            ProbeResult probeResult = inspectionResult.GetProbeResult(probe);
                            if (probeResult != null)
                            {
                                probeResult.AppendResultFigures(tempFigureGroup);
                            }
                        }
                        //ProbeResult probeResult = inspectionResult.GetTargetResult(target.Name);
                        //if (probeResult != null)
                        //{
                        //    probeResult.AppendResultFigures(tempFigureGroup);
                        //}
                    }
                }
            }
        }

        public override void ClearSelection()
        {
            selectedObjs.Clear();
        }

        public override void ShowTracker(DrawBox drawBox)
        {
            foreach (ITeachObject teachObject in selectedObjs)
            {
                drawBox.SelectFigureByTag(teachObject);
            }
        }

        public override bool IsSelectable(Figure figure)
        {
            ITeachObject teachObject = figure.Tag as ITeachObject;
            if (teachObject != null)
            {
                if (teachObject is Target && singleTargetMode)
                    return false;
            }

            return true;
        }

        public override void Select(Figure figure)
        {
            ITeachObject teachObject = figure.Tag as ITeachObject;
            if (teachObject != null)
            {
                if (teachObject is Target && singleTargetMode)
                    return;

                selectedObjs.Add(teachObject);
            }
        }

        public override void Unselect(Figure figure)
        {
            ITeachObject teachObject = figure.Tag as ITeachObject;
            if (teachObject != null)
            {
                if (teachObject is Target && singleTargetMode)
                    return;

                selectedObjs.Remove(teachObject);
            }
        }

        public void Select(ITeachObject teachObject)
        {
			if (teachObject == null)
                return;

            selectedObjs.Add(teachObject);
        }

        public override void Copy(List<Figure> figureList)
        {
            selectedObjs.Clear();

            List<Probe> newProbes = new List<Probe>();

            foreach (Figure figure in figureList)
            {
                Probe probe = figure.Tag as Probe;
                if (probe != null)
                {
                    //Probe newProbe = (Probe)probe.Clone();

                    //RotatedRect figureRect = figure.GetRectangle();
                    //if (Rectangle.Intersect(boundary, figureRect.ToRectangle()) != figureRect.ToRectangle())
                    //{
                    //    figure.SetRectangle(target.Region);
                    //    continue;
                    //}

                    //newProbe.Region = figureRect;

                    //target.Add(newProbe);

                    //selectedProbes.Add(newProbe);
                    selectedObjs.Add(probe);
                }
            }        
        }

        public override void Move(List<Figure> figureList)
        {
            UserHandler.Instance().DoTask("TeachMove", () =>
            {
                foreach (Figure figure in figureList)
                {
                    RotatedRect rectangle = figure.GetRectangle();
                    Rectangle figureRect = rectangle.ToRectangle();

                    ITeachObject teachObject = figure.Tag as ITeachObject;
                    if (teachObject != null)
                    {
                        if (boundary.Contains(figureRect) == false)
                        {
                            figure.SetRectangle(teachObject.BaseRegion);
                            continue;
                        }

                        teachObject.UpdateRegion(rectangle, positionAligner);
                    }
                }
            });
        }

        public void SetAddType(string probeTypeName, string algorithmTypeName)
        {
            this.probeTypeName = probeTypeName;
            this.algorithmTypeName = algorithmTypeName;
        }

        public override void AddObject(Rectangle rectangle, Point startPos, Point endPos, Bitmap wholeImage)
        {
            Debug.Assert(false);

            //if (rectangle.IsEmpty == true)
            //    return;

            //Probe probe = null;
            //switch(probeTypeName)
            //{
            //    case "Vision":
            //        probe = CreateVisionProbe(rectangle, wholeImage);   break;
            //    case "Io":
            //        probe = new IoProbe();          break;
            //    case "Serial":
            //        probe = new SerialProbe();      break;
            //    case "Daq":
            //        probe = new DaqProbe();         break;
            //    case "Compute":
            //        probe = new ComputeProbe();     break;
            //}

            //if (probe == null)
            //    return;

            //probe.Region = new RotatedRect(rectangle, 0);

            //target.AddProbe(probe);

            //selectedProbes.Clear();
            //selectedProbes.Add(probe);
        }

        //private Probe CreateVisionProbe(Rectangle rectangle, Bitmap wholeImage)
        //{
        //    Algorithm algorithm = CreateAlgorithm(rectangle, wholeImage);
        //    if (algorithm == null)
        //        return null;

        //    VisionProbe visionProbe = (VisionProbe)ProbeFactory.Create(ProbeType.Vision);

        //    visionProbe.InspAlgorithm = algorithm;

        //    visionProbe.InspAlgorithm.SourceImageType = ImageType.Color;
        //    if (target.GetPixelFormat() == PixelFormat.Format8bppIndexed)
        //        visionProbe.InspAlgorithm.SourceImageType = ImageType.Grey;

        //    return visionProbe;
        //}

        //protected virtual Algorithm CreateAlgorithm(Rectangle rectangle, Bitmap wholeImage)
        //{
        //    switch(algorithmTypeName)
        //    {
        //        case "PatternMatching":

        //            if (imageClipped)
        //            {
        //                rectangle.X -= (int)target.Region.Left;
        //                rectangle.Y -= (int)target.Region.Top;
        //            }

        //            PatternMatching patternMatching = new PatternMatching();

        //            Pattern pattern = AlgorithmBuilder.CreatePattern();

        //            AlgoImage algoImage = ImageBuilder.Build(patternMatching.GetAlgorithmType(),
        //                                    ImageHelper.ClipImage((Bitmap)wholeImage, rectangle), ImageType.Grey, ImageBandType.Luminance);

        //            pattern.Train(algoImage);

        //            return patternMatching;
        //        case "BinaryCounter":
        //            return new BinaryCounter();
        //        case "BrightnessChecker":
        //            return new BrightnessChecker();
        //        case "WidthChecker":
        //            return new WidthChecker();
        //        case "LineChecker":
        //            return new LineChecker();
        //        case "BoltChecker":
        //            return new BoltChecker();
        //        case "ColorMatchChecker":
        //            return AlgorithmBuilder.CreateColorMatchChecker();
        //    }

        //    return null;
        //}

        public override bool IsSelected()
        {
            return selectedObjs.Count > 0;
        }

        public override void DeleteObject()
        {
            List<Target> targetList = GetTargetList();

            if (targetList.Count > 1)
            {
                foreach (Target target in targetList)
                {
                    target.TargetGroup.RemoveTarget(target);
                }
            }
            else if (targetList[0] != null)
            {
                Target target = targetList[0];
                foreach (ITeachObject teachObject in selectedObjs)
                {
                    Probe probe = teachObject as Probe;
                    if (probe != null)
                        target.RemoveProbe(probe);
                }

                if (target.ProbeList.Count == 0)
                {
                    target.TargetGroup.RemoveTarget(target);
                }

                foreach (ITeachObject teachObject in selectedObjs)
                {
                    Target selTarget = teachObject as Target;
                    if (selTarget != null)
                        selTarget.TargetGroup.RemoveTarget(selTarget);
                }
            }

            selectedObjs.Clear();
        }

        public MarkerProbe GetMarkerProbe(MarkerType markerType)
        {
            List<Target> targetList = GetTargetList();

            foreach (Target target in targetList)
            {
                MarkerProbe markerProbe = target.GetMarkerProbe(markerType);
                if (markerProbe != null)
                    return markerProbe;
            }

            return null;
        }

        public override InspectionResult Inspect(DeviceImageSet deviceImageList, bool saveDebugImage,
            Calibration calibration, DigitalIoHandler digitalIoHandler, InspectionResult inspectionResult)
        {
            //InspectionResult inspectionResult = new InspectionResult();

            LogHelper.Debug(LoggerType.Operation, "ModellerPage - testTargetButton_Click");

            InspParam inspParam = new InspParam(ProbeResult.DefaultSequence, deviceImageList, saveDebugImage, false, false, false, ImageFormat.Bmp, 0);
            inspParam.InspectionResult = inspectionResult;
            inspParam.CameraCalibration = calibration;
            inspParam.DigitalIoHandler = digitalIoHandler;
            inspParam.TeachMode = true;
            inspParam.PixelRes3d = PixelRes3d;

            MarkerProbe mergeTarget = GetMarkerProbe(MarkerType.MergeTarget);
            if (mergeTarget != null)
            {
                mergeTarget.Inspect(inspParam, inspectionResult);
            }

            MarkerProbe mergeSource = GetMarkerProbe(MarkerType.MergeSource);
            if (mergeSource != null)
            {
                mergeSource.Inspect(inspParam, inspectionResult);
            }

            InspectionResult targetInspectionResult = new InspectionResult();

            List<Target> targetList = GetTargetList();

            if (targetList.Count > 1)
            {
                foreach (Target target in targetList)
                {
                    InspectTarget(target, inspParam, targetInspectionResult);
                }
            }
            else if (targetList.Count == 1)
            {
                /*Target target = targetList[0];

                FiducialSet fiducialSet = target.TargetGroup.FiducialSet;
                fiducialSet.Inspect(inspParam, targetInspectionResult);

                inspParam.PositionAligner = fiducialSet.Calculate(targetInspectionResult);
                
                if (fiducialSet.IsContained(target) == false)*/
                Target target = targetList[0];
                target.Inspect(inspParam, targetInspectionResult);
            }

            inspectionResult.AddProbeResult(targetInspectionResult);

            return targetInspectionResult;
        }

        void InspectTarget(Target target, InspParam inspParam, InspectionResult targetInspectionResult)
        {
            FiducialSet fiducialSet = target.TargetGroup.FiducialSet;
            fiducialSet.Inspect(inspParam, targetInspectionResult);

            inspParam.PositionAligner = fiducialSet.Calculate(targetInspectionResult);

            if (fiducialSet.IsContained(target) == false)
                target.Inspect(inspParam, targetInspectionResult);
        }
    }
}
