using System;

namespace UniScanM.Gloss.Data
{
    public class GlossData
    {
        #region 속성
        public float X { get; set; }

        public float Y { get; set; }

        public float Distance { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        #endregion

        #region 메서드
        public GlossData(float x, float y, float distance)
        {
            this.X = x;
            this.Y = y;
            this.Distance = distance;
        }

        public GlossData Clone()
        {
            return new GlossData(X, Y, Distance);
        }
        #endregion
    }
}