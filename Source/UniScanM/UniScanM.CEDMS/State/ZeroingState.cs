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
using UniScanM.CEDMS.Operation;
using UniScanM.CEDMS.Settings;

namespace UniScanM.CEDMS.State
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
            CEDMS.Data.InspectionResult cedmsInspectionResult = (CEDMS.Data.InspectionResult)inspectionResult;
            cedmsInspectionResult.ZeroingComplete = false;
            //sedmsInspectionResult.CurValueList
            cedmsInspectionResult.InFeed = new Data.CEDMSScanData(cedmsInspectionResult.RollDistance, 0, cedmsInspectionResult.CurValueList[1], 0);
            cedmsInspectionResult.OutFeed = new Data.CEDMSScanData(cedmsInspectionResult.RollDistance, 0, cedmsInspectionResult.CurValueList[0], 0);

            //초기화
            if (recentlyDataList.Count == 0)
            {
                for (int i = 0; i < cedmsInspectionResult.CurValueList.Count; i++)
                {
                    recentlyDataList.Add(new List<float>());
                }
            }

            CEDMSSettings cedmsSettings = (CEDMSSettings)CEDMSSettings.Instance();
            
            for (int i = 0; i < recentlyDataList.Count; i++)
            {
                recentlyDataList[i].Add(cedmsInspectionResult.CurValueList[i]);
                recentlyDataList[i].RemoveRange(0, Math.Max(0, recentlyDataList[i].Count - cedmsSettings.DataCountForZeroSetting));
            }

            int zeroingNum = recentlyDataList.Min(f => f.Count);
            double maxVariance = recentlyDataList.Max(f => f.Max() - f.Min());
            //double threshold = Math.Abs(cedmsSettings.LineStopUpper + cedmsSettings.LineStopLower);
            //bool isStable = maxVariance < threshold;
            cedmsInspectionResult.ZeroingNum = zeroingNum;
            cedmsInspectionResult.ZeroingVariance = maxVariance;
            //cedmsInspectionResult.ZeroingStable = isStable;

            zeroingValues = recentlyDataList.ConvertAll(f => f.Average()).ToArray();
        }
        
        public override UniScanState GetNextState(DynMvp.InspData.InspectionResult inspectionResult)
        {
            CEDMS.Data.InspectionResult cedmsInspectionResult = (CEDMS.Data.InspectionResult)inspectionResult;

            CEDMSSettings cedmsSettings = (CEDMSSettings)CEDMSSettings.Instance();
            int zeroingNum = cedmsInspectionResult.ZeroingNum;

            if (zeroingNum < cedmsSettings.DataCountForZeroSetting || !cedmsInspectionResult.ZeroingStable)
                return this;
            else
                return new CEDMS.State.InspectionState(zeroingValues[1], zeroingValues[0], true);
        }
    }
}
