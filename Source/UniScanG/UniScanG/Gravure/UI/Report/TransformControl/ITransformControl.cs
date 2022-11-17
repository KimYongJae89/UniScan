using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using UniScanG.Gravure.Data;

namespace UniScanG.Gravure.UI.Report.TransformControl
{
    interface ITransformControl
    {
        void ClearAll();
        void UpdateImage(Bitmap bitmap);
        void ZoomFit();
        void Update(List<Tuple<CalcResult, OffsetObj[]>> tupleList, float baseCircleRadUm);
        //void UpdateValue(CalcResult[] calcResults);
        //void UpdateFigure(List<OffsetObj[]> offsetObjsList, float baseCircleRadPx);
    }
}
