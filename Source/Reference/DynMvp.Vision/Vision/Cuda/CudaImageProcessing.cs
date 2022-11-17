using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision.Cuda;

namespace DynMvp.Vision.Vision.Cuda
{
    public class CudaImageProcessing : ImageProcessing
    {
        public override bool StreamExist => throw new NotImplementedException();

        public static void CheckCudaImage(AlgoImage algoImage, string functionName, string variableName)
        {
            if (algoImage == null)
                throw new InvalidSourceException(String.Format("[{0}] {1} Image is null", functionName, variableName));

            try
            {
                CudaImage image = algoImage as CudaImage;
                if (image.ImageID == 0)
                {
                    throw new InvalidTargetException(String.Format("[{0}] {1} Image Object is null", functionName, variableName));
                }
            }
            catch (InvalidCastException)
            {
                throw new InvalidSourceException(String.Format("[{0}] {1} Image must be gray image", functionName, variableName));
            }
        }

        public override IConvolveKernel CreateConvolveKernel(Kernel kernel, int normalize)
        {
            throw new NotFiniteNumberException();
        }

        public override void Convolve(AlgoImage srcImage, AlgoImage destImage, IConvolveKernel kernel)
        {
            throw new NotFiniteNumberException();
        }

        public override IMorphicStruct CreateMorphicStruct(Kernel kernel)
        {
            throw new NotFiniteNumberException();
        }

        public override void Morphic(AlgoImage srcImage, AlgoImage destImage, IMorphicStruct morpStruct, EMorphicType morphicType, int iteration, bool isGrayScale)
        {
            throw new NotFiniteNumberException();
        }

        public override void AdaptiveBinarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower)
        {
            CheckCudaImage(srcImage, "CudaImageProcessing.AdaptiveBinarize", "srcImage");
            CheckCudaImage(srcImage, "CudaImageProcessing.AdaptiveBinarize", "destImage");

            CudaImage cudaSrcImage = srcImage as CudaImage;
            CudaImage cudaDstImage = destImage as CudaImage;

            CudaMethods.CUDA_ADAPTIVE_BINARIZE_LOWER(cudaSrcImage.ImageID, cudaDstImage.ImageID, thresholdLower);
        }

        public override void Add(AlgoImage image1, AlgoImage image2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Add(AlgoImage srcImage, AlgoImage dstImage, int value)
        {
            throw new NotImplementedException();
        }

        public override void Add(AlgoImage srcImage, AlgoImage dstImage, AlgoImage maskImage, int value)
        {
            throw new NotImplementedException();
        }

        public override void And(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Average(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Level(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void AvgStdDevXy(AlgoImage greyImage, AlgoImage maskImage, out float[] avgX, out float[] stdDevX, out float[] avgY, out float[] stdDevY)
        {
            throw new NotImplementedException();
        }

        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int threshold, bool inverse = false)
        {
            throw new NotImplementedException();
        }

        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower, int thresholdUpper, bool inverse = false)
        {
            throw new NotImplementedException();
        }

        public override double GetBinarizeValue(AlgoImage srcImage)
        {
            throw new NotImplementedException();
        }

        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, bool inverse = false)
        {
            throw new NotImplementedException();
        }

        public override void BinarizeHistogram(AlgoImage srcImage, AlgoImage destImage, int percent)
        {
            throw new NotImplementedException();
        }

        public override BlobRectList Blob(AlgoImage algoImage, BlobParam blobParam, AlgoImage greyMask = null)
        {
            CheckCudaImage(algoImage, "CudaImageProcessing.AdaptiveBinarize", "algoImage");

            CudaImage cudaBinImage = algoImage as CudaImage;
            CudaImage cudaGreyImage = greyMask as CudaImage;

            int count = CudaMethods.CUDA_LABELING(cudaBinImage.ImageID);

            UInt32[] areaArray = new UInt32[count];
            UInt32[] xMinArray = new UInt32[count];
            UInt32[] xMaxArray = new UInt32[count];
            UInt32[] yMinArray = new UInt32[count];
            UInt32[] yMaxArray = new UInt32[count];
            byte[] vMinArray = new byte[count];
            byte[] vMaxArray = new byte[count];
            float[] vMeanArray = new float[count];

            CudaMethods.CUDA_BLOBING(cudaBinImage.ImageID, cudaGreyImage.ImageID, count,
                areaArray, xMinArray, xMaxArray, yMinArray, yMaxArray,
                vMinArray, vMaxArray, vMeanArray);

            BlobRectList blobRectList = new BlobRectList();

            for (int i = 0; i < count; i++)
            {
                BlobRect blobRect = new BlobRect();
                blobRect.BoundingRect = new RectangleF(xMinArray[i], yMinArray[i], xMaxArray[i] - xMinArray[i] + 1, yMaxArray[i] - yMinArray[i] + 1);

                blobRect.Area = areaArray[i];
                blobRect.MinValue = vMinArray[i];
                blobRect.MaxValue = vMaxArray[i];
                blobRect.MeanValue = vMeanArray[i];

                blobRectList.GetList().Add(blobRect);
            }

            return blobRectList;//milProcessing.Blob(milImage, blobParam);
        }

        public override void FilterBlob(BlobRectList blobRectList, BlobParam blobParam)
        {
            throw new NotImplementedException();
        }

        public override BlobRectList BlobMerge(BlobRectList blobRectList1, BlobRectList blobRectList2, BlobParam blobParam)
        {
            throw new NotImplementedException();
        }

        public override void BottomHat(AlgoImage srcImage, AlgoImage destImage, int numClose, bool useGray = false)
        {
            throw new NotImplementedException();
        }
        
        public override void Canny(AlgoImage greyImage, double threshold, double thresholdLinking)
        {
            throw new NotImplementedException();
        }

        public override void Clear(AlgoImage algoImage, byte value)
        {
            throw new NotImplementedException();
        }

        public override void Clear(AlgoImage srcImage, Rectangle rect, Color value)
        {
            throw new NotImplementedException();
        }

        public override void Clear(AlgoImage image, AlgoImage mask, Color value)
        {
            throw new NotImplementedException();
        }

        public override void Clipping(AlgoImage srcImage, AlgoImage dstImage, int minValue, int minLimit, int maxLimit, int maxValue)
        {
            throw new NotImplementedException();
        }

        public override void Close(AlgoImage srcImage, AlgoImage destImage, int numClose, bool useGray = false)
        {
            throw new NotImplementedException();
        }

        public override double CodeTest(AlgoImage algoImage1, AlgoImage algoImage2, int[] intParams, double[] dblParams)
        {
            throw new NotImplementedException();
        }

        public override void Compare(AlgoImage image1, AlgoImage image2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }
        
        public override void Count(AlgoImage algoImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel)
        {
            throw new NotImplementedException();
        }

        public override void Count(AlgoImage algoImage, AlgoImage maskImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel)
        {
            throw new NotImplementedException();
        }

        public override void Count2(AlgoImage algoImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel)
        {
            throw new NotImplementedException();
        }

        public override void Count2(AlgoImage algoImage, AlgoImage maskImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel)
        {
            throw new NotImplementedException();
        }

        public override void CustomBinarize(AlgoImage srcImage, AlgoImage destImage, bool inverse)
        {
            throw new NotImplementedException();
        }

        public override void CustomEdge(AlgoImage srcImage, AlgoImage destImage, AlgoImage bufferX, AlgoImage bufferY, int length)
        {
            throw new NotImplementedException();
        }

        public override void Dilate(AlgoImage srcImage, AlgoImage destImage, int numDilate, int kernelSize = 3, bool useGray = false)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override void Div(AlgoImage srcImage, AlgoImage dstImage, float value)
        {
            throw new NotImplementedException();
        }

        public override void DrawArc(AlgoImage srcImage, ArcEq arcEq, double value, bool filled)
        {
            throw new NotImplementedException();
        }

        public override void DrawBlob(AlgoImage algoImage, BlobRectList blobRectList, BlobRect[] blobRects, DrawBlobOption drawBlobOption)
        {
            throw new NotImplementedException();
        }

        public override void DrawPolygon(AlgoImage srcImage, float[] xArray, float[] yArray, double valueS, bool filled)
        {
            throw new NotImplementedException();
        }

        public override void DrawRect(AlgoImage srcImage, Rectangle rectangle, double value, bool filled)
        {
            throw new NotImplementedException();
        }

        public override void DrawRotateRact(AlgoImage srcImage, RotatedRect rotateRect, double value, bool filled)
        {
            throw new NotImplementedException();
        }

        public override void DrawText(AlgoImage srcImage, Point pointLT, double value, string text, double size = 1)
        {
            throw new NotImplementedException();
        }

        public override void EdgeDetect(AlgoImage srcImage, AlgoImage destImage, AlgoImage maskImage = null, double scaleFactor = 1)
        {
            throw new NotImplementedException();
        }

        public override void EraseBoder(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override BlobRectList EreseBorderBlobs(BlobRectList blobRectList, BlobParam blobParam)
        {
            throw new NotImplementedException();
        }
        
        public override void Erode(AlgoImage srcImage, AlgoImage destImage, int numErode, int kernelSize = 3, bool useGray = false)
        {
            throw new NotImplementedException();
        }

        public override float[,] FFT(AlgoImage srcImage)
        {
            throw new NotImplementedException();
        }

        public override void FillHoles(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void FindHoles(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Flip(AlgoImage srcImage, AlgoImage destImage, Direction direction)
        {
            throw new NotImplementedException();
        }

        public override Color GetColorAverage(AlgoImage algoImage, Rectangle rect)
        {
            throw new NotImplementedException();
        }

        public override Color GetColorAverage(AlgoImage algoImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        public override float GetGreyAverage(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override float GetGreyAverage(AlgoImage algoImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        public override float GetGreyMax(AlgoImage algoImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        public override Point GetGreyMaxPos(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override float GetGreyMin(AlgoImage algoImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        public override Point GetGreyMinPos(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override StatResult GetStatValue(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override StatResult GetStatValue(AlgoImage algoImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        public override float GetStdDev(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override float GetStdDev(AlgoImage algoImage, AlgoImage maskImage)
        {
            throw new NotImplementedException();
        }

        public override long[] Histogram(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override void HistogramEqualization(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override void HistogramStretch(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override void LogPolar(AlgoImage greyImage)
        {
            throw new NotImplementedException();
        }

        public override void Max(AlgoImage image1, AlgoImage image2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Median(AlgoImage srcImage, AlgoImage destImage, int size)
        {
            throw new NotImplementedException();
        }

        public override void Min(AlgoImage image1, AlgoImage image2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Morphology(AlgoImage srcImage, AlgoImage destImage, MorphologyType type, int[,] kernalXY, int repeatNum, bool isGray = false)
        {
            throw new NotImplementedException();
        }

        public override void Mul(AlgoImage srcImage, AlgoImage dstImage, float value)
        {
            throw new NotImplementedException();
        }

        public override void MulMultiple(AlgoImage srcImage, AlgoImage dstImage, float mul, float sum, float div)
        {
            throw new NotImplementedException();
        }

        public override void Not(AlgoImage algoImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Open(AlgoImage srcImage, AlgoImage destImage, int numOpen, bool useGray = false)
        {
            throw new NotImplementedException();
        }

        public override void Or(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void XOr(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override float[] Projection(AlgoImage greyImage, Direction projectionDir, ProjectionType projectionType)
        {
            throw new NotImplementedException();
        }

        public override float[] Projection(AlgoImage greyImage, AlgoImage maskImage, Direction projectionDir, ProjectionType projectionType)
        {
            throw new NotImplementedException();
        }

        public override int Projection(AlgoImage greyImage, ref float[] projData, Direction projectionDir, ProjectionType projectionType)
        {
            throw new NotImplementedException();
        }

        public override void ReconstructIncludeBlob(AlgoImage srcImage, AlgoImage destImage, AlgoImage seedImage, ResconstructParam resconstructParam)
        {
            throw new NotImplementedException();
        }
        
        public override void Resize(AlgoImage srcImage, AlgoImage destImage, double scaleFactorX, double scaleFactorY)
        {
            throw new NotImplementedException();
        }
        
        public override void Rotate(AlgoImage srcImage, AlgoImage destImage, float angle)
        {
            throw new NotImplementedException();
        }
        
        public override void Sobel(AlgoImage algoImage, int size = 3)
        {
            throw new NotImplementedException();
        }

        public override void Sobel(AlgoImage algoImage1, AlgoImage algoImage2)
        {
            throw new NotImplementedException();
        }

        public override void Sobel(AlgoImage srcImage, AlgoImage destImage, Direction direction)
        {
            throw new NotImplementedException();
        }

        public override void Stitch(AlgoImage srcImage1, AlgoImage srcImage2, AlgoImage destImage, Direction direction)
        {
            throw new NotImplementedException();
        }
        
        public override void Subtract(AlgoImage srcImage, AlgoImage dstImage, int value)
        {
            throw new NotImplementedException();
        }

        public override void Subtract(AlgoImage image1, AlgoImage image2, AlgoImage destImage, bool isAbs = false)
        {
            throw new NotImplementedException();
        }

        public override void TopHat(AlgoImage srcImage, AlgoImage destImage, int numOpen, bool useGray = false)
        {
            throw new NotImplementedException();
        }

        public override void Translate(AlgoImage srcImage, AlgoImage destImage, Point offset)
        {
            throw new NotImplementedException();
        }

        public override bool WaitStream()
        {
            throw new NotImplementedException();
        }

        public override void WeightedAdd(AlgoImage[] srcImages, AlgoImage dstImage)
        {
            throw new NotImplementedException();
        }

        public override void AdaptiveBinarize(AlgoImage srcImage, AlgoImage destImage, AlgoImage seedImage, bool inverse)
        {
            throw new NotImplementedException();
        }

        public override void MulSumDiv(AlgoImage srcImage, AlgoImage dstImage, int mul, int sum, int div)
        {
            throw new NotImplementedException();
        }

        public override void Match(AlgoImage srcImage, AlgoImage template, AlgoImage score)
        {
            throw new NotImplementedException();
        }

        public override void Test(params object[] arguments)
        {
            throw new NotImplementedException();
        }

        public override float[] LineProfile(AlgoImage greyImage, Point src, Point dst)
        {
            throw new NotImplementedException();
        }
    }
}
