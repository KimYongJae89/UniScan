using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.ComponentModel;
using System.Globalization;

using DynMvp.Base;
using System.Drawing.Imaging;

namespace DynMvp.UI
{
    public enum FigureType
    {
        None = -1, Group, Grid, Rectangle, Line, Ellipse, Oblong, Polygon, Text, Image, Cross, XRect, Custom
    }

    public class FigureFactory
    {
        public static Figure Create(string figureTypeName)
        {
            FigureType figureType = (FigureType)Enum.Parse(typeof(FigureType), figureTypeName);
            return Create(figureType);
        }

        public static Figure Create(FigureType figureType)
        {
            Figure figure;
            switch (figureType)
            {
                case FigureType.Rectangle:
                    figure = new RectangleFigure();
                    break;
                case FigureType.Text:
                    figure = new TextFigure();
                    break;
                case FigureType.Image:
                    figure = new ImageFigure();
                    break;
                case FigureType.Ellipse:
                    figure = new EllipseFigure();
                    break;
                case FigureType.Line:
                    figure = new LineFigure();
                    break;
                case FigureType.Group:
                    figure = new FigureGroup();
                    break;
                case FigureType.Cross:
                    figure = new CrossFigure();
                    break;
                case FigureType.Polygon:
                    figure = new PolygonFigure();
                    break;
                case FigureType.Oblong:
                    figure = new OblongFigure();
                    break;
                default:
                    throw new InvalidTypeException();
            }

            return figure;
        }
    }

    public interface ITrackRegion
    {
        TrackPos TrackPos { get; }

        void AddGraphic(GraphicsPath gp);
    }

    public class TrackRectangle : ITrackRegion
    {
        private Rectangle rectangle;

        private TrackPos trackPos = new TrackPos();
        public TrackPos TrackPos
        {
            get { return trackPos; }
        }

        public TrackRectangle(int X, int Y, int width, int height, TrackPosType trackPosType)
        {
            this.rectangle = new Rectangle(X, Y, width, height);
            this.trackPos.PosType = trackPosType;
        }

        public TrackRectangle(float X, float Y, float width, float height, TrackPosType trackPosType)
        {
            this.rectangle = new Rectangle((int)X, (int)Y, (int)width, (int)height);
            this.trackPos.PosType = trackPosType;
        }

        public TrackRectangle(int X, int Y, int width, int height, TrackPosType trackPosType, int index)
        {
            this.rectangle = new Rectangle(X, Y, width, height);
            this.trackPos.PosType = trackPosType;
            this.trackPos.PolygonIndex = index;
        }

        public TrackRectangle(float X, float Y, float width, float height, TrackPosType trackPosType, int index)
        {
            this.rectangle = new Rectangle((int)X, (int)Y, (int)width, (int)height);
            this.trackPos.PosType = trackPosType;
            this.trackPos.PolygonIndex = index;
        }

        public void AddGraphic(GraphicsPath gp)
        {
            gp.AddRectangle(rectangle);
        }
    }

    public class TrackPolygon : ITrackRegion
    {
        List<PointF> pointList = new List<PointF>();
        public List<PointF> PointList
        {
            get { return pointList; }
            set { pointList = value; }
        }

        private TrackPos trackPos = new TrackPos();
        public TrackPos TrackPos
        {
            get { return trackPos; }
        }

        public TrackPolygon(List<PointF> pointList, TrackPosType trackPosType, int index)
        {
            this.trackPos.PosType = trackPosType;
            this.trackPos.PolygonIndex = index;
            this.pointList = pointList;
        }

        public void AddGraphic(GraphicsPath gp)
        {
            gp.AddPolygon(pointList.ToArray());
        }
    }

    public class FigureDrawOption
    {
        public bool useTargetCoord = true;

        public static FigureDrawOption Default
        {
            get { return new FigureDrawOption() { useTargetCoord = true }; }
        }
    }

    public abstract class Figure : ICloneable
    {
        public string Name { get => this.name; set => this.name = value; }
        private string name;

        public string Id { get => this.id; set => this.id = value; }
        private string id;

        public object Tag { get => this.tag; set => this.tag = value; }
        private object tag;

        public FigureType Type { get => this.type; }
        protected FigureType type;

        public bool Selectable { get => this.selectable; set => this.selectable = value; }
        private bool selectable = true;

        public bool Deletable { get => this.deletable; set => this.deletable = value; }
        private bool deletable = true;

        public bool Movable { get => this.movable; set => this.movable = value; }
        private bool movable = true;

        public bool Resizable { get => this.resizable; set => this.resizable = value; }
        private bool resizable = true;

        public bool Visible { get => this.visible; set => this.visible = value; }
        private bool visible = true;

        public FigureProperty FigureProperty { get => this.figureProperty; set => this.figureProperty = value; }
        protected FigureProperty figureProperty;

        public virtual Brush TempBrush { get => this.tempBrush; set => this.tempBrush = value; }
        protected Brush tempBrush;

        public int Thickness { get => this.thickness; set => this.thickness = value; }
        private int thickness;

        public Figure(string figurePropertyName = "")
        {
            if (String.IsNullOrEmpty(figurePropertyName) == true)
                figureProperty = new FigureProperty();
            else
                figureProperty = FigurePropertyPool.Instance().GetFigureProperty(figurePropertyName);
        }

        public Figure(Pen pen, Brush brush)
        {
            figureProperty = new FigureProperty();
            figureProperty.Pen = pen;
            figureProperty.Brush = brush;
        }

        public Figure(Font font, Color textColor, StringAlignment stringAlignment)
        {
            figureProperty = new FigureProperty();
            figureProperty.Font = font;
            figureProperty.TextColor = textColor;
            figureProperty.Alignment = stringAlignment;
        }

        public abstract object Clone();
        public abstract void Inflate(int w, int h);

        public virtual void Copy(Figure srcFigure)
        {
            id = srcFigure.id;
            name = srcFigure.name;
            tag = srcFigure.tag;
            visible = srcFigure.visible;

            if (String.IsNullOrEmpty(figureProperty.Name) == true)
                figureProperty = srcFigure.figureProperty.Clone();
            else
                figureProperty = srcFigure.figureProperty;

            movable = srcFigure.movable;
            selectable = srcFigure.selectable;
        }

        public virtual void ResetTempProperty()
        {
            if (tempBrush != null)
            {
                tempBrush = null;
            }
        }

        public virtual void Load(XmlElement figureElement)
        {
            id = XmlHelper.GetValue(figureElement, "Id", "");
            tag = XmlHelper.GetValue(figureElement, "Tag", "");
            visible = Convert.ToBoolean(XmlHelper.GetValue(figureElement, "Visible", "True"));
            selectable = Convert.ToBoolean(XmlHelper.GetValue(figureElement, "Selectable", "True"));
            movable = Convert.ToBoolean(XmlHelper.GetValue(figureElement, "Movable", "True"));
            resizable = Convert.ToBoolean(XmlHelper.GetValue(figureElement, "Resizable", "True"));
            name = XmlHelper.GetValue(figureElement, "Name", "");

            string figurePropertyName = XmlHelper.GetValue(figureElement, "FigurePropertyName", "");
            if (String.IsNullOrEmpty(figurePropertyName) == true)
            {
                XmlElement figurePropertyElement = figureElement["FigureProperty"];
                if (figurePropertyElement != null)
                {
                    figureProperty = new FigureProperty();
                    figureProperty.Load(figurePropertyElement);
                }
            }
            else
            {
                figureProperty = FigurePropertyPool.Instance().GetFigureProperty(figurePropertyName);
            }
        }

        public virtual void Save(XmlElement figureElement)
        {
            XmlHelper.SetValue(figureElement, "Id", id);

            if (tag != null)
            {
                string tagStr = tag as string;
                if (tagStr != null)
                    XmlHelper.SetValue(figureElement, "Tag", tagStr);
            }
            XmlHelper.SetValue(figureElement, "Type", type.ToString());
            XmlHelper.SetValue(figureElement, "Visible", visible.ToString());
            XmlHelper.SetValue(figureElement, "Selectable", selectable.ToString());
            XmlHelper.SetValue(figureElement, "Movable", movable.ToString());
            XmlHelper.SetValue(figureElement, "Resizable", resizable.ToString());
            XmlHelper.SetValue(figureElement, "Name", name);

            if (String.IsNullOrEmpty(figureProperty.Name) == true)
            {
                XmlElement figurePropertyElement = figureElement.OwnerDocument.CreateElement("", "FigureProperty", "");
                figureElement.AppendChild(figurePropertyElement);

                figureProperty.Save(figurePropertyElement);

            }
            else
            {
                XmlHelper.SetValue(figureElement, "FigurePropertyName", figureProperty.Name);
            }
        }

        public virtual List<ITrackRegion> GetTrackerRegionList(ICoordTransform coordTransformer, bool rotationLocked)
        {
            int trackerHalfSize = Configuration.TrackerSize / 2;
            int trackerSize = Configuration.TrackerSize;

            RotatedRect rect = GetRectangle();
            if (coordTransformer != null)
                rect = coordTransformer.Transform(rect);

            Point centerPt = DrawingHelper.ToPoint(DrawingHelper.CenterPoint(rect));

            List<ITrackRegion> rectangleList = new List<ITrackRegion>();

            if (tag is ITrackTarget)
            {
                ITrackTarget trackTarget = (ITrackTarget)tag;

                if (trackTarget.IsSizable())
                {
                    rectangleList.Add(new TrackRectangle(rect.Left - trackerHalfSize, rect.Top - trackerHalfSize, trackerSize, trackerSize, TrackPosType.LeftTop));
                    rectangleList.Add(new TrackRectangle(rect.Right - trackerHalfSize, rect.Top - trackerHalfSize, trackerSize, trackerSize, TrackPosType.RightTop));
                    rectangleList.Add(new TrackRectangle(rect.Left - trackerHalfSize, rect.Bottom - trackerHalfSize, trackerSize, trackerSize, TrackPosType.LeftBottom));
                    rectangleList.Add(new TrackRectangle(rect.Right - trackerHalfSize, rect.Bottom - trackerHalfSize, trackerSize, trackerSize, TrackPosType.RightBottom));
                }

                if (trackTarget.IsRotatable() || rotationLocked == false)
                    rectangleList.Add(new TrackRectangle(rect.Right - trackerHalfSize, centerPt.Y - trackerHalfSize, trackerSize, trackerSize, TrackPosType.Rotate));

                if (trackTarget.IsContainer())
                    rectangleList.Add(new TrackRectangle(rect.Left + trackerSize * 2, rect.Top - trackerHalfSize, trackerSize * 2, trackerSize * 2, TrackPosType.Move));
                else
                    rectangleList.Add(new TrackRectangle(rect.Left, rect.Top, rect.Width, rect.Height, TrackPosType.Inner));
            }
            else
            {
                if (resizable == true)
                {
                    rectangleList.Add(new TrackRectangle(rect.Left - trackerHalfSize, rect.Top - trackerHalfSize, trackerSize, trackerSize, TrackPosType.LeftTop));
                    rectangleList.Add(new TrackRectangle(rect.Right - trackerHalfSize, rect.Top - trackerHalfSize, trackerSize, trackerSize, TrackPosType.RightTop));
                    rectangleList.Add(new TrackRectangle(rect.Left - trackerHalfSize, rect.Bottom - trackerHalfSize, trackerSize, trackerSize, TrackPosType.LeftBottom));
                    rectangleList.Add(new TrackRectangle(rect.Right - trackerHalfSize, rect.Bottom - trackerHalfSize, trackerSize, trackerSize, TrackPosType.RightBottom));
                }
                else
                {

                }

                if (rotationLocked == false)
                    rectangleList.Add(new TrackRectangle(rect.Right - trackerHalfSize, centerPt.Y - trackerHalfSize, trackerSize, trackerSize, TrackPosType.Rotate));

                rectangleList.Add(new TrackRectangle(rect.Left, rect.Top, rect.Width, rect.Height, TrackPosType.Inner));
            }

            return rectangleList;
        }

        public TrackPos GetTrackPos(PointF point, ICoordTransform coordTransformer, bool rotationLocked, ref int polygonIndex)
        {
            RotatedRect transformRect = GetRectangle();
            if (coordTransformer != null)
                transformRect = coordTransformer.Transform(transformRect);

            List<ITrackRegion> trackerRegionList = GetTrackerRegionList(coordTransformer, rotationLocked);

            polygonIndex = 0;

            foreach (ITrackRegion trackerRegion in trackerRegionList)
            {
                GraphicsPath gp = new GraphicsPath();

                trackerRegion.AddGraphic(gp);

                Matrix rotationTransform = new Matrix(1, 0, 0, 1, 0, 0);
                rotationTransform.RotateAt(-transformRect.Angle, DrawingHelper.CenterPoint(transformRect));
                gp.Transform(rotationTransform);

                if (gp.IsVisible(point))
                {
                    return trackerRegion.TrackPos;
                }
            }

            // Line 일 경우, Outline을 찾는다.
            if (PtInOutline(point, coordTransformer) == true)
                return new TrackPos(TrackPosType.Inner, 0);

            return new TrackPos(TrackPosType.None, 0);
        }

        public virtual RotatedRect GetTrackingRect(TrackPos trackPos, SizeF offset, bool rotationLocked)
        {
            RotatedRect rectangle = GetRectangle();

            PointF centerPt = DrawingHelper.CenterPoint(rectangle);

            SizeF floatOffset = MathHelper.Rotate(offset, rectangle.Angle);
            SizeF floatOffset2 = MathHelper.Rotate(offset, -rectangle.Angle);

            //Debug.WriteLine(StringManager.GetString(this.GetType().FullName, "float offset : ") + floatOffset.ToString());
            //Debug.Flush();

            switch (trackPos.PosType)
            {
                case TrackPosType.LeftTop:
                    rectangle.X += floatOffset.Width;
                    rectangle.Width -= floatOffset.Width;
                    rectangle.Y += floatOffset.Height;
                    rectangle.Height -= floatOffset.Height;
                    break;
                case TrackPosType.Left:
                    rectangle.X += floatOffset.Width;
                    rectangle.Width -= floatOffset.Width;
                    break;
                case TrackPosType.RightTop:
                    rectangle.Width += floatOffset.Width;
                    rectangle.Y += floatOffset.Height;
                    rectangle.Height -= floatOffset.Height;
                    break;
                case TrackPosType.Top:
                    rectangle.Y += floatOffset.Height;
                    rectangle.Height -= floatOffset.Height;
                    break;
                case TrackPosType.RightBottom:
                    rectangle.Width += floatOffset.Width;
                    rectangle.Height += floatOffset.Height;
                    break;
                case TrackPosType.Right:
                    rectangle.Width += floatOffset.Width;
                    break;
                case TrackPosType.LeftBottom:
                    rectangle.X += floatOffset.Width;
                    rectangle.Width -= floatOffset.Width;
                    rectangle.Height += floatOffset.Height;
                    break;
                case TrackPosType.Bottom:
                    rectangle.Height += floatOffset.Height;
                    break;
                case TrackPosType.Rotate:
                    {
                        if (rotationLocked == false)
                        {
                            PointF orgRotateTrackPos = new PointF(rectangle.Right, centerPt.Y);
                            PointF curRotateTrackPos = MathHelper.Rotate(orgRotateTrackPos, centerPt, rectangle.Angle);
                            curRotateTrackPos += offset;

                            rectangle.Angle = (float)MathHelper.GetAngle360(centerPt, orgRotateTrackPos, curRotateTrackPos);
                        }
                    }
                    break;
                case TrackPosType.Move:
                case TrackPosType.Inner:
                    rectangle.X += floatOffset.Width;
                    rectangle.Y += floatOffset.Height;
                    break;
            }

            PointF newCenterPt = DrawingHelper.CenterPoint(rectangle);
            PointF newRotatedCenterPt = MathHelper.Rotate(newCenterPt, centerPt, rectangle.Angle);
            SizeF centerOffset = new SizeF(newRotatedCenterPt.X - newCenterPt.X, newRotatedCenterPt.Y - newCenterPt.Y);

            rectangle.Offset(centerOffset);

            return rectangle;
        }

        public virtual void TrackMove(TrackPos trackPos, SizeF offset, bool rotationLocked)
        {
            RotatedRect rectangle = GetTrackingRect(trackPos, offset, rotationLocked);

            SetRectangle(rectangle);
        }

        public abstract void SetRectangle(RotatedRect rectangle);

        public virtual void DrawSelection(Graphics g, ICoordTransform coordTransformer, bool rotationLocked)
        {
            List<ITrackRegion> trackerRegionList = GetTrackerRegionList(coordTransformer, rotationLocked);

            RotatedRect transformRect = GetRectangle();
            if (coordTransformer != null)
                transformRect = coordTransformer.Transform(transformRect);

            GraphicsPath gp = new GraphicsPath();

            Pen trackerPen = new Pen(Color.Black, 1.0F);
            Brush trackerBrush = new SolidBrush(Color.LightBlue);
            foreach (ITrackRegion trackRegion in trackerRegionList)
            {
                if (trackRegion.TrackPos.PosType != TrackPosType.Inner)
                {
                    trackRegion.AddGraphic(gp);
                }
            }

            Matrix rotationTransform = new Matrix(1, 0, 0, 1, 0, 0);
            rotationTransform.RotateAt(-transformRect.Angle, DrawingHelper.CenterPoint(transformRect));
            gp.Transform(rotationTransform);

            g.FillPath(trackerBrush, gp);
            g.DrawPath(trackerPen, gp);
        }

        public virtual void Draw(Graphics g, CoordTransformer coordTransformer, bool editable)
        {
            if (/*editable == false && */Visible == false)
                return;

            GraphicsPath gp = GetGraphicsPath(coordTransformer);

            if (tempBrush != null)
                g.FillPath(tempBrush, gp);
            else if (figureProperty.Brush != null)
                g.FillPath(figureProperty.Brush, gp);

            g.DrawPath(figureProperty.Pen, gp);

            DrawCaption(g, coordTransformer);
        }

        public virtual void DrawCaption(Graphics g, CoordTransformer coordTransformer = null)
        {
            PointF startPoint = new PointF(GetRectangle().Left, GetRectangle().Top);

            PointF transStartPoint;
            float scaledFontSize = 10;
            if (coordTransformer != null)
            {
                transStartPoint = coordTransformer.Transform(startPoint);
                scaledFontSize = coordTransformer.Transform(new SizeF(10, 0)).Width;
            }
            else
            {
                transStartPoint = startPoint;
            }

            StringFormat stringFormat = new StringFormat();

            Font scaledFont = new Font(FontFamily.GenericSansSerif, scaledFontSize);
            Brush brush = new SolidBrush(figureProperty.Pen.Color);

            g.DrawString(Name, scaledFont, brush, transStartPoint, stringFormat);
        }

        public virtual GraphicsPath GetGraphicsPath(ICoordTransform coordTransformer = null)
        {
            RotatedRect rectangle = GetRectangle();
            RotatedRect transformRect = new RotatedRect(rectangle);
            if (coordTransformer != null)
                transformRect = coordTransformer.Transform(rectangle);

            PointF[] points = transformRect.GetPoints();
            GraphicsPath gp = new GraphicsPath();
            gp.AddPolygon(points);

            return gp;
        }

        public virtual bool PtInOutline(PointF point, ICoordTransform coordTransformer = null)
        {
            return GetGraphicsPath(coordTransformer).IsOutlineVisible(point, new Pen(Color.Black, Configuration.TrackerSize * 2));
        }

        public virtual bool IsCrossed(PointF startPt, PointF endPt)
        {
            return false;
        }

        public virtual bool IsSame(Figure figure)
        {
            return false;
        }

        public virtual bool PtInRegion(PointF point, CoordTransformer coordTransformer = null)
        {
            return GetGraphicsPath(coordTransformer).IsVisible(point);
        }

        public virtual bool IsFilled()
        {
            return (figureProperty.Brush != null);
        }

        public abstract RotatedRect GetRectangle();
        public void Offset(Point pt) { Offset(pt.X, pt.Y); }
        public abstract void Offset(float x, float y);
        public void Scale(float scale) { Scale(scale, scale); }
        public abstract void Scale(float scaleX, float scaleY);

        public abstract void FlipX();
        public abstract void FlipY();

        public virtual void Rotate(float offAngle)
        {

        }

        public virtual void GetTrackPath(List<GraphicsPath> graphicPathList, SizeF offset, TrackPos trackPos)
        {
            RectangleF rectangle = GetRectangle().ToRectangleF();

            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(new RectangleF(new PointF(rectangle.X + offset.Width, rectangle.Y + offset.Height), rectangle.Size));

            graphicPathList.Add(graphicsPath);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", base.ToString(), this.name);
        }

        public int testValue = new Random().Next(0, 1000);
    }

    public class OblongFigure : RectangleFigure
    {
        public OblongFigure()
        {
            type = FigureType.Oblong;
        }

        public OblongFigure(RotatedRect rectangle, Pen pen, Brush brush = null)
            : base(rectangle, pen, brush)
        {
            type = FigureType.Oblong;

        }

        public override object Clone()
        {
            OblongFigure figure = new OblongFigure();
            figure.Copy(this);

            return figure;
        }

        public override GraphicsPath GetGraphicsPath(ICoordTransform coordTransformer = null)
        {
            RotatedRect rectangle = GetRectangle();
            RotatedRect transformRect = new RotatedRect(rectangle);
            if (coordTransformer != null)
                transformRect = coordTransformer.Transform(rectangle);

            // Point 순서: LT, RT, RB, LB
            PointF[] points = transformRect.GetPoints();
            GraphicsPath gp = new GraphicsPath();

            if (transformRect.Width < 1 || transformRect.Height < 1)
            {
                gp.AddRectangle(rectangle.GetBoundRect());
            }
            else
            {
                float arcDim;
                if (transformRect.Width > transformRect.Height)
                {
                    arcDim = transformRect.Height;
                    transformRect.Inflate(-arcDim / 2, 0);
                }
                else
                {
                    arcDim = transformRect.Width;
                    transformRect.Inflate(0, -arcDim / 2);
                }
                PointF[] point4 = transformRect.GetPoints();
                // Point 순서: LT, RT, RB, LB

                PointF center;
                RectangleF rect;

                // 1
                gp.AddLine(point4[0], point4[1]);

                // 2
                center = DrawingHelper.CenterPoint(point4[1], point4[2]);
                rect = DrawingHelper.FromCenterSize(center, new SizeF(arcDim, arcDim));
                gp.AddArc(rect, -rectangle.Angle - 90, 180);

                // 3
                gp.AddLine(point4[2], point4[3]);

                // 4
                center = DrawingHelper.CenterPoint(point4[3], point4[0]);
                rect = DrawingHelper.FromCenterSize(center, new SizeF(arcDim, arcDim));
                gp.AddArc(rect, -rectangle.Angle + 90, 180);
            }

            return gp;
        }
    }

    public class LineFigure : Figure
    {
        PointF startPoint = new Point();
        public PointF StartPoint
        {
            get { return startPoint; }
            set { startPoint = value; }
        }

        PointF endPoint = new Point();
        public PointF EndPoint
        {
            get { return endPoint; }
            set { endPoint = value; }
        }

        public LineFigure()
        {
            type = FigureType.Line;
        }

        public LineFigure(PointF startPoint, PointF endPoint, string figurePropertyName) : base(figurePropertyName)
        {
            type = FigureType.Line;

            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public LineFigure(PointF startPoint, PointF endPoint, Pen pen) : base(pen, null)
        {
            type = FigureType.Line;

            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public override object Clone()
        {
            LineFigure lineFigure = new LineFigure();
            lineFigure.Copy(this);

            return lineFigure;
        }

        public override void Copy(Figure srcFigure)
        {
            base.Copy(srcFigure);

            LineFigure lineFigure = (LineFigure)srcFigure;

            startPoint = lineFigure.startPoint;
            endPoint = lineFigure.endPoint;
        }

        public override bool IsFilled()
        {
            return false;
        }

        public override RotatedRect GetRectangle()
        {
            RotatedRect startRect = new RotatedRect(startPoint.X, startPoint.Y, 0, 0, 0);
            RotatedRect endRect = new RotatedRect(endPoint.X, endPoint.Y, 0, 0, 0);
            RotatedRect rectangle = RotatedRect.Union(startRect, endRect);
            if (rectangle.Width == 0)
                rectangle.Inflate(1, 0);
            else if (rectangle.Height == 0)
                rectangle.Inflate(0, 1);

            return rectangle;
        }

        public override void Inflate(int w, int h)
        {

        }

        public override void Load(XmlElement figureElement)
        {
            if (figureElement == null)
                return;

            base.Load(figureElement);

            XmlHelper.GetValue(figureElement, "StartPoint", ref startPoint);
            XmlHelper.GetValue(figureElement, "EndPoint", ref endPoint);
        }

        public override void Save(XmlElement figureElement)
        {
            base.Save(figureElement);

            XmlHelper.SetValue(figureElement, "StartPoint", startPoint);
            XmlHelper.SetValue(figureElement, "EndPoint", endPoint);
        }

        public override void Offset(float x, float y)
        {
            startPoint.X += x;
            startPoint.Y += y;
            endPoint.X += x;
            endPoint.Y += y;
        }

        public override void Scale(float scaleX, float scaleY)
        {
            startPoint.X = Convert.ToInt32(startPoint.X * scaleX);
            startPoint.Y = Convert.ToInt32(startPoint.Y * scaleY);
            endPoint.X = Convert.ToInt32(endPoint.X * scaleX);
            endPoint.Y = Convert.ToInt32(endPoint.Y * scaleY);
        }

        public override void GetTrackPath(List<GraphicsPath> graphicPathList, SizeF offset, TrackPos trackPos)
        {
            List<PointF> offsetPointList = new List<PointF>();
            if (trackPos.PosType == TrackPosType.Inner)
            {
                offsetPointList.Add(PointF.Add(startPoint, offset));
                offsetPointList.Add(PointF.Add(endPoint, offset));
            }
            else if (trackPos.PosType == TrackPosType.LeftTop)
            {
                offsetPointList.Add(PointF.Add(startPoint, offset));
                offsetPointList.Add(endPoint);
            }
            else if (trackPos.PosType == TrackPosType.RightBottom)
            {
                offsetPointList.Add(startPoint);
                offsetPointList.Add(PointF.Add(endPoint, offset));
            }

            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddLine(offsetPointList[0], offsetPointList[1]);

            graphicPathList.Add(graphicsPath);
        }

        public override List<ITrackRegion> GetTrackerRegionList(ICoordTransform coordTransformer, bool rotationLocked)
        {
            int trackerHalfSize = Configuration.TrackerSize / 2;
            int trackerSize = Configuration.TrackerSize;

            RotatedRect rect = GetRectangle();
            if (coordTransformer != null)
                rect = coordTransformer.Transform(rect);

            List<ITrackRegion> tackRegionList = new List<ITrackRegion>();
            tackRegionList.Add(new TrackRectangle(startPoint.X - trackerHalfSize, startPoint.Y - trackerHalfSize, trackerSize, trackerSize, TrackPosType.LeftTop));
            tackRegionList.Add(new TrackRectangle(endPoint.X - trackerHalfSize, endPoint.Y - trackerHalfSize, trackerSize, trackerSize, TrackPosType.RightBottom));

            return tackRegionList;
        }

        public override RotatedRect GetTrackingRect(TrackPos trackPos, SizeF offset, bool rotationLocked)
        {
            RotatedRect startRect = new RotatedRect(startPoint.X, startPoint.Y, 0, 0, 0);
            RotatedRect endRect = new RotatedRect(endPoint.X, endPoint.Y, 0, 0, 0);

            switch (trackPos.PosType)
            {
                case TrackPosType.LeftTop:
                    startRect.X += offset.Width;
                    startRect.Y += offset.Height;
                    break;
                case TrackPosType.RightBottom:
                    endRect.X += offset.Width;
                    endRect.Y += offset.Height;
                    break;
                case TrackPosType.Inner:
                    startRect.X += offset.Width;
                    startRect.Y += offset.Height;
                    endRect.X += offset.Width;
                    endRect.Y += offset.Height;
                    break;
            }

            return RotatedRect.Union(startRect, endRect);
        }

        public override void TrackMove(TrackPos trackPos, SizeF offset, bool rotationLocked)
        {
            switch (trackPos.PosType)
            {
                case TrackPosType.LeftTop:
                    startPoint.X += offset.Width;
                    startPoint.Y += offset.Height;
                    break;
                case TrackPosType.RightBottom:
                    endPoint.X += offset.Width;
                    endPoint.Y += offset.Height;
                    break;
                case TrackPosType.Inner:
                    startPoint.X += offset.Width;
                    startPoint.Y += offset.Height;
                    endPoint.X += offset.Width;
                    endPoint.Y += offset.Height;
                    break;
            }
        }

        public override GraphicsPath GetGraphicsPath(ICoordTransform coordTransformer)
        {
            PointF transStartPoint = startPoint;
            PointF transEndPoint = endPoint;

            if (coordTransformer != null)
            {
                transStartPoint = coordTransformer.Transform(startPoint);
                transEndPoint = coordTransformer.Transform(endPoint);
            }

            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(transStartPoint, transEndPoint);

            return gp;
        }

        public override void FlipX()
        {
            startPoint = new PointF(startPoint.X, -startPoint.Y);
            endPoint = new PointF(endPoint.X, -endPoint.Y);
        }

        public override void FlipY()
        {
            startPoint = new PointF(-startPoint.X, startPoint.Y);
            endPoint = new PointF(-endPoint.X, endPoint.Y);
        }

        public override void SetRectangle(RotatedRect rectangle)
        {
            throw new NotImplementedException();
        }
    }

    public class RectangleFigure : Figure
    {
        RotatedRect rectangle = new RotatedRect();
        public RotatedRect Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        public RectangleFigure()
        {
            type = FigureType.Rectangle;
        }

        public RectangleFigure(RotatedRect rectangle, string figurePropertyName) : base(figurePropertyName)
        {
            type = FigureType.Rectangle;
            this.rectangle = rectangle;
        }

        public RectangleFigure(RotatedRect rectangle, Pen pen, Brush brush = null) : base(pen, brush)
        {
            type = FigureType.Rectangle;
            this.rectangle = rectangle;
        }

        public RectangleFigure(RectangleF rectangle, string figurePropertyName) : base(figurePropertyName)
        {
            type = FigureType.Rectangle;

            this.rectangle.X = (int)rectangle.X;
            this.rectangle.Y = (int)rectangle.Y;
            this.rectangle.Width = (int)rectangle.Width;
            this.rectangle.Height = (int)rectangle.Height;
        }

        public RectangleFigure(RectangleF rectangle, Pen pen, Brush brush = null) : base(pen, brush)
        {
            type = FigureType.Rectangle;

            this.rectangle.X = (int)rectangle.X;
            this.rectangle.Y = (int)rectangle.Y;
            this.rectangle.Width = (int)rectangle.Width;
            this.rectangle.Height = (int)rectangle.Height;
        }

        public override object Clone()
        {
            RectangleFigure rectangleFigure = new RectangleFigure();
            rectangleFigure.Copy(this);

            return rectangleFigure;
        }

        public override void Copy(Figure srcFigure)
        {
            base.Copy(srcFigure);

            RectangleFigure rectangleFigure = (RectangleFigure)srcFigure;
            rectangle = rectangleFigure.rectangle;
        }

        public override RotatedRect GetRectangle()
        {
            return new RotatedRect(rectangle);
        }

        public override void Inflate(int w, int h)
        {
            this.rectangle.Inflate(w, h);
        }

        public override void Load(XmlElement figureElement)
        {
            if (figureElement == null)
                return;

            base.Load(figureElement);

            XmlHelper.GetValue(figureElement, "Rect", ref rectangle);
        }

        public override void Save(XmlElement figureElement)
        {
            base.Save(figureElement);

            XmlHelper.SetValue(figureElement, "Rect", rectangle);
        }

        public override void Offset(float x, float y)
        {
            rectangle.Offset(x, y);
        }

        public override void Rotate(float offAngle)
        {
            rectangle.Angle = (rectangle.Angle + offAngle) % 360;
        }

        public override void Scale(float scaleX, float scaleY)
        {
            rectangle.X = Convert.ToInt32(rectangle.X * scaleX);
            rectangle.Y = Convert.ToInt32(rectangle.Y * scaleY);
            rectangle.Width = Convert.ToInt32(rectangle.Width * scaleX);
            rectangle.Height = Convert.ToInt32(rectangle.Height * scaleY);
        }

        public override void SetRectangle(RotatedRect rectangle)
        {
            this.rectangle = rectangle;
        }

        public override void FlipX()
        {
            rectangle.FromLTRB(rectangle.Left, -rectangle.Top, rectangle.Right, -rectangle.Bottom);
            rectangle.Angle = 360 - rectangle.Angle;
        }

        public override void FlipY()
        {
            rectangle.FromLTRB(-rectangle.Left, rectangle.Top, -rectangle.Right, rectangle.Bottom);
            rectangle.Angle = (180 - rectangle.Angle);
            if (rectangle.Angle < 0)
                rectangle.Angle = 360 + rectangle.Angle;
        }
    }

    public class EllipseFigure : RectangleFigure
    {
        public EllipseFigure()
        {
            type = FigureType.Ellipse;
        }

        public EllipseFigure(Rectangle rectangle, string figurePropertyName) : base(rectangle, figurePropertyName)
        {
            type = FigureType.Ellipse;
        }

        public EllipseFigure(Rectangle rectangle, Pen pen, Brush brush = null) : base(rectangle, pen, brush)
        {
            type = FigureType.Ellipse;
        }

        public EllipseFigure(RectangleF rectangle, string figurePropertyName) : base(rectangle, figurePropertyName)
        {
            type = FigureType.Ellipse;
        }

        public EllipseFigure(RectangleF rectangle, Pen pen, Brush brush = null)
            : base(rectangle, pen, brush)
        {
            type = FigureType.Ellipse;
        }

        public EllipseFigure(PointF centerPoint, float halfSize, Pen pen)
            : base(new RotatedRect(centerPoint.X - halfSize, centerPoint.Y - halfSize, halfSize * 2, halfSize * 2, 0), pen)
        {
            type = FigureType.Ellipse;
        }

        public override object Clone()
        {
            EllipseFigure ellipseFigure = new EllipseFigure();
            ellipseFigure.Copy(this);

            return ellipseFigure;
        }

        public override GraphicsPath GetGraphicsPath(ICoordTransform coordTransformer)
        {
            RotatedRect transformRect = Rectangle;
            if (coordTransformer != null)
                transformRect = coordTransformer.Transform(Rectangle);

            GraphicsPath gp = new GraphicsPath();
            RectangleF drawingRect = transformRect.ToRectangleF();
            if (drawingRect.Width < 1 || drawingRect.Height < 1)
                gp.AddRectangle(drawingRect);
            else
                gp.AddEllipse(drawingRect);
            //gp.AddEllipse(new RectangleF(363.0587f,98.42592f,0.9f,0.9f));

            //			Matrix rotationTransform = new Matrix(1, 0, 0, 1, 0, 0);
            //			rotationTransform.RotateAt(-transformRect.Angle, DrawingHelper.CenterPoint(transformRect));
            //			gp.Transform(rotationTransform);

            return gp;
        }

        public override void FlipX()
        {
            RotatedRect rectangle = Rectangle;

            rectangle.FromLTRB(rectangle.Left, -rectangle.Top, rectangle.Right, -rectangle.Bottom);

            SetRectangle(rectangle);
        }

        public override void FlipY()
        {
            RotatedRect rectangle = Rectangle;

            rectangle.FromLTRB(-rectangle.Left, rectangle.Top, -rectangle.Right, rectangle.Bottom);

            SetRectangle(rectangle);
        }
    }

    public class CrossFigure : RectangleFigure
    {
        public CrossFigure()
        {
            type = FigureType.Cross;
        }

        public CrossFigure(Rectangle rectangle, string figurePropertyName)
            : base(rectangle, figurePropertyName)
        {
            type = FigureType.Cross;
        }

        public CrossFigure(Rectangle rectangle, Pen pen)
            : base(rectangle, pen, null)
        {
            type = FigureType.Cross;
        }

        public CrossFigure(RectangleF rectangle, string figurePropertyName)
            : base(rectangle, figurePropertyName)
        {
            type = FigureType.Cross;
        }

        public CrossFigure(RectangleF rectangle, Pen pen)
            : base(rectangle, pen, null)
        {
            type = FigureType.Cross;
        }

        public CrossFigure(PointF centerPoint, float halfSize, Pen pen)
            : base(new RotatedRect(centerPoint.X - halfSize, centerPoint.Y - halfSize, halfSize * 2, halfSize * 2, 0), pen)
        {
            type = FigureType.Cross;
        }

        public CrossFigure(Point centerPoint, float halfSize, Pen pen)
            : base(new RotatedRect(centerPoint.X - halfSize, centerPoint.Y - halfSize, halfSize * 2, halfSize * 2, 0), pen)
        {
            type = FigureType.Cross;
        }

        public override object Clone()
        {
            CrossFigure crossFigure = new CrossFigure();
            crossFigure.Copy(this);

            return crossFigure;
        }

        public override GraphicsPath GetGraphicsPath(ICoordTransform coordTransformer)
        {
            RotatedRect transformRect = Rectangle;
            if (coordTransformer != null)
                transformRect = coordTransformer.Transform(Rectangle);

            PointF centerPt = DrawingHelper.CenterPoint(transformRect);

            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(centerPt.X, transformRect.Y, centerPt.X, transformRect.Bottom);
            gp.CloseFigure();
            gp.AddLine(transformRect.X, centerPt.Y, transformRect.Right, centerPt.Y);

            Matrix rotationTransform = new Matrix(1, 0, 0, 1, 0, 0);
            rotationTransform.RotateAt(-transformRect.Angle, DrawingHelper.CenterPoint(transformRect));
            gp.Transform(rotationTransform);

            return gp;
        }

        public override void FlipX()
        {
            RotatedRect rectangle = Rectangle;

            rectangle.FromLTRB(rectangle.Left, -rectangle.Top, rectangle.Right, -rectangle.Bottom);

            SetRectangle(rectangle);
        }

        public override void FlipY()
        {
            RotatedRect rectangle = Rectangle;

            rectangle.FromLTRB(-rectangle.Left, rectangle.Top, -rectangle.Right, rectangle.Bottom);

            SetRectangle(rectangle);
        }
    }

    public class XRectFigure : RectangleFigure
    {
        public XRectFigure()
        {
            type = FigureType.XRect;
        }

        public XRectFigure(Rectangle rectangle, string figurePropertyName)
            : base(rectangle, figurePropertyName)
        {
            type = FigureType.XRect;
        }

        public XRectFigure(Rectangle rectangle, Pen pen)
            : base(rectangle, pen, null)
        {
            type = FigureType.XRect;
        }

        public XRectFigure(RectangleF rectangle, string figurePropertyName)
            : base(rectangle, figurePropertyName)
        {
            type = FigureType.XRect;
        }

        public XRectFigure(RectangleF rectangle, Pen pen)
            : base(rectangle, pen, null)
        {
            type = FigureType.XRect;
        }

        public XRectFigure(RotatedRect rectangle, string figurePropertyName)
            : base(rectangle, figurePropertyName)
        {
            type = FigureType.XRect;
        }

        public XRectFigure(RotatedRect rectangle, Pen pen)
            : base(rectangle, pen, null)
        {
            type = FigureType.XRect;
        }

        public XRectFigure(PointF centerPoint, float halfSize, Pen pen)
            : base(new RotatedRect(centerPoint.X - halfSize, centerPoint.Y - halfSize, halfSize * 2, halfSize * 2, 0), pen)
        {
            type = FigureType.XRect;
        }

        public override object Clone()
        {
            XRectFigure xRectFigure = new XRectFigure();
            xRectFigure.Copy(this);

            return xRectFigure;
        }

        public override GraphicsPath GetGraphicsPath(ICoordTransform coordTransformer)
        {
            RotatedRect rectangle = GetRectangle();
            RotatedRect transformRect = new RotatedRect(rectangle);
            if (coordTransformer != null)
                transformRect = coordTransformer.Transform(rectangle);

            GraphicsPath gp = new GraphicsPath();
            gp.AddRectangle(transformRect.ToRectangleF());
            gp.CloseFigure();

            PointF[] points = DrawingHelper.GetPoints(transformRect.ToRectangleF(), 0);

            gp.AddLine(points[0].X, points[0].Y, points[2].X, points[2].Y);
            gp.CloseFigure();
            gp.AddLine(points[1].X, points[1].Y, points[3].X, points[3].Y);

            Matrix rotationTransform = new Matrix(1, 0, 0, 1, 0, 0);
            rotationTransform.RotateAt(-rectangle.Angle, DrawingHelper.CenterPoint(transformRect));
            gp.Transform(rotationTransform);

            return gp;
        }

        public override void FlipX()
        {
            RotatedRect rectangle = Rectangle;

            rectangle.FromLTRB(rectangle.Left, -rectangle.Top, rectangle.Right, -rectangle.Bottom);

            SetRectangle(rectangle);
        }

        public override void FlipY()
        {
            RotatedRect rectangle = Rectangle;

            rectangle.FromLTRB(-rectangle.Left, rectangle.Top, -rectangle.Right, rectangle.Bottom);

            SetRectangle(rectangle);
        }
    }

    public class PolygonFigure : Figure
    {
        float angle;
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        List<PointF> pointList = new List<PointF>();
        public List<PointF> PointList
        {
            get { return pointList; }
            set { pointList = value; }
        }

        bool isClose = true;
        public bool IsClose
        {
            get { return isClose; }
            set { isClose = value; }
        }

        bool isEditable = true;
        public bool IsEditable
        {
            get { return isEditable; }
            set { isEditable = value; }
        }

        public PolygonFigure() : base()
        {
            type = FigureType.Polygon;
        }

        public PolygonFigure(List<PointF> pointList, bool isClose, string figurePropertyName) : base(figurePropertyName)
        {
            type = FigureType.Polygon;

            this.pointList.AddRange(pointList);
            this.isClose = isClose;
        }

        public PolygonFigure(List<PointF> pointList, bool isClose, Pen pen, Brush brush = null) : base(pen, brush)
        {
            type = FigureType.Polygon;

            this.pointList.AddRange(pointList);
            this.isClose = isClose;
        }

        public PolygonFigure(bool isClose, string figurePropertyName) : base(figurePropertyName)
        {
            this.isClose = isClose;
        }

        public PolygonFigure(bool isClose, Pen pen, Brush brush = null) : base(pen, brush)
        {
            type = FigureType.Polygon;
        }

        public override object Clone()
        {
            PolygonFigure polygonFigure = new PolygonFigure();
            polygonFigure.Copy(this);

            return polygonFigure;
        }

        public void AddPoint(PointF point)
        {
            pointList.Add(point);
        }

        public void AddPoints(PointF[] points)
        {
            pointList.AddRange(points);
        }

        public void AddPoints(Point[] points)
        {
            pointList.AddRange(Array.ConvertAll(points, f => new PointF(f.X, f.Y)));
        }

        public override void Copy(Figure srcFigure)
        {
            base.Copy(srcFigure);

            PolygonFigure polygonFigure = (PolygonFigure)srcFigure;

            pointList.AddRange(polygonFigure.PointList);

            angle = polygonFigure.angle;
        }

        public override void Draw(Graphics g, CoordTransformer coordTransformer, bool editable)
        {
            if (/*editable == false && */Visible == false)
                return;

            GraphicsPath gp = GetGraphicsPath(coordTransformer);

            if (this.isClose)
            {
                if (tempBrush != null)
                    g.FillPath(tempBrush, gp);
                else if (figureProperty.Brush != null)
                    g.FillPath(figureProperty.Brush, gp);
            }
            g.DrawPath(figureProperty.Pen, gp);
        }

        public override RotatedRect GetRectangle()
        {
            return new RotatedRect(DrawingHelper.GetBoundRect(pointList.ToArray()), angle);
        }

        public override void Inflate(int w, int h)
        {

        }

        public override void Load(XmlElement figureElement)
        {
            if (figureElement == null)
                return;

            base.Load(figureElement);

            foreach (XmlElement pointListElement in figureElement)
            {
                if (pointListElement.Name == "PointList")
                {
                    foreach (XmlElement pointElement in pointListElement)
                    {
                        if (pointElement.Name == "Point")
                        {
                            PointF point = new PointF();
                            float x = Convert.ToSingle(XmlHelper.GetValue(pointElement, "X", "0"));
                            float y = Convert.ToSingle(XmlHelper.GetValue(pointElement, "Y", "0"));
                            point.X = x;
                            point.Y = y;

                            pointList.Add(point);
                        }
                    }

                }
            }
        }

        public override void Save(XmlElement figureElement)
        {
            base.Save(figureElement);

            XmlElement pointListElement = figureElement.OwnerDocument.CreateElement("", "PointList", "");
            figureElement.AppendChild(pointListElement);

            foreach (PointF point in pointList)
            {
                XmlHelper.SetValue(pointListElement, "Point", point);
            }
        }

        public override void Offset(float x, float y)
        {
            for (int i = 0; i < pointList.Count; i++)
            {
                pointList[i] = new PointF(pointList[i].X + x, pointList[i].Y + y);
            }
        }

        public override void Scale(float scaleX, float scaleY)
        {
            for (int i = 0; i < pointList.Count; i++)
            {
                pointList[i] = new PointF(pointList[i].X * scaleX, pointList[i].Y * scaleY);
            }
        }

        public override List<ITrackRegion> GetTrackerRegionList(ICoordTransform coordTransformer, bool rotationLocked)
        {
            int trackerHalfSize = Configuration.TrackerSize / 2;
            int trackerSize = Configuration.TrackerSize;

            RotatedRect rect = GetRectangle();
            List<PointF> pointList = new List<PointF>();
            pointList.AddRange(this.pointList);
            if (coordTransformer != null)
            {
                pointList.Clear();
                rect = coordTransformer.Transform(rect);
                this.pointList.ForEach(f => pointList.Add(coordTransformer.Transform(f)));
            }

            List<ITrackRegion> regionList = new List<ITrackRegion>();
            if (isEditable)
            {
                for (int index = 0; index < pointList.Count; index++)
                    regionList.Add(new TrackRectangle(pointList[index].X - trackerHalfSize, pointList[index].Y - trackerHalfSize, trackerSize, trackerSize, TrackPosType.Polygon, index));
                regionList.Add(new TrackPolygon(pointList, TrackPosType.Inner, 0));
            }
            else
            {
                regionList.Add(new TrackRectangle(rect.Left - trackerHalfSize, rect.Top - trackerHalfSize, trackerSize, trackerSize, TrackPosType.LeftTop));
                regionList.Add(new TrackRectangle(rect.Right - trackerHalfSize, rect.Top - trackerHalfSize, trackerSize, trackerSize, TrackPosType.RightTop));
                regionList.Add(new TrackRectangle(rect.Left - trackerHalfSize, rect.Bottom - trackerHalfSize, trackerSize, trackerSize, TrackPosType.LeftBottom));
                regionList.Add(new TrackRectangle(rect.Right - trackerHalfSize, rect.Bottom - trackerHalfSize, trackerSize, trackerSize, TrackPosType.RightBottom));
                regionList.Add(new TrackRectangle(rect.Left, rect.Top, rect.Width, rect.Height, TrackPosType.Inner));
            }

            return regionList;
        }

        public override RotatedRect GetTrackingRect(TrackPos trackPos, SizeF offset, bool rotationLocked)
        {
            List<PointF> offsetPointList = new List<PointF>();

            if (trackPos.PosType == TrackPosType.Inner)
            {
                pointList.ForEach(x => { offsetPointList.Add(new PointF(x.X + offset.Width, x.Y + offset.Height)); });
            }
            else
            {
                offsetPointList.AddRange(pointList);

                PointF point = offsetPointList[trackPos.PolygonIndex];
                point.X += offset.Width;
                point.Y += offset.Height;

                offsetPointList[trackPos.PolygonIndex] = point;
            }

            return new RotatedRect(DrawingHelper.GetBoundRect(offsetPointList.ToArray()), 0);
        }

        public override void TrackMove(TrackPos trackPos, SizeF offset, bool rotationLocked)
        {
            if (trackPos.PosType == TrackPosType.Inner)
            {
                List<PointF> pointList = new List<PointF>();
                this.pointList.ForEach(x => pointList.Add(new PointF(x.X += offset.Width, x.Y += offset.Height)));
                this.pointList = pointList;

            }
            else
            {
                if (isEditable)
                {
                    PointF point = pointList[trackPos.PolygonIndex];
                    point.X += offset.Width;
                    point.Y += offset.Height;

                    pointList[trackPos.PolygonIndex] = point;
                }
                else
                {

                    //RotatedRect RotatedRect = this.GetRectangle();
                    //float scaleX = (float)(RotatedRect.Width + offset.Width) / (float)RotatedRect.Width;
                    //float scaleY = (float)(RotatedRect.Height+offset.Height) / (float)RotatedRect.Height;

                    //)
                    //PointF scaleRef = DrawingHelper.CenterPoint(this.GetTrackerRectangleList().Find(f => f.TrackPos.PosType == TrackPosType.LeftBottom).Rectangle);
                }
            }
        }

        public override GraphicsPath GetGraphicsPath(ICoordTransform coordTransformer)
        {
            List<PointF> transPointList = new List<PointF>();

            RotatedRect rotatedRect = this.GetRectangle();
            PointF rotateCenter = new PointF((rotatedRect.Left + rotatedRect.Right) / 2.0f, (rotatedRect.Top + rotatedRect.Bottom) / 2.0f);

            GraphicsPath gp = new GraphicsPath();

            pointList.ForEach(x =>
            {
                PointF transPoint = x;
                if (coordTransformer != null)
                {
                    transPoint = MathHelper.Rotate(x, rotateCenter, coordTransformer.InvertY ? -angle : angle);
                    transPoint = coordTransformer.Transform(transPoint);
                }
                else
                {
                    transPoint = MathHelper.Rotate(x, rotateCenter, angle);
                }

                transPointList.Add(transPoint);
            });

            try
            {
                if (transPointList.Count != 0)
                {
                    if (transPointList.Count > 2 && isClose)
                    {
                        gp.AddPolygon(transPointList.ToArray());
                    }
                    else
                    {
                        gp.AddLines(transPointList.ToArray());
                    }
                }
            }
            catch { }
            return gp;
        }

        public override void FlipX()
        {
            for (int i = 0; i < pointList.Count; i++)
            {
                pointList[i] = new PointF(pointList[i].X, -pointList[i].Y);
            }
        }

        public override void FlipY()
        {
            for (int i = 0; i < pointList.Count; i++)
            {
                pointList[i] = new PointF(-pointList[i].X, pointList[i].Y);
            }
        }

        public override void Rotate(float offAngle)
        {
            angle = (angle + offAngle) % 360;
        }

        public override bool IsCrossed(PointF startPt, PointF endPt)
        {
            for (int i = 0; i < pointList.Count; i++)
            {
                if (i == (pointList.Count - 1))
                {
                    if (DrawingHelper.IsCross(pointList[i], pointList[0], startPt, endPt) == true)
                        return true;
                }
                else
                {
                    if (DrawingHelper.IsCross(pointList[i], pointList[i + 1], startPt, endPt) == true)
                        return true;
                }
            }

            return false;
        }

        public override void GetTrackPath(List<GraphicsPath> graphicPathList, SizeF offset, TrackPos trackPos)
        {
            List<PointF> offsetPointList = new List<PointF>();
            if (trackPos.PosType == TrackPosType.Inner)
            {
                foreach (PointF pt in pointList)
                    offsetPointList.Add(PointF.Add(pt, offset));
            }
            else
            {
                for (int i = 0; i < pointList.Count; i++)
                {
                    if (trackPos.PolygonIndex == i)
                        offsetPointList.Add(PointF.Add(pointList[i], offset));
                    else
                        offsetPointList.Add(pointList[i]);
                }
            }

            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddPolygon(offsetPointList.ToArray());

            graphicPathList.Add(graphicsPath);
        }

        public override bool IsSame(Figure figure)
        {
            if (!(figure is PolygonFigure))
                return false;

            PointF[] ptArray1 = pointList.ToArray();
            PointF centerPt1 = DrawingHelper.CenterPoint(ptArray1);
            ptArray1 = DrawingHelper.Offset(ptArray1, new SizeF(-centerPt1.X, -centerPt1.Y));

            PolygonFigure otherFigure = (PolygonFigure)figure;
            PointF[] ptArray2 = otherFigure.PointList.ToArray();
            PointF centerPt2 = DrawingHelper.CenterPoint(ptArray2);
            ptArray2 = DrawingHelper.Offset(ptArray2, new SizeF(-centerPt2.X, -centerPt2.Y));

            foreach (PointF pt1 in ptArray1)
            {
                bool found = false;

                foreach (PointF pt2 in ptArray2)
                {
                    PointF offset = PointF.Subtract(pt1, new SizeF(pt2));
                    if (Math.Abs(offset.X) < 1 && Math.Abs(offset.Y) < 1)
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                    return false;
            }

            return true;
        }

        public override void SetRectangle(RotatedRect rectangle)
        {
            throw new NotImplementedException();
        }
    }

    public class TextFigure : Figure
    {
        PointF position;
        public PointF Position
        {
            get { return position; }
            set { position = value; }
        }

        float angle;
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        StringAlignment lineAlignment;

        public TextFigure() : base()
        {
            type = FigureType.Text;
        }

        public TextFigure(string text, PointF position, Font font, Color textColor, StringAlignment alignment = StringAlignment.Center,
                                    StringAlignment lineAlignment = StringAlignment.Center) : base(font, textColor, alignment)
        {
            type = FigureType.Text;

            this.text = text;
            this.position = position;
            this.lineAlignment = lineAlignment;
        }

        public override object Clone()
        {
            TextFigure textFigure = new TextFigure();
            textFigure.Copy(this);

            return textFigure;
        }

        public override void Copy(Figure srcFigure)
        {
            base.Copy(srcFigure);

            TextFigure textFigure = (TextFigure)srcFigure;

            this.text = textFigure.text;
            this.position = textFigure.position;
        }

        public override bool IsFilled()
        {
            return true;
        }

        public override void Inflate(int w, int h)
        {
            throw new NotImplementedException();
        }

        public override RotatedRect GetRectangle()
        {
            PointF newPosition = position;
            Size textSize = TextRenderer.MeasureText(text, figureProperty.Font);
            if (figureProperty.Alignment == StringAlignment.Far)
            {
                newPosition.X -= textSize.Width;
            }
            else if (figureProperty.Alignment == StringAlignment.Center)
            {
                newPosition.X -= textSize.Width / 2;
            }

            return new RotatedRect(newPosition, textSize, angle);
        }

        public override void Load(XmlElement figureElement)
        {
            if (figureElement == null)
                return;

            base.Load(figureElement);

            text = XmlHelper.GetValue(figureElement, "Text", "");
            XmlHelper.GetValue(figureElement, "Position", ref position);
        }

        public override void Save(XmlElement figureElement)
        {
            base.Save(figureElement);

            XmlHelper.SetValue(figureElement, "Text", text);
            XmlHelper.SetValue(figureElement, "Position", position);
        }

        public override void Offset(float x, float y)
        {
            position.X += x;
            position.Y += y;
        }

        public override void Scale(float scaleX, float scaleY)
        {
            position.X *= scaleX;
            position.Y *= scaleY;

            Font font = figureProperty.Font;
            figureProperty.Font = new Font(font.FontFamily, (float)(font.Size * scaleX), font.Style);
        }

        public override void SetRectangle(RotatedRect rectangle)
        {
            if (figureProperty.Alignment == StringAlignment.Center)
            {
                position.X = rectangle.X + rectangle.Width / 2;
            }
            else if (figureProperty.Alignment == StringAlignment.Far)
            {
                position.X = rectangle.X + rectangle.Width;
            }
            else
            {
                position.X = rectangle.X;
            }

            position.Y = rectangle.Y;
        }

        public override void Draw(Graphics g, CoordTransformer coordTransformer, bool editable)
        {
            if (/*editable == false &&*/ Visible == false)
                return;

            RotatedRect transformRect = GetRectangle();
            PointF transformPt = position;
            float scaledFontSize = figureProperty.Font.Size;
            if (coordTransformer != null)
            {
                transformRect = coordTransformer.Transform(transformRect);
                transformPt = coordTransformer.Transform(position);
                scaledFontSize = coordTransformer.Transform(new SizeF(figureProperty.Font.Size, 0)).Width;
            }

            if (scaledFontSize < 1)
                scaledFontSize = 1;

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            Matrix m = g.Transform.Clone();
            //			m.RotateAt((float)-transformRect.Angle, new PointF(transformRect.X + transformRect.Width / 2.0f, transformRect.Y + transformRect.Height / 2.0f));
            m.RotateAt((float)-transformRect.Angle, transformPt);

            Matrix preTransform = g.Transform;
            g.Transform = m;

            StringFormat stringFormat = new StringFormat()
            {
                Alignment = figureProperty.Alignment,
                LineAlignment = lineAlignment
            };

            Font scaledFont = new Font(figureProperty.Font.FontFamily, scaledFontSize);
            Brush brush = new SolidBrush(figureProperty.TextColor);

            g.DrawString(text, scaledFont, brush, transformPt, stringFormat);

            g.Transform = preTransform;
        }

        public override void FlipX()
        {
            position = new PointF(position.X, -position.Y);
        }

        public override void FlipY()
        {
            position = new PointF(-position.X, position.Y);
        }
    }

    public class ImageFigure : Figure
    {
        Image image;
        public Image Image
        {
            get { return image; }
            set { image = value; }
        }

        string imageFormatName = "bmp";
        public string ImageFormatName
        {
            get { return imageFormatName; }
            set { imageFormatName = value; }
        }

        RotatedRect rectangle = new RotatedRect();

        public ImageFigure()
        {
            type = FigureType.Image;
        }

        public ImageFigure(Image image, string imageFormatName, RotatedRect rectangle) // png 파일 불러오기
        {
            type = FigureType.Image;

            this.image = image;
            this.imageFormatName = imageFormatName.ToLower();
            this.rectangle = rectangle;
        }

        public override object Clone()
        {
            ImageFigure imageFigure = new ImageFigure();
            imageFigure.Copy(this);

            return imageFigure;
        }

        public override void Copy(Figure srcFigure)
        {
            base.Copy(srcFigure);

            ImageFigure imageFigure = (ImageFigure)srcFigure;

            this.rectangle = imageFigure.rectangle;
            this.imageFormatName = imageFigure.imageFormatName;
            this.image = ImageHelper.CloneImage(imageFigure.image);
        }

        public override bool IsFilled()
        {
            return true;
        }

        public override RotatedRect GetRectangle()
        {
            return new RotatedRect(rectangle);
        }

        public override void Inflate(int w, int h)
        {

        }

        public override void Load(XmlElement figureElement)
        {
            if (figureElement == null)
                return;

            base.Load(figureElement);

            XmlHelper.GetValue(figureElement, "Rect", ref rectangle);
            imageFormatName = XmlHelper.GetValue(figureElement, "ImageFormat", "Bmp");

            string imageStr = XmlHelper.GetValue(figureElement, "Image", "");
            if (imageStr != "")
                image = ImageHelper.Base64StringToImage(imageStr, GetImageFormat(imageFormatName));
        }

        private ImageFormat GetImageFormat(string imageFormatStr)
        {
            switch (imageFormatStr)
            {
                default:
                case "bmp": return ImageFormat.Bmp;
                case "png": return ImageFormat.Png;
            }
        }

        public override void Save(XmlElement figureElement)
        {
            base.Save(figureElement);

            XmlHelper.SetValue(figureElement, "Rect", rectangle);
            XmlHelper.SetValue(figureElement, "ImageFormat", imageFormatName.ToString());
            XmlHelper.SetValue(figureElement, "Image", ImageHelper.ImageToBase64String(image, GetImageFormat(imageFormatName)));
        }

        public override void Offset(float x, float y)
        {
            rectangle.Offset(x, y);
        }

        public override void Scale(float scaleX, float scaleY)
        {
            rectangle.X = Convert.ToInt32(rectangle.X * scaleX);
            rectangle.Y = Convert.ToInt32(rectangle.Y * scaleY);
            rectangle.Width = Convert.ToInt32(rectangle.Width * scaleX);
            rectangle.Height = Convert.ToInt32(rectangle.Height * scaleY);
        }

        public override void Draw(Graphics g, CoordTransformer coordTransformer, bool editable)
        {
            if (/*editable == false &&*/ Visible == false)
                return;

            RotatedRect transformRect = rectangle;
            if (coordTransformer != null)
                transformRect = coordTransformer.Transform(rectangle);

            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //Transformation matrix
            Matrix m = new Matrix();
            m.RotateAt((float)-transformRect.Angle, new PointF(transformRect.Width / 2.0f, transformRect.Height / 2.0f));

            Matrix preTransform = g.Transform;
            g.Transform = m;
            g.DrawImage(image, transformRect.ToRectangleF());
            g.Transform = preTransform;
        }

        public override void SetRectangle(RotatedRect rectangle)
        {
            this.rectangle = rectangle;
        }

        public override void FlipX()
        {
            rectangle.FromLTRB(rectangle.Left, -rectangle.Top, rectangle.Right, -rectangle.Bottom);
        }

        public override void FlipY()
        {
            rectangle.FromLTRB(-rectangle.Left, rectangle.Top, -rectangle.Right, rectangle.Bottom);
        }
    }

    public class FigureGroup : Figure
    {
        public override Brush TempBrush
        {
            set
            {
                foreach (Figure figure in figureList)
                    figure.TempBrush = value;
            }
        }

        List<Figure> figureList = new List<Figure>();
        public List<Figure> FigureList
        {
            get { return figureList; }
        }

        public Figure this[string name]
        {
            get { return figureList.Find(f => f.Name == name); }
            set { figureList.Add(value); }
        }

        public Figure this[int key]
        {
            get { return figureList[key]; }
        }

        public int NumFigure
        {
            get { return figureList.Count; }
        }

        public FigureGroup()
        {
            type = FigureType.Group;
        }

        public FigureGroup(string name)
        {
            type = FigureType.Group;
            this.Name = name;
        }

        public bool FigureExist
        {
            get { return figureList.Count > 0; }
        }

        public override object Clone()
        {
            FigureGroup figureGroup = new FigureGroup();
            figureGroup.Copy(this);

            return figureGroup;
        }

        public override void Copy(Figure srcFigure)
        {
            base.Copy(srcFigure);

            FigureGroup figureGroup = (FigureGroup)srcFigure;

            foreach (Figure figure in figureGroup.figureList)
                AddFigure((Figure)figure.Clone());
        }

        public void SetTempBrush(Brush brush)
        {
            foreach (Figure figure in figureList)
            {
                figure.TempBrush = brush;
            }
        }

        public override void ResetTempProperty()
        {
            foreach (Figure figure in figureList)
            {
                figure.ResetTempProperty();
            }
        }

        public IEnumerator<Figure> GetEnumerator()
        {
            return figureList.GetEnumerator();
        }

        public Figure GetFigure(string id)
        {
            foreach (Figure figure in figureList)
            {
                if (figure.Id == id)
                    return figure;
            }

            return null;
        }

        public void AddFigure(int index, Figure figure)
        {
            lock (figureList)
                figureList.Insert(index, figure);
            //return figure;
        }

        public void AddFigure(Figure figure)
        {
            lock (figureList)
                figureList.Add(figure);
            //return figure;
        }

        public void AddFigure(IEnumerable<Figure> figures)
        {
            lock (figureList)
                figureList.AddRange(figures);
        }

        public void RemoveFigure(Figure figure)
        {
            lock (figureList)
                figureList.Remove(figure);
        }

        public void RemoveFigure(IEnumerable<Figure> figures)
        {
            lock (figureList)
            {
                foreach (Figure figure in figures)
                    figureList.Remove(figure);
            }
        }

        public void RemoveOutOf(Rectangle displayImageRect)
        {
            this.figureList.RemoveAll(f =>
            {
                RectangleF intersectRect = RectangleF.Intersect(f.GetRectangle().ToRectangleF(), displayImageRect);
                return (intersectRect.Width < 5 || intersectRect.Height < 5);
            });
        }

        public void Clear()
        {
            lock (figureList)
                figureList.Clear();
        }

        public override bool IsFilled()
        {
            return false;
        }

        public bool IsExsit(Figure figure)
        {
            return figureList.IndexOf(figure) >= 0;
        }

        public override RotatedRect GetRectangle()
        {
            RotatedRect rectangle = new RotatedRect();
            lock (figureList)
            {
                foreach (Figure figure in figureList)
                {
                    if (rectangle.IsEmpty)
                        rectangle = figure.GetRectangle();
                    else
                        rectangle = RotatedRect.Union(rectangle, figure.GetRectangle());
                }
            }
            return rectangle;
        }

        public override void Inflate(int w, int h)
        {
            this.figureList.ForEach(f => f.Inflate(w, h));
        }

        public override void Load(XmlElement figureGroupElement)
        {
            if (figureGroupElement == null)
                return;

            base.Load(figureGroupElement);

            lock (figureList)
            {
                foreach (XmlElement figureElement in figureGroupElement)
                {
                    if (figureElement.Name == "Figure")
                    {
                        string typeStr = XmlHelper.GetValue(figureElement, "Type", "");

                        Figure figure = FigureFactory.Create(typeStr);
                        figureList.Add(figure);

                        figure.Load(figureElement);
                    }
                }
            }
        }

        public override void Save(XmlElement figureGroupElement)
        {
            base.Save(figureGroupElement);

            lock (figureList)
            {
                foreach (Figure figure in figureList)
                {
                    XmlElement figureElement = figureGroupElement.OwnerDocument.CreateElement("", "Figure", "");
                    figureGroupElement.AppendChild(figureElement);

                    figure.Save(figureElement);
                }
            }
        }

        public Figure GetFigureByName(string name)
        {
            Predicate<Figure> func = new Predicate<Figure>(f => f.Name == name);
            return FindFigure(func);

            //foreach (Figure figure in figureList.Reverse<Figure>())
            //{
            //    if (figure.Name == name)
            //        return figure;
            //}

            //return null;

        }
        public Figure GetFigureByTag(object tagObj)
        {
            Predicate<Figure> func = new Predicate<Figure>(f => f.Tag == tagObj);
            return FindFigure(func);

            //foreach (Figure figure in figureList.Reverse<Figure>())
            //{
            //    if (figure.Tag == tagObj)
            //        return figure;
            //}

            //return null;
        }

        public Figure GetFigureByTagStr(object tagObj)
        {
            Predicate<Figure> func = new Predicate<Figure>(f => f.Tag.ToString() == tagObj.ToString());
            return FindFigure(func);

            //foreach (Figure figure in figureList.Reverse<Figure>())
            //{
            //    if (figure.Tag.ToString() == tagObj.ToString())
            //        return figure;
            //}

            //return null;
        }

        public Figure GetFigure(Point point)
        {
            Predicate<Figure> func = new Predicate<Figure>(f => f.PtInRegion(point));
            return FindFigure(func);

            //foreach (Figure figure in figureList.Reverse<Figure>())
            //{
            //    if (figure.PtInRegion(point))
            //        return figure;
            //}

            //return null;
        }

        private Figure FindFigure(Predicate<Figure> func)
        {
            return figureList.FindLast(f => func(f));
        }

        public void SetSelectable(bool selectable)
        {
            lock (figureList)
                figureList.ForEach(f => f.Selectable = selectable);

            //foreach (Figure figure in figureList.Reverse<Figure>())
            //{
            //    figure.Selectable = selectable;
            //}
        }

        public Figure Select(PointF point, bool selectableOnly = true)
        {
            lock (figureList)
            {
                foreach (Figure figure in figureList.Reverse<Figure>())
                {
                    if (figure.PtInOutline(point) == true)
                    {
                        if ((!selectableOnly || figure.Selectable) && figure.Visible)
                            return figure;
                    }
                }

                foreach (Figure figure in figureList.Reverse<Figure>())
                {
                    if (figure.PtInRegion(point) == true)
                    {
                        if ((!selectableOnly || figure.Selectable) && figure.Visible)
                            return figure;
                    }
                }
            }

            return null;
        }

        public List<Figure> Select(Rectangle rectangle)
        {
            List<Figure> selectedFigureList = new List<Figure>();
            // Outline 객체 선택
            lock (figureList)
            {
                foreach (Figure figure in figureList.Reverse<Figure>())
                {
                    Rectangle boundRect = Rectangle.Round(figure.GetRectangle().GetBoundRect());
                    if (rectangle.Contains(boundRect) == true)
                    {
                        if (figure.Selectable == true && figure.Visible)
                            selectedFigureList.Add(figure);
                    }
                }
            }
            return selectedFigureList;
        }

        public Figure GetTaggedFigure(Point point)
        {
            lock (figureList)
                foreach (Figure figure in figureList.Reverse<Figure>())
                {
                    if (figure.PtInRegion(point) && figure.Tag != null)
                        return figure;
                }

            return null;
        }

        static Predicate<Figure> ByRef(Figure figureRef)
        {
            return delegate (Figure figure)
            {
                return figure == figureRef;
            };
        }

        public void MoveUp(Figure figure)
        {
            int index = figureList.FindIndex(ByRef(figure));
            if (index > -1 && (index + 1) < figureList.Count)
            {
                figureList.Remove(figure);
                figureList.Insert(index + 1, figure);
            }
        }

        public void MoveTop(Figure figure)
        {
            figureList.Remove(figure);
            figureList.Add(figure);
        }

        public void MoveDown(Figure figure)
        {
            int index = figureList.FindIndex(ByRef(figure));
            if ((index > 0) && (index < figureList.Count))
            {
                figureList.Remove(figure);
                figureList.Insert(index - 1, figure);
            }
        }

        public void MoveBottom(Figure figure)
        {
            lock (figureList)
            {
                figureList.Remove(figure);
                figureList.Insert(0, figure);
            }
        }

        public void Offset(SizeF size) => Offset(size.Width, size.Height);

        public override void Offset(float x, float y)
        {
            lock (figureList)
                foreach (Figure figure in figureList)
                {
                    figure.Offset(x, y);
                }
        }

        public override void Scale(float scaleX, float scaleY)
        {
            lock (figureList)
                figureList.ForEach(f => f.Scale(scaleX, scaleY));
            //foreach (Figure figure in figureList)
            //{
            //    figure.Scale(scaleX, scaleY);
            //}
        }

        public override void Draw(Graphics g, CoordTransformer coordTransformer, bool editable)
        {
            if (/*editable == false &&*/ Visible == false)
                return;

            lock (figureList)
                foreach (Figure figure in figureList)
                {
                    figure.Draw(g, coordTransformer, editable);
                }
        }

        public override void SetRectangle(RotatedRect newRect)
        {
            RotatedRect preRect = GetRectangle();
            PointF centerPt = DrawingHelper.CenterPoint(preRect);
            Offset(-centerPt.X, -centerPt.Y);

            Scale((float)(newRect.Width) / preRect.Width, (float)(newRect.Height) / preRect.Height);

            PointF newCenterPt = DrawingHelper.CenterPoint(newRect);
            Offset(newCenterPt.X, newCenterPt.Y);
        }

        public override bool PtInRegion(PointF point, CoordTransformer coordTransformer = null)
        {
            lock (figureList)
                foreach (Figure figure in figureList.Reverse<Figure>())
                {
                    if (figure.PtInRegion(point, coordTransformer) == true)
                        return true;
                }

            return false;
        }

        public override bool PtInOutline(PointF point, ICoordTransform coordTransformer = null)
        {
            lock (figureList)
                foreach (Figure figure in figureList.Reverse<Figure>())
                {
                    if (figure.PtInOutline(point, coordTransformer) == true)
                        return true;
                }

            return false;
        }

        public override void FlipX()
        {
            RotatedRect rotatedRect = GetRectangle();
            PointF centerPt = DrawingHelper.CenterPoint(rotatedRect);

            lock (figureList)
                foreach (Figure figure in figureList.Reverse<Figure>())
                {
                    figure.Offset(0, -centerPt.Y);
                    figure.FlipX();
                    figure.Offset(0, centerPt.Y);
                }
        }

        public override void FlipY()
        {
            RotatedRect rotatedRect = GetRectangle();
            PointF centerPt = DrawingHelper.CenterPoint(rotatedRect);

            lock (figureList)
                foreach (Figure figure in figureList.Reverse<Figure>())
                {
                    figure.Offset(-centerPt.X, 0);
                    figure.FlipY();
                    figure.Offset(centerPt.Y, 0);
                }
        }

        public override void Rotate(float offAngle)
        {
            lock (figureList)
                this.figureList.ForEach(f => f.Rotate(offAngle));
        }


        public List<Figure> GetFigureByName(string name, bool recursive, bool wildSearch)
        {
            List<Figure> figureList = new List<Figure>();
            lock (figureList)
            {
                if (recursive)
                {
                    List<Figure> figureGroupList = this.figureList.FindAll(f => f is FigureGroup);
                    foreach (FigureGroup figureGroup in figureGroupList)
                        figureList.AddRange(figureGroup.GetFigureByName(name, true, wildSearch));
                }

                figureList = this.figureList.FindAll(f =>
                {
                    if (wildSearch)
                        return f.Name.Contains(name);
                    else
                        return f.Name == name;
                });
            }
            return figureList;
        }

        //public List<Figure> GetFigureByName(string name, bool recursive, bool wildSearch)
        //{
        //    List<Figure> figureList = new List<Figure>();

        //    foreach (Figure figure in this.figureList)
        //    {
        //        if (figure is FigureGroup && recursive)
        //        {
        //            figureList.AddRange(((FigureGroup)figure).GetFigureByName(name, recursive, wildSearch));
        //        }

        //        string figureString = figure.Name;

        //        if (wildSearch == true)
        //        {
        //            if (figureString.Contains(name))
        //            {
        //                figureList.Add(figure);
        //            }
        //        }
        //        else
        //        {
        //            if (figureString == name)
        //            {
        //                figureList.Add(figure);
        //            }
        //        }
        //    }

        //    return figureList;
        //}

        public List<Figure> GetFigureByTag(string tag, bool wildSearch)
        {
            List<Figure> figureList = new List<Figure>();

            lock (figureList)
            {
                foreach (Figure figure in this.figureList)
                {
                    if (figure is FigureGroup)
                    {
                        figureList.AddRange(((FigureGroup)figure).GetFigureByTag(tag, wildSearch));
                    }

                    string figureString = figure.Tag as string;

                    if (wildSearch == true)
                    {
                        if (figureString.Contains(tag))
                        {
                            figureList.Add(figure);
                        }
                    }
                    else
                    {
                        if (figureString == tag)
                        {
                            figureList.Add(figure);
                        }
                    }
                }
            }
            return figureList;
        }

        public void Delete(List<Figure> figures)
        {
            lock (figureList)
            {
                foreach (Figure figure in figures)
                {
                    if (figureList.Remove(figure))
                    {

                    }
                }
            }
        }

    }
}
