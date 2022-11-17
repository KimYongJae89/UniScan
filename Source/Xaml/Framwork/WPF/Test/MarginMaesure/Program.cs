using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarginMaesure
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] filenames = new string[]
            {
                "BackLightImage.bmp",
                "TopLightImage.bmp",
                "MaskBuffer.bmp",
                "SheetBuffer.bmp"
            };

            // Init LIB
            InitLib();

            // File Load
            AlgoImage[] algoImages = new AlgoImage[filenames.Length];
            for (int i = 0; i < algoImages.Length; i++)
                algoImages[i] = ImageBuilder.Build(ImagingLibrary.MatroxMIL, new Image2D(Path.Combine(@"D:\temp\dd", filenames[i])), ImageType.Grey);

            // Process
            MarginMeasureProcess.Measure(algoImages[0], algoImages[1], algoImages[2], algoImages[3]);

            // Unload
            Array.ForEach(algoImages, f => f.Dispose());

            // Uninit LIB
            UninitLib();
        }

        private static void InitLib()
        {
            MatroxHelper.InitApplication();
        }

        private static void UninitLib()
        {
            MatroxHelper.FreeApplication();
        }
    }
}
