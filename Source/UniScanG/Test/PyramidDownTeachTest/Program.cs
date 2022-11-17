using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PyramidDownTeachTest
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo fileInfo = new FileInfo(@"D:\UniScan\Gravure_Inspector_Sapera\VirtualImage\201012_CL05X226MR6NUWB\Image_C00_S000_L00.bmp");
            if (args.Length > 1)
                fileInfo = new FileInfo(args[1]);

            if (!fileInfo.Exists)
            {
                Console.WriteLine("There is no file.");
                Console.WriteLine(fileInfo.FullName);
                Console.ReadKey();
                return;
            }


            // Load
            Image<Gray, byte> image = LoadImage(fileInfo);

            // Generate Map
            Image<Gray, float> reduce = new Image<Gray, float>(image.Width, 1);
            CvInvoke.Reduce(image, reduce, Emgu.CV.CvEnum.ReduceDimension.SingleRow, Emgu.CV.CvEnum.ReduceType.ReduceAvg, Emgu.CV.CvEnum.DepthType.Cv32F);
            byte[] bytes = reduce.Bytes;
            float[] floats = new float[image.Width];
            for (int i = 0; i < image.Width; i++)
                floats[i] = BitConverter.ToSingle(bytes, i * sizeof(float));
            TTTTTT(floats);
        }

        private static void TTTTTT(float[] datas)
        {
            SaveText(@"C:\temp\datas.txt", datas, 0);

            float overallAver = datas.Average();
            int startAt = -1;
            for (int i = 1; i < datas.Length; i++)
            {
                if (datas[i] < overallAver && datas[i - 1] > datas[i])
                {
                    startAt = i;
                    break;
                }
            }

            float[] validDatas = datas.Skip(startAt).ToArray();
            SaveText(@"C:\temp\validDatas.txt", validDatas, datas.Length);

            float validAver = validDatas.Average();
            int rising = 0;
            validDatas.Aggregate((f, g) =>
            {
                if (f < g && f < validAver && g >= validAver)
                    rising++;
                return g;
            });

            float[] averDatas = new float[validDatas.Length];
            float averAver = 0;
            int averCnt = 0;
            int averSrc = 0;
            for (int i = 0; i < averDatas.Length; i++)
            {
                averDatas[i] = validDatas.Skip(averSrc).Take(i - averSrc + 1).Min();
                averAver = ((averAver * averCnt) + averDatas[i]) / (averCnt + 1);
                averCnt++;
                if (averCnt > 100 && Math.Abs(averDatas[i] - averAver)>5)
                {
                    averDatas[i] = 255;
                    averAver = 0;
                    averCnt = 0;
                    averSrc = i;
                }
            }
            SaveText(@"C:\temp\averDatas.txt", averDatas, datas.Length);

            //int kernelSize = validDatas.Length / rising/2;
            //int kernelSize2 = kernelSize / 2;
            //float[] convDatas = new float[validDatas.Length];
            //for (int i = 0; i < convDatas.Length; i++)
            //{
            //    float[] fs = validDatas.Skip(i).Take(kernelSize).ToArray();
            //    //float upper = fs.Take(kernelSize2).Min();
            //    //float lower = fs.Skip(kernelSize2).Min();
            //    //convDatas[i] = (upper>lower)?upper:
            //    convDatas[i] = fs.Min();
            //}
            //SaveText(@"C:\temp\convDatas.txt", convDatas, datas.Length);

            //float convAver = convDatas.Max() - convDatas.Min();
            //float[] convBin = Array.ConvertAll(convDatas, f => f > convAver ? 255f : 0f);
            //SaveText(@"C:\temp\convBin.txt", convBin, datas.Length);

        }

        private static void SaveText(string v, float[] data, int minLength)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < minLength-data.Length ; i++)
                sb.AppendLine("0");

            for (int i = 0; i < data.Length; i++)
                sb.AppendLine(data[i].ToString());

            File.WriteAllText(v, sb.ToString());
        }
        private static void SaveTextY(string v, float[,,] data, int dimY)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.GetLength(1); i++)
                sb.AppendLine(data[dimY, i, 0].ToString());

            File.WriteAllText(v, sb.ToString());
        }

        private static void SaveText(string v, float[,,] data)
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < data.GetLength(0); y++)
            {
                for (int x = 0; x < data.GetLength(1); x++)
                {
                    if (x > 0)
                        sb.Append(", ");
                    sb.Append(data[y, x, 0]);
                }
                sb.AppendLine();
            }

            File.WriteAllText(v, sb.ToString());
        }

        private static Image<Gray, byte> LoadImage(FileInfo fileInfo)
        {
            long headerSize = 54;
            byte[] headerData = new byte[headerSize];
            byte[] imageData = null;
            Image<Gray, byte> image = null;

            using (StreamReader sr = new StreamReader(fileInfo.FullName))
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
