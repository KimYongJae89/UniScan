using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanWPF.Screen.PinHoleColor.Inspect
{
    public enum SearchDireciton
    {
        LeftToRight, RightToLeft
    }

    public class EdgePositionFinder
    {
        public static double[] SheetEdgePosition(AlgoImage algoImage, double[] thresholdArray, out float[] edgeValueArray, SearchDireciton searchDirection = SearchDireciton.LeftToRight)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            float[] sheetProfile = imageProcessing.Projection(algoImage, Direction.Horizontal, ProjectionType.Mean);

            if (searchDirection == SearchDireciton.RightToLeft)
                Array.Reverse(sheetProfile);

            int[] raw1D = new int[sheetProfile.Length];

            for (int index = 0; index < sheetProfile.Length; index++)
                raw1D[index] = Convert.ToInt32(sheetProfile[index]);

            DilatedDiff(raw1D, 30, out int[] diff);

            FindEdgeIndex(diff, thresholdArray, out double[] edgeIndex, out edgeValueArray);

            return edgeIndex;
        }

        private static void DilatedDiff(int[] src, int kernelSize, out int[] dst)
        {
            dst = new int[src.Length];
            int bound = src.Length - kernelSize;
            for (int i = kernelSize; i < bound; ++i)
                dst[i] = src[i - kernelSize] - src[i + kernelSize];
        }

        private static void FindEdgeIndex(int[] src, double[] thres, out double[] indices, out float[] edgeValueArray)
        {
            indices = new double[thres.Length];
            edgeValueArray = new float[thres.Length];

            const int dilatedSize = 20;
            int dilatedIndex = 0;
            int edgeIndexSize = thres.Length;
            int edgeIndex = 0;
            bool begin = false;
            int beginIndex = 0;
            for (int i = 0; i < src.Length; ++i)
            {
                if (src[i] > thres[edgeIndex])
                {
                    if (!begin)
                    {
                        begin = true;
                        beginIndex = i;
                    }
                    else
                    {
                        dilatedIndex++;
                    }
                }
                else
                {
                    if (begin)
                    {
                        if (dilatedIndex < dilatedSize)
                        {
                            begin = false;
                        }
                        else
                        {
                            indices[edgeIndex] = (beginIndex + i - dilatedSize) / 2;
                            edgeValueArray[edgeIndex] = (float)src.Skip(beginIndex).Take(dilatedSize).Average();
                            dilatedIndex = 0;
                            begin = false;
                            edgeIndex++;
                            if (edgeIndex >= edgeIndexSize) break;
                            if (thres[edgeIndex] < thres[edgeIndex - 1])
                            {
                                int halfThres = (int)(thres[edgeIndex] / 2);
                                while (i < src.Length)
                                {
                                    if (src[i] < halfThres) break;
                                    i++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
