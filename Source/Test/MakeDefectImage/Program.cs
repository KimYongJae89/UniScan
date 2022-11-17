using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeDefectImage
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] workingPath = new string[]
            {
                //@"D:\그라비아 퀄\가상이미지\1608_440-10BME5DM-GL08IC\1608불량이미지",
                //@"D:\그라비아 퀄\가상이미지\2012_440-21BME502-GL03IC\2012불량이미지",
                @"D:\그라비아 퀄\가상이미지\3216_440-31BMJE503-GL03SC\3216불량이미지",
                //@"D:\그라비아 퀄\가상이미지\3225_440-32BMJE502-GL08SC\3225불량이미지"
            };

            for (int i = 0; i < workingPath.Length; i++)
                Work(workingPath[i]);
        }

        private static void Work(string path)
        {
            for (int i = 0; i < 2; i++)
                Work(path, i);
        }

        private static void Work(string path, int cam)
        {
            Console.WriteLine(string.Format("Verify - Camera {0}, Reference {1}", cam, path));

            string refImage = Path.Combine(path, "..", string.Format("Image_C{0:00}_S{1:000}_L{2:00}.bmp", cam, 0, 0));
            if (File.Exists(refImage) == false)
            {
                Console.WriteLine(string.Format("Err:: ReferenceImage is not exist"));
                return;
            }

            string defImage = Path.Combine(path, string.Format("C{0:00}.bmp", cam));
            if (File.Exists(defImage) == false)
            {
                Console.WriteLine(string.Format("Err:: DefectImage is not exist"));
                return;
            }

            string roiPath = Path.Combine(path, string.Format("C{0:00}", cam));
            if (Directory.Exists(roiPath) == false)
            {
                Console.WriteLine(string.Format("Err:: Roi Path not exist"));
                return;
            }

            Rectangle[] pinholeRect = new Rectangle[5];
            Rectangle[] noprintRect = new Rectangle[5];
            Rectangle[] shetAtkRect = new Rectangle[5];
            Console.WriteLine(string.Format("Roi Load - {0}", roiPath));
            LoadRoi(roiPath, pinholeRect, noprintRect, shetAtkRect);

            bool emptyExist = Array.Exists(pinholeRect, f => f.IsEmpty)|| Array.Exists(noprintRect, f => f.IsEmpty)|| Array.Exists(shetAtkRect, f => f.IsEmpty);
            if (emptyExist)
            {
                Console.WriteLine(string.Format("Err:: Some Roi is Empty"));
                return;
            }

            Work(refImage, defImage, path,cam, pinholeRect, noprintRect, shetAtkRect);
        }

        private static void Work(string refImage, string defImage, string savePath,int camera, Rectangle[] pinholeRect, Rectangle[] noprintRect, Rectangle[] shetAtkRect)
        {
            Console.WriteLine(string.Format("Image Load - {0}", refImage));
            ImageD refImageD = new Image2D(refImage);
            AlgoImage refAlgoImage = ImageBuilder.Build(ImagingLibrary.OpenCv, refImageD, ImageType.Grey);

            Console.WriteLine(string.Format("Image Load - {0}", defImage));
            ImageD defImageD = new Image2D(defImage);
            AlgoImage defAlgoImage = ImageBuilder.Build(ImagingLibrary.OpenCv, defImageD, ImageType.Grey);

            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(refAlgoImage);

            for (int i = 0; i < 5; i++)
            {
                AlgoImage algoImage = refAlgoImage.Clone();

                Console.WriteLine("Image Copy - Pinhole");
                algoImage.Copy(defAlgoImage, pinholeRect[i].Location, pinholeRect[i].Location, pinholeRect[i].Size);

                Console.WriteLine("Image Copy - Noprint");
                algoImage.Copy(defAlgoImage, noprintRect[i].Location, noprintRect[i].Location, noprintRect[i].Size);

                Console.WriteLine("Image Copy - Sheet attack");
                algoImage.Copy(defAlgoImage, shetAtkRect[i].Location, shetAtkRect[i].Location, shetAtkRect[i].Size);

                string resImage = Path.Combine(savePath, string.Format("Image_C{0:00}_S{1:000}_L{2:00}.bmp", camera, 0, i));
                Console.WriteLine(string.Format("Image Save - {0}", resImage));
                algoImage.Save(resImage);

                // daraw thumnail
                AlgoImage colorImage = DynMvp.Vision.ImageConverter.Convert(algoImage, ImagingLibrary.OpenCv, ImageType.Color);
                int lineWidth = 30;
                // draw pinhole
                DrawRect(colorImage, pinholeRect[i], lineWidth);
                DrawRect(colorImage, noprintRect[i], lineWidth);
                DrawRect(colorImage, shetAtkRect[i], lineWidth);
                ip.Resize(colorImage, colorImage, 0.1, 0.1);
                string thumnailImage = Path.Combine(savePath, string.Format("Image_C{0:00}_S{1:000}_L{2:00}.jpg", camera, 0, i));
                colorImage.Save(thumnailImage);
                colorImage.Dispose();

                algoImage.Dispose();
            }
        }

        private static void DrawRect(AlgoImage algoImage, Rectangle rectangle, int lineWidth)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

            ip.DrawRect(algoImage, Rectangle.FromLTRB(rectangle.Left - lineWidth, rectangle.Top - lineWidth, rectangle.Left, rectangle.Bottom + lineWidth), Color.Red.ToArgb(), true);
            ip.DrawRect(algoImage, Rectangle.FromLTRB(rectangle.Left - lineWidth, rectangle.Top - lineWidth, rectangle.Right + lineWidth, rectangle.Top), Color.Red.ToArgb(), true);
            ip.DrawRect(algoImage, Rectangle.FromLTRB(rectangle.Right, rectangle.Top - lineWidth, rectangle.Right + lineWidth, rectangle.Bottom + lineWidth), Color.Red.ToArgb(), true);
            ip.DrawRect(algoImage, Rectangle.FromLTRB(rectangle.Left - lineWidth, rectangle.Bottom, rectangle.Right + lineWidth, rectangle.Bottom + lineWidth), Color.Red.ToArgb(), true);
        }

        private static void LoadRoi(string roiPath, Rectangle[] pinholeRect, Rectangle[] noprintRect, Rectangle[] shetAtkRect)
        {
            string[] files = Directory.GetFiles(roiPath, "*.roi");
            foreach(string file in files)
            {
                string filename = Path.GetFileNameWithoutExtension(file);
                if (filename.Length == 3 && filename.Substring(0, 2) == "SA")
                    LoadRoi(file, int.Parse(filename.Substring(2, 1)), shetAtkRect);
                else if (filename.Length == 4 && filename.Substring(0, 3) == "PIN")
                    LoadRoi(file, int.Parse(filename.Substring(3, 1)), pinholeRect);
                else if (filename.Length == 4 && filename.Substring(0, 3) == "NOP")
                    LoadRoi(file, int.Parse(filename.Substring(3, 1)), noprintRect);
            }
        }

        private static void LoadRoi(string file, int index, Rectangle[] rectangleList)
        {
            FileStream fs = File.Open(file, FileMode.Open);
            int len = (int)fs.Length;
            byte[] data = new byte[len];
            fs.Read(data, 0, len);
            fs.Close();

            short t = (short)(data[8] << 8 | data[9]);
            short l = (short)(data[10] << 8 | data[11]);
            short b = (short)(data[12] << 8 | data[13]);
            short r = (short)(data[14] << 8 | data[15]);

            Rectangle rectangle = Rectangle.FromLTRB(l, t, r, b);
            rectangleList[index-1] = rectangle;
        }
    }
}
