using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ThumbnailGen
{
    class Program
    {
        static FileStream logWriter = new FileStream($"ThumbnailGen_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.log", FileMode.Create, FileAccess.Write);

        static void Main(string[] args)
        {
            ConsoleWriteLine(string.Join(" / ", args));
            FileInfo[] workFileInfos;
            if (args.Length > 0)
            {
                List<FileInfo> fileInfoList = new List<FileInfo>();
                Array.ForEach(args, f =>
                {
                    FileInfo fi = new FileInfo(f);
                    fileInfoList.AddRange(GetWorkFileInfos(fi));
                });
                workFileInfos = fileInfoList.ToArray();
            }
            else
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);
                ConsoleWriteLine(string.Format("Target Path: {0}", directoryInfo.FullName));
                workFileInfos = directoryInfo.GetFiles("*.bmp");
            }
            ConsoleWriteLine(string.Format("Found {0} Working Files.", workFileInfos.Length));
            Dictionary<FileInfo, string> failList = new Dictionary<FileInfo, string>();
            if (workFileInfos.Length == 0)
            {
                Console.WriteLine("There is no file to process.");
                Console.WriteLine("Press Any key to exit.");
                Console.ReadKey();
                return;
            }

            int commonPathIdx;
            if (workFileInfos.Length == 1)
                commonPathIdx = Path.GetDirectoryName(workFileInfos[0].FullName).Length + 1;
            else
                commonPathIdx = GetCommanPathIndex(Array.ConvertAll(workFileInfos, f => f.FullName)) + 1;
            //commonPathIdx = GetLCS(Array.ConvertAll(bitmapFileInfos, f => f.FullName));

            for (int i = 0; i < workFileInfos.Length; i++)
            {
                FileInfo bitmapFileInfo = workFileInfos[i];
                ConsoleWrite(string.Format("{0}/{1} [{2}] {3}MB: ", i + 1, workFileInfos.Length, bitmapFileInfo.FullName.Substring(commonPathIdx), bitmapFileInfo.Length / 1024 / 1024));

                string fromFile = bitmapFileInfo.FullName;
                string toFile = Path.ChangeExtension(bitmapFileInfo.FullName, "jpg");
                //string path = bitmapFileInfo.DirectoryName;
                //string fileName = Path.GetFileNameWithoutExtension(bitmapFileInfo.Name);
                //string toFile = Path.Combine(path, string.Format("{0}p.jpg", fileName));

                if (File.Exists(toFile))
                {
                    ConsoleWriteLine("Exist. ");
                }
                else
                {
                    try
                    {
                        ConsoleWrite(" Load...");
                        Emgu.CV.Image<Gray, byte> image = LoadImage(fromFile);
                        ConsoleWrite($"({image.Width} x {image.Height})");

                        ConsoleWrite(" Resize...");
                        int longLen = Math.Max(image.Width, image.Height);
                        int shortLen = Math.Min(image.Width, image.Height);
                        // 장축을 2048[px]에 맞춘다.
                        double scale = 2048.0 / longLen;

                        // 단축이 최소 512[px]이 되도록 scale 조절
                        int newShortLen = (int)Math.Round(scale * shortLen);
                        if (newShortLen < 512)
                            scale = 512.0 / shortLen;
                        scale = Math.Min(scale, 1);

                        Emgu.CV.Image<Gray, byte> resImage = image.Resize(scale, Emgu.CV.CvEnum.Inter.Linear);
                        ConsoleWrite($"({resImage.Width} x {resImage.Height}, {scale * 100:F1}%)");

                        ConsoleWrite(" Save... ");
                        resImage.Save(toFile);

                        resImage.Dispose();
                        image.Dispose();

                        ConsoleWriteLine("Done.");
                    }
                    catch (Exception ex)
                    {
                        failList.Add(bitmapFileInfo, ex.Message);
                        ConsoleWriteLine($"Fail. {ex.Message}");
                    }
                }
            }

            if (failList.Count > 0)
            {
                ConsoleWriteLine("Some file(s) was failed.");
                foreach(KeyValuePair<FileInfo, string> pair in failList)
                    ConsoleWriteLine($"{pair.Key.FullName}, {pair.Value}");
                Console.WriteLine("Press Any key to exit.");
                Console.ReadKey();
            }
            else
            {
                ConsoleWriteLine("Well Done.");
            }
        }

        private static void ConsoleWrite(string v)
        {
            Console.Write(v);
            LogWrite(v);
        }

        private static void ConsoleWriteLine(string v)
        {
            Console.WriteLine(v);
            LogWrite(v + Environment.NewLine);
        }

        private static void LogWrite(string v)
        {
            byte[] bytes = Encoding.Default.GetBytes(v);
            logWriter.Write(bytes, 0, bytes.Length);
            logWriter.Flush();
        }

        static FileInfo[] GetWorkFileInfos(FileInfo fi)
        {
            ConsoleWriteLine($"Search In: {fi.FullName}");

            List<FileInfo> fileInfoList = new List<FileInfo>();
            if (fi.Attributes.HasFlag(FileAttributes.Directory))
            {
                DirectoryInfo di = new DirectoryInfo(fi.FullName);
                fileInfoList.AddRange(di.GetFiles("*.bmp"));
                fileInfoList.AddRange(di.GetFiles("*.png"));

                DirectoryInfo[] subDirectortInfos = di.GetDirectories();
                Array.ForEach(subDirectortInfos, f =>
                {
                    FileInfo subFi = new FileInfo(f.FullName);
                    fileInfoList.AddRange(GetWorkFileInfos(subFi));
                });
            }
            else
            {
                string extension = fi.Extension.ToLower();
                if (extension == ".bmp" || extension == ".png")
                    fileInfoList.Add(fi);
            }
            return fileInfoList.ToArray();
        }

        private static int GetCommanPathIndex(string[] v)
        {
            List<string[]> list = v.Select(f => f.Split('\\')).ToList();
            int minPath = list.Min(f => f.Length);
            int idx = minPath;

            for (int i = 0; i < minPath; i++)
            {
                string[] s = list.Select(f => f[i]).ToArray();
                if (!Array.TrueForAll(s, f => f == s[0]))
                {
                    idx = i;
                    break;
                }
            }
            string[] ss = new string[idx];
            Array.Copy(list[0], 0, ss, 0, idx);
            return string.Join("\\", ss).Length;
        }

        private static int GetLCS(string[] v)
        {
            if (v.Length == 1)
                return v[0].Length;

            int idx = 0;
            int lim = v.Min(f => f.Length);
            bool good = false;
            do
            {
                char c = v[0][idx];
                good = Array.TrueForAll(v, f => f[idx] == c);
                if (good)
                    idx++;
            } while (good && (idx < lim));

            return idx;
        }

        private static Image<Gray, byte> LoadBmpImage(string fromFile)
        {
            long headerSize = 54;
            byte[] headerData = new byte[headerSize];
            byte[] imageData = null;
            Image<Gray, byte> image = null;

            using (StreamReader sr = new StreamReader(fromFile))
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

        private static Image<Gray, byte> LoadImage(string fromFile)
        {
            Image<Gray, byte> image = new Image<Gray, byte>(fromFile);
            return image;
        }
    }
}
