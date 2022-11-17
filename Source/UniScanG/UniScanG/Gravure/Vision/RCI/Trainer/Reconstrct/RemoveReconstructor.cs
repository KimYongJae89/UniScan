using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Vision;

namespace UniScanG.Gravure.Vision.RCI.Trainer.Reconstrct
{
    class RemoveReconstructor : Reconstrctor
    {
        public override void Reconstruct(AlgoImage algoImage, AlgoImage modelImage, AlgoImage weightImage)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(algoImage);

            GetStatResult(algoImage, out StatResult highStat, out StatResult lowStat);

            modelImage.Copy(algoImage);
            //modelImage.Save(@"C:\temp\modelImage.bmp");

            using (AlgoImage binalImage = ImageBuilder.BuildSameTypeSize(algoImage))
            {
                // 전극을 밝게
                ip.Binarize(algoImage, binalImage, (int)((highStat.average * 1 + lowStat.average * 1) / 2), true);
                //binalImage.Save(@"C:\temp\binalImage.bmp");

                //System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, algoImage.Width / 2, algoImage.Height / 2);
                //using (AlgoImage a = algoImage.GetSubImage(rect))
                //    a.Save(@"C:\temp\algoImage.bmp");
                //using (AlgoImage a = binalImage.GetSubImage(rect))
                //    a.Save(@"C:\temp\binalImage.bmp");

                //double value = ip.GetBinarizeValue(algoImage);
                //ip.Binarize(algoImage, binalImage, true);
                //binalImage.Save(@"C:\temp\binalImage2.bmp");

                // 성형층 튐성 불량 지움
                RemoveSmallBlobs(binalImage, modelImage, (byte)highStat.average);
                //modelImage.Save(@"C:\temp\modelImage.bmp");

                // 전극 내 미인쇄 지움
                using (AlgoImage findHolesImage = ImageBuilder.BuildSameTypeSize(algoImage))
                {
                    findHolesImage.Clear();
                    ip.FindHoles(binalImage, findHolesImage);
                    //findHolesImage.Save(@"C:\temp\findHolesImage.bmp");

                    RemoveSmallBlobs(findHolesImage, modelImage, (byte)lowStat.average);
                    //modelImage.Save(@"C:\temp\modelImage.bmp");
                }

                for (int i = 0; i < 2; i++)
                    ip.Average(modelImage);
            }

            ip.Sobel(modelImage, weightImage);
        }

        private void RemoveSmallBlobs(AlgoImage blobImage, AlgoImage targetImage, byte value)
        {
            ImageProcessing ip = AlgorithmBuilder.GetImageProcessing(blobImage);
            BlobParam blobParam = new BlobParam()
            {
                SelectLabelValue = true,
                SelectArea = true,
                SelectBoundingRect = true,
                SelectRotateRect = true,
                SelectBorderBlobs = false,
                SelectCenterPt = true,
                EraseBorderBlobs = true,
                RotateWidthMax = 5,
                RotateHeightMax = 5,
            };

            using (BlobRectList list = ip.Blob(blobImage, blobParam))
            {
                using (AlgoImage maskImage = ImageBuilder.BuildSameTypeSize(blobImage))
                {
                    maskImage.Clear();
                    ip.DrawBlob(maskImage, list, null, new DrawBlobOption() { SelectBlob = true });
                    ip.Dilate(maskImage, 3);

                    using (AlgoImage bgImage = ImageBuilder.BuildSameTypeSize(targetImage))
                    {
                        bgImage.Clear(value);
                        targetImage.MaskingCopy(bgImage, maskImage);
                    }
                }
            }
        }
    }
}
