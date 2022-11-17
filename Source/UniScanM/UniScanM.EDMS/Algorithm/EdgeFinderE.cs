using DynMvp.Vision;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using System.IO;
using System;
using System.Diagnostics;

namespace UniScanM.EDMS.Algorithm
{
    public enum SearchDireciton
    {
        LeftToRight, RightToLeft
    }

    public static class EdgeFinderE
    {
       public static double[] SheetEdgePosition(float[] sheetProfile, double[] thresholdArray, SearchDireciton searchDirection, int refSize = 0)
        {
#if DEBUG
            string pf = String.Join("\n", sheetProfile.Select(p => p.ToString()).ToArray());
            Debug.WriteLine(pf);
            System.IO.File.WriteAllText(@"D:\001_pf.csv", pf);
#endif
            if (searchDirection == SearchDireciton.RightToLeft)
                Array.Reverse(sheetProfile);

            //int[] raw1D = new int[sheetProfile.Length];
            //for (int index = 0; index < sheetProfile.Length; index++)
            //{
            //    raw1D[index] = Convert.ToInt32(sheetProfile[index]);
            //}

            float[] diffdata= DilatedDiff(sheetProfile, 30 );

            AverageFilter(diffdata, 10);

            for (int index = 0; index < sheetProfile.Length; index++)
            {
                sheetProfile[index] = diffdata[index];
            }
#if DEBUG
            string pfDiff = String.Join("\n", diffdata.Select(p => p.ToString()).ToArray());
            Debug.WriteLine(pfDiff);
            System.IO.File.WriteAllText(@"D:\002_pfDiff.csv", pfDiff);

            //string pfsheet = String.Join("\n", sheetProfile.Select(p => p.ToString()).ToArray());
            //Debug.WriteLine(pfsheet);
            //System.IO.File.WriteAllText(@"D:\003_pfsheet.csv", pfsheet);
#endif

            var minSize = 0;
            if (refSize != 0)
                minSize = (int)(refSize * 0.85);
            double[] Edges = new double[3];
            FindEdgeIndex(diffdata, thresholdArray, Edges, minSize);
            Debug.WriteLine("▶"+ String.Join(",", Edges.Select(p => p.ToString()).ToArray()));
            //return Edges;
            for (int i=0; i<3; i++)
            {
                if (Edges[i] <= 0)
                    continue;
                if(double.IsNaN(Edges[i]) ==true)
                {
                    Edges[i] = 0;
                    continue;
                }
                Edges[i] = GetWeightPosition(diffdata, (int)(Edges[i] + 0.5), thresholdArray[i]);
                Edges[i] = Edges[i] < 0 ? 0 : Edges[i];
                Edges[i] = Edges[i] > diffdata.Length ? 0 : Edges[i];
                if (Edges[i] <= 0)
                    continue;
                if (double.IsNaN(Edges[i]) == true)
                {
                    Edges[i] = 0;
                    continue;
                }
            }

            Debug.WriteLine("▷"+ String.Join(",", Edges.Select(p => p.ToString()).ToArray()));

            return Edges;
        }

        static float GetWeightPosition(float[] data, int center, double threshold)
        {
            float weightPos = 0.0f;
            int begin = 0;
            int end = 0;
            int i = 0;
            int maxLength = data.Length;

            //시작점 찾기
            i = center;
            while (data[i] > threshold)
            {
                i--;
                if (i < 0) break;
            }
            begin = i ;
            
            //끝점찾기
            i = center;
            while (data[i] > threshold)
            {
                i++;
                if(i>=maxLength) break;
            }
            end = i ;

            //무게 중심.
            double sum = 0;
            double nsum = 0;
            for( i = begin; i<=end; i++)
            {
                sum += data[i] * i;
                nsum += data[i];
            }

            weightPos = (float)(sum / nsum);

            return weightPos;
        }

        static void AverageFilter(float[] src, int kernelRadius)
        {
            float[] dst = new float[src.Length];
            dst.Initialize();

            float fsum = 0;

            int kernelWidth = kernelRadius * 2 + 1;
            for (int i=0; i< kernelWidth; i++)
            {
                fsum += src[i];
            }
            
            for(int i= kernelRadius; i< src.Length - kernelRadius - 1; i++)
            {
                src[i] = fsum / kernelWidth;
                fsum = fsum - src[i - kernelRadius] + src[i + kernelRadius];
            }
        }
        static float []  DilatedDiff(float[] src, int kernelSize)
        {
            float[] dst = new float[src.Length];
            int bound = src.Length - kernelSize;
            for (int i = kernelSize; i < bound; ++i)
            {
                dst[i] = src[i - kernelSize] - src[i + kernelSize];
            }
            return dst;
        }
       
        static void FindEdgeIndex(float[] src, double[] thres,  double[] Edges, int minSize)
        {
            const int dilatedSize = 20;
            int dilatedIndex = 0;
            int edgeIndexSize = thres.Length;
            int edgeIndex = 0;
            bool begin = false;
            int beginIndex = 0;
            for (int i = 0; i < src.Length; ++i)
            {
                if(thres[edgeIndex] <=0) //문턱값이 0으로 선택되면 써치안하고 스킵...
                {
                    Edges[edgeIndex] = -1;
                    edgeIndex++;
                    if (edgeIndex == 3) break;
                    continue;
                }
                if (src[i] > thres[edgeIndex])//문턱값을 넘는 라이징 엣지 위치 기억...
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
                else  //falling edge  // 일정사이즈 이하 버림..
                {
                    if (begin) //rising edge 가 검출되었고..
                    {
                        if (dilatedIndex < dilatedSize)
                        {
                            dilatedIndex++;
                        }
                        else
                        {
                            Edges[edgeIndex] = (beginIndex + i - dilatedSize) / 2;//

                            if (edgeIndex == 2 && minSize > (Edges[edgeIndex] - Edges[0]))
                            {
                                begin = false;//무시
                            }
                            else
                            {
                                dilatedIndex = 0;
                                begin = false;
                                edgeIndex++;
                                if (edgeIndex >= edgeIndexSize) break;
                                if (thres[edgeIndex] < thres[edgeIndex - 1])
                                {
                                    int halfThres = (int)(thres[edgeIndex] / 2); //ㅎㅎ
                                    while (i < src.Length)
                                    {
                                        if (src[i] < halfThres) break;
                                        i++;
                                    }
                                }
                            }
                        }
                    }//if (begin) //rising edge 가 검출되었고..
                }
            }
        }
    }
}
