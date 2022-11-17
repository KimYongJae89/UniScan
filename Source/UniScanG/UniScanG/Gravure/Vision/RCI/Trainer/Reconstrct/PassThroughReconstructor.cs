using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Vision;

namespace UniScanG.Gravure.Vision.RCI.Trainer.Reconstrct
{
    class PassThroughReconstructor : Reconstrctor
    {
        public override void Reconstruct(AlgoImage scAlgoImage, AlgoImage scModelImage, AlgoImage scWeightImage)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(scAlgoImage);

            scModelImage.Copy(scAlgoImage);
            ip.Sobel(scAlgoImage, scWeightImage);
        }
    }
}
