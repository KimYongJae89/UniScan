using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

using DynMvp.Devices.Light;
using DynMvp.Base;
using System.Drawing.Imaging;

namespace DynMvp.Devices.FrameGrabber
{
    public class ImageAcquisition
    {
        int numDevice;
        int numLightType;
        LightCtrlHandler lightCtrlHandler;

        private ImageDeviceHandler imageDeviceHandler;
        public ImageDeviceHandler ImageDeviceHandler
        {
            get { return imageDeviceHandler; }
        }

        private ImageBuffer imageBuffer = new ImageBuffer();
        public ImageBuffer ImageBuffer
        {
            get { return imageBuffer; }
        }

        public ImageAcquisition()
        {

        }

        public void Initialize(ImageDeviceHandler imageDeviceHandler, LightCtrlHandler lightCtrlHandler, int numLightType, int num3dImage)
        {
            this.imageDeviceHandler = imageDeviceHandler;
            this.lightCtrlHandler = lightCtrlHandler;
            this.numDevice = imageDeviceHandler.Count;
            this.numLightType = numLightType;

            imageBuffer.Initialize(numDevice, numLightType);

            foreach(ImageDevice imageDevice in imageDeviceHandler)
            {
                for (int lightTypeIndex=0; lightTypeIndex < numLightType; lightTypeIndex++)
                    imageBuffer.Set2dImage(imageDevice.Index, lightTypeIndex, (Image2D)imageDevice.CreateCompatibleImage());

                if (imageDevice.IsDepthScanner())
                {
                    Size imageSize = imageDevice.ImageSize;
                    imageBuffer.Init3dImage(imageDevice.Index, num3dImage, imageSize.Width, imageSize.Height);
                }
            }
        }

        public ImageD GetGrabbedImage(int deviceIndex)
        {
            ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(deviceIndex);
            if (imageDevice == null)
                return null;

            return imageDevice.GetGrabbedImage(IntPtr.Zero);
        }

        public void Acquire(int inspectionStep, int lightTypeIndex, LightParam lightParam = null, float exposureTimeUs = 0)
        {
            int numDevice = imageDeviceHandler.Count;
            
            for (int deviceIndex = 0; deviceIndex < numDevice; deviceIndex++)
            {
                ImageBuffer2dItem imageCell = ImageBuffer.GetImageBuffer2dItem(deviceIndex, lightTypeIndex);
                if (imageCell.Image != null)
                    imageCell.Image.Clear();

                if (lightParam == null)
                    lightParam = imageCell.LightParam;

                imageCell.LightParam = lightParam;
                if (lightParam.LightParamType == LightParamType.Composite)
                {
                    if (lightTypeIndex == lightParam.FirstImageIndex || lightTypeIndex == lightParam.SecondImageIndex)
                    {
                        imageCell.Image.Clear();
                    }
                    else
                    {
                        if (imageCell.Image.NumBand != 1)
                        {
                            imageCell.Image = new Image2D(imageCell.Image.Width, imageCell.Image.Height, 1);
                        }

                        Image2D firstImage = ImageBuffer.GetImageBuffer2dItem(deviceIndex, lightParam.FirstImageIndex).Image.GetGrayImage();
                        Image2D secondImage = ImageBuffer.GetImageBuffer2dItem(deviceIndex, lightParam.SecondImageIndex).Image.GetGrayImage();

                        ImageOperation.Operate(lightParam.OperationType, firstImage, secondImage, imageCell.Image);
                    }
                }
                else
                {
                    float grabExposureTimeUs = 0;
                    if (exposureTimeUs == 0)
                        grabExposureTimeUs = lightParam.ExposureTimeUs;
                    else
                        grabExposureTimeUs = exposureTimeUs;

                    ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(deviceIndex);
                    if (imageDevice != null)
                    {
                        imageDevice.SetExposureTime(grabExposureTimeUs);

                        if (lightCtrlHandler != null)
                            lightCtrlHandler.TurnOn(lightParam);

                        imageDevice.GrabOnce();

                        imageDeviceHandler.WaitGrabDone(5000);

                        imageCell.Image.CopyFrom(imageDevice.GetGrabbedImage(IntPtr.Zero));
                        if (lightCtrlHandler != null)
                            lightCtrlHandler.TurnOff();
                    }
                }
            }

        }

        // 버퍼에 설정된 조명 조건으로 여러 카메라의 이미지를 획득한다. 
        // 동일 조명 타입이라도 카메라별로 다른 조명 조건이 설정되어 있을 수 있고, 이에 따른 조명 설정값을 얻어와 영상이 얻어진다.
        public void Acquire(int inspectionStep)
        {
            for (int lightTypeIndex = 0; lightTypeIndex < numLightType; lightTypeIndex++)
            {
                LightParamList lightValueList = imageBuffer.GetLightValueList(lightTypeIndex);
                foreach (LightParam lightParam in lightValueList)
                {
                    if (lightParam.LightParamType == LightParamType.Value)
                    {
                        if (lightCtrlHandler != null)
                        {
                            lightCtrlHandler.TurnOn(lightParam);
                        }
                    }
                    for (int deviceIndex = 0; deviceIndex < numDevice; deviceIndex++)
                    {
                        ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(deviceIndex);
                        if (imageDevice != null && imageDevice.IsDepthScanner() == false)
                        {
                            ImageBuffer2dItem imageCell = imageBuffer.GetImageBuffer2dItem(deviceIndex, lightTypeIndex);

                            if (lightCtrlHandler == null || imageCell.LightParam.KeyValue == lightParam.KeyValue)
                            {
                                if (imageCell.Image != null)
                                    imageCell.Image.Clear();
 
                                if (lightParam.LightParamType == LightParamType.Value)
                                {
                                    imageDevice.SetExposureTime(imageCell.LightParam.ExposureTimeUs);
                                    imageDevice.GrabOnce();
                                    imageDeviceHandler.WaitGrabDone(5000);

                                    imageCell.Image.CopyFrom(imageDevice.GetGrabbedImage(IntPtr.Zero));
                                }
                                else if (lightParam.LightParamType == LightParamType.Composite)
                                {
                                    if (imageCell.Image.NumBand != 1)
                                    {
                                        imageCell.Image = new Image2D(imageCell.Image.Width, imageCell.Image.Height, 1);
                                    }

                                    Image2D firstImage = ImageBuffer.GetImageBuffer2dItem(deviceIndex, lightParam.FirstImageIndex).Image.GetGrayImage();
                                    Image2D secondImage = ImageBuffer.GetImageBuffer2dItem(deviceIndex, lightParam.SecondImageIndex).Image.GetGrayImage();

                                    ImageOperation.Operate(lightParam.OperationType, firstImage, secondImage, imageCell.Image);
                                }
                            }
                        }
                    }

                    if (lightCtrlHandler != null)
                    {
                        lightCtrlHandler.TurnOff();
                    }
                }
            }
            imageDeviceHandler.Stop();

            for (int deviceIndex = 0; deviceIndex < numDevice; deviceIndex++)
            {
                ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(deviceIndex);
                if (imageDevice != null && imageDevice.IsDepthScanner())
                {
                    ImageBuffer3dItem imageBuffer3dItem = imageBuffer.GetImageBuffer3dItem(deviceIndex);
                    if (imageBuffer3dItem.LightParam != null)
                    {
                        imageDevice.SetExposureTime3d(imageBuffer3dItem.ExposureTime3dUs);
                    }

                    imageDevice.Grab3D();

                    imageBuffer3dItem.CopyImages(imageDevice.GetGrabbedImageList());

                    imageBuffer3dItem.ResultImage = imageDevice.Calculate(Rectangle.Empty, imageBuffer3dItem.TransformDataList, imageBuffer.PixelRes3D);
                }
            }
#if DEBUG
            for (int lightTypeIndex = 0; lightTypeIndex < numLightType; lightTypeIndex++)
            {
                for (int deviceIndex = 0; deviceIndex < numDevice; deviceIndex++)
                {
                    ImageBuffer2dItem imageCell = imageBuffer.GetImageBuffer2dItem(deviceIndex, lightTypeIndex);
                    imageCell.SaveImage(string.Format(@"D:\{0}_{1}.bmp", deviceIndex, lightTypeIndex), System.Drawing.Imaging.ImageFormat.Bmp);
                }
            }
#endif
        }

        // 하나의 조명 조건으로 여러 카메라의 영상을 획득한다.
        public void Acquire(int inspectionStep, LightParamSet lightParamSet)
        {
            int numDevice = imageDeviceHandler.Count;

            for (int lightTypeIndex = 0; lightTypeIndex < lightParamSet.NumLightType; lightTypeIndex++)
            {
                LightParam lightParam = lightParamSet.LightParamList[lightTypeIndex];
                LightValue lightValue = lightParam.LightValue;
                if (lightCtrlHandler != null)
                {
                    lightCtrlHandler.TurnOn(lightValue);
                }

                for (int deviceIndex = 0; deviceIndex < numDevice; deviceIndex++)
                {
                    ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(deviceIndex);
                    if (imageDevice != null && imageDevice.IsDepthScanner() == false)
                    {
                        ImageBuffer2dItem imageCell = ImageBuffer.GetImageBuffer2dItem(deviceIndex, lightTypeIndex);
                        imageCell.LightParam.ExposureTimeUs = lightParam.ExposureTimeUs;

                        if (imageCell.Image != null)
                            imageCell.Image.Clear();

                        imageDevice.SetExposureTime(imageCell.LightParam.ExposureTimeUs);

                        imageDevice.GrabOnce();

                        imageCell.Image.CopyFrom(imageDevice.GetGrabbedImage(IntPtr.Zero));
                    }
                }

                if (lightCtrlHandler != null)
                {
                    lightCtrlHandler.TurnOff();
                }

                imageDeviceHandler.WaitGrabDone(5000);
            }

            imageDeviceHandler.Stop();

            for (int deviceIndex = 0; deviceIndex < numDevice; deviceIndex++)
            {
                ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(deviceIndex);
                if (imageDevice != null && imageDevice.IsDepthScanner())
                {
                    ImageBuffer3dItem imageBuffer3dItem = imageBuffer.GetImageBuffer3dItem(deviceIndex);
                    if (imageBuffer3dItem.LightParam != null)
                    {
                        imageDevice.SetExposureTime3d(imageBuffer3dItem.ExposureTime3dUs);
                    }

                    imageDevice.Grab3D();

                    imageBuffer3dItem.CopyImages(imageDevice.GetGrabbedImageList());

                    imageBuffer3dItem.ResultImage = imageDevice.Calculate(Rectangle.Empty, imageBuffer3dItem.TransformDataList, imageBuffer.PixelRes3D);
                }
            }
        }

        public void AcquireCalibation(int deviceIndex, int lightTypeIndex)
        {
            ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(deviceIndex);
            LightValue lightValue = new LightValue(2);
            lightValue.Value[0] = 0;
            lightValue.Value[1] = 255;

            if (lightCtrlHandler != null)
            {
                lightCtrlHandler.TurnOn(lightValue);
            }

            if (imageDevice != null && imageDevice.IsDepthScanner() == false)
            {
                ImageBuffer2dItem imageCell = ImageBuffer.GetImageBuffer2dItem(deviceIndex, lightTypeIndex);

                if (imageCell.Image != null)
                    imageCell.Image.Clear();

                imageDevice.SetExposureTime(100000);

                imageDevice.GrabOnce();

                imageCell.Image.CopyFrom(imageDevice.GetGrabbedImage(IntPtr.Zero));
            }
                
            imageDeviceHandler.WaitGrabDone(5000);

            if (lightCtrlHandler != null)
            {
                lightCtrlHandler.TurnOff();
            }
        }

        public Image2D Acquire(int deviceIndex, int inspectionStep, int lightTypeIndex, LightParam lightParam, LightParamSet lightParamSet)
        {
            ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(deviceIndex);
            if (imageDevice == null)
                return null;
            
            ImageBuffer2dItem imageBuffer2dItem = ImageBuffer.GetImageBuffer2dItem(deviceIndex, lightTypeIndex);

            if (lightParam == null)
                lightParam = imageBuffer2dItem.LightParam;

            if (lightParam.LightParamType == LightParamType.Composite)
            {
                if (imageBuffer2dItem.Image.NumBand != 1)
                {
                    imageBuffer2dItem.Image = new Image2D(imageBuffer2dItem.Image.Width, imageBuffer2dItem.Image.Height, 1);
                }

                if (lightTypeIndex == lightParam.FirstImageIndex || lightTypeIndex == lightParam.SecondImageIndex)
                {
                    return null;
                }
                Image2D firstImage = Acquire(deviceIndex, inspectionStep, lightParam.FirstImageIndex, lightParamSet.LightParamList[lightParam.FirstImageIndex], lightParamSet);
                Image2D secondImage = Acquire(deviceIndex, inspectionStep, lightParam.SecondImageIndex, lightParamSet.LightParamList[lightParam.SecondImageIndex], lightParamSet);

                ImageOperation.Operate(lightParam.OperationType, firstImage.GetGrayImage(), secondImage.GetGrayImage(), imageBuffer2dItem.Image);
            }
            else
            {
                if (lightCtrlHandler != null)
                    lightCtrlHandler.TurnOn(lightParam);

                imageDevice.SetExposureTime(lightParam.ExposureTimeUs);

                imageDevice.GrabOnce();

                imageDeviceHandler.WaitGrabDone(5000);

                imageBuffer2dItem.Image.CopyFrom(imageDevice.GetGrabbedImage(IntPtr.Zero));

                if (lightCtrlHandler != null)
                    lightCtrlHandler.TurnOff();
            }

            return imageBuffer2dItem.Image;
        }
  
        public ImageD Acquire3D(int deviceIndex, int inspectionStep, int lightTypeIndex, float pixelRes)
        {
            ImageDevice imageDevice = imageDeviceHandler.GetImageDevice(deviceIndex);
            if (imageDevice == null || imageDevice.IsDepthScanner() == false)
                return null;
            
            ImageBuffer3dItem imageBuffer3dItem = ImageBuffer.GetImageBuffer3dItem(deviceIndex);
            if (imageBuffer3dItem.LightParam != null)
            {
                imageDevice.SetExposureTime(imageBuffer3dItem.ExposureTime3dUs);
            }

            imageDevice.Grab3D();

            imageBuffer3dItem.CopyImages(imageDevice.GetGrabbedImageList());

            imageBuffer3dItem.ResultImage = imageDevice.Calculate(new Rectangle(0, 0, imageDevice.ImageSize.Width, imageDevice.ImageSize.Height), pixelRes);

            return imageBuffer3dItem.ResultImage;
        }
    }
}
