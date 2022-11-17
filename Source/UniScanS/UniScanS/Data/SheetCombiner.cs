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
using UniScanS.Common;
using UniScanS.Common.Data;
using UniScanS.Common.Settings;
using UniScanS.Screen.Data;
using UniScanS.Screen.Vision.Detector;

namespace UniScanS.Data
{
    public static class SheetCombiner
    {
        public static Bitmap CreatePrevImage(Bitmap bitmap)
        {
            float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;
            return ImageHelper.Resize(bitmap, resizeRatio, resizeRatio);
        }

        public static void CombineImage(MergeSheetResult sheetResult)
        {
            string[] splitString = sheetResult.ResultPath.Split("Result".ToArray());
            string lastString = splitString.Last();
            lastString = lastString.Trim('\\');

            float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;
            int mergeWidth = (int)(SystemTypeSettings.Instance().MonitorFov.Width * resizeRatio);
            int mergeHeight = (int)(SystemTypeSettings.Instance().MonitorFov.Height * resizeRatio);
            Image2D image = new Image2D(mergeWidth, mergeHeight, 1);

            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorList = server.GetInspectorList();

            object lockObject = new object();

            Parallel.For(0, inspectorList.Count, i =>
            {
                string imagePath = Path.Combine(inspectorList[i].Info.Path, "Result", lastString, "Prev.jpg");

                if (File.Exists(imagePath) == false)
                    return;

                Bitmap bitmap = (Bitmap)ImageHelper.LoadImage(imagePath);

                if (bitmap == null)
                    return;

                int offset = 0;
                for (int offsetIndex = 0; offsetIndex < i; offsetIndex++)
                    offset += (int)inspectorList[offsetIndex].Info.Fov.Width;
                
                int resizeFovX = (int)(inspectorList[i].Info.Fov.X * resizeRatio);
                int resizeFovY = (int)(inspectorList[i].Info.Fov.Y * resizeRatio);
                int resizeFovWidth = (int)(inspectorList[i].Info.Fov.Width * resizeRatio);
                int resizeFovHeight = (int)(inspectorList[i].Info.Fov.Height * resizeRatio);

                if (resizeFovWidth == 0 || resizeFovHeight == 0)
                    return;

                Rectangle srcRect = new Rectangle(resizeFovX, resizeFovY, resizeFovWidth, resizeFovHeight);
                Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                srcRect.Intersect(rect);
                if (srcRect.Width == 0 || srcRect.Height == 0)
                    return;

                image.CopyFrom(Image2D.FromBitmap(bitmap), srcRect, bitmap.Width, new Point((int)(offset * resizeRatio), 0));
            });

            sheetResult.PrevImage = image.ToBitmap();
        }

        public static SheetResult CombineResult(Tuple<string, string> foundedT)
        {
            SheetResult mergeSheetResult = new SheetResult();

            if (SystemManager.Instance().ExchangeOperator is IServerExchangeOperator == false)
                return null;

            float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;
            int mergeWidth = (int)(SystemTypeSettings.Instance().MonitorFov.Width * resizeRatio);
            int mergeHeight = (int)(SystemTypeSettings.Instance().MonitorFov.Height * resizeRatio);
            Image2D image = new Image2D(mergeWidth, mergeHeight, 1);

            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorList = server.GetInspectorList();

            object lockObject = new object();

            Parallel.For(0, inspectorList.Count, i =>
            {
                UniScanS.Data.Model.ModelDescriptionS md = SystemManager.Instance().CurrentModel.ModelDescription;
                string resultPath = Path.Combine(inspectorList[i].Info.Path, "Result", foundedT.Item2, md.Name, md.Thickness.ToString(), md.Paste, SystemManager.Instance().ProductionManager.CurProduction.LotNo, foundedT.Item1);

                SheetResult sheetResult = new SheetResult();
                sheetResult.Import(SheetInspector.TypeName, resultPath);

                int offset = 0;
                for (int offsetIndex = 0; offsetIndex < i; offsetIndex++)
                    offset += (int)inspectorList[offsetIndex].Info.Fov.Width;

                sheetResult.Offset(offset - (int)inspectorList[i].Info.Fov.X, -(int)inspectorList[i].Info.Fov.Y);

                if (sheetResult.PrevImage == null)
                    return; ;

                int resizeFovX = (int)(inspectorList[i].Info.Fov.X * resizeRatio);
                int resizeFovY = (int)(inspectorList[i].Info.Fov.Y * resizeRatio);
                int resizeFovWidth = (int)(inspectorList[i].Info.Fov.Width * resizeRatio);
                int resizeFovHeight = (int)(inspectorList[i].Info.Fov.Height * resizeRatio);

                if (resizeFovWidth == 0 || resizeFovHeight == 0)
                    return;

                Rectangle srcRect = new Rectangle(resizeFovX, resizeFovY, resizeFovWidth, resizeFovHeight);
                Rectangle rect = new Rectangle(0, 0, sheetResult.PrevImage.Width, sheetResult.PrevImage.Height);
                srcRect.Intersect(rect);
                if (srcRect.Width == 0 || srcRect.Height == 0)
                    return;

                image.CopyFrom(Image2D.FromBitmap(sheetResult.PrevImage), srcRect, sheetResult.PrevImage.Width, new Point((int)(offset * resizeRatio), 0));

                lock (lockObject)
                    mergeSheetResult += sheetResult;
            });

            if (mergeSheetResult.SubResultList.Count == 0)
                mergeSheetResult.Good = true;

            if (image.Size.Width == 0 || image.Size.Height == 0)
                mergeSheetResult.PrevImage = null;
            else
                mergeSheetResult.PrevImage = image.ToBitmap();

            mergeSheetResult.RemoveAllIntersectResult();

            return mergeSheetResult;
        }

        public static Bitmap CreateModelImage(UniScanS.Common.Data.ModelDescription modelDescription)
        {
            if (SystemManager.Instance().ExchangeOperator is IServerExchangeOperator == false)
                return null;

            IServerExchangeOperator server = (IServerExchangeOperator)SystemManager.Instance().ExchangeOperator;
            List<InspectorObj> inspectorList = server.GetInspectorList();

            float resizeRatio = SystemTypeSettings.Instance().ResizeRatio;
            RectangleF monitorFov = SystemTypeSettings.Instance().MonitorFov;
            
            int mergeWidth = (int)(monitorFov.Width * resizeRatio);
            int mergeHeight = (int)(monitorFov.Height * resizeRatio);
            if (mergeWidth == 0 || mergeHeight == 0)
                return null;
            Image2D image = new Image2D(mergeWidth, mergeHeight, 1);
            
            object lockObject = new object();

            Parallel.For(0, inspectorList.Count, i =>
            {
                if (inspectorList[i].IsTrained(modelDescription) == false)
                    return;

                Bitmap prevImage = inspectorList[i].GetPreviewImage(modelDescription);
                if (prevImage == null)
                    return;
                
                int offset = 0;
                for (int offsetIndex = 0; offsetIndex < i; offsetIndex++)
                    offset += (int)inspectorList[offsetIndex].Info.Fov.Width;
                
                int resizeFovX = (int)(inspectorList[i].Info.Fov.X * resizeRatio);
                int resizeFovY = (int)(inspectorList[i].Info.Fov.Y * resizeRatio);
                int resizeFovWidth = (int)(inspectorList[i].Info.Fov.Width * resizeRatio);
                int resizeFovHeight = (int)(inspectorList[i].Info.Fov.Height * resizeRatio);

                if (resizeFovWidth == 0 || resizeFovHeight == 0)
                    return;

                Rectangle srcRect = new Rectangle(resizeFovX, resizeFovY, resizeFovWidth, resizeFovHeight);
                Rectangle rect = new Rectangle(0, 0, prevImage.Width, prevImage.Height);
                srcRect.Intersect(rect);
                if (srcRect.Width == 0 || srcRect.Height == 0)
                    return;

                image.CopyFrom(Image2D.FromBitmap(prevImage), srcRect, prevImage.Width, new Point((int)(offset * resizeRatio), 0));
            });

            return image.ToBitmap();
        }
    }
}
