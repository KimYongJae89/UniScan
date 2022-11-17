using DynMvp.Base;
using DynMvp.Devices.MotionController;
using DynMvp.UI;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using UniScanWPF.Table.Data;
using UniScanWPF.Table.Inspect;
using UniScanWPF.Table.Operation;
using UniScanWPF.Table.Operation.Operators;
using UniScanWPF.Table.Settings;
using WpfControlLibrary.Helper;
using WpfControlLibrary.UI;

namespace UniScanWPF.Table.UI
{
    /// <summary>
    /// WPFCanvasPanel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InspectPage : UserControl, INotifyPropertyChanged, IMultiLanguageSupport
    {
        double canvasOpacity = 0.7;

        double originTranslateX;
        double originTranslateY;

        bool measureMarginMode = false;
        double originMeasureMarginX;
        double originMeasureMarginY;

        System.Timers.Timer zoomStopTimer;

        public string PatternCheckBoxText { get => LocalizeHelper.GetString(this.GetType().FullName, string.Format("Pattern({0})", !this.PatternCheckBox.IsChecked.HasValue? "Lump": this.PatternCheckBox.IsChecked.Value ? "Normal" : "Off")); }
        public string MarginCheckBoxText { get => LocalizeHelper.GetString(this.GetType().FullName, string.Format("Margin({0})", !this.MarginCheckBox.IsChecked.HasValue ? "Circle" : this.MarginCheckBox.IsChecked.Value ? "All" : "Off")); }
        public string ShapeCheckBoxText { get => LocalizeHelper.GetString(this.GetType().FullName, string.Format("Shape({0})", this.ShapeCheckBox.IsChecked.Value ? "All" : "Off")); }
        public List<ExtraMeasure> ExtraMeasureList => SystemManager.Instance().OperatorManager.ResultCombiner.ExtraMeasureList;

        List<Shape> shapeList = new List<Shape>();

        BitmapSource bigDefectBitmapSource = null;
        public BitmapSource BigDefectBitmapSource
        {
            get => bigDefectBitmapSource;
            set
            {
                if (this.bigDefectBitmapSource != value)
                {
                    this.bigDefectBitmapSource = value;
                    OnPropertyChanged("BigDefectBitmapSource");
                }
            }
        }

        double scale = 1;
        public double Scale
        {
            get => scale;
            set
            {
                if (scale != value)
                {
                    scale = value;
                    if (scale > 0)
                    {
                        OnPropertyChanged("Scale");
                        OnPropertyChanged("MarkSize");
                        OnPropertyChanged("HomeMarkPos");
                        OnPropertyChanged("MarkFontSize");
                    }
                }
            }
        }

        double translateX;
        public double TranslateX
        {
            get => translateX;
            set
            {
                if (translateX != value)
                {
                    translateX = value;
                    OnPropertyChanged("TranslateX");
                }
            }
        }

        double translateY;
        public double TranslateY
        {
            get => translateY;
            set
            {
                if (translateY != value)
                {
                    translateY = value;
                    OnPropertyChanged("TranslateY");
                }
            }
        }

        public System.Windows.Size MarkSize
        {
            get
            {
                double scale = this.scale / this.scale;
                double x = Math.Min(3500 * scale, 100.0 / this.scale);
                double y = Math.Min(3500 / scale, 100.0 / this.scale);
                return new System.Windows.Size(x, y);
            }
        }

        public double MarkFontSize { get => Math.Min(35791, Math.Min(MarkSize.Width, MarkSize.Height) / 2); }

        public System.Drawing.PointF HomeMarkPos
        {
            get
            {
                PointF homePos = InfoBox.Instance.DispHomePos;
                System.Windows.Size markSize = MarkSize;
                return new PointF((float)(homePos.X - markSize.Width / 2), (float)(homePos.Y - markSize.Height / 2));
            }
        }

        public System.Windows.Point CurMarkPos
        {
            get => new System.Windows.Point(
                HomeMarkPos.X - (SystemManager.Instance().MachineObserver.MotionBox.ActualPositionX / DeveloperSettings.Instance.Resolution),
                HomeMarkPos.Y + (SystemManager.Instance().MachineObserver.MotionBox.ActualPositionY / DeveloperSettings.Instance.Resolution)
                );
        }

        public SolidColorBrush CurMarkBrush { get => SystemManager.Instance().MachineObserver.MotionBox.OnMoveBrush; }

        public bool FigureVisible {
            get => FigureLayoutCanvas.Visibility == Visibility.Visible;
            set
            {
                FigureLayoutCanvas.Visibility = value ? Visibility.Visible : Visibility.Hidden;
                if(FigureLayoutCanvas.Visibility == Visibility.Visible)
                    AdaptiveFigureScale(null);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public InspectPage()
        {
            InitializeComponent();

            for (int i = 0; i < DeveloperSettings.Instance.ScanNum; i++)
            {
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
                image.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;
                //image.SetValue(Canvas.HeightProperty, "10000");

                ImageCanvas.Children.Add(image);
            }

            this.DataContext = this;

            this.CheckBoxGrid.DataContext = this;

            //this.imageCanvas.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;
            this.defectListBox.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;

            this.bgCanvas.DataContext = InfoBox.Instance;
            //this.ScanRegionLabel.DataContext = SystemManager.Instance().OperatorManager.ScanOperator;
            //this.RobotWorkingRectangle.DataContext = SystemManager.Instance().OperatorManager.ScanOperator;

            this.ImageCanvas.DataContext = InfoBox.Instance;
            //this.LightTuneMessage.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;
            //this.LightTuneImage.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;

            this.MenuPanel.DataContext = InfoBox.Instance;
            this.InfoGrid.DataContext = InfoBox.Instance;
            this.ScanOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.ScanOperator;
            this.ScanOperatorProgressBsr.DataContext = SystemManager.Instance().OperatorManager.ScanOperator;
            this.ExtractOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.ExtractOperator;
            this.ExtractOperatorProgressBsr.DataContext = SystemManager.Instance().OperatorManager.ExtractOperator;
            this.InspectOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.InspectOperator;
            this.InspectOperatorProgressBsr.DataContext = SystemManager.Instance().OperatorManager.InspectOperator;
            this.StoringOperatorLabel.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner.StoringOperator;
            this.StoringOperatorProgressBar.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner.StoringOperator;

            this.mesauredMarginGrid.DataContext = SystemManager.Instance().OperatorManager.ResultCombiner;

            SystemManager.Instance().OperatorManager.ResultCombiner.CombineCompleted += CombineCompleted;
            SystemManager.Instance().OperatorManager.ResultCombiner.CombineClear += CombineClear;
            LocalizeHelper.AddListener(this);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Tick += Tick;
            timer.Start();

            this.zoomStopTimer = new System.Timers.Timer()
            {
                AutoReset = false,
                Interval = 500
            };
            this.zoomStopTimer.Elapsed += new System.Timers.ElapsedEventHandler((o, e) => AdaptiveFigureScale(null));
        }

        private void Tick(object sender, EventArgs e)
        {
            OperatorManager operatorManager = SystemManager.Instance().OperatorManager;
            if (SystemManager.Instance().OperatorManager.IsRun)
            {
                loopModeCheckCount = 0;
                LoadingImage.Visibility = Visibility.Visible;
                rotation.Angle += 15;
            }
            else
            {
                LoadingImage.Visibility = Visibility.Collapsed;

                if (loopMode && operatorManager.ResultCombiner.StoringOperator.OperatorState == OperatorState.Idle)
                {
                    loopModeCheckCount++;
                    if (loopModeCheckCount == 10)
                        loopMode = SystemManager.Instance().OperatorManager.Start(false);
                }
            }

            OnPropertyChanged("CurMarkPos");
            OnPropertyChanged("CurMarkBrush");         
        }

        public void UpdateLanguage()
        {
            LocalizeHelper.UpdateString(this);
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
        
        private void ParamButton_Click(object sender, RoutedEventArgs e)
        {
            //PropertyWIndow propertyWIndow = new PropertyWIndow("Inspector", SystemManager.Instance().OperatorManager.InspectOperator.Settings);
            InspectParamWindow inspectParamWindow = new InspectParamWindow();
            inspectParamWindow.ShowDialog();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemManager.Instance().MachineObserver.IoBox.InOnDoor1 == false || SystemManager.Instance().MachineObserver.IoBox.InOnDoor2 == false)
            {
                string message = LocalizeHelper.GetString("Door is open !!");
                CustomMessageBox.Show(message, null, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Stop);
                return;
            }

            string msg = LocalizeHelper.GetString("Robot will move to home position.");
            if (CustomMessageBox.Show(msg, null, System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            MachineOperator.MoveHome(0, null, new CancellationTokenSource());
        }

        private void MainCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(defectListBox.ItemsSource).Filter = Filter;

            TranslateX = -InfoBox.Instance.DispRobotRegion.X + 10000.0;
            TranslateY = -InfoBox.Instance.DispRobotRegion.Y + 10000.0;

            double scaleX = Math.Max(1, mainCanvas.ActualWidth) / ((InfoBox.Instance.DispRobotRegion.Width) + 20000.0);
            double scaleY = Math.Max(1, mainCanvas.ActualHeight) / ((InfoBox.Instance.DispRobotRegion.Height) + 20000.0);
            this.Scale = Math.Min(scaleX, scaleY);

            ZoomUpdated();
        }

        public void Clear()
        {

        }
        
        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            if (measureMarginMode)
            {
                System.Windows.Point origin = new System.Windows.Point(originMeasureMarginX, originMeasureMarginY);
                System.Windows.Point current = new System.Windows.Point(originMeasureMarginX + e.HorizontalChange, originMeasureMarginY + e.VerticalChange);
                
                if (Math.Abs(origin.X - current.X) > Math.Abs(origin.Y - current.Y))
                {
                    Rect visualRect = new Rect(-Math.Min(origin.X, current.X), -origin.Y, 2500, 2500);//Math.Abs(origin.X - current.X), 200);
                    //Rect visualRect = new Rect(Math.Min(originMeasureMarginX, current.X), Math.Min(originMeasureMarginY, current.Y), Math.Abs(origin.X - current.X), 1);

                    DrawingVisual visual = new DrawingVisual();
                    using (var dc = visual.RenderOpen())
                    {
                        dc.DrawRectangle(new VisualBrush(mainCanvas), null, visualRect);
                    }

                    var bitmapSource = new RenderTargetBitmap((int)Math.Abs(origin.X - current.X), 100, 96, 96, PixelFormats.Default);
                    bitmapSource.Render(visual);

                    BitmapEncoder encoder = new BmpBitmapEncoder();
                    using (MemoryStream stream = new MemoryStream())
                    {
                        encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                        encoder.Save(stream);
                        byte[] byteArray = stream.ToArray();
                        AlgoImage algoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Color, (int)bitmapSource.Width, 100, ImageBandType.Luminance);

                        //byteArray.
                        algoImage.SetByte(byteArray);
                        
                        //StripeCheckerResult result =  StripeChecker.Check(algoImage, new PointF(0, 0), algoImage.Width, 100, 0);
                        //MeasureMarginLayoutCanvas.Children.Clear();

                        //foreach (var line in result.GetStripeLineList())
                        //    MeasureMarginLayoutCanvas.Children.Add(line);

                        algoImage.Dispose();
                    }
                }
                else
                {
                    //var bitmapSource = new RenderTargetBitmap(1, (int)Math.Abs(origin.Y - current.Y), 96, 96, PixelFormats.Indexed8);
                }

            }
            else
            {
                TranslateX = originTranslateX + (e.HorizontalChange / this.scale);
                TranslateY = originTranslateY + (e.VerticalChange / this.scale);
            }
        }
        
        private void Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            if (measureMarginMode)
            {
                originMeasureMarginX = e.HorizontalOffset;
                originMeasureMarginY = e.VerticalOffset;
            }
            else
            {
                originTranslateX = translateX;
                originTranslateY = translateY;
            }
        }
        
        private void Thumb_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            System.Windows.Point prevPt = e.GetPosition(mainCanvas);
            
            Scale *= e.Delta > 0 ? 1.2 : 0.8;

            System.Windows.Point nextPt = e.GetPosition(mainCanvas);

            TranslateX += nextPt.X - prevPt.X;
            TranslateY += nextPt.Y - prevPt.Y;

            ZoomUpdated();
        }

        private void ZoomIn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
            {
                e.Handled = true;

                double newScale= Scale * 1.2;
                ZoomCenter(newScale);

            }
        }

        private void ZoomOut_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
            {
                e.Handled = true;

                double newScale = Scale * 0.8;
                ZoomCenter(newScale);
            }
        }

        private void ZoomFit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                e.Handled = true;

                if (e.ChangedButton == MouseButton.Left)
                {
                    ScaleTransform scaleTransform = UISettings.Instance().ScaleTransform;
                    if (this.Scale != scaleTransform.ScaleX)
                    {
                        ZoomCenter(scaleTransform.ScaleX);
                    }
                    else
                    {
                        //TranslateX = InfoBox.Instance.DispScanRegion.Width * 0.05 + 2000;
                        //TranslateY = InfoBox.Instance.DispScanRegion.Height * 0.05;

                        //double scaleX = Math.Max(1, mainCanvas.ActualWidth) / (InfoBox.Instance.DispScanRegion.Width * 1.1);
                        //double scaleY = Math.Max(1, mainCanvas.ActualHeight) / ((InfoBox.Instance.DispScanRegion.Height * 1.1));
                        //Scale = Math.Min(scaleX, scaleY);
                        //ZoomUpdated();

                        ZoomFit();
                    }
                }
                else if (e.ChangedButton == MouseButton.Right)
                {
                    string message = LocalizeHelper.GetString("Do you want save current zoom scale? ({0:F0}%)");
                    if (CustomMessageBox.Show(string.Format(message, this.scale * 100), null, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        UISettings.Instance().ScaleTransform.ScaleX = this.scale;
                        UISettings.Instance().ScaleTransform.ScaleY = this.scale;
                        UISettings.Instance().Save();
                    }
                }
            }
        }

        private void ZoomCenter(double newScale)
        {
            System.Windows.Point centerPt = new System.Windows.Point(this.mainCanvas.ActualWidth / 2, this.mainCanvas.ActualHeight / 2);
            System.Windows.Point prevPt = this.mainCanvas.RenderTransform.Inverse.Transform(centerPt);

            Scale = newScale;

            System.Windows.Point nextPt = this.mainCanvas.RenderTransform.Inverse.Transform(centerPt);
            TranslateX += nextPt.X - prevPt.X;
            TranslateY += nextPt.Y - prevPt.Y;

            ZoomUpdated();
        }

        private void ZoomFit()
        {
            double scaleX = Math.Max(1, mainCanvas.ActualWidth) / ((InfoBox.Instance.DispRobotRegion.Width) * 1.1);
            double scaleY = Math.Max(1, mainCanvas.ActualHeight) / ((InfoBox.Instance.DispRobotRegion.Height) * 1.1);
            this.Scale = Math.Min(scaleX, scaleY);
            ZoomUpdated();

            TranslateX = (this.mainCanvas.ActualWidth - (InfoBox.Instance.DispRobotRegion.Width * scale)) / scale / 3 * 2;
            TranslateY = (this.mainCanvas.ActualHeight - (InfoBox.Instance.DispRobotRegion.Height * scale)) / scale / 2;
        }

        private void ZoomUpdated()
        {
            if (scale > 0.2)
                RenderOptions.SetBitmapScalingMode(ImageCanvas, BitmapScalingMode.HighQuality);
            else
                RenderOptions.SetBitmapScalingMode(ImageCanvas, BitmapScalingMode.Unspecified);

            this.zoomStopTimer.Stop();
            this.zoomStopTimer.Start();
        }

        private void DefectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SelectionLayoutCanvas.Children.Clear();
            if (defectListBox.SelectedItem == null)
                return;

            CanvasDefect selectedDefect = (CanvasDefect)defectListBox.SelectedItem;
            this.BigDefectBitmapSource = selectedDefect.Defect.GetBitmapSource();
            
            RectangleF dispScanRegion = InfoBox.Instance.DispScanRegion;
            Rect rect = selectedDefect.GetRect();
            PointF centerPt = new PointF((float)(rect.Left + rect.Right) / 2.0f, (float)(rect.Top + rect.Bottom) / 2.0f);

            this.TranslateX = -centerPt.X + (dispScanRegion.Width / 2) / (this.scale / (mainCanvas.ActualWidth / dispScanRegion.Width));
            this.TranslateY = -centerPt.Y + (dispScanRegion.Height / 2) / (this.scale / (mainCanvas.ActualHeight / dispScanRegion.Height));

            Rect selectionRect = selectedDefect.GetRect(500);
            Polygon polygon = new Polygon()
            {
                Stroke = new SolidColorBrush(Colors.LightGreen),
                StrokeThickness = 3 / this.scale,
                Opacity = 1,
                Points = new PointCollection(new System.Windows.Point[] { selectionRect.TopLeft, selectionRect.TopRight, selectionRect.BottomRight, selectionRect.BottomLeft })
            };
            this.SelectionLayoutCanvas.Children.Add(polygon);
        }

        public void CombineCompleted(List<CanvasDefect> canvasDefectList)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Array temp = null;
                lock (canvasDefectList)
                    temp = canvasDefectList.ToArray();

                foreach (CanvasDefect canvasDefect in temp)
                {
                    Shape shape = canvasDefect.GetShape();
                    AdaptiveFigureScale(shape);

                    //Rect rect = canvasDefect.GetRect();
                    //Debug.WriteLine(string.Format("InspectPage::AddCanvasDefect - {0}, {1}, {2}, {3}", rect.X, rect.Y, rect.Width, rect.Height));
                    shape.MouseEnter += Shape_MouseEnter;
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

                    this.shapeList.Add(shape);
                }
            }));
        }

        private void Shape_MouseEnter(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AdaptiveFigureScale(Shape specShape)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (FigureLayoutCanvas.Visibility != Visibility.Visible)
                    return;

                if (scale <= 0)
                    return;

                double thickness = 5 / scale;

                RobotWorkingRectangle.StrokeThickness = thickness;
                if (specShape != null)
                {
                    specShape.StrokeThickness = thickness / 2;
                }
                else
                {
                    Canvas[] canvas = new Canvas[]
                    {
                        this.MeanderLayoutCanvas,
                        this.LumpLayoutCanvas,
                        this.PatternLayoutCanvas,
                        this.CircularPatternLayoutCanvas,
                        this.MarginLayoutCanvas,
                        this.CircularMarginLayoutCanvas,
                        this.ShapeLayoutCanvas,
                    };

                    Array.ForEach(canvas, f =>
                    {
                        if (f.Visibility == Visibility.Visible)
                        {
                            foreach (Shape shape in f.Children)
                                shape.StrokeThickness = thickness / 2;
                        }
                    });

                    //if (this.MeanderLayoutCanvas.Visibility == Visibility.Visible)
                    //{
                    //    foreach (Shape shape in this.MeanderLayoutCanvas.Children)
                    //        shape.StrokeThickness = thickness / 2;
                    //}

                    //if (this.LumpLayoutCanvas.Visibility == Visibility.Visible)
                    //{
                    //    foreach (Shape shape in this.LumpLayoutCanvas.Children)
                    //        shape.StrokeThickness = thickness / 2;
                    //}

                    //if (this.PatternLayoutCanvas.Visibility == Visibility.Visible)
                    //{
                    //    foreach (Shape shape in this.PatternLayoutCanvas.Children)
                    //        shape.StrokeThickness = thickness / 2;
                    //}

                    //if (this.CircularPatternLayoutCanvas.Visibility == Visibility.Visible)
                    //{
                    //    foreach (Shape shape in this.CircularPatternLayoutCanvas.Children)
                    //        shape.StrokeThickness = thickness / 2;
                    //}

                    //if (this.MarginLayoutCanvas.Visibility == Visibility.Visible)
                    //{
                    //    foreach (Shape shape in this.MarginLayoutCanvas.Children)
                    //        shape.StrokeThickness = thickness / 2;
                    //}

                    //if (this.CircularMarginLayoutCanvas.Visibility == Visibility.Visible)
                    //{
                    //    foreach (Shape shape in this.CircularMarginLayoutCanvas.Children)
                    //        shape.StrokeThickness = thickness / 2;
                    //}

                    //if (this.ShapeLayoutCanvas.Visibility == Visibility.Visible)
                    //{
                    //    foreach (Shape shape in this.ShapeLayoutCanvas.Children)
                    //        shape.StrokeThickness = thickness / 2;
                    //}
                }
            }));
        }

        private void CombineClear()
        {
            this.BigDefectBitmapSource = null;

            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                this.shapeList.Clear();

                this.LumpLayoutCanvas.Children.Clear();
                this.PatternLayoutCanvas.Children.Clear();
                this.CircularPatternLayoutCanvas.Children.Clear();

                this.MarginLayoutCanvas.Children.Clear();
                this.CircularMarginLayoutCanvas.Children.Clear();

                this.ShapeLayoutCanvas.Children.Clear();
                this.MeanderLayoutCanvas.Children.Clear();
            }));
        }
        
        private void MeasureMargin_Checked(object sender, RoutedEventArgs e)
        {
            measureMarginMode = true;
        }

        private void MeasureMargin_Unchecked(object sender, RoutedEventArgs e)
        {
            measureMarginMode = false;
        }

        private bool Filter(object item)
        {
            Enum type = ((CanvasDefect)item).Defect.ResultObjectType;
            
            switch (type)
            {
                case DefectType.Lump:
                    return !PatternCheckBox.IsChecked.HasValue;

                case DefectType.Pattern:
                case DefectType.CircularPattern:
                    return PatternCheckBox.IsChecked.HasValue && PatternCheckBox.IsChecked.Value;
                    //return !PatternCheckBox.IsChecked.HasValue;

                case DefectType.Margin:
                    return MarginCheckBox.IsChecked.HasValue && MarginCheckBox.IsChecked.Value;
                case DefectType.CircularMargin:
                    return !MarginCheckBox.IsChecked.HasValue;

                case DefectType.Shape:
                    return ShapeCheckBox.IsChecked.Value;

                default:
                    return false;
            }
           
            return true;
        }

        private void PatternCheckBox_Click(object sender, RoutedEventArgs e)
        {
            bool? isChecked = PatternCheckBox.IsChecked;
            if (isChecked.HasValue == false)
            {
                SetCanvasVisible(this.LumpLayoutCanvas, true);

                SetCanvasVisible(this.PatternLayoutCanvas, false);
                SetCanvasVisible(this.CircularPatternLayoutCanvas, false);

                //this.LumpLayoutCanvas.Opacity = canvasOpacity;

                //this.PatternLayoutCanvas.Opacity = 0;
                //this.CircularPatternLayoutCanvas.Opacity = 0;
            }
            else
            {
                SetCanvasVisible(this.LumpLayoutCanvas, false);

                SetCanvasVisible(this.PatternLayoutCanvas, isChecked.Value);
                SetCanvasVisible(this.CircularPatternLayoutCanvas, isChecked.Value);

                //this.LumpLayoutCanvas.Opacity = 0;

                //this.PatternLayoutCanvas.Opacity = isChecked.Value ? canvasOpacity : 0;
                //this.CircularPatternLayoutCanvas.Opacity = isChecked.Value ? canvasOpacity : 0;
            }

            OnPropertyChanged("PatternCheckBoxText");
            CollectionViewSource.GetDefaultView(defectListBox.ItemsSource).Refresh();

        }

        private void MarginCheckBox_Click(object sender, RoutedEventArgs e)
        {
            bool? isChecked = MarginCheckBox.IsChecked;
            if (isChecked.HasValue == false)
            {
                SetCanvasVisible(this.MarginLayoutCanvas, false);
                SetCanvasVisible(this.CircularMarginLayoutCanvas, true);
                //this.MarginLayoutCanvas.Opacity = 0;
                //this.CircularMarginLayoutCanvas.Opacity = canvasOpacity;
            }
            else
            {
                SetCanvasVisible(this.MarginLayoutCanvas, isChecked.Value);
                SetCanvasVisible(this.CircularMarginLayoutCanvas, isChecked.Value);

                //this.MarginLayoutCanvas.Opacity = isChecked.Value ? canvasOpacity : 0;
                //this.CircularMarginLayoutCanvas.Opacity = isChecked.Value ? canvasOpacity : 0;
            }

            OnPropertyChanged("MarginCheckBoxText");
            CollectionViewSource.GetDefaultView(defectListBox.ItemsSource).Refresh();

        }

        private void ShapeCheckBox_Click(object sender, RoutedEventArgs e)
        {
            bool? isChecked = ShapeCheckBox.IsChecked;
            SetCanvasVisible(this.ShapeLayoutCanvas, isChecked.HasValue && isChecked.Value);
            //this.ShapeLayoutCanvas.Opacity = isChecked.HasValue && isChecked.Value ? canvasOpacity : 0.0;
            OnPropertyChanged("ShapeCheckBoxText");
            CollectionViewSource.GetDefaultView(defectListBox.ItemsSource).Refresh();
        }

        public void SetCanvasVisible(Canvas canvas,bool visible )
        {
            canvas.Opacity = visible ? canvasOpacity : 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            this.loopMode = false;
            SystemManager.Instance().OperatorManager.Cancle(LocalizeHelper.GetString("Operation Cancled"));
        }

        private void TeachButton_Click(object sender, RoutedEventArgs e)
        {

        }

        bool loopMode = false;
        int loopModeCheckCount = 0;
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (SystemManager.Instance().CurrentModel == null)
                return;

            DynMvp.Data.ProductionBase production = SystemManager.Instance().ProductionManager.CurProduction;
            if (production == null)
                production = SystemManager.Instance().ProductionManager.GetLastProduction(SystemManager.Instance().CurrentModel);

            string prev = production?.LotNo;
#if DEBUG
            prev = DateTime.Now.ToString("HHmm");
#endif

            Tuple<MessageBoxResult, string> result = CustomInputForm.Show(LocalizeHelper.GetString("Enter Lot No."), "Lot No.", MessageBoxImage.Question, prev);
            if (result.Item1 == MessageBoxResult.OK)
            {
                string lotNo = result.Item2;
                if(lotNo.Contains("LOOPTEST"))
                {
                    MessageBoxResult messageBoxResult = CustomMessageBox.Show(LocalizeHelper.GetString("Is Loop Test Mode?"), "Select Mode", MessageBoxButton.YesNo);
                    loopMode = messageBoxResult == MessageBoxResult.Yes;
                }
                SystemManager.Instance().ProductionManager.LotChange(SystemManager.Instance().CurrentModel, lotNo);
                loopModeCheckCount = 0;

                SystemManager.Instance().OperatorManager.Start(false);
            }
        }
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.PatternCheckBox.Foreground = Defect.GetBrush(DefectType.Pattern);
            this.MarginCheckBox.Foreground = Defect.GetBrush(DefectType.Margin);
            this.ShapeCheckBox.Foreground = Defect.GetBrush(DefectType.Shape);

            AdaptiveFigureScale(null);
            ZoomFit();
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                OnPropertyChanged("HomeMarkPos");
                OnPropertyChanged("CurMarkPos");
            }

        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {
                if (this.bigDefectBitmapSource != null)
                {
                    Directory.CreateDirectory(@"C:\temp");
                    Helper.WPFImageHelper.SaveBitmapSource(@"C:\temp\UniScanWpfTemp.bmp", this.bigDefectBitmapSource);
                    Process.Start(@"C:\temp\UniScanWpfTemp.bmp");
                }
            }
        }

        private void FigureLayoutCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Point point = e.GetPosition(mainCanvas);
            //this.PatternLayoutCanvas.IsMouseDirectlyOver
            //Shape shape = this.shapeList.Find(f => f.IsMouseOver);
            //if (shape != null)
            //{

            //}

        }
    }
}
