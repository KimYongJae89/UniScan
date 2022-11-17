using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using DynMvp.UI;
using System.Threading.Tasks;
using DynMvp.Base;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Globalization;

namespace DynMvp.Base
{
    public class TypeConverterImageD : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is Image2D)
                return ((Image2D)value).Size.ToString();
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class ImageBase<T> : IDisposable
    {
        T[] data;
        public T[] Data
        {
            get { return data; }
        }

        IntPtr dataPtr = IntPtr.Zero;
        public IntPtr DataPtr
        {
            get { return dataPtr; }
        }

        int width;
        public int Width
        {
            get { return width; }
        }

        int height;
        public int Height
        {
            get { return height; }
        }

        int pitch;
        public int Pitch
        {
            get { return pitch; }
        }

        int numBand;
        public int NumBand
        {
            get { return numBand; }
        }

        int dataSize;
        public int DataSize
        {
            get { return dataSize; }
        }

        ~ImageBase()
        {
            Dispose();
        }

        public void Initialize(int width, int height, int numBand, int pitch = 0, T[] data = null)
        {
            try
            {
                this.width = width;
                this.height = height;
                this.numBand = numBand;
                if (pitch == 0)
                    this.pitch = (width * numBand + 3) / 4 * 4;
                else
                    this.pitch = pitch;

                if (data != null)
                    SetData(data);
                else
                    SetData(new T[this.pitch * this.height]);

                if (this.data.Length > 0)
                    dataSize = Marshal.SizeOf(this.data[0]);

                Debug.Assert(this.data.Length == this.pitch * height);
            }
            catch (OverflowException)
            {

            }
        }

        public void Initialize(int width, int height, int numBand, int pitch, IntPtr dataPtr)
        {
            this.width = width;
            this.height = height;
            this.numBand = numBand;
            if (pitch == 0)
                this.pitch = width * numBand;
            else
                this.pitch = pitch;

            SetDataPtr(dataPtr);

            T[] testData = new T[1];
            this.dataSize = System.Runtime.InteropServices.Marshal.SizeOf(testData[0]);
        }

        public void SetData(T[] data)
        {
            this.data = data;
        }

        public void SetDataPtr(IntPtr dataPtr)
        {
            this.dataPtr = dataPtr;
        }

        public void Clear(T value)
        {
            if (data == null)
                return;

            //if (value == 0)
            //{
            //    Array.Clear(data, 0, data.Length);
            //}
            //else
            {
                for (int i = 0; i < data.Length; i++)
                    data[i] = value;
            }
        }

        public ImageBase<T> GetLayer(int index)
        {
            ImageBase<T> imageData = new ImageBase<T>();
            imageData.Initialize(width, height, 1);

            int size = pitch * height;
            Buffer.BlockCopy(data, size * index, imageData.Data, 0, size);

            return imageData;
        }

        public void Copy(T[] dataSrc)
        {
            int size = pitch * height;
            if (dataSrc.Count() != size)
            {
                for (int h = 0; h < height; h++)
                {
                    Buffer.BlockCopy(dataSrc, pitch * h, data, pitch * h, pitch);
                }
                //LogHelper.Warn(LoggerType.Operation, StringManager.GetString(this.GetType().FullName, "Image Data size is not match."));
                LogHelper.Warn(LoggerType.Operation, "Image Data size is not match.");
                return;
            }

            if (data == null)
                data = new T[size];

            Buffer.BlockCopy(dataSrc, 0, data, 0, size * dataSize);
        }

        public void Copy(T[] dataSrc, Rectangle srcRect, int srcPitch, Point destPt)
        {
            //for (int y = 0; y < srcRect.Height; y++)
            Parallel.For(0, srcRect.Height, y =>
            {
                try
                {
                    Buffer.BlockCopy(dataSrc, (y + srcRect.Y) * srcPitch + srcRect.X * numBand, data, (destPt.Y + y) * pitch + destPt.X * numBand, srcRect.Width * numBand);
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Error, "ImageBase::Copy " + ex.Message);
                }
            }
            );
        }

        public void Save(string fileName) //save RAW
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);

            binaryWriter.Write(width);
            binaryWriter.Write(height);
            binaryWriter.Write(pitch);
            binaryWriter.Write(numBand);
            byte[] byteReserved = new byte[1000];

            byte[] byteBuffer = new byte[data.Length * dataSize];
            Buffer.BlockCopy(data, 0, byteBuffer, 0, byteBuffer.Length);

            binaryWriter.Write(byteBuffer, 0, byteBuffer.Length);
            fileStream.Close();
        }

        public void SaveRaw(string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);

            binaryWriter.Write(width);
            binaryWriter.Write(height);
            binaryWriter.Write(numBand);

            byte[] byteBuffer = new byte[data.Length * dataSize];
            Buffer.BlockCopy(data, 0, byteBuffer, 0, byteBuffer.Length);

            binaryWriter.Write(byteBuffer, 0, byteBuffer.Length);
        }

        public void Load(string fileName)
        {
            if (File.Exists(fileName) == false)
                return;

            FileStream fileStream = new FileStream(fileName, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(fileStream);

            width = binaryReader.ReadInt32();
            height = binaryReader.ReadInt32();
            pitch = binaryReader.ReadInt32();
            numBand = binaryReader.ReadInt32();
            //            byte[] byteReserved = binaryReader.ReadBytes(1000);

            data = new T[width * height * numBand];
            dataSize = System.Runtime.InteropServices.Marshal.SizeOf(data[0]);

            int size = width * height * numBand * dataSize;
            byte[] byteBuffer = binaryReader.ReadBytes(size);

            Buffer.BlockCopy(byteBuffer, 0, data, 0, size);

            dataPtr = System.Runtime.InteropServices.GCHandle.Alloc(data, GCHandleType.Pinned).AddrOfPinnedObject();

            binaryReader.Dispose();
            fileStream.Dispose();
        }

        public float[] GetRangeValue(Point point, int range = 5)
        {
            float[] sum = new float[numBand];

            int count = 0;

            try
            {
                for (int y = point.Y - range; y <= point.Y + range; y++)
                {
                    for (int x = point.X - range; x <= point.X + range; x++)
                    {
                        int index = (y * pitch) + x * numBand;

                        sum[0] += float.Parse(data[index].ToString());
                        count++;

                        if (numBand == 3)
                        {
                            sum[1] += float.Parse(data[index + 1].ToString());
                            sum[2] += float.Parse(data[index + 2].ToString());
                        }
                    }
                }
            }
            catch (FormatException ex)
            {

            }

            for (int i = 0; i < numBand; i++)
                sum[i] /= count;

            return sum;
        }

        public void Clip(ImageBase<T> destImageData, Rectangle rectangle)
        {
            int xStart = (rectangle.Left < 0 ? 0 : rectangle.Left);
            int yStart = (rectangle.Top < 0 ? 0 : rectangle.Top);
            int xEnd = (rectangle.Right > width ? width : rectangle.Right);
            int yEnd = (rectangle.Bottom > height ? height : rectangle.Bottom);
            int realWidth = Math.Min(rectangle.Width, xEnd - xStart) * numBand * dataSize;

            if (dataPtr == IntPtr.Zero)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    int srcIndex = y * pitch + xStart * NumBand;
                    int destIndex = (y - yStart) * destImageData.Pitch;
                    Array.Copy(data, srcIndex, destImageData.Data, destIndex, realWidth);
                }
            }
            else
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    byte[] bufferLine = new byte[realWidth];
                    int srcIndex = y * pitch + xStart * NumBand;
                    int destIndex = (y - yStart) * destImageData.Pitch;
                    Marshal.Copy(dataPtr + srcIndex, bufferLine, 0, realWidth);
                    Array.Copy(bufferLine, 0, destImageData.Data, destIndex, realWidth);
                }
            }
        }

        public void Dispose()
        {
            if (data != null)
            {
                Array.Resize(ref data, 0);
            }
            data = null;
            dataPtr = IntPtr.Zero;
            this.dataSize = 0;
        }
    }

    [TypeConverter(typeof(TypeConverterImageD))]
    public abstract class ImageD : IDisposable
    {
        public object Tag;
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract int Pitch { get; }
        public abstract int NumBand { get; }
        public abstract int DataSize { get; }

        public Size Size
        {
            get { return new Size(Width, Height); }
        }

        Rectangle roi = Rectangle.Empty;
        public Rectangle Roi
        {
            get { return roi; }
            set { roi = value; }
        }

        public abstract void Initialize(int width, int height, int numBand, int pitch = 0);

        public abstract ImageD Clone();

        public abstract void CopyFrom(ImageD imageSrc);
        public abstract void CopyFrom(ImageD imageSrc, Rectangle srcRect, int srcPitch, Point destPt);
        public abstract Bitmap ToBitmap();
        public abstract void Clear();

        public abstract void ConvertFromPtr(int srcPitch = 0);
        public abstract void ConvertFromData();

        public abstract ImageD GetLayer(int index);

        public abstract void Save(string fileName);
        public abstract void Load(string fileName);

        public void SaveImage(string fileName)
        {
            ImageFormat imageFormat;
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".bmp":
                default:
                    imageFormat = ImageFormat.Bmp;
                    break;
                case ".jpg":
                case ".jpeg":
                    imageFormat = ImageFormat.Jpeg;
                    break;
                case ".png":
                    imageFormat = ImageFormat.Png;
                    break;
            }
            SaveImage(fileName, imageFormat);
        }
        public abstract void SaveImage(string fileName, ImageFormat imageFormat);
        public abstract void LoadImage(string fileName);

        public abstract ImageD ClipImage(RotatedRect rotatedRect);
        public abstract ImageD ClipImage(Rectangle rectangle);

        public abstract float GetAverage();
        public abstract float GetMax();
        public abstract float GetMin();

        public abstract ImageD Not();
        public abstract void Mul(float mul);
        public abstract ImageD FlipX();
        public abstract ImageD FlipY();

        public abstract void RotateFlip(RotateFlipType rotateFlipType);

        public bool IsSame(ImageD imageD)
        {
            return imageD.Width == Width && imageD.Height == Height && imageD.Pitch == Pitch && imageD.NumBand == NumBand && imageD.DataSize == DataSize;
        }

        //        public abstract float GetValue(Point point);
        public abstract float[] GetRangeValue(Point point, int range = 5);
        public abstract ImageD Resize(int destWidth, int destHeight);
        public ImageD Resize(float v1) { return this.Resize(v1, v1); }
        public ImageD Resize(float v1, float v2)
        {
            return this.Resize((int)(this.Width * v1), (int)(this.Height * v2));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public abstract void FreeImageData();

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    FreeImageData();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ImageD() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }


    public class Image2D : ImageD
    {
        //public ImageBase<byte> ImageData { get { return imageData; } }
        ImageBase<byte> imageData = null;

        public override int Width { get { return imageData.Width; } }
        public override int Height { get { return imageData.Height; } }
        public override int Pitch { get { return imageData.Pitch; } }
        public override int NumBand { get { return imageData.NumBand; } }
        public override int DataSize { get { return imageData.DataSize; } }

        public byte[] Data { get { return imageData == null ? null : imageData.Data; } }
        public IntPtr DataPtr { get { return imageData == null ? IntPtr.Zero : imageData.DataPtr; } }
        public bool IsValid => this.imageData != null;

        public override void ConvertFromData()
        {
            if (imageData.Data != null)
            {
                GCHandle gcHandle = GCHandle.Alloc(imageData.Data, GCHandleType.Pinned);
                imageData.SetDataPtr(gcHandle.AddrOfPinnedObject());
                gcHandle.Free();
            }
        }

        public override void ConvertFromPtr(int srcPitch = 0)
        {
            if (imageData.DataPtr != IntPtr.Zero)
            {
                if (srcPitch == 0)
                    srcPitch = imageData.Pitch;

                int size = imageData.Pitch * imageData.Height;
                byte[] data = new byte[size];

                if (srcPitch == imageData.Pitch)
                {
                    Marshal.Copy(imageData.DataPtr, data, 0, size);
                }
                else
                {
                    for (int y = 0; y < imageData.Height; y++)
                    {
                        int ptrOffset = y * srcPitch;
                        int idxOffset = y * imageData.Pitch;
                        Marshal.Copy(imageData.DataPtr + ptrOffset, data, idxOffset, imageData.Pitch);
                    }
                }

                imageData.SetData(data);
                imageData.SetDataPtr(IntPtr.Zero);
            }
        }

        public Image2D()
        {
        }

        public Image2D(int width, int height, int numBand, int pitch = 0)
        {
            Initialize(width, height, numBand, pitch);
        }

        public Image2D(int width, int height, int numBand, int pitch, byte[] data)
        {
            Initialize(width, height, numBand, pitch, data);
        }

        public Image2D(int width, int height, int numBand, int pitch, IntPtr data)
        {
            Initialize(width, height, numBand, pitch, data);
        }

        public Image2D(string fileName)
        {
            LoadImage(fileName);
        }

        public override void Initialize(int width, int height, int numBand, int pitch = 0)
        {
            imageData = new ImageBase<byte>();
            imageData.Initialize(width, height, numBand, pitch);
        }

        public void Initialize(int width, int height, int numBand, int pitch, byte[] data)
        {
            imageData = new ImageBase<byte>();
            imageData.Initialize(width, height, numBand, pitch, data);
        }

        public void Initialize(int width, int height, int numBand, int pitch, IntPtr data)
        {
            imageData = new ImageBase<byte>();
            imageData.Initialize(width, height, numBand, pitch, data);
        }

        public bool IsUseIntPtr()
        {
            return imageData.DataPtr != IntPtr.Zero;
        }

        public override void FreeImageData()
        {
            imageData.Dispose();
            imageData = null;
        }

        public override ImageD Clone()
        {
            Image2D cloneImage = new Image2D();
            if (imageData.Data == null)
                cloneImage.Initialize(Width, Height, NumBand, Pitch, imageData.DataPtr);
            else
                cloneImage.Initialize(Width, Height, NumBand, Pitch);

            if (imageData.Data != null)
            {
                cloneImage.imageData.Copy(imageData.Data);
            }

            cloneImage.Tag = this.Tag;

            return cloneImage;
        }

        public override void CopyFrom(ImageD imageSrc)
        {
            Debug.Assert(imageSrc is Image2D);
            Debug.Assert(IsSame(imageSrc));

            Image2D imageSrc2d = (Image2D)imageSrc;

            bool srcIsPtr = imageSrc2d.IsUseIntPtr();
            bool dstIsPtr = this.IsUseIntPtr();

            if (srcIsPtr && dstIsPtr)
            {
                //memCopy;
            }
            else if (srcIsPtr && !dstIsPtr)
            {
                Marshal.Copy(imageSrc2d.DataPtr, this.Data, 0, this.Data.Length);
            }
            else if (!srcIsPtr && dstIsPtr)
            {
                Marshal.Copy(imageSrc2d.Data, 0, this.DataPtr, imageSrc2d.Data.Length);
            }
            else if (!srcIsPtr && !dstIsPtr)
            {
                Buffer.BlockCopy(imageSrc2d.Data, 0, this.Data, 0, this.Data.Length);
            }
            //imageData.SetDataPtr(imageSrc2d.DataPtr);
            //if (imageSrc2d.Data != null)
            //    imageData.Copy(imageSrc2d.Data);
        }

        public override void CopyFrom(ImageD imageSrc, Rectangle srcRect, int srcPitch, Point destPt)
        {
            if (srcRect.IsEmpty)
                srcRect = new Rectangle(Point.Empty, imageSrc.Size);

            Image2D imageSrc2d = (Image2D)imageSrc;
            if (imageData.NumBand == imageSrc2d.NumBand)
            {
                imageData.Copy(imageSrc2d.Data, srcRect, srcPitch, destPt);
            }
            else if (imageData.NumBand == 3)
            {
                Image2D colorImage = (Image2D)imageSrc2d.GetColorImage();
                imageData.Copy(colorImage.Data, srcRect, colorImage.Pitch, destPt);
            }
            else
            {
                Image2D grayImage = (Image2D)imageSrc2d.GetGrayImage();
                imageData.Copy(grayImage.Data, srcRect, grayImage.Pitch, destPt);
            }
        }

        public void SetData(IntPtr srcPtr)
        {
            imageData.SetDataPtr(srcPtr);
        }

        public override ImageD GetLayer(int index)
        {
            Image2D layerImage = new Image2D();
            layerImage.Initialize(Width, Height, 1);
            layerImage.SetData(imageData.GetLayer(index).Data);

            return layerImage;
        }

        public void SetData(byte[] dataSrc)
        {
            if (imageData != null)
                imageData.Copy(dataSrc);
        }

        public override void Clear()
        {
            if (imageData != null)
                imageData.Clear(0);

            if (DataPtr != IntPtr.Zero)
            {
                Bitmap bitmap = new Bitmap(imageData.Width, imageData.Height, imageData.Pitch, PixelFormat.Format8bppIndexed, imageData.DataPtr);

                ImageHelper.Clear(bitmap, 0);
                /*for (int y = 0; y < imageData.Height; y++)
                {
                    Parallel.For(0, imageData.Width, x =>
                    {
                        bitmap.SetPixel(x, y, Color.Black);
                    });
                }*/
            }
        }

        public override Bitmap ToBitmap()
        {
            //LogHelper.Debug(LoggerType.Function, "Image2D::ToBitmap");
            Bitmap bitMap = null;
            try
            {
                if (IsUseIntPtr() == true)
                    bitMap = ImageHelper.CreateBitmap(Width, Height, Pitch, NumBand, DataPtr);
                else
                    bitMap = ImageHelper.CreateBitmap(Width, Height, Pitch, NumBand, Data);
                return bitMap;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, "ImageD::ToBitmap Exception: " + ex.Message);
                return null;
            }
        }

        public static Image2D FromBitmap(Bitmap bitmap)
        {
            if (bitmap == null)
                return null;

            int numBand = 1;

            if (bitmap.PixelFormat == PixelFormat.Format32bppArgb)
            {
                numBand = 4;
            }
            else if (bitmap.PixelFormat == PixelFormat.Format24bppRgb)
            {
                numBand = 3;
            }
            else if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                numBand = 1;
            }
            else
            {
                //Debug.Assert(false, StringManager.GetString(this.GetType().FullName, "DynMvp is support only 8bit or 24bit bitmap"));
                return null;
            }

            //if ((stride % 4) != 0)
            //    stride = stride + (4 - stride % 4);//4바이트 배수가 아닐시..

            Image2D image2d = new Image2D();
            image2d.Initialize(bitmap.Width, bitmap.Height, numBand);

            Rectangle srcRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            BitmapData srcBmpData = bitmap.LockBits(srcRect, ImageLockMode.ReadWrite, bitmap.PixelFormat);

            IntPtr srcPtr = srcBmpData.Scan0;
            //image2d.Initialize(bitmap.Width, bitmap.Height, numBand, stride, srcPtr);
            image2d.SetData(srcBmpData.Scan0);
            image2d.ConvertFromPtr(srcBmpData.Stride); //여기시 deep-copy

            bitmap.UnlockBits(srcBmpData);

            return image2d;
        }

        public override void SaveImage(string fileName, ImageFormat imageFormat)
        {
            Debug.Assert(imageData != null);

            bool ok = false;

            if (imageFormat == ImageFormat.Bmp && this.NumBand == 1)
            {
                int p = (int)(this.Width / 4f + 0.75f) * 4;
                int l = this.Width * 1;

                string dirName = Path.GetDirectoryName(fileName);
                if (Directory.Exists(dirName) == false)
                    Directory.CreateDirectory(dirName);

                using (Stream stream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    int fihSize = 14;
                    int bihSize = 40;
                    int dataSize = p * this.Height;
                    int fileSize = fihSize + bihSize + dataSize;
                    short bpp = (short)(this.NumBand * 8);

                    stream.WriteByte((byte)'B');
                    stream.WriteByte((byte)'M');
                    stream.Write(BitConverter.GetBytes(fileSize), 0, 4);
                    stream.WriteByte((byte)0);
                    stream.WriteByte((byte)0);
                    stream.WriteByte((byte)0);
                    stream.WriteByte((byte)0);
                    stream.Write(BitConverter.GetBytes(fihSize + bihSize), 0, 4);

                    stream.Write(BitConverter.GetBytes(bihSize), 0, 4); // biSize
                    stream.Write(BitConverter.GetBytes(this.Width), 0, 4); // biWidth
                    stream.Write(BitConverter.GetBytes(this.Height), 0, 4); // biHeight
                    stream.Write(BitConverter.GetBytes((short)1), 0, 2); // biPlanes
                    stream.Write(BitConverter.GetBytes(bpp), 0, 2); // biBitCount
                    stream.Write(BitConverter.GetBytes((int)0), 0, 4); // biCompression
                    stream.Write(BitConverter.GetBytes(dataSize), 0, 4); // biSizeImage
                    stream.Write(BitConverter.GetBytes(96), 0, 4); // biXPelsPerMeter
                    stream.Write(BitConverter.GetBytes(96), 0, 4); // biYPelsPerMeter
                    stream.Write(BitConverter.GetBytes(0), 0, 4); // biClrUsed
                    stream.Write(BitConverter.GetBytes(bpp == 8 ? 256 : 0), 0, 4); // biClrImportant

                    //List<byte> byteList = new List<byte>();
                    //byteList.AddRange(new byte[2] { (byte)'B', (byte)'M' }); // Magin Code 2
                    //byteList.AddRange(BitConverter.GetBytes(fileSize)); // FileSize 4
                    //byteList.AddRange(new byte[2]); // Reserved 2
                    //byteList.AddRange(new byte[2]); // Reserved 2
                    //byteList.AddRange(BitConverter.GetBytes(fihSize + bihSize)); // Data offset 4

                    //byteList.AddRange(BitConverter.GetBytes(bihSize)); // biSize 4
                    //byteList.AddRange(BitConverter.GetBytes(this.Width)); // biWidth 4
                    //byteList.AddRange(BitConverter.GetBytes(this.Height)); // biHeight 4
                    //byteList.AddRange(BitConverter.GetBytes((short)1)); // biPlanes 2
                    //byteList.AddRange(BitConverter.GetBytes(bpp)); // biBitCount 2
                    //byteList.AddRange(BitConverter.GetBytes((int)0)); // biCompression 4
                    //byteList.AddRange(BitConverter.GetBytes(dataSize)); // biSizeImage
                    //byteList.AddRange(BitConverter.GetBytes(96)); // biXPelsPerMeter 4
                    //byteList.AddRange(BitConverter.GetBytes(96)); // biYPelsPerMeter 4
                    //byteList.AddRange(BitConverter.GetBytes(0)); // biClrUsed 4
                    //byteList.AddRange(BitConverter.GetBytes(bpp == 8 ? 256 : 0)); // biClrImportant 4
                    if (bpp == 8)
                    {
                        for (int i = 0; i < 256; i++)
                            stream.Write(BitConverter.GetBytes(Color.FromArgb(0xff, i, i, i).ToArgb()), 0, 4); // biClrImportant 4
                            //byteList.AddRange(BitConverter.GetBytes(Color.FromArgb(0xff, i, i, i).ToArgb())); // biClrImportant 4
                    }

                    if (this.imageData.DataPtr == IntPtr.Zero)
                    {
                        for (int h = 0; h < this.Height; h++)
                        {
                            int offsetSrc = (this.Height - h - 1) * this.Pitch;

                            stream.Write(this.Data, offsetSrc, l);
                            //Array.Copy(this.Data, offsetSrc, rowBytes, 0, this.Pitch);
                            //byteList.AddRange(rowBytes);
                        }
                    }
                    else
                    {
                        byte[] rowBytes = new byte[p];
                        for (int h = 0; h < this.Height; h++)
                        {
                            int offsetSrc = (this.Height - h - 1) * this.Pitch;
                            int offsetDst = (h) * this.Pitch;
                            Marshal.Copy(this.imageData.DataPtr + offsetSrc, rowBytes, 0, l);

                            stream.Write(rowBytes, 0, p);
                            //byteList.AddRange(rowBytes);
                        }
                    }

                    //byte[] bytes = byteList.ToArray();
                    //stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                    stream.Close();
                }
                ok = true;
            }

            if (!ok)
            {
                Bitmap bitmap = ImageHelper.CreateBitmap(Width, Height, Pitch, NumBand, this.imageData.Data);
                if (bitmap != null)
                {
                    ImageHelper.SaveImage(bitmap, fileName, imageFormat);
                    bitmap.Dispose();
                }
            }
        }

        //public override void SaveImage(string fileName, ImageFormat imageFormat)
        //{
        //    Debug.Assert(imageData != null);
        //    //LogHelper.Debug(LoggerType.Function, string.Format("Image2D::SaveImage - {0}", fileName));

        //    string path = Path.GetDirectoryName(fileName);
        //    Directory.CreateDirectory(path);

        //    Bitmap bitmap = ToBitmap();
        //    if (bitmap != null)
        //    {
        //        ImageHelper.SaveImage(bitmap, fileName, imageFormat);
        //        bitmap?.Dispose();
        //    }
        //    //LogHelper.Debug(LoggerType.Function, "Image2D::SaveImage End");

        //}

        public override void LoadImage(string fileName)
        {
            //Debug.WriteLine(string.Format("Image2D::LoadImage - {0}", fileName));
            LogHelper.Debug(LoggerType.Function, string.Format("Image2D::LoadImage - {0}", fileName));

            bool ok = false;
            string extention = Path.GetExtension(fileName).ToLower();
            using (StreamReader sr = new StreamReader(fileName))
            {
                switch (extention)
                {
                    case ".bmp":
                        ok = LoadBMP(sr.BaseStream);
                        break;
                }

                if (ok == false)
                {
                    sr.BaseStream.Position = 0;

                    // Old Code
                    Bitmap bitmap = Image.FromStream(sr.BaseStream, false, false) as Bitmap;

                    if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
                    {
                        byte[] bytes = ImageHelper.GetByte(bitmap);
                        int bytePerPixel = Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;

                        this.imageData = new ImageBase<byte>();
                        this.imageData.Initialize(bitmap.Width, bitmap.Height, bytePerPixel);
                        this.imageData.SetData(bytes);
                    }
                    else
                    // Gray 영상만 로드함...
                    {
                        byte[] bytes = ImageHelper.GetGrayByte(bitmap);

                        this.imageData = new ImageBase<byte>();
                        this.imageData.Initialize(bitmap.Width, bitmap.Height, 1);
                        this.imageData.SetData(bytes);
                    }
                    bitmap.Dispose();
                }
            }
            //LogHelper.Debug(LoggerType.Function, "Image2D::LoadImage End");
        }

        private bool LoadBMP(Stream stream)
        {
            stream.Position = 0;

            // Support Extrime Size
            long headerSize = 54;//sr.BaseStream.Length;
            byte[] headerData = new byte[headerSize];
            stream.Read(headerData, 0, headerData.Length);

            if (!(headerData[0] == 'B' && headerData[1] == 'M'))
                return false;

            int width = headerData[21] << 24 | headerData[20] << 16 | headerData[19] << 8 | headerData[18];
            int height = headerData[25] << 24 | headerData[24] << 16 | headerData[23] << 8 | headerData[22];
            int bpp = headerData[29] << 8 | headerData[28];
            int pitch = ((width * bpp + 31) / 32) * 4;

            int dataStartIdx = 54 + (bpp == 8 ? 4 * 256 : 0);
            long dataSize = stream.Length - dataStartIdx;
            int imageSize = pitch * height;
            byte[] imageData = new byte[imageSize];

            stream.Position = dataStartIdx;
            for (int h = 0; h < height; h++)
            {
                int offset = (height - h - 1) * pitch;
                stream.Read(imageData, offset, pitch);
            }

            this.imageData = new ImageBase<byte>();
            this.imageData.Initialize(width, height, bpp / 8, pitch, imageData);

            return true;
        }

        private bool LoadPNG(Stream stream)
        {
            throw new NotImplementedException();
        }

        public override void Save(string fileName)
        {
            imageData.Save(fileName);
        }

        public override void Load(string fileName)
        {
            imageData.Load(fileName);
        }

        public override ImageD ClipImage(RotatedRect rotatedRect)
        {
            if (rotatedRect.Angle == 0)
                return ClipImage(rotatedRect.ToRectangle());

            Image2D clipImage = new Image2D((int)rotatedRect.Width, (int)rotatedRect.Height, NumBand);
            double radian = MathHelper.DegToRad(rotatedRect.Angle);
            PointF centerFloatPt = DrawingHelper.CenterPoint(rotatedRect);
            centerFloatPt.X = centerFloatPt.X - rotatedRect.X;
            centerFloatPt.Y = centerFloatPt.Y - rotatedRect.Y;

            if (IsUseIntPtr())
            {
                this.ConvertFromPtr();
            }

            Point centerPt = Point.Round(centerFloatPt);
            //for (int i = 0; i < rotatedRect.Height; ++i)

            ///////////////////////////////////////////////
            // 이건 바이리니어, 밑에건 보간 없음
            ////////////////////////////////////////////
            Parallel.For(0, clipImage.Height, i =>
            {
                double fDistance = 0;
                double fPolarAngle = 0;

                double fTrueX = 0;
                double fTrueY = 0;

                int iFloorX = 0;
                int iFloorY = 0;
                int iCeilingX = 0;
                int iCeilingY = 0;

                double fDeltaX = 0;
                double fDeltaY = 0;

                double clrTopLeft = 0;
                double clrTopRight = 0;
                double clrBottomLeft = 0;
                double clrBottomRight = 0;

                double fTop = 0;
                double fBottom = 0;

                byte iValue = 0;

                for (int j = 0; j < clipImage.Width; ++j)
                {
                    int x = j - centerPt.X;
                    int y = centerPt.Y - (int)i;

                    fDistance = Math.Sqrt(x * x + y * y);
                    fPolarAngle = 0.0;
                    if (x == 0)
                    {
                        if (y == 0)
                        {
                            for (int k = 0; k < NumBand; k++)
                            {
                                clipImage.Data[i * clipImage.Pitch + j * NumBand + k] = Data[(i + (int)Math.Round(rotatedRect.Y)) * Pitch + (j + (int)Math.Round(rotatedRect.X)) * NumBand + k];
                            }
                            continue;
                        }
                        else if (y < 0)
                        {
                            fPolarAngle = 1.5 * Math.PI;
                        }
                        else
                        {
                            fPolarAngle = 0.5 * Math.PI;
                        }
                    }
                    else
                    {
                        fPolarAngle = Math.Atan2((double)y, (double)x);
                    }

                    fPolarAngle += radian;

                    fTrueX = fDistance * Math.Cos(fPolarAngle);
                    fTrueY = fDistance * Math.Sin(fPolarAngle);

                    fTrueX = fTrueX + (double)centerPt.X;
                    fTrueY = (double)centerPt.Y - fTrueY;

                    iFloorX = (int)(Math.Floor(fTrueX));
                    iFloorY = (int)(Math.Floor(fTrueY));
                    iCeilingX = (int)(Math.Ceiling(fTrueX));
                    iCeilingY = (int)(Math.Ceiling(fTrueY));

                    if (iFloorX < 0 || iCeilingX < 0 || iFloorX >= rotatedRect.Width || iCeilingX >= rotatedRect.Width || iFloorY < 0 || iCeilingY < 0 || iFloorY >= rotatedRect.Height || iCeilingY >= rotatedRect.Height) continue;

                    fDeltaX = fTrueX - (double)iFloorX;
                    fDeltaY = fTrueY - (double)iFloorY;
                    for (int k = 0; k < NumBand; k++)
                    {
                        clrTopLeft = Data[(iFloorY + (int)Math.Round(rotatedRect.Y)) * Pitch + (iFloorX + (int)Math.Round(rotatedRect.X)) * NumBand + k];//bm.GetPixel(iFloorX, iFloorY);
                        clrTopRight = Data[(iFloorY + (int)Math.Round(rotatedRect.Y)) * Pitch + (iCeilingX + (int)Math.Round(rotatedRect.X)) * NumBand + k];//bm.GetPixel(iCeilingX, iFloorY);
                        clrBottomLeft = Data[(iCeilingY + (int)Math.Round(rotatedRect.Y)) * Pitch + (iFloorX + (int)Math.Round(rotatedRect.X)) * NumBand + k];//bm.GetPixel(iFloorX, iCeilingY);
                        clrBottomRight = Data[(iCeilingY + (int)Math.Round(rotatedRect.Y)) * Pitch + (iCeilingX + (int)Math.Round(rotatedRect.X)) * NumBand + k];//bm.GetPixel(iCeilingX, iCeilingY);

                        fTop = (1 - fDeltaX) * clrTopLeft + fDeltaX * clrTopRight;
                        fBottom = (1 - fDeltaX) * clrBottomLeft + fDeltaX * clrBottomRight;

                        iValue = (byte)(Math.Round((1 - fDeltaY) * fTop + fDeltaY * fBottom));

                        if (iValue < 0)
                            iValue = 0;
                        if (iValue > 255)
                            iValue = 255;

                        clipImage.Data[i * clipImage.Pitch + j * NumBand + k] = iValue;
                    }
                }
            });

            /*double radian = MathHelper.DegToRad(rotatedRect.Angle);

            double sinValue = Math.Sin(radian);
            double cosValue = Math.Cos(radian);
            int srcX = 0, srcY = 0;
            Image2D clipImage = new Image2D((int)rotatedRect.Width, (int)rotatedRect.Height, NumBand);
            Rectangle rect = Rectangle.Round(rotatedRect.ToRectangleF());
            PointF centerPt = DrawingHelper.CenterPoint(rect);
            if (double.IsNaN(sinValue))
                sinValue = 0;

            if (double.IsNaN(cosValue))
                cosValue = 0;

            for (int y = 0; y < (int)rect.Height; y++)
            {
                for (int x = 0; x < (int)rect.Width; x++)
                {
                    srcX = (int)((y + rect.Y - centerPt.Y) * sinValue + (x + rect.X - centerPt.X) * cosValue + centerPt.X);
                    srcY = (int)((y + rect.Y - centerPt.Y) * cosValue + (x + rect.X - centerPt.X) * (-sinValue) + centerPt.Y);

                    if (x < 0 || srcX < 0 || x > rect.Width - 1 || srcX > Width - 1 || y < 0 || srcY < 0 || y > rect.Height - 1 || srcY > Height - 1)
                        continue;

                    for (int i = 0; i < NumBand; i++)
                    {
                        clipImage.Data[y * clipImage.Pitch + x * NumBand + i] = Data[srcY * Pitch + srcX * NumBand + i];
                    }
                        
                }
            }*/

            return clipImage;
        }

        public override ImageD ClipImage(Rectangle rectangle)
        {
            Image2D image2d = new Image2D(rectangle.Width, rectangle.Height, NumBand);

            imageData.Clip(image2d.imageData, rectangle);

            return image2d;
        }

        public Image2D GetGrayImage()
        {
            if (NumBand == 1)
                return (Image2D)Clone();

            Image2D grayImage = new Image2D(Width, Height, 1);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    float rValue = (float)Data[y * Pitch + x * NumBand] / 255;
                    float gValue = (float)Data[y * Pitch + x * NumBand + 1] / 255;
                    float bValue = (float)Data[y * Pitch + x * NumBand + 2] / 255;

                    grayImage.Data[y * Width + x] = (byte)((0.299 * rValue + 0.587 * gValue + 0.114 * bValue) * 255);
                }
            }

            return grayImage;
        }

        public ImageD GetColorImage()
        {
            if (NumBand == 3)
                return (Image2D)Clone();

            Image2D colorImage = new Image2D(Width, Height, 3);

            int colorImagePitch = colorImage.Pitch;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    colorImage.Data[y * colorImagePitch + x * 3] = Data[y * Pitch + x];
                    colorImage.Data[y * colorImagePitch + x * 3 + 1] = Data[y * Pitch + x];
                    colorImage.Data[y * colorImagePitch + x * 3 + 2] = Data[y * Pitch + x];
                }
            }

            return colorImage;
        }

        public override float GetAverage()
        {
            return Data.Average(x => (float)x);
        }

        public override float GetMax()
        {
            return Data.Max();
        }

        public override float GetMin()
        {
            return Data.Min();
        }

        public override ImageD Not()
        {
            Bitmap tempImage = ToBitmap();
            ImageHelper.Not(tempImage);

            return Image2D.FromBitmap(tempImage);
        }

        public override float[] GetRangeValue(Point point, int range = 5)
        {
            return imageData.GetRangeValue(point, range);
        }

        float GetValue(PointF point)
        {
            int xStep = 0, yStep = 0;
            Point ptLT = new Point((int)point.X, (int)point.Y);
            if (point.X < Width) xStep = 1;
            if (point.Y < Height) yStep = 1;

            float ltValue = Data[ptLT.Y * Pitch + ptLT.X];
            float rtValue = Data[ptLT.Y * Pitch + ptLT.X + xStep];
            float lbValue = Data[(ptLT.Y + yStep) * Pitch + ptLT.X];
            float rbValue = Data[(ptLT.Y + yStep) * Pitch + ptLT.X + xStep];

            float topValue = ltValue + (rtValue - ltValue) * (point.X - (int)point.X);
            float bottomValue = lbValue + (rbValue - lbValue) * (point.X - (int)point.X);

            return topValue + (bottomValue - topValue) * (point.Y - (int)point.Y);
        }

        public override ImageD Resize(int destWidth, int destHeight)
        {
            Image2D resizeImage = new Image2D(destWidth, destHeight, NumBand);
            if (this.Size == resizeImage.Size)
                return this.Clone();

            for (int y = 0; y < destHeight; y++)
            {
                for (int x = 0; x < destWidth; x++)
                {
                    PointF point = new PointF((float)x / destWidth * Width, (float)y / destHeight * Height);
                    float value = GetValue(point);
                    resizeImage.Data[y * resizeImage.Pitch + x] = (byte)value;
                }
            }

            return resizeImage;
        }

        public override ImageD FlipX()
        {
            Image2D flipImage = new Image2D(Width, Height, NumBand);


            unsafe
            {
                int _width = Width;
                int _offset = 0;
                int _width_1 = _width - 1;

                fixed (byte* dst = &(flipImage.Data[0]))
                {
                    fixed (byte* src = &(Data[0]))
                    {
                        for (int y = 0; y < Height; y++)
                        {
                            _offset = y * _width;
                            for (int x = 0; x < Width; x++)
                            {
                                dst[_offset + _width_1 - x] = src[_offset + x];
                            }
                        }
                    }
                }
            }

            //int _width = Width;
            //int _offset = 0;
            //int _width_1 = _width - 1;
            //for (int y = 0; y < Height; y++)
            //{
            //    _offset = y * _width;
            //    for (int x = 0; x < Width; x++)
            //    {
            //        flipImage.Data[_offset + _width_1 - x] = Data[_offset + x];
            //    }
            //}
            return flipImage;
        }

        public override ImageD FlipY()
        {
            Image2D flipImage = new Image2D(Width, Height, NumBand);

            Parallel.For(0, Height, y =>
            {
                for (int x = 0; x < Width; x++)
                {
                    flipImage.Data[y * Width + ((Width - 1) - x)] = Data[y * Width + x];
                }
            });

            return flipImage;
        }

        public override void RotateFlip(RotateFlipType rotateFlipType)
        {
            Image2D flipImage = null;
            switch (rotateFlipType)
            {
                case RotateFlipType.RotateNoneFlipX:
                    flipImage = (Image2D)FlipX();
                    break;
                case RotateFlipType.RotateNoneFlipY:
                    flipImage = (Image2D)FlipY();
                    break;
            }

            imageData = flipImage.imageData;
        }

        public override void Mul(float mul)
        {
            for (int i = 0; i < this.imageData.Data.Length; i++)
                this.imageData.Data[i] = (byte)(this.imageData.Data[i] * mul);
        }
    }

    public class Image3D : ImageD
    {
        ImageBase<float> imageData = null;

        public ImageBase<float> ImageData { get { return imageData; } }
        public override int Width { get { return imageData.Width; } }
        public override int Height { get { return imageData.Height; } }
        public override int Pitch { get { return imageData.Pitch; } }
        public override int NumBand { get { return imageData.NumBand; } }
        public override int DataSize { get { return imageData.DataSize; } }

        RectangleF mappingRect;
        public RectangleF MappingRect
        {
            get { return mappingRect; }
            set { mappingRect = value; }
        }

        Point3d[] pointArray;
        public Point3d[] PointArray
        {
            get { return pointArray; }
            set { pointArray = value; }
        }

        public float[] Data { get { return imageData.Data; } }
        public IntPtr DataPtr { get { return imageData.DataPtr; } }

        public override void ConvertFromPtr(int srcPitch = 0)
        {
            if (imageData.DataPtr != IntPtr.Zero)
            {
                float[] data = new float[imageData.Pitch * imageData.Height];
                Marshal.Copy(imageData.DataPtr, data, 0, imageData.Pitch * imageData.Height);

                imageData.SetData(data);
            }
        }

        public Image3D()
        {

        }

        public Image3D(string fileName)
        {
            Load(fileName);
        }

        public Image3D(int width, int height)
        {
            Initialize(width, height, 1);
        }

        public override void Initialize(int width, int height, int numBand, int pitch = 0)
        {
            imageData = new ImageBase<float>();
            imageData.Initialize(width, height, numBand, pitch);
        }

        public override void FreeImageData()
        {
            imageData.Dispose();
            imageData = null;
        }

        public override ImageD Clone()
        {
            Image3D cloneImage = new Image3D();
            cloneImage.Initialize(Width, Height, NumBand, Pitch);
            cloneImage.CopyFrom(this);

            return cloneImage;
        }

        public override void CopyFrom(ImageD imageSrc)
        {
            Debug.Assert(imageSrc is Image3D);
            Debug.Assert(IsSame(imageSrc));

            Image3D imageSrc3d = (Image3D)imageSrc;

            imageData.Copy(imageSrc3d.Data);
            if (imageSrc3d.PointArray != null)
                pointArray = (Point3d[])imageSrc3d.PointArray.Clone();

            mappingRect = imageSrc3d.MappingRect;
        }

        public override void CopyFrom(ImageD imageSrc, Rectangle srcRect, int srcPitch, Point destPt)
        {
            Debug.Assert(imageSrc is Image3D);
            Debug.Assert(IsSame(imageSrc));

            Image3D imageSrc3d = (Image3D)imageSrc;

            imageData.Copy(imageSrc3d.Data);
            if (imageSrc3d.PointArray != null)
                pointArray = (Point3d[])imageSrc3d.PointArray.Clone();

            mappingRect = imageSrc3d.MappingRect;
        }

        public override void Clear()
        {
            imageData.Clear(0);
        }

        public override ImageD GetLayer(int index)
        {
            Image3D layerImage = new Image3D();
            layerImage.Initialize(Width, Height, 1);
            layerImage.SetData(imageData.GetLayer(index).Data);

            return layerImage;
        }

        public void SetData(float[] dataSrc)
        {
            imageData.Copy(dataSrc);
        }

        public void SetData(float[] dataSrc, float valueScale, float valueOffset)
        {
            Parallel.For(0, dataSrc.Count(), index =>
            {
                Data[index] = dataSrc[index] * valueScale + valueOffset;
            });
        }

        public void SetData(byte[] dataSrc)
        {
            int sizeByte = Pitch * Height * DataSize;

            Buffer.BlockCopy(dataSrc, 0, Data, 0, sizeByte);
        }

        public override Bitmap ToBitmap()
        {
            Debug.Assert(imageData != null);

            byte[] byteData = ImageHelper.ConvertByteBuffer(Data);

            return ImageHelper.CreateBitmap(Width, Height, Pitch, NumBand, byteData);
        }

        public Image2D ToImage2D()
        {
            Debug.Assert(imageData != null);

            byte[] byteData = ImageHelper.ConvertByteBuffer(Data);

            Image2D image2d = new Image2D(Width, Height, NumBand, Pitch);
            image2d.SetData(byteData);

            return image2d;
        }

        public override void SaveImage(string fileName, ImageFormat imageFormat)
        {
            Debug.Assert(imageData != null);

            Bitmap bitmap = ToBitmap();
            ImageHelper.SaveImage(bitmap, fileName, imageFormat);
            bitmap.Dispose();
        }

        public override void LoadImage(string fileName)
        {
            Bitmap bitmap = new Bitmap(fileName);

            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;

            byte[] byteImageData = ImageHelper.GetByte(bitmap);
            float[] floatImageData = new float[byteImageData.Count()];

            byteImageData.CopyTo(floatImageData, 0);

            imageData = new ImageBase<float>();
            imageData.Initialize(bitmap.Width, bitmap.Height, bytesPerPixel, bitmap.Width * bytesPerPixel, floatImageData);

            bitmap.Dispose();
        }

        public override void Save(string fileName)
        {
            imageData.Save(fileName);
        }

        public override void Load(string fileName)
        {
            if (imageData == null)
                imageData = new ImageBase<float>();
            imageData.Load(fileName);
        }

        public void SaveRaw(string fileName)
        {
            imageData.SaveRaw(fileName);
        }

        public void LoadRaw(string fileName)
        {
            if (File.Exists(fileName) == false)
                return;

            FileStream fileStream = new FileStream(fileName, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(fileStream);

            int width = binaryReader.ReadInt32();
            int height = binaryReader.ReadInt32();
            int numBand = binaryReader.ReadInt32();

            imageData = new ImageBase<float>();
            imageData.Initialize(width, height, numBand);

            int size = width * height * numBand * imageData.DataSize;
            byte[] byteBuffer = binaryReader.ReadBytes(size);

            Buffer.BlockCopy(byteBuffer, 0, Data, 0, size);
        }

        public override ImageD ClipImage(RotatedRect rotatedRect)
        {
            Bitmap tempImage = ToBitmap();
            Bitmap clipImage = ImageHelper.ClipImage(tempImage, rotatedRect);

            return Image2D.FromBitmap(clipImage);
        }

        public override ImageD ClipImage(Rectangle rectangle)
        {
            Image3D image3d = new Image3D(rectangle.Width, rectangle.Height);

            for (int y = rectangle.Top; y < rectangle.Bottom; y++)
            {
                int srcIndex = y * Pitch + rectangle.Left;
                int destIndex = (y - rectangle.Top) * rectangle.Width;
                Array.Copy(Data, srcIndex, image3d.Data, destIndex, rectangle.Width);
            }

            return image3d;
        }

        public override float GetAverage()
        {
            return Data.Average();
        }

        public override float GetMax()
        {
            return Data.Max();
        }

        public override float GetMin()
        {
            return Data.Min();
        }

        public override ImageD Not()
        {
            Bitmap tempImage = ToBitmap();
            ImageHelper.Not(tempImage);

            return Image2D.FromBitmap(tempImage);
        }

        public override float[] GetRangeValue(Point point, int range = 5)
        {
            return imageData.GetRangeValue(point, range);
        }

        float GetValue(PointF point)
        {
            int xStep = 0, yStep = 0;
            Point ptLT = new Point((int)point.X, (int)point.Y);
            if (point.X < (Width - 1)) xStep = 1;
            if (point.Y < (Height - 1)) yStep = 1;

            float ltValue = Data[ptLT.Y * Width + ptLT.X];
            float rtValue = Data[ptLT.Y * Width + ptLT.X + xStep];
            float lbValue = Data[(ptLT.Y + yStep) * Width + ptLT.X];
            float rbValue = Data[(ptLT.Y + yStep) * Width + ptLT.X + xStep];

            float topValue = ltValue + (rtValue - ltValue) * (point.X - (int)point.X);
            float bottomValue = lbValue + (rbValue - lbValue) * (point.X - (int)point.X);

            return topValue + (bottomValue - topValue) * (point.Y - (int)point.Y);
        }

        public override ImageD FlipX()
        {
            Image3D flipImage = new Image3D(Width, Height);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    flipImage.Data[((Height - 1) - y) * Width + x] = Data[y * Width + x];
                }
            }

            return flipImage;
        }

        public override ImageD FlipY()
        {
            Image3D flipImage = new Image3D(Width, Height);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    flipImage.Data[y * Width + ((Width - 1) - x)] = Data[y * Width + x];
                }
            }

            return flipImage;
        }

        public override void RotateFlip(RotateFlipType rotateFlipType)
        {
            Image3D flipImage = null;
            switch (rotateFlipType)
            {
                case RotateFlipType.RotateNoneFlipX:
                    flipImage = (Image3D)FlipX();
                    break;
                case RotateFlipType.RotateNoneFlipY:
                    flipImage = (Image3D)FlipY();
                    break;
            }

            imageData = flipImage.ImageData;
        }

        public override ImageD Resize(int destWidth, int destHeight)
        {
            Image3D resizeImage = new Image3D(destWidth, destHeight);

            for (int y = 0; y < destHeight; y++)
            {
                for (int x = 0; x < destWidth; x++)
                {
                    PointF point = new PointF((float)x / destWidth * Width, (float)y / destHeight * Height);
                    resizeImage.Data[y * destWidth + x] = GetValue(point);
                }
            }

            return resizeImage;
        }

        public static Image3D Average(Image3D image3d1, Image3D image3d2)
        {
            Image3D averageImage = new Image3D(image3d1.Width, image3d1.Height);

            for (int i = 0; i < image3d1.Width * image3d1.Height; i++)
            {
                averageImage.Data[i] = (image3d1.Data[i] + image3d2.Data[i]) / 2;
            }

            return averageImage;
        }

        public override void ConvertFromData()
        {
            throw new NotImplementedException();
        }

        public override void Mul(float mul)
        {
            throw new NotImplementedException();
        }
    }
}
