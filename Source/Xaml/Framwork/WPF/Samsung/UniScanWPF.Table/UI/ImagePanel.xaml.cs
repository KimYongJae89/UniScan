using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Operation.Operators;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// ImagePanel.xaml에 대한 상호 작용 논리
    /// </summary>
    public delegate void NotifyDelegate(ImagePanel sender, int index);
    public delegate void OnZoomChangedDelegate(ScaleTransform scaleTransform);
    public delegate void OnTranslateChangedDelegate(TranslateTransform translateTransform);

    public partial class ImagePanel : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public NotifyDelegate Notify;
        public OnZoomChangedDelegate OnZoomChanged;
        public OnTranslateChangedDelegate OnTranslateChanged;

        public bool ShowGuideLine
        {
            get => this.showGuideLine;
            set
            {
                if (this.showGuideLine != value)
                {
                    this.showGuideLine = value;
                    OnPropertyChanged("ShowGuideLine");
                }
            }
        }
        bool showGuideLine;

        int maxLoadItem;
        int curLoadItem;
        int minLoadItem;

        bool translatable = true;
        bool zoomable = true;
        bool onUpdate = false;


        System.Timers.Timer zoomStopTimer;
        TranslateTransform originTranslateTransform = new TranslateTransform();
        Canvas[] canvases;

        public ScanOperatorResult[] ScanOperatorResultArray { get => this.scanOperatorResultArray; }
        ScanOperatorResult[] scanOperatorResultArray = null;

        public ScaleTransform ScaleTransform
        {
            get => this.scaleTransform;
            set
            {
                this.scaleTransform.ScaleX = value.ScaleX;
                this.scaleTransform.ScaleY = value.ScaleY;
                UpdateZoom();
            }
        }

        public TranslateTransform TranslateTransform
        {
            get => this.translateTransform;
            set
            {
                translateTransform.X = value.X;
                translateTransform.Y = value.Y;
            }
        }

        public bool IsCanPrev { get => curLoadItem < 0 ? false : (curLoadItem > minLoadItem); }
        public bool IsCanNext { get => curLoadItem < 0 ? false : (curLoadItem < maxLoadItem); }
        public bool FigureVisible { get => FigureLayoutCanvas.Visibility == Visibility.Visible; set => FigureLayoutCanvas.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        public float ResizeRatio { get => 1 / UniScanWPF.Table.Operation.Operator.DispResizeRatio; }
        public Visibility Zoomable { get => zoomable ? Visibility.Visible : Visibility.Collapsed; }

        public int CurLoadItem
        {
            get => curLoadItem;
            set
            {
                if (curLoadItem != value)
                {
                    curLoadItem = value;
                    OnPropertyChanged("CurLoadItem");
                    OnPropertyChanged("IsCanPrev");
                    OnPropertyChanged("IsCanNext");
                }
            }
        }

        public ImagePanel(bool translatable, bool zoomable)
        {
            InitializeComponent();

            this.canvases = new Canvas[]
            {
                this.LumpLayoutCanvas,
                this.PatternLayoutCanvas,
                this.CircularPatternLayoutCanvas,
                this.MarginLayoutCanvas,
                this.CircularMarginLayoutCanvas,
                this.ShapeLayoutCanvas,
                this.MeanderLayoutCanvas,
                this.PLengthLayoutCanvas,
                this.MarginDistLayoutCanvas,
                this.SelectionLayoutCanvas
            };


            this.zoomStopTimer = new System.Timers.Timer()
            {
                AutoReset = false,
                Interval = 500
            };
            this.zoomStopTimer.Elapsed += new ElapsedEventHandler((s, e) => AdaptiveFigureScale(null));

            this.translatable = translatable;
            this.zoomable = zoomable;
            this.DataContext = this;
            BuildImageCanvas();
        }

        private void BuildImageCanvas()
        {
            this.scanOperatorResultArray = new ScanOperatorResult[DeveloperSettings.Instance.ScanNum];
            for (int i = 0; i < DeveloperSettings.Instance.ScanNum; i++)
            {
                this.scanOperatorResultArray[i] = new ScanOperatorResult(null, null);

                System.Windows.Controls.Image image = new System.Windows.Controls.Image();
                image.SetBinding(System.Windows.Controls.Image.SourceProperty, new Binding()
                {
                    Path = new PropertyPath(string.Format("ScanOperatorResultArray[{0}].TopLightBitmap", i)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                image.SetBinding(Canvas.LeftProperty, new Binding()
                {
                    Path = new PropertyPath(string.Format("ScanOperatorResultArray[{0}].CanvasAxisPosition.Position[0]", i)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                image.SetBinding(Canvas.TopProperty, new Binding()
                {
                    Path = new PropertyPath(string.Format("ScanOperatorResultArray[{0}].CanvasAxisPosition.Position[1]", i)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                image.SetBinding(Canvas.WidthProperty, new Binding()
                {
                    Path = new PropertyPath(string.Format("ScanOperatorResultArray[{0}].TopLightBitmap.PixelWidth", i)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                image.SetBinding(Canvas.HeightProperty, new Binding()
                {
                    Path = new PropertyPath(string.Format("ScanOperatorResultArray[{0}].TopLightBitmap.PixelHeight", i)),
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                });
                image.DataContext = this;
                //image.SetValue(Canvas.HeightProperty, "10000");
                imageCanvas.Children.Add(image);
            }
            OnPropertyChanged("ScanOperatorResultArray");

            RobotWorkingRectangle.DataContext = InfoBox.Instance;
            RobotWorkingRectangle.StrokeThickness = 100;
        }

        private void ResultCombiner_CombineCompleted(List<CanvasDefect> canvasDefectList)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                // test
                Array temp = null;
                lock (canvasDefectList)
                    temp = canvasDefectList.ToArray();

                foreach (CanvasDefect canvasDefect in temp)
                {
                    Shape shape = canvasDefect.GetShape();
                    AdaptiveFigureScale(shape);

                    //Rect rect = canvasDefect.GetRect();
                    //Debug.WriteLine(string.Format("InspectPage::AddCanvasDefect - {0}, {1}, {2}, {3}", rect.X, rect.Y, rect.Width, rect.Height));

                    Enum resObjType = canvasDefect.Defect.ResultObjectType;
                    switch (resObjType)
                    {
                        case MeasureType.Meander:
                        case MeasureType.Length:
                        case MeasureType.Extra:
                            this.MeanderLayoutCanvas.Children.Add(shape);
                            break;

                        case DefectType.Lump:
                            this.LumpLayoutCanvas.Children.Add(shape);
                            break;

                        case DefectType.Pattern:
                            this.PatternLayoutCanvas.Children.Add(shape);
                            break;
                        case DefectType.CircularPattern:
                            this.CircularPatternLayoutCanvas.Children.Add(shape);
                            break;

                        case DefectType.Margin:
                            this.MarginLayoutCanvas.Children.Add(shape);
                            break;
                        case DefectType.CircularMargin:
                            this.CircularMarginLayoutCanvas.Children.Add(shape);
                            break;

                        case DefectType.Shape:
                            this.ShapeLayoutCanvas.Children.Add(shape);
                            break;
                    }
                }
            }));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (translatable == false)
                return;

            translateTransform.X = this.originTranslateTransform.X + (e.HorizontalChange / scaleTransform.ScaleX);
            translateTransform.Y = this.originTranslateTransform.Y + (e.VerticalChange / scaleTransform.ScaleY);

            this.OnTranslateChanged?.Invoke(translateTransform);
        }

        private void Thumb_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (zoomable)
            {
                System.Windows.Point prevPt = e.GetPosition(this.mainCanvas);
                
                scaleTransform.ScaleX *= e.Delta > 0 ? 1.2 : 0.8;
                scaleTransform.ScaleY *= e.Delta > 0 ? 1.2 : 0.8;
                this.OnZoomChanged?.Invoke(scaleTransform);

                System.Windows.Point nextPt = e.GetPosition(this.mainCanvas);

                translateTransform.X += nextPt.X - prevPt.X;
                translateTransform.Y += nextPt.Y - prevPt.Y;
                this.OnTranslateChanged?.Invoke(translateTransform);

                UpdateZoom();
            }
        }

        internal void SetSelection(CanvasDefect selectedDefect)
        {
            if (selectedDefect == null)
                return;

            Rect rect = selectedDefect.GetRect();
            System.Drawing.PointF centerPt = new System.Drawing.PointF((float)(rect.Left + rect.Right) / 2.0f, (float)(rect.Top + rect.Bottom) / 2.0f);
            Point pt = ConvertToScreen(centerPt);
            if (translatable)
            {
                translateTransform.X = pt.X;
                translateTransform.Y = pt.Y;
                this.OnTranslateChanged?.Invoke(translateTransform);
            }

            Rect selectionRect = selectedDefect.GetRect(500);
            Polygon polygon = new Polygon()
            {
                Stroke = new SolidColorBrush(Colors.LightGreen),
                StrokeThickness = 3 / Math.Max(scaleTransform.ScaleX, scaleTransform.ScaleY),
                Opacity = 1,
                Points = new PointCollection(new Point[] { selectionRect.TopLeft, selectionRect.TopRight, selectionRect.BottomRight, selectionRect.BottomLeft })
            };
            this.SelectionLayoutCanvas.Children.Add(polygon);
        }

        internal void VisiblePatternCanvas(bool? visible)
        {
            if (visible.HasValue)
            {
                this.LumpLayoutCanvas.Visibility = Visibility.Collapsed;

                this.PatternLayoutCanvas.Visibility = visible.Value ? Visibility.Visible : Visibility.Collapsed;
                this.CircularPatternLayoutCanvas.Visibility = visible.Value ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                this.LumpLayoutCanvas.Visibility = Visibility.Visible;

                this.PatternLayoutCanvas.Visibility = Visibility.Collapsed;
                this.CircularPatternLayoutCanvas.Visibility = Visibility.Collapsed;
            }
        }

        internal void VisibleMarginCanvas(bool? visible)
        {
            if (visible.HasValue)
            {
                this.MarginLayoutCanvas.Visibility = visible.Value ? Visibility.Visible : Visibility.Collapsed;
                this.CircularMarginLayoutCanvas.Visibility = visible.Value ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                this.MarginLayoutCanvas.Visibility = Visibility.Visible;
                this.CircularMarginLayoutCanvas.Visibility = Visibility.Collapsed;
            }
        }

        internal void VisibleShapeCanvas(bool? visible)
        {
            this.ShapeLayoutCanvas.Visibility = visible.Value ? Visibility.Visible : Visibility.Collapsed;
        }

        internal void VisibleMeanderCanvas(bool? visible)
        {
            this.MeanderLayoutCanvas.Visibility = visible.Value ? Visibility.Visible : Visibility.Collapsed;
        }

        internal void VisibleMarginDistCanvas(bool? visible)
        {
            this.MarginDistLayoutCanvas.Visibility = visible.Value ? Visibility.Visible : Visibility.Collapsed;
        }
        internal void VisiblePLengthCanvas(bool? visible)
        {
            this.PLengthLayoutCanvas.Visibility = visible.Value ? Visibility.Visible : Visibility.Collapsed;
        }
        private Point ConvertToScreen(System.Drawing.PointF centerPt)
        {
            Point convert = new Point();
            //convert.X = -centerPt.X * DeveloperSettings.Instance.Resolution + (MainCanvas.ActualWidth / 2 / scaleX) + (30700);
            //convert.Y = -centerPt.Y * DeveloperSettings.Instance.Resolution + (MainCanvas.ActualHeight / 2 / scaleY);

            convert.X = -centerPt.X + (InfoBox.Instance.DispScanRegion.Width / 2) / (scaleTransform.ScaleX / (this.mainCanvas.ActualWidth / InfoBox.Instance.DispScanRegion.Width));
            convert.Y = -centerPt.Y + (InfoBox.Instance.DispScanRegion.Height / 2) / (scaleTransform.ScaleY / (this.mainCanvas.ActualHeight / InfoBox.Instance.DispScanRegion.Height));

            return convert;
        }

        internal void ClearSelection()
        {
            this.SelectionLayoutCanvas.Children.Clear();
        }

        private void MainCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //ZoomFit();
        }

        private void Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            this.originTranslateTransform = translateTransform.Clone();
        }

        private void ZoomIn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!zoomable)
                return;

            if (e.ButtonState == MouseButtonState.Pressed)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    e.Handled = true;
                    ZoomCenter(scaleTransform.ScaleX*1.2);
                }
            }
        }

        private void ZoomOut_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!zoomable)
                return;

            if (e.ButtonState == MouseButtonState.Pressed)
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    e.Handled = true;

                    ZoomCenter(scaleTransform.ScaleX * 0.8);
                }
            }
        }

        private void ZoomFit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!zoomable)
                return;

            if (e.ButtonState == MouseButtonState.Pressed)
            {
                e.Handled = true;

                if (e.ChangedButton == MouseButton.Left)
                {
                    ScaleTransform scaleTransform = UISettings.Instance().ScaleTransform;
                    if (this.scaleTransform.ScaleX != scaleTransform.ScaleX)
                    // 스케일이 안맞으면 설정된 스케일.
                    {
                        ZoomCenter(scaleTransform.ScaleX);
                    }
                    else
                    // 스케일이 맞으면 전체 스케일
                    {
                        ZoomFit();
                    }
                }
                else if (e.ChangedButton == MouseButton.Right)
                {
                    double scale = Math.Min(scaleTransform.ScaleX, scaleTransform.ScaleY);
                    string message = LocalizeHelper.GetString("Do you want save current zoom scale? ({0:F0}%)");
                    if (CustomMessageBox.Show(string.Format(message, scale * 100), null, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        UISettings.Instance().ScaleTransform.ScaleX = scale;
                        UISettings.Instance().ScaleTransform.ScaleY = scale;
                        UISettings.Instance().Save();
                    }
                }
            }
        }

        private void ZoomCenter(double newScale)
        {
            Point centerPt = new Point(this.mainCanvas.ActualWidth / 2, this.mainCanvas.ActualHeight / 2);

            System.Windows.Point prevPt = this.mainCanvas.RenderTransform.Inverse.Transform(centerPt);

            scaleTransform.ScaleX = scaleTransform.ScaleY = newScale;
            this.OnZoomChanged?.Invoke(scaleTransform);

            System.Windows.Point nextPt = this.mainCanvas.RenderTransform.Inverse.Transform(centerPt);
            translateTransform.X += nextPt.X - prevPt.X;
            translateTransform.Y += nextPt.Y - prevPt.Y;
            this.OnTranslateChanged?.Invoke(translateTransform);

            UpdateZoom();
        }

        public void ZoomFit()
        {
            double scaleX, scaleY, scale;
            scaleX = Math.Max(1, this.ActualWidth) / (InfoBox.Instance.DispRobotRegion.Width * 1.2);
            scaleY = Math.Max(1, this.ActualHeight) / (InfoBox.Instance.DispRobotRegion.Height * 1.2);
            scaleTransform.ScaleX = scaleTransform.ScaleY = scale = Math.Min(scaleX, scaleY);
            this.OnZoomChanged?.Invoke(scaleTransform);

            translateTransform.X = (this.ActualWidth - InfoBox.Instance.DispRobotRegion.Width * scale) / scale / 2;
            translateTransform.Y = (this.ActualHeight - InfoBox.Instance.DispRobotRegion.Height * scale) / scale / 2;
            this.OnTranslateChanged?.Invoke(translateTransform);

            UpdateZoom();
        }

        private void UpdateZoom()
        {
            if (Math.Min(scaleTransform.ScaleX, scaleTransform.ScaleY) > 0.2)
                RenderOptions.SetBitmapScalingMode(imageCanvas, BitmapScalingMode.HighQuality);
            else
                RenderOptions.SetBitmapScalingMode(imageCanvas, BitmapScalingMode.Unspecified);

            this.zoomStopTimer.Stop();
            this.zoomStopTimer.Start();
        }

        private void AdaptiveFigureScale(Shape specShape)
        {
            if (!this.IsVisible)
                return;

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (scaleTransform.ScaleX == 0 || scaleTransform.ScaleY == 0)
                    return;

                double thickness = 5 / Math.Max(scaleTransform.ScaleX, scaleTransform.ScaleY);
                this.RobotWorkingRectangle.StrokeThickness = thickness;

                if (specShape != null)
                {
                    specShape.StrokeThickness = thickness / 2;
                }
                else
                {
                    Array.ForEach(this.canvases, f =>
                    {
                            //if (f.Visibility == Visibility.Visible)
                            {
                            foreach (Shape shape in f.Children)
                                shape.StrokeThickness = thickness / 2;
                        }
                    });
                }
            }));
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentIndex(curLoadItem - 1);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            SetCurrentIndex(curLoadItem + 1);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            if (onUpdate == false)
                Notify?.Invoke(this, (int)e.AddedItems[0] - 1);
        }

        private void SetCurrentIndex(int index)
        {
            int newIndex = Math.Min(this.maxLoadItem, Math.Max(this.minLoadItem, index));
            this.CurLoadItem = index;
        }

        private void CheckResultIndex()
        {
            OnPropertyChanged("IsCanNext");
            OnPropertyChanged("IsCanPrev");
        }

        internal void ProductionChange(LoadItem loadItem)
        {
            Array.ForEach(this.canvases, f =>
            {
                f.Children.Clear();
            });

            int count = Math.Min(scanOperatorResultArray.Length, loadItem.ExtractOperatorResultList.Count);
            for (int i = 0; i < count; i++)
            {
                this.scanOperatorResultArray[i] = loadItem.ExtractOperatorResultList[i].ScanOperatorResult;
                //this.scanOperatorResultArray[i] = resultCombiner.ScanOperatorResultArray[i];
            }
            OnPropertyChanged("ScanOperatorResultArray");

            foreach (CanvasDefect canvasDefect in loadItem.CanvasDefectList)
            {
                Shape shape = canvasDefect.GetShape();
                shape.DataContext = this;

                switch (canvasDefect.Defect.ResultObjectType)
                {
                    case DefectType.Lump:
                        this.LumpLayoutCanvas.Children.Add(shape);
                        break;

                    case DefectType.Pattern:
                        this.PatternLayoutCanvas.Children.Add(shape);
                        break;
                    case DefectType.CircularPattern:
                        this.CircularPatternLayoutCanvas.Children.Add(shape);
                        break;

                    case DefectType.Margin:
                        this.MarginLayoutCanvas.Children.Add(shape);
                        break;
                    case DefectType.CircularMargin:
                        this.CircularMarginLayoutCanvas.Children.Add(shape);
                        break;

                    case DefectType.Shape:
                        this.ShapeLayoutCanvas.Children.Add(shape);
                        break;

                    case MeasureType.Meander:
                        this.MeanderLayoutCanvas.Children.Add(shape);
                        break;

                    case MeasureType.Length:
                        if (((LengthMeasure)canvasDefect.Defect).IsValid)
                            this.PLengthLayoutCanvas.Children.Add(shape);
                        break;

                    case MeasureType.Extra:
                        this.MarginDistLayoutCanvas.Children.Add(shape);
                        break;
                }
            }
            AdaptiveFigureScale(null);

            OnPropertyChanged("DefectList");
            OnPropertyChanged("");
        }

        internal void UpdateCombobox(Production production)
        {
            onUpdate = true;

            this.CurLoadItem = -1;

            List<int> list = new List<int>();
            for (int i = 0; i < production.Count; i++)
                list.Add(i + 1);

            comboBox.ItemsSource = list;

            if (list.Count > 0)
            {
                this.minLoadItem = list.Min();
                this.maxLoadItem = list.Max();
                this.CurLoadItem = minLoadItem;
            }

            onUpdate = false;
        }
    }

    class GuideLineConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value) / 2;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
