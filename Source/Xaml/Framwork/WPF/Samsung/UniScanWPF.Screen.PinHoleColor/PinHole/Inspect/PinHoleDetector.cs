using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using UniEye.Base.Settings;
using UniScanWPF.Screen.PinHoleColor.Inspect;
using UniScanWPF.Screen.PinHoleColor.PinHole.Data;
using UniScanWPF.Screen.PinHoleColor.PinHole.Settings;

namespace UniScanWPF.Screen.PinHoleColor.PinHole.Inspect
{
    internal class PinHoleDetector : UniScanWPF.Screen.PinHoleColor.Inspect.Detector
    {
        public override Tuple<int, ConcurrentStack<AlgoImage>> GetBufferStack(ImageDevice targetDevice)
        {
            ConcurrentStack<AlgoImage> stack = new ConcurrentStack<AlgoImage>();

            for (int i = 0; i < PinHoleSettings.Instance().BufferNum; i++)
            {
                stack.Push(ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, new Size(targetDevice.ImageSize.Width, targetDevice.ImageSize.Height)));
                stack.Push(ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, new Size(targetDevice.ImageSize.Width, targetDevice.ImageSize.Height)));
                stack.Push(ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, ImageType.Grey, new Size(targetDevice.ImageSize.Width, targetDevice.ImageSize.Height)));
            }

            return new Tuple<int, ConcurrentStack<AlgoImage>>(3, stack);
        }

        public override DetectorResult Detect(AlgoImage targetImage, AlgoImage[] buffers, DetectorParam detectorParam)
        {
            PinHoleDetectorResult pinHoleDetectorResult = new PinHoleDetectorResult();

            PinHoleDetectorParam param = detectorParam as PinHoleDetectorParam;
            
            int width = targetImage.Width;
            int height = targetImage.Height;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(targetImage);
            
            Rectangle interestRect = new Rectangle();
            
            Rectangle sheetRegionRect = new Rectangle(new Point(0, targetImage.Height / 2 - 100), new Size(targetImage.Width, 200));
            if (sheetRegionRect.Width <= 0 || sheetRegionRect.Height <= 0)
            {
                pinHoleDetectorResult.Judgment = DynMvp.InspData.Judgment.Reject;
                return pinHoleDetectorResult;
            }

            AlgoImage projectionImage = targetImage.GetSubImage(Rectangle.Round(sheetRegionRect));
            double[] edgePosition = EdgePositionFinder.SheetEdgePosition(projectionImage, new double[] { param.EdgeThreshold }, out float[] edgeValueArray, param.SearchDireciton);
            projectionImage.Dispose();

            switch (param.SearchDireciton)
            {
                case SearchDireciton.LeftToRight:
                    interestRect = new Rectangle(
                        (int)edgePosition[0] + (int)(param.SkipLength * 1000.0f / PinHoleSettings.Instance().PixelResolution), 0, 
                        width - (int)edgePosition[0] - (int)(param.SkipLength * 1000.0f / PinHoleSettings.Instance().PixelResolution), height);
                    break;
                case SearchDireciton.RightToLeft:
                    interestRect = new Rectangle(0, 0, width - (int)edgePosition[0] - (int)(param.SkipLength * 1000.0f / PinHoleSettings.Instance().PixelResolution), height);
                    break;
            }

            if (edgePosition[0] == 0 || interestRect.Width <= 0)
            {
                pinHoleDetectorResult.Judgment = DynMvp.InspData.Judgment.Reject;
                return pinHoleDetectorResult;
            }

            pinHoleDetectorResult.EdgeValueList.AddRange(edgeValueArray);

            AlgoImage sheetRegionImage = targetImage.GetSubImage(interestRect);
            float avgValue = imageProcessing.GetGreyAverage(sheetRegionImage);
            sheetRegionImage.Dispose();

            pinHoleDetectorResult.Average = avgValue;

            interestRect.Height = height;
            pinHoleDetectorResult.InterestRegion = interestRect;

            AlgoImage interestImage = targetImage.GetSubImage(Rectangle.Round(interestRect));
            AlgoImage binImage = buffers[0].GetSubImage(Rectangle.Round(interestRect));
            
            BlobParam blobParam = new BlobParam();
            blobParam.MaxCount = 10000;
            blobParam.SelectGrayMeanValue = true;
            blobParam.SelectFeretDiameter = true;

            imageProcessing.Binarize(interestImage, binImage, Math.Max(0, (int)(avgValue - param.LowerThreshold)), Math.Min(255, (int)(avgValue + param.UpperThreshold)), true);
            BlobRectList defectList = imageProcessing.Blob(binImage, blobParam, interestImage);
            binImage.Dispose();
            interestImage.Dispose();
            
            AddDefectBlob(targetImage, defectList.GetList(), pinHoleDetectorResult, avgValue, interestRect.X);

            if (pinHoleDetectorResult.DefectList.Count == 0)
                pinHoleDetectorResult.Judgment = Judgment.Accept;
            
            return pinHoleDetectorResult;
        }

        void AddDefectBlob(AlgoImage algoImage, List<BlobRect> blobRectList, PinHoleDetectorResult pinHoleDetectorResult, float avgValue, int offset)
        {
            Rectangle srcRect = new Rectangle(0, 0, algoImage.Width, algoImage.Height);

            PinHoleSettings settings = PinHoleSettings.Instance();

            List<BlobRect> orderBlobRectList = blobRectList.OrderByDescending(blobRect => blobRect.Area).TakeWhile(blobRect => blobRect.MaxFeretDiameter * settings.PixelResolution >= settings.MinSize).ToList();

            int index = 0;
            foreach (BlobRect blobRect in orderBlobRectList)
            {
                if (pinHoleDetectorResult.DefectList.Count >= settings.MaxDefectNum)
                    break;

                Rectangle rect = Rectangle.Truncate(blobRect.BoundingRect);
                rect.Offset(offset, 0);
                
                Rectangle clipRect = Rectangle.Round(rect);
                clipRect.Inflate(20, 20);
                clipRect.Intersect(srcRect);

                if (clipRect.Width <= 0 || clipRect.Height <= 0)
                    continue;

                PinHoleDefectType defectType = blobRect.MeanValue >= avgValue ? PinHoleDefectType.PinHole : PinHoleDefectType.Dust;
                
                AlgoImage subImage = algoImage.GetSubImage(clipRect);
                BitmapSource defectImage = subImage.ToBitmapSource();
                subImage.Dispose();
                //바뀐코드

                PinHoleDefect defect = new PinHoleDefect(index++, defectType == PinHoleDefectType.Dust ? avgValue - blobRect.MeanValue : blobRect.MeanValue - avgValue, blobRect.MaxFeretDiameter, defectImage, rect, defectType);
                pinHoleDetectorResult.DefectList.Add(defect);
            }
        }
    }
}
