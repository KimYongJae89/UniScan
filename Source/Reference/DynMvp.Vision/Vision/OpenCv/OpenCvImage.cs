using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Cuda;
using DynMvp.Base;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DynMvp.Vision.Matrox;
using Emgu.CV.CvEnum;
using Emgu.Util;
using System.Diagnostics;

namespace DynMvp.Vision.OpenCv
{
    public interface IOpenCvImage<TColor, TDepth>
        where TColor : struct, IColor
        where TDepth : new()
    {
        Image<TColor, TDepth> Image { get; set; }
    }

    public abstract class OpenCvImage : AlgoImage
    {
        public override bool IsAllocated { get; }

        protected override void Disposing()
        {
            lock (this.SubImageList)
            {
                while (this.SubImageList.Count() > 0)
                {
                    AlgoImage algoImage = this.SubImageList.Last();
                    if (algoImage != null)
                        algoImage.Dispose();
                }
                this.SubImageList.Clear();
            }

            if (this.ParentImage != null)
            {
                lock (this.ParentImage.SubImageList)
                    this.ParentImage.SubImageList.Remove(this);
            }
            this.ParentImage = null;

            DisposeImageObject();
        }

        public abstract bool IsCudaImage { get; }

        public abstract UnmanagedObject UnmanagedObject { get; }
        public abstract DepthType DepthType { get; }
        public abstract void DisposeImageObject();
        public abstract IInputArray InputArray { get; }
        public abstract IOutputArray OutputArray { get; }
        public abstract IInputOutputArray InputOutputArray { get; }
        public override int BitPerPixel { get; }

        public abstract IOpenCvImage<TColor, TDepth> GetInterface<TColor, TDepth>()
            where TColor : struct, IColor
        where TDepth : new();
    }

    public class OpenCvGreyImage : OpenCvImage, IOpenCvImage<Gray, Byte>
    {
        public Image<Gray, Byte> Image { get; set; }
        public override DepthType DepthType => this.Image.Mat.Depth;
        public override IInputArray InputArray => this.Image;
        public override IOutputArray OutputArray => this.Image;
        public override IInputOutputArray InputOutputArray => this.Image;
        public override int BitPerPixel => this.Image.Mat.ElementSize * 8;
        public override IOpenCvImage<TColor, TDepth> GetInterface<TColor, TDepth>() => (IOpenCvImage<TColor, TDepth>)this.Image;

        public override bool IsCudaImage { get => false; }
        public override UnmanagedObject UnmanagedObject => Image;

        public OpenCvGreyImage()
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Grey;
        }

        public OpenCvGreyImage(int width, int height) 
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Grey;

            this.Image = new Image<Gray, byte>(width, height);
        }

        public OpenCvGreyImage(ConvertPack convertPack)
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Grey;

            if (convertPack.Ptr == IntPtr.Zero)
            {
                this.Image = new Image<Gray, byte>(convertPack.Width, convertPack.Height);
                byte[] bytes = null;

                switch (convertPack.ImageType)
                {
                    case ImageType.Binary:
                        break;
                    case ImageType.Grey:
                        bytes = convertPack.Bytes;
                        break;
                    case ImageType.Color:
                        break;
                    case ImageType.Depth:
                        {
                            bytes = new byte[this.Pitch * this.Height];
                            for (int i = 0; i < convertPack.Bytes.Length / 4; i++)
                            {
                                float data = BitConverter.ToSingle(convertPack.Bytes, i * 4);
                                
                                int y = i / convertPack.Width;
                                int x = i % convertPack.Width;
                                bytes[y * this.Pitch + x] = (data > byte.MaxValue ? byte.MaxValue : (byte)data);
                            }
                        }
                        break;
                    case ImageType.Gpu:
                        break;
                    case ImageType.GrayCopy:
                        break;
                }

                if (bytes == null)
                    throw new NotImplementedException();
                this.Image.Bytes = bytes;
            }
            else
            {
                this.Image = new Image<Gray, byte>(convertPack.Width, convertPack.Height, convertPack.Pitch, convertPack.Ptr);
            }
        }

        public override void DisposeImageObject()
        {
            if (this.Image != null)
                this.Image.Dispose();
            this.Image = null;
        }

        public override void Clear(byte initVal)
        {
            this.Image.SetValue(new MCvScalar(initVal));
        }
        public override IntPtr GetImagePtr()
        {
            return this.Image.Mat.DataPointer;
        }

        public override AlgoImage Clone()
        {
            OpenCvGreyImage cloneImage = new OpenCvGreyImage();
            cloneImage.Image = this.Image.Clone();

            return cloneImage;
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            OpenCvGreyImage clipImage = new OpenCvGreyImage();
            clipImage.Image = Image.Copy(rectangle);

            return clipImage;
        }

        
        public override int Width
        {
            get { return this.Image.Width; }
        }

        public override int Pitch
        {
            get { return this.Image.Data.GetUpperBound(1) + 1; }
        }

        public override int Height
        {
            get { return this.Image.Height; }
        }

        public override void GetByte(byte[] bytes)
        {
            Debug.Assert(bytes.Length >= this.Image.Bytes.Length);
            Array.Copy(this.Image.Bytes, bytes, this.Image.Bytes.Length);
            //return this.Image.Bytes; //여기 잘못됨...
        }

        public override void PutByte(byte[] data)
        {
            Array.Copy(data, 0, this.Image.Bytes, 0, Math.Min(this.Image.Bytes.Length, data.Length));
        }

        public override void PutByte(IntPtr srcPtr, int srcPitch)
        {
            if (srcPitch == this.Pitch)
            {
                int length = Width * Height;
                unsafe
                {
                    Buffer.MemoryCopy(srcPtr.ToPointer(), this.Image.Mat.DataPointer.ToPointer(), length, length);
                }
            }
            else
            {
                int length = Math.Min(srcPitch, this.Pitch);
                IntPtr ptr = this.Image.Mat.DataPointer;
                for (int i = 0; i < this.Height; i++)
                {
                    IntPtr src = srcPtr + (i * srcPitch);
                    IntPtr dst = ptr + (i * this.Pitch);
                    unsafe
                    {
                        Buffer.MemoryCopy(ptr.ToPointer(), this.Image.Mat.DataPointer.ToPointer(), length, length);
                    }
                }
            }
        }

        public override ImageD ToImageD()
        {
            return OpenCvImageBuilder.ConvertImage(this.Image);
        }

        public override Bitmap ToBitmap()
        {
            return this.Image.Mat.Bitmap;
        }

        public override void Save(string fileName)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            this.Image.Save(fileName);
            //LogHelper.Debug(LoggerType.Operation, string.Format("OpenCvGreyImage::Save - {0}", fileName));
        }

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            dstImage = new OpenCvGreyImage();
            OpenCvGreyImage openCvGreyImage = dstImage as OpenCvGreyImage;
            openCvGreyImage.Image = this.Image.GetSubRect(rectangle);

            openCvGreyImage.ParentImage = this;
            lock(this.SubImageList)
                this.SubImageList.Add(openCvGreyImage);
        }

        //public override AlgoImage ConvertToMilImage(ImageType imageType)
        //{
        //    AlgoImage algoImage;
        //    switch (ImageType)
        //    {
        //        case ImageType.Grey:
        //            MilGreyImage milGreyImage = new MilGreyImage(this.Width, this.Height, this.image.MIplImage.ImageData);
        //            algoImage = milGreyImage;
        //            break;
        //        default:
        //            throw new NotImplementedException();
        //    }

        //    this.SubImageList.Add(algoImage);
        //    algoImage.ParentImage = this;

        //    return algoImage;
        //    ImageD imageD = this.ToImageD();
        //    return ImageBuilder.Build(ImagingLibrary.MatroxMIL, imageD, imageType);
        //}

        //public override AlgoImage ConvertToOpenCvImage(ImageType imageType)
        //{
        //    if (imageType == ImageType.Grey)
        //        return this.Clone();

        //    if(imageType == ImageType.Color)
        //    {
        //        Image<Gray, byte> channel = this.image.Clone();
        //        OpenCvColorImage openCvColorImage = new OpenCvColorImage();
        //        openCvColorImage.Image = new Image<Bgr, byte>(new Image<Gray, byte>[3] { this.image, this.image, this.image });
        //        return openCvColorImage;
        //    }

        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage Convert2GreyImage()
        //{
        //    return this.Clone();
        //}

        //protected override AlgoImage Convert2ColorImage()
        //{
        //    Image<Gray, byte> channel = this.image.Clone();
        //    OpenCvColorImage openCvColorImage = new OpenCvColorImage();
        //    openCvColorImage.Image = new Image<Bgr, byte>(new Image<Gray, byte>[3] { this.image, this.image, this.image });
        //    return openCvColorImage;
        //}
        public override void MaskingCopy(AlgoImage srcImage, AlgoImage maskImage)
        {
            OpenCvGreyImage openCvSrcImage = (OpenCvGreyImage)srcImage;
            OpenCvGreyImage openCvMaskImage = (OpenCvGreyImage)maskImage;
            openCvSrcImage.Image.Copy(this.Image, openCvMaskImage.Image);
        }

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            Copy(srcImage, srcRect.Location, Point.Empty, srcRect.Size);
        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {
            OpenCvGreyImage openCvSrcImage = (OpenCvGreyImage)srcImage;

            Rectangle srcRect = new Rectangle(srcPt, size);
            Rectangle dstRect = new Rectangle(dstPt, size);

            openCvSrcImage.Image.ROI = srcRect;
            this.Image.ROI = dstRect;

            CvInvoke.cvCopy(openCvSrcImage.Image, this.Image, IntPtr.Zero);

            openCvSrcImage.Image.ROI = Rectangle.Empty;
            this.Image.ROI = Rectangle.Empty;
        }
    }

    public class OpenCvDepthImage : OpenCvImage, IOpenCvImage<Gray, float>
    {
        public Image<Gray, float> Image { get; set; }
        public override DepthType DepthType => DepthType.Cv32F;
        public override IInputArray InputArray => this.Image; 
        public override IOutputArray OutputArray => this.Image; 
        public override IInputOutputArray InputOutputArray => this.Image; 
        public override int BitPerPixel => this.Image.Mat.ElementSize * 8;
        public override IOpenCvImage<TColor, TDepth> GetInterface<TColor, TDepth>() => (IOpenCvImage<TColor, TDepth>)this.Image;

        public override bool IsCudaImage { get => false; }
        public override UnmanagedObject UnmanagedObject => Image;

        public OpenCvDepthImage()
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Depth;
        }

        public OpenCvDepthImage(int width, int height)
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Depth;

            this.Image = new Image<Gray, float>(width, height);
        }

        public OpenCvDepthImage(ConvertPack convertPack)
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Depth;

            if (convertPack.Ptr == IntPtr.Zero)
            {
                this.Image = new Image<Gray, float>(convertPack.Width, convertPack.Height);
                byte[] bytes = null;

                switch (convertPack.ImageType)
                {
                    case ImageType.Binary:
                        break;
                    case ImageType.Grey:
                        {
                            float[] datas = new float[this.Width * this.Height];
                            for (int y = 0; y < convertPack.Size.Height; y++)
                                Array.Copy(convertPack.Bytes, convertPack.Pitch * y, datas, this.Width * y, convertPack.Size.Width);

                            int byteLength = this.Pitch * this.Height;
                            System.Diagnostics.Debug.Assert(byteLength == datas.Length * sizeof(float));
                            bytes = new byte[byteLength];
                            Buffer.BlockCopy(datas, 0, bytes, 0, bytes.Length);
                        }
                        break;
                    case ImageType.Color:
                        break;
                    case ImageType.Depth:
                        bytes = convertPack.Bytes;
                        break;
                    case ImageType.Gpu:
                        break;
                    case ImageType.GrayCopy:
                        break;
                }
                if (bytes == null)
                    throw new NotImplementedException();
                this.Image.Bytes = bytes;
            }
            else
            {
                this.Image = new Image<Gray, float>(convertPack.Width, convertPack.Height, convertPack.Pitch, convertPack.Ptr);
            }
        }

        public override void DisposeImageObject()
        {
            if (this.Image != null)
                this.Image.Dispose();
            this.Image = null;

            if (pinnedArray.IsAllocated)
                pinnedArray.Free();
        }


        public override void Clear(byte initVal)
        {
            Image.SetValue(new MCvScalar(initVal));
        }

        public override IntPtr GetImagePtr()
        {
            return this.Image.Mat.DataPointer;
        }

        public override AlgoImage Clone()
        {
            OpenCvDepthImage cloneImage = new OpenCvDepthImage();
            cloneImage.Image = Image.Clone();

            return cloneImage;
        }

        public override AlgoImage Clip(Rectangle rectangle)
        {
            OpenCvDepthImage clipImage = new OpenCvDepthImage();
            clipImage.Image = Image.Copy(rectangle);

            return clipImage;
        }


        GCHandle pinnedArray;
        public GCHandle PinnedArray
        {
            get { return pinnedArray; }
            set { pinnedArray = value; }
        }

        public override int Width
        {
            get { return Image.Width; }
        }

        public override int Pitch
        {
            get { return Image.Bytes.Length / Height; }
        }

        public override int Height
        {
            get { return Image.Height; }
        }

        public override void GetByte(byte[] bytes)
        {
            Debug.Assert(bytes.Length >= this.Image.Bytes.Length);
            Array.Copy(bytes, this.Image.Bytes, this.Image.Bytes.Length);
        }

        public override void PutByte(byte[] data)
        {
            Image.Bytes = data;
        }

        public override void PutByte(IntPtr ptr, int pitch)
        {
            if (pitch == this.Pitch)
            {
                int length = this.Pitch * this.Height;
                unsafe
                {
                    Buffer.MemoryCopy(ptr.ToPointer(), this.Image.Mat.DataPointer.ToPointer(), length, length);
                }
            }
            else
            {
                int length = Math.Min(pitch, this.Pitch);
                IntPtr basePtr = this.Image.Mat.DataPointer;
                for (int i = 0; i < this.Height; i++)
                {
                    IntPtr src = ptr + (i * pitch);
                    IntPtr dst = basePtr + (i * this.Pitch);
                    unsafe
                    {
                        Buffer.MemoryCopy(ptr.ToPointer(), this.Image.Mat.DataPointer.ToPointer(), length, length);
                    }
                }
            }
        }

        //public override void PutByte(IntPtr srcPtr, int srcPitch)
        //{
        //    int length = Math.Min(srcPitch, this.Pitch);
        //    IntPtr ptr = this.image.Mat.DataPointer;
        //    for (int i = 0; i < this.Height; i++)
        //    {
        //        IntPtr src = srcPtr + (i * srcPitch);
        //        IntPtr dst = ptr + (i * this.Pitch);
        //        unsafe
        //        {
        //            Buffer.MemoryCopy(ptr.ToPointer(), this.image.Mat.DataPointer.ToPointer(), length, length);
        //        }
        //    }
        //}

        public override ImageD ToImageD()
        {
            return OpenCvImageBuilder.ConvertImage(Image);
        }
        
        public override void Save(string fileName)
        {
            Image.Save(fileName);
        }

        protected override void GetSubImage(Rectangle rectangle,  out AlgoImage dstImage)
        {
            throw new InvalidObjectException("[MilGreyImage.GetChildImage] float data type is not support");
        }

        //protected override AlgoImage Convert2GreyImage()
        //{
        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage Convert2ColorImage()
        //{
        //    throw new NotImplementedException();
        //}

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            Copy(srcImage, srcRect.Location, Point.Empty, srcRect.Size);
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

    public class OpenCvColorImage : OpenCvImage, IOpenCvImage<Bgr, Byte>
    {
        public Image<Bgr, Byte> Image { get; set; }
        public override DepthType DepthType => this.Image.Mat.Depth;
        public override IInputArray InputArray => this.Image;
        public override IOutputArray OutputArray => this.Image;
        public override IInputOutputArray InputOutputArray => this.Image;
        public override int BitPerPixel => this.Image.Mat.ElementSize * 8;
        public override IOpenCvImage<TColor, TDepth> GetInterface<TColor, TDepth>() => (IOpenCvImage<TColor, TDepth>)this.Image;
        
        public override bool IsCudaImage => false;
        public override UnmanagedObject UnmanagedObject => Image;

        public OpenCvColorImage()
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Color;
        }

        public OpenCvColorImage(int width, int height)
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Color;

            this.Image = new Image<Bgr, byte>(width, height);
        }

        public OpenCvColorImage(ConvertPack convertPack)
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Color;

            if (convertPack.ImageType == ImageType.Grey)
            {
                OpenCvGreyImage openCvGreyImage = new OpenCvGreyImage(convertPack);
               //     openCvGreyImage.Image.Save(@"D:\temp\image.bmp");
                this.Image = new Image<Bgr, byte>(new Image<Gray, byte>[] { openCvGreyImage.Image, openCvGreyImage.Image, openCvGreyImage.Image });
                //    this.Image.Save(@"D:\temp\image1.bmp");
            }
            else
            {
                if (convertPack.Ptr == IntPtr.Zero)
                {
                    this.Image = new Image<Bgr, byte>(convertPack.Width, convertPack.Height);
                    this.Image.Bytes = convertPack.Bytes;
                }
                else
                {
                    this.Image = new Image<Bgr, byte>(convertPack.Width, convertPack.Height, convertPack.Pitch * 3, convertPack.Ptr);
                }
            }
        }

        public override void DisposeImageObject()
        {
            if (this.Image != null)
                this.Image.Dispose();
            this.Image = null;
        }

        public override void Clear(byte initVal)
        {
            Image.SetValue(new Bgr(initVal, initVal, initVal));
        }

        public override IntPtr GetImagePtr()
        {
            return this.Image.Mat.DataPointer;
        }

        public override AlgoImage Clone()
        {
            OpenCvColorImage cloneImage = new OpenCvColorImage();
            cloneImage.Image = Image.Clone();

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
            OpenCvColorImage clipImage = new OpenCvColorImage();
            clipImage.Image = Image.Copy(rectangle);

            clipImage.FilteredList = FilteredList;

            return clipImage;
        }


        public override int Pitch
        {
            get { return Image.Data.GetUpperBound(1) + 1; }
        }

        public override int Width
        {
            get { return Image.Width; }
        }

        public override int Height
        {
            get { return Image.Height; }
        }

        public override void GetByte(byte[] bytes)
        {
            Debug.Assert(bytes.Length >= this.Image.Bytes.Length);
            Array.Copy(bytes, this.Image.Bytes, this.Image.Bytes.Length);
        }

        public override void PutByte(byte[] data)
        {
            throw new NotImplementedException();
        }

        public override void PutByte(IntPtr ptr, int pitch)
        {
            if (pitch == this.Pitch)
            {
                int length = this.Pitch * this.Height;
                unsafe
                {
                    Buffer.MemoryCopy(ptr.ToPointer(), this.Image.Mat.DataPointer.ToPointer(), length, length);
                }
            }
            else
            {
                int length = Math.Min(pitch, this.Pitch);
                IntPtr basePtr = this.Image.Mat.DataPointer;
                for (int i = 0; i < this.Height; i++)
                {
                    IntPtr src = ptr + (i * pitch);
                    IntPtr dst = basePtr + (i * this.Pitch);
                    unsafe
                    {
                        Buffer.MemoryCopy(ptr.ToPointer(), this.Image.Mat.DataPointer.ToPointer(), length, length);
                    }
                }
            }
        }

        public override ImageD ToImageD()
        {
            return OpenCvImageBuilder.ConvertImage(Image);
        }

        public override void Save(string fileName)
        {
            string dir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            Image.Save(fileName);
        }

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            dstImage = new OpenCvColorImage();
            OpenCvColorImage openCvGreyImage = dstImage as OpenCvColorImage;
            openCvGreyImage.Image = this.Image.GetSubRect(rectangle);

            openCvGreyImage.ParentImage = this;
            lock (this.SubImageList)
                this.SubImageList.Add(openCvGreyImage);
        }

        //protected override AlgoImage Convert2GreyImage()
        //{
        //    Image<Bgr, byte> color = this.image.Clone();
        //    OpenCvGreyImage openCvGrayImage = new OpenCvGreyImage();
        //    openCvGrayImage.Image = new Image<Gray, byte>(color.Size);
        //    CvInvoke.CvtColor(color, openCvGrayImage.Image, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
        //    return openCvGrayImage;
        //}

        //protected override AlgoImage Convert2ColorImage()
        //{
        //    return this.Clone();
        //}

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            throw new NotImplementedException();
        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {
            OpenCvColorImage openCvSrcImage = (OpenCvColorImage)srcImage;

            Rectangle srcRect = new Rectangle(srcPt, size);
            Rectangle dstRect = new Rectangle(dstPt, size);

            openCvSrcImage.Image.ROI = srcRect;
            this.Image.ROI = dstRect;

            CvInvoke.cvCopy(openCvSrcImage.Image, this.Image, IntPtr.Zero);

            openCvSrcImage.Image.ROI = Rectangle.Empty;
            this.Image.ROI = Rectangle.Empty;
        }

        public override void MaskingCopy(AlgoImage srcImage, AlgoImage maskImage)
        {
            OpenCvColorImage openCvSrcImage = (OpenCvColorImage)srcImage;
            OpenCvGreyImage openCvMaskImage = (OpenCvGreyImage)maskImage;
            openCvSrcImage.Image.Copy(this.Image, openCvMaskImage.Image);
        }
    }

    public class OpenCvCudaImage : OpenCvImage, IOpenCvImage<Gray, Byte>
    {
        public Image<Gray, byte> Image { get; set; }
        public override DepthType DepthType => this.Image.Mat.Depth;
        public override IInputArray InputArray => this.Image;
        public override IOutputArray OutputArray => this.Image;
        public override IInputOutputArray InputOutputArray => this.Image;
        public override int BitPerPixel => this.Image.Mat.ElementSize * 8;
        public override IOpenCvImage<TColor, TDepth> GetInterface<TColor, TDepth>() => (IOpenCvImage<TColor, TDepth>)this.Image;


        public override bool IsCudaImage { get => true; }
        public override UnmanagedObject UnmanagedObject => Image;

        public CudaImage<Gray, byte> CudaImage { get; set; }
        public Emgu.CV.Cuda.Stream TransferStream { get; private set; } = new Emgu.CV.Cuda.Stream();
        public override bool IsStreamCompleted => TransferStream.Completed;

        public OpenCvCudaImage()
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Gpu;
        }

        public OpenCvCudaImage(int width, int height)
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Gpu;

            Create(width, height);
        }

        public OpenCvCudaImage(ConvertPack convertPack)
        {
            LibraryType = ImagingLibrary.OpenCv;
            ImageType = ImageType.Gpu;

            if (convertPack.Ptr == IntPtr.Zero)
                Create(convertPack.Width, convertPack.Height, convertPack.Bytes);
            else
                Create(convertPack.Width, convertPack.Height, convertPack.Pitch, convertPack.Ptr);
        }

        public override void WaitStreamComplete()
        {
            this.TransferStream.WaitForCompletion();
        }

        public override void DisposeImageObject()
        {
            this.CudaImage?.Dispose();
            this.CudaImage = null;

            this.Image?.Dispose();
            this.Image = null;

            this.TransferStream?.Dispose();
            this.TransferStream = null;
        }

        public override int Height
        {
            get { return this.Image.Height; }
        }

        public override int Width
        {
            get { return this.Width; }
        }

        public override int Pitch
        {
            get { return this.Image.Mat.Step; }
        }
        
        public override void Clear(byte initVal)
        {
            this.CudaImage.SetTo(new MCvScalar(initVal), null, this.TransferStream);
        }

        public void Clear(byte initVal, Emgu.CV.Cuda.Stream stream)
        {
            this.CudaImage.SetTo(new MCvScalar(initVal), null, stream);

        }
        public override AlgoImage Clip(Rectangle rectangle)
        {
            OpenCvCudaImage clipImage = new OpenCvCudaImage();
            CudaImage<Gray, byte> temp = new CudaImage<Gray, byte>(this.CudaImage, new Range(rectangle.Top, rectangle.Bottom), new Range(rectangle.Left, rectangle.Right));

            clipImage.CudaImage = temp;
            return clipImage;
        }

        public override AlgoImage Clone()
        {
            OpenCvCudaImage newImage = new OpenCvCudaImage();
            newImage.CudaImage = this.CudaImage.Clone(null);
            return newImage;
        }

        public override void GetByte(byte[] bytes)
        {
            Debug.Assert(bytes.Length >= this.Image.Bytes.Length);

            UpdateHostImage();
            this.WaitStreamComplete();

            Array.Copy(bytes, this.Image.Bytes, this.Image.Bytes.Length);
        }

        protected override void GetSubImage(Rectangle rectangle, out AlgoImage dstImage)
        {
            OpenCvCudaImage newImage = new OpenCvCudaImage(); 
            newImage.Image = Image.GetSubRect(rectangle);
            newImage.CudaImage = this.CudaImage.GetSubRect(rectangle);

            newImage.ParentImage = this;
            lock(this.SubImageList)
                this.SubImageList.Add(newImage);

            dstImage = newImage as OpenCvCudaImage;
        }

        public override IntPtr GetImagePtr()
        {
            UpdateHostImage();
            //transferStream.WaitForCompletion();
            return this.Image.Mat.DataPointer;
        }

        public override void PutByte(byte[] data)
        {
            this.Image.Bytes = data;
            this.CudaImage.Upload(this.Image, this.TransferStream);
            this.TransferStream.WaitForCompletion();
        }

        public override void PutByte(IntPtr ptr, int pitch)
        {
            Mat mat = this.Image.Mat;
            Image<Gray, byte> newHostImage = new Image<Gray, byte>(mat.Width, mat.Height, pitch, ptr);
            this.Image?.Dispose();
            this.Image = newHostImage;

            this.CudaImage.Upload(this.Image, this.TransferStream);
            this.TransferStream.WaitForCompletion();
        }

        public void Create(int width, int height)
        {
            this.Image = new Image<Gray, byte>(width, height);
            this.CudaImage = new CudaImage<Gray, byte>(height, width);
        }

        public void Create(int width, int height, byte[] data)
        {
            if (Image != null)
                Image.Dispose();

            //System.Diagnostics.Debug.Assert(data.Length == pitch * height);

            //GCHandle pinnedArray = GCHandle.Alloc(data, GCHandleType.Pinned);
            //IntPtr ptr = pinnedArray.AddrOfPinnedObject();

            //Create(width, height, pitch, ptr);

            //pinnedArray.Free();

            this.Image = new Image<Gray, byte>(width, height);
            Marshal.Copy(data, 0, this.Image.Mat.DataPointer, data.Length);
            //this.hostImage.Bytes = data;

            this.CudaImage = new CudaImage<Gray, byte>(this.Image);
        }

        public void Create(int width, int height, int pitch, IntPtr data)
        {
            if (this.Image != null)
                this.Image.Dispose();

            this.Image = new Image<Gray, byte>(width, height, pitch, data);
            this.CudaImage = new CudaImage<Gray, byte>(this.Image);
        }

        public void UpdateHostImage()
        {
            this.CudaImage.Download(this.Image.Mat, this.TransferStream);

            //this.WaitTransferStream();
        }

        public override void Save(string fileName)
        {
            string dirName = Path.GetDirectoryName(fileName);
            Directory.CreateDirectory(dirName);

            UpdateHostImage();
            this.WaitStreamComplete();
            OpenCvGreyImage openCvGreyImage = new OpenCvGreyImage();
            openCvGreyImage.Image = this.Image;
            openCvGreyImage.Save(fileName);
        }

        public override ImageD ToImageD()
        {
            Image<Gray, byte> openCvImage = this.CudaImage.ToImage();
            ImageD imageD = OpenCvImageBuilder.ConvertImage(openCvImage);
            openCvImage.Dispose();
            return imageD;
        }



        //public override AlgoImage ConvertToMilImage(ImageType imageType)
        //{
        //    AlgoImage openCvImage = this.ConvertToOpenCvImage(imageType);
        //    AlgoImage milImage = openCvImage.ConvertToMilImage(imageType);
        //    //openCvImage.Dispose();
        //    return milImage;

        //    //return ImageBuilder.Build(ImagingLibrary.MatroxMIL, this.ToImageD(), imageType);
        //}

        //public override AlgoImage ConvertToOpenCvImage(ImageType imageType)
        //{
        //    AlgoImage algoImage;
        //    switch (imageType)
        //    {
        //        case ImageType.Grey:
        //            OpenCvGreyImage openCvGreyImage = new OpenCvGreyImage();
        //            //openCvGreyImage.Image = new Image<Gray, byte>(this.Width, this.Height);
        //            //this.image.Download(openCvGreyImage.Image);
        //            openCvGreyImage.Image = this.image.ToImage();
        //            algoImage = openCvGreyImage;
        //            break;

        //        default:
        //            throw new NotImplementedException();
        //    }

        //    this.SubImageList.Add(algoImage);
        //    algoImage.ParentImage = this;
        //    return algoImage;

        //}

        //protected override AlgoImage Convert2GreyImage()
        //{
        //    throw new NotImplementedException();
        //}

        //protected override AlgoImage Convert2ColorImage()
        //{
        //    throw new NotImplementedException();
        //}

        public override void Copy(AlgoImage srcImage, Rectangle srcRect)
        {
            Copy(srcImage, srcRect.Location, Point.Empty, srcRect.Size);
        }

        public override void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size)
        {
            OpenCvCudaImage openCvSrcImage = (OpenCvCudaImage)srcImage;

            Rectangle srcRect = new Rectangle(srcPt, size);
            Rectangle dstRect = new Rectangle(dstPt, size);

            CudaImage<Gray, byte> srcCudaImage = openCvSrcImage.CudaImage.ColRange(srcRect.Left, srcRect.Right).RowRange(srcRect.Top, srcRect.Bottom);
            CudaImage<Gray, byte> dstCudaImage = this.CudaImage.ColRange(dstRect.Left, dstRect.Right).RowRange(dstRect.Top, dstRect.Bottom);
            srcCudaImage.CopyTo(dstCudaImage, null, this.TransferStream);
            //this.transferStream?.WaitForCompletion();
        }

        public override void MaskingCopy(AlgoImage srcImage, AlgoImage maskImage)
        {
            OpenCvCudaImage openCvSrcImage = (OpenCvCudaImage)srcImage;
            OpenCvCudaImage openCvMaskImage = (OpenCvCudaImage)maskImage;

            openCvSrcImage.CudaImage.CopyTo(this.CudaImage, openCvMaskImage.Image, this.TransferStream);
        }
    }
}
