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
    public class SaturationTuneValue : TuneValue
    {
        float saturaionRatio = 0.03f;

        public SaturationTuneValue(int deviceIndex, float saturaionRatio) : base(deviceIndex)
        {
            this.saturaionRatio = saturaionRatio;
        }

        protected override float GetValue(ImageD image)
        {
            AlgoImage algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, image, ImageType.Grey);
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);


            //1. Histogram
            long[] histo = imageProcessing.Histogram(algoImage);

            long sum = 0;
            double avg = 0;
            long targetsize = (long)( algoImage.Width * algoImage.Height * saturaionRatio );
            for(int i =254; i>=0; i--)
            {
                sum += histo[i];

                avg += histo[i] * i;
                if (sum > targetsize)
                    break;
            }

            double averageIntensityTopArea = avg / (double)sum;

            algoImage.Dispose();


            //System.Diagnostics.Debug.WriteLine(whiteNum.ToString());

            //최대값 1
            return (float)(averageIntensityTopArea / 255.0);
        }
    }
}
