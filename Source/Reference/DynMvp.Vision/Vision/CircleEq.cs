using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DynMvp.Vision
{
    public class CircleEq
    {
        private PointF center = new PointF();
        public PointF Center
        {
            get { return center; }
            set { center = value; }
        }

        private float radius;
        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public CircleEq()
        {

        }

        public CircleEq(PointF center, float radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public bool IsValid()
        {
            return center.X != 0 && center.Y != 0 && radius > 0;
        }
    }
}
