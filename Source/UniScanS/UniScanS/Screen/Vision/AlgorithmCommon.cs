using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanS.Screen.Data;
using UniScanS.Vision;

namespace UniScanS.Screen.Vision
{
    public class AlgorithmCommon
    {
        Thread disposeThread;
        List<IDisposable> disposeList;

        static AlgorithmCommon _instance;
        public static AlgorithmCommon Instance()
        {
            if (_instance == null)
                _instance = new AlgorithmCommon();

            return _instance;
        }

        public AlgorithmCommon()
        {
            disposeList = new List<IDisposable>();

            disposeThread = new Thread(Dispose);
            disposeThread.Start();
        }
        
        private void Dispose()
        {
            while (true)
            {
                if (disposeList.Count == 0)
                {
                    Thread.Sleep(0);
                    continue;
                }

                IDisposable disposeObj = null;
                lock (disposeList)
                {
                    disposeObj = disposeList.First();
                    disposeList.Remove(disposeObj);
                }

                disposeObj.Dispose();
            }
        }

        public void AddDisposeList(IDisposable disposeObj)
        {
            lock (disposeList)
                disposeList.Add(disposeObj);
        }

        public bool CreateMaskImage(AlgoImage bin, AlgoImage mask, int minPatternArea, bool fillHole)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(bin);
            
            BlobParam patternBlobParam = new BlobParam();
            patternBlobParam.SelectBoundingRect = false;
            patternBlobParam.SelectLabelValue = true;
            patternBlobParam.AreaMin = minPatternArea;

            BlobRectList patternBlobRectList = imageProcessing.Blob(bin, patternBlobParam);

            List<BlobRect> blobRectList = patternBlobRectList.GetList();

            DrawBlobOption drawBlobOption = new DrawBlobOption();
            drawBlobOption.SelectBlob = true;

            imageProcessing.DrawBlob(mask, patternBlobRectList, null, drawBlobOption);
            AddDisposeList(patternBlobRectList);

            if (fillHole == true)
                imageProcessing.FillHoles(mask, mask);

            if (blobRectList.Count == 0)
                return false;

            return true;
        }

        public void RefineSubResult(AlgoImage interest, int emptyPos, AlgoImage binImage, ref List<SheetSubResult> sheetSubResultList)
        {
            Rectangle srcRect = new Rectangle(0, 0, interest.Width, emptyPos);

            List<SheetSubResult> outRangeResultList = new List<SheetSubResult>();

            Parallel.ForEach(sheetSubResultList, subResult =>
            {
                Rectangle offsetRect = Rectangle.Truncate(subResult.Region);
                offsetRect.Offset(subResult.SrcRegion.X, subResult.SrcRegion.Y);
                
                if (offsetRect.IntersectsWith(srcRect) == false)
                {
                    lock (outRangeResultList)
                        outRangeResultList.Add(subResult);
                    return;
                }

                subResult.Region = offsetRect;

                Rectangle inflateRect = Rectangle.Truncate(subResult.Region);
                inflateRect.Inflate(50, 50);
                inflateRect.Intersect(srcRect);

                if (inflateRect.Width == 0 || inflateRect.Height == 0)
                {
                    lock (outRangeResultList)
                        outRangeResultList.Add(subResult);
                    return;
                }

                AlgoImage subInterest = interest.GetSubImage(inflateRect);
                subResult.Image = subInterest.ToImageD().ToBitmap();
                AlgorithmCommon.Instance().AddDisposeList(subInterest);

                if (binImage != null)
                {
                    AlgoImage subInspect = binImage.GetSubImage(inflateRect);
                    subResult.BinaryImage = subInspect.ToImageD().ToBitmap();
                    AlgorithmCommon.Instance().AddDisposeList(subInspect);
                }

                subResult.RealPos = new PointF(subResult.Region.X * AlgorithmSetting.Instance().XPixelCal,
                    subResult.Region.Y * AlgorithmSetting.Instance().YPixelCal);
            });

            foreach (SheetSubResult subResult in outRangeResultList)
                sheetSubResultList.Remove(subResult);
        }

        public bool IsNecessaryDefect(BlobRect blobRect, List<BlobRect> notNecessaryList)
        {
            foreach (BlobRect NecessaryBlob in notNecessaryList)
            {
                if (NecessaryBlob.BoundingRect.Contains(blobRect.CenterPt) == true)
                    return true;
            }

            return false;
        }

        public int GetEmptyRegionPos(AlgoImage sourceImage)
        {
            byte[] sourceImageData = sourceImage.GetByte();

            int y = sourceImage.Height - 1;
            int halfWidth = sourceImage.Width / 2;

            int index = 0;
            while (true)
            {
                index = y * sourceImage.Width + halfWidth;

                if (sourceImageData[index] == 0)
                {
                    y--;
                    break;
                }
                    

                if (y == 0)
                    break;

                y--;
            }

            if (y == 0)
                y = sourceImage.Height - 1;

            return y;
        }

        public  void MergeBlobs(int inflate, ref List<BlobRect> blobRectList)
        {
            bool merged = true;

            int tryNum = 0;
            while (merged == true)
            {
                merged = false;

                if (tryNum % 2 == 0)
                    blobRectList = blobRectList.OrderBy(defect => defect.BoundingRect.X).ToList();
                else
                    blobRectList = blobRectList.OrderBy(defect => defect.BoundingRect.Y).ToList();

                for (int srcIndex = 0; srcIndex < blobRectList.Count; srcIndex++)
                {
                    BlobRect srcBlob = blobRectList[srcIndex];

                    int endSearchIndex = srcIndex + 1;

                    if (tryNum % 2 == 0)
                    {
                        for (int i = endSearchIndex; i < blobRectList.Count; i++)
                        {
                            if (blobRectList[i].BoundingRect.Left - srcBlob.BoundingRect.Right <= inflate)
                                endSearchIndex = i;
                            else
                                break;
                        }
                    }
                    else
                    {
                        for (int i = endSearchIndex; i < blobRectList.Count; i++)
                        {
                            if (blobRectList[i].BoundingRect.Top - srcBlob.BoundingRect.Bottom <= inflate)
                                endSearchIndex = i;
                            else
                                break;
                        }
                    }

                    RectangleF inflateRect = srcBlob.BoundingRect;
                    inflateRect.Inflate(inflate, inflate);

                    for (int destIndex = srcIndex + 1; destIndex <= endSearchIndex && destIndex < blobRectList.Count; destIndex++)
                    {
                        BlobRect destBlob = blobRectList[destIndex];

                        if (inflateRect.IntersectsWith(destBlob.BoundingRect) == true)
                        {
                            srcBlob = srcBlob + destBlob;
                            blobRectList[srcIndex] = srcBlob;

                            blobRectList.RemoveAt(destIndex);

                            endSearchIndex--;
                            destIndex--;

                            if (tryNum % 2 == 0)
                            {
                                for (int i = endSearchIndex + 1; i < blobRectList.Count; i++)
                                {
                                    if (blobRectList[i].BoundingRect.Left - srcBlob.BoundingRect.Right <= inflate)
                                        endSearchIndex = i;
                                    else
                                        break;
                                }
                            }
                            else
                            {
                                for (int i = endSearchIndex + 1; i < blobRectList.Count; i++)
                                {
                                    if (blobRectList[i].BoundingRect.Top - srcBlob.BoundingRect.Bottom <= inflate)
                                        endSearchIndex = i;
                                    else
                                        break;
                                }
                            }

                            if (merged == false)
                                merged = true;

                            inflateRect = srcBlob.BoundingRect;
                            inflateRect.Inflate(inflate, inflate);
                        }
                    }
                }

                if (merged == true)
                    tryNum++;
            }

            blobRectList = blobRectList.OrderBy(defect => defect.BoundingRect.X + defect.BoundingRect.Y).ToList();
        }

        public void RemoveIntersectBlobs(float inflateValue, ref List<BlobRect> blobList)
        {
            blobList = blobList.OrderByDescending(defect => defect.Area).ToList();

            for (int srcIndex = 0; srcIndex < blobList.Count; srcIndex++)
            {
                RectangleF region = blobList[srcIndex].BoundingRect;
                region.Inflate(inflateValue, inflateValue);

                for (int destIndex = srcIndex + 1; destIndex < blobList.Count; destIndex++)
                {
                    if (region.IntersectsWith(blobList[destIndex].BoundingRect) == true)
                    {
                        blobList.RemoveAt(destIndex);
                        destIndex--;
                    }
                }
            }
        }
    }
}
