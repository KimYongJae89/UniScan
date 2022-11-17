using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Drawing.Imaging;

using DynMvp.Base;
using DynMvp.Devices.Light;

namespace DynMvp.Devices
{
    public class ImageBuffer2dItem
    {
        //private int imageDeviceIndex;
        //public int ImageDeviceIndex
        //{
        //    get { return imageDeviceIndex; }
        //    set { imageDeviceIndex = value; }
        //}

        private LightParam lightParam = null;
        public LightParam LightParam
        {
            get { return lightParam; }
            set { lightParam = value; }
        }

        Image2D image;
        public Image2D Image
        {
            get { return image; }
            set { image = value; }
        }

        private bool ready;
        public bool Ready
        {
            get { return ready; }
            set { ready = value; }
        }

        //public ImageBuffer2dItem(int imageDeviceIndex, int numLight)
        //{
        //    this.imageDeviceIndex = imageDeviceIndex;
        //    this.lightParam = new LightParam(numLight);
        //}

        //public ImageBuffer2dItem(int imageDeviceIndex, LightParam lightParam, Image2D image)
        //{
        //    this.imageDeviceIndex = imageDeviceIndex;
        //    this.lightParam = lightParam;
        //    this.image = image;
        //}

        public ImageBuffer2dItem()
        {
            this.image = new Image2D();
        }

        //public ImageBuffer2dItem(int imageDeviceIndex, Image2D image)
        //{
        //    this.imageDeviceIndex = imageDeviceIndex;
        //    this.image = image;
        //}

        //public void Save(XmlElement imageCellElement)
        //{
        //    XmlElement lightParamElement = imageCellElement.OwnerDocument.CreateElement("", "LightParam", "");
        //    imageCellElement.AppendChild(lightParamElement);

        //    lightParam.Save(lightParamElement);

        //    XmlHelper.SetValue(imageCellElement, "ImageDeviceIndex", imageDeviceIndex.ToString());
        //}

        public void SaveImage(string fileName, ImageFormat imageFormat)
        {
            ImageHelper.SaveImage(image.ToBitmap(), fileName, imageFormat);
        }

        //public void Load(XmlElement imageCellElement)
        //{
        //    XmlElement lightParamElement = imageCellElement["LightParam"];
        //    if (lightParamElement != null)
        //    {
        //        lightParam.Load(lightParamElement);
        //    }

        //    string valueStr = XmlHelper.GetValue(imageCellElement, "ImageDeviceIndex", "");
        //    if (valueStr == "")
        //        valueStr = XmlHelper.GetValue(imageCellElement, "CameraIndex", "0");

        //    imageDeviceIndex = Convert.ToInt32(valueStr);
        //}

        public void LoadImage(string fileName)
        {
            image.LoadImage(fileName);
        }
    }

    public class ImageBuffer3dItem
    {
        //private int imageDeviceIndex;
        //public int ImageDeviceIndex
        //{
        //    get { return imageDeviceIndex; }
        //    set { imageDeviceIndex = value; }
        //}

        private LightParam lightParam;
        public LightParam LightParam
        {
            get { return lightParam; }
            set { lightParam = value; }
        }

        private List<ImageD> imageList = new List<ImageD>();
        public List<ImageD> ImageList
        {
            get { return imageList; }
            set { imageList = value; }
        }

        private Image3D resultImage;
        public Image3D ResultImage
        {
            get { return resultImage; }
            set { resultImage = value; }
        }

        private bool ready;
        public bool Ready
        {
            get { return ready; }
            set { ready = value; }
        }

        TransformDataList transformDataList;
        public TransformDataList TransformDataList
        {
            get { return transformDataList; }
            set { transformDataList = value; }
        }

        private float pixelRes;
        public float PixelRes
        {
            get { return pixelRes; }
            set { pixelRes = value; }
        }

        private int exposureTime3dUs = 50;
        public int ExposureTime3dUs
        {
            get { return exposureTime3dUs; }
            set { exposureTime3dUs = value; }
        }

        public ImageBuffer3dItem()
        {
            //            this.imageDeviceIndex = imageDeviceIndex;
        }

        public void Initialize(int numImage, int width, int height)
        {
            for (int i = 0; i < numImage; i++)
                imageList.Add(new Image2D(width, height, 1));

            resultImage = new Image3D(width, height);
        }

        //public void Save(XmlElement imageCellElement)
        //{
        //    XmlHelper.SetValue(imageCellElement, "ImageDeviceIndex", imageDeviceIndex.ToString());
        //}

        public void SaveImage(string imagePath, string fileNameFormat)
        {
            for (int index = 0; index < imageList.Count; index++)
            {
                string fileName = Path.Combine(imagePath, string.Format(fileNameFormat, index));
                ImageHelper.SaveImage(imageList[index].ToBitmap(), fileName, ImageFormat.Bmp);
            }
        }

        //public void Load(XmlElement imageCellElement)
        //{
        //    imageDeviceIndex = Convert.ToInt32(XmlHelper.GetValue(imageCellElement, "ImageDeviceIndex", ""));
        //}

        public void LoadImage(string imagePath, string fileNameFormat)
        {
            for (int index = 0; index < imageList.Count; index++)
            {
                string fileName = Path.Combine(imagePath, string.Format(fileNameFormat, index));
                if (File.Exists(fileName))
                    imageList[index].LoadImage(fileName);
            }
        }

        public void CopyImages(List<ImageD> srcImageList)
        {
            for (int i = 0; i < imageList.Count && i < srcImageList.Count; i++)
            {
                imageList[i].CopyFrom(srcImageList[i]);
            }
        }
    }

    public class DeviceImageSet
    {
        List<Image2D> imageList2D = new List<Image2D>();
        public List<Image2D> ImageList2D
        {
            get { return imageList2D; }
        }

        Image3D image3D;
        public Image3D Image3D
        {
            get { return image3D; }
        }

        ImageBuffer3dItem imageBuffer3dItem;

        public DeviceImageSet(params Image2D[] image2D)
        {
            imageList2D.AddRange(image2D);
        }

        public DeviceImageSet Clone()
        {
            DeviceImageSet cloneSet = new DeviceImageSet();
            foreach (Image2D image2d in imageList2D)
            {
                cloneSet.ImageList2D.Add((Image2D)image2d.Clone());
            }

            if (image3D != null)
                cloneSet.UpdateImage3D((Image3D)image3D.Clone());

            return cloneSet;
        }

        public void SetImage3D(ImageBuffer3dItem imageBuffer3dItem)
        {
            this.imageBuffer3dItem = imageBuffer3dItem;
            this.image3D = imageBuffer3dItem.ResultImage;
        }

        public void UpdateImage3D(Image3D image3D)
        {
            this.image3D = image3D;
            imageBuffer3dItem.ResultImage = image3D;
        }

        public void UpdateImage2D(Image2D image2D)
        {
            this.imageList2D.Add(image2D);
        }
    }

    public class ImageBuffer
    {
        int numDevice;
        int numLightType;
        public int NumLightType
        {
            get { return numLightType; }
        }

        List<ImageBuffer2dItem> imageBuffer2dItemList = new List<ImageBuffer2dItem>();
        List<ImageBuffer3dItem> imageBuffer3dItemList = new List<ImageBuffer3dItem>();

        float pixelRes3D;
        public float PixelRes3D
        {
            get { return pixelRes3D; }
            set { pixelRes3D = value; }
        }

        public ImageBuffer()
        {
        }

        static string image2dNameFormat = "Image_C{0:00}_S{1:000}_L{2:00}.{3}";
        public static string Image2dNameFormat
        {
            get { return image2dNameFormat; }
            set { image2dNameFormat = value; }
        }

        public static string GetImage2dFileName(int cameraIndex, int stepNo, int lightTypeIndex, ImageFormat imageFormat)
        {
            return GetImage2dFileName(cameraIndex, stepNo, lightTypeIndex, imageFormat, image2dNameFormat);
        }

        public static string GetImage2dFileName(int cameraIndex, int stepNo, int lightTypeIndex, ImageFormat imageFormat, string imageFormatStr)
        {
            return string.Format(image2dNameFormat, cameraIndex, stepNo, lightTypeIndex, imageFormat.ToString());
        }

        static string image3dNameFormat = "Image_C{0:00}_S{1:000}_D{{0:000}}.bmp";
        public static string Image3dNameFormat
        {
            get { return image3dNameFormat; }
            set { image3dNameFormat = value; }
        }

        public static string GetImage3dFileName(int cameraIndex, int stepNo)
        {
            return String.Format(image3dNameFormat, cameraIndex, stepNo);
        }

        public IEnumerator<ImageBuffer2dItem> GetEnumerator()
        {
            return imageBuffer2dItemList.GetEnumerator();
        }

        public void Initialize(int numDevice, int numLightType)
        {
            this.numDevice = numDevice;
            this.numLightType = numLightType;

            imageBuffer2dItemList.Clear();
            for (int i = 0; i < numDevice * numLightType; i++)
                imageBuffer2dItemList.Add(new ImageBuffer2dItem());

            for (int i = 0; i < numDevice; i++)
                imageBuffer3dItemList.Add(new ImageBuffer3dItem());
        }

        public void ResetReadyFlag()
        {
            foreach (ImageBuffer2dItem imageCell in imageBuffer2dItemList)
            {
                imageCell.Ready = false;
            }
        }

        public List<ImageD> GetImageList()
        {
            List<ImageD> imageList = new List<ImageD>();

            for (int index = 0; index < (numDevice + numLightType); index++)
            {
                imageList.Add(imageBuffer2dItemList[index].Image);
            }

            return imageList;
        }

        public DeviceImageSet GetDeviceImageSet(int deviceIndex)
        {
            DeviceImageSet deviceImageSet = new DeviceImageSet();

            int startIndex = deviceIndex * numLightType;
            for (int index = startIndex; index < (startIndex + numLightType); index++)
            {
                deviceImageSet.ImageList2D.Add(imageBuffer2dItemList[index].Image);
#if DEBUG
                string savePath = string.Format("{0}\\{1}.bmp", Configuration.TempFolder, index.ToString());
                imageBuffer2dItemList[index].Image.SaveImage(savePath, ImageFormat.Bmp);
#endif
            }

            deviceImageSet.SetImage3D(imageBuffer3dItemList[deviceIndex]);

            return deviceImageSet;
        }

        public void Set2dImage(int deviceIndex, int lightTypeIndex, Image2D image2d)
        {
            ImageBuffer2dItem bufferItem = GetImageBuffer2dItem(deviceIndex, lightTypeIndex);
            if (bufferItem == null)
            {
                return;
            }
            bufferItem.Image = image2d;
        }

        public void Init3dImage(int deviceIndex, int num3dImage, int width, int height)
        {
            imageBuffer3dItemList[deviceIndex].Initialize(num3dImage, width, height);
        }

        public ImageBuffer2dItem GetImageBuffer2dItem(int deviceIndex, int lightTypeIndex)
        {
            int index = deviceIndex * numLightType + lightTypeIndex;
            if (index < imageBuffer2dItemList.Count)
                return imageBuffer2dItemList[index];

            return null;
        }

        public ImageBuffer3dItem GetImageBuffer3dItem(int deviceIndex)
        {
            if (deviceIndex < imageBuffer3dItemList.Count)
                return imageBuffer3dItemList[deviceIndex];

            return null;
        }

        // 이미지 버퍼에서 사용되고 있는 조명의 수를 반환한다.
        public LightParamList GetLightValueList(int lightTypeIndex)
        {
            LightParamList lightValueList = new LightParamList();

            for (int index = 0; index < imageBuffer2dItemList.Count; index++)
            {
                if ((index % numLightType) == lightTypeIndex)
                {
                    LightParam lightParam = imageBuffer2dItemList[index].LightParam;
                    if (lightParam != null)
                    {
                        lightValueList.AddLightValue(lightParam);
                    }
                }
            }

            return lightValueList;
        }

        public void DebugOutput(ImageFormat imageFormat)
        {
            LogHelper.Debug(LoggerType.Grab, "Begin save garb images");

            string grabImagePath = String.Format("{0}\\GrabImage", Configuration.TempFolder);
            Directory.CreateDirectory(grabImagePath);

            int index = 0;
            foreach (ImageBuffer2dItem imageBufferItem in imageBuffer2dItemList)
            {
                string fileName = String.Format("{0}\\GrabImage\\Image_{1:00}.bmp", Configuration.TempFolder, index);
                imageBufferItem.Image.SaveImage(fileName, ImageFormat.Bmp);

                index++;
            }

            LogHelper.Debug(LoggerType.Grab, "End save garb images");
        }

        public void Save(string folder, int stepNo, ImageFormat imageFormat)
        {
            for (int index = 0; index < imageBuffer2dItemList.Count; index++)
            {
                int cameraIndex = index / numLightType;
                int lightTypeIndex = index % numLightType;

                string fileName = GetImage2dFileName(cameraIndex, stepNo, lightTypeIndex, imageFormat);
                string filePath = Path.Combine(folder, fileName);
                imageBuffer2dItemList[index].SaveImage(filePath, imageFormat);
            }

            for (int deviceIndex = 0; deviceIndex < imageBuffer3dItemList.Count; deviceIndex++)
            {
                string fileNameFormat = GetImage3dFileName(deviceIndex, stepNo);
                imageBuffer3dItemList[deviceIndex].SaveImage(folder, fileNameFormat);
            }
        }

        public void Save(string folder, int cameraIndex, int stepNo, int lightTypeIndex, ImageFormat imageFormat)
        {
            ImageBuffer2dItem imageBufferItem = GetImageBuffer2dItem(cameraIndex, lightTypeIndex);
            if (imageBufferItem != null)
            {
                string fileName = GetImage2dFileName(cameraIndex, stepNo, lightTypeIndex, imageFormat);
                string filePath = Path.Combine(folder, fileName);
                imageBufferItem.SaveImage(filePath, imageFormat);
            }

            string fileNameFormat = GetImage3dFileName(cameraIndex, stepNo);
            imageBuffer3dItemList[cameraIndex].SaveImage(folder, fileNameFormat);
        }

        public void Load(string folder, int stepNo)
        {
            for (int index = 0; index < imageBuffer2dItemList.Count; index++)
            {
                int cameraIndex = index / numLightType;
                int lightTypeIndex = index % numLightType;

                string searchImageFileName = GetImage2dFileName(cameraIndex, stepNo, lightTypeIndex, ImageFormat.Bmp, "*");

                string[] imageFiles = Directory.GetFiles(folder, searchImageFileName);
                if (imageFiles.Count() > 0)
                {
                    string imageFilePath = Path.Combine(folder, imageFiles[0]);
                    imageBuffer2dItemList[cameraIndex].LoadImage(imageFilePath);
                }
                else
                {
                    imageBuffer2dItemList[cameraIndex].Image.Clear();
                }
            }

            for (int deviceIndex = 0; deviceIndex < imageBuffer3dItemList.Count; deviceIndex++)
            {
                string fileNameFormat = GetImage3dFileName(deviceIndex, stepNo);
                imageBuffer3dItemList[deviceIndex].LoadImage(folder, fileNameFormat);
            }
        }

        public void Load(string folder, int cameraIndex, int stepNo, int lightTypeIndex)
        {
            ImageBuffer2dItem imageBufferItem = GetImageBuffer2dItem(cameraIndex, lightTypeIndex);
            if (imageBufferItem != null)
            {
                string searchImageFileName = GetImage2dFileName(cameraIndex, stepNo, lightTypeIndex, ImageFormat.Bmp, "*");

                string[] imageFiles = Directory.GetFiles(folder, searchImageFileName);
                if (imageFiles.Count() > 0)
                {
                    string imageFilePath = Path.Combine(folder, imageFiles[0]);
                    imageBuffer2dItemList[cameraIndex].LoadImage(imageFilePath);
                }
                else
                {
                    imageBuffer2dItemList[cameraIndex].Image?.Clear();
                }
            }

            string fileNameFormat = GetImage3dFileName(cameraIndex, stepNo);
            imageBuffer3dItemList[cameraIndex].LoadImage(folder, fileNameFormat);
        }
    }
}
