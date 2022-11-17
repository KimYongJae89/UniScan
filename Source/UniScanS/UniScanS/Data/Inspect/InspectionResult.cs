using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanS.Data.Inspect
{
    internal class InspectionResult : DynMvp.InspData.InspectionResult
    {
        public InspectionResult()
        {
            this.algorithmResultDic = new Dictionary<string, AlgorithmResult>();
        }

        public void Add(string key, AlgorithmResult algorithmResult)
        {
            this.algorithmResultDic.Add(key, algorithmResult);
        }
    }
}
