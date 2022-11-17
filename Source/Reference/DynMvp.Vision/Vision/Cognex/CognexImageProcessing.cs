using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using DynMvp.UI;

using Cognex.VisionPro;
using Cognex.VisionPro.Blob;
using Cognex.VisionPro.ImageProcessing;
using System.Drawing;
using DynMvp.Base;

namespace DynMvp.Vision.Cognex
{
    public class CognexImageProcessing : ImageProcessing
    {
        public override bool StreamExist => false;
        public override bool WaitStream() { return true; }
        public override void Dispose() { }

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

        public override long[] Histogram(AlgoImage algoImage)
        {
            const int HIST_NUM_INTENSITIES = 256;
            //MIL_INT[]
            long[] HistValues = new long[HIST_NUM_INTENSITIES];
            return HistValues;
        }

        // Auto Threshold
        public override double GetBinarizeValue(AlgoImage srcImage)
        {
            throw new NotImplementedException();
        }

        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, bool inverse = false)
        {
            CognexGreyImage cognexSrcImage = (CognexGreyImage)srcImage;
            CognexGreyImage cognexDestImage = (CognexGreyImage)destImage;

            CogBlob blob = new CogBlob();
            blob.SaveSegmentedImage = true;
            blob.SegmentationParams.SetSegmentationHardDynamicThreshold(0, 0, CogBlobSegmentationPolarityConstants.LightBlobs);
            CogBlobResults blobResults = blob.Execute(cognexSrcImage.Image, null);
            cognexDestImage.Image = blobResults.CreateSegmentedImage();
        }

        // Single Threshold
        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int threshold, bool inverse = false)
        {
            CognexGreyImage cognexSrcImage = (CognexGreyImage)srcImage;
            CognexGreyImage cognexDestImage = (CognexGreyImage)destImage;

            CogBlob blob = new CogBlob();
            blob.SegmentationParams.SetSegmentationHardFixedThreshold(threshold, CogBlobSegmentationPolarityConstants.LightBlobs);
            blob.SaveSegmentedImage = true;
            CogBlobResults blobResults = blob.Execute(cognexSrcImage.Image, null);
            cognexDestImage.Image = blobResults.CreateSegmentedImage();
//            CogImage8Grey image = blobResults.CreateSegmentedImage();
        }

        // Double Threshold
        public override void Binarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower, int thresholdUpper, bool inverse = false)
        {
            throw new NotImplementedException();
        }
        
        public override void BinarizeHistogram(AlgoImage srcImage, AlgoImage destImage, int percent)
        {
            throw new NotImplementedException();
        }

        public override void Morphology(AlgoImage srcImage, AlgoImage destImage, MorphologyType type, int[,] kernalXY, int repeatNum, bool isGray = false)
        {
            throw new NotImplementedException();
        }

        public override void Erode(AlgoImage srcImage, AlgoImage destImage, int numErode, int kernelSize = 3, bool useGray = false)
        {
            CognexGreyImage cognexSrcImage = (CognexGreyImage)srcImage;
            CognexGreyImage cognexDestImage = (CognexGreyImage)destImage;

            CogRectangle rectRegion = new CogRectangle();
            rectRegion.SetXYWidthHeight(0, 0, srcImage.Width, srcImage.Height);

            CogIPOneImageGreyMorphology imageMorphic = new CogIPOneImageGreyMorphology();
            imageMorphic.Operation = CogIPOneImageMorphologyOperationConstants.Erode;
            cognexDestImage.Image = (CogImage8Grey)imageMorphic.Execute(cognexSrcImage.Image, CogRegionModeConstants.PixelAlignedBoundingBox, rectRegion);
        }

        public override void Dilate(AlgoImage srcImage, AlgoImage destImage, int numDilate, int kernelSize = 3, bool useGray = false)
        {
            CognexGreyImage cognexSrcImage = (CognexGreyImage)srcImage;
            CognexGreyImage cognexDestImage = (CognexGreyImage)destImage;

            CogRectangle rectRegion = new CogRectangle();
            rectRegion.SetXYWidthHeight(0, 0, srcImage.Width, srcImage.Height);

            CogIPOneImageGreyMorphology imageMorphic = new CogIPOneImageGreyMorphology();
            imageMorphic.Operation = CogIPOneImageMorphologyOperationConstants.Dilate;
            cognexDestImage.Image = (CogImage8Grey)imageMorphic.Execute(cognexSrcImage.Image, CogRegionModeConstants.PixelAlignedBoundingBox, rectRegion);
        }

        public override void Open(AlgoImage srcImage, AlgoImage destImage, int numDilate, bool useGray)
        {
            throw new NotImplementedException();
        }

        public override void Close(AlgoImage srcImage, AlgoImage destImage, int numDilate, bool useGray)
        {
            throw new NotImplementedException();
        }

        public override void TopHat(AlgoImage srcImage, AlgoImage destImage, int numTopHat, bool useGray)
        {
            throw new NotImplementedException();
        }

        public override void BottomHat(AlgoImage srcImage, AlgoImage destImage, int numTopHat, bool useGray)
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

        public override float GetStdDev(AlgoImage algoImage)
        {
            throw new NotImplementedException();
        }

        public override float GetStdDev(AlgoImage algoImage, AlgoImage maskImage)
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

        public override void Sobel(AlgoImage algoImage, int size = 3)
        {
            if (algoImage.ImageType == ImageType.Grey)
            {
                CognexGreyImage greyImage = (CognexGreyImage)algoImage;
                CogSobelEdge sobelEdge = new CogSobelEdge();
                CogSobelEdgeResult sobelResult = sobelEdge.Execute(greyImage.Image, null);
                greyImage.Image = sobelResult.EdgeMagnitudeImage;
            }
            else
            {
                Debug.Assert(false, "Image must be grey iamge");
            }
        }

        public override void And(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Not(AlgoImage algoImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Canny(AlgoImage greyImage, double threshold, double thresholdLinking)
        {
            throw new NotImplementedException();
        }

        public override int Projection(AlgoImage greyImage, ref float[] projData, Direction projectionDir, ProjectionType projectionType)
        {
            throw new NotImplementedException();
        }

        public override float[] Projection(AlgoImage greyImage, AlgoImage maskImage, Direction projectionDir, ProjectionType projectionType)
        {
            throw new NotImplementedException();
        }

        public override float[] Projection(AlgoImage greyImage, Direction projectionDir, ProjectionType projectionType)
        {
            throw new NotImplementedException();
        }

        public override void LogPolar(AlgoImage greyImage)
        {
            throw new NotImplementedException();
        }

        public override double CodeTest(AlgoImage algoImage1, AlgoImage algoImage2, int[] intParams, double[] dblParams)
        {
            throw new NotImplementedException();
        }

        public override BlobRectList Blob(AlgoImage algoImage, BlobParam blobParam, AlgoImage greyMask = null)
        {
            throw new NotImplementedException();
        }

        public override void FilterBlob(BlobRectList blobRectList, BlobParam blobParam)
        {
            throw new NotImplementedException();
        }

        public override void DrawBlob(AlgoImage algoImage, BlobRectList blobRectList, BlobRect[] blobRects, DrawBlobOption drawBlobOption)
        {
            throw new NotImplementedException();
        }

        public override void AvgStdDevXy(AlgoImage greyImage, AlgoImage maskImage, out float[] avgX, out float[] stdDevX, out float[] avgY, out float[] stdDevY)
        {
            throw new NotImplementedException();
        }

        public override void Average(AlgoImage srcImage, AlgoImage destImage)
        {
            if (srcImage.ImageType == ImageType.Grey)
            {
                CognexGreyImage greyImageSrc = (CognexGreyImage)srcImage;
                CognexGreyImage greyImageDst = (CognexGreyImage)destImage;
                CogImageAverage imageAverage = new CogImageAverage();
                imageAverage.Add(greyImageSrc.Image, null);
                greyImageDst.Image = imageAverage.ExecuteAverage();
            }
            else
            {
                Debug.Assert(false, "Image must be grey iamge");
            }
        }

        public override void Level(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void HistogramStretch(AlgoImage algoImage)
        {
            //if (algoImage.ImageType != ImageType.Grey)
            //{
            //    throw new InvalidImageTypeException();
            //}

            //OpenEVisionGreyImage greyImage = (OpenEVisionGreyImage)algoImage;

            //EBWHistogramVector histogramVector = new EBWHistogramVector();

            //EasyImage.Histogram(greyImage.Image, histogramVector);

            //float maxIndex = EasyImage.AnalyseHistogram(histogramVector, EHistogramFeature.GreatestPixelValue);
            //float minIndex = EasyImage.AnalyseHistogram(histogramVector, EHistogramFeature.SmallestPixelValue);

            //EBW8Vector lutVector = new EBW8Vector();

            //double scale_factor = 256.0 / (maxIndex - minIndex);

            //for (int i = 0; i < 256; i++)
            //{
            //    byte value;

            //    if (i < minIndex)
            //        value = 0;
            //    else if (i > maxIndex)
            //        value = 255;
            //    else
            //    {
            //        int intValue = (int)((i - minIndex) * scale_factor);
            //        if (intValue < 255)
            //            value = (byte)((i - minIndex) * scale_factor);
            //        else
            //            value = 255;
            //    }

            //    lutVector.AddElement(new EBW8(value));
            //}

            //EImageBW8 destImage = new EImageBW8(greyImage.Image.Width, greyImage.Image.Height);
            //EasyImage.Lut(greyImage.Image, destImage, lutVector);

            //greyImage.Image = destImage;
        }

        public override void HistogramEqualization(AlgoImage algoImage)
        {
            if (algoImage.ImageType == ImageType.Grey)
            {
                CognexGreyImage greyImage = (CognexGreyImage)algoImage;

                CogRectangle rectRegion = new CogRectangle();
                rectRegion.SetXYWidthHeight(0, 0, greyImage.Width, greyImage.Height);

                CogIPOneImageEqualize imageEqualizer = new CogIPOneImageEqualize();
                greyImage.Image = (CogImage8Grey)imageEqualizer.Execute(greyImage.Image, CogRegionModeConstants.PixelAlignedBoundingBox, rectRegion);
            }
        }

        public override void FillHoles(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void FindHoles(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void WeightedAdd(AlgoImage[] srcImages, AlgoImage dstImage)
        {
            throw new NotImplementedException();
        }

        public override void Add(AlgoImage image1, AlgoImage image2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Subtract(AlgoImage image1, AlgoImage image2, AlgoImage destImage, bool isAbs = false)
        {
            throw new NotImplementedException();
        }

        public override void Median(AlgoImage srcImage, AlgoImage destImage, int size)
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

        public override void Or(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void XOr(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Translate(AlgoImage srcImage, AlgoImage destImage, Point offset)
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

        public override void DrawArc(AlgoImage srcImage, ArcEq arcEq, double value, bool filled)
        {
            throw new NotImplementedException();
        }

        public override void EraseBoder(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void ReconstructIncludeBlob(AlgoImage srcImage, AlgoImage destImage, AlgoImage seedImage, ResconstructParam resconstructParam)
        {
            throw new NotImplementedException();
        }

        public override void EdgeDetect(AlgoImage srcImage, AlgoImage destImage, AlgoImage maskImage = null, double scaleFactor = 1)
        {
            throw new NotImplementedException();
        }

        public override void Resize(AlgoImage srcImage, AlgoImage destImage, double scaleFactorX, double scaleFactorY)
        {
            throw new NotImplementedException();
        }

        public override void Stitch(AlgoImage srcImage1, AlgoImage srcImage2, AlgoImage destImage, Direction direction)
        {
            throw new NotImplementedException();
        }

        public override void Min(AlgoImage image1, AlgoImage image2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Max(AlgoImage image1, AlgoImage image2, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override float[,] FFT(AlgoImage srcImage)
        {
            throw new NotImplementedException();
        }

        public override void AdaptiveBinarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower)
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

        public override void Subtract(AlgoImage srcImage, AlgoImage dstImage, int value)
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

        public override void Div(AlgoImage srcImage, AlgoImage dstImage, float value)
        {
            throw new NotImplementedException();
        }

        public override void Compare(AlgoImage image1, AlgoImage image2, AlgoImage dstImage)
        {
            throw new NotImplementedException();
        }

        public override void Sobel(AlgoImage srcImage, AlgoImage destImage)
        {
            throw new NotImplementedException();
        }

        public override void Sobel(AlgoImage srcImage, AlgoImage destImage, Direction direction)
        {
            throw new NotImplementedException();
        }

        public override void Clipping(AlgoImage srcImage, AlgoImage dstImage, int minValue, int minLimit, int maxLimit, int maxValue)
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

        public override void Flip(AlgoImage srcImage, AlgoImage destImage, Direction direction)
        {
            throw new NotImplementedException();
        }

        public override void Rotate(AlgoImage srcImage, AlgoImage destImage, float angle)
        {
            throw new NotImplementedException();
        }

        public override void DrawPolygon(AlgoImage srcImage, float[] xArray, float[] yArray, double value, bool filled)
        {
            throw new NotImplementedException();
        }

        public override BlobRectList BlobMerge(BlobRectList blobRectList1, BlobRectList blobRectList2, BlobParam blobParam)
        {
            throw new NotImplementedException();
        }

        public override BlobRectList EreseBorderBlobs(BlobRectList blobRectList, BlobParam blobParam)
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
