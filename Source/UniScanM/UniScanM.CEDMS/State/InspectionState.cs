using DynMvp.Devices.MotionController;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScanM.Algorithm;
using UniScanM.Data;
using DynMvp.InspData;
using UniEye.Base;
using UniEye.Base.Inspect;
using DynMvp.Base;
using UniScanM.State;
using UniEye.Base.MachineInterface;
using UniScanM.CEDMS.Operation;
using UniScanM.CEDMS.Data;
using UniScanM.CEDMS.Settings;
using UniEye.Base.Data;

namespace UniScanM.CEDMS.State
{
    public class InspectionState : UniScanState
    {
        bool zeroingComplete;

        float inFeedZeroingOffset;
        float outFeedZeroingOffset;
        
        public override bool IsTeachState
        {
            get { return zeroingComplete; }
        }

        public InspectionState(float inFeedZeroingOffset, float outFeedZeroingOffset, bool zeroingComplete) : base()
        {
            this.inFeedZeroingOffset = inFeedZeroingOffset;
            this.outFeedZeroingOffset = outFeedZeroingOffset;
            this.zeroingComplete = zeroingComplete;
        }

        protected override void Init()
        {
        }

        public override void PreProcess()
        {

        }

        public override void OnProcess(ImageD imageD, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            LogHelper.Debug(LoggerType.Operation, "InspectionState::OnProcess");

            CEDMS.Data.InspectionResult cedmsInspectionResult = (CEDMS.Data.InspectionResult)inspectionResult;
            
            cedmsInspectionResult.ZeroingComplete = zeroingComplete;
            cedmsInspectionResult.InFeedZeroingValue = this.inFeedZeroingOffset;
            cedmsInspectionResult.OutFeedZeroingValue = this.outFeedZeroingOffset;

            MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            if (machineIf != null)
            {
                if (SystemManager.Instance().InspectStarter is PLCInspectStarter)
                {
                    MachineState state = ((PLCInspectStarter)SystemManager.Instance().InspectStarter).MelsecMonitor.State;
                }
            }

            if (cedmsInspectionResult.CurValueList.Count > 0)
            {
                cedmsInspectionResult.InFeed =
                     new CEDMSScanData(
                         cedmsInspectionResult.RollDistance,
                         -(cedmsInspectionResult.CurValueList[0] - inFeedZeroingOffset),
                         cedmsInspectionResult.CurValueList[0],
                         inFeedZeroingOffset);

                if (cedmsInspectionResult.CurValueList.Count > 1)
                {
                    cedmsInspectionResult.OutFeed =
                         new CEDMSScanData(
                             cedmsInspectionResult.RollDistance,
                             -(cedmsInspectionResult.CurValueList[1] - outFeedZeroingOffset),
                             cedmsInspectionResult.CurValueList[1],
                             outFeedZeroingOffset);
                }
            }

            CEDMSSettings setting = CEDMSSettings.Instance() as CEDMSSettings;

            //if (SEDMSSettings.Instance().Setting.UseLineStop)
            //{


            if (cedmsInspectionResult.OutFeed.Y > setting.OutFeedLineStopUpper || cedmsInspectionResult.OutFeed.Y < -setting.OutFeedLineStopLower ||
                cedmsInspectionResult.InFeed.Y > setting.InFeedLineStopUpper || cedmsInspectionResult.InFeed.Y < -setting.InFeedLineStopLower)

                cedmsInspectionResult.Judgment = Judgment.Reject;
            else if (cedmsInspectionResult.OutFeed.Y > setting.OutFeedLineWarningUpper || cedmsInspectionResult.OutFeed.Y < -setting.OutFeedLineWarningLower ||
           cedmsInspectionResult.InFeed.Y > setting.InFeedLineWarningUpper || cedmsInspectionResult.InFeed.Y < -setting.InFeedLineWarningLower)
                cedmsInspectionResult.Judgment = Judgment.Warn;


            //}
        }

        public override void PostProcess(DynMvp.InspData.InspectionResult inspectionResult)
        {
        }

        public override UniScanState GetNextState(DynMvp.InspData.InspectionResult inspectionResult)
        {
            CEDMS.Data.InspectionResult cedmsInspectionResult = (CEDMS.Data.InspectionResult)inspectionResult;
        
            if (cedmsInspectionResult.ResetZeroing == true)
                return new ZeroingState();
            else
                return this;
        }
    }
}
