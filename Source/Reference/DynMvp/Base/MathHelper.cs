using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DynMvp.Base
{
    public class MathHelper
    {
        public static bool IsPrimitive(object o)
        {
            return IsNumeric(o) || IsBoolean(o);
        }

        public static bool IsNumeric(object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsBoolean(object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.Boolean:
                    return true;
                default:
                    return false;
            }
        }

        public static double arctan(double y, double x)
        {
            double value;
            if (x == 0)
            {
                if (y > 0)
                    value = Math.PI / 2;
                else
                    value = Math.PI * 3 / 2;
            }
            else
            {
                value = Math.Atan(y / x);
                if (x < 0)
                    value += Math.PI;
            }

            return value;
        }

        public static double DegToRad(double deg)
        {
            return deg / 180.0 * Math.PI;
        }

        public static double RadToDeg(double rad)
        {
            return rad * 180.0 / Math.PI;
        }

        public static double GetAngle360(Point basePt, Point point1, Point point2)
        {
            return GetAngle360(DrawingHelper.ToPointF(basePt), DrawingHelper.ToPointF(point1), DrawingHelper.ToPointF(point2));
        }

        public static double GetAngle360(PointF basePt, PointF point1, PointF point2)
        {
            double theta1 = arctan(basePt.Y - point1.Y, point1.X - basePt.X);
            double theta2 = arctan(basePt.Y - point2.Y, point2.X - basePt.X);

            double angle = RadToDeg(theta2 - theta1);
            if (angle < 0)
                angle = 360 + angle;

            return angle % 360;
        }

        public static Point Rotate(Point point, Point centerPt, double angleDegree)
        {
            return DrawingHelper.ToPoint(Rotate(DrawingHelper.ToPointF(point), DrawingHelper.ToPointF(centerPt), angleDegree));
        }

        public static PointF Rotate(PointF point, PointF centerPt, double angleDegree)
        {
            if (angleDegree == 0)
                return point;

            PointF tempPoint = new PointF(point.X, point.Y);

            tempPoint.X -= centerPt.X;
            tempPoint.Y -= centerPt.Y;

            tempPoint.Y *= -1;

            double angleRad = DegToRad(angleDegree);
            double X = (double)((tempPoint.X * Math.Cos(angleRad)) - (tempPoint.Y * Math.Sin(angleRad)));
            double Y = (double)((tempPoint.X * Math.Sin(angleRad)) + (tempPoint.Y * Math.Cos(angleRad)));

            tempPoint.X = (float)(X + centerPt.X);
            //            tempPoint.Y = Y + centerPt.Y;
            tempPoint.Y = (float)((Y * (-1)) + centerPt.Y);

            return tempPoint;
        }

        public static SizeF Rotate(Size size, double angleDegree)
        {
            SizeF tempSize = new SizeF();

            double angleRad = DegToRad(angleDegree);
            tempSize.Width = (float)((size.Width * Math.Cos(angleRad)) - (size.Height * Math.Sin(angleRad)));
            tempSize.Height = (float)((size.Width * Math.Sin(angleRad)) + (size.Height * Math.Cos(angleRad)));

            return tempSize;
        }

        public static SizeF Rotate(SizeF size, double angleDegree)
        {
            SizeF tempSize = new SizeF();

            double angleRad = DegToRad(angleDegree);
            tempSize.Width = (float)((size.Width * Math.Cos(angleRad)) - (size.Height * Math.Sin(angleRad)));
            tempSize.Height = (float)((size.Width * Math.Sin(angleRad)) + (size.Height * Math.Cos(angleRad)));

            return tempSize;
        }

        public static float GetLength(PointF pt1, PointF pt2)
        {
            float deltaX = pt2.X - pt1.X;
            float deltaY = pt2.Y - pt1.Y;

            return (float)Math.Sqrt((deltaX * deltaX) + (deltaY * deltaY));
        }

        public static float StdDev(IEnumerable<float> projDatas)
        {
            float result = 0;

            //System.IO.File.WriteAllText(@"D:\temp\projDatas.txt", string.Join(Environment.NewLine, projDatas));
            if (projDatas.Any())
            {
                double average = projDatas.Average();
                double sum = projDatas.Sum(d => Math.Pow(d - average, 2));
                result = (float)Math.Sqrt((sum) / (projDatas.Count() - 1));
            }
            return result;
        }

        public static void StdDev(IEnumerable<float> projDatas, out double average, out double stdDev)
        {
            average = double.NaN;
            stdDev = double.NaN;

            //System.IO.File.WriteAllText(@"D:\temp\projDatas.txt", string.Join(Environment.NewLine, projDatas));
            if (projDatas.Any())
            {
                double mean = projDatas.Average();
                double sum = projDatas.Sum(d => Math.Pow(d - mean, 2));

                average = mean;
                stdDev = (float)Math.Sqrt((sum) / (projDatas.Count() - 1));
            }
        }

        public static float GetLength(SizeF size)
        {
            return (float)Math.Sqrt((size.Width * size.Width) + (size.Height * size.Height));
        }

        public static float Bound(float value, float minValue, float maxValue)
        {
            return Math.Min(Math.Max(value, minValue), maxValue);
        }

        public static int Bound(int value, int minValue, int maxValue)
        {
            return Math.Min(Math.Max(value, minValue), maxValue);
        }

        public static float Median(List<int> list)
        {
            List<int> copiedList = new List<int>(list);
            copiedList.Sort();
            if (copiedList.Count % 2 == 1)
            {
                int midIdx = copiedList.Count / 2;
                int value = copiedList[midIdx];
                return value;
            }
            else
            {
                int midIdx = copiedList.Count / 2 - 1;
                List<int> subList = copiedList.GetRange(midIdx, 2);
                return (float)subList.Average();
            }
        }

        //public static TResult Clip<TNumber, TResult>(TNumber number, TNumber min, TNumber max)
        //    where TResult : struct
        //    where TNumber : struct
        //{
        //    double dNum = Convert.ToDouble(number);
        //    double dMax = Convert.ToDouble(max);
        //    double dMin = Convert.ToDouble(min);
        //    double dRes = Math.Min(dMax, Math.Max(dMin, dNum));
        //    return (TResult)Convert.ChangeType(dRes, typeof(TResult));
        //}

        //public static bool IsInRange(decimal value, decimal min, decimal max)
        //{
        //    return value >= max ? false : value < min ? false : true;
        //}
        //public static bool IsInRange(double value, double min, double max)
        //{
        //    return value >= max ? false : value < min ? false : true;
        //}
        public static bool IsInRange<T>(T value, T min, T max) where T: IComparable
        {
            return value.CompareTo(max) > 0 ? false : value.CompareTo(min) < 0 ? false : true;
        }

        public static T Clipping<T>(T value, T min, T max) where T: IComparable
        {
            return value.CompareTo(max) > 0 ? max : value.CompareTo(min) < 0 ? min : value;
        }

        public static void Regression1(IEnumerable<PointF> points, out double coff1, out double coff0)
        {
            Regression1(points.Select(f => (double)f.X), points.Select(f => (double)f.Y), out coff1, out coff0);
        }

        /// <summary>
        /// y = ax + b
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void Regression1(IEnumerable<double> xs, IEnumerable<double> ys, out double coff1, out double coff0)
        {
            double mX = xs.Average(f => f);
            double mY = ys.Average(f => f);

            IEnumerable<double> diffX = xs.Select(f => f - mX);
            IEnumerable<double> diffY = ys.Select(f => f - mY);

            double nom = diffX.Select((f, i) => f * diffY.ElementAt(i)).Sum();
            double denom = diffX.Sum(f => f * f);

            coff1 = nom / denom;
            coff0 = mY - coff1 * mX;
        }

        public static double Min(double a, double b)
        {
            bool aIsNan = double.IsNaN(a);
            bool bIsNan = double.IsNaN(b);
            if (aIsNan && bIsNan)
                return double.NaN;
            else if(aIsNan)
                return b;
            else if (bIsNan)
                return a;
            else
                return Math.Min(a,b);
        }
    }
}
