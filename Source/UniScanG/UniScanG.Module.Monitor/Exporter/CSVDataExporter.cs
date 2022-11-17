using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.InspData;
using UniScanG.Module.Monitor.Data;
using UniScanG.Module.Monitor.MachineIF;

namespace UniScanG.Module.Monitor.Exporter
{
    public class CSVDataExporter : DynMvp.Data.DataExporter
    {
        DataArchiver dataArchiver = new DataArchiver();
        public CSVDataExporter()
        {

        }

        public override void Export(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            dataArchiver.Save(inspectionResult, cancellationToken);
        }
    }
}
