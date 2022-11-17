using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DynMvp.Base;
using DynMvp.UI;
using System.Diagnostics;
using DynMvp.Devices.MotionController;

namespace DynMvp.Devices.UI
{
    public delegate void FovChangedDelegate(int fovNo, PointF position);
    public delegate void FovMovedDelegate(List<Figure> figureList);

    public partial class FovNavigator : UserControl
    {
        public FovChangedDelegate FovChanged;
        public FovMovedDelegate FovMoved;

        AxisHandler robotStage;
        public AxisHandler RobotStage
        {
            set
            {
                robotStage = value;
                if (robotStage != null)
                    workingRect = robotStage.GetWorkingRange();
            }
        }

        private FigureGroup fovList = new FigureGroup();
        public FigureGroup FovList
        {
            get { return fovList; }
            set { fovList = value; }
        }

        private RectangleF workingRect;

        private SizeF fovSize;
        public SizeF FovSize
        {
            get { return fovSize; }
            set { fovSize = value; }
        }

        private PointF currentPosition;
        public PointF CurrentPosition
        {
            get { return currentPosition; }
            set { currentPosition = value; }
        }

        private bool invertY;
        public bool InvertY
        {
            get { return invertY; }
            set { invertY = value; }
        }

        private bool appendMode;
        private Tracker tracker;

        private bool enable = false;
        public bool Enable
        {
            set
            {
                enable = value;
                tracker.Enable = value;
            }
        }

        private float viewScale = 1;
        public float ViewScale
        {
            get { return viewScale; }
            set { viewScale = value; }
        }

        private int selectedFovNo = -1;
        public int SelectedFovNo
        {
            get { return selectedFovNo; }
            set { selectedFovNo = value; }
        }

        public FovNavigator()
        {
            InitializeComponent();
            tracker = new Tracker(this);
            tracker.Enable = true;
            tracker.RotationLocked = true;
            tracker.TrackerMoved = new TrackerMovedDelegate(Tracker_FigureMoved);
            tracker.SelectionPointCaptured = new SelectionPointCapturedDelegate(Tracker_SelectionPointCaptured);
            tracker.SelectionRectCaptured = new SelectionRectCapturedDelegate(Tracker_SelectionRectCaptured);
        }

        public void ClearFovList()
        {
            fovList.Clear();
            tracker.ClearFigure();
        }

        public Figure AddFovFigure(PointF robotPos)
        {
            RectangleF rect = new RectangleF(robotPos.X - fovSize.Width / 2, robotPos.Y - fovSize.Height / 2, fovSize.Width, fovSize.Height);
            RectangleFigure rectangleFigure = new RectangleFigure(rect, new Pen(Color.Red));

            fovList.AddFigure(rectangleFigure);

            return rectangleFigure;
        }

        public void ResetSelection()
        {
            Debug.WriteLine("RobotNavigator.ResetSelection");

            tracker.ClearFigure();
        }

        public void SelectFigure(Figure figure)
        {
            Debug.WriteLine("RobotNavigator.SelectFigure");
            tracker.AddFigure(figure);
        }

        public void SelectFigureByTag(Object tag)
        {
            Debug.WriteLine("RobotNavigator.SelectFigureByTag");
            tracker.AddFigure(fovList.GetFigureByTag(tag));
        }

        public void SelectFov(int fovNo)
        {
            Figure fovFigure = fovList.GetFigureByTagStr(fovNo);
            if (fovFigure != null)
            {
                tracker.ClearFigure();
                tracker.AddFigure(fovFigure);


                selectedFovNo = (int)fovFigure.Tag;
                currentPosition = DrawingHelper.CenterPoint(fovFigure.GetRectangle());

                if (FovChanged != null)
                    FovChanged(selectedFovNo, currentPosition);

                Invalidate();
            }
        }

        public void SelectFigure(List<Figure> figureList)
        {
            tracker.AddFigure(figureList);
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (enable == false)
                return;

            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            appendMode = (Control.ModifierKeys == Keys.Control);

            Debug.WriteLine("RobotNavigator.OnMouseDown");

            tracker.CoordTransformer = GetCoordTransformer();

            tracker.MouseDown(e.Location);

            base.OnMouseDown(e);
        }

        void Tracker_SelectionPointCaptured(Point point)
        {
            Debug.WriteLine("RobotNavigator.Tracker_SelectionPointCaptured");

            if (workingRect.Contains(point) == false)
                return;

            if (appendMode == false)
                ResetSelection();

            point.X = (int)MathHelper.Bound(point.X, workingRect.X, workingRect.X + workingRect.Width);
            point.Y = (int)MathHelper.Bound(point.Y, workingRect.Y, workingRect.Y + workingRect.Height);

            Figure figure = fovList.Select(point);
            if (figure != null)
            {
                tracker.AddFigure(figure);

                if (tracker.IsSingleSelected())
                {
                    selectedFovNo = (int)figure.Tag;
                    currentPosition = DrawingHelper.CenterPoint(figure.GetRectangle());

                    if (FovChanged != null)
                        FovChanged(selectedFovNo, currentPosition);
                }
            }
            else
            {
                selectedFovNo = -1;
                currentPosition = new PointF(point.X, point.Y);
            }

            if (FovChanged != null)
                FovChanged(selectedFovNo, currentPosition);

            Invalidate();
        }

        void Tracker_SelectionRectCaptured(Rectangle rectangle, Point startPos, Point endPos)
        {
            Debug.WriteLine("RobotNavigator.Tracker_SelectionRectCaptured");

                if (appendMode == false)
                    ResetSelection();

                List<Figure> figureList = fovList.Select(rectangle);
                if (figureList.Count() > 0)
                {
                    tracker.AddFigure(figureList);

                    if (tracker.IsSingleSelected())
                    {
                        selectedFovNo = (int)figureList[0].Tag;

                        PointF centerPoint = DrawingHelper.CenterPoint(figureList[0].GetRectangle());
                        if (FovChanged != null)
                            FovChanged(selectedFovNo, centerPoint);
                    }
                }

            Invalidate();
        }

        void Tracker_FigureMoved()
        {
            Debug.WriteLine("SchemaViewer.Tracker_FigureMoved");

            if (FovMoved != null)
                FovMoved(tracker.GetFigureList());

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            Debug.WriteLine("SchemaViewer.OnMouseUp");

            tracker.MouseUp(e.Location);

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            Debug.WriteLine("SchemaViewer.OnMouseMove");

            tracker.MouseMove(e.Location);

            base.OnMouseMove(e);
        }

        private CoordTransformer GetCoordTransformer()
        {
            CoordTransformer coordTransformer = new CoordTransformer();
            coordTransformer.SetSrcRect(workingRect);
            coordTransformer.SetDisplayRect(new RectangleF(0, 0, Width, Height));
            coordTransformer.InvertY = true;
                        
            return coordTransformer;
        }

        protected override void  OnPaint(PaintEventArgs e)
        {
 	        base.OnPaint(e);

            CoordTransformer coordTransformer = GetCoordTransformer();

            RectangleFigure workingRectFigure = new RectangleFigure(workingRect, new Pen(Color.Black));
            workingRectFigure.Draw(e.Graphics, coordTransformer, enable);

            RectangleF fovRect = new RectangleF(currentPosition.X - fovSize.Width / 2, currentPosition.Y - fovSize.Height / 2, fovSize.Width, fovSize.Height);
            RectangleFigure fovRectFigure = new RectangleFigure(fovRect, new Pen(Color.Blue, 1));
            fovRectFigure.Draw(e.Graphics, coordTransformer, enable);

            fovList.Draw(e.Graphics, coordTransformer, enable);
            tracker.Draw(e.Graphics, coordTransformer);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
        }

        public void ResetTargetResult()
        {
            fovList.ResetTempProperty();
            Invalidate();
        }

        public void UpdateTargetResult(int fovNo, bool result)
        {
            Figure fovFigure = fovList.GetFigureByTag(fovNo);

            if (result == true)
            {
                fovFigure.TempBrush = new SolidBrush(Color.LimeGreen);
            }
            else
            {
                fovFigure.TempBrush = new SolidBrush(Color.Red);
            }

            Invalidate();
        }

        private void SchemaViewer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ResetTargetResult();
        }
    }
}
