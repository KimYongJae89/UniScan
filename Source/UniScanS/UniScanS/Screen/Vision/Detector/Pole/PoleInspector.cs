using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanS.Screen.Data;
using UniScanS.Screen.Settings;
using UniScanS.Vision;

namespace UniScanS.Screen.Vision.Detector.Pole
{
    public class PoleInspector
    {
        PoleInspectorParam param;

        public PoleInspector(PoleInspectorParam param)
        {
            this.param = param;
        }

        public void Inspect(AlgoImage interest, AlgoImage poleInspect, AlgoImage poleMask, RegionInfo regionInfo, Rectangle srcRegion, List<BlobRect> notNecessaryList,
            ref List<SheetSubResult> sheetAttackList, ref List<SheetSubResult> poleList, ref bool isReached)
        {
            if (param.UseLower == false && param.UseUpper == false)
                return;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(interest);
            
            if (param.UseLower == true && param.UseUpper == true)
                imageProcessing.Binarize(interest, poleInspect, Math.Max(0, regionInfo.PoleValue - param.LowerThreshold), Math.Min(255, regionInfo.PoleValue + param.UpperThreshold), true);
            else if (param.UseLower == true)
                imageProcessing.Binarize(interest, poleInspect, Math.Max(0, regionInfo.PoleValue - param.LowerThreshold), true);
            else if (param.UseUpper == true)
                imageProcessing.Binarize(interest, poleInspect, Math.Min(255, regionInfo.PoleValue + param.UpperThreshold));

            imageProcessing.And(poleInspect, poleMask, poleInspect);

            BlobParam blobParam = new BlobParam();
            blobParam.MaxCount = AlgorithmSetting.Instance().MaxDefectNum;
            blobParam.SelectArea = true;
            blobParam.SelectCenterPt = true;
            blobParam.SelectBoundingRect = true;
            blobParam.SelectGrayMaxValue = true;
            blobParam.SelectGrayMinValue = true;
            //blobParam.SelectSigmaValue = true;
            blobParam.EraseBorderBlobs = true;
            blobParam.SelectCompactness = true;
            blobParam.SelectFeretDiameter = true;
            blobParam.SelectElongation = true;
            //blobParam.SelectRotateRect = true;

            //imageProcessing.Dilate(poleInspect, 1);
            BlobRectList blobRectList = imageProcessing.Blob(poleInspect, blobParam, interest);
            List<BlobRect> defectList = blobRectList.GetList();
            AlgorithmCommon.Instance().AddDisposeList(blobRectList);

            if (blobRectList.IsReached == true)
            {
                isReached = true;
                return;
            }

            defectList.RemoveAll(defect => AlgorithmCommon.Instance().IsNecessaryDefect(defect, notNecessaryList));

            Rectangle srcRect = new Rectangle(0, 0, interest.Width, interest.Height);
            
            List<SheetSubResult> tempSheetAttackList = new List<SheetSubResult>();
            List<SheetSubResult> tempPoleList = new List<SheetSubResult>();
            
            float calValue = (AlgorithmSetting.Instance().XPixelCal + AlgorithmSetting.Instance().YPixelCal) / 2.0f;

            Parallel.ForEach(defectList, blobRect =>
            {
                SheetSubResult subResult = new SheetSubResult();

                subResult.Length = blobRect.MaxFeretDiameter;
                subResult.RealLength = subResult.Length * calValue;

                float lowerValue = 0;
                float upperValue = 0;

                if (regionInfo.PoleValue > blobRect.MinValue)
                    lowerValue = regionInfo.PoleValue - blobRect.MinValue;

                if (regionInfo.PoleValue < blobRect.MaxValue)
                    upperValue = blobRect.MaxValue - regionInfo.PoleValue;

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

                if (subResult.RealLength >= AlgorithmSetting.Instance().SheetAttackMinSize 
                && lowerValue > 0
                && (subResult.Compactness > AlgorithmSetting.Instance().PoleCompactness || subResult.Elongation > AlgorithmSetting.Instance().PoleElongation))
                {
                    subResult.DefectType = DefectType.SheetAttack;

                    lock (tempSheetAttackList)
                        tempSheetAttackList.Add(subResult);
                }
                else
                {
                    if (subResult.RealLength < AlgorithmSetting.Instance().PoleMinSize)
                        return;

                    subResult.DefectType = DefectType.Pole;

                    lock (tempPoleList)
                        tempPoleList.Add(subResult);
                }
                
                //AlgorithmCommon.Instance().RefineSubResult(interest, poleInspect, srcRect, blobRect, regionInfo.Region, ref subResult);
            });

            sheetAttackList.AddRange(tempSheetAttackList);
            poleList.AddRange(tempPoleList);
        }
    }
}
