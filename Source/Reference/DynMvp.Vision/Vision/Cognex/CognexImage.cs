using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

using Cognex.VisionPro;
using Cognex.VisionPro.ImageFile;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using DynMvp.Base;

using Emgu.CV;
using Emgu.CV.Structure;
using DynMvp.Vision.OpenCv;
using System.Diagnostics;

namespace DynMvp.Vision.Cognex
{
    public class CogImageSaver
    {
        public static void Save(ICogImage cogImage, string fileName)
        {
            CogImageFile imageFile = new CogImageFile();
            imageFile.Open(fileName, CogImageFileModeConstants.Write);
            imageFile.Append(cogImage);
            imageFile.Close();
        }
    }

    public class CognexGreyImage : AlgoImage
    {

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);


        private CogImage8Grey image;
        public CogImage8Grey Image
        {
            get { return image; }
            set { image = value; }
        }

        public override int Width
        {
            get { return image.Width; }
        }

        public override int Pitch
        {
            get { return image.Width; }
        }

        public override int Height
        {
            get { return image.Height; }
        }

        public override int BitPerPixel => throw new NotImplementedException();

        public override bool IsAllocated => this.image != null;

        public CognexGreyImage()
        {
            LibraryType = ImagingLibrary.CognexVisionPro;
            ImageType = ImageType.Grey;
        }

        protected override void Disposing()
        {

        }

        public override void Clear(byte initVal)
        {

        }

        public override AlgoImage Clone()
        {
            CognexGreyImage cloneImage = new CognexGreyImage();
            cloneImage.Image = image.Copy(CogImageCopyModeConstants.CopyPixels);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        //public override AlgoImage Clone(ImageType imageType)
        //{
        //    AlgoImage cloneImage = null;
        //    switch (imageType)
        //    {
        //        case ImageType.Grey:
        //            cloneImage = this.Clone();
        //            break;
        //        case ImageType.Color:
        //            cloneImage = this.Convert2ColorImage();
        //            break;
        //    }
        //    if (cloneImage == null)
        //        throw new NotImplementedException();

        //    return cloneImage;
        //}

        public override AlgoImage Clip(Rectangle rectangle)
        {
            CogImage8Grey cogImage = (CogImage8Grey)CogImageConvert.GetIntensityImage(image, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

            CognexGreyImage cloneImage = new CognexGreyImage();
            cloneImage.Image = cogImage.Copy(CogImageCopyModeConstants.CopyPixels);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override IntPtr GetImagePtr()
        {
            throw new NotImplementedException();
        }

        public override void GetByte(byte[] bytes)
        {
            Debug.Assert(bytes.Length == (image.Width * image.Height));

            int index = 0;

            for (int yIndex = 0; yIndex < image.Height; yIndex++)
            {
                for (int xIndex = 0; xIndex < image.Width; xIndex++)
                {
                    bytes[index++] = image.GetPixel(xIndex, yIndex);
                }
            }
        }

        public override void PutByte(byte[] data)
        {
            int index = 0;

            for (int yIndex = 0; yIndex < image.Height; yIndex++)
            {
                for (int xIndex = 0; xIndex < image.Width; xIndex++)
                {
                    image.SetPixel(xIndex, yIndex, data[index++]);
                }
            }
        }

        public override void PutByte(IntPtr ptr, int pitch)
        {
            throw new NotImplementedException();
        }

        public override ImageD ToImageD()
        {
            return CognexImageBuilder.ConvertImage(image);
        }

        public override void Save(string fileName)
        {
            CogImageSaver.Save(image, fileName);
        }
        
        //public override AlgoImage ConvertToOpenCvImage(ImageType imageType)
        //{
        //    ICogImage8PixelMemory pixelMemory = image.Get8GreyPixelMemory(CogImageDataModeConstants.Read, 0, 0, image.Width, image.Height);

        //    switch(imageType)
        //    {
        //        case ImageType.Grey:
        //            OpenCv.OpenCvGreyImage openCvGreyImage = new OpenCv.OpenCvGreyImage();
        //            openCvGreyImage.Image = new Image<Gray, Byte>(pixelMemory.Width, pixelMemory.Height);
        //            for (int j = 0; j < image.Height; j++)
        //                for (int i = 0; i < image.Width; i++)
        //                    openCvGreyImage.Image.Data[j, i, 0] = image.GetPixel(i, j);

        //            openCvGreyImage.FilteredList = FilteredList;

        //            return openCvGreyImage;
        //        default:
        //            throw new NotImplementedException();
        //    }
        //}

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
        }

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            throw new NotImplementedException();
        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {
            throw new NotImplementedException();
        }

        public override void MaskingCopy(AlgoImage srcImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        //protected override AlgoImage Convert2GreyImage()
        //{
        //    return this.Clone();
        //}

        //protected override AlgoImage Convert2ColorImage()
        //{
        //    throw new NotImplementedException();
        //}
    }

    public class CognexColorImage : AlgoImage
    {

        private CogImage24PlanarColor image;
        public CogImage24PlanarColor Image
        {
            get { return image; }
            set { image = value; }
        }


        public override int Width
        {
            get { return image.Width; }
        }

        public override int Pitch
        {
            get { return image.Width; }
        }

        public override int Height
        {
            get { return image.Height; }
        }

        public override int BitPerPixel => throw new NotImplementedException();

        public override bool IsAllocated => this.image != null;

        public CognexColorImage()
        {
            LibraryType = ImagingLibrary.CognexVisionPro;
            ImageType = ImageType.Color;
        }

        protected override void Disposing()
        {

        }

        public override void Clear(byte initVal)
        {

        }

        public override AlgoImage Clone()
        {
            CognexColorImage cloneImage = new CognexColorImage();
            cloneImage.Image = image.Copy(CogImageCopyModeConstants.CopyPixels);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        //public override AlgoImage Clone(ImageType imageType)
        //{
        //    AlgoImage cloneImage = null;
        //    switch (imageType)
        //    {
        //        case ImageType.Grey:
        //            cloneImage = this.Convert2GreyImage();
        //            break;
        //        case ImageType.Color:
        //            cloneImage = this.Clone();
        //            break;
        //    }
        //    if (cloneImage == null)
        //        throw new NotImplementedException();

        //    return cloneImage;
        //}

        public override AlgoImage Clip(Rectangle rectangle)
        {
            CogImage24PlanarColor cogImage = (CogImage24PlanarColor)CogImageConvert.GetRGBImage(image, rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

            CognexColorImage cloneImage = new CognexColorImage();
            cloneImage.Image = cogImage.Copy(CogImageCopyModeConstants.CopyPixels);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override IntPtr GetImagePtr()
        {
            throw new NotImplementedException();
        }

        public override void GetByte(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public override void PutByte(byte[] data)
        {
            throw new NotImplementedException();
        }

        public override void PutByte(IntPtr ptr, int pitch)
        {
            throw new NotImplementedException();
        }

        public override ImageD ToImageD()
        {
            return CognexImageBuilder.ConvertImage(image);
        }

        public override void Save(string fileName)
        {
            CogImageSaver.Save(image, fileName);
        }

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            throw new InvalidObjectException("[MilGreyImage.GetChildImage] float data type is not support");
        }

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            throw new NotImplementedException();
        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {
            throw new NotImplementedException();
        }

        public override void MaskingCopy(AlgoImage srcImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        //protected override AlgoImage Convert2GreyImage()
        //{
        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage Convert2ColorImage()
        //{
        //    return this.Clone();
        //}
    }
}
