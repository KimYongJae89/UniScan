using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices.MotionController;
using DynMvp.InspData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniEye.Base;
using UniEye.Base.Data;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanM.MachineIF;

namespace UniScanM.EDMS.Operation
{
    public class InspectRunnerExtender: UniScanM.Operation.InspectRunnerExtender
    {
        protected override InspectionResult CreateInspectionResult()
        {
            return new EDMS.Data.InspectionResult();
        }

        protected override string GetInspectionNo()
        {
            UniScanM.Data.Production production = SystemManager.Instance().ProductionManager.CurProduction as UniScanM.Data.Production;
            return production == null ? "0" : (production.LastInspectionNo + 1).ToString();
        }

        public override InspectionResult BuildInspectionResult(string extendInfo = "")
        {
            LogHelper.Debug(LoggerType.Inspection, "CreateInspectionResult");

            UniScanM.Data.Production production = SystemManager.Instance().ProductionManager.CurProduction;
            Data.InspectionResult inspectionResult = (Data.InspectionResult)CreateInspectionResult();

            inspectionResult.ModelName = production.Name;
            inspectionResult.LotNo = production.LotNo;
            inspectionResult.Wroker = production.Worker;
            inspectionResult.InspectionTime = new TimeSpan(0);
            inspectionResult.ExportTime = new TimeSpan(0);
            inspectionResult.InspectionStartTime = DateTime.Now;
            inspectionResult.InspectionEndTime = DateTime.Now;

            inspectionResult.InspectionNo = GetInspectionNo();
            //inspectionResult.InspectionNo = "0";

            EDMS_Type Edmstype = EDMS_Type.EDMS_Gravure;
            Enum.TryParse(OperationSettings.Instance().SystemType, out Edmstype);

            if (Edmstype == EDMS_Type.EDMS_Gravure || Edmstype == EDMS_Type.EDMS_Dual)
            {
                // 롤의 현재 위치
                inspectionResult.RollDistance = SystemManager.Instance().InspectStarter.GetPosition();
            }
            else if(Edmstype == EDMS_Type.EDMS_Mobile)
            {
                if( SystemManager.Instance().InspectStarter.StartMode == StartMode.Auto)
                {
                    inspectionResult.RollDistance = SystemManager.Instance().InspectStarter.GetPosition();
                }
                else if (SystemManager.Instance().InspectStarter.StartMode == StartMode.Manual)
                {
                    // 롤의 현재 위치 (추정)
                    TimeSpan time = inspectionResult.InspectionStartTime - SystemManager.Instance().ProductionManager.CurProduction.StartTime;
                    double lineSpeed = SystemManager.Instance().InspectStarter.GetLineSpeedSv();
                    inspectionResult.RollDistance = (int)(SystemManager.Instance().ProductionManager.CurProduction.StartPosition + (time.TotalSeconds * lineSpeed / 60));

                    var state = ((PLCInspectStarter)(SystemManager.Instance().InspectStarter)).MelsecMonitor.State;
                    state.PvPosition = inspectionResult.RollDistance;

                    UniScanM.EDMS.Data.Model currentModel = SystemManager.Instance().CurrentModel as UniScanM.EDMS.Data.Model;
                    UniScanM.EDMS.Data.MobileParam mobieinfo = currentModel.Mobileparam;
                    if (state.PvPosition > mobieinfo.maxDistance)
                    {
                        //state.RewinderCut ^= true; 
                        state.PvPosition = 0;
                        inspectionResult.RollDistance = 0;
                        SystemManager.Instance().InspectStarter.PreStartInspect(false);
                    }
                }
            }// else if(Edmstype == EDMS_Type.EDMS_Mobile)

            string autoManual = SystemManager.Instance().InspectStarter.StartMode == StartMode.Auto ? "Auto" : "Manual";
            string productionName = SystemManager.Instance().ProductionManager.CurProduction.Name;

            //todo production name....
            inspectionResult.ResultPath = Path.Combine(
                PathSettings.Instance().Result,
                SystemManager.Instance().ProductionManager.CurProduction.StartTime.ToString("yyyyMMdd"),
                SystemManager.Instance().ProductionManager.CurProduction.Name,
                autoManual,
                SystemManager.Instance().ProductionManager.CurProduction.LotNo);

            Directory.CreateDirectory(inspectionResult.ResultPath);

            inspectionResult.ReportPath = inspectionResult.ResultPath.Replace(@"Result\", @"Report\");
            Directory.CreateDirectory(inspectionResult.ReportPath);

            LogHelper.Debug(LoggerType.Inspection, String.Format("Create Inspection Result: {0}", inspectionResult.InspectionNo));

            return inspectionResult;
        }

    }
}
