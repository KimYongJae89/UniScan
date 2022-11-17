using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanS.Screen.Data;

namespace UniScanS.Data
{
    public enum AlarmIOType
    {
        Alarm, NG
    }

    public enum AlarmType
    {
        CheckPoint, Recent, SamePoint
    }

    public abstract class AlarmChecker
    {
        protected List<MergeSheetResult> resultList = new List<MergeSheetResult>();

        private AlarmType alarmType;
        private AlarmIOType alarmIOType;
        
        public AlarmType AlarmType { get => alarmType; set => alarmType = value; }
        public List<MergeSheetResult> ResultList { get => resultList; }
        public AlarmIOType AlarmIOType { get => alarmIOType; set => alarmIOType = value; }

        public abstract void Reset();
        public abstract bool CalcResult(MergeSheetResult sheetResult);
        public abstract FigureGroup GetFigure(List<MergeSheetResult> sheetResult);
        

        public virtual void Save(XmlElement alarmElement)
        {
            XmlHelper.SetValue(alarmElement, "AlarmType", AlarmType.ToString());
            XmlHelper.SetValue(alarmElement, "AlarmIOType", alarmIOType.ToString());
        }

        public virtual void Load(XmlElement alarmElement)
        {
            AlarmType = (AlarmType)Enum.Parse(typeof(AlarmType), XmlHelper.GetValue(alarmElement, "AlarmType", "CheckPoint"));
            alarmIOType = (AlarmIOType)Enum.Parse(typeof(AlarmIOType), XmlHelper.GetValue(alarmElement, "AlarmIOType", AlarmIOType.Alarm.ToString()));
        }

        public static AlarmChecker CreateAlarmChecker(AlarmType alarmType)
        {
            switch (alarmType)
            {
                case AlarmType.CheckPoint:
                    return new CheckPointAlarmChecker();
                case AlarmType.Recent:
                    return new RecentAlarmChecker();
                case AlarmType.SamePoint:
                    return new SamePointAlarmChecker();
            }

            return null;
        }

        public static AlarmChecker LoadAlarmChecker(XmlElement alarmElement)
        {
            AlarmChecker alarmChecker = null;

            AlarmType alarmType = (AlarmType)Enum.Parse(typeof(AlarmType), XmlHelper.GetValue(alarmElement, "AlarmType", "CheckPoint"));
            switch (alarmType)
            {
                case AlarmType.CheckPoint:
                    alarmChecker = new CheckPointAlarmChecker();
                    break;
                case AlarmType.Recent:
                    alarmChecker = new RecentAlarmChecker();
                    break;
                case AlarmType.SamePoint:
                    alarmChecker = new SamePointAlarmChecker();
                    break;
            }

            alarmChecker.Load(alarmElement);

            return alarmChecker;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class CheckPointAlarmChecker : AlarmChecker
    {
        //Public
        int checkIndex;
        float ratio;
        
        public int CheckIndex { get => checkIndex; set => checkIndex = value; }
        public float Ratio { get => ratio; set => ratio = value; }

        public CheckPointAlarmChecker()
        {
            AlarmType = AlarmType.CheckPoint;
        }

        public override void Reset()
        {
            lock (resultList)
                resultList.Clear();
        }

        public override bool CalcResult(MergeSheetResult sheetResult)
        {
            if (sheetResult.Index > checkIndex)
                return true;
            
            if (sheetResult.Index <= checkIndex - 10)
                return true;

            lock (resultList)
                resultList.Add(sheetResult);

            if (sheetResult.Index < checkIndex)
                return true;
            
            if (ratio <= SystemManager.Instance().ProductionManager.CurProduction.NgRatio)
                return false;

            return true;
        }

        public override void Save(XmlElement alarmElement)
        {
            base.Save(alarmElement);

            XmlHelper.SetValue(alarmElement, "CheckIndex", checkIndex.ToString());
            XmlHelper.SetValue(alarmElement, "Ratio", ratio.ToString());
        }

        public override void Load(XmlElement alarmElement)
        {
            base.Load(alarmElement);

            checkIndex = Convert.ToInt32(XmlHelper.GetValue(alarmElement, "CheckIndex", "100"));
            ratio = Convert.ToSingle(XmlHelper.GetValue(alarmElement, "Ratio", "1"));
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override FigureGroup GetFigure(List<MergeSheetResult> sheetResult)
        {
            throw new NotImplementedException();
        }
    }

    public class RecentAlarmChecker : AlarmChecker
    {
        int recentNum;
        float ratio;

        bool useSheetAttack;
        bool usePole;
        bool useDielectric;
        bool usePinHole;
        bool useShape;

        public int RecentNum { get => recentNum; set => recentNum = value; }
        public float Ratio { get => ratio; set => ratio = value; }
        public bool UseSheetAttack { get => useSheetAttack; set => useSheetAttack = value; }
        public bool UsePole { get => usePole; set => usePole = value; }
        public bool UseDielectric { get => useDielectric; set => useDielectric = value; }
        public bool UsePinHole { get => usePinHole; set => usePinHole = value; }
        public bool UseShape { get => useShape; set => useShape = value; }
        
        public RecentAlarmChecker()
        {
            AlarmType = AlarmType.Recent;
        }

        public override bool CalcResult(MergeSheetResult sheetResult)
        {
            if (resultList.Count == recentNum)
                lock (resultList)
                    resultList.RemoveAt(0);

            lock (resultList)
                resultList.Add(sheetResult);

            if (resultList.Count < recentNum)
                return true;

            int good = 0;
            int ng = 0;
            foreach (SheetResult result in resultList)
            {
                if (UseSheetAttack && result.SheetAttackList.Count > 0)
                    ng++;
                else if (usePole && result.PoleList.Count > 0)
                    ng++;
                else if (useDielectric && result.DielectricList.Count > 0)
                    ng++;
                else if (usePinHole && result.PinHoleList.Count > 0)
                    ng++;
                else if (useShape && result.ShapeList.Count > 0)
                    ng++;
                else
                    good++;
            }

            if (ng / (good + ng) >= ratio)
                return false;

            return true;
        }

        public override void Reset()
        {
            lock (resultList)
                resultList.Clear();
        }

        public override void Save(XmlElement alarmElement)
        {
            base.Save(alarmElement);

            XmlHelper.SetValue(alarmElement, "RecentNum", recentNum.ToString());
            XmlHelper.SetValue(alarmElement, "Ratio", ratio.ToString());

            XmlHelper.SetValue(alarmElement, "UseSheetAttack", useSheetAttack.ToString());
            XmlHelper.SetValue(alarmElement, "UsePole", usePole.ToString());
            XmlHelper.SetValue(alarmElement, "UseDielectric", useDielectric.ToString());
            XmlHelper.SetValue(alarmElement, "UsePinHole", usePinHole.ToString());
            XmlHelper.SetValue(alarmElement, "UseShape", useShape.ToString());
        }

        public override void Load(XmlElement alarmElement)
        {
            base.Load(alarmElement);

            recentNum = Convert.ToInt32(XmlHelper.GetValue(alarmElement, "RecentNum", "100"));
            ratio = Convert.ToSingle(XmlHelper.GetValue(alarmElement, "Ratio", "1"));

            useSheetAttack = Convert.ToBoolean(XmlHelper.GetValue(alarmElement, "UseSheetAttack", "true"));
            usePole = Convert.ToBoolean(XmlHelper.GetValue(alarmElement, "UsePole", "true"));
            useDielectric = Convert.ToBoolean(XmlHelper.GetValue(alarmElement, "UseDielectric", "true"));
            usePinHole = Convert.ToBoolean(XmlHelper.GetValue(alarmElement, "UsePinHole", "true"));
            useShape = Convert.ToBoolean(XmlHelper.GetValue(alarmElement, "UseShape", "true"));
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override FigureGroup GetFigure(List<MergeSheetResult> sheetResult)
        {
            throw new NotImplementedException();
        }
    }

    public class SamePointAlarmChecker : AlarmChecker
    {
        int sameNum;

        bool useSheetAttack;
        bool usePole;
        bool useDielectric;
        bool usePinHole;
        bool useShape;

        public int SameNum { get => sameNum; set => sameNum = value; }
        public bool UseSheetAttack { get => useSheetAttack; set => useSheetAttack = value; }
        public bool UsePole { get => usePole; set => usePole = value; }
        public bool UseDielectric { get => useDielectric; set => useDielectric = value; }
        public bool UsePinHole { get => usePinHole; set => usePinHole = value; }
        public bool UseShape { get => useShape; set => useShape = value; }


        public SamePointAlarmChecker()
        {
            AlarmType = AlarmType.SamePoint;
        }

        public override bool CalcResult(MergeSheetResult sheetResult)
        {
            if (resultList.Count > sameNum && resultList.Count > 0)
                resultList.RemoveAt(0);

            if (resultList.Find(result => result.Index == sheetResult.Index) != null)
                return true;

            resultList.Add(sheetResult);

            if (resultList.Count < sameNum)
                return true;

            List<SamePointDefects> pointsList = new List<SamePointDefects>();
            foreach (MergeSheetResult mergeSheetResult in resultList)
            {
                foreach (SheetSubResult subResult in mergeSheetResult.SheetSubResultList)
                {
                    bool isContain = false;
                    foreach (SamePointDefects defects in pointsList)
                    {
                        if (defects.TryAdd(subResult) == true)
                        {
                            isContain = true;
                            break;
                        }
                    }
                    
                    if (isContain == false)
                        pointsList.Add(new SamePointDefects(subResult));
                }
            }
            
            foreach (SamePointDefects defects in pointsList)
            {
                if (defects.Count >= sameNum)
                    return false;
            }

            return true;
        }
        
        public override FigureGroup GetFigure(List<MergeSheetResult> sheetResult)
        {
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            resultList.Clear();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void Save(XmlElement alarmElement)
        {
            base.Save(alarmElement);
            
            XmlHelper.SetValue(alarmElement, "SameNum", sameNum.ToString());

            XmlHelper.SetValue(alarmElement, "UseSheetAttack", useSheetAttack.ToString());
            XmlHelper.SetValue(alarmElement, "UsePoleLine", usePole.ToString());
            XmlHelper.SetValue(alarmElement, "UseDielectric", useDielectric.ToString());
            XmlHelper.SetValue(alarmElement, "UsePinHole", usePinHole.ToString());
            XmlHelper.SetValue(alarmElement, "UseShape", useShape.ToString());
        }

        public override void Load(XmlElement alarmElement)
        {
            base.Load(alarmElement);
            
            sameNum = Convert.ToInt32(XmlHelper.GetValue(alarmElement, "SameNum", "3"));

            useSheetAttack = Convert.ToBoolean(XmlHelper.GetValue(alarmElement, "UseSheetAttack", "true"));
            usePole = Convert.ToBoolean(XmlHelper.GetValue(alarmElement, "UsePole", "true"));
            useDielectric = Convert.ToBoolean(XmlHelper.GetValue(alarmElement, "UseDielectric", "true"));
            usePinHole = Convert.ToBoolean(XmlHelper.GetValue(alarmElement, "UsePinHole", "true"));
            useShape = Convert.ToBoolean(XmlHelper.GetValue(alarmElement, "UseShape", "true"));
        }
    }
}
