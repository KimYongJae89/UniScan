using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.Devices.Light;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanM.Algorithm;

namespace UniScanM.Algorithm
{
    //2개의 영역 분리값이 가장 커지는 값인데..
    //http://darkpgmr.tistory.com/115

    public class OtsuTuneValue : TuneValue
    {
        public OtsuTuneValue(int deviceIndex) : base(deviceIndex)
        {

        }

        protected override float GetValue(ImageD image)
        {
            AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, image, ImageType.Grey);
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            AlgoImage maskImage = ImageBuilder.Build(algoImage.LibraryType, algoImage.ImageType, algoImage.Width, algoImage.Height);
            imageProcessing.Binarize(algoImage, maskImage);
            
            float avgUpper = imageProcessing.GetGreyAverage(algoImage, maskImage);
            StatResult statResultUpper = imageProcessing.GetStatValue(algoImage, maskImage);

            imageProcessing.Not(maskImage, maskImage);

            float avgLower = imageProcessing.GetGreyAverage(algoImage, maskImage);
            StatResult statResultLower = imageProcessing.GetStatValue(algoImage, maskImage);

            algoImage.Dispose();
            maskImage.Dispose();

            double area = algoImage.Width * algoImage.Height;
            double upperRatio = statResultUpper.count / area;
            double lowerRatio = statResultLower.count / area;

            return (float)(upperRatio * lowerRatio * Math.Pow(avgLower - avgUpper, 2));
        }
    }
}
