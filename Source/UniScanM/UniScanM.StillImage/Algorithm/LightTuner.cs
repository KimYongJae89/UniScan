using DynMvp.Base;
using DynMvp.Devices.Light;
using DynMvp.Vision;
//using UniScanM.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniScanM.StillImage.Data;
using UniScanM.StillImage.Settings;
using System.Diagnostics;

namespace UniScanM.StillImage.Algorithm
{
    public abstract class LightTuner
    {
        public static LightTuner Create(int version)
        {
            switch (version)
            {
                case 0:
                    return new LightTunerV1();
                case 1:
                    return new LightTunerV2();
                case 2:
                    return new LightTunerV3();
            }
            return null;
        }

        public abstract void Tune(AlgoImage algoImage, InspectionResult inspectionResult);
    }

    public class LightTuneResult
    {
        int offsetLevel;
        int tryCount;
        bool tuneFinished = false;
        public int currentBright = -1;

        public bool IsGood
        {
            get { return offsetLevel == 0; }
        }

        public bool TuneFinished
        {
            get { return tuneFinished; }
            set { tuneFinished = value; }
        }

        public int CurrentBright
        {
            get { return currentBright; }
            set { currentBright = value; }
        }

        public int OffsetLevel
        {
            get { return offsetLevel; }
            set { offsetLevel = value; }
        }

        public int TryCount
        {
            get { return tryCount; }

            set { tryCount = value; }
        }
    }

    public class LightTunerV1 : LightTuner
    {
        const int mul = 16;
        public override void Tune(AlgoImage algoImage, InspectionResult inspectionResult)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            // Clip display image
            Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);
            Point imageCenter = Point.Round(DrawingHelper.CenterPoint(imageRect));
            Rectangle dispRect = DrawingHelper.FromCenterSize(imageCenter, new Size(1000, 1000));
            dispRect.Intersect(imageRect);
            inspectionResult.InspRectInSheet = dispRect;
            
            float avgValue = imageProcessing.GetGreyAverage(algoImage);
            //long[] histogram = imageProcessing.Histogram(algoImage);
            //Array.ForEach(histogram, f => System.Diagnostics.Debug.WriteLine(f));
            LightTuneResult lightTuneResult = new LightTuneResult();
            lightTuneResult.currentBright = (int)avgValue;
            if (avgValue > 200)
            {
                lightTuneResult.OffsetLevel = -2 * mul;
            }
            else if (avgValue > 120)
            {
                lightTuneResult.OffsetLevel = -1 * mul;
            }
            else if (avgValue < 30)
            {
                lightTuneResult.OffsetLevel = +1 * mul;
            }
            else if (avgValue < 50)
            {
                lightTuneResult.OffsetLevel = +2 * mul;
            }
            else
            {
                lightTuneResult.OffsetLevel = 0;
            }
            //      brightnessResultItem.OffsetValue = 0;

            LogHelper.Debug(LoggerType.Algorithm, $"LightTunerV1::Tune - avgValue: {avgValue}, OffsetLevel: {lightTuneResult.OffsetLevel}");

            if (lightTuneResult.OffsetLevel != 0)
                inspectionResult.SetDefect();
            inspectionResult.LightTuneResult = lightTuneResult;
        }
    }

    public class LightTunerV2 : LightTuner
    {
        int offsetValue = 64; // 조명값 부호 변경 횟수

        public override void Tune(AlgoImage algoImage, InspectionResult inspectionResult)
        {
            LightTuneResult lightTuneResult = new LightTuneResult();
            inspectionResult.LightTuneResult = lightTuneResult;

            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            // Clip display image
            Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);
            Point imageCenter = Point.Round(DrawingHelper.CenterPoint(imageRect));
            Rectangle dispRect = DrawingHelper.FromCenterSize(imageCenter, new Size(1000, 1000));
            dispRect.Intersect(imageRect);
            inspectionResult.InspRectInSheet = dispRect;
            using (AlgoImage displayImage = algoImage.GetSubImage(dispRect))
                inspectionResult.DisplayBitmap = displayImage.ToImageD().ToBitmap();

            int avgValue = (int)imageProcessing.GetGreyAverage(algoImage);
            long[] histogram = imageProcessing.Histogram(algoImage);

            List<long> lowerHistogram = histogram.Take(avgValue).ToList(); // 평균 미만 히스토그램
            List<long> upperHistogram = histogram.Skip(avgValue).ToList(); // 평균 이상 히스토그램
            LogHelper.Debug(LoggerType.Algorithm, $"LightTunerV2::Tune - lowerHistogram: {lowerHistogram}, upperHistogram: {upperHistogram}");

            if (lowerHistogram.Count == 0 || upperHistogram.Count == 0)
            {
                if (lowerHistogram.Count == 0)
                    lightTuneResult.OffsetLevel = 15;
                else
                    lightTuneResult.OffsetLevel = -15;

                lightTuneResult.currentBright = -1;
                inspectionResult.SetDefect();
                return;
            }

            // 두 히스토그램의 최대값
            long lowerMax = lowerHistogram.Max();
            int lowerMaxValue = lowerHistogram.IndexOf(lowerMax);

            long upperMax = upperHistogram.Max();
            int upperMaxValue = upperHistogram.IndexOf(upperMax) + (int)avgValue;
            lightTuneResult.currentBright = upperMaxValue;

            LogHelper.Debug(LoggerType.Algorithm, $"LightTunerV2::Tune - lowerMax: {lowerHistogram}, upperMax: {upperMax}");

            // 최대값의 차이가 10 미만이면 영상에 패턴이 없음.
            //if (upperMaxValue - lowerMaxValue < 10)
            //{
            //    inspectionResult.SetDefect();
            //    if (upperMaxValue > 200)
            //        offsetValue = GetOffsetValue(false);
            //    else if (upperMaxValue < 50)
            //        offsetValue = GetOffsetValue(true);
            //}
            //else
            {
                inspectionResult.SetGood();

                int offsetValue = 0;
                int targetValue = StillImageSettings.Instance().TargetIntensity;
                int targetValueVal = StillImageSettings.Instance().TargetIntensityVal;
                bool ok = true;

                if (upperMaxValue < targetValue - targetValueVal)
                {
                    offsetValue = GetOffsetValue(true);
                    inspectionResult.SetDefect();
                }
                else if (upperMaxValue > targetValue + targetValueVal)
                {
                    offsetValue = GetOffsetValue(false);
                    inspectionResult.SetDefect();
                }

                lightTuneResult.OffsetLevel = offsetValue;
            }
        }

        private int GetOffsetValue(bool increase)
        {
            // 조명값 변화의 부호가 반대방향이면 값을 절반으로 줄인다.
            float offset = this.offsetValue;
            if (increase)
            {
                offset /= (offset > 0) ? 1 : -2;
            }
            else
            {
                offset /= (offset < 0) ? 1 : -2;
            }


            this.offsetValue = (int)offset;
            LogHelper.Debug(LoggerType.Algorithm, string.Format("LightTunerV2::GetOffsetValue - {0}, {1}", increase, offset));
            return this.offsetValue;
        }
    }

    //## new minong style #################################################################################
    public class LightTunerV3 : LightTuner
    {
        int offsetValue = 64; // 조명값 부호 변경 횟수
        bool tuneFinished = false;
        public override void Tune(AlgoImage algoImage, InspectionResult inspectionResult)
        {
            LightTuneResult lightTuneResult = new LightTuneResult();
            inspectionResult.LightTuneResult = lightTuneResult;
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(algoImage);

            //01. Clip display image
            Rectangle imageRect = new Rectangle(Point.Empty, algoImage.Size);
            Point imageCenter = Point.Round(DrawingHelper.CenterPoint(imageRect));
            Rectangle dispRect = DrawingHelper.FromCenterSize(imageCenter, new Size(1000, 1000));
            dispRect.Intersect(imageRect);
            inspectionResult.InspRectInSheet = dispRect;
            AlgoImage displayImage = algoImage.GetSubImage(dispRect);
            inspectionResult.DisplayBitmap = displayImage.ToImageD().ToBitmap();
            displayImage.Dispose();

            lightTuneResult.OffsetLevel = offsetValue;
            lightTuneResult.TuneFinished = tuneFinished;
            if (tuneFinished == true) return; /////////////////////////////////////aleady succes.. just wait for pattern-length....

            //02. Get Histogram 
            //Light Tune = LT,lt,
            double rollerDia = SystemManager.Instance().InspectStarter.GetRollerDia();
            int ltHeight = (int)(rollerDia * Math.PI / SystemManager.Instance().DeviceBox.CameraCalibrationList[0].PelSize.Height * 1000 + 0.5f);
            Rectangle ltRect = new Rectangle(0, 0, algoImage.Size.Width, ltHeight);
            AlgoImage ltImage = algoImage.GetSubImage(ltRect);

            long sum = 0;
            long count = 0;
            int avgValue = 0; // (int)imageProcessing.GetGreyAverage(ltImage);
            long[] histogram = imageProcessing.Histogram(ltImage);
            for(int i=0; i< histogram.Length; i++)
            {
                sum += i * histogram[i];
                count += histogram[i];
            }
            avgValue = (int)(sum / count  +0.5f);

            sum = 0;
            count = 0;
            avgValue = 0;
            //Average of Top 1% intensity 
            long wholeCount = ltImage.Width * ltImage.Height;
            long _1percentCNT =(long) (0.01 * wholeCount); //1% is best
            for (int i= histogram.Length-1; i>=0; i-- )
            {
                sum += (long)(i * histogram[i]);
                count += histogram[i];
                if (count > _1percentCNT) break;
            }
            double topAvgIntensity = ((double)sum / count);
            double saturateRate = (double)histogram.Last() / wholeCount;

            // 두 히스토그램의 최대값
            lightTuneResult.currentBright = (int)topAvgIntensity;
            inspectionResult.SetGood();
            if (tuneFinished==false)
            {
               // int targetValue = 255;//StillImageSettings.Instance().TargetIntensity;
               // int targetValueVal = StillImageSettings.Instance().TargetIntensityVal;
                bool ok = true;

                LightValue lightValue = SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0].LightValue.Clone();
                int currentLightValue = lightValue.Value[0];// topValue;

                if (topAvgIntensity == 255) //saturation
                {
                    if(saturateRate < 0.02)
                    {
                        //tuneFinished = true;
                        offsetValue = 0;
                    }
                    else offsetValue = (int) Math.Min(-currentLightValue / 3, -currentLightValue* saturateRate) ;
                    inspectionResult.SetDefect();
                }
                else if (topAvgIntensity > 50) // calculate Controller value proportionally & finish //success
                {
                    Settings.StillImageSettings additionalSettings = StillImageSettings.Instance() as Settings.StillImageSettings;
                    int target =additionalSettings.TargetIntensity;
                    offsetValue = (int)(currentLightValue  * target / topAvgIntensity - currentLightValue);
                    //tuneFinished = true;
                    //offsetValue = 0;
                    //tuneFinished = true;
                    SystemManager.Instance().CurrentModel.LightParamSet.LightParamList[0].LightValue.Value[0] += offsetValue;
                    inspectionResult.SetDefect();
                }
                else ///////////////////////// Dark
                {
                    offsetValue = (int)(128 / topAvgIntensity * currentLightValue);
                    if (currentLightValue < 10) offsetValue = 10;
                    inspectionResult.SetDefect();
                }
                Debug.WriteLine(string.Format("Light:{0}----TopIntensity:{1}----offset:{2}-------", 
                    currentLightValue, topAvgIntensity, offsetValue));
            }
            lightTuneResult.OffsetLevel = offsetValue;
            lightTuneResult.TuneFinished = tuneFinished;
        }

        private int GetTopIntensity(List<long>  histo, double topArea=0.03) //기본 상위 3%...
        {
            long totalcount = 0;
            long calcCount = 0;
            //long []Histo = histo.Reverse<long>().ToArray<long>();
            long[] Histo = histo.ToArray<long>();
            foreach (long count in Histo)
                totalcount += count;

            calcCount = (long)(totalcount * topArea);
            long totalcnt = 0;
            long totalIntensity = 0;
            for(int i=255; i>0; i--)
            {
                totalcount += histo[i];
                totalIntensity += histo[i] * i;
                if (totalcount > calcCount) break;
            }
            int topAreaAverageIntensity = (int)(totalIntensity / totalcount);
            return topAreaAverageIntensity;
        }

    }

}
