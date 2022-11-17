using DynMvp.Base;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Data;
using UniScanG.Data;
using UniScanG.Gravure.Data;
using UniScanG.Gravure.Device;
using UniScanG.Gravure.Inspect;
using UniScanG.Gravure.Settings;
using UniScanG.Module.Controller.MachineIF;

namespace UniScanG.Module.Controller.Inspect
{
    public class AlarmManager : UniScanG.Gravure.Inspect.AlarmManager
    {
        // 알람 조건
        AdditionalSettings additionalSettings = (AdditionalSettings)AdditionalSettings.Instance();

        // 알람 통신
        MachineIfData machineIfData = (SystemManager.Instance().DeviceController as DeviceControllerG)?.MachineIfMonitor?.MachineIfData as MachineIfData;

        // 패턴번호, 불량개수
        protected SortedList<int, int> patternResultList = new SortedList<int, int>();

        // 마진측정
        public MarginPanelItem MarginPanelItemList { get; } = new MarginPanelItem();

        // 최근 패턴의 불량
        public float[] DefectDensity { get; private set; }

        // 최근 패턴의 길이
        public float LastPatternLength { get; private set; }

        public AlarmManager() { }

        public override void ClearSignal()
        {
            machineIfData.ClearVisionNG();
        }

        public override void UpdateSignal()
        {
            if (this.patternResultList.Count == 0)
            {
                machineIfData.ClearVisionNG();
                return;
            }

            lock (patternResultList)
            {
                bool isNg = this.patternResultList.Last().Value > 0;
                machineIfData.SET_VISION_GRAVURE_INSP_RESULT = isNg;

                CalculateNormalDefectAlarm();
                CalculateDefectCountAlarm();
                CalculateMarginAlarm();
            }

            CalculatePatternLengthAlarm();
            CalculateStripeBlotAlarm();
            CalculateRollCriticalAlarm();
            
        }

        public override void ClearData()
        {
            base.ClearData();

            lock (this.patternResultList)
                this.patternResultList.Clear();

            lock (this.MarginPanelItemList)
                this.MarginPanelItemList.ClearAlarm = true;
        }

        public override void AddResult(InspectionResult inspectionResult)
        {
            if (!int.TryParse(inspectionResult.InspectionNo, out int patternNo))
                return;

            bool result = inspectionResult.IsGood();

            if (!inspectionResult.AlgorithmResultLDic.ContainsKey(SheetCombiner.TypeName))
                return;

            MergeSheetResult mergeSheetResult = inspectionResult.AlgorithmResultLDic[SheetCombiner.TypeName] as MergeSheetResult;

            float height = mergeSheetResult.SheetSize.Height;
            lock (this.patternLengthList)
            {
                this.patternLengthList.Add(height);
                this.patternLengthList.Sort();
                UpdatePatternLength();
            }

            List<FoundedObjInPattern> list = mergeSheetResult.SheetSubResultList;
            if (list != null)
            {
                int dCount = list.Count(f => f.IsDefect);
                lock (this.patternResultList)
                    this.patternResultList.Add(patternNo, dCount);                

                //LogHelper.Debug(LoggerType.Inspection, string.Format("AlarmManager::AddResult - PatternNo: {0}, dCount: {1}, height: {2}", patternNo, dCount, height));

                List<MarginObj> marginObjList = list.FindAll(f => f is MarginObj).ConvertAll<MarginObj>(f => (MarginObj)f);
                if (marginObjList.Count > 0)
                    this.MarginPanelItemList.Update(patternNo, marginObjList);

                this.DefectDensity = mergeSheetResult.DefectDensity;
                this.LastPatternLength = mergeSheetResult.SheetSize.Height;
                this.machineIfData.SET_VISION_GRAVURE_INSP_INFO_SHTLEN_REAL = mergeSheetResult.SheetSize.Height;
            }

            UpdateSignal();
        }

        private void CalculateNormalDefectAlarm()
        {
            NormalDefectAlarmSetting normalDefectAlarm = additionalSettings.NormalDefectAlarm;
            if (!normalDefectAlarm.Use)
            {
                machineIfData.SET_VISION_GRAVURE_INSP_NG_NORDEF = false;
                return;
            }

            if (this.patternResultList.Count >= normalDefectAlarm.Count)
            {
                int skipCnt = this.patternResultList.Count - normalDefectAlarm.Count;
                int takeCnt = normalDefectAlarm.Count;
                int ngCnt = this.patternResultList.Skip(skipCnt).Take(takeCnt).Count(f => f.Value >= normalDefectAlarm.MinimumDefects);
                bool alarm = (ngCnt * 100f / takeCnt) >= normalDefectAlarm.Percent;
                alarm &= (SystemState.Instance().OpState == OpState.Inspect);

                if (!machineIfData.SET_VISION_GRAVURE_INSP_NG_NORDEF && alarm)
                {
                    ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Inspect.Information, ErrorLevel.Warning,
                          "Defects", "Normal Defect Ratio Warning", null);
                }

                // 알람 조건이 해제되면 알람이 꺼짐 -> 이미 발동된 알람은 인쇄기에서 처리.
                machineIfData.SET_VISION_GRAVURE_INSP_NG_NORDEF = alarm;
            }
        }

        private void CalculatePatternLengthAlarm()
        {
            AbsoluteAlarmSetting sheetLengthAlarm = additionalSettings.SheetLengthAlarm;
            if (!sheetLengthAlarm.Use || this.patternLengthList.Count == 0)
            {
                machineIfData.SET_VISION_GRAVURE_INSP_NG_SHTLEN = false;
                return;
            }

            if (this.patternLengthList.Count >= sheetLengthAlarm.Count)
            {
                bool alarm = ((this.Upper10 - this.Lower10) >= sheetLengthAlarm.Value);
                alarm &= (SystemState.Instance().OpState == OpState.Inspect);

                if (!machineIfData.SET_VISION_GRAVURE_INSP_NG_SHTLEN && alarm)
                {
                    ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Inspect.Information, ErrorLevel.Warning,
                          "Defects", "Pattern Length Warning", null);
                }

                // 알람 조건이 해제되면 알람이 꺼짐 -> 이미 발동된 알람은 인쇄기에서 처리.
                machineIfData.SET_VISION_GRAVURE_INSP_NG_SHTLEN = alarm;
            }
        }

        private void CalculateDefectCountAlarm()
        {
            if (!additionalSettings.DefectCountAlarm.Use)
            {
                machineIfData.SET_VISION_GRAVURE_INSP_NG_DEFCNT = false;
                return;
            }

            int dCount = this.patternResultList.Last().Value;
            bool alarm = dCount >= additionalSettings.DefectCountAlarm.Count;
            alarm &= (SystemState.Instance().OpState == OpState.Inspect);

            if (!machineIfData.SET_VISION_GRAVURE_INSP_NG_DEFCNT && alarm)
            {
                ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Inspect.Information, ErrorLevel.Warning,
                      "Defects", "Defect Count Warning", null);
            }

            // 알람 조건이 해제되면 알람이 꺼짐 -> 이미 발동된 알람은 인쇄기에서 처리.
            machineIfData.SET_VISION_GRAVURE_INSP_NG_DEFCNT = alarm;
        }

        private void CalculateMarginAlarm()
        {
            if (!additionalSettings.MarginLengthAlarm.Use)
            {
                machineIfData.SET_VISION_GRAVURE_INSP_NG_MARGIN = false;
                return;
            }

            bool alarm = this.MarginPanelItemList.IsAlarm(additionalSettings.MarginLengthAlarm);
            alarm &= (SystemState.Instance().OpState == OpState.Inspect);

            if (!machineIfData.SET_VISION_GRAVURE_INSP_NG_MARGIN && alarm)
            {
                ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Inspect.Information, ErrorLevel.Warning,
                       "Defects", "Margin Length Warning", null);
            }

            // 알람 조건이 해제되면 알람이 꺼짐 -> 이미 발동된 알람은 인쇄기에서 처리.
            machineIfData.SET_VISION_GRAVURE_INSP_NG_MARGIN = alarm;
        }

        private void CalculateStripeBlotAlarm()
        {
            if (!additionalSettings.StripeDefectAlarm.Use || this.DefectDensity == null)
            {
                machineIfData.SET_VISION_GRAVURE_INSP_NG_STRIPE = false;
                return;
            }

            bool alarm = Array.Exists(this.DefectDensity, f => f >= additionalSettings.StripeDefectAlarm.Percent);
            alarm &= (SystemState.Instance().OpState == OpState.Inspect);

            if (!machineIfData.SET_VISION_GRAVURE_INSP_NG_STRIPE && alarm)
            {
                ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Inspect.Information, ErrorLevel.Warning,
                       "Defects", "Stripe Blot Detected", null);
            }

            // 알람 조건이 해제되면 알람이 꺼짐 -> 이미 발동된 알람은 인쇄기에서 처리.
            machineIfData.SET_VISION_GRAVURE_INSP_NG_STRIPE = alarm;
        }

        private void CalculateRollCriticalAlarm()
        {
            CriticalRollAlarmSetting criticalRollAlarm = additionalSettings.CriticalRollAlarm;
            if (!criticalRollAlarm.Use)
            {
                machineIfData.SET_VISION_GRAVURE_INSP_NG_CRITICAL = false;
                return;
            }

            ProductionG productionG = SystemManager.Instance().ProductionManager.CurProduction as ProductionG;
            if (productionG == null)
                return;

            bool state = (productionG.CriticalPatternNum >= criticalRollAlarm.Count);
            if (criticalRollAlarm.NotifyOnce)
                state &= (criticalRollAlarm.LastAlarmedLotNo != productionG.LotNo);
            state &= (SystemState.Instance().OpState == OpState.Inspect);

            if (!machineIfData.SET_VISION_GRAVURE_INSP_NG_CRITICAL && state)
            {
                criticalRollAlarm.LastAlarmedLotNo = productionG.LotNo;
                ErrorManager.Instance().Report(ErrorSectionSystem.Instance.Inspect.Information, ErrorLevel.Warning,
                       "Defects", "Critical Roll Detected.", null);
            }

            // 알람 조건이 해제되면 알람이 꺼짐 -> 이미 발동된 알람은 인쇄기에서 처리.
            machineIfData.SET_VISION_GRAVURE_INSP_NG_CRITICAL = state;
        }
    }
}
