using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynMvp.UI;
using System.Drawing;
using DynMvp.Base;
using System.IO;
using UniScanM.CGInspector.Data;
using UniScanM.Data;

namespace UniScanM.CGInspector.Data
{
    public class InspectionResult : UniScanM.Data.InspectionResult
    {
        public override void Clear(bool clearImage = true)
        {
            base.Clear(clearImage);

            //teachResult?.Image.Dispose();
            //processResult?.Image.Dispose();
        }

        public override void UpdateJudgement()
        {

        }

        public override void AppendResultFigures(FigureGroup figureGroup, DynMvp.UI.FigureDrawOption option)
        {
            // useLocalCoord true: Image is Grabbed image
            // useLocalCoord false: Image is ROI image
            base.AppendResultFigures(figureGroup, option);
        }
    }
}

