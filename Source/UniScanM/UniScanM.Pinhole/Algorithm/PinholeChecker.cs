using DynMvp.Base;
using DynMvp.InspData;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanM.Algorithm;
using UniScanM.Pinhole.Data;
using UniScanM.Pinhole.Settings;
using System.Drawing.Imaging;

namespace UniScanM.Pinhole.Algorithm
{
    public class PinholeChecker
    {
        EdgePositionFinder edgePositionFinder;
        ObjectPoolQueue<(AlgoImage, DefectInfoList)>[] mergeCheckPool;
        bool topBottomMergeCheck = false;
        public PinholeChecker()
        {
            int poolCount = SystemManager.Instance().DeviceBox.ImageDeviceHandler.Count;
            mergeCheckPool = new ObjectPoolQueue<(AlgoImage, DefectInfoList)>[poolCount];
            for (int i = 0; i < poolCount; ++i)
                mergeCheckPool[i] = new ObjectPoolQueue<(AlgoImage, DefectInfoList)>();
            edgePositionFinder = new EdgePositionFinder();
        }

        private bool GetAverageValue(int deviceIndex, AlgoImage algoImage, AlgoImage maskImage, ref int average)
        {
            int width = algoImage.Width;
            int height = algoImage.Height;

            Data.Model model = (Data.Model)SystemManager.Instance().CurrentModel;

            int searchHeight = 10;

            int searchStartY = (algoImage.Height / 2) - (searchHeight / 2);
            Rectangle srcRect = new Rectangle(0, 0, width, height);
            Rectangle searchRect = new Rectangle(0, searchStartY, width, searchHeight);
            searchRect.Intersect(srcRect);
            if (searchRect.Width == 0 || searchRect.Height == 0)
                return false;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            AlgoImage srcSubImage = algoImage.GetSubImage(searchRect);
            AlgoImage maskSubImage = maskImage.GetSubImage(searchRect);

            average = (int)Math.Round(imageProcessing.GetGreyAverage(srcSubImage, maskSubImage));

            srcSubImage.Dispose();
            maskSubImage.Dispose();

            return true;
        }

        //public bool AutoSet(int deviceIndex, ImageD imageD)
        //{
        //    AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);

        //    int width = algoImage.Width;
        //    int height = algoImage.Height;

        //    Data.Model model = (Data.Model)SystemManager.Instance().CurrentModel;

        //    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

        //    AlgoImage maskImage = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, algoImage.Width, algoImage.Height);
        //    PinholeParamValue paramValue;
        //    if (deviceIndex == 0)
        //        paramValue = model.InspectParam.PinholeParamValue1;
        //    else
        //        paramValue = model.InspectParam.PinholeParamValue2;

        //    paramValue.BinarizeValue = (int)imageProcessing.Binarize(algoImage, maskImage, true);

        //    maskImage.Dispose();
        //    algoImage.Dispose();
        //    return true;
        //}

        //private void FindSheetRegion(Data.InspectionResult inspectionResult, AlgoImage algoImage, ref Rectangle interestRect, ref bool isSkip)
        //{
        //    Data.Model model = (Data.Model)SystemManager.Instance().CurrentModel;
        //    PinholeParamValue paramValue;
        //    if (inspectionResult.DeviceIndex == 0)
        //        paramValue = model.InspectParam.PinholeParamValue1;
        //    else
        //        paramValue = model.InspectParam.PinholeParamValue2;

        //    int bin = paramValue.BinarizeValue;
        //    int passSize = PinholeSettings.Instance().SkipLength;

        //    ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

        //    AlgoImage maskImage = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, algoImage.Width, algoImage.Height);

        //    imageProcessing.Binarize(algoImage, maskImage, bin);

        //    imageProcessing.Dilate(maskImage, 10);
        //    imageProcessing.Erode(maskImage, 10);


        //    BlobParam interestBlobParam = new BlobParam();
        //    BlobRectList interestBlobRectList = imageProcessing.Blob(maskImage, interestBlobParam);

        //    interestBlobRectList.Dispose();
        //    maskImage.Dispose();

        //    BlobRect maxBlob = interestBlobRectList.GetMaxAreaBlob();
        //    if (maxBlob == null)
        //    {
        //        isSkip = true;
        //        return;
        //    }

        //    int width = algoImage.Width;
        //    int height = algoImage.Height;

        //    if (maxBlob.BoundingRect.X == 0)
        //        interestRect = new Rectangle((int)maxBlob.BoundingRect.Width, 0, width - (int)maxBlob.BoundingRect.Width, height);
        //    else
        //        interestRect = new Rectangle(0, 0, width - (int)maxBlob.BoundingRect.Width, height);

        //    // 왼쪽
        //    if (maxBlob.BoundingRect.X == 0)
        //        interestRect.X += passSize;

        //    interestRect.Width -= passSize;

        //    //    skipRect.X = interestRect.Right - passSize;

        //    //interestRect = new Rectangle(0, 0, width, height);

        //    //interestRect.Width -= passSize;

        //    if (interestRect.Width <= 0)
        //    {
        //        isSkip = true;
        //        return;
        //    }
        //    inspectionResult.InterestRegion = interestRect;

        //    return;
        //}

        public Data.InspectionResult Inspect(ImageD image, Data.InspectionResult inspectResult)
        {
            //1. 검사에 사용할 이미지 및 라이브러리 확인, DeviceIndex를 통한 파라미터 구분
            AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, image, ImageType.Grey);
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            var model = SystemManager.Instance().CurrentModel as Data.Model;
            PinholeParamValue paramValue;
            if (inspectResult.DeviceIndex == 0)
                paramValue = model.InspectParam.PinholeParamValue1; // CAM1
            else /*(inspectResult.DeviceIndex == 1)*/
                paramValue = model.InspectParam.PinholeParamValue2; // CAM2

            #region Display 화면 갱신을 위한 작업 ( 원본의 1/5 크기 )
            Size displaySize = new Size((int)((float)algoImage.Width * 0.2f), (int)((float)algoImage.Height * 0.2f));
            using (AlgoImage displayImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, algoImage.ImageType, displaySize))
            {
                //Dest 사이즈로 변환만 사용하시요.
                //OpenCV 반올림 문제 있음. cvImage.size랑 실제 Mat 사이즈 문제 발생할수 있음
                imageProcessing.Resize(algoImage, displayImage, 0, 0);
                inspectResult.DisplayBitmap = displayImage.ToBitmap();
                inspectResult.SaveBitmap = displayImage.ToBitmap();
                //algoImage.Save("d:\\TEST\\10.Algo_SaveImage.bmp");//OK
                //var bitmap = image.ToBitmap(); //OK
                //bitmap.Save("d:\\TEST\\20.ImageD_Bitmap_src.bmp", ImageFormat.Bmp);//OK
                //displayImage.Save("d:\\TEST\\30.Algo_Resize.bmp"); //OK
                //var bitmap2 = displayImage.ToBitmap(); //OK
                //bitmap2.Save("d:\\TEST\\40.ResizeAlgo_Bitmap.bmp", ImageFormat.Bmp);//NG >> 수정OK
                //imageD.Save("d:\\TEST\\50.ImageD_save.RAW"); //RAW 저장은되나 영상이상함. 
                //imageD.SaveImage("d:\\TEST\\60.ImageD_saveImage.bmp"); //OK -> unsafe로...
            }
            #endregion

            //3. 실제 검사 할 영역 찾기 (일부 영역을 짤라서 모서리를 찾음)
            double[] edgePosition;
            int edgeFinderHight = 200;
            //int startX = inspectResult.DeviceIndex == 0 ? 0 : algoImage.Width;
            Rectangle edgeFinderArea = new Rectangle(new Point(0, algoImage.Height / 2 - edgeFinderHight / 2),
                new Size(algoImage.Width, edgeFinderHight));
            using (AlgoImage edgeFinderImage = algoImage.GetSubImage(Rectangle.Round(edgeFinderArea)))
            {
                edgePosition = edgePositionFinder.SheetEdgePosition(edgeFinderImage,
                    new double[] { paramValue.EdgeThreshold },
                    inspectResult.DeviceIndex == 0 ? SearchDireciton.LeftToRight : SearchDireciton.RightToLeft);
            }

            //4. (레시피에 설정된 SkipLength) + (3에서 찾은 edgePosition) 만큼 영역을 자름
            Rectangle roi = new Rectangle();
            int skipWidth = PinholeSettings.Instance().SkipLength + (int)edgePosition[0];
            if (inspectResult.DeviceIndex == 0)
                roi = new Rectangle(skipWidth, 0, image.Width - skipWidth, image.Height);
            else /*(inspectResult.DeviceIndex == 1)*/
                roi = new Rectangle(0, 0, image.Width - skipWidth, image.Height);

            //4-1. 엣지를 못찾았거나 roi가 0보다 작을 경우 skip함
            if (edgePosition[0] == 0 || roi.Width <= 0)
            {
                algoImage.Dispose();
                inspectResult.Judgment = Judgment.Skip;
                return inspectResult;
            }
            roi.Height = image.Height;
            inspectResult.InterestRegion = roi;

            string strpath = string.Format("D:\\TEST\\{0}\\", inspectResult.DeviceIndex);

            

            //5. 이미지 처리
            using (AlgoImage interestImage = algoImage.GetSubImage(Rectangle.Round(inspectResult.InterestRegion)))
            using (AlgoImage interestAvgImage = algoImage.GetSubImage(new Rectangle(roi.X, algoImage.Height / 2 - edgeFinderHight / 2, roi.Width, edgeFinderHight)))
            using (AlgoImage sobelBinImage = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, roi.Width, roi.Height))
            {
                //interestImage.Save(strpath + ("10.interestImage.bmp"));
                //interestAvgImage.Save(strpath + ("20.interestAvgImage.bmp"));
                //sobelBinImage.Save(strpath + ("30.sobelBinImage.bmp"));
                //5-1. 소벨을통한 엣지검출 및 이진화
                imageProcessing.Sobel(interestImage, sobelBinImage);
                //sobelBinImage.Save(strpath + ("31.sobelBinImage.bmp"));
                imageProcessing.Binarize(sobelBinImage, sobelBinImage, paramValue.DefectThreshold);
                //sobelBinImage.Save(strpath + ("32.sobelBinImage.bmp"));

                //5-2. 이진화된 영역에서 불량이 있는지 확인하여 검출
                BlobParam blobParam = new BlobParam();
                blobParam.MaxCount = 10000;
                blobParam.SelectGrayMeanValue = true;

                float avgValue = imageProcessing.GetGreyAverage(interestAvgImage);
                int offsetX = (int)inspectResult.InterestRegion.X;
                using (BlobRectList blobRectList = imageProcessing.Blob(sobelBinImage, blobParam, interestImage))
                {
                    //if (blobRectList.Count > 0) ;
                    AddDefectBlob(algoImage, blobRectList, inspectResult, avgValue, offsetX);
                    if (topBottomMergeCheck)
                        ReCheckList(algoImage, blobRectList, inspectResult, mergeCheckPool[inspectResult.DeviceIndex]);
                }
            }

            if (inspectResult.NumDefect > 0)
                inspectResult.Judgment = Judgment.Reject;

            algoImage.Dispose();

            return inspectResult;
        }
        public void ReCheckList(AlgoImage algoImage, BlobRectList blobRectList, Data.InspectionResult inspectionResult, ObjectPoolQueue<(AlgoImage, DefectInfoList)> defectinfolist)
        {
            if (inspectionResult.LastDefectInfoList.Count() == 0)
            {
                mergeCheckPool[inspectionResult.DeviceIndex].ClearObject();
                return;
            }
            var reCheckDefectList = new DefectInfoList();
            foreach (var defect in inspectionResult.LastDefectInfoList)
            {
                if (defect.DefectType == Data.DefectType.Pinhole || defect.DefectType == Data.DefectType.Dust)
                {
                    defectinfolist.ClearObject();
                    mergeCheckPool[inspectionResult.DeviceIndex].ClearObject();
                    return;
                }
                else if (defect.DefectType == Data.DefectType.ReCheckTop || defect.DefectType == Data.DefectType.ReCheckBottom)
                {
                    reCheckDefectList.Add(defect);
                }
            }
            foreach (var defect in reCheckDefectList)
            {
                inspectionResult.LastDefectInfoList.Remove(defect);
            }

            mergeCheckPool[inspectionResult.DeviceIndex].PutObject((algoImage, reCheckDefectList));
            if (mergeCheckPool[inspectionResult.DeviceIndex].Count() >= 2)
            {
                (AlgoImage, DefectInfoList) beforeReCheckDefectList = mergeCheckPool[inspectionResult.DeviceIndex].GetObject();
                foreach (var before in beforeReCheckDefectList.Item2)
                {
                    foreach (var current in reCheckDefectList)
                    {
                        if (before.DefectType == Data.DefectType.ReCheckTop && current.DefectType == Data.DefectType.ReCheckBottom)
                        {
                            if (Math.Abs(before.BoundingRect.X - current.BoundingRect.X) < 10)
                            {
                                ReAddDefectBlob(beforeReCheckDefectList.Item2, algoImage, before.BoundingRect, current.BoundingRect);
                                inspectionResult.LastDefectInfoList.Add(current);
                            }
                        }
                    }
                }
            }
            inspectionResult.NumDefect = inspectionResult.LastDefectInfoList.Count;
        }

        public long[] GetHisto(ImageD image)
        {
            using (AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, image, ImageType.Grey))
            {
                ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
                long[] histo = imageProcessing.Histogram(algoImage);
                return histo;
            }
        }
        protected void ReAddDefectBlob(DefectInfoList beforeImage, AlgoImage currentImage, RectangleF beforDefectRect, RectangleF afterDefectRect)
        {
            var (beforeRect, afterRect) = ReSizeRect(beforDefectRect, afterDefectRect);

        }
        protected (RectangleF, RectangleF) ReSizeRect(RectangleF before, RectangleF after)
        {
            RectangleF reBeforeRect = before;
            reBeforeRect.Y = before.Y < after.Y ? before.Y : after.Y;
            reBeforeRect.Width = (before.Y + before.Width) > (after.Y + after.Width) ?
                (before.Y + before.Width - reBeforeRect.Y) : (after.Width + after.Y - reBeforeRect.Y);

            RectangleF reAfterRect = after;
            reAfterRect.Y = reBeforeRect.Y;
            reAfterRect.Width = reBeforeRect.Width;

            return (reBeforeRect, reAfterRect);
        }

        void AddDefectBlob(AlgoImage algoImage, BlobRectList blobList, Data.InspectionResult inspectResult, float avgValue, int offset)
        {
            int defectNum = 0;

            List<BlobRect> blobRectList = blobList.GetList();
            blobRectList = blobRectList.OrderByDescending(blobRect => blobRect.Area).ToList();
            LogHelper.Debug(LoggerType.Inspection, string.Format("Start Clip Defects, {0}EA", blobRectList.Count));

            var model = SystemManager.Instance().CurrentModel as Data.Model;
            NGType ngType = model.InspectParam.NGType;
            switch(ngType)
            {
                case NGType.Any:
                    break;
                case NGType.Bright:
                    blobRectList.RemoveAll(f => f.MeanValue < avgValue); // 어두운놈 지우기
                    break;
                case NGType.Dark:
                    blobRectList.RemoveAll(f => f.MeanValue > avgValue); //밝은놈 지우기
                    break;
            }
            
            foreach (BlobRect blobRect in blobRectList)
            {
                if (inspectResult.NumDefect >= PinholeSettings.Instance().MaxDefect)
                    return;

                

                float width = blobRect.BoundingRect.Width * PinholeSettings.Instance().PixelResolution;
                float height = blobRect.BoundingRect.Height * PinholeSettings.Instance().PixelResolution;

                Data.DefectType defectType = Data.DefectType.Invaild;


                if (width < PinholeSettings.Instance().SmallSize.Width * 1000 && height < PinholeSettings.Instance().SmallSize.Height * 1000)
                {
                    if (topBottomMergeCheck)
                    {
                        if (blobRect.BoundingRect.Y < PinholeSettings.Instance().MergeCheckTop)
                            defectType = Data.DefectType.ReCheckTop;
                        else if (blobRect.BoundingRect.Y > (algoImage.Height - PinholeSettings.Instance().MergeCheckBottom))
                            defectType = Data.DefectType.ReCheckBottom;
                    }

                    continue;
                }


                RectangleF boundingRect = blobRect.BoundingRect;
                boundingRect.Offset(offset, 0);

                Rectangle clipRect = Rectangle.Round(boundingRect);
                Rectangle srcRect = new Rectangle(0, 0, algoImage.Width, algoImage.Height);
                clipRect.Inflate(10, 10);
                clipRect.Intersect(srcRect);

                if (clipRect.Width == 0 || clipRect.Height == 0)
                    continue;

                if ((defectType != Data.DefectType.ReCheckTop) && (defectType != Data.DefectType.ReCheckBottom))
                    defectType = blobRect.MeanValue < avgValue ? Data.DefectType.Dust : Data.DefectType.Pinhole;

                using (AlgoImage subImage = algoImage.GetSubImage(clipRect))
                {
                    byte[] data = subImage.GetByte();
                    Bitmap bitmap = ImageHelper.CreateBitmap(clipRect.Width, clipRect.Height, clipRect.Width, 1, data);
                    PointF realPosition = new PointF((blobRect.CenterPt.X + offset) * PinholeSettings.Instance().PixelResolution /*mm*/ , (blobRect.CenterPt.Y * PinholeSettings.Instance().PixelResolution) + inspectResult.RollDistance);

                    DefectInfo defectInfo = new DefectInfo(inspectResult.DeviceIndex, inspectResult.SectionIndex, defectNum,
                                                                boundingRect, blobRect.CenterPt, realPosition, defectType
                                                                , (int)blobRect.MinValue, (int)blobRect.MaxValue, bitmap);
                    inspectResult.AddDefectInfo(defectInfo); //여기서 inspectResult.NumDefect ++ 됨

                }
                defectNum++;
            }
            LogHelper.Debug(LoggerType.Inspection, "End Clip Defects");
        }
    }
}
