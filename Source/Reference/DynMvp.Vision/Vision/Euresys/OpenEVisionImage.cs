using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

using Euresys.Open_eVision_1_2;

using DynMvp.Base;

namespace DynMvp.Vision.Euresys
{
    public abstract class OpenEVisionImage : AlgoImage
    {
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
    }

    public class OpenEVisionGreyImage : OpenEVisionImage
    {

        private EImageBW8 image;
        public EImageBW8 Image
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

        public OpenEVisionGreyImage()
        {
            LibraryType = ImagingLibrary.EuresysOpenEVision;
            ImageType = ImageType.Grey;
        }

        protected override void Disposing()
        {
            if (image != null)
                image.Dispose();
        }

        public override void Clear(byte initVal)
        {

        }

        public override IntPtr GetImagePtr()
        {
            throw new NotImplementedException();
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

        public override AlgoImage Clone()
        {
            OpenEVisionGreyImage cloneImage = new OpenEVisionGreyImage();
            cloneImage.Image = new EImageBW8();
            cloneImage.Image.SetSize(image.Width, image.Height);

            EasyImage.Copy(image, cloneImage.Image);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            DrawingHelper.Arrange(rectangle, new Size(image.Width, image.Height));

            EROIBW8 roi = new EROIBW8();
            roi.Attach(image);
            roi.SetPlacement(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);

            OpenEVisionGreyImage clipImage = new OpenEVisionGreyImage();
            clipImage.Image = new EImageBW8();
            clipImage.Image.SetSize(rectangle.Width, rectangle.Height);

            EasyImage.Copy(roi, clipImage.Image);

            roi.Dispose();

            clipImage.FilteredList = FilteredList;

            return clipImage;
        }


        public override ImageD ToImageD()
        {
            return OpenEVisionImageBuilder.ConvertImage(image);
        }
        
        public override void Save(string fileName)
        {
            image.Save(fileName, EImageFileType.Auto);
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

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
        }
    }

    public class OpenEVisionColorImage : OpenEVisionImage
    {

        private EImageC24 image;
        public EImageC24 Image
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

        public OpenEVisionColorImage()
        {
            LibraryType = ImagingLibrary.EuresysOpenEVision;
            ImageType = ImageType.Color;
        }

        public override IntPtr GetImagePtr()
        {
            throw new NotImplementedException();
        }

        protected override void Disposing()
        {
            if (image != null)
                image.Dispose();
        }

        public override void Clear(byte initVal)
        {

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
        
        public override AlgoImage Clone()
        {
            OpenEVisionColorImage cloneImage = new OpenEVisionColorImage();
            cloneImage.Image = new EImageC24();
            cloneImage.Image.SetSize(image.Width, image.Height);

            return cloneImage;
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            OpenEVisionColorImage cloneImage = new OpenEVisionColorImage();
            cloneImage.Image = new EImageC24();
            cloneImage.Image.SetSize(rectangle.Width, rectangle.Height);

            return cloneImage;
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
            return OpenEVisionImageBuilder.ConvertImage(image);
        }

        public override void Save(string fileName)
        {
            image.Save(fileName, EImageFileType.Auto);
        }
        
        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
        }
    }
}
