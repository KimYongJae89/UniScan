using DynMvp.Vision;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using System.IO;
using System;

namespace UniScanM.Algorithm
{                   
    public enum SearchDireciton
    {
        LeftToRight, RightToLeft
    }

    public class EdgePositionFinder
    {                      
        
        public EdgePositionFinder() 
        {

        }

        public float[] SheetProfile(AlgoImage algoImage, float multiplier = 1.3f)
        {
            float[] projection = new float[algoImage.Width];
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);
            DebugContext debugContext = new DebugContext(OperationSettings.Instance().SaveDebugImage, Path.Combine(PathSettings.Instance().Temp, "EdgePositonFinder"));

            try
            {   
                // Projection
                projection = imageProcessing.Projection(algoImage, Direction.Horizontal, ProjectionType.Mean);
            }
            finally
            { 
            }

            return projection;
        }

        public double[] SheetEdgePosition(AlgoImage algoImage, double[] thresholdArray, SearchDireciton searchDirection, int refSize = 0)
        {                             
            float[] sheetProfile = SheetProfile(algoImage);

            if(searchDirection == SearchDireciton.RightToLeft)
                Array.Reverse(sheetProfile);

            int[] raw1D = new int[sheetProfile.Length];
            for(int index = 0; index < sheetProfile.Length; index++)
            {
                raw1D[index] = Convert.ToInt32(sheetProfile[index]);
            }

            DilatedDiff(raw1D, 30, out int[] diff);

            var minSize = 0;
            if (refSize != 0)
                minSize = (int)(refSize * 0.85);

            FindEdgeIndex(diff, thresholdArray, out double[] edgeIndex, minSize);

            return edgeIndex;
        }

        private void DilatedDiff(int[] src, int kernelSize, out int[] dst)
        {
            dst = new int[src.Length];
            int bound = src.Length - kernelSize;
            for (int i = kernelSize; i < bound; ++i)
            {
                dst[i] = src[i - kernelSize] - src[i + kernelSize];
            }
        }

        private void FindEdgeIndex(int[] src, double[] thres, out double[] indices, int minSize)
        {
            indices = new double[thres.Length];

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
                        dilatedIndex = 0;
                    }
                }
                else
                {
                    if (begin)
                    {
                        if (dilatedIndex < dilatedSize)
                        {
                            dilatedIndex++;
                        }
                        else
                        {
                            indices[edgeIndex] = (beginIndex + i - dilatedSize) / 2;

                            if (edgeIndex == 2 && minSize > (indices[edgeIndex] - indices[0]))
                            {
                                begin = false;
                            }
                            else
                            {
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
}
