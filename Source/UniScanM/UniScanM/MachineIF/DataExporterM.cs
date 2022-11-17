using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniScanM.MachineIF
{
    public abstract class DataExporterM : DynMvp.Data.DataExporter
    {
        //public abstract void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken);
        public abstract void SetVisionState();
    }
}
