using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

using DynMvp.Base;
using DynMvp.UI;

namespace DynMvp.Vision
{
    public enum Direction
    {
        Horizontal, Vertical
    }

    public static class DirectionMethod
    {
        public static Direction Invert(this Direction d)
        {
            return d == Direction.Horizontal ? Direction.Vertical : Direction.Horizontal;
        }
    }

    public enum ProjectionType
    {
        Sum, Mean
    }

    public enum BinarizationType
    {
        SingleThreshold, DoubleThreshold, AutoThreshold, AdaptiveThreshold, Custom
    }

    public enum EMorphicType { Open, Close, TopHat, BottomHat, Dilate, Erode, Level }

    public enum SegmentCalcType
    {
        Ratio, Count
    }

    public struct StatResult
    {
        public double count;
        public double average;
        public double min;
        public double max;
        public double stdDev;
        public double squareSum;
    }

    public interface IKernel : IDisposable
    {
        Kernel Kernel { get; }
    }

    public class Kernel 
    {
        public Size Size { get; }
        public Point Center { get; set; }
        public byte[] Datas { get; private set; }

        public Kernel(int width, int height)
        {
            this.Size = new Size(width, height);
            this.Datas = new byte[width * height];
            this.Center = new Point(-1, -1);
        }

        public void Set(byte[] bytes)
        {
            System.Diagnostics.Debug.Assert(Datas.Length == bytes.Length);
            Array.Copy(bytes, this.Datas, this.Datas.Length);
        }
    }

    public interface IConvolveKernel : IKernel
    {
        int Normalize { get; }
    }

    public interface IMorphicStruct : IKernel
    {
    }




    public abstract class ImageProcessing : IDisposable
    {
        public abstract bool StreamExist { get; }

        public abstract void Dispose();

        public static ImageProcessing Create(string algorithmType)
        {
            AlgorithmStrategy algorithmStrategy = AlgorithmBuilder.GetStrategy(algorithmType);
            return Create(algorithmStrategy.LibraryType);
        }

        public static ImageProcessing Create(ImagingLibrary imagingLibrary)
        {
            switch (imagingLibrary)
            {
                case ImagingLibrary.OpenCv:
                    return new OpenCv.OpenCvImageProcessing();
                case ImagingLibrary.EuresysOpenEVision:
                    return new Euresys.OpenEVisionImageProcessing();
                case ImagingLibrary.CognexVisionPro:
                    return new Cognex.CognexImageProcessing();
                case ImagingLibrary.MatroxMIL:
                    return new Matrox.MilImageProcessing();
                case ImagingLibrary.Halcon:
                    return null;
                case ImagingLibrary.Custom:
                    return null;
            }
            return null;
        }

        public abstract IConvolveKernel CreateConvolveKernel(Kernel kernel, int normalize);
        public abstract void Convolve(AlgoImage srcImage, AlgoImage destImage, IConvolveKernel convolvekernel);

        public abstract IMorphicStruct CreateMorphicStruct(Kernel kernel);
        public abstract void Morphic(AlgoImage srcImage, AlgoImage destImage, IMorphicStruct morpStruct, EMorphicType morphicType, int iteration, bool isGrayScale);

        public void Binarize(AlgoImage srcImage, BinarizationType binarizationType, int thresholdLower, int thresholdUpper, bool inverse = false)
        {
            Binarize(srcImage, srcImage, binarizationType, thresholdLower, thresholdUpper, inverse);
        }

        public void Binarize(AlgoImage srcImage, AlgoImage destImage, BinarizationType binarizationType, int thresholdLower, int thresholdUpper, bool inverse = false)
        {
            switch (binarizationType)
            {
                case BinarizationType.SingleThreshold:
                    Binarize(srcImage, destImage, thresholdLower, inverse);
                    break;
                case BinarizationType.DoubleThreshold:
                    Binarize(srcImage, destImage, thresholdLower, thresholdUpper, inverse);
                    break;
                case BinarizationType.AutoThreshold:
                    //Otsu(srcImage, destImage);
                    Binarize(srcImage, srcImage, inverse);
                    break;
                case BinarizationType.AdaptiveThreshold:
                    AdaptiveBinarize(srcImage, destImage, thresholdLower);
                    break;
                case BinarizationType.Custom:
                    CustomBinarize(srcImage, destImage, inverse);
                    break;
            }
        }

        public abstract void CustomBinarize(AlgoImage srcImage, AlgoImage destImage, bool inverse);
        public abstract void AdaptiveBinarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower);
        public abstract void AdaptiveBinarize(AlgoImage srcImage, AlgoImage destImage, AlgoImage seedImage, bool inverse);

        public void Binarize(AlgoImage algoImage, bool inverse = false)
        {
            Binarize(algoImage, algoImage, inverse);
        }

        public void Binarize(AlgoImage algoImage, int threshold, bool inverse = false)
        {
            Binarize(algoImage, algoImage, threshold, inverse);
        }

        public void Binarize(AlgoImage algoImage, int thresholdLower, int thresholdUpper, bool inverse = false)
        {
            Binarize(algoImage, algoImage, thresholdLower, thresholdUpper, inverse);
        }

        public void Erode(AlgoImage algoImage, int numErode, int kernelSize = 3)
        {
            Erode(algoImage, algoImage, numErode, kernelSize);
        }

        public void Dilate(AlgoImage algoImage, int numDilate, int kernelSize = 3)
        {
            Dilate(algoImage, algoImage, numDilate, kernelSize);
        }

        public void Open(AlgoImage algoImage, int numOpen)
        {
            Open(algoImage, algoImage, numOpen);
        }

        public void Close(AlgoImage algoImage, int numClose)
        {
            Close(algoImage, algoImage, numClose);
        }

        public abstract bool WaitStream();

        public abstract void Clipping(AlgoImage srcImage, AlgoImage dstImage, int minValue, int minLimit, int maxLimit, int maxValue);
        public abstract void MulSumDiv(AlgoImage srcImage, AlgoImage dstImage, int mul, int sum, int div);

        // Auto Threshold
        public abstract double GetBinarizeValue(AlgoImage srcImage);
        public abstract void Binarize(AlgoImage srcImage, AlgoImage destImage, bool inverse = false);
        public abstract void Binarize(AlgoImage srcImage, AlgoImage destImage, int threshold, bool inverse = false);
        public abstract void Binarize(AlgoImage srcImage, AlgoImage destImage, int thresholdLower, int thresholdUpper, bool inverse = false);

        public enum MorphologyType { Dilate, Erode }
        public static int[,] Kernal_3x3_Cross => new int[,] { { -1, +1, -1 }, { +1, +1, +1 }, { -1, +1, -1 } };
        public abstract void Morphology(AlgoImage srcImage, AlgoImage destImage, MorphologyType type, int[,] kernalXY, int repeatNum, bool isGray = false);

        public abstract void Erode(AlgoImage srcImage, AlgoImage destImage, int numErode, int kernelSize = 3, bool useGray = false);
        public abstract void Dilate(AlgoImage srcImage, AlgoImage destImage, int numDilate, int kernelSize = 3, bool useGray = false);
        public abstract void Open(AlgoImage srcImage, AlgoImage destImage, int numOpen, bool useGray = false);
        public abstract void Close(AlgoImage srcImage, AlgoImage destImage, int numClose, bool useGray = false);
        public abstract void TopHat(AlgoImage srcImage, AlgoImage destImage, int numOpen, bool useGray = false);
        public abstract void BottomHat(AlgoImage srcImage, AlgoImage destImage, int numClose, bool useGray = false);

        public abstract void Count(AlgoImage algoImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel);
        public abstract void Count(AlgoImage algoImage, AlgoImage maskImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel);

        public abstract void Count2(AlgoImage algoImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel);
        public abstract void Count2(AlgoImage algoImage, AlgoImage maskImage, out int numBlackPixel, out int numGreyPixel, out int numWhitePixel);

        public abstract float GetGreyAverage(AlgoImage algoImage);
        public float GetGreyAverage(AlgoImage algoImage, Rectangle rectangle)
        {
            AlgoImage subImage = algoImage.GetSubImage(rectangle);
            float average = GetGreyAverage(subImage);
            subImage.Dispose();
            return average;
        }
        public abstract float GetGreyAverage(AlgoImage algoImage, AlgoImage maskImage);

        public abstract void Flip(AlgoImage srcImage, AlgoImage destImage, Direction direction);

        public Color GetColorAverage(AlgoImage algoImage)
        {
            return GetColorAverage(algoImage, Rectangle.Empty);
        }

        public abstract Color GetColorAverage(AlgoImage algoImage, Rectangle rect);
        public abstract Color GetColorAverage(AlgoImage algoImage, AlgoImage maskImage);

        public abstract float GetGreyMax(AlgoImage algoImage, AlgoImage maskImage = null);
        public abstract Point GetGreyMaxPos(AlgoImage algoImage);

        public abstract float GetGreyMin(AlgoImage algoImage, AlgoImage maskImage = null);
        public abstract Point GetGreyMinPos(AlgoImage algoImage);

        public abstract float GetStdDev(AlgoImage algoImage);
        public abstract float GetStdDev(AlgoImage algoImage, AlgoImage maskImage);

        public abstract StatResult GetStatValue(AlgoImage algoImage);
        public abstract StatResult GetStatValue(AlgoImage algoImage, AlgoImage maskImage);

        public Bitmap CreateRectMask(int width, int height, RectangleF maskRect)
        {
            Rectangle imageRect = new Rectangle(0, 0, width, height);

            Bitmap rgbImage = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(rgbImage);
            g.FillRectangle(new SolidBrush(Color.Black), imageRect);
            RectangleFigure figure = new RectangleFigure(maskRect, new Pen(Color.White), new SolidBrush(Color.White));
            figure.Draw(g, new CoordTransformer(), true);

            g.Dispose();

            Bitmap grayImage = ImageHelper.MakeGrayscale(rgbImage);

            rgbImage.Dispose();

            return grayImage;
        }

        public Bitmap CreateCircleMask(int width, int height, int xRadius = 0, int yRadius = 0, int centerX = 0, int centerY = 0)
        {
            Rectangle imageRect = new Rectangle(0, 0, width, height);

            Rectangle circleRect;
            if (xRadius == 0 || yRadius == 0)
                circleRect = imageRect;
            else
            {
                if (centerX == 0 && centerY == 0)
                {
                    circleRect = new Rectangle(width / 2 - xRadius, height / 2 - yRadius, xRadius * 2, yRadius * 2);
                }
                else
                {
                    circleRect = new Rectangle(centerX - xRadius, centerY - yRadius, xRadius * 2, yRadius * 2);
                }
            }

            Bitmap rgbImage = new Bitmap(width, height);

            Graphics g = Graphics.FromImage(rgbImage);
            g.FillRectangle(new SolidBrush(Color.Black), imageRect);
            EllipseFigure figure = new EllipseFigure(circleRect, new Pen(Color.White), new SolidBrush(Color.White));
            figure.Draw(g, new CoordTransformer(), true);

            g.Dispose();

            //            ImageHelper.SaveImage(rgbImage, String.Format("{0}\\MaskImage.bmp", Configuration.TempFolder));

            Bitmap grayImage = ImageHelper.MakeGrayscale(rgbImage);

            rgbImage.Dispose();

            return grayImage;
        }

        private long[] GetHistogram(byte[] imageData, byte[] maskData, int width, int height)
        {
            long[] histogram = new long[256];

            if (maskData != null)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        int index = (j * width) + i;

                        if (maskData[index] > 0)
                        {
                            histogram[imageData[index]]++;
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        if (imageData[(j * width) + i] > 0)
                            histogram[imageData[(j * width) + i]]++;
                    }
                }
            }

            return histogram;
        }

        public float Li(long[] histogram)
        {
            double tolerance = 0.5f;

            long count = histogram.Sum();

            double mean = 0;
            double sum = 0;

            for (int ih = 0 + 1; ih < 256; ih++)
                sum += ih * histogram[ih];

            mean = sum / count;

            double sumBack;           /* sum of the background pixels at a given threshold */
            double sumObj;            /* sum of the object pixels at a given threshold */
            double numBack;           /* number of background pixels at a given threshold */
            double numObj;            /* number of object pixels at a given threshold */

            double meanBack;       /* mean of the background pixels at a given threshold */
            double meanObj;        /* mean of the object pixels at a given threshold */

            double newThreshold = mean;
            double oldThreshold = 0;
            double threshold = 0;

            do
            {
                oldThreshold = newThreshold;
                threshold = oldThreshold + 0.5f;

                /* Calculate the means of background and object pixels */

                /* Background */
                sumBack = 0;
                numBack = 0;
                for (int ih = 0; ih <= (int)threshold; ih++)
                {
                    sumBack += ih * histogram[ih];
                    numBack += histogram[ih];
                }

                meanBack = (numBack == 0 ? 0 : (sumBack / numBack));

                sumObj = 0;
                numObj = 0;
                for (int ih = (int)threshold + 1; ih < 256; ih++)
                {
                    sumObj += ih * histogram[ih];
                    numObj += histogram[ih];
                }

                meanObj = (numObj == 0 ? 0 : (sumObj / numObj));

                /* Calculate the new threshold: Equation (7) in Ref. 2 */
                double logDiff = Math.Log(meanBack) - Math.Log(meanObj);
                newThreshold = (meanBack - meanObj) / logDiff;

                /* 
                    Stop the iterations when the difference between the
                    new and old threshold values is less than the tolerance 
                */
            }
            while (Math.Abs(newThreshold - oldThreshold) > tolerance);

            return (float)threshold;
        }

        public float Li(AlgoImage srcImage, AlgoImage destImage = null, AlgoImage maskImage = null)
        {
            //srcImage.Save(@"d:\temp\tt.bmp", null);

            byte[] imageData = srcImage.GetByte();
            byte[] maskData = maskImage != null ? maskImage.GetByte() : null;

            return Li(GetHistogram(imageData, maskData, srcImage.Width, srcImage.Height));
        }

        public float Otsu(AlgoImage srcImage, AlgoImage destImage = null, AlgoImage maskImage = null)
        {
            byte[] imageData = srcImage.GetByte();
            byte[] maskData = null;

            if (maskImage != null)
                maskData = maskImage.GetByte();

            int pitch = 4 * ((int)(Math.Truncate(((float)srcImage.Width - (float)1) / (float)4)) + 1);

            long[] histogram = new long[256];

            if (maskData != null)
            {
                for (int j = 0; j < srcImage.Height; j++)
                {
                    for (int i = 0; i < srcImage.Width; i++)
                    {
                        int index = (j * srcImage.Width) + i;

                        if (maskData[index] > 0)
                        {
                            histogram[imageData[index]]++;
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < srcImage.Height; j++)
                {
                    for (int i = 0; i < srcImage.Width; i++)
                    {
                        histogram[imageData[(j * srcImage.Width) + i]]++;
                    }
                }
            }

            float thresholdValue = Otsu(histogram);

            if (destImage != null)
            {
                if (maskData != null)
                {
                    Parallel.For(0, srcImage.Height, j =>
                    {
                        for (int i = 0; i < srcImage.Width; i++)
                        {
                            int index = (j * srcImage.Width) + i;

                            if (maskData[index] > 0)
                            {
                                if (imageData[index] >= thresholdValue)
                                    imageData[index] = 255;
                                else
                                    imageData[index] = 0;
                            }
                            else
                            {
                                imageData[index] = 0;
                            }
                        }
                    });
                }
                else
                {
                    Parallel.For(0, srcImage.Height, j =>
                    {
                        for (int i = 0; i < srcImage.Width; i++)
                        {
                            int index = (j * srcImage.Width) + i;

                            if (imageData[index] >= thresholdValue)
                                imageData[index] = 255;
                            else
                                imageData[index] = 0;

                            index++;
                        }
                    });
                }

                destImage.SetByte(imageData);
            }

            return thresholdValue;
        }

        public static float Otsu(long[] histogram)
        {
            long total = histogram.Sum();
            double sum = histogram.Select((f, i) => f * i).Sum();

            double sumB = 0;
            double wB = 0;
            double wF = 0;
            double mB;
            double mF;
            double max = 0.0;
            double between = 0.0;
            double threshold1 = 0.0;
            double threshold2 = 0.0;

            for (int i = 0; i < 256; i++)
            {
                wB += histogram[i];
                if (wB == 0)
                    continue;
                wF = total - wB;
                if (wF == 0)
                    break;
                sumB += i * histogram[i];
                mB = sumB / wB;
                mF = (sum - sumB) / wF;
                between = wB * wF * (mB - mF) * (mB - mF);
                if (between >= max)
                {
                    threshold1 = i;
                    if (between > max)
                    {
                        threshold2 = i;
                    }
                    max = between;
                }
            }

            return (float)((threshold1 + threshold2) / 2.0F);
        }

        public float Triangle(AlgoImage srcImage, AlgoImage destImage = null, AlgoImage maskImage = null)
        {
            byte[] imageData = srcImage.GetByte();
            byte[] maskData = null;

            if (maskImage != null)
                maskData = maskImage.GetByte();

            int pitch = 4 * ((int)(Math.Truncate(((float)srcImage.Width - (float)1) / (float)4)) + 1);

            float[] histogram = new float[256];

            if (maskData != null)
            {
                for (int j = 0; j < srcImage.Height; j++)
                {
                    for (int i = 0; i < srcImage.Width; i++)
                    {
                        int index = (j * srcImage.Width) + i;

                        if (maskData[index] > 0)
                        {
                            histogram[imageData[index]]++;
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < srcImage.Height; j++)
                {
                    for (int i = 0; i < srcImage.Width; i++)
                    {
                        if (imageData[(j * srcImage.Width) + i] > 0)
                            histogram[imageData[(j * srcImage.Width) + i]]++;
                    }
                }
            }

            int total = 0;

            for (int i = 0; i < 256; i++)
            {
                total += (int)histogram[i];
            }

            float thresholdValue = Triangle(histogram);

            if (destImage != null)
            {
                if (maskData != null)
                {
                    for (int j = 0; j < srcImage.Height; j++)
                    {
                        for (int i = 0; i < srcImage.Width; i++)
                        {
                            int index = (j * srcImage.Width) + i;

                            if (maskData[index] > 0)
                            {
                                if (imageData[index] >= thresholdValue)
                                    imageData[index] = 255;
                                else
                                    imageData[index] = 0;
                            }
                            else
                            {
                                imageData[index] = 0;
                            }
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < srcImage.Height; j++)
                    {
                        for (int i = 0; i < srcImage.Width; i++)
                        {
                            int index = (j * srcImage.Width) + i;

                            if (imageData[index] >= thresholdValue)
                                imageData[index] = 255;
                            else
                                imageData[index] = 0;

                            index++;
                        }
                    }
                }

                destImage.SetByte(imageData);
            }

            return thresholdValue;
        }

        public float Triangle(float[] histogram)
        {
            int min = 0, max = 0, min2 = 0;
            float dmax = 0;

            for (int i = 0; i < histogram.Length; i++)
            {
                if (histogram[i] > 0)
                {
                    min = i;
                    break;
                }
            }
            if (min > 0) min--; // line to the (p==0) point, not to data[min]

            for (int i = 255; i > 0; i--)
            {
                if (histogram[i] > 0)
                {
                    min2 = i;
                    break;
                }
            }
            if (min2 < 255) min2++; // line to the (p==0) point, not to data[min]

            for (int i = 0; i < 256; i++)
            {
                if (histogram[i] > dmax)
                {
                    max = i;
                    dmax = histogram[i];
                }
            }
            // find which is the furthest side
            //IJ.log(""+min+" "+max+" "+min2);
            bool inverted = false;
            if ((max - min) < (min2 - max))
            {
                // reverse the histogram
                //IJ.log("Reversing histogram.");
                inverted = true;
                int left = 0;          // index of leftmost element
                int right = 255; // index of rightmost element
                while (left < right)
                {
                    // exchange the left and right elements
                    float temp = histogram[left];
                    histogram[left] = histogram[right];
                    histogram[right] = temp;
                    // move the bounds toward the center
                    left++;
                    right--;
                }
                min = 255 - min2;
                max = 255 - max;
            }

            if (min == max)
            {
                //IJ.log("Triangle:  min == max.");
                return min;
            }

            // describe line by nx * x + ny * y - d = 0
            double nx, ny, d;
            // nx is just the max frequency as the other point has freq=0
            nx = histogram[max];   //-min; // data[min]; //  lowest value bmin = (p=0)% in the image
            ny = min - max;
            d = Math.Sqrt(nx * nx + ny * ny);
            nx /= d;
            ny /= d;
            d = nx * min + ny * histogram[min];

            // find split point
            int split = min;
            double splitDistance = 0;
            for (int i = min + 1; i <= max; i++)
            {
                double newDistance = nx * i + ny * histogram[i] - d;
                if (newDistance > splitDistance)
                {
                    split = i;
                    splitDistance = newDistance;
                }
            }
            split--;

            if (inverted)
            {
                // The histogram might be used for something else, so let's reverse it back
                int left = 0;
                int right = 255;
                while (left < right)
                {
                    float temp = histogram[left];
                    histogram[left] = histogram[right];
                    histogram[right] = temp;
                    left++;
                    right--;
                }
                return (255 - split);
            }
            else
                return split;
        }

        public abstract void Sobel(AlgoImage algoImage, int size = 3);
        public abstract void Sobel(AlgoImage srcAlgoImage, AlgoImage dstAlgoImage);
        public abstract void Sobel(AlgoImage srcAlgoImage, AlgoImage dstAlgoImage, Direction direction);

        public abstract void And(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage);
        public abstract void Or(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage);
        public abstract void XOr(AlgoImage algoImage1, AlgoImage algoImage2, AlgoImage destImage);

        public void Not(AlgoImage algoImage)
        {
            Not(algoImage, algoImage);
        }
        public abstract void Not(AlgoImage algoImage, AlgoImage destImage);
        public abstract void Clear(AlgoImage image, byte value);

        public void Clear(AlgoImage image, Color value)
        {
            Clear(image, Rectangle.Empty, value);
        }

        public abstract void Clear(AlgoImage image, Rectangle rect, Color value);
        public abstract void Clear(AlgoImage image, AlgoImage mask, Color value);

        public abstract void Canny(AlgoImage greyImage, double threshold, double thresholdLinking);

        public abstract int Projection(AlgoImage greyImage, ref float[] projData, Direction projectionDir, ProjectionType projectionType = ProjectionType.Mean);
        public abstract float[] Projection(AlgoImage greyImage, Direction projectionDir, ProjectionType projectionType = ProjectionType.Mean);
        public abstract float[] Projection(AlgoImage greyImage, AlgoImage maskImage, Direction projectionDir, ProjectionType projectionType = ProjectionType.Mean);
        public abstract float[] LineProfile(AlgoImage greyImage, Point src, Point dst);

        public abstract void LogPolar(AlgoImage greyImage);

        public abstract void AvgStdDevXy(AlgoImage greyImage, AlgoImage maskImage, out float[] avgX, out float[] stdDevX, out float[] avgY, out float[] stdDevY);

        public abstract BlobRectList Blob(AlgoImage algoImage, BlobParam blobParam, AlgoImage greyMask = null);

        public abstract void FilterBlob(BlobRectList blobRectList, BlobParam blobParam);

        public abstract void DrawBlob(AlgoImage algoImage, BlobRectList blobRectList, BlobRect[] blobRects, DrawBlobOption drawBlobOption);

        public abstract void FillHoles(AlgoImage srcImage, AlgoImage destImage);
        public abstract void FindHoles(AlgoImage srcImage, AlgoImage destImage);

        public abstract double CodeTest(AlgoImage algoImage1, AlgoImage algoImage2, int[] intParams, double[] dblParams);

        public void Average(AlgoImage srcImage) { Average(srcImage, srcImage); }
        public abstract void Average(AlgoImage srcImage, AlgoImage destImage);

        public void Level(AlgoImage image) { Level(image, image); }
        public abstract void Level(AlgoImage srcImage, AlgoImage destImage);

        public abstract long[] Histogram(AlgoImage algoImage); //ms

        public abstract void HistogramEqualization(AlgoImage algoImage);
        public abstract void HistogramStretch(AlgoImage algoImage);

        public abstract void WeightedAdd(AlgoImage[] srcImages, AlgoImage dstImage);
        public abstract void Add(AlgoImage image1, AlgoImage image2, AlgoImage destImage);

        public abstract void Compare(AlgoImage image1, AlgoImage image2, AlgoImage destImage);
        public abstract void Add(AlgoImage srcImage, AlgoImage dstImage, int value);
        public abstract void Add(AlgoImage srcImage, AlgoImage dstImage, AlgoImage maskImage, int value);
        public abstract void Subtract(AlgoImage srcImage, AlgoImage dstImage, int value);
        public abstract void Mul(AlgoImage srcImage, AlgoImage dstImage, float value);
        public abstract void MulMultiple(AlgoImage srcImage, AlgoImage dstImage, float mul, float sum, float div);
        public abstract void Div(AlgoImage srcImage, AlgoImage dstImage, float value);
        public abstract void Subtract(AlgoImage image1, AlgoImage image2, AlgoImage destImage, bool isAbs = false);

        public abstract void BinarizeHistogram(AlgoImage srcImage, AlgoImage destImage, int percent);

        public abstract void Median(AlgoImage srcImage, AlgoImage destImage, int size);

        public abstract void Translate(AlgoImage srcImage, AlgoImage destImage, Point offset);

        public abstract void Match(AlgoImage srcImage, AlgoImage template, AlgoImage score);
        public abstract void Test(params object[] arguments);

        //Draw Function
        public abstract void DrawRect(AlgoImage srcImage, Rectangle rectangle, double value, bool filled);
        public abstract void DrawRotateRact(AlgoImage srcImage, RotatedRect rotateRect, double value, bool filled);
        public abstract void DrawArc(AlgoImage srcImage, ArcEq arcEq, double value, bool filled);
        public abstract void DrawPolygon(AlgoImage srcImage, float[] xArray, float[] yArray, double value, bool filled);
        public abstract void DrawText(AlgoImage srcImage, Point pointLT, double value, string text, double size = 1);

        public abstract void EraseBoder(AlgoImage srcImage, AlgoImage destImage);
        public abstract void ReconstructIncludeBlob(AlgoImage srcImage, AlgoImage destImage, AlgoImage seedImage, ResconstructParam resconstructParam);

        public abstract void EdgeDetect(AlgoImage srcImage, AlgoImage destImage, AlgoImage maskImage = null, double scaleFactor = 1);

        public abstract void Resize(AlgoImage srcImage, AlgoImage destImage, double scaleFactorX, double scaleFactorY);
        public void Resize(AlgoImage srcImage, AlgoImage destImage, double scaleFactor = -1)
        {
            this.Resize(srcImage, destImage, scaleFactor, scaleFactor);
        }

        public abstract void Stitch(AlgoImage srcImage1, AlgoImage srcImage2, AlgoImage destImage, Direction direction);

        public abstract void Min(AlgoImage image1, AlgoImage image2, AlgoImage destImage);
        public abstract void Max(AlgoImage image1, AlgoImage image2, AlgoImage destImage);

        public abstract float[,] FFT(AlgoImage srcImage);
        public abstract void Rotate(AlgoImage srcImage, AlgoImage destImage, float angle);
        public abstract void CustomEdge(AlgoImage srcImage, AlgoImage destImage, AlgoImage bufferX, AlgoImage bufferY, int length);

        public abstract BlobRectList BlobMerge(BlobRectList blobRectList1, BlobRectList blobRectList2, BlobParam blobParam);
        public abstract BlobRectList EreseBorderBlobs(BlobRectList blobRectList, BlobParam blobParam);
    }
}
