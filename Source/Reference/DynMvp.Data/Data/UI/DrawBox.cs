using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.Devices;
using DynMvp.UI;
using System.Diagnostics;
using DynMvp.Vision;
using DynMvp.Devices.FrameGrabber;
using System.Threading;
using DynMvp.Authentication;

namespace DynMvp.Data.UI
{
    public delegate void AddRegionCapturedDelegate(Rectangle rectangle, Point startPoint, Point endPoint);
    public delegate void FigureSelectedDelegate(Figure figure, bool select = true);
    public delegate void FigureMultiSelectedDelegate(List<Figure> figureList, bool select = true);
    public delegate bool FigureSelectableDelegate(Figure figure);
    public delegate void FigureAddDelegate(List<PointF> pointList, FigureType figureType);
    public delegate void FigureMovedDelegate(List<Figure> figureList);
    public delegate void FigureCopyDelegate(List<Figure> figureList);
    public delegate void MouseClickedDelegate(DrawBox senderView, Point clickPos, ref bool processingCancelled);
    public delegate void MouseMovedDelegate(DrawBox senderView, Point movedPos, Image image, MouseEventArgs e, ref bool processingCancelled);
    public delegate void MouseDoubleClickedDelegate(DrawBox senderView);
    public delegate void ImageUpdatedDelegate();
    public enum AutoFitStyle { FitAll, FitWidthOnly, FitHeigthOnly, KeepRatio }

    public class DrawBoxOption
    {
        bool lockDoubleClick = false;
        public bool LockDoubleClick
        {
            get { return lockDoubleClick; }
            set { lockDoubleClick = value; }
        }

        bool lockMouseWheel = false;
        public bool LockMouseWheel
        {
            get { return lockMouseWheel; }
            set { lockMouseWheel = value; }
        }
        Color backColor;
        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }
    }


    public class DrawBox : UserControl
    {
        public PictureBox pictureBox;
        private Tracker tracker;
        public FigureAddDelegate FigureAdd;
        public FigureSelectedDelegate FigureSelected;
        public FigureMultiSelectedDelegate FigureMultiSelected;
        public FigureSelectableDelegate FigureSelectable;
        public FigureMovedDelegate FigureMoved;
        public FigureCopyDelegate FigureCopy;
        public MouseClickedDelegate MouseClicked;
        public MouseMovedDelegate MouseMoved;
        public MouseDoubleClickedDelegate MouseDoubleClicked;
        public PositionShiftedDelegate PositionShifted;

        int index = 0;
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        private Rectangle zoomDisplayRect;
        private RectangleF displayRect = RectangleF.Empty;
        public RectangleF DisplayRect
        {
            get { return displayRect; }
            set { displayRect = value; }
        }

        private Calibration calibration;
        public Calibration Calibration
        {
            get { return calibration; }
            set
            {
                calibration = value;
            }
        }

        object updateLockObject = new object();
        //ImageD imageD = null;
        //public ImageD ImageD
        //{
        //    get { return imageD; }
        //}

        private Image imageBuffer;
        public Image ImageBuffer
        {
            get { return imageBuffer; }
        }

        public Image Image
        {
            get { return pictureBox.Image; }
            //    //set { UpdateImage(value); }
        }

        private Image3D image3d;
        public Image3D Image3d
        {
            get { return image3d; }
            set
            {
                image3d = value;
                //pictureBox.Image = value.ToBitmap();
            }
        }

        private Bitmap overlayImage;
        public Bitmap OverlayImage
        {
            get { return overlayImage; }
            set
            {
                overlayImage = value;
                overlayPos = new Point(0, 0);
            }
        }

        private Point overlayPos;
        public Point OverlayPos
        {
            get { return overlayPos; }
            set { overlayPos = value; }
        }

        private SizeF positionOffset;
        public SizeF PositionOffset
        {
            get { return positionOffset; }
            set { positionOffset = value; }
        }

        AutoFitStyle autoFitStyle = AutoFitStyle.KeepRatio;
        public AutoFitStyle AutoFitStyle
        {
            get { return autoFitStyle; }
            set { autoFitStyle = value; }
        }

        private SizeF zoomScale = new SizeF(-1,-1);
        public SizeF ZoomScale
        {
            get { return zoomScale; }
            set { zoomScale = value; }
        }

        private SizeF zoomOffset = new SizeF(0, 0);
        public SizeF ZoomOffset
        {
            get { return zoomOffset; }
        }

        private Point viewOffset;
        public Point ViewOffset
        {
            get { return viewOffset; }
            set { viewOffset = value; }
        }

        FigureGroup figureGroup = new FigureGroup();
        public FigureGroup FigureGroup
        {
            get { return figureGroup; }
            set
            {
                //ResetSelection();
                //if (value == null)
                //    figureGroup = new FigureGroup();
                //else
                figureGroup = value;
            }
        }

        FigureGroup backgroundFigures = new FigureGroup();
        public FigureGroup BackgroundFigures
        {
            get { return backgroundFigures; }
            set { backgroundFigures = value; }
        }

        FigureGroup tempFigureGroup = new FigureGroup();
        public FigureGroup TempFigureGroup
        {
            get { return tempFigureGroup; }
            set { tempFigureGroup = value; }
        }

        FigureGroup customFigure = new FigureGroup();
        public FigureGroup CustomFigure
        {
            get { return customFigure; }
            set { customFigure = value; }
        }

        public FigureType TrackerShape
        {
            get { return tracker.Shape; }
            set { tracker.Shape = value; }
        }

        bool showCenterGuide;
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

        ToolTip toolTip = new ToolTip();

        bool showTooltip;
        public bool ShowTooltip
        {
            get { return showTooltip; }
            set { showTooltip = value; }
        }

        public void SetMeasureScale(float scaleX, float scaleY)
        {
            throw new NotImplementedException();
        }
        
        private bool enable = false;
        public bool Enable
        {
            get { return enable; }
            set
            {
                enable = value;
                tracker.Enable = value;
            }
        }

        bool hideLine = false;
        public bool HideLine
        {
            get { return hideLine; }
            set { hideLine = value; }
        }

        public bool RotationLocked
        {
            get { return tracker.RotationLocked; }
            set { tracker.RotationLocked = value; }
        }

        public bool MoveLocked
        {
            get { return tracker.MoveLocked; }
            set { tracker.MoveLocked = value; }
        }

        private bool measureMode;
        public bool MeasureMode
        {
            set
            {
                measureMode = value;
                tracker.Shape = FigureType.Line;
                tracker.MeasureMode = value;
            }
        }

        LineFigure measureFigure = null;

        private float measureScaleX = 1;
        public float MeasureScaleX
        {
            get { return measureScaleX; }
            set { measureScaleX = value; }
        }

        private float measureScaleY = 1;
        public float MeasureScaleY
        {
            get { return measureScaleY; }
            set { measureScaleY = value; }
        }

        private bool appendMode;
        bool shiftPositionMode;
        public bool ShiftPositionMode
        {
            get { return shiftPositionMode; }
            set { shiftPositionMode = value; }
        }

        private bool breakLineMode;

        private bool addFigureMode;
        public bool AddFigureMode
        {
            get { return addFigureMode;  }
            set
            {
                addFigureMode = value;
                tracker.AddFigureMode = value;
            }
        }

        private bool overlayMoveMode;
        public bool OverlayMoveMode
        {
            get { return overlayMoveMode; }
            set
            {
                overlayMoveMode = value;
                tracker.Shape = FigureType.Line;
            }
        }

        private ImageDevice linkedImageDevice = null;
        public ImageDevice LinkedImageDevice
        {
            get { return linkedImageDevice; }
            set { linkedImageDevice = value; }
        }

        private bool lockLiveUpdate = false;
        public bool LockLiveUpdate
        {
            get { return lockLiveUpdate; }
            set { lockLiveUpdate = value; }
        }

        private bool invertY = false;
        public bool InvertY
        {
            get { return invertY; }
            set { invertY = value; }
        }

        private float coordScaleX = 1.0f;
        public float CoordScaleX
        {
            get { return coordScaleX; }
            set { coordScaleX = value; }
        }

        private float coordScaleY = 1.0f;
        public float CoordScaleY
        {
            get { return coordScaleY; }
            set { coordScaleY = value; }
        }
        
        int preScrollWidth = 0;
        int preScrollHeight = 0;

        public DrawBox()
        {
            InitializeComponent();

            this.MouseDoubleClick += drawBox_MouseDoubleClick;

            this.pictureBox = new PictureBox();
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.None;
            this.pictureBox.Name = "pictureBox";
            //this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 8;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseDown += pictureBox_MouseDown;
            this.pictureBox.MouseUp += pictureBox_MouseUp;
            this.pictureBox.MouseMove += pictureBox_MouseMove;
            this.pictureBox.Paint += pictureBox_Paint;
            this.pictureBox.Invalidated += pictureBox_Invalidated;
            this.pictureBox.MouseDoubleClick += pictureBox_MouseDoubleClick;
            //this.pictureBox.MouseWheel += pictureBox_MouseWheel;
            this.pictureBox.MouseEnter += PictureBox_MouseEnter;

            this.Controls.Add(pictureBox);

            this.AutoScroll = true;

            tracker = new Tracker(pictureBox);
            tracker.TrackerMoved = new TrackerMovedDelegate(Tracker_FigureMoved);
            tracker.SelectionPointCaptured = new SelectionPointCapturedDelegate(Tracker_SelectionPointCaptured);
            tracker.SelectionRectCaptured = new SelectionRectCapturedDelegate(Tracker_SelectionRectCaptured);
            tracker.AddFigureCaptured = new AddFigureCapturedDelegate(Tracker_AddFigureCaptured);
            tracker.PositionShifted = new PositionShiftedDelegate(Tracker_PositionShifted);
        }

        public DrawBox(Color backgroundColor)
        {
            InitializeComponent();

            this.MouseDoubleClick += drawBox_MouseDoubleClick;

            this.pictureBox = new PictureBox();
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.None;
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.BackColor = backgroundColor;
            this.pictureBox.TabIndex = 8;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseDown += pictureBox_MouseDown;
            this.pictureBox.MouseUp += pictureBox_MouseUp;
            this.pictureBox.MouseMove += pictureBox_MouseMove;
            this.pictureBox.Paint += pictureBox_Paint;
            this.pictureBox.Invalidated += pictureBox_Invalidated;
            //this.pictureBox.MouseDoubleClick += pictureBox_MouseDoubleClick;
            //this.pictureBox.MouseWheel += pictureBox_MouseWheel;
            this.pictureBox.MouseEnter += PictureBox_MouseEnter;
            

            this.Controls.Add(pictureBox);

            this.AutoScroll = true;

            tracker = new Tracker(pictureBox);
            tracker.TrackerMoved = new TrackerMovedDelegate(Tracker_FigureMoved);
            tracker.SelectionPointCaptured = new SelectionPointCapturedDelegate(Tracker_SelectionPointCaptured);
            tracker.SelectionRectCaptured = new SelectionRectCapturedDelegate(Tracker_SelectionRectCaptured);
            tracker.AddFigureCaptured = new AddFigureCapturedDelegate(Tracker_AddFigureCaptured);
            tracker.PositionShifted = new PositionShiftedDelegate(Tracker_PositionShifted);
        }

        public DrawBox(DrawBoxOption drawBoxOption)
        {
            InitializeComponent();

            this.MouseDoubleClick += drawBox_MouseDoubleClick;

            this.pictureBox = new PictureBox();
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.None;
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            if(drawBoxOption.BackColor != null)
                this.pictureBox.BackColor = drawBoxOption.BackColor;
            this.pictureBox.TabIndex = 8;
            this.pictureBox.TabStop = false;
            this.pictureBox.MouseDown += pictureBox_MouseDown;
            this.pictureBox.MouseUp += pictureBox_MouseUp;
            this.pictureBox.MouseMove += pictureBox_MouseMove;
            this.pictureBox.Paint += pictureBox_Paint;
            if (drawBoxOption.LockDoubleClick == false)
                this.pictureBox.MouseDoubleClick += pictureBox_MouseDoubleClick;
            this.pictureBox.MouseEnter += PictureBox_MouseEnter;


            this.Controls.Add(pictureBox);

            this.AutoScroll = true;

            tracker = new Tracker(pictureBox);
            tracker.TrackerMoved = new TrackerMovedDelegate(Tracker_FigureMoved);
            tracker.SelectionPointCaptured = new SelectionPointCapturedDelegate(Tracker_SelectionPointCaptured);
            tracker.SelectionRectCaptured = new SelectionRectCapturedDelegate(Tracker_SelectionRectCaptured);
            tracker.AddFigureCaptured = new AddFigureCapturedDelegate(Tracker_AddFigureCaptured);
            tracker.PositionShifted = new PositionShiftedDelegate(Tracker_PositionShifted);
        }

        private void drawBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (MouseDoubleClicked != null)
                MouseDoubleClicked(this);
        }

        protected override Point ScrollToControl(Control activeControl)
        {
            //return base.ScrollToControl(activeControl);
            return this.DisplayRectangle.Location;
        }

        private void PictureBox_MouseEnter(object sender, EventArgs e)
        {
            pictureBox.Focus();
        }

        private void PictureBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            
        }

        public void DisableMeasureMode()
        {
            throw new NotImplementedException();
        }

        public void SetImageDevice(ImageDevice imageDevice)
        {
            if (imageDevice != null)
            {
                linkedImageDevice = imageDevice;

                if (linkedImageDevice != null)
                    linkedImageDevice.ImageGrabbed += ImageGrabbed;
            }
            else
            {
                if (linkedImageDevice != null)
                    linkedImageDevice.ImageGrabbed -= ImageGrabbed;
                linkedImageDevice = null;
            }
        }

        public delegate void ImageGrabbedDelegate(ImageDevice imageDevice);
        public void ImageGrabbed(ImageDevice imageDevice, IntPtr ptr)
        {
            if (lockLiveUpdate == true)
                return;

            lockLiveUpdate = true;
            try
            {
                LogHelper.Debug(LoggerType.Grab, String.Format("DrawBox::ImageGrabbed"));

                Image2D image2D = (Image2D)imageDevice.GetGrabbedImage(ptr);
                
                //if (image2D.IsUseIntPtr())
                //{
                //    Image2D image2DD = (Image2D)image2D.Clone();
                //    image2DD.ConvertFromDataPtr();
                //    image2D = image2DD;
                //}

                if (OnImageGrabbed != null)
                {
                    image2D = (Image2D)OnImageGrabbed(this, image2D);
                }

                Image image = image2D.ToBitmap();
                 
                UpdateImage(image);
                image.Dispose();

                //CalculateZoomScale();

                if (OnImageUpdated != null)
                {
                    OnImageUpdated(this, image2D);
                }
                //Thread.Sleep(100); // ????
            }
            finally
            {
                lockLiveUpdate = false;
            }
        }

        public void UpdateImage(Image image)
        {
            //LogHelper.Debug(LoggerType.Debug, "DrwaBox::UpdateImage Start");
            lock (updateLockObject)
            {
                if (imageBuffer == image)
                    return;

                if (imageBuffer != null)
                    imageBuffer.Dispose();
                imageBuffer = null;

                if (image != null)
                    imageBuffer = (Image)image.Clone();
            }

            if (InvokeRequired)
            {
                Invoke(new UpdateImageDelegate(UpdateImage2));
            }
            else
            {
                UpdateImage2();
            }
            //LogHelper.Debug(LoggerType.Debug, "DrwaBox::UpdateImage End");
        }

        public delegate void UpdateImageDelegate();
        private void UpdateImage2()
            //LogHelper.Debug(LoggerType.Debug, "DrwaBox::UpdateImage2 Start");
        {
            //if (pictureBox.Image == imageBuffer)
            //return;
            bool calcZoom = (zoomScale.Width < 0 || zoomScale.Height < 0);
            lock (updateLockObject)
            {
                if (pictureBox.Image != null)
                {
                    if (pictureBox.Image.PixelFormat != PixelFormat.DontCare)
                    {
                        calcZoom = (imageBuffer == null) ? false : (pictureBox.Image.Size != imageBuffer.Size);
                        pictureBox.Image.Dispose();
                    }
                }
            }

            pictureBox.Image = null;

            lock (updateLockObject)
            {
                if (imageBuffer != null)
                {
                    pictureBox.Image = (Image)imageBuffer.Clone();
                }
                else
                {
                    pictureBox.Image = null;
                }
            }

            if (calcZoom)
            {
                displayRect = Rectangle.Empty;
                CalculateZoomScale();
            }
            else
                UpdateZoom();


        }

        public void ClearImage()
        {
            pictureBox.Image = null;
        }

        public delegate void OnImageUpdatedDelegate(DrawBox srcDrawBox, ImageD image);
        public OnImageUpdatedDelegate OnImageUpdated = null;

        public delegate ImageD OnImageGrabbedDelegate(DrawBox srcDrawBox, ImageD image);
        public OnImageGrabbedDelegate OnImageGrabbed = null;

        private void pictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (enable == false)
                return;

            tempFigureGroup.Clear();

            if (MouseDoubleClicked != null)
                MouseDoubleClicked(this);

            Invalidate(true);
        }

        public void AutoFit(bool onOff)
        {
            if (onOff == true)
            {
                this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
//                ZoomFit();
            }
            else
            {
                this.pictureBox.Dock = System.Windows.Forms.DockStyle.None;
            }
        }

        public void Offset(float offsetX, float offsetY)
        {
            tracker.Offset((int)offsetX, (int)offsetY);
            Invalidate(true);
        }

        private void Tracker_PositionShifted(System.Drawing.SizeF offset)
        {
            if (shiftPositionMode == true)
            {
                CoordTransformer coordTransformer = GetCoordTransformer();
                if (coordTransformer == null)
                    return;

                SizeF offsetF = coordTransformer.InverseTransform(offset);

                PositionShifted?.Invoke(offsetF);
            }
            else if (overlayMoveMode == true)
            {
                overlayPos -= Size.Round(offset);
                Invalidate();
            }
            else
            {
                viewOffset += Size.Round(offset);
                UpdateZoom();
            }
        }

        public void SetTrackerShape(FigureType shape, int numGridColumn = 0, int numGridRow = 0)
        {
            tracker.Shape = shape;
            tracker.NumGridColumn = numGridColumn;
            tracker.NumGridRow = numGridRow;
        }

        public void ClearFigure()
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.ClearFigure");
            this.Invalidate();
            this.pictureBox.Controls.Clear();
            tracker.ClearFigure();
        }

        public void ClearFigure2()
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.ClearFigure");
            Invalidate();
        }

        public void SelectFigure(Figure figure)
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.SelectFigure");
            tracker.AddFigure(figure);
        }

        public void SelectFigureByTag(Object tag)
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.SelectFigureByTag");
            tracker.AddFigure(figureGroup.GetFigureByTag(tag));
        }

        public void RemoveSelectedFigure()
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.MouseMove");

            foreach(Figure selectedFigure in tracker)
                figureGroup.RemoveFigure(selectedFigure);

            tracker.ClearFigure();
            Invalidate(true);
        }

        void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (enable == false)
                return;

            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            CoordTransformer coordTransformer = GetCoordTransformer();
            if (coordTransformer == null)
                return;

            LogHelper.Debug(LoggerType.Operation, "TeachBox.OnMouseDown");

            breakLineMode = (Control.ModifierKeys == Keys.Alt);
            if (breakLineMode == false)
                appendMode = (Control.ModifierKeys == Keys.Control);

            tracker.CoordTransformer = coordTransformer;
            tracker.ShiftPositionMode = (Control.ModifierKeys == Keys.Shift) || shiftPositionMode || overlayMoveMode;

            tracker.MouseDown(e.Location);

            base.OnMouseDown(e);
        }

        public void ResetSelection()
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.ResetSelection");

            tracker.ClearFigure();

            if (FigureSelected != null)
                FigureSelected(null);
        }

        void Tracker_SelectionPointCaptured(Point point)
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.Tracker_SelectionPointCaptured");

            if (appendMode == false)
                ResetSelection();

            Figure figure = figureGroup.Select(point);
            if (figure != null)
            {
                bool isSelected = tracker.IsSelected(figure);
                if (isSelected)
                {
                    tracker.RemoveFigure(figure);
                    //Figure nextFigure = figureGroup.Select(point, figure);
                    //if (nextFigure != null)
                    //    figure = nextFigure;
                }
                else if (FigureSelectable == null || FigureSelectable(figure))
                {
                    tracker.AddFigure(figure);

                   
                }
                if (FigureSelected != null)
                    FigureSelected(figure, !isSelected);
            }

            Invalidate(true);
        }

        void Tracker_SelectionRectCaptured(Rectangle rectangle, Point startPos, Point endPos)
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.Tracker_SelectionRectCaptured");

            if (appendMode == false)
                ResetSelection();

            List<Figure> figureList = figureGroup.Select(rectangle);

            if (FigureMultiSelected != null)
            {
                FigureMultiSelected(figureList);
                foreach (Figure figure in figureList)
                {
                    tracker.AddFigure(figure);
                }
            }

            //List<Figure> targetFigureList = GetFigureList(figureList, true);
            //List<Figure> probeFigureList = GetFigureList(figureList, false);
            //if (figureList.Count() > 0)
            //{
            //    if(targetFigureList.Count > 1)
            //        tracker.AddFigure(targetFigureList);
            //    else
            //        tracker.AddFigure(probeFigureList);
            //    if (FigureSelected != null)
            //    {
            //        foreach (Figure figure in figureList)
            //        {
            //             FigureSelected(figure);                           
            //        }

            //    }
            //}


            Invalidate(true);
        }

        void Tracker_AddFigureCaptured(List<PointF> pointList)
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.Tracker_AddFigureCaptured");

            if (addFigureMode == true)
            {
                ResetSelection();
                if (FigureAdd!= null)
                    FigureAdd(pointList, TrackerShape);
            }

            Invalidate(true);
        }

        List<Figure> GetFigureList(List<Figure> figureList, bool TargetFlag)
        {
            List<Figure> returnFigureList = new List<Figure>();
            foreach (Figure figure in figureList)
            {
                if (TargetFlag)
                {
                    if(figure.Tag.GetType().Name == "Target")
                    {
                        returnFigureList.Add(figure);
                    }
                }
                else
                {
                    if (figure.Tag.GetType().Name == "VisionProbe")
                    {
                        returnFigureList.Add(figure);
                    }
                }
            }

            return returnFigureList;

        }

        void Tracker_FigureMoved()
        {
            LogHelper.Debug(LoggerType.Operation, "DrawBox.Tracker_FigureMoved");

            if (appendMode == true)
            {
                if (FigureCopy != null)
                    FigureCopy(tracker.GetFigureList());
            }
            else
            {
                if (FigureMoved != null)
                    FigureMoved(tracker.GetFigureList());
            }

            pictureBox.Invalidate();
            Invalidate(true);
        }

        void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            CoordTransformer coordTransformer = GetCoordTransformer();
            if (coordTransformer == null)
                return;

            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            bool processingCancelled = false;
            if (MouseClicked != null)
                MouseClicked(this, coordTransformer.InverseTransform(e.Location), ref processingCancelled);

            if (processingCancelled)
                return;

            LogHelper.Debug(LoggerType.Operation, "DrawBox.OnMouseUp");

            tracker.MouseUp(e.Location);

            SizeF measureSize = new SizeF(Math.Abs(tracker.EndTrackPos.X - tracker.StartTrackPos.X), Math.Abs(tracker.EndTrackPos.Y - tracker.StartTrackPos.Y));

            if (measureMode)
            {
                if (measureSize != new SizeF(0, 0))
                {
                    Pen pen = new Pen(Color.Red, 1);
                    pen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                    measureFigure = new LineFigure(coordTransformer.InverseTransform(tracker.StartTrackPos), coordTransformer.InverseTransform(tracker.EndTrackPos), pen);
                    tempFigureGroup.AddFigure(measureFigure);

                    measureSize = coordTransformer.InverseTransform(measureSize);

                    measureSize = new SizeF(measureSize.Width * measureScaleX, measureSize.Height * measureScaleY);
                    float length = MathHelper.GetLength(measureFigure.StartPoint, measureFigure.EndPoint) * measureScaleX;

                    SizeF fontSize = coordTransformer.InverseTransform(new SizeF(10, 0));

                    String text = String.Format("{0} (W{1}, H{2})", length, measureSize.Width, measureSize.Height);
                    PointF centerPos = coordTransformer.InverseTransform(DrawingHelper.CenterPoint(tracker.StartTrackPos, tracker.EndTrackPos));
                    TextFigure textFigure = new TextFigure(text, Point.Round(centerPos), new Font("Arial", fontSize.Width), Color.Red);

                    tempFigureGroup.AddFigure(textFigure);
                }
            }

            base.OnMouseUp(e);
        }

        void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            
            if (Image == null)
                return;

            CoordTransformer coordTransformer = GetCoordTransformer();
            if (coordTransformer == null)
                return;
            
            bool processingCancelled = false;
            if (MouseMoved != null)
                MouseMoved(this, coordTransformer.InverseTransform(e.Location), pictureBox.Image, e,ref processingCancelled);

            if (e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                if (showTooltip == true)
                {
                    Point searchPt = coordTransformer.InverseTransform(new Point(e.X, e.Y));

                    Figure figure = tempFigureGroup.Select(searchPt);
                    if (figure != null && figure.Tag != null)
                    {
                        toolTip.Show(figure.Tag.ToString(), pictureBox, e.X, e.Y, 1000);
                    }
                }
            }
            else
            {
                LogHelper.Debug(LoggerType.Operation, "DrawBox.OnMouseMove");

                tracker.MouseMove(e.Location);
            }

//            base.OnMouseMove(e);
        }

        //protected override void OnMouseEnter(EventArgs e)
        //{
        //    Focus();
        //}

        //protected override void OnMouseWheel(MouseEventArgs e)
        //{
        //    base.OnMouseWheel(e);

        //    StepZoom((((float)e.Delta) / displayRect.Width));
        //}

        //protected override void OnMouseClick(MouseEventArgs e)
        //{
        //    base.OnMouseClick(e);
        //    if(e.Button == MouseButtons.Middle)
        //    {

        //    }
        //}

        //protected override void OnMouseDoubleClick(MouseEventArgs e)
        //{
        //    tempFigureGroup.Clear();

        //    if (AddRegionCaptured != null)
        //        AddRegionCaptured(new Rectangle(), new Point(), new Point());

        //    base.OnMouseDoubleClick(e);

        //    Invalidate(true);
        //}

        public void ScrollCenterTo(PointF pointF)
        {
            CoordTransformer coordTransformer = GetCoordTransformer();
            PointF transf = coordTransformer.Transform(pointF);
            if (this.HorizontalScroll.Visible)
            {
                int scroll = (int)(transf.X - this.Width / 2);
                this.HorizontalScroll.Value = Math.Min(this.HorizontalScroll.Maximum, Math.Max(this.HorizontalScroll.Minimum, scroll));
            }

            if(this.VerticalScroll.Visible)
            {
                this.VerticalScroll.Value = (int)(transf.Y-this.Height/2);
            }

            //this.Width;
            //this.Height;
            //this.HorizontalScroll.Minimum;
            //this.HorizontalScroll.Maximum;
            //this.VerticalScroll.Minimum;
            //this.VerticalScroll.Maximum;
            //this.Image.Size
        }

        public void SetZoomOffset(SizeF zoomOffset)
        {
            this.zoomOffset = zoomOffset;
            UpdateZoom();
        }

        public void StepZoom(float value)
        {
            if (zoomScale.Width < 0 || zoomScale.Height < 0)
                CalculateZoomScale();

            this.zoomOffset.Width += value;
            this.zoomOffset.Height += value;

            UpdateZoom();
        }

        public void ZoomIn()
        {
            StepZoom(0.01f);
        }

        public void ZoomOut()
        {
            StepZoom(-0.01f);
        }

        public void ZoomFit()
        {
            this.displayRect = RectangleF.Empty;
            CalculateZoomScale();
        }

        public void Zoom(SizeF newZoomScale)
        {
            UpdateZoom(this.zoomScale, newZoomScale);
        }

        private void UpdateZoom()
        {
            if (Image != null)
            {
                SizeF adjustZoomScale = SizeF.Add(zoomScale, this.zoomOffset);
                pictureBox.Width = (int)(Image.Width * adjustZoomScale.Width);
                pictureBox.Height = (int)(Image.Height * adjustZoomScale.Height);
            }            
        }

        public delegate void UpdateZoomDelegate(SizeF preZoomScale, SizeF newZoomScale);
        private void UpdateZoom(SizeF preZoomScale, SizeF newZoomScale)
        {
            if (InvokeRequired)
            {
                Invoke(new UpdateZoomDelegate(UpdateZoom), preZoomScale, newZoomScale);
                return;
            }

            preScrollWidth = HorizontalScroll.Maximum;
            preScrollHeight = VerticalScroll.Maximum;

            SizeF adjustZoomScale = SizeF.Add(newZoomScale, this.zoomOffset);
            //adjustZoomScale = newZoomScale;
            if (displayRect.IsEmpty == false)
            {
                pictureBox.Width = (int)(displayRect.Width * adjustZoomScale.Width);
                pictureBox.Height = (int)(displayRect.Height * adjustZoomScale.Height);
            }
            else if (Image != null)
            {
                pictureBox.Width = (int)(Image.Width * adjustZoomScale.Width);
                pictureBox.Height = (int)(Image.Height * adjustZoomScale.Height);
            }

            zoomScale = newZoomScale;
        }
        
        private void CalculateZoomScale()
        {
            int wScrollBarSize = 0;
            int hScrollBarSize = 0;

            if (autoFitStyle == AutoFitStyle.FitWidthOnly)
                hScrollBarSize = 20;

            if (autoFitStyle == AutoFitStyle.FitHeigthOnly)
                wScrollBarSize = 20;

            zoomDisplayRect = new Rectangle(0, 0, Width - wScrollBarSize, Height - hScrollBarSize);
            SizeF newZoomScale = new SizeF(1, 1);

            if (displayRect.IsEmpty == false)
            {
                float zoomScaleWidth = zoomDisplayRect.Width / ((float)displayRect.Width);
                float zoomScaleHeight = zoomDisplayRect.Height / ((float)displayRect.Height);

                newZoomScale = new SizeF(zoomScaleWidth, zoomScaleHeight);
            }
            else if (Image != null)
            {
                //lock (image)
                {
                    displayRect = new RectangleF(0, 0, Image.Width, Image.Height);
                    float zoomScaleWidth = zoomDisplayRect.Width / ((float)Image.Width);
                    float zoomScaleHeight = zoomDisplayRect.Height / ((float)Image.Height);

                    newZoomScale = new SizeF(zoomScaleWidth, zoomScaleHeight);
                }
            }
            else
                return;

            switch (autoFitStyle)
            {
                case AutoFitStyle.KeepRatio:
                    this.HorizontalScroll.Visible = this.VerticalScroll.Visible = false;
                    
                    break;
                case AutoFitStyle.FitWidthOnly:
                    newZoomScale.Height = newZoomScale.Width;
                    break;
                case AutoFitStyle.FitHeigthOnly:
                    newZoomScale.Width = newZoomScale.Height;
                    break;
                default:
                    break;
            }

            UpdateZoom(zoomScale, newZoomScale);
        }

        public CoordTransformer GetCoordTransformer()
        {
            if (displayRect.IsEmpty && Image == null)
                return null;

            CoordTransformer coordTransformer = new CoordTransformer();

            if (zoomScale.Width > 0 && zoomScale.Height > 0)
            {
                coordTransformer.SetScale(zoomScale.Width, zoomScale.Height);
            }
            else
            {
                if (displayRect.IsEmpty == false)
                {
                    coordTransformer.SetSrcRect(displayRect);
                    coordTransformer.SetDisplayRect(new RectangleF(0, 0, pictureBox.Width, pictureBox.Height));
                }
                else if (Image != null)
                {
                    lock (Image)
                        coordTransformer.SetSrcRect(new RectangleF(0, 0, Image.Width, Image.Height));
                    coordTransformer.SetDisplayRect(new RectangleF(0, 0, pictureBox.Width, pictureBox.Height));
                }

                coordTransformer.UpdateScale();
            }
            coordTransformer.InvertY = invertY;

            coordTransformer.ModifyScale(coordScaleX, coordScaleY);
            //coordTransformer.ModifyScale(1/this.ZoomScale.Width, 1/this.ZoomScale.Height);

            return coordTransformer;
        }

        void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            //pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            // Call base class, invoke Paint handlers
            lock (updateLockObject)
            {
                base.OnPaint(e);

                CoordTransformer coordTransformer = GetCoordTransformer();
                if (coordTransformer == null)
                    return;

                if (showCenterGuide)
                {
                    Pen pen = new Pen(Color.Blue, centerGuideThickness);
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

                    e.Graphics.DrawLine(pen, new PointF(0, pictureBox.Height / 2 + centerGuidePos.X), new PointF(pictureBox.Width, pictureBox.Height / 2 + centerGuidePos.X));
                    e.Graphics.DrawLine(pen, new PointF(pictureBox.Width / 2 + centerGuidePos.Y, 0), new PointF(pictureBox.Width / 2 + centerGuidePos.Y, pictureBox.Height));
                }

                if (figureGroup != null)
                {
                    if (hideLine == false)
                    {
                        figureGroup.Draw(e.Graphics, coordTransformer, false);

                        if (tempFigureGroup != null)
                            tempFigureGroup.Draw(e.Graphics, coordTransformer, false);

                        if (backgroundFigures != null)
                            backgroundFigures.Draw(e.Graphics, coordTransformer, false);

                        if (customFigure != null)
                            customFigure.Draw(e.Graphics, coordTransformer, false);
                    }
                }

                if (overlayImage != null)
                {
                    ColorMatrix colorMatrix = new ColorMatrix();
                    ImageAttributes ia = new ImageAttributes();

                    Rectangle imgRect = new Rectangle(overlayPos.X, overlayPos.Y, pictureBox.Width, pictureBox.Height);
                    colorMatrix.Matrix33 = 0.50f;
                    ia.SetColorMatrix(colorMatrix);

                    //                e.Graphics.DrawImage(overlayImage, imgRect, 0,0, );
                    e.Graphics.DrawImage(overlayImage, imgRect, 0, 0, overlayImage.Width, overlayImage.Height, GraphicsUnit.Pixel, ia);
                }

                if (tracker != null)
                {
                    tracker.Draw(e.Graphics, coordTransformer);
                }

                if (preScrollWidth != 0)
                {
                    if (pictureBox.Width > Width)
                    {
                        int posValue = (int)((float)HorizontalScroll.Value / preScrollWidth * HorizontalScroll.Maximum);
                        if (posValue >= 0)
                            HorizontalScroll.Value = posValue;
                    }

                    if (pictureBox.Height > Height)
                    {
                        int posValue = (int)((float)VerticalScroll.Value / preScrollHeight * VerticalScroll.Maximum);

                        if (posValue >= 0)
                            VerticalScroll.Value = posValue;
                    }

                    preScrollWidth = 0;
                    preScrollHeight = 0;
                }
            }
        }

        private void pictureBox_Invalidated(object sender, InvalidateEventArgs e)
        {
            
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DrawBox
            // 
            this.Name = "DrawBox";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DrawBox_KeyUp);
            this.Validated += new System.EventHandler(this.DrawBox_Validated);
            this.ResumeLayout(false);

        }

        private void DrawBox_Validated(object sender, EventArgs e)
        {
            pictureBox.Invalidate();
        }

        public void SaveImage(string fileName)
        {
            Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            pictureBox.DrawToBitmap(bmp, pictureBox.Bounds);

            ImageHelper.SaveImage(bmp, fileName);
        }

        private void DrawBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (Image == null)
                return;

            if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right)
                return;

            LogHelper.Debug(LoggerType.Operation, "DrawBox.OnKeyUp");

            Size offset = new Size(0, 0);
            if (e.KeyCode == Keys.Down)
                offset.Height = 1;
            else if (e.KeyCode == Keys.Up)
                offset.Height = -1;
            else if (e.KeyCode == Keys.Left)
                offset.Width = -1;
            else if (e.KeyCode == Keys.Right)
                offset.Width = 1;

            tracker.Move(offset);
        }

        public delegate void InvalidateDelegate();
        public new void Invalidate()
        {
            if(InvokeRequired)
            {
                BeginInvoke(new InvalidateDelegate(Invalidate));
                return;
            }
            Invalidate(true);
        }

        public void DrawCrossLine(PointF centerPt)
        {
            FigureGroup tempFigureGroup = new FigureGroup();

            tempFigureGroup.AddFigure(new LineFigure(new PointF(0, centerPt.Y),
                        new PointF(Image.Width, centerPt.Y), new Pen(Color.Red, 10)));
            tempFigureGroup.AddFigure(new LineFigure(new PointF(centerPt.X, 0),
                        new PointF(centerPt.X, Image.Height), new Pen(Color.Red, 10)));

            this.tempFigureGroup = tempFigureGroup;
            Invalidate(true);
            Update();
        }

        public void DrawEllipse(Rectangle rect, Pen pen = null)
        {
            FigureGroup tempFigureGroup = new FigureGroup();
            if (pen == null)
                pen = new Pen(Color.Red);

            tempFigureGroup.AddFigure(new EllipseFigure(rect, pen));
            this.tempFigureGroup = tempFigureGroup;
            Invalidate(true);
            Update();
        }
    }
}
 