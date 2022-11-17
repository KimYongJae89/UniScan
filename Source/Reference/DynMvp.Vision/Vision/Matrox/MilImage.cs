using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

using DynMvp.Base;
using Matrox.MatroxImagingLibrary;
using DynMvp.Vision.OpenCv;
using DynMvp.Devices;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DynMvp.Vision.Matrox
{
    public abstract class MilImage : AlgoImage, MilObject
    {
        public StackTrace StackTrace { get; private set; }

        protected MIL_ID image = MIL.M_NULL;
        public MIL_ID Image
        {
            get { return this.image; }
            //set { image = value; }
        }

        public override int Width
        {
            get { return this.width; }
            //get => (image == MIL.M_NULL) ? 0 : (int)MIL.MbufInquire(image, MIL.M_SIZE_X);
        }
        protected int width;

        public override int Height
        {
            get { return this.height; }
            //get => (image == MIL.M_NULL) ? 0 : (int)MIL.MbufInquire(image, MIL.M_SIZE_Y);
        }
        protected int height;

        public override int Pitch
        {
            //get { return this.pitch; }
            //get => (image == MIL.M_NULL) ? 0 : (int)MIL.MbufInquire(image, MIL.M_PITCH_BYTE);
            get => this.width * (this.BitPerPixel / 8);
        }
        //protected int pitch;

        // 암시적 형 변환
        public static implicit operator MIL_ID(MilImage img) => img.image;

        public override int BitPerPixel => (int)(MIL.MbufInquire(image, MIL.M_SIZE_BIT) * MIL.MbufInquire(image, MIL.M_SIZE_BAND));

        public override bool IsAllocated => this.image != MIL.M_NULL;

        public override int PitchLib => (int)MIL.MbufInquire(image, MIL.M_PITCH_BYTE);

        public MilImage() { }

        ~MilImage()
        {
            System.Diagnostics.Debug.Assert(image == MIL.M_NULL);

           // var tr= this.StackTrace;

            Dispose();
        }

        public void Free()
        {
            if (image != MIL.M_NULL)
            {
                MIL.MbufFree(image);
                image = MIL.M_NULL;
            }
        }

        protected override void Disposing()
        {
            if (this.SubImageList.Count > 0)
            {
                LogHelper.Error(LoggerType.Error, string.Format("MilImage Dispose but Subimage(s) is(are) exist. - {0}{1}{2}", this.SubImageList, Environment.NewLine, new System.Diagnostics.StackTrace().ToString()));
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("ChildImage Tree");
                LogSubImageProp(this, sb);
                LogHelper.Error(LoggerType.Inspection, sb.ToString());
            }

            System.Diagnostics.Debug.Assert(this.SubImageList.Count == 0, "SubImage is Exist!!"); 
            MilObjectManager.Instance.ReleaseObject(this);

            if (this.parentImage != null)
            {
                lock (this.parentImage.SubImageList)
                    this.parentImage.SubImageList.Remove(this);
                this.parentImage = null;
            }

            //this.stackTrace = null;
        }

        private void LogSubImageProp(AlgoImage algoImage, StringBuilder sb, int detpth = 0)
        {
            sb.AppendLine(string.Format("{0} N:{1}, W:{2}, H:{3}, T:{4}", new string('\t', detpth), algoImage.Name, algoImage.Width, algoImage.Height, algoImage.Tag?.ToString()));
            for (int i = 0; i < algoImage.SubImageList.Count; i++)
                LogSubImageProp(algoImage.SubImageList[i], sb, detpth + 1);
        }

        public override void Clear(byte initVal)
        {
            MIL.MbufClear(image, initVal);
        }

        public override AlgoImage Clone()
        {
            return Clip(new Rectangle(0, 0, width, height));
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            MilImage milAlgoImage = this.CreateInstance();
            milAlgoImage.Alloc(rectangle.Width, rectangle.Height);
            milAlgoImage.Copy(this, rectangle);
            return milAlgoImage;
        }

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            MilImage milSrcImage = MilImage.CheckMilImage(srcImage, "MilImage.Copy", "Source");

            MIL.MbufCopyColor2d(milSrcImage.Image, image, MIL.M_ALL_BAND, srcRect.Left, srcRect.Top, MIL.M_ALL_BAND, 0, 0, srcRect.Width, srcRect.Height);

            milSrcImage.FilteredList = FilteredList;
        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {
            MilImage milSrcImage = MilImage.CheckMilImage(srcImage, "MilImage.Copy", "Source");

            MIL.MbufCopyColor2d(milSrcImage.Image, image, MIL.M_ALL_BAND, srcPt.X, srcPt.Y, MIL.M_ALL_BAND, dstPt.X, dstPt.Y, size.Width, size.Height);

            FilteredList = milSrcImage.FilteredList;
        }
        public override void MaskingCopy(AlgoImage srcImage, AlgoImage maskImage)
        {
            MilImage milsrcImage = MilImage.CheckGreyImage(srcImage, "MilImage::MaskingCopy", "Source");
            MilImage milMaskImage = MilImage.CheckGreyImage(maskImage, "MilImage::MaskingCopy", "Mask");

            MIL.MbufSetRegion(this, milMaskImage, MIL.M_DEFAULT, MIL.M_DEFAULT, MIL.M_DEFAULT);
            MIL.MbufCopy(milsrcImage, this);
            MIL.MbufSetRegion(this, MIL.M_NULL, MIL.M_DEFAULT, MIL.M_DELETE, MIL.M_DEFAULT);
        }

        public static MilImage CheckMilImage(AlgoImage algoImage, string functionName, string imageName)
        {
            if (algoImage == null)
                throw new InvalidSourceException(String.Format("[{0}] {1} Image is null", functionName, imageName));

            try
            {
                MilImage milImage = algoImage as MilImage;
                if (milImage == null || milImage.Image == MIL.M_NULL)
                    throw new InvalidTargetException(String.Format("[{0}] {1} Image Object is null", functionName, imageName));
                return milImage;
            }
            catch (InvalidCastException)
            {
                throw new InvalidSourceException(String.Format("[{0}] {1} Image must be gray image", functionName, imageName));
            }
        }

        public static MilGreyImage CheckGreyImage(object algoImage, string functionName, string imageName)
        {
            if (algoImage == null)
                throw new InvalidSourceException(String.Format("[{0}] {1} Image is null", functionName, imageName));

            try
            {
                MilGreyImage milImage = algoImage as MilGreyImage;
                if (milImage == null || milImage.Image == MIL.M_NULL)
                    throw new InvalidTargetException(String.Format("[{0}] {1} Image Object is null", functionName, imageName));
                return milImage;
            }
            catch (InvalidCastException)
            {
                throw new InvalidSourceException(String.Format("[{0}] {1} Image must be gray image", functionName, imageName));
            }
        }

        public static MilColorImage CheckColorImage(object algoImage, string functionName, string imageName)
        {
            if (algoImage == null)
                throw new InvalidSourceException(String.Format("[{0}] {1} Image is null", functionName, imageName));

            try
            {
                MilColorImage milImage = algoImage as MilColorImage;
                if (milImage == null || milImage.Image == MIL.M_NULL)
                    throw new InvalidTargetException(String.Format("[{0}] {1} Image Object is null", functionName, imageName));
                return milImage;
            }
            catch (InvalidCastException)
            {
                throw new InvalidSourceException(String.Format("[{0}] {1} Image must be color image", functionName, imageName));
            }
        }

        public void Alloc(int width, int height)
        {
            Alloc(width, height, IntPtr.Zero, 0);
        }

        public abstract void Alloc(int width, int height, IntPtr dataPtr, int pitch);
        //public abstract void Copy(AlgoImage srcImage, Rectangle rectangle);
        public abstract void Put(byte[] userArrayPtr);
        public abstract void Get(byte[] userArrayPtr);
        public abstract void Put(float[] userArrayPtr);
        public abstract void Get(float[] userArrayPtr);

        public override IntPtr GetImagePtr()
        {
            IntPtr ptr = MIL.MbufInquire(image, MIL.M_HOST_ADDRESS, MIL.M_NULL);
            return ptr;
        }

        public override void Save(string fileName)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilImage.Alloc]");

            string path = Path.GetDirectoryName(fileName);
            if (string.IsNullOrEmpty(path))
                return;

            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);

            MIL_INT fileFormat = MIL.M_BMP;

            string extension = Path.GetExtension(fileName).ToLower();
            if (extension.Contains(".jpg") == true || extension.Contains(".jpeg") == true)
                fileFormat = MIL.M_JPEG_LOSSY;
            if (extension.Contains(".png") == true)
                fileFormat = MIL.M_PNG;

            MIL.MbufExport(fileName, fileFormat, image);
        }

        public override void GetByte(byte[] bytes)
        {
            // 이건가?
            //MIL_INT size = MIL.MbufInquire(image, MIL.M_SIZE_BYTE);

            // 이건가?
            // (*BitPerPixel) 하면서 오버플로우....
            long length = width * height * (BitPerPixel / 8);

            // 이건가?
            //MIL_INT w = MIL.MbufInquire(image, MIL.M_SIZE_X);
            //MIL_INT h = MIL.MbufInquire(image, MIL.M_SIZE_Y);
            //MIL_INT b = MIL.MbufInquire(image, MIL.M_SIZE_BAND);
            //int count = (int)(w * h * b);

            Debug.Assert(bytes.Length >= length);
            Get(bytes);
        }

        public override void PutByte(byte[] data)
        {
            Put(data);
        }

        public override void PutByte(IntPtr ptr, int pitch)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Put]");

            int bpp = this.BitPerPixel;
            long attribute = MIL.M_IMAGE + MIL.M_PROC;
            MIL_ID mIL_ID = MIL.MbufCreate2d(MIL.M_DEFAULT_HOST, width, height, bpp + MIL.M_UNSIGNED, attribute, MIL.M_HOST_ADDRESS + MIL.M_PITCH, pitch, (ulong)ptr, MIL.M_NULL);
            MIL.MbufCopy(mIL_ID, this.image);
            MIL.MbufFree(mIL_ID);
        }

        public override bool Equals(object obj)
        {
            if (obj is MilImage)
            {
                MilImage milImage = (MilImage)obj;
                return milImage.Image == this.image;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.image.GetHashCode();
        }

        public void AddTrace()
        {
#if DEBUG
            this.StackTrace = new StackTrace();
#endif
        }

        protected abstract MilImage CreateInstance();

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            MilImage milChildImage = this.CreateInstance();
            //MilImage milChildImage = (MilImage)Activator.CreateInstance(this.GetType());

            milChildImage.image = MIL.MbufChild2d(image, (int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height, MIL.M_NULL);
            if (milChildImage.image == MIL.M_NULL)
                throw new Exception($"MilImage::GetSubImage - ChildImage ID is NULL");

            milChildImage.width = rectangle.Width;
            milChildImage.height = rectangle.Height;
            milChildImage.parentImage = this;

            lock (this.subImageList)
                this.subImageList.Add(milChildImage);

            MilObjectManager.Instance.AddObject(milChildImage);

            dstImage = milChildImage;
        }
    }


    public class MilBinalImage : MilImage
    {
        public MilBinalImage()
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Binary;
        }

        public MilBinalImage(int width, int height) : this()
        {
            Alloc(width, height);
        }

        public MilBinalImage(int width, int height, IntPtr dataPtr, int pitch = 0) : this()
        {
            Alloc(width, height, dataPtr, pitch);
        }

        public MilBinalImage(ConvertPack convertPack) : this()
        {
            throw new NotImplementedException();
        }

        protected override MilImage CreateInstance()
        {
            return new MilBinalImage();
        }

        public override void Alloc(int width, int height, IntPtr dataPtr, int pitch)
        {
            if (image != MIL.M_NULL)
                throw new InvalidObjectException("[MilBinalImage.Alloc]");

            this.width = width;
            this.height = height;
            long attribute = MIL.M_IMAGE + MIL.M_PROC;

            if (dataPtr == IntPtr.Zero)
            {
                if (MatroxHelper.UseNonPagedMem)
                    attribute += MIL.M_NON_PAGED;

                image = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, this.width, this.height, 1 + MIL.M_UNSIGNED, attribute, MIL.M_NULL);
            }
            else
            {
                if (pitch == 0)
                    pitch = ((this.width + 31) / 32) * 4;
                image = MIL.MbufCreate2d(MIL.M_DEFAULT_HOST, this.width, this.height, 1 + MIL.M_UNSIGNED, attribute, MIL.M_HOST_ADDRESS + MIL.M_PITCH_BYTE, pitch, (ulong)dataPtr, MIL.M_NULL);
            }

            MilObjectManager.Instance.AddObject(this);
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            MilBinalImage cloneImage = new MilBinalImage(rectangle.Width, rectangle.Height);
            cloneImage.Copy(this, rectangle);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override void Get(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilBinalImage.Get]");

            MIL.MbufGet(image, userArrayPtr);
        }

        public override void Get(float[] userArrayPtr)
        {
            throw new NotImplementedException();
        }

        public override void Put(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilBinalImage.Put]");

            MIL.MbufPut(image, userArrayPtr);
        }

        public override void Put(float[] userArrayPtr)
        {
            throw new NotImplementedException();
        }

        public override ImageD ToImageD()
        {
            return MilImageBuilder.ConvertImage(this);
        }
    }

    public class MilGreyImage : MilImage
    {
        public MilGreyImage()
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Grey;
        }

        public MilGreyImage(int width, int height) : this()
        {
            Alloc(width, height);
        }

        public MilGreyImage(int width, int height, IntPtr dataPtr, int pitch = 0) : this()
        {
            Alloc(width, height, dataPtr, pitch);
        }

        public MilGreyImage(ConvertPack convertPack) : this()
        {
            if (convertPack.Ptr != IntPtr.Zero)
            {
                Alloc(convertPack.Width, convertPack.Height, convertPack.Ptr, convertPack.Pitch);
            }
            else
            {
                Alloc(convertPack.Width, convertPack.Height);

                switch (convertPack.ImageType)
                {
                    case ImageType.Binary:
                    case ImageType.Color:
                    case ImageType.Gpu:
                    case ImageType.GrayCopy:
                        throw new NotImplementedException();
                        break;

                    case ImageType.Grey:
                        PutByte(convertPack.Bytes);
                        break;

                    case ImageType.Depth:
                        {
                            int byteLength = convertPack.Width * convertPack.Height;
                            byte[] bytes = new byte[byteLength];
                            for (int i = 0; i < byteLength; i++)
                            {
                                float single = BitConverter.ToSingle(convertPack.Bytes, 4 * i);
                                bytes[i] = (byte)single;
                            }
                            PutByte(bytes);
                        }
                        break;
                }
            }
        }

        protected override MilImage CreateInstance()
        {
            return new MilGreyImage();
        }

        public override void Alloc(int width, int height, IntPtr dataPtr, int pitch)
        {
            if (image != MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Alloc]");

            this.width = width;
            this.height = height;

            long attribute = MIL.M_IMAGE + MIL.M_PROC;

            if (dataPtr == IntPtr.Zero)
            {
                if (MatroxHelper.UseNonPagedMem)
                    attribute += MIL.M_NON_PAGED;

                image = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, 8 + MIL.M_UNSIGNED, attribute, MIL.M_NULL);
            }
            else
            {
                if (pitch == 0)
                    pitch = (int)((this.width * 1 + 3) / 4) * 4;

                image = MIL.MbufCreate2d(MIL.M_DEFAULT_HOST, width, height, 8 + MIL.M_UNSIGNED, attribute, MIL.M_HOST_ADDRESS + MIL.M_PITCH_BYTE, pitch, (ulong)dataPtr, MIL.M_NULL);
            }

            MilObjectManager.Instance.AddObject(this);
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

        public override void Put(float[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
        }

        public override void Get(float[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
        }

        public override void Put(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Put]");

            MIL.MbufPut(image, userArrayPtr);
        }

        public override void Get(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Get]");

           MIL.MbufGet(image, userArrayPtr);
        }

        public override ImageD ToImageD()
        {
            return MilImageBuilder.ConvertImage(this);
        }

        //public override AlgoImage ConvertToOpenCvImage(ImageType imageType)
        //{
        //    IntPtr ptr = this.GetImagePtr();
        //    Image2D image2d = new Image2D(width, height, 1, Pitch, ptr);

        //    AlgoImage newImage;
        //    switch (imageType)
        //    {
        //        case ImageType.Grey:
        //            newImage = ImageBuilder.Build(ImagingLibrary.OpenCv, image2d, imageType);
        //            break;
        //        case ImageType.Gpu:
        //            newImage = ImageBuilder.Build(ImagingLibrary.OpenCv, image2d, imageType);
        //            //newImage = new OpenCvCudaImage();
        //            //((OpenCvCudaImage)newImage).Create(this.width, this.height, ptr);
        //            break;

        //        default:
        //            throw new NotImplementedException();
        //    }

        //    this.SubImageList.Add(newImage);
        //    newImage.ParentImage = this;
        //    return newImage;
        //}

        //protected override AlgoImage Convert2GreyImage()
        //{
        //    return this.Clone();
        //}

        //protected override AlgoImage Convert2ColorImage()
        //{
        //    MilColorImage milColorImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Color, this.width, this.height) as MilColorImage;

        //    MIL.MbufCopy(this.image, milColorImage.Image);
        //    return milColorImage;
        //}
    }

    public class MilDepthImage : MilImage
    {
        public MilDepthImage()
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Depth;
        }

        public MilDepthImage(int width, int height) : this()
        {
            Alloc(width, height);
        }

        public MilDepthImage(int width, int height, IntPtr dataPtr, int pitch = 0) : this()
        {
            Alloc(width, height, dataPtr, pitch);
        }

        public MilDepthImage(ConvertPack convertPack) : this()
        {
            if (convertPack.Ptr != IntPtr.Zero)
            {
                Alloc(convertPack.Width, convertPack.Height, convertPack.Ptr, convertPack.Pitch);
            }
            else
            {
                Alloc(convertPack.Width, convertPack.Height);

                switch (convertPack.ImageType)
                {
                    case ImageType.Binary:
                    case ImageType.Color:
                    case ImageType.Gpu:
                    case ImageType.GrayCopy:
                        throw new NotImplementedException();
                        break;

                    case ImageType.Grey:
                        {
                            int dataLength = convertPack.Width * convertPack.Height;
                            byte[] bytes = new byte[4 * dataLength];
                            for (int i = 0; i < dataLength; i++)
                                Buffer.BlockCopy(BitConverter.GetBytes((float)convertPack.Bytes[i]), 0, bytes, 4 * i, 4);
                            PutByte(bytes);
                        }
                        break;

                    case ImageType.Depth:
                        PutByte(convertPack.Bytes);
                        break;
                }
            }
        }

        protected override MilImage CreateInstance()
        {
            return new MilDepthImage();
        }

        public override void Alloc(int width, int height, IntPtr dataPtr, int pitch)
        {
            if (image != MIL.M_NULL)
                throw new InvalidObjectException("[MilDepthImage.Alloc]");

            this.width = width;
            this.height = height;

            long attribute = MIL.M_IMAGE + MIL.M_PROC;

            if (dataPtr == IntPtr.Zero)
            {
                if (MatroxHelper.UseNonPagedMem)
                    attribute += MIL.M_NON_PAGED;

                image = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_FLOAT + 32, attribute, MIL.M_NULL);
            }
            else
            {
                if (pitch == 0)
                    pitch = ((this.width * 4 + 3) / 4) * 4;
                image = MIL.MbufCreate2d(MIL.M_DEFAULT_HOST, width, height, MIL.M_FLOAT + 32, attribute, MIL.M_HOST_ADDRESS + MIL.M_PITCH_BYTE, pitch, (ulong)dataPtr.ToInt64(), MIL.M_NULL);
            }
            //MIL.MbufExport(@"C:\temp\bmp\alloc.bmp", MIL.M_BMP, image);
            MilObjectManager.Instance.AddObject(this);
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            MilDepthImage cloneImage = new MilDepthImage(rectangle.Width, rectangle.Height);
            cloneImage.Copy(this, rectangle);

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override void Put(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Put]");

            int w = (int)MIL.MbufInquire(this.image, MIL.M_SIZE_X);
            int h = (int)MIL.MbufInquire(this.image, MIL.M_SIZE_Y);
            int b = (int)MIL.MbufInquire(this.image, MIL.M_SIZE_BAND);
            long dataLength = w * h * b;

            Debug.Assert(dataLength * 4 == userArrayPtr.Length);

            //float[] datas = new float[dataLength];
            //Buffer.BlockCopy(userArrayPtr, 0, datas, 0, userArrayPtr.Length);
            //MIL.MbufPut(this.image, datas);

            MIL.MbufPut(this.image, userArrayPtr);
        }

        public override void Get(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Put]");

            int w = (int)MIL.MbufInquire(this.image, MIL.M_SIZE_X);
            int h = (int)MIL.MbufInquire(this.image, MIL.M_SIZE_Y);
            int b = (int)MIL.MbufInquire(this.image, MIL.M_SIZE_BAND);
            long dataLength = w * h * b;
            Debug.Assert(4* dataLength == userArrayPtr.Length);

            //float[] datas = new float[dataLength];
            //MIL.MbufGet(this.image, datas);
            //Buffer.BlockCopy(datas, 0, userArrayPtr, 0, userArrayPtr.Length);
            MIL.MbufGet(this.image, userArrayPtr);
        }

        public override void Put(float[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Put]");

            MIL.MbufPut(image, userArrayPtr);
        }

        public override void Get(float[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Get]");

            MIL.MbufGet(image, userArrayPtr);
        }

        public override ImageD ToImageD()
        {
            return MilImageBuilder.ConvertImage(this);
        }
    }

    public class MilColorImage : MilImage
    {
        public MilColorImage()
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Color;
        }

        public MilColorImage(int width, int height) : this()
        {
            Alloc(width, height);
        }
        public MilColorImage(int width, int height, IntPtr dataPtr, int pitch = 0) : this()
        {
            Alloc(width, height, dataPtr, pitch);
        }

        public MilColorImage(ConvertPack convertPack) : this()
        {
            if (convertPack.Ptr != IntPtr.Zero)
            {
                Alloc(convertPack.Width, convertPack.Height, convertPack.Ptr, convertPack.Pitch);
            }
            else
            {
                Alloc(convertPack.Width, convertPack.Height);

                switch (convertPack.ImageType)
                {
                    case ImageType.Binary:
                    case ImageType.Color:
                    case ImageType.Gpu:
                    case ImageType.GrayCopy:
                        throw new NotImplementedException();
                        break;

                    case ImageType.Grey:
                        {
                            MIL_ID tempMilObject = MIL.MbufAlloc2d(MIL.M_DEFAULT_HOST, convertPack.Width, convertPack.Height, 8 + MIL.M_UNSIGNED, MIL.M_IMAGE + MIL.M_PROC, MIL.M_NULL);
                            int tempPitch = convertPack.Width;
                            byte[] bytes;
                            if (convertPack.Pitch == tempPitch)
                            {
                                bytes = convertPack.Bytes;
                            }
                            else
                            {
                                bytes = new byte[tempPitch * convertPack.Height];
                                int lineLength = Math.Min(convertPack.Pitch, tempPitch);
                                for (int y = 0; y < convertPack.Height; y++)
                                {
                                    int srcOffset = y * convertPack.Pitch;
                                    int dstOffset = y * tempPitch;
                                    Array.Copy(convertPack.Bytes, srcOffset, bytes, dstOffset, lineLength);
                                }
                            }
                            MIL.MbufPut(tempMilObject, bytes);
                            MIL.MbufCopy(tempMilObject, this.Image);
                            MIL.MbufFree(tempMilObject);
                        }
                        break;

                    case ImageType.Depth:
                        throw new NotImplementedException();                      
                }
            }
        }

        protected override MilImage CreateInstance()
        {
            return new MilColorImage();
        }

        public override void Alloc(int width, int height, IntPtr dataPtr, int pitch)
        {
            if (image != MIL.M_NULL)
                throw new InvalidObjectException("[MilColorImage.Alloc] Already Allocated.");

            this.width = width;
            this.height = height;

            long attribute = MIL.M_IMAGE + MIL.M_PROC;

            if (dataPtr == IntPtr.Zero)
            {
                if (MatroxHelper.UseNonPagedMem)
                    attribute += MIL.M_NON_PAGED;

                image = MIL.MbufAllocColor(MIL.M_DEFAULT_HOST, 3, width, height, MIL.M_UNSIGNED + 8, attribute, MIL.M_NULL);
            }
            else
            {
                if (pitch == 0)
                    pitch = ((this.width * 3 + 3) / 4) * 4;
                image = MIL.MbufCreateColor(MIL.M_DEFAULT_HOST, 3, width, height, MIL.M_UNSIGNED + 8, attribute, MIL.M_HOST_ADDRESS + MIL.M_PITCH_BYTE, pitch, ref dataPtr, MIL.M_NULL);
            }

            MilObjectManager.Instance.AddObject(this);
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            MilGreyImage cloneImage = new MilGreyImage(rectangle.Width, rectangle.Height);
            cloneImage.Copy(this, rectangle);

            return cloneImage;
        }

        private MIL_INT GetBand(ImageBandType imageBandType)
        {
            MIL_INT band;
            switch (imageBandType)
            {
                case ImageBandType.Red: band = MIL.M_RED; break;
                case ImageBandType.Blue: band = MIL.M_BLUE; break;
                case ImageBandType.Green: band = MIL.M_GREEN; break;
                default:
                    throw new ArgumentException("Invalid Image Band Type");
            }

            return band;
        }

        public MilImage Clone(ImageBandType imageBandType)
        {
            return Clone(new Rectangle(0, 0, width, height), imageBandType);
        }

        public override void GetByte(byte[] bytes)
        {
            int size = width * height * 3;
            Debug.Assert(bytes.Length >= size);
            Get(bytes);
        }

        public MilImage Clone(Rectangle rectangle, ImageBandType imageBandType)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilColorImage.Clone] Image is not allocated ");

            MilImage cloneImage;

            if (imageBandType == ImageBandType.Luminance)
            {
                cloneImage = new MilGreyImage(rectangle.Width, rectangle.Height);
                MIL.MimConvert(image, cloneImage.Image, MIL.M_RGB_TO_L);
            }
            else
            {
                MIL_INT band = GetBand(imageBandType);
                cloneImage = new MilColorImage(rectangle.Width, rectangle.Height);
                MIL.MbufCopyColor2d(image, cloneImage.Image, band, rectangle.Left, rectangle.Top, MIL.M_ALL_BAND, 0, 0, rectangle.Width, rectangle.Height);
            }

            cloneImage.FilteredList = FilteredList;

            return cloneImage;
        }

        public override void Put(float[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilColorImage.Put] float data type is not support");
        }

        public override void Get(float[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilColorImage.Put] float data type is not support");
        }

        public override void Put(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilColorImage.Put]");

            MIL.MbufPutColor(image, MIL.M_PACKED + MIL.M_BGR24, MIL.M_ALL_BAND, userArrayPtr);
        }

        public override void Get(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilColorImage.Get]");

            MIL.MbufGetColor(image, MIL.M_PACKED + MIL.M_BGR24, MIL.M_ALL_BAND, userArrayPtr);
        }

        public override ImageD ToImageD()
        {
            return MilImageBuilder.ConvertImage(this);
        }

        //protected override AlgoImage Convert2GreyImage()
        //{
        //    MilGreyImage milImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Color, this.width, this.height) as MilGreyImage;

        //    MIL.MbufCopy(this.image, milImage.Image);
        //    return milImage;
        //}

        //protected override AlgoImage Convert2ColorImage()
        //{
        //    return this.Clone();
        //}
    }

    public class MilGpuImage : MilImage
    {
        public MilGpuImage()
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Gpu;
        }

        public MilGpuImage(int width, int height) : this()
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Gpu;

            Alloc(width, height);
        }

        public MilGpuImage(int width, int height, IntPtr dataPtr, int pitch = 0) : this()
        {
            LibraryType = ImagingLibrary.MatroxMIL;
            ImageType = ImageType.Gpu;

            Alloc(width, height, dataPtr, pitch);
        }

        public MilGpuImage(ConvertPack convertPack) : this()
        {
            throw new NotImplementedException();
        }

        protected override MilImage CreateInstance()
        {
            return new MilGpuImage();
        }

        public override void Alloc(int width, int height, IntPtr dataPtr, int pitch)
        {
            if (image != MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Alloc]");

            this.width = width;
            this.height = height;

            long attribute = MIL.M_VIDEO_MEMORY + MIL.M_IMAGE + MIL.M_PROC;

            if (dataPtr == IntPtr.Zero)
            {
                image = MIL.MbufAlloc2d(MatroxHelper.GpuSystemId, width, height, 8 + MIL.M_UNSIGNED, attribute, MIL.M_NULL);
            }
            else
            {
                if (pitch == 0)
                    pitch = (int)((this.width * 1 + 3) / 4) * 4;

                image = MIL.MbufCreate2d(MatroxHelper.GpuSystemId, width, height, 8 + MIL.M_UNSIGNED, attribute, MIL.M_HOST_ADDRESS + MIL.M_PITCH_BYTE, pitch, (ulong)dataPtr, MIL.M_NULL);
            }

            MilObjectManager.Instance.AddObject(this);
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

        public override void Put(float[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
        }

        public override void Get(float[] userArrayPtr)
        {
            throw new InvalidObjectException("[MilGreyImage.Put] float data type is not support");
        }

        public override void Put(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Put]");

            MIL.MbufPut(image, userArrayPtr);
        }

        public override void Get(byte[] userArrayPtr)
        {
            if (image == MIL.M_NULL)
                throw new InvalidObjectException("[MilGreyImage.Get]");

            MIL.MbufGet(image, userArrayPtr);
        }

        public override ImageD ToImageD()
        {
            return MilImageBuilder.ConvertImage(this);
        }

        //public override AlgoImage ConvertToOpenCvImage(ImageType imageType)
        //{
        //    IntPtr ptr = this.GetImagePtr();
        //    Image2D image2d = new Image2D(width, height, 1, Pitch, ptr);

        //    AlgoImage newImage;
        //    switch (imageType)
        //    {
        //        case ImageType.Grey:
        //            newImage = ImageBuilder.Build(ImagingLibrary.OpenCv, image2d, imageType);
        //            break;
        //        case ImageType.Gpu:
        //            newImage = ImageBuilder.Build(ImagingLibrary.OpenCv, image2d, imageType);
        //            //newImage = new OpenCvCudaImage();
        //            //((OpenCvCudaImage)newImage).Create(this.width, this.height, ptr);
        //            break;

        //        default:
        //            throw new NotImplementedException();
        //    }

        //    this.SubImageList.Add(newImage);
        //    newImage.ParentImage = this;
        //    return newImage;
        //}

        //protected override AlgoImage Convert2GreyImage()
        //{
        //    return this.Clone();
        //}

        //protected override AlgoImage Convert2ColorImage()
        //{
        //    MilColorImage milColorImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Color, this.width, this.height) as MilColorImage;

        //    MIL.MbufCopy(this.image, milColorImage.Image);
        //    return milColorImage;
        //}
    }
}
