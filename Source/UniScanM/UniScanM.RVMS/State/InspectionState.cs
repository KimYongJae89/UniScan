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
using UniScanM.RVMS.Operation;
using UniScanM.RVMS.Data;
using UniScanM.RVMS.Settings;
using UniEye.Base.Data;

namespace UniScanM.RVMS.State
{
    public class InspectionState : UniScanState
    {
        bool zeroingComplate;

        float manZeroingOffset;
        float gearZeroingOffset;

        public override bool IsTeachState
        {
            get { return zeroingComplate; }
        }

        public InspectionState(float gearZeroingOffset, float manZeroingOffset, bool zeroingComplate) : base()
        {
            this.gearZeroingOffset = gearZeroingOffset;
            this.manZeroingOffset = manZeroingOffset;
            this.zeroingComplate = zeroingComplate;
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

            RVMS.Data.InspectionResult rvmsInspectionResult = (RVMS.Data.InspectionResult)inspectionResult;

            //this.gearZeroingOffset = this.manZeroingOffset = 0;

            rvmsInspectionResult.ZeroingComplate = zeroingComplate;
            rvmsInspectionResult.GearZeroingValue = this.gearZeroingOffset;
            rvmsInspectionResult.ManZeroingValue = this.manZeroingOffset;

            MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            if (machineIf != null)
            {
                if (SystemManager.Instance().InspectStarter is PLCInspectStarter)
                {
                    MachineState state = ((PLCInspectStarter)SystemManager.Instance().InspectStarter).MelsecMonitor.State;
                    //todo zmsong
                    uint order = RVMSSettings.Instance().DecimalpointPl;
                    float divider = (float)Math.Pow(10,order);

                    rvmsInspectionResult.BeforePattern = new ScanData(inspectionResult.InspectionStartTime, (float)state.Rvms_BeforePattern / divider, (float)state.Rvms_BeforePattern / divider, 0);
                    rvmsInspectionResult.AffterPattern = new ScanData(inspectionResult.InspectionStartTime, (float)state.Rvms_AfterPattern / divider, (float)state.Rvms_AfterPattern / divider, 0);
                }
            }

            //if(MachineSettings.Instance().VirtualMode)
            //{
            //    rvmsInspectionResult.BeforePattern = new ScanData(inspectionResult.InspectionStartTime, (float)48512/ 100.0f, (float)48512 / 100.0f, (float)48512 / 100.0f);
            //    rvmsInspectionResult.AffterPattern = new ScanData(inspectionResult.InspectionStartTime, (float)48636 / 100.0f, (float)48636 / 100.0f, (float)48636 / 100.0f);
            //}

            if (rvmsInspectionResult.CurValueList.Count > 0)
            {
                rvmsInspectionResult.ManSide =
                     new ScanData(
                         inspectionResult.InspectionStartTime,
                         -(rvmsInspectionResult.CurValueList[0] - manZeroingOffset),
                         rvmsInspectionResult.CurValueList[0], manZeroingOffset, rvmsInspectionResult.FirstTime);

                if (rvmsInspectionResult.CurValueList.Count > 1)
                {
                    rvmsInspectionResult.GearSide =
                         new ScanData(
                             inspectionResult.InspectionStartTime,
                             -(rvmsInspectionResult.CurValueList[1] - gearZeroingOffset),
                             rvmsInspectionResult.CurValueList[1], gearZeroingOffset, rvmsInspectionResult.FirstTime);
                }
            }

            RVMSSettings setting = RVMSSettings.Instance() as RVMSSettings;

            //if (RVMSSettings.Instance().Setting.UseLineStop)
            //{
                if (rvmsInspectionResult.ManSide.Y > setting.LineStopUpper || rvmsInspectionResult.ManSide.Y < -setting.LineStopLower ||
                    rvmsInspectionResult.GearSide.Y > setting.LineStopUpper || rvmsInspectionResult.GearSide.Y < -setting.LineStopLower)
                    rvmsInspectionResult.Judgment = Judgment.Reject;
                else if (rvmsInspectionResult.ManSide.Y > setting.LineWarningUpper || rvmsInspectionResult.ManSide.Y < -setting.LineWarningLower ||
               rvmsInspectionResult.GearSide.Y > setting.LineWarningUpper || rvmsInspectionResult.GearSide.Y < -setting.LineWarningLower)
                rvmsInspectionResult.Judgment = Judgment.Warn;
            //}


            if (machineIf != null)
            {
                if (RVMSSettings.Instance().UseLineStopPL)
                {
                    //if (Math.Abs(rvmsInspectionResult.BeforePattern.Y) > setting.LineStopPL ||
                    //    Math.Abs(rvmsInspectionResult.AffterPattern.Y) > setting.LineStopPL)
                    //    rvmsInspectionResult.Judgment = Judgment.Reject;

                    float diff = Math.Abs(rvmsInspectionResult.BeforePattern.Y - rvmsInspectionResult.AffterPattern.Y);
                    if (diff > setting.LineStopPL)
                        rvmsInspectionResult.Judgment = Judgment.Reject;
                }
            }
        }

        public override void PostProcess(DynMvp.InspData.InspectionResult inspectionResult)
        {
        }

        public override UniScanState GetNextState(DynMvp.InspData.InspectionResult inspectionResult)
        {
            RVMS.Data.InspectionResult rvmsInspectionResult = (RVMS.Data.InspectionResult)inspectionResult;
        
            if (rvmsInspectionResult.ResetZeroing == true)
                return new ZeroingState();
            else
                return this;
        }
    }
}
