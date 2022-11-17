using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanS.Screen.Data;
using UniScanS.Vision;

namespace UniScanS.Screen.Vision.Detector.Shape
{
    public class ShapeInspector
    {
        ShapeInspectorParam param;
        
        public ShapeInspector(ShapeInspectorParam param)
        {
            this.param = param;
        }

        public void Inspect(AlgoImage interest, AlgoImage maskR, RegionInfo regionInfo, Rectangle srcRegion, ref List<ShapeResult> shapeList, ref List<BlobRect> notNecessaryList, ref List<BlobRect> needInspectList)
        {
            if (param.UseInspect == false  )
                return;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(maskR);

            BlobParam blobParam = new BlobParam();
            blobParam.SelectCenterPt = true;
            blobParam.SelectIsTouchBorder = true;
            
            //blobParam.AreaMin = param.MinPatternArea;

            BlobRectList blobRectList = imageProcessing.Blob(maskR, blobParam);
            AlgorithmCommon.Instance().AddDisposeList(blobRectList);
            
            Rectangle srcRect = new Rectangle(0, 0, interest.Width, interest.Height);
            List<BlobRect> tempNeedInspectList = new List<BlobRect>();
            List<BlobRect> tempNotNecessaryList = new List<BlobRect>();
            List<ShapeResult> tempShapeList = new List<ShapeResult>();

            List<BlobRect> shapeBlobRectList = blobRectList.GetList();

            float maxArea = 0;
            if (param.PatternList.Count >= 1)
                maxArea = param.PatternList.OrderByDescending(p => p.PatternGroup.AverageArea).First().PatternGroup.AverageArea;

            Parallel.ForEach(shapeBlobRectList, blobRect =>
            //foreach (BlobRect blobRect in shapeBlobRectList)
            {
                if (blobRect.IsTouchBorder == true)
                {
                    lock (tempNotNecessaryList)
                        tempNotNecessaryList.Add(blobRect);
                    return;
                }

                if (blobRect.Area < param.MinPatternArea)
                    return;

                ShapeDiffValue shapeDiffValue = new ShapeDiffValue(true);

                foreach (SheetPattern pattern in param.PatternList)
                {
                    ShapeDiffValue subShapeDiffValue = new ShapeDiffValue(false);
                    subShapeDiffValue.AreaDiff = pattern.PatternGroup.GetDiffValue(PatternFeature.Area, blobRect);
                    subShapeDiffValue.WidthDiff = pattern.PatternGroup.GetDiffValue(PatternFeature.Width, blobRect);
                    subShapeDiffValue.HeightDiff = pattern.PatternGroup.GetDiffValue(PatternFeature.Height, blobRect);
                    subShapeDiffValue.CenterXDiff = pattern.PatternGroup.GetDiffValue(PatternFeature.CenterX, blobRect);
                    subShapeDiffValue.CenterYDiff = pattern.PatternGroup.GetDiffValue(PatternFeature.CenterY, blobRect);

                    if (shapeDiffValue.SumDiff > subShapeDiffValue.SumDiff)
                    {
                        shapeDiffValue = subShapeDiffValue;
                        shapeDiffValue.SimilarPattern = pattern;
                    }
                }

                if (shapeDiffValue.SimilarPattern.NeedInspect == false)
                {
                    lock (tempNotNecessaryList)
                        tempNotNecessaryList.Add(blobRect);
                    return;
                }

                if (param.UseHeightDiffTolerence == true)
                    shapeDiffValue.SetTolerance(param.DiffTolerence, param.HeightDiffTolerence);
                else
                    shapeDiffValue.SetTolerance(param.DiffTolerence);

                if (shapeDiffValue.SimilarPattern.NeedInspect == true)
                {
                    if (shapeDiffValue.IsDefect())
                    {
                        ShapeResult subResult = new ShapeResult();
                        subResult.ShapeDiffValue = shapeDiffValue;

                        SheetSubResult sheetSubResult = (SheetSubResult)subResult;
                        sheetSubResult.Length = Math.Max(blobRect.BoundingRect.Width, blobRect.BoundingRect.Height);
                        sheetSubResult.RealLength = Math.Max(blobRect.BoundingRect.Width * AlgorithmSetting.Instance().XPixelCal,
                            blobRect.BoundingRect.Height * AlgorithmSetting.Instance().YPixelCal);
                        sheetSubResult.Area = (int)blobRect.Area;
                        Rectangle region = Rectangle.Round(blobRect.BoundingRect);
                        region.Intersect(srcRect);
                        sheetSubResult.Region = region;
                        sheetSubResult.SrcRegion = srcRegion;

                        lock (tempShapeList)
                            tempShapeList.Add(subResult);
                    }
                    else
                    {
                        lock (tempNeedInspectList)
                            tempNeedInspectList.Add(blobRect);
                    }
                }
            });

            tempShapeList = tempShapeList.OrderBy(x => x.Region.X + x.Region.Y).ToList();

            needInspectList.AddRange(tempNeedInspectList);
            notNecessaryList.AddRange(tempNotNecessaryList);
            shapeList.AddRange(tempShapeList);
        }
    }
}
