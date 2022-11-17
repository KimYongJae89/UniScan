using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Devices.Comm;
using DynMvp.InspData;
using DynMvp.Vision;
using UniScanS.Screen.Vision.Detector;
using UniScanS.Screen.Data;
using UniScanS.Vision.FiducialFinder;
using UniScanS.Common.Data;

namespace UniScanS.Data.Inspect
{
    internal class InspectorDataExporterS : DynMvp.Data.DataExporter
    {
        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            StringBuilder sb = new StringBuilder();

            foreach(KeyValuePair<string, AlgorithmResult> pair in inspectionResult.AlgorithmResultLDic)
            {
                string key = pair.Key;
                IExportable exportable = pair.Value as IExportable;
                exportable.Export(key, inspectionResult.ResultPath);
            }

            // Wirte File Need
            sb.ToString();
        }
    }

    internal class MonitorDataExporterS : DynMvp.Data.DataExporter
    {
        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, AlgorithmResult> pair in inspectionResult.AlgorithmResultLDic)
            {
                IExportable exportable = pair.Value as IExportable;
                exportable.Export(pair.Key, inspectionResult.ResultPath);
            }

            // Wirte File Need
            sb.ToString();
        }
    }
}
