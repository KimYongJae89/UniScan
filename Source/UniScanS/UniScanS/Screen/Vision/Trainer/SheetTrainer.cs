using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Xml;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanS.Screen.Data;
using UniScanS.Screen.Vision.Detector;
using UniScanS.Screen.Vision.Detector.Shape;
using UniScanS.Screen.Vision.FiducialFinder;
using UniScanS.Vision;
using UniScanS.Vision.FiducialFinder;

namespace UniScanS.Screen.Vision.Trainer
{
    public class SheetTrainer : Algorithm
    {
        public static string TypeName
        {
            get { return "SheetS_Trainer"; }
        }

        public SheetTrainer()
        {
            param = new SheetTrainerParam();
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {

        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {

        }

        public override void CopyFrom(DynMvp.Vision.Algorithm algorithm)
        {
            base.CopyFrom(algorithm);

            SheetTrainer srcAlgorithm = (SheetTrainer)algorithm;
            this.param.CopyFrom(srcAlgorithm.param);
        }

        public override DynMvp.Vision.Algorithm Clone()
        {
            SheetTrainer clone = new SheetTrainer();
            clone.CopyFrom(this);

            return clone;
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return TypeName;
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            throw new System.NotImplementedException();
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            throw new NotImplementedException();
        }
        
        internal void Teach(BackgroundWorker worker, Image2D currentImage, DoWorkEventArgs args)
        {
            // 0. Image check
            if (currentImage == null)
            {
                args.Result = "No image";
                return;
            }

            Image2D teachImage = (Image2D)currentImage;
            SheetTrainerParam param = (SheetTrainerParam)this.Param;
            SheetInspectorParam sheetInspectorParam = (SheetInspectorParam)AlgorithmPool.Instance().GetAlgorithm(SheetInspector.TypeName).Param;

            worker.ReportProgress(10, "1. Teach start");

            AlgoImage sourceImage = ImageBuilder.Build(TypeName, teachImage, ImageType.Grey, ImageBandType.Luminance);

            int width = sourceImage.Width;
            int height = sourceImage.Height;

            // 1. Search for empty areas
            worker.ReportProgress(15, "2. Search for empty areas");
            sheetInspectorParam.EmptyRegionPos = AlgorithmCommon.Instance().GetEmptyRegionPos(sourceImage);
            if (sheetInspectorParam.EmptyRegionPos < teachImage.Height * 0.5)
            {
                sourceImage.Dispose();
                args.Result = "Light off";
                return;
            }

            // 2.Buffer allocation
            worker.ReportProgress(20, "3. Buffer allocation");

            ProcessBufferSetS bufferSet = new ProcessBufferSetS(TypeName, width, sheetInspectorParam.EmptyRegionPos);
            AlgoImage interest = sourceImage.GetSubImage(new Rectangle(0, 0, width, sheetInspectorParam.EmptyRegionPos));
            Rectangle interestRect = new Rectangle(0, 0, interest.Width, interest.Height);

            // 3. 전체 이미지 Triangle 값 계산
            worker.ReportProgress(25, "4. Calculate Value");
            ImageProcessing imageProcessing = ImageProcessingFactory.CreateImageProcessing(ImagingLibrary.MatroxMIL);

            int meanValue = 0;

            if (param.IsThinFilm == false)
                meanValue = (int)Math.Round(imageProcessing.Li(interest));
            else
                meanValue = (int)Math.Round(imageProcessing.Triangle(interest));

            imageProcessing.Binarize(interest, bufferSet.InterestBin, meanValue, true);

            // 4. Find Pattern
            worker.ReportProgress(30, "5. Find pattern");
            AlgorithmCommon.Instance().CreateMaskImage(bufferSet.InterestBin, bufferSet.Mask, sheetInspectorParam.ShapeParam.MinPatternArea, true);

            List<SheetPatternGroup> patternGroupList = PatternTrain(bufferSet.Mask);
            SheetPatternGroup refPatternGroup = patternGroupList.First();

            SheetInspectorParam sheetParam = (SheetInspectorParam)AlgorithmPool.Instance().GetAlgorithm(SheetInspector.TypeName).Param;

            if (InspectorSetting.Instance().IsFiducial == true)
            {
                // 5. Fiducial Find
                worker.ReportProgress(40, "6. Create fid pattern");
                List<BlobRect> fidPatternList = FiducialPatternTrain(bufferSet.InterestBin, patternGroupList);

                FiducialFinderSParam fidParam = (FiducialFinderSParam)AlgorithmPool.Instance().GetAlgorithm(UniScanS.Vision.FiducialFinder.FiducialFinder.TypeName).Param;
                
                foreach (FiducialPattern fidPattern in fidParam.FidPatternList)
                    fidPattern.Dispose();

                fidParam.FidPatternList.Clear();
                foreach (BlobRect blobRect in fidPatternList)
                {
                    Rectangle patternRegion = Rectangle.Truncate(blobRect.BoundingRect);
                    patternRegion.Inflate(10, 10);
                    patternRegion.Intersect(interestRect);
                    AlgoImage patternImage = interest.Clip(patternRegion);
                    Rectangle fidRegion = Rectangle.Truncate(blobRect.BoundingRect);
                    fidParam.FidPatternList.Add(new FiducialPattern((Image2D)patternImage.ToImageD(), fidRegion, blobRect.CenterPt));
                    patternImage.Dispose();
                }

                fidParam.Train();
            }
            //patternGroupList.RemoveAll(pattern => fidPatternGroupList.Find(fidPattern => fidPattern == pattern) != null);
            
            // 5. ROI 생성
            worker.ReportProgress(50, "7. Find interest region");
            List<BlobRect> refRegionBlobRect = new List<BlobRect>();
            refPatternGroup.PatternList.ForEach(blob => refRegionBlobRect.Add(blob.Clone()));
            AlgorithmCommon.Instance().MergeBlobs(param.PatternMaxGap, ref refRegionBlobRect);

            //Mask 안쓰고 Bin 쓰는건 좀 모호하나... 이게 나을듯 합니다.
            imageProcessing.Not(bufferSet.Mask, bufferSet.DielectricMask);

            imageProcessing.Erode(bufferSet.Mask, bufferSet.PoleMask, InspectorSetting.Instance().RemovalNum);
            imageProcessing.Erode(bufferSet.DielectricMask, InspectorSetting.Instance().RemovalNum);

            // 6. ROI 별 값 계산
            worker.ReportProgress(70, "8. Calculate region value");
            sheetParam.RegionInfoList.Clear();
            foreach (BlobRect blobRect in refRegionBlobRect)
            {
                Rectangle regionRect = Rectangle.Truncate(blobRect.BoundingRect);
                regionRect.Inflate(5, 5);
                regionRect.Intersect(interestRect);

                if (regionRect.Width == 0 || regionRect.Height == 0)
                    continue;

                RegionInfo regionInfo = new RegionInfo();
                regionInfo.Region = regionRect;

                AlgoImage regionSubImage = interest.GetSubImage(regionInfo.Region);

                if (param.IsThinFilm == false)
                    regionInfo.MeanValue = (int)Math.Round(imageProcessing.Li(regionSubImage));
                else
                    regionInfo.MeanValue = (int)Math.Round(imageProcessing.Triangle(regionSubImage));
                
                AlgoImage poleSubImage = bufferSet.PoleMask.GetSubImage(regionInfo.Region);
                regionInfo.PoleValue = (int)Math.Round(imageProcessing.GetGreyAverage(regionSubImage, poleSubImage));

                AlgoImage dielectricSubImage = bufferSet.DielectricMask.GetSubImage(regionInfo.Region);
                regionInfo.DielectricValue = (int)Math.Round(imageProcessing.GetGreyAverage(regionSubImage, dielectricSubImage));

                sheetParam.RegionInfoList.Add(regionInfo);

                regionSubImage.Dispose();
                poleSubImage.Dispose();
                dielectricSubImage.Dispose();
            }

            //foreach (RegionInfo regionInfo in sheetParam.RegionInfoList)
            //{
            //    //if (sheetParam.RegionInfoList.Find(r => r.Region.Top > regionInfo.Region.Bottom) == null)
            //    //{
            //    //    if (sheetParam.RegionInfoList.Find(r => r.Region.Bottom < regionInfo.Region.Top) != null)
            //    //    {
            //    //        regionInfo.Region = new Rectangle(regionInfo.Region.X, regionInfo.Region.Y, regionInfo.Region.Width, interest.Height - regionInfo.Region.Y);
            //    //    }
            //    //}

            //    if (sheetParam.RegionInfoList.Find(r => r.Region.Bottom < regionInfo.Region.Top) == null)
            //    {
            //        if (sheetParam.RegionInfoList.Find(r => r.Region.Top > regionInfo.Region.Bottom) != null)
            //        {
            //            regionInfo.Region = new Rectangle(regionInfo.Region.X, 0, regionInfo.Region.Width, regionInfo.Region.Height + regionInfo.Region.Y);
            //        }
            //    }
            //}

            List<SheetPatternGroup> regionPatternGroupList = new List<SheetPatternGroup>();
            foreach (SheetPatternGroup patternGroup in patternGroupList)
            {
                foreach (RegionInfo regionInofo in sheetParam.RegionInfoList)
                {
                    if (patternGroup.IsContain(regionInofo.Region) == true)
                    {
                        regionPatternGroupList.Add(patternGroup);
                        break;
                    }
                }
            }
                

            // 6. Create sheet pattern
            worker.ReportProgress(80, "9. Create refference pattern");
            sheetParam.ShapeParam.PatternList.Clear();
            foreach (SheetPatternGroup patternGroup in regionPatternGroupList)
            {
                Rectangle patternRegion = Rectangle.Truncate(patternGroup.GetAverageBlobRect().BoundingRect);
                patternRegion.Inflate(5, 5);
                patternRegion.Intersect(interestRect);
                AlgoImage patternImage = interest.Clip(patternRegion);
                sheetParam.ShapeParam.PatternList.Add(new SheetPattern(patternImage.ToImageD().ToBitmap(), patternGroup));
                patternImage.Dispose();
            }

            if (sheetParam.ShapeParam.PatternList.Count > 0)
                sheetParam.ShapeParam.PatternList.First().NeedInspect = true;

            worker.ReportProgress(90, "10. Calculate recommend value");
            GetRecommendValues(bufferSet, interest);

            interest.Dispose();
            sourceImage.Dispose();

            bufferSet.Dispose();
            
            return;
        }

        public void GetRecommendValues(ProcessBufferSetS bufferSet, AlgoImage interest)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(interest);

            SheetTrainerParam param = (SheetTrainerParam)this.Param;
            SheetInspectorParam sheetParam = (SheetInspectorParam)AlgorithmPool.Instance().GetAlgorithm(SheetInspector.TypeName).Param;
            
            List<float> poleLowerGapList = new List<float>();
            List<float> poleUpperGapList = new List<float>();

            List<float> dielectricLowerGapList = new List<float>();
            List<float> dielectricUpperGapList = new List<float>();

            foreach (RegionInfo regionInfo in sheetParam.RegionInfoList)
            {
                int gridWidth = regionInfo.Region.Width / InspectorSetting.Instance().GridColNum;
                int gridheight = regionInfo.Region.Height / InspectorSetting.Instance().GridRowNum;

                if (gridWidth == 0 || gridheight == 0)
                    continue;

                for (int col = 0; col < InspectorSetting.Instance().GridColNum; col++)
                {
                    int gridX = regionInfo.Region.X + gridWidth * col;
                    for (int row = 0; row < InspectorSetting.Instance().GridRowNum; row++)
                    {
                        int gridY = regionInfo.Region.Y + gridheight * row;
                        Rectangle gridRegion = new Rectangle(gridX, gridY, gridWidth, gridheight);

                        AlgoImage interestGrid = interest.GetSubImage(gridRegion);
                        AlgoImage poleGrid = bufferSet.PoleMask.GetSubImage(gridRegion);
                        AlgoImage dielectricGrid = bufferSet.DielectricMask.GetSubImage(gridRegion);

                        //전극
                        float poleMinValue = imageProcessing.GetGreyMin(interestGrid, poleGrid);
                        float poleMaxValue = imageProcessing.GetGreyMax(interestGrid, poleGrid);

                        poleLowerGapList.Add(regionInfo.PoleValue - poleMinValue);
                        poleUpperGapList.Add(poleMaxValue - regionInfo.PoleValue);

                        //성형
                        float dielectricMinValue = imageProcessing.GetGreyMin(interestGrid, dielectricGrid);
                        float dielectricMaxValue = imageProcessing.GetGreyMax(interestGrid, dielectricGrid);

                        dielectricLowerGapList.Add(regionInfo.DielectricValue- dielectricMinValue);
                        dielectricUpperGapList.Add(dielectricMaxValue - regionInfo.DielectricValue);

                        interestGrid.Dispose();
                        poleGrid.Dispose();
                        dielectricGrid.Dispose();
                    }
                }
            }

            poleLowerGapList.Sort();
            poleUpperGapList.Sort();

            if (poleLowerGapList.Count > 0)
                param.PoleRecommendLowerTh = Math.Max(0, (int)(Math.Round(poleLowerGapList[(int)(poleLowerGapList.Count * 0.5)] * 1.1) * (AlgorithmSetting.Instance().PoleLowerWeight / 100.0f)));

            if (poleUpperGapList.Count > 0)
                param.PoleRecommendUpperTh = Math.Max(0, (int)(Math.Round(poleUpperGapList[(int)(poleUpperGapList.Count * 0.5)] * 1.1) * (AlgorithmSetting.Instance().PoleUpperWeight / 100.0f)));

            dielectricLowerGapList.Sort();
            dielectricUpperGapList.Sort(); 
            
            if (dielectricLowerGapList.Count > 0)
                param.DielectricRecommendLowerTh = Math.Max(0, (int)(Math.Round(dielectricLowerGapList[(int)(dielectricLowerGapList.Count * 0.5)] * 1.1) * (AlgorithmSetting.Instance().DielectricLowerWeight / 100.0f)));

            if (dielectricUpperGapList.Count > 0)
                param.DielectricRecommendUpperTh = Math.Max(0, (int)(Math.Round(dielectricUpperGapList[(int)(dielectricUpperGapList.Count * 0.75)] * 1.1) * (AlgorithmSetting.Instance().DielectricUpperWeight / 100.0f)));
        }
        
        public List<SheetPatternGroup> PatternTrain(AlgoImage maskR)
        {
            SheetInspectorParam sheetInspectorParam = (SheetInspectorParam)AlgorithmPool.Instance().GetAlgorithm(SheetInspector.TypeName).Param;

            SheetTrainerParam param = (SheetTrainerParam)this.Param;

            ImageProcessing imageProcessing = ImageProcessingFactory.CreateImageProcessing(ImagingLibrary.MatroxMIL);

            BlobParam patternBlobParam = new BlobParam();
            patternBlobParam.EraseBorderBlobs = true;
            patternBlobParam.SelectCenterPt = true;
            patternBlobParam.AreaMin = (int)Math.Round((double)sheetInspectorParam.ShapeParam.MinPatternArea);

            BlobRectList patternBlobRectList = null;
            patternBlobRectList = imageProcessing.Blob(maskR, patternBlobParam);

            patternBlobRectList.Dispose();

            List<BlobRect> blobList = patternBlobRectList.GetList();
            GetPatternWaist(maskR, blobList, imageProcessing);

            SheetPatternGroup patternGroup = new SheetPatternGroup();
            patternGroup.AddPattern(blobList);

            return patternGroup.DivideSubGroup(param.GroupThreshold).OrderByDescending(p => p.PatternList.Count * p.AverageArea).ToList();
        }

        private void GetPatternWaist(AlgoImage binImage, List<BlobRect> blobList, ImageProcessing imageProcessing)
        {
            BlobParam blobParam = new BlobParam();
            blobParam.EraseBorderBlobs = false;

            foreach (var blob in blobList)
            {
                Rectangle subRect = new Rectangle((int)blob.BoundingRect.X, (int)blob.CenterPt.Y, (int)blob.BoundingRect.Width, 1);

                AlgoImage subImage = binImage.GetSubImage(subRect);

                BlobRectList waistBlobRectList = imageProcessing.Blob(subImage, blobParam);
                waistBlobRectList.Dispose();
                subImage.Dispose();

                BlobRect waistBlob = waistBlobRectList.GetMaxAreaBlob();
                if (waistBlob != null)
                    blob.Elongation = waistBlob.BoundingRect.Width;
            }
        }

        private Rectangle FindPatternRegion(AlgoImage interestBin)
        {
            int height = interestBin.Height;
            int width = interestBin.Width;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(interestBin);
            float[] pArray = imageProcessing.Projection(interestBin, Direction.Horizontal, ProjectionType.Mean);

            double avgValue = pArray.Average() / 2.0;

            List<Tuple<int, int>> edgeList = new List<Tuple<int, int>>();

            int subtractRegion = 50;

            bool isDielectricSpace = false;

            int startPos = 0;
            int endPos = 0;

            //처음에 255..
            if (pArray[0] < avgValue)
                isDielectricSpace = true;
                
            for (int i = 1; i < pArray.Count(); i++)
            {
                //검은색 -> 흰색
                if (pArray[i - 1] >= avgValue && pArray[i] < avgValue)
                {
                    isDielectricSpace = true;
                    startPos = i;
                }

                //반대
                if (pArray[i - 1] < avgValue && pArray[i] >= avgValue)
                {
                    if (isDielectricSpace == true)
                    {
                        isDielectricSpace = false;
                        endPos = i;
                        edgeList.Add(new Tuple<int, int>(startPos + subtractRegion, endPos - subtractRegion));
                    }
                }
            }

            if (isDielectricSpace == true)
            {
                isDielectricSpace = false;
                endPos = pArray.Count() - 1;
                edgeList.Add(new Tuple<int, int>(startPos + subtractRegion, endPos - subtractRegion));
            }

            if (edgeList.Count == 0)
                return Rectangle.Empty;

            Tuple<int, int> maxEdge = edgeList.OrderByDescending(edge => edge.Item2 - edge.Item1).First( );

            if (maxEdge.Item2 - maxEdge.Item1 <= 0)
                return Rectangle.Empty;

            return new Rectangle(maxEdge.Item1, 0, maxEdge.Item2 - maxEdge.Item1, height);
        }

        List<BlobRect> FiducialPatternTrain(AlgoImage interestBin, List<SheetPatternGroup> patternGroupList)
        {
            List<BlobRect> fidPatternList = new List<BlobRect>();

            Rectangle fidRegion = FindPatternRegion(interestBin);
            if (fidRegion.IsEmpty == true)
                return fidPatternList;

            foreach (SheetPatternGroup patternGroup in patternGroupList)
            {
                foreach (BlobRect blobRect in patternGroup.PatternList)
                {
                    if (fidRegion.Contains(Point.Round(blobRect.CenterPt)))
                        fidPatternList.Add(blobRect);
                }
            }

            fidPatternList.OrderByDescending(fid => fid.Area);

            return fidPatternList;
        }
    }
}
