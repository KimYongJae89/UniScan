using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;

using DynMvp.Base;

using Matrox.MatroxImagingLibrary;
using System.Threading.Tasks;

namespace DynMvp.Vision.Matrox
{
    public class MilImageBuilder : ImageBuilder
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public override AlgoImage Build(ImageType imageType, int width, int height)
        {
            MilImage milImage = null;

            switch (imageType)
            {
                case ImageType.Grey:
                    MilGreyImage milGreyImage = new MilGreyImage();
                    milGreyImage.Alloc(width, height);
                    MIL.MbufClear(milGreyImage.Image, MIL.M_COLOR_BLACK);
                    milImage = milGreyImage;
                    break;
                case ImageType.Color:
                    MilColorImage milColorImage = new MilColorImage();
                    milColorImage.Alloc(width, height);
                    MIL.MbufClear(milColorImage.Image, MIL.M_COLOR_BLACK);
                    milImage = milColorImage;
                    break;
                case ImageType.Depth:
                    MilDepthImage milDepthImage = new MilDepthImage();
                    milDepthImage.Alloc(width, height);
                    MIL.MbufClear(milDepthImage.Image, MIL.M_COLOR_BLACK);
                    milImage = milDepthImage;
                    break;
                case ImageType.Binary:
                    MilBinalImage milBinalImage = new MilBinalImage();
                    milBinalImage.Alloc(width, height);
                    MIL.MbufClear(milBinalImage.Image, MIL.M_COLOR_BLACK);
                    milImage = milBinalImage;
                    break;
                case ImageType.Gpu:
                    MilGpuImage milGpuImage = new MilGpuImage();
                    milGpuImage.Alloc(width, height);
                    MIL.MbufClear(milGpuImage.Image, MIL.M_COLOR_BLACK);
                    milImage = milGpuImage;
                    break;
                default:
                    throw new InvalidTypeException();
            }

            return milImage;
        }

        public override AlgoImage Build(ImageD image, ImageType imageType,  ImageBandType imageBand = ImageBandType.Luminance)
        {
            MilImage milImage = null;

            switch (imageType)
            {
                case ImageType.Grey:
                     milImage = BuildGreyImage(image, imageBand);
                    break;
                case ImageType.Color:
                    milImage = BuildColorImage(image);
                    break;
                case ImageType.Depth:
                    milImage = BuildDepthImage(image);
                    break;
                default:
                    throw new InvalidTypeException();
            }

            if (milImage.Image == MIL.M_NULL)
                throw new Exception("MilImageBuilder::Build - Build Fail.");

            //Devices.MilObjectManager.Instance.AddObject(milImage);
            return milImage;
        }

        private MilImage BuildGreyImage(ImageD image, ImageBandType imageBand)
        {
            if (image.NumBand == 3)
            {
                MilColorImage milColorImage = ConvertColorImage(image);
                
                return milColorImage.Clone(imageBand);
            }
            else
            {
                return ConvertGreyImage(image);
            }
        }

        private MilImage BuildDepthImage(ImageD image)
        {
            return ConvertDepthImage(image);
        }

        private MilGreyImage ConvertGreyImage(ImageD image)
        {
            System.Diagnostics.Debug.Assert(image.NumBand == 1);

            int width = image.Width;
            int height = image.Height;

            Image2D image2d = (Image2D)image;
            MilGreyImage milGreyImage = new MilGreyImage();

            if (image2d.IsUseIntPtr())
                milGreyImage.Alloc(width, height, image2d.DataPtr, image.Pitch);
            else
            {
                byte[] imageBuf = image2d.Data;
                if (imageBuf == null)
                    throw new InvalidTypeException();

                milGreyImage.Alloc(width, height);

                int bufSize = width * height;
                if (imageBuf.Length == bufSize)
                {
                    milGreyImage.Put(imageBuf);
                }
                else
                {
                    byte[] milBuf = new byte[width * height];
                    Parallel.For(0, height, y => Array.Copy(imageBuf, image.Pitch * y, milBuf, width * y, width));

                    //for (int y = 0; y < height; y++)
                    //    Array.Copy(imageBuf, image.Pitch * y, milBuf, width * y, width);

                    milGreyImage.Put(milBuf);
                }
            }

            return milGreyImage;
        }

        private MilDepthImage ConvertDepthImage(ImageD image)
        {
            int width = image.Width;
            int height = image.Height;

            MilDepthImage milDepthImage = new MilDepthImage();
            milDepthImage.Alloc(width, height);

            Image3D image3d = (Image3D)image;
            float[] imageBuf = image3d.Data;

            float[] milBuf = new float[width * height];

            Parallel.For(0, height, y =>
            { Array.Copy(imageBuf, image.Pitch * y, milBuf, width * y, width); });

            //for (int y = 0; y < height; y++)
            //    Array.Copy(imageBuf, image.Pitch * y, milBuf, width * y, width);

            milDepthImage.Put(milBuf);

            return milDepthImage;
        }

        private MilImage BuildColorImage(ImageD image)
        {
            return ConvertColorImage(image);
        }

        public static ImageD ConvertImage(MilBinalImage milBinalImage)
        {
            if (milBinalImage == null || milBinalImage.Image == MIL.M_NULL)
                return null;

            int width = milBinalImage.Width;
            int height = milBinalImage.Height;

            MilGreyImage milGreyImage = new MilGreyImage(width, height);
            milGreyImage.Copy(milBinalImage);

            ImageD imageD = MilImageBuilder.ConvertImage(milGreyImage);
            milGreyImage.Dispose();
            return imageD;
        }

        private MilColorImage ConvertColorImage(ImageD image)
        {
            int width = image.Width;
            int height = image.Height;
            int bufPitch = width * 3; //  milColorImage.Pitch;

            MilColorImage milColorImage = new MilColorImage();
            milColorImage.Alloc(width, height);

            Image2D image2d = (Image2D)image;
            byte[] imageBuf = image2d.Data;

            byte[] milBuf = new byte[bufPitch * height];

            Parallel.For(0, height, y =>
            { Array.Copy(imageBuf, image.Pitch * y, milBuf, bufPitch * y, width*3); });

            //for (int y = 0; y < height; y++)
            //    Array.Copy(imageBuf, image.Pitch * y, milBuf, bufPitch * y, width*3);

            milColorImage.Put(milBuf);

            return milColorImage;
        }

        public static ImageD ConvertImage(MilGreyImage milGreyImage)
        {
            if (milGreyImage == null || milGreyImage.Image == null)
                return null;

            int width = milGreyImage.Width;
            int height = milGreyImage.Height;
            int milPitch = milGreyImage.Pitch;
            int pitch = (width + 3) / 4 * 4;

            byte[] milBuf = new byte[width * height];
            milGreyImage.Get(milBuf);

            Image2D image = new Image2D();
            if (width == pitch)
            {
                image.Initialize(width, height, 1, pitch, milBuf);
            }
            else
            {
                byte[] imageData = new byte[pitch*height];
                Parallel.For(0, height, y =>
                //Array.Copy(milBuf, width * y, imageData, pitch * y, width)
                Buffer.BlockCopy(milBuf, width * y, imageData, pitch * y, width)
                );
                image.Initialize(width, height, 1, pitch, imageData);
            }

            //byte[] imageBuf = image.Data;

            //byte[] milBuf = new byte[width * height];
            //milGreyImage.Get(milBuf);


            return image;
        }

        public static ImageD ConvertImage(MilDepthImage milDepthImage)
        {
            if (milDepthImage == null || milDepthImage.Image == null)
                return null;

            int width = milDepthImage.Width;
            int height = milDepthImage.Height;

            int size = width * height;

            Image3D image = new Image3D();
            image.Initialize(width, height, 1);

            float[] imageBuf = image.Data;
            float[] milBuf = new float[width * height];

            milDepthImage.Get(milBuf);

            Array.Copy(milBuf, imageBuf, width * height);

            return image;
        }

        public static ImageD ConvertImage(MilColorImage milColorImage)
        {
            if (milColorImage == null || milColorImage.Image == null)
                return null;

            int width = milColorImage.Width;
            int pitch = milColorImage.Pitch;
            int height = milColorImage.Height;
            int band = 3;

            int size = width * height * band;
            int pp = width * band;
            int pp2 = pitch * band;

            Image2D image = new Image2D();
            image.Initialize(width, height, band, width * band);
            byte[] imageBuf = image.Data;

            byte[] milBuf = new byte[imageBuf.Length];
            milColorImage.Get(milBuf);

            Array.Copy(milBuf, imageBuf, size);

            return image;
        }

        public static ImageD ConvertImage(MilGpuImage milGpuImage)
        {
            throw new NotImplementedException();
        }

        public override bool IsCudaAvailable()
        {
            return false;
        }

        //protected override AlgoImage ConvertFromOpenCv(AlgoImage algoImage, ImageType imageType)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage ConvertFromEuresysOpenEVision(AlgoImage algoImage, ImageType imageType)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage ConvertFromCognexVisionPro(AlgoImage algoImage, ImageType imageType)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage ConvertFromMatroxMIL(AlgoImage algoImage, ImageType imageType)
        //{
        //    if (algoImage.ImageType == imageType)
        //        return algoImage.Clone();

        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage ConvertFromHalcon(AlgoImage algoImage, ImageType imageType)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage ConvertFromCustom(AlgoImage algoImage, ImageType imageType)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
