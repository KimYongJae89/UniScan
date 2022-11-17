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
using System.Collections;



namespace UniScanM.EDMSW.Algorithm
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
            //Debug.WriteLine(pf);
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
            //Debug.WriteLine(pfDiff);
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

            if (searchDirection == SearchDireciton.RightToLeft)
                Array.Reverse(sheetProfile);

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

        public static List<Point> FindBlackEdges(float []src, int mergeRadius, int expectSize)
        {
            const byte __beginEdge = 100;  //falling 인쇄시작
            const byte __endEdge = 200;     //rising 인쇄 끝
            List<Point> findEdges = new List<Point>();

            var fmax = src.Max();
            var fmin = src.Min();
            var fdiff = fmax - fmin;

            int [] histo = new int[256];
            histo.Initialize();
            //make histo
            for(int i=0; i<src.Length; i++)
            {
                int index =(int )((src[i] - fmin)/fdiff * 255 );
                histo[index]++;
            }
            var threshold8=Li( histo);


            byte[] binary = new byte[src.Length];
            byte[] edges = new byte[src.Length];
            edges.Initialize();

            BitArray filterPrevbinary = new BitArray(src.Length,false);
            BitArray filterNextbinary = new BitArray(src.Length,false);
            //binary
            float threshold32 = threshold8/255 * fdiff + fmin;
            for (int i = 0; i < src.Length; i++)
            {
                binary[i] = (src[i] < threshold32) ?  (byte)0 : (byte)255;
            }

            bool value = false; //인쇄된게 검정,0,false //인쇄안된게 흰색,0,true
            //Prev///////////////////////////////////////////////////////////////////
            for (int i = 0; i < mergeRadius; i++)
            {
                if (binary[i] == 0)//인쇄 되면0, 그냥 흰색이면 255
                {
                    value = false;
                    break;
                }
                else value = true;
            }
            for (int i = 0; i < mergeRadius; i++)
                filterPrevbinary[i] = value;
            for (int i = mergeRadius; i < src.Length ; i++)
            {
                for(int j=1; j <= mergeRadius; j++)
                {
                    if (binary[i - j] == 0)//인쇄 되면0, 그냥 흰색이면 255
                    {
                        value = false;
                        break;
                    }
                    else value = true;
                }
                filterPrevbinary[i] = value;
            }
            //next ///////////////////////////////////////////////////////////////////
            for (int i = src.Length - mergeRadius; i < src.Length ; i++)
            {
                if (binary[i] == 0)//인쇄 되면0, 그냥 흰색이면 255
                {
                    value = false;
                    break;
                }
                else value = true;
            }

            for (int i = src.Length - mergeRadius; i < src.Length; i++)
                filterNextbinary[i] = value;

            for (int i = 0; i < src.Length - mergeRadius; i++)
            {
                for (int j = 1; j <= mergeRadius; j++)
                {
                    if (binary[i + j] == 0)//인쇄 된거이면.
                    {
                        value = false;
                        break;
                    }
                    else value = true;
                }
                filterNextbinary[i] = value;
            }
            ////////////////////////////////////////////////////////////////////////////
            filterPrevbinary.Or(filterNextbinary); //전후에 false(인쇄) 영역 확장

            //find edge 
            for (int i = 0 ; i < src.Length -1; i++)
            {
                if (filterPrevbinary[i] == filterPrevbinary[i+1]) continue;
                else //엣지이면
                {
                    if (filterPrevbinary[i] == false)  //인쇄 끝점, 
                        edges[i] = __endEdge; 
                    else edges[i] = __beginEdge;//인쇄 시작점
                    Point pt = new Point(edges[i], i);
                    findEdges.Add(pt);
                }
            }

#if DEBUG
            string pf1 = String.Join("\n", src.Select(p => p.ToString()).ToArray());
            System.IO.File.WriteAllText(@"D:\001.csv", pf1);
            //string pf2 = String.Join("\n", binary.Select(p => p.ToString()).ToArray());
            //System.IO.File.WriteAllText(@"D:\002.csv", pf2);


            //for (int i = 0; i < src.Length ; i++)
            //    binary[i] = filterPrevbinary[i] ? (byte)200 : (byte)100;
            ////Debug.WriteLine(pf);
            //string pf3 = String.Join("\n", binary.Select(p => p.ToString()).ToArray());
            //System.IO.File.WriteAllText(@"D:\003.csv", pf3);

            //for (int i = 0; i < src.Length; i++)
            //    binary[i] = filterNextbinary[i] ? (byte)250 : (byte)150;
            ////Debug.WriteLine(pf);
            //string pf4 = String.Join("\n", binary.Select(p => p.ToString()).ToArray());
            //System.IO.File.WriteAllText(@"D:\004.csv", pf4);


            //string data = "";
            //for (int i = 0; i < src.Length; i++)
            //{
            //    data += src[i].ToString(); data += ",";
            //    data += binary[i].ToString(); data += ",";
            //    data += filterPrevbinary[i] ? "200" : "100"; data += ",";
            //    data += filterNextbinary[i] ? "250" : "150"; data += "\n";
            //}
            //System.IO.File.WriteAllText(@"D:\FindEdges.csv", data);

            //filterPrevbinary.And(filterNextbinary); //전후에 false(인쇄) 영역 확장

            //for (int i = 0; i < src.Length; i++)
            //    binary[i] = filterPrevbinary[i] ? (byte)50 : (byte)0;
            //string pf3 = String.Join("\n", binary.Select(p => p.ToString()).ToArray());
            //System.IO.File.WriteAllText(@"D:\OR.csv", pf3);

#endif

            return findEdges;
        }

        //지우기 먼저 후 병합
        public static List<Point> FindWhiteEdges(float[] src, int mergeSize, int removeSize)
        {
            const byte __beginEdge = 100;  //falling 흰색시작
            const byte __endEdge = 200;     //rising 흰색 끝
            List<Point> findEdges = new List<Point>();

            var fmax = src.Max();
            var fmin = src.Min();
            var fdiff = fmax - fmin;

            int[] histo = new int[256];
            histo.Initialize();
            //make histo
            for (int i = 0; i < src.Length; i++)
            {
                int index = (int)((src[i] - fmin) / fdiff * 255);
                histo[index]++;
            }
            var threshold8 = Li(histo);


            byte[] binary = new byte[src.Length];
            byte[] edges = new byte[src.Length];
            edges.Initialize();

            //binary
            float threshold32 = threshold8 / 255 * fdiff + fmin;
            for (int i = 0; i < src.Length; i++)
            {
                binary[i] = (src[i] < threshold32) ? (byte)0 : (byte)255;
            }

            //find 성형층 edge 
            int beginEdgeIndex = -1;
            for (int i = 0; i < src.Length - 1; i++)
            {
                if (binary[i] == binary[i + 1]) continue;
                else //엣지이면
                {
                    if (binary[i] == 255)  //성형 끝점, 
                        edges[i] = __endEdge;
                    else edges[i] = __beginEdge;//성형 시작점
                }

                if (edges[i] == __beginEdge)
                {
                    beginEdgeIndex = i;
                }
                if(beginEdgeIndex >= 0 && edges[i] == __endEdge)
                {
                    Point pt = new Point(beginEdgeIndex, i);
                    findEdges.Add(pt);
                    beginEdgeIndex = -1;
                }
            }

            findEdges.RemoveAll(f => Math.Abs(f.Y - f.X) < removeSize);
            
            // 가까운것끼리 합치기  //점프패턴이 운없게 2개 이상으로 분리된경우 대비, 서로 합치기
            for (int i = 0; i < findEdges.Count - 1; i++)
            {
                int dist = Math.Abs(findEdges[i + 1].X - findEdges[i].Y); 
                if (dist < mergeSize)
                {
                    findEdges[i] = new Point(findEdges[i].X, findEdges[i + 1].Y); //현재꺼 합쳐진걸로...다시 저장
                    findEdges.RemoveAt(i + 1); //다음꺼 삭제.
                    i--; //this is  important
                }
            }



#if DEBUG
            string pf1 = String.Join("\n", src.Select(p => p.ToString()).ToArray());
            System.IO.File.WriteAllText(@"D:\001.csv", pf1);
            //string pf2 = String.Join("\n", binary.Select(p => p.ToString()).ToArray());
            //System.IO.File.WriteAllText(@"D:\002.csv", pf2);


            //for (int i = 0; i < src.Length ; i++)
            //    binary[i] = filterPrevbinary[i] ? (byte)200 : (byte)100;
            ////Debug.WriteLine(pf);
            //string pf3 = String.Join("\n", binary.Select(p => p.ToString()).ToArray());
            //System.IO.File.WriteAllText(@"D:\003.csv", pf3);

            //for (int i = 0; i < src.Length; i++)
            //    binary[i] = filterNextbinary[i] ? (byte)250 : (byte)150;
            ////Debug.WriteLine(pf);
            //string pf4 = String.Join("\n", binary.Select(p => p.ToString()).ToArray());
            //System.IO.File.WriteAllText(@"D:\004.csv", pf4);


            //string data = "";
            //for (int i = 0; i < src.Length; i++)
            //{
            //    data += src[i].ToString(); data += ",";
            //    data += binary[i].ToString(); data += ",";
            //    data += filterPrevbinary[i] ? "200" : "100"; data += ",";
            //    data += filterNextbinary[i] ? "250" : "150"; data += "\n";
            //}
            //System.IO.File.WriteAllText(@"D:\FindEdges.csv", data);

            //filterPrevbinary.And(filterNextbinary); //전후에 false(인쇄) 영역 확장

            //for (int i = 0; i < src.Length; i++)
            //    binary[i] = filterPrevbinary[i] ? (byte)50 : (byte)0;
            //string pf3 = String.Join("\n", binary.Select(p => p.ToString()).ToArray());
            //System.IO.File.WriteAllText(@"D:\OR.csv", pf3);

#endif

            return findEdges;
        }

        static float Li(int[] histogram)
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
    }
}
