using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Extensions
{
    public static class RectangleExtensions
    {
        public static Point RightTop(this Rectangle rectangle)
        {
            return new Point(rectangle.Right, rectangle.Top);
        }

        public static Rectangle Standardization(this Rectangle rectangle)
        {
            Point loc = rectangle.Location;
            Size size = rectangle.Size;
            if (size.Width < 0)
            {
                loc.X += size.Width;
                size.Width = -size.Width;
            }

            if (size.Height < 0)
            {
                loc.Y += size.Height;
                size.Height = -size.Height;
            }

            return new Rectangle(loc, size);
        }
    }
}
