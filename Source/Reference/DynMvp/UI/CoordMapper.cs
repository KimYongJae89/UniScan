using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.UI
{
    public class CoordMapper : CoordTransformer
    {
        public override float ScaleX => this.matrix.Elements[0];
        public override float ScaleY => this.matrix.Elements[3];

        Matrix matrix;
        public Matrix Matrix
        {
            get { return matrix; }
        }

        Matrix invMatrix;
        public Matrix InvMatrix
        {
            get { return invMatrix; }
        }
        public override void ModifyScale(float modifyScaleX, float modifyScaleY)
        {
            this.matrix.Scale(modifyScaleX, modifyScaleY);
        }

        public CoordMapper(Matrix matrix)
        {
            this.matrix = matrix;

            invMatrix = matrix.Clone();
            if (invMatrix.IsInvertible)
                invMatrix.Invert();
        }

        public Point WorldToPixel(Point pt)
        {
            return Point.Round(WorldToPixel(new PointF(pt.X, pt.Y)));
        }

        public Point PixelToWorld(Point pt)
        {
            return Point.Round(PixelToWorld(new PointF(pt.X, pt.Y)));
        }

        public PointF WorldToPixel(PointF pt)
        {
            PointF[] ptArr = new PointF[] { pt };
            matrix.TransformPoints(ptArr);

            return ptArr[0];
        }

        public PointF PixelToWorld(PointF pt)
        {
            PointF[] ptArr = new PointF[] { pt };
            invMatrix.TransformPoints(ptArr);

            return ptArr[0];
        }

        public Rectangle WorldToPixel(Rectangle rect)
        {
            RotatedRect rotatedRect = new RotatedRect(rect, 0);
            return WorldToPixel(rotatedRect).ToRectangle();
        }

        public Rectangle PixelToWorld(Rectangle rect)
        {
            RotatedRect rotatedRect = new RotatedRect(rect, 0);
            return PixelToWorld(rotatedRect).ToRectangle();
        }

        public RectangleF WorldToPixel(RectangleF rect)
        {
            RotatedRect rotatedRect = new RotatedRect(rect, 0);
            return WorldToPixel(rotatedRect).ToRectangleF();
        }

        public RectangleF PixelToWorld(RectangleF rect)
        {
            RotatedRect rotatedRect = new RotatedRect(rect, 0);
            return PixelToWorld(rotatedRect).ToRectangleF();
        }

        public RotatedRect WorldToPixel(RotatedRect rect)
        {
            PointF topLeftPt = WorldToPixel(new PointF(rect.X, rect.Y));
            SizeF size = WorldToPixel(new SizeF(rect.Width, rect.Height));

            return new RotatedRect(topLeftPt, size, rect.Angle);
        }

        public RotatedRect PixelToWorld(RotatedRect rect)
        {
            PointF topLeftPt = PixelToWorld(new PointF(rect.X, rect.Y));
            SizeF size = PixelToWorld(new SizeF(rect.Width, rect.Height));

            return new RotatedRect(topLeftPt, size, rect.Angle);
        }

        public Size WorldToPixel(Size size)
        {
            PointF[] ptArr = new PointF[] { DrawingHelper.ToPointF(size) };
            matrix.TransformVectors(ptArr);

            return DrawingHelper.ToSize(ptArr[0]);
        }

        public Size PixelToWorld(Size size)
        {
            PointF[] ptArr = new PointF[] { DrawingHelper.ToPointF(size) };
            invMatrix.TransformVectors(ptArr);

            return DrawingHelper.ToSize(ptArr[0]);
        }

        public SizeF WorldToPixel(SizeF size)
        {
            PointF[] ptArr = new PointF[] { DrawingHelper.ToPointF(size) };
            matrix.TransformVectors(ptArr);

            return DrawingHelper.ToSizeF(ptArr[0]);
        }

        public SizeF PixelToWorld(SizeF size)
        {
            PointF[] ptArr = new PointF[] { DrawingHelper.ToPointF(size) };
            invMatrix.TransformVectors(ptArr);

            return DrawingHelper.ToSizeF(ptArr[0]);
        }

        public override Point Transform(Point pt)
        {
            return WorldToPixel(pt);
        }

        public override Point InverseTransform(Point pt)
        {
            return PixelToWorld(pt);
        }

        public override PointF Transform(PointF pt)
        {
            return WorldToPixel(pt);
        }

        public override PointF InverseTransform(PointF pt)
        {
            return PixelToWorld(pt);
        }

        public override Rectangle Transform(Rectangle rect)
        {
            return WorldToPixel(rect);
        }

        public override Rectangle InverseTransform(Rectangle rect)
        {
            return PixelToWorld(rect);
        }

        public override RectangleF Transform(RectangleF rect)
        {
            return WorldToPixel(rect);
        }

        public override RectangleF InverseTransform(RectangleF rect)
        {
            return PixelToWorld(rect);
        }

        public override RotatedRect Transform(RotatedRect rect)
        {
            return WorldToPixel(rect);
        }

        public override RotatedRect InverseTransform(RotatedRect rect)
        {
            return PixelToWorld(rect);
        }

        public override Size Transform(Size size)
        {
            return WorldToPixel(size);
        }

        public override Size InverseTransform(Size size)
        {
            return PixelToWorld(size);
        }

        public override SizeF Transform(SizeF size)
        {
            return WorldToPixel(size);
        }

        public override SizeF InverseTransform(SizeF size)
        {
            return PixelToWorld(size);
        }
    }
}
