using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanM.Settings;
using UniEye.Base.MachineInterface;
using UniScanM.StillImage.Operation;
using UniEye.Base.Settings;
//using UniEye.Base.Inspect;

namespace UniScanM.StillImage
{
    enum StillImageVersion { Version_1_0_a, Version_1_1_a , Version_1_1_b }

    public class SystemManager : UniScanM.SystemManager
    {
        public override UniEye.Base.Inspect.InspectRunner CreateInspectRunner()
        {
            return new InspectRunner();
        }
        
        public override string[] GetSystemTypeNames()
        {
            return Enum.GetNames(typeof(StillImageVersion));
        }
        
        public override UniEye.Base.Inspect.InspectRunnerExtender GetInspectRunnerExtender()
        {
            return new InspectRunnerExtender();
        }

        public override void InitializeDataExporter()
        {
            dataExporterList.Add(new UniScanM.StillImage.Data.DataExporter());
            dataExporterList.Add(new UniScanM.StillImage.MachineIF.MachineIfDataExporter());
        }

        protected override void LoadAdditialSettings()
        {
            Settings.StillImageSettings.CreateInstance();
            Settings.StillImageSettings.Instance().Load();
        }

        public override void CreateInspectStarter()
        {
            //inspectStarter = new EncoderInspectStarter();
            this.inspectStarter = new PLCInspectStarter();
        }
    }
}
