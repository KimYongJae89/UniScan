using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanS.Screen.Data;
using UniScanS.Screen.Settings;
using UniScanS.Vision;

namespace UniScanS.Screen.Vision.Detector.Dielectric
{
    public class DielectricInspector
    {
        DielectricInspectorParam param;
       
        public DielectricInspector(DielectricInspectorParam param)
        {
            this.param = param;
        }

        public void Inspect(AlgoImage interest, AlgoImage dielectricInspect, AlgoImage dielectricMask, RegionInfo regionInfo, Rectangle srcRegion,
            ref List<SheetSubResult> dielectricList, ref List<SheetSubResult> pinHoleList, ref bool isReached)
        {
            if (param.UseLower == false && param.UseUpper == false)
                return;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(interest);
            
            if (param.UseLower == true && param.UseUpper == true)
                imageProcessing.Binarize(interest, dielectricInspect, Math.Max(0, regionInfo.DielectricValue - param.LowerThreshold), Math.Min(255, regionInfo.DielectricValue + param.UpperThreshold), true);
            else if (param.UseLower == true)
                imageProcessing.Binarize(interest, dielectricInspect, Math.Max(0, regionInfo.DielectricValue - param.LowerThreshold), true);
            else if (param.UseUpper == true)
                imageProcessing.Binarize(interest, dielectricInspect, Math.Min(255, regionInfo.DielectricValue + param.UpperThreshold));
            //dielectricInspect.Save("dielectricInspect.bmp", new DebugContext(true, "d:\\"));
            //dielectricMask.Save("dielectricMask.bmp", new DebugContext(true, "d:\\"));

            imageProcessing.And(dielectricInspect, dielectricMask, dielectricInspect);
            
            BlobParam blobParam = new BlobParam();
            blobParam.MaxCount = AlgorithmSetting.Instance().MaxDefectNum;
            blobParam.EraseBorderBlobs = true;
            blobParam.SelectArea = true;
            blobParam.SelectCenterPt = true;
            blobParam.SelectBoundingRect = true;
            blobParam.SelectGrayMinValue = true;
            blobParam.SelectGrayMaxValue = true;
            blobParam.SelectSigmaValue = true;
            blobParam.SelectCompactness = true;
            blobParam.SelectRotateRect = true;
            blobParam.SelectFeretDiameter = true;
            blobParam.SelectElongation = true;
            //전극이랑 달라요.. 밑에 3개
            blobParam.EraseBorderBlobs = true;
            
            BlobRectList blobRectList = imageProcessing.Blob(dielectricInspect, blobParam, interest);
            List<BlobRect> defectList = blobRectList.GetList();
            AlgorithmCommon.Instance().AddDisposeList(blobRectList);

            if (blobRectList.IsReached == true)
            {
                isReached = true;
                return;
            }

            //AlgorithmCommon.Instance().RemoveIntersectBlobs(ref defectList);
            //AlgorithmCommon.Instance().MergeBlobs(25, ref defectList);

            //ImageHelper.SaveImage(interest.ToImageD().ToBitmap(), "..\\interest.bmp");
            //ImageHelper.SaveImage(dielectricMask.ToImageD().ToBitmap(), "..\\dielectricMask.bmp");
            //ImageHelper.SaveImage(dielectricInspect.ToImageD().ToBitmap(), "..\\dielectricInspect.bmp");

            Rectangle srcRect = new Rectangle(0, 0, interest.Width, interest.Height);

            List<SheetSubResult> tempDielectricList = new List<SheetSubResult>();
            List<SheetSubResult> tempPinHoleList = new List<SheetSubResult>();
            
            float calValue = (AlgorithmSetting.Instance().XPixelCal + AlgorithmSetting.Instance().YPixelCal) / 2.0f;
            //foreach (BlobRect blobRect in defectList)
            Parallel.ForEach(defectList, blobRect =>
            {
                SheetSubResult subResult = new SheetSubResult();

                subResult.Length = blobRect.MaxFeretDiameter;
                subResult.RealLength = blobRect.MaxFeretDiameter * calValue;

                float lowerValue = 0;
                float upperValue = 0;
                if (regionInfo.DielectricValue > blobRect.MinValue)
                    lowerValue = regionInfo.DielectricValue - blobRect.MinValue;

                if (regionInfo.DielectricValue < blobRect.MaxValue)
                    upperValue = blobRect.MaxValue - regionInfo.DielectricValue;

                subResult.SetThreshold(param.LowerThreshold, param.UpperThreshold);
                subResult.LowerDiffValue = lowerValue;
                subResult.UpperDiffValue = upperValue;
                subResult.Compactness = blobRect.Compactness;
                subResult.Elongation = blobRect.Elongation;
                Rectangle region = Rectangle.Round(blobRect.BoundingRect);
                region.Intersect(srcRect);
                subResult.Region = region;
                subResult.SrcRegion = srcRegion;
                subResult.Area = (int)blobRect.Area;

                if ((subResult.Compactness <= AlgorithmSetting.Instance().DielectricCompactness || subResult.Elongation <= AlgorithmSetting.Instance().DielectricElongation) && lowerValue > 0)
                {
                    if (subResult.RealLength < AlgorithmSetting.Instance().PinHoleMinSize)
                        return;

                    subResult.DefectType = DefectType.PinHole;

                    lock (tempPinHoleList)
                        tempPinHoleList.Add(subResult);
                }
                else
                {
                    if (subResult.RealLength < AlgorithmSetting.Instance().DielectricMinSize)
                        return;

                    subResult.DefectType = DefectType.Dielectric;

                    lock (tempDielectricList)
                        tempDielectricList.Add(subResult);
                }

                //AlgorithmCommon.Instance().RefineSubResult(interest, dielectricInspect, srcRect, blobRect, regionInfo.Region, ref subResult);
            });

            pinHoleList.AddRange(tempPinHoleList);
            dielectricList.AddRange(tempDielectricList);
        }
    }
}
