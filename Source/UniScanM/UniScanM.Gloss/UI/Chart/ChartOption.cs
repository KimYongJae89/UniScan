using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanM.Gloss.UI.Chart
{
    public class ChartOption
    {
        #region 생성자
        public ChartOption() { }

        public ChartOption(bool xInvert, bool yInvert, bool autoScaleY)
        {
            this.XInvert = xInvert;
            this.YInvert = yInvert;
            this.AutoScaleY = autoScaleY;
        }
        #endregion

        #region 속성
        public bool XInvert { get; set; }

        public bool YInvert { get; set; }

        public bool AutoScaleY { get; set; } = true;
        #endregion
    }
}
