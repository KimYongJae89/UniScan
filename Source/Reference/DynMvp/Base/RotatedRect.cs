using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Globalization;

using DynMvp.Base;

namespace DynMvp.Base
{
    public class RotatedRectConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                                          System.Type destinationType)
        {
            if (destinationType == typeof(RotatedRect))
                return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                       CultureInfo culture,
                                       object value,
                                       System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is RotatedRect)
            {
                RotatedRect rotatedRect = (RotatedRect)value;
                return "";
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [TypeConverterAttribute(typeof(RotatedRectConverter))]
    public struct RotatedRect
    {
        RectangleF rectangle;

        [BrowsableAttribute(false)]
        public float Left { get { return X; } }

        [BrowsableAttribute(false)]
        public float Top { get { return Y; } }

        [BrowsableAttribute(false)]
        public float Right { get { return X + Width; } }

        [BrowsableAttribute(false)]
        public float Bottom { get { return Y + Height; } }

        public Rectangle ToRectangle() { return new Rectangle((int)X, (int)Y, (int)Width, (int)Height); }
        public RectangleF ToRectangleF() { return new RectangleF(X, Y, Width, Height); }

        public float X
        {
            get { return rectangle.X; }
            set { rectangle.X = value; }
        }
        public float Y
        {
            get { return rectangle.Y; }
            set { rectangle.Y = value; }
        }
        public float Width
        {
            get { return rectangle.Width; }
            set { rectangle.Width = value; }
        }
        public float Height
        {
            get { return rectangle.Height; }
            set { rectangle.Height = value; }
        }

        float angle;
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public RotatedRect(RotatedRect srcRect)
        {
            rectangle = srcRect.rectangle;
            this.angle = srcRect.Angle;
        }

        public RotatedRect(float x, float y, float width, float height, float angle)
        {
            rectangle = new RectangleF(x, y, width, height);
            this.angle = angle;
        }

        public RotatedRect(PointF point, SizeF size, float angle)
        {
            rectangle = new RectangleF(point, size);
            this.angle = angle;
        }

        public RotatedRect(RectangleF rectangle, float angle)
        {
            this.rectangle = rectangle;
            this.angle = angle;
        }

        public void FromLTRB(float left, float top, float right, float bottom)
        {
            rectangle = RectangleF.FromLTRB(Math.Min(left, right), Math.Min(top, bottom), Math.Max(left, right), Math.Max(top, bottom));
        }

        public void Offset(float offsetX, float offsetY)
        {
            rectangle.Offset(offsetX, offsetY);
        }

        public void Offset(SizeF offset)
        {
            rectangle.Offset(offset.Width, offset.Height);
        }

        public void Offset(PointF offset)
        {
            rectangle.Offset(offset.X, offset.Y);
        }

        public void Inflate(float x, float y)
        {
            rectangle.Inflate(x, y);
        }

        public void Inflate(float left, float top, float right, float bottom)
        {
            rectangle = RectangleF.FromLTRB(rectangle.Left - left, rectangle.Top - top, rectangle.Right + right, rectangle.Bottom + bottom);
        }

        [BrowsableAttribute(false)]
        public bool IsEmpty
        {
            get { return rectangle.IsEmpty; }
        }

        [BrowsableAttribute(false)]
        public static RotatedRect Empty {
            get { return new RotatedRect(0, 0, 0, 0, 0); }
        }

        public PointF[] GetPoints()
        {
            PointF centerPt = DrawingHelper.CenterPoint(rectangle);

            PointF[] points = new PointF[4]
            {
                new PointF(rectangle.Left, rectangle.Top),
                new PointF(rectangle.Right, rectangle.Top),
                new PointF(rectangle.Right, rectangle.Bottom),
                new PointF(rectangle.Left, rectangle.Bottom)
            };

            for (int j = 0; j < 4; j++)
                points[j] = MathHelper.Rotate(points[j], centerPt, angle);

            return points;
        }

        // 회전이 고려된 버전으로 Revision 필요
        public static RotatedRect Union(RotatedRect rect1, RotatedRect rect2)
        {
            return new RotatedRect(RectangleF.Union(rect1.GetBoundRect(), rect2.GetBoundRect()), 0);
        }

        //// 회전이 고려된 버전으로 Revision 필요
        //public static RotatedRect Intersect(RotatedRect rect1, RotatedRect rect2)
        //{
        //    return new RotatedRect(RectangleF.Intersect(rect1.GetBoundRect(), rect2.GetBoundRect()), 0);
        //}

        public static RotatedRect Inflate(RotatedRect rect, float x, float y)
        {
            RectangleF tempRect = rect.ToRectangleF();
            tempRect.Inflate(x, y);

            return new RotatedRect(tempRect, rect.Angle);
        }

        public RectangleF GetBoundRect()
        {
            PointF[] points = GetPoints();
            return DrawingHelper.GetBoundRect(points);
        }

        public override string ToString()
        {
            return rectangle.ToString() + " angle : " + angle.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj is RotatedRect)
            {
                RotatedRect rotatedRect = (RotatedRect)obj;
                bool ok1 = this.Angle == rotatedRect.Angle;
                bool ok2 = this.rectangle.Equals(rotatedRect.rectangle);

                return ok1&&ok2;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.rectangle.GetHashCode() ^ this.angle.GetHashCode();
        }

    }

}
