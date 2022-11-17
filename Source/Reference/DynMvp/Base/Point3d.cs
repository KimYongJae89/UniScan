using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Base
{
    public class Point3d
    {
        double x;
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        double y;
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        double z;
        public double Z
        {
            get { return z; }
            set { z = value; }
        }

        public Point3d()
        {

        }

        public Point3d(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double GetAngle()
        {
            double angleRad = Math.Atan2(Y, X);
            return MathHelper.RadToDeg(angleRad);
        }

        public bool IsEmpty()
        {
            return x == 0 && y == 0 && z == 0;
        }
    }
}
