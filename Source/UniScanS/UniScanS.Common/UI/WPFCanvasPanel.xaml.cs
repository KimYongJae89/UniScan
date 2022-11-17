using DynMvp.Base;
using DynMvp.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MColor = System.Windows.Media.Color;
using MBrush = System.Windows.Media.SolidColorBrush;
using MPen = System.Windows.Media.Pen;
using DynMvp.Data.UI;

namespace UniScanS.Common.UI
{
    /// <summary>
    /// WPFCanvasPanel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WPFCanvasPanel : UserControl
    {
        public delegate DynMvp.UI.Figure CreateCustomFigureDelegate(PointF pt1, PointF pt2);
        public delegate void FigureCreatedDelegate(DynMvp.UI.Figure figure, CoordMapper coordMapper, FigureGroup workingFigures, FigureGroup backgroundFigures);
        public delegate void FigureSelectedDelegate(List<DynMvp.UI.Figure> figureList);
        public delegate void FigureDeletedDelegate(List<DynMvp.UI.Figure> figureList);
        public delegate void FigureCopiedDelegate(List<DynMvp.UI.Figure> figureList);
        public delegate void FigurePastedDelegate(List<DynMvp.UI.Figure> figureList, FigureGroup workingFigures, FigureGroup backgroundFigures, SizeF pasteOffset);
        public delegate void FigureModifiedDelegate(List<DynMvp.UI.Figure> figureList);
        public delegate void FigureFocusedDelegate(DynMvp.UI.Figure figure);
        public delegate void MouseClickedDelegate(PointF point, ref bool processingCancelled);
        public delegate void MouseDblClickedDelegate();
        public delegate void MouseLeavedDelegate();

        /// <summary>
        /// 내부에서 개체를 생성하고, 생성된 객체를 참조하는 Figure를 반환한다.
        /// </summary>
        public CreateCustomFigureDelegate CreateCustomFigure;
        /// <summary>
        /// Drag완료 후 새로운 Figure를 생성한 후 호출한다. FigureCreated는 Figure에 연관된 객체를 생성하여 Tag에 저장한다.
        /// 추가적인 Figure의 생성이 필요할 경우 새로운 Figure를 생성하여 additionalFIgureList에 추가한다.
        /// </summary>
        public FigureCreatedDelegate FigureCreated;
        public FigureSelectedDelegate FigureSelected;
        public FigureCopiedDelegate FigureCopied;
        public FigureDeletedDelegate FigureDeleted;
        public FigureModifiedDelegate FigureModified;
        public FigureFocusedDelegate FigureFocused;
        /// <summary>
        /// 새로운 Figure의 목록이 생성된 후, 이 Delegation이 호출된다.
        /// 필요에 따라 Figure.Tag에 등록된 정보를 이용하여 Data 객체를 생성해야 한다.
        /// </summary>
        public FigurePastedDelegate FigurePasted;
        /// <summary>
        /// Mouse Clicked
        /// </summary>
        public MouseClickedDelegate MouseClicked;
        public MouseDblClickedDelegate MouseDblClicked;
        public MouseLeavedDelegate MouseLeaved;

        DragMode curDragMode = DragMode.Select;
        DragMode dragMode = DragMode.Select;
        public DragMode DragMode
        {
            get { return dragMode; }
            set { dragMode = value; }
        }

        FigureGroup workingFigures = new FigureGroup();
        public FigureGroup WorkingFigures
        {
            get { return workingFigures; }
            set { workingFigures = value; }
        }

        FigureGroup backgroundFigures = new FigureGroup();
        public FigureGroup BackgroundFigures
        {
            get { return backgroundFigures; }
            set { backgroundFigures = value; }
        }

        FigureGroup tempFigures = new FigureGroup();
        public FigureGroup TempFigures
        {
            get { return tempFigures; }
            set { tempFigures = value; }
        }

        /// <summary>
        /// 마우스가 지나가고 있는 위치에 있는 Figure
        /// </summary>
        DynMvp.UI.Figure focusedFigure = null;
        DynMvp.UI.Figure lastFocusedFigure = null;

        bool showRuler;
        public bool ShowRuler
        {
            get { return showRuler; }
            set { showRuler = value; }
        }

        bool showCenterGuide = true;
        public bool ShowCenterGuide
        {
            get { return showCenterGuide; }
            set { showCenterGuide = value; }
        }

        System.Drawing.Point centerGuidePos;
        public System.Drawing.Point CenterGuidePos
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

        bool invertY = false;
        public bool InvertY
        {
            get { return invertY; }
            set { invertY = value; }
        }

        private bool enable = false;
        public bool Enable
        {
            get { return enable; }
            set { enable = value; }
        }

        private bool readOnly = false;
        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        private bool editable = false;
        public bool Editable
        {
            get { return editable; }
            set { editable = value; }
        }

        private bool rotationLocked = false;
        public bool RotationLocked
        {
            get { return rotationLocked; }
            set { rotationLocked = value; }
        }

        private bool singleAxisTracking = false;
        public bool SingleAxisTracking
        {
            get { return singleAxisTracking; }
            set { singleAxisTracking = value; }
        }

        private bool noneClickMode = false;
        public bool NoneClickMode
        {
            get { return noneClickMode; }
            set { noneClickMode = value; }
        }

        private List<PointF> trackPointList = new List<PointF>();
        public List<PointF> TrackPointList
        {
            get { return trackPointList; }
        }

        private FigureType trackerShape = FigureType.Rectangle;
        public FigureType TrackerShape
        {
            get { return trackerShape; }
            set { trackerShape = value; }
        }

        bool onDrag = false;
        PointF dragStart;
        PointF dragEnd;
        SizeF dragOffset;

        SelectionContainer selectionContainer = new SelectionContainer();
        private bool onUpdateStateButton;
        private TrackPos curTrackPos;
        private int copyCount;

        BitmapImage sourceImage;

        private ScaleTransform scale = new ScaleTransform();
        private TranslateTransform translate = new TranslateTransform();

        public WPFCanvasPanel(BitmapScalingMode mode = BitmapScalingMode.NearestNeighbor)
        {
            InitializeComponent();

            TransformGroup transformGroup = new TransformGroup();
            transformGroup.Children.Add(translate);
            transformGroup.Children.Add(scale);
            imageCanvas.RenderTransform = transformGroup;

            RenderOptions.SetBitmapScalingMode(imageCanvas, mode);
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
            //HwndTarget hwndTarget = hwndSource.CompositionTarget;
            //hwndTarget.RenderMode = RenderMode.;
        }

        public void ClearFigure()
        {
            workingFigures.Clear();
            backgroundFigures.Clear();
            tempFigures.Clear();
            selectionContainer.ClearSelection();

            InvalidateVisual();
        }

        public void SetAddMode(FigureType trackerShape)
        {
            Cursor = Cursors.Cross;
            dragMode = DragMode.Add;
            this.trackerShape = trackerShape;
        }

        public void AddFigure(DynMvp.UI.Figure figure)
        {
            selectionContainer.AddSelection(figure);
        }

        public void SelectFigure(DynMvp.UI.Figure figure)
        {
            if (noneClickMode)
                return;
            selectionContainer.ClearSelection();
            selectionContainer.AddSelection(figure);
        }

        public void SelectFigure(List<DynMvp.UI.Figure> figureList)
        {
            if (noneClickMode)
                return;

            selectionContainer.ClearSelection();
            selectionContainer.AddSelection(figureList);
        }

        public void SelectFigureByTag(List<Object> tagList)
        {
            if (noneClickMode)
                return;

            foreach (Object tag in tagList)
                selectionContainer.AddSelection(workingFigures.GetFigureByTag(tag));
        }

        public void SelectFigureByTag(Object tag)
        {
            if (noneClickMode)
                return;
            selectionContainer.AddSelection(workingFigures.GetFigureByTag(tag));
        }

        public void DeleteSelection()
        {
            List<DynMvp.UI.Figure> figureList = selectionContainer.GetRealFigures();
            //foreach (Figure selectedFigure in figureList)
            //{
            //    workingFigures.RemoveFigure(selectedFigure);
            //}

            if (FigureDeleted != null)
                FigureDeleted(figureList);

            selectionContainer.ClearSelection();
            InvalidateVisual();
        }

        public void ClearSelection()
        {
            selectionContainer.ClearSelection();
            InvalidateVisual();
        }

        private PixelFormat ConvertPixelFormat(System.Drawing.Imaging.PixelFormat pixelFormat)
        {
            PixelFormat wpfPixelFormat = PixelFormats.Gray8;

            switch (pixelFormat)
            {
                case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
                    wpfPixelFormat = PixelFormats.Gray8;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    wpfPixelFormat = PixelFormats.Rgb24;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                    wpfPixelFormat = PixelFormats.Bgr32;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                case System.Drawing.Imaging.PixelFormat.Format32bppPArgb:
                    wpfPixelFormat = PixelFormats.Bgra32;
                    break;
            }

            return wpfPixelFormat;
        }

        private BitmapImage ConvertImage(System.Drawing.Bitmap bitmap)
        {
            MemoryStream stream = new MemoryStream();
            
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);

            stream.Position = 0;
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }

        public void UpdateImage(string fileName)
        {
            if (File.Exists(fileName) == false)
                return;

            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var bitmapSource = new System.Windows.Media.Imaging.BitmapImage();
            bitmapSource.BeginInit();
            bitmapSource.StreamSource = fileStream;
            bitmapSource.EndInit();

            bool zoomFitRequired = (this.image.Source == null) || (this.image.Source.Width != bitmapSource.Width && this.image.Source.Height != bitmapSource.Height);

            sourceImage = bitmapSource;

            this.image.Source = sourceImage;
            
            if (zoomFitRequired)
                ZoomFit();
            else
                InvalidateVisual();
        }

        public void UpdateImage(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                this.image.Source = null;
                InvalidateVisual();

                return;
            }

            bool zoomFitRequired = (this.image.Source == null) || (this.image.Source.Width != bitmap.Width && this.image.Source.Height != bitmap.Height);

            sourceImage = ConvertImage(bitmap);

            lock (this)
                this.image.Source = sourceImage;

            if (zoomFitRequired)
                ZoomFit();
            else
                InvalidateVisual();
        }

        public void ZoomFit()
        {
            double scaleX = this.ActualWidth / image.Source.Width;
            double scaleY = this.ActualHeight / image.Source.Height;

            double minScale = Math.Min(scaleX, scaleY);

            scale.ScaleX = minScale;
            scale.ScaleY = minScale;
            scale.CenterX = 0;
            scale.CenterY = 0;

            if (scaleX > scaleY)
            {
                translate.X = ((this.ActualWidth / scale.ScaleX) - image.Source.Width) / 2.0;
                translate.Y = 0;
            }
            else
            {
                translate.X = 0;
                translate.Y = ((this.ActualHeight / scale.ScaleX) - image.Source.Height) / 2.0;
            }
        }
        
        private void Zoom(System.Windows.Point originPos, System.Windows.Point zoomPos, double scaleValue)
        {
            translate.X = - zoomPos.X + originPos.X;
            translate.Y = - zoomPos.Y + originPos.Y;

            scale.ScaleX = scaleValue;
            scale.ScaleY = scaleValue;
        }

        public void ZoomIn()
        {
            double scaleValue = scale.ScaleX * 1.2;
            
            System.Windows.Point actualCenter = new System.Windows.Point(this.ActualWidth / 2.0, this.ActualHeight / 2.0);
            actualCenter.X /= scaleValue;
            actualCenter.Y /= scaleValue;

            Zoom(new System.Windows.Point(translate.X, translate.Y), actualCenter, scaleValue);
        }
         
        public void ZoomOut()
        {
            double scaleValue = scale.ScaleX * 0.8;
            System.Windows.Point actualCenter = new System.Windows.Point(this.ActualWidth / 2.0, this.ActualHeight / 2.0);
            actualCenter.X /= scaleValue;
            actualCenter.Y /= scaleValue;

            Zoom(new System.Windows.Point(translate.X, translate.Y), actualCenter, scaleValue);
        }

        public void ZoomRange(System.Drawing.Rectangle rectangle)
        {
            double maxValue = Math.Max(rectangle.Width, rectangle.Height);

            double scaleValue = Math.Max(this.ActualWidth / maxValue, this.ActualHeight / maxValue);

            PointF centerPt = DrawingHelper.CenterPoint(rectangle);
            Zoom(new System.Windows.Point(0, 0), new System.Windows.Point(centerPt.X, centerPt.Y), scaleValue);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (sourceImage == null)
                return;
            
            ImageDrawing imageDrawing = new ImageDrawing(sourceImage, new Rect(0, 0, sourceImage.PixelWidth, sourceImage.PixelHeight));
            
            DrawingGroup drawingGroup = new DrawingGroup();
            drawingGroup.Children.Add(imageDrawing);
            
            foreach (DynMvp.UI.Figure figure in workingFigures.FigureList)
            {
                MColor color = MColor.FromArgb(figure.FigureProperty.Pen.Color.A, figure.FigureProperty.Pen.Color.R, figure.FigureProperty.Pen.Color.G, figure.FigureProperty.Pen.Color.B);
                MBrush brush = null;
                MPen pen = new MPen(new MBrush(color), figure.FigureProperty.Pen.Width);

                if (figure.FigureProperty.Brush != null)
                    brush = new MBrush(color);

                Geometry geometry = null;
                //imageCanvas.Children.Clear();
                switch (figure.Type)
                {
                    case FigureType.Rectangle:
                        RotatedRect rectangle = figure.GetRectangle();
                        geometry = new RectangleGeometry(new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height));
                        break;
                    case FigureType.None:
                        break;
                    case FigureType.Group:
                        break;
                    case FigureType.Grid:
                        break;
                    case FigureType.Line:
                        break;
                    case FigureType.Ellipse:
                        break;
                    case FigureType.Oblong:
                        break;
                    case FigureType.Polygon:
                        break;
                    case FigureType.Text:
                        break;
                    case FigureType.Image:
                        break;
                    case FigureType.Cross:
                        break;
                    case FigureType.XRect:
                        break;
                    case FigureType.Custom:
                        break;
                }

                GeometryDrawing geometryDrawing = new GeometryDrawing(brush, pen, geometry);
                drawingGroup.Children.Add(geometryDrawing);
            }


            DrawingImage drawingImage = new DrawingImage();
            drawingImage.Drawing = drawingGroup;

            image.Source = drawingImage;
            
            //if (hideFigure == false)
            //{
            //    if (workingFigures != null)
            //        workingFigures.Draw(e.Graphics, null, false);
            //    if (tempFigures != null)
            //        tempFigures.Draw(e.Graphics, null, false);
            //    if (backgroundFigures != null)
            //        backgroundFigures.Draw(e.Graphics, null, false);
            //}

            //if (focusedFigure != null)
            //{
            //    using (System.Windows.Media.Pen p = new System.Windows.Media.Pen(Color.OrangeRed, 0))
            //    {
            //        p.DashStyle = DashStyle.Dot;

            //        GraphicsPath focusedFigurePath = focusedFigure.GetGraphicsPath();
            //        e.Graphics.DrawPath(p, focusedFigurePath);
            //    }
            //}

            //selectionContainer.Draw(e.Graphics, GetCoordMapper(), rotationLocked);
        }

        private void UpdateCursor(DragMode dragMode)
        {
            switch (dragMode)
            {
                case DragMode.Add:
                    Cursor = Cursors.Cross;
                    break;
                case DragMode.Pan:
                    Cursor = Cursors.Hand;
                    break;
                case DragMode.Measure:
                case DragMode.Zoom:
                    //Cursor = new Cursor(new System.IO.MemoryStream(DynMvp.Properties.Resources.zoom_in));
                    //Cursor = new Cursor(GetType(), "zoom_in.cur");
                    //Cursor = new Cursor(@"D:\Project\PrintEye\Source\DynMvp\Resources\zoom-in.cur");
                    break;
                default:
                    Cursor = Cursors.Arrow;
                    break;
            }
        }
        
        private void ImageThumb_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            System.Windows.Point controlPt = e.GetPosition(this);

            double scaleValue = 1.0;

            if (e.Delta > 0)
                scaleValue = scale.ScaleX * 1.1;
            else
                scaleValue = scale.ScaleX * 0.9;
            //translate.Y -= (zoomCenter.Y / scale.ScaleY) - (zoomCenter.Y / (scaleValue));

            controlPt.X /= scaleValue;
            controlPt.Y /= scaleValue;

            Zoom(new System.Windows.Point(translate.X, translate.Y), controlPt, scaleValue);
        }

        private void ImageThumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            translate.X += e.HorizontalChange;
            translate.Y += e.VerticalChange;
        }
        
        private void ImageThumb_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void ImageThumb_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Focus();
        }
    }
}
;