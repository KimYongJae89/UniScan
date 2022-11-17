using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Data.Vision;

namespace UniScanG.Gravure.Data
{
    class SheetPatternGroupG : UniScanG.Data.Vision.SheetPatternGroup
    {
        protected Image2D patternGroupImage = null;

        public SheetPatternGroupG() : base() { }

        public SheetPatternGroupG(List<PatternInfo> patternList) : base(patternList) { }
    }
}
