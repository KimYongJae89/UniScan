using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanM.Settings;
using UniEye.Base.MachineInterface;
using UniScanM.CGInspector.Operation;
using UniEye.Base.Settings;
using DynMvp.Vision;
using UniScanM.CGInspector.Algorithm.Glass;
//using UniEye.Base.Inspect;

namespace UniScanM.CGInspector
{
    enum SystemVersion { Version_1_0_a }

    public class SystemManager : UniScanM.SystemManager
    {
        public override UniEye.Base.Inspect.InspectRunner CreateInspectRunner()
        {
            return new InspectRunner();
        }
        
        public override string[] GetSystemTypeNames()
        {
            return Enum.GetNames(typeof(SystemVersion));
        }
        
        public override UniEye.Base.Inspect.InspectRunnerExtender GetInspectRunnerExtender()
        {
            return new InspectRunnerExtender();
        }

        public override void BuildAlgorithmStrategy()
        {
            base.BuildAlgorithmStrategy();

            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(GlassAlgorithm.TypeName, OperationSettings.Instance().ImagingLibrary, ""));

        }

        public override void SelectAlgorithmStrategy()
        {
            base.SelectAlgorithmStrategy();

            AlgorithmBuilder.SetAlgorithmEnabled(GlassAlgorithm.TypeName, true);
        }

        public override void InitializeDataExporter()
        {
            dataExporterList.Add(new UniScanM.CGInspector.Data.DataExporter());
            dataExporterList.Add(new UniScanM.CGInspector.MachineIF.MachineIfDataExporter());
        }

        protected override void LoadAdditialSettings()
        {
            Settings.StillImageSettings.CreateInstance();
            Settings.StillImageSettings.Instance().Load();
        }
        
        public override void CreateInspectStarter()
        {
            this.inspectStarter = new CoverGlassInspectStarter();
        }
    }
}
