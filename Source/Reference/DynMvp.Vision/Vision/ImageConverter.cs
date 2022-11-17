using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision
{
    public class ConvertPack : IDisposable
    {
        public ImageType ImageType { get; set; }

        public int Width { get => Size.Width; }
        public int Height { get => Size.Height; }

        public int NumBand { get => this.ImageType == ImageType.Color ? 3 : 1; }
        public int BitPerPixel { get; set; }

        public Size Size { get; set; }

        public int Pitch { get; set; }

        public byte[] Bytes { get => this.bytes; set => this.bytes = value; }
        protected byte[] bytes;

        public IntPtr Ptr { get; set; }

        public void Dispose()
        {
            if (this.bytes != null)
                Array.Resize(ref this.bytes, 0);
        }
    }

    public abstract class ImageConverter
    {
        static ImageConverter openCvImageConverter = null;
        private static ImageConverter OpenCvImageConverter
        {
            get
            {
                if (openCvImageConverter == null)
                    openCvImageConverter = new OpenCv.ImageConverter();
                return openCvImageConverter;
            }
        }


        static ImageConverter matroxImageConverter = null;
        private static ImageConverter MatroxImageConverter
        {
            get
            {
                if (matroxImageConverter == null)
                    matroxImageConverter = new Matrox.ImageConverter();
                return matroxImageConverter;
            }
        }

        private static ImageConverter GetInstance(ImagingLibrary libraryType)
        {
            switch (libraryType)
            {
                case ImagingLibrary.OpenCv:
                    return OpenCvImageConverter;
                case ImagingLibrary.EuresysOpenEVision:
                    break;
                case ImagingLibrary.CognexVisionPro:
                    break;
                case ImagingLibrary.MatroxMIL:
                    return MatroxImageConverter;
                case ImagingLibrary.Halcon:
                    break;
                case ImagingLibrary.Custom:
                    break;
            }

            throw new NotImplementedException();
        }

        public static AlgoImage Convert(AlgoImage algoImage, ImagingLibrary imagingLibrary, ImageType imageType)
        {
            //algoImage.Save(@"C:\temp\Convert_1.bmp");
            ConvertPack pack = GetInstance(algoImage.LibraryType).Pack(algoImage, false);
            AlgoImage convert = GetInstance(imagingLibrary).Unpack(pack, imageType);
            pack.Dispose();
            //convert.Save(@"C:\temp\Convert_2.bmp");
            return convert;
        }

        public static void Convert(AlgoImage srcAlgoImage, AlgoImage dstAlgoImage)
        {
            //srcAlgoImage.Save(@"C:\temp\srcAlgoImage.bmp");
            ConvertPack pack = GetInstance(srcAlgoImage.LibraryType).Pack(srcAlgoImage, true);
            srcAlgoImage.WaitStreamComplete();
            GetInstance(dstAlgoImage.LibraryType).Unpack(pack, dstAlgoImage);
            dstAlgoImage.WaitStreamComplete();
            pack.Dispose();
            //dstAlgoImage.Save(@"C:\temp\dstAlgoImage.bmp");
        }

        public abstract ConvertPack Pack(AlgoImage algoImage, bool usePtr);
        public abstract AlgoImage Unpack(ConvertPack convertPack, ImageType imageType);
        public abstract void Unpack(ConvertPack convertPack, AlgoImage algoImage);
    }
}
