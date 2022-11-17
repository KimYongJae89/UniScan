using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageConverter
{
    public class ConverterParam
    {
        public string TargetExt;
        public bool MakeThumbnail;
        public int ThumbnailSize;
        public bool DeleteOrigin;
    }

    public delegate void OnProgressChangedDelegate();
    class Converter
    {
        public static event OnProgressChangedDelegate OnProgressChanged;

        public static bool IsRunning { get => runningTask != null && !runningTask.IsCompleted; }

        static Task runningTask;

        internal static void Run(List<PathState> list, ConverterParam converterParam, CancellationTokenSource cancellationTokenSource)
        {
            runningTask = Task.Run(() =>
            {
                list.ForEach(f =>
                {
                    Run(f, converterParam, cancellationTokenSource.Token);
                });
            });
        }

        private static void Run(PathState f, ConverterParam converterParam, CancellationToken token)
        {
            if(token.IsCancellationRequested)
            {
                f.SetState(ConvertState.Pass);
                return;
            }

            f.SetState(ConvertState.Working);
            Image<Gray, byte> image = null;
            try
            {
                string ext = System.IO.Path.GetExtension(f.FullPath).ToLower();
                if((ext == converterParam.TargetExt) && !converterParam.MakeThumbnail)
                {
                    f.SetState(ConvertState.Pass);
                    return;
                }

                if (ext == ".bmp")
                    image = LoadBitmap(f.FullPath);
                else
                    image = new Image<Gray, byte>(f.FullPath);

                if (image == null)
                    throw new Exception("Read Fail.");

                if(converterParam.MakeThumbnail)
                {
                    string thumbnailPath = System.IO.Path.ChangeExtension(f.FullPath, ".jpg");

                    int longLen = Math.Max(image.Width, image.Height);
                    int shortLen = Math.Min(image.Width, image.Height);
                    // 장축을 2048[px]에 맞춘다.
                    double scale = converterParam.ThumbnailSize * 1.0 / longLen;

                    // 단축이 최소 256[px]이 되도록 scale 조절.
                    int newShortLen = (int)Math.Round(scale * shortLen);
                    if (newShortLen < 256)
                        scale = 256.0 / shortLen;

                    // 확대는 하지 않음.
                    scale = Math.Min(scale, 1);

                    Emgu.CV.Image<Gray, byte> resImage = image.Resize(scale, Emgu.CV.CvEnum.Inter.Linear);
                    resImage.Save(thumbnailPath);
                    resImage.Dispose();
                }

                string newPath = System.IO.Path.ChangeExtension(f.FullPath, converterParam.TargetExt);
                image.Save(newPath);
                if (converterParam.DeleteOrigin)
                    System.IO.File.Delete(f.FullPath);

                f.SetState(ConvertState.Done);
            }
            catch(Exception ex)
            {
                f.SetState(ConvertState.Error, $"{ex.GetType().Name}({ex.Message})");
            }
            finally
            {
                OnProgressChanged?.Invoke();
                image?.Dispose();
            }
        }

        private static Image<Gray, byte> LoadBitmap(string fullPath)
        {
            long headerSize = 54;
            byte[] headerData = new byte[headerSize];
            byte[] imageData = null;
            Image<Gray, byte> image = null;

            using (StreamReader sr = new StreamReader(fullPath))
            {
                sr.BaseStream.Read(headerData, 0, headerData.Length);
                if (headerData[0] == 'B' && headerData[1] == 'M')
                {
                    int width = headerData[21] << 24 | headerData[20] << 16 | headerData[19] << 8 | headerData[18];
                    int height = headerData[25] << 24 | headerData[24] << 16 | headerData[23] << 8 | headerData[22];
                    int bpp = headerData[29] << 8 | headerData[28];
                    int pitch = ((width * bpp + 31) / 32) * 4;

                    int dataStartIdx = 54 + (bpp == 8 ? 4 * 256 : 0);
                    long dataSize = sr.BaseStream.Length - dataStartIdx;
                    int imageSize = pitch * height;
                    imageData = new byte[imageSize];
                    sr.BaseStream.Position = dataStartIdx;
                    for (int h = 0; h < height; h++)
                    {
                        int offset = (height - h - 1) * pitch;
                        sr.BaseStream.Read(imageData, offset, pitch);
                    }

                    image = new Image<Gray, byte>(width, height);
                    image.Bytes = imageData;
                }
            }
            return image;
        }
    }
}
