using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScanM.EDMSW.Operation;
//using UniEye.Base.Inspect;

namespace UniScanM.EDMSW
{
    enum EDMS_Type { EDMS_Gravure, EDMS_Mobile, EDMS_Dual }

    public class SystemManager : UniScanM.SystemManager
    {
        public override string[] GetSystemTypeNames()
        {
            return Enum.GetNames(typeof(EDMS_Type));
        }

        public override UniEye.Base.Inspect.InspectRunner CreateInspectRunner()
        {
            //return new InspectRunner();
            return new InspectRunner_M();
        }
        
        public override UniEye.Base.Inspect.InspectRunnerExtender GetInspectRunnerExtender()
        {
            return new EDMSW.Operation.InspectRunnerExtender();
        }

        public override void InitializeDataExporter()
        {
            //dataExporterList.Add(new UniScanM.Data.InspectionResultDataExporter());
            dataExporterList.Add(new MachineIF.MachineIfDataExporter());
            dataExporterList.Add(new Data.ReportDataExporter());
        }

        protected override void LoadAdditialSettings()
        {
            EDMSW.Settings.EDMSSettings.CreateInstance();
            EDMSW.Settings.EDMSSettings.Instance().Load();
        }

        public override void InitializeAdditionalUnits()
        {
            base.InitializeAdditionalUnits();
        }

        public override void CreateInspectStarter()
        {
            this.inspectStarter = new PLCInspectStarter();
        }
    }
}
