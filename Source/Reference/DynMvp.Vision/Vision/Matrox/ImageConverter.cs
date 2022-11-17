using DynMvp.Base;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Vision.Matrox
{
    public class ImageConverter : DynMvp.Vision.ImageConverter
    {
        public override ConvertPack Pack(AlgoImage algoImage, bool usePtr)
        {
            ConvertPack pack = new ConvertPack();
            pack.ImageType = algoImage.ImageType;
            pack.Size = algoImage.Size;
            pack.Pitch = algoImage.Pitch;
            if (usePtr)
                pack.Ptr = algoImage.Ptr;
            else
                pack.Bytes = algoImage.GetByte();

            return pack;
        }

        public override AlgoImage Unpack(ConvertPack convertPack, ImageType imageType)
        {
            if (convertPack.ImageType != imageType)
            // 포멧 변환 필요함
            {
                return Unpack2(convertPack, imageType);
            }
            else
            // 포멧 변환 필요 없음
            {
                Image2D image2D = null;
                if (convertPack.Ptr == null)
                    image2D = new Image2D(convertPack.Width, convertPack.Height, convertPack.NumBand, convertPack.Pitch, convertPack.Bytes);
                else
                    image2D = new Image2D(convertPack.Width, convertPack.Height, convertPack.NumBand, convertPack.Pitch, convertPack.Ptr);
                MilImage convertImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, image2D, imageType) as MilImage;
                return convertImage;
            }
        }

        private AlgoImage Unpack2(ConvertPack convertPack, ImageType imageType)
        {
            byte[] bytes = null;
            switch (imageType)
            {
                case ImageType.Grey:
                    {
                        return new MilGreyImage(convertPack);
                        bytes = new byte[convertPack.Width * convertPack.Height];
                        for (int y = 0; y < convertPack.Height; y++)
                            Array.Copy(convertPack.Bytes, y * convertPack.Pitch, bytes, y * convertPack.Width, convertPack.Width);
                    }
                    break;

                case ImageType.Color:
                    {
                        return new MilColorImage(convertPack);
                    }
                    break;
                case ImageType.Depth:
                    {
                        return new MilDepthImage(convertPack);
                        float[] datas = new float[convertPack.Width * convertPack.Height];
                        for (int y = 0; y < convertPack.Height; y++)
                            Array.Copy(convertPack.Bytes, y * convertPack.Pitch, datas, y * convertPack.Width, convertPack.Width);

                        bytes = new byte[4 * convertPack.Width * convertPack.Height];
                        Buffer.BlockCopy(datas, 0, bytes, 0, bytes.Length);
                        break;
                    }
                case ImageType.Binary:
                case ImageType.Gpu:
                    throw new NotImplementedException();
            }

            MilImage convertImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, imageType, convertPack.Size) as MilImage;
            convertImage.PutByte(bytes);
            return convertImage;
        }

        //private AlgoImage Unpack2(ConvertPack convertPack, ImageType imageType)
        //{
        //    MIL_ID tempMilObject = MIL.M_NULL;
        //    MIL_INT type = 0;
        //    MIL_INT attribute = MIL.M_IMAGE + MIL.M_PROC;
        //    switch (convertPack.ImageType)
        //    {
        //        case ImageType.Binary: type = 1 + MIL.M_UNSIGNED; break;
        //        case ImageType.Grey: type = 8 + MIL.M_UNSIGNED; break;
        //        case ImageType.Color: type = 24 + MIL.M_UNSIGNED; break;
        //        case ImageType.Depth: type = 32 + MIL.M_FLOAT; break;
        //        case ImageType.Gpu: type = 8 + MIL.M_UNSIGNED; break;
        //    }

        //    if (convertPack.Ptr == IntPtr.Zero)
        //    {
        //        tempMilObject = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, convertPack.Width, convertPack.Height, type, attribute, MIL.M_NULL);
        //        int tempPitch = convertPack.Width;
        //        if (convertPack.Pitch == tempPitch)
        //        {
        //            MIL.MbufPut(tempMilObject, convertPack.Bytes);
        //        }
        //        else
        //        {
        //            byte[] bytes = new byte[tempPitch * convertPack.Height];
        //            int lineLength = Math.Min(convertPack.Pitch, tempPitch);
        //            for (int y = 0; y < convertPack.Height; y++)
        //            {
        //                int srcOffset = y * convertPack.Pitch;
        //                int dstOffset = y * tempPitch;
        //                Array.Copy(convertPack.Bytes, srcOffset, bytes, dstOffset, lineLength);
        //            }
        //            MIL.MbufPut(tempMilObject, bytes);
        //        }
        //    }
        //    else
        //    {
        //        tempMilObject = MIL.MbufCreate2d(MIL.M_DEFAULT_HOST, convertPack.Width, convertPack.Height, type, attribute, MIL.M_HOST_ADDRESS + MIL.M_PITCH_BYTE, convertPack.Pitch, (ulong)convertPack.Ptr, MIL.M_NULL);
        //    }

        //    MilImage convertImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, imageType, convertPack.Size) as MilImage;
        //    MIL.MbufCopy(tempMilObject, convertImage.Image);

        //    MIL.MbufFree(tempMilObject);

        //    return convertImage;
        //}

        public override void Unpack(ConvertPack convertPack, AlgoImage algoImage)
        {
            if (convertPack.Ptr == IntPtr.Zero)
                algoImage.PutByte(convertPack.Bytes);
            else
                algoImage.PutByte(convertPack.Ptr, convertPack.Pitch);
        }
    }
}
