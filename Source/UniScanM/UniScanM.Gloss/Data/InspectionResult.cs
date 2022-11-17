using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanM.Gloss.Operation;

namespace UniScanM.Gloss.Data
{
    public class InspectionResult : UniScanM.Data.InspectionResult
    {
        #region 속성
        public GlossScanData GlossScanData { get; set; }

        public CalibrationData CalibrationData { get; set; }
        #endregion
    }
}
