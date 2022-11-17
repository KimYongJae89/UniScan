using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

using DynMvp.Base;

using Cognex.VisionPro;
using System.Threading.Tasks;

namespace DynMvp.Vision.Cognex
{
    public class CognexImageBuilder : ImageBuilder
    {
        public override AlgoImage Build(ImageType imageType, int width, int height)
        {
            throw new NotImplementedException();
        }

        public override AlgoImage Build(ImageD image, ImageType imageType, ImageBandType imageBand = ImageBandType.Luminance)
        {
            AlgoImage algoImage = null;

            switch (imageType)
            {
                case ImageType.Grey:
                    algoImage = BuildGreyImage(image, imageBand);
                    break;
                case ImageType.Color:
                    algoImage = BuildColorImage(image);
                    break;
                default:
                    throw new InvalidTypeException();
            }

            return algoImage;
        }

        private AlgoImage BuildGreyImage(ImageD image, ImageBandType imageBand)
        {
            CognexGreyImage cognexGreyImage = new CognexGreyImage();

            Bitmap bitmap = image.ToBitmap();

            if (image.NumBand == 3)
            {
                CogImage24PlanarColor cogColorImage = new CogImage24PlanarColor(bitmap);

                switch (imageBand)
                {
                    case ImageBandType.Luminance:
                        cognexGreyImage.Image = (CogImage8Grey)CogImageConvert.GetIntensityImage(cogColorImage, 0, 0, cogColorImage.Width, cogColorImage.Height);
                        break;
                    case ImageBandType.Red:
                        cognexGreyImage.Image = (CogImage8Grey)CogImageConvert.GetIntensityImageFromWeightedRGB(cogColorImage, 0, 0, cogColorImage.Width, cogColorImage.Height, 1, 0, 0);
                        break;
                    case ImageBandType.Green:
                        cognexGreyImage.Image = (CogImage8Grey)CogImageConvert.GetIntensityImageFromWeightedRGB(cogColorImage, 0, 0, cogColorImage.Width, cogColorImage.Height, 0, 1, 0);
                        break;
                    case ImageBandType.Blue:
                        cognexGreyImage.Image = (CogImage8Grey)CogImageConvert.GetIntensityImageFromWeightedRGB(cogColorImage, 0, 0, cogColorImage.Width, cogColorImage.Height, 0, 0, 1);
                        break;
                }
            }
            else
            {
                cognexGreyImage.Image = new CogImage8Grey(bitmap);
            }

            bitmap.Dispose();

            return cognexGreyImage;
        }

        private AlgoImage BuildColorImage(ImageD image)
        {
            CognexColorImage cognexColorImage = new CognexColorImage();


            if (image.NumBand == 3)
            {
                Bitmap bitmap = image.ToBitmap();
                cognexColorImage.Image = new CogImage24PlanarColor(bitmap);
                bitmap.Dispose();
            }
            else
            {
                Debug.Assert(false, "The image is not color image");
            }

            return cognexColorImage;
        }

        public static CogImage8Grey ConvertGreyImage(ImageD image)
        {
            return new CogImage8Grey(image.ToBitmap());
        }

        public static CogImage24PlanarColor ConvertColorImage(ImageD image)
        {
            return new CogImage24PlanarColor(image.ToBitmap());
        }

        public static ImageD ConvertImage(CogImage8Grey cogImage)
        {
            ICogImage8PixelMemory pixelMemory = cogImage.Get8GreyPixelMemory(CogImageDataModeConstants.Read, 0, 0, cogImage.Width, cogImage.Height);

            Image2D image2d = new Image2D();
            image2d.Initialize(pixelMemory.Width, pixelMemory.Height, 1, pixelMemory.Stride);

            image2d.SetData(pixelMemory.Scan0);
            pixelMemory.Dispose();
                
            return image2d;
        }

        public static ImageD ConvertImage(CogImage24PlanarColor cogImage)
        {
            ICogImage8PixelMemory pixelMemory0, pixelMemory1, pixelMemory2;
            cogImage.Get24PlanarColorPixelMemory(CogImageDataModeConstants.Read, 0, 0, cogImage.Width, cogImage.Height, out pixelMemory0, out pixelMemory1, out pixelMemory2);

            Image2D image2d = new Image2D();
            image2d.Initialize(pixelMemory0.Width, pixelMemory0.Height, 3, pixelMemory0.Stride);

            int size = cogImage.Width * cogImage.Height;

            unsafe
            {
                byte* pixelMemory0Ptr = (byte*)pixelMemory0.Scan0.ToPointer();
                byte* pixelMemory1Ptr = (byte*)pixelMemory1.Scan0.ToPointer();
                byte* pixelMemory2Ptr = (byte*)pixelMemory2.Scan0.ToPointer();

                byte[] imageData = image2d.Data;
                Parallel.For(0, size, pos =>
                {
                    imageData[pos * 3] = pixelMemory0Ptr[pos];
                    imageData[pos * 3 + 1] = pixelMemory1Ptr[pos];
                    imageData[pos * 3 + 2] = pixelMemory2Ptr[pos];
                });
            }

            pixelMemory0.Dispose();
            pixelMemory1.Dispose();
            pixelMemory2.Dispose();

            return image2d;
        }

        public override bool IsCudaAvailable()
        {
            return false;
        }

        //protected override AlgoImage ConvertFromOpenCv(AlgoImage algoImage, ImageType imageType)
        //{
        //    ICogImage8PixelMemory pixelMemory = image.Get8GreyPixelMemory(CogImageDataModeConstants.Read, 0, 0, image.Width, image.Height);

        //    switch (imageType)
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
