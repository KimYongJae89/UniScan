using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DynMvp.UI;
using System.Drawing.Drawing2D;
using DynMvp.Base;
using System.Drawing.Imaging;
using DynMvp.Properties;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics;

namespace DynMvp.UI
{
    public enum DragMode { None = -1, Add, Pan, Select, Modify, Cross, Zoom, Measure };
    public delegate void UpdateImageDelegate(Bitmap bitmap, RectangleF viewPort);
    public delegate void ZoomRangeDelegate(RectangleF zoomRange);
    public delegate void UpdateZoomDelegate(float zoomOffset, PointF zoomCenter);

    public delegate Figure CreateCustomFigureDelegate(PointF pt1, PointF pt2);

    // WorkingFigure 대상 이벤트
    public delegate void FigureCreatedDelegate(Figure figure, CoordMapper coordMapper);
    public delegate void FigureSelectedDelegate(List<Figure> figureList);
    public delegate void FigureRemovedDelegate(List<Figure> figureList);
    public delegate void FigureCopiedDelegate(List<Figure> figureList);
    public delegate void FigurePastedDelegate(List<Figure> figureList, FigureGroup workingFigures, FigureGroup backgroundFigures, SizeF pasteOffset);
    public delegate void FigureModifiedDelegate(List<Figure> figureList);

    // 모든 Figure 대상 이벤트
    public delegate void FigureMouseOverDelegate(Figure figure);
    public delegate void FigureClickedDelegate(Figure figure, MouseEventArgs e);

    // Panel 대상 이벤트
    public delegate void MouseClickedDelegate(CanvasPanel canvasPanel, PointF point, ref bool processingCancelled);
    public delegate void MouseDblClickedDelegate(CanvasPanel canvasPanel);
    public delegate void MouseLeavedDelegate(CanvasPanel canvasPanel);
    public delegate void OnViewPortChangedDelegate(CanvasPanel canvasPanel);

    public partial class CanvasPanel : UserControl
    {
        const long maxSizeByte = 2147483648;    // 2GB
        public class Option
        {
            private Pen pen = null;
            public Pen Pen
            {
                get { return pen; }
                set { pen = value; }
            }

            private bool showNumber;
            public bool ShowProbeNumber
            {
                get { return showNumber; }
                set { showNumber = value; }
            }

            private int probeNumberSize = 20;
            public int ProbeNumberSize
            {
                get { return probeNumberSize; }
                set { probeNumberSize = value; }
            }

            private bool includeProbe;
            public bool IncludeProbe
            {
                get { return includeProbe; }
                set { includeProbe = value; }
            }
        }

        public CreateCustomFigureDelegate CreateCustomFigure;

        // 모든 Figure 이벤트는 Working Figure에서만 작동함.
        public FigureCreatedDelegate FigureCreated;
        public FigureSelectedDelegate FigureSelected;
        public FigureRemovedDelegate FigureRemoved;
        public FigureCopiedDelegate FigureCopied;
        public FigurePastedDelegate FigurePasted;
        public FigureModifiedDelegate FigureModified;

        public FigureMouseOverDelegate FigureMouseEnter;
        public FigureMouseOverDelegate FigureMouseLeave;
        public FigureClickedDelegate FigureClicked;

        // Panel 대상 이벤트
        public MouseClickedDelegate MouseClicked;
        public MouseDblClickedDelegate MouseDblClicked;
        public MouseLeavedDelegate MouseLeaved;

        public event OnViewPortChangedDelegate OnViewPortChanged;

        // DragMode 직접 수정 금지 (SetPanMode(), SetSelectMode() 등 사용)
        public DragMode DragMode => dragMode;
        DragMode dragMode = DragMode.Select;
        DragMode tempDragMode = DragMode.Select;

        public Bitmap Image => this.image;
        Bitmap image;

        public Size ImageSize { get; private set; }

        public RectangleF ViewPort
        {
            get => this.viewPort;
            set
            {
                this.viewPort = value;
                this.ZoomScale = new SizeF(
                    this.Width / this.viewPort.Width,
                    this.ClientHeight / this.viewPort.Height);
                this.Invalidate(false);
                OnViewPortChanged?.Invoke(this);
            }
        }
        RectangleF viewPort;

        FigureGroup workingFigures = new FigureGroup();
        public FigureGroup WorkingFigures
        {
            get { return workingFigures; }
            //set { workingFigures = value; }
        }

        FigureGroup backgroundFigures = new FigureGroup();
        public FigureGroup BackgroundFigures
        {
            get { return backgroundFigures; }
        }

        FigureGroup tempFigures = new FigureGroup();
        public FigureGroup TempFigures
        {
            get { return tempFigures; }
        }

        Figure enteredFigure = null;
        Figure lastEnterdFigure = null;

        bool showRuler;
        public bool ShowRuler
        {
            get { return showRuler; }
            set { showRuler = value; }
        }

        bool showCenterGuide = false;
        public bool ShowCenterGuide
        {
            get { return showCenterGuide; }
            set { showCenterGuide = value; }
        }

        Point centerGuidePos;
        public Point CenterGuidePos
        {
            get { return centerGuidePos; }
            set { centerGuidePos = value; }
        }

        int centerGuideThickness;
        public int CenterGuideThickness
        {
            get { return centerGuideThickness; }
            set { centerGuideThickness = value; }
        }

        bool hideFigure = false;
        public bool HideFigure
        {
            get { return hideFigure; }
            set { hideFigure = value; }
        }

        private bool useZoom = true;
        public bool UseZoom
        {
            get { return useZoom; }
            set { useZoom = value; }
        }

        public HorizontalAlignment HorizontalAlignment { get => this.horizontalAlignment; set => this.horizontalAlignment = value; }
        HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center;

        public VerticalAlignment VerticalAlignment { get => this.verticalAlignment; set => this.verticalAlignment = value; }
        VerticalAlignment verticalAlignment = VerticalAlignment.Center;

        public InterpolationMode BaseInterpolationMode { get => this.baseInterpolationMode; set => this.baseInterpolationMode = value; }
        InterpolationMode baseInterpolationMode = InterpolationMode.NearestNeighbor;

        public bool ShowToolbar
        {
            get { return statusBar.Visible; }
            set { statusBar.Visible = value; }
        }

        public bool ShowModeToolBar
        {
            set
            {
                statusBar.Panels["Mode"].Visible = value;
                statusBar.Panels["Pan"].Visible = value;
                statusBar.Panels["Select"].Visible = value;
                statusBar.Panels["Cross"].Visible = value;
            }
        }

        public bool ShowShapeToolBar
        {
            set
            {
                statusBar.Panels["Shape"].Visible = value;
                statusBar.Panels["Rectangle"].Visible = value;
                statusBar.Panels["Circle"].Visible = value;
            }
        }

        public bool ShowZoomToolBar
        {
            set
            {
                statusBar.Panels["Zoom"].Visible = value;
                statusBar.Panels["ZoomFit"].Visible = value;
                statusBar.Panels["ZoomIn"].Visible = value;
                statusBar.Panels["ZoomOut"].Visible = value;
                statusBar.Panels["ZoomRange"].Visible = value;
            }
        }

        public bool InvertY { get => this.invertY; set => this.invertY = value; }
        bool invertY = false;

        /// <summary>
        /// 선택된 Figure의 Tracker 표시 여부
        /// </summary>
        public bool ReadOnly { get => this.readOnly; set => this.readOnly = value; }
        private bool readOnly = false;

        public bool RotationLocked { get => this.rotationLocked; set => this.rotationLocked = value; }
        private bool rotationLocked = false;

        public bool SingleAxisTracking { get => this.singleAxisTracking; set => this.singleAxisTracking = value; }
        private bool singleAxisTracking = false;

        public List<PointF> TrackPointList => trackPointList;
        private List<PointF> trackPointList = new List<PointF>();

        public FigureType TrackerShape => trackerShape;
        private FigureType trackerShape = FigureType.Rectangle;

        public int ClientHeight
        {
            get { return Height - (statusBar.Visible ? statusBar.Height : 0); }
        }

        //public float DrawScale { get => this.drawScale; set => this.drawScale = value; }
        //float drawScale = 1;

        public SizeF ZoomScale { get; private set; } = new Size(1, 1);

        public bool FastMode { get => this.fastMode; set => this.fastMode = value; }
        bool fastMode = false;

        bool onMouseDown = false;
        bool onkeyDown = false;
        PointF dragStart;
        PointF dragEnd;
        SizeF dragOffset;

        SelectionContainer selectionContainer = new SelectionContainer();
        private bool onUpdateStateButton;
        private TrackPos curTrackPos;
        List<Figure> copyBuffer = new List<Figure>();
        private int copyCount;

        public CanvasPanel()
        {
            Initialize();
        }

        public CanvasPanel(bool readOnly)
        {
            this.readOnly = readOnly;
            Initialize();
        }

        public CanvasPanel(bool readOnly, InterpolationMode baseInterpolationMode)
        {
            this.readOnly = readOnly;
            this.baseInterpolationMode = baseInterpolationMode;
            Initialize();
        }

        private void Initialize()
        {
            InitializeComponent();

            this.LostFocus += new EventHandler((s, e) => ((CanvasPanel)s).onkeyDown = false);
            this.Dock = DockStyle.Fill;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            SetSelectMode();
        }

        public RectangleF GetBoundRect()
        {
            RectangleF boundRect = workingFigures.GetRectangle().ToRectangleF();
            if (backgroundFigures != null)
                boundRect = DrawingHelper.GetUnionRect(boundRect, backgroundFigures.GetRectangle().ToRectangleF());
            if (tempFigures != null)
                boundRect = DrawingHelper.GetUnionRect(boundRect, tempFigures.GetRectangle().ToRectangleF());

            if (this.image == null)
                return boundRect;
            else
                return DrawingHelper.GetUnionRect(boundRect, new Rectangle(Point.Empty, ImageSize));
        }

        public void Clear()
        {
            workingFigures.Clear();
            backgroundFigures.Clear();
            tempFigures.Clear();
            selectionContainer.ClearSelection();

            FigureSelected?.Invoke(new List<Figure>());
            Invalidate();
        }

        #region Mode
        public void SetPanMode()
        {
            this.tempDragMode = DragMode.None;
            this.dragMode = DragMode.Pan;
            this.trackerShape = FigureType.Line;
            UpdateCursor();
        }

        public void SetSelectMode()
        {
            this.tempDragMode = DragMode.None;
            this.dragMode = DragMode.Select;
            UpdateCursor();
        }

        public void SetPointMode()
        {
            this.tempDragMode = DragMode.None;
            this.dragMode = DragMode.Cross;
            UpdateCursor();
        }

        public void SetAddMode(FigureType figureType)
        {
            this.tempDragMode = this.dragMode;
            this.dragMode = DragMode.Add;
            this.trackerShape = figureType;
            UpdateCursor();
        }

        public void SetMeasureMode(FigureType figureType)
        {
            this.tempDragMode = DragMode.None;
            this.dragMode = DragMode.Measure;
            this.trackerShape = figureType;
            UpdateCursor();
        }

        public void SetZoomMode()
        {
            this.tempDragMode = DragMode.None;
            this.dragMode = DragMode.Zoom;
            UpdateCursor();
        }
        #endregion

        #region Add/Remove

        #endregion

        #region Selection
        public void SelectFigure(Figure figure)
        {
            if (this.readOnly)
                return;

            selectionContainer.ClearSelection();
            selectionContainer.AddSelection(figure);
            FigureSelected?.Invoke(new List<Figure>(new Figure[] { figure }));
        }

        public void SelectFigure(List<Figure> figureList)
        {
            if (this.readOnly)
                return;

            selectionContainer.ClearSelection();
            selectionContainer.AddSelection(figureList);
            FigureSelected?.Invoke(new List<Figure>(figureList));
        }

        public void SelectFigureByTag(Object tag)
        {
            if (this.readOnly)
                return;

            Figure selectFigure = workingFigures.GetFigureByTag(tag);
            SelectFigure(selectFigure);
        }

        public void SelectFigureByTag(List<Object> tagList)
        {
            if (this.readOnly)
                return;

            List<Figure> selectFigureList = tagList.Select(f => workingFigures.GetFigureByTag(f)).ToList();
            SelectFigure(selectFigureList);
        }

        /// <summary>
        /// 외부 Param Controller 등으로 인해 수정된 경우 호출.
        /// 외부 요인으로 인해 수정된 내용 반영하기 위함.
        /// </summary>
        /// <param name="figure">수정된 Figure</param>
        public void OnSelectionUpdated(Figure figure)
        {
            this.selectionContainer.OnSelectionUpdated(figure);
        }

        public void OnSelectionUpdated(List<Figure> figureList)
        {
            this.selectionContainer.OnSelectionUpdated(figureList);
        }

        public void DeleteSelection(Figure figure)
        {
            List<Figure> figureList = selectionContainer.GetRealFigures();
            selectionContainer.RemoveSelection(figure);

            selectionContainer.ClearSelection();
            Invalidate(true);
        }

        public Figure[] GetSelectedFigures()
        {
            return this.selectionContainer.Figures.ToArray();
        }

        public void ClearSelection()
        {
            selectionContainer.ClearSelection();
            Invalidate(true);
        }

        #endregion

        public void UpdateImage(Bitmap bitmap, bool zoomFit)
        {
            UpdateImage(bitmap);
            if (zoomFit)
                ZoomFit();
        }

        public void UpdateImage(Bitmap bitmap)
        {
            //RectangleF viewPort = bitmap == null ? RectangleF.Empty : new RectangleF(Point.Empty, bitmap.Size);
            RectangleF viewPort = bitmap == null ? RectangleF.Empty : new RectangleF(Point.Empty, bitmap.Size);
            if (this.ViewPort.IntersectsWith(viewPort))
                viewPort = this.ViewPort;
            UpdateImage(bitmap, viewPort);
        }

        public void UpdateImage(Bitmap bitmap, RectangleF viewPort)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateImageDelegate(UpdateImage), bitmap, viewPort);
                return;
            }

            lock (this)
            {
                if (bitmap == null)
                {
                    //this.image?.Dispose();
                    this.image = null;
                    this.ImageSize = Size.Empty;

                    Invalidate();

                    return;
                }
                this.image = bitmap;
                this.ImageSize = bitmap.Size;

                ZoomRange(viewPort);
            }

        }

        private List<GraphicsPath> GetTrackPath()
        {
            List<GraphicsPath> graphicsPathList = new List<GraphicsPath>();

            CoordMapper coordMapper = GetCoordMapper();
            PointF scaledDragStart = coordMapper.PixelToWorld(dragStart);
            PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);
            SizeF scaledDragOffset = new SizeF(0, 0); // coordMapper.PixelToWorld(dragOffset);

            GraphicsPath graphicsPath = new GraphicsPath();
            switch (this.dragMode)
            {
                case DragMode.Pan:
                    //graphicsPath.AddLine(scaledDragStart, scaledDragEnd);
                    break;
                case DragMode.Add:
                case DragMode.Measure:
                    AddTrackerGraphicPath(graphicsPath, scaledDragStart, scaledDragEnd);
                    break;
                case DragMode.Select:
                    graphicsPath.AddRectangle(DrawingHelper.FromPoints(scaledDragStart, scaledDragEnd));
                    break;
                case DragMode.Modify:
                    this.selectionContainer.Figures.ForEach(f => graphicsPath.AddPath(f.GetGraphicsPath(), false));
                    break;
            }
            graphicsPathList.Add(graphicsPath);

            return graphicsPathList;
        }

        private void AddTrackerGraphicPath(GraphicsPath graphicsPath, PointF scaledDragStart, PointF scaledDragEnd)
        {
            switch (trackerShape)
            {
                default:
                case FigureType.Rectangle:
                    if (singleAxisTracking)
                    {
                        SizeF dragOffset = new SizeF(scaledDragEnd.X - scaledDragStart.X, scaledDragEnd.Y - scaledDragStart.Y);
                        if (Math.Abs(dragOffset.Width) > Math.Abs(dragOffset.Height))
                            scaledDragEnd = PointF.Add(scaledDragStart, new SizeF(dragOffset.Width, dragOffset.Width));
                        else
                            scaledDragEnd = PointF.Add(scaledDragStart, new SizeF(dragOffset.Height, dragOffset.Height));
                    }
                    graphicsPath.AddRectangle(DrawingHelper.FromPoints(scaledDragStart, scaledDragEnd));
                    break;

                case FigureType.Ellipse:
                    int length = (int)Math.Round(MathHelper.GetLength(scaledDragStart, scaledDragEnd) * 2);
                    graphicsPath.AddEllipse(DrawingHelper.FromCenterSize(scaledDragStart, new Size(length, length)));
                    break;

                case FigureType.Line:
                    if (singleAxisTracking)
                    {
                        if (Math.Abs(dragOffset.Width) > Math.Abs(dragOffset.Height))
                            graphicsPath.AddLine(scaledDragStart, new PointF(scaledDragEnd.X, scaledDragStart.Y));
                        else
                            graphicsPath.AddLine(scaledDragStart, new PointF(scaledDragStart.X, scaledDragEnd.Y));
                    }
                    else
                    {
                        graphicsPath.AddLine(scaledDragStart, scaledDragEnd);
                    }
                    break;

                case FigureType.Polygon:
                    graphicsPath.AddLines(this.trackPointList.ToArray());
                    break;
            }
        }

        CoordMapper GetCoordMapper()
        {
            Matrix m = new Matrix();

            if (invertY)
            {
                m.Scale(this.ZoomScale.Width, -this.ZoomScale.Height);
                m.Translate(-this.ViewPort.X, -(this.ViewPort.Height + this.ViewPort.Y));
            }
            else
            {
                m.Scale(this.ZoomScale.Width, this.ZoomScale.Height);
                m.Translate(-this.ViewPort.X, -this.ViewPort.Y);
            }

            if (this.dragMode == DragMode.Pan)
                m.Translate(dragOffset.Width, dragOffset.Height, MatrixOrder.Append);

            return new CoordMapper(m);
        }

        #region Events
        private void CanvasPanel_Leave(object sender, EventArgs e)
        {
            MouseLeaved?.Invoke(this);
        }

        private void CanvasPanel_DoubleClick(object sender, EventArgs e)
        {
            //TempFigures.Clear();
            //Invalidate();

            MouseDblClicked?.Invoke(this);
        }

        private void CanvasPanel_Load(object sender, EventArgs e)
        {
            //if (hideToolbar == true)
            //    statusBar.Height = 0;
            //statusBar.Visible = (hideToolbar == false);
        }

        private void CanvasPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
            {
                if (e.KeyCode == Keys.C)
                {
                    Copy();
                }
                else if (e.KeyCode == Keys.V)
                {
                    Paste();
                }
            }
            else
            {
                int accel = (Control.ModifierKeys == Keys.Alt ? 5 : 1);

                switch (e.KeyCode)
                {
                    case Keys.Delete:
                        Remove();
                        break;
                    case Keys.Z:
                        hideFigure = !hideFigure;
                        Invalidate();
                        break;
                    case Keys.Q:
                        ZoomIn();
                        break;
                    case Keys.W:
                        ZoomOut();
                        break;
                    case Keys.Left:
                        Offset(new SizeF(-accel, 0));
                        break;
                    case Keys.Right:
                        Offset(new SizeF(accel, 0));
                        break;
                    case Keys.Up:
                        Offset(new SizeF(0, -accel));
                        break;
                    case Keys.Down:
                        Offset(new SizeF(0, accel));
                        break;
                }
            }
        }

        private void CanvasPanel_Paint(object sender, PaintEventArgs e)
        {
            if (!System.Threading.Monitor.TryEnter(this))
            {
                System.Diagnostics.Debug.WriteLine("lock힝");
                return;
            }

            Stopwatch sw = Stopwatch.StartNew();

            int clientHeight = this.ClientHeight;

            Rectangle clientRect = new Rectangle(0, 0, Width, clientHeight);
            e.Graphics.SetClip(clientRect, CombineMode.Replace);

            CoordMapper newCoordMapper = GetCoordMapper();
            if (!newCoordMapper.Matrix.IsInvertible)
            {
                System.Threading.Monitor.Exit(this);
                return;
            }

            if (image != null)
            {
                e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
                e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
                e.Graphics.CompositingMode = CompositingMode.SourceOver;
                try
                {
                    bool fModeUsable = (this.ZoomScale.Width < 1);
                    if (this.fastMode && fModeUsable)
                    {
                        float zoomScale = Math.Min(this.ZoomScale.Width, this.ZoomScale.Height);
                        RectangleF srcRect = this.ViewPort;
                        RectangleF validSrcRect = RectangleF.Intersect(new RectangleF(Point.Empty, this.ImageSize), this.ViewPort);
                        //Debug.WriteLine("CanvasPanel_Paint::validSrcRect - " + validSrcRect.ToString());

                        Rectangle rect = Rectangle.Truncate(validSrcRect);
                        if (rect.Width > 0 && rect.Height > 0)
                        {
                            Bitmap resizeBitmap = ImageHelper.CopyResize(this.image, rect, zoomScale);
                            //Debug.WriteLine("CanvasPanel_Paint::CopyResize {0}ms", sw.ElapsedMilliseconds);

                            //Bitmap clipBirmap = this.image.Clone(validSrcRect, this.image.PixelFormat);
                            //Debug.WriteLine("CanvasPanel_Paint::CloneImage {0}ms", sw.ElapsedMilliseconds);
                            //Bitmap resizeBitmap = ImageHelper.Resize(clipBirmap, this.zoomScale, this.zoomScale);
                            //Debug.WriteLine("CanvasPanel_Paint::ResizeImage {0}ms", sw.ElapsedMilliseconds);
                            RectangleF displayRect = newCoordMapper.Transform(validSrcRect);
                            e.Graphics.DrawImage(resizeBitmap, displayRect.X, displayRect.Y);
                        }
                    }
                    else
                    {
                        e.Graphics.Transform = newCoordMapper.Matrix;
                        e.Graphics.DrawImage(this.image, Point.Empty);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(LoggerType.Operation, string.Format("CanvasPanel::CanvalPanel_Paint - {0}", ex.Message));
                }

                //e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                //e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Default;
                //Debug.WriteLine("CanvasPanel_Paint::DrawImage {0}ms", sw.ElapsedMilliseconds);
            }

            e.Graphics.Transform = newCoordMapper.Matrix;

            if (onMouseDown)
            {
                float penWidth = 3 / Math.Max(this.ZoomScale.Width, this.ZoomScale.Height);
                using (Pen p = new Pen(Color.Red, penWidth))
                {
                    p.DashStyle = DashStyle.Dot;

                    List<GraphicsPath> trackPathList = GetTrackPath();
                    foreach (GraphicsPath graphicsPath in trackPathList)
                    {
                        e.Graphics.DrawPath(p, graphicsPath);
                    }
                }
            }

            if (hideFigure == false)
            {
                if (workingFigures != null)
                    lock (workingFigures)
                        workingFigures.Draw(e.Graphics, null, false);
                if (tempFigures != null)
                    lock (tempFigures)
                        tempFigures.Draw(e.Graphics, null, false);
                if (backgroundFigures != null)
                    lock (backgroundFigures)
                        backgroundFigures.Draw(e.Graphics, null, false);
            }

            //if (focusedFigure != null)
            //{
            //    using (Pen p = new Pen(Color.OrangeRed, 0))
            //    {
            //        p.DashStyle = DashStyle.Dot;

            //        GraphicsPath focusedFigurePath = focusedFigure.GetGraphicsPath();
            //        e.Graphics.DrawPath(p, focusedFigurePath);
            //    }
            //}

            //selectionContainer.Draw(e.Graphics, null, rotationLocked);

            e.Graphics.Transform.Reset();
            e.Graphics.Transform = new Matrix();

            bool showCenterGuide = this.showCenterGuide || this.dragMode == DragMode.Measure || this.dragMode == DragMode.Cross;
            if (showCenterGuide)
            {
                Pen pen = new Pen(Color.Blue, centerGuideThickness)
                {
                    DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
                };

                e.Graphics.DrawLine(pen, new PointF(0, clientHeight / 2 + centerGuidePos.X), new PointF(Width, clientHeight / 2 + centerGuidePos.X));
                e.Graphics.DrawLine(pen, new PointF(Width / 2 + centerGuidePos.Y, 0), new PointF(Width / 2 + centerGuidePos.Y, clientHeight));
            }

            if (!this.readOnly)
                selectionContainer.Draw(e.Graphics, newCoordMapper, rotationLocked);

            //Debug.WriteLine("CanvasPanel_Paint::DrawAll {0}ms", sw.ElapsedMilliseconds);
            System.Threading.Monitor.Exit(this);
        }

        private void CanvasPanel_SizeChanged(object sender, EventArgs e)
        {
            Rectangle clientRect = new Rectangle(0, 0, Width, this.ClientHeight);
            this.ViewPort = new RectangleF(this.ViewPort.Location, new SizeF(clientRect.Width / this.ZoomScale.Width, clientRect.Height / this.ZoomScale.Height));
            Invalidate();
        }

        private void CanvasPanel_MouseDown(object sender, MouseEventArgs e)
        {
            dragStart = dragEnd = new PointF(e.X, e.Y);
            dragOffset = new SizeF(0, 0);
            onMouseDown = true;

            if (dragMode == DragMode.Select || dragMode == DragMode.Pan)
            {
                CoordMapper coordMapper = GetCoordMapper();
                curTrackPos = selectionContainer.GetTrackPos(coordMapper, dragStart, rotationLocked);

                if (!this.readOnly && curTrackPos.PosType != TrackPosType.None)
                {
                    this.tempDragMode = this.dragMode;
                    this.dragMode = DragMode.Modify;
                    UpdateCursor();
                }
            }

            UpdateCursor();
        }

        private void CanvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            CoordMapper coordMapper = GetCoordMapper();
            PointF point = coordMapper.PixelToWorld(new PointF(e.X, e.Y));
            statusBar.Panels["Pos"].Text = String.Format("{0:0.00}, {1:0.00}", point.X, point.Y);

            if (onMouseDown == true)
            {
                if (this.singleAxisTracking)
                {
                    if (dragEnd.X > dragEnd.Y)
                        dragEnd = new PointF(dragEnd.X, dragStart.Y);
                    else
                        dragEnd = new PointF(dragStart.X, dragEnd.Y);
                }

                dragOffset = new SizeF(e.X - dragStart.X, e.Y - dragStart.Y);
                SizeF dragDelta = new SizeF(e.X - dragEnd.X, e.Y - dragEnd.Y);
                dragEnd = new PointF(e.X, e.Y);

                SizeF size = coordMapper.PixelToWorld(dragOffset);
                statusBar.Panels["Size"].Text = String.Format("{0:0.00}, {1:0.00}", size.Width, size.Height);

                if (dragMode == DragMode.Modify)
                {
                    SizeF size2 = coordMapper.PixelToWorld(dragDelta);
                    selectionContainer.TrackMove(curTrackPos, size2, rotationLocked, false);
                }
                Invalidate(true);
            }
            else
            {
                this.enteredFigure = this.workingFigures.Select(point, false);
                if (this.enteredFigure != this.lastEnterdFigure)
                {
                    if (this.lastEnterdFigure != null)
                        FigureMouseLeave?.Invoke(this.lastEnterdFigure);
                    if (this.enteredFigure != null)
                        FigureMouseEnter?.Invoke(this.enteredFigure);

                    //Invalidate(true);
                    this.lastEnterdFigure = this.enteredFigure;
                }
            }
        }

        private void CanvasPanel_MouseUp(object sender, MouseEventArgs e)
        {
            dragEnd = new PointF(e.X, e.Y);
            dragOffset = new SizeF(e.X - dragStart.X, e.Y - dragStart.Y);

            CoordMapper coordMapper = GetCoordMapper();
            PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);

            bool processingCancelled = false;
            if (dragStart == dragEnd)
            {
                Figure selectedFigure = this.workingFigures.FigureList.Find(f => f.Selectable && f.GetRectangle().GetBoundRect().Contains(scaledDragEnd));
                if (selectedFigure != null)
                    FigureClicked?.Invoke(selectedFigure, e);
                else
                    MouseClicked?.Invoke(this, scaledDragEnd, ref processingCancelled);

                if (!processingCancelled)
                    SelectRange();
            }
            else if (onMouseDown == true)
            {
                switch (this.dragMode)
                {
                    case DragMode.Add:
                        Add();
                        break;
                    case DragMode.Pan:
                        Pan();
                        break;
                    case DragMode.Select:
                        SelectRange();
                        break;
                    case DragMode.Modify:
                        Modify();
                        break;
                    case DragMode.Measure:
                        Measure();
                        break;
                    case DragMode.Zoom:
                        ZoomRange();
                        break;
                }

                statusBar.Panels["Size"].Text = "";
                dragOffset = new SizeF(0, 0);
            }
            onMouseDown = false;
            if (this.tempDragMode != DragMode.None)
            {
                this.dragMode = this.tempDragMode;
                this.tempDragMode = DragMode.None;
                UpdateCursor();
            }
            Invalidate();
        }

        private void CanvasPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.onMouseDown || this.onkeyDown)
                return;

            this.onkeyDown = true;
            //Debug.WriteLine("CanvasPanel_KeyDown");
            switch (e.KeyCode)
            {
                case Keys.Space:
                    this.tempDragMode = this.dragMode;
                    this.dragMode = DragMode.Pan;
                    break;
                case Keys.ShiftKey:
                    this.singleAxisTracking = true;
                    break;
            }
            UpdateCursor();

        }

        private void CanvasPanel_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.onMouseDown || !this.onkeyDown)
                return;

            this.onkeyDown = false;
            Debug.WriteLine("CanvasPanel_KeyUp");
            switch (e.KeyCode)
            {
                case Keys.Space:
                    this.dragMode = this.tempDragMode;
                    this.tempDragMode = DragMode.None;
                    break;
                case Keys.ShiftKey:
                    this.singleAxisTracking = false;
                    break;
            }
            UpdateCursor();

            //this.curDragMode = this.dragMode;
            //if (e.KeyCode == Keys.Delete)
            //{
            //    workingFigures.Delete(selectionContainer.Figures);
            //    selectionContainer.ClearSelection();
            //    return;
            //}

            //Size offset = new Size(0, 0);
            //if (e.KeyCode == Keys.Down)
            //    offset.Height = 1;
            //else if (e.KeyCode == Keys.Up)
            //    offset.Height = -1;
            //else if (e.KeyCode == Keys.Left)
            //    offset.Width = -1;
            //else if (e.KeyCode == Keys.Right)
            //    offset.Width = 1;

            //if (offset != new Size(0, 0))
            //{
            //    CoordMapper coordMapper = GetCoordMapper();
            //    selectionContainer.Offset(coordMapper.PixelToWorld(offset));
            //}
            //else
            //{
            //    int clientHeight = Height - statusBar.Height;

            //    Rectangle clientRect = new Rectangle(0, 0, Width, clientHeight);

            //    if (e.KeyData == Keys.Z)
            //        UpdateZoom(1.2f, DrawingHelper.CenterPoint(clientRect));
            //    else if (e.KeyData == Keys.X)
            //        UpdateZoom(0.8f, DrawingHelper.CenterPoint(clientRect));
            //}
        }

        private void statusBar_ButtonClick(object sender, Infragistics.Win.UltraWinStatusBar.PanelEventArgs e)
        {
            if (onUpdateStateButton == true)
                return;

            this.selectionContainer.ClearSelection();
            switch (e.Panel.Key)
            {
                case "Pan":
                    SetPanMode();
                    break;
                case "Select":
                    SetSelectMode();
                    break;
                case "Cross":
                    SetPointMode();
                    break;
                case "Rectangle":
                    SetMeasureMode(FigureType.Rectangle);
                    break;
                case "Circle":
                    SetMeasureMode(FigureType.Ellipse);
                    break;

                case "ZoomFit":
                    ZoomFit();
                    break;
                case "ZoomIn":
                    ZoomIn();
                    break;
                case "ZoomOut":
                    ZoomOut();
                    break;
                case "ZoomRange":
                    SetZoomMode();
                    break;
            }

            UpdateCursor();
            UpdateStateButton();

            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            UpdateZoom((float)(e.Delta > 0 ? 1.5 : 0.5), new PointF(e.X, e.Y));
        }

        #endregion

        private void UpdateCursor()
        {
            switch (this.dragMode)
            {
                case DragMode.Pan:
                    Cursor = Cursors.Hand;
                    break;
                case DragMode.Add:
                case DragMode.Measure:
                case DragMode.Zoom:
                    Cursor = Cursors.Cross;
                    //Cursor = new Cursor(new System.IO.MemoryStream(DynMvp.Properties.Resources.zoom_in));
                    //Cursor = new Cursor(GetType(), "zoom_in.cur");
                    //Cursor = new Cursor(@"D:\Project\PrintEye\Source\DynMvp\Resources\zoom-in.cur");
                    break;
                case DragMode.Select:
                default:
                    Cursor = Cursors.Arrow;
                    break;
            }
        }

        private void Add()
        {
            selectionContainer.ClearSelection();

            if (Math.Abs(dragOffset.Width) < 5 && Math.Abs(dragOffset.Height) < 5)
                return;

            CoordMapper coordMapper = GetCoordMapper();
            PointF scaledDragStart = coordMapper.PixelToWorld(dragStart);
            PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);

            Figure figure = null;

            // 적용 X
            //if (trackerShape == FigureType.Custom)
            //{
            //    if (CreateCustomFigure != null)
            //        figure = CreateCustomFigure(scaledDragStart, scaledDragEnd);
            //}
            //else
            //{
            //    figure = CreateFigure(scaledDragStart, scaledDragEnd);
            //}

            figure = CreateFigure(scaledDragStart, scaledDragEnd);
            if (figure != null)
            {
                workingFigures.AddFigure(figure);
                FigureCreated?.Invoke(figure, coordMapper);
                //selectionContainer.AddSelection(figure);
            }

            //SetSelectMode();

            Invalidate();
        }

        private Figure CreateFigure(PointF pt1, PointF pt2)
        {
            Figure figure = null;
            switch (trackerShape)
            {
                case FigureType.Ellipse:
                    figure = new EllipseFigure(DrawingHelper.FromPoints(pt1, pt2), new Pen(Color.Red));
                    break;
                case FigureType.Rectangle:
                    figure = new RectangleFigure(DrawingHelper.FromPoints(pt1, pt2), new Pen(Color.Red));
                    break;
            }

            return figure;
        }

        private void Remove()
        {
            List<Figure> selectedFigureList = selectionContainer.GetRealFigures();
            selectionContainer.ClearSelection();

            workingFigures.RemoveFigure(selectedFigureList);
            FigureRemoved?.Invoke(selectedFigureList);

            Invalidate();
        }

        private void Pan()
        {
            int clientHeight = this.ClientHeight;

            Rectangle clientRect = new Rectangle(0, 0, Width, clientHeight);

            if (invertY)
                this.ViewPort = DrawingHelper.Offset(this.ViewPort, new SizeF(-dragOffset.Width / this.ZoomScale.Width, +dragOffset.Height / this.ZoomScale.Height));
            else
                this.ViewPort = DrawingHelper.Offset(this.ViewPort, new SizeF(-dragOffset.Width / this.ZoomScale.Width, -dragOffset.Height / this.ZoomScale.Height));
        }

        private void Modify()
        {
            CoordMapper coordMapper = GetCoordMapper();
            SizeF scaledDragOffset = coordMapper.PixelToWorld(dragOffset);

            selectionContainer.TrackMove(curTrackPos, scaledDragOffset, rotationLocked, true);

            //if (curTrackPos.PosType == TrackPosType.Inner)
            //    selectionContainer.Offset(scaledDragOffset);
            //else
            //    selectionContainer.TrackMove(curTrackPos, scaledDragOffset, rotationLocked, true);

            if (FigureModified != null)
            {
                List<Figure> realFigureList = selectionContainer.GetRealFigures();
                FigureModified(realFigureList);
            }

            Invalidate(true);
        }

        private void SelectRange()
        {
            if (Control.ModifierKeys != Keys.Control)
                selectionContainer.ClearSelection();

            CoordMapper coordMapper = GetCoordMapper();

            List<Figure> figureList = new List<Figure>();

            PointF scaledDragStart = coordMapper.PixelToWorld(dragStart);

            // 선택 영역이 작으면 시작위치에 있는 객체를 얻어오고
            // 선택 영역이 크면 시작 위치와 종료 위치의 Rectangle 영역안의 객체 목록을 얻어 온다.
            if (Math.Abs(dragOffset.Width) > 5 && Math.Abs(dragOffset.Height) > 5)
            {
                PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);

                RectangleF selectedRect = DrawingHelper.FromPoints(scaledDragStart, scaledDragEnd);
                figureList = workingFigures.Select(Rectangle.Round(selectedRect));
            }
            else
            {
                Figure figure = workingFigures.Select(scaledDragStart);
                if (figure != null)
                    figureList.Add(figure);
            }
            selectionContainer.AddSelection(figureList);
            FigureSelected?.Invoke(figureList);
        }

        private void Measure()
        {
            //dragOffset
        }

        private void ZoomRange()
        {
            CoordMapper coordMapper = GetCoordMapper();

            PointF scaledDragStart = coordMapper.PixelToWorld(dragStart);
            PointF scaledDragEnd = coordMapper.PixelToWorld(dragEnd);

            ZoomRange(DrawingHelper.FromPoints(scaledDragStart, scaledDragEnd));
        }

        private void UpdateZoom(float zoomOffset, PointF zoomCenter)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateZoomDelegate(UpdateZoom), zoomOffset, zoomCenter);
                return;
            }

            SizeF newZoomScale = DrawingHelper.Mul(this.ZoomScale, zoomOffset); // (float)Math.Pow(2, zoomStep);
            if (newZoomScale.Width == 0 || newZoomScale.Height == 0)
                return;

            Rectangle clientRect = new Rectangle(0, 0, Width, this.ClientHeight);

            SizeF newCanvasSize = new SizeF(clientRect.Width / newZoomScale.Width, clientRect.Height / newZoomScale.Height);

            PointF curZoomPos;
            PointF newLeftTopPos;

            if (invertY)
            {
                curZoomPos = new PointF(this.ViewPort.X + zoomCenter.X / this.ZoomScale.Width, this.ViewPort.Y + (clientRect.Height - zoomCenter.Y) / this.ZoomScale.Height);
                newLeftTopPos = new PointF(curZoomPos.X - zoomCenter.X / newZoomScale.Width, curZoomPos.Y - (clientRect.Height - zoomCenter.Y) / newZoomScale.Height);
            }
            else
            {
                curZoomPos = new PointF(this.ViewPort.X + zoomCenter.X / this.ZoomScale.Width, this.ViewPort.Y + zoomCenter.Y / this.ZoomScale.Height);
                newLeftTopPos = new PointF(curZoomPos.X - zoomCenter.X / newZoomScale.Width, curZoomPos.Y - zoomCenter.Y / newZoomScale.Height);
            }

            this.ViewPort = new RectangleF(newLeftTopPos, newCanvasSize);
            this.ZoomScale = newZoomScale;

            Invalidate();
        }

        public void ZoomFit()
        {
            RectangleF boundRect = GetBoundRect();
            ZoomRange(boundRect);
        }

        public void ZoomRange(RectangleF zoomRange)
        {
            if (InvokeRequired)
            {
                Invoke(new ZoomRangeDelegate(ZoomRange), zoomRange);
                return;
            }

            if (zoomRange.Width == 0 || zoomRange.Height == 0)
                return;

            int clientHeight = this.ClientHeight;

            float scaleX = Width / zoomRange.Width;
            float scaleY = clientHeight / zoomRange.Height;
            if (scaleX <= 0 || scaleY <= 0)
                return;

            RectangleF canvasRegion = RectangleF.Empty;

            if (scaleX < scaleY)
            {
                // Width 일치
                //canvasRegion = new RectangleF(zoomRange.X, zoomRange.Y, zoomRange.Width, clientHeight / scaleX);
                this.ZoomScale = new SizeF(scaleX, scaleX);

                float canvasRegionH = clientHeight / scaleX;
                float canvasRegionCenterY = (canvasRegionH - zoomRange.Height) / 2;
                if (canvasRegionCenterY < 0)
                    canvasRegionCenterY = 0;

                canvasRegionH -= canvasRegionCenterY;
                canvasRegion = RectangleF.FromLTRB(zoomRange.Left, zoomRange.Top - canvasRegionCenterY, zoomRange.Right, zoomRange.Bottom + canvasRegionCenterY);
            }
            else
            {
                // Height 일치

                //canvasRegion = new RectangleF(zoomRange.X, zoomRange.Y, Width / scaleY, zoomRange.Height);
                this.ZoomScale = new SizeF(scaleY, scaleY);

                float canvasRegionW = Width / scaleY;
                float canvasRegionCenterX = (canvasRegionW - zoomRange.Width) / 2;
                if (canvasRegionCenterX < 0)
                    canvasRegionCenterX = 0;

                canvasRegionW -= canvasRegionCenterX;

                if (horizontalAlignment == HorizontalAlignment.Left)
                {
                    //canvasRegion = RectangleF.FromLTRB(zoomRange.Left, zoomRange.Top, Width / scaleY, zoomRange.Bottom);
                    canvasRegion = RectangleF.FromLTRB(zoomRange.Left - 0 * canvasRegionCenterX, zoomRange.Top, zoomRange.Right + 2 * canvasRegionCenterX, zoomRange.Bottom);
                }
                else if (horizontalAlignment == HorizontalAlignment.Center)
                {
                    canvasRegion = RectangleF.FromLTRB(zoomRange.Left - 1 * canvasRegionCenterX, zoomRange.Top, zoomRange.Right + 1 * canvasRegionCenterX, zoomRange.Bottom);
                }
                else if (horizontalAlignment == HorizontalAlignment.Right)
                {
                    canvasRegion = RectangleF.FromLTRB(zoomRange.Left - 2 * canvasRegionCenterX, zoomRange.Top, zoomRange.Right + 0 * canvasRegionCenterX, zoomRange.Bottom);
                }
            }

            if (this.ZoomScale.Width != scaleX || this.ZoomScale.Height != scaleY || canvasRegion != this.ViewPort)
            {
                this.ViewPort = canvasRegion;
                Invalidate(true);
            }
        }

        public void ZoomIn()
        {
            int clientHeight = this.ClientHeight;
            Rectangle clientRect = new Rectangle(0, 0, Width, clientHeight);

            UpdateZoom(1.2f, DrawingHelper.CenterPoint(clientRect));
        }

        public void ZoomOut()
        {
            int clientHeight = this.ClientHeight;
            Rectangle clientRect = new Rectangle(0, 0, Width, clientHeight);

            UpdateZoom(0.8f, DrawingHelper.CenterPoint(clientRect));
        }

        public void Copy()
        {
            copyCount = 1;
            copyBuffer.Clear();
            foreach (Figure selectedFigure in selectionContainer)
            {
                copyBuffer.Add(selectedFigure.Tag as Figure);
            }
            copyBuffer.RemoveAll(f => f == null);
            FigureCopied?.Invoke(copyBuffer);
        }

        public void Paste()
        {
            CoordMapper coordMapper = GetCoordMapper();
            SizeF pasteOffset = coordMapper.PixelToWorld(new Size(10 * copyCount, 10 * copyCount));

            FigureGroup workingFigures = new FigureGroup();

            FigurePasted?.Invoke(copyBuffer, workingFigures, backgroundFigures, pasteOffset);

            foreach (Figure figure in workingFigures)
                this.workingFigures.AddFigure(figure);

            copyCount++;

            Invalidate(true);
        }

        void UpdateStateButton()
        {
            onUpdateStateButton = true;

            statusBar.Panels["Pan"].Checked = (tempDragMode == DragMode.Pan);
            statusBar.Panels["Select"].Checked = (tempDragMode == DragMode.Select);
            statusBar.Panels["Cross"].Checked = (tempDragMode == DragMode.Cross);
            statusBar.Panels["Rectangle"].Checked = (tempDragMode == DragMode.Measure && trackerShape == FigureType.Rectangle);
            statusBar.Panels["Circle"].Checked = (tempDragMode == DragMode.Measure && trackerShape == FigureType.Ellipse);
            statusBar.Panels["ZoomRange"].Checked = (tempDragMode == DragMode.Zoom);

            onUpdateStateButton = false;
        }

        void Offset(SizeF size)
        {
            CoordMapper coordMapper = GetCoordMapper();
            SizeF scaledDragOffset = coordMapper.PixelToWorld(size);

            selectionContainer.Offset(scaledDragOffset);
            selectionContainer.TrackMove(new TrackPos(TrackPosType.Inner, 0), scaledDragOffset, true, false);

            if (FigureModified != null)
            {
                FigureModified(selectionContainer.GetRealFigures());
            }

            Invalidate(true);
            Update();
        }

        public Point PointToImage(PointF point)
        {
            PointF pt = new PointF(
                point.X / this.ZoomScale.Width + this.ViewPort.X,
                point.Y / this.ZoomScale.Height + this.ViewPort.Y);
            return Point.Round(pt);
        }

        public Color GetPixel(Point point)
        {
            if (this.image == null)
                return Color.Transparent;

            return this.image.GetPixel(point.X, point.Y);
        }

    }
}
