using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

using DynMvp.Vision;
using DynMvp.UI;
using DynMvp.Base;
using DynMvp.InspData;
using System.Drawing.Imaging;
using System.IO;
using System.ComponentModel;

namespace DynMvp.Data
{
    public class VisionProbeResult : ProbeResult
    {
        string algorithmType;
        public string AlgorithmType
        {
            get { return (Probe != null ? ((VisionProbe)Probe).InspAlgorithm.GetAlgorithmType() : algorithmType); }
            set { algorithmType = value; }
        }

        public VisionProbeResult()
        {
            shortResultMessage = StringManager.GetString(this.GetType().FullName, "AlgorithmResult is invalid. Check the probe region.");

            resultMessage.AddTextLine(shortResultMessage);
        }

        public VisionProbeResult(Probe probe, AlgorithmResult algorithmResult, ImageD image)
        {
            this.algorithmResult = algorithmResult;

            if (algorithmResult != null)
            {
                this.Judgment = Judgment.Accept;
                if (/*probe?.ProbeResultType == ProbeResultType.Judgement &&*/ algorithmResult.Good == false)
                    this.Judgment = Judgment.Reject;

                ResultValueList.Add(new ProbeResultValue("Result", algorithmResult.Good));
                foreach (AlgorithmResultValue algorithmResultValue in algorithmResult.ResultValueList)
                {
                    ResultValueList.Add(new ProbeResultValue(algorithmResultValue));
                }

                this.shortResultMessage = algorithmResult.ShortResultMessage;
                this.resultMessage = algorithmResult.MessageBuilder.Clone();
            }
            else
            {
                this.Judgment = Judgment.Reject;

                List<ProbeResultValue> resultValueList = probe.GetResultValues();
                foreach (ProbeResultValue resultValue in resultValueList)
                {
                    ResultValueList.Add(resultValue);
                }
            }

            this.Image = image;
            this.Probe = probe;
        }

        public VisionProbeResult(Probe probe, string shortResultMessage)
        {
            this.algorithmResult = null;

            this.Judgment = Judgment.Reject;

            List<ProbeResultValue> resultValueList = probe.GetResultValues();
            foreach (ProbeResultValue resultValue in resultValueList)
            {
                ResultValueList.Add(resultValue);
            }

            this.shortResultMessage = shortResultMessage;
            this.Image = null;
            this.Probe = probe;
        }

        ~VisionProbeResult()
        {
            //if (this.Image != null)
            //    this.Image.Dispose();
        }

        private AlgorithmResult algorithmResult;
        public AlgorithmResult AlgorithmResult
        {
            get { return algorithmResult; }
            set { algorithmResult = value; }
        }

        public override void InvertJudgment()
        {
            if (Judgment == Judgment.Accept)
            {
                Judgment = Judgment.Reject;
                algorithmResult.Good = false;
            }
            else
            {
                Judgment = Judgment.Accept;
                algorithmResult.Good = true;
            }
        }

        public override void BuildResultMessage(MessageBuilder totalResultMessage)
        {
            if (resultMessage == null)
            {
                resultMessage = new MessageBuilder();
                resultMessage.AddText(shortResultMessage);
            }

            totalResultMessage.Append(resultMessage);
        }

        public override void AppendResultFigures(FigureGroup figureGroup, bool useTargetCoord)
        {
            Pen pen;
            if (Judgment == Judgment.Reject)
                pen = new Pen(Color.Red, 4.0F);
            else if (Judgment == Judgment.FalseReject)
                pen = new Pen(Color.Yellow, 4.0F);
            else
                pen = new Pen(Color.Blue, 4.0F);

            PointF offset = new PointF(0, 0);
            
            RotatedRect resultRect = new RotatedRect();
            if (algorithmResult != null)
            {
                resultRect = algorithmResult.ResultRect;
            }

            if (resultRect.IsEmpty && Probe != null)
            {
                resultRect = Probe.AlignedRegion;
            }

            resultRect.Offset(offset);

            RectangleFigure figure = new RectangleFigure(resultRect, pen);
            figure.Selectable = false;
            figure.Tag = this;

            figureGroup.AddFigure(figure);

            if (algorithmResult != null)
            {
                PointF probeOffset = resultRect.GetBoundRect().Location;
                algorithmResult.AppendResultFigures(figureGroup, probeOffset);
            }
        }

        public override string ToString()
        {
            return shortResultMessage;
        }
    }

    public class VisionProbe : Probe
    {
        private Algorithm inspAlgorithm;
        public Algorithm InspAlgorithm
        {
            get { return inspAlgorithm; }
            set { inspAlgorithm = value; }
        }

        [Browsable(false)]
        protected FigureGroup maskFigures = new FigureGroup();
        public FigureGroup MaskFigures
        {
            get { return maskFigures; }
            set { maskFigures = value; }
        }

        protected FigureGroup alignedMaskFigures = new FigureGroup();
        public FigureGroup AlignedMaskFigures
        {
            get { return alignedMaskFigures; }
            set { alignedMaskFigures = value; }
        }

        public override bool IsControllable()
        {
            if (inspAlgorithm as ObjectFinder != null)
                return false;

            if (inspAlgorithm as BoltChecker != null)
                return false;

            return true;
        }

        public override List<ProbeResultValue> GetResultValues()
        {
            List<AlgorithmResultValue> algorithmResultValueList = inspAlgorithm.GetResultValues();
            List<ProbeResultValue> probeResultValueList = new List<ProbeResultValue>();

            foreach (AlgorithmResultValue algorithmResultValue in algorithmResultValueList)
            {
                probeResultValueList.Add(new ProbeResultValue(algorithmResultValue));
            }

            return probeResultValueList;
        }

        public override Object Clone()
        {
            VisionProbe visionProbe = new VisionProbe();
            visionProbe.Copy(this);

            return visionProbe;
        }

        public override bool SyncParam(Probe srcProbe, IProbeFilter probeFilter)
        {
            VisionProbe srcVisionProbe = (VisionProbe)srcProbe;

            if (srcVisionProbe.InspAlgorithm.GetAlgorithmType() == InspAlgorithm.GetAlgorithmType())
            {
                if (base.SyncParam(srcProbe, probeFilter) == false)
                    return false;

                inspAlgorithm.SyncParam(srcVisionProbe.InspAlgorithm);
            }

            return true;
        }

        public override void Clear()
        {
            inspAlgorithm.Clear();
        }

        public override void PrepareInspection()
        {
            inspAlgorithm.PrepareInspection();
        }

        public override void Copy(Probe probe)
        {
            base.Copy(probe);

            VisionProbe visionProbe = (VisionProbe)probe;

            if (visionProbe.InspAlgorithm.IsAlgorithmPoolItem == true)
                inspAlgorithm = visionProbe.InspAlgorithm;
            else
                inspAlgorithm = visionProbe.InspAlgorithm.Clone();
        }

        public override string GetProbeTypeDetailed()
        {
            return inspAlgorithm.GetAlgorithmType();
        }

        public override string GetProbeTypeShortName()
        {
            return inspAlgorithm.GetAlgorithmTypeShort();
        }

        public override string[] GetPreviewNames()
        {
            return inspAlgorithm.GetPreviewNames();
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup)
        {
            if (inspAlgorithm.Enabled == false)
            {
                RectangleFigure figure = new RectangleFigure(AlignedRegion, new Pen(Color.LightGray, 1.0F));
                figure.Selectable = false;
                figureGroup.AddFigure(figure);

                return;
            }

            Searchable searchable = inspAlgorithm as Searchable;
            if (searchable != null)
            {
                Size searchRange = searchable.GetSearchRangeSize();
                RotatedRect searchRangeRect = RotatedRect.Inflate(AlignedRegion, searchRange.Width, searchRange.Height);

                Pen pen = new Pen(Color.Cyan, 1.0F);
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                RectangleFigure figure = new RectangleFigure(searchRangeRect, pen);
                figure.Selectable = false;
                figureGroup.AddFigure(figure);
            }

            inspAlgorithm.AppendAdditionalFigures(figureGroup, AlignedRegion);
        }

        public override ImageD PreviewFilterResult(ImageD targetImage, int previewFilterType, bool useTargetCoordinate)
        {
            ImageD filteredImage = targetImage.Clone();

            RotatedRect filterRegion = AlignedRegion;

            bool useWholeImage = false;
            inspAlgorithm.AdjustInspRegion(ref filterRegion, ref useWholeImage);

            RectangleF clipRegionF = filterRegion.GetBoundRect();
            if (useTargetCoordinate)
                clipRegionF.Offset(-Target.AlignedRegion.X, -Target.AlignedRegion.Y);

            Rectangle clipRegion = Rectangle.Round(clipRegionF);

            ImageD clipImage = inspAlgorithm.PreFilterClipImage(filteredImage, clipRegion, previewFilterType);

            ImageD clipFilteredImage = inspAlgorithm.Filter(clipImage, previewFilterType);
#if DEBUG            
            clipImage.SaveImage("D:\\Filterd1.bmp", ImageFormat.Bmp);
            clipFilteredImage.SaveImage("D:\\Filterd2.bmp", ImageFormat.Bmp);
#endif

            filteredImage = inspAlgorithm.PostFilterCopyForm(filteredImage, clipFilteredImage, clipRegion, previewFilterType);

            return filteredImage;
        }

        //private RotatedRect AlignRegion(PositionAligner positionAligner)
        //{
        //    RotatedRect newRegion = BaseRegion;

        //    if (positionAligner != null)
        //    {
        //        newRegion = positionAligner.Align(BaseRegion, Target.TargetGroup.InspectionStep.AlignedPosition);
        //    }

        //    return newRegion;
        //}

        public override ProbeResult Inspect(InspParam inspParam)
        {
            if (inspAlgorithm.Enabled == false)
                return new VisionProbeResult(this, "Algorithm is disabled");

            if (AlgorithmBuilder.IsAlgorithmEnabled(inspAlgorithm.GetAlgorithmType()) == false)
                return new VisionProbeResult(this, String.Format("{0} Algorithm is not licensed.", inspAlgorithm.GetAlgorithmType()));


            LogHelper.Debug(LoggerType.Inspection, String.Format("Insepct Probe - {0}", FullId));

            bool saveDebugImage = inspParam.SaveDebugImage;
            string path = String.Format("{0}\\{1}", Configuration.TempFolder, FullId);
            DebugContext debugContext = new DebugContext(saveDebugImage, path);

            ImageD grabImage = null;

            if (inspAlgorithm.CanProcess3dImage() == true)
            {
                grabImage = inspParam.DeviceImageSet.Image3D;
            }
            else
            {
                if (inspParam.DeviceImageSet.ImageList2D.Count <= LightTypeIndex)
                {
                    LogHelper.Debug(LoggerType.Inspection, String.Format("Inspection Skipped. {0}", Target.FullId));
                    return new VisionProbeResult(this, "Number of grab image is less than lightTypeIndex");
                }

                grabImage = inspParam.DeviceImageSet.ImageList2D[LightTypeIndex];
            }

            if (grabImage == null)
            {
                LogHelper.Debug(LoggerType.Inspection, String.Format("Inspection Skipped. {0}", Target.FullId));
                return new VisionProbeResult(this, "Grab image is null");
            }
            LogHelper.Debug(LoggerType.Inspection, String.Format("Insepct Probe - Align Probe : {0}", FullId));

            RotatedRect probeRegionInFov = AlignedRegion; // AlignRegion(inspParam.PositionAligner);

            RectangleF fovRegion = new RectangleF(0, 0, grabImage.Width, grabImage.Height);

            if (Adjustable)
            {
                if (inspParam.FiducialProbeAngle != 0)
                {
                    probeRegionInFov.Angle = inspParam.FiducialProbeAngle;
                    PointF probeCenterPt = DrawingHelper.CenterPoint(probeRegionInFov);
                    PointF fiducialCenterPt = DrawingHelper.CenterPoint(inspParam.FiducialProbeRect);
                    PointF rotatePoint = MathHelper.Rotate(probeCenterPt, fiducialCenterPt, inspParam.FiducialProbeAngle);

                    probeRegionInFov.Offset(inspParam.FiducialProbeOffset);
                    probeRegionInFov.Offset(rotatePoint.X - probeCenterPt.X, rotatePoint.Y - probeCenterPt.Y);
                }
                else
                {
                    probeRegionInFov.Offset(inspParam.FiducialProbeOffset);
                }
            }

            /*if (fovRegion.Contains(probeRegionInFov.ToRectangle()) == false)
            {
                LogHelper.Debug(LoggerType.Inspection, String.Format("Inspection Skipped. {0}", Target.FullId));
                return new VisionProbeResult(this, "Probe region is out of image");
            }*/

            bool useWholeImage = false;
            RotatedRect clipRegionInFov = new RotatedRect(probeRegionInFov);
            inspAlgorithm.AdjustInspRegion(ref clipRegionInFov, ref useWholeImage);

            LogHelper.Debug(LoggerType.Inspection, String.Format("Insepct Probe - Clip Image : {0}", FullId));
            Size wholeImageSize = new Size(grabImage.Width, grabImage.Height);
            ImageD clipImage = null;
            if (false)
            {
                clipRegionInFov = new RotatedRect(0, 0, grabImage.Width, grabImage.Height, 0);
                clipImage = grabImage;
            }
            else
            {
                if (clipRegionInFov.Width == 0 || clipRegionInFov.Height == 0)
                    clipRegionInFov = Target.AlignedRegion;

                if (fovRegion.Contains(clipRegionInFov.ToRectangle()) == false)
                {
                    LogHelper.Debug(LoggerType.Inspection, String.Format("Inspection Skipped. {0}", Target.FullId));
                    return new VisionProbeResult(this, "Probe region is out of image");
                }

                if (grabImage is Image2D)
                    clipImage = grabImage.ClipImage(clipRegionInFov);
            }

            if (inspParam.SaveProbeImage)
                DebugHelper.SaveImage(clipImage, "ClipImage.bmp", debugContext);

            LogHelper.Debug(LoggerType.Inspection, String.Format("Insepct Probe - Algorithm Inspect : {0}", FullId));

            AlgorithmInspectParam algorithmInspectParam = new AlgorithmInspectParam(clipImage, probeRegionInFov, clipRegionInFov, wholeImageSize, inspParam.CameraCalibration, debugContext);
            algorithmInspectParam.PixelRes3D = inspParam.PixelRes3d;
            algorithmInspectParam.TeachMode = inspParam.TeachMode;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            AlgorithmResult algorithmResult = inspAlgorithm.Inspect(algorithmInspectParam);
            sw.Stop();

            if (algorithmResult != null)
            {
                algorithmResult.ResultFigureGroup.AddFigure((Figure)maskFigures.Clone());
                inspAlgorithm.BuildMessage(algorithmResult);
            }

            LogHelper.Debug(LoggerType.Inspection, String.Format("VisionProbe Inspect Time : {0} ms", sw.ElapsedMilliseconds));

            if (inspParam.SaveProbeImage)
            {
                InspectionResult inspectionResult = inspParam.InspectionResult;
                if (String.IsNullOrEmpty(inspectionResult?.ResultPath) != true)
                {
                    if (inspParam.ClipExtendSize > 0)
                    {
                        clipRegionInFov.Inflate(inspParam.ClipExtendSize, inspParam.ClipExtendSize);

                        if (fovRegion.Contains(clipRegionInFov.ToRectangle()) == false)
                        {
                            RectangleF intersectRect = RectangleF.Intersect(fovRegion, clipRegionInFov.ToRectangleF());
                            clipRegionInFov = new RotatedRect(intersectRect, clipRegionInFov.Angle);
                        }

                        clipImage = grabImage.ClipImage(clipRegionInFov);
                    }

                    string fileName = String.Format("{0}\\{1}_{2}.jpg", inspectionResult.ResultPath, FullId, (algorithmResult.Good == true ? "G" : "N"));

                    if (File.Exists(fileName) == true)
                        File.Delete(fileName);

                    clipImage.SaveImage(fileName, ImageFormat.Jpeg);
                }
            }

            VisionProbeResult visionProbeResult = new VisionProbeResult(this, algorithmResult, clipImage);
            visionProbeResult.RegionInFov = probeRegionInFov;
            visionProbeResult.RegionInProbe = new RotatedRect(probeRegionInFov.X - clipRegionInFov.X, probeRegionInFov.Y - clipRegionInFov.Y, probeRegionInFov.Width, probeRegionInFov.Height, 0);


            return visionProbeResult;
        }

        public override ProbeResult CreateDefaultResult()
        {
            return new VisionProbeResult(this, null, null);
        }

        public override void OnPreInspection()
        {

        }

        public override void OnPostInspection()
        {

        }

        public override void AppendFigures(FigureGroup figureGroup, Pen pen)
        {
            if (alignedMaskFigures.FigureExist == true)
            {
                foreach (Figure maskFigure in alignedMaskFigures)
                {
                    maskFigure.Selectable = false;
                    figureGroup.AddFigure(maskFigure);
                }
            }

            base.AppendFigures(figureGroup, pen);
        }

        public override void AlignFigures(PositionAligner positionAligner)
        {
            if (maskFigures.FigureExist == true)
            {
                alignedMaskFigures = new FigureGroup();

                foreach (Figure maskFigure in maskFigures)
                {
                    maskFigure.Selectable = false;
                    Figure alignedMaskFigure = (Figure)maskFigure.Clone();

                    PointF curPos = DrawingHelper.CenterPoint(alignedMaskFigure.GetRectangle());
                    PointF alignedPos = positionAligner.AlignFov(curPos);

                    alignedMaskFigure.Offset(alignedPos.X - curPos.X, alignedPos.Y - curPos.Y);
                    alignedMaskFigures.AddFigure(alignedMaskFigure);
                }
            }
        }
    }
}
