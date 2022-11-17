using DynMvp.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.Vision;

namespace UniScanG.Data
{
    public class RepeatedDefectItemCollection : IEnumerable, IEnumerator
    {
        public List<RepeatedDefectItem> List => this.repeatedDefectItemList;
        List<RepeatedDefectItem> repeatedDefectItemList = new List<RepeatedDefectItem>();

        public int Count
        {
            get { return this.repeatedDefectItemList.Count; }
        }

        public RepeatedDefectItem this[int i]
        {
           get { return repeatedDefectItemList[i]; }
        }

        public object Current { get { return this.repeatedDefectItemList; } }

        public void Clear()
        {
            repeatedDefectItemList.Clear();
        }

        public void AddResult(AlgorithmResultG sheetResult, bool autoRemove)
        {
            lock (repeatedDefectItemList)
            {
                // 기존 반복불량 영역들에 null을 추가한다.
                this.repeatedDefectItemList.ForEach(f => f.Add(autoRemove));

                List<FoundedObjInPattern> list = sheetResult.SheetSubResultList.FindAll(f => f.IsDefect);
                foreach (FoundedObjInPattern sheetSubResult in list)
                {
                    bool contained = false;
                    // 검사에서 검출된 불량이 기존 영역에 속하면 해당 영역 마지막에 덮어쓴다.
                    foreach (RepeatedDefectItem repeatedDefectItem in this.repeatedDefectItemList)
                    {
                        if (repeatedDefectItem.RepeatedDefectElementList.FirstOrDefault() != null)
                            continue;

                        contained = repeatedDefectItem.IsContained(sheetSubResult);
                        if (contained)
                        {
                            repeatedDefectItem.Update(sheetSubResult);
                            break;
                        }
                    }

                    // 마지막 검사에서 검출된 불량이 기존 영역에 속하지 않으면 신규 영역으로 추가한다.
                    if (contained == false)
                    {
                        RepeatedDefectAlarmSetting alarmSetting = AdditionalSettings.Instance().RepeatedDefectAlarm.GetAlarmSetting(sheetSubResult.GetDefectType());
                        //RepeatedDefectAlarmSetting alarmSetting = null;
                        //switch (sheetSubResult.GetDefectType())
                        //{
                        //    case DefectType.PinHole:
                        //        alarmSetting = AdditionalSettings.Instance().RepeatedDefectAlarm.PinHole; break;

                        //    case DefectType.Spread:
                        //        alarmSetting = AdditionalSettings.Instance().RepeatedDefectAlarm.Spread; break;

                        //    case DefectType.Noprint:
                        //        alarmSetting = AdditionalSettings.Instance().RepeatedDefectAlarm.NoPrint; break;

                        //    default:
                        //        alarmSetting = AdditionalSettings.Instance().RepeatedDefectAlarm.Etcetera; break;
                        //}

                        if (alarmSetting != null)
                            repeatedDefectItemList.Add(new RepeatedDefectItem(sheetSubResult, alarmSetting));
                    }
                }
                
                // 모두 null인 영역은 지운다.
                RemovePureData();
            }
        }

        public void RemovePureData()
        {
            lock (repeatedDefectItemList)
                repeatedDefectItemList.RemoveAll(f => f.IsPureData);

        }

        public List<RepeatedDefectItem> GetAlarmData()
        {
            return repeatedDefectItemList.FindAll(f => f.IsAlarmState);
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return this.repeatedDefectItemList.GetEnumerator();
        }

        internal void Sort()
        {
            repeatedDefectItemList.Sort((f, g) => g.ValidItemCount.CompareTo(f.ValidItemCount));
        }
    }

    //public class RepeatedDefectElement
    //{
    //    public FoundedObjInPattern Reference { get => this.reference; set => this.reference = value; }
    //    public FoundedObjInPattern reference;

    //    public int Count { get => this.count; set => this.count = value; }
    //    public int count;

    //    public RepeatedDefectElement()
    //    {
    //        this.reference = null;
    //        this.count = 0;
    //    }
    //}

    public class RepeatedDefectItem
    {
        public RepeatedDefectAlarmSetting alarmSetting { get; private set; }

        public DefectType DefectType { get; private set; }

        public RectangleF BoundingRect { get; private set; }

        public List<FoundedObjInPattern> RepeatedDefectElementList { get; private set; } = new List<FoundedObjInPattern>();

        /// <summary>
        /// 알람: 한번이라도 알람 기준점을 넘긴 경우 
        /// </summary>
        public bool IsAlarmState { get; private set; }

        /// <summary>
        /// 클리어: 알람장 CLOSE하여 닫은 경우 
        /// </summary>
        public bool IsAlarmCleared { get; set; }

        /// <summary>
        /// 통지: 신규 발생한 알람 
        /// </summary>
        public bool IsNotified { get; set; }

        public int ValidItemCount
        {
            get
            {
                lock (this.RepeatedDefectElementList)
                    return this.RepeatedDefectElementList.Count(f => f != null);
            }
        }

        public int TotalItemCount
        {
            get
            {
                lock (this.RepeatedDefectElementList)
                    return this.RepeatedDefectElementList.Count();
            }
        }

        public float RepeatRatio
        {
            get => ValidItemCount * 100.0f / alarmSetting.Count;
        }

        //public float ContinueRatio
        //{

        //    get
        //    {
        //        lock (sheetSubResultList)
        //        {
        //            int lastIdx = sheetSubResultList.FindLastIndex(f => f == null);
        //            int count = Math.Min(sheetSubResultList.Count - lastIdx - 1, settings.ContinuousDefectAlarm.Count);
        //            return count * 100.0f / settings.ContinuousDefectAlarm.Count;
        //        }
        //    }
        //}

        public bool IsPureData
        {
            get
            {
                lock (this.RepeatedDefectElementList)
                    return this.RepeatedDefectElementList.Count(f => f != null) == 0;
            }
        }

        public RepeatedDefectItem(FoundedObjInPattern sheetSubResult, RepeatedDefectAlarmSetting alarmSetting)
        {
            this.RepeatedDefectElementList = new List<FoundedObjInPattern>();
            this.DefectType = DefectType.Unknown;
            this.alarmSetting = alarmSetting;

            Add(false);
            Update(sheetSubResult);
        }

        public bool IsContained(FoundedObjInPattern sheetSubResult)
        {
            //if (boundingRect.IsEmpty)
            //    return false;

            FoundedObjInPattern lastAdded = this.RepeatedDefectElementList.FirstOrDefault(f => f != null);
            if (lastAdded != null)
            {
                if (lastAdded.GetDefectType() != sheetSubResult.GetDefectType())
                    return false;
            }

            SizeF inflate = SizeF.Empty;
            DynMvp.Vision.Calibration calibration = SystemManager.Instance()?.DeviceBox.CameraCalibrationList.FirstOrDefault();
            if (calibration != null)
                inflate = calibration.WorldToPixel(new SizeF(this.alarmSetting.InflateRangeWUm, this.alarmSetting.InflateRangeHUm));

            bool intersect = RectangleF.Inflate(BoundingRect, inflate.Width, inflate.Height).IntersectsWith(sheetSubResult.Region);
            return intersect;
        }

        public void Add(bool autoRemove)
        {
            lock (this.RepeatedDefectElementList)
            {
                this.RepeatedDefectElementList.Insert(0, null);

                if (autoRemove)
                {
                    int listMaxCnt = alarmSetting.Count;
                    if (this.RepeatedDefectElementList.Count > listMaxCnt)
                    {
                        int removeCnt = this.RepeatedDefectElementList.Count - listMaxCnt;
                        this.RepeatedDefectElementList.RemoveRange(listMaxCnt, removeCnt);
                    }
                }
            }
        }

        public void Update(FoundedObjInPattern sheetSubResult)
        {
            lock (this.RepeatedDefectElementList)
            {
                if (this.RepeatedDefectElementList[0] == null)
                {
                    this.RepeatedDefectElementList[0] = sheetSubResult;
                    if (this.DefectType == DefectType.Unknown)
                        this.DefectType = sheetSubResult.GetDefectType();
                }
                else
                {

                }

                UpdateRect();

                bool isReapDefected = false;
                if (alarmSetting.Use)
                {
                    double N = alarmSetting.Count / 100.0 * alarmSetting.Percent;
                    int N2 = this.RepeatedDefectElementList.Count(f => f != null);
                    isReapDefected = N <= N2;
                }

                bool alarmNeed = (isReapDefected);
                if (alarmNeed)
                {
                    if (this.IsAlarmState == false)
                        this.IsNotified = false; // 신규 알람인 경우 통지해줘야 한다.

                    this.IsAlarmState = true;
                }
                else
                {
                    //if (this.isAlarmState && this.isAlarmCleared)   // 이전에 알람이었는데, Clear 한 경우
                    //    this.isAlarmState = false;
                }
            }
        }

        private void UpdateRect()
        {
            List<FoundedObjInPattern> validSheetSubResultList = this.RepeatedDefectElementList.FindAll(f => f != null);
            int validSheetSubResultCount = validSheetSubResultList.Count;
            if (validSheetSubResultList.Count == 0)
            {
                this.BoundingRect = RectangleF.Empty;
            }
            else
            {
                float l = 0, t = 0, r = 0, b = 0;
                validSheetSubResultList.ForEach(f =>
                {
                    FoundedObjInPattern sheetSubResult = f;
                    l += sheetSubResult.Region.Left; t += sheetSubResult.Region.Top;
                    r += sheetSubResult.Region.Right; b += sheetSubResult.Region.Bottom;
                });

                this.BoundingRect = RectangleF.FromLTRB(l / validSheetSubResultCount, t / validSheetSubResultCount, r / validSheetSubResultCount, b / validSheetSubResultCount);
                //boundingRect.Inflate(140, 140);
            }
        }

        public Figure GetFigure(float resizeRatio)
        {
            //FoundedObjInPattern sheetSubResult = this.RepeatedDefectElementList.First(f => f.Reference != null)?.Reference;
            //if (sheetSubResult == null)
            //    return null;

            //Figure figure = sheetSubResult.GetFigure(resizeRatio);
            //figure.Tag = sheetSubResult;
            //return figure;

            Figure figure = null;
            List<FoundedObjInPattern> objList = this.RepeatedDefectElementList.FindAll(f => f != null);
            if (objList.Count == 1)
            {
                FoundedObjInPattern foundObj = objList.First();
                figure = foundObj.GetFigure(resizeRatio);
            }
            else
            {
                throw new NotImplementedException();
            }
            return figure;
        }
    }
}
