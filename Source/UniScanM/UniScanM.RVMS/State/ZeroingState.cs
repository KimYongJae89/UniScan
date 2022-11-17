using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanM.Algorithm;
using UniScanM.Data;
using DynMvp.InspData;
using UniEye.Base;
using UniEye.Base.Inspect;
using DynMvp.Base;
using UniScanM.State;
using UniScanM.RVMS.Operation;
using UniScanM.RVMS.Settings;

namespace UniScanM.RVMS.State
{
    public class ZeroingState : UniScanState
    {
        List<List<float>> recentlyDataList = new List<List<float>>();
        float[] zeroingValues = null;

        public override bool IsTeachState
        {
            get { return false; }
        }

        public ZeroingState() : base()
        { 
        }

        protected override void Init()
        {
        }

        public override void PreProcess()
        {
        }

        public override void PostProcess(DynMvp.InspData.InspectionResult inspectionResult)
        {
        }

        public override void OnProcess(ImageD imageD, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            RVMS.Data.InspectionResult rvmsInspectionResult = (RVMS.Data.InspectionResult)inspectionResult;
            rvmsInspectionResult.ZeroingComplate = false;
            //rvmsInspectionResult.CurValueList
            rvmsInspectionResult.ManSide = new Data.ScanData(inspectionResult.InspectionStartTime, 0, rvmsInspectionResult.CurValueList[0], 0);
            rvmsInspectionResult.GearSide = new Data.ScanData(inspectionResult.InspectionStartTime, 0, rvmsInspectionResult.CurValueList[1], 0);

            //초기화
            if (recentlyDataList.Count == 0)
            {
                for (int i = 0; i < rvmsInspectionResult.CurValueList.Count; i++)
                {
                    recentlyDataList.Add(new List<float>());
                }
            }

            RVMSSettings rvmsSetting = (RVMSSettings)RVMSSettings.Instance();

            for (int i = 0; i < recentlyDataList.Count; i++)
            {
                recentlyDataList[i].Add(rvmsInspectionResult.CurValueList[i]);
                recentlyDataList[i].RemoveRange(0, Math.Max(0, recentlyDataList[i].Count - rvmsSetting.DataCountForZeroSetting));
            }

            int zeroingNum = recentlyDataList.Min(f => f.Count);
            double maxVariance = recentlyDataList.Max(f => f.Max() - f.Min());
            double threshold = Math.Abs(rvmsSetting.LineStopUpper + rvmsSetting.LineStopLower);
            bool isStable = maxVariance < threshold;
            rvmsInspectionResult.ZeroingNum = zeroingNum;
            rvmsInspectionResult.ZeroingVariance = maxVariance;
            rvmsInspectionResult.ZeroingStable = isStable;

            zeroingValues = recentlyDataList.ConvertAll(f => f.Average()).ToArray();
        }
        
        public override UniScanState GetNextState(DynMvp.InspData.InspectionResult inspectionResult)
        {
            RVMS.Data.InspectionResult rvmsInspectionResult = (RVMS.Data.InspectionResult)inspectionResult;

            RVMSSettings rvmsSetting = (RVMSSettings)RVMSSettings.Instance();
            int zeroingNum = rvmsInspectionResult.ZeroingNum;

            if (zeroingNum < rvmsSetting.DataCountForZeroSetting || !rvmsInspectionResult.ZeroingStable)
                return this;
            else
                return new RVMS.State.InspectionState(zeroingValues[1], zeroingValues[0], true);
        }
    }
}
