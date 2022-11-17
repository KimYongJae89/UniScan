using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.Vision.RCI.Trainer
{
    abstract class Reconstrctor
    {
        public static Reconstrctor Create(RCIReconOptions.EReconstruct @enum)
        {
            Reconstrctor reconstrctor;
            switch (@enum)
            {
                case RCIReconOptions.EReconstruct.Blob:
                    reconstrctor = new Reconstrct.BlobReconstrctor();
                    break;

                case RCIReconOptions.EReconstruct.Remove:
                    reconstrctor = new Reconstrct.RemoveReconstructor();
                    break;

                case RCIReconOptions.EReconstruct.None:
                default:
                    reconstrctor = new Reconstrct.PassThroughReconstructor();
                    break;
            }

            return reconstrctor;
        }

        public void GetStatResult(AlgoImage algoImage, out StatResult highStat, out StatResult lowStat)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

            using (AlgoImage maskImage = ImageBuilder.BuildSameTypeSize(algoImage))
            {
                ip.Binarize(algoImage, maskImage, true); // 전극을 하얗게.
                lowStat = ip.GetStatValue(algoImage, maskImage);

                ip.Not(maskImage); // 성형을 하얗게.
                ip.Erode(maskImage, 5);
                highStat = ip.GetStatValue(algoImage, maskImage);
            }
        }

        public abstract void Reconstruct(AlgoImage scAlgoImage, AlgoImage scModelImage, AlgoImage scWeightImage);
    }
}
