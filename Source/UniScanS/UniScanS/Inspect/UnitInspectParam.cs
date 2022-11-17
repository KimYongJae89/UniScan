using DynMvp.InspData;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Inspect;
using UniScanS.Inspect;

namespace UniScanS.Inspect
{
    public class UnitInspectParam : UnitInspectItem
    {
        ProcessBufferSet processBufferSet;
        public ProcessBufferSet ProcessBufferSet
        {
            get { return processBufferSet; }
            set { processBufferSet = value; }
        }

        public UnitInspectParam(AlgorithmInspectParam algorithmInspectParam, ProcessBufferSet processBufferSet, InspectionResult inspectionResult, InspectOption inspectionOption = null) : base(algorithmInspectParam, inspectionResult, inspectionOption)
        {
            this.processBufferSet = processBufferSet;
        }
    }
}
