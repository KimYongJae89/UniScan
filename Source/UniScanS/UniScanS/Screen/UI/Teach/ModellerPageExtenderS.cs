using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.UI;
using DynMvp.UI.Touch;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniEye.Base.UI;
using UniScanS.Common;
using UniScanS.Data;
using UniScanS.Data.Model;
using UniScanS.Screen.Data;
using UniScanS.Screen.Vision;
using UniScanS.Screen.Vision.Detector;
using UniScanS.Screen.Vision.Trainer;
using UniScanS.UI.Etc;
using UniScanS.UI.Teach;
using UniScanS.Vision;
using UniScanS.Vision.FiducialFinder;

namespace UniScanS.Screen.UI.Teach
{
    public class ModellerPageExtenderS : UniScanS.UI.Teach.ModellerPageExtender
    {
        public ModellerPageExtenderS()
        {
            // Base 생성자에서 추가됨
            //SystemManager.Instance().ExchangeOperator.AddModelListener(this);
        }

        public override string GetModelImageName()
        {
            return string.Format("Image.{0}", ImageFormat.Bmp.ToString());
        }

        public override void Grab()
        {
            System.Threading.CancellationTokenSource token = new System.Threading.CancellationTokenSource();

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            foreach (ImageDevice device in imageDeviceHandler)
            {
                if (device is Camera)
                    ((Camera)device).SetupGrab();
            }

            imageDeviceHandler.AddImageGrabbed(ImageGrabbed);
            imageDeviceHandler.GrabOnce();

            
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() => imageDeviceHandler.WaitGrabDone(30000), token);
            
            imageDeviceHandler.Stop();
            imageDeviceHandler.RemoveImageGrabbed(ImageGrabbed);
        }

        public override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            if (currentImage != null)
                currentImage.Dispose();

            currentImage = (Image2D)imageDevice.GetGrabbedImage(ptr).Clone();
            currentImage.ConvertFromPtr();

            if (UpdateImage != null)
                UpdateImage(currentImage, false);

            string imagePath = SystemManager.Instance().CurrentModel.GetImagePath();
            if (Directory.Exists(imagePath) == false)
                Directory.CreateDirectory(imagePath);

            Bitmap bitmap = currentImage.ToBitmap();
            ImageHelper.SaveImage(bitmap, Path.Combine(imagePath, GetModelImageName()), ImageFormat.Bmp);
            bitmap.Dispose();
        }

        protected override void TeachBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SheetTrainer trainer = (SheetTrainer)AlgorithmPool.Instance().GetAlgorithm(SheetTrainer.TypeName);

            if (trainer == null)
                return;

            trainer.Teach((BackgroundWorker)sender, currentImage, e);
        }

        protected override void TeachRunWorkerCompleted(bool result)
        {
            if (result == true)
            {
                if (SystemManager.Instance().CurrentModel.IsTrained == false)
                {
                    SheetInspector sheetInspector = (SheetInspector)AlgorithmPool.Instance().GetAlgorithm(SheetInspector.TypeName);
                    SheetInspectorParam sheetInspectorParam = (SheetInspectorParam)sheetInspector.Param;

                    SheetTrainer sheetTrainer = (SheetTrainer)AlgorithmPool.Instance().GetAlgorithm(SheetTrainer.TypeName);
                    SheetTrainerParam sheetTrainerParam = (SheetTrainerParam)sheetTrainer.Param;
                    sheetInspectorParam.PoleParam.LowerThreshold = sheetTrainerParam.PoleRecommendLowerTh;
                    sheetInspectorParam.PoleParam.UpperThreshold = sheetTrainerParam.PoleRecommendUpperTh;
                    sheetInspectorParam.DielectricParam.LowerThreshold = sheetTrainerParam.DielectricRecommendLowerTh;
                    sheetInspectorParam.DielectricParam.UpperThreshold = sheetTrainerParam.DielectricRecommendUpperTh;
                }

                if (currentImage != null)
                {
                    Bitmap prevImage = SheetCombiner.CreatePrevImage(currentImage.ToBitmap());
                    
                    string imagePath = SystemManager.Instance().CurrentModel.GetImagePath();

                    if (Directory.Exists(imagePath) == false)
                        Directory.CreateDirectory(imagePath);

                    string fileName = string.Format("Prev.{0}", ImageFormat.Bmp.ToString());
                    string filePath = Path.Combine(imagePath, fileName);
                    
                    ImageHelper.SaveImage(prevImage, filePath, ImageFormat.Bmp);
                }
                    
                SystemManager.Instance().CurrentModel.IsTrained = true;
            }

            SaveModel();
            SystemManager.Instance().ExchangeOperator.ModelTeachDone();
        }

        //public void SelectedPattern(SheetPattern pattern)
        //{
        //    if (UpdatePatternFigure != null)
        //        UpdatePatternFigure(pattern);
        //}

        //public void SelectedFiducialPattern(FiducialPattern pattern)
        //{
        //    if (UpdateFiducialPatternFigure != null)
        //        UpdateFiducialPatternFigure(pattern);
        //}

        //public void SelectedRegionInfo(RegionInfo regionInfo)
        //{
        //    if (UpdateRegionInfo != null)
        //        UpdateRegionInfo(regionInfo);
        //}

        public override void Inspect()
        {
            if (currentImage == null)
                return;

            SizeF offset = new SizeF();
            ProcessBufferSetS bufferSet = new ProcessBufferSetS(SheetInspector.TypeName, currentImage.Width, currentImage.Height);
            
            FiducialFinderAlgorithmResult finderResult = new FiducialFinderAlgorithmResult();
            if (InspectorSetting.Instance().IsFiducial == true)
            {
                SimpleProgressForm fiducialForm = new SimpleProgressForm("Find Fiducial");
                fiducialForm.Show(new Action(() =>
                {
                    FiducialFinder fiducialFinder = (FiducialFinder)AlgorithmPool.Instance().GetAlgorithm(FiducialFinder.TypeName);
                    SheetInspectParam inspectParam = new SheetInspectParam(currentImage, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, null);
                    inspectParam.ClipImage = currentImage;
                    inspectParam.ProcessBufferSet = bufferSet;
                    finderResult = (FiducialFinderAlgorithmResult)fiducialFinder.Inspect(inspectParam);
                    offset = finderResult.OffsetFound;
                }));

                UpdateFiducialResult(finderResult);
            }
            else
            {
                finderResult.Good = true;
            }

            SheetResult sheetResult = new SheetResult();

            SimpleProgressForm inspectorForm = new SimpleProgressForm("Inspect");
            inspectorForm.Show(new Action(() =>
            {
                SheetInspector sheetInspector = (SheetInspector)AlgorithmPool.Instance().GetAlgorithm(SheetInspector.TypeName);
                SheetInspectParam inspectParam = new SheetInspectParam(currentImage, RotatedRect.Empty, RotatedRect.Empty, Size.Empty, null, null);
                inspectParam.ClipImage = currentImage;
                inspectParam.ProcessBufferSet = bufferSet;
                inspectParam.FidOffset = finderResult.OffsetFound;
                inspectParam.FidResult = finderResult.Good;
                sheetResult = (SheetResult)sheetInspector.Inspect(inspectParam);
            }));

            UpdateSheetResult(sheetResult);

            bufferSet.Dispose();
        }
    }
}