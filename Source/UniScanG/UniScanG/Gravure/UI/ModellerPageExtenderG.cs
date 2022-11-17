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

namespace UniScanG.Gravure.UI
{
    public delegate void UpdatePatternFigureDelegate(SheetPattern pattern);
    public delegate void UpdateFiducialPatternFigureDelegate(FiducialPattern pattern);
    public delegate void UpdateRegionInfoDelegate(RegionInfo regionInfo);
    public delegate void UpdateSheetResultDelegate(SheetResult sheetResult);

    public class ModellerPageExtenderG : UniScanG.UI.ModellerPageExtender
    {
        protected override void SetupGrab(LightParam lightParam)
        {
            LightCtrlHandler lightCtrlHandler = SystemManager.Instance().DeviceBox.LightCtrlHandler;

            foreach (LightCtrl light in lightCtrlHandler)
                light.TurnOn(lightParam.LightValue);
            
            ImageDevice imageDevice = GetImageDevice(deviceIndex);
            imageDevice.ImageGrabbed += ImageGrabbed;    
        }

        /// <summary>
        /// Gravure Grab Process
        /// </summary>
        public override void Grab()
        {
            System.Threading.CancellationTokenSource token = new System.Threading.CancellationTokenSource();

            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.AddimageGrabbed(ImageGrabbed);
            imageDeviceHandler.GrabOnce();

            SimpleProgressForm simpleProgressForm = new SimpleProgressForm();
            simpleProgressForm.Show(() => imageDeviceHandler.WaitGrabDone());
        }
        
        /// <summary>
        /// Arrive a new frame
        /// </summary>
        /// <param name="imageDevice"></param>
        /// <param name="ptr"></param>
        public override void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            SimpleProgressForm simpleProgressForm = new SimpleProgressForm("Update");
            simpleProgressForm.Show(() =>
            {

                ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
                imageDeviceHandler.SetTriggerMode(TriggerMode.Software);

                imageDevice.ImageGrabbed -= ImageGrabbed;

                if (currentImage != null)
                    currentImage.Dispose();

                currentImage = (Image2D)imageDevice.GetGrabbedImage(ptr);

                currentImage.ConvertFromDataPtr();

                if (UpdateImage != null)
                    UpdateImage(currentImage);

                string imagePath;
                if (SystemManager.Instance().CurrentModel != null)
                    imagePath = SystemManager.Instance().CurrentModel.GetImagePath();
                else
                    imagePath = Path.Combine(UniEye.Base.Settings.PathSettings.Instance().Temp, "Image");

                if (Directory.Exists(imagePath) == false)
                    Directory.CreateDirectory(imagePath);

                string fileName = string.Format("Image.{0}", ImageFormat.Bmp.ToString());
                string filePath = Path.Combine(imagePath, fileName);

                Bitmap bitmap = currentImage.ToBitmap();
                ImageHelper.SaveImage(bitmap, filePath, ImageFormat.Bmp);
                bitmap.Dispose();

                // 메모리 정리가 필요함 (메모리 터짐)
                GC.Collect();
            });
        }

        public override void Teach()
        {
            throw new NotImplementedException();
        }

        public override void Inspect()
        {
            throw new NotImplementedException();
        }
    }
}