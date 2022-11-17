using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Vision;
using UniScanG.Gravure.Vision.SheetFinder;

namespace BasePositionFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"D:\Work\주식회사 유니아이\P4 - 문서\25. MLCC 그라비아 전면검사기\이미지들\E21BMJH03-NOR-GA01SC";

            Process("IM0", path, "Image_C00_S???_L??.bmp");
            Process("IM1", path, "Image_C01_S???_L??.bmp");
        }

        private static void Process(string name, string path, string filter)
        {
            StreamWriter sw = File.AppendText($@"C:\temp\{name}.txt");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] fileInfos = directoryInfo.GetFiles(filter);
            foreach (FileInfo fileInfo in fileInfos)
            {
                Console.Write($"File Load: {fileInfo.Name}");
                Image2D image2D = new Image2D(fileInfo.FullName);

                using (AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.OpenCv, image2D, ImageType.Grey))
                {
                    Point basePt = AlgorithmCommon.FindOffsetPosition(algoImage, null, Point.Empty, null, null);
                    Console.WriteLine($"\tFounded Position: {basePt}");

                    sw.WriteLine($"{fileInfo.Name}, {basePt.X}, {basePt.Y}");
                    using (AlgoImage algoImageScaled = ImageBuilder.Build(ImagingLibrary.OpenCv, ImageType.Grey, DrawingHelper.Div(image2D.Size, 10)))
                    {
                        Point basePtScaled = DrawingHelper.Div(basePt, 10);
                        Rectangle baseRectScaled = DrawingHelper.FromCenterSize(basePtScaled, new Size(21, 21));

                        ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);
                        ip.Resize(algoImage, algoImageScaled);
                        using (AlgoImage colorImage = algoImageScaled.ConvertTo(ImageType.Color))
                        {
                            ip.DrawRect(colorImage, baseRectScaled, Color.Red.ToArgb(), false);
                            float[] verX = new float[] { (baseRectScaled.Left + baseRectScaled.Right) / 2, (baseRectScaled.Left + baseRectScaled.Right) / 2 };
                            float[] verY = new float[] { baseRectScaled.Bottom, colorImage.Height };
                            float[] horX = new float[] { baseRectScaled.Right, colorImage.Width };
                            float[] horY = new float[] { (baseRectScaled.Top + baseRectScaled.Bottom) / 2, (baseRectScaled.Top + baseRectScaled.Bottom) / 2 };
                            ip.DrawPolygon(colorImage, verX, verY, Color.Yellow.ToArgb(), false);
                            ip.DrawPolygon(colorImage, horX, horY, Color.Yellow.ToArgb(), false);
                            colorImage.Save($@"C:\temp\{name}\{fileInfo.Name}");
                        }
                    }
                }
            }
            sw.Flush();
            sw.Close();
        }
    }
}
