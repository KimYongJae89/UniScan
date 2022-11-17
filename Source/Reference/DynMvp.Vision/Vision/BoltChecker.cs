using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.IO;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Cvb;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision.UI;

namespace DynMvp.Vision
{
    public class BoltCheckerParam : AlgorithmParam
    {
        private int minSilkArea = 100;
        public int MinSilkArea
        {
            get { return minSilkArea; }
            set { minSilkArea = value; }
        }

        private int thresholdBolt = 120;
        public int ThresholdBolt
        {
            get { return thresholdBolt; }
            set { thresholdBolt = value; }
        }

        private int thresholdSilk = 240;
        public int ThresholdSilk
        {
            get { return thresholdSilk; }
            set { thresholdSilk = value; }
        }

        private int maxHoleCount = 2000;
        public int MaxHoleCount
        {
            get { return maxHoleCount; }
            set { maxHoleCount = value; }
        }

        public override AlgorithmParam Clone()
        {
            BoltCheckerParam param = new BoltCheckerParam();

            param.CopyFrom(this);

            return param;
        }

        public override void CopyFrom(AlgorithmParam srcAlgorithmParam)
        {
            base.CopyFrom(srcAlgorithmParam);

            BoltCheckerParam param = (BoltCheckerParam)srcAlgorithmParam;

            minSilkArea = param.minSilkArea;
            thresholdBolt = param.thresholdBolt;
            thresholdSilk = param.thresholdSilk;
            maxHoleCount = param.maxHoleCount;
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            minSilkArea = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MinSilkArea", "100"));
            thresholdBolt = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "ThresholdBolt", "120"));
            thresholdSilk = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "ThresholdSilk", "240"));
            maxHoleCount = Convert.ToInt32(XmlHelper.GetValue(algorithmElement, "MaxHoleCount", "2000"));
        }

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            XmlHelper.SetValue(algorithmElement, "MinSilkArea", minSilkArea.ToString());
            XmlHelper.SetValue(algorithmElement, "ThresholdBolt", thresholdBolt.ToString());
            XmlHelper.SetValue(algorithmElement, "ThresholdSilk", thresholdSilk.ToString());
            XmlHelper.SetValue(algorithmElement, "MaxHoleCount", maxHoleCount.ToString());
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class BoltChecker : Algorithm
    {
        public BoltChecker()
        {
            param = new BoltCheckerParam();
        }

        public override Algorithm Clone()
        {
            BoltChecker boltChecker = new BoltChecker();
            boltChecker.CopyFrom(this);

            return boltChecker;
        }

        public override void CopyFrom(Algorithm algorithm)
        {
            base.CopyFrom(algorithm);

            BoltChecker boltChecker = (BoltChecker)algorithm;
            param = (BoltCheckerParam)boltChecker.Param.Clone();
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            BoltCheckerParam boltCheckerParam = (BoltCheckerParam)param;

            List<AlgorithmResultValue> resultValues = new List<AlgorithmResultValue>();
            resultValues.Add(new AlgorithmResultValue("Hole Count", boltCheckerParam.MaxHoleCount, 0, 0));

            return resultValues;
        }

        public static string TypeName
        {
            get { return "BoltChecker"; }
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return "Bolt";
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, Base.RotatedRect region)
        {

        }

        public override void AdjustInspRegion(ref Base.RotatedRect inspRegion, ref bool useWholeImage)
        {

        }

        public override string[] GetPreviewNames()
        {
            return new string[] { "Silk", "Bolt" };
        }

        public override ImageD Filter(ImageD image, int previewFilterType)
        {
            AlgoImage algoImage = ImageBuilder.Build(GetAlgorithmType(), image, ImageType.Grey, param.ImageBand);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            BoltCheckerParam boltCheckerParam = (BoltCheckerParam)param;

            switch (previewFilterType)
            {
                case 0:
                    imageProcessing.Binarize(algoImage, BinarizationType.SingleThreshold, boltCheckerParam.ThresholdSilk, 0);
                    break;
                case 1:
                    imageProcessing.Binarize(algoImage, BinarizationType.SingleThreshold, boltCheckerParam.ThresholdBolt, 0);
                    break;
            }

            return algoImage.ToImageD();
        }

        private AlgoImage GetConvexHullMask(AlgoImage binaryImage, out RectangleF resultRect, out int maskArea)
        {
            OpenCv.OpenCvGreyImage openCvGreyImage = binaryImage as OpenCv.OpenCvGreyImage;

            CvBlobs blobs = new CvBlobs();
            CvBlobDetector blobDetector = new CvBlobDetector();
            blobDetector.Detect(openCvGreyImage.Image, blobs);

            BoltCheckerParam boltCheckerParam = (BoltCheckerParam)param;

            blobs.FilterByArea(boltCheckerParam.MinSilkArea, Int32.MaxValue);

            List<CvBlob> silkList = blobs.Values.ToList();
            CvBlob silkBlob = silkList.First();

            foreach(CvBlob blob in silkList)
            {
                if (blob.BoundingBox.Width > silkBlob.BoundingBox.Width)
                    silkBlob = blob;
            }
            
            resultRect = new RectangleF(silkBlob.BoundingBox.Location, silkBlob.BoundingBox.Size);

            //MemStorage memStorage = new MemStorage();
            //Seq<Point> silkConvexHull = silkBlob.GetContour(memStorage).GetConvexHull(Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);

            AlgoImage maskImage = binaryImage.Clone();
            OpenCv.OpenCvGreyImage openCvMaskImage = maskImage as OpenCv.OpenCvGreyImage;
            openCvMaskImage.Image.SetZero();

            int width = openCvMaskImage.Image.Width;
            int height = openCvMaskImage.Image.Height;
            int pitch = 4 * ((int)(Math.Truncate(((float)width - (float)1) / (float)4)) + 1);

            maskArea = 0;

            for (int yPos = 0; yPos < height; yPos++)
            {
                for(int xPos = 0; xPos < pitch; xPos++)
                {
                    Point position = new Point(xPos, yPos);
                    
                    //if(silkConvexHull.InContour(position) > 0)
                    {
                        openCvMaskImage.Image.Data[yPos, xPos, 0] = byte.MaxValue;
                        maskArea++;
                    }
                }
            }

            return maskImage;
        }

        private int GetHoleCount(AlgoImage holeImage)
        {
            OpenCv.OpenCvGreyImage openCvHoleImage = (OpenCv.OpenCvGreyImage)holeImage;

            CvBlobs blobs = new CvBlobs();
            CvBlobDetector blobDetector = new CvBlobDetector();
            blobDetector.Detect(openCvHoleImage.Image, blobs);

            List<CvBlob> holeList = blobs.Values.ToList();

            int sumHoleCount = 0;

            foreach (CvBlob hole in holeList)
                sumHoleCount += hole.Area;

            return sumHoleCount;
        }
        

        public override AlgorithmResult Inspect(AlgorithmInspectParam inspectParam)
        {
            AlgorithmResult boltCheckerResult = CreateAlgorithmResult();

            BoltCheckerParam boltCheckerParam = (BoltCheckerParam)param;

            AlgoImage clipImage = ImageBuilder.Build(GetAlgorithmType(), inspectParam.ClipImage, ImageType.Grey, param.ImageBand);
            Filter(clipImage);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(clipImage);
            AlgoImage silkImage = clipImage.Clone();
            imageProcessing.Binarize(clipImage, silkImage, boltCheckerParam.ThresholdSilk);

            if (inspectParam.DebugContext.SaveDebugImage)
            {
                clipImage.Save("Source.bmp", inspectParam.DebugContext);
                silkImage.Save("Silk.bmp", inspectParam.DebugContext);
            }

            RectangleF resultRect;
            int maskArea;
            
            AlgoImage maskImage = GetConvexHullMask(silkImage, out resultRect, out maskArea);

            AlgoImage boltImage = clipImage.Clone();
            imageProcessing.Binarize(clipImage, boltImage, boltCheckerParam.ThresholdBolt);

            if (inspectParam.DebugContext.SaveDebugImage)
            {
                boltImage.Save("Bolt.bmp", inspectParam.DebugContext);
                maskImage.Save("Mask.bmp", inspectParam.DebugContext);
            }
            OpenCv.OpenCvGreyImage openCvBoltImage = (OpenCv.OpenCvGreyImage)boltImage;
            openCvBoltImage.Image = openCvBoltImage.Image.Not();

            OpenCv.OpenCvGreyImage openCvMaskImage = (OpenCv.OpenCvGreyImage)maskImage;
            
            AlgoImage holeImage = clipImage.Clone();
            OpenCv.OpenCvGreyImage openCvHoleImage = (OpenCv.OpenCvGreyImage)holeImage;

            openCvHoleImage.Image = openCvBoltImage.Image.And(openCvMaskImage.Image);

            if (inspectParam.DebugContext.SaveDebugImage)
                holeImage.Save("Hole.bmp", inspectParam.DebugContext);
            
            int holeCount = GetHoleCount(holeImage);

            resultRect.Offset(inspectParam.ProbeRegionInFov.X, inspectParam.ProbeRegionInFov.Y);
            boltCheckerResult.ResultRect = new Base.RotatedRect(resultRect, 0);

            if (holeCount < boltCheckerParam.MaxHoleCount)
                boltCheckerResult.Good = true;

            boltCheckerResult.ResultValueList.Add(new AlgorithmResultValue("Hole Count", boltCheckerParam.MaxHoleCount, 0, holeCount));

            return boltCheckerResult;
        }
    }
}
