using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DynMvp.Base;

namespace DynMvp.UI
{
    public interface ICoordTransform
    {
        float ScaleX { get; }
        float ScaleY { get; }
        bool InvertY { get; }
        Point Transform(Point pt);
        Point InverseTransform(Point pt);
        PointF Transform(PointF pt);
        PointF InverseTransform(PointF pt);
        Rectangle Transform(Rectangle rect);
        Rectangle InverseTransform(Rectangle rect);
        RotatedRect Transform(RotatedRect rect);
        RotatedRect InverseTransform(RotatedRect rect);
        Size Transform(Size size);
        Size InverseTransform(Size size);
        SizeF Transform(SizeF size);
        SizeF InverseTransform(SizeF size);
    }

    public class CoordTransformer : ICoordTransform
    {
        RectangleF srcRect;
        RectangleF displayRect;

        float drawOffsetX = 0;
        float drawOffsetY = 0;
        float scaleX = 1;
        float scaleY = 1;

        public virtual float ScaleX => this.scaleX;
        public virtual float ScaleY => this.scaleY;

        protected bool invertY;
        public bool InvertY
        {
            get { return invertY; }
            set { invertY = value; }
        }

        public CoordTransformer()
        {
        }

        public CoordTransformer(float scale)
        {
            if (scale == 0)
                scaleX = scaleY = 1;
            else
                scaleX = scaleY = scale;
        }

        public virtual Point Transform(Point pt)
        {
            PointF pointF = new PointF(pt.X, pt.Y);
            return Point.Truncate(Transform(pointF));
        }

        public virtual Point InverseTransform(Point pt)
        {
            Point newPt = new Point();
            newPt.X = (int)((float)pt.X - drawOffsetX);
            newPt.X = (int)((newPt.X / scaleX) + srcRect.Left);

            if (invertY == true)
                newPt.Y = (int)(displayRect.Bottom - ((float)pt.Y - drawOffsetY));
            else
                newPt.Y = (int)((float)pt.Y - drawOffsetY);
            newPt.Y = (int)((newPt.Y / scaleY) + srcRect.Top);

            return newPt;
        }

        public virtual PointF Transform(PointF pt)
        {
            PointF newPt = new PointF();
            newPt.X = ((pt.X - srcRect.Left) * scaleX);
            newPt.X = ((float)newPt.X + drawOffsetX);
            //newPt.Y = ((pt.Y - srcRect.Top) * scaleY);

            //if (invertY == true)
            //    newPt.Y = (displayRect.Bottom - newPt.Y) + drawOffsetY;
            //else
            //    newPt.Y = newPt.Y + drawOffsetY;

            if (invertY == true)
            {
                newPt.Y = ((srcRect.Bottom - pt.Y) * scaleY);
            }
            else
            {
                newPt.Y = ((pt.Y - srcRect.Top) * scaleY);
            }

            newPt.Y = newPt.Y + drawOffsetY;

            return newPt;
        }

        public virtual PointF InverseTransform(PointF pt)
        {
            PointF newPt = new PointF();
            newPt.X = ((float)pt.X - drawOffsetX);
            newPt.X = (newPt.X / scaleX) + srcRect.Left;

            //if (invertY == true)
            //    newPt.Y = displayRect.Bottom - (pt.Y - drawOffsetY);
            //else
            //    newPt.Y = pt.Y - drawOffsetY;
            //newPt.Y = (newPt.Y / scaleY) + srcRect.Top;


            newPt.Y = ((float)pt.Y - drawOffsetY);

            if (invertY == true)
            {
                newPt.Y = -(pt.Y / scaleY) + srcRect.Bottom;
            }
            else
            {
                newPt.Y = (pt.Y / scaleY) + srcRect.Top;
            }

            return newPt;
        }

        public virtual Rectangle Transform(Rectangle rect)
        {
            RotatedRect rotatedRect = new RotatedRect(rect, 0);
            return Transform(rotatedRect).ToRectangle();
        }

        public virtual Rectangle InverseTransform(Rectangle rect)
        {
            RotatedRect rotatedRect = new RotatedRect(rect, 0);
            return InverseTransform(rotatedRect).ToRectangle();

        }

        public virtual RectangleF Transform(RectangleF rect)
        {
            RotatedRect rotatedRect = new RotatedRect(rect, 0);
            return Transform(rotatedRect).ToRectangleF();
        }

        public virtual RectangleF InverseTransform(RectangleF rect)
        {
            RotatedRect rotatedRect = new RotatedRect(rect, 0);
            return InverseTransform(rotatedRect).ToRectangleF();

        }

        public virtual RotatedRect Transform(RotatedRect rect)
        {
            RotatedRect newRect = new RotatedRect();
            newRect.X = ((rect.X - srcRect.Left) * scaleX);
            newRect.X = ((float)newRect.X + drawOffsetX);

            if (invertY == true)
            {
                newRect.Y = ((srcRect.Bottom - rect.Bottom)  * scaleY);
            }
            else
            {
                newRect.Y = ((rect.Y - srcRect.Top) * scaleY);
            }

            newRect.Y = newRect.Y + drawOffsetY;
            newRect.Width = (rect.Width * scaleX);
            newRect.Height = (rect.Height * scaleY);
            newRect.Angle = rect.Angle;

            return newRect;
        }

        public virtual RotatedRect InverseTransform(RotatedRect rect)
        {
            RotatedRect newRect = new RotatedRect();

            newRect.Width = (rect.Width / scaleX);
            newRect.Height = (rect.Height / scaleY);

            newRect.X = ((float)rect.X - drawOffsetX);
            newRect.X = (newRect.X / scaleX) + srcRect.Left;
            newRect.Y = ((float)rect.Y - drawOffsetY);

            if (invertY == true)
            {
                newRect.Y = -(newRect.Y / scaleY) + srcRect.Bottom;
            }
            else
            {
                newRect.Y = (newRect.Y / scaleY) + srcRect.Top;
            }

            newRect.Angle = rect.Angle;

            return newRect;
        }

        public virtual Size Transform(Size size)
        {
            Size newSize = new Size();
            newSize.Width = (int)(size.Width * scaleX);
            newSize.Height = (int)(size.Height * scaleY);

            return newSize;
        }

        public virtual Size InverseTransform(Size size)
        {
            Size newSize = new Size();
            newSize.Width = (int)(size.Width / scaleX);
            newSize.Height = (int)(size.Height / scaleY); // 초기에 상하가 반전이 되어있었음

            //if (invertY == true)
            //    newSize.Height *= -1;

            return newSize;
        }

        public virtual SizeF Transform(SizeF size)
        {
            SizeF newSize = new SizeF();
            newSize.Width = size.Width * scaleX;
            newSize.Height = size.Height * scaleY;

            return newSize;
        }

        public virtual SizeF InverseTransform(SizeF size)
        {
            SizeF newSize = new SizeF();
            newSize.Width = size.Width / scaleX;
            newSize.Height = size.Height / scaleY;

            return newSize;
        }

        public void UpdateScale()
        {
            if (srcRect.IsEmpty == true || displayRect.IsEmpty == true)
                return;

            float srcAspectRatio = (float)srcRect.Width / srcRect.Height;
            float displayAspectRatio = (float)displayRect.Width / displayRect.Height;

            //if (lockWHZoomRatio==false)
            //{
            //    scaleX = (float)displayRect.Width / srcRect.Width;
            //    scaleY = (float)displayRect.Height / srcRect.Height;
            //    if (srcAspectRatio > displayAspectRatio)
            //        drawOffsetY = (displayRect.Height - (srcRect.Height * scaleY)) / 2;
            //    else
            //        drawOffsetX = (displayRect.Width - (srcRect.Width * scaleX)) / 2;
            //}
            //else
            { 
                if (srcAspectRatio > displayAspectRatio)
                {
                    scaleX = scaleY = (float)displayRect.Width / srcRect.Width;
                    float srcScaledHeight = srcRect.Height * scaleY;
                    drawOffsetY = (displayRect.Height - srcScaledHeight) / 2;
                }
                else
                {
                    scaleX = scaleY = (float)displayRect.Height / srcRect.Height;
                    float srcScaledWidth = srcRect.Width * scaleX;
                    drawOffsetX = (displayRect.Width - srcScaledWidth) / 2;
                }
            }
        }

        public void SetScale(float scaleX, float scaleY)
        {
            this.scaleX = scaleX;
            this.scaleY = scaleY;
        }

        public void SetSrcRect(RectangleF srcRect)
        {
            this.srcRect = srcRect;
            //UpdateScale();
        }

        public void SetDisplayRect(RectangleF displayRect)
        {
            this.displayRect = displayRect;
            //UpdateScale();
        }

        public virtual void ModifyScale(float modifyScaleX, float modifyScaleY)
        {
            this.scaleX *= modifyScaleX;
            this.scaleY *= modifyScaleY;
        }
    }
}
