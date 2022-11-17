using DynMvp.Base;
using DynMvp.Devices;
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

namespace UniScanS.UI.Teach
{
    public delegate void ImageUpdatedDelegate(Image image);
    public delegate void UpdateFigureDelegate(FigureGroup figureGroup);
    public delegate void UpdateSheetResultDelegate(SheetResult sheetResult);
    public delegate void ExportDataDelegate();
    public delegate void UpdateFiducialResultDelegate(FiducialFinderAlgorithmResult fiducialResult);
    public delegate void ParamChangedDelegate();

    public abstract class ModellerPageExtender : UniEye.Base.UI.HwTriggerModellerPageExtender, IModelListener
    {
        protected Image2D currentImage;

        public ImageUpdatedDelegate ImageUpdated;
        public UpdateFigureDelegate UpdateFigure;
        public UpdateSheetResultDelegate UpdateSheetResult;
        public UpdateFiducialResultDelegate UpdateFiducialResult;
        public ExportDataDelegate ExportData;
        public ParamChangedDelegate ParamChanged;

        public ModellerPageExtender()
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
                    UpdateImage(currentImage, true);
            }
        }

        //protected override void SetupGrab(LightParam lightParam)
        //{
        //    LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;

        //    foreach (LightCtrl light in lightCtrlHandler)
        //        light.TurnOn(lightParam.LightValue);

        //    ImageDevice imageDevice = GetImageDevice(deviceIndex);
        //    imageDevice.ImageGrabbed += ImageGrabbed;    
        //}

        public abstract void Grab();
        
        public void Teach()
        {
            Etc.ProgressForm progressForm = new Etc.ProgressForm();
            progressForm.StartPosition = FormStartPosition.CenterScreen;
            progressForm.TitleText = "Auto Teach";
            progressForm.MessageText = "Start";
            
            progressForm.BackgroundWorker.DoWork += TeachBackgroundWorker_DoWork;
            progressForm.RunWorkerCompleted = TeachRunWorkerCompleted;

            progressForm.TopMost = true;
            
            progressForm.ShowDialog();
        }
        
        protected abstract void TeachBackgroundWorker_DoWork(object sender, DoWorkEventArgs e);
        protected abstract void TeachRunWorkerCompleted(bool result);
        
        public virtual void SaveModel()
        {
            UniScanS.Data.Model.Model currentModel = SystemManager.Instance().CurrentModel;

            SimpleProgressForm loadingForm = new SimpleProgressForm("Save Model");
            loadingForm.Show(new Action(() =>
            {
                SystemManager.Instance().ModelManager.SaveModel(currentModel);
                SystemManager.Instance().ModelManager.SaveModelDescription(currentModel.ModelDescription);
                AlgorithmSetting.Instance().Save();
            }));

            SystemManager.Instance().ExchangeOperator.ModelTeachDone();
        }

        public abstract void Inspect();

        public void ModelChanged()
        {
            string imagePath = SystemManager.Instance().CurrentModel.GetImagePath();

            string fileName = GetModelImageName();
            string filePath = Path.Combine(imagePath, fileName);

            if (currentImage != null)
                currentImage.Dispose();

            if (File.Exists(filePath))
            {
                currentImage = new Image2D(filePath);

                if (currentImage != null && UpdateImage != null)
                    UpdateImage(currentImage, true);
            }
            else
            {
                currentImage = null;
                UpdateImage(currentImage, true);
            }
        }

        public abstract string GetModelImageName();

        public void ModelTeachDone() { }
    }
}