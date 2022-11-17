using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.Devices.Dio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using DynMvp.InspData;
using UniEye.Base.Data;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using UniEye.Base.Settings;

namespace UniEye.Base.UI
{
    public delegate void UpdateImageDelegate(ImageD grabImage, bool zoomFit);
    
    public class ModellerPageExtender
    {
        int centerCameraIndex;

        InspectionResult inspectionResult;
        public InspectionResult InspectionResult
        {
            get { return inspectionResult; }
            set { inspectionResult = value; }
        }

        public UpdateImageDelegate UpdateImage;
        public UpdateImageDelegate UpdateImage3D;

        protected int stepIndex;
        protected int deviceIndex;
        protected int lightTypeIndex;

        TransformData transformData2d = null;

        public ModellerPageExtender()
        {
        }
        
        protected ImageDevice GetImageDevice(int deviceIndex)
        {
            return SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(deviceIndex);
        }

        protected virtual void SetupGrab(LightParam lightParam = null)
        {
            ImageDevice imageDevice = GetImageDevice(deviceIndex);
            if (imageDevice == null)
                return;

            //if (lightParam == null || lightParam.LightParamType == LightParamType.Value)
            //    GetImageDevice(deviceIndex).ImageGrabbed += ImageGrabbed;
        }

        public virtual void GrabProcess(int stepIndex, int deviceIndex, int lightTypeIndex, LightParamSet lightParamSet)
        {
            ImageDevice imageDevice = GetImageDevice(deviceIndex);
            if (imageDevice == null)
                return;

            GrabAll(stepIndex, lightParamSet);
        }

        public virtual float[] AutoLigthProcess(float rollDiaMm)
        {
            return new float[256];
        }

        public virtual void AutoTeachProcess(float lineSpeedMpm)
        {

        }

        bool IsTransformInitialized(TransformData[] transformDataArray)
        {
            foreach(TransformData transformData in transformDataArray)
            {
                if (transformData.IsInitialized())
                    return true;
            }

            return false;
        }

        //private Point3d[] ConvertPointArray(Image2D image, Size imageSize)
        //{
        //    Calibration cameraCalibration = cameraCalibrationList[centerCameraIndex];
            
        //    List<Point3d> point3dList = new List<Point3d>();
        //    for (int i=0; i<image)
        //}

        //private void ImageGrabbed_Mapping(ImageDevice imageDevice)
        //{
        //    imageDevice.ImageGrabbed -= ImageGrabbed_Mapping;

        //    if (UpdateImage != null)
        //    {
        //        ImageD image = imageDevice.GetGrabDestImage(0).Clone();
        //        ConvertPointArray((Image2D)image, imageDevice.ImageSize);
        //        int srcWidth = image.Width - 6;
        //        int srcHeight = image.Height - 6;
        //        int height = srcHeight / 4 * 3;
        //        RotatedRect clipRect = new RotatedRect((srcWidth - srcHeight) /2 + 3, (srcHeight - height)/2 + 3, srcHeight, height, 90);
        //        Bitmap rotatedImage = ImageHelper.ClipImage(image.ToBitmap(), clipRect);
        //        UpdateImage(Image2D.ToImage2D(rotatedImage));
        //    }
        //}

        //private void CalibImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        //{
        //    imageDevice.ImageGrabbed -= CalibImageGrabbed;

        //    if (UpdateImage != null)
        //        UpdateImage(imageDevice.GetGrabbedImage(ptr));
        //}

        public virtual Image2D GrabOnce(int stepIndex, int deviceIndex, int lightTypeIndex, LightParam lightParam, LightParamSet lightParamSet)
        {
            this.stepIndex = stepIndex;
            this.deviceIndex = deviceIndex;
            this.lightTypeIndex = lightTypeIndex;

            SetupGrab(lightParam);

            if (lightParam == null || lightParam.LightParamType == LightParamType.Value)
                GetImageDevice(deviceIndex).ImageGrabbed += ImageGrabbed;

            ImageAcquisition imageAcquisition = SystemManager.Instance().DeviceBox.GetImageAcquisition();
            Image2D image2D = imageAcquisition.Acquire(deviceIndex, stepIndex, lightTypeIndex, lightParam, lightParamSet);

            GetImageDevice(deviceIndex).ImageGrabbed -= ImageGrabbed;
            return image2D;
        }

        public void GrabAll(int stepIndex, LightParamSet lightParamSet)
        {
            this.stepIndex = stepIndex;

            // Value타입 먼저 그랩.
            List<LightParam> grabLightParamList = lightParamSet.LightParamList.FindAll(f => f.LightParamType == LightParamType.Value);

            List<LightParam> compositeLightParamList = lightParamSet.LightParamList.FindAll(f => f.LightParamType == LightParamType.Composite);
            grabLightParamList.AddRange(compositeLightParamList);

            foreach (LightParam lightParam in grabLightParamList)
            {
                this.lightTypeIndex = lightParamSet.LightParamList.FindIndex(f => f.Equals(lightParam));

                SetupGrab(lightParam);

                ImageAcquisition imageAcquisition = SystemManager.Instance().DeviceBox.GetImageAcquisition();

                imageAcquisition.Acquire(stepIndex, this.lightTypeIndex, lightParam);

                Model currentModel = SystemManager.Instance().CurrentModel;
                string imagePath = Path.Combine(currentModel.ModelPath, "Image");
                if (Directory.Exists(imagePath) == false)
                {
                    Directory.CreateDirectory(imagePath);
                }

                imageAcquisition.ImageBuffer.Save(imagePath, stepIndex, ImageFormat.Bmp);
            }
            //this.lightTypeIndex = lightTypeIndex;
        }

        public virtual void StopGrab()
        {
            ImageDevice imageDevice = GetImageDevice(deviceIndex);
            if (imageDevice == null)
                return;

            imageDevice.ImageGrabbed -= ImageGrabbed;
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.SetTriggerMode(TriggerMode.Software);
        }

        public virtual void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            LogHelper.Debug(LoggerType.Operation, "ModellerPage - ImageGrabbed");
            
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;
            imageDeviceHandler.SetTriggerMode(TriggerMode.Software);

            imageDevice.ImageGrabbed -= ImageGrabbed;
            
            ImageD grabImage = imageDevice.GetGrabbedImage(ptr);

            if (UpdateImage != null)
                UpdateImage(grabImage, false);

            Model currentModel = SystemManager.Instance().CurrentModel;

            string imagePath = Path.Combine(currentModel.ModelPath, "Image");
            if (Directory.Exists(imagePath) == false)
            {
                Directory.CreateDirectory(imagePath);
            }

            string fileName = ImageBuffer.GetImage2dFileName(deviceIndex, stepIndex, lightTypeIndex, ImageFormat.Bmp);
            string filePath = Path.Combine(imagePath, fileName);
            ImageHelper.SaveImage(grabImage.ToBitmap(), filePath, ImageFormat.Bmp);
        }

        public virtual void OnPreStepInspection(InspectionStep inspectionStep)
        {

        }

        public virtual void OnPostStepInspection(InspectionStep inspectionStep)
        {

        }

        public virtual bool GetProbeFilter(Probe probe, out IProbeFilter probeFilter)
        {
            probeFilter = null;
            return true;
        }
    }

    public class HwTriggerModellerPageExtender : ModellerPageExtender
    {
        public HwTriggerModellerPageExtender() : base()
        {
        }

        public override void GrabProcess(int stepIndex, int deviceIndex, int lightTypeIndex, LightParamSet lightParamSet)
        {
            this.stepIndex = stepIndex;
            this.deviceIndex = deviceIndex;
            this.lightTypeIndex = lightTypeIndex;

            SetupGrab();

            ImageAcquisition imageAcquisition = SystemManager.Instance().DeviceBox.GetImageAcquisition();
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                ImageBuffer2dItem imageCell = imageAcquisition.ImageBuffer.GetImageBuffer2dItem(imageDevice.Index, 0);

                if (imageCell.LightParam != null)
                    imageDevice.SetExposureTime(imageCell.LightParam.ExposureTimeUs);

                imageDevice.GrabOnce();
                imageDeviceHandler.SetTriggerMode(TriggerMode.Hardware, TriggerType.RisingEdge);
            }
        }

        public void GrabProcess(int stepIndex, int deviceIndex, int lightTypeIndex, LightParamSet lightParamSet, ImageDeviceEventDelegate imageGrabbed)
        {
            this.stepIndex = stepIndex;
            this.deviceIndex = deviceIndex;
            this.lightTypeIndex = lightTypeIndex;
            
            ImageDevice curImageDevice = SystemManager.Instance().DeviceBox.ImageDeviceHandler.GetImageDevice(deviceIndex);
            if (curImageDevice == null)
                return;
            
            curImageDevice.ImageGrabbed += imageGrabbed;

            ImageAcquisition imageAcquisition = SystemManager.Instance().DeviceBox.GetImageAcquisition();
            ImageDeviceHandler imageDeviceHandler = SystemManager.Instance().DeviceBox.ImageDeviceHandler;

            foreach (ImageDevice imageDevice in imageDeviceHandler)
            {
                ImageBuffer2dItem imageCell = imageAcquisition.ImageBuffer.GetImageBuffer2dItem(imageDevice.Index, 0);

                if (imageCell.LightParam != null)
                    imageDevice.SetExposureTime(imageCell.LightParam.ExposureTimeUs);

                imageDeviceHandler.SetTriggerMode(TriggerMode.Hardware, TriggerType.RisingEdge);
            }
        }
    }
}
