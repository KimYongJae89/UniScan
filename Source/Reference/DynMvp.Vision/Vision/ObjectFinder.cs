using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.Util;
using Emgu.CV.Features2D;

using DynMvp.Base;
using DynMvp.Vision.OpenCv;
using DynMvp.UI;

namespace DynMvp.Vision
{
    public class ObjectFinderParam : AlgorithmParam
    {
        private int searchRangeWidth;
        public int SearchRangeWidth
        {
            get { return searchRangeWidth; }
            set { searchRangeWidth = value; }
        }

        private int searchRangeHeight;
        public int SearchRangeHeight
        {
            get { return searchRangeHeight; }
            set { searchRangeHeight = value; }
        }

        private List<Image2D> patternList = new List<Image2D>();
        public List<Image2D> PatternList
        {
            get { return patternList; }
            set { patternList = value; }
        }

        public ObjectFinderParam()
        {
            searchRangeWidth = 50;
            searchRangeHeight = 50;
        }

        public override AlgorithmParam Clone()
        {
            ObjectFinderParam param = new ObjectFinderParam();

            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            ObjectFinderParam param = (ObjectFinderParam)srcAlgorithmParam;

            searchRangeWidth = param.searchRangeWidth;
            searchRangeHeight = param.searchRangeHeight;

            foreach (ImageD patternImage in param.PatternList)
                patternList.Add((Image2D)patternImage.Clone());
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            searchRangeWidth = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeWidth", "50"));
            searchRangeHeight = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeHeight", "50"));

            foreach (XmlElement patternElement in algorithmElement)
            {
                if (patternElement.Name == "Pattern")
                {
                    string imageString = XmlHelper.GetValue(patternElement, "Image", "");
                    Bitmap patternImage = ImageHelper.Base64StringToBitmap(imageString);
                    if (patternImage != null)
                    {
                        AddPattern(Image2D.FromBitmap(patternImage));
                        patternImage.Dispose();
                    }
                }
            }
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            searchRangeWidth = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeWidth", "50"));
            searchRangeHeight = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "SearchRangeHeight", "50"));

            foreach (Image2D patternImage in patternList)
            {
                XmlElement patternImageElement = algorithmElement.OwnerDocument.CreateElement("", "Pattern", "");
                algorithmElement.AppendChild(patternImageElement);

                Bitmap bitmap = patternImage.ToBitmap();

                XmlHelper.SetValue(patternImageElement, "Image", ImageHelper.BitmapToBase64String(bitmap));
            }
        }

        public void AddPattern(Image2D patternImage)
        {
            patternList.Add(patternImage);
        }

        public void RemovePattern(Image2D patternImage)
        {
            patternList.Remove(patternImage);
        }

        public void RemoveAllPatterns()
        {
            patternList.Clear();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class ObjectFinder : Algorithm, Searchable
    {
        public ObjectFinder()
        {
            param = new ObjectFinderParam();
        }

        public override Algorithm Clone()
        {
            ObjectFinder objectFinder = new ObjectFinder();
            objectFinder.CopyFrom(this);

            return objectFinder;
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, Base.RotatedRect region)
        {

        }

        public Size GetSearchRangeSize()
        {
            ObjectFinderParam objectFinderParam = (ObjectFinderParam)param;

            return new Size(objectFinderParam.SearchRangeWidth, objectFinderParam.SearchRangeHeight);
        }

        public void SetSearchRangeSize(Size searchRange)
        {
            ObjectFinderParam objectFinderParam = (ObjectFinderParam)param;

            objectFinderParam.SearchRangeWidth = searchRange.Width;
            objectFinderParam.SearchRangeHeight = searchRange.Height;
        }

        public void AddPattern(Image2D patternImage)
        {
            ObjectFinderParam objectFinderParam = (ObjectFinderParam)param;
            objectFinderParam.AddPattern(patternImage);
        }

        public void RemovePattern(Image2D patternImage)
        {
            ObjectFinderParam objectFinderParam = (ObjectFinderParam)param;
            objectFinderParam.RemovePattern(patternImage);
        }

        public void RemoveAllPatterns()
        {
            ObjectFinderParam objectFinderParam = (ObjectFinderParam)param;
            objectFinderParam.RemoveAllPatterns();
        }

        public static string TypeName
        {
            get { return "ObjectFinder"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Object";
        }

        public override void AdjustInspRegion(ref Base.RotatedRect inspRegion, ref bool useWholeImage)
        {
            ObjectFinderParam objectFinderParam = (ObjectFinderParam)param;
            inspRegion.Inflate(objectFinderParam.SearchRangeWidth, objectFinderParam.SearchRangeHeight);
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();

            return resultValues;
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.ClipImage, ImageType.Grey, param.ImageBand);
            Filter(clipImage);

            Base.RotatedRect probeRegionInFov = inspectParam.ProbeRegionInFov;
            Base.RotatedRect clipRegionInFov = inspectParam.ClipRegionInFov;
            DebugContext debugContext = inspectParam.DebugContext;
            Calibration cameraCalibration = inspectParam.CameraCalibration;

            AlgorithmResult pmResult = new AlgorithmResult("ObjectFinder");

            pmResult.Good = false;

            //lock (this)
            //{
            //    HomographyMatrix homography = null;

            //    SURFDetector surfCPU = new SURFDetector(1000, true);
            //    VectorOfKeyPoint modelKeyPoints;
            //    VectorOfKeyPoint observedKeyPoints;
            //    Matrix<int> indices;

            //    Matrix<byte> mask;
            //    int k = 2;
            //    double uniquenessThreshold = 0.8;

            //    OpenCvGreyImage openCvTargetImage = clipImage as OpenCvGreyImage;
            //    ObjectFinderParam objectFinderParam = (ObjectFinderParam)param;

            //    foreach (Image2D patternImage in objectFinderParam.PatternList)
            //    {
            //        OpenCvGreyImage openCvPattern = ImageBuilder.OpenCvImageBuilder.Build(patternImage, ImageType.Grey) as OpenCvGreyImage;

            //        //extract features from the model image
            //        modelKeyPoints = surfCPU.DetectKeyPointsRaw(openCvPattern.Image, null);
            //        Matrix<float> modelDescriptors = surfCPU.ComputeDescriptorsRaw(openCvPattern.Image, null, modelKeyPoints);

            //        // extract features from the observed image
            //        observedKeyPoints = surfCPU.DetectKeyPointsRaw(openCvTargetImage.Image, null);
            //        Matrix<float> observedDescriptors = surfCPU.ComputeDescriptorsRaw(openCvTargetImage.Image, null, observedKeyPoints);
            //        if (observedDescriptors == null)
            //            continue;

            //        BruteForceMatcher<float> matcher = new BruteForceMatcher<float>(DistanceType.L2Sqr);
            //        matcher.Add(modelDescriptors);

            //        indices = new Matrix<int>(observedDescriptors.Rows, k);

            //        // using이 이렇게 사용될 때에는 {}안에 있는 모든 내용이 완료되면 동적으로 생성한 dist객체에 대한 dispose()문이 자동으로 호출된다. 
            //        using (Matrix<float> dist = new Matrix<float>(observedDescriptors.Rows, k))
            //        {
            //            matcher.KnnMatch(observedDescriptors, indices, dist, k, null);
            //            mask = new Matrix<byte>(dist.Rows, 1);
            //            mask.SetValue(255);
            //            Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
            //        }

            //        int nonZeroCount = CvInvoke.cvCountNonZero(mask);
            //        if (nonZeroCount >= 4)
            //        {
            //            nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
            //            if (nonZeroCount >= 4)
            //                homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, 2);
            //        }

            //        if (homography != null)
            //        {
            //            RectangleF findedRect = DrawingHelper.FovToClip(clipRegionInFov, probeRegionInFov);
            //            PointF[] pts = DrawingHelper.GetPoints(findedRect, 0);
            //            homography.ProjectPoints(pts);

            //            RectangleF rectangle1 = RectangleF.FromLTRB(pts[0].X, pts[0].Y, pts[2].X, pts[2].Y);
            //            RectangleF rectangle2 = RectangleF.FromLTRB(pts[3].X, pts[1].Y, pts[1].X, pts[3].Y);
            //            RectangleF findedRectangle = RectangleF.Union(rectangle1, rectangle2);

            //            PointF maxMatchPos = new PointF(findedRectangle.X + findedRectangle.Width / 2, findedRectangle.Y + findedRectangle.Height / 2);
            //            maxMatchPos = DrawingHelper.ClipToFov(clipRegionInFov, maxMatchPos);

            //            MatchPos matchPos = new MatchPos(maxMatchPos, 1);
            //            matchPos.PatternSize = openCvPattern.Image.Size;

            //            PatternResult patternResult = new PatternResult();
            //            patternResult.AddMatchPos(matchPos);

            //            pmResult.AddSubResult(patternResult);

            //            if (pmResult.Good == false)
            //            {
            //                patternResult.Found = true;

            //                PointF probeCenter = DrawingHelper.CenterPoint(probeRegionInFov);
            //                PointF foundPosInFov = DrawingHelper.ClipToFov(clipRegionInFov, matchPos.Pos);

            //                SizeF offset = new SizeF();
            //                offset.Width = Math.Abs(maxMatchPos.X - probeCenter.X);
            //                offset.Height = Math.Abs(maxMatchPos.Y - probeCenter.Y);

            //                pmResult.OffsetFound = offset;

            //                if (cameraCalibration != null)
            //                    pmResult.Calibrated = cameraCalibration.IsCalibrated();

            //                if (pmResult.Calibrated)
            //                {
            //                    PointF realRefPos = cameraCalibration.PixelToWorld(probeCenter);
            //                    PointF realFoundPos = cameraCalibration.PixelToWorld(foundPosInFov);

            //                    pmResult.RealOffsetFound = new SizeF(realFoundPos.X - realRefPos.X, realFoundPos.Y - realRefPos.Y);
            //                }

            //                DynMvp.UI.RotatedRect resultRect = probeRegionInFov;
            //                resultRect.Offset(offset.Width, offset.Height);
            //                pmResult.ResultRect = resultRect;

            //                pmResult.ResultValueList.Add(new AlgorithmResultValue("Matching Pos", matchPos.Pos));

            //                pmResult.Good = true;
            //            }
            //        }
            //    }
            //}

            //return pmResult;
            return null;
        }

        public override void BuildMessage(AlgorithmResult algorithmResult)
        {
            MessageBuilder resultMessage = algorithmResult.MessageBuilder;

            PointF matchingPos = (PointF)algorithmResult.GetResultValue("Matching Pos").Value;

            resultMessage.BeginTable(null, "Item", "Value");

            resultMessage.AddTableRow("Result", (algorithmResult.Good ? "Good" : "NG"));
            resultMessage.AddTableRow("Matching Pos", matchingPos.ToString());
            resultMessage.AddTableRow("Offset", String.Format("({0:0.00}, {1:0.00})", algorithmResult.OffsetFound.Width, algorithmResult.OffsetFound.Height));

            if (algorithmResult.Calibrated)
                resultMessage.AddTableRow("RealOffset", String.Format("({0:0.00}, {1:0.00})", algorithmResult.RealOffsetFound.Width, algorithmResult.RealOffsetFound.Height));

            resultMessage.EndTable();

            resultMessage.BeginTable(null, "No", "Found");

            for (int i = 0; i < algorithmResult.SubResultList.Count; i++)
            {
                PatternResult patternResult = (PatternResult)algorithmResult.SubResultList[i];
                resultMessage.AddTableRow(i.ToString(), (patternResult.Good ? "OK" : ""));
            }

            resultMessage.EndTable();
        }
    }
}
