using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;

namespace DynMvp.Vision
{
    public class CircleCheckerParam : AlgorithmParam
    {
        CircleDetectorParam circleDetectorParam = new CircleDetectorParam();
        public CircleDetectorParam CircleDetectorParam
        {
            get { return circleDetectorParam; }
            set { circleDetectorParam = value; }
        }

        bool useImageCenter;
        public bool UseImageCenter
        {
            get { return useImageCenter; }
            set { useImageCenter = value; }
        }

        bool showOffset;
        public bool ShowOffset
        {
            get { return showOffset; }
            set { showOffset = value; }
        }

        public CircleCheckerParam()
        {

        }

        public override AlgorithmParam Clone()
        {
            CircleCheckerParam param = new CircleCheckerParam();
            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            CircleCheckerParam param = (CircleCheckerParam)srcAlgorithmParam;

            useImageCenter = param.useImageCenter;
            showOffset = param.useImageCenter;
            circleDetectorParam.Copy(param.CircleDetectorParam);
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            circleDetectorParam.LoadParam(algorithmElement);

            useImageCenter = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "UseImageCenter", "false"));
            showOffset = Convert.ToBoolean(XmlHelper.GetValue(algorithmElement, "ShowOffset", "false"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            circleDetectorParam.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "UseImageCenter", useImageCenter.ToString());
            XmlHelper.SetValue(algorithmElement, "ShowOffset", showOffset.ToString());
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class CircleChecker : Algorithm
    {
        public CircleChecker()
        {
            param = new CircleCheckerParam();
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            PointF centerPt = DrawingHelper.CenterPoint(region);

            CircleDetectorParam circleDetectorParam = ((CircleCheckerParam)param).CircleDetectorParam;

            Pen innerPen = new Pen(Color.Tomato, 3.0F);
            Pen outterPen = new Pen(Color.BlueViolet, 3.0F);
            innerPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            outterPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            RectangleF innerCircleRect = new RectangleF(centerPt.X - circleDetectorParam.InnerRadius, centerPt.Y - circleDetectorParam.InnerRadius, circleDetectorParam.InnerRadius * 2, circleDetectorParam.InnerRadius * 2);
            figureGroup.AddFigure(new EllipseFigure(innerCircleRect, innerPen));

            RectangleF outterCircleRect = new RectangleF(centerPt.X - circleDetectorParam.OutterRadius, centerPt.Y - circleDetectorParam.OutterRadius, circleDetectorParam.OutterRadius * 2, circleDetectorParam.OutterRadius * 2);
            figureGroup.AddFigure(new EllipseFigure(outterCircleRect, outterPen));
        }

        public override Algorithm Clone()
        {
            CircleChecker circleChecker = new CircleChecker();
            circleChecker.CopyFrom(this);

            return circleChecker;
        }

        public static string TypeName
        {
            get { return "CircleChecker"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Circle";
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
//            PointF centerPt = DrawingHelper.CenterPoint(inspRegion);
//            inspRegion = new RotatedRect(centerPt.X - param.OutterRadius, centerPt.Y - param.OutterRadius, param.OutterRadius * 2, param.OutterRadius * 2, 0);
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("CenterX", 0));
            resultValues.Add(new AlgorithmResultValue("CenterY", 0));
            resultValues.Add(new AlgorithmResultValue("CenterPos", new Point(0, 0)));
            resultValues.Add(new AlgorithmResultValue("Radius", 0));
            resultValues.Add(new AlgorithmResultValue("Offset X", 0, 0, 0));
            resultValues.Add(new AlgorithmResultValue("Offset Y", 0, 0, 0));
            resultValues.Add(new AlgorithmResultValue("Real Offset X", 0, 0, 0));
            resultValues.Add(new AlgorithmResultValue("Real Offset Y", 0, 0, 0));
            resultValues.Add(new AlgorithmResultValue("CenterPos", new Point(0, 0)));

            return resultValues;
        }

        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new AlgorithmResult("CircleCheckerResult");
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.ClipImage, ImageType.Grey, param.ImageBand);
            Filter(clipImage);

            RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            RotatedRect clipRegionInFov = inspectParam.ClipRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;
            Calibration cameraCalibration = inspectParam.CameraCalibration;
            Size whileImageSize = inspectParam.WholeImageSize;

            AlgorithmResult circleCheckerResult = CreateAlgorithmResult();
            circleCheckerResult.ResultRect = probeRegionInFov;

            CircleDetector circleDetector = AlgorithmBuilder.CreateCircleDetector();
            if (circleDetector == null)
                return circleCheckerResult;

            CircleCheckerParam circleCheckerParam = (CircleCheckerParam)param;
            CircleDetectorParam circleDetectorParam = ((CircleCheckerParam)param).CircleDetectorParam;

            circleDetector.Param = circleDetectorParam;
            circleDetector.Param.CenterPosition = new PointF(clipImage.Width/2, clipImage.Height/2);

            CircleEq circleFound = circleDetector.Detect(clipImage, debugContext);
            if (circleFound != null && circleFound.IsValid())
            {
                circleCheckerResult.Good = true;

                circleFound.Center = new PointF(circleFound.Center.X + clipRegionInFov.X, circleFound.Center.Y + clipRegionInFov.Y);

                circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Center X", 0, 0, circleFound.Center.X));
                circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Center Y", 0, 0, circleFound.Center.Y));
                circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("CenterPos", circleFound.Center));
                circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Radius", 0, 0, circleFound.Radius));
                circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("CenterPos", circleFound));

                PointF centerPt = DrawingHelper.CenterPoint(probeRegionInFov);
                if (circleCheckerParam.UseImageCenter)
                {
                    centerPt = new PointF(whileImageSize.Width / 2, whileImageSize.Height / 2);
                }

                PointF offset = PointF.Subtract(circleFound.Center, new SizeF(centerPt));
                offset.Y = -offset.Y;

                circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Offset X", 0, 0, offset.X));
                circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Offset Y", 0, 0, offset.Y));

                if (cameraCalibration != null)
                {
                    PointF circleReal = cameraCalibration.PixelToWorld(centerPt);
                    PointF circleFoundReal = cameraCalibration.PixelToWorld(circleFound.Center);

                    offset = PointF.Subtract(circleFoundReal, new SizeF(circleReal));
                    offset.Y = -offset.Y;

                    circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Real Offset X", 0, 0, offset.X));
                    circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Real Offset Y", 0, 0, offset.Y));
                }
                else
                {
                    circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Real Offset X", 0, 0, 0));
                    circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Real Offset Y", 0, 0, 0));
                }

                SubResult subResult = new SubResult("CircleCheckerSubResult");
                subResult.Good = true;

                RectangleF defectRect = new RectangleF(circleFound.Center.X - circleFound.Radius, circleFound.Center.Y - circleFound.Radius, circleFound.Radius*2, circleFound.Radius*2);
                subResult.ResultFigureGroup.AddFigure(new EllipseFigure(defectRect, new Pen(Color.Green, 4)));

                if (circleCheckerParam.ShowOffset)
                {
                    subResult.ResultFigureGroup.AddFigure(new LineFigure(circleFound.Center, centerPt, new Pen(Color.OrangeRed, 4)));
                    subResult.ResultFigureGroup.AddFigure(new CrossFigure(circleFound.Center, 6, new Pen(Color.OrangeRed)));
                    subResult.ResultFigureGroup.AddFigure(new TextFigure(String.Format("({0:0.000} ,{1:0.000})", offset.X, offset.Y), Point.Truncate(circleFound.Center), new Font(FontFamily.GenericSansSerif, 14, FontStyle.Bold), Color.Green));
                }

                circleCheckerResult.SubResultList.Add(subResult);
            }
            else
            {
                circleCheckerResult.Good = false;

                circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("CenterX", 0));
                circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("CenterY", 0));
                circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Radius", 0));

                circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Offset X", 0));
                circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Offset Y", 0));
                circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Real Offset X", 0));
                circleCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Real Offset Y", 0));
            }

            return circleCheckerResult;
        }
    }
}
