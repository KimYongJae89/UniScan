using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DynMvp.InspData
{
    public class DbInspResultArchiver : InspResultArchiver
    {
        public void GetProbeResult(InspectionResult inspectionResult)
        {
            throw new NotImplementedException();
        }

        public InspectionResult Load(string dataPath)
        {
            throw new NotImplementedException();
        }

        public void Save(InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
