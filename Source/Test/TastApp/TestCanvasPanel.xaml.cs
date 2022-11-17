using DynMvp.Base;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace WpfControlLibrary.UI
{
    public abstract class TestTargetResult
    {
        List<BlobRect> blobRectList;
        public List<BlobRect> BlobRectList { get => blobRectList; set => blobRectList = value; }

        public BitmapSource BitmapImage
        {
            get
            {
                return GetBitmapImage();
            }
        }

        protected abstract BitmapSource GetBitmapImage();
        public abstract List<UIElement> GetFigureList();
    }

    /// <summary>
    /// WPFCanvasPanel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TestCanvasPanel : UserControl, INotifyPropertyChanged
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
        object targetResult;
        public object TargetResult
        {
            get { return targetResult; }
            set
            {
                targetResult = value;
                OnPropertyChanged("TargetResult");
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        double scaleX = 1;
        public double ScaleX
        {
            get => scaleX;
            set
            {
                scaleX = value;
                OnPropertyChanged("ScaleX");
            }
        }

        double scaleY = 1;
        public double ScaleY
        {
            get => scaleY;
            set
            {
                scaleY = value;
                OnPropertyChanged("ScaleY");
            }
        }

        double translateX;
        public double TranslateX
        {
            get => translateX;
            set
            {
                translateX = value;
                OnPropertyChanged("TranslateX");
            }
        }

        double translateY;
        public double TranslateY
        {
            get => translateY;
            set
            {
                translateY = value;
                OnPropertyChanged("TranslateY");
            }
        }

        double angle;
        public double Angle
        {
            get => angle;
            set
            {
                angle = value;
                OnPropertyChanged("Angle");
            }
        }

        bool lockTransform;

        public TestCanvasPanel(bool lockTransform = true)
        {
            InitializeComponent();

            this.DataContext = this;
            this.lockTransform = lockTransform;

            if (lockTransform == true)
                Menu.Visibility = Visibility.Collapsed;
            
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;

            //HwndTarget hwndTarget = hwndSource.CompositionTarget;
            //hwndTarget.RenderMode = RenderMode.;
        }


        private void OnPropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void ClearFigure()
        {
            workingFigures.Clear();
            backgroundFigures.Clear();
            tempFigures.Clear();
            selectionContainer.ClearSelection();
        }

        public void SetAddMode(FigureType trackerShape)
        {
            Cursor = Cursors.Cross;
            dragMode = DragMode.Add;
            this.trackerShape = trackerShape;
        }

        public void AddFigure(DynMvp.UI.Figure figure)
        {
            selectionContainer.AddFigure(figure);
        }

        public void SelectFigure(DynMvp.UI.Figure figure)
        {
            if (noneClickMode)
                return;

            selectionContainer.ClearSelection();
            selectionContainer.AddFigure(figure);
        }

        public void SelectFigure(List<DynMvp.UI.Figure> figureList)
        {
            if (noneClickMode)
                return;

            selectionContainer.ClearSelection();
            selectionContainer.AddFigure(figureList);
        }

        public void SelectFigureByTag(List<Object> tagList)
        {
            if (noneClickMode)
                return;

            foreach (Object tag in tagList)
                selectionContainer.AddFigure(workingFigures.GetFigureByTag(tag));
        }

        public void SelectFigureByTag(Object tag)
        {
            if (noneClickMode)
                return;
            selectionContainer.AddFigure(workingFigures.GetFigureByTag(tag));
        }

        public void DeleteSelection()
        {
            List<DynMvp.UI.Figure> figureList = selectionContainer.GetRealFigures();
            if (FigureDeleted != null)
                FigureDeleted(figureList);

            selectionContainer.ClearSelection();
        }

        public void ClearSelection()
        {
            selectionContainer.ClearSelection();
        }
        
        public void ZoomFit()
        {
            TestTargetResult targetResult = (TestTargetResult)this.targetResult;

            if (targetResult == null || targetResult.BitmapImage == null)
                return;

            double curScaleX = MainCanvas.ActualWidth / targetResult.BitmapImage.Width;
            double curScaleY = MainCanvas.ActualHeight / targetResult.BitmapImage.Height;

            double minScale = Math.Min(curScaleX, curScaleY);
            
            ScaleX = minScale;
            ScaleY = minScale;

            if (curScaleX > curScaleY)
            {
                TranslateX = ((MainCanvas.ActualWidth / scaleX) - targetResult.BitmapImage.Width) / 2.0;
                TranslateY = 0;
            }
            else
            {
                TranslateX = 0;
                TranslateY = ((MainCanvas.ActualHeight / scaleY) - targetResult.BitmapImage.Height) / 2.0;
            }
        }
        
        private void Zoom(System.Windows.Point zoomCenter, double scaleValue)
        {
            TranslateX -= (zoomCenter.X / ScaleX) - (zoomCenter.X / (ScaleX * scaleValue));
            TranslateY -= (zoomCenter.Y / ScaleY) - (zoomCenter.Y / (ScaleY * scaleValue));

            ScaleX *= scaleValue;
            ScaleY *= scaleValue;
        }

        public void ZoomIn()
        {
            Zoom(new System.Windows.Point(this.ActualWidth / 2.0, this.ActualHeight / 2.0), 1.2);
        }
        
        public void ZoomOut()
        {
            Zoom(new System.Windows.Point(this.ActualWidth / 2.0, this.ActualHeight / 2.0), 0.8);
        }
        
        private void ImageThumb_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (lockTransform == true)
                return;

            System.Windows.Point controlPt = e.GetPosition(this);
            
            if (e.Delta > 0)
                Zoom(controlPt, 1.1);
            else
                Zoom(controlPt, 0.9);
        }
        
        private void Image_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            canvas.Children.Clear();

            if (this.targetResult == null)
                return;
            
            TestTargetResult targetResult = (TestTargetResult)this.targetResult;

            if (targetResult.BlobRectList != null)
            {
                foreach (BlobRect blobRect in targetResult.BlobRectList)
                {
                    Polygon polygon = new Polygon();
                    //polygon.Focusable = true;
                    polygon.MouseEnter += Shape_MouseEnter;
                    polygon.MouseLeave += Shape_MouseLeave;
                    polygon.StrokeThickness = 5;
                    polygon.Stroke = new SolidColorBrush(Colors.Green);

                    for (int i = 0; i < 4; i++)
                        polygon.Points.Add(new System.Windows.Point(blobRect.RotateXArray[i], blobRect.RotateYArray[i]));

                    if (blobRect.MeasureData != null)
                    {
                        if ((float)blobRect.MeasureData[0] != 0)
                        {
                            PointF start = (PointF)blobRect.MeasureData[1];
                            PointF end = (PointF)blobRect.MeasureData[2];

                            Line line = new Line();
                            line.MouseEnter += Shape_MouseEnter;

                            line.MouseLeave += Shape_MouseLeave;
                            line.StrokeThickness = 5;
                            line.Stroke = new SolidColorBrush(Colors.Blue);

                            line.X1 = start.X;
                            line.Y1 = start.Y;

                            line.X2 = end.X;
                            line.Y2 = end.Y;

                            canvas.Children.Add(line);
                        }
                        else
                        {

                        }
                    }
                    
                    //TransformGroup group = new TransformGroup();
                    //group.Children.Add(new RotateTransform(blobRect.RotateRect.Angle));

                    //for (int i = transformGroup.Children.Count - 1; i >= 0; i--)
                    //    group.Children.Add(transformGroup.Children.ToArray()[i]);

                    //foreach (Transform transform in transformGroup.Children)
                    //    group.Children.Add(transform);

                    //rectangle.RenderTransform = new RotateTransform(-blobRect.RotateRect.Angle);

                    canvas.Children.Add(polygon);
                }
            }

            ZoomFit();
        }

        private void Shape_MouseLeave(object sender, MouseEventArgs e)
        {
            Shape shape = (Shape)sender;
            shape.StrokeDashArray = new DoubleCollection();
        }

        private void Shape_MouseEnter(object sender, MouseEventArgs e)
        {
            Shape shape = (Shape)sender;
            shape.StrokeDashArray = new DoubleCollection() { 5, 2 };
        }


        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (lockTransform == true)
                return;

            TranslateX = originTranslateX + (e.HorizontalChange / scaleX);
            TranslateY = originTranslateY + (e.VerticalChange / scaleY);
        }

        double originTranslateX;
        double originTranslateY;
        
        private void Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            if (lockTransform == true)
                return;

            originTranslateX = translateX;
            originTranslateY = translateY;
        }

        private void ZoomFitButton_Click(object sender, RoutedEventArgs e)
        {
            ZoomFit();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
        }
    }
}
