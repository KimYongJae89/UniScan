using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using UniScanX.MPAlignment.Data;
using System.Drawing;
using UniEye.Base.Settings;
using DynMvp.Vision.OpenCv;
using System.IO;
using System.Diagnostics;


namespace UniScanX.MPAlignment.Algo
{
    public class Peak
    {
        //단위 표시없는것은 모두 픽셀좌표임
        public float[] refData = null;
        public int peakPos = -1;
        public float peakValue = -1;
        public float peakPosCOM = -1; //Center of Mass, sub pixel 단위
        public float peakValueCOM = -1; //Center of Mass, sub pixel 단위

        public int peakPosMetric = -1; //um - 픽셀좌료를 환산함.

        public int beginThPos = -1;  //문턱값을 넘기기 시작한 시작점 (좌->우)
        public int endThPos = -1;    //문탁값 밑으로 내려가기 직전 끝점 (좌->우)

        public int beginZeroPos = -1; //검출된 Peak의 덩어리 y축 0을 지나는 시작 기점(x좌표), y값은 0보다 크거나 같다
        public int endZeroPos = -1; //검출된 Peak의 덩어리 y축 0을 지나는 끝 기점(x좌표), y값은 0보다 크거나 같다
        public void WriteCSV(StreamWriter sw)
        {
            sw.WriteLine(peakPos.ToString() + "," +
                  peakValue.ToString() + "," +
                  peakPosCOM.ToString() + "," +
                  peakValueCOM.ToString() + "," +
                  peakPosMetric.ToString() + "," +
                  beginThPos.ToString() + "," +
                  endThPos.ToString() + "," +
                  beginZeroPos.ToString() + "," +
                  endZeroPos.ToString()
                  );
        }
        public Peak Clone()
        {
            var clone = new Peak();
            clone.peakPos = this.peakPos;
            clone.peakValue = this.peakValue;
            clone.peakPosCOM = this.peakPosCOM;
            clone.peakValueCOM = this.peakValueCOM;
            clone.peakPosMetric = this.peakPosMetric;
            clone.beginThPos = this.beginThPos;
            clone.endThPos = this.endThPos;
            clone.beginZeroPos = this.beginZeroPos;
            clone.endZeroPos = this.endZeroPos;

            return clone;
        }
    }

    public class MarginEdgePair
    {
        public MarginEdgePair(Peak rising, Peak falling)
        {
            Left1stRising = rising;
            Right1stFalling = falling;
        }
        public MarginEdgePair(Peak Rising, Peak Falling, Peak secFaling, Peak secrRising)
        {
            Left1stRising = Rising;
            Right1stFalling = Falling;

            Left2ndFalling = secFaling;
            Right2ndRising = secrRising;
        }

        public Peak Left1stRising = null; //LEFT  전극엣지
        public Peak Right1stFalling = null; //Right 전극엣지

        public Peak Left2ndFalling = null; //LEFT 2도 엣지
        public Peak Right2ndRising = null; //Right 2도 엣지

        public int GetMagin() //pixel
        {
            if (Left1stRising == null || Right1stFalling == null)
                return -99999;
            return Math.Abs(Left1stRising.peakPos - Right1stFalling.peakPos);
        }

        public float GetOffset2nd() //pixel
        {
            if (isValid() == false)
                return float.NaN;

            return (Left2ndFalling.peakPos - Left1stRising.peakPos + Right2ndRising.peakPos - Right1stFalling.peakPos) / 2.0f;
        }

        public bool isValid()
        {
            if (Left1stRising != null && Right1stFalling != null && Left2ndFalling != null && Right2ndRising != null &&
                Left1stRising.peakPos > 0 && Right1stFalling.peakPos > 0 &&
                Left2ndFalling.peakPos > 0 && Right2ndRising.peakPos > 0)
                return true;
            else return false;
        }
        public string GetString()
        {
            string str;
            str = makeString(Left1stRising) + "," +
                makeString(Left2ndFalling) + "," +
                makeString(Right1stFalling) + "," +
                makeString(Right2ndRising);

            string makeString(Peak peak)
            {
                if (peak == null)
                    return string.Format("(null)");
                else
                    return string.Format("({0})", peak.peakPos);
            }
            return str;
        }
    }

    public class MPAlgorithm : DynMvp.Vision.Algorithm
    {
        public static string TypeName => "MPAlignment";

        public MPAlgorithm()
        {
            this.param = new MPAlgorithmParam();
        }
        private MPAlgorithmParam GetParam()
        {
            return this.param as MPAlgorithmParam;
        }
        public override AlgorithmInspectParam CreateAlgorithmInspectParam(ImageD clipImage, RotatedRect probeRegionInFov, RotatedRect clipRegionInFov, Size wholeImageSize, Calibration calibration, DebugContext debugContext)
        {
            return new AlgorithmInspectParam(clipImage, probeRegionInFov, clipRegionInFov, wholeImageSize, calibration, debugContext);
        }

        public override AlgorithmResult CreateAlgorithmResult()
        {
            return new MPAlgorithmResult();
        }

        public override void AdjustInspRegion(ref RotatedRect inspRegion, ref bool useWholeImage)
        {
            useWholeImage = true;
            return;
        }

        public override void AppendAdditionalFigures(FigureGroup figureGroup, RotatedRect region)
        {
            throw new NotImplementedException();
        }

        public override DynMvp.Vision.Algorithm Clone()
        {
            throw new NotImplementedException();
        }

        public override string GetAlgorithmType()
        {
            return TypeName;
        }

        public override string GetAlgorithmTypeShort()
        {
            return TypeName;
        }

        public override List<AlgorithmResultValue> GetResultValues()
        {
            return new List<AlgorithmResultValue>();
        }

        public override AlgorithmResult Inspect(AlgorithmInspectParam algorithmInspectParam)
        {
            var imageIR = algorithmInspectParam.ClipImage;
            var imageBlue = algorithmInspectParam.ClipImage2;
            MPAlgorithmResult algorithmResult = this.CreateAlgorithmResult() as MPAlgorithmResult;
            //var algoImage = ImageBuilder.Build(OperationSettings.Instance().ImagingLibrary, imageD, ImageType.Grey);
            var algoblue = ImageBuilder.Build(ImagingLibrary.OpenCv, imageBlue, ImageType.Grey);
            var algoIR = ImageBuilder.Build(ImagingLibrary.OpenCv, imageIR, ImageType.Grey);
            OpenCvImageProcessing ipc = AlgorithmBuilder.GetImageProcessing(algoblue) as OpenCvImageProcessing;

            float[] EdgeBlueX;
            float[] EdgeBlueY;
            //electrode 먼저 Pair 찾고 => 여긴 100% 찾아져야하는 위치.. 사전에 이미지 자체의 에러를 찾아야하나?
            ipc.SobelEdgeProjection(algoblue, out EdgeBlueX, out EdgeBlueY);

            //X
            var ElecXPairList = PairingPeakElectrode(ref EdgeBlueX, 
                GetParam().ThresholdX,
                GetParam().HysterisisX  );
            Debug.WriteLine("ElecXPairList");
            DebugList(ElecXPairList);
            //Y
            var ElecYPairList = PairingPeakElectrode(ref EdgeBlueY,
                GetParam().ThresholdX,
                GetParam().HysterisisX);
            Debug.WriteLine("ElecYPairList");
            DebugList(ElecYPairList);

            //2도 인쇄랑 매칭.
            float[] EdgeX;
            float[] EdgeY;
            ipc.SobelEdgeProjection(algoIR, out EdgeX, out EdgeY);

            algorithmResult.EdgeProfileX = EdgeX;
            algorithmResult.EdgeProfileY = EdgeY;

            var XPairList = PairingPeak4(ref ElecXPairList, ref EdgeX, GetParam().ThresholdX,
                GetParam().ThresholdX_2nd,
                GetParam().HysterisisX);

            Debug.WriteLine("XPairList");
            DebugList(XPairList);

            var YPairList = PairingPeak4(ref ElecYPairList, ref EdgeY, GetParam().ThresholdY,
                GetParam().ThresholdY_2nd,
                GetParam().HysterisisY);
            Debug.WriteLine("YPairList");
            DebugList(YPairList);


            algorithmResult.XLinePairList = XPairList;
            algorithmResult.YLinePairList = YPairList;
            algorithmResult.AlgorithmParam = GetParam();

            return algorithmResult;
        }

        private List<MarginEdgePair> PairingPeakElectrode(ref float[] EdgeProfile, float Threshod1st, float hysterisis)
        {
            //업엣지 부터 시작 => 고로 초반에 다운엣지 나오면 쌩깜...
            var listAll = ExtractAllPeak(ref EdgeProfile, hysterisis);
            var listUpEdge = listAll.FindAll(x => x.peakValue > Threshod1st);
            listUpEdge.Sort((f,g)=> f.peakPos.CompareTo(g.peakPos)); //오름
            var listDownEdge = listAll.FindAll(x => x.peakValue < -Threshod1st);
            listDownEdge.Sort((f, g) => f.peakPos.CompareTo(g.peakPos)); //오름

            List<MarginEdgePair> pairList = new List<MarginEdgePair>();

            if (listUpEdge.Count > 0 && listDownEdge.Count > 0)
            {
                foreach (var upEdge in listUpEdge)
                {
                    var nextUpEdge = listUpEdge.Find(f => f.peakPos > upEdge.peakPos);
                    Peak downEdge=null ;
                    if (nextUpEdge != null)
                        downEdge = listDownEdge.Find(f => f.peakPos > upEdge.endZeroPos && f.peakPos < nextUpEdge.peakPos);
                    else
                        downEdge = listDownEdge.Find(f => f.peakPos > upEdge.endZeroPos);

                    if (downEdge != null)
                    {
                        MarginEdgePair pair = new MarginEdgePair(upEdge, downEdge);
                        pairList.Add(pair);
                    }
                }
            }   
            return pairList;
        }


        private List<MarginEdgePair> PairingPeak(ref float[] EdgeProfile, float Threshod1st, float Threshold2nd, bool force)
        {
            var listAll = ExtractAllPeak(ref EdgeProfile, 0.2f);
            //StreamWriter sw = new StreamWriter("d:\\peak_list.csv", false, Encoding.UTF8);
            //sw.WriteLine("peakPos, peakValue, peakPosCOM, peakValueCOM, peakPosMetric, beginThPos, endThPos, beginZeroPos, endZeroPos");
            //foreach (var peak in listAll)
            //    peak.WriteCSV(sw);
            //sw.Close();

            var listUpEdge = listAll.FindAll(x => x.peakValue > Threshod1st);
            //StreamWriter sw2 = new StreamWriter("d:\\Edge_peak_list.csv", false, Encoding.UTF8);
            //sw2.WriteLine("peakPos, peakValue, peakPosCOM, peakValueCOM, peakPosMetric, beginThPos, endThPos, beginZeroPos, endZeroPos");
            //foreach (var peak in listUpEdge)
            //    peak.WriteCSV(sw2);
            //sw2.Close();

            var listDownEdge = listAll.FindAll(x => x.peakValue < -Threshod1st);
            //pairing MajorEdge => 전극엣지
            List<MarginEdgePair> pairList = new List<MarginEdgePair>();
            if (listUpEdge.Count > 0 && listDownEdge.Count > 0)
            {
                foreach (var upEdge in listUpEdge)
                {
                    int begin = upEdge.endZeroPos;
                    //첫번째 다운이 2도가 밀려서 전극엣지 우측에 걸릴수도 있음. 어떻하지?
                    var downEdge = listDownEdge.Find(f => f.peakPos > begin);
                    if (downEdge != null)
                    {
                        MarginEdgePair pair = new MarginEdgePair(upEdge, downEdge);
                        pairList.Add(pair);
                    }
                }
            }

            ////pairing MajorEdge => 2도엣지
            if (pairList.Count > 0)
            {
                foreach (var pair in pairList)
                {
                    //2도인쇄 2nd left  ***************************************************//
                    int range_end = pair.Right1stFalling.beginZeroPos;
                    //int range_end = pair.Right1stFalling.peakPos;
                    int margin = pair.GetMagin();
                    int range_begin = pair.Left1stRising.beginZeroPos - margin;
                    range_begin = range_begin < 0 ? 0 : range_begin;

                    var listfalling = listAll.FindAll(f =>
                    f.peakValue < 0 &&  //마이너스 피크
                    f.peakPos < range_end &&
                    f.peakPos > range_begin);

                    //일단 후보군.
                    var list = listfalling.FindAll(f => Math.Abs(f.peakValue) > Threshold2nd);
                    list.Sort((f, g) => f.peakPos.CompareTo(g.peakPos)); //오름
                    if (list.Count > 0)
                    {   //찾아진 제일 낮은 피크
                        pair.Left2ndFalling = list[0];
                    }
                    else if (force)
                    {
                        //문턱값에 만족하는게 없을때. 검색영역의 제일 낮은 값
                        listfalling.Sort((f, g) => f.peakValue.CompareTo(g.peakValue)); //오름
                        if (listfalling.Count > 0)
                            pair.Left2ndFalling = listfalling[0];
                    }
                    //아직도 못찾았다면... 전극과 전극 사이에 있을 확률이 높다.
                    {
                        range_end = pair.Right1stFalling.beginZeroPos;
                        //int range_end = pair.Right1stFalling.peakPos;
                        margin = pair.GetMagin();
                        range_begin = pair.Left1stRising.endZeroPos;
                        var listRising1 = listAll.FindAll(f =>
                        f.peakValue > 0 &&  //플러스 피크
                        f.peakPos < range_end &&
                        f.peakPos > range_begin);
                        list = listRising1.FindAll(f => Math.Abs(f.peakValue) > Threshold2nd);
                        list.Sort((f, g) => g.peakPos.CompareTo(f.peakPos)); //내림
                        if (list.Count > 0)////찾아진 제일 높은 피크
                            pair.Left2ndFalling = list[0];
                    }
                    //여전히 못찾았다면, 없는것,=> 즉 전극엣지와 2도 엣지가 겹쳤음.


                    //else nothing to do
                    //2도 인쇄2nd right  ***************************************************//

                    margin = pair.GetMagin();
                    range_end = pair.Right1stFalling.endZeroPos + margin;
                    range_begin = pair.Left1stRising.endZeroPos;

                    var listRising = listAll.FindAll(f =>
                    f.peakValue > 0 &&  //플러스 피크
                    f.peakPos < range_end &&
                    f.peakPos > range_begin);

                    //일단 후보군.
                    list = listRising.FindAll(f => Math.Abs(f.peakValue) > Threshold2nd);
                    list.Sort((f, g) => g.peakPos.CompareTo(f.peakPos)); //내림
                    if (list.Count > 0)
                    {   //찾아진 제일 높은 피크
                        pair.Right2ndRising = list[0];
                    }
                    else if (force)
                    {
                        //문턱값에 만족하는게 없을때. 검색영역의  제일 높은값
                        listRising.Sort((f, g) => g.peakValue.CompareTo(f.peakValue)); //내림
                        if (listRising.Count > 0)
                            pair.Right2ndRising = listRising[0];
                    }
                }
            }
            while (pairList.Count > 1)//2개 이상
            {

                var previous = pairList[0];
                var next = pairList[0];
                int i = 0;
                for (i = 1; i < pairList.Count; i++)
                {
                    next = pairList[i];
                    if (previous.Right1stFalling == next.Left2ndFalling ||
                        previous.Right2ndRising == next.Left1stRising)
                    {
                        if (previous.GetMagin() > next.GetMagin())
                            pairList.Remove(previous);
                        else
                            pairList.Remove(next);

                        break;
                    }
                    previous = next;
                }
                if (i == pairList.Count)
                    break;
            }
            return pairList;
        }

        private List<MarginEdgePair> PairingPeak2(ref float[] EdgeProfile, float Threshod1st, float Threshold2nd, bool force)
        {
            var listAll = ExtractAllPeak(ref EdgeProfile, Threshod1st * 0.2f);
            var listUpEdge = listAll.FindAll(x => x.peakValue > Threshod1st);

            var listDownEdge = listAll.FindAll(x => x.peakValue < -Threshod1st);
            //pairing MajorEdge => 전극엣지
            List<MarginEdgePair> pairList = new List<MarginEdgePair>();
            if (listUpEdge.Count > 0 && listDownEdge.Count > 0)
            {
                foreach (var upEdge in listUpEdge)
                {
                    int begin = upEdge.endZeroPos;
                    //첫번째 다운이 2도가 밀려서 전극엣지 우측에 걸릴수도 있음. 어떻하지?=> 전극엣지 사이인 마진에 상승엣지로 걸릴수 있음
                    var downEdge = listDownEdge.Find(f => f.peakPos > begin);
                    if (downEdge != null)
                    {
                        MarginEdgePair pair = new MarginEdgePair(upEdge, downEdge);
                        pairList.Add(pair);
                    }
                }
            }

            ////pairing MajorEdge => 2도엣지
            if (pairList.Count > 0)
            {
                foreach (var pair in pairList)
                {
                    //2도인쇄 2nd left  ***************************************************//
                    int range_end = pair.Right1stFalling.beginZeroPos;
                    //int range_end = pair.Right1stFalling.peakPos;
                    int margin = pair.GetMagin();
                    int range_begin = pair.Left1stRising.beginZeroPos - margin;
                    range_begin = range_begin < 0 ? 0 : range_begin;

                    //일단 후보군.
                    //마이너스 폴링 리스트
                    var listfalling = FindPeakWithDir(listAll, range_begin, range_end, -Math.Abs(Threshold2nd));
                    if (listfalling.Count > 0)
                    {   //찾아진 제일 낮은 피크
                        pair.Left2ndFalling = listfalling[0];
                    }
                    else if (force)
                    {
                        ////문턱값에 만족하는게 없을때. 검색영역의 제일 낮은 값
                        //listfalling.Sort((f, g) => f.peakValue.CompareTo(g.peakValue)); //오름
                        //if (listfalling.Count > 0)
                        //    pair.Left2ndFalling = listfalling[0];
                    }
                    //아직도 못찾았다면... 전극과 전극 사이에 있을 확률이 높다.
                    if (pair.Right2ndRising == null)
                    {
                        range_begin = pair.Left1stRising.endZeroPos;
                        range_end = pair.Right1stFalling.beginZeroPos;
                        var listRising1 = FindPeakWithDir(listAll, range_begin, range_end, Math.Abs(Threshold2nd));
                        if (listRising1.Count > 0)
                            pair.Left2ndFalling = listRising1[0];
                    }
                    //여전히 못찾았다면, 없는것,=> 즉 전극엣지와 2도 엣지가 겹쳤음.


                    //else nothing to do
                    //2도 인쇄2nd right  ***************************************************//

                    margin = pair.GetMagin();
                    range_end = pair.Right1stFalling.endZeroPos + margin;
                    range_begin = pair.Left1stRising.endZeroPos;

                    //플러스 폴링 리스트
                    var listRising = FindPeakWithDir(listAll, range_begin, range_end, Math.Abs(Threshold2nd));
                    if (listRising.Count > 0)
                    {   //찾아진 제일 높은 피크
                        pair.Right2ndRising = listRising[0];
                    }
                    else if (force)
                    {
                        ////문턱값에 만족하는게 없을때. 검색영역의  제일 높은값
                        //listRising.Sort((f, g) => g.peakValue.CompareTo(f.peakValue)); //내림
                        //if (listRising.Count > 0)
                        //    pair.Right2ndRising = listRising[0];
                    }
                    //아직도 못찾았다면... 전극과 전극 사이에 있을 확률이 높다.
                    if (pair.Right2ndRising == null)
                    {
                        range_begin = pair.Right1stFalling.beginZeroPos;
                        range_end = pair.Left1stRising.endZeroPos;
                        var listFalling1 = FindPeakWithDir(listAll, range_begin, range_end, -Math.Abs(Threshold2nd));
                        if (listFalling1.Count > 0)
                            pair.Left2ndFalling = listFalling1[0];
                    }
                }
            }
            removeOverlapedPair(pairList, 200);
            return pairList;
        }

        private List<MarginEdgePair> PairingPeak3(ref float[] EdgeProfile, float Threshold1st, float Threshold2nd,
            float hysterisis, int marginPix = 250)
        {
            var listAll = ExtractAllPeak(ref EdgeProfile, hysterisis);
            var listUpEdge = listAll.FindAll(x => x.peakValue > Threshold1st);

            var listDownEdge = listAll.FindAll(x => x.peakValue < -Threshold1st);
            //pairing MajorEdge => 전극엣지
            List<MarginEdgePair> pairList = new List<MarginEdgePair>();
            if (listUpEdge.Count > 0 && listDownEdge.Count > 0)
            {
                foreach (var upEdge in listUpEdge)
                {
                    Peak marginLeft = upEdge, marginRight = null, secLeft = null, secRight = null;
                    List<Peak> groupList = new List<Peak>();
                    FindNearPeakAll(upEdge, listAll, groupList, marginPix, Threshold1st);
                    groupList.Add(upEdge);



                    //다운 엣지중 가장 오른쪽 (전극엣지) //이건 안나올수가 없으며, 안나왔으면 영상에서 짤린거임.(즉 나중에 무시)
                    var ListFalling = groupList.FindAll(f => f.peakValue < -Threshold1st);
                    if (ListFalling.Count > 0)
                    {
                        var list = ListFalling.FindAll(f => f.peakPos > upEdge.peakPos); //오른쪽
                        if (list.Count > 0)
                        {
                            list.Sort((f, g) => g.peakPos.CompareTo(f.peakPos)); //내림
                            marginRight = list[0];
                            groupList.Remove(list[0]);
                        }
                    }

                    //다운 엣지중 가장 왼쪽 (2도엣지 왼쪽)
                    ListFalling = groupList.FindAll(f => f.peakValue < -Threshold2nd);
                    if (ListFalling.Count > 0)
                    {
                        var list = ListFalling.FindAll(f => f.peakPos < upEdge.peakPos); //왼쪽
                        if (list.Count > 0)
                        {
                            list.Sort((f, g) => f.peakPos.CompareTo(g.peakPos)); //오름
                            secLeft = list[0];
                            groupList.Remove(list[0]);
                        }
                    }

                    //가장 왼쪽 업엣지 (마진엣지 왼쪽)
                    var ListRising = groupList.FindAll(f => f.peakValue > Threshold1st);
                    if (ListRising.Count > 0)
                    {
                        Peak origin;
                        if (marginRight != null) origin = marginRight;
                        else if (secRight != null) origin = secRight;
                        else origin = upEdge;
                        var list = ListRising.FindAll(f => f.peakPos < origin.peakPos); //왼쪽
                        if (list.Count > 0)
                        {
                            list.Sort((f, g) => f.peakPos.CompareTo(g.peakPos)); //오름
                            marginLeft = list[0];
                            groupList.Remove(list[0]);
                        }
                    }
                    //가장 오른쪽 업 엣지 (2도엣지 오른쪽)
                    ListRising = groupList.FindAll(f => f.peakValue > Threshold2nd);
                    if (ListRising.Count > 0)
                    {
                        var list = ListRising.FindAll(f => f.peakPos > upEdge.peakPos); //오른쪽
                        if (list.Count > 0)
                        {
                            list.Sort((f, g) => g.peakPos.CompareTo(f.peakPos)); //내림
                            secRight = list[0];
                            groupList.Remove(list[0]);
                        }
                    }
                    // 일반적으로 2도down ~ 전극up ~ 전극down ~ 2도up 이 정상적임
                    // up-down (rising- falling)은 반드시 한페어는 나옴 (전극끝, 마진 양쪽)
                    // 2도 엣지는 down-up 으로 페어가 되어야하는 OFFSET 이 너무커서 한쪽 엣지가 마진 사이에 위치할수 있음
                    //이때 극은 본래 가져야하는 극과 반대가 됨.
                    //
                    //(2도down 과 전극up) (전극down 과 2도up) 한쪽만 서로 겹치는 경우 있음.
                    //
                    //
                    //if( marginFalling==null) //이건 안나올수가 없으며, 안나왔으면 영상에서 짤린거임.(즉 나중에 무시)
                    if (marginRight == null || marginLeft == null) //뭔가 잘못됬네..
                        continue;
                    if (groupList.Count > 0) //아직도 남은게 있다면... 2도 엣지를 못찾은거임...
                    {
                        if (secLeft == null)//여전히 없다는 것은 //본래 다운엣지이지만 //마진 내에서 업엣지로 존재. 하거나 겹치거나
                        {
                            var ListRising2 = groupList.FindAll(f =>
                            f.peakValue > Threshold2nd &&
                            f.peakPos > marginLeft.peakPos &&
                            f.peakPos < marginRight.peakPos
                            );
                            if (ListRising2.Count > 0)
                            {
                                ListRising2.Sort((f, g) => g.peakValue.CompareTo(f.peakValue)); //내림
                                secLeft = ListRising2[0];
                                groupList.Remove(secLeft);
                            }
                        }
                        else if (secRight == null)//여전히 없다는 것은 //본래 업엣지이지만 //마진 내에서 다운 엣지로 존재. 하거나 겹치거나
                        {
                            var ListFalling2 = groupList.FindAll(f =>
                            f.peakValue < -Threshold2nd &&
                            f.peakPos > marginLeft.peakPos &&
                            f.peakPos < marginRight.peakPos
                            );
                            if (ListFalling2.Count > 0)
                            {
                                ListFalling2.Sort((f, g) => g.peakValue.CompareTo(f.peakValue)); //내림
                                secRight = ListFalling2[0];
                                groupList.Remove(secRight);
                            }
                        }
                        else //노이즈로 인해쓸데 없는 엣지가 추가 검출된거임..//이거 나중에 지워야될듯..
                        {
                            //add noise edge
                        }
                    }

                    // 아직도 남아있다? 노이즈로인해 엣지가 추가 검출된거 인데...그렇다면...이게 다른 엣지를 잘못찾게 만든것일수 있음
                    if (groupList.Count > 0)
                    {
                        var ListRising2 = groupList.FindAll(f =>
                        f.peakValue > Threshold1st &&
                        f.peakPos > marginLeft.peakPos &&
                        f.peakPos < marginRight.peakPos
                        );
                        if (ListRising2.Count > 0)
                        {
                            ListRising2.Sort((f, g) => g.peakValue.CompareTo(f.peakValue));
                            var ambiguous = ListRising2[0];
                            if (ambiguous.peakValue > marginLeft.peakValue) //신호가 강한것이 더 유효한것으로 판단.
                                marginLeft = ambiguous;
                        }

                        var ListFalling2 = groupList.FindAll(f =>
                        f.peakValue < -Threshold1st &&
                        f.peakPos > marginLeft.peakPos &&
                        f.peakPos < marginRight.peakPos
                        );

                        if (ListFalling2.Count > 0)
                        {
                            ListFalling2.Sort((f, g) => g.peakValue.CompareTo(f.peakValue));
                            var ambiguous = ListFalling2[0];
                            if (ambiguous.peakValue < marginRight.peakValue) //신호가 강한것이 더 유효한것으로 판단.
                                marginRight = ambiguous;
                        }
                    }
                    //여전히 못찾은게 있다면... 겹친거임
                    if (secLeft == null) secLeft = marginLeft;
                    if (secRight == null) secRight = marginRight;

                    MarginEdgePair pair = new MarginEdgePair(marginLeft, marginRight, secLeft, secRight);
                    pairList.Add(pair);
                }
            }


            removeOverlapedPair(pairList, marginPix);
            return pairList;
        }


        private List<MarginEdgePair> PairingPeak4(ref List<MarginEdgePair> ElectroPair ,
            ref float[] EdgeProfile, float Threshold1st, float Threshold2nd,
            float hysterisis)
        {
            
            var listAll = ExtractAllPeak(ref EdgeProfile, hysterisis);

            StreamWriter sw;
            string filepath = "d:\\peak_list.csv";
            try
            {
                sw = new StreamWriter(filepath, false, Encoding.UTF8);
            }
            catch(Exception e)
            {
                filepath = "d:\\peak_list__.csv";
                 sw = new StreamWriter(filepath, false, Encoding.UTF8);
            }
            
            sw.WriteLine("peakPos, peakValue, peakPosCOM, peakValueCOM, peakPosMetric, beginThPos, endThPos, beginZeroPos, endZeroPos");
            foreach (var peak in listAll)
                peak.WriteCSV(sw);
            sw.Close();

            var listUpEdge = listAll.FindAll(x => x.peakValue > Threshold1st);
            var listDownEdge = listAll.FindAll(x => x.peakValue < -Threshold1st);

            //pairing MajorEdge => 전극엣지
            List<MarginEdgePair> pairList = new List<MarginEdgePair>();
            if (listUpEdge.Count > 0 && listDownEdge.Count > 0)
            {
                foreach (var elecPair in ElectroPair)
                {
                    List<Peak> nearPeakList = new List<Peak>();
                    int marginPix = (int)( elecPair.GetMagin()*1.8f );

                    FindNearPeakAll(elecPair.Left1stRising , listAll, nearPeakList, marginPix, Threshold1st);
                    
                    void sortListNear(List<Peak> list, Peak peek)
                    {
                        list.Sort((f, g) =>
                        {
                            int df = Math.Abs(f.peakPos - peek.peakPos);
                            int dg = Math.Abs(g.peakPos - peek.peakPos);
                            return df.CompareTo(dg);
                        });
                    }

                    Peak marginLeft = elecPair.Left1stRising;
                    if (nearPeakList.Count > 0) //제일 가까운 피크 검색
                    {
                        sortListNear(nearPeakList, marginLeft);
                        marginLeft = nearPeakList[0];
                        nearPeakList.RemoveAt(0); 
                    }
                                                                          
                    Peak marginRight = elecPair.Right1stFalling;
                    if (nearPeakList.Count > 0) //제일 가까운 피크 검색
                    {
                        sortListNear(nearPeakList, marginRight);
                        marginRight = nearPeakList[0];
                        nearPeakList.RemoveAt(0);
                    }

                    Peak secLeft = null;
                    var templist=nearPeakList.FindAll(f => f.peakPos < marginLeft.peakPos); //왼쪽 2도의 마이너스 피크 검출
                    if (templist.Count > 0) //인근 제일 큰값
                    {
                        templist.Sort((f, g) => Math.Abs(g.peakValue).CompareTo(Math.Abs(f.peakValue)));
                        secLeft = templist[0];
                    }

                    Peak secRight = null;
                    templist = nearPeakList.FindAll(f => f.peakPos > marginRight.peakPos ); //오른쪽 2도의 플러스 피크 검출
                    if (templist.Count > 0)//인근 제일 큰값
                    {
                        templist.Sort((f, g) => Math.Abs(g.peakValue).CompareTo(Math.Abs(f.peakValue)));
                        secRight = templist[0];
                    }
                    //정상조건이면 여기까지 다검출,, 인쇄 오프셋이 더 크면 검출이 안되거나 하는 경우 아래 조건으로..

                    if(secRight !=null &&  secLeft ==null) //왼쪽 2도만 못찾음
                    { //방향 상관없이 마진엣지 사이의 피크 취함
                        templist = nearPeakList.FindAll(f => f.peakPos > marginLeft.peakPos &&f.peakPos < marginRight.peakPos);
                        if (templist.Count > 0)
                            secLeft = templist[0];
                    }
                    if (secLeft != null && secRight == null) //오른쪽 2도만 못찾음
                    {//방향 상관없이 마진엣지 사이의 피크 취함
                        templist = nearPeakList.FindAll(f => f.peakPos > marginLeft.peakPos && f.peakPos < marginRight.peakPos);
                        if (templist.Count > 0)
                            secRight = templist[0];
                    }

                    if(secLeft ==null && secRight ==null) //2도 양쪽 다 못찾음
                    { //피크 방향 바뀌어서  마진엣지 사이 피크로 2도 엣지 찾기... 결국 한개만 찾아질 가능성 높음.
                        templist = nearPeakList.FindAll(f => f.peakPos < marginLeft.peakPos && f.peakValue > 0); //왼쪽 2도의 마이너스 피크 검출
                        if (templist.Count > 0)
                            secLeft = templist[0];
                        templist = nearPeakList.FindAll(f => f.peakPos > marginRight.peakPos && f.peakValue < 0); //오른쪽 2도의 플러스 피크 검출
                        if (templist.Count > 0)
                            secRight = templist[0];
                    }

                    pairList.Add(new MarginEdgePair(marginLeft, marginRight, secLeft, secRight));
                }
            }
            return pairList;
        }


        void removeOverlapedPair(List<MarginEdgePair> pairList, int marginPix)
        {
            // 앞뒤 두개의 페어가 서로 공유되는것을 지음
            while (pairList.Count > 1)//2개 이상
            {
                var previous = pairList[0];
                var next = pairList[0];
                int i = 0;
                for (i = 1; i < pairList.Count; i++)
                {
                    next = pairList[i];
                    if (previous.Right1stFalling == next.Left2ndFalling ||
                        previous.Right2ndRising == next.Left1stRising)
                    {
                        int diffmarginPrev = Math.Abs(marginPix - previous.GetMagin());
                        int diffmarginNext = Math.Abs(marginPix - next.GetMagin());
                        if (diffmarginNext < diffmarginPrev)
                        {
                            pairList.Remove(previous);
                            previous = next;
                        }
                        else
                            pairList.Remove(next);

                        break;
                    }
                    if (previous.Right1stFalling == next.Right1stFalling &&
                        previous.Right2ndRising == next.Right2ndRising &&
                        previous.Left1stRising == next.Left1stRising &&
                         previous.Left2ndFalling == next.Left2ndFalling
                        )
                    {
                        pairList.Remove(next);
                        //previous = next;
                        break;
                    }
                    previous = next;
                }
                if (i == pairList.Count)
                    break;
            }

            //실제 한개의 페어에 노이즈나 2도 


        }


        //검색 영역안에 ± threshold 값의 리스트를 -+ 내림 오름 차순으로 정렬
        private List<Peak> FindPeakWithDir(List<Peak> srcList, int begin, int end, float threshold)
        {
            List<Peak> list = null;
            if (threshold > 0) //플러스 피크
            {
                list = srcList.FindAll(f =>
                            f.peakValue > threshold &&  //플러스 피크
                            f.peakPos < end &&
                            f.peakPos > begin);
                list.Sort((f, g) => g.peakValue.CompareTo(f.peakValue)); //내림 제일 큰값순
            }
            else //마이너스 피크
            {
                list = srcList.FindAll(f =>
                            f.peakValue < threshold &&  //마이너스 피크
                            f.peakPos < end &&
                            f.peakPos > begin);
                list.Sort((f, g) => f.peakValue.CompareTo(g.peakValue)); //오름 //제일 낮은값
            }
            return list;
        }
        //검색 영역안에 ± 모든 피크
        private List<Peak> FindPeakAll(List<Peak> srcList, int begin, int end, float threshold)
        {
            List<Peak> list = null;

            list = srcList.FindAll(f =>
                        Math.Abs(f.peakValue) > Math.Abs(threshold) &&  //모든 피크
                        f.peakPos < end &&
                        f.peakPos > begin);
            //list.Sort((f, g) => Math.Abs(g.peakValue).CompareTo(Math.Abs(f.peakValue))); //내림, 제일 절대값 큰값순
            return list;
        }

        private void FindNearPeakAll(Peak startingPt, List<Peak> srcList, List<Peak> dstList, int margin, float threshold)
        {
            List<Peak> list = null;
   
            //마이너스 방향으로 검색
            Peak stpt =startingPt.Clone();
            stpt.peakPos += 1; //현재 기점도 검색해야함.
            while (true)
            {
                int begin = stpt.peakPos - margin ;
                int end = stpt.peakPos ;// +margin;

                list = FindPeakAll(srcList, begin, end, threshold);
                if (list.Count > 0)
                {
                    list.Sort((f, g) => f.peakPos.CompareTo(g.peakPos)); 
                    stpt = list[0];
                    //if( dstList.IndexOf(stpt) <0)
                        dstList.AddRange(list);
                }
                else break;
            }
            //플러스 방향으로 검색
            stpt = startingPt;

            while (true)
            {
                int begin = stpt.peakPos;// - margin;
                int end = stpt.peakPos + margin ;

                list = FindPeakAll(srcList, begin, end, threshold);
                if (list.Count > 0)
                {
                    list.Sort((f, g) => g.peakPos.CompareTo(f.peakPos)); //sofla
                    stpt = list[0];
                    dstList.AddRange(list);
                }
                else break;
            }
        }

        private List<Peak> ExtractAllPeak(ref float[] datas) //x절편을 기점으로 상위 피크 하위 피크를 모두 추출
        {
            List<Peak> list = new List<Peak>();
            ///
            Peak peak = null;

            for (int i = 0; i < datas.Length; i++)
            {
                //플러스 rising  x절편
                if (datas[i] > 0) //rising
                {
                    peak = new Peak();
                    peak.refData = datas;
                    peak.beginZeroPos = i;

                    //falling 찾기
                    float maxValue = datas[i];
                    int maxIndex = i;
                    for (int j = i; j < datas.Length; j++)
                    {
                        if (datas[j] > maxValue) { maxValue = datas[j]; maxIndex = j; }
                        if (datas[j] < 0) //falling
                        {
                            peak.peakPos = maxIndex;
                            peak.peakValue = maxValue;
                            peak.endZeroPos = j - 1;
                            list.Add(peak);
                            i = j - 1; //건너뛰기
                            break;
                        }
                    }
                }
                else //마이너스 falling x 절편
                {
                    peak = new Peak();
                    peak.refData = datas;
                    peak.beginZeroPos = i;

                    //rising 찾기
                    float minValue = datas[i];
                    int minIndex = i;
                    for (int j = i; j < datas.Length; j++)
                    {
                        if (datas[j] < minValue) { minValue = datas[j]; minIndex = j; }
                        if (datas[j] > 0) //rising
                        {
                            peak.peakPos = minIndex;
                            peak.peakValue = minValue;
                            peak.endZeroPos = j - 1;
                            list.Add(peak);
                            i = j - 1; //건너뛰기
                            break;
                        }
                    }
                }
                //                
            }

            //영상의 시작점 끝점, 또는 페이가 맞지 않는 것 삭제
            int datalength = datas.Length;
            var removed = list.RemoveAll(f => f.beginZeroPos < 1 || f.endZeroPos < 1 ||
             f.endZeroPos >= datalength - 1);
            return list;
        }

        //Threshold = ± 문턱값으로 이값 내의 것은 무시.
        private List<Peak> ExtractAllPeak(ref float[] datas, float Hysterisis) //x절편을 기점으로 상위 피크 하위 피크를 모두 추출
        {
            float plusTh = Math.Abs(Hysterisis);
            float minusTh = -plusTh;
            List<Peak> list = new List<Peak>();
            ///
            Peak peak = null;

            for (int i = 0; i < datas.Length; i++)
            {
                //플러스 rising  x절편
                if (datas[i] > plusTh) //rising
                {
                    peak = new Peak();
                    peak.refData = datas;
                    peak.beginZeroPos = i;

                    //falling 찾기
                    float maxValue = datas[i];
                    int maxIndex = i;
                    for (int j = i; j < datas.Length; j++)
                    {
                        if (datas[j] > maxValue) { maxValue = datas[j]; maxIndex = j; }
                        if (datas[j] < plusTh) //falling
                        {
                            peak.peakPos = maxIndex;
                            peak.peakValue = maxValue;
                            peak.endZeroPos = j - 1;
                            list.Add(peak);
                            i = j - 1; //건너뛰기
                            break;
                        }
                    }
                }
                else if (datas[i] < minusTh)//마이너스 falling x 절편
                {
                    peak = new Peak();
                    peak.refData = datas;
                    peak.beginZeroPos = i;

                    //rising 찾기
                    float minValue = datas[i];
                    int minIndex = i;
                    for (int j = i; j < datas.Length; j++)
                    {
                        if (datas[j] < minValue) { minValue = datas[j]; minIndex = j; }
                        if (datas[j] > minusTh) //rising
                        {
                            peak.peakPos = minIndex;
                            peak.peakValue = minValue;
                            peak.endZeroPos = j - 1;
                            list.Add(peak);
                            i = j - 1; //건너뛰기
                            break;
                        }
                    }
                }
                //                
            }

            //영상의 시작점 끝점, 또는 페이가 맞지 않는 것 삭제
            int datalength = datas.Length;
            var removed = list.RemoveAll(f => f.beginZeroPos < 1 || f.endZeroPos < 1 ||
             f.endZeroPos >= datalength - 1);
            return list;
        }


        private void Binalize(AlgoImage algoImage, AlgoImage binalImage, ImageProcessing ip)
        {
            //float grayAverage = ip.GetGreyAverage(algoImage);
            //byte thMin = MathHelper.Clip<double, byte>(grayAverage + binalizeTh.Min, 0, 255);
            //byte thMax = MathHelper.Clip<double, byte>(grayAverage + binalizeTh.Max, 0, 255);

            //ip.Binarize(algoImage, binalImage, thMin, thMax, true);
        }


        void DebugList(List<MarginEdgePair> pairlist)
        {
            int n = 0;
            foreach (var pair in pairlist)
            {
                Debug.WriteLine("{0}:{1}", n, pair.GetString());
                n++;
            }
        }
    }
}
