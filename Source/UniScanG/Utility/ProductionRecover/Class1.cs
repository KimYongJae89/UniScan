using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using UniScanG.Gravure.Data;

namespace ProductionRecover
{
    struct BgWorkerParam
    {
        public string WorkingPath { get; }
        public bool IsDeepScan { get; }

        public BgWorkerParam(string workingPath, bool isDeepScan)
        {
            this.WorkingPath = workingPath;
            this.IsDeepScan = isDeepScan;
        }
    }
}
