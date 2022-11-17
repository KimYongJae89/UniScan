using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.Light;
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
using UniScan.Common;
using UniScanG.Data;
using UniScanG.Data.Model;
using UniScanG.Screen.Data;
using UniScanG.Screen.Vision;
using UniScanG.Screen.Vision.Detector;
using UniScanG.Screen.Vision.Trainer;
using UniScanG.UI.Etc;
using UniScanG.UI.Teach;
using UniScanG.Vision;
using UniScanG.Vision.FiducialFinder;

namespace UniScanG.Screen.UI.Teach
{
    public delegate void UpdatePatternFigureDelegate(SheetPattern pattern);
    public delegate void UpdateFiducialPatternFigureDelegate(FiducialPattern pattern);
    public delegate void UpdateRegionInfoDelegate(RegionInfo regionInfo);
    public delegate void UpdateSheetResultDelegate(SheetResult sheetResult);
    public delegate void ExportDataDelegate();

    public class ModellerPageExtenderS : UniEye.Base.UI.HwTriggerModellerPageExtender, IModelListener
    {
        Image2D currentImage;
        
        public UpdatePatternFigureDelegate UpdatePatternFigure;
        public UpdateFiducialPatternFigureDelegate UpdateFiducialPatternFigure;
        public UpdateRegionInfoDelegate UpdateRegionInfo;
        public UpdateSheetResultDelegate UpdateSheetResult;
        public ExportDataDelegate ExportData;

        public ModellerPageExtenderS()
        {
            SystemManager.Instance().ExchangeOperator.AddModelListener(this);
        }

        public void LoadImage()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (currentImage != null)
                    currentImage.Dispose();

                currentImage = new Image2D(dlg.FileName);
                currentImage.ConvertFromData();

                if (currentImage != null && UpdateImage != null)
                    UpdateImage(currentImage);
            }
        }

        protected override void SetupGrab(LightParam lightParam)
        {
            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;

            foreach (LightCtrl light in lightCtrlHandler)
                light.TurnOn(lightParam.LightValue);
            
            ImageDevice imageDevice = GetImageDevice(deviceIndex);
            imageDevice.ImageGrabbed += ImageGrabbed;    
        }

        public void Grab()
        {
            System.Threading.CancellationTokenSource token = new System.Threading.CancellationTokenSource();

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.AddimageGrabbed(ImageGrabbed);
            imageDeviceHandler.SetTriggerMode(TriggerMode.Hardware);
            imageDeviceHandler.GrabOnce();

            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() => imageDeviceHandler.WaitGrabDone(30000));
        }

        public void GrabProcess(int deviceIndex, LightParam lightParam)
        {
            this.deviceIndex = deviceIndex;

            SetupGrab(lightParam);
            
            GetImageDevice(deviceIndex).GrabOnce();
        }

        public override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.SetTriggerMode(TriggerMode.Software);

            imageDevice.ImageGrabbed -= ImageGrabbed;



            if (currentImage != null)
                currentImage.Dispose();

            currentImage = (Image2D)imageDevice.GetGrabbedImage(ptr).Clone();

            currentImage.ConvertFromDataPtr();
            
            if (UpdateImage != null)
                UpdateImage(currentImage);
            
            string imagePath = SystemManager.Instance().CurrentModel.GetImagePath();

            if (Directory.Exists(imagePath) == false)
                Directory.CreateDirectory(imagePath);

            string fileName = string.Format("Image.{0}", ImageFormat.Bmp.ToString());
            string filePath = Path.Combine(imagePath, fileName);

            Bitmap bitmap = currentImage.ToBitmap();
            ImageHelper.SaveImage(bitmap, filePath, ImageFormat.Bmp);
            bitmap.Dispose();

            // 메모리 정리가 필요함 (메모리 터짐)
            GC.Collect();
        }

        public void Teach()
        {
            ProgressForm progressForm = new ProgressForm();
            progressForm.StartPosition = FormStartPosition.CenterScreen;
            progressForm.TitleText = "Auto Teach";
            progressForm.MessageText = "Start";
            
            progressForm.BackgroundWorker.DoWork += BackgroundWorker_DoWork;
            progressForm.RunWorkerCompleted = RunWorkerCompleted;

            progressForm.TopMost = true;
            
            progressForm.Show();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SheetTrainer trainer = (SheetTrainer)AlgorithmPool.Instance().GetAlgorithm(SheetTrainer.TypeName);

            if (trainer == null)
                return;

            trainer.Teach((BackgroundWorker)sender, currentImage, e);
        }

        private void RunWorkerCompleted(bool result)
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

        public void SelectedPattern(SheetPattern pattern)
        {
            if (UpdatePatternFigure != null)
                UpdatePatternFigure(pattern);
        }

        public void SelectedFiducialPattern(FiducialPattern pattern)
        {
            if (UpdateFiducialPatternFigure != null)
                UpdateFiducialPatternFigure(pattern);
        }

        public void SelectedRegionInfo(RegionInfo regionInfo)
        {
            if (UpdateRegionInfo != null)
                UpdateRegionInfo(regionInfo);
        }

        public void SaveModel()
        {
            Model currentModel = SystemManager.Instance().CurrentModel;

            SimpleProgressForm loadingForm = new SimpleProgressForm("Save Model");
            loadingForm.Show(new Action(() =>
            {
                SystemManager.Instance().ModelManager.SaveModel(currentModel);
                SystemManager.Instance().ModelManager.SaveModelDescription(currentModel);
                AlgorithmSetting.Instance().Save();
            }));
        }

        public void Inspect()
        {
            if (currentImage == null)
                return;

            SizeF offset = new SizeF();
            ProcessBufferSetS bufferSet = new ProcessBufferSetS(SheetInspector.TypeName, currentImage.Width, currentImage.Height);

            FiducialFinderAlgorithmResult finderResult = new FiducialFinderAlgorithmResult();
            if (AlgorithmSetting.Instance().IsFiducial == true)
            {
                SimpleProgressForm fiducialForm = new SimpleProgressForm("Find Fiducial");
                fiducialForm.Show(new Action(() =>
                {
                    FiducialFinder fiducialFinder = (FiducialFinder)AlgorithmPool.Instance().GetAlgorithm(FiducialFinder.TypeName);
                    SheetInspectParam inspectParam = new SheetInspectParam();
                    inspectParam.ClipImage = currentImage;
                    inspectParam.BuffetSet = bufferSet;
                    finderResult = (FiducialFinderAlgorithmResult)fiducialFinder.Inspect(inspectParam);
                    offset = finderResult.OffsetFound;
                }));
            }

            Stopwatch stopwatch = new Stopwatch();

            SheetResult sheetResult = new SheetResult();

            SimpleProgressForm inspectorForm = new SimpleProgressForm("Inspect");
            inspectorForm.Show(new Action(() =>
            {
                SheetInspector sheetInspector = (SheetInspector)AlgorithmPool.Instance().GetAlgorithm(SheetInspector.TypeName);
                SheetInspectParam inspectParam = new SheetInspectParam();
                inspectParam.ClipImage = currentImage;
                inspectParam.BuffetSet = bufferSet;
                inspectParam.FidOffset = finderResult.OffsetFound;
                stopwatch.Start();
                sheetResult = (SheetResult)sheetInspector.Inspect(inspectParam);
                stopwatch.Stop();
            }));

            UpdateSheetResult(sheetResult);

            bufferSet.Dispose();
        }

        public void ModelChanged()
        {
            string imagePath = SystemManager.Instance().CurrentModel.GetImagePath();
            
            string fileName = string.Format("Image.{0}", ImageFormat.Bmp.ToString());
            string filePath = Path.Combine(imagePath, fileName);

            if (currentImage != null)
                currentImage.Dispose();

            if (File.Exists(filePath))
            {
                currentImage = new Image2D(filePath);

                if (currentImage != null && UpdateImage != null)
                    UpdateImage(currentImage);
            }
            else
            {
                currentImage = null;
                UpdateImage(currentImage);
            }
        }

        public void ModelTeachDone() { }
    }
}