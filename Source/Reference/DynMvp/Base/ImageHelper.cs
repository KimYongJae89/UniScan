using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;

using DynMvp.UI;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Concurrent;

namespace DynMvp.Base
{
    public class ImageHelper
    {
        private class SaveQueueData
        {
            public string FilePath => filePath;
            string filePath;
            public Image Image => this.image;
            Image image;

            public SaveQueueData(string filePath, Image image)
            {
                this.filePath = filePath;
                this.image = image;
            }
        }

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        public static void SetGrayColorPallet(Bitmap srcBitmap, Bitmap dstBitmap)
        {
            ColorPalette src = srcBitmap.Palette;
            ColorPalette dst = dstBitmap.Palette;
            if (src.Entries.Length != dst.Entries.Length)
                return;

            Buffer.BlockCopy(src.Entries, 0, dst.Entries, 0, src.Entries.Length);
            dstBitmap.Palette = dst;
        }

        public static void SetGrayColorPallet(Bitmap bitmap)
        {
            ColorPalette cp = bitmap.Palette;
            if (cp.Entries.Length < 1)
                return;

            float scale = 255f / (cp.Entries.Length - 1);
            for (int i = 0; i < cp.Entries.Length; i++)
            {
                int value = (int)Math.Round(i * scale);
                cp.Entries[i] = Color.FromArgb(value, value, value);
            }
            bitmap.Palette = cp;
        }

        public static string BitmapToBase64String(Bitmap image)
        {
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Bmp);

            memoryStream.Position = 0;
            byte[] imageByte = memoryStream.ToArray();

            string base64String = Convert.ToBase64String(imageByte);

            imageByte = null;

            return base64String;
        }

        public static string ImageToBase64String(Image image, ImageFormat imageFormat)
        {
            if (imageFormat == null)
                imageFormat = ImageFormat.Bmp;

            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, imageFormat);

            memoryStream.Position = 0;
            byte[] imageByte = memoryStream.ToArray();

            string base64String = Convert.ToBase64String(imageByte);

            imageByte = null;

            return base64String;
        }

        public static Bitmap Base64StringToBitmap(string base64string)
        {
            if (string.IsNullOrEmpty(base64string))
                return null;

            byte[] imageByte = Convert.FromBase64String(base64string);

            MemoryStream memoryStream = new MemoryStream(imageByte);
            memoryStream.Position = 0;

            Bitmap bitmap = new Bitmap(memoryStream);

            return bitmap;
        }

        public static Image Base64StringToImage(string base64string, ImageFormat imageFormat)
        {
            if (base64string == "")
                return null;

            byte[] imageByte = Convert.FromBase64String(base64string);

            MemoryStream memoryStream = new MemoryStream(imageByte);
            memoryStream.Position = 0;

            Image image = Image.FromStream(memoryStream);

            return image;
        }

        public static Image LoadImage(string fileName)
        {
            try
            {
                if (File.Exists(fileName) == false)
                    return null;

                if (false/*Path.GetExtension(fileName) == ".bmp"*/)
                {
                    ImageD imageD = new Image2D();
                    imageD.LoadImage(fileName);
                    Bitmap bitmap = imageD.ToBitmap();
                    imageD.Dispose();
                    return bitmap;
                }
                else
                {
                    try
                    {
                        Image image = null;
                        using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            //FileStream fs = new FileStream(fileName, FileMode.Open);
                            image = Bitmap.FromStream(fs);
                            fs.Close();
                        }
                        //Image image = Image.FromFile(fileName);
                        return image;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Error(LoggerType.Error, string.Format("ImageHelper::LoadImage - {0},{1}", fileName, ex.Message));
                        return null;
                    }
                }
            }
#if DEBUG==false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("ImageHelper::LoadImage - {0}", ex.Message));
            }
#endif
            finally { }

            return null;
        }

        public static void SaveImage(Image image, string fileName)
        {
            ImageFormat imageFormat = ImageFormat.Bmp;

            string extension = Path.GetExtension(fileName).ToLower();
            if (extension == ".jpg" || extension == ".jpeg")
                imageFormat = ImageFormat.Jpeg;
            else if (extension == ".png")
                imageFormat = ImageFormat.Png;

            SaveImage(image, fileName, imageFormat);
        }

        public static void SaveImage(Image image, string fileName, ImageFormat imageFormat)
        {
            if (image == null)
                return;

            //LogHelper.Debug(LoggerType.Debug, string.Format("ImageHelper::SaveImage - {0}", fileName));
            try
            {
                byte[] imageByte = null;
                lock (image)
                {
                    //if (image is Bitmap)
                    //{
                    //    Bitmap bitmap = (Bitmap)image;
                    //    BitmapData bitmapData = bitmap.LockBits(new Rectangle(Point.Empty, image.Size), ImageLockMode.ReadOnly, bitmap.PixelFormat);

                    //    int fihSize = 14;
                    //    int bihSize = 40;
                    //    int dataSize = bitmapData.Stride * bitmapData.Height;
                    //    int fileSize = fihSize + bihSize + dataSize;
                    //    short bpp = (short)((bitmap.PixelFormat == PixelFormat.Format8bppIndexed) ? 8 : 24);

                    //    List<byte> byteList = new List<byte>();
                    //    byteList.AddRange(new byte[2] { (byte)'B', (byte)'M' }); // Magin Code
                    //    byteList.AddRange(BitConverter.GetBytes(fileSize)); // FileSize
                    //    byteList.AddRange(new byte[2]); // Reserved
                    //    byteList.AddRange(new byte[2]); // Reserved
                    //    byteList.AddRange(BitConverter.GetBytes(fihSize + bihSize)); // Data offset

                    //    byteList.AddRange(BitConverter.GetBytes(bihSize)); // biSize
                    //    byteList.AddRange(BitConverter.GetBytes(bitmap.Width)); // biWidth
                    //    byteList.AddRange(BitConverter.GetBytes(bitmap.Height)); // biHeight
                    //    byteList.AddRange(BitConverter.GetBytes((short)1)); // biPlanes
                    //    byteList.AddRange(BitConverter.GetBytes(bpp)); // biBitCount
                    //    byteList.AddRange(BitConverter.GetBytes((int)0)); // biCompression
                    //    byteList.AddRange(BitConverter.GetBytes(fileSize)); // biSizeImage
                    //    byteList.AddRange(BitConverter.GetBytes(bitmap.HorizontalResolution)); // biXPelsPerMeter
                    //    byteList.AddRange(BitConverter.GetBytes(bitmap.VerticalResolution)); // biYPelsPerMeter
                    //    byteList.AddRange(BitConverter.GetBytes(0)); // biClrUsed
                    //    byteList.AddRange(BitConverter.GetBytes(bpp == 8 ? 256 : 0)); // biClrImportant
                    //    if(bpp==8)
                    //    {
                    //        foreach(Color color in  bitmap.Palette.Entries)
                    //            byteList.AddRange(BitConverter.GetBytes(color.ToArgb()));
                    //    }

                    //    byte[] imageData = new byte[bitmapData.Stride * bitmapData.Height];
                    //    Marshal.Copy(bitmapData.Scan0, imageData, 0, imageData.Length);
                    //    bitmap.UnlockBits(bitmapData);

                    //    byteList.AddRange(imageData);

                    //    imageByte = byteList.ToArray();
                    //}
                    //else
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        image?.Save(memoryStream, imageFormat);
                        memoryStream.Position = 0;

                        imageByte = memoryStream.ToArray();
                        memoryStream.Close();
                        memoryStream.Dispose();
                    }
                }

                string dirName = Path.GetDirectoryName(fileName);
                if (Directory.Exists(dirName) == false)
                    Directory.CreateDirectory(dirName);

                using (Stream stream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
                {
                    stream.Write(imageByte, 0, imageByte.Length);
                    stream.Flush();
                    stream.Close();
                }

                //LogHelper.Debug(LoggerType.Debug, "ImageHelper::SaveImage End");
            }
            //#if DEBUG == false
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("ImageHelper::SaveImage - {0} {1}", ex.ToString(), ex.Message));
            }
            //#endif
            finally { }
        }

        public static void SaveImage(float[] floatData, int width, int height, string fileName)
        {
            int i, j, j0;
            byte[] byteData = new byte[width * height];

            //   float minValue = float.MaxValue;
            //float mavValue = float.MinValue;

            //for(j=0; j< height; j++)
            //{
            // j0 = j* width;
            // for(i=0; i< width; i++)
            // {
            //           minValue = Math.Min(floatData[j0 + i], minValue);
            //           mavValue = Math.Max(floatData[j0 + i], mavValue);
            // }
            //   }

            float minValue = floatData.Min();
            float maxValue = floatData.Max();

            float diffValue = Math.Abs(maxValue - minValue);

            for (j = 0; j < height; j++)
            {
                j0 = j * width;
                for (i = 0; i < width; i++)
                {
                    byteData[j0 + i] = (byte)(Math.Abs(floatData[j0 + i] - minValue) / diffValue * 255);
                }
            }

            SaveImage(byteData, width, height, fileName);
        }

        public static void SaveImage(byte[] byteData, int width, int height, string fileName)
        {
            int stride = width;
            if ((width % 4) != 0)
                stride = width + (4 - width % 4);//4바이트 배수가 아닐시..

            IntPtr imageBuffer = Marshal.AllocHGlobal(byteData.Length);
            Marshal.Copy(byteData, 0, imageBuffer, byteData.Length);

            Bitmap bitmap = new Bitmap(width, height, stride, PixelFormat.Format8bppIndexed, imageBuffer);

            SetGrayColorPallet(bitmap);
            bitmap.Save(fileName);

            bitmap.Dispose();
            Marshal.FreeHGlobal(imageBuffer);
        }

        public static Bitmap CopyResize(Bitmap srcBitmap, Rectangle srcRect, float scale)
        {
            Size dstSize = DrawingHelper.Mul(srcRect.Size, scale);
            Bitmap dstBitmap = new Bitmap(dstSize.Width, dstSize.Height, srcBitmap.PixelFormat);
            SetGrayColorPallet(dstBitmap);

            BitmapData srcBmpData = null;
            BitmapData dstBmpData = null;
            try
            {
                srcBmpData = srcBitmap.LockBits(srcRect, ImageLockMode.ReadWrite, dstBitmap.PixelFormat);
                //srcBmpData = srcBitmap.LockBits(new Rectangle(Point.Empty, srcBitmap.Size), ImageLockMode.ReadWrite, dstBitmap.PixelFormat);
                dstBmpData = dstBitmap.LockBits(new Rectangle(Point.Empty, dstSize), ImageLockMode.ReadWrite, dstBitmap.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(dstBitmap.PixelFormat) / 8;

                unsafe
                {
                    byte* srcPtr = (byte*)srcBmpData.Scan0;
                    byte* dstPtr = (byte*)dstBmpData.Scan0;
                    for (int y = 0; y < dstSize.Height; y++)
                    {
                        for (int x = 0; x < dstSize.Width; x++)
                        {
                            int srcOffset = ((int)(x / scale) * bytesPerPixel) + (((int)(y / scale)) * srcBmpData.Stride);
                            int dstOffset = (x * bytesPerPixel) + (y * dstBmpData.Stride);
                            for (int i = 0; i < bytesPerPixel; i++)
                                dstPtr[dstOffset + i] = srcPtr[srcOffset + i];
                        }
                    }
                }
                return dstBitmap;
            }
            finally
            {
                srcBitmap.UnlockBits(srcBmpData);
                dstBitmap.UnlockBits(dstBmpData);
            }
        }

        public static Bitmap Resize(Bitmap original, float scaleX, float scaleY)
        {
            //Debug.Assert(scaleX <= 0 || scaleY <= 0, "Destination is invalid");

            Bitmap resizeImage = new Bitmap((int)(original.Width * scaleX), (int)(original.Height * scaleY), PixelFormat.Format8bppIndexed);
            Rectangle resizeImageRect = new Rectangle(0, 0, resizeImage.Width, resizeImage.Height);
            BitmapData resizeBmpData = resizeImage.LockBits(resizeImageRect, ImageLockMode.ReadWrite, resizeImage.PixelFormat);
            IntPtr resizeImagePtr = resizeBmpData.Scan0;

            int resizeImageSize = resizeBmpData.Stride * resizeBmpData.Height;

            Rectangle originalImageRect = new Rectangle(0, 0, original.Width, original.Height);
            BitmapData originalBmpData = original.LockBits(originalImageRect, ImageLockMode.ReadWrite, resizeImage.PixelFormat);
            IntPtr originalImagePtr = originalBmpData.Scan0;

            int originalImageSize = originalBmpData.Stride * originalBmpData.Height;

            float inverseScaleX = 1.0f / scaleX;
            float inverseScaleY = 1.0f / scaleY;

            if (false)
            {
                byte[] resizeImageBuffer = new byte[resizeImageSize];
                byte[] originalImageBuffer = new byte[originalImageSize];

                System.Runtime.InteropServices.Marshal.Copy(resizeImagePtr, resizeImageBuffer, 0, resizeImageSize);
                System.Runtime.InteropServices.Marshal.Copy(originalImagePtr, originalImageBuffer, 0, originalImageSize);

                for (int i = 0; i < resizeImage.Width; i++)
                {
                    for (int j = 0; j < resizeImage.Height; j++)
                    {
                        resizeImageBuffer[i + j * resizeBmpData.Stride] =
                            originalImageBuffer[(int)(i * inverseScaleX) + (int)(j * inverseScaleY) * originalBmpData.Stride];
                    }
                }
                System.Runtime.InteropServices.Marshal.Copy(resizeImageBuffer, 0, resizeImagePtr, resizeImageSize);
            }
            else
            {
                unsafe
                {
                    byte* dstPtr = (byte*)resizeImagePtr;
                    byte* srcPtr = (byte*)originalImagePtr;
                    for (int i = 0; i < resizeImage.Width; i++)
                    {
                        for (int j = 0; j < resizeImage.Height; j++)
                        {
                            int srcIdx = (int)(i * inverseScaleX) + (int)(j * inverseScaleY) * originalBmpData.Stride;
                            int dstIdx = i + j * resizeBmpData.Stride;
                            dstPtr[dstIdx] = srcPtr[srcIdx];
                        }
                    }
                }
            }

            resizeImage.UnlockBits(resizeBmpData);
            original.UnlockBits(resizeBmpData);

            SetGrayColorPallet(resizeImage);
            
            return resizeImage;
        }

        public static Bitmap ClipImage(Bitmap bitmap, RectangleF clipRect)
        {
            return ClipImage(bitmap, new Rectangle((int)clipRect.X, (int)clipRect.Y, (int)clipRect.Width, (int)clipRect.Height));
        }

        public static Bitmap ClipImage(Bitmap bitmap, Rectangle clipRect)
        {
            Rectangle bmpRect = new Rectangle(new Point(0, 0), bitmap.Size);
            clipRect = Rectangle.Intersect(bmpRect, clipRect);
            if (bmpRect.Contains(clipRect) == false)
                return null;

            Bitmap cloneBitmap = bitmap.Clone(clipRect, bitmap.PixelFormat);

            return cloneBitmap;
        }

        public static Bitmap ClipImage(Bitmap bitmap, RotatedRect clipRect)
        {
            Rectangle boundRect = DrawingHelper.ToRect(clipRect.GetBoundRect());
            //            boundRect.Inflate(1, 1);

            Bitmap boundImage = ClipImage(bitmap, boundRect);

#if DEBUG
            if (string.IsNullOrEmpty(Configuration.TempFolder) == false)
                ImageHelper.SaveImage(boundImage, Path.Combine(Configuration.TempFolder, "BoundImage.bmp"));
#endif

            if (clipRect.Angle != 0)
            {
                Bitmap rotatedBitmap = new Bitmap((int)boundRect.Width, (int)boundRect.Height);
                using (Graphics g = Graphics.FromImage(rotatedBitmap))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    //Transformation matrix
                    Matrix m = new Matrix();
                    m.RotateAt((float)clipRect.Angle, new PointF(boundImage.Width / 2.0f, boundImage.Height / 2.0f));

                    g.Transform = m;
                    g.DrawImage(boundImage, 0, 0);

                    g.Dispose();
                }

#if DEBUG
                if (string.IsNullOrEmpty(Configuration.TempFolder) == false)
                    ImageHelper.SaveImage(rotatedBitmap, Path.Combine(Configuration.TempFolder, "RotatedImage.bmp"));
#endif
                Rectangle angle0Rect = new Rectangle((int)(clipRect.X - boundRect.X), (int)(clipRect.Y - boundRect.Y), (int)clipRect.Width, (int)clipRect.Height);
                Bitmap clipImage = ClipImage(rotatedBitmap, angle0Rect);

#if DEBUG
                if (string.IsNullOrEmpty(Configuration.TempFolder) == false)
                    ImageHelper.SaveImage(clipImage, Path.Combine(Configuration.TempFolder, "ClipImage.bmp"));
#endif
                rotatedBitmap.Dispose();

                return clipImage;

            }
            else
            {
                return boundImage;
            }
        }

        public static Bitmap CloneImage(Bitmap bitmap)
        {
            Rectangle bmpRect = new Rectangle(new Point(0, 0), bitmap.Size);
            return bitmap.Clone(bmpRect, bitmap.PixelFormat);
        }

        public static Image CloneImage(Image image)
        {
            return (Image)image.Clone();
        }

        public static void Copy(Bitmap srcImage, Bitmap destImage, Point destPt)
        {
            if (destPt.X < 0 || (destPt.X + srcImage.Width) > destImage.Width ||
                    destPt.Y < 0 || (destPt.Y + srcImage.Height) > destImage.Height)
            {
                Debug.Assert(false, "Destination is invalid");
                return;
            }

            lock (srcImage)
            {
                lock (destImage)
                {
                    unsafe
                    {
                        Rectangle srcRect = new Rectangle(0, 0, srcImage.Width, srcImage.Height);
                        Rectangle destRect = new Rectangle(0, 0, destImage.Width, destImage.Height);

                        BitmapData srcBmpData = srcImage.LockBits(srcRect, System.Drawing.Imaging.ImageLockMode.ReadWrite, srcImage.PixelFormat);
                        BitmapData destBmpData = destImage.LockBits(destRect, System.Drawing.Imaging.ImageLockMode.ReadWrite, destImage.PixelFormat);

                        IntPtr srcPtr = srcBmpData.Scan0;
                        IntPtr destPtr = destBmpData.Scan0;

                        if (destImage.PixelFormat == srcImage.PixelFormat)
                        {
                            int srcPixelSize = 1;
                            if (srcImage.PixelFormat == PixelFormat.Format32bppRgb)
                            {
                                srcPixelSize = 4;
                            }
                            else if (srcImage.PixelFormat == PixelFormat.Format24bppRgb)
                            {
                                srcPixelSize = 3;
                            }

                            if ((srcImage.Width == destImage.Width) && (srcImage.Height == destImage.Height) && (destPt.X == 0 && destPt.Y == 0) && (srcBmpData.Stride == destBmpData.Stride))
                            {
                                LogHelper.Debug(LoggerType.Operation, "Imagehelper - Copy - 6");

                                CopyMemory(destPtr, srcPtr, (uint)(srcBmpData.Width * srcBmpData.Height * srcPixelSize));
                            }
                            else
                            {
                                for (int y = 0; y < srcImage.Height; y++)
                                {
                                    CopyMemory(destPtr + destPt.X * srcPixelSize + (destPt.Y + y) * destBmpData.Stride, srcPtr + y * srcBmpData.Stride, (uint)(srcBmpData.Width * srcPixelSize));
                                }
                            }
                        }
                        else
                        {
                            int srcImageSize = srcBmpData.Stride * srcBmpData.Height;
                            int destImageSize = destBmpData.Stride * destBmpData.Height;

                            byte[] srcImageBuffer = new byte[srcImageSize];
                            byte[] destImageBuffer = new byte[destImageSize];

                            Marshal.Copy(srcPtr, srcImageBuffer, 0, srcImageSize);
                            Marshal.Copy(destPtr, destImageBuffer, 0, destImageSize);

                            int srcPixelSize = 1;
                            int pixelStep1 = 1;
                            int pixelStep2 = 2;
                            int pixelStep3 = 3;
                            if (srcImage.PixelFormat == PixelFormat.Format32bppRgb)
                            {
                                srcPixelSize = 4;
                            }
                            else if (srcImage.PixelFormat == PixelFormat.Format24bppRgb)
                            {
                                srcPixelSize = 3;
                            }
                            else  // 8bit
                            {
                                pixelStep1 = 0;
                                pixelStep2 = 0;
                                pixelStep3 = 0;
                            }

                            Parallel.For(0, srcImage.Height, y =>
                            {
                                for (int x = 0; x < srcImage.Width; x++)
                                {
                                    if (destImage.PixelFormat == PixelFormat.Format32bppRgb)
                                    {
                                        destImageBuffer[(destPt.X + x) * 4 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + (y * srcBmpData.Stride)];
                                        destImageBuffer[(destPt.X + x) * 4 + 1 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + pixelStep1 + (y * srcBmpData.Stride)];
                                        destImageBuffer[(destPt.X + x) * 4 + 2 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + pixelStep2 + (y * srcBmpData.Stride)];
                                    }
                                    else if (destImage.PixelFormat == PixelFormat.Format32bppArgb)
                                    {
                                        destImageBuffer[(destPt.X + x) * 4 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + (y * srcBmpData.Stride)];
                                        destImageBuffer[(destPt.X + x) * 4 + 1 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + pixelStep1 + (y * srcBmpData.Stride)];
                                        destImageBuffer[(destPt.X + x) * 4 + 2 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + pixelStep2 + (y * srcBmpData.Stride)];
                                        destImageBuffer[(destPt.X + x) * 4 + 3 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + pixelStep3 + (y * srcBmpData.Stride)];
                                    }
                                    else if (destImage.PixelFormat == PixelFormat.Format24bppRgb)
                                    {
                                        destImageBuffer[(destPt.X + x) * 3 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + (y * srcBmpData.Stride)];
                                        destImageBuffer[(destPt.X + x) * 3 + 1 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + pixelStep1 + (y * srcBmpData.Stride)];
                                        destImageBuffer[(destPt.X + x) * 3 + 2 + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + pixelStep2 + (y * srcBmpData.Stride)];
                                    }
                                    else
                                    {
                                        destImageBuffer[(destPt.X + x) + (destPt.Y + y) * destBmpData.Stride] = srcImageBuffer[x * srcPixelSize + (y * srcBmpData.Stride)];
                                    }
                                }
                            });

                            Marshal.Copy(srcImageBuffer, 0, srcPtr, srcImageSize);
                            Marshal.Copy(destImageBuffer, 0, destPtr, destImageSize);
                        }

                        srcImage.UnlockBits(srcBmpData);

                        destImage.UnlockBits(destBmpData);

                        if (destImage.PixelFormat == PixelFormat.Format8bppIndexed)
                        {
                            SetGrayColorPallet(destImage);
                        }
                    }
                }
            }
        }

        public static void Add(Bitmap srcImage, int value)
        {
            Rectangle srcRect = new Rectangle(0, 0, srcImage.Width, srcImage.Height);

            BitmapData srcBmpData = srcImage.LockBits(srcRect, ImageLockMode.ReadWrite, srcImage.PixelFormat);

            IntPtr srcPtr = srcBmpData.Scan0;

            int srcImageSize = srcBmpData.Stride * srcBmpData.Height;

            byte[] srcImageBuffer = new byte[srcImageSize];

            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcImageBuffer, 0, srcImageSize);

            for (int index = 0; index < srcImage.Height; index++)
            {
                srcImageBuffer[index] = (byte)(srcImageBuffer[index] + value);
            }

            System.Runtime.InteropServices.Marshal.Copy(srcImageBuffer, 0, srcPtr, srcImageSize);

            srcImage.UnlockBits(srcBmpData);

            ColorPalette colorPalette = srcImage.Palette;
            for (int i = 0; i < 256; i++)
            {
                colorPalette.Entries[i] = Color.FromArgb(i, i, i);
            }
            srcImage.Palette = colorPalette;
        }

        public static Bitmap MakeColor24(Bitmap original)
        {
            Bitmap newBitmap = new Bitmap(original.Width, original.Height, PixelFormat.Format24bppRgb);

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    //get the pixel from the original image
                    Color originalColor = original.GetPixel(i, j);

                    ////create the color object
                    //Color newColor = Color.FromArgb(originalColor, originalColor, originalColor);

                    ////set the new image's pixel to the grayscale version
                    newBitmap.SetPixel(i, j, originalColor);
                }
            }

            return newBitmap;
        }

        public static Bitmap MakeGrayscale(Bitmap original)
        {
            Rectangle rect = new Rectangle(Point.Empty, original.Size);
            Bitmap greyImage = new Bitmap(rect.Width, rect.Height, PixelFormat.Format8bppIndexed);

            BitmapData greyData = greyImage.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            BitmapData originalData = original.LockBits(rect, ImageLockMode.ReadOnly, original.PixelFormat);

            int length = greyData.Stride * greyData.Height;
            int originlength = originalData.Stride * originalData.Height;

            //Parallel.For(0, length, i =>
            //{
            //    CopyMemory(greyData.Scan0 + i, originalData.Scan0 + (i * 3), 1);
            //});

            byte[] greyBuffer = new byte[length];
            //Marshal.Copy(greyData.Scan0, greyBuffer, 0, length);

            byte[] originalBuffer = new byte[originlength];
            Marshal.Copy(originalData.Scan0, originalBuffer, 0, originlength);

            for (int y = 0; y < originalData.Height; y++)
            {
                int originalIndex = y * originalData.Stride;
                int greyIndex = y * greyData.Stride;
                for (int x = 2; x < originalData.Stride; x += 3)
                {
                    greyBuffer[greyIndex + (x / 3)] = (byte)((originalBuffer[originalIndex + x - 2] + originalBuffer[originalIndex + x - 1] + originalBuffer[originalIndex + x]) / 3);
                }
            }

            Marshal.Copy(greyBuffer, 0, greyData.Scan0, length);

            original.UnlockBits(originalData);
            greyImage.UnlockBits(greyData);

            SetGrayColorPallet(greyImage);

            return greyImage;
        }

        public static ImageD GetRotateMask(int width, int height, RotatedRect rotatedRect)
        {
            RectangleF boundRect = rotatedRect.GetBoundRect();
            rotatedRect.Offset(-boundRect.X, -boundRect.Y);

            RectangleFigure rectangleFigure = new RectangleFigure(rotatedRect, new Pen(Color.White), new SolidBrush(Color.White));
            Bitmap rotatedMask = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(rotatedMask);
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, width, height));
            rectangleFigure.Draw(g, new CoordTransformer(), true);
            g.Dispose();

            Bitmap grayImage = ImageHelper.MakeGrayscale(rotatedMask);

            rotatedMask.Dispose();

            return Image2D.FromBitmap(grayImage);
        }

        public static byte[] GetByte(Bitmap bitmap)
        {
            Rectangle bitmapRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            BitmapData bmpData = bitmap.LockBits(bitmapRect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bitmap.PixelFormat);

            int imageSize = bmpData.Stride * bmpData.Height;
            byte[] imageData = new byte[imageSize];

            Parallel.For(0, bmpData.Height, y =>
            {
                Marshal.Copy(bmpData.Scan0 + y * bmpData.Stride, imageData, y * bmpData.Stride, bmpData.Stride);
            });

            bitmap.UnlockBits(bmpData);

            return imageData;
        }

        public static byte[] GetGrayByte(Bitmap bitmap)
        {
            int bpp = System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat);
            int srcStep = bpp / 8;
            int srcPitch = (((bitmap.Width * bpp) + 31) / 32) * 4;
            int grayPitch = (((bitmap.Width * 8) + 31) / 32) * 4;
            byte[] grayBytes = new byte[grayPitch * bitmap.Height];

            int heigth2 = bitmap.Height / 2;
            Point[] locs = new Point[] { new Point(0, 0), new Point(0, heigth2) };
            Size[] sizes = new Size[] { new Size(bitmap.Width, heigth2), new Size(bitmap.Width, bitmap.Height - heigth2) };
            for (int i = 0; i < sizes.Length; i++)
            {
                Point loc = locs[i];
                Size size = sizes[i];
                using (Bitmap clone = new Bitmap(size.Width, size.Height, bitmap.PixelFormat))
                {
                    using (Graphics gr = Graphics.FromImage(clone))
                    {
                        gr.TranslateTransform(-loc.X, -loc.Y);
                        gr.DrawImageUnscaled(bitmap, 0, 0);
                    }
                    byte[] lineBytes = GetByte(clone);
                    for (int y = 0; y < size.Height; y++)
                    {
                        int srcOffset = srcPitch * y;
                        int grayOffset = grayPitch * (y + loc.Y);
                        for (int x = 0; x < size.Width; x++)
                            grayBytes[grayOffset + x] = lineBytes[srcOffset + (x * srcStep)];
                    }
                }
            }
            return grayBytes;

            //int bpp = System.Drawing.Bitmap.GetPixelFormatSize(bitmap.PixelFormat);
            //int step = bpp / 8;

            //int srcPitch = (((bitmap.Width * bpp) + 31) / 32) * 4;
            //int dstPitch = (((bitmap.Width * 8) + 31) / 32) * 4;
            //int heigth = bitmap.Height;

            //byte[] bytes = new byte[dstPitch * heigth];

            //byte[] line = new byte[srcPitch];
            //using (MemoryStream ms = new MemoryStream())
            //{        
            //    bitmap.Save(ms, ImageFormat.Png);
            //    byte[] bytess = ms.ToArray();


            //    for (int y = 0; y < heigth; y++)
            //    {
            //        int srcOffset = srcPitch * y;
            //        ms.Seek(-(srcOffset + srcPitch), SeekOrigin.End);
            //        ms.Read(line, 0, srcPitch);

            //        int dstOffset = dstPitch * y;
            //        for (int x = 0; x < dstPitch; x++)
            //            bytes[dstOffset + x] = line[x * step];
            //    }
            //}
            //return bytes;
        }

        public static Bitmap CreateBitmap(int width, int height, int pitch, int numBand, byte[] imageData)
        {
            Debug.Assert(imageData != null);
            //Stopwatch sw = Stopwatch.StartNew();

            if (pitch == 0)
            {
                pitch = width * numBand;
                if ((pitch % 4) != 0)
                    pitch = pitch + (4 - pitch % 4);//4바이트 배수가 아닐시..
            }

            //byte[] destImageData = new byte[stride * height];

            Bitmap bmpImage;
            System.Drawing.Imaging.BitmapData bmpData;

            if (numBand == 3)
            {
                //pitch = width * numBand;
                bmpImage = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                bmpData = bmpImage.LockBits(new Rectangle(0, 0, bmpImage.Width, bmpImage.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                //pitch = bmpData.Stride;
            }
            else if (numBand == 4)
            {
                bmpImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                bmpData = bmpImage.LockBits(new Rectangle(0, 0, bmpImage.Width, bmpImage.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            }

            else
            {
                bmpImage = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                SetGrayColorPallet(bmpImage);

                bmpData = bmpImage.LockBits(new Rectangle(0, 0, bmpImage.Width, bmpImage.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
            }
            //Debug.WriteLine(string.Format("ImageHelper::CreateBitmap Alloc and Lock {0}", sw.Elapsed.TotalMilliseconds));

            //Parallel.For(0, height, y =>
            //{
            //    try
            //    {
            //        int offsetSrc = pitch * y;
            //        int offsetDst = bmpData.Stride * y;
            //        Marshal.Copy(imageData, offsetSrc, IntPtr.Add(bmpData.Scan0, offsetDst), pitch);
            //    }
            //    catch (/*ArgumentOutOfRangeException*/Exception ex)
            //    {
            //        LogHelper.Error(LoggerType.Error, string.Format("ImageHelper::CreateBitmap {0}",ex.GetType().ToString(), ex.Message));
            //    }
            //    //Buffer.BlockCopy(imageData, pitch * y, destImageData, stride * y, pitch);
            //    //Array.Copy(imageData, pitch * y, destImageData, stride * y, pitch);
            //});

            try //수정된 코드
            {
                if (bmpData.Stride == pitch)
                {
                    int size = bmpData.Stride * height;
                    Marshal.Copy(imageData, 0, bmpData.Scan0, size);
                }
                else
                {
                    int copyLength = Math.Min(bmpData.Stride, pitch);

                    //Parallel.For(0, height, y =>
                    //{
                    //    int offsetSrc = pitch * y;
                    //    int offsetDst = bmpData.Stride * y;
                    //    Marshal.Copy(imageData, offsetSrc, IntPtr.Add(bmpData.Scan0, offsetDst), copyLength);
                    //});

                    for (int y = 0; y < height; y++)
                    {
                        int offsetSrc = pitch * y;
                        int offsetDst = bmpData.Stride * y;
                        Marshal.Copy(imageData, offsetSrc, IntPtr.Add(bmpData.Scan0, offsetDst), copyLength);
                    }
                    //Debug.WriteLine(string.Format("ImageHelper::CreateBitmap Copy {0}", sw.Elapsed.TotalMilliseconds));
                }
            }
            catch (/*ArgumentOutOfRangeException*/Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("ImageHelper::CreateBitmap {0}", ex.GetType().ToString(), ex.Message));
            }

            //System.Runtime.InteropServices.Marshal.Copy(destImageData, 0, bmpData.Scan0, destImageData.Length);

            bmpImage.UnlockBits(bmpData);
            //Array.Resize(ref destImageData, 0);
            //bmpImage.Save(@"d:\temp\bbb.bmp");
            return bmpImage;
        }

        public static Bitmap CreateBitmap(int width, int height, int pitch, int numBand, IntPtr dataPtr)
        {
            Debug.Assert(dataPtr != null);

            int stride = width * numBand;
            if ((stride % 4) != 0)
                stride = stride + (4 - stride % 4);//4바이트 배수가 아닐시..

            Bitmap bmpImage;

            if (numBand == 3)
            {
                bmpImage = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format24bppRgb, dataPtr);
            }
            else
            {
                if (pitch == stride)
                    bmpImage = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format8bppIndexed, dataPtr);
                else
                {
                    bmpImage = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                    BitmapData bitmapData = bmpImage.LockBits(new Rectangle(0, 0, bmpImage.Width, bmpImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);

                    int length = Math.Min(pitch, bitmapData.Stride);
                    //Parallel.For(0, bmpImage.Height, i =>
                    for (int i = 0; i < bmpImage.Height; i++)
                    {
                        int srcIndex = i * pitch;
                        int dstIndex = i * bitmapData.Stride;
                        CopyMemory(dataPtr + srcIndex, bitmapData.Scan0 + dstIndex, (uint)length);
                    }
                    //);
                    bmpImage.UnlockBits(bitmapData);
                }

                System.Drawing.Imaging.ColorPalette cp = bmpImage.Palette;
                for (int i = 0; i < 256; i++)
                    cp.Entries[i] = Color.FromArgb(i, i, i);
                bmpImage.Palette = cp;
            }

            return bmpImage;
        }

        public static byte[] ConvertByteBuffer(float[] floatData)
        {
            byte[] byteData = new byte[floatData.Count()];

            float fmin = floatData.Min();
            float fmax = floatData.Max();
            float fdiff = Math.Abs(fmax - fmin);

            if (fdiff > 0)
            {
                Parallel.For(0, floatData.Count(), index =>
                {
                    byteData[index] = Convert.ToByte(Math.Abs(floatData[index] - fmin) / fdiff * 255);
                });
            }
            else
            {
                Array.Clear(byteData, 0, byteData.Length);
            }

            return byteData;
        }

        public static void Clear(Bitmap srcImage, byte value)
        {
            if (srcImage == null)
                return;

            Rectangle srcRect = new Rectangle(0, 0, srcImage.Width, srcImage.Height);

            BitmapData srcBmpData = srcImage.LockBits(srcRect, ImageLockMode.ReadWrite, srcImage.PixelFormat);

            IntPtr srcPtr = srcBmpData.Scan0;

            int srcImageSize = srcBmpData.Stride * srcBmpData.Height;

            byte[] srcImageBuffer = new byte[srcImageSize];
            Array.Clear(srcImageBuffer, 0, srcImageBuffer.Length);
            //System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcImageBuffer, 0, srcImageSize);

            if (value != 0)
            {
                for (int yIndex = 0; yIndex < srcImage.Height; yIndex++)
                {
                    for (int xIndex = 0; xIndex < srcImage.Width; xIndex++)
                    {
                        int index = yIndex * srcBmpData.Stride + xIndex;
                        srcImageBuffer[index] = value;
                    }
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(srcImageBuffer, 0, srcPtr, srcImageSize);

            srcImage.UnlockBits(srcBmpData);

            if (srcImage.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                ColorPalette colorPalette = srcImage.Palette;
                for (int i = 0; i < 256; i++)
                {
                    colorPalette.Entries[i] = Color.FromArgb(i, i, i);
                }
            }
        }

        public static void Not(Bitmap srcImage)
        {
            Rectangle srcRect = new Rectangle(0, 0, srcImage.Width, srcImage.Height);

            BitmapData srcBmpData = srcImage.LockBits(srcRect, ImageLockMode.ReadWrite, srcImage.PixelFormat);

            IntPtr srcPtr = srcBmpData.Scan0;

            int srcImageSize = srcBmpData.Stride * srcBmpData.Height;

            byte[] srcImageBuffer = new byte[srcImageSize];

            System.Runtime.InteropServices.Marshal.Copy(srcPtr, srcImageBuffer, 0, srcImageSize);

            for (int index = 0; index < srcImage.Height * srcImage.Width; index++)
            {
                srcImageBuffer[index] = (byte)(255 - srcImageBuffer[index]);
            }

            System.Runtime.InteropServices.Marshal.Copy(srcImageBuffer, 0, srcPtr, srcImageSize);

            srcImage.UnlockBits(srcBmpData);

            ColorPalette colorPalette = srcImage.Palette;
            for (int i = 0; i < 256; i++)
            {
                colorPalette.Entries[i] = Color.FromArgb(i, i, i);
            }
            srcImage.Palette = colorPalette;
        }

        public static Bitmap Capture(Rectangle captureRegion)
        {
            int bitsPerPixel = System.Windows.Forms.Screen.PrimaryScreen.BitsPerPixel;
            PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
            if (bitsPerPixel <= 16)
                pixelFormat = PixelFormat.Format16bppRgb565;
            else if (bitsPerPixel == 24)
                pixelFormat = PixelFormat.Format24bppRgb;

            Bitmap bmp = new Bitmap(captureRegion.Width, captureRegion.Height);
            using (Graphics gr = Graphics.FromImage(bmp))
                gr.CopyFromScreen(captureRegion.X, captureRegion.Y, 0, 0, captureRegion.Size);

            return bmp;
        }
    }
}
