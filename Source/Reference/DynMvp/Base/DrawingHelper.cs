using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using DynMvp.UI;

namespace DynMvp.Base
{
    public class DrawingHelper
    {
        public static bool IsValid(Rectangle rectangle, Size srcImageSize)
        {
            if (rectangle.Left < 0 || rectangle.Top < 0)
                return false;

            if (rectangle.Right >= srcImageSize.Width || rectangle.Bottom >= srcImageSize.Height)
                return false;

            return true;
        }

        public static void Arrange(Rectangle rectangle, Size srcImageSize)
        {
            if (rectangle.X < 0)
                rectangle.X = 0;

            if (rectangle.Y < 0)
                rectangle.Y = 0;

            if ((rectangle.X + rectangle.Width) > srcImageSize.Width)
                rectangle.Width = srcImageSize.Width - rectangle.X;

            if ((rectangle.Y + rectangle.Height) > srcImageSize.Height)
                rectangle.Height = srcImageSize.Height - rectangle.Y;
        }

        public static PointF CenterPoint(PointF[] pointArray)
        {
            return new PointF(pointArray.Average(x => x.X), pointArray.Average(y => y.Y));
        }

        public static Point CenterPoint(Size size)
        {
            Point centerPt = new Point();
            centerPt.X = size.Width / 2;
            centerPt.Y = size.Height / 2;

            return centerPt;
        }

        public static PointF CenterPoint(SizeF size)
        {
            PointF centerPt = new PointF();
            centerPt.X = size.Width / 2;
            centerPt.Y = size.Height / 2;

            return centerPt;
        }

        public static Point CenterPoint(Rectangle rectangle)
        {
            Point centerPt = new Point();
            centerPt.X = rectangle.X + (rectangle.Width / 2);
            centerPt.Y = rectangle.Y + (rectangle.Height / 2);

            return centerPt;
        }

        public static PointF CenterPoint(RectangleF rectangle)
        {
            PointF centerPt = new PointF();
            centerPt.X = (rectangle.Left + rectangle.Right) / 2;
            centerPt.Y = (rectangle.Top + rectangle.Bottom) / 2;

            return centerPt;
        }

        public static PointF CenterPoint(RotatedRect rectangle)
        {
            PointF centerPt = new Point();
            centerPt.X = rectangle.X + rectangle.Width / 2;
            centerPt.Y = rectangle.Y + rectangle.Height / 2;

            return centerPt;
        }

        public static PointF CenterPoint(PointF pt1, PointF pt2)
        {
            PointF centerPt = new PointF();
            centerPt.X = (pt1.X + pt2.X) / 2;
            centerPt.Y = (pt1.Y + pt2.Y) / 2;

            return centerPt;
        }

        public static Point ToPoint(PointF pointF)
        {
            return new Point((int)pointF.X, (int)pointF.Y);
        }

        public static PointF ToPointF(Point point)
        {
            return new PointF(point.X, point.Y);
        }

        public static PointF ToPointF(SizeF size)
        {
            return new PointF(size.Width, size.Height);
        }
        
        public static SizeF ToSizeF(PointF point)
        {
            return new SizeF(point.X, point.Y);
        }

        public static SizeF ToSizeF(Point point)
        {
            return new SizeF(point.X, point.Y);
        }

        public static bool IntersectsWith(Rectangle rectangle, Point point)
        {
            return (rectangle.Left <= point.X) &&
                (rectangle.Top <= point.Y) &&
                (rectangle.Right > point.X) &&
                (rectangle.Bottom > point.Y);
        }

        public static bool IntersectsWith(RectangleF rectangle, PointF point)
        {
            return (rectangle.Left <= point.X) &&
                (rectangle.Top <= point.Y) &&
                (rectangle.Right > point.X) &&
                (rectangle.Bottom > point.Y);
        }

        public static byte ToGreyByte(Color color)
        {
            return (byte)((color.R + color.G + color.B) / 3);
        }

        public static Size ToSize(PointF point)
        {
            return new Size((int)point.X, (int)point.Y);
        }

        public static Size ToSize(Point point)
        {
            return new Size(point.X, point.Y);
        }

        public static Rectangle ToRect(RectangleF rectangleF)
        {
            return new Rectangle((int)rectangleF.Left, (int)rectangleF.Top, (int)rectangleF.Width, (int)rectangleF.Height);
        }

        public static RectangleF ToRectF(Rectangle rectangle)
        {
            return new RectangleF(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
        }

        public static RectangleF FromPoints(PointF point1, PointF point2)
        {
            return FromPoints(point1.X, point1.Y, point2.X, point2.Y);
        }

        public static RectangleF FromPoints(float x1, float y1, float x2, float y2)
        {
            float left = Math.Min(x1, x2);
            float right = Math.Max(x1, x2);
            float top = Math.Min(y1, y2);
            float bottom = Math.Max(y1, y2);

            return RectangleF.FromLTRB(left, top, right, bottom);
        }

        public static RectangleF FromCenterSize(PointF centerPt, SizeF size)
        {
            float w = size.Width;
            float h = size.Height;
            return new RectangleF(centerPt.X - w / 2, centerPt.Y - h / 2, w, h);
        }

        public static Rectangle FromCenterSize(Point centerPt, Size size)
        {
            return new Rectangle(centerPt.X - (size.Width / 2), centerPt.Y - (size.Height / 2), size.Width, size.Height);
        }

        public static Point[] GetPoints(Rectangle rectangle)
        {
            Point[] points = new Point[]
            {
                new Point(rectangle.Left, rectangle.Top),
                new Point(rectangle.Right, rectangle.Top),
                new Point(rectangle.Right, rectangle.Bottom),
                new Point(rectangle.Left, rectangle.Bottom)
            };
            return points;
        }

        public static PointF[] GetPoints(RectangleF rectangle, float angle)
        {
            PointF centerPt = DrawingHelper.CenterPoint(rectangle);

            PointF[] points = new PointF[4];

            points[0] = new PointF(rectangle.Left, rectangle.Top);
            points[1] = new PointF(rectangle.Right, rectangle.Top);
            points[2] = new PointF(rectangle.Right, rectangle.Bottom);
            points[3] = new PointF(rectangle.Left, rectangle.Bottom);

            for (int j = 0; j < 4; j++)
            {
                points[j] = MathHelper.Rotate(points[j], centerPt, angle);
            }

            return points;
        }

        public static RectangleF GetBoundRect(PointF[] points)
        {
            float left = points.Min( x => x.X );
            float right = points.Max(x => x.X);
            float top = points.Min(x => x.Y);
            float bottom = points.Max(x => x.Y);

            return RectangleF.FromLTRB(left, top, right, bottom);
        }

        public static SizeF Subtract(SizeF pt1, SizeF pt2)
        {
            return new SizeF(pt1.Width - pt2.Width, pt1.Height - pt2.Height);
        }

        public static SizeF Add(SizeF pt1, SizeF pt2)
        {
            return new SizeF(pt1.Width + pt2.Width, pt1.Height + pt2.Height);
        }

        public static Size Add(Size sz1, Size sz2)
        {
            return new Size(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
        }

        public static Size Add(Size pt1, int length)
        {
            return new Size(pt1.Width + length, pt1.Height + length);
        }

        public static Rectangle Mul(Rectangle rectangle, int mul)
        {
            return new Rectangle(rectangle.X * mul, rectangle.Y * mul, rectangle.Width * mul, rectangle.Height * mul);
        }

        public static Rectangle Mul(Rectangle rectangle, float mul)
        {
            return Rectangle.Round(new RectangleF(rectangle.X * mul, rectangle.Y * mul, rectangle.Width * mul, rectangle.Height * mul));
        }

        public static RectangleF Mul(RectangleF rectangle, float mul)
        {
            return new RectangleF(rectangle.X * mul, rectangle.Y * mul, rectangle.Width * mul, rectangle.Height * mul);
        }

        public static RectangleF Mul(RectangleF rectangle, SizeF mul)
        {
            return new RectangleF(rectangle.X * mul.Width, rectangle.Y * mul.Height, rectangle.Width * mul.Width, rectangle.Height * mul.Height);
        }

        public static Size Mul(Size size, int mul)
        {
            return new Size(size.Width * mul, size.Height * mul);
        }

        public static Size Mul(Size size, float mul)
        {
            return Size.Round(Mul(new SizeF(size), mul));
        }

        public static SizeF Mul(SizeF size, float mul)
        {
            return new SizeF(size.Width * mul, size.Height * mul);
        }
        public static SizeF Mul(SizeF size, SizeF mul)
        {
            return new SizeF(size.Width * mul.Width, size.Height * mul.Height);
        }

        public static Point Mul(Point point, int mul)
        {
            return new Point(point.X * mul, point.Y * mul);
        }

        public static Point Mul(Point point, float mul)
        {
            return Point.Round(new PointF(point.X * mul, point.Y * mul));
        }

        public static PointF Mul(PointF pointF, float mul)
        {
            return new PointF(pointF.X * mul, pointF.Y * mul);
        }

        public static PointF Mul(PointF pointF, SizeF mul)
        {
            return new PointF(pointF.X * mul.Width, pointF.Y * mul.Height);
        }

        public static SizeF Div(SizeF numerator, SizeF denominator)
        {
            if (denominator.Width == 0 || denominator.Height == 0)
                throw new DivideByZeroException();
            return new SizeF(numerator.Width / denominator.Width, numerator.Height / denominator.Height);
        }

        public static SizeF Div(SizeF numerator, float denominator)
        {
            if (denominator == 0)
                throw new DivideByZeroException();
            return new SizeF(numerator.Width / denominator, numerator.Height / denominator);

        }
        public static PointF Div(PointF numerator, SizeF denominator)
        {
            if (denominator.Width == 0 || denominator.Height == 0)
                throw new DivideByZeroException();
            return new PointF(numerator.X / denominator.Width, numerator.Y / denominator.Height);
        }

        public static Point Div(Point numerator, int denominator)
        {
            if (denominator == 0)
                throw new DivideByZeroException();
            return new Point(numerator.X / denominator, numerator.Y / denominator);
        }


        public static Size Div(Size numerator, int denominator)
        {
            if (denominator == 0)
                throw new DivideByZeroException();
            return new Size(numerator.Width / denominator, numerator.Height / denominator);
        }

        public static PointF Div(PointF numerator, PointF denominator)
        {
            if (denominator.X == 0 || denominator.Y == 0)
                throw new DivideByZeroException();
            return new PointF(numerator.X / denominator.X, numerator.Y / denominator.Y);
        }

        public static PointF Div(PointF numerator, float denominator)
        {
            if (denominator == 0)
                throw new DivideByZeroException();
            return new PointF(numerator.X / denominator, numerator.Y / denominator);
        }

        public static Rectangle Div(Rectangle numerator, int denominator)
        {
            if (denominator == 0)
                throw new DivideByZeroException();
            return new Rectangle(numerator.X / denominator, numerator.Y / denominator, numerator.Width / denominator, numerator.Height / denominator);
        }

        public static RectangleF Div(RectangleF numerator, float denominator)
        {
            if (denominator == 0)
                throw new DivideByZeroException();
            return new RectangleF(numerator.X / denominator, numerator.Y / denominator, numerator.Width / denominator, numerator.Height / denominator);
        }

        public static RectangleF Div(RectangleF numerator, SizeF denominator)
        {
            if (denominator.Width == 0 || denominator.Height == 0)
                throw new DivideByZeroException();
            return new RectangleF(numerator.X / denominator.Width, numerator.Y / denominator.Height, numerator.Width / denominator.Width, numerator.Height / denominator.Height);
        }

        public static RectangleF Offset(RectangleF rect, PointF offset, bool negative = false)
        {
            if (negative)
                return new RectangleF(rect.X - offset.X, rect.Y - offset.Y, rect.Width, rect.Height);
            return new RectangleF(rect.X + offset.X, rect.Y + offset.Y, rect.Width, rect.Height);
        }

        public static RectangleF Offset(RectangleF rect, SizeF offset)
        {
            return new RectangleF(rect.X + offset.Width, rect.Y + offset.Height, rect.Width, rect.Height);
        }

        public static Rectangle Offset(Rectangle rect, Point offset, bool negative = false)
        {
            if (negative)
                return new Rectangle(rect.X - offset.X, rect.Y - offset.Y, rect.Width, rect.Height);
            else
                return new Rectangle(rect.X + offset.X, rect.Y + offset.Y, rect.Width, rect.Height);
        }

        public static Rectangle Offset(Rectangle rect, Size offset)
        {
            return new Rectangle(rect.X + offset.Width, rect.Y + offset.Height, rect.Width, rect.Height);
        }

        public static PointF[] Offset(PointF[] ptArray, SizeF offset)
        {
            List<PointF> ptList = new List<PointF>();

            foreach (PointF pt in ptArray)
                ptList.Add(new PointF(pt.X + offset.Width, pt.Y + offset.Height));

            return ptList.ToArray();
        }

        public static PointF Subtract(PointF pt1, PointF pt2)
        {
            return new PointF(pt1.X - pt2.X, pt1.Y - pt2.Y);
        }

        public static Point Subtract(Point pt1, Point pt2)
        {
            return new Point(pt1.X - pt2.X, pt1.Y - pt2.Y);
        }

        public static Size Subtract(Size sz1, Size sz2)
        {
            return new Size(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
        }

        public static Point Add(Point pt1, Point pt2)
        {
            return new Point(pt1.X + pt2.X, pt1.Y + pt2.Y);
        }

        public static Point Add(Point pt1, int x, int y)
        {
            return new Point(pt1.X + x, pt1.Y + y);
        }

        public static PointF Add(PointF pt1, PointF pt2)
        {
            return new PointF(pt1.X + pt2.X, pt1.Y + pt2.Y);
        }

        public static List<PointF> ClipToFov(RotatedRect clipRegion, List<PointF> pointList)
        {
            List<PointF> newPointList = new List<PointF>();
            foreach(PointF point in pointList)
            {
                newPointList.Add(ClipToFov(clipRegion, point));
            }

            return newPointList;
        }

        public static PointF ClipToFov(RotatedRect clipRegion, PointF point)
        {
            PointF regionCenter = new PointF(clipRegion.Width / 2, clipRegion.Height / 2);
            PointF fovPoint = MathHelper.Rotate(point, regionCenter, clipRegion.Angle);
            fovPoint = PointF.Subtract(fovPoint, new SizeF(clipRegion.Width / 2, clipRegion.Height / 2));

            PointF regionFovCenter = DrawingHelper.CenterPoint(clipRegion);
            return PointF.Add(fovPoint, new SizeF(regionFovCenter.X, regionFovCenter.Y));
        }

        public static RotatedRect ClipToFov(RotatedRect clipRegion, RectangleF rectangle)
        {
            PointF rectCenter = ClipToFov(clipRegion, DrawingHelper.CenterPoint(rectangle));
            RectangleF clipRect = DrawingHelper.FromCenterSize(rectCenter, new SizeF(rectangle.Width, rectangle.Height));
            return new RotatedRect(clipRect, clipRegion.Angle);
        }

        public static PointF FovToClip(RotatedRect clipRegion, PointF point)
        {
            PointF regionFovCenter = DrawingHelper.CenterPoint(clipRegion);
            PointF clipPos = PointF.Subtract(point, new SizeF(regionFovCenter.X, regionFovCenter.Y));

            PointF regionCenter = new PointF(clipRegion.Width / 2, clipRegion.Height / 2);
            return MathHelper.Rotate(clipPos, regionCenter, -clipRegion.Angle);
        }

        public static RectangleF FovToClip(RotatedRect clipRegion, RotatedRect rectangle)
        {
            PointF rectCenter = FovToClip(clipRegion, DrawingHelper.CenterPoint(rectangle));
            return new RectangleF(rectCenter, new SizeF(rectangle.Width, rectangle.Height));
        }

        public static RectangleF GetUnionRect(RectangleF rectangle1, RectangleF rectangle2)
        {
            if (rectangle1.IsEmpty && rectangle2.IsEmpty)
                return new RectangleF();
            else if (rectangle1.IsEmpty)
                return rectangle2;
            else if (rectangle2.IsEmpty)
                return rectangle1;

            return RectangleF.Union(rectangle1, rectangle2);
        }

        public static bool IsCross(PointF point1, PointF point2, PointF point3, PointF point4)
        {
            PointF intersectionPoint = PointF.Empty;
            if (FindIntersection(point1, point2, point3, point4, ref intersectionPoint) == true)
            {
                float minX1 = Math.Min(point1.X, point2.X);
                float maxX1 = Math.Max(point1.X, point2.X);
                float minY1 = Math.Min(point1.Y, point2.Y);
                float maxY1 = Math.Max(point1.Y, point2.Y);

                float minX2 = Math.Min(point3.X, point4.X);
                float maxX2 = Math.Max(point3.X, point4.X);
                float minY2 = Math.Min(point3.Y, point4.Y);
                float maxY2 = Math.Max(point3.Y, point4.Y);

                float minX = Math.Max(minX1, minX2);
                float maxX = Math.Min(maxX1, maxX2);
                float minY = Math.Max(minY1, minY2);
                float maxY = Math.Min(maxY1, maxY2);

                if (minX <= intersectionPoint.X && maxX >= intersectionPoint.X && minY <= intersectionPoint.Y && maxY >= intersectionPoint.Y)
                    return true;
            }

            return false;
        }

        public static bool FindIntersection(PointF point1, PointF point2, PointF point3, PointF point4, ref PointF intersectionPoint)
        {
            if (point1.X == point2.X)
            {
                if (point3.X == point4.X)
                {
                    if (point1.X == point3.X)
                    {
                        intersectionPoint = new PointF(point1.X, 0);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    float angle = (point4.Y - point3.Y) / (point4.X - point3.X);
                    intersectionPoint = new PointF(point1.X, angle * (point1.X - point3.X) + point3.Y);
                    return true;
                }
            }
            else if (point3.X == point4.X)
            {
                float angle = (point2.Y - point1.Y) / (point2.X - point1.X);
                intersectionPoint = new PointF(point3.X, angle * (point3.X - point1.X) + point1.Y);
                return true;
            }
            else if (point1.Y == point2.Y)
            {
                if (point3.Y == point4.Y)
                {
                    if (point1.Y == point3.Y)
                    {
                        intersectionPoint = new PointF(0, point1.Y);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    float angle = (point4.Y - point3.Y) / (point4.X - point3.X);
                    intersectionPoint = new PointF((point1.Y - point3.Y) / angle + point3.X, point1.Y);
                    return true;
                }
            }
            else if (point3.Y == point4.Y)
            {
                float angle = (point2.Y - point1.Y) / (point2.X - point1.X);
                intersectionPoint = new PointF((point3.Y - point1.Y) / angle + point1.X, point3.Y);
                return true;
            }

            double den = (point1.X - point2.X) * (point3.Y - point4.Y) - (point1.Y - point2.Y) * (point3.X - point4.X);

            double num1 = (point1.X * point2.Y - point1.Y * point2.X);
            double num2 = (point3.X * point4.Y - point3.Y * point4.X);

            double numX = num1 * (point3.X - point4.X) - (point1.X - point2.X) * num2;
            double numY = num1 * (point3.Y - point4.Y) - (point1.Y - point2.Y) * num2;

            intersectionPoint = new PointF((float)(numX / den), (float)(numY / den));

            return true;
        }

        public static int GetArea(Rectangle rectangle)
        {
            return rectangle.Width * rectangle.Height;
        }
    }
}
