using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DynMvp.Vision
{
    interface ILineEquation
    {
        int Order { get; }
        double[] Coffs { get; }
        double Calculate(double x);
    }

    public class LineEq
    {
        PointF slope = new PointF();
        public PointF Slope
        {
            get { return slope; }
            set { slope = value; }
        }
        public float SlopeValue
        {
            get { return slope.Y / slope.X;  }
        }

        PointF pointOnLine = new PointF();
        public PointF PointOnLine
        {
            get { return pointOnLine; }
            set { pointOnLine = value; }
        }

        public LineEq()
        {

        }

        public LineEq(PointF pt1, PointF pt2)
        {
            slope.X = pt2.X - pt1.X;
            slope.Y = pt2.Y - pt1.Y;
            pointOnLine = pt1;
        }

        public bool IsValid()
        {
            return (slope.X != 0 && slope.Y != 0);
        }

        public float GetY(float valueX)
        {
            return SlopeValue * (valueX - pointOnLine.X) + pointOnLine.Y;
        }

        public static bool GetIntersectPoint(LineEq lineEq1, LineEq lineEq2, ref PointF point)
        {
            if (lineEq1.IsValid() == false && lineEq2.IsValid() == false)
                return false;

            if (lineEq1.Slope.X == 0 && lineEq2.Slope.X == 0)
                return false;

            if (lineEq1.Slope.X == 0)
            {
                point.X = lineEq1.PointOnLine.X;
            }
            else if (lineEq2.Slope.X == 0)
            {
                point.X = lineEq2.PointOnLine.X;
            }
            else
            {
                PointF p1 = lineEq1.PointOnLine;
                PointF p2 = lineEq2.PointOnLine;

                float s1 = lineEq1.SlopeValue;
                float s2 = lineEq2.SlopeValue;

                point.X = (s1 * p1.X - s2 * p2.X - p1.Y + p2.Y) / (s1 - s2);
            }

            point.Y = lineEq1.GetY(point.X);

            return true;
        }
    }
}
