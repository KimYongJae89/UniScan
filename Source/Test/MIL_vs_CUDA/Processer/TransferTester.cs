using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MIL_vs_CUDA.Form1;

namespace MIL_vs_CUDA.Processer
{
    public class TransferTester
    {
        public SetProgressBarDelegate SetProgressBar = null;

        public long UploadTest(bool alloc, Image2D[] images)
        {
            Stopwatch sw = null;
            AlgoImage saveAlgoImage = null;
            if (alloc)
            {
                //saveAlgoImage = ImageBuilder.Build(ImagingLibrary.OpenCv, images[0], ImageType.Gpu);
                //saveAlgoImage.Save(@"C:\temp\UploadTest_Alloc.bmp");
                //saveAlgoImage.Dispose();
                //saveAlgoImage = null;

                sw = Stopwatch.StartNew();
                for (int i = 0; i < images.Length; i++)
                {
                    SetProgressBar.Invoke(i + 1, images.Length);
                    AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.OpenCv, images[i], ImageType.Gpu);
                    algoImage.Dispose();
                }
                sw.Stop();

            }
            else
            {
                //saveAlgoImage = ImageBuilder.Build(ImagingLibrary.OpenCv, ImageType.Gpu, images[0].Size);
                //saveAlgoImage.PutByte(images[0].Data);
                //saveAlgoImage.Save(@"C:\temp\UploadTest_WithoutAlloc.bmp");
                //saveAlgoImage.Dispose();
                //saveAlgoImage = null;

                AlgoImage algoImage2 = ImageBuilder.Build(ImagingLibrary.OpenCv, ImageType.Gpu, images[0].Size);
                sw = Stopwatch.StartNew();
                for (int i = 0; i < images.Length; i++)
                {
                    SetProgressBar.Invoke(i + 1, images.Length);
                    algoImage2.SetByte(images[i].Data);
                }
                sw.Stop();
                algoImage2.Dispose();
            }

            return sw.ElapsedMilliseconds;
        }

        public long DownloadTest(AlgoImage algoImage, int count)
        {
            //AlgoImage saveAlgoImage = algoImage.ConvertTo(ImagingLibrary.OpenCv, ImageType.Grey);
            //saveAlgoImage.Save(@"C:\temp\DownloadTest.bmp");
            //saveAlgoImage.Dispose();

            //AlgoImage convert = ImageBuilder.Build(ImagingLibrary.OpenCv, ImageType.Grey, algoImage.Size);
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                SetProgressBar.Invoke(i + 1, count);
                //ImageConverter.Convert(algoImage, convert);
                AlgoImage convert = ImageConverter.Convert(algoImage, ImagingLibrary.MatroxMIL, ImageType.Grey);
                //convert.Save(@"C:\temp\convert.bmp");
                convert.Dispose();
            }
            sw.Stop();
            //convert.Dispose();
            return sw.ElapsedMilliseconds;
        }
    }
}

