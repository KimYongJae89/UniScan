using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DynMvp.Base;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DynMvp.Vision
{
    public enum ImageBandType
    {
        Luminance, Red, Green, Blue
    }

    public enum ImageFilterType
    {
        EdgeExtraction, AverageFilter, HistogramEqualization, Binarization, Erode, Dilate
    }

    public abstract class AlgoImage : IDisposable
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public object Tag { get => this.tag; set => this.tag = value; }
        object tag = null;

        protected AlgoImage parentImage = null;
        public AlgoImage ParentImage
        {
            get { return parentImage; }
            set { parentImage = value; }
        }

        protected List<AlgoImage> subImageList = new List<AlgoImage>();
        public List<AlgoImage> SubImageList
        {
            get { return subImageList; }
        }
        public abstract bool IsAllocated { get; }

        private ImageType imageType;
        public ImageType ImageType
        {
            get { return imageType; }
            set { imageType = value; }
        }

        private ImagingLibrary libraryType;
        public ImagingLibrary LibraryType
        {
            get { return libraryType; }
            set { libraryType = value; }
        }

        private DateTime dateTime = DateTime.Now;
        public DateTime DateTime { get => this.dateTime; set => this.dateTime = value; }
        //private byte[] byteData = null;

        public IntPtr Ptr
        {
            get
            {
                if (this.ptr == IntPtr.Zero)
                    this.ptr = this.GetImagePtr();
                return this.ptr;
            }
        }
        protected IntPtr ptr = IntPtr.Zero;

        public Size Size
        {
            get { return new Size(Width, Height); }
        }

        public void SetByte(byte[] data)
        {
            PutByte(data);
        }

        public bool HasChild
        {
            get { return this.subImageList.Count > 0; }
        }

        public void DisposeChild()
        {
            foreach (AlgoImage subImage in this.subImageList)
            {
                subImage.DisposeChild();
                subImage.Dispose();
            }
        }

        public virtual bool IsStreamCompleted { get => true; }
        public virtual void WaitStreamComplete() { }

        public byte[] GetByte()
        {
            byte[] bytes = new byte[this.Pitch * this.Height];
            GetByte(bytes);
            return bytes;
        }

        public void Dispose()
        {
            Disposing();
            this.ptr = IntPtr.Zero;
        }

        public abstract void GetByte(byte[] bytes);
        public abstract void PutByte(byte[] data);
        public abstract void PutByte(IntPtr ptr, int pitch);
        public abstract int Width { get; }
   
        protected abstract void Disposing();
        public abstract int Height { get; }
        public abstract int Pitch { get; }
        public virtual int PitchLib { get => Pitch; }
        public abstract int BitPerPixel { get; }
        public abstract AlgoImage Clone();
        public abstract ImageD ToImageD();
        public abstract AlgoImage Clip(Rectangle rectangle);
        public abstract void Save(string fileName);
        public void Save(DebugContext debugContext)
        {
            if (debugContext == null || debugContext.SaveDebugImage == false)
                return;

            Save(Path.Combine(debugContext.FullPath, this.name));
        }
        public void Save(string fileName, DebugContext debugContext)
        {
            if (debugContext != null && debugContext.SaveDebugImage == false)
                return;

            string fullName = "";
            if (debugContext != null)
                fullName = Path.Combine(debugContext.FullPath, fileName);
            else
                fullName = fileName;

            Save(fullName);
        }

        public void Save(string fileName, SizeF scale, DebugContext debugContext)
        {
            if (debugContext != null && debugContext.SaveDebugImage == false)
                return ;

            ImageProcessing processing = AlgorithmBuilder.GetImageProcessing(this);
            int w = Math.Max((int)Math.Round(this.Width * scale.Width), 1);
            int h = Math.Max((int)Math.Round(this.Height * scale.Height), 1);
            AlgoImage resize = ImageBuilder.Build(this.libraryType, this.ImageType, w, h);
            processing.Resize(this, resize, -1, -1);
            resize.Save(fileName, debugContext);
            resize.Dispose();
        }
        public void Save(string fileName, float scale, DebugContext debugContext) {  Save(fileName, new SizeF(scale, scale), debugContext); }
        public abstract void Clear(byte initVal = 0);
        public abstract void MaskingCopy(AlgoImage srcImage, AlgoImage maskImage);
        public abstract void Copy(AlgoImage srcImage, Rectangle srcRect);
        public abstract void Copy(AlgoImage srcImage, Point srcPt, Point dstPt, Size size);
        public void Copy(AlgoImage srcImage) { Copy(srcImage, new Rectangle(0, 0, srcImage.Width, srcImage.Height)); }
        protected abstract void GetSubImage(Rectangle rectangle, out AlgoImage dstImage);
        public AlgoImage GetSubImage(Rectangle rectangle)
        {
            Rectangle adjestRect = Rectangle.Intersect(new Rectangle(Point.Empty, this.Size), rectangle);
            if (rectangle != adjestRect || rectangle.Width <= 0 || rectangle.Height <= 0)
            {
                LogHelper.Error(LoggerType.Error, new Exception($"AlgoImage::GetSubImage - out of image size. Image.Size: {this.Size}, Rectangle: {rectangle}"), true);
                //System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace();
                //LogHelper.Error(LoggerType.Error, string.Format("ImageSize: {0}, Rectangle: L{1} T{2} R{3} B{4} W{5} H{6}",
                //    this.Size.ToString(), rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom, rectangle.Width, rectangle.Height));
                //LogHelper.Error(LoggerType.Error, string.Format("AlgoImage::GetSubImage - Invalid Rectangle{0}{1}", Environment.NewLine, st.ToString()));
            }

            AlgoImage subImage;
            GetSubImage(rectangle, out subImage);
            return subImage;
        }

        public abstract IntPtr GetImagePtr();

        List<ImageFilterType> filteredList = new List<ImageFilterType>();
        public List<ImageFilterType> FilteredList
        {
            get { return filteredList; }
            set { filteredList = value; }
        }

        public bool CheckFilterd(ImageFilterType imageFilterType)
        {
            return filteredList.Contains(imageFilterType);
        }

        public bool IsCompatible(string algorithmType)
        {
            AlgorithmStrategy strategy = AlgorithmBuilder.GetStrategy(algorithmType);
            return IsCompatible(strategy);
        }

        public bool IsCompatible(AlgorithmStrategy strategy)
        {
            if (strategy == null)
                return false;

            return IsCompatible(strategy.LibraryType, strategy.ImageType);
        }

        public bool IsCompatible(AlgoImage algoImage)
        {
            return IsCompatible(algoImage.LibraryType, algoImage.ImageType);
        }

        public bool IsCompatible(ImagingLibrary imagingLibrary, ImageType imageType)
        {
            return (this.LibraryType == imagingLibrary && this.ImageType == imageType);
        }

        public AlgoImage ConvertTo(AlgorithmStrategy strategy)
        {
            if (strategy == null)
                throw new ArgumentNullException("[AlgoImage.ConvertTo] strategy is null");

            return ConvertTo(strategy.LibraryType, strategy.ImageType);
        }

        public AlgoImage ConvertTo(string algorithmType)
        {
            ImagingLibrary libraryType = ImagingLibrary.OpenCv;
            ImageType imageType = ImageType.Grey;

            AlgorithmStrategy strategy = AlgorithmBuilder.GetStrategy(algorithmType);
            if (strategy != null)
            {
                libraryType = strategy.LibraryType;
                imageType = strategy.ImageType;
            }

            if(imageType == ImageType.Gpu)
                libraryType = ImagingLibrary.OpenCv;

            return ConvertTo(libraryType, imageType);
        }

        public AlgoImage ConvertTo(ImageType imageType)
        {
            return ConvertTo(this.libraryType, imageType);
        }

        public AlgoImage ConvertTo(ImagingLibrary imagingLibrary, ImageType imageType)
        {
            return ImageConverter.Convert(this, imagingLibrary, imageType);

            //byte[] bytes = this.GetByte();

            //AlgoImage convertImage = null;
            //if (imageType == ImageType.Binary)
            //{
            //    convertImage = ImageBuilder.Build(imagingLibrary, imageType, this.Width, this.Height);
            //    if (imagingLibrary == ImagingLibrary.MatroxMIL)
            //    {
            //        AlgoImage milImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, this.Width, this.Height);
            //        milImage.PutByte(bytes);
            //        convertImage.Copy(milImage);
            //        milImage.Dispose();
            //    }
            //    else
            //        throw new NotImplementedException();
            //}
            //else
            //{
            //    int numBand = this.ImageType == ImageType.Color ? 3 : 1;
            //    Image2D image2D = new Image2D(this.Width, this.Height, numBand, this.Width, bytes);
            //    convertImage = ImageBuilder.Build(imagingLibrary, image2D, imageType);
            //}

            //return convertImage;
        }

        public bool IsInnerRect(Rectangle rectangle)
        {
            if (rectangle.Width <= 0 || rectangle.Height <= 0)
                return false;

            Rectangle intersect = Rectangle.Intersect(rectangle, new Rectangle(Point.Empty, this.Size));
            return rectangle == intersect;
        }

        public virtual Bitmap ToBitmap()
        {
            PixelFormat pixelFormat = this.imageType == ImageType.Color ? PixelFormat.Format24bppRgb : PixelFormat.Format8bppIndexed;
            Rectangle rectangle = new Rectangle(Point.Empty, this.Size);
            IntPtr imagePtr = this.Ptr;

            byte[] bytes = GetByte();
            int byteLength = bytes.Length;

            Bitmap bitmap = new Bitmap(Width, Height, pixelFormat);
            ImageHelper.SetGrayColorPallet(bitmap);
            BitmapData bitmapData = bitmap.LockBits(rectangle, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            int imageLength = bitmapData.Stride * bitmapData.Height;

            if (byteLength == imageLength)
            {
                Marshal.Copy(bytes, 0, bitmapData.Scan0, byteLength);
            }
            else
            {
                int pitchBytes = byteLength / bitmapData.Height;
                for (int h = 0; h < bitmapData.Height; h++)
                {
                    int srcOffset = pitchBytes * h;
                    int dstOffset = bitmapData.Stride * h;
                    Marshal.Copy(bytes, srcOffset, bitmapData.Scan0 + dstOffset, this.Width * this.BitPerPixel / 8);
                }
            }
            bitmap.UnlockBits(bitmapData);

            //bitmap.Save(@"C:\temp\bitmap.bmp");
            return bitmap;
        }

        public BitmapSource ToBitmapSource()
        {
            BitmapSource bitmapSource = null;

            switch (this.imageType)
            {
                case ImageType.Grey:
                    bitmapSource = BitmapSource.Create(this.Width, this.Height, 96, 96, System.Windows.Media.PixelFormats.Gray8, null, GetByte(), this.Width);
                    break;
                case ImageType.Color:
                    bitmapSource = BitmapSource.Create(this.Width, this.Height, 96, 96, System.Windows.Media.PixelFormats.Bgr24, null, GetByte(), this.Width * 3);
                    break;
            }
            bitmapSource?.Freeze();

            return bitmapSource;
        }
    }
}
