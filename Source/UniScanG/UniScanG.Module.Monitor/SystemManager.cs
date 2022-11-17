using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Inspect;
using UniEye.Base.MachineInterface;
using UniEye.Base.Settings;
using UniScanG.Module.Monitor.Exporter;

namespace UniScanG.Module.Monitor
{
    internal class SystemManager : UniScanG.SystemManager
    {
        public override void BuildAlgorithmStrategy()
        {
            ImagingLibrary imagingLibrary = OperationSettings.Instance().ImagingLibrary;
            AlgorithmBuilder.AddStrategy(new AlgorithmStrategy(Vision.MyAlgorithm.TypeName, imagingLibrary, ""));

            base.BuildAlgorithmStrategy();
        }

        public override InspectRunner CreateInspectRunner()
        {
            Inspect.InspectRunner inspectRunner = new Inspect.InspectRunner();
            inspectRunner.Processer = new Processing.TeachProcesser();
            return inspectRunner;
        }

        public override InspectRunnerExtender GetInspectRunnerExtender()
        {
            return new Inspect.InspectRunnerExtender();
        }

        public override void InitializeDataExporter()
        {
            this.dataExporterList.Add(new MelsecDataExporter());
            this.dataExporterList.Add(new CSVDataExporter());
            this.dataExporterList.Add(new ExcelDataExporter());
        }
    }
}
