using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UniScanWPF.Helper
{
    public class WPFImageHelper : DynMvp.Base.ImageHelper
    {
        public static string BitmapSoruceToBase64String(BitmapSource bitmapSource)
        {
            byte[] bytes;
            BitmapEncoder bitmapEncoder = new BmpBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            using (var ms = new MemoryStream())
            {
                bitmapEncoder.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);
                bytes = ms.ToArray();
            }
            return Convert.ToBase64String(bytes);
        }

        public static BitmapSource Base64StringToBitmapSoruce(string base64String)
        {
            try
            {
                BitmapImage bitmapImage = null;
                byte[] bytes = Convert.FromBase64String(base64String);
                using (var ms = new MemoryStream())
                {
                    ms.Write(bytes, 0, bytes.Length);

                    bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = ms;
                    bitmapImage.EndInit();
                }
                bitmapImage?.Freeze();
                return bitmapImage;
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        public static PixelFormat ConvertPixelFormat(System.Drawing.Imaging.PixelFormat pixelFormat)
        {
            PixelFormat wpfPixelFormat = PixelFormats.Gray8;

            switch (pixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    wpfPixelFormat = PixelFormats.Gray8;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    wpfPixelFormat = PixelFormats.Rgb24;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                    wpfPixelFormat = PixelFormats.Bgr32;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                case System.Drawing.Imaging.PixelFormat.Format32bppPArgb:
                    wpfPixelFormat = PixelFormats.Bgra32;
                    break;
            }

            return wpfPixelFormat;
        }

        public static System.Drawing.Imaging.PixelFormat ConvertPixelFormat(PixelFormat pixelFormat)
        {
            if (pixelFormat == PixelFormats.Gray8)
                return System.Drawing.Imaging.PixelFormat.Format8bppIndexed;
            else if (pixelFormat == PixelFormats.Rgb24)
                return System.Drawing.Imaging.PixelFormat.Format24bppRgb;
            else if (pixelFormat == PixelFormats.Bgr32)
                return System.Drawing.Imaging.PixelFormat.Format32bppRgb;
            else if (pixelFormat == PixelFormats.Bgra32)
                return System.Drawing.Imaging.PixelFormat.Format32bppArgb;

            throw new NotImplementedException();
        }

        public static BitmapSource BitmapToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
            new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
               bitmapData.Width, bitmapData.Height, 96, 96, ConvertPixelFormat(bitmap.PixelFormat), null,
               bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);

            bitmapSource.Freeze();

            return bitmapSource;
        }

        public static System.Drawing.Bitmap BitmapSourceToBitmap(BitmapSource bitmapSource)
        {
            System.Drawing.Imaging.PixelFormat pixelFormat = ConvertPixelFormat(bitmapSource.Format);
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(bitmapSource.PixelWidth, bitmapSource.PixelHeight, pixelFormat);

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, pixelFormat);
            bitmapSource.CopyPixels(System.Windows.Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);
            bmp.UnlockBits(data);
            return bmp;
        }

        public static byte[] BitmapSourceToBytes(BitmapSource bitmapSource)
        {
            int length = bitmapSource.PixelWidth * bitmapSource.PixelHeight;
            byte[] bytes = new byte[bitmapSource.PixelWidth * bitmapSource.PixelHeight];
            bitmapSource.CopyPixels(bytes, bitmapSource.PixelWidth, 0);
            return bytes;
        }

        public static void SaveBitmapSource(string filePath, BitmapSource bitmapSource)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();

                string[] split = filePath.Split('.');
                switch (split.Last().ToString().ToUpper())
                {
                    case "JPG":
                        encoder = new JpegBitmapEncoder();
                        break;
                    case "PNG":
                        encoder = new PngBitmapEncoder();
                        break;
                }

                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(fileStream);
            }
        }

        public static BitmapSource LoadBitmapSource(string filePath)
        {
            if (File.Exists(filePath) == false)
                return null;

            try
            {
                BitmapImage bitmapImage = new BitmapImage();// = new BitmapImage(new Uri(filePath));
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmapImage.UriSource = new Uri(filePath);
                bitmapImage.EndInit();

                bitmapImage.Freeze();

                return bitmapImage;
            }
            catch (Exception ex)
            {
                LogHelper.Error(LoggerType.Error, string.Format("WPFImageHelper::{0}", ex.Message));
                return null;
            }
        }
    }
}
