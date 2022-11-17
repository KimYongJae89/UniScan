using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Devices.MotionController;
using DynMvp.Vision;

namespace DynMvp.Data
{
    public class PositionAligner
    {
        bool fid1Error;
        public bool Fid1Error
        {
            get { return fid1Error; }
            set { fid1Error = value; }
        }

        bool fid2Error;
        public bool Fid2Error
        {
            get { return fid2Error; }
            set { fid2Error = value; }
        }

        public bool IsFidError()
        {
            return fid1Error || fid2Error;
        }

        private SizeF offset;
        public SizeF Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        private float angle;
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        private float desiredFiducialDistance;
        public float DesiredFiducialDistance
        {
            get { return desiredFiducialDistance; }
            set { desiredFiducialDistance = value; }
        }

        private float fiducialDistance;
        public float FiducialDistance
        {
            get { return fiducialDistance; }
            set { fiducialDistance = value; }
        }

        float fiducialDistanceOffset;
        public float FiducialDistanceOffset
        {
            get { return fiducialDistanceOffset; }
            set { fiducialDistanceOffset = value; }
        }

        bool stretchResult = true;
        public bool StretchResult
        {
            get { return stretchResult; }
            set { stretchResult = value; }
        }

        private PointF rotationCenter;
        public PointF RotationCenter
        {
            get { return rotationCenter; }
            set { rotationCenter = value; }
        }

        private PointF fovCenter;
        public PointF FovCenter
        {
            get { return fovCenter; }
            set { fovCenter = value; }
        }

        private Calibration calibration;
        public Calibration Calibration
        {
            get { return calibration; }
            set { calibration = value; }
        }

        bool globalCoordination;
        public bool GlobalCoordination
        {
            get { return globalCoordination; }
            set { globalCoordination = value; }
        }

        Size imageSize;
        public Size ImageSize
        {
            get { return imageSize; }
            set { imageSize = value; }
        }

        public bool StretchInspect()
        {
            if (Math.Abs(desiredFiducialDistance - fiducialDistance) > FiducialDistanceOffset)
                stretchResult = false;
            else
                stretchResult = true;

            return stretchResult;
        }

        public RotatedRect InvAlignFov(RotatedRect pelRect)
        {
            PointF centerPoint = InvAlignFov(DrawingHelper.CenterPoint(pelRect));

            SizeF halfSize = new SizeF(pelRect.Width / 2, pelRect.Height / 2);
            RectangleF rectangleF = new RectangleF(centerPoint.X - halfSize.Width, centerPoint.Y - halfSize.Height, pelRect.Width, pelRect.Height);
            RotatedRect newRect = new RotatedRect(rectangleF, 0); //  (pelRect.Angle - angle) % 360);

            return newRect;
        }

        public PointF InvAlignFov(PointF position)
        {
            return MathHelper.Rotate(position, fovCenter, -angle);
        }

        public RotatedRect AlignFov(RotatedRect pelRect)
        {
            PointF centerPoint = AlignFov(DrawingHelper.CenterPoint(pelRect));

            SizeF halfSize = new SizeF(pelRect.Width / 2, pelRect.Height / 2);
            RectangleF rectangleF = new RectangleF(centerPoint.X - halfSize.Width, centerPoint.Y - halfSize.Height, pelRect.Width, pelRect.Height);
            RotatedRect newRect = new RotatedRect(rectangleF, 0); //  (pelRect.Angle + angle) % 360);

            return newRect;
        }

        public PointF AlignFov(PointF position)
        {
            return MathHelper.Rotate(position, fovCenter, -angle);
        }

        public RotatedRect Align(RotatedRect rect)
        {
            PointF centerPoint = Align(DrawingHelper.CenterPoint(rect));

            SizeF halfSize = new SizeF(rect.Width / 2, rect.Height / 2);
            RectangleF rectangleF = new RectangleF(centerPoint.X - halfSize.Width, centerPoint.Y - halfSize.Height, rect.Width, rect.Height);
            RotatedRect newRect = new RotatedRect(rectangleF, (rect.Angle + angle)%360);

            return newRect;
        }

        public RotatedRect Align(RotatedRect rect, AxisPosition position)
        {
            if (globalCoordination == false)
                return Align(rect);

            PointF curPosition = position.ToPointF();
            PointF alignedPosition = Align(curPosition);

            PointF centerPt = new PointF(imageSize.Width/2.0f, imageSize.Height/2.0f);
            PointF realCenterPt = calibration.PixelToWorld(centerPt);
            SizeF realOffset = DrawingHelper.ToSize(DrawingHelper.Subtract(alignedPosition, curPosition));

            PointF realOffsetPt = PointF.Add(realCenterPt, realOffset);
            PointF offsetPt = calibration.WorldToPixel(realOffsetPt);

            SizeF offset = DrawingHelper.ToSize(DrawingHelper.Subtract(offsetPt, centerPt));
//            offset = new SizeF(offset.Width * (-1), offset.Height);

            RotatedRect newRect = rect;

            newRect.Offset(offset);

            return newRect;
        }

        public PointF Align(PointF position)
        {
            PointF newPosition = PointF.Add(position, offset.ToSize());
            newPosition = MathHelper.Rotate(newPosition, RotationCenter, angle);

            return newPosition;
        }
    }
}
