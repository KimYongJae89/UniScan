using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.OpenCv
{
    internal class ImageConverter : DynMvp.Vision.ImageConverter
    {
        public ImageConverter() { }

        public override ConvertPack Pack(AlgoImage algoImage, bool usePtr)
        {
            ConvertPack pack = new ConvertPack();
            pack.ImageType = algoImage.ImageType;
            pack.BitPerPixel = algoImage.BitPerPixel;
            pack.Size = algoImage.Size;
            pack.Pitch = algoImage.Pitch;

            if (usePtr)
                pack.Ptr = algoImage.Ptr;
            else
                pack.Bytes = algoImage.GetByte();

            if (pack.ImageType == ImageType.Gpu)
                pack.ImageType = ImageType.Grey;
            return pack;
        }

        public override AlgoImage Unpack(ConvertPack convertPack, ImageType imageType)
        {
            AlgoImage algoImage = null;
            switch (imageType)
            {
                case ImageType.Grey:
                    algoImage = new OpenCvGreyImage(convertPack);
                    break;
                case ImageType.Color:
                    algoImage = new OpenCvColorImage(convertPack);
                    break;
                case ImageType.Depth:
                    algoImage = new OpenCvDepthImage(convertPack);
                    break;
                case ImageType.Gpu:
                    algoImage = new OpenCvCudaImage(convertPack);
                    break;
                default:
                    throw new NotImplementedException();
            }
            
            return algoImage;
        }

        public override void Unpack(ConvertPack convertPack, AlgoImage algoImage)
        {
            System.Diagnostics.Debug.Assert(algoImage.Size == algoImage.Size);
                if (convertPack.Ptr != IntPtr.Zero)
                    algoImage.PutByte(convertPack.Ptr, convertPack.Pitch);
                else
                    algoImage.PutByte(convertPack.Bytes);
        }
    }
}
