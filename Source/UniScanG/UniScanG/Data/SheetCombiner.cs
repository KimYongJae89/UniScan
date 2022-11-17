using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Common;
using UniScanG.Common.Data;
using UniScanG.Common.Settings;

namespace UniScanG.Data
{
    public static class SheetCombiner
    {
        public static ResultCollector resultCollector = null;

        public static string TypeName { get { return "SheetCombiner"; } }

        public static void SetCollector(ResultCollector collector)
        {
            resultCollector = collector;
        }

        public static Image2D CreatePrevImage(Image2D image2D)
        {
            float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;
            resizeRatio = 0.1005386f;
            return (Image2D)image2D?.Resize(resizeRatio);
        }

        public static AlgorithmResultG CombineResult(Tuple<string, string> foundedT)
        {
            if (SystemManager.Instance().ExchangeOperator is IServerExchangeOperator == false)
                return null;

            int sheetNo = int.Parse(foundedT.Item1);
            float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;
            Production curProduction = SystemManager.Instance().ProductionManager.CurProduction as Production;

            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorList = server.GetInspectorList(sheetNo);

            TimeSpan maxInspectTimeSpan = TimeSpan.Zero;

            MergeSheetResult[] sheetResults = new MergeSheetResult[inspectorList.Count];

            //LogHelper.Debug(LoggerType.Inspection, string.Format("SheetCombiner::CombineResult, {0}", foundedT.Item1));
            Parallel.For(0, inspectorList.Count, i =>
            {
                UniScanG.Data.Model.ModelDescription md = SystemManager.Instance().CurrentModel.ModelDescription;
                string resultPath = Path.Combine(inspectorList[i].Info.Path, "Result", foundedT.Item2, foundedT.Item1);
                sheetResults[i] = (MergeSheetResult)resultCollector.Collect(resultPath);

                AlgorithmResultG sheetResult = sheetResults[i];
                //LogHelper.Debug(LoggerType.Inspection, $"SheetCombiner::CombineResult - {sheetResult.SheetSize}");
                sheetResult.UpdateSubResultImage();
            });

            // 합치기
            RectangleF monitorFov = SystemTypeSettings.Instance().MonitorFov;
            Size mergeImageSize = Size.Round(new SizeF(SystemTypeSettings.Instance().MonitorFov.Width * resizeRatio, SystemTypeSettings.Instance().MonitorFov.Height * resizeRatio));
            bool zeroSize = mergeImageSize.IsEmpty;
            if (zeroSize)
            {
                monitorFov = new RectangleF(PointF.Empty, new SizeF(sheetResults.Sum(f => f.SheetSizePx.Width), sheetResults.Max(f => f.SheetSizePx.Height)));
                mergeImageSize = new Size(sheetResults.Sum(f => f.PrevImage.Width), sheetResults.Max(f => f.PrevImage.Height));
            }

            Size mergeSheetSizePx = Size.Empty;
            SizeF mergeSheetSize = SizeF.Empty;
            Image2D image = new Image2D(mergeImageSize.Width, mergeImageSize.Height, 1);
            for (int i = 0; i < sheetResults.Length; i++)
            {
                AlgorithmResultG sheetResult = sheetResults[i];

                // Cam 번호에 따른 FOV 오프셋 반영
                SizeF pelSize = new SizeF(
                    sheetResults[i].SheetSize.Width / sheetResults[i].SheetSizePx.Width * 1000,
                    sheetResults[i].SheetSize.Height / sheetResults[i].SheetSizePx.Height * 1000);
                //LogHelper.Debug(LoggerType.Inspection, $"SheetCombiner::CombineResult - PelSize of [{i}]: {pelSize}");
                int offset = 0;
                if (inspectorList[i].Info.Fov.IsEmpty)
                {
                    for (int offsetIndex = 0; offsetIndex < i; offsetIndex++)
                        offset += (int)sheetResults[offsetIndex].SheetSizePx.Width;
                    mergeSheetSizePx.Width += sheetResults[i].SheetSizePx.Width;
                    mergeSheetSize.Width += sheetResults[i].SheetSize.Width;
                }
                else
                {
                    for (int j = 0; j < i; j++)
                        offset += (int)inspectorList[j].Info.Fov.Width;
                    mergeSheetSizePx.Width += (int)inspectorList[i].Info.Fov.Width;
                    //LogHelper.Debug(LoggerType.Inspection, $"SheetCombiner::CombineResult - Fov of [{i}]: {inspectorList[i].Info.Fov}");
                    mergeSheetSize.Width += (inspectorList[i].Info.Fov.Width * pelSize.Width / 1000);
                }

                if (mergeSheetSizePx.Width == 0)
                    mergeSheetSizePx.Width = sheetResults.Sum(f => f.SheetSizePx.Width);

                if (mergeSheetSize.Width == 0)
                    mergeSheetSize.Width = sheetResults.Sum(f => f.SheetSize.Width);

                if (false)
                // IM 값 중 최대값 사용
                {
                    mergeSheetSizePx.Height = Math.Max(mergeSheetSizePx.Height, sheetResults[i].SheetSizePx.Height);
                    mergeSheetSize.Height = Math.Max(mergeSheetSize.Height, sheetResults[i].SheetSize.Height);
                }
                else
                // 마지막 IM 값만 사용
                {
                    mergeSheetSizePx.Height = sheetResults[i].SheetSizePx.Height;
                    mergeSheetSize.Height = sheetResults[i].SheetSize.Height;
                }

                sheetResult.Offset(offset - (int)inspectorList[i].Info.Fov.X, 0, pelSize);

                // 전체 이미지 합성
                if (sheetResult.PrevImage != null)
                {
                    Rectangle srcRect = Rectangle.Empty;
                    if (inspectorList[i].Info.Fov.IsEmpty)
                    {
                        srcRect = new Rectangle(0, 0, sheetResult.PrevImage.Width, sheetResult.PrevImage.Height);
                    }
                    else
                    {
                        int resizeFovX = (int)(inspectorList[i].Info.Fov.X * resizeRatio);
                        int resizeFovY = (int)(inspectorList[i].Info.Fov.Y * resizeRatio);
                        int resizeFovWidth = (int)(inspectorList[i].Info.Fov.Width * resizeRatio);
                        int resizeFovHeight = (int)(inspectorList[i].Info.Fov.Height * resizeRatio);
                        srcRect = new Rectangle(resizeFovX, resizeFovY, resizeFovWidth, resizeFovHeight);
                    }

                    Size fidOffset = Size.Round(sheetResult.OffsetFound);
                    //srcRect.Offset(fidOffset.Width, fidOffset.Height);
                    srcRect.Intersect(new Rectangle(Point.Empty, sheetResult.PrevImage.Size));

                    if (srcRect.Width > 0 && srcRect.Height > 0)
                    {
                        Image2D orgImage2D = Image2D.FromBitmap(sheetResult.PrevImage);
                        lock (image)
                        {
                            Rectangle dstRect = new Rectangle(new Point((int)(offset * resizeRatio), 0), srcRect.Size);
                            dstRect.Intersect(new Rectangle(Point.Empty, mergeImageSize));
                            srcRect.Size = dstRect.Size;
                            image.CopyFrom(orgImage2D, srcRect, orgImage2D.Pitch, dstRect.Location);
                        }
                    }
                }
                else
                {
                    //LogHelper.Error(LoggerType.Error, string.Format("SheetCombiner::CombineResult - Inspector {0} prevImage is null", inspectorList[i].Info.GetName()));
                }
            }

            // 불량 리스트 합치기
            MergeSheetResult mergeSheetResult = (MergeSheetResult)resultCollector.CreateSheetResult(sheetNo, sheetResults.Length, Path.Combine(curProduction.ResultPath, sheetNo.ToString()));
            mergeSheetResult.Union(sheetResults);
            mergeSheetResult.SheetSizePx = mergeSheetSizePx;
            mergeSheetResult.SheetSize = mergeSheetSize;

            //LogHelper.Debug(LoggerType.Inspection, $"SheetCombiner::CombineResult - mergeSheetResult.SheetSizePx: {mergeSheetResult.SheetSizePx}");
            SizeF ratio = DrawingHelper.Div(monitorFov.Size, mergeSheetResult.SheetSizePx);
            //mergeSheetResult.SheetSizePx = Size.Round(DrawingHelper.Mul(mergeSheetResult.SheetSizePx, ratio));
            //mergeSheetResult.SheetSize = DrawingHelper.Mul(mergeSheetResult.SheetSize, ratio);

            if (image != null && image.Size.Width > 0 && image.Size.Height > 0)
                mergeSheetResult.PrevImage = image.ToBitmap();

            return mergeSheetResult;
        }

        public static Bitmap CreateModelImage(UniScanG.Common.Data.ModelDescription modelDescription)
        {
            LogHelper.Debug(LoggerType.Operation, "SheetCombiner::CreateModelImage Start");
            if (SystemManager.Instance().ExchangeOperator is IServerExchangeOperator == false)
                return null;

            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> masterInspectorList = server.GetInspectorList(-1).FindAll(f => f.Info.ClientIndex <= 0);

            float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;

            try
            {
                ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = 1 };
#if DEBUG
                parallelOptions.MaxDegreeOfParallelism = 1;
#endif
                SortedList<int, Tuple<Rectangle, Image2D>> tupleList = new SortedList<int, Tuple<Rectangle, Image2D>>();
                Parallel.For(0, masterInspectorList.Count, parallelOptions, i =>
                {
                    InspectorObj inspectorObj = masterInspectorList[i];
                    Bitmap prevImage = inspectorObj.GetPreviewImage(modelDescription);

                    // 크기가 지정되면 지정된 크기 사용
                    Rectangle srcRect = Rectangle.Round(inspectorObj.Info.Fov);
                    srcRect = DrawingHelper.Mul(srcRect, resizeRatio);

                    if (srcRect.IsEmpty)
                    // 크기가 지정되지 않으면
                    {
                        if (prevImage != null)
                        // 이미지 크기 사용
                        {
                            srcRect = new Rectangle(Point.Empty, prevImage.Size);
                        }
                        else
                        // 둘 다 없으면..??
                        {

                        }
                    }

                    Image2D image2D;
                    if (prevImage != null)
                    {
                        if (prevImage.PixelFormat != System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                            image2D = Image2D.FromBitmap(ImageHelper.MakeGrayscale(prevImage));
                        else
                            image2D = Image2D.FromBitmap(prevImage);
                        tupleList.Add(i, new Tuple<Rectangle, Image2D>(srcRect, image2D));
                    }
                });

                Size mergeSize = Size.Round(DrawingHelper.Mul(SystemTypeSettings.Instance().MonitorFov.Size, resizeRatio));
                if (mergeSize.IsEmpty)
                    mergeSize = new Size(tupleList.Sum(f => f.Value.Item1.Width), tupleList.Max(f => f.Value.Item1.Height));

                Image2D image = new Image2D(mergeSize.Width, mergeSize.Height, 1);
                Rectangle fullImageRect = new Rectangle(Point.Empty, image.Size);
                Rectangle dstRectangle = Rectangle.Empty;
                for (int i = 0; i < tupleList.Count; i++)
                {
                    Rectangle srcRectangle = tupleList[i].Item1;
                    Image2D srcImage = tupleList[i].Item2;

                    if (srcImage != null)
                    //LogHelper.Debug(LoggerType.Operation, string.Format("SheetCombiner::CreateModelImage - SrcImage[{0}] is NULL", i));
                    {
                        srcRectangle.Intersect(new Rectangle(Point.Empty, srcImage.Size));
                        dstRectangle.Size = srcRectangle.Size;
                        dstRectangle.Intersect(fullImageRect);
                        srcRectangle.Size = dstRectangle.Size;

                        if (dstRectangle.Width > 0 && dstRectangle.Height > 0)
                        {
                            lock (image)
                                image.CopyFrom(srcImage, srcRectangle, srcImage.Pitch, dstRectangle.Location);
                        }
                    }
                    dstRectangle.Location = new Point(dstRectangle.Right, 0);
                }

                LogHelper.Debug(LoggerType.Operation, "SheetCombiner::CreateModelImage End");
                return image.ToBitmap();
            }
            catch (Exception ex)
            {
                while (ex != null)
                {
                    LogHelper.Debug(LoggerType.Error, string.Format("SheetCombiner::CreateModelImage - {0}", ex.Message));
                    ex = ex.InnerException;
                }
                return null;
            }
        }
    }
}
