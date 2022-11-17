using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanS.Common.Settings;
using UniScanS.Screen.Data;
using UniScanS.Screen.Vision.Detector.Dielectric;
using UniScanS.Screen.Vision.Detector.Pole;
using UniScanS.Screen.Vision.Detector.Shape;
using UniScanS.Vision;

namespace UniScanS.Screen.Vision.Detector
{
    public class SheetInspector : DynMvp.Vision.Algorithm
    {
        PoleInspector poleInspector;
        DielectricInspector dielectricInspector;
        ShapeInspector shapeInspector;

        public static string TypeName
        {
            get { return "SheetS_Inspector"; }
        }

        public SheetInspector()
        {
            param = new SheetInspectorParam();

            SheetInspectorParam sheetInspectorParam = (SheetInspectorParam)this.param;

            poleInspector = new PoleInspector(sheetInspectorParam.PoleParam);
            dielectricInspector = new DielectricInspector(sheetInspectorParam.DielectricParam);
            shapeInspector = new ShapeInspector(sheetInspectorParam.ShapeParam);
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

            SheetInspector srcAlgorithm = (SheetInspector)algorithm;
            this.param.CopyFrom(srcAlgorithm.param);
        }

        public override DynMvp.Vision.Algorithm Clone()
        {
            SheetInspector clone = new SheetInspector();
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

        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new SheetResult();
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            throw new System.NotImplementedException();
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            SheetResult sheetResult = (SheetResult)CreateAlgorithmResult();
            
            SheetInspectorParam param = (SheetInspectorParam)this.Param;

            SheetInspectParam sheetAlgorithmInspectParam = (SheetInspectParam)algorithmInspectParam;
            ProcessBufferSetS bufferSet = (ProcessBufferSetS)(sheetAlgorithmInspectParam).ProcessBufferSet;

            if (algorithmInspectParam.ClipImage == null )
            {
                LogHelper.Debug(LoggerType.Operation, "SheetInspector == null");

                sheetResult.SheetErrorType = SheetErrorType.Error;
                
                stopwatch.Stop();
                sheetResult.SpandTime = stopwatch.Elapsed;
                sheetResult.Good = true;
                return sheetResult;
            }

            LogHelper.Debug(LoggerType.Operation, "SheetInspector - 1");
            AlgoImage sourceImage = ImageBuilder.Build(GetAlgorithmType(), algorithmInspectParam.ClipImage, ImageType.Grey, ImageBandType.Luminance);

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(sourceImage);

            Rectangle interestRect = new Rectangle(0, 0, sourceImage.Width, param.EmptyRegionPos);
            int width = sourceImage.Width;
            int height = sourceImage.Height;

            imageProcessing.Resize(sourceImage, bufferSet.InterestP, SystemTypeSettings.Instance().ResizeRatio, SystemTypeSettings.Instance().ResizeRatio);
            sheetResult.PrevImage = bufferSet.InterestP.ToImageD().ToBitmap();

            LogHelper.Debug(LoggerType.Operation, "SheetInspector - 2");
            
            //int emptyPos = AlgorithmCommon.Instance().GetEmptyRegionPos(sourceImage);
            //if (param.EmptyRegionPos * 0.9 > emptyPos || param.EmptyRegionPos * 1.1 < emptyPos)
            //{
            //    LogHelper.Debug(LoggerType.Operation, "SheetInspector - EmptyRegionPos");

            //    sheetResult.SheetErrorType = SheetErrorType.Error;

            //    AlgorithmCommon.Instance().AddDisposeList(sourceImage);

            //    stopwatch.Stop();
            //    sheetResult.SpandTime = stopwatch.Elapsed;
            //    return sheetResult;
            //}

            if (sheetAlgorithmInspectParam.FidResult == false)
            {
                sheetResult.SheetErrorType = SheetErrorType.FiducialNG;
                AlgorithmCommon.Instance().AddDisposeList(sourceImage);

                stopwatch.Stop();
                sheetResult.SpandTime = stopwatch.Elapsed;
                sheetResult.Good = true;
                return sheetResult;
            }

            List<SheetSubResult> totalSheetAttackList = new List<SheetSubResult>();
            List<SheetSubResult> totalPoleList = new List<SheetSubResult>();
            List<SheetSubResult> totalDielectricList = new List<SheetSubResult>();
            List<SheetSubResult> totalPinHoleList = new List<SheetSubResult>();
            List<ShapeResult> totalShapeList = new List<ShapeResult>();

            List<BlobRect> totalNeedInspectBlobList = new List<BlobRect>();

            List<SheetErrorType> regionResultList = new List<SheetErrorType>();

            int fidOffsetWidth = (int)Math.Round(sheetAlgorithmInspectParam.FidOffset.Width);
            int fidOffsetHeight = (int)Math.Round(sheetAlgorithmInspectParam.FidOffset.Height);

            //Rectangle emptyPosRect = new Rectangle(interestRect.X, interestRect.Y, interestRect.Width, param.EmptyRegionPos - interestRect.Y);
            
            foreach (RegionInfo regionInfo in param.RegionInfoList)
            {
                LogHelper.Debug(LoggerType.Operation, "SheetInspector - 3-1");

                Rectangle region = regionInfo.Region;

                region.Offset(fidOffsetWidth, fidOffsetHeight);
                region.Intersect(interestRect);
                if (region.Width == 0 || region.Height == 0)
                    continue;

                ///////////////////////////////
                // SubImage
                ///////////////////////////////
                AlgoImage interestRegion = sourceImage.GetSubImage(region);

                AlgoImage interestBinRegion = bufferSet.InterestBin.GetSubImage(region);
                AlgoImage maskRegion = bufferSet.Mask.GetSubImage(region);

                AlgoImage poleInspectRegion = bufferSet.PoleInspect.GetSubImage(region);
                AlgoImage poleMaskRegion = bufferSet.PoleMask.GetSubImage(region);

                AlgoImage dielectricInspectRegion = bufferSet.DielectricInspect.GetSubImage(region);
                AlgoImage dielectricMaskRegion = bufferSet.DielectricMask.GetSubImage(region);
                ///////////////////////////////
                
                imageProcessing.Binarize(interestRegion, interestBinRegion, regionInfo.MeanValue, true);
                
                if (AlgorithmCommon.Instance().CreateMaskImage(interestBinRegion, maskRegion, param.ShapeParam.MinPatternArea, true) == false)
                {
                    regionResultList.Add(SheetErrorType.InvalidInspect);

                    AlgorithmCommon.Instance().AddDisposeList(interestBinRegion);
                    AlgorithmCommon.Instance().AddDisposeList(maskRegion);
                    AlgorithmCommon.Instance().AddDisposeList(interestRegion);
                    AlgorithmCommon.Instance().AddDisposeList(poleInspectRegion);
                    AlgorithmCommon.Instance().AddDisposeList(poleMaskRegion);
                    AlgorithmCommon.Instance().AddDisposeList(dielectricInspectRegion);
                    AlgorithmCommon.Instance().AddDisposeList(dielectricMaskRegion);

                    continue;
                }

                //Shape
                List<BlobRect> notNecessaryList = new List<BlobRect>();
                List<ShapeResult> shapeList = new List<ShapeResult>();
                List<BlobRect> needInspectBlobList = new List<BlobRect>();
                shapeInspector.Inspect(interestRegion, maskRegion, regionInfo, region, ref shapeList, ref notNecessaryList, ref needInspectBlobList);

                //Prepare
                imageProcessing.Not(maskRegion, dielectricMaskRegion);

                imageProcessing.Erode(maskRegion, poleMaskRegion, InspectorSetting.Instance().RemovalNum);
                imageProcessing.Erode(dielectricMaskRegion, dielectricMaskRegion, InspectorSetting.Instance().RemovalNum);

                LogHelper.Debug(LoggerType.Operation, "SheetInspector - 3-2");

                List<Task> taskList = new List<Task>();

                //Pole Task
                List<SheetSubResult> sheetAttackList = new List<SheetSubResult>();
                List<SheetSubResult> poleList = new List<SheetSubResult>();
                bool poleReached = false;
                taskList.Add(Task.Run(() => poleInspector.Inspect(interestRegion, poleInspectRegion, poleMaskRegion, regionInfo, region, notNecessaryList, ref sheetAttackList, ref poleList, ref poleReached)));

                //Dielectric Task
                List<SheetSubResult> dielectricList = new List<SheetSubResult>();
                List<SheetSubResult> pinHoleList = new List<SheetSubResult>();
                bool dielectricReached = false;
                taskList.Add(Task.Run(() => dielectricInspector.Inspect(interestRegion, dielectricInspectRegion, dielectricMaskRegion, regionInfo, region, ref dielectricList, ref pinHoleList, ref dielectricReached)));
                totalShapeList.AddRange(shapeList);
                totalNeedInspectBlobList.AddRange(needInspectBlobList);

                //검사중에 해제해도 됨
                AlgorithmCommon.Instance().AddDisposeList(interestBinRegion);
                AlgorithmCommon.Instance().AddDisposeList(maskRegion);
                Task.WaitAll(taskList.ToArray());
                
                if (poleReached)
                {
                    regionResultList.Add(SheetErrorType.InvalidPoleParam);
                }
                else if (dielectricReached)
                {
                    regionResultList.Add(SheetErrorType.InvalidDielectricParam);
                }
                else
                {
                    totalSheetAttackList.AddRange(sheetAttackList);
                    totalPoleList.AddRange(poleList);
                    totalDielectricList.AddRange(dielectricList);
                    totalPinHoleList.AddRange(pinHoleList);
                }

                LogHelper.Debug(LoggerType.Operation, "SheetInspector - 3-3");
                //검사 끝나고 해제
                
                AlgorithmCommon.Instance().AddDisposeList(interestRegion);
                AlgorithmCommon.Instance().AddDisposeList(poleInspectRegion);
                AlgorithmCommon.Instance().AddDisposeList(poleMaskRegion);
                AlgorithmCommon.Instance().AddDisposeList(dielectricInspectRegion);
                AlgorithmCommon.Instance().AddDisposeList(dielectricMaskRegion);

                LogHelper.Debug(LoggerType.Operation, "SheetInspector - 3-4");
            }

            int refPatternNum = 0;
            param.ShapeParam.PatternList.ForEach(p => refPatternNum += p.NeedInspect == true ? p.PatternGroup.NumPattern : 0);

            bool exception = false;
            if (refPatternNum * 0.1 > totalNeedInspectBlobList.Count)
            {
                sheetResult.SheetErrorType = SheetErrorType.DifferenceModel;
            }
            else if (regionResultList.Count != 0 && regionResultList.FindAll(result => result == SheetErrorType.None).Count == 0)
            {
                int invalidInspectCount = regionResultList.FindAll(result => result == SheetErrorType.InvalidInspect).Count;
                int invalidPoleCount = regionResultList.FindAll(result => result == SheetErrorType.InvalidPoleParam).Count;
                int invalidDielectricCount = regionResultList.FindAll(result => result == SheetErrorType.InvalidDielectricParam).Count;

                if (invalidPoleCount != 0)
                    sheetResult.SheetErrorType = SheetErrorType.InvalidPoleParam;
                else if (invalidDielectricCount != 0)
                    sheetResult.SheetErrorType = SheetErrorType.InvalidDielectricParam;
                else if (invalidInspectCount > param.RegionInfoList.Count / 2)
                    sheetResult.SheetErrorType = SheetErrorType.InvalidInspect;
            }

            LogHelper.Debug(LoggerType.Operation, "SheetInspector - 3-5");

            if (sheetResult.SheetErrorType != SheetErrorType.None)
                exception = true;

            if (exception == true)
            {
                AlgorithmCommon.Instance().AddDisposeList(sourceImage);

                stopwatch.Stop();
                sheetResult.SpandTime = stopwatch.Elapsed;
                sheetResult.Good = true;

                return sheetResult;
            }

            LogHelper.Debug(LoggerType.Operation, "SheetInspector - 3-6");

            int defectCount = totalSheetAttackList.Count + totalPoleList.Count
                + totalDielectricList.Count + totalPinHoleList.Count + totalShapeList.Count;
            
            if (defectCount == 0)
            {
                sheetResult.Good = true;
            }
            else
            {
                List<SheetSubResult> tempList = new List<SheetSubResult>();

                tempList.AddRange(totalSheetAttackList);
                tempList.AddRange(totalPoleList);
                tempList.AddRange(totalDielectricList);
                tempList.AddRange(totalPinHoleList);
                tempList.AddRange(totalShapeList);

                tempList = tempList.OrderByDescending(sheetSubResult => sheetSubResult.Area).ToList();

                for (int srcIndex = 0; srcIndex < tempList.Count; srcIndex++)
                {
                    RectangleF region = tempList[srcIndex].Region;
                    region.Inflate(AlgorithmSetting.Instance().DefectDistance, AlgorithmSetting.Instance().DefectDistance);

                    for (int destIndex = srcIndex + 1; destIndex < tempList.Count; destIndex++)
                    {
                        if (region.IntersectsWith(tempList[destIndex].Region) == true)
                        {
                            tempList.RemoveAt(destIndex);
                            destIndex--;
                        }
                    }
                }

                totalSheetAttackList.Clear();
                totalPoleList.Clear();
                totalDielectricList.Clear();
                totalPinHoleList.Clear();
                totalShapeList.Clear();

                for (int i = 0; i < Math.Min(tempList.Count, AlgorithmSetting.Instance().MaxDefectNum); i++)
                {
                    switch (tempList[i].DefectType)
                    {
                        case DefectType.SheetAttack:
                            totalSheetAttackList.Add(tempList[i]);
                            break;
                        case DefectType.Pole:
                            totalPoleList.Add(tempList[i]);
                            break;
                        case DefectType.Dielectric:
                            totalDielectricList.Add(tempList[i]);
                            break;
                        case DefectType.PinHole:
                            totalPinHoleList.Add(tempList[i]);
                            break;
                        case DefectType.Shape:
                            totalShapeList.Add((ShapeResult)tempList[i]);
                            break;
                    }
                }

                AlgorithmCommon.Instance().RefineSubResult(sourceImage, param.EmptyRegionPos, bufferSet.PoleInspect, ref totalSheetAttackList);
                AlgorithmCommon.Instance().RefineSubResult(sourceImage, param.EmptyRegionPos, bufferSet.PoleInspect, ref totalPoleList);
                AlgorithmCommon.Instance().RefineSubResult(sourceImage, param.EmptyRegionPos, bufferSet.DielectricInspect, ref totalDielectricList);
                AlgorithmCommon.Instance().RefineSubResult(sourceImage, param.EmptyRegionPos, bufferSet.DielectricInspect, ref totalPinHoleList);

                List<SheetSubResult> convertShapeList = totalShapeList.ConvertAll(x => (SheetSubResult)x);
                AlgorithmCommon.Instance().RefineSubResult(sourceImage, param.EmptyRegionPos, null, ref convertShapeList);
                totalShapeList = convertShapeList.ConvertAll(x => (ShapeResult)x);

                sheetResult.AddSheetSubResult(totalSheetAttackList, totalPoleList, totalDielectricList, totalPinHoleList, totalShapeList);
            }

            LogHelper.Debug(LoggerType.Operation, "SheetInspector - 3-7");
            
            AlgorithmCommon.Instance().AddDisposeList(sourceImage);

            stopwatch.Stop();
            sheetResult.SpandTime = stopwatch.Elapsed;

            return sheetResult;
        }
    }
}
