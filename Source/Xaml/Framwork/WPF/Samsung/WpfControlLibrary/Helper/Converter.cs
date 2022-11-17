using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfControlLibrary.Helper
{
    public static class Converter
    {
        public static System.Windows.Size ToSize(System.Drawing.Size size)
        {
            return new System.Windows.Size(size.Width, size.Height);
        }

        public static System.Windows.Size ToSize(System.Drawing.SizeF size)
        {
            return new System.Windows.Size(size.Width, size.Height);
        }

        public static System.Windows.Point ToPoint(System.Drawing.Point point)
        {
            return new System.Windows.Point(point.X, point.Y);
        }

        public static System.Windows.Point ToPoint(System.Drawing.PointF point)
        {
            return new System.Windows.Point(point.X, point.Y);
        }

        public static Rect ToRect(Rectangle rectangle)
        {
            return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static Rect ToRect(RectangleF rectangle)
        {
            return new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static System.Drawing.Rectangle ToRectangle(Rect rect)
        {
            return new System.Drawing.Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        public static System.Drawing.RectangleF ToRectangleF(Rect rect)
        {
            return new System.Drawing.RectangleF((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
        }
    }
}
