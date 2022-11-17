using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

using DynMvp.Base;

namespace DynMvp.UI
{
    public enum TrackPosType
    {
        None, LeftTop , Top, RightTop, Right, RightBottom, Bottom, LeftBottom, Left, Rotate, Move, Inner, Polygon, Link
    }

    public interface ITrackTarget
    {
        bool IsSizable();
        bool IsRotatable();
        bool IsContainer();
    }

    public struct TrackPos
    {
        TrackPosType posType;
        public TrackPosType PosType
        {
            get { return posType; }
            set { posType = value; }
        }

        private int polygonIndex;
        public int PolygonIndex
        {
            get { return polygonIndex; }
            set { polygonIndex = value; }
        }

        public TrackPos(TrackPosType posType, int polygonIndex)
        {
            this.posType = posType;
            this.polygonIndex = polygonIndex;
        }
    }
    
    public delegate void TrackerMovedDelegate();
    public delegate void SelectionPointCapturedDelegate(Point point);
    public delegate void SelectionRectCapturedDelegate(Rectangle rectangle, Point startPos, Point endPos);
    public delegate void AddFigureCapturedDelegate(List<PointF> pointList);
    public delegate void PositionShiftedDelegate(SizeF offset);
    
    public class Tracker
    {
        private Control ownerControl;
        public TrackerMovedDelegate TrackerMoved;
        public SelectionPointCapturedDelegate SelectionPointCaptured;
        public SelectionRectCapturedDelegate SelectionRectCaptured;
        public AddFigureCapturedDelegate AddFigureCaptured;
        public PositionShiftedDelegate PositionShifted;

        private bool enable;
        public bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }

        private bool measureMode;
        public bool MeasureMode
        {
            get { return measureMode; }
            set { measureMode = value; }
        }

        private bool rotationLocked = false;
        public bool RotationLocked
        {
            get { return rotationLocked; }
            set { rotationLocked = value; }
        }

        private bool moveLocked = false;
        public bool MoveLocked
        {
            get { return moveLocked; }
            set { moveLocked = value; }
        }

        private bool onSelectRange = false;
        private bool onMoveFigure = false;

        private Point startTrackPos;
        public Point StartTrackPos
        {
            get { return startTrackPos; }
        }

        private Point endTrackPos;
        public Point EndTrackPos
        {
            get { return endTrackPos; }
        }

        private List<PointF> trackPointList = new List<PointF>();
        public List<PointF> TrackPointList
        {
            get { return trackPointList; }
        }

        private List<Figure> figureList = new List<Figure>();

        private TrackPos trackPos;
        internal TrackPos TrackPos
        {
            get { return trackPos; }
            set { trackPos = value; }
        }

        CoordTransformer coordTransformer = null;
        public CoordTransformer CoordTransformer
        {
            get { return coordTransformer; }
            set { coordTransformer = value; }
        }

        private bool addFigureMode;
        public bool AddFigureMode
        {
            get { return addFigureMode; }
            set 
            {
                if (addFigureMode != value)
                {
                    LogHelper.Debug(LoggerType.Operation, "Tracker.AddFigureMode.");

                    addFigureMode = value;
                    figureList.Clear();
                    ownerControl.Invalidate();
                }
            }
        }

        private bool shiftPositionMode;
        public bool ShiftPositionMode
        {
            get { return shiftPositionMode; }
            set { shiftPositionMode = value; }
        }

        private FigureType shape;
        public FigureType Shape
        {
            get { return shape; }
            set { shape = value; }
        }

        private int numGridRow = 0;
        public int NumGridRow
        {
            get { return numGridRow; }
            set { numGridRow = value; }
        }

        private int numGridColumn = 0;
        public int NumGridColumn
        {
            get { return numGridColumn; }
            set { numGridColumn = value; }
        }

        public Tracker(Control control)
        {
            ownerControl = control;
        }

        public IEnumerator<Figure> GetEnumerator()
        {
            return figureList.GetEnumerator();
        }

        public void ClearFigure()
        {
            LogHelper.Debug(LoggerType.Operation, "Tracker.ClearFigure.");
            
            figureList.Clear();
            trackPos.PosType = TrackPosType.None;
        }

        public void AddFigure(Figure figure)
        {
            if (figure != null)
            {
                bool add = false;
                LogHelper.Debug(LoggerType.Operation, "Tracker.AddFigure.");
                if (figureList.Count > 0)
                {
                    if (figureList[0].Tag == null || figure.Tag == null)
                    {
                        add = true;
                    }
                    else if (figureList[0].Tag.GetType().Name == figure.Tag.GetType().Name)
                    {
                        add = true;
                    }
                }
                else
                {
                    add = true;
                }

                if (add)
                {
                    Figure fig = figureList.Find(f => f.Tag.Equals(figure.Tag));
                    figureList.Add(figure);
                }
            }
        }

        public void AddFigure(List<Figure> figureList)
        {
            if (figureList != null && figureList.Count > 0)
            {
                LogHelper.Debug(LoggerType.Operation, "Tracker.AddFigure.List");
                foreach (Figure figure in figureList)
                    AddFigure(figure);
//                this.figureList.AddRange(figureList);
            }
        }

        public Figure GetFirstFigure()
        {
            if (figureList.Count > 0)
                return figureList[0];

            return null;
        }

        public bool IsSelected(Figure figure)
        {
            int found = figureList.IndexOf(figure);
            return found >=0;
        }

        public void MouseDown(Point startTrackPos)
        {
            onSelectRange = false;
            onMoveFigure = false;

            if (enable)
            {
                LogHelper.Debug(LoggerType.Operation, "Tracker.MouseDown");

                if (shiftPositionMode)
                {

                }
                else if (addFigureMode == true)
                {
                    onSelectRange = true;
                }
                else
                {
                    trackPos = GetTrackPos(startTrackPos);
                    if (trackPos.PosType == TrackPosType.None /*|| trackPos.PosType == TrackPosType.Inner*/)
                        onSelectRange = true;
                    else 
                        onMoveFigure = true;

                    Debug.WriteLine("Track Pos : " + trackPos.ToString());
                }
                trackPointList.Clear();
                this.startTrackPos = startTrackPos;
                this.endTrackPos = startTrackPos;
                trackPointList.Add(startTrackPos);

                ownerControl.Invalidate();
            }
        }

        public List<Figure> GetFigureList()
        {
            List<Figure> copyFigureList = new List<Figure>();
            copyFigureList.AddRange(figureList);

            return copyFigureList;
        }

        private TrackPos GetTrackPos(Point point)
        {
            TrackPos trackPos = new TrackPos(TrackPosType.None, 0);

            int polygonIndex = 0;

            if (figureList.Count() == 1)
            {
                trackPos = figureList[0].GetTrackPos(point, coordTransformer, rotationLocked, ref polygonIndex);
            }
            else
            {
                foreach (Figure selectedFigure in figureList)
                {
                    trackPos = selectedFigure.GetTrackPos(point, coordTransformer, rotationLocked, ref polygonIndex);
                    if (trackPos.PosType != TrackPosType.None)
                    {
                        trackPos.PosType = TrackPosType.Inner;
                        break;
                    }
                }
            }

            return trackPos;
        }

        public bool IsSingleSelected()
        {
            return figureList.Count() == 1;
        }

        public bool IsMultiSelected()
        {
            return figureList.Count() > 1;
        }

        private bool GetSelectRange(ref RotatedRect selectRange)
        {
            RotatedRect rect = new RotatedRect();
            rect.FromLTRB(Math.Min(startTrackPos.X, endTrackPos.X), Math.Min(startTrackPos.Y, endTrackPos.Y),
                            Math.Max(startTrackPos.X, endTrackPos.X), Math.Max(startTrackPos.Y, endTrackPos.Y));

            if (coordTransformer != null)
                selectRange = coordTransformer.InverseTransform(rect);
            else
                selectRange = rect;

            return (!(rect.Width==0 && rect.Height==0));
        }

        public void Offset(float offsetX, float offsetY)
        {
            SizeF offset = new SizeF(offsetX, offsetY);
            SizeF newOffset = offset;
            if (coordTransformer != null)
                newOffset = coordTransformer.InverseTransform(offset);

            foreach (Figure selectedFigure in figureList)
            {
                selectedFigure.Offset(newOffset.Width, newOffset.Height);
            }

            if (TrackerMoved != null)
                TrackerMoved();
        }

        public void Move(Size offset)
        {
            Debug.WriteLine("Tracker.KeyUp.onMoveFigure");

            //onMoveFigure = true;
            //onSelectRange = true;
            trackPos.PosType = TrackPosType.Inner;
            if (figureList.Count() > 0 && moveLocked == false)
            {
                lock (figureList)
                {
                    Size newOffset = offset;
                    if (coordTransformer != null)
                        newOffset = coordTransformer.InverseTransform(offset);

                    foreach (Figure selectedFigure in figureList)
                    {
                        selectedFigure.TrackMove(trackPos, newOffset, rotationLocked);
                    }

                    if (TrackerMoved != null)
                        TrackerMoved();
                }
            }
            else
            {
                Debug.WriteLine("Tracker.KeyUp.onMoveFigure - There is no selected figure.");
            }

            ownerControl.Invalidate();
            trackPos.PosType = TrackPosType.None;
            //onMoveFigure = false;
            //onSelectRange = false;
        }

        public void MouseUp(Point endTrackPos)
        {
            if (enable == false)
                return;

            if (shiftPositionMode)
            {
                Size offset = new Size(startTrackPos.X - endTrackPos.X, startTrackPos.Y - endTrackPos.Y);

                if (PositionShifted != null)
                    PositionShifted(offset);
            }
            else if (addFigureMode)
            {
                Debug.WriteLine("Tracker.MouseUp.addFigureMode");

                RotatedRect selectRange = new RotatedRect();

                if (GetSelectRange(ref selectRange) == true)
                {
                    if (AddFigureCaptured!= null)
                    {
                        List<PointF> pointList = new List<PointF>();

                        foreach (PointF point in this.trackPointList)
                        {
                            if (coordTransformer != null)
                            {
                                pointList.Add(coordTransformer.InverseTransform(point));
                            }
                            else
                            {
                                pointList.Add(point);
                            }
                        }
                        AddFigureCaptured(pointList);
                    }
                }
            }
            else if (onSelectRange)
            {
                Debug.WriteLine("Tracker.MouseUp.onSelectRange");

                RotatedRect selectRange = new RotatedRect();
                if (GetSelectRange(ref selectRange) == true)
                {
                    if (SelectionRectCaptured != null)
                    {
                        Point startPosTransformed = startTrackPos;
                        Point endPosTransformed = endTrackPos;
                        if (coordTransformer != null)
                        {
                            startPosTransformed = coordTransformer.InverseTransform(startTrackPos);
                            endPosTransformed = coordTransformer.InverseTransform(endTrackPos);
                        }

                        SelectionRectCaptured(DrawingHelper.ToRect(selectRange.ToRectangleF()), startPosTransformed, endPosTransformed);
                    }
                }
                else
                {
                    if (SelectionPointCaptured != null)
                        SelectionPointCaptured(new Point((int)selectRange.Left, (int)selectRange.Top));
                }
            }
            else if (onMoveFigure)
            {
                Debug.WriteLine("Tracker.MouseUp.onMoveFigure");

                if (figureList.Count() > 0 && moveLocked == false)
                {
                    lock (figureList)
                    {
                        Size offset = new Size(endTrackPos.X - startTrackPos.X, endTrackPos.Y - startTrackPos.Y);
                        Size newOffset = offset;
                        if (coordTransformer != null)
                            newOffset = coordTransformer.InverseTransform(offset);

                        foreach (Figure selectedFigure in figureList)
                        {
                            selectedFigure.TrackMove(trackPos, newOffset, rotationLocked);
                        }

                        if (TrackerMoved != null)
                        {
                            TrackerMoved();
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("Tracker.MouseUp.onMoveFigure - There is no selected figure.");
                }
            }

            onSelectRange = false;
            onMoveFigure = false;

            ownerControl.Invalidate();
        }

        public void RemoveFigure(Figure figure)
        {
            if (figure != null)
            {
                LogHelper.Debug(LoggerType.Operation, "Tracker.RemoveFigure.");
                figureList.Remove(figure);
            }
        }

        public void MouseMove(Point point)
        {
            if (enable == false)
                return;

            Debug.WriteLine("Tracker.MouseMove");

            endTrackPos = point;
            trackPointList.Add(point);

            Debug.WriteLine(endTrackPos);

            ownerControl.Invalidate();
        }

        public void Draw(Graphics g, CoordTransformer coordTransformer)
        {
            if (shiftPositionMode || measureMode)
            {
                using (Pen p = new Pen(Color.Red, 1.0F))
                {
                    p.DashStyle = DashStyle.Dot;

                    g.DrawLine(p, startTrackPos, endTrackPos);
                }
            }
            else
            {
                using (Pen p = new Pen(Color.Blue, 2.0F))
                {
                    if (figureList.Count > 0)
                    {
                        foreach (Figure selectedFigure in figureList)
                        {
                            selectedFigure.DrawSelection(g, coordTransformer, rotationLocked);
                        }
                    }

                    if (onSelectRange || onMoveFigure)
                    {
                        Debug.WriteLine("Draw Track Line");

                        List<GraphicsPath> trackPathList = GetTrackPath();
                        foreach (GraphicsPath graphicsPath in trackPathList)
                        {
                            g.DrawPath(p, graphicsPath);
                        }
                    }
                }
            }
        }

        private List<GraphicsPath> GetTrackPath()
        {
            RotatedRect trackRect;
            List<GraphicsPath> trackPathList = new List<GraphicsPath>();

            if (figureList.Count > 0)
            {
                foreach (Figure selectedFigure in figureList)
                {
                    GraphicsPath graphicsPath = new GraphicsPath();

                    Point transStartTrackPos = startTrackPos;
                    Point transEndTrackPos = endTrackPos;

//                    Debug.WriteLine("Figure Offset : " + offset.ToString());

                    if (coordTransformer != null)
                    {
                        transStartTrackPos = coordTransformer.InverseTransform(startTrackPos);
                        transEndTrackPos = coordTransformer.InverseTransform(endTrackPos);
                    }

                    Size offset = new Size(transEndTrackPos.X - transStartTrackPos.X, transEndTrackPos.Y - transStartTrackPos.Y);

                    Debug.WriteLine("Figure Offset : " + offset.ToString());

                    trackRect = selectedFigure.GetTrackingRect(trackPos, offset, rotationLocked);
                    if (coordTransformer != null)
                        trackRect = coordTransformer.Transform(trackRect);

                    graphicsPath.AddRectangle(trackRect.ToRectangleF());

                    float angle = trackRect.Angle;

                    Matrix rotationTransform = new Matrix(1, 0, 0, 1, 0, 0);
                    rotationTransform.RotateAt(-angle, DrawingHelper.CenterPoint(trackRect));
                    graphicsPath.Transform(rotationTransform);

                    trackPathList.Add(graphicsPath);
                }
            }
            else
            {
                trackRect = new RotatedRect();
                trackRect.FromLTRB(Math.Min(startTrackPos.X, endTrackPos.X), Math.Min(startTrackPos.Y, endTrackPos.Y),
                                Math.Max(startTrackPos.X, endTrackPos.X), Math.Max(startTrackPos.Y, endTrackPos.Y));

                if (shape == FigureType.Grid)
                {
                    DrawGrid(trackPathList, trackRect);
                }
                else
                {
                    GraphicsPath graphicsPath = new GraphicsPath();

                    switch (shape)
                    {
                        default:
                        case FigureType.Rectangle:
                            graphicsPath.AddRectangle(trackRect.ToRectangleF());
                            break;
                        case FigureType.Ellipse:
                            graphicsPath.AddEllipse(trackRect.ToRectangleF());
                            break;
                        case FigureType.Line:
                            graphicsPath.AddLine(startTrackPos, endTrackPos);
                            break;
                        case FigureType.Grid:
                            break;
                        case FigureType.Polygon:
                            graphicsPath.AddLines(this.trackPointList.ToArray());
                            break;
                    }

                    trackPathList.Add(graphicsPath);
                }
            }

            return trackPathList;
        }

        void DrawGrid(List<GraphicsPath> trackPathList, RotatedRect trackRect)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(trackRect.ToRectangleF());
            trackPathList.Add(graphicsPath);

            if (numGridColumn > 1 && numGridRow > 1)
            {
                float cellWidth = trackRect.Width / numGridColumn;
                float cellHeight = trackRect.Height / numGridRow;

                for (int x = 1; x < numGridColumn; x++)
                {
                    graphicsPath = new GraphicsPath();
                    graphicsPath.AddLine(new PointF(trackRect.X + x * cellWidth, trackRect.Top), new PointF(trackRect.X + x * cellWidth, trackRect.Bottom));
                    trackPathList.Add(graphicsPath);
                }

                for (int y = 1; y < numGridRow; y++)
                {
                    graphicsPath = new GraphicsPath();
                    graphicsPath.AddLine(new PointF(trackRect.Left, trackRect.Y + y * cellHeight), new PointF(trackRect.Right, trackRect.Y + y * cellHeight));
                    trackPathList.Add(graphicsPath);
                }
            }
        }
    }
}
