using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.InspData;
using UniEye.Base.Inspect;
using UniEye.Base.MachineInterface;
using UniScanM.Gloss.Data;
using UniScanM.Gloss.MachineIF;
using UniScanM.Gloss.Settings;
using UniScanM.State;

namespace UniScanM.Gloss.State
{
    public class InspectionState : UniScanState
    {
        public InspectionState() : base()
        {
        }
        public override bool IsTeachState => throw new NotImplementedException();

        public override UniScanState GetNextState(DynMvp.InspData.InspectionResult inspectionResult)
        {
            return this;
        }

        public override void OnProcess(ImageD imageD, DynMvp.InspData.InspectionResult inspectionResult, InspectOption inspectionOption = null)
        {
            LogHelper.Debug(LoggerType.Operation, "InspectionState::OnProcess");

            Gloss.Data.InspectionResult glossInspectionResult = (Gloss.Data.InspectionResult)inspectionResult;

            // PLC에 써줘야 할 내용이 있을 경우 코드 살려서 내용 만들 것
            //MachineIf machineIf = SystemManager.Instance().DeviceBox.MachineIf;
            //if (machineIf != null)
            //{
            //    if (SystemManager.Instance().InspectStarter is PLCInspectStarter)
            //    {
            //        MachineState state = ((PLCInspectStarter)SystemManager.Instance().InspectStarter).MelsecMonitor.State;
            //    }
            //}

            GlossSettings setting = GlossSettings.Instance() as GlossSettings;

            var avgGloss = glossInspectionResult.GlossScanData.AvgGloss;
            var minGloss = glossInspectionResult.GlossScanData.MinGloss;
            var maxGloss = glossInspectionResult.GlossScanData.MaxGloss;
            var minStopGloss = avgGloss * (1 - setting.ProfileLineStopRange / 100.0f);
            var maxStopGloss = avgGloss * (1 + setting.ProfileLineStopRange / 100.0f);
            var minWarnGloss = avgGloss * (1 - setting.ProfileLineWarningRange / 100.0f);
            var maxWarnGloss = avgGloss * (1 + setting.ProfileLineWarningRange / 100.0f);

            if (minGloss < minStopGloss || maxGloss > maxStopGloss)
                glossInspectionResult.Judgment = Judgment.Reject;
            else if (minGloss < minWarnGloss || maxGloss > maxWarnGloss)
                glossInspectionResult.Judgment = Judgment.Warn;
        }
        
        public override void PostProcess(DynMvp.InspData.InspectionResult inspectionResult)
        {
        }

        public override void PreProcess()
        {
        }

        protected override void Init()
        {
        }
    }
}
