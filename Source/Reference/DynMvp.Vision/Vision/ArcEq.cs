using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DynMvp.Vision
{
    public class ArcEq
    {
        private PointF center = new PointF();
        public PointF Center
        {
            get { return center; }
            set { center = value; }
        }

        private float xRadius;
        public float XRadius
        {
            get { return xRadius; }
            set { xRadius = value; }
        }

        private float yRadius;
        public float YRadius
        {
            get { return yRadius; }
            set { yRadius = value; }
        }

        private float startAngle;
        public float StartAngle
        {
            get { return startAngle; }
            set { startAngle = value; }
        }

        private float endAngle;
        public float EndAngle
        {
            get { return endAngle; }
            set { endAngle = value; }
        }

        public ArcEq()
        {

        }

        public ArcEq(PointF center, float xRadius, float yRadius, float startAngle, float endAngle)
        {
            this.center = center;
            this.xRadius = xRadius;
            this.yRadius = yRadius;
            this.startAngle = startAngle;
            this.endAngle = endAngle;
        }

        public bool IsValid()
        {
            return xRadius > 0 && yRadius > 0;
        }
    }
}
