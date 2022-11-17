using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.InspData;

namespace UniScanG.Module.Monitor.Exporter
{
    public class ExcelDataExporter : DynMvp.Data.DataExporter
    {
        public override void Export(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
        }
    }
}
