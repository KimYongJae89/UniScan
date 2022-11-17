using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Data;

namespace UniScanG.Gravure.UI.Report.TransformControl
{
    public class CalcResult
    {
        public SizeF pelSize;
        public float dX, dY, dT, dL1, dL2, dW, dH;
        public OffsetObj[] cornorObjs;

        public CalcResult() { pelSize = SizeF.Empty; dX = dY = dT = dL1 = dL2 = dW = dH = float.NaN; cornorObjs = null; }

        internal static CalcResult GetAverage(CalcResult[] calcResults)
        {
            CalcResult calcResult = new CalcResult();
            calcResult.dX = CalcResult.GetAverage(calcResults, new Func<CalcResult, float>(f => f.dX));
            calcResult.dY = CalcResult.GetAverage(calcResults, new Func<CalcResult, float>(f => f.dY));

            calcResult.dT = CalcResult.GetAverage(calcResults, new Func<CalcResult, float>(f => f.dT));

            calcResult.dL1 = CalcResult.GetAverage(calcResults, new Func<CalcResult, float>(f => f.dL1));
            calcResult.dL2 = CalcResult.GetAverage(calcResults, new Func<CalcResult, float>(f => f.dL2));

            calcResult.dW = CalcResult.GetAverage(calcResults, new Func<CalcResult, float>(f => f.dW));
            calcResult.dH = CalcResult.GetAverage(calcResults, new Func<CalcResult, float>(f => f.dH));

            calcResult.pelSize.Width = CalcResult.GetAverage(calcResults, new Func<CalcResult, float>(f => f.pelSize.Width));
            calcResult.pelSize.Height = CalcResult.GetAverage(calcResults, new Func<CalcResult, float>(f => f.pelSize.Height));

            return calcResult;
        }

        private static float GetAverage(CalcResult[] calcResults, Func<CalcResult, float> selector)
        {
            List<float> dXList = calcResults.Select(selector).ToList();
            dXList.RemoveAll(f => float.IsNaN(f));
            if (dXList.Count == 0)
                return float.NaN;
            return (float)dXList.Average();
        }
    }
}
