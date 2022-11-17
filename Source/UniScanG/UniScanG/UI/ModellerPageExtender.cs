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
using UniScanG.Data.Model;
using UniScanG.Screen.Data;
using UniScanG.Screen.Vision;
using UniScanG.Screen.Vision.Detector;
using UniScanG.Screen.Vision.Trainer;
using UniScanG.UI.Etc;
using UniScanG.Vision;
using UniScanG.Vision.FiducialFinder;

namespace UniScanG.UI
{
    public delegate void UpdatePatternFigureDelegate(SheetPattern pattern);
    public delegate void UpdateFiducialPatternFigureDelegate(FiducialPattern pattern);
    public delegate void UpdateRegionInfoDelegate(RegionInfo regionInfo);
    public delegate void UpdateSheetResultDelegate(SheetResult sheetResult);

    public abstract class ModellerPageExtender : UniEye.Base.UI.HwTriggerModellerPageExtender
    {
        protected Image2D currentImage;
        
        public UpdatePatternFigureDelegate UpdatePatternFigure;
        public UpdateFiducialPatternFigureDelegate UpdateFiducialPatternFigure;
        public UpdateRegionInfoDelegate UpdateRegionInfo;
        public UpdateSheetResultDelegate UpdateSheetResult;

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

        public void GrabProcess(int deviceIndex, LightParam lightParam)
        {
            this.deviceIndex = deviceIndex;

            SetupGrab(lightParam);
            
            GetImageDevice(deviceIndex).GrabOnce();
        }
        public abstract void Grab();

        public abstract void Teach();
        
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
            UniScanG.Data.Model.Model currentModel = SystemManager.Instance().CurrentModel;

            SimpleProgressForm loadingForm = new SimpleProgressForm("Save Model");
            loadingForm.Show(new Action(() =>
            {
                SystemManager.Instance().ModelManager.SaveModel(currentModel);
                SystemManager.Instance().ModelManager.SaveModelDescription(currentModel);
                AlgorithmSetting.Instance().Save();
            }));
        }

        public abstract void Inspect();
    }
}